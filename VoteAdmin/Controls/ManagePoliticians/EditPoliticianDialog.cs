using System;
using DB.Vote;
using DB.VoteLog;
using Vote.Politician;
using static System.String;

namespace Vote.Controls
{
  public partial class ManagePoliticiansPanel
  {
    #region DataItem objects

    private class EditPoliticianDialogItem : DataItemBase
    {
      private const string GroupName = "EditPolitician";
      private readonly ManagePoliticiansPanel ThisControl;

      private EditPoliticianDialogItem(ManagePoliticiansPanel thisControl) :
        base(GroupName)
      {
        ThisControl = thisControl;
      }

      protected override string GetCurrentValue() => Politicians.GetColumnExtended(Column,
        ThisControl.GetPoliticianKeyToEdit().Key);

      public static EditPoliticianDialogItem[] GetDialogInfo(ManagePoliticiansPanel control)
      {
        var editPoliticianInfo = new[]
        {
          new EditPoliticianDialogItem(control)
          {
            Column = "FName",
            Description = "First Name",
            Validator = ValidateFirstName
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "MName",
            Description = "Middle Name",
            Validator = ValidateMiddleName
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "Nickname",
            Description = "Nickname",
            Validator = ValidateNickname
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "LName",
            Description = "Last Name",
            Validator = ValidateLastName
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "Suffix",
            Description = "Suffix",
            Validator = ValidateSuffix
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "AddOn",
            Description = "ANC AddOn"
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "DateOfBirth",
            Description = "Date of Birth",
            Validator = ValidateDateOfBirthOptional
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "StateAddress",
            Description = "State Street Address",
            Validator = ValidateStreet
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "StateCityStateZip",
            Description = "State City, State Zip",
            Validator = ValidateCityStateZip
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "StatePhone",
            Description = "State Phone"
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "StateEmailAddr",
            Description = "State Email",
            Validator = ValidateEmail
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "StateWebAddr",
            Description = "State Web Site",
            Validator = ValidateWebAddress
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "Address",
            Description = "Candidate Street Address",
            Validator = ValidateStreet
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "CityStateZip",
            Description = "Candidate City, State Zip",
            Validator = ValidateCityStateZip
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "Phone",
            Description = "Candidate Phone"
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "EmailAddr",
            Description = "Candidate Email",
            Validator = ValidateEmail
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "WebAddr",
            Description = "Candidate Web Site",
            Validator = ValidateWebAddress
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "PartyKey",
            Description = "Political Party"
          }
        };

        foreach (var item in editPoliticianInfo) item.InitializeItem(control);

        InitializeGroup(control, GroupName);

        return editPoliticianInfo;
      }

      protected override void Log(string oldValue, string newValue) =>
        LogDataChange.LogUpdate(Politicians.TableName, Column, oldValue, newValue,
          VotePage.UserName, SecurePage.UserSecurityClass, DateTime.UtcNow,
          ThisControl.GetPoliticianKeyToEdit().Key);

      protected override bool Update(object newValue)
      {
        Politicians.UpdateColumnExtended(Column, newValue,
          ThisControl.GetPoliticianKeyToEdit().Key);
        return true;
      }
    }

    private EditPoliticianDialogItem[] _EditPoliticianDialogInfo;

    #endregion DataItem objects

    #region Event handlers and overrides

    protected void ButtonEditPolitician_OnClick(object sender, EventArgs e)
    {
      var toEdit = GetPoliticianKeyToEdit();
      var politicianKey = toEdit.Key;
      CandidateHtml.Controls.Clear();
      var isRunningMate = !IsNullOrWhiteSpace(MainCandidateIfRunningMate.Value);
      if (Politicians.GetStateCodeFromKey(politicianKey) != "DC")
        AddOnEditElement.AddCssClasses("hidden");

      switch (EditPoliticianReloading.Value)
      {
        case "reloading":
        {
          EditPoliticianReloading.Value = Empty;
          ControlEditPoliticianPartyKey.Items.Clear();
          VotePage.LoadPartiesDropdown(Politicians.GetStateCodeFromKey(politicianKey),
            ControlEditPoliticianPartyKey, Empty, VotePage.PartyCategories.None,
            VotePage.PartyCategories.StateParties, VotePage.PartyCategories.NationalParties,
            VotePage.PartyCategories.NonParties);
          _EditPoliticianDialogInfo.LoadControls();
          FeedbackEditPolitician.AddInfo("Politician information loaded.");
          var href = SecurePoliticianPage.GetUpdateIntroPageUrl(politicianKey);
          UpdateIntroLink.HRef = href;
          UpdateIntroLinkProfessional.HRef = href + "#bio2-ALLBio333333";
        }
          break;

        case "":
        {
          // normal update
          _EditPoliticianDialogInfo.ClearValidationErrors();
          _EditPoliticianDialogInfo.Update(FeedbackEditPolitician);
          var partyCodeToSuppress = Empty;
          if (isRunningMate)
            partyCodeToSuppress =
              Parties.GetPartyCode(
                Politicians.GetPartyKey(MainCandidateIfRunningMate.Value));
          var row = Politicians.GetCandidateData(SafeGetElectionKey(), politicianKey,
            isRunningMate);
          // use current dynamic IsIncumbent
          row["IsIncumbent"] = toEdit.ShowAsIncumbent;
          CandidateHtml.Controls.Add(CreateCandidateEntry(row, Mode, partyCodeToSuppress, 
            !isRunningMate && ElectionsPoliticians.ElectionKeyOfficeKeyPoliticianKeyExists(
              SafeGetElectionKey(), SafeGetOfficeKey(), politicianKey)));
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{EditPoliticianReloading.Value}'");
      }

      NameOnBallots.InnerText = PageCache.GetTemporary().Politicians
        .GetPoliticianName(politicianKey);
      UpdateIntroPage.SetPartyNameAndLink(ControlEditPoliticianPartyKey.GetValue(),
        PartyName);
    }

    #endregion Event handlers and overrides
  }
}