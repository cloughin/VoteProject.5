using System;
using static System.String;

namespace Vote.Politician
{
  public partial class DefaultPage : VotePage
  {
    #region Dead code

    protected override void OnPreInit(EventArgs e)
    {
      {
        var query = Request.QueryString.ToString();
        var url = "/politician/main.aspx";
        if (!IsNullOrWhiteSpace(query))
          url += "?" + query;
        Response.Redirect(url);
      }
      base.OnPreInit(e);
    }

    #endregion Dead code
  }
}