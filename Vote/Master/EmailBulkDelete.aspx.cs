using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using DB.Vote;

namespace Vote.Master
{
  public partial class EmailBulkDeletePage : SecurePage, ISuperUser
  {
    private List<string> _Messages;

    private bool AnalyzeCount(int count, string address, string desc)
    {
      if (count > 0)
      {
        var ct = count > 1 ? $" ({count})" : string.Empty;
        _Messages.Add($"<p>{address} found in {desc}{ct}</p>");
      }
      return count != 0;
    }

    private void AnalyzeDeletions(string address)
    {
      var found = false;
      found |= AnalyzeCount(States.CountByEmail(address), address, "States.Email");
      found |= AnalyzeCount(States.CountByAltEmail(address), address, "States.AltEmail");
      found |= AnalyzeCount(Counties.CountByEmail(address), address, "Counties.Email");
      found |= AnalyzeCount(Counties.CountByAltEmail(address), address, "Counties.AltEmail");
      found |= AnalyzeCount(LocalDistricts.CountByEmail(address), address, "LocalDistricts.Email");
      found |= AnalyzeCount(LocalDistricts.CountByAltEmail(address), address,
        "LocalDistricts.AltEmail");
      found |= AnalyzeCount(Politicians.CountByEmail(address), address, "Politicians.Email");
      found |= AnalyzeCount(Politicians.CountByCampaignEmail(address), address,
        "Politicians.CampaignEmail");
      found |= AnalyzeCount(Politicians.CountByEmailVoteUSA(address), address,
        "Politicians.EmailVoteUSA");
      found |= AnalyzeCount(Politicians.CountByStateEmail(address), address,
        "Politicians.StateEmail");
      //found |= AnalyzeCount(Politicians.CountByLDSEmail(address), address, "Politicians.LDSEmail");
      found |= AnalyzeCount(Addresses.EmailExists(address) ? 1 : 0, address, "Addresses.Email");
      found |= AnalyzeCount(PartiesEmails.PartyEmailExists(address) ? 1 : 0, address,
        "PartiesEmails.PartyEmail");
      if (!found)
        _Messages.Add($"<p class=\"error\">{address} not found</p>");
    }

    private bool DeletionCount(int count, string address, string desc)
    {
      if (count > 0)
      {
        var ct = count > 1 ? $" ({count})" : string.Empty;
        _Messages.Add($"<p>{address} deleted from {desc}{ct}</p>");
      }
      return count != 0;
    }

    private void DoDeletions(string address)
    {
      var found = false;
      found |= DeletionCount(States.UpdateEmailByEmail(string.Empty, address), address,
        "States.Email");
      found |= DeletionCount(States.UpdateAltEmailByAltEmail(string.Empty, address), address,
        "States.AltEmail");
      found |= DeletionCount(Counties.UpdateEmailByEmail(string.Empty, address), address,
        "Counties.Email");
      found |= DeletionCount(Counties.UpdateAltEmailByAltEmail(string.Empty, address), address,
        "Counties.AltEmail");
      found |= DeletionCount(LocalDistricts.UpdateEmailByEmail(string.Empty, address), address,
        "LocalDistricts.Email");
      found |= DeletionCount(LocalDistricts.UpdateAltEmailByAltEmail(string.Empty, address), address,
        "LocalDistricts.AltEmail");
      found |= DeletionCount(Politicians.UpdateEmailByEmail(null, address), address,
        "Politicians.Email");
      found |= DeletionCount(Politicians.UpdateCampaignEmailByCampaignEmail(string.Empty, address),
        address, "Politicians.CampaignEmail");
      found |= DeletionCount(Politicians.UpdateEmailVoteUSAByEmailVoteUSA(string.Empty, address),
        address, "Politicians.EmailVoteUSA");
      found |= DeletionCount(Politicians.UpdateStateEmailByStateEmail(string.Empty, address),
        address, "Politicians.StateEmail");
      //found |= DeletionCount(Politicians.UpdateLDSEmailByLDSEmail(string.Empty, address), address, "Politicians.LDSEmail");
      //for Addresses we actually delete
      if (Addresses.EmailExists(address))
      {
        Addresses.DeleteByEmail(address);
        found |= DeletionCount(1, address, "Addresses");
      }
      //found |= DeletionCount(Addresses.UpdateEmailByEmail(string.Empty, address), address,
      //  "Addresses.Email");
      found |= DeletionCount(PartiesEmails.DeleteByPartyEmail(address), address,
        "PartiesEmails.PartyEmail");
      if (!found)
        _Messages.Add($"<p class=\"error\">{address} not found</p>");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Bulk Email Delete";
        H1.InnerHtml = "Bulk Email Delete";
        Master.MasterForm.Attributes.Add("enctype", "multipart/form-data");
      }
      else
      {
        _Messages = new List<string>();
        for (var i = 0; i < Request.Files.Count; i++)
        {
          var file = Request.Files[i];
          if (file.ContentLength == 0) continue;
          try
          {
            var address =
              EmailUtility.ExtractUndeliverableEmailAddressFromMsg(file.InputStream);
            if (DeleteCheckBox.Checked) DoDeletions(address);
            else AnalyzeDeletions(address);
          }
          catch (Exception ex)
          {
            _Messages.Add(
              $"<p class=\"error\">Error processing file: {file.FileName}, {ex.Message}</p>");
          }
        }
        foreach (var address in ExtraAddresses.Text.Split(new[] {'\n'},
            StringSplitOptions.RemoveEmptyEntries)
          .Select(a => a.Trim()))
        {
          if (DeleteCheckBox.Checked) DoDeletions(address);
          else AnalyzeDeletions(address);
        }
        SummaryPlaceHolder.Controls.Add(new LiteralControl(string.Join(string.Empty, _Messages)));
        SummaryContainer.RemoveCssClass("hidden");
      }
    }
  }
}