using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using VoteLibrary.Utility;
using static System.String;

namespace Vote.Controls
{
  public partial class OfficeControl : UserControl
  {
    private static void CreateRelatedJurisdictionsNode(Control parent, string label,
      string href, string tab, string stateCode, string countyCode, string localKey)
    {
      href += "?state=" + stateCode;
      if (!IsNullOrWhiteSpace(countyCode)) href += "&county=" + countyCode;
      if (!IsNullOrWhiteSpace(localKey)) href += "&local=" + localKey;
      if (!IsNullOrWhiteSpace(tab)) href += "#" + tab;
      var data = "addClass:'jurisdiction-name',hideCheckbox:true,href:'" + href + "'";
      var officeNode = new HtmlLi {InnerHtml = label}.AddTo(parent);
      officeNode.Attributes.Add("data", data);
    }

    private static string MakeJurisdictionName(string name, string code, string template)
    {
      if (!IsNullOrWhiteSpace(name)) return name;
      return Format(template, code);
    }

    public static Control CreateRelatedJurisdictionsNodes(string href, string tab,
      string stateCode, string countyCode, string localKey)
    {
      if (!StateCache.IsValidStateCode(stateCode)) return new PlaceHolder();
      var mainNode = new HtmlLi {InnerHtml = "Related Jurisdictions"};
      var data =
        "addClass:'related-jurisdictions office-class',hideCheckbox:true,unselectable:true";
      // start this node expanded if county or local
      if (!IsNullOrWhiteSpace(countyCode) || !IsNullOrWhiteSpace(localKey))
        data += ", expand:true";
      mainNode.Attributes.Add("data", data);
      var subTree = new HtmlUl().AddTo(mainNode);
      if (!IsNullOrWhiteSpace(localKey))
      {
        // For Locals, the state, the county, and all other locals
        CreateRelatedJurisdictionsNode(subTree, StateCache.GetStateName(stateCode), href,
          tab, stateCode, Empty, Empty);
        CreateRelatedJurisdictionsNode(subTree,
          MakeJurisdictionName(CountyCache.GetCountyName(stateCode, countyCode), countyCode,
            "County {0}"), href, tab, stateCode, countyCode, Empty);
        foreach (var l in LocalDistricts.GetLocalsForCounty(stateCode, countyCode).Rows
          .OfType<DataRow>().OrderBy(l => l.LocalDistrict(), new AlphanumericComparer()))
          if (l.LocalKey() != localKey)
            CreateRelatedJurisdictionsNode(subTree,
              MakeJurisdictionName(l.LocalDistrict(), l.LocalKey(), "Local District {0}"),
              href, tab, stateCode, countyCode, l.LocalKey());
      }
      else if (!IsNullOrWhiteSpace(countyCode))
      {
        // For Counties, the state and all other counties (in sub-tree) plus all locals for the county
        CreateRelatedJurisdictionsNode(subTree, StateCache.GetStateName(stateCode), href,
          tab, stateCode, Empty, Empty);
        var subNode = new HtmlLi {InnerHtml = "Counties"};
        const string subData =
          "addClass:'office-class',hideCheckbox:true,unselectable:true";
        subNode.Attributes.Add("data", subData);
        subNode.AddTo(subTree);
        var subSubTree = new HtmlUl().AddTo(subNode);
        foreach (var c in CountyCache.GetCountiesByState(stateCode))
          if (c != countyCode)
            CreateRelatedJurisdictionsNode(subSubTree,
              MakeJurisdictionName(CountyCache.GetCountyName(stateCode, c), c,
                "County {0}"), href, tab, stateCode, c, Empty);
        foreach (var l in LocalDistricts.GetLocalsForCounty(stateCode, countyCode).Rows
          .OfType<DataRow>().OrderBy(l => l.LocalDistrict(), new AlphanumericComparer()))
          CreateRelatedJurisdictionsNode(subTree,
            MakeJurisdictionName(l.LocalDistrict(), l.LocalKey(), "Local District {0}"),
            href, tab, stateCode, countyCode, l.LocalKey());
      }
      else
      {
        // For State, a list of counties
        foreach (var c in CountyCache.GetCountiesByState(stateCode))
          CreateRelatedJurisdictionsNode(subTree,
            MakeJurisdictionName(CountyCache.GetCountyName(stateCode, c), c, "County {0}"),
            href, tab, stateCode, c, Empty);
      }

      return mainNode;
    }

    public string ExpandedNode
    {
      set { SelectOfficeExpandedNode.Value = value; }
    }

    public string Message
    {
      get { return FieldLabelMessage.InnerHtml; }
      set { FieldLabelMessage.InnerHtml = value; }
    }

    public string OfficeKey
    {
      get { return SelectedOfficeKey.Value; }
      set { SelectedOfficeKey.Value = value; }
    }

    public Control OfficeTree => PlaceHolderSelectOfficeTree;

    public static int PopulateOfficeTree(IList<DataRow> table, Control root,
      string stateCode, bool withCheckboxes = false, bool includeVirtual = false,
      bool integrateVirtual = false, bool stateLevel = false,
      Control relatedJurisdictionsNodes = null)
    {
      root.Controls.Clear();
      var tree = new HtmlUl().AddTo(root);
      var officeCount = 0;

      // ReSharper disable once ImplicitlyCapturedClosure
      void CreateNode(Control parent, DataRow office,
        bool useLine2Only = false, bool isTemplates = false)
      {
        useLine2Only &= office.OfficeClass() != OfficeClass.USPresident;
        var text = useLine2Only && !IsNullOrWhiteSpace(office.OfficeLine2())
          ? office.OfficeLine2()
          : Offices.FormatOfficeName(office);
        var candidateCount = office.Table.Columns.Contains("CandidateCountForOffice")
          ? office.CandidateCountForOffice()
          : 0;
        var isOfficeInElection = office.Table.Columns.Contains("IsOfficeInElection") &&
          office.IsOfficeInElection();
        int? positions = null;
        if (office.Table.Columns.Contains("ElectionType") &&
          office.Table.Columns.Contains("ElectionPositions") &&
          office.Table.Columns.Contains("PrimaryPositions") &&
          office.Table.Columns.Contains("PrimaryRunoffPositions") &&
          office.Table.Columns.Contains("GeneralRunoffPositions"))
          positions = Elections.GetOfficePositions(office);
        var classes = "office-name";
        if (isTemplates) classes += " template-node";
        if (office.IsInactiveOptional())
        {
          classes += " inactive-node";
          text += " (inactive)";
        }
        var data = "key:'" + office.OfficeKey() + "',addClass:'" + classes + "',desc:'" +
          Offices.FormatOfficeName(office.OfficeKey()).JavascriptEscapeString() + "'";
        if (candidateCount > 0 || isOfficeInElection)
        {
          text = $"{text} [{candidateCount}]";
          if (withCheckboxes) data += ",candidates:" + candidateCount;
        }
        var officeNode = new HtmlLi { InnerHtml = text, ID = "officekey-" + office.OfficeKey() }
          .AddTo(parent);
        if (withCheckboxes && isOfficeInElection) data += ",select:true";
        //disable "undeletable"
        //if (office.Undeletable()) data += ",undeletable:true";
        if (positions != null) data += ",positions:" + positions;
        officeNode.Attributes.Add("data", data);
      }

      void CreateNodes(IEnumerable<IGrouping<OfficeClass, DataRow>> classes, 
        bool isTemplates = false)
      {
        foreach (var officeClass in classes)
        {
          var offices = officeClass.OrderBy(row => Offices.FormatOfficeName(row),
            MixedNumericComparer.Instance).ToList();
          // If there is only one office in the class, don't create a class node
          // Also, no class node for county and local (indicated by OfficeClass.Undefined)
          officeCount += offices.Count;
          if (offices.Count == 1 && !isTemplates || officeClass.Key == OfficeClass.Undefined)
            foreach (var office in offices)
              CreateNode(tree, office, withCheckboxes);
          else
          {
            bool useLine2Only;
            switch (officeClass.Key)
            {
              case OfficeClass.USSenate:
              case OfficeClass.USHouse:
              case OfficeClass.StateSenate:
              case OfficeClass.StateHouse:
                {
                  // For these office classes, if all OfficeLine1's are identical and no 
                  // OfficeLine2's are blank, don't the  OfficeLine1's
                  var hasVariedLine1 =
                    offices.Exists(row => row.OfficeLine1() != offices[0].OfficeLine1());
                  useLine2Only = !hasVariedLine1 &&
                    !offices.Exists(row => IsNullOrWhiteSpace(row.OfficeLine2()));
                  break;
                }

              default:
                useLine2Only = false;
                break;
            }
            var text = Offices.GetOfficeClassShortDescription(officeClass.Key, stateCode);
            if (isTemplates) text += " templates";
            var classNode = new HtmlLi { InnerHtml = text }.AddTo(tree);
            var classNames = "office-class";
            if (isTemplates) classNames += " template-node";
            if (offices.All(o => o.IsInactiveOptional())) classNames += " inactive-node";
            var data = "addClass:'" + classNames + "'";
            if (!withCheckboxes) data += ",unselectable:true";
            classNode.Attributes.Add("data", data);
            var classSubTree = new HtmlUl().AddTo(classNode);
            foreach (var office in offices)
              CreateNode(classSubTree, office, useLine2Only, isTemplates);
          }
        }
      }

      // exclude virtual unless integrated
      var officeClasses = table.Where(row => !row.IsVirtual() || integrateVirtual)
        .GroupBy(row => stateLevel ? row.OfficeClass() : OfficeClass.Undefined).ToList();
      if (officeClasses.Count == 0)
        new HtmlLi {InnerHtml = "No offices were found for this jurisdiction"}.AddTo(tree);
      else
        CreateNodes(officeClasses);

      if (includeVirtual)
      {
        if (!table.Any(row => row.IsVirtual()))
        {
          new HtmlLi {InnerHtml = "No county or local office templates for this state"}
            .AddTo(tree);
        }
        else
        {
          var countyTemplates = table
            .Where(row => row.IsVirtual() &&
              IsNullOrWhiteSpace(Offices.GetLocalKeyFromKey(row.OfficeKey())))
            .GroupBy(row => row.OfficeClass()).ToList();
          CreateNodes(countyTemplates, true);

          var localTemplates = table
            .Where(row => row.IsVirtual() &&
              !IsNullOrWhiteSpace(Offices.GetLocalKeyFromKey(row.OfficeKey())))
            .GroupBy(row => row.OfficeClass()).ToList();
          CreateNodes(localTemplates, true);
        }
      }

      relatedJurisdictionsNodes?.AddTo(tree);

      return officeCount;
    }

    public bool ShowSelectOfficePanel
    {
      set { ShowSelectOfficePanelPrivate.Value = value.ToString().ToLowerInvariant(); }
    }

    public void Update() => UpdatePanelSelectOffice.Update();

    protected void Page_Load(object sender, EventArgs e)
    {
    }
  }
}