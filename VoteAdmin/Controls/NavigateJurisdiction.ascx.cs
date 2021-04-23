using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using DB.Vote;
using static System.String;

namespace Vote.Controls
{
  public partial class NavigateJurisdiction : UserControl
  {
    #region Private

    private SecureAdminPage _SecureAdminPage;
    private Dictionary<string, string> _NonStateNameDictionary;

    private string GetNonStateCodeName(string nonStateCode)
    {
      if (_NonStateNameDictionary?.ContainsKey(nonStateCode) == true)
        return _NonStateNameDictionary[nonStateCode];
      return StateCache.GetStateName(nonStateCode);
    }

    private void PopulateStateDropDown(bool includeSelectMessage = false)
    {
      if (includeSelectMessage)
        StateCache.Populate(StateDropDownList, "<select a state>", Empty);
      else
        StateCache.Populate(StateDropDownList, _SecureAdminPage.StateCode);

      var nonStateCodesAllowed = _SecureAdminPage.NonStateCodesAllowed;
      if (nonStateCodesAllowed == null) return;
      if (!SecurePage.IsSuperUser)
      {
        var nonStateCodesRequireSuperUser = _SecureAdminPage.NonStateCodesRequireSuperUser;
        if (nonStateCodesRequireSuperUser != null)
          nonStateCodesAllowed =
            nonStateCodesAllowed.Where(
              s => !nonStateCodesRequireSuperUser.Contains(s));
      }
      foreach (var nonState in nonStateCodesAllowed)
        StateDropDownList.AddItem(GetNonStateCodeName(nonState), nonState,
          nonState == _SecureAdminPage.StateCode);
    }

    private void PopulateCountyDropDown(bool includeSelectMessage = false)
    {
      if (includeSelectMessage)
        CountyCache.Populate(CountyDropDownList, _SecureAdminPage.StateCode,
          "<select a county>", Empty);
      else
        CountyCache.Populate(CountyDropDownList, _SecureAdminPage.StateCode,
          _SecureAdminPage.CountyCode);
    }

    private void PopulateLocalDropDown(bool includeSelectMessage = false)
    {
      bool hasLocals;
      if (includeSelectMessage)
        hasLocals = LocalDistricts.Populate(LocalDropDownList, _SecureAdminPage.StateCode,
          _SecureAdminPage.CountyCode, "<select a local district>", Empty);
      else
        hasLocals = LocalDistricts.Populate(LocalDropDownList, _SecureAdminPage.StateCode,
          _SecureAdminPage.CountyCode, _SecureAdminPage.LocalKey);
      if (!hasLocals)
      {
        LocalRadioButton.Disabled = true;
        LocalDropDownList.AddCssClasses("hidden");
        LocalName.RemoveCssClass("hidden");
        LocalName.InnerHtml = "no local districts available";
      }
    }

    private void InitializeForMaster()
    {
      DialogCredentialMessage.InnerHtml =
        "Your sign-in credentials permit any jurisdiction to be selected.";
      StateName.AddCssClasses("hidden");

      switch (_SecureAdminPage.AdminPageLevel)
      {
        case AdminPageLevel.President:
        case AdminPageLevel.PresidentTemplate:
        case AdminPageLevel.Federal:
          StateRadioButton.Checked = true;
          CountyRadioButton.Disabled = true;
          LocalRadioButton.Disabled = true;
          PopulateStateDropDown();
          CountyDropDownList.AddCssClasses("hidden");
          LocalDropDownList.AddCssClasses("hidden");
          break;

        case AdminPageLevel.State:
          StateRadioButton.Checked = true;
          LocalRadioButton.Disabled = true;
          PopulateStateDropDown();
          PopulateCountyDropDown(true);
          CountyName.AddCssClasses("hidden");
          LocalDropDownList.AddCssClasses("hidden");
          break;

        case AdminPageLevel.County:
          CountyRadioButton.Checked = true;
          PopulateStateDropDown();
          PopulateCountyDropDown();
          CountyName.AddCssClasses("hidden");
          PopulateLocalDropDown(true);
          LocalName.AddCssClasses("hidden");
          break;

        case AdminPageLevel.Local:
          LocalRadioButton.Checked = true;
          PopulateStateDropDown();
          PopulateCountyDropDown();
          PopulateLocalDropDown();
          CountyName.AddCssClasses("hidden");
          LocalName.AddCssClasses("hidden");
          break;

        case AdminPageLevel.Unknown:
          StateRadioButton.Checked = true;
          CountyRadioButton.Disabled = true;
          LocalRadioButton.Disabled = true;
          PopulateStateDropDown(true);
          CountyDropDownList.AddCssClasses("hidden");
          LocalDropDownList.AddCssClasses("hidden");
          break;
      }
    }

    private void InitializeForStateAdmin()
    {
      DialogCredentialMessage.InnerHtml = "Your sign-in credentials allow any " +
        States.GetName(_SecureAdminPage.StateCode) +
        " jurisdiction to be selected.";

      StateName.InnerHtml = States.GetName(_SecureAdminPage.StateCode);
      StateDropDownList.AddCssClasses("hidden");
      CountyName.AddCssClasses("hidden");

      switch (_SecureAdminPage.AdminPageLevel)
      {
        case AdminPageLevel.State:
          StateRadioButton.Checked = true;
          LocalRadioButton.Disabled = true;
          PopulateCountyDropDown(true);
          LocalDropDownList.AddCssClasses("hidden");
          break;

        case AdminPageLevel.County:
          CountyRadioButton.Checked = true;
          PopulateCountyDropDown();
          PopulateLocalDropDown(true);
          LocalName.AddCssClasses("hidden");
          break;

        case AdminPageLevel.Local:
          LocalRadioButton.Checked = true;
          PopulateCountyDropDown();
          PopulateLocalDropDown();
          LocalName.AddCssClasses("hidden");
          break;
      }
    }

    private void InitializeForCountyAdmin()
    {
      DialogCredentialMessage.InnerHtml = "Your sign-in credentials allow only " +
        Counties.GetFullName(_SecureAdminPage.StateCode,
          _SecureAdminPage.CountyCode) + " local districts to be selected.";

      StateName.InnerHtml = States.GetName(_SecureAdminPage.StateCode);
      StateDropDownList.AddCssClasses("hidden");
      StateRadioButton.AddCssClasses("invisible");
      CountyName.InnerHtml = Counties.GetName(_SecureAdminPage.StateCode,
        _SecureAdminPage.CountyCode);
      CountyDropDownList.AddCssClasses("hidden");
      LocalName.AddCssClasses("hidden");

      switch (_SecureAdminPage.AdminPageLevel)
      {
        case AdminPageLevel.County:
          CountyRadioButton.Checked = true;
          PopulateLocalDropDown(true);
          break;

        case AdminPageLevel.Local:
          LocalRadioButton.Checked = true;
          PopulateLocalDropDown();
          break;
      }
    }

    #endregion Private

    #region Public

    public void Initialize()
    {
      switch (SecurePage.UserSecurityClass)
      {
        case SecurePage.MasterSecurityClass:
          InitializeForMaster();
          break;

        case SecurePage.StateAdminSecurityClass:
          InitializeForStateAdmin();
          break;

        case SecurePage.CountyAdminSecurityClass:
          InitializeForCountyAdmin();
          break;

        case SecurePage.LocalAdminSecurityClass:
          break;

        default:
          throw new VoteException("Unexpected UserSecurityClass: " +
            SecurePage.UserSecurityClass);
      }
    }

    public string NonStateNames
    {
      set
      {
        var segments = value.Split('|');
        if (segments.Length <= 0 || segments.Length % 2 != 0) return;
        _NonStateNameDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var inx = 0; inx < segments.Length; inx += 2)
          if (!_NonStateNameDictionary.ContainsKey(segments[inx]))
            _NonStateNameDictionary.Add(segments[inx], segments[inx + 1]);
      }
    }

    #endregion Public

    public string AdminPageName { private get; set; }

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      _SecureAdminPage = VotePage.GetPage<SecureAdminPage>();
      if (_SecureAdminPage == null)
        throw new VoteException(
          "The NavigateJurisdiction control can only be used in the SecureAdminPage class");

      if (!IsPostBack)
      {
        Page.IncludeCss("~/css/vote/controls/NavigateJurisdiction.css");
        var cs = Page.ClientScript;
        var type = GetType();
        const string scriptName = "NavigateJurisdiction";
        if (cs.IsStartupScriptRegistered(type, scriptName)) return;
        cs.RegisterStartupScript(type, scriptName,
          "require(['vote/controls/navigateJurisdiction'], function(){{}});", true);

        if (IsNullOrWhiteSpace(AdminPageName))
        {
          var match = Regex.Match(VotePage.CurrentPath, @"/([^./]+)\.");
          if (!match.Success)
            throw new VoteException("Missing AdminPageName");
          AdminPageName = match.Groups[1].Value;
        }
        PageName.Value = AdminPageName;

        UserSecurityClass.Value = SecurePage.UserSecurityClass;
        OriginalStateCode.Value = _SecureAdminPage.StateCode;
        OriginalCountyCode.Value = _SecureAdminPage.CountyCode;
        OriginalLocalKey.Value = _SecureAdminPage.LocalKey;

        Initialize();
      }
    }

    #endregion Event handlers and overrides
  }
}