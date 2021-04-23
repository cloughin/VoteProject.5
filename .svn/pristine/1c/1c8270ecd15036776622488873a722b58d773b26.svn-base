//using System;
//using System.Data;
//using System.Web.UI.HtmlControls;
//using System.Net.Mail;
//using DB.Vote;
//using System.Web.UI.WebControls;

namespace Vote.Master
{
  public partial class VolunteersPage : SecurePage
  {
//    #region from db

//    public static string Url_Master_Volunteers(string email)
//    {
//      var url = "/Master/Volunteers.aspx";
//      if (!string.IsNullOrWhiteSpace(email))
//        url += "?Email=" + email;
//      return url;
//    }

//    public static void Master_Update_Str(string column, string columnValue)
//    {
//      var updateSQL = "UPDATE Master "
//        + " SET " + column + " = " + db.SQLLit(columnValue.Trim())
//        + " WHERE ID = 'MASTER' ";
//      db.ExecuteSql(updateSQL);
//    }

//    #endregion from db

//    #region The All Volunteers report

//    private void CreateAllVolunteersReportHeading(HtmlTable htmlTable)
//    {
//      var trHeading =
//        db.Add_Tr_To_Table_Return_Tr(htmlTable, "trReportDetailHeading");

//      db.Add_Td_To_Tr(trHeading, "State", "tdReportDetailHeading");
//      db.Add_Td_To_Tr(trHeading, "Party", "tdReportDetailHeading partyColumn");
//      db.Add_Td_To_Tr(trHeading, "Email Address", "tdReportDetailHeading");
//      db.Add_Td_To_Tr(trHeading, "Phone", "tdReportDetailHeading");
//      db.Add_Td_To_Tr(trHeading, "First Name", "tdReportDetailHeading");
//      db.Add_Td_To_Tr(trHeading, "Last Name", "tdReportDetailHeading");
//      db.Add_Td_To_Tr(trHeading, "Password", "tdReportDetailHeading");
//    }

//    private void CreateAllVolunteersReportRow(HtmlTable htmlTable,
//      VolunteersViewRow dataRow)
//    {
//      var htmlRow = db.Add_Tr_To_Table_Return_Tr(htmlTable,
//        "trReportDetailHeading");

//      var stateName =
//        dataRow.StateName ?? "** Missing State **";
//      var partyName =
//        dataRow.PartyName ?? "** Missing Party **";
//      var volunteerAnchor = string.Format("<a href=\"{0}\">{1}</a>",
//        Url_Master_Volunteers(dataRow.Email), dataRow.Email);

//      db.Add_Td_To_Tr(htmlRow, stateName + "&nbsp;", "tdReportDetail");
//      db.Add_Td_To_Tr(htmlRow, partyName + "&nbsp;", "tdReportDetail");
//      db.Add_Td_To_Tr(htmlRow, volunteerAnchor, "tdReportDetail");
//      db.Add_Td_To_Tr(htmlRow, dataRow.Phone + "&nbsp;", "tdReportDetail");
//      db.Add_Td_To_Tr(htmlRow, dataRow.FirstName + "&nbsp;", "tdReportDetail");
//      db.Add_Td_To_Tr(htmlRow, dataRow.LastName + "&nbsp;", "tdReportDetail");
//      db.Add_Td_To_Tr(htmlRow, dataRow.Password + "&nbsp;", "tdReportDetail");
//    }

//    private void GenerateAllVolunteersReport()
//    {
//      var htmlTable = new HtmlTable();
//      htmlTable.Attributes["cellspacing"] = "0";
//      htmlTable.Attributes["border"] = "0";

//      CreateAllVolunteersReportHeading(htmlTable);

//      var dataTable = VolunteersView.GetAllDataSorted();
//      if (dataTable.Count == 0)
//      {
//      }
//      else
//        foreach (var dataRow in dataTable)
//          CreateAllVolunteersReportRow(htmlTable, dataRow);

//      ReportPlaceHolder.Controls.Add(htmlTable);
//    }

//    #endregion The All Volunteers report

//    #region Validation

//    private void CheckTextBoxes()
//    {
//      //db.Throw_Exception_TextBox_Html(TextBoxAddEmailAddress);
//      //db.Throw_Exception_TextBox_Html(TextBoxEmailAddress);
//      //db.Throw_Exception_TextBox_Html(TextBoxPhone);
//      //db.Throw_Exception_TextBox_Html(TextBoxFirstName);
//      //db.Throw_Exception_TextBox_Html(TextBoxLastName);
//      //db.Throw_Exception_TextBox_Html(TextBoxPassword);
//      //db.Throw_Exception_TextBox_Html(SubjectBefore);
//      //db.Throw_Exception_TextBox_Html(SubjectAfter);
//      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxAddEmailAddress);
//      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxEmailAddress);
//      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPhone);
//      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxFirstName);
//      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxLastName);
//      db.Throw_Exception_TextBox_Html_Or_Script(TextBoxPassword);
//      db.Throw_Exception_TextBox_Html_Or_Script(SubjectBefore);
//      db.Throw_Exception_TextBox_Html_Or_Script(SubjectAfter);

//      db.Throw_Exception_TextBox_Script(EmailBodyBefore);
//      db.Throw_Exception_TextBox_Script(EmailBodyAfter);
//    }

//    private void CheckDropDownLists()
//    {
//      if (RadioButtonListEmail.SelectedValue != "ThankYou")
//      {
//        if (DropDownState.SelectedIndex == 0)
//          throw new ApplicationException("You need to select a State.");
//        if (DropDownParty.SelectedIndex == 0)
//          throw new ApplicationException("You need to select a Party.");
//      }
//    }

//    private void CheckTextBoxesForEmpty()
//    {

//      if (SubjectBefore.Text.Trim() == string.Empty)
//        throw new ApplicationException("Subject Before: text box is empty.");
//      if (EmailBodyBefore.Text.Trim() == string.Empty)
//        throw new ApplicationException("Body Before: text box is empty.");
//    }

//    private void CheckEmailTypeSelected()
//    {
//      if (RadioButtonListEmail.SelectedIndex == -1)
//        throw new ApplicationException("You need to select a radio button of the email to send.");
//    }

//    private void ValidateEmailAddress(string newEmail)
//    {
//      if (!Validation.IsValidEmailAddress(newEmail))
//        throw new ApplicationException("The email address is not valid.");
//      if (PartiesEmails.PartyEmailExists(newEmail))
//        throw new ApplicationException("This email address is already in use by a volunteer or party contact.");
//    }

//    #endregion

//    #region Control state

//    private void PopulateStateDropDown(string selectedStateCode)
//    {
//      StateCache.Populate(DropDownState, "-- Select a state --", string.Empty,
//        selectedStateCode);
//    }

//    public void PopulatePartyDropDown(string stateCode, string selectedPartyKey)
//    {
//      DropDownParty.Items.Clear();
//      PopulatePartyDropDownHelper("-- Select a party --", string.Empty, selectedPartyKey);

//      if (stateCode != string.Empty)
//      {
//        var parties = Parties.GetDataByStateCode(stateCode)
//          .Select(row => new { partyKey = row.PartyKey, partyName = row.PartyName })
//          .OrderBy(party => party.partyName);
//        foreach (var party in parties)
//          PopulatePartyDropDownHelper(party.partyName, party.partyKey,
//            selectedPartyKey);
//        DropDownParty.Enabled = true;
//      }
//      else
//        DropDownParty.Enabled = false;
//    }

//    private void PopulatePartyDropDownHelper(string text, string value, string selectedValue)
//    {
//      var li = new ListItem(text, value);
//      if (value == selectedValue)
//        li.Selected = true;
//      DropDownParty.Items.Add(li);
//    }

//    private void EnableVolunteerEditing(bool enabled)
//    {
//      ButtonDeleteVolunteer.Enabled = enabled;
//      TextBoxEmailAddress.Enabled = enabled;
//      TextBoxPhone.Enabled = enabled;
//      TextBoxFirstName.Enabled = enabled;
//      TextBoxLastName.Enabled = enabled;
//      TextBoxPassword.Enabled = enabled;
//      DropDownState.Enabled = enabled;
//      DropDownParty.Enabled = enabled;
//    }

//    private void LoadVolunteerData(string email)
//    {
//      var loaded = false;
//      if (!string.IsNullOrWhiteSpace(email))
//      {
//        var table = VolunteersView.GetData(email);
//        if (table.Count == 1)
//        {
//          var row = table[0];
//          var emailLabelText = " at " + row.Email;
//          LabelEditEmailAddress.Text = emailLabelText;
//          LabelDeleteEmailAddress.Text = emailLabelText;
//          ButtonDeleteVolunteer.OnClientClick = string.Format(
//            "return confirm('Are you sure you want to delete the volunteer at {0}?');",
//            row.Email);
//          TextBoxEmailAddress.Text = row.Email;
//          TextBoxPhone.Text = row.Phone;
//          TextBoxFirstName.Text = row.FirstName;
//          TextBoxLastName.Text = row.LastName;
//          TextBoxPassword.Text = row.Password;
//          var stateCode = row.StateCode;
//          if (string.IsNullOrWhiteSpace(stateCode))
//            stateCode = DropDownState.SelectedValue;
//          PopulateStateDropDown(stateCode);
//          EnableVolunteerEditing(true);
//          PopulatePartyDropDown(stateCode, row.PartyKey);
//          loaded = true;
//        }
//      }

//      if (!loaded) ClearVolunteerData();
//    }

//    private void ClearVolunteerData()
//    {
//      LabelEditEmailAddress.Text = string.Empty;
//      LabelDeleteEmailAddress.Text = string.Empty;
//      ButtonDeleteVolunteer.OnClientClick = string.Empty;
//      TextBoxEmailAddress.Text = string.Empty;
//      TextBoxPhone.Text = string.Empty;
//      TextBoxFirstName.Text = string.Empty;
//      TextBoxLastName.Text = string.Empty;
//      TextBoxPassword.Text = string.Empty;
//      PopulateStateDropDown(string.Empty);
//      EnableVolunteerEditing(false);
//      PopulatePartyDropDown(string.Empty, string.Empty);
//    }

//    private string Substitutions(string template)
//    {
//      return
//        new Substitutions("[[username]]", TextBoxEmailAddress.Text, "[[password]]",
//          TextBoxPassword.Text, "[[fname]]", TextBoxFirstName.Text, "[[lname]]",
//          TextBoxLastName.Text, "[[phone]]", TextBoxPhone.Text)
//          {
//            StateCode = DropDownState.SelectedValue,
//            PartyKey = DropDownParty.SelectedValue
//          }.Substitute(template);
//    }

//    //private string Substitutions(string strBefore)
//    //{
//    //  string StrAfter = strBefore;

//    //  //[[Date]]
//    //  StrAfter = db.Subsitutions_For_Date(
//    //    StrAfter
//    //    );
//    //  //##Vote-USA.org##
//    //  StrAfter = db.Subsitutions_For_Anchor_Vote_USA_org(
//    //    StrAfter
//    //    );
//    //  // ##Vote-XX.org##
//    //  StrAfter = db.Subsitutions_For_Vote_XX_org_Anchor(
//    //    DropDownState.SelectedValue
//    //    , StrAfter
//    //    );
//    //  // ##Vote-XX.org/Party##
//    //  StrAfter = db.Subsitutions_For_Party_Anchor(
//    //    DropDownState.SelectedValue
//    //    , StrAfter
//    //    );

//    //  //##video-scrape-bios##
//    //  StrAfter = db.Substitutions_For_Anchor(
//    //    StrAfter
//    //    ,"video-scrape-bios"
//    //    , @"Vote-USA.org/Video.aspx?video=introBio"
//    //    , "Scraping Candidates’ Websites for Biographical Information, Website Address, and Social Media Links"
//    //    );


//    //  //##video-capture-pic##

//    //  //##video-scrape-views##


//    //  //@@ron.kahlow@vote-usa.org@@
//    //  StrAfter = db.Substitutions_For_Email_Ron_Kahlow(
//    //    StrAfter
//    //    );
//    //  //@@info@vote-usa.org@@
//    //  StrAfter = db.Subsitutions_For_Email_Vote_USA_Info(
//    //     StrAfter
//    //    );
//    //  //@@mgr@vote-usa.org@@
//    //  StrAfter = db.Subsitutions_For_Email_Vote_USA_Mgr(
//    //     StrAfter
//    //    );
//    //  //@@electionmgr@vote-usa.org@@
//    //  StrAfter = db.Subsitutions_For_Email_Vote_USA_Election_Mgr(
//    //     StrAfter
//    //    );
//    //  //[[UserName]]
//    //  StrAfter = db.Substitutions_For(StrAfter
//    //    , "UserName"
//    //    , TextBoxEmailAddress.Text
//    //    );
//    //  //[[Password]]
//    //  StrAfter = db.Substitutions_For(StrAfter
//    //    , "Password"
//    //    , TextBoxPassword.Text
//    //    );
//    //  //[[FName]]
//    //  StrAfter = db.Substitutions_For(StrAfter
//    //    , "FName"
//    //    , TextBoxFirstName.Text
//    //    );
//    //  //[[LName]]
//    //  StrAfter = db.Substitutions_For(StrAfter
//    //    , "LName"
//    //    , TextBoxLastName.Text
//    //    );
//    //  //[[Phone]]
//    //  StrAfter = db.Substitutions_For(StrAfter
//    //    , "Phone"
//    //    , TextBoxPhone.Text
//    //    );

//    //  //[[StateCode]] 
//    //  StrAfter = db.Subsitutions_For_StateCode(
//    //    DropDownState.SelectedValue
//    //    , StrAfter
//    //    );

//    //  //[[State]] 
//    //  StrAfter = db.Subsitutions_For_State(
//    //    DropDownState.SelectedValue
//    //    , StrAfter
//    //    );

//    //  string partyKey = DropDownParty.SelectedValue;

//    //  //[[PartyKey]] 
//    //  StrAfter = db.Subsitutions_For_PartyKey(
//    //    partyKey
//    //    , StrAfter
//    //    );

//    //  //[[PartyCode]] 
//    //  StrAfter = db.Subsitutions_For_PartyCode(
//    //    partyKey
//    //    , StrAfter
//    //    );

//    //  //[[Party]] 
//    //  StrAfter = db.Subsitutions_For_Party(
//    //    partyKey
//    //    , StrAfter
//    //    );

//    //  return StrAfter;
//    //}

//    private void Subject_Body_Substitutions()
//    {
//      //Subject:
//      SubjectAfter.Text = Substitutions(
//        SubjectBefore.Text);
//      //Body:
//      EmailBodyAfter.Text = Substitutions(
//        EmailBodyBefore.Text);

//      //Check Substitutions
//      Vote.Substitutions.CheckForIncompleteSubstitutions(SubjectAfter.Text);
//      Vote.Substitutions.CheckForIncompleteSubstitutions(EmailBodyAfter.Text);
//    }

//    protected void Update_Templates_In_Database()
//    {
//      switch (RadioButtonListEmail.SelectedValue)
//      {
//        case "Custom":
//          break;
//        case "ThankYou":
//          Master_Update_Str("EmailsSubjectThankYou"
//            , SubjectBefore.Text.Trim());
//          Master_Update_Str("EmailsBodyThankYou"
//            , EmailBodyBefore.Text.Trim());
//          break;
//        case "YourLogin":
//          Master_Update_Str("EmailsSubjectYourLogin"
//            , SubjectBefore.Text.Trim());
//          Master_Update_Str("EmailsBodyYourLogin"
//            , EmailBodyBefore.Text.Trim());
//          break;
//      }
//    }

//    #endregion Control state

//    #region Event handlers

//    protected void ButtonAddVolunteer_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        var newEmail = db.Str_Remove_MailTo(TextBoxAddEmailAddress.Text.Trim());

//        CheckTextBoxes();
//        ValidateEmailAddress(newEmail);

//        VolunteersView.Insert(newEmail, db.MakeUniquePassword(), string.Empty,
//          string.Empty, string.Empty, string.Empty);

//        DB.VoteLog.LogAdminData.Insert(DateTime.Now, UserSecurityClass,
//          UserName, "PartyEmail", string.Empty, newEmail);

//        ViewState["EmailAddress"] = newEmail;

//        TextBoxAddEmailAddress.Text = string.Empty;
//        DropDownState.SelectedIndex = 0;
//        DropDownParty.SelectedIndex = 0;

//        LoadVolunteerData(newEmail);
//        ErrorMessage.Text = db.Ok("The new volunteer email address has been added.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void ButtonDeleteVolunteer_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        var email = ViewState["EmailAddress"].ToString();

//        VolunteersView.DeleteByEmail(email);
//        ViewState["EmailAddress"] = string.Empty;
//        ClearVolunteerData();
//        ErrorMessage.Text = db.Ok("The volunteer has been deleted.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void DropDownState_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        var email = ViewState["EmailAddress"].ToString();
//        var newStateCode = DropDownState.SelectedValue;

//        if (string.IsNullOrWhiteSpace(newStateCode))
//          throw new ApplicationException("Please select a state for the volunteer");

//        VolunteersView.UpdatePartyKey(string.Empty, email);
//        LoadVolunteerData(email);
//        ErrorMessage.Text = db.Ok("Party key was cleared.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void DropDownParty_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        var email = ViewState["EmailAddress"].ToString();
//        var newPartyKey = DropDownParty.SelectedValue;

//        if (string.IsNullOrWhiteSpace(newPartyKey))
//          throw new ApplicationException("Please select a party for the volunteer");

//        VolunteersView.UpdatePartyKey(newPartyKey, email);
//        LoadVolunteerData(email);
//        ErrorMessage.Text = db.Ok("Party was updated.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void TextBoxEmailAddress_TextChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        var oldEmail = ViewState["EmailAddress"].ToString();
//        var newEmail = db.Str_Remove_MailTo(TextBoxEmailAddress.Text.Trim());

//        CheckTextBoxes();
//        ValidateEmailAddress(newEmail);

//        VolunteersView.UpdateEmail(newEmail, oldEmail);
//        ViewState["EmailAddress"] = newEmail;
//        LoadVolunteerData(newEmail);
//        ErrorMessage.Text = db.Ok("Email address was updated.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void TextBoxFirstName_TextChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        var email = ViewState["EmailAddress"].ToString();
//        //string newFirstName = TextBoxFirstName.Text.Trim();
//        var newFirstName = Validation.FixGivenName(
//          TextBoxFirstName.Text);

//        CheckTextBoxes();

//        VolunteersView.UpdateFirstName(newFirstName, email);
//        LoadVolunteerData(TextBoxEmailAddress.Text.Trim());
//        ErrorMessage.Text = db.Ok("First name was updated.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void TextBoxLastName_TextChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        var email = ViewState["EmailAddress"].ToString();
//        //string newLastName = TextBoxLastName.Text.Trim();
//        var newLastName = Validation.FixGivenName(
//          TextBoxLastName.Text);

//        CheckTextBoxes();

//        VolunteersView.UpdateLastName(newLastName, email);
//        LoadVolunteerData(TextBoxEmailAddress.Text.Trim());
//        ErrorMessage.Text = db.Ok("Last name was updated.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void TextBoxPhone_TextChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        var email = ViewState["EmailAddress"].ToString();
//        var newPhone = TextBoxPhone.Text.Trim();

//        CheckTextBoxes();

//        VolunteersView.UpdatePhone(newPhone, email);
//        LoadVolunteerData(email);
//        ErrorMessage.Text = db.Ok("Phone was updated.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void TextBoxPassword_TextChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        var email = ViewState["EmailAddress"].ToString();
//        var newPassword = TextBoxPassword.Text.Trim();

//        CheckTextBoxes();
//        if (newPassword.Length == 0)
//          throw new ApplicationException("The password is required.");

//        VolunteersView.UpdatePassword(newPassword, email);
//        LoadVolunteerData(email);
//        ErrorMessage.Text = db.Ok("Password was updated.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void RadioButtonListEmail_SelectedIndexChanged(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckTextBoxes();
//        CheckDropDownLists();

//        EnableVolunteerEditing(true);

//        #region Load Templates
//        switch (RadioButtonListEmail.SelectedValue)
//        {
//          case "Custom":
//            break;
//          case "ThankYou":
//            SubjectBefore.Text = db.Master_Str("EmailsSubjectThankYou");
//            EmailBodyBefore.Text = db.Master_Str("EmailsBodyThankYou");
//            break;
//          case "YourLogin":
//            SubjectBefore.Text = db.Master_Str("EmailsSubjectYourLogin");
//            EmailBodyBefore.Text = db.Master_Str("EmailsBodyYourLogin");
//            break;
//        }
//        #endregion Load Templates

//        Subject_Body_Substitutions();

//        switch (RadioButtonListEmail.SelectedValue)
//        {
//          case "Custom":
//            ErrorMessage.Text = db.Msg("Enter a Subject and Body");
//            break;
//          case "ThankYou":
//            ErrorMessage.Text = db.Msg("The 'Thank You' Template has been loaded in the textboxes.");
//            break;
//          case "YourLogin":
//            ErrorMessage.Text = db.Msg("The 'Your Login' Template has been loaded in the textboxes.");
//            break;
//        }

//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void Button_Save_Changes_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        if (RadioButtonListEmail.SelectedValue != "Custom")
//        {
//          CheckTextBoxes();
//          CheckTextBoxesForEmpty();
//          CheckEmailTypeSelected();

//          Subject_Body_Substitutions();

//          Update_Templates_In_Database();

//          ErrorMessage.Text = db.Ok("The Subject and Body templates for this email type have been saved.");
//        }
//        else
//        {
//          ErrorMessage.Text = db.Fail("Custom emails can't be saved. Any changes need to be made manually to the custom text file.");
//        }
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }

//    }

//    protected void Button_View_Substitutions_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckTextBoxes();
//        CheckTextBoxesForEmpty();
//        CheckDropDownLists();
//        CheckEmailTypeSelected();

//        Subject_Body_Substitutions();


//        ErrorMessage.Text = db.Ok("The Subject and Body substitutions have been made.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    protected void Button_Test_Email_Click1(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckTextBoxes();
//        CheckTextBoxesForEmpty();
//        CheckDropDownLists();
//        CheckEmailTypeSelected();

//        Subject_Body_Substitutions();

//        var email = new MailMessage
//        {
//          IsBodyHtml = true,
//          From = RadioButtonList_From.SelectedValue == "Mgr"
//            ? new MailAddress("mgr@vote-usa.org")
//            : new MailAddress("ron.kahlow@vote-usa.org")
//        };

//        #region From

//        #endregion From

//        #region cc
//        email.CC.Clear();
//        if (RadioButtonList_Cc.SelectedValue
//          == "RonKahlow")
//          email.CC.Add("ron.kahlow@vote-usa.org");
//        #endregion cc

//        #region To
//        email.To.Clear();
//        email.To.Add("ron.kahlow@businessol.com");
//        #endregion To

//        email.Subject = SubjectAfter.Text;
//        email.Body = EmailBodyAfter.Text.ReplaceNewLinesWithBreakTags();

//#if !NoEmail
//        //var smtpClient = new SmtpClient("localhost");
//        //smtpClient.Send(email);
//        EmailUtility.GetConfiguredSmtpClient().Send(email);
//#endif

//        ErrorMessage.Text = db.Ok("A test email with a sample Subject and Body"
//        + " has been sent To: ron.kahlow@vote-usa.org"
//        + " From: ron.kahlow@businessol.com");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }

//    }

//    protected void ButtonSendEmail_Click1(object sender, EventArgs e)
//    {
//      try
//      {
//        CheckTextBoxes();
//        CheckTextBoxesForEmpty();
//        CheckDropDownLists();
//        CheckEmailTypeSelected();

//        Subject_Body_Substitutions();

//        var email = new MailMessage
//        {
//          IsBodyHtml = true,
//          From = RadioButtonList_From.SelectedValue
//            == "Mgr"
//            ? new MailAddress("mgr@vote-usa.org")
//            : new MailAddress("ron.kahlow@vote-usa.org")
//        };

//        #region From

//        #endregion From

//        #region cc
//        email.CC.Clear();
//        if (RadioButtonList_Cc.SelectedValue
//          == "RonKahlow")
//          email.CC.Add("ron.kahlow@vote-usa.org");
//        #endregion cc

//        #region To
//        email.To.Clear();
//        email.To.Add(TextBoxEmailAddress.Text);
//        #endregion To

//        email.Subject = SubjectAfter.Text;
//        email.Body = EmailBodyAfter.Text.ReplaceNewLinesWithBreakTags();

//#if !NoEmail
//        //var smtpClient = new SmtpClient("localhost");
//        //smtpClient.Send(email);
//        EmailUtility.GetConfiguredSmtpClient().Send(email);
//#endif

//        ErrorMessage.Text = db.Ok("The email was sent to the volunteer.");
//      }
//      catch (Exception ex)
//      {
//        #region
//        ErrorMessage.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);
//        #endregion
//      }
//      finally
//      {
//        GenerateAllVolunteersReport();
//      }
//    }

//    private void Page_Load(object sender, EventArgs e)
//    {
//      Response.Redirect("/master/updatevolunteers.aspx");
//      if (!IsPostBack)
//      {
//        Page.Title = "Volunteers";
//        if (!IsMasterUser)
//          HandleSecurityException();

//        try
//        {
//          var email = GetQueryString("Email") ?? string.Empty;
//          ViewState["EmailAddress"] = email.Trim();

//          LoadVolunteerData(email);
//          GenerateAllVolunteersReport();
//        }
//        catch (Exception ex)
//        {
//          ErrorMessage.Text = db.Fail(ex.Message);
//          db.Log_Error_Admin(ex);
//        }
//      }
//    }

//    #endregion Event handlers
  }
}