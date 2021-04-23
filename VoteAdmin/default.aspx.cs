using System;
using System.Web.UI;

namespace VoteAdmin
{
  public partial class DefaultPage : Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      // ReSharper disable once Html.PathError
      Response.Redirect("/signinredirect.aspx");
    }
  }
}