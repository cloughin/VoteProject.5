using System;
using System.Data;
using System.Globalization;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Master
{
  public partial class DefaultPage : SecurePage
  {
    #region from db

    public static string Ok(string msg) => $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";

    public static string Fail(string msg) => $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";

    public static string Message(string msg) => $"<span class=\"Msg\">{msg}</span>";

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

    //public static bool Is_TextBox_Html(TextBox textBox)
    //{
    //  return (textBox.Text.IndexOf("<", StringComparison.Ordinal) >= 0)
    //    || (textBox.Text.IndexOf("/>", StringComparison.Ordinal) >= 0);
    //}

    //public static bool Is_Str_Script(string strToCheck)
    //{
    //  return strToCheck.Trim().ToUpper().IndexOf("<SCRIPT", StringComparison.Ordinal) >= 0;
    //}

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    public static string DbErrorMsg(string sql, string err)
    {
      //Write code to log database errors to a DBFailues Table
      return "Database Failure for SQL Statement::" + sql + " :: Error Msg:: " + err;
    }

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

    public static DataRow Row_Optional(string sql)
    {
      var table = G.Table(sql);
      return table.Rows.Count == 1
        ? table.Rows[0]
        : null;
    }

    public static DataRow Row_First_Optional(string sql)
    {
      var table = G.Table(sql);
      return table.Rows.Count >= 1
        ? table.Rows[0]
        : null;
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

    //public static void Single_Key_Update_Str(string table, string column, string columnValue,
    //  string keyName, string keyValue)
    //{
    //  Db.UpdateColumnByKey(table, column, columnValue, keyName, keyValue);
    //}

    //public static void Politicians_Update_Str(string politicianKey, string column, string columnValue)
    //{
    //  Single_Key_Update_Str("Politicians", column, columnValue, "PoliticianKey", politicianKey);
    //}

    public static string ElectionKey_State(string electionKey)
    {
      if (
        !string.IsNullOrEmpty(electionKey)
        && (electionKey.Length >= Elections.ElectionKeyLengthStateOrFederal)
      )
        return electionKey.Substring(0, Elections.ElectionKeyLengthStateOrFederal);
      return string.Empty;
    }

    public static void ElectionsOffices_Update_DistrictCode(string officeKey, string districtCode)
    {
      var sqlupdate = "UPDATE ElectionsOffices"
        + " SET DistrictCode =" + SqlLit(districtCode)
        + " WHERE OfficeKey =" + SqlLit(officeKey);
      G.ExecuteSql(sqlupdate);
    }

    //public static void Throw_Exception_TextBox_Html(TextBox textBox)
    //{
    //  if (Is_TextBox_Html(textBox))
    //    throw new ApplicationException(
    //      "Text in a textbox appears to be HTML because it contains an opening or closing HTML tag (< or />). Please remove and try again.");
    //}

    //public static void Throw_Exception_TextBox_Script(TextBox textBox)
    //{
    //  if (Is_Str_Script(textBox.Text))
    //    throw new ApplicationException("Text in the textbox is illegal.");
    //}

    //public static void Throw_Exception_TextBox_Html_Or_Script(TextBox textBox)
    //{
    //  Throw_Exception_TextBox_Html(textBox);
    //  Throw_Exception_TextBox_Script(textBox);
    //}

    public static bool Master_Bool(string column)
    {
      return Single_Key_Bool("Master", column, "ID", "Master");
    }

    public static string ElectionKey_USSenate(string electionKey)
    {
      if (electionKey != string.Empty)
      {
        electionKey = ElectionKey_State(electionKey);
        electionKey = electionKey.Remove(0, 2);
        electionKey = electionKey.Insert(0, "U2");
        return electionKey;
      }
      return string.Empty;
    }

    public static string ElectionKey_USHouse(string electionKey)
    {
      if (electionKey != string.Empty)
      {
        electionKey = ElectionKey_State(electionKey);
        electionKey = electionKey.Remove(0, 2);
        electionKey = electionKey.Insert(0, "U3");
        return electionKey;
      }
      return string.Empty;
    }

    public static string ElectionKey_USPres(string electionKey)
    {
      if (electionKey != string.Empty)
      {
        electionKey = ElectionKey_State(electionKey);
        electionKey = electionKey.Remove(0, 2);
        electionKey = electionKey.Insert(0, "U1");
        return electionKey;
      }
      return string.Empty;
    }

    public static void OfficesOfficials_Delete(string officeKey)
    {
      var sql = string.Empty;
      sql += " DELETE FROM OfficesOfficials ";
      sql += " WHERE OfficesOfficials.OfficeKey = " + SqlLit(officeKey);
      G.ExecuteSql(sql);
      //G.Log_OfficesOfficials_Add_Or_Delete(
      //  "D"
      //  , officeKey
      //  , string.Empty
      //  );
    }

    public static void Politician_Delete_All_Tables_All_Rows(string politicianKey)
    {
      var sqlDelete = "DELETE FROM Politicians WHERE PoliticianKey = "
        + SqlLit(politicianKey);
      G.ExecuteSql(sqlDelete);

      sqlDelete = "DELETE FROM Answers WHERE PoliticianKey = "
        + SqlLit(politicianKey);
      G.ExecuteSql(sqlDelete);

      sqlDelete = "DELETE FROM ElectionsPoliticians WHERE PoliticianKey = "
        + SqlLit(politicianKey);
      G.ExecuteSql(sqlDelete);

      sqlDelete = "DELETE FROM OfficesOfficials WHERE PoliticianKey = "
        + SqlLit(politicianKey);
      G.ExecuteSql(sqlDelete);
    }

    public static void Master_Update_Bool(string column, bool columnValue)
    {
      var sql = "UPDATE Master "
        + " SET " + column + " = " + Convert.ToUInt16(columnValue)
        + " WHERE ID = 'MASTER' ";
      G.ExecuteSql(sql);
    }

    #endregion from db

    protected void RadioButtonListPermitElectionDeletions_SelectedIndexChanged(object sender,
      EventArgs e)
    {
      try
      {
        if (IsSuperUser)
        {
          if (RadioButtonListPermitElectionDeletions.SelectedValue == "T")
          {
            Master_Update_Bool("IsElectionDeletionPermitted", true);
            Msg.Text =
              Message(
                "Elections can now be deleted. After deleting an election remember to reset the radio button.");
          }
          else
          {
            Master_Update_Bool("IsElectionDeletionPermitted", false);
            Msg.Text = Message("Deletion of Elections will now NOT BE PERMITTED.");
          }
        }
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

    protected void ButtonCleanElections_Click(object sender, EventArgs e)
    {
      try
      {
        //ElectionsPoliticians Clean up - delete all rows without a cooresponding row in Offices and Politicians Tables
        Response.Write(
          "<br>ElectionsPoliticians rows deleted because no Politician or Office Row(s) in Politicians and offices Table for: ");
        var sql =
          "SELECT ElectionKey,PoliticianKey,OfficeKey FROM ElectionsPoliticians ORDER BY ElectionKey DESC, PoliticianKey,OfficeKey, OrderOnBallot";
        var electionsPoliticiansTable = G.Table(sql);
        foreach (DataRow electionsPoliticiansRowA in electionsPoliticiansTable.Rows)
        {
          var rows = G.Rows("Politicians", "PoliticianKey",
            electionsPoliticiansRowA["PoliticianKey"].ToString());
          if (rows != 1)
          {
            sql = "DELETE FROM ElectionsPoliticians"
              + " WHERE ElectionKey = " + SqlLit(electionsPoliticiansRowA["ElectionKey"].ToString())
              + " AND PoliticianKey = " +
              SqlLit(electionsPoliticiansRowA["PoliticianKey"].ToString());
            G.ExecuteSql(sql);
            Response.Write("<br>" + electionsPoliticiansRowA["ElectionKey"] + " / " +
              electionsPoliticiansRowA["PoliticianKey"]);
          }
          rows = G.Rows("Offices", "OfficeKey", electionsPoliticiansRowA["OfficeKey"].ToString());
          if (rows != 1)
          {
            sql = "DELETE FROM ElectionsPoliticians"
              + " WHERE ElectionKey = " + SqlLit(electionsPoliticiansRowA["ElectionKey"].ToString())
              + " AND OfficeKey = " + SqlLit(electionsPoliticiansRowA["OfficeKey"].ToString());
            G.ExecuteSql(sql);
            Response.Write("<br>" + electionsPoliticiansRowA["ElectionKey"] + " / " +
              electionsPoliticiansRowA["OfficeKey"]);
          }
        }
        //ElectionsOffices Clean up
        Response.Write(
          "<br><br>ElectionsOffices rows deleted because no Office Row(s) in Offices Table for following Election/OfficeKeys: ");
        sql =
          "SELECT ElectionKey,OfficeKey FROM ElectionsOffices ORDER BY ElectionKey DESC,OfficeKey";
        var electionsOfficesTable = G.Table(sql);
        foreach (DataRow electionsOfficesRowA in electionsOfficesTable.Rows)
        {
          var rows = G.Rows("Offices", "OfficeKey", electionsOfficesRowA["OfficeKey"].ToString());
          if (rows != 1)
          {
            sql = "DELETE FROM ElectionsOffices"
              + " WHERE ElectionKey = " + SqlLit(electionsOfficesRowA["ElectionKey"].ToString())
              + " AND OfficeKey = " + SqlLit(electionsOfficesRowA["OfficeKey"].ToString());
            G.ExecuteSql(sql);
            Response.Write("<br>" + electionsOfficesRowA["ElectionKey"] + " / " +
              electionsOfficesRowA["OfficeKey"]);
          }
        }
        Msg.Text = Ok("Done!");
      }
      catch (Exception ex)
      {
        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);
      }
    }

//    protected void ButtonRestructure_Click(object sender, EventArgs e)
//    {
//      var sqlupdate = string.Empty;
//#if false
//      var sql = string.Empty;
//      var sqldelete = string.Empty;
//      var electionKey = string.Empty;
//      var newElectionKey = string.Empty;
//      var electionKeyFederal = string.Empty;
//#endif

//      try
//      {
//        Server.ScriptTimeout = 6000;//6000 sec = 100 min

//        //			ElectionsOffices Changes
//        // 1) Delete level 1-3 Offices with StateCode in ElectionKey
//#if false
//          SQL = "SELECT * FROM ElectionsOffices";
//          Office_Class = 0;
//          string StateCodeInKey = string.Empty;
//          DataTable ElectionsOfficesTable = db.Table(SQL);
//          foreach (DataRow ElectionOfficeRow in ElectionsOfficesTable.Rows)
//          {
//            Office_Class = db.Office_Class(ElectionOfficeRow["OfficeKey"].ToString());
//            if (Office_Class <= 3)
//            {
//              StateCodeInKey = ElectionOfficeRow["ElectionKey"].ToString().Substring(9, 2).ToUpper();
//              if (
//                (StateCodeInKey == ElectionOfficeRow["StateCode"].ToString())
//                && (ElectionOfficeRow["StateCode"].ToString() != "U1")
//                )
//              {
//                SQLDELETE = "DELETE FROM ElectionsOffices"
//                  + " WHERE ElectionKey = " + db.SQLLit(ElectionOfficeRow["ElectionKey"].ToString())
//                  + " AND OfficeKey = " + db.SQLLit(ElectionOfficeRow["OfficeKey"].ToString());
//                db.ExecuteSQL(SQLDELETE);
//              }
//            }
//          }
//          Msg.Text += "Done 1) Delete level 1-3 Offices with StateCode in ElectionKey";
//#endif

//        // 2) Change all ElectionKey to replace any U1-U6 with StateCode -&- Copy ElectionKey to ElectionKey4USReports but Change all ElectionKey4USReports to replace StateCode with U1-U6
//#if false
//          Office_Class = 0;
//          SQL = "SELECT * FROM ElectionsOffices";
//          DataTable ElectionsOfficesTable = db.Table(SQL);
//          foreach (DataRow ElectionOfficeRow in ElectionsOfficesTable.Rows)
//          {
//            Office_Class = db.Office_Class(ElectionOfficeRow["OfficeKey"].ToString());
//            NewElectionKey = db.ElectionKey_State(ElectionOfficeRow["ElectionKey"].ToString(), ElectionOfficeRow["StateCode"].ToString());
//            ElectionKey_Federal = NewElectionKey;
//            ElectionKey_Federal = ElectionKey_Federal.Remove(9, 2);//StateCode
//            ElectionKey_Federal = ElectionKey_Federal.Insert(9, "U");
//            ElectionKey_Federal = ElectionKey_Federal.Insert(10, Office_Class.ToString());
//            SQLUPDATE = "UPDATE ElectionsOffices "
//              + " SET ElectionKey = " + db.SQLLit(NewElectionKey)
//              + ",ElectionKeyFederal = " + db.SQLLit(ElectionKey_Federal)
//              + " WHERE ElectionKey = " + db.SQLLit(ElectionOfficeRow["ElectionKey"].ToString())
//              + " AND OfficeKey = " + db.SQLLit(ElectionOfficeRow["OfficeKey"].ToString());
//            db.ExecuteSQL(SQLUPDATE);
//          }
//          Msg.Text += "<br>Done 2) Change all ElectionKey to replace any U1-U6 with StateCode -&- Copy ElectionKey to ElectionKey4USReports but Change all ElectionKey4USReports to replace StateCode with U1-U6";
//#endif

//        //ElectionsPoliticians Changes
//        // 3) Change all ElectionKey to replace any U1-U6 with StateCode -&&- Copy ElectionKey to ElectionKey4USReports  but Change all ElectionKey4USReports to replace StateCode with U1-U6
//#if false
//          Office_Class = 0;
//          SQL = "SELECT * FROM ElectionsPoliticians";
//          DataTable ElectionsPoliticiansTable = db.Table(SQL);
//          foreach (DataRow ElectionPoliticianRow in ElectionsPoliticiansTable.Rows)
//          {
//            Office_Class = db.Office_Class(ElectionPoliticianRow["OfficeKey"].ToString());
//            NewElectionKey = db.ElectionKey_State(ElectionPoliticianRow["ElectionKey"].ToString(), ElectionPoliticianRow["StateCode"].ToString());
//            ElectionKey_Federal = NewElectionKey;
//            ElectionKey_Federal = ElectionKey_Federal.Remove(9, 2);//StateCode
//            ElectionKey_Federal = ElectionKey_Federal.Insert(9, "U");
//            ElectionKey_Federal = ElectionKey_Federal.Insert(10, Office_Class.ToString());
//            SQLUPDATE = "UPDATE ElectionsPoliticians "
//              + " SET ElectionKey = " + db.SQLLit(NewElectionKey)
//              + ",ElectionKeyFederal = " + db.SQLLit(ElectionKey_Federal)
//              + " WHERE ElectionKey = " + db.SQLLit(ElectionPoliticianRow["ElectionKey"].ToString())
//              + " AND OfficeKey = " + db.SQLLit(ElectionPoliticianRow["OfficeKey"].ToString())
//              + " AND PoliticianKey = " + db.SQLLit(ElectionPoliticianRow["PoliticianKey"].ToString());
//            db.ExecuteSQL(SQLUPDATE);
//          }
//          Msg.Text += "<br>Done 3) Change all ElectionKey to replace any U1-U6 with StateCode -&&- Copy ElectionKey to ElectionKey4USReports  but Change all ElectionKey4USReports to replace StateCode with U1-U6";
//#endif

//      }
//      catch (Exception ex)
//      {
//        Msg.Text = Fail(ex.Message
//          + "<br>" + sqlupdate);
//        Log_Error_Admin(ex);
//      }
//    }

    //protected void ButtonCleanData_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Server.ScriptTimeout = 60000;// = 1000 min = 16 hrs

    //    #region Offices Table
    //    var sql = "UPDATE Offices set DistrictCode = '' WHERE LTRIM(DistrictCode) = ''";
    //    G.ExecuteSql(sql);

    //    sql = "UPDATE Offices set DistrictCodeAlpha = '' WHERE LTRIM(DistrictCodeAlpha) = ''";
    //    G.ExecuteSql(sql);

    //    sql = "UPDATE Offices set CountyCode = '' WHERE LTRIM(CountyCode) = ''";
    //    G.ExecuteSql(sql);

    //    sql = "UPDATE Offices set LocalCode = '' WHERE LTRIM(LocalCode) = ''";
    //    G.ExecuteSql(sql);
    //    #endregion Offices Table

    //    #region OfficesOfficials Table
    //    sql = "UPDATE OfficesOfficials set CountyCode = '' WHERE LTRIM(CountyCode) = ''";
    //    G.ExecuteSql(sql);

    //    sql = "UPDATE OfficesOfficials set LocalCode = '' WHERE LTRIM(LocalCode) = ''";
    //    G.ExecuteSql(sql);
    //    #endregion OfficesOfficials Table

    //    #region Elections Table
    //    sql = "UPDATE Elections set CountyCode = '' WHERE LTRIM(CountyCode) = ''";
    //    G.ExecuteSql(sql);

    //    sql = "UPDATE Elections set LocalCode = '' WHERE LTRIM(LocalCode) = ''";
    //    G.ExecuteSql(sql);
    //    #endregion Elections Table

    //    #region ElectionsOffices Table
    //    sql = "UPDATE ElectionsOffices set CountyCode = '' WHERE LTRIM(CountyCode) = ''";
    //    G.ExecuteSql(sql);

    //    sql = "UPDATE ElectionsOffices set LocalCode = '' WHERE LTRIM(LocalCode) = ''";
    //    G.ExecuteSql(sql);
    //    #endregion ElectionsOffices Table

    //    #region Ballots Table
    //    sql = "UPDATE Ballots set CountyCode = '' WHERE LTRIM(CountyCode) = ''";
    //    G.ExecuteSql(sql);
    //    #endregion Ballots Table

    //    #region Issues Table
    //    sql = "UPDATE Issues set CountyCode = '' WHERE LTRIM(CountyCode) = ''";
    //    G.ExecuteSql(sql);

    //    sql = "UPDATE Issues set LocalCode = '' WHERE LTRIM(LocalCode) = ''";
    //    G.ExecuteSql(sql);
    //    #endregion Issues Table

    //    #region JudicialDistrictCounties Table
    //    sql = "UPDATE JudicialDistrictCounties set DistrictCode = '' WHERE LTRIM(DistrictCode) = ''";
    //    G.ExecuteSql(sql);

    //    sql = "UPDATE JudicialDistrictCounties set DistrictCodeAlpha = '' WHERE LTRIM(DistrictCodeAlpha) = ''";
    //    G.ExecuteSql(sql);

    //    sql = "UPDATE JudicialDistrictCounties set CountyCode = '' WHERE LTRIM(CountyCode) = ''";
    //    G.ExecuteSql(sql);
    //    #endregion JudicialDistrictCounties Table

    //    #region Referendums Table
    //    sql = "UPDATE Referendums set CountyCode = '' WHERE LTRIM(CountyCode) = ''";
    //    G.ExecuteSql(sql);

    //    sql = "UPDATE Referendums set LocalCode = '' WHERE LTRIM(LocalCode) = ''";
    //    G.ExecuteSql(sql);
    //    #endregion Referendums Table

    //    Msg.Text = Ok("Done");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = Fail(ex.Message);
    //    Log_Error_Admin(ex);
    //    #endregion
    //  }

    //}

    //protected void ButtonOneShot_Click(object sender, EventArgs e)
    //{
    //  try
    //  {

    //    //foreach (string stateCode in StateCache.All51StateCodes)
    //    //{
    //    //  string electionKeyMostRecent = db.ElectionKey_Previous_Most_Recent(stateCode);
    //    //  string sql = string.Empty;
    //    //  sql += " select PoliticianKey,OfficeKey from ElectionsPoliticians";
    //    //  sql += " where ElectionKey = " + db.SQLLit(electionKeyMostRecent);
    //    //  sql += " order by OfficeKey asc";
    //    //  DataTable tableElectionsPoliticians = db.Table(sql);
    //    //  foreach (DataRow rowElectionsPoliticians in tableElectionsPoliticians.Rows)
    //    //  //DataRow rowElectionsPoliticians = db.Row_First(sql);
    //    //  {
    //    //    if(db.Is_Incumbent(rowElectionsPoliticians["PoliticianKey"].ToString(),
    //    //      rowElectionsPoliticians["OfficeKey"].ToString()))
    //    //    {
    //    //      db.ElectionsPoliticians_Update_Bool(
    //    //        electionKeyMostRecent,
    //    //        rowElectionsPoliticians["OfficeKey"].ToString(),
    //    //        rowElectionsPoliticians["PoliticianKey"].ToString(),
    //    //        "IsIncumbent",
    //    //        true
    //    //        );
    //    //    }
    //    //      else
    //    //    {
    //    //      db.ElectionsPoliticians_Update_Bool(
    //    //        electionKeyMostRecent,
    //    //        rowElectionsPoliticians["OfficeKey"].ToString(),
    //    //        rowElectionsPoliticians["PoliticianKey"].ToString(),
    //    //        "IsIncumbent",
    //    //        false
    //    //        );
    //    //    }
    //    //  }

    //    //  Msg.Text = db.Ok("<br>Done");
    //    //}
    //  }
    //  catch (Exception ex)
    //  {
    //    #region

    //    Msg.Text = Fail(ex.Message);
    //    Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void ButtonCleanCodes_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Server.ScriptTimeout = 60000;// = 1000 min = 16 hrs

    //    var sql = "update offices set countycode = '',localdistrictcode = '' where officelevel >= 0 AND officelevel <= 7";
    //    G.ExecuteSql(sql);

    //    sql = "update offices set localdistrictcode = '' where officelevel >= 8 AND officelevel <= 11";
    //    G.ExecuteSql(sql);

    //    sql = "update offices set countycode = '' ,localdistrictcode = '' where officelevel >= 16 AND officelevel <= 17";
    //    G.ExecuteSql(sql);

    //    sql = "update offices set localdistrictcode = '' where officelevel =18 ";
    //    G.ExecuteSql(sql);

    //    sql = "update offices set countycode = '' ,localdistrictcode = '' where officelevel >= 20 AND officelevel <= 21";
    //    G.ExecuteSql(sql);

    //    sql = "update offices set localdistrictcode = '' where officelevel =22 ";
    //    G.ExecuteSql(sql);

    //    Msg.Text = Ok("Done");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = Fail(ex.Message);
    //    Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void ButtonDeletePolitician_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Throw_Exception_TextBox_Html_Or_Script(TextBoxDeletePoliticianKey);

    //    G.Politician_Delete(TextBoxDeletePoliticianKey.Text.Trim());

    //    Msg.Text = Ok("Politician with PoliticianKey: "
    //      + TextBoxDeletePoliticianKey.Text.Trim()
    //    + " was deleted in all tables.");

    //    TextBoxDeletePoliticianKey.Text = string.Empty;
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = G.Fail(ex.Message);
    //    Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void ButtonDeleteBadPoliticianRows_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    var count = 0;
    //    var politiciansTable = G.Table("SELECT * from Politicians");
    //    foreach (DataRow politicianRow in politiciansTable.Rows)
    //    {
    //      var politicianKey = politicianRow["PoliticianKey"].ToString();
    //      //string sql = string.Empty;
    //      //sql =
    //      //"Select Count(*) FROM Offices"
    //      //+ " WHERE OfficeKey =" + db.SQLLit(PoliticianRow["OfficeKey"].ToString());

    //      //if (db.Rows_Sql
    //      //  (
    //      //    "Select Count(*) FROM Offices"
    //      //    + " WHERE OfficeKey =" + db.SQLLit(PoliticianRow["OfficeKey"].ToString())
    //      //  ) == 0)
    //      var officeKey = PageCache.Politicians.GetOfficeKey(politicianKey);
    //      if (G.Rows("Offices", "OfficeKey", officeKey) == 0)
    //      {
    //        G.Politician_Delete(politicianKey);
    //        count++;
    //      }
    //    }

    //    Msg.Text = Ok(count + " Politicians Deleted.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = G.Fail(ex.Message);
    //    Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    protected void ButtonDeleteOffice_Click(object sender, EventArgs e)
    {
      try
      {
        var msgReturn = string.Empty;
        var officeKey = TextBoxOfficeKey.Text.Trim();
        if (TextBoxOfficeKey.Text.Trim() == string.Empty)
        {
          throw new ApplicationException("The OfficeKey textbox is empty.");
        }
        if (!Offices.CanAddOfficesToOfficeClass(officeKey))
        {
          throw new ApplicationException("This office is not allowed to be deleted.");
        }
        //sql_count = "SELECT COUNT(*) FROM Offices WHERE OfficeKey = " + db.SQLLit(OfficeKey);
        msgReturn += "<br>"
          + G.Rows("Offices", "OfficeKey", officeKey)
          + " Offices Rows deleted.";

        var sqlDelete = "DELETE FROM Offices WHERE OfficeKey = " + SqlLit(officeKey);
        G.ExecuteSql(sqlDelete);
        //---------

        //sql_count = "SELECT COUNT(*) FROM OfficesOfficials WHERE OfficeKey = " + db.SQLLit(OfficeKey);
        msgReturn += "<br>"
          + G.Rows("OfficesOfficials", "OfficeKey", officeKey)
          + " OfficesOfficials Rows deleted.";

        sqlDelete = "DELETE FROM OfficesOfficials WHERE OfficeKey = " + SqlLit(officeKey);
        G.ExecuteSql(sqlDelete);
        //---------

        //sql_count = "SELECT COUNT(*) FROM ElectionsOffices WHERE OfficeKey = " + db.SQLLit(OfficeKey);
        msgReturn += "<br>"
          + G.Rows("ElectionsOffices", "OfficeKey", officeKey)
          + " ElectionsOffices Rows deleted.";

        sqlDelete = "DELETE FROM ElectionsOffices WHERE OfficeKey = " + SqlLit(officeKey);
        G.ExecuteSql(sqlDelete);
        //---------

        //sql_count = "SELECT COUNT(*) FROM ElectionsPoliticians WHERE OfficeKey = " + db.SQLLit(OfficeKey);
        msgReturn += "<br>"
          + G.Rows("ElectionsPoliticians", "OfficeKey", officeKey)
          + " ElectionsPoliticians Rows deleted.";

        sqlDelete = "DELETE FROM ElectionsPoliticians WHERE OfficeKey = " + SqlLit(officeKey);
        G.ExecuteSql(sqlDelete);


        Msg.Text = Ok(msgReturn);
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    //protected void RadioButtonListMasterControls_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    if (RadioButtonListMasterControls.SelectedValue == "T")
    //    {
    //      Master_Update_Bool("IsMasterControlsVisible", true);
    //      Msg.Text = db.Msg("Master Form Controls are set VISIBLE.");
    //    }
    //    else
    //    {
    //      Master_Update_Bool("IsMasterControlsVisible", false);
    //      Msg.Text = db.Msg("Master Form Controls are set NOT VISIBLE.");
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

    protected void ButtonBadPoliticianOfficeKeys_Click(object sender, EventArgs e)
    {
      try
      {
        const string sqlText = "SELECT * FROM Politicians";
        var politiciansTable = G.Table(sqlText);
        foreach (DataRow politicianRow in politiciansTable.Rows)
        {
          var politicianKey = politicianRow["PoliticianKey"].ToString();
          var officeKey =
            PageCache.Politicians.GetOfficeKey(politicianKey);
          var sqlOffice = "SELECT OfficeKey FROM Offices"
            + " WHERE OfficeKey=" + SqlLit(officeKey);
          var officeRow = Row_Optional(sqlOffice);
          if (officeRow == null)
          {
            Politician_Delete_All_Tables_All_Rows(politicianKey);

            Msg.Text += "<br>Politician for: " + politicianKey
              + " rows deleted in: Politicians, Answers,ElectionsPoliticians";
          }
        }
        Msg.Text = Ok("Done" + Msg.Text);
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void Button_Delete_Politicians_Report_Click(object sender, EventArgs e)
    {
      try
      {
        var count = 0;
        var politiciansList = string.Empty;
        const string sqlText = "SELECT * FROM Politicians";
        var politiciansTable = G.Table(sqlText);
        foreach (DataRow politicianRow in politiciansTable.Rows)
        {
          var politicianKey = politicianRow["PoliticianKey"].ToString();
          var officeKey =
            PageCache.Politicians.GetOfficeKey(politicianKey);
          var sqlOfficeText = "SELECT OfficeKey FROM Offices"
            + " WHERE OfficeKey=" + SqlLit(officeKey);
          var officeRow = Row_Optional(sqlOfficeText);
          if (officeRow == null)
          {
            politiciansList += "<br>Politician in Politicians Table for PoliticianKey: "
              + politicianKey
              + " has no Office Row for OfficeKey: "
              + officeKey;
            count++;
          }
        }
        Msg.Text = Message(politiciansList
          + "<br>Politicians to be deleted:" + count);
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonUpdateAnswers_Click(object sender, EventArgs e)
    {
      try
      {
        var sql = "SELECT *";
        sql += " FROM Answers";
        sql += " WHERE DateStamp = '1/1/1900'";
        var answersTable = G.Table(sql);
        foreach (DataRow answerRow in answersTable.Rows)
        {
          var sqlSelect = string.Empty;
          sqlSelect += "select ElectionKey";
          sqlSelect += " from ElectionsPoliticians";
          sqlSelect += " where PoliticianKey = " + SqlLit(answerRow["PoliticianKey"].ToString());
          sqlSelect += " order by ElectionKey desc";
          var electionsPoliticiansRow = Row_First_Optional(sqlSelect);
          if (electionsPoliticiansRow != null)
          {
            var electionKey = electionsPoliticiansRow["ElectionKey"].ToString();
            var yyyy = electionKey.Substring(2, 4);
            var mm = electionKey.Substring(6, 2).ToUpper();
            var dd = electionKey.Substring(8, 2);
            int imm = Convert.ToUInt16(mm);
            if (imm > 1)
              imm--;
            mm = imm.ToString(CultureInfo.InvariantCulture);
            if (mm.Length == 1)
              mm = mm.PadLeft(2, '0');
            var date = mm + "/" + dd + "/" + yyyy;


            var sqlUpdate = "UPDATE Answers";
            sqlUpdate += " SET DateStamp=" + SqlLit(date);
            sqlUpdate += ",Source='Candidate Website'";
            sqlUpdate += " WHERE QuestionKey=" + SqlLit(answerRow["QuestionKey"].ToString());
            sqlUpdate += " AND PoliticianKey =" + SqlLit(answerRow["PoliticianKey"].ToString());
            sqlUpdate += " AND DateStamp = '1/1/1900'";
            G.ExecuteSql(sqlUpdate);
          }
        }

#if false
        SQL = "SELECT QuestionKey,IssueKey";
        SQL += " FROM Answers";
        SQL += " WHERE IssueKey = ''";
        AnswersTable = db.Table(SQL);
        foreach (DataRow AnswerRow in AnswersTable.Rows)
        {
          int Len = AnswerRow["QuestionKey"].ToString().Length;
          string IssueKey = AnswerRow["QuestionKey"].ToString().Substring(0, Len - 6);
          string Sql_Update = string.Empty;
          Sql_Update = "UPDATE Answers";
          Sql_Update += " SET IssueKey=" + db.SQLLit(IssueKey);
          Sql_Update += " WHERE QuestionKey=" + db.SQLLit(AnswerRow["QuestionKey"].ToString());
          Sql_Update += " AND IssueKey=''";
          db.ExecuteSQL(Sql_Update);
        }
#endif
        Msg.Text = Ok("Answers Table Updated.");
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonFix2_Click(object sender, EventArgs e)
    {
      try
      {
        #region Deletes all ElectionsOffices rows where there is no matching OfficeKey in Offices Table

        var electionsRowsDeleted = 0;
        const string sql = "SELECT * FROM ElectionsOffices";
        var electionsOfficesTable = G.Table(sql);
        foreach (DataRow electionsOfficesRow in electionsOfficesTable.Rows)
        {
          var officeKey = electionsOfficesRow["OfficeKey"].ToString();
          if (!Offices.OfficeKeyExists(officeKey))
          {
            //SQL = "SELECT COUNT(*)";
            //SQL += " FROM ElectionsOffices";
            //SQL += " WHERE ElectionsOffices.OfficeKey = "
            //  + db.SQLLit(ElectionsOfficesRow["OfficeKey"].ToString());
            electionsRowsDeleted += G.Rows("ElectionsOffices"
              , "OfficeKey", officeKey);

            var sqlDelete = "DELETE FROM ElectionsOffices WHERE OfficeKey ="
              + SqlLit(officeKey);
            G.ExecuteSql(sqlDelete);
          }
        }
        Msg.Text = Ok(electionsRowsDeleted + " ElectionsOffices Rows deleted.");

        #endregion Deletes all ElectionsOffices rows where there is no matching OfficeKey in Offices Table
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonFix3_Click(object sender, EventArgs e)
    {
      try
      {
        #region Deletes all ElectionsPoliticians rows where there is no matching OfficeKey in Offices Table

        var electionsPoliticiansRowsDeleted = 0;
        const string sql = "SELECT * FROM ElectionsPoliticians";
        var electionsPoliticiansTable = G.Table(sql);
        foreach (DataRow electionsPoliticiansRow in electionsPoliticiansTable.Rows)
        {
          var officeKey = electionsPoliticiansRow["OfficeKey"].ToString();
          if (!Offices.OfficeKeyExists(officeKey))
          {
            //SQL = "SELECT COUNT(*)";
            //SQL += " FROM ElectionsPoliticians";
            //SQL += " WHERE ElectionsPoliticians.OfficeKey = "
            //  + db.SQLLit(ElectionsPoliticiansRow["OfficeKey"].ToString());
            electionsPoliticiansRowsDeleted += G.Rows("ElectionsPoliticians"
              , "OfficeKey", officeKey);

            var sqlDelete = "DELETE FROM ElectionsPoliticians WHERE OfficeKey ="
              + SqlLit(officeKey);
            G.ExecuteSql(sqlDelete);
          }
        }
        Msg.Text =
          Ok(electionsPoliticiansRowsDeleted +
            " ElectionsPoliticians Rows deleted with non-matching OfficeKey.");

        #endregion Deletes all ElectionsPoliticians rows where there is no matching OfficeKey in Offices Table
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonFix4_Click(object sender, EventArgs e)
    {
      try
      {
        #region Deletes all ElectionsPoliticians rows where there is no matching PoliticianKey in Politicians Table

        var electionsPoliticiansRowsDeletedForPoliticians = 0;
        const string sql = "SELECT * FROM ElectionsPoliticians";
        var electionsPoliticiansTable = G.Table(sql);
        foreach (DataRow electionsPoliticiansRow in electionsPoliticiansTable.Rows)
        {
          if (!Politicians.IsValid(electionsPoliticiansRow["PoliticianKey"].ToString()))
          {
            electionsPoliticiansRowsDeletedForPoliticians += G.Rows("ElectionsPoliticians"
              , "PoliticianKey", electionsPoliticiansRow["PoliticianKey"].ToString());

            var sqlDelete = "DELETE FROM ElectionsPoliticians WHERE PoliticianKey ="
              + SqlLit(electionsPoliticiansRow["PoliticianKey"].ToString());
            G.ExecuteSql(sqlDelete);
          }
        }
        Msg.Text =
          Ok(electionsPoliticiansRowsDeletedForPoliticians +
            " ElectionsPoliticians Rows deleted with non-matching PoliticianKey.");

        #endregion Deletes all ElectionsPoliticians rows where there is no matching PoliticianKey in Politicians Table
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonFix5_Click(object sender, EventArgs e)
    {
      try
      {
        #region Deletes all OfficesOfficials rows where there is no matching OfficeKey in Offices Table

        Server.ScriptTimeout = 60000; // = 1000 min = 16 hrs

        var deleteCount = 0;
        const string sqlOfficesOfficials = "select OfficeKey from OfficesOfficials";
        var officesOfficialsTable = G.Table(sqlOfficesOfficials);
        foreach (DataRow officesOfficialsRow in officesOfficialsTable.Rows)
        {
          if (officesOfficialsRow["OfficeKey"].ToString().Trim() != string.Empty)
          {
            if (!Offices.OfficeKeyExists(officesOfficialsRow["OfficeKey"].ToString()))
            {
              OfficesOfficials_Delete(officesOfficialsRow["OfficeKey"].ToString());
              Msg.Text += "<br>Deleted:" + officesOfficialsRow["OfficeKey"];
              deleteCount++;
            }
          }
        }
        Msg.Text +=
          Ok("<br>" + deleteCount + " OfficesOfficials Rows deleted with non-matching OfficeKey.");

        #endregion Deletes all OfficesOfficials rows where there is no matching OfficeKey in Offices Table
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonFix6_Click(object sender, EventArgs e)
    {
      try
      {
        var msgReturn = string.Empty;
        string sqlUpdate;

        #region Update ElectionKeyCounty, ElectionKeyLocal,ElectionKeyFederal in ElectionsOffices & ElectionsPoliticians Tables

        //ElectionsOffices & ElectionsPoliticians
        //Set ElectionKey_State = ElectionKey

        #region ElectionsOffices

        var sql = "SELECT * FROM ElectionsOffices";
        var electionsOfficesTable = G.Table(sql);
        foreach (DataRow electionsOfficesRow in electionsOfficesTable.Rows)
        {
          sqlUpdate = "UPDATE ElectionsOffices SET ElectionKeyState ="
            + SqlLit(ElectionKey_State(electionsOfficesRow["ElectionKey"].ToString()));
          sqlUpdate += " WHERE ElectionKey=" + SqlLit(electionsOfficesRow["ElectionKey"].ToString());
          sqlUpdate += " AND OfficeKey=" + SqlLit(electionsOfficesRow["OfficeKey"].ToString());
          G.ExecuteSql(sqlUpdate);

          string electionKeyCounty;
          if (electionsOfficesRow["CountyCode"].ToString().Trim() != string.Empty)
            electionKeyCounty = electionsOfficesRow["ElectionKeyState"]
              + electionsOfficesRow["CountyCode"].ToString();
          else
            electionKeyCounty = string.Empty;

          sqlUpdate = "UPDATE ElectionsOffices SET ElectionKeyCounty =" + SqlLit(electionKeyCounty);
          sqlUpdate += " WHERE ElectionKey=" + SqlLit(electionsOfficesRow["ElectionKey"].ToString());
          sqlUpdate += " AND OfficeKey=" + SqlLit(electionsOfficesRow["OfficeKey"].ToString());
          G.ExecuteSql(sqlUpdate);

          string electionKeyLocal;
          if (electionsOfficesRow["LocalCode"].ToString().Trim() != string.Empty)
            electionKeyLocal = electionsOfficesRow["ElectionKeyState"]
              + electionsOfficesRow["CountyCode"].ToString()
              + electionsOfficesRow["LocalCode"];
          else
            electionKeyLocal = string.Empty;

          sqlUpdate = "UPDATE ElectionsOffices SET ElectionKeyLocal =" + SqlLit(electionKeyLocal);
          sqlUpdate += " WHERE ElectionKey=" + SqlLit(electionsOfficesRow["ElectionKey"].ToString());
          sqlUpdate += " AND OfficeKey=" + SqlLit(electionsOfficesRow["OfficeKey"].ToString());
          G.ExecuteSql(sqlUpdate);

          var electionKeyFederal = string.Empty;
          var officeClass = Offices.GetOfficeClass(electionsOfficesRow["OfficeKey"].ToString());
          if (officeClass.IsFederal())
            switch (officeClass)
            {
              case OfficeClass.USPresident:
                electionKeyFederal =
                  ElectionKey_USPres(electionsOfficesRow["ElectionKeyState"].ToString());
                break;
              case OfficeClass.USSenate:
                electionKeyFederal =
                  ElectionKey_USSenate(electionsOfficesRow["ElectionKeyState"].ToString());
                break;
              case OfficeClass.USHouse:
                electionKeyFederal =
                  ElectionKey_USHouse(electionsOfficesRow["ElectionKeyState"].ToString());
                break;
            }
          else
            electionKeyFederal = string.Empty;

          sqlUpdate = "UPDATE ElectionsOffices SET ElectionKeyFederal =" +
            SqlLit(electionKeyFederal);
          sqlUpdate += " WHERE ElectionKey=" + SqlLit(electionsOfficesRow["ElectionKey"].ToString());
          sqlUpdate += " AND OfficeKey=" + SqlLit(electionsOfficesRow["OfficeKey"].ToString());
          G.ExecuteSql(sqlUpdate);
        }
        msgReturn +=
          "<br> ElectionKeyCounty, ElectionKeyLocal,ElectionKeyFederal Updated in ElectionsOffices";

        #endregion ElectionsOffices

        #region ElectionsPoliticians

        sql = "SELECT * FROM ElectionsPoliticians";
        var electionsPoliticiansTable = G.Table(sql);
        foreach (DataRow electionsPoliticiansRow in electionsPoliticiansTable.Rows)
        {
          sqlUpdate = "UPDATE ElectionsPoliticians SET ElectionKeyState ="
            + SqlLit(ElectionKey_State(electionsPoliticiansRow["ElectionKey"].ToString()));
          sqlUpdate += " WHERE ElectionKey=" +
            SqlLit(electionsPoliticiansRow["ElectionKey"].ToString());
          sqlUpdate += " AND OfficeKey=" + SqlLit(electionsPoliticiansRow["OfficeKey"].ToString());
          G.ExecuteSql(sqlUpdate);

          string electionKeyCounty;
          if (electionsPoliticiansRow["CountyCode"].ToString().Trim() != string.Empty)
            electionKeyCounty = electionsPoliticiansRow["ElectionKeyState"].ToString()
              + electionsPoliticiansRow["CountyCode"];
          else
            electionKeyCounty = string.Empty;

          sqlUpdate = "UPDATE ElectionsPoliticians SET ElectionKeyCounty =" +
            SqlLit(electionKeyCounty);
          sqlUpdate += " WHERE ElectionKey=" +
            SqlLit(electionsPoliticiansRow["ElectionKey"].ToString());
          sqlUpdate += " AND OfficeKey=" + SqlLit(electionsPoliticiansRow["OfficeKey"].ToString());
          G.ExecuteSql(sqlUpdate);

          string electionKeyLocal;
          if (electionsPoliticiansRow["LocalCode"].ToString().Trim() != string.Empty)
            electionKeyLocal = electionsPoliticiansRow["ElectionKeyState"].ToString()
              + electionsPoliticiansRow["CountyCode"]
              + electionsPoliticiansRow["LocalCode"];
          else
            electionKeyLocal = string.Empty;

          sqlUpdate = "UPDATE ElectionsPoliticians SET ElectionKeyLocal =" +
            SqlLit(electionKeyLocal);
          sqlUpdate += " WHERE ElectionKey=" +
            SqlLit(electionsPoliticiansRow["ElectionKey"].ToString());
          sqlUpdate += " AND OfficeKey=" + SqlLit(electionsPoliticiansRow["OfficeKey"].ToString());
          G.ExecuteSql(sqlUpdate);

          var electionKeyFederal = string.Empty;
          var officeClass = Offices.GetOfficeClass(electionsPoliticiansRow["OfficeKey"].ToString());
          if (officeClass.IsFederal())
            switch (officeClass)
            {
              case OfficeClass.USPresident:
                electionKeyFederal =
                  ElectionKey_USPres(electionsPoliticiansRow["ElectionKeyState"].ToString());
                break;
              case OfficeClass.USSenate:
                electionKeyFederal =
                  ElectionKey_USSenate(electionsPoliticiansRow["ElectionKeyState"].ToString());
                break;
              case OfficeClass.USHouse:
                electionKeyFederal =
                  ElectionKey_USHouse(electionsPoliticiansRow["ElectionKeyState"].ToString());
                break;
            }
          else
            electionKeyFederal = string.Empty;

          sqlUpdate = "UPDATE ElectionsPoliticians SET ElectionKeyFederal =" +
            SqlLit(electionKeyFederal);
          sqlUpdate += " WHERE ElectionKey=" +
            SqlLit(electionsPoliticiansRow["ElectionKey"].ToString());
          sqlUpdate += " AND OfficeKey=" + SqlLit(electionsPoliticiansRow["OfficeKey"].ToString());
          G.ExecuteSql(sqlUpdate);
        }
        msgReturn +=
          "<br> ElectionKeyCounty, ElectionKeyLocal,ElectionKeyFederal Updated in ElectionsPoliticians";

        #endregion ElectionsPoliticians

        #endregion Set ElectionKey_County, ElectionKeyLocal,ElectionKeyFederal in ElectionsOffices & ElectionsPoliticians Tables

        #region Updates ElectionKey for County and Local elections on ElectionsOffices & ElectionsPoliticians

        //if ElectionKey_County, ElectionKeyLocal not empty Set ElectionKey = ElectionKeyLocal 
        //if ElectionKey_County not empty Set ElectionKey = ElectionKey_County 

        #region ElectionsOffices

        sql = "SELECT * FROM ElectionsOffices";
        electionsOfficesTable = G.Table(sql);
        foreach (DataRow electionsOfficesRow in electionsOfficesTable.Rows)
        {
          if (
            (electionsOfficesRow["ElectionKeyCounty"].ToString().Trim() != string.Empty)
            && (electionsOfficesRow["ElectionKeyLocal"].ToString().Trim() != string.Empty)
          )
          {
            sqlUpdate = "UPDATE ElectionsOffices SET ElectionKey =" +
              SqlLit(electionsOfficesRow["ElectionKeyLocal"].ToString().Trim());
            sqlUpdate += " WHERE ElectionKey=" +
              SqlLit(electionsOfficesRow["ElectionKey"].ToString());
            sqlUpdate += " AND OfficeKey=" + SqlLit(electionsOfficesRow["OfficeKey"].ToString());
            G.ExecuteSql(sqlUpdate);
          }
          else if (
            (electionsOfficesRow["ElectionKeyCounty"].ToString().Trim() != string.Empty)
            && (electionsOfficesRow["ElectionKeyLocal"].ToString().Trim() == string.Empty)
          )
          {
            sqlUpdate = "UPDATE ElectionsOffices SET ElectionKey =" +
              SqlLit(electionsOfficesRow["ElectionKeyCounty"].ToString().Trim());
            sqlUpdate += " WHERE ElectionKey=" +
              SqlLit(electionsOfficesRow["ElectionKey"].ToString());
            sqlUpdate += " AND OfficeKey=" + SqlLit(electionsOfficesRow["OfficeKey"].ToString());
            G.ExecuteSql(sqlUpdate);
          }
        }
        msgReturn += "<br> ElectionKey updated for county and local elections in ElectionsOffices";

        #endregion ElectionsOffices

        #region ElectionsPoliticians

        sql = "SELECT * FROM ElectionsPoliticians";
        electionsPoliticiansTable = G.Table(sql);
        foreach (DataRow electionsPoliticiansRow in electionsPoliticiansTable.Rows)
        {
          if (
            (electionsPoliticiansRow["ElectionKeyCounty"].ToString().Trim() != string.Empty)
            && (electionsPoliticiansRow["ElectionKeyLocal"].ToString().Trim() != string.Empty)
          )
          {
            sqlUpdate = "UPDATE ElectionsPoliticians SET ElectionKey =" +
              SqlLit(electionsPoliticiansRow["ElectionKeyLocal"].ToString().Trim());
            sqlUpdate += " WHERE ElectionKey=" +
              SqlLit(electionsPoliticiansRow["ElectionKey"].ToString());
            sqlUpdate += " AND OfficeKey=" + SqlLit(electionsPoliticiansRow["OfficeKey"].ToString());
            G.ExecuteSql(sqlUpdate);
          }
          else if (
            (electionsPoliticiansRow["ElectionKeyCounty"].ToString().Trim() != string.Empty)
            && (electionsPoliticiansRow["ElectionKeyLocal"].ToString().Trim() == string.Empty)
          )
          {
            sqlUpdate = "UPDATE ElectionsPoliticians SET ElectionKey =" +
              SqlLit(electionsPoliticiansRow["ElectionKeyCounty"].ToString().Trim());
            sqlUpdate += " WHERE ElectionKey=" +
              SqlLit(electionsPoliticiansRow["ElectionKey"].ToString());
            sqlUpdate += " AND OfficeKey=" + SqlLit(electionsPoliticiansRow["OfficeKey"].ToString());
            G.ExecuteSql(sqlUpdate);
          }
        }
        msgReturn +=
          "<br> ElectionKey updated for county and local elections in ElectionsPoliticians";

        #endregion ElectionsPoliticians

        #endregion Updates ElectionKey for County and Local elections on ElectionsOffices & ElectionsPoliticians

        Msg.Text = Ok(msgReturn);
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonFix7_Click(object sender, EventArgs e)
    {
      try
      {
        var msgReturn = string.Empty;

        #region Deletes all Elections, ElectionsOffices and ElectionsPoliticians rows where there are no 1) ElectionsOffices rows (on the ballot) and 2) no Referendum rows for the ElectionKey

        var electionsRowsDeleted = 0;
        //int Referendums_Rows_Deleted = 0;
        var sql = "SELECT * FROM Elections";
        sql += " Where (SUBSTRING(ElectionKey,1,2) <> 'U1')";
        sql += " And (SUBSTRING(ElectionKey,1,2) <> 'U2')";
        sql += " and (SUBSTRING(ElectionKey,1,2) <> 'U3')";
        sql += " and (SUBSTRING(ElectionKey,1,2) <> 'U4')";
        var electionsTable = G.Table(sql);
        foreach (DataRow electionsRow in electionsTable.Rows)
        {
          sql = "ElectionsOffices WHERE ElectionsOffices.ElectionKey = "
            + SqlLit(electionsRow["ElectionKey"].ToString());
          var electionsOfficesRows = G.Rows_Count_From(sql);

          var referendumRows = G.Rows("Referendums"
            , "ElectionKey", electionsRow["ElectionKey"].ToString());

          if ((electionsOfficesRows == 0) && (referendumRows == 0))
          {
            var sqlDelete = "DELETE FROM Elections WHERE ElectionKey ="
              + SqlLit(electionsRow["ElectionKey"].ToString());
            G.ExecuteSql(sqlDelete);

            //sql_delete = "DELETE FROM ReportsElections WHERE ElectionKey ="
            //  + db.SQLLit(ElectionsRow["ElectionKey"].ToString());
            //db.ExecuteSQL(sql_delete);

            sqlDelete = "DELETE FROM ElectionsPoliticians WHERE ElectionKey ="
              + SqlLit(electionsRow["ElectionKey"].ToString());
            G.ExecuteSql(sqlDelete);

            sqlDelete = "DELETE FROM ElectionsOffices WHERE ElectionKey ="
              + SqlLit(electionsRow["ElectionKey"].ToString());
            G.ExecuteSql(sqlDelete);

            electionsRowsDeleted++;

            msgReturn += "<br>Election rows deleted for: " + electionsRow["ElectionKey"];
          }
        }
        msgReturn += "<br>" + electionsRowsDeleted +
          " Elections Deleted and removing all Elections, ElectionsOffices, ElectionsPoliticians, ReportsElections Table rows for these elections.";

        #endregion Delete all Elections rows where there are no ElectionsOffices rows for the ElectionKey

        Msg.Text = Ok(msgReturn);
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonDeleteUserErrorLogs_Click(object sender, EventArgs e)
    {
      try
      {
        LogErrorsAdmin.TruncateTable();
        DB.VoteLog.Log301Redirect.TruncateTable();
        Log302Redirect.TruncateTable();
        Log404PageNotFound.TruncateTable();

        Msg.Text = Ok("The Log Tables have been truncated.");
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void RadioButtonList_Log_301_404_Errors_SelectedIndexChanged(object sender,
      EventArgs e)
    {
      try
      {
        if (RadioButtonList_Log_301_404_Errors.SelectedValue == "T")
        {
          Master_Update_Bool("IsLog301And404Errors", true);
          Msg.Text = Message("Logging of 301, 404 and unhandled errors has been set ON.");
        }
        else
        {
          Master_Update_Bool("IsLog301And404Errors", false);
          Msg.Text = Message("Logging of 301, 404 and unhandled errors has been set OFF.");
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

    protected void Button_Update_ElectionsOffices_Click(object sender, EventArgs e)
    {
      try
      {
        var sql = " SELECT OfficeKey,DistrictCode";
        sql += " FROM Offices";
        sql += " WHERE OfficeLevel = 3";
        sql += " OR OfficeLevel = 5";
        sql += " OR OfficeLevel = 6";
        sql += " OR OfficeLevel = 7";
        sql += " OR OfficeLevel = 17";
        sql += " OR OfficeLevel = 21";
        sql += " GROUP BY OfficeKey,DistrictCode";
        var tableOffices = G.Table(sql);
        foreach (DataRow rowOffices in tableOffices.Rows)
        {
          ElectionsOffices_Update_DistrictCode(
            rowOffices["OfficeKey"].ToString()
            , rowOffices["DistrictCode"].ToString()
          );
        }

        Msg.Text += Ok("<br>Done");
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    //protected void Button_Update_Politician_Names_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    const string sql = "SELECT PoliticianKey, FName, MName, Nickname, LName, LName, Suffix, AddOn FROM Politicians";
    //    var tablePoliticians = G.Table(sql);
    //    foreach (DataRow rowPolitician in tablePoliticians.Rows)
    //    {
    //      //FName
    //      if (rowPolitician["FName"].ToString().Length > 0)
    //      {
    //        var fName = rowPolitician["FName"].ToString();
    //        Validation.FixGivenName(fName);
    //        Politicians_Update_Str(rowPolitician["PoliticianKey"].ToString(), "FName", fName);
    //      }

    //      //MName
    //      if (rowPolitician["MName"].ToString().Length > 0)
    //      {
    //        var mName = rowPolitician["MName"].ToString();
    //        Validation.FixGivenName(mName);
    //        Politicians_Update_Str(rowPolitician["PoliticianKey"].ToString(), "MName", mName);
    //      }

    //      //Nickname
    //      if (rowPolitician["Nickname"].ToString().Length > 0)
    //      {
    //        var nickname = rowPolitician["Nickname"].ToString();
    //        Validation.FixNickname(nickname);
    //        Politicians_Update_Str(rowPolitician["PoliticianKey"].ToString(), "Nickname", nickname);
    //      }

    //      //LName
    //      if (rowPolitician["LName"].ToString().Length > 0)
    //      {
    //        var lName = rowPolitician["LName"].ToString();
    //        Validation.FixLastName(lName);
    //        Politicians_Update_Str(rowPolitician["PoliticianKey"].ToString(), "LName", lName);
    //      }

    //      //Suffix
    //      if (rowPolitician["Suffix"].ToString().Length > 0)
    //      {
    //        var suffix = rowPolitician["Suffix"].ToString();
    //        Validation.FixNameSuffix(suffix);
    //        Politicians_Update_Str(rowPolitician["PoliticianKey"].ToString(), "Suffix", suffix);
    //      }

    //    }
    //    Msg.Text += Ok("<br>Done");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region

    //    Msg.Text = Fail(ex.Message);
    //    Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    protected void Button_Remake_Party_Email_Passwords_Click(object sender, EventArgs e)
    {
      try
      {
        const string sql = "Select PartyEmail from PartiesEmails";
        var tableEmails = G.Table(sql);
        foreach (DataRow rowEmail in tableEmails.Rows)
        {
          var uniquePassword = MakeUniquePassword();
          var updateSql = "UPDATE PartiesEmails"
            + " SET PartyPassword = " + SqlLit(uniquePassword)
            + " WHERE PartyEmail = " + SqlLit(rowEmail["PartyEmail"].ToString());
          G.ExecuteSql(updateSql);
        }
        Msg.Text += Ok("<br>Done");
      }
      catch (Exception ex)
      {
        #region

        Msg.Text = Fail(ex.Message);
        Log_Error_Admin(ex);

        #endregion
      }
    }

    protected void ButtonDeleteWebAddress_Click(object sender, EventArgs e)
    {
      try
      {
        var sql = " UPDATE Politicians";
        sql += " SET WebAddr = ''";
        sql += " WHERE WebAddr = " + SqlLit(TextBoxWebAddress.Text.Trim());
        G.ExecuteSql(sql);

        sql = " UPDATE Politicians";
        sql += " SET StateWebAddr = ''";
        sql += " WHERE StateWebAddr = " + SqlLit(TextBoxWebAddress.Text.Trim());
        G.ExecuteSql(sql);

        //sql = " UPDATE Politicians";
        //sql += " SET LDSWebAddr = ''";
        //sql += " WHERE LDSWebAddr = " + db.SQLLit(TextBoxWebAddress.Text.Trim());
        //db.ExecuteSql(sql);

        Msg.Text = Ok("ALL " + TextBoxWebAddress.Text.Trim()
          + " website addresses have been deleted.");

        TextBoxWebAddress.Text = string.Empty;
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
      if (!IsSuperUser)
      {
        Column1.Visible = false;
        Column2.AddCssClasses("wide");
      }

      if (!IsPostBack)
      {
        const string title = "Master Administration";
        Page.Title = title;
        H1.InnerHtml = title;

        try
        {
          NoJurisdiction.CreateStateLinks("/admin/?state={StateCode}");
          NoJurisdiction.SetHead("Links to State Administration Pages");

          #region Session["UserStateCode"] Notes

          //Session["UserStateCode"] gets set at Login then:
          //Here is the only place Session["UserStateCode"] gets reset to string.Empty.
          //The only place it gets set to a StateCode when entering /Admin/Default.aspx

          #endregion Session["UserStateCode"] Notes

          Session["UserStateCode"] = string.Empty;

          if (IsSuperUser)
          {
            #region Load labels, checkboxes and radion buttons

            #region Radio Button Lists

            //RadioButtonListMasterControls.SelectedValue = Master_Bool("IsMasterControlsVisible") ? "T" : "F";

            RadioButtonList_Log_301_404_Errors.SelectedValue = Master_Bool("IsLog301And404Errors")
              ? "T"
              : "F";

            #endregion Radio Button Lists

            #endregion load, checkboxes labels and radion buttons

            #region Is Election Deletion Permitted

            RadioButtonListPermitElectionDeletions.SelectedValue =
              Master_Bool("IsElectionDeletionPermitted") ? "T" : "F";
            RadioButtonListPermitElectionDeletions.Enabled = true;

            #endregion
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

    //protected void TextBoxEmailAddr_TextChanged(object sender, EventArgs e)
    //{

    //}
  }
}