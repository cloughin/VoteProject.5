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
  public partial class ElectionPage : VotePage
  {
    //#region Notes
    ////Controls visible for State ADMIN or MASTER users vs. COUNTY users:
    ////if (
    ////  (SecurePage.IsMasterUser)
    ////  || (db.User_Security() == db.User_State)
    ////  )
    ////------------------------
    ////Some control values are set depending on whether a State or County ELECTION DATA is being processed using:
    ////if (ViewState["CountyCode"].ToString() == string.Empty) //for State ELECTION DATA
    //#endregion Notes

    //#region SQL

    //#region Select
    //protected string sqlElectionsOffices4ElectionKey()
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " ElectionsOffices.ElectionKey ";
    //  SQL += " ,ElectionsOffices.ElectionKeyFederal ";
    //  SQL += " ,ElectionsOffices.OfficeKey ";
    //  SQL += " FROM ElectionsOffices ";
    //  SQL += " WHERE ElectionKey = " + db.SQLLit(TextboxElectionKey.Text.Trim());
    //  SQL += " AND CountyCode != ''";
    //  return SQL;
    //}
    //protected string sqlElectionsPoliticians4ElectionKey()
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " ElectionKey";
    //  SQL += " FROM ElectionsPoliticians ";
    //  SQL += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //  return SQL;
    //}
    //protected string sqlElectionsPoliticians4ElectionKeyOfficeKey(string ElectionKey_Electoral, string OfficeKey_Electoral)
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " ElectionsPoliticians.Election";
    //  SQL += " ,ElectionsPoliticians.ElectionKey";
    //  SQL += " ,ElectionsPoliticians.ElectionKeyState";
    //  SQL += " ,ElectionsPoliticians.ElectionKeyFederal";
    //  SQL += " ,ElectionsPoliticians.PoliticianKey";
    //  SQL += " ,ElectionsPoliticians.RunningMateKey";
    //  SQL += " ,ElectionsPoliticians.StateCode";
    //  SQL += " ,ElectionsPoliticians.OfficeKey";
    //  SQL += " ,ElectionsPoliticians.OrderOnBallot";
    //  SQL += " FROM ElectionsPoliticians ";
    //  SQL += " WHERE ElectionsPoliticians.ElectionKey = " + db.SQLLit(ElectionKey_Electoral);
    //  SQL += " AND ElectionsPoliticians.OfficeKey = " + db.SQLLit(OfficeKey_Electoral);
    //  return SQL;
    //}
    //#endregion Select
    //#endregion SQL

    //private void Page_Title()
    //{
    //  PageTitle.Text = string.Empty;

    //  #region commented
    //  //if (ViewState["StateCode"].ToString() == "US")
    //  //{
    //  //  PageTitle.Text += "National Presidential Comparisons";
    //  //}
    //  //else
    //  //{
    //  //  PageTitle.Text += db.Name_Electoral(
    //  //     ViewState["StateCode"].ToString()
    //  //    , ViewState["CountyCode"].ToString()
    //  //    , ViewState["LocalCode"].ToString()
    //  //    , true
    //  //    );
    //  //}
    //  #endregion commented

    //  if (!db.Is_StateCode_National_Party_Contest(ViewState["StateCode"].ToString()))
    //  {
    //    PageTitle.Text += Offices.GetElectoralClassDescription(
    //       ViewState["StateCode"].ToString()
    //      , ViewState["CountyCode"].ToString()
    //      , ViewState["LocalCode"].ToString());
    //    PageTitle.Text += "<br>";
    //  }

    //  if (db.Is_Valid_Election(ViewState["ElectionKey"].ToString()))
    //    PageTitle.Text += db.PageTitle_Election(
    //      ViewState["ElectionKey"].ToString());
    //  else
    //    PageTitle.Text += db.PageTitle_Election(
    //      db.ElectionKey_State(ViewState["ElectionKey"].ToString()));
    //}

    //private void Visible_Controls()
    //{
    //  #region Nothing Visible
    //  TableElectionMaintenance.Visible = false;
    //  TableWinners.Visible = false;
    //  TableIncumbents.Visible = false;
    //  TableEmails.Visible = false;
    //  TableCountyLinks.Visible = false;
    //  TableLocalLinks.Visible = false;

    //  TableElectionInfo.Visible = false;
    //  TableViewingStatus.Visible = false;

    //  TrPoliticianData.Visible = false;
    //  TableCountyElectionLinks.Visible = false;
    //  TableLocalElectionLinks.Visible = false;
    //  //LabelElectionReport.Visible = false;
    //  ReportPlaceHolder.Visible = false;

    //  //Master controls
    //  TableMiscOperations1.Visible = false;
    //  TableMiscOperations2.Visible = false;
    //  TableElectionStatus.Visible = false;
    //  TableDelete4Master.Visible = false;
    //  #endregion Nothing Visible

    //  #region Help links
    //  if (SecurePage.IsMasterUser)
    //  {
    //    HyperLink_Interns.Visible = true;
    //    HyperLink_Help.Visible = false;
    //  }
    //  else
    //  {
    //    HyperLink_Help.Visible = true;
    //    HyperLink_Interns.Visible = false;
    //  }
    //  #endregion Help links

    //  #region Offices Referendums Election Results
    //  switch (db.Electoral_Class(
    //         ViewState["StateCode"].ToString()
    //        , ViewState["CountyCode"].ToString()
    //        , ViewState["LocalCode"].ToString()
    //   ))
    //  {
    //    case db.ElectoralClass.State:
    //      TableElectionMaintenance.Visible = true;
    //      break;
    //    case db.ElectoralClass.County:
    //      TableElectionMaintenance.Visible = true;
    //      break;
    //    case db.ElectoralClass.Local:
    //      TableElectionMaintenance.Visible = true;
    //      break;
    //    default:
    //      if (QueryState == "U1")
    //        //Special case: Comparing Presidential Candidates
    //        TableElectionMaintenance.Visible = true;
    //      else
    //        TableElectionMaintenance.Visible = false;
    //      break;
    //  }
    //  if (TableElectionMaintenance.Visible)
    //  {
    //    #region HyperLinks
    //    #region commented out Office Categories
    //    //HyperLinkElectionCategories.NavigateUrl = db.Url_Admin_ElectionCategories(db.QueryString("Election"));
    //    //HyperLinkElectionCategories.Text = db.Name_Electoral_Plus_Text_ElectionCategories();
    //    #endregion commented out Office Categories

    //    HyperLinkElectionOffices.NavigateUrl =
    //      db.Url_Admin_Election_Offices(QueryElection);
    //    HyperLinkElectionOffices.Text =
    //      //db.Name_Electoral_Plus_Text_ElectionOffices();
    //      //db.Name_Electoral_Plus_Text(
    //      //  "Offices Contests in this "
    //      //, " Election");
    //      db.Name_Electoral_Plus_Text(
    //        "Add or Delete Offices Contests in this "
    //      , " Election");

    //    HyperLinkReferendums.NavigateUrl =
    //      db.Ur4AdminReferendums(QueryElection);
    //    HyperLinkReferendums.Text =
    //      //db.Name_Electoral_Plus_Text_Referendum();
    //      db.Name_Electoral_Plus_Text(
    //        "Add or Delete Ballot Measures in this "
    //        , " Election");

    //    //HyperLinkElectionEdit.NavigateUrl = db.Url_Admin_ElectionReport(db.QueryString("Election"));
    //    //HyperLinkElectionEdit.Text = db.Name_Electoral_Plus_Text_Candidates();

    //    HyperLinkElectionResults.NavigateUrl = db.Url_Admin_Officials();
    //    HyperLinkElectionResults.Text = "Report of Elected Officials (Incumbents)";
    //    //db.Name_Electoral_Plus_Text_ElectionResults();
    //    //db.Name_Electoral_Plus_Text(
    //    //    "Incumbents or Winners in this "
    //    //    , " Election");

    //    Hyperlink_Send_Emails.NavigateUrl = db.Url_Admin_Emails(
    //      db.State_Code(),
    //      ViewState["ElectionKey"].ToString()
    //      );
    //    #endregion HyperLinks
    //  }
    //  #endregion Offices Referendums Election Results

    //  var electionKey = ViewState["ElectionKey"].ToString();
    //  if (
    //    (db.Is_Valid_Election(db.ElectionKey_State(ViewState["ElectionKey"].ToString())))
    //    //&& (SecurePage.IsSuperUser)
    //    )
    //  {
    //    #region note
    //    //There must be a valid State Election
    //    //even though no county or local elections exist.
    //    //
    //    //County and local elections are created on the fly
    //    //the first time an election contest is identified.
    //    #endregion note

    //    #region TableElectionInfo
    //    //if (db.Electoral_Class(
    //    //       ViewState["StateCode"].ToString()
    //    //      , ViewState["CountyCode"].ToString()
    //    //      , ViewState["LocalCode"].ToString()
    //    //         ) == db.Electoral.State)
    //    //{
    //    //if (
    //    //  (!db.Is_StateCode_State_By_State(ViewState["StateCode"].ToString()))
    //    //  && (!db.Is_StateCode_National_Party_Contest(ViewState["StateCode"].ToString()))
    //    //  )
    //    if (!db.Is_StateCode_State_By_State(ViewState["StateCode"].ToString()))
    //    {
    //      TableIncumbents.Visible = true;

    //      TableElectionInfo.Visible = true;
    //      TextBoxElectionStatus.Text = db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionStatus");
    //      TextboxElectionDesc.Text = db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionDesc");
    //      LabelCurrentTitle.Text = db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionDesc");
    //      //LabelCurrentDate.Text = db.Elections_Date(ViewState["ElectionKey"].ToString(), "ElectionDate").ToString("MM/dd/yy");
    //      LabelCurrentDate.Text = db.Elections_Date(ViewState["ElectionKey"].ToString(), "ElectionDate").ToString("M/d/yy");
    //      LabelCurrentElectionKey.Text = ViewState["ElectionKey"].ToString();
    //      TextboxElectionInfo.Text = db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionAdditionalInfo");
    //      TextboxBallotInstructions.Text = db.Elections_Str(ViewState["ElectionKey"].ToString(), "BallotInstructions");

    //      TextBoxElectionOrder.Text = db.Elections_Int(ViewState["ElectionKey"].ToString(), "ElectionOrder").ToString();

    //      ElectionResultsSource.Text = db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionResultsSource");
    //      //DateTime ResultsDate = db.Report_Election_Get_LastUpdate_Date(ViewState["ElectionKey"].ToString());
    //      //if (ResultsDate != Convert.ToDateTime("1900/01/01"))
    //      //  ElectionResultsDate.Text = ResultsDate.ToString("MM/dd/yyyy");
    //      //else
    //        ElectionResultsDate.Text = string.Empty;
    //    }
    //    #endregion TableElectionInfo

    //    #region TableWinners
    //    //The election has to be a previous election
    //    //but does not have to be the most previous election
    //    //because the winners of ANY previous election can be identified
    //    //even though this will probably never be done.
    //    if (!db.Is_Election_Upcoming(ViewState["ElectionKey"].ToString()))
    //    {
    //      TableWinners.Visible = true;
    //      CheckBoxElection_Winners_Status_Set();

    //      if (db.Referendums_In_Election(ViewState["ElectionKey"].ToString()) > 0)
    //      {
    //        tr_BallotMeasures.Visible = true;
    //        HyperLinkBallotMeasures.NavigateUrl =
    //          "/Admin/Referendum.aspx?Election=" + ViewState["ElectionKey"].ToString();
    //        HyperLinkBallotMeasures.Target = "_politician";
    //      }
    //      else
    //      {
    //        tr_BallotMeasures.Visible = false;
    //      }
    //    }
    //    #endregion TableWinners

    //    #region Tables Always Shown

    //    #region TableViewingStatus

    //    if (!(Elections.IsCountyElection(electionKey) ||
    //      Elections.IsLocalElection(electionKey)))
    //    {
    //      TableViewingStatus.Visible = true;
    //      CheckBox_Election_Viewable_Status_Set();
    //    }
    //    #endregion TableViewingStatus

    //    //#region Update or Refresh Report Buttons
    //    //if (
    //    //    (!db.Is_Report_Current_Election(
    //    //        ViewState["ElectionKey"].ToString()))
    //    //    || (SecurePage.IsMasterUser)
    //    //  )
    //    //{
    //    //  UpdateReportTable.Visible = true;
    //    //  RefreshReportTable.Visible = false;
    //    //}
    //    //else
    //    //{
    //    //  RefreshReportTable.Visible = true;
    //    //  UpdateReportTable.Visible = false;
    //    //}
    //    //#endregion Update or Refresh Report Buttons

    //    #region Politician Data Instructions
    //    if (
    //      (SecurePage.IsMasterUser)
    //      || (db.Is_User_Admin_DataEntry())//Interns
    //      )
    //      TrPoliticianData.Visible = true;
    //    #endregion Politician Data Instructions

    //    #region LabelElectionReport
    //    //LabelElectionReport.Visible = true;
    //    ReportPlaceHolder.Visible = true;
    //    ShowElectionReport();
    //    #endregion LabelElectionReport

    //    #region County and Local Election Links Msg Row
    //    //if (db.Is_Has_Rows_ElectionsOffices_County_Or_Local(ViewState["ElectionKey"].ToString()))
    //    //  trCountyLocalElectionLinks.Visible = true;
    //    #endregion County and Local Election Links Msg Row

    //    #endregion Tables Always Shown

    //    #region County Election Links
    //    string SQL = "ElectionsOffices WHERE ElectionKeyState = "
    //      + db.SQLLit(db.ElectionKey_State(ViewState["ElectionKey"].ToString()));
    //    SQL += " AND CountyCode !=''";

    //    int County_Contests = db.Rows_Count_From(SQL);
    //    if (County_Contests > 0)
    //    {
    //      TableCountyElectionLinks.Visible = true;

    //      //LabelCountyElectionLinks.Text = db.County_Links(
    //      //    db.Anchor_For.Admin_Election
    //      //  , db.ElectionKey_State(ViewState["ElectionKey"].ToString())
    //      //);
    //      LabelCountyElectionLinks.Text =
    //        CountyLinks.GetElectionCountyLinks(ViewState["ElectionKey"].ToString())
    //        .RenderToString();
    //    }
    //    #endregion County Election Links

    //    #region Local Election Links
    //    SQL = "ElectionsOffices WHERE ElectionKeyState = "
    //      + db.SQLLit(db.ElectionKey_State(ViewState["ElectionKey"].ToString()));
    //    SQL += " AND LocalCode !=''";

    //    int Local_Contests = db.Rows_Count_From(SQL);
    //    if (Local_Contests > 0)
    //    {
    //      TableLocalElectionLinks.Visible = true;

    //      //LabelLocalElectionLinks.Text = db.Local_Links(
    //      //  db.Sort_Order.Name
    //      //  , db.Anchor_For.Admin_Election
    //      //  , Elections.GetStateCodeFromKey(ViewState["ElectionKey"].ToString())
    //      //  , Elections.GetCountyCodeFromKey(ViewState["ElectionKey"].ToString())
    //      //  , db.ElectionKey_State(ViewState["ElectionKey"].ToString())
    //      //);
    //      LabelLocalElectionLinks.Text =
    //        LocalLinks.GetElectionLocalLinks(
    //          ViewState["ElectionKey"].ToString()).RenderToString();
    //    }
    //    #endregion Local Election Links
    //  }

    //  #region Controls for MASTER user only

    //  if (db.Is_Valid_Election(electionKey) && SecurePage.IsSuperUser)
    //  {
    //    if (!(Elections.IsCountyElection(electionKey) ||
    //      Elections.IsLocalElection(electionKey)))
    //    {
    //      TableEmails.Visible = true;
    //    }
    //    TableMiscOperations1.Visible = true;
    //    TableMiscOperations2.Visible = true;
    //    TableElectionStatus.Visible = true;

    //    #region HyperLinks for Master User
    //    //HyperlinkEmails.NavigateUrl = "/Admin/Emails.aspx?Election="
    //    //  + db.ElectionKey_State(ViewState["ElectionKey"].ToString());
    //    HyperlinkDataFile.NavigateUrl = "/Master/WordTextFile4Letters.aspx?Election="
    //      + db.ElectionKey_State(electionKey);


    //    HyperLinkCandidateNames.NavigateUrl = "/Master/MgtReports.aspx?Report=ElectionNames&Election="
    //      + db.ElectionKey_State(ViewState["ElectionKey"].ToString());
    //    #endregion HyperLinks

    //    #region Delete Election
    //    if (
    //      (SecurePage.IsSuperUser)
    //      && (db.Master_Bool("IsElectionDeletionPermitted"))
    //      )
    //      TableDelete4Master.Visible = true;
    //    #endregion Delete Election
    //  }

    //  #endregion Controls for MASTER user only
    //}

    //private void ShowElectionReport()
    //{
    //  var electionKey = ViewState["ElectionKey"].ToString();
    //  ElectionReport.GetReport(Report.SignedInReportUser, electionKey)
    //    .AddTo(ReportPlaceHolder);
    //}

    //#region Buttons & CheckBoxes

    //protected void ButtonRecordElectionGraphic_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    #region Check TextBoxes for Illegal Stuff
    //    db.Throw_Exception_TextBox_Html_Or_Script(ElectionResultsSource);
    //    db.Throw_Exception_TextBox_Html_Or_Script(ElectionResultsDate);
    //    #endregion Check TextBoxes for Illegal Stuff

    //    #region Update
    //    string UpdateSQL = string.Empty;
    //    UpdateSQL = "UPDATE Elections SET "
    //      + " ElectionResultsSource = " + db.SQLLit(ElectionResultsSource.Text.Trim())
    //      + ",ElectionResultsDate = " + db.SQLLit(ElectionResultsDate.Text.Trim())
    //      + " WHERE ElectionKey= " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //    db.ExecuteSQL(UpdateSQL);
    //    //db.Invalidate_Election(ViewState["ElectionKey"].ToString());
    //    #endregion

    //    //LoadElectionDataInTextBoxes();
    //    Visible_Controls();

    //    Msg.Text = db.Ok("The Graphic of Election Results has been recorded.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void ButtonRecordStatus_Click(object sender, EventArgs e)
    //{
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxElectionStatus);

    //  string UpdateSQL = "UPDATE Elections SET "
    //+ " ElectionStatus = " + db.SQLLit(TextBoxElectionStatus.Text.Trim())
    //+ " WHERE ElectionKey= " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //  db.ExecuteSQL(UpdateSQL);

    //  TextBoxElectionStatus.Text = db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionStatus");

    //  Msg.Text = db.Ok("The Election Status for Vote-USA has been recorded.");

    //}

    //protected void ButtonDelete_Click1(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    //if (
    //    //  (db.User_Name().ToLower() != "ron")
    //    //  || (!db.Master_Bool("IsElectionDeletionPermitted"))
    //    //  )
    //    if (Elections.GetElectionTypeFromKey(ViewState["ElectionKey"].ToString()) == db.Election_Type_StateGeneral_G)
    //    {
    //      throw new ApplicationException("A General Election can not be deleted because there is no way to create a single State election.");
    //    }
    //    if (
    //      (db.Is_Valid_Election(ViewState["ElectionKey"].ToString()))
    //      && (!SecurePage.IsSuperUser)
    //      )
    //      throw new ApplicationException("You are not the special Master user or deletions of elections are not permitted.");
    //    else if (Elections.IsCountyElection(ViewState["ElectionKey"].ToString()))
    //    {
    //      throw new ApplicationException("This is a county election. You can only delete State elections.");
    //    }
    //    else
    //    {
    //      #region DELETE Elections, ElectionsOffices rows for State Election
    //      string SQL = string.Empty;
    //      SQL = "DELETE FROM Elections";
    //      SQL += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //      db.ExecuteSQL(SQL);

    //      SQL = "DELETE FROM ElectionsOffices";
    //      SQL += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //      db.ExecuteSQL(SQL);
    //      #endregion DELETE Elections, ElectionsOffices rows for State Election

    //      if (CheckBoxDeletePoliticiansReferendums.Checked)
    //      {
    //        SQL = "DELETE FROM ElectionsPoliticians";
    //        SQL += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //        db.ExecuteSQL(SQL);

    //        SQL = "DELETE FROM Referendums";
    //        SQL += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //        db.ExecuteSQL(SQL);

    //        //db.Invalidate_Election(ViewState["ElectionKey"].ToString());
    //      }


    //      #region  Hide All Controls
    //      //CheckBoxListOfficesInElection.Visible = false;
    //      TableElectionMaintenance.Visible = false;
    //      //TableCountyLinks4State.Visible = false;
    //      //HyperLinkStateOffices.Visible = false;
    //      //HyperLinkCountyOffices.Visible = false;
    //      //HyperLinkEditElection.Visible = false;
    //      //HyperLinkEditElectionCounties.Visible = false;
    //      //HyperLinkStateReferendums.Visible = false;
    //      //HyperLinkCountyReferendums.Visible = false;
    //      TextboxElectionDesc.Visible = false;
    //      TextboxElectionInfo.Visible = false;
    //      TextboxBallotInstructions.Visible = false;
    //      //HyperLinkResultsState.Visible = false;
    //      TableMiscOperations1.Visible = false;
    //      TableMiscOperations2.Visible = false;
    //      TableDelete4Master.Visible = false;
    //      TableElectionStatus.Visible = false;
    //      #endregion  Hide All Controls

    //      Msg.Text = db.Ok("This Election has been deleted. You need to return to the State Home Page to do any additional work.");
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void AddElectionsPoliticiansRow(
    //  string ElectionKey_Electoral
    //  , string PoliticianKey_Electoral
    //  , string RunningMateKey_Electoral
    //  , string OfficeKey_Electoral
    //  )
    //{
    //  #region replaced
    //  //string SQL = "INSERT INTO ElectionsPoliticians";
    //  //SQL += "(";
    //  ////SQL += "Election";
    //  //SQL += " ElectionKey";
    //  //SQL += ",ElectionKeyFederal";
    //  //SQL += ",PoliticianKey";
    //  //SQL += ",RunningMateKey";
    //  //SQL += ",StateCode";
    //  //SQL += ",CountyCode";
    //  //SQL += ",OfficeKey";
    //  //SQL += ")";
    //  //SQL += " VALUES(";
    //  ////SQL += db.SQLLit(db.Election4ElectionKey(ElectionKey_Electoral));
    //  //SQL += db.SQLLit(ElectionKey_Electoral);
    //  //SQL += "," + db.SQLLit(db.ElectionKey_Federal(ElectionKey_Electoral, OfficeKey_Electoral));
    //  //SQL += "," + db.SQLLit(PoliticianKey_Electoral);
    //  //SQL += "," + db.SQLLit(RunningMateKey_Electoral);
    //  //SQL += "," + db.SQLLit(ViewState["StateCode"].ToString());
    //  //SQL += "," + db.SQLLit(ViewState["CountyCode"].ToString());
    //  //SQL += "," + db.SQLLit(OfficeKey_Electoral);
    //  //SQL += ")";
    //  //db.ExecuteSQL(SQL);
    //  //string x = string.Empty;
    //  #endregion replaced

    //  db.ElectionsPoliticians_INSERT(
    //      ElectionKey_Electoral
    //    , OfficeKey_Electoral
    //    , PoliticianKey_Electoral
    //    );


    //}

    //protected void ButtonCopy_Click(object sender, EventArgs e)
    //{
    //  try
    //  {

    //    db.Throw_Exception_TextBox_Script(TextboxElectionKey);
    //    db.Throw_Exception_TextBox_Html(TextboxElectionKey);

    //    if (!db.Is_Valid_Election(TextboxElectionKey.Text.Trim()))
    //      throw new ApplicationException("You did not enter one of the Election ID's");

    //    if (Elections.IsCountyElection(ViewState["ElectionKey"].ToString()))//County Election
    //    {
    //      if (db.Rows(sqlElectionsPoliticians4ElectionKey()) > 0)
    //        throw new ApplicationException("This Copy Utility has already been run for ALL COUNTIES Offices. There are ElectionsPoliticians rows. You can only run it once.");

    //      //All ElectionsOffices rows for THIS election, whether on ballot or not
    //      DataTable ElectionsOfficesTable = db.Table(sqlElectionsOffices4ElectionKey());
    //      foreach (DataRow ElectionsOfficesRow in ElectionsOfficesTable.Rows)
    //      {
    //        #region Politicians for each office in the previous election
    //        //DataTable ElectionsPoliticiansTable = db.Table(sql.Elections_Politicians_Office(
    //        //  ElectionsOfficesRow["ElectionKey"].ToString()
    //        //, ElectionsOfficesRow["OfficeKey"].ToString()));
    //        DataTable ElectionsPoliticiansTable = db.Table(sqlElectionsPoliticians4ElectionKeyOfficeKey(
    //          ElectionsOfficesRow["ElectionKey"].ToString()
    //        , ElectionsOfficesRow["OfficeKey"].ToString()));
    //        foreach (DataRow ElectionsPoliticiansRow in ElectionsPoliticiansTable.Rows)
    //        {
    //          string CountyCode = db.CountyCode_In_Elections_Row(ElectionsOfficesRow["ElectionKey"].ToString());
    //          string ElectionKey_County = db.ElectionKey_County(
    //            ViewState["ElectionKey"].ToString()
    //            , Offices.GetStateCodeFromKey(ViewState["ElectionKey"].ToString())
    //            , CountyCode
    //            );
    //          //Office has to be in this election to add politicians to it
    //          if (Offices.IsInElection(ElectionsOfficesRow["OfficeKey"].ToString(), ElectionKey_County))
    //          {
    //            //Insert if office is on the ballot for THIS election (creating a new elections puts all offices on ballot)
    //            //if (db.ElectionsOffices_Bool_Optional(ElectionKey_County, ElectionsOfficesRow["OfficeKey"].ToString(), "IsOnBallot"))
    //            //{
    //            AddElectionsPoliticiansRow(ElectionKey_County
    //            , ElectionsPoliticiansRow["PoliticianKey"].ToString()
    //            , ElectionsPoliticiansRow["RunningMateKey"].ToString()
    //            , ElectionsPoliticiansRow["OfficeKey"].ToString());
    //            //}
    //          }
    //        }
    //        #endregion
    //      }
    //    }
    //    else//State election
    //    {
    //      if (db.Rows(sqlElectionsPoliticians4ElectionKey()) > 0)
    //        throw new ApplicationException("This Copy Utility has already been run FEDERAL AND STATE Offices. You can only run it once.");

    //      //All ElectionsOffices rows for THIS election, whether on ballot or not
    //      DataTable ElectionsOfficesTable = db.Table(sqlElectionsOffices4ElectionKey());
    //      foreach (DataRow ElectionsOfficesRow in ElectionsOfficesTable.Rows)
    //      {
    //        #region Politicians for each office in the previous election
    //        DataTable ElectionsPoliticiansTable = db.Table(sqlElectionsPoliticians4ElectionKeyOfficeKey(
    //          ElectionsOfficesRow["ElectionKey"].ToString()
    //        , ElectionsOfficesRow["OfficeKey"].ToString()));
    //        foreach (DataRow ElectionsPoliticiansRow in ElectionsPoliticiansTable.Rows)
    //        {
    //          //Office has to be in this election to add politicians to it
    //          if (Offices.IsInElection(ElectionsOfficesRow["OfficeKey"].ToString(), ViewState["ElectionKey"].ToString()))
    //          {
    //            //Insert if office is on the ballot for this election (creating a new elections puts all offices on ballot)
    //            //if (db.ElectionsOffices_Bool_Optional(ViewState["ElectionKey"].ToString(), ElectionsOfficesRow["OfficeKey"].ToString(), "IsOnBallot"))
    //            //{
    //            AddElectionsPoliticiansRow(ViewState["ElectionKey"].ToString()
    //            , ElectionsPoliticiansRow["PoliticianKey"].ToString()
    //              , ElectionsPoliticiansRow["RunningMateKey"].ToString()
    //              , ElectionsPoliticiansRow["OfficeKey"].ToString());
    //            //}
    //          }
    //        }
    //        #endregion
    //      }
    //    }
    //    //db.Invalidate_Election(ViewState["ElectionKey"].ToString());

    //    Msg.Text = db.Ok("The politician rows have been copied.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }

    //}

    //protected void CheckBoxElectionViewable_CheckedChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    var electionKey = ViewState["ElectionKey"].ToString();

    //    if (Elections.IsStateElection(electionKey))
    //    {
    //      Elections.UpdateIsViewable(CheckBoxElectionViewable.Checked, electionKey);
    //      Msg.Text =
    //        db.Ok("The public viewing status of this election has been changed.");
    //    }
    //    else
    //      Msg.Text =
    //        db.Fail(
    //          "This function is not currently available for county and local elections.");
    //    //if (CheckBoxElectionViewable.Checked)
    //    //{
    //    //  if (db.Electoral_Class_Election(ViewState["ElectionKey"].ToString()) == db.Electoral.State)
    //    //  {
    //    //    #region Only for State elections, set all county and local elections as viewable, but don't set current
    //    //    string sql_Update = string.Empty;
    //    //    sql_Update += " UPDATE Elections";
    //    //    sql_Update += " SET Elections.IsViewable = 1";
    //    //    sql_Update += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //    //    //sql_Update += " WHERE ElectionYYYYMMDD = " + db.SQLLit(db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionYYYYMMDD"));
    //    //    //sql_Update += " AND StateCode = " + db.SQLLit(db.Elections_Str(ViewState["ElectionKey"].ToString(), "StateCode"));
    //    //    //sql_Update += " AND CountyCode <>''";
    //    //    db.ExecuteSQL(sql_Update);
    //    //    #endregion Only for State elections, set all county and local elections as viewable, but don't set current
    //    //  }
    //    //}
    //    //else
    //    //{
    //    //  #region Status Changed to - NOT Ready to be Viewed
    //    //  db.Elections_Update_Bool(ViewState["ElectionKey"].ToString(), "IsViewable", false);
    //    //  #endregion Status Changed to - NOT Ready to be Viewed
    //    //}

    //    //CheckBox_Election_Viewable_Status_Set();

    //    Visible_Controls();

    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void CheckBox_Election_Viewable_Status_Set()
    //{
    //  try
    //  {
    //    #region Note
    //    //County and Local election are created on the fly
    //    //and my not yet exits
    //    #endregion Note

    //    var electionKey = ViewState["ElectionKey"].ToString();
    //    if (Elections.IsCountyElection(electionKey) ||
    //      Elections.IsLocalElection(electionKey))
    //    {
    //      CheckBoxElectionViewable.Checked = false;
    //      CheckBoxElectionViewable.Text = "All ballots and reports for this county and local election are NOT VIEWABLE for public.";
    //    }
    //    else
    //    {
    //      if (db.Elections_Bool(electionKey, "IsViewable"))
    //      {
    //        CheckBoxElectionViewable.Checked = true;
    //        CheckBoxElectionViewable.Text = "All ballots and reports for this election ARE VIEWABLE for public.";
    //      }
    //      else
    //      {
    //        CheckBoxElectionViewable.Checked = false;
    //        CheckBoxElectionViewable.Text = "All ballots and reports for this election are NOT VIEWABLE for public.";
    //      }
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

    //protected void CheckBoxElection_Winners_CheckedChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Server.ScriptTimeout = 300;//300 sec = 5 min

    //    if (CheckBoxElectionWinners.Checked)
    //    {
    //      #region Status Changed to - All winners identified

    //      bool isAllWinnersIdentified = true;
    //      LabelWinersNotIdentified.Text = string.Empty;

    //      #region sql for ElectionsOffices
    //      string sql = " SELECT OfficeKey";
    //      sql += " FROM ElectionsOffices";
    //      sql += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //      //sql += " AND OfficeKey != 'USPresident'";
    //      sql += " ORDER BY OfficeLevel,DistrictCode";
    //      #endregion sql for ElectionsOffices
    //      DataTable tableElectionsOffices = db.Table(sql);
    //      foreach (DataRow rowElectionOffice in tableElectionsOffices.Rows)
    //      {
    //        #region Office Contest

    //        #region sql for ElectionsPoliticians
    //        sql = " Select PoliticianKey,IsWinner";
    //        sql += " FROM ElectionsPoliticians";
    //        sql += " WHERE ElectionKey = " + db.SQLLit(ViewState["ElectionKey"].ToString());
    //        sql += " AND OfficeKey = " + db.SQLLit(rowElectionOffice["OfficeKey"].ToString());
    //        #endregion sql for ElectionsPoliticians

    //        //skip office contests where there are no candidates
    //        int rows = db.Rows(sql);
    //        if (rows > 0)
    //        {
    //          #region 1 or more candidates
    //          bool isWinnerIdentified = false;
    //          DataTable tableElectionsPoliticians = db.Table(sql);
    //          foreach (DataRow rowElectionsPoliticians in tableElectionsPoliticians.Rows)
    //          {
    //            //One or more winners need to be identified
    //            if (Convert.ToInt16(rowElectionsPoliticians["IsWinner"]) == 1)
    //              isWinnerIdentified = true;
    //          }

    //          #region Add Office Contest Anchor to identify winner later
    //          if (!isWinnerIdentified)
    //          {
    //            LabelWinersNotIdentified.Text += "<br>";
    //            //LabelWinersNotIdentified.Text += db.Anchor_Admin_Politician_Election(
    //            LabelWinersNotIdentified.Text += db.Anchor_Admin_OfficeWinner(
    //              ViewState["ElectionKey"].ToString(),
    //              rowElectionOffice["OfficeKey"].ToString(),
    //              Offices.FormatOfficeName(rowElectionOffice["OfficeKey"].ToString())
    //              );

    //            isAllWinnersIdentified = false;
    //          }
    //          #endregion Add Office Contest Anchor to identify winner later

    //          #endregion 1 or more candidates
    //        }

    //        #endregion Office Contest
    //      }

    //      #region sql for Referendums
    //      string sqlReferendums = string.Empty;
    //      sqlReferendums += "SELECT";
    //      sqlReferendums += " ElectionKey";
    //      sqlReferendums += ",ReferendumKey";
    //      sqlReferendums += ",IsResultRecorded";
    //      sqlReferendums += " FROM Referendums";
    //      sqlReferendums += " WHERE ElectionKey =" + db.SQLLit(ViewState["ElectionKey"].ToString());
    //      #endregion sql for Referendums
    //      DataTable tableReferendums = db.Table(sqlReferendums);
    //      foreach (DataRow rowReferendum in tableReferendums.Rows)
    //      {
    //        #region Referendum
    //        bool isBallotMeasureRecorded = false;
    //        if (Convert.ToInt16(rowReferendum["IsResultRecorded"]) == 1)
    //          isBallotMeasureRecorded = true;

    //        #region Add Ballot Measure Anchor to record later
    //        if (!isBallotMeasureRecorded)
    //        {
    //          LabelWinersNotIdentified.Text += "<br>";
    //          LabelWinersNotIdentified.Text += db.Anchor_Admin_Referendums(
    //          rowReferendum["ElectionKey"].ToString()
    //          , rowReferendum["ReferendumKey"].ToString()
    //          );

    //          isAllWinnersIdentified = false;
    //        }
    //        #endregion Add Ballot Measure Anchor to record later
    //        #endregion Referendum
    //      }

    //      #region Election Status & Msg

    //      if (isAllWinnersIdentified)
    //      {

    //        #region All winners identified
    //        db.Elections_Update_Bool(
    //          ViewState["ElectionKey"].ToString(),
    //          "IsWinnersIdentified",
    //          true);

    //        //UpdateElectionReport();

    //        //db.Report_Officials_Update(PageCache, ViewState["StateCode"].ToString());

    //        Msg.Text = db.Ok("All the election contest winners have been identified."
    //          + " The Election Report and the Elected Officials Report also have been updated.");
    //        #endregion All winners identified
    //      }
    //      else
    //      {
    //        Msg.Text = db.Fail("The election contest winners that have NOT been identified are shown in the report below."
    //          + " Use the office contest links to identify the winners.");
    //      }

    //      #endregion Election Status & Msg

    //      #endregion Status Changed to - All winners identified
    //    }
    //    else
    //    {
    //      #region Status Changed to - All winners NOT identified
    //      db.Elections_Update_Bool(
    //        ViewState["ElectionKey"].ToString(),
    //        "IsWinnersIdentified",
    //        false);
    //      Msg.Text = db.Ok("The status of the identification of the election contest winners has been changed indicating all have not been changed.");
    //      #endregion Status Changed to - All winners NOT identified
    //    }

    //    #region Remove Cached Pages affected
    //    #endregion Remove Cached Pages affected

    //    CheckBoxElection_Winners_Status_Set();

    //    Visible_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void CheckBoxElection_Winners_Status_Set()
    //{
    //  try
    //  {
    //    var electionKey = ViewState["ElectionKey"].ToString();
    //    if (Elections.IsCountyElection(electionKey) || 
    //      Elections.IsLocalElection(electionKey))
    //    {
    //      CheckBoxElectionWinners.Checked = false;
    //      CheckBoxElectionWinners.Text = "Winners for this county or local election have not yet been identified.";
    //    }
    //    else
    //    {
    //      if (db.Elections_Bool(electionKey, "IsWinnersIdentified"))
    //      {
    //        CheckBoxElectionWinners.Checked = true;
    //        CheckBoxElectionWinners.Text = "Winners for this election have all been identified.";
    //      }
    //      else
    //      {
    //        CheckBoxElectionWinners.Checked = false;
    //        CheckBoxElectionWinners.Text = "Winners for this election have NOT been identified.";
    //      }
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

    ////protected void UpdateElectionReport()
    ////{
    ////  string electionKey = ViewState["ElectionKey"].ToString();

    ////  Server.ScriptTimeout = 30000;//3000 sec = 50 min

    ////  if (db.Is_Election_Local(electionKey))
    ////  {
    ////    #region Single Local Election
    ////    db.Report_Election_Update(PageCache, electionKey);
    ////    #endregion Single Local Election
    ////  }
    ////  else if (db.Is_Election_County(electionKey))
    ////  {
    ////    #region County and Local Elections in County
    ////    DataTable Table_County_And_Local_ElectionKeys = db.Table_ElectionKeys_County_And_Locals(
    ////     db.StateCode_In_ElectionKey(electionKey)
    ////    , db.CountyCode_In_ElectionKey(electionKey)
    ////    , db.ElectionYYYYMMDD_In_ElectionKey(electionKey)
    ////    , db.ElectionType_In_ElectionKey(electionKey)
    ////    , db.NationalPartyCode_In_ElectionKey(electionKey)
    ////    );
    ////    foreach (DataRow Row_ElectionKey in Table_County_And_Local_ElectionKeys.Rows)
    ////    {
    ////      db.Report_Election_Update(PageCache, Row_ElectionKey["ElectionKey"].ToString());
    ////    }
    ////    #endregion County and Local Elections in County
    ////  }
    ////  else if (db.Is_Election_State(electionKey))
    ////  {
    ////    #region State and All Counties and Locals Elections in State
    ////    DataTable Table_State_Countie_And_Locals_ElectionKeys =
    ////      db.Table_ElectionKeys_State_Counties_And_Locals(
    ////         db.StateCode_In_ElectionKey(electionKey)
    ////        , db.ElectionYYYYMMDD_In_ElectionKey(electionKey)
    ////        , db.ElectionType_In_ElectionKey(electionKey)
    ////        , db.NationalPartyCode_In_ElectionKey(electionKey)
    ////        );
    ////    foreach (DataRow Row_ElectionKey in Table_State_Countie_And_Locals_ElectionKeys.Rows)
    ////    {
    ////      db.Report_Election_Update(PageCache, Row_ElectionKey["ElectionKey"].ToString());
    ////    }
    ////    #endregion State and Counties and Locals Elections in State
    ////  }
    ////  else if (db.Is_Election_State_By_State(electionKey))
    ////  {
    ////    #region State-by-State Report
    ////    db.Report_Election_Update(PageCache, electionKey);
    ////    #endregion State-by-State Report
    ////  }
    ////  else if (db.Is_Election_Type_Presidential_Comparison(PageCache, electionKey))
    ////  {
    ////    #region Special Case: Presidential Candidates
    ////    db.Report_Election_Update(PageCache, electionKey);
    ////    #endregion Special Case: Presidential Candidates
    ////  }
    ////  else
    ////  {
    ////    throw new ApplicationException("The ElectionKey appears to be invalid.");
    ////  }

    ////}

    ////protected void ButtonUpdateReport_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    //UpdateElectionReport();

    ////    //ShowElectionReport();
    ////    Visible_Controls();

    ////    Msg.Text = db.Ok("This election report has been updated and reflects the current election data.");
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    #region
    ////    //LabelElectionReport.Text = "An error was encountered: " + ex.Message;
    ////    db.Log_Error_Admin(ex);
    ////    #endregion
    ////  }
    ////}

    //protected void ButtonSingleCandidateWinners_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    #region note
    //    //This button should only be visible for past elections.
    //    //
    //    //ElectionsPoliticians.IsWinner can be set for any past election.
    //    //
    //    //However, ElectionsPoliticians.IsIncumbent can not be set here
    //    //because if this operation is done more than once
    //    //all the winners will be treated as incumbents
    //    //Incumbency can only be determined when a election contest 
    //    //is created by OfficeContest.aspx.
    //    //Or set in OfficeIncumbent.aspx, which has yet to be created.
    //    //
    //    //Currently elected officials can only be set here if this is 
    //    //is the most recent previous election.
    //    #endregion note

    //    string electionKey = ViewState["ElectionKey"].ToString();

    //    #region office contests in election
    //    string sql = string.Empty;
    //    sql += " SELECT OfficeKey";
    //    sql += " FROM ElectionsOffices";
    //    sql += " WHERE ElectionKey = " + db.SQLLit(electionKey);
    //    sql += " ORDER BY OfficeLevel,OfficeKey";
    //    DataTable tableElectionsOffices = db.Table(sql);
    //    #endregion office contests in election

    //    foreach (DataRow rowElectionsOffices in tableElectionsOffices.Rows)
    //    {
    //      #region office contest in election

    //      #region candidates and office positions for office
    //      string sqlElectionsPoliticians = string.Empty;
    //      sqlElectionsPoliticians += " SELECT OfficeKey,PoliticianKey";
    //      sqlElectionsPoliticians += " FROM vote.ElectionsPoliticians";
    //      sqlElectionsPoliticians += " WHERE ElectionKey = " + db.SQLLit(electionKey);
    //      sqlElectionsPoliticians += " AND OfficeKey = " + db.SQLLit(rowElectionsOffices["OfficeKey"].ToString());

    //      int candidates = db.Rows(sqlElectionsPoliticians);
    //      //int officePositions = db.Offices_Int(rowElectionsOffices["OfficeKey"].ToString(), "Incumbents");
    //      int electionPositions = db.Offices_Int(rowElectionsOffices["OfficeKey"].ToString(), "ElectionPositions");
    //      #endregion candidates and office positions for office

    //      if (candidates == electionPositions)
    //      {
    //        #region Remove ALL the currently elected officials for this office only if most recent previous election
    //        if (db.Is_Election_Previous_Most_Recent(electionKey))
    //        {
    //          //string sqlDelete = string.Empty;
    //          //sqlDelete += " DELETE FROM vote.OfficesOfficials";
    //          //sqlDelete += " WHERE OfficeKey = " + db.SQLLit(rowElectionsOffices["OfficeKey"].ToString());
    //          //sqlDelete += " AND StateCode = " + db.SQLLit(db.StateCode_In_ElectionKey(electionKey));
    //          //db.ExecuteSQL(sqlDelete);
    //          db.OfficesOfficials_Delete(
    //            rowElectionsOffices["OfficeKey"].ToString()
    //            );

    //        }
    //        #endregion Remove ALL the current elected officials for this office only if most recent previous election

    //        #region Office contest candidates - identify as winner(s)

    //        DataTable tableElectionsPoliticians = db.Table(sqlElectionsPoliticians);
    //        foreach (DataRow rowElectionsPoliticians in tableElectionsPoliticians.Rows)
    //        {
    //          #region a candidate

    //          #region commented
    //          //bool is_Incumbent_Set = db.Set_Winner_And_Incumbent(
    //          //  electionKey,
    //          //  rowElectionsPoliticians["OfficeKey"].ToString(),
    //          //  rowElectionsPoliticians["PoliticianKey"].ToString(),
    //          //  true
    //          //  );
    //          #endregion commented

    //          #region Set IsWinner flag in ElectionsPoliticians Table
    //          db.Set_IsWinner_For_Election(
    //            electionKey,
    //            rowElectionsPoliticians["OfficeKey"].ToString(),
    //            rowElectionsPoliticians["PoliticianKey"].ToString(),
    //            true
    //            );
    //          #endregion set IsWinner flag in ElectionsPoliticians Table

    //          #region Add as currently elected official for this office only if most recent previous election
    //          if (db.Is_Election_Previous_Most_Recent(electionKey))
    //            db.OfficesOfficials_INSERT(
    //              rowElectionsPoliticians["OfficeKey"].ToString(),
    //              rowElectionsPoliticians["PoliticianKey"].ToString()
    //              );
    //          #endregion Add as currently elected official for this office only if most recent previous election

    //          #endregion a candidate
    //        }

    //        #region Record whether all the winners for all the office positions have been identified
    //        db.Update_ElectionsOffices_Is_All_Winners_Identified(
    //          electionKey,
    //          rowElectionsOffices["OfficeKey"].ToString()
    //          );
    //        #endregion Record whether all the winners for all the office positions have been identified

    //        #endregion Office contest candidates - identify winner(s)
    //      }
    //      #endregion office contests in election
    //    }

    //    //UpdateElectionReport();

    //    ShowElectionReport();

    //    Msg.Text = db.Ok("All the uncontested winners have been identified."
    //      + " They can be seen in the report below.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    //LabelElectionReport.Text = "An error was encountered: " + ex.Message;
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    ////protected void ButtonRefreshReport_Click(object sender, EventArgs e)
    ////{
    ////  try
    ////  {
    ////    //ShowElectionReport();
    ////    Visible_Controls();

    ////    if (db.Is_Report_Current_Election(
    ////      ViewState["ElectionKey"].ToString())
    ////      )
    ////      Msg.Text = db.Msg("The election report is current and reflects the current election data,"
    ////    + " unless you made data changes since you last clicked the Refresh Report Button. Click again when in doubt.");
    ////    else
    ////      Msg.Text = db.Warn("The election report is not current. Click the Update Report Button to reflect the current data.");
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    #region
    ////    //LabelElectionReport.Text = "An error was encountered: " + ex.Message;
    ////    db.Log_Error_Admin(ex);
    ////    #endregion
    ////  }
    ////}

    //protected string New_ElectionKey(string electionKey)
    //{
    //  return db.ElectionKey_Build(
    //    Elections.GetStateCodeFromKey(electionKey)
    //    , Elections.GetCountyCodeFromKey(electionKey)
    //    , Elections.GetLocalCodeFromKey(electionKey)
    //    , TextboxElectionDate.Text.Trim()
    //    , db.Elections_Str(electionKey, "ElectionType")
    //    , db.Elections_Str(electionKey, "NationalPartyCode")
    //    );
    //}

    //protected void ButtonChangeDate_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    #region checks
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextboxElectionDate);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextboxElectionTitle);

    //    if (TextboxElectionDate.Text.Trim() == string.Empty)//empty 
    //      throw new ApplicationException("The New Date textbox is empty!");

    //    if (TextboxElectionTitle.Text.Trim() == string.Empty)//empty 
    //      throw new ApplicationException("The New Election Title textbox is empty!");
    //    #endregion checks

    //    //string newElectionKey = db.ElectionKey_Build(
    //    //  db.StateCode_In_ElectionKey(ViewState["ElectionKey"].ToString())
    //    //  , db.CountyCode_In_ElectionKey(ViewState["ElectionKey"].ToString())
    //    //  , db.LocalCode_In_ElectionKey(ViewState["ElectionKey"].ToString())
    //    //  , TextboxElectionDate.Text.Trim()
    //    //  , db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionType")
    //    //  , db.Elections_Str(ViewState["ElectionKey"].ToString(), "NationalPartyCode")
    //    //  );
    //    string SQL = string.Empty;

    //    string oldElectionKey = ViewState["ElectionKey"].ToString();
    //    string newElectionKeyState = string.Empty;
    //    string newElectionKeyFederal = string.Empty;
    //    string newElectionKeyCounty = string.Empty;
    //    string newElectionKeyLocal = string.Empty;

    //    string newElectionKey = New_ElectionKey(ViewState["ElectionKey"].ToString());

    //    #region ElectionsOffices Table
    //    SQL = " SELECT OfficeKey";
    //    SQL += " FROM ElectionsOffices";
    //    SQL += " WHERE ElectionKey = " + db.SQLLit(oldElectionKey);
    //    DataTable Table_ElectionsOffices = db.Table(SQL);
    //    foreach (DataRow Row_ElectionOffice in Table_ElectionsOffices.Rows)
    //    {
    //      newElectionKeyState = db.ElectionKey_State(newElectionKey);
    //      newElectionKeyFederal = db.ElectionKey_Federal(newElectionKey
    //        , Row_ElectionOffice["OfficeKey"].ToString());
    //      newElectionKeyCounty = db.ElectionKey_County(newElectionKey);
    //      newElectionKeyLocal = db.ElectionKey_Local(newElectionKey
    //        , Row_ElectionOffice["OfficeKey"].ToString());

    //      SQL = " UPDATE ElectionsOffices";
    //      SQL += " SET ElectionKey = " + db.SQLLit(newElectionKey);
    //      SQL += " ,ElectionKeyState = " + db.SQLLit(newElectionKeyState);
    //      SQL += " ,ElectionKeyFederal = " + db.SQLLit(newElectionKeyFederal);
    //      SQL += " ,ElectionKeyCounty = " + db.SQLLit(newElectionKeyCounty);
    //      SQL += " ,ElectionKeyLocal = " + db.SQLLit(newElectionKeyLocal);
    //      SQL += " WHERE ElectionKey = " + db.SQLLit(oldElectionKey);
    //      SQL += " AND OfficeKey = " + db.SQLLit(Row_ElectionOffice["OfficeKey"].ToString());
    //      //------------UNCOMMENT----------
    //      db.ExecuteSQL(SQL);
    //    }
    //    #endregion ElectionsOffices Table

    //    #region ElectionsPoliticians Table
    //    SQL = " SELECT OfficeKey,PoliticianKey";
    //    SQL += " FROM ElectionsPoliticians";
    //    SQL += " WHERE ElectionKey = " + db.SQLLit(oldElectionKey);
    //    DataTable Table_ElectionsPoliticians = db.Table(SQL);
    //    foreach (DataRow ElectionsPoliticians in Table_ElectionsPoliticians.Rows)
    //    {
    //      newElectionKeyState = db.ElectionKey_State(newElectionKey);
    //      newElectionKeyFederal = db.ElectionKey_Federal(newElectionKey
    //        , ElectionsPoliticians["OfficeKey"].ToString()
    //        );
    //      newElectionKeyCounty = db.ElectionKey_County(newElectionKey);
    //      newElectionKeyLocal = db.ElectionKey_Local(newElectionKey
    //        , ElectionsPoliticians["OfficeKey"].ToString());

    //      SQL = " UPDATE ElectionsPoliticians";
    //      SQL += " SET ElectionKey = " + db.SQLLit(newElectionKey);
    //      SQL += " ,ElectionKeyState = " + db.SQLLit(newElectionKeyState);
    //      SQL += " ,ElectionKeyFederal = " + db.SQLLit(newElectionKeyFederal);
    //      SQL += " ,ElectionKeyCounty = " + db.SQLLit(newElectionKeyCounty);
    //      SQL += " ,ElectionKeyLocal = " + db.SQLLit(newElectionKeyLocal);
    //      SQL += " WHERE ElectionKey = " + db.SQLLit(oldElectionKey);
    //      SQL += " AND OfficeKey = " + db.SQLLit(ElectionsPoliticians["OfficeKey"].ToString());
    //      SQL += " AND PoliticianKey = " + db.SQLLit(ElectionsPoliticians["PoliticianKey"].ToString());
    //      //------------UNCOMMENT----------
    //      db.ExecuteSQL(SQL);
    //    }
    //    #endregion ElectionsPoliticians Table

    //    //#region ReportsElections
    //    ////db.ReportsElections_Update_Str(
    //    ////  oldElectionKey
    //    ////  , "ElectionKey"
    //    ////  , newElectionKey);

    //    //SQL = " DELETE FROM ReportsElections";
    //    //SQL += " WHERE ElectionKey = " + db.SQLLit(oldElectionKey);
    //    //db.ExecuteSQL(SQL);

    //    //#endregion ReportsElections

    //    #region Single Elections Table Row
    //    SQL = string.Empty;

    //    db.Elections_Update_Date(
    //      oldElectionKey
    //    , "ElectionDate"
    //    , Convert.ToDateTime(TextboxElectionDate.Text.Trim())
    //    );

    //    db.Elections_Update_Str(
    //      oldElectionKey
    //    , "ElectionDesc"
    //    , TextboxElectionTitle.Text.Trim()
    //    );

    //    db.Elections_Update_Str(
    //      oldElectionKey
    //    , "ElectionYYYYMMDD"
    //    , Convert.ToDateTime(TextboxElectionDate.Text.Trim()).ToString("yyyyMMdd")
    //    );

    //    string electionKeyCanonical = db.Elections_Str(
    //        oldElectionKey
    //      , "ElectionKeyCanonical");
    //    if (!string.IsNullOrEmpty(electionKeyCanonical))
    //    {
    //      DateTime electionYYYYMMDD = Convert.ToDateTime(TextboxElectionDate.Text.Trim());
    //      string yYYYMMDD = electionYYYYMMDD.ToString("yyyyMMdd");
    //      string newElectionKeyCanonical = electionKeyCanonical.Remove(2, 8);
    //      newElectionKeyCanonical = newElectionKeyCanonical.Insert(2, yYYYMMDD);
    //      newElectionKeyCanonical = newElectionKeyCanonical.Remove(10, 1);
    //      newElectionKeyCanonical = newElectionKeyCanonical.Insert(10, "A");

    //      db.Elections_Update_Str(
    //        oldElectionKey
    //      , "ElectionKeyCanonical"
    //      , newElectionKeyCanonical
    //      );
    //    }

    //    #region Updating the ElectionKey must come last
    //    newElectionKey = New_ElectionKey(oldElectionKey);

    //    db.Elections_Update_Str(
    //      oldElectionKey
    //    , "ElectionKey"
    //    , newElectionKey
    //    );
    //    #endregion Updating the ElectionKey must come last

    //    #endregion Single Elections Table Row

    //    ViewState["ElectionKey"] = newElectionKey;

    //    ShowElectionReport();

    //    Page_Title();

    //    Visible_Controls();

    //    Msg.Text = db.Ok("The election date has been changed to "
    //     + TextboxElectionDate.Text.Trim());

    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}
    //#endregion Buttons & CheckBoxes

    //#region TextBox Changes
    //protected void TextboxElectionDesc_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextboxElectionDesc);

    //    if (TextboxElectionDesc.Text.Length > 90)
    //    {
    //      int ShortenBy = TextboxElectionDesc.Text.Length - 90;
    //      throw new ApplicationException("The Election Title can not exceed 90 characters. You need to shorten by: "
    //        + ShortenBy.ToString() + " characters.");
    //    }

    //    db.Elections_Update_Str(ViewState["ElectionKey"].ToString(), "ElectionDesc", TextboxElectionDesc.Text.Trim());
    //    TextboxElectionDesc.Text = db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionDesc");

    //    //db.Invalidate_Election(ViewState["ElectionKey"].ToString());

    //    Visible_Controls();

    //    Page_Title();

    //    Msg.Text = db.Ok("The Election Title has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextboxElectionInfo_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextboxElectionInfo);

    //    db.Elections_Update_Str(ViewState["ElectionKey"].ToString(), "ElectionAdditionalInfo", TextboxElectionInfo.Text.Trim());
    //    TextboxElectionInfo.Text = db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionAdditionalInfo");

    //    //db.Invalidate_Election(ViewState["ElectionKey"].ToString());

    //    Visible_Controls();

    //    Msg.Text = db.Ok("Additional Information on Ballots and Reports has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextboxBallotInstructions_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextboxBallotInstructions);

    //    db.Elections_Update_Str(ViewState["ElectionKey"].ToString(), "BallotInstructions", TextboxBallotInstructions.Text.Trim());
    //    TextboxBallotInstructions.Text = db.Elections_Str(ViewState["ElectionKey"].ToString(), "BallotInstructions");

    //    //db.Invalidate_Election(ViewState["ElectionKey"].ToString());

    //    Visible_Controls();

    //    Msg.Text = db.Ok("Ballot Instructions has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxElectionOrder_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxElectionOrder);

    //    if (!db.Is_Valid_Integer(TextBoxElectionOrder.Text.Trim()))
    //      throw new ApplicationException("The ElectionOrder is not an integer.");

    //    db.Elections_Update_Int(ViewState["ElectionKey"].ToString()
    //      , "ElectionOrder"
    //      , Convert.ToInt16(TextBoxElectionOrder.Text.Trim())
    //      );
    //    TextBoxElectionOrder.Text = db.Elections_Int(ViewState["ElectionKey"].ToString()
    //      , "ElectionOrder").ToString();

    //    //db.Invalidate_Election(ViewState["ElectionKey"].ToString());

    //    Visible_Controls();

    //    Msg.Text = db.Ok("The Election Order has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}


    //protected void TextboxElectionDate_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextboxElectionDate);
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextboxElectionTitle);

    //    if (TextboxElectionDate.Text.Trim() == string.Empty)//empty 
    //      throw new ApplicationException("The Date textbox is empty!");

    //    if (!db.Is_Valid_Date(TextboxElectionDate.Text.Trim()))//bad date
    //      throw new ApplicationException("You entered a bad date!");

    //    if (Convert.ToDateTime(TextboxElectionDate.Text.Trim()) < DateTime.Today)//in the future
    //      throw new ApplicationException("The new date needs to be in the future!");

    //    TextboxElectionTitle.Text = db.Str_Election_Description(
    //      db.Elections_Str(ViewState["ElectionKey"].ToString(), "ElectionType")
    //      , db.Elections_Str(ViewState["ElectionKey"].ToString(), "NationalPartyCode")
    //      , TextboxElectionDate.Text.Trim()
    //      , ViewState["StateCode"].ToString()
    //      );
    //    //          LabelCurrentDate.Text = db.Elections_Date(ViewState["ElectionKey"].ToString(), "ElectionDate").ToString("M/d/yy");

    //    Visible_Controls();

    //    Msg.Text = db.Ok("The new election date is ok."
    //    + " Make any necessary changes to the election title."
    //    + " Then click the Change Election Date Button.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}
    //#endregion TextBox Changes

    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    #region ViewState Values and Security Checks
    //    #region ViewState Values
    //    ViewState["ElectionKey"] = string.Empty;

    //    #region ViewState["ElectionKey"]
    //    if (!string.IsNullOrEmpty(QueryElection))
    //      ViewState["ElectionKey"] = QueryElection;
    //    else
    //      throw new ApplicationException("No Election ID was provided.");
    //    #endregion ViewState["ElectionKey"]
    //    #endregion Values

    //    #region Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

    //    #region Notes
    //    //The Session UserStateCode, UserCountyCode, UserLocalCode can be changed
    //    //by a higher administration level using query strings
    //    //This is done in db.State_Code(), db.County_Code(), db.Local_Code()
    //    //
    //    //Using ViewState variables insures these values won't
    //    //change on any postbacks or in different tab or browser Sessions.
    //    //
    //    //ViewState["StateCode"] can be a StateCode or U1, u2, u3 for FederalCode
    //    #endregion Notes

    //    ViewState["StateCode"] = db.State_Code();
    //    ViewState["CountyCode"] = db.County_Code();
    //    ViewState["LocalCode"] = db.Local_Code();
    //    if (!db.Is_User_Security_Ok())
    //      SecurePage.HandleSecurityException();

    //    #endregion Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]
    //    #endregion ViewState Values and Security Checks

    //    try
    //    {
    //      db.Path_Css_Custom_Href_Set(ref This, db.Domain_DesignCode_This(), "Election.css");

    //      string electionKey = ViewState["ElectionKey"].ToString();
    //      HyperLink_View_Public_Report.NavigateUrl =
    //        UrlManager.GetElectionPageUri(electionKey).ToString();

    //      Visible_Controls();

    //      Page_Title();

    //      if (Elections.IsStateElection(ViewState["ElectionKey"].ToString()))
    //      {
    //        #region Load Email Dates for election

    //        //Request Non-General Election Roster for election
    //        Label_Emails_Date_Election_Roster.Text =
    //          db.Elections_Date_Str(
    //            ViewState["ElectionKey"].ToString()
    //            , "EmailsDateElectionRoster");
    //        Label_Emails_Sent_Election_Roster.Text =
    //          db.Elections_Int(
    //            ViewState["ElectionKey"].ToString()
    //            , "EmailsSentElectionRoster").ToString();

    //        //Notify State of election completion
    //        Label_Emails_Date_Election_Completion.Text =
    //          db.Elections_Date_Str(
    //            ViewState["ElectionKey"].ToString()
    //            , "EmailsDateElectionCompletion");
    //        Label_Emails_Sent_Election_Completion.Text =
    //          db.Elections_Int(
    //            ViewState["ElectionKey"].ToString()
    //            , "EmailsSentElectionCompletion").ToString();

    //        //Provide candidates with login to provide information
    //        Label_Emails_Date_Candidates_Login.Text =
    //          db.Elections_Date_Str(
    //            ViewState["ElectionKey"].ToString()
    //            , "EmailsDateCandidatesLogin");
    //        Label_Emails_Sent_Candidates_Login.Text =
    //          db.Elections_Int(
    //            ViewState["ElectionKey"].ToString()
    //            , "EmailsSentCandidatesLogin").ToString();

    //        //Provide State Parties with Login to Promote Candidates
    //        Label_Emails_Date_Parties_Login.Text =
    //          db.Elections_Date_Str(
    //            ViewState["ElectionKey"].ToString()
    //            , "EmailsDatePartiesLogin");
    //        Label_Emails_Sent_Parties_Login.Text =
    //          db.Elections_Int(
    //            ViewState["ElectionKey"].ToString()
    //            , "EmailsSentPartiesLogin").ToString();

    //        #endregion Load Email Dates for election
    //      }

    //      //LoadElectionDataInTextBoxes();

    //      //ShowElectionReport();

    //      //CheckBox_Election_Viewable_Status_Set();

    //      #region Msg
    //      if (db.Is_Valid_Election(ViewState["ElectionKey"].ToString()))
    //      {
    //        //#region Election Exists
    //        //if (db.Is_Report_Current_Election(
    //        //ViewState["ElectionKey"].ToString())
    //        //)
    //        //  Msg.Text = db.Msg("Click the Refresh Report Button to insure the report reflects the current election data.");
    //        //else
    //        //  Msg.Text = db.Warn("The election report is not current."
    //        //+ " Click the Update Report Button to reflect the current election data.");
    //        //#endregion Election Exists
    //      }
    //      else
    //      {
    //        #region No Election Exists
    //        if (db.Is_Electoral_Class_State(
    //         ViewState["StateCode"].ToString()
    //        , ViewState["CountyCode"].ToString()
    //        , ViewState["LocalCode"].ToString()
    //          )
    //          )
    //        {
    //          #region State Election
    //          Msg.Text = db.Fail("No Election has been created.");
    //          #endregion State Election
    //        }
    //        else
    //        {
    //          #region Local or County Election
    //          Msg.Text = db.Warn("The State Election has been created."
    //              + " But no office contests or referendums"
    //              + " have been defined for this county or local district in this election."
    //              + " You need to define at least one office contest or referendum"
    //              + " to create this county or local election before you"
    //              + " can perform any maintenance on it.");
    //          #endregion Local or County Election
    //        }

    //        #endregion No Election Exists
    //      }
    //      #endregion Msg
    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}

    protected override void OnInit(EventArgs e)
    {
      LegacyRedirect(SecureAdminPage.GetUpdateElectionsPageUrl(QueryState,
        QueryCounty, QueryLocal));
      base.OnInit(e);
    }

    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    if (Session["ErrNavBarAdmin"].ToString() != string.Empty)
    //      Msg.Text = db.Fail(Session["ErrNavBarAdmin"].ToString());
    //  }
    //  catch /*(Exception ex)*/
    //  {
    //  }
    //}

    #region Dead code

    //#region Counties
    //protected string sqlCounties()
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " County ";
    //  SQL += " ,StateCode ";
    //  SQL += " ,CountyCode ";
    //  SQL += " FROM Counties ";
    //  SQL += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
    //  SQL += " ORDER BY County";
    //  return SQL;
    //}
    //#endregion Counties

    //#region ROWS
    //protected string sqlRowsReferendums4State()
    //{
    //  string SQL = string.Empty;
    //  SQL += " SELECT ";
    //  SQL += " Referendums.ElectionKey ";
    //  SQL += ",Referendums.CountyCode ";
    //  SQL += " FROM Referendums ";
    //  SQL += " WHERE Referendums.ElectionKey = " + db.SQLLit(db.ElectionKey_State(ViewState["ElectionKey"].ToString()));
    //  SQL += " AND Referendums.IsReferendumTagForDeletion = 0";
    //  return SQL;
    //}
    //#endregion ROWS

    ////Election_ContestsCovered
    //protected void xRadioButtonList_Contests_Covered_Set()
    //{
    //  //if (db.Election_ContestsCovered(ViewState["ElectionKey"].ToString()) == 0)
    //  //  RadioButtonList_Contests_Covered.SelectedValue = "0";
    //  //else if (db.Election_ContestsCovered(ViewState["ElectionKey"].ToString()) == 1)
    //  //  RadioButtonList_Contests_Covered.SelectedValue = "1";

    //}

    //protected void xRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    //if (RadioButtonList_Contests_Covered.SelectedValue == "0")
    //    //  db.Election_Update_ContestsCovered(ViewState["ElectionKey"].ToString()
    //    //    , db.xElection_Contests_Covered_All_Or_Unknown);
    //    //else if (RadioButtonList_Contests_Covered.SelectedValue == "1")
    //    //  db.Election_Update_ContestsCovered(ViewState["ElectionKey"].ToString()
    //    //    , db.xElection_Contests_Covered_Only_Federal_And_Statewide);

    //    //RadioButtonList_Contests_Covered_Set();

    //    //Msg.Text = db.Ok("The offices covered status has been recorded.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    #endregion Dead code



  }
}
