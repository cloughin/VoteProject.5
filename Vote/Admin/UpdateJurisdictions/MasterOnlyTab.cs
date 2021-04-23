using System;
using System.Collections.Generic;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    private enum MasterOnlySubTab
    {
      PlaceHolder
    }

// ReSharper disable once NotAccessedField.Local
    private List<MasterOnlySubTab> _MasterOnlySubTabs;

    #region DataItem object

    [PageInitializer]
// ReSharper disable once ClassNeverInstantiated.Local
    private class MasterOnlyTabItem : JurisdictionsDataItem
    {
      private const string GroupName = "MasterOnly";
      //private MasterOnlySubTab SubTab { get; set; }

      protected MasterOnlyTabItem(UpdateJurisdictionsPage page) : base(page, GroupName)
      {
      }

      protected override string GetCurrentValue()
      {
        throw new NotImplementedException();
      }

      private static MasterOnlyTabItem[] GetTabInfo(UpdateJurisdictionsPage page)
      {
        var masterOnlyTabInfo = new MasterOnlyTabItem[]
        {
          //new MasterOnlyClassesToLockTabItem(page)
          //{
          //  Column = "ClassesToLock",
          //  Description = "Office classes to Lock",
          //  SubTab = MasterOnlySubTab.LockClasses
          //}
        };

        foreach (var item in masterOnlyTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return masterOnlyTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._MasterOnlyTabInfo = GetTabInfo(page);

        page._MasterOnlySubTabs = new List<MasterOnlySubTab>
        {
          MasterOnlySubTab.PlaceHolder
        };

        //if (!IsMasterUser) page.TabMasterItem.Visible = false;
      }

      protected override bool Update(object newValue)
      {
        throw new NotImplementedException();
      }
    }

// ReSharper disable once NotAccessedField.Local
    private MasterOnlyTabItem[] _MasterOnlyTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    //protected void ButtonMasterOnly_OnClick(object sender, EventArgs e)
    //{
    //  //switch (MasterOnlyReloading.Value)
    //  //{
    //  //  case "reloading":
    //  //    {
    //  //      MasterOnlyReloading.Value = String.Empty;
    //  ////      ControlMasterOnlyElectionDesc.Enabled = false;
    //  ////      MasterOnlyDateWasChanged.Value = String.Empty;
    //  //      _MasterOnlyTabInfo.LoadControls();
    //  ////      SetElectionHeading(HeadingMasterOnly);
    //  //      FeedbackMasterOnly.AddInfo("Master-only data loaded.");
    //  //    }
    //  //    break;

    //  //  case "":
    //  //    {
    //  //      // normal update
    //  //      int subTabIndex;
    //  //      _MasterOnlyTabInfo.ClearValidationErrors();
    //  //      if (Int32.TryParse(ContainerMasterOnlySubTabIndex.Value, out subTabIndex))
    //  //        switch (_MasterOnlySubTabs[subTabIndex])
    //  //        {
    //  //          case MasterOnlySubTab.PlaceHolder:
    //  //            break;
    //  //        }
    //  //    }
    //  //    break;

    //  //  default:
    //  //    throw new VoteException("Unknown reloading option");
    //  //}
    //}

    #endregion Event handlers and overrides
  }
}