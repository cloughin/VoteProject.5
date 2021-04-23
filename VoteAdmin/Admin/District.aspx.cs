using System;
using System.Data;
using System.Globalization;
using DB.Vote;

namespace Vote.Admin
{
  public partial class DistrictPage : SecureAdminPage, IAllowEmptyStateCode
  {
    #region from db

    private const int TypeJudicial = 5;
    //private const int ElectoralMultiCounties = 3;

    public static string JudicialDistrictCounties(string stateCode, string districtCode,
      string districtCodeAlpha)
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

    //public static string MultiCountyDistrictCountiesDelete(string stateCode, string districtCode,
    //  string districtCodeAlpha)
    //{
    //  var sql = string.Empty;
    //  sql += "DELETE FROM MultiCountyDistrictCounties";
    //  sql += " WHERE StateCode = " + SqlLit(stateCode);
    //  sql += " AND DistrictCode = " + SqlLit(districtCode);
    //  sql += " AND DistrictCodeAlpha = " + SqlLit(districtCodeAlpha);
    //  return sql;
    //}

    //public static string MultiCountyDistrictCounties4StateCodeMultiCountyDistrictCode(
    //  string stateCode, string districtCode, string districtCodeAlpha)
    //{
    //  var sql = string.Empty;
    //  sql += " SELECT ";
    //  sql += " StateCode ";
    //  sql += " ,DistrictCode ";
    //  sql += " ,DistrictCodeAlpha ";
    //  sql += " ,District";
    //  sql += " ,CountyCode ";
    //  sql += " FROM MultiCountyDistrictCounties ";
    //  sql += " WHERE StateCode = " + SqlLit(stateCode);
    //  sql += " AND DistrictCode = " + SqlLit(districtCode);
    //  sql += " AND DistrictCodeAlpha = " + SqlLit(districtCodeAlpha);
    //  return sql;
    //}

    //public static string MultiCountyDistricts(string stateCode, string districtCode,
    //  string districtCodeAlpha)
    //{
    //  var sql = string.Empty;
    //  sql += " SELECT ";
    //  sql += " StateCode ";
    //  sql += " ,DistrictCode ";
    //  sql += " ,DistrictCodeAlpha";
    //  sql += " ,District ";
    //  sql += " FROM MultiCountyDistricts ";
    //  sql += " WHERE StateCode = " + SqlLit(stateCode);
    //  sql += " AND DistrictCode = " + SqlLit(districtCode);
    //  sql += " AND DistrictCodeAlpha = " + SqlLit(districtCodeAlpha);
    //  return sql;
    //}

    //public static string MultiCountyDistricts(string stateCode)
    //{
    //  var sql = string.Empty;
    //  sql += " SELECT ";
    //  sql += " StateCode ";
    //  sql += " ,DistrictCode ";
    //  sql += " ,DistrictCodeAlpha";
    //  sql += " ,District ";
    //  sql += " FROM MultiCountyDistricts ";
    //  sql += " WHERE StateCode = " + SqlLit(stateCode);
    //  sql += " ORDER BY DistrictCode";
    //  return sql;
    //}

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

    private static string Ok(string msg)
    {
      return $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";
    }

    private static string Fail(string msg)
    {
      return $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";
    }

    public static string Message(string msg)
    {
      return $"<span class=\"Msg\">{msg}</span>";
    }

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    public static string DbErrorMsg(string sql, string err)
    {
      return $"Database Failure for SQL Statement::{sql} :: Error Msg:: {err}";
    }

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
      return table.Rows.Count == 1 ? table.Rows[0] : null;
    }

    public static void Triple_Key_Update_Str(string table, string column, string columnValue,
      string keyName1, string keyValue1, string keyName2, string keyValue2, string keyName3,
      string keyValue3)
    {
      var updateSql = "UPDATE " + table + " SET " + column + " = " + SqlLit(columnValue) + " WHERE " +
        keyName1 + " = " + SqlLit(keyValue1) + " AND " + keyName2 + " = " + SqlLit(keyValue2) +
        " AND " + keyName3 + " = " + SqlLit(keyValue3);
      G.ExecuteSql(updateSql);
    }

    private static bool Is_Valid_Judicial_District(string stateCode, string districtCode,
        string districtCodeAlpha)
    {
      return Row_Optional(JudicialDistricts(stateCode, districtCode, districtCodeAlpha)) != null;
    }

    //private static bool Is_Valid_MultiCounty_District(string stateCode, string districtCode,
    //    string districtCodeAlpha)
    //{
    //  return Row_Optional(MultiCountyDistricts(stateCode, districtCode, districtCodeAlpha)) != null;
    //}

    //private static string MultiCountyDistricts(string stateCode, string districtCode,
    //    string districtCodeAlpha, string column)
    //  =>
    //  Triple_Key_Str("MultiCountyDistricts", column, "StateCode", stateCode, "DistrictCode",
    //    districtCode, "DistrictCodeAlpha", districtCodeAlpha);

    //private static void MultiCountyDistrictsUpdate(string stateCode, string districtCode,
    //    string districtCodeAlpha, string column, string columnValue)
    //  =>
    //  Triple_Key_Update_Str("MultiCountyDistricts", column, columnValue, "StateCode", stateCode,
    //    "DistrictCode", districtCode, "DistrictCodeAlpha", districtCodeAlpha);

    private static string Triple_Key_Str(string tableName, string column, string keyName1,
      string keyValue1, string keyName2, string keyValue2, string keyName3, string keyValue3)
    {
      var sql = "SELECT " + column.Trim() + " FROM " + tableName.Trim() + " WHERE " +
        keyName1.Trim() + " = " + SqlLit(keyValue1.Trim()) + " AND " + keyName2.Trim() + " = " +
        SqlLit(keyValue2.Trim()) + " AND " + keyName3.Trim() + " = " + SqlLit(keyValue3.Trim());
      var table = G.Table(sql);
      if (table.Rows.Count == 1)
        return table.Rows[0][column].ToString().Trim();
      throw new ApplicationException(DbErrorMsg(sql, "Did not find a single row"));
    }

    private static string JudicialDistricts(string stateCode, string districtCode,
        string districtCodeAlpha, string column)
      =>
      Triple_Key_Str("JudicialDistricts", column, "StateCode", stateCode, "DistrictCode",
        districtCode, "DistrictCodeAlpha", districtCodeAlpha);

    private static void JudicialDistrictsUpdate(string stateCode, string districtCode,
        string districtCodeAlpha, string column, string columnValue)
      =>
      Triple_Key_Update_Str("JudicialDistricts", column, columnValue, "StateCode", stateCode,
        "DistrictCode", districtCode, "DistrictCodeAlpha", districtCodeAlpha);

    #endregion from db

    private int _DistrictType;
    private string _DistrictCode;
    private string _DistrictCodeAlpha;
    private int _Districts;
    private int _CurrentDistrictRow;

    private void LoadDistrictCodesAndName()
    {
      if (_DistrictType == TypeJudicial)
      {
        TextBoxDistrictCode.Text = JudicialDistricts(StateCode, _DistrictCode,
          _DistrictCodeAlpha, "DistrictCode");
        TextBoxAlphaCode.Text = JudicialDistricts(StateCode, _DistrictCode,
          _DistrictCodeAlpha, "DistrictCodeAlpha");
        TextBoxDistrict.Text = JudicialDistricts(StateCode, _DistrictCode,
          _DistrictCodeAlpha, "District");
      }
      //else if (_DistrictType == ElectoralMultiCounties)
      //{
      //  TextBoxDistrictCode.Text = MultiCountyDistricts(StateCode, _DistrictCode,
      //    _DistrictCodeAlpha, "DistrictCode");
      //  TextBoxAlphaCode.Text = MultiCountyDistricts(StateCode, _DistrictCode,
      //    _DistrictCodeAlpha, "DistrictCodeAlpha");
      //  TextBoxDistrict.Text = MultiCountyDistricts(StateCode, _DistrictCode,
      //    _DistrictCodeAlpha, "District");
      //}
    }

    private void DistrictRetrievedMsg()
      =>
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
        if (!TextBoxDistrictCode.Text.Trim().IsDigits())
          throw new ApplicationException("The District Code must be all digits.");
      }

      if (TextBoxAlphaCode.Text.Trim() != string.Empty)
      {
        if (TextBoxAlphaCode.Text.Trim().Length > 4)
          throw new ApplicationException(
            "When the District Alpha Code is present it must less than 4 characters.");
        if (!TextBoxAlphaCode.Text.Trim().IsLetters())
          throw new ApplicationException("The Alpha Code must be all alphabetic.");
      }

      if (_DistrictType == TypeJudicial)
      {
        if (
          !Is_Valid_Judicial_District(StateCode, districtCode,
            districtCodeAlpha.Trim().ToUpper()))
          throw new ApplicationException("The District Code(s) is invalid.");
      }
      //else //Multi-County Districts
      //{
      //  if (
      //    !Is_Valid_MultiCounty_District(StateCode, districtCode,
      //      districtCodeAlpha.Trim().ToUpper()))
      //    throw new ApplicationException("The District Code(s) is invalid.");
      //}
    }

    private void CheckIsNewDistrictCode(string districtCode)
    {
      //if (_DistrictType == ElectoralMultiCounties)
      //  //Multi-County Districts
      //{
      //  if (Is_Valid_MultiCounty_District(StateCode, districtCode,
      //    TextBoxAlphaCode.Text.Trim().ToUpper()))
      //    throw new ApplicationException("The District for this District Code already exists.");
      //}
      //else
      {
        if (Is_Valid_Judicial_District(StateCode, districtCode,
          TextBoxAlphaCode.Text.Trim().ToUpper()))
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
      if (TextBoxDistrictCode.Text.Trim() == string.Empty)
        throw new ApplicationException(
          "No District has been retrieved. You need to get the District before you can delete it.");
    }

    private void ClearAllCheckboxes()
    {
      var countiesTable = G.Table(Counties(StateCode));

      for (var inx = 0; inx < countiesTable.Rows.Count; inx++)
        CheckBoxListCounties.Items[inx].Selected = false;
    }

    private void ClearTextboxes()
    {
      TextBoxDistrictCode.Text = string.Empty;
      TextBoxDistrict.Text = string.Empty;
      TextBoxOfficeTitle.Text = string.Empty;
    }

    private int CheckboxesChecked()
    {
      var countiesTable = G.Table(Counties(StateCode));
      var countiesChecked = 0;

      for (var inx = 0; inx < countiesTable.Rows.Count; inx++)
        if (CheckBoxListCounties.Items[inx].Selected)
          countiesChecked++;


      return countiesChecked;
    }

    private void SetControlsForMasterUser()
    {
      //if (_DistrictType == ElectoralMultiCounties)
      //{
      //  HyperLinkAddMultiCountyDistricts.Visible = true;
      //  HyperLinkAddJudicialDistricts.Visible = false;
      //  ViewState["Districts"] = _Districts = Rows(MultiCountyDistricts(StateCode));
      //}
      //else
      {
        //HyperLinkAddMultiCountyDistricts.Visible = false;
        //HyperLinkAddJudicialDistricts.Visible = true;
        ViewState["Districts"] = _Districts = Rows(JudicialDistricts(StateCode));
      }
    }

    private int RecordJudicialCounties(DataRow judicialDistrictRow)
    {
      var countiesTable = G.Table(Counties(StateCode));
      var countyIndex = 0;
      var countiesInDistrictRecorded = 0;

      G.ExecuteSql(JudicialDistrictCounties_DELETE(StateCode,
        judicialDistrictRow["DistrictCode"].ToString()));

      foreach (DataRow countyRow in countiesTable.Rows)
      {
        if (CheckBoxListCounties.Items[countyIndex].Selected)
        {
          var sqlinsert = "INSERT INTO JudicialDistrictCounties " + "(" + "StateCode" +
            ",DistrictCode" + ",DistrictCodeAlpha" + ",CountyCode" + ")" + " VALUES(" +
            SqlLit(StateCode) + "," +
            SqlLit(judicialDistrictRow["DistrictCode"].ToString()) + "," +
            SqlLit(judicialDistrictRow["DistrictCodeAlpha"].ToString()) + "," +
            SqlLit(countyRow["CountyCode"].ToString()) + ")";
          G.ExecuteSql(sqlinsert);
          countiesInDistrictRecorded++;
        }
        countyIndex++;
      }

      return countiesInDistrictRecorded;
    }

    //private void RecordMultiCountyCounties(DataRow multiCountyDistrictRow)
    //{
    //  var countiesTable = G.Table(Counties(StateCode));
    //  var countyIndex = 0;

    //  #region Delete all counties

    //  G.ExecuteSql(MultiCountyDistrictCountiesDelete(StateCode,
    //    multiCountyDistrictRow["DistrictCode"].ToString(),
    //    multiCountyDistrictRow["DistrictCodeAlpha"].ToString()));

    //  #endregion

    //  #region add new counties

    //  foreach (DataRow countyRow in countiesTable.Rows)
    //  {
    //    if (CheckBoxListCounties.Items[countyIndex].Selected)
    //    {
    //      #region Insert the MultiCountyDistrictCounties Rows

    //      var sqlinsert = "INSERT INTO MultiCountyDistrictCounties " + "(" + "StateCode" +
    //        ",DistrictCode" + ",DistrictCodeAlpha" + ",CountyCode" + ")" + " VALUES(" +
    //        SqlLit(StateCode) + "," +
    //        SqlLit(multiCountyDistrictRow["DistrictCode"].ToString()) + "," +
    //        SqlLit(multiCountyDistrictRow["DistrictCodeAlpha"].ToString()) + "," +
    //        SqlLit(countyRow["CountyCode"].ToString()) + ")";
    //      G.ExecuteSql(sqlinsert);

    //      #endregion
    //    }
    //    countyIndex++;
    //  }

    //  #endregion

    //  //ClearAllCheckboxes();
    //}

    //private void LoadCheckboxes4MultiCountyDistrict()
    //{
    //  var districtsTable = G.Table(MultiCountyDistricts(StateCode));
    //  var districtRow = districtsTable.Rows[_CurrentDistrictRow];

    //  if (_DistrictType == TypeJudicial)
    //  {
    //    var districtCountiesTable = G.Table(JudicialDistrictCounties(StateCode,
    //      districtRow["DistrictCode"].ToString(), districtRow["DistrictCodeAlpha"].ToString()));
    //    foreach (DataRow districtCountiesRow in districtCountiesTable.Rows)
    //    {
    //      var countiesTable = G.Table(StateCode);
    //      var countyIndex = 0;
    //      foreach (DataRow countyRow in countiesTable.Rows)
    //      {
    //        if (countyRow["CountyCode"].ToString() == districtCountiesRow["CountyCode"].ToString())
    //        {
    //          CheckBoxListCounties.Items[countyIndex].Selected = true;
    //        }
    //        countyIndex++;
    //      }
    //    }
    //  }
    //  //else
    //  //{
    //  //  //not tested
    //  //  var districtCountiesTable =
    //  //    G.Table(
    //  //      MultiCountyDistrictCounties4StateCodeMultiCountyDistrictCode(
    //  //        StateCode, districtRow["DistrictCode"].ToString(),
    //  //        districtRow["DistrictCodeAlpha"].ToString()));
    //  //foreach (DataRow districtCountiesRow in districtCountiesTable.Rows)
    //  //{
    //  //  var countiesTable = G.Table(StateCode);
    //  //  var countyIndex = 0;
    //  //  foreach (DataRow countyRow in countiesTable.Rows)
    //  //  {
    //  //    if (countyRow["CountyCode"].ToString() == districtCountiesRow["CountyCode"].ToString())
    //  //    {
    //  //      CheckBoxListCounties.Items[countyIndex].Selected = true;
    //  //    }
    //  //    countyIndex++;
    //  //  }
    //  //}
    //  //}
    //}

    private void LoadCheckboxes4JudicialDistrict()
    {
      var districtsTable = G.Table(JudicialDistricts(StateCode));
      var districtRow = districtsTable.Rows[_CurrentDistrictRow];
      var districtCountiesTable =
        G.Table(JudicialDistrictCounties(StateCode,
          districtRow["DistrictCode"].ToString(), districtRow["DistrictCodeAlpha"].ToString()));
      foreach (DataRow districtCountiesRow in districtCountiesTable.Rows)
      {
        var countiesTable = G.Table(Counties(StateCode));
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

    //private void LoadMultiCountyDistrict()
    //{
    //  #region get the next District Row

    //  var districtsTable = G.Table(MultiCountyDistricts(StateCode));
    //  if (districtsTable.Rows.Count < _CurrentDistrictRow + 1)
    //    throw new ApplicationException("There are no more Multi County Districts.");
    //  var districtRow = districtsTable.Rows[_CurrentDistrictRow];

    //  #endregion

    //  TextBoxDistrictCode.Text = districtRow["DistrictCode"].ToString();
    //  TextBoxAlphaCode.Text = districtRow["DistrictCodeAlpha"].ToString();
    //  TextBoxDistrict.Text = districtRow["District"].ToString();
    //}

    private void LoadJudicialDistrict()
    {
      #region get the District Row

      var districtsTable = G.Table(JudicialDistricts(StateCode));
      if (districtsTable.Rows.Count < _CurrentDistrictRow + 1)
        throw new ApplicationException("There are no more Judicial Districts.");
      var districtRow = districtsTable.Rows[_CurrentDistrictRow];

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
        if (_CurrentDistrictRow < _Districts - 1)
        {
          ViewState["CurrentDistrictRow"] = _CurrentDistrictRow = _CurrentDistrictRow + 1;

          //if (_DistrictType == ElectoralMultiCounties)
          //  //Multi-County Districts
          //{
          //  LoadMultiCountyDistrict();
          //  ClearAllCheckboxes();
          //  LoadCheckboxes4MultiCountyDistrict();
          //}
          //else // Judicial Districts
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
        LogAdminError(ex);

        #endregion
      }
    }

    protected void ButtonGetPrevious_Click(object sender, EventArgs e)
    {
      try
      {
        if (_CurrentDistrictRow > 0)
        {
          ViewState["CurrentDistrictRow"] = _CurrentDistrictRow = _CurrentDistrictRow - 1;

          //if (_DistrictType == ElectoralMultiCounties)
          //  //Multi-County Districts
          //{
          //  LoadMultiCountyDistrict();
          //  ClearAllCheckboxes();
          //  LoadCheckboxes4MultiCountyDistrict();
          //}
          //else // Judicial Districts
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
        LogAdminError(ex);

        #endregion
      }
    }

    private void GetDistrictDataAndCounties()
    {
      CheckValidDistrictCode(TextBoxDistrictCode.Text.Trim(), TextBoxAlphaCode.Text.Trim());

      var index = 0;
      var found = false;

      if (_DistrictType == TypeJudicial)
      {
        var districtsTable = G.Table(JudicialDistricts(StateCode));
        foreach (DataRow districtRow in districtsTable.Rows)
        {
          if ((districtRow["DistrictCode"].ToString() == TextBoxDistrictCode.Text.ToUpper()) &&
            (districtRow["DistrictCodeAlpha"].ToString() == TextBoxAlphaCode.Text.ToUpper().Trim()) &&
            !found)
          {
            found = true;
            ViewState["CurrentDistrictRow"] = _CurrentDistrictRow = index;
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
        LogAdminError(ex);

        #endregion
      }
    }

    protected void ButtonDelete_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(TextBoxDistrictCode, TextBoxDistrict, TextBoxOfficeTitle);

        Checks4Delete();

        //if (_DistrictType == ElectoralMultiCounties)
        //  //Multi-County Districts
        //{
        //  //sqldelete = "DELETE FROM MultiCountyDistrictCounties" + " WHERE DistrictCode = " +
        //  //  SqlLit(TextBoxDistrictCode.Text) + " AND DistrictCodeAlpha = " +
        //  //  SqlLit(TextBoxAlphaCode.Text);
        //  //G.ExecuteSql(sqldelete);

        //  sqldelete = "DELETE FROM Offices" + " WHERE StateCode = " +
        //    SqlLit(StateCode) + " AND DistrictCode = " +
        //    SqlLit(TextBoxDistrictCode.Text) + " AND DistrictCodeAlpha = " +
        //    SqlLit(TextBoxAlphaCode.Text) + " AND OfficeLevel = " + ElectoralMultiCounties;
        //  G.ExecuteSql(sqldelete);

        //  sqldelete = "DELETE FROM MultiCountyDistricts" + " WHERE StateCode = " +
        //    SqlLit(StateCode) + " AND DistrictCode = " +
        //    SqlLit(TextBoxDistrictCode.Text) + " AND DistrictCodeAlpha = " +
        //    SqlLit(TextBoxAlphaCode.Text);
        //  G.ExecuteSql(sqldelete);
        //}
        //else // Judicial Districts
        {
          var sqldelete = "DELETE FROM JudicialDistrictCounties" + " WHERE DistrictCode = " +
            SqlLit(TextBoxDistrictCode.Text) + " AND DistrictCodeAlpha = " +
            SqlLit(TextBoxAlphaCode.Text);
          G.ExecuteSql(sqldelete);

          //sqldelete = "DELETE FROM Offices" + " WHERE StateCode = " +
          //  SqlLit(StateCode) + " AND DistrictCode = " +
          //  SqlLit(TextBoxDistrictCode.Text) + " AND DistrictCodeAlpha = " +
          //  SqlLit(TextBoxAlphaCode.Text) + " AND OfficeLevel = " + ElectoralMultiCounties;
          //G.ExecuteSql(sqldelete);

          sqldelete = "DELETE FROM JudicialDistricts" + " WHERE StateCode = " +
            SqlLit(StateCode) + " AND DistrictCode = " +
            SqlLit(TextBoxDistrictCode.Text) + " AND DistrictCodeAlpha = " +
            SqlLit(TextBoxAlphaCode.Text);
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
        LogAdminError(ex);

        #endregion
      }
    }

    protected void ButtonRecord_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(TextBoxDistrictCode, TextBoxDistrict, TextBoxOfficeTitle);

        Checks4AddOrUpdate();
        CheckValidDistrictCode(TextBoxDistrictCode.Text.Trim(), TextBoxAlphaCode.Text.Trim());

        #region get the District Row

        var message = string.Empty;

        //if (_DistrictType == ElectoralMultiCounties)
        //  //Multi-County Districts
        //{
        //  districtsTable = G.Table(MultiCountyDistricts(StateCode));
        //  var districtRow = districtsTable.Rows[_CurrentDistrictRow];
        //  RecordMultiCountyCounties(districtRow);

        //  message += "The Counties comprising " + TextBoxDistrict.Text.Trim() + " was updated.";
        //  if (
        //    MultiCountyDistricts(StateCode, TextBoxDistrictCode.Text,
        //      TextBoxAlphaCode.Text, "District") != TextBoxDistrict.Text.Trim())
        //  {
        //    MultiCountyDistrictsUpdate(StateCode,
        //      TextBoxDistrictCode.Text.Trim(), TextBoxAlphaCode.Text.Trim(), "District",
        //      TextBoxDistrict.Text.Trim());
        //    message += " The District Name was also changed to " + TextBoxDistrict.Text.Trim() + ".";
        //  }
        //}
        //else //db.Type_Judicial = Judicial Districts
        {
          var districtsTable = G.Table(JudicialDistricts(StateCode));
          var districtRow = districtsTable.Rows[_CurrentDistrictRow];
          RecordJudicialCounties(districtRow);

          message += "The Counties comprising " + TextBoxDistrict.Text.Trim() + " was updated.";
          if (
            JudicialDistricts(StateCode, TextBoxDistrictCode.Text,
              TextBoxAlphaCode.Text.Trim(), "District") != TextBoxDistrict.Text.Trim())
          {
            JudicialDistrictsUpdate(StateCode,
              TextBoxDistrictCode.Text.Trim(), TextBoxAlphaCode.Text.Trim(), "District",
              TextBoxDistrict.Text.Trim());
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
        LogAdminError(ex);

        #endregion
      }
    }

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(TextBoxDistrictCode, TextBoxDistrict, TextBoxOfficeTitle);

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

        var officeKey = string.Empty;
        var officeAdded = false;

        #endregion

        if (_DistrictType.ToOfficeClass() == OfficeClass.StateDistrictMultiCounties)
        {
          //#region Add MultiCountyDistricts Row

          //sqlinsert = "INSERT INTO MultiCountyDistricts " + "(" + "StateCode" + ",DistrictCode" +
          //  ",DistrictCodeAlpha" + ",District" + ")" + " VALUES(" +
          //  SqlLit(StateCode) + "," + SqlLit(districtCode) + "," +
          //  SqlLit(TextBoxDistrict.Text.Trim()) + "," + SqlLit(TextBoxAlphaCode.Text.Trim()) + ")";
          //G.ExecuteSql(sqlinsert);

          //ViewState["Districts"] = _Districts = Rows(MultiCountyDistricts(StateCode));

          //#endregion

          //#region Add MultiCountyDistrictCounties Rows

          //districtRow =
          //  Row(MultiCountyDistricts(StateCode, districtCode,
          //    TextBoxAlphaCode.Text.Trim().ToUpper()));

          //RecordMultiCountyCounties(districtRow);

          //#endregion
        }
        else
        {
          #region Judicial Districts

          #region Add JudicialDistricts Row

          var sqlinsert = "INSERT INTO JudicialDistricts " + "(" + "StateCode" + ",DistrictCode" +
            ",DistrictCodeAlpha" + ",District" + ")" + " VALUES(" +
            SqlLit(StateCode) + "," + SqlLit(districtCode) + "," +
            SqlLit(TextBoxAlphaCode.Text.Trim().ToUpper()) + "," +
            SqlLit(TextBoxDistrict.Text.Trim()) + ")";
          G.ExecuteSql(sqlinsert);

          ViewState["Districts"] = _Districts = Rows(JudicialDistricts(StateCode));

          #endregion

          #region Add JudicialDistrictCounties Row

          var districtRow = Row(JudicialDistricts(StateCode, districtCode,
            TextBoxAlphaCode.Text.Trim().ToUpper()));
          var countiesInDistrictRecorded = RecordJudicialCounties(districtRow);

          #endregion

          #region Add Optional Offices and ElectionsOffices Rows

          if (TextBoxOfficeTitle.Text.Trim() != string.Empty)
          {
            if (!Offices.OfficeKeyExists(officeKey))
            {
              officeAdded = true;

              #region Add ElectionsOffices Row (only for upcoming elections)

              //need to code

              #endregion
            }
          }

          #endregion

          #region Msg

          if (officeAdded)
            Msg.Text =
              Ok(countiesInDistrictRecorded + " Counties were added for: " + districtRow["District"] +
                "." + "<br>And " + TextBoxOfficeTitle.Text.Trim() + " | " + districtRow["District"] +
                " was added to Offices Table.");
          else
            Msg.Text =
              Ok(countiesInDistrictRecorded + " Counties were added for: " + districtRow["District"] +
                "." + "<br>And NO OFFICE was added to Offices Table.");

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
          if (currentDistrictCode.IsValidInteger()) //3 digit code - 076
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
            nextDistrictCode = currentDistrictCode[0] + nextDistrictCode;

            nextDistrictName = currentDistrictName.Replace(currentDistrict, nextDistrict);
          }
          //else if (db.Is_Digit(CurrentDistrictCode[2]))// 3rd char is digit = 2 Char + digit - AA2
          else if (char.IsDigit(currentDistrictCode[2])) // 3rd char is digit = 2 Char + digit - AA2
          {
            currentDistrictNumber = Convert.ToInt16(currentDistrictCode.Substring(2));
            currentDistrict = currentDistrictNumber.ToString(CultureInfo.InvariantCulture);

            nextDistrictCodeNumber = Convert.ToInt16(currentDistrictNumber) + 1;
            nextDistrict = nextDistrictCodeNumber.ToString(CultureInfo.InvariantCulture);

            nextDistrictCode = currentDistrictCode.Substring(0, 2) + nextDistrict;

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
        LogAdminError(ex);

        #endregion
      }
    }

    protected void ButtonUpdateJudicialDistrict_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput(TextBoxDistrict);

        //if (_DistrictType == ElectoralMultiCounties)
        //  MultiCountyDistrictsUpdate(StateCode,
        //    _DistrictCode, _DistrictCodeAlpha,
        //    "District", TextBoxDistrict.Text.Trim());
        //else
          JudicialDistrictsUpdate(StateCode,
            _DistrictCode, _DistrictCodeAlpha,
            "District", TextBoxDistrict.Text.Trim());

        LoadDistrictCodesAndName();

        Msg.Text = Ok("The District was updated.");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var queryType = GetQueryString("Type");
      _DistrictType = queryType.IsValidInteger() ? int.Parse(queryType) : TypeJudicial;
      _DistrictCode = GetQueryString("District");
      _DistrictCodeAlpha = GetQueryString("DistrictAlpha");

      if (string.IsNullOrEmpty(_DistrictCode) && string.IsNullOrEmpty(_DistrictCodeAlpha))
        throw new ApplicationException(
          "No District and/or DistrictAlpha Codes were passed as a query string.");

      if (IsPostBack)
      {
        var districts = ViewState["Districts"];
        if (districts is int)
          _Districts = (int) districts;
        var currentDistrictRow = ViewState["CurrentDistrictRow"];
        if (currentDistrictRow is int)
          _CurrentDistrictRow = (int) currentDistrictRow;
      }
      else
      {
        Title = H1.InnerText = "Judicial District";

        try
        {
          #region checks

          if (_DistrictType == TypeJudicial)
          {
            #region Check DistrictCode in DB

            if (
              !Is_Valid_Judicial_District(StateCode,
                _DistrictCode, _DistrictCodeAlpha))
              throw new ApplicationException(
                "No Judicial District Code in JudicialDistricts Table for State: " +
                StateCode + " DistrictCode: " + _DistrictCode);

            #endregion
          }
          //else if (_DistrictType == ElectoralMultiCounties)
          //{
          //}
          else
          {
            throw new ApplicationException(
              "The Office Group was not a Multi-County Judicial Group or Multi-County Group.");
          }

          #endregion checks

          #region Page Title

          if (_DistrictType == TypeJudicial)
          {
            LabelPageTitle.Text = StateCache.GetStateName(StateCode)
              + "<br>Judicial District";
          }
          //else if (_DistrictType == ElectoralMultiCounties)
          //{
          //  LabelPageTitle.Text = StateCache.GetStateName(StateCode)
          //    + "<br>Multi-County District";
          //}

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

            SetControlsForMasterUser();

            #endregion Controls for MASTER users

            var countiesTable = G.Table(Counties(StateCode));
            foreach (DataRow countyRow in countiesTable.Rows)
            {
              var onlyCountyName = countyRow["County"].ToString();
              onlyCountyName = "  " + onlyCountyName.Replace("County", string.Empty).Trim() + "  ";
              CheckBoxListCounties.Items.Add(onlyCountyName);
            }

            TextBoxDistrictCode.Text = _DistrictCode;
            TextBoxAlphaCode.Text = _DistrictCode;

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
          LogAdminError(ex);

          #endregion
        }
      }
    }
  }
}