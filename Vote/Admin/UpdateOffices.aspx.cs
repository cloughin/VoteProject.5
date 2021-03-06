using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using DB.Vote;
using Vote.Controls;

namespace Vote.Admin
{
  [PageInitializers]
  public partial class UpdateOfficesPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region Private

    private ManagePoliticiansPanel _ManagePoliticiansPanel;

    private string JurisdictionalKey => StateCode + CountyCode + LocalCode;

    #region DataItem object

    // Base class for tab classes to inherit from
    private abstract class OfficesDataItem : DataItemBase
    {
      protected readonly UpdateOfficesPage Page;

      protected OfficesDataItem(UpdateOfficesPage page, string groupName)
        : base(groupName)
      {
        Page = page;
      }

      protected override string GetCurrentValue()
      {
        throw new NotImplementedException();
      }

      protected override bool Update(object newValue)
      {
        throw new NotImplementedException();
      }
    }

    #endregion DataItem object

    private void LoadOfficeControl(string officeKey = null)
    {
      var table = Elections.GetJurisdictionOfficeData(StateCode, CountyCode, LocalCode);

      OfficeControl.PopulateOfficeTree(table, OfficeControl.OfficeTree,
        StateCode, false, AdminPageLevel == AdminPageLevel.State,
        AdminPageLevel != AdminPageLevel.State,
        AdminPageLevel == AdminPageLevel.State);
      if (!string.IsNullOrWhiteSpace(officeKey))
        OfficeControl.OfficeKey = officeKey;
      OfficeControl.Update();
    }

    private void PopulateAddOfficeSelectOfficeClass()
    {
      // initial office class selection is from Query String as string ordinal
      var initialOfficeClass =
        Offices.GetValidatedOfficeClass(GetQueryString("class"));

      // iterator options
      var iteratorOptions = GetOfficeClassesOptions.None;
      switch (AdminPageLevel)
      {
        case AdminPageLevel.State:
          iteratorOptions |= GetOfficeClassesOptions.IncludeCongress |
            GetOfficeClassesOptions.IncludeState;
          break;

        case AdminPageLevel.County:
          iteratorOptions |= GetOfficeClassesOptions.IncludeCounty;
          break;

        case AdminPageLevel.Local:
          iteratorOptions |= GetOfficeClassesOptions.IncludeLocal;
          break;
      }

      // create an entry for each OfficeClass returned by the iterator
      AddOfficeSelectOfficeClass.Items.Clear();
      AddOfficeSelectOfficeClass.Items.Add(new ListItem
      {
        Value = string.Empty,
        Text = "<select an office class>"
      });
      foreach (var officeClass in Offices.GetOfficeClasses(iteratorOptions))
      {
        var listItem = new ListItem
        {
          Value = officeClass.ToInt().ToString(CultureInfo.InvariantCulture),
          Text = Offices.GetOfficeClassDescription(officeClass, StateCode),
          Selected = officeClass == initialOfficeClass
        };
        if (OfficesAllIdentified.GetIsOfficesAllIdentified(StateCode,
          officeClass.ToInt(), CountyCode, LocalCode))
          listItem.Attributes.Add("disabled", "disabled");
        AddOfficeSelectOfficeClass.Items.Add(listItem);
      }
    }

    private void SetCredentialMessage()
    {
      switch (UserSecurityClass)
      {
        case MasterSecurityClass:
          CredentialMessage.InnerHtml = IsSuperUser
            ? "Your sign-in credentials allow any offices to be updated."
            : "Your sign-in credentials allow any federal, state, county or local offices to be updated.";
          break;

        case StateAdminSecurityClass:
          CredentialMessage.InnerHtml = "Your sign-in credentials permit any " +
            States.GetName(StateCode) + " offices to be updated.";
          break;

        case CountyAdminSecurityClass:
          CredentialMessage.InnerHtml = "Your sign-in credentials permit only " +
            Counties.GetFullName(StateCode, CountyCode) +
            " offices to be updated.";
          break;

        case LocalAdminSecurityClass:
          CredentialMessage.InnerHtml = "Your sign-in credentials permit only " +
            LocalDistricts.GetFullName(StateCode, CountyCode, LocalCode) +
            " offices to be updated.";
          ChangeJurisdictionButton.Visible = false;
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
        //case AdminPageLevel.President:
        //case AdminPageLevel.PresidentTemplate:
        //case AdminPageLevel.Federal:
        //  H2.InnerHtml = States.GetName(StateCode) + ", All States";
        //  break;

        case AdminPageLevel.State:
          H2.InnerHtml = "State Offices for " + States.GetName(StateCode);
          break;

        case AdminPageLevel.County:
          H2.InnerHtml = "County Offices for " +
            Counties.GetFullName(StateCode, CountyCode);
          break;

        case AdminPageLevel.Local:
          H2.InnerHtml = "Local Offices for " +
            LocalDistricts.GetFullName(StateCode, CountyCode, LocalCode);
          break;

        case AdminPageLevel.Unknown:
          H2.InnerHtml = "No Jurisdiction Selected";
          break;
      }
    }

    #endregion Private

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global
    // ReSharper disable UnusedMember.Global

    #endregion ReSharper disable

    public override IEnumerable<string> NonStateCodesAllowed => new[] {"US"};

    public override IEnumerable<string> NonStateCodesRequireSuperUser => new[] {"US"};

    #region ReSharper restore

    // ReSharper restore UnusedMember.Global
    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      _ManagePoliticiansPanel =
        Master.FindMainContentControl("ManagePoliticiansPanel") as ManagePoliticiansPanel;
      if (_ManagePoliticiansPanel != null)
      {
        _ManagePoliticiansPanel.Mode = ManagePoliticiansPanel.DataMode.ManageIncumbents;
        _ManagePoliticiansPanel.GetOfficeKey = () => OfficeControl.OfficeKey;
        _ManagePoliticiansPanel.PageFeedback = FeedbackAddCandidates;
      }

      if (!IsPostBack)
      {
        const string title = "Update Offices";
        Page.Title = title;
        H1.InnerHtml = title;

        SetSubHeading();
        SetCredentialMessage();
        LoadOfficeControl();
        PopulateAddOfficeSelectOfficeClass();

        Master.MasterForm.Attributes.Add("data-state-code", StateCode);
        Master.MasterForm.Attributes.Add("data-county-code", CountyCode);
        Master.MasterForm.Attributes.Add("data-local-code", LocalCode);

        if (AdminPageLevel == AdminPageLevel.Unknown)
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateStateLinks("/admin/UpdateOffices.aspx?state={StateCode}");
          NoJurisdiction.Visible = true;
        }

        if (AdminPageLevel == AdminPageLevel.State)
        {
          InitializeTabOfficeTemplate();
        }
        else
        {
          TabAddOfficeTemplateItem.Visible = false;
        }
      }
    }

    #endregion Event handlers and overrides
  }
}