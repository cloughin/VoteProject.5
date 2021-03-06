using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DB.Vote;
using DB.VoteLog;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class OfficePage : SecureAdminPage
  {
    #region from db

    public static string ElectionsUpcomingCountyLocal(string stateCode,
      string countyCode)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Elections.ElectionKey ";
      sql += " ,Elections.StateCode ";
      sql += " ,Elections.ElectionDate ";
      sql += " ,Elections.ElectionDesc ";
      sql += " ,Elections.IsViewable ";
      sql += " FROM Elections ";
      sql += " WHERE Elections.ElectionDate >= " + SqlLit(Db.DbToday);
      sql += " AND Elections.StateCode = " + SqlLit(stateCode);
      sql += " AND Elections.CountyCode = " + SqlLit(countyCode);
      sql += " ORDER BY Elections.ElectionDate DESC";
      return sql;
    }

    public static string ElectionsUpcoming(string federalCodeOrStateCode)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Elections.ElectionKey ";
      sql += " ,Elections.StateCode ";
      sql += " ,Elections.ElectionDate ";
      sql += " ,Elections.ElectionDesc ";
      sql += " ,Elections.IsViewable ";
      sql += " FROM Elections ";
      sql += " WHERE Elections.ElectionDate >= " + SqlLit(Db.DbToday);
      sql += " AND Elections.StateCode = " + SqlLit(federalCodeOrStateCode);
      sql += " ORDER BY Elections.ElectionDate DESC";
      return sql;
    }

    public static int Rows(string table, string keyName1, string keyValue1,
      string keyName2, string keyValue2)
    {
      return Db.Rows(table, keyName1, keyValue1, keyName2, keyValue2);
    }

    public static bool Is_Valid_Election(string electionKey)
    {
      if (!string.IsNullOrEmpty(electionKey))
        return G.Rows("Elections", "ElectionKey", electionKey) == 1;
      return false;
    }

    public static bool Is_Electoral_Federal(OfficeClass officeClass)
    {
      switch (officeClass)
      {
        case OfficeClass.USPresident:
        case OfficeClass.USSenate:
        case OfficeClass.USHouse:
          return true;
        default:
          return false;
      }
    }

    public static bool Is_Electoral_District_Multi_Counties(OfficeClass officeClass)
    {
      switch (officeClass)
      {
        case OfficeClass.StateDistrictMultiCounties:
        case OfficeClass.StateDistrictMultiCountiesJudicial:
        case OfficeClass.StateDistrictMultiCountiesParty:
          return true;
        default:
          return false;
      }
    }

    public static bool Is_Electoral_County(OfficeClass officeClass)
    {
      switch (officeClass)
      {
        case OfficeClass.CountyExecutive:
        case OfficeClass.CountyLegislative:
        case OfficeClass.CountySchoolBoard:
        case OfficeClass.CountyCommission:
        case OfficeClass.CountyJudicial:
        case OfficeClass.CountyParty:
          return true;
        default:
          return false;
      }
    }

    public static bool Is_Electoral_District_Multi_Partial_Counties(OfficeClass officeClass)
    {
      switch (officeClass)
      {
        case OfficeClass.StateDistrictMultiPartialCounties:
          return true;
        default:
          return false;
      }
    }

    public static bool Is_Electoral_Federal(string officeKey)
    {
      return Is_Electoral_Federal(Offices.GetOfficeClass(officeKey));
    }

    public static bool Is_Electoral_District_Multi_Counties(string officeKey)
    {
      return Is_Electoral_District_Multi_Counties(Offices.GetOfficeClass(officeKey));
    }

    public static bool Is_OfficeKey_County(string officeKey)
    {
      return Is_Electoral_County(Offices.GetOfficeClass(officeKey));
    }

    public static bool Is_Electoral_District_Multi_Partial_Counties(string officeKey)
    {
      return Is_Electoral_District_Multi_Partial_Counties(Offices.GetOfficeClass(officeKey));
    }

    public static bool Is_Electoral_District(string officeKey)
    {
      //Need a DistrictCode
      return Offices.IsUSHouse(officeKey)
        || Offices.IsStateSenate(officeKey)
        || Offices.IsStateHouse(officeKey)
        || Is_Electoral_District_Multi_Counties(officeKey)
        || Is_Electoral_District_Multi_Partial_Counties(officeKey);
    }

    public static ElectoralClass Electoral_Class(string stateCode, string countyCode,
      string localCode)
    {
      if ((!string.IsNullOrEmpty(localCode)) && (!string.IsNullOrEmpty(countyCode)) &&
        (!string.IsNullOrEmpty(stateCode)))
        return ElectoralClass.Local;

      if ((!string.IsNullOrEmpty(countyCode)) && (!string.IsNullOrEmpty(stateCode)))
        return ElectoralClass.County;

      if (!string.IsNullOrEmpty(stateCode))
        switch (stateCode)
        {
          case "PP":
          case "US":
          case "U1":
            return ElectoralClass.USPresident;
          case "U2":
            return ElectoralClass.USSenate;
          case "U3":
            return ElectoralClass.USHouse;
          case "U4":
            return ElectoralClass.USGovernors;
          default:
            return StateCache.IsValidStateCode(stateCode)
              ? ElectoralClass.State
              : ElectoralClass.All;
        }

      return ElectoralClass.Unknown;
    }

    public static string Ok(string msg)
    {
      return "<span class=" + "\"" + "MsgOk" + "\"" + ">"
        + "SUCCESS!!! " + msg + "</span>";
    }

    public static string Fail(string msg)
    {
      return "<span class=" + "\"" + "MsgFail" + "\"" + ">"
        + "****FAILURE**** " + msg + "</span>";
    }

    public static string Message(string msg)
    {
      return "<span class=" + "\"" + "Msg" + "\"" + ">"
        + msg + "</span>";
    }

    public static string Warn(string msg)
    {
      return "<span class=" + "\"" + "MsgWarn" + "\"" + ">"
        + "******WARNING****** " + msg + "</span>";
    }

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

    public static bool Is_Digits(string strToCheck)
    {
      var chars = strToCheck.ToCharArray(0, strToCheck.Length);
      for (var i = 0; i <= strToCheck.Length - 1; i++)
      {
        //if (!db.Is_Digit(chars[i]))
        if (!char.IsDigit(chars[i]))
          return false;
      }
      return true;
    }

    public static bool Is_TextBox_Html(TextBox textBox)
    {
      return textBox.Text.IndexOf("<", StringComparison.Ordinal) >= 0
        || textBox.Text.IndexOf("/>", StringComparison.Ordinal) >= 0;
    }

    public static bool Is_Str_Script(string strToCheck)
    {
      return strToCheck.Trim().ToUpper().IndexOf("<SCRIPT", StringComparison.Ordinal) >= 0;
    }

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
          (char.IsWhiteSpace(c))
            || (char.IsPunctuation(c))
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

    public static string Str_Remove_Puctuation(string str2Modify)
    {
      var str = str2Modify;
      //characters to strip off
      str = str.Trim();
      str = str.Replace("\"", string.Empty);
      str = str.Replace("\'", string.Empty);
      str = str.Replace(".", string.Empty);
      str = str.Replace(",", string.Empty);
      str = str.Replace("(", string.Empty);
      str = str.Replace(")", string.Empty);
      str = str.Replace("[", string.Empty);
      str = str.Replace("[", string.Empty);
      str = str.Replace("_", string.Empty);
      str = str.Replace("-", string.Empty);
      return str;
    }

    public static string Str_Replace_Puctuation_With(string str2Modify, string strReplaceWith)
    {
      var str = str2Modify;
      //characters to strip off
      str = str.Trim();
      str = str.Replace("\"", strReplaceWith);
      str = str.Replace("\'", strReplaceWith);
      str = str.Replace(".", strReplaceWith);
      str = str.Replace(",", strReplaceWith);
      str = str.Replace("(", strReplaceWith);
      str = str.Replace(")", strReplaceWith);
      str = str.Replace("[", strReplaceWith);
      str = str.Replace("[", strReplaceWith);
      str = str.Replace("_", strReplaceWith);
      str = str.Replace("-", strReplaceWith);
      return str;
    }

    public static string DbErrorMsg(string sql, string err)
    {
      //Write code to log database errors to a DBFailues Table
      return "Database Failure for SQL Statement::" + sql + " :: Error Msg:: " + err;
    }

    public static string Fix_Url_Parms(string url)
    {
      //sets first parm in a query string to ? if all seperators are &'s
      if (url.IndexOf('?', 0, url.Length) == -1 //no ?
          && url.IndexOf('&', 0, url.Length) > -1) //but one or more &
      {
        var loc = url.IndexOf('&', 0, url.Length);
        var lenAfter = url.Length - loc - 1;
        var urlBefore = url.Substring(0, loc);
        var urlAfter = url.Substring(loc + 1, lenAfter);
        return urlBefore + "?" + urlAfter;
      }
      return url;
    }

    public static string Url_Add_State_County_Local_Codes()
    {
      //Add only lower level codes
      var urlParms = string.Empty;
      switch (Electoral_Class(
        G.State_Code() //db.State_Code()
        , G.County_Code()
        , G.Local_Code()))
      {
        case ElectoralClass.State:
          if (!string.IsNullOrEmpty(G.State_Code()))
            urlParms += "&State=" + G.State_Code();
          break;
        case ElectoralClass.County:
          if (!string.IsNullOrEmpty(G.State_Code()))
            urlParms += "&State=" + G.State_Code();
          if (!string.IsNullOrEmpty(G.User_CountyCode()))
            urlParms += "&County=" + G.User_CountyCode();
          break;
        case ElectoralClass.Local:
          if (!string.IsNullOrEmpty(G.State_Code()))
            urlParms += "&State=" + G.State_Code();
          if (!string.IsNullOrEmpty(G.User_CountyCode()))
            urlParms += "&County=" + G.User_CountyCode();
          if (!string.IsNullOrEmpty(G.User_LocalCode()))
            urlParms += "&Local=" + G.User_LocalCode();
          break;
        default: //for Federal Codes U!...U4
          if (!string.IsNullOrEmpty(G.State_Code()))
            urlParms += "&State=" + G.State_Code();
          break;
      }
      return urlParms;
    }

    public static string Ur4AdminOffice()
    {
      return "/Admin/Office.aspx";
    }

    public static string Url_Admin_Office_UPDATE(string officeKey)
    {
      var url = Ur4AdminOffice();
      url += "&Office=" + officeKey;
      //Url += db.xUrl_Add_ViewState_DataCodes_OfficeKey(OfficeKey);
      return Fix_Url_Parms(url);
    }

    //public static string Url_Admin_Politician()
    //{
    //  return "/Admin/Politician.aspx";
    //}

    public static DataRow Row(string sql)
    {
      var table = G.Table(sql);
      if (table.Rows.Count == 1)
        return table.Rows[0];
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a unique row for this Id."));
    }

    public static DataRow Row_Last_Optional(string sql)
    {
      var table = G.Table(sql);
      return table.Rows.Count >= 1
        ? table.Rows[table.Rows.Count - 1]
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
        if (result != null) result = result.Trim();
        return result;
      }
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    public static int Single_Key_Int(string tableName, string column, string keyName, string keyValue)
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

    public static bool Single_Key_Bool(string tableName, string column, string keyName, string keyValue)
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
      string keyName, string keyValue)
    {
      Db.UpdateColumnByKey(table, column, columnValue, keyName, keyValue);
    }

    public static void Single_Key_Update_Int(string table, string column, int columnValue,
      string keyName, string keyValue)
    {
      var updateSQL = "UPDATE " + table
        + " SET " + column + " = " + columnValue
        + " WHERE " + keyName + " = " + SqlLit(keyValue);
      G.ExecuteSql(updateSQL);
    }

    public static void Single_Key_Update_Bool(string table, string column, bool columnValue,
      string keyName, string keyValue)
    {
      var updateSQL = "UPDATE " + table
        + " SET " + column + " = " + Convert.ToUInt16(columnValue)
        + " WHERE " + keyName + " = " + SqlLit(keyValue);
      G.ExecuteSql(updateSQL);
    }

    public static string Offices_Str(string officeKey, string column)
    {
      return officeKey != string.Empty
        ? Single_Key_Str("Offices", column, "OfficeKey", officeKey)
        : string.Empty;
    }

    public static void Offices_Update_Int(string officeKey, string column, int columnValue)
    {
      Single_Key_Update_Int("Offices", column, columnValue, "OfficeKey", officeKey);
    }

    public static void Office_Delete_All_Tables_All_Rows(string officeKey)
    {
      //LogOfficeAddsDeletes.Insert(
      //  DateTime.Now,
      //  deleteOrConsolidate.ToUpper(),
      //  SecurePage.UserSecurityClass,
      //  VotePage.UserName,
      //  officeKey,
      //  Offices.GetStateCodeFromKey(officeKey),
      //  Offices.GetOfficeClass(officeKey).ToInt(),
      //  0,
      //  string.Empty,
      //  string.Empty,
      //  string.Empty,
      //  string.Empty,
      //  Offices_Str(officeKey, "OfficeLine1"),
      //  Offices_Str(officeKey, "OfficeLine2"),
      //  false,
      //  false,
      //  1,
      //  string.Empty,
      //  string.Empty,
      //  string.Empty,
      //  string.Empty,
      //  1);

      var sql = " DELETE FROM ElectionsOffices";
      sql += " WHERE OfficeKey = " + SqlLit(officeKey);
      G.ExecuteSql(sql);

      sql = " DELETE FROM ElectionsPoliticians";
      sql += " WHERE OfficeKey = " + SqlLit(officeKey);
      G.ExecuteSql(sql);

      sql = " DELETE FROM OfficesOfficials";
      sql += " WHERE OfficeKey = " + SqlLit(officeKey);
      G.ExecuteSql(sql);

      sql = " DELETE FROM Offices";
      sql += " WHERE OfficeKey = " + SqlLit(officeKey);
      G.ExecuteSql(sql);
    }

    public static string ElectionKey_Federal(string electionKey, string officeKey)
    {
      if (!string.IsNullOrEmpty(electionKey) && !string.IsNullOrEmpty(officeKey) &&
        Is_Electoral_Federal(officeKey))
      {
        var electionKeyFederal = "U";

        electionKeyFederal += Offices.GetOfficeClass(officeKey).ToInt();

        if (electionKey.Length >= Elections.ElectionKeyLengthStateOrFederal)
          electionKeyFederal +=
            electionKey.Substring(2, Elections.ElectionKeyLengthStateOrFederal - 2);

        return electionKeyFederal;
      }
      return string.Empty;
    }

    public static string ElectionKey_State(string electionKey)
    {
      if (
        (!string.IsNullOrEmpty(electionKey))
          && (electionKey.Length >= Elections.ElectionKeyLengthStateOrFederal)
        )
        return electionKey.Substring(0, Elections.ElectionKeyLengthStateOrFederal);
      return string.Empty;
    }

    public static string ElectionKey_State(string electionKey, string stateCode)
    {
      return electionKey.Length >= 12 && (stateCode.Length == 2)
        ? stateCode + electionKey.Substring(2, 10)
        : string.Empty;
    }

    public static bool Is_Electoral_Local(OfficeClass officeClass)
    {
      switch (officeClass)
      {
        case OfficeClass.LocalExecutive:
        case OfficeClass.LocalLegislative:
        case OfficeClass.LocalSchoolBoard:
        case OfficeClass.LocalCommission:
        case OfficeClass.LocalJudicial:
        case OfficeClass.LocalParty:
          return true;
        default:
          return false;
      }
    }

    public static bool Is_OfficeKey_Local(string officeKey)
    {
      return Is_Electoral_Local(Offices.GetOfficeClass(officeKey));
    }

    public static string ElectionKey_County(string electionKeyAny, string officeKey)
    {
      #region Note

      //Used to insert ElectionsOffices rows
      //ElectionKeyCounty is empty for State level offices
      //ElectionKeyCounty is ElectionKeyState + CountyCode
      //for County offices and Local offices

      #endregion Note

      if (
        (!string.IsNullOrEmpty(electionKeyAny))
          && (!string.IsNullOrEmpty(officeKey))
          && (Is_OfficeKey_County(officeKey) || (Is_OfficeKey_Local(officeKey)))
        )
      {
        return
          ElectionKey_State(
            electionKeyAny
            , Offices.GetStateCodeFromKey(officeKey)
            )
            + Offices.GetCountyCodeFromKey(officeKey);
      }
      return string.Empty;
    }

    public static string ElectionKey_Local(string electionKeyAny, string officeKey)
    {
      if (!string.IsNullOrEmpty(electionKeyAny) && !string.IsNullOrEmpty(officeKey)
          && Is_OfficeKey_Local(officeKey))
      {
        return ElectionKey_State(electionKeyAny, Offices.GetStateCodeFromKey(officeKey)) +
          Offices.GetCountyCodeFromKey(officeKey) + Offices.GetLocalCodeFromKey(officeKey);
      }
      return string.Empty;
    }

    public static string Elections_Str(string electionKey, string columnName)
    {
      return Elections_Str(electionKey, Elections.GetColumn(columnName));
    }

    public static string Elections_Str(string electionKey, Elections.Column column)
    {
      var value = Elections.GetColumn(column, electionKey);
      if (value == null) return string.Empty;
      return value as string;
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

    private const int ElectoralUndefined = 99;
    private const int ElectoralCounty = 4;
    private const int ElectoralLocal = 6;

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

    public static HtmlTableRow Add_Tr_To_Table_Return_Tr(HtmlTable htmlTable, string rowClass)
    {
      return Add_Tr_To_Table_Return_Tr(htmlTable, rowClass, string.Empty);
    }

    public static void Add_Td_To_Tr(HtmlTableRow htmlTr, string text, string tdClass, string align,
      int colspan = 0)
    {
      //<td Class="TdClass">
      var htmlTableCell = new HtmlTableCell { InnerHtml = text };
      if (tdClass != string.Empty)
        htmlTableCell.Attributes["class"] = tdClass;
      if (align != string.Empty)
        htmlTableCell.Attributes["align"] = align;
      if (colspan != 0)
        htmlTableCell.Attributes["colspan"] = colspan.ToString(CultureInfo.InvariantCulture);
      //</td>
      htmlTr.Cells.Add(htmlTableCell);
    }

    public static void Add_Td_To_Tr(HtmlTableRow htmlTr, string text, string tdClass, int colspan)
    {
      Add_Td_To_Tr(htmlTr, text, tdClass, string.Empty, colspan);
    }

    public static void Add_Td_To_Tr(HtmlTableRow htmlTr, string text, string tdClass)
    {
      Add_Td_To_Tr(htmlTr, text, tdClass, string.Empty);
    }

    private static bool Is_Str_Html(string strTest)
    {
      if (
        strTest.IndexOf("<", StringComparison.Ordinal) >= 0
          || strTest.IndexOf("/>", StringComparison.Ordinal) >= 0
        )
        return true;
      return false;
    }

    private static bool Is_Str_Html_Or_Illegal(string str)
    {
      if (
        Is_Str_Html(str)
          || (Is_Str_Script(str))
        )
        return true;
      return false;
    }

    private static void Throw_Exception_If_Html_Or_Script(string str)
    {
      if (Is_Str_Html_Or_Illegal(str))
        throw new ApplicationException(
          "The text has Html or illegal tags.");
    }

    private static string Anchor_Admin_Office_UPDATE_Office(
      string officeKey
      , string anchorText
      , string target
      )
    {
      var anchor = string.Empty;
      anchor += "<a href=";

      anchor += "\"";
      anchor += Url_Admin_Office_UPDATE(officeKey);
      anchor += "\"";

      anchor += " target=";
      anchor += "\"";
      if (target != string.Empty)
        anchor += target;
      else
        anchor += "office";
      anchor += "\"";

      anchor += ">";

      anchor += anchorText;
      anchor += "</a>";
      return anchor;
    }

    private static bool Is_Electoral_Class_Local(int officeClass)
    {
      return Electoral_Class(officeClass.ToOfficeClass()) == ElectoralClass.Local;
    }

    private static ElectoralClass Electoral_Class(OfficeClass officeClass)
    {
      switch (officeClass)
      {
        case OfficeClass.All:
          return ElectoralClass.All;
        case OfficeClass.USPresident:
          return ElectoralClass.USPresident;
        case OfficeClass.USSenate:
          return ElectoralClass.USSenate;
        case OfficeClass.USHouse:
          return ElectoralClass.USHouse;
        case OfficeClass.USGovernors:
          return ElectoralClass.USGovernors;
        case OfficeClass.StateWide:
        case OfficeClass.StateSenate:
        case OfficeClass.StateHouse:
        case OfficeClass.StateDistrictMultiCounties:
        case OfficeClass.StateJudicial:
        case OfficeClass.StateDistrictMultiCountiesJudicial:
        case OfficeClass.StateParty:
        case OfficeClass.StateDistrictMultiCountiesParty:
          return ElectoralClass.State;
        case OfficeClass.CountyExecutive:
        case OfficeClass.CountyLegislative:
        case OfficeClass.CountySchoolBoard:
        case OfficeClass.CountyCommission:
        case OfficeClass.CountyJudicial:
        case OfficeClass.CountyParty:
          return ElectoralClass.County;
        case OfficeClass.LocalExecutive:
        case OfficeClass.LocalLegislative:
        case OfficeClass.LocalSchoolBoard:
        case OfficeClass.LocalCommission:
        case OfficeClass.LocalJudicial:
        case OfficeClass.LocalParty:
          return ElectoralClass.Local;
        default:
          return ElectoralClass.Unknown;
      }
    }

    private static bool Is_Electoral_Class_County(int officeClass)
    {
      return Electoral_Class(officeClass.ToOfficeClass()) == ElectoralClass.County;
    }

    private static void ElectionsOffices_INSERT(
      string electionKey
      , string officeKey
      , string districtCode
      )
    {
      if (!Is_Valid_ElectionsOffices(
        electionKey
        , officeKey))
      {

        if (Is_Electoral_District(officeKey))
        {
          #region District Offices only
          if (string.IsNullOrEmpty(districtCode))
            throw new ApplicationException(
              "The office class requires DistrictCode");
          #endregion District Offices only
        }
        else
        {
          #region Non-District Offices
          if (!string.IsNullOrEmpty(districtCode))
            throw new ApplicationException(
              "The office class has a DistrictCode, but should be empty");
          #endregion Non-District Offices
        }

        //if (db.Is_Valid_ElectionsOffices(
        //  Election_Key
        //  , officeKey))
        //  throw new ApplicationException("An Office Contest already exists"
        //    + " in the ElectionsOffices Table with ElectionKey:" + Election_Key
        //    + " OfficeKey:" + officeKey);

        var sql = string.Empty;
        sql += "INSERT INTO ElectionsOffices";
        sql += "(";
        sql += " ElectionKey";
        sql += ",OfficeKey";
        sql += ",ElectionKeyState";
        sql += ",ElectionKeyFederal";
        sql += ",ElectionKeyCounty";
        sql += ",ElectionKeyLocal";
        sql += ",StateCode";
        sql += ",CountyCode";
        sql += ",LocalCode";
        sql += ",DistrictCode";
        sql += ",OfficeLevel";
        sql += ")VALUES(";
        sql += SqlLit(electionKey);
        sql += "," + SqlLit(officeKey);
        sql += "," + SqlLit(ElectionKey_State(electionKey));
        sql += "," + SqlLit(ElectionKey_Federal(electionKey, officeKey));
        sql += "," + SqlLit(ElectionKey_County(electionKey, officeKey));
        sql += "," + SqlLit(ElectionKey_Local(electionKey, officeKey));
        sql += "," + SqlLit(Offices.GetStateCodeFromKey(electionKey));
        sql += "," + SqlLit(Elections.GetCountyCodeFromKey(electionKey));
        sql += "," + SqlLit(Elections.GetLocalCodeFromKey(electionKey));
        sql += "," + SqlLit(districtCode);
        sql += "," + Offices.GetOfficeClass(officeKey).ToInt();
        sql += ")";
        G.ExecuteSql(sql);
      }
    }

    private static bool Is_Valid_ElectionsOffices(string electionKey, string officeKey)
    {
      return Is_Valid_Election_Office(electionKey, officeKey);
    }

    private static bool Is_Valid_Election_Office(string electionKey, string officeKey)
    {
      return Rows("ElectionsOffices", "ElectionKey", electionKey, "OfficeKey", officeKey) == 1;
    }

    private static bool Offices_Bool(string officeKey, string column)
    {
      return officeKey != string.Empty && Single_Key_Bool("Offices", column, "OfficeKey", officeKey);
    }

    //private static void LogOfficeChange(string officeKey, string dataItem,
    //  bool dataFrom, bool dataTo)
    //{
    //  G.LogOfficeChange(officeKey, dataItem, dataFrom.ToString(), dataTo.ToString());
    //}

    private static int Offices_Int(string officeKey, string column)
    {
      return officeKey != string.Empty
        ? Single_Key_Int("Offices", column, "OfficeKey", officeKey)
        : 0;
    }

    private static string Str_ReCase_Office_Title(string str2Fix)
    {
      var strFixed = Str_ReCase_WhiteSpace(str2Fix);

      //' of ' is always lower case
      //may have serveral Of's
      //Chairman Of Board Of Supervisors
      var startIndex = 0;
      var stringRemaining = string.Empty;
      if (strFixed.ToLower().IndexOf(" of ", StringComparison.Ordinal) != -1)
      {
        var index = strFixed.ToLower().IndexOf(" of ", startIndex, StringComparison.Ordinal);
        strFixed = strFixed.Remove(index, 4);
        strFixed = strFixed.Insert(index, " of ");
        startIndex = index + 4;
        stringRemaining = strFixed.Substring(startIndex);
      }
      if (stringRemaining.ToLower().IndexOf(" of ", StringComparison.Ordinal) != -1)
      {
        var index = strFixed.ToLower().IndexOf(" of ", startIndex, StringComparison.Ordinal);
        strFixed = strFixed.Remove(index, 4);
        strFixed = strFixed.Insert(index, " of ");
      }

      return strFixed;
    }

    private static string Str_ReCase_WhiteSpace(string str2Fix)
    {
      var sb = new StringBuilder(str2Fix.Length);
      var wordBegin = true;
      foreach (var c in str2Fix)
      {
        sb.Append(wordBegin ? char.ToUpper(c) : char.ToLower(c));
        wordBegin = char.IsWhiteSpace(c);
      }
      return sb.ToString();
    }

    //public static string OfficeKey(int officeClass, string stateCode, string countyCode, string localCode, 
    //  string districtCode, string districtCodeAlpha, string officeLine1, string officeLine2)
    //{
    //  return OfficeKey(officeClass, stateCode, countyCode, localCode, districtCode, districtCodeAlpha, 
    //    officeLine1, officeLine2, string.Empty);
    //}

    public static string OfficeKey(int officeClass, string stateCode, string countyCode, string localCode, 
      string districtCode, string districtCodeAlpha, string officeLine1, string officeLine2/*, 
      string officeKeySuffix = "", bool isSpecialOffice = false*/)
    {
      //CountyCode - 3 digits
      //LocalCode - 2 digits
      //DistrictCode - 3 digits
      //DistrictCodeAlpha - 1-4 alpha chars

      //string theOffice = (
      //  OfficeLine1 + OfficeLine2).Trim();
      //theOffice = db.Str_Remove_Puctuation(theOffice);

      officeLine1 = Str_ReCase(Str_Replace_Puctuation_With(officeLine1, " ")).Trim();
      officeLine2 = Str_ReCase(Str_Replace_Puctuation_With(officeLine2, " ")).Trim();
      var theOffice = (
        officeLine1 + officeLine2);
      theOffice = Str_Remove_Puctuation(theOffice);

      if (theOffice != string.Empty)
      {
        var stateNameUpper = StateCache.GetStateName(stateCode).ToUpper();
        //int StateNamePos = Office.ToUpper().IndexOf(StateNameUpper, 0, Office.Length);
        var stateNamePos = theOffice.ToUpper().IndexOf(stateNameUpper, 0, theOffice.Length, StringComparison.Ordinal);
        if (stateNamePos >= 0) //State is in the Office Name - remove it
        {
          var stateNameLen = stateNameUpper.Length;
          theOffice = theOffice.Remove(stateNamePos, stateNameLen);
        }
      }

      string districtCodeWithout0S;
      if (Is_Valid_Integer(districtCode))
      {
        var districtInt = Convert.ToInt32(districtCode);
        districtCodeWithout0S = districtInt.ToString(CultureInfo.InvariantCulture);
      }
      else
      {
        districtCodeWithout0S = districtCode.Trim();
      }

      var theOfficeKey = string.Empty;
      theOffice = Str_Remove_Non_Key_Chars(theOffice);
      switch (officeClass.ToOfficeClass())
      {
        case OfficeClass.USPresident:
          theOfficeKey = stateCode.ToUpper();
          //if (isSpecialOffice)
          //  theOfficeKey += theOffice;
          //else
            theOfficeKey += "President";
          break;
        case OfficeClass.USSenate:
          //if (isSpecialOffice)
          //  theOfficeKey = stateCode.ToUpper()
          //    + theOffice;
          //else
            theOfficeKey = stateCode.ToUpper()
              + "USSenate" + officeLine2;
          break;
        case OfficeClass.USHouse:
          //if (isSpecialOffice)
          //  theOfficeKey = stateCode.ToUpper()
          //    + theOffice;
          //else
            theOfficeKey = stateCode.ToUpper()
              + "USHouse"
              + districtCodeWithout0S;
          break;
        case OfficeClass.StateWide:
          theOfficeKey = stateCode.ToUpper()
            + theOffice;
          break;
        case OfficeClass.StateSenate:
          //if (isSpecialOffice)
          //  theOfficeKey = stateCode.ToUpper()
          //    + theOffice;
          //else
            theOfficeKey = stateCode.ToUpper()
              + "StateSenate"
              + districtCodeWithout0S
              + districtCodeAlpha.Trim();
          break;
        case OfficeClass.StateHouse:
          //if (isSpecialOffice)
          //  theOfficeKey = stateCode.ToUpper()
          //    + theOffice;
          //else
          {
            //if (
            //  (stateCode.ToUpper() == "ID")
            //    || (stateCode.ToUpper() == "WA")
            //  )
            //  theOfficeKey = stateCode.ToUpper()
            //    + "StateHouse" //+ officeKeySuffix
            //    + districtCodeWithout0S
            //    + districtCodeAlpha.Trim();
            //else
              theOfficeKey = stateCode.ToUpper()
                + "StateHouse"
                + districtCodeWithout0S
                + districtCodeAlpha.Trim();
          }
          break;

        case OfficeClass.StateDistrictMultiCounties:
          theOfficeKey = stateCode.ToUpper()
            + districtCode
            + districtCodeAlpha.Trim()
            + theOffice;
          break;

        case OfficeClass.CountyExecutive:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + theOffice;
          break;
        case OfficeClass.CountyLegislative:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + theOffice;
          break;
        case OfficeClass.CountySchoolBoard:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + theOffice;
          break;
        case OfficeClass.CountyCommission:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + theOffice;
          break;
        case OfficeClass.LocalExecutive:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + localCode
          + theOffice;
          break;
        case OfficeClass.LocalLegislative:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + localCode
          + theOffice;
          break;
        case OfficeClass.LocalSchoolBoard:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + localCode
          + theOffice;
          break;
        case OfficeClass.LocalCommission:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + localCode
          + theOffice;
          break;

        case OfficeClass.StateJudicial:
          theOfficeKey = stateCode.ToUpper()
            + theOffice;
          break;

        case OfficeClass.StateDistrictMultiCountiesJudicial:
          theOfficeKey = stateCode.ToUpper()
            + districtCode
            + districtCodeAlpha.Trim()
            + theOffice;
          break;
        case OfficeClass.CountyJudicial:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + theOffice;
          break;
        case OfficeClass.LocalJudicial:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + localCode
          + theOffice;
          break;
        case OfficeClass.StateParty:
          theOfficeKey = stateCode.ToUpper()
            + theOffice;
          break;
        case OfficeClass.StateDistrictMultiCountiesParty:
          theOfficeKey = stateCode.ToUpper()
            + districtCode
            + districtCodeAlpha.Trim()
            + theOffice;
          break;
        case OfficeClass.CountyParty:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + theOffice;
          break;
        case OfficeClass.LocalParty:
          theOfficeKey = stateCode.ToUpper()
          + countyCode
          + localCode
          + theOffice;
          break;
      }
      //theOfficeKey = db.Str_Remove_SpecialChars_All_And_Spaces(theOfficeKey);
      //theOfficeKey = theOfficeKey.Replace(" ", string.Empty).Trim();
      ////Str_Remove_SpecialChars_All_And_Spaces leaves on the ' for names like O'Donnell
      //theOfficeKey = theOfficeKey.Replace("\'", string.Empty);
      //theOfficeKey = db.Str_Remove_Non_Key_Chars(theOfficeKey);

      if (theOfficeKey.Length > 150)
        theOfficeKey = theOfficeKey.Substring(0, 150);

      return theOfficeKey;
    }

    private static void Offices_Update_Bool(string officeKey, string column, bool columnValue)
    {
      Single_Key_Update_Bool("Offices", column, columnValue, "OfficeKey", officeKey);
    }

    private static void Offices_Update_Str(string officeKey, string column, string columnValue)
    {
      //string UpdateSQL = "UPDATE Offices";
      //UpdateSQL += " SET " + Column + " = " + db.SQLLit(ColumnValue.Trim());
      //UpdateSQL += " WHERE OfficeKey = " + db.SQLLit(OfficeKey);
      //db.ExecuteSQL(UpdateSQL);
      Single_Key_Update_Str("Offices", column, columnValue, "OfficeKey", officeKey);
    }

    private static void TrAddOfficesAnchor(ref HtmlTable htmlOfficesTable, OfficeClass officeClass, string stateCode, string countyCode, string localCode)
    {
      var officeHtmlTr = Add_Tr_To_Table_Return_Tr(htmlOfficesTable, "trReportDetail");
      if (officeClass.IsStateOrFederal())
      {
        Add_Td_To_Tr(officeHtmlTr
        , Anchor_Admin_Office_ADD(
            officeClass
            , Link_Text_Add_Office(officeClass, stateCode))
        , "tdReportDetail"
        , 2);
      }
      else if (officeClass.IsCounty())
      {
        Add_Td_To_Tr(officeHtmlTr
          , Anchor_Admin_Office_ADD(
              officeClass, Link_Text_Add_Office(officeClass, stateCode, countyCode))
          , "tdReportDetail"
          , 2);
      }
      else if (officeClass.IsLocal())
      {
        Add_Td_To_Tr(officeHtmlTr
          , Anchor_Admin_Office_ADD(
              officeClass, Link_Text_Add_Office(officeClass, stateCode, countyCode, localCode))
          , "tdReportDetail"
          , 2);
      }
    }

    private static string Link_Text_Add_Office(OfficeClass officeClass, string stateCode, string countyCode, string localCode)
    {
      if (officeClass.IsStateOrFederal())
      {
        //ADD Virginia Statewide Non-Judicial Offices [i.e Goverernor...]
        return "ADD "
          + StateCache.GetStateName(stateCode)
          + " " + Name_Office_Contest_And_Electoral_Plus_Offices(officeClass, stateCode, countyCode, localCode);
      }

      if (officeClass.IsCounty())
      {
        //ADD Fairfax County - Executive Offices

        //return "ADD "
        //  + db.Name_County(StateCode, CountyCode)
        //  + " - " + db.Name_Office_Contest_And_Electoral_Plus_Offices(Office_Class, StateCode, CountyCode, LocalCode);
        return "ADD "
          //+ db.Name_County(StateCode, CountyCode)
          //+ " - " 
          + Name_Office_Contest_And_Electoral_Plus_Offices(officeClass, stateCode, countyCode, localCode);
      }

      if (officeClass.IsLocal())
      {
        //ADD Sully District, Fairfax County Local and Town Executive Offices

        //return "ADD "
        //  + db.Name_Local(StateCode, CountyCode, LocalCode) + ", "
        //  + db.Name_County(StateCode, CountyCode)
        //  + " - "
        //  + db.Name_Office_Contest_And_Electoral_Plus_Offices(Office_Class, StateCode, CountyCode, LocalCode);
        return "ADD "
          //+ db.Name_Local(StateCode, CountyCode, LocalCode) 
          //+ ", "
          //+ db.Name_County(StateCode, CountyCode)
          //+ " - "
          + Name_Office_Contest_And_Electoral_Plus_Offices(officeClass, stateCode, countyCode, localCode);
      }

      return string.Empty;
    }

    private static string Name_Office_Contest_And_Electoral_Plus_Offices(OfficeClass officeClass, string stateCode, string countyCode, string localCode)
    {
      return Offices.GetLocalizedOfficeClassDescription(officeClass, stateCode,
        countyCode, localCode) + " Offices";
    }

    private static string Link_Text_Add_Office(OfficeClass officeClass, string stateCode, string countyCode)
    {
      return Link_Text_Add_Office(officeClass, stateCode, countyCode, string.Empty);
    }

    private static string Link_Text_Add_Office(OfficeClass officeClass, string stateCode)
    {
      return Link_Text_Add_Office(officeClass, stateCode, string.Empty, string.Empty);
    }

    private static string Anchor_Admin_Office_ADD(OfficeClass officeClass,
      string anchorText)
    {
      var anchor = string.Empty;
      anchor += "<a href=";

      anchor += "\"";
      anchor += Url_Admin_Office_ADD(officeClass);
      anchor += "\"";

      anchor += " target=";
      anchor += "\"";
      anchor += "office";
      anchor += "\"";

      anchor += ">";

      anchor += anchorText;
      anchor += "</a>";
      return anchor;
    }

    private static string Url_Admin_Office_ADD(OfficeClass officeClass)
    {
      var url = Ur4AdminOffice();
      url += "&Class=" + officeClass.ToInt();
      url += Url_Add_State_County_Local_Codes();
      return Fix_Url_Parms(url);
    }

    private static void TrAddOfficesAnchor(ref HtmlTable htmlOfficesTable, OfficeClass officeClass, string stateCode, string countyCode)
    {
      TrAddOfficesAnchor(ref  htmlOfficesTable, officeClass, stateCode, countyCode, string.Empty);
    }

    private static void TrAddOfficesAnchor(ref HtmlTable htmlOfficesTable, OfficeClass officeClass, string stateCode)
    {
      TrAddOfficesAnchor(ref  htmlOfficesTable, officeClass, stateCode, string.Empty, string.Empty);
    }

    //private static string Url_Admin_Politician_Office(
    //  string officeKey)
    //{
    //  var url = Url_Admin_Politician();

    //  if (!string.IsNullOrEmpty(officeKey))
    //    url += "&Office=" + officeKey;

    //  return Fix_Url_Parms(url);
    //}

    private static int Office_Positions(string officeKey)
    {
      return Offices_Int(officeKey, "Incumbents");
    }

    private static string PageTitle_Election(string electionKey)
    {
      //if (db.Is_Valid_Election(ElectionKey))
      //{
      //  return db.Elections_Str(ElectionKey, "ElectionDesc");
      //}
      //else
      //  throw new ApplicationException("Bad Elections Table ElectionKey:" + ElectionKey);
      //---------------
      if (Is_Valid_Election(electionKey))
        return Elections_Str(electionKey, "ElectionDesc");

      var electionKeyState = ElectionKey_State(
        electionKey
        , Elections.GetStateCodeFromKey(electionKey)
        );

      return Is_Valid_Election(electionKeyState)
        ? Elections_Str(electionKey, "ElectionDesc")
        : "No Election for Election Id";
    }

    private static string Electoral_Class_Type(OfficeClass officeClass)
    {
      switch (officeClass)
      {
        case OfficeClass.All:
          return "All";
        case OfficeClass.USPresident:
          return "US President";
        case OfficeClass.USSenate:
          return "US Senate";
        case OfficeClass.USHouse:
          return "US House";
        case OfficeClass.StateWide:
          return "Statewide";
        case OfficeClass.StateSenate:
          return "State Senate";
        case OfficeClass.StateHouse:
          return "State House";
        case OfficeClass.StateDistrictMultiCounties:
          return "Multi County";
        case OfficeClass.CountyExecutive:
          return "Executive";
        case OfficeClass.LocalExecutive:
          return "Executive";
        case OfficeClass.CountyLegislative:
          return "Legislative";
        case OfficeClass.LocalLegislative:
          return "Legislative";
        case OfficeClass.CountySchoolBoard:
          return "School Board";
        case OfficeClass.LocalSchoolBoard:
          return "School Board";
        case OfficeClass.CountyCommission:
          return "Commission";
        case OfficeClass.LocalCommission:
          return "Commission";
        case OfficeClass.StateJudicial:
          return "Judicial";
        case OfficeClass.StateDistrictMultiCountiesJudicial:
          return "Judicial";
        case OfficeClass.CountyJudicial:
          return "Judicial";
        case OfficeClass.LocalJudicial:
          return "Judicial";
        case OfficeClass.StateParty:
          return "Political Party";
        case OfficeClass.StateDistrictMultiCountiesParty:
          return "Political Party";
        case OfficeClass.CountyParty:
          return "Political Party";
        case OfficeClass.LocalParty:
          return "Political Party";
        default:
          return string.Empty;
      }
    }

    #endregion from db

    internal override IEnumerable<string> NonStateCodesAllowed
    {
      get { return new []{ "US" }; }
    }

    #region checks

    private void Check_TextBoxs_Illeagal_Input()
    {
      //Election Authority Contact Information for Vote-USA
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Line_Add_1);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Line_Add_2);
      Throw_Exception_TextBox_Html_Or_Script(TextBoxDistrict);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Order);
      //db.Throw_Exception_TextBox_Html_Or_Script(TextBox_Remove_Incumbent);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Vote_Instructions);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Line_Edit_1);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Line_Edit_2);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_WriteIn_Instructions);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Vote_Wording);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_WriteIn_Wording);
      Throw_Exception_TextBox_Html_Or_Script(TextBoxWriteInLines);
      Throw_Exception_TextBox_Html_Or_Script(TextBoxOfficeOrderOnBallot);
      Throw_Exception_TextBox_Html_Or_Script(TextBoxOfficePositions);
      Throw_Exception_TextBox_Html_Or_Script(TextBoxElectionPositions);
      Throw_Exception_TextBox_Html_Or_Script(TextBoxPrimaryPositions);
      //db.Throw_Exception_TextBox_Html_Or_Script(TextBox_RunningMate);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Title_Search);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_CountyCode);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_LocalCode);
      Throw_Exception_TextBox_Html_Or_Script(TextBox_OfficeKey);
    }
    #endregion checks

    private enum FormFunction
    {
      UpdateOfficeElection,
      AddOfficeContest,
      UpdateOffice,
      AddOfficeAtLevel,
      BulkOfficeAdditionsCounty,
      BulkOfficeAdditionsLocal,
      Unknown
    }

    private FormFunction Function()
    {
      if (Convert.ToInt16(ViewState["Electoral"]) == ElectoralCounty)
        return FormFunction.BulkOfficeAdditionsCounty;
      if (Convert.ToInt16(ViewState["Electoral"]) == ElectoralLocal)
        return FormFunction.BulkOfficeAdditionsLocal;
      if (!(string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()) ||
       string.IsNullOrEmpty(ViewState["OfficeKey"].ToString())))
        return FormFunction.UpdateOfficeElection;
      if (!(string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()) ||
       !string.IsNullOrEmpty(ViewState["OfficeKey"].ToString())))
        return FormFunction.AddOfficeContest;
      if (!string.IsNullOrEmpty(ViewState["OfficeKey"].ToString())
       && (string.IsNullOrEmpty(ViewState["ElectionKey"].ToString())))
        return FormFunction.UpdateOffice;
      if (Offices.GetOfficeClass(ViewState["OfficeKey"].ToString()) != OfficeClass.Undefined)
        return FormFunction.AddOfficeAtLevel;
      return FormFunction.Unknown;
    }

    private string Form_Function_Description()
    {
      switch (Function())
      {
        case FormFunction.UpdateOfficeElection:
          return "Update this Office Contest in this Election";
        case FormFunction.AddOfficeContest:
          return "Add an Office Contest in this Election";
        case FormFunction.UpdateOffice:
          return "Update this Office";
        case FormFunction.AddOfficeAtLevel:
          return "Add Office(s)";
        case FormFunction.BulkOfficeAdditionsCounty:
          return "Bulk COUNTY Office Additions";
        case FormFunction.BulkOfficeAdditionsLocal:
          return "Bulk LOCAL DISTRICT Office Additions";
        case FormFunction.Unknown:
          return "Form Purpose is Unknown";
        default:
          return "Form Purpose is Unknown";
      }
    }
    private void Clear_Office_Title_Textboxes()
    {
      TextBox_Office_Line_Add_1.Text = string.Empty;
      TextBox_Office_Line_Add_2.Text = string.Empty;
      TextBox_Order.Text = string.Empty;
    }
    private void Page_Title()
    {
      H1.InnerHtml = string.Empty;

      H1.InnerHtml += Offices.GetElectoralClassDescription(
           ViewState["StateCode"].ToString()
          , ViewState["CountyCode"].ToString()
          , ViewState["LocalCode"].ToString());

      if (Convert.ToInt16(ViewState["OfficeClass"]) != ElectoralUndefined)
      {
        H1.InnerHtml += "<br>";
        H1.InnerHtml += Electoral_Class_Type(ViewState["OfficeClass"].ToOfficeClass());
      }

      H1.InnerHtml += "<br>";
      switch (Function())
      {
        case FormFunction.UpdateOfficeElection:
          H1.InnerHtml += Offices.FormatOfficeName(ViewState["OfficeKey"].ToString(), 
            "<br />");

          H1.InnerHtml += "<br>";
          H1.InnerHtml += PageTitle_Election(ViewState["ElectionKey"].ToString());
          H1.InnerHtml += " Update this Office Contest in this Election";
          break;
        case FormFunction.AddOfficeContest:
          H1.InnerHtml += "<br>";
          H1.InnerHtml += PageTitle_Election(ViewState["ElectionKey"].ToString());
          H1.InnerHtml += " Add an Office Contest in this Election";
          break;
        case FormFunction.UpdateOffice:
          H1.InnerHtml += Offices.FormatOfficeName(ViewState["OfficeKey"].ToString());
          H1.InnerHtml += " Update this Office";
          break;
        case FormFunction.AddOfficeAtLevel:
          H1.InnerHtml += " Add Office";
          break;
        case FormFunction.BulkOfficeAdditionsCounty:
          H1.InnerHtml += " Bulk COUNTY Office Additions";
          break;
        case FormFunction.BulkOfficeAdditionsLocal:
          H1.InnerHtml += " Bulk LOCAL DISTRICT Office Additions";
          break;
      }
    }

    #region SQL

    private static string SQLOffices4USPres()
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Offices.OfficeKey ";
      sql += " ,Offices.StateCode ";
      sql += " ,Offices.OfficeLevel ";
      sql += " ,Offices.OfficeOrderWithinLevel ";
      sql += " ,Offices.OfficeLine1 ";
      sql += " ,Offices.OfficeLine2 ";
      sql += " ,Offices.DistrictCode ";
      sql += " ,Offices.DistrictCodeAlpha ";
      sql += " ,Offices.CountyCode ";
      sql += " ,Offices.LocalCode ";
      sql += " ,Offices.OfficeOrderWithinLevel ";
      sql += " ,Offices.IsRunningMateOffice ";
      sql += " ,Offices.Incumbents ";
      sql += " ,Offices.VoteInstructions ";
      sql += " ,Offices.VoteForWording ";
      sql += " ,Offices.WriteInInstructions ";
      sql += " ,Offices.WriteInWording ";
      sql += " ,Offices.WriteInLines ";
      sql += ",Offices.IsOnlyForPrimaries ";
      sql += ",Offices.IsInactive";
      sql += " FROM Offices ";
      sql += " WHERE Offices.OfficeLevel = " + OfficeClass.USPresident.ToInt();
      return sql;
    }

    private string SqlOffices4Level(string stateCode, int officeClass)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Offices.OfficeKey ";
      sql += " ,Offices.StateCode ";
      sql += " ,Offices.OfficeLevel ";
      sql += " ,Offices.OfficeOrderWithinLevel ";
      sql += " ,Offices.OfficeLine1 ";
      sql += " ,Offices.OfficeLine2 ";
      sql += " ,Offices.DistrictCode ";
      sql += " ,Offices.DistrictCodeAlpha ";
      sql += " ,Offices.CountyCode ";
      sql += " ,Offices.LocalCode ";
      sql += " ,Offices.OfficeOrderWithinLevel ";
      sql += " ,Offices.IsRunningMateOffice ";
      sql += " ,Offices.Incumbents ";
      sql += " ,Offices.VoteInstructions ";
      sql += " ,Offices.VoteForWording ";
      sql += " ,Offices.WriteInInstructions ";
      sql += " ,Offices.WriteInWording ";
      sql += " ,Offices.WriteInLines ";
      sql += ",Offices.IsOnlyForPrimaries ";
      sql += ",Offices.IsInactive";
      sql += " FROM Offices ";
      sql += " WHERE Offices.StateCode = " + SqlLit(stateCode);
      sql += " AND Offices.OfficeLevel = " + officeClass;
      sql += " ORDER BY Offices.OfficeOrderWithinLevel";
      sql += " ,Offices.DistrictCode";
      return sql;
    }

    private string SqlOffices4Level(string stateCode, int officeClass, string countyCode)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Offices.OfficeKey ";
      sql += " ,Offices.StateCode ";
      sql += " ,Offices.OfficeLevel ";
      sql += " ,Offices.OfficeOrderWithinLevel ";
      sql += " ,Offices.OfficeLine1 ";
      sql += " ,Offices.OfficeLine2 ";
      sql += " ,Offices.DistrictCode ";
      sql += " ,Offices.DistrictCodeAlpha ";
      sql += " ,Offices.CountyCode ";
      sql += " ,Offices.LocalCode ";
      sql += " ,Offices.OfficeOrderWithinLevel ";
      sql += " ,Offices.IsRunningMateOffice ";
      sql += " ,Offices.Incumbents ";
      sql += " ,Offices.VoteInstructions ";
      sql += " ,Offices.VoteForWording ";
      sql += " ,Offices.WriteInInstructions ";
      sql += " ,Offices.WriteInWording ";
      sql += " ,Offices.WriteInLines ";
      sql += ",Offices.IsOnlyForPrimaries ";
      sql += ",Offices.IsInactive";
      sql += " FROM Offices ";
      sql += " WHERE Offices.StateCode = " + SqlLit(stateCode);
      sql += " AND Offices.OfficeLevel = " + officeClass;
      sql += " AND Offices.CountyCode = " + SqlLit(countyCode);
      sql += " ORDER BY Offices.OfficeOrderWithinLevel";
      return sql;
    }

    private string SqlOffices4Level(string stateCode, int officeClass, string countyCode, string localCode)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " Offices.OfficeKey ";
      sql += " ,Offices.StateCode ";
      sql += " ,Offices.OfficeLevel ";
      sql += " ,Offices.OfficeOrderWithinLevel ";
      sql += " ,Offices.OfficeLine1 ";
      sql += " ,Offices.OfficeLine2 ";
      sql += " ,Offices.DistrictCode ";
      sql += " ,Offices.DistrictCodeAlpha ";
      sql += " ,Offices.CountyCode ";
      sql += " ,Offices.LocalCode ";
      sql += " ,Offices.OfficeOrderWithinLevel ";
      sql += " ,Offices.IsRunningMateOffice ";
      sql += " ,Offices.Incumbents ";
      sql += " ,Offices.VoteInstructions ";
      sql += " ,Offices.VoteForWording ";
      sql += " ,Offices.WriteInInstructions ";
      sql += " ,Offices.WriteInWording ";
      sql += " ,Offices.WriteInLines ";
      sql += ",Offices.IsOnlyForPrimaries ";
      sql += ",Offices.IsInactive";
      sql += " FROM Offices ";
      sql += " WHERE Offices.StateCode = " + SqlLit(stateCode);
      sql += " AND Offices.OfficeLevel = " + officeClass;
      sql += " AND Offices.CountyCode = " + SqlLit(countyCode);
      sql += " AND Offices.LocalCode = " + SqlLit(localCode);
      sql += " ORDER BY Offices.OfficeOrderWithinLevel";
      return sql;
    }

    private string SqlOffices4Level()
    {
      if (ViewState["OfficeClass"].ToOfficeClass() == OfficeClass.USPresident)
      {
        #region US President
        return SQLOffices4USPres();
        #endregion US President
      }
      if (ViewState["OfficeClass"].ToOfficeClass().IsStateOrFederal())
      {
        #region Federal and State Offices
        return SqlOffices4Level(ViewState["StateCode"].ToString()
          , Convert.ToInt32(ViewState["OfficeClass"])
          );
        #endregion Federal and State Offices
      }
      if (ViewState["OfficeClass"].ToOfficeClass().IsCounty())
      {
        #region County Offices
        return SqlOffices4Level(ViewState["StateCode"].ToString()
          , Convert.ToInt32(ViewState["OfficeClass"])
          , ViewState["CountyCode"].ToString()
          );
        #endregion County Offices
      }
      if (ViewState["OfficeClass"].ToOfficeClass().IsLocal())
      {
        #region Local District Offices
        return SqlOffices4Level(ViewState["StateCode"].ToString()
          , Convert.ToInt32(ViewState["OfficeClass"])
          , ViewState["CountyCode"].ToString()
          , ViewState["LocalCode"].ToString()
          );
        #endregion Local District Offices
      }
      return string.Empty;
    }

    #endregion SQL

    #region Visible Controls

    // [Add an Office]
    private void Table_Add_Office_Data_Visible_Load()
    {
      #region Conditions for Visible and Load
      //no ElectionKey
      //no OfficeKey        
      if (
      (string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
      && (string.IsNullOrEmpty(ViewState["OfficeKey"].ToString()))
        //&& (string.IsNullOrEmpty(ViewState["Electoral"].ToString()))
        )
      #endregion Conditions for Visible and Load
      {
        Table_Add_Office_Data.Visible = true;

        trAdditionalLinesAdd.Visible = false;//
        if (ViewState["CountyCode"].ToString() != string.Empty)
        {
          trAdditionalLinesAdd.Visible = true;//
          trCountyNameAdd.Visible = true;//
          LabelCountyNameAdd.Text = CountyCache.GetCountyName(
            ViewState["StateCode"].ToString()
          , ViewState["CountyCode"].ToString()
          );//
        }
        else
        {
          trCountyNameAdd.Visible = false;//
        }

        if (ViewState["LocalCode"].ToString() != string.Empty)
        {
          trAdditionalLinesAdd.Visible = true;//
          trLocalDistrictNameAdd.Visible = true;//
          LabelLocalDistrictNameAdd.Text = GetPageCache().LocalDistricts.GetLocalDistrict(
            ViewState["StateCode"].ToString()
          , ViewState["CountyCode"].ToString()
          , ViewState["LocalCode"].ToString());//
        }
        else
        {
          trLocalDistrictNameAdd.Visible = false;//
        }
      }
      else
      {
        Table_Add_Office_Data.Visible = false;
      }
    }

    //[Add Office]
    private void Table_Add_Office_Visible()
    {
      #region Conditions for Visible and Load
      //no ElectionKey
      //no OfficeKey        
      if (
        (string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
        && (string.IsNullOrEmpty(ViewState["OfficeKey"].ToString()))
        //&& (db.QueryString("Mode") != "Bulk")
        && (string.IsNullOrEmpty(GetQueryString("Electoral")))
        )
      //if (string.IsNullOrEmpty(ViewState["OfficeKey"].ToString()))
      #endregion Conditions for Visible and Load
      {
        Table_Add_Office.Visible = true;
      }
      else
      {
        Table_Add_Office.Visible = false;
      }
    }

    // [Add Office and Add as Office Contest in this Election]
    private void Table_Add_Office_And_In_Election_Visible()
    {
      #region Conditions for Visible and Load
      //For now - supress when creating an election
      //need ElectionKey
      //need OfficeKey        
      //if (
      //(!string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
      //&& (!string.IsNullOrEmpty(ViewState["OfficeKey"].ToString()))
      //&&(db.QueryString("Mode") != "Bulk")
      //  )
      #endregion Conditions for Visible and Load
      //{
      //  Table_Add_Office_And_In_Election.Visible = true;
      //}
      //else
      //{
      Table_Add_Office_And_In_Election.Visible = false;
      //}
    }

    // [Add Another Office]
    private void Table_Another_Office_Visible()
    {
      #region Conditions for Visible and Load
      //need OfficeLevel
      if (ViewState["OfficeClass"].ToOfficeClass() != OfficeClass.Undefined
        && (string.IsNullOrEmpty(GetQueryString("Electoral"))))
      #endregion Conditions for Visible and Load
      {
        Table_Another_Office.Visible = true;

        HyperLinkAddAnotherOffice.NavigateUrl = Url_Admin_Office_ADD(
          ViewState["OfficeClass"].ToOfficeClass());
      }
      else
      {
        Table_Another_Office.Visible = false;
      }
    }

    // [Current Office Incumbent(s)]
    private void Table_Incumbent_Visible_Load()
    {
      #region Conditions for Visible and Load
      //no ElectionKey
      //need OfficeKey
      if (
        (string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
        && (!string.IsNullOrEmpty(ViewState["OfficeKey"].ToString()))
        )
      #endregion Conditions for Visible and Load
      {
        Table_Incumbent.Visible = true;

        #region Incumbnet Msg.
        var officeIncumbents = Office_Positions(ViewState["OfficeKey"].ToString());
        var officesOfficialsRows = G.Rows("OfficesOfficials"
          , "OfficeKey", ViewState["OfficeKey"].ToString());
        var incumbentPositionsOpen = officeIncumbents - officesOfficialsRows;
        if (incumbentPositionsOpen == 0)
        {
          LabelIncumbentMsg.Text = "Currently the incumbent(s) have been identified.";
          //LabelIncumbentMsg.Text += " To change an incumbent, you need to first remove an incumbent"
          //+ " and then add the correct incumbent.";
        }
        else if (incumbentPositionsOpen > 0)
          LabelIncumbentMsg.Text = "Currently " + incumbentPositionsOpen
            + " positions are vacant or not yet identified.";
        else
        {
          incumbentPositionsOpen = -incumbentPositionsOpen;
          LabelIncumbentMsg.Text = "Currently there are " + incumbentPositionsOpen
            + " more incumbents than there are incumbent positions.";
          //LabelIncumbentMsg.Text += " You need to first remove the extra incumbent(s)"
          //+ " before you can add any correct incumbent(s).";
        }
        #endregion Incumbnet Msg.

        //TextBox_Remove_Incumbent.Text = string.Empty;
        //TextBoxSelectIncumbent.Text = string.Empty;

        //IncumbentTable.Text = db.Report_Incumbents(ViewState["OfficeKey"].ToString());
        IncumbentsReport.GetReport(ViewState["OfficeKey"].ToString()).
          AddTo(IncumbentsReportPlaceHolder);

        #region Remove Incumbent with ID tr
        //var sql = "OfficesOfficials,Politicians";
        //sql += " WHERE OfficesOfficials.OfficeKey = " + db.SQLLit(ViewState["OfficeKey"].ToString());
        //sql += " AND OfficesOfficials.PoliticianKey = Politicians.PoliticianKey";

        //var incumbents = db.Rows_Count_From(sql);
        //trRemoveIncumbent.Visible = incumbents > 0;
        #endregion Remove Incumbent with ID tr

        #region Find Politicianss HyperLink
        if (Offices.GetOfficeClass(ViewState["OfficeKey"].ToString()).IsValid())
        {
          //HyperLink_Find_PoliticianID_Incumbent.NavigateUrl =
          //  db.Url_Admin_Politicians(
          //  OfficeClass.All
          //  , Offices.GetStateCodeFromKey(ViewState["OfficeKey"].ToString())
          //  );
          HyperLink_EditIncumbents.NavigateUrl =
            GetOfficeIncumbentsPageUrl(ViewState["OfficeKey"].ToString());
          Table_Incumbent.Visible = true;
        }
        #endregion Find Politicianss HyperLink
      }
      else
      {
        Table_Incumbent.Visible = false;
      }
    }

    //[Edit Office]
    private void Table_Edit_Office_Visible_Load()
    {
      #region Conditions for Visible and Load
      //need OfficeKey
      if (!string.IsNullOrEmpty(ViewState["OfficeKey"].ToString()))
      #endregion Conditions for Visible and Load
      {
        Table_Edit_Office.Visible = true;

        Label_Edit_Name.Text = Offices.FormatOfficeName(
              ViewState["OfficeKey"].ToString(), ", ");
        Label_Add_Politician.Text = Offices.FormatOfficeName(
              ViewState["OfficeKey"].ToString(), ", ");

        trAdditionalLines.Visible = ((ViewState["CountyCode"].ToString() != string.Empty)
          || (ViewState["LocalCode"].ToString() != string.Empty));

        #region Load Textboxes & Labels
        var sql = string.Empty;
        sql += " SELECT ";
        sql += " Offices.OfficeKey ";
        sql += ",Offices.OfficeLevel ";
        sql += ",Offices.OfficeOrderWithinLevel ";
        sql += ",Offices.DistrictCode ";
        sql += ",Offices.DistrictCodeAlpha ";
        sql += ",Offices.CountyCode ";
        sql += ",Offices.LocalCode ";
        sql += ",Offices.OfficeLine1 ";
        sql += ",Offices.OfficeLine2 ";
        sql += ",Offices.IsRunningMateOffice ";
        sql += ",Offices.IsOnlyForPrimaries ";
        sql += ",Offices.Incumbents ";
        sql += ",Offices.VoteInstructions ";
        sql += ",Offices.VoteForWording ";
        sql += ",Offices.WriteInInstructions ";
        sql += ",Offices.WriteInWording ";
        sql += ",Offices.WriteInLines ";
        sql += ",Offices.IsInactive";
        sql += ",Offices.ElectionPositions";
        sql += ",Offices.PrimaryPositions";
        sql += ",Offices.PrimaryRunoffPositions";
        sql += ",Offices.GeneralRunoffPositions";
        sql += " FROM Offices ";
        sql += " WHERE Offices.OfficeKey = " + SqlLit(ViewState["OfficeKey"].ToString());

        var officeRow = Row(sql);
        TextBox_Office_Line_Edit_1.Text = officeRow["OfficeLine1"].ToString().Trim();//
        TextBox_Office_Line_Edit_2.Text = officeRow["OfficeLine2"].ToString().Trim();//

        trAdditionalLines.Visible = false;
        if (ViewState["CountyCode"].ToString() != string.Empty)
        {
          trAdditionalLines.Visible = true;
          trCountyName.Visible = true;
          LabelCountyName.Text = CountyCache.GetCountyName(
            ViewState["StateCode"].ToString()
          , ViewState["CountyCode"].ToString()
          );//
        }
        else
        {
          trCountyName.Visible = false;
        }

        if (ViewState["LocalCode"].ToString() != string.Empty)
        {
          trAdditionalLines.Visible = true;
          trLocalDistrictName.Visible = true;//
          LabelLocalDistrictName.Text = GetPageCache().LocalDistricts.GetLocalDistrict(
            ViewState["StateCode"].ToString()
          , ViewState["CountyCode"].ToString()
          , ViewState["LocalCode"].ToString());//
        }
        else
        {
          trLocalDistrictName.Visible = false;//
        }

        TextBox_Vote_Instructions.Text = officeRow["VoteInstructions"].ToString();//
        TextBox_Vote_Wording.Text = officeRow["VoteForWording"].ToString();//
        TextBox_WriteIn_Instructions.Text = officeRow["WriteInInstructions"].ToString();//
        TextBox_WriteIn_Wording.Text = officeRow["WriteInWording"].ToString();//
        TextBoxWriteInLines.Text = officeRow["WriteInLines"].ToString();//

        TextBoxOfficePositions.Text = officeRow["Incumbents"].ToString();//
        TextBoxElectionPositions.Text = officeRow["ElectionPositions"].ToString();//
        TextBoxPrimaryPositions.Text = officeRow["PrimaryPositions"].ToString();//
        TextBoxPrimaryRunoffPositions.Text = officeRow["PrimaryRunoffPositions"].ToString();//
        TextBoxGeneralRunoffPositions.Text = officeRow["GeneralRunoffPositions"].ToString();//
        TextBoxOfficeOrderOnBallot.Text = officeRow["OfficeOrderWithinLevel"].ToString();//

        RadioButtonListRunningMateOffice.SelectedValue = (bool)officeRow["IsRunningMateOffice"]
          ? "Yes"
          : "No";

        RadioButtonListIsOnlyForPrimaries.SelectedValue = (bool)officeRow["IsOnlyForPrimaries"]
          ? "Yes"
          : "No";

        RadioButtonListActive.SelectedValue = (bool)officeRow["IsInactive"]
          ? "Yes"
          : "No";
        #endregion Load Textboxes & Labels

        //if (db.Is_Office_HaveBallotOrder(db.Office_Class(ViewState["OfficeKey"].ToString())))
        //  trBallotOrder.Visible = true;
        //else
        //  trBallotOrder.Visible = false;

        #region USPresident

        if (ViewState["OfficeKey"].ToString() != "USPresident") return;
        if (IsSuperUser)
        {
          TextBox_Office_Line_Edit_1.Enabled = true;
          TextBox_Office_Line_Edit_2.Enabled = true;
          TextBox_Vote_Instructions.Enabled = true;
          TextBox_Vote_Wording.Enabled = true;
          TextBox_WriteIn_Instructions.Enabled = true;
          TextBox_WriteIn_Wording.Enabled = true;
          TextBoxWriteInLines.Enabled = true;
          TextBoxOfficePositions.Enabled = true;
          TextBoxElectionPositions.Enabled = true;
          TextBoxPrimaryPositions.Enabled = true;
          RadioButtonListRunningMateOffice.Enabled = true;
          //TextBox_Remove_Incumbent.Enabled = true;
          //TextBoxSelectIncumbent.Enabled = true;
          //TextBox_RunningMate.Enabled = true;
          //IncumbentTable.Enabled = true;
        }
        else
        {
          TextBox_Office_Line_Edit_1.Enabled = false;
          TextBox_Office_Line_Edit_2.Enabled = false;
          TextBox_Vote_Instructions.Enabled = false;
          TextBox_Vote_Wording.Enabled = false;
          TextBox_WriteIn_Instructions.Enabled = false;
          TextBox_WriteIn_Wording.Enabled = false;
          TextBoxWriteInLines.Enabled = false;
          TextBoxOfficePositions.Enabled = false;
          TextBoxElectionPositions.Enabled = false;
          TextBoxPrimaryPositions.Enabled = false;
          RadioButtonListRunningMateOffice.Enabled = false;
          //TextBox_Remove_Incumbent.Enabled = false;
          //TextBoxSelectIncumbent.Enabled = false;
          //TextBox_RunningMate.Enabled = false;
          //IncumbentTable.Enabled = false;
        }
        #endregion USPresident
      }
      else
      {
        Table_Edit_Office.Visible = false;
      }
    }

    // [Add New Politician as Incumbent for this Office]
    private void Table_Add_New_Politician_Without_Election_Visible_Load()
    {
      #region Conditions for Visible and Load
      //no ElectionKey
      //need OfficeKey
      if (
        (string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
        && (!string.IsNullOrEmpty(ViewState["OfficeKey"].ToString()))
        )
      #endregion Conditions for Visible and Load
      {
        Table_Add_New_Politician_Without_Election.Visible = true;

        HyperLink_Add_Politician.NavigateUrl =
          //db.Url_Admin_Politician_Incumbent(ViewState["OfficeKey"].ToString());
          Url_Admin_Politician_Office(ViewState["OfficeKey"].ToString());
      }
      else
      {
        Table_Add_New_Politician_Without_Election.Visible = false;
      }
    }

    //[Current Office Running Mate]
    //private void Table_Running_Mate_Visible()
    //{
    //  #region Conditions for Visible and Load
    //  //need ElectionKey
    //  //need OfficeKey 
    //  //is a running mate office
    //  if (
    //  Offices.IsRunningMateOffice(ViewState["OfficeKey"].ToString())
    //    )
    //  #endregion Conditions for Visible and Load
    //  {
    //    Table_Running_Mate.Visible = true;

    //    if (!string.IsNullOrEmpty(db.OfficesOfficials_Str(ViewState["OfficeKey"].ToString(), "RunningMateKey")))
    //    {
    //      TextBox_RunningMate.Text = db.OfficesOfficials_Str(ViewState["OfficeKey"].ToString(), "RunningMateKey");
    //      Label_RunningMate.Text = Politicians.GetFormattedName(TextBox_RunningMate.Text);
    //    }
    //    else
    //    {
    //      TextBox_RunningMate.Text = string.Empty;
    //      Label_RunningMate.Text = string.Empty;
    //    }
    //  }
    //  else
    //  {
    //    Table_Running_Mate.Visible = false;
    //  }
    //}

    // [Add Offices Report]
    private void Table_Report_Add_Office_Anchors_Visible_Load()
    {
      #region Conditions for Visible and Load
      //no ElectionKey
      //need OfficeKey
      //valid office level to add offices
      if (
        (string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
        && (!string.IsNullOrEmpty(ViewState["OfficeKey"].ToString()))
        && (Offices.CanAddOfficesToOfficeClass(ViewState["OfficeKey"].ToString()))
        )
      #endregion Conditions for Visible and Load
      {
        Table_Report_Add_Office_Anchors.Visible = true;

        #region Create HTML Table for Add Offices report and set attributes
        HTMLTableAddOfficesInThisCategory.Text = string.Empty;
        var htmlTableAddLinks = new HtmlTable();
        htmlTableAddLinks.Attributes["cellspacing"] = "0";
        htmlTableAddLinks.Attributes["border"] = "0";
        htmlTableAddLinks.Attributes["class"] = "tableAdmin";
        htmlTableAddLinks.Attributes["align"] = "left";
        #endregion Create HTML Table for Add Offices report and set attributes

        #region Table Heading
        if (
          Offices.GetOfficeClass(ViewState["OfficeKey"].ToString()).IsState())
        {
          #region State Offices
          LabelAddOffices.Text = "Add Other " + StateCache.GetStateName(ViewState["StateCode"].ToString())
            + " Statewide Offices (Non-Judicial)";
          #endregion Federal and State Offices
        }
        else if (
          Offices.GetOfficeClass(ViewState["OfficeKey"].ToString()).IsCounty())
        {
          #region County Offices
          LabelAddOffices.Text = "Add Other " + CountyCache.GetCountyName(ViewState["StateCode"].ToString()
            , ViewState["CountyCode"].ToString()
            )
            + " Offices in a Differnt Ballot Category";
          #endregion County Offices
        }
        else if (
         Offices.GetOfficeClass(ViewState["OfficeKey"].ToString()).IsLocal())
        {
          #region Local District Offices
          LabelAddOffices.Text = "Add Other "
            + GetPageCache().LocalDistricts.GetLocalDistrict(ViewState["StateCode"].ToString()
            , ViewState["CountyCode"].ToString()
            , ViewState["LocalCode"].ToString())
            + ", " + CountyCache.GetCountyName(ViewState["StateCode"].ToString()
            , ViewState["CountyCode"].ToString()
            )
            + " Offices in a Different Ballot Category";
          #endregion Local District Offices
        }
        #endregion Table Heading

        if (Offices.CanAddOfficesToOfficeClass(ViewState["OfficeKey"].ToString()))
        {
          #region Add Office Category Title Link to add offices at this level

          switch (Electoral_Class(
                 ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                , ViewState["LocalCode"].ToString()
           ))
          {
            case ElectoralClass.State:
              #region State Offices
              //db.AddTr2HtmlTable4AddOffices(ref HTMLTableAddLinks
              //  , db.Anchor_Admin_Office_ADD(Convert.ToInt32(ViewState["OfficeClass"])
              //  , "ADD " + db.Name_Office_Contest_And_Electoral_Plus_Offices(Convert.ToInt32(ViewState["OfficeClass"]))));

              //Non-Judicial Statewide
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.StateWide
                , ViewState["StateCode"].ToString()
                );

              //Judicial Statewide
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.StateJudicial
                , ViewState["StateCode"].ToString()
                );

              //Party Statewide
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.StateParty
                , ViewState["StateCode"].ToString()
                );
              #endregion Federal and State Offices
              break;
            case ElectoralClass.County:
              #region County Offices
              //County Executive
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.CountyExecutive
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                );

              //County Legislative
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.CountyLegislative
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                );

              //County SchoolBoard
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.CountySchoolBoard
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                );

              //County Commission
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.CountyCommission
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                );

              //County Judicial
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.CountyJudicial
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                );

              //County Party
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.CountyParty
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                );

              #endregion County Offices
              break;
            case ElectoralClass.Local:
              #region Local District Offices
              //Local Executive
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.LocalExecutive
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                , ViewState["LocalCode"].ToString()
                );

              //Local Legislative
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.LocalLegislative
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                , ViewState["LocalCode"].ToString()
                );

              //Local SchoolBoard
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.LocalSchoolBoard
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                , ViewState["LocalCode"].ToString()
                );

              //Local Commission
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.LocalCommission
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                , ViewState["LocalCode"].ToString()
                );

              //Local Judicial
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.LocalJudicial
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                , ViewState["LocalCode"].ToString()
                );

              //Local Party
              TrAddOfficesAnchor(ref htmlTableAddLinks
                , OfficeClass.LocalParty
                , ViewState["StateCode"].ToString()
                , ViewState["CountyCode"].ToString()
                , ViewState["LocalCode"].ToString()
                );

              #endregion Local District Offices
              break;
          }
          //HTMLTableAddOfficesInThisCategory.Text = db.RenderToString(HTMLTableAddLinks);

          HTMLTableAddOfficesInThisCategory.Text = htmlTableAddLinks.RenderToString();
          #endregion Add Office Category Title Link to add offices at this level
        }
      }
      else
      {
        Table_Report_Add_Office_Anchors.Visible = false;
      }
    }

    //[Edit Offices Report]
    private void Table_Report_Edit_Other_Offices_At_Level_Visible_Load()
    {
      #region Conditions for Visible and Load
      //no ElectionKey
      //need OfficeKey
      //valid office level to add offices
      if (
        (string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
        && (!string.IsNullOrEmpty(ViewState["OfficeKey"].ToString()))
        // federals are excluded by following condition
        && Offices.CanAddOfficesToOfficeClass(ViewState["OfficeKey"].ToString())
        )
      #endregion Conditions for Visible and Load
      {
        Table_Report_Edit_Other_Offices_At_Level.Visible = true;

        #region Offices Report Header
        LabelOfficesHeader.Text = Name_Office_Contest_And_Electoral_Plus_Offices(
          //Convert.ToUInt16(ViewState["OfficeClass"]
          //)
          Offices.GetOfficeClass(ViewState["OfficeKey"].ToString()
          )
          , ViewState["StateCode"].ToString()
          , ViewState["CountyCode"].ToString()
          , ViewState["LocalCode"].ToString()
          );
        #endregion Offices Report Header

        #region Offices Report

        #region StateCode of Office because ViewState["StateCode"] could be U1, U2 OR U3
        var stateCode = ViewState["StateCode"].ToString();
        if (StateCache.IsValidFederalCode(ViewState["StateCode"].ToString()))
          stateCode = Offices.GetStateCodeFromKey(ViewState["OfficeKey"].ToString());
        #endregion StateCode of Politician because ViewState["StateCode"] could be U1, u2 OR U3

        // Done on the fly now
        //db.Report_Offices_Update(
        //  db.Office_Class(ViewState["OfficeKey"].ToString())
        //  , StateCode
        //  , ViewState["CountyCode"].ToString()
        //  , ViewState["LocalCode"].ToString()
        //  );

        //HTMLTableOfficesInThisCategory.Text = db.Report_Offices_Get(
        //  Offices.GetOfficeClass(ViewState["OfficeKey"].ToString()).ToInt()
        //  , StateCode
        //  , ViewState["CountyCode"].ToString()
        //  , ViewState["LocalCode"].ToString()
        //  );

        var options = new OfficesAdminReportViewOptions
        {
          OfficeClass = Offices.GetOfficeClass(ViewState["OfficeKey"].ToString()),
          StateCode = stateCode,
          CountyCode = ViewState["CountyCode"].ToString(),
          LocalCode = ViewState["LocalCode"].ToString(),
          Option = OfficesAdminReportViewOption.ByLocal
        };
        HTMLTableOfficesInThisCategory.Text =
          OfficesAdminReport.GetReport(options).RenderToString();

        #endregion Offices Report
      }
      else
      {
        Table_Report_Edit_Other_Offices_At_Level.Visible = false;
      }
    }

    private void Controls_Visible_Load()
    {
      Table_Add_Office_Data_Visible_Load();
      Table_Add_Office_Visible();
      Table_Add_Office_And_In_Election_Visible();
      Table_Another_Office_Visible();
      Table_Edit_Office_Visible_Load();
      Table_Incumbent_Visible_Load();
      Table_Add_New_Politician_Without_Election_Visible_Load();
      //Table_Running_Mate_Visible();
      Table_Report_Add_Office_Anchors_Visible_Load();
      Table_Report_Edit_Other_Offices_At_Level_Visible_Load();

      if (IsSuperUser
        //only for bulk adding of County and Local Offices
        //&& (string.IsNullOrEmpty(ViewState["OfficeKey"].ToString()))
        //&& (db.QueryString("Mode") == "Bulk")
        && !string.IsNullOrEmpty(GetQueryString("Electoral")))
      {
        TableMasterOnly.Visible = true;
        ViewState["OfficeClass"] = OfficeClass.Undefined.ToInt();
        //if (db.QueryString("County") == "ALL")
        //{
        //  RadioButtonList_County_Classes.Visible = true;
        //  RadioButtonList_Local_Classes.Visible = false;
        //  if (RadioButtonList_County_Classes.SelectedIndex != -1)
        //    ViewState["OfficeClass"] = RadioButtonList_County_Classes.SelectedValue;
        //}
        //else
        //{
        //  RadioButtonList_County_Classes.Visible = false;
        //  RadioButtonList_Local_Classes.Visible = true;
        //  if (RadioButtonList_Local_Classes.SelectedIndex != -1)
        //    ViewState["OfficeClass"] = RadioButtonList_Local_Classes.SelectedValue;
        //}
        if (RadioButtonList_Office_Classes.SelectedIndex != -1)
          ViewState["OfficeClass"] = RadioButtonList_Office_Classes.SelectedValue;

      }
      else
        TableMasterOnly.Visible = false;
    }

    #endregion Visible Controls

    #region DB Utilities
    private int OfficeOrderWithinLevelNext()
    {
      var lastOfficeOnBallotRow = Row_Last_Optional(SqlOffices4Level());
      if (lastOfficeOnBallotRow == null)
        return 10;
      return Convert.ToInt32(lastOfficeOnBallotRow["OfficeOrderWithinLevel"]) + 10;
    }

    private string OfficeOrder()
    {
      if (TextBoxOfficeOrderOnBallot.Text.Trim() != string.Empty)
      {
        if (!Is_Valid_Integer(TextBoxOfficeOrderOnBallot.Text.Trim()))
          throw new ApplicationException(
            "The Ballot Order needs to be a whole number or left empty.");
        return TextBoxOfficeOrderOnBallot.Text.Trim();
      }
      return "10";
    }

    #endregion DB Utilities

    #region Checks

    private void Check_Is_Integer(TextBox textBox)
    {
      if (!Is_Valid_Integer(textBox.Text.Trim()))
        throw new ApplicationException(
          "The value entered needs to be a whole number.");
    }

    //private void Check_Politician_Exists(string politicianKey)
    //{
    //  if (!Politicians.IsValid(politicianKey))
    //    throw new ApplicationException(
    //      "The Politician ID: "
    //      + politicianKey
    //      + " entered in the text box is invalid.");
    //}

    private void Check_Both_Office_Title_And_BallotOrder_Not_Empty()
    {
      if (string.IsNullOrEmpty(TextBox_Office_Line_Add_1.Text.Trim())
       && string.IsNullOrEmpty(TextBox_Order.Text.Trim()))
        throw new ApplicationException("The 1st line of the office title and ballot order are empty.");
    }

    private void Check_Office_Title_Or_BallotOrder_Empty()
    {
      if (string.IsNullOrEmpty(TextBox_Office_Line_Add_1.Text.Trim()))
        throw new ApplicationException("The 1st line of the office title is empty.");
      if (string.IsNullOrEmpty(TextBox_Order.Text.Trim()))
        throw new ApplicationException("The ballot order is empty.");
    }

    private void Check_Search_And_OfficeTitle_Compatable()
    {
      if (
        TextBox_Office_Line_Add_1.Text.ToLower()
          .IndexOf(TextBox_Office_Title_Search.Text.Trim().ToLower(), StringComparison.Ordinal) ==
          -1 &&
          TextBox_Office_Line_Add_1.Text.ToLower()
            .IndexOf(TextBox_Office_Title_Search.Text.Trim().ToLower(), StringComparison.Ordinal) ==
            -1)
        throw new ApplicationException(
          "The part of the office title must be in one or the office title lines.");
    }

    #endregion Checks

    #region Edits
    private string EditIsRunningMateOffice()
    {
      return RadioButtonListRunningMateOffice.SelectedValue == "Yes"
        ? "1"
        : "0";
    }

    #endregion Edits

    #region Logs

    private void Offices_Update_Log_Str(string columnName, TextBox textBox)
    {
      //G.LogOfficeChange(
      //  ViewState["OfficeKey"].ToString()
      //  , columnName
      //  , G.Offices_Str(ViewState["OfficeKey"].ToString(), columnName)
      //  , textBox.Text.Trim()
      //  );

      Offices_Update_Str(ViewState["OfficeKey"].ToString(), columnName, textBox.Text.Trim());
    }

    private void Offices_Update_Log_Int(string columnName, TextBox textBox)
    {
      //G.LogOfficeChange(
      //  ViewState["OfficeKey"].ToString()
      //  , columnName
      //  , Offices_Int(ViewState["OfficeKey"].ToString(), columnName)
      //  , Convert.ToInt16(textBox.Text.Trim())
      //  );

      Offices_Update_Str(ViewState["OfficeKey"].ToString(), columnName, textBox.Text.Trim());

      Msg.Text = Ok("Office Data has been changed to: " + textBox.Text.Trim());
    }

    private void OfficesUpdateAndLogBool(string columnName, RadioButtonList radioButtonListTo)
    {
      //var fromBool = true;
      var toBool = radioButtonListTo.SelectedValue == "Yes";

      //LogOfficeChange(
      //  ViewState["OfficeKey"].ToString()
      //  , columnName
      //  , fromBool
      //  , toBool
      //  );

      Offices_Update_Bool(ViewState["OfficeKey"].ToString(), columnName, toBool);

      Msg.Text = Ok("Office Data has been changed to: " + toBool.ToString());
    }

    #endregion Logs

    #region Msg

    private string MsgCommonUpdateOffice(string information, TextBox textBox = null)
    {
      var msg = string.Empty;

      if (information != string.Empty)
      {
        if (textBox != null)
        {
          if (textBox.Text.Trim() == string.Empty)
            msg = information + " has been deleted.";
          else
            msg = information + " has been changed to: " + textBox.Text.Trim() + ".";
        }
        else
          msg = information + " has been changed.";
      }

      msg += " On this form you may "
      + " make additional changes to this office using the textboxes."
      + " You can also add, change or delete the incumbent for this office."
      + " Use the 'Add' links to add offices in different office categories."
      + " Use the office title links to edit information about a different office.";

      return msg;
    }

    //private string MsgCommonUpdateOffice()
    //{
    //  return MsgCommonUpdateOffice(string.Empty);
    //}
    #endregion Msg

    //private int IncumbentsOnFile()
    //{
    //  //return db.Rows(sql.Incumbents(ViewState["OfficeKey"].ToString()));
    //  return db.Rows(sql.OfficesOfficials_Select(ViewState["OfficeKey"].ToString()));
    //}

    private string Insert_Into_Offices(string stateCode, string countyCode,
      string localCode, string districtCode, string districtCodeAlpha,
      string officeLine1, string officeLine2, OfficeClass officeClass,
      string strOfficeOrderWithinLevel, string electionKey)
    {
      Throw_Exception_If_Html_Or_Script(officeLine1);
      Throw_Exception_If_Html_Or_Script(officeLine2);

      //if (!db.Is_Valid_Office_To_Add_Offices(
      // Convert.ToUInt16(Office_Class)
      // , StateCode))
      if (OfficesAllIdentified.GetIsOfficesAllIdentified(stateCode, 
        officeClass.ToInt(), countyCode, localCode))
        throw new ApplicationException(
          "Offices can not be added in this category of offices.");

      if (officeLine1 == string.Empty)
        throw new ApplicationException(
          "The first line of the office title is required");

      var officeKey = OfficeKey(officeClass.ToInt(), stateCode,
        countyCode, localCode, districtCode, districtCodeAlpha,
        Str_ReCase_Office_Title(officeLine1.Trim()),
        Str_ReCase_Office_Title(officeLine2.Trim()));

      //Check_Office_Not_Already_Exists(OfficeKey);
      if (Offices.OfficeKeyExists(officeKey))
        throw new ApplicationException(Offices.GetLocalizedOfficeName(officeKey) +
          " Office with Office OfficeKey: (" + officeKey + ")" + " already exists.");

      int officeOrderWithinLevel;
      if (strOfficeOrderWithinLevel != string.Empty)
      {
        #region  ballot passed

        if (!Is_Valid_Integer(strOfficeOrderWithinLevel.Trim()))
          throw new ApplicationException("The Ballot order is not an integer");
        officeOrderWithinLevel = Convert.ToInt16(strOfficeOrderWithinLevel.Trim());

        #endregion  ballot passed
      }
      else
        #region No ballot order passed

        officeOrderWithinLevel =
          OfficeOrderWithinLevelNext();
        #endregion No ballot order passed

      Offices.Insert(officeKey, stateCode, countyCode, localCode, districtCode,
        districtCodeAlpha, Str_ReCase_Office_Title(officeLine1.Trim()),
        Str_ReCase_Office_Title(officeLine2.Trim()), officeClass.ToInt(), 0,
        officeOrderWithinLevel, false, false, 1, string.Empty,
        "(Vote for no more than one)", string.Empty, "Write in", 1, /*string.Empty,
        string.Empty, string.Empty, string.Empty, string.Empty, DateTime.MinValue,*/
        false, DateTime.Now, false, false, false, 1, 1, 0, 0, false);

      #region Adding Office in the election

      if (!string.IsNullOrEmpty(electionKey))
        if (Offices.IsInElection(officeKey, electionKey))
          throw new ApplicationException(Offices.FormatOfficeName(officeKey) +
            " Office with Office Id: (" + officeKey + ")" +
            " already exists for this election and has been deleted. Please try to add again.");

      #endregion Adding Office in the election

      //db.Invalidate_Office(OfficeKey);

      return officeKey;
    }

    #region TextBoxs Change
    protected void TextBoxOfficeLine1_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Line_Edit_1);

        if (Offices_Str(ViewState["OfficeKey"].ToString(), "OfficeLine1") != TextBox_Office_Line_Edit_1.Text.Trim())
        {
          //TextBox_Office_Line_Edit_1.Text = db.Str_ReCase_Office_Title(TextBox_Office_Line_Edit_1.Text);

          Offices_Update_Log_Str("OfficeLine1", TextBox_Office_Line_Edit_1);

          Msg.Text = Ok(MsgCommonUpdateOffice("First line of the office title", TextBox_Office_Line_Edit_1));

          Page_Title();

          //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

          Controls_Visible_Load();
        }

      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxOfficeLine2_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Line_Edit_2);

        if (Offices_Str(ViewState["OfficeKey"].ToString(), "OfficeLine2") != TextBox_Office_Line_Edit_2.Text.Trim())
        {
          //TextBox_Office_Line_Edit_2.Text = db.Str_ReCase_Office_Title(TextBox_Office_Line_Edit_2.Text);

          Offices_Update_Log_Str("OfficeLine2", TextBox_Office_Line_Edit_2);

          Msg.Text = Ok(MsgCommonUpdateOffice("Second line of office title", TextBox_Office_Line_Edit_2));

          Page_Title();

          //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

          Controls_Visible_Load();
        }

      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxVoteInstructions_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_Vote_Instructions);

        if (Offices_Str(ViewState["OfficeKey"].ToString(), "VoteInstructions") != TextBox_Vote_Instructions.Text.Trim())
        {
          Offices_Update_Log_Str("VoteInstructions", TextBox_Vote_Instructions);

          //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

          Msg.Text = Ok(MsgCommonUpdateOffice("Vote instructions", TextBox_Vote_Instructions));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxVoteForWording_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_Vote_Wording);

        if (Offices_Str(ViewState["OfficeKey"].ToString(), "VoteForWording") != TextBox_Vote_Wording.Text.Trim())
        {
          Offices_Update_Log_Str("VoteForWording", TextBox_Vote_Wording);

          //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

          Msg.Text = Ok(MsgCommonUpdateOffice("Vote for wording", TextBox_Vote_Wording));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxWriteInInstructions_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_WriteIn_Instructions);

        if (Offices_Str(ViewState["OfficeKey"].ToString(), "WriteInInstructions") != TextBox_WriteIn_Instructions.Text.Trim())
        {
          Offices_Update_Log_Str("WriteInInstructions", TextBox_WriteIn_Instructions);

          Msg.Text = Ok(MsgCommonUpdateOffice("Write in instructions", TextBox_WriteIn_Instructions));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxWriteInWording_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_WriteIn_Wording);

        if (Offices_Str(ViewState["OfficeKey"].ToString(), "WriteInWording") != TextBox_WriteIn_Wording.Text.Trim())
        {
          Offices_Update_Log_Str("WriteInWording", TextBox_WriteIn_Wording);

          //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

          Msg.Text = Ok(MsgCommonUpdateOffice("Write in wording", TextBox_WriteIn_Wording));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxOfficeOrderOnBallot_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBoxOfficeOrderOnBallot);

        Check_Is_Integer(TextBoxOfficeOrderOnBallot);

        if (Offices_Int(ViewState["OfficeKey"].ToString(), "OfficeOrderWithinLevel") != Convert.ToUInt16(TextBoxOfficeOrderOnBallot.Text.Trim()))
        {
          Offices_Update_Log_Int("OfficeOrderWithinLevel", TextBoxOfficeOrderOnBallot);

          //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

          // 10/29/2013 JCL test
          //Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxWriteInLines_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBoxWriteInLines);

        Check_Is_Integer(TextBoxWriteInLines);

        //CheckWriteInLines0To5();
        if (!Is_Valid_Integer(TextBoxWriteInLines.Text.Trim()))
          throw new ApplicationException(
            "The Write in Lines needs to be a whole number between 0 and 5.");
        int lines = Convert.ToUInt16(TextBoxWriteInLines.Text.Trim());
        if ((lines < 0) | (lines > 5))
          throw new ApplicationException(
            "The Write in Lines needs to be a number between 0 and 5.");

        if (Offices_Int(ViewState["OfficeKey"].ToString(), "WriteInLines") != Convert.ToUInt16(TextBoxWriteInLines.Text.Trim()))
        {
          Offices_Update_Log_Int("WriteInLines", TextBoxWriteInLines);

          //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

          Msg.Text = Ok(MsgCommonUpdateOffice("Write in lines", TextBoxWriteInLines));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxOfficePositions_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBoxOfficePositions);

        Check_Is_Integer(TextBoxOfficePositions);
        int positions = Convert.ToUInt16(TextBoxOfficePositions.Text.Trim());
        //if ((Positions < 1) | (Positions > 2))
        //  throw new ApplicationException("Office positions needs to be 1 or 2 ony.");
        if (positions < 1)
          throw new ApplicationException(
            "Office positions needs to be 1 or more.");

        if (Office_Positions(ViewState["OfficeKey"].ToString()) !=
          Convert.ToUInt16(TextBoxOfficePositions.Text.Trim()))
        {
          Offices_Update_Log_Int("Incumbents", TextBoxOfficePositions);

          if (positions > 1)
            Msg.Text = Warn(
              "You changed the number of incumbents to " + TextBoxOfficePositions.Text
              + " which is greater than the nomal 1.");
          else

            Msg.Text = Ok(MsgCommonUpdateOffice("The number of incumbents", TextBoxOfficePositions));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxElectionPositions_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBoxElectionPositions);

        Check_Is_Integer(TextBoxElectionPositions);
        int positions = Convert.ToUInt16(TextBoxElectionPositions.Text.Trim());
        if (positions < 1)
          throw new ApplicationException(
            "Election positions needs to be 1 or more.");

        if (Offices_Int(ViewState["OfficeKey"].ToString(), "ElectionPositions") !=
          Convert.ToUInt16(TextBoxElectionPositions.Text.Trim()))
        {
          Offices_Update_Log_Int("ElectionPositions", TextBoxElectionPositions);

          if (positions > 1)
            Msg.Text = Warn(
              "You changed the number of elected positions to " + TextBoxElectionPositions.Text
              + " which is greater than the nomal 1.");
          else

            Msg.Text = Ok(MsgCommonUpdateOffice("The number of elected positions was set to", TextBoxElectionPositions));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxPrimaryPositions_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBoxPrimaryPositions);

        Check_Is_Integer(TextBoxPrimaryPositions);
        int positions = Convert.ToUInt16(TextBoxPrimaryPositions.Text.Trim());
        if (positions < 1)
          throw new ApplicationException(
            "Primary positions needs to be 1 or more.");

        if (Offices_Int(ViewState["OfficeKey"].ToString(), "PrimaryPositions") !=
          Convert.ToUInt16(TextBoxPrimaryPositions.Text.Trim()))
        {
          Offices_Update_Log_Int("PrimaryPositions", TextBoxPrimaryPositions);

          if (positions > 1)
            Msg.Text = Warn(
              "You changed the number of primary positions to " + TextBoxPrimaryPositions.Text
              + " which is greater than the normal 1.");
          else

            Msg.Text = Ok(MsgCommonUpdateOffice("The number of primary positions was set to", TextBoxPrimaryPositions));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxPrimaryRunoffPositions_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBoxPrimaryRunoffPositions);

        Check_Is_Integer(TextBoxPrimaryRunoffPositions);
        int positions = Convert.ToInt16(TextBoxPrimaryRunoffPositions.Text.Trim());
        if (positions == 1 || positions < -1)
          throw new ApplicationException(
            "Primary runoff positions must be -1, 0 or greater than 1");

        if (Offices_Int(ViewState["OfficeKey"].ToString(), "PrimaryRunoffPositions") !=  positions)
        {
          Offices_Update_Log_Int("PrimaryRunoffPositions", TextBoxPrimaryRunoffPositions);

            Msg.Text = Ok(MsgCommonUpdateOffice("The number of primary runoff positions was set to", TextBoxPrimaryRunoffPositions));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void TextBoxGeneralRunoffPositions_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBoxGeneralRunoffPositions);

        Check_Is_Integer(TextBoxGeneralRunoffPositions);
        int positions = Convert.ToInt16(TextBoxGeneralRunoffPositions.Text.Trim());
        if (positions == 1 || positions < -1)
          throw new ApplicationException(
            "General runoff positions must be -1, 0 or greater than 1");

        if (Offices_Int(ViewState["OfficeKey"].ToString(), "GeneralRunoffPositions") !=  positions)
        {
          Offices_Update_Log_Int("GeneralRunoffPositions", TextBoxGeneralRunoffPositions);

            Msg.Text = Ok(MsgCommonUpdateOffice("The number of general runoff positions was set to", TextBoxGeneralRunoffPositions));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    //protected void TextBoxRemoveIncumbent_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBox_Remove_Incumbent);

    //    #region Check if politician one of the incumbents
    //    if (TextBox_Remove_Incumbent.Visible)
    //    {
    //      Check_Politician_Exists(TextBox_Remove_Incumbent.Text.Trim());

    //      if (!db.Is_Valid_Office_Politician(
    //        ViewState["OfficeKey"].ToString()
    //        , TextBox_Remove_Incumbent.Text.Trim())
    //        )
    //        throw new ApplicationException(
    //          "The Politician ID is not an incumbent for this office and was not be removed.");
    //    }
    //    #endregion

    //    #region PoliticianKey to Remove
    //    string politicianKey;
    //    if (IncumbentsOnFile() == 1)//use only Politician ID for this office
    //    {
    //      var politicianRow = db.Row_First(sql.OfficesOfficials_Select(ViewState["OfficeKey"].ToString()));
    //      politicianKey = politicianRow["PoliticianKey"].ToString();
    //    }
    //    else //use ID provided by user in text box
    //    {
    //      politicianKey = TextBox_Remove_Incumbent.Text.Trim();
    //    }
    //    //db.Invalidate_Politician(PoliticianKey);
    //    #endregion

    //    db.Log_OfficesOfficials_Change(
    //      politicianKey
    //      , ViewState["OfficeKey"].ToString()
    //      , ViewState["StateCode"].ToString()
    //      , "PoliticianKey"
    //      , politicianKey
    //      , string.Empty
    //      //, db.User_Security()
    //      //, db.User_Name()
    //      );

    //    db.OfficesOfficials_Delete(
    //       ViewState["OfficeKey"].ToString()
    //      , politicianKey
    //      //, db.User_Security()
    //      //, db.User_Name()
    //      );

    //    //db.Invalidate_Office_Incumbent(ViewState["OfficeKey"].ToString());
    //    //db.Invalidate_Politician(PoliticianKey);

    //    Msg.Text = db.Ok(Politicians.GetFormattedName(politicianKey)
    //      + " was REMOVED as the currently elected official for this office."
    //      + MsgCommonUpdateOffice());

    //    Controls_Visible_Load();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxSelectIncumbent_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    #region checks
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxSelectIncumbent);

    //    Check_Politician_Exists(TextBoxSelectIncumbent.Text.Trim());

    //    if (ViewState["OfficeKey"].ToString() == string.Empty)
    //      throw new ApplicationException(
    //        "You need to enter the 1st line of the Office Title to create an office before you can identify the incumbent.");

    //    if (db.Is_Valid_Office_Politician(
    //      ViewState["OfficeKey"].ToString()
    //      , TextBoxSelectIncumbent.Text.Trim())
    //      )
    //      throw new ApplicationException(
    //        "The Politician ID IS ALREADY an incumbent for this office and can not be selected a second time.");

    //    #region can not exceed the number of incumbents for this office
    //    var officeIncumbents = db.Office_Positions(ViewState["OfficeKey"].ToString());
    //    var officesOfficialsRows = db.Rows("OfficesOfficials"
    //      , "OfficeKey", ViewState["OfficeKey"].ToString());
    //    //if (OfficesOfficialsRows + 1 > OfficeIncumbents)
    //    if (officesOfficialsRows + 1 > officeIncumbents)
    //      throw new ApplicationException("The " + officeIncumbents
    //        + " Incumbent for this office is already identified."
    //        + " Please Remove an incumbent in the list above and try again.");
    //    #endregion
    //    #endregion checks

    //    db.Log_OfficesOfficials_Change(
    //      TextBoxSelectIncumbent.Text.Trim()
    //      , ViewState["OfficeKey"].ToString()
    //      , ViewState["StateCode"].ToString()
    //      , "PoliticianKey"
    //      , string.Empty
    //      , TextBoxSelectIncumbent.Text.Trim()
    //      //, db.User_Security()
    //      //, db.User_Name()
    //      );


    //    db.OfficesOfficials_INSERT(
    //      ViewState["OfficeKey"].ToString()
    //      , TextBoxSelectIncumbent.Text.Trim()
    //      //, db.User_Security()
    //      //, db.User_Name()
    //      );

    //    //associate politician with this office as incumbent or seeking office
    //    db.Politicians_Update_Str(
    //      TextBoxSelectIncumbent.Text.Trim()
    //      //, "OfficeKey"
    //      , "TemporaryOfficeKey"
    //      , ViewState["OfficeKey"].ToString()
    //      );

    //    //db.Invalidate_Office(ViewState["OfficeKey"].ToString());
    //    //db.Invalidate_Politician(TextBoxSelectIncumbent.Text.Trim());

    //    Msg.Text = db.Ok(Politicians.GetFormattedName(TextBoxSelectIncumbent.Text.Trim())
    //      + " was IDENTIFIED as the currently elected official for this office."
    //      + MsgCommonUpdateOffice());

    //    Controls_Visible_Load();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxRunningMate_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBox_RunningMate);

    //    Check_Politician_Exists(TextBox_RunningMate.Text.Trim());

    //    if (Offices.IsRunningMateOffice(ViewState["OfficeKey"].ToString()))
    //    {
    //      #region office has a running mate
    //      //the main Politician for office like Governor
    //      var politicianKey = db.OfficesOfficials_Str(ViewState["OfficeKey"].ToString(), "politicianKey");
    //      if (politicianKey != string.Empty)
    //      {
    //        db.OfficesOfficialsUpdate(ViewState["OfficeKey"].ToString()
    //        , politicianKey//the main Politician for office like Governor
    //        , "RunningMateKey"
    //        , TextBox_RunningMate.Text.Trim());//the Running Mate like Lt. Governor
    //        //db.Invalidate_Politician(TextBox_RunningMate.Text.Trim());

    //        //db.Invalidate_Office(ViewState["OfficeKey"].ToString());
    //        //db.Invalidate_Politician(politicianKey);

    //        Msg.Text = db.Ok(Politicians.GetFormattedName(TextBox_RunningMate.Text.Trim())
    //          + " was recorded as the currently elected running mate for "
    //          + Politicians.GetFormattedName(politicianKey));

    //        Controls_Visible_Load();
    //      }
    //      else
    //      {
    //        throw new ApplicationException(
    //          "The main Politician holding this office, like President or Governor,"
    //        + " has not yet been identified.");
    //      }
    //      #endregion office has a running mate
    //    }
    //    else
    //    {
    //      Msg.Text = db.Msg("This is not a Running Mate Office");
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}
    #endregion TextBoxs Change

    #region Radio Button Changes

    protected void RadioButtonListRunningMateOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        var isRunningMateOffice = 
          RadioButtonListRunningMateOffice.SelectedValue == "Yes";

        if (Offices_Bool(ViewState["OfficeKey"].ToString(), "IsRunningMateOffice")
          != isRunningMateOffice)
        {
          OfficesUpdateAndLogBool(
            "IsRunningMateOffice"
            , RadioButtonListRunningMateOffice
            );
          //Visible_Table_Running_Mate();

          //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

          Msg.Text = Ok(MsgCommonUpdateOffice("The office running mate status"));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void RadioButtonListIsOnlyForPrimaries_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        var isOnlyForPrimaries = 
          RadioButtonListIsOnlyForPrimaries.SelectedValue == "Yes";

        if (Offices_Bool(ViewState["OfficeKey"].ToString(), "IsOnlyForPrimaries")
          != isOnlyForPrimaries)
        {
          OfficesUpdateAndLogBool(
            "IsOnlyForPrimaries"
            , RadioButtonListIsOnlyForPrimaries
            );

          //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

          Msg.Text = Ok(MsgCommonUpdateOffice("The office only for primaries status"));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }

    }
    protected void RadioButtonListActive_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        var isInactive = RadioButtonListActive.SelectedValue == "Yes";

        if (Offices_Bool(ViewState["OfficeKey"].ToString(), "IsInactive")
          != isInactive)
        {
          OfficesUpdateAndLogBool(
            "IsInactive"
            , RadioButtonListActive
            );

          //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

          Msg.Text = Ok(MsgCommonUpdateOffice("The office active status"));

          Controls_Visible_Load();
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }
    #endregion Radio Button Changes


    protected void Button_Add_Office_Click(object sender, EventArgs e)
    {
      try
      {
        Check_TextBoxs_Illeagal_Input();

        var districtCode = ViewState["DistrictCode"].ToString();

        var isHasDistrictNumber = false;
        if (
          ViewState["OfficeClass"].ToOfficeClass() == OfficeClass.USHouse
          || ViewState["OfficeClass"].ToOfficeClass() == OfficeClass.StateSenate
          || ViewState["OfficeClass"].ToOfficeClass() == OfficeClass.StateHouse
          )
        {
          if (!Is_Valid_Integer(TextBoxDistrict.Text.Trim()))
            throw new ApplicationException("The District Number is not an integer.");
          isHasDistrictNumber = true;
          TextBoxDistrict.Enabled = true;
        }
        else
          TextBoxDistrict.Enabled = true;


        if ((!string.IsNullOrEmpty(TextBoxDistrict.Text.Trim()))
          && (isHasDistrictNumber)
          )
          districtCode = TextBoxDistrict.Text.Trim();

        //Checks done in Insert_Into_Offices
        ViewState["OfficeKey"] = Insert_Into_Offices(
          ViewState["StateCode"].ToString()
          , ViewState["CountyCode"].ToString()
          , ViewState["LocalCode"].ToString()
          //, ViewState["DistrictCode"].ToString()
          , districtCode
          , ViewState["DistrictCodeAlpha"].ToString()
          , TextBox_Office_Line_Add_1.Text.Trim()
          , TextBox_Office_Line_Add_2.Text.Trim()
          , ViewState["OfficeClass"].ToOfficeClass()
          , TextBoxOfficeOrderOnBallot.Text.Trim()
          , string.Empty
          );

        //db.Invalidate_Office(ViewState["OfficeKey"].ToString());

        #region Msg.Text
        Msg.Text = Ok(TextBox_Office_Line_Add_1.Text + " " + TextBox_Office_Line_Add_2.Text
          + " was ADDED."
          + " You may now edit or complete the information about this office on ballots."
          + " Click on Add Another Office to continue adding offices for this office type."
          + " Use the other sections as described.");
        #endregion

        Controls_Visible_Load();
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void Button_Add_Office_And_In_Election_Click1(object sender, EventArgs e)
    {
      try
      {
        Check_TextBoxs_Illeagal_Input();

        //Checks done in Insert_Into_Offices
        ViewState["OfficeKey"] = Insert_Into_Offices(
          ViewState["StateCode"].ToString()
          , ViewState["CountyCode"].ToString()
          , ViewState["LocalCode"].ToString()
          , ViewState["DistrictCode"].ToString()
          , ViewState["DistrictCodeAlpha"].ToString()
          , TextBox_Office_Line_Add_1.Text.Trim()
          , TextBox_Office_Line_Add_2.Text.Trim()
          , ViewState["OfficeClass"].ToOfficeClass()
          , TextBoxOfficeOrderOnBallot.Text.Trim()
          , string.Empty
          );

        //db.Invalidate_Office(ViewState["OfficeKey"].ToString());


        #region Add ElectionsOffices for any upcoming election so it will appear on list of offices to select
        //Needed so that offices can be created after an election is created
        //if (
        //  (Convert.ToInt16(ViewState["OfficeClass"]) < db.Office_County_Executive)
        //  || (Convert.ToInt16(ViewState["OfficeClass"]) == db.Judicial)//Judicial
        //  )//Statewide Offices (State and Federal Elections)
        var upcomingElectionsTable = G.Table(ViewState["CountyCode"].ToString() == string.Empty
          ? ElectionsUpcoming(ViewState["StateCode"].ToString())
          : ElectionsUpcomingCountyLocal(ViewState["StateCode"].ToString(), ViewState["CountyCode"].ToString()));
        if (upcomingElectionsTable.Rows.Count > 0)
        {
          //string ElectionKey_Federal = string.Empty;
          foreach (DataRow upcomingElectionRow in upcomingElectionsTable.Rows)
          {
            //if (db.Row_Optional(sql.ElectionsOffices4ElectionKeyOfficeKey(
            //  ViewState["ElectionKey"].ToString(), ViewState["OfficeKey"].ToString())) == null)
            //{
            #region Add ElectionsOffices Row if not exist
            #region Add ElectionsOffices for Election
            #region replaced
            //ElectionKey_Federal = db.ElectionKey_Federal(UpcomingElectionRow["ElectionKey"].ToString(), ViewState["OfficeKey"].ToString());

            //Add an ElectionsOffices row
            //string SQLINSERT = "INSERT INTO ElectionsOffices "
            //  + "("
            //  + "ElectionKey"
            //  + ",ElectionKeyState"
            //  + ",ElectionKeyFederal"
            //  + ",OfficeKey"
            //  + ",OfficeLevel"
            //  + ",StateCode"
            //  + ",DistrictCode"
            //  + ",CountyCode"
            //  + ",LocalCode"
            //  + ")"
            //  + " VALUES"
            //  + "("
            //  + db.SQLLit(UpcomingElectionRow["ElectionKey"].ToString())
            //  + db.SQLLit(db.ElectionKey_State(UpcomingElectionRow["ElectionKey"].ToString()))
            //  + "," + db.SQLLit(ElectionKey_Federal)
            //  + "," + db.SQLLit(ViewState["OfficeKey"].ToString())
            //  + "," + db.SQLLit(ViewState["OfficeClass"].ToString())
            //  + "," + db.SQLLit(UpcomingElectionRow["StateCode"].ToString())
            //  + "," + db.SQLLit(ViewState["DistrictCode"].ToString())
            //  + "," + db.SQLLit(ViewState["CountyCode"].ToString())
            //  + "," + db.SQLLit(ViewState["LocalCode"].ToString())
            //  + ")";

            //db.ExecuteSQL(SQLINSERT);
            #endregion replaced

            ElectionsOffices_INSERT(
              upcomingElectionRow["ElectionKey"].ToString()
            , ViewState["OfficeKey"].ToString()
            , ViewState["DistrictCode"].ToString()
            );


            //db.Invalidate_Election(ViewState["ElectionKey"].ToString());

            #endregion

            #region LogOfficeAddsDeletes
            LogOfficeAddsDeletes.Insert(
              DateTime.Now,
              "A",
              UserSecurityClass,
              UserName,
              ViewState["OfficeKey"].ToString(),
              ViewState["StateCode"].ToString(),
              int.Parse(ViewState["OfficeClass"].ToString()),
              int.Parse(OfficeOrder()),
              ViewState["DistrictCode"].ToString(),
              string.Empty,
              ViewState["CountyCode"].ToString(),
              ViewState["LocalCode"].ToString(),
              TextBox_Office_Line_Edit_1.Text.Trim(),
              TextBox_Office_Line_Edit_2.Text.Trim(),
              false,
              EditIsRunningMateOffice() == "1",
              1,
              string.Empty,
              string.Empty,
              string.Empty,
              string.Empty,
              1);
            //string InsertSQL = "INSERT INTO LogOfficeAddsDeletes "
            //  + "("
            //  + "DateStamp"
            //  + ",AddOrDelete"
            //  + ",UserSecurity"
            //  + ",UserName"
            //  + ",OfficeKey"
            //  + ",StateCode"
            //  + ",OfficeLevel"
            //  + ",OfficeOrderWithinLevel"
            //  + ",DistrictCode"
            //  + ",DistrictCodeAlpha"
            //  + ",CountyCode"
            //  + ",LocalCode"
            //  + ",OfficeLine1"
            //  + ",OfficeLine2"
            //  + ",IsRunningMateOffice"
            //  //+ ",Incumbents"
            //  //+ ",VoteInstructions"
            //  //+ ",VoteForWording"
            //  //+ ",WriteInInstructions"
            //  //+ ",WriteInWording"
            //  //+ ",WriteInLines"
            //  + ")"
            //  + " VALUES"
            //  + "("
            //  + db.SQLLit(Db.DbNow)
            //  + ",'A'"
            //  + "," + db.SQLLit(db.User_Security())
            //  + "," + db.SQLLit(db.User_Name())
            //  + "," + db.SQLLit(ViewState["OfficeKey"].ToString())
            //  + "," + db.SQLLit(ViewState["StateCode"].ToString())
            //  + "," + ViewState["OfficeClass"].ToString()
            //  + "," + OfficeOrder()
            //  + "," + db.SQLLit(ViewState["DistrictCode"].ToString())
            //  + ",''" //DistrictCodeAlpha
            //  + "," + db.SQLLit(ViewState["CountyCode"].ToString())
            //  + "," + db.SQLLit(ViewState["LocalCode"].ToString())
            //  + "," + db.SQLLit(TextBox_Office_Line_Edit_1.Text.Trim())//Intensionally not recased
            //  + "," + db.SQLLit(TextBox_Office_Line_Edit_2.Text.Trim())
            //  + "," + EditIsRunningMateOffice()
            //  //+ "," + xEditIncumbents()
            //  //+ "," + db.SQLLit(xEditVoteInstructions())
            //  //+ "," + db.SQLLit(xEditVoteForWording())
            //  //+ "," + db.SQLLit(xEditWriteInInstructions())
            //  //+ "," + db.SQLLit(xEditWriteInWording())
            //  //+ "," + xEditWriteInLines()
            //  + ")";
            //db.ExecuteSQL(InsertSQL);
            #endregion
            #endregion
            //}
          }
        }
        #endregion Add ElectionsOffices for any upcoming election so it will appear on list of offices to select

        #region Msg.Text
        Msg.Text = Ok(TextBox_Office_Line_Edit_1.Text + " " + TextBox_Office_Line_Edit_2.Text
          + ": was ADDED at position " + OfficeOrder()
          + " for " + Name_Office_Contest_And_Electoral_Plus_Offices(
              TextBoxOfficeOrderOnBallot.Text.Trim().ToOfficeClass()
              , ViewState["StateCode"].ToString()
              , ViewState["CountyCode"].ToString()
              , ViewState["LocalCode"].ToString()
              )
          + " Offices. "
          + " The data recorded for the office is shown in red."
          + " You can now identify the incumbent or current elected official for this office."
          + " Or you can add another office, make additional changes "
          + " or make changes to one of the offices in the Table below.");
        #endregion

        Controls_Visible_Load();
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }

    }

    //protected void Button_Remove_RunningMate_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Check_TextBoxs_Illeagal_Input();
        
    //    if (!string.IsNullOrEmpty(TextBox_RunningMate.Text))
    //    {
    //      if (!Politicians.IsValid(TextBox_RunningMate.Text.Trim()))
    //        throw new ApplicationException("The politician ID is invalid");

    //      db.OfficesOfficialsUpdate(
    //        ViewState["OfficeKey"].ToString()
    //        //, TextBox_RunningMate.Text.Trim()
    //        , db.OfficesOfficials_Str(
    //          ViewState["OfficeKey"].ToString()
    //          , "PoliticianKey"
    //          )
    //        , "RunningMateKey"
    //        , string.Empty
    //        );

    //      Controls_Visible_Load();

    //      Msg.Text = db.Ok("The elected official as the running mate has been removed.");
    //    }
    //    else
    //    {
    //      Msg.Text = db.Fail("The is no elected official running mate currently identified"
    //      + " and so none was removed.");
    //    }
    //  }

    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }

    //}
    protected void TextBox_Order_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_Order);

        if (!string.IsNullOrEmpty(TextBox_Order.Text.Trim()))
        {
          if (!Is_Valid_Integer(TextBox_Order.Text.Trim()))
            throw new ApplicationException(
              "The Ballot Order is not an integer.");

          Msg.Text = Ok("Ballot Order is ok.");
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    private DataTable Counties_Or_Locals_Table()
    {
      string sql;
      if (Is_Electoral_Class_County(Convert.ToInt16(ViewState["OfficeClass"])))
      {
        #region Counties
        sql = "SELECT CountyCode FROM Counties WHERE StateCode = "
          + SqlLit(ViewState["StateCode"].ToString());
        return G.Table(sql);
        #endregion Counties
      }

      #region Local Districts
      sql = "SELECT CountyCode,LocalCode FROM LocalDistricts WHERE StateCode = "
        + SqlLit(ViewState["StateCode"].ToString());
      return G.Table(sql);
      #endregion Local Districts
    }

    private DataTable Offices_Table(
      string stateCode
      , string countyCode
      , string localCode
      , int officeClass
        )
    {
      var sql = string.Empty;
      sql += " SELECT OfficeKey,CountyCode,LocalCode,OfficeLine1,OfficeLine2,OfficeOrderWithinLevel";
      sql += " FROM vote.Offices";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " AND CountyCode = " + SqlLit(countyCode);
      sql += " AND LocalCode = " + SqlLit(localCode);
      sql += " AND OfficeLevel = " + officeClass;
      sql += " ORDER BY CountyCode,LocalCode";
      return G.Table(sql);
    }

    private bool Is_Office_Exist(string officeLine1, string officeLine2)
    {
      return officeLine1.ToLower().IndexOf(TextBox_Office_Title_Search.Text.Trim().ToLower(), StringComparison.Ordinal) != -1
        || officeLine2.ToLower().IndexOf(TextBox_Office_Title_Search.Text.Trim().ToLower(), StringComparison.Ordinal) != -1;
    }

    private bool Is_Office_In_Table(
      string stateCode
      , string countyCode
      , string localCode
      , ref string officeKey
      , ref string officeLine1
      , ref string officeLine2
      , int officeClass
      , ref int officeOrderWithinLevel
      )
    {
      var isOfficeAlreadyExist = false;
      if (CheckBox_SkipCheck.Checked)
      {
        var tableCountyOrLocalOffices = Offices_Table(
         stateCode
         , countyCode
         , localCode
         , officeClass
         );
        foreach (DataRow rowOffice in tableCountyOrLocalOffices.Rows)
        {
          if (Is_Office_Exist(
            rowOffice["OfficeLine1"].ToString()
            , rowOffice["OfficeLine2"].ToString())
            )
          {
            isOfficeAlreadyExist = true;
            officeKey = rowOffice["OfficeKey"].ToString();
            officeLine1 = rowOffice["OfficeLine1"].ToString();
            officeLine2 = rowOffice["OfficeLine2"].ToString();
            officeOrderWithinLevel = Convert.ToInt16(rowOffice["OfficeOrderWithinLevel"].ToString());
          }
        }
      }
      return isOfficeAlreadyExist;
    }

    private void Report_Office_With_Title(
      string officeKey
      , string officeLine1
      , string officeLine2
      , string countyCode
      , string localCode
      , int officeOrderWithinLevel
      , string countyCodeOld
      )
    {
      #region local offices break between counties
      if (
       Is_Electoral_Class_Local(Convert.ToInt16(ViewState["OfficeClass"]))
        && String.Compare(countyCode, countyCodeOld, StringComparison.Ordinal) != 0
       )
        Label_Bulk_Offices_Report.Text += "<br>";
      #endregion local offices break between counties

      Label_Bulk_Offices_Report.Text += "<br>";
      Label_Bulk_Offices_Report.Text += officeLine1;
      //Label_Bulk_Offices_Report.Text += db.Anchor_Admin_Office_UPDATE_Office(OfficeKey, OfficeLine1, "_edit2");
      if (string.IsNullOrEmpty(officeLine2))
        officeLine2 = "_";
      Label_Bulk_Offices_Report.Text += " | " + officeLine2;
      Label_Bulk_Offices_Report.Text += " - "
        + CountyCache.GetCountyName(ViewState["StateCode"].ToString(), countyCode);
      if (Is_Electoral_Class_Local(Convert.ToInt16(ViewState["OfficeClass"])))
        Label_Bulk_Offices_Report.Text += " - "
        + GetPageCache().LocalDistricts.GetLocalDistrict(ViewState["StateCode"].ToString()
          , countyCode, localCode);
      Label_Bulk_Offices_Report.Text += " (" + officeOrderWithinLevel + ")";
      Label_Bulk_Offices_Report.Text += " [" + countyCode + "]";
      if (Is_Electoral_Class_Local(Convert.ToInt16(ViewState["OfficeClass"])))
        Label_Bulk_Offices_Report.Text += " [" + localCode + "]";
      Label_Bulk_Offices_Report.Text += " - " + officeKey;
      //+db.Anchor_Admin_Office_UPDATE_Office(OfficeKey, OfficeKey, "_edit2");
      //Anchor_Admin_Office_DELETE
      //Label_Bulk_Offices_Report.Text += " - " 
      //  + db.Anchor_Admin_Office_DELETE(
      //    Convert.ToInt16(ViewState["OfficeClass"])
      //    ,OfficeKey
      //    ,"_self"
      //    );
      Label_Bulk_Offices_Report.Text += " - [";
      Label_Bulk_Offices_Report.Text += Anchor_Admin_Office_UPDATE_Office(officeKey, "edit", "edit2") + "]";
    }

    private void Report_Search_Results()
    {
      if (string.IsNullOrEmpty(TextBox_Office_Title_Search.Text.Trim()))
        throw new ApplicationException("Part of Office Titles to Search is empty.");
      if (ViewState["OfficeClass"].ToOfficeClass() == OfficeClass.Undefined)
        throw new ApplicationException("An office class needs to be selected from the radio button list");

      #region Inits

      var countCountiesOrLocals = 0;
      var countOfficesWithTitle = 0;
      var countOfficesWithoutTitle = 0;
      Label_Bulk_Offices_Report.Text = string.Empty;
      var countyCodeOld = string.Empty;

      #endregion Inits

      #region Offices WITH Part in Office Title
      Label_Bulk_Offices_Report.Text += "<strong>Offices WITH Search Part in Office Title (Ballot Order) [CountyCode]</strong><br>";
      var tableCountiesOrLocals = Counties_Or_Locals_Table();
      foreach (DataRow rowCountyOrLocal in tableCountiesOrLocals.Rows)
      {
        countCountiesOrLocals++;

        #region A County or Local District

        #region Inits
        var officeKey = string.Empty;
        var officeLine1 = string.Empty;
        var officeLine2 = string.Empty;
        var officeOrderWithinLevel = 0;

        #endregion Inits

        var localCode = string.Empty;
        if (Is_Electoral_Class_Local(Convert.ToInt16(ViewState["OfficeClass"])))
          localCode = rowCountyOrLocal["LocalCode"].ToString();

        if (Is_Office_In_Table(
          ViewState["StateCode"].ToString()
          , rowCountyOrLocal["CountyCode"].ToString()
          , localCode
          , ref officeKey
          , ref officeLine1
          , ref officeLine2
          , Convert.ToInt16(ViewState["OfficeClass"])
          , ref officeOrderWithinLevel
          )
        )
        {
          countOfficesWithTitle++;

          //bool Is_County_Break_For_Local_Offices = false;
          //if (
          //  (db.Is_Electoral_Class_Local(Convert.ToInt16(ViewState["OfficeClass"])))
          //  && (CountyCode_New != CountyCode_Old)
          //  )
          //  Is_County_Break_For_Local_Offices = true;


          Report_Office_With_Title(
             officeKey
            , officeLine1
            , officeLine2
            , rowCountyOrLocal["CountyCode"].ToString()
            , localCode
            , officeOrderWithinLevel
            , countyCodeOld
            );
          countyCodeOld = rowCountyOrLocal["CountyCode"].ToString();
        }
        else
        {
          countOfficesWithoutTitle++;
        }

        #endregion A County or Local District
      }
      #endregion Offices WITH Part in Office Title

      #region Offices WITHOUT Part in Office Title
      Label_Bulk_Offices_Report.Text += "<br><br><strong>Offices WITHOUT Search Part in Office Title [CountyCode]</strong><br>";
      tableCountiesOrLocals = Counties_Or_Locals_Table();
      foreach (DataRow rowCountyOrLocal in tableCountiesOrLocals.Rows)
      {
        #region A County or Local District

        var officeKey = string.Empty;
        var officeLine1 = string.Empty;
        var officeLine2 = string.Empty;
        var officeOrderWithinLevel = 0;
        var localCode = string.Empty;

        if (Is_Electoral_Class_Local(Convert.ToInt16(ViewState["OfficeClass"])))
          localCode = rowCountyOrLocal["LocalCode"].ToString();

        if (!Is_Office_In_Table(
          ViewState["StateCode"].ToString()
          , rowCountyOrLocal["CountyCode"].ToString()
          , localCode
          , ref officeKey
          , ref officeLine1
          , ref officeLine2
          , Convert.ToInt16(ViewState["OfficeClass"])
          , ref officeOrderWithinLevel
          )
        )
        {
          #region Report Office
          Label_Bulk_Offices_Report.Text += "<br>";
          Label_Bulk_Offices_Report.Text += CountyCache.GetCountyName(ViewState["StateCode"].ToString(), rowCountyOrLocal["CountyCode"].ToString());
          if (Is_Electoral_Class_Local(Convert.ToInt16(ViewState["OfficeClass"])))
            Label_Bulk_Offices_Report.Text += " - "
            + GetPageCache().LocalDistricts.GetLocalDistrict(ViewState["StateCode"].ToString()
              , rowCountyOrLocal["CountyCode"].ToString(), localCode);
          Label_Bulk_Offices_Report.Text += " [" + rowCountyOrLocal["CountyCode"] + "]";
          if (Is_Electoral_Class_Local(Convert.ToInt16(ViewState["OfficeClass"])))
            Label_Bulk_Offices_Report.Text += " [" + localCode + "]";
          #endregion Report Office
        }

        #endregion A County or Local District
      }
      #endregion Offices WITHOUT Part in Office Title

      #region results
      Label_Bulk_Offices_Report.Text += "<br><br>";
      Label_Bulk_Offices_Report.Text += countOfficesWithTitle
        + " offices WITH test string in title.";
      Label_Bulk_Offices_Report.Text += "<br>";
      Label_Bulk_Offices_Report.Text += countOfficesWithoutTitle
        + " offices WITHOUT test string in title.";

      Label_Bulk_Offices_Report.Text += "<br><br>";
      Label_Bulk_Offices_Report.Text += countCountiesOrLocals.ToString(CultureInfo.InvariantCulture);
      if (Is_Electoral_Class_County(Convert.ToInt16(ViewState["OfficeClass"])))
      {
        Label_Bulk_Offices_Report.Text += " counties checked.";
        var counties = G.Rows_Count_From("Counties WHERE StateCode = "
          + SqlLit(ViewState["StateCode"].ToString()));
        Label_Bulk_Offices_Report.Text += "<br>" + counties
          + " counties in the state.";
      }
      else
      {
        Label_Bulk_Offices_Report.Text += " local districts checked.";
        var locals = G.Rows_Count_From("LocalDistricts WHERE StateCode = "
          + SqlLit(ViewState["StateCode"].ToString()));
        Label_Bulk_Offices_Report.Text += "<br>" + locals
          + " local districts in the state.";
      }

      #endregion results

    }

    protected void TextBox_Office_Title_Search_TextChanged(object sender, EventArgs e)
    {
      try
      {
        Report_Search_Results();

        Msg.Text = Ok("Report is finished.");
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void Button_Add_Bulk_Offices_Click1(object sender, EventArgs e)
    {
      try
      {
        #region checks
        Check_TextBoxs_Illeagal_Input();

        if (ViewState["OfficeClass"].ToOfficeClass() == OfficeClass.Undefined)
          throw new ApplicationException("An office class needs to be selected from the radio button list");
        if (string.IsNullOrEmpty(TextBox_Office_Line_Add_1.Text.Trim()))
          throw new ApplicationException("The 1st line of the office title is empty.");
        if (string.IsNullOrEmpty(TextBox_Order.Text.Trim()))
          throw new ApplicationException("You need to provide a Ballot order.");
        if (!Is_Valid_Integer(TextBox_Order.Text.Trim()))
          throw new ApplicationException("The Ballot order is not an integer.");
        if (!CheckBox_SkipCheck.Checked)
          Check_Search_And_OfficeTitle_Compatable();
        #endregion checks

        var countOfficesAdded = 0;
        var countCountiesOrLocals = 0;
        Label_Bulk_Offices_Report.Text = string.Empty;
        var countyCodeOld = string.Empty;

        var tableCountiesOrLocals = Counties_Or_Locals_Table();
        foreach (DataRow rowCountyOrLocal in tableCountiesOrLocals.Rows)
        {
          countCountiesOrLocals++;

          #region A County or Local District
          {
            #region Add Office if doesn't exist
            var officeKey = string.Empty;
            var officeLine1 = string.Empty;
            var officeLine2 = string.Empty;
            var officeOrderWithinLevel = 0;

            var localCode = string.Empty;
            if (Is_Electoral_Class_Local(Convert.ToInt16(ViewState["OfficeClass"])))
              localCode = rowCountyOrLocal["LocalCode"].ToString();

            if (!Is_Office_In_Table(
                ViewState["StateCode"].ToString()
                , rowCountyOrLocal["CountyCode"].ToString()
                , localCode
                , ref officeKey
                , ref officeLine1
                , ref officeLine2
                , Convert.ToInt16(ViewState["OfficeClass"])
                , ref officeOrderWithinLevel
                )
              )
            {
              officeKey = Insert_Into_Offices(
                ViewState["StateCode"].ToString()
                , rowCountyOrLocal["CountyCode"].ToString()
                , localCode
                , string.Empty
                , string.Empty
                , TextBox_Office_Line_Add_1.Text.Trim()
                , TextBox_Office_Line_Add_2.Text.Trim()
                , ViewState["OfficeClass"].ToOfficeClass()
                , TextBox_Order.Text.Trim()
                , string.Empty
                );

              countOfficesAdded++;

              Report_Office_With_Title(
                 officeKey
                , TextBox_Office_Line_Add_1.Text.Trim()
                , TextBox_Office_Line_Add_2.Text.Trim()
                , rowCountyOrLocal["CountyCode"].ToString()
                , localCode
                , Convert.ToInt16(TextBox_Order.Text.Trim())
                , countyCodeOld
                );
              countyCodeOld = rowCountyOrLocal["CountyCode"].ToString();
            }
            #endregion Add Office if doesn't exist
          }

          #endregion A County or Local District
        }

        Clear_Office_Title_Textboxes();

        #region results
        Label_Bulk_Offices_Report.Text += "<br><br>";
        if (countOfficesAdded == 0)
          Label_Bulk_Offices_Report.Text += "No offices were added.";
        else
          Label_Bulk_Offices_Report.Text += countOfficesAdded
            + " offices were added.";

        Label_Bulk_Offices_Report.Text += "<br><br>";
        Label_Bulk_Offices_Report.Text += countCountiesOrLocals.ToString(CultureInfo.InvariantCulture);
        if (Is_Electoral_Class_County(Convert.ToInt16(ViewState["OfficeClass"])))
        {
          Label_Bulk_Offices_Report.Text += " counties checked.";
          var counties = G.Rows_Count_From("Counties WHERE StateCode = "
            + SqlLit(ViewState["StateCode"].ToString()));
          Label_Bulk_Offices_Report.Text += "<br>" + counties
            + " counties in the state.";

          Msg.Text = Ok(TextBox_Office_Line_Add_1.Text + " " + TextBox_Office_Line_Add_2.Text
           + " was ADDED for every COUNTY that did not have this office.");
        }
        else
        {
          Label_Bulk_Offices_Report.Text += " local districts checked.";
          var locals = G.Rows_Count_From("LocalDistricts WHERE StateCode = "
            + SqlLit(ViewState["StateCode"].ToString()));
          Label_Bulk_Offices_Report.Text += "<br>" + locals
            + " local districts in the state.";

          Msg.Text = Ok(TextBox_Office_Line_Add_1.Text + " " + TextBox_Office_Line_Add_2.Text
            + " was ADDED for every LOCAL DISTRICT that did not have this office.");
        }

        #endregion results

      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void Button_Update_Bulk_Offices_Click(object sender, EventArgs e)
    {
      try
      {
        #region checks
        Check_TextBoxs_Illeagal_Input();

        if (ViewState["OfficeClass"].ToOfficeClass() == OfficeClass.Undefined)
          throw new ApplicationException("An office class needs to be selected from the radio button list");
        if (!string.IsNullOrEmpty(TextBox_Order.Text.Trim()))
        {
          if (!Is_Valid_Integer(TextBox_Order.Text.Trim()))
            throw new ApplicationException("The Ballot order is not an integer.");
        }
        Check_Both_Office_Title_And_BallotOrder_Not_Empty();

        if (
            (!CheckBox_SkipCheck.Checked)
            && (!string.IsNullOrEmpty(TextBox_Office_Line_Add_1.Text.Trim()))
          )
          Check_Search_And_OfficeTitle_Compatable();
        #endregion checks

        var countOfficesUpdated = 0;
        var countCountiesOrLocals = 0;
        Label_Bulk_Offices_Report.Text = string.Empty;
        var countyCodeOld = string.Empty;

        var tableCountiesOrLocals = Counties_Or_Locals_Table();
        foreach (DataRow rowCountyOrLocal in tableCountiesOrLocals.Rows)
        {
          countCountiesOrLocals++;

          #region A County or Local District
          {
            #region Update Office if office exist
            var officeKey = string.Empty;
            var officeLine1 = string.Empty;
            var officeLine2 = string.Empty;
            var officeOrderWithinLevel = 0;

            var localCode = string.Empty;
            if (Is_Electoral_Class_Local(Convert.ToInt16(ViewState["OfficeClass"])))
              localCode = rowCountyOrLocal["LocalCode"].ToString();

            if (Is_Office_In_Table(
                ViewState["StateCode"].ToString()
                , rowCountyOrLocal["CountyCode"].ToString()
                , localCode
                , ref officeKey
                , ref officeLine1
                , ref officeLine2
                , Convert.ToInt16(ViewState["OfficeClass"])
                , ref officeOrderWithinLevel
                )
              )
            {
              #region Update Office
              if (!string.IsNullOrEmpty(TextBox_Office_Line_Add_1.Text.Trim()))
              {
                Offices.UpdateOfficeLine1(TextBox_Office_Line_Add_1.Text.Trim(), officeKey);
                Offices.UpdateOfficeLine2(TextBox_Office_Line_Add_2.Text.Trim(), officeKey);
                //db.Offices_Update_Str(OfficeKey, "OfficeLine1", TextBox_Office_Line_Add_1.Text.Trim());
                //db.Offices_Update_Str(OfficeKey, "OfficeLine2", TextBox_Office_Line_Add_2.Text.Trim());
              }
              if (!string.IsNullOrEmpty(TextBox_Order.Text.Trim()))
                Offices_Update_Int(officeKey, "OfficeOrderWithinLevel", Convert.ToInt16(TextBox_Order.Text.Trim()));

              countOfficesUpdated++;
              #endregion Update Office

              Report_Office_With_Title(
                 officeKey
                , Offices_Str(officeKey, "OfficeLine1")
                , Offices_Str(officeKey, "OfficeLine2")
                , rowCountyOrLocal["CountyCode"].ToString()
                , localCode
                , Offices_Int(officeKey, "OfficeOrderWithinLevel")
                , countyCodeOld
                );
              countyCodeOld = rowCountyOrLocal["CountyCode"].ToString();
            }
            #endregion Add Office if doesn't exist
          }

          #endregion A County or Local District
        }

        Clear_Office_Title_Textboxes();

        #region results
        Label_Bulk_Offices_Report.Text += "<br><br>";
        if (countOfficesUpdated == 0)
          Label_Bulk_Offices_Report.Text += "No offices were updated.";
        else
          Label_Bulk_Offices_Report.Text += countOfficesUpdated
            + " offices were updated.";

        Label_Bulk_Offices_Report.Text += "<br><br>";
        Label_Bulk_Offices_Report.Text += countCountiesOrLocals.ToString(CultureInfo.InvariantCulture);
        if (Is_Electoral_Class_County(Convert.ToInt16(ViewState["OfficeClass"])))
        {
          Label_Bulk_Offices_Report.Text += " counties checked.";
          var counties = G.Rows_Count_From("Counties WHERE StateCode = "
            + SqlLit(ViewState["StateCode"].ToString()));
          Label_Bulk_Offices_Report.Text += "<br>" + counties
            + " counties in the state.";

          Msg.Text = Ok(TextBox_Office_Line_Add_1.Text + " " + TextBox_Office_Line_Add_2.Text
           + " was UPDATED for every COUNTY that did had this office title.");
        }
        else
        {
          Label_Bulk_Offices_Report.Text += " local districts checked.";
          var locals = G.Rows_Count_From("LocalDistricts WHERE StateCode = "
            + SqlLit(ViewState["StateCode"].ToString()));
          Label_Bulk_Offices_Report.Text += "<br>" + locals
            + " local districts in the state.";

          Msg.Text = Ok(TextBox_Office_Line_Add_1.Text + " " + TextBox_Office_Line_Add_2.Text
            + " was UPDATED for every LOCAL DISTRICT that did had this office title.");
        }

        #endregion results

      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void Button_Delete_Office_Click(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_OfficeKey);

        if (!Offices.OfficeKeyExists(TextBox_OfficeKey.Text.Trim()))
          throw new ApplicationException("The office does not exist so there was no office to delete.");

        Office_Delete_All_Tables_All_Rows(TextBox_OfficeKey.Text.Trim());

        if (!CheckBox_Supress_Report.Checked)
          Report_Search_Results();

        Msg.Text = Ok("The Office with the OfficeKey:"
          + TextBox_OfficeKey.Text
          + " has been DELETED in all Tables.");

        TextBox_OfficeKey.Text = string.Empty;
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }

    }

    protected void Button_Run_Report_Click(object sender, EventArgs e)
    {
      try
      {
        Report_Search_Results();

        Msg.Text = Ok("Report is finished.");
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void Button_Add_One_Office_Click1(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_CountyCode);
        Throw_Exception_TextBox_Html_Or_Script(TextBox_LocalCode);

        if (
          (!Is_Digits(TextBox_CountyCode.Text.Trim()))
          && (TextBox_CountyCode.Text.Trim().Length != 3)
          )
          throw new ApplicationException("The CountyCode must be 3 digits.");
        if (!CountyCache.CountyExists(
          ViewState["StateCode"].ToString()
          , TextBox_CountyCode.Text.Trim()))
          throw new ApplicationException("The CountyCode is not a valid.");

        if (string.IsNullOrEmpty(TextBox_Office_Title_Search.Text.Trim()))
          throw new ApplicationException("The partof office titles to search is empty."
            + " This is needed to insure duplicate offices are not created.");

        Check_Office_Title_Or_BallotOrder_Empty();

        if (Is_Electoral_Class_County(Convert.ToInt16(ViewState["OfficeClass"])))
        {
          #region County

          #region checks

          if (!string.IsNullOrEmpty(TextBox_LocalCode.Text))
            throw new ApplicationException("The LocalCode must be empty.");
          #endregion checks

          var officeKey = Insert_Into_Offices(
           ViewState["StateCode"].ToString()
           , TextBox_CountyCode.Text.Trim()
           , string.Empty
           , string.Empty
           , string.Empty
           , TextBox_Office_Line_Add_1.Text.Trim()
           , TextBox_Office_Line_Add_2.Text.Trim()
           , ViewState["OfficeClass"].ToOfficeClass()
           , TextBox_Order.Text.Trim()
           , string.Empty
           );

          if (!CheckBox_Supress_Report.Checked)
            Report_Search_Results();

          Msg.Text = Ok(TextBox_Office_Line_Add_1.Text.Trim()
            + " " + TextBox_Office_Line_Add_2.Text.Trim()
            + " was added in "
            + CountyCache.GetCountyName(ViewState["StateCode"].ToString()
                , TextBox_CountyCode.Text.Trim())
            + " OfficeKey:" + officeKey
            + " CountyCode:" + TextBox_CountyCode.Text.Trim()
            );

          TextBox_CountyCode.Text = string.Empty;

          #endregion County
        }
        else
        {
          #region Local

          #region checks
          if (
           (!Is_Digits(TextBox_LocalCode.Text.Trim()))
           && (TextBox_CountyCode.Text.Trim().Length != 2)
           )
            throw new ApplicationException("The LocalCode must be 2 digits.");

          if (!LocalDistricts.IsValid(
                ViewState["StateCode"].ToString()
                , TextBox_CountyCode.Text.Trim()
                , TextBox_LocalCode.Text.Trim()
                )
              )
            throw new ApplicationException("The LocalCode is not a valid.");

          #endregion checks

          var officeKey = Insert_Into_Offices(
           ViewState["StateCode"].ToString()
           , TextBox_CountyCode.Text.Trim()
           , TextBox_LocalCode.Text.Trim()
           , string.Empty
           , string.Empty
           , TextBox_Office_Line_Add_1.Text.Trim()
           , TextBox_Office_Line_Add_2.Text.Trim()
           , ViewState["OfficeClass"].ToOfficeClass()
           , TextBox_Order.Text.Trim()
           , string.Empty
           );

          if (!CheckBox_Supress_Report.Checked)
            Report_Search_Results();

          Msg.Text = Ok(TextBox_Office_Line_Add_1.Text.Trim()
            + " " + TextBox_Office_Line_Add_2.Text.Trim()
            + " was added in " + CountyCache.GetCountyName(
              ViewState["StateCode"].ToString()
              , TextBox_CountyCode.Text.Trim())
            + ", " + GetPageCache().LocalDistricts.GetLocalDistrict(
              ViewState["StateCode"].ToString()
              , TextBox_CountyCode.Text.Trim()
              , TextBox_LocalCode.Text.Trim())
            + " OfficeKey:" + officeKey
            + " CountyCode:" + TextBox_CountyCode.Text.Trim()
            + " LocalCode:" + TextBox_LocalCode.Text.Trim()
             );

          TextBox_CountyCode.Text = string.Empty;
          TextBox_LocalCode.Text = string.Empty;
          #endregion Local
        }
      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void RadioButtonList_Office_Classes_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        ViewState["OfficeClass"] = RadioButtonList_Office_Classes.SelectedValue;

        Clear_Office_Title_Textboxes();
        TextBox_Office_Title_Search.Text = string.Empty;

        Page_Title();

      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }
    }

    protected void Button_Recase1_Click(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Line_Add_1);
        Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Line_Add_2);

        TextBox_Office_Line_Add_1.Text = Str_ReCase_Office_Title(TextBox_Office_Line_Add_1.Text);

        TextBox_Office_Line_Add_2.Text = Str_ReCase_Office_Title(TextBox_Office_Line_Add_2.Text);

        Msg.Text = Ok("Both office title lines were recased.");

      }
      catch (Exception ex)
      {
        #region
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
        #endregion
      }

    }

    protected void Button_Recase2_Click(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Line_Edit_1);
        Throw_Exception_TextBox_Html_Or_Script(TextBox_Office_Line_Edit_2);

        TextBox_Office_Line_Edit_1.Text =
          Str_ReCase_Office_Title(TextBox_Office_Line_Edit_1.Text);
        Offices.UpdateOfficeLine1(TextBox_Office_Line_Edit_1.Text.Trim(),
          ViewState["OfficeKey"].ToString());

        TextBox_Office_Line_Edit_2.Text =
          Str_ReCase_Office_Title(TextBox_Office_Line_Edit_2.Text);
        Offices.UpdateOfficeLine2(TextBox_Office_Line_Edit_2.Text.Trim(),
          ViewState["OfficeKey"].ToString());

        Msg.Text = Ok("Both office title lines were recased.");
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        #region ViewState Values and Security Checks
        #region ViewState Values
        ViewState["StateCode"] = string.Empty;
        ViewState["OfficeKey"] = string.Empty;
        ViewState["OfficeClass"] = OfficeClass.Undefined.ToInt();
        ViewState["ElectionKey"] = string.Empty;
        #region Note
        //New Multi-County Offices may have a DistrictCode and DistrictCodeAlpha
        //If so they are pass via querystrings
        #endregion Note
        ViewState["DistrictCode"] = string.Empty;
        ViewState["DistrictCodeAlpha"] = string.Empty;
        ViewState["Electoral"] = ElectoralUndefined;

        if (!string.IsNullOrEmpty(QueryOffice))
          ViewState["OfficeKey"] = QueryOffice;

        if (!string.IsNullOrEmpty(GetQueryString("Class")))
          ViewState["OfficeClass"] = Convert.ToUInt16(GetQueryString("Class"));

        if (!string.IsNullOrEmpty(QueryElection))
          ViewState["ElectionKey"] = QueryElection;

        if (!string.IsNullOrEmpty(GetQueryString("District")))
          ViewState["DistrictCode"] = GetQueryString("District");

        if (!string.IsNullOrEmpty(GetQueryString("DistrictAlpha")))
          ViewState["DistrictCodeAlpha"] = GetQueryString("District");

        if (!string.IsNullOrEmpty(GetQueryString("Electoral")))
          ViewState["Electoral"] = Convert.ToInt16(GetQueryString("Electoral"));

        #endregion Values

        #region Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

        #region Notes
        //The Session UserStateCode, UserCountyCode, UserLocalCode can be changed
        //by a higher administration level using query strings
        //This is done in db.State_Code(), db.County_Code(), db.Local_Code()
        //
        //Using ViewState variables insures these values won't
        //change on any postbacks or in different tab or browser Sessions.
        //
        //ViewState["StateCode"] can be a StateCode or U1, u2, u3 for FederalCode
        #endregion Notes

        if (!string.IsNullOrEmpty(QueryState))
        {
          ViewState["StateCode"] = QueryState;
        }

        if (!string.IsNullOrEmpty(QueryOffice))
        {
          ViewState["StateCode"] =
            Offices.GetStateCodeFromKey(QueryOffice);
          ViewState["CountyCode"] =
            Offices.GetCountyCodeFromKey(QueryOffice);
          ViewState["LocalCode"] =
            Offices.GetLocalCodeFromKey(QueryOffice);
        }
        else if (!string.IsNullOrEmpty(QueryElection))
        {
          ViewState["StateCode"] =
            Elections.GetStateCodeFromKey(QueryElection);
          ViewState["CountyCode"] =
            Elections.GetCountyCodeFromKey(QueryElection);
          ViewState["LocalCode"] =
            Elections.GetLocalCodeFromKey(QueryElection);
        }
        else
        {
          ViewState["StateCode"] = G.State_Code();
          ViewState["CountyCode"] = G.County_Code();
          ViewState["LocalCode"] = G.Local_Code();
        }

        #endregion Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]
        #endregion ViewState Values and Security Checks

        try
        {
          #region checks
          var officeKey = ViewState["OfficeKey"].ToString();

          if (!string.IsNullOrWhiteSpace(officeKey) && !Offices.OfficeKeyExists(officeKey))
            throw new ApplicationException(
              "No OfficeID in Offices Table: "
              + officeKey);

          if (ViewState["OfficeClass"].ToOfficeClass() != OfficeClass.Undefined)
          {
            if (!ViewState["OfficeClass"].ToOfficeClass().IsValid())
              throw new ApplicationException(
                "The Office Level is not valid.");
          }

          if (
            (!string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
            && (!Is_Valid_Election(ViewState["ElectionKey"].ToString()))
            )
          {
            throw new ApplicationException(
              "No ElectionID in Elections Table: "
              + ViewState["ElectionKey"]);
          }

          if (
            !string.IsNullOrEmpty(ViewState["ElectionKey"].ToString())
            && !string.IsNullOrEmpty(ViewState["OfficeKey"].ToString())
            && !Offices.IsInElection(officeKey, ViewState["ElectionKey"].ToString())
                )
          {
            throw new ApplicationException(
              "This office is not and office contest in this election.");
          }
          #endregion checks

          #region form ID
          if (officeKey != string.Empty)
          {
            OfficeID.Visible = true;
            OfficeID.Text = "ID: " + Offices_Str(officeKey, "OfficeKey");
          }
          else
            OfficeID.Visible = false;
          #endregion form ID

          Title = "Office";
          Page_Title();

          Controls_Visible_Load();

          Msg.Text = Message("Use this form to " + Form_Function_Description().ToLower() + ".");
        }
        catch (Exception ex)
        {
          #region
          Msg.Text = Fail(ex.Message);
          Log_Error_Admin(ex);
          #endregion
        }
      }
    }

    protected void TextBox_Office_Line_Add_1_TextChanged1(object sender, EventArgs e)
    {

    }
  }
}
