using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DB;
using DB.Vote;
using static System.String;

// Classes used for per-page caching, to minimize repeated and redundant DB access

// Note: Locking not required on Dictionaries because each cache instance is 
// accessed by a single thread only.

namespace Vote
{
  public sealed class AddressesCache
  {
    #region Private

    private bool _Initialized;
    private Dictionary<int, AddressesRow> _Dictionary;

    private void Initialize()
    {
      _Dictionary =
        new Dictionary<int, AddressesRow>();
      _Initialized = true;
    }

    private AddressesRow GetAddressesRow(int addressId)
    {
      if (!_Initialized) Initialize();
      if (addressId == 0) return null;
      if (!_Dictionary.TryGetValue(addressId, out var row))
      {
        var table = Addresses.GetDataById(addressId);
        if (table.Count == 1)
          row = table[0];
        _Dictionary[addressId] = row;
      }
      return row;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public bool Exists(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row != null;
    }

    public string GetAddress(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.Address;
    }

    public string GetCity(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.City;
    }

    public string GetCityCouncil(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.CityCouncil;
    }

    public string GetCongressionalDistrict(int addressId)
    {
      var row = GetAddressesRow(addressId);
      var district = row == null ? Empty : row.CongressionalDistrict;
      if (district == "00") district = Empty;
      return district;
    }

    public string GetCongressionalDistrictDesc(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null
        ? Empty
        : Offices.GetDistrictItem(row.StateCode, OfficeClass.USHouse,
          row.CongressionalDistrict);
    }

    public string GetCountyCode(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.County;
    }

    public string GetCountySupervisors(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.CountySupervisors;
    }

    public DateTime GetDateStamp(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row?.DateStamp ?? VotePage.DefaultDbDate;
    }

    public string GetDistrict(int addressId)
    {
      var row = GetAddressesRow(addressId);
      var district = row == null ? Empty : row.District;
      return district;
    }

    public DateTime GetDistrictLookupDate(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row?.DistrictLookupDate ?? VotePage.DefaultDbDate;
    }

    public string GetElementary(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.Elementary;
    }

    public string GetEmail(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.Email;
    }

    public DateTime GetEmailAttachedDate(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row?.EmailAttachedDate ?? VotePage.DefaultDbDate;
    }

    public string GetEmailAttachedSource(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.EmailAttachedSource;
    }

    public string GetFirstName(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.FirstName;
    }

    public string GetLastName(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.LastName;
    }

    public bool GetIsPartyMajor(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row?.OptOut == true;
    }

    public string GetPhone(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.Phone;
    }

    public string GetPlace(int addressId)
    {
      var row = GetAddressesRow(addressId);
      var place = row == null ? Empty : row.Place;
      return place;
    }

    public string GetSchoolDistrictDistrict(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.SchoolDistrictDistrict;
    }

    public string GetSecondary(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.Secondary;
    }

    public bool GetSendSampleBallots(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row?.SendSampleBallots == true;
    }

    public string GetSourceCode(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.SourceCode;
    }

    public string GetStateCode(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.StateCode;
    }

    public string GetStateHouseDistrict(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.StateHouseDistrict;
    }

    public string GetStateHouseDistrictDesc(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null
        ? Empty
        : Offices.GetDistrictItem(row.StateCode, OfficeClass.StateHouse,
          row.StateHouseDistrict);
    }

    public string GetStateSenateDistrict(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.StateSenateDistrict;
    }

    public string GetStateSenateDistrictDesc(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null
        ? Empty
        : Offices.GetDistrictItem(row.StateCode, OfficeClass.StateSenate,
          row.StateSenateDistrict);
    }

    public string GetUnified(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.Unified;
    }

    public string GetZip4(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.Zip4;
    }

    public string GetZip5(int addressId)
    {
      var row = GetAddressesRow(addressId);
      return row == null ? Empty : row.Zip5;
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public sealed class DonationsCache
  {
    #region Private

    private bool _Initialized;
    private Dictionary<int, DonationsRow> _Dictionary;

    private void Initialize()
    {
      _Dictionary =
        new Dictionary<int, DonationsRow>();
      _Initialized = true;
    }

    private DonationsRow GetDonationsRow(int donationId)
    {
      if (!_Initialized) Initialize();
      if (donationId == 0) return null;
      if (!_Dictionary.TryGetValue(donationId, out var row))
      {
        var table = Donations.GetDataById(donationId);
        if (table.Count == 1)
          row = table[0];
        _Dictionary[donationId] = row;
      }
      return row;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public bool Exists(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row != null;
    }

    public string GetAddress(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row == null ? Empty : row.Address;
    }

    public decimal GetAmount(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row?.Amount ?? 0;
    }

    public string GetCity(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row == null ? Empty : row.City;
    }

    public DateTime GetDate(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row?.Date ?? VotePage.DefaultDbDate;
    }

    public string GetEmail(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row == null ? Empty : row.Email;
    }

    public string GetFirstName(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row == null ? Empty : row.FirstName;
    }

    public string GetFullName(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row == null ? Empty : row.FirstName;
    }

    public string GetLastName(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row == null ? Empty : row.LastName;
    }

    public string GetPhone(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row == null ? Empty : row.Phone;
    }

    public string GetStateCode(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row == null ? Empty : row.StateCode;
    }

    public string GetZip4(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row == null ? Empty : row.Zip4;
    }

    public string GetZip5(int donationId)
    {
      var row = GetDonationsRow(donationId);
      return row == null ? Empty : row.Zip5;
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }


  public sealed class ElectionsCache
  {
    #region Private

    private bool _Initialized;
    private Dictionary<string, ElectionsRow> _Dictionary;

    private Dictionary<string, string>
      _LatestViewableElectionKeyByStateCodeDictionary;

    private Dictionary<string, string>
      _LatestViewableGeneralElectionKeyByStateCodeDictionary;

    private Dictionary<string, string>
      _LatestViewableOffYearElectionKeyByStateCodeDictionary;

    private Dictionary<string, string>
      _LatestViewableSpecialElectionKeyByStateCodeDictionary;

    private Dictionary<string, string>
      _LatestViewablePrimaryElectionKeyByStateCodeDictionary;

    private void Initialize()
    {
      _Dictionary =
        new Dictionary<string, ElectionsRow>(StringComparer.OrdinalIgnoreCase);
      _LatestViewableElectionKeyByStateCodeDictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _LatestViewableGeneralElectionKeyByStateCodeDictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _LatestViewableOffYearElectionKeyByStateCodeDictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _LatestViewableSpecialElectionKeyByStateCodeDictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _LatestViewablePrimaryElectionKeyByStateCodeDictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _Initialized = true;
    }

    private ElectionsRow GetElectionsRow(string electionKey)
    {
      if (!_Initialized) Initialize();
      if (electionKey == null) return null;
      if (!_Dictionary.TryGetValue(electionKey, out var row))
      {
        var table = Elections.GetCacheData(electionKey);
        if (table.Count == 1)
          row = table[0];
        _Dictionary[electionKey] = row;
      }
      return row;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public string GetBallotInstructions(string electionKey)
    {
      var row = GetElectionsRow(electionKey);
      return row == null ? Empty : row.BallotInstructions;
    }

    public string GetElectionAdditionalInfo(string electionKey)
    {
      var row = GetElectionsRow(electionKey);
      return row == null ? Empty : row.ElectionAdditionalInfo;
    }

    public DateTime GetElectionDate(string electionKey)
    {
      var row = GetElectionsRow(electionKey);
      return row?.ElectionDate ?? DateTime.MinValue;
    }

    public string GetElectionDesc(string electionKey)
    {
      var row = GetElectionsRow(electionKey);
      return row == null ? Empty : row.ElectionDesc;
    }

    public string GetElectionResultsSource(string electionKey)
    {
      var row = GetElectionsRow(electionKey);
      return row == null ? Empty : row.ElectionResultsSource;
    }

    public string GetLatestViewableElectionKeyByStateCode(string stateCode)
    {
      if (!_Initialized) Initialize();
      if (stateCode == null) return null;
      if (
        !_LatestViewableElectionKeyByStateCodeDictionary.TryGetValue(
          stateCode, out var electionKey))
      {
        electionKey = Elections.GetLatestViewableElectionKeyByStateCode(
          stateCode, Empty);
        _LatestViewableElectionKeyByStateCodeDictionary[stateCode] = electionKey;
      }
      return electionKey;
    }

    public string GetLatestViewableGeneralElectionKeyByStateCode(string stateCode)
    {
      if (!_Initialized) Initialize();
      if (stateCode == null) return null;
      if (
        !_LatestViewableGeneralElectionKeyByStateCodeDictionary.TryGetValue(
          stateCode, out var electionKey))
      {
        electionKey =
          Elections.GetLatestViewableGeneralElectionKeyByStateCode(
            stateCode, Empty);
        _LatestViewableGeneralElectionKeyByStateCodeDictionary[stateCode] =
          electionKey;
      }
      return electionKey;
    }

    public string GetLatestViewableOffYearElectionKeyByStateCode(string stateCode)
    {
      if (!_Initialized) Initialize();
      if (stateCode == null) return null;
      if (
        !_LatestViewableOffYearElectionKeyByStateCodeDictionary.TryGetValue(
          stateCode, out var electionKey))
      {
        electionKey =
          Elections.GetLatestViewableOffYearElectionKeyByStateCode(
            stateCode, Empty);
        _LatestViewableOffYearElectionKeyByStateCodeDictionary[stateCode] =
          electionKey;
      }
      return electionKey;
    }

    public string GetLatestViewableSpecialElectionKeyByStateCode(string stateCode)
    {
      if (!_Initialized) Initialize();
      if (stateCode == null) return null;
      if (
        !_LatestViewableSpecialElectionKeyByStateCodeDictionary.TryGetValue(
          stateCode, out var electionKey))
      {
        electionKey =
          Elections.GetLatestViewableSpecialElectionKeyByStateCode(
            stateCode, Empty);
        _LatestViewableSpecialElectionKeyByStateCodeDictionary[stateCode] =
          electionKey;
      }
      return electionKey;
    }

    public string GetLatestViewablePrimaryElectionKeyByStateCode(string stateCode)
    {
      if (!_Initialized) Initialize();
      if (stateCode == null) return null;
      if (
        !_LatestViewablePrimaryElectionKeyByStateCodeDictionary.TryGetValue(
          stateCode, out var electionKey))
      {
        electionKey =
          Elections.GetLatestViewablePrimaryElectionKeyByStateCode(
            stateCode, Empty);
        _LatestViewablePrimaryElectionKeyByStateCodeDictionary[stateCode] =
          electionKey;
      }
      return electionKey;
    }

    #endregion Public
  }

  public sealed class ElectionsPoliticiansCache
  {
    #region Private

    private bool _Initialized;

    private Dictionary<string, ElectionsPoliticiansTable>
      _ElectionKeyOfficeKeyDictionary;

    private Dictionary<string, string> _FutureElectionKeyByPoliticianKeyDictionary;
    private Dictionary<string, string> _FutureElectionKeyByRunningMateKeyDictionary;

    private void Initialize()
    {
      _ElectionKeyOfficeKeyDictionary =
        new Dictionary<string, ElectionsPoliticiansTable>(
          StringComparer.OrdinalIgnoreCase);
      _FutureElectionKeyByPoliticianKeyDictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _FutureElectionKeyByRunningMateKeyDictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _Initialized = true;
    }

    private static string FormatElectionKeyOfficeKeyDictionaryKey(
      string electionKey, string officeKey)
    {
      return electionKey + "|" + officeKey;
    }

    // We concatenate politicianKey and isViewable for the FutureElectionDictionary key
    private static string FormatFutureElectionDictionaryKey(
      string politicianKey, bool isViewable)
    {
      return politicianKey + "|" + isViewable;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public int CountByElectionKeyOfficeKey(string electionKey, string officeKey)
    {
      var table = GetElectionKeyOfficeKeyTable(electionKey, officeKey);
      return table?.Count ?? 0;
    }

    public ElectionsPoliticiansTable GetElectionKeyOfficeKeyTable(
      string electionKey, string officeKey)
    {
      if (!_Initialized) Initialize();
      if (electionKey == null || officeKey == null) return null;
      var key = FormatElectionKeyOfficeKeyDictionaryKey(electionKey, officeKey);
      if (!_ElectionKeyOfficeKeyDictionary.TryGetValue(key, out var table))
      {
        table = ElectionsPoliticians.GetCacheDataByElectionKeyOfficeKey(
          electionKey, officeKey);
        _ElectionKeyOfficeKeyDictionary[key] = table;
      }
      return table;
    }

    public string GetFutureElectionKeyByPoliticianKey(
      string politicianKey, bool isViewable)
    {
      if (!_Initialized) Initialize();
      if (politicianKey == null) return null;
      var key = FormatFutureElectionDictionaryKey(politicianKey, isViewable);
      if (
        !_FutureElectionKeyByPoliticianKeyDictionary.TryGetValue(
          key, out var electionKey))
      {
        electionKey =
          ElectionsPoliticians.GetFutureOfficeKeyInfoByPoliticianKey(
              politicianKey, isViewable)
            .ElectionKey;
        _FutureElectionKeyByPoliticianKeyDictionary[key] = electionKey;
      }
      return electionKey;
    }

    public string GetFutureElectionKeyByRunningMateKey(
      string runningMateKey, bool isViewable)
    {
      if (!_Initialized) Initialize();
      if (runningMateKey == null) return null;
      var key = FormatFutureElectionDictionaryKey(runningMateKey, isViewable);
      if (
        !_FutureElectionKeyByRunningMateKeyDictionary.TryGetValue(
          key, out var electionKey))
      {
        electionKey =
          ElectionsPoliticians.GetFutureOfficeKeyInfoByRunningMateKey(
              runningMateKey, isViewable)
            .ElectionKey;
        _FutureElectionKeyByRunningMateKeyDictionary[key] = electionKey;
      }
      return electionKey;
    }

    public string GetPreviousElectionKeyByPoliticianKey(string politicianKey)
    {
      if (!_Initialized) Initialize();
      if (politicianKey == null) return null;
      if (
        !_FutureElectionKeyByPoliticianKeyDictionary.TryGetValue(
          politicianKey, out var electionKey))
      {
        electionKey =
          ElectionsPoliticians.GetPreviousOfficeKeyInfoByPoliticianKey(politicianKey)
            .ElectionKey;
        _FutureElectionKeyByPoliticianKeyDictionary[politicianKey] = electionKey;
      }
      return electionKey;
    }

    public string GetPreviousElectionKeyByRunningMateKey(string runningMateKey)
    {
      if (!_Initialized) Initialize();
      if (runningMateKey == null) return null;
      if (
        !_FutureElectionKeyByRunningMateKeyDictionary.TryGetValue(
          runningMateKey, out var electionKey))
      {
        electionKey =
          ElectionsPoliticians.GetPreviousOfficeKeyInfoByRunningMateKey(
              runningMateKey)
            .ElectionKey;
        _FutureElectionKeyByRunningMateKeyDictionary[runningMateKey] = electionKey;
      }
      return electionKey;
    }

    public string GetRunningMateKey(
      string electionKey, string officeKey, string politicianKey)
    {
      if (!_Initialized) Initialize();
      if (politicianKey == null) return null;
      var runningMateKey = Empty;
      var table = GetElectionKeyOfficeKeyTable(electionKey, officeKey);
      if (table != null)
        runningMateKey = Enumerable.Select(table.Where(row => row.PoliticianKey == politicianKey), row => row.RunningMateKey)
          .FirstOrDefault();
      return runningMateKey;
    }


    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public sealed class IssuesCache
  {
    #region Private

    private bool _Initialized;
    //private Dictionary<string, IssuesRow> _Dictionary;
    private Dictionary<int, Issues2Row> _Dictionary2;

    private void Initialize()
    {
      //_Dictionary =
      //  new Dictionary<string, IssuesRow>(StringComparer.OrdinalIgnoreCase);
      _Dictionary2 =
        new Dictionary<int, Issues2Row>();
      _Initialized = true;
    }

    //private IssuesRow GetIssuesRow(string issueKey)
    //{
    //  if (!_Initialized) Initialize();
    //  if (issueKey == null) return null;
    //  if (!_Dictionary.TryGetValue(issueKey, out var row))
    //  {
    //    var table = Issues.GetCacheData(issueKey);
    //    if (table.Count == 1)
    //      row = table[0];
    //    _Dictionary[issueKey] = row;
    //  }
    //  return row;
    //}

    private Issues2Row GetIssues2Row(int? issueId)
    {
      if (!_Initialized) Initialize();
      if (issueId == null) return null;
      if (!_Dictionary2.TryGetValue(issueId.Value, out var row))
      {
        var table = Issues2.GetCacheData(issueId.Value);
        if (table.Count == 1)
          row = table[0];
        _Dictionary2[issueId.Value] = row;
      }
      return row;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public string GetIssue(string issueKey)
    {
      var issueId = int.Parse(issueKey);
      if (issueId == Issues.IssueId.Biographical.ToInt()) return "Biographical";
      var row = GetIssues2Row(issueId);
      return row == null ? Empty : row.Issue;
    }

    //public string GetIssueDescription(string issueKey)
    //{
    //  if (Issues.IsIssuesListKey(issueKey))
    //    return "List of Issues";
    //  return Issues.IsBiographicalKey(issueKey)
    //    ? "Biographical Information"
    //    : GetIssue(issueKey);
    //}

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public sealed class LocalDistrictsCache
  {
    #region Private

    private bool _Initialized;
    private Dictionary<string, LocalDistrictsRow> _Dictionary;

    private void Initialize()
    {
      _Dictionary =
        new Dictionary<string, LocalDistrictsRow>(StringComparer.OrdinalIgnoreCase);
      _Initialized = true;
    }

    // We concatenate state code and local key for the Dictionary key
    private static string FormatDictionaryKey(string stateCode, string localKey)
    {
      return stateCode + "|" + localKey;
    }

    private LocalDistrictsRow GetLocalDistrictsRow(string stateCode, string localKey)
    {
      if (!_Initialized) Initialize();
      if (stateCode == null || localKey == null) return null;
      var dictionaryKey = FormatDictionaryKey(stateCode, localKey);
      if (!_Dictionary.TryGetValue(dictionaryKey, out var row))
      {
        var table = LocalDistricts.GetCacheDataByStateCodeLocalKey(stateCode, localKey);
        if (table.Count == 1)
          row = table[0];
        _Dictionary[dictionaryKey] = row;
      }
      return row;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public bool Exists(string stateCode, string localKey)
    {
      var row = GetLocalDistrictsRow(stateCode, localKey);
      return row != null;
    }

    public string GetLocalDistrict(string stateCode, string localKey)
    {
      var row = GetLocalDistrictsRow(stateCode, localKey);
      return row == null ? Empty : row.LocalDistrict;
    }


    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public sealed class OfficesCache
  {
    #region Private

    private bool _Initialized;
    private Dictionary<string, OfficesRow> _Dictionary;
    private Dictionary<string, OfficesTable> _TableDictionary;

    private void Initialize()
    {
      _Dictionary =
        new Dictionary<string, OfficesRow>(StringComparer.OrdinalIgnoreCase);
      _TableDictionary =
        new Dictionary<string, OfficesTable>(StringComparer.OrdinalIgnoreCase);
      _Initialized = true;
    }

    private static string FormatTableDictionaryKey(
      bool isInactive, bool isOnlyForPrimaries, int officeLevel)
    {
      return isInactive + "|" + isOnlyForPrimaries + "|" + officeLevel;
    }

    private static string FormatTableDictionaryKey(
      bool isInactive, bool isOnlyForPrimaries, int officeLevel, string stateCode)
    {
      return isInactive + "|" + isOnlyForPrimaries + "|" + officeLevel + "|" +
        stateCode;
    }

    private OfficesRow GetOfficesRow(string officeKey)
    {
      if (!_Initialized) Initialize();
      if (officeKey == null) return null;
      if (!_Dictionary.TryGetValue(officeKey, out var row))
      {
        var table = Offices.GetCacheData(officeKey);
        if (table.Count == 1)
          row = table[0];
        _Dictionary[officeKey] = row;
      }
      return row;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public OfficesTable GetOfficesTable(
      bool isInactive, bool isOnlyForPrimaries, int officeLevel)
    {
      if (!_Initialized) Initialize();
      var key = FormatTableDictionaryKey(isInactive, isOnlyForPrimaries, officeLevel);
      if (!_TableDictionary.TryGetValue(key, out var table))
      {
        table =
          Offices.GetCacheDataByIsInactiveIsOnlyForPrimariesOfficeLevel(
            isInactive, isOnlyForPrimaries, officeLevel);
        _TableDictionary[key] = table;
      }
      return table;
    }

    public OfficesTable GetOfficesTable(
      bool isInactive, bool isOnlyForPrimaries, int officeLevel, string stateCode)
    {
      if (!_Initialized) Initialize();
      var key = FormatTableDictionaryKey(
        isInactive, isOnlyForPrimaries, officeLevel, stateCode);
      if (!_TableDictionary.TryGetValue(key, out var table))
      {
        table =
          Offices.GetCacheDataByIsInactiveIsOnlyForPrimariesOfficeLevelStateCode(
            isInactive, isOnlyForPrimaries, officeLevel, stateCode);
        _TableDictionary[key] = table;
      }
      return table;
    }

    public string GetDistrictCode(string officeKey)
    {
      var row = GetOfficesRow(officeKey);
      return row == null ? Empty : row.DistrictCode;
    }

    public bool GetIsRunningMateOffice(string officeKey)
    {
      var row = GetOfficesRow(officeKey);
      return row?.IsRunningMateOffice == true;
    }

    public bool GetIsPrimaryRunningMateOffice(string officeKey)
    {
      var row = GetOfficesRow(officeKey);
      return row?.IsPrimaryRunningMateOffice == true;
    }

    public OfficeClass GetOfficeClass(string officeKey)
    {
      var row = GetOfficesRow(officeKey);
      if (row == null || !Enum.IsDefined(typeof (OfficeClass), row.OfficeLevel))
        return OfficeClass.Undefined;
      return (OfficeClass) row.OfficeLevel;
    }

    public int GetOfficeLevel(string officeKey)
    {
      var row = GetOfficesRow(officeKey);
      return row?.OfficeLevel ?? 0;
    }

    public string GetOfficeLine1(string officeKey)
    {
      var row = GetOfficesRow(officeKey);
      return row == null ? Empty : row.OfficeLine1;
    }

    public string GetOfficeLine2(string officeKey)
    {
      var row = GetOfficesRow(officeKey);
      return row == null ? Empty : row.OfficeLine2;
    }


    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public sealed class OfficesOfficialsCache
  {
    #region Private

    private bool _Initialized;
    private Dictionary<string, string> _IncumbentOfficeKeyByPoliticianKeyDictionary;
    private Dictionary<string, string> _IncumbentOfficeKeyByRunningMateKeyDictionary;

    private void Initialize()
    {
      _IncumbentOfficeKeyByPoliticianKeyDictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _IncumbentOfficeKeyByRunningMateKeyDictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _Initialized = true;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public string GetIncumbentOfficeKeyByPoliticianKey(string politicianKey)
    {
      if (!_Initialized) Initialize();
      if (politicianKey == null) return null;
      if (
        !_IncumbentOfficeKeyByPoliticianKeyDictionary.TryGetValue(
          politicianKey, out var officeKey))
      {
        officeKey =
          OfficesOfficials.GetIncumbentOfficeKeyByPoliticianKey(politicianKey);
        _IncumbentOfficeKeyByPoliticianKeyDictionary[politicianKey] = officeKey;
      }
      return officeKey;
    }

    public string GetIncumbentOfficeKeyByRunningMateKey(string runningMateKey)
    {
      if (!_Initialized) Initialize();
      if (runningMateKey == null) return null;
      if (
        !_IncumbentOfficeKeyByRunningMateKeyDictionary.TryGetValue(
          runningMateKey, out var officeKey))
      {
        officeKey =
          OfficesOfficials.GetIncumbentOfficeKeyByRunningMateKey(runningMateKey);
        _IncumbentOfficeKeyByRunningMateKeyDictionary[runningMateKey] = officeKey;
      }
      return officeKey;
    }


    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public sealed class OrganizationContactsCache
  {
    #region Private

    private bool _Initialized;
    private Dictionary<int, DataRow> _Dictionary;

    private void Initialize()
    {
      _Dictionary =
        new Dictionary<int, DataRow>();
      _Initialized = true;
    }

    private DataRow GetOrgContactCacheRow(int orgContactId)
    {
      if (!_Initialized) Initialize();
      if (!_Dictionary.TryGetValue(orgContactId, out var row))
      {
        var table = OrganizationContacts.GetCacheData(orgContactId);
        if (table.Rows.Count == 1)
          row = table.Rows[0];
        _Dictionary[orgContactId] = row;
      }
      return row;
    }

    #endregion

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public bool Exists(int orgContactId)
    {
      return GetOrgContactCacheRow(orgContactId) != null;
    }

    public string GetAddress1(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.Address1();
    }

    public string GetAddress2(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.Address2();
    }

    public string GetCity(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.City();
    }

    public string GetContact(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.Contact();
    }

    public string GetEmail(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.Email();
    }

    public string GetIdeology(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.Ideology();
    }

    public string GetName(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.Name();
    }

    public string GetOrgAbbreviation(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.OrgAbbreviation();
    }

    public string GetOrgSubType(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.OrgSubType();
    }

    public string GetOrgType(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.OrgType();
    }

    public string GetPhone(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.Phone();
    }

    public string GetStateCode(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.StateCode();
    }

    public string GetTitle(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.Title();
    }

    public string GetZip(int orgContactId)
    {
      var row = GetOrgContactCacheRow(orgContactId);
      return row == null ? Empty : row.Zip();
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion
  }

  public sealed class PartiesCache
  {
    #region Private

    private bool _Initialized;
    private Dictionary<string, PartiesRow> _Dictionary;

    private void Initialize()
    {
      _Dictionary =
        new Dictionary<string, PartiesRow>(StringComparer.OrdinalIgnoreCase);
      _Initialized = true;
    }

    private PartiesRow GetPartiesRow(string partyKey)
    {
      if (!_Initialized) Initialize();
      if (partyKey == null) return null;
      if (!_Dictionary.TryGetValue(partyKey, out var row))
      {
        var table = Parties.GetCacheData(partyKey);
        if (table.Count == 1)
          row = table[0];
        _Dictionary[partyKey] = row;
      }
      return row;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public bool Exists(string partyKey)
    {
      var row = GetPartiesRow(partyKey);
      return row != null;
    }

    public bool GetIsPartyMajor(string partyKey)
    {
      var row = GetPartiesRow(partyKey);
      return row?.IsPartyMajor == true;
    }

    public string GetPartyCode(string partyKey)
    {
      var row = GetPartiesRow(partyKey);
      return row == null ? Empty : row.PartyCode;
    }

    public string GetPartyName(string partyKey)
    {
      var row = GetPartiesRow(partyKey);
      return row == null ? Empty : row.PartyName;
    }

    public string GetPartyUrl(string partyKey)
    {
      var row = GetPartiesRow(partyKey);
      return row == null ? Empty : row.PartyUrl;
    }

    public string GetStateCode(string partyKey)
    {
      var row = GetPartiesRow(partyKey);
      return row == null ? Empty : row.StateCode;
    }

    public bool PartyKeyExists(string partyKey)
    {
      return GetPartiesRow(partyKey) != null;
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public sealed class PoliticiansCache
  {
    #region Private

    private bool _Initialized;
    private readonly PageCache _PageCache;
    private Dictionary<string, PoliticiansRow> _Dictionary;
    private Dictionary<string, string> _PoliticiansListForOfficeDictionary;

    private Dictionary<string, PoliticianOfficeStatus>
      _PoliticianOfficeStatusDictionary;

    public PoliticiansCache(PageCache pageCache)
    {
      _PageCache = pageCache;
    }

    private void Initialize()
    {
      _Dictionary =
        new Dictionary<string, PoliticiansRow>(StringComparer.OrdinalIgnoreCase);
      _PoliticiansListForOfficeDictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      _PoliticianOfficeStatusDictionary =
        new Dictionary<string, PoliticianOfficeStatus>(
          StringComparer.OrdinalIgnoreCase);
      _Initialized = true;
    }

    private static string FormatPoliticianListForOfficeDictionaryKey(
      string electionKey, string officeKey)
    {
      return electionKey + "|" + officeKey;
    }

    private PoliticiansRow GetPoliticiansRow(string politicianKey)
    {
      if (!_Initialized) Initialize();
      if (politicianKey == null) return null;
      if (!_Dictionary.TryGetValue(politicianKey, out var row))
      {
        var table = Politicians.GetCacheData(politicianKey);
        if (table.Count == 1)
          row = table[0];
        _Dictionary[politicianKey] = row;
      }
      return row;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public bool Exists(string politicianKey)
    {
      if (IsNullOrWhiteSpace(politicianKey)) return false;
      return GetPoliticiansRow(politicianKey) != null;
    }

    public string GetAge(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.Age;
    }

    public string GetBallotPediaWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.BallotPediaWebAddress;
    }

    public string GetBloggerWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.BloggerWebAddress;
    }

    public string GetCrowdpacWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.CrowdpacWebAddress;
    }

    public DateTime GetDateOfBirth(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row?.DateOfBirth ?? VotePage.DefaultDbDate;
    }

    public string GetDateOfBirthAsString(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.DateOfBirthAsString;
    }

    public string GetFacebookWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.FacebookWebAddress;
    }

    public string GetFirstName(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.FirstName;
    }

    public string GetFlickrWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.FlickrWebAddress;
    }

    public string GetFutureViewableElectionDescription(string politicianKey)
    {
      var electionKey = GetFutureViewableElectionKeyByPoliticianKey(politicianKey);
      return _PageCache.Elections.GetElectionDesc(electionKey);
    }

    public string GetFutureViewableElectionKey(string politicianKey)
    {
      var key = GetFutureViewableElectionKeyByPoliticianKey(politicianKey);
      if (IsNullOrWhiteSpace(key))
        key = GetFutureViewableElectionKeyByRunningMateKey(politicianKey);
      return key;
    }

    public string GetFutureViewableElectionKeyByPoliticianKey(string politicianKey)
    {
      return
        _PageCache.ElectionsPoliticians.GetFutureElectionKeyByPoliticianKey(
          politicianKey, true);
    }

    public string GetFutureViewableElectionKeyByRunningMateKey(string politicianKey)
    {
      return
        _PageCache.ElectionsPoliticians.GetFutureElectionKeyByRunningMateKey(
          politicianKey, true);
    }

    public string GetGoFundMeWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.GoFundMeWebAddress;
    }

    public string GetGooglePlusWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.GooglePlusWebAddress;
    }

    public string GetLastName(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.LastName;
    }

    public string GetLinkedInWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.LinkedInWebAddress;
    }

    public string GetLiveElectionKey(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.LiveElectionKey;
    }

    public string GetLiveOfficeKey(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.LiveOfficeKey;
    }

    public string GetLiveOfficeStatus(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.LiveOfficeStatus;
    }

    public PoliticianStatus GetLivePoliticianStatus(string politicianKey)
    {
      return GetLiveOfficeStatus(politicianKey).ToPoliticianStatus();
    }

    public string GetMiddleName(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.MiddleName;
    }

    public string GetNickname(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.Nickname;
    }

    public string GetOfficeAndStatus(string politicianKey)
    {
      var officeStatus = _PageCache.Politicians.GetOfficeStatus(politicianKey);
      if (!Offices.IsValid(officeStatus.OfficeKey)) return Empty;
      var officeName = Offices.GetLocalizedOfficeNameWithElectoralClass(_PageCache,
        officeStatus.OfficeKey);
      return officeStatus.PoliticianStatus.GetOfficeStatusDescription(officeName);
      //return G.Politician_Current_Office_And_Status(_PageCache, politicianKey);
    }

    public string GetOfficeKey(string politicianKey)
    {
      return GetOfficeStatus(politicianKey)
        .OfficeKey;
    }

    public PoliticianOfficeStatus GetOfficeStatus(string politicianKey)
    {
      if (!_Initialized) Initialize();
      if (politicianKey == null) return null;
      if (
        !_PoliticianOfficeStatusDictionary.TryGetValue(
          politicianKey, out var politicianOfficeStatus))
      {
        politicianOfficeStatus = Politicians.GetOfficeStatus(politicianKey);
        _PoliticianOfficeStatusDictionary[politicianKey] = politicianOfficeStatus;
      }
      return politicianOfficeStatus;
    }

    public string GetPartyKey(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.PartyKey;
    }

    public string GetPinterestWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.PinterestWebAddress;
    }

    public string GetPodcastWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.PodcastWebAddress;
    }

    public string GetPoliticianListForOffice(string electionKey, string officeKey)
    {
      if (!_Initialized) Initialize();
      if (electionKey == null || officeKey == null) return null;
      var key = FormatPoliticianListForOfficeDictionaryKey(electionKey, officeKey);
      if (!_PoliticiansListForOfficeDictionary.TryGetValue(key, out var politicianList))
      {
        politicianList = ElectionsPoliticians.GetPoliticianListForOfficeInElection(
          electionKey, officeKey);
        _PoliticiansListForOfficeDictionary[key] = politicianList;
      }
      return politicianList;
    }

    public string GetPoliticianName(string politicianKey, bool includeAddOn = false,
      int breakAfterPosition = 0)
    {
      return Politicians.FormatName(
        GetPoliticiansRow(politicianKey), includeAddOn, breakAfterPosition);
    }

    public PoliticianStatus GetPoliticianStatus(string politicianKey)
    {
      return GetOfficeStatus(politicianKey)
        .PoliticianStatus;
    }

    public string GetPublicAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.PublicAddress;
    }

    public string GetPublicCityStateZip(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.PublicCityStateZip;
    }

    public string GetPublicEmail(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.PublicEmail;
    }

    public string GetPublicPhone(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.PublicPhone;
    }

    public string GetPublicWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.PublicWebAddress;
    }

    public string GetRssFeedWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.RSSFeedWebAddress;
    }

    public string GetStatePhone(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.StatePhone;
    }

    public string GetSuffix(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.Suffix;
    }

    public string GetTwitterWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.TwitterWebAddress;
    }

    public string GetVimeoWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.VimeoWebAddress;
    }

    public string GetWebstagramWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.WebstagramWebAddress;
    }

    public string GetWikipediaWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.WikipediaWebAddress;
    }

    public string GetYouTubeWebAddress(string politicianKey)
    {
      var row = GetPoliticiansRow(politicianKey);
      return row == null ? Empty : row.YouTubeWebAddress;
    }

    //public bool IsInUpcomingViewableElection(string politicanKey)
    //{
    //  return G.Is_Politician_In_Election_Upcoming_Viewable(_PageCache, politicanKey);
    //}


    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  //public sealed class QuestionsCache
  //{
  //  #region Private

  //  private bool _Initialized;
  //  private Dictionary<string, QuestionsTable> _NonOmmitedByIssueKeyDictionary;

  //  private void Initialize()
  //  {
  //    _NonOmmitedByIssueKeyDictionary =
  //      new Dictionary<string, QuestionsTable>(StringComparer.OrdinalIgnoreCase);
  //    _Initialized = true;
  //  }

  //  #endregion Private

  //  #region Public

  //  // ReSharper disable MemberCanBePrivate.Global
  //  // ReSharper disable MemberCanBeProtected.Global
  //  // ReSharper disable UnusedMember.Global

  //  public QuestionsTable GetNonOmittedDataByIssueKey(string issueKey)
  //  {
  //    if (!_Initialized) Initialize();
  //    if (issueKey == null) return null;
  //    if (!_NonOmmitedByIssueKeyDictionary.TryGetValue(issueKey, out var table))
  //    {
  //      table = Questions.GetNonOmittedDataByIssueKey(issueKey);
  //      _NonOmmitedByIssueKeyDictionary[issueKey] = table;
  //    }
  //    return table;
  //  }


  //  // ReSharper restore UnusedMember.Global
  //  // ReSharper restore MemberCanBeProtected.Global
  //  // ReSharper restore MemberCanBePrivate.Global

  //  #endregion Public
  //}

  public sealed class ReferendumsCache
  {
    #region Private

    private bool _Initialized;
    private Dictionary<string, ReferendumsRow> _Dictionary;

    private void Initialize()
    {
      _Dictionary =
        new Dictionary<string, ReferendumsRow>(StringComparer.OrdinalIgnoreCase);
      _Initialized = true;
    }

    private static string FormatDictionaryKey(string electionKey, string referendumKey)
    {
      return electionKey + "|" + referendumKey;
    }

    private ReferendumsRow GetReferendumsRow(
      string electionKey, string referendumKey)
    {
      if (!_Initialized) Initialize();
      if (electionKey == null || referendumKey == null) return null;
      var dictionaryKey = FormatDictionaryKey(electionKey, referendumKey);
      if (!_Dictionary.TryGetValue(dictionaryKey, out var row))
      {
        var table = Referendums.GetCacheData(electionKey, referendumKey);
        if (table.Count == 1)
          row = table[0];
        _Dictionary[dictionaryKey] = row;
      }
      return row;
    }

    #endregion Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public bool Exists(string electionKey, string referendumKey)
    {
      if (IsNullOrWhiteSpace(electionKey) ||
        IsNullOrWhiteSpace(referendumKey)) return false;
      var row = GetReferendumsRow(electionKey, referendumKey);
      return row != null;
    }

    public string GetReferendumDescription(string electionKey, string referendumKey)
    {
      var row = GetReferendumsRow(electionKey, referendumKey);
      return row == null ? Empty : row.ReferendumDescription;
    }

    public string GetReferendumDetail(string electionKey, string referendumKey)
    {
      var row = GetReferendumsRow(electionKey, referendumKey);
      return row == null ? Empty : row.ReferendumDetail;
    }

    public string GetReferendumDetailUrl(string electionKey, string referendumKey)
    {
      var row = GetReferendumsRow(electionKey, referendumKey);
      return row == null ? Empty : row.ReferendumDetailUrl;
    }

    public string GetReferendumFullText(string electionKey, string referendumKey)
    {
      var row = GetReferendumsRow(electionKey, referendumKey);
      return row == null ? Empty : row.ReferendumFullText;
    }

    public string GetReferendumFullTextUrl(string electionKey, string referendumKey)
    {
      var row = GetReferendumsRow(electionKey, referendumKey);
      return row == null ? Empty : row.ReferendumFullTextUrl;
    }

    public string GetReferendumTitle(string electionKey, string referendumKey)
    {
      var row = GetReferendumsRow(electionKey, referendumKey);
      return row == null ? Empty : row.ReferendumTitle;
    }

    public string GetStateCode(string electionKey, string referendumKey)
    {
      var row = GetReferendumsRow(electionKey, referendumKey);
      return row == null ? Empty : row.StateCode;
    }


    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public sealed class PageCache
  {
    #region Private

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    private PageCache(bool isTemporary)
      : this()
    {
      IsTemporary = isTemporary;
    }

    #endregion Private

    public readonly AddressesCache Addresses = new AddressesCache();
    public readonly DonationsCache Donations = new DonationsCache();
    public readonly ElectionsCache Elections = new ElectionsCache();
    public readonly ElectionsPoliticiansCache ElectionsPoliticians =
      new ElectionsPoliticiansCache();
    public readonly IssuesCache Issues = new IssuesCache();
    public readonly LocalDistrictsCache LocalDistricts = new LocalDistrictsCache();
    public readonly OfficesCache Offices = new OfficesCache();
    public readonly OfficesOfficialsCache OfficesOfficials =
      new OfficesOfficialsCache();
    public readonly OrganizationContactsCache OrgContacts = new OrganizationContactsCache();
    public readonly PartiesCache Parties = new PartiesCache();
    public readonly PoliticiansCache Politicians;
    //public readonly QuestionsCache Questions = new QuestionsCache();
    public readonly ReferendumsCache Referendums = new ReferendumsCache();
    public PageCache()
    {
      Politicians = new PoliticiansCache(this);
    }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool IsTemporary { get; }

    public static PageCache GetTemporary()
    {
      return new PageCache(true);
    }


    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}