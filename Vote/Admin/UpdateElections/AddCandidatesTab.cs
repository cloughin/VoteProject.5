using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using DB.Vote;
using Vote.Controls;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    // ReSharper disable once UnusedMember.Local
    // Invoked via Reflection
    private class AddCandidatesTabItem : DataItemBase
    {
      // The rest of this is in the ManagePoliticiansPanel control
      // ReSharper disable once UnusedMember.Local
      internal static void Initialize(TemplateControl page)
      {
        InitializeGroup(page, "SelectOffice");
        InitializeGroup(page, "AddCandidates");
      }
    }

    #endregion DataItem object

    private int PopulateOfficeTree(IList<DataRow> table)
    {
      var relatedJurisdictions = OfficeControl.CreateRelatedJurisdictionsNodes(
        "/admin/updateElections.aspx", "addcandidates", StateCode, CountyCode, LocalCode);
      return OfficeControl.PopulateOfficeTree(table,
        OfficeControl.OfficeTree, StateCode, false, false, true,
        AdminPageLevel == AdminPageLevel.State, relatedJurisdictions);
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonAddCandidates_OnClick(object sender, EventArgs e)
    {
      switch (AddCandidatesReloading.Value)
      {
        case "reloading":
        {
          // This option just loads the office tree
          AddCandidatesReloading.Value = string.Empty;
          OfficeControl.OfficeKey = string.Empty; // experimental 16/02/16
          SetElectionHeading(HeadingAddCandidates);
          HeadingAddCandidatesOffice.InnerHtml = "No office selected";

          var table = Elections.GetActiveElectionOfficeData(GetElectionKey(),
            StateCode, CountyCode, LocalCode); //.Rows.Cast<DataRow>().ToList();
          var officeCount = 0;
          if (table.Count == 0)
          {
            _ManagePoliticialsPanel.Message.RemoveCssClass("hidden");
            _ManagePoliticialsPanel.Message.InnerText =
              "No offices were found for this election";
          }
          else
          {
            _ManagePoliticialsPanel.Message.AddCssClasses("hidden");
            officeCount = PopulateOfficeTree(table);
            OfficeControl.ShowSelectOfficePanel = true;
            OfficeControl.ExpandedNode = string.Empty;
            OfficeControl.Update();
          }
          FeedbackAddCandidates.AddInfo($"{officeCount} offices loaded.");
        }
          break;

        case "loadoffice":
        {
          AddCandidatesReloading.Value = string.Empty;
          OfficeControl.ShowSelectOfficePanel = false;
          _ManagePoliticialsPanel.LoadControls();
          SetElectionHeading(HeadingAddCandidates);
          HeadingAddCandidatesOffice.InnerText = Offices.FormatOfficeName(OfficeControl.OfficeKey);
          _ManagePoliticialsPanel.ClearAddNewCandidate(true);
        }
          break;

        case "":
        {
          // normal update

          // If this is a virtual office key, we may need to actualize the election,
          // the electionOffice, and the office
          if (Offices.IsVirtualKey(OfficeControl.OfficeKey))
          {
            Elections.ActualizeElection(GetElectionKey());
            OfficeControl.OfficeKey = Offices.ActualizeOffice(OfficeControl.OfficeKey, CountyCode,
              LocalCode);
            ActualizeElectionOffice(GetElectionKey(), OfficeControl.OfficeKey);
          }

          _ManagePoliticialsPanel.Update();
          _ManagePoliticialsPanel.ClearAddNewCandidate();
          // to update candidate counts...
          var table = Elections.GetActiveElectionOfficeData(GetElectionKey(),
            StateCode, CountyCode, LocalCode); //.Rows.Cast<DataRow>().ToList();
          // In case counts changed
          PopulateOfficeTree(table);
          OfficeControl.ShowSelectOfficePanel = false;
          OfficeControl.Update();
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}