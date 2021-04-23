using System;
using System.Web;

namespace Vote
{
  // This contains the final info from the LDS table.
  // We make this an immutable object because it goes in a Dictionary
  // as a key. We also override the equality operators and GetHashCode 
  // for the same reason.
  public class LdsInfo
  {
    public LdsInfo(string stateCode, string congress, string stateSenate,
      string stateHouse, string county)
    {
      if ((stateCode == null) || (congress == null) || (stateSenate == null) ||
        (stateHouse == null) || (county == null))
        throw new ArgumentNullException();

      if ((congress.Length > 3) || (stateSenate.Length > 3) ||
        (stateHouse.Length > 3) || (county.Length > 3) || (stateCode.Length != 2))
        throw new ArgumentException();

      StateCode = stateCode;
      Congress = congress.ZeroPad(3);
      StateSenate = stateSenate.ZeroPad(3);
      StateHouse = stateHouse.ZeroPad(3);
      County = county.ZeroPad(3);
    }

    public void ApplyToResult(AddressFinderResult result)
    {
      result.Congress = Congress;
      result.StateSenate = StateSenate;
      result.StateHouse = StateHouse;
      result.County = County;
      result.State = StateCode;

      var votersUrl = MakeSetCookiesUrl(
        UrlManager.GetForVotersPageUri(StateCode), result.OriginalInput, result.Remember);
      result.RedirectUrl = votersUrl;
      result.SuccessMessage = "Redirecting to the " + StateCache.GetStateName(StateCode) +
        " 'for Voters' page";

      //string electedOfficialsUrl = MakeSetCookiesUrl(
      //  UrlManager.GetElectedPageUri(
      //  StateCode, Congress, StateSenate, StateHouse, County));

      //ElectionsTable futureElectionsTable =
      //  Elections.GetFutureViewableDisplayDataByStateCode(StateCode);

      //if (futureElectionsTable.Count == 0)
      //{
      //  result.RedirectUrl = electedOfficialsUrl;
      //  result.SuccessMessage = "Redirecting to your elected officials page";
      //}
      //else
      //{
      //  //redirectUri = UrlManager.GetBallotPageUri(StateCode,
      //  //  Congress, StateSenate, StateHouse, County);
      //  HtmlGenericControl span = new HtmlSpan();
      //  span.Attributes["class"] = "selectOne";
      //  span.Controls.Add(new LiteralControl("Select "));
      //  HtmlAnchor a = new HtmlAnchor();
      //  span.Controls.Add(a);
      //  a.HRef = electedOfficialsUrl;
      //  a.InnerText = "Current elected officials";
      //  a.Title = a.InnerText;
      //  span.Controls.Add(new LiteralControl(" or sample ballot for:"));
      //  foreach (var row in futureElectionsTable)
      //  {
      //    span.Controls.Add(new LiteralControl("<br />"));
      //    a = new HtmlAnchor();
      //    span.Controls.Add(a);
      //    Uri uri = UrlManager.GetBallotPageUri(row.ElectionKey,
      //      Congress, StateSenate, StateHouse, County);
      //    a.HRef = MakeSetCookiesUrl(uri);
      //    a.InnerText = row.ElectionDesc;
      //    a.Title = a.InnerText;
      //  }
      //  result.SuccessMessage = db.RenderToString(span);
      //}
    }

    private string MakeSetCookiesUrl(Uri uri, string address = null, bool remember = true)
    {
      var qsc = new QueryStringCollection
      {
        {"Congress", Congress},
        {"StateSenate", StateSenate},
        {"StateHouse", StateHouse},
        {"County", County},
        {"Page", HttpUtility.UrlEncode(uri.ToString())},
        {"Address", address}
      };
      if (!remember) qsc.Add("Remember", "n");
      return UrlManager.GetStateUri(StateCode, "SetCookies.aspx", qsc).ToString();
    }

    public string Congress { get; }

    public string County { get; }

    public bool IsMissing => this == Missing;

    public static LdsInfo Missing { get; } = new LdsInfo("XX", "000", "000", "000", "000");

    public string StateCode { get; }

    public string StateHouse { get; }

    public string StateSenate { get; }

    #region Override GetHashCode and equality methods

    public override int GetHashCode() =>
      StateCode.GetHashCode() ^
      Congress.GetHashCode() ^
      StateSenate.GetHashCode() ^
      StateHouse.GetHashCode() ^
      County.GetHashCode();

    public static bool Equals(LdsInfo x, LdsInfo y) =>
      (x.StateCode == y.StateCode) &&
      (x.Congress == y.Congress) &&
      (x.StateSenate == y.StateSenate) &&
      (x.StateHouse == y.StateHouse) &&
      (x.County == y.County);

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to LdsInfo return false.
      var other = obj as LdsInfo;
      return ((object) other != null) && Equals(this, other);

      // Return true if the fields match:
    }

    //public bool Equals(LdsInfo other)
    //{
    //  // If parameter is null return false:
    //  if ((object) other == null) return false;

    //  // Return true if the fields match:
    //  return Equals(this, other);
    //}

    public static bool operator ==(LdsInfo a, LdsInfo b)
    {
      // If both are null, or both are same instance, return true.
      if (ReferenceEquals(a, b)) return true;

      // If one is null, but not both, return false.
      if (((object) a == null) || ((object) b == null))
        return false;

      // Return true if the fields match:
      return Equals(a, b);
    }

    public static bool operator !=(LdsInfo a, LdsInfo b) => !(a == b);

    #endregion Override GetHashCode and equality methods
  }
}