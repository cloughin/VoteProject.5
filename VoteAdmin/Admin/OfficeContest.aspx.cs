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
using DB.Vote;

namespace Vote.Admin
{
  public partial class OfficeContests : VotePage
  {
    //#region Checks
    //private void Check_TextBoxes_Not_Illegal()
    //{
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxFirstAdd);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxMiddleAdd);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxNickNameAdd);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxLastAdd);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxSuffixAdd);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAddOnAdd);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxOrder);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextboxRunningMate);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxLastName);
    //}

    //#endregion Checks

    //#region sql

    //protected void ElectionsPoliticians_INSERT()
    //{
    //  if (db.Is_Valid_ElectionsPoliticians(
    //    ViewState["ElectionKey"].ToString(),
    //    ViewState["OfficeKey"].ToString(),
    //    ViewState["PoliticianKey"].ToString()))
    //    throw new ApplicationException("The candidate is already in the election.");

    //  db.ElectionsPoliticians_INSERT_And_Whether_Incumbent(
    //    ViewState["ElectionKey"].ToString()
    //    , ViewState["OfficeKey"].ToString()
    //    , ViewState["PoliticianKey"].ToString()
    //    );
    //}

    //protected string BallotOrder_Get()
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " ElectionsPoliticians.OrderOnBallot ";
    //  SQL += " FROM ElectionsPoliticians";
    //  SQL += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //  SQL += " AND OfficeKey = " + db.SQLLit(ViewState["OfficeKey"].ToString());
    //  SQL += " AND PoliticianKey = " + db.SQLLit(ViewState["PoliticianKey"].ToString());
    //  DataRow ElectionPoliticianRow = db.Row_Optional(SQL);
    //  if (ElectionPoliticianRow != null)
    //    return ElectionPoliticianRow["OrderOnBallot"].ToString().Trim();
    //  else
    //    return "10";
    //}

    //protected void Resequence_BallotOrder()
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " ElectionsPoliticians.OrderOnBallot ";
    //  SQL += ",ElectionsPoliticians.PoliticianKey ";
    //  SQL += " FROM ElectionsPoliticians";
    //  SQL += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //  SQL += " AND OfficeKey = " + db.SQLLit(ViewState["OfficeKey"].ToString());
    //  SQL += " ORDER BY OrderOnBallot";
    //  DataTable tableElectionsPoliticians = db.Table(SQL);
    //  int Order = 10;
    //  foreach (DataRow rowElectionsPoliticians in tableElectionsPoliticians.Rows)
    //  {
    //    db.ElectionsPoliticians_Update_Int(
    //      ViewState["ElectionKey"].ToString(),
    //      ViewState["OfficeKey"].ToString(),
    //      rowElectionsPoliticians["PoliticianKey"].ToString(),
    //      "OrderOnBallot",
    //      Order
    //      );
    //    Order = Order + 10;
    //  }
    //}
    //#endregion sql

    //#region Candidates Report

    //protected void Election_Contest_Office_Politician(
    //  PageCache cache,
    //  ref HtmlTable htmlTable,
    //  string officeKey,
    //  string politicianKey,
    //  bool isRunningMate,
    //  string electionKey,
    //  string ballotOrder)
    //{
    //  string anchorName = string.Empty;
    //  string anchorHeadshot = string.Empty;

    //  HtmlTableRow Html_Tr_Politician =
    //    db.Add_Tr_To_Table_Return_Tr(
    //      htmlTable
    //      , "trReportDetail"
    //      );

    //  #region Ballot Order or Politician Pic
    //  if (isRunningMate)
    //    ballotOrder = "&nbsp";//need so td cell will have borders

    //  db.Add_Td_To_Tr(
    //      Html_Tr_Politician
    //      , ballotOrder
    //      , "tdReportDetail"
    //      );

    //  #endregion Ballot Order or Politician Pic

    //  #region Politician Headshot and Name Anchor Td

    //  string politicianName = cache.Politicians.GetPoliticianName(
    //    politicianKey, true);

    //  #region * Incumbent indication
    //  //Running mate doesn't have an order or * incumbent indication
    //  if (!isRunningMate)
    //  {
    //    if (db.Is_Election_Upcoming(electionKey))
    //    {
    //      //Look at the currently elected officials to see if incumbent
    //      if (db.Is_Incumbent(politicianKey, officeKey))
    //        politicianName = "* " + politicianName;
    //    }
    //    else
    //    {
    //      //Look at the incumbent flag for the election
    //      //because looking at currently elected officials
    //      //for old elections is invalid.
    //      if (db.Is_Incumbent_For_Election(politicianKey, officeKey, electionKey))
    //        politicianName = "* " + politicianName;
    //    }
    //  }
    //  #endregion * Incumbent indication

    //  #region Name Anchor

    //  //Has 2 anchors: Headshot of politician and name
    //  #region Headshot anchor
    //  anchorHeadshot = db.AnchorPoliticianImageOrNoPhoto(
    //    db.Url_Politician_Intro(politicianKey),
    //    politicianKey,
    //    db.Image_Size_35_Headshot,
    //    cache.Politicians.GetPoliticianName(politicianKey) +
    //      " Intro Page Data Entry",
    //    "_intro"
    //    );
    //  #endregion Headshot anchor

    //  #region 2nd Name anchor
    //  //anchorName += db.Anchor_Admin_OfficeContest(
    //  //    politicianKey
    //  //    , officeKey
    //  //    , electionKey
    //  //    , politicianName
    //  //    , "_officecontest"
    //  //    , "EditCandidate"
    //  //    );
    //  anchorName += db.Anchor_Admin_OfficeContest(
    //      electionKey
    //      , officeKey
    //      , politicianName
    //      , "_officecontest"
    //      , politicianKey
    //      , "EditCandidate"
    //      );
    //  #endregion 2nd Name anchor

    //  if (isRunningMate)
    //    anchorName += "<br><small><small>(Running Mate)</small></small>";

    //  //if (!isRunningMate)
    //  ////winner indications for running mates are not shown on reports
    //  //{
    //  //  if (db.Is_Winner(
    //  //     electionKey,
    //  //     officeKey,
    //  //     politicianKey
    //  //    )
    //  //    )
    //  //  {
    //  //    anchorName += "<br><small><small><span style=color:red>(winner)</span></small></small>";
    //  //  }
    //  //}
    //  db.Add_Td_Politician_Name(
    //    ref Html_Tr_Politician
    //    , db.ReportUser.Admin
    //    , anchorHeadshot + " " + anchorName
    //    );
    //  #endregion Name Anchor

    //  #endregion Politician Name Anchor Td

    //  db.Add_Td_Party_Code(cache, ref Html_Tr_Politician, politicianKey, isRunningMate);

    //  db.Add_Td_Phone(ref Html_Tr_Politician, politicianKey, db.ReportUser.Admin);

    //  db.Add_Td_Address(ref Html_Tr_Politician, politicianKey, db.ReportUser.Admin);

    //  db.Add_Td_Email_WebAddress(ref Html_Tr_Politician, politicianKey, db.ReportUser.Admin);
    //}

    //protected void Election_Contest_Office(
    //  PageCache cache,
    //  ref HtmlTable htmlTable,
    //  string stateCode,
    //  string electionKey,
    //  string officeKey
    //  )
    //{
    //  int reportColumns = 6;

    //  #region sql_ElectionsPoliticians - Politicians for Office Contest
    //  string sql_ElectionsPoliticians = string.Empty;
    //  sql_ElectionsPoliticians = " SELECT ";
    //  sql_ElectionsPoliticians += " ElectionsPoliticians.PoliticianKey";
    //  sql_ElectionsPoliticians += ",ElectionsPoliticians.RunningMateKey";
    //  sql_ElectionsPoliticians += ",ElectionsPoliticians.OrderOnBallot";
    //  sql_ElectionsPoliticians += " FROM ElectionsPoliticians";
    //  sql_ElectionsPoliticians += " WHERE ElectionsPoliticians.OfficeKey = " + db.SQLLit(officeKey);
    //  if (!Elections.IsUSPresidentialPrimary(electionKey))
    //    sql_ElectionsPoliticians += " AND ElectionsPoliticians.StateCode = " + db.SQLLit(stateCode);

    //  switch (db.Electoral_Class_Election(electionKey))
    //  {
    //    case db.ElectoralClass.USPresident:
    //      if (!Elections.IsUSPresidentialPrimary(electionKey))
    //      {
    //        sql_ElectionsPoliticians += " AND ElectionsPoliticians.ElectionKeyFederal = " + db.SQLLit(electionKey);
    //        //sql_ElectionsPoliticians += " AND ElectionsPoliticians.OfficeKey = 'USPresident'";
    //      }
    //      else
    //      {
    //        sql_ElectionsPoliticians += " AND ElectionsPoliticians.ElectionKey = " + db.SQLLit(electionKey);
    //      }
    //      break;

    //    case db.ElectoralClass.USSenate:
    //      sql_ElectionsPoliticians += " AND ElectionsPoliticians.ElectionKeyFederal = " + db.SQLLit(electionKey);
    //      break;

    //    case db.ElectoralClass.USHouse:
    //      sql_ElectionsPoliticians += " AND ElectionsPoliticians.ElectionKeyFederal = " + db.SQLLit(electionKey);
    //      break;

    //    case db.ElectoralClass.USGovernors:
    //      sql_ElectionsPoliticians += " AND ElectionsPoliticians.ElectionKeyFederal = " + db.SQLLit(electionKey);
    //      break;

    //    case db.ElectoralClass.State:
    //      sql_ElectionsPoliticians += " AND ElectionsPoliticians.ElectionKey = " + db.SQLLit(electionKey);
    //      //sql_ElectionsPoliticians += " AND ElectionsPoliticians.CountyCode = ''";
    //      break;

    //    case db.ElectoralClass.County:
    //      sql_ElectionsPoliticians += " AND ElectionsPoliticians.ElectionKey = " + db.SQLLit(electionKey);
    //      break;

    //    case db.ElectoralClass.Local:
    //      sql_ElectionsPoliticians += " AND ElectionsPoliticians.ElectionKey = " + db.SQLLit(electionKey);
    //      break;
    //  }
    //  sql_ElectionsPoliticians += " ORDER BY ElectionsPoliticians.OrderOnBallot";
    //  //sql_ElectionsPoliticians += ",Politicians.LName";
    //  //sql_ElectionsPoliticians += ",Politicians.FName";
    //  sql_ElectionsPoliticians += ",ElectionsPoliticians.PoliticianKey";
    //  #endregion sql_ElectionsPoliticians

    //  #region Note
    //  //Public report omit any office contest where there are no candidates
    //  #endregion Note
    //  DataTable electionsPoliticiansTable = db.Table(sql_ElectionsPoliticians);

    //  #region OfficeHeading
    //  string officeHeading = string.Empty;

    //  #region Candidates for Office Title
    //  if (Elections.IsUSPresidentialPrimary(electionKey))
    //    officeHeading = "U.S. Presidential Candidates";

    //  //if (db.Is_StateCode_Federal(db.StateCode_Or_FederalCode_In_ElectionKey(ElectionKey)))
    //  //else if (db.Is_StateCode_Federal(db.StateCode_Or_FederalCode_In_ElectionKey(electionKey)))
    //  //  officeHeading += "Candidates for " + db.Name_Electoral_And_Contest(stateCode, officeKey);
    //  ////else
    //  //else
    //  officeHeading += "Candidates for"
    //    + " " + StateCache.GetShortName(stateCode)
    //    + " " + Offices.FormatOfficeName(officeKey, "<br />");
    //  #endregion Candidates for Office Title

    //  #region Add Party Name for Primaries
    //  if (
    //    (db.Elections_Str(electionKey, "ElectionType") == "A")//National Presidental Primary 
    //    || (db.Elections_Str(electionKey, "ElectionType") == "B")//State Presidental Primary 
    //    || (db.Elections_Str(electionKey, "ElectionType") == "P")//State Primary 
    //    )
    //  {
    //    if (db.Elections_Str(electionKey, "NationalPartyCode") == "D")
    //      officeHeading += " Democratic Party";
    //    else if (db.Elections_Str(electionKey, "NationalPartyCode") == "R")
    //      officeHeading += " Republican Party";
    //    else if (db.Elections_Str(electionKey, "NationalPartyCode") == "G")
    //      officeHeading += " Green Party";
    //    else if (db.Elections_Str(electionKey, "NationalPartyCode") == "L")
    //      officeHeading += " Libertarian Party";
    //  }
    //  #endregion Add Party Name for Primaries

    //  #region Add HTML Office Heading Row

    //  HtmlTableRow htmlTr_Office_Title = db.Add_Tr_To_Table_Return_Tr(
    //    htmlTable
    //    , "trReportGroupHeading");
    //  db.Add_Td_To_Tr(
    //    htmlTr_Office_Title
    //    , officeHeading
    //    , "tdReportGroupHeading"
    //    , "center"
    //    //, db.Columns_Report_Election(db.ReportUser.Admin, electionKey)
    //    ,reportColumns
    //    );
    //  #endregion Add HTML Office Heading Row

    //  #endregion OfficeHeading

    //  #region * = Incumbent ROW should be removed for old elections)
    //  if (db.Columns_Report_Election(db.ReportUser.Admin, electionKey) == db.Columns_With_BallotOrder_Or_Pic)
    //  {
    //    HtmlTableRow keyHTMLRow = db.Add_Tr_To_Table_Return_Tr(
    //      htmlTable
    //      , "trReportIncumbentKeyHeading");
    //    db.Add_Td_To_Tr(
    //      keyHTMLRow
    //      , "* = Incumbent"
    //      , "tdReportIncumbentKeyHeading"
    //      , reportColumns
    //      //, db.Columns_Report_Election(db.ReportUser.Admin, electionKey)
    //      );

    //    //db.Add_Td_To_Tr(ref KeyHTMLRow, "Order", "tdReportDetailHeading");
    //  }
    //  #endregion

    //  #region Order  Name    Party Code   Phone   City, State Zip Web Address ROW
    //  HtmlTableRow candidateHeadingHTMLRow =
    //    db.Add_Tr_To_Table_Return_Tr(
    //      htmlTable
    //      , "trReportDetailHeading"
    //      );

    //  //Order
    //  db.Add_Td_To_Tr(
    //    candidateHeadingHTMLRow
    //    //, "Ballot&nbsp;<br>Order&nbsp;&nbsp;"
    //    , "Order"
    //    , "tdReportDetailHeading"
    //    );
    //  //Name
    //  db.Add_Td_To_Tr(
    //    candidateHeadingHTMLRow
    //    //, "Name"
    //    , "Candidate"
    //    , "tdReportDetailHeading"
    //    );
    //  //Party Code
    //  db.Add_Td_To_Tr(
    //    candidateHeadingHTMLRow
    //    , "Party<br>Code"
    //    , "tdReportDetailHeading"
    //    );
    //  //Phone
    //  db.Add_Td_To_Tr(
    //    candidateHeadingHTMLRow
    //    , "Phone"
    //    , "tdReportDetailHeading"
    //    );
    //  //Street Address
    //  db.Add_Td_To_Tr(
    //    candidateHeadingHTMLRow
    //    , "Street Address<br><nobr>City, State Zip</nobr>"
    //    , "tdReportDetailHeading"
    //    );
    //  //Email Address
    //  db.Add_Td_To_Tr(
    //    candidateHeadingHTMLRow
    //    , "Email Address<br>Web Address"
    //    , "tdReportDetailHeading"
    //    );
    //  #endregion Name   Ballot Order   Party Code   Phone   City, State Zip Web Address

    //  #region Candidates for the One Office

    //  if (electionsPoliticiansTable.Rows.Count > 0)
    //  {
    //    foreach (DataRow rowElectionPolitician in electionsPoliticiansTable.Rows)
    //    {
    //      #region Report 1 candidate
    //      Election_Contest_Office_Politician(
    //        cache,
    //        ref htmlTable,
    //        officeKey,
    //        rowElectionPolitician["PoliticianKey"].ToString(),
    //        false, //false = candidate not running mate
    //        //Forces ElectionKey to have a real StateCode not U1,U2,U3 which is case for Federal elections
    //        db.ElectionKey_State_From_ElectionKey_Federal(electionKey, stateCode), //Can be State, county or local
    //        rowElectionPolitician["OrderOnBallot"].ToString());
    //      #endregion Report 1 candidate

    //      #region If running mate office, Report Running Mate or N/A
    //      if (db.Offices_Bool(officeKey, "IsRunningMateOffice"))
    //      {
    //        if (Politicians.IsValid(rowElectionPolitician["RunningMateKey"].ToString()))
    //        {
    //          Election_Contest_Office_Politician(
    //            cache,
    //            ref htmlTable,
    //            officeKey,
    //            rowElectionPolitician["RunningMateKey"].ToString(),
    //            true, //true = running mate
    //            electionKey,
    //            rowElectionPolitician["OrderOnBallot"].ToString());
    //        }
    //      }
    //      //else
    //      //{
    //      //  var row = db.Add_Tr_To_Table_Return_Tr(htmlTable, "trReportDetail");
    //      //  db.Add_Td_To_Tr(row, "&nbsp;", "tdReportDetail");
    //      //}
    //      #endregion If running mate office, Report Running Mate or N/A
    //    }
    //  }

    //  #endregion Candidates for the One Office

    //}

    //protected string Report_Candidates_Office(
    //  PageCache cache,
    //  string electionKey,
    //  string officeKey)
    //{
    //  // Note: StateCode can't be passed because StateCode could be U1,U2,U3 in Forms for Federal elections

    //  #region Create HTML Candidates for Office report and set attributes
    //  HtmlTable html_Table_Office_Contest = new System.Web.UI.HtmlControls.HtmlTable();
    //  html_Table_Office_Contest.Attributes["cellSpacing"] = "0";
    //  html_Table_Office_Contest.Attributes["align"] = "left";
    //  html_Table_Office_Contest.Attributes["border"] = "0";
    //  //Html_Table_Office_Contest.Attributes["width"] = "100%";
    //  html_Table_Office_Contest.Attributes["Class"] = "tableAdmin";
    //  #endregion Create HTML Candidates for Office report attributes

    //  #region commented SQL
    //  //string sqlText = string.Empty;
    //  //sqlText += " SELECT ";
    //  //sqlText += " ElectionsOffices.ElectionKey ";
    //  //sqlText += " ,Offices.OfficeKey ";
    //  //sqlText += " ,Offices.StateCode ";
    //  //sqlText += " ,Offices.OfficeLine1 ";
    //  //sqlText += " ,Offices.OfficeLine2 ";
    //  //sqlText += " ,Offices.OfficeLevel ";
    //  //sqlText += " ,Offices.IsRunningMateOffice ";
    //  //sqlText += " FROM ElectionsOffices,Offices ";
    //  //sqlText += " WHERE ElectionsOffices.OfficeKey = Offices.OfficeKey";
    //  //sqlText += " AND ElectionsOffices.ElectionKey = " + db.SQLLit(electionKey);
    //  //sqlText += " AND Offices.OfficeKey = " + db.SQLLit(officeKey);
    //  //sqlText += " ORDER BY Offices.OfficeOrderWithinLevel, Offices.DistrictCode, Offices.DistrictCodeAlpha";
    //  ////DataRow OfficeRow = db.Row_Optional(sql.Elections_Offices_Text_File(
    //  ////  ElectionKey
    //  ////  , OfficeKey));
    //  //DataRow officeRow = db.Row_Optional(sqlText);
    //  //if (officeRow != null)
    //  //{
    //  #endregion SQL

    //  if (db.Is_Valid_Election_Office(electionKey, officeKey))
    //  {
    //    Election_Contest_Office(
    //      cache,
    //      ref html_Table_Office_Contest,
    //      // StateCode can't be passed because StateCode could be U1,U2,U3 in Forms for Federal elections
    //      Offices.GetStateCodeFromKey(electionKey),
    //      electionKey,
    //      officeKey
    //      );

    //    return db.RenderToString(html_Table_Office_Contest);
    //  }
    //  else
    //    return "The office is not in the election.";
    //}

    //#endregion Candidates Report

    //#region Same Last Name Report
    //protected PoliticiansTable Table_Same_Last_Name()
    //{
    //  string lastName = ViewState["Name"].ToString();
    //  string stateCode =
    //    Offices.GetStateCodeFromKey(ViewState["OfficeKey"].ToString());
    //  return Politicians.GetDuplicateNamesDataLikeLastNameStateCode(lastName, stateCode);
    //}
    //protected int Rows_Same_Last_Name()
    //{
    //  PoliticiansTable TablePoliticians = Table_Same_Last_Name();
    //  return TablePoliticians.Count;
    //}

    //protected void Report_Same_Last_Name()
    //{
    //  #region Table with Same Last Name
    //  PoliticiansTable SameLastNameTable = Table_Same_Last_Name();
    //  int politicians = SameLastNameTable.Count;

    //  #region Same Last Name Report
    //  //PoliticiansTable SameLastNameTable = Table_Same_Last_Name();
    //  //int politicians = SameLastNameTable.Count;

    //  string politiciansHTMLTable = string.Empty;

    //  if (politicians > 0)
    //  {
    //    #region Report politicians with same last name
    //    LabelCandidateToAdd.Text = "Select Candidate to Add to Office Contest";
    //    tr_SelectCandidate.Visible = true;
    //    tr_SelectCandidateInstruction.Visible = true;

    //    #region Table Init
    //    politiciansHTMLTable += "<table cellspacing=0 cellpadding=0 width=100%>";
    //    #endregion Table Init

    //    #region Heading
    //    politiciansHTMLTable += "<tr>";

    //    politiciansHTMLTable += "<td align=center class=tdReportDetailHeading>";
    //    politiciansHTMLTable += "Politician (Party)";
    //    politiciansHTMLTable += "</td>";

    //    politiciansHTMLTable += "<td align=center class=tdReportDetailHeading>";
    //    politiciansHTMLTable += "Office";
    //    politiciansHTMLTable += "</td>";

    //    politiciansHTMLTable += "<td align=center class=tdReportDetailHeading>";
    //    politiciansHTMLTable += "State Information";
    //    politiciansHTMLTable += "</td>";

    //    politiciansHTMLTable += "<td align=center class=tdReportDetailHeading>";
    //    politiciansHTMLTable += "Politician Information";
    //    politiciansHTMLTable += "</td>";

    //    politiciansHTMLTable += "<td align=center class=tdReportDetailHeading>";
    //    politiciansHTMLTable += "Other";
    //    politiciansHTMLTable += "</td>";

    //    politiciansHTMLTable += "</tr>";
    //    #endregion Heading

    //    foreach (PoliticiansRow SameLastNameRow in SameLastNameTable)
    //    {
    //      int breakAt = 30; //tested on US Pres

    //      string officeKey = PageCache.Politicians
    //        .GetOfficeKey(SameLastNameRow.PoliticianKey);

    //      #region Politician with same last name
    //      politiciansHTMLTable += "<tr>";

    //      #region Politician <td>
    //      politiciansHTMLTable += "<td valign=top class=tdReportDetail>";

    //      #region HeadShot Anchor
    //      string HeadShot = string.Empty;
    //      HeadShot += db.AnchorPoliticianImageOrNoPhoto(
    //          db.Url_Politician_Intro(SameLastNameRow.PoliticianKey)
    //        , SameLastNameRow.PoliticianKey
    //        , db.Image_Size_25_Headshot
    //        , Politicians.GetFormattedName(SameLastNameRow.PoliticianKey) + " Intro Page Data Entry"
    //        , "_intro"
    //        );

    //      politiciansHTMLTable += HeadShot;
    //      politiciansHTMLTable += "&nbsp";
    //      #endregion HeadShot Anchor

    //      #region Name Anchor
    //      //politiciansHTMLTable += db.Anchor_Admin_OfficeContest(
    //      //  SameLastNameRow.PoliticianKey,
    //      //  ViewState["OfficeKey"].ToString(),
    //      //  ViewState["ElectionKey"].ToString(),
    //      //  db.GetPoliticianName(SameLastNameRow.PoliticianKey)
    //      //  , "_officecontest"
    //      //  , "AddCandidate"
    //      //);
    //      politiciansHTMLTable += db.Anchor_Admin_OfficeContest(
    //        ViewState["ElectionKey"].ToString(),
    //        ViewState["OfficeKey"].ToString(),
    //        Politicians.GetFormattedName(SameLastNameRow.PoliticianKey),
    //        "_officecontest",
    //        SameLastNameRow.PoliticianKey,
    //        "AddCandidate"
    //      );

    //      #endregion Name Anchor

    //      #region Party
    //      if (SameLastNameRow.PartyKey != string.Empty)
    //        politiciansHTMLTable += " (" + db.Parties_Str(SameLastNameRow.PartyKey, "PartyCode") + ")";
    //      #endregion Party

    //      #region PoliticianKey
    //      politiciansHTMLTable += "<br>(" + SameLastNameRow.PoliticianKey + ")";
    //      #endregion PoliticianKey

    //      politiciansHTMLTable += "</td>";
    //      #endregion Politician <td>

    //      #region Office <td>
    //      politiciansHTMLTable += "<td valign=top class=tdReportDetail>";

    //      string Office = string.Empty;
    //      if (officeKey != string.Empty)
    //      {
    //        if (Offices.OfficeKeyExists(officeKey))
    //        {
    //          Office += db.Offices_Str(officeKey, "OfficeLine1");
    //          Office += " " + db.Offices_Str(officeKey, "OfficeLine2");
    //          Office = db.Anchor_Admin_Office_UPDATE_Office(
    //            officeKey, Office);
    //        }
    //        else
    //          Office += "No Office Identified for Politician";
    //      }
    //      else
    //        Office += "No Office Identified for Politician";

    //      politiciansHTMLTable += Office;
    //      politiciansHTMLTable += "</td>";
    //      #endregion

    //      #region State Data <td>
    //      politiciansHTMLTable += "<td valign=top class=tdReportDetail>";
    //      //if (SameLastNameRow["StateAddress"].ToString() != string.Empty)
    //      //{
    //      //  politiciansHTMLTable += SameLastNameRow["StateAddress"].ToString() + "<br>";
    //      //  politiciansHTMLTable += SameLastNameRow["StateCityStateZip"].ToString() + "<br>";
    //      //}
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      //if (DbPolitician.Has_Complete_State_Address(SameLastNameRow))
    //      //{
    //      //  politiciansHTMLTable += DbPolitician.Get_State_Address(SameLastNameRow) + "<br>";
    //      //  politiciansHTMLTable += DbPolitician.Get_State_CityStateZip(SameLastNameRow) + "<br>";
    //      //}
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (SameLastNameRow.HasCompleteStateAddress)
    //      {
    //        politiciansHTMLTable += SameLastNameRow.StateAddress + "<br>";
    //        politiciansHTMLTable += SameLastNameRow.StateCityStateZip + "<br>";
    //      }
    //      else
    //        politiciansHTMLTable += "&nbsp;";

    //      //if (SameLastNameRow["StatePhone"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += SameLastNameRow["StatePhone"].ToString() + "<br>";
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.StatePhone))
    //        politiciansHTMLTable += SameLastNameRow.StatePhone + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (SameLastNameRow["StateEmailAddr"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += SameLastNameRow["StateEmailAddr"].ToString() + "<br>";
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.StateEmail))
    //        politiciansHTMLTable += db.Insert_BR(SameLastNameRow.StateEmail, breakAt) + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (SameLastNameRow["StateWebAddr"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += SameLastNameRow["StateWebAddr"].ToString() + "<br>";
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.StateWebAddress))
    //        politiciansHTMLTable += db.Insert_BR(SameLastNameRow.StateWebAddress, breakAt) + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      politiciansHTMLTable += "</td>";
    //      #endregion State Data <td>

    //      #region Politician Data <td>
    //      politiciansHTMLTable += "<td valign=top class=tdReportDetail>";
    //      //if (SameLastNameRow["Address"].ToString() != string.Empty)
    //      //{
    //      //  politiciansHTMLTable += SameLastNameRow["Address"].ToString() + "<br>";
    //      //  politiciansHTMLTable += SameLastNameRow["CityStateZip"].ToString() + "<br>";
    //      //}
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      //if (DbPolitician.Get_Politician_Address(SameLastNameRow) != string.Empty)
    //      //  politiciansHTMLTable += DbPolitician.Get_Politician_Address(SameLastNameRow);
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.Address))
    //        politiciansHTMLTable += SameLastNameRow.Address + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (DbPolitician.Get_Politician_CityStateZip(SameLastNameRow) != string.Empty)
    //      //  politiciansHTMLTable += DbPolitician.Get_Politician_CityStateZip(SameLastNameRow);
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.CityStateZip))
    //        politiciansHTMLTable += SameLastNameRow.CityStateZip + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (SameLastNameRow["Phone"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += SameLastNameRow["Phone"].ToString() + "<br>";
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.Phone))
    //        politiciansHTMLTable += SameLastNameRow.Phone + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (SameLastNameRow["EmailAddr"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += SameLastNameRow["EmailAddr"].ToString() + "<br>";
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.Email))
    //        politiciansHTMLTable += db.Insert_BR(SameLastNameRow.Email, breakAt) + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (SameLastNameRow["WebAddr"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += SameLastNameRow["WebAddr"].ToString() + "<br>";
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.WebAddress))
    //        politiciansHTMLTable += db.Insert_BR(SameLastNameRow.WebAddress, breakAt) + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      politiciansHTMLTable += "</td>";
    //      #endregion Politician Data <td>

    //      #region Other Data <td>
    //      politiciansHTMLTable += "<td valign=top class=tdReportDetail>";
    //      //if (SameLastNameRow["LDSAddress"].ToString() != string.Empty)
    //      //{
    //      //  politiciansHTMLTable += SameLastNameRow["LDSAddress"].ToString() + "<br>";
    //      //  politiciansHTMLTable += SameLastNameRow["LDSCityStateZip"].ToString() + "<br>";
    //      //}
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      //if (DbPolitician.Has_Complete_Lds_Address(SameLastNameRow))
    //      //{
    //      //  politiciansHTMLTable += DbPolitician.Get_Lds_Address(SameLastNameRow) + "<br>";
    //      //  politiciansHTMLTable += DbPolitician.Get_Lds_CityStateZip(SameLastNameRow) + "<br>";
    //      //}
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (SameLastNameRow.HasCompleteLDSAddress)
    //      {
    //        politiciansHTMLTable += SameLastNameRow.LDSAddress + "<br>";
    //        politiciansHTMLTable += SameLastNameRow.LDSCityStateZip + "<br>";
    //      }
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (SameLastNameRow["LDSPhone"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += SameLastNameRow["LDSPhone"].ToString() + "<br>";
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.LDSPhone))
    //        politiciansHTMLTable += SameLastNameRow.LDSPhone + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (SameLastNameRow["LDSEmailAddr"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += SameLastNameRow["LDSEmailAddr"].ToString() + "<br>";
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.LDSEmail))
    //        politiciansHTMLTable += db.Insert_BR(SameLastNameRow.LDSEmail, breakAt) + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      //if (SameLastNameRow["LDSWebAddr"].ToString() != string.Empty)
    //      //  politiciansHTMLTable += SameLastNameRow["LDSWebAddr"].ToString() + "<br>";
    //      //else
    //      //  politiciansHTMLTable += "&nbsp;";
    //      if (!string.IsNullOrEmpty(SameLastNameRow.LDSWebAddress))
    //        politiciansHTMLTable += db.Insert_BR(SameLastNameRow.LDSWebAddress, breakAt) + "<br>";
    //      else
    //        politiciansHTMLTable += "&nbsp;";
    //      politiciansHTMLTable += "</td>";
    //      #endregion

    //      politiciansHTMLTable += "</tr>";
    //      #endregion Other Data <td>
    //    }
    //    #endregion Politician with same last name
    //  }
    //  else
    //  {
    //    #region Report No politicians have the same last name
    //    politiciansHTMLTable += "<tr>";
    //    politiciansHTMLTable += "<td colspan=5 class=tdReportDetail>";
    //    politiciansHTMLTable += "<center>There are no politicians in the State with the same last name in the database.</center>";
    //    politiciansHTMLTable += "</td>";
    //    politiciansHTMLTable += "</tr>";
    //    #endregion Report No politicians have the same last name
    //  }

    //  politiciansHTMLTable += "</table>";

    //  HTMLTableSameLastName.Text = politiciansHTMLTable;
    //  #endregion Report Detail lines

    //  #endregion Table with Same Last Name
    //}

    //#endregion Same Last Name Report

    //private void Page_Title()
    //{
    //  string electionKey = ViewState["ElectionKey"].ToString();
    //  string officeKey = ViewState["OfficeKey"].ToString();

    //  PageTitle.Text = "Office Contest for ";

    //  if (!Elections.IsUSPresidentialPrimary(electionKey))
    //  {
    //    PageTitle.Text += Offices.GetElectoralClassDescription(
    //      ViewState["StateCode"].ToString()
    //    , ViewState["CountyCode"].ToString()
    //    , ViewState["LocalCode"].ToString());

    //    PageTitle.Text += "<br>";
    //    PageTitle.Text += Offices.FormatOfficeName(officeKey, "<br />");
    //  }
    //  PageTitle.Text += "<br>";
    //  PageTitle.Text += db.PageTitle_Election(electionKey);
    //}

    //#region Visible Html Tables

    ////Office Contest Candidates Report
    //private void Table_Candidates_Report_Visible_Load()
    //{
    //  HTMLTableCandidatesForThisOffice.Text =
    //    Report_Candidates_Office(
    //      PageCache,
    //      ViewState["ElectionKey"].ToString(),
    //      ViewState["OfficeKey"].ToString());
    //}

    ////Politicians in Database with Same Last Name Report
    //private void Table_Same_Last_Name_Report_Visible_Load()
    //{
    //  if (ViewState["Mode"].ToString() == "CompleteCandidateAddition")
    //  {
    //    Table_Same_Last_Name_Report.Visible = true;

    //    #region commented
    //    //#region Table with Same Last Name
    //    //PoliticiansTable SameLastNameTable = Table_Same_Last_Name();
    //    //int politicians = SameLastNameTable.Count;
    //    ////if (politicians > 0)
    //    ////{

    //    //#region Same Last Name Report
    //    ////PoliticiansTable SameLastNameTable = Table_Same_Last_Name();
    //    ////int politicians = SameLastNameTable.Count;

    //    //string politiciansHTMLTable = string.Empty;

    //    //if (politicians > 0)
    //    //{
    //    //  #region Report politicians with same last name
    //    //  LabelCandidateToAdd.Text = "Select Candidate to Add to Office Contest";
    //    //  tr_SelectCandidate.Visible = true;
    //    //  tr_SelectCandidateInstruction.Visible = true;

    //    //  #region Table Init
    //    //  politiciansHTMLTable += "<table cellspacing=0 cellpadding=0 width=100%>";
    //    //  #endregion Table Init

    //    //  #region Heading
    //    //  politiciansHTMLTable += "<tr>";

    //    //  politiciansHTMLTable += "<td align=center class=tdReportDetailHeading>";
    //    //  politiciansHTMLTable += "Politician (Party)";
    //    //  politiciansHTMLTable += "</td>";

    //    //  politiciansHTMLTable += "<td align=center class=tdReportDetailHeading>";
    //    //  politiciansHTMLTable += "Office";
    //    //  politiciansHTMLTable += "</td>";

    //    //  politiciansHTMLTable += "<td align=center class=tdReportDetailHeading>";
    //    //  politiciansHTMLTable += "State Information";
    //    //  politiciansHTMLTable += "</td>";

    //    //  politiciansHTMLTable += "<td align=center class=tdReportDetailHeading>";
    //    //  politiciansHTMLTable += "Politician Information";
    //    //  politiciansHTMLTable += "</td>";

    //    //  politiciansHTMLTable += "<td align=center class=tdReportDetailHeading>";
    //    //  politiciansHTMLTable += "Other";
    //    //  politiciansHTMLTable += "</td>";

    //    //  politiciansHTMLTable += "</tr>";
    //    //  #endregion Heading

    //    //  foreach (PoliticiansRow SameLastNameRow in SameLastNameTable)
    //    //  {
    //    //    string officeKey = PageCache.GetTemporary().Politicians
    //    //      .GetOfficeStatus(SameLastNameRow.PoliticianKey).OfficeKey;

    //    //    #region Politician with same last name
    //    //    politiciansHTMLTable += "<tr>";

    //    //    #region Politician <td>
    //    //    politiciansHTMLTable += "<td valign=top class=tdReportDetail>";

    //    //    #region HeadShot Anchor
    //    //    string HeadShot = string.Empty;
    //    //    HeadShot += db.AnchorPoliticianImageOrNoPhoto(
    //    //        db.Url_Politician_Intro(SameLastNameRow.PoliticianKey)
    //    //      , SameLastNameRow.PoliticianKey
    //    //      , db.Image_Size_25_Headshot
    //    //      , db.GetPoliticianName(SameLastNameRow.PoliticianKey) + " Intro Page Data Entry"
    //    //      , "_intro"
    //    //      );

    //    //    politiciansHTMLTable += HeadShot;
    //    //    politiciansHTMLTable += "&nbsp";
    //    //    //}
    //    //    #endregion HeadShot Anchor

    //    //    #region Name Anchor
    //    //    politiciansHTMLTable += db.Anchor_Admin_OfficeContest(
    //    //      SameLastNameRow.PoliticianKey,
    //    //      ViewState["OfficeKey"].ToString(),
    //    //      ViewState["ElectionKey"].ToString(),
    //    //      db.GetPoliticianName(SameLastNameRow.PoliticianKey)
    //    //      , "_officecontest"
    //    //    );

    //    //    #endregion Name Anchor

    //    //    #region Party
    //    //    if (SameLastNameRow.PartyKey != string.Empty)
    //    //      politiciansHTMLTable += " (" + db.Parties_Str(SameLastNameRow.PartyKey, "PartyCode") + ")";
    //    //    #endregion Party

    //    //    #region PoliticianKey
    //    //    politiciansHTMLTable += "<br>(" + SameLastNameRow.PoliticianKey + ")";
    //    //    #endregion PoliticianKey

    //    //    politiciansHTMLTable += "</td>";
    //    //    #endregion Politician <td>

    //    //    #region Office <td>
    //    //    politiciansHTMLTable += "<td valign=top class=tdReportDetail>";

    //    //    string Office = string.Empty;
    //    //    if (officeKey != string.Empty)
    //    //    {
    //    //      if (db.Is_Valid_Office(officeKey))
    //    //      {
    //    //        Office += db.Offices_Str(officeKey, "OfficeLine1");
    //    //        Office += " " + db.Offices_Str(officeKey, "OfficeLine2");
    //    //        Office = db.Anchor_Admin_Office_UPDATE_Office(
    //    //          officeKey, Office);
    //    //      }
    //    //      else
    //    //        Office += "No Office Identified for Politician";
    //    //    }
    //    //    else
    //    //      Office += "No Office Identified for Politician";

    //    //    politiciansHTMLTable += Office;
    //    //    politiciansHTMLTable += "</td>";
    //    //    #endregion

    //    //    #region State Data <td>
    //    //    politiciansHTMLTable += "<td valign=top class=tdReportDetail>";
    //    //    //if (SameLastNameRow["StateAddress"].ToString() != string.Empty)
    //    //    //{
    //    //    //  politiciansHTMLTable += SameLastNameRow["StateAddress"].ToString() + "<br>";
    //    //    //  politiciansHTMLTable += SameLastNameRow["StateCityStateZip"].ToString() + "<br>";
    //    //    //}
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    //if (DbPolitician.Has_Complete_State_Address(SameLastNameRow))
    //    //    //{
    //    //    //  politiciansHTMLTable += DbPolitician.Get_State_Address(SameLastNameRow) + "<br>";
    //    //    //  politiciansHTMLTable += DbPolitician.Get_State_CityStateZip(SameLastNameRow) + "<br>";
    //    //    //}
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.HasCompleteStateAddress)
    //    //    {
    //    //      politiciansHTMLTable += SameLastNameRow.StateAddress + "<br>";
    //    //      politiciansHTMLTable += SameLastNameRow.StateCityStateZip + "<br>";
    //    //    }
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";

    //    //    //if (SameLastNameRow["StatePhone"].ToString() != string.Empty)
    //    //    //  politiciansHTMLTable += SameLastNameRow["StatePhone"].ToString() + "<br>";
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.StatePhone != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.StatePhone + "<br>";
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    //if (SameLastNameRow["StateEmailAddr"].ToString() != string.Empty)
    //    //    //  politiciansHTMLTable += SameLastNameRow["StateEmailAddr"].ToString() + "<br>";
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.StateEmail != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.StateEmail + "<br>";
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    //if (SameLastNameRow["StateWebAddr"].ToString() != string.Empty)
    //    //    //  politiciansHTMLTable += SameLastNameRow["StateWebAddr"].ToString() + "<br>";
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.StateWebAddress != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.StateWebAddress + "<br>";
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    politiciansHTMLTable += "</td>";
    //    //    #endregion State Data <td>

    //    //    #region Politician Data <td>
    //    //    politiciansHTMLTable += "<td valign=top class=tdReportDetail>";
    //    //    //if (SameLastNameRow["Address"].ToString() != string.Empty)
    //    //    //{
    //    //    //  politiciansHTMLTable += SameLastNameRow["Address"].ToString() + "<br>";
    //    //    //  politiciansHTMLTable += SameLastNameRow["CityStateZip"].ToString() + "<br>";
    //    //    //}
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    //if (DbPolitician.Get_Politician_Address(SameLastNameRow) != string.Empty)
    //    //    //  politiciansHTMLTable += DbPolitician.Get_Politician_Address(SameLastNameRow);
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.Address != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.Address;
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    //if (DbPolitician.Get_Politician_CityStateZip(SameLastNameRow) != string.Empty)
    //    //    //  politiciansHTMLTable += DbPolitician.Get_Politician_CityStateZip(SameLastNameRow);
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.CityStateZip != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.CityStateZip;
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    //if (SameLastNameRow["Phone"].ToString() != string.Empty)
    //    //    //  politiciansHTMLTable += SameLastNameRow["Phone"].ToString() + "<br>";
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.Phone != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.Phone + "<br>";
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    //if (SameLastNameRow["EmailAddr"].ToString() != string.Empty)
    //    //    //  politiciansHTMLTable += SameLastNameRow["EmailAddr"].ToString() + "<br>";
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.Email != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.Email + "<br>";
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    //if (SameLastNameRow["WebAddr"].ToString() != string.Empty)
    //    //    //  politiciansHTMLTable += SameLastNameRow["WebAddr"].ToString() + "<br>";
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.WebAddress != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.WebAddress + "<br>";
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    politiciansHTMLTable += "</td>";
    //    //    #endregion Politician Data <td>

    //    //    #region Other Data <td>
    //    //    politiciansHTMLTable += "<td valign=top class=tdReportDetail>";
    //    //    //if (SameLastNameRow["LDSAddress"].ToString() != string.Empty)
    //    //    //{
    //    //    //  politiciansHTMLTable += SameLastNameRow["LDSAddress"].ToString() + "<br>";
    //    //    //  politiciansHTMLTable += SameLastNameRow["LDSCityStateZip"].ToString() + "<br>";
    //    //    //}
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    //if (DbPolitician.Has_Complete_Lds_Address(SameLastNameRow))
    //    //    //{
    //    //    //  politiciansHTMLTable += DbPolitician.Get_Lds_Address(SameLastNameRow) + "<br>";
    //    //    //  politiciansHTMLTable += DbPolitician.Get_Lds_CityStateZip(SameLastNameRow) + "<br>";
    //    //    //}
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.HasCompleteLDSAddress)
    //    //    {
    //    //      politiciansHTMLTable += SameLastNameRow.LDSAddress + "<br>";
    //    //      politiciansHTMLTable += SameLastNameRow.LDSCityStateZip + "<br>";
    //    //    }
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    //if (SameLastNameRow["LDSPhone"].ToString() != string.Empty)
    //    //    //  politiciansHTMLTable += SameLastNameRow["LDSPhone"].ToString() + "<br>";
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.LDSPhone != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.LDSPhone + "<br>";
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    //if (SameLastNameRow["LDSEmailAddr"].ToString() != string.Empty)
    //    //    //  politiciansHTMLTable += SameLastNameRow["LDSEmailAddr"].ToString() + "<br>";
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.LDSEmail != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.LDSEmail + "<br>";
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    //if (SameLastNameRow["LDSWebAddr"].ToString() != string.Empty)
    //    //    //  politiciansHTMLTable += SameLastNameRow["LDSWebAddr"].ToString() + "<br>";
    //    //    //else
    //    //    //  politiciansHTMLTable += "&nbsp;";
    //    //    if (SameLastNameRow.LDSWebAddress != string.Empty)
    //    //      politiciansHTMLTable += SameLastNameRow.LDSWebAddress + "<br>";
    //    //    else
    //    //      politiciansHTMLTable += "&nbsp;";
    //    //    politiciansHTMLTable += "</td>";
    //    //    #endregion

    //    //    politiciansHTMLTable += "</tr>";
    //    //    #endregion Other Data <td>
    //    //  }
    //    //  #endregion Politician with same last name
    //    //}
    //    //else
    //    //{
    //    //  #region Report No politicians have the same last name
    //    //  politiciansHTMLTable += "<tr>";
    //    //  politiciansHTMLTable += "<td colspan=5 class=tdReportDetail>";
    //    //  politiciansHTMLTable += "<center>There are no politicians in the State with the same last name in the database.</center>";
    //    //  politiciansHTMLTable += "</td>";
    //    //  politiciansHTMLTable += "</tr>";
    //    //  #endregion Report No politicians have the same last name
    //    //}

    //    //politiciansHTMLTable += "</table>";

    //    //HTMLTableSameLastName.Text = politiciansHTMLTable;
    //    //#endregion Report Detail lines


    //    //#endregion Table with Same Last Name
    //    #endregion commented

    //    Report_Same_Last_Name();

    //    #region commented Msg
    //    //if (SameLastNameTable.Rows.Count == 0)
    //    //{
    //    //  #region NO POLITICIANS WITH Same Last Names
    //    //  if (ViewState["OfficeKey"].ToString() != "USPresident")
    //    //  {
    //    //    #region Msg
    //    //    Msg.Text = db.Msg(
    //    //    "There are no politicians in the State with the same last name in the database."
    //    //    + " Use the textboxes to complete the name and click the Add This Politician Button."
    //    //    + " You will then be presented with a form to complete the information about this person."
    //    //    + " Or you can skip adding this person"
    //    //    + " and proceed with adding a different person"
    //    //    + " by clicking the Add a Politician Link in the last section.");
    //    //    #endregion Msg
    //    //  }
    //    //  else
    //    //  {
    //    //    #region Msg
    //    //    Msg.Text = db.Msg("There are NO politicians with this last name."
    //    //    + " BUT all US President candidates need to be added at the State level.");
    //    //    #endregion Msg
    //    //  }
    //    //  #endregion NO POLITICIANS WITH Same Last Names
    //    //}
    //    //else
    //    //{
    //    //  #region SOME POLITICIANS WITH Same Last Names
    //    //  if (ViewState["OfficeKey"].ToString() != "USPresident")
    //    //  {
    //    //    #region Msg
    //    //    Msg.Text = db.Msg(
    //    //      "Politicians with the last name that you entered are in the report below."
    //    //      + " If the person you are about to add is DEFINITELY NOT in this list,"
    //    //      + " then use the textboxes to complete the name and click the Add This Politician Button."
    //    //      + " If the person IS in the list, you may click on the name link to add that politician in this office contest."
    //    //      + " After either action you will then be presented with a form to edit or complete information about the candidate."
    //    //      + " Or click the Terminate Politician Link to abort the addition.");
    //    //    #endregion Msg
    //    //  }
    //    //  else
    //    //  {
    //    //    #region Msg
    //    //    Msg.Text = db.Msg("There are SOME politicians with this last name."
    //    //    + " BUT all US President politicians need to be added at the State level.");
    //    //    #endregion Msg
    //    //  }
    //    //  #endregion SOME POLITICIANS WITH Same Last Names
    //    //}
    //    #endregion Msg

    //  }
    //  else
    //  {
    //    Table_Same_Last_Name_Report.Visible = false;
    //  }
    //}

    ////Add a Candidate
    //private void Table_Add_A_Candidate_Visible_Load()
    //{
    //  if (
    //    (ViewState["Mode"].ToString() == "IdentifyCandidate")
    //    )
    //  {
    //    Table_Add_A_Candidate.Visible = true;
    //    TextBoxLastName.Text = string.Empty;
    //  }
    //  else
    //  {
    //    Table_Add_A_Candidate.Visible = false;
    //  }
    //}

    ////Complete Addition of Candidate Not in Database  
    //private void Table_Complete_Politician_Add_Visible_Load()
    //{
    //  if (ViewState["Mode"].ToString() == "CompleteCandidateAddition")
    //  {
    //    #region Table with Same Last Name
    //    Table_Complete_Politician_Add.Visible = true;

    //    if (Offices.IsUSPresident(ViewState["OfficeKey"].ToString()))
    //      tr_President_State.Visible = true;
    //    else
    //      tr_President_State.Visible = false;

    //    #region empty textboxes, load last name
    //    TextBoxFirstAdd.Text = string.Empty;
    //    TextBoxMiddleAdd.Text = string.Empty;
    //    TextBoxNickNameAdd.Text = string.Empty;
    //    TextBoxLastAdd.Text = ViewState["Name"].ToString();
    //    TextBoxSuffixAdd.Text = string.Empty;
    //    TextBoxAddOnAdd.Text = string.Empty;
    //    #endregion ViewState["Name"].ToString()

    //    #region Hide or Show ANC Controls
    //    if (ViewState["StateCode"].ToString() == "DC")
    //    {
    //      tdANCTextAdd.Visible = true;
    //      tdANCTextBoxAdd.Visible = true;
    //    }
    //    else
    //    {
    //      tdANCTextAdd.Visible = false;
    //      tdANCTextBoxAdd.Visible = false;
    //    }
    //    #endregion Hide or Show ANC Controls

    //    #endregion Table with Same Last Name
    //  }
    //  else
    //  {
    //    Table_Complete_Politician_Add.Visible = false;
    //  }
    //}

    ////Add More Candidates
    //private void Table_Add_More_Candidates_Visible_Load()
    //{
    //  if (
    //    (ViewState["Mode"].ToString() == "EditCandidate")
    //    || (ViewState["Mode"].ToString() == "AddCandidate")
    //    )
    //  {
    //    Table_Add_More_Candidates.Visible = true;
    //  }
    //  else
    //  {
    //    Table_Add_More_Candidates.Visible = false;
    //  }
    //}

    ////Remove the Candidate
    //private void Table_Remove_On_Ballot_Visible_Load()
    //{
    //  if (
    //    (ViewState["Mode"].ToString() == "EditCandidate")
    //    || (ViewState["Mode"].ToString() == "AddCandidate")
    //    )
    //  {
    //    Table_Remove_On_Ballot.Visible = true;
    //    Label_Remove_Name.Text = Politicians.GetFormattedName(ViewState["PoliticianKey"].ToString());
    //  }
    //  else
    //  {
    //    Table_Remove_On_Ballot.Visible = false;
    //  }
    //}

    ////Candidate Ballot Order
    //private void Table_Ballot_Order_Visible_Load()
    //{
    //  if (
    //    (ViewState["Mode"].ToString() == "EditCandidate")
    //    || (ViewState["Mode"].ToString() == "AddCandidate")
    //    )
    //  {
    //    Table_Ballot_Order.Visible = true;

    //    TextBoxOrder.Text = BallotOrder_Get();
    //  }
    //  else
    //  {
    //    Table_Ballot_Order.Visible = false;
    //  }
    //}

    ////Candidate's Party and Contact Information
    //private void Table_Party_And_Contact_Info_Visible_Load()
    //{
    //  if (
    //    (ViewState["Mode"].ToString() == "EditCandidate")
    //    || (ViewState["Mode"].ToString() == "AddCandidate")
    //    )
    //  {
    //    Table_Candidate_Info.Visible = true;

    //    HyperLinkCandidateInfo.Text = Politicians.GetFormattedName(ViewState["PoliticianKey"].ToString())
    //      + "'s Party and Contact Information";
    //    HyperLinkCandidateInfo.NavigateUrl = db.Url_Admin_Politician(ViewState["PoliticianKey"].ToString());
    //  }
    //  else
    //  {
    //    Table_Candidate_Info.Visible = false;
    //  }
    //}

    ////Terminate Candidate Addition
    //private void Table_Terminate_Candidate_Addition_Visible_Load()
    //{
    //  if (ViewState["Mode"].ToString() == "CompleteCandidateAddition")
    //  {
    //    #region Table with Same Last Name
    //    Table_Terminate_Addition.Visible = true;

    //    HyperLink_Terminate_Addition.NavigateUrl =
    //      db.Url_Admin_OfficeContest(
    //      ViewState["ElectionKey"].ToString(),
    //      ViewState["OfficeKey"].ToString()
    //      );

    //    #endregion Table with Same Last Name
    //  }
    //  else
    //  {
    //    Table_Terminate_Addition.Visible = false;
    //  }
    //}

    ////Running Mate
    //private void Table_Running_Mate_Visible_Load()
    //{
    //  if (Offices.IsRunningMateOffice(ViewState["OfficeKey"].ToString()))
    //  {
    //    if (
    //      (ViewState["Mode"].ToString() == "EditCandidate")
    //      || (ViewState["Mode"].ToString() == "AddCandidate")
    //      )
    //    {
    //      Table_Running_Mate.Visible = true;

    //      RunningMateID.Text = string.Empty;
    //      RunningMate.Text = "No Running Mate Identified";

    //      //if (db.ElectionsPoliticians(
    //      if (db.ElectionsPoliticians_Optional(
    //      ViewState["ElectionKey"].ToString()
    //        , ViewState["OfficeKey"].ToString()
    //        , ViewState["PoliticianKey"].ToString()
    //        , "RunningMateKey") != string.Empty)
    //      {
    //        RunningMateID.Text = db.ElectionsPoliticians_Str(
    //          ViewState["ElectionKey"].ToString()
    //          , ViewState["OfficeKey"].ToString()
    //        , ViewState["PoliticianKey"].ToString()
    //        , "RunningMateKey");
    //        RunningMate.Text = Politicians.GetFormattedName(RunningMateID.Text.Trim());
    //      }

    //      #region Find Politicianss HyperLink
    //      if (Offices.GetOfficeClass(ViewState["OfficeKey"].ToString()).IsValid())
    //      {
    //        HyperlinkPoliticiansList.NavigateUrl =
    //          db.Url_Admin_Politicians(
    //          OfficeClass.All
    //          , Offices.GetStateCodeFromKey(ViewState["OfficeKey"].ToString())
    //          );
    //      }
    //      #endregion Find Politicianss HyperLink
    //    }
    //    else
    //    {
    //      Table_Running_Mate.Visible = false;
    //    }
    //  }
    //  else
    //    Table_Running_Mate.Visible = false;
    //}

    ////Eidt Office
    //private void Table_Edit_Office_Visible_Load()
    //{
    //  Table_Edit_Office.Visible = true;

    //  string officeKey = ViewState["OfficeKey"].ToString();

    //  HyperLinkOffice.Text = "Edit ";
    //  HyperLinkOffice.Text += Offices.FormatOfficeName(officeKey);
    //  HyperLinkOffice.NavigateUrl = db.Url_Admin_Office_UPDATE(officeKey);
    //}

    //private void Controls_Visible_And_Load()
    //{
    //  Table_Candidates_Report_Visible_Load();
    //  Table_Same_Last_Name_Report_Visible_Load();
    //  Table_Add_A_Candidate_Visible_Load();
    //  Table_Complete_Politician_Add_Visible_Load();
    //  Table_Add_More_Candidates_Visible_Load();
    //  Table_Remove_On_Ballot_Visible_Load();
    //  Table_Ballot_Order_Visible_Load();
    //  Table_Party_And_Contact_Info_Visible_Load();
    //  Table_Terminate_Candidate_Addition_Visible_Load();
    //  Table_Running_Mate_Visible_Load();
    //  Table_Edit_Office_Visible_Load();
    //}

    //#endregion Visible Html Tables

    //protected void TextBoxLastName_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    #region checks
    //    Check_TextBoxes_Not_Illegal();

    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxLastName);

    //    if (TextBoxLastName.Text.Trim() == string.Empty)
    //      throw new ApplicationException("A Last Name needs to be entered in the text box provided.");
    //    #endregion checks

    //    ViewState["Name"] = db.Str_Remove_Last_Char(
    //      TextBoxLastName.Text.Trim());

    //    ViewState["Mode"] = "CompleteCandidateAddition";

    //    Controls_Visible_And_Load();

    //    #region Msg
    //    string msg = string.Empty;
    //    if (Rows_Same_Last_Name() == 0)
    //      msg = "Enter the other parts of the candidate's name in the textboxes provided"
    //        + " as the name should appear on ballots.";
    //    else
    //      msg = "In the report of candidates with the same last name"
    //      + " click the name of the candidate to add to this election contest."
    //      + " If the candidate is not in the report of candidates"
    //      + " enter the other parts of the candidate's name in the textboxes provided"
    //      + " as the name should appear on ballots.";

    //    Msg.Text = db.Msg(msg);
    //    #endregion Msg

    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void Button_Complete_Politician_Add_Click1(object sender, EventArgs e)
    //{
    //  #region note
    //  //User selected this 'Complete Politician Addition' Button
    //  //because a candidate was not selected in the report
    //  //of candidates with the same last name.
    //  #endregion note
    //  try
    //  {
    //    #region Checks

    //    Check_TextBoxes_Not_Illegal();

    //    #region Check US President has a selected StateCode
    //    if (ViewState["OfficeKey"].ToString() == "USPresident")
    //    {
    //      if (DropDownListState.SelectedValue == "Select State")
    //        throw new ApplicationException("For US President you need to select a state"
    //          + " where the candidate lives");
    //    }
    //    #endregion Check US President has a selected StateCode

    //    #region Check Name Parts
    //    if (!Validation.IsValidGivenName(TextBoxFirstAdd.Text))
    //      throw new ApplicationException(string.Format(
    //        Validation.GivenNameValidationMessage, "The First Name"));

    //    if (!Validation.IsValidGivenName(TextBoxFirstAdd.Text))
    //      throw new ApplicationException(string.Format(
    //        Validation.GivenNameValidationMessage, "The Middle Name"));

    //    if (!Validation.IsValidNickname(db.Str_Remove_Quotes(TextBoxNickNameAdd.Text)))
    //      throw new ApplicationException(string.Format(
    //        Validation.NicknameValidationMessage, "The Nickname"));

    //    if (!Validation.IsValidLastName(TextBoxLastAdd.Text))
    //      throw new ApplicationException(string.Format(
    //        Validation.LastNameValidationMessage, "The Last Name"));

    //    if (!Validation.IsValidNameSuffix(TextBoxSuffixAdd.Text))
    //      throw new ApplicationException(string.Format(
    //        Validation.NameSuffixValidationMessage, "The Suffix"));
    //    #endregion Check Name Parts

    //    #region Check Minimum entry data
    //    if (
    //      (TextBoxFirstAdd.Text.Trim() == string.Empty)
    //      || (TextBoxLastAdd.Text.Trim() == string.Empty)
    //      )
    //      throw new ApplicationException("As a minimum you need to enter a First Name and Last Name.");
    //    #endregion Check Minimum entry data

    //    #region Every politician must have a valid OfficeKey
    //    if (!Offices.OfficeKeyExists(ViewState["OfficeKey"].ToString()))
    //      throw new ApplicationException("There is no valid office for OfficeKey: "
    //        + ViewState["OfficeKey"].ToString());
    //    #endregion Every politician must have a valid OfficeKey

    //    #region Check Politicians with same Last Name
    //    DataTable Table_Same_LastName = Table_Same_Last_Name();
    //    if (
    //      (Table_Same_LastName.Rows.Count > 0)
    //      && (!Convert.ToBoolean(ViewState["Is_Confirmed_To_Complete_Addition"]))
    //      )
    //    {
    //      if (!Convert.ToBoolean(ViewState["Is_Confirmed_To_Complete_Addition"]))
    //      {
    //        ViewState["Is_Confirmed_To_Complete_Addition"] = true;

    //        throw new ApplicationException("There are "
    //          + Table_Same_LastName.Rows.Count.ToString()
    //          + " politicians with the same last name."
    //          + " Please click the Complete Politician Addition Button again"
    //          + " to confirm that this person is truly a NEW politician"
    //          + " and not one of politicians listed in the report.");
    //      }
    //    }
    //    #endregion Check Politicians with same Last Name

    //    #endregion Checks

    //    #region Create a new Politician

    //    #region stateCode
    //    string stateCode = string.Empty;
    //    if (ViewState["OfficeKey"].ToString() != "USPresident")
    //      stateCode = ViewState["StateCode"].ToString();
    //    else
    //      stateCode = DropDownListState.SelectedValue;
    //    #endregion stateCode

    //    #region Create New PoliticianKey

    //    ViewState["PoliticianKey"] = db.Politician_Key(
    //      stateCode
    //      , db.Str_Remove_Puctuation(TextBoxLastAdd.Text.Trim())
    //      , db.Str_Remove_Puctuation(TextBoxFirstAdd.Text.Trim())
    //      , db.Str_Remove_Puctuation(TextBoxMiddleAdd.Text.Trim())
    //      , db.Str_Remove_Puctuation(TextBoxSuffixAdd.Text.Trim()));
    //    if (string.IsNullOrEmpty(ViewState["PoliticianKey"].ToString()))
    //      throw new ApplicationException("The new PoliticianKey is empty.");
    //    if (Politicians.IsValid(ViewState["PoliticianKey"].ToString()))
    //      throw new ApplicationException("A politician with this exact name already exist in the database."
    //        + " If this is truly a new politician with the same name"
    //        + " you can still add this politician by modifying the name slightly so that a unique identity can be created."
    //        + " You do this by modifying the first, last or middle name,"
    //        + " like Bill for William"
    //        + " or adding or deleting the middle initial."
    //        + " Then after you have added the politician, with a uniqe identity, you can edit the name as it should appear on ballots.");
    //    #endregion Create New PoliticianKey

    //    #region Check OfficeKey is valid
    //    if (!Offices.OfficeKeyExists(ViewState["OfficeKey"].ToString()))
    //    {
    //      throw new ApplicationException(
    //        "The Office does not exist for OfficeKey: "
    //        + ViewState["OfficeKey"].ToString());
    //    }
    //    #endregion Check OfficeKey is valid

    //    #region Identify Party if election is a Party Primary
    //    string Party_Key = "X"; //No Party
    //    if (!string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
    //    {
    //      if (Elections.IsPrimaryElection(ViewState["ElectionKey"].ToString()))
    //      {
    //        Party_Key = ViewState["StateCode"].ToString();
    //        Party_Key += db.Elections_Str(
    //          ViewState["ElectionKey"].ToString()
    //        , "NationalPartyCode");
    //      }
    //    }
    //    #endregion Identify Party if election is a Party Primary

    //    #region Insert Politicians row
    //    db.Politician_Insert(
    //       ViewState["PoliticianKey"].ToString()
    //      , ViewState["OfficeKey"].ToString()
    //      , Politicians.GetStateCodeFromKey(ViewState["PoliticianKey"].ToString())
    //      , TextBoxFirstAdd.Text.Trim()
    //      , db.Str_Remove_Quotes(TextBoxMiddleAdd.Text.Trim())
    //      , TextBoxLastAdd.Text.Trim()
    //      , TextBoxSuffixAdd.Text.Trim()
    //      , TextBoxAddOnAdd.Text.Trim()
    //      , TextBoxNickNameAdd.Text.Trim()
    //      , Party_Key
    //      );
    //    #endregion Insert Politicians row

    //    #endregion Create a new Politician

    //    #region Insert into ElectionsPoliticians as Candidate for Office in Election

    //    #region Add candidate to the office contest
    //    //db.ElectionsPoliticians_INSERT_And_Whether_Incumbent(
    //    //  ViewState["ElectionKey"].ToString()
    //    //  , ViewState["OfficeKey"].ToString()
    //    //  , ViewState["PoliticianKey"].ToString()
    //    //  );
    //    ElectionsPoliticians_INSERT();
    //    #endregion Add candidate to the office contest

    //    #region Msg.Text
    //    //Msg_Return = db.GetPoliticianName(ViewState["PoliticianKey"].ToString())
    //    //   + " was ADDED to the database"
    //    //   + " as a candidate for " + db.Name_Office(ViewState["OfficeKey"].ToString()) + "."
    //    //   + " Use the link provided to identify the politician's political party"
    //    //   + " and contact information.";
    //    //if (Is_ReCased_Name)
    //    //  Msg.Text = db.Warn(Msg_ReCased
    //    //  + Msg_Return);
    //    //else
    //    //  Msg.Text = db.Ok(Msg_Return);
    //    string msg = Politicians.GetFormattedName(ViewState["PoliticianKey"].ToString())
    //       + " was ADDED to the database"
    //       + " as a candidate for " + Offices.FormatOfficeName(ViewState["OfficeKey"].ToString()) + "."
    //       + " You can now either:"
    //       + " <br>Enter the last name of another candidate to add."
    //       + " <br>Or remove the candidate you just added."
    //       + " <br>Or use the link provided to identify the candidates's political party"
    //       + " and contact information."
    //       + " <br>Or change the order of the candidate on ballots.";
    //    if (Offices.IsRunningMateOffice(ViewState["OfficeKey"].ToString()))
    //      msg += "<br>Or identify the candidate's running mate.";

    //    Msg.Text = db.Ok(msg);
    //    #endregion Msg.Text
    //    #endregion Insert into ElectionsPoliticians as Candidate for Office in Election

    //    Msg.Text = db.Ok(Politicians.GetFormattedName(ViewState["PoliticianKey"].ToString())
    //      + " has been added to the database and added as a candidate in this office contest.");

    //    ViewState["Name"] = string.Empty;

    //    ViewState["Mode"] = "EditCandidate";

    //    Resequence_BallotOrder();

    //    Controls_Visible_And_Load();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }

    //}

    //protected void Button_Remove_From_Election_Click(object sender, EventArgs e)
    //{
    //  Check_TextBoxes_Not_Illegal();

    //  db.ElectionsPoliticians_Delete_Log_Invalidate(
    //  ViewState["ElectionKey"].ToString()
    //  , ViewState["OfficeKey"].ToString()
    //  , ViewState["PoliticianKey"].ToString()
    //    );

    //  //db.Invalidate_Politician(ViewState["PoliticianKey"].ToString());

    //  Msg.Text = db.Ok(Politicians.GetFormattedName(ViewState["PoliticianKey"].ToString())
    //    + " has been removed from this election.");

    //  ViewState["PoliticianKey"] = string.Empty;
    //  ViewState["Mode"] = "IdentifyCandidate";

    //  Resequence_BallotOrder();

    //  Controls_Visible_And_Load();
    //}

    //protected void Button_AddCandidates_Click(object sender, EventArgs e)
    //{
    //  Check_TextBoxes_Not_Illegal();

    //  ViewState["PoliticianKey"] = string.Empty;
    //  ViewState["Mode"] = "IdentifyCandidate";

    //  Controls_Visible_And_Load();

    //  Msg.Text = db.Ok("Enter the last name of a candidate to add to this office contest.");

    //}

    //protected void TextBoxOrder_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Check_TextBoxes_Not_Illegal();
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxOrder);

    //    if (BallotOrder_Get() != TextBoxOrder.Text.Trim())
    //    {
    //      db.Ballot_Order_Check(TextBoxOrder.Text.Trim());

    //      db.ElectionsPoliticians_Update_Int(
    //        ViewState["ElectionKey"].ToString()
    //      , ViewState["OfficeKey"].ToString()
    //      , ViewState["PoliticianKey"].ToString()
    //      , "OrderOnBallot"
    //      , Convert.ToInt16(TextBoxOrder.Text.Trim())
    //      );

    //      //db.Invalidate_Election(ViewState["ElectionKey"].ToString());

    //      Msg.Text = db.Ok("The candidate order on ballot has been changed.");

    //      Resequence_BallotOrder();

    //      Controls_Visible_And_Load();
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

    //protected void TextboxRunningMate_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    #region checks
    //    Check_TextBoxes_Not_Illegal();

    //    db.Throw_Exception_TextBox_Html_Or_Script(TextboxRunningMate);

    //    if (TextboxRunningMate.Text.Trim() == string.Empty)
    //      throw new ApplicationException("A PoliticianKey of the running mate needs to be entered in the text box provided.");
    //    #endregion checks

    //    db.ElectionsPoliticians_Update_Str(
    //          ViewState["ElectionKey"].ToString()
    //          , ViewState["OfficeKey"].ToString()
    //          , ViewState["PoliticianKey"].ToString()
    //          , "RunningMateKey"
    //          , TextboxRunningMate.Text.Trim());

    //    //db.Invalidate_Politician(ViewState["PoliticianKey"].ToString());

    //    Msg.Text = db.Ok(
    //      Politicians.GetFormattedName(TextboxRunningMate.Text.Trim())
    //     + " was identified as the running mate of "
    //     + Politicians.GetFormattedName(ViewState["PoliticianKey"].ToString())
    //     );

    //    Controls_Visible_And_Load();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }

    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    try
    //    {
    //      #region ViewState inits & checks
    //      ViewState["ElectionKey"] = string.Empty;
    //      ViewState["OfficeKey"] = string.Empty;
    //      ViewState["PoliticianKey"] = string.Empty;
    //      ViewState["StateCode"] = string.Empty;
    //      ViewState["CountyCode"] = string.Empty;
    //      ViewState["LocalCode"] = string.Empty;
    //      ViewState["LastName"] = string.Empty;
    //      ViewState["Mode"] = string.Empty;

    //      #region electionKey
    //      string electionKey = string.Empty;
    //      if (!string.IsNullOrEmpty(QueryElection))
    //      {
    //        ViewState["ElectionKey"] = QueryElection;
    //        electionKey = QueryElection;
    //      }
    //      if (string.IsNullOrEmpty(electionKey))
    //        throw new ApplicationException("No ElectionKey was passed in the query string.");
    //      if (!db.Is_Valid_Election(electionKey))
    //        throw new ApplicationException("The ElectionKey passed in the query string is invalid.");
    //      #endregion electionKey

    //      #region officeKey
    //      string officeKey = string.Empty;
    //      if (!string.IsNullOrEmpty(QueryOffice))
    //      {
    //        ViewState["OfficeKey"] = QueryOffice;
    //        officeKey = QueryOffice;
    //      }
    //      if (string.IsNullOrEmpty(officeKey))
    //        throw new ApplicationException("No OfficeKey was passed in the query string.");
    //      if (!Offices.OfficeKeyExists(officeKey))
    //        throw new ApplicationException("The OfficeKey passed in the query string is invalid.");
    //      if (!db.Is_Valid_ElectionsOffices(electionKey,
    //        officeKey))
    //        throw new ApplicationException("The office is not in the election..");
    //      #endregion officeKey

    //      #region politicianKey
    //      string politicianKey = string.Empty;
    //      if (!string.IsNullOrEmpty(QueryId))
    //      {
    //        ViewState["PoliticianKey"] = QueryId;
    //        politicianKey = QueryId;
    //        if (!Politicians.IsValid(politicianKey))
    //          throw new ApplicationException("No PoliticianKey passed in the query string was invalid.");
    //      }
    //      #endregion politicianKey

    //      #region stateCode, countyCode, localCode

    //      if (!string.IsNullOrEmpty(QueryElection))
    //      {
    //        ViewState["StateCode"] =
    //          Elections.GetStateCodeFromKey(QueryElection);
    //        ViewState["CountyCode"] =
    //          Elections.GetCountyCodeFromKey(QueryElection);
    //        ViewState["LocalCode"] =
    //          Elections.GetLocalCodeFromKey(QueryElection);
    //      }
    //      else if (!string.IsNullOrEmpty(QueryOffice))
    //      {
    //        ViewState["StateCode"] =
    //          Offices.GetStateCodeFromKey(QueryOffice);
    //        ViewState["CountyCode"] =
    //          Offices.GetCountyCodeFromKey(QueryOffice);
    //        ViewState["LocalCode"] =
    //          Offices.GetLocalCodeFromKey(QueryOffice);
    //      }
    //      #endregion stateCode, countyCode, localCode

    //      #region lastName
    //      string lastName = string.Empty;
    //      if (!string.IsNullOrEmpty(GetQueryString("Name")))
    //      {
    //        ViewState["LastName"] = GetQueryString("Name");
    //        lastName = GetQueryString("Name");
    //      }
    //      #endregion lastName
    //      #endregion ViewState inits & checks

    //      #region mode

    //      #region Three modes when not postback
    //      //1) EditCandidate - A candidate was selected in the report of candidates for editing
    //      //2) AddCandidate - A politician with the same last name was selected from Candidates with Same Last Name Report for adding
    //      //3) IdentifyCandidate - No candidate selected. User is prompted to supply a last name.
    //      //
    //      //Either the EditCandidate or AddCandidate mode and PoliticianKey is part of the politician query string in both reports.
    //      #endregion Three modes when not postback

    //      string mode = string.Empty;
    //      if (!string.IsNullOrEmpty(GetQueryString("Mode")))
    //      {
    //        ViewState["Mode"] = GetQueryString("Mode");
    //        mode = GetQueryString("Mode");

    //        if (!string.IsNullOrEmpty(mode))
    //          ViewState["Mode"] = mode;
    //      }
    //      else
    //      {
    //        ViewState["Mode"] = "IdentifyCandidate";
    //      }
    //      #endregion mode

    //      if (ViewState["Mode"].ToString() == "AddCandidate")
    //        ElectionsPoliticians_INSERT();

    //      Page_Title();

    //      LabelIncumbents.Text = db.Office_Positions(ViewState["OfficeKey"].ToString()).ToString();

    //      Controls_Visible_And_Load();

    //      #region Msg
    //      switch (ViewState["Mode"].ToString())
    //      {
    //        case "AddCandidate":
    //          if (db.Is_Election_Previous(electionKey))
    //            Msg.Text = db.Warn("Candidates are normally NOT ADDED OR DELETED"
    //              + " for a previous election."
    //              + " If you choose to contine, "
    //              + " enter the last name of the candidate in the textbox.");
    //          else
    //            Msg.Text = db.Msg("To add a candidate for this office contest"
    //              + " enter the last name of the candidate in the textbox.");
    //          break;
    //        case "EditCandidate":
    //          Msg.Text = db.Msg("Use the various controls to add, change and delete information about this office contest.");
    //          break;
    //        case "IdentiyCandidate":
    //          Msg.Text = db.Msg("Use the various controls to add, change and delete information about this office contest.");
    //          break;
    //      }
    //      #endregion Msg
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

    protected override void OnInit(EventArgs e)
    {
      var stateCode = QueryState;
      var countyCode = QueryCounty;
      var localCode = QueryLocal;
      var electionKey = QueryElection;
      if (!string.IsNullOrWhiteSpace(electionKey))
      {
        stateCode = Elections.GetStateCodeFromKey(electionKey);
        countyCode = Elections.GetCountyCodeFromKey(electionKey);
        localCode = Elections.GetLocalCodeFromKey(electionKey);
      }
      LegacyRedirect(SecureAdminPage.GetUpdateElectionsPageUrl(stateCode,
        countyCode, localCode));
      base.OnInit(e);
    }

    #region Dead code

    //protected void OfficesOfficials_Delete_Insert_Log_Invalidate()
    //{
    //  string SQL = string.Empty;
    //  #region Checks
    //  Check_Valid_Politician();
    //  Check_Valid_Office();
    //  #endregion Checks

    //  #region Delete any then Insert OfficesOfficials row
    //  #region Delete
    //  SQL = "DELETE FROM OfficesOfficials";
    //  SQL += " WHERE OfficeKey = " + db.SQLLit(ViewState["OfficeKey"].ToString());
    //  SQL += " AND PoliticianKey = " + db.SQLLit(ViewState["PoliticianKey"].ToString());
    //  db.ExecuteSQL(SQL);

    //  db.Log_OfficesOfficials_Add_Or_Delete(
    //      "D"
    //    , ViewState["OfficeKey"].ToString()
    //    , ViewState["PoliticianKey"].ToString()
    //  );
    //  #endregion Delete

    //  #region Insert
    //  if (!db.Is_Valid_Office_Politician(
    //    ViewState["OfficeKey"].ToString()
    //    , ViewState["PoliticianKey"].ToString())
    //    )
    //  {
    //    #region No current ElectionsPoliticians row so insert one
    //    SQL = "INSERT INTO OfficesOfficials";
    //    SQL += "(";
    //    SQL += "OfficeKey";
    //    SQL += ",PoliticianKey";
    //    SQL += ",StateCode";
    //    SQL += ",CountyCode";
    //    SQL += ",LocalCode";
    //    SQL += ")";
    //    SQL += " VALUES";
    //    SQL += "(";
    //    SQL += db.SQLLit(ViewState["OfficeKey"].ToString());
    //    SQL += "," + db.SQLLit(ViewState["PoliticianKey"].ToString());
    //    SQL += "," + db.SQLLit(ViewState["StateCode"].ToString());
    //    SQL += "," + db.SQLLit(ViewState["CountyCode"].ToString());
    //    SQL += "," + db.SQLLit(ViewState["LocalCode"].ToString());
    //    SQL += ")";
    //    db.ExecuteSQL(SQL);
    //    #endregion No current ElectionsPoliticians row so insert one
    //  }

    //  db.Log_OfficesOfficials_Add_Or_Delete(
    //      "A"
    //    , ViewState["OfficeKey"].ToString()
    //    , ViewState["PoliticianKey"].ToString()
    //  );
    //  #endregion Insert
    //  #endregion Delete any then Insert OfficesOfficials row

    //  #region Update Politician office as incumbent
    //  db.Politicians_Update_Str(
    //    ViewState["PoliticianKey"].ToString()
    //  , "OfficeKey"
    //  , ViewState["OfficeKey"].ToString()
    //  );
    //  #endregion Update Politician office as incumbent

    //  #region Invalidate
    //  db.Invalidate_Office(ViewState["OfficeKey"].ToString());
    //  db.Invalidate_Politician(ViewState["PoliticianKey"].ToString());
    //  #endregion Invalidate
    //}

    //private void Check_Valid_Politician()
    //{
    //  if (!db.Is_Valid_Politician(ViewState["PoliticianKey"].ToString()))
    //    throw new ApplicationException("There is no politician with the ID:  "
    //      + ViewState["PoliticianKey"].ToString());
    //}
    //private void Check_Valid_Office()
    //{
    //  if (!db.Is_Valid_Office(ViewState["OfficeKey"].ToString()))
    //    throw new ApplicationException("There is no office with the ID: "
    //      + ViewState["OfficeKey"].ToString());
    //}

    #endregion Dead code


  }
}