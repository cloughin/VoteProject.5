using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;
using Vote.Controls;
using static System.String;

namespace Vote.Admin
{
  [PageInitializers]
  public partial class UpdateElectionsPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region Private

    private ManagePoliticiansPanel _ManagePoliticiansPanel;

    protected bool ShowAddElections;
    protected bool ShowStateDefaults;
    protected bool ShowChangeInfo;
    protected bool ShowChangeDeadlines;
    protected bool ShowChangeOffices;
    protected bool ShowAddCandidates;
    protected bool ShowAdjustIncumbents;
    protected bool ShowIdentifyPrimaryWinners;
    protected bool ShowIdentifyGeneralWinners;
    protected bool ShowAddBallotMeasures;

    #region DataItem object

    // Base class for tab classes to inherit from
    private abstract class ElectionsDataItem : DataItemBase
    {
      protected readonly UpdateElectionsPage Page;

      protected ElectionsDataItem(UpdateElectionsPage page, string groupName)
        : base(groupName)
      {
        Page = page;
      }

      protected override string GetCurrentValue()
      {
        var column = Elections.GetColumn(Column);
        var electionKey = Page.GetElectionKey();
        var value = Elections.GetColumn(column, electionKey);
        if (value == null && !Elections.IsStateElection(electionKey))
          value = Elections.GetColumn(column, Elections.GetStateElectionKeyFromKey(electionKey));
        return value == null ? Empty : ToDisplay(value);
      }

      protected override void Log(string oldValue, string newValue) => 
        LogElectionsDataChange(Page.GetElectionKey(), Column, oldValue, newValue);

      protected override bool Update(object newValue)
      {
        var column = Elections.GetColumn(Column);
        var electionKey = Page.GetElectionKey();
        Elections.ActualizeElection(electionKey);
        Elections.UpdateColumn(column, newValue, electionKey);
        return true;
      }

      protected static bool ValidateDescription(DataItemBase item)
      {
        item.Feedback.ValidateLength(item.DataControl as TextBox, item.Description,
          1, 90, out var success);
        return success;
      }
    }

    #endregion DataItem object

    private string GetElectionKey()
    {
      return SelectedElectionKey.Value;
    }

    //private string GetOfficeKey()
    //{
    //  return SelectedOfficeKey.Value;
    //}

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
            LocalDistricts.GetFullName(StateCode, CountyCode, LocalKey) +
            " elections to be updated.";
          SelectJurisdictionButton.Visible = false;
          break;

        default:
          throw new VoteException("Unexpected UserSecurityClass: " +
            UserSecurityClass);
      }
    }

    private void SetElectionHeading(HtmlContainerControl control)
    {
      var electionKey = GetElectionKey();
      var desc = Elections.GetElectionDesc(electionKey, Empty);
      if (IsNullOrEmpty(desc) && !Elections.IsStateElection(electionKey))
        desc = Elections.GetElectionDesc(Elections.GetStateElectionKeyFromKey(electionKey),
          Empty);
      control.InnerHtml = desc;
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
            LocalDistricts.GetFullName(StateCode, CountyCode, LocalKey);
          int countyCount;
          if ((countyCount = FormatOtherCountiesMessage(MultiCountyMessage)) > 1)
            MultiCountyMessage.InnerText +=
              $". Changes you make to elections will appear in {(countyCount == 2 ? "both" : "all")} counties.";
          break;

        case AdminPageLevel.Unknown:
          H2.InnerHtml = "No Jurisdiction Selected";
          break;
      }
    }

    private static DateTime ValidateElectionDate(TextBox textBox,
      FeedbackContainerControl feedback, bool allowPastElection, out bool success)
    {
      var mindate = allowPastElection
        ? new DateTime(Elections.MinimumElectionYear, 1, 1)
        : DateTime.UtcNow.Date;
      var maxdate = new DateTime(Elections.MaximumElectionYear, 12, 31);
      var electionDate = feedback.ValidateDate(textBox, out success,
        "Election Date", mindate, maxdate);
      return electionDate;
    }

    #endregion Private

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global
    // ReSharper disable UnusedMember.Global

    #endregion ReSharper disable

    //protected const bool ShowSendEmailTab = false;

    public override IEnumerable<string> NonStateCodesAllowed => 
      new[] {"US", "PP", "U1", "U2", "U3", "U4"};

    public override IEnumerable<string> NonStateCodesRequireSuperUser => 
      new[] {"US", "PP"};

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
        _ManagePoliticiansPanel.Mode = ManagePoliticiansPanel.DataMode.ManageCandidates;
        _ManagePoliticiansPanel.GetElectionKey = GetElectionKey;
        _ManagePoliticiansPanel.GetOfficeKey = () => OfficeControl.OfficeKey;
        _ManagePoliticiansPanel.PageFeedback = FeedbackAddCandidates;
      }

      if (!IsPostBack)
      {
        const string title = "Update Elections";
        Page.Title = title;
        H1.InnerHtml = title;

        SetSubHeading();
        SetCredentialMessage();
        ClientStateCode.Value = StateCode;

        OfficeControl.Message += "<br />The current number of candidates is in [square brackets].";

        if (AdminPageLevel == AdminPageLevel.Unknown)
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateStateLinks("/admin/UpdateElections.aspx?state={StateCode}");
          NoJurisdiction.Visible = true;
        }

        if (!ShowAddElections)
        {
          SelectElectionControl.RemoveCssClass("disabled");
          SelectedElectionKey.Value = _MostRecentElectionKey;
          if (!IsNullOrEmpty(GetElectionKey())) // It's the default tab
          {
            _ChangeInfoTabInfo.LoadControls();
            SetElectionHeading(HeadingChangeInfo);
          }
        }

        ShowAddCandidates = StateCache.IsValidStateCode(StateCode) ||
          StateCache.IsPresidentialPrimary(StateCode);
        if (!ShowAddCandidates)
          TabAddCandidatesItem.Visible = false;
      }
    }

    #endregion Event handlers and overrides
  }
}