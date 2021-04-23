using System;
using System.Collections.Generic;
using System.Data;
using DB.Vote;
using DB.VoteLog;
using Vote.Reports;
using static System.String;

namespace Vote.Admin
{
  public partial class PartiesPage : SecureAdminPage, IAllowEmptyStateCode
  {
    public override IEnumerable<string> NonStateCodesAllowed => new[] {"US"};

    private string _PartyKey;
    private string _EmailAddress;

    #region legacy

    public static string Ok(string msg)
    {
      return $"<span class=\"MsgOk\">SUCCESS!!! {msg}</span>";
    }

    public static string Fail(string msg)
    {
      return $"<span class=\"MsgFail\">****FAILURE**** {msg}</span>";
    }

    public static string Message(string msg)
    {
      return $"<span class=\"Msg\">{msg}</span>";
    }

    public static Random RandomObject;

    public static char GetRandomAlpha24()
    {
      if (RandomObject == null)
        RandomObject = new Random();
      var n = RandomObject.Next(24);
      if (n < 8)
        return Convert.ToChar(n + Convert.ToInt32('A'));
      return n < 13
        ? Convert.ToChar(n + Convert.ToInt32('B'))
        : Convert.ToChar(n + Convert.ToInt32('C'));
    }

    public static char GetRandomDigit()
    {
      if (RandomObject == null)
        RandomObject = new Random();
      return Convert.ToChar(RandomObject.Next(10) + Convert.ToUInt32('0'));
    }

    public static string MakeUniquePassword()
    {
      var password = Empty;
      //Make a AAADDD password
      for (var n = 0; n < 3; n++)
        password += GetRandomAlpha24();
      for (var n = 0; n < 3; n++)
        password += GetRandomDigit();
      return password;
    }

    public static string Fix_Url_Parms(string url)
    {
      //sets first parm in a query string to ? if all seperators are &'s
      if (url.IndexOf('?', 0, url.Length) == -1 //no ?
        && url.IndexOf('&', 0, url.Length) > -1) //but one or more &
      {
        var loc = url.IndexOf('&', 0, url.Length);
        var lenAfter = url.Length - loc - 1;
        var urlBefore = url.Substring(0, loc);
        var urlAfter = url.Substring(loc + 1, lenAfter);
        return urlBefore + "?" + urlAfter;
      }
      return url;
    }

    public static string PartyKey_Add_To_QueryString_Master_User()
    {
      if (IsMasterUser && !IsNullOrEmpty(QueryParty))
        return "&Party=" + QueryParty;
      return Empty;
    }

    public static string Url_Party_Default(string stateCode, string electionKey)
    {
      var url = @"/Party/Default.aspx";
      if (!IsNullOrEmpty(stateCode))
        url += "&State=" + stateCode;
      if (!IsNullOrEmpty(electionKey))
        url += "&Election=" + electionKey;
      url += PartyKey_Add_To_QueryString_Master_User();
      return Fix_Url_Parms(url);
    }

    private static string MakePartyKey(string stateCode, string partyCode)
    {
      return stateCode.Trim().ToUpper() + partyCode.Trim().ToUpper();
    }

    private static string Url_Party_Default() => Url_Party_Default(Empty, Empty);

    #endregion legacy

    private void PartiesReport()
    {
      PartiesSummaryReport.GetReport(StateCode).AddTo(PartiesReportPlaceHolder);
    }

    private void Emails_Report()
    {
      var partyName = Parties.GetPartyName(_PartyKey);
      PartyEmailsAndContactsHeading.Text = $"{partyName} Emails and Contacts";

      PartiesEmailReport.GetReport(_PartyKey).AddTo(EmailReportPlaceHolder);
    }

    private void CheckForDangerousInput()
    {
      CheckForDangerousInput(TextboxBallotCode_Add, TextBoxPartyName_Add, TextboxBallotCode,
        TextBoxPartyName, TextBoxAddressLine1, TextBoxAddressLine2, TextBoxCityStateZip,
        TextboxWebAddress, TextboxEmailAddress, TextboxPhone, TextBox_First_Name,
        TextBox_Last_Name, TextBox_Title);
    }

    private void CheckBallotCode()
    {
      if (TextboxBallotCode_Add.Text == Empty)
        throw new ApplicationException("A Ballot Code is required.");
      if (TextboxBallotCode_Add.Text.Trim().Length > 3)
        throw new ApplicationException("A Ballot Code can only be 1 to 3 characters.");
    }

    private void Check_Email_Address_Empty()
    {
      if (IsNullOrEmpty(TextboxEmailAddress.Text.Trim()))
        throw new ApplicationException("The Email Address is empty.");
    }

    private void Load_Party_Data()
    {
      var partiesTable = Parties.GetDataByPartyKey(_PartyKey);
      if (partiesTable.Count != 1)
        throw new ApplicationException($"Did not find a unique row for this PartyKey: {_PartyKey}");

      var rowParty = partiesTable[0];
      PartyKey.Text = rowParty.PartyKey;
      TextBoxPartyName.Text = rowParty.PartyName;
      TextboxBallotCode.Text = rowParty.PartyCode;
      TextboxWebAddress.Text = rowParty.PartyUrl;
      TextBoxAddressLine1.Text = rowParty.PartyAddressLine1;
      TextBoxAddressLine2.Text = rowParty.PartyAddressLine2;
      TextBoxCityStateZip.Text = rowParty.PartyCityStateZip;

      HyperLink_Party_Politician_Links.NavigateUrl = Url_Party_Default();
    }

    private void Load_Email_Data(string partyEmail)
    {
      var tableEmail = PartiesEmails.GetDataByPartyEmail(partyEmail);
      if (tableEmail.Count != 1)
        throw new ApplicationException($"Did not find a unique row for this PartyEmail: {partyEmail}");

      var rowEmail = tableEmail[0];
      TextboxEmailAddress.Text = rowEmail.PartyEmail;
      TextboxPhone.Text = rowEmail.PartyContactPhone;
      TextBox_First_Name.Text = rowEmail.PartyContactFirstName;
      TextBox_Last_Name.Text = rowEmail.PartyContactLastName;
      TextBox_Title.Text = rowEmail.PartyContactTitle;

      Emails_Report();
    }

    private void Clear_Political_Party_TextBoxes()
    {
      PartyKey.Text = Empty;
      TextBoxPartyName.Text = Empty;
      TextboxBallotCode.Text = Empty;
      TextboxWebAddress.Text = Empty;
      TextBoxAddressLine1.Text = Empty;
      TextBoxAddressLine2.Text = Empty;
      TextBoxCityStateZip.Text = Empty;
    }

    private void Clear_Add_Party_TextBoxes()
    {
      TextBox_Order_Add.Text = Empty;
      TextboxBallotCode_Add.Text = Empty;
      TextBoxPartyName_Add.Text = Empty;
    }

    private void Clear_Email_TextBoxes()
    {
      TextboxEmailAddress.Text = Empty;
      TextboxPhone.Text = Empty;
      TextBox_First_Name.Text = Empty;
      TextBox_Last_Name.Text = Empty;
      TextBox_Title.Text = Empty;
    }

    private void ThePageTitle()
    {
      var title = $"{StateCache.GetStateName(StateCode)} Political Parties Emails, Websites and Information";
      Page.Title = title;
      H1.InnerHtml = title;
    }

    private void Controls_All_Not_Visible()
    {
      TableAdd.Visible = false;
      TableParty.Visible = false;
      TableDelete.Visible = false;
    }

    private void Controls_Select_Or_Add_Party()
    {
      Controls_All_Not_Visible();

      TableAdd.Visible = true;
      TableParty.Visible = true;

      Msg_Party.Text =
        Message("To edit a party's information including emails, click on a Party Name." +
          " To view the party's website, click the Web Address.");
    }

    private void Controls_Edit_Party()
    {
      Controls_All_Not_Visible();

      TableParty.Visible = true;
      Load_Party_Data();

      Clear_Email_TextBoxes();

      Emails_Report();

      TableDelete.Visible = IsMasterUser;

      Msg_Email.Text = Message("To edit an email address, click on the address.");
    }

    private static void Controls_Add_EmailAddress_Mode()
    {
    }

    private void Controls_Edit_Email_Address()
    {
      Load_Party_Data();
      Load_Email_Data(_EmailAddress);
    }

    private void Controls_Entry()
    {
      if (_EmailAddress != Empty)
        Controls_Edit_Email_Address();
      else if (_PartyKey != Empty)
        Controls_Edit_Party();
      else if (_PartyKey == Empty &&
        _EmailAddress == Empty)
        Controls_Select_Or_Add_Party();
      else if (_PartyKey != Empty &&
        _EmailAddress == Empty)
        Controls_Add_EmailAddress_Mode();
    }

    protected void ButtonAdd_Click1(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();
        CheckBallotCode();
        var partyName = TextBoxPartyName_Add.Text.Trim();
        var partyOrder = TextBox_Order_Add.Text.Trim();
        if (partyName == Empty)
          throw new ApplicationException("A Party Name is required.");
        if (partyOrder == Empty)
          throw new ApplicationException("A Party Order on dropdown list is required.");
        if (!partyOrder.IsValidInteger())
          throw new ApplicationException("A Party Order must be an integer.");

        ViewState["PartyKey"] = _PartyKey = MakePartyKey(StateCode, TextboxBallotCode_Add.Text);

        if (Parties.PartyKeyExists(_PartyKey))
          throw new ApplicationException($"{Parties.GetPartyName(_PartyKey)}" +
            $" has the Party Code {_PartyKey}. You need to change the Ballot Code.");

        // Add the Party
        Parties.Insert(_PartyKey, TextboxBallotCode_Add.Text.Trim().ToUpper(), StateCode, 
          int.Parse(partyOrder), partyName.SimpleRecase(), Empty, Empty,Empty,
          Empty, false);

        Clear_Add_Party_TextBoxes();
        Load_Party_Data();
        PartiesReport();
        Emails_Report();

        Msg_Add.Text =
          Ok($"{TextBoxPartyName.Text.Trim()}: was ADDED.  " +
            " The data recorded for the political party  is shown in red." +
            " You can now add another party, make additional changes " +
            " or make changes to one of the parties in the Parties Table below.");
      }
      catch (Exception ex)
      {
        Msg_Add.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void Button_Add_Email_Click1(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();

        if (IsNullOrEmpty(_PartyKey))
        {
          Msg_Email.Text = Fail("You need to select a party before you can add an email address.");
        }
        else
        {
          var emailAddress = TextboxEmailAddress_Add.Text.Trim().RemoveMailTo().ToLower();
          if (Validation.IsValidEmailAddress(emailAddress))
          {
            if (PartiesEmails.PartyEmailExists(emailAddress))
              Msg_Email.Text = Fail("The email address already exists.");
            else
            {
              PartiesEmails.Insert(emailAddress, MakeUniquePassword(), _PartyKey,
                Empty, Empty, Empty, Empty, false,
                DateTime.UtcNow, false);

              LogAdminData.Insert(DateTime.Now, UserSecurityClass, UserName, "PartyEmail",
                Empty, emailAddress);

              ViewState["EmailAddress"] = _EmailAddress = emailAddress;
              TextboxEmailAddress_Add.Text = Empty;
              Load_Email_Data(_EmailAddress);

              PartiesReport();

              Msg_Email.Text = Ok("The email address has been added.");
            }
          }
          else
            Msg_Email.Text = Fail("The email address is not valid.");
        }
      }
      catch (Exception ex)
      {
        Msg_Email.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void Button_Delete_Email_Click(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();

        if (!IsNullOrEmpty(TextboxEmailAddress.Text.Trim()))
        {
          PartiesEmails.DeleteByPartyEmail(TextboxEmailAddress.Text.Trim());

          Clear_Email_TextBoxes();

          PartiesReport();
          Emails_Report();

          Msg_Email.Text = Ok("The email address has been deleted.");
        }
        else
          Msg_Email.Text = Fail("No email address was selected for deletion.");
      }
      catch (Exception ex)
      {
        Msg_Email.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void ButtonDelete_Click1(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();

        // Only Minor National Parties or State Parties can be deleted
        if (Parties.GetIsPartyMajor(_PartyKey, false))
          throw new ApplicationException("Only Minor National Parties can be deleted.");

        // save Party Info before we delete row
        var partyName = Parties.GetPartyName(_PartyKey);

        Parties.DeleteByPartyKey(_PartyKey);

        ViewState["PartyKey"] = _PartyKey = Empty;

        Controls_Select_Or_Add_Party();

        Clear_Political_Party_TextBoxes();

        ThePageTitle();

        PartiesReport();

        Msg_Delete_Party.Text = Ok(partyName + " has been DELETED.");
      }
      catch (Exception ex)
      {
        Msg_Delete_Party.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextboxBallotCode_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();
        CheckBallotCode();
        Parties.UpdatePartyCode(TextboxBallotCode.Text.Trim(), _PartyKey);
        Load_Party_Data();
        PartiesReport();
        Msg_Party.Text = Ok("Ballot Code was updated.");
      }
      catch (Exception ex)
      {
        Msg_Party.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextBoxPartyName_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();
        if (TextBoxPartyName.Text.Trim() == Empty)
          throw new ApplicationException("A Party Name is required.");
        Parties.UpdatePartyName(TextBoxPartyName.Text.Trim(), _PartyKey);
        Load_Party_Data();
        PartiesReport();
        Msg_Party.Text = Ok("Party Name was updated.");
      }
      catch (Exception ex)
      {
        Msg_Party.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextBoxAddressLine1_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();
        Parties.UpdatePartyAddressLine1(TextBoxAddressLine1.Text.Trim(), _PartyKey);
        Load_Party_Data();
        PartiesReport();
        Msg_Party.Text = Ok("Address Line 1 was updated.");
      }
      catch (Exception ex)
      {
        Msg_Party.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextBoxAddressLine2_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();
        Parties.UpdatePartyAddressLine2(TextBoxAddressLine2.Text.Trim(), _PartyKey);
        Load_Party_Data();
        PartiesReport();
        Msg_Party.Text = Ok("Address Line 2 was updated.");
      }
      catch (Exception ex)
      {
        Msg_Party.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextBoxCityStateZip_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();
        Parties.UpdatePartyAddressLine2ByStateCode(TextBoxCityStateZip.Text.Trim(),
          _PartyKey);
        Load_Party_Data();
        PartiesReport();
        Msg_Party.Text = Ok("City, State Zip was updated.");
      }
      catch (Exception ex)
      {
        Msg_Party.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextboxWebAddress_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();
        Parties.UpdatePartyUrl(TextboxWebAddress.Text.Trim().RemoveHttp(), _PartyKey);
        Load_Party_Data();
        PartiesReport();
        Msg_Party.Text = Ok("Web address was updated.");
      }
      catch (Exception ex)
      {
        Msg_Party.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    #region Email Maint

    protected void TextboxEmailAddress_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();
        Check_Email_Address_Empty();
        var email = TextboxEmailAddress.Text.Trim();
        PartiesEmails.UpdatePartyEmail(TextboxEmailAddress.Text.Trim().RemoveMailTo(), email);
        ViewState["EmailAddress"] = _EmailAddress = TextboxEmailAddress.Text.Trim().RemoveMailTo();
        Load_Party_Data();
        Load_Email_Data(TextboxEmailAddress.Text.Trim());
        PartiesReport();
        Msg_Party.Text = Ok("Email address was updated.");
      }
      catch (Exception ex)
      {
        Msg_Party.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextboxPhone_TextChanged(object sender, EventArgs e)
    {
      try
      {
        CheckForDangerousInput();
        Check_Email_Address_Empty();
        var email = TextboxEmailAddress.Text.Trim();
        PartiesEmails.UpdatePartyContactPhone(TextboxPhone.Text.Trim(), email);
        Load_Party_Data();
        Load_Email_Data(email);
        PartiesReport();
        Msg_Party.Text = Ok("Phone was updated.");
      }
      catch (Exception ex)
      {
        Msg_Party.Text = Fail(ex.Message);
        LogAdminError(ex);
      }
    }

    protected void TextBox_First_Name_TextChanged(object sender, EventArgs e)
    {
      CheckForDangerousInput();
      Check_Email_Address_Empty();
      var email = TextboxEmailAddress.Text.Trim();
      PartiesEmails.UpdatePartyContactFirstName(TextBox_First_Name.Text.Trim(), email);
      Load_Party_Data();
      Load_Email_Data(email);
      PartiesReport();
      Msg_Party.Text = Ok("First Name was updated.");
    }

    protected void TextBox_Last_Name_TextChanged(object sender, EventArgs e)
    {
      CheckForDangerousInput();
      Check_Email_Address_Empty();
      var email = TextboxEmailAddress.Text.Trim();
      PartiesEmails.UpdatePartyContactLastName(TextBox_Last_Name.Text.Trim(), email);
      Load_Party_Data();
      Load_Email_Data(email);
      PartiesReport();
      Msg_Party.Text = Ok("Last Name was updated.");
    }

    protected void TextBox_Title_TextChanged(object sender, EventArgs e)
    {
      CheckForDangerousInput();
      Check_Email_Address_Empty();
      var email = TextboxEmailAddress.Text.Trim();
      PartiesEmails.UpdatePartyContactTitle(TextBox_Title.Text.Trim(), email);
      Load_Party_Data();
      Load_Email_Data(email);
      PartiesReport();
      Msg_Party.Text = Ok("Title was updated.");
    }

    #endregion Email Maint

    private void Page_Load(object sender, EventArgs e)
    {
      if (!IsMasterUser && !IsStateAdminUser)
        HandleSecurityException();
      ViewState["PartyKey"] =_PartyKey = QueryParty;
      ViewState["EmailAddress"] = _EmailAddress = GetQueryString("Email");

      if (IsPostBack)
      {
        var partyKey = ViewState["PartyKey"] as string;
        if (!IsNullOrEmpty(partyKey))
          _PartyKey = partyKey;
        var emailAddress = (string) ViewState["EmailAddress"];
        if (!IsNullOrEmpty(emailAddress))
          _EmailAddress = emailAddress;
      }
      else
      {
        Page.Title = "Parties";

        if (IsNullOrWhiteSpace(StateCode))
        {
          UpdateControls.Visible = false;
          NoJurisdiction.CreateLink("/admin/Parties.aspx?state=US", "National Parties");
          NoJurisdiction.CreateStateLinks("/admin/Parties.aspx?state={StateCode}");
          NoJurisdiction.ShowHeadOptional(false);
          NoJurisdiction.Visible = true;
          return;
        }

        try
        {
          Controls_Entry();

          ThePageTitle();

          // Renumber State Parties by 10
          var order = 0;
          var tableParties = Parties.GetCacheDataByStateCode(StateCode)
            .OrderBy(r => r.PartyOrder);
          foreach (var rowParty in tableParties)
          {
            order += 10;
            Parties.UpdatePartyOrder(order, rowParty.PartyKey);
          }

          PartiesReport();
        }
        catch (Exception ex)
        {
          #region

          Msg_Party.Text = Fail(ex.Message);
          LogAdminError(ex);

          #endregion
        }
      }
    }
  }
}