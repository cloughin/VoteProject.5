using System;
using System.Data;
using System.Globalization;
using DB.Vote;

namespace Vote.Admin
{
  public partial class ElectionCreate : VotePage
  {
    //private const int IndexRadioButtonAllParties = 0;
    //private const int IndexRadioButtonDemocratic = 1;
    //private const int IndexRadioButtonRepublican = 2;
    //private const int IndexRadioButtonGreen = 3;
    //private const int IndexRadioButtonLibertarian = 4;
    //private const int IndexRadioButtonConstitution = 5;
    //private const int IndexRadioButtonNonpartisan = 6;

    //private const int IndexRadioButtonExcludeUSPresident = 0;

    //private const int IndexRadioButtonIncludeUSPresidentWithoutCandidates
    //  = 1;

    //private const int IndexRadioButtonIncludeUSPresidentWithCandidates = 2;

    //private void Html_Tables_Not_Visible_Election_Dependent()
    //{
    //  TableCreateGeneralStateElections.Visible = false;
    //  TableCreateElection.Visible = false;

    //  TableElectionType.Visible = false;
    //  RadioButtonListElectionType.Visible = false;
    //  RadioButtonListElectionTypeNational.Visible = false;

    //  TableElectionTypeP.Visible = false;

    //  TableElectionTypeB.Visible = false;
    //}

    //private void Html_Tables_Not_Visible_Constant()
    //{
    //  TableCreateButton.Visible = false;
    //  TableElectionDate.Visible = false;
    //  TableParty.Visible = false;
    //  TableElectionTitle.Visible = false;
    //  TableAdditionalInfo.Visible = false;
    //}

    //private void CheckTextBoxs4HtmlAndIlleagalInput()
    //{
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextboxElectionDate);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextboxElectionTitle);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextboxAdditionalInfo);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextboxBallotInstructions);
    //}

    //private void DisableAllParties()
    //{
    //  for (var party = IndexRadioButtonAllParties;
    //    party <= IndexRadioButtonNonpartisan;
    //    party++) RadioButtonListParty.Items[party].Enabled = false;
    //}

    //private void UnSelectAllParties()
    //{
    //  for (var party = IndexRadioButtonAllParties;
    //    party <= IndexRadioButtonNonpartisan;
    //    party++) RadioButtonListParty.Items[party].Selected = false;
    //}

    //private void EnableAllParties()
    //{
    //  for (var party = IndexRadioButtonAllParties;
    //    party <= IndexRadioButtonNonpartisan;
    //    party++) RadioButtonListParty.Items[party].Enabled = true;
    //}

    //private void ClearAllTextBoxes()
    //{
    //  TextboxElectionDate.Text = string.Empty;
    //  TextboxElectionTitle.Text = string.Empty;
    //  TextboxAdditionalInfo.Text = string.Empty;
    //}

    //private static string Sql_USPresident_Candidates(string electionKeyPp)
    //{
    //  var sql = string.Empty;
    //  sql += " SELECT PoliticianKey";
    //  sql += ",OfficeKey";
    //  sql += ",OrderOnBallot";
    //  sql += " FROM ElectionsPoliticians";
    //  sql += " WHERE ElectionKey = " + db.SQLLit(electionKeyPp);
    //  sql += " AND OfficeKey ='USPresident'";
    //  sql += " ORDER BY OrderOnBallot";
    //  return sql;
    //}

    //private static string ElectionKey_PP(string nationalPartyCode)
    //{
    //  var sql = string.Empty;
    //  sql += " SELECT ElectionKey";
    //  sql += " FROM Elections";
    //  sql += " WHERE ElectionType ='A'";
    //  sql += " AND NationalPartyCode = " + db.SQLLit(nationalPartyCode);
    //  sql += " AND StateCode ='PP'";
    //  sql += " AND ElectionDate > " +
    //    db.SQLLit(DateTime.Today.ToString("yyyy-MM-dd"));
    //  var rowElectionPpTemplate = db.Row_First_Optional(sql);
    //  return rowElectionPpTemplate != null 
    //    ? rowElectionPpTemplate["ElectionKey"].ToString() 
    //    : string.Empty;
    //}

    //private static string Election_Key(string stateCode, string electionDate,
    //  string electionType, string partyCode)
    //{
    //  var electionKey = db.ElectionKey_Build(stateCode, string.Empty, string.Empty, 
    //    electionDate, StateCache.IsValidStateCode(stateCode) 
    //      ? electionType 
    //      : "A", partyCode);

    //  return electionKey;
    //}

    //private string ElectionKey_New()
    //{
    //  var electionKey = db.ElectionKey_Build(ViewState["StateCode"].ToString(),
    //    string.Empty, string.Empty, TextboxElectionDate.Text.Trim(),
    //    RadioButtonListElectionType.SelectedValue,
    //    RadioButtonListParty.SelectedValue);
    //  return electionKey;
    //}

    //private string Election_Key_First_Primary_Same_Day_P()
    //{
    //  var electionKeyThis = Election_Key(ViewState["StateCode"].ToString(),
    //    TextboxElectionDate.Text.Trim(), "P", RadioButtonListParty.SelectedValue);

    //  var sqlText = " SELECT ";
    //  sqlText += " ElectionKey";
    //  sqlText += " FROM Elections";
    //  sqlText += " WHERE ElectionType = 'P'";
    //  sqlText += " AND StateCode = " + db.SQLLit(ViewState["StateCode"].ToString());
    //  sqlText += " AND ElectionDate = " +
    //    db.SQLLit(Convert.ToDateTime(TextboxElectionDate.Text.Trim())
    //      .ToString("yyyy-MM-dd"));
    //  sqlText += " AND ElectionKey !=" + db.SQLLit(electionKeyThis);
    //  sqlText += " AND (NationalPartyCode != 'A' AND NationalPartyCode != 'X')";

    //  var electionKeyFirstPrimarySameDayP = string.Empty;

    //  //Find a primary with office contests
    //  var tableElections = db.Table(sqlText);
    //  foreach (DataRow rowElection in tableElections.Rows)
    //    if (string.IsNullOrEmpty(electionKeyFirstPrimarySameDayP))
    //    {
    //      var sql = "ElectionsOffices WHERE ElectionKey = " +
    //        db.SQLLit(rowElection["ElectionKey"].ToString());
    //      var rowsElectionsOffices = db.Rows_Count_From(sql);
    //      if (rowsElectionsOffices > 0)
    //        electionKeyFirstPrimarySameDayP =
    //          rowElection["ElectionKey"].ToString();
    //    }
    //  return electionKeyFirstPrimarySameDayP;
    //}

    //private static string Party_Code(string stateCode, string party)
    //{
    //  if (!string.IsNullOrEmpty(party)) return stateCode + party;
    //  return "ALL";
    //}

    //private void Set_Election_Description()
    //{
    //  if (ViewState["StateCode"].ToString() != "UU") //General Election for each State
    //    if (StateCache.IsValidStateCode(ViewState["StateCode"].ToString()))
    //      TextboxElectionTitle.Text =
    //        db.Str_Election_Description(RadioButtonListElectionType.SelectedValue,
    //          RadioButtonListParty.SelectedValue, TextboxElectionDate.Text.Trim(),
    //          ViewState["StateCode"].ToString());
    //    else
    //      //Canonical Presidential Primary Template
    //      TextboxElectionTitle.Text = db.Str_Election_Description("A",
    //        RadioButtonListParty.SelectedValue, TextboxElectionDate.Text.Trim(),
    //        RadioButtonListElectionTypeNational.SelectedValue);
    //}

    //private void Set_Election_Key()
    //{
    //  if (ViewState["StateCode"].ToString() == "UU") return;
    //  if (StateCache.IsValidStateCode(ViewState["StateCode"].ToString()))
    //    Label_ElectionKey.Text = Election_Key(ViewState["StateCode"].ToString(),
    //      TextboxElectionDate.Text.Trim(),
    //      RadioButtonListElectionType.SelectedValue,
    //      RadioButtonListParty.SelectedValue);
    //  else
    //    Label_ElectionKey.Text =
    //      Election_Key(RadioButtonListElectionTypeNational.SelectedValue,
    //        TextboxElectionDate.Text.Trim(), "A",
    //        RadioButtonListParty.SelectedValue);
    //}

    //private void Set_Election_Description_And_Key()
    //{
    //  Set_Election_Description();
    //  Set_Election_Key();
    //}

    //private void SetPartyRadioButton()
    //{
    //  UnSelectAllParties();
    //  switch (QueryParty)
    //  {
    //    case "A":
    //      RadioButtonListParty.Items[IndexRadioButtonAllParties].Selected =
    //        true;
    //      break;
    //    case "D":
    //      RadioButtonListParty.Items[IndexRadioButtonDemocratic].Selected = true;
    //      break;
    //    case "R":
    //      RadioButtonListParty.Items[IndexRadioButtonRepublican].Selected = true;
    //      break;
    //    case "G":
    //      RadioButtonListParty.Items[IndexRadioButtonGreen].Selected = true;
    //      break;
    //    case "L":
    //      RadioButtonListParty.Items[IndexRadioButtonLibertarian].Selected =
    //        true;
    //      break;
    //    case "X":
    //      RadioButtonListParty.Items[IndexRadioButtonNonpartisan].Selected =
    //        true;
    //      break;
    //  }
    //}

    //private static string USPresident_Candidates(PageCache cache, string electionKeyPp)
    //{
    //  var usPresidentCandidates = string.Empty;

    //  var tablePoliticians = db.Table(Sql_USPresident_Candidates(electionKeyPp));
    //  foreach (DataRow rowPolitician in tablePoliticians.Rows)
    //    usPresidentCandidates += "<br>" +
    //      cache.Politicians.GetPoliticianName(
    //        rowPolitician["PoliticianKey"].ToString());
    //  return usPresidentCandidates;
    //}

    //private void Election_P_Controls(PageCache cache, string nationalPartyCode)
    //{
    //  #region Create Same Office Contests for a Different Party in same Primary Section

    //  var electionKeyFirstPrimarySameDay =
    //    Election_Key_First_Primary_Same_Day_P();
    //  if (!string.IsNullOrEmpty(electionKeyFirstPrimarySameDay))
    //  {
    //    #region Create Same Office Contests in this Party Primary

    //    Label_ElectionKey_P.Text = electionKeyFirstPrimarySameDay;
    //    Label_SameDatePrimary.Text =
    //      db.Elections_ElectionDesc(Election_Key_First_Primary_Same_Day_P());
    //    Label_OfficeContests.Text =
    //      db.Rows_Count_From("ElectionsOffices WHERE ElectionKey =" +
    //        db.SQLLit(electionKeyFirstPrimarySameDay))
    //        .ToString(CultureInfo.InvariantCulture);

    //    CheckBox_CreateOfficeContests.Checked = true;

    //    #endregion Create Same Office Contests in this Party Primary

    //    #region US Presidential Primary Contest and Candidates:

    //    RadioButtonListUSPresident.Enabled = true;

    //    #endregion US Presidential Primary Contest and Candidates:
    //  }
    //  else
    //  {
    //    Label_ElectionKey_P.Text = string.Empty;
    //    Label_SameDatePrimary.Text = "There are no primaries on the same day" +
    //      " with Office contests that can be copied.";
    //    Label_OfficeContests.Text = "0";
    //    CheckBox_CreateOfficeContests.Checked = false;
    //    CheckBox_CreateOfficeContests.Enabled = false;
    //  }

    //  #endregion Create Same Office Contests for a Different Party in same Primary Section

    //  #region Include or Exclude US President Contest and Candidates:

    //  if (RadioButtonListParty.SelectedValue == "A") //All Parties
    //  {
    //    RadioButtonListUSPresident.Items[IndexRadioButtonExcludeUSPresident]
    //      .Enabled = true;
    //    RadioButtonListUSPresident.Items[
    //      IndexRadioButtonIncludeUSPresidentWithoutCandidates].Enabled =
    //      true;
    //    RadioButtonListUSPresident.Items[
    //      IndexRadioButtonIncludeUSPresidentWithCandidates].Enabled = false;
    //  }
    //  else
    //  {
    //    RadioButtonListUSPresident.Items[IndexRadioButtonExcludeUSPresident]
    //      .Enabled = true;
    //    RadioButtonListUSPresident.Items[
    //      IndexRadioButtonIncludeUSPresidentWithoutCandidates].Enabled =
    //      true;
    //    RadioButtonListUSPresident.Items[
    //      IndexRadioButtonIncludeUSPresidentWithCandidates].Enabled = true;
    //  }

    //  var electionKeyPp = ElectionKey_PP(nationalPartyCode);
    //  if (!string.IsNullOrEmpty(electionKeyPp))
    //    Label_Candidates_President.Text = USPresident_Candidates(cache,
    //      electionKeyPp);
    //  else
    //  {
    //    Label_Candidates_President.Text =
    //      "No Candidates for a Presidential Primary Template are available.";
    //    RadioButtonListUSPresident.Items[
    //      IndexRadioButtonIncludeUSPresidentWithCandidates].Enabled = false;
    //  }

    //  #endregion Include or Exclude US President Contest and Candidates:
    //}

    //private void Election_B_Controls_Candidates_Available_To_Add(PageCache cache)
    //{
    //  var electionKeyPp = ElectionKey_PP(RadioButtonListParty.SelectedValue);

    //  if (!string.IsNullOrEmpty(electionKeyPp))
    //  {
    //    #region State Presidential Primary (Election Type B)

    //    Label_ElectionKeyCanonical.Text = electionKeyPp;
    //    Label_Election_Canonical.Text = db.Elections_ElectionDesc(electionKeyPp);
    //    Label_Candidates_Canonical.Text = USPresident_Candidates(cache,
    //      electionKeyPp);

    //    #endregion State Presidential Primary (Election Type B)

    //    #region Add Presidential Candidates & Election Information:

    //    RadioButtonList_PresidentialCandidates.Visible = true;
    //    RadioButtonList_PresidentialCandidates.Enabled = true;

    //    #endregion Add Presidential Candidates & Election Information:
    //  }
    //  else
    //  {
    //    Label_ElectionKeyCanonical.Text = string.Empty;
    //    Label_Election_Canonical.Text =
    //      "No Canonical Presidential Primary Template exists.";
    //    Label_Candidates_Canonical.Text = string.Empty;

    //    RadioButtonList_PresidentialCandidates.SelectedValue = "None";
    //    RadioButtonList_PresidentialCandidates.Enabled = false;
    //  }
    //}

    //protected void TextboxElectionDate_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxs4HtmlAndIlleagalInput();

    //    if (TextboxElectionDate.Text.Trim() == string.Empty) //empty 
    //      throw new ApplicationException("The Future Date textbox is empty!");

    //    if (!db.Is_Valid_Date(TextboxElectionDate.Text.Trim())) //bad date
    //      throw new ApplicationException("You entered a bad date!");

    //    if (Convert.ToDateTime(TextboxElectionDate.Text.Trim()) < DateTime.Today) //in the future
    //      throw new ApplicationException(
    //        "The Future Date for a new election needs to be in the future!");

    //    TextboxElectionDate.Text = Convert.ToDateTime(
    //      TextboxElectionDate.Text.Trim())
    //      .ToString("MMMM d, yyyy");

    //    Set_Election_Description_And_Key();

    //    Msg.Text = db.Ok("The date is ok and the Election Title was modified.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //private void Enable_Party_RadioButtons_State(string electionType)
    //{
    //  DisableAllParties();

    //  switch (electionType)
    //  {
    //    case db.Election_Type_StateOffYear_O:
    //      RadioButtonListParty.Items[IndexRadioButtonAllParties].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonNonpartisan].Enabled = true;
    //      if ((RadioButtonListParty.Items[IndexRadioButtonDemocratic].Selected) ||
    //        (RadioButtonListParty.Items[IndexRadioButtonRepublican].Selected) ||
    //        (RadioButtonListParty.Items[IndexRadioButtonGreen].Selected) ||
    //        (RadioButtonListParty.Items[IndexRadioButtonLibertarian].Selected))
    //        throw new ApplicationException(
    //          "The Political Party is invalid for the type of election.");
    //      break;
    //    case db.Election_Type_StateSpecial_S:
    //      RadioButtonListParty.Items[IndexRadioButtonAllParties].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonNonpartisan].Enabled = true;
    //      if ((RadioButtonListParty.Items[IndexRadioButtonDemocratic].Selected) ||
    //        (RadioButtonListParty.Items[IndexRadioButtonRepublican].Selected) ||
    //        (RadioButtonListParty.Items[IndexRadioButtonGreen].Selected) ||
    //        (RadioButtonListParty.Items[IndexRadioButtonLibertarian].Selected))
    //        throw new ApplicationException(
    //          "The Political Party is invalid for the type of election.");
    //      break;
    //    case db.Election_Type_StatePartyPrimary_P:
    //      RadioButtonListParty.Items[IndexRadioButtonAllParties].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonDemocratic].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonRepublican].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonGreen].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonLibertarian].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonConstitution].Enabled =
    //        true;
    //      RadioButtonListParty.Items[IndexRadioButtonNonpartisan].Enabled = true;
    //      break;
    //    case db.Election_Type_StatePresidentialPartyPrimary_B:
    //      RadioButtonListParty.Items[IndexRadioButtonDemocratic].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonRepublican].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonGreen].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonLibertarian].Enabled = true;
    //      if ((RadioButtonListParty.Items[IndexRadioButtonAllParties].Selected) ||
    //        (RadioButtonListParty.Items[IndexRadioButtonNonpartisan].Selected))
    //        throw new ApplicationException(
    //          "The Political Party is invalid for the type of election.");
    //      break;
    //  }
    //}

    //private void Enable_Party_RadioButtons_Presidential_Primary(string stateCode)
    //{
    //  DisableAllParties();

    //  switch (stateCode)
    //  {
    //    case "US":
    //      RadioButtonListParty.Items[IndexRadioButtonDemocratic].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonRepublican].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonGreen].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonLibertarian].Enabled = true;
    //      if ((RadioButtonListParty.Items[IndexRadioButtonAllParties].Selected) ||
    //        (RadioButtonListParty.Items[IndexRadioButtonNonpartisan].Selected))
    //        throw new ApplicationException(
    //          "The Political Party is invalid for the type of election.");
    //      break;
    //    case "PP":
    //      RadioButtonListParty.Items[IndexRadioButtonDemocratic].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonRepublican].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonGreen].Enabled = true;
    //      RadioButtonListParty.Items[IndexRadioButtonLibertarian].Enabled = true;
    //      if ((RadioButtonListParty.Items[IndexRadioButtonAllParties].Selected) ||
    //        (RadioButtonListParty.Items[IndexRadioButtonNonpartisan].Selected))
    //        throw new ApplicationException(
    //          "The Political Party is invalid for the type of election.");
    //      break;
    //  }
    //}

    //private void Html_Tables_For_Primaries_B_P_Visible(string electionType)
    //{
    //  TableElectionTypeP.Visible = false;
    //  TableElectionTypeB.Visible = false;

    //  if (electionType == "P") //State Primary
    //    TableElectionTypeP.Visible = true;
    //  if (electionType == "B") //State Presidential Primary
    //    TableElectionTypeB.Visible = true;
    //}

    //protected void CheckBox_CreateOfficeContests_CheckedChanged(object sender,
    //  EventArgs e)
    //{
    //  try
    //  {
    //    RadioButtonListUSPresident.Enabled = !CheckBox_CreateOfficeContests.Checked;
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //private void Set_State_Controls()
    //{
    //  switch (RadioButtonListElectionType.SelectedValue)
    //  {
    //    case "P":
    //      Election_P_Controls(PageCache, RadioButtonListParty.SelectedValue);
    //      break;
    //    case "B":
    //      Election_B_Controls_Candidates_Available_To_Add(PageCache);
    //      break;
    //  }

    //  Enable_Party_RadioButtons_State(RadioButtonListElectionType.SelectedValue);

    //  Set_Election_Description_And_Key();
    //}

    //protected void RadioButtonListElectionType_SelectedIndexChanged1(object sender,
    //  EventArgs e)
    //{
    //  try
    //  {
    //    Html_Tables_For_Primaries_B_P_Visible(
    //      RadioButtonListElectionType.SelectedValue);

    //    Set_State_Controls();

    //    Msg.Text =
    //      db.Ok(
    //        "The election type selection is ok and the Election Title was modified.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListElectionTypeNational_SelectedIndexChanged1(
    //  object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Enable_Party_RadioButtons_Presidential_Primary(
    //      ViewState["StateCode"].ToString());

    //    Set_Election_Description_And_Key();

    //    Msg.Text =
    //      db.Ok(
    //        "The election type selection is ok and the Election Title was modified.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListParty_SelectedIndexChanged(object sender,
    //  EventArgs e)
    //{
    //  try
    //  {
    //    Set_State_Controls();

    //    Msg.Text =
    //      db.Ok(
    //        "Your party selection has been noted and the Election Title was modified to accomodate the chage." +
    //          " You may proceed to create the election");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonList_PresidentialCandidates_SelectedIndexChanged(
    //  object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Msg.Text =
    //      db.Ok(
    //        "Your selection of whether to add presidential candidates and election information has been noted." +
    //          " You may proceed to create the election");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void RadioButtonListUSPresident_SelectedIndexChanged(object sender,
    //  EventArgs e)
    //{
    //  try
    //  {
    //    Msg.Text =
    //      db.Ok(
    //        "Your selection of whether to include the US President office has been noted." +
    //          " You may proceed to create the election");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonNewElection_Click1(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string electionKey;
    //    var paryCode = string.Empty;
    //    var electionType = string.Empty;
    //    var nationalPartyCode = string.Empty;

    //    #region Checks for All Election Types

    //    CheckTextBoxs4HtmlAndIlleagalInput();

    //    if (TextboxElectionDate.Text.Trim() == string.Empty) //empty 
    //      throw new ApplicationException("The Future Date textbox is empty!");

    //    if (!db.Is_Valid_Date(TextboxElectionDate.Text.Trim())) //bad date
    //      throw new ApplicationException("You entered a bad date!");

    //    #endregion Checks for All Election Types

    //    // 3 Types of possible Election Creation
    //    if (StateCache.IsValidStateCode(ViewState["StateCode"].ToString()))
    //    {
    //      #region checks for both STATE and NATIONAL election types

    //      if (TextboxElectionTitle.Text.Trim() == string.Empty) //blanked text box 
    //        throw new ApplicationException("The Election Title textbox is empty!");

    //      if (TextboxElectionTitle.Text.Length > 90)
    //      {
    //        var shortenBy = TextboxElectionTitle.Text.Length - 90;
    //        throw new ApplicationException(
    //          "The Election Title can not exceed 90 characters. You need to shorten by: " +
    //            shortenBy + " characters.");
    //      }

    //      #endregion checks for both STATE and NATIONAL election types

    //      if (RadioButtonListElectionType.Visible)
    //      {
    //        #region checks for ALL STATE election types

    //        if (RadioButtonListElectionType.SelectedIndex == -1)
    //          throw new ApplicationException(
    //            "You need to select a STATE election type!");

    //        #endregion checks for ALL STATE election types

    //        #region Checks for State Primary or Presidential Primary (Type P & B)

    //        if ((RadioButtonListElectionType.SelectedValue == "B") ||
    //          (RadioButtonListElectionType.SelectedValue == "P"))
    //          if (RadioButtonListParty.SelectedIndex == -1)
    //            throw new ApplicationException(
    //              "For a Primary Election you need to select a party.");

    //        #endregion Checks for State Primary or Presidential Primary (Type P & B)

    //        #region Checks for State Primary (Type P)

    //        if (RadioButtonListElectionType.SelectedValue == "P")
    //          if ((RadioButtonListUSPresident.SelectedIndex == -1) &&
    //            (!CheckBox_CreateOfficeContests.Checked))
    //            throw new ApplicationException(
    //              "You need to select a radio button" +
    //                " in the 'Include or Exclude US Presidential Primary Contest and Candidates' selection .");

    //        #endregion Checks for State Primary (Type P)

    //        #region Checks for State Primary (Type B)

    //        if (RadioButtonListElectionType.SelectedValue == "B")
    //          if (RadioButtonList_PresidentialCandidates.SelectedIndex == -1)
    //            throw new ApplicationException(
    //              "A selection of whether to add presidential candidates & election information needs to be made.");

    //        #endregion Checks for State Primary (Type B)
    //      }
    //      else if (RadioButtonListElectionTypeNational.Visible)
    //      {
    //        #region checks for ALL NATIONAL election types

    //        if (RadioButtonListElectionTypeNational.SelectedIndex == -1)
    //          throw new ApplicationException(
    //            "You need to select a NATIONAL election type!");

    //        if (RadioButtonListParty.SelectedIndex == -1)
    //          throw new ApplicationException(
    //            "For a Primary Election you need to select a party.");

    //        #endregion checks for ALL NATIONAL election types
    //      }

    //      #region CREATE State Non-General Election

    //      electionKey = Election_Key(ViewState["StateCode"].ToString(),
    //        TextboxElectionDate.Text.Trim(),
    //        RadioButtonListElectionType.SelectedValue,
    //        RadioButtonListParty.SelectedValue);

    //      #region New Election checks

    //      if (db.Is_Valid_Election(electionKey))
    //        throw new ApplicationException(
    //          "This election already exists with the ElectionKey: " + electionKey);

    //      #region Checks for Election Type P or B

    //      if ((RadioButtonListElectionType.SelectedValue == "P") ||
    //        (RadioButtonListElectionType.SelectedValue == "B"))
    //      {
    //        var electionKeySameDayPrimaries = electionKey.Substring(0, 11);
    //        var sqlElections = "Elections WHERE SUBSTRING(ElectionKey,1,11) = " +
    //          db.SQLLit(electionKeySameDayPrimaries);
    //        var sameDayPrimaryElections = db.Rows_Count_From(sqlElections);
    //        if ((RadioButtonListParty.SelectedValue == "A") &&
    //          (sameDayPrimaryElections > 0))
    //          throw new ApplicationException(
    //            "A primary election can not be created" +
    //              " if the 'All Parties' radio button is selected" +
    //              " and any primary already exists on the same day.");

    //        var electionKeyAllPartiesPrimary = electionKey.Substring(0, 11) +
    //          "A";
    //        if (db.Is_Valid_Election(electionKeyAllPartiesPrimary))
    //          throw new ApplicationException(
    //            "A primary election can not be created" +
    //              " if a primary for all parties exists on the same day." +
    //              "<br>You probably need to change the Political Party radio button.");
    //      }

    //      #endregion Checks for Election Type P or B

    //      #endregion New Election checks

    //      #region Party Code

    //      if ((RadioButtonListElectionType.SelectedValue ==
    //        db.Election_Type_StatePartyPrimary_P) ||
    //        (RadioButtonListElectionType.SelectedValue ==
    //          db.Election_Type_StatePresidentialPartyPrimary_B))
    //        paryCode = Party_Code(ViewState["StateCode"].ToString(),
    //          RadioButtonListParty.SelectedValue);
    //      else paryCode = "ALL";

    //      #endregion Party Code

    //      electionType = RadioButtonListElectionType.SelectedValue;

    //      nationalPartyCode = RadioButtonListParty.SelectedValue;

    //      db.Elections_Insert_And_Report_Election_Update(
    //        ViewState["StateCode"].ToString(), string.Empty, string.Empty,
    //        TextboxElectionDate.Text.Trim(), electionType, nationalPartyCode,
    //        paryCode, TextboxElectionTitle.Text.Trim(),
    //        TextboxAdditionalInfo.Text.Trim(),
    //        TextboxBallotInstructions.Text.Trim());

    //      #region Insert Offices & Candidates - State Primary (Election Type P)

    //      switch (RadioButtonListElectionType.SelectedValue)
    //      {
    //        case db.Election_Type_StatePartyPrimary_P:
    //        {
    //          var electionKeyFirstPrimarySameDay =
    //            Election_Key_First_Primary_Same_Day_P();

    //          if ((CheckBox_CreateOfficeContests.Checked) &&
    //            (!string.IsNullOrEmpty(electionKeyFirstPrimarySameDay)))
    //          {
    //            #region Insert same office contests and election information as for a previous party in this primary

    //            #region Insert ONLY ElectionsOffices rows for Primary Elections being copied

    //            electionKey = ElectionKey_New();

    //            var sql = string.Empty;
    //            sql += " SELECT OfficeKey";
    //            sql += " FROM ElectionsOffices";
    //            sql += " WHERE electionKey = " +
    //              db.SQLLit(electionKeyFirstPrimarySameDay);
    //            sql += " ORDER BY OfficeLevel ASC,DistrictCode ASC";
    //            var tableElectionOffices = db.Table(sql);
    //            foreach (DataRow rowOffice in tableElectionOffices.Rows)
    //              if (
    //                !db.Is_Valid_ElectionsOffices(electionKey,
    //                  rowOffice["OfficeKey"].ToString()))
    //                db.ElectionsOffices_INSERT(electionKey,
    //                  rowOffice["OfficeKey"].ToString());

    //            #endregion Insert ONLY ElectionsOffices rows for Primary Elections being copied

    //            #region copy Additional Information and Ballot Instructions

    //            db.Elections_Update_Str(electionKey, "ElectionAdditionalInfo",
    //              db.Elections_Str(Election_Key_First_Primary_Same_Day_P(),
    //                "ElectionAdditionalInfo"));
    //            db.Elections_Update_Str(electionKey, "BallotInstructions",
    //              db.Elections_Str(Election_Key_First_Primary_Same_Day_P(),
    //                "BallotInstructions"));

    //            #endregion copy Additional Information and Ballot Instructions

    //            #endregion Insert same office contests and election information as for a previous party in this primary
    //          }

    //          #region Insert only US President Office w/wo candidates from Presidential Primary template

    //          if ((RadioButtonListUSPresident.SelectedValue ==
    //            "IncludePresidentNoCandidates") ||
    //            (RadioButtonListUSPresident.SelectedValue ==
    //              "IncludePresidentTemplateCandidates")) db.ElectionsOffices_INSERT(electionKey, "USPresident");

    //          if ((RadioButtonListUSPresident.SelectedValue ==
    //            "IncludePresidentTemplateCandidates") &&
    //            (!string.IsNullOrEmpty(ElectionKey_PP(nationalPartyCode)))
    //            //Presidential Primary candidates
    //            )
    //          {
    //            var tablePoliticians =
    //              db.Table(
    //                Sql_USPresident_Candidates(ElectionKey_PP(nationalPartyCode)));
    //            foreach (DataRow rowPolitician in tablePoliticians.Rows)
    //              if (
    //                !db.Is_Valid_ElectionsPoliticians(electionKey,
    //                  rowPolitician["OfficeKey"].ToString(),
    //                  rowPolitician["PoliticianKey"].ToString()))
    //                db.ElectionsPoliticians_INSERT(electionKey,
    //                  rowPolitician["OfficeKey"].ToString(),
    //                  rowPolitician["PoliticianKey"].ToString(),
    //                  Convert.ToInt16(rowPolitician["OrderOnBallot"].ToString()));
    //          }

    //          #endregion Insert only US President Office w/wo candidates from Presidential Primary template
    //        }
    //          break;
    //        case db.Election_Type_StatePresidentialPartyPrimary_B:
    //          db.ElectionsOffices_INSERT(electionKey, "USPresident");
    //          if ((RadioButtonList_PresidentialCandidates.SelectedValue ==
    //            "Candidates") ||
    //            (RadioButtonList_PresidentialCandidates.SelectedValue ==
    //              "Canonical"))
    //          {
    //            #region Insert ElectionsPoliticians rows

    //            electionKey = ElectionKey_New();
    //            var sql = string.Empty;
    //            sql += " SELECT OfficeKey";
    //            sql += ",PoliticianKey";
    //            sql += ",OrderOnBallot";
    //            sql += " FROM ElectionsPoliticians";
    //            sql += " WHERE electionKey = " +
    //              db.SQLLit(Label_ElectionKeyCanonical.Text.Trim());
    //            var tableElectionsPoliticians = db.Table(sql);
    //            foreach (DataRow rowPolitician in tableElectionsPoliticians.Rows)
    //              if (
    //                !db.Is_Valid_ElectionsPoliticians(electionKey,
    //                  rowPolitician["OfficeKey"].ToString(),
    //                  rowPolitician["PoliticianKey"].ToString()))
    //                db.ElectionsPoliticians_INSERT(electionKey,
    //                  rowPolitician["OfficeKey"].ToString(),
    //                  rowPolitician["PoliticianKey"].ToString(),
    //                  Convert.ToInt16(rowPolitician["OrderOnBallot"].ToString()));

    //            #endregion Insert ElectionsPoliticians rows
    //          }
    //          if (RadioButtonList_PresidentialCandidates.SelectedValue ==
    //            "Canonical")

    //            db.Elections_Update_Str(electionKey, "ElectionKeyCanonical",
    //              Label_ElectionKeyCanonical.Text.Trim());
    //          break;
    //      }

    //      #endregion Insert Offices & Candidates - State Presidential Primary (Election Type B)

    //      #endregion CREATE State Non-General Election
    //    }
    //    else if (
    //      db.Is_StateCode_National_Party_Contest(
    //        ViewState["StateCode"].ToString()))
    //    {
    //      #region CREATE National Presidential Party Primary Contest

    //      if ((RadioButtonListParty.SelectedValue != "D") &&
    //        (RadioButtonListParty.SelectedValue != "R") &&
    //        (RadioButtonListParty.SelectedValue != "G") &&
    //        (RadioButtonListParty.SelectedValue != "L"))
    //        throw new ApplicationException(
    //          "You need to select a political party.");

    //      electionKey = Election_Key(ViewState["StateCode"].ToString(),
    //        TextboxElectionDate.Text.Trim(), "A",
    //        RadioButtonListParty.SelectedValue);
    //      if (db.Is_Valid_Election(electionKey))
    //        throw new ApplicationException(
    //          "This election already exists with the ElectionKey: " + electionKey);

    //      paryCode = Party_Code(ViewState["StateCode"].ToString(),
    //        RadioButtonListParty.SelectedValue);

    //      electionType = "A";

    //      nationalPartyCode = RadioButtonListParty.SelectedValue;

    //      db.Elections_Insert_And_Report_Election_Update(
    //        ViewState["StateCode"].ToString(), string.Empty, string.Empty,
    //        TextboxElectionDate.Text.Trim(), electionType, nationalPartyCode,
    //        paryCode, TextboxElectionTitle.Text.Trim(),
    //        TextboxAdditionalInfo.Text.Trim(),
    //        TextboxBallotInstructions.Text.Trim());

    //      db.ElectionsOffices_INSERT(electionKey, "USPresident");

    //      #endregion CREATE National Presidential Party Primary Contest
    //    }

    //    else if (db.Is_StateCode_All_51_States(ViewState["StateCode"].ToString()))
    //    {
    //      #region Checks for General Election for all 51 states

    //      if (db.Is_StateCode_All_51_States(ViewState["StateCode"].ToString()))
    //        if (RadioButtonListIncludePresident.SelectedIndex == -1)
    //          throw new ApplicationException(
    //            "You need to select whether US President is included in the election.");

    //      #endregion Checks for General Election for all 51 states

    //      #region CREATE General Election for all 51 States and U1...U4 Elections

    //      var tableStates = db.Table(sql.States_51());
    //      string electionTitle;
    //      foreach (DataRow rowState in tableStates.Rows)
    //      {
    //        electionKey = Election_Key(rowState["StateCode"].ToString(),
    //          TextboxElectionDate.Text.Trim(), "G", "A");
    //        if (db.Is_Valid_Election(electionKey))
    //          throw new ApplicationException(
    //            "This election already exists with the ElectionKey: " +
    //              electionKey);

    //        paryCode = "ALL";

    //        electionType = "G";

    //        nationalPartyCode = "A";

    //        #region Insert rows in ElectionsOffices for US President

    //        //This is done first so that these offices will be included in the Election Report
    //        //which is part of Elections_Insert_And_Report_Election_Update
    //        //if (CheckBoxIncludePresident.Checked)
    //        if (RadioButtonListIncludePresident.SelectedValue == "Y") db.ElectionsOffices_INSERT(electionKey, "USPresident");

    //        #endregion Insert rows in ElectionsOffices for US President

    //        #region State's US Senate and US House Offices

    //        //Note US Senate is only for 1/3 of the States
    //        //But is created for all States anyway because it can be easily unchecked
    //        var sqlText = string.Empty;
    //        sqlText += " SELECT ";
    //        sqlText += " StateCode,OfficeKey,DistrictCode";
    //        sqlText += " FROM Offices";
    //        sqlText += " WHERE StateCode = " +
    //          db.SQLLit(rowState["StateCode"].ToString());
    //        sqlText += " AND CountyCode = ''";
    //        sqlText += " AND LocalCode = ''";
    //        sqlText += " AND OfficeLevel < 4";
    //        sqlText += " AND IsSpecialOffice = 0";
    //        sqlText +=
    //          " ORDER BY OfficeLevel,DistrictCode,OfficeOrderWithinLevel,DistrictCodeAlpha,OfficeLine1";

    //        var tableOffices = db.Table(sqlText);
    //        foreach (DataRow rowOffice in tableOffices.Rows)
    //          db.ElectionsOffices_INSERT(electionKey,
    //            rowOffice["OfficeKey"].ToString(),
    //            rowOffice["DistrictCode"].ToString());

    //        #endregion State's US Senate and US House Offices

    //        electionTitle = Convert.ToDateTime(TextboxElectionDate.Text.Trim())
    //          .ToString("MMMM d, yyyy") + " " +
    //          StateCache.GetStateName(rowState["StateCode"].ToString()) +
    //          " General Election";

    //        db.Elections_Insert_And_Report_Election_Update(
    //          rowState["StateCode"].ToString(), string.Empty, string.Empty,
    //          TextboxElectionDate.Text.Trim(), electionType, nationalPartyCode,
    //          paryCode, electionTitle, string.Empty, string.Empty);
    //      }

    //      var electionDate = Convert.ToDateTime(TextboxElectionDate.Text.Trim())
    //        .ToString("MMMM d, yyyy");

    //      if (RadioButtonListIncludePresident.SelectedValue == "Y")
    //      {
    //        #region Insert row in Elections for US President State-by-State

    //        electionTitle = electionDate +
    //          " General Election of U.S. President State-By-State";
    //        db.Elections_Insert_And_Report_Election_Update("U1", string.Empty,
    //          string.Empty, TextboxElectionDate.Text.Trim(), electionType,
    //          nationalPartyCode, paryCode, electionTitle, string.Empty,
    //          string.Empty);

    //        #endregion Insert row in Elections for US President State-by-State
    //      }

    //      #region Insert row in Elections for US Senate State-by-State

    //      electionTitle = electionDate +
    //        " General Election of U.S. Senate State-By-State";
    //      db.Elections_Insert_And_Report_Election_Update("U2", string.Empty,
    //        string.Empty, TextboxElectionDate.Text.Trim(), electionType,
    //        nationalPartyCode, paryCode, electionTitle, string.Empty,
    //        string.Empty);

    //      #endregion Insert row in Elections for US Senate State-by-State

    //      #region Insert row in Elections for US House State-by-State

    //      electionTitle = electionDate +
    //        " General Election of U.S. House of Representatives State-By-State";
    //      db.Elections_Insert_And_Report_Election_Update("U3", string.Empty,
    //        string.Empty, TextboxElectionDate.Text.Trim(), electionType,
    //        nationalPartyCode, paryCode, electionTitle, string.Empty,
    //        string.Empty);

    //      #endregion Insert row in Elections for US House State-by-State

    //      #region Insert row in Elections for Governors State-by-State

    //      electionTitle = electionDate +
    //        " General Election of Governors State-By-State";
    //      db.Elections_Insert_And_Report_Election_Update("U4", string.Empty,
    //        string.Empty, TextboxElectionDate.Text.Trim(), electionType,
    //        nationalPartyCode, paryCode, electionTitle, string.Empty,
    //        string.Empty);

    //      #endregion Insert row in Elections for Governors State-by-State

    //      #endregion CREATE General Election for all 51 States and U1...U4 Elections
    //    }

    //    if ((RadioButtonListElectionType.SelectedValue == "O") ||
    //      (RadioButtonListElectionType.SelectedValue == "S") ||
    //      (RadioButtonListParty.SelectedValue == "A"))
    //    {
    //      Html_Tables_Not_Visible_Election_Dependent();
    //      Html_Tables_Not_Visible_Constant();
    //    }

    //    #region Msg

    //    var msg = TextboxElectionTitle.Text.Trim() + " has been created.";

    //    if (RadioButtonListElectionType.Visible)

    //      #region STATE Elections and Primaries

    //      if ((RadioButtonListElectionType.SelectedValue == "O") ||
    //        (RadioButtonListElectionType.SelectedValue == "S"))
    //      {
    //        msg +=
    //          "<br>Close this form then return to the Elections.aspx Form and refresh the form.";
    //        msg += "<br>Then remove the election from the future elections list.";
    //      }
    //      else
    //      {
    //        if (RadioButtonListParty.SelectedValue == "A")
    //          msg +=
    //            "<br>Close this form then return to the Elections Form and refresh the form.";
    //        else
    //        {
    //          msg +=
    //            "<br>If this is the first party primary created, your next step should be to identify the office contests in the election.";
    //          msg +=
    //            "<br>Otherwise, you can now create an additional party primary.";
    //        }
    //        if ((RadioButtonListElectionType.SelectedValue == "B") ||
    //          (RadioButtonListElectionType.SelectedValue == "P"))
    //          msg +=
    //            "<br>If all primaries have been created, return to the Elecltions.aspx form and remove the primary from the future elections list.";
    //      }
    //      #endregion STATE Elections and Primaries

    //    else if (RadioButtonListElectionTypeNational.Visible)

    //      #region National Presidential Primaries

    //      msg += "<br>Your next step is to identify the presidential candidates";
    //      #endregion National Presidential Primaries

    //    Msg.Text = db.Ok(msg);

    //    #endregion Msg
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //private void Page_Title()
    //{
    //  if (StateCache.IsValidStateCode(ViewState["StateCode"].ToString()))
    //  {
    //    PageTitle.Text =
    //      Offices.GetElectoralClassDescription(ViewState["StateCode"].ToString(),
    //        string.Empty, string.Empty);
    //    PageTitle.Text += "<br>";
    //    PageTitle.Text += "Create a State Election or Primary";
    //    PageTitle.Text += "<br>";
    //    PageTitle.Text += " (Not a Even Year General Election)";
    //  }
    //  else switch (ViewState["StateCode"].ToString())
    //  {
    //    case "PP":
    //    case "US":
    //      PageTitle.Text =
    //        "Create a National Presidential Primary of Remaining Candidates";
    //      PageTitle.Text += "<br>";
    //      PageTitle.Text +=
    //        "of Remaining Candidates or Template for States with Same Candidates";
    //      break;
    //    case "UU":
    //      PageTitle.Text = "Create a General Election " +
    //        "<br>for Each State and 4 State-by-State Elections";
    //      break;
    //  }
    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    #region Security Check and Values for ViewState["StateCode"]

    //    //The Session UserStateCode, UserCountyCode, UserLocalCode can be changed
    //    //by a higher administration level using query strings
    //    //This is done in db.State_Code(), db.County_Code(), db.Local_Code()
    //    //
    //    //Using ViewState variables insures these values won't
    //    //change on any postbacks or in different tab or browser Sessions.
    //    //
    //    //ViewState["StateCode"] can be a StateCode or U1, u2, u3 for FederalCode

    //    var stateCode = db.State_Code();
    //    ViewState["StateCode"] = stateCode;

    //    // Special security for "UU"
    //    // just for this page, so handle it here
    //    var securityOk = stateCode == "UU" 
    //      ? SecurePage.IsSuperUser 
    //      : db.Is_User_Security_Ok();
    //    if (!securityOk) SecurePage.HandleSecurityException();

    //    #endregion Security Check and Values for ViewState["StateCode"]

    //    try
    //    {
    //      //The StateCode can be either:
    //      //UU to create a general election for each state and 4 state-by-state elections
    //      //A state StateCode like VA, TX which indicates a non-general state election
    //      //US for national Presidential Primary Candidates Remaining in Race
    //      //PP for a national Template of All Major Presidential Primary Candidates

    //      Html_Tables_Not_Visible_Election_Dependent();

    //      ClearAllTextBoxes();

    //      #region Set Query String Parms for Election Controls

    //      if (!string.IsNullOrEmpty(GetQueryString("Type")))
    //        if (StateCache.IsValidStateCode(ViewState["StateCode"].ToString()))
    //        {
    //          RadioButtonListElectionType.Enabled = true;
    //          RadioButtonListElectionType.SelectedValue = GetQueryString("Type");
    //        }
    //        else
    //        {
    //          #region Presidential Primaray

    //          RadioButtonListElectionTypeNational.Enabled = true;
    //          RadioButtonListElectionTypeNational.SelectedValue =
    //            ViewState["StateCode"].ToString();

    //          #endregion Presidential Primaray
    //        }

    //      if (!string.IsNullOrEmpty(QueryParty))
    //      {
    //        RadioButtonListParty.Enabled = true;
    //        EnableAllParties();
    //        RadioButtonListParty.SelectedValue = QueryParty;
    //      }

    //      if (!string.IsNullOrEmpty(GetQueryString("Date"))) //TextboxElectionDate.Text = db.QueryString("Date");
    //        TextboxElectionDate.Text = Convert.ToDateTime(GetQueryString("Date"))
    //          .ToString("MMMM d, yyyy");

    //      if (!string.IsNullOrEmpty(GetQueryString("Desc"))) TextboxElectionTitle.Text = GetQueryString("Desc");

    //      #endregion Set Query String Parms for Election Controls

    //      Page_Title();

    //      if (StateCache.IsValidStateCode(ViewState["StateCode"].ToString()))
    //      {
    //        #region State election

    //        #region Html Tables

    //        TableCreateElection.Visible = true;
    //        TableElectionType.Visible = true;
    //        RadioButtonListElectionType.Visible = true;

    //        Html_Tables_For_Primaries_B_P_Visible(GetQueryString("Type"));

    //        #endregion Html Tables

    //        Enable_Party_RadioButtons_State(GetQueryString("Type"));

    //        SetPartyRadioButton();

    //        switch (GetQueryString("Type"))
    //        {
    //          case "P":
    //            Election_P_Controls(PageCache, RadioButtonListParty.SelectedValue);
    //            break;
    //          case "B":
    //            Election_B_Controls_Candidates_Available_To_Add(PageCache);
    //            break;
    //        }

    //        #endregion state election
    //      }
    //      else switch (ViewState["StateCode"].ToString())
    //      {
    //        case "US":
    //          TableCreateElection.Visible = true;
    //          TableElectionType.Visible = true;
    //          RadioButtonListElectionTypeNational.Visible = true;
    //          Enable_Party_RadioButtons_Presidential_Primary("US");
    //          SetPartyRadioButton();
    //          break;
    //        case "PP":
    //          TableCreateElection.Visible = true;
    //          TableElectionType.Visible = true;
    //          RadioButtonListElectionTypeNational.Visible = true;
    //          Enable_Party_RadioButtons_Presidential_Primary("PP");
    //          SetPartyRadioButton();
    //          break;
    //        case "UU":
    //          TableCreateGeneralStateElections.Visible = true;
    //          break;
    //        default:
    //          throw new ApplicationException("The querystring StateCode is invalid.");
    //      }

    //      if (ViewState["StateCode"].ToString() != "UU") Set_Election_Key();

    //      var msg =
    //        "Set, check and/or change the options, parameters and title that define the election you are about to create.";
    //      if (RadioButtonListElectionType.SelectedValue == "P")
    //      {
    //        if (CheckBox_CreateOfficeContests.Checked)
    //          msg +=
    //            " <br>UNCHECK the 'Create Same Office Contests for a Different Party in same Primary' checkbox" +
    //              " if NO office contests are to be created.";

    //        if (RadioButtonListParty.SelectedValue == "A") //All Parties
    //          msg +=
    //            " <br>MOST STATE PRIMARIES ARE FOR A PARTICULAR PARTY. Change the 'Political Party' radio button if this is the case.";
    //      }

    //      msg += " <br>Click the Create Election(s) Button to create the election.";

    //      Msg.Text = db.Msg(msg);
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

    //protected void TextboxAdditionalInfo_TextChanged(object sender, EventArgs e)
    //{
    //  //Do nothing on Enter
    //}

    //protected void TextboxElectionTitle_TextChanged(object sender, EventArgs e)
    //{
    //  //Do nothing on Enter
    //}

    //protected void TextboxBallotInstructions_TextChanged(object sender, EventArgs e)
    //{
    //  //Do nothing on Enter
    //}
  }
}