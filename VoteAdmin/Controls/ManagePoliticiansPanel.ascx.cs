using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using Vote.Reports;
using static System.String;

namespace Vote.Controls
{
  public partial class ManagePoliticiansPanel : UserControl
  {
    #region Private

    private SecureAdminPage _SecureAdminPage;

    private static Control CreateCandidateEntry(DataRow candidate, DataMode mode,
      string partyCodeToSuppress = null, bool showAdMenuItem = true)
    {
      var isRunningMate = candidate.IsRunningMate();
      var modeDescription = mode == DataMode.ManageIncumbents ? "Incumbent" : "Candidate";
      var innerDiv = new HtmlDiv().AddCssClasses("candidate " +
        (isRunningMate ? "running-mate mate" : "main"));
      if (isRunningMate)
        new HtmlInputHidden {Value = candidate.PoliticianKey()}.AddTo(innerDiv, "mate-id");

      var cellDiv = new HtmlDiv().AddTo(innerDiv, "cell pic");
      Report.CreatePoliticianImageTag(candidate.PoliticianKey(), 35).AddTo(cellDiv);

      cellDiv = new HtmlDiv().AddTo(innerDiv, "cell data data1");
      if (isRunningMate)
      {
        var mateDiv = new HtmlDiv().AddTo(cellDiv, "mate-header");
        new LiteralControl("Running Mate").AddTo(mateDiv);
      }

      var itemDiv = new HtmlDiv().AddTo(cellDiv, "name");
      new LiteralControl(Politicians.FormatName(candidate, true)).AddTo(itemDiv);
      var partyCode = candidate.PartyCode();
      if (!IsNullOrWhiteSpace(partyCode) && partyCode != partyCodeToSuppress)
      {
        new LiteralControl(" (").AddTo(itemDiv);
        var anchor = Report.CreatePartyAnchor(candidate, "view").AddTo(itemDiv);
        if (anchor is HtmlAnchor) anchor.AddCssClasses("tiptip");
        new LiteralControl(")").AddTo(itemDiv);
      }
      //if (mode == DataMode.ManageCandidates && !isRunningMate && candidate.IsIncumbent())
      //  new HtmlSpan {InnerHtml = " •"}.AddTo(itemDiv, "tiptip").Attributes["title"] =
      //    "Candidate is incumbent for this election";

      var text = candidate.StateOrCandidateAddress();
      var className = Empty;
      if (IsNullOrWhiteSpace(text))
      {
        text = "Address not available";
        className = " na";
      }
      itemDiv = new HtmlDiv().AddTo(cellDiv, "data-item address" + className);
      new LiteralControl(text).AddTo(itemDiv);

      text = candidate.StateOrCandidateCityStateZip();
      className = Empty;
      if (IsNullOrWhiteSpace(text))
      {
        text = "City State Zip not available";
        className = " na";
      }
      itemDiv = new HtmlDiv().AddTo(cellDiv, "data-item city-state-zip" + className);
      new LiteralControl(text).AddTo(itemDiv);

      if (mode == DataMode.ManageCandidates && !isRunningMate)
      {
        var showAsIncumbentDiv = new HtmlDiv().AddTo(cellDiv, "show-as-incumbent");
        var checkboxId = $"incumbent-{candidate.PoliticianKey()}";
        new HtmlInputCheckBox
        {
          ID = checkboxId,
          ClientIDMode = ClientIDMode.Static,
          Checked = candidate.IsIncumbent()
        }.AddTo(showAsIncumbentDiv);
        new HtmlLabel
        {
          InnerText = "Show as incumbent"
        }.AddTo(showAsIncumbentDiv, "data-item").Attributes.Add("for", checkboxId);
      }
      cellDiv = new HtmlDiv().AddTo(innerDiv, "cell data data2");

      text = candidate.StateOrCandidatePhone();
      className = Empty;
      if (IsNullOrWhiteSpace(text))
      {
        text = "Phone not available";
        className = " na";
      }
      itemDiv = new HtmlDiv().AddTo(cellDiv, "data-item phone" + className);
      new LiteralControl(text).AddTo(itemDiv);

      text = candidate.StateOrCandidateEmail();
      className = Empty;
      Control control;
      if (IsNullOrWhiteSpace(text))
      {
        className = " na";
        control = new LiteralControl("Email not available");
      }
      else
        control = new HtmlAnchor
        {
          HRef = VotePage.NormalizeEmailHRef(text),
          InnerHtml = text
        };
      itemDiv = new HtmlDiv().AddTo(cellDiv, "data-item email" + className);
      control.AddTo(itemDiv);

      text = candidate.StateOrCandidateWebAddress();
      className = Empty;
      if (IsNullOrWhiteSpace(text))
      {
        className = " na";
        control = new LiteralControl("Web site not available");
      }
      else
        control = new HtmlAnchor
        {
          HRef = VotePage.NormalizeUrl(text),
          InnerHtml = text,
          Target = "view"
        };
      itemDiv = new HtmlDiv().AddTo(cellDiv, "data-item web-site" + className);
      control.AddTo(itemDiv);

      cellDiv = new HtmlDiv().AddTo(innerDiv, "cell icons");

      new HtmlDiv().AddTo(cellDiv, "icon-move tiptip").Attributes["title"] =
        "Drag to reorder the " + modeDescription + "s";

      var isEditable = SecurePage.IsMasterUser || SecurePage.IsStateAdminUser &&
        SecurePage.UserStateCode ==
        Politicians.GetStateCodeFromKey(candidate.PoliticianKey());

      var menu = new HtmlDiv().AddTo(cellDiv, "candidate-menu");
      var ul1 = new HtmlUl().AddTo(menu, "candidate-menu");
      var li1 = new HtmlLi().AddTo(ul1);
      var a1 = new HtmlAnchor().AddTo(li1);
      new HtmlDiv().AddTo(a1, "icon icon-menu");
      var ul2 = new HtmlUl().AddTo(li1);

      HtmlControl li2;
      if (isEditable)
      {
        li2 = new HtmlLi().AddTo(ul2, "rounded-border");
        new HtmlAnchor { InnerHtml = "Edit Name, DOB, Address &amp; Party" }
          .AddTo(li2, "edit-candidate-link").Attributes["pkey"] = candidate.PoliticianKey();
      }

      li2 = new HtmlLi().AddTo(ul2, "rounded-border");
      new HtmlAnchor
      {
        InnerHtml = "View Public Intro Page",
        HRef = UrlManager.GetIntroPageUri(candidate.PoliticianKey()).ToString(),
        Target = "view"
      }.AddTo(li2);

      if (isEditable)
      {
        li2 = new HtmlLi().AddTo(ul2, "rounded-border");
        new HtmlAnchor
        {
          InnerHtml = "Edit Links, Picture, Bio &amp; Reasons",
          HRef = SecurePoliticianPage.GetUpdateIntroPageUrl(candidate.PoliticianKey()),
          Target = "view"
        }.AddTo(li2);
      }

      if (isEditable)
      {
        li2 = new HtmlLi().AddTo(ul2, "rounded-border");
        new HtmlAnchor
        {
          InnerHtml = "Edit Issue Topic Responses",
          HRef = SecurePoliticianPage.GetUpdateIssuesPageUrl(candidate.PoliticianKey()),
          Target = "view"
        }.AddTo(li2);
      }

      if (SecurePage.IsMasterUser)
      {
        li2 = new HtmlLi().AddTo(ul2, "rounded-border");
        new HtmlAnchor { InnerHtml = "Get " + modeDescription + "'s Key" }
            .AddTo(li2, "get-candidate-key-link").Attributes["pkey"] =
          candidate.PoliticianKey();
      }

      if (SecurePage.IsMasterUser && mode == DataMode.ManageCandidates && showAdMenuItem)
      {
        li2 = new HtmlLi().AddTo(ul2, "rounded-border");
        new HtmlAnchor { InnerHtml = "Set Up " + modeDescription + "'s Ad" }
          .AddTo(li2, "setup-candidate-ad-link").Attributes["pkey"] =
          candidate.PoliticianKey();
      }

      new HtmlDiv().AddTo(cellDiv, "clear-both");

      new HtmlDiv().AddTo(cellDiv, "icon icon-remove tiptip").Attributes["title"] =
        isRunningMate
          ? "Check to remove this running mate"
          : "Check to remove this " + modeDescription;

      new HtmlDiv().AddTo(innerDiv, "clear-both");

      return innerDiv;
    }

    private static Control CreateNoRunningMateEntry()
    {
      var innerDiv = new HtmlDiv().AddCssClasses("candidate running-mate no-mate");

      new HtmlDiv().AddTo(innerDiv, "cell pic");

      var cellDiv = new HtmlDiv().AddTo(innerDiv, "cell data data1");
      var mateDiv = new HtmlDiv().AddTo(cellDiv, "mate-header");
      new LiteralControl("No Running Mate").AddTo(mateDiv);

      new HtmlDiv().AddTo(innerDiv, "cell data data2");

      cellDiv = new HtmlDiv().AddTo(innerDiv, "cell icons");

      new HtmlDiv().AddTo(cellDiv, "icon icon-add-mate tiptip").Attributes["title"] =
        "Add running mate";

      new HtmlDiv().AddTo(innerDiv, "clear-both");

      return innerDiv;
    }

    private string SafeGetOfficeKey()
    {
      return GetOfficeKey == null ? Empty : GetOfficeKey();
    }

    private string SafeGetElectionKey()
    {
      return GetElectionKey == null ? Empty : GetElectionKey();
    }

    private (string Key, bool ShowAsIncumbent) GetPoliticianKeyToEdit()
    {
      bool.TryParse(CandidateToEditIncumbent.Value, out var showAsIncumbent);
      return (CandidateToEdit.Value, showAsIncumbent);
    }

    #endregion Private

    #region Public

    public enum DataMode
    {
      ManageCandidates,
      ManageIncumbents,
      AddPoliticians
    }

    private DataMode _Mode = DataMode.ManageCandidates;

    public Func<string> GetElectionKey { private get; set; }
    public Func<string> GetOfficeKey { private get; set; }
    public FeedbackContainerControl PageFeedback { private get; set; }

    public HtmlGenericControl Message => ManagePoliticiansMessage;

    public DataMode Mode
    {
      private get { return _Mode; }
      set
      {
        _Mode = value;
        ManagePoliticiansPanelMode.Value = value.ToString();
        switch (value)
        {
          case DataMode.AddPoliticians:
            AddExistingCandidateButton.Value =
              AddNewCandidateButton.Value = "Add Politician";
            EditPoliticianTitle.InnerText = "Edit Politician Information";
            break;

          case DataMode.ManageCandidates:
            ManagePoliticiansMessage.InnerText = "No candidates were found for this office";
            AddExistingCandidateButton.Value = AddNewCandidateButton.Value =
              "Add Politician as Candidate";
            EditPoliticianTitle.InnerText = "Edit Candidate Information";
            break;

          case DataMode.ManageIncumbents:
            ManagePoliticiansMessage.InnerText = "No incumbents were found for this office";
            AddExistingCandidateButton.Value = AddNewCandidateButton.Value =
              "Add Politician as Incumbent";
            EditPoliticianTitle.InnerText = "Edit Incumbent Information";
            break;
        }
      }
    }

    internal static string GetCandidateHtml(string electionKey, string politicianKey,
      string officeKey, DataMode mode)
    {
      //var isRunningMateOffice = Offices.GetIsRunningMateOffice(officeKey, false);
      var isRunningMateOffice = Offices.IsRunningMateOfficeInElection(electionKey, officeKey);
      var placeHolder = new PlaceHolder();
      DataRow politician = null;
      switch (mode)
      {
        case DataMode.ManageCandidates:
          politician = Politicians.GetCandidateData(electionKey, politicianKey, false);
          // because this is always for an add, get incumbency from OfficesOfficials
          politician["IsIncumbent"] =
            OfficesOfficials.OfficeKeyPoliticianKeyExists(officeKey, politicianKey);
          break;

        case DataMode.ManageIncumbents:
          politician = Politicians.GetCandidateData(politicianKey, false);
          break;
      }
      var li = new HtmlLi
      {
        ID = "candidate-" + politician.PoliticianKey(),
        ClientIDMode = ClientIDMode.Static
      }.AddTo(placeHolder, "is-new");
      var outerDiv = new HtmlDiv().AddTo(li, "outer shadow-2");
      CreateCandidateEntry(politician, mode, null, false).AddTo(outerDiv);
      if (isRunningMateOffice) CreateNoRunningMateEntry().AddTo(outerDiv);
      return placeHolder.RenderToString();
    }

    internal static string GetRunningMateHtml(string electionKey, string politicianKey,
      string mainCandidateKey, DataMode mode)
    {
      var partyCodeToSuppress =
        Parties.GetPartyCode(Politicians.GetPartyKey(mainCandidateKey));
      DataRow politician = null;
      switch (mode)
      {
        case DataMode.ManageCandidates:
          politician = Politicians.GetCandidateData(electionKey, politicianKey, true);
          break;

        case DataMode.ManageIncumbents:
          politician = Politicians.GetCandidateData(politicianKey, true);
          break;
      }
      return CreateCandidateEntry(politician, mode, partyCodeToSuppress, false).RenderToString();
    }

    public void LoadControls()
    {
      _AddCandidatesTabInfo.LoadControls();
    }

    public void Update()
    {
      _AddCandidatesTabInfo.ClearValidationErrors();
      _AddCandidatesTabInfo.Update(PageFeedback);
    }

    #endregion Public

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      _SecureAdminPage = VotePage.GetPage<SecureAdminPage>();
      if (_SecureAdminPage == null)
        throw new VoteException(
          "The ManagePoliticiansPanel control can only be used in the SecureAdminPage class");

      _AddNewCandidateSubTabInfo =
        AddNewCandidateSubTabItem.GetSubTabInfo(this, PageFeedback);
      _AddCandidatesTabInfo = AddCandidatesTabItem.GetTabInfo(this, PageFeedback);
      _EditPoliticianDialogInfo = EditPoliticianDialogItem.GetDialogInfo(this);
      _SetupAdDialogInfo = SetupAdDialogItem.GetDialogInfo(this);
      DataItemBase.InitializeGroup(this, "Consolidate");

      if (Mode == DataMode.AddPoliticians)
      {
        UsePoliticianFromListButton.Value = "Cancel Addition";
        UsePoliticianFromListButton.RemoveCssClass("disabled");
        UsePoliticianFromListMessage.Visible = false;
      }

      if (!IsPostBack)
      {
        Page.IncludeCss("~/css/vote/controls/ManagePoliticiansPanel.css");
        var cs = Page.ClientScript;
        var type = GetType();
        const string scriptName = "managePoliticiansPanel";
        if (!cs.IsStartupScriptRegistered(type, scriptName))
          cs.RegisterStartupScript(type, scriptName,
            "require(['vote/controls/managePoliticiansPanel'], function(){{}});", true);

        DataItemBase.InitializeGroup(_SecureAdminPage, "SelectOffice");

        StateCache.Populate(ControlAddNewCandidateStateCode, "Select politician home state",
          Empty);
      }
    }

    #endregion Event handlers and overrides
  }
}