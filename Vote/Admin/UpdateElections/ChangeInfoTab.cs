using System;
using System.Linq;
using System.Net;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class ChangeInfoTabItem : ElectionsDataItem
    {
      public const string GroupName = "ChangeInfo";

      private ChangeInfoTabItem(UpdateElectionsPage page) : base(page, GroupName)
      {
      }

      protected override string GetCurrentValue() => 
        Column == "ChangeLocals"
        ? false.ToString()
        : base.GetCurrentValue();

      private static ChangeInfoTabItem[] GetTabInfo(UpdateElectionsPage page)
      {
        var changeInfoTabInfo = new[]
        {
          new ChangeInfoTabItem(page)
          {
            Column = "ElectionDesc",
            Description = "Election Description",
            Validator = ValidateDescription
          },
          new ChangeInfoTabItem(page)
          {
            Column = "ChangeLocals",
            Description = "Apply Description to County and Local Elections",
            ConvertFn = ToBool
          },
          new ChangeInfoTabItem(page)
          {
            Column = "ElectionAdditionalInfo",
            Description = "Additional Election Information"
          },
          new ChangeInfoTabItem(page)
          {
            Column = "BallotInstructions",
            Description = "Special Ballot Instructions"
          },
          new ChangeInfoTabItem(page)
          {
            Column = "IsViewable",
            Description = "Election Is Publicly Viewable",
            ConvertFn = ToBool
          }
        };

        foreach (var item in changeInfoTabInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return changeInfoTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._ChangeInfoTabInfo = GetTabInfo(page);

        page.ShowChangeInfo = true;
        if (!page.ShowChangeInfo)
          page.TabChangeInfoItem.Visible = false;

        switch (page.AdminPageLevel)
        {
          case AdminPageLevel.State:
          case AdminPageLevel.Federal:
            break; // no action

          case AdminPageLevel.President:
            page.InputElementChangeLocals.AddCssClasses("hidden");
            break;

          default:
            page.InputElementChangeLocals.AddCssClasses("hidden");
            page.InputElementIsViewable.AddCssClasses("hidden");
            break;
        }
      }

      protected override void Log(string oldValue, string newValue)
      {
        if (Column != "ChangeLocals")
          base.Log(oldValue, newValue);
      }

      protected override bool Update(object newValue)
      {
        if (Column == "ChangeLocals") return false;
        // for IsViewable on a state election, we update all corresponding county
        // and local elections too.
        if (((Page.AdminPageLevel == AdminPageLevel.State) ||
            (Page.AdminPageLevel == AdminPageLevel.Federal)) &&
          (Column == "IsViewable"))
        {
          Elections.UpdateIsViewableForElectionFamily((bool) newValue,
            Page.GetElectionKey());
          return true;
        }
        // for ElectionDesc on a state election, we update all corresponding county
        // and local elections too if ChangeLocals is checked.
        if (Page.ControlChangeInfoChangeLocals.Checked && (Column == "ElectionDesc"))
        {
          Elections.UpdateElectionDescForElectionFamily(newValue as string,
            Page.GetElectionKey());
          Page.ControlChangeInfoChangeLocals.Checked = false;
          return true;
        }
        return base.Update(newValue);
      }
    }

    private ChangeInfoTabItem[] _ChangeInfoTabInfo;

    #endregion DataItem object

    private void LoadDefaultText()
    {
      var defaultElectionKey = Elections.GetDefaultElectionKeyFromKey(GetElectionKey());
      var defaults =
        ElectionsDefaults.GetData(defaultElectionKey).FirstOrDefault();
      foreach (var item in _ChangeInfoTabInfo)
      {
        var defaultControl =
          DataItemBase.FindControl(this, "Default" + ChangeInfoTabItem.GroupName + item.Column) as
            HtmlContainerControl;
        if (defaultControl != null)
        {
          var val = defaults?[item.Column] as string;
          var str = string.IsNullOrWhiteSpace(val)
            ? "<none>"
            : val;
          defaultControl.InnerText = "Default: " + str;
          defaultControl.Attributes["title"] = WebUtility.HtmlEncode(defaultControl.InnerText);
        }
      }
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonChangeInfo_OnClick(object sender, EventArgs e)
    {
      var electionKey = GetElectionKey();
      switch (ChangeInfoReloading.Value)
      {
        case "reloading":
        {
          ChangeInfoReloading.Value = string.Empty;
          _ChangeInfoTabInfo.LoadControls();
          LoadDefaultText();
          SetElectionHeading(HeadingChangeInfo);
          FeedbackChangeInfo.AddInfo("Election information loaded.");
        }
          break;

        case "":
        {
          // normal update
          _ChangeInfoTabInfo.ClearValidationErrors();
          var originalDesc = Elections.GetElectionDesc(electionKey);
          _ChangeInfoTabInfo.Update(FeedbackChangeInfo);

          if (originalDesc != Elections.GetElectionDesc(electionKey))
          {
            SetElectionHeading(HeadingChangeInfo);
            ReloadElectionControl();
          }
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}