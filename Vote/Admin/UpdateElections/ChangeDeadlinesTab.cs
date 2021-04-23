using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class ChangeDeadlinesTabItem : ElectionsDataItem
    {
      public const string GroupName = "ChangeDeadlines";
      public bool ElectionOrderChanged { get; protected set; }

      protected ChangeDeadlinesTabItem(UpdateElectionsPage page)
        : base(page, GroupName)
      {
      }

      private static ChangeDeadlinesTabItem[] GetTabInfo(UpdateElectionsPage page)
      {
        var changeDeadlinesTabInfo = new[]
        {
          new ChangeDeadlinesElectionOrderTabItem(page)
          {
            Column = "ElectionOrder",
            Description = "Election Order"
          },
          new ChangeDeadlinesTabItem(page)
          {
            Column = "RegistrationDeadline",
            Description = "Registration Deadline",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new ChangeDeadlinesTabItem(page)
          {
            Column = "EarlyVotingBegin",
            Description = "Early Voting Begin Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new ChangeDeadlinesTabItem(page)
          {
            Column = "EarlyVotingEnd",
            Description = "Early Voting End Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new ChangeDeadlinesTabItem(page)
          {
            Column = "MailBallotBegin",
            Description = "Mail Ballot Begin Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new ChangeDeadlinesTabItem(page)
          {
            Column = "MailBallotEnd",
            Description = "Mail Ballot End Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new ChangeDeadlinesTabItem(page)
          {
            Column = "MailBallotDeadline",
            Description = "Mail Ballot Must Be Received By",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new ChangeDeadlinesTabItem(page)
          {
            Column = "AbsenteeBallotBegin",
            Description = "Absentee Ballot Begin Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new ChangeDeadlinesTabItem(page)
          {
            Column = "AbsenteeBallotEnd",
            Description = "Absentee Ballot End Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new ChangeDeadlinesTabItem(page)
          {
            Column = "AbsenteeBallotDeadline",
            Description = "Absentee Ballot Must Be Received By",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          }
        };

        foreach (var item in changeDeadlinesTabInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return changeDeadlinesTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._ChangeDeadlinesTabInfo = GetTabInfo(page);
        page.ShowChangeDeadlines = StateCache.IsValidStateCode(page.StateCode);
        if (!page.ShowChangeDeadlines)
          page.TabChangeDeadlinesItem.Visible = false;
      }
    }

    private class ChangeDeadlinesElectionOrderTabItem : ChangeDeadlinesTabItem
    {
      internal ChangeDeadlinesElectionOrderTabItem(UpdateElectionsPage page)
        : base(page)
      {
      }

      protected override string GetCurrentValue() => null;

      public override void LoadControl()
      {
        var electionDate = Elections.GetElectionDateFromKey(Page.GetElectionKey());
        var table = Elections.GetElectionsOnDate(Page.StateCode, Page.CountyCode,
          Page.LocalCode, electionDate);
        if (table.Count < 2)
        {
          Page.SetOrderMessage.RemoveCssClass("hidden");
          Page.SetOrderControl.AddCssClasses("hidden");
        }
        else
        {
          Page.SetOrderMessage.AddCssClasses("hidden");
          Page.SetOrderControl.RemoveCssClass("hidden");
          Page.PopulateElectionOrderControl(table);
        }
      }

      protected override void Log(string oldValue, string newValue)
      {
        // Logging done in Update
      }

      protected override bool Update(object newValue)
      {
        ElectionOrderChanged = false;
        var stringValue = newValue.ToString();
        if (!string.IsNullOrWhiteSpace(stringValue))
        {
          var electionKeys = stringValue.Split(new[] {'|'},
              StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Substring(s.LastIndexOf('-') + 1));
          var electionOrder = 10;
          foreach (var electionKey in electionKeys)
          {
            var oldElectionOrder = Elections.GetElectionOrder(electionKey, 0);
            if (electionOrder != oldElectionOrder)
            {
              Elections.UpdateElectionOrder(electionOrder, electionKey);
              LogElectionsDataChange(electionKey, Column,
                oldElectionOrder.ToString(CultureInfo.InvariantCulture),
                electionOrder.ToString(CultureInfo.InvariantCulture));
              ElectionOrderChanged = true;
            }
            electionOrder += 10;
          }
        }
        LoadControl();
        return ElectionOrderChanged;
      }
    }

    private ChangeDeadlinesTabItem[] _ChangeDeadlinesTabInfo;

    #endregion DataItem object

    private void LoadDefaultDeadlines()
    {
      var defaultElectionKey = Elections.GetDefaultElectionKeyFromKey(GetElectionKey());
      var defaults =
        ElectionsDefaults.GetData(defaultElectionKey).FirstOrDefault();
      foreach (var item in _ChangeDeadlinesTabInfo)
      {
        var defaultControl =
          DataItemBase.FindControl(this, "Default" + ChangeDeadlinesTabItem.GroupName + item.Column)
            as HtmlContainerControl;
        if (defaultControl != null)
        {
          var val = (DateTime?) defaults?[item.Column] ?? DefaultDbDate;
          var str = val == DefaultDbDate
            ? "<none>"
            : val.ToString("d");
          defaultControl.InnerText = "Default: " + str;
        }
      }
    }

    private void PopulateElectionOrderControl(IEnumerable<ElectionsRow> table)
    {
      ControlChangeDeadlinesElectionOrder.Controls.Clear();
      foreach (var row in table)
        new HtmlLi
        {
          ID = "electionorder-" + row.ElectionKey,
          InnerHtml = row.ElectionDesc
        }.AddTo(ControlChangeDeadlinesElectionOrder);
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonChangeDeadlines_OnClick(object sender, EventArgs e)
    {
      switch (ChangeDeadlinesReloading.Value)
      {
        case "reloading":
        {
          ChangeDeadlinesReloading.Value = string.Empty;
          _ChangeDeadlinesTabInfo.LoadControls();
          LoadDefaultDeadlines();
          SetElectionHeading(HeadingChangeDeadlines);
          FeedbackChangeDeadlines.AddInfo("Election information loaded.");
        }
          break;

        case "":
        {
          // normal update
          _ChangeDeadlinesTabInfo.ClearValidationErrors();
          _ChangeDeadlinesTabInfo.Update(FeedbackChangeDeadlines);
          if (_ChangeDeadlinesTabInfo.First(i => i.Column == "ElectionOrder")
            .ElectionOrderChanged)
            ReloadElectionControl();
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}