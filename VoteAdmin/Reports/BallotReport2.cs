using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using DB;
using DB.Vote;

namespace Vote.Reports
{
  internal abstract class BallotReport2 : Report
  {
    internal BallotReport2(bool includeOptionalData = false,
      bool reportAllLocals = false)
    {
      _IncludeOptionalData = includeOptionalData;
      _ReportAllLocals = reportAllLocals;
    }

    #region Private

    private readonly bool _IncludeOptionalData;
    private readonly bool _ReportAllLocals;

    protected string ElectionKey { get; private set; }
    protected string Congress { get; private set; }
    protected string StateSenate { get; private set; }
    protected string StateHouse { get; private set; }
    protected string StateElectionKey { get; private set; }
    protected string StateCode { get; private set; }
    protected string CountyCode { get; private set; }

    private int _TotalContests;

    private readonly BallotReportDataManager _DataManager =
      new BallotReportDataManager();

    private sealed class StateFilter : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return string.IsNullOrWhiteSpace(row.CountyCode()) && !row.IsRunningMate();
      }
    }

    private sealed class CountyFilter : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return !string.IsNullOrWhiteSpace(row.CountyCode()) &&
          string.IsNullOrWhiteSpace(row.LocalCode()) && !row.IsRunningMate();
      }
    }

    private sealed class LocalFilter : ReportDataManager<DataRow>.FilterBy
    {
      public override bool Filter(DataRow row)
      {
        return !string.IsNullOrWhiteSpace(row.LocalCode()) && !row.IsRunningMate();
      }
    }

    private sealed class OneLocalFilter : ReportDataManager<DataRow>.FilterBy
    {
      private readonly string _LocalCode;

      public OneLocalFilter(string localCode)
      {
        _LocalCode = localCode;
      }

      public override bool Filter(DataRow row)
      {
        return row.LocalCode() == _LocalCode && !row.IsRunningMate();
      }
    }

    private sealed class BallotOrderBy : ReportDataManager<DataRow>.OrderBy
    {
      public override int Compare(DataRow row1, DataRow row2)
      {
        var result = row1.OfficeOrder()
          .CompareTo(row2.OfficeOrder());
        if (result != 0) return result;
        result = row1.OfficeOrderWithinLevel()
          .CompareTo(row2.OfficeOrderWithinLevel());
        if (result != 0) return result;
        result = string.Compare(row1.OfficeKey(), row2.OfficeKey(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        result = row1.OrderOnBallot()
          .CompareTo(row2.OrderOnBallot());
        if (result != 0) return result;
        result = string.Compare(row1.LastName(), row2.LastName(),
          StringComparison.OrdinalIgnoreCase);
        if (result != 0) return result;
        return string.Compare(row1.FirstName(), row2.FirstName(),
          StringComparison.OrdinalIgnoreCase);
      }
    }

    private sealed class BallotReportDataManager : ReportDataManager<DataRow>
    {
      public void GetData(string electionKey, string congress, string stateSenate, string stateHouse, string countyCode, bool includeOptionalData)
      {
        DataTable = ElectionsPoliticians.GetSampleBallotData(electionKey, congress,
          stateSenate, stateHouse, countyCode, includeOptionalData);
      }

      public List<IGrouping<string, DataRow>> GetOfficeGroups(
        FilterBy filterBy = null, OrderBy orderBy = null)
      {
        return GetDataSubset(filterBy, orderBy ?? new BallotOrderBy())
          .GroupBy(row => row.OfficeKey())
          .ToList();
      }

      public List<IGrouping<string, DataRow>> GetLocalGroups(
        FilterBy filterBy = null, OrderBy orderBy = null)
      {
        return GetDataSubset(filterBy, orderBy ?? new BallotOrderBy())
          .GroupBy(row => row.LocalCode())
          .ToList();
      }

      public DataRow GetRunningMate(string officeKey, string politicianKey)
      {
        return DataTable.Rows.OfType<DataRow>()
          .FirstOrDefault(row => row.OfficeKey()
            .IsEqIgnoreCase(officeKey) && row.RunningMateKey()
              .IsEqIgnoreCase(politicianKey) && row.IsRunningMate());
      }
    }

    private void ReportStateContests()
    {
      var offices = _DataManager.GetOfficeGroups(new StateFilter());

      OnBeginState(StateElectionKey, StateCache.GetStateName(StateCode), offices.Count);

      foreach (var office in offices)
      {
        ReportOneOffice(StateElectionKey, office);
        _TotalContests++;
      }

      OnEndState(StateElectionKey);
    }

    private void ReportCountyContests()
    {
      var offices = _DataManager.GetOfficeGroups(new CountyFilter());

      var countyKey = Elections.GetCountyElectionKeyFromKey(ElectionKey,
        StateCode, CountyCode);

      OnBeginCounty(countyKey, CountyCache.GetCountyName(StateCode, CountyCode), 
        offices.Count);

      if (offices.Count == 0) return;

      foreach (var office in offices)
      {
        ReportOneOffice(countyKey, office);
        _TotalContests++;
      }

      OnEndCounty(countyKey);
    }

    private void ReportLocalContestsForOneLocal()
    {
      var localCode = Elections.GetLocalCodeFromKey(ElectionKey);
      var offices = _DataManager.GetOfficeGroups(new OneLocalFilter(localCode));

      OnBeginLocals(offices.Count == 0 ? 0 : 1);

      if (offices.Count == 0) return;

      var localInfo = offices.First()
        .First();
      var localDistrict = localInfo.LocalDistrict();

      OnBeginLocal(ElectionKey, localCode, localDistrict, offices.Count);

      foreach (var office in offices)
      {
        ReportOneOffice(ElectionKey, office);
        _TotalContests++;
      }

      OnEndLocal(ElectionKey);
      OnEndLocals();
    }

    private void ReportLocalContestsForAllLocals()
    {
      var locals = _DataManager.GetLocalGroups(new LocalFilter());

      OnBeginLocals(locals.Count);

      if (_ReportAllLocals)
      {
        foreach (var l in LocalDistricts.GetNamesDictionary(StateCode, CountyCode)
          .OrderBy(kvp => kvp.Value))
        {
          // get the offices
          var local = locals.FirstOrDefault(g => g.Key == l.Key);
          var localKey = Elections.GetLocalElectionKeyFromKey(ElectionKey,
           StateCode, CountyCode, l.Key);
          if (local == null) OnBeginLocal(localKey, l.Key, l.Value, 0);
          else
          {
            var offices = local.GroupBy(row => row.OfficeKey()).ToList();

            OnBeginLocal(localKey, l.Key, l.Value, offices.Count);

            foreach (var office in offices)
            {
              ReportOneOffice(localKey, office);
              _TotalContests++;
            }
            OnEndLocal(localKey);
          }
        }

      }
      else
      {
        if (locals.Count == 0) return;

        foreach (var local in locals)
        {
          var localInfo = local.First();
          var localCode = localInfo.LocalCode();
          var localDistrict = localInfo.LocalDistrict();
          var localKey = Elections.GetLocalElectionKeyFromKey(ElectionKey,
            StateCode, CountyCode, localCode);
          var offices = local.GroupBy(row => row.OfficeKey()).ToList();

          OnBeginLocal(localKey, localCode, localDistrict, offices.Count);

          foreach (var office in offices)
          {
            ReportOneOffice(localKey, office);
            _TotalContests++;
          }
          OnEndLocal(localKey);
        }
      }

      OnEndLocals();
    }

    private void ReportOneOffice(string electionKey, IEnumerable<DataRow> office)
    {
      var candidates = office.ToList();
      var officeInfo = candidates.First();

      OnBeginOffice(electionKey, officeInfo, 
        candidates.Select(c => c.PoliticianKey()).ToList(),
        candidates.Select(c => c.RunningMateKey()).ToList());

      foreach (var candidate in candidates)
      {
        OnReportCandidate(candidate);
      }

      OnEndOffice(electionKey, officeInfo);
    }

    #endregion Private

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global
    // ReSharper disable UnusedMember.Global

    #endregion ReSharper disable

    #region Members to demote

    #endregion Members to demote

    protected Control GenerateReport(string electionKey, string congress, 
      string stateSenate, string stateHouse, string countyCode, 
      out int totalContests, out int ballotMeasures)
    {
      congress = congress.ZeroPad(3);
      ElectionKey = electionKey;
      Congress = congress;
      StateSenate = stateSenate;
      StateHouse = stateHouse;
      CountyCode = countyCode;
      StateElectionKey = Elections.GetStateElectionKeyFromKey(ElectionKey);
      StateCode = Elections.GetStateCodeFromKey(ElectionKey);

      _DataManager.GetData(ElectionKey, congress, stateSenate, stateHouse,
        CountyCode, _IncludeOptionalData);

      OnBeginReport();

      if (Elections.IsStateElection(ElectionKey))
        ReportStateContests();

      if (Elections.IsStateElection(ElectionKey) ||
        Elections.IsCountyElection(ElectionKey))
        ReportCountyContests();

      if (Elections.IsLocalElection(ElectionKey))
        ReportLocalContestsForOneLocal();
      else
        ReportLocalContestsForAllLocals();

      totalContests = _TotalContests;

      return OnEndReport(out ballotMeasures);
    }

    protected DataRow GetRunningMate(string officeKey, string runningMateKey)
    {
      return _DataManager.GetRunningMate(officeKey, runningMateKey);
    }

    protected virtual void OnBeginCounty(string electionKey, string countyName, int officeCount) { }

    protected virtual void OnBeginLocal(string electionKey, string localCode, string localName, int officeCount) { }

    protected virtual void OnBeginLocals(int localCount) { }

    protected virtual void OnBeginOffice(string electionKey, DataRow officeInfo, IList<string> candidateKeys, IList<string> runningMateKeys) { }

    protected virtual void OnBeginReport() { }

    protected virtual void OnBeginState(string electionKey, string stateName, int officeCount) {}

    protected virtual void OnEndCounty(string electionKey) { }

    protected virtual void OnEndLocal(string electionKey) { }

    protected virtual void OnEndLocals() { }

    protected virtual void OnEndOffice(string electionKey, DataRow officeInfo) { }

    protected abstract Control OnEndReport(out int ballotMeasures);

    protected virtual void OnEndState(string electionKey) { }

    protected virtual void OnReportCandidate(DataRow candidate){}

    #region ReSharper restore

    // ReSharper restore UnusedMember.Global
    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected
  }
}