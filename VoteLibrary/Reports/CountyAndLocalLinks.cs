using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB.Vote;
using VoteLibrary.Utility;
using static System.String;

namespace Vote.Reports
{
  public class CountyAndLocalLinks : Report
  {
    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global

    #endregion ReSharper disable

    protected const int MaxCellsInRow = 4;

    protected static HtmlAnchor CreateAdminDefaultAnchor(string stateCode,
      string countyCode, string localKey, string anchorText, string title = "",
      string target = "")
    {
      return new HtmlAnchor
      {
        HRef = SecureAdminPage.GetDefaultPageUrl(stateCode, countyCode, localKey),
        InnerHtml = anchorText,
        Title = title,
        Target = target
      };
    }

    #region ReSharper restore

    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected
  }

  // ReSharper disable ClassNeverInstantiated.Global
  public sealed class CountyLinks : CountyAndLocalLinks
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

    public static void AddLinks(Control parent, string stateCode,
      Func<string, string, string, Control> getAnchor, bool sortByCode = false)
    {
      var counties = CountyCache.GetCountiesByState(stateCode)
        .AsEnumerable();
      if (sortByCode) counties = counties.OrderBy(code => code);
      foreach (var countyCode in counties)
        getAnchor(stateCode, countyCode, CountyCache.GetCountyName(stateCode, countyCode))
          .AddTo(parent);
    }

    public static void AddDefaultCountyLinks(Control parent, string stateCode,
      bool sortByCode = false)
    {
      AddLinks(parent, stateCode,
        (s, c, name) => CreateAdminDefaultAnchor(s, c, null, name), sortByCode);
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
  public sealed class LocalLinks : CountyAndLocalLinks
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

    public static void AddLinks(Control parent, string stateCode, string countyCode,
      Func<string, string, string, string, string, Control> getAnchor)
    {
      var localsDictionary = LocalDistricts.GetNamesDictionary(stateCode, countyCode)
       .OrderBy(kvp => kvp.Value, new AlphanumericComparer()).ToArray();
      var otherCounties = LocalIdsCodes.FormatOtherCountyNamesDictionary(stateCode,
        countyCode, localsDictionary.Select(l => l.Key));
      foreach (var local in localsDictionary)
        getAnchor(stateCode, countyCode, local.Key, local.Value, otherCounties[local.Key])
          .AddTo(parent);
    }

    public static void AddDefaultLocalLinks(Control parent, string stateCode,
      string countyCode)
    {
      AddLinks(parent, stateCode, countyCode,
        (s, c, l, name, otherCounties) => 
        CreateAdminDefaultAnchor(s, c, l, name +
        (IsNullOrWhiteSpace(otherCounties) 
          ? Empty 
          : " (also in " + otherCounties + ")" )));
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