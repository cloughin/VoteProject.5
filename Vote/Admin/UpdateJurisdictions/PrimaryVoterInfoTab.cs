//using System;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    //#region Private

    //#region DataItem object

    //[PageInitializer]
    //private class PrimaryVoterInfoTabItem : StateJurisdictionsDataItem
    //{
    //  private const string GroupName = "PrimaryVoterInfo";

    //  private PrimaryVoterInfoTabItem(UpdateJurisdictionsPage page)
    //    : base(page, GroupName) { }

    //  private static PrimaryVoterInfoTabItem[] GetTabInfo(
    //    UpdateJurisdictionsPage page)
    //  {
    //    var primaryVoterInfoTabInfo = new PrimaryVoterInfoTabItem[]
    //    { 
    //      /*
    //      new PrimaryVoterInfoTabItem(page)
    //      {
    //        Column = "HowPrimariesAreDone",
    //        Description = "How Primaries are conducted"
    //      },
    //      new PrimaryVoterInfoTabItem(page)
    //      {
    //        Column = "StatePrimariesHaveSeparatePartyBallots",
    //        Description = "State primaries have separate ballots for each party",
    //        ConvertFn = ToBool
    //      },
    //      new PrimaryVoterInfoTabItem(page)
    //      {
    //        Column = "PresidentialPrimariesHaveSeparatePartyBallots",
    //        Description = "Presidential primaries have separate ballots for each party",
    //        ConvertFn = ToBool
    //      }
    //       * */
    //    };

    //    foreach (var item in primaryVoterInfoTabInfo) item.InitializeItem(page);

    //    InitializeGroup(page, GroupName);

    //    return primaryVoterInfoTabInfo;
    //  }

    //  // ReSharper disable UnusedMember.Local
    //  // Invoked via Reflection
    //  internal static void Initialize(UpdateJurisdictionsPage page)
    //  // ReSharper restore UnusedMember.Local
    //  {
    //    page._PrimaryVoterInfoTabInfo = GetTabInfo(page);
    //    if (page.AdminPageLevel != AdminPageLevel.State)
    //      page.TabPrimaryVoterInfo.Visible = false;
    //  }
    //}

    //private PrimaryVoterInfoTabItem[] _PrimaryVoterInfoTabInfo;

    //#endregion DataItem object

    //#endregion Private

    //#region Event handlers and overrides

    //protected void ButtonPrimaryVoterInfo_OnClick(object sender, EventArgs e)
    //{
    //  switch (PrimaryVoterInfoReloading.Value)
    //  {
    //    case "reloading":
    //      {
    //        PrimaryVoterInfoReloading.Value = String.Empty;
    //        _PrimaryVoterInfoTabInfo.LoadControls();
    //        FeedbackPrimaryVoterInfo.AddInfo("Primary voter information loaded.");
    //      }
    //      break;

    //    case "":
    //      {
    //        // normal update
    //        _PrimaryVoterInfoTabInfo.ClearValidationErrors();
    //        _PrimaryVoterInfoTabInfo.Update(FeedbackPrimaryVoterInfo);
    //      }
    //      break;

    //    default:
    //      throw new VoteException("Unknown reloading option");
    //  }
    //}

    //#endregion Event handlers and overrides
  }
}