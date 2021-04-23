using System;
using System.Text.RegularExpressions;
using Vote;
using static System.String;

namespace VoteAdmin.Controls
{
  public partial class SelectJurisdictionButton : System.Web.UI.UserControl
  {
    private SecureAdminPage _SecureAdminPage;

    public bool AddClasses { private get; set; }

    public string AdminPageName { private get; set; }

    public string Tooltip
    {
      set { ChangeJurisdictionButton.Attributes["title"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      _SecureAdminPage = VotePage.GetPage<SecureAdminPage>();
      if (_SecureAdminPage == null)
        throw new VoteException(
          "The SelectJurisdictionButton control can only be used in the" +
          " SecureAdminPage class");

      if (!IsPostBack)
      {
        Page.IncludeCss("~/css/vote/controls/SelectJurisdictionButton.css");

        if (IsNullOrWhiteSpace(AdminPageName))
        {
          var match = Regex.Match(VotePage.CurrentPath, @"/([^./]+)\.");
          if (!match.Success) throw new VoteException("Missing AdminPageName");
          AdminPageName = match.Groups[1].Value;
        }

        StateLink.NavigateUrl =
          SecureAdminPage.GetAdminFolderPageUrl(AdminPageName, "state",
            _SecureAdminPage.StateCode);
        StateLink.Text = "► " + StateCache.GetStateName(_SecureAdminPage.StateCode);

        switch (_SecureAdminPage.AdminPageLevel)
        {
          case AdminPageLevel.State:
            if (AddClasses) Container.AddCssClasses("state");
            break;

          case AdminPageLevel.County:
            StateLink.Visible = true;
            if (AddClasses) Container.AddCssClasses("county");
            break;

          case AdminPageLevel.Local:
            StateLink.Visible = true;
            if (AddClasses) Container.AddCssClasses("local");
            CountyLink.Visible = true;
            CountyLink.NavigateUrl = SecureAdminPage.GetAdminFolderPageUrl(AdminPageName,
              "state", _SecureAdminPage.StateCode, "county", _SecureAdminPage.CountyCode);
            CountyLink.Text = "► " +
              CountyCache.GetCountyName(_SecureAdminPage.StateCode,
                _SecureAdminPage.CountyCode);
            break;
        }
      }
    }
  }
}