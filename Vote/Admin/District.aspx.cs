using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Admin
{
  public partial class DistrictPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region from db

    private const int TypeJudicial = 5;
    private const int ElectoralMultiCounties = 3;

    public static string JudicialDistrictCounties(string stateCode,
      string districtCode, string districtCodeAlpha)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " StateCode";
      sql += " ,DistrictCode";
      sql += " ,DistrictCodeAlpha";
      sql += " ,CountyCode ";
      sql += " FROM JudicialDistrictCounties ";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " AND DistrictCode = " + SqlLit(districtCode);
      sql += " AND DistrictCodeAlpha = " + SqlLit(districtCodeAlpha);
      return sql;
    }

    public static string JudicialDistrictCounties_DELETE(string stateCode,
      string judicialDistrictCode)
    {
      var sql = string.Empty;
      sql += "DELETE FROM JudicialDistrictCounties";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " AND DistrictCode = " + SqlLit(judicialDistrictCode);
      return sql;
    }

    public static string JudicialDistricts(string stateCode)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " StateCode ";
      sql += " ,DistrictCode ";
      sql += " ,DistrictCodeAlpha";
      sql += " ,District ";
      sql += " FROM JudicialDistricts ";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " ORDER BY DistrictCode,DistrictCodeAlpha";
      return sql;
    }

    public static string JudicialDistricts(string stateCode, string districtCode,
      string districtCodeAlpha)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " StateCode ";
      sql += " ,DistrictCode ";
      sql += " ,DistrictCodeAlpha";
      sql += " ,District ";
      sql += " FROM JudicialDistricts ";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " AND DistrictCode = " + SqlLit(districtCode);
      sql += " AND DistrictCodeAlpha = " + SqlLit(districtCodeAlpha);
      return sql;
    }

    public static string MultiCountyDistrictCountiesDelete(string stateCode,
      string districtCode, string districtCodeAlpha)
    {
      var sql = string.Empty;
      sql += "DELETE FROM MultiCountyDistrictCounties";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " AND DistrictCode = " + SqlLit(districtCode);
      sql += " AND DistrictCodeAlpha = " + SqlLit(districtCodeAlpha);
      return sql;
    }

    public static string
      MultiCountyDistrictCounties4StateCodeMultiCountyDistrictCode(
        string stateCode, string districtCode, string districtCodeAlpha)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " StateCode ";
      sql += " ,DistrictCode ";
      sql += " ,DistrictCodeAlpha ";
      sql += " ,District";
      sql += " ,CountyCode ";
      sql += " FROM MultiCountyDistrictCounties ";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " AND DistrictCode = " + SqlLit(districtCode);
      sql += " AND DistrictCodeAlpha = " + SqlLit(districtCodeAlpha);
      return sql;
    }

    public static string MultiCountyDistricts(string stateCode, string districtCode,
      string districtCodeAlpha)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " StateCode ";
      sql += " ,DistrictCode ";
      sql += " ,DistrictCodeAlpha";
      sql += " ,District ";
      sql += " FROM MultiCountyDistricts ";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " AND DistrictCode = " + SqlLit(districtCode);
      sql += " AND DistrictCodeAlpha = " + SqlLit(districtCodeAlpha);
      return sql;
    }

    public static string MultiCountyDistricts(string stateCode)
    {
      var sql = string.Empty;
      sql += " SELECT ";
      sql += " StateCode ";
      sql += " ,DistrictCode ";
      sql += " ,DistrictCodeAlpha";
      sql += " ,District ";
      sql += " FROM MultiCountyDistricts ";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " ORDER BY DistrictCode";
      return sql;
    }

    private static string Counties(string stateCode)
    {
      var sql = string.Empty;
      sql += " SELECT";
      sql += " County";
      sql += ",StateCode";
      sql += ",CountyCode";
      sql += " FROM Counties";
      sql += " WHERE StateCode = " + SqlLit(stateCode);
      sql += " ORDER BY County";
      return sql;
    }

    private static string Ok(string msg) =>
      $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    private static string Fail(string msg) =>
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

    public static string DbErrorMsg(string sql, string err) =>
      $"Database Failure for SQL Statement::{sql} :: Error Msg:: {err}";

    public static int Rows(string sql)
    {
      var table = G.Table(sql);
      return table?.Rows.Count ?? 0;
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

    private static bool Is_Valid_Judicial_District(string stateCode, string districtCode,
        string districtCodeAlpha) =>
      Row_Optional(JudicialDistricts(stateCode, districtCode, districtCodeAlpha)) != null;

    private static bool Is_Valid_MultiCounty_District(string stateCode, string districtCode,
        string districtCodeAlpha) =>
      Row_Optional(MultiCountyDistricts(stateCode, districtCode, districtCodeAlpha)) != null;

    private static bool Is_Chars_Upper(string strToCheck)
    {
      var chars = strToCheck.ToCharArray(0, strToCheck.Length);
      for (var i = 0; i <= strToCheck.Length - 1; i++)
      {
        //if (!db.Is_Char_Upper(chars[i]))
        if (!char.IsUpper(chars[i]))
          return false;
      }
      return true;
    }

    private static string MultiCountyDistricts(string stateCode, string districtCode,
        string districtCodeAlpha, string column) =>
      Triple_Key_Str("MultiCountyDistricts", column, "StateCode", stateCode, "DistrictCode",
        districtCode, "DistrictCodeAlpha", districtCodeAlpha);

    private static void MultiCountyDistrictsUpdate(string stateCode, string districtCode,
        string districtCodeAlpha, string column, string columnValue) =>
      Triple_Key_Update_Str("MultiCountyDistricts", column, columnValue, "StateCode", stateCode,
        "DistrictCode", districtCode, "DistrictCodeAlpha", districtCodeAlpha);

    private static string Triple_Key_Str(string tableName, string column, string keyName1,
      string keyValue1, string keyName2, string keyValue2, string keyName3, string keyValue3)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim()
        + " WHERE " + keyName1.Trim() + " = " + SqlLit(keyValue1.Trim())
        + " AND " + keyName2.Trim() + " = " + SqlLit(keyValue2.Trim())
        + " AND " + keyName3.Trim() + " = " + SqlLit(keyValue3.Trim());
      var table = G.Table(sql);
      if (table.Rows.Count == 1)
        return table.Rows[0][column].ToString().Trim();
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    private static string JudicialDistricts(string stateCode, string districtCode,
        string districtCodeAlpha, string column) =>
      Triple_Key_Str("JudicialDistricts", column, "StateCode", stateCode, "DistrictCode",
        districtCode, "DistrictCodeAlpha", districtCodeAlpha);

    private static void JudicialDistrictsUpdate(string stateCode, string districtCode,
        string districtCodeAlpha, string column, string columnValue) =>
      Triple_Key_Update_Str("JudicialDistricts", column, columnValue, "StateCode", stateCode,
        "DistrictCode", districtCode, "DistrictCodeAlpha", districtCodeAlpha);

    #endregion from db

    private void LoadDistrictCodesAndName()
    {
      if (Convert.ToInt16(ViewState["DistrictType"]) == TypeJudicial)
      {
        TextBoxDistrictCode.Text = JudicialDistricts(
          ViewState["StateCode"].ToString()
          , ViewState["DistrictCode"].ToString()
          , ViewState["DistrictCodeAlpha"].ToString()
          , "DistrictCode");
        TextBoxAlphaCode.Text = JudicialDistricts(
          ViewState["StateCode"].ToString()
          , ViewState["DistrictCode"].ToString()
          , ViewState["DistrictCodeAlpha"].ToString()
          , "DistrictCodeAlpha");
        TextBoxDistrict.Text = JudicialDistricts(
          ViewState["StateCode"].ToString()
          , ViewState["DistrictCode"].ToString()
          , ViewState["DistrictCodeAlpha"].ToString()
          , "District");
      }
      else if (Convert.ToInt16(ViewState["DistrictType"]) == ElectoralMultiCounties)
      {
        TextBoxDistrictCode.Text = MultiCountyDistricts(
          ViewState["StateCode"].ToString()
          , ViewState["DistrictCode"].ToString()
          , ViewState["DistrictCodeAlpha"].ToString()
          , "DistrictCode");
        TextBoxAlphaCode.Text = MultiCountyDistricts(
          ViewState["StateCode"].ToString()
          , ViewState["DistrictCode"].ToString()
          , ViewState["DistrictCodeAlpha"].ToString()
          , "DistrictCodeAlpha");
        TextBoxDistrict.Text = MultiCountyDistricts(
          ViewState["StateCode"].ToString()
          , ViewState["DistrictCode"].ToString()
          , ViewState["DistrictCodeAlpha"].ToString()
          , "District");
      }
    }

    private void DistrictRetrievedMsg() =>
      Msg.Text =
        Message("The District has been retrieved and its name is in the District Name Textbox." +
          " You may now either: 1) Add a District after this one, 2) Change the District Name, or" +
          " 3) Delete the District along with its counties and offices." +
          " Click the appropriate Help Instructions to perform either of these operations.");

    private void CheckValidDistrictCode(string districtCode, string districtCodeAlpha)
    {
      if (TextBoxDistrictCode.Text.Trim() != string.Empty)
      {
        if (TextBoxDistrictCode.Text.Trim().Length != 3)
          throw new ApplicationException("When the District Code is present it must be 3 digits.");
        if (!Is_Digits(TextBoxDistrictCode.Text.Trim()))
          throw new ApplicationException("The District Code must be all digits.");
      }

      if (TextBoxAlphaCode.Text.Trim() != string.Empty)
      {
        if (TextBoxAlphaCode.Text.Trim().Length > 4)
          throw new ApplicationException(
            "When the District Alpha Code is present it must less than 4 characters.");
        if (!Is_Chars_Upper(TextBoxAlphaCode.Text.Trim().ToUpper()))
          throw new ApplicationException("The Alpha Code must be all characters.");
      }

      if (Convert.ToInt16(ViewState["DistrictType"]) == TypeJudicial)
      {
        if (!Is_Valid_Judicial_District(
          ViewState["StateCode"].ToString()
          , districtCode
          , districtCodeAlpha.Trim().ToUpper()))
          throw new ApplicationException("The District Code(s) is invalid.");
      }
      else //Multi-County Districts
      {
        if (!Is_Valid_MultiCounty_District(
          ViewState["StateCode"].ToString()
          , districtCode
          , districtCodeAlpha.Trim().ToUpper()))
          throw new ApplicationException("The District Code(s) is invalid.");
      }
    }

    private void CheckIsNewDistrictCode(string districtCode)
    {
      if (Convert.ToInt16(ViewState["DistrictType"]) == ElectoralMultiCounties)
        //Multi-County Districts
      {
        if (Is_Valid_MultiCounty_District(ViewState["StateCode"].ToString()
          , districtCode
          , TextBoxAlphaCode.Text.Trim().ToUpper()))
          throw new ApplicationException("The District for this District Code already exists.");
      }
      else
      {
        if (Is_Valid_Judicial_District(ViewState["StateCode"].ToString()
          , districtCode
          , TextBoxAlphaCode.Text.Trim().ToUpper()))
          throw new ApplicationException("The District for this District Code already exists.");
      }
    }

    private void Checks4AddOrUpdate()
    {
      if (TextBoxDistrictCode.Text.Trim() == string.Empty)
        throw new ApplicationException("There is no District Code.");
      if (TextBoxDistrict.Text.Trim() == string.Empty)
        throw new ApplicationException("There is no District Name.");
      if (CheckboxesChecked() == 0)
        throw new ApplicationException("No Counties were checked that comprise this District.");
    }

    private void Checks4Delete()
    {
      //if (LabelDistrictCode.Text.Trim() == string.Empty)
      if (TextBoxDistrictCode.Text.Trim() == string.Empty)
        throw new ApplicationException(
          "No District has been retrieved. You need to get the District before you can delete it.");
    }

    private void ClearAllCheckboxes()
    {
      var countiesTable = G.Table(Counties(ViewState["StateCode"].ToString()));
      //var index = 0;
      //foreach (DataRow countyRow in countiesTable.Rows)
      //{
      //  CheckBoxListCounties.Items[index].Selected = false;
      //  index++;
      //}

      for (var inx = 0; inx < countiesTable.Rows.Count; inx++)
        CheckBoxListCounties.Items[inx].Selected = false;
    }

    private void ClearTextboxes()
    {
      //LabelDistrictCode.Text = string.Empty;
      TextBoxDistrictCode.Text = string.Empty;
      TextBoxDistrict.Text = string.Empty;
      TextBoxOfficeTitle.Text = string.Empty;
    }

    private int CheckboxesChecked()
    {
      var countiesTable = G.Table(Counties(ViewState["StateCode"].ToString()));
      //int index = 0;
      var countiesChecked = 0;
      //foreach (DataRow CountyRow in countiesTable.Rows)
      //{
      //  if (CheckBoxListCounties.Items[index].Selected == true)
      //    CountiesChecked++;
      //  index++;
      //}

      for (var inx = 0; inx < countiesTable.Rows.Count; inx++)
        if (CheckBoxListCounties.Items[inx].Selected)
          countiesChecked++;


      return countiesChecked;
    }

    private void SetControls4MasterUser()
    {
      if (Convert.ToInt16(ViewState["DistrictType"]) == ElectoralMultiCounties)
      {
        HyperLinkAddMultiCountyDistricts.Visible = true;
        HyperLinkAddJudicialDistricts.Visible = false;
        ViewState["Districts"] = Rows(MultiCountyDistricts(ViewState["StateCode"].ToString()));
      }
      else
      {
        HyperLinkAddMultiCountyDistricts.Visible = false;
        HyperLinkAddJudicialDistricts.Visible = true;
        ViewState["Districts"] = Rows(JudicialDistricts(ViewState["StateCode"].ToString()));
      }
    }

    private int RecordJudicialCounties(DataRow judicialDistrictRow)
    {
      var countiesTable = G.Table(Counties(ViewState["StateCode"].ToString()));
      var countyIndex = 0;
      var countiesInDistrictRecorded = 0;

      #region Delete all counties

      G.ExecuteSql(JudicialDistrictCounties_DELETE(ViewState["StateCode"].ToString()
        , judicialDistrictRow["DistrictCode"].ToString()));

      #endregion

      #region add new counties

      foreach (DataRow countyRow in countiesTable.Rows)
      {
        if (CheckBoxListCounties.Items[countyIndex].Selected)
        {
          #region Insert the JudicialDistrictCounties Rows

          var sqlinsert = "INSERT INTO JudicialDistrictCounties "
            + "("
            + "StateCode"
            + ",DistrictCode"
            + ",DistrictCodeAlpha"
            + ",CountyCode"
            + ")"
            + " VALUES("
            + SqlLit(ViewState["StateCode"].ToString())
            + "," + SqlLit(judicialDistrictRow["DistrictCode"].ToString())
            + "," + SqlLit(judicialDistrictRow["DistrictCodeAlpha"].ToString())
            + "," + SqlLit(countyRow["CountyCode"].ToString())
            + ")";
          G.ExecuteSql(sqlinsert);
          countiesInDistrictRecorded++;

          #endregion
        }
        countyIndex++;
      }

      #endregion

      //ClearAllCheckboxes();

      return countiesInDistrictRecorded;
    }

    private void RecordMultiCountyCounties(DataRow multiCountyDistrictRow)
    {
      var countiesTable = G.Table(Counties(ViewState["StateCode"].ToString()));
      var countyIndex = 0;

      #region Delete all counties

      G.ExecuteSql(MultiCountyDistrictCountiesDelete(ViewState["StateCode"].ToString()
        , multiCountyDistrictRow["DistrictCode"].ToString()
        , multiCountyDistrictRow["DistrictCodeAlpha"].ToString()));

      #endregion

      #region add new counties

      foreach (DataRow countyRow in countiesTable.Rows)
      {
        if (CheckBoxListCounties.Items[countyIndex].Selected)
        {
          #region Insert the MultiCountyDistrictCounties Rows

          var sqlinsert = "INSERT INTO MultiCountyDistrictCounties "
            + "("
            + "StateCode"
            + ",DistrictCode"
            + ",DistrictCodeAlpha"
            + ",CountyCode"
            + ")"
            + " VALUES("
            + SqlLit(ViewState["StateCode"].ToString())
            + "," + SqlLit(multiCountyDistrictRow["DistrictCode"].ToString())
            + "," + SqlLit(multiCountyDistrictRow["DistrictCodeAlpha"].ToString())
            + "," + SqlLit(countyRow["CountyCode"].ToString())
            + ")";
          G.ExecuteSql(sqlinsert);

          #endregion
        }
        countyIndex++;
      }

      #endregion

      //ClearAllCheckboxes();
    }

    private void LoadCheckboxes4MultiCountyDistrict()
    {
      var districtsTable = G.Table(MultiCountyDistricts(ViewState["StateCode"].ToString()));
      var districtRow = districtsTable.Rows[Convert.ToInt16(ViewState["CurrentDistrictRow"])];

      DataTable districtCountiesTable;
      if (Convert.ToInt16(ViewState["DistrictType"]) == TypeJudicial)
      {
        districtCountiesTable = G.Table(JudicialDistrictCounties(
          ViewState["StateCode"].ToString()
          , districtRow["DistrictCode"].ToString()
          , districtRow["DistrictCodeAlpha"].ToString()));
      }
      else
      {
        //not tested
        districtCountiesTable =
          G.Table(MultiCountyDistrictCounties4StateCodeMultiCountyDistrictCode(
            ViewState["StateCode"].ToString()
            , districtRow["DistrictCode"].ToString()
            , districtRow["DistrictCodeAlpha"].ToString()));
      }

      foreach (DataRow districtCountiesRow in districtCountiesTable.Rows)
      {
        var countiesTable = G.Table(Counties(ViewState["StateCode"].ToString()));
        var countyIndex = 0;
        foreach (DataRow countyRow in countiesTable.Rows)
        {
          if (countyRow["CountyCode"].ToString() == districtCountiesRow["CountyCode"].ToString())
          {
            CheckBoxListCounties.Items[countyIndex].Selected = true;
          }
          countyIndex++;
        }
      }
    }

    private void LoadCheckboxes4JudicialDistrict()
    {
      var districtsTable = G.Table(JudicialDistricts(ViewState["StateCode"].ToString()));
      var districtRow = districtsTable.Rows[Convert.ToInt16(ViewState["CurrentDistrictRow"])];
      var districtCountiesTable = G.Table(JudicialDistrictCounties(
        ViewState["StateCode"].ToString()
        , districtRow["DistrictCode"].ToString()
        , districtRow["DistrictCodeAlpha"].ToString()));
      foreach (DataRow districtCountiesRow in districtCountiesTable.Rows)
      {
        var countiesTable = G.Table(Counties(ViewState["StateCode"].ToString()));
        var countyIndex = 0;
        foreach (DataRow countyRow in countiesTable.Rows)
        {
          if (countyRow["CountyCode"].ToString() == districtCountiesRow["CountyCode"].ToString())
          {
            CheckBoxListCounties.Items[countyIndex].Selected = true;
          }
          countyIndex++;
        }
      }
    }

    private void LoadMultiCountyDistrict()
    {
      #region get the next District Row

      var districtsTable = G.Table(MultiCountyDistricts(ViewState["StateCode"].ToString()));
      if (districtsTable.Rows.Count < Convert.ToInt16(ViewState["CurrentDistrictRow"]) + 1)
        throw new ApplicationException("There are no more Multi County Districts.");
      var districtRow = districtsTable.Rows[Convert.ToInt16(ViewState["CurrentDistrictRow"])];

      #endregion

      TextBoxDistrictCode.Text = districtRow["DistrictCode"].ToString();
      TextBoxAlphaCode.Text = districtRow["DistrictCodeAlpha"].ToString();
      TextBoxDistrict.Text = districtRow["District"].ToString();
    }

    private void LoadJudicialDistrict()
    {
      #region get the District Row

      var districtsTable = G.Table(JudicialDistricts(ViewState["StateCode"].ToString()));
      if (districtsTable.Rows.Count < Convert.ToInt16(ViewState["CurrentDistrictRow"]) + 1)
        throw new ApplicationException("There are no more Judicial Districts.");
      var districtRow = districtsTable.Rows[Convert.ToInt16(ViewState["CurrentDistrictRow"])];

      #endregion

      TextBoxDistrictCode.Text = districtRow["DistrictCode"].ToString();
      TextBoxDistrict.Text = districtRow["District"].ToString();
      TextBoxAlphaCode.Text = districtRow["DistrictCodeAlpha"].ToString();
    }

    protected void ButtonClearCheckboxes_Click(object sender, EventArgs e) => ClearAllCheckboxes();

    protected void ButtonGetNext_Click(object sender, EventArgs e)
    {
      try
      {
        if (Convert.ToInt16(ViewState["CurrentDistrictRow"]) <
          Convert.ToInt16(ViewState["Districts"]) - 1)
        {
          ViewState["CurrentDistrictRow"] = Convert.ToInt16(ViewState["CurrentDistrictRow"]) + 1;

          if (Convert.ToInt16(ViewState["DistrictType"]) == ElectoralMultiCounties)
            //Multi-County Districts
          {
            LoadMultiCountyDistrict();
            ClearAllCheckboxes();
            LoadCheckboxes4MultiCountyDistrict();
          }
          else // Judicial Districts
          {
            LoadJudicialDistrict();
            ClearAllCheckboxes();
            LoadCheckboxes4JudicialDistrict();
          }

          DistrictRetrievedMsg();
        }
        else
        {
          Msg.Text = Fail("This is the last District. There are no more Districts.");
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

    protected void ButtonGetPrevious_Click(object sender, EventArgs e)
    {
      try
      {
        if (Convert.ToInt16(ViewState["CurrentDistrictRow"]) > 0)
        {
          ViewState["CurrentDistrictRow"] = Convert.ToInt16(ViewState["CurrentDistrictRow"]) - 1;

          if (Convert.ToInt16(ViewState["DistrictType"]) == ElectoralMultiCounties)
            //Multi-County Districts
          {
            LoadMultiCountyDistrict();
            ClearAllCheckboxes();
            LoadCheckboxes4MultiCountyDistrict();
          }
          else // Judicial Districts
          {
            LoadJudicialDistrict();
            ClearAllCheckboxes();
            LoadCheckboxes4JudicialDistrict();
          }

          DistrictRetrievedMsg();
        }
        else
        {
          Msg.Text = Fail("This is the first District. There are no previous Districts.");
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

    private void GetDistrictDataAndCounties()
    {
      CheckValidDistrictCode(TextBoxDistrictCode.Text.Trim(), TextBoxAlphaCode.Text.Trim());

      var index = 0;
      var found = false;

      if (Convert.ToInt16(ViewState["DistrictType"]) == TypeJudicial)
      {
        var districtsTable = G.Table(JudicialDistricts(ViewState["StateCode"].ToString()));
        foreach (DataRow districtRow in districtsTable.Rows)
        {
          if (
            (districtRow["DistrictCode"].ToString() == TextBoxDistrictCode.Text.ToUpper())
            &&
            (districtRow["DistrictCodeAlpha"].ToString() == TextBoxAlphaCode.Text.ToUpper().Trim())
            && !found
          )
          {
            found = true;
            ViewState["CurrentDistrictRow"] = index;
            LoadJudicialDistrict();
            ClearAllCheckboxes();
            LoadCheckboxes4JudicialDistrict();
          }
          index++;
        }
      }

      if (found)
      {
        DistrictRetrievedMsg();
      }
      else
      {
        Msg.Text =
          Fail("The District for District Code " + TextBoxDistrictCode.Text + " was NOT found.");
      }
    }

    protected void ButtonGetDistrict_Click(object sender, EventArgs e)
    {
      try
      {
        GetDistrictDataAndCounties();
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonDelete_Click(object sender, EventArgs e)
    {
      try
      {
        #region Check TextBoxes for Illegal Stuff

        Throw_Exception_TextBox_Script(TextBoxDistrictCode);
        Throw_Exception_TextBox_Html(TextBoxDistrictCode);

        Throw_Exception_TextBox_Script(TextBoxDistrict);
        Throw_Exception_TextBox_Html(TextBoxDistrict);

        Throw_Exception_TextBox_Script(TextBoxOfficeTitle);
        Throw_Exception_TextBox_Html(TextBoxOfficeTitle);

        #endregion Check TextBoxes for Illegal Stuff

        Checks4Delete();

        string sqldelete;
        if (Convert.ToInt16(ViewState["DistrictType"]) == ElectoralMultiCounties)
          //Multi-County Districts
        {
          sqldelete = "DELETE FROM MultiCountyDistrictCounties"
            + " WHERE DistrictCode = " + SqlLit(TextBoxDistrictCode.Text)
            + " AND DistrictCodeAlpha = " + SqlLit(TextBoxAlphaCode.Text);
          G.ExecuteSql(sqldelete);

          sqldelete = "DELETE FROM Offices"
            + " WHERE StateCode = " + SqlLit(ViewState["StateCode"].ToString())
            + " AND DistrictCode = " + SqlLit(TextBoxDistrictCode.Text)
            + " AND DistrictCodeAlpha = " + SqlLit(TextBoxAlphaCode.Text)
            + " AND OfficeLevel = " + ElectoralMultiCounties;
          G.ExecuteSql(sqldelete);

          sqldelete = "DELETE FROM MultiCountyDistricts"
            + " WHERE StateCode = " + SqlLit(ViewState["StateCode"].ToString())
            + " AND DistrictCode = " + SqlLit(TextBoxDistrictCode.Text)
            + " AND DistrictCodeAlpha = " + SqlLit(TextBoxAlphaCode.Text);
          G.ExecuteSql(sqldelete);
        }
        else // Judicial Districts
        {
          sqldelete = "DELETE FROM JudicialDistrictCounties"
            + " WHERE DistrictCode = " + SqlLit(TextBoxDistrictCode.Text)
            + " AND DistrictCodeAlpha = " + SqlLit(TextBoxAlphaCode.Text);
          G.ExecuteSql(sqldelete);

          sqldelete = "DELETE FROM Offices"
            + " WHERE StateCode = " + SqlLit(ViewState["StateCode"].ToString())
            + " AND DistrictCode = " + SqlLit(TextBoxDistrictCode.Text)
            + " AND DistrictCodeAlpha = " + SqlLit(TextBoxAlphaCode.Text)
            + " AND OfficeLevel = " + ElectoralMultiCounties;
          G.ExecuteSql(sqldelete);

          sqldelete = "DELETE FROM JudicialDistricts"
            + " WHERE StateCode = " + SqlLit(ViewState["StateCode"].ToString())
            + " AND DistrictCode = " + SqlLit(TextBoxDistrictCode.Text)
            + " AND DistrictCodeAlpha = " + SqlLit(TextBoxAlphaCode.Text);
          G.ExecuteSql(sqldelete);
        }

        Msg.Text = Ok("The Counties and Offices in " + TextBoxDistrict.Text + " have been deleted.");

        ClearTextboxes();
        ClearAllCheckboxes();
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonRecord_Click(object sender, EventArgs e)
    {
      try
      {
        #region Check TextBoxes for Illegal Stuff

        Throw_Exception_TextBox_Script(TextBoxDistrictCode);
        Throw_Exception_TextBox_Html(TextBoxDistrictCode);

        Throw_Exception_TextBox_Script(TextBoxDistrict);
        Throw_Exception_TextBox_Html(TextBoxDistrict);

        Throw_Exception_TextBox_Script(TextBoxOfficeTitle);
        Throw_Exception_TextBox_Html(TextBoxOfficeTitle);

        #endregion Check TextBoxes for Illegal Stuff

        Checks4AddOrUpdate();
        CheckValidDistrictCode(TextBoxDistrictCode.Text.Trim(), TextBoxAlphaCode.Text.Trim());

        #region get the District Row

        DataTable districtsTable;
        var message = string.Empty;

        if (Convert.ToInt16(ViewState["DistrictType"]) == ElectoralMultiCounties)
          //Multi-County Districts
        {
          districtsTable = G.Table(MultiCountyDistricts(ViewState["StateCode"].ToString()));
          var districtRow = districtsTable.Rows[Convert.ToInt16(ViewState["CurrentDistrictRow"])];
          RecordMultiCountyCounties(districtRow);

          message += "The Counties comprising " + TextBoxDistrict.Text.Trim() + " was updated.";
          //if (db.MultiCountyDistricts(ViewState["StateCode"].ToString(), LabelDistrictCode.Text, "District")
          if (MultiCountyDistricts(
              ViewState["StateCode"].ToString()
              , TextBoxDistrictCode.Text
              , TextBoxAlphaCode.Text
              , "District")
            != TextBoxDistrict.Text.Trim())
          {
            MultiCountyDistrictsUpdate(
              ViewState["StateCode"].ToString()
              , TextBoxDistrictCode.Text.Trim()
              , TextBoxAlphaCode.Text.Trim()
              , "District"
              , TextBoxDistrict.Text.Trim());
            message += " The District Name was also changed to " + TextBoxDistrict.Text.Trim() + ".";
          }
        }
        else //db.Type_Judicial = Judicial Districts
        {
          districtsTable = G.Table(JudicialDistricts(ViewState["StateCode"].ToString()));
          var districtRow = districtsTable.Rows[Convert.ToInt16(ViewState["CurrentDistrictRow"])];
          RecordJudicialCounties(districtRow);

          message += "The Counties comprising " + TextBoxDistrict.Text.Trim() + " was updated.";
          if (JudicialDistricts(
              ViewState["StateCode"].ToString()
              , TextBoxDistrictCode.Text
              , TextBoxAlphaCode.Text.Trim()
              , "District")
            != TextBoxDistrict.Text.Trim())
          {
            JudicialDistrictsUpdate(
              ViewState["StateCode"].ToString()
              , TextBoxDistrictCode.Text.Trim()
              , TextBoxAlphaCode.Text.Trim()
              , "District"
              , TextBoxDistrict.Text.Trim());
            message += " The District Name was also changed to " + TextBoxDistrict.Text.Trim() + ".";
          }
        }

        #endregion

        //ClearTextboxes();
        //ClearAllCheckboxes();

        Msg.Text = Ok(message);
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
      try
      {
        #region Check TextBoxes for Illegal Stuff

        Throw_Exception_TextBox_Script(TextBoxDistrictCode);
        Throw_Exception_TextBox_Html(TextBoxDistrictCode);

        Throw_Exception_TextBox_Script(TextBoxDistrict);
        Throw_Exception_TextBox_Html(TextBoxDistrict);

        Throw_Exception_TextBox_Script(TextBoxOfficeTitle);
        Throw_Exception_TextBox_Html(TextBoxOfficeTitle);

        #endregion Check TextBoxes for Illegal Stuff

        Checks4AddOrUpdate();

        var currentDistrictName = TextBoxDistrict.Text.Trim();
        var currentDistrictCode = TextBoxDistrictCode.Text.Trim();

        #region DistrictCode

        var districtCode = TextBoxDistrictCode.Text.Trim();
        if (districtCode.Length == 1)
          districtCode = districtCode.Insert(0, "00");
        else if (districtCode.Length == 2)
          districtCode = districtCode.Insert(0, "0");
        CheckIsNewDistrictCode(districtCode);

        #endregion

        #region Inits

        string sqlinsert;
        DataRow districtRow;
        var officeKey = string.Empty;
        var officeAdded = false;

        #endregion

        //if (Convert.ToInt16(ViewState["DistrictType"]) == db.Judicial)
        if (ViewState["DistrictType"].ToOfficeClass() == OfficeClass.StateDistrictMultiCounties)
        {
          #region Add MultiCountyDistricts Row

          sqlinsert = "INSERT INTO MultiCountyDistricts "
            + "("
            + "StateCode"
            + ",DistrictCode"
            + ",DistrictCodeAlpha"
            + ",District"
            + ")"
            + " VALUES("
            + SqlLit(ViewState["StateCode"].ToString())
            + "," + SqlLit(districtCode)
            + "," + SqlLit(TextBoxDistrict.Text.Trim())
            + "," + SqlLit(TextBoxAlphaCode.Text.Trim())
            + ")";
          G.ExecuteSql(sqlinsert);

          ViewState["Districts"] = Rows(MultiCountyDistricts(ViewState["StateCode"].ToString()));

          #endregion

          #region Add MultiCountyDistrictCounties Rows

          districtRow = Row(MultiCountyDistricts(
            ViewState["StateCode"].ToString()
            , districtCode
            , TextBoxAlphaCode.Text.Trim().ToUpper()));

          RecordMultiCountyCounties(districtRow);

          #endregion
        }
        else
        {
          #region Judicial Districts

          #region Add JudicialDistricts Row

          sqlinsert = "INSERT INTO JudicialDistricts "
            + "("
            + "StateCode"
            + ",DistrictCode"
            + ",DistrictCodeAlpha"
            + ",District"
            + ")"
            + " VALUES("
            + SqlLit(ViewState["StateCode"].ToString())
            + "," + SqlLit(districtCode)
            + "," + SqlLit(TextBoxAlphaCode.Text.Trim().ToUpper())
            + "," + SqlLit(TextBoxDistrict.Text.Trim())
            + ")";
          G.ExecuteSql(sqlinsert);

          ViewState["Districts"] = Rows(JudicialDistricts(ViewState["StateCode"].ToString()));

          #endregion

          #region Add JudicialDistrictCounties Row

          districtRow = Row(JudicialDistricts(ViewState["StateCode"].ToString()
            , districtCode
            , TextBoxAlphaCode.Text.Trim().ToUpper()));
          var countiesInDistrictRecorded = RecordJudicialCounties(districtRow);

          #endregion

          #region Add Optional Offices and ElectionsOffices Rows

          if (TextBoxOfficeTitle.Text.Trim() != string.Empty)
          {
            #region Build the OfficeKey

            //OfficeKey = db.OfficeKey(
            //  db.Office_State_District_Multi_Counties_Judicial
            //  , ViewState["StateCode"].ToString()
            //, string.Empty
            //, string.Empty
            //, DistrictRow["DistrictCode"].ToString().ToUpper()
            //, DistrictRow["DistrictCodeAlpha"].ToString().ToUpper()
            //, OffiecTitle);

            #endregion

            //if (db.Row_Optional(db.Sql_Row_Office(OfficeKey)) == null)
            if (!Offices.OfficeKeyExists(officeKey))
            {
              #region Add the Office

              officeAdded = true;

              #endregion

              #region Add ElectionsOffices Row (only for upcoming elections)

              //need to code

              #endregion
            }
          }

          #endregion

          #region Msg

          if (officeAdded)
            Msg.Text = Ok(countiesInDistrictRecorded + " Counties were added for: "
              + districtRow["District"] + "."
              + "<br>And " + TextBoxOfficeTitle.Text.Trim() + " | " + districtRow["District"] +
              " was added to Offices Table.");
          else
            Msg.Text = Ok(countiesInDistrictRecorded + " Counties were added for: "
              + districtRow["District"] + "."
              + "<br>And NO OFFICE was added to Offices Table.");

          #endregion

          #endregion
        }

        #region Make next DistrictCode and DistrictName

        string nextDistrictName;
        string nextDistrictCode;

        int currentDistrictNumber;
        string currentDistrict;

        int nextDistrictCodeNumber;
        string nextDistrict;

        //if (db.Is_Digit(CurrentDistrictCode[0]))//1st char is digit (076 or 01A for District 1A)
        if (char.IsDigit(currentDistrictCode[0])) //1st char is digit (076 or 01A for District 1A)
        {
          if (Is_Valid_Integer(currentDistrictCode)) //3 digit code - 076
          {
            currentDistrictNumber = Convert.ToInt16(currentDistrictCode);
            currentDistrict = currentDistrictNumber.ToString(CultureInfo.InvariantCulture);

            nextDistrictCodeNumber = Convert.ToInt16(currentDistrictCode) + 1;
            nextDistrict = nextDistrictCodeNumber.ToString(CultureInfo.InvariantCulture);

            nextDistrictCode = nextDistrictCodeNumber.ToString(CultureInfo.InvariantCulture);
            if (nextDistrictCode.Length == 1)
              nextDistrictCode = nextDistrictCode.Insert(0, "00");
            else if (nextDistrictCode.Length == 2)
              nextDistrictCode = nextDistrictCode.Insert(0, "0");

            nextDistrictName = currentDistrictName.Replace(currentDistrict, nextDistrict);
          }
          else //like 01A, 22B (District 1A or 22B)
          {
            nextDistrictCode = string.Empty; //next district is probably not 02A or 23B
            nextDistrictName = string.Empty;
          }
        }
        else // like A02, AA2, DAL
        {
          //if (db.Is_Digit(CurrentDistrictCode[1]))// 2nd char is digit = like A02
          if (char.IsDigit(currentDistrictCode[1])) // 2nd char is digit = like A02
          {
            currentDistrictNumber = Convert.ToInt16(currentDistrictCode.Substring(1, 2));
            currentDistrict = currentDistrictNumber.ToString(CultureInfo.InvariantCulture);

            nextDistrictCodeNumber = Convert.ToInt16(currentDistrictNumber) + 1;
            nextDistrict = nextDistrictCodeNumber.ToString(CultureInfo.InvariantCulture);

            nextDistrictCode = nextDistrictCodeNumber.ToString(CultureInfo.InvariantCulture);
            if (nextDistrictCode.Length == 1)
              nextDistrictCode = nextDistrictCode.Insert(0, "0");
            nextDistrictCode = currentDistrictCode[0]
              + nextDistrictCode;

            nextDistrictName = currentDistrictName.Replace(currentDistrict, nextDistrict);
          }
          //else if (db.Is_Digit(CurrentDistrictCode[2]))// 3rd char is digit = 2 Char + digit - AA2
          else if (char.IsDigit(currentDistrictCode[2])) // 3rd char is digit = 2 Char + digit - AA2
          {
            currentDistrictNumber = Convert.ToInt16(currentDistrictCode.Substring(2));
            currentDistrict = currentDistrictNumber.ToString(CultureInfo.InvariantCulture);

            nextDistrictCodeNumber = Convert.ToInt16(currentDistrictNumber) + 1;
            nextDistrict = nextDistrictCodeNumber.ToString(CultureInfo.InvariantCulture);

            nextDistrictCode = currentDistrictCode.Substring(0, 2)
              + nextDistrict;

            nextDistrictName = currentDistrictName.Replace(currentDistrict, nextDistrict);
          }
          else //3 Chars like DAL for Dallas
          {
            nextDistrictCode = string.Empty;
            nextDistrictName = string.Empty;
          }
        }

        #region Next District

        TextBoxDistrictCode.Text = nextDistrictCode;
        TextBoxDistrict.Text = nextDistrictName;

        #endregion

        #endregion

        ClearAllCheckboxes();

        //LabelDistrictCode.Text = string.Empty;
        //TextBoxOfficeTitle is left unchanged intensionally
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonUpdateJudicialDistrict_Click(object sender, EventArgs e)
    {
      try
      {
        Throw_Exception_TextBox_Html_Or_Script(TextBoxDistrict);

        if (Convert.ToInt16(ViewState["DistrictType"]) == ElectoralMultiCounties)
          //Multi-County Districts
          MultiCountyDistrictsUpdate(
            ViewState["StateCode"].ToString()
            , ViewState["DistrictCode"].ToString()
            , ViewState["DistrictCodeAlpha"].ToString()
            , "District"
            , TextBoxDistrict.Text.Trim());
        else
          JudicialDistrictsUpdate(
            ViewState["StateCode"].ToString()
            , ViewState["DistrictCode"].ToString()
            , ViewState["DistrictCodeAlpha"].ToString()
            , "District"
            , TextBoxDistrict.Text.Trim());

        LoadDistrictCodesAndName();

        Msg.Text = Ok("The District was updated.");
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
        Title = H1.InnerText = "Judicial District";

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

        ViewState["StateCode"] = G.State_Code();
        ViewState["CountyCode"] = G.County_Code();
        ViewState["LocalCode"] = G.Local_Code();
        //if (!db.Is_User_Security_Ok())
        //  SecurePage.HandleSecurityException();

        #endregion Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

        try
        {
          ViewState["DistrictCode"] = string.Empty;
          ViewState["DistrictCodeAlpha"] = string.Empty;
          ViewState["DistrictType"] = TypeJudicial; //default

//          #region DEBUG ViewState["DistrictType"] ViewState["DistrictCode"] ViewState["DistrictCodeAlpha"]
//#if DEBUG
//          ViewState["DistrictType"] = db.Type_Judicial;
//          //			ViewState["DistrictType"] = db.Electoral_Multi_Counties;

//          //    ViewState["DistrictCode"] = string.Empty;
//          //ViewState["DistrictCode"] = "001";

//          //    ViewState["DistrictCodeAlpha"] = string.Empty;
//          ViewState["DistrictCodeAlpha"] = "DAL";
//          //    ViewState["DistrictCodeAlpha"] = "A";
//#endif
//          #endregion

          #region ViewState["DistrictType"] ViewState["DistrictCode"] ViewState["DistrictCodeAlpha"]

          if (!string.IsNullOrEmpty(GetQueryString("Type")))
            ViewState["DistrictType"] = GetQueryString("Type");

          if (ViewState["DistrictType"].ToString() == string.Empty)
            throw new ApplicationException("No Office Group was passed as a query string.");


          if (!string.IsNullOrEmpty(GetQueryString("District")))
            ViewState["DistrictCode"] = GetQueryString("District");

          if (!string.IsNullOrEmpty(GetQueryString("DistrictAlpha")))
            ViewState["DistrictCodeAlpha"] = GetQueryString("DistrictAlpha");


          if ((ViewState["DistrictCode"].ToString() == string.Empty)
            && (ViewState["DistrictCodeAlpha"].ToString() == string.Empty))
            throw new ApplicationException(
              "No District and/or DistrictAlpha Codes were passed as a query string.");

          #endregion

          #region checks

          if (Convert.ToInt16(ViewState["DistrictType"]) == TypeJudicial)
          {
            #region Check DistrictCode in DB

            if (!Is_Valid_Judicial_District(ViewState["StateCode"].ToString(),
              ViewState["DistrictCode"].ToString()
              , ViewState["DistrictCodeAlpha"].ToString()))
              throw new ApplicationException("No Judicial District Code in JudicialDistricts Table for State: "
                + ViewState["StateCode"]
                + " DistrictCode: " + ViewState["DistrictCode"]);

            #endregion
          }
          else if (Convert.ToInt16(ViewState["DistrictType"]) == ElectoralMultiCounties)
          {
            #region Not implemented or tested

            //#region Check DistrictCode in DB
            //if (!db.Is_Valid_MultiCounty_District(
            //  ViewState["StateCode"].ToString(),
            //  ViewState["DistrictCode"].ToString()
            //  , ViewState["DistrictCodeAlpha"].ToString()))
            //  throw new ApplicationException("No Multi-County District Code in MultiCountyDistricts Table for State: "
            //    + ViewState["StateCode"].ToString()
            //    + " DistrictCode: " + ViewState["DistrictCode"].ToString());
            //#endregion

            #endregion Not implemented or tested
          }
          else
          {
            throw new ApplicationException(
              "The Office Group was not a Multi-County Judicial Group or Multi-County Group.");
          }

          #endregion checks

          #region Page Title

          if (Convert.ToInt16(ViewState["DistrictType"]) == TypeJudicial)
          {
            LabelPageTitle.Text = StateCache.GetStateName(ViewState["StateCode"].ToString())
              //+ "<br>"
              //+ db.JudicialDistricts(
              //    ViewState["StateCode"].ToString()
              //    , ViewState["DistrictCode"].ToString()
              //    , ViewState["DistrictCodeAlpha"].ToString()
              //    , "District")
              + "<br>Judicial District";
          }
          else if (Convert.ToInt16(ViewState["DistrictType"]) == ElectoralMultiCounties)
          {
            LabelPageTitle.Text = StateCache.GetStateName(ViewState["StateCode"].ToString())
              //+ "<br>"
              //+ db.MultiCountyDistricts(
              //    ViewState["StateCode"].ToString()
              //    , ViewState["DistrictCode"].ToString()
              //    , ViewState["DistrictCodeAlpha"].ToString()
              //    , "District")
              + "<br>Multi-County District";
          }

          #endregion Page Title

          if (IsMasterUser)
          {
            #region Controls for MASTER users

            TableMaster1.Visible = true;
            TableMaster4.Visible = true;
            TableMaster5.Visible = true;
            TableMaster6.Visible = true;
            TableMaster7.Visible = true;
            TableMaster8.Visible = true;

            SetControls4MasterUser();

            #endregion Controls for MASTER users

            #region Create County checkboxes

            var countiesTable = G.Table(Counties(ViewState["StateCode"].ToString()));
            foreach (DataRow countyRow in countiesTable.Rows)
            {
              var onlyCountyName = countyRow["County"].ToString();
              onlyCountyName = "  " + onlyCountyName.Replace("County", string.Empty).Trim() + "  ";
              CheckBoxListCounties.Items.Add(onlyCountyName);
            }

            #endregion Create County checkboxes

            //ViewState["CurrentDistrictRow"] = -1;

            #region commented out Load Textboxes

            //DataRow DistrictRow = db.Row_Last_Optional(sql.JudicialDistricts(
            //  ViewState["StateCode"].ToString()
            //,ViewState["DistrictCode"].ToString()
            //,ViewState["DistrictCodeAlpha"].ToString()));
            //if(DistrictRow != null)
            //{
            //TextBoxDistrictCode.Text = DistrictRow["DistrictCode"].ToString();
            //TextBoxDistrict.Text = DistrictRow["District"].ToString();
            //TextBoxAlphaCode.Text = DistrictRow["DistrictCodeAlpha"].ToString();
            //}
            //else
            //{
            //  Msg.Text = db.Fail("This District does not exist in the database.");
            //}

            #endregion Load Textboxes

            TextBoxDistrictCode.Text = ViewState["DistrictCode"].ToString();
            TextBoxAlphaCode.Text = ViewState["DistrictCodeAlpha"].ToString();

            GetDistrictDataAndCounties();
          }
          else
          {
            #region Controls for ADMIN users

            TableMaster1.Visible = false;
            TableMaster4.Visible = false;
            TableMaster5.Visible = false;
            TableMaster6.Visible = false;
            TableMaster7.Visible = false;
            TableMaster8.Visible = false;

            LoadDistrictCodesAndName();

            #endregion Controls for ADMIN users
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
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
      try
      {
        if (Session["ErrNavBarAdmin"].ToString() != string.Empty)
          Msg.Text = Fail(Session["ErrNavBarAdmin"].ToString());
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch /*(Exception ex)*/
      {
      }
    }
  }
}