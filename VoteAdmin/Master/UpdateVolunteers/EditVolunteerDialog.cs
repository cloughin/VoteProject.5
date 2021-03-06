using System;
using System.Linq;
using DB.Vote;
using DB.VoteLog;
using static System.String;

namespace Vote.Master
{
  public partial class UpdateVolunteersPage
  {
    [PageInitializer]
    private class EditVolunteerDialogItem : DataItemBase
    {
      private const string GroupName = "EditVolunteer";
      private readonly UpdateVolunteersPage _Page;

      private EditVolunteerDialogItem(UpdateVolunteersPage page) : base(GroupName)
      {
        _Page = page;
      }

      protected override string GetCurrentValue()
      {
        var email = _Page.GetVolunteerToEdit();
        if (Column == "PartyCode") return VolunteersView.GetPartyKey(email).Substring(2);
        var column = VolunteersView.GetColumn(Column);
        var value = VolunteersView.GetColumn(column, email);
        return value == null ? Empty : ToDisplay(value);
      }

      private static bool ValidateUniqueEmail(DataItemBase item)
      {
        var success = ValidateRequired(item);
        if (success)
        {
          var newValue = item.DataControl.GetValue();
          var oldValue = GetCurrentValue(item);
          if (newValue.IsNeIgnoreCase(oldValue) &&
            PartiesEmails.PartyEmailExists(item.DataControl.GetValue()))
          {
            item.Feedback.PostValidationError(item.DataControl,
              "The new email address already exists.");
            success = false;
          }
        }
        return success;
      }

      private static EditVolunteerDialogItem[] GetDialogInfo(UpdateVolunteersPage page)
      {
        var editVolunteerInfo = new[]
        {
          new EditVolunteerDialogItem(page)
          {
            Column = "Password",
            Description = "Password",
            Validator = ValidateRequired
          },
          new EditVolunteerDialogItem(page)
          {
            Column = "FirstName",
            Description = "First Name"
          },
          new EditVolunteerDialogItem(page)
          {
            Column = "LastName",
            Description = "Last Name"
          },
          new EditVolunteerDialogItem(page) {Column = "Phone", Description = "Phone"},
          new EditVolunteerDialogItem(page) {Column = "StateCode", Description = "State"},
          new EditVolunteerDialogItem(page) {Column = "PartyCode", Description = "Party"},
          // this must be last because it can change the key
          new EditVolunteerDialogItem(page)
          {
            Column = "Email",
            Description = "Email Address",
            Validator = ValidateUniqueEmail
          }
        };

        foreach (var item in editVolunteerInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return editVolunteerInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateVolunteersPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._EditVolunteerDialogInfo = GetDialogInfo(page);
      }

      protected override void Log(string oldValue, string newValue)
      {
        LogDataChange.LogUpdate(VolunteersView.TableName, Column, oldValue, newValue,
          UserName, UserSecurityClass, DateTime.UtcNow, _Page.GetVolunteerToEdit());
      }

      protected override bool Update(object newValue)
      {
        var email = _Page.GetVolunteerToEdit();
        if (Column == "StateCode" || Column == "PartyCode")
        {
          var partyKey = _Page.ControlEditVolunteerStateCode.GetValue() +
            _Page.ControlEditVolunteerPartyCode.GetValue();
          VolunteersView.UpdatePartyKey(partyKey, email);
        }
        else
        {
          var column = VolunteersView.GetColumn(Column);
          VolunteersView.UpdateColumn(column, newValue, email);
        }
        if (Column == "Email")
        {
          // update notes
          VolunteersNotes.UpdateEmailByEmail(newValue as string, email);
        }
        return true;
      }
    }

    private EditVolunteerDialogItem[] _EditVolunteerDialogInfo;

    protected void ButtonEditVolunteer_OnClick(object sender, EventArgs e)
    {
      switch (EditVolunteerReloading.Value)
      {
        case "reloading":
        {
          EditVolunteerReloading.Value = Empty;
          RefreshReport.Value = "N";
          var stateCode = VolunteersView.GetStateCode(GetVolunteerToEdit()).SafeString();
          StateCache.Populate(ControlEditVolunteerStateCode, stateCode);
          Parties.PopulateNationalParties(ControlEditVolunteerPartyCode, true, null, true,
            null);
          //LoadEditParties(stateCode);
          _EditVolunteerDialogInfo.LoadControls();
          //LoadEditUndos();
          FeedbackEditVolunteer.AddInfo("Volunteer information loaded.");
        }
          break;

        case "":
        {
          // normal update
          _EditVolunteerDialogInfo.ClearValidationErrors();
          _EditVolunteerDialogInfo.Update(FeedbackEditVolunteer);
          var emailItem = _EditVolunteerDialogInfo.FirstOrDefault(i => i.Column == "Email");
          if (!emailItem?.DataControl.HasClass("error") == true)
            VolunteerToEdit.Value = DataItemBase.GetCurrentValue(emailItem);
          RefreshReport.Value = "Y";
          //LoadEditUndos();
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{EditVolunteerReloading.Value}'");
      }
    }
  }
}