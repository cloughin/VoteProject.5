using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Admin
{
  [PageInitializers]
  public partial class UpdateJurisdictionsPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region Private

    #region DataItem objects

    // Base classes for tab classes to inherit from
    private abstract class JurisdictionsDataItem : DataItemBase
    {
      protected readonly UpdateJurisdictionsPage Page;

      protected JurisdictionsDataItem(UpdateJurisdictionsPage page,
        string groupName) : base(groupName)
      {
        Page = page;
      }

      protected Counties.Column CountiesColumn
      {
        get
        {
          Counties.Column column;
          switch (Column)
          {
            case "JurisdictionName":
              column = Counties.Column.County;
              break;

            default:
              column = Counties.GetMappedColumn(Column);
              break;
          }
          return column;
        }
      }

      protected LocalDistricts.Column LocalDistrictsColumn
      {
        get
        {
          LocalDistricts.Column column;
          switch (Column)
          {
            case "JurisdictionName":
              column = LocalDistricts.Column.LocalDistrict;
              break;

            default:
              column = LocalDistricts.GetMappedColumn(Column);
              break;
          }
          return column;
        }
      }

      protected States.Column StatesColumn
      {
        get
        {
          States.Column column;
          switch (Column)
          {
            case "JurisdictionName":
              column = States.Column.State;
              break;

            default:
              column = States.GetMappedColumn(Column);
              break;
          }
          return column;
        }
      }

      protected override void IncrementUpdateCount() => Page._UpdateCount++;
    }

    private abstract class AllJurisdictionsDataItem : JurisdictionsDataItem
    {
      protected AllJurisdictionsDataItem(UpdateJurisdictionsPage page,
        string groupName) : base(page, groupName)
      {
      }

      protected override string GetCurrentValue()
      {
        switch (Page.AdminPageLevel)
        {
          case AdminPageLevel.State:
            return
              (States.GetColumn(StatesColumn, Page.StateCode) ?? string.Empty).ToString();

          case AdminPageLevel.County:
            return
            (Counties.GetColumn(CountiesColumn, Page.StateCode,
              Page.CountyCode) ?? string.Empty).ToString();

          case AdminPageLevel.Local:
            return
              (LocalDistricts.GetColumn(LocalDistrictsColumn,
                  Page.StateCode, Page.CountyCode, Page.LocalCode) ?? string.Empty)
                .ToString();

          default:
            return null;
        }
      }

      protected override void Log(string oldValue, string newValue)
      {
        switch (Page.AdminPageLevel)
        {
          case AdminPageLevel.State:
            LogDataChange.LogUpdate(States.TableName, Column, oldValue, newValue,
              UserName, UserSecurityClass, DateTime.UtcNow, Page.StateCode);
            break;

          case AdminPageLevel.County:
            LogDataChange.LogUpdate(Counties.TableName, Column, oldValue, newValue,
              UserName, UserSecurityClass, DateTime.UtcNow, Page.StateCode,
              Page.CountyCode);
            break;

          case AdminPageLevel.Local:
            LogDataChange.LogUpdate(LocalDistricts.TableName, Column, oldValue,
              newValue, UserName, UserSecurityClass, DateTime.UtcNow,
              Page.StateCode, Page.CountyCode, Page.LocalCode);
            break;
        }
      }

      protected override bool Update(object newValue)
      {
        switch (Page.AdminPageLevel)
        {
          case AdminPageLevel.State:
            States.UpdateColumn(StatesColumn, newValue, Page.StateCode);
            break;

          case AdminPageLevel.County:
            Counties.UpdateColumn(CountiesColumn, newValue, Page.StateCode, Page.CountyCode);
            break;

          case AdminPageLevel.Local:
            LocalDistricts.UpdateColumn(LocalDistrictsColumn, newValue, Page.StateCode,
              Page.CountyCode,
              Page.LocalCode);
            break;
        }
        return true;
      }
    }

    private abstract class StateJurisdictionsDataItem : JurisdictionsDataItem
    {
      protected StateJurisdictionsDataItem(UpdateJurisdictionsPage page,
        string groupName) : base(page, groupName)
      {
      }

      protected override string GetCurrentValue() => 
        (States.GetColumn(StatesColumn, Page.StateCode) ?? string.Empty).ToString();

      protected override void Log(string oldValue, string newValue) => 
        LogDataChange.LogUpdate(States.TableName, Column, oldValue, newValue,
        UserName, UserSecurityClass, DateTime.UtcNow, Page.StateCode);

      protected override bool Update(object newValue)
      {
        States.UpdateColumn(StatesColumn, newValue, Page.StateCode);
        return true;
      }
    }

    #endregion DataItem objects

    // ReSharper disable once NotAccessedField.Local
    private int _UpdateCount;

    private void SetCredentialMessage()
    {
      switch (UserSecurityClass)
      {
        case MasterSecurityClass:
          CredentialMessage.InnerHtml =
            "Your sign-in credentials allow any jurisdiction to be updated.";
          break;

        case StateAdminSecurityClass:
          CredentialMessage.InnerHtml = "Your sign-in credentials permit any " +
            States.GetName(StateCode) + " jurisdiction to be updated.";
          break;

        case CountyAdminSecurityClass:
          CredentialMessage.InnerHtml = "Your sign-in credentials permit only " +
            Counties.GetFullName(StateCode, CountyCode) +
            " jurisdictions to be updated.";
          break;

        case LocalAdminSecurityClass:
          CredentialMessage.InnerHtml =
            "Your sign-in credentials permit only the " +
            LocalDistricts.GetFullName(StateCode, CountyCode, LocalCode) +
            " jurisdiction to be updated.";
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
          H2.InnerHtml = "General State Information for " +
            States.GetName(StateCode);
          break;

        case AdminPageLevel.County:
          H2.InnerHtml = "General County Information for " +
            Counties.GetFullName(StateCode, CountyCode);
          break;

        case AdminPageLevel.Local:
          H2.InnerHtml = "General Local Information for " +
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

    public override IEnumerable<string> NonStateCodesAllowed => new string[0];

    #region ReSharper restore

    // ReSharper restore UnusedMember.Global
    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      const string title = "Update Jurisdictions";
      Page.Title = title;
      H1.InnerHtml = title;

      var body = Master.FindControl("body") as HtmlGenericControl;
      Debug.Assert(body != null, "body != null");
      body.Attributes.Add("data-state", StateCode);

      SetSubHeading();
      SetCredentialMessage();

      if (AdminPageLevel == AdminPageLevel.Unknown)
      {
        UpdateControls.Visible = false;
        NoJurisdiction.CreateStateLinks("/admin/UpdateJurisdictions.aspx?state={StateCode}");
        NoJurisdiction.Visible = true;
      }
    }

    #endregion Event handlers and overrides
  }
}