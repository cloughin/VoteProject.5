namespace Vote.LDSDataUpdate
{
  public partial class LDSDataUpdate : VotePage
  {
//    #region SQL

//    private string sqlOffices4DC()
//    {
//      string SQL = string.Empty;
//      SQL += " SELECT";
//      SQL += " Offices.OfficeKey";
//      SQL += ",Offices.StateCode ";
//      SQL += ",Offices.OfficeLevel";
//      SQL += ",Offices.DistrictCode";
//      SQL += ",Offices.DistrictCodeAlpha";
//      SQL += " FROM Offices";
//      SQL += " WHERE (StateCode = 'DC')";
//      SQL += " AND ((OfficeLevel = 3)";
//      SQL += " OR (OfficeLevel = 5)";
//      SQL += " OR (OfficeLevel = 6)";
//      SQL += " OR ((OfficeLevel = 4) AND (OfficeKey = 'DCAtLargeMemberOfTheCouncil'))";//only DC Council At Large for DC
//      SQL += " )";
//      SQL += " AND DistrictCodeAlpha = ''";
//      SQL += " ORDER BY StateCode,OfficeLevel,DistrictCode,DistrictCodeAlpha";
//      return SQL;
//    }
//    private string sqlOffices4ID_WA()
//    {
//      string SQL = string.Empty;
//      SQL += " SELECT";
//      SQL += " Offices.OfficeKey";
//      SQL += ",Offices.StateCode ";
//      SQL += ",Offices.OfficeLevel";
//      SQL += ",Offices.DistrictCode";
//      SQL += ",Offices.DistrictCodeAlpha";
//      SQL += " FROM Offices";
//      SQL += " WHERE (OfficeLevel = 6)";
//      SQL += " AND DistrictCodeAlpha != ''";
//      SQL += " ORDER BY StateCode,OfficeLevel,DistrictCode,DistrictCodeAlpha";
//      return SQL;
//    }
//    private string sqlLEGIDYY(string LDSStateCode, string LDSTypeCode, string LDSDistrictCode)
//    {
//      string SQL = string.Empty;
//      SQL += " SELECT ";
//      SQL += " * ";
//      SQL += " FROM LEGIDYY ";
//      SQL += " WHERE STATE = " + db.SQLLit(LDSStateCode);
//      SQL += " AND TYPE = " + db.SQLLit(LDSTypeCode);
//      SQL += " AND DISTRICT = " + db.SQLLit(LDSDistrictCode);
//      SQL += " ORDER BY STATE,TYPE,DISTRICT";
//      return SQL;
//    }

//    private string sqlOfficesOfficials(string OfficeKey)
//    {
//      string SQL = string.Empty;
//      //SQL += " SELECT OfficeKey FROM OfficesOfficials ";
//      SQL += " SELECT * FROM OfficesOfficials ";
//      SQL += " WHERE OfficeKey = " + db.SQLLit(OfficeKey);
//      return SQL;
//    }
//    private string sql_OfficesOfficials_Delete(string OfficeKey)
//    {
//      string SQL = string.Empty;
//      SQL += " DELETE FROM OfficesOfficials ";
//      SQL += " WHERE OfficesOfficials.OfficeKey = " + db.SQLLit(OfficeKey);
//      return SQL;
//    }
//    private string sql_OfficesOfficials_Delete_Politician(string PoliticianKey)
//    {
//      string SQL = string.Empty;
//      SQL += " DELETE FROM OfficesOfficials ";
//      SQL += " WHERE OfficesOfficials.PoliticianKey = " + db.SQLLit(PoliticianKey);
//      return SQL;
//    }
//    private string sql_OfficesOfficials_Delete(string OfficeKey, string PoliticianKey)
//    {
//      string SQL = string.Empty;
//      SQL += " DELETE FROM OfficesOfficials ";
//      SQL += " WHERE OfficesOfficials.OfficeKey = " + db.SQLLit(OfficeKey);
//      SQL += " AND OfficesOfficials.PoliticianKey = " + db.SQLLit(PoliticianKey);
//      return SQL;
//    }
//    private string sqlOfficesOfficialsInsertLDS(string OfficeKey, string PoliticianKey, string RunningMateKey, string LDSVersion, DateTime LDSUpdateDate)
//    {
//      string SQL = string.Empty;
//      SQL += " INSERT INTO OfficesOfficials ";
//      SQL += "(";
//      SQL += "OfficeKey";
//      SQL += ",StateCode";
//      SQL += ",PoliticianKey";
//      SQL += ",RunningMateKey";
//      SQL += ",LDSVersion";
//      SQL += ",LDSUpdateDate";
//      SQL += ",DataLastUpdated";
//      SQL += ",UserSecurity";
//      SQL += ",UserName";
//      SQL += ")";
//      SQL += " VALUES ";
//      SQL += "(";
//      SQL += db.SQLLit(OfficeKey);
//      SQL += "," + db.SQLLit(db.StateCode4OfficeKey(OfficeKey));
//      SQL += "," + db.SQLLit(PoliticianKey);
//      SQL += "," + db.SQLLit(RunningMateKey);
//      SQL += "," + db.SQLLit(LDSVersion);
//      SQL += "," + db.SQLLit(LDSUpdateDate.ToString());
//      SQL += "," + db.SQLLit(LDSUpdateDate.ToString());
//      SQL += "," + db.SQLLit(db.User_Security());
//      SQL += "," + db.SQLLit(db.User_Name());
//      SQL += ")";
//      return SQL;
//    }
//    private string sql_OfficesOfficials_Insert_ChairmanOfCouncil(string LDSVersion, DateTime LDSUpdateDate)
//    {
//      string SQL = string.Empty;
//      SQL += " INSERT INTO OfficesOfficials ";
//      SQL += "(";
//      SQL += "OfficeKey";
//      SQL += ",StateCode";
//      SQL += ",PoliticianKey";
//      SQL += ",RunningMateKey";
//      SQL += ",LDSVersion";
//      SQL += ",LDSUpdateDate";
//      SQL += ",DataLastUpdated";
//      SQL += ",UserSecurity";
//      SQL += ",UserName";
//      SQL += ")";
//      SQL += " VALUES ";
//      SQL += "(";
//      SQL += "'DCChairmanOfTheCouncil'";
//      SQL += ",'DC'";
//      SQL += "," + db.SQLLit(TextBoxDCCouncilChairman.Text.Trim());
//      SQL += ",''";
//      SQL += "," + db.SQLLit(LDSVersion);
//      SQL += "," + db.SQLLit(LDSUpdateDate.ToString());
//      SQL += "," + db.SQLLit(LDSUpdateDate.ToString());
//      SQL += "," + db.SQLLit(db.User_Security());
//      SQL += "," + db.SQLLit(db.User_Name());
//      SQL += ")";
//      return SQL;
//    }
//    private string sqlRunningMateKey4LastElection(string PoliticianKey)
//    {
//      string SQL = string.Empty;
//      SQL += " SELECT ";
//      SQL += " RunningMateKey ";
//      SQL += " FROM ElectionsPoliticians ";
//      SQL += " WHERE PoliticianKey = " + db.SQLLit(PoliticianKey);
//      SQL += " ORDER BY ElectionKey DESC";
//      return SQL;
//    }
//    private string sql_CENSUS(string LdsStateCode, string LdsCountyCode)
//    {
//      string SQL = string.Empty;
//      SQL += " SELECT ";
//      SQL += " STATE";
//      SQL += ",COUNTY";
//      SQL += ",NAME";
//      SQL += " FROM CENSUS";
//      SQL += " WHERE STATE = " + db.SQLLit(LdsStateCode);
//      SQL += " AND COUNTY = " + db.SQLLit(LdsCountyCode);
//      return SQL;
//    }
//    private string sql_States_LDS()
//    {
//      string SQL = string.Empty;
//      SQL += " SELECT ";
//      SQL += " StateCode ";
//      SQL += " ,State ";
//      SQL += " ,LDSStateCode ";
//      SQL += " ,LDSStateName ";
//      SQL += " FROM States ";
//      SQL += " WHERE StateCode != 'US' ";
//      SQL += " AND StateCode != 'U1' ";
//      SQL += " AND StateCode != 'U2' ";
//      SQL += " AND StateCode != 'U3' ";
//      SQL += " AND StateCode != 'AS' ";
//      SQL += " AND StateCode != 'GU' ";
//      SQL += " AND StateCode != 'VI' ";
//      SQL += " AND StateCode != 'PR' ";
//      SQL += " ORDER BY State";
//      return SQL;
//    }
//    private string Office4StateCodeOfficeLevelDistrictCode(
//      string StateCode
//      , int Office_Class
//      , string DistrictCode
//      )
//    {
//      string SQL = string.Empty;
//      SQL += " SELECT ";
//      SQL += " Offices.StateCode";
//      SQL += " ,Offices.OfficeKey";
//      SQL += " ,Offices.OfficeLine1";
//      SQL += " ,Offices.OfficeLine2";
//      SQL += " ,Offices.OfficeOrderWithinLevel";
//      SQL += " FROM Offices";
//      SQL += " WHERE Offices.StateCode = " + db.SQLLit(StateCode);
//      SQL += " AND Offices.OfficeLevel = " + Office_Class.ToString();
//      SQL += " AND Offices.DistrictCode = " + db.SQLLit(DistrictCode);
//      SQL += " ORDER BY Offices.OfficeOrderWithinLevel";
//      return SQL;
//    }
//    #endregion SQL

//    private void LoadMasterControls()
//    {
//      LDSVersion.Text = db.Master_Str("LDSVersion");
//      TextBoxVersion.Text = db.Master_Str("LDSVersion");


//      LDSDateCompleted.Text = db.Master_Date("LDSDateCompleted").ToString();
//      LastUpdateDate.Text = db.Master_Date_Str(
//        "LDSUpdateDate");

//      LDSVersionCompleted.Text = db.Master_Str("LDSVersionCompleted");
//      TextBoxUpdateDate.Text = db.Master_Date(
//        "LDSUpdateDate").ToString("MM/dd/yyyy");

//      OfficesRunTime.Text = db.Master_Str("LDSOfficesRunTime");
//      LabelLDSDateCompletedOffices.Text = db.Master_Date_Str(
//        "LDSDateCompletedOffices");

//      PoliticiansRunTime.Text = db.Master_Str("LDSPoliticiansRunTime");
//      LabelLDSDateCompletedPoliticians.Text = db.Master_Date_Str(
//        "LDSDateCompletedPoliticians");

//      LabelLDSNewPoliticiansAdded.Text = db.Master_Date_Str(
//        "LDSDateCompletedPoliticiansAdded");

//      OfficesOfficialsRunTime.Text = db.Master_Str("LDSOfficesOfficialsRunTime");
//      LabelLDSDateCompletedOfficesOfficials.Text = db.Master_Date_Str(
//        "LDSDateCompletedOfficesOfficials");

//      ReportsRunTime.Text = db.Master_Str("LDSReportsRunTime");
//      LabelLDSDateCompletedReports.Text = db.Master_Date_Str(
//        "LDSDateCompletedReports");
//    }
//    private void CheckTextBoxs4HtmlAndIlleagalInput()
//    {
//      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxVersion);
//      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpdateDate);
//      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxDCCouncilChairman);
//    }
//    protected void CheckVersionAndUpdateDate()
//    {
//      if (TextBoxVersion.Text.Trim() == string.Empty)
//        throw new ApplicationException("You need to enter an LDS Version.");
//      if (TextBoxUpdateDate.Text.Trim() == string.Empty)
//        throw new ApplicationException("You need to enter an Update Date.");
//      if (!db.Is_Valid_Date(TextBoxUpdateDate.Text.Trim()))
//        throw new ApplicationException("Update Date is invalid.");
//      //ViewState["UpdateDate"] = Convert.ToDateTime(TextBoxUpdateDate.Text.Trim());
//    }

//    protected void ReportLEGIDYYRow(string LEG_ID_NUM, string PoliticianKey)
//    {
//      DataRow LEGIDYYRow = db.Row("SELECT * FROM LEGIDYY WHERE LEG_ID_NUM = " + db.SQLLit(LEG_ID_NUM));
//      Report.Text += "LEGIDYY STATE: " + LEGIDYYRow["STATE"].ToString() + " (" + db.StateCode4LDSStateCode(LEGIDYYRow["STATE"].ToString()) + ")"
//      + " TYPE: " + LEGIDYYRow["TYPE"].ToString() + " (" + db.OfficeName4LDSType(LEGIDYYRow["TYPE"].ToString()) + ")"
//      + " ,DISTRICT: " + LEGIDYYRow["DISTRICT"].ToString()
//      + " ,LEG_ID_NUM: " + LEGIDYYRow["LEG_ID_NUM"].ToString();
//      if (
//        (PoliticianKey != "Vacant")
//        && (PoliticianKey != "OfficeKey")
//        )
//        Report.Text += " ,NAME: " + LEGIDYYRow["FIRST_NAME"].ToString() + " " + LEGIDYYRow["MID_NAME"].ToString() + " " + LEGIDYYRow["LAST_NAME"].ToString();

//      Report.Text += ". </span>";
//    }

//    protected void ReportBadPoliticianKey(string LEG_ID_NUM, string PoliticianKey)
//    {
//      if (PoliticianKey == "Vacant")
//      {
//        Report.Text += "<br><span class=TSmallColor>Vacant Position: ";
//        ReportLEGIDYYRow(LEG_ID_NUM, PoliticianKey);
//      }
//      else
//      {
//        Report.Text += "<br><br><span class=TSmallColor>There is no Politicians row for PoliticianKey: " + PoliticianKey;
//        Report.Text += "<br>";
//        ReportLEGIDYYRow(LEG_ID_NUM, PoliticianKey);
//        Report.Text += "<br>";
//      }

//    }

//    protected void Add2LEGIDYYNotProcessedTable(DataRow LEGIDYYRow)
//    {
//      try
//      {
//        string SQLINSERT = "INSERT INTO LEGIDYYNotProcessed "
//          + "("
//          + "STATE"
//          + ",TYPE"
//          + ",DISTRICT"
//          + ",LEG_ID_NUM"
//          + ",TITLE"
//          + ",FIRST_NAME"
//          + ",MID_NAME"
//          + ",LAST_NAME"
//          + ",SUFFIX"
//          + ",PARTY"
//          + ",GENDER"
//          + ",DIST_ADD1"
//          + ",DIST_ADD2"
//          + ",DIST_CITY"
//          + ",DIST_STATE"
//          + ",DIST_ZIP"
//          + ",DIST_PHONE"
//          + ",DIST_FAX"
//          + ",CAP_ADD1"
//          + ",CAP_ADD2"
//          + ",CAP_CITY"
//          + ",CAP_STATE"
//          + ",CAP_ZIP"
//          + ",CAP_PHONE"
//          + ",CAP_FAX"
//          + ",CMMTTEE_1"
//          + ",CMMTTEE_2"
//          + ",CMMTTEE_3"
//          + ",CMMTTEE_4"
//          + ",CMMTTEE_5"
//          + ",CMMTTEE_6"
//          + ",CMMTTEE_7"
//          + ",CMMTTEE_8"
//          + ",CMMTTEE_9"
//          + ",CMMTTEE_10"
//          + ",CMMTTEE_11"
//          + ",CMMTTEE_12"
//          + ",CMMTTEE_13"
//          + ",CMMTTEE_14"
//          + ",CMMTTEE_15"
//          + ",CMMTTEE_16"
//          + ",CMMTTEE_17"
//          + ",CMMTTEE_18"
//          + ",CMMTTEE_19"
//          + ",CMMTTEE_20"
//          + ",E_MAIL_ADD"
//          + ",W_WIDE_WEB"
//          //+ ",ENDREC"
//          + ")"
//          + " VALUES("
//          + db.SQLLit(LEGIDYYRow["STATE"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["TYPE"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["DISTRICT"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["LEG_ID_NUM"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["TITLE"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["FIRST_NAME"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["MID_NAME"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["LAST_NAME"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["SUFFIX"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["PARTY"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["GENDER"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["DIST_ADD1"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["DIST_ADD2"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["DIST_CITY"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["DIST_STATE"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["DIST_ZIP"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["DIST_PHONE"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["DIST_FAX"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CAP_ADD1"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CAP_ADD2"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CAP_CITY"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CAP_STATE"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CAP_ZIP"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CAP_PHONE"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CAP_FAX"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_1"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_2"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_3"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_4"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_5"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_6"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_7"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_8"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_9"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_10"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_11"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_12"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_13"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_14"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_15"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_16"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_17"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_18"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_19"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["CMMTTEE_20"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["E_MAIL_ADD"].ToString())
//          + "," + db.SQLLit(LEGIDYYRow["W_WIDE_WEB"].ToString())
//          //+ "," + db.SQLLit(LEGIDYYRow["ENDREC"].ToString())
//          + ")";
//        //--------------- UNCOMMENT -----------------------------
//        db.ExecuteSQL(SQLINSERT);
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }
//    protected string StatesLDSOptional(string LDSStateCode, string Column)
//    {
//      return db.Single_Key_Str_Optional("States", Column, "LDSStateCode", LDSStateCode);
//    }
//    protected void MsgPoliticianNotFound(DataRow LEGIDYYRow)
//    {
//      string StateCode = StatesLDSOptional(LEGIDYYRow["STATE"].ToString(), "StateCode");
//      Report.Text += "<br>" + StateCode;
//      Report.Text += " " + LEGIDYYRow["TITLE"].ToString();
//      Report.Text += " " + LEGIDYYRow["FIRST_NAME"].ToString();
//      Report.Text += " " + LEGIDYYRow["MID_NAME"].ToString();
//      Report.Text += " " + LEGIDYYRow["LAST_NAME"].ToString();
//      Report.Text += " " + LEGIDYYRow["SUFFIX"].ToString();
//      Report.Text += " Not found in Politicians Table and added to LEGIDYYNotProcessed Table.";
//    }

//    protected int OfficesOfficials_Insert(string OfficeKey, string LEG_ID_NUM, ref int BadPoliticianKeys, ref int Vacant)
//    {
//      string PoliticianKey = db.Politicians_Str_LDSLegIDNum(LEG_ID_NUM, "PoliticianKey");
//      if (PoliticianKey == string.Empty)
//      {
//        PoliticianKey = db.Politician_Key(
//          db.StateCode4LDSStateCode(db.LEGIDYY(LEG_ID_NUM, "STATE"))
//        , db.LEGIDYY(LEG_ID_NUM, "LAST_NAME")
//        , db.LEGIDYY(LEG_ID_NUM, "FIRST_NAME")
//        , db.LEGIDYY(LEG_ID_NUM, "MID_NAME")
//        , db.LEGIDYY(LEG_ID_NUM, "SUFFIX")
//        );
//      }

//      if (!db.Is_Valid_Politician(PoliticianKey))
//      {
//        string VacantKey = db.StateCode4LDSStateCode(db.LEGIDYY(LEG_ID_NUM, "STATE"))
//          + "VacantVacant";
//        if (PoliticianKey.ToUpper() == VacantKey.ToUpper())
//        {
//          Vacant++;
//          ReportBadPoliticianKey(LEG_ID_NUM, "Vacant");
//        }
//        else
//        {
//          BadPoliticianKeys++;
//          ReportBadPoliticianKey(LEG_ID_NUM, PoliticianKey);
//        }
//        return 0;
//      }
//      else
//      {
//        string RunningMateKey = string.Empty;
//        if (db.Is_Office_RunningMate(OfficeKey))
//        {
//          DataRow ElectionsPoliticiansRow =
//            db.Row_First_Optional(sqlRunningMateKey4LastElection(PoliticianKey));
//          if (ElectionsPoliticiansRow != null)
//            RunningMateKey = ElectionsPoliticiansRow["RunningMateKey"].ToString();
//        }

//        db.ExecuteSQL(
//          //string test1 = //here
//      sqlOfficesOfficialsInsertLDS(
//      OfficeKey
//      , PoliticianKey
//      , RunningMateKey
//      , TextBoxVersion.Text.Trim()
//      , Convert.ToDateTime(TextBoxUpdateDate.Text.Trim())
//          )
//    );
//        return 1;
//      }
//    }

//    protected string ReportLDCODEAndOffices(DataRow StatesRow, char LDSOfficeLevel, int Office_Class)
//    {
//      string Report = "<br><br>LDCODE Rows: STATE TYPE DISTRICT - LDCODE: Office Title";
//      DataTable LDCODETable = db.Table(sql.LDCODE4LdsStateCodeLDSTypeCode(
//      StatesRow["LDSStateCode"].ToString(), LDSOfficeLevel));
//      foreach (DataRow LDCODERow in LDCODETable.Rows)
//      {
//        Report += "<br>"
//          + LDCODERow["STATE"].ToString()
//          + " " + LDCODERow["TYPE"].ToString()
//          + " " + LDCODERow["DISTRICT"].ToString()
//          + " - " + LDCODERow["NAME"].ToString();
//      }

//      Report += "<br><br>Offices Rows: OfficeKey StateCode OfficeLevel DistrictCode - Offieces Table: Office Title";

//      string SQL = string.Empty;
//      SQL += " SELECT ";
//      SQL += " Offices.OfficeKey ";
//      SQL += " ,Offices.StateCode ";
//      SQL += " ,Offices.OfficeLevel ";
//      SQL += " ,Offices.OfficeOrderWithinLevel ";
//      SQL += " ,Offices.OfficeLine1 ";
//      SQL += " ,Offices.OfficeLine2 ";
//      SQL += " ,Offices.DistrictCode ";
//      SQL += " ,Offices.DistrictCodeAlpha ";
//      SQL += " ,Offices.CountyCode ";
//      SQL += " ,Offices.LocalCode ";
//      SQL += " ,Offices.OfficeOrderWithinLevel ";
//      SQL += " ,Offices.IsRunningMateOffice ";
//      SQL += " ,Offices.Incumbents ";
//      SQL += " ,Offices.VoteInstructions ";
//      SQL += " ,Offices.VoteForWording ";
//      SQL += " ,Offices.WriteInInstructions ";
//      SQL += " ,Offices.WriteInWording ";
//      SQL += " ,Offices.WriteInLines ";
//      SQL += ",Offices.IsInactive";
//      SQL += " FROM Offices";
//      SQL += " WHERE Offices.StateCode = " + db.SQLLit(StatesRow["StateCode"].ToString());
//      SQL += " AND Offices.OfficeLevel = " + Office_Class.ToString();
//      SQL += " ORDER BY DistrictCode,DistrictCodeAlpha";


//      //DataTable OfficesTable = db.Table(sql.RowsOffices4StateCodeOfficeLevel(StatesRow["StateCode"].ToString()
//      //, Office_Class));
//      DataTable OfficesTable = db.Table(SQL);
//      foreach (DataRow OfficesRow in OfficesTable.Rows)
//      {
//        Report += "<br>"
//          + OfficesRow["OfficeKey"].ToString()
//          + " " + OfficesRow["StateCode"].ToString()
//          + " " + OfficesRow["OfficeLevel"].ToString()
//          + " " + OfficesRow["DistrictCode"].ToString()
//          + " - " + OfficesRow["OfficeLine1"].ToString()
//          + " " + OfficesRow["OfficeLine2"].ToString();
//      }
//      Report += "<br><br><b>Office Titles Should Match for LDCODE Table and Offices Table</b>";
//      return Report;
//    }
//    //1) Check LDS Tables
//    protected void ButtonCheckStatesCounties_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        DataTable StatesTable = null;
//        DataTable OfficesTable = null;
//        DataTable LDCODETable = null;
//        DataRow CENSUSRow = null;
//        DataRow LDCODEROW = null;
//        string strDistrict = string.Empty;
//        int District = 0;
//        string OfficeKey = string.Empty;
//        string Office = string.Empty;
//        string NAME = string.Empty;

//        if (true == true)
//        {
//          #region CENSUS Table Problems

//          Report.Text = "&&&&&&&&&  CENSUS Table Problems (State Names, County Names, County Codes) &&&&&&&&&&&&&";
//          int Census_Table_Problems = 0;
//          #region States Table
//          Report.Text += "<br>=== with States Table ============<br>";

//          StatesTable = db.Table(sql_States_LDS());
//          foreach (DataRow StatesRow in StatesTable.Rows)
//          {
//            #region Every States Table row should have a unique CENSUS row with matching LDSStateCode and LDSStateName
//            bool Ok = true;
//            //LDSStateCode in States Table for the StateCode in States row
//            string test = db.States_Str(StatesRow["StateCode"].ToString(), "LDSStateCode");
//            CENSUSRow = db.Row_Optional(sql_CENSUS(
//             db.States_Str(StatesRow["StateCode"].ToString()
//              , "LDSStateCode")
//              , "000"));
//            if (CENSUSRow == null)
//            {
//              Report.Text += "<br>There is no CENSUS row for STATE(StateCode): "
//                + db.States_Str(StatesRow["StateCode"].ToString(), "LDSStateCode")
//                + "(" + StatesRow["StateCode"].ToString() + ")"
//                + " COUNTY: 000";
//              Ok = false;
//              Census_Table_Problems++;
//            }
//            if (CENSUSRow != null)
//            {
//              if (CENSUSRow["NAME"].ToString().ToUpper() != StatesRow["State"].ToString().ToUpper()) //Of vs of Columbia
//              {
//                Report.Text += "<br>The State Name, <b>STATE</B> in the CENSUS row STATE:" + CENSUSRow["STATE"].ToString()
//                  + " COUNTY:" + CENSUSRow["COUNTY"].ToString()
//                  + " is [" + CENSUSRow["NAME"].ToString() + "]"
//                  + "<br>does not match the State Name, <b>State</b> in  the States row StateCode:" + StatesRow["StateCode"].ToString()
//                  + " which is [" + StatesRow["State"].ToString() + "]";
//                Ok = false;
//                Census_Table_Problems++;
//              }
//            }
//            if (!Ok)
//              Report.Text += "<br>";//extra space between rows
//            #endregion Every States Table row should have a unique CENSUS row with matching LDSStateCode and LDSStateName

//          }
//          Report.Text += "<br>Census Table problems with States Table: " + Census_Table_Problems.ToString();
//          #endregion States Table

//          #region Counties Table
//          Report.Text += "<br>====== with Counties Table ==========<br>";
//          int Census_With_Counties = 0;
//          DataTable CountiesTable = db.Table(sql.Counties_LDS());
//          foreach (DataRow CountyRow in CountiesTable.Rows)
//          {
//            #region Every Counties row should have a unique CENSUS row with matching LDSStateCode, LDSCountyCode and LDSCounty
//            bool Ok = true;

//            if (CountyRow["County"].ToString().Trim() == string.Empty)
//            {
//              CENSUSRow = db.Row_Optional(sql_CENSUS(
//               db.States_Str(CountyRow["StateCode"].ToString(), "LDSStateCode")
//             , CountyRow["CountyCode"].ToString()));

//              if (CENSUSRow == null)
//              {
//                Report.Text += "<br>County column name is missing for StateCode:"
//                  + CountyRow["StateCode"].ToString()
//                  + "(No CENSUS row for STATE)"
//                  + " CountyCode:"
//                  + CountyRow["CountyCode"].ToString();
//                Census_With_Counties++;
//              }
//              else
//              {
//                Report.Text += "<br>County column name is missing for StateCode:"
//                  + CountyRow["StateCode"].ToString()
//                  + "(" + CENSUSRow["STATE"].ToString() + ")"
//                  + " CountyCode:"
//                  + CountyRow["CountyCode"].ToString();
//                Census_With_Counties++;
//              }
//              Ok = false;
//            }
//            if (CountyRow["LDSCounty"].ToString().Trim() == string.Empty)
//            {
//              Report.Text += "<br>LDSCounty column name is missing for StateCode:"
//                + CountyRow["StateCode"].ToString()
//                + " CountyCode:"
//                + CountyRow["CountyCode"].ToString();
//              Census_With_Counties++;
//              Ok = false;
//            }

//            CENSUSRow = db.Row_Optional(sql_CENSUS(
//             db.States_Str(CountyRow["StateCode"].ToString(), "LDSStateCode")
//           , CountyRow["CountyCode"].ToString()));
//            if (CENSUSRow == null)
//            {
//              Report.Text += "<br>There is no CENSUS row for STATE(StateCode): "
//                + db.States_Str(CountyRow["StateCode"].ToString(), "LDSStateCode")
//                + "(" + CountyRow["StateCode"].ToString() + ")"
//                + " COUNTY: "
//                + CountyRow["CountyCode"].ToString()
//                + "(" + CountyRow["County"].ToString() + ")";
//              Ok = false;
//              Census_With_Counties++;
//            }
//            if (CENSUSRow != null)
//            {
//              if (
//                (CENSUSRow["NAME"].ToString().Trim().ToUpper() != CountyRow["County"].ToString().ToUpper())
//                && (CountyRow["County"].ToString().Trim() != string.Empty)
//                )
//              {
//                Report.Text += "<br>The County Name in the CENSUS row STATE:" + CENSUSRow["STATE"].ToString() + " COUNTY:" + CENSUSRow["COUNTY"].ToString()
//                  + " is [" + CENSUSRow["NAME"].ToString() + "]"
//                  + "<br>does not match column <b>County</b> in  the Counties row StateCode:" + CountyRow["StateCode"].ToString() + " CountyCode:" + CountyRow["CountyCode"].ToString()
//                  + " which is [" + CountyRow["County"].ToString() + "]";
//                Ok = false;
//                Census_With_Counties++;
//              }
//              if (
//                (CENSUSRow["NAME"].ToString().Trim() != CountyRow["LDSCounty"].ToString())
//                && (CountyRow["LDSCounty"].ToString().Trim() != string.Empty)
//                )
//              {
//                Report.Text += "<br>The County Name in the CENSUS row STATE:" + CENSUSRow["STATE"].ToString() + " COUNTY:" + CENSUSRow["COUNTY"].ToString()
//                  + " is [" + CENSUSRow["NAME"].ToString() + "]"
//                  + "<br>does not match column <b>LDSCounty</b> in  the Counties row StateCode:" + CountyRow["StateCode"].ToString() + " CountyCode:" + CountyRow["CountyCode"].ToString()
//                  + " which is [" + CountyRow["LDSCounty"].ToString() + "]";
//                Ok = false;
//                Census_With_Counties++;
//              }
//            }
//            if (!Ok)
//              Report.Text += "<br>";//extra space between rows
//            #endregion Every Counties row should have a unique CENSUS row with matching LDSStateCode, LDSCountyCode and LDSCounty

//          }
//          Report.Text += "<br>Census Table problems with Counties Table: " + Census_With_Counties.ToString();
//          #endregion Counties Table

//          #endregion CENSUS Table Problems
//        }

//        if (true == true)
//        {
//          #region LDCODE Table Problems

//          Report.Text += "<br><br>&&&&&&&&&  LDCODE Table Problems (Offices at each office level) &&&&&&&&&&&&&";

//          #region Offices Table
//          Report.Text += "<br>=== with Offices Table ============<br>";
//          int OfficesInLDCODE = 0;
//          int OfficesInOffices = 0;
//          if (true == true)
//          {
//            StatesTable = db.Table(sql_States_LDS());
//            foreach (DataRow StatesRow in StatesTable.Rows)
//            {
//              #region The number of offices at each office level must be the same

//              #region US Senate - LDSOfficeLevelUSSenate
//              OfficesInLDCODE = db.Rows(sql.LDCODE4LdsStateCodeLDSTypeCode(
//                StatesRow["LDSStateCode"].ToString(), db.LDSOfficeLevelUSSenate));
//              OfficesInOffices = db.Rows_Offices_In_Class(StatesRow["StateCode"].ToString()
//                , db.Office_US_Senate);
//              if (OfficesInLDCODE != OfficesInOffices)
//              {
//                Report.Text += "<br><br><b>" + StatesRow["State"].ToString() + "</b> - " + db.Office4LDSTYPE(db.LDSOfficeLevelUSSenate.ToString());
//                Report.Text += "<br>" + OfficesInLDCODE.ToString()
//                  + " LDCODE Table rows for STATE:" + db.States_Str(StatesRow["StateCode"].ToString(), "LDSStateCode")
//                  + " TYPE:" + db.LDSOfficeLevelUSSenate.ToString()
//                  + "<br>" + OfficesInOffices.ToString() + " Offices Table rows for StateCode:" + StatesRow["StateCode"].ToString();

//                Report.Text += ReportLDCODEAndOffices(StatesRow, db.LDSOfficeLevelUSSenate, db.Office_US_Senate);

//              }
//              #endregion US Senate - LDSOfficeLevelUSSenate

//              #region US House - LDSOfficeLevelUSHouse
//              OfficesInLDCODE = db.Rows(sql.LDCODE4LdsStateCodeLDSTypeCode(
//                StatesRow["LDSStateCode"].ToString(), db.LDSOfficeLevelUSHouse));
//              OfficesInOffices = db.Rows_Offices_In_Class(StatesRow["StateCode"].ToString()
//                , db.Office_US_House);
//              if (OfficesInLDCODE != OfficesInOffices)
//              {
//                Report.Text += "<br><br><b>" + StatesRow["State"].ToString() + "</b> - " + db.Office4LDSTYPE(db.LDSOfficeLevelUSHouse.ToString());
//                Report.Text += "<br>" + OfficesInLDCODE.ToString()
//                  + " LDCODE Table rows for STATE:" + db.States_Str(StatesRow["StateCode"].ToString(), "LDSStateCode")
//                  + " TYPE:" + db.LDSOfficeLevelUSHouse.ToString()
//                  + "<br>" + OfficesInOffices.ToString() + " Offices Table rows for StateCode:" + StatesRow["StateCode"].ToString();

//                Report.Text += ReportLDCODEAndOffices(StatesRow, db.LDSOfficeLevelUSHouse, db.Office_US_House);

//              }
//              #endregion US House - LDSOfficeLevelUSHouse

//              #region State Senate - LDSOfficeLevelStateSenate
//              OfficesInLDCODE = db.Rows(sql.LDCODE4LdsStateCodeLDSTypeCode(
//                StatesRow["LDSStateCode"].ToString(), db.LDSOfficeLevelStateSenate));
//              OfficesInOffices = db.Rows_Offices_In_Class(StatesRow["StateCode"].ToString()
//                , db.Office_State_Senate);
//              if (OfficesInLDCODE != OfficesInOffices)
//              {
//                Report.Text += "<br><br><b>" + StatesRow["State"].ToString() + "</b> - " + db.Office4LDSTYPE(db.LDSOfficeLevelStateSenate.ToString());
//                Report.Text += "<br>" + OfficesInLDCODE.ToString()
//                  + " LDCODE Table rows for STATE:" + db.States_Str(StatesRow["StateCode"].ToString(), "LDSStateCode")
//                  + " TYPE:" + db.LDSOfficeLevelStateSenate.ToString()
//                  + "<br>" + OfficesInOffices.ToString() + " Offices Table rows for StateCode:" + StatesRow["StateCode"].ToString()
//                  + " OfficeLevel:" + db.Office_State_Senate.ToString();

//                Report.Text += ReportLDCODEAndOffices(StatesRow, db.LDSOfficeLevelStateSenate, db.Office_State_Senate);

//              }
//              #endregion State Senate - LDSOfficeLevelStateSenate

//              #region State House - LDSOfficeLevelStateHouse
//              OfficesInLDCODE = db.Rows_Offices_In_Class(
//                StatesRow["LDSStateCode"].ToString(), db.LDSOfficeLevelStateHouse);
//              OfficesInOffices = db.Rows_Offices_In_Class(StatesRow["StateCode"].ToString()
//                , db.Office_State_House);
//              if (OfficesInLDCODE != OfficesInOffices)
//              {
//                Report.Text += "<br><br><b>" + StatesRow["State"].ToString() + "</b> - " + db.Office4LDSTYPE(db.LDSOfficeLevelStateHouse.ToString());
//                Report.Text += "<br>" + OfficesInLDCODE.ToString()
//                  + " LDCODE Table rows for STATE:" + db.States_Str(StatesRow["StateCode"].ToString(), "LDSStateCode")
//                  + " TYPE:" + db.LDSOfficeLevelStateHouse.ToString()
//                  + "<br>" + OfficesInOffices.ToString() + " Offices Table rows for StateCode:" + StatesRow["StateCode"].ToString();

//                Report.Text += ReportLDCODEAndOffices(StatesRow, db.LDSOfficeLevelStateHouse, db.Office_State_House);


//              }
//              #endregion State House - LDSOfficeLevelStateHouse

//              #endregion The number of offices at each office level must be the same

//            }
//          }

//          #region The DC Ward offices for each district in LDCODE Table must be the same as in Offices Table
//          Report.Text += "<br><br>The DC Ward offices for each district in LDCODE Table must be the same as in Offices Table";
//          LDCODETable = db.Table(sql.LDCODE4LdsStateCodeLDSTypeCode("11", '2'));
//          foreach (DataRow LDCODERow in LDCODETable.Rows)
//          {
//            if (LDCODERow["DISTRICT"].ToString() != "000")//At-Large Ward Member should not be here
//            {
//              Report.Text += "<br>STATE:" + LDCODERow["STATE"].ToString()
//              + " TYPE:" + LDCODERow["TYPE"].ToString()
//              + " DISTRICT:" + LDCODERow["DISTRICT"].ToString()
//              + " NAME:" + LDCODERow["NAME"].ToString();
//              strDistrict = LDCODERow["DISTRICT"].ToString();
//              District = Convert.ToUInt16(strDistrict);
//              OfficeKey = "DCStateSenate" + District.ToString();

//              if (db.Is_Valid_Office(OfficeKey))
//                Office = db.Offices_Str(OfficeKey, "OfficeLine1") + " " + db.Offices_Str(OfficeKey, "OfficeLine2");
//              else
//                Office = "<br><B>No Offices row for this OfficeKey</B>";
//              Report.Text += " | OfficeKey:" + OfficeKey
//             + " Office:" + Office;
//            }
//          }
//          #endregion The DC Ward offices for each district must be the same

//          #region The DC ANC's offices for each district n LDCODE Table must be the same as in Offices Table
//          Report.Text += "<br><br>The DC ANC's offices for each district n LDCODE Table must be the same as in Offices Table";
//          LDCODETable = db.Table(sql.LDCODE4LdsStateCodeLDSTypeCode("11", '3'));
//          foreach (DataRow LDCODERow in LDCODETable.Rows)
//          {
//            Report.Text += "<br>STATE:" + LDCODERow["STATE"].ToString()
//            + " TYPE:" + LDCODERow["TYPE"].ToString()
//            + " DISTRICT:" + LDCODERow["DISTRICT"].ToString()
//            + " NAME:" + LDCODERow["NAME"].ToString();
//            District = Convert.ToUInt16(LDCODERow["DISTRICT"].ToString());
//            OfficeKey = "DCStateHouse" + District.ToString();

//            if (db.Is_Valid_Office(OfficeKey))
//              Office = db.Offices_Str(OfficeKey, "OfficeLine1") + " " + db.Offices_Str(OfficeKey, "OfficeLine2");
//            else
//              Office = "<br><B>No Offices row for this OfficeKey</B>";
//            Report.Text += " | OfficeKey:" + OfficeKey
//            + " Office:" + Office;
//          }
//          #endregion The DC ANC's offices for each district must be the same

//          #region The DC Ward offices for each district in Offices Table must be the same as in LDCODE Table
//          Report.Text += "<br><br>The DC Ward offices for each district in Offices Table must be the same as in LDCODE Table";
//          OfficesTable = db.Table_Offices("DC", db.Office_State_Senate);
//          foreach (DataRow OfficesRow in OfficesTable.Rows)
//          {
//            Report.Text += "<br>OfficeKey:" + OfficesRow["OfficeKey"].ToString()
//            + " OfficeLevel:" + OfficesRow["OfficeLevel"].ToString()
//            + " DistrictCode:" + OfficesRow["DistrictCode"].ToString()
//            + "<BR>OfficeLine1 & 2:" + OfficesRow["OfficeLine1"].ToString()
//            + " " + OfficesRow["OfficeLine2"].ToString();

//            LDCODEROW = db.Row_Optional(sql.LDCODE4LdsStateCodeLDSTypeCodeLDSDistrict("11", '2', OfficesRow["DistrictCode"].ToString()));

//            if (LDCODEROW != null)
//              NAME = " | NAME:" + LDCODEROW["NAME"].ToString();
//            else
//              NAME = " | No LDCODE row for STATE:11 TYPE:2 DISTRICT:" + OfficesRow["DistrictCode"].ToString();
//            Report.Text += NAME;
//          }

//          #endregion The DC Ward offices for each district in Offices Table must be the same as in LDCODE Table

//          #region The DC ANC offices for each district in Offices Table must be the same as in LDCODE Table
//          Report.Text += "<br><br>The DC ANC offices for each district in Offices Table must be the same as in LDCODE Table";
//          OfficesTable = db.Table_Offices("DC", db.Office_State_House);
//          foreach (DataRow OfficesRow in OfficesTable.Rows)
//          {
//            Report.Text += "<br>OfficeKey:" + OfficesRow["OfficeKey"].ToString()
//            + " OfficeLevel:" + OfficesRow["OfficeLevel"].ToString()
//            + " DistrictCode:" + OfficesRow["DistrictCode"].ToString()
//            + "<BR>OfficeLine1 & 2:" + OfficesRow["OfficeLine1"].ToString()
//            + " " + OfficesRow["OfficeLine2"].ToString();

//            LDCODEROW = db.Row_Optional(sql.LDCODE4LdsStateCodeLDSTypeCodeLDSDistrict("11", '3', OfficesRow["DistrictCode"].ToString()));

//            if (LDCODEROW != null)
//              NAME = " | NAME:" + LDCODEROW["NAME"].ToString();
//            else
//              NAME = " | No LDCODE row for STATE:11 TYPE:3 DISTRICT:" + OfficesRow["DistrictCode"].ToString();
//            Report.Text += NAME;
//          }
//          #endregion The DC ANC offices for each district in Offices Table must be the same as in LDCODE Table

//          #endregion Offices Table

//          #endregion LDCODE Table Problems
//        }

//        Msg.Text = db.Msg("LDS Tables checked. See error report below.");

//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }

//    }
//    //2) Update Master Table
//    protected void ButtonUpdateMaster_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckTextBoxs4HtmlAndIlleagalInput();
//        CheckVersionAndUpdateDate();
//        db.Master_Update_Str("LDSVersion", TextBoxVersion.Text.Trim());
//        db.Master_Update_Date("LDSUpdateDate", Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()));
//        LDSVersion.Text = db.Master_Str("LDSVersion");
//        Msg.Text = db.Ok("Version and Update Date in Master Table was Updated to: " + TextBoxVersion.Text.Trim()
//          + " and " + TextBoxUpdateDate.Text.Trim());

//        LoadMasterControls();
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }
//    //3) Update States Table
//    protected void ButtonUpdateStates_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckTextBoxs4HtmlAndIlleagalInput();
//        Msg.Text = string.Empty;
//        Report.Text = string.Empty;//to clear any old report and in case button is clicked a second time

//        CheckVersionAndUpdateDate();

//        if (db.Master_Str("LDSVersion") != TextBoxVersion.Text.Trim())
//          throw new ApplicationException("You need to run [Update Master Table] before you perform this operation.");

//        #region Loop LDS CENSUS Table extracting and updating States Table info only
//        //SQL - CENSUS Table
//        #region Exclude National (STATE=00) and County (COUNTY=000) records in LDS CENSUS Table
//        string SQL = "SELECT STATE,NAME FROM CENSUS WHERE STATE <> '00' AND COUNTY = '000'";
//        DataTable CENSUSTable = db.Table(SQL);
//        #endregion
//        int States = 0;
//        int StatesUpdated = 0;
//        int StatesNotUpdated = 0;
//        foreach (DataRow CENSUSRow in CENSUSTable.Rows)
//        {
//          #region Update the State Table row with LDS Data
//          //Get State Code (like VA) where State Name matches LDS State Name
//          string StateCode = db.Single_Key_Str_Optional("States", "StateCode", "State", CENSUSRow["NAME"].ToString().Trim());
//          if (StateCode != string.Empty)
//          {
//            string UpdateSQL = "UPDATE States SET "
//              + " LDSStateCode = " + db.SQLLit(CENSUSRow["STATE"].ToString().Trim())
//              + " ,LDSStateName = " + db.SQLLit(CENSUSRow["NAME"].ToString().Trim())
//              + " ,LDSVersion = " + db.SQLLit(TextBoxVersion.Text.Trim())
//              + " ,LDSUpdateDate = " + db.SQLLit(TextBoxUpdateDate.Text.Trim())
//              + " WHERE StateCode= " + db.SQLLit(StateCode);
//            db.ExecuteSQL(UpdateSQL);

//            Report.Text += "<br>State Updated: " + CENSUSRow["NAME"].ToString().Trim();
//            States += 1;
//            StatesUpdated += 1;
//          }
//          else//no matching row in States Table
//          {
//            StatesNotUpdated += 1;
//            Report.Text += "<br><span class=TSmallColor> No matching State Name is States Table for LDS CENSUS Table NAME: "
//              + CENSUSRow["NAME"].ToString().Trim() + "</span>";
//          }
//          #endregion
//        }
//        Report.Text += "<br><br>" + States.ToString() + " LDS CENSUS Table Rows Processed:";
//        Report.Text += "<br>" + StatesUpdated.ToString() + " State Rows Updated Sucessfully with LDS CENSUS Table data.";
//        Report.Text += "<br>" + StatesNotUpdated.ToString() + " State Rows NOT Updated because there was no 'State' column in States Table matching the LDS CENSUS NAME column";
//        #endregion

//        db.Master_Update_Date("LDSUpdateDate", Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()));
//        Msg.Text = db.Msg("Updating State Table completed. Check report below for possible problems.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }
//    //4) Update Counties Table
//    protected void ButtonUpdateCountiesTable_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckTextBoxs4HtmlAndIlleagalInput();
//        Msg.Text = string.Empty;
//        Report.Text = string.Empty;//to clear any old report and in case button is clicked a second time

//        CheckVersionAndUpdateDate();
//        int TotalCounties = 0;
//        int TotalCountiesAdded = 0;

//        #region SQL Update County Table rows one State at a time
//        string SQL = string.Empty;
//        SQL += "SELECT StateCode,State,LDSVersion,LDSStateCode ";
//        SQL += " FROM States";
//        SQL += " WHERE LDSStateCode <> '00' ";// 00 is USA
//        SQL += " AND StateCode != 'US' ";
//        SQL += " AND StateCode != 'U1' ";
//        SQL += " AND StateCode != 'U2' ";
//        SQL += " AND StateCode != 'U3' ";
//        SQL += " AND StateCode != 'AS' ";
//        SQL += " AND StateCode != 'GU' ";
//        SQL += " AND StateCode != 'VI' ";
//        SQL += " AND StateCode != 'PR' ";
//        SQL += " ORDER BY StateCode";
//        #endregion
//        DataTable StatesTable = db.Table(SQL);
//        foreach (DataRow StateRow in StatesTable.Rows)
//        {
//          #region
//          if (StateRow["LDSVersion"].ToString().Trim() != TextBoxVersion.Text.Trim())
//            throw new ApplicationException("You need to run [Update States Table] before you perform this operation.");

//          #region State inits
//          int LDSCounties = 0;
//          int CountiesAdded = 0;
//          int CountiesUpdated = 0;
//          string Condition = string.Empty;
//          int CountyRows = 0;
//          #endregion

//          #region CENSUS Table row of counties for the paricular state
//          //Only County records in the LDS CENSUS table (000 indicates a state)
//          SQL = "SELECT * FROM CENSUS ";
//          SQL += " WHERE COUNTY <> '000' ";//not a State or USA
//          SQL += " AND STATE = " + db.SQLLit(StateRow["LDSStateCode"].ToString().Trim());
//          DataTable CENSUSTable = db.Table(SQL);
//          #endregion
//          foreach (DataRow CENSUSRow in CENSUSTable.Rows)
//          {
//            #region County Row in State

//            #region Add or update the single County row for the single State
//            //Condition = "StateCode = " + db.SQLLit(StateRow["StateCode"].ToString().Trim())
//            //  + " AND CountyCode = " + db.SQLLit(CENSUSRow["COUNTY"].ToString().Trim());
//            //CountyRows = db.RowsInTableWithCondition("Counties", Condition);
//            string SQLCounties = "SELECT CountyCode FROM Counties";
//            SQLCounties += " WHERE StateCode = " + db.SQLLit(StateRow["StateCode"].ToString().Trim());
//            SQLCounties += " AND CountyCode = " + db.SQLLit(CENSUSRow["COUNTY"].ToString().Trim());
//            CountyRows = db.Rows(SQLCounties);

//            if (CountyRows == 0)//Add County if row does not exist
//            {
//              #region INSERT INTO Counties
//              string InsertSQL = "INSERT INTO Counties ("
//                + " StateCode"
//                + ",County"
//                + ",CountyCode"
//                + ",LDSStateCode"
//                + ",LDSCountyCode"
//                + ",LDSCounty"
//                + ",LDSVersion"
//                + ",LDSUpdateDate"
//                + ")"
//                + " VALUES("
//                + db.SQLLit(StateRow["StateCode"].ToString().Trim())//StateCode (key)
//                + "," + db.SQLLit(CENSUSRow["NAME"].ToString().Trim()) //County (key)
//                + "," + db.SQLLit(CENSUSRow["COUNTY"].ToString().Trim()) //CountyCode
//                + "," + db.SQLLit(StateRow["LDSStateCode"].ToString().Trim()) //LDSStateCode
//                + "," + db.SQLLit(CENSUSRow["COUNTY"].ToString().Trim()) //LDSCountyCode
//                + "," + db.SQLLit(CENSUSRow["NAME"].ToString().Trim()) //LDSCounty
//                + "," + db.SQLLit(TextBoxVersion.Text.Trim())//LDSVersion
//                + "," + db.SQLLit(TextBoxUpdateDate.Text.Trim())//LDSVersion
//                + ")";
//              db.ExecuteSQL(InsertSQL);
//              #endregion
//              CountiesAdded += 1;
//              TotalCountiesAdded++;
//              Report.Text += "<br>ADDING County: " + StateRow["StateCode"].ToString() + " / " + CENSUSRow["COUNTY"].ToString();
//            }
//            else if (CountyRows == 1)//Update County if row exists
//            {
//              #region UPDATE Counties
//              string UpdateSQL = "UPDATE Counties SET "
//                + " LDSStateCode = " + db.SQLLit(StateRow["LDSStateCode"].ToString().Trim())//LDSStateCode
//                + " ,LDSCountyCode = " + db.SQLLit(CENSUSRow["COUNTY"].ToString().Trim())//LDSCountyCode
//                + " ,LDSCounty = " + db.SQLLit(CENSUSRow["NAME"].ToString().Trim())//LDSCounty
//                + " ,LDSVersion = " + db.SQLLit(TextBoxVersion.Text.Trim())//LDSVersion
//                + " ,LDSUpdateDate = " + db.SQLLit(TextBoxUpdateDate.Text.Trim())//LDSVersion
//                + " WHERE StateCode= " + db.SQLLit(StateRow["StateCode"].ToString().Trim())//StateCode (key)
//                + " AND County = " + db.SQLLit(CENSUSRow["NAME"].ToString().Trim());//County (key)
//              db.ExecuteSQL(UpdateSQL);
//              #endregion
//              CountiesUpdated += 1;
//            }
//            else
//            {
//              throw new ApplicationException("There is more than 1 row in Counties Table where StateCod = " + StateRow["StateCode"].ToString() + " and CountyCode = " + CENSUSRow["COUNTY"].ToString());
//            }
//            #endregion

//            LDSCounties++;
//            TotalCounties++;
//            #endregion
//          }

//          #region Report State Stats
//          Report.Text += "<br><br>" + LDSCounties.ToString() + " LDS CENSUS County Records Processed for: "
//            + StateCache.GetStateName(StateRow["StateCode"].ToString());
//          Report.Text += "<br>" + CountiesAdded.ToString() + " Added";
//          Report.Text += "<br>" + CountiesUpdated.ToString() + " Updated";
//          #endregion

//          #region Update States record with LDS Version and counties
//          string UpdateStateSQL = "UPDATE States SET "
//            + " LDSCountiesVersion = " + db.SQLLit(TextBoxVersion.Text.Trim())
//            + " ,LDSCounties = " + db.SQLLit(LDSCounties.ToString())
//            + " WHERE StateCode= " + db.SQLLit(StateRow["StateCode"].ToString().Trim());
//          db.ExecuteSQL(UpdateStateSQL);
//          #endregion

//          #endregion
//        }
//        #region Report.Text and update counts
//        Report.Text += "<br><br>" + TotalCounties.ToString() + " LDS CENSUS County Records Processed";
//        Report.Text += "<br>" + TotalCountiesAdded.ToString() + " ---NEW LDS COUNTIES ADDED-----";
//        #endregion

//        db.Master_Update_Date("LDSUpdateDate", Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()));
//        Msg.Text = db.Msg("Updating Counties Table completed. Check Report below for possible problems.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }

//    }
//    //5) Update Offices Table
//    protected void ButtonUpdateOffices_Click(object sender, EventArgs e)
//    {
//      CheckTextBoxs4HtmlAndIlleagalInput();
//      Server.ScriptTimeout = 600;//600 sec = 10 min
//      DateTime RunTimeStatesStart = DateTime.Now;
//      try
//      {
//        Msg.Text = string.Empty;
//        Report.Text = string.Empty;//to clear any old report and in case button is clicked a second time
//        //int Incumbents = 0;
//        DataTable LDCODETable = null;
//        DataTable OfficeTable = null;
//        string StateCode = string.Empty;
//        int Office_Class = 0;
//        string DistrictCode = string.Empty;

//        CheckVersionAndUpdateDate();

//        #region Update the LDS STATE, TYPE or DISTRICT in Offices Table when the LDS NAME and STATE are the Same to synchronize the office rows in Offices and LDCODE Tables
//        LDCODETable = db.Table(sql.LDCODE());
//        foreach (DataRow LDCODERow in LDCODETable.Rows)
//        {
//          StateCode = db.StateCode4LDSStateCode(LDCODERow["STATE"].ToString());
//          Office_Class = db.OfficeLevel4LDSTYPE(LDCODERow["TYPE"].ToString());
//          DistrictCode = LDCODERow["DISTRICT"].ToString();
//          //OfficeTable = db.Table(sql.Office4LDSStateCodeGovernor(LDCODERow["STATE"].ToString()));
//          if (DistrictCode == "000")
//            //OfficeTable = db.Table(sql.RowsOffices4StateCodeOfficeLevel(StateCode, Office_Class));
//            OfficeTable = db.Table_Offices(StateCode, Office_Class);
//          else
//            OfficeTable = db.Table(Office4StateCodeOfficeLevelDistrictCode(StateCode, Office_Class, DistrictCode));
//          if (OfficeTable.Rows.Count >= 1)
//          {
//            foreach (DataRow OfficeRow in OfficeTable.Rows)
//            {
//              #region update LDSStateCode, LDSTypeCode, LDSDistrictCode of Offices Table row if the LDCODE row's STATE, TYPE or DISTRICT don't match (commented out)
//              db.Offices_Update_Str(OfficeRow["OfficeKey"].ToString(), "LDSStateCode", LDCODERow["STATE"].ToString());
//              db.Offices_Update_Str(OfficeRow["OfficeKey"].ToString(), "LDSTypeCode", LDCODERow["TYPE"].ToString());
//              db.Offices_Update_Str(OfficeRow["OfficeKey"].ToString(), "LDSDistrictCode", LDCODERow["DISTRICT"].ToString());
//              db.Offices_Update_Str(OfficeRow["OfficeKey"].ToString(), "LDSOffice", LDCODERow["NAME"].ToString());
//              db.Offices_Update_Str(OfficeRow["OfficeKey"].ToString(), "LDSVersion", db.Master_Str("LDSVersion"));
//              db.Offices_Update_Str(OfficeRow["OfficeKey"].ToString(), "LDSUpdateDate", db.Master_Str("LDSUpdateDate"));
//              #endregion
//            }
//          }
//          else
//          {
//            Report.Text = "<br>There is no Offices row for LDCODE row: STATE(StateCode):" + LDCODERow["STATE"].ToString() + "(" + StateCode + ")"
//              + " TYPE(OfficeLevel):" + LDCODERow["TYPE"].ToString() + "(" + Office_Class.ToString() + ")"
//              + " DISTRICT:" + LDCODERow["DISTRICT"].ToString();
//          }
//        }
//        #endregion

//        #region no need to add offices
//#if false
//          #region Each StateHouse row in the LDCODE Table should have only 1 OR 2 matching row(s) in the Offices Table for State and Office Name
//          // The LDCODE columns STATE and NAME are used to match the Offices columns LDSStateCode, LDSOffice
//          LDCODETable = db.Table(sql.LDCODE4TYPE("3"));
//          foreach (DataRow LDCODERow in LDCODETable.Rows)
//          {
//            #region Offices Table Rows
//            StateCode = db.StateCode4LDSStateCode(LDCODERow["STATE"].ToString());
//            Office_Class = db.OfficeLevel4LDSTYPE(LDCODERow["TYPE"].ToString());
//            DistrictCode = LDCODERow["DISTRICT"].ToString();
//            OfficeTable = db.Table(Office4StateCodeOfficeLevelDistrictCode(StateCode, Office_Class, DistrictCode));
//            #endregion

//            #region check of synchronization of office rows
//            if (OfficeTable.Rows.Count == 0)
//            {
//              if ((LDCODERow["STATE"].ToString() == "16") || (LDCODERow["STATE"].ToString() == "53"))
//              {
//                #region ID and WA have require 2 Offices rows for each LDCODE row (ID positions A & B) (WA P1 & P2)
//                //Add 2 office Rows
//                AddLDSOffice(LDCODERow);
//                AddLDSOffice(LDCODERow);
//                #endregion
//              }
//              else
//              {
//                AddLDSOffice(LDCODERow);
//              }
//            }
//            else if ((OfficeTable.Rows.Count == 1) || (OfficeTable.Rows.Count == 2))
//            {
//              #region 1 or 2 Office rows
//              if ((LDCODERow["STATE"].ToString() == "16") || (LDCODERow["STATE"].ToString() == "53"))
//              {
//                #region ID and WA have 2 Offices rows for each LDCODE row (ID positions A & B) (WA P1 & P2)
//                if (OfficeTable.Rows.Count > 2)
//                  Report.Text += "<br><br><span class=TSmallColor>There is are more than 2 office rows in the Offices Table with LDSStateCode: " + LDCODERow["STATE"].ToString()
//                    + " LDSOffice: " + LDCODERow["NAME"].ToString() + "."
//                    + "<br>Delete or tag for deletion the Office rows to fix: </span>";
//                #endregion
//              }
//              else
//              {
//                #region All other State have 1 Office row for each LDCODE row
//                if (OfficeTable.Rows.Count > 1)
//                {
//                  Report.Text += "<br><br>There are more than 1 office row in the Offices Table for LDCODE Row where"
//                    + " LDSStateCode: " + LDCODERow["STATE"].ToString()
//                    + " | LDSOffice: " + LDCODERow["NAME"].ToString() + "."
//                    + " <br>The Offices rows are: ";
//                  foreach (DataRow OfficeRow in OfficeTable.Rows)
//                  {
//                    Report.Text += "<br>OfficeKey: " + OfficeRow["OfficeKey"].ToString()
//                    + " | Office: " + OfficeRow["OfficeLine1"].ToString() + " " + OfficeRow["OfficeLine2"].ToString();
//                  }
//                  Report.Text += "<br><span class=TSmallColor>Delete or tag for deletion all Office row(s) with a different office title, leaving only 1 (except ID and WA with 2). </span>";
//                }
//                #endregion
//              }
//              #endregion
//            }
//            else //more than 2 offices
//            {
//              #region more than 2 office rows
//              Report.Text += "<br><br>There are more than 1 office row in the Offices Table for LDCODE Row where"
//            + " LDSStateCode: " + LDCODERow["STATE"].ToString()
//                + " LDSOffice: " + LDCODERow["NAME"].ToString() + "."
//                + " <br>The Offices rows are: ";
//              foreach (DataRow OfficeRow in OfficeTable.Rows)
//              {
//                Report.Text += "<br>OfficeKey: " + OfficeRow["OfficeKey"].ToString()
//                + " | Office: " + OfficeRow["OfficeLine1"].ToString() + " " + OfficeRow["OfficeLine2"].ToString();
//              }
//              Report.Text += "<br><span class=TSmallColor>Delete or tag for deletion all Office row(s) with a different office title, leaving only 1 (except ID and WA with 2). </span>";
//              #endregion
//            }
//            #endregion
//          }
//          #endregion Each StateHouse row in the LDCODE Table should have only 1 OR 2 matching row(s) in the Offices Table for State and Office Name

//          #region For all the other offices in LDCODE there should be only 1 Offices row with matching office name and State. If 0 Offices Table row create a row
//          LDCODETable = db.Table(sql.LDCODE4NotTYPE("3"));
//          foreach (DataRow LDCODERow in LDCODETable.Rows)
//          {
//            #region Offices Table Rows
//            if ((LDCODERow["STATE"].ToString() == "11") && (LDCODERow["TYPE"].ToString() == "2") && (LDCODERow["DISTRICT"].ToString() == "000"))
//            {
//              //skip DC At Large Council 
//            }
//            else
//            {
//              StateCode = db.StateCode4LDSStateCode(LDCODERow["STATE"].ToString());
//              Office_Class = db.OfficeLevel4LDSTYPE(LDCODERow["TYPE"].ToString());
//              DistrictCode = LDCODERow["DISTRICT"].ToString();
//              if (Office_Class != 4) //All but Governor
//                OfficeTable = db.Table(Office4StateCodeOfficeLevelDistrictCode(StateCode, Office_Class, DistrictCode));
//              else
//                OfficeTable = db.Table_Offices_Governor(StateCode);
//            #endregion

//              if (OfficeTable.Rows.Count == 0)
//              {
//                AddLDSOffice(LDCODERow);
//              }

//              else if (OfficeTable.Rows.Count > 1)
//              {
//                #region Report more than one row
//                Report.Text += "<br><br>There is are more than 1 office row in the Offices Table  for LDCODE Row where"
//                + " LDSStateCode: " + LDCODERow["STATE"].ToString()
//                + " LDSOffice: " + LDCODERow["NAME"].ToString() + "."
//                + " <br>The Offices rows are: ";

//                foreach (DataRow OfficeRow in OfficeTable.Rows)
//                {
//                  Report.Text += "<br>OfficeKey: " + OfficeRow["OfficeKey"].ToString()
//                  + " | Office: " + OfficeRow["OfficeLine1"].ToString() + " " + OfficeRow["OfficeLine2"].ToString();
//                }

//                Report.Text += "<br><span class=TSmallColor>Delete or tag for deletion all Office row(s) with a different office title, leaving only 1. </span>";
//                #endregion
//              }
//            }

//          }
//          #endregion

//          #region Tag for Deletion all Offices rows where the LDSStateCode and LDSOffice are empty only for Office Levels 2,3,5,6 (LDS Reported offices) and report
//          DataTable OfficesTable = db.Table(sql.Offices4LDS());
//          foreach (DataRow OfficeRow in OfficesTable.Rows)
//          {
//            if ((OfficeRow["LDSOffice"].ToString() == string.Empty) || (OfficeRow["LDSStateCode"].ToString() == string.Empty))
//            {
//              Report.Text += "<br><br>The LDSOffice and/or LDSStateCode for this Office row is empty. ";

//              Report.Text += "<br>OfficeKey: " + OfficeRow["OfficeKey"].ToString()
//              + " | Office: " + OfficeRow["OfficeLine1"].ToString() + " " + OfficeRow["OfficeLine2"].ToString();

//              Report.Text += "<br><span class=TSmallColor>Delete or tag for deletion this Office row. </span>";
//            }
//          }
//          #endregion
//#endif
//        #endregion

//        #region Run Time
//        DateTime RunTimeStatesEnd = DateTime.Now;
//        TimeSpan RunTimeStatesTimeSpan = RunTimeStatesEnd.Subtract(RunTimeStatesStart);
//        int Hours = RunTimeStatesTimeSpan.Hours;
//        int Minutes = RunTimeStatesTimeSpan.Minutes;
//        int Seconds = RunTimeStatesTimeSpan.Seconds;
//        OfficesRunTime.Text = Hours.ToString() + ":" + Minutes.ToString() + ":" + Seconds.ToString();
//        db.Master_Update_Str("LDSOfficesRunTime", OfficesRunTime.Text);
//        db.Master_Update_Date("LDSDateCompletedOffices", DateTime.Now);
//        #endregion

//        db.Master_Update_Date("LDSUpdateDate", Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()));

//        LoadMasterControls();

//        Msg.Text = db.Msg("Update Offices Table completed.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }
//    //6) Update Politicians Table
//    protected void ButtonUpdatePoliticians_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckTextBoxs4HtmlAndIlleagalInput();
//        Server.ScriptTimeout = 6000;//6000 sec = 100 min = 1.66 hr
//        DateTime RunTimeStatesStart = DateTime.Now;

//        #region Inits
//        Report.Text = string.Empty;
//        Msg.Text = string.Empty;//to clear any old report and in case button is clicked a second time
//        string StateCode = string.Empty;
//        string OfficeKey = string.Empty;
//        string FName = string.Empty;
//        string LName = string.Empty;
//        //int Office_Class = 0;
//        int PoliticiansFoundMatchingLDSNum = 0;
//        int PoliticiansNotFound = 0;
//        int PoliticiansVacant = 0;
//        DataRow PoliticianRow = null;
//        string SQL = string.Empty;
//        #endregion

//        Report.Text += "Politicians in LEGIDYY Table Not Found in Politicians Table<br>";

//        CheckVersionAndUpdateDate();

//        #region Truncate and Build Table of Not Processed Politicians
//        db.ExecuteSQL("TRUNCATE TABLE LEGIDYYNotProcessed");
//        #endregion

//        DataTable LEGIDYYTable = null;
//        LEGIDYYTable = db.Table(sql.LEGIDYY());
//        //LEGIDYYTable = db.Table(sql.LEGIDYYExcludingDCIDWA());//Excludes DC, ID, and WA
//        foreach (DataRow LEGIDYYRow in LEGIDYYTable.Rows)
//        {
//          if (LEGIDYYRow["LAST_NAME"].ToString().Trim() != "VACANT")
//          {
//            #region Non-Vacant LDS Politicians in LEGIDYY Table

//            #region 1st Find Politician with the same LEG_ID_NUM
//            SQL = "SELECT PoliticianKey,LName,LDSStateCode,LDSTypeCode "
//             + " FROM Politicians "
//             + " WHERE LDSLegIDNum = " + db.SQLLit(LEGIDYYRow["LEG_ID_NUM"].ToString().Trim());
//            PoliticianRow = db.Row_Optional(SQL);
//            #endregion 1st Find Politician with the same LEG_ID_NUM

//            if (PoliticianRow == null)
//            {
//              #region commented out - redo - 2nd Find Politician with same First Name, Last Name and Office
//              // FName = LEGIDYYRow["FIRST_NAME"].ToString();
//              // LName = LEGIDYYRow["LAST_NAME"].ToString();
//              // StateCode = db.StateCode4LDSStateCode(LEGIDYYRow["STATE"].ToString());
//              // Office_Class = db.OfficeLevel4LDSTYPE(LEGIDYYRow["TYPE"].ToString());
//              // OfficeKey = db.OfficeKey(
//              //   OfficeLevel
//              // , StateCode
//              // , string.Empty //CountyCode
//              // , string.Empty //LocalCode
//              // , LEGIDYYRow["DISTRICT"].ToString() //DistrictCode
//              // , string.Empty //DistrictCodeAlpha
//              // , string.Empty //Office not needed
//              //);
//              // SQL = "SELECT PoliticianKey,LName,LDSStateCode,LDSTypeCode "
//              // + " FROM Politicians "
//              // + " WHERE FName = " + db.SQLLit(FName)
//              // + " AND LName = " + db.SQLLit(LName)
//              // + " AND OfficeKey = " + db.SQLLit(OfficeKey);
//              // PoliticianRow = db.Row_Optional(SQL);
//              // #endregion commented out - redo - 2nd Find Politician with same First Name, Last Name and Office
//              #endregion commented out - redo - 2nd Find Politician with same First Name, Last Name and Office
//            }

//            if (PoliticianRow != null)
//            {
//              #region Found Single Politician Row in Politicians Table
//              if (db.Is_Mulitple_LDSLegIDNum(LEGIDYYRow["LEG_ID_NUM"].ToString()))
//              {
//                Report.Text += "<br><br><span class=TSmallColor>There is more than one politician for LEG_ID_NUM: "
//                  + LEGIDYYRow["LEG_ID_NUM"].ToString() + "</span>";
//              }
//              else
//              {
//                PoliticiansFoundMatchingLDSNum++;

//                //Report.Text += db.xLDS_Data_Politicians_Update(
//                //  LEGIDYYRow
//                //  , PoliticianRow["PoliticianKey"].ToString()
//                //  , TextBoxVersion.Text.Trim()
//                //  , Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()));

//                //Report.Text += db.OfficesOfficialsUpdate4Politician(
//                //  LEGIDYYRow
//                //  , PoliticianRow["PoliticianKey"].ToString()
//                //  , TextBoxVersion.Text.Trim()
//                //  , Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()));
//              }
//              #endregion
//            }
//            else
//            {
//              #region Politician Not Found - increment count, add to not processed table and report
//              PoliticiansNotFound++;
//              MsgPoliticianNotFound(LEGIDYYRow);
//              Add2LEGIDYYNotProcessedTable(LEGIDYYRow);
//              #endregion
//            }
//            #endregion Non-Vacant LDS Politicians in LEGIDYY Table
//          }
//          else
//          {
//            #region Just count Vacant LDS Politicians in LEGIDYY Table
//            PoliticiansVacant++;
//            //Report.Text += db.OfficesOfficialsUpdate4Vacant(
//            //  LEGIDYYRow
//            //  , TextBoxVersion.Text.Trim()
//            //  , Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()));

//            #endregion
//          }
//        }
//        Report.Text += "<br><br>Politicians Updated with Matching LEG_ID_NUM: " + PoliticiansFoundMatchingLDSNum.ToString()
//          //+ "<br>DC, ID, WA State House Politicians, requiring special processin, Updated with Matching LEG_ID_NUM: " + LDSStateHouseRows.ToString()
//          + "<br>Politicians Not Found as listed above and added to LEGIDYYNotProcessed: " + PoliticiansNotFound.ToString()
//          + "<br>Politicians Vacant: " + PoliticiansVacant.ToString();

//        #region Set report flags for Elected Officials Reports as not current
//        db.Master_Update_Bool("IsUSSenateOfficialsReportCurrent", false);
//        db.Master_Update_Bool("IsUSHouseOfficialsReportCurrent", false);
//        DataTable StatesTable = db.Table(sql.States_51());
//        foreach (DataRow StateRow in StatesTable.Rows)
//        {
//          db.Report_Officials_Set_Current_Not_And_When(StateRow["StateCode"].ToString());
//        }
//        #endregion Set report flags for Elected Officials Reports as not current

//        #region Run Time
//        DateTime RunTimeStatesEnd = DateTime.Now;
//        TimeSpan RunTimeStatesTimeSpan = RunTimeStatesEnd.Subtract(RunTimeStatesStart);
//        int Hours = RunTimeStatesTimeSpan.Hours;
//        int Minutes = RunTimeStatesTimeSpan.Minutes;
//        int Seconds = RunTimeStatesTimeSpan.Seconds;
//        PoliticiansRunTime.Text = Hours.ToString() + ":" + Minutes.ToString() + ":" + Seconds.ToString();
//        db.Master_Update_Str("LDSPoliticiansRunTime", PoliticiansRunTime.Text);
//        db.Master_Update_Date("LDSDateCompletedPoliticians", DateTime.Now);
//        #endregion

//        db.Master_Update_Date("LDSUpdateDate", Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()));

//        LoadMasterControls();

//        Msg.Text = db.Msg("Update Politicians Table completed.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }
//    //7) Add New Politicians to Politicians Table
//    protected void ButtonAddNewPoliticians_Click(object sender, EventArgs e)
//    {
//      Response.Redirect("/Master/LDSNewPoliticiansAdd.aspx");
//    }
//    //8) Update OfficesOfficials Table
//    protected void ButtonUpdateOfficesOfficials_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckVersionAndUpdateDate();

//        #region Inits
//        Report.Text = string.Empty;

//        int OfficeRows = 0;//rows in Offices Table
//        int LEGIDYYPoliticianRows = db.Rows_Table("LEGIDYY");//rows in LEGIDYY Table
//        int LEGIDYYPoliticianRowsProcessed = 0;
//        int OfficesUpdated = 0;

//        //int BadOfficeKeys = 0;
//        int OfficesUnchanged = 0;
//        int Vacant = 0;
//        int BadPoliticianKeys = 0;
//        int Inserted = 0;

//        int Deleted = 0;

//        DataTable OfficesTable = null;
//        #endregion

//        #region Process LEGIDYY updating OfficesOfficials rows

//        CheckTextBoxs4HtmlAndIlleagalInput();
//        Server.ScriptTimeout = 6000;//6000 sec = 100 min = 1.66 hr
//        DateTime RunTimeStatesStart = DateTime.Now;

//        #region Normal case of one office on ballots for this/these LEGIDYY elected politicians
//        //ID and WA StateHouse Offices and all DC offices not included - they are done seperately below
//        string SQL = string.Empty;
//        SQL += " SELECT";
//        SQL += " Offices.OfficeKey";
//        SQL += ",Offices.StateCode ";
//        SQL += ",Offices.OfficeLevel";
//        SQL += ",Offices.DistrictCode";
//        SQL += ",Offices.DistrictCodeAlpha";
//        SQL += " FROM Offices";
//        SQL += " WHERE (StateCode != 'DC')";
//        SQL += " AND (";
//        SQL += " ((OfficeLevel = 2)";
//        SQL += " OR (OfficeLevel = 3)";
//        SQL += " OR (OfficeLevel = 5)";
//        SQL += " OR (OfficeLevel = 6)";
//        SQL += " OR ((OfficeLevel = 4) AND (SUBSTRING(OfficeKey,3,8) ='Governor')))";//only Governors for non-DC States
//        SQL += " AND DistrictCodeAlpha = ''";//ID and WA have a DistrictCodeAlpha
//        SQL += " )";
//        SQL += " ORDER BY StateCode,OfficeLevel,DistrictCode,DistrictCodeAlpha";
//        //OfficesTable = db.Table(sqlOffices());
//        OfficesTable = db.Table(SQL);
//        OfficeRows += OfficesTable.Rows.Count;
//        #region Note
//        //Testing whether data is newer was commented out so:
//        //All rows in OfficesOfficials EXCEPT DC are updated
//        #endregion Note
//        foreach (DataRow OfficeRow in OfficesTable.Rows)
//        {
//          string OfficeKeyTest = OfficeRow["OfficeKey"].ToString();
//          #region One Office
//          //if (Is_More_Recent_OfficesOfficials_Data(OfficeRow["OfficeKey"].ToString(), ref OfficesUnchanged))
//          //{
//          #region Delete ALL OfficesOfficials rows for office
//          Deleted += db.Rows(sqlOfficesOfficials(OfficeRow["OfficeKey"].ToString()));
//          db.ExecuteSQL(
//              sql_OfficesOfficials_Delete(OfficeRow["OfficeKey"].ToString())
//            )
//            ;
//          #endregion Delete ALL OfficesOfficials rows for office
//          //}

//          #region Table of all the LDS politicians for StateCode, OfficeLevel, Legislative Distirct
//          DataTable LEGIDYYTable = db.Table(sqlLEGIDYY(
//            db.LDSState4StateCode(OfficeRow["StateCode"].ToString())
//            , db.LDSTYPE4OfficeLevel(
//                OfficeRow["StateCode"].ToString()
//               , Convert.ToInt16(OfficeRow["OfficeLevel"].ToString()))
//            , db.LDSDISTRICT4DistrictCode(OfficeRow["DistrictCode"].ToString()))
//            );
//          #endregion Table of all the LDS politicians for StateCode, OfficeLevel, Legislative Distirct
//          foreach (DataRow LEGIDYYRow in LEGIDYYTable.Rows)
//          {
//            //if (Is_More_Recent_OfficesOfficials_Data(OfficeRow["OfficeKey"].ToString(), ref OfficesUnchanged))
//            //{
//            #region Insert 1 or more OfficesOfficials rows for Office
//            LEGIDYYPoliticianRowsProcessed++;
//            Inserted += OfficesOfficials_Insert(
//              OfficeRow["OfficeKey"].ToString()
//              , LEGIDYYRow["LEG_ID_NUM"].ToString()
//              , ref BadPoliticianKeys
//              , ref Vacant
//              );
//            #endregion Insert 1 or more OfficesOfficials rows for Office
//            //}

//            #region Update Politician OfficeKey to indicate as incumbent if date is more recent
//            string PoliticianKey = db.Politicians_Str_LDSLegIDNum(LEGIDYYRow["LEG_ID_NUM"].ToString(), "PoliticianKey");
//            DateTime Politician_Most_Recent_Date = db.Politicians_Date_LDSLegIDNum(LEGIDYYRow["LEG_ID_NUM"].ToString(), "DataLastUpdated");
//            //if (Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()) > Politician_Most_Recent_Date)
//            //{
//            db.Politicians_Update_Str(PoliticianKey, "OfficeKey", OfficeRow["OfficeKey"].ToString());
//            db.Politicians_Update_Date(PoliticianKey, "DataLastUpdated", Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()));
//            //}
//            #endregion Update Politician OfficeKey to indicate as incumbent if date is more recent

//            OfficesUpdated++;
//          #endregion One Office
//          }
//        }
//        #endregion Normal case of one office on ballots for this/these LEGIDYY elected politicians

//        #region ID and WA State House Exception cases where 2 different office positions for the same office on ballots
//        string LAST_NAME_Previous = string.Empty;
//        string FIRST_NAME_Previous = string.Empty;

//        DataTable OfficesTableSpecial = db.Table(sqlOffices4ID_WA());
//        OfficeRows += OfficesTableSpecial.Rows.Count;
//        foreach (DataRow OfficeRow in OfficesTableSpecial.Rows)
//        {
//          string OfficeKeyTest = OfficeRow["OfficeKey"].ToString();
//          #region One Office
//          //if (Is_More_Recent_OfficesOfficials_Data(OfficeRow["OfficeKey"].ToString(), ref OfficesUnchanged))
//          //{
//          #region Delete ALL OfficesOfficials rows for office
//          Deleted += db.Rows(sqlOfficesOfficials(OfficeRow["OfficeKey"].ToString()));
//          db.ExecuteSQL(
//            //string test1 = //here
//              sql_OfficesOfficials_Delete(OfficeRow["OfficeKey"].ToString())
//            )
//            ;
//          #endregion Delete ALL OfficesOfficials rows for office

//          #region 2 LEGIDYY Rows for the single office
//          DataTable LEGIDYYTable = db.Table(sqlLEGIDYY(
//            db.LDSState4StateCode(OfficeRow["StateCode"].ToString())
//            , db.LDSTYPE4OfficeLevel(Convert.ToInt16(OfficeRow["OfficeLevel"].ToString()))
//            , db.LDSDISTRICT4DistrictCode(OfficeRow["DistrictCode"].ToString()))
//            );
//          #endregion 2 LEGIDYY Rows for the single office

//          if (LEGIDYYTable.Rows.Count == 2)
//          {
//            #region Insert either 1st or 2nd LEGIDYY row depending whether the politician is new
//            if (
//              (LAST_NAME_Previous != LEGIDYYTable.Rows[0]["LAST_NAME"].ToString())
//              && (FIRST_NAME_Previous != LEGIDYYTable.Rows[0]["FIRST_NAME"].ToString())
//              )
//            {
//              #region update OfficesOfficials using 1st LEGIDYY row
//              LEGIDYYPoliticianRowsProcessed++;
//              Inserted += OfficesOfficials_Insert(
//                OfficeRow["OfficeKey"].ToString()
//                , LEGIDYYTable.Rows[0]["LEG_ID_NUM"].ToString()
//                , ref BadPoliticianKeys
//                , ref Vacant
//                );
//              LAST_NAME_Previous = LEGIDYYTable.Rows[0]["LAST_NAME"].ToString();
//              FIRST_NAME_Previous = LEGIDYYTable.Rows[0]["FIRST_NAME"].ToString();
//              #endregion update OfficesOfficials using 1st LEGIDYY row
//            }
//            else
//            {
//              #region update OfficesOfficials using 2nd LEGIDYY row
//              LEGIDYYPoliticianRowsProcessed++;
//              Inserted += OfficesOfficials_Insert(
//                OfficeRow["OfficeKey"].ToString()
//                , LEGIDYYTable.Rows[1]["LEG_ID_NUM"].ToString()
//                , ref BadPoliticianKeys
//                , ref Vacant
//                );
//              LAST_NAME_Previous = LEGIDYYTable.Rows[1]["LAST_NAME"].ToString();
//              FIRST_NAME_Previous = LEGIDYYTable.Rows[1]["FIRST_NAME"].ToString();
//              #endregion update OfficesOfficials using 2nd LEGIDYY row
//            }

//            OfficesUpdated++;
//            #endregion Insert either 1st or 2nd LEGIDYY row depending whether the politician is new
//          }
//          else
//          {
//            Report.Text += "<br><br><span class=TSmallColor>There are not 2 State House Offices row for State: "
//              + OfficeRow["StateCode"].ToString()
//              + " District:" + OfficeRow["DistrictCode"].ToString();
//          }

//          //}
//          #endregion One Office
//        }
//        #endregion ID and WA State House Exception cases where 2 different office positions for the same office on ballots

//        #region DC Exception where only US Congress Delegate, Council Members and ANC Commissioners
//        OfficesTable = db.Table(sqlOffices4DC());
//        OfficeRows += OfficesTable.Rows.Count;
//        foreach (DataRow OfficeRow in OfficesTable.Rows)
//        {
//          string OfficeKeyTest = OfficeRow["OfficeKey"].ToString();
//          #region One Office
//          //if (Is_More_Recent_OfficesOfficials_Data(OfficeRow["OfficeKey"].ToString(), ref OfficesUnchanged))
//          //{
//          #region Delete ALL OfficesOfficials rows for office
//          Deleted += db.Rows(sqlOfficesOfficials(OfficeRow["OfficeKey"].ToString()));
//          db.ExecuteSQL(
//            //string test1 = //here
//              sql_OfficesOfficials_Delete(OfficeRow["OfficeKey"].ToString())
//            )
//            ;
//          #endregion Delete ALL OfficesOfficials rows for office

//          #region Insert 1 or more OfficesOfficials rows for Office
//          DataTable LEGIDYYTable = db.Table(sqlLEGIDYY(
//            db.LDSState4StateCode(OfficeRow["StateCode"].ToString())
//            , db.LDSTYPE4OfficeLevel(
//                OfficeRow["StateCode"].ToString()
//               , Convert.ToInt16(OfficeRow["OfficeLevel"].ToString()))
//            , db.LDSDISTRICT4DistrictCode(OfficeRow["DistrictCode"].ToString()))
//            );
//          foreach (DataRow LEGIDYYRow in LEGIDYYTable.Rows)
//          {
//            LEGIDYYPoliticianRowsProcessed++;
//            Inserted += OfficesOfficials_Insert(
//              OfficeRow["OfficeKey"].ToString()
//              , LEGIDYYRow["LEG_ID_NUM"].ToString()
//              , ref BadPoliticianKeys
//              , ref Vacant
//              );
//          }
//          #endregion Insert 1 or more OfficesOfficials rows for Office

//          OfficesUpdated++;
//          //}
//          #endregion One Office
//        }
//        #endregion DC Exception where only US Congress Delegate, Council Members and ANC Commissioners

//        #endregion Process LEGIDYY updating OfficesOfficials rows

//        #region Run Time
//        DateTime RunTimeStatesEnd = DateTime.Now;
//        TimeSpan RunTimeStatesTimeSpan = RunTimeStatesEnd.Subtract(RunTimeStatesStart);
//        int Hours = RunTimeStatesTimeSpan.Hours;
//        int Minutes = RunTimeStatesTimeSpan.Minutes;
//        int Seconds = RunTimeStatesTimeSpan.Seconds;
//        OfficesOfficialsRunTime.Text = Hours.ToString() + ":" + Minutes.ToString() + ":" + Seconds.ToString();
//        db.Master_Update_Str("LDSOfficesOfficialsRunTime", OfficesOfficialsRunTime.Text);
//        db.Master_Update_Date("LDSDateCompletedOfficesOfficials", DateTime.Now);
//        #endregion

//        LoadMasterControls();

//        #region Msg
//        Msg.Text = db.Ok(
//          "<br>---Offices Table---------------"
//          + "<br>" + OfficesUpdated.ToString() + ": OfficesOfficials rows updated"
//          + "<br>" + OfficesUnchanged.ToString() + ": Offices rows unchaged for OfficesOfficials because Office was more recently updated"
//          + "<br>" + OfficeRows.ToString() + ": Offices rows (of the OfficesOfficials rows) updated using LEGIDYY rows"
//          + "<br>---LEGIDYY rows for OfficesOfficials Table---------------"
//          + "<br>" + Vacant.ToString() + ": Vacant LEGIDYY rows - No OfficesOfficials rows inserted as new office official"
//          + "<br>" + BadPoliticianKeys.ToString() + ": BadPoliticianKeys LEGIDYY rows also not inserted for OfficesOfficials"
//          + "<br>" + Inserted.ToString() + ": OfficesOfficials Updated using LEGIDYY rows to insert as new OfficesOfficials rows"
//          + "<br>" + LEGIDYYPoliticianRowsProcessed.ToString() + ": Total politician rows PROCESSED in the LEGIDYY Table"
//          + "<br>" + LEGIDYYPoliticianRows.ToString() + ": Total politician rows in the LEGIDYY Table"
//          + "<br>" + (LEGIDYYPoliticianRows - LEGIDYYPoliticianRowsProcessed).ToString() + ": LEGIDYY Table row unprocessed and unaccounted for"
//          + "<br>---OfficesOfficials rows deleted---------------"
//          + "<br>" + Deleted.ToString() + ": Deleted OfficesOfficials rows - May have been replaced with inserted rows so may not be the same as inserted rows"
//          + "<br>------------------"
//        );
//        #endregion Msg
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }

//    }
//    //9) Update DC Chairman of Council
//    protected void ButtonUpdateDCCouncilChairman_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckVersionAndUpdateDate();
//        CheckTextBoxs4HtmlAndIlleagalInput();

//        db.ExecuteSQL(
//          //string test1 = //here
//          sql_OfficesOfficials_Delete(
//          "DCAtLargeMemberOfTheCouncil"
//          , TextBoxDCCouncilChairman.Text.Trim())
//          )
//        ;

//        db.ExecuteSQL(
//          //string test1 = //here
//          sql_OfficesOfficials_Delete("DCChairmanOfTheCouncil")
//          )
//        ;

//        db.ExecuteSQL(sql_OfficesOfficials_Delete_Politician(
//          TextBoxDCCouncilChairman.Text.Trim()));

//        db.ExecuteSQL(
//          //string test2 = //here
//        sql_OfficesOfficials_Insert_ChairmanOfCouncil(
//          TextBoxVersion.Text.Trim()
//          , Convert.ToDateTime(TextBoxUpdateDate.Text.Trim())
//          )
//        );

//        Msg.Text = db.Ok("Done");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//    }
//    //10) Update All Elected Officials Reports
//    protected void ButtonUpdateReports_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckTextBoxs4HtmlAndIlleagalInput();
//        Server.ScriptTimeout = 6000;//6000 sec = 100 min = 1.66 hr
//        DateTime RunTimeStatesStart = DateTime.Now;
//        Report.Text = string.Empty;
//        Msg.Text = string.Empty;//to clear any old report and in case button is clicked a second time

//        if (!db.Is_Report_Current_Officials("U1"))
//          db.Report_Officials_Update(PageCache, "U1"); //US Senate
//        if (!db.Is_Report_Current_Officials("U2"))
//          db.Report_Officials_Update(PageCache, "U2"); //US Senate
//        if (!db.Is_Report_Current_Officials("U3"))
//          db.Report_Officials_Update(PageCache, "U3"); //US House
//        if (!db.Is_Report_Current_Officials("U4"))
//          db.Report_Officials_Update(PageCache, "U4"); //State Governors

//        DataTable StatesTable = db.Table(sql.States_51());
//        foreach (DataRow StateRow in StatesTable.Rows)
//        {
//          if (!db.Is_Report_Current_Officials(StateRow["StateCode"].ToString()))
//            db.Report_Officials_Update(PageCache, StateRow["StateCode"].ToString());
//        }

//        #region Run Time
//        DateTime RunTimeStatesEnd = DateTime.Now;
//        TimeSpan RunTimeStatesTimeSpan = RunTimeStatesEnd.Subtract(RunTimeStatesStart);
//        int Hours = RunTimeStatesTimeSpan.Hours;
//        int Minutes = RunTimeStatesTimeSpan.Minutes;
//        int Seconds = RunTimeStatesTimeSpan.Seconds;
//        ReportsRunTime.Text = Hours.ToString() + ":" + Minutes.ToString() + ":" + Seconds.ToString();
//        db.Master_Update_Str("LDSReportsRunTime", ReportsRunTime.Text);
//        db.Master_Update_Date("LDSDateCompletedReports", DateTime.Now);
//        #endregion

//        LoadMasterControls();

//        db.Master_Update_Str("LDSVersionCompleted", db.Master_Str("LDSVersion"));
//        db.Master_Update_Date("LDSDateCompleted", DateTime.Now);

//        Msg.Text = db.Msg("Update of All the Elected Officials Reports completed."
//          + " And LDS data update is complete.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }

//    }

//    protected void Page_Load(object sender, EventArgs e)
//    {
//      if (!SecurePage.IsMasterUser)
//        SecurePage.HandleSecurityException();

//      if (!IsPostBack)
//      {
//        try
//        {
//          LoadMasterControls();
//        }
//        catch (Exception ex)
//        {
//          #region
//          Msg.Text = db.Fail(ex.Message);
//          db.Log_Error_Admin(ex);
//          #endregion
//        }
//      }
//    }

//    #region Dead code


//    //private string xsqlOffices()
//    //{
//    //  string SQL = string.Empty;
//    //  SQL += " SELECT";
//    //  SQL += " Offices.OfficeKey";
//    //  SQL += ",Offices.StateCode ";
//    //  SQL += ",Offices.OfficeLevel";
//    //  SQL += ",Offices.DistrictCode";
//    //  SQL += ",Offices.DistrictCodeAlpha";
//    //  SQL += " FROM Offices";
//    //  SQL += " WHERE (StateCode != 'DC')";
//    //  SQL += " AND (";
//    //  SQL += " ((OfficeLevel = 2)";
//    //  SQL += " OR (OfficeLevel = 3)";
//    //  SQL += " OR (OfficeLevel = 5)";
//    //  SQL += " OR (OfficeLevel = 6)";
//    //  SQL += " OR ((OfficeLevel = 4) AND (SUBSTRING(OfficeKey,3,8) ='Governor')))";//only Governors for non-DC States
//    //  SQL += " AND DistrictCodeAlpha = ''";//ID and WA have a DistrictCodeAlpha
//    //  SQL += " )";
//    //  SQL += " ORDER BY StateCode,OfficeLevel,DistrictCode,DistrictCodeAlpha";
//    //  return SQL;
//    //}

//    //private string sqlLEGIDYY4_LDSState_LDSType(string LDSStateCode, string LDSTypeCode)
//    //{
//    //  string SQL = string.Empty;
//    //  SQL += " SELECT ";
//    //  SQL += " * ";
//    //  SQL += " FROM LEGIDYY ";
//    //  SQL += " WHERE STATE = " + db.SQLLit(LDSStateCode);
//    //  SQL += " AND TYPE = " + db.SQLLit(LDSTypeCode);
//    //  SQL += " AND STATE <= '56'";//Wyoming and before - exclude PR, VI...
//    //  SQL += " ORDER BY STATE,TYPE,DISTRICT";
//    //  return SQL;
//    //}
//    //private string sqlOffices4LDSStateLDSType(string LDSStateCode, string LDSTypeCode)
//    //{
//    //  string SQL = string.Empty;
//    //  SQL += " SELECT ";
//    //  SQL += " Offices.StateCode";
//    //  SQL += " ,Offices.OfficeKey";
//    //  SQL += " ,Offices.OfficeLine1";
//    //  SQL += " ,Offices.OfficeLine2";
//    //  SQL += " ,Offices.OfficeLevel";
//    //  SQL += " ,Offices.LDSStateCode";
//    //  SQL += " ,Offices.LDSTypeCode";
//    //  SQL += " ,Offices.LDSDistrictCode";
//    //  SQL += " ,Offices.LDSOffice";
//    //  SQL += " FROM Offices";
//    //  SQL += " WHERE Offices.LDSStateCode = " + db.SQLLit(LDSStateCode);
//    //  SQL += " AND Offices.LDSTypeCode = " + db.SQLLit(LDSTypeCode);
//    //  return SQL;
//    //}
//    //private string sqlLEGIDYY4LDSStateLDSType(string LDSStateCode, string LDSTypeCode)
//    //{
//    //  string SQL = string.Empty;
//    //  SQL += " SELECT ";
//    //  SQL += " * ";
//    //  SQL += " FROM LEGIDYY ";
//    //  SQL += " WHERE STATE = " + db.SQLLit(LDSStateCode);
//    //  SQL += " AND TYPE = " + db.SQLLit(LDSTypeCode);
//    //  SQL += " AND LAST_NAME != 'VACANT'";
//    //  SQL += " ORDER BY STATE,TYPE,DISTRICT";
//    //  return SQL;
//    //}

//    //protected void ReportBadOfficeKey(string LEG_ID_NUM, string OfficeKey)
//    //{
//    //  Report.Text += "<br><br><span class=TSmallColor>There is no Offices row for OfficeKey: " + OfficeKey;
//    //  ReportLEGIDYYRow(LEG_ID_NUM, "OfficeKey");
//    //}

//    //protected void AddLDSOffice(DataRow LDCODERow)
//    //{
//    //  DataRow OfficeRow = null;
//    //  int Incumbents = 0;
//    //  //Need to add the office row
//    //  //Make sure there is no office with the same OfficeKey that is about to be created 
//    //  string OfficeKey = db.OfficeKey4LDS(LDCODERow);
//    //  OfficeRow = db.Row_Optional(db.Sql_Row_Office(OfficeKey));
//    //  if (OfficeRow == null)
//    //  {
//    //    #region Ok to add the Office row
//    //    Incumbents = db.Rows(sql.LEGIDYY4STATETYPEDISTRICTAll(LDCODERow["STATE"].ToString()
//    //      , LDCODERow["TYPE"].ToString()
//    //      , LDCODERow["DISTRICT"].ToString()));
//    //    AddLDSOffice2OfficesTable(LDCODERow, OfficeKey, Incumbents);
//    //    #endregion
//    //  }
//    //  else
//    //  {
//    //    #region 3 tries to create a unique OfficeKey to add the Offices row
//    //    bool IsUniqueOfficeKey = false;
//    //    string NewDistrictCode = string.Empty;
//    //    NewDistrictCode = LDCODERow["DISTRICT"].ToString() + "A";
//    //    OfficeKey = db.OfficeKey4LDS(LDCODERow["STATE"].ToString(), LDCODERow["TYPE"].ToString(), NewDistrictCode);
//    //    OfficeRow = db.Row_Optional(db.Sql_Row_Office(OfficeKey));
//    //    if (OfficeRow == null)
//    //    {
//    //      #region
//    //      IsUniqueOfficeKey = true;
//    //      Incumbents = db.Rows(sql.LEGIDYY4STATETYPEDISTRICTAll(LDCODERow["STATE"].ToString()
//    //        , LDCODERow["TYPE"].ToString()
//    //        , LDCODERow["DISTRICT"].ToString()));
//    //      AddLDSOffice2OfficesTable(LDCODERow, OfficeKey, Incumbents);
//    //      #endregion
//    //    }
//    //    if (!IsUniqueOfficeKey)
//    //    {
//    //      #region
//    //      NewDistrictCode = LDCODERow["DISTRICT"].ToString() + "B";
//    //      OfficeKey = db.OfficeKey4LDS(LDCODERow["STATE"].ToString(), LDCODERow["TYPE"].ToString(), NewDistrictCode);
//    //      OfficeRow = db.Row_Optional(db.Sql_Row_Office(OfficeKey));
//    //      if (OfficeRow == null)
//    //      {
//    //        IsUniqueOfficeKey = true;
//    //        Incumbents = db.Rows(sql.LEGIDYY4STATETYPEDISTRICTAll(LDCODERow["STATE"].ToString()
//    //          , LDCODERow["TYPE"].ToString()
//    //          , LDCODERow["DISTRICT"].ToString()));
//    //        AddLDSOffice2OfficesTable(LDCODERow, OfficeKey, Incumbents);
//    //      }
//    //      #endregion
//    //    }
//    //    if (!IsUniqueOfficeKey)
//    //    {
//    //      #region
//    //      NewDistrictCode = LDCODERow["DISTRICT"].ToString() + "C";
//    //      OfficeKey = db.OfficeKey4LDS(LDCODERow["STATE"].ToString(), LDCODERow["TYPE"].ToString(), NewDistrictCode);
//    //      OfficeRow = db.Row_Optional(db.Sql_Row_Office(OfficeKey));
//    //      if (OfficeRow == null)
//    //      {
//    //        IsUniqueOfficeKey = true;
//    //        Incumbents = db.Rows(sql.LEGIDYY4STATETYPEDISTRICTAll(LDCODERow["STATE"].ToString()
//    //          , LDCODERow["TYPE"].ToString()
//    //          , LDCODERow["DISTRICT"].ToString()));
//    //        AddLDSOffice2OfficesTable(LDCODERow, OfficeKey, Incumbents);
//    //      }
//    //      #endregion
//    //    }
//    //    if (!IsUniqueOfficeKey)
//    //    {
//    //      //Report the office that will need to be added using the Offices Form
//    //      ReportLDSOffice2AddManually(LDCODERow);
//    //    }
//    //    #endregion
//    //  }
//    //}

//    //protected void UpdateLDSOfficeInOfficesTable(DataRow LDCODERow, string StateCode)
//    //{
//    //  try
//    //  {
//    //    string UpdateSQL = "UPDATE Offices SET "
//    //      + " LDSVersion = " + db.SQLLit(TextBoxVersion.Text.Trim())
//    //      + " WHERE OfficeKey = " + db.SQLLit(db.OfficeKey4LDS(LDCODERow));
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    #region
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //    #endregion
//    //  }
//    //}

//    //protected bool Is_More_Recent_OfficesOfficials_Data(string OfficeKey, ref int Unchanged)
//    //{
//    //  int OfficesOfficialsRows = 0;
//    //  DateTime OfficesOfficials_Data_Most_Recent_Date = DateTime.MinValue;
//    //  DataTable OfficesOfficialsTable = db.Table(sqlOfficesOfficials(OfficeKey));
//    //  foreach (DataRow OfficesOfficialsRow in OfficesOfficialsTable.Rows)
//    //  {
//    //    OfficesOfficialsRows++;
//    //    //DateTime TestD = Convert.ToDateTime(OfficesOfficialsRow["DataLastUpdated"].ToString());
//    //    //string Test = OfficesOfficialsRow["PoliticianKey"].ToString();
//    //    if (Convert.ToDateTime(OfficesOfficialsRow["DataLastUpdated"].ToString()) > OfficesOfficials_Data_Most_Recent_Date)
//    //      OfficesOfficials_Data_Most_Recent_Date = Convert.ToDateTime(OfficesOfficialsRow["DataLastUpdated"].ToString());
//    //  }
//    //  if (Convert.ToDateTime(TextBoxUpdateDate.Text.Trim()) > OfficesOfficials_Data_Most_Recent_Date)
//    //  {
//    //    return true;
//    //  }
//    //  else
//    //  {
//    //    Unchanged += OfficesOfficialsRows;//Some offices have more than one row (office positions)
//    //    return false;
//    //  }
//    //}
//    //protected string PoliticianKey4LEGIDYY(string LEG_ID_NUM)
//    //{
//    //  string PoliticianKey = db.Politicians_Str_LDSLegIDNum(LEG_ID_NUM, "PoliticianKey");
//    //  if (PoliticianKey == string.Empty)
//    //  {
//    //    PoliticianKey = db.Politician_Key(
//    //      db.StateCode4LDSStateCode(db.LEGIDYY(LEG_ID_NUM, "STATE"))
//    //    , db.LEGIDYY(LEG_ID_NUM, "LAST_NAME")
//    //    , db.LEGIDYY(LEG_ID_NUM, "FIRST_NAME")
//    //    , db.LEGIDYY(LEG_ID_NUM, "MID_NAME")
//    //    , db.LEGIDYY(LEG_ID_NUM, "SUFFIX")
//    //    );
//    //  }
//    //  return PoliticianKey;
//    //}
//    //protected void AddLDSOffice2OfficesTable(DataRow LDCODERow, string OfficeKey, int Incumbents)
//    //{
//    //  try
//    //  {
//    //    int Office_Class = OfficeLevelFromLDSRow(LDCODERow);
//    //    //string OfficeKey = db.OfficeKey4LDS(LDCODERow);
//    //    string StateCode = db.StateCode4LDSStateCode(LDCODERow["STATE"].ToString());
//    //    // Add Offices deleted

//    //    #region Report ADD
//    //    Report.Text += "<br><br>ADDED Office Using LDCODE Table row: ";
//    //    Report.Text += "<br>----OfficeKey: " + OfficeKey;
//    //    Report.Text += "<br>----StateCode: " + StateCode;
//    //    Report.Text += "<br>----OfficeLevel: " + Office_Class.ToString();
//    //    Report.Text += "<br>----DistrictCode: " + LDCODERow["DISTRICT"].ToString().Trim();
//    //    Report.Text += "<br>----DistrictCodeAlpha: " + db.DistrictCode_3_Digits(LDCODERow["DISTRICT"].ToString().Trim());
//    //    Report.Text += "<br>----OfficeLine1: " + LDCODERow["NAME"].ToString().Trim();
//    //    Report.Text += "<br>----OfficeLine2: ";
//    //    Report.Text += "<br>----VoteForWording: Vote for not more than one";
//    //    Report.Text += "<br>----LDSStateCode: " + LDCODERow["STATE"].ToString().Trim();
//    //    Report.Text += "<br>----LDSTypeCode: " + LDCODERow["TYPE"].ToString();
//    //    Report.Text += "<br>----LDSDistrictCode: " + LDCODERow["DISTRICT"].ToString();
//    //    Report.Text += "<br>----LDSOffice: " + LDCODERow["NAME"].ToString().Trim();
//    //    Report.Text += "<br>----LDSVersion: " + TextBoxVersion.Text.Trim();
//    //    Report.Text += "<br><span class=TSmallColor>OfficeLine1 and OfficeLine2 may need to be changed to be consistant with ballot office titles of similar offices.</span>";
//    //    #endregion
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    #region
//    //    Msg.Text = db.Fail(ex.Message);
//    //    db.Log_Error_Admin(ex);
//    //    #endregion
//    //  }
//    //}

//    //protected void ReportLDSOffice2AddManually(DataRow LDCODERow)
//    //{
//    //  int Office_Class = OfficeLevelFromLDSRow(LDCODERow);
//    //  string OfficeKey = db.OfficeKey4LDS(LDCODERow);
//    //  string StateCode = db.StateCode4LDSStateCode(LDCODERow["STATE"].ToString());

//    //  Report.Text += "<br><br><span class=TSmallColor>AN OFFICE NEEDS TO BE ADD in the Offices Table for this LDCODE Table row: </span>";
//    //  Report.Text += "<br>----DISTRICT: " + LDCODERow["DISTRICT"].ToString().Trim();
//    //  Report.Text += "<br>----STATE: " + LDCODERow["STATE"].ToString().Trim();
//    //  Report.Text += "<br>----TYPE: " + LDCODERow["TYPE"].ToString();
//    //  Report.Text += "<br>The office row that could not be added was:";
//    //  Report.Text += "<span class=TSmallColor><br>----OfficeKey: " + OfficeKey + " (a new unique key needs to be constructed)</span>";
//    //  Report.Text += "<br>----StateCode: " + StateCode;
//    //  Report.Text += "<br>----OfficeLevel: " + Office_Class.ToString();
//    //  Report.Text += "<br>----DistrictCode: " + LDCODERow["DISTRICT"].ToString().Trim();
//    //  Report.Text += "<br>----DistrictCodeAlpha: " + db.DistrictCode_3_Digits(LDCODERow["DISTRICT"].ToString().Trim());
//    //  Report.Text += "<br>----OfficeLine1: " + LDCODERow["NAME"].ToString().Trim();
//    //  Report.Text += "<br>----OfficeLine2: ";
//    //  Report.Text += "<br>----VoteForWording: Vote for not more than one";
//    //  Report.Text += "<br>----LDSStateCode: " + LDCODERow["STATE"].ToString().Trim();
//    //  Report.Text += "<br>----LDSTypeCode: " + LDCODERow["TYPE"].ToString();
//    //  Report.Text += "<br>----LDSDistrictCode: " + LDCODERow["DISTRICT"].ToString();
//    //  Report.Text += "<br>----LDSOffice: " + LDCODERow["NAME"].ToString().Trim();
//    //  Report.Text += "<br>----LDSVersion: " + TextBoxVersion.Text.Trim();
//    //}
//    //private int OfficeLevelFromLDSRow(DataRow LDSRow) //
//    //{
//    //  int Office_Class = 0;
//    //  if (LDSRow["STATE"].ToString().Trim() == "00")//National Offices
//    //    Office_Class = 1;//US President
//    //  else //State wide offices
//    //    switch (LDSRow["TYPE"].ToString().Trim())
//    //    {
//    //      case "0":
//    //        Office_Class = 2; //US Senate
//    //        break;
//    //      case "1":
//    //        Office_Class = 3; //US congress
//    //        break;
//    //      case "2":
//    //        Office_Class = 5; //State Senate
//    //        break;
//    //      case "3":
//    //        Office_Class = 6; //State House
//    //        break;
//    //      case "7":
//    //        Office_Class = 4; //Governor (state wide)
//    //        break;
//    //      default:
//    //        throw new ApplicationException("The LDS Type was not 0,1,2,3,7");
//    //    }
//    //  return Office_Class;
//    //}

//    #endregion Dead code





  }
}
