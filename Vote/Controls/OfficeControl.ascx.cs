using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;

namespace Vote.Controls
{
  public partial class OfficeControl : UserControl
  {
    private static void CreateRelatedJurisdictionsNode(Control parent, string label, string href,
      string tab, string stateCode, string countyCode, string localCode)
    {
      href += "?state=" + stateCode;
      if (!string.IsNullOrWhiteSpace(countyCode)) href += "&county=" + countyCode;
      if (!string.IsNullOrWhiteSpace(localCode)) href += "&local=" + localCode;
      if (!string.IsNullOrWhiteSpace(tab)) href += "#" + tab;
      var data = "addClass:'jurisdiction-name',hideCheckbox:true,href:'" + href + "'";
      var officeNode =
        new HtmlLi
        {
          InnerHtml = label
        }.AddTo(parent);
      officeNode.Attributes.Add("data", data);
    }

    private static string MakeJurisdictionName(string name, string code, string template)
    {
      if (!string.IsNullOrWhiteSpace(name)) return name;
      return string.Format(template, code);
    }

    public static Control CreateRelatedJurisdictionsNodes(string href, string tab, string stateCode,
      string countyCode, string localCode)
    {
      if (!StateCache.IsValidStateCode(stateCode))
        return new PlaceHolder();
      var mainNode =
        new HtmlLi {InnerHtml = "Related Jurisdictions"};
      var data = "addClass:'related-jurisdictions office-class',hideCheckbox:true,unselectable:true";
      // start this node expanded if county or local
      if (!string.IsNullOrWhiteSpace(countyCode))
        data += ", expand:true";
      mainNode.Attributes.Add("data", data);
      var subTree = new HtmlUl().AddTo(mainNode);
      if (!string.IsNullOrWhiteSpace(localCode))
      {
        // For Locals, the state, the county, and all other locals
        CreateRelatedJurisdictionsNode(subTree, StateCache.GetStateName(stateCode),
          href, tab, stateCode, string.Empty, string.Empty);
        CreateRelatedJurisdictionsNode(subTree,
          MakeJurisdictionName(CountyCache.GetCountyName(stateCode, countyCode), countyCode,
            "County {0}"),
          href, tab, stateCode, countyCode, string.Empty);
        foreach (var l in LocalDistricts.GetNamesDataByStateCodeCountyCode(stateCode, countyCode)
          .OrderBy(l => l.LocalDistrict, StringComparer.OrdinalIgnoreCase))
          if (l.LocalCode != localCode)
            CreateRelatedJurisdictionsNode(subTree,
              MakeJurisdictionName(l.LocalDistrict, l.LocalCode, "Local District {0}"),
              href, tab, stateCode, countyCode, l.LocalCode);
      }
      else if (!string.IsNullOrWhiteSpace(countyCode))
      {
        // For Counties, the state and all other counties (in sub-tree) plus all locals for the county
        CreateRelatedJurisdictionsNode(subTree, StateCache.GetStateName(stateCode),
          href, tab, stateCode, string.Empty, string.Empty);
        var subNode =
          new HtmlLi {InnerHtml = "Counties"};
        const string subData = "addClass:'office-class',hideCheckbox:true,unselectable:true";
        subNode.Attributes.Add("data", subData);
        subNode.AddTo(subTree);
        var subSubTree = new HtmlUl().AddTo(subNode);
        foreach (var c in CountyCache.GetCountiesByState(stateCode))
          if (c != countyCode)
            CreateRelatedJurisdictionsNode(subSubTree,
              MakeJurisdictionName(CountyCache.GetCountyName(stateCode, c), c, "County {0}"),
              href, tab, stateCode, c, string.Empty);
        foreach (var l in LocalDistricts.GetNamesDataByStateCodeCountyCode(stateCode, countyCode)
          .OrderBy(l => l.LocalDistrict, StringComparer.OrdinalIgnoreCase))
          CreateRelatedJurisdictionsNode(subTree,
            MakeJurisdictionName(l.LocalDistrict, l.LocalCode, "Local District {0}"),
            href, tab, stateCode, countyCode, l.LocalCode);
      }
      else
      {
        // For State, a list of counties
        foreach (var c in CountyCache.GetCountiesByState(stateCode))
          CreateRelatedJurisdictionsNode(subTree,
            MakeJurisdictionName(CountyCache.GetCountyName(stateCode, c), c, "County {0}"),
            href, tab, stateCode, c, string.Empty);
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

    public static int PopulateOfficeTree(IList<DataRow> table, Control parent, string stateCode,
      bool withCheckboxes = false, bool includeVirtual = false, bool integrateVirtual = false,
      bool stateLevel = false, Control relatedJurisdictionsNodes = null)
    {
      parent.Controls.Clear();
      var tree = new HtmlUl().AddTo(parent);
      var officeCount = 0;

      // exclude virtual unless integrated
      var officeClasses = table
        .Where(row => !row.IsVirtual() || integrateVirtual)
        .GroupBy(row => stateLevel ? row.OfficeClass() : OfficeClass.Undefined).ToList();
      if (officeClasses.Count == 0)
      {
        new HtmlLi
        {
          InnerHtml = "No offices were found for this jurisdiction"
        }.AddTo(tree);
      }
      else
        officeCount = PopulateOfficeTree_CreateNodes(stateCode, withCheckboxes, officeClasses,
          officeCount, tree);

      if (includeVirtual)
      {
        if (!table.Any(row => row.IsVirtual()))
        {
          new HtmlLi
          {
            InnerHtml = "No county or local office templates for this state"
          }.AddTo(tree);
        }
        else
        {
          var countyTemplates = table
            .Where(row => row.IsVirtual()
              && string.IsNullOrWhiteSpace(Offices.GetLocalCodeFromKey(row.OfficeKey())))
            .GroupBy(row => row.OfficeClass()).ToList();
          officeCount = PopulateOfficeTree_CreateNodes(stateCode, withCheckboxes, countyTemplates,
            officeCount, tree, true);

          var localTemplates = table
            .Where(row => row.IsVirtual()
              && !string.IsNullOrWhiteSpace(Offices.GetLocalCodeFromKey(row.OfficeKey())))
            .GroupBy(row => row.OfficeClass()).ToList();
          officeCount = PopulateOfficeTree_CreateNodes(stateCode, withCheckboxes, localTemplates,
            officeCount, tree, true);
        }
      }

      relatedJurisdictionsNodes?.AddTo(tree);

      return officeCount;
    }

    private static int PopulateOfficeTree_CreateNodes(string stateCode, bool withCheckboxes,
      IEnumerable<IGrouping<OfficeClass, DataRow>> officeClasses, int officeCount, Control tree,
      bool isTemplates = false)
    {
      foreach (var officeClass in officeClasses)
      {
        var offices = officeClass
          .OrderBy(row => Offices.FormatOfficeName(row), MixedNumericComparer.Instance)
          .ToList();
        // If there is only one office in the class, don't create a class node
        // Also, no class node for county and local (indicated by OfficeClass.Undefined)
        officeCount += offices.Count;
        if (((offices.Count == 1) && !isTemplates) || (officeClass.Key == OfficeClass.Undefined))
          foreach (var office in offices)
            PopulateOfficeTree_CreateNode(tree, office, withCheckboxes);
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
                !offices.Exists(row => string.IsNullOrWhiteSpace(row.OfficeLine2()));
              break;
            }

            default:
              useLine2Only = false;
              break;
          }
          var text = Offices.GetOfficeClassShortDescription(officeClass.Key,
            stateCode);
          if (isTemplates) text += " templates";
          var classNode =
            new HtmlLi {InnerHtml = text}.AddTo(tree);
          var classes = "office-class";
          if (isTemplates) classes += " template-node";
          if (offices.All(o => o.IsInactiveOptional())) classes += " inactive-node";
          var data = "addClass:'" + classes + "'";
          if (!withCheckboxes)
            data += ",unselectable:true";
          classNode.Attributes.Add("data", data);
          var classSubTree = new HtmlUl().AddTo(classNode);
          foreach (var office in offices)
            PopulateOfficeTree_CreateNode(classSubTree, office, withCheckboxes,
              useLine2Only, isTemplates);
        }
      }
      return officeCount;
    }

    private static void PopulateOfficeTree_CreateNode(Control parent,
      DataRow office, bool withCheckboxes, bool useLine2Only = false, bool isTemplates = false)
    {
      var text = useLine2Only && !string.IsNullOrWhiteSpace(office.OfficeLine2())
        ? office.OfficeLine2()
        : Offices.FormatOfficeName(office);
      //if (isTemplates) text += " template";
      var candidateCount = office.Table.Columns.Contains("CandidateCountForOffice")
        ? office.CandidateCountForOffice()
        : 0;
      var classes = "office-name";
      if (isTemplates) classes += " template-node";
      if (office.IsInactiveOptional())
      {
        classes += " inactive-node";
        text += " (inactive)";
      }
      var data = "key:'" + office.OfficeKey() + "',addClass:'" + classes + "',desc:'" +
        Offices.FormatOfficeName(office.OfficeKey()).JavascriptEscapeString() + "'";
      if (candidateCount > 0)
      {
        text = $"{text} [{candidateCount}]";
        if (withCheckboxes)
          data += ",candidates:" + candidateCount;
      }
      var officeNode =
        new HtmlLi
        {
          InnerHtml = text,
          ID = "officekey-" + office.OfficeKey()
        }.AddTo(parent);
      if (withCheckboxes && office.IsOfficeInElection())
        data += ",select:true";
      officeNode.Attributes.Add("data", data);
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