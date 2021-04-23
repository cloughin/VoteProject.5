using System;
using DB.Vote;

namespace Vote.Admin
{
  public partial class AuthorityPage : SecureAdminPage
  {
    #region Private

    //private void CheckTextBoxesForIllegalInput()
    //{
    //  //Election Authority Contact Information for Vote-USA
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxContact);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxContactTitle);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxContactEmail);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPhone);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAltContact);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAltContactTitle);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAltEmail);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAltPhone);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAdressLine1);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAddressLine2);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxCityStateZip);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxNotes);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEmail);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxURL);
    //  //Election Authority Information for Voters on Pages
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEmail);
    //  db.Throw_Exception_TextBox_Html_Or_Script(TextBoxURL);
    //}

    //private void SetPageTitle()
    //{
    //  PageTitle.Text = string.Empty;
    //  PageTitle.Text += Offices.GetElectoralClassDescription(ViewState["StateCode"].ToString(),
    //    ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString());

    //  PageTitle.Text += "<br>";
    //  PageTitle.Text += "ELECTION AUTHORITY";
    //}

    //private string Get(string column)
    //{
    //  if (!string.IsNullOrEmpty(ViewState["LocalCode"].ToString()))

    //    return db.GetLocalDistricts(ViewState["StateCode"].ToString(),
    //      ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString(),
    //      column);

    //  if (!string.IsNullOrEmpty(ViewState["CountyCode"].ToString()))

    //    return db.Counties(ViewState["StateCode"].ToString(),
    //      ViewState["CountyCode"].ToString(), column);

    //  return db.States_Str(ViewState["StateCode"].ToString(), column);
    //}

    //private void Update(string column, string newValue, string oldValue,
    //  bool isLogUpdate = true)
    //{
    //  db.States_Counties_LocalDistricts_Update(ViewState["StateCode"].ToString(),
    //    ViewState["CountyCode"].ToString(), ViewState["LocalCode"].ToString(), column,
    //    newValue, oldValue, isLogUpdate);
    //}

    //private void Update(string column, string newValue)
    //{
    //  Update(column, newValue, string.Empty, false);
    //}

    //private void RecordElectionAuthorityInformation()
    //{
    //  //These are not logged because I don't want to pay for these updates
    //  Update("Contact", TextBoxContact.Text.Trim());
    //  Update("ContactTitle", TextBoxContactTitle.Text.Trim());
    //  Update("ContactEmail", TextBoxContactEmail.Text.Trim());
    //  Update("Phone", TextBoxPhone.Text.Trim());
    //  Update("AltContact", TextBoxAltContact.Text.Trim());
    //  Update("AltContactTitle", TextBoxAltContactTitle.Text.Trim());
    //  Update("AltEMail", TextBoxAltEmail.Text.Trim());
    //  Update("AltPhone", TextBoxAltPhone.Text.Trim());
    //  Update("AddressLine1", TextBoxAdressLine1.Text.Trim());
    //  Update("AddressLine2", TextBoxAddressLine2.Text.Trim());
    //  Update("CityStateZip", TextBoxCityStateZip.Text.Trim());
    //  Update("Notes", TextBoxNotes.Text.Trim());

    //  if (!string.IsNullOrEmpty(ViewState["LocalCode"].ToString()))
    //    Msg.Text = db.Ok("The State Election Authority Data was Recorded.");
    //  else if (!string.IsNullOrEmpty(ViewState["CountyCode"].ToString()))
    //    Msg.Text = db.Ok("The County Election Authority Data was Recorded.");
    //  else
    //    Msg.Text = db.Ok("The Local District Election Authority Data was Recorded.");
    //}

    //private void LoadElectionAuthorityInformationVoteUSA()
    //{
    //  TextBoxContact.Text = Get("Contact");
    //  TextBoxContactTitle.Text = Get("ContactTitle");
    //  HyperLinkContactEmail.NavigateUrl = "mailto:" + Get("ContactEmail");
    //  HyperLinkContactEmail.Text = Get("ContactEmail");
    //  TextBoxContactEmail.Text = Get("ContactEmail");
    //  TextBoxPhone.Text = Get("Phone");
    //  TextBoxAltContact.Text = Get("AltContact");
    //  TextBoxAltContactTitle.Text = Get("AltContactTitle");
    //  HyperLinkAltEMail.Text = Get("AltEMail");
    //  HyperLinkAltEMail.NavigateUrl = "mailto:" + Get("AltEMail");
    //  TextBoxAltEmail.Text = Get("AltEMail");
    //  TextBoxAltPhone.Text = Get("AltPhone");
    //  //TextBoxElectionsAuthority.Text = Get("ElectionsAuthority");
    //  TextBoxAdressLine1.Text = Get("AddressLine1");
    //  TextBoxAddressLine2.Text = Get("AddressLine2");
    //  TextBoxCityStateZip.Text = Get("CityStateZip");
    //  TextBoxNotes.Text = Get("Notes");
    //}

    //private void LoadEmailForVoters()
    //{
    //  if (Get("EMail") != string.Empty)
    //  {
    //    HyperLinkEmail.NavigateUrl = "mailto:" + Get("EMail");
    //    HyperLinkEmail.Text = Get("EMail");
    //    TextBoxEmail.Text = Get("EMail");
    //  }
    //  else
    //  {
    //    HyperLinkEmail.NavigateUrl = string.Empty;
    //    HyperLinkEmail.Text = string.Empty;
    //    TextBoxEmail.Text = string.Empty;
    //  }
    //}

    //private void LoadUrlForVoters()
    //{
    //  if (Get("URL") != string.Empty)
    //  {
    //    //HyperLinkURL.NavigateUrl = db.Http() + Get("URL");
    //    HyperLinkURL.NavigateUrl = NormalizeUrl(Get("URL"));
    //    //HyperLinkURL.Text = Get("URL");
    //    //TextBoxURL.Text = Get("URL");
    //    HyperLinkURL.Text = "Election Authority Page";
    //    TextBoxURL.Text = Get("URL");
    //  }
    //  else
    //  {
    //    HyperLinkURL.NavigateUrl = string.Empty;
    //    HyperLinkURL.Text = string.Empty;
    //    TextBoxURL.Text = string.Empty;
    //  }
    //}

    //private void LoadElectionInformationForVoters()
    //{
    //  LoadEmailForVoters();
    //  LoadUrlForVoters();
    //}

    #endregion Private

    #region Event handlers and overrides

    protected override void OnInit(EventArgs e)
    {
      LegacyRedirect(GetUpdateJurisdictionsPageUrl(QueryState,
        QueryCounty, QueryLocal));
      base.OnInit(e);
    }

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    // Security Check and Values for ViewState["StateCode"] ViewState["CountyCode"] ViewState["LocalCode"]

    //    //The Session UserStateCode, UserCountyCode, UserLocalCode can be changed
    //    //by a higher administration level using query strings
    //    //This is done in db.State_Code(), db.County_Code(), db.Local_Code()
    //    //
    //    //Using ViewState variables insures these values won't
    //    //change on any postbacks or in different tab or browser Sessions.
    //    //
    //    //ViewState["StateCode"] can be a StateCode or U1, u2, u3 for FederalCode

    //    ViewState["StateCode"] = db.State_Code();
    //    ViewState["CountyCode"] = db.County_Code();
    //    ViewState["LocalCode"] = db.Local_Code();
    //    if (!db.Is_User_Security_Ok())
    //      SecurePage.HandleSecurityException();

    //    Page.Title = "Authority";

    //    try
    //    {
    //      SetPageTitle();

    //      LoadElectionAuthorityInformationVoteUSA();
    //      LoadElectionInformationForVoters();
    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}

    //protected void TextBoxEmail_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEmail);

    //    if (Get("EMail") != TextBoxEmail.Text.Trim())
    //    {
    //      TextBoxEmail.Text = db.Str_Remove_MailTo(TextBoxEmail.Text.Trim());
    //      Update("EMail", TextBoxEmail.Text.Trim());

    //      LoadEmailForVoters();

    //      Msg.Text = db.Ok("The election authority email address was recorded.");
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxURL_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    db.Throw_Exception_TextBox_Html_Or_Script(TextBoxURL);

    //    if (Get("URL") != TextBoxURL.Text.Trim())
    //    {
    //      TextBoxURL.Text = db.Str_Remove_Http(TextBoxURL.Text.Trim());
    //      Update("URL", TextBoxURL.Text.Trim());

    //      LoadUrlForVoters();

    //      //db.Cache_Remove_Domain_Data(
    //      //  ViewState["StateCode"].ToString());

    //      Msg.Text = db.Ok("The election authority web address was recorded.");
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void ButtonSwitchContacts_Click(object sender, EventArgs e)
    //{
    //  CheckTextBoxesForIllegalInput();

    //  var name = TextBoxContact.Text;
    //  var title = TextBoxContactTitle.Text;
    //  var email = TextBoxContactEmail.Text;
    //  var phone = TextBoxPhone.Text;

    //  TextBoxContact.Text = TextBoxAltContact.Text;
    //  TextBoxContactTitle.Text = TextBoxAltContactTitle.Text;
    //  TextBoxContactEmail.Text = TextBoxAltEmail.Text;
    //  TextBoxPhone.Text = TextBoxAltPhone.Text;

    //  TextBoxAltContact.Text = name;
    //  TextBoxAltContactTitle.Text = title;
    //  TextBoxAltEmail.Text = email;
    //  TextBoxAltPhone.Text = phone;

    //  RecordElectionAuthorityInformation();

    //  LoadElectionAuthorityInformationVoteUSA();
    //}

    //protected void TextBoxContact_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();
    //    Update("Contact", TextBoxContact.Text.Trim(), Get("Contact"));
    //    TextBoxContact.Text = Get("Contact");
    //    Msg.Text = db.Ok("The MAIN CONTACT NAME has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxContactTitle_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();
    //    Update("ContactTitle", TextBoxContactTitle.Text.Trim(), Get("ContactTitle"));
    //    TextBoxContactTitle.Text = Get("ContactTitle");
    //    Msg.Text = db.Ok("The MAIN TITLE has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxContactEmail_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();

    //    TextBoxContactEmail.Text =
    //      Validation.FixWebAddress(TextBoxContactEmail.Text.Trim());

    //    Update("ContactEmail", TextBoxContactEmail.Text.Trim(), Get("ContactEmail"));
    //    HyperLinkContactEmail.NavigateUrl = "mailto:" + Get("ContactEmail");
    //    HyperLinkContactEmail.Text = Get("ContactEmail");
    //    TextBoxContactEmail.Text = Get("ContactEmail");
    //    Msg.Text = db.Ok("The MAIN CONTACT NAME has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxPhone_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();
    //    Update("Phone", TextBoxPhone.Text.Trim(), Get("Phone"));
    //    TextBoxPhone.Text = Get("Phone");
    //    Msg.Text = db.Ok("The MAIN PHONE has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxAltContact_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();
    //    Update("AltContact", TextBoxAltContact.Text.Trim(), Get("AltContact"));
    //    TextBoxAltContact.Text = Get("AltContact");
    //    Msg.Text = db.Ok("The ALTERNATE NAME has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxAltContactTitle_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();
    //    Update("AltContactTitle", TextBoxAltContactTitle.Text.Trim(),
    //      Get("AltContactTitle"));
    //    TextBoxAltContactTitle.Text = Get("AltContactTitle");
    //    Msg.Text = db.Ok("The ALTERNATE TITLE has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxAltEmail_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();

    //    TextBoxAltEmail.Text = Validation.FixWebAddress(TextBoxAltEmail.Text.Trim());

    //    Update("AltEMail", TextBoxAltEmail.Text.Trim(), Get("AltEMail"));
    //    HyperLinkAltEMail.Text = Get("AltEMail");
    //    HyperLinkAltEMail.NavigateUrl = "mailto:" + Get("AltEMail");
    //    TextBoxAltEmail.Text = Get("AltEMail");
    //    Msg.Text = db.Ok("The ALTERNAME EMAIL has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxAltPhone_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();
    //    Update("AltPhone", TextBoxAltPhone.Text.Trim(), Get("AltPhone"));
    //    TextBoxAltPhone.Text = Get("AltPhone");
    //    Msg.Text = db.Ok("The ALTERNAME PHONE has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxAdressLine1_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();
    //    Update("AddressLine1", TextBoxAdressLine1.Text.Trim(), Get("AddressLine1"));
    //    TextBoxAdressLine1.Text = Get("AddressLine1");
    //    Msg.Text = db.Ok("The ADDRESS LINE 1 has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxAddressLine2_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();
    //    Update("AddressLine2", TextBoxAddressLine2.Text.Trim(), Get("AddressLine2"));
    //    TextBoxAddressLine2.Text = Get("AddressLine2");
    //    Msg.Text = db.Ok("The ADDRESS LINE 2 has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxCityStateZip_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();
    //    Update("CityStateZip", TextBoxCityStateZip.Text.Trim(), Get("CityStateZip"));
    //    TextBoxCityStateZip.Text = Get("CityStateZip");
    //    Msg.Text = db.Ok("The ADDRESS STATE AND ZIP has been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    //protected void TextBoxNotes_TextChanged(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    CheckTextBoxesForIllegalInput();
    //    Update("Notes", TextBoxNotes.Text.Trim(), Get("Notes"));
    //    TextBoxNotes.Text = Get("Notes");
    //    Msg.Text = db.Ok("The NOTES have been updated.");
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    #endregion Event handlers and overrides

    #region Dead code

    //protected void Msg_Update()
    //{
    //  if (!string.IsNullOrEmpty(ViewState["LocalCode"].ToString()))
    //    Msg.Text = db.Ok("The State Election Authority Data was Recorded.");
    //  else if (!string.IsNullOrEmpty(ViewState["CountyCode"].ToString()))
    //    Msg.Text = db.Ok("The County Election Authority Data was Recorded.");
    //  else
    //    Msg.Text = db.Ok("The Local District Election Authority Data was Recorded.");
    //}

    //protected void xLoadBallotName4Voters()
    //{
    //  //TextBoxBallotName.Text = Get("BallotName");
    //}

    //protected void xButtonElectionAuthority_Click(object sender, EventArgs e)
    //{
    //  try
    //  {
    //    Check_TextBoxs_Illeagal_Input();

    //    Record_Election_Authority_Information();

    //    Load_Election_Authority_Information_VoteUSA();
    //  }
    //  catch (Exception ex)
    //  {
    //    Msg.Text = db.Fail(ex.Message);
    //    db.Log_Error_Admin(ex);
    //  }
    //}

    #endregion Dead code
  }
}