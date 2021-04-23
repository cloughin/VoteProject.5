using System;
using System.Diagnostics;
using System.Globalization;
using System.Web;
using System.Web.UI;
using DB.Vote;
using static System.String;

namespace Vote
{
  public partial class SetCookiesPage : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var congress = Request.QueryString["Congress"];
      var stateCode = Request.QueryString["State"];
      var stateSenate = Request.QueryString["StateSenate"];
      var stateHouse = Request.QueryString["StateHouse"];
      var county = Request.QueryString["County"];
      var district = Request.QueryString["District"];
      var place = Request.QueryString["Place"];
      var elementary = Request.QueryString["Esd"];
      var secondary = Request.QueryString["Ssd"];
      var unified = Request.QueryString["Usd"];
      var cityCouncil = Request.QueryString["Cc"];
      var countySupervisors = Request.QueryString["Cs"];
      var schoolDistrictDistrict = Request.QueryString["Sdd"];
      var geo = Request.QueryString["Geo"];
      var page = Request.QueryString["Page"];
      var address = Request.QueryString["Address"];
      var components = Request.QueryString["Components"];
      var remember = !Request.QueryString["Remember"].IsEqIgnoreCase("n");
      if (!IsNullOrWhiteSpace(congress) && !IsNullOrWhiteSpace(stateSenate) &&
        Offices.IsValidStateHouseDistrict(stateHouse, stateCode) && 
        !IsNullOrWhiteSpace(county) && !IsNullOrWhiteSpace(components) && 
        !IsNullOrWhiteSpace(geo) && !IsNullOrWhiteSpace(page))
      {
        var expiration = DateTime.UtcNow.AddDays(30); // 30 days

        var cookie = new HttpCookie("State", IsNullOrWhiteSpace(stateCode)
          ? DomainData.FromQueryStringOrDomain
          : stateCode);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("Congress", congress);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("StateSenate", stateSenate);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("StateHouse", stateHouse);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("County", county);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("District", district);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("Place", place);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("Elementary", elementary);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("Secondary", secondary);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("Unified", unified);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("CityCouncil", cityCouncil);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("CountySupervisors", countySupervisors);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("SchoolDistrictDistrict", schoolDistrictDistrict);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("Geo", geo);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("Address", address);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("Components", components);
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        cookie = new HttpCookie("CDate", HttpUtility.UrlEncode(expiration.ToString(CultureInfo.InvariantCulture)));
        if (remember) cookie.Expires = expiration;
        Response.Cookies.Add(cookie);

        if (UrlManager.CurrentDomainDataCode == "US")
        {
          var ub = new UriBuilder(UrlNormalizer.BuildCurrentUri())
          {
            Host = UrlManager.GetStateHostName(stateCode)
          };
          Response.Redirect(ub.ToString());
        }
        else
        {
          page = HttpUtility.UrlDecode(page);
          Debug.Assert(page != null, "page != null");
          Response.Redirect(page);
        }
      }
      else
        Response.Redirect("/404.aspx");
    }
  }
}