using System;
using System.Web.Security;

namespace Vote
{
  public partial class SigninRedirectPage : SecurePage, ISignInPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var url = FormsAuthentication.LoginUrl;

      switch (UserSecurityClass)
      {
        case MasterSecurityClass:
          url = "/Master/default.aspx";
          break;

        case StateAdminSecurityClass:
        case CountyAdminSecurityClass:
        case LocalAdminSecurityClass:
          url = "/Admin/default.aspx";
          break;

        case PartySecurityClass:
          url = "/Party/default.aspx";
          break;

        case PoliticianSecurityClass:
          url = "/Politician/main.aspx";
          break;
      }

      Response.Redirect(url);
    }
  }
}