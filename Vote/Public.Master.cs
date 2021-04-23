using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Vote
{
  public partial class PublicMasterPage : MasterPage
  {
    public static bool IsCookieTrusted
    {
      get
      {
        // to force re-cookieing after redistricting
        // * set the date below at least 30 days in the future (31 to be safe) (from when the new shapefiles were published)
        // * test for the states to which it should apply

        // don't trust NC cookies until 11/19/2018

        var stateCookie = HttpContext.Current?.Request.Cookies["State"];
        if (stateCookie == null) return false;

        // for all states skip this test;
        // for one state: if (stateCookie.Value != "PA") return true;
        //if (!new []{"PA","NJ","MD","VA"}.Contains(stateCookie.Value)) return true;
        if (stateCookie.Value != "NC") return true;

        var expCookie = HttpContext.Current?.Request.Cookies["CDate"];
        if (expCookie == null) return false; // no or very old cookie
        if (!DateTime.TryParse(HttpUtility.UrlDecode(expCookie.Value), out var exp))
          return false;
        return exp > new DateTime(2018, 11, 19);
      }
    }

    public void ShowDisclaimer()
    {
      Body.RemoveCssClass("disclaimer-hidden");
      Body.AddCssClasses("disclaimer-shown");
      Disclaimer.Visible = true;
    }

    public static string StateName
    {
      get
      {
        var stateCode = UrlManager.GetStateCodeFromHostName();
        if (StateCache.IsValidStateCode(stateCode))
          return StateCache.GetStateName(stateCode);
        return "US";
      }
    }

    public static string SiteName
    {
      get
      {
        var stateCode = UrlManager.GetStateCodeFromHostName();
        if (StateCache.IsValidStateCode(stateCode))
          return $"Vote-{stateCode}";
        return "Vote-USA";
      }
    }

    private void SetupMainBanner()
    {
      //LogoName.InnerHtml = $"{DomainDesign.FromQueryStringOrDomain.Replace("-", " ")} <span>Since 2004</span>";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var page = Regex.Match(Request.Path,
        @"^/(?<page>[^.]*)\.aspx$", RegexOptions.IgnoreCase).Groups["page"].Value;
      Body.AddCssClasses(page.ToLowerInvariant() + "-page");
      SetupMainBanner();
    }
  }
}