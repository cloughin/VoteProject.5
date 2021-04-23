using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DB;
using DB.Vote;
using Vote;

namespace VoteAdmin.Admin
{
  public partial class OfficesAndCandidatesReportPage : SecureAdminPage, IAllowEmptyStateCode
  {
    private enum OfficeHeading
    {
      President,
      USSenate,
      USHouse,
      Governor,
      StateExecutive,
      StateSenate,
      StateHouse,
      County,
      Local,
      Other,
      Count
    }

    private bool _HasCandidates;
    private bool _HasOffices;
    private bool _HasBallotMeasures;
    private bool _HasElections;

    private readonly int[] _ElectionCandidates = new int[(int)OfficeHeading.Count];
    private readonly int[] _MonthCandidates = new int[(int)OfficeHeading.Count];
    private readonly int[] _YearCandidates = new int[(int)OfficeHeading.Count];
    private readonly int[] _ReportCandidates = new int[(int)OfficeHeading.Count];

    private readonly int[] _ElectionOfficeCount = new int[(int)OfficeHeading.Count];
    private readonly int[] _MonthOfficeCount = new int[(int)OfficeHeading.Count];
    private readonly int[] _YearOfficeCount = new int[(int)OfficeHeading.Count];
    private readonly int[] _ReportOfficeCount = new int[(int)OfficeHeading.Count];

    private int _ElectionBallotMeasures;
    private int _MonthBallotMeasures;
    private int _YearBallotMeasures;
    private int _ReportBallotMeasures;

    private readonly List<string>[] _ElectionOffices = new List<string>[(int)OfficeHeading.Count];

    private static OfficeHeading GetOfficeHeading(OfficeClass officeClass, OfficeClass altClass)
    {
      switch (officeClass)
      {
        case OfficeClass.USPresident:
          return OfficeHeading.President;

        case OfficeClass.USSenate:
          return OfficeHeading.USSenate;

        case OfficeClass.USHouse:
          return OfficeHeading.USHouse;

        case OfficeClass.StateWide:
          if (altClass == OfficeClass.USGovernors || altClass == OfficeClass.USLtGovernor ||
            altClass == OfficeClass.DCMayor)
            return OfficeHeading.Governor;
          if (altClass == OfficeClass.DCShadowSenator)
            return OfficeHeading.USSenate;
          if (altClass == OfficeClass.DCShadowRepresentative)
            return OfficeHeading.USHouse;
          return OfficeHeading.StateExecutive;

        case OfficeClass.StateSenate:
          return OfficeHeading.StateSenate;

        case OfficeClass.StateHouse:
          return OfficeHeading.StateHouse;

        case OfficeClass.CountyExecutive:
        case OfficeClass.CountyLegislative:
        case OfficeClass.CountySchoolBoard:
        case OfficeClass.CountyCommission:
        case OfficeClass.CountyJudicial:
        case OfficeClass.CountyParty:
          return OfficeHeading.County;

        case OfficeClass.LocalExecutive:
        case OfficeClass.LocalLegislative:
        case OfficeClass.LocalSchoolBoard:
        case OfficeClass.LocalCommission:
        case OfficeClass.LocalJudicial:
        case OfficeClass.LocalParty:
          return OfficeHeading.Local;

        case OfficeClass.StateJudicial:
        case OfficeClass.StateParty:
          return OfficeHeading.StateExecutive;

        default:
          return OfficeHeading.Other;

      }
    }

    private static void RollCounters(IList<int> lower, IList<int> upper)
    {
      for (var i = 0; i < lower.Count; i++)
      {
        upper[i] += lower[i];
        lower[i] = 0;
      }
    }

    private static void RollBallotMeasures(ref int lower, ref int upper)
    {
      upper += lower;
      lower = 0;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      SimpleCsvWriter csvWriter;
      StreamWriter streamWriter;

      void FillDataRows(IList<int> candidates, IList<int> offices, int ballotMeasures, 
        string description = null)
      {
        if (_HasElections)
          csvWriter.AddField(description.SafeString());

        if (_HasOffices)
          csvWriter.AddField(offices[(int)OfficeHeading.President].BlankIfZero());
        if (_HasCandidates)
          csvWriter.AddField(candidates[(int)OfficeHeading.President].BlankIfZero());

        if (_HasOffices)
          csvWriter.AddField(offices[(int)OfficeHeading.USSenate].BlankIfZero());
        if (_HasCandidates)
          csvWriter.AddField(candidates[(int)OfficeHeading.USSenate].BlankIfZero());

        if (_HasOffices)
          csvWriter.AddField(offices[(int)OfficeHeading.USHouse].BlankIfZero());
        if (_HasCandidates)
          csvWriter.AddField(candidates[(int)OfficeHeading.USHouse].BlankIfZero());

        if (_HasOffices)
          csvWriter.AddField(offices[(int)OfficeHeading.Governor].BlankIfZero());
        if (_HasCandidates)
          csvWriter.AddField(candidates[(int)OfficeHeading.Governor].BlankIfZero());

        if (_HasOffices)
          csvWriter.AddField(offices[(int)OfficeHeading.StateExecutive].BlankIfZero());
        if (_HasCandidates)
          csvWriter.AddField(candidates[(int)OfficeHeading.StateExecutive].BlankIfZero());

        if (_HasOffices)
          csvWriter.AddField(offices[(int)OfficeHeading.StateSenate].BlankIfZero());
        if (_HasCandidates)
          csvWriter.AddField(candidates[(int)OfficeHeading.StateSenate].BlankIfZero());

        if (_HasOffices)
          csvWriter.AddField(offices[(int)OfficeHeading.StateHouse].BlankIfZero());
        if (_HasCandidates)
          csvWriter.AddField(candidates[(int)OfficeHeading.StateHouse].BlankIfZero());

        if (_HasOffices)
          csvWriter.AddField(offices[(int)OfficeHeading.County].BlankIfZero());
        if (_HasCandidates)
          csvWriter.AddField(candidates[(int)OfficeHeading.County].BlankIfZero());

        if (_HasOffices)
          csvWriter.AddField(offices[(int)OfficeHeading.Local].BlankIfZero());
        if (_HasCandidates)
          csvWriter.AddField(candidates[(int)OfficeHeading.Local].BlankIfZero());

        if (_HasBallotMeasures)
          csvWriter.AddField(ballotMeasures.BlankIfZero());

        var allOffices = 0;
        var allCandidates = 0;

        for (var o = OfficeHeading.President; o < OfficeHeading.Count; o++)
        {
          allOffices += offices[(int)o];
          allCandidates += candidates[(int)o];
        }

        if (_HasOffices)
          csvWriter.AddField(allOffices.BlankIfZero());
        if (_HasCandidates)
          csvWriter.AddField(allCandidates.BlankIfZero());

        csvWriter.Write(streamWriter);
      }

      bool IsBallotMeasure(DataRow r)
      {
        return r.ReferendumKey() != null;
      }

      if (!DateTime.TryParse(Request.QueryString["from"], out var fromDate))
        fromDate = DateTime.MinValue;
      if (!DateTime.TryParse(Request.QueryString["to"], out var toDate))
        toDate = DateTime.MaxValue;
      var reportType = Request.QueryString["type"].SafeString();
      _HasCandidates = reportType.Contains("C");
      _HasOffices = reportType.Contains("O");
      _HasBallotMeasures = reportType.Contains("B");
      _HasElections = Request.QueryString["detail"] == "E";

      for (var i = 0; i < (int) OfficeHeading.Count; i++)
        _ElectionOffices[i] = new List<string>();

      // get data table from database
      var table = ElectionsPoliticians.GetOfficesAndCandidatesReportData(fromDate, toDate, 0)
        .Rows.OfType<DataRow>().ToList();

      // create dummy entries so months with no elections will show
      var lowDate = fromDate == DateTime.MinValue
        ? table.Min(r => r.ElectionDate())
        : fromDate;
      var highDate = toDate == DateTime.MaxValue
        ? table.Max(r => r.ElectionDate())
        : toDate;
      var monthList = new List<DateTime>();
      while (lowDate <= highDate)
      {
        monthList.Add(lowDate);
        lowDate = lowDate.AddMonths(1);
      }

      var dummies = monthList.Select(m => new
      {
        row = (DataRow) null,
        MonthYear = m.ToString("yyyyMM"),
        MonthYearDesc = m.ToString("MMM yyyy"),
        Year = m.ToString("yyyy"),
        OfficeHeading = OfficeHeading.Other
      }).ToList();

      var data = table
        .Select(r => new
        {
          row = r,
          MonthYear = r.ElectionDate().ToString("yyyyMM"),
          MonthYearDesc = r.ElectionDate().ToString("MMM yyyy"),
          Year = r.ElectionDate().ToString("yyyy"),
          OfficeHeading = GetOfficeHeading(r.OfficeClass(), r.AlternateOfficeClass())
        })
        .Union(dummies)
        .GroupBy(r => r.row == null ? r.MonthYear : r.row.ElectionKeyState())
        .GroupBy(el => el.First().MonthYear)
        .OrderBy(g => g.Key)
        .GroupBy(m => m.First().First().Year)
        .ToList();

      using (var ms = new MemoryStream())
      {
        streamWriter = new StreamWriter(ms);
        csvWriter = new SimpleCsvWriter();

        // write CSV headings
        csvWriter.AddField("Date");
        if (_HasElections) csvWriter.AddField("Election Title");
        if (_HasOffices) csvWriter.AddField("President Offices");
        if (_HasCandidates) csvWriter.AddField("President Candidates");
        if (_HasOffices) csvWriter.AddField("US Senate Offices");
        if (_HasCandidates) csvWriter.AddField("US Senate Candidates");
        if (_HasOffices) csvWriter.AddField("US House Offices");
        if (_HasCandidates) csvWriter.AddField("US House Candidates");
        if (_HasOffices) csvWriter.AddField("Governor Offices");
        if (_HasCandidates) csvWriter.AddField("Governor Candidates");
        if (_HasOffices) csvWriter.AddField("State Executive Offices");
        if (_HasCandidates) csvWriter.AddField("State Executive Candidates");
        if (_HasOffices) csvWriter.AddField("State Senate Offices");
        if (_HasCandidates) csvWriter.AddField("State Senate Candidates");
        if (_HasOffices) csvWriter.AddField("State House Offices");
        if (_HasCandidates) csvWriter.AddField("State House Candidates");
        if (_HasOffices) csvWriter.AddField("Countywide Offices");
        if (_HasCandidates) csvWriter.AddField("Countywide Candidates");
        if (_HasOffices) csvWriter.AddField("Local Offices");
        if (_HasCandidates) csvWriter.AddField("Local Candidates");
        if (_HasBallotMeasures) csvWriter.AddField("Ballot Measures");
        if (_HasOffices) csvWriter.AddField("TOTAL OFFICES");
        if (_HasCandidates) csvWriter.AddField("TOTAL CANDIDATES");
        csvWriter.Write(streamWriter);

        foreach (var year in data)
        {
          foreach (var monthYear in year)
          {
            foreach (var election in monthYear)
            {
              for (var i = 0; i < _ElectionOfficeCount.Length; i++)
                _ElectionOffices[i].Clear();
              foreach (var candidate in election)
                if (candidate.row != null)
                  if (IsBallotMeasure(candidate.row))
                  {
                    _ElectionBallotMeasures++;
                  }
                  else
                  {
                    _ElectionCandidates[(int) candidate.OfficeHeading]++;
                    _ElectionOffices[(int)candidate.OfficeHeading].Add(candidate.row.OfficeKey());
                  }
              for (var i = 0; i < _ElectionOfficeCount.Length; i++)
                _ElectionOfficeCount[i] = _ElectionOffices[i].Distinct().Count();
              if (_HasElections)
              {
                var row = election.First().row;
                if (row != null)
                {
                  csvWriter.AddField(row.ElectionDate().ToString("M/d/yyyy"));
                  var desc =
                    $"=HYPERLINK(\"{UrlManager.GetAdminUri(GetAdminFolderPageUrl("election", "election", row.ElectionKeyState(), "complete", "1"))}\",\"{row.ElectionDescription()}\")";
                  FillDataRows(_ElectionCandidates, _ElectionOfficeCount, _ElectionBallotMeasures,
                    desc);
                }
              }
              RollCounters(_ElectionCandidates, _MonthCandidates);
              RollCounters(_ElectionOfficeCount, _MonthOfficeCount);
              RollBallotMeasures(ref _ElectionBallotMeasures, ref _MonthBallotMeasures);
            }
            csvWriter.AddField($"Total {monthYear.First().First().MonthYearDesc}");
            FillDataRows(_MonthCandidates, _MonthOfficeCount, _MonthBallotMeasures);
            RollCounters(_MonthCandidates, _YearCandidates);
            RollCounters(_MonthOfficeCount, _YearOfficeCount);
            RollBallotMeasures(ref _MonthBallotMeasures, ref _YearBallotMeasures);
          }
          csvWriter.Write(streamWriter);
          csvWriter.AddField($"TOTAL YEAR {year.First().First().First().Year}");
          FillDataRows(_YearCandidates, _YearOfficeCount, _YearBallotMeasures);
          RollCounters(_YearCandidates, _ReportCandidates);
          RollCounters(_YearOfficeCount, _ReportOfficeCount);
          RollBallotMeasures(ref _YearBallotMeasures, ref _ReportBallotMeasures);
          csvWriter.Write(streamWriter);
        }
        csvWriter.AddField("REPORT TOTAL");
        FillDataRows(_ReportCandidates, _ReportOfficeCount, _ReportBallotMeasures);

        streamWriter.Flush();
        ms.Position = 0;
        var csv = new StreamReader(ms).ReadToEnd();

        var reportDesc = String.Empty;
        if (_HasOffices)
          if (_HasCandidates)
            reportDesc += "Offices & Candidates";
          else
            reportDesc += "Offices";
        else if (_HasCandidates)
          reportDesc += "Candidates";
        else if (_HasBallotMeasures)
          reportDesc += "Ballot Measures";
        reportDesc += " in Elections";
        if (fromDate != DateTime.MinValue)
          if (toDate != DateTime.MaxValue)
            reportDesc += $" {fromDate:M-d-yyyy} to {toDate:M-d-yyyy}";
          else
            reportDesc += $" after {fromDate:M-d-yyyy}";
        else if (toDate != DateTime.MaxValue)
          reportDesc += $" before {toDate:M-d-yyyy}";
        if (_HasElections)
          reportDesc += " with election detail";

        // download
        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition",
          $"attachment;filename=\"{reportDesc}.csv\"");
        Response.Write("\xfeff"); // BOM
        Response.Write(csv);
        Response.End();
      }
    }
  }
}