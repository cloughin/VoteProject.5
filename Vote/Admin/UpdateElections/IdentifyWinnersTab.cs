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

      protected IdentifyWinnersTabItem(UpdateElectionsPage page)
        : base(page, GroupName)
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
        page._IdentifyWinnersTabInfo = GetTabInfo(page);
        page.ShowIdentifyGeneralWinners = StateCache.IsValidStateCode(page.StateCode);
        if (!page.ShowIdentifyGeneralWinners)
          page.TabIdentifyGeneralWinnersItem.Visible = false;
      }
    }

    private class IdentifyWinnersOfficeTreeTabItem : IdentifyWinnersTabItem
    {
      internal IdentifyWinnersOfficeTreeTabItem(UpdateElectionsPage page)
        : base(page)
      {
      }

      protected override string GetCurrentValue() => null;

      public override void LoadControl()
      {
        OfficeCount = 0;
        var table = Elections.GetWinnersData(Page.GetElectionKey());
        var incumbentsToEliminate =
          ElectionsOffices.GetOfficesWithCandidatesToEliminate(
            Page.GetElectionKey());
        if (table.Rows.Count == 0)
        {
          Page.IdentifyWinnersMessage.RemoveCssClass("hidden");
          Page.IdentifyWinnersInstructions.RemoveCssClass("hidden");
          Page.IdentifyWinnersControl.AddCssClasses("hidden");
          Page.IdentifyWinnersMessage.InnerHtml =
            "No offices were found for this election";
        }
        else if (incumbentsToEliminate.Count > 0)
        {
          Page.IdentifyWinnersMessage.RemoveCssClass("hidden");
          Page.IdentifyWinnersInstructions.AddCssClasses("hidden");
          Page.IdentifyWinnersControl.AddCssClasses("hidden");
          Page.IdentifyWinnersMessage.InnerHtml =
            "<em>There are too many incumbents for the following offices:</em><br/><br/>" +
            string.Join("<br/>",
              incumbentsToEliminate.Select(g => Offices.FormatOfficeName(g.First()))) +
            "<br/><br/><em>Please use the </em>Adjust Incumbents<em> tab to remove the extra incumbents or use the </em>Add/Remove Offices<em> tab to remove the office contest.</em>";
        }
        else
        {
          Page.IdentifyWinnersMessage.AddCssClasses("hidden");
          Page.IdentifyWinnersInstructions.RemoveCssClass("hidden");
          Page.IdentifyWinnersControl.RemoveCssClass("hidden");
          OfficeCount = Page.PopulateWinnersTree(table,
            Page.PlaceHolderIdentifyWinnersTree);
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
            var selected = officeStr[0] == '*';
            var officeSplit = officeStr.Substring(selected ? 1 : 0)
              .Split('=');
            var isRunoff = officeSplit[1].StartsWith("*", StringComparison.Ordinal);
            if (isRunoff) officeSplit[1] = officeSplit[1].Substring(1);
            return
              new
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
          if (office.Selected)
          {
            // Update incumbents
            var positions = Offices.GetPositionsDataByOfficeKey(office.OfficeKey)[0];
            var incumbents = OfficesOfficials.GetData(office.OfficeKey);
            var politicianKeysToAdd = new List<string>(office.Ids.Where(id => id != "vacant"));
            foreach (var incumbent in incumbents)
            {
              var index =
                politicianKeysToAdd.FindIndex(
                  key => key.IsEqIgnoreCase(incumbent.PoliticianKey));
              if (index >= 0)
                politicianKeysToAdd.RemoveAt(index);
              // we don't remove old incumbents for offices with 
              // Incumbents > ElectionPositions
              else if (positions.ElectionPositions == positions.Incumbents)
                incumbent.Delete();
            }
            foreach (var keyToAdd in politicianKeysToAdd)
            {
              var politician =
                politicians.FirstOrDefault(
                  row => row.PoliticianKey.IsEqIgnoreCase(keyToAdd));
              var runningMateKey = politician == null
                ? string.Empty
                : politician.RunningMateKey;
              incumbents.AddRow(officeKey: office.OfficeKey,
                politicianKey: keyToAdd, runningMateKey: runningMateKey,
                stateCode: Offices.GetStateCodeFromKey(office.OfficeKey),
                countyCode: Offices.GetCountyCodeFromKey(office.OfficeKey),
                localCode: Offices.GetLocalCodeFromKey(office.OfficeKey),
                districtCode: string.Empty,
                //LDSVersion: String.Empty, LDSUpdateDate: DefaultDbDate,
                dataLastUpdated: DateTime.UtcNow,
                userSecurity: UserSecurityClass, userName: UserName);
              LogDataChange.LogInsert(OfficesOfficials.TableName, DateTime.UtcNow,
                office.OfficeKey, keyToAdd);
            }

            // Update if any changes
            var incumbentsChanged =
              incumbents.FirstOrDefault(
                row => row.RowState != DataRowState.Unchanged) != null;
            if (incumbentsChanged)
              OfficesOfficials.UpdateTable(incumbents);
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

    private IdentifyWinnersTabItem[] _IdentifyWinnersTabInfo;

    #endregion DataItem object

    //private int PopulateWinnersTreeOld(DataTable table, Control parent,
    //  bool isPrimary)
    //{
    //  var officeCount = 0;
    //  parent.Controls.Clear();
    //  var tree = new HtmlUl().AddTo(parent);

    //  var rootText = isPrimary
    //    ? "Cannot update incumbents for primary election"
    //    : "Check to update incumbents in addition to recording winners";

    //  var rootData = isPrimary
    //    ? "addClass:'root-node no-checkbox',hideCheckbox:true,unselectable:true"
    //    : "addClass:'root-node'";

    //  var rootNode =
    //    new HtmlLi
    //    {
    //      InnerHtml = rootText
    //    }.AddTo(tree);
    //  rootNode.Attributes.Add("data", rootData);
    //  var rootTree = new HtmlUl().AddTo(rootNode);

    //  var officeClasses = table.Rows.Cast<DataRow>()
    //    .GroupBy(row => row.OfficeClass());
    //  foreach (var officeClass in officeClasses)
    //  {
    //    var offices = officeClass.GroupBy(row => row.OfficeKey())
    //      .ToList();
    //    officeCount += offices.Count;
    //    if (offices.Count == 1)
    //      PopulateWinnersTree_CreateNodeOld(rootTree, offices[0], false, false);
    //    else
    //    {
    //      // If all OfficeLine1's are identical, don't show them 
    //      var hasVariedLine1 = offices.Exists(row => row.First()
    //        .OfficeLine1() != offices[0].First()
    //          .OfficeLine1());
    //      var text = Offices.GetOfficeClassShortDescription(officeClass.Key,
    //        StateCode);
    //      var classNode =
    //        new HtmlLi { InnerHtml = text }.AddTo(rootTree);
    //      classNode.Attributes.Add("data", 
    //        "addClass:'office-class office-class-" + officeClass.Key +
    //         "',key:'office-class-" + officeClass.Key + "'");
    //      var classSubTree = new HtmlUl().AddTo(classNode);
    //      foreach (var office in offices)
    //        PopulateWinnersTree_CreateNodeOld(classSubTree, office, !hasVariedLine1, isPrimary);
    //    }
    //  }
    //  return officeCount;
    //}

    //private void PopulateWinnersTree_CreateNodeOld(Control parent,
    //  IEnumerable<DataRow> office, bool useLine2Only = false,
    //  bool isPrimary = false)
    //{
    //  var officeRows = office.ToList();
    //  var officeInfo = officeRows[0];
    //  var officeState = Offices.GetStateCodeFromKey(officeInfo.OfficeKey());
    //  var electionState = Elections.GetStateCodeFromKey(GetElectionKey());
    //  var canUpdateIncumbents = StateCache.IsValidStateCode(officeState) &&
    //    StateCache.IsValidStateCode(electionState) && !isPrimary;

    //  var winners = officeRows.Where(row => row.IsWinner())
    //    .ToList();
    //  var candidates = officeRows.Where(row => !row.IsIncumbentRow())
    //    .ToList();
    //  var incumbents = officeRows.Where(row => row.IsIncumbentRow())
    //    .ToList();

    //  // We begin by creating a dropdown for each already-identified winner,
    //  // with the default selection as the winner.
    //  var dropdownContents =
    //    winners.Select(
    //      winner =>
    //        new
    //        {
    //          DefaultPolitician = winner.PoliticianKey(),
    //          List = new List<DataRow>(candidates)
    //        })
    //      .ToList();

    //  if (dropdownContents.Count < officeInfo.Incumbents())
    //    // We need additional dropdowns
    //    // If this is a single incumbent office, we add incumbent as
    //    // default selection only if a candidate
    //    if (officeInfo.Incumbents() == 1)
    //      dropdownContents.AddRange(incumbents.Where(
    //        incumbent =>
    //          candidates.FirstOrDefault(candidate => candidate.PoliticianKey()
    //            .IsEqIgnoreCase(incumbent.PoliticianKey())) != null)
    //        .Select(
    //          incumbent =>
    //            new
    //            {
    //              DefaultPolitician = incumbent.PoliticianKey(),
    //              List = new List<DataRow>(candidates)
    //            }));
    //    else
    //      // There are multiple incumbent slots
    //      if (winners.Count > 0)
    //        // There were winners, but not enough to fill all slots.
    //        // We add any incumbents NOT in the election as defaults, along
    //        // with all candidates
    //        dropdownContents.AddRange(incumbents.Where(
    //          incumbent =>
    //            candidates.FirstOrDefault(candidate => candidate.PoliticianKey()
    //              .IsEqIgnoreCase(incumbent.PoliticianKey())) == null)
    //          .Select(
    //            incumbent =>
    //              new
    //              {
    //                DefaultPolitician = incumbent.PoliticianKey(),
    //                List =
    //                  new List<DataRow>(
    //                    candidates.Union(new List<DataRow> { incumbent }))
    //              }));
    //      else
    //        // No winners were identified. Add all incumbents as defaults, 
    //        // whether in the election or not. The Where on candidates is to
    //        // prevent a duplicate entry when the incumbent is in the election
    //        dropdownContents.AddRange(
    //          incumbents.Select(
    //            incumbent =>
    //              new
    //              {
    //                DefaultPolitician = incumbent.PoliticianKey(),
    //                List =
    //                  new List<DataRow>(candidates.Where(
    //                    candidate => candidate.PoliticianKey()
    //                      .IsNeIgnoreCase(incumbent.PoliticianKey()))
    //                    .Union(new List<DataRow> { incumbent }))
    //              }));

    //  // If there are still office slots to fill, fill them out with undefaulted
    //  // candidate lists.
    //  while (dropdownContents.Count < officeInfo.Incumbents())
    //    dropdownContents.Add(
    //      new
    //      {
    //        DefaultPolitician = String.Empty,
    //        List = new List<DataRow>(candidates)
    //      });

    //  // Now create the node
    //  // Format he office description
    //  var text = useLine2Only
    //    ? officeInfo.OfficeLine2()
    //    : Offices.FormatOfficeName(officeInfo);
    //  // Include the incumbent slot count if > 1
    //  if (officeInfo.Incumbents() > 1)
    //    text = String.Format("{0} [{1}]", text, officeInfo.Incumbents());

    //  var data = "key:'" + officeInfo.OfficeKey();
    //  if (canUpdateIncumbents)
    //    data += "',addClass:'office-name'";
    //  else
    //    data +=
    //      "',addClass:'office-name no-checkbox',hideCheckbox:true,unselectable:true";
    //  var officeNode = new HtmlLi().AddTo(parent);
    //  officeNode.Attributes.Add("data", data);
    //  new HtmlDiv { InnerHtml = text }.AddTo(officeNode, "label");
    //  var dropdownDiv = new HtmlDiv().AddTo(officeNode,
    //    "dropdowns idwinners-" + officeInfo.OfficeKey());

    //  // Create the dropdowns
    //  foreach (var dropdownContent in dropdownContents)
    //  {
    //    var dropdownList =
    //      new HtmlSelect { EnableViewState = false }.AddTo(dropdownDiv);

    //    // The first option in each dropdown is a disabled header
    //    var dropDownMessage = officeInfo.Incumbents() == 1
    //      ? "Select winner or Vacant"
    //      : "Select winner, incumbent or Vacant";
    //    var option = dropdownList.AddItem(dropDownMessage, String.Empty,
    //      dropdownContent.DefaultPolitician == String.Empty);
    //    option.Attributes.Add("disabled", "disabled");

    //    // Add an option for each politician
    //    foreach (
    //      var politician in dropdownContent.List.OrderBy(row => row.LastName())
    //        .ThenBy(row => row.FirstName()))
    //    {
    //      var name = Politicians.FormatName(politician);
    //      if (!String.IsNullOrWhiteSpace(politician.PartyCode()))
    //        name += " (" + politician.PartyCode() + ")";
    //      var indicators = String.Empty;

    //      // Add winner indicator
    //      if (winners.FirstOrDefault(winner => winner.PoliticianKey()
    //        .IsEqIgnoreCase(politician.PoliticianKey())) != null)
    //        indicators += "◄";

    //      // Add incumbent indicator
    //      if (incumbents.FirstOrDefault(incumbent => incumbent.PoliticianKey()
    //        .IsEqIgnoreCase(politician.PoliticianKey())) != null)
    //        if (candidates.FirstOrDefault(candidate => candidate.PoliticianKey()
    //          .IsEqIgnoreCase(politician.PoliticianKey())) != null)
    //          indicators += "■";
    //        else
    //          indicators += "□";

    //      if (indicators != String.Empty)
    //        name += " " + indicators;

    //      dropdownList.AddItem(name, politician.PoliticianKey(),
    //        dropdownContent.DefaultPolitician == politician.PoliticianKey());
    //    }

    //    // Add a "Vacant" option
    //    dropdownList.AddItem("Vacant", "vacant");
    //  }
    //}

    private int PopulateWinnersTree(DataTable table, Control parent)
    {
      var officeCount = 0;
      parent.Controls.Clear();
      var tree = new HtmlUl().AddTo(parent);

      const string rootText = "Check to update incumbents in addition to recording winners";

      const string rootData = "addClass:'root-node'";

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
          PopulateWinnersTree_CreateNode(rootTree, offices[0], /*even ? "even" :*/ "odd");
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
            "addClass:'office-class office-class-" + officeClass.Key +
            "',key:'office-class-" + officeClass.Key + "'");
          var classSubTree = new HtmlUl().AddTo(classNode);
          foreach (var office in offices)
            PopulateWinnersTree_CreateNode(classSubTree, office, /*even ? "even" :*/ "odd",
              !hasVariedLine1);
          even = !even;
        }
      }
      return officeCount;
    }

    private void PopulateWinnersTree_CreateNode(Control parent,
      IEnumerable<DataRow> office, string className, bool useLine2Only = false)
    {
      var officeRows = office.ToList();
      var officeInfo = officeRows[0];
      var officeState = Offices.GetStateCodeFromKey(officeInfo.OfficeKey());
      var electionState = Elections.GetStateCodeFromKey(GetElectionKey());
      var canUpdateIncumbents = StateCache.IsValidStateCode(officeState) &&
        StateCache.IsValidStateCode(electionState);

      var winners = officeRows.Where(row => row.IsWinner())
        .ToList();
      var candidates = officeRows.Where(row => !row.IsIncumbentRow())
        .ToList();
      var advancers = candidates.Where(row => row.AdvanceToRunoff())
        .ToList();
      // if any candidates are marked as AdvanceToRunoff, we default it as a runoff
      var isRunoff = advancers.Count > 0;
      var incumbents = officeRows.Where(row => row.IsIncumbentRow())
        .ToList();
      var incumbentsInElectionNotMarkedWinners = officeRows
        .Where(row => !row.IsIncumbentRow() && !row.IsWinner() &&
        (incumbents.FirstOrDefault(i => i.PoliticianKey()
          .IsEqIgnoreCase(row.PoliticianKey())) != null))
        .ToList();
      var defaultPoliticians = winners.Select(row => row.PoliticianKey())
        .Union(
          incumbentsInElectionNotMarkedWinners.Select(row => row.PoliticianKey()))
        .ToList();

      // Creating a dropdown for each Election position
      var dropdownContents = Enumerable.Range(0, officeInfo.ElectionPositions())
        .Select(n => new
        {
          DefaultPolitician = n < defaultPoliticians.Count ? defaultPoliticians[n] : string.Empty,
          List = new List<DataRow>(candidates)
        })
        .ToList();

      // Now create the node
      // Format the office description
      var text = useLine2Only
        ? officeInfo.OfficeLine2()
        : Offices.FormatOfficeName(officeInfo);

      var addClass = "office-name";
      if (!canUpdateIncumbents) addClass += " no-checkbox";
      if (!string.IsNullOrWhiteSpace(className)) addClass += " " + className;
      var data = "key:'" + officeInfo.OfficeKey();
      if (canUpdateIncumbents)
        data += "',addClass:'" + addClass + "',select:true";
      else
        data +=
          "',addClass:'" + addClass + "',hideCheckbox:true,unselectable:true";
      var officeNode = new HtmlLi().AddTo(parent);
      officeNode.Attributes.Add("data", data);
      new HtmlDiv {InnerHtml = text}.AddTo(officeNode, "label")
        .Attributes.Add("Title", text);
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

      // Create the dropdowns
      var inx = 0;
      foreach (var dropdownContent in dropdownContents)
      {
        var dropdownList =
          new HtmlSelect {EnableViewState = false}.AddTo(winnersDiv);

        if (inx >= winners.Count) dropdownList.AddCssClasses("bold");

        // The first option in each dropdown is a disabled header
        var dropDownMessage = dropdownContent.List.Count > 0
          ? "Select winner or Vacant"
          : "No candidates";
        var option = dropdownList.AddItem(dropDownMessage, string.Empty,
          dropdownContent.DefaultPolitician == string.Empty);
        option.Attributes.Add("disabled", "disabled");

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
            .IsEqIgnoreCase(politician.PoliticianKey())) != null)
            indicators += "◄";

          if (indicators != string.Empty)
            name += " " + indicators;

          dropdownList.AddItem(name, politician.PoliticianKey(),
            dropdownContent.DefaultPolitician == politician.PoliticianKey());
        }

        // Add a "Vacant" option
        dropdownList.AddItem("Vacant", "vacant");
        inx++;
      }
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonIdentifyWinners_OnClick(object sender, EventArgs e)
    {
      switch (IdentifyWinnersReloading.Value)
      {
        case "reloading":
        {
          IdentifyWinnersReloading.Value = string.Empty;
          //if (Elections.IsPrimaryElection(GetElectionKey()))
          //{
          //  IdentifyWinnersDataArea.AddCssClasses("hidden");
          //  HeadingIdentifyWinners.InnerHtml =
          //    "This panel is not available for primary elections";
          //  FeedbackIdentifyWinners.AddError(
          //    "This panel is not available for primary elections");
          //}
          //else
          {
            //if (Elections.IsPrimaryElection(GetElectionKey()))
            //  ControlI
            IdentifyWinnersDataArea.RemoveCssClass("hidden");
            _IdentifyWinnersTabInfo.LoadControls();
            SetElectionHeading(HeadingIdentifyWinners);
            FeedbackIdentifyWinners.AddInfo($"{IdentifyWinnersTabItem.OfficeCount} offices loaded.");
          }
        }
          break;

        case "":
        {
          // normal update
          _IdentifyWinnersTabInfo.Update(FeedbackIdentifyWinners);
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}