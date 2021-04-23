using System;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class BallotTabItem : StateJurisdictionsDataItem
    {
      private const string GroupName = "Ballot";

      private BallotTabItem(UpdateJurisdictionsPage page)
        : base(page, GroupName)
      {
      }

      private static BallotTabItem[] GetTabInfo(
        UpdateJurisdictionsPage page)
      {
        var ballotTabInfo = new[]
        {
          new BallotTabItem(page)
          {
            Column = "EncloseNickname",
            Description = "How Nicknames are Enclosed"
          },
          new BallotTabItem(page)
          {
            Column = "BallotStateName",
            Description = "State Name on Ballots"
          },
          new BallotTabItem(page)
          {
            Column = "IsIncumbentShownOnBallots",
            Description = "Show Incumbent",
            ConvertFn = ToBool
          },
          new BallotTabItem(page)
          {
            Column = "ShowUnopposed",
            Description = "Show Contests with Unopposed Candidates",
            ConvertFn = ToBool
          },
          new BallotTabItem(page)
          {
            Column = "ShowWriteIn",
            Description = "Show a Write-in Line for Each Non-Primary Contest",
            ConvertFn = ToBool
          },
          new BallotTabItem(page)
          {
            Column = "ShowPrimaryWriteIn",
            Description = "Show a Write-in Line for Each Primary Contest",
            ConvertFn = ToBool
          },
          new BallotTabItem(page)
          {
            Column = "ElectionAdditionalInfo",
            Description = "Additional Election Information"
          },
          new BallotTabItem(page)
          {
            Column = "BallotInstructions",
            Description = "Special Ballot Instructions"
          }
        };

        foreach (var item in ballotTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return ballotTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._BallotTabInfo = GetTabInfo(page);
        if (page.AdminPageLevel != AdminPageLevel.State)
        {
          page.TabBallot.Visible = false;
          page.TabViewReports.Visible = false;
        }
      }
    }

    private BallotTabItem[] _BallotTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonBallot_OnClick(object sender, EventArgs e)
    {
      switch (BallotReloading.Value)
      {
        case "reloading":
        {
          BallotReloading.Value = string.Empty;
          _BallotTabInfo.LoadControls();
          FeedbackBallot.AddInfo("Ballot settings loaded.");
        }
          break;

        case "":
        {
          // normal update
          _BallotTabInfo.ClearValidationErrors();
          _BallotTabInfo.Update(FeedbackBallot);
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}