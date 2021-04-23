using System;
using System.Web;

namespace Vote
{
  public class LocationCookies
  {
    public string Address;
    public string State;
    public string Congress;
    public string StateSenate;
    public string StateHouse;
    public string County;

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
      //if (string.IsNullOrWhiteSpace(ValueOrNull(request.Cookies["Vote"])))
      result = new LocationCookies
      {
        Address = ValueOrNull(request.Cookies["Address"]),
        State = ValueOrNull(request.Cookies["State"]),
        Congress = ValueOrNull(request.Cookies["Congress"]),
        StateSenate = ValueOrNull(request.Cookies["StateSenate"]),
        StateHouse = ValueOrNull(request.Cookies["StateHouse"]),
        County = ValueOrNull(request.Cookies["County"])
      };
      //else
      //{
      //  // ReSharper disable once PossibleNullReferenceException
      //  var cookies = request.Cookies["Vote"].Values;
      //  result = new LocationCookies
      //  {
      //    State = cookies["State"],
      //    Congress = cookies["Congress"],
      //    StateSenate = cookies["StateSenate"],
      //    StateHouse = cookies["StateHouse"],
      //    County = cookies["County"]
      //  };
      //}

      return result;
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(State) &&
      !string.IsNullOrWhiteSpace(Congress) &&
      !string.IsNullOrWhiteSpace(StateSenate) &&
      !string.IsNullOrWhiteSpace(StateHouse) &&
      !string.IsNullOrWhiteSpace(County);
  }
}