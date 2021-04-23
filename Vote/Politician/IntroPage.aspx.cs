using System;

namespace Vote.Politician
{
  public partial class IntroPage : VotePage
  {
    //public static string Url_OrganizationDefault(string OrganizationCode)
    //{
    //  return "/Organization/Default.aspx?Organization=" + OrganizationCode;
    //}

    protected override void OnPreInit(EventArgs e)
    {
      //if (string.IsNullOrWhiteSpace(Request.QueryString["noredirect"]))
      {
        var query = Request.QueryString.ToString();
        var url = "/politician/updateintro.aspx";
        if (!string.IsNullOrWhiteSpace(query))
          url += "?" + query;
        Response.Redirect(url);
      }
      base.OnPreInit(e);
    }

    #region Dead code

    //#region SocialMedia methods

    //string SocialMediumHyperLinkName(SocialMedium medium)
    //{
    //  return "Hyperlink" + medium.Name + "WebAddress";
    //}

    //string SocialMediumTextBoxName(SocialMedium medium)
    //{
    //  return "TextBox" + medium.Name + "WebAddress";
    //}

    //void CreateSocialMediaDynamicControls()
    //{
    //  foreach (var medium in SocialMedia.SocialMediaList)
    //  {
    //    var tr = new TableRow();
    //    SocialMediaPlaceHolder.Controls.Add(tr);

    //    var tdLabel = new TableCell();
    //    tr.Controls.Add(tdLabel);
    //    tdLabel.Attributes.Add("align", "left");
    //    tdLabel.CssClass = "TBold";
    //    tdLabel.Style.Add(HtmlTextWriterStyle.Width, "142px");

    //    var link = new HyperLink();
    //    tdLabel.Controls.Add(link);
    //    link.ID = SocialMediumHyperLinkName(medium);
    //    link.Target = "view";
    //    link.Text = medium.Name + " Address:";

    //    var tdTextBox = new TableCell();
    //    tr.Controls.Add(tdTextBox);
    //    tdTextBox.Attributes.Add("align", "left");
    //    tdTextBox.CssClass = "T";

    //    var textBox = new TextBoxWithNormalizedLineBreaks();
    //    tdTextBox.Controls.Add(textBox);
    //    textBox.ID = SocialMediumTextBoxName(medium);
    //    textBox.Width = new Unit("770px");
    //    textBox.CssClass = "TextBoxInput";
    //    textBox.AutoPostBack = true;
    //    textBox.TextChanged += TextBoxSocialMedia_TextChanged;
    //  }
    //}

    //void InitializeSocialMediaDynamicControls()
    //{
    //  string politicianKey = ViewState["PoliticianKey"].ToString();
    //  foreach (var medium in SocialMedia.SocialMediaList)
    //  {
    //    string webAddress = medium.GetLinkFromDatabase(politicianKey);

    //    var link = FindControl(SocialMediumHyperLinkName(medium)) as HyperLink;
    //    if (!string.IsNullOrWhiteSpace(webAddress))
    //      link.NavigateUrl = db.Http(webAddress);

    //    var textBox = FindControl(SocialMediumTextBoxName(medium)) as TextBox;
    //    textBox.Text = webAddress;
    //  }
    //}

    //protected void TextBoxSocialMedia_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string politicianKey = ViewState["PoliticianKey"].ToString();
    //    TextBox textBox = sender as TextBox;
    //    string mediumName = Regex.Match(textBox.ID, @"TextBox(?<medium>[A-Z0-9]+)WebAddress",
    //      RegexOptions.IgnoreCase).Groups["medium"].Captures[0].Value;
    //    var medium = SocialMedia.GetMedium(mediumName);

    //    string oldWebAddress = medium.GetLinkFromDatabase(politicianKey);
    //    string newWebAddress = Validation.FixWebAddress(textBox.Text);
    //    db.Throw_Exception_If_Html_Or_Script(newWebAddress);
    //    textBox.Text = newWebAddress;

    //    UpdateSocialMedium(medium, oldWebAddress, newWebAddress);

    //    var link = FindControl(SocialMediumHyperLinkName(medium)) as HyperLink;
    //    if (string.IsNullOrWhiteSpace(newWebAddress))
    //      link.NavigateUrl = string.Empty;
    //    else
    //      link.NavigateUrl = db.Http(newWebAddress);
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void UpdateSocialMedium(SocialMedium medium,
    //  string oldValue, string newValue)
    //{
    //  string politicianKey = ViewState["PoliticianKey"].ToString();
    //  string description = medium.Description;

    //  if (oldValue != newValue)
    //  {
    //    // Log the change
    //    DB.VoteLog.LogPoliticianChanges.Insert(
    //      DateTime.Now,
    //      db.User_Security(),
    //      db.User_Name(),
    //      politicianKey,
    //      medium.DatabaseColumn,
    //      oldValue,
    //      newValue);

    //    medium.UpdateLink(politicianKey, newValue);
    //    db.Politician_Update_Post(politicianKey);
    //    Msg.Text = db.Ok(description + " was changed");
    //  }
    //  else
    //    //Msg.Text = db.Fail(description + " was NOT changed.");
    //    Msg.Text = db.Ok("although no changes were made to " + description);
    //}

    //#endregion SocialMedia methods

    //protected string Web_Address(string PoliticianKey)
    //{
    //  return db.Politician_WebAddress_Public_Text_Or_Empty(
    //      PoliticianKey
    //      );
    //}

    //protected void Set_Hyperlink_Web_Address(string PoliticianKey)
    //{
    //  if (!string.IsNullOrEmpty(Web_Address(PoliticianKey)))
    //  {
    //    HyperLink_Website.NavigateUrl = db.Http(Web_Address(PoliticianKey));
    //    HyperLink_Website.Target = "view";
    //  }
    //}

    //#region TextBox entry and changes

    //#region Politician Changes
    //protected void Update_Name_Controls()
    //{
    //  PoliticianName.Text =
    //    db.GetPoliticianName(ViewState["PoliticianKey"].ToString());
    //  LabelCandidateName.Text =
    //    db.GetPoliticianName(ViewState["PoliticianKey"].ToString());
    //}

    //protected string Str_Value_Changed(
    //  string Old_Value
    //  , string New_Value
    //  , db.Politician_Column Column
    //  , int Max_Length
    //  )
    //{
    //  // New_Value can be null for Politician Address, CityStateZip, Phone, 
    //  // EmailAddr and WebAddr
    //  if (New_Value != null)
    //    db.Throw_Exception_If_Html_Or_Script(New_Value);

    //  if (Max_Length != 0)
    //  {
    //    if (New_Value.Length > Max_Length)
    //      throw new ApplicationException("Your Response was "
    //        + New_Value.Length.ToString()
    //        + " characters. Your response can not exceed "
    //        + Max_Length.ToString() + " characters.");
    //  }

    //  //if (db.Politician(ViewState["PoliticianKey"].ToString(), Column)
    //  if (New_Value == null || Old_Value != New_Value.Trim())
    //  {
    //    if (New_Value != null)
    //    {
    //      New_Value = New_Value.Trim();
    //      New_Value = db.Str_Remove_Http(New_Value);
    //      New_Value = db.Str_Remove_MailTo(New_Value);
    //    }

    //    string Data_Description = db.Politician_Update_Transaction_Str(
    //      ViewState["PoliticianKey"].ToString()
    //    , Column
    //    , New_Value
    //    );

    //    db.Politician_Update_Post(
    //      ViewState["PoliticianKey"].ToString());

    //    Msg.Text = db.Ok(Data_Description + " was changed");
    //  }
    //  else
    //  {
    //    //Msg.Text = db.Fail(db.Politician_Column_Description(Column)
    //    //  + " was NOT changed.");
    //    Msg.Text = db.Ok("although no changes were made to " + 
    //      db.Politician_Column_Description(Column));
    //  }
    //  return New_Value;
    //}

    //protected void TextBox_Changed(
    //  TextBox TextBox
    //  , db.Politician_Column Column
    //  , int Max_Length
    //  )
    //{
    //  Str_Value_Changed(
    //    db.Politician(ViewState["PoliticianKey"].ToString(), Column)//old value
    //    , TextBox.Text.Trim()
    //    , Column
    //    , Max_Length
    //    );
    //}

    //protected void TextBox_Changed(
    //  TextBox TextBox
    //  , db.Politician_Column Column
    //  )
    //{
    //  Str_Value_Changed(
    //    db.Politician(ViewState["PoliticianKey"].ToString(), Column)//old value
    //    , TextBox.Text.Trim()
    //    , Column
    //    , 0
    //    );
    //}
    //protected void TextBox_Changed(
    //  string Old_Value
    //  , string New_Value
    //  , db.Politician_Column Column
    //  )
    //{
    //  Str_Value_Changed(
    //    Old_Value
    //    , New_Value
    //    , Column
    //    , 0
    //    );
    //}
    //#endregion Politician Changes

    //#region Name Textbox Changes
    //protected void TextBoxFirst_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Script(TextBoxFirst);

    //    if (TextBoxFirst.Text.Trim() == string.Empty)
    //      throw new ApplicationException("A first name needs to be provided.");

    //    //TextBoxFirst.Text = db.Str_Fix_Name(
    //    TextBoxFirst.Text = Validation.FixGivenName(
    //      TextBoxFirst.Text);

    //    TextBox_Changed(
    //      TextBoxFirst
    //      , db.Politician_Column.FName
    //      );

    //    Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxMiddle_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Script(TextBoxMiddle);

    //    TextBoxMiddle.Text = Validation.FixGivenName(
    //      TextBoxMiddle.Text);

    //    TextBox_Changed(
    //      TextBoxMiddle
    //      , db.Politician_Column.MName
    //      );

    //    Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxNickName_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {

    //    #region replaced
    //    //TextBox_Changed(
    //    //  ref TextBoxNickName
    //    //  , "Nickname"
    //    //  , "Nick Name"
    //    //  );

    //    //PoliticianName.Text = db.Name_Politician(ViewState["PoliticianKey"].ToString());
    //    //LabelCandidateName.Text = db.Name_Politician(ViewState["PoliticianKey"].ToString());
    //    #endregion replaced

    //    db.Throw_Exception_TextBox_Script(TextBoxNickName);
    //    //TextBoxNickName.Text = db.Str_Fix_Name(
    //    TextBoxNickName.Text = Validation.FixNickname(
    //      TextBoxNickName.Text);

    //    TextBox_Changed(
    //      TextBoxNickName
    //      , db.Politician_Column.Nickname
    //      );

    //    Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxLast_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {

    //    #region replaced
    //    //TextBox_Changed(
    //    //  ref TextBoxLast
    //    //  , "LName"
    //    //  , "Last Name"
    //    //  );

    //    //PoliticianName.Text = db.Name_Politician(ViewState["PoliticianKey"].ToString());
    //    //LabelCandidateName.Text = db.Name_Politician(ViewState["PoliticianKey"].ToString());
    //    #endregion replaced

    //    db.Throw_Exception_TextBox_Script(TextBoxLast);

    //    if (TextBoxLast.Text.Trim() == string.Empty)
    //      throw new ApplicationException("A last name needs to be provided.");

    //    TextBoxLast.Text = Validation.FixLastName(
    //      TextBoxLast.Text);

    //    TextBox_Changed(
    //      TextBoxLast
    //      , db.Politician_Column.LName
    //      );

    //    Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxSuffix_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Script(TextBoxSuffix);

    //    if (!Validation.IsValidNameSuffix(TextBoxSuffix.Text.Trim()))
    //      throw new ApplicationException("The suffix is not valid.");

    //    TextBoxSuffix.Text = Validation.FixNameSuffix(
    //      TextBoxSuffix.Text);

    //    TextBox_Changed(
    //      TextBoxSuffix
    //      , db.Politician_Column.Suffix
    //      );

    //    Update_Name_Controls();
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}
    //#endregion Name Textbox Changes

    //#region Political Party Changes
    //protected void DropDownListParty_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string politicianKey = ViewState["PoliticianKey"].ToString();

    //    #region Party
    //    if (db.Politicians_Str(politicianKey, "PartyKey")
    //      != DropDownListParty.SelectedValue)
    //    {
    //      //Politician_Update_Date_Count_Log_Invalidate
    //      //  (
    //      //  ViewState["PoliticianKey"].ToString()
    //      //  , "PartyKey"
    //      //  , db.Politicians_Str(ViewState["PoliticianKey"].ToString(), "PartyKey")
    //      //  , DropDownListParty.SelectedValue
    //      //  , db.User_Security_Default()
    //      //  , db.User_Name()
    //      //  );
    //      //------------
    //      Str_Value_Changed(
    //        db.Politician(politicianKey, db.Politician_Column.PartyKey)//old value
    //        , DropDownListParty.SelectedValue
    //        , db.Politician_Column.PartyKey
    //        , 0
    //        );
    //    }
    //    #endregion

    //    #region Reload Party controls
    //    PartyCode.Text = db.Html_From_Db_For_Page(db.Parties_Str(
    //        Politicians.GetPartyKey(politicianKey, string.Empty)
    //      , "PartyCode"
    //      )
    //      );

    //    Party.Text = db.Html_From_Db_For_Page(db.Parties_Str(
    //        Politicians.GetPartyKey(politicianKey, string.Empty)
    //      , "PartyName"
    //      )
    //      );

    //    if (db.Parties_Str(
    //        Politicians.GetPartyKey(politicianKey, string.Empty)
    //      , "PartyURL"
    //      )
    //      != string.Empty)
    //    {
    //      HyperlinkPartyKey.NavigateUrl = db.Http(db.Parties_Str(
    //        Politicians.GetPartyKey(politicianKey, string.Empty)
    //        , "PartyURL"
    //        ));
    //      HyperlinkPartyKey.Text = "Political Party Website";
    //    }
    //    else
    //      HyperlinkPartyKey.Text = string.Empty;
    //    #endregion Reload Party controls

    //    Msg.Text = db.Ok("Your political party has been recorded.");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }

    //}
    //#endregion Political Party Changes

    //#region Contact Information Changes

    //protected void TextBoxWebSite_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Script(TextBoxWebSite);

    //    string politicianKey = ViewState["PoliticianKey"].ToString();
    //    string WebAddress_Old = Politicians.GetPublicWebAddress(politicianKey);

    //    string newWebAddress = Validation.FixWebAddress(TextBoxWebSite.Text);

    //    // If we change back to the state version, we revert to null for default
    //    string updateWebAddress = newWebAddress;
    //    if (updateWebAddress ==
    //      Politicians.GetStateOrLDSWebAddress(politicianKey))
    //      updateWebAddress = null;

    //    TextBox_Changed(
    //      WebAddress_Old
    //      , updateWebAddress
    //      , db.Politician_Column.WebAddr//Column to update
    //      );

    //    // make sure the display reflects validation changes
    //    TextBoxWebSite.Text = newWebAddress;

    //    #region commented
    //    //if (
    //    //  (string.IsNullOrEmpty(TextBoxWebSite.Text.Trim()))
    //    //  && (!string.IsNullOrEmpty(WebAddress_Old))
    //    //  )
    //    //{
    //    //  #region Deleting All - Candidate, State and LDS Data
    //    //  db.Politician_Update_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //  , db.Politician_Column.StateWebAddr
    //    //  , string.Empty);

    //    //  db.Politician_Update_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //  , db.Politician_Column.LDSWebAddr
    //    //  , string.Empty);
    //    //  #endregion Deleting All - Candidate, State and LDS Data
    //    //}

    //    //string webAddress = db.Politician_WebAddress_Public_Text_Or_Empty(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //    );
    //    //if (!string.IsNullOrEmpty(webAddress))
    //    //  HyperLink_Website.NavigateUrl = db.Http(webAddress);
    //    ////db.Politician_WebAddress_Public_Text_Or_Empty(
    //    ////ViewState["PoliticianKey"].ToString()
    //    ////));
    //    #endregion commented

    //    Set_Hyperlink_Web_Address(politicianKey);
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxEmail_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Script(TextBoxEmail);

    //    string politicianKey = ViewState["PoliticianKey"].ToString();
    //    string Email_Old =
    //      Politicians.GetPublicEmail(politicianKey);

    //    string newEmail = Validation.FixWebAddress(TextBoxEmail.Text.Trim().ToLower());

    //    // If we change back to the state version, we revert to null for default
    //    string updateEmail = newEmail;
    //    if (updateEmail ==
    //     Politicians.GetStateOrLDSEmail(politicianKey))
    //      updateEmail = null;

    //    TextBox_Changed(
    //      Email_Old
    //      , updateEmail
    //      , db.Politician_Column.EmailAddr
    //      );

    //    // make sure the display reflects validation changes
    //    TextBoxEmail.Text = newEmail;

    //    //if (
    //    //  (string.IsNullOrEmpty(TextBoxEmail.Text.Trim()))
    //    //  && (!string.IsNullOrEmpty(Email_Old))
    //    //  )
    //    //{
    //    //  #region also delete State and LDS Data
    //    //  db.Politician_Update_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //  , db.Politician_Column.StateEmailAddr
    //    //  , string.Empty);

    //    //  db.Politician_Update_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //  , db.Politician_Column.LDSEmailAddr
    //    //  , string.Empty);
    //    //  #endregion also delete State and LDS Data
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

    //protected void TextBoxPhone_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string politicianKey = ViewState["PoliticianKey"].ToString();
    //    string Phone_Old =
    //      Politicians.GetPublicPhone(politicianKey);

    //    string newPhone = TextBoxPhone.Text.Trim();

    //    // If we change back to the state version, we revert to null for default
    //    string updatePhone = newPhone;
    //    if (updatePhone ==
    //      Politicians.GetStateOrLDSPhone(politicianKey))
    //      updatePhone = null;

    //    TextBox_Changed(
    //      Phone_Old
    //      , updatePhone
    //      , db.Politician_Column.Phone
    //      );

    //    // make sure the display reflects validation changes
    //    TextBoxPhone.Text = newPhone;

    //    //if (
    //    //  (string.IsNullOrEmpty(TextBoxPhone.Text.Trim()))
    //    //  && (!string.IsNullOrEmpty(Phone_Old))
    //    //  )
    //    //{
    //    //  #region also delete State and LDS Data
    //    //  db.Politician_Update_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //  , db.Politician_Column.StatePhone
    //    //  , string.Empty);

    //    //  db.Politician_Update_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //  , db.Politician_Column.LDSPhone
    //    //  , string.Empty);
    //    //  #endregion also delete State and LDS Data
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

    //protected void TextBoxAddress_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string politicianKey = ViewState["PoliticianKey"].ToString();
    //    string Address_Old =
    //      Politicians.GetPublicAddress(politicianKey);

    //    string newAddress = Validation.FixAddress(TextBoxAddress.Text);

    //    // If we change back to the state version, we revert to null for default
    //    string updateAddress = newAddress;
    //    if (updateAddress == Politicians.GetStateOrLDSAddress(politicianKey))
    //      updateAddress = null;

    //    TextBox_Changed(
    //      Address_Old
    //      , updateAddress
    //      , db.Politician_Column.Address
    //      );

    //    // make sure the display reflects validation changes
    //    TextBoxAddress.Text = newAddress;

    //    //if (
    //    //  (string.IsNullOrEmpty(TextBoxAddress.Text.Trim()))
    //    //  && (!string.IsNullOrEmpty(Address_Old))
    //    //  )
    //    //{
    //    //  #region also delete State and LDS Data
    //    //  db.Politician_Update_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //  , db.Politician_Column.StateAddress
    //    //  , string.Empty);

    //    //  db.Politician_Update_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //  , db.Politician_Column.LDSAddress
    //    //  , string.Empty);
    //    //  #endregion also delete State and LDS Data
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

    //protected void TextBoxCityStateZip_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string politicianKey = ViewState["PoliticianKey"].ToString();
    //    string CityStateZip_Old =
    //      Politicians.GetPublicCityStateZip(politicianKey);

    //    string newCityStateZip = Validation.FixCityStateZip(TextBoxCityStateZip.Text);

    //    // If we change back to the state version, we revert to null for default
    //    string updateCityStateZip = newCityStateZip;
    //    if (updateCityStateZip ==
    //     Politicians.GetStateOrLDSCityStateZip(politicianKey))
    //      updateCityStateZip = null;

    //    TextBox_Changed(
    //      CityStateZip_Old
    //      , updateCityStateZip
    //      , db.Politician_Column.CityStateZip
    //      );

    //    // make sure the display reflects validation changes
    //    TextBoxCityStateZip.Text = newCityStateZip;

    //    //if (
    //    //  (string.IsNullOrEmpty(TextBoxCityStateZip.Text.Trim()))
    //    //  && (!string.IsNullOrEmpty(CityStateZip_Old))
    //    //  )
    //    //{
    //    //  #region also delete State and LDS Data
    //    //  db.Politician_Update_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //  , db.Politician_Column.StateCityStateZip
    //    //  , string.Empty);

    //    //  db.Politician_Update_Str(
    //    //    ViewState["PoliticianKey"].ToString()
    //    //  , db.Politician_Column.LDSCityStateZip
    //    //  , string.Empty);
    //    //  #endregion also delete State and LDS Data
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

    //protected void TextBoxDateOfBirth_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    string politicianKey = ViewState["PoliticianKey"].ToString();
    //    db.Throw_Exception_TextBox_Script(TextBoxDateOfBirth);

    //    if (
    //      (!string.IsNullOrEmpty(TextBoxDateOfBirth.Text.Trim()))
    //      && (!db.Is_Valid_Date(TextBoxDateOfBirth.Text.Trim()))
    //      )
    //      throw new ApplicationException(
    //        "The Date textbox is not recognized as valid.");

    //    if (!string.IsNullOrEmpty(TextBoxDateOfBirth.Text.Trim()))
    //    {
    //      Politicians.UpdateDateOfBirth(Convert.ToDateTime(TextBoxDateOfBirth.Text), 
    //        politicianKey);
    //      Age.Text = db.Age(politicianKey);
    //    }
    //    else
    //    {
    //      Politicians.UpdateDateOfBirth(Convert.ToDateTime("1/1/1900"), politicianKey);
    //      Age.Text = string.Empty;
    //    }

    //    db.Politician_Update_Post(politicianKey);

    //    Msg.Text = db.Ok("Date of Birth was changed");
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}
    //#endregion Contact Information Changes

    //#region Biographical Changes
    //protected void TextBoxGeneral_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxGeneral);

    //    TextBox_Changed(
    //      TextBoxGeneral
    //      , db.Politician_Column.GeneralStatement
    //      , db.Max_Answer_Length_2000
    //      );
    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxPersonal_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPersonal);

    //    TextBox_Changed(
    //      TextBoxPersonal
    //      , db.Politician_Column.Personal
    //      , db.Max_Answer_Length_1000
    //      );

    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxEducation_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEducation);

    //    TextBox_Changed(
    //      TextBoxEducation
    //      , db.Politician_Column.Education
    //      , db.Max_Answer_Length_1000
    //      );

    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxProfession_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxProfession);

    //    TextBox_Changed(
    //      TextBoxProfession
    //      , db.Politician_Column.Profession
    //      , db.Max_Answer_Length_1000
    //      );

    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxMilitary_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxMilitary);

    //    TextBox_Changed(
    //      TextBoxMilitary
    //      , db.Politician_Column.Military
    //      , db.Max_Answer_Length_1000
    //      );

    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxCivic_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCivic);

    //    TextBox_Changed(
    //      TextBoxCivic
    //      , db.Politician_Column.Civic
    //      , db.Max_Answer_Length_1000
    //      );

    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxPolitical_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPolitical);

    //    TextBox_Changed(
    //      TextBoxPolitical
    //      , db.Politician_Column.Political
    //      , db.Max_Answer_Length_1000
    //      );

    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxReligion_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxReligion);

    //    TextBox_Changed(
    //      TextBoxReligion
    //      , db.Politician_Column.Religion
    //      , db.Max_Answer_Length_1000
    //      );

    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}

    //protected void TextBoxAccomplishments_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAccomplishments);

    //    TextBox_Changed(
    //      TextBoxAccomplishments
    //      , db.Politician_Column.Accomplishments
    //      , db.Max_Answer_Length_1000
    //      );

    //  }
    //  catch (Exception ex)
    //  {
    //    #region
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //    #endregion
    //  }
    //}
    //#endregion Biographical Changes

    //#endregion TextBox entry and changes

    //protected void ButtonUploadPicture_ServerClick1(object sender, EventArgs e)
    //{
    //  string returnMsg = string.Empty;
    //  string politicianKey = ViewState["PoliticianKey"].ToString();
    //  try
    //  {
    //    #region Image Upload and resize

    //    //upload date in PoliticianImages needs to match LogPoliticianImagesOriginal
    //    DateTime uploadTime = DateTime.Now;
    //    //string politicianKey = ViewState["PoliticianKey"].ToString();

    //    HttpPostedFile postedFile = Request.Files["PictureFile"];

    //    // We only update headshot images if there is no Headshot -- we check Headshot100
    //    byte[] blob;
    //    Size originalSize;
    //    if (PoliticiansImagesBlobs.GetHeadshot100(politicianKey) == null)
    //      blob = ImageManager.UpdateAllPoliticianImages(politicianKey,
    //       postedFile.InputStream, uploadTime, out originalSize);
    //    else
    //      blob = ImageManager.UpdatePoliticianProfileImages(politicianKey,
    //       postedFile.InputStream, uploadTime, out originalSize);
    //    CommonCacheInvalidation.ScheduleInvalidation("politicianimage", politicianKey);

    //    db.LogPoliticiansImagesOriginal_Insert(politicianKey, blob,
    //      uploadTime);

    //    #endregion Image Upload and resize

    //    db.Invalidate_Politician(
    //      //db.User_Name()
    //       politicianKey
    //       );

    //    #region remove Issue.aspx Cached Pages
    //    #region Note
    //    //remove Issue.aspx Cached Pages 
    //    //because there is now a small pic of candidate (after image is sent)
    //    #endregion Note
    //    //string officeKey = db.Politicians_Str(politicianKey, "OfficeKey");
    //    string officeKey = PageCache.GetTemporary().Politicians
    //      .GetOfficeKey(politicianKey);
    //    string electionKey = db.ElectionKey_State(db.Politicians_Str(politicianKey, "StateCode"));
    //    db.Cache_Remove_Issue_Pages_Office_Contest(electionKey, officeKey);//<--
    //    #endregion remove Issue.aspx Cached Pages

    //    CandidateImage.ImageUrl = InsertNoCacheIntoUrl(
    //      db.Url_Image_Politician_Or_NoPhoto(politicianKey, 
    //       db.Image_Size_200_Profile, db.Image_Size_200_Profile));

    //    #region  force reloading of new candidate image on the page
    //    string Url = db.Url_Politician_Intro();
    //    Url += "&Id=" + politicianKey;
    //    Url += "&Uploaded=The candidate picture was uploaded for your Introduction Page.";
    //    #endregion  force reloading of new candidate image on the page

    //    db.Politician_Update_DatePictureUploaded(
    //      politicianKey);

    //    db.Politician_Update_Post(
    //      politicianKey);

    //    Response.Redirect(db.Fix_Url_Parms(Url));
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //  CreateSocialMediaDynamicControls();

    //  if (!IsPostBack)
    //  {
    //    #region ViewState["PoliticianKey"]
    //    string politicianKey = db.PoliticianKey_ViewState();
    //    ViewState["PoliticianKey"] = politicianKey;
    //    #endregion ViewState["PoliticianKey"]

    //    #region Redirect if Bad Entry
    //    if (ViewState["PoliticianKey"].ToString() == string.Empty)
    //      db.HandleFatalError("The PoliticianKey is missing");
    //    #endregion

    //    try
    //    {
    //      InitializeSocialMediaDynamicControls();

    //      #region Heading
    //      PoliticianName.Text = db.GetPoliticianName(politicianKey);
    //      PoliticianOffice.Text = db.Politician_Current_Office_And_Status(politicianKey);
    //      PoliticianElection.Text = db.Politician_Current_Election(politicianKey);
    //      #endregion Heading

    //      #region HyperLinks
    //      #region View Your Introduction Page that We Provide to Voters
    //      HyperLinkViewIntro.NavigateUrl =
    //        UrlManager.GetIntroPageUri(
    //          ViewState["PoliticianKey"].ToString()).ToString();
    //      #endregion View Your Introduction Page that We Provide to Voters

    //      #region Enter Your Views and Positions on Issues
    //      HyperLinkViews.NavigateUrl =
    //        db.Url_Politician_IssueQuestions("ALLPersonal");
    //        //db.Url_Politician_IssueQuestions("ALLBio");
    //      #endregion Enter Your Views and Positions on Issues

    //      #region Submit Your Introduction Page to the Google Search Engine
    //      HyperLinkSearchEngineIntroPage.NavigateUrl
    //        = db.Url_Politician_SearchEngineSubmit_Intro_Page();
    //      #endregion Submit Your Introduction Page to the Google Search Engine
    //      #endregion HyperLinks

    //      #region Name

    //      LabelCandidateName.Text = db.GetPoliticianName(politicianKey);

    //      TextBoxFirst.Text = Politicians.GetFirstName(politicianKey, string.Empty);
    //      TextBoxMiddle.Text = Politicians.GetMiddleName(politicianKey, string.Empty);
    //      TextBoxNickName.Text = Politicians.GetNickname(politicianKey, string.Empty);
    //      TextBoxLast.Text = Politicians.GetLastName(politicianKey, string.Empty);
    //      TextBoxSuffix.Text = Politicians.GetSuffix(politicianKey, string.Empty);

    //      if (SecurePage.IsSuperUser)
    //      {
    //        TextBoxFirst.Enabled = true;
    //        TextBoxMiddle.Enabled = true;
    //        TextBoxNickName.Enabled = true;
    //        TextBoxLast.Enabled = true;
    //        TextBoxSuffix.Enabled = true;
    //      }
    //      else
    //      {
    //        TextBoxFirst.Enabled = false;
    //        TextBoxMiddle.Enabled = false;
    //        TextBoxNickName.Enabled = false;
    //        TextBoxLast.Enabled = false;
    //        TextBoxSuffix.Enabled = false;
    //      }
    //      #endregion Name

    //      #region removed Load politician Bio Answers

    //      //General.Text = db.Html_From_Db_For_Page(
    //      //      db.Politician_GeneralStatement_Or_No_Response(
    //      //      politicianKey)
    //      //);

    //      //Personal.Text = db.Html_From_Db_For_Page(
    //      //      db.Politician_Personal_Or_No_Response(
    //      //      politicianKey)
    //      //);

    //      //Education.Text = db.Html_From_Db_For_Page(
    //      //      db.Politician_Education_Or_No_Response(
    //      //      politicianKey)
    //      //);

    //      //Profession.Text = db.Html_From_Db_For_Page(
    //      //      db.Politician_Profession_Or_No_Response(
    //      //      politicianKey)
    //      //      );

    //      //Military.Text = db.Html_From_Db_For_Page(
    //      //      db.Politician_Military_Or_No_Response(
    //      //      politicianKey)
    //      //      );

    //      //Civic.Text = db.Html_From_Db_For_Page(
    //      //      db.Politician_Civic_Or_No_Response(
    //      //      politicianKey)
    //      //      );

    //      //Political.Text = db.Html_From_Db_For_Page(
    //      //      db.Politician_Civic_Or_No_Response(
    //      //      politicianKey)
    //      //      );

    //      //Religion.Text = db.Html_From_Db_For_Page(
    //      //  db.Politician_Religion_Or_No_Response(
    //      //      politicianKey)
    //      //      );

    //      //Accomplishments.Text = db.Html_From_Db_For_Page(
    //      //  db.Politician_Accomplishments_Or_No_Response(
    //      //      politicianKey)
    //      //      );
    //      #endregion removed Load politician Bio Answers

    //      #region Party
    //      PartyCode.Text = db.Html_From_Db_For_Page(db.Parties_Str(
    //        //PoliticianRow["PartyKey"].ToString()
    //            Politicians.GetPartyKey(politicianKey, string.Empty)
    //        , "PartyCode"
    //        ));

    //      //Party.Text = db.Html_From_Db_For_Page(db.Parties_Str(PartyID.Text, "PartyName"));
    //      Party.Text = db.Html_From_Db_For_Page(db.Parties_Str(
    //        //PoliticianRow["PartyKey"].ToString()
    //            Politicians.GetPartyKey(politicianKey, string.Empty)
    //        , "PartyName"
    //        ));

    //      if (db.Parties_Str(
    //        //PoliticianRow["PartyKey"].ToString()
    //            Politicians.GetPartyKey(politicianKey, string.Empty)
    //        , "PartyURL"
    //        ) != string.Empty)
    //      {
    //        HyperlinkPartyKey.NavigateUrl = db.Http(
    //            db.Parties_Str(
    //          //PoliticianRow["PartyKey"].ToString()
    //            Politicians.GetPartyKey(politicianKey, string.Empty)
    //          , "PartyURL"
    //          ));
    //        HyperlinkPartyKey.Text = "Political Party Website";
    //      }
    //      else
    //        HyperlinkPartyKey.Text = string.Empty;

    //      DropDownListParty = db.DropDownList_Party_Load(
    //        DropDownListParty
    //        , Politicians.GetStateCodeFromKey(
    //        politicianKey)
    //        //, false);//exclude national parties
    //      );//exclude national parties

    //      DropDownListParty.SelectedValue = db.Politicians_Str(
    //        politicianKey
    //        , "PartyKey");

    //      #endregion Party

    //      #region Your Contact Info

    //      PoliticiansTable table = Politicians.GetData(politicianKey);
    //      if (table.Count != 0)
    //      {
    //        PoliticiansRow row = table[0];

    //        TextBoxWebSite.Text = Web_Address(politicianKey);
    //        Set_Hyperlink_Web_Address(politicianKey);

    //        TextBoxEmail.Text = row.PublicEmail;
    //        TextBoxPhone.Text = row.PublicPhone;
    //        TextBoxAddress.Text = row.PublicAddress;
    //        TextBoxCityStateZip.Text = row.PublicCityStateZip;

    //        TextBoxDateOfBirth.Text = db.Str_Date_Of_Birth(politicianKey);

    //        if (!string.IsNullOrEmpty(TextBoxDateOfBirth.Text.Trim()))
    //          Age.Text = db.Age(politicianKey);
    //        else
    //          Age.Text = string.Empty;
    //      }
    //      #endregion Your Contact Info

    //      #region YouTube
    //      if (SecurePage.IsMasterUser)
    //      {
    //        trYouTubeInstruction.Visible = true;
    //        trYouTubeHyperlink.Visible = true;
    //      }
    //      else
    //      {
    //        trYouTubeInstruction.Visible = false;
    //        trYouTubeHyperlink.Visible = false;
    //      }
    //      #endregion YouTube

    //      #region Photo
    //      CandidateImage.ImageUrl = InsertNoCacheIntoUrl(
    //        db.Url_Image_Politician_Or_NoPhoto(politicianKey, 
    //         db.Image_Size_300_Profile, db.Image_Size_200_Profile));
    //      #endregion Photo

    //      #region Bio
    //      TextBoxGeneral.Text = Politicians.GetGeneralStatement(politicianKey, string.Empty);
    //      TextBoxPersonal.Text = Politicians.GetPersonal(politicianKey, string.Empty);
    //      TextBoxEducation.Text = Politicians.GetEducation(politicianKey, string.Empty);
    //      TextBoxProfession.Text = Politicians.GetProfession(politicianKey, string.Empty);
    //      TextBoxMilitary.Text = Politicians.GetMilitary(politicianKey, string.Empty);
    //      TextBoxCivic.Text = Politicians.GetCivic(politicianKey, string.Empty);
    //      TextBoxPolitical.Text = Politicians.GetPolitical(politicianKey, string.Empty);
    //      TextBoxReligion.Text = Politicians.GetReligion(politicianKey, string.Empty);
    //      TextBoxAccomplishments.Text = Politicians.GetAccomplishments(politicianKey, string.Empty);
    //      #endregion Bio

    //      Msg.Text = db.Msg("Use this form to add, change and delete"
    //      + " what is currently on your Introduction Page.");

    //      if (!string.IsNullOrEmpty(db.QueryString("Uploaded")))
    //        //on second rendering of the page to present new image
    //        Msg.Text = db.Ok(db.QueryString("Uploaded"));
    //      else if (!string.IsNullOrEmpty(db.QueryString("UploadedErr")))
    //        //on second rendering of the page to present new image
    //        Msg.Text = db.Fail(db.QueryString("UploadedErr"));
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

    #endregion Dead code


  }
}
