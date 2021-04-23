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
using DB;
using DB.Vote;

namespace Vote.Master
{
  public partial class MgtReports : VotePage
  {
    //private void ElectionNames(string ElectionKey)
    //{
    //  //LabelReport.Text = string.Empty;
    //  LabelReport.Text += "Candidate Names in This election for Google";
    //  LabelReport.Text += "<br><br>";

    //  #region sql
    //  string SQL = string.Empty;
    //  SQL += "SELECT ";
    //  SQL += "ElectionsPoliticians.OfficeKey";
    //  SQL += ",ElectionsPoliticians.PoliticianKey";
    //  SQL += ",Politicians.FName";
    //  SQL += ",Politicians.LName";
    //  SQL += ",Politicians.Password";
    //  SQL += ",Politicians.StateCode";
    //  SQL += ",Politicians.EmailAddrVoteUSA";
    //  SQL += ",Politicians.EmailAddr";
    //  SQL += ",Politicians.StateEmailAddr";
    //  SQL += ",Politicians.LDSEmailAddr";
    //  SQL += ",Politicians.CampaignEmail";
    //  SQL += ",ElectionsOffices.OfficeKey";
    //  SQL += ",Offices.OfficeLine1";
    //  SQL += ",Offices.OfficeLine2";
    //  SQL += ",Elections.ElectionKey";
    //  SQL += ",Elections.ElectionDesc";
    //  SQL += " FROM Elections,ElectionsOffices,ElectionsPoliticians,Politicians,Offices";
    //  SQL += " WHERE ElectionsOffices.ElectionKey = " + db.SQLLit(ElectionKey);
    //  SQL += " AND ElectionsPoliticians.ElectionKey = Elections.ElectionKey";
    //  SQL += " AND ElectionsPoliticians.OfficeKey = ElectionsOffices.OfficeKey";
    //  SQL += " AND Offices.OfficeKey = ElectionsOffices.OfficeKey";
    //  SQL += " AND Politicians.PoliticianKey = ElectionsPoliticians.PoliticianKey";
    //  SQL += " ORDER BY ElectionsOffices.OfficeLevel";
    //  SQL += " ,ElectionsOffices.OfficeKey,ElectionsPoliticians.PoliticianKey";
    //  #endregion sql
    //  //DataTable Table_Offices = db.Table(sql.ElectionsOffices4Names(ElectionKey));
    //  //DataTable PoliticiansTable = db.Table(sql.AllCandidatesInElection4Emails(ElectionKey));
    //  DataTable PoliticiansTable = db.Table(SQL);
    //  foreach (DataRow PoliticianRow in PoliticiansTable.Rows)
    //  {
    //    //if more than 2 parts to the name add a Name with just 2 parts, (always last name, but either firs, middle or nickname)
    //    if (db.Str_Remove_Puctuation(Politicians.GetFormattedName(PoliticianRow["PoliticianKey"].ToString()))
    //    != db.PoliticianName2Part4Google(PoliticianRow["PoliticianKey"].ToString()))
    //      LabelReport.Text += "<br>" + db.PoliticianName2Part4Google(PoliticianRow["PoliticianKey"].ToString());

    //    //possible Nickname
    //    if ((db.PoliticianNameNickName4Google(PoliticianRow["PoliticianKey"].ToString()) != string.Empty)
    //      && (db.PoliticianName2Part4Google(PoliticianRow["PoliticianKey"].ToString()) != db.PoliticianNameNickName4Google(PoliticianRow["PoliticianKey"].ToString())))
    //      LabelReport.Text += "<br>" + db.PoliticianNameNickName4Google(PoliticianRow["PoliticianKey"].ToString());
    //    //}
    //  }
    //}
    //private void OfficialsNames(string StateCode)
    //{
    //  DataTable Table_Offices = null;
    //  switch (StateCode)
    //  {
    //    case "U2":
    //      #region US Senate Members
    //      //LabelReport.Text = string.Empty;
    //      LabelReport.Text += "US Senate Members for Google";
    //      LabelReport.Text += "<br><br>";

    //      Table_Offices = db.Table_Offices(OfficeClass.USSenate.ToInt());
    //      foreach (DataRow Row_Office in Table_Offices.Rows)
    //      {
    //        DataTable PoliticiansTable = db.Table(sql.Politicians1Office(Row_Office["OfficeKey"].ToString()));
    //        foreach (DataRow PoliticianRow in PoliticiansTable.Rows)
    //        {
    //          LabelReport.Text += "<br>" + Politicians.GetFormattedName(PoliticianRow["PoliticianKey"].ToString());
    //        }
    //      }
    //      #endregion
    //      break;
    //    case "U3":
    //      #region US House Members
    //      //LabelReport.Text = string.Empty;
    //      LabelReport.Text += "US House Members for Google";
    //      LabelReport.Text += "<br><br>";

    //      Table_Offices = db.Table_Offices(OfficeClass.USHouse.ToInt());
    //      foreach (DataRow Row_Office in Table_Offices.Rows)
    //      {
    //        DataTable PoliticiansTable = db.Table(sql.Politicians1Office(Row_Office["OfficeKey"].ToString()));
    //        foreach (DataRow PoliticianRow in PoliticiansTable.Rows)
    //        {
    //          LabelReport.Text += "<br>" + Politicians.GetFormattedName(PoliticianRow["PoliticianKey"].ToString());
    //        }
    //      }
    //      #endregion
    //      break;
    //    default:
    //      #region State Elected Officials
    //      //LabelReport.Text = string.Empty;
    //      LabelReport.Text += "Elected Officials Names in This State for Google";
    //      LabelReport.Text += "<br><br>";

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
    //      SQL += " FROM Offices ";
    //      SQL += " WHERE Offices.StateCode = " + db.SQLLit(StateCode);
    //      SQL += " ORDER BY Offices.StateCode";
    //      SQL += " ,Offices.DistrictCode";
    //      //SQL += " ,CONVERT(int,Offices.DistrictCode)";
    //      SQL += " ,Offices.DistrictCode";
    //      Table_Offices = db.Table(SQL);
    //      foreach (DataRow Row_Office in Table_Offices.Rows)
    //      {
    //        DataTable PoliticiansTable = db.Table(sql.Politicians1Office(Row_Office["OfficeKey"].ToString()));
    //        foreach (DataRow PoliticianRow in PoliticiansTable.Rows)
    //        {
    //          LabelReport.Text += "<br>" + Politicians.GetFormattedName(PoliticianRow["PoliticianKey"].ToString());
    //        }
    //      }
    //      #endregion
    //      break;
    //  }
    //}
    ////private void PoliticianDataEntriesReport(int Office_Class)
    //private void PoliticianDataEntriesReport(DataTable ElectedTable)
    //{
    //  //DataTable ElectedTable = db.Table(sql.ElectedRepresentatives(Office_Class));
    //  //LabelReport.Text = string.Empty;
    //  foreach (DataRow PoliticianRow in ElectedTable.Rows)
    //  {
    //    LabelReport.Text += "<br><br>";
    //    LabelReport.Text += " " + PoliticianRow["StateCode"].ToString();
    //    LabelReport.Text += " " + PoliticianRow["OfficeLine1"].ToString();
    //    LabelReport.Text += " " + PoliticianRow["OfficeLine2"].ToString();
    //    LabelReport.Text += " " + PoliticianRow["FName"].ToString();
    //    LabelReport.Text += " " + PoliticianRow["MName"].ToString();
    //    LabelReport.Text += " " + PoliticianRow["LName"].ToString();
    //    LabelReport.Text += " " + PoliticianRow["Suffix"].ToString();
    //    LabelReport.Text += " " + PoliticianRow["Nickname"].ToString();
    //    #region Count Intro Entries
    //    //int IntroEntries = db.Rows("LogPoliticianChanges", "PoliticianKey", PoliticianRow["PoliticianKey"].ToString());
    //    int IntroEntries = 0;
    //    if (PoliticianRow["GeneralStatement"].ToString() != string.Empty)
    //      IntroEntries++;
    //    if (PoliticianRow["Personal"].ToString() != string.Empty)
    //      IntroEntries++;
    //    if (PoliticianRow["Education"].ToString() != string.Empty)
    //      IntroEntries++;
    //    if (PoliticianRow["Profession"].ToString() != string.Empty)
    //      IntroEntries++;
    //    if (PoliticianRow["Military"].ToString() != string.Empty)
    //      IntroEntries++;
    //    if (PoliticianRow["Civic"].ToString() != string.Empty)
    //      IntroEntries++;
    //    if (PoliticianRow["Political"].ToString() != string.Empty)
    //      IntroEntries++;
    //    if (PoliticianRow["Religion"].ToString() != string.Empty)
    //      IntroEntries++;
    //    if (PoliticianRow["Accomplishments"].ToString() != string.Empty)
    //      IntroEntries++;
    //    #endregion

    //    LabelReport.Text += "<br> - - Intro Page Entries: " + IntroEntries.ToString() + " of 9. Last Info Entered: " + PoliticianRow["DataLastUpdated"].ToString();
    //    //int AnswerEntries = db.Rows("LogPoliticianAnswers", "PoliticianKey", PoliticianRow["PoliticianKey"].ToString());
    //    //int AnswerEntries = db.Rows("Answers", "PoliticianKey", PoliticianRow["PoliticianKey"].ToString());
    //    DataTable AnswersTable = db.Table(sql.Answers4Politician(PoliticianRow["PoliticianKey"].ToString()));
    //    LabelReport.Text += "<br> - - Answer Entries: " + AnswersTable.Rows.Count.ToString();
    //    if (AnswersTable.Rows.Count > 0)
    //    {
    //      DataRow AnswerRow = AnswersTable.Rows[0];
    //      LabelReport.Text += " Last Answer Entered: " + AnswerRow["DateStamp"].ToString();
    //    }
    //    //remove after answer length are gotten.
    //    //foreach (DataRow AnswerRow in AnswersTable.Rows)
    //    //{
    //    //  LabelReport.Text += "<br>" + AnswerRow["AnswerLength"].ToString();
    //    //  //  + " " + AnswerRow["PoliticianKey"].ToString()
    //    //  //+ " " + AnswerRow["QuestionKey"].ToString();
    //    //}

    //  }
    //}

    //private void Office_Class_Offices(DataTable tableOffices, string stateCode,
    //  OfficeClass officeClass, HtmlTable htmlTableOffices)
    //{
    //  #region Office Class

    //  if (db.Is_Office_Class_Open(stateCode, officeClass))
    //  {
    //    #region Office Class Name

    //    HtmlTableRow OfficeHTMLTr = null;

    //    OfficeHTMLTr = db.Add_Tr_To_Table_Return_Tr(htmlTableOffices);

    //    db.Add_Td_To_Tr(OfficeHTMLTr,
    //      "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" +
    //        DB.Vote.Offices.GetOfficeClassDescription(officeClass, stateCode)
    //      //, "tdReportGroupHeadingLeft"
    //      , "tdReportDetailBold");
    //    #endregion Office Class Name

    //    foreach (DataRow Row_Office in tableOffices.Rows)
    //    {
    //      #region Offices

    //      OfficeHTMLTr = db.Add_Tr_To_Table_Return_Tr(htmlTableOffices);

    //      db.Add_Td_To_Tr(OfficeHTMLTr,
    //        "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" +
    //          Row_Office["OfficeLine1"] + " " + Row_Office["OfficeLine2"] + " (" +
    //          Row_Office["OfficeOrderWithinLevel"] + ")"
    //        //, "tdReportDetailBold"
    //        , "tdReportHeadingLeft");

    //      #endregion Offices
    //    }
    //  }

    //  #endregion Office Class
    //}

    //private DataTable Table_County_Offices(string StateCode, string CountyCode, int Office_Class)
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " Offices.OfficeLine1 ";
    //  SQL += " ,Offices.OfficeLine2 ";
    //  SQL += " ,Offices.OfficeOrderWithinLevel ";
    //  SQL += " FROM Offices ";
    //  SQL += " WHERE Offices.StateCode = " + db.SQLLit(StateCode);
    //  SQL += " AND Offices.CountyCode = " + db.SQLLit(CountyCode);
    //  SQL += " AND Offices.LocalCode = ''";
    //  SQL += " AND Offices.OfficeLevel = " + Office_Class.ToString();
    //  SQL += " ORDER BY Offices.OfficeOrderWithinLevel";
    //  SQL += ",Offices.OfficeLine1";
    //  return db.Table(SQL);
    //}
    //private DataTable Table_Local_Offices(string StateCode, string CountyCode, string LocalCode, int Office_Class)
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " Offices.OfficeLine1 ";
    //  SQL += " ,Offices.OfficeLine2 ";
    //  SQL += " FROM Offices ";
    //  SQL += " WHERE Offices.StateCode = " + db.SQLLit(StateCode);
    //  SQL += " AND Offices.CountyCode = " + db.SQLLit(CountyCode);
    //  SQL += " AND Offices.LocalCode = " + db.SQLLit(LocalCode);
    //  SQL += " AND Offices.OfficeLevel = " + Office_Class.ToString();
    //  SQL += " ORDER BY Offices.OfficeOrderWithinLevel";
    //  SQL += ",Offices.OfficeLine1";
    //  return db.Table(SQL);
    //}

    //private void ReportOffices(string StateCode, string Report)
    //{
    //  HtmlTable HtmlTableOffices = new HtmlTable();
    //  HtmlTableRow OfficeHTMLTr = null;
    //  DataTable TableOffices = null;
    //  string SQL = string.Empty;

    //  #region report attributes
    //  HtmlTableOffices.Attributes["cellspacing"] = "0";
    //  HtmlTableOffices.Attributes["border"] = "0";
    //  HtmlTableOffices.Attributes["class"] = "tableAdmin";
    //  #endregion report attributes

    //  if (
    //    (Report == "All")
    //    || (Report == "State")
    //    )
    //  {
    //    #region State Heading
    //    OfficeHTMLTr = db.Add_Tr_To_Table_Return_Tr(
    //      HtmlTableOffices);
    //    db.Add_Td_To_Tr(
    //      OfficeHTMLTr
    //     , StateCache.GetStateName(StateCode)
    //      //, "tdReportHeadingLeft"
    //     , "tdReportDetailBold"
    //      //, 3
    //     );
    //    #endregion State Heading

    //    #region State Offices

    //    TableOffices = db.Table(db.sql_Offices_ByOrder_ByDistrictCode(
    //      StateCode
    //      , OfficeClass.USSenate.ToInt()
    //      ));
    //    Office_Class_Offices(TableOffices, StateCode, OfficeClass.USSenate, HtmlTableOffices);

    //    TableOffices = db.Table(db.sql_Offices_ByDistrictCode(
    //      StateCode
    //      , OfficeClass.USHouse.ToInt()
    //      ));
    //    Office_Class_Offices(TableOffices, StateCode, OfficeClass.USHouse, HtmlTableOffices);

    //    TableOffices = db.Table(db.sql_Offices_ByOrder(
    //      StateCode
    //      , OfficeClass.StateWide.ToInt()
    //      ));
    //    Office_Class_Offices(TableOffices, StateCode, OfficeClass.StateWide, HtmlTableOffices);

    //    TableOffices = db.Table(db.sql_Offices_ByDistrictCode(
    //      StateCode
    //      , OfficeClass.StateSenate.ToInt()
    //      ));
    //    Office_Class_Offices(TableOffices, StateCode, OfficeClass.StateSenate, HtmlTableOffices);

    //    TableOffices = db.Table(db.sql_Offices_ByDistrictCode(
    //      StateCode
    //      , OfficeClass.StateHouse.ToInt()
    //      ));
    //    Office_Class_Offices(TableOffices, StateCode, OfficeClass.StateHouse, HtmlTableOffices);

    //    TableOffices = db.Table(db.sql_Offices_ByOrder(
    //      StateCode
    //      , OfficeClass.StateDistrictMultiCounties.ToInt()
    //      ));
    //    Office_Class_Offices(TableOffices, StateCode, OfficeClass.StateDistrictMultiCounties, HtmlTableOffices);

    //    TableOffices = db.Table(db.sql_Offices_ByOrder(
    //      StateCode
    //      , OfficeClass.StateJudicial.ToInt()
    //      ));
    //    Office_Class_Offices(TableOffices, StateCode, OfficeClass.StateJudicial, HtmlTableOffices);

    //    TableOffices = db.Table(db.sql_Offices_ByOrder(
    //       StateCode
    //       , OfficeClass.StateDistrictMultiCountiesJudicial.ToInt()
    //       ));
    //    Office_Class_Offices(TableOffices, StateCode, OfficeClass.StateDistrictMultiCountiesJudicial, HtmlTableOffices);

    //    TableOffices = db.Table(db.sql_Offices_ByOrder(
    //       StateCode
    //       , OfficeClass.StateDistrictMultiCountiesParty.ToInt()
    //       ));
    //    Office_Class_Offices(TableOffices, StateCode, OfficeClass.StateDistrictMultiCountiesParty, HtmlTableOffices);

    //    #endregion State Offices
    //  }

    //  if (
    //   (Report == "All")
    //   || (Report == "Counties")
    //   || (Report == "Locals")
    //   )
    //  {
    //    SQL = sql.Counties(StateCode);
    //    DataTable Table_Counties = db.Table(SQL);
    //    foreach (DataRow Row_County in Table_Counties.Rows)
    //    {
    //      if (
    //      (Report == "All")
    //      || (Report == "Counties")
    //      )
    //      {
    //        #region County Heading
    //        OfficeHTMLTr = db.Add_Tr_To_Table_Return_Tr(
    //          HtmlTableOffices);
    //        db.Add_Td_To_Tr(
    //          OfficeHTMLTr
    //         , StateCache.GetStateName(StateCode)
    //              + " - "
    //              + CountyCache.GetCountyName(StateCode, Row_County["CountyCode"].ToString())
    //          //, "tdReportGroupHeadingLeft"
    //         , "tdReportDetailBold"
    //          //, 3
    //         );
    //        #endregion County Heading

    //        #region County Offices
    //        Office_Class_Offices(
    //          Table_County_Offices(StateCode, Row_County["CountyCode"].ToString(), OfficeClass.CountyExecutive.ToInt())
    //          , StateCode
    //          , OfficeClass.CountyExecutive
    //          , HtmlTableOffices
    //          );

    //        Office_Class_Offices(
    //          Table_County_Offices(StateCode, Row_County["CountyCode"].ToString(), OfficeClass.CountyLegislative.ToInt())
    //          , StateCode
    //          , OfficeClass.CountyLegislative
    //          , HtmlTableOffices
    //          );

    //        Office_Class_Offices(
    //          Table_County_Offices(StateCode, Row_County["CountyCode"].ToString(), OfficeClass.CountySchoolBoard.ToInt())
    //          , StateCode
    //          , OfficeClass.CountySchoolBoard
    //          , HtmlTableOffices
    //          );

    //        Office_Class_Offices(
    //          Table_County_Offices(StateCode, Row_County["CountyCode"].ToString(), OfficeClass.CountyCommission.ToInt())
    //          , StateCode
    //          , OfficeClass.CountyCommission
    //          , HtmlTableOffices
    //          );

    //        Office_Class_Offices(
    //          Table_County_Offices(StateCode, Row_County["CountyCode"].ToString(), OfficeClass.CountyJudicial.ToInt())
    //          , StateCode
    //          , OfficeClass.CountyJudicial
    //          , HtmlTableOffices
    //          );

    //        Office_Class_Offices(
    //          Table_County_Offices(StateCode, Row_County["CountyCode"].ToString(), OfficeClass.CountyParty.ToInt())
    //          , StateCode
    //          , OfficeClass.CountyParty
    //          , HtmlTableOffices
    //          );
    //        #endregion County Offices
    //      }

    //      if (
    //      (Report == "All")
    //      || (Report == "Locals")
    //      )
    //      {
    //        SQL = sql.LocalDistricts(StateCode, Row_County["CountyCode"].ToString());
    //        DataTable Table_Locals = db.Table(SQL);
    //        foreach (DataRow rowLocal in Table_Locals.Rows)
    //        {
    //          #region Local Heading
    //          OfficeHTMLTr = db.Add_Tr_To_Table_Return_Tr(
    //            HtmlTableOffices);
    //          db.Add_Td_To_Tr(
    //            OfficeHTMLTr
    //           , StateCache.GetStateName(StateCode)
    //                + " - "
    //                + CountyCache.GetCountyName(StateCode, Row_County["CountyCode"].ToString())
    //                + " - "
    //                + rowLocal.LocalDistrict()
    //            //, "tdReportGroupHeadingLeft"
    //           , "tdReportDetailBold"
    //            //, 3
    //           );
    //          #endregion Local Heading

    //          #region Local Offices

    //          Office_Class_Offices(
    //           Table_Local_Offices(StateCode, Row_County["CountyCode"].ToString(), rowLocal["LocalCode"].ToString(), OfficeClass.LocalExecutive.ToInt())
    //           , StateCode
    //           , OfficeClass.LocalExecutive
    //           , HtmlTableOffices
    //           );

    //          Office_Class_Offices(
    //           Table_Local_Offices(StateCode, Row_County["CountyCode"].ToString(), rowLocal["LocalCode"].ToString(), OfficeClass.LocalLegislative.ToInt())
    //           , StateCode
    //           , OfficeClass.LocalLegislative
    //           , HtmlTableOffices
    //           );

    //          Office_Class_Offices(
    //           Table_Local_Offices(StateCode, Row_County["CountyCode"].ToString(), rowLocal["LocalCode"].ToString(), OfficeClass.LocalSchoolBoard.ToInt())
    //           , StateCode
    //           , OfficeClass.LocalSchoolBoard
    //           , HtmlTableOffices
    //           );

    //          Office_Class_Offices(
    //           Table_Local_Offices(StateCode, Row_County["CountyCode"].ToString(), rowLocal["LocalCode"].ToString(), OfficeClass.LocalCommission.ToInt())
    //           , StateCode
    //           , OfficeClass.LocalCommission
    //           , HtmlTableOffices
    //           );

    //          Office_Class_Offices(
    //           Table_Local_Offices(StateCode, Row_County["CountyCode"].ToString(), rowLocal["LocalCode"].ToString(), OfficeClass.LocalJudicial.ToInt())
    //           , StateCode
    //           , OfficeClass.LocalJudicial
    //           , HtmlTableOffices
    //           );

    //          Office_Class_Offices(
    //           Table_Local_Offices(StateCode, Row_County["CountyCode"].ToString(), rowLocal["LocalCode"].ToString(), OfficeClass.LocalParty.ToInt())
    //           , StateCode
    //           , OfficeClass.LocalParty
    //           , HtmlTableOffices
    //           );

    //          #endregion Local Offices
    //        }
    //      }
    //    }
    //  }



    //  LabelReport.Text = db.RenderToString(HtmlTableOffices);
    //}

    //#region Future Elections Reports
    //private string sql_Select()
    //{
    //  string SQL = string.Empty;
    //  SQL += "SELECT ";
    //  SQL += " StateCode";
    //  SQL += ",CountyCode";
    //  SQL += ",LocalCode";
    //  SQL += ",ElectionDate";
    //  SQL += ",ElectionDesc";
    //  SQL += ",EarlyVotingDate";
    //  SQL += ",RegistrationDate";
    //  SQL += ",ElectionType";
    //  SQL += ",PartyCode";
    //  SQL += " FROM ElectionsFuture";
    //  SQL += " WHERE ElectionDate > "
    //    + db.SQLLit(Db.DbToday);
    //  return SQL;
    //}
    //private DataTable Table_ElectionsFuture()
    //{
    //  string SQL = string.Empty;
    //  //SQL += "SELECT ";
    //  //SQL += " StateCode";
    //  //SQL += ",CountyCode";
    //  //SQL += ",LocalCode";
    //  //SQL += ",ElectionDate";
    //  //SQL += ",ElectionDesc";
    //  //SQL += ",EarlyVotingDate";
    //  //SQL += ",RegistrationDate";
    //  //SQL += " FROM ElectionsFuture";
    //  //SQL += " WHERE ElectionDate > "
    //  //  + db.SQLLit(Db.DbToday);

    //  SQL += sql_Select();
    //  switch (QueryReport)
    //  {
    //    case "FutureElectionsAll":
    //      break;
    //    case "FuturePresidentialPrimaries":
    //      SQL += " AND ElectionType = 'B'";
    //      break;
    //    case "FutureStatePrimaries":
    //      SQL += " AND ElectionType = 'P'";
    //      break;
    //    case "FutureGeneralElections":
    //      SQL += " AND ElectionType = 'G'";
    //      break;
    //    case "FutureSpecialElections":
    //      SQL += " AND ElectionType = 'S'";
    //      break;
    //    default:
    //      break;
    //  }
    //  SQL += " ORDER BY ElectionDate, StateCode,CountyCode,LocalCode";
    //  return db.Table(SQL);
    //}
    //private string ElectionsHeading()
    //{
    //  string heading = "<br>";
    //  heading += "Election Date - Description - Early Voting Date - Registration Date";
    //  heading += "<br>";
    //  return heading;
    //}
    //private string ElectionDetail(DataRow ElectionsRow)
    //{
    //  DateTime NoDate = Convert.ToDateTime("01/01/1900");
    //  string detail = string.Empty;

    //  detail += "<br>";
    //  detail += " " + Convert.ToDateTime(ElectionsRow["ElectionDate"]).ToString("MM/dd/yyyy");
    //  detail += ": " + StateCache.GetStateName(ElectionsRow["StateCode"].ToString()).ToUpper()
    //    + ":: " + ElectionsRow["ElectionDesc"].ToString();
    //  if (Convert.ToDateTime(ElectionsRow["EarlyVotingDate"]) != NoDate)
    //    detail += " - " + Convert.ToDateTime(ElectionsRow["EarlyVotingDate"]).ToString("MM/dd/yyyy");
    //  else
    //    detail += " - n/a ";
    //  if (Convert.ToDateTime(ElectionsRow["RegistrationDate"]) != NoDate)
    //    detail += " - " + Convert.ToDateTime(ElectionsRow["RegistrationDate"]).ToString("MM/dd/yyyy");
    //  else
    //    detail += " - n/a ";

    //  return detail;
    //}
    //private void FutureElectionsAll()
    //{
    //  //DateTime NoDate = Convert.ToDateTime("01/01/1900");
    //  DateTime LastDate = Convert.ToDateTime("01/01/1900");
    //  LabelReport.Text = "<strong>All Future State Elections in ElectionsFuture Table</strong>";

    //  //LabelReport.Text += "<br><br>";
    //  //LabelReport.Text += "Election Date - Description - Early Voting Date - Registration Date";
    //  //LabelReport.Text += "<br>";
    //  //LabelReport.Text += "_____________   ___________   _________________   _________________";
    //  //LabelReport.Text += "<br><br>";
    //  LabelReport.Text += ElectionsHeading();

    //  DataTable ElectionsFutureTable = Table_ElectionsFuture();

    //  foreach (DataRow ElectionsRow in ElectionsFutureTable.Rows)
    //  {
    //    if (LastDate != Convert.ToDateTime(ElectionsRow["ElectionDate"]))
    //      LabelReport.Text += "<br>___________________________________________________________________<br>";
    //    LastDate = Convert.ToDateTime(ElectionsRow["ElectionDate"]);
    //    LabelReport.Text += ElectionDetail(ElectionsRow);
    //  }

    //  LabelReport.Text += "<br><br>Elections: " + ElectionsFutureTable.Rows.Count.ToString();
    //}
    //private void FuturePresidentialPrimaries()
    //{
    //  DateTime LastDate = Convert.ToDateTime("01/01/1900");
    //  LabelReport.Text = "<strong>Future Presidential Primaries and Caucuses in ElectionsFuture Table</strong>";
    //  LabelReport.Text += "<br>------------------------------------------";
    //  LabelReport.Text += ElectionsHeading();
    //  DataTable ElectionsFutureTable = Table_ElectionsFuture();
    //  foreach (DataRow ElectionsRow in ElectionsFutureTable.Rows)
    //  {
    //    if (LastDate != Convert.ToDateTime(ElectionsRow["ElectionDate"]))
    //      LabelReport.Text += "<br>___________________________________________________________________<br>";
    //    LastDate = Convert.ToDateTime(ElectionsRow["ElectionDate"]);
    //    LabelReport.Text += ElectionDetail(ElectionsRow);
    //  }

    //  LabelReport.Text += "<br><br>Elections: " + ElectionsFutureTable.Rows.Count.ToString();

    //  LabelReport.Text += "<br><br><strong>States with NO Future Presidential Primaries and Caucuses in ElectionsFuture Table</strong>";
    //  DataTable Table_States = db.Table(sql.States_51());
    //  foreach (DataRow Row_State in Table_States.Rows)
    //  {
    //    if (db.Rows_Count_From("ElectionsFuture WHERE StateCode =" + db.SQLLit(Row_State["StateCode"].ToString())
    //      + " AND ElectionType = 'B'") == 0)
    //      LabelReport.Text += "<br>" + StateCache.GetStateName(Row_State["StateCode"].ToString());
    //  }
    //}
    //private void FutureStatePrimaries()
    //{
    //  DateTime LastDate = Convert.ToDateTime("01/01/1900");
    //  LabelReport.Text = "<strong>Future State Primaries in ElectionsFuture Table</strong>";
    //  LabelReport.Text += "<br>------------------------------------------";
    //  LabelReport.Text += ElectionsHeading();
    //  DataTable ElectionsFutureTable = Table_ElectionsFuture();
    //  foreach (DataRow ElectionsRow in ElectionsFutureTable.Rows)
    //  {
    //    if (LastDate != Convert.ToDateTime(ElectionsRow["ElectionDate"]))
    //      LabelReport.Text += "<br>___________________________________________________________________<br>";
    //    LastDate = Convert.ToDateTime(ElectionsRow["ElectionDate"]);
    //    LabelReport.Text += ElectionDetail(ElectionsRow);
    //  }

    //  LabelReport.Text += "<br><br>Elections: " + ElectionsFutureTable.Rows.Count.ToString();

    //  LabelReport.Text += "<br><br><strong>States with NO Future State Primaries in ElectionsFuture Table</strong>";
    //  DataTable Table_States = db.Table(sql.States_51());
    //  foreach (DataRow Row_State in Table_States.Rows)
    //  {
    //    if (db.Rows_Count_From("ElectionsFuture WHERE StateCode =" + db.SQLLit(Row_State["StateCode"].ToString())
    //      + " AND ElectionType = 'P'") == 0)
    //      LabelReport.Text += "<br>" + StateCache.GetStateName(Row_State["StateCode"].ToString());
    //  }
    //}
    //private void FutureGeneralElections()
    //{
    //  DateTime LastDate = Convert.ToDateTime("01/01/1900");
    //  LabelReport.Text = "<strong>Future General Elections in ElectionsFuture Table</strong>";
    //  LabelReport.Text += "<br>------------------------------------------";
    //  LabelReport.Text += ElectionsHeading();
    //  DataTable ElectionsFutureTable = Table_ElectionsFuture();
    //  foreach (DataRow ElectionsRow in ElectionsFutureTable.Rows)
    //  {
    //    if (LastDate != Convert.ToDateTime(ElectionsRow["ElectionDate"]))
    //      LabelReport.Text += "<br>___________________________________________________________________<br>";
    //    LastDate = Convert.ToDateTime(ElectionsRow["ElectionDate"]);
    //    LabelReport.Text += ElectionDetail(ElectionsRow);
    //  }

    //  LabelReport.Text += "<br><br>Elections: " + ElectionsFutureTable.Rows.Count.ToString();

    //  LabelReport.Text += "<br><br><strong>States with NO Future General Elections in ElectionsFuture Table</strong>";
    //  DataTable Table_States = db.Table(sql.States_51());
    //  foreach (DataRow Row_State in Table_States.Rows)
    //  {
    //    if (db.Rows_Count_From("ElectionsFuture WHERE StateCode =" + db.SQLLit(Row_State["StateCode"].ToString())
    //      + " AND ElectionType = 'G'") == 0)
    //      LabelReport.Text += "<br>" + StateCache.GetStateName(Row_State["StateCode"].ToString());
    //  }
    //}
    //private void FutureSpecialElections()
    //{
    //  DateTime LastDate = Convert.ToDateTime("01/01/1900");
    //  LabelReport.Text = "<strong>Future Special Elections in ElectionsFuture Table</strong>";
    //  LabelReport.Text += "<br>------------------------------------------";
    //  LabelReport.Text += ElectionsHeading();
    //  DataTable ElectionsFutureTable = Table_ElectionsFuture();
    //  foreach (DataRow ElectionsRow in ElectionsFutureTable.Rows)
    //  {
    //    if (LastDate != Convert.ToDateTime(ElectionsRow["ElectionDate"]))
    //      LabelReport.Text += "<br>___________________________________________________________________<br>";
    //    LastDate = Convert.ToDateTime(ElectionsRow["ElectionDate"]);
    //    LabelReport.Text += ElectionDetail(ElectionsRow);
    //  }

    //  LabelReport.Text += "<br><br>Elections: " + ElectionsFutureTable.Rows.Count.ToString();

    //  LabelReport.Text += "<br><br><strong>States with NO Future Special Elections in ElectionsFuture Table</strong>";
    //  DataTable Table_States = db.Table(sql.States_51());
    //  foreach (DataRow Row_State in Table_States.Rows)
    //  {
    //    if (db.Rows_Count_From("ElectionsFuture WHERE StateCode =" + db.SQLLit(Row_State["StateCode"].ToString())
    //      + " AND ElectionType = 'S'") == 0)
    //      LabelReport.Text += "<br>" + StateCache.GetStateName(Row_State["StateCode"].ToString());
    //  }
    //}
    //#endregion Future Elections Reports

    //private void PoliticianAdds()
    //{
    //  LabelReport.Text += "<br><br>";
    //  DataTable MasterUsersTable = db.Table(sql.MasterUsers());
    //  foreach (DataRow MasterUserRow in MasterUsersTable.Rows)
    //  {
    //    //DataTable PoliticianAddsTable = db.Table(sql.MasterPoliticianAdds(MasterUserRow["UserName"].ToString()));
    //    var politicianAddsTable =
    //      DB.VoteLog.LogPoliticianAdds.GetStateCodeDataByUserName(MasterUserRow["UserName"].ToString());
    //    LabelReport.Text += "<br>" + MasterUserRow["UserName"].ToString()
    //      + ": " + politicianAddsTable.Count.ToString();
    //  }
    //}

    ////public static void OfficialsReportHeading(ref HtmlTable HtmlTableReport, string Heading)
    ////{
    ////  HtmlTableRow HTMLTr = db.Add_Tr_To_Table_Return_Tr(
    ////    HtmlTableReport
    ////    , "trReportGroupHeading");
    ////  db.Add_Td_To_Tr(
    ////    HTMLTr
    ////    , Heading
    ////    , "tdReportGroupHeading"
    ////    , 2);
    ////}
    //public static void WinnersReportHeading(ref HtmlTable HtmlTableReport, string Heading)
    //{
    //  HtmlTableRow HTMLTr = db.Add_Tr_To_Table_Return_Tr(
    //    HtmlTableReport
    //    , "trReportGroupHeading");
    //  db.Add_Td_To_Tr(
    //    HTMLTr
    //    , Heading
    //    , "tdReportGroupHeading"
    //    , 3);
    //}

    //#region Elections
    ////private void Report_Elections_Heading(ref HtmlTable HtmlTable_Report_Elections, string Heading)
    ////{
    ////  HtmlTableRow USPresidentHeadingHTMLTr = db.Add_Tr_To_Table_Return_Tr(
    ////    HtmlTable_Report_Elections
    ////    , "trReportGroupHeading");
    ////  db.Add_Td_To_Tr(
    ////    USPresidentHeadingHTMLTr
    ////    //, "Elections for U.S. President"
    ////    , Heading
    ////    , "tdReportGroupHeading"
    ////    //, "center"
    ////    , 5);
    ////}
    ////private void Report_Elections_SubHeading(ref HtmlTable HtmlTable_Report_Elections)
    ////{
    ////  HtmlTableRow TrArchiveHeading = db.Add_Tr_To_Table_Return_Tr(
    ////    HtmlTable_Report_Elections, "trReportDetailHeading");
    ////  db.Add_Td_To_Tr(TrArchiveHeading, "<nobr>Election Description</nobr>", "tdReportDetailHeading");
    ////  db.Add_Td_To_Tr(TrArchiveHeading, "Election Date", "tdReportDetailHeading");
    ////  db.Add_Td_To_Tr(TrArchiveHeading, "Last Updated", "tdReportDetailHeading");
    ////  db.Add_Td_To_Tr(TrArchiveHeading, "Current Status", "tdReportDetailHeading");
    ////  db.Add_Td_To_Tr(TrArchiveHeading, "Viewable Status", "tdReportDetailHeading");

    ////  //db.Add_Td_To_Tr(ref TrArchiveHeading, " ", "tdReportDetailHeadingLastCollumn");
    ////}
    ////private void Report_Elections_Detail(ref HtmlTable HtmlTable_Report_Elections, string SQLArchive)
    ////{
    ////  HtmlTableRow HTMLTrArchiveElection = null;
    ////  DataTable ArchiveElectionTable = db.Table(SQLArchive);
    ////  if (db.Rows(SQLArchive) > 0)
    ////  {
    ////    DateTime ElectionDate = DateTime.MinValue;
    ////    DateTime ReportLastUpdated = DateTime.MinValue;
    ////    bool Is_Report_For_Current = false;
    ////    bool IsViewable = false;
    ////    string Anchor = string.Empty;
    ////    foreach (DataRow HTMLArchiveElectionRow in ArchiveElectionTable.Rows)
    ////    {
    ////      HTMLTrArchiveElection = db.Add_Tr_To_Table_Return_Tr(HtmlTable_Report_Elections, "trReportDetail");

    ////      string AnchorText = HTMLArchiveElectionRow["ElectionDesc"].ToString();

    ////      if (db.Elections_Bool(
    ////        HTMLArchiveElectionRow["ElectionKey"].ToString(),
    ////        "IsWinnersIdentified"
    ////        ))
    ////      {
    ////        AnchorText += "<span style=color:red> (winners identified)</span>";
    ////      }

    ////      db.Add_Td_To_Tr(
    ////        HTMLTrArchiveElection
    ////        , AnchorText
    ////        , "tdReportDetail"
    ////        );

    ////      ElectionDate = (DateTime)HTMLArchiveElectionRow["ElectionDate"];
    ////      db.Add_Td_To_Tr(HTMLTrArchiveElection, ElectionDate.ToString("MM/dd/yyyy"), "tdReportDetail");

    ////      ReportLastUpdated = (DateTime)HTMLArchiveElectionRow["ReportLastUpdated"];
    ////      db.Add_Td_To_Tr(HTMLTrArchiveElection, ReportLastUpdated.ToString(), "tdReportDetail");

    ////      Is_Report_For_Current = Convert.ToBoolean(HTMLArchiveElectionRow["IsReportCurrent"]);
    ////      if (Is_Report_For_Current)
    ////        db.Add_Td_To_Tr(HTMLTrArchiveElection, "Current", "tdReportDetail");
    ////      else
    ////        db.Add_Td_To_Tr(HTMLTrArchiveElection, "NOT Current", "tdReportDetail");

    ////      IsViewable = Convert.ToBoolean(HTMLArchiveElectionRow["IsViewable"]);
    ////      if (IsViewable)
    ////        db.Add_Td_To_Tr(HTMLTrArchiveElection, "Viewable", "tdReportDetail");
    ////      else
    ////        db.Add_Td_To_Tr(HTMLTrArchiveElection, "NOT Viewable", "tdReportDetail");

    ////      //db.Add_Td_To_Tr(ref HTMLTrArchiveElection, " ", "tdReportDetailHeadingLastCollumn");
    ////    }
    ////  }
    ////  else
    ////  {
    ////    HTMLTrArchiveElection = db.Add_Tr_To_Table_Return_Tr(HtmlTable_Report_Elections, "trReportDetail");
    ////    db.Add_Td_To_Tr(HTMLTrArchiveElection, "No Reports Available", "tdReportDetail", 5);
    ////  }
    ////}

    ////private void Report_Elections(ref HtmlTable HtmlTable_Report_Elections, string SQLArchive, string Heading)
    ////{
    ////  //Heading Elections for U.S. President
    ////  Report_Elections_Heading(ref HtmlTable_Report_Elections, Heading);

    ////  // Election Description | Date | Last Updated | Status Heading
    ////  Report_Elections_SubHeading(
    ////    ref HtmlTable_Report_Elections
    ////    );

    ////  // DataTable ArchiveElectionTable = db.Table(SQLArchive);
    ////  Report_Elections_Detail(ref HtmlTable_Report_Elections, SQLArchive);
    ////}

    ////private string Sql_ElectionReports_State(string StateCode, bool IsUpcoming)
    ////{
    ////  string SQL = string.Empty;
    ////  SQL += " SELECT Elections.ElectionKey";
    ////  SQL += ",Elections.ElectionDesc";
    ////  SQL += ",Elections.ElectionDate";
    ////  SQL += ",Elections.IsViewable";
    ////  SQL += ",Elections.StateCode";
    ////  SQL += ",Elections.CountyCode";
    ////  SQL += ",Elections.LocalCode";
    ////  SQL += ",ReportsElections.ReportLastUpdated";
    ////  SQL += ",ReportsElections.IsReportCurrent";
    ////  SQL += " FROM Elections,ReportsElections";
    ////  SQL += " WHERE Elections.ElectionKey = ReportsElections.ElectionKey";
    ////  SQL += " AND Elections.StateCode = " + db.SQLLit(StateCode);
    ////  SQL += " AND Elections.CountyCode = ''";
    ////  if (IsUpcoming)
    ////    SQL += " AND Elections.ElectionDate >= " + db.SQLLit(Db.DbToday);
    ////  else
    ////    SQL += " AND Elections.ElectionDate < " + db.SQLLit(Db.DbToday);
    ////  SQL += " ORDER BY Elections.ElectionDate DESC";
    ////  return SQL;
    ////}
    ////private string Sql_ElectionReports_County(string StateCode, string CountyCode, bool IsUpcoming)
    ////{
    ////  string SQL = string.Empty;
    ////  SQL += " SELECT Elections.ElectionKey";
    ////  SQL += ",Elections.ElectionDesc";
    ////  SQL += ",Elections.ElectionDate";
    ////  SQL += ",Elections.IsViewable";
    ////  SQL += ",Elections.StateCode";
    ////  SQL += ",Elections.CountyCode";
    ////  SQL += ",Elections.LocalCode";
    ////  SQL += ",ReportsElections.ReportLastUpdated";
    ////  SQL += ",ReportsElections.IsReportCurrent";
    ////  SQL += " FROM Elections,ReportsElections";
    ////  SQL += " WHERE Elections.ElectionKey = ReportsElections.ElectionKey";
    ////  SQL += " AND Elections.StateCode = " + db.SQLLit(StateCode);
    ////  SQL += " AND Elections.CountyCode = " + db.SQLLit(CountyCode);
    ////  SQL += " AND Elections.LocalCode = ''";
    ////  if (IsUpcoming)
    ////    SQL += " AND Elections.ElectionDate >= " + db.SQLLit(Db.DbToday);
    ////  else
    ////    SQL += " AND Elections.ElectionDate < " + db.SQLLit(Db.DbToday);
    ////  SQL += " ORDER BY Elections.ElectionDate DESC";
    ////  return SQL;
    ////}
    ////private string Sql_ElectionReports_Local(string StateCode, string CountyCode, string LocalCode, bool IsUpcoming)
    ////{
    ////  string SQL = string.Empty;
    ////  SQL += " SELECT Elections.ElectionKey";
    ////  SQL += ",Elections.ElectionDesc";
    ////  SQL += ",Elections.ElectionDate";
    ////  SQL += ",Elections.IsViewable";
    ////  SQL += ",Elections.StateCode";
    ////  SQL += ",Elections.CountyCode";
    ////  SQL += ",Elections.LocalCode";
    ////  SQL += ",ReportsElections.ReportLastUpdated";
    ////  SQL += ",ReportsElections.IsReportCurrent";
    ////  SQL += " FROM Elections,ReportsElections";
    ////  SQL += " WHERE Elections.ElectionKey = ReportsElections.ElectionKey";
    ////  SQL += " AND Elections.StateCode = " + db.SQLLit(StateCode);
    ////  SQL += " AND Elections.CountyCode = " + db.SQLLit(CountyCode);
    ////  SQL += " AND Elections.LocalCode = " + db.SQLLit(LocalCode);
    ////  if (IsUpcoming)
    ////    SQL += " AND Elections.ElectionDate >= " + db.SQLLit(Db.DbToday);
    ////  else
    ////    SQL += " AND Elections.ElectionDate < " + db.SQLLit(Db.DbToday);
    ////  SQL += " ORDER BY Elections.ElectionDate DESC";
    ////  return SQL;
    ////}
    //#endregion Elections

    //private void Report_Status(string What)
    //{
    //  LabelReport.Visible = false; //old reports

    //  HtmlTable HtmlTable_Report = new System.Web.UI.HtmlControls.HtmlTable();
    //  db.Html_Table_Attributes_Report(ref HtmlTable_Report, db.ReportUser.Master);

    //  //if (What == "ElectionsNextUS")
    //  //{
    //  //  #region Upcoming Federal Elections

    //  //  Report_Elections(
    //  //    ref HtmlTable_Report
    //  //    , Sql_ElectionReports_State("U1", true)
    //  //    , "Upcoming Elections for U.S. President");

    //  //  Report_Elections(
    //  //    ref HtmlTable_Report
    //  //    , Sql_ElectionReports_State("U2", true)
    //  //    , "Upcoming Elections for U.S. Senate");

    //  //  Report_Elections(
    //  //    ref HtmlTable_Report
    //  //    , Sql_ElectionReports_State("U3", true)
    //  //    , "Upcoming Elections for U.S. House of Representatives");

    //  //  Report_Elections(
    //  //    ref HtmlTable_Report
    //  //    , Sql_ElectionReports_State("U4", true)
    //  //    , "Upcoming Elections for State Governors");

    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);
    //  //  #endregion Upcoming Federal Elections
    //  //}
    //  //else if (What == "ElectionsNextStates")
    //  //{
    //  //  #region Upcoming State Elections
    //  //  DataTable StatesTable = db.Table(sql.States_51());
    //  //  foreach (DataRow StateRow in StatesTable.Rows)
    //  //  {
    //  //    Report_Elections(
    //  //      ref HtmlTable_Report
    //  //      , Sql_ElectionReports_State(StateRow["StateCode"].ToString(), true)
    //  //      , "Upcoming Elections for " + StateCache.GetStateName(StateRow["StateCode"].ToString()));
    //  //  }
    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);
    //  //  #endregion Upcoming State Elections
    //  //}
    //  //else if (What == "ElectionsNextCounties")
    //  //{
    //  //  #region Upcoming County Elections
    //  //  DataTable StatesTable = db.Table(sql.States_51());
    //  //  foreach (DataRow StateRow in StatesTable.Rows)
    //  //  {
    //  //    #region Each State
    //  //    DataTable CountiesTable = db.Table(sql.Counties(StateRow["StateCode"].ToString().Trim()));
    //  //    foreach (DataRow CountyRow in CountiesTable.Rows)
    //  //    {
    //  //      #region each county in State
    //  //      Report_Elections_Heading(
    //  //        ref HtmlTable_Report
    //  //        , "Upcoming Elections for "
    //  //            + StateCache.GetStateName(StateRow["StateCode"].ToString())
    //  //            + ", " + CountyCache.GetCountyName(StateRow["StateCode"].ToString(), CountyRow["CountyCode"].ToString())
    //  //        );
    //  //      Report_Elections_SubHeading(
    //  //        ref HtmlTable_Report
    //  //        );

    //  //      Report_Elections_Detail(
    //  //        ref HtmlTable_Report
    //  //        , Sql_ElectionReports_County(
    //  //            StateRow["StateCode"].ToString()
    //  //            , CountyRow["CountyCode"].ToString()
    //  //            , true
    //  //            )
    //  //        );

    //  //      #endregion each county in State
    //  //    }
    //  //    #endregion Each State
    //  //  }
    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);
    //  //  #endregion Upcoming County Elections
    //  //}
    //  //else if (What == "ElectionsNextLocals")
    //  //{
    //  //  #region Upcoming Local Elections

    //  //  DataTable StatesTable = db.Table(sql.States_51());
    //  //  foreach (DataRow StateRow in StatesTable.Rows)
    //  //  {
    //  //    #region Each State
    //  //    DataTable CountiesTable = db.Table(sql.Counties(StateRow["StateCode"].ToString().Trim()));
    //  //    foreach (DataRow CountyRow in CountiesTable.Rows)
    //  //    {
    //  //      DataTable LocalsTable = db.Table(sql.LocalDistricts(
    //  //        StateRow["StateCode"].ToString()
    //  //      , CountyRow["CountyCode"].ToString()));
    //  //      foreach (DataRow LocalRow in LocalsTable.Rows)
    //  //      {
    //  //        #region each local in County in State
    //  //        Report_Elections_Heading(
    //  //          ref HtmlTable_Report
    //  //          , "Upcoming Elections for "
    //  //              + StateCache.GetStateName(StateRow["StateCode"].ToString())
    //  //              + ", " + CountyCache.GetCountyName(
    //  //                StateRow["StateCode"].ToString()
    //  //                , CountyRow["CountyCode"].ToString())
    //  //              + ", " + db.Name_Local(
    //  //                StateRow["StateCode"].ToString()
    //  //                , CountyRow["CountyCode"].ToString()
    //  //                , LocalRow["LocalCode"].ToString())
    //  //          );
    //  //        Report_Elections_SubHeading(
    //  //          ref HtmlTable_Report
    //  //          );

    //  //        Report_Elections_Detail(
    //  //          ref HtmlTable_Report
    //  //          , Sql_ElectionReports_Local(
    //  //              StateRow["StateCode"].ToString()
    //  //              , CountyRow["CountyCode"].ToString()
    //  //              , LocalRow["LocalCode"].ToString()
    //  //              , true
    //  //              )
    //  //          );

    //  //        #endregion each local in County in State
    //  //      }
    //  //    }
    //  //    #endregion Each State
    //  //  }
    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);

    //  //  #endregion Upcoming Local Elections
    //  //}

    //  //else if (What == "ElectionsPreviousUS")//All US Pres, Senate and House elections
    //  //{
    //  //  #region Previous Federal Elections

    //  //  Report_Elections(
    //  //    ref HtmlTable_Report
    //  //    , Sql_ElectionReports_State("U1", false)
    //  //    , "Previous Elections for U.S. President");

    //  //  Report_Elections(
    //  //    ref HtmlTable_Report
    //  //    , Sql_ElectionReports_State("U2", false)
    //  //    , "Previous Elections for U.S. Senate");

    //  //  Report_Elections(
    //  //    ref HtmlTable_Report
    //  //    , Sql_ElectionReports_State("U3", false)
    //  //    , "Previous Elections for U.S. House of Representatives");

    //  //  Report_Elections(
    //  //    ref HtmlTable_Report
    //  //    , Sql_ElectionReports_State("U4", false)
    //  //    , "Previous Elections for Governors");

    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);
    //  //  #endregion Previous Federal Elections
    //  //}
    //  //else if (What == "ElectionsPreviousStates")//All 51 States' previous elections
    //  //{
    //  //  #region Previous State Elections

    //  //  DataTable StatesTable = db.Table(sql.States_51());
    //  //  foreach (DataRow StateRow in StatesTable.Rows)
    //  //  {
    //  //    Report_Elections_Heading(
    //  //      ref HtmlTable_Report
    //  //      , "Previous Elections for " + StateCache.GetStateName(StateRow["StateCode"].ToString())
    //  //      );
    //  //    Report_Elections_SubHeading(
    //  //      ref HtmlTable_Report
    //  //      );
    //  //    Report_Elections_Detail(
    //  //      ref HtmlTable_Report
    //  //      , Sql_ElectionReports_State(
    //  //          StateRow["StateCode"].ToString()
    //  //          , false
    //  //          )
    //  //      );
    //  //  }
    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);

    //  //  #endregion Previous State Elections
    //  //}
    //  //else if (What == "ElectionsPreviousCounties")
    //  //{
    //  //  #region Previous County Elections

    //  //  DataTable StatesTable = db.Table(sql.States_51());
    //  //  foreach (DataRow StateRow in StatesTable.Rows)
    //  //  {
    //  //    #region Each State
    //  //    DataTable CountiesTable = db.Table(sql.Counties(StateRow["StateCode"].ToString().Trim()));
    //  //    foreach (DataRow CountyRow in CountiesTable.Rows)
    //  //    {
    //  //      #region each county in State
    //  //      Report_Elections_Heading(
    //  //        ref HtmlTable_Report
    //  //        , "Previous Elections for "
    //  //            + StateCache.GetStateName(StateRow["StateCode"].ToString())
    //  //            + ", " + CountyCache.GetCountyName(StateRow["StateCode"].ToString(), CountyRow["CountyCode"].ToString())
    //  //        );
    //  //      Report_Elections_SubHeading(
    //  //        ref HtmlTable_Report
    //  //        );

    //  //      Report_Elections_Detail(
    //  //        ref HtmlTable_Report
    //  //        , Sql_ElectionReports_County(
    //  //            StateRow["StateCode"].ToString()
    //  //            , CountyRow["CountyCode"].ToString()
    //  //            , false
    //  //            )
    //  //        );

    //  //      #endregion each county in State
    //  //    }
    //  //    #endregion Each State
    //  //  }
    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);

    //  //  #endregion Previous County Elections
    //  //}
    //  //else if (What == "ElectionsPreviousLocals")
    //  //{
    //  //  #region Previous Local Elections

    //  //  DataTable StatesTable = db.Table(sql.States_51());
    //  //  foreach (DataRow StateRow in StatesTable.Rows)
    //  //  {
    //  //    #region Each State
    //  //    DataTable CountiesTable = db.Table(sql.Counties(StateRow["StateCode"].ToString().Trim()));
    //  //    foreach (DataRow CountyRow in CountiesTable.Rows)
    //  //    {
    //  //      DataTable LocalsTable = db.Table(sql.LocalDistricts(
    //  //        StateRow["StateCode"].ToString()
    //  //      , CountyRow["CountyCode"].ToString()));
    //  //      foreach (DataRow LocalRow in LocalsTable.Rows)
    //  //      {
    //  //        #region each local in County in State
    //  //        Report_Elections_Heading(
    //  //          ref HtmlTable_Report
    //  //          , "Previous Elections for "
    //  //              + StateCache.GetStateName(StateRow["StateCode"].ToString())
    //  //              + ", " + CountyCache.GetCountyName(
    //  //              StateRow["StateCode"].ToString()
    //  //              , CountyRow["CountyCode"].ToString())
    //  //              + ", " + db.Name_Local(
    //  //              StateRow["StateCode"].ToString()
    //  //              , CountyRow["CountyCode"].ToString()
    //  //              , LocalRow["LocalCode"].ToString())
    //  //          );
    //  //        Report_Elections_SubHeading(
    //  //          ref HtmlTable_Report
    //  //          );

    //  //        Report_Elections_Detail(
    //  //          ref HtmlTable_Report
    //  //          , Sql_ElectionReports_Local(
    //  //              StateRow["StateCode"].ToString()
    //  //              , CountyRow["CountyCode"].ToString()
    //  //              , LocalRow["LocalCode"].ToString()
    //  //              , false
    //  //              )
    //  //          );

    //  //        #endregion each local in County in State
    //  //      }
    //  //    }
    //  //    #endregion Each State
    //  //  }
    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);

    //  //  #endregion Previous Local Elections
    //  //}
    //  ////----------------------------
    //  ////----------------------------
    //  //else 
    //  //if (What == "OfficialsUS")
    //  //{
    //  //  #region OfficialsUS

    //  //  HtmlTableRow HtmlTr = null;

    //  //  OfficialsReportHeading(ref HtmlTable_Report, "US President, US Senate, US House REPORTS for Officials.aspx Last Updated");
    //  //  HtmlTr = db.Add_Tr_To_Table_Return_Tr(HtmlTable_Report, "trReportDetail");
    //  //  db.Add_Td_To_Tr(HtmlTr, "US President", "tdReportDetail");
    //  //  db.Add_Td_To_Tr(HtmlTr, string.Empty//db.Report_Officials_Get_LastUpdate("U1")
    //  //    , "tdReportDetail");

    //  //  HtmlTr = db.Add_Tr_To_Table_Return_Tr(HtmlTable_Report, "trReportDetail");
    //  //  db.Add_Td_To_Tr(HtmlTr, "US Senate", "tdReportDetail");
    //  //  db.Add_Td_To_Tr(HtmlTr, string.Empty//db.Report_Officials_Get_LastUpdate("U2")
    //  //    , "tdReportDetail");

    //  //  HtmlTr = db.Add_Tr_To_Table_Return_Tr(HtmlTable_Report, "trReportDetail");
    //  //  db.Add_Td_To_Tr(HtmlTr, "US House", "tdReportDetail");
    //  //  db.Add_Td_To_Tr(HtmlTr, string.Empty//db.Report_Officials_Get_LastUpdate("U3")
    //  //    , "tdReportDetail");

    //  //  HtmlTr = db.Add_Tr_To_Table_Return_Tr(HtmlTable_Report, "trReportDetail");
    //  //  db.Add_Td_To_Tr(HtmlTr, "Governors", "tdReportDetail");
    //  //  db.Add_Td_To_Tr(HtmlTr, string.Empty//db.Report_Officials_Get_LastUpdate("U4")
    //  //    , "tdReportDetail");

    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);

    //  //  #endregion
    //  //}
    //  //else if (What == "OfficialsStates")
    //  //{
    //  //  #region OfficialsStates

    //  //  HtmlTableRow HtmlTr = null;

    //  //  OfficialsReportHeading(ref HtmlTable_Report, "STATE REPORTS for Officials.aspx Last Updated");
    //  //  DataTable StatesTable = db.Table(sql.States_51());
    //  //  foreach (DataRow StateRow in StatesTable.Rows)
    //  //  {
    //  //    HtmlTr = db.Add_Tr_To_Table_Return_Tr(HtmlTable_Report, "trReportDetail");
    //  //    db.Add_Td_To_Tr(HtmlTr, StateCache.GetStateName(StateRow["StateCode"].ToString()), "tdReportDetail");
    //  //    db.Add_Td_To_Tr(HtmlTr, string.Empty//db.Report_Officials_Get_LastUpdate(StateRow["StateCode"].ToString())
    //  //      , "tdReportDetail");
    //  //  }
    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);
    //  //  #endregion
    //  //}
    //  //else if (What == "OfficialsCounties")
    //  //{
    //  //  #region OfficialsCounties

    //  //  HtmlTableRow HtmlTr = null;

    //  //  OfficialsReportHeading(ref HtmlTable_Report, "COUNTY REPORTS for Officials.aspx Last Updated");

    //  //  DataTable StatesTable = db.Table(sql.States_51());
    //  //  foreach (DataRow StateRow in StatesTable.Rows)
    //  //  {
    //  //    #region Each State
    //  //    OfficialsReportHeading(ref HtmlTable_Report, StateCache.GetStateName(StateRow["StateCode"].ToString())
    //  //      + " Counties");

    //  //    DataTable CountiesTable = db.Table(sql.Counties(StateRow["StateCode"].ToString().Trim()));
    //  //    foreach (DataRow CountyRow in CountiesTable.Rows)
    //  //    {
    //  //      HtmlTr = db.Add_Tr_To_Table_Return_Tr(HtmlTable_Report, "trReportDetail");
    //  //      db.Add_Td_To_Tr(HtmlTr, CountyRow["County"].ToString(), "tdReportDetail");
    //  //      //<----------
    //  //      db.Add_Td_To_Tr(HtmlTr
    //  //        , 
    //  //        string.Empty//db.Report_Officials_Get_LastUpdate(StateRow["StateCode"].ToString(), CountyRow["CountyCode"].ToString())
    //  //        , "tdReportDetail");
    //  //    }
    //  //    #endregion Each State
    //  //  }

    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);
    //  //  #endregion
    //  //}
    //  //else if (What == "OfficialsLocals")
    //  //{
    //  //  OfficialsReportHeading(ref HtmlTable_Report, "LOCAL DISTRICT REPORTS for Officials.aspx Last Updated");

    //  //  var statesTable = db.Table(sql.States_51());
    //  //  foreach (DataRow stateRow in statesTable.Rows)
    //  //  {
    //  //    OfficialsReportHeading(ref HtmlTable_Report, StateCache.GetStateName(stateRow["StateCode"].ToString())
    //  //      + " Local Districts");
    //  //    var countiesTable = db.Table(sql.Counties(stateRow["StateCode"].ToString().Trim()));
    //  //    foreach (DataRow countyRow in countiesTable.Rows)
    //  //    {
    //  //      var localsTable = db.Table(sql.LocalDistricts(
    //  //        stateRow.StateCode()
    //  //      , countyRow.CountyCode()));
    //  //      foreach (DataRow localRow in localsTable.Rows)
    //  //      {
    //  //        var localName =
    //  //          CountyCache.GetCountyName(
    //  //              stateRow.StateCode()
    //  //              , countyRow.CountyCode()
    //  //              )
    //  //          + ", "
    //  //          + localRow.LocalDistrict()
    //  //          ;

    //  //        var htmlTr = db.Add_Tr_To_Table_Return_Tr(HtmlTable_Report, "trReportDetail");
    //  //        db.Add_Td_To_Tr(htmlTr, localName, "tdReportDetail");
    //  //        db.Add_Td_To_Tr(htmlTr
    //  //         , 
    //  //         string.Empty
    //  //         , "tdReportDetail");
    //  //      }
    //  //    }
    //  //  }

    //  //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);
    //  //}
    //  //else if (What == "Counties4Officials")
    //  //{
    //  //  #region  commented out Counties4Officials.aspx
    //  //  //LabelReport.Text += "<br><br>";
    //  //  //LabelReport.Text += "<br>--- COUNTY Counties4Officials.aspx REPORTS Last Updated";
    //  //  //LabelReport.Text += "<br><br>";
    //  //  //DataTable StatesTable = db.Table(sql.States_51());
    //  //  //foreach (DataRow StateRow in StatesTable.Rows)
    //  //  //{
    //  //  //  LabelReport.Text += "<br>" + StateRow["StateCode"].ToString()
    //  //  //    + ": " + db.States_Str(StateRow["StateCode"].ToString(), "CountiesReportOfficialsLastUpdated");
    //  //  //}
    //  //  #endregion
    //  //}
    //  //else if (What == "Counties4ElectionReports")
    //  //{
    //  //  #region commented out Counties4ElectionReports.aspx.aspx
    //  //  //LabelReport.Text += "<br><br>";
    //  //  //LabelReport.Text += "<br>--- COUNTY Counties4ElectionReports.aspx REPORTS Last Updated";
    //  //  //LabelReport.Text += "<br><br>";
    //  //  //DataTable StatesTable = db.Table(sql.States_51());
    //  //  //foreach (DataRow StateRow in StatesTable.Rows)
    //  //  //{
    //  //  //  //LabelReport.Text += "<br>" + StateRow["StateCode"].ToString()
    //  //  //  //  + ": " + db.States_Str(StateRow["StateCode"].ToString(), "ReportLastUpdated");
    //  //  //}
    //  //  #endregion
    //  //}
    //  //else
    //  if (What == "Offices")
    //  {
    //    #region Offices
    //    LabelReport.Text += "<br><br>";
    //    LabelReport.Text += "<br>--- States' Offices Lists: Last Updated | IsCurrent | Office Level";

    //    //51 States
    //    DataTable StatesTable = db.Table(sql.States_51());
    //    foreach (DataRow StateRow in StatesTable.Rows)
    //    {
    //      LabelReport.Text += "<br><br>" + StateCache.GetStateName(StateRow["StateCode"].ToString());
    //      for (int officeClass = OfficeClass.USSenate.ToInt(); officeClass <= OfficeClass.LocalParty.ToInt(); officeClass++)
    //      {
    //        LabelReport.Text += "<br>"
    //          + db.States_Str(StateRow["StateCode"].ToString(), "ReportLastUpdated" + officeClass.ToString())
    //          + " | " + db.States_Bool(StateRow["StateCode"].ToString(), "IsReportCurrent" + officeClass.ToString())
    //          + " | " + Offices.GetOfficeClassDescription(officeClass.ToOfficeClass(), StateRow["StateCode"].ToString());
    //      }
    //    }
    //    #endregion
    //  }
    //  else if (What == "Politicians")
    //  {
    //    #region Politicians
    //    #region Federal
    //    LabelReport.Text += "<br>--- Federal Politicians Lists: Last Updated | IsCurrent | Office Level";
    //    LabelReport.Text += "<br>";
    //    LabelReport.Text += "<br>"
    //      + db.States_Str("U1", "ReportLastUpdated" + OfficeClass.USPresident.ToInt())
    //      + " | " + db.States_Bool("U1", "IsReportCurrent" + OfficeClass.USPresident.ToInt())
    //      + " | " + Offices.GetOfficeClassDescription(OfficeClass.USPresident, "U1");
    //    LabelReport.Text += "<br>"
    //      + db.States_Str("U2", "ReportLastUpdated" + OfficeClass.USSenate.ToInt())
    //      + " | " + db.States_Bool("U2", "IsReportCurrent" + OfficeClass.USSenate.ToInt())
    //      + " | " + Offices.GetOfficeClassDescription(OfficeClass.USSenate, "U2");
    //    LabelReport.Text += "<br>"
    //      + db.States_Str("U3", "ReportLastUpdated" + OfficeClass.USHouse.ToInt())
    //      + " | " + db.States_Bool("U3", "IsReportCurrent" + OfficeClass.USHouse.ToInt())
    //      + " | " + Offices.GetOfficeClassDescription(OfficeClass.USHouse, "U3");
    //    LabelReport.Text += "<br>"
    //      + db.States_Str("U4", "ReportLastUpdated" + OfficeClass.USGovernors.ToInt())
    //      + " | " + db.States_Bool("U4", "IsReportCurrent" + OfficeClass.USGovernors.ToInt())
    //      + " | " + Offices.GetOfficeClassDescription(OfficeClass.USGovernors, "U4");
    //    #endregion Federal

    //    LabelReport.Text += "<br><br>";
    //    LabelReport.Text += "<br>--- States' Politicians Lists: Last Updated | IsCurrent | Office Level";

    //    //51 States
    //    DataTable StatesTable = db.Table(sql.States_51());
    //    foreach (DataRow StateRow in StatesTable.Rows)
    //    {
    //      LabelReport.Text += "<br><br>" + StateCache.GetStateName(StateRow["StateCode"].ToString());
    //      for (int officeClass = OfficeClass.USPresident.ToInt(); officeClass <= OfficeClass.LocalParty.ToInt(); officeClass++)
    //      {
    //        LabelReport.Text += "<br>"
    //          + db.States_Str(StateRow["StateCode"].ToString(), "ReportLastUpdated" + officeClass.ToString())
    //          + " | " + db.States_Bool(StateRow["StateCode"].ToString(), "IsReportCurrent" + officeClass.ToString().ToString())
    //          + " | " + Offices.GetOfficeClassDescription(officeClass.ToOfficeClass(), StateRow["StateCode"].ToString());
    //      }
    //    }
    //    #endregion
    //  }
    //}
    //private void WinnersStatus()
    //{
    //  LabelReport.Visible = false; //old reports

    //  HtmlTable HtmlTable_Report = new HtmlTable();
    //  db.Html_Table_Attributes_Report(ref HtmlTable_Report, db.ReportUser.Master);

    //  HtmlTableRow HtmlTr = null;

    //  WinnersReportHeading(
    //    ref HtmlTable_Report, 
    //    "STATE Election Winners Identified");
    //  DataTable StatesTable = db.Table(sql.States_51());
    //  foreach (DataRow StateRow in StatesTable.Rows)
    //  {
    //    HtmlTr = db.Add_Tr_To_Table_Return_Tr(HtmlTable_Report, "trReportDetail");
    //    db.Add_Td_To_Tr(
    //      HtmlTr, 
    //      StateCache.GetStateName(StateRow["StateCode"].ToString()), 
    //      "tdReportDetail",
    //      1
    //      );
    //    //db.Add_Td_To_Tr(HtmlTr, db.Report_Officials_Get_LastUpdate(StateRow["StateCode"].ToString()), "tdReportDetail");
    //    db.Add_Td_To_Tr(
    //      HtmlTr, 
    //      db.Elections_Winners_Identified(StateRow["StateCode"].ToString()), 
    //      "tdReportDetail",
    //      2
    //      );
    //  }
    //  LabelHTMLTableReport.Text = db.RenderToString(HtmlTable_Report);
    //}

    ////private void Errors(string Day)
    ////{
    ////  DateTime DayDate = Convert.ToDateTime(Day);
    ////  DateTime NextDayDate = DayDate.AddDays(1);
    ////  string NextDay = NextDayDate.ToShortDateString();

    ////  HtmlTable HTMLErrorsTable = new HtmlTable();
    ////  HtmlTableRow HeadingTr = new HtmlTableRow();
    ////  HtmlTableRow HTMLErrorRow = new HtmlTableRow();

    ////  //heading
    ////  //<tr Class="HeadingTr">
    ////  HeadingTr = db.Add_Tr_To_Table_Return_Tr(HTMLErrorsTable, "HeadingTr");
    ////  //<td Class="PartyReportRowsHeading" align="center">
    ////  db.Add_Td_To_Tr(HeadingTr, "Date Time", "PartyReportRowsHeading", "center");
    ////  db.Add_Td_To_Tr(HeadingTr, "Domain", "PartyReportRowsHeading", "center");
    ////  db.Add_Td_To_Tr(HeadingTr, "Type", "PartyReportRowsHeading", "center");
    ////  db.Add_Td_To_Tr(HeadingTr, "Page", "PartyReportRowsHeading", "center");
    ////  db.Add_Td_To_Tr(HeadingTr, "Method", "PartyReportRowsHeading", "center");

    ////  HeadingTr = db.Add_Tr_To_Table_Return_Tr(HTMLErrorsTable, "HeadingTr");
    ////  db.Add_Td_To_Tr(HeadingTr, "Message", "PartyReportRowsHeading", "center", 5);

    ////  HeadingTr = db.Add_Tr_To_Table_Return_Tr(HTMLErrorsTable, "HeadingTr");
    ////  db.Add_Td_To_Tr(HeadingTr, "Stack Trace", "PartyReportRowsHeading", "center", 5);

    ////  //string SQL = "SELECT * FROM LogSystemErrors "
    ////  string SQL = "SELECT * FROM Log000ErrorsUnhandled "
    ////    + " WHERE DateStamp > " + db.SQLLit(Day)
    ////    + " AND DateStamp < " + db.SQLLit(NextDay);

    ////  DataTable ErrorsTable = db.Table(SQL);
    ////  foreach (DataRow ErrorRow in ErrorsTable.Rows)
    ////  {
    ////    //<tr Class="PartyRow">
    ////    HTMLErrorRow = db.Add_Tr_To_Table_Return_Tr(HTMLErrorsTable, "PartyRow");
    ////    //<td Class="ReportPartyRow" align="left">
    ////    db.Add_Td_To_Tr(HTMLErrorRow, ErrorRow["DateStamp"].ToString(), "ReportPartyRow");
    ////    db.Add_Td_To_Tr(HTMLErrorRow, ErrorRow["DomainServerName"].ToString(), "ReportPartyRow");
    ////    db.Add_Td_To_Tr(HTMLErrorRow, ErrorRow["Type"].ToString(), "ReportPartyRow");
    ////    db.Add_Td_To_Tr(HTMLErrorRow, ErrorRow["Page"].ToString(), "ReportPartyRow");
    ////    db.Add_Td_To_Tr(HTMLErrorRow, ErrorRow["EventMethod"].ToString(), "ReportPartyRow");

    ////    HTMLErrorRow = db.Add_Tr_To_Table_Return_Tr(HTMLErrorsTable, "PartyRow");
    ////    db.Add_Td_To_Tr(HTMLErrorRow, db.Str_CrLf_To_Br(ErrorRow["Message"].ToString()), "ReportPartyRow", "left", 5);

    ////    //<tr Class="PartyRow">
    ////    HTMLErrorRow = db.Add_Tr_To_Table_Return_Tr(HTMLErrorsTable, "PartyRow");
    ////    //<td Class="ReportPartyRow" align="center colspan=6">
    ////    db.Add_Td_To_Tr(HTMLErrorRow, db.Str_CrLf_To_Br(ErrorRow["StackTrace"].ToString()), "ReportPartyRow", "left", 5);
    ////  }

    ////  LabelReport.Text = db.RenderToString(HTMLErrorsTable);
    ////}

    //private string OfficeID(string OfficeKey)
    //{
    //  DataRow Row_Office = db.Row("Select * From Offices where OfficeKey =" + db.SQLLit(OfficeKey));
    //  return "<br><br><strong>Offices Table row for OfficeKey:"
    //  + Row_Office["OfficeKey"].ToString()
    //  + " OfficeTitle:" + Row_Office["OfficeLine1"].ToString() + " " + Row_Office["OfficeLine2"].ToString() + "</strong>";
    //}
    //private string PoliticianID(string PoliticianKey)
    //{
    //  DataRow PoliticianRow = db.Row("Select * From Politicians where PoliticianKey =" + db.SQLLit(PoliticianKey));
    //  return "<br><br><strong>Politicians Table row for PoliticianKey:"
    //  + PoliticianRow["PoliticianKey"].ToString()
    //  + " Name:" + PoliticianRow["FName"].ToString()
    //  + " " + PoliticianRow["MName"].ToString()
    //  + " " + PoliticianRow["LName"].ToString()
    //  + "</strong>";
    //}
    //private void CheckTable_OfficesKeys()
    //{
    //  LabelReport.Text += "Offices Table rows with unmatching key columns rows for"
    //  + " StateCode, CountyCode, and LocalDistrictCodeStates"
    //  + " in tables States, Counties and LocalDistricts";
    //  LabelReport.Text += "<br><br>";

    //  DataTable Table_Offices = db.Table("SELECT * from Offices");
    //  foreach (DataRow Row_Office in Table_Offices.Rows)
    //  {
    //    string sql = string.Empty;

    //    if (Row_Office["OfficeLevel"].ToOfficeClass().IsStateOrFederal())
    //    {
    //      #region State Level Offices
    //      #region Check Keys are Good
    //      //if (db.Rows_Sql
    //      //  (
    //      //  "Select Count(*) FROM States"
    //      //  + " WHERE StateCode =" + db.SQLLit(Row_Office["StateCode"].ToString())
    //      //) == 0)
    //      if (db.Rows("States"
    //        , "StateCode", Row_Office["StateCode"].ToString()) == 0)
    //      {
    //        LabelReport.Text += OfficeID(Row_Office["OfficeKey"].ToString())
    //        + "<br>"
    //        + "States Table needs a row for StateCode:" + Row_Office["StateCode"].ToString();
    //      }
    //      #endregion Check Keys are Good

    //      #region Check Unused LocalCode Key is Empty
    //      if (Row_Office["CountyCode"].ToString() != string.Empty)
    //      {
    //        LabelReport.Text += OfficeID(Row_Office["OfficeKey"].ToString())
    //        + "<br>"
    //        + "For a State level office, Offices Table CountyCode should be empty but contains:" + Row_Office["CountyCode"].ToString();
    //      }
    //      if (Row_Office["LocalCode"].ToString() != string.Empty)
    //      {
    //        LabelReport.Text += OfficeID(Row_Office["OfficeKey"].ToString())
    //        + "<br>"
    //        + "For a State level office, Offices Table LocalCode should be empty but contains:" + Row_Office["LocalCode"].ToString();
    //      }
    //      #endregion Check Unused LocalCode Key is Empty
    //      #endregion State Level Offices
    //    }

    //    if (Row_Office["OfficeLevel"].ToOfficeClass().IsCounty())
    //    {
    //      #region County Level Offices
    //      #region Check Keys are Good
    //      //if (db.Rows_Sql
    //      //  (
    //      //  "Select Count(*) FROM Counties"
    //      //  + " WHERE StateCode =" + db.SQLLit(Row_Office["StateCode"].ToString())
    //      //  + " AND CountyCode =" + db.SQLLit(Row_Office["CountyCode"].ToString())
    //      //  ) == 0)
    //      if (db.Rows("Counties"
    //        , "StateCode", Row_Office["StateCode"].ToString()
    //        , "CountyCode", Row_Office["CountyCode"].ToString()) == 0)
    //      {
    //        LabelReport.Text += OfficeID(Row_Office["OfficeKey"].ToString())
    //        + "<br>"
    //        + "Counties Table is missing a row for StateCode:" + Row_Office["StateCode"].ToString()
    //        + ", CountyCode:" + Row_Office["CountyCode"].ToString();
    //      }
    //      #endregion Check Keys are Good

    //      #region Check Unused LocalCode is Empty
    //      if (Row_Office["LocalCode"].ToString() != string.Empty)
    //      {
    //        LabelReport.Text += OfficeID(Row_Office["OfficeKey"].ToString())
    //        + "<br>"
    //        + "For a county level office, Offices Table LocalCode should be empty but contains:" + Row_Office["LocalCode"].ToString();
    //      }
    //      #endregion Check Unused LocalCode is Empty
    //      #endregion County Level Offices
    //    }

    //    if (Row_Office["OfficeLevel"].ToOfficeClass().IsLocal())
    //    {
    //      #region LocalCode Level Offices
    //      #region Check Keys are Good
    //      //if (db.Rows_Sql
    //      //  (
    //      //  "Select Count(*) FROM LocalDistricts"
    //      //  + " WHERE StateCode =" + db.SQLLit(Row_Office["StateCode"].ToString())
    //      //  + " AND CountyCode =" + db.SQLLit(Row_Office["CountyCode"].ToString())
    //      //  + " AND LocalCode =" + db.SQLLit(Row_Office["LocalCode"].ToString())
    //      //) == 0)
    //      if (db.Rows("LocalDistricts"
    //        , "StateCode", Row_Office["StateCode"].ToString()
    //        , "CountyCode", Row_Office["CountyCode"].ToString()
    //        , "LocalCode", Row_Office["LocalCode"].ToString()) == 0)
    //      {
    //        LabelReport.Text += OfficeID(Row_Office["OfficeKey"].ToString())
    //        + "<br>"
    //        + "LocalDistricts Table is missing a row for StateCode:" + Row_Office["StateCode"].ToString()
    //        + ", CountyCode:" + Row_Office["CountyCode"].ToString()
    //        + ", LocalCode:" + Row_Office["LocalCode"].ToString();
    //      }
    //      #endregion Check Keys are Good

    //      #region Check Unused Kes are Empty
    //      #endregion Check Unused Kes are Empty

    //      #endregion LocalCode Level Offices
    //    }
    //  }
    //  LabelReport.Text += "<br><br>Done checking Offices Table";
    //  Msg.Text = db.Ok("DONE");

    //}
    //private void CheckPoliticiansTableKeys()
    //{
    //  LabelReport.Text += "Politicians Table rows with unmatching key columns rows for OfficeKey in Offices Table";
    //  LabelReport.Text += "<br><br>";

    //  DataTable politiciansTable = db.Table("SELECT * from Politicians");
    //  foreach (DataRow politicianRow in politiciansTable.Rows)
    //  {
    //    string politicianKey = politicianRow["PoliticianKey"].ToString();
    //    string officeKey =
    //      VotePage.GetPageCache().Politicians.GetOfficeKey(politicianKey);
    //    #region Politicians OfficeKey
    //    //string sql = string.Empty;
    //    //sql =
    //    //"Select Count(*) FROM Offices"
    //    //+ " WHERE OfficeKey =" + db.SQLLit(PoliticianRow["OfficeKey"].ToString());

    //    //if (db.Rows_Sql
    //    //  (
    //    //    "Select Count(*) FROM Offices"
    //    //    + " WHERE OfficeKey =" + db.SQLLit(PoliticianRow["OfficeKey"].ToString())
    //    //  ) == 0)
    //    if (db.Rows("Offices", "OfficeKey", officeKey) == 0)
    //    {
    //      LabelReport.Text += PoliticianID(politicianKey)
    //      + "<br>"
    //      + "Offices Table is missing a row for OfficeKey:" + politicianRow["OfficeKey"].ToString();
    //    }

    //    #endregion Politicians OfficeKey
    //  }

    //  LabelReport.Text += "<br><br>Done checking Offices Table";
    //  Msg.Text = db.Ok("DONE");
    //}

    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  if (!IsPostBack) //first time presented
    //  {
    //    //			Session.Timeout = 6000;
    //    Server.ScriptTimeout = 6000;//300 sec = 5 min
    //    string Report = string.Empty;
    //    string What = string.Empty;
    //    string StateCode = string.Empty;
    //    string ElectionKey = string.Empty;

    //    if (!SecurePage.IsMasterUser)
    //      SecurePage.HandleSecurityException();

    //    try
    //    {
    //      //LabelReport.Visible = false; //old reports
    //      LabelReport.Text = string.Empty;

    //      #region QueryStrings for Report, What, StateCode, ElectionKey
    //      if (!string.IsNullOrEmpty(QueryReport))
    //        Report = QueryReport;

    //      if (!string.IsNullOrEmpty(GetQueryString("What")))//Report What
    //        What = GetQueryString("What");

    //      if (!String.IsNullOrEmpty(QueryState))
    //        StateCode = QueryState;
    //      else if (!string.IsNullOrEmpty(db.StateCode_Domain_This()))//State
    //        StateCode = db.State_Code();

    //      if (!string.IsNullOrEmpty(VotePage.QueryElection))//State
    //        ElectionKey = VotePage.QueryElection;
    //      #endregion QueryStrings for Report, What, StateCode, ElectionKey

    //      DataTable ElectedTable = null;
    //      switch (Report)
    //      {
    //        case "WinnersStatus":
    //          switch (What)
    //          {
    //            case "States":
    //              WinnersStatus();
    //              break;
    //          }
    //          break;
    //        case "Offices":
    //          //Offices(StateCode);
    //          switch (What)
    //          {
    //            case "All":
    //              ReportOffices(StateCode, "All");
    //              break;
    //            case "State":
    //              ReportOffices(StateCode, "State");
    //              break;
    //            case "Counties":
    //              ReportOffices(StateCode, "Counties");
    //              break;
    //            case "Locals":
    //              ReportOffices(StateCode, "Locals");
    //              break;
    //          }
    //          break;
    //        case "FutureElectionsAll":
    //          FutureElectionsAll();
    //          break;
    //        case "FuturePresidentialPrimaries":
    //          FuturePresidentialPrimaries();
    //          break;
    //        case "FutureStatePrimaries":
    //          FutureStatePrimaries();
    //          break;
    //        case "FutureGeneralElections":
    //          FutureGeneralElections();
    //          break;
    //        case "FutureSpecialElections":
    //          FutureSpecialElections();
    //          break;
    //        case "ReportStatus":
    //          Report_Status(What);
    //          break;
    //        case "ElectionNames":
    //          // Admin/Election.aspx
    //          ElectionNames(ElectionKey);
    //          break;
    //        case "OfficialsNames":
    //          // Admin/Default.aspx
    //          OfficialsNames(StateCode);
    //          break;
    //        //case "PrimaryElections":
    //        //  PrimaryElections();
    //        //  break;
    //        case "PoliticianData":
    //          switch (StateCode)
    //          {
    //            case "U1P":
    //              string SQL = string.Empty;
    //              SQL += " SELECT ";
    //              SQL += " Elections.ElectionKey ";
    //              //SQL += " ,Elections.StateCode ";
    //              //SQL += " ,Elections.ElectionDate ";
    //              //SQL += " ,Elections.ElectionDesc ";
    //              //SQL += " ,Elections.IsViewable ";
    //              SQL += " FROM Elections ";
    //              SQL += " WHERE Elections.ElectionDate >= " + db.SQLLit(Db.DbToday);
    //              SQL += " AND Elections.StateCode = 'U1'";
    //              SQL += " AND Elections.PartyCode <> 'ALL' ";
    //              SQL += " ORDER BY Elections.ElectionDate DESC";
    //              //DataTable UpcomingElectionsTable = db.Table(sql.ElectionsUpcomingPrimary());
    //              DataTable UpcomingElectionsTable = db.Table(SQL);
    //              foreach (DataRow UpcomingElectionsRow in UpcomingElectionsTable.Rows)
    //              {
    //                ElectionKey = UpcomingElectionsRow["ElectionKey"].ToString();
    //                ElectedTable = db.Table(sql.ElectionPoliticians(ElectionKey));
    //                PoliticianDataEntriesReport(ElectedTable);
    //              }
    //              break;
    //            case "U2":
    //              ElectedTable = db.Table(sql.ElectedRepresentatives(2));
    //              PoliticianDataEntriesReport(ElectedTable);
    //              break;
    //            case "U3":
    //              ElectedTable = db.Table(sql.ElectedRepresentatives(3));
    //              PoliticianDataEntriesReport(ElectedTable);
    //              break;
    //            case "U4":
    //              ElectedTable = db.Table(sql.ElectedRepresentatives(4));
    //              PoliticianDataEntriesReport(ElectedTable);
    //              break;
    //          }
    //          break;
    //        case "PoliticianAdds":
    //          PoliticianAdds();
    //          break;
    //        //						case "Duplicates":
    //        //							DuplicatePoliticians();
    //        //							break;
    //        case "Errors":
    //          // Master/Default.aspx - Log of Errors
    //          //string Day = db.QueryString("Day");
    //          //Errors(Day);
    //          break;
    //        case "CheckTable_OfficesKeys":
    //          CheckTable_OfficesKeys();
    //          break;
    //        //CheckPoliticiansTableKeys
    //        case "CheckPoliticiansTableKeys":
    //          CheckPoliticiansTableKeys();
    //          break;
    //      }
    //    }

    //    catch (Exception ex)
    //    {
    //      #region
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //      #endregion
    //    }
    //  }
    //}

    #region Dead code

    //private void PrimaryElections()
    //{
    //  string SQL = "TRUNCATE TABLE ElectionsUpcomingTemp";
    //  db.ExecuteSQL(SQL);

    //  #region Build a temporary Table of just Primary Elections
    //  //DataTable ElectionsFutureTable = db.Table(sql.ElectionsFutureStates());
    //  DataTable ElectionsFutureTable = Table_ElectionsFuture();
    //  foreach (DataRow StateRow in ElectionsFutureTable.Rows)
    //  {
    //    #region Build ElectionsUpcomingTemp Table
    //    DateTime DemocraticPrimaryDate = db.States_Date(
    //      StateRow["StateCode"].ToString()
    //      , "DemocraticPrimaryDate");
    //    string DemocraticPrimaryDesc = StateCache.GetStateName(
    //      StateRow["StateCode"].ToString())
    //      + " " + db.States_Str(StateRow["StateCode"].ToString(), "DemocraticPrimaryDesc");

    //    db.ElectionsUpcomingTempInsert(
    //      DemocraticPrimaryDate
    //      , DemocraticPrimaryDesc
    //      , StateRow["StateCode"].ToString()
    //      , "D"
    //      );

    //    DateTime RepublicanPrimaryDate = db.States_Date(
    //      StateRow["StateCode"].ToString()
    //      , "RepublicanPrimaryDate");
    //    string RepublicanPrimaryDesc = StateCache.GetStateName(StateRow["StateCode"].ToString())
    //      + " " + db.States_Str(StateRow["StateCode"].ToString(), "RepublicanPrimaryDesc");

    //    db.ElectionsUpcomingTempInsert(RepublicanPrimaryDate
    //      , RepublicanPrimaryDesc
    //      , StateRow["StateCode"].ToString()
    //      , "R"
    //      );
    //    #endregion
    //  }
    //  #endregion

    //  #region Sort and Display Elections by Date
    //  //LabelReport.Text = string.Empty;
    //  LabelReport.Text = "<br>==============================";
    //  LabelReport.Text += "<br>==============================";
    //  LabelReport.Text += "<br>Primary Elections by Date";
    //  LabelReport.Text += "<br>==============================";
    //  LabelReport.Text += "<br>==============================";
    //  LabelReport.Text += "<br><br>";

    //  DateTime LastDate = Convert.ToDateTime("01/01/1900");
    //  DataTable ElectionsTable = db.Table(sql.ElectionsUpcomingTemp());
    //  foreach (DataRow ElectionsRow in ElectionsTable.Rows)
    //  {
    //    if (LastDate != Convert.ToDateTime(ElectionsRow["ElectionDate"]))
    //      LabelReport.Text += "<br>---------------------------------<br>";
    //    LastDate = Convert.ToDateTime(ElectionsRow["ElectionDate"]);
    //    LabelReport.Text += "<br>";
    //    LabelReport.Text += " " + Convert.ToDateTime(ElectionsRow["ElectionDate"]).ToString("MM/dd/yyyy");
    //    LabelReport.Text += ": " + ElectionsRow["ElectionDesc"].ToString();
    //  }
    //  #endregion
    //  #region Sort and Display Elections by State
    //  //LabelReport.Text = string.Empty;
    //  LabelReport.Text += "<br><br>";
    //  LabelReport.Text += "<br>==============================";
    //  LabelReport.Text += "<br>==============================";
    //  LabelReport.Text += "<br>Primary Elections by State";
    //  LabelReport.Text += "<br>==============================";
    //  LabelReport.Text += "<br>==============================";
    //  LabelReport.Text += "<br><br>";

    //  string LastStateCode = string.Empty;
    //  ElectionsTable = db.Table(sql.ElectionsUpcomingByStateTemp());
    //  foreach (DataRow ElectionsRow in ElectionsTable.Rows)
    //  {
    //    if (LastStateCode != ElectionsRow["StateCode"].ToString())
    //      LabelReport.Text += "<br>---------------------------------<br>";
    //    LastStateCode = ElectionsRow["StateCode"].ToString();
    //    LabelReport.Text += "<br>";
    //    LabelReport.Text += " " + Convert.ToDateTime(ElectionsRow["ElectionDate"]).ToString("MM/dd/yyyy");
    //    LabelReport.Text += ": " + ElectionsRow["ElectionDesc"].ToString();
    //  }
    //  #endregion
    //}

    //private void Governors()
    //{
    //  LabelReport.Text += "<br><br>";
    //  DataTable StatesTable = db.Table(sql.States_51());
    //  foreach (DataRow StateRow in StatesTable.Rows)
    //  {
    //    string GovOfficeKey = StateRow["StateCode"].ToString() + "Governor";
    //    LabelReport.Text += "<br>" + StateRow["StateCode"].ToString() + " - " + GovOfficeKey;
    //    //DataRow Row_Office = db.OfficeRowOptional(GovOfficeKey);
    //    DataRow Row_Office = db.Row_Optional(db.Sql_Row_Office(GovOfficeKey));
    //    if (Row_Office != null)
    //    {
    //      //					LabelReport.Text += "("+Row_Office["OfficeLine1"].ToString() + " " + Row_Office["OfficeLine2"].ToString()+")";
    //    }
    //    else
    //    {
    //      LabelReport.Text += " --  NO OFFICE WITH THIS OfficeKey!";
    //    }
    //  }
    //}
    //private void UpdateOfficesOfficials(DataRow politicianRow1, DataRow politicianRow)
    //{
    //  PageCache tempCache = PageCache.GetTemporary();
    //  string politicianKey1 = politicianRow1["PoliticianKey"].ToString().Trim();
    //  string officeKey1 = tempCache.Politicians.GetOfficeKey(politicianKey1);
    //  string politicianKey = politicianRow["PoliticianKey"].ToString().Trim();
    //  string officeKey = tempCache.Politicians.GetOfficeKey(politicianKey);
    //  string officeKey2Use = string.Empty;
    //  //Politicians LDSLegIDNum, OfficeKey
    //  //if PoliticianKey1 has and LDSLegIDNum copy the LDSLegIDNum and OfficeKey to PoliticianKey
    //  string ldsLegIDNum = politicianRow1["LDSLegIDNum"].ToString().Trim();
    //  if (ldsLegIDNum != string.Empty)
    //  {
    //    db.Politicians_Update_Str(politicianKey, "LDSLegIDNum", ldsLegIDNum);
    //    officeKey2Use = officeKey1;
    //  }
    //  else if (politicianRow1["LDSVersion"].ToString().Trim() == "10.2")//just updated with Version 10.2
    //    officeKey2Use = officeKey1;
    //  //if PoliticianKey1 has a more recent DataLastUpdated use it
    //  else if (Convert.ToDateTime(politicianRow1["DataLastUpdated"].ToString()) > Convert.ToDateTime(politicianRow["DataLastUpdated"].ToString()))
    //    officeKey2Use = officeKey1;
    //  else
    //    officeKey2Use = officeKey;

    //  db.Politicians_Update_Str(politicianKey, "TemporaryOfficeKey", officeKey2Use);


    //  #region OfficesOfficials
    //  string sqlDeleteText = string.Empty;
    //  string sqlUpdateText = string.Empty;

    //  string sqlPoliticianKey1Text = "SELECT OfficeKey,PoliticianKey FROM OfficesOfficials WHERE PoliticianKey = "
    //    + db.SQLLit(politicianKey1);
    //  DataRow officesOfficials1DataRow = db.Row_Optional(sqlPoliticianKey1Text);

    //  string sqlPoliticianKeyText = "SELECT OfficeKey,PoliticianKey FROM OfficesOfficials WHERE PoliticianKey = "
    //    + db.SQLLit(politicianKey);
    //  DataRow officesOfficialsDataRow = db.Row_Optional(sqlPoliticianKeyText);

    //  //If there is an OfficesOfficials row for PoliticianKey and a row for PoliticianKey1
    //  if ((officesOfficials1DataRow != null) && (officesOfficialsDataRow != null))
    //  //	Delete OfficesOfficials row for PoliticianKey1
    //  //	Update the row for PoliticianKey with OfficeKey2Use
    //  {
    //    sqlDeleteText = "DELETE FROM OfficesOfficials WHERE PoliticianKey = "
    //      + db.SQLLit(politicianKey1)
    //      + " AND OfficeKey = " + db.SQLLit(officesOfficials1DataRow["OfficeKey"].ToString());
    //    db.ExecuteSQL(sqlDeleteText);

    //    sqlUpdateText = "UPDATE OfficesOfficials SET OfficeKey = " + db.SQLLit(officeKey2Use)
    //      + " WHERE PoliticianKey = " + db.SQLLit(politicianKey)
    //      + " AND OfficeKey = " + db.SQLLit(officesOfficialsDataRow["OfficeKey"].ToString());
    //    db.ExecuteSQL(sqlUpdateText);
    //  }
    //  else if (officesOfficialsDataRow != null)
    //  //else there is only an OfficesOfficials row for PoliticianKey
    //  //	Update the row for PoliticianKey with OfficeKey2Use
    //  {
    //    sqlUpdateText = "UPDATE OfficesOfficials SET OfficeKey = " + db.SQLLit(officeKey2Use)
    //      + ",PoliticianKey = " + db.SQLLit(politicianKey)
    //      + " WHERE PoliticianKey = " + db.SQLLit(officesOfficialsDataRow["PoliticianKey"].ToString())
    //      + " AND OfficeKey = " + db.SQLLit(officesOfficialsDataRow["OfficeKey"].ToString());
    //    db.ExecuteSQL(sqlUpdateText);
    //  }
    //  else if (officesOfficials1DataRow != null)
    //  //else there is only an OfficesOfficials row for PoliticianKey1
    //  //	Update the row for PoliticianKey with OfficeKey2Use
    //  {
    //    sqlUpdateText = "UPDATE OfficesOfficials SET OfficeKey = " + db.SQLLit(officeKey2Use)
    //      + ",PoliticianKey = " + db.SQLLit(politicianKey)
    //      + " WHERE PoliticianKey = " + db.SQLLit(officesOfficials1DataRow["PoliticianKey"].ToString())
    //      + " AND OfficeKey = " + db.SQLLit(officesOfficials1DataRow["OfficeKey"].ToString());
    //    db.ExecuteSQL(sqlUpdateText);
    //  }
    //  #endregion
    //}

    //private void UpdateOfficesElectionPoliticians(DataRow PoliticianRow1, DataRow PoliticianRow)
    //{
    //  #region ElectionsPoliticians
    //  string SQLUPDATE = string.Empty;
    //  string SQLPoliticianKey = string.Empty;
    //  string SQLPoliticianKey1 = "SELECT ElectionKey,OfficeKey,PoliticianKey FROM ElectionsPoliticians "
    //    + " WHERE PoliticianKey = " + db.SQLLit(PoliticianRow1["PoliticianKey"].ToString());
    //  DataTable ElectionsPoliticians1DataTable = db.Table(SQLPoliticianKey1);
    //  if (ElectionsPoliticians1DataTable.Rows.Count > 0)
    //  {
    //    foreach (DataRow ElectionsPoliticians1Row in ElectionsPoliticians1DataTable.Rows)
    //    {
    //      //Check if there is a row for PoliticianKey with same office and election 
    //      SQLPoliticianKey = "SELECT ElectionKey,OfficeKey,PoliticianKey FROM ElectionsPoliticians "
    //        + " WHERE PoliticianKey = " + db.SQLLit(PoliticianRow["PoliticianKey"].ToString())
    //        + " AND OfficeKey = " + db.SQLLit(ElectionsPoliticians1Row["OfficeKey"].ToString())
    //        + " AND ElectionKey = " + db.SQLLit(ElectionsPoliticians1Row["ElectionKey"].ToString());
    //      DataRow ElectionsPoliticiansDataRow = db.Row_Optional(SQLPoliticianKey);

    //      //If there is no PoliticianKey row make the PoliticianKey1 a PoliticianKey row
    //      //i.e. don't try to insert a duplicate row
    //      if (ElectionsPoliticiansDataRow == null)
    //      {
    //        //Update the PoliticianKey1 row 
    //        SQLUPDATE = "UPDATE ElectionsPoliticians "
    //          + " SET PoliticianKey = " + db.SQLLit(PoliticianRow["PoliticianKey"].ToString())
    //          + " WHERE PoliticianKey = " + db.SQLLit(ElectionsPoliticians1Row["PoliticianKey"].ToString())
    //          + " AND OfficeKey = " + db.SQLLit(ElectionsPoliticians1Row["OfficeKey"].ToString())
    //          + " AND ElectionKey = " + db.SQLLit(ElectionsPoliticians1Row["ElectionKey"].ToString());
    //        db.ExecuteSQL(SQLUPDATE);
    //      }
    //      else//delete the PoliticianKey1 row
    //      {
    //        string SQLDELETE = "DELETE FROM ElectionsPoliticians "
    //          + " WHERE PoliticianKey = " + db.SQLLit(ElectionsPoliticians1Row["PoliticianKey"].ToString())
    //          + " AND OfficeKey = " + db.SQLLit(ElectionsPoliticians1Row["OfficeKey"].ToString())
    //          + " AND ElectionKey = " + db.SQLLit(ElectionsPoliticians1Row["ElectionKey"].ToString());
    //        db.ExecuteSQL(SQLDELETE);
    //      }
    //    }
    //  }
    //  #endregion
    //}
    //private void UpdateAnswers(DataRow PoliticianRow1, DataRow PoliticianRow)
    //{
    //  #region Answers
    //  string SQLUPDATE = string.Empty;
    //  string SQLPoliticianKey = string.Empty;
    //  string SQLPoliticianKey1 = "SELECT PoliticianKey,QuestionKey FROM Answers "
    //    + " WHERE PoliticianKey = " + db.SQLLit(PoliticianRow1["PoliticianKey"].ToString());
    //  DataTable Answers1DataTable = db.Table(SQLPoliticianKey1);

    //  if (Answers1DataTable.Rows.Count > 0)
    //  {
    //    foreach (DataRow Answer1Row in Answers1DataTable.Rows)
    //    {
    //      SQLPoliticianKey = "SELECT PoliticianKey,QuestionKey FROM Answers "
    //        + " WHERE PoliticianKey = " + db.SQLLit(PoliticianRow["PoliticianKey"].ToString())
    //        + " AND QuestionKey = " + db.SQLLit(Answer1Row["QuestionKey"].ToString());
    //      DataRow AnswerDataRow = db.Row_Optional(SQLPoliticianKey);

    //      //don't try to insert a duplicate row
    //      if (AnswerDataRow == null)
    //      {
    //        SQLUPDATE = "UPDATE Answers "
    //          + " SET PoliticianKey = " + db.SQLLit(PoliticianRow["PoliticianKey"].ToString())
    //          + " WHERE PoliticianKey = " + db.SQLLit(Answer1Row["PoliticianKey"].ToString())
    //          + " AND QuestionKey = " + db.SQLLit(Answer1Row["QuestionKey"].ToString());
    //        db.ExecuteSQL(SQLUPDATE);
    //      }
    //    }
    //  }
    //  #endregion
    //}
    //private void xUpdatePoliticiansCommittees(DataRow PoliticianRow1, DataRow PoliticianRow)
    //{
    //  #region LDS PoliticiansCommittees
    //  //string SQLUPDATE = string.Empty;
    //  //string SQLPoliticianKey1 = "SELECT PoliticianKey FROM PoliticiansCommittees "
    //  //  + " WHERE PoliticianKey = " + db.SQLLit(PoliticianRow1["PoliticianKey"].ToString());
    //  //DataTable PoliticiansCommittees1DataTable = db.Table(SQLPoliticianKey1);

    //  //if (PoliticiansCommittees1DataTable.Rows.Count > 0)
    //  //{
    //  //  SQLUPDATE = "UPDATE PoliticiansCommittees "
    //  //    + " SET PoliticianKey = " + db.SQLLit(PoliticianRow["PoliticianKey"].ToString())
    //  //    + " WHERE PoliticianKey = " + db.SQLLit(PoliticianRow1["PoliticianKey"].ToString());
    //  //  db.ExecuteSQL(SQLUPDATE);
    //  //}
    //  #endregion LDS PoliticiansCommittees
    //}
    //private void UpdatePoliticianData(DataRow PoliticianRow1, DataRow PoliticianRow, string Column)
    //{
    //  string PoliticianKey = PoliticianRow["PoliticianKey"].ToString().Trim();
    //  //PoliticianKey1 has a value PoliticianKey does not
    //  if (
    //    (PoliticianRow1[Column].ToString().Trim() != string.Empty)
    //    && (PoliticianRow[Column].ToString().Trim() == string.Empty)
    //    )
    //  {
    //    db.Politicians_Update_Str(
    //      PoliticianKey
    //      , Column
    //      , PoliticianRow1[Column].ToString().Trim());
    //  }

    //  //PoliticianKey1 and PoliticianKey both have values
    //  if (
    //    (PoliticianRow1[Column].ToString().Trim() != string.Empty)
    //    && (PoliticianRow[Column].ToString().Trim() != string.Empty)
    //    )
    //  {
    //    //if PoliticianKey1 has and LDSLegIDNum use it
    //    if (PoliticianRow1["LDSLegIDNum"].ToString().Trim() != string.Empty)
    //      db.Politicians_Update_Str(
    //        PoliticianKey
    //        , Column
    //        , PoliticianRow1[Column].ToString().Trim());
    //    //if PoliticianKey1 has a more recent DataLastUpdated use it
    //    else if (Convert.ToDateTime(PoliticianRow1["DataLastUpdated"].ToString()) > Convert.ToDateTime(PoliticianRow["DataLastUpdated"].ToString()))
    //      db.Politicians_Update_Str(
    //        PoliticianKey
    //        , Column
    //        , PoliticianRow1[Column].ToString().Trim());
    //  }
    //}
    //    private void DuplicatePoliticians()
    //    {

    //      #region SQL
    //      string SQL = string.Empty;
    //      SQL += " SELECT ";
    //      SQL += " Politicians.PoliticianKey ";
    //      SQL += " ,Politicians.TemporaryOfficeKey ";
    //      SQL += " ,Politicians.AddOn ";
    //      SQL += " ,Politicians.EmailAddrVoteUSA ";
    //      SQL += " ,Politicians.EmailAddr ";
    //      SQL += " ,Politicians.StateEmailAddr ";
    //      SQL += " ,Politicians.LastEmailCode ";
    //      SQL += " ,Politicians.WebAddr ";
    //      SQL += " ,Politicians.StateWebAddr ";
    //      SQL += " ,Politicians.Phone ";
    //      SQL += " ,Politicians.StatePhone ";
    //      SQL += " ,Politicians.Gender ";
    //      SQL += " ,Politicians.PartyKey ";
    //      SQL += " ,Politicians.Address ";
    //      SQL += " ,Politicians.CityStateZip ";
    //      SQL += " ,Politicians.StateAddress ";
    //      SQL += " ,Politicians.StateCityStateZip ";
    //      SQL += " ,Politicians.CampaignAddr ";
    //      SQL += " ,Politicians.CampaignPhone ";
    //      SQL += " ,Politicians.StateData ";
    //      SQL += " ,Politicians.LDSStateCode ";
    //      SQL += " ,Politicians.LDSTypeCode ";
    //      SQL += " ,Politicians.LDSDistrictCode ";
    //      SQL += " ,Politicians.LDSLegIDNum ";
    //      SQL += " ,Politicians.LDSPoliticianName ";
    //      SQL += " ,Politicians.LDSEmailAddr ";
    //      SQL += " ,Politicians.LDSWebAddr ";
    //      SQL += " ,Politicians.LDSPhone ";
    //      SQL += " ,Politicians.LDSGender ";
    //      SQL += " ,Politicians.LDSPartyCode ";
    //      SQL += " ,Politicians.LDSAddress ";
    //      SQL += " ,Politicians.LDSCityStateZip ";
    //      SQL += " ,Politicians.LDSVersion ";
    //      SQL += " ,Politicians.GeneralStatement ";
    //      SQL += " ,Politicians.Personal ";
    //      SQL += " ,Politicians.Education ";
    //      SQL += " ,Politicians.Profession ";
    //      SQL += " ,Politicians.Military ";
    //      SQL += " ,Politicians.Civic ";
    //      SQL += " ,Politicians.Political ";
    //      SQL += " ,Politicians.Religion ";
    //      SQL += " ,Politicians.Accomplishments ";
    //      SQL += " ,Politicians.DataLastUpdated ";
    //      SQL += " FROM Politicians ";
    //      #endregion

    //      string SQLTable = SQL + " ORDER BY PoliticianKey";

    //      DataTable PoliticiansTable = db.Table(SQLTable);
    //      int i = 0;
    //      LabelReport.Text += "<br><br>";
    //      foreach (DataRow PoliticianRow1 in PoliticiansTable.Rows)
    //      {
    //        string PoliticianKey1 = PoliticianRow1["PoliticianKey"].ToString().Trim();
    //        string PoliticianKey = string.Empty;
    //        int len = PoliticianKey1.Length;
    //        string LastChar = PoliticianKey1.Substring(len - 1, 1);
    //        if ((LastChar == "1") || (LastChar == "2"))
    //        {
    //          #region Report Dup
    //          i++;
    //          string LastUpdated = string.Empty;
    //          if (PoliticianRow1["DataLastUpdated"].ToString() != "1/1/1900 12:00:00 AM")
    //          {
    //            DateTime UpdatedDate = Convert.ToDateTime(PoliticianRow1["DataLastUpdated"].ToString());
    //            LastUpdated = UpdatedDate.Year.ToString();
    //          }

    //          PoliticianKey = PoliticianKey1.Substring(0, len - 1);

    //          string SQLRow = SQL + " WHERE  PoliticianKey = " + db.SQLLit(PoliticianKey);

    //          DataRow PoliticianRow = db.Row_Optional(SQLRow);
    //          string DupLastUpdated = string.Empty;
    //          if (PoliticianRow != null)
    //          {
    //            if (PoliticianRow["DataLastUpdated"].ToString() != "1/1/1900 12:00:00 AM")
    //            {
    //              //							DupLastUpdated = PoliticianRow["DataLastUpdated"].ToString();
    //              DateTime DupUpdatedDate = Convert.ToDateTime(PoliticianRow["DataLastUpdated"].ToString());
    //              DupLastUpdated = DupUpdatedDate.Year.ToString();
    //            }
    //          }
    //          else
    //          {
    //            DupLastUpdated = "--- NO DUPLICATE POLITICIAN ROW -----";
    //          }
    //          LabelReport.Text += "<br>"
    //            + PoliticianKey1
    //            + " (" + LastUpdated + ") "
    //            + PoliticianKey
    //            + " (" + DupLastUpdated + ") ";
    //          if (LastChar == "2")
    //            LabelReport.Text += "****2****";

    //          if ((PoliticianRow1["LDSLegIDNum"].ToString().Trim() != string.Empty)
    //            && (PoliticianRow["LDSLegIDNum"].ToString().Trim() != string.Empty))
    //          {
    //            LabelReport.Text += "-------" + PoliticianRow1["LDSLegIDNum"].ToString()
    //              + " | " + PoliticianRow["LDSLegIDNum"].ToString();
    //          }
    //          #endregion

    //          UpdateOfficesOfficials(PoliticianRow1, PoliticianRow);
    //          UpdateOfficesElectionPoliticians(PoliticianRow1, PoliticianRow);
    //          UpdateAnswers(PoliticianRow1, PoliticianRow);
    //          //xUpdatePoliticiansCommittees(PoliticianRow1, PoliticianRow);

    //          #region Consolidate Politician Rows
    //#if false
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "AddOn");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "EmailAddrVoteUSA");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "EmailAddr");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "StateEmailAddr");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LastEmailCode");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "WebAddr");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "StateWebAddr");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Phone");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "StatePhone");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Gender");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "PartyKey");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Address");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "CityStateZip");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "StateAddress");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "StateCityStateZip");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "CampaignAddr");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "CampaignPhone");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "StateData");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSStateCode");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSTypeCode");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSDistrictCode");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSLegIDNum");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSPoliticianName");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSEmailAddr");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSWebAddr");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSPhone");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSGender");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSPartyCode");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSAddress");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSCityStateZip");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "LDSVersion");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "GeneralStatement");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Personal");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Education");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Profession");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Military");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Civic");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Political");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Religion");
    //            UpdatePoliticianData(PoliticianRow1, PoliticianRow, "Accomplishments");
    //#endif
    //          #endregion

    //          string SQLDELETE = "DELETE FROM Politicians WHERE PoliticianKey = "
    //            + db.SQLLit(PoliticianRow1["PoliticianKey"].ToString());
    //          db.ExecuteSQL(SQLDELETE);
    //          string test3 = string.Empty;

    //        }
    //      }
    //      LabelReport.Text += "<br><br>Duplicates: " + i.ToString();
    //      Msg.Text = db.Ok("Done");
    //    }
    //

    #endregion Dead code


  }
}
