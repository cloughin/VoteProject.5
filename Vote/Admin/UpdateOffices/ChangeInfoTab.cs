namespace Vote.Admin
{
  public partial class UpdateOfficesPage
  {
    #region Private

    #region DataItem object

    //[PageInitializer]
    //private class ChangeInfoTabItem : OfficesDataItem
    //{
    //  private const string GroupName = "ChangeInfo";

    //  private ChangeInfoTabItem(UpdateOfficesPage page) : base(page, GroupName) { }

    //  //protected override string GetCurrentValue()
    //  //{
    //  //  return Column == "ChangeLocals" ?
    //  //    false.ToString() :
    //  //    base.GetCurrentValue();
    //  //}

    //  private static ChangeInfoTabItem[] GetTabInfo(UpdateOfficesPage page)
    //  {
    //    var changeInfoTabInfo = new ChangeInfoTabItem[]
    //      {
    //        //new ChangeInfoTabItem(page)
    //        //  {
    //        //    Column = "ElectionDesc",
    //        //    Description = "Election Description",
    //        //    Validator = ValidateDescription
    //        //  },
    //        //new ChangeInfoTabItem(page)
    //        //  {
    //        //    Column = "ChangeLocals",
    //        //    Description = "Apply Description to County and Local Elections",
    //        //    ConvertFn = ToBool
    //        //  },
    //        //new ChangeInfoTabItem(page)
    //        //  {
    //        //    Column = "ElectionAdditionalInfo",
    //        //    Description = "Additional Election Information"
    //        //  },
    //        //new ChangeInfoTabItem(page)
    //        //  {
    //        //    Column = "BallotInstructions",
    //        //    Description = "Special Ballot Instructions"
    //        //  },
    //        //new ChangeInfoTabItem(page)
    //        //  {
    //        //    Column = "IsViewable",
    //        //    Description = "Election Is Publicly Viewable",
    //        //    ConvertFn = ToBool
    //        //  }
    //      };

    //    foreach (var item in changeInfoTabInfo)
    //      item.InitializeItem(page);

    //    InitializeGroup(page, GroupName);

    //    return changeInfoTabInfo;
    //  }

    //  // ReSharper disable UnusedMember.Local
    //  // Invoked via Reflection
    //  internal static void Initialize(UpdateOfficesPage page)
    //  // ReSharper restore UnusedMember.Local
    //  {
    //    page._ChangeInfoTabInfo = GetTabInfo(page);
    //    if (page.AdminPageLevel != AdminPageLevel.State)
    //    {
    //      //page.InputElementChangeLocals.AddCssClasses("hidden");
    //      //page.InputElementIsViewable.AddCssClasses("hidden");
    //    }
    //  }

    //  protected override void Log(string oldValue, string newValue)
    //  {
    //    if (Column != "ChangeLocals")
    //      base.Log(oldValue, newValue);
    //  }

    //  protected override bool Update(object newValue)
    //  {
    //    //if (Column == "ChangeLocals") return false;
    //    //// for IsViewable on a state election, we update all corresponding county
    //    //// and local elections too.
    //    //if (Page.AdminPageLevel == AdminPageLevel.State &&
    //    //  Column == "IsViewable")
    //    //{
    //    //  Elections.UpdateIsViewableForElectionFamily((bool) newValue,
    //    //    Page.GetElectionKey());
    //    //  return true;
    //    //}
    //    //// for ElectionDesc on a state election, we update all corresponding county
    //    //// and local elections too if ChangeLocals is checked.
    //    //if (Page.ControlChangeInfoChangeLocals.Checked && Column == "ElectionDesc")
    //    //{
    //    //  Elections.UpdateElectionDescForElectionFamily(newValue as string,
    //    //    Page.GetElectionKey());
    //    //  Page.ControlChangeInfoChangeLocals.Checked = false;
    //    //  return true;
    //    //}
    //    return base.Update(newValue);
    //  }
    //}

    //private ChangeInfoTabItem[] _ChangeInfoTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    //protected void ButtonChangeInfo_OnClick(object sender, EventArgs e)
    //{
    //  //var electionKey = GetElectionKey();
    //  //switch (ChangeInfoReloading.Value)
    //  //{
    //  //  case "reloading":
    //  //    {
    //  //      //ChangeInfoReloading.Value = String.Empty;
    //  //      //_ChangeInfoTabInfo.LoadControls();
    //  //      //SetElectionHeading(HeadingChangeInfo);
    //  //      //FeedbackChangeInfo.AddInfo("Election information loaded.");
    //  //    }
    //  //    break;

    //  //  case "":
    //  //    {
    //  //      // normal update
    //  //      //_ChangeInfoTabInfo.ClearValidationErrors();
    //  //      //var originalDesc = Elections.GetElectionDesc(electionKey);
    //  //      //_ChangeInfoTabInfo.Update(FeedbackChangeInfo);

    //  //      //if (originalDesc != Elections.GetElectionDesc(electionKey))
    //  //      //{
    //  //      //  SetElectionHeading(HeadingChangeInfo);
    //  //      //  ReloadElectionControl();
    //  //      //}
    //  //    }
    //  //    break;

    //  //  default:
    //  //    throw new VoteException("Unknown reloading option");
    //  //}
    //}

    #endregion Event handlers and overrides
  }
}