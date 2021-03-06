using System;
using DB.Vote;
using DB.VoteLog;

namespace Vote.Admin
{
  public partial class UpdateOfficesPage
  {
    #region Private

    private string GetOfficeKeyToEdit() => OfficeToEdit.Value;

    [PageInitializer]
    private class EditOfficeDialogItem : OfficesDataItem
    {
      private const string GroupName = "EditOffice";

      private EditOfficeDialogItem(UpdateOfficesPage page) : base(page, GroupName)
      {
      }

      protected override string GetCurrentValue()
      {
        var officeKey = Page.GetOfficeKeyToEdit();
        object value;
        if (Column == "ShowWriteIn")
          value = Offices.GetWriteInLines(officeKey, 0) > 0;
        else
        {
          var column = Offices.GetColumn(Column);
          value = Offices.GetColumn(column, officeKey);
        }
        return value == null ? string.Empty : ToDisplay(value);
      }

      private static EditOfficeDialogItem[] GetDialogInfo(UpdateOfficesPage page)
      {
        var editPoliticianInfo = new[]
        {
          new EditOfficeDialogItem(page)
          {
            Column = "OfficeLine1",
            Description = "1st Line of Office Title",
            Validator = ValidateRequired
          },
          new EditOfficeDialogItem(page)
          {
            Column = "OfficeLine2",
            Description = "1st Line of Office Title"
          },
          new EditOfficeDialogItem(page)
          {
            Column = "Incumbents",
            Description = "Incumbents",
            Validator = ValidateNumeric
          },
          new EditOfficeDialogItem(page)
          {
            Column = "ElectionPositions",
            Description = "Election Positions",
            Validator = ValidateNumeric
          },
          new EditOfficeDialogItem(page)
          {
            Column = "PrimaryPositions",
            Description = "Primary Positions",
            Validator = ValidateNumeric
          },
          new EditOfficeDialogItem(page)
          {
            Column = "PrimaryRunoffPositions",
            Description = "Primary Runoff Positions",
            Validator = ValidateSignedNumeric
          },
          new EditOfficeDialogItem(page)
          {
            Column = "GeneralRunoffPositions",
            Description = "General Runoff Positions",
            Validator = ValidateSignedNumeric
          },
          new EditOfficeDialogItem(page)
          {
            Column = "ShowWriteIn",
            Description = "Show Write In",
            ConvertFn = ToBool
          },
          new EditOfficeDialogItem(page)
          {
            Column = "IsRunningMateOffice",
            Description = "Is Running Mate Office",
            ConvertFn = ToBool
          },
          new EditOfficeDialogItem(page)
          {
            Column = "IsOnlyForPrimaries",
            Description = "Is Only For Primaries",
            ConvertFn = ToBool
          },
          new EditOfficeDialogItem(page)
          {
            Column = "IsInactive",
            Description = "Is Inactive",
            ConvertFn = ToBool
          }
        };

        foreach (var item in editPoliticianInfo)
          item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return editPoliticianInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateOfficesPage page)
        // ReSharper restore UnusedMember.Local
        => page._EditOfficeDialogInfo = GetDialogInfo(page);

      protected override void Log(string oldValue, string newValue) => 
        LogDataChange.LogUpdate(Offices.TableName, Column, oldValue, newValue,
        UserName, UserSecurityClass, DateTime.UtcNow,
        Page.GetOfficeKeyToEdit());

      protected override bool Update(object newValue)
      {
        var officeKey = Page.GetOfficeKeyToEdit();
        if (Column == "ShowWriteIn")
        {
          Offices.UpdateWriteInLines((bool) newValue
            ? 1
            : 0, officeKey);
        }
        else
        {
          var column = Offices.GetColumn(Column);
          Offices.UpdateColumn(column, newValue, officeKey);
        }
        return true;
      }
    }

    private EditOfficeDialogItem[] _EditOfficeDialogInfo;

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonEditOffice_OnClick(object sender, EventArgs e)
    {
      switch (EditOfficeReloading.Value)
      {
        case "reloading":
        {
          EditOfficeReloading.Value = string.Empty;
          _EditOfficeDialogInfo.LoadControls();
          FeedbackEditOffice.AddInfo("Office information loaded.");
        }
          break;

        case "":
        {
          // normal update
          _EditOfficeDialogInfo.ClearValidationErrors();
          if (AdminPageLevel != AdminPageLevel.State)
            OfficeToEdit.Value = Offices.ActualizeOffice(GetOfficeKeyToEdit(), CountyCode, LocalCode);
          if (_EditOfficeDialogInfo.Update(FeedbackEditOffice) == 0)
            LoadOfficeControl(OfficeToEdit.Value);
        }
          break;

        default:
          throw new VoteException("Unknown reloading option");
      }
    }

    #endregion Event handlers and overrides
  }
}