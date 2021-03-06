using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Admin
{
  public partial class PoliticianPage : SecureAdminPage
  {
    //#region from db

    //public static string Url_Admin_Politician(
    //  string politicianKey
    //  )
    //{
    //  //return db.Url_Admin_Politician(PoliticianKey, string.Empty, string.Empty);
    //  var url = db.Url_Admin_Politician();

    //  if (!string.IsNullOrEmpty(politicianKey))
    //    url += "&Id=" + politicianKey;

    //  return db.Fix_Url_Parms(url);
    //}

    //public static string Str_Remove_Quotes(string str2Modify)
    //{
    //  var str = str2Modify;
    //  //characters to strip off
    //  str = str.Trim();
    //  str = str.Replace("\"", string.Empty);
    //  str = str.Replace("\'", string.Empty);
    //  return str;
    //}

    //public static void ElectionsPoliticians_Update_Bool(string electionKey, string officeKey,
    //  string politicianKey, string column, bool columnValue)
    //{
    //  Triple_Key_Update_Bool("ElectionsPoliticians", column, columnValue, "ElectionKey", 
    //    electionKey, "OfficeKey", officeKey, "PoliticianKey", politicianKey);
    //}

    //public static void Triple_Key_Update_Bool(string table, string column, bool columnValue, 
    //  string keyName1, string keyValue1, string keyName2, string keyValue2, string keyName3, 
    //  string keyValue3)
    //{
    //  var updateSQL = "UPDATE " + table
    //    + " SET " + column + " = " + Convert.ToUInt16(columnValue)
    //    + " WHERE " + keyName1 + " = " + db.SQLLit(keyValue1)
    //    + " AND " + keyName2 + " = " + db.SQLLit(keyValue2)
    //  + " AND " + keyName3 + " = " + db.SQLLit(keyValue3);
    //  db.ExecuteSql(updateSQL);
    //}

    //public static void OfficesOfficials_Delete(string officeKey, string politicianKey)
    //{
    //  if (db.Rows(sql.OfficesOfficials_Office_Politician(officeKey, politicianKey)) > 0)
    //  {
    //    var sqlText = string.Empty;
    //    sqlText += " DELETE FROM OfficesOfficials ";
    //    sqlText += " WHERE OfficesOfficials.OfficeKey = " + db.SQLLit(officeKey);
    //    sqlText += " AND OfficesOfficials.PoliticianKey = " + db.SQLLit(politicianKey);
    //    //db.ExecuteSQL(sql.OfficesOfficials_Delete(OfficeKey, PoliticianKey));
    //    db.ExecuteSql(sqlText);
    //    db.Log_OfficesOfficials_Add_Or_Delete(
    //      "D"
    //      , officeKey
    //      , politicianKey
    //      );
    //  }
    //}

    //public static void Set_IsWinner_For_Election(
    //  string electionKey,
    //  string officeKey,
    //  string politicianKey,
    //  bool isWinner
    //  )
    //{
    //  #region Note
    //  //ElectionsPoliticians.IsWinner for an office contest is set
    //  //normally set when winners are identified by OfficeWinner.aspx.
    //  //But old elections can also be set because it does not
    //  //affect the currently elected officials.
    //  //
    //  //OfficesOfficials rows are only inserted and deleted 
    //  //if the election is the most recent previous election.
    //  //in OfficeWinner.aspx form
    //  //to identify the currently elected officials (incumbents)
    //  #endregion Note

    //  ElectionsPoliticians_Update_Bool(
    //    electionKey,
    //    officeKey,
    //    politicianKey,
    //    "IsWinner",
    //    isWinner
    //    );
    //}

    //public static bool Is_Incumbent(string officeKey, string politicianKey)
    //{
    //  var sql = string.Empty;
    //  sql += " OfficesOfficials";
    //  sql += " WHERE OfficeKey =" + db.SQLLit(officeKey);
    //  sql += " AND PoliticianKey =" + db.SQLLit(politicianKey);

    //  var rows = db.Rows_Count_From(sql);
    //  return rows != 0;
    //}

    //public static string ElectionKey_Previous_Most_Recent(string stateCode)
    //{
    //  var sql = string.Empty;
    //  sql += "SELECT";
    //  sql += " ElectionKey";
    //  sql += " FROM vote.Elections";
    //  sql += " WHERE ElectionDate < " + db.SQLLit(Db.DbToday);
    //  sql += " AND StateCode = " + db.SQLLit(stateCode);
    //  sql += " ORDER BY ElectionDate DESC";
    //  var rowElection = db.Row_First(sql);
    //  return (rowElection["ElectionKey"].ToString());
    //}


    //public static string Url_Master_ImagesHeadshots(string politicianKey)
    //{
    //  return "/Master/ImagesHeadshots.aspx?Id=" + politicianKey;
    //}

    //public static DropDownList DropDownList_Party_Load(
    //DropDownList dropDownListParty
    //, string stateCode
    //)
    //{
    //  return DropDownList_Party_Load(
    //   dropDownListParty
    //  , stateCode
    //  , true
    //  );
    //}

    //#region DropDown State List

    //public static DropDownList DropDownList_Party_Load(
    //  DropDownList dropDownListParty
    //  , string stateCode
    //  , bool isExcludeNationalParties)
    //{
    //  dropDownListParty.Items.Clear();

    //  var dropDownParty = new ListItem {Value = "Z", Text = "--Select Different Party--"};
    //  dropDownListParty.Items.Add(dropDownParty);

    //  #region State Parties
    //  var sql = string.Empty;
    //  sql += " SELECT PartyKey,PartyCode,PartyName";
    //  sql += " FROM Parties";
    //  sql += " WHERE StateCode =" + db.SQLLit(stateCode);
    //  sql += " ORDER BY PartyOrder";
    //  //DataTable Table_Parties = db.Table(sql.Parties(db.State_Party, StateCode));
    //  var tableParties = db.Table(sql);
    //  foreach (DataRow rowParty in tableParties.Rows)
    //  {
    //    dropDownParty = new ListItem
    //    {
    //      Value = rowParty["PartyKey"].ToString(),
    //      Text = rowParty["PartyName"] + " (" + rowParty["PartyCode"] + ")"
    //    };
    //    dropDownListParty.Items.Add(dropDownParty);
    //  }
    //  #endregion State Parties

    //  #region Non-Pary Categories
    //  sql = string.Empty;
    //  sql += " SELECT PartyKey,PartyCode,PartyName";
    //  sql += " FROM Parties";
    //  sql += " WHERE StateCode ='US'";
    //  sql += " AND PartyCode = ''";
    //  sql += " ORDER BY PartyOrder";
    //  //Table_Parties = db.Table(sql.Parties(db.No_Party, "US"));
    //  tableParties = db.Table(sql);
    //  foreach (DataRow rowParty in tableParties.Rows)
    //  {
    //    dropDownParty = new ListItem
    //    {
    //      Value = rowParty["PartyKey"].ToString(),
    //      Text = rowParty["PartyName"].ToString()
    //    };
    //    dropDownListParty.Items.Add(dropDownParty);
    //  }
    //  #endregion


    //  #region Minor National Parties
    //  sql = string.Empty;
    //  sql += " SELECT PartyKey,PartyCode,PartyName";
    //  sql += " FROM Parties";
    //  sql += " WHERE StateCode ='US'";
    //  sql += " AND PartyCode != ''";
    //  sql += " AND IsPartyMajor = 0";
    //  sql += " ORDER BY PartyOrder";
    //  //Table_Parties = db.Table(sql.Parties(db.Minor_National_Party, "US"));
    //  tableParties = db.Table(sql);
    //  foreach (DataRow rowParty in tableParties.Rows)
    //  {
    //    dropDownParty = new ListItem
    //    {
    //      Value = rowParty["PartyKey"].ToString(),
    //      Text = rowParty["PartyName"] + " (" + rowParty["PartyCode"] + ")"
    //    };
    //    dropDownListParty.Items.Add(dropDownParty);
    //  }
    //  #endregion Minor National Parties

    //  if (!isExcludeNationalParties)
    //  {
    //    #region  National Parties
    //    sql = string.Empty;
    //    sql += " SELECT PartyKey,PartyCode,PartyName";
    //    sql += " FROM Parties";
    //    sql += " WHERE StateCode ='US'";
    //    sql += " AND IsPartyMajor = 1";
    //    sql += " ORDER BY PartyOrder";
    //    //Table_Parties = db.Table(sql.Parties(db.Minor_National_Party, "US"));
    //    tableParties = db.Table(sql);
    //    foreach (DataRow rowParty in tableParties.Rows)
    //    {
    //      dropDownParty = new ListItem
    //      {
    //        Value = rowParty["PartyKey"].ToString(),
    //        Text = rowParty["PartyName"] + " (" + rowParty["PartyCode"] + ")"
    //      };
    //      dropDownListParty.Items.Add(dropDownParty);
    //    }
    //    #endregion National Parties
    //  }

    //  return dropDownListParty;
    //}
    //#endregion DropDown State List

    //#endregion from db

    //#region checked

    //private void Check_TextBoxes_Not_Illegal()
    //{
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxFirst);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxMiddle);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxNickName);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxLast);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxSuffix);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAddOn);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxStreetAddress);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCityStateZipCode);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPhone);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEmailAddress);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxWebAddress);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCandidatePassword);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPasswordHint);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCampaignEmail);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCampaignPhone);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCampaignName);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCampaignAddr);

    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCampaignCityStateZip);
    //}

    //private void Set_Ballot_Name_Controls(string politicianKey, string officeKey)
    //{
    //  LabelCandidateName.Text = !string.IsNullOrEmpty(politicianKey)
    //    ? Politicians.GetFormattedName(politicianKey)
    //    : string.Empty;

    //  if (Politicians.GetStateCodeFromKey(politicianKey) == "DC" ||
    //    Offices.GetStateCodeFromKey(officeKey) == "DC")
    //  {
    //    tdANCText.Visible = true;
    //    tdANCTextBox.Visible = true;
    //  }
    //  else
    //  {
    //    tdANCText.Visible = false;
    //    tdANCTextBox.Visible = false;
    //  }

    //  if (!string.IsNullOrEmpty(politicianKey))
    //  {
    //    TextBoxFirst.Text = Politicians.GetFirstName(politicianKey, string.Empty);
    //    TextBoxMiddle.Text = Politicians.GetMiddleName(politicianKey, string.Empty);
    //    TextBoxNickName.Text = Politicians.GetNickname(politicianKey, string.Empty);
    //    TextBoxLast.Text = Politicians.GetLastName(politicianKey, string.Empty);
    //    TextBoxSuffix.Text = Politicians.GetSuffix(politicianKey, string.Empty);
    //    TextBoxAddOn.Text = Politicians.GetAddOn(politicianKey, string.Empty);
    //  }
    //  else
    //  {
    //    TextBoxFirst.Text = string.Empty;
    //    TextBoxMiddle.Text = string.Empty;
    //    TextBoxNickName.Text = string.Empty;
    //    TextBoxLast.Text = string.Empty;
    //    TextBoxSuffix.Text = string.Empty;
    //    TextBoxAddOn.Text = string.Empty;
    //  }

    //  tr_AddPolitician.Visible = string.IsNullOrEmpty(politicianKey);
    //}

    //#endregion checked

    //private void Set_Last_Office_Held_Or_Sought_Controls(string officeKey)
    //{
    //  HyperLinkOfficeTitleLine1Line2.Text = Offices.FormatOfficeName(officeKey);
    //  HyperLinkOfficeTitleLine1Line2.NavigateUrl =
    //    db.Url_Admin_Office_UPDATE(officeKey);

    //  TextBoxOfficeKey.Text = officeKey;
    //}

    //private void Set_Political_Party_Controls(string politicianKey)
    //{
    //  if (!string.IsNullOrEmpty(politicianKey))
    //  {
    //    Table_Party_Code.Visible = true;

    //    PartyCode.Text =
    //      db.Parties_Str(db.Politicians_Str(politicianKey, "PartyKey"), "PartyCode");

    //    if (Politicians.GetTemporaryOfficeKey(politicianKey) == "USPresident")
    //      DropDownListParty = DropDownList_Party_Load(DropDownListParty,
    //        Politicians.GetStateCodeFromKey(politicianKey));
    //    else
    //      DropDownListParty = DropDownList_Party_Load(DropDownListParty,
    //        Politicians.GetStateCodeFromKey(politicianKey));
    //    DropDownListParty.SelectedValue = "Z"; // ---Select Different Party---

    //    Party.Text = db.Parties_Str(db.Politicians_Str(politicianKey, "PartyKey")
    //      .Trim(), "PartyName");
    //    if (
    //      db.Parties_Str(db.Politicians_Str(politicianKey, "PartyKey"), "PartyURL") !=
    //        string.Empty)
    //    {
    //      HyperlinkPartyKey.NavigateUrl =
    //        NormalizeUrl(
    //          db.Parties_Str(db.Politicians_Str(politicianKey, "PartyKey"),
    //            "PartyURL"));
    //      HyperlinkPartyKey.Text = "Political Party Website";
    //    }
    //    else HyperlinkPartyKey.Text = string.Empty;
    //  }
    //  else Table_Party_Code.Visible = false;
    //}

    //private void Set_Politician_Incumbent_Status_Controls(string politicianKey,
    //  string officeKey)
    //{
    //  if ((!string.IsNullOrEmpty(politicianKey)) &&
    //    (!string.IsNullOrEmpty(officeKey)))
    //  {
    //    Table_Incumbent_Status.Visible = true;

    //    OfficeTitle4Inumbent.Text = Offices.FormatOfficeName(officeKey);

    //    var sql = string.Empty;
    //    sql += " SELECT OfficeKey FROM OfficesOfficials ";
    //    sql += " WHERE OfficeKey = " + db.SQLLit(officeKey);
    //    sql += " AND PoliticianKey = " + db.SQLLit(politicianKey);

    //    if (db.Row_Optional(sql) == null)
    //    {
    //      CheckBoxIsIncumbent.Checked = false;
    //      LabelIncumbentOffice.Visible = false;
    //      CheckBoxIsIncumbent.Text = "Politician is NOT the incumbent";
    //    }
    //    else
    //    {
    //      CheckBoxIsIncumbent.Checked = true;
    //      LabelIncumbentOffice.Visible = true;
    //      CheckBoxIsIncumbent.Text = "Politician IS the incumbent";
    //    }
    //    CheckBoxIsIncumbent.Text += " for " + Offices.FormatOfficeName(officeKey);
    //  }
    //  else Table_Incumbent_Status.Visible = false;
    //}

    //private void Set_Addresses_Controls(string politicianKey)
    //{
    //  if (!string.IsNullOrEmpty(politicianKey))
    //  {
    //    Table_Address.Visible = true;

    //    var politicianRow = db.Row(sql.RowPolitician(politicianKey));
    //    TextBoxPhone.Text = politicianRow["Phone"].ToString()
    //      .Trim();
    //    TextBoxEmailAddress.Text = politicianRow["EmailAddr"].ToString()
    //      .Trim();
    //    TextBoxWebAddress.Text = politicianRow["WebAddr"].ToString()
    //      .Trim();
    //    TextBoxStreetAddress.Text = politicianRow["Address"].ToString()
    //      .Trim();
    //    TextBoxCityStateZipCode.Text = politicianRow["CityStateZip"].ToString()
    //      .Trim();
    //  }
    //  else Table_Address.Visible = false;
    //}

    //private void Set_Edit_Office_Controls(string officeKey)
    //{
    //  if (!string.IsNullOrEmpty(officeKey))
    //  {
    //    Table_Edit_Office.Visible = true;

    //    HyperLinkOffice.Text = "Edit ";
    //    HyperLinkOffice.Text += Offices.FormatOfficeName(officeKey);
    //    HyperLinkOffice.Text += ", " +
    //      Offices.GetElectoralClassDescription(ViewState["StateCode"].ToString(),
    //        ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString());
    //    HyperLinkOffice.NavigateUrl = db.Url_Admin_Office_UPDATE(officeKey);
    //  }
    //  else Table_Edit_Office.Visible = false;
    //}

    //private void Set_Intro_Headshot_Controls(string politicianKey)
    //{
    //  if (!string.IsNullOrEmpty(politicianKey))
    //    if (IsSuperUser)
    //    {
    //      Table_Master_Politician_Intro.Visible = true;

    //      //Politician/IntroPage.aspx & Intro.aspx 
    //      Label_Intro_Page.Text = string.Empty;
    //      Label_Intro_Page.Text +=
    //        db.AnchorPoliticianImageOrNoPhoto(
    //          db.Url_Politician_Intro(politicianKey), politicianKey,
    //          db.Image_Size_25_Headshot,
    //          Politicians.GetFormattedName(politicianKey) +
    //            " Intro Page Data Entry", "_intro");

    //      Label_Intro_Page.Text += "&nbsp";

    //      Label_Intro_Page.Text += db.Anchor_Intro(politicianKey,
    //        Politicians.GetFormattedName(politicianKey) +
    //          " Public Introduction Page",
    //        Politicians.GetFormattedName(politicianKey) +
    //          " Public Introduction Page", "view");
    //      //-----------
    //      //Master/ImagesHeadshots.aspx 
    //      HyperLink_PoliticiansImages.NavigateUrl =
    //        Url_Master_ImagesHeadshots(politicianKey);
    //      HyperLink_PoliticiansImages.Text =
    //        " Profile and Headshot Images form for " +
    //          Politicians.GetFormattedName(politicianKey);

    //      HyperLinkOfficeIDs.NavigateUrl =
    //        db.Url_Admin_Offices(Politicians.GetOfficeClass(politicianKey)
    //          .ToInt());
    //    }
    //    else Table_Master_Politician_Intro.Visible = false;
    //  else Table_Master_Politician_Intro.Visible = false;
    //}

    //private void Set_Password_Campaign_Addresses_Controls(string politicianKey)
    //{
    //  if (!string.IsNullOrEmpty(politicianKey))
    //    if (IsSuperUser)
    //    {
    //      Table_Password_Delete_Reinstate.Visible = true;

    //      TextBoxCandidatePassword.Text = Politicians.GetPassword(politicianKey,
    //        string.Empty);
    //      TextBoxPasswordHint.Text = Politicians.GetPasswordHint(politicianKey,
    //        string.Empty);
    //      TextBoxCampaignEmail.Text = db.Politicians_Str(politicianKey,
    //        "CampaignEmail");
    //      TextBoxCampaignPhone.Text = db.Politicians_Str(politicianKey,
    //        "CampaignPhone");
    //      TextBoxCampaignName.Text = db.Politicians_Str(politicianKey,
    //        "CampaignName");
    //      TextBoxCampaignAddr.Text = db.Politicians_Str(politicianKey,
    //        "CampaignAddr");
    //      TextBoxCampaignCityStateZip.Text = db.Politicians_Str(politicianKey,
    //        "CampaignCityStateZip");
    //    }
    //    else Table_Password_Delete_Reinstate.Visible = false;
    //  else Table_Password_Delete_Reinstate.Visible = false;
    //}

    //private void Politicians_Change_Log(string dataItem, string fromDisplayed,
    //  string toTextBox)
    //{
    //  LogPoliticianChanges.Insert(DateTime.Now, UserSecurityClass,
    //    UserName, ViewState["PoliticianKey"].ToString(), dataItem,
    //    fromDisplayed.Trim()
    //      .ReplaceBreakTagsWithNewLines(), toTextBox.Trim());
    //}

    //private void PoliticiansUpdateAndLogChange(string columnName,
    //  string columnValue)
    //{
    //  Politicians_Change_Log(columnName,
    //    db.Politicians_Str(ViewState["PoliticianKey"].ToString(), columnName),
    //    columnValue);

    //  db.Politicians_Update_Str(ViewState["PoliticianKey"].ToString(), columnName,
    //    columnValue);
    //}

    //private void PoliticiansUpdateAndLogChange(string columnName,
    //  ITextControl textBox)
    //{
    //  PoliticiansUpdateAndLogChange(columnName, textBox.Text.Trim());
    //}

    //private static string MsgCommonUpdatePolitician(string information,
    //  string textBoxText)
    //{
    //  var msg = string.Empty;

    //  if (information != string.Empty)
    //    if (textBoxText != string.Empty)
    //      if (textBoxText.Trim() == string.Empty) msg = information + " has been deleted.";
    //      else msg = information + " has been changed to: " + textBoxText.Trim() + ".";
    //    else msg = information + " has been changed.";
    //  return msg;
    //}

    ////private void Update_Name_Controls()
    ////{
    ////  LabelCandidateName.Text =
    ////    Politicians.GetFormattedName(ViewState["PoliticianKey"].ToString());
    ////}

    //private string Str_Value_Changed(string strNewValue,
    //  db.Politician_Column column)
    //{
    //  db.Throw_Exception_If_Html_Or_Script(strNewValue);

    //  if (db.Politician(ViewState["PoliticianKey"].ToString(), column) !=
    //    strNewValue.Trim())
    //  {
    //    strNewValue = strNewValue.Trim();
    //    strNewValue = db.Str_Remove_Http(strNewValue);
    //    strNewValue = db.Str_Remove_MailTo(strNewValue);

    //    var dataDescription =
    //      db.Politician_Update_Transaction_Str(
    //        ViewState["PoliticianKey"].ToString(), column, strNewValue.Trim());

    //    Msg.Text = db.Ok(dataDescription + " was accepted.");
    //  }
    //  else
    //    Msg.Text =
    //      db.Fail(db.Politician_Column_Description(column) + " was NOT changed.");
    //  return strNewValue;
    //}

    //private void TextBox_Changed(ITextControl textBox, db.Politician_Column column)
    //{
    //  textBox.Text = Str_Value_Changed(textBox.Text, column);
    //}

    //protected void TextBoxFirst_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    if (TextBoxFirst.Text.Trim() == string.Empty) throw new ApplicationException("A first name needs to be provided.");

    //    TextBoxFirst.Text = Validation.FixGivenName(TextBoxFirst.Text);

    //    TextBox_Changed(TextBoxFirst, db.Politician_Column.FName);

    //    ResetPageCache();
    //    SetUpPage();
    //    //Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxMiddle_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBoxMiddle.Text = Validation.FixGivenName(TextBoxMiddle.Text);

    //    TextBox_Changed(TextBoxMiddle, db.Politician_Column.MName);

    //    ResetPageCache();
    //    SetUpPage();
    //    //Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxNickName_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBoxNickName.Text = Validation.FixNickname(TextBoxNickName.Text);

    //    TextBox_Changed(TextBoxNickName, db.Politician_Column.Nickname);

    //    ResetPageCache();
    //    SetUpPage();
    //    //Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxLast_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    if (TextBoxLast.Text.Trim() == string.Empty) throw new ApplicationException("A last name needs to be provided.");

    //    TextBoxLast.Text = Validation.FixLastName(TextBoxLast.Text);

    //    TextBox_Changed(TextBoxLast, db.Politician_Column.LName);

    //    ResetPageCache();
    //    SetUpPage();
    //    //Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxSuffix_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    if (!Validation.IsValidNameSuffix(TextBoxSuffix.Text.Trim())) throw new ApplicationException("The suffix is not valid.");

    //    TextBoxSuffix.Text = Validation.FixNameSuffix(TextBoxSuffix.Text);

    //    TextBox_Changed(TextBoxSuffix, db.Politician_Column.Suffix);

    //    ResetPageCache();
    //    SetUpPage();
    //    //Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxAddOn_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBox_Changed(TextBoxAddOn, db.Politician_Column.AddOn);

    //    ResetPageCache();
    //    SetUpPage();
    //    //Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxWebAddress_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBoxWebAddress.Text = TextBoxWebAddress.Text.ToLower();

    //    TextBox_Changed(TextBoxWebAddress, db.Politician_Column.WebAddr);

    //    ResetPageCache();
    //    SetUpPage();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxStreetAddress_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBoxStreetAddress.Text = Validation.FixAddress(TextBoxStreetAddress.Text);

    //    TextBox_Changed(TextBoxStreetAddress, db.Politician_Column.Address);

    //    ResetPageCache();
    //    SetUpPage();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxCityStateZipCode_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBoxCityStateZipCode.Text =
    //      Validation.FixCityStateZip(TextBoxCityStateZipCode.Text);

    //    TextBox_Changed(TextBoxCityStateZipCode, db.Politician_Column.CityStateZip);

    //    ResetPageCache();
    //    SetUpPage();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxPhoneNum_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBox_Changed(TextBoxPhone, db.Politician_Column.Phone);

    //    ResetPageCache();
    //    SetUpPage();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxEmailAddress_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBoxEmailAddress.Text = TextBoxEmailAddress.Text.ToLower();

    //    TextBox_Changed(TextBoxEmailAddress, db.Politician_Column.EmailAddr);

    //    ResetPageCache();
    //    SetUpPage();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxOfficeKey_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    if (!Offices.OfficeKeyExists(TextBoxOfficeKey.Text.Trim())) throw new ApplicationException("The Office ID is invalid or empty.");

    //    TextBox_Changed(TextBoxOfficeKey, db.Politician_Column.TemporaryOfficeKey);

    //    Set_Last_Office_Held_Or_Sought_Controls(TextBoxOfficeKey.Text.Trim());

    //    ResetPageCache();
    //    SetUpPage();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxCandidatePassword_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBox_Changed(TextBoxCandidatePassword, db.Politician_Column.Password);

    //    ResetPageCache();
    //    SetUpPage();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxPasswordHint_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    TextBox_Changed(TextBoxPasswordHint, db.Politician_Column.PasswordHint);

    //    ResetPageCache();
    //    SetUpPage();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxCampaignEmail_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCampaignEmail);

    //    if (
    //      db.Politicians_Str(ViewState["PoliticianKey"].ToString(), "CampaignEmail") !=
    //        TextBoxCampaignEmail.Text.Trim())
    //    {
    //      PoliticiansUpdateAndLogChange("CampaignEmail", TextBoxCampaignEmail);

    //      ResetPageCache();
    //      SetUpPage();

    //      Msg.Text =
    //        db.Ok(MsgCommonUpdatePolitician("Campaign Email Address",
    //          TextBoxCampaignEmail.Text));
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxCampaignPhone_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCampaignPhone);

    //    if (
    //      db.Politicians_Str(ViewState["PoliticianKey"].ToString(), "CampaignPhone") !=
    //        TextBoxCampaignPhone.Text.Trim())
    //    {
    //      PoliticiansUpdateAndLogChange("CampaignPhone", TextBoxCampaignPhone);

    //      ResetPageCache();
    //      SetUpPage();

    //      Msg.Text =
    //        db.Ok(MsgCommonUpdatePolitician("Campaign Phone",
    //          TextBoxCampaignPhone.Text));
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxCampaignName_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCampaignName);

    //    if (
    //      db.Politicians_Str(ViewState["PoliticianKey"].ToString(), "CampaignName") !=
    //        TextBoxCampaignName.Text.Trim())
    //    {
    //      PoliticiansUpdateAndLogChange("CampaignName", TextBoxCampaignName);

    //      ResetPageCache();
    //      SetUpPage();

    //      Msg.Text =
    //        db.Ok(MsgCommonUpdatePolitician("Campaign Name",
    //          TextBoxCampaignName.Text));
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxCampaignAddr_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCampaignAddr);

    //    if (
    //      db.Politicians_Str(ViewState["PoliticianKey"].ToString(), "CampaignAddr") !=
    //        TextBoxCampaignAddr.Text.Trim())
    //    {
    //      PoliticiansUpdateAndLogChange("CampaignAddr", TextBoxCampaignAddr);

    //      ResetPageCache();
    //      SetUpPage();

    //      Msg.Text =
    //        db.Ok(MsgCommonUpdatePolitician("Campaign Addrress",
    //          TextBoxCampaignAddr.Text));
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxCampaignCityStateZip_TextChanged(object sender,
    //  EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCampaignCityStateZip);

    //    if (
    //      db.Politicians_Str(ViewState["PoliticianKey"].ToString(),
    //        "CampaignCityStateZip") != TextBoxCampaignCityStateZip.Text.Trim())
    //    {
    //      PoliticiansUpdateAndLogChange("CampaignCityStateZip",
    //        TextBoxCampaignCityStateZip);

    //      ResetPageCache();
    //      SetUpPage();

    //      Msg.Text =
    //        db.Ok(MsgCommonUpdatePolitician("Campaign City State Zip",
    //          TextBoxCampaignCityStateZip.Text));
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void DropDownListParty_SelectedIndexChanged(object sender,
    //  EventArgs e)
    //{
    //  try
    //  {
    //    Check_TextBoxes_Not_Illegal();

    //    Str_Value_Changed(DropDownListParty.SelectedValue,
    //      db.Politician_Column.PartyKey);

    //    //Set_Political_Party_Controls(politicianKey);

    //    ResetPageCache();
    //    SetUpPage();

    //    Msg.Text = db.Ok("Your political party has been recorded.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void CheckBoxIsIncumbent_CheckedChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Check_TextBoxes_Not_Illegal();

    //    var politicianKey = ViewState["PoliticianKey"].ToString();
    //    var officeKey = PageCache.Politicians.GetOfficeKey(politicianKey);
    //    var electionKeyPreviousMostRecent =
    //      ElectionKey_Previous_Most_Recent(
    //        Politicians.GetStateCodeFromKey(politicianKey));

    //    if (CheckBoxIsIncumbent.Checked)
    //    {
    //      //Only for Msg
    //      var isIncumbent = Is_Incumbent(officeKey, politicianKey);

    //      //ElectionsPoliticians.IsIncumbent for an office contest is
    //      //normally set when politicians are added
    //      //as the office contest is created by OfficeContest.aspx
    //      //NOT when winners are identified by OfficeWinner.aspx.
    //      //
    //      //However, these can also be set on the Politician.aspx form 
    //      //where it will only be applied to the most recent past election
    //      //because no ElectionKey is provided. 

    //      //Set ElectionsPoliticians.IsIncumbent
    //      ElectionsPoliticians_Update_Bool(electionKeyPreviousMostRecent,
    //        officeKey, politicianKey, "IsIncumbent", true);

    //      Set_IsWinner_For_Election(electionKeyPreviousMostRecent, officeKey,
    //        politicianKey, true);

    //      if (isIncumbent)
    //        Msg.Text =
    //          db.Ok(Politicians.GetFormattedName(politicianKey) +
    //            " is now identified as the currently elected official (incumbent) for " +
    //            Offices.GetLocalizedOfficeName(officeKey) +
    //            " and winner in the most recent past election.");
    //      else
    //        Msg.Text =
    //          db.Ok(Politicians.GetFormattedName(politicianKey) +
    //            " is identified as the winner in the most recent past election for " +
    //            Offices.GetLocalizedOfficeName(officeKey) +
    //            " and winner in the most recent past election.");
    //    }
    //    else
    //    {
    //      OfficesOfficials_Delete(officeKey, politicianKey);

    //      //Set ElectionsPoliticians.IsIncumbent
    //      ElectionsPoliticians_Update_Bool(electionKeyPreviousMostRecent,
    //        officeKey, politicianKey, "IsIncumbent", false);

    //      Set_IsWinner_For_Election(electionKeyPreviousMostRecent, officeKey,
    //        politicianKey, false);

    //      Msg.Text =
    //        db.Ok(Politicians.GetFormattedName(politicianKey) +
    //          " is now NOT indicated as the currently elected official (incumbent) for " +
    //          Offices.GetLocalizedOfficeName(officeKey));
    //    }

    //    ResetPageCache();
    //    SetUpPage();

    //    //Set_Politician_Incumbent_Status_Controls(politicianKey, officeKey);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Button_Add_Politician_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Check_TextBoxes_Not_Illegal();

    //    if (!Validation.IsValidGivenName(TextBoxFirst.Text))
    //      throw new ApplicationException(
    //        string.Format(Validation.GivenNameValidationMessage, "The First Name"));

    //    if (!Validation.IsValidGivenName(TextBoxFirst.Text))
    //      throw new ApplicationException(
    //        string.Format(Validation.GivenNameValidationMessage, "The Middle Name"));

    //    if (!Validation.IsValidNickname(Str_Remove_Quotes(TextBoxNickName.Text)))
    //      throw new ApplicationException(
    //        string.Format(Validation.NicknameValidationMessage, "The Nickname"));

    //    if (!Validation.IsValidLastName(TextBoxLast.Text))
    //      throw new ApplicationException(
    //        string.Format(Validation.LastNameValidationMessage, "The Last Name"));

    //    if (!Validation.IsValidNameSuffix(TextBoxSuffix.Text))
    //      throw new ApplicationException(
    //        string.Format(Validation.NameSuffixValidationMessage, "The Suffix"));

    //    if ((TextBoxFirst.Text.Trim() == string.Empty) ||
    //      (TextBoxLast.Text.Trim() == string.Empty))
    //      throw new ApplicationException(
    //        "As a minimum you need to enter a First Name and Last Name.");

    //    if (!Offices.OfficeKeyExists(ViewState["OfficeKey"].ToString()))
    //      throw new ApplicationException(
    //        "There is no valid office for OfficeKey: " + ViewState["OfficeKey"]);

    //    var stateCode =
    //      Offices.GetStateCodeFromKey(ViewState["OfficeKey"].ToString());

    //    ViewState["PoliticianKey"] = db.Politician_Key(stateCode,
    //      db.Str_Remove_Puctuation(TextBoxLast.Text.Trim()),
    //      db.Str_Remove_Puctuation(TextBoxFirst.Text.Trim()),
    //      db.Str_Remove_Puctuation(TextBoxMiddle.Text.Trim()),
    //      db.Str_Remove_Puctuation(TextBoxSuffix.Text.Trim()));
    //    if (string.IsNullOrEmpty(ViewState["PoliticianKey"].ToString())) throw new ApplicationException("The new PoliticianKey is empty.");
    //    if (Politicians.IsValid(ViewState["PoliticianKey"].ToString()))
    //      throw new ApplicationException(
    //        "A politician with this exact name already exist in the database." +
    //          " If this is truly a new politician with the same name" +
    //          " you can still add this politician by modifying the name slightly so that a unique identity can be created." +
    //          " You do this by modifying the first, last or middle name," +
    //          " like Bill for William" +
    //          " or adding or deleting the middle initial." +
    //          " Then after you have added the politician, with a uniqe identity, you can edit the name as it should appear on ballots.");

    //    if (!Offices.OfficeKeyExists(ViewState["OfficeKey"].ToString()))
    //      throw new ApplicationException(
    //        "The Office does not exist for OfficeKey: " + ViewState["OfficeKey"]);

    //    const string partyKey = "X"; //No Party

    //    db.Politician_Insert(ViewState["PoliticianKey"].ToString(),
    //      ViewState["OfficeKey"].ToString(),
    //      Politicians.GetStateCodeFromKey(ViewState["PoliticianKey"].ToString()),
    //      TextBoxFirst.Text.Trim(), Str_Remove_Quotes(TextBoxMiddle.Text.Trim()),
    //      TextBoxLast.Text.Trim(), TextBoxSuffix.Text.Trim(),
    //      TextBoxAddOn.Text.Trim(), TextBoxNickName.Text.Trim(), partyKey);

    //    Response.Redirect(
    //      Url_Admin_Politician(ViewState["PoliticianKey"].ToString()));
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //    try
    //    {
    //      Page.Title = "Politician";
    //      ViewState["StateCode"] = string.Empty;
    //      ViewState["CountyCode"] = string.Empty;
    //      ViewState["LocalCode"] = string.Empty;
    //      ViewState["PoliticianKey"] = string.Empty;

    //      var politicianKey = SetUpPage();

    //      if (string.IsNullOrEmpty(politicianKey))
    //        Msg.Text =
    //          db.Msg("Enter the parts of the politician's name" +
    //            " exactly how it should appear on ballots." +
    //            " After each name part select Enter to check and edit its validity." +
    //            " Then click the Add Politician Button.");
    //      else
    //        Msg.Text =
    //          db.Ok("Add, change or delete information about the politician.");
    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text =
    //        db.Fail(
    //          " Press the back button to return to the previous page and report this problem: " +
    //            ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //}

    //private string SetUpPage()
    //{ 
    //  //Normally just a PoliticianKey is passed
    //  //to enable editing of the politician's data.
    //  //
    //  //However just an OfficeKey is can be passed
    //  //when a politician needs to be added NOT as an election is created.
    //  //This is the case when politicians are appointed to an office
    //  //or when a chalenger not in the database requests 
    //  //to be added.
    //  //
    //  //Only a PoliticianKey or OfficeKey is passed.
    //  //Never both.

    //  var politicianKey = string.Empty;
    //  var officeKey = string.Empty;
    //  var stateCode = string.Empty;

    //  if (!string.IsNullOrEmpty(QueryId)) ViewState["PoliticianKey"] = QueryId;

    //  if (!string.IsNullOrEmpty(ViewState["PoliticianKey"].ToString()))
    //  {
    //    politicianKey = ViewState["PoliticianKey"].ToString();
    //    officeKey = db.Politicians_Str(politicianKey, "TemporaryOfficeKey");

    //    ViewState["StateCode"] = Politicians.GetStateCodeFromKey(politicianKey);
    //    stateCode = Politicians.GetStateCodeFromKey(politicianKey);
    //  }
    //  else if (!string.IsNullOrEmpty(QueryOffice))
    //  {
    //    ViewState["OfficeKey"] = QueryOffice;
    //    officeKey = ViewState["OfficeKey"].ToString();

    //    ViewState["StateCode"] = Offices.GetStateCodeFromKey(officeKey);
    //    stateCode = Offices.GetStateCodeFromKey(officeKey);
    //  }

    //  if ((string.IsNullOrEmpty(politicianKey)) && (string.IsNullOrEmpty(officeKey)))
    //    throw new ApplicationException(
    //      "Either a politician ID or an OfficeKey needs to be passed in query string.");

    //  PageTitle.Text = string.Empty;
    //  if (string.IsNullOrEmpty(politicianKey)) PageTitle.Text += "Add a Politician";
    //  else PageTitle.Text += "Edit " + Politicians.GetFormattedName(politicianKey);
    //  PageTitle.Text += "<br>";
    //  if (!string.IsNullOrEmpty(officeKey)) PageTitle.Text += Offices.FormatOfficeName(officeKey) + ", ";
    //  PageTitle.Text += Offices.GetElectoralClassDescription(stateCode,
    //    ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString());

    //  PoliticianID.Text = politicianKey;

    //  Set_Ballot_Name_Controls(politicianKey, officeKey);

    //  Set_Last_Office_Held_Or_Sought_Controls(officeKey);

    //  Set_Political_Party_Controls(politicianKey);

    //  Set_Politician_Incumbent_Status_Controls(politicianKey, officeKey);

    //  Set_Addresses_Controls(politicianKey);

    //  Set_Edit_Office_Controls(officeKey);

    //  Set_Intro_Headshot_Controls(politicianKey);

    //  Set_Password_Campaign_Addresses_Controls(politicianKey);

    //  return politicianKey;
    //}

    #region Dead code

    #endregion Dead code
  }
}