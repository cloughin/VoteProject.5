using System;
using System.Linq;
using System.Text.RegularExpressions;
using DB.Vote;

namespace Vote.Admin
{
  public partial class UpdateJurisdictionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class BulkAddDistrictsTabItem : JurisdictionsDataItem
    {
      private const string GroupName = "BulkAddDistricts";

      private BulkAddDistrictsTabItem(UpdateJurisdictionsPage page)
        : base(page, GroupName)
      {
      }

      protected override string GetCurrentValue()
      {
        return null;
      }

      protected override bool Update(object newValue)
      {
        return false;
      }

      private static BulkAddDistrictsTabItem[] GetTabInfo(
        UpdateJurisdictionsPage page)
      {
        var bulkAddDistrictsTabInfo = new[]
        {
          new BulkAddDistrictsTabItem(page)
          {
            Column = "LocalDistrict",
            Description = "Local District Name Template",
            Validator = item =>
            {
              // Special validator:
              // ValidateTitleCaseRequired plus must contain exactly one *
              if (!ValidateTitleCaseRequired(item)) return false;
              if (Regex.Matches(item.DataControl.GetValue(), @"\*").Count == 1) return true;
              item.Feedback.PostValidationError(item.DataControl,
                "The template must contain exactly one asterisk");
              return false;
            }
          },
          new BulkAddDistrictsTabItem(page)
          {
            // bulk add not available in phase 3
            Column = "LocalCode",
            Description = "Local Code"/*,
            Validator = ValidateLocalCodeOptional*/
          },
          new BulkAddDistrictsTabItem(page)
          {
            Column = "Override",
            Description = "Add Conflicted District",
            ConvertFn = ToBool
          },
          new BulkAddDistrictsTabItem(page)
          {
            Column = "StartSequence",
            Description = "Starting Sequence Number",
            Validator = ValidateNumeric,
            ConvertFn = ToInt
          },
          new BulkAddDistrictsTabItem(page)
          {
            Column = "EndSequence",
            Description = "Ending Sequence Number",
            Validator = ValidateNumeric,
            ConvertFn = ToInt
          }
        };

        foreach (var item in bulkAddDistrictsTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return bulkAddDistrictsTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateJurisdictionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._BulkAddDistrictsTabInfo = GetTabInfo(page);
        //if (page.AdminPageLevel != AdminPageLevel.County || IsPhase3(page.StateCode))
          page.TabBulkAddDistrictsItem.Visible = false;
      }
    }

    private BulkAddDistrictsTabItem[] _BulkAddDistrictsTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonBulkAddDistricts_OnClick(object sender, EventArgs e)
    {
      BulkAddDistrictsOverride.AddCssClasses("hidden");
      switch (BulkAddDistrictsReloading.Value)
      {
        case "reloading":
        {
          BulkAddDistrictsReloading.Value = string.Empty;
          _BulkAddDistrictsTabInfo.ClearValidationErrors();
          RefreshCurrentDistrictsList(BulkAddDistrictsCurrentDistricts);
          _BulkAddDistrictsTabInfo.LoadControls();
          FeedbackBulkAddDistricts.AddInfo("Bulk Add Districts information loaded.");
        }
          break;

        case "":
        {
          // normal update
          _BulkAddDistrictsTabInfo.ClearValidationErrors();
          RefreshCurrentDistrictsList(BulkAddDistrictsCurrentDistricts);

          // validate
          if (!_BulkAddDistrictsTabInfo.FindItem("LocalDistrict").Validate() ||
            //!_BulkAddDistrictsTabInfo.FindItem("LocalCode").Validate() ||
            !_BulkAddDistrictsTabInfo.FindItem("StartSequence").Validate() ||
            !_BulkAddDistrictsTabInfo.FindItem("EndSequence").Validate())
            return;

          // additional validation for sequence numbers
          var startSequence = int.Parse(ControlBulkAddDistrictsStartSequence.Text);
          var endSequence = int.Parse(ControlBulkAddDistrictsEndSequence.Text);
          if (startSequence < 1 || startSequence > 99 || endSequence < startSequence ||
            endSequence > 99)
          {
            FeedbackBulkAddDistricts.PostValidationError(
              new[]
                {ControlBulkAddDistrictsStartSequence, ControlBulkAddDistrictsEndSequence},
              "The sequence numbers must be between 1 and 99 and the ending sequence must be greater than or equal the starting sequence.");
            return;
          }

          var startCodeText = ControlBulkAddDistrictsLocalCode.Text;
          var startCode = string.IsNullOrWhiteSpace(startCodeText)
            ? startSequence
            : int.Parse(startCodeText);
          var endCode = startCode + (endSequence - startSequence);
          if (endCode > 99)
          {
            FeedbackBulkAddDistricts.PostValidationError(ControlBulkAddDistrictsLocalCode,
              "The ending local code would exceed 99.");
            return;
          }

          var localCodes =
            Enumerable.Range(startCode, endCode - startCode + 1)
              .Select(i => i.ToString().ZeroPad(2))
              .ToList();

          var localDistrictTemplate = ControlBulkAddDistrictsLocalDistrict.Text;
          var @override = ControlBulkAddDistrictsOverride.Checked;
          var conflicted = RefreshCurrentDistrictsList(BulkAddDistrictsCurrentDistricts,
            null, localCodes);
          if (!@override && conflicted)
          {
            BulkAddDistrictsOverride.RemoveCssClass("hidden");
            FeedbackBulkAddDistricts.PostValidationError(BulkAddDistrictsOverride,
              "Potential local code");
            return;
          }
          if (!EnsureLocalCodesAreAvailable(localCodes))
          {
            FeedbackBulkAddDistricts.PostValidationError(BulkAddDistrictsOverride,
              "Could not find an available code -- all 99 are in use");
            return;
          }

          // do the adds
          // create a linq that returns the sequence and the code
          //var isPhase2 = IsPhase2(StateCode);
          foreach (var add in
            Enumerable.Range(startSequence, endSequence - startSequence + 1)
              .Select((seq, inx) => new {seq, code = localCodes[inx]}))
          {
            var localDistrict = localDistrictTemplate.Replace("*", add.seq.ToString());
            LocalDistricts.Insert(StateCode, null, CountyCode, add.code, localDistrict,
              /*string.Empty,*/ string.Empty, string.Empty, string.Empty, string.Empty,
              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
              string.Empty, string.Empty, string.Empty, string.Empty, false/*, 
              string.Empty*/);
            //if (isPhase2)
            //  LocalIdsCodes.Insert(StateCode, LocalIdsCodes.LocalTypeVote, 
            //    LocalIdsCodes.GetNextVoteIdForState(StateCode), CountyCode, add.code, 
            //    null);
            FeedbackBulkAddDistricts.AddInfo(
            $"Local District {localDistrict} ({add.code}) added.");
          }
          _BulkAddDistrictsTabInfo.ClearValidationErrors();
          RefreshCurrentDistrictsList(BulkAddDistrictsCurrentDistricts);
          _BulkAddDistrictsTabInfo.LoadControls();
            NavigateJurisdictionUpdatePanel.Update();
            NavigateJurisdiction.Initialize();
          }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{BulkAddDistrictsReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}