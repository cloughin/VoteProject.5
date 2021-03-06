using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Admin
{
  public partial class IssuesPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region from db

    public static bool Is_Valid_Issue(string issueKey)
    {
      if (string.IsNullOrEmpty(issueKey)) return false;
      if ((issueKey.Length == 13) && (issueKey.Substring(3, 10)
        .ToUpper() == "ISSUESLIST"))
        return true;
      if ((issueKey.Length == 6) && (issueKey.Substring(3, 3)
        .ToUpper() == "BIO"))
        return true;
      return G.Rows("Issues", "IssueKey", issueKey) == 1;
    }

    public static string Ok(string msg) =>
      $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    public static string Fail(string msg) =>
      $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    public static string Message(string msg) =>
      $"<span class=\"Msg\">{msg}</span>";

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

    public static bool Is_Valid_Integer(string number2Check)
    {
      int value;
      return int.TryParse(number2Check, out value);
    }

    public static bool Is_TextBox_Html(TextBox textBox) => 
      (textBox.Text.IndexOf("<", StringComparison.Ordinal) >= 0)
      || (textBox.Text.IndexOf("/>", StringComparison.Ordinal) >= 0);

    public static bool Is_Str_Script(string strToCheck) => 
      strToCheck.Trim().ToUpper().IndexOf("<SCRIPT", StringComparison.Ordinal) >= 0;

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    public static string Str_ReCase(string str2Fix)
    {
      var sb = new StringBuilder(str2Fix.Length);
      var wordBegin = true;
      foreach (var c in str2Fix)
      {
        sb.Append(wordBegin
          ? char.ToUpper(c)
          : char.ToLower(c));
        //wordBegin = char.IsWhiteSpace(c);
        if (
          char.IsWhiteSpace(c)
          || char.IsPunctuation(c)
        )
          wordBegin = true;
        else
          wordBegin = false;
      }
      return sb.ToString();
    }

    public static string Str_Remove_SpecialChars_All(string str2Modify)
    {
      var str = str2Modify;
      str = str.Trim();
      str = str.Replace("-", string.Empty);
      str = str.Replace("+", string.Empty);
      str = str.Replace("=", string.Empty);
      str = str.Replace("\"", string.Empty);
      str = str.Replace("\'", string.Empty);
      str = str.Replace(".", string.Empty);
      str = str.Replace(",", string.Empty);
      str = str.Replace("(", string.Empty);
      str = str.Replace(")", string.Empty);
      str = str.Replace("!", string.Empty);
      str = str.Replace("@", string.Empty);
      str = str.Replace("#", string.Empty);
      str = str.Replace("%", string.Empty);
      str = str.Replace("&", string.Empty);
      str = str.Replace("*", string.Empty);
      str = str.Replace(":", string.Empty);
      str = str.Replace(";", string.Empty);
      str = str.Replace("$", string.Empty);
      str = str.Replace("^", string.Empty);
      str = str.Replace("?", string.Empty);
      str = str.Replace("<", string.Empty);
      str = str.Replace(">", string.Empty);
      str = str.Replace("[", string.Empty);
      str = str.Replace("]", string.Empty);
      str = str.Replace("{", string.Empty);
      str = str.Replace("}", string.Empty);
      str = str.Replace("|", string.Empty);
      str = str.Replace("~", string.Empty);
      str = str.Replace("`", string.Empty);
      str = str.Replace("_", string.Empty);
      str = str.Replace("/", string.Empty);
      return str;
    }

    public static string Str_Remove_Non_Key_Chars(string str2Modify)
    {
      var str = str2Modify;
      str = Str_Remove_SpecialChars_All(str);
      str = str.Replace(" ", string.Empty);
      return str;
    }

    public static string DbErrorMsg(string sql, string err) =>
      $"Database Failure for SQL Statement::{sql} :: Error Msg:: {err}";

    public static Random RandomObject;

    public static char GetRandomDigit()
    {
      if (RandomObject == null)
        RandomObject = new Random();
      return Convert.ToChar(RandomObject.Next(10) + Convert.ToUInt32('0'));
    }

    public static string MakeUnique6Digits()
    {
      var password = string.Empty;
      //Make a DDDDDD password
      for (var n = 0; n < 6; n++)
        password += GetRandomDigit();
      return password;
    }

    public static string Fix_Url_Parms(string url)
    {
      //sets first parm in a query string to ? if all seperators are &'s
      if ((url.IndexOf('?', 0, url.Length) == -1) //no ?
        && (url.IndexOf('&', 0, url.Length) > -1)) //but one or more &
      {
        var loc = url.IndexOf('&', 0, url.Length);
        var lenAfter = url.Length - loc - 1;
        var urlBefore = url.Substring(0, loc);
        var urlAfter = url.Substring(loc + 1, lenAfter);
        return urlBefore + "?" + urlAfter;
      }
      return url;
    }

    public static DataRow Row_Optional(string sql)
    {
      var table = G.Table(sql);
      return table.Rows.Count == 1
        ? table.Rows[0]
        : null;
    }

    public static string Single_Key_Str(string tableName, string column, string keyName,
      string keyValue)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
        + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());

      var table = G.Table(sql);
      if (table.Rows.Count == 1)
      {
        // modified to pass through nulls (Politician.Address etc)
        //return (string)table.Rows[0][Column].ToString().Trim();
        var result = table.Rows[0][column] as string;
        result = result?.Trim();
        return result;
      }
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    public static int Single_Key_Int(string tableName, string column, string keyName,
      string keyValue)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
        + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());
      var table = G.Table(sql);
      if (table.Rows.Count == 1)
      {
        return (int) table.Rows[0][column];
      }
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    public static bool Single_Key_Bool(string tableName, string column, string keyName,
      string keyValue)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
        + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());
      var table = G.Table(sql);
      if (table.Rows.Count == 1)
      {
        return Convert.ToBoolean(table.Rows[0][column]);
      }
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    public static void Single_Key_Update_Str(string table, string column, string columnValue,
      string keyName, string keyValue) => 
      Db.UpdateColumnByKey(table, column, columnValue, keyName, keyValue);

    public static void Single_Key_Update_Int(string table, string column, int columnValue,
      string keyName, string keyValue)
    {
      var updateSql =
        $"UPDATE {table} SET {column} = {columnValue} WHERE {keyName} = {SqlLit(keyValue)}";
      G.ExecuteSql(updateSql);
    }

    public static void Single_Key_Update_Bool(string table, string column, bool columnValue,
      string keyName, string keyValue)
    {
      var updateSql =
        $"UPDATE {table} SET {column} = {Convert.ToUInt16(columnValue)} WHERE {keyName} = {SqlLit(keyValue)}";
      G.ExecuteSql(updateSql);
    }

    public static string IssueGroup_IssueKey(string issueKey) => 
      issueKey.Length >= 3
      ? issueKey.Substring(1, 2).ToUpper()
      : string.Empty;

    public static string Issues_List(string issueKey)
    {
      if (string.IsNullOrEmpty(issueKey)) return string.Empty;
      var issueLevel = Issues.GetIssueLevelFromKey(issueKey);
      var stateCode = IssueGroup_IssueKey(issueKey); //LL, US or StateCode
      return issueLevel + stateCode + "IssuesList";
    }

    public static bool Is_Valid_Question(string questionKey)
    {
      //if (db.Row_Optional(sql.QuestionKey(QuestionKey)) != null)
      var sql = " SELECT";
      sql += " Questions.QuestionKey";
      sql += " FROM Questions";
      sql += " WHERE Questions.QuestionKey =" + SqlLit(questionKey);
      return Row_Optional(sql) != null;
    }

    public static string Questions_Str(string questionKey, string column)
    {
      if (questionKey != string.Empty)
        return Single_Key_Str("Questions", column, "QuestionKey", questionKey);
      return string.Empty;
    }

    public static void Throw_Exception_TextBox_Html(TextBox textBox)
    {
      if (Is_TextBox_Html(textBox))
        throw new ApplicationException(
          "Text in a textbox appears to be HTML because it contains an opening or closing HTML tag (< or />). Please remove and try again.");
    }

    public static void Throw_Exception_TextBox_Script(TextBox textBox)
    {
      if (Is_Str_Script(textBox.Text))
        throw new ApplicationException("Text in the textbox is illegal.");
    }

    public static void Throw_Exception_TextBox_Html_Or_Script(TextBox textBox)
    {
      Throw_Exception_TextBox_Html(textBox);
      Throw_Exception_TextBox_Script(textBox);
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

    private static string Url_Admin_Issues_Question_Omit(string stateCode,
      string issueKey, string questionKey, bool isOmit)
    {
      var url = string.Empty;
      url += Url_Admin_Issues();
      if (!string.IsNullOrEmpty(stateCode))
        url += "&State=" + stateCode;
      if (!string.IsNullOrEmpty(issueKey))
        url += "&Issue=" + issueKey;
      if (!string.IsNullOrEmpty(questionKey))
        url += "&Question=" + questionKey;
      if (isOmit)
        url += "&Omit=true";
      else
        url += "&Omit=false";

      return Fix_Url_Parms(url);
    }

    private static void Questions_Update_IsQuestionOmit(string questionKey, bool columnValue) => 
      Questions_Update_Bool(questionKey, "IsQuestionOmit", columnValue);

    private static void Questions_Update_Bool(string questionKey, string column, bool columnValue) => 
      Single_Key_Update_Bool("Questions", column, columnValue, "QuestionKey", questionKey);

    private static void Issues_Update_IsIssueOmit(string issueKey, bool columnValue) => 
      Issues_Update_Bool(issueKey, "IsIssueOmit", columnValue);

    private static void Issues_Update_Bool(string issueKey, string column, bool columnValue) => 
      Single_Key_Update_Bool("Issues", column, columnValue, "IssueKey", issueKey);

    private static void Questions_Update_Question(string questionKey, string columnValue) =>
      Questions_Update_Str(questionKey, "Question", columnValue);

    private static void Questions_Update_Str(string questionKey, string column, string columnValue) =>
      Single_Key_Update_Str("Questions", column, columnValue, "QuestionKey", questionKey);

    private static void Issues_Update_Issue(string issueKey, string columnValue) =>
      Issues_Update_Str(issueKey, "Issue", columnValue);

    private static void Issues_Update_Str(string issueKey, string column, string columnValue) => 
      Single_Key_Update_Str("Issues", column, columnValue, "IssueKey", issueKey);

    private static int Questions_QuestionOrder(string questionKey) => 
      Questions_Int(questionKey, "QuestionOrder");

    private static int Questions_Int(string questionKey, string column) => 
      questionKey != string.Empty
      ? Single_Key_Int("Questions", column, "QuestionKey", questionKey)
      : 0;

    private static string Questions_Question(string questionKey) => 
      Questions_Str(questionKey, "Question");

    private static int Issues_IssueOrder(string issueKey) => 
      Issues_Int(issueKey, "IssueOrder");

    private static int Issues_Int(string issueKey, string column) => 
      issueKey != string.Empty
      ? Single_Key_Int("Issues", column, "IssueKey", issueKey)
      : 0;

    private static string Issue_Desc(string issueKey) => 
      Issue_Desc(GetPageCache(), issueKey);

    private static string Issue_Desc(PageCache cache, string issueKey)
    {
      if (issueKey == Issues_List(issueKey))
        return "List of Issues";
      return issueKey.ToUpper() == "ALLBIO"
        ? "Biographical Information"
        : cache.Issues.GetIssue(issueKey);
    }

    private static void Questions_Update_QuestionOrder(string questionKey, int columnValue) => 
      Questions_Update_Int(questionKey, "QuestionOrder", columnValue);

    private static void Questions_Update_Int(string questionKey, string column, int columnValue) => 
      Single_Key_Update_Int("Questions", column, columnValue, "QuestionKey", questionKey);

    private static void Issues_Update_IssueOrder(string issueKey, int columnValue) => 
      Issues_Update_Int(issueKey, "IssueOrder", columnValue);

    private static void Issues_Update_Int(string issueKey, string column, int columnValue) => 
      Single_Key_Update_Int("Issues", column, columnValue, "IssueKey", issueKey);

    private static string Anchor_Admin_Question_Issues_Omit(string stateCode,
      string issueKey, string questionKey, string anchorText, bool isOmit)
    {
      var anchor = string.Empty;
      anchor += "<a href="; //
      anchor += "\"";
      anchor += Url_Admin_Issues_Question_Omit(stateCode, issueKey, questionKey, isOmit);
      anchor += "\"";
      anchor += ">";
      anchor += "<nobr>" + anchorText + "</nobr>";
      anchor += "</a>";
      return anchor;
    }

    private static bool Is_Question_Omit(string questionKey) => 
      Questions_Bool(questionKey, "IsQuestionOmit");

    private static bool Questions_Bool(string questionKey, string column) => 
      (questionKey != string.Empty) &&
      Single_Key_Bool("Questions", column, "QuestionKey", questionKey);

    private static string Anchor_Admin_Issues_Question(string stateCode, string issueKey,
      string questionKey, string anchorText)
    {
      var anchor = string.Empty;
      anchor += "<a href="; //
      anchor += "\"";
      anchor += Url_Admin_Issues_Question(stateCode, issueKey, questionKey);
      anchor += "\"";
      anchor += ">";
      anchor += "<nobr>" + anchorText + "</nobr>";
      anchor += "</a>";
      return anchor;
    }

    private static string Url_Admin_Issues_Question(string stateCode, string issueKey,
      string questionKey)
    {
      var url = string.Empty;
      url += Url_Admin_Issues();

      if (!string.IsNullOrEmpty(stateCode))
        url += "&State=" + stateCode;
      if (!string.IsNullOrEmpty(issueKey))
        url += "&Issue=" + issueKey;
      if (!string.IsNullOrEmpty(questionKey))
        url += "&Question=" + questionKey;

      return Fix_Url_Parms(url);
    }

    private static string Anchor_Admin_Issues_Omit(string stateCode, string issueKey,
      string anchorText, bool isOmit)
    {
      var anchor = string.Empty;
      anchor += "<a href="; //
      anchor += "\"";
      anchor += Url_Admin_Issues_Omit(stateCode, issueKey, isOmit);
      anchor += "\"";
      anchor += ">";
      anchor += "<nobr>" + anchorText + "</nobr>";
      anchor += "</a>";
      return anchor;
    }

    private static string Url_Admin_Issues_Omit(string stateCode, string issueKey,
      bool isOmit)
    {
      var url = string.Empty;
      url += Url_Admin_Issues();
      if (!string.IsNullOrEmpty(stateCode))
        url += "&State=" + stateCode;
      if (!string.IsNullOrEmpty(issueKey))
        url += "&Issue=" + issueKey;
      if (isOmit)
        url += "&Omit=true";
      else
        url += "&Omit=false";

      return Fix_Url_Parms(url);
    }

    private static bool Is_Issue_Omit(string issueKey) => 
      Issues_Bool(issueKey, "IsIssueOmit");

    private static bool Issues_Bool(string issueKey, string column) => 
      (issueKey != string.Empty) && Single_Key_Bool("Issues", column, "IssueKey", issueKey);

    private static string Anchor_Admin_Issues(string stateCode, string issueKey,
      string anchorText)
    {
      var anchor = string.Empty;
      anchor += "<a href="; //
      anchor += "\"";
      anchor += Url_Admin_Issues(stateCode, issueKey);
      anchor += "\"";
      anchor += ">";
      anchor += "<nobr>" + anchorText + "</nobr>";
      anchor += "</a>";
      return anchor;
    }

    private static string Url_Admin_Issues(string stateCode, string issueKey)
    {
      var url = string.Empty;
      url += Url_Admin_Issues();
      if (!string.IsNullOrEmpty(stateCode))
        url += "&State=" + stateCode;
      if (!string.IsNullOrEmpty(issueKey))
        url += "&Issue=" + issueKey;

      return Fix_Url_Parms(url);
    }

    private static string Url_Admin_Issues() => "/Admin/Issues.aspx";

    #endregion from db

    public override IEnumerable<string> NonStateCodesAllowed { get; } = new[] {"LL", "US"};

    private void MakeAllControlsNotVisible()
    {
      Table_Issue_Edit.Visible = false;
      Table_Issue_Add.Visible = false;

      Table_Question_Edit.Visible = false;
      Table_Question_Add.Visible = false;
      Table_Question.Visible = false;
      Table_Questions_Report.Visible = false;

      Table_Delete_Question.Visible = false;
      Table_Delete_Issue.Visible = false;
    }

    // general methods
    private static int QuestionsCount(string issueKey) => 
      G.Rows_Count_From("Questions WHERE IssueKey = "
      + SqlLit(issueKey));

    private static int AnswersCount(string questionKey) => 
      G.Rows_Count_From("Answers WHERE QuestionKey = "
      + SqlLit(questionKey));

    private static string GetIssueLevelInIssueKey(string issueKey) => 
      !string.IsNullOrEmpty(issueKey) ? issueKey.Substring(0, 1) : string.Empty;

    private string MakeIssueKey(
      string countyCode
      , string localCode
      , string issue
    )
    {
      //IssueKey is also the first part of the QuestionsKey.
      var issueLevel = string.Empty;
      switch (ViewState["StateCode"].ToString().ToUpper())
      {
        case "LL":
          issueLevel += "A";
          break;
        case "US":
          issueLevel += "B";
          break;
        default:
          if (!string.IsNullOrEmpty(localCode))
            issueLevel += "E";
          else if (!string.IsNullOrEmpty(countyCode))
            issueLevel += "D";
          else if (!string.IsNullOrEmpty(ViewState["StateCode"].ToString()))
            issueLevel += "C";
          else
            throw new ApplicationException("IssueLevel invalid in Issue_Key");
          break;
      }

      var issueKey = issueLevel
        + ViewState["StateCode"]
        + issue;
      issueKey = Str_Remove_Non_Key_Chars(issueKey);

      return issueKey;
    }

    // Checks
    private void CheckTextboxesForBadContent()
    {
      //Issues
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Issue_Description);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Issue_Order);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Issue_Description_Add);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Issue_Order_Add);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Delete_IssueKey);
      //Questions
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Question_Description);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Question_Order);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Question_Description_Add);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Question_Order_Add);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Delete_QuestionKey);
    }

    // Issues Report

    private static void HeadingIssuesReport(ref HtmlTable htmlTableIssues)
    {
      //<tr Class="trReportDetailHeading">
      var htmlTrHeading = Add_Tr_To_Table_Return_Tr(
        htmlTableIssues
        , "trReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "Order"
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "Issue"
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "Omit"
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "IssueKey"
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "Questions"
        , "tdReportDetailHeading"
      );
    }

    private void CreateIssuesReportRow(ref HtmlTable htmlTableIssues, DataRow rowIssue)
    {
      //<tr Class="trReportDetail">
      var htmlTrIssue = Add_Tr_To_Table_Return_Tr(
        htmlTableIssues
        , "trReportDetail"
      );

      //Order
      Add_Td_To_Tr(
        htmlTrIssue
        , rowIssue["IssueOrder"].ToString()
        , "tdReportDetail"
      );
      //Issue
      Add_Td_To_Tr(
        htmlTrIssue
        , Anchor_Admin_Issues(
          ViewState["StateCode"].ToString()
          , rowIssue["IssueKey"].ToString()
          , rowIssue["Issue"].ToString()
        )
        , "tdReportDetail"
      );
      //Omit
      string omitAnchor;
      if (Is_Issue_Omit(rowIssue["IssueKey"].ToString()))
      {
        //anchor to reinstate
        omitAnchor = Anchor_Admin_Issues_Omit(
          ViewState["StateCode"].ToString()
          , rowIssue["IssueKey"].ToString()
          , "<strong>OMIT</strong>"
          , false
        );
      }
      else
      {
        //anchor to omit
        omitAnchor = Anchor_Admin_Issues_Omit(
          ViewState["StateCode"].ToString()
          , rowIssue["IssueKey"].ToString()
          , "ok"
          , true
        );
      }
      Add_Td_To_Tr(
        htmlTrIssue
        , omitAnchor
        , "tdReportDetail"
      );

      // IssueKey
      Add_Td_To_Tr(
        htmlTrIssue
        , rowIssue["IssueKey"].ToString()
        , "tdReportDetail"
      );

      // Questions
      Add_Td_To_Tr(
        htmlTrIssue
        , QuestionsCount(rowIssue["IssueKey"].ToString()).ToString(CultureInfo.InvariantCulture)
        , "tdReportDetail"
      );
    }

    private void CreateIssuesReport()
    {
      var htmlTableIssues = new HtmlTable();
      htmlTableIssues.Attributes["cellspacing"] = "0";
      htmlTableIssues.Attributes["border"] = "0";

      // Issues
      HeadingIssuesReport(ref htmlTableIssues);
      var sql = string.Empty;
      sql += "SELECT * FROM Issues ";
      sql += " WHERE StateCode = " + SqlLit(ViewState["StateCode"].ToString());
      sql += " ORDER BY IsIssueOmit ASC,IssueOrder";
      var tableIssues = G.Table(sql);
      foreach (DataRow rowIssue in tableIssues.Rows)
        CreateIssuesReportRow(ref htmlTableIssues, rowIssue);

      //return db.RenderToString(HTML_Table_Issues);
      Label_Issues_Report.Text = htmlTableIssues.RenderToString();
    }

    // Questions Report

    private static void CreateQuestionsReportHeading(HtmlTable htmlTableQuestions)
    {
      //<tr Class="trReportDetailHeading">
      var htmlTrHeading = Add_Tr_To_Table_Return_Tr(
        htmlTableQuestions
        , "trReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "Order"
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "Question"
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "Omit"
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "QuestionKey"
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "Answers"
        , "tdReportDetailHeading"
      );
    }

    private void CreateQuestionsReportRow(ref HtmlTable htmlTableQuestions, DataRow rowQuestion)
    {
      //<tr Class="trReportDetail">
      var htmlTrQuestion = Add_Tr_To_Table_Return_Tr(
        htmlTableQuestions, "trReportDetail");

      //Order
      Add_Td_To_Tr(htmlTrQuestion,
        rowQuestion["QuestionOrder"].ToString(), "tdReportDetail");
      //Question
      Add_Td_To_Tr(
        htmlTrQuestion
        , Anchor_Admin_Issues_Question(
          ViewState["StateCode"].ToString()
          , rowQuestion["IssueKey"].ToString()
          , rowQuestion["QuestionKey"].ToString()
          , rowQuestion["Question"].ToString()
        )
        , "tdReportDetail"
      );
      //Omit
      string omitAnchor;
      if (Is_Question_Omit(
          rowQuestion["QuestionKey"].ToString())
      )
      {
        //anchor to reinstate
        omitAnchor = Anchor_Admin_Question_Issues_Omit(
          ViewState["StateCode"].ToString()
          , rowQuestion["IssueKey"].ToString()
          , rowQuestion["QuestionKey"].ToString()
          , "<strong>OMIT</strong>"
          , false
        );
      }
      else
      {
        //anchor to omit
        omitAnchor = Anchor_Admin_Question_Issues_Omit(
          ViewState["StateCode"].ToString()
          , rowQuestion["IssueKey"].ToString()
          , rowQuestion["QuestionKey"].ToString()
          , "ok"
          , true
        );
      }
      Add_Td_To_Tr(
        htmlTrQuestion
        , omitAnchor
        , "tdReportDetail"
      );

      // QuestionKey
      Add_Td_To_Tr(
        htmlTrQuestion
        , rowQuestion["QuestionKey"].ToString()
        , "tdReportDetail"
      );

      // Answers
      Add_Td_To_Tr(
        htmlTrQuestion
        , AnswersCount(rowQuestion["QuestionKey"].ToString()).ToString(CultureInfo.InvariantCulture)
        , "tdReportDetail"
      );
    }

    private void CreateQuestionsReport()
    {
      var htmlTableQuestions = new HtmlTable();
      htmlTableQuestions.Attributes["cellspacing"] = "0";
      htmlTableQuestions.Attributes["border"] = "0";

      // Questions
      CreateQuestionsReportHeading(htmlTableQuestions);
      var sql = string.Empty;
      sql += "SELECT * FROM Questions ";
      sql += " WHERE IssueKey = " + SqlLit(ViewState["IssueKey"].ToString());
      sql += " ORDER BY IsQuestionOmit ASC,QuestionOrder";
      var tableQuestions = G.Table(sql);
      foreach (DataRow rowIssue in tableQuestions.Rows)
        CreateQuestionsReportRow(ref htmlTableQuestions, rowIssue);

      LabelQuestionsReport.Text = htmlTableQuestions.RenderToString();
    }

    private void RenumberIssues()
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " IssueKey ";
      sql += ",IssueOrder ";
      sql += " FROM Issues ";
      sql += " WHERE StateCode = "
        + SqlLit(ViewState["StateCode"].ToString());
      sql += " ORDER BY IssueOrder";
      var tableIssues = G.Table(sql);
      var count = 10;
      foreach (DataRow rowIssue in tableIssues.Rows)
      {
        Issues_Update_IssueOrder(
          rowIssue["IssueKey"].ToString()
          , count
        );
        count += 10;
      }
    }

    private void RenumberQuestions()
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " QuestionKey ";
      sql += ",QuestionOrder ";
      sql += " FROM Questions ";
      sql += " WHERE IssueKey = "
        + SqlLit(ViewState["IssueKey"].ToString());
      sql += " ORDER BY QuestionOrder";
      var tableQuestions = G.Table(sql);
      var count = 10;
      foreach (DataRow rowQuestion in tableQuestions.Rows)
      {
        Questions_Update_QuestionOrder(
          rowQuestion["QuestionKey"].ToString()
          , count
        );
        count += 10;
      }
    }

    // Buttons
    //Issues
    protected void ButtonAddIssue_Click(object sender, EventArgs e)
    {
      try
      {
        CheckTextboxesForBadContent();

        if (!Table_Issue_Add.Visible)
        {
          // 1st Step in Adding an Issue

          //Only Add Issue Table Visible
          Table_Issue_Add.Visible = true;
          Table_Issue_Edit.Visible = false;

          TextBox_Issue_Description_Add.Text = string.Empty;
          TextBox_Issue_Order_Add.Text = string.Empty;

          Msg.Text = Message("To complete the add, enter the issue description and optional order."
            + " Then click the 'Add an Issue' Button.");
        }
        else
        {
          // 2nd Step - Complete the Issue Addition

          // Checks
          if (TextBox_Issue_Description_Add.Text.Trim() == string.Empty)
            throw new ApplicationException("The Description Textbox is empty.");
          if (
            !string.IsNullOrEmpty(TextBox_Issue_Order_Add.Text.Trim())
            && !Is_Valid_Integer(TextBox_Issue_Order_Add.Text.Trim())
          )
            throw new ApplicationException("The Order Textbox needs to be an integer.");
          if (TextBox_Issue_Description_Add.Text.Trim().Length > 40)
          {
            var tooLongBy = TextBox_Issue_Description_Add.Text.Trim().Length - 40;
            throw new ApplicationException("When adding a new issue,"
              + " the Issue description must be 40 characters or less."
              + " You need to shorten by "
              + tooLongBy + " characters");
          }


          // Make IssueKey and check its unique
          var issueKey = MakeIssueKey(
            string.Empty // CountyCode
            , string.Empty // LocalCode
            , Str_ReCase(TextBox_Issue_Description_Add.Text) // Issue
          );

          //SQL = string.Empty;
          //SQL += " SELECT ";
          //SQL += " IssueKey ";
          //SQL += " FROM Issues ";
          //SQL += " WHERE IssueKey = " + db.SQLLit(ViewState["IssueKey"].ToString());


          //if (db.Row_Optional(SQL) != null)
          if (Is_Valid_Issue(issueKey))
            throw new ApplicationException("Issue (" + TextBox_Issue_Description_Add.Text +
              ") already exists");
          ViewState["IssueKey"] = issueKey;

          // Issue Order
          var issueOrder = TextBox_Issue_Order_Add.Text.Trim() != string.Empty
            ? Convert.ToInt16(TextBox_Issue_Order_Add.Text.Trim())
            : 1;

          // Add the Issue
          var insertSql = "INSERT INTO Issues"
            + "("
            + "IssueKey"
            + ",IssueOrder"
            + ",Issue"
            + ",IssueLevel"
            + ",StateCode"
            + ",CountyCode"
            + ",LocalCode"
            + ",IsIssueOmit"
            + " )"
            + " VALUES"
            + "("
            + SqlLit(ViewState["IssueKey"].ToString())
            + "," + issueOrder
            + "," + SqlLit(TextBox_Issue_Description_Add.Text.Trim())
            + "," + SqlLit(GetIssueLevelInIssueKey(ViewState["IssueKey"].ToString()))
            + "," + SqlLit(ViewState["StateCode"].ToString())
            + "," + SqlLit(ViewState["CountyCode"].ToString())
            + "," + SqlLit(ViewState["LocalCode"].ToString())
            + ",0"
            + ")";
          G.ExecuteSql(insertSql);

          //TextBox_Issue_Description_Add.Text = string.Empty;
          //TextBox_Issue_Order_Add.Text = string.Empty;

          RenumberIssues();

          CreateIssuesReport();

          //  Edit Issue Table and load
          Table_Issue_Add.Visible = false;
          Table_Issue_Edit.Visible = true;
          TextBox_Issue_Description.Text =
            Issue_Desc(ViewState["IssueKey"].ToString());
          TextBox_Issue_Order.Text =
            Issues_IssueOrder(ViewState["IssueKey"].ToString())
              .ToString(CultureInfo.InvariantCulture);

          Msg.Text = Ok("The issue has been added and is in the report below.");
        }
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ButtonDeleteIssue_Click(object sender, EventArgs e)
    {
      try
      {
        // checks
        CheckTextboxesForBadContent();

        if (!Is_Valid_Issue(TextBox_Delete_IssueKey.Text.Trim()))
          throw new ApplicationException("The IssueKey is not valid.");

        if (QuestionsCount(TextBox_Delete_IssueKey.Text.Trim()) > 0)
          throw new ApplicationException("There are "
            + QuestionsCount(TextBox_Delete_IssueKey.Text.Trim())
            + " questions. These need to be reassigned beore this issue can be deleted.");

        if (AnswersCount(TextBox_Delete_QuestionKey.Text.Trim()) > 0)
          throw new ApplicationException("There are "
            + AnswersCount(TextBox_Delete_QuestionKey.Text.Trim())
            +
            " answers. These need to be reassigned or deleted before this question can be deleted.");

        var sql = "DELETE FROM Issues WHERE IssueKey = "
          + SqlLit(TextBox_Delete_IssueKey.Text.Trim());
        G.ExecuteSql(sql);

        TextBox_Delete_IssueKey.Text = string.Empty;

        RenumberIssues();

        CreateIssuesReport();

        // Reset Tables Not Visible
        Table_Question_Edit.Visible = false;
        Table_Question_Add.Visible = false;
        Table_Question.Visible = false;
        Table_Delete_Question.Visible = false;
        Table_Questions_Report.Visible = false;


        Table_Issue_Edit.Visible = false;
        Table_Issue_Add.Visible = false;

        Msg.Text = Ok("The issue has been deleted.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    //Questions

    protected void ButtonAddQuestion_Click(object sender, EventArgs e)
    {
      try
      {
        CheckTextboxesForBadContent();

        if (!Table_Question_Add.Visible)
        {
          // 1st Step in Adding an Question

          //Only Add Question Table Visible
          Table_Question_Add.Visible = true;
          Table_Question_Edit.Visible = false;

          TextBox_Question_Description_Add.Text = string.Empty;
          TextBox_Question_Order_Add.Text = string.Empty;

          Msg.Text = Message("To complete the add, enter the question and optional order."
            + " Then click the 'Add a Question' Button.");
        }
        else
        {
          // 2nd Step - Complete the Question Addition

          // Checks
          if (TextBox_Question_Description_Add.Text.Trim() == string.Empty)
            throw new ApplicationException("The Question Textbox is empty.");
          if (
            !string.IsNullOrEmpty(TextBox_Question_Order_Add.Text.Trim())
            && !Is_Valid_Integer(TextBox_Question_Order_Add.Text.Trim())
          )
            throw new ApplicationException("The Order Textbox needs to be an integer.");
          if (TextBox_Question_Description_Add.Text.Trim().Length > 80)
          {
            var tooLongBy = TextBox_Question_Description_Add.Text.Trim().Length - 80;
            throw new ApplicationException("The question must be 80 characters or less."
              + " You need to shorten by "
              + tooLongBy + " characters");
          }


          // Make QuestionKey and check its unique
          var questionKey =
            ViewState["IssueKey"]
            + MakeUnique6Digits();

          if (Is_Valid_Question(questionKey))
            throw new ApplicationException(
              "Question (" + TextBox_Question_Description_Add.Text + ") already exists");
          ViewState["QuestionKey"] = questionKey;

          // Question Order
          var questionOrder = TextBox_Question_Order_Add.Text.Trim() != string.Empty
            ? Convert.ToInt16(TextBox_Question_Order_Add.Text.Trim())
            : 1;

          // Add the Question
          var insertSql = "INSERT INTO Questions"
            + "("
            + "QuestionKey"
            + ",IssueKey"
            + ",QuestionOrder"
            + ",Question"
            + " )"
            + " VALUES"
            + "("
            + SqlLit(ViewState["QuestionKey"].ToString())
            + "," + SqlLit(ViewState["IssueKey"].ToString())
            + "," + questionOrder
            + "," + SqlLit(TextBox_Question_Description_Add.Text.Trim())
            + ")";
          G.ExecuteSql(insertSql);

          RenumberQuestions();

          CreateQuestionsReport();

          // Edit Question Table Visible and load
          Table_Question_Add.Visible = false;
          Table_Question_Edit.Visible = true;
          TextBox_Question_Description.Text =
            Questions_Question(ViewState["QuestionKey"].ToString());
          TextBox_Question_Order.Text =
            Questions_QuestionOrder(ViewState["QuestionKey"].ToString())
              .ToString(CultureInfo.InvariantCulture);

          Msg.Text = Ok("The question has been added and is in the report below.");
          TextBox_Question_Description.Text = string.Empty;
          TextBox_Question_Order.Text = string.Empty;

          //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
          //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());
        }
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ButtonDeleteQuestion_Click(object sender, EventArgs e)
    {
      try
      {
        // checks
        CheckTextboxesForBadContent();

        if (!Is_Valid_Question(TextBox_Delete_QuestionKey.Text.Trim()))
          throw new ApplicationException("The QuestionKey is not valid.");

        if (AnswersCount(TextBox_Delete_QuestionKey.Text.Trim()) > 0)
          throw new ApplicationException("There are "
            + AnswersCount(TextBox_Delete_QuestionKey.Text.Trim())
            +
            " answers. These need to be reassigned or deleted before this question can be deleted.");

        // Msg needs to come before question is deleted
        Msg.Text = Ok("The question ("
          + Questions_Str(TextBox_Delete_QuestionKey.Text.Trim(), "Question")
          + ") has been deleted.");

        var sql = "DELETE FROM Questions WHERE QuestionKey = "
          + SqlLit(TextBox_Delete_QuestionKey.Text.Trim());
        G.ExecuteSql(sql);

        RenumberQuestions();

        CreateQuestionsReport();

        // Reset Tables Not Visible
        if (QuestionsCount(ViewState["IssueKey"].ToString()) == 0)
        {
          Table_Question_Edit.Visible = false;
          Table_Question_Add.Visible = false;
          Table_Question.Visible = false;
          Table_Delete_Question.Visible = false;
          Table_Questions_Report.Visible = false;
        }

        TextBox_Delete_QuestionKey.Text = string.Empty;

        //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
        //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ButtonDeleteAnswers_Click(object sender, EventArgs e)
    {
      try
      {
        // checks
        CheckTextboxesForBadContent();

        if (!Is_Valid_Question(TextBox_Delete_QuestionKey.Text.Trim()))
          throw new ApplicationException("The QuestionKey is not valid.");

        var sql = "DELETE FROM Answers WHERE QuestionKey = "
          + SqlLit(TextBox_Delete_QuestionKey.Text.Trim());
        G.ExecuteSql(sql);

        RenumberQuestions();

        CreateQuestionsReport();

        // Reset Tables Not Visible
        if (QuestionsCount(ViewState["IssueKey"].ToString()) == 0)
        {
          Table_Question_Edit.Visible = false;
          Table_Question_Add.Visible = false;
          Table_Question.Visible = false;
          Table_Delete_Question.Visible = false;
          Table_Questions_Report.Visible = false;
        }

        Msg.Text = Ok("All the ANSWERES to question ("
          + Questions_Str(TextBox_Delete_QuestionKey.Text.Trim(), "Question")
          + ") have been deleted.");

        //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
        //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ButtonDeleteAllQuestions_Click(object sender, EventArgs e)
    {
      try
      {
        CheckTextboxesForBadContent();

        var sql = "SELECT * FROM Questions WHERE IssueKey="
          + SqlLit(ViewState["IssueKey"].ToString())
          + " ORDER BY QuestionOrder";
        var tableQuestions = G.Table(sql);
        foreach (DataRow rowQuestion in tableQuestions.Rows)
        {
          var answersInt = AnswersCount(rowQuestion["QuestionKey"].ToString());
          if (answersInt == 0)
          {
            sql = "DELETE FROM Questions WHERE QuestionKey = "
              + SqlLit(rowQuestion["QuestionKey"].ToString());
            G.ExecuteSql(sql);
          }
        }

        RenumberQuestions();

        CreateQuestionsReport();

        // Reset Tables Not Visible
        if (QuestionsCount(ViewState["IssueKey"].ToString()) == 0)
        {
          Table_Question_Edit.Visible = false;
          Table_Question_Add.Visible = false;
          Table_Question.Visible = false;
          Table_Delete_Question.Visible = false;
          Table_Questions_Report.Visible = false;
        }

        Msg.Text = Ok("The questions with no answers have been deleted.");

        //db.Cache_Remove_Issue_Pages(db.Questions_IssueKey(TextBox_Delete_QuestionKey.Text.Trim()));
        //db.Cache_Remove_PoliticianIssue_Issue(db.Questions_IssueKey(TextBox_Delete_QuestionKey.Text.Trim()));
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    // Textboxes
    //Issues
    protected void TextBoxIssueDescription_TextChanged(object sender, EventArgs e)
    {
      try
      {
        // checks
        CheckTextboxesForBadContent();

        if (TextBox_Issue_Description.Text.Trim() == string.Empty)
          throw new ApplicationException("The Issue Description Textbox is empty.");

        if (TextBox_Issue_Description.Text.Trim().Length > 40)
        {
          var tooLongBy = TextBox_Issue_Description.Text.Trim().Length - 40;
          throw new ApplicationException("The Issue description must be 40 characters or less."
            + " You need to shorten by "
            + tooLongBy + " characters");
        }

        Issues_Update_Issue(
          ViewState["IssueKey"].ToString()
          , TextBox_Issue_Description.Text.Trim()
        );

        Table_Issue_Edit.Visible = false;

        CreateIssuesReport();

        Msg.Text = Ok("The issue description has been updated");

        //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
        //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void TextBoxIssueOrder_TextChanged(object sender, EventArgs e)
    {
      try
      {
        // Checks
        CheckTextboxesForBadContent();

        if (TextBox_Issue_Order.Text.Trim() == string.Empty)
          throw new ApplicationException("The Order Textbox can not be empty.");

        if (!Is_Valid_Integer(TextBox_Issue_Order.Text.Trim()))
          throw new ApplicationException("The Order Textbox needs to be an integer.");

        Issues_Update_IssueOrder(ViewState["IssueKey"].ToString(),
          string.IsNullOrEmpty(TextBox_Issue_Order.Text.Trim())
            ? 1
            : Convert.ToInt16(TextBox_Issue_Order.Text.Trim()));

        Table_Issue_Edit.Visible = false;

        RenumberIssues();

        CreateIssuesReport();

        Msg.Text = Ok("The issue order has been updated");

        //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
        //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    //Questions
    protected void TextBoxQuestionDescription_TextChanged(object sender, EventArgs e)
    {
      try
      {
        // checks
        CheckTextboxesForBadContent();

        if (TextBox_Question_Description.Text.Trim() == string.Empty)
          throw new ApplicationException("The Question Textbox is empty.");

        Questions_Update_Question(
          ViewState["QuestionKey"].ToString()
          , TextBox_Question_Description.Text.Trim()
        );

        Table_Question_Edit.Visible = false;

        CreateQuestionsReport();

        Msg.Text = Ok("The question has been updated");

        //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
        //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void TextBoxQuestionOrder_TextChanged(object sender, EventArgs e)
    {
      try
      {
        // Checks
        CheckTextboxesForBadContent();

        if (TextBox_Question_Order.Text.Trim() == string.Empty)
          throw new ApplicationException("The Order Textbox can not be empty.");

        if (!Is_Valid_Integer(TextBox_Question_Order.Text.Trim()))
          throw new ApplicationException("The Order Textbox needs to be an integer.");

        Questions_Update_QuestionOrder(
          ViewState["QuestionKey"].ToString()
          , Convert.ToInt16(TextBox_Question_Order.Text.Trim())
        );

        Table_Question_Edit.Visible = false;

        RenumberQuestions();

        CreateQuestionsReport();

        Msg.Text = Ok("The question order has been updated");

        //db.Cache_Remove_Issue_Pages(ViewState["IssueKey"].ToString());
        //db.Cache_Remove_PoliticianIssue_Issue(ViewState["IssueKey"].ToString());
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Issues";
        // Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

        if (string.IsNullOrWhiteSpace(StateCode))
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateStateLinks("/admin/Issues.aspx?state={StateCode}");
          NoJurisdiction.ShowHeadOptional(false);
          NoJurisdiction.Visible = true;
          return;
        }

        ViewState["StateCode"] = G.State_Code();
        ViewState["CountyCode"] = G.County_Code();
        ViewState["LocalCode"] = G.Local_Code();
        //if (!db.Is_User_Security_Ok())
        //  HandleSecurityException();

        // ViewState["IssueKey"] ViewState["QuestionKey"]

        ViewState["IssueKey"] = string.Empty;
        if (!string.IsNullOrEmpty(QueryIssue))
          ViewState["IssueKey"] = QueryIssue;

        ViewState["QuestionKey"] = string.Empty;
        if (!string.IsNullOrEmpty(QueryQuestion))
          ViewState["QuestionKey"] = QueryQuestion;

        try
        {
          // Page Title
          PageTitle.Text = "Issues and Questions";

          if (ViewState["StateCode"].ToString() == "LL")
            PageTitle.Text += "<br>" + "All Candidates";
          else if (ViewState["StateCode"].ToString() == "US")
            PageTitle.Text += "<br>" + "National Candidates";
          else
            PageTitle.Text += "<br>" + StateCache.GetStateName(ViewState["StateCode"].ToString())
              + " Candidates";

          if (!string.IsNullOrEmpty(ViewState["IssueKey"].ToString()))
          {
            PageTitle.Text += "<br>"
              + "<span style=color:red>"
              + Issue_Desc(ViewState["IssueKey"].ToString())
              + "</span>";
          }

          //On initial entry there is only a StateCode Anchor
          //Anchors on Issues and Questions Reports have 4 possible query string prameters:
          //Issue Link => db.QueryString("Issue")
          //Issue Ok/Omit Link => db.QueryString("Issue") & db.QueryString("Omit")
          //Question Link => db.QueryString("Issue") & db.QueryString("Question")
          //Question Ok/Omit Link => db.QueryString("Issue") & db.QueryString("Question") & db.QueryString("Omit")

          if (string.IsNullOrEmpty(QueryIssue))
          {
            MakeAllControlsNotVisible();
          }
          else
          {
            // Entry from Link on Issues or Question Reports
            if (
              string.IsNullOrEmpty(QueryQuestion)
              && string.IsNullOrEmpty(GetQueryString("Omit"))
            )
            {
              // Issue Link to edit issue and/or issue questions
              MakeAllControlsNotVisible();
              Table_Issue_Edit.Visible = true;
              Table_Question.Visible = true;
              Table_Questions_Report.Visible = true;

              TextBox_Issue_Description.Text =
                Issue_Desc(QueryIssue);
              TextBox_Issue_Order.Text =
                Issues_IssueOrder(QueryIssue).ToString(CultureInfo.InvariantCulture);
              ViewState["IssueKey"] = QueryIssue;

              CreateQuestionsReport();
            }
            else if (
              string.IsNullOrEmpty(QueryQuestion)
              && !string.IsNullOrEmpty(GetQueryString("Omit"))
            )
            {
              // Issue Ok/Omit Link to change status to ok or omit

              MakeAllControlsNotVisible();

              Issues_Update_IsIssueOmit(QueryIssue, GetQueryString("Omit") == "true");
            }
            else if (!string.IsNullOrEmpty(QueryQuestion) && string.IsNullOrEmpty(GetQueryString("Omit")))
            {
              // Question Link to edit question

              MakeAllControlsNotVisible();
              Table_Question_Edit.Visible = true;
              Table_Question.Visible = true;
              Table_Questions_Report.Visible = true;

              TextBox_Question_Description.Text =
                Questions_Question(ViewState["QuestionKey"].ToString());
              TextBox_Question_Order.Text =
                Questions_QuestionOrder(ViewState["QuestionKey"].ToString())
                  .ToString(CultureInfo.InvariantCulture);

              RenumberQuestions();

              CreateQuestionsReport();
            }
            else if (
              !string.IsNullOrEmpty(QueryQuestion)
              && !string.IsNullOrEmpty(GetQueryString("Omit"))
            )
            {
              // Question Ok/Omit Link to change status to ok or omit

              MakeAllControlsNotVisible();
              Table_Question.Visible = true;
              Table_Questions_Report.Visible = true;

              Questions_Update_IsQuestionOmit(QueryQuestion, GetQueryString("Omit") == "true");

              RenumberQuestions();

              CreateQuestionsReport();
            }
          }

          RenumberIssues();

          CreateIssuesReport();

          // Only Master controls
          if (IsSuperUser)
          {
            Table_Delete_Issue.Visible = true;

            if (
              !string.IsNullOrEmpty(QueryIssue)
              && string.IsNullOrEmpty(GetQueryString("Omit"))
            )
              Table_Delete_Question.Visible = true;
          }
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