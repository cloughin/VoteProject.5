using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using DB.VoteLog;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class IdentifyWinnersTabItem : ElectionsDataItem
    {
      private const string GroupName = "IdentifyWinners";
      public static int OfficeCount { get; protected set; }

      protected IdentifyWinnersTabItem(UpdateElectionsPage page) : base(page, GroupName)
      {
      }

      private static IdentifyWinnersTabItem[] GetTabInfo(UpdateElectionsPage page)
      {
        var identifyWinnersTabInfo = new[]
        {
          new IdentifyWinnersOfficeTreeTabItem(page)
          {
            Column = "OfficeTree",
            Description = "Office Tree"
          },
          new IdentifyWinnersTabItem(page)
          {
            Column = "IsWinnersIdentified",
            Description = "All Winners are Identified",
            ConvertFn = ToBool
          }
        };

        foreach (var item in identifyWinnersTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return identifyWinnersTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._IdentifyWinnersTabInfo = GetTabInfo(page);
        page.ShowIdentifyGeneralWinners = StateCache.IsValidStateCode(page.StateCode);
        if (!page.ShowIdentifyGeneralWinners)
          page.TabIdentifyGeneralWinnersItem.Visible = false;
      }
    }

    private class IdentifyWinnersOfficeTreeTabItem : IdentifyWinnersTabItem
    {
      internal IdentifyWinnersOfficeTreeTabItem(UpdateElectionsPage page) : base(page)
      {
      }

      protected override string GetCurrentValue() => null;

      public override void LoadControl()
      {
        OfficeCount = 0;
        var table = Elections.GetWinnersData(Page.GetElectionKey());
        var incumbentsToEliminate =
          ElectionsOffices.GetOfficesWithCandidatesToEliminate(Page.GetElectionKey());
        if (table.Rows.Count == 0)
        {
          Page.IdentifyWinnersMessage.RemoveCssClass("hidden");
          Page.IdentifyWinnersInstructions.RemoveCssClass("hidden");
          Page.IdentifyWinnersControl.AddCssClasses("hidden");
          Page.IdentifyWinnersMessage.InnerHtml = "No offices were found for this election";
        }
        else if (incumbentsToEliminate.Count > 0)
        {
          Page.IdentifyWinnersMessage.RemoveCssClass("hidden");
          Page.IdentifyWinnersInstructions.AddCssClasses("hidden");
          Page.IdentifyWinnersControl.AddCssClasses("hidden");
          Page.IdentifyWinnersMessage.InnerHtml =
            "<em>There are too many incumbents for the following offices:</em><br/><br/>" +
            Join("<br/>",
              incumbentsToEliminate.Select(g => Offices.FormatOfficeName(g.First()))) +
            "<br/><br/><em>Please use the </em>Adjust Incumbents<em> tab to remove the extra incumbents or use the </em>Add/Remove Offices<em> tab to remove the office contest.</em>";
        }
        else
        {
          Page.IdentifyWinnersMessage.AddCssClasses("hidden");
          Page.IdentifyWinnersInstructions.RemoveCssClass("hidden");
          Page.IdentifyWinnersControl.RemoveCssClass("hidden");
          OfficeCount =
            Page.PopulateWinnersTree(table, Page.PlaceHolderIdentifyWinnersTree);
        }
      }

      protected override void Log(string oldValue, string newValue)
      {
        // Logging done in Update);
      }

      protected override bool Update(object newValue)
      {
        //Parse the value from the UI tree
        if (!(newValue is string valueStr)) return false;
        var offices = valueStr.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries)
          .Select(officeStr =>
          {
            var selected = officeStr[0] == '*';
            var officeSplit = officeStr.Substring(selected ? 1 : 0).Split('=');
            var isRunoff = officeSplit[1].StartsWith("*", StringComparison.Ordinal);
            if (isRunoff) officeSplit[1] = officeSplit[1].Substring(1);
            return new
            {
              Selected = selected,
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
          var politicians = table
            .Where(row => row.OfficeKey().IsEqIgnoreCase(office.OfficeKey)).ToList();
          foreach (var politician in politicians)
          {
            if (office.IsRunoff)
            {
              var advance = office.Ids.Contains(politician.PoliticianKey,
                StringComparer.OrdinalIgnoreCase);
              if (politician.AdvanceToRunoff != advance)
                LogDataChange.LogUpdate(ElectionsPoliticians.Column.AdvanceToRunoff,
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
          if (office.Selected)
          {
            var keys = politicians.Select(p =>
              (politicianKey: p.PoliticianKey, runningMateKey: p.RunningMateKey)).ToList();
            OfficesOfficials.UpdateIncumbents(office.OfficeKey, office.Ids, keys, UserSecurityClass,
              UserName);
            //// Update incumbents
            //var positions = Offices.GetPositionsDataByOfficeKey(office.OfficeKey)[0];
            //var incumbents = OfficesOfficials.GetData(office.OfficeKey);
            //var politicianKeysToAdd =
            //  new List<string>(
            //    office.Ids.Where(id => id != "vacant" && id != Empty));
            //foreach (var incumbent in incumbents)
            //{
            //  var index =
            //    politicianKeysToAdd.FindIndex(
            //      key => key.IsEqIgnoreCase(incumbent.PoliticianKey));
            //  if (index >= 0) politicianKeysToAdd.RemoveAt(index);
            //  // we don't remove old incumbents for offices with 
            //  // Incumbents > ElectionPositions
            //  else if (positions.ElectionPositions == positions.Incumbents)
            //    incumbent.Delete();
            //}
            //foreach (var keyToAdd in politicianKeysToAdd)
            //{
            //  var politician =
            //    politicians.FirstOrDefault(
            //      row => row.PoliticianKey.IsEqIgnoreCase(keyToAdd));
            //  var runningMateKey = politician == null
            //    ? Empty
            //    : politician.RunningMateKey;
            //  incumbents.AddRow(office.OfficeKey, keyToAdd, runningMateKey,
            //    Offices.GetStateCodeFromKey(office.OfficeKey),
            //    Offices.GetCountyCodeFromKey(office.OfficeKey),
            //    Offices.GetLocalKeyFromKey(office.OfficeKey), Empty, DateTime.UtcNow,
            //    UserSecurityClass, UserName);
            //  LogDataChange.LogInsert(OfficesOfficials.TableName, DateTime.UtcNow,
            //    office.OfficeKey, keyToAdd);
            //}

            //// Update if any changes
            //var incumbentsChanged =
            //  incumbents.FirstOrDefault(row => row.RowState != DataRowState.Unchanged) !=
            //  null;
            //if (incumbentsChanged) OfficesOfficials.UpdateTable(incumbents);
          }
        }

        // Update if any changes
        var winnersChanged =
          table.FirstOrDefault(row => row.RowState != DataRowState.Unchanged) != null;
        if (winnersChanged)
          ElectionsPoliticians.UpdateTable(table,
            ElectionsPoliticiansTable.ColumnSet.Winners);

        LoadControl();
        return winnersChanged;
      }
    }

    private IdentifyWinnersTabItem[] _IdentifyWinnersTabInfo;

    #endregion DataItem object

    private int PopulateWinnersTree(DataTable table, Control root)
    {

      void CreateNode(Control parent, IEnumerable<DataRow> office,
        string className, bool useLine2Only = false)
      {
        var officeRows = office.ToList();
        var officeInfo = officeRows[0];
        var officeState = Offices.GetStateCodeFromKey(officeInfo.OfficeKey());
        var electionState = Elections.GetStateCodeFromKey(GetElectionKey());
        var canUpdateIncumbents = StateCache.IsValidStateCode(officeState) &&
          StateCache.IsValidStateCode(electionState);

        var winners = officeRows.Where(row => row.IsWinner()).ToList();
        var candidates = officeRows.Where(row => !row.IsIncumbentRow()).ToList();
        var advancers = candidates.Where(row => row.AdvanceToRunoff()).ToList();
        // if any candidates are marked as AdvanceToRunoff, we default it as a runoff
        var isRunoff = advancers.Count > 0;
        var incumbents = officeRows.Where(row => row.IsIncumbentRow()).ToList();
        var incumbentsInElectionNotMarkedWinners = officeRows
          .Where(row => !row.IsIncumbentRow() && !row.IsWinner() &&
            incumbents.FirstOrDefault(i => i.PoliticianKey()
              .IsEqIgnoreCase(row.PoliticianKey())) != null).ToList();
        var defaultPoliticians = winners.Select(row => row.PoliticianKey())
          .Union(incumbentsInElectionNotMarkedWinners.Select(row => row.PoliticianKey()))
          .ToList();

        // Creating a dropdown for each Election position
        var dropdownContents = Enumerable.Range(0, officeInfo.ElectionPositions())
          .Select(n => new
          {
            DefaultPolitician =
            n < defaultPoliticians.Count ? defaultPoliticians[n] : Empty,
            List = new List<DataRow>(candidates)
          }).ToList();

        // Now create the node
        // Format the office description
        var text = useLine2Only
          ? officeInfo.OfficeLine2()
          : Offices.FormatOfficeName(officeInfo);

        var addClass = "office-name";
        if (!canUpdateIncumbents) addClass += " no-checkbox";
        if (!IsNullOrWhiteSpace(className)) addClass += " " + className;
        var data = "key:'" + officeInfo.OfficeKey();
        if (canUpdateIncumbents) data += "',addClass:'" + addClass + "',select:true";
        else data += "',addClass:'" + addClass + "',hideCheckbox:true,unselectable:true";
        var officeNode = new HtmlLi().AddTo(parent);
        officeNode.Attributes.Add("data", data);
        new HtmlDiv { InnerHtml = text }.AddTo(officeNode, "label").Attributes
          .Add("Title", text);
        var dropdownDiv = new HtmlDiv().AddTo(officeNode,
          "dropdowns idwinners-" + officeInfo.OfficeKey());

        // if there is a possible runoff, create checkbox and runoff list
        var runoffPositions = officeInfo.GeneralRunoffPositions();
        if (runoffPositions != 0)
        {
          var minCandidates = runoffPositions == -1
            ? officeInfo.ElectionPositions() + 1
            : runoffPositions + 1;
          if (candidates.Count >= minCandidates)
          {
            // if any candidates are marked as AdvanceToRunoff, we default it as a runoff
            isRunoff = candidates.Any(row => row.AdvanceToRunoff());

            var runnoffId = "runoff-" + officeInfo.OfficeKey();
            var runoffDiv = new HtmlDiv().AddTo(dropdownDiv);
            var runoffCheckbox =
              new HtmlInputCheckBox { Checked = isRunoff }.AddTo(runoffDiv, "runoff-checkbox");
            if (runoffPositions > 0)
              runoffCheckbox.Attributes.Add("rel",
                runoffPositions.ToString(CultureInfo.InvariantCulture));
            new HtmlLabel { ID = runnoffId, InnerText = "Runoff is required" }.AddTo(runoffDiv)
              .Attributes["for"] = runnoffId;
            var runoffsDiv = new HtmlDiv().AddTo(runoffDiv, "runoff-dropdown");
            if (!isRunoff) runoffsDiv.AddCssClasses("hidden");
            else runoffsDiv.RemoveCssClass("hidden");
            new HtmlP
            {
              InnerText = "Select " +
                (runoffPositions == -1
                  ? Empty
                  : runoffPositions.ToString(CultureInfo.InvariantCulture)) +
                " candidates to advance"
            }.AddTo(runoffsDiv);

            var runoffList = new HtmlSelect
            {
              EnableViewState = false,
              Size = candidates.Count,
              Multiple = true
            }.AddTo(runoffsDiv);

            foreach (var politician in candidates)
            {
              var name = Politicians.FormatName(politician);
              if (!IsNullOrWhiteSpace(politician.PartyCode()))
                name += " (" + politician.PartyCode() + ")";
              var indicators = Empty;

              // Add winner indicator
              if (politician.AdvanceToRunoff()) indicators += "◄";

              if (indicators != Empty) name += " " + indicators;

              runoffList.AddItem(name, politician.PoliticianKey(),
                politician.AdvanceToRunoff());
            }
          }
        }

        var winnersDiv = new HtmlDiv().AddTo(dropdownDiv, "winners-dropdowns");
        if (isRunoff) winnersDiv.AddCssClasses("hidden");
        else winnersDiv.RemoveCssClass("hidden");

        // Create the dropdowns
        var inx = 0;
        foreach (var dropdownContent in dropdownContents)
        {
          var dropdownList = new HtmlSelect { EnableViewState = false }.AddTo(winnersDiv);

          if (inx >= winners.Count) dropdownList.AddCssClasses("bold");

          // The first option in each dropdown is a disabled header
          var dropDownMessage = dropdownContent.List.Count > 0
            ? "Select winner or Vacant"
            : "No candidates";
          var option = dropdownList.AddItem(dropDownMessage, Empty,
            dropdownContent.DefaultPolitician == Empty);
          option.Attributes.Add("disabled", "disabled");

          // Add an option for each politician
          foreach (var politician in dropdownContent.List.OrderBy(row => row.LastName())
            .ThenBy(row => row.FirstName()))
          {
            var name = Politicians.FormatName(politician);
            if (!IsNullOrWhiteSpace(politician.PartyCode()))
              name += " (" + politician.PartyCode() + ")";
            var indicators = Empty;

            // Add winner indicator
            if (winners.FirstOrDefault(winner => winner.PoliticianKey()
              .IsEqIgnoreCase(politician.PoliticianKey())) != null) indicators += "◄";

            if (indicators != Empty) name += " " + indicators;

            dropdownList.AddItem(name, politician.PoliticianKey(),
              dropdownContent.DefaultPolitician == politician.PoliticianKey());
          }

          // Add a "Vacant" option
          dropdownList.AddItem("Vacant", "vacant");
          inx++;
        }
      }

      var officeCount = 0;
      root.Controls.Clear();
      var tree = new HtmlUl().AddTo(root);

      const string rootText = "Check to update incumbents in addition to recording winners";

      const string rootData = "addClass:'root-node'";

      var rootNode = new HtmlLi {InnerHtml = rootText}.AddTo(tree);
      rootNode.Attributes.Add("data", rootData);
      var rootTree = new HtmlUl().AddTo(rootNode);

      var officeClasses = table.Rows.Cast<DataRow>().GroupBy(row => row.OfficeClass());
      var even = false;
      foreach (var officeClass in officeClasses)
      {
        var offices = officeClass.GroupBy(row => row.OfficeKey()).ToList();
        officeCount += offices.Count;
        if (offices.Count == 1)
        {
          CreateNode(rootTree, offices[0], /*even ? "even" :*/ "odd");
          even = !even;
        }
        else
        {
          // If all OfficeLine1's are identical, don't show them 
          var hasVariedLine1 = offices.Exists(row => row.First().OfficeLine1() !=
            offices[0].First().OfficeLine1());
          var text = Offices.GetOfficeClassShortDescription(officeClass.Key, StateCode);
          var classNode = new HtmlLi {InnerHtml = text}.AddTo(rootTree);
          classNode.Attributes.Add("data",
            "addClass:'office-class office-class-" + officeClass.Key +
            "',key:'office-class-" + officeClass.Key + "'");
          var classSubTree = new HtmlUl().AddTo(classNode);
          foreach (var office in offices)
            CreateNode(classSubTree, office, /*even ? "even" :*/ "odd",
              !hasVariedLine1);
          even = !even;
        }
      }
      return officeCount;
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonIdentifyWinners_OnClick(object sender, EventArgs e)
    {
      switch (IdentifyWinnersReloading.Value)
      {
        case "reloading":
        {
          IdentifyWinnersReloading.Value = Empty;
          IdentifyWinnersDataArea.RemoveCssClass("hidden");
          _IdentifyWinnersTabInfo.LoadControls();
          SetElectionHeading(HeadingIdentifyWinners);
          FeedbackIdentifyWinners.AddInfo(
            $"{IdentifyWinnersTabItem.OfficeCount} offices loaded.");
        }
          break;

        case "":
        {
          // normal update
          _IdentifyWinnersTabInfo.Update(FeedbackIdentifyWinners);
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{IdentifyWinnersReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}