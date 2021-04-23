using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using DB.Vote;
using Vote.Controls;
using static System.String;

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
      var relatedJurisdictions =
        OfficeControl.CreateRelatedJurisdictionsNodes("/admin/updateElections.aspx",
          "addcandidates", StateCode, CountyCode, LocalKey);
      return OfficeControl.PopulateOfficeTree(table, OfficeControl.OfficeTree, StateCode,
        false, false, true, AdminPageLevel == AdminPageLevel.State, relatedJurisdictions);
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
          AddCandidatesReloading.Value = Empty;
          OfficeControl.OfficeKey = Empty; // experimental 16/02/16
          SetElectionHeading(HeadingAddCandidates);
          HeadingAddCandidatesOffice.InnerHtml = "No office selected";

          var table =
            Elections.GetAvailableElectionOfficeData(GetElectionKey(), StateCode,
              CountyCode, LocalKey); //.Rows.Cast<DataRow>().ToList();
          var officeCount = 0;
          if (table.Count == 0)
          {
            _ManagePoliticiansPanel.Message.RemoveCssClass("hidden");
            _ManagePoliticiansPanel.Message.InnerText =
              "No offices were found for this election";
          }
          else
          {
            _ManagePoliticiansPanel.Message.AddCssClasses("hidden");
            officeCount = PopulateOfficeTree(table);
            OfficeControl.ShowSelectOfficePanel = true;
            OfficeControl.ExpandedNode = Empty;
            OfficeControl.Update();
          }
          FeedbackAddCandidates.AddInfo($"{officeCount} offices loaded.");
        }
          break;

        case "loadoffice":
        {
          AddCandidatesReloading.Value = Empty;
          OfficeControl.ShowSelectOfficePanel = false;
          _ManagePoliticiansPanel.LoadControls();
          SetElectionHeading(HeadingAddCandidates);
          HeadingAddCandidatesOffice.InnerText =
            Offices.FormatOfficeName(OfficeControl.OfficeKey);
          _ManagePoliticiansPanel.ClearAddNewCandidate(true);
        }
          break;

        case "":
        {
          // normal update
          // We may need to actualize the election,  the electionOffice, and the office
          Elections.ActualizeElection(GetElectionKey());
          OfficeControl.OfficeKey =
            Offices.ActualizeOffice(OfficeControl.OfficeKey, CountyCode, LocalKey);
          ActualizeElectionOffice(GetElectionKey(), OfficeControl.OfficeKey);

          _ManagePoliticiansPanel.Update();
          _ManagePoliticiansPanel.ClearAddNewCandidate();
          // to update candidate counts...
          var table =
            Elections.GetAvailableElectionOfficeData(GetElectionKey(), StateCode,
              CountyCode, LocalKey); //.Rows.Cast<DataRow>().ToList();
          // In case counts changed
          PopulateOfficeTree(table);
          OfficeControl.ShowSelectOfficePanel = false;
          OfficeControl.Update();
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{AddCandidatesReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}