using System.Web;

namespace Vote
{
  // These classes might be reorganized and perhaps renamed later.
  //
  public static class DomainData
  {
    public static string FromQueryStringOrDomain
    {
      get
      {
        var dataCode = HttpContext.Current.Request.QueryString["Data"];
        if (!string.IsNullOrWhiteSpace(dataCode))
          return dataCode.ToUpperInvariant();

        var stateCode = HttpContext.Current.Request.QueryString["State"];
        return !string.IsNullOrWhiteSpace(stateCode)
          ? stateCode.ToUpperInvariant()
          : UrlManager.CurrentDomainDataCode;
      }
    }

    private static bool IsDataCodeValidStateCode(string dataCode)
    {
      return StateCache.IsValidStateCode(dataCode);
    }

    public static bool IsValidStateCode => IsDataCodeValidStateCode(FromQueryStringOrDomain);
  }
}