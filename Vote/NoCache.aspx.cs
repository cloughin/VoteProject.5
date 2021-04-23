using System;
using System.Web;
using System.Web.UI;

namespace Vote
{
  public partial class NoCachePage : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var message = "NoCache set for 1 day";
      if (Request.QueryString.ToString() == "P")
      {
        var cookie = new HttpCookie("pnocache") {Expires = DateTime.Now.AddYears(20)};
        Response.Cookies.Add(cookie);
        message = "NoCache set";
      }
      else
      {
        var cookie = new HttpCookie("nocache") {Expires = DateTime.Now.AddDays(1)};
        Response.Cookies.Add(cookie);
      }
      PlaceHolder.Controls.Add(new LiteralControl(message));
    }
  }
}