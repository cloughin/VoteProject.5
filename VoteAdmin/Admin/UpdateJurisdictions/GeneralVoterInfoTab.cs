using System;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class GeneralVoterInfoTabItem : StateJurisdictionsDataItem
    {
      private const string GroupName = "GeneralVoterInfo";

      private GeneralVoterInfoTabItem(UpdateJurisdictionsPage page)
        : base(page, GroupName)
      {
      }

      private static GeneralVoterInfoTabItem[] GetTabInfo(
        UpdateJurisdictionsPage page)
      {
        var generalVoterInfoTabInfo = new[]
        {
          new GeneralVoterInfoTabItem(page)
          {
            Column = "PollHours",
            Description = "Normal Polling Hours"
          },
          new GeneralVoterInfoTabItem(page)
          {
            Column = "HowVotingIsDone",
            Description = "How Voting is Done for General Elections"
          },
          new GeneralVoterInfoTabItem(page)
          {
            Column = "HowPrimariesAreDone",
            Description = "How Voting is Done for Primary Elections"
          }
        };

        foreach (var item in generalVoterInfoTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return generalVoterInfoTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._GeneralVoterInfoTabInfo = GetTabInfo(page);
        if (page.AdminPageLevel != AdminPageLevel.State)
          page.TabGeneralVoterInfo.Visible = false;
      }
    }

    private GeneralVoterInfoTabItem[] _GeneralVoterInfoTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonGeneralVoterInfo_OnClick(object sender, EventArgs e)
    {
      switch (GeneralVoterInfoReloading.Value)
      {
        case "reloading":
        {
          GeneralVoterInfoReloading.Value = Empty;
          _GeneralVoterInfoTabInfo.LoadControls();
          FeedbackGeneralVoterInfo.AddInfo("General voter information loaded.");
        }
          break;

        case "":
        {
          // normal update
          _GeneralVoterInfoTabInfo.ClearValidationErrors();
          _GeneralVoterInfoTabInfo.Update(FeedbackGeneralVoterInfo);
        }
          break;

        default:
          throw new VoteException($"Unknown reloading option: '{GeneralVoterInfoReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}