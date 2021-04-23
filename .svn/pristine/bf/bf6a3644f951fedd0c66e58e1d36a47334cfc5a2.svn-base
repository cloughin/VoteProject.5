using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DB.Vote;

namespace Vote.Politician
{
  public partial class IssueQuestions1 : VotePage
  {
    #region Dead code

    protected override void OnPreInit(EventArgs e)
    {
      //if (string.IsNullOrWhiteSpace(Request.QueryString["noredirect"]))
      {
        string query = Request.QueryString.ToString();
        string url = "/politician/updateissues.aspx";
        if (!string.IsNullOrWhiteSpace(query))
          url += "?" + query;
        Response.Redirect(url);
      }
      base.OnPreInit(e);
    }

    //private string Question_Anchor(
    //  string QuestionKey
    //  , string Question
    //  )
    //{
    //  //<A href="Answer.aspx?Question=BUSIraqWar170343">
    //  string QuestionAnchor = "<a href=";
    //  //QuestionAnchor += "Answer.aspx";
    //  //QuestionAnchor += "?Question=" + HttpUtility.UrlEncode(QuestionKey);
    //  QuestionAnchor += db.Url_Politician_Answer(
    //    ViewState["IssueKey"].ToString()
    //    ,QuestionKey
    //    );
    //  QuestionAnchor += ">";
    //  QuestionAnchor += Question;
    //  QuestionAnchor += "</a>";
    //  return QuestionAnchor;
    //}

    //private void QuestionTableRow(string QuestionKey, string Question, string Answer)
    //{
    //  //<A href="Answer.aspx?Issue=AXXBiographical&amp;Question=AXXBiographical170343">
    //  string QuestionAnchor = 
    //    Question_Anchor(
    //      QuestionKey
    //      , db.Html_From_Db_For_Page(Question)
    //      );
    //  //<tr Class="QuestionRow">
    //  HtmlTableRow QuestionRow = db.Add_Tr_To_Table_Return_Tr(QuestionsAnswersTable, "QuestionRow");
    //  //<td Class="tdReportQuestion" align="left"  colspan="1">
    //  db.Add_Td_To_Tr(QuestionRow, QuestionAnchor, "tdReportQuestion", "left", 1);

    //  //Answer = db.Html_From_Db_For_Page(Answer);
    //  if (Answer == string.Empty)
    //    Answer = db.No_Response;
    //  //<td Class="tdReportDetail" align="left"  colspan="1">
    //  db.Add_Td_To_Tr(QuestionRow, db.Html_From_Db_For_Page(Answer), "tdReportDetail", "left", 1);
    //}

    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  if (!IsPostBack)//first time
    //  {
    //    #region ViewState["PoliticianKey"] ViewState["IssueKey"]
    //    //if (!string.IsNullOrEmpty(ViewState["PoliticianKey"].ToString()))
    //    //  //Politician Login
    //    //  ViewState["PoliticianKey"] = ViewState["PoliticianKey"].ToString();
    //    //else
    //    //  //Master User
    //    //  ViewState["PoliticianKey"] = db.PoliticianKey_ViewState();
    //    ViewState["PoliticianKey"] = db.PoliticianKey_ViewState();
    //    string politicianKey = ViewState["PoliticianKey"].ToString();

    //    ViewState["IssueKey"] = string.Empty;
    //    if (!string.IsNullOrEmpty(db.QueryString("Issue")))
    //      ViewState["IssueKey"] = HttpUtility.UrlDecode(db.QueryString("Issue").Trim());
    //    string issueKey = ViewState["IssueKey"].ToString();
    //    #endregion  ViewState["PoliticianKey"] ViewState["IssueKey"]

    //    #region Redirect if Bad Entry
    //    if (politicianKey == string.Empty || issueKey == string.Empty)
    //      db.HandleFatalError("The PoliticianKey and/or IssueKey is missing");
    //    #endregion

    //    try
    //    {

    //      #region HyperLink to Issue.aspx or PoliticianIssue.aspx depending whether in an upcoming viewable election

    //      #region View the Pages of Your Positions and Views that We Provide to Voters

    //      #region Text
    //      if (db.Is_Politician_In_Election_Upcoming_Viewable(
    //        PageCache, politicianKey))
    //      {
    //        HyperLinkViewActual.Text = "View the pages we provide voters comparing your "
    //        //+ db.Issues_Issue(ViewState["IssueKey"].ToString())
    //          + " respones to issues with those of the other candidates.";
    //      }
    //      else
    //      {
    //        HyperLinkViewActual.Text = "View the pages we provide voters presenting your "
    //          + " respones to issues.";
    //      }
    //      #endregion Text

    //      HyperLinkViewActual.NavigateUrl = db.Url_Issue_Or_PoliticianIssue(
    //        issueKey, politicianKey);

    //      #endregion View the Pages of Your Positions and Views that We Provide to Voters

    //      #region Submit Your Pages of Positions and Views to the Google Search Engine

    //      HyperLinkSearchEngineComparePage.NavigateUrl
    //        = db.Url_Politician_SearchEngineSubmitCompare(politicianKey, issueKey);

    //      #endregion Submit Your Pages of Positions and Views to the Google Search Engine

    //      #region Hyperlink - List of Issues
    //      //HyperLinkIssuesList.NavigateUrl = db.Url_Issue(
    //      //    string.Empty
    //      //    , db.Politician_OfficeKey(ViewState["PoliticianKey"].ToString())
    //      //    , db.Politician_Issues_List(ViewState["PoliticianKey"].ToString())
    //      //    , db.StateCode_In_PoliticianKey(ViewState["PoliticianKey"].ToString())
    //      //    );
    //      HyperLinkIssuesList.NavigateUrl
    //        = "/IssueList.aspx?State=" + Politicians.GetStateCodeFromKey(politicianKey);
    //      #endregion Hyperlink - List of Issues
    //      #endregion HyperLink to Issue.aspx or PoliticianIssue.aspx depending whether in an upcoming viewable election

    //      PoliticianName.Text = db.GetPoliticianName(politicianKey);
    //      PoliticianOffice.Text = db.Politician_Current_Office_And_Status(politicianKey);
    //      PoliticianElection.Text = db.Politician_Current_Election(politicianKey);

    //      #region Photo
    //      CandidateImage.ImageUrl = InsertNoCacheIntoUrl(
    //        db.Url_Image_Politician_Or_NoPhoto(politicianKey, db.Image_Size_200_Profile, 
    //         db.Image_Size_200_Profile));
    //      #endregion Photo

    //      //Issue.Text = db.Issues_Issue(ViewState["IssueKey"].ToString());
    //      Issue.Text = db.Issue_Desc(issueKey);

    //      #region Reasons & Objectives | Education | Abortion | Affirmative Action | Crime
    //      //DataRow PoliticianRow = db.PoliticiansRow(ViewState["PoliticianKey"].ToString());
    //      //string officeKey = db.Politicians_Str(politicianKey, "OfficeKey");
    //      string officeKey = 
    //        PageCache.GetTemporary().Politicians.GetOfficeKey(politicianKey);
    //      int Office_Class = db.Office_Class(officeKey);

    //      string SQL = string.Empty;
    //      DataTable OfficeIssuesTable = null;
    //      db.IssueLevelType IssueLevel = db.OfficeLevel2IssueLevelType(Office_Class);
    //      switch (IssueLevel)
    //      {
    //        case db.IssueLevelType.All:
    //          SQL = string.Empty;
    //          SQL += " SELECT ";
    //          SQL += " IssueKey ";
    //          SQL += " ,Issue ";
    //          SQL += " ,IssueOrder ";
    //          SQL += " ,IsIssueOmit ";
    //          SQL += " FROM Issues ";
    //          SQL += " WHERE IssueLevel = " + db.SQLLit("A");
    //          SQL += " AND IsIssueOmit = 0";
    //          SQL += " ORDER BY IssueOrder";
    //          //OfficeIssuesTable = db.Table(sql.IssuesAtLevel("A"));
    //          OfficeIssuesTable = db.Table(SQL);
    //          break;
    //        case db.IssueLevelType.National:
    //          SQL = string.Empty;
    //          SQL += " SELECT ";
    //          SQL += " IssueKey ";
    //          SQL += " ,Issue ";
    //          SQL += " ,IssueOrder ";
    //          SQL += " ,IsIssueOmit ";
    //          SQL += " FROM Issues ";
    //          SQL += " WHERE (StateCode = 'LL' AND IssueLevel = 'A')";
    //          SQL += " OR (StateCode = 'US' AND IssueLevel = 'B')";
    //          SQL += " AND IsIssueOmit = 0";
    //          SQL += " ORDER BY IssueOrder";
    //          //OfficeIssuesTable = db.Table(sql.Issues4National());
    //          OfficeIssuesTable = db.Table(SQL);
    //          break;
    //        default:
    //          SQL = string.Empty;
    //          SQL += " SELECT ";
    //          SQL += " IssueKey ";
    //          SQL += " ,Issue ";
    //          SQL += " ,IssueOrder ";
    //          SQL += " ,IsIssueOmit ";
    //          SQL += " FROM Issues ";
    //          SQL += " WHERE (StateCode = 'ALL' AND IssueLevel = 'A')";
    //          //SQL += " OR (StateCode = " + db.SQLLit(StateCode) + " AND IssueLevel = 'C')";
    //          SQL += " OR (StateCode = " + db.SQLLit(db.Offices_Str(officeKey, "StateCode"))
    //            + " AND IssueLevel = 'C')";
    //          SQL += " AND IsIssueOmit = 0";
    //          SQL += " ORDER BY IssueOrder";
    //          //OfficeIssuesTable = db.Table(sql.Issues4State(
    //          //  db.Offices_Str(OfficeKey, "StateCode")));
    //          OfficeIssuesTable = db.Table(SQL);
    //          break;
    //      }


    //      string ComparisonLinks = string.Empty;
    //      bool isfirst = true;
    //      foreach (DataRow IssueRow in OfficeIssuesTable.Rows)
    //      {
    //        if (isfirst)
    //          isfirst = false;
    //        else
    //          ComparisonLinks += " | ";
    //        string IssueLinkTitle = "<nobr>" + IssueRow["Issue"].ToString() + "</nobr>";
    //        string ComparisonIssueKey = IssueRow["IssueKey"].ToString();
    //        //<a href = "IssueQuestions.aspx?Issue=BUSCivilLiberties"> Issue </a>
    //        ComparisonLinks += "<a href="
    //          + "\""
    //          //+ "/Politician/IssueQuestions.aspx"
    //          //+ db.Url_Politician_IssueQuestions(IssueRow["IssueKey"].ToString())
    //          + db.Url_Politician_IssueQuestions(
    //                IssueRow["IssueKey"].ToString()
    //          //, db.User_Name()
    //                )
    //          + "\""
    //          + ">";
    //        ComparisonLinks += "<nobr>" + IssueRow["Issue"].ToString() + "</nobr>";
    //        ComparisonLinks += "</a>";
    //      }

    //      // push out the links row
    //      HtmlTableRow OtherIssuesRow = db.Add_Tr_To_Table_Return_Tr(IssueLinksTable, "OtherIssuesRow");//<tr Class="OtherComparisonsRow">
    //      //db.Add_Td_To_Tr(OtherIssuesRow, "NavbarComparisonsLinks", ComparisonLinks, "left", "1");//<td Class="NavbarComparisonsLinks" align="left"  colspan="1">
    //      db.Add_Td_To_Tr(OtherIssuesRow, ComparisonLinks, "NavbarComparisonsLinks", "left", 1);//<td Class="IssueLinks" align="left"  colspan="1">
    //      #endregion


    //      #region Questions and Answers Table
    //      SQL = string.Empty;
    //      SQL = "SELECT * FROM Questions WHERE ";
    //      SQL += " IssueKey = " + db.SQLLit(issueKey);
    //      SQL += " AND  IsQuestionOmit = 0";
    //      //SQL += " AND  xIsQuestionTagForDeletion = 0";
    //      SQL += " ORDER BY QuestionOrder";
    //      DataTable QuestionTable = db.Table(SQL);//make Query Table
    //      foreach (DataRow QuestionRow in QuestionTable.Rows)
    //      {
    //        string QuestionKey = QuestionRow["QuestionKey"].ToString().Trim();
    //        string Question = QuestionRow["Question"].ToString().Trim();

    //        string Answer = db.Answer_Issue_Question(politicianKey, QuestionKey, false, false, false);//exclude last name, source, datestamp
    //        QuestionTableRow(QuestionKey, Question, Answer);
    //      }
    //      #endregion

    //      Msg.Text = db.Msg("Use this from to express your views and positions on issues of your choosing.");

    //    }
    //    catch (Exception ex)
    //    {
    //      #region
    //      Msg.Visible = true;
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //      #endregion
    //    }
    //  }
    //}

    #endregion Dead code

  }
}
