using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Globalization;
//using System.Net.Mail;
//using DB;
//using DB.Vote;
//using DB.VoteLog;
//using DB.VoteTemp;

namespace Vote.Master
{
  public partial class EmailCandidates : VotePage
  {
//    private void Clear_Textboxes()
//    {
//      TextBox_Attachments.Text = string.Empty;
//      SubjectBefore.Text = string.Empty;
//      SubjectAfter.Text = string.Empty;
//      EmailBodyBefore.Text = string.Empty;
//      EmailBodyAfter.Text = string.Empty;
//    }

//    private bool Is_Custom_Email()
//    {
//      if (!string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))

//        #region Election Emails

//        switch (RadioButtonList_Election.SelectedValue)
//        {
//          case "ElectionCustomStates":

//            #region Custom Text File to State Contacts for a Specific Election

//            return true;

//            #endregion Custom Text File to State Contacts for a Specific Election

//          case "ElectionCustomCandidates":

//            #region Custom Text File to ALL Candidates in a Specific Election

//            return true;

//            #endregion Custom Text File to ALL Candidates in a Specific Election

//          case "ElectionCustomParties":

//            #region Custom Text File to ALL Political Parties in State for a Specific Election

//            return true;

//            #endregion Custom Text File to ALL Political Parties in State for a Specific Election

//          case "ElectionCompletion":

//            #region Notify State of Election Completion

//            return false;

//            #endregion Notify State of Election Completion

//          case "ElectionCandidatesLogin":

//            #region Provide Candidates with Login for a Specific Election

//            return false;

//            #endregion Provide Candidates with Login for a Specific Election

//          case "ElectionPartiesLogin":

//            #region Provide State Parties with Login for a Specific Election

//            return false;

//            #endregion Provide State Parties with Login for a Specific Election

//          default:
//            return false;
//        }

//      #endregion Election Emails

//      if (!string.IsNullOrEmpty(Session["UserStateCode"].ToString()))

//        #region Single State Emails

//        switch (RadioButtonList_State.SelectedValue)
//        {
//          case "StateCustom":

//            #region Custom Text File to State Contacts

//            return true;

//            #endregion Custom Text File to State Contacts

//          case "StateCustomCandidates":

//            #region Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in State

//            return true;

//            #endregion Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in State

//          case "StateCustomParties":

//            #region Custom Text File to ALL Political Parties in State

//            return true;

//            #endregion Custom Text File to ALL Political Parties in State

//          case "StatePrimaryRosters":

//            #region Request Primary Election Roster from State

//            return false;

//            #endregion Request Primary Election Roster from State

//          case "StateGeneralRosters":

//            #region Request General Election Roster from State

//            return false;

//            #endregion Request General Election Roster from State

//          case "StateCandidates":

//            #region Candidates in Next Viewable Upcoming Elections in State

//            return false;

//            #endregion Candidates in Next Viewable Upcoming Elections in State

//          default:
//            return false;
//        }

//      #endregion Single State Emails

//      #region All States Emails

//      switch (RadioButtonList_All.SelectedValue)
//      {
//        case "AllCustomStates":

//          #region Custom Text File to ALL States Contacts

//          return true;

//          #endregion Custom Text File to ALL States Contacts

//        case "AllCustomCandidates":

//          #region Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in All States

//          return true;

//          #endregion Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in All States

//        case "AllCustomParties":

//          #region Custom Text File to ALL Political Parties in ALL States

//          return true;

//          #endregion Custom Text File to ALL Political Parties in ALL States

//        case "AllPrimaryRosters":

//          #region Request Primary Election Roster from ALL States

//          return false;

//          #endregion Request Primary Election Roster from ALL States

//        case "AllGeneralRosters":

//          #region Request General Election Roster from ALL States

//          return false;

//          #endregion Request General Election Roster from ALL States

//        case "AllCandidates":

//          #region All Candidates in Next Viewable Upcoming Elections in All States

//          return false;

//          #endregion All Candidates in Next Viewable Upcoming Elections in All States

//        default:
//          return false;
//      }

//      #endregion All States Emails
//    }

//    private static string Substitutions(string template, DataRow tempEmailRow)
//    {
//      return
//        new Substitutions("[[contact]]", tempEmailRow.Contact())
//        {
//          StateCode = tempEmailRow.StateCode(),
//          PoliticianKey = tempEmailRow.PoliticianKey(),
//          ElectionKey = tempEmailRow.ElectionKey(),
//          OfficeKey = tempEmailRow.OfficeKey(),
//          IssueKey = "ALLBio",
//          PartyKey = tempEmailRow.PartyKey(),
//          PartyEmail = tempEmailRow.Email()
//        }.Substitute(template);
//    }

//    private void Update_Templates_In_Database()
//    {
//      if (!string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))

//        #region Election Emails

//        switch (RadioButtonList_Election.SelectedValue)
//        {
//          case "ElectionCustomStates":

//            #region Custom Text File to State Contacts for a Specific Election

//            #endregion Custom Text File to State Contacts for a Specific Election

//            break;
//          case "ElectionCustomCandidates":

//            #region Custom Text File to ALL Candidates in a Specific Election

//            #endregion Custom Text File to ALL Candidates in a Specific Election

//            break;
//          case "ElectionCustomParties":

//            #region Custom Text File to ALL Political Parties in State for a Specific Election

//            #endregion Custom Text File to ALL Political Parties in State for a Specific Election

//            break;
//          case "ElectionCompletion":

//            #region Notify State of Election Completion

//            db.Master_Update_Str("EmailsSubjectElectionCompletion",
//              SubjectBefore.Text.Trim());
//            db.Master_Update_Str("EmailsBodyElectionCompletion",
//              EmailBodyBefore.Text.Trim());

//            #endregion Notify State of Election Completion

//            break;
//          case "ElectionCandidatesLogin":

//            #region Provide Candidates with Login for a Specific Election

//            db.Master_Update_Str("EmailsSubjectCandidatesLogin",
//              SubjectBefore.Text.Trim());
//            db.Master_Update_Str("EmailsBodyCandidatesLogin",
//              EmailBodyBefore.Text.Trim());

//            #endregion Provide Candidates with Login for a Specific Election

//            break;
//          case "ElectionPartiesLogin":

//            #region Provide State Parties with Login for a Specific Election

//            db.Master_Update_Str("EmailsSubjectPartiesLogin",
//              SubjectBefore.Text.Trim());
//            db.Master_Update_Str("EmailsBodyPartiesLogin",
//              EmailBodyBefore.Text.Trim());

//            #endregion Provide State Parties with Login for a Specific Election

//            break;
//        }
//        #endregion Election Emails

//      else if (!string.IsNullOrEmpty(Session["UserStateCode"].ToString()))

//        #region Single State Emails

//        switch (RadioButtonList_State.SelectedValue)
//        {
//          case "StateCustom":

//            #region Custom Text File to State Contacts

//            #endregion Custom Text File to State Contacts

//            break;
//          case "StateCustomCandidates":

//            #region Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in State

//            #endregion Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in State

//            break;
//          case "StateCustomParties":

//            #region Custom Text File to ALL Political Parties in State

//            #endregion Custom Text File to ALL Political Parties in State

//            break;
//          case "StatePrimaryRosters":

//            #region Request Primary Election Roster from State

//            db.Master_Update_Str("EmailsSubjectElectionRosterPrimary",
//              SubjectBefore.Text.Trim());
//            db.Master_Update_Str("EmailsBodyElectionRosterPrimary",
//              EmailBodyBefore.Text.Trim());

//            #endregion Request Primary Election Roster from State

//            break;
//          case "StateGeneralRosters":

//            #region Request General Election Roster from State

//            db.Master_Update_Str("EmailsSubjectElectionRosterGeneral",
//              SubjectBefore.Text.Trim());
//            db.Master_Update_Str("EmailsBodyElectionRosterGeneral",
//              EmailBodyBefore.Text.Trim());

//            #endregion Request General Election Roster from State

//            break;
//          case "StateCandidates":

//            #region Candidates in Next Viewable Upcoming Elections in State

//            db.Master_Update_Str("EmailsSubjectStateCandidates",
//              SubjectBefore.Text.Trim());
//            db.Master_Update_Str("EmailsBodyStateCandidates",
//              EmailBodyBefore.Text.Trim());

//            #endregion Candidates in Next Viewable Upcoming Elections in State

//            break;
//        }
//        #endregion Single State Emails

//      else
//        #region All States Emails

//        switch (RadioButtonList_All.SelectedValue)
//        {
//          case "AllCustomStates":

//            #region Custom Text File to ALL States Contacts

//            #endregion Custom Text File to ALL States Contacts

//            break;
//          case "AllCustomCandidates":

//            #region Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in All States

//            #endregion Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in All States

//            break;
//          case "AllCustomParties":

//            #region Custom Text File to ALL Political Parties in ALL States

//            #endregion Custom Text File to ALL Political Parties in ALL States

//            break;
//          case "AllPrimaryRosters":

//            #region Request Primary Election Roster from ALL States

//            db.Master_Update_Str("EmailsSubjectAllPrimaryRosters",
//              SubjectBefore.Text.Trim());
//            db.Master_Update_Str("EmailsBodyAllPrimaryRosters",
//              EmailBodyBefore.Text.Trim());

//            #endregion Request Primary Election Roster from ALL States

//            break;
//          case "AllGeneralRosters":

//            #region Request General Election Roster from ALL States

//            db.Master_Update_Str("EmailsSubjectAllRosters",
//              SubjectBefore.Text.Trim());
//            db.Master_Update_Str("EmailsBodyAllRosters",
//              EmailBodyBefore.Text.Trim());

//            #endregion Request General Election Roster from ALL States

//            break;
//          case "AllCandidates":

//            #region All Candidates in Next Viewable Upcoming Elections in All States

//            db.Master_Update_Str("EmailsSubjectAllCandidates",
//              SubjectBefore.Text.Trim());
//            db.Master_Update_Str("EmailsBodyAllCandidates",
//              EmailBodyBefore.Text.Trim());

//            #endregion All Candidates in Next Viewable Upcoming Elections in All States

//            break;
//        }

//      #endregion All States Emails
//    }

//    private void Substitutions_First_Email_Row_Example()
//    {
//      //DataRow EmailRow = db.Row_First_Optional(Sql_TempEmailAddresses());
//      var emailTable =
//        TempEmailAddresses.GetDataSortedByStateCodeElectionKeyPoliticianKey();
//      if (emailTable.Count > 0) //if (EmailRow != null)
//      {
//        #region Note: Columns in emailRow

//        //Contact- empty
//        //ElectionKey - present
//        //Email - present
//        //FirstName - optional
//        //LastName - Optional
//        //OfficeKey - empty
//        //PartyKey - present
//        //StateCode - present
//        //Title - optional

//        #endregion Note: Columns in emailRow

//        var emailRow = emailTable[0];
//        //Subject:
//        SubjectAfter.Text = Substitutions(SubjectBefore.Text, emailRow);
//        //Body:
//        EmailBodyAfter.Text = Substitutions(EmailBodyBefore.Text, emailRow);
//      }
//    }

//    #region Checks

//    private void Check_Illegal_Input_In_Textboxes()
//    {
//      db.Throw_Exception_TextBox_Html_Or_Script(SubjectBefore);
//      db.Throw_Exception_TextBox_Script(SubjectAfter);
//      db.Throw_Exception_TextBox_Html_Or_Script(TextBox_Attachments);
//      db.Throw_Exception_TextBox_Html_Or_Script(EmailBodyBefore);
//      db.Throw_Exception_TextBox_Script(EmailBodyAfter);
//    }

//    private void Check_Sustitutions()
//    {
//      Vote.Substitutions.CheckForIncompleteSubstitutions(SubjectAfter.Text);
//      Vote.Substitutions.CheckForIncompleteSubstitutions(EmailBodyAfter.Text);
//    }

//    private void Check_Email_Controls()
//    {
//      #region Textboxes

//      Check_Illegal_Input_In_Textboxes();

//      if (SubjectAfter.Text.Trim() == string.Empty) throw new ApplicationException("Subject After: text box is empty.");
//      if (EmailBodyAfter.Text.Trim() == string.Empty) throw new ApplicationException("Body After: text box is empty.");

//      #endregion Textboxes

//      #region Radio Buttons

//      if ((RadioButtonList_Election.SelectedValue == string.Empty) &&
//        (RadioButtonList_State.SelectedValue == string.Empty) &&
//        (RadioButtonList_All.SelectedValue == string.Empty))
//        throw new ApplicationException(
//          "One of the Email Recipients radio buttons needs to be checked");
//      if (RadioButtonList_From.SelectedIndex == -1) throw new ApplicationException("From: radio button is not selected.");
//      if (RadioButtonList_Cc.SelectedIndex == -1) throw new ApplicationException("Cc: radio button is not selected.");
//      if (RadioButtonList_MailFormat.SelectedIndex == -1) throw new ApplicationException("Mail Format: radio button is not selected.");
//      if (RadioButtonList_Attachments.SelectedIndex == -1) throw new ApplicationException("Attachments: radio button is not selected.");

//      #endregion Radio Buttons

//      Substitutions_First_Email_Row_Example();

//      Check_Sustitutions();
//    }

//    #endregion Checks

//    #region Attachments

//    private void Attachments(MailMessage email)
//    {
//      email.Attachments.Clear();

//      //Implement special attachements later
//      var imageList = string.Empty;
//      if (RadioButtonList_Attachments.SelectedValue == "SampleBallot") imageList = "Ballot-Button-Small.jpg,Ballot-Button-Large.jpg";

//      if (!string.IsNullOrEmpty(imageList))
//      {
//        var path = GetServerPath() + @"images\";
//        char[] delim = {','};
//        //foreach (string sSubstr in TextBox_Attachments.Text.Split(delim))
//        foreach (var sSubstr in imageList.Split(delim))
//        {
//          var image = @path + sSubstr;
//          var myAttachment = new Attachment(image);
//          email.Attachments.Add(myAttachment);
//        }
//      }
//    }

//    #endregion Attachments

//    protected void Show_Email_Addresses()
//    {
//      EmailAddresses.Text = string.Empty;

//      var emailAddressesTable =
//        TempEmailAddresses.GetDataSortedByStateCodeElectionKeyPoliticianKey();
//      foreach (var emailAddressesRow in emailAddressesTable)
//        EmailAddresses.Text += "<br>" + emailAddressesRow.Contact + " - " +
//          emailAddressesRow.Email;
//    }

//    protected void Append_Bad_Email_Addresses(List<string> badEmailAddresses)
//    {
//      if (badEmailAddresses.Count != 0)
//      {
//        var badEmailText = string.Join("<br />", badEmailAddresses);
//        EmailAddresses.Text +=
//          "<br /><br />Emails could not be sent to the following addresses:<br />" +
//            badEmailText;
//      }
//    }

//    #region Buttons

//    protected void Button_Reload_Template_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        if (!Is_Custom_Email())
//          if (!string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))

//            #region Election Emails

//            if (RadioButtonList_Election.SelectedIndex != -1)
//            {
//              Load_Templates_Election();
//              Msg.Text = db.Ok("The selected election template has been reloaded.");
//            }
//            else Msg.Text = db.Fail("No template has been selected to reload.");
//            #endregion Election Emails

//          else if (!string.IsNullOrEmpty(Session["UserStateCode"].ToString()))

//            #region Single State Emails

//            if (RadioButtonList_State.SelectedIndex != -1)
//            {
//              Load_Templates_State();
//              Msg.Text = db.Ok("The selected state template has been reloaded.");
//            }
//            else Msg.Text = db.Fail("No template has been selected to reload.");
//            #endregion Single State Emails

//          else
//            #region All States Emails

//            if (RadioButtonList_All.SelectedIndex != -1)
//            {
//              Load_Templates_All();
//              Msg.Text = db.Ok("The selected template has been reloaded.");
//            }
//            else Msg.Text = db.Fail("No template has been selected to reload.");
//          #endregion All States Emails

//        else Msg.Text = db.Fail("Custom emails can't be reloaded.");
//      }
//      catch (Exception ex)
//      {
//        #region

//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);

//        #endregion
//      }
//    }

//    protected void Button_Save_Changes_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        if (!Is_Custom_Email())
//        {
//          #region Checks

//          Check_Illegal_Input_In_Textboxes();

//          if (SubjectBefore.Text.Trim() == string.Empty) throw new ApplicationException("Subject Before: text box is empty.");
//          if (EmailBodyBefore.Text.Trim() == string.Empty) throw new ApplicationException("Body Before: text box is empty.");

//          Substitutions_First_Email_Row_Example();
//          Check_Sustitutions();

//          #endregion Checks

//          Update_Templates_In_Database();

//          Msg.Text =
//            db.Ok(
//              "The Subject and Body templates for this email type have been saved.");
//        }
//        else
//          Msg.Text =
//            db.Fail(
//              "Custom emails can't be saved. Any changes need to be made manually to the custom text file.");
//      }
//      catch (Exception ex)
//      {
//        #region

//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);

//        #endregion
//      }
//    }

//    protected void Button_View_Substitutions_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        #region checks

//        Check_Illegal_Input_In_Textboxes();

//        if (string.IsNullOrEmpty(SubjectBefore.Text.Trim())) throw new ApplicationException("The Subject Before textbox is empty.");

//        if (string.IsNullOrEmpty(EmailBodyBefore.Text.Trim())) throw new ApplicationException("The Body Before textbox is empty.");

//        #endregion checks

//        if (RadioButtonList_Election.Visible) //Build_TempAddresses_Set_Controls_Election();
//          Build_TempAddresses_Election();
//        else if (RadioButtonList_State.Visible) //Build_TempAddresses_Set_Controls_State();
//          Build_TempAddresses_State();
//        else if (RadioButtonList_All.Visible) //Build_TempAddresses_Set_Controls_All_States();
//          Build_TempAddresses_All_States();

//        Substitutions_First_Email_Row_Example();
//        Check_Sustitutions();

//        Show_Email_Addresses();

//        Msg_Email_Addresses_Built();
//      }
//      catch (Exception ex)
//      {
//        #region

//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);

//        #endregion
//      }
//    }

//    protected void Button_Test_First_Email_Click1(object sender, EventArgs e)
//    {
//      try
//      {
//        Check_Email_Controls();

//        var emailTable =
//          TempEmailAddresses.GetDataSortedByStateCodeElectionKeyPoliticianKey();
//        if (emailTable.Count <= 0) throw new ApplicationException("Unknown email problem.");

//        var email = new MailMessage
//        {
//          IsBodyHtml = RadioButtonList_MailFormat.SelectedValue == "Html",
//          Subject = SubjectAfter.Text,
//          Body = EmailBodyAfter.Text.ReplaceNewLinesWithBreakTags()
//        };
//        email.To.Add("ron.kahlow@businessol.com");

//        switch (RadioButtonList_From.SelectedValue)
//        {
//          case "RonKahlow":
//            email.From = new MailAddress("ron.kahlow@vote-usa.org");
//            break;

//          default:
//            email.From = new MailAddress("mgr@vote-usa.org");
//            break;
//        }

//        Attachments(email);

//#if !NoEmail
//        var smtpClient = new SmtpClient("localhost");
//        smtpClient.Send(email);
//#endif

//        Msg.Text =
//          db.Ok("A test email with a sample Subject and Body" +
//            " has been sent To: ron.kahlow@vote-usa.org" +
//            " From: ron.kahlow@businessol.com");
//      }
//      catch (Exception ex)
//      {
//        #region

//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);

//        #endregion
//      }
//    }

//    protected void Button_Emails_To_Ron_Click(object sender, EventArgs e)
//    {
//      try
//      {
//        Server.ScriptTimeout = 30000; //300 sec = 500 min = 8hr
//        SendEmails(true); //a test to ron.kahlow@vote-usa.org
//      }
//      catch (Exception ex)
//      {
//        #region

//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);

//        #endregion
//      }
//    }

//    public static DataTable Table_ElectionKeys_On_Same_Day(string electionKey)
//    {
//      var sql = "SELECT ElectionKey";
//      sql += " FROM Elections";
//      sql += " WHERE SUBSTRING(ElectionKey,1,10) = " +
//        db.SQLLit(electionKey.Substring(0, 10));
//      return db.Table(sql);
//    }

//    protected void SendEmails(bool isTest)
//    {
//      var badEmailAddresses = new List<string>();

//      #region checks

//      Check_Illegal_Input_In_Textboxes();
//      Check_Email_Controls();

//      #endregion checks

//      var email = new MailMessage();

//      #region From

//      switch (RadioButtonList_From.SelectedValue)
//      {
//        case "Mgr":
//          email.From = new MailAddress("mgr@vote-usa.org");
//          break;
//        case "RonKahlow":
//          email.From = new MailAddress("ron.kahlow@vote-usa.org");
//          break;
//        default:
//          email.From = new MailAddress("mgr@vote-usa.org");
//          break;
//      }

//      #endregion From

//      #region cc

//      email.CC.Clear();
//      switch (RadioButtonList_Cc.SelectedValue)
//      {
//        case "ElectionMgr":
//          email.CC.Add("electionmgr@vote-usa.org");
//          break;
//        case "Mgr":
//          email.CC.Add("mgr@vote-usa.org");
//          break;
//      }

//      #endregion cc

//      email.IsBodyHtml = RadioButtonList_MailFormat.SelectedValue == "Html";

//      var emailsTable =
//        TempEmailAddresses.GetDataSortedByStateCodeElectionKeyPoliticianKey();
//      foreach (var emailsRow in emailsTable)
//        try
//        {
//          #region Substitutions for Unique Subject and Body

//          email.Subject = Substitutions(SubjectBefore.Text, emailsRow);

//          email.Body = Substitutions(EmailBodyBefore.Text, emailsRow)
//            .ReplaceNewLinesWithBreakTags();

//          #endregion Substitutions for Unique Subject and Body

//          //to fix but of adding all email addresses
//          email.To.Clear();

//          email.To.Add(!isTest ? emailsRow.Email : "ron.kahlow@vote-usa.org");

//          //for testing
//          //email.From = new MailAddress(Row_Email["EmailAddress"].ToString());
//          //email.To.Add("ron.kahlow@Businssol.com");//for testing

//          Attachments(email);

//#if !NoEmail
//          var smtpClient = new SmtpClient("localhost");
//          smtpClient.Send(email);
//#endif

//          #region Log Email

//          LogEmails.Insert(DateTime.Now, emailsRow.StateCode, string.Empty,
//            string.Empty, emailsRow.ElectionKey, emailsRow.OfficeKey,
//            emailsRow.PoliticianKey, emailsRow.Contact, SecurePage.UserName,
//            email.From.Address, email.To.ToString(), email.CC.ToString(),
//            string.Empty, email.Subject, email.Body);

//          #endregion Log Email
//        }
//        catch (Exception ex)
//        {
//          db.Log_Error_Admin(ex, "EmailAddress:" + emailsRow.Email);
//          badEmailAddresses.Add(emailsRow.Email);
//        }

//      Show_Email_Addresses();
//      Append_Bad_Email_Addresses(badEmailAddresses);

//      #region uncommented moved -Record when emails sent and how many

//      if (!string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))

//        #region Election Emails

//        switch (RadioButtonList_Election.SelectedValue)
//        {
//          case "ElectionCustomStates":

//            #region Custom Text File to State Contacts for a Specific Election

//            #endregion Custom Text File to State Contacts for a Specific Election

//            break;
//          case "ElectionCustomCandidates":

//            #region Custom Text File to ALL Candidates in a Specific Election

//            #endregion Custom Text File to ALL Candidates in a Specific Election

//            break;
//          case "ElectionCustomParties":

//            #region Custom Text File to ALL Political Parties in State for a Specific Election

//            #endregion Custom Text File to ALL Political Parties in State for a Specific Election

//            break;
//          case "ElectionCompletion":

//            #region Notify State of Election Completion

//            var electionKeysTable =
//              Table_ElectionKeys_On_Same_Day(ViewState["ElectionKey"].ToString());
//            foreach (DataRow electionRow in electionKeysTable.Rows)
//            {
//              db.Elections_Update_Date(electionRow["ElectionKey"].ToString(),
//                "EmailsDateElectionCompletion", DateTime.Now);
//              db.Elections_Update_Int(electionRow["ElectionKey"].ToString(),
//                "EmailsSentElectionCompletion", emailsTable.Count);
//            }

//            #endregion Notify State of Election Completion

//            break;
//          case "ElectionCandidatesLogin":

//            #region Provide Candidates with Login for a Specific Election

//            db.Elections_Update_Date(ViewState["ElectionKey"].ToString(),
//              "EmailsDateCandidatesLogin", DateTime.Now);
//            db.Elections_Update_Int(ViewState["ElectionKey"].ToString(),
//              "EmailsSentCandidatesLogin", emailsTable.Count);

//            #endregion Provide Candidates with Login for a Specific Election

//            break;
//          case "ElectionPartiesLogin":

//            #region Provide State Parties with Login for a Specific Election

//            db.Elections_Update_Date(ViewState["ElectionKey"].ToString(),
//              "EmailsDatePartiesLogin", DateTime.Now);
//            db.Elections_Update_Int(ViewState["ElectionKey"].ToString(),
//              "EmailsSentPartiesLogin", emailsTable.Count);

//            #endregion Provide State Parties with Login for a Specific Election

//            break;
//        }
//        #endregion Election Emails

//      else if (!string.IsNullOrEmpty(Session["UserStateCode"].ToString()))

//        #region Single State Emails

//        switch (RadioButtonList_State.SelectedValue)
//        {
//          case "StateCustom":

//            #region Custom Text File to State Contacts

//            #endregion Custom Text File to State Contacts

//            break;
//          case "StateCustomCandidates":

//            #region Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in State

//            #endregion Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in State

//            break;
//          case "StateCustomParties":

//            #region Custom Text File to ALL Political Parties in State

//            #endregion Custom Text File to ALL Political Parties in State

//            break;
//          case "StatePrimaryRosters":

//            #region Request Primary Election Roster from State

//            db.States_Update_Date(Session["UserStateCode"].ToString()
//              //, "EmailsDatePrimaryInfoRequest"
//              , "EmailsDateElectionRosterPrimary", DateTime.Now);
//            db.States_Update_Int(Session["UserStateCode"].ToString()
//              //, "EmailsElectionKeyPrimaryInfoRequest"
//              , "EmailsSentElectionRosterPrimary"
//              //, ViewState["ElectionKey"].ToString()
//              , TempEmailAddresses.CountTable());

//            #endregion Request Primary Election Roster from State

//            break;
//          case "StateGeneralRosters":

//            #region Request General Election Roster from State

//            db.States_Update_Date(Session["UserStateCode"].ToString(),
//              "EmailsDateElectionRosterGeneral", DateTime.Now);
//            db.States_Update_Int(Session["UserStateCode"].ToString(),
//              "EmailsSentElectionRosterGeneral", TempEmailAddresses.CountTable());

//            #endregion Request General Election Roster from State

//            break;
//          case "StateCandidates":

//            #region Candidates in Next Viewable Upcoming Elections in State

//            #endregion Candidates in Next Viewable Upcoming Elections in State

//            break;
//        }
//        #endregion Single State Emails

//      else
//        #region All States Emails

//        switch (RadioButtonList_All.SelectedValue)
//        {
//          case "AllCustomStates":

//            #region Custom Text File to ALL States Contacts

//            #endregion Custom Text File to ALL States Contacts

//            break;
//          case "AllCustomCandidates":

//            #region Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in All States

//            #endregion Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in All States

//            break;
//          case "AllCustomParties":

//            #region Custom Text File to ALL Political Parties in ALL States

//            #endregion Custom Text File to ALL Political Parties in ALL States

//            break;
//          case "AllPrimaryRosters":

//            #region Request Primary Election Roster from ALL States

//            db.Master_Update_Date("EmailsDateAllPrimaryRosters", DateTime.Now);

//            db.Master_Update_Int("EmailsSentAllPrimaryRosters", emailsTable.Count);

//            #endregion Request Primary Election Roster from ALL States

//            break;
//          case "AllGeneralRosters":

//            #region Request General Election Roster from ALL States

//            db.Master_Update_Date("EmailsDateAllRosters", DateTime.Now);

//            db.Master_Update_Int("EmailsSentALLRosters", emailsTable.Count);

//            #endregion Request General Election Roster from ALL States

//            break;
//          case "AllCandidates":

//            #region All Candidates in Next Viewable Upcoming Elections in All States

//            db.Master_Update_Date("EmailsDateAllCandidates", DateTime.Now);

//            #endregion All Candidates in Next Viewable Upcoming Elections in All States

//            break;
//        }

//      #endregion All States Emails

//      #endregion commented moved - Record when emails sent and how many

//      Update_Templates_In_Database();

//      #region Msg

//      if (TempEmailAddresses.CountTable() > 0)
//        if (!isTest) Msg.Text = db.Ok(TempEmailAddresses.CountTable() + " emails were sent.");
//        else
//          Msg.Text =
//            db.Ok(TempEmailAddresses.CountTable() +
//              " emails were sent to ron.kahlow@vote-usa.org.");
//      else Msg.Text = db.Fail("NO EMAILS WERE SENT.");

//      #endregion Msg
//    }

//    protected void Button_Send_All_Emails_Click1(object sender, EventArgs e)
//    {
//      //List<string> badEmailAddresses = new List<string>();

//      try
//      {
//        Server.ScriptTimeout = 30000; //300 sec = 500 min = 8hr

//        SendEmails(false); //not a test
//      }
//      catch (Exception ex)
//      {
//        #region

//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);

//        #endregion
//      }
//    }

//    #endregion Buttons

//    #region Build Temp Table of Email Addresses

//    private static void Insert_TempEmailAddresses(string emailAddr, string contact,
//      string politicianKey, string officeKey, string electionKey, string stateCode,
//      string partyKey, string fName, string lName, string title)
//    {
//      if ((!string.IsNullOrEmpty(emailAddr)) && (!db.Is_Has_Http(emailAddr)) &&
//        (db.Is_Valid_Email_Address(emailAddr)))
//      {
//        emailAddr = db.Str_Remove_MailTo(emailAddr);

//        //Make sure email address does not already exist
//        //as may be case with presidential candidates
//        if (!TempEmailAddresses.EmailExists(emailAddr))
//          TempEmailAddresses.Insert(emailAddr, contact, politicianKey, officeKey,
//            electionKey, stateCode, partyKey, fName, lName, title, string.Empty,
//            string.Empty);
//      }
//    }

//    private static void Insert_One_Candidate_Email_Addresses_Temp_Table(
//      DataRow rowElectionsPoliticians)
//    {
//      var sql = string.Empty;
//      sql += " SELECT";
//      sql += " PoliticianKey";
//      sql += ",EmailAddrVoteUSA";
//      sql += ",EmailAddr";
//      sql += ",StateEmailAddr";
//      sql += ",CampaignEmail";
//      sql += ",LDSEmailAddr";
//      sql += " FROM Politicians";
//      sql += " WHERE PoliticianKey = " +
//        db.SQLLit(rowElectionsPoliticians["PoliticianKey"].ToString());
//      var rowPolitician = db.Row_Optional(sql);
//      if (rowPolitician != null)
//      {
//        #region 5 possible email addresses

//        Insert_TempEmailAddresses(rowPolitician["EmailAddrVoteUSA"].ToString()
//          .Trim(),
//          Politicians.GetFormattedName(rowPolitician["PoliticianKey"].ToString()),
//          rowPolitician["PoliticianKey"].ToString()
//            .Trim(), rowElectionsPoliticians["OfficeKey"].ToString()
//              .Trim(), rowElectionsPoliticians["ElectionKey"].ToString()
//                .Trim(), rowElectionsPoliticians["StateCode"].ToString()
//                  .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

//        Insert_TempEmailAddresses(rowPolitician["EmailAddr"].ToString()
//          .Trim(),
//          Politicians.GetFormattedName(rowPolitician["PoliticianKey"].ToString()),
//          rowPolitician["PoliticianKey"].ToString()
//            .Trim(), rowElectionsPoliticians["OfficeKey"].ToString()
//              .Trim(), rowElectionsPoliticians["ElectionKey"].ToString()
//                .Trim(), rowElectionsPoliticians["StateCode"].ToString()
//                  .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

//        Insert_TempEmailAddresses(rowPolitician["StateEmailAddr"].ToString()
//          .Trim(),
//          Politicians.GetFormattedName(rowPolitician["PoliticianKey"].ToString()),
//          rowPolitician["PoliticianKey"].ToString()
//            .Trim(), rowElectionsPoliticians["OfficeKey"].ToString()
//              .Trim(), rowElectionsPoliticians["ElectionKey"].ToString()
//                .Trim(), rowElectionsPoliticians["StateCode"].ToString()
//                  .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

//        Insert_TempEmailAddresses(rowPolitician["CampaignEmail"].ToString()
//          .Trim(),
//          Politicians.GetFormattedName(rowPolitician["PoliticianKey"].ToString()),
//          rowPolitician["PoliticianKey"].ToString()
//            .Trim(), rowElectionsPoliticians["OfficeKey"].ToString()
//              .Trim(), rowElectionsPoliticians["ElectionKey"].ToString()
//                .Trim(), rowElectionsPoliticians["StateCode"].ToString()
//                  .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

//        Insert_TempEmailAddresses(rowPolitician["LDSEmailAddr"].ToString()
//          .Trim(),
//          Politicians.GetFormattedName(rowPolitician["PoliticianKey"].ToString()),
//          rowPolitician["PoliticianKey"].ToString()
//            .Trim(), rowElectionsPoliticians["OfficeKey"].ToString()
//              .Trim(), rowElectionsPoliticians["ElectionKey"].ToString()
//                .Trim(), rowElectionsPoliticians["StateCode"].ToString()
//                  .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

//        #endregion 5 possible email addresses
//      }
//    }

//    private static void Insert_One_State_Email_Addresses_Temp_Table(DataRow rowState)
//    {
//      if (rowState != null)
//      {
//        #region 2 possible email addresses

//        Insert_TempEmailAddresses(rowState["ContactEmail"].ToString()
//          .Trim(), rowState["Contact"].ToString()
//            .Trim(), string.Empty, string.Empty
//          //, db.ElectionKey_Upcoming_General(Row_State["StateCode"].ToString())
//          , string.Empty, rowState["StateCode"].ToString()
//            .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

//        Insert_TempEmailAddresses(rowState["AltEMail"].ToString()
//          .Trim(), rowState["AltContact"].ToString()
//            .Trim(), string.Empty, string.Empty
//          //, db.ElectionKey_Upcoming_General(Row_State["StateCode"].ToString())
//          , string.Empty, rowState["StateCode"].ToString()
//            .Trim(), string.Empty, string.Empty, string.Empty, string.Empty);

//        #endregion 5 possible email addresses
//      }
//    }

//    protected void Temp_Email_Addresses_State_Contacts(string stateCode,
//      string electionKey)
//    {
//      #region Build a temporary tables of email addresses

//      //db.ExecuteSQL("TRUNCATE TABLE TempEmailAddresses");
//      TempEmailAddresses.TruncateTable();

//      if (!string.IsNullOrEmpty(db.States_Str(stateCode, "ContactEmail")))
//        Insert_TempEmailAddresses(db.States_Str(stateCode, "ContactEmail"),
//          db.States_Str(stateCode, "Contact"), string.Empty, string.Empty,
//          electionKey, stateCode, string.Empty, string.Empty, string.Empty,
//          string.Empty);

//      if (!string.IsNullOrEmpty(db.States_Str(stateCode, "AltEMail")))
//        Insert_TempEmailAddresses(db.States_Str(stateCode, "AltEMail"),
//          db.States_Str(stateCode, "AltContact"), string.Empty, string.Empty,
//          electionKey, stateCode, string.Empty, string.Empty, string.Empty,
//          string.Empty);

//      #endregion Build a temporary table of email addresses

//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void Temp_Email_Addresses_State_Contacts()
//    {
//      Temp_Email_Addresses_State_Contacts(Session["UserStateCode"].ToString(),
//        string.Empty);
//    }

//    protected void Temp_Email_Addresses_States_Contacts()
//    {
//      TempEmailAddresses.TruncateTable();

//      var sql = " SELECT";
//      sql += " StateCode";
//      sql += ",Contact";
//      sql += ",ContactEmail";
//      sql += ",AltContact";
//      sql += ",AltEMail";
//      sql += " FROM States";
//      sql += " ORDER BY StateCode";
//      var tableStates = db.Table(sql);
//      foreach (DataRow rowState in tableStates.Rows) Insert_One_State_Email_Addresses_Temp_Table(rowState);

//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void Temp_Email_Addresses_States_Contacts_Future_Election(
//      string electionType)
//    {
//      TempEmailAddresses.TruncateTable();

//      var sql = " SELECT";
//      sql += " StateCode";
//      sql += " FROM ElectionsFuture";
//      sql += " WHERE ElectionType= " + db.SQLLit(electionType);
//      sql += " ORDER BY StateCode";
//      var tableElectionsFuture = db.Table(sql);
//      foreach (DataRow rowElectionsFuture in tableElectionsFuture.Rows)
//      {
//        sql = " SELECT";
//        sql += " StateCode";
//        sql += ",Contact";
//        sql += ",ContactEmail";
//        sql += ",AltContact";
//        sql += ",AltEMail";
//        sql += " FROM States";
//        sql += " WHERE StateCode=" +
//          db.SQLLit(rowElectionsFuture["StateCode"].ToString());
//        var rowState = db.Row_First_Optional(sql);
//        if (rowState != null) Insert_One_State_Email_Addresses_Temp_Table(rowState);
//      }

//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void Temp_Email_Addresses_States_Contacts_Future_Primary_Election()
//    {
//      Temp_Email_Addresses_States_Contacts_Future_Election("P");
//    }

//    protected void Temp_Email_Addresses_States_Contacts_Future_General_Election()
//    {
//      Temp_Email_Addresses_States_Contacts_Future_Election("G");
//    }

//    protected void Temp_Email_Addresses_States_Contacts_Future_OffYear_Election()
//    {
//      Temp_Email_Addresses_States_Contacts_Future_Election("O");
//    }

//    protected void Temp_Email_Addresses_States_Contacts_Future_Special_Election()
//    {
//      Temp_Email_Addresses_States_Contacts_Future_Election("S");
//    }

//    protected void
//      Temp_Email_Addresses_States_Contacts_Future_PresidentialPrimary_Election()
//    {
//      Temp_Email_Addresses_States_Contacts_Future_Election("B");
//    }

//    protected void Temp_Email_Addresses_States_Contacts_NO_Future_Election(
//      string electionType)
//    {
//      TempEmailAddresses.TruncateTable();

//      var sql = " SELECT";
//      sql += " StateCode";
//      sql += ",Contact";
//      sql += ",ContactEmail";
//      sql += ",AltContact";
//      sql += ",AltEMail";
//      sql += " FROM States";
//      sql += " ORDER BY StateCode";
//      var tableStates = db.Table(sql);
//      foreach (DataRow rowState in tableStates.Rows)
//      {
//        sql = " SELECT";
//        sql += " StateCode";
//        sql += " FROM ElectionsFuture";
//        sql += " WHERE StateCode= " + db.SQLLit(rowState["StateCode"].ToString());
//        sql += " AND ElectionType= " + db.SQLLit(electionType);
//        var rowElectionsFuture = db.Row_First_Optional(sql);
//        if (rowElectionsFuture == null) Insert_One_State_Email_Addresses_Temp_Table(rowState);
//      }

//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void Temp_Email_Addresses_States_Contacts_NO_Future_Primary_Election()
//    {
//      Temp_Email_Addresses_States_Contacts_NO_Future_Election("P");
//    }

//    protected void Temp_Email_Addresses_States_Contacts_NO_Future_General_Election()
//    {
//      Temp_Email_Addresses_States_Contacts_NO_Future_Election("G");
//    }

//    protected void Temp_Email_Addresses_States_Contacts_NO_Future_OffYear_Election()
//    {
//      Temp_Email_Addresses_States_Contacts_NO_Future_Election("O");
//    }

//    protected void Temp_Email_Addresses_States_Contacts_NO_Future_Special_Election()
//    {
//      Temp_Email_Addresses_States_Contacts_NO_Future_Election("S");
//    }

//    protected void
//      Temp_Email_Addresses_States_Contacts_NO_Future_PresidentialPrimary_Election()
//    {
//      Temp_Email_Addresses_States_Contacts_NO_Future_Election("B");
//    }

//    protected void Temp_Email_Addresses_Election_General_Contacts_State()
//    {
//      var sql = string.Empty;
//      sql += " SELECT ";
//      sql += " ElectionKey";
//      sql += " FROM Elections ";
//      sql += " WHERE Elections.StateCode = " +
//        db.SQLLit(ViewState["StateCode"].ToString());
//      sql += " AND ElectionDate >= " + db.SQLLit(Db.DbToday);
//      sql += " AND ElectionType = 'G'";
//      sql += " AND CountyCode = ''";
//      sql += " AND LocalCode = ''";
//      sql += " ORDER BY ElectionDate ASC";
//      var electionRow = db.Row_First_Optional(sql);
//      if (electionRow != null)
//      {
//        Temp_Email_Addresses_State_Contacts(ViewState["StateCode"].ToString(),
//          electionRow["ElectionKey"].ToString());
//        Label_Election.Text =
//          db.Elections_ElectionDesc(electionRow["ElectionKey"].ToString());
//      }
//      else
//      {
//        TempEmailAddresses.TruncateTable();
//        Label_Emails_Sending.Text = "0";
//        Label_Election.Text = "No Upcoming General Election";
//      }

//      #region Report last time emails were sent using template

//      Label_Emails_Last_Sent_Date.Text =
//        db.States_Date(ViewState["StateCode"].ToString(),
//          "EmailsDateElectionRosterGeneral")
//          .ToString(CultureInfo.InvariantCulture);
//      Label_Election.Text = string.Empty;

//      #endregion Report last time emails were sent using template
//    }

//    protected void Temp_Email_Addresses_Election_General_Contacts_All_States()
//    {
//      var sql = string.Empty;
//      sql += " SELECT ";
//      sql += " StateCode";
//      sql += ",ElectionKey";
//      sql += " FROM Elections ";
//      sql += " WHERE ElectionDate >= " + db.SQLLit(Db.DbToday);
//      sql += " AND ElectionType = 'G'";
//      sql += " AND CountyCode = ''";
//      sql += " AND LocalCode = ''";
//      sql += " ORDER BY ElectionDate ASC";
//      var tableElection = db.Table(sql);
//      foreach (DataRow rowElection in tableElection.Rows)
//        Temp_Email_Addresses_State_Contacts(rowElection["StateCode"].ToString(),
//          rowElection["ElectionKey"].ToString());

//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void Temp_Email_Addresses_Election_Primary_Contacts_All_States()
//    {
//      TempEmailAddresses.TruncateTable();

//      var sql = string.Empty;
//      sql += " SELECT ";
//      sql += " StateCode";
//      sql += ",ElectionKey";
//      sql += " FROM Elections ";
//      sql += " WHERE ElectionDate >= " + db.SQLLit(Db.DbToday);
//      sql += " AND ElectionType = 'P'";
//      sql += " AND CountyCode = ''";
//      sql += " AND LocalCode = ''";
//      sql += " ORDER BY ElectionDate ASC";
//      var tableElection = db.Table(sql);
//      foreach (DataRow rowElection in tableElection.Rows)
//        Temp_Email_Addresses_State_Contacts(rowElection["StateCode"].ToString(),
//          rowElection["ElectionKey"].ToString());
//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void Temp_Email_Addresses_Election(string electionKey)
//    {
//      var sql = string.Empty;
//      sql += " SELECT StateCode,ElectionKey,OfficeKey,PoliticianKey";
//      sql += " FROM ElectionsPoliticians";
//      sql += " WHERE ElectionKey =" + db.SQLLit(electionKey);
//      sql += " AND OfficeKey != 'USPresident'";
//      sql += " ORDER BY PoliticianKey";
//      var tableElectionsPoliticiansFuture = db.Table(sql);
//      foreach (
//        DataRow rowElectionsPoliticians in tableElectionsPoliticiansFuture.Rows) 
//        Insert_One_Candidate_Email_Addresses_Temp_Table(rowElectionsPoliticians);

//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void Temp_Email_Addresses_Election_Candidates(string electionKey)
//    {
//      TempEmailAddresses.TruncateTable();
//      var sql = string.Empty;
//      sql += " SELECT ";
//      sql += " ElectionKey";
//      sql += " FROM Elections ";
//      sql += " WHERE IsViewable = 1";
//      // Temporarily allow past elections for debug
//      //SQL += " AND ElectionDate >= " + db.SQLLit(Db.DbToday);
//      sql += " AND ElectionKey = " + db.SQLLit(electionKey);
//      sql += " ORDER BY ElectionDate ASC";
//      var rowElection = db.Row_First_Optional(sql);
//      if (rowElection != null) Temp_Email_Addresses_Election(electionKey);

//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void
//      Temp_Email_Addresses_Elections_Candidates_Next_Upcoming_Viewable_State(
//      string stateCode)
//    {
//      //CHECKED

//      //db.ExecuteSQL("TRUNCATE TABLE TempEmailAddresses");
//      TempEmailAddresses.TruncateTable();
//      var sql = string.Empty;
//      sql += " SELECT ";
//      sql += " ElectionKey";
//      sql += " FROM Elections ";
//      sql += " WHERE IsViewable = 1";
//      sql += " AND ElectionDate >= " + db.SQLLit(Db.DbToday);
//      sql += " AND StateCode = " + db.SQLLit(stateCode);
//      sql += " ORDER BY ElectionDate ASC";

//      var rowElection = db.Row_First_Optional(sql);
//      if (rowElection != null) Temp_Email_Addresses_Election(rowElection["ElectionKey"].ToString());

//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void
//      Temp_Email_Addresses_Elections_Candidates_Upcoming_Viewable_All_States()
//    {
//      //CHECKED

//      //db.ExecuteSQL("TRUNCATE TABLE TempEmailAddresses");
//      TempEmailAddresses.TruncateTable();

//      //------------------
//      var sql = string.Empty;
//      sql += " SELECT ";
//      sql += " ElectionKey";
//      sql += " FROM Elections ";
//      sql += " WHERE IsViewable = 1";
//      sql += " AND ElectionDate >= " + db.SQLLit(Db.DbToday);
//      sql +=
//        " AND ((StateCode != 'US') AND (StateCode != 'U1') AND (StateCode != 'U2') AND (StateCode != 'U3') AND (StateCode != 'U4'))";
//      sql += " ORDER BY StateCode,ElectionDate DESC";
//      var tableElections = db.Table(sql);
//      foreach (DataRow rowElection in tableElections.Rows) //SQL = string.Empty;
//        Temp_Email_Addresses_Election(rowElection["ElectionKey"].ToString());

//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void Temp_Email_Addresses_State_Parties()
//    {
//      TempEmailAddresses.TruncateTable();

//      var sql = string.Empty;
//      sql += " SELECT";
//      sql += " Parties.StateCode";
//      sql += " ,Parties.PartyCode";
//      sql += " ,PartiesEmails.PartyKey";
//      sql += " ,PartiesEmails.PartyEmail";
//      sql += " ,PartiesEmails.PartyPassword";
//      sql += " ,PartiesEmails.PartyContactFName";
//      sql += " ,PartiesEmails.PartyContactLName";
//      sql += " ,PartiesEmails.PartyContactTitle";
//      sql += " FROM PartiesEmails,Parties";
//      sql += " WHERE Parties.StateCode = " +
//        db.SQLLit(Session["UserStateCode"].ToString());
//      sql += " AND PartiesEmails.PartyKey = Parties.PartyKey ";

//      if (Elections.IsPrimaryElection(ViewState["ElectionKey"].ToString())) //string Party_Code_Election =
//        //  db.PartyCode4ElectionKey(ViewState["ElectionKey"].ToString());
//        sql += " AND " +
//          db.SQLLit(db.PartyCode4ElectionKey(ViewState["ElectionKey"].ToString())) +
//          " = " + "Parties.PartyKey";

//      var tableStatePartyEmails = db.Table(sql);
//      foreach (DataRow rowPartyEmail in tableStatePartyEmails.Rows)
//        Insert_TempEmailAddresses(rowPartyEmail["PartyEmail"].ToString(),
//          rowPartyEmail["PartyContactFName"] + " " +
//            rowPartyEmail["PartyContactLName"], string.Empty, string.Empty
//          //, db.ElectionKey_Viewable_Latest_Any_Upcoming(
//          //    Session["UserStateCode"].ToString())
//          , ViewState["ElectionKey"].ToString(),
//          rowPartyEmail["StateCode"].ToString(),
//          rowPartyEmail["PartyKey"].ToString(),
//          rowPartyEmail["PartyContactFName"].ToString(),
//          rowPartyEmail["PartyContactLName"].ToString(),
//          rowPartyEmail["PartyContactTitle"].ToString());
//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void Temp_Email_Addresses_States_Parties()
//    {
//      //NEEDS TO BE TESTED

//      //db.ExecuteSQL("TRUNCATE TABLE TempEmailAddresses");
//      TempEmailAddresses.TruncateTable();

//      var sql = string.Empty;
//      sql += " SELECT";
//      sql += " Parties.StateCode";
//      sql += " ,Parties.PartyCode";
//      sql += " ,PartiesEmails.PartyKey";
//      sql += " ,PartiesEmails.PartyEmail";
//      sql += " ,PartiesEmails.PartyPassword";
//      sql += " ,PartiesEmails.PartyContactFName";
//      sql += " ,PartiesEmails.PartyContactLName";
//      sql += " ,PartiesEmails.PartyContactTitle";
//      sql += " FROM PartiesEmails,Parties";
//      sql += " WHERE PartiesEmails.PartyKey = Parties.PartyKey ";

//      var tableStatePartyEmails = db.Table(sql);
//      foreach (DataRow rowPartyEmail in tableStatePartyEmails.Rows)
//        Insert_TempEmailAddresses(rowPartyEmail["PartyEmail"].ToString(),
//          rowPartyEmail["PartyContactFName"] + " " +
//            rowPartyEmail["PartyContactLName"], string.Empty, string.Empty
//          //, db.ElectionKey_Viewable_Latest_Any_Upcoming(
//          //    Session["UserStateCode"].ToString())
//          , ViewState["ElectionKey"].ToString(),
//          rowPartyEmail["StateCode"].ToString(),
//          rowPartyEmail["PartyKey"].ToString(),
//          rowPartyEmail["PartyContactFName"].ToString(),
//          rowPartyEmail["PartyContactLName"].ToString(),
//          rowPartyEmail["PartyContactTitle"].ToString());

//      Label_Emails_Sending.Text = TempEmailAddresses.CountTable()
//        .ToString(CultureInfo.InvariantCulture);
//    }

//    protected void Msg_Email_Addresses_Built()
//    {
//      Msg.Text =
//        db.Ok("A temporary table of email addresses" +
//          " has been built. But no emails have been sent.");
//    }

//    protected void Msg_Email_Addresses_Not_Built()
//    {
//      Msg.Text =
//        db.Fail("No email addresses were retrieved for option selected." +
//          " And therefore no Subject After or Body After were created.");
//    }

//    #endregion Build Temp Table of Email Addresses

//    #region Radio Buttons

//    protected void Load_Templates_Election()
//    {
//      switch (RadioButtonList_Election.SelectedValue)
//      {
//        case "ElectionCustomStates":
//          break;
//        case "ElectionCustomCandidates":
//          break;
//        case "ElectionCustomParties":
//          break;
//        case "ElectionCompletion":
//          SubjectBefore.Text = db.Master_Str("EmailsSubjectElectionCompletion");
//          EmailBodyBefore.Text = db.Master_Str("EmailsBodyElectionCompletion");
//          break;
//        case "ElectionCandidatesLogin":
//          SubjectBefore.Text = db.Master_Str("EmailsSubjectCandidatesLogin");
//          EmailBodyBefore.Text = db.Master_Str("EmailsBodyCandidatesLogin");
//          break;
//        case "ElectionPartiesLogin":
//          SubjectBefore.Text = db.Master_Str("EmailsSubjectPartiesLogin");
//          EmailBodyBefore.Text = db.Master_Str("EmailsBodyPartiesLogin");
//          break;
//      }
//    }

//    //protected void Build_TempAddresses_Set_Controls_Election()
//    protected void Build_TempAddresses_Election()
//    {
//      #region Build TempAddresses Table - Load From: To: Cc: Format: Attachments

//      switch (RadioButtonList_Election.SelectedValue)
//      {
//        case "ElectionCustomStates":

//          Temp_Email_Addresses_State_Contacts(ViewState["StateCode"].ToString(),
//            ViewState["ElectionKey"].ToString());

//            break;
//        case "ElectionCustomCandidates":

//          Temp_Email_Addresses_Election_Candidates(
//            ViewState["ElectionKey"].ToString());
//          break;
//        case "ElectionCustomParties":

//          Temp_Email_Addresses_State_Parties();
//          break;
//        case "ElectionCompletion":

//          Temp_Email_Addresses_State_Contacts(ViewState["StateCode"].ToString(),
//            ViewState["ElectionKey"].ToString());
//          break;
//        case "ElectionCandidatesLogin":

//          Temp_Email_Addresses_Election_Candidates(
//            ViewState["ElectionKey"].ToString());
//          break;
//        case "ElectionPartiesLogin":

//          Temp_Email_Addresses_State_Parties();
//          break;
//      }

//      #endregion Build TempAddresses Table - Load From: To: Cc: Format: Attachments
//    }

//    protected void Set_Controls_Election()
//    {
//      string emailDate;
//      Button_Save_Changes.Enabled = true;

//      #region Load From: To: Cc: Format: Attachments

//      switch (RadioButtonList_Election.SelectedValue)
//      {
//        case "ElectionCustomStates":

//          #region Custom Text File to State Contacts for a Specific Election

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "Mgr";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";
//          //RadioButtonList_Attachments.SelectedValue = "SampleBallot";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text =
//            "Dates not recorded for custom emails.";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File to State Contacts for a Specific Election

//          break;
//        case "ElectionCustomCandidates":

//          #region Custom Text File for Candidates in a Specific Election

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "Mgr";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "None";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text =
//            "Dates not recorded for custom emails.";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File for Candidates in a Specific Election

//          break;
//        case "ElectionCustomParties":

//          #region Custom Text File to ALL Political Parties in State for a Specific Election

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "None";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text =
//            "Dates not recorded for custom emails.";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File to ALL Political Parties in State for a Specific Election

//          break;
//        case "ElectionCompletion":

//          #region Notify State of Election Completion

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "Mgr";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using template

//          emailDate = db.Elections_Date_Str(ViewState["ElectionKey"].ToString(),
//            "EmailsDateElectionCompletion");
//          Label_Emails_Last_Sent_Date.Text = !string.IsNullOrEmpty(emailDate) ? emailDate : "No emails sent previously for: ";

//          Label_Election.Text =
//            db.Elections_ElectionDesc(ViewState["ElectionKey"].ToString());

//          #endregion Report last time emails were sent using template

//          #endregion Notify State of Election Completion

//          break;
//        case "ElectionCandidatesLogin":

//          #region Provide Candidates with Login for a Specific Election

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "Mgr";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "None";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          emailDate = db.Elections_Date_Str(ViewState["ElectionKey"].ToString(),
//            "EmailsDateCandidatesLogin");
//          Label_Emails_Last_Sent_Date.Text = !string.IsNullOrEmpty(emailDate) ? emailDate : "No emails sent previously for: ";

//          Label_Election.Text =
//            db.Elections_ElectionDesc(ViewState["ElectionKey"].ToString());

//          #endregion Report last time emails were sent using this template

//          #endregion Provide Candidates with Login for a Specific Election

//          break;
//        case "ElectionPartiesLogin":

//          #region Provide State Parties with Login for a Specific Election

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "None";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using template

//          emailDate = db.Elections_Date_Str(ViewState["ElectionKey"].ToString(),
//            "EmailsDatePartiesLogin");
//          Label_Emails_Last_Sent_Date.Text = !string.IsNullOrEmpty(emailDate) ? emailDate : "No emails sent previously for: ";

//          Label_Election.Text =
//            db.Elections_ElectionDesc(ViewState["ElectionKey"].ToString());

//          #endregion Report last time emails were sent using template

//          #endregion Provide State Parties with Login for a Specific Election

//          break;
//      }

//      #endregion Load From: To: Cc: Format: Attachments
//    }

//    protected void Load_Templates_State()
//    {
//      switch (RadioButtonList_State.SelectedValue)
//      {
//        case "StateCustom":
//          break;
//        case "StateCustomCandidates":
//          break;
//        case "StateCustomParties":
//          break;
//        case "StatePrimaryRosters":
//          SubjectBefore.Text = db.Master_Str("EmailsSubjectElectionRosterPrimary");
//          EmailBodyBefore.Text = db.Master_Str("EmailsBodyElectionRosterPrimary");
//          break;
//        case "StateGeneralRosters":
//          SubjectBefore.Text = db.Master_Str("EmailsSubjectElectionRosterGeneral");
//          EmailBodyBefore.Text = db.Master_Str("EmailsBodyElectionRosterGeneral");
//          break;
//        case "StateCandidates":
//          SubjectBefore.Text = db.Master_Str("EmailsSubjectStateCandidates");
//          EmailBodyBefore.Text = db.Master_Str("EmailsBodyStateCandidates");
//          break;
//      }
//    }

//    protected void Build_TempAddresses_State()
//    {
//      #region Build TempAddresses Table - Load From: To: Cc: Format: Attachments

//      switch (RadioButtonList_State.SelectedValue)
//      {
//        case "StateCustom":
//          Temp_Email_Addresses_State_Contacts();
//          break;
//        case "StateCustomCandidates":
//          Temp_Email_Addresses_Elections_Candidates_Next_Upcoming_Viewable_State(
//            ViewState["StateCode"].ToString());
//          break;
//        case "StateCustomParties":
//          Temp_Email_Addresses_State_Parties();
//          break;
//        case "StatePrimaryRosters":
//          Temp_Email_Addresses_State_Contacts();
//          break;
//        case "StateGeneralRosters":
//          Temp_Email_Addresses_Election_General_Contacts_State();
//          break;
//        case "StateCandidates":
//          Temp_Email_Addresses_Elections_Candidates_Next_Upcoming_Viewable_State(
//            ViewState["StateCode"].ToString());
//          break;
//      }

//      #endregion Build TempAddresses Table - Load From: To: Cc: Format: Attachments
//    }

//    //protected void Build_TempAddresses_Set_Controls_State()
//    protected void Set_Controls_State()
//    {
//      Button_Save_Changes.Enabled = true;

//      #region Load From: To: Cc: Format: Attachments

//      switch (RadioButtonList_State.SelectedValue)
//      {
//        case "StateCustom":

//          #region Custom Text File to State Contacts

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";
          
//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "Mgr";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          //RadioButtonList_Attachments.SelectedValue = "None";
//          RadioButtonList_Attachments.SelectedValue = "SampleBallot";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text =
//            "Dates not recorded for custom emails.";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File to State Contacts

//          break;
//        case "StateCustomCandidates":

//          #region Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in State

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "Mgr";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "None";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text =
//            "Dates not recorded for custom emails.";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in State

//          break;
//        case "StateCustomParties":

//          #region Custom Text File to ALL Political Parties in State

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "None";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text =
//            "Dates not recorded for custom emails.";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File to ALL Political Parties in State

//          break;
//        case "StatePrimaryRosters":

//          #region Request Primary Election Roster from State

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "Mgr";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #endregion Request Primary Election Roster from State

//          break;
//        case "StateGeneralRosters":

//          #region Request General Election Roster from State

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "Mgr";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #endregion Request General Election Roster from State

//          break;
//        case "StateCandidates":

//          #region Candidates in Next Viewable Upcoming Elections in State

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "Mgr";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "None";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #endregion Candidates in Next Viewable Upcoming Elections in State

//          break;
//      }

//      #endregion Load From: To: Cc: Format: Attachments
//    }

//    protected void Load_Templates_All()
//    {
//      switch (RadioButtonList_All.SelectedValue)
//      {
//        case "AllCustomStates":
//          break;
//        case "AllCustomCandidates":
//          break;
//        case "AllCustomParties":
//          break;
//        case "AllPrimaryRosters":
//          SubjectBefore.Text = db.Master_Str("EmailsSubjectAllPrimaryRosters");
//          EmailBodyBefore.Text = db.Master_Str("EmailsBodyAllPrimaryRosters");
//          break;
//        case "AllGeneralRosters":
//          SubjectBefore.Text = db.Master_Str("EmailsSubjectAllRosters");
//          EmailBodyBefore.Text = db.Master_Str("EmailsBodyAllRosters");
//          break;
//        case "AllCandidates":
//          SubjectBefore.Text = db.Master_Str("EmailsSubjectAllCandidates");
//          EmailBodyBefore.Text = db.Master_Str("EmailsBodyAllCandidates");
//          break;
//      }
//    }

//    protected void Email_Controls_All_States()
//    {
//      #region Set From:

//      RadioButtonList_From.SelectedValue = "RonKahlow";

//      #endregion Set From:

//      #region cc:

//      RadioButtonList_Cc.SelectedValue = "Mgr";

//      #endregion cc:

//      #region Mail Format:

//      RadioButtonList_MailFormat.SelectedValue = "Html";

//      #endregion Mail Format:

//      #region Attachments:

//      RadioButtonList_Attachments.SelectedValue = "None";

//      #endregion Attachments:
//    }

//    protected void Build_TempAddresses_All_States()
//    {
//      #region Build TempAddresses Table - Load From: To: Cc: Format: Attachments

//      switch (RadioButtonList_All.SelectedValue)
//      {
//        case "AllCustomStates":
//          Temp_Email_Addresses_States_Contacts();
//          break;
//        case "AllCustomStatesPrimaryElection":
//          Temp_Email_Addresses_States_Contacts_Future_Primary_Election();
//          break;
//        case "AllCustomStatesGeneralElection":
//          Temp_Email_Addresses_States_Contacts_Future_General_Election();
//          break;
//        case "AllCustomStatesOffYearElection":
//          Temp_Email_Addresses_States_Contacts_Future_OffYear_Election();
//          break;
//        case "AllCustomStatesSpecialElection":
//          Temp_Email_Addresses_States_Contacts_Future_Special_Election();
//          break;
//        case "AllCustomStatesPresidentialPrimaryElection":
//          Temp_Email_Addresses_States_Contacts_Future_PresidentialPrimary_Election();
//          break;
//        case "AllCustomStatesNoPrimaryElection":
//          Temp_Email_Addresses_States_Contacts_NO_Future_Primary_Election();
//          break;
//        case "AllCustomStatesNoGeneralElection":
//          Temp_Email_Addresses_States_Contacts_NO_Future_General_Election();
//          break;
//        case "AllCustomStatesNoOffYearElection":
//          Temp_Email_Addresses_States_Contacts_NO_Future_OffYear_Election();
//          break;
//        case "AllCustomStatesNoSpecialElection":
//          Temp_Email_Addresses_States_Contacts_NO_Future_Special_Election();
//          break;
//        case "AllCustomStatesNoPresidentialPrimaryElection":
//          Temp_Email_Addresses_States_Contacts_NO_Future_PresidentialPrimary_Election
//            ();
//          break;
//        case "AllCustomCandidates":
//          Temp_Email_Addresses_Elections_Candidates_Upcoming_Viewable_All_States();
//          break;
//        case "AllCustomParties":
//          Temp_Email_Addresses_States_Parties();
//          break;
//        case "AllPrimaryRosters":
//          Temp_Email_Addresses_Election_Primary_Contacts_All_States();
//          break;
//        case "AllGeneralRosters":
//          Temp_Email_Addresses_Election_General_Contacts_All_States();
//          break;
//        case "AllCandidates":
//          Temp_Email_Addresses_Elections_Candidates_Upcoming_Viewable_All_States();
//          break;
//      }

//      #endregion Build TempAddresses Table - Load From: To: Cc: Format: Attachments
//    }

//    protected void Set_Controls_All_States()
//    {
//      Button_Save_Changes.Enabled = true;

//      #region Build TempAddresses Table - Load From: To: Cc: Format: Attachments

//      switch (RadioButtonList_All.SelectedValue)
//      {
//        case "AllCustomStates":

//          #region Custom Text File to ALL States Contacts

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File to ALL States Contacts

//          break;
//        case "AllCustomStatesPrimaryElection":

//          #region Custom Text File only for States' Contacts with Future PRIMARY Election

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File only for States' Contacts with Future PRIMARY Election

//          break;
//        case "AllCustomStatesGeneralElection":

//          #region Custom Text File only for States' Contacts with Future GENERAL Election

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File only for States' Contacts with Future GENERAL Election

//          break;
//        case "AllCustomStatesOffYearElection":

//          #region Custom Text File only for States' Contacts with Future OFF-YEAR Election

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File only for States' Contacts with Future OFF-YEAR Election

//          break;
//        case "AllCustomStatesSpecialElection":

//          #region Custom Text File only for States' Contacts with Future SPECIAL Election

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File only for States' Contacts with Future SPECIAL Election

//          break;
//        case "AllCustomStatesPresidentialPrimaryElection":

//          #region Custom Text File only for States' Contacts with Future PRESIDENTIAL PRIMARY Election

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File only for States' Contacts with Future PRESIDENTIAL PRIMARY Election

//          break;
//        case "AllCustomStatesNoPrimaryElection":

//          #region Custom Text File only for States' Contacts with Future PRIMARY Election

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File only for States' Contacts with Future PRIMARY Election

//          break;
//        case "AllCustomStatesNoGeneralElection":

//          #region Custom Text File only for States' Contacts with Future GENERAL Election

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File only for States' Contacts with Future GENERAL Election

//          break;
//        case "AllCustomStatesNoOffYearElection":

//          #region Custom Text File only for States' Contacts with Future OFF-YEAR Election

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File only for States' Contacts with Future OFF-YEAR Election

//          break;
//        case "AllCustomStatesNoSpecialElection":

//          #region Custom Text File only for States' Contacts with Future SPECIAL Election

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File only for States' Contacts with Future SPECIAL Election

//          break;
//        case "AllCustomStatesNoPresidentialPrimaryElection":

//          #region Custom Text File only for States' Contacts with Future PRESIDENTIAL PRIMARY Election

//          Email_Controls_All_States();

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File only for States' Contacts with Future PRESIDENTIAL PRIMARY Election

//          break;
//        case "AllCustomCandidates":

//          #region Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in All States

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "Mgr";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "None";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File to ALL Candidates in Next Viewable Upcoming Elections in All States

//          break;
//        case "AllCustomParties":

//          #region Custom Text File to ALL Political Parties in ALL States

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "None";

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #endregion cc:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text = "Dates not recorded for custom emails";
//          Label_Election.Text = string.Empty;

//          #endregion Report last time emails were sent using this template

//          Button_Save_Changes.Enabled = false;

//          #endregion Custom Text File to ALL Political Parties in ALL States

//          break;
//        case "AllPrimaryRosters":

//          #region Request Primary Election Roster from ALL States

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "Mgr";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text =
//            db.Master_Date_Str("EmailsDateAllPrimaryRosters");
//          if (Label_Emails_Last_Sent_Date.Text == string.Empty) Label_Emails_Last_Sent_Date.Text = "First Time";
//          Label_Election.Text = " All States";

//          #endregion Report last time emails were sent using this template

//          #endregion Request Primary Election Roster from ALL States

//          break;
//        case "AllGeneralRosters":

//          #region Request General Election Roster from ALL States

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "RonKahlow";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "Mgr";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text =
//            db.Master_Date_Str("EmailsDateAllRosters");
//          if (Label_Emails_Last_Sent_Date.Text == string.Empty) Label_Emails_Last_Sent_Date.Text = "First Time";
//          Label_Election.Text = " All States";

//          #endregion Report last time emails were sent using this template

//          #endregion Request General Election Roster from ALL States

//          break;
//        case "AllCandidates":

//          #region All Candidates in Next Viewable Upcoming Elections in All States

//          #region Set From:

//          RadioButtonList_From.SelectedValue = "Mgr";

//          #endregion Set From:

//          #region cc:

//          RadioButtonList_Cc.SelectedValue = "None";

//          #endregion cc:

//          #region Mail Format:

//          RadioButtonList_MailFormat.SelectedValue = "Html";

//          #endregion Mail Format:

//          #region Attachments:

//          RadioButtonList_Attachments.SelectedValue = "None";

//          #endregion Attachments:

//          #region Report last time emails were sent using this template

//          Label_Emails_Last_Sent_Date.Text =
//            db.Master_Date_Str("EmailsDateAllCandidates");
//          if (Label_Emails_Last_Sent_Date.Text == string.Empty) Label_Emails_Last_Sent_Date.Text = "First Time";

//          Label_Election.Text = " All Candidates in all upcoming State Elections";

//          #endregion Report last time emails were sent using this template

//          #endregion All Candidates in Next Viewable Upcoming Elections in All States

//          break;
//      }

//      #endregion Build TempAddresses Table - Load From: To: Cc: Format: Attachments
//    }

//    protected void RadioButtonList_Election_SelectedIndexChanged(object sender,
//      EventArgs e)
//    {
//      try
//      {
//        Check_Illegal_Input_In_Textboxes();

//        Clear_Textboxes();

//        Load_Templates_Election();

//        //Build_TempAddresses_Set_Controls_Election();
//        Build_TempAddresses_Election();
//        Set_Controls_Election();

//        if (TempEmailAddresses.CountTable() > 0)
//        {
//          if (!Is_Custom_Email())
//          {
//            Substitutions_First_Email_Row_Example();
//            Check_Sustitutions();
//          }

//          Show_Email_Addresses();

//          Msg_Email_Addresses_Built();
//        }
//        else Msg_Email_Addresses_Not_Built();
//      }
//      catch (Exception ex)
//      {
//        #region

//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);

//        #endregion
//      }
//    }

//    protected void RadioButtonList_State_SelectedIndexChanged(object sender,
//      EventArgs e)
//    {
//      try
//      {
//        Check_Illegal_Input_In_Textboxes();

//        Clear_Textboxes();

//        Load_Templates_State();

//        Build_TempAddresses_State();
//        Set_Controls_State();

//        if (TempEmailAddresses.CountTable() > 0)
//        {
//          if (!Is_Custom_Email())
//          {
//            Substitutions_First_Email_Row_Example();
//            Check_Sustitutions();
//          }

//          Show_Email_Addresses();

//          Msg_Email_Addresses_Built();
//        }
//        else Msg_Email_Addresses_Not_Built();
//      }
//      catch (Exception ex)
//      {
//        #region

//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);

//        #endregion
//      }
//    }

//    protected void RadioButtonList_All_SelectedIndexChanged(object sender,
//      EventArgs e)
//    {
//      try
//      {
//        Check_Illegal_Input_In_Textboxes();

//        Clear_Textboxes();

//        Load_Templates_All();

//        //Build_TempAddresses_Set_Controls_All_States();
//        Build_TempAddresses_All_States();
//        Set_Controls_All_States();

//        if (TempEmailAddresses.CountTable() > 0)
//        {
//          if (!Is_Custom_Email())
//          {
//            Substitutions_First_Email_Row_Example();
//            Check_Sustitutions();
//          }

//          Show_Email_Addresses();

//          Msg_Email_Addresses_Built();
//        }
//        else Msg_Email_Addresses_Not_Built();
//      }
//      catch (Exception ex)
//      {
//        #region

//        Msg.Text = db.Fail(ex.Message);
//        db.Log_Error_Admin(ex);

//        #endregion
//      }
//    }

//    #endregion Radio Buttons

    protected override void OnInit(EventArgs e)
    {
      LegacyRedirect(SecureAdminPage.GetBulkEmailPageUrl(QueryState,
        QueryCounty, QueryLocal));
      base.OnInit(e);
    }

    //private void Page_Load(object sender, EventArgs e)
    //{
    //  if (!IsPostBack)
    //  {
    //    ViewState["StateCode"] = db.State_Code();
    //    ViewState["CountyCode"] = db.County_Code();
    //    ViewState["LocalCode"] = db.Local_Code();
    //    if (!SecurePage.IsMasterUser) if (!db.Is_User_Security_Ok()) SecurePage.HandleSecurityException();

    //    try
    //    {
    //      #region ViewState["ElectionKey"]

    //      ViewState["ElectionKey"] = string.Empty;
    //      if (!string.IsNullOrEmpty(QueryElection)) ViewState["ElectionKey"] = QueryElection;

    //      #endregion

    //      #region PageTitle

    //      PageTitle.Text = "Emails";
    //      if (!string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
    //      {
    //        PageTitle.Text += " ";
    //        PageTitle.Text += db.Elections_Str(
    //          ViewState["ElectionKey"].ToString(), "ElectionDesc");
    //        if (db.Elections_Bool(ViewState["ElectionKey"].ToString(), "IsViewable")) PageTitle.Text += " (Viewable)";
    //        else PageTitle.Text += " (Not Public Viewable)";
    //      }

    //      else if (!string.IsNullOrEmpty(Session["UserStateCode"].ToString()))
    //        PageTitle.Text += " " +
    //          StateCache.GetStateName(Session["UserStateCode"].ToString());
    //      else PageTitle.Text += " All States";

    //      #endregion

    //      #region Tables Visible

    //      Table_All.Visible = false;
    //      Table_State.Visible = false;
    //      Table_Election.Visible = false;

    //      if (!string.IsNullOrEmpty(ViewState["ElectionKey"].ToString()))
    //      {
    //        #region Election Emails

    //        Table_Election.Visible = true;

    //        #region check election is upcoming and viewable

    //        if (
    //          !db.Is_Election_Upcoming_Viewable(ViewState["ElectionKey"].ToString()))
    //        {
    //          //throw new ApplicationException("The election is not upcoming and viewable.");
    //          Msg.Text = db.Msg("NOTE: The election is not upcoming and viewable.");
    //          Msg.Text += db.Msg(" Select the type of emails to send.");
    //        }
    //        else Msg.Text = db.Msg(" Select the type of emails to send.");

    //        #endregion check election is upcoming and viewable

    //        #endregion Election Emails
    //      }
    //      else if (!string.IsNullOrEmpty(Session["UserStateCode"].ToString()))
    //      {
    //        #region Single State Emails

    //        Table_State.Visible = true;

    //        #endregion Single State Emails

    //        Msg.Text = db.Msg(" Select the type of emails to send.");
    //      }
    //      else
    //      {
    //        #region All States Emails

    //        Table_All.Visible = true;

    //        #endregion All States Emails

    //        Msg.Text = db.Msg(" Select the type of emails to send.");
    //      }

    //      #endregion Tables Visible
    //    }
    //    catch (Exception ex)
    //    {
    //      Msg.Text = db.Fail(ex.Message);
    //      db.Log_Error_Admin(ex);
    //    }
    //  }
    //}
  }
}