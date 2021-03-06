using System;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    // ReSharper disable once ClassNeverInstantiated.Local
    private class VoterUrlsTabItem : StateJurisdictionsDataItem
    {
      private const string GroupName = "VoterUrls";

      private VoterUrlsTabItem(UpdateJurisdictionsPage page)
        : base(page, GroupName)
      {
      }

      private static VoterUrlsTabItem[] GetTabInfo(
        UpdateJurisdictionsPage page)
      {
        var voterUrlsTabInfo = new[]
        {
          new VoterUrlsTabItem(page)
          {
            Column = "VoterRegistrationWebAddress",
            Description = "Voter Registration URL",
            LabelIsHyperLink = true,
            Validator = ValidateWebAddress
          },
          new VoterUrlsTabItem(page)
          {
            Column = "EarlyVotingWebAddress",
            Description = "Early Voting URL",
            LabelIsHyperLink = true,
            Validator = ValidateWebAddress
          },
          new VoterUrlsTabItem(page)
          {
            Column = "VoteByMailWebAddress",
            Description = "Vote by Mail URL",
            LabelIsHyperLink = true,
            Validator = ValidateWebAddress
          },
          new VoterUrlsTabItem(page)
          {
            Column = "VoteByAbsenteeBallotWebAddress",
            Description = "Vote by Absentee Ballot URL",
            LabelIsHyperLink = true,
            Validator = ValidateWebAddress
          },
          new VoterUrlsTabItem(page)
          {
            Column = "PollHoursUrl",
            Description = "Polling Hours URL",
            LabelIsHyperLink = true,
            Validator = ValidateWebAddress
          },
          new VoterUrlsTabItem(page)
          {
            Column = "PollPlacesUrl",
            Description = "Polling Places URL",
            LabelIsHyperLink = true,
            Validator = ValidateWebAddress
          }
        };

        foreach (var item in voterUrlsTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return voterUrlsTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._VoterUrlsTabInfo = GetTabInfo(page);
        if (page.AdminPageLevel != AdminPageLevel.State)
          page.TabVoterUrls.Visible = false;
      }
    }

    private VoterUrlsTabItem[] _VoterUrlsTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonVoterUrls_OnClick(object sender, EventArgs e)
    {
      switch (VoterUrlsReloading.Value)
      {
        case "reloading":
        {
          VoterUrlsReloading.Value = Empty;
          _VoterUrlsTabInfo.LoadControls();
          FeedbackVoterUrls.AddInfo("State voter urls loaded.");
        }
          break;

        case "":
        {
          // normal update
          _VoterUrlsTabInfo.ClearValidationErrors();
          _VoterUrlsTabInfo.Update(FeedbackVoterUrls);
        }
          break;

        default:
          throw new VoteException($"Unknown reloading option: '{VoterUrlsReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}