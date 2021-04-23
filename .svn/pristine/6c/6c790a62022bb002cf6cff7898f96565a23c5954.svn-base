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
          url = "/Admin/default.aspx";
          break;

        case CountyAdminSecurityClass:
          url = "/Admin/default.aspx";
          break;

        case LocalAdminSecurityClass:
          url = "/Admin/default.aspx";
          break;

        case PartySecurityClass:
          url = "/Party/default.aspx";
          break;

        case PoliticianSecurityClass:
          //url = "/Politician/default.aspx";
          url = "/Politician/main.aspx";
          break;

        case DesignSecurityClass:
          url = "/Design/default.aspx";
          break;

        case OrganizationSecurityClass:
          url = "/Organization/default.aspx";
          break;
      }

      Response.Redirect(url);
    }
  }
}