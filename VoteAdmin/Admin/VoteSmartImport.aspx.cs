using System;
using DB.Vote;

namespace Vote.Admin
{
  [PageInitializers]
  public partial class VoteSmartImportPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region Private

    // ReSharper disable UnusedMember.Local
    // No updating here, only to provide initialization services
    [PageInitializer]
    private class SelectElectionControlItem
      // ReSharper restore UnusedMember.Local
    {
      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(VoteSmartImportPage page)
        // ReSharper restore UnusedMember.Local
      {
        if (!page.IsPostBack) page.PopulateElectionControl();
      }
    }

    private void PopulateElectionControl() => 
      ElectionControl.Populate(StateCode, CountyCode, LocalCode);

    private void SetCredentialMessage()
    {
      switch (UserSecurityClass)
      {
        case MasterSecurityClass:
          CredentialMessage.InnerHtml = IsSuperUser
            ? "Your sign-in credentials allow any election to be updated."
            : "Your sign-in credentials allow any federal, state, county or local election to be updated.";
          break;

        case StateAdminSecurityClass:
          CredentialMessage.InnerHtml = "Your sign-in credentials permit any " +
            States.GetName(StateCode) + " election to be updated.";
          break;

        case CountyAdminSecurityClass:
          CredentialMessage.InnerHtml = "Your sign-in credentials permit only " +
            Counties.GetFullName(StateCode, CountyCode) +
            " elections to be updated.";
          break;

        case LocalAdminSecurityClass:
          CredentialMessage.InnerHtml = "Your sign-in credentials permit only " +
            LocalDistricts.GetFullName(StateCode, CountyCode, LocalCode) +
            " elections to be updated.";
          SelectJurisdictionButton.Visible = false;
          break;

        default:
          throw new VoteException("Unexpected UserSecurityClass: " +
            UserSecurityClass);
      }
    }

    private void SetSubHeading()
    {
      switch (AdminPageLevel)
      {
        case AdminPageLevel.President:
        case AdminPageLevel.PresidentTemplate:
        case AdminPageLevel.Federal:
          H2.InnerHtml = States.GetName(StateCode) + ", All States";
          break;

        case AdminPageLevel.State:
          H2.InnerHtml = "State Elections for " + States.GetName(StateCode);
          break;

        case AdminPageLevel.County:
          H2.InnerHtml = "County Elections for " +
            Counties.GetFullName(StateCode, CountyCode);
          break;

        case AdminPageLevel.Local:
          H2.InnerHtml = "Local Elections for " +
            LocalDistricts.GetFullName(StateCode, CountyCode, LocalCode);
          break;

        case AdminPageLevel.Unknown:
          H2.InnerHtml = "No Jurisdiction Selected";
          break;
      }
    }

    #endregion Private

    #region Protected

    #endregion Protected

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        const string title = "VoteSmart Import";
        Page.Title = title;
        H1.InnerHtml = title;

        SetSubHeading();
        SetCredentialMessage();
        ClientStateCode.Value = StateCode;
        ClientStateName.Value = StateCache.GetStateName(StateCode);

        if (AdminPageLevel == AdminPageLevel.Unknown)
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateStateLinks("/admin/VoteSmartImport.aspx?state={StateCode}");
          NoJurisdiction.Visible = true;
        }

        RefreshElectionsButton.Disabled = true;
        if (StateCodeExists)
          RefreshElectionsButton.Value = $"Refresh VoteSmart Elections for {ClientStateName.Value}";
      }
    }

    #endregion Event handlers and overrides
  }
}