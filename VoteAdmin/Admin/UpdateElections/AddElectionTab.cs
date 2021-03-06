using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using JetBrains.Annotations;
using static System.String;

namespace Vote.Admin
{
  public partial class UpdateElectionsPage
  {
    #region Private

    #region DataItem object

    [PageInitializer]
    private class AddElectionTabItem : ElectionsDataItem
    {
      private const string GroupName = "AddElection";

      private AddElectionTabItem(UpdateElectionsPage page) : base(page, GroupName)
      {
      }

      protected override string GetCurrentValue() => null;

      private static AddElectionTabItem[] GetTabInfo(UpdateElectionsPage page)
      {
        var addElectionTabInfo = new[]
        {
          new AddElectionTabItem(page)
          {
            Column = "ElectionDate",
            Description = "Election Date"
          },
          new AddElectionTabItem(page)
          {
            Column = "PastElection",
            Description = "Allow Past Election"
          },
          new AddElectionTabItem(page)
          {
            Column = "ElectionType",
            Description = "Election Type"
          },
          new AddElectionTabItem(page) {Column = "NationalParty", Description = "Party"},
          new AddElectionTabItem(page)
          {
            Column = "IncludePresident",
            Description = "Include President"
          },
          new AddElectionTabItem(page)
          {
            Column = "IncludePresidentCandidates",
            Description = "Include Presidential Candidates"
          },
          new AddElectionTabItem(page)
          {
            Column = "CopyOffices",
            Description = "Copy Offices"
          },
          new AddElectionTabItem(page)
          {
            Column = "CopyCandidates",
            Description = "Copy Candidates"
          }
        };

        addElectionTabInfo.Initialize(page, GroupName);

        return addElectionTabInfo;
      }

      // ReSharper disable UnusedMember.Local
      // Invoked via Reflection
      internal static void Initialize(UpdateElectionsPage page)
        // ReSharper restore UnusedMember.Local
      {
        page._AddElectionTabInfo = GetTabInfo(page);

        page.ShowAddElections = StateCache.IsValidStateCode(page.StateCode) ||
          StateCache.IsPresidentialPrimary(page.StateCode);

        // I think this is correct. Non- state elections are always added
        // implicitly (I think)
        page.ShowAddElections &= IsNullOrWhiteSpace(page.CountyCode);

        if (!page.ShowAddElections) page.TabAddElectionItem.Visible = false;
        if (!page.IsPostBack)
        {
          Elections.PopulateElectionTypes(page.ControlAddElectionElectionType,
            page.StateCode);
          // We remove General Elections -- done under Master tab
          var generalElection = page.ControlAddElectionElectionType.Items.OfType<ListItem>()
            .FirstOrDefault(i => i.Value == Elections.ElectionTypeGeneralElection);
          if (generalElection != null)
            page.ControlAddElectionElectionType.Items.Remove(generalElection);
          // We remove PresidentialPrimary -- now included with State Primary
          var presidentialPrimary = page.ControlAddElectionElectionType.Items
            .OfType<ListItem>()
            .FirstOrDefault(i => i.Value == Elections.ElectionTypeStatePresidentialPrimary);
          if (presidentialPrimary != null)
            page.ControlAddElectionElectionType.Items.Remove(presidentialPrimary);
          // Relabel the State Primary to include Presidential
          var statePrimary = page.ControlAddElectionElectionType.Items.OfType<ListItem>()
            .FirstOrDefault(i => i.Value == Elections.ElectionTypeStatePrimary);
          if (statePrimary != null) statePrimary.Text = "State and/or Presidential Primary";
          // hide the Election Type if only 1 selection
          if (page.ControlAddElectionElectionType.Items.Count <= 1)
            page.AddElectionElectionType.AddCssClasses("hidden");
          else page.AddElectionElectionType.RemoveCssClass("hidden");
          var stateCodeForPartyLookup = page.StateCode == "PP" ? "US" : page.StateCode;
          Parties.PopulateMajorParties(page.ControlAddElectionNationalParty,
            stateCodeForPartyLookup, StateCache.IsValidStateCode(page.StateCode));
        }
      }

      protected override void Log(string oldValue, string newValue)
      {
      }

      protected override bool Update(object newValue) => false;
    }

    private AddElectionTabItem[] _AddElectionTabInfo;

    #endregion DataItem object

    private static void InsertCandidate(ElectionsPoliticiansTable electionsPoliticiansTable,
      string electionKey, string officeKey, string politicianKey, int orderOnBallot)
    {
      electionsPoliticiansTable.AddRow(electionKey, officeKey, politicianKey, Empty,
        Elections.GetStateElectionKeyFromKey(electionKey),
        Elections.GetFederalElectionKeyFromKey(electionKey,
          OfficeClass.USPresident.StateCodeProxy()),
        Elections.GetStateCodeFromKey(electionKey), Empty, Empty,
        Empty, orderOnBallot, false, false, false, Empty, null, null, null, Empty,false, DefaultDbDate,
        Empty, Empty, false);
    }

    private static void InsertElection(ElectionsTable electionsTable, string electionKey,
      [CanBeNull] string electionKeyToCopyDatesFrom = null, string electionDesc = null)
    {
      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      var electionDate = Elections.GetElectionDateFromKey(electionKey);
      var electionDateString = Elections.GetElectionDateStringFromKey(electionKey);
      if (electionDesc == null)
        electionDesc = Elections.FormatElectionDescription(electionKey);
      var electionType = Elections.GetElectionTypeFromKey(electionKey);
      var nationalPartyCode = Elections.GetNationalPartyCodeFromKey(electionKey);
      var partyCode = Parties.FormatPartyKey(stateCode, nationalPartyCode);
      var electionOrder = Parties.GetNationalPartyOrder(nationalPartyCode);

      // Dates
      var registrationDeadline = DefaultDbDate;
      var earlyVotingBegin = DefaultDbDate;
      var earlyVotingEnd = DefaultDbDate;
      var mailBallotBegin = DefaultDbDate;
      var mailBallotEnd = DefaultDbDate;
      var mailBallotDeadline = DefaultDbDate;
      var absenteeBallotBegin = DefaultDbDate;
      var absenteeBallotEnd = DefaultDbDate;
      var absenteeBallotDeadline = DefaultDbDate;
      var electionAdditionalInfo = Empty;
      //States.GetElectionAdditionalInfo(stateCode).SafeString();
      var ballotInstructions =
        Empty; //States.GetBallotInstructions(stateCode).SafeString();

      if (!IsNullOrWhiteSpace(electionKeyToCopyDatesFrom))
      {
        var electionToCopyDatesFrom = Elections.GetData(electionKeyToCopyDatesFrom);
        if (electionToCopyDatesFrom.Count == 1)
        {
          var row = electionToCopyDatesFrom[0];
          registrationDeadline = row.RegistrationDeadline;
          earlyVotingBegin = row.EarlyVotingBegin;
          earlyVotingEnd = row.EarlyVotingEnd;
          mailBallotBegin = row.MailBallotBegin;
          mailBallotEnd = row.MailBallotEnd;
          mailBallotDeadline = row.MailBallotDeadline;
          absenteeBallotBegin = row.AbsenteeBallotBegin;
          absenteeBallotEnd = row.AbsenteeBallotEnd;
          absenteeBallotDeadline = row.AbsenteeBallotDeadline;
          electionAdditionalInfo = row.ElectionAdditionalInfo;
          ballotInstructions = row.BallotInstructions;
        }
      }

      electionsTable.AddRow(electionKey, stateCode, Empty, Empty,
        electionDate, electionDateString, electionType, nationalPartyCode, partyCode,
        Empty, electionDesc, electionAdditionalInfo, Empty, DefaultDbDate,
        ballotInstructions, false, /*0, DefaultDbDate, 0, DefaultDbDate, 0,
        DefaultDbDate, 0, DefaultDbDate, 0,*/ Empty, electionOrder, false, false,
        registrationDeadline, earlyVotingBegin, earlyVotingEnd, mailBallotBegin,
        mailBallotEnd, mailBallotDeadline, absenteeBallotBegin, absenteeBallotEnd,
        absenteeBallotDeadline, null);
    }

    private static void InsertOffice(ElectionsOfficesTable electionsOfficesTable,
      string electionKey, string officeKey, OfficeClass officeClass, string districtCode)
    {
      var stateCode = Elections.GetStateCodeFromKey(electionKey);
      electionsOfficesTable.AddRow(electionKey, officeKey, electionKey,
        Elections.GetFederalElectionKeyFromKey(electionKey, officeClass.StateCodeProxy()),
        stateCode, Empty, Empty, districtCode, officeClass.ToInt(), false);
    }

    #endregion Private

    #region Event handlers and overrides

    private void PostAddElectionValidationError(string message)
    {
      FeedbackAddElection.PostValidationError(
        new Control[]
        {
          ControlAddElectionElectionDate, ControlAddElectionElectionType,
          ControlAddElectionNationalParty
        }, message);
    }

    protected void ButtonAddElection_OnClick(object sender, EventArgs e)
    {
      try
      {
        _AddElectionTabInfo.ClearValidationErrors();
        var electionType = ControlAddElectionElectionType.SelectedValue;
        var nationalPartyCode = ControlAddElectionNationalParty.SelectedValue;

        var electionDate = ValidateElectionDate(ControlAddElectionElectionDate,
          FeedbackAddElection, ControlAddElectionPastElection.Checked, out var success);
        if (!success) return;

        // if election type is not primary, the nationalPartyCode is always
        // NationalPartyAll
        if (Elections.IsPrimaryElectionType(electionType))
        {
          FeedbackAddElection.ValidateRequired(ControlAddElectionNationalParty, "Party",
            out success);
          if (!success) return;
        }
        else nationalPartyCode = Parties.NationalPartyAll;

        // special for Presidential candidates
        if (StateCode == "US" && electionType == "A" && nationalPartyCode == "A")
          electionType = "G";

        // This tab is now for state elections only
        var newElectionKey = Elections.FormatElectionKey(electionDate, electionType,
          nationalPartyCode, StateCode);

        // Make sure the election isn't a duplicate
        if (Elections.ElectionKeyExists(newElectionKey))
        {
          PostAddElectionValidationError("This election already exists.");
          return;
        }

        // for ElectionTypeStatePrimary, we need to check party conflicts:
        //   ● if it's NationalPartyNonPartisan, there can't be other party primaries
        //     on the same day
        //   ● if it's not NationalPartyNonPartisan, there can't be a NationalPartyNonPartisan
        //     on the same day
        // It is necessary to allow this sometimes, so this has been replaced by a client-side
        // warning/override
        var alreadyHasPartyPrimary = false;
        if (electionType == Elections.ElectionTypeStatePrimary)
        {
          alreadyHasPartyPrimary = Elections.GetPartyPrimaryExists(StateCode, electionDate);
        }

        bool addPresident;
        switch (electionType)
        {
          case Elections.ElectionTypeStatePresidentialPrimary:
          case Elections.ElectionTypeUSPresidentialPrimary:
            addPresident = true;
            break;

          case Elections.ElectionTypeStatePrimary:
            addPresident = ControlAddElectionIncludePresident.Checked;
            break;

          case Elections.ElectionTypeGeneralElection:
            addPresident = StateCode == "US" && nationalPartyCode == "A";
            break;

          default:
            addPresident = false;
            break;
        }

        var includePresidentialCandidates = false;
        if (addPresident && electionType != Elections.ElectionTypeUSPresidentialPrimary)
          includePresidentialCandidates = ControlAddElectionIncludePresidentCandidates
            .Checked;

        var electionKeyToCopyOfficesAndDatesFrom = Empty;
        if (electionType == Elections.ElectionTypeStatePrimary)
          electionKeyToCopyOfficesAndDatesFrom = AddElectionCopyOfficesHidden.Value.Trim();

        var copyCandidates = ControlAddElectionCopyCandidates.Checked;

        // Build the tables to add

        var electionsTable = new ElectionsTable();
        var electionsOfficesTable = new ElectionsOfficesTable();
        var electionsPoliticiansTable = new ElectionsPoliticiansTable();

        LogElectionsInsert(newElectionKey);

        // The election
        InsertElection(electionsTable, newElectionKey,
          electionKeyToCopyOfficesAndDatesFrom);

        // Offices
        if (addPresident)
          InsertOffice(electionsOfficesTable, newElectionKey, Offices.USPresident,
            OfficeClass.USPresident, Empty);

        if (!IsNullOrWhiteSpace(electionKeyToCopyOfficesAndDatesFrom))
        {
          var copyTable =
            ElectionsOffices.GetOfficeKeysData(electionKeyToCopyOfficesAndDatesFrom);
          foreach (var row in copyTable.Where(row => row.OfficeKey != Offices.USPresident))
            InsertOffice(electionsOfficesTable, newElectionKey, row.OfficeKey,
              row.OfficeLevel.ToOfficeClass(), row.DistrictCode);
        }

        // Candidates
        if (includePresidentialCandidates)
        {
          var candidateTable =
            ElectionsPoliticians.GetPresidentialCandidatesFromTemplate(electionDate,
              nationalPartyCode);
          if (candidateTable == null)
            throw new ApplicationException($"Presidential candidate template for national party {nationalPartyCode} with date >= {electionDate} not found");
          foreach (var row in candidateTable.Rows.OfType<DataRow>())
            InsertCandidate(electionsPoliticiansTable, newElectionKey, row.OfficeKey(),
              row.PoliticianKey(), row.OrderOnBallot());
        }

        if (!IsNullOrWhiteSpace(electionKeyToCopyOfficesAndDatesFrom) &&
          copyCandidates)
        {
          var candidateTable =
            ElectionsPoliticians.GetPrimaryCandidatesToCopy(
              electionKeyToCopyOfficesAndDatesFrom);
          foreach (var row in candidateTable.Rows.OfType<DataRow>())
            InsertCandidate(electionsPoliticiansTable, newElectionKey, row.OfficeKey(),
              row.PoliticianKey(), row.OrderOnBallot());
        }

        Elections.UpdateElectionsAndOffices(electionsTable, electionsOfficesTable,
          electionsPoliticiansTable);

        ReloadElectionControl(newElectionKey);
        _AddElectionTabInfo.Reset();
        FeedbackAddElection.AddInfo(
          $"Election added: {Elections.FormatElectionDescription(newElectionKey)}");

        if (electionType == Elections.ElectionTypeStatePrimary && !alreadyHasPartyPrimary)
          FeedbackAddElection.AddInfo(
            "Your next step is to use the Add/Remove Offices tab to add office contests for" +
            " this party primary. You can then copy these office for the remaining party primaries.");

        if (!StateCache.IsValidStateCode(StateCode)) // Presidential Primary
          FeedbackAddElection.AddInfo(
            "Use the Setup Candidates for Office tab to identify the presidential candidates.");
      }
      catch (Exception ex)
      {
        FeedbackAddElection.HandleException(ex);
      }
    }

    #endregion Event handlers and overrides
  }
}