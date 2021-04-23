using System;
using System.Collections.Generic;
using System.Linq;
using DB.Vote;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    private enum MasterOnlySubTab
    {
      ChangeDate,
      IncludeElection,
      CopyCandidates,
      CopyPrimaryWinners,
      CopyRunoffAdvancers,
      DeleteElection,
      CreateGeneral,
      VotersEdge,
      StatusNotes
    }

    private List<MasterOnlySubTab> _MasterOnlySubTabs;

    #region DataItem object

    [PageInitializer]
    private class MasterOnlyTabItem : ElectionsDataItem
    {
      private const string GroupName = "MasterOnly";
      public MasterOnlySubTab SubTab { get; private set; }

      protected MasterOnlyTabItem(UpdateElectionsPage page) : base(page, GroupName)
      {
      }

      private static MasterOnlyTabItem[] GetTabInfo(UpdateElectionsPage page)
      {
        var masterOnlyTabInfo = new[]
        {
          new MasterOnlyTabItem(page)
          {
            Column = "ElectionDesc",
            Description = "Election Title",
            Validator = ValidateDescription,
            SubTab = MasterOnlySubTab.ChangeDate
          },
          new MasterOnlyElectionDateTabItem(page)
          {
            Column = "ElectionDate",
            Description = "Election Date",
            ConvertFn = ToDateTime,
            Validator = ValidateDate,
            SubTab = MasterOnlySubTab.ChangeDate
          },
          new MasterOnlyTabItem(page)
          {
            Column = "ElectionKeyToInclude",
            Description = "Election to Include",
            SubTab = MasterOnlySubTab.IncludeElection
          },
          new MasterOnlyElectionToCopyTabItem(page)
          {
            Column = "ElectionToCopy",
            Description = "Election to Copy",
            SubTab = MasterOnlySubTab.CopyCandidates
          },
          new MasterOnlyPrimaryWinnersCopyTabItem(page)
          {
            Column = "PrimaryDateToCopy",
            Description = "Primary Date to Copy",
            SubTab = MasterOnlySubTab.CopyPrimaryWinners
          },
          new MasterOnlyPrimaryWinnersCopyTabItem(page)
          {
            Column = "EnableCopyPrimaryWinners",
            Description = "Enable Update",
            ConvertFn = ToBool,
            SubTab = MasterOnlySubTab.CopyPrimaryWinners
          },
          new MasterOnlyPrimaryWinnersCopyTabItem(page)
          {
            Column = "RunoffDateToCopy",
            Description = "Runoff Date to Copy",
            SubTab = MasterOnlySubTab.CopyPrimaryWinners
          },
          new MasterOnlyRunoffAdvancersTabItem(page)
          {
            Column = "ElectionDateToCopy",
            Description = "Election Date to Copy",
            SubTab = MasterOnlySubTab.CopyRunoffAdvancers
          },
          new MasterOnlyDeleteElectionTabItem(page)
          {
            Column = "DeleteElection",
            Description = "Delete Election",
            ConvertFn = ToBool,
            SubTab = MasterOnlySubTab.DeleteElection
          },
          new MasterOnlyCreateGeneralTabItem(page)
          {
            Column = "GeneralElectionDate",
            Description = "Election Date",
            SubTab = MasterOnlySubTab.CreateGeneral
          },
          new MasterOnlyCreateGeneralTabItem(page)
          {
            Column = "GeneralElectionDesc",
            Description = "Election Title",
            Validator = ValidateDescription,
            SubTab = MasterOnlySubTab.CreateGeneral
          },
          new MasterOnlyCreateGeneralTabItem(page)
          {
            Column = "GeneralPastElection",
            Description = "Allow Past Election",
            SubTab = MasterOnlySubTab.CreateGeneral
          },
          new MasterOnlyCreateGeneralTabItem(page)
          {
            Column = "GeneralIncludePresident",
            Description = "Include US President",
            SubTab = MasterOnlySubTab.CreateGeneral
          },
          new MasterOnlyTabItem(page)
          {
            Column = "ElectionStatus",
            Description = "Status Notes",
            SubTab = MasterOnlySubTab.StatusNotes
          }
        };

        foreach (var item in masterOnlyTabInfo) item.InitializeItem(page);

        InitializeGroup(page, GroupName);

        return masterOnlyTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._MasterOnlyTabInfo = GetTabInfo(page);

        page._MasterOnlySubTabs = new List<MasterOnlySubTab>
        {
          MasterOnlySubTab.ChangeDate,
          MasterOnlySubTab.IncludeElection,
          MasterOnlySubTab.CopyCandidates,
          MasterOnlySubTab.CopyPrimaryWinners,
          MasterOnlySubTab.CopyRunoffAdvancers,
          MasterOnlySubTab.DeleteElection,
          MasterOnlySubTab.CreateGeneral,
          MasterOnlySubTab.VotersEdge,
          MasterOnlySubTab.StatusNotes
        };

        if (!IsMasterUser) page.TabMasterItem.Visible = false;

        if (page.AdminPageLevel != AdminPageLevel.State)
        {
          page.TabMasterOnlyChangeDate.Visible = false;
          page._MasterOnlySubTabs.Remove(MasterOnlySubTab.ChangeDate);
        }
        if (!IsMasterUser && page.AdminPageLevel != AdminPageLevel.State)
        {
          page.TabViewReportItem.Visible = false;
        }
      }
    }

    private class MasterOnlyElectionDateTabItem : MasterOnlyTabItem
    {
      internal MasterOnlyElectionDateTabItem(UpdateElectionsPage page) : base(page)
      {
      }

      protected override void Log(string oldValue, string newValue)
      {
      }

      protected override bool Update(object newValue) => false;
    }

    private class MasterOnlyElectionToCopyTabItem : MasterOnlyTabItem
    {
      internal MasterOnlyElectionToCopyTabItem(UpdateElectionsPage page) : base(page)
      {
      }

      protected override string GetCurrentValue() => Elections.GetElectionDesc(
        Page.MasterOnlyElectionKeyToCopy.Value, Empty);

      protected override void Log(string oldValue, string newValue)
      {
      }

      protected override bool Update(object newValue) => false;
    }

    private class MasterOnlyPrimaryWinnersCopyTabItem : MasterOnlyTabItem
    {
      internal MasterOnlyPrimaryWinnersCopyTabItem(UpdateElectionsPage page) : base(page)
      {
      }

      protected override string GetCurrentValue()
      {
        if (Column == "EnableCopyPrimaryWinners") return "false";
        return Elections.GetElectionDesc(Page.ControlMasterOnlyPrimaryDateToCopy.Text,
          Empty);
      }

      public override void LoadControl()
      {
        if (Column == "EnableCopyPrimaryWinners")
        {
          base.LoadControl();
          return;
        }
        DataControl.SetValue("");
      }

      protected override void Log(string oldValue, string newValue)
      {
      }

      protected override bool Update(object newValue) => false;
    }

    private class MasterOnlyRunoffAdvancersTabItem : MasterOnlyTabItem
    {
      internal MasterOnlyRunoffAdvancersTabItem(UpdateElectionsPage page) : base(page)
      {
      }

      protected override string GetCurrentValue() => Elections.GetElectionDesc(
        Page.ControlMasterOnlyElectionDateToCopy.Text, Empty);

      public override void LoadControl() => DataControl.SetValue("");

      protected override void Log(string oldValue, string newValue)
      {
      }

      protected override bool Update(object newValue) => false;
    }

    private class MasterOnlyDeleteElectionTabItem : MasterOnlyTabItem
    {
      internal MasterOnlyDeleteElectionTabItem(UpdateElectionsPage page) : base(page)
      {
      }

      protected override string GetCurrentValue() => "false";

      protected override void Log(string oldValue, string newValue)
      {
      }

      protected override bool Update(object newValue) => false;
    }

    private class MasterOnlyCreateGeneralTabItem : MasterOnlyTabItem
    {
      internal MasterOnlyCreateGeneralTabItem(UpdateElectionsPage page) : base(page)
      {
      }

      protected override string GetCurrentValue()
      {
        switch (Column)
        {
          case "GeneralPastElection":
          case "GeneralIncludePresident": return "false";
        }
        return Empty;
      }

      protected override void Log(string oldValue, string newValue)
      {
      }

      protected override bool Update(object newValue) => false;
    }

    private MasterOnlyTabItem[] _MasterOnlyTabInfo;

    #endregion DataItem object

    private void ChangeSelectedElectionDate(DateTime newElectionDate)
    {
      // Only for state elections -- changes county and local elections too
      try
      {
        var newElectionKey =
          Elections.ChangeElectionDate(GetElectionKey(), newElectionDate, 0);
        FeedbackMasterOnly.AddInfo("The election date was changed");
        SelectedElectionKey.Value = newElectionKey;
        SetElectionHeading(HeadingMasterOnly);
        ReloadElectionControl();
        ControlMasterOnlyElectionDesc.Enabled = true;
        MasterOnlyDateWasChanged.Value = "true";
      }
      catch (Exception ex)
      {
        FeedbackMasterOnly.PostValidationError(ControlMasterOnlyElectionDate,
          "The election date could not be changed: " + ex.Message);
      }
    }

    private void CopyCandidates()
    {
      var candidatesCopied = 0;
      var officesCopied = 0;
      var officesSkipped = 0;
      try
      {
        var offices = ElectionsOffices.GetDataByElectionKey(GetElectionKey());
        if (offices.Count == 0)
          throw new VoteException(
            "There are no offices for this election to copy. Use the Add/Remove Offices tab to add the offices you want to copy.");
        var electionKeyToCopy = MasterOnlyElectionKeyToCopy.Value;
        foreach (var office in offices)
        {
          if (ElectionsPoliticians.CountByElectionKeyOfficeKey(GetElectionKey(),
            office.OfficeKey) > 0)
          {
            officesSkipped++;
            continue;
          }
          var oldPoliticians =
            ElectionsPoliticians.GetDataByElectionKeyOfficeKey(electionKeyToCopy,
              office.OfficeKey);
          if (oldPoliticians.Count == 0) continue;
          officesCopied++;
          candidatesCopied += oldPoliticians.Count;
          var newPoliticians = new ElectionsPoliticiansTable();
          foreach (var oldPolitician in oldPoliticians)
            newPoliticians.AddRow(office.ElectionKey, office.OfficeKey,
              oldPolitician.PoliticianKey, oldPolitician.RunningMateKey,
              office.ElectionKeyState, office.ElectionKeyFederal, office.StateCode,
              office.CountyCode, office.LocalKey, office.DistrictCode,
              oldPolitician.OrderOnBallot, false,
              OfficesOfficials.OfficeKeyPoliticianKeyExists(office.OfficeKey,
                oldPolitician.PoliticianKey), 
              false, Empty, null, null, null, Empty,false, DefaultDbDate, Empty, Empty, false);
          ElectionsPoliticians.UpdateTable(newPoliticians);
        }
        FeedbackMasterOnly.AddInfo(officesCopied > 0
          ? $"{candidatesCopied} candidates were copied for {officesCopied} offices."
          : "There were no candidates that could be copied.");
        if (officesSkipped > 0)
          FeedbackMasterOnly.AddInfo(
            $"{officesSkipped} offices were skipped because there were already candidates entered.");
      }
      catch (Exception ex)
      {
        FeedbackMasterOnly.PostValidationError(ControlMasterOnlyElectionToCopy,
          "The candidates could not be copied: " + ex.Message);
      }
    }

    private void CopyPrimaryWinners()
    {
      try
      {
        var candidatesAdded = 0;
        var candidateDuplicates = 0;
        var generalElectionKey = GetElectionKey();
        if (Elections.IsPrimaryElection(generalElectionKey))
          throw new VoteException("This function may not be used with primary elections.");
        var generalElectionDate = Elections.GetElectionDateFromKey(generalElectionKey);
        var primaryElectionDate =
          FeedbackMasterOnly.ValidateDate(ControlMasterOnlyPrimaryDateToCopy, out var success,
            "Primary Election Date", generalElectionDate.AddYears(-1),
            generalElectionDate.AddDays(-7));
        if (!success) return;
        var runoffElectionDate =
          FeedbackMasterOnly.ValidateDateOptional(ControlMasterOnlyRunoffDateToCopy,
            out success, "Runoff Election Date", generalElectionDate.AddYears(-1),
            generalElectionDate.AddDays(-7), DefaultDbDate);
        if (!success) return;

        // for state elections we now copy for the whole ElectionKeyFamily (includes county and local)
        var family = Elections.GetVirtualElectionKeyFamily(generalElectionKey) ??
          new List<string> {generalElectionKey};

        foreach (var electionKey in family)
        {
          MergePrimaryWinners(electionKey, primaryElectionDate, false,
            ref candidateDuplicates,
            ref candidatesAdded);
          if (!runoffElectionDate.IsDefaultDate())
            MergePrimaryWinners(electionKey, runoffElectionDate, true,
              ref candidateDuplicates,
              ref candidatesAdded);
        }

        FeedbackMasterOnly.AddInfo(
          $"{candidatesAdded} candidates were added, {candidateDuplicates} were duplicates.");
      }
      catch (Exception ex)
      {
        FeedbackMasterOnly.PostValidationError(ControlMasterOnlyPrimaryDateToCopy,
          "The candidates could not be copied: " + ex.Message);
      }
    }

    private static void MergePrimaryWinners(string generalElectionKey,
      DateTime primaryElectionDate, bool isRunoff, ref int candidateDuplicates,
      ref int candidatesAdded)
    {
      var offices = ElectionsPoliticians
        .GetPrimaryWinnersForGeneralElection(generalElectionKey, primaryElectionDate,
          isRunoff).GroupBy(r => r.OfficeKey);
      foreach (var o in offices)
      {
        foreach (var c in o)
          if (ElectionsPoliticians.ElectionKeyOfficeKeyPoliticianKeyExists(
            generalElectionKey, c.OfficeKey, c.PoliticianKey)) candidateDuplicates++;
          else
          {
            Elections.ActualizeElection(generalElectionKey);
            ActualizeElectionOffice(generalElectionKey, c.OfficeKey);
            ElectionsPoliticians.Insert(generalElectionKey, c.OfficeKey, c.PoliticianKey,
              c.RunningMateKey, Elections.GetStateElectionKeyFromKey(generalElectionKey),
              Empty, c.StateCode, c.CountyCode, c.LocalKey, c.DistrictCode, 0, false,
              OfficesOfficials.OfficeKeyPoliticianKeyExists(c.OfficeKey, c.PoliticianKey),
              false, Empty, null, null, null, Empty,false, DefaultDbDate, Empty, Empty, false);
            candidatesAdded++;
          }
      }
    }

    private void CopyRunoffAdvancers()
    {
      try
      {
        var candidatesAdded = 0;
        var officesAdded = 0;
        var runoffElectionKey = GetElectionKey();
        if (!Elections.IsRunoffElection(runoffElectionKey))
          throw new VoteException("This function may only be used with runoff elections.");
        var runoffElectionDate = Elections.GetElectionDateFromKey(runoffElectionKey);
        var previousElectionDate =
          FeedbackMasterOnly.ValidateDate(ControlMasterOnlyElectionDateToCopy, out var success,
            "Previous Election Date", runoffElectionDate.AddYears(-1),
            runoffElectionDate.AddDays(-7));
        if (!success) return;
        var offices = ElectionsPoliticians
          .GetRunoffAdvancersForElection(runoffElectionKey, previousElectionDate)
          .GroupBy(r => r.OfficeKey);
        ElectionsOffices.DeleteByElectionKey(runoffElectionKey);
        ElectionsPoliticians.DeleteByElectionKey(runoffElectionKey);
        foreach (var o in offices)
        {
          var office = o.First();
          InsertElectionOffice(runoffElectionKey, office.OfficeKey);
          officesAdded++;
          foreach (var c in o)
          {
            ElectionsPoliticians.Insert(runoffElectionKey, c.OfficeKey, c.PoliticianKey,
              c.RunningMateKey, Elections.GetStateElectionKeyFromKey(runoffElectionKey),
              Empty, c.StateCode, c.CountyCode, c.LocalKey, c.DistrictCode, 0, false,
              OfficesOfficials.OfficeKeyPoliticianKeyExists(c.OfficeKey, c.PoliticianKey),
              false, Empty, null, null, null, Empty,false, DefaultDbDate, Empty, Empty, false);
            candidatesAdded++;
          }
        }
        FeedbackMasterOnly.AddInfo(
          $"{candidatesAdded} candidates were added, {officesAdded} offices were added.");
      }
      catch (Exception ex)
      {
        FeedbackMasterOnly.PostValidationError(ControlMasterOnlyPrimaryDateToCopy,
          "The candidates could not be copied: " + ex.Message);
      }
    }

    private void CreateGeneralElection()
    {
      try
      {
        var electionDate = ValidateElectionDate(ControlMasterOnlyGeneralElectionDate,
          FeedbackMasterOnly, ControlMasterOnlyGeneralPastElection.Checked, out var success);
        if (!success) return;

        var existingElections = Elections.GetStateGeneralElectionsByDate(electionDate);
        var existingStates = existingElections.Select(Elections.GetStateCodeFromKey)
          .OrderBy(code => code).ToList();
        var includePresident = ControlMasterOnlyGeneralIncludePresident.Checked;
        var electionDescTemplate = ControlMasterOnlyGeneralElectionDesc.GetValue().Trim();
        if (IsNullOrWhiteSpace(electionDescTemplate))
          electionDescTemplate =
            Elections.GetGeneralElectionDescriptionTemplate(electionDate);

        var electionsTable = new ElectionsTable();
        var electionsOfficesTable = new ElectionsOfficesTable();
        var generalElectionOffices = Offices.GetGeneralElectionOffices();

        var statesCreated = 0;

        // create the elections and (optionally) the presidential contests
        foreach (var code in StateCache.All51StateCodes.Union(StateCache.AllFederalCodes))
          if (!existingStates.Contains(code))
          {
            var electionKey = Elections.FormatElectionKey(electionDate,
              Elections.ElectionTypeGeneralElection, Parties.NationalPartyAll, code);
            var electionDesc =
              GetElectionDescriptionFromTemplate(electionDescTemplate, code, electionDate);

            if (includePresident || !StateCache.IsUSPresident(code))
            {
              InsertElection(electionsTable, electionKey, null, electionDesc);
              statesCreated++;
            }

            if (includePresident && StateCache.IsValidStateCode(code))
              InsertOffice(electionsOfficesTable, electionKey, Offices.USPresident,
                OfficeClass.USPresident, Empty);
          }

        // add all the offices
        foreach (var row in generalElectionOffices)
        {
          var stateCode = Offices.GetStateCodeFromKey(row.OfficeKey);
          if (StateCache.IsValidStateCode(stateCode) && !existingStates.Contains(stateCode))
            InsertOffice(electionsOfficesTable,
              Elections.FormatElectionKey(electionDate,
                Elections.ElectionTypeGeneralElection, Parties.NationalPartyAll, stateCode),
              row.OfficeKey, row.OfficeLevel.ToOfficeClass(), row.DistrictCode);
        }

        Elections.UpdateElectionsAndOffices(electionsTable, electionsOfficesTable, null);

        ReloadElectionControl();
        var msg =
          $"General elections were created for {statesCreated} states and pseudo-states.";
        if (existingStates.Count > 0)
          msg +=
            $" There were already general elections on this date for the following states: {Join(", ", existingStates)}.";
        FeedbackMasterOnly.AddInfo(msg);
        ResetMasterOnlySubTab(MasterOnlySubTab.CreateGeneral);
      }
      catch (Exception ex)
      {
        FeedbackMasterOnly.PostValidationError(ControlMasterOnlyGeneralElectionDate,
          "The general election could not be created: " + ex.Message);
      }
    }

    private void DeleteSelectedElection()
    {
      try
      {
        var electionKey = GetElectionKey();
        if (Elections.IsStateElection(electionKey))
          Elections.DeleteElectionFamily(electionKey);
        else Elections.DeleteElection(electionKey);
        FeedbackMasterOnly.AddInfo("The election was deleted");
        ControlMasterOnlyDeleteElection.Checked = false;
        SelectedElectionKey.Value = Empty;
        ReloadElectionControl();
      }
      catch (Exception ex)
      {
        FeedbackMasterOnly.PostValidationError(ControlMasterOnlyDeleteElection,
          "The election could not be deleted: " + ex.Message);
      }
    }

    private static string GetElectionDescriptionFromTemplate(string template,
      string stateCode, DateTime electionDate)
    {
      switch (stateCode)
      {
        case "U1":
          return electionDate.ToString("MMMM d, yyyy") +
            " General Election of U.S. President State-By-State";

        case "U2":
          return electionDate.ToString("MMMM d, yyyy") +
            " General Election of U.S. Senate State-By-State";

        case "U3":
          return electionDate.ToString("MMMM d, yyyy") +
            " General Election of U.S. House of Representatives State-By-State";

        case "U4":
          return electionDate.ToString("MMMM d, yyyy") +
            " General Election of Governors State-By-State";

        default: return template.Replace("{StateName}", StateCache.GetStateName(stateCode));
      }
    }

    private void ResetMasterOnlySubTab(MasterOnlySubTab subTab)
    {
      foreach (var item in _MasterOnlyTabInfo)
        if (item.SubTab == subTab) item.ResetControl();
    }

    #endregion Private

    #region Event handlers and overrides

    protected void ButtonMasterOnly_OnClick(object sender, EventArgs e)
    {
      switch (MasterOnlyReloading.Value)
      {
        case "reloading":
        {
          MasterOnlyReloading.Value = Empty;
          ControlMasterOnlyElectionDesc.Enabled = false;
          MasterOnlyDateWasChanged.Value = Empty;
          var electionKey = GetElectionKey();
          ControlMasterOnlyElectionKeyToInclude.Items.Clear();
          ControlMasterOnlyElectionKeyToInclude.AddItem("<No election included>", " ");
          if (!IsNullOrWhiteSpace(electionKey))
          {
            var table = Elections.GetElectionsOnSameDate(electionKey)
              .Where(r => r.ElectionKey != electionKey).OrderBy(r => r.ElectionDesc);
            foreach (var row in table)
              ControlMasterOnlyElectionKeyToInclude.AddItem(row.ElectionDesc,
                row.ElectionKey);
          }
          if (!Elections.IsPrimaryElection(electionKey))
          {
            // set the date to the most recent primary
            var date = Elections.GetPrimaryDateForGeneralElection(electionKey);
            if (!date.IsDefaultDate())
              HiddenMasterOnlyPrimaryDateToCopy.Value = date.ToString("MM/dd/yyyy");
          }
          if (Elections.IsRunoffElection(electionKey))
          {
            // set the date to the most recent matching election on type and party
            var date = Elections.GetElectionDateForRunoffElection(electionKey);
            if (!date.IsDefaultDate())
              HiddenMasterOnlyElectionDateToCopy.Value = date.ToString("MM/dd/yyyy");
          }
          SetElectionHeading(HeadingMasterOnly);
          _MasterOnlyTabInfo.LoadControls();
          FeedbackMasterOnly.AddInfo("Master-only data loaded.");
        }
          break;

        case "":
        {
          // normal update
          _MasterOnlyTabInfo.ClearValidationErrors();
          if (int.TryParse(ContainerMasterOnlySubTabIndex.Value, out var subTabIndex))
            switch (_MasterOnlySubTabs[subTabIndex])
            {
              case MasterOnlySubTab.ChangeDate:
                var newElectionDate =
                  FeedbackMasterOnly.ValidateDate(ControlMasterOnlyElectionDate,
                    out var success, "Election Date");
                if (!success) return;
                var oldElectionDate = Elections.GetElectionDateFromKey(GetElectionKey());
                if (oldElectionDate != newElectionDate)
                {
                  ChangeSelectedElectionDate(newElectionDate);
                  _MasterOnlyTabInfo.LoadControls();
                }
                else
                {
                  _MasterOnlyTabInfo.Update(FeedbackMasterOnly);
                  SetElectionHeading(HeadingMasterOnly);
                  ReloadElectionControl();
                }
                break;

              case MasterOnlySubTab.CopyCandidates:
                CopyCandidates();
                break;

              case MasterOnlySubTab.CopyPrimaryWinners:
                CopyPrimaryWinners();
                _MasterOnlyTabInfo.Single(i => i.Column == "EnableCopyPrimaryWinners")
                  .DataControl.SetValue("false");
                break;

              case MasterOnlySubTab.CopyRunoffAdvancers:
                CopyRunoffAdvancers();
                break;

              case MasterOnlySubTab.DeleteElection:
                DeleteSelectedElection();
                break;

              case MasterOnlySubTab.CreateGeneral:
                CreateGeneralElection();
                break;

              case MasterOnlySubTab.IncludeElection:
              case MasterOnlySubTab.StatusNotes:
                _MasterOnlyTabInfo
                  .Where(tab => tab.SubTab == _MasterOnlySubTabs[subTabIndex])
                  .Update(FeedbackMasterOnly);
                break;
            }
        }
          break;

        default:
          throw new VoteException(
            $"Unknown reloading option: '{MasterOnlyReloading.Value}'");
      }
    }

    #endregion Event handlers and overrides
  }
}