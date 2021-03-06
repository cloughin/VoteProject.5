using System;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class StateDefaultsTabItem : DataItemBase
    {
      private readonly UpdateElectionsPage _Page;
      private const string GroupName = "StateDefaults";

      private StateDefaultsTabItem(UpdateElectionsPage page) : base(GroupName)
      {
        _Page = page;
      }

      protected override string GetCurrentValue()
      {
        var column = ElectionsDefaults.GetColumn(Column);
        var defaultElectionKey =
          Elections.GetDefaultElectionKeyFromKey(_Page.GetElectionKey());
        var value = ElectionsDefaults.GetColumn(column, defaultElectionKey);
        return value == null ? Empty : ToDisplay(value);
      }

      protected override bool Update(object newValue)
      {
        var column = ElectionsDefaults.GetColumn(Column);
        var defaultElectionKey =
          Elections.GetDefaultElectionKeyFromKey(_Page.GetElectionKey());
        ElectionsDefaults.UpdateColumn(column, newValue, defaultElectionKey);
        return true;
      }

      private static StateDefaultsTabItem[] GetTabInfo(UpdateElectionsPage page)
      {
        var changeInfoTabInfo = new[]
        {
          new StateDefaultsTabItem(page)
          {
            Column = "ElectionAdditionalInfo",
            Description = "Additional Election Information"
          },
          new StateDefaultsTabItem(page)
          {
            Column = "BallotInstructions",
            Description = "Special Ballot Instructions"
          },
          new StateDefaultsTabItem(page)
          {
            Column = "RegistrationDeadline",
            Description = "Registration Deadline",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new StateDefaultsTabItem(page)
          {
            Column = "EarlyVotingBegin",
            Description = "Early Voting Begin Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new StateDefaultsTabItem(page)
          {
            Column = "EarlyVotingEnd",
            Description = "Early Voting End Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new StateDefaultsTabItem(page)
          {
            Column = "MailBallotBegin",
            Description = "Mail Ballot Begin Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new StateDefaultsTabItem(page)
          {
            Column = "MailBallotEnd",
            Description = "Mail Ballot End Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new StateDefaultsTabItem(page)
          {
            Column = "MailBallotDeadline",
            Description = "Mail Ballot Must Be Received By",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new StateDefaultsTabItem(page)
          {
            Column = "AbsenteeBallotBegin",
            Description = "Absentee Ballot Begin Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new StateDefaultsTabItem(page)
          {
            Column = "AbsenteeBallotEnd",
            Description = "Absentee Ballot End Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          },
          new StateDefaultsTabItem(page)
          {
            Column = "AbsenteeBallotDeadline",
            Description = "Absentee Ballot Must Be Received By",
            ConvertFn = ToDateTime,
            Validator = ValidateDateOptional
          }
        };

        foreach (var item in changeInfoTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return changeInfoTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._StateDefaultsTabInfo = GetTabInfo(page);

        page.ShowStateDefaults = page.TabStateDefaultsItem.Visible =
          page.AdminPageLevel == AdminPageLevel.State;
      }
    }

    private StateDefaultsTabItem[] _StateDefaultsTabInfo;

    #endregion DataItem object

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonStateDefaults_OnClick(object sender, EventArgs e)
    {
      var electionKey = GetElectionKey();
      switch (StateDefaultsReloading.Value)
      {
        case "reloading":
        {
          StateDefaultsReloading.Value = Empty;
          _StateDefaultsTabInfo.LoadControls();
          var heading = "Defaults for all " +
            StateCache.GetStateName(Elections.GetStateCodeFromKey(GetElectionKey())) +
            " elections on " + Elections.GetElectionDateFromKey(GetElectionKey())
              .ToString("d");
          HeadingStateDefaults.InnerHtml = heading;
          FeedbackStateDefaults.AddInfo("Election defaults loaded.");
        }
          break;

        case "":
        {
          // normal update
          _StateDefaultsTabInfo.ClearValidationErrors();
          var originalDesc = Elections.GetElectionDesc(electionKey);
          _StateDefaultsTabInfo.Update(FeedbackStateDefaults);

          if (originalDesc != Elections.GetElectionDesc(electionKey))
          {
            SetElectionHeading(HeadingStateDefaults);
            ReloadElectionControl();
          }
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{StateDefaultsReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}