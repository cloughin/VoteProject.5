using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using DB.VoteLog;
using static System.String;

namespace Vote.Controls
{
  public partial class ManagePoliticiansPanel
  {
    #region DataItem objects

    private class AddNewCandidateSubTabItem : DataItemBase
    {
      private const string GroupName = "AddNewCandidate";

      private AddNewCandidateSubTabItem() : base(GroupName)
      {
      }

      protected override string GetCurrentValue()
      {
        return GetControlValue();
      }

      public static AddNewCandidateSubTabItem[] GetSubTabInfo(TemplateControl control,
        FeedbackContainerControl feedback)
      {
        var addNewCandidateTabInfo = new[]
        {
          new AddNewCandidateSubTabItem
          {
            Column = "FName",
            Description = "First Name",
            Validator = ValidateFirstName
          },
          new AddNewCandidateSubTabItem
          {
            Column = "MName",
            Description = "Middle Name",
            Validator = ValidateMiddleName
          },
          new AddNewCandidateSubTabItem
          {
            Column = "Nickname",
            Description = "Nickname",
            Validator = ValidateNickname
          },
          new AddNewCandidateSubTabItem
          {
            Column = "LName",
            Description = "Last Name",
            Validator = ValidateLastName
          },
          new AddNewCandidateSubTabItem
          {
            Column = "Suffix",
            Description = "Suffix",
            Validator = ValidateSuffix
          },
          new AddNewCandidateSubTabItem
          {
            Column = "StateCode",
            Description = "State Code",
            Validator = ValidateRequired
          }
        };

        foreach (var item in addNewCandidateTabInfo)
          item.InitializeItem(control, true, feedback);

        InitializeGroup(control, GroupName);

        return addNewCandidateTabInfo;
      }

      protected override void Log(string oldValue, string newValue)
      {
        // no logging
      }

      protected override bool Update(object newValue)
      {
        // no updating
        return false;
      }
    }

    private AddNewCandidateSubTabItem[] _AddNewCandidateSubTabInfo;

    private class AddCandidatesTabItem : DataItemBase
    {
      // This class assumes CandidateList is the only column
      // The containing GroupName must be "AddCandidates"
      private const string GroupName = "AddCandidates";

      private readonly ManagePoliticiansPanel _ThisControl;

      private AddCandidatesTabItem(ManagePoliticiansPanel thisControl) : base(GroupName)
      {
        _ThisControl = thisControl;
      }

      private static bool ValidateIncumbents(DataItemBase item)
      {
        var tabItem = item as AddCandidatesTabItem;
        Debug.Assert(tabItem != null, nameof(tabItem) + " != null");
        if (tabItem._ThisControl.Mode != DataMode.ManageCandidates) return true;

        // make sure there aren't too many incumbents
        var newCandidates = UpdateParse(tabItem.GetControlValue());
        var officeKey = tabItem._ThisControl.SafeGetOfficeKey();
        var checkedIncumbents = newCandidates.Count(c => c.ShowAsIncumbent);
        var allowedIncumbents = Offices.GetIncumbents(officeKey, 0);
        if (checkedIncumbents <= allowedIncumbents) return true;
        item.Feedback.AddError($"Warning: There are too many incumbents: {checkedIncumbents} checked, " +
          $"{allowedIncumbents} allowed");
        return true;
      }

      public static AddCandidatesTabItem[] GetTabInfo(ManagePoliticiansPanel control,
        FeedbackContainerControl feedback)
      {
        var addCandidatesTabInfo = new[]
        {
          new AddCandidatesTabItem(control)
          {
            Column = "CandidateList",
            Description = control.Mode == DataMode.ManageIncumbents
              ? "Incumbent List"
              : "Candidate List",
            Validator = ValidateIncumbents
          }
        };

        foreach (var item in addCandidatesTabInfo)
          item.InitializeItem(control, true, feedback);

        // handled by outer page
        //InitializeGroup(control, GroupName);

        return addCandidatesTabInfo;
      }

      protected override string GetCurrentValue()
      {
        return null;
      }

      public override void LoadControl()
      {
        _ThisControl.IncumbentCountMessage.Visible = false;
        _ThisControl.IncumbentCount.Visible = false;
        switch (_ThisControl.Mode)
        {
          case DataMode.AddPoliticians: break;

          case DataMode.ManageCandidates:
            LoadControlManageCandidates();
            break;

          case DataMode.ManageIncumbents:
            LoadControlManageIncumbents();
            break;
        }
      }

      private void LoadControlManageCandidates()
      {
        var electionKey = _ThisControl.SafeGetElectionKey();
        var officeKey = _ThisControl.SafeGetOfficeKey();

        var table = Elections.GetOneElectionOffice(electionKey, officeKey);
        var candidates = table.Rows.Cast<DataRow>().Where(row => !row.IsRunningMate())
          .OrderBy(row => row.OrderOnBallot()).ThenBy(row => row.PoliticianKey(),
            StringComparer.OrdinalIgnoreCase).ToList();

        _ThisControl.PageFeedback.AddInfo(
          $"{candidates.Count} candidate{candidates.Count.Plural()} loaded.");
        if (candidates.Count == 0)
        {
          _ThisControl.Message.RemoveCssClass("hidden");
          _ThisControl.Message.InnerHtml = "No candidates were found for this office.";
        }
        else
        {
          _ThisControl.Message.AddCssClasses("hidden");
          foreach (var candidate in candidates)
          {
            var li = new HtmlLi
            {
              ID = "candidate-" + candidate.PoliticianKey(),
              ClientIDMode = ClientIDMode.Static
            }.AddTo(_ThisControl.ControlAddCandidatesCandidateList);
            var outerDiv = new HtmlDiv().AddTo(li, "outer shadow-2");
            CreateCandidateEntry(candidate, DataMode.ManageCandidates).AddTo(outerDiv);
            var runningMateKey = candidate.RunningMateKey();
            //if (!candidate.IsRunningMateOffice() ||
            //  Elections.IsPrimaryElection(electionKey)) continue;
            if (!Offices.IsRunningMateOfficeInElection(electionKey, officeKey)) continue;
            DataRow runningMate = null;
            if (!IsNullOrWhiteSpace(runningMateKey))
              runningMate = table.Rows.Cast<DataRow>()
                .FirstOrDefault(row => row.PoliticianKey().IsEqIgnoreCase(runningMateKey));
            if (runningMate == null) CreateNoRunningMateEntry().AddTo(outerDiv);
            else
              CreateCandidateEntry(runningMate, DataMode.ManageCandidates,
                candidate.PartyCode(), false).AddTo(outerDiv);
          }
        }
      }

      private void LoadControlManageIncumbents()
      {
        var officeKey = _ThisControl.SafeGetOfficeKey();

        var table = OfficesOfficials.GetIncumbentsForOffice(officeKey);
        int incumbentsAllowed;
        var incumbents = table.Rows.Cast<DataRow>() // these are pre-sorted
          .Where(row => !row.IsRunningMate()).ToList();
        _ThisControl.PageFeedback.AddInfo(
          $"{incumbents.Count} incumbent{incumbents.Count.Plural()} loaded.");
        if (incumbents.Count == 0)
        {
          if (IsNullOrWhiteSpace(officeKey))
          {
            _ThisControl.Message.AddCssClasses("hidden");
            _ThisControl.Message.InnerHtml = Empty;
          }
          else
          {
            _ThisControl.Message.RemoveCssClass("hidden");
            _ThisControl.Message.InnerHtml = "No incumbents were found for this office.";
          }
          incumbentsAllowed = Offices.GetIncumbents(officeKey, 0);
        }
        else
        {
          _ThisControl.Message.AddCssClasses("hidden");
          foreach (var incumbent in incumbents)
          {
            var li = new HtmlLi
            {
              ID = "candidate-" + incumbent.PoliticianKey(),
              ClientIDMode = ClientIDMode.Static
            }.AddTo(_ThisControl.ControlAddCandidatesCandidateList);
            var outerDiv = new HtmlDiv().AddTo(li, "outer shadow-2");
            CreateCandidateEntry(incumbent, DataMode.ManageIncumbents, null, false).AddTo(outerDiv);
            var runningMateKey = incumbent.RunningMateKey();
            if (!incumbent.IsRunningMateOffice()) continue;
            DataRow runningMate = null;
            if (!IsNullOrWhiteSpace(runningMateKey))
              runningMate = table.Rows.Cast<DataRow>()
                .FirstOrDefault(row => row.PoliticianKey().IsEqIgnoreCase(runningMateKey));
            if (runningMate == null) CreateNoRunningMateEntry().AddTo(outerDiv);
            else
              CreateCandidateEntry(runningMate, DataMode.ManageIncumbents,
                incumbent.PartyCode(), false).AddTo(outerDiv);
          }
          incumbentsAllowed = incumbents.First().Incumbents();
        }
        if (incumbentsAllowed > 0)
        {
          _ThisControl.IncumbentCountMessage.InnerText = incumbentsAllowed == 1
            ? "This office can have only a single incumbent"
            : $"This office can have {incumbentsAllowed} incumbents";
          _ThisControl.IncumbentCountMessage.Visible = true;
        }
        else
        {
          _ThisControl.IncumbentCountMessage.InnerText = Empty;
          _ThisControl.IncumbentCountMessage.Visible = false;
        }
        _ThisControl.IncumbentCount.Value =
          incumbentsAllowed.ToString(CultureInfo.InvariantCulture);
        _ThisControl.IncumbentCount.Visible = true;
      }

      protected override void Log(string oldValue, string newValue)
      {
        // Logging done in Update
      }

      protected override bool Update(object newValue)
      {
        switch (_ThisControl.Mode)
        {
          case DataMode.ManageCandidates: return UpdateManageCandidates(newValue);

          case DataMode.ManageIncumbents: return UpdateManageIncumbents(newValue);

          default: return false;
        }
      }

      private class UpdateParsed
      {
        public string PoliticianKey;
        public string RunningMateKey;
        public bool ShowAsIncumbent;
      }

      private static IEnumerable<UpdateParsed> UpdateParse(object newValue)
      {
        // parse the new value
        // syntax:
        //   candidate = prefix-<politicianKey>[+<runningMateKey][*] <== * = show as incumbent
        //   candidates = <candidate>[|<candidate>]*
        var value = newValue as string;
        Debug.Assert(value != null, "value != null");
        var newCandidates = value.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries)
          .Select(candidate =>
          {
            var showAsIncumbent = false;
            if (candidate.EndsWith("*", StringComparison.Ordinal))
            {
              showAsIncumbent = true;
              candidate = candidate.Substring(0, candidate.Length - 1);
            }
            var ids = candidate.Split('+');
            return new UpdateParsed
            {
              PoliticianKey = ids[0].Substring(ids[0].LastIndexOf('-') + 1),
              RunningMateKey = ids.Length == 2 ? ids[1] : Empty,
              ShowAsIncumbent = showAsIncumbent
            };
          });
        return newCandidates;
      }

      private bool UpdateManageCandidates(object newValue)
      {
        var electionKey = _ThisControl.SafeGetElectionKey();
        var officeKey = _ThisControl.SafeGetOfficeKey();
        var newCandidates = UpdateParse(newValue).ToList();

        // Get the current slate of candidate for this election/office
        var currentCandidatesTable =
          ElectionsPoliticians.GetDataByElectionKeyOfficeKey(electionKey, officeKey);

        // If we process a row, we delete it from this list. What's left needs
        // to be deleted from the DB.
        var rowsToDelete = Enumerable.Select(currentCandidatesTable, row => row).ToList();

        var orderOnBallot = 0;
        var federalCode = Offices.GetOfficeClass(officeKey).StateCodeProxy();
        var stateCode = Elections.GetStateCodeFromKey(electionKey);
        if (StateCache.IsValidFederalCode(stateCode, false)) stateCode = Empty;
        var countyCode = Elections.GetCountyCodeFromKey(electionKey);
        var localKey = Elections.GetLocalKeyFromKey(electionKey);
        var electionKeyFederal = IsNullOrWhiteSpace(federalCode)
          ? Empty
          : Elections.GetFederalElectionKeyFromKey(electionKey, federalCode);
        var electionKeyState = Elections.GetStateElectionKeyFromKey(electionKey);
        foreach (var candidate in newCandidates)
        {
          orderOnBallot += 10;
          var currentRow =
            currentCandidatesTable.FirstOrDefault(
              row => row.PoliticianKey.IsEqIgnoreCase(candidate.PoliticianKey));
          if (currentRow == null)
          {
            // new candidate, add
            LogDataChange.LogInsert(ElectionsPoliticians.TableName,
              candidate.RunningMateKey, DateTime.UtcNow, electionKey, officeKey,
              candidate.PoliticianKey);
            currentCandidatesTable.AddRow(electionKey, officeKey, candidate.PoliticianKey,
              candidate.RunningMateKey, electionKeyState, electionKeyFederal, stateCode,
              countyCode, localKey, Empty, orderOnBallot, false,
              candidate.ShowAsIncumbent, false, Empty, null, null, null, Empty, false, VotePage.DefaultDbDate,
              Empty, Empty, false);
          }
          else
          {
            // existing candidate, update if necessary
            if (currentRow.RunningMateKey.IsNeIgnoreCase(candidate.RunningMateKey))
            {
              LogDataChange.LogUpdate(ElectionsPoliticians.Column.RunningMateKey,
                currentRow.RunningMateKey, candidate.RunningMateKey, DateTime.UtcNow,
                electionKey, officeKey, candidate.PoliticianKey);
              currentRow.RunningMateKey = candidate.RunningMateKey;
            }
            if (currentRow.OrderOnBallot != orderOnBallot)
            {
              LogDataChange.LogUpdate(ElectionsPoliticians.Column.OrderOnBallot,
                currentRow.OrderOnBallot, orderOnBallot, DateTime.UtcNow, electionKey,
                officeKey, candidate.PoliticianKey);
              currentRow.OrderOnBallot = orderOnBallot;
            }
            if (currentRow.IsIncumbent != candidate.ShowAsIncumbent)
            {
              LogDataChange.LogUpdate(ElectionsPoliticians.Column.IsIncumbent,
                currentRow.IsIncumbent, candidate.ShowAsIncumbent, DateTime.UtcNow, electionKey,
                officeKey, candidate.PoliticianKey);
              currentRow.IsIncumbent = candidate.ShowAsIncumbent;
            }
            rowsToDelete.Remove(currentRow);
          }
        }

        foreach (var row in rowsToDelete)
        {
          LogDataChange.LogDelete(ElectionsPoliticians.TableName, DateTime.UtcNow,
            electionKey, officeKey, row.PoliticianKey);
          row.Delete();
        }

        // Update if any changes
        var candidateListChanged =
          currentCandidatesTable.FirstOrDefault(row => row.RowState !=
            DataRowState.Unchanged) != null;
        if (candidateListChanged) ElectionsPoliticians.UpdateTable(currentCandidatesTable);

        LoadControl();
        return candidateListChanged;
      }

      private bool UpdateManageIncumbents(object newValue)
      {
        var officeKey = _ThisControl.SafeGetOfficeKey();
        var newIncumbents = UpdateParse(newValue);

        // Get the current slate of incumbents for this office
        var currentIncumbentsTable = OfficesOfficials.GetDataByOfficeKey(officeKey);

        // If we process a row, we delete it from this list. What's left needs
        // to be deleted from the DB.
        var rowsToDelete = Enumerable.Select(currentIncumbentsTable, row => row).ToList();

        var stateCode = Offices.GetStateCodeFromKey(officeKey);
        var countyCode = Offices.GetCountyCodeFromKey(officeKey);
        var localKey = Offices.GetLocalKeyFromKey(officeKey);
        foreach (var incumbent in newIncumbents)
        {
          var currentRow =
            currentIncumbentsTable.FirstOrDefault(
              row => row.PoliticianKey.IsEqIgnoreCase(incumbent.PoliticianKey));
          if (currentRow == null)
          {
            // new incumbent, add
            LogDataChange.LogInsert(OfficesOfficials.TableName, incumbent.RunningMateKey,
              DateTime.UtcNow, officeKey, incumbent.PoliticianKey);
            currentIncumbentsTable.AddRow(officeKey, incumbent.PoliticianKey,
              incumbent.RunningMateKey, stateCode, countyCode, localKey,
              Offices.GetDistrictCode(officeKey), /*String.Empty, VotePage.DefaultDbDate,*/
              DateTime.UtcNow, SecurePage.AdminSecurityClass, VotePage.UserName);
          }
          else
          {
            // existing incumbent, update if necessary
            if (currentRow.RunningMateKey.IsNeIgnoreCase(incumbent.RunningMateKey))
            {
              LogDataChange.LogUpdate(OfficesOfficials.Column.RunningMateKey,
                currentRow.RunningMateKey, incumbent.RunningMateKey, DateTime.UtcNow,
                officeKey, incumbent.PoliticianKey);
              currentRow.RunningMateKey = incumbent.RunningMateKey;
            }
            rowsToDelete.Remove(currentRow);
          }
        }

        foreach (var row in rowsToDelete)
        {
          LogDataChange.LogDelete(OfficesOfficials.TableName, DateTime.UtcNow, officeKey,
            row.PoliticianKey);
          row.Delete();
        }

        // Update if any changes
        var incumbentListChanged =
          currentIncumbentsTable.FirstOrDefault(row => row.RowState !=
            DataRowState.Unchanged) != null;
        if (incumbentListChanged) OfficesOfficials.UpdateTable(currentIncumbentsTable);

        LoadControl();
        return incumbentListChanged;
      }
    }

    private AddCandidatesTabItem[] _AddCandidatesTabInfo;

    #endregion DataItem objects

    public void ClearAddNewCandidate(bool clearErrors = false)
    {
      if (clearErrors) _AddNewCandidateSubTabInfo.ClearValidationErrors();
      ControlAddNewCandidateFName.SetValue(Empty);
      ControlAddNewCandidateMName.SetValue(Empty);
      ControlAddNewCandidateNickname.SetValue(Empty);
      ControlAddNewCandidateLName.SetValue(Empty);
      ControlAddNewCandidateSuffix.SetValue(Empty);
      var stateCode = Mode == DataMode.AddPoliticians
        ? VotePage.GetPage<SecureAdminPage>().StateCode
        : Offices.GetValidatedStateCodeFromKey(SafeGetOfficeKey());
      ControlAddNewCandidateStateCode.SetValue(stateCode);
      ControlAddNewCandidateStateCode.Enabled = !StateCache.IsValidStateCode(stateCode);
    }

    #region Event handlers and overrides

    protected void ButtonAddNewCandidate_OnClick(object sender, EventArgs e)
    {
      bool.TryParse(AddCandidateValidateDuplicates.GetValue(), out var validateDuplicates);

      AddCandidateDuplicatesHtml.Controls.Clear();
      _AddNewCandidateSubTabInfo.ClearValidationErrors();
      AddCandidateNewId.SetValue(Empty);

      // No actual updating here, just validation and reformatting
      _AddNewCandidateSubTabInfo.Update(FeedbackAddNewCandidate, false);
      if (FeedbackAddNewCandidate.ValidationErrorCount > 0) return;

      var stateCode = ControlAddNewCandidateStateCode.GetValue();
      var firstName = ControlAddNewCandidateFName.GetValue();
      var middleName = ControlAddNewCandidateMName.GetValue();
      var nickname = ControlAddNewCandidateNickname.GetValue();
      var lastName = ControlAddNewCandidateLName.GetValue();
      var suffix = ControlAddNewCandidateSuffix.GetValue();

      var formattedName =
        Politicians.FormatName(firstName, middleName, nickname, lastName, suffix);

      if (validateDuplicates)
      {
        var duplicatesHtml =
          Politicians.GetCandidateList(lastName, null, stateCode, null, true);
        AddCandidateDuplicatesHtml.Controls.Add(duplicatesHtml);
        if (duplicatesHtml.Controls.Count > 0)
        {
          // Set up the duplicates dialog
          AddCandidateFormattedName.SetValue(formattedName);
          AddCandidateStateName.SetValue(StateCache.GetStateName(stateCode));
          FeedbackAddNewCandidate.PostValidationError(ControlAddNewCandidateLName,
            "Potential duplicate politician");
          return;
        }
      }

      var newPoliticianKey =
        Politicians.GetUniqueKey(stateCode, lastName, firstName, middleName, suffix);
      AddCandidateNewId.SetValue(newPoliticianKey);

      // If it's a primary, get the party key from the election
      var partyKey = Empty; // mantis 508
      var electionKey = SafeGetElectionKey();
      if (Elections.IsPrimaryElection(electionKey))
        partyKey = stateCode + Elections.GetNationalPartyCodeFromKey(electionKey);

      Politicians.AddPolitician(newPoliticianKey, firstName, middleName, nickname, lastName,
        suffix, partyKey, SecurePage.CreateUniquePassword());

      LogDataChange.LogInsert(Politicians.TableName, VotePage.UserName,
        SecurePage.UserSecurityClass, DateTime.UtcNow, newPoliticianKey);

      ClearAddNewCandidate();

      FeedbackAddNewCandidate.AddInfo("Politician " + formattedName + " was added.");
      if (Mode == DataMode.AddPoliticians)
      {
        FeedbackAddNewCandidate.AddInfo(new HtmlAnchor
        {
          InnerText = "Intro Page",
          HRef = UrlManager.GetIntroPageUri(newPoliticianKey).ToString(),
          Target = "Politician"
        }.RenderToString());
        FeedbackAddNewCandidate.AddInfo(new HtmlAnchor
        {
          InnerText = "Politician Admin Page",
          HRef = SecurePoliticianPage.GetUpdateIntroPageUrl(newPoliticianKey),
          Target = "Politician"
        }.RenderToString());
      }
    }

    #endregion Event handlers and overrides
  }
}