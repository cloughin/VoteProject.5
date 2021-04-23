using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class SelectBallotMeasureControlItem : ElectionsDataItem
    {
      // This class assumes List is the only column
      private const string GroupName = "SelectBallotMeasure";
      public static int BallotMeasureCount { get; private set; }

      private SelectBallotMeasureControlItem(UpdateElectionsPage page)
        : base(page, GroupName)
      {
      }

      private static SelectBallotMeasureControlItem[] GetTabInfo(
        UpdateElectionsPage page)
      {
        var selectBallotMeasureTabInfo = new[]
        {
          new SelectBallotMeasureControlItem(page)
          {
            Column = "List",
            Description = "Ballot Measure List Order"
          }
        };

        foreach (var item in selectBallotMeasureTabInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return selectBallotMeasureTabInfo;
      }

      protected override string GetCurrentValue() => null;

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
        => page._SelectBallotMeasureControlInfo = GetTabInfo(page);

      public override void LoadControl()
      {
        var table = Referendums.GetListData(Page.GetElectionKey());
        BallotMeasureCount = Page.PopulateReferendumList(table);
      }

      protected override void Log(string oldValue, string newValue)
      {
        // Logging done by Update
      }

      protected override bool Update(object newValue)
      {
        var orderChanged = false;
        var stringValue = newValue.ToString();
        var electionKey = Page.GetElectionKey();
        if (!string.IsNullOrWhiteSpace(stringValue))
        {
          var referendumKeys = stringValue.Split(new[] {'|'},
              StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Substring(s.LastIndexOf('-') + 1));
          var referendumOrder = 10;
          foreach (var referendumKey in referendumKeys)
          {
            var oldReferendumOrder = Referendums.GetOrderOnBallot(electionKey,
              referendumKey, 0);
            if (referendumOrder != oldReferendumOrder)
            {
              Referendums.UpdateOrderOnBallot(referendumOrder, electionKey,
                referendumKey);
              LogDataChange.LogUpdate(Referendums.Column.OrderOnBallot,
                oldReferendumOrder.ToString(CultureInfo.InvariantCulture),
                referendumOrder.ToString(CultureInfo.InvariantCulture),
                DateTime.UtcNow, electionKey, referendumKey);
              orderChanged = true;
            }
            referendumOrder += 10;
          }
        }
        LoadControl();
        return orderChanged;
      }
    }

    private SelectBallotMeasureControlItem[] _SelectBallotMeasureControlInfo;

    #endregion DataItem object

    private int PopulateReferendumList(ReferendumsTable table)
    {
      if (table.Count == 0)
        new HtmlLi
          {
            InnerHtml = "No ballot measures have been added for this election"
          }.AddTo
          (ControlSelectBallotMeasureList, "no-ballot-measures");
      else
        foreach (var row in table)
        {
          var referendumLi =
            new HtmlLi
            {
              ID = "referendumorder-" + row.ReferendumKey
            }.AddTo(ControlSelectBallotMeasureList,
              "ballot-measure-desc" +
              (row.IsReferendumTagForDeletion ? " tagged" : string.Empty));
          var icons = new HtmlDiv().AddTo(referendumLi, "icons");
          new HtmlDiv().AddTo(icons, "icon-move tiptip")
            .Attributes["title"] = "Drag to reorder the ballot measures";
          var indicator = "&#x25ba;";
          var status = string.Empty;
          var statusTooltip = "Passed/defeated status unknown";
          if (row.IsResultRecorded)
            if (row.IsPassed)
            {
              indicator = "&#x25b2;";
              status = " passed";
              statusTooltip = "Measure was passed";
            }
            else
            {
              indicator = "&#x25bc;";
              status = " defeated";
              statusTooltip = "Measure was defeated";
            }
          new HtmlDiv {InnerHtml = indicator}.AddTo(icons,
              "tiptip icon-status" + status)
            .Attributes["title"] = statusTooltip;
          new LiteralControl(row.ReferendumTitle).AddTo(referendumLi);
          new HtmlInputHidden {Value = row.ReferendumKey}.AddTo(referendumLi,
            "ballot-measure-key");
        }
      return table.Count;
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonSelectBallotMeasure_OnClick(object sender, EventArgs e)
    {
      switch (SelectBallotMeasureReloading.Value)
      {
        case "reloading":
        {
          SelectBallotMeasureReloading.Value = string.Empty;
          SelectBallotMeasureScrollPosition.Value = string.Empty;
          SelectedBallotMeasureKey.Value = string.Empty;
          SetElectionHeading(HeadingAddBallotMeasures);
          HeadingAddBallotMeasuresBallotMeasure.InnerHtml =
            "No ballot measure selected";
          UpdatePanelAddBallotMeasures.Update();

          _SelectBallotMeasureControlInfo.LoadControls();

          FeedbackSelectBallotMeasure.AddInfo(
            $"{SelectBallotMeasureControlItem.BallotMeasureCount} ballot measures loaded.");
        }
          break;

        case "":
        {
          // normal update
          _SelectBallotMeasureControlInfo.Update(FeedbackSelectBallotMeasure);
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
      ControlSelectBallotMeasureList.AddCssClasses("show");
    }

    #endregion Event handlers and overrides
  }
}