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
using Vote.Reports;

namespace Vote.Admin
{
  public partial class OfficeWinners : VotePage
  {
    //#region Important Notes
    ////This form is used to set ElectionsPoliticians.IsWinner
    ////for ANY past election, whether it was the
    ////most previous election or not.
    ////
    ////It will also delete and insert 
    ////OfficesOfficials rows to correctly identify 
    ////the currently elected officials (incumbents)
    ////ONLY if the election is the most recent previous election.
    ////Hence, old previous elections do not effect the
    ////identity of the currently elected officials (incumbents).
    ////
    ////ElectionsPoliticians.IsIncumbent is NEVER set in this form.
    ////It is set when an election is created 
    ////in the OfficeCreate.aspx form.
    ////This is the only time we know whether the candidate
    ////was the incumbent for a particular election.
    ////
    ////At a future time a new OfficeIncumbent.aspx form
    ////may be developed to identify incumbents for 
    ////elections older than the most recent previous election.
    ////Currently there is nothing available to do this.
    //#endregion Important Notes

    //private int OfficePositions()
    //{
    //  return db.Office_Positions(ViewState["OfficeKey"].ToString());
    //}
    //private int CurrentElectedOfficials()
    //{
    //  return db.Office_ElectedOfficials(ViewState["OfficeKey"].ToString());
    //}
    //private int ElectionPositions()
    //{
    //  return db.Offices_Int(
    //          ViewState["OfficeKey"].ToString(),
    //          "ElectionPositions");
    //}
    //private int WinnersIdentified()
    //{
    //  string sql = string.Empty;
    //  sql += " vote.ElectionsPoliticians";
    //  sql += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //  sql += " AND OfficeKey = " + db.SQLLit(ViewState["OfficeKey"].ToString());
    //  sql += " AND IsWinner = 1";
    //  return db.Rows_Count_From(sql);
    //}

    //private bool Is_All_Winners_Identified()
    //{
    //  if (WinnersIdentified() >= ElectionPositions())
    //    return true;
    //  else
    //    return false;
    //}
    //private bool Is_All_Elected_Officials_Identified(string electionKey)
    //{
    //  if (
    //    (db.Is_Election_Previous_Most_Recent(electionKey))
    //    && (CurrentElectedOfficials() >= OfficePositions())
    //    )
    //    return true;
    //  else
    //    return false;
    //}
    //private bool Is_Candidate_Already_Selected_As_A_Winner(string politicianKey)
    //{
    //  bool isWinner = false;
    //  //if (!db.Is_Election_Upcoming(ElectionKey))
    //  //{
    //  string sql = string.Empty;
    //  sql += " ElectionsPoliticians";
    //  sql += " WHERE ElectionKey =" + db.SQLLit(ViewState["ElectionKey"].ToString());
    //  sql += " AND OfficeKey =" + db.SQLLit(ViewState["OfficeKey"].ToString());
    //  sql += " AND PoliticianKey =" + db.SQLLit(politicianKey);
    //  if (db.Rows_Count_From(sql) == 1)
    //  {
    //    isWinner = db.ElectionsPoliticians_Bool(
    //      ViewState["ElectionKey"].ToString(),
    //      ViewState["OfficeKey"].ToString(),
    //      politicianKey,
    //      "IsWinner"
    //      );
    //    //}
    //  }
    //  return isWinner;
    //}
    //private bool Is_Candidate_Already_Selected_As_A_Winner()
    //{
    //  return Is_Candidate_Already_Selected_As_A_Winner(ViewState["PoliticianKey"].ToString());
    //}

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

    //  #region Ballot Order
    //  if (isRunningMate)
    //    ballotOrder = "&nbsp";//need so td cell will have borders

    //  db.Add_Td_To_Tr(
    //      Html_Tr_Politician
    //      , ballotOrder
    //      , "tdReportDetail"
    //      );

    //  #endregion Ballot Order

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

    //  #region Headshot and Name Anchors

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
    //  //    , "_officewinner"
    //  //    , "EditCandidate"
    //  //    );
    //  if (!isRunningMate)
    //  {
    //    anchorName += db.Anchor_Admin_OfficeWinner(
    //        electionKey
    //        , officeKey
    //        , politicianName
    //        , "_officewinner"
    //        , politicianKey
    //        );
    //    //anchorName += db.Anchor_Admin_Office_W
    //  }
    //  else
    //    anchorName += politicianName;

    //  #endregion 2nd Name anchor

    //  if (isRunningMate)
    //    anchorName += "<br><small><small>(Running Mate)</small></small>";

    //  if (!isRunningMate)
    //  //winner indications for running mates are not shown on reports
    //  {
    //    if (Is_Candidate_Already_Selected_As_A_Winner(politicianKey))
    //    // electionKey,
    //    // officeKey,
    //    // politicianKey
    //    //)
    //    //)
    //    {
    //      anchorName += "<br><small><small><span style=color:red>(winner)</span></small></small>";
    //    }
    //  }
    //  db.Add_Td_Politician_Name(
    //    ref Html_Tr_Politician
    //    , db.ReportUser.Admin
    //    , anchorHeadshot + " " + anchorName
    //    );
    //  #endregion Headshot and Name Anchors
    //  //=============
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

    //  DataTable electionsPoliticiansTable = db.Table(sql_ElectionsPoliticians);

    //  #region OfficeHeading
    //  string officeHeading = string.Empty;

    //  #region Candidates for Office Title
    //  if (Elections.IsUSPresidentialPrimary(electionKey))
    //    officeHeading = "U.S. Presidential Candidates";
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

    //  #region Office Title as Anchor to Office.aspx

    //  officeHeading = db.Anchor_Admin_Office_UPDATE_Office(
    //    officeKey,
    //    officeHeading
    //    );

    //  #endregion Office Title as Anchor to Office.aspx

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
    //    , 6
    //    );
    //  #endregion Add HTML Office Heading Row

    //  #endregion OfficeHeading

    //  #region * = Incumbent ROW
    //  //if (db.Columns_Report_Election(db.ReportUser.Admin, electionKey) == db.Columns_With_BallotOrder_Or_Pic)
    //  //{
    //  HtmlTableRow keyHTMLRow = db.Add_Tr_To_Table_Return_Tr(
    //    htmlTable
    //    , "trReportIncumbentKeyHeading");
    //  db.Add_Td_To_Tr(
    //    keyHTMLRow
    //    , "* = Incumbent"
    //    , "tdReportIncumbentKeyHeading"
    //    , 6
    //    //, db.Columns_Report_Election(db.ReportUser.Admin, electionKey)
    //    );

    //  //db.Add_Td_To_Tr(ref KeyHTMLRow, "Order", "tdReportDetailHeading");
    //  //}
    //  #endregion * = Incumbent ROW

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

    //protected string Report_Winners_Office(
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
    //  html_Table_Office_Contest.Attributes["Class"] = "tableAdmin";
    //  #endregion Create HTML Candidates for Office report attributes

    //  # region commented SQL
    //  //string sqlText = string.Empty;
    //  //sqlText += " SELECT ";
    //  //sqlText += " ElectionsOffices.ElectionKey ";
    //  //sqlText += " ,Offices.OfficeKey ";
    //  //sqlText += " ,Offices.StateCode ";
    //  //sqlText += " ,Offices.OfficeLine1 ";
    //  //sqlText += " ,Offices.OfficeLine2 ";
    //  //sqlText += " ,Offices.OfficeLevel ";
    //  //sqlText += " ,Offices.IsRunningMateOffice ";
    //  //sqlText += " ,Offices.IsWinnerIdentified ";
    //  //sqlText += " FROM ElectionsOffices,Offices ";
    //  //sqlText += " WHERE ElectionsOffices.OfficeKey = Offices.OfficeKey";
    //  //sqlText += " AND ElectionsOffices.ElectionKey = " + db.SQLLit(electionKey);
    //  //sqlText += " AND Offices.OfficeKey = " + db.SQLLit(officeKey);
    //  //sqlText += " ORDER BY Offices.OfficeOrderWithinLevel, Offices.DistrictCode, Offices.DistrictCodeAlpha";
    //  //DataRow officeRow = db.Row_Optional(sqlText);
    //  ////if (officeRow != null)
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

    //private void Page_Title()
    //{
    //  string electionKey = ViewState["ElectionKey"].ToString();
    //  string officeKey = ViewState["OfficeKey"].ToString();

    //  PageTitle.Text = "Winners for ";

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

    //private void Table_Winners_Report_Visible_Load()
    //{
    //  LabelIncumbents.Text = db.Office_Positions(ViewState["OfficeKey"].ToString()).ToString();
    //  LabelElectionPositions.Text = db.Offices_Int(ViewState["OfficeKey"].ToString(), "ElectionPositions").ToString();

    //  HTMLTableWinnersForThisOffice.Text =
    //    Report_Winners_Office(
    //      PageCache,
    //      ViewState["ElectionKey"].ToString(),
    //      ViewState["OfficeKey"].ToString());
    //}

    //private void Table_Office_Holder_Being_Replaced_Visible_Load()
    //{
    //  if (!IsPostBack)
    //  {
    //    if (
    //      (db.Is_Election_Previous_Most_Recent(ViewState["ElectionKey"].ToString()))
    //      && (!string.IsNullOrEmpty(ViewState["PoliticianKey"].ToString()))
    //      && (Is_All_Elected_Officials_Identified(ViewState["ElectionKey"].ToString()))
    //      && (!Is_All_Winners_Identified())
    //      )
    //    {
    //      Table_Radio_Buttons_Winner_Replaced.Visible = true;

    //      RadioButtonListWinnerReplaced.Items.Clear();
    //      int Index = 0;

    //      #region Load radio buttons with current office holders excluding any candidate already identified as a winner
    //      string sql = " SELECT PoliticianKey FROM OfficesOfficials";
    //      sql += " WHERE OfficeKey = " + db.SQLLit(ViewState["OfficeKey"].ToString());
    //      DataTable tableOfficesOfficials = db.Table(sql);
    //      foreach (DataRow rowOfficesOfficials in tableOfficesOfficials.Rows)
    //      {
    //        //exclude any candidate already identified as a winner
    //        if (!Is_Candidate_Already_Selected_As_A_Winner(rowOfficesOfficials["PoliticianKey"].ToString()))
    //        {
    //          RadioButtonListWinnerReplaced.Items.Add(new ListItem());
    //          string nameIncumbent = Politicians.GetFormattedName(rowOfficesOfficials["PoliticianKey"].ToString());
    //          RadioButtonListWinnerReplaced.Items[Index].Text = nameIncumbent;
    //          RadioButtonListWinnerReplaced.Items[Index].Value = rowOfficesOfficials["PoliticianKey"].ToString();
    //          Index++;
    //        }
    //      }
    //      #endregion Load radio buttons with current office holders excluding any candidate already identified as a winner
    //    }
    //    else
    //    {
    //      Table_Radio_Buttons_Winner_Replaced.Visible = false;
    //    }
    //  }
    //  else
    //  {
    //    Table_Radio_Buttons_Winner_Replaced.Visible = false;
    //  }
    //}

    //private void Table_Current_Incumbent_Visible_Load()
    //{
    //  //if (!db.Is_Election_Previous_Most_Recent(ViewState["ElectionKey"].ToString()))
    //  //{
    //  //  //Table_Radio_Buttons_Winner_Replaced.Visible = false;
    //  //}
    //  //else
    //  //{
    //  Table_Report_Incumbents.Visible = true;

    //  Label_Office_Name.Text = Offices.FormatOfficeName(ViewState["OfficeKey"].ToString(), ", ");
    //  if (!db.Is_Election_Previous_Most_Recent(ViewState["ElectionKey"].ToString()))
    //  {
    //    Label_Office_Name.Text += "<br>Currently - Not at ";
    //    Label_Office_Name.Text += db.Elections_ElectionDesc(ViewState["ElectionKey"].ToString());
    //  }


    //  //IncumbentTable.Text = db.Report_Incumbents(ViewState["OfficeKey"].ToString());
    //  IncumbentsReport.GetReport(ViewState["OfficeKey"].ToString()).
    //    AddTo(IncumbentsReportPlaceHolder);

    //  //}
    //}

    //private void Table_Edit_Office_Visible_Load()
    //{
    //  Table_Edit_Office.Visible = true;

    //  string officeKey = ViewState["OfficeKey"].ToString();

    //  HyperLinkOffice.Text = "Edit ";
    //  HyperLinkOffice.Text += Offices.FormatOfficeName(officeKey);
    //  HyperLinkOffice.NavigateUrl = db.Url_Admin_Office_UPDATE(officeKey);
    //}

    //private void Table_Edit_Office_Contest_Candidates_Visible_Load()
    //{
    //  Table_Change_Office_Contest_Candidates.Visible = true;
    //  HyperLinkChangeCandidates.NavigateUrl = db.Url_Admin_OfficeContest(
    //    ViewState["ElectionKey"].ToString(),
    //    ViewState["OfficeKey"].ToString()
    //    );
    //  HyperLinkChangeCandidates.Text = "Add or Delete Candidates for ";
    //  HyperLinkChangeCandidates.Text += Offices.FormatOfficeName(ViewState["OfficeKey"].ToString())
    //    + " Office Contest";
    //}

    //private void Controls_Visible_And_Load()
    //{
    //  //Winner(s) for this Office Contest 
    //  Table_Winners_Report_Visible_Load();

    //  //Office Holder being Replced by Winner  
    //  Table_Office_Holder_Being_Replaced_Visible_Load();

    //  //Current Incumbent(s) for Office
    //  Table_Current_Incumbent_Visible_Load();

    //  //Edit Office
    //  Table_Edit_Office_Visible_Load();

    //  //Edit Office Contest Candidates
    //  Table_Edit_Office_Contest_Candidates_Visible_Load();
    //}

    //protected void RadioButtonListWinnerReplaced_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  #region note
    //  //Replacing incumbent with winners only applies of it was the most recent previous election.
    //  //
    //  //Radio buttons are presented when there are 2 or more office positions for an office.

    //  //If the elected officials (OfficesOfficials row for office) is less than the office positions
    //  //the first radio button allows the addition of the candidate WITHOUT replacing an elected official

    //  //The remaining radio buttons are the names of the currently elected officials
    //  //excludind any candidate already identified as a winner
    //  //and excluding the candidate just selected as a winner 
    //  #endregion note

    //  try
    //  {
    //    //Table_Radio_Buttons_Winner_Replaced.Visible = true;

    //    string electionKey = ViewState["ElectionKey"].ToString();
    //    string officeKey = ViewState["OfficeKey"].ToString();
    //    string politicianKey = ViewState["PoliticianKey"].ToString();


    //    db.Set_IsWinner_For_Election(
    //      electionKey,
    //      officeKey,
    //      politicianKey,
    //      true
    //      );

    //    if (db.Is_Election_Previous_Most_Recent(electionKey))
    //    {
    //      #region Delete the selected office holder to be replaced
    //      db.OfficesOfficials_Delete(
    //        officeKey,
    //        RadioButtonListWinnerReplaced.SelectedValue
    //        );
    //      #endregion Delete the selected office holder to be replaced
    //      //}

    //      #region Insert new elected office holder
    //      db.OfficesOfficials_INSERT(
    //        officeKey,
    //        politicianKey
    //        );
    //      #endregion Insert new elected office holder
    //    }

    //    #region Record whether all the winners for all the office positions have been identified
    //    db.Update_ElectionsOffices_Is_All_Winners_Identified(
    //      electionKey,
    //      officeKey
    //      );
    //    #endregion Record whether all the winners for all the office positions have been identified

    //    #region Msg
    //    string msg = string.Empty;
    //    //if (RadioButtonListWinnerReplaced.SelectedValue == "Add")
    //    //{
    //    //  msg = db.GetPoliticianName(politicianKey)
    //    //    + " has been recorded as the winner for this office."
    //    //    + " No elected official was replaced.";
    //    //}
    //    //else
    //    //{
    //    msg = Politicians.GetFormattedName(politicianKey)
    //      + " has been recorded as the winner and new elected official for this office replacing "
    //      + Politicians.GetFormattedName(RadioButtonListWinnerReplaced.SelectedValue) + ".";
    //    //if (politicianKey == RadioButtonListWinnerReplaced.SelectedValue)
    //    //  msg += " and was the incumbdent.";
    //    //else
    //    //  msg += " and was NOT the incumbent.";
    //    //}

    //    //int winnersIdentified = Winners_Identified(
    //    //  electionKey,
    //    //  officeKey);
    //    //int currentElectedOfficials = db.Office_ElectedOfficials(officeKey);
    //    //int officePositions = db.Office_Positions(officeKey);
    //    //int electionPositions = db.Offices_Int(officeKey, "ElectionPositions");
    //    //if (winnersIdentified == officePositions)
    //    if (WinnersIdentified() == ElectionPositions())
    //      msg += " All the current elected officials appear to be identified."
    //          + " PROCEED TO THE NEXT OFFICE CONTEST!";
    //    else
    //    {
    //      msg += " There are " + ElectionPositions().ToString()
    //        + " positions to filled in the election";
    //      msg += " but only " + WinnersIdentified().ToString()
    //        + " winners have been identified.";
    //      msg += " ADDIIONAL WINNER(S) MAY NEED TO BE IDENTIFIED!";
    //    }
    //    Msg.Text = db.Ok(msg);
    //    #endregion Msg

    //    Controls_Visible_And_Load();

    //    //Table_Radio_Buttons_Winner_Replaced.Visible = false;
    //    //}
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
    //      if (db.Is_Election_Upcoming(electionKey))
    //        throw new ApplicationException("Winner(s) can not be identified for an upcoming election.");
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
    //      #endregion ViewState inits & checks

    //      #region commented mode

    //      //#region Three modes when not postback
    //      ////1) EditCandidate - A candidate was selected in the report of candidates for editing
    //      ////2) AddCandidate - A politician with the same last name was selected from Candidates with Same Last Name Report for adding
    //      ////3) IdentifyCandidate - No candidate selected. User is prompted to supply a last name.
    //      ////
    //      ////Either the EditCandidate or AddCandidate mode and PoliticianKey is part of the politician query string in both reports.
    //      //#endregion Three modes when not postback

    //      //string mode = string.Empty;
    //      ////if (!string.IsNullOrEmpty(db.QueryString("Mode")))
    //      ////{
    //      ////  ViewState["Mode"] = db.QueryString("Mode").Trim();
    //      ////  mode = db.QueryString("Mode").Trim();

    //      ////  if (!string.IsNullOrEmpty(mode))
    //      ////    ViewState["Mode"] = mode;
    //      ////}
    //      ////else
    //      ////{
    //      ////  ViewState["Mode"] = "IdentifyCandidate";
    //      ////}
    //      #endregion commented mode

    //      Page_Title();

    //      if (string.IsNullOrEmpty(politicianKey))
    //      {
    //        #region Msg to select winner

    //        //Table_Radio_Buttons_Winner_Replaced.Visible = false;

    //        Msg.Text = db.Msg("Select a candidate NAME (not picture) as the winner of this office contest"
    //          + " or select a candidate who is identified as a winner to remove him or her as the winner.");
    //        #endregion Msg to select winner
    //      }
    //      else
    //      {
    //        #region A candidate has been selected as winner or remove as winner
    //        if (Is_Candidate_Already_Selected_As_A_Winner())
    //        {
    //          #region remove winner - selected candidate has been previously selected as winner

    //          //Table_Radio_Buttons_Winner_Replaced.Visible = false;

    //          #region remove (winner) status of selected candidate
    //          db.Set_IsWinner_For_Election(
    //            electionKey,
    //            officeKey,
    //            politicianKey,
    //            false
    //            );
    //          #endregion remove (winner) status of selected candidate

    //          if (db.Is_Election_Previous_Most_Recent(electionKey))
    //          {
    //            #region remove this current elected official only for the most recent previous election
    //            db.OfficesOfficials_Delete(
    //              officeKey,
    //              politicianKey
    //              );
    //            #endregion remove this current elected official only for the most recent previous election

    //            #region Msg
    //            Msg.Text = db.Ok(Politicians.GetFormattedName(politicianKey)
    //              + " has been removed as a winner for this office contest"
    //              + " and current elected official."
    //              );
    //            #endregion Msg
    //          }
    //          else
    //          {
    //            #region Msg
    //            Msg.Text = db.Ok(Politicians.GetFormattedName(politicianKey)
    //              + " has been removed as a winner for this office contest"
    //              + " but NOT as an elected official."
    //              );
    //            #endregion Msg
    //          }

    //          #endregion remove winner - selected candidate has been previously selected as winner
    //        }
    //        else if (Is_All_Winners_Identified())
    //        {
    //          #region All positions filled
    //          //the current selected winner(s) plus incumbents - can't identify another winner

    //          //Table_Radio_Buttons_Winner_Replaced.Visible = false;

    //          Msg.Text = db.Fail(Politicians.GetFormattedName(politicianKey)
    //           + " can not be identified as a winner"
    //           + " because the winner(s) for the election positions up for election"
    //           + " have been identified."
    //           + " You need to remove a candidate"
    //           + " who is identified as (winner) before you can identify"
    //           + " a different candidate as a winner.");
    //          #endregion All positions filled
    //        }
    //        else if (
    //                 (db.Is_Election_Previous_Most_Recent(electionKey))
    //                 && (db.Is_Incumbent(officeKey, politicianKey))
    //                )
    //        {
    //          #region Only set winner flage because incumbent is already defined in OfficesOfficials Table
    //          db.Set_IsWinner_For_Election(
    //            electionKey,
    //            officeKey,
    //            politicianKey,
    //            true
    //            );
    //          Msg.Text = db.Ok(Politicians.GetFormattedName(politicianKey)
    //           + " has been recorded as the winner and remains incumbent for this office."
    //           );
    //          #endregion Only set winner flage because incumbent is already defined in OfficesOfficials Table
    //        }
    //        else
    //        {
    //          if (
    //            (!Is_All_Winners_Identified())
    //            && (!Is_All_Elected_Officials_Identified(electionKey))
    //          )
    //          {
    //            #region Set winner and elected official if recent election
    //            db.Set_IsWinner_For_Election(
    //              electionKey,
    //              officeKey,
    //              politicianKey,
    //              true
    //              );

    //            if (db.Is_Election_Previous_Most_Recent(electionKey))
    //            {
    //              #region update elected officials (incumbent) if most recent election

    //              db.OfficesOfficials_INSERT(
    //                officeKey,
    //                politicianKey
    //                );

    //              Msg.Text = db.Ok(Politicians.GetFormattedName(politicianKey)
    //                + " has been recorded as the winner and the new elected official (incumbent) for this office."
    //                );
    //              #endregion update elected officials (incumbent) if most recent election
    //            }
    //            else
    //            {
    //              #region don't update elected officials
    //              Msg.Text = db.Ok(Politicians.GetFormattedName(politicianKey)
    //                + " has been recorded as the winner BUT the elected official (incumbent) was not changed"
    //                + " because this was not the most recent previous election."
    //                );
    //              #endregion don't update elected officials
    //            }

    //            #region Record whether all the winners for all the office positions have been identified
    //            db.Update_ElectionsOffices_Is_All_Winners_Identified(
    //              electionKey,
    //              officeKey
    //              );
    //            #endregion Record whether all the winners for all the office positions have been identified

    //            #endregion Set winner and elected official if recent election
    //          }
    //          else
    //          {
    //            #region no vacant positions need to choose who is being replaced - show radio buttons of office holders

    //            Msg.Text = db.Fail(Politicians.GetFormattedName(politicianKey)
    //              + " was NOT recorded as the winner"
    //              + " because there is more than one office position and there are no vacant positions."
    //              + " You need to identify a current office holder to be replaced by the winner."
    //              + " Please select one of the radio buttons of the current office holder"
    //              + " to be replaced.");
    //            //}
    //            #endregion no vacant positions need to choose who is being replaced - show radio buttons of office holders
    //          }
    //        }
    //        #endregion A candidate has been selected as winner or remove as winner
    //      }

    //      Controls_Visible_And_Load();
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
      LegacyRedirect(SecureAdminPage.GetUpdateElectionsPageUrl(QueryState,
        QueryCounty, QueryLocal));
      base.OnInit(e);
    }
  }
}