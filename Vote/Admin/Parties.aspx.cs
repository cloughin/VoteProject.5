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
  public partial class PartiesPage : SecureAdminPage, IAllowEmptyStateCode
  {
    public override IEnumerable<string> NonStateCodesAllowed => new[] {"US"};

    #region from db

    public static string PartyKeyCheck(string partyKey)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Parties.PartyKey ";
      sql += " FROM Parties ";
      sql += " WHERE Parties.PartyKey = " + SqlLit(partyKey);
      return sql;
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

    public static string Str_Remove_Single_And_Double_Quotes(string str2Modify)
    {
      var str = str2Modify;
      str = str.Replace("\'", string.Empty);
      str = str.Replace("\"", string.Empty);

      return str;
    }

    public static string Str_Remove_MailTo(string str2Fix)
    {
      //?? dangerous: sendmailtome@gmail.com -> sendme@gmail.com
      //int loc = Str2Fix.ToLower().IndexOf("mailto", 0, Str2Fix.Length);
      //if (loc != -1)
      //  Str2Fix = Str2Fix.Remove(loc, 6).Trim();
      var loc = str2Fix.ToLower().IndexOf(@"mailto:", 0, str2Fix.Length, StringComparison.Ordinal);
      if (loc != -1)
        str2Fix = str2Fix.Remove(loc, 7).Trim();

      //?? dangerous: myemail@gmail.com -> my@gmail.com
      //loc = Str2Fix.ToLower().IndexOf("email", 0, Str2Fix.Length);
      //if (loc != -1)
      //{
      //  Str2Fix = Str2Fix.Remove(loc, 5).Trim();
      //}

      //Str2Fix = Str2Fix.Replace(":", string.Empty);
      return str2Fix;
    }

    public static string Str_Remove_Http(string str2Fix)
    {
      if (!string.IsNullOrEmpty(str2Fix))
      {
        var loc = str2Fix.ToLower().IndexOf("http://", 0, str2Fix.Length, StringComparison.Ordinal);
        if (loc != -1)
        {
          str2Fix = str2Fix.Remove(loc, 7).Trim();
        }

        loc = str2Fix.ToLower().IndexOf("http//", 0, str2Fix.Length, StringComparison.Ordinal);
        if (loc != -1)
        {
          str2Fix = str2Fix.Remove(loc, 6).Trim();
        }

        loc = str2Fix.ToLower().IndexOf("https://", 0, str2Fix.Length, StringComparison.Ordinal);
        if (loc != -1)
        {
          str2Fix = str2Fix.Remove(loc, 8).Trim();
        }

        //?? dangerous: burlington.com -> bington.com
        //loc = Str2Fix.ToLower().IndexOf("url", 0, Str2Fix.Length);
        //if (loc != -1)
        //{
        //  Str2Fix = Str2Fix.Remove(loc, 3).Trim();
        //}

        //?? NB: for bare domain names the trailing / is typically retained
        if (str2Fix.EndsWith(@"/"))
        {
          str2Fix = str2Fix.Substring(0, str2Fix.Length - 1);
        }
      }

      //Str2Fix = Str2Fix.Replace(":", string.Empty);

      return str2Fix;
    }

    public static string DbErrorMsg(string sql, string err) =>
      $"Database Failure for SQL Statement::{sql} :: Error Msg:: {err}";

    public static Random RandomObject;

    public static char GetRandomAlpha24()
    {
      if (RandomObject == null)
        RandomObject = new Random();
      var n = RandomObject.Next(24);
      if (n < 8)
        return Convert.ToChar(n + Convert.ToInt32('A'));
      return n < 13
        ? Convert.ToChar(n + Convert.ToInt32('B'))
        : Convert.ToChar(n + Convert.ToInt32('C'));
    }

    public static char GetRandomDigit()
    {
      if (RandomObject == null)
        RandomObject = new Random();
      return Convert.ToChar(RandomObject.Next(10) + Convert.ToUInt32('0'));
    }

    public static string MakeUniquePassword()
    {
      var password = string.Empty;
      //Make a AAADDD password
      for (var n = 0; n < 3; n++)
        password += GetRandomAlpha24();
      for (var n = 0; n < 3; n++)
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

    public static string Url_Admin_Parties(
      string stateCode
      , string partyKey
      , string partyEmail
    )
    {
      var url = "/Admin/Parties.aspx";
      if (!string.IsNullOrEmpty(stateCode))
        url += "&State=" + stateCode;
      if (!string.IsNullOrEmpty(partyKey))
        url += "&Party=" + partyKey;
      if (!string.IsNullOrEmpty(partyEmail))
        url += "&Email=" + partyEmail;
      return Fix_Url_Parms(url);
    }

    public static string PartyKey_Add_To_QueryString_Master_User()
    {
      if (IsMasterUser && !string.IsNullOrEmpty(QueryParty))
        return "&Party=" + QueryParty;
      return string.Empty;
    }

    public static string Url_Party_Default(string stateCode, string electionKey)
    {
      var url = @"/Party/Default.aspx";
      if (!string.IsNullOrEmpty(stateCode))
        url += "&State=" + stateCode;
      if (!string.IsNullOrEmpty(electionKey))
        url += "&Election=" + electionKey;
      url += PartyKey_Add_To_QueryString_Master_User();
      return Fix_Url_Parms(url);
    }

    public static DataRow Row(string sql)
    {
      var table = G.Table(sql);
      if (table.Rows.Count == 1)
        return table.Rows[0];
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a unique row for this Id."));
    }

    public static DataRow Row_Optional(string sql)
    {
      var table = G.Table(sql);
      return table.Rows.Count == 1
        ? table.Rows[0]
        : null;
    }

    public static string Single_Key_Str_Optional(string tableName, string column, string keyName,
      string keyValue)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
        + " WHERE " + keyName.Trim() + " = " + SqlLit(keyValue.Trim());
      var table = G.Table(sql);
      switch (table.Rows.Count)
      {
        case 1: //KeyValue
          return table.Rows[0][column].ToString().Trim();
        case 0: //no row
          return string.Empty;
        default:
          return string.Empty;
      }
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

    public static string Parties_Str(string partyKey, string column) => 
      partyKey != string.Empty
      ? Single_Key_Str_Optional("Parties", column, "PartyKey", partyKey)
      : string.Empty;

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

    //public static void Throw_Exception_TextBox_Html_Or_Script(TextBox textBox)
    //{
    //  Throw_Exception_TextBox_Html(textBox);
    //  Throw_Exception_TextBox_Script(textBox);
    //}

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

    private static void Add_Td_To_Tr(HtmlTableRow htmlTr, string text, string tdClass, int colspan) => 
      Add_Td_To_Tr(htmlTr, text, tdClass, string.Empty, colspan);

    private static void Add_Td_To_Tr(HtmlTableRow htmlTr, string text, string tdClass) => 
      Add_Td_To_Tr(htmlTr, text, tdClass, string.Empty);

    private static void PartiesEmails_Update_Str(string partyEmail, string column, string valueStr)
    {
      var updateSql =
        $"UPDATE PartiesEmails SET {column} = {SqlLit(valueStr.Trim())} WHERE PartyEmail = {SqlLit(partyEmail)}";
      G.ExecuteSql(updateSql);
    }

    private static void Parties_Update_Str(string partyKey, string column, string valueStr)
    {
      var updateSql =
        $"UPDATE Parties SET {column} = {SqlLit(valueStr.Trim())} WHERE PartyKey = {SqlLit(partyKey)}";
      G.ExecuteSql(updateSql);
    }

    private static bool Parties_Bool(string partyKey, string column) => 
      (partyKey != string.Empty) && Single_Key_Bool("Parties", column, "PartyKey", partyKey);

    private static bool Is_Valid_Email_Address(string emailAddress) => 
      emailAddress.Trim().IndexOf("@", StringComparison.Ordinal) != -1;

    private static string MakePartyKey(string stateCode, string partyCode) => 
      stateCode.Trim().ToUpper() + partyCode.Trim().ToUpper();

    private static string Url_Party_Default() => 
      Url_Party_Default(string.Empty, string.Empty);

    private static string Generic_WebAddress_Anchor(string webAddress, string anchorText, string toolTip, string target)
    {
      var anchor = "<a href=";
      anchor += "\"" + NormalizeUrl(webAddress);
      anchor += "\"";

      if (toolTip != string.Empty)
      {
        anchor += " title=";
        anchor += "\"";
        anchor += Str_Remove_Single_And_Double_Quotes(toolTip);
        anchor += "\"";
        anchor += " ";
      }

      if (target.Trim() != string.Empty)
      {
        anchor += " Target=";
        anchor += "\"";
        anchor += target;
        anchor += "\"";
      }

      anchor += ">";

      if (anchorText != string.Empty)
        anchor += anchorText;
      else
        anchor += webAddress;

      anchor += "</a>";

      return anchor;
    }

    private static string Anchor_Admin_Parties(
      string stateCode
      , string partyKey
      , string partyEmail
      , string anchorText
    )
    {
      var anchor = string.Empty;
      anchor += "<a href=";
      anchor += "\"";
      anchor += Url_Admin_Parties(
        stateCode
        , partyKey
        , partyEmail
      );
      anchor += "\"";

      anchor += " target=";
      anchor += "\"";
      anchor += "edit";
      anchor += "\"";

      anchor += ">";
      anchor += anchorText;

      anchor += "</a>";
      return anchor;
    }

    #endregion from db

    private static void Heading_Parties_Report(ref HtmlTable htmlTableParites, string reportColumnHeading)
    {
      //<tr Class="trReportDetailHeading">
      var htmlTrHeading = Add_Tr_To_Table_Return_Tr(
        htmlTableParites
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
        , "Party<br>Key"
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "Ballot<br>Code"
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , reportColumnHeading
        , "tdReportDetailHeading"
      );
      //<td Class="tdReportDetailHeading" align="center">
      Add_Td_To_Tr(
        htmlTrHeading
        , "Web Address"
        , "tdReportDetailHeading"
      );
    }

    private void Row_Parties_Report(ref HtmlTable htmlTableParites, DataRow partyRow)
    {
      var partyUrl = partyRow["PartyURL"].ToString().Trim();
      if (partyUrl == string.Empty)
        partyUrl = @"&nbsp;";
      //<tr Class="trReportDetail">
      var htmlTrParty = Add_Tr_To_Table_Return_Tr(
        htmlTableParites
        , "trReportDetail"
      );
      //Order
      Add_Td_To_Tr(
        htmlTrParty
        , partyRow["PartyOrder"].ToString()
        , "tdReportDetail"
      );
      //Party Key
      Add_Td_To_Tr(
        htmlTrParty
        , partyRow["PartyKey"].ToString()
        , "tdReportDetail"
      );
      //Ballot Code
      Add_Td_To_Tr(
        htmlTrParty
        , partyRow["PartyCode"].ToString()
        , "tdReportDetail"
      );
      // Party Name
      Add_Td_To_Tr(
        htmlTrParty
        //, PartyRow["PartyName"].ToString()
        , Anchor_Admin_Parties(
          //Session["UserPartyKey"].ToString()
          ViewState["UserStateCode"].ToString()
          , partyRow["PartyKey"].ToString()
          , string.Empty
          , partyRow["PartyName"].ToString()
        )
        , "tdReportDetail"
      );
      // Web Address
      Add_Td_To_Tr(htmlTrParty
        , Generic_WebAddress_Anchor(
          partyUrl
          , partyUrl
          , "Party Website"
          , "view"
        ) + "&nbsp;"
        , "tdReportDetail"
      );
    }

    private void Parties_Report()
    {
      var htmlTableParites = new HtmlTable();
      htmlTableParites.Attributes["cellspacing"] = "0";
      //HTML_Table_Parites.Attributes["align"] = "center";
      htmlTableParites.Attributes["border"] = "0";

      #region Major Parties

      Heading_Parties_Report(
        ref htmlTableParites
        , "Major Parties"
      );
      var sql = string.Empty;
      sql += "SELECT * FROM Parties ";
      sql += " WHERE StateCode = " + SqlLit(ViewState["UserStateCode"].ToString());
      sql += " AND IsPartyMajor = '1'";
      sql += " ORDER BY PartyOrder,PartyName";
      var paritesTable = G.Table(sql);
      foreach (DataRow partyRow in paritesTable.Rows)
        Row_Parties_Report(
          ref htmlTableParites
          , partyRow
        );

      #endregion Major Parties

      #region Minor Parties

      Heading_Parties_Report(
        ref htmlTableParites
        , "Minor Parties"
      );
      sql = string.Empty;
      sql += "SELECT * FROM Parties ";
      sql += " WHERE StateCode = " + SqlLit(ViewState["UserStateCode"].ToString());
      sql += " AND IsPartyMajor = '0'";
      sql += " ORDER BY PartyOrder,PartyName";
      paritesTable = G.Table(sql);
      foreach (DataRow partyRow in paritesTable.Rows)
        Row_Parties_Report(
          ref htmlTableParites
          , partyRow
        );

      #endregion Minor Parties

      //return db.RenderToString(HTML_Table_Parites);
      Label_Political_Parties_Report.Text = htmlTableParites.RenderToString();
    }

    //---------------------------
    private static void Heading_Emails_Report(HtmlTable htmlTableParites)
    {
      var htmlTrHeading = Add_Tr_To_Table_Return_Tr(
        htmlTableParites
        , "trReportDetailHeading"
      );

      Add_Td_To_Tr(
        htmlTrHeading
        , "Email Address"
        , "tdReportDetailHeading"
      );

      Add_Td_To_Tr(
        htmlTrHeading
        , "Phone"
        , "tdReportDetailHeading"
      );

      Add_Td_To_Tr(
        htmlTrHeading
        , "First Name"
        , "tdReportDetailHeading"
      );

      Add_Td_To_Tr(
        htmlTrHeading
        , "Last Name"
        , "tdReportDetailHeading"
      );

      Add_Td_To_Tr(
        htmlTrHeading
        , "Title"
        , "tdReportDetailHeading"
      );

      Add_Td_To_Tr(
        htmlTrHeading
        , "Password"
        , "tdReportDetailHeading"
      );
    }

    private void Row_Email_Report(ref HtmlTable htmlTableEmails, DataRow rowEmail)
    {
      var htmlTrEmail = Add_Tr_To_Table_Return_Tr(
        htmlTableEmails
        , "trReportDetailHeading"
      );

      Add_Td_To_Tr(
        htmlTrEmail
        , Anchor_Admin_Parties(
          //Session["UserPartyKey"].ToString()
          ViewState["UserStateCode"].ToString()
          , rowEmail["PartyKey"].ToString()
          , rowEmail["PartyEmail"].ToString()
          , rowEmail["PartyEmail"].ToString()
        )
        , "tdReportDetail"
      );

      Add_Td_To_Tr(
        htmlTrEmail
        , rowEmail["PartyContactPhone"] + "&nbsp;"
        , "tdReportDetail"
      );

      Add_Td_To_Tr(
        htmlTrEmail
        , rowEmail["PartyContactFName"] + "&nbsp;"
        , "tdReportDetail"
      );

      Add_Td_To_Tr(
        htmlTrEmail
        , rowEmail["PartyContactLName"] + "&nbsp;"
        , "tdReportDetail"
      );

      Add_Td_To_Tr(
        htmlTrEmail
        , rowEmail["PartyContactTitle"] + "&nbsp;"
        , "tdReportDetail"
      );

      Add_Td_To_Tr(
        htmlTrEmail
        , rowEmail["PartyPassword"] + "&nbsp;"
        , "tdReportDetail"
      );
    }

    private void Emails_Report()
    {
      var htmlTableEmails = new HtmlTable();
      htmlTableEmails.Attributes["cellspacing"] = "0";
      //HTML_Table_Emails.Attributes["align"] = "center";
      htmlTableEmails.Attributes["border"] = "0";

      var partyKey = ViewState["PartyKey"].ToString();
      var partyName = Parties.GetPartyName(partyKey);

      Heading_Emails_Report(htmlTableEmails);

      #region Emails

      var sql = string.Empty;
      sql += "SELECT * FROM PartiesEmails";
      sql += " WHERE IsVolunteer=0 AND PartyKey = " + SqlLit(partyKey);
      var tableEmails = G.Table(sql);
      if (tableEmails.Rows.Count > 0)
      {
        foreach (DataRow rowEmail in tableEmails.Rows)
          Row_Email_Report(
            ref htmlTableEmails
            , rowEmail
          );
      }
      else
      {
        #region No Email Addresses

        var htmlTrEmail = Add_Tr_To_Table_Return_Tr(
          htmlTableEmails
          , "trReportDetailHeading"
        );

        Add_Td_To_Tr(
          htmlTrEmail
          , "There are No Email Addresses for this Party."
          , "tdReportDetail"
          , 5
        );

        #endregion No Email Addresses
      }

      #endregion Emails

      Label_Contacts_Emails_Report.Text = htmlTableEmails.RenderToString();
      PartyEmailsAndContactsHeading.Text = $"{partyName} Emails and Contacts";
    }

    private void CheckTextBoxes4IllegalText()
    {
      Throw_Exception_TextBox_Script(TextboxBallotCode_Add);
      Throw_Exception_TextBox_Html(TextboxBallotCode_Add);

      Throw_Exception_TextBox_Script(TextBoxPartyName_Add);
      Throw_Exception_TextBox_Html(TextBoxPartyName_Add);

      Throw_Exception_TextBox_Script(TextboxBallotCode);
      Throw_Exception_TextBox_Html(TextboxBallotCode);

      Throw_Exception_TextBox_Script(TextBoxPartyName);
      Throw_Exception_TextBox_Html(TextBoxPartyName);

      Throw_Exception_TextBox_Script(TextBoxAddressLine1);
      Throw_Exception_TextBox_Html(TextBoxAddressLine1);

      Throw_Exception_TextBox_Script(TextBoxAddressLine2);
      Throw_Exception_TextBox_Html(TextBoxAddressLine2);

      Throw_Exception_TextBox_Script(TextBoxCityStateZip);
      Throw_Exception_TextBox_Html(TextBoxCityStateZip);

      Throw_Exception_TextBox_Script(TextboxWebAddress);
      Throw_Exception_TextBox_Html(TextboxWebAddress);

      Throw_Exception_TextBox_Script(TextboxEmailAddress);
      Throw_Exception_TextBox_Html(TextboxEmailAddress);

      Throw_Exception_TextBox_Script(TextboxPhone);
      Throw_Exception_TextBox_Html(TextboxPhone);

      Throw_Exception_TextBox_Script(TextBox_First_Name);
      Throw_Exception_TextBox_Html(TextBox_First_Name);

      Throw_Exception_TextBox_Script(TextBox_Last_Name);
      Throw_Exception_TextBox_Html(TextBox_Last_Name);

      Throw_Exception_TextBox_Script(TextBox_Title);
      Throw_Exception_TextBox_Html(TextBox_Title);
    }

    private void CheckBallotCode()
    {
      if (TextboxBallotCode_Add.Text == string.Empty)
        throw new ApplicationException("A Ballot Code is required.");
      if (TextboxBallotCode_Add.Text.Trim().Length > 3)
        throw new ApplicationException("A Ballot Code can only be 1 to 3 characters.");
    }

    private void CheckPartyKeyNotExist()
    {
      if (Row_Optional(PartyKeyCheck(ViewState["PartyKey"].ToString())) != null)
        throw new ApplicationException(Parties_Str(ViewState["PartyKey"].ToString(), "PartyName")
          + " has the Party Code " + ViewState["PartyKey"] + ". You need to change the Ballot Code.");
    }

    private void Check_Email_Address_Empty()
    {
      if (string.IsNullOrEmpty(TextboxEmailAddress.Text.Trim()))
      {
        throw new ApplicationException("The Email Address is empty.");
      }
    }

    private void Load_Party_Data()
    {
      var sql = string.Empty;
      sql += " SELECT * ";
      sql += " FROM Parties ";
      sql += " WHERE Parties.PartyKey = "
        + SqlLit(ViewState["PartyKey"].ToString());

      var rowParty = Row(sql);
      PartyKey.Text = rowParty["PartyKey"].ToString();
      TextBoxPartyName.Text = rowParty["PartyName"].ToString();
      TextboxBallotCode.Text = rowParty["PartyCode"].ToString();
      TextboxWebAddress.Text = rowParty["PartyURL"].ToString();
      TextBoxAddressLine1.Text = rowParty["PartyAddressLine1"].ToString();
      TextBoxAddressLine2.Text = rowParty["PartyAddressLine2"].ToString();
      TextBoxCityStateZip.Text = rowParty["PartyCityStateZip"].ToString();

      HyperLink_Party_Politician_Links.NavigateUrl =
        Url_Party_Default();
      //Row_Party["PartyKey"].ToString());

      //Emails_Report();
    }

    private void Load_Email_Data(string partyEmail)
    {
      var sql = string.Empty;
      sql += " SELECT * ";
      sql += " FROM PartiesEmails ";
      sql += " WHERE PartyEmail = "
        + SqlLit(partyEmail);

      var rowEmail = Row(sql);
      TextboxEmailAddress.Text = rowEmail["PartyEmail"].ToString().Trim();
      TextboxPhone.Text = rowEmail["PartyContactPhone"].ToString().Trim();
      TextBox_First_Name.Text = rowEmail["PartyContactFName"].ToString().Trim();
      TextBox_Last_Name.Text = rowEmail["PartyContactLName"].ToString().Trim();
      TextBox_Title.Text = rowEmail["PartyContactTitle"].ToString().Trim();

      Emails_Report();
    }

    private void Clear_Political_Party_TextBoxes()
    {
      PartyKey.Text = string.Empty;
      TextBoxPartyName.Text = string.Empty;
      TextboxBallotCode.Text = string.Empty;
      TextboxWebAddress.Text = string.Empty;
      TextBoxAddressLine1.Text = string.Empty;
      TextBoxAddressLine2.Text = string.Empty;
      TextBoxCityStateZip.Text = string.Empty;
    }

    private void Clear_Add_Party_TextBoxes()
    {
      TextBox_Order_Add.Text = string.Empty;
      TextboxBallotCode_Add.Text = string.Empty;
      TextBoxPartyName_Add.Text = string.Empty;
    }

    private void Clear_Email_TextBoxes()
    {
      TextboxEmailAddress.Text = string.Empty;
      TextboxPhone.Text = string.Empty;
      TextBox_First_Name.Text = string.Empty;
      TextBox_Last_Name.Text = string.Empty;
      TextBox_Title.Text = string.Empty;
    }

    private void ThePageTitle()
    {
      if (!string.IsNullOrEmpty(ViewState["UserStateCode"].ToString()))
        PageTitle.Text = StateCache.GetStateName(ViewState["UserStateCode"].ToString());

      PageTitle.Text += "<br>Political Parties Emails, Websites and Information";
    }

    private void Controls_All_Not_Visible()
    {
      //TableUpdate.Visible = false;
      TableAdd.Visible = false;
      TableParty.Visible = false;
      TableDelete.Visible = false;
    }

    private void Controls_Select_Or_Add_Party()
    {
      Controls_All_Not_Visible();

      TableAdd.Visible = true;
      TableParty.Visible = true;

      Msg_Party.Text =
        Message("To edit a party's information including emails, click on a Party Name."
          + " To view the party's website, click the Web Address.");
    }

    private void Controls_Edit_Party()
    {
      Controls_All_Not_Visible();

      TableParty.Visible = true;
      Load_Party_Data();

      Clear_Email_TextBoxes();

      Emails_Report();

      TableDelete.Visible = Session["UserSecurity"] as string == "MASTER";

      Msg_Email.Text =
        Message("To edit an email address, click on the address.");
    }

    private static void Controls_Add_EmailAddress_Mode()
    {
    }

    private void Controls_Edit_Email_Address()
    {
      Load_Party_Data();

      Load_Email_Data(ViewState["EmailAddress"].ToString());
    }

    private void Controls_Entry()
    {
      if (ViewState["EmailAddress"].ToString() != string.Empty)
        Controls_Edit_Email_Address();
      else if (ViewState["PartyKey"].ToString() != string.Empty)
        Controls_Edit_Party();
      else if (
        (ViewState["PartyKey"].ToString() == string.Empty)
        && (ViewState["EmailAddress"].ToString() == string.Empty)
      )
        Controls_Select_Or_Add_Party();
      else if (
        (ViewState["PartyKey"].ToString() != string.Empty)
        && (ViewState["EmailAddress"].ToString() == string.Empty)
      )
        Controls_Add_EmailAddress_Mode();
    }

    protected void ButtonAdd_Click1(object sender, EventArgs e)
    {
      try
      {
        #region checks

        CheckTextBoxes4IllegalText();
        CheckBallotCode();
        if (TextBoxPartyName_Add.Text.Trim() == string.Empty)
          throw new ApplicationException("A Party Name is required.");
        if (TextBox_Order_Add.Text.Trim() == string.Empty)
          throw new ApplicationException("A Party Order on dropdown list is required.");
        if (!Is_Valid_Integer(TextBox_Order_Add.Text.Trim()))
          throw new ApplicationException("A Party Order must be an integer.");

        #endregion checks

        #region PartyKey

        ViewState["PartyKey"] = MakePartyKey(
          ViewState["UserStateCode"].ToString()
          , TextboxBallotCode_Add.Text
        );

        #endregion

        CheckPartyKeyNotExist();

        #region Add the Party

        var insertSql =
          "INSERT INTO Parties(PartyKey,PartyCode,StateCode,PartyOrder,PartyName)" +
          $" VALUES({SqlLit(ViewState["PartyKey"].ToString())}," +
          $"{SqlLit(TextboxBallotCode_Add.Text.Trim().ToUpper())}," +
          $"{SqlLit(ViewState["UserStateCode"].ToString())}," +
          $"{Convert.ToUInt16(TextBox_Order_Add.Text.Trim())}," +
          $"{SqlLit(Str_ReCase(TextBoxPartyName_Add.Text.Trim()))})";
        G.ExecuteSql(insertSql);

        #endregion

        Clear_Add_Party_TextBoxes();

        Load_Party_Data();

        Parties_Report();

        Emails_Report();

        #region Msg

        Msg_Add.Text = Ok(TextBoxPartyName.Text.Trim()
          + ": was ADDED.  "
          + " The data recorded for the political party  is shown in red."
          + " You can now add another party, make additional changes "
          + " or make changes to one of the parties in the Parties Table below.");

        #endregion
      }
      catch (Exception ex)
      {
        #region

        Msg_Add.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void Button_Add_Email_Click1(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxes4IllegalText();

        if (string.IsNullOrEmpty(ViewState["PartyKey"].ToString()))
        {
          Msg_Email.Text =
            Fail("You need to select a party before you can add an email address.");
        }
        else
        {
          if (Is_Valid_Email_Address(TextboxEmailAddress_Add.Text))
          {
            var sqlCheck = "PartiesEmails WHERE PartyEmail ="
              + SqlLit(TextboxEmailAddress_Add.Text.Trim());
            if (G.Rows_Count_From(sqlCheck) > 0)
            {
              Msg_Email.Text =
                Fail("The email address already exists.");
            }
            else
            {
              #region Add Email

              var insertSql =
                "INSERT INTO PartiesEmails(PartyEmail,PartyPassword,PartyKey,PartyContactFName,PartyContactLName,PartyContactPhone,PartyContactTitle)" +
                $" VALUES({SqlLit(Str_Remove_MailTo(TextboxEmailAddress_Add.Text.Trim().ToLower()))}," +
                $"{SqlLit(MakeUniquePassword())}," +
                $"{SqlLit(ViewState["PartyKey"].ToString())}," +
                "\'\',\'\',\'\',\'\')";
              G.ExecuteSql(insertSql);

              LogAdminData.Insert(
                DateTime.Now
                , UserSecurityClass
                , UserName
                , "PartyEmail"
                , string.Empty
                , TextboxEmailAddress_Add.Text.Trim().ToLower()
              );

              #endregion Add Email

              ViewState["EmailAddress"] = TextboxEmailAddress_Add.Text.Trim().ToLower();
              TextboxEmailAddress_Add.Text = string.Empty;
              Load_Email_Data(ViewState["EmailAddress"].ToString());

              Parties_Report();

              Msg_Email.Text =
                Ok("The email address has been added.");
            }
          }
          else
            Msg_Email.Text =
              Fail("The email address did not contain @.");
        }
      }
      catch (Exception ex)
      {
        #region

        Msg_Email.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void Button_Delete_Email_Click(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxes4IllegalText();

        if (!string.IsNullOrEmpty(TextboxEmailAddress.Text.Trim()))
        {
          #region Add Email

          var insertSql =
            $"DELETE FROM PartiesEmails WHERE PartyEmail = {SqlLit(TextboxEmailAddress.Text.Trim())}";
          G.ExecuteSql(insertSql);

          #endregion Add Email

          Clear_Email_TextBoxes();

          Parties_Report();
          Emails_Report();

          Msg_Email.Text =
            Ok("The email address has been deleted.");
        }
        else
          Msg_Email.Text =
            Fail("No email address was selected for deletion.");
      }
      catch (Exception ex)
      {
        #region

        Msg_Email.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonDelete_Click1(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxes4IllegalText();

        #region Only Minor National Parties or State Parties can be deleted

        //Master user maintaining all States
        if (Session["UserPartyKey"].ToString() == string.Empty)
        {
          //if (db.Parties_Int(ViewState["PartyKey"].ToString(), "PartyLevel") != 2)
          if (Parties_Bool(ViewState["PartyKey"].ToString(), "IsPartyMajor"))
            throw new ApplicationException("Only Minor National Parties can be deleted.");
        }
        //Master user selected single State to maintain
        //else
        //{
        //  if (db.Parties_Int(ViewState["PartyKey"].ToString(), "PartyLevel") != 3)
        //    throw new ApplicationException("Only State Parties can be deleted.");
        //}

        #endregion

        #region save Party Info before we delete row

        var partyName = Parties_Str(ViewState["PartyKey"].ToString(), "PartyName");

        #endregion

        #region Delete row

        var sql = string.Empty;
        sql += " DELETE FROM Parties";
        sql += " WHERE PartyKey = " + SqlLit(ViewState["PartyKey"].ToString());
        G.ExecuteSql(sql);

        #endregion

        #region set ViewState for Add Behavior

        ViewState["PartyKey"] = string.Empty;

        #endregion

        Controls_Select_Or_Add_Party();

        Clear_Political_Party_TextBoxes();

        ThePageTitle();

        Parties_Report();

        Msg_Delete_Party.Text = Ok(partyName + " has been DELETED.");
      }
      catch (Exception ex)
      {
        #region

        Msg_Delete_Party.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void TextboxBallotCode_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxes4IllegalText();
        CheckBallotCode();
        Parties_Update_Str(
          ViewState["PartyKey"].ToString()
          , "PartyCode"
          , TextboxBallotCode.Text.Trim()
        );

        Load_Party_Data();

        Parties_Report();

        Msg_Party.Text = Ok("Ballot Code was updated.");
      }
      catch (Exception ex)
      {
        #region

        Msg_Party.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void TextBoxPartyName_TextChanged(object sender, EventArgs e)
    {
      try
      {
        #region checks

        CheckTextBoxes4IllegalText();

        if (TextBoxPartyName.Text.Trim() == string.Empty)
          throw new ApplicationException("A Party Name is required.");

        #endregion checks

        Parties_Update_Str(
          ViewState["PartyKey"].ToString()
          , "PartyName"
          , TextBoxPartyName.Text.Trim()
        );

        Load_Party_Data();

        Parties_Report();

        Msg_Party.Text = Ok("Party Name was updated.");
      }
      catch (Exception ex)
      {
        #region

        Msg_Party.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void TextBoxAddressLine1_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxes4IllegalText();
        Parties_Update_Str(
          ViewState["PartyKey"].ToString()
          , "PartyAddressLine1"
          , TextBoxAddressLine1.Text.Trim()
        );

        Load_Party_Data();

        Parties_Report();

        Msg_Party.Text = Ok("Address Line 1 was updated.");
      }
      catch (Exception ex)
      {
        #region

        Msg_Party.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void TextBoxAddressLine2_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxes4IllegalText();
        Parties_Update_Str(
          ViewState["PartyKey"].ToString()
          , "PartyAddressLine2"
          , TextBoxAddressLine2.Text.Trim()
        );

        Load_Party_Data();

        Parties_Report();

        Msg_Party.Text = Ok("Address Line 2 was updated.");
      }
      catch (Exception ex)
      {
        #region

        Msg_Party.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void TextBoxCityStateZip_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxes4IllegalText();
        Parties_Update_Str(
          ViewState["PartyKey"].ToString()
          , "PartyCityStateZip"
          , TextBoxCityStateZip.Text.Trim()
        );

        Load_Party_Data();

        Parties_Report();

        Msg_Party.Text = Ok("City, State Zip was updated.");
      }
      catch (Exception ex)
      {
        #region

        Msg_Party.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void TextboxWebAddress_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxes4IllegalText();
        Parties_Update_Str(
          ViewState["PartyKey"].ToString()
          , "PartyURL"
          , Str_Remove_Http(TextboxWebAddress.Text.Trim())
        );

        Load_Party_Data();

        Parties_Report();

        Msg_Party.Text = Ok("Web address was updated.");
      }
      catch (Exception ex)
      {
        #region

        Msg_Party.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    #region Email Maint

    protected void TextboxEmailAddress_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxes4IllegalText();
        Check_Email_Address_Empty();

        PartiesEmails_Update_Str(
          ViewState["EmailAddress"].ToString()
          , "PartyEmail"
          , Str_Remove_MailTo(TextboxEmailAddress.Text.Trim())
        );

        ViewState["EmailAddress"] =
          Str_Remove_MailTo(TextboxEmailAddress.Text.Trim());

        Load_Party_Data();
        Load_Email_Data(TextboxEmailAddress.Text.Trim());

        Parties_Report();

        Msg_Party.Text = Ok("Email address was updated.");
      }
      catch (Exception ex)
      {
        #region

        Msg_Party.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void TextboxPhone_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckTextBoxes4IllegalText();
        Check_Email_Address_Empty();

        PartiesEmails_Update_Str(
          TextboxEmailAddress.Text.Trim()
          , "PartyContactPhone"
          , TextboxPhone.Text.Trim()
        );

        Load_Party_Data();
        Load_Email_Data(TextboxEmailAddress.Text.Trim());

        Parties_Report();

        Msg_Party.Text = Ok("Phone was updated.");
      }
      catch (Exception ex)
      {
        #region

        Msg_Party.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void TextBox_First_Name_TextChanged(object sender, EventArgs e)
    {
      CheckTextBoxes4IllegalText();
      Check_Email_Address_Empty();

      PartiesEmails_Update_Str(
        TextboxEmailAddress.Text.Trim()
        , "PartyContactFName"
        , TextBox_First_Name.Text.Trim()
      );

      Load_Party_Data();
      Load_Email_Data(TextboxEmailAddress.Text.Trim());

      Parties_Report();

      Msg_Party.Text = Ok("First Name was updated.");
    }

    protected void TextBox_Last_Name_TextChanged(object sender, EventArgs e)
    {
      CheckTextBoxes4IllegalText();
      Check_Email_Address_Empty();

      PartiesEmails_Update_Str(
        TextboxEmailAddress.Text.Trim()
        , "PartyContactLName"
        , TextBox_Last_Name.Text.Trim()
      );

      Load_Party_Data();
      Load_Email_Data(TextboxEmailAddress.Text.Trim());

      Parties_Report();

      Msg_Party.Text = Ok("Last Name was updated.");
    }

    protected void TextBox_Title_TextChanged(object sender, EventArgs e)
    {
      CheckTextBoxes4IllegalText();
      Check_Email_Address_Empty();

      PartiesEmails_Update_Str(
        TextboxEmailAddress.Text.Trim()
        , "PartyContactTitle"
        , TextBox_Title.Text.Trim()
      );

      Load_Party_Data();
      Load_Email_Data(TextboxEmailAddress.Text.Trim());

      Parties_Report();

      Msg_Party.Text = Ok("Title was updated.");
    }

    #endregion Email Maint

    private void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Parties";

        if (string.IsNullOrWhiteSpace(StateCode))
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateLink("/admin/Parties.aspx?state=US", "National Parties");
          NoJurisdiction.CreateStateLinks("/admin/Parties.aspx?state={StateCode}");
          NoJurisdiction.ShowHeadOptional(false);
          NoJurisdiction.Visible = true;
          return;
        }

        //if (db.User() == db.User_.Master)
        //{
        if (!string.IsNullOrEmpty(QueryState))
          ViewState["UserStateCode"] = QueryState;
        else
          ViewState["UserStateCode"] = Session["UserStateCode"].ToString();

        //}

        #region Security Checks

        if (ViewState["UserStateCode"].ToString() == string.Empty)
        {
          HandleFatalError("The UserStateCode is missing");
        }

        #endregion Security Checks

        try
        {
          #region QueryString for PartyKey and EmailAddress

          if (!string.IsNullOrEmpty(QueryParty))
            ViewState["PartyKey"] = QueryParty;
          else
            ViewState["PartyKey"] = string.Empty;

          if (!string.IsNullOrEmpty(GetQueryString("Email")))
            ViewState["EmailAddress"] = GetQueryString("Email");
          else
            ViewState["EmailAddress"] = string.Empty;

          #endregion

          Controls_Entry();

          ThePageTitle();

          #region Renumber State Parties by 10

          var order = 0;
          var sql = string.Empty;
          sql += " SELECT PartyKey FROM Parties";
          sql += " WHERE StateCode = "
            + SqlLit(ViewState["UserStateCode"].ToString());
          sql += " ORDER BY PartyOrder";
          var tableParties = G.Table(sql);
          foreach (DataRow rowParty in tableParties.Rows)
          {
            order += 10;
            var updateSql = " UPDATE Parties";
            updateSql += " SET PartyOrder = "
              + order;
            updateSql += " WHERE PartyKey= "
              + SqlLit(rowParty["PartyKey"].ToString());
            G.ExecuteSql(updateSql);
          }

          #endregion Renumber State Parties

          Parties_Report();
        }
        catch (Exception ex)
        {
          #region

          Msg_Party.Text = Fail(ex.Message);
          Log_Error_Admin(ex);

          #endregion
        }
      }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
      try
      {
        if (Session["ErrNavBarAdmin"].ToString() != string.Empty)
          Msg_Party.Text = Fail(Session["ErrNavBarAdmin"].ToString());
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch /*(Exception ex)*/
      {
      }
    }
  }
}