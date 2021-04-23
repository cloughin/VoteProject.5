using System;
using System.Data;
using System.Globalization;
using System.Web.UI.HtmlControls;
using DB.VoteLog;

namespace Vote.Admin
{
  public partial class IssuesReportPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region from db

    public static string CandidateQuestions4IssueAll(string issueKey)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Questions.QuestionKey ";
      sql += " ,Questions.Question ";
      sql += " ,Questions.IsQuestionOmit ";
      //sql += " ,Questions.xIsQuestionTagForDeletion ";
      sql += " FROM Questions ";
      sql += " WHERE Questions.IssueKey = " + SqlLit(issueKey);
      sql += " ORDER BY Questions.QuestionOrder";
      return sql;
    }

    public static string IssuesAtLevelAll(string issueLevel, string stateCode)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " IssueKey ";
      sql += " ,Issue ";
      sql += " ,IssueOrder ";
      sql += " ,IsIssueOmit ";
      //sql += " ,xIsIssueTagForDeletion ";
      sql += " FROM Issues ";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " AND IssueLevel = " + SqlLit(issueLevel);
      sql += " ORDER BY IssueOrder";
      return sql;
    }

    public static string Fail(string msg) =>
      $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    public static void Log_Error_Admin(Exception ex, string message = null)
    {
      var logMessage = string.Empty;
      var stackTrace = string.Empty;
      if (ex != null)
      {
        logMessage = ex.Message;
        stackTrace = ex.StackTrace;
      }
      if (!string.IsNullOrWhiteSpace(message))
      {
        if (!string.IsNullOrWhiteSpace(logMessage))
          logMessage += " :: ";
        logMessage += message;
      }
      LogErrorsAdmin.Insert(DateTime.Now, UrlManager.GetCurrentPathUri(true).ToString(),
        logMessage, stackTrace);
    }

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    private static HtmlTableRow Add_Tr_To_Table_Return_Tr(HtmlTable htmlTable, string rowClass,
      string align)
    {
      //<tr Class="RowClass">
      var htmlTr = new HtmlTableRow();
      if (rowClass != string.Empty)
        htmlTr.Attributes["Class"] = rowClass;
      if (align != string.Empty)
        htmlTr.Attributes["align"] = align;
      //</tr>
      htmlTable.Rows.Add(htmlTr);
      return htmlTr;
    }

    public static HtmlTableRow Add_Tr_To_Table_Return_Tr(HtmlTable htmlTable, string rowClass) => 
      Add_Tr_To_Table_Return_Tr(htmlTable, rowClass, string.Empty);

    private static void Add_Td_To_Tr(HtmlTableRow htmlTr, string text, string tdClass, string align,
      int colspan = 0)
    {
      //<td Class="TdClass">
      var htmlTableCell = new HtmlTableCell {InnerHtml = text};
      if (tdClass != string.Empty)
        htmlTableCell.Attributes["class"] = tdClass;
      if (align != string.Empty)
        htmlTableCell.Attributes["align"] = align;
      if (colspan != 0)
        htmlTableCell.Attributes["colspan"] = colspan.ToString(CultureInfo.InvariantCulture);
      //</td>
      htmlTr.Cells.Add(htmlTableCell);
    }

    private static void Add_Td_To_Tr(HtmlTableRow htmlTr, string text, string tdClass) => 
      Add_Td_To_Tr(htmlTr, text, tdClass, string.Empty);

    #endregion from db

    private void IssueTableReport(DataTable issueTable)
    {
      foreach (DataRow issueRow in issueTable.Rows)
      {
        var issue = "<hr>" + issueRow["Issue"]
          + "<br>[" + issueRow["IssueKey"] + "]";
        if ((bool) issueRow["IsIssueOmit"])
          issue += " O";
        var htmlIssueTr = Add_Tr_To_Table_Return_Tr(HTMLIssuesTable, "PartyRow");
        Add_Td_To_Tr(htmlIssueTr, issue, "tdReportDetail");
        Add_Td_To_Tr(htmlIssueTr, "<hr>", "tdReportDetail");

        var questionsTable = G.Table(CandidateQuestions4IssueAll(issueRow["IssueKey"].ToString()));
        foreach (DataRow questionRow in questionsTable.Rows)
        {
          var sqlText = "Answers ";
          sqlText += " WHERE Answers.QuestionKey = " + SqlLit(questionRow["QuestionKey"].ToString());
          var answers = G.Rows_Count_From(sqlText);
          var question = questionRow["Question"]
            + " (" + answers + ")"
            + " ...[" + questionRow["QuestionKey"] + "]";
          if ((bool) questionRow["IsQuestionOmit"])
            question += " O";
          var htmlQuestionRow = Add_Tr_To_Table_Return_Tr(HTMLIssuesTable, "PartyRow");
          Add_Td_To_Tr(htmlQuestionRow, string.Empty, "tdReportDetail");
          Add_Td_To_Tr(htmlQuestionRow, question, "tdReportDetail");
        }
      }
    }

    private void ShowReport(string issueLevel, string issuesGroup)
    {
      //IssuesGroup can be either: LL (for All), US or StateCode
      //in Page html <table id="HTMLIssuesTable" align="center" cellSpacing="0" width="760" runat="server">
      var headingRow = Add_Tr_To_Table_Return_Tr(HTMLIssuesTable, "HeadingRow");
        //<tr Class="HeadingRow">

      #region Heading <td Class="tdReportGroupHeadingLeft" align="left">

      Add_Td_To_Tr(headingRow
        , "Issue on Navbar" + "<br>[Issue ID] O D"
        , "tdReportGroupHeadingLeft");
      Add_Td_To_Tr(headingRow
        ,
        "Question (Answers) ...[Question ID] O D" +
        "<br>(What is your position and views regarding)"
        , "tdReportGroupHeadingLeft");

      #endregion

      var issueTable = G.Table(IssuesAtLevelAll(issueLevel, issuesGroup));
      IssueTableReport(issueTable);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Title = H1.InnerText = "Issues Report";

        ViewState["StateCode"] = G.State_Code();
        ViewState["CountyCode"] = G.County_Code();
        ViewState["LocalCode"] = G.Local_Code();
        //if (!db.Is_User_Security_Ok())
        //  SecurePage.HandleSecurityException();

        #region ViewState Values

        ViewState["IssueLevel"] = string.Empty;
        ViewState["IssuesGroup"] = string.Empty;

        #region ViewState["IssueLevel"]  ViewState["IssuesGroup"]

        if (!string.IsNullOrEmpty(GetQueryString("IssueLevel")))
          ViewState["IssueLevel"] = GetQueryString("IssueLevel");
        if (!string.IsNullOrEmpty(GetQueryString("Group")))
          ViewState["IssuesGroup"] = GetQueryString("Group");

        #endregion

        #endregion ViewState Values

        #region Additional Security

        if ((ViewState["IssueLevel"].ToString() == string.Empty)
          || (ViewState["IssuesGroup"].ToString() == string.Empty))
        {
          HandleFatalError("IssueLevel and/or IssueGroup is missing");
        }

        #endregion Additional Security

        try
        {
          ShowReport(ViewState["IssueLevel"].ToString(), ViewState["IssuesGroup"].ToString());
        }
        catch (Exception ex)
        {
          Msg.Text = Fail(ex.Message);
          Log_Error_Admin(ex);
        }
      }
    }
  }
}