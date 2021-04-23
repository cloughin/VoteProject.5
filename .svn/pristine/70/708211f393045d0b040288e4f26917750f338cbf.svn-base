using System.Data;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB.Vote;
using DB.VoteTemp;
using Vote;

namespace VoteTemp
{
  //public static class EmailWork
  //{
  //  // ReSharper disable UnassignedField.Compiler
  //  // ReSharper disable InconsistentNaming
  //  private static HttpSessionState Session;
  //  private static StateBag ViewState;
  //  private static RadioButtonList RadioButtonList_Election;
  //  private static RadioButtonList RadioButtonList_State;
  //  private static RadioButtonList RadioButtonList_All;
  //  // ReSharper restore InconsistentNaming
  //  // ReSharper restore UnassignedField.Compiler

  //  private static void Insert_TempEmailAddresses(string emailAddr, string contact,
  //    string politicianKey, string officeKey, string electionKey, string stateCode,
  //    string partyKey, string fName, string lName, string title)
  //  {
  //    if ((!string.IsNullOrEmpty(emailAddr)) && (!db.Is_Has_Http(emailAddr)) &&
  //      (db.Is_Valid_Email_Address(emailAddr)))
  //    {
  //      emailAddr = db.Str_Remove_MailTo(emailAddr);

  //      //Make sure email address does not already exist
  //      //as may be case with presidential candidates
  //      if (!TempEmailAddresses.EmailExists(emailAddr))
  //        TempEmailAddresses.Insert(emailAddr, contact, politicianKey, officeKey,
  //          electionKey, stateCode, partyKey, fName, lName, title, string.Empty,
  //          string.Empty);
  //    }
  //  }

  //  private static void Insert_One_Candidate_Email_Addresses_Temp_Table(
  //    DataRow rowElectionsPoliticians)
  //  {
  //    var sql = string.Empty;
  //    sql += " SELECT";
  //    sql += " PoliticianKey";
  //    sql += ",EmailAddrVoteUSA";
  //    sql += ",EmailAddr";
  //    sql += ",StateEmailAddr";
  //    sql += ",CampaignEmail";
  //    sql += ",LDSEmailAddr";
  //    sql += " FROM Politicians";
  //    sql += " WHERE PoliticianKey = " +
  //      db.SQLLit(rowElectionsPoliticians["PoliticianKey"].ToString());
  //    var rowPolitician = db.Row_Optional(sql);
  //    if (rowPolitician != null)
  //    {
  //      #region 5 possible email addresses

  //      Insert_TempEmailAddresses(rowPolitician["EmailAddrVoteUSA"].ToString()
  //        .Trim(),
  //        Politicians.GetFormattedName(rowPolitician["PoliticianKey"].ToString()),
  //        rowPolitician["PoliticianKey"].ToString()
  //          .Trim(), rowElectionsPoliticians["OfficeKey"].ToString()
  //            .Trim(), rowElectionsPoliticians["ElectionKey"].ToString()
  //              .Trim(), rowElectionsPoliticians["StateCode"].ToString()
  //                .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

  //      Insert_TempEmailAddresses(rowPolitician["EmailAddr"].ToString()
  //        .Trim(),
  //        Politicians.GetFormattedName(rowPolitician["PoliticianKey"].ToString()),
  //        rowPolitician["PoliticianKey"].ToString()
  //          .Trim(), rowElectionsPoliticians["OfficeKey"].ToString()
  //            .Trim(), rowElectionsPoliticians["ElectionKey"].ToString()
  //              .Trim(), rowElectionsPoliticians["StateCode"].ToString()
  //                .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

  //      Insert_TempEmailAddresses(rowPolitician["StateEmailAddr"].ToString()
  //        .Trim(),
  //        Politicians.GetFormattedName(rowPolitician["PoliticianKey"].ToString()),
  //        rowPolitician["PoliticianKey"].ToString()
  //          .Trim(), rowElectionsPoliticians["OfficeKey"].ToString()
  //            .Trim(), rowElectionsPoliticians["ElectionKey"].ToString()
  //              .Trim(), rowElectionsPoliticians["StateCode"].ToString()
  //                .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

  //      Insert_TempEmailAddresses(rowPolitician["CampaignEmail"].ToString()
  //        .Trim(),
  //        Politicians.GetFormattedName(rowPolitician["PoliticianKey"].ToString()),
  //        rowPolitician["PoliticianKey"].ToString()
  //          .Trim(), rowElectionsPoliticians["OfficeKey"].ToString()
  //            .Trim(), rowElectionsPoliticians["ElectionKey"].ToString()
  //              .Trim(), rowElectionsPoliticians["StateCode"].ToString()
  //                .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

  //      Insert_TempEmailAddresses(rowPolitician["LDSEmailAddr"].ToString()
  //        .Trim(),
  //        Politicians.GetFormattedName(rowPolitician["PoliticianKey"].ToString()),
  //        rowPolitician["PoliticianKey"].ToString()
  //          .Trim(), rowElectionsPoliticians["OfficeKey"].ToString()
  //            .Trim(), rowElectionsPoliticians["ElectionKey"].ToString()
  //              .Trim(), rowElectionsPoliticians["StateCode"].ToString()
  //                .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

  //      #endregion 5 possible email addresses
  //    }
  //  }

  //  private static void Temp_Email_Addresses_Election(string electionKey)
  //  {
  //    var sql = string.Empty;
  //    sql += " SELECT StateCode,ElectionKey,OfficeKey,PoliticianKey";
  //    sql += " FROM ElectionsPoliticians";
  //    sql += " WHERE ElectionKey =" + db.SQLLit(electionKey);
  //    sql += " AND OfficeKey != 'USPresident'";
  //    sql += " ORDER BY PoliticianKey";
  //    var tableElectionsPoliticiansFuture = db.Table(sql);
  //    foreach (
  //      DataRow rowElectionsPoliticians in tableElectionsPoliticiansFuture.Rows)
  //      Insert_One_Candidate_Email_Addresses_Temp_Table(rowElectionsPoliticians);

  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }

  //  private static void Temp_Email_Addresses_Election_Candidates(string electionKey)
  //  {
  //    TempEmailAddresses.TruncateTable();
  //    var sql = string.Empty;
  //    sql += " SELECT ";
  //    sql += " ElectionKey";
  //    sql += " FROM Elections ";
  //    sql += " WHERE IsViewable = 1";
  //    // Temporarily allow past elections for debug
  //    //SQL += " AND ElectionDate >= " + db.SQLLit(Db.DbToday);
  //    sql += " AND ElectionKey = " + db.SQLLit(electionKey);
  //    sql += " ORDER BY ElectionDate ASC";
  //    var rowElection = db.Row_First_Optional(sql);
  //    if (rowElection != null) Temp_Email_Addresses_Election(electionKey);

  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }

  //  //-> RadioButtonList_Election_SelectedIndexChanged
  //  // ReSharper disable once UnusedMember.Local
  //  private static void Build_TempAddresses_Election()
  //  {
  //    #region Build TempAddresses Table - Load From: To: Cc: Format: Attachments

  //    switch (RadioButtonList_Election.SelectedValue)
  //    {
  //      case "ElectionCustomStates":

  //        Temp_Email_Addresses_State_Contacts(ViewState["StateCode"].ToString(),
  //          ViewState["ElectionKey"].ToString());

  //        break;
  //      case "ElectionCustomCandidates":

  //        Temp_Email_Addresses_Election_Candidates(
  //          ViewState["ElectionKey"].ToString());
  //        break;
  //      case "ElectionCustomParties":

  //        Temp_Email_Addresses_State_Parties();
  //        break;
  //      case "ElectionCompletion":

  //        Temp_Email_Addresses_State_Contacts(ViewState["StateCode"].ToString(),
  //          ViewState["ElectionKey"].ToString());
  //        break;
  //      case "ElectionCandidatesLogin":

  //        Temp_Email_Addresses_Election_Candidates(
  //          ViewState["ElectionKey"].ToString());
  //        break;
  //      case "ElectionPartiesLogin":

  //        Temp_Email_Addresses_State_Parties();
  //        break;
  //    }

  //    #endregion Build TempAddresses Table - Load From: To: Cc: Format: Attachments
  //  }
    
  //  private static void
  //    Temp_Email_Addresses_Elections_Candidates_Next_Upcoming_Viewable_State(
  //    string stateCode)
  //  {
  //    //CHECKED

  //    //db.ExecuteSQL("TRUNCATE TABLE TempEmailAddresses");
  //    TempEmailAddresses.TruncateTable();
  //    var sql = string.Empty;
  //    sql += " SELECT ";
  //    sql += " ElectionKey";
  //    sql += " FROM Elections ";
  //    sql += " WHERE IsViewable = 1";
  //    sql += " AND ElectionDate >= " + db.SQLLit(Db.DbToday);
  //    sql += " AND StateCode = " + db.SQLLit(stateCode);
  //    sql += " ORDER BY ElectionDate ASC";

  //    var rowElection = db.Row_First_Optional(sql);
  //    if (rowElection != null) Temp_Email_Addresses_Election(rowElection["ElectionKey"].ToString());

  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }

  //  //-> Button_View_Substitutions_Click
  //  //-> RadioButtonList_State_SelectedIndexChanged
  //  // ReSharper disable once UnusedMember.Local
  //  private static void Build_TempAddresses_State()
  //  {
  //    #region Build TempAddresses Table - Load From: To: Cc: Format: Attachments

  //    switch (RadioButtonList_State.SelectedValue)
  //    {
  //      case "StateCustom":
  //        Temp_Email_Addresses_State_Contacts();
  //        break;
  //      case "StateCustomCandidates":
  //        Temp_Email_Addresses_Elections_Candidates_Next_Upcoming_Viewable_State(
  //          ViewState["StateCode"].ToString());
  //        break;
  //      case "StateCustomParties":
  //        Temp_Email_Addresses_State_Parties();
  //        break;
  //      case "StatePrimaryRosters":
  //        Temp_Email_Addresses_State_Contacts();
  //        break;
  //      case "StateGeneralRosters":
  //        Temp_Email_Addresses_Election_General_Contacts_State();
  //        break;
  //      case "StateCandidates":
  //        Temp_Email_Addresses_Elections_Candidates_Next_Upcoming_Viewable_State(
  //          ViewState["StateCode"].ToString());
  //        break;
  //    }

  //    #endregion Build TempAddresses Table - Load From: To: Cc: Format: Attachments
  //  }
    
  //  private static void Temp_Email_Addresses_Election_General_Contacts_State()
  //  {
  //    var sql = string.Empty;
  //    sql += " SELECT ";
  //    sql += " ElectionKey";
  //    sql += " FROM Elections ";
  //    sql += " WHERE Elections.StateCode = " +
  //      db.SQLLit(ViewState["StateCode"].ToString());
  //    sql += " AND ElectionDate >= " + db.SQLLit(Db.DbToday);
  //    sql += " AND ElectionType = 'G'";
  //    sql += " AND CountyCode = ''";
  //    sql += " AND LocalCode = ''";
  //    sql += " ORDER BY ElectionDate ASC";
  //    var electionRow = db.Row_First_Optional(sql);
  //    if (electionRow != null)
  //    {
  //      Temp_Email_Addresses_State_Contacts(ViewState["StateCode"].ToString(),
  //        electionRow["ElectionKey"].ToString());
  //      //Label_Election.Text =
  //      //  db.Elections_ElectionDesc(electionRow["ElectionKey"].ToString());
  //    }
  //    else
  //    {
  //      TempEmailAddresses.TruncateTable();
  //      //Label_Emails_Sending.Text = "0";
  //      //Label_Election.Text = "No Upcoming General Election";
  //    }

  //    #region Report last time emails were sent using template

  //    //Label_Emails_Last_Sent_Date.Text =
  //    //  db.States_Date(ViewState["StateCode"].ToString(),
  //    //    "EmailsDateElectionRosterGeneral")
  //    //    .ToString(CultureInfo.InvariantCulture);
  //    //Label_Election.Text = string.Empty;

  //    #endregion Report last time emails were sent using template
  //  }
    
  //  private static void
  //    Temp_Email_Addresses_Elections_Candidates_Upcoming_Viewable_All_States()
  //  {
  //    //CHECKED

  //    //db.ExecuteSQL("TRUNCATE TABLE TempEmailAddresses");
  //    TempEmailAddresses.TruncateTable();

  //    //------------------
  //    var sql = string.Empty;
  //    sql += " SELECT ";
  //    sql += " ElectionKey";
  //    sql += " FROM Elections ";
  //    sql += " WHERE IsViewable = 1";
  //    sql += " AND ElectionDate >= " + db.SQLLit(Db.DbToday);
  //    sql +=
  //      " AND ((StateCode != 'US') AND (StateCode != 'U1') AND (StateCode != 'U2') AND (StateCode != 'U3') AND (StateCode != 'U4'))";
  //    sql += " ORDER BY StateCode,ElectionDate DESC";
  //    var tableElections = db.Table(sql);
  //    foreach (DataRow rowElection in tableElections.Rows) //SQL = string.Empty;
  //      Temp_Email_Addresses_Election(rowElection["ElectionKey"].ToString());

  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }

  //  //-> Button_View_Substitutions_Click
  //  //-> RadioButtonList_All_SelectedIndexChanged
  //  // ReSharper disable once UnusedMember.Local
  //  private static void Build_TempAddresses_All_States()
  //  {
  //    #region Build TempAddresses Table - Load From: To: Cc: Format: Attachments

  //    switch (RadioButtonList_All.SelectedValue)
  //    {
  //      case "AllCustomStates":
  //        Temp_Email_Addresses_States_Contacts();
  //        break;
  //      case "AllCustomStatesPrimaryElection":
  //        Temp_Email_Addresses_States_Contacts_Future_Election("P");
  //        break;
  //      case "AllCustomStatesGeneralElection":
  //        Temp_Email_Addresses_States_Contacts_Future_Election("G");
  //        break;
  //      case "AllCustomStatesOffYearElection":
  //        Temp_Email_Addresses_States_Contacts_Future_Election("O");
  //        break;
  //      case "AllCustomStatesSpecialElection":
  //        Temp_Email_Addresses_States_Contacts_Future_Election("S");
  //        break;
  //      case "AllCustomStatesPresidentialPrimaryElection":
  //        Temp_Email_Addresses_States_Contacts_Future_Election("B");
  //        break;
  //      case "AllCustomStatesNoPrimaryElection":
  //        Temp_Email_Addresses_States_Contacts_NO_Future_Election("P");
  //        break;
  //      case "AllCustomStatesNoGeneralElection":
  //        Temp_Email_Addresses_States_Contacts_NO_Future_Election("G");
  //        break;
  //      case "AllCustomStatesNoOffYearElection":
  //        Temp_Email_Addresses_States_Contacts_NO_Future_Election("O");
  //        break;
  //      case "AllCustomStatesNoSpecialElection":
  //        Temp_Email_Addresses_States_Contacts_NO_Future_Election("S");
  //        break;
  //      case "AllCustomStatesNoPresidentialPrimaryElection":
  //        Temp_Email_Addresses_States_Contacts_NO_Future_Election("B");
  //        break;
  //      case "AllCustomParties":
  //        Temp_Email_Addresses_States_Parties();
  //        break;
  //      case "AllPrimaryRosters":
  //        Temp_Email_Addresses_Election_Primary_Contacts_All_States();
  //        break;
  //      case "AllGeneralRosters":
  //        Temp_Email_Addresses_Election_General_Contacts_All_States();
  //        break;
  //      case "AllCustomCandidates":
  //      case "AllCandidates":
  //        Temp_Email_Addresses_Elections_Candidates_Upcoming_Viewable_All_States();
  //        break;
  //    }

  //    #endregion Build TempAddresses Table - Load From: To: Cc: Format: Attachments
  //  }

  //  private static void Temp_Email_Addresses_States_Contacts()
  //  {
  //    TempEmailAddresses.TruncateTable();

  //    var sql = " SELECT";
  //    sql += " StateCode";
  //    sql += ",Contact";
  //    sql += ",ContactEmail";
  //    sql += ",AltContact";
  //    sql += ",AltEMail";
  //    sql += " FROM States";
  //    sql += " ORDER BY StateCode";
  //    var tableStates = db.Table(sql);
  //    foreach (DataRow rowState in tableStates.Rows) 
  //      Insert_One_State_Email_Addresses_Temp_Table(rowState);

  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }

  //  private static void Temp_Email_Addresses_States_Parties()
  //  {
  //    //NEEDS TO BE TESTED

  //    //db.ExecuteSQL("TRUNCATE TABLE TempEmailAddresses");
  //    TempEmailAddresses.TruncateTable();

  //    var sql = string.Empty;
  //    sql += " SELECT";
  //    sql += " Parties.StateCode";
  //    sql += " ,Parties.PartyCode";
  //    sql += " ,PartiesEmails.PartyKey";
  //    sql += " ,PartiesEmails.PartyEmail";
  //    sql += " ,PartiesEmails.PartyPassword";
  //    sql += " ,PartiesEmails.PartyContactFName";
  //    sql += " ,PartiesEmails.PartyContactLName";
  //    sql += " ,PartiesEmails.PartyContactTitle";
  //    sql += " FROM PartiesEmails,Parties";
  //    sql += " WHERE PartiesEmails.PartyKey = Parties.PartyKey ";

  //    var tableStatePartyEmails = db.Table(sql);
  //    foreach (DataRow rowPartyEmail in tableStatePartyEmails.Rows)
  //      Insert_TempEmailAddresses(rowPartyEmail["PartyEmail"].ToString(),
  //        rowPartyEmail["PartyContactFName"] + " " +
  //          rowPartyEmail["PartyContactLName"], string.Empty, string.Empty
  //        //, db.ElectionKey_Viewable_Latest_Any_Upcoming(
  //        //    Session["UserStateCode"].ToString())
  //        , ViewState["ElectionKey"].ToString(),
  //        rowPartyEmail["StateCode"].ToString(),
  //        rowPartyEmail["PartyKey"].ToString(),
  //        rowPartyEmail["PartyContactFName"].ToString(),
  //        rowPartyEmail["PartyContactLName"].ToString(),
  //        rowPartyEmail["PartyContactTitle"].ToString());

  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }

  //  private static void Temp_Email_Addresses_Election_Primary_Contacts_All_States()
  //  {
  //    TempEmailAddresses.TruncateTable();

  //    var sql = string.Empty;
  //    sql += " SELECT ";
  //    sql += " StateCode";
  //    sql += ",ElectionKey";
  //    sql += " FROM Elections ";
  //    sql += " WHERE ElectionDate >= " + db.SQLLit(Db.DbToday);
  //    sql += " AND ElectionType = 'P'";
  //    sql += " AND CountyCode = ''";
  //    sql += " AND LocalCode = ''";
  //    sql += " ORDER BY ElectionDate ASC";
  //    var tableElection = db.Table(sql);
  //    foreach (DataRow rowElection in tableElection.Rows)
  //      Temp_Email_Addresses_State_Contacts(rowElection["StateCode"].ToString(),
  //        rowElection["ElectionKey"].ToString());
  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }

  //  private static void Temp_Email_Addresses_Election_General_Contacts_All_States()
  //  {
  //    var sql = string.Empty;
  //    sql += " SELECT ";
  //    sql += " StateCode";
  //    sql += ",ElectionKey";
  //    sql += " FROM Elections ";
  //    sql += " WHERE ElectionDate >= " + db.SQLLit(Db.DbToday);
  //    sql += " AND ElectionType = 'G'";
  //    sql += " AND CountyCode = ''";
  //    sql += " AND LocalCode = ''";
  //    sql += " ORDER BY ElectionDate ASC";
  //    var tableElection = db.Table(sql);
  //    foreach (DataRow rowElection in tableElection.Rows)
  //      Temp_Email_Addresses_State_Contacts(rowElection["StateCode"].ToString(),
  //        rowElection["ElectionKey"].ToString());

  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //   //   .ToString(CultureInfo.InvariantCulture);
  //  }

  //  private static void Temp_Email_Addresses_States_Contacts_Future_Election(
  //    string electionType)
  //  {
  //    TempEmailAddresses.TruncateTable();

  //    var sql = " SELECT";
  //    sql += " StateCode";
  //    sql += " FROM ElectionsFuture";
  //    sql += " WHERE ElectionType= " + db.SQLLit(electionType);
  //    sql += " ORDER BY StateCode";
  //    var tableElectionsFuture = db.Table(sql);
  //    foreach (DataRow rowElectionsFuture in tableElectionsFuture.Rows)
  //    {
  //      sql = " SELECT";
  //      sql += " StateCode";
  //      sql += ",Contact";
  //      sql += ",ContactEmail";
  //      sql += ",AltContact";
  //      sql += ",AltEMail";
  //      sql += " FROM States";
  //      sql += " WHERE StateCode=" +
  //        db.SQLLit(rowElectionsFuture["StateCode"].ToString());
  //      var rowState = db.Row_First_Optional(sql);
  //      if (rowState != null) Insert_One_State_Email_Addresses_Temp_Table(rowState);
  //    }

  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }

  //  private static void Temp_Email_Addresses_States_Contacts_NO_Future_Election(
  //    string electionType)
  //  {
  //    TempEmailAddresses.TruncateTable();

  //    var sql = " SELECT";
  //    sql += " StateCode";
  //    sql += ",Contact";
  //    sql += ",ContactEmail";
  //    sql += ",AltContact";
  //    sql += ",AltEMail";
  //    sql += " FROM States";
  //    sql += " ORDER BY StateCode";
  //    var tableStates = db.Table(sql);
  //    foreach (DataRow rowState in tableStates.Rows)
  //    {
  //      sql = " SELECT";
  //      sql += " StateCode";
  //      sql += " FROM ElectionsFuture";
  //      sql += " WHERE StateCode= " + db.SQLLit(rowState["StateCode"].ToString());
  //      sql += " AND ElectionType= " + db.SQLLit(electionType);
  //      var rowElectionsFuture = db.Row_First_Optional(sql);
  //      if (rowElectionsFuture == null) Insert_One_State_Email_Addresses_Temp_Table(rowState);
  //    }

  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }

  //  private static void Insert_One_State_Email_Addresses_Temp_Table(
  //    DataRow rowState)
  //  {
  //    if (rowState != null)
  //    {
  //      #region 2 possible email addresses

  //      Insert_TempEmailAddresses(rowState["ContactEmail"].ToString()
  //        .Trim(), rowState["Contact"].ToString()
  //          .Trim(), string.Empty, string.Empty
  //        //, db.ElectionKey_Upcoming_General(Row_State["StateCode"].ToString())
  //        , string.Empty, rowState["StateCode"].ToString()
  //          .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

  //      Insert_TempEmailAddresses(rowState["AltEMail"].ToString()
  //        .Trim(), rowState["AltContact"].ToString()
  //          .Trim(), string.Empty, string.Empty
  //        //, db.ElectionKey_Upcoming_General(Row_State["StateCode"].ToString())
  //        , string.Empty, rowState["StateCode"].ToString()
  //          .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

  //      #endregion 5 possible email addresses
  //    }
  //  }

  //  private static void Temp_Email_Addresses_State_Contacts()
  //  {
  //    Temp_Email_Addresses_State_Contacts(Session["UserStateCode"].ToString(),
  //      string.Empty);
  //  }

  //  private static void Temp_Email_Addresses_State_Contacts(string stateCode,
  //    string electionKey)
  //  {
  //    #region Build a temporary tables of email addresses

  //    //db.ExecuteSQL("TRUNCATE TABLE TempEmailAddresses");
  //    TempEmailAddresses.TruncateTable();

  //    if (!string.IsNullOrEmpty(db.States_Str(stateCode, "ContactEmail")))
  //      Insert_TempEmailAddresses(db.States_Str(stateCode, "ContactEmail"),
  //        db.States_Str(stateCode, "Contact"), string.Empty, string.Empty,
  //        electionKey, stateCode, string.Empty, string.Empty, string.Empty,
  //        string.Empty);

  //    if (!string.IsNullOrEmpty(db.States_Str(stateCode, "AltEMail")))
  //      Insert_TempEmailAddresses(db.States_Str(stateCode, "AltEMail"),
  //        db.States_Str(stateCode, "AltContact"), string.Empty, string.Empty,
  //        electionKey, stateCode, string.Empty, string.Empty, string.Empty,
  //        string.Empty);

  //    #endregion Build a temporary table of email addresses

  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }

  //  private static void Temp_Email_Addresses_State_Parties()
  //  {
  //    TempEmailAddresses.TruncateTable();

  //    var sql = string.Empty;
  //    sql += " SELECT";
  //    sql += " Parties.StateCode";
  //    sql += " ,Parties.PartyCode";
  //    sql += " ,PartiesEmails.PartyKey";
  //    sql += " ,PartiesEmails.PartyEmail";
  //    sql += " ,PartiesEmails.PartyPassword";
  //    sql += " ,PartiesEmails.PartyContactFName";
  //    sql += " ,PartiesEmails.PartyContactLName";
  //    sql += " ,PartiesEmails.PartyContactTitle";
  //    sql += " FROM PartiesEmails,Parties";
  //    sql += " WHERE Parties.StateCode = " +
  //      db.SQLLit(Session["UserStateCode"].ToString());
  //    sql += " AND PartiesEmails.PartyKey = Parties.PartyKey ";

  //    if (Elections.IsPrimaryElection(ViewState["ElectionKey"].ToString())) //string Party_Code_Election =
  //      //  db.PartyCode4ElectionKey(ViewState["ElectionKey"].ToString());
  //      sql += " AND " +
  //        db.SQLLit(db.PartyCode4ElectionKey(ViewState["ElectionKey"].ToString())) +
  //        " = " + "Parties.PartyKey";

  //    var tableStatePartyEmails = db.Table(sql);
  //    foreach (DataRow rowPartyEmail in tableStatePartyEmails.Rows)
  //      Insert_TempEmailAddresses(rowPartyEmail["PartyEmail"].ToString(),
  //        rowPartyEmail["PartyContactFName"] + " " +
  //          rowPartyEmail["PartyContactLName"], string.Empty, string.Empty
  //        //, db.ElectionKey_Viewable_Latest_Any_Upcoming(
  //        //    Session["UserStateCode"].ToString())
  //        , ViewState["ElectionKey"].ToString(),
  //        rowPartyEmail["StateCode"].ToString(),
  //        rowPartyEmail["PartyKey"].ToString(),
  //        rowPartyEmail["PartyContactFName"].ToString(),
  //        rowPartyEmail["PartyContactLName"].ToString(),
  //        rowPartyEmail["PartyContactTitle"].ToString());
  //    //Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
  //    //  .ToString(CultureInfo.InvariantCulture);
  //  }
  //}
}