using System;
using System.Linq;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class AddBallotMeasuresTabItem : ElectionsDataItem
    {
      private const string GroupName = "AddBallotMeasures";
      protected bool ChangesList { get; private set; }
      public static bool ListChanged { get; protected set; }

      protected AddBallotMeasuresTabItem(UpdateElectionsPage page)
        : base(page, GroupName)
      {
      }

      private static AddBallotMeasuresTabItem[] GetTabInfo(UpdateElectionsPage page)
      {
        var addBallotMeasuresTabInfo = new[]
        {
          new AddBallotMeasuresTabItem(page)
          {
            Column = "ReferendumTitle",
            Description = "Ballot Measure Title",
            Validator = ValidateTitleCaseRequired,
            ChangesList = true
          },
          new AddBallotMeasuresPassedStatusTabItem(page)
          {
            Column = "PassedStatus",
            Description = "Passed/Defeated Status",
            ChangesList = true
          },
          new AddBallotMeasuresTabItem(page)
          {
            Column = "IsReferendumTagForDeletion",
            Description = "Ballot Measure Marked for Deletion",
            ConvertFn = ToBool,
            ChangesList = true
          },
          new AddBallotMeasuresTabItem(page)
          {
            Column = "ReferendumDesc",
            Description = "Referendum Description",
            Validator = ValidateSentenceCase
          },
          new AddBallotMeasuresTabItem(page)
          {
            Column = "ReferendumDetailUrl",
            Description = "Referendum Detail Url",
            Validator = ValidateWebAddress
          },
          new AddBallotMeasuresTabItem(page)
          {
            Column = "ReferendumDetail",
            Description = "Referendum Detail",
            Validator = ValidateSentenceCase
          },
          new AddBallotMeasuresTabItem(page)
          {
            Column = "ReferendumFullTextUrl",
            Description = "Referendum Full Text Url",
            Validator = ValidateWebAddress
          },
          new AddBallotMeasuresTabItem(page)
          {
            Column = "ReferendumFullText",
            Description = "Referendum Full Text",
            Validator = ValidateSentenceCase
          }
        };

        foreach (var item in addBallotMeasuresTabInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return addBallotMeasuresTabInfo;
      }

      protected override string GetCurrentValue()
      {
        var column = Referendums.GetColumn(Column);
        var value = Referendums.GetColumn(column, Page.GetElectionKey(),
          Page.GetBallotMeasureKey());
        return value == null ? string.Empty : ToDisplay(value);
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._AddBallotMeasuresTabInfo = GetTabInfo(page);
        page.ShowAddBallotMeasures = StateCache.IsValidStateCode(page.StateCode);
        if (!page.ShowAddBallotMeasures)
          page.TabAddBallotMeasuresItem.Visible = false;
      }

      protected override void Log(string oldValue, string newValue) => 
        LogDataChange.LogUpdate(Referendums.TableName, Column, oldValue, newValue,
        UserName, UserSecurityClass, DateTime.UtcNow, Page.GetElectionKey(),
        Page.GetBallotMeasureKey());

      protected override bool Update(object newValue)
      {
        var column = Referendums.GetColumn(Column);
        Referendums.UpdateColumn(column, newValue, Page.GetElectionKey(),
          Page.GetBallotMeasureKey());
        if (ChangesList) ListChanged = true;
        return true;
      }
    }

    private class AddBallotMeasuresPassedStatusTabItem : AddBallotMeasuresTabItem
    {
      internal AddBallotMeasuresPassedStatusTabItem(UpdateElectionsPage page)
        : base(page)
      {
      }

      protected override string GetCurrentValue()
      {
        if (!Referendums.GetIsResultRecorded(Page.GetElectionKey(),
          Page.GetBallotMeasureKey(), false)) return "unknown";
        return Referendums.GetIsPassed(Page.GetElectionKey(),
          Page.GetBallotMeasureKey(), false)
          ? "passed"
          : "defeated";
      }

      protected override string GetDefaultValue() => "unknown";

      protected override bool Update(object newValue)
      {
        var isResultRecorded = false;
        var isPassed = false;

        switch (newValue as string)
        {
          case "unknown":
            // defaults are ok
            break;

          case "passed":
            isResultRecorded = true;
            isPassed = true;
            break;

          case "defeated":
            isResultRecorded = true;
            break;

          default:
            return false;
        }

        Referendums.UpdateIsResultRecorded(isResultRecorded, Page.GetElectionKey(),
          Page.GetBallotMeasureKey());
        Referendums.UpdateIsPassed(isPassed, Page.GetElectionKey(),
          Page.GetBallotMeasureKey());

        if (ChangesList) ListChanged = true;

        return true;
      }
    }

    private AddBallotMeasuresTabItem[] _AddBallotMeasuresTabInfo;

    private string GetBallotMeasureKey() => SelectedBallotMeasureKey.Value;

    #endregion DataItem object

    private void AddBallotMeasure(string referendumKey)
    {
      var electionKey = GetElectionKey();
      var electionKeyState = Elections.GetStateElectionKeyFromKey(electionKey);
      var electionKeyCounty = Elections.GetCountyElectionKeyFromKey(electionKey);
      var electionKeyLocal = Elections.GetLocalElectionKeyFromKey(electionKey);
      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      var countyCode = Elections.GetCountyCodeFromKey(electionKey);
      var localCode = Elections.GetLocalCodeFromKey(electionKey);
      var orderOnBallot = Referendums.GetNextOrderOnBallot(electionKey);
      Referendums.Insert(electionKey: electionKey, referendumKey: referendumKey,
        electionKeyState: electionKeyState, electionKeyCounty: electionKeyCounty,
        electionKeyLocal: electionKeyLocal, stateCode: stateCode,
        countyCode: countyCode, localCode: localCode, orderOnBallot: orderOnBallot,
        referendumTitle: string.Empty, referendumDescription: string.Empty,
        referendumDetail: string.Empty, referendumDetailUrl: string.Empty,
        referendumFullText: string.Empty, referendumFullTextUrl: string.Empty,
        isReferendumTagForDeletion: false, isPassed: false, isResultRecorded: false);
    }

    private void RefreshAddBallotMeasuresTab()
    {
      var value = ControlAddBallotMeasuresReferendumDetailUrl.GetValue().Trim();
      if (string.IsNullOrWhiteSpace(value))
      {
        IconBoxReferendumDetailUrl.HRef = string.Empty;
        IconBoxReferendumDetailUrl.AddCssClasses("disabled");
      }
      else
      {
        IconBoxReferendumDetailUrl.HRef = NormalizeUrl(value);
        IconBoxReferendumDetailUrl.RemoveCssClass("disabled");
      }
      value = ControlAddBallotMeasuresReferendumFullTextUrl.GetValue().Trim();
      if (string.IsNullOrWhiteSpace(value))
      {
        IconBoxReferendumFullTextUrl.HRef = string.Empty;
        IconBoxReferendumFullTextUrl.AddCssClasses("disabled");
      }
      else
      {
        IconBoxReferendumFullTextUrl.HRef = NormalizeUrl(value);
        IconBoxReferendumFullTextUrl.RemoveCssClass("disabled");
      }
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonAddBallotMeasures_OnClick(object sender, EventArgs e)
    {
      AddBallotMeasuresRecased.Value = string.Empty;
      switch (AddBallotMeasuresReloading.Value)
      {
        case "loadballotmeasure":
        {
          AddBallotMeasuresReloading.Value = string.Empty;
          AddBallotMeasuresAnimate.Value = "true";
          AddBallotMeasuresSubTabIndex.Value = "0";
          var referendumKey = GetBallotMeasureKey();
          if (referendumKey == "add")
          {
            HeadingAddBallotMeasuresBallotMeasure.InnerHtml =
              "Adding new ballot measure";
            ContainerAddBallotMeasures.AddCssClasses("update-all");
            _AddBallotMeasuresTabInfo.Reset();
            FeedbackAddBallotMeasures.AddInfo("Blank form loaded for new ballot measure.");
          }
          else
          {
            HeadingAddBallotMeasuresBallotMeasure.InnerHtml =
              Referendums.GetReferendumTitle(GetElectionKey(), referendumKey);
            ContainerAddBallotMeasures.RemoveCssClass("update-all");
            _AddBallotMeasuresTabInfo.LoadControls();
            FeedbackAddBallotMeasures.AddInfo("Ballot measure loaded.");
          }
          RefreshAddBallotMeasuresTab();
        }
          break;

        case "":
          AddBallotMeasuresAnimate.Value = "false";
          var listChanged = false;
          if (GetBallotMeasureKey() == "add")
          {
            if (_AddBallotMeasuresTabInfo.Validate())
            {
              // add, then update
              listChanged = true;
              Elections.ActualizeElection(GetElectionKey());
              var newReferendumKey = Referendums.GetUniqueKey(GetElectionKey(),
                ControlAddBallotMeasuresReferendumTitle.GetValue());
              AddBallotMeasure(newReferendumKey);
              ContainerAddBallotMeasures.RemoveCssClass("update-all");
              SelectedBallotMeasureKey.Value = newReferendumKey;
              _AddBallotMeasuresTabInfo.Update(FeedbackAddBallotMeasures, false);
              RefreshAddBallotMeasuresTab();
              FeedbackAddBallotMeasures.Clear();
              FeedbackAddBallotMeasures.AddInfo("Ballot measure added.");
            }
          }
          else
          {
            // normal update
            _AddBallotMeasuresTabInfo.Update(FeedbackAddBallotMeasures);
            listChanged |= AddBallotMeasuresTabItem.ListChanged;
            RefreshAddBallotMeasuresTab();
          }

          AddBallotMeasuresRecased.Value = string.Join("|",
            _AddBallotMeasuresTabInfo.WithWarning("recased")
              .Select(item => item.Description));

          if (listChanged)
          {
            _SelectBallotMeasureControlInfo.LoadControls();
            UpdatePanelSelectBallotMeasure.Update();
            HeadingAddBallotMeasuresBallotMeasure.InnerHtml =
              Referendums.GetReferendumTitle(GetElectionKey(), GetBallotMeasureKey());
          }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}