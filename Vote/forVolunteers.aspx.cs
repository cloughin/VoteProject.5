using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using DB.Vote;
using static System.String;

namespace Vote
{
  public partial class ForVolunteersPage : PublicPage // not CacheablePage due to Captcha
  {
    #region legacy

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

    #endregion legacy

    private const string TitleTagAllStatesDomain =
      "Volunteer to Help Inform Voters and Support Better Elections";

    private const string MetaDescriptionAllStatesDomain =
      "We offer many ways to volunteer your help to inform voters and to support better elections. All you need is an internet connection and the desire.";

    private const string TitleTagSingleStateDomain =
      "Volunteer to Help Inform [[state]] Voters and Support Better Elections";

    private const string MetaDescriptionSingleStateDomain =
      "We offer many ways to volunteer your help to inform voters and to support better [[state]] elections. All you need is an internet connection and the desire.";

    private void SubmitCallback(Dictionary<string, string> dict)
    {
      var email = dict["EmailFormFromEmailAddress"];
      var isVolunteer = PartiesEmails.GetIsVolunteer(email);
      if (isVolunteer != null) // email exists
        throw new VoteException(isVolunteer.Value
          ? "This email address is already registered to a volunteer"
          : "This email address is registered to a party official");
      var subject = dict["EmailFormSubject"];
      var message = dict["EmailFormMessage"];
      var firstName = dict["EmailFormFirstName"];
      var lastName = dict["EmailFormLastName"];
      var partyCode = dict["EmailFormParty"];
      var stateCode = dict["EmailFormState"];
      var phone = dict["EmailFormPhone"];
      var password = dict["EmailFormPassword"];
      var dateStamp = DateTime.UtcNow;
      if (IsNullOrWhiteSpace(password)) 
        password = MakeUniquePassword();

      // for email display
      EmailForm.SetOptionalValue("EmailFormParty", Parties.GetNationalPartyDescription(partyCode));
      EmailForm.SetOptionalValue("EmailFormPassword", password);

      VolunteersView.Insert(email, password, stateCode + partyCode, firstName, lastName, phone);
      VolunteersNotes.Insert(email, dateStamp, subject);
      if (!IsNullOrWhiteSpace(message))
        VolunteersNotes.Insert(email, dateStamp.AddSeconds(1), message);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      var firstNameControl = new TextBoxWithNormalizedLineBreaks().AddCssClasses("email-form-first-name narrow no-zoom");
      EmailForm.AddOptionalItem(firstNameControl, true, "EmailFormFirstName", "First Name");

      var lastNameControl = new TextBoxWithNormalizedLineBreaks().AddCssClasses("email-form-last-name narrow no-zoom");
      EmailForm.AddOptionalItem(lastNameControl, true, "EmailFormLastName", "Last Name");

      var partyControl = new HtmlSelect();
      partyControl.AddCssClasses("email-form-party narrow no-zoom");
      //Parties.PopulateNationalParties(partyControl, true, null, true);
      // eliminate non-partisan
      Parties.PopulateNationalParties(partyControl, false, null, true);
      EmailForm.AddOptionalItem(partyControl, true, "EmailFormParty",
        "Your Preferred Political Party – the political party you would like to focus most of your efforts on",
        "Preferred Political Party");

      var stateControl = new HtmlSelect();
      stateControl.AddCssClasses("email-form-state narrow no-zoom");
      StateCache.Populate(stateControl, "--- Select state ---", Empty);
      EmailForm.AddOptionalItem(stateControl, true, "EmailFormState", "Your State", "State");

      var phoneControl = new TextBoxWithNormalizedLineBreaks().AddCssClasses("email-form-phone narrow no-zoom");
      EmailForm.AddOptionalItem(phoneControl, "EmailFormPhone", 
        "Phone Number – only used if we can not reach you via email (optional)",
        "Phone");

      var passwordControl = new TextBoxWithNormalizedLineBreaks().AddCssClasses("email-form-password narrow no-zoom");
      EmailForm.AddOptionalItem(passwordControl, "EmailFormPassword", 
        "Preferred Login Password – we will create and assign one for you if you have no preference (optional)",
        "Preferred Login Password");

      EmailForm.MessageOptional = true;
      EmailForm.Callback = SubmitCallback;

      if (DomainData.IsValidStateCode) // Single state
      {
        Title = $"{PublicMasterPage.SiteName} | {TitleTagSingleStateDomain.Substitute()}";
        MetaDescription = MetaDescriptionSingleStateDomain.Substitute();
      }
      else
      {
        Title = $"{PublicMasterPage.SiteName} | {TitleTagAllStatesDomain.Substitute()}";
        MetaDescription = MetaDescriptionAllStatesDomain.Substitute();
      }

      EmailForm.ToEmailAddress = "john.fleisch@vote-usa.org";
      EmailForm.CcEmailAddress = "ron.kahlow@vote-usa.org";

      EmailForm.SetItems(
        "I would like to volunteer for whatever is needed",
        "I would like to volunteer to scrape candidate websites",
        "I would like to volunteer to research county and local elected offices",
        "I would like to volunteer to interview and video candidates",
        "I would like to volunteer to speak to my local political parties/community outreach groups",
        "I would like to contact candidates via email");
    }
  }
}