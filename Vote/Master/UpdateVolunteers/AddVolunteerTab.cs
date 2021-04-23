using System;
using DB.Vote;

namespace Vote.Master
{
  public partial class UpdateVolunteersPage
  {
    #region from db

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
      var password = string.Empty;
      //Make a AAADDD password
      for (var n = 0; n < 3; n++)
        password += GetRandomAlpha24();
      for (var n = 0; n < 3; n++)
        password += GetRandomDigit();
      return password;
    }

    #endregion from db

    #region Private

    #region DataItem object

    [PageInitializer]
    private class AddVolunteerTabItem : DataItemBase
    {
      private const string GroupName = "AddVolunteer";
      // ReSharper disable once NotAccessedField.Local
      private readonly UpdateVolunteersPage _Page;

      private AddVolunteerTabItem(UpdateVolunteersPage page)
        : base(GroupName)
      {
        _Page = page;
      }

      private static AddVolunteerTabItem[] GetTabInfo(UpdateVolunteersPage page)
      {
        var addVolunteerTabInfo = new[]
        {
          new AddVolunteerTabItem(page)
          {
            Column = "Email",
            Description = "Email",
            Validator = ValidateEmailRequired
          },
          new AddVolunteerTabItem(page)
          {
            Column = "FirstName",
            Description = "First Name",
            Validator = ValidateRequired
          },
          new AddVolunteerTabItem(page)
          {
            Column = "LastName",
            Description = "Last Name",
            Validator = ValidateRequired
          },
          new AddVolunteerTabItem(page)
          {
            Column = "Phone",
            Description = "Phone"
          },
          new AddVolunteerTabItem(page)
          {
            Column = "Password",
            Description = "Password"
          },
          new AddVolunteerTabItem(page)
          {
            Column = "State",
            Description = "State",
            Validator = ValidateRequired
          },
          new AddVolunteerTabItem(page)
          {
            Column = "Party",
            Description = "Party Preference",
            Validator = ValidateRequired
          },
          new AddVolunteerTabItem(page)
          {
            Column = "Notes",
            Description = "Notes"
          }
        };

        addVolunteerTabInfo.Initialize(page, GroupName);

        return addVolunteerTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateVolunteersPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._AddVolunteerTabInfo = GetTabInfo(page);
        if (!page.IsPostBack)
        {
          StateCache.Populate(page.ControlAddVolunteerState, "--- Select state ---", string.Empty);
          Parties.PopulateNationalParties(page.ControlAddVolunteerParty, true, null, true);
        }
      }
    }

    private AddVolunteerTabItem[] _AddVolunteerTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonAddVolunteer_OnClick(object sender, EventArgs e)
    {
      try
      {
        _AddVolunteerTabInfo.ClearValidationErrors();
        var success = _AddVolunteerTabInfo.Validate();
        if (!success) return;

        // check for existing email
        var email = ControlAddVolunteerEmail.GetValue();
        var isVolunteer = PartiesEmails.GetIsVolunteer(email);
        if (isVolunteer != null)
        {
          // duplicate
          FeedbackAddVolunteer.PostValidationError(ControlAddVolunteerEmail,
            isVolunteer.Value
              ? "The email address is already registered as a volunteer"
              : "The email address is registered as a party official");
          return;
        }

        var password = ControlAddVolunteerPassword.GetValue().Trim();
        if (string.IsNullOrWhiteSpace(password))
          password = MakeUniquePassword();

        VolunteersView.Insert(email, password,
          ControlAddVolunteerState.GetValue() + ControlAddVolunteerParty.GetValue(),
          ControlAddVolunteerFirstName.GetValue(), ControlAddVolunteerLastName.GetValue(),
          ControlAddVolunteerPhone.GetValue().Trim());
        var notes = ControlAddVolunteerNotes.GetValue().StripRedundantWhiteSpace();
        if (!string.IsNullOrWhiteSpace(notes))
          VolunteersNotes.Insert(email, DateTime.UtcNow, notes);

        _AddVolunteerTabInfo.Reset();
        FeedbackAddVolunteer.AddInfo("Volunteer added.");
      }
      catch (Exception ex)
      {
        FeedbackAddVolunteer.HandleException(ex);
      }
    }

    #endregion Event handlers and overrides
  }
}