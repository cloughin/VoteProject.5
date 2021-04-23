using System;
using System.Web;
using DB.Vote;
using static System.String;

namespace Vote
{
  public class LocationCookies
  {
    // ReSharper disable once NotAccessedField.Global
    public string Address;

    public string State;
    public string Congress;
    public string StateSenate;
    public string StateHouse;
    public string County;
    public string District;
    public string Place;
    public string Elementary;
    public string Secondary;
    public string Unified;
    public string CityCouncil;
    public string CountySupervisors;
    public string SchoolDistrictDistrict;
    public string Geo;

    private static string ValueOrNull(HttpCookie cookie)
    {
      return cookie?.Value;
    }

    public static LocationCookies GetCookies()
    {
      var httpContext = HttpContext.Current;
      if (httpContext == null)
        throw new ApplicationException("HttpContext is not available.");
      var request = httpContext.Request;

      // ReSharper disable once JoinDeclarationAndInitializer
      LocationCookies result;
      result = new LocationCookies
      {
        Address = ValueOrNull(request.Cookies["Address"]),
        State = ValueOrNull(request.Cookies["State"]),
        Congress = ValueOrNull(request.Cookies["Congress"]),
        StateSenate = ValueOrNull(request.Cookies["StateSenate"]),
        StateHouse = ValueOrNull(request.Cookies["StateHouse"]),
        County = ValueOrNull(request.Cookies["County"]),
        District = ValueOrNull(request.Cookies["District"]),
        Place = ValueOrNull(request.Cookies["Place"]),
        Elementary = ValueOrNull(request.Cookies["Elementary"]),
        Secondary = ValueOrNull(request.Cookies["Secondary"]),
        Unified = ValueOrNull(request.Cookies["Unified"]),
        CityCouncil = ValueOrNull(request.Cookies["CityCouncil"]),
        CountySupervisors = ValueOrNull(request.Cookies["CountySupervisors"]),
        SchoolDistrictDistrict = ValueOrNull(request.Cookies["SchoolDistrictDistrict"]),
        Geo = ValueOrNull(request.Cookies["Geo"])
      };

      return result;
    }

    public bool IsValid => !IsNullOrWhiteSpace(State) && !IsNullOrWhiteSpace(Congress) &&
      !IsNullOrWhiteSpace(StateSenate) && Offices.IsValidStateHouseDistrict(StateHouse, State) &&
      !IsNullOrWhiteSpace(County) && District != null && Place != null &&
      !IsNullOrWhiteSpace(Geo) && PublicMasterPage.IsCookieTrusted;
  }
}