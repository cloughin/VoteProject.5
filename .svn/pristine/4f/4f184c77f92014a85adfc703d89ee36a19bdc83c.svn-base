using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB.Vote;

namespace Vote.Admin
{
  public partial class ElectionsPage : VotePage
  {
    //#region Private

    //private string AdjustElectionKeyForElectoralClass(string electionKey)
    //{
    //  //All the State election links are presented 
    //  //even for county and local elections.
    //  //
    //  //But the url links need to be the county or local election links.

    //  switch (
    //    db.Electoral_Class(ViewState["StateCode"].ToString(),
    //      ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString()))
    //  {
    //    case db.ElectoralClass.USPresident:
    //      return electionKey;
    //    case db.ElectoralClass.USSenate:
    //      return electionKey;
    //    case db.ElectoralClass.USHouse:
    //      return electionKey;
    //    case db.ElectoralClass.USGovernors:
    //      return electionKey;
    //    case db.ElectoralClass.State:
    //      return db.ElectionKey_State(electionKey);
    //    case db.ElectoralClass.County:
    //      return db.ElectionKey_County(electionKey,
    //        ViewState["StateCode"].ToString(), ViewState["CountyCode"].ToString());
    //    case db.ElectoralClass.Local:
    //      return db.ElectionKey_Local(electionKey, ViewState["StateCode"].ToString(),
    //        ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString());
    //    default:
    //      return string.Empty;
    //  }
    //}

    //private void CheckForIllegalInputInAllTextBoxes()
    //{
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPollHours);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPollHoursUrl);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPollPlacesUrl);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxType1);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxParty1);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDate1);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDesc1);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEarlyDate1);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxRegistrationDate1);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxType2);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxParty2);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDate2);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDesc2);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEarlyDate2);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxRegistrationDate2);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxType3);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxParty3);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDate3);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDesc3);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEarlyDate3);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxRegistrationDate3);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxType4);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxParty4);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDate4);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDesc4);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEarlyDate4);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxRegistrationDate4);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxType5);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxParty5);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDate5);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDesc5);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEarlyDate5);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxRegistrationDate5);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxType6);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxParty6);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDate6);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDesc6);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEarlyDate6);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxRegistrationDate6);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxType7);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxParty7);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDate7);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDesc7);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEarlyDate7);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxRegistrationDate7);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxType8);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxParty8);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDate8);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxUpcomingDesc8);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEarlyDate8);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxRegistrationDate8);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextboxElectionDate);
    //}

    //private static bool IsValidElectionTypeForState(ITextControl textBoxElectionType)
    //{
    //  var electionType = textBoxElectionType.Text.Trim()
    //    .ToUpper();
    //  return electionType == "G" || electionType == "O" || electionType == "S" ||
    //    electionType == "P" || electionType == "B";
    //}

    //private static bool IsValidParty(ITextControl textBoxParty)
    //{
    //  var party = textBoxParty.Text.Trim()
    //    .ToUpper();
    //  return party == "D" || party == "R" || party == "G" || party == "L" ||
    //    party == "A" || party == "X";
    //}

    //private static bool IsValidElectionTypeAndParty(ITextControl textBoxElectionType,
    //  ITextControl textBoxParty)
    //{
    //  return IsValidElectionTypeForState(textBoxElectionType) &&
    //    IsValidParty(textBoxParty);
    //}

    //private static string GetInvalidDateErrorMessage(ITextControl textBoxDate)
    //{
    //  var date = textBoxDate.Text.Trim();

    //  var errMsg = string.Empty;

    //  if (date == string.Empty)
    //    errMsg = "The Date is empty!";
    //  else if (!db.Is_Valid_Date(date))
    //    errMsg = "The Date or its format is not recognizable.";

    //  return errMsg;
    //}

    //private static string GetInvalidFutureDateErrorMessage(ITextControl textBoxDate)
    //{
    //  var date = textBoxDate.Text.Trim();

    //  var errMsg = GetInvalidDateErrorMessage(textBoxDate);

    //  if (string.IsNullOrEmpty(errMsg) &&
    //    Convert.ToDateTime(date) < DateTime.Today)
    //    errMsg = "The Date needs to be in the future!";

    //  return errMsg;
    //}

    //private void ValidateEarlyVoting(ITextControl textBoxEarlyDate,
    //  ITextControl textBoxUpcomingDate)
    //{
    //  var earlyDate = textBoxEarlyDate.Text.Trim();
    //  var upcomingDate = textBoxUpcomingDate.Text.Trim();

    //  if (!string.IsNullOrEmpty(earlyDate))
    //  {
    //    if (string.IsNullOrEmpty(upcomingDate))
    //      throw new ApplicationException(
    //        "An election date needs to be identified before a early voting date can be entered");

    //    if (!string.IsNullOrEmpty(GetInvalidDateErrorMessage(textBoxEarlyDate)))
    //      throw new ApplicationException(GetInvalidDateErrorMessage(textBoxEarlyDate));

    //    if (Convert.ToDateTime(upcomingDate) < Convert.ToDateTime(earlyDate))
    //      throw new ApplicationException(
    //        "The early voting date needs to be before the election date. You entered: " +
    //          earlyDate);

    //    Msg.Text = db.Ok("The early voting date is valid.");
    //  }
    //  else
    //    Msg.Text = db.Ok("The early voting date will be deleted.");
    //}

    //private void CheckRegistrationDate(ITextControl textBoxRegistrationDate,
    //  ITextControl textBoxEarlyDate, ITextControl textBoxUpcomingDate)
    //{
    //  var registrationDate = textBoxRegistrationDate.Text.Trim();
    //  var earlyDate = textBoxEarlyDate.Text.Trim();
    //  var upcomingDate = textBoxUpcomingDate.Text.Trim();

    //  if (!string.IsNullOrEmpty(registrationDate))
    //  {
    //    if (string.IsNullOrEmpty(upcomingDate))
    //      throw new ApplicationException(
    //        "An election date needs to be identified before a registration can be entered");

    //    //if (!string.IsNullOrEmpty(Check_Date_ErrMsg(textBoxRegistrationDate)))
    //    //  throw new ApplicationException(Check_Date_ErrMsg(textBoxRegistrationDate));

    //    if (!string.IsNullOrEmpty(GetInvalidDateErrorMessage(textBoxRegistrationDate)))
    //      throw new ApplicationException(GetInvalidDateErrorMessage(textBoxRegistrationDate));

    //    if (Convert.ToDateTime(upcomingDate) < Convert.ToDateTime(registrationDate))
    //      throw new ApplicationException(
    //        "The registration date needs to be before the election date. You entered: " +
    //          registrationDate);

    //    if (!string.IsNullOrEmpty(earlyDate))
    //      if (Convert.ToDateTime(earlyDate) < Convert.ToDateTime(registrationDate))
    //        throw new ApplicationException(
    //          "The registration date needs to be before the early voting date. You entered: " +
    //            registrationDate);
    //    Msg.Text = db.Ok("The registration date is valid.");
    //  }
    //  else
    //    Msg.Text = db.Ok("The registration date will be deleted.");
    //}

    //private bool Is_Type_Party_Date_Ok(TextBox textBoxElectionType,
    //  TextBox textBoxParty, TextBox textBoxUpcomingDate, TextBox textBoxUpcomingDesc,
    //  bool isGenerateDescription)
    //{
    //  db.Throw_Exception_TextBox_Script(textBoxElectionType);
    //  db.Throw_Exception_TextBox_Script(textBoxParty);
    //  db.Throw_Exception_TextBox_Script(textBoxUpcomingDate);
    //  db.Throw_Exception_TextBox_Script(textBoxUpcomingDesc);

    //  var electionType = textBoxElectionType.Text.Trim()
    //    .ToUpper();
    //  var party = textBoxParty.Text.Trim()
    //    .ToUpper();

    //  var isValidElectionTypePartyDate = false;
    //  if ((!string.IsNullOrEmpty(textBoxElectionType.Text.Trim())) ||
    //    (!string.IsNullOrEmpty(textBoxParty.Text.Trim())))
    //  {
    //    if (IsValidElectionTypeAndParty(textBoxElectionType, textBoxParty))

    //      if (!string.IsNullOrEmpty(textBoxUpcomingDate.Text.Trim()))

    //        if (string.IsNullOrEmpty(GetInvalidFutureDateErrorMessage(textBoxUpcomingDate)))
    //        {
    //          // ElectionDate present and ok
    //          textBoxElectionType.Text = electionType;
    //          textBoxParty.Text = party;
    //          if (isGenerateDescription)
    //            textBoxUpcomingDesc.Text =
    //              db.Str_Election_Description(textBoxElectionType.Text,
    //                textBoxParty.Text, textBoxUpcomingDate.Text,
    //                ViewState["StateCode"].ToString());
    //          isValidElectionTypePartyDate = true;
    //          Msg.Text =
    //            db.Ok("The ElectionType, Party Code and Date are valid." +
    //              " An Election Description has been generated, which you are free to edit." +
    //              " You may now enter an Early Vote Date and/or Registration Date.");
    //        }
    //        else
    //        {
    //          Msg.Text = db.Fail(GetInvalidFutureDateErrorMessage(textBoxUpcomingDate));
    //        }

    //      else
    //      {
    //        isValidElectionTypePartyDate = true;
    //        Msg.Text =
    //          db.Ok("The ElectionType and Party Code are valid." +
    //            " You now need to enter an Election Date.");
    //      }

    //    else
    //    {
    //      if ((IsValidElectionTypeForState(textBoxElectionType)) &&
    //        (!IsValidParty(textBoxParty)))
    //        Msg.Text =
    //          db.Ok("The election type is ok. You now need to enter a party code.");
    //      else if ((!IsValidElectionTypeForState(textBoxElectionType)) &&
    //        (IsValidParty(textBoxParty)))
    //        Msg.Text =
    //          db.Ok("The party code is ok. You now need to enter an election type.");
    //      else
    //        Msg.Text =
    //          db.Fail("The Election Type and Party Code are missing or invalid.");
    //    }

    //    //Upercase
    //    textBoxElectionType.Text = electionType;
    //    textBoxParty.Text = party;
    //  }
    //  return isValidElectionTypePartyDate;
    //}

    //private void Check_All_TextBoxes(TextBox textBoxElectionType,
    //  TextBox textBoxParty, TextBox textBoxUpcomingDate, TextBox textBoxUpcomingDesc,
    //  ITextControl textBoxEarlyDate, ITextControl textBoxRegistrationDate)
    //{
    //  if (!string.IsNullOrEmpty(textBoxElectionType.Text.Trim()))
    //    if (Is_Type_Party_Date_Ok(textBoxElectionType, textBoxParty,
    //      textBoxUpcomingDate, textBoxUpcomingDesc, false))
    //    {
    //      #region Type Party and Date OK

    //      ValidateEarlyVoting(textBoxEarlyDate, textBoxUpcomingDate);

    //      CheckRegistrationDate(textBoxRegistrationDate, textBoxEarlyDate,
    //        textBoxUpcomingDate);

    //      #endregion Type Party and Date OK
    //    }
    //    else
    //      throw new ApplicationException(
    //        "There is a problem with either the election" +
    //          " Type, Party, Date or Description.");
    //  else if ((!string.IsNullOrEmpty(textBoxParty.Text.Trim())) ||
    //    (!string.IsNullOrEmpty(textBoxUpcomingDate.Text.Trim())) ||
    //    (!string.IsNullOrEmpty(textBoxUpcomingDesc.Text.Trim())) ||
    //    (!string.IsNullOrEmpty(textBoxEarlyDate.Text.Trim())) ||
    //    (!string.IsNullOrEmpty(textBoxRegistrationDate.Text.Trim())))
    //    throw new ApplicationException("The Election Type needs to be" +
    //      " identified before any other election data is provided.");
    //}

    //private void Table_ElectionsFuture_Delete()
    //{
    //  var sql = string.Empty;
    //  sql += "DELETE FROM ElectionsFuture";
    //  sql += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
    //  sql += " AND CountyCode = " + db.SQLLit(ViewState["CountyCode"].ToString());
    //  sql += " AND LocalCode = " + db.SQLLit(ViewState["LocalCode"].ToString());
    //  db.ExecuteSQL(sql);
    //}

    //private string sql_Upcoming_Elections()
    //{
    //  //For upcoming elections 
    //  //all the State elections are presented
    //  //even for county and local administrators
    //  //because all elections are created at the State level
    //  //and no county or local office contests 
    //  //have been identified at election creation time.
    //  //
    //  //Then county and local elections are created on the fly
    //  //when the first office contest is identified.
    //  //

    //  var sql = string.Empty;
    //  sql += "SELECT ";
    //  sql += " ElectionKey";
    //  sql += ",StateCode";
    //  sql += ",CountyCode";
    //  sql += ",LocalCode";
    //  sql += ",ElectionYYYYMMDD";
    //  sql += ",ElectionType";
    //  sql += ",NationalPartyCode";
    //  sql += ",ElectionDate";
    //  sql += ",PartyCode";
    //  sql += ",ElectionStatus";
    //  sql += ",ElectionDesc";
    //  sql += ",ElectionAdditionalInfo";
    //  sql += ",ElectionResultsSource";
    //  sql += ",ElectionResultsDate";
    //  sql += ",BallotInstructions";
    //  sql += ",IsViewable";
    //  sql += " FROM Elections";
    //  sql += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
    //  sql += " AND CountyCode = ''";
    //  sql += " AND LocalCode = ''";
    //  sql += " AND Elections.ElectionDate >= " + db.SQLLit(Db.DbToday);
    //  sql += " ORDER BY ElectionDate ASC,ElectionOrder ASC,NationalPartyCode ASC";
    //  return sql;
    //}

    //private string sql_Previous_Elections()
    //{
    //  //For previous elections
    //  //only the specific county or local district elctions 
    //  //are prsented.
    //  //
    //  //The Master Panel contains a utility to delete
    //  //all county and local previous elections
    //  //where there are on office contests identified.

    //  var sql = string.Empty;
    //  sql += "SELECT ";
    //  sql += " ElectionKey";
    //  sql += ",StateCode";
    //  sql += ",CountyCode";
    //  sql += ",LocalCode";
    //  sql += ",ElectionYYYYMMDD";
    //  sql += ",ElectionType";
    //  sql += ",NationalPartyCode";
    //  sql += ",ElectionDate";
    //  sql += ",PartyCode";
    //  sql += ",ElectionStatus";
    //  sql += ",ElectionDesc";
    //  sql += ",ElectionAdditionalInfo";
    //  sql += ",ElectionResultsSource";
    //  sql += ",ElectionResultsDate";
    //  sql += ",BallotInstructions";
    //  sql += ",IsViewable";
    //  sql += ",IsWinnersIdentified";
    //  sql += " FROM Elections";
    //  sql += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
    //  sql += " AND CountyCode = " + db.SQLLit(ViewState["CountyCode"].ToString());
    //  sql += " AND LocalCode = " + db.SQLLit(ViewState["LocalCode"].ToString());
    //  sql += " AND ElectionDate < " + db.SQLLit(Db.DbToday);
    //  sql += " ORDER BY ElectionDate DESC,ElectionOrder ASC,NationalPartyCode ASC";
    //  return sql;
    //}

    //private void UpcomingElections()
    //{
    //  //electionDate_Not_To_Create = DateTime.MinValue;
    //  var tableUpcomingElections = db.Table(sql_Upcoming_Elections());

    //  //In Page HTML has: <table id="UpcomingHTMLElectionsTable" Class="tablePage">
    //  //to build the upcoming electiong

    //  HtmlTableRow htmlRowUpcomingElection;
    //  if ((tableUpcomingElections == null) ||
    //    (tableUpcomingElections.Rows.Count == 0))
    //  {
    //    //<tr>
    //    htmlRowUpcomingElection =
    //      db.Add_Tr_To_Table_Return_Tr(UpcomingHTMLElectionsTable,
    //        "trHyperLinkButtons");
    //    //<td Class="HyperLink" align="center">
    //    db.Add_Td_To_Tr(htmlRowUpcomingElection,
    //      "No Upcoming Elections have been created", "HyperLink", "left", 1);
    //  }
    //  else
    //  {
    //    foreach (DataRow rowUpcomingElection in tableUpcomingElections.Rows)
    //    {
    //      //<tr>
    //      htmlRowUpcomingElection =
    //        db.Add_Tr_To_Table_Return_Tr(UpcomingHTMLElectionsTable,
    //          "trHyperLinkButtons");
    //      //<td Class="HyperLink" align="left">
    //      var electionDate = (DateTime) rowUpcomingElection["ElectionDate"];

    //      //string Url = db.Url_Admin_Election(
    //      //  Row_UpcomingElection["ElectionKey"].ToString()
    //      //  );
    //      var url =
    //        db.Url_Admin_Election(
    //          AdjustElectionKeyForElectoralClass(rowUpcomingElection["ElectionKey"].ToString()));

    //      var anchorText = electionDate.ToString("MM/dd/yyyy");
    //      anchorText += ": " + rowUpcomingElection["ElectionDesc"];
    //      if (Convert.ToBoolean(rowUpcomingElection["IsViewable"].ToString()) ==
    //        false)
    //        anchorText += " (Not Public Viewable)";
    //      else
    //        anchorText += " (Viewable)";

    //      var anchor = db.Anchor(url, anchorText);

    //      db.Add_Td_To_Tr(htmlRowUpcomingElection, anchor, "HyperLink", "left", 1);
    //    }
    //  }
    //}

    //private void PreviousElections()
    //{
    //  var tablePreviousElections = db.Table(sql_Previous_Elections());
    //  //In Page HTML <table id="PreviousElectionsHTMLTable" Class="tablePage">
    //  HtmlTableRow htmlRowPreviousElection;
    //  if ((tablePreviousElections == null) ||
    //    (tablePreviousElections.Rows.Count == 0))
    //  {
    //    #region No previous elections data are available

    //    //<tr>
    //    htmlRowPreviousElection =
    //      db.Add_Tr_To_Table_Return_Tr(PreviousElectionsHTMLTable,
    //        "trHyperLinkButtons");
    //    //<td Class="HyperLink" align="center">
    //    db.Add_Td_To_Tr(htmlRowPreviousElection,
    //      "No previous elections data are available", "HyperLink", "left", 1);

    //    #endregion No previous elections data are available
    //  }
    //  else
    //  {
    //    foreach (DataRow rowPreviousElection in tablePreviousElections.Rows)
    //    {
    //      //<tr>
    //      htmlRowPreviousElection =
    //        db.Add_Tr_To_Table_Return_Tr(PreviousElectionsHTMLTable,
    //          "trHyperLinkButtons");
    //      //<td Class="HyperLink" align="left">
    //      var electionDate = (DateTime) rowPreviousElection["ElectionDate"];

    //      var url =
    //        db.Url_Admin_Election(
    //          AdjustElectionKeyForElectoralClass(rowPreviousElection["ElectionKey"].ToString()));

    //      var electionAnchor = electionDate.ToString("MM/dd/yyyy");
    //      electionAnchor += ": " + rowPreviousElection["ElectionDesc"];
    //      if (Convert.ToBoolean(rowPreviousElection["IsViewable"].ToString()) ==
    //        false)
    //        electionAnchor += " (Not Viewable)";

    //      electionAnchor = db.Anchor(url, electionAnchor);

    //      var electionDescription = electionAnchor;

    //      if (
    //        Convert.ToBoolean(rowPreviousElection["IsWinnersIdentified"].ToString()))
    //        electionDescription += " (winners identified)";

    //      //<td Class="HyperLink" align="left">
    //      db.Add_Td_To_Tr(htmlRowPreviousElection, electionDescription, "HyperLink",
    //        "left", 1);
    //    }
    //  }
    //}

    //private void ElectionsFuture_Load_TextBoxes()
    //{
    //  #region Delete any Future Elections with previous dates

    //  var sql = " DELETE FROM ElectionsFuture";
    //  sql += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
    //  sql += " AND ElectionDate < " + db.SQLLit(Db.DbToday);
    //  db.ExecuteSQL(sql);

    //  #endregion Delete any Future Elections with previous dates

    //  sql = " ElectionsFuture";
    //  sql += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
    //  sql += " AND CountyCode = " + db.SQLLit(ViewState["CountyCode"].ToString());
    //  sql += " AND LocalCode = " + db.SQLLit(ViewState["LocalCode"].ToString());
    //  sql += " ORDER BY ElectionDate ASC";
    //  var elections = db.Rows_Count_From(sql);
    //  if ((!string.IsNullOrEmpty(ViewState["StateCode"].ToString())) &&
    //    (elections > 0))
    //  {
    //    sql = " SELECT ";
    //    sql += " StateCode";
    //    sql += ",CountyCode";
    //    sql += ",LocalCode";
    //    sql += ",ElectionDate";
    //    sql += ",ElectionDesc";
    //    sql += ",EarlyVotingDate";
    //    sql += ",RegistrationDate";
    //    sql += ",ElectionType";
    //    sql += ",PartyCode";
    //    sql += " FROM ElectionsFuture";
    //    sql += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
    //    sql += " AND CountyCode = " + db.SQLLit(ViewState["CountyCode"].ToString());
    //    sql += " AND LocalCode = " + db.SQLLit(ViewState["LocalCode"].ToString());
    //    sql += " ORDER BY ElectionDate ASC";

    //    var electionsTable = db.Table(sql);

    //    if (elections > 0)
    //    {
    //      TextBoxType1.Text = electionsTable.Rows[0]["ElectionType"].ToString();
    //      TextBoxParty1.Text = electionsTable.Rows[0]["PartyCode"].ToString();
    //      TextBoxUpcomingDate1.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[0]["ElectionDate"]));
    //      TextBoxUpcomingDesc1.Text =
    //        electionsTable.Rows[0]["ElectionDesc"].ToString();
    //      TextBoxEarlyDate1.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[0]["EarlyVotingDate"]));
    //      TextBoxRegistrationDate1.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[0]["RegistrationDate"]));
    //    }
    //    if (elections > 1)
    //    {
    //      TextBoxType2.Text = electionsTable.Rows[1]["ElectionType"].ToString();
    //      TextBoxParty2.Text = electionsTable.Rows[1]["PartyCode"].ToString();
    //      TextBoxUpcomingDate2.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[1]["ElectionDate"]));
    //      TextBoxUpcomingDesc2.Text =
    //        electionsTable.Rows[1]["ElectionDesc"].ToString();
    //      TextBoxEarlyDate2.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[1]["EarlyVotingDate"]));
    //      TextBoxRegistrationDate2.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[1]["RegistrationDate"]));
    //    }
    //    if (elections > 2)
    //    {
    //      TextBoxType3.Text = electionsTable.Rows[2]["ElectionType"].ToString();
    //      TextBoxParty3.Text = electionsTable.Rows[2]["PartyCode"].ToString();
    //      TextBoxUpcomingDate3.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[2]["ElectionDate"]));
    //      TextBoxUpcomingDesc3.Text =
    //        electionsTable.Rows[2]["ElectionDesc"].ToString();
    //      TextBoxEarlyDate3.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[2]["EarlyVotingDate"]));
    //      TextBoxRegistrationDate3.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[2]["RegistrationDate"]));
    //    }
    //    if (elections > 3)
    //    {
    //      TextBoxType4.Text = electionsTable.Rows[3]["ElectionType"].ToString();
    //      TextBoxParty4.Text = electionsTable.Rows[3]["PartyCode"].ToString();
    //      TextBoxUpcomingDate4.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[3]["ElectionDate"]));
    //      TextBoxUpcomingDesc4.Text =
    //        electionsTable.Rows[3]["ElectionDesc"].ToString();
    //      TextBoxEarlyDate4.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[3]["EarlyVotingDate"]));
    //      TextBoxRegistrationDate4.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[3]["RegistrationDate"]));
    //    }
    //    if (elections > 4)
    //    {
    //      TextBoxType5.Text = electionsTable.Rows[4]["ElectionType"].ToString();
    //      TextBoxParty5.Text = electionsTable.Rows[4]["PartyCode"].ToString();
    //      TextBoxUpcomingDate5.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[4]["ElectionDate"]));
    //      TextBoxUpcomingDesc5.Text =
    //        electionsTable.Rows[4]["ElectionDesc"].ToString();
    //      TextBoxEarlyDate5.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[4]["EarlyVotingDate"]));
    //      TextBoxRegistrationDate5.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[4]["RegistrationDate"]));
    //    }
    //    if (elections > 5)
    //    {
    //      TextBoxType6.Text = electionsTable.Rows[5]["ElectionType"].ToString();
    //      TextBoxParty6.Text = electionsTable.Rows[5]["PartyCode"].ToString();
    //      TextBoxUpcomingDate6.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[5]["ElectionDate"]));
    //      TextBoxUpcomingDesc6.Text =
    //        electionsTable.Rows[5]["ElectionDesc"].ToString();
    //      TextBoxEarlyDate6.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[5]["EarlyVotingDate"]));
    //      TextBoxRegistrationDate6.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[5]["RegistrationDate"]));
    //    }
    //    if (elections > 6)
    //    {
    //      TextBoxType7.Text = electionsTable.Rows[6]["ElectionType"].ToString();
    //      TextBoxParty7.Text = electionsTable.Rows[6]["PartyCode"].ToString();
    //      TextBoxUpcomingDate7.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[6]["ElectionDate"]));
    //      TextBoxUpcomingDesc7.Text =
    //        electionsTable.Rows[6]["ElectionDesc"].ToString();
    //      TextBoxEarlyDate7.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[6]["EarlyVotingDate"]));
    //      TextBoxRegistrationDate7.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[6]["RegistrationDate"]));
    //    }
    //    if (elections > 7)
    //    {
    //      TextBoxType8.Text = electionsTable.Rows[7]["ElectionType"].ToString();
    //      TextBoxParty8.Text = electionsTable.Rows[7]["PartyCode"].ToString();
    //      TextBoxUpcomingDate8.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[7]["ElectionDate"]));
    //      TextBoxUpcomingDesc8.Text =
    //        electionsTable.Rows[7]["ElectionDesc"].ToString();
    //      TextBoxEarlyDate8.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[7]["EarlyVotingDate"]));
    //      TextBoxRegistrationDate8.Text =
    //        Db.DbDateTime_Long_Or_Empty(
    //          Convert.ToDateTime(electionsTable.Rows[7]["RegistrationDate"]));
    //    }
    //  }
    //}

    //private void PollHours_Load_TextBox()
    //{
    //  TextBoxPollHours.Text = db.States_Str(ViewState["StateCode"].ToString(),
    //    "PollHours");
    //}

    //private void PollHoursUrl_Load_TextBox()
    //{
    //  TextBoxPollHoursUrl.Text = db.States_Str(ViewState["StateCode"].ToString(),
    //    "PollHoursUrl");
    //}

    //private void PollPlacesUrl_Load_TextBox()
    //{
    //  TextBoxPollPlacesUrl.Text = db.States_Str(ViewState["StateCode"].ToString(),
    //    "PollPlacesUrl");
    //}

    //private void Set_HyperLinkCreateNewElection()
    //{
    //  var sql = " SELECT ";
    //  sql += " StateCode";
    //  sql += ",ElectionDate";
    //  sql += ",ElectionDesc";
    //  sql += ",ElectionType";
    //  sql += ",PartyCode";
    //  sql += " FROM ElectionsFuture";
    //  sql += " WHERE StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
    //  sql += " AND CountyCode = " + db.SQLLit(ViewState["CountyCode"].ToString());
    //  sql += " AND LocalCode = " + db.SQLLit(ViewState["LocalCode"].ToString());

    //  sql += " AND ElectionType != 'G'";

    //  sql += " ORDER BY ElectionDate ASC";

    //  var electionKey2Create = string.Empty;
    //  var electionType2Create = string.Empty;
    //  var partyCode2Create = string.Empty;
    //  var electionDate2Create = string.Empty;
    //  var electionDesc2Create = string.Empty;
    //  var tableFutureElections = db.Table(sql);
    //  foreach (DataRow rowFutureElection in tableFutureElections.Rows)
    //    if (electionKey2Create == string.Empty)
    //    {
    //      var electionKey = db.ElectionKey_Build(ViewState["StateCode"].ToString(),
    //        ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString(),
    //        rowFutureElection["ElectionDate"].ToString(),
    //        rowFutureElection["ElectionType"].ToString(),
    //        rowFutureElection["PartyCode"].ToString());
    //      if (!db.Is_Valid_Election(electionKey))
    //      {
    //        electionKey2Create = electionKey;
    //        electionType2Create = rowFutureElection["ElectionType"].ToString();
    //        partyCode2Create = rowFutureElection["PartyCode"].ToString();
    //        electionDate2Create = rowFutureElection["ElectionDate"].ToString();
    //        electionDesc2Create = rowFutureElection["ElectionDesc"].ToString();
    //      }
    //    }

    //  if (!string.IsNullOrEmpty(electionKey2Create))
    //  {
    //    HyperLinkCreateNewElection.Text = "Create " + electionDesc2Create;
    //    HyperLinkCreateNewElection.NavigateUrl =
    //      db.Url_Admin_ElectionCreate(ViewState["StateCode"].ToString(),
    //        electionType2Create, partyCode2Create,
    //        Convert.ToDateTime(electionDate2Create), electionDesc2Create);
    //  }
    //  else
    //    HyperLinkCreateNewElection.Text =
    //      "No Future Election is Available to be created";
    //}

    //private void ElectionFuture_Insert(ITextControl textBoxUpcomingDesc,
    //  ITextControl textBoxUpcomingDate, ITextControl textBoxEarlyDate,
    //  ITextControl textBoxRegistration, ITextControl textBoxElectionType, ITextControl textBoxParty)
    //{
    //  if ((textBoxUpcomingDate.Text.Trim() != string.Empty) &&
    //    (textBoxUpcomingDesc.Text.Trim() != string.Empty))
    //  {
    //    var sql = "INSERT INTO ElectionsFuture";
    //    sql += "(";
    //    sql += "StateCode";
    //    sql += ",CountyCode";
    //    sql += ",LocalCode";
    //    sql += ",ElectionDate";
    //    sql += ",ElectionDesc";
    //    //if (!string.IsNullOrEmpty(textBoxEarlyDate.Text))
    //    sql += ",EarlyVotingDate";
    //    //if (!string.IsNullOrEmpty(textBoxRegistration.Text))
    //    sql += ",RegistrationDate";
    //    sql += ",ElectionType";
    //    sql += ",PartyCode";
    //    sql += ")";
    //    sql += " VALUES(";
    //    sql += db.SQLLit(ViewState["StateCode"].ToString());
    //    sql += "," + db.SQLLit(ViewState["CountyCode"].ToString());
    //    sql += "," + db.SQLLit(ViewState["LocalCode"].ToString());
    //    sql += "," +
    //      db.SQLLit(Db.DbDateTime(Convert.ToDateTime(textBoxUpcomingDate.Text)));
    //    sql += "," + db.SQLLit(textBoxUpcomingDesc.Text.Trim());
    //    if (!string.IsNullOrEmpty(textBoxEarlyDate.Text))
    //      sql += "," +
    //        db.SQLLit(Db.DbDateTime(Convert.ToDateTime(textBoxEarlyDate.Text)));
    //    else
    //      sql += ",'1900-01-01 00:00:00'";
    //    if (!string.IsNullOrEmpty(textBoxRegistration.Text))
    //      sql += "," +
    //        db.SQLLit(Db.DbDateTime(Convert.ToDateTime(textBoxRegistration.Text)));
    //    else
    //      sql += ",'1900-01-01 00:00:00'";
    //    sql += "," + db.SQLLit(textBoxElectionType.Text.Trim()
    //      .ToUpper());
    //    sql += "," + db.SQLLit(textBoxParty.Text.Trim()
    //      .ToUpper());
    //    sql += ")";
    //    db.ExecuteSQL(sql);
    //  }
    //}

    //private void Table_ElectionsFuture_Insert()
    //{
    //  ElectionFuture_Insert(TextBoxUpcomingDesc1, TextBoxUpcomingDate1,
    //    TextBoxEarlyDate1, TextBoxRegistrationDate1, TextBoxType1, TextBoxParty1);
    //  ElectionFuture_Insert(TextBoxUpcomingDesc2, TextBoxUpcomingDate2,
    //    TextBoxEarlyDate2, TextBoxRegistrationDate2, TextBoxType2, TextBoxParty2);
    //  ElectionFuture_Insert(TextBoxUpcomingDesc3, TextBoxUpcomingDate3,
    //    TextBoxEarlyDate3, TextBoxRegistrationDate3, TextBoxType3, TextBoxParty3);
    //  ElectionFuture_Insert(TextBoxUpcomingDesc4, TextBoxUpcomingDate4,
    //    TextBoxEarlyDate4, TextBoxRegistrationDate4, TextBoxType4, TextBoxParty4);

    //  ElectionFuture_Insert(TextBoxUpcomingDesc5, TextBoxUpcomingDate5,
    //    TextBoxEarlyDate5, TextBoxRegistrationDate5, TextBoxType5, TextBoxParty5);
    //  ElectionFuture_Insert(TextBoxUpcomingDesc6, TextBoxUpcomingDate6,
    //    TextBoxEarlyDate6, TextBoxRegistrationDate6, TextBoxType6, TextBoxParty6);
    //  ElectionFuture_Insert(TextBoxUpcomingDesc7, TextBoxUpcomingDate7,
    //    TextBoxEarlyDate7, TextBoxRegistrationDate7, TextBoxType7, TextBoxParty7);
    //  ElectionFuture_Insert(TextBoxUpcomingDesc8, TextBoxUpcomingDate8,
    //    TextBoxEarlyDate8, TextBoxRegistrationDate8, TextBoxType8, TextBoxParty8);
    //}

    //#endregion Private

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
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

    //    try
    //    {
    //      #region Help links

    //      if (SecurePage.IsMasterUser)
    //      {
    //        HyperLink_Interns.Visible = true;
    //        HyperLink_Help.Visible = false;
    //      }
    //      else
    //      {
    //        HyperLink_Help.Visible = true;
    //        HyperLink_Interns.Visible = false;
    //      }

    //      #endregion Help links

    //      #region All Controls Not Visible

    //      TableCreateNewElectionState.Visible = false;
    //      TableCreateNewElectionPresident.Visible = false;
    //      //TableElectionReports.Visible = false;
    //      HyperLinkCreateNewElection.Visible = false;
    //      HyperLinkCreateNewElectionPresident.Visible = false;

    //      //for States only
    //      TableFutureElections1.Visible = false;
    //      TableFutureElections2.Visible = false;
    //      TableElectionSpecs.Visible = false;
    //      TableElectionPolls.Visible = false;

    //      #endregion All Controls Not Visible

    //      if (StateCache.IsValidStateCode(ViewState["StateCode"].ToString()))
    //      {
    //        #region State

    //        #region Tables Visible

    //        TableFutureElections1.Visible = true;
    //        TableFutureElections2.Visible = true;
    //        TableElectionSpecs.Visible = true;
    //        TableElectionPolls.Visible = true;
    //        if (SecurePage.IsSuperUser)
    //        {
    //          TableCreateNewElectionState.Visible = true;
    //          HyperLinkCreateNewElection.Visible = true;

    //          Set_HyperLinkCreateNewElection();
    //        }

    //        #endregion Tables Visible

    //        #region PageTitle

    //        PageTitle.Text = Offices.GetElectoralClassDescription(ViewState["StateCode"].ToString(),
    //          ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString());
    //        PageTitle.Text += "<br>";
    //        PageTitle.Text += "ELECTIONS";

    //        #endregion PageTitle

    //        PollHours_Load_TextBox();

    //        PollHoursUrl_Load_TextBox();

    //        PollPlacesUrl_Load_TextBox();

    //        ElectionsFuture_Load_TextBoxes();

    //        Set_HyperLinkCreateNewElection();

    //        #endregion State
    //      }
    //      else switch (QueryState)
    //      {
    //        case "US":
    //          if (SecurePage.IsSuperUser)
    //            TableCreateNewElectionPresident.Visible = true;
    //          PageTitle.Text = "Remaining Presidential Party Primary Candidates";
    //          break;
    //        case "PP":
    //          if (SecurePage.IsSuperUser)
    //            TableCreateNewElectionPresident.Visible = true;
    //          PageTitle.Text =
    //            "Template of Major Presidential Party Primary Candidates";
    //          break;
    //        default:
    //          if (db.Is_StateCode_State_By_State(QueryState))

    //            switch (QueryState)
    //            {
    //              case "U1":
    //                PageTitle.Text = "President State-By-State ";
    //                PageTitle.Text += "<br>";
    //                PageTitle.Text += "ELECTIONS";
    //                break;
    //              case "U2":
    //                PageTitle.Text = "Senate State-By-State ";
    //                PageTitle.Text += "<br>";
    //                PageTitle.Text += "ELECTIONS";
    //                break;
    //              case "U3":
    //                PageTitle.Text = "House of Representatives State-By-State ";
    //                PageTitle.Text += "<br>";
    //                PageTitle.Text += "ELECTIONS";
    //                break;
    //              case "U4":
    //                PageTitle.Text = "Governor State-By-State ";
    //                PageTitle.Text += "<br>";
    //                PageTitle.Text += "ELECTIONS";
    //                break;
    //            }
    //          break;
    //      }

    //      UpcomingElections();

    //      PreviousElections();
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
    //  catch /*(Exception ex)*/ { }
    //}

    //protected void ButtonRemakeUpcomingElectionReports_Click(object sender,
    //  EventArgs e)
    //{
    //  try
    //  {
    //    Server.ScriptTimeout = 300; //300 sec = 5 min
    //    var msgReturn = "Upcoming Elections Remade: ";

    //    //DataTable Table_UpcomingElections = sql.Table_Elections_Upcoming(
    //    //  ViewState["StateCode"].ToString()
    //    //  , ViewState["CountyCode"].ToString()
    //    //  , ViewState["LocalCode"].ToString()
    //    //  );
    //    var tableUpcomingElections = db.Table(sql_Upcoming_Elections());
    //    foreach (DataRow upcomingElectionRow in tableUpcomingElections.Rows)
    //    {
    //      db.Report_Election_Update(PageCache,
    //        upcomingElectionRow["ElectionKey"].ToString());
    //      msgReturn += "<br>" + upcomingElectionRow["ElectionDesc"];
    //    }

    //    UpcomingElections();

    //    PreviousElections();

    //    Msg.Text = db.Ok(msgReturn);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonRemakePreviousElectionReports_Click(object sender,
    //  EventArgs e)
    //{
    //  try
    //  {
    //    Server.ScriptTimeout = 300; //300 sec = 5 min
    //    var msgReturn = "Previous Elections Remade: ";

    //    //DataTable Table_PreviousElections = sql.Table_Elections_Previous(
    //    //  ViewState["StateCode"].ToString()
    //    //  , ViewState["CountyCode"].ToString()
    //    //  , ViewState["LocalCode"].ToString()
    //    //  );
    //    var tablePreviousElections = db.Table(sql_Previous_Elections());
    //    foreach (DataRow previousElectionRow in tablePreviousElections.Rows)
    //    {
    //      db.Report_Election_Update(PageCache,
    //        previousElectionRow["ElectionKey"].ToString());
    //      msgReturn += "<br>" + previousElectionRow["ElectionDesc"];
    //    }

    //    UpcomingElections();

    //    PreviousElections();

    //    Msg.Text = db.Ok(msgReturn);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonFutureElectionDates_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    #region Check Future Elections Textboxes

    //    Check_All_TextBoxes(TextBoxType1, TextBoxParty1, TextBoxUpcomingDate1,
    //      TextBoxUpcomingDesc1, TextBoxEarlyDate1, TextBoxRegistrationDate1);

    //    Check_All_TextBoxes(TextBoxType2, TextBoxParty2, TextBoxUpcomingDate2,
    //      TextBoxUpcomingDesc2, TextBoxEarlyDate2, TextBoxRegistrationDate2);

    //    Check_All_TextBoxes(TextBoxType3, TextBoxParty3, TextBoxUpcomingDate3,
    //      TextBoxUpcomingDesc3, TextBoxEarlyDate3, TextBoxRegistrationDate3);

    //    Check_All_TextBoxes(TextBoxType4, TextBoxParty4, TextBoxUpcomingDate4,
    //      TextBoxUpcomingDesc4, TextBoxEarlyDate4, TextBoxRegistrationDate4);

    //    Check_All_TextBoxes(TextBoxType5, TextBoxParty5, TextBoxUpcomingDate5,
    //      TextBoxUpcomingDesc5, TextBoxEarlyDate5, TextBoxRegistrationDate5);

    //    Check_All_TextBoxes(TextBoxType6, TextBoxParty6, TextBoxUpcomingDate6,
    //      TextBoxUpcomingDesc6, TextBoxEarlyDate6, TextBoxRegistrationDate6);

    //    Check_All_TextBoxes(TextBoxType7, TextBoxParty7, TextBoxUpcomingDate7,
    //      TextBoxUpcomingDesc7, TextBoxEarlyDate7, TextBoxRegistrationDate7);

    //    Check_All_TextBoxes(TextBoxType8, TextBoxParty8, TextBoxUpcomingDate8,
    //      TextBoxUpcomingDesc8, TextBoxEarlyDate8, TextBoxRegistrationDate8);

    //    #endregion Check Future Elections Textboxes

    //    Table_ElectionsFuture_Delete();

    //    Table_ElectionsFuture_Insert();

    //    #region Clear TextBoxes

    //    TextBoxType1.Text = string.Empty;
    //    TextBoxParty1.Text = string.Empty;
    //    TextBoxUpcomingDate1.Text = string.Empty;
    //    TextBoxUpcomingDesc1.Text = string.Empty;
    //    TextBoxEarlyDate1.Text = string.Empty;
    //    TextBoxRegistrationDate1.Text = string.Empty;

    //    TextBoxType2.Text = string.Empty;
    //    TextBoxParty2.Text = string.Empty;
    //    TextBoxUpcomingDate2.Text = string.Empty;
    //    TextBoxUpcomingDesc2.Text = string.Empty;
    //    TextBoxEarlyDate2.Text = string.Empty;
    //    TextBoxRegistrationDate2.Text = string.Empty;

    //    TextBoxType3.Text = string.Empty;
    //    TextBoxParty3.Text = string.Empty;
    //    TextBoxUpcomingDate3.Text = string.Empty;
    //    TextBoxUpcomingDesc3.Text = string.Empty;
    //    TextBoxEarlyDate3.Text = string.Empty;
    //    TextBoxRegistrationDate3.Text = string.Empty;

    //    TextBoxType4.Text = string.Empty;
    //    TextBoxParty4.Text = string.Empty;
    //    TextBoxUpcomingDate4.Text = string.Empty;
    //    TextBoxUpcomingDesc4.Text = string.Empty;
    //    TextBoxEarlyDate4.Text = string.Empty;
    //    TextBoxRegistrationDate4.Text = string.Empty;
    //    //--

    //    TextBoxType5.Text = string.Empty;
    //    TextBoxParty5.Text = string.Empty;
    //    TextBoxUpcomingDate5.Text = string.Empty;
    //    TextBoxUpcomingDesc5.Text = string.Empty;
    //    TextBoxEarlyDate5.Text = string.Empty;
    //    TextBoxRegistrationDate5.Text = string.Empty;

    //    TextBoxType6.Text = string.Empty;
    //    TextBoxParty6.Text = string.Empty;
    //    TextBoxUpcomingDate6.Text = string.Empty;
    //    TextBoxUpcomingDesc6.Text = string.Empty;
    //    TextBoxEarlyDate6.Text = string.Empty;
    //    TextBoxRegistrationDate6.Text = string.Empty;

    //    TextBoxType7.Text = string.Empty;
    //    TextBoxParty7.Text = string.Empty;
    //    TextBoxUpcomingDate7.Text = string.Empty;
    //    TextBoxUpcomingDesc7.Text = string.Empty;
    //    TextBoxEarlyDate7.Text = string.Empty;
    //    TextBoxRegistrationDate7.Text = string.Empty;

    //    TextBoxType8.Text = string.Empty;
    //    TextBoxParty8.Text = string.Empty;
    //    TextBoxUpcomingDate8.Text = string.Empty;
    //    TextBoxUpcomingDesc8.Text = string.Empty;
    //    TextBoxEarlyDate8.Text = string.Empty;
    //    TextBoxRegistrationDate8.Text = string.Empty;

    //    #endregion Clear TextBoxes

    //    ElectionsFuture_Load_TextBoxes();

    //    UpcomingElections();

    //    PreviousElections();

    //    Set_HyperLinkCreateNewElection();

    //    Msg.Text = db.Ok("Future election dates and descriptions were recorded.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxPollHours_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    db.States_Update_Str(ViewState["StateCode"].ToString(), "PollHours",
    //      TextBoxPollHours.Text.Trim());

    //    PollHours_Load_TextBox();

    //    Msg.Text = db.Ok("Poll Hours was recorded");

    //    //Visible_Controls();

    //    //Page_Title();

    //    //UpcomingElections();

    //    //PreviousElections();

    //    //ElectionsFuture_Load_TextBoxes();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxPollHoursUrl_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    db.States_Update_Str(ViewState["StateCode"].ToString(), "PollHoursUrl",
    //      db.Str_Remove_Http(TextBoxPollHoursUrl.Text.Trim()));

    //    PollHoursUrl_Load_TextBox();

    //    Msg.Text = db.Ok("Poll Hours URL was recorded");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxPollPlacesUrl_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    db.States_Update_Str(ViewState["StateCode"].ToString(), "PollPlacesUrl",
    //      db.Str_Remove_Http(TextBoxPollPlacesUrl.Text.Trim()));

    //    PollPlacesUrl_Load_TextBox();

    //    Msg.Text = db.Ok("Poll Places URL was recorded");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxType1_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType1, TextBoxParty1, TextBoxUpcomingDate1,
    //      TextBoxUpcomingDesc1, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxType2_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType2, TextBoxParty2, TextBoxUpcomingDate2,
    //      TextBoxUpcomingDesc2, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxType3_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType3, TextBoxParty3, TextBoxUpcomingDate3,
    //      TextBoxUpcomingDesc3, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxType4_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType4, TextBoxParty4, TextBoxUpcomingDate4,
    //      TextBoxUpcomingDesc4, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxType5_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType5, TextBoxParty5, TextBoxUpcomingDate5,
    //      TextBoxUpcomingDesc5, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxType6_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType6, TextBoxParty6, TextBoxUpcomingDate6,
    //      TextBoxUpcomingDesc6, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxType7_TextChanged(object sender, EventArgs e)
    //{
    //  CheckForIllegalInputInAllTextBoxes();

    //  try
    //  {
    //    Is_Type_Party_Date_Ok(TextBoxType7, TextBoxParty7, TextBoxUpcomingDate7,
    //      TextBoxUpcomingDesc7, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxType8_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType8, TextBoxParty8, TextBoxUpcomingDate8,
    //      TextBoxUpcomingDesc8, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxParty1_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType1, TextBoxParty1, TextBoxUpcomingDate1,
    //      TextBoxUpcomingDesc1, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxParty2_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType2, TextBoxParty2, TextBoxUpcomingDate2,
    //      TextBoxUpcomingDesc2, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxParty3_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType3, TextBoxParty3, TextBoxUpcomingDate3,
    //      TextBoxUpcomingDesc3, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxParty4_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType4, TextBoxParty4, TextBoxUpcomingDate4,
    //      TextBoxUpcomingDesc4, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxParty5_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType5, TextBoxParty5, TextBoxUpcomingDate5,
    //      TextBoxUpcomingDesc5, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxParty6_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType6, TextBoxParty6, TextBoxUpcomingDate6,
    //      TextBoxUpcomingDesc6, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxParty7_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType7, TextBoxParty7, TextBoxUpcomingDate7,
    //      TextBoxUpcomingDesc7, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxParty8_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType8, TextBoxParty8, TextBoxUpcomingDate8,
    //      TextBoxUpcomingDesc8, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDate1_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType1, TextBoxParty1, TextBoxUpcomingDate1,
    //      TextBoxUpcomingDesc1, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDate2_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType2, TextBoxParty2, TextBoxUpcomingDate2,
    //      TextBoxUpcomingDesc2, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDate3_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType3, TextBoxParty3, TextBoxUpcomingDate3,
    //      TextBoxUpcomingDesc3, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDate4_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType4, TextBoxParty4, TextBoxUpcomingDate4,
    //      TextBoxUpcomingDesc4, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDate5_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType5, TextBoxParty5, TextBoxUpcomingDate5,
    //      TextBoxUpcomingDesc5, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDate6_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType6, TextBoxParty6, TextBoxUpcomingDate6,
    //      TextBoxUpcomingDesc6, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDate7_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType7, TextBoxParty7, TextBoxUpcomingDate7,
    //      TextBoxUpcomingDesc7, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDate8_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Is_Type_Party_Date_Ok(TextBoxType8, TextBoxParty8, TextBoxUpcomingDate8,
    //      TextBoxUpcomingDesc8, true);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxEarlyDate1_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    ValidateEarlyVoting(TextBoxEarlyDate1, TextBoxUpcomingDate1);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxEarlyDate2_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    ValidateEarlyVoting(TextBoxEarlyDate2, TextBoxUpcomingDate2);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxEarlyDate3_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    ValidateEarlyVoting(TextBoxEarlyDate3, TextBoxUpcomingDate3);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxEarlyDate4_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    ValidateEarlyVoting(TextBoxEarlyDate4, TextBoxUpcomingDate4);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxEarlyDate5_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    ValidateEarlyVoting(TextBoxEarlyDate5, TextBoxUpcomingDate5);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxEarlyDate6_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    ValidateEarlyVoting(TextBoxEarlyDate6, TextBoxUpcomingDate6);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxEarlyDate7_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    ValidateEarlyVoting(TextBoxEarlyDate7, TextBoxUpcomingDate7);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxEarlyDate8_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    ValidateEarlyVoting(TextBoxEarlyDate8, TextBoxUpcomingDate8);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxRegistrationDate1_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    CheckRegistrationDate(TextBoxRegistrationDate1, TextBoxEarlyDate1,
    //      TextBoxUpcomingDate1);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxRegistrationDate2_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    CheckRegistrationDate(TextBoxRegistrationDate2, TextBoxEarlyDate2,
    //      TextBoxUpcomingDate2);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxRegistrationDate3_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    CheckRegistrationDate(TextBoxRegistrationDate3, TextBoxEarlyDate3,
    //      TextBoxUpcomingDate3);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxRegistrationDate4_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    CheckRegistrationDate(TextBoxRegistrationDate4, TextBoxEarlyDate4,
    //      TextBoxUpcomingDate4);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxRegistrationDate5_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    CheckRegistrationDate(TextBoxRegistrationDate5, TextBoxEarlyDate5,
    //      TextBoxUpcomingDate5);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxRegistrationDate6_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    CheckRegistrationDate(TextBoxRegistrationDate6, TextBoxEarlyDate6,
    //      TextBoxUpcomingDate6);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxRegistrationDate7_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    CheckRegistrationDate(TextBoxRegistrationDate7, TextBoxEarlyDate7,
    //      TextBoxUpcomingDate7);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxRegistrationDate8_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    CheckRegistrationDate(TextBoxRegistrationDate8, TextBoxEarlyDate8,
    //      TextBoxUpcomingDate8);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDesc1_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Msg.Text =
    //      db.Ok(
    //        "The election description change will be made when the Submit Button is clicked.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDesc2_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Msg.Text =
    //      db.Ok(
    //        "The election description change will be made when the Submit Button is clicked.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDesc3_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Msg.Text =
    //      db.Ok(
    //        "The election description change will be made when the Submit Button is clicked.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDesc4_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Msg.Text =
    //      db.Ok(
    //        "The election description change will be made when the Submit Button is clicked.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDesc5_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Msg.Text =
    //      db.Ok(
    //        "The election description change will be made when the Submit Button is clicked.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDesc6_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Msg.Text =
    //      db.Ok(
    //        "The election description change will be made when the Submit Button is clicked.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDesc7_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Msg.Text =
    //      db.Ok(
    //        "The election description change will be made when the Submit Button is clicked.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxUpcomingDesc8_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckForIllegalInputInAllTextBoxes();

    //    Msg.Text =
    //      db.Ok(
    //        "The election description change will be made when the Submit Button is clicked.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextboxElectionDate_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    #region checks

    //    CheckForIllegalInputInAllTextBoxes();

    //    if (RadioButtonListPresPrimaryType.SelectedIndex == -1)
    //      throw new ApplicationException(
    //        "A Presidential Primary Type needs to be selected.");

    //    if (RadioButtonListParty.SelectedIndex == -1)
    //      throw new ApplicationException("A party needs to be selected.");

    //    if (TextboxElectionDate.Text.Trim() == string.Empty) //empty 
    //      throw new ApplicationException("The Future Date textbox is empty!");

    //    if (!db.Is_Valid_Date(TextboxElectionDate.Text.Trim())) //bad date
    //      throw new ApplicationException("You entered a bad date!");

    //    if (Convert.ToDateTime(TextboxElectionDate.Text.Trim()) < DateTime.Today)
    //      //in the future
    //      throw new ApplicationException(
    //        "The Future Date for a new election needs to be in the future!");

    //    #endregion checks

    //    HyperLinkCreateNewElectionPresident.Visible = true;

    //    TextboxElectionDate.Text = Convert.ToDateTime(TextboxElectionDate.Text.Trim())
    //      .ToString("MMMM d, yyyy");

    //    var electionDescription = db.Str_Election_Description("A",
    //      RadioButtonListParty.SelectedValue, TextboxElectionDate.Text
    //      //, "PP"
    //      , QueryState);

    //    HyperLinkCreateNewElectionPresident.Text = "Create " + electionDescription;
    //    HyperLinkCreateNewElectionPresident.NavigateUrl = db.Url_Admin_ElectionCreate
    //      ( //"PP"
    //        QueryState, "A", RadioButtonListParty.SelectedValue,
    //        Convert.ToDateTime(TextboxElectionDate.Text), electionDescription);

    //    //ViewState is enabled but the state is lost
    //    //and needs to be created when a exception is thrown.
    //    UpcomingElections();

    //    PreviousElections();

    //    Msg.Text =
    //      db.Ok(
    //        "Click the Create link below to be presented with a form to create this election.");
    //  }
    //  catch (Exception ex)
    //  {
    //    //ViewState is enabled but the state is lost
    //    //and needs to be created when a exception is thrown.
    //    UpcomingElections();

    //    PreviousElections();

    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    #region Dead code

    //protected void Check_ElectionDate(
    //  TextBox textBoxUpcomingDate)
    //{
    //  if (!string.IsNullOrEmpty(textBoxUpcomingDate.Text.Trim()))
    //  {
    //    if (!string.IsNullOrEmpty(Check_Date_ErrMsg(textBoxUpcomingDate)))
    //      throw new ApplicationException(Check_Date_ErrMsg(textBoxUpcomingDate));
    //  }
    //}

    //protected void Check_Party(
    //  TextBox textBoxElectionType
    //  , TextBox textBoxParty)
    //{
    //  if (
    //    (textBoxElectionType.Text.Trim().ToUpper() == "P")
    //    || (textBoxElectionType.Text.Trim().ToUpper() == "B")
    //   )
    //  {
    //    if (
    //      (textBoxParty.Text.Trim().ToUpper() == "D")
    //      || (textBoxParty.Text.Trim().ToUpper() == "R")
    //      || (textBoxParty.Text.Trim().ToUpper() == "G")
    //      || (textBoxParty.Text.Trim().ToUpper() == "L")
    //      )
    //    {
    //      textBoxParty.Text = textBoxParty.Text.Trim().ToUpper();
    //    }
    //    else
    //    {
    //      Msg.Text = db.Fail("The Party must be D,R,G or L.");
    //    }
    //  }
    //  else
    //  {
    //    if (!string.IsNullOrEmpty(textBoxParty.Text.Trim()))
    //      Msg.Text = db.Fail("A Party Code is not allowed.");
    //  }
    //}

    //protected string Str_Election_Description(
    //  TextBox textBoxType
    //  , TextBox textBoxParty
    //  , TextBox textBoxUpcomingDate
    //  )
    //{
    //  //textBoxType = TextBoxType1;
    //  //textBoxParty = TextBoxParty1;
    //  //textBoxUpcomingDate = TextBoxUpcomingDate1;

    //  db.Throw_Exception_TextBox_Script(textBoxUpcomingDate);
    //  Check_ElectionDate(
    //    textBoxType
    //    , textBoxParty
    //    , textBoxUpcomingDate
    //    );

    //  //TextBoxUpcomingDesc1.Text = db.Str_Election_Description(
    //  Msg.Text = db.Ok("You may edit the generated Election Description."
    //    + " You may now optionally enter and Early Voting Date and/or Registration Date.");
    //  return db.Str_Election_Description(
    //    textBoxType.Text.Trim()
    //    , textBoxParty.Text.Trim()
    //    , textBoxUpcomingDate.Text.Trim()
    //    , ViewState["StateCode"].ToString()
    //    );

    //}

    //protected void Check_ElectionDate(
    //  TextBox textBoxElectionType
    //  , TextBox textBoxParty
    //  , TextBox textBoxElectionDate)
    //{
    //  if (textBoxElectionType.Text.Trim() == string.Empty)//empty 
    //    throw new ApplicationException("The Election Type textbox is empty!");

    //  if (
    //    (textBoxElectionType.Text.Trim().ToUpper() == "P")
    //    || (textBoxElectionType.Text.Trim().ToUpper() == "B")
    //   )
    //  {
    //    if (textBoxParty.Text.Trim() == string.Empty)//empty 
    //      throw new ApplicationException("The Party Code textbox is empty!");
    //  }

    //  if (textBoxElectionDate.Text.Trim() == string.Empty)//empty 
    //    throw new ApplicationException("The Future Date textbox is empty!");

    //  if (!db.Is_Valid_Date(textBoxElectionDate.Text.Trim()))//bad date
    //    throw new ApplicationException("You entered a bad date!");

    //  if (Convert.ToDateTime(textBoxElectionDate.Text.Trim()) < DateTime.Today)//in the future
    //    throw new ApplicationException("The Future Date for a new election needs to be in the future!");
    //}

    #endregion Dead code
  }
}