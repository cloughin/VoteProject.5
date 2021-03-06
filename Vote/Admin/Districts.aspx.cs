using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Web.UI.HtmlControls;
using DB.Vote;
using DB.VoteLog;
using Vote.Reports;

namespace Vote.Admin
{
  public partial class DistrictsPage : SecureAdminPage
  {
    #region from db

    public static int Rows(string table, string keyName1, string keyValue1,
      string keyName2, string keyValue2, string keyName3, string keyValue3) => 
      Db.Rows(table, keyName1, keyValue1, keyName2, keyValue2, keyName3,
      keyValue3);

    public static bool Is_Valid_Election(string electionKey)
    {
      if (!string.IsNullOrEmpty(electionKey))
        return G.Rows("Elections", "ElectionKey", electionKey) == 1;
      return false;
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

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    public static string Str_Remove_Single_And_Double_Quotes(string str2Modify)
    {
      var str = str2Modify;
      str = str.Replace("\'", string.Empty);
      str = str.Replace("\"", string.Empty);

      return str;
    }

    public static DataRow Row_Last_Optional(string sql)
    {
      var table = G.Table(sql);
      return table.Rows.Count >= 1
        ? table.Rows[table.Rows.Count - 1]
        : null;
    }

    public static void Triple_Key_Update_Str(string table, string column, string columnValue,
      string keyName1, string keyValue1, string keyName2, string keyValue2, string keyName3,
      string keyValue3)
    {
      var updateSql = "UPDATE " + table
        + " SET " + column + " = " + SqlLit(columnValue)
        + " WHERE " + keyName1 + " = " + SqlLit(keyValue1)
        + " AND " + keyName2 + " = " + SqlLit(keyValue2)
        + " AND " + keyName3 + " = " + SqlLit(keyValue3);
      G.ExecuteSql(updateSql);
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

    public static string Elections_Str(string electionKey, string columnName) => 
      Elections_Str(electionKey, Elections.GetColumn(columnName));

    public static string Elections_Str(string electionKey, Elections.Column column)
    {
      var value = Elections.GetColumn(column, electionKey);
      if (value == null) return string.Empty;
      return value as string;
    }

    public static string Name_Election(string electionKey) => 
      (electionKey != string.Empty) && Is_Valid_Election(electionKey)
        ? Elections_Str(electionKey, "ElectionDesc")
        : string.Empty;

    public static string Name_Office_Contest_And_Electoral(string officeKey)
    {
      if (Offices.IsValid(officeKey))
      {
        var nameOfficeContest = string.Empty;

        nameOfficeContest += Offices.FormatOfficeName(officeKey);

        nameOfficeContest += ", ";

        nameOfficeContest +=
          Offices.GetElectoralClassDescriptionFromOfficeKey(officeKey);

        return nameOfficeContest;
      }
      return string.Empty;
    }

    public static string GetOfficeData(string officeKey)
    {
      var officeData = string.Empty;

      if (Offices.IsValid(officeKey))
      {
        officeData += "<br>StateCode:" + Offices.GetStateCodeFromKey(officeKey);
        officeData += ", CountyCode:" + Offices.GetCountyCodeFromKey(officeKey);
        officeData += ", LocalCode:" + Offices.GetLocalCodeFromKey(officeKey);
        officeData += "<br>OfficeClass:"
          + " (" + Offices.GetOfficeClass(officeKey).ToInt() + ") "
          + Offices.GetOfficeClassDescription(officeKey);

        officeData += "<br>";
        officeData += Offices.GetOfficeClassDescription(
          Offices.GetOfficeClass(officeKey)
          , Offices.GetStateCodeFromKey(officeKey));
        officeData += "<br>";
        officeData += "<strong>"
          + Name_Office_Contest_And_Electoral(officeKey)
          + "</strong>";

        officeData += "<br>";
        officeData += "In the following ELECTIONS:";
        var sql = string.Empty;
        sql += " ElectionsOffices";
        sql += " WHERE OfficeKey = " + SqlLit(officeKey);
        if (G.Rows_Count_From(sql) > 0)
        {
          sql = string.Empty;
          sql += " SELECT";
          sql += " ElectionKey";
          sql += " FROM ElectionsOffices";
          sql += " ElectionsOffices";
          sql += " WHERE OfficeKey = " + SqlLit(officeKey);
          var electionsTable = G.Table(sql);
          if (electionsTable != null)
          {
            foreach (DataRow electionsRow in electionsTable.Rows)
              officeData += "<br><strong>"
                + Name_Election(electionsRow["ElectionKey"].ToString())
                + "</strong>";
          }
        }
        else
        {
          officeData += "<br><strong>NONE</strong>";
        }

        officeData += "<br>";
        officeData += "POLITICIAN(S) as Incumbnet to this office:";
        sql = string.Empty;
        sql += " OfficesOfficials";
        sql += " WHERE OfficeKey = " + SqlLit(officeKey);
        if (G.Rows_Count_From(sql) > 0)
        {
          sql = string.Empty;
          sql += " SELECT";
          sql += " PoliticianKey";
          sql += " FROM OfficesOfficials";
          sql += " WHERE OfficeKey = " + SqlLit(officeKey);
          var politiciansTable = G.Table(sql);
          if (politiciansTable != null)
          {
            foreach (DataRow politicianRow in politiciansTable.Rows)
              officeData += "<br><strong>"
                + Politicians.GetFormattedName(politicianRow["PoliticianKey"].ToString())
                + "</strong>";
          }
        }
        else
        {
          officeData += "<br><strong>NONE</strong>";
        }
      }
      else
        officeData += "<br><strong>Office does not exist</strong>";

      return officeData;
    }

    private static string FormatName(string name) => 
      (name.Trim() != string.Empty) && !Is_Request_Erase(name.Trim())
      ? name.Trim().ToTitleCase()
      : name;

    private static bool Is_Request_Erase(string dataTo) => 
      dataTo.ToUpper().Trim().Replace(" ", string.Empty) == "N/A";

    private static string Str_Remove_SpecialChars_All_Except_Spaces(string str2Modify)
    {
      var str = str2Modify;
      str = str.Trim();
      str = Str_Remove_SpecialChars_Except_Single_And_Double_Quotes_Dash_And_Period(str);
      str = Str_Remove_Single_And_Double_Quotes(str);

      return str;
    }

    private static string Str_Remove_SpecialChars_Except_Single_And_Double_Quotes_Dash_And_Period(
      string str2Modify)
    {
      var str = str2Modify;
      //characters to strip off
      //single ' keep for names like O'Donnell
      //. also kept for Middle Name abreviations 
      //- kept for Hypenated last names
      str = str.Trim();
      str = str.Replace("+", string.Empty);
      str = str.Replace("=", string.Empty);
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

    //private static string Str_Remove_Non_Digits(string str2Fix)
    //{
    //  var fixedStr = string.Empty;
    //  var chars = str2Fix.ToCharArray(0, str2Fix.Length);
    //  for (var i = 0; i <= str2Fix.Length - 1; i++)
    //  {
    //    if (char.IsDigit(chars[i]))
    //      fixedStr += chars[i];
    //  }
    //  return fixedStr;
    //}

    //private static bool Is_Has_Digits(string strToCheck)
    //{
    //  var chars = strToCheck.ToCharArray(0, strToCheck.Length);
    //  for (var i = 0; i <= strToCheck.Length - 1; i++)
    //  {
    //    if (char.IsDigit(chars[i]))
    //      return true;
    //  }
    //  return false;
    //}

    //private static int Tokens(string strToCheck)
    //{
    //  strToCheck = strToCheck.Trim();

    //  var count = 0;
    //  if (!string.IsNullOrEmpty(strToCheck))
    //    count = 1;

    //  while (strToCheck.IndexOf(" ", 0, strToCheck.Length, StringComparison.Ordinal) != -1)
    //  {
    //    var locToken = strToCheck.IndexOf(" ", 0, strToCheck.Length, StringComparison.Ordinal);
    //    count++;
    //    strToCheck = strToCheck.Substring(locToken + 1, strToCheck.Length - (locToken + 1));
    //    strToCheck = strToCheck.Trim();
    //  }
    //  return count;
    //}

    private static void LocalDistrictsUpdate(string stateCode, string countyCode, string localCode,
      string column, string columnValue)
    {
      LocalDistricts_Row_Insert_Empty(stateCode, countyCode, localCode);
      Triple_Key_Update_Str("LocalDistricts", column, columnValue, "StateCode", stateCode,
        "CountyCode", countyCode, "LocalCode", localCode);
    }

    private static void LocalDistricts_Row_Insert_Empty(string stateCode, string countyCode,
      string localCode)
    {
      if (Rows("LocalDistricts", "StateCode", stateCode, "CountyCode", countyCode, "LocalCode",
        localCode) == 0)
      {
        var sql = string.Empty;
        sql += "INSERT INTO LocalDistricts";
        sql += "(";
        sql += "StateCode";
        sql += ",CountyCode";
        sql += ",LocalCode";
        sql += ",LocalDistrict";
        sql += ",StateLocalDistrictCode";
        sql += ",Contact";
        sql += ",ContactTitle";
        sql += ",ContactEmail";
        sql += ",Phone";
        sql += ",AltContact";
        sql += ",AltContactTitle";
        sql += ",AltEMail";
        sql += ",AltPhone";
        sql += ",EMail";
        sql += ",URL";
        sql += ",BallotName";
        sql += ",ElectionsAuthority";
        sql += ",AddressLine1";
        sql += ",AddressLine2";
        sql += ",CityStateZip";
        sql += ",Notes";
        sql += ",EmailPage";
        sql += ",URLDataPage";
        sql += ",IsLocalDistrictTagForDeletion";
        sql += ",ElectionKeyOfficialsReportStatus";
        sql += ")";
        sql += "VALUES";
        sql += "(";
        sql += SqlLit(stateCode);
        sql += "," + SqlLit(countyCode);
        sql += "," + SqlLit(localCode);
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",''";
        sql += ",0";
        sql += ",''";
        sql += ")";
        G.ExecuteSql(sql);
      }
    }

    #endregion from db

    private void Local_Links()
    {
      #region Report of Local Districts

      LabelLocalDistricts.Text =
        LocalLinks.GetDistrictsLocalLinks(ViewState["StateCode"].ToString(),
          ViewState["CountyCode"].ToString(),
          RadioButtonListSort.SelectedValue != "Name").RenderToString();

      #endregion Report of Local Districts
    }

    private DataTable DataTable_Offices()
    {
      var sql = string.Empty;
      sql += " SELECT OfficeKey";
      sql += " FROM Offices";
      sql += " WHERE StateCode = " + SqlLit(ViewState["StateCode"].ToString());
      sql += " AND CountyCode = " + SqlLit(ViewState["CountyCode"].ToString());
      sql += " AND LocalCode = " + SqlLit(ViewState["LocalCode"].ToString());
      return G.Table(sql);
    }

    protected void ButtonLocalDistrictUpdate_Click(object sender, EventArgs e)
    {
      try
      {
        LocalDistrictsUpdate(ViewState["StateCode"].ToString(), ViewState["CountyCode"].ToString(),
          ViewState["LocalCode"].ToString(), "LocalDistrict", TextBoxLocalDistrictUpdate.Text.Trim());

        Local_Links();

        #region remove Change Controls

        TextBoxLocalDistrictUpdate.Text = string.Empty;
        TableLocalDistrictUpdate.Visible = false;

        #endregion remove Change Controls

        Msg.Text =
          Ok("The local district or town name has been changed and should appear in the list below.");
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    //private string Strip_Name(string nameToStrip)
    //{
    //  nameToStrip = nameToStrip.ToUpper();
    //  nameToStrip = nameToStrip.Replace("DISTRICT", string.Empty);
    //  nameToStrip = nameToStrip.Replace("TOWN", string.Empty);
    //  nameToStrip = nameToStrip.Replace("CITY", string.Empty);
    //  nameToStrip = nameToStrip.Replace("WARD", string.Empty);
    //  nameToStrip = nameToStrip.Replace("OF", string.Empty);
    //  nameToStrip = nameToStrip.Replace("THE", string.Empty);
    //  nameToStrip = nameToStrip.Replace("AND", string.Empty);
    //  nameToStrip = nameToStrip.Trim();
    //  nameToStrip = nameToStrip.Replace(" ", string.Empty);
    //  return nameToStrip;
    //}

    protected void ButtonLocalDistrictAdd_Click(object sender, EventArgs e)
    {
      try
      {
        //#region Name Checks
        //#region check if already exists
        //var nameStripped = Strip_Name(TextBoxLocalDistrictAdd.Text.Trim());

        //var tokens = Tokens(nameStripped);
        //if (tokens == 0)
        //  throw new ApplicationException("The name of the local district or town is missing.");

        //var isHasDigits = false;
        //if (Is_Has_Digits(nameStripped))
        //{
        //  isHasDigits = true;
        //  nameStripped = Str_Remove_Non_Digits(nameStripped);
        //}

        //var sql = string.Empty;
        //sql += " SELECT ";
        //sql += " LocalDistrict";
        //sql += ",LocalCode";
        //sql += " FROM LocalDistricts";
        //sql += " WHERE StateCode = " + SqlLit(ViewState["StateCode"].ToString());
        //sql += " AND CountyCode = " + SqlLit(ViewState["CountyCode"].ToString());
        //sql += " AND LocalCode != '00'";
        //sql += " ORDER BY LocalDistrict ";

        //var isDuplicateName = false;
        //var duplicateName = string.Empty;
        //var localDistrictsTable = G.Table(sql);
        //foreach (DataRow localDistrictsRow in localDistrictsTable.Rows)
        //{
        //  if (!isDuplicateName)
        //  {
        //    if (isHasDigits)
        //    {
        //      var localDistrictNum = Str_Remove_Non_Digits(localDistrictsRow["LocalDistrict"].ToString());
        //      if (localDistrictNum == nameStripped)
        //      {
        //        isDuplicateName = true;
        //        duplicateName = localDistrictsRow["LocalDistrict"].ToString();
        //      }
        //    }
        //    else
        //    {
        //      //Can't be a single character like A for District A
        //      if (nameStripped.Length != 1)
        //      {
        //        var localDistrictStripped = Strip_Name(localDistrictsRow["LocalDistrict"].ToString());
        //        var index = localDistrictStripped.IndexOf(nameStripped, StringComparison.Ordinal);
        //        if (index != -1)
        //        {
        //          isDuplicateName = true;
        //          duplicateName = localDistrictsRow["LocalDistrict"].ToString();
        //        }
        //      }
        //    }
        //  }
        //}
        //if (isDuplicateName)
        //  throw new ApplicationException(duplicateName + " appears to be a duplicate local district or town."
        //    + " If it is definitely NOT a duplicate (like: Herndon District and Herndon Town)"
        //    + " then misspell the name. And then edit the name as it should appear on ballots."
        //    + " The name is NOT part of the LocalCode so this procedure is harmless.");

        //#endregion check if already exists
        //#endregion Name Checks

        var codeNext = string.Empty;
        if (TextBoxCode.Text.Trim() != string.Empty)
        {
          #region Code Checks

          if (TextBoxCode.Text.Trim().Length != 2)
            throw new ApplicationException("The code is not 2 digits.");
          if (!Is_Digits(TextBoxCode.Text.Trim()))
            throw new ApplicationException("The code is not  digits.");
          if (LocalDistricts.IsValid(
            ViewState["StateCode"].ToString()
            , ViewState["CountyCode"].ToString()
            , TextBoxCode.Text.Trim()
          ))
            throw new ApplicationException("This code already exists.");

          #endregion Code Checks
        }
        else
        {
          #region Next Sequential Code

          var sql = string.Empty;
          sql += " SELECT ";
          sql += " LocalCode";
          sql += " FROM LocalDistricts ";
          sql += " WHERE StateCode = " + SqlLit(ViewState["StateCode"].ToString());
          sql += " AND CountyCode = " + SqlLit(ViewState["CountyCode"].ToString());
          sql += " AND LocalCode != '00'";
          sql += " ORDER BY LocalCode ASC";
          var localDistrictRow = Row_Last_Optional(sql);
          if (localDistrictRow != null)
          {
            int codeInt = Convert.ToInt16(localDistrictRow["LocalCode"]);
            if (codeInt < 10)
              codeInt = 10;
            codeInt++;
            codeNext = codeInt.ToString(CultureInfo.InvariantCulture);
          }
          else
          {
            codeNext = "11";
          }

          #endregion Next Sequential Code
        }

        #region Insert Local District

        var insertSql = "INSERT INTO LocalDistricts ";
        insertSql += "(";
        insertSql += "StateCode";
        insertSql += ",CountyCode";
        insertSql += ",LocalCode";
        insertSql += ",LocalDistrict";
        insertSql += ",StateLocalDistrictCode";
        insertSql += ",Contact";
        insertSql += ",ContactTitle";
        insertSql += ",ContactEmail";
        insertSql += ",Phone";
        insertSql += ",AltContact";
        insertSql += ",AltContactTitle";
        insertSql += ",AltEMail";
        insertSql += ",AltPhone";
        insertSql += ",EMail";
        insertSql += ",URL";
        insertSql += ",BallotName";
        insertSql += ",ElectionsAuthority";
        insertSql += ",AddressLine1";
        insertSql += ",AddressLine2";
        insertSql += ",CityStateZip";
        insertSql += ",Notes";
        insertSql += ",EmailPage";
        insertSql += ",URLDataPage";
        insertSql += ",IsLocalDistrictTagForDeletion";
        insertSql += ",ElectionKeyOfficialsReportStatus";
        insertSql += ")";
        insertSql += " VALUES(";
        insertSql += SqlLit(ViewState["StateCode"].ToString());
        insertSql += "," + SqlLit(ViewState["CountyCode"].ToString());

        if (TextBoxCode.Text.Trim() != string.Empty)
          insertSql += "," + SqlLit(TextBoxCode.Text.Trim());
        else
          insertSql += "," + SqlLit(codeNext);

        insertSql += "," + SqlLit(TextBoxLocalDistrictAdd.Text.Trim());

        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";

        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",''";
        insertSql += ",0";
        insertSql += ",''";
        insertSql += ")";
        G.ExecuteSql(insertSql);

        #endregion Insert Local District

        #region clear textboxes

        TextBoxLocalDistrictAdd.Text = string.Empty;
        TextBoxCode.Text = string.Empty;

        #endregion clear textboxes

        Local_Links();

        Msg.Text =
          Ok("The local district or town name has been added and should appear in the list below.");
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void RadioButtonListSort_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        ViewState["SortSelected"] = RadioButtonListSort.SelectedValue;

        Local_Links();
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ButtonAppendDistrict_Click(object sender, EventArgs e) => 
      TextBoxLocalDistrictAdd.Text = TextBoxLocalDistrictAdd.Text.Trim() + " District";

    protected void ButtonAppendTown_Click(object sender, EventArgs e) => 
      TextBoxLocalDistrictAdd.Text = TextBoxLocalDistrictAdd.Text.Trim() + " Town";

    protected void ButtonAppendCity_Click(object sender, EventArgs e) => 
      TextBoxLocalDistrictAdd.Text = TextBoxLocalDistrictAdd.Text.Trim() + " City";

    protected void Button_Format_Name_Click(object sender, EventArgs e)
    {
      var textbox = sender == Button_Format_Name_Add
        ? TextBoxLocalDistrictAdd
        : TextBoxLocalDistrictUpdate;
      if (!string.IsNullOrWhiteSpace(textbox?.Text))
      {
        textbox.Text = textbox.Text.Trim();
        textbox.Text = Str_Remove_SpecialChars_All_Except_Spaces(textbox.Text);
        textbox.Text = FormatName(textbox.Text);
        Msg.Text = Message("Name has been re-cased.");
      }
    }

    protected void Button_View_Offices_Click(object sender, EventArgs e)
    {
      try
      {
        Label_Offices.Text = string.Empty;
        Label_Offices.Text += "<strong>Offices in this Local District or Town</strong>";

        if (Rows("Offices", "StateCode", ViewState["StateCode"].ToString(),
          "CountyCode", ViewState["CountyCode"].ToString(),
          "LocalCode", ViewState["LocalCode"].ToString()) > 0)
        {
          var officesTable = DataTable_Offices();
          foreach (DataRow officeRow in officesTable.Rows)
          {
            Label_Offices.Text += "<br>----------------";
            Label_Offices.Text += "<br>";
            Label_Offices.Text += GetOfficeData(officeRow["OfficeKey"].ToString());
          }
        }
        else
        {
          Label_Offices.Text = "No Offices in this Local District or Town";
        }
        Msg.Text = Ok("Any office(s) for this local district or town is shown above.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void Button_Delete_Local_Click(object sender, EventArgs e)
    {
      try
      {
        if (TextBoxLocalDistrictUpdate.Text != string.Empty)
        {
          var sql = string.Empty;
          sql += " DELETE FROM LocalDistricts";
          sql += " WHERE StateCode = " + SqlLit(ViewState["StateCode"].ToString());
          sql += " AND CountyCode = " + SqlLit(ViewState["CountyCode"].ToString());
          sql += " AND LocalCode = " + SqlLit(ViewState["LocalCode"].ToString());
          G.ExecuteSql(sql);

          var officesTable = DataTable_Offices();
          foreach (DataRow officeRow in officesTable.Rows)
          {
            Office_Delete_All_Tables_All_Rows(officeRow["OfficeKey"].ToString());
          }

          Msg.Text = Ok(TextBoxLocalDistrictUpdate.Text + " has been deleted.");
          TextBoxLocalDistrictUpdate.Text = string.Empty;

          Local_Links();
        }
        else
        {
          Msg.Text = Fail("No local district or town was identified.");
        }
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
        Title = H1.InnerText = "Districts";

        ViewState["StateCode"] = G.State_Code();
        ViewState["CountyCode"] = G.County_Code();
        ViewState["LocalCode"] = G.Local_Code();

        ViewState["OfficeLevel"] = OfficeClass.Undefined.ToInt();
        if (!string.IsNullOrEmpty(GetQueryString("Level")))
          ViewState["OfficeLevel"] = Convert.ToInt16(GetQueryString("Level"));

        var mainForm = Master.FindControl("MainForm") as HtmlControl;
        Debug.Assert(mainForm != null, "mainForm != null");
        mainForm.Attributes.Add("data-state", StateCode);
        mainForm.Attributes.Add("data-county", CountyCode);

        try
        {
          #region Page Title

          PageTitle.Text = Offices.GetElectoralClassDescription(ViewState["StateCode"].ToString(),
            ViewState["CountyCode"].ToString());
          PageTitle.Text += "<br>";


          PageTitle.Text += "ADD & EDIT LOCAL DISTRICTS OR TOWNS";

          #endregion Page Title

          if (string.IsNullOrEmpty(ViewState["LocalCode"].ToString()))
          {
            #region Setup to Add a Local District

            TableLocalDistrictAdd.Visible = true;
            TableLocalDistrictUpdate.Visible = false;

            #endregion Setup to Add a Local District

            Msg.Text =
              Message("Use the section below to add a local district or town in this county."
                + " Or click one of the links in the Local Districts and Towns Report"
                + " to edit a district or town name.");
          }
          else
          {
            #region Update a Local District

            TableLocalDistrictAdd.Visible = true;
            TableLocalDistrictUpdate.Visible = true;

            #region Load Name in Textbox

            TextBoxLocalDistrictUpdate.Text =
              GetPageCache().LocalDistricts.GetLocalDistrict(ViewState["StateCode"].ToString(),
                ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString());

            #endregion Load Name in Textbox

            Msg.Text =
              Message("Use the section below to add a local district or town in this county."
                + " Use the next section to edit the district or town shown in the textbox."
                + " Or click one of the links in the Local Districts and Towns Report"
                + " to edit a different district or town name.");

            #endregion Update a Local District
          }

          ViewState["SortSelected"] = RadioButtonListSort.SelectedValue;

          Local_Links();
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