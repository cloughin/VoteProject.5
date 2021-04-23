using System;
using System.Web.UI;
using static System.String;

namespace Vote.Controls
{
  public partial class AdminHeadingControl : UserControl
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      try
      {
        var name = Empty;
        if (SecurePage.IsPoliticianUser && !IsNullOrWhiteSpace(VotePage.UserName))
        {
          var votePage = VotePage.Current;
          if (votePage != null)
            name = votePage.PageCache.Politicians.GetPoliticianName(VotePage.UserName);
        }
        MainBannerHomeLink.HRef = UrlManager.GetSiteUri().ToString();
        if (name == Empty) name = VotePage.UserName;
        if (IsNullOrWhiteSpace(name))
          LabelLogin.Text =
            "<span class=\"logged-in-as\"><a href=\"/SignIn.aspx\">Sign in</a></span>";
        else
          LabelLogin.Text =
            $"<span class=\"logged-in-as\">Signed in as: <span class=\"name\">{name}" +
            "</span>&nbsp;&nbsp;&nbsp;<a href=\"/SignIn.aspx?Signout=Y\">Sign out</a></span>";
      }
      catch (Exception ex)
      {
        VotePage.Log404Error("AdminHeading.aspx: " + ex.Message);
        if (!VotePage.IsDebugging) VotePage.SafeTransferToError500();
      }
    }
  }
}