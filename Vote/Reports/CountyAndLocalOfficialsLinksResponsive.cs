using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Reports
{
  internal class CountyAndLocalOfficialsLinksResponsive : Report
  {
    #region Private

    protected static HtmlAnchor CreatePublicOfficalsAnchor(string anchorText,
      string stateOrFederalCode, string countyCode = "", string localCode = "")
    {
      var a = new HtmlAnchor
      {
        HRef =
          UrlManager.GetOfficialsPageUri(stateOrFederalCode, countyCode, localCode)
            .ToString(),
        InnerHtml = anchorText
      };
      return a;
    }

    #endregion Private
  }

// ReSharper disable ClassNeverInstantiated.Global
  internal class CountyOfficialsLinksResponsive : CountyAndLocalOfficialsLinksResponsive
    // ReSharper restore ClassNeverInstantiated.Global
  {
    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static void GetReport(Control container, string stateCode)
    {
      // We get a dictionary of office counts by county
      // Key: countyCode; Value: count of offices
      // Counties with no offices will not be in the dictionary
      var countCountyOfficesDictionary =
        Offices.CountCountyOfficesByCounty(stateCode);
      if (countCountyOfficesDictionary.Count == 0) return;

      new HtmlDiv {InnerText = "County Elected Representatives"}.AddTo(container, "accordion-header");
      var content = new HtmlDiv().AddTo(container, "local-anchors accordion-content");

      // For reporting we start with all counties for the state (it will be in
      // order by county name). Then we select only those in the offices dictionary.
      var reorderedCounties = CountyCache.GetCountiesByState(stateCode)
        .Where(countCountyOfficesDictionary.ContainsKey)
        .ToList();

      foreach (var iteratedCountyCode in reorderedCounties)
      {
        var countyName = CountyCache.GetCountyName(stateCode, iteratedCountyCode);
        CreatePublicOfficalsAnchor(countyName, stateCode, iteratedCountyCode)
          .AddTo(content, "local-anchor");
      }
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }

// ReSharper disable ClassNeverInstantiated.Global
  internal class LocalOfficialsLinksResponsive : CountyAndLocalOfficialsLinksResponsive
    // ReSharper restore ClassNeverInstantiated.Global
  {
    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static void GetReport(Control container, string stateCode, string countyCode)
    {
      // Get a dictionary of all locals with offices defined
      // Key: localCode; Value: localDistrictName
      var localNamesWithOfficesDictionary =
        Offices.GetLocalNamesWithOffices(stateCode, countyCode);
      if (localNamesWithOfficesDictionary.Count == 0) return;

      // For reporting, we sort the dictionary by name
      var sortedListOflocalNamesWithOffices = localNamesWithOfficesDictionary
        .OrderBy(kvp => kvp.Value)
        .Select(kvp => kvp.Key)
        .ToList();

      new HtmlDiv {InnerText = "Local District Elected Representatives"}.AddTo(container,
        "accordion-header");
      var content = new HtmlDiv().AddTo(container, "local-anchors accordion-content");

      foreach (var iteratedLocalCode in sortedListOflocalNamesWithOffices)
      {
        var localName = localNamesWithOfficesDictionary[iteratedLocalCode];
        CreatePublicOfficalsAnchor(localName, stateCode, countyCode, iteratedLocalCode)
          .AddTo(content, "local-anchor");
      }
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}