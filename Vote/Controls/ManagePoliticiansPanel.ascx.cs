using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using Vote.Reports;

namespace Vote.Controls
{
  public partial class ManagePoliticiansPanel : UserControl
  {
    #region Private

    private SecureAdminPage _SecureAdminPage;

    private static Control CreateCandidateEntry(DataRow candidate,
      DataMode mode, string partyCodeToSuppress = null)
    {
      var isRunningMate = candidate.IsRunningMate();
      var modeDescription = mode == DataMode.ManageIncumbents ? "incumbent" : "candidate";
      var innerDiv =
        new HtmlDiv().AddCssClasses("candidate " +
          (isRunningMate ? "running-mate mate" : "main"));
      if (isRunningMate)
        new HtmlInputHidden {Value = candidate.PoliticianKey()}.AddTo(innerDiv,
          "mate-id");

      var cellDiv = new HtmlDiv().AddTo(innerDiv, "cell pic");
      Report.CreatePoliticianImageTag(candidate.PoliticianKey(), 35)
        .AddTo(cellDiv);

      cellDiv = new HtmlDiv().AddTo(innerDiv, "cell data data1");
      if (isRunningMate)
      {
        var mateDiv = new HtmlDiv().AddTo(cellDiv, "mate-header");
        new LiteralControl("Running Mate").AddTo(mateDiv);
      }

      var itemDiv = new HtmlDiv().AddTo(cellDiv, "name");
      new LiteralControl(Politicians.FormatName(candidate, true)).AddTo(itemDiv);
      var partyCode = candidate.PartyCode();
      if (!string.IsNullOrWhiteSpace(partyCode) && (partyCode != partyCodeToSuppress))
      {
        new LiteralControl(" (").AddTo(itemDiv);
        var anchor = Report.CreatePartyAnchor(candidate, "view")
          .AddTo(itemDiv);
        if (anchor is HtmlAnchor) anchor.AddCssClasses("tiptip");
        new LiteralControl(")").AddTo(itemDiv);
      }
      if ((mode == DataMode.ManageCandidates) && !isRunningMate && candidate.IsIncumbent())
        new HtmlSpan {InnerHtml = " •"}.AddTo(itemDiv, "tiptip")
          .Attributes["title"] = "Candidate is incumbent for this election";

      //itemDiv = new HtmlDiv().AddTo(cellDiv, "header");
      //new LiteralControl("Address / City State Zip").AddTo(itemDiv);

      //var text = candidate.PublicAddress();
      var text = candidate.StateOrCandidateAddress();
      var className = string.Empty;
      if (string.IsNullOrWhiteSpace(text))
      {
        text = "Address not available";
        className = " na";
      }
      itemDiv = new HtmlDiv().AddTo(cellDiv,
        "data-item address" + className);
      new LiteralControl(text).AddTo(itemDiv);

      //text = candidate.PublicCityStateZip();
      text = candidate.StateOrCandidateCityStateZip();
      className = string.Empty;
      if (string.IsNullOrWhiteSpace(text))
      {
        text = "City State Zip not available";
        className = " na";
      }
      itemDiv = new HtmlDiv().AddTo(cellDiv,
        "data-item city-state-zip" + className);
      new LiteralControl(text).AddTo(itemDiv);

      cellDiv = new HtmlDiv().AddTo(innerDiv, "cell data data2");

      //itemDiv = new HtmlDiv().AddTo(cellDiv, "header");
      //new LiteralControl("Phone / Email / Web Site").AddTo(itemDiv);

      //text = candidate.PublicPhone();
      text = candidate.StateOrCandidatePhone();
      className = string.Empty;
      if (string.IsNullOrWhiteSpace(text))
      {
        text = "Phone not available";
        className = " na";
      }
      itemDiv = new HtmlDiv().AddTo(cellDiv,
        "data-item phone" + className);
      new LiteralControl(text).AddTo(itemDiv);

      //text = candidate.PublicEmail();
      text = candidate.StateOrCandidateEmail();
      className = string.Empty;
      Control control;
      if (string.IsNullOrWhiteSpace(text))
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
      itemDiv = new HtmlDiv().AddTo(cellDiv,
        "data-item email" + className);
      control.AddTo(itemDiv);

      //text = candidate.PublicWebAddress();
      text = candidate.StateOrCandidateWebAddress();
      className = string.Empty;
      if (string.IsNullOrWhiteSpace(text))
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
      itemDiv = new HtmlDiv().AddTo(cellDiv,
        "data-item web-site" + className);
      control.AddTo(itemDiv);

      cellDiv = new HtmlDiv().AddTo(innerDiv, "cell icons");

      new HtmlDiv().AddTo(cellDiv, "icon-move tiptip")
        .Attributes["title"] = "Drag to reorder the " + modeDescription + "s";

      var menu = new HtmlDiv().AddTo(cellDiv, "candidate-menu");
      var ul1 = new HtmlUl().AddTo(menu, "candidate-menu");
      var li1 = new HtmlLi().AddTo(ul1);
      var a1 = new HtmlAnchor().AddTo(li1);
      new HtmlDiv().AddTo(a1, "icon icon-menu");
      var ul2 = new HtmlUl().AddTo(li1);
      var li2 = new HtmlLi().AddTo(ul2, "rounded-border");
      new HtmlAnchor {InnerHtml = "Edit " + modeDescription + "'s information"}.AddTo(li2,
          "edit-candidate-link")
        .Attributes["pkey"] = candidate.PoliticianKey();
      li2 = new HtmlLi().AddTo(ul2, "rounded-border");
      new HtmlAnchor
      {
        InnerHtml = "View " + modeDescription + "'s public page",
        HRef = UrlManager.GetIntroPageUri(candidate.PoliticianKey())
          .ToString(),
        Target = "view"
      }.AddTo(li2);
      if (SecurePage.IsMasterUser)
      {
        li2 = new HtmlLi().AddTo(ul2, "rounded-border");
        new HtmlAnchor
        {
          InnerHtml = "Edit " + modeDescription + "'s public page",
          HRef =
            SecurePoliticianPage.GetUpdateIntroPageUrl(candidate.PoliticianKey()),
          Target = "view"
        }.AddTo(li2);
        li2 = new HtmlLi().AddTo(ul2, "rounded-border");
        new HtmlAnchor
        {
          InnerHtml = "Go to " + modeDescription + "'s admin page",
          HRef =
            SecureAdminPage.GetPoliticianPageUrl(candidate.PoliticianKey()),
          Target = "view"
        }.AddTo(li2);
        li2 = new HtmlLi().AddTo(ul2, "rounded-border");
        new HtmlAnchor {InnerHtml = "Get " + modeDescription + "'s key"}.AddTo(li2,
            "get-candidate-key-link")
          .Attributes["pkey"] = candidate.PoliticianKey();
      }
      new HtmlDiv().AddTo(cellDiv, "clear-both");

      new HtmlDiv().AddTo(cellDiv, "icon icon-remove tiptip")
        .Attributes["title"] = isRunningMate
        ? "Check to remove this running mate"
        : "Check to remove this " + modeDescription;

      new HtmlDiv().AddTo(innerDiv, "clear-both");

      return innerDiv;
    }

    private static Control CreateNoRunningMateEntry()
    {
      var innerDiv =
        new HtmlDiv().AddCssClasses(
          "candidate running-mate no-mate");

      new HtmlDiv().AddTo(innerDiv, "cell pic");

      var cellDiv = new HtmlDiv().AddTo(innerDiv, "cell data data1");
      var mateDiv = new HtmlDiv().AddTo(cellDiv, "mate-header");
      new LiteralControl("No Running Mate").AddTo(mateDiv);

      new HtmlDiv().AddTo(innerDiv, "cell data data2");

      cellDiv = new HtmlDiv().AddTo(innerDiv, "cell icons");

      new HtmlDiv().AddTo(cellDiv, "icon icon-add-mate tiptip")
        .Attributes["title"] = "Add running mate";

      new HtmlDiv().AddTo(innerDiv, "clear-both");

      return innerDiv;
    }

    private string SafeGetOfficeKey() => 
      GetOfficeKey == null
      ? string.Empty
      : GetOfficeKey();

    private string SafeGetElectionKey() => 
      GetElectionKey == null
      ? string.Empty
      : GetElectionKey();

    private string GetPoliticianKeyToEdit() => CandidateToEdit.Value;

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
            AddExistingCandidateButton.Value = AddNewCandidateButton.Value = "Add Politician";
            EditPoliticianTitle.InnerText = "Edit Politician Information";
            break;

          case DataMode.ManageCandidates:
            ManagePoliticiansMessage.InnerText = "No candidates were found for this office";
            AddExistingCandidateButton.Value =
              AddNewCandidateButton.Value = "Add Politician as Candidate";
            EditPoliticianTitle.InnerText = "Edit Candidate Information";
            break;

          case DataMode.ManageIncumbents:
            ManagePoliticiansMessage.InnerText = "No incumbents were found for this office";
            AddExistingCandidateButton.Value =
              AddNewCandidateButton.Value = "Add Politician as Incumbent";
            EditPoliticianTitle.InnerText = "Edit Incumbent Information";
            break;
        }
      }
    }

    internal static string GetCandidateHtml(string electionKey,
      string politicianKey, string officeKey, DataMode mode)
    {
      var isRunningMateOffice = Offices.GetIsRunningMateOffice(officeKey, false);
      var placeHolder = new PlaceHolder();
      DataRow politician = null;
      switch (mode)
      {
        case DataMode.ManageCandidates:
          politician = Politicians.GetCandidateData(electionKey, politicianKey, false);
          break;

        case DataMode.ManageIncumbents:
          politician = Politicians.GetCandidateData(politicianKey, false);
          break;
      }
      var li =
        new HtmlLi
        {
          ID = "candidate-" + politician.PoliticianKey(),
          ClientIDMode = ClientIDMode.Static
        }.AddTo(placeHolder);
      var outerDiv = new HtmlDiv().AddTo(li, "outer shadow-2");
      CreateCandidateEntry(politician, mode)
        .AddTo(outerDiv);
      if (isRunningMateOffice)
        CreateNoRunningMateEntry()
          .AddTo(outerDiv);
      return placeHolder.RenderToString();
    }

    internal static string GetRunningMateHtml(string electionKey,
      string politicianKey, string mainCandidateKey, DataMode mode)
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
      return CreateCandidateEntry(politician, mode, partyCodeToSuppress)
        .RenderToString();
    }

    public void LoadControls() => _AddCandidatesTabInfo.LoadControls();

    public void Update() => _AddCandidatesTabInfo.Update(PageFeedback);

    #endregion Public

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      _SecureAdminPage = VotePage.GetPage<SecureAdminPage>();
      if (_SecureAdminPage == null)
        throw new VoteException(
          "The ManagePoliticiansPanel control can only be used in the SecureAdminPage class");

      _AddNewCandidateSubTabInfo = AddNewCandidateSubTabItem.GetSubTabInfo(this, PageFeedback);
      _AddCandidatesTabInfo = AddCandidatesTabItem.GetTabInfo(this, PageFeedback);
      _EditPoliticianDialogInfo = EditPoliticianDialogItem.GetDialogInfo(this);
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

        StateCache.Populate(ControlAddNewCandidateStateCode,
          "Select politician home state", string.Empty);
      }
    }

    #endregion Event handlers and overrides
  }
}