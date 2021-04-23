using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class IdentifyWinnersBetaTabItem : ElectionsDataItem
    {
      private const string GroupName = "IdentifyWinnersBeta";
      public static int OfficeCount { get; protected set; }

      protected IdentifyWinnersBetaTabItem(UpdateElectionsPage page)
        : base(page, GroupName)
      {
      }

      private static IdentifyWinnersBetaTabItem[] GetTabInfo(UpdateElectionsPage page)
      {
        var identifyWinnersTabInfo = new IdentifyWinnersBetaTabItem[]
        {
          new IdentifyWinnersBetaOfficeTreeTabItem(page)
          {
            Column = "OfficeTree",
            Description = "Office Tree"
          }
        };

        foreach (var item in identifyWinnersTabInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return identifyWinnersTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._IdentifyWinnersBetaTabInfo = GetTabInfo(page);
        page.ShowIdentifyPrimaryWinners = StateCache.IsValidStateCode(page.StateCode);
        if (!page.ShowIdentifyPrimaryWinners)
          page.TabIdentifyPrimaryWinnersItem.Visible = false;
      }
    }

    private class IdentifyWinnersBetaOfficeTreeTabItem : IdentifyWinnersBetaTabItem
    {
      internal IdentifyWinnersBetaOfficeTreeTabItem(UpdateElectionsPage page)
        : base(page)
      {
      }

      protected override string GetCurrentValue() => null;

      public override void LoadControl()
      {
        OfficeCount = 0;
        var table = Elections.GetPrimaryWinnersData(Page.GetElectionKey());
        if (table.Rows.Count == 0)
        {
          Page.IdentifyWinnersBetaMessage.RemoveCssClass("hidden");
          Page.IdentifyWinnersBetaControl.AddCssClasses("hidden");
          Page.IdentifyWinnersBetaMessage.InnerHtml =
            "No offices were found for this election";
        }
        else
        {
          Page.IdentifyWinnersBetaMessage.AddCssClasses("hidden");
          Page.IdentifyWinnersBetaControl.RemoveCssClass("hidden");
          OfficeCount = Page.PopulateWinnersBetaTree(table,
            Page.PlaceHolderIdentifyWinnersBetaTree);
        }
      }

      protected override void Log(string oldValue, string newValue)
      {
        // Logging done in Update);
      }

      protected override bool Update(object newValue)
      {
        //Parse the value from the UI tree
        var valueStr = newValue as string;
        if (valueStr == null) return false;
        var offices = valueStr.Split(new[] {'|'},
            StringSplitOptions.RemoveEmptyEntries)
          .Select(officeStr =>
          {
            var officeSplit = officeStr.Split('=');
            var isRunoff = officeSplit[1].StartsWith("*", StringComparison.Ordinal);
            if (isRunoff) officeSplit[1] = officeSplit[1].Substring(1);
            return
              new
              {
                OfficeKey = officeSplit[0],
                IsRunoff = isRunoff,
                Ids = officeSplit[1].Split(',')
              };
          });

        var electionKey = Page.GetElectionKey();
        var table = ElectionsPoliticians.GetWinnersData(electionKey);
        foreach (var o in offices)
        {
          var office = o;
          var politicians = table.Where(row => row.OfficeKey()
              .IsEqIgnoreCase(office.OfficeKey))
            .ToList();
          foreach (var politician in politicians)
          {
            if (office.IsRunoff)
            {
              var advance = office.Ids.Contains(politician.PoliticianKey,
                StringComparer.OrdinalIgnoreCase);
              if (politician.AdvanceToRunoff != advance)
                LogDataChange.LogUpdate(
                  ElectionsPoliticians.Column.AdvanceToRunoff,
                  politician.AdvanceToRunoff, advance, DateTime.UtcNow, electionKey,
                  politician.OfficeKey, politician.PoliticianKey);
              politician.AdvanceToRunoff = advance;
              politician.IsWinner = false;
            }
            else // non-runoff
            {
              var isWinner = office.Ids.Contains(politician.PoliticianKey,
                StringComparer.OrdinalIgnoreCase);
              if (politician.IsWinner != isWinner)
                LogDataChange.LogUpdate(ElectionsPoliticians.Column.IsWinner,
                  politician.IsWinner, isWinner, DateTime.UtcNow, electionKey,
                  politician.OfficeKey, politician.PoliticianKey);
              politician.IsWinner = isWinner;
              politician.AdvanceToRunoff = false;
            }
          }
        }

        // Update if any changes
        var winnersChanged =
          table.FirstOrDefault(row => row.RowState != DataRowState.Unchanged) !=
          null;
        if (winnersChanged)
          ElectionsPoliticians.UpdateTable(table,
            ElectionsPoliticiansTable.ColumnSet.Winners);

        LoadControl();
        return winnersChanged;
      }
    }

    private IdentifyWinnersBetaTabItem[] _IdentifyWinnersBetaTabInfo;

    #endregion DataItem object

    private int PopulateWinnersBetaTree(DataTable table, Control parent)
    {
      var officeCount = 0;
      parent.Controls.Clear();
      var tree = new HtmlUl().AddTo(parent);

      const string rootText = "Select the Winner(s) for Each Contested Primary Office";
      const string rootData = "addClass:'root-node no-checkbox',hideCheckbox:true,unselectable:true";

      var rootNode =
        new HtmlLi
        {
          InnerHtml = rootText
        }.AddTo(tree);
      rootNode.Attributes.Add("data", rootData);
      var rootTree = new HtmlUl().AddTo(rootNode);

      var officeClasses = table.Rows.Cast<DataRow>()
        .GroupBy(row => row.OfficeClass());
      var even = false;
      foreach (var officeClass in officeClasses)
      {
        var offices = officeClass.GroupBy(row => row.OfficeKey())
          .ToList();
        officeCount += offices.Count;
        if (offices.Count == 1)
        {
          PopulateWinnersBetaTree_CreateNode(rootTree, offices[0],
            /*even ? "even" :*/ "odd");
          even = !even;
        }
        else
        {
          // If all OfficeLine1's are identical, don't show them 
          var hasVariedLine1 = offices.Exists(row => row.First()
            .OfficeLine1() != offices[0].First()
            .OfficeLine1());
          var text = Offices.GetOfficeClassShortDescription(officeClass.Key,
            StateCode);
          var classNode =
            new HtmlLi {InnerHtml = text}.AddTo(rootTree);
          classNode.Attributes.Add("data",
            "addClass:'office-class no-checkbox office-class-" + officeClass.Key +
            "',key:'office-class-" + officeClass.Key +
            "',hideCheckbox:true,unselectable:true");
          var classSubTree = new HtmlUl().AddTo(classNode);
          foreach (var office in offices)
          {
            PopulateWinnersBetaTree_CreateNode(classSubTree, office,
              /*even ? "even" :*/ "odd", !hasVariedLine1);
            even = !even;
          }
        }
      }
      return officeCount;
    }

    private static void PopulateWinnersBetaTree_CreateNode(Control parent,
      IEnumerable<DataRow> office, string className = null, bool useLine2Only = false)
    {
      var candidates = office.ToList();
      var officeInfo = candidates[0];
      var winners = candidates.Where(row => row.IsWinner())
        .ToList();
      var advancers = candidates.Where(row => row.AdvanceToRunoff())
        .ToList();
      // if any candidates are marked as AdvanceToRunoff, we default it as a runoff
      var isRunoff = advancers.Count > 0;

      // Now create the node
      // Format the office description
      var text = useLine2Only
        ? officeInfo.OfficeLine2()
        : Offices.FormatOfficeName(officeInfo);
      // Include the position slot count if > 1
      if (officeInfo.PrimaryPositions() > 1)
        text = $"{text} [{officeInfo.PrimaryPositions()}]";

      var addClass = "office-name no-checkbox";
      if (!string.IsNullOrWhiteSpace(className)) addClass += " " + className;
      var data = "key:'" + officeInfo.OfficeKey() +
        "',addClass:'" + addClass + "',hideCheckbox:true,unselectable:true";
      var officeNode = new HtmlLi().AddTo(parent);
      officeNode.Attributes.Add("data", data);
      new HtmlDiv {InnerHtml = text}.AddTo(officeNode, "label");
      var dropdownDiv = new HtmlDiv().AddTo(officeNode,
        "dropdowns idwinners-" + officeInfo.OfficeKey());

      // if there is a possible runoff, create checkbox and runoff list
      var runoffPositions = officeInfo.PrimaryRunoffPositions();
      if (runoffPositions != 0)
      {
        var minCandidates = runoffPositions == -1
          ? officeInfo.PrimaryPositions() + 1
          : runoffPositions + 1;
        if (candidates.Count >= minCandidates)
        {
          // if any candidates are marked as AdvanceToRunoff, we default it as a runoff
          isRunoff = candidates.Any(row => row.AdvanceToRunoff());

          var runnoffId = "runoff-" + officeInfo.OfficeKey();
          var runoffDiv = new HtmlDiv().AddTo(dropdownDiv);
          var runoffCheckbox = new HtmlInputCheckBox {Checked = isRunoff}.AddTo(runoffDiv,
            "runoff-checkbox");
          if (runoffPositions > 0)
            runoffCheckbox.Attributes.Add("rel",
              runoffPositions.ToString(CultureInfo.InvariantCulture));
          new HtmlLabel
            {
              ID = runnoffId,
              InnerText = "Runoff is required"
            }.AddTo(runoffDiv)
            .Attributes["for"] = runnoffId;
          var runoffsDiv = new HtmlDiv().AddTo(runoffDiv, "runoff-dropdown");
          if (!isRunoff) runoffsDiv.AddCssClasses("hidden");
          else runoffsDiv.RemoveCssClass("hidden");
          new HtmlP
          {
            InnerText = "Select " + (runoffPositions == -1
              ? string.Empty
              : runoffPositions.ToString(CultureInfo.InvariantCulture)) + " candidates to advance"
          }.AddTo(runoffsDiv);

          var runoffList =
            new HtmlSelect
            {
              EnableViewState = false,
              Size = candidates.Count,
              Multiple = true
            }.AddTo(runoffsDiv);

          foreach (var politician in candidates)
          {
            var name = Politicians.FormatName(politician);
            if (!string.IsNullOrWhiteSpace(politician.PartyCode()))
              name += " (" + politician.PartyCode() + ")";
            var indicators = string.Empty;

            // Add winner indicator
            if (politician.AdvanceToRunoff()) indicators += "◄";

            if (indicators != string.Empty) name += " " + indicators;

            runoffList.AddItem(name, politician.PoliticianKey(), politician.AdvanceToRunoff());
          }
        }
      }

      var winnersDiv = new HtmlDiv().AddTo(dropdownDiv, "winners-dropdowns");
      if (isRunoff) winnersDiv.AddCssClasses("hidden");
      else winnersDiv.RemoveCssClass("hidden");

      if (candidates.Count <= officeInfo.PrimaryPositions())
      {
        // uncontested -- create a disabled dropdown for each candidate
        foreach (var politician in candidates)
        {
          var dropdownList =
            new HtmlSelect {EnableViewState = false}.AddTo(winnersDiv);
          dropdownList.Attributes.Add("disabled", "disabled");

          var name = Politicians.FormatName(politician);
          if (!string.IsNullOrWhiteSpace(politician.PartyCode()))
            name += " (" + politician.PartyCode() + ")";
          var indicators = string.Empty;

          // Add winner indicator
          if (winners.FirstOrDefault(winner => winner.PoliticianKey()
            .IsEqIgnoreCase(politician.PoliticianKey())) != null) indicators += "◄";

          if (indicators != string.Empty) name += " " + indicators;

          dropdownList.AddItem(name, politician.PoliticianKey(), true);
        }
      }
      else
      {
        // contested -- create a dropdown for each already-identified winner,
        // with the default selection as the winner 
        //// and the other winners removed from the list
        var dropdownContents =
          winners.Select(
              winner =>
                new
                {
                  DefaultPolitician = winner.PoliticianKey(),
                  List =
                  candidates.Where(
                      row =>
                        //  row.PoliticianKey() == winner.PoliticianKey() ||
                        //    !row.IsWinner()
                          true)
                    .ToList()
                })
            .ToList();

        // Create the winner dropdowns
        foreach (var dropdownContent in dropdownContents)
        {
          var dropdownList =
            new HtmlSelect {EnableViewState = false}.AddTo(winnersDiv);
          dropdownList.Attributes.Add("title", "Select one");

          // Add an option for each politician
          foreach (
            var politician in dropdownContent.List.OrderBy(row => row.LastName())
              .ThenBy(row => row.FirstName()))
          {
            var name = Politicians.FormatName(politician);
            if (!string.IsNullOrWhiteSpace(politician.PartyCode()))
              name += " (" + politician.PartyCode() + ")";
            var indicators = string.Empty;

            // Add winner indicator
            if (winners.FirstOrDefault(winner => winner.PoliticianKey()
              .IsEqIgnoreCase(politician.PoliticianKey())) != null) indicators += "◄";

            if (indicators != string.Empty) name += " " + indicators;

            dropdownList.AddItem(name, politician.PoliticianKey(),
              !isRunoff && (dropdownContent.DefaultPolitician == politician.PoliticianKey()));
          }
        }

        // if there are more positions than already-identified winners and some non-winning candidates,
        // fill out with undefaulted lists that contain all candidates 
        if (winners.Count < officeInfo.PrimaryPositions())
        {
          var nonWinners = candidates.Where(row => !row.IsWinner())
            .ToList();
          var counter = Math.Min(nonWinners.Count,
            officeInfo.PrimaryPositions() - winners.Count);
          while (counter-- > 0)
          {
            var dropdownList =
              new HtmlSelect {EnableViewState = false}.AddTo(winnersDiv).AddCssClasses("bold") as
                HtmlSelect;
            Debug.Assert(dropdownList != null, "dropdownList != null");
            dropdownList.Attributes.Add("title", "Select one");

            // The first option in each dropdown is a disabled header
            var option = dropdownList.AddItem("Select Winner", string.Empty, true);
            option.Attributes.Add("disabled", "disabled");

            // Add an option for each politician
            foreach (var politician in candidates.OrderBy(row => row.LastName())
              .ThenBy(row => row.FirstName()))
            {
              var name = Politicians.FormatName(politician);
              if (!string.IsNullOrWhiteSpace(politician.PartyCode()))
                name += " (" + politician.PartyCode() + ")";

              dropdownList.AddItem(name, politician.PoliticianKey());
            }
          }
        }
      }
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonIdentifyWinnersBeta_OnClick(object sender, EventArgs e)
    {
      switch (IdentifyWinnersBetaReloading.Value)
      {
        case "reloading":
        {
          if (!Elections.IsPrimaryElection(GetElectionKey()))
            throw new VoteException("This tab is only for primary elections");
          IdentifyWinnersBetaReloading.Value = string.Empty;
          IdentifyWinnersBetaDataArea.RemoveCssClass("hidden");
          _IdentifyWinnersBetaTabInfo.LoadControls();
          SetElectionHeading(HeadingIdentifyWinnersBeta);
          FeedbackIdentifyWinnersBeta.AddInfo(
            $"{IdentifyWinnersBetaTabItem.OfficeCount} offices loaded.");
        }
          break;

        case "":
        {
          // normal update
          _IdentifyWinnersBetaTabInfo.Update(FeedbackIdentifyWinnersBeta);
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}