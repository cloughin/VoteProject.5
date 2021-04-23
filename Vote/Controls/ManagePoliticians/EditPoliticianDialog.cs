using System;
using DB.Vote;
using DB.VoteLog;
using Vote.Politician;

namespace Vote.Controls
{
  public partial class ManagePoliticiansPanel
  {
    #region DataItem objects

    private class EditPoliticianDialogItem : DataItemBase
    {
      private const string GroupName = "EditPolitician";
      protected readonly ManagePoliticiansPanel ThisControl;

      protected EditPoliticianDialogItem(ManagePoliticiansPanel thisControl)
        : base(GroupName)
      {
        ThisControl = thisControl;
      }

      protected override string GetCurrentValue() => 
        Politicians.GetColumnExtended(Column, ThisControl.GetPoliticianKeyToEdit());

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
            Validator = ValidateDateOfBirth
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "StateAddress",
            Description = "Street Address",
            Validator = ValidateStreet
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "StateCityStateZip",
            Description = "City, State Zip",
            Validator = ValidateCityStateZip
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "StatePhone",
            Description = "Phone"
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "StateEmailAddr",
            Description = "Email",
            Validator = ValidateEmail
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "StateWebAddr",
            Description = "Web Site",
            Validator = ValidateWebAddress
          },
          new EditPoliticianDialogItem(control)
          {
            Column = "PartyKey",
            Description = "Political Party"
          },
          new EditPoliticianIncumbentDialogItem(control)
          {
            Column = "IsIncumbent",
            Description = "Is Incumbent",
            ConvertFn = ToBool
          }
        };

        foreach (var item in editPoliticianInfo)
          item.InitializeItem(control);

        InitializeGroup(control, GroupName);

        return editPoliticianInfo;
      }

      protected override void Log(string oldValue, string newValue) => 
        LogDataChange.LogUpdate(Politicians.TableName, Column, oldValue, newValue,
        VotePage.UserName, SecurePage.UserSecurityClass, DateTime.UtcNow,
        ThisControl.GetPoliticianKeyToEdit());

      protected override bool Update(object newValue)
      {
        Politicians.UpdateColumnExtended(Column, newValue,
          ThisControl.GetPoliticianKeyToEdit());
        return true;
      }
    }

    private class EditPoliticianIncumbentDialogItem : EditPoliticianDialogItem
    {
      internal EditPoliticianIncumbentDialogItem(ManagePoliticiansPanel thisControl)
        : base(thisControl)
      {
      }

      protected override string GetCurrentValue() => 
        ThisControl.Mode == DataMode.ManageCandidates
        ? ElectionsPoliticians.GetIsIncumbentByElectionKeyOfficeKeyPoliticianKey(
            ThisControl.SafeGetElectionKey(), ThisControl.SafeGetOfficeKey(),
            ThisControl.GetPoliticianKeyToEdit(), false)
          .ToString()
        : string.Empty;

      protected override void Log(string oldValue, string newValue)
      {
        if (ThisControl.Mode == DataMode.ManageCandidates)
          LogDataChange.LogUpdate(ElectionsPoliticians.Column.IsIncumbent, oldValue,
            newValue, VotePage.UserName, SecurePage.UserSecurityClass, DateTime.UtcNow,
            ThisControl.SafeGetElectionKey(), ThisControl.SafeGetOfficeKey(),
            ThisControl.GetPoliticianKeyToEdit());
      }

      protected override bool Update(object newValue)
      {
        if (ThisControl.Mode == DataMode.ManageCandidates)
          ElectionsPoliticians.UpdateIsIncumbentByElectionKeyOfficeKeyPoliticianKey(
            (bool) newValue, ThisControl.SafeGetElectionKey(), ThisControl.SafeGetOfficeKey(),
            ThisControl.GetPoliticianKeyToEdit());
        return true;
      }
    }

    private EditPoliticianDialogItem[] _EditPoliticianDialogInfo;

    #endregion DataItem objects

    #region Event handlers and overrides

    protected void ButtonEditPolitician_OnClick(object sender, EventArgs e)
    {
      var politiciankey = GetPoliticianKeyToEdit();
      CandidateHtml.Controls.Clear();
      var isRunningMate =
        !string.IsNullOrWhiteSpace(MainCandidateIfRunningMate.Value);
      if (Politicians.GetStateCodeFromKey(politiciankey) != "DC")
        AddOnEditElement.AddCssClasses("hidden");

      switch (EditPoliticianReloading.Value)
      {
        case "reloading":
        {
          EditPoliticianReloading.Value = string.Empty;
          ControlEditPoliticianPartyKey.Items.Clear();
          VotePage.LoadPartiesDropdown(Politicians.GetStateCodeFromKey(politiciankey),
            ControlEditPoliticianPartyKey, string.Empty, VotePage.PartyCategories.None,
            VotePage.PartyCategories.StateParties, VotePage.PartyCategories.NationalParties,
            VotePage.PartyCategories.NonParties);
          _EditPoliticianDialogInfo.LoadControls();
          if ((Mode != DataMode.ManageCandidates) || isRunningMate ||
            !ElectionsPoliticians.ElectionKeyOfficeKeyPoliticianKeyExists(
              SafeGetElectionKey(), SafeGetOfficeKey(), politiciankey))
            InputEditPoliticianIsIncumbent.AddCssClasses("hidden");
          else
            InputEditPoliticianIsIncumbent.RemoveCssClass("hidden");
          FeedbackEditPolitician.AddInfo("Politician information loaded.");
          var href = SecurePoliticianPage.GetUpdateIntroPageUrl(politiciankey);
          UpdateIntroLink.HRef = href;
          UpdateIntroLinkProfessional.HRef = href + "#bio2-ALLBio333333";
        }
          break;

        case "":
        {
          // normal update
          _EditPoliticianDialogInfo.ClearValidationErrors();
          _EditPoliticianDialogInfo.Update(FeedbackEditPolitician);
          var partyCodeToSuppress = string.Empty;
          if (isRunningMate)
            partyCodeToSuppress =
              Parties.GetPartyCode(
                Politicians.GetPartyKey(MainCandidateIfRunningMate.Value));
          var row = Politicians.GetCandidateData(SafeGetElectionKey(), politiciankey,
            isRunningMate);
          CandidateHtml.Controls.Add(CreateCandidateEntry(row,
            Mode, partyCodeToSuppress));
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }

      NameOnBallots.InnerText =
        PageCache.GetTemporary().Politicians.GetPoliticianName(politiciankey);
      UpdateIntroPage.SetPartyNameAndLink(
        ControlEditPoliticianPartyKey.GetValue(), PartyName);
    }

    #endregion Event handlers and overrides
  }
}