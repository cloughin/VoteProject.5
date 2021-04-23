using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using DB;
using DB.Vote;
using EGIS.ShapeFileLib;
using MySql.Data.MySqlClient;
using Vote;

namespace AnalyzeTigerPlaces
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
      InitializeForm();
    }

    //private const string StatCodeCity = "25";
    private const string StatCodeCdp = "57";

    private void InitializeForm()
    {
      foreach (var stateCode in StateCache.All51StateCodes)
        StateDropDown.Items.Add(stateCode);
    }

    private static DataTable GetVoteData(string stateCode)
    {
      const string cmdText =
        "SELECT l.StateCode,l.CountyCode,l.LocalCode,c.County,l.localDistrict," +
        "(SELECT SUBSTR(ElectionKey, 3, 4) FROM ElectionsPoliticians ep" +
        " WHERE ep.StateCode = l.StateCode AND ep.CountyCode = l.CountyCode" +
        " AND ep.LocalCode = l.LocalCode" +
        " ORDER BY ep.ElectionKey DESC LIMIT 1) AS LastElectionYear" +
        " FROM LocalDistricts l" + 
        " INNER JOIN Counties c ON c.StateCode = l.StateCode AND" +
        " c.CountyCode = l.CountyCode AND c.IsCountyTagForDeletion=0" +
        " WHERE l.StateCode = @StateCode AND l.IsLocalDistrictTagForDeletion=0" +
        " ORDER BY c.County,l.localDistrict";

      var cmd = VoteDb.GetCommand(cmdText, 0);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      try
      {
        var stateCode = StateDropDown.SelectedItem as string;
        if (string.IsNullOrWhiteSpace(stateCode))
          throw new Exception("Please select a State");
        //var stateFips = States.GetLdsStateCode(stateCode);
        var optionsFilename = OptionsFileTextBox.Text;
        if (string.IsNullOrWhiteSpace(optionsFilename))
          throw new Exception("Please select a JSON Options file");
        var tigerFolderPath = TigerFolderTextBox.Text;
        if (string.IsNullOrWhiteSpace(tigerFolderPath))
          throw new Exception("Please select Tiger Data Folder");
        var outputFilename = OutputFileTextBox.Text;
        if (string.IsNullOrWhiteSpace(outputFilename))
          throw new Exception("Please select an output file");
        var optionsJson = File.ReadAllText(optionsFilename);
        var serializer = new JavaScriptSerializer();
        var allOptions = serializer.Deserialize<Dictionary<string, OptionGroup>>(optionsJson);
        if (!allOptions.ContainsKey(stateCode))
          throw new Exception($"No Options Group for {stateCode}");

        var options = allOptions[stateCode];
        var ignoreUnmatchedCousubs =
          options.IgnoreUnmatchedCousubs?.ToDictionary(s => s, s => (object) null) ??
          new Dictionary<string, object>();
        var ignoreCousubs =
          options.IgnoreCousubs?.ToDictionary(s => s, s => (object) null) ??
          new Dictionary<string, object>();
        var ignoreUnmatchedPlaces =
          options.IgnoreUnmatchedPlaces?.ToDictionary(s => s, s => (object) null) ??
          new Dictionary<string, object>();
        var countyWideLocalsDictionary =
          options.CountyWideLocals?.ToDictionary(l => l, l => (object) null) ??
          new Dictionary<LocalId, object>();
        if (options.CountyWideLocalsTemp != null)
          foreach(var id in options.CountyWideLocalsTemp)
            countyWideLocalsDictionary.Add(id, null);
        var namesToMatch = options.NamesToMatch?.ToDictionary(n => n.Fips, n => n.Name) ??
          new Dictionary<string, string>();

        var voteData =
          GetVoteData(stateCode)
            .Rows.Cast<DataRow>()
            .Select(
              r =>
                new VoteInfo
                {
                  CountyCode = r.CountyCode(),
                  LocalCode = r.LocalCode(),
                  CountyName = r.County(),
                  LocalName = r.LocalDistrict(),
                  LastElectionYear = r.LastElectionYear() ?? "none"
                })
            .ToList();

        var voteDataByCounty =
          voteData
            .GroupBy(i => i.CountyCode)
            .ToDictionary(g => g.Key, g => g.ToList());

        var tigerData = TigerPlacesCounties.GetAllDataByStateCode(stateCode);

        var cousubDatabaseData = GetCousubDatabaseData(tigerData, options);
        var cousubDatabaseDataByCounty = cousubDatabaseData.GroupBy(i => i.CountyCode)
          .ToDictionary(g => g.Key, g => g.ToList());
        var placeDatabaseData = GetPlaceDatabaseData(tigerData, options);
        var placeDatabaseDataByCounty = placeDatabaseData.GroupBy(i => i.CountyCode)
          .ToDictionary(g => g.Key, g => g.ToList());

        //var cousubShapeData = GetCousubShapeData(stateFips, tigerFolderPath, options);
        //var cousubShapeDataByCounty = cousubShapeData.GroupBy(i => i.CountyFips)
        //  .ToDictionary(g => g.Key, g => g.ToList());
        //var placeShapeData = GetPlaceShapeData(stateFips, tigerFolderPath, options);
        //var placeShapeDictionary = placeShapeData.ToDictionary(p => p.PlaceFips, p => p);

        // match cousubs by name
        var cousubsMatched = 0;
        var cousubsIgnored = 0;
        var cousubsNotMatched = 0;
        var cousubDuplicates = 0;
        //MatchCousubsFromShapefileByName(voteDataByCounty, cousubShapeDataByCounty,
        //  ref cousubsMatched, ref cousubsNotMatched, ref cousubDuplicates);
        foreach (var kvp in voteDataByCounty)
        {
          var countyCode = kvp.Key;
          var vInfos = kvp.Value;
          if (cousubDatabaseDataByCounty.ContainsKey(countyCode))
          {
            var cInfos = cousubDatabaseDataByCounty[countyCode];
            foreach (var cInfo in cInfos.Where(c => !ignoreCousubs.ContainsKey(c.TigerCode)))
            {
              var matches =
                 vInfos.Where(v => cInfo.MatchesVoteName(v.LocalName, namesToMatch, options)).ToList();
              if (matches.Count == 0)
              {
                if (ignoreUnmatchedCousubs.ContainsKey(cInfo.TigerCode))
                  cousubsIgnored++;
                else
                  cousubsNotMatched++;
              }
              else
              {
                cInfo.Matched = true;
                if (matches.Count == 1)
                {
                  cousubsMatched++;
                  matches[0].CousubFips = cInfo.TigerCode;
                }
                else
                {
                  cousubDuplicates++;
                  cInfo.IsDuplicate = true;
                }
              }
            }
          }
        }

        // match places by name
        var placesMatched = 0;
        var placesNotMatched = 0;
        var placeDuplicates = 0;
        //MatchPlacesFromShapefileByName(voteData, placeShapeData, ref placesMatched, ref placesNotMatched);
        foreach (var kvp in voteDataByCounty)
        {
          var countyCode = kvp.Key;
          var vInfos = kvp.Value;
          if (placeDatabaseDataByCounty.ContainsKey(countyCode))
          {
            var pInfos = placeDatabaseDataByCounty[countyCode];
            foreach (var pInfo in pInfos)
            {
              var matches =
                 vInfos.Where(v => pInfo.MatchesVoteName(v.LocalName, namesToMatch, options)).ToList();
              if (matches.Count == 0)
                placesNotMatched++;
              else
              {
                pInfo.Matched = true;
                if (matches.Count == 1)
                {
                  placesMatched++;
                  matches[0].PlaceFips = pInfo.TigerCode;
                }
                else
                {
                  placeDuplicates++;
                  pInfo.IsDuplicate = true;
                }
              }
            }
          }
        }

        // build spreadsheet
        //BuildSpreadsheetFromShapefiles(stateCode, outputFilename, countyWideLocalsDictionary,
        //  voteDataByCounty, cousubShapeDataByCounty, placeShapeData, placeShapeDictionary);
        BuildSpreadsheetFromDatabase(stateCode, outputFilename, countyWideLocalsDictionary,
          voteDataByCounty, cousubDatabaseDataByCounty, placeDatabaseDataByCounty,
          ignoreUnmatchedCousubs, ignoreUnmatchedPlaces);

        BuildUpdateCsv(stateCode, outputFilename, voteDataByCounty, countyWideLocalsDictionary);

        var localsMatched = 0;
        var localsUnmatched = 0;
        foreach (var vInfo in voteData)
        {
          if (string.IsNullOrWhiteSpace(vInfo.CousubFips) &&
            string.IsNullOrWhiteSpace(vInfo.PlaceFips) &&
            !countyWideLocalsDictionary.ContainsKey(new LocalId(vInfo.CountyCode, vInfo.LocalCode)))
            localsUnmatched++;
          else
          {
            localsMatched++;
          }
        }

        MessageBox.Show($"Cousubs Matched={cousubsMatched}\n" +
          $"Cousubs Ignored={cousubsIgnored}\n" +
          $"Cousub Duplicates={cousubDuplicates}\n" +
          $"Cousubs Not Matched={cousubsNotMatched}\n" +
          $"Places Matched={placesMatched}\n" +
          $"Place Duplicates={placeDuplicates}\n" +
          $"Places Not Matched={placesNotMatched}\n" +
          $"Vote Locals Matched={localsMatched}\n" +
          $"Vote Locals Not Matched={localsUnmatched}");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    //private static string CombineNames(string name1, string name2)
    //{
    //  if (string.IsNullOrWhiteSpace(name2)) return name1;
    //  return name1 + ", " + name2;
    //}

    private static void BuildUpdateCsv(string stateCode, string outputFilename,
      IReadOnlyDictionary<string, List<VoteInfo>> voteDataByCounty,
      IReadOnlyDictionary<LocalId, object> countyWideLocalsDictionary)
    {
      // use same filename but as ".update.txt";
      var filename = Path.ChangeExtension(outputFilename, ".update.txt");
      using (
        var textWriter = new StreamWriter(filename, false, System.Text.Encoding.UTF8))
      {
        var csvWriter = new SimpleCsvWriter();
        csvWriter.AddField("StateCode");
        csvWriter.AddField("LocalType");
        csvWriter.AddField("LocalId");
        csvWriter.AddField("CountyCode");
        csvWriter.AddField("LocalCode");
        csvWriter.Write(textWriter);
        foreach (var kvp in
          voteDataByCounty.OrderBy(kvp => kvp.Value.First().CountyCode))
          foreach (var v in kvp.Value.OrderBy(v => v.LocalCode))
          {
            if (!string.IsNullOrWhiteSpace(v.CousubFips))
            {
              csvWriter.AddField(stateCode);
              csvWriter.AddField("T");
              csvWriter.AddField(v.CousubFips);
              csvWriter.AddField(v.CountyCode);
              csvWriter.AddField(v.LocalCode);
              csvWriter.Write(textWriter);
            }
            if (!string.IsNullOrWhiteSpace(v.PlaceFips))
            {
              csvWriter.AddField(stateCode);
              csvWriter.AddField("T");
              csvWriter.AddField(v.PlaceFips);
              csvWriter.AddField(v.CountyCode);
              csvWriter.AddField(v.LocalCode);
              csvWriter.Write(textWriter);
            }
          }
        var firstCode = 10001;
        foreach (
          var kvp in
          countyWideLocalsDictionary.OrderBy(kvp => kvp.Key.CountyCode)
            .ThenBy(kvp => kvp.Key.LocalCode))
        {
          csvWriter.AddField(stateCode);
          csvWriter.AddField("V");
          csvWriter.AddField(firstCode++.ToString());
          csvWriter.AddField(kvp.Key.CountyCode);
          csvWriter.AddField(kvp.Key.LocalCode);
          csvWriter.Write(textWriter);
        }
        textWriter.Close();
      }
    }

    private static void BuildSpreadsheetFromDatabase(string stateCode, string outputFilename,
      IReadOnlyDictionary<LocalId, object> countyWideLocalsDictionary,
      IReadOnlyDictionary<string, List<VoteInfo>> voteDataByCounty,
      IReadOnlyDictionary<string, List<DbPlaceInfo>> cousubDatabaseDataByCounty,
      IReadOnlyDictionary<string, List<DbPlaceInfo>> placeDatabaseDataByCounty,
      IReadOnlyDictionary<string, object> ignoreUnmatchedCousubs,
      IReadOnlyDictionary<string, object> ignoreUnmatchedPlaces)
    {
      using (var textWriter = new StreamWriter(outputFilename, false, System.Text.Encoding.UTF8))
      {
        var csvWriter = new SimpleCsvWriter();
        csvWriter.AddField("ST");
        csvWriter.AddField("CTY");
        csvWriter.AddField("LC");
        csvWriter.AddField("CTYWIDE");
        csvWriter.AddField("County Name");
        csvWriter.AddField("Local Name");
        csvWriter.AddField("Last ElectionYear");
        csvWriter.AddField("FIPS");
        csvWriter.AddField("#CTY");
        csvWriter.AddField("Name");
        csvWriter.AddField("Long Name");
        csvWriter.AddField("FIPS");
        csvWriter.AddField("#CTY");
        csvWriter.AddField("Name");
        csvWriter.AddField("Long Name");
        csvWriter.AddField("Notes");
        csvWriter.Write(textWriter);

        foreach (var kvp in voteDataByCounty.OrderBy(kvp => kvp.Value.First().CountyName))
        {
          List<DbPlaceInfo> cInfos = null;
          if (cousubDatabaseDataByCounty.ContainsKey(kvp.Key))
            cInfos = cousubDatabaseDataByCounty[kvp.Key];
          List<DbPlaceInfo> pInfos = null;
          if (placeDatabaseDataByCounty.ContainsKey(kvp.Key))
            pInfos = placeDatabaseDataByCounty[kvp.Key];
          foreach (var v in kvp.Value.OrderBy(v => v.LocalName))
          {
            var notes = new List<string>();
            var isCountyWide =
              countyWideLocalsDictionary.ContainsKey(new LocalId(v.CountyCode, v.LocalCode));
            csvWriter.AddField(stateCode);
            csvWriter.AddField(v.CountyCode);
            csvWriter.AddField(v.LocalCode);
            csvWriter.AddField(isCountyWide ? "X" : string.Empty);
            csvWriter.AddField(v.CountyName);
            csvWriter.AddField(v.LocalName);
            csvWriter.AddField(v.LastElectionYear);
            DbPlaceInfo cInfo = null;
            if (cInfos != null)
              cInfo = cInfos.FirstOrDefault(c => c.TigerCode == v.CousubFips);
            if (cInfo == null)
            {
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
            }
            else
            {
              csvWriter.AddField(cInfo.TigerCode);
              csvWriter.AddField(cInfo.CountyCount > 1 ? cInfo.CountyCount.ToString() : string.Empty);
              //csvWriter.AddField(CombineNames(cInfo.ShortName, cInfo.Name2));
              csvWriter.AddField(cInfo.Name);
              csvWriter.AddField(cInfo.LongName);
            }
            DbPlaceInfo pInfo = null;
            if (pInfos != null)
              pInfo = pInfos.FirstOrDefault(p => p.TigerCode == v.PlaceFips);
            if (pInfo == null)
            {
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
            }
            else
            {
              csvWriter.AddField(pInfo.TigerCode);
              csvWriter.AddField(pInfo.CountyCount > 1 ? pInfo.CountyCount.ToString() : string.Empty);
              //csvWriter.AddField(CombineNames(pInfo.ShortName, pInfo.Name2));
              csvWriter.AddField(pInfo.Name);
              csvWriter.AddField(pInfo.LongName);
            }
            //if (string.IsNullOrWhiteSpace(v.PlaceFips))
            //{
            //  csvWriter.AddField(string.Empty);
            //  csvWriter.AddField(string.Empty);
            //  csvWriter.AddField(string.Empty);
            //  csvWriter.AddField(string.Empty);
            //}
            //else
            //{
            //  var pInfo = placeShapeDictionary[v.PlaceFips];
            //  csvWriter.AddField(pInfo.PlaceFips);
            //  csvWriter.AddField(string.Empty);
            //  csvWriter.AddField(pInfo.PlaceName);
            //  csvWriter.AddField(pInfo.PlaceLongName);
            //  foreach (var inCounty in pInfo.InVoteCounties.Where(i => i != kvp.Key))
            //    notes.Add($"Also in {CountyCache.GetCountyName(stateCode, inCounty)}");
            //  if (!pInfo.InTigerCounties.Contains(v.CountyCode))
            //    notes.Add("Place not in this county in Tiger data");
            //}
            if (string.IsNullOrWhiteSpace(v.CousubFips) &&
              string.IsNullOrWhiteSpace(v.PlaceFips) && !isCountyWide)
              notes.Add("No match from cousub or place");
            csvWriter.AddField(string.Join("\n", notes));
            csvWriter.Write(textWriter);
          }
          if (cInfos != null)
            foreach (var c in cInfos.OrderBy(c => c.Name))
            {
              if (!c.Matched && ignoreUnmatchedCousubs.ContainsKey(c.TigerCode))
                c.Matched = true;
              if (c.IsDuplicate || !c.Matched)
              {
                csvWriter.AddField(stateCode);
                csvWriter.AddField(c.CountyCode);
                csvWriter.AddField("??");
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(CountyCache.GetCountyName(stateCode, c.CountyCode));
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(c.TigerCode);
                csvWriter.AddField(c.CountyCount > 1 ? c.CountyCount.ToString() : string.Empty);
                //csvWriter.AddField(CombineNames(c.ShortName, c.Name2));
                csvWriter.AddField(c.Name);
                csvWriter.AddField(c.LongName);
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(c.IsDuplicate
                  ? "Duplicate in vote data"
                  : "Unmatched");
                csvWriter.Write(textWriter);
              }
            }
          if (pInfos != null)
            foreach (var p in pInfos.OrderBy(c => c.Name))
            {
              if (!p.Matched && ignoreUnmatchedPlaces.ContainsKey(p.TigerCode))
                p.Matched = true;
              if (p.IsDuplicate || !p.Matched)
              {
                csvWriter.AddField(stateCode);
                csvWriter.AddField(p.CountyCode);
                csvWriter.AddField("??");
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(CountyCache.GetCountyName(stateCode, p.CountyCode));
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(string.Empty);
                csvWriter.AddField(p.TigerCode);
                csvWriter.AddField(p.CountyCount > 1 ? p.CountyCount.ToString() : string.Empty);
                //csvWriter.AddField(CombineNames(p.ShortName, p.Name2));
                csvWriter.AddField(p.Name);
                csvWriter.AddField(p.LongName);
                csvWriter.AddField(p.IsDuplicate
                  ? "Duplicate in vote data"
                  : "Unmatched");
                csvWriter.Write(textWriter);
              }
            }
        }

        // there could be counties omitted from the vote data
        var missingCounties =
          cousubDatabaseDataByCounty.Select(g => g.Key)
            .Where(k => !voteDataByCounty.ContainsKey(k))
            .Union(
              placeDatabaseDataByCounty.Select(g => g.Key)
                .Where(k => !voteDataByCounty.ContainsKey(k)))
            .Distinct()
            .OrderBy(k => k);
        foreach (var countyCode in missingCounties)
        {
          if (cousubDatabaseDataByCounty.ContainsKey(countyCode))
            foreach (var c in cousubDatabaseDataByCounty[countyCode])
            {
              csvWriter.AddField(stateCode);
              csvWriter.AddField(c.CountyCode);
              csvWriter.AddField("??");
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(c.TigerCode);
              csvWriter.AddField(c.CountyCount > 1 ? c.CountyCount.ToString() : string.Empty);
              //csvWriter.AddField(CombineNames(c.ShortName, c.Name2));
              csvWriter.AddField(c.Name);
              csvWriter.AddField(c.LongName);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField("No locals found for this county in vote data");
              csvWriter.Write(textWriter);
            }
          if (placeDatabaseDataByCounty.ContainsKey(countyCode))
            foreach (var p in placeDatabaseDataByCounty[countyCode])
            {
              csvWriter.AddField(stateCode);
              csvWriter.AddField(p.CountyCode);
              csvWriter.AddField("??");
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(string.Empty);
              csvWriter.AddField(p.TigerCode);
              csvWriter.AddField(p.CountyCount > 1 ? p.CountyCount.ToString() : string.Empty);
              //csvWriter.AddField(CombineNames(p.ShortName, p.Name2));
              csvWriter.AddField(p.Name);
              csvWriter.AddField(p.LongName);
              csvWriter.AddField("No locals found for this county in vote data");
              csvWriter.Write(textWriter);
            }
        }

        //foreach (
        //  var p in
        //  placeShapeData.Where(p => p.InVoteCounties.Count == 0).OrderBy(p => p.PlaceLongName))
        //{
        //  csvWriter.AddField(stateCode);
        //  csvWriter.AddField("???");
        //  csvWriter.AddField("??");
        //  csvWriter.AddField(string.Empty);
        //  csvWriter.AddField(string.Empty);
        //  csvWriter.AddField(string.Empty);
        //  csvWriter.AddField(string.Empty);
        //  csvWriter.AddField(string.Empty);
        //  csvWriter.AddField(string.Empty);
        //  csvWriter.AddField(string.Empty);
        //  csvWriter.AddField(string.Empty);
        //  csvWriter.AddField(p.PlaceFips);
        //  csvWriter.AddField(string.Empty);
        //  csvWriter.AddField(p.PlaceName);
        //  csvWriter.AddField(p.PlaceLongName);
        //  csvWriter.AddField("Unmatched place");
        //  csvWriter.Write(textWriter);
        //}
        textWriter.Close();
      }
    }

    //private static void BuildSpreadsheetFromShapefiles(string stateCode, string outputFilename,
    //  IReadOnlyDictionary<LocalId, object> countyWideLocalsDictionary,
    //  Dictionary<string, List<VoteInfo>> voteDataByCounty,
    //  IReadOnlyDictionary<string, List<CousubInfo>> cousubShapeDataByCounty,
    //  IEnumerable<PlaceInfo> placeShapeData,
    //  IReadOnlyDictionary<string, PlaceInfo> placeShapeDictionary)
    //{
    //  var textWriter = new StreamWriter(outputFilename);
    //  var csvWriter = new SimpleCsvWriter();
    //  csvWriter.AddField("ST");
    //  csvWriter.AddField("CTY");
    //  csvWriter.AddField("LC");
    //  csvWriter.AddField("CTYWIDE");
    //  csvWriter.AddField("County Name");
    //  csvWriter.AddField("Local Name");
    //  csvWriter.AddField("Last ElectionYear");
    //  csvWriter.AddField("FIPS");
    //  csvWriter.AddField("Name");
    //  csvWriter.AddField("Long Name");
    //  csvWriter.AddField("FIPS");
    //  csvWriter.AddField("Name");
    //  csvWriter.AddField("Long Name");
    //  csvWriter.AddField("Notes");
    //  csvWriter.Write(textWriter);

    //  foreach (var kvp in voteDataByCounty.OrderBy(kvp => kvp.Value.First().CountyName))
    //  {
    //    List<CousubInfo> cInfos = null;
    //    if (cousubShapeDataByCounty.ContainsKey(kvp.Key))
    //      cInfos = cousubShapeDataByCounty[kvp.Key];
    //    foreach (var v in kvp.Value.OrderBy(v => v.LocalName))
    //    {
    //      var notes = new List<string>();
    //      var isCountyWide =
    //        countyWideLocalsDictionary.ContainsKey(new LocalId(v.CountyCode, v.LocalCode));
    //      csvWriter.AddField(stateCode);
    //      csvWriter.AddField(v.CountyCode);
    //      csvWriter.AddField(v.LocalCode);
    //      csvWriter.AddField(isCountyWide ? "X" : string.Empty);
    //      csvWriter.AddField(v.CountyName);
    //      csvWriter.AddField(v.LocalName);
    //      csvWriter.AddField(v.LastElectionYear);
    //      CousubInfo cInfo = null;
    //      if (cInfos != null)
    //        cInfo = cInfos.FirstOrDefault(c => c.CousubFips == v.CousubFips);
    //      if (cInfo == null)
    //      {
    //        csvWriter.AddField(string.Empty);
    //        csvWriter.AddField(string.Empty);
    //        csvWriter.AddField(string.Empty);
    //      }
    //      else
    //      {
    //        csvWriter.AddField(cInfo.CousubFips);
    //        csvWriter.AddField(cInfo.CousubName);
    //        csvWriter.AddField(cInfo.CousubLongName);
    //      }
    //      if (string.IsNullOrWhiteSpace(v.PlaceFips))
    //      {
    //        csvWriter.AddField(string.Empty);
    //        csvWriter.AddField(string.Empty);
    //        csvWriter.AddField(string.Empty);
    //      }
    //      else
    //      {
    //        var pInfo = placeShapeDictionary[v.PlaceFips];
    //        csvWriter.AddField(pInfo.PlaceFips);
    //        csvWriter.AddField(pInfo.PlaceName);
    //        csvWriter.AddField(pInfo.PlaceLongName);
    //        foreach (var inCounty in pInfo.InVoteCounties.Where(i => i != kvp.Key))
    //          notes.Add($"Also in {CountyCache.GetCountyName(stateCode, inCounty)}");
    //        if (!pInfo.InTigerCounties.Contains(v.CountyCode))
    //          notes.Add("Place not in this county in Tiger data");
    //      }
    //      if (string.IsNullOrWhiteSpace(v.CousubFips) &&
    //        string.IsNullOrWhiteSpace(v.PlaceFips) && !isCountyWide)
    //        notes.Add("No match from cousub or place");
    //      csvWriter.AddField(string.Join("\n", notes));
    //      csvWriter.Write(textWriter);
    //    }
    //    if (cInfos != null)
    //      foreach (var c in cInfos.OrderBy(c => c.CousubLongName))
    //        if (c.IsDuplicate || !c.Matched)
    //        {
    //          csvWriter.AddField(stateCode);
    //          csvWriter.AddField(c.CountyFips);
    //          csvWriter.AddField("??");
    //          csvWriter.AddField(string.Empty);
    //          csvWriter.AddField(CountyCache.GetCountyName(stateCode, c.CountyFips));
    //          csvWriter.AddField(string.Empty);
    //          csvWriter.AddField(string.Empty);
    //          csvWriter.AddField(c.CousubFips);
    //          csvWriter.AddField(c.CousubName);
    //          csvWriter.AddField(c.CousubLongName);
    //          csvWriter.AddField(string.Empty);
    //          csvWriter.AddField(string.Empty);
    //          csvWriter.AddField(string.Empty);
    //          csvWriter.AddField(c.IsDuplicate
    //            ? "Duplicate in vote data"
    //            : "Unmatched");
    //          csvWriter.Write(textWriter);
    //        }
    //  }

    //  foreach (
    //    var p in
    //    placeShapeData.Where(p => p.InVoteCounties.Count == 0).OrderBy(p => p.PlaceLongName))
    //  {
    //    csvWriter.AddField(stateCode);
    //    csvWriter.AddField("???");
    //    csvWriter.AddField("??");
    //    csvWriter.AddField(string.Empty);
    //    csvWriter.AddField(string.Empty);
    //    csvWriter.AddField(string.Empty);
    //    csvWriter.AddField(string.Empty);
    //    csvWriter.AddField(string.Empty);
    //    csvWriter.AddField(string.Empty);
    //    csvWriter.AddField(string.Empty);
    //    csvWriter.AddField(p.PlaceFips);
    //    csvWriter.AddField(p.PlaceName);
    //    csvWriter.AddField(p.PlaceLongName);
    //    csvWriter.AddField("Unmatched place");
    //    csvWriter.Write(textWriter);
    //  }
    //  textWriter.Close();
    //}

    //private static void MatchPlacesFromShapefileByName(List<VoteInfo> voteData, 
    //  IEnumerable<PlaceInfo> placeData, ref int placesMatched, ref int placesNotMatched)
    //{
    //  foreach (var pInfo in placeData)
    //  {
    //    var matches =
    //      voteData.Where(
    //        v =>
    //          v.LocalName.IsEqIgnoreCase(pInfo.PlaceName) ||
    //          v.LocalName.IsEqIgnoreCase(pInfo.PlaceLongName)).ToList();
    //    if (matches.Count == 0)
    //      placesNotMatched++;
    //    else
    //    {
    //      placesMatched++;
    //      foreach (var m in matches)
    //      {
    //        pInfo.InVoteCounties.Add(m.CountyCode);
    //        m.PlaceFips = pInfo.PlaceFips;
    //      }
    //    }
    //  }
    //}

    //private static void MatchCousubsFromShapefileByName(Dictionary<string, 
    //  List<VoteInfo>> voteDataByCounty, 
    //  IReadOnlyDictionary<string, List<CousubInfo>> cousubDataByCounty, 
    //  ref int cousubsMatched, ref int cousubsNotMatched, ref int cousubDuplicates)
    //{
    //  foreach (var kvp in voteDataByCounty)
    //  {
    //    var countyFips = kvp.Key;
    //    var vInfos = kvp.Value;
    //    if (cousubDataByCounty.ContainsKey(countyFips))
    //    {
    //      var cInfos = cousubDataByCounty[countyFips];
    //      foreach (var cInfo in cInfos)
    //      {
    //        var matches =
    //          vInfos.Where(
    //            v =>
    //              v.LocalName.IsEqIgnoreCase(cInfo.CousubName) ||
    //              v.LocalName.IsEqIgnoreCase(cInfo.CousubLongName)).ToList();
    //        if (matches.Count == 0)
    //          cousubsNotMatched++;
    //        else
    //        {
    //          cInfo.Matched = true;
    //          if (matches.Count == 1)
    //          {
    //            cousubsMatched++;
    //            matches[0].CousubFips = cInfo.CousubFips;
    //          }
    //          else
    //          {
    //            cousubDuplicates++;
    //            cInfo.IsDuplicate = true;
    //          }
    //        }
    //      }
    //    }
    //  }
    //}

    //private static List<CousubInfo> GetCousubShapeData(string stateFips, string tigerFolderPath,
    //  // ReSharper disable once UnusedParameter.Local
    //  OptionGroup options)
    //{
    //  var shapeFile = new ShapeFile(Path.Combine(tigerFolderPath,
    //    @"tl_2016_us_cousub\tl_2016_us_cousub.shp"));
    //  var enumerator = shapeFile.GetShapeFileEnumerator();
    //  var cousubInfos = new List<CousubInfo>();
    //  while (enumerator.MoveNext())
    //  {
    //    var fieldValues = shapeFile.GetAttributeFieldValues(enumerator.CurrentShapeIndex);
    //    if (fieldValues[0].Trim() == stateFips)
    //      cousubInfos.Add(new CousubInfo
    //      {
    //        CountyFips = fieldValues[1].Trim(),
    //        CousubFips = fieldValues[2].Trim(),
    //        CousubName = fieldValues[5].Trim(),
    //        CousubLongName = fieldValues[6].Trim()
    //      });
    //  }
    //  return cousubInfos;
    //}

    private static List<DbPlaceInfo> GetCousubDatabaseData(DataTable tigerData,
      // ReSharper disable once UnusedParameter.Local
      OptionGroup options)
    {
      var result =
        tigerData.Rows.Cast<DataRow>()
          .Where(r => r.TigerType() == "C")
          .Select(r => new DbPlaceInfo(r))
          .ToList();

      ApplyCountyCounts(result);

      return result;
    }

    private static List<DbPlaceInfo> GetPlaceDatabaseData(DataTable tigerData,
      // ReSharper disable once UnusedParameter.Local
      OptionGroup options)
    {
      var result =
        tigerData.Rows.Cast<DataRow>()
          .Where(r => r.TigerType() == "P" &&
           (!options.ExcludeCdpPlaces || r.Lsad() != StatCodeCdp))
          .Select(r => new DbPlaceInfo(r))
          .ToList();

      ApplyCountyCounts(result);

      return result;
    }

    private static void ApplyCountyCounts(List<DbPlaceInfo> result)
    {
      var tigerCodeDictionary = result.GroupBy(i => i.TigerCode)
        .ToDictionary(g => g.Key, g => g.Count());

      foreach (var i in result)
        i.CountyCount = tigerCodeDictionary[i.TigerCode];
    }

    private static PointD[] ScalePolar(IReadOnlyList<PointD> data, double scale)
    {
      var scaled = new PointD[data.Count];
      for (var i = 0; i < data.Count; i++)
      {
        scaled[i].X = data[i].X * scale;
        scaled[i].Y = data[i].Y;
      }
      return scaled;
    }

    private static PointD[] ToPolar(IReadOnlyList<PointD> data, PointD centroid)
    {
      var polar = new PointD[data.Count];
      for (var i = 0; i < data.Count; i++)
      {
        var x = data[i].X - centroid.X;
        var y = data[i].Y - centroid.Y;
        polar[i] = new PointD(Math.Sqrt(x * x + y * y), Math.Atan2(y, x));
      }
      return polar;
    }

    private static PointD[] FromPolar(IReadOnlyList<PointD> data, PointD centroid)
    {
      var cartesian = new PointD[data.Count];
      for (var i = 0; i < data.Count; i++)
        cartesian[i] = new PointD(data[i].X * Math.Cos(data[i].Y) + centroid.X, 
          data[i].X * Math.Sin(data[i].Y) + centroid.Y);
      return cartesian;
    }

    private static PointD Centroid(IReadOnlyList<PointD> data)
    {
      var area = ShapeArea(data);
      var x = 0.0D;
      for (var i = 0; i < data.Count - 1; i++)
        x += (data[i].X + data[i + 1].X) * (data[i].X * data[i + 1].Y - data[i + 1].X * data[i].Y);
      x = x / (6.0D * area);
      var y = 0.0D;
      for (var i = 0; i < data.Count - 1; i++)
        y += (data[i].Y + data[i + 1].Y) * (data[i].X * data[i + 1].Y - data[i + 1].X * data[i].Y);
      y = y / (6.0D * area);
      return new PointD(x, y);
    }

    private static double ShapeArea(IReadOnlyList<PointD> data)
    {
      var area = 0.0D;
      for (var i = 0; i < data.Count - 1; i++)
        area += data[i].X * data[i + 1].Y - data[i + 1].X * data[i].Y;
      return area / 2.0D;
    }

    // ReSharper disable once UnusedMember.Local
    private static PointD[] ScaleShape(IReadOnlyList<PointD> data, double scale)
    {
      var centroid = Centroid(data);
      var polar = ToPolar(data, centroid);
      var scaled = ScalePolar(polar, scale);
      return FromPolar(scaled, centroid);
    }

    //private static List<PlaceInfo> GetPlaceShapeData(string stateFips, string tigerFolderPath,
    //  OptionGroup options)
    //{
    //  var countyShapeFile = new ShapeFile(Path.Combine(tigerFolderPath,
    //    @"tl_2016_us_county\tl_2016_us_county.shp"));
    //  var placeShapeFile = new ShapeFile(Path.Combine(tigerFolderPath,
    //    @"tl_2016_us_place\tl_2016_us_place.shp"));
    //  var placeEnumerator = placeShapeFile.GetShapeFileEnumerator();
    //  var placeInfos = new List<PlaceInfo>();
    //  while (placeEnumerator.MoveNext())
    //  {
    //    var placeFieldValues = placeShapeFile.GetAttributeFieldValues(placeEnumerator.CurrentShapeIndex);
    //    if (placeFieldValues[0].Trim() == stateFips)
    //      if (!options.ExcludeCdpPlaces || placeFieldValues[6] != StatCodeCdp)
    //      {
    //        var placeBounds = placeShapeFile.GetShapeBoundsD(placeEnumerator.CurrentShapeIndex);
    //        var placeData = placeShapeFile.GetShapeDataD(placeEnumerator.CurrentShapeIndex);
    //        var countyEnumerator = countyShapeFile.GetShapeFileEnumerator(placeBounds);
    //        var inCounties = new List<string>();
    //        while (countyEnumerator.MoveNext())
    //        {
    //          var countyFieldValues = countyShapeFile.GetAttributeFieldValues(countyEnumerator.CurrentShapeIndex);
    //          if (options.MatchCitiesToCounties && placeFieldValues[6] == StatCodeCity &&
    //            placeFieldValues[5].Trim() != countyFieldValues[5].Trim())
    //            continue;
    //          var inCounty = false;
    //          var countyData =
    //            countyShapeFile.GetShapeDataD(countyEnumerator.CurrentShapeIndex);
    //          foreach (var c in countyData)
    //            foreach (var p in placeData)
    //              if (GeometryAlgorithms.PolygonPolygonIntersect(c, c.Length,
    //                p, p.Length))
    //                inCounty = true;
    //          if (inCounty)
    //            inCounties.Add(countyFieldValues[1]); // fips
    //        }
    //        placeInfos.Add(new PlaceInfo
    //        {
    //          PlaceFips = placeFieldValues[1].Trim(),
    //          PlaceName = placeFieldValues[4].Trim(),
    //          PlaceLongName = placeFieldValues[5].Trim(),
    //          InTigerCounties = inCounties
    //        });
    //      }
    //  }
    //  return placeInfos;
    //}

    private void OptionsFileBrowseButton_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(OptionsFileTextBox.Text))
        try
        {
          OptionsOpenFileDialog.FileName = Path.GetFileName(OptionsFileTextBox.Text);
          OptionsOpenFileDialog.InitialDirectory =
            Path.GetDirectoryName(OptionsFileTextBox.Text);
        }
        catch
        {
          // ignored
        }
      if (OptionsOpenFileDialog.ShowDialog() == DialogResult.OK)
        OptionsFileTextBox.Text = OptionsOpenFileDialog.FileName;
    }

    private void TigerFolderBrowseButton_Click(object sender, EventArgs e)
    {
      TigerFolderBrowserDialog.SelectedPath = TigerFolderTextBox.Text;
      if (TigerFolderBrowserDialog.ShowDialog() == DialogResult.OK)
        TigerFolderTextBox.Text = TigerFolderBrowserDialog.SelectedPath;
    }

    private void OutputFileBrowseButton_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(OutputFileTextBox.Text))
        try
        {
          OutputSaveFileDialog.FileName = Path.GetFileName(OutputFileTextBox.Text);
          OutputSaveFileDialog.InitialDirectory =
            Path.GetDirectoryName(OutputFileTextBox.Text);
        }
        catch
        {
          // ignored
        }
      if (OutputSaveFileDialog.ShowDialog() == DialogResult.OK)
        OutputFileTextBox.Text = OutputSaveFileDialog.FileName;
    }

    private void StateDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      var stateCode = StateDropDown.SelectedItem as string;
      if (!string.IsNullOrWhiteSpace(stateCode))
      {
        var outputFile = OutputFileTextBox.Text;
        if (string.IsNullOrWhiteSpace(outputFile))
          OutputFileTextBox.Text =
            $"D:\\Users\\CurtNew\\Dropbox\\Documents\\Vote\\Tiger\\Reconcile Districts\\Analysis\\{stateCode}.csv";
        else
        {
          var path = Path.GetDirectoryName(outputFile);
          if (!string.IsNullOrWhiteSpace(path))
            OutputFileTextBox.Text = Path.Combine(path, $"{stateCode}.csv");
        }
      }
    }

    private void TigerFolderBrowserDialog_HelpRequest(object sender, EventArgs e)
    {

    }

    private void StartShapefilesButton_Click(object sender, EventArgs e)
    {
      try
      {
        var tigerFolderPath = TigerFolderTextBox.Text;
        if (string.IsNullOrWhiteSpace(tigerFolderPath))
          throw new Exception("Please select Tiger Data Folder");
        var cousubFilename = OutputCousubsTextBox.Text;
        if (string.IsNullOrWhiteSpace(cousubFilename))
          throw new Exception("Please select a cousubs output file");
        var placeFilename = OutputPlacesTextBox.Text;
        if (string.IsNullOrWhiteSpace(placeFilename))
          throw new Exception("Please select a places output file");

        //// process the cousub data
        //using (var cousubWriter = new StreamWriter(cousubFilename, false, System.Text.Encoding.UTF8))
        //{
        //  var csvWriter = new SimpleCsvWriter();
        //  csvWriter.AddField("State");
        //  csvWriter.AddField("STATEFP");
        //  csvWriter.AddField("COUNTYFP");
        //  csvWriter.AddField("COUSUBFP");
        //  csvWriter.AddField("NAME");
        //  csvWriter.AddField("NAMELSAD");
        //  csvWriter.AddField("LSAD");
        //  csvWriter.AddField("FUNCSTAT");
        //  csvWriter.Write(cousubWriter);
        //  var shapeFile = new ShapeFile(Path.Combine(tigerFolderPath,
        //    @"tl_2016_us_cousub\tl_2016_us_cousub.shp"));
        //  var enumerator = shapeFile.GetShapeFileEnumerator();
        //  while (enumerator.MoveNext())
        //  {
        //    var fieldValues = shapeFile.GetAttributeFieldValues(enumerator.CurrentShapeIndex);
        //    var statefp = fieldValues[0].Trim();
        //    var stateCode = StateCache.StateCodeFromLdsStateCode(statefp);
        //    csvWriter.AddField(stateCode);
        //    csvWriter.AddField(statefp);
        //    csvWriter.AddField(fieldValues[1].Trim());
        //    csvWriter.AddField(fieldValues[2].Trim());
        //    csvWriter.AddField(fieldValues[5].Trim());
        //    csvWriter.AddField(fieldValues[6].Trim());
        //    csvWriter.AddField(fieldValues[7].Trim());
        //    csvWriter.AddField(fieldValues[13].Trim());
        //    csvWriter.Write(cousubWriter);
        //  }
        //  cousubWriter.Close();
        //}

        //// process the place data
        //using (var placeWriter = new StreamWriter(placeFilename, false, System.Text.Encoding.UTF8))
        //{
        //  var csvWriter = new SimpleCsvWriter();
        //  csvWriter.AddField("State");
        //  csvWriter.AddField("STATEFP");
        //  csvWriter.AddField("PLACEFP");
        //  csvWriter.AddField("NAME");
        //  csvWriter.AddField("NAMELSAD");
        //  csvWriter.AddField("LSAD");
        //  csvWriter.AddField("FUNCSTAT");
        //  csvWriter.Write(placeWriter);
        //  var shapeFile = new ShapeFile(Path.Combine(tigerFolderPath,
        //    @"tl_2016_us_place\tl_2016_us_place.shp"));
        //  var enumerator = shapeFile.GetShapeFileEnumerator();
        //  while (enumerator.MoveNext())
        //  {
        //    var fieldValues = shapeFile.GetAttributeFieldValues(enumerator.CurrentShapeIndex);
        //    var statefp = fieldValues[0].Trim();
        //    var stateCode = StateCache.StateCodeFromLdsStateCode(statefp);
        //    csvWriter.AddField(stateCode);
        //    csvWriter.AddField(statefp);
        //    csvWriter.AddField(fieldValues[1].Trim());
        //    csvWriter.AddField(fieldValues[4].Trim());
        //    csvWriter.AddField(fieldValues[5].Trim());
        //    csvWriter.AddField(fieldValues[6].Trim());
        //    csvWriter.AddField(fieldValues[11].Trim());
        //    csvWriter.Write(placeWriter);
        //  }
        //  placeWriter.Close();
        //}

      //  // process the vtd10 data
      //  using (var placeWriter = new StreamWriter(@"D:\Users\CurtNew\Dropbox\Documents\Vote\Tiger\Reconcile Districts\Analysis\vtd10s.csv", false, System.Text.Encoding.UTF8))
      //  {
      //    var csvWriter = new SimpleCsvWriter();
      //    csvWriter.AddField("State");
      //    csvWriter.AddField("STATEFP");
      //    csvWriter.AddField("COUNTYFP");
      //    csvWriter.AddField("VTDST");
      //    csvWriter.AddField("VTDI");
      //    csvWriter.AddField("NAME");
      //    csvWriter.AddField("NAMELSAD");
      //    csvWriter.AddField("LSAD");
      //    csvWriter.AddField("FUNCSTAT");
      //    csvWriter.Write(placeWriter);
      //    var shapeFile = new ShapeFile(Path.Combine(tigerFolderPath,
      //      @"tl_2012_us_vtd10\tl_2012_us_vtd10.shp"));
      //    var enumerator = shapeFile.GetShapeFileEnumerator();
      //    while (enumerator.MoveNext())
      //    {
      //      var fieldValues = shapeFile.GetAttributeFieldValues(enumerator.CurrentShapeIndex);
      //      var statefp = fieldValues[0].Trim();
      //      var stateCode = StateCache.StateCodeFromLdsStateCode(statefp);
      //      csvWriter.AddField(stateCode);
      //      csvWriter.AddField(statefp);
      //      csvWriter.AddField(fieldValues[1].Trim());
      //      csvWriter.AddField(fieldValues[2].Trim());
      //      csvWriter.AddField(fieldValues[4].Trim());
      //      csvWriter.AddField(fieldValues[5].Trim());
      //      csvWriter.AddField(fieldValues[6].Trim());
      //      csvWriter.AddField(fieldValues[7].Trim());
      //      csvWriter.AddField(fieldValues[9].Trim());
      //      csvWriter.Write(placeWriter);
      //    }
      //    placeWriter.Close();
      //  }
      //}
      //catch (Exception ex)
      //{
      //  MessageBox.Show(ex.Message);
      //}

      using (var placeWriter = new StreamWriter(@"D:\Users\CurtNew\Dropbox\Documents\Vote\Tiger\Cicero\local-philadelphia.csv", false, System.Text.Encoding.UTF8))
      {
        var csvWriter = new SimpleCsvWriter();
        //csvWriter.AddField("State");
        //csvWriter.AddField("STATEFP");
        //csvWriter.AddField("COUNTYFP");
        //csvWriter.AddField("VTDST");
        //csvWriter.AddField("VTDI");
        //csvWriter.AddField("NAME");
        //csvWriter.AddField("NAMELSAD");
        //csvWriter.AddField("LSAD");
        //csvWriter.AddField("FUNCSTAT");
        //csvWriter.Write(placeWriter);
        var shapeFile = new ShapeFile(@"D:\Users\CurtNew\Dropbox\Documents\Vote\Tiger\Cicero\local-philadelphia-master\local-philadelphia-master\districts\district_philadelphia.shp");
        var enumerator = shapeFile.GetShapeFileEnumerator();
        while (enumerator.MoveNext())
        {
          var fieldValues = shapeFile.GetAttributeFieldValues(enumerator.CurrentShapeIndex);
          //var statefp = fieldValues[0].Trim();
          //var stateCode = StateCache.StateCodeFromLdsStateCode(statefp);
          //csvWriter.AddField(stateCode);
          //csvWriter.AddField(statefp);
          //csvWriter.AddField(fieldValues[1].Trim());
          //csvWriter.AddField(fieldValues[2].Trim());
          //csvWriter.AddField(fieldValues[4].Trim());
          //csvWriter.AddField(fieldValues[5].Trim());
          //csvWriter.AddField(fieldValues[6].Trim());
          //csvWriter.AddField(fieldValues[7].Trim());
          //csvWriter.AddField(fieldValues[9].Trim());
          foreach (var field in fieldValues)
            csvWriter.AddField(field.Trim());
          csvWriter.Write(placeWriter);
        }
        placeWriter.Close();
      }
    }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
}

private void OutputCousubsBrowseButton_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(OutputCousubsTextBox.Text))
        try
        {
          CousubSaveFileDialog.FileName = Path.GetFileName(OutputCousubsTextBox.Text);
          CousubSaveFileDialog.InitialDirectory =
            Path.GetDirectoryName(OutputCousubsTextBox.Text);
        }
        catch
        {
          // ignored
        }
      if (CousubSaveFileDialog.ShowDialog() == DialogResult.OK)
        OutputCousubsTextBox.Text = CousubSaveFileDialog.FileName;
    }

    private void OutputPlacesBrowseButton_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(OutputPlacesTextBox.Text))
        try
        {
          PlacesSaveFileDialog.FileName = Path.GetFileName(OutputPlacesTextBox.Text);
          PlacesSaveFileDialog.InitialDirectory =
            Path.GetDirectoryName(OutputPlacesTextBox.Text);
        }
        catch
        {
          // ignored
        }
      if (PlacesSaveFileDialog.ShowDialog() == DialogResult.OK)
        OutputPlacesTextBox.Text = PlacesSaveFileDialog.FileName;
    }
  }

  public static class Extensions
  {
    public static string LastElectionYear(this DataRow row)
    {
      return row["LastElectionYear"] as string;
    }
  }

  // ReSharper disable ClassNeverInstantiated.Global
  public class LocalId: IEquatable<LocalId>
  {
    public readonly string CountyCode;
    public readonly string LocalCode;

    // ReSharper disable once UnusedMember.Global
    public LocalId() { }

    public LocalId(string countyCode, string localCode)
    {
      CountyCode = countyCode;
      LocalCode = localCode;
    }

    public override int GetHashCode()
    {
      return CountyCode.GetHashCode() ^ LocalCode.GetHashCode();
    }

    public bool Equals(LocalId other)
    {
      return other != null && other.CountyCode == CountyCode && other.LocalCode == LocalCode;
    }

    public override bool Equals(object other)
    {
      return Equals (other as LocalId);
    }
  }

  public class NameToMatch
  {
    public string Fips;
    public string Name;
  }

  public class OptionGroup
  {
    // ReSharper disable UnassignedField.Global
    public bool ExcludeCdpPlaces;
    public bool MatchCitiesToCounties;
    public bool MatchToCityOf;
    public bool MatchToTownOf;
    public bool MatchToVillageOf;
    public NameToMatch[] NamesToMatch;
    public string[] IgnoreUnmatchedCousubs;
    public string[] IgnoreCousubs;
    public string[] IgnoreUnmatchedPlaces;
    public LocalId[] CountyWideLocals;
    public LocalId[] CountyWideLocalsTemp;
    // ReSharper restore UnassignedField.Global
  }
  // ReSharper restore ClassNeverInstantiated.Global

  public class VoteInfo
  {
    public string CountyCode;
    public string LocalCode;
    public string CountyName;
    public string LocalName;
    public string LastElectionYear;
    public string CousubFips;
    public string PlaceFips;
  }

  //public class CousubInfo
  //{
  //  public string CountyFips;
  //  public string CousubFips;
  //  public string CousubName;
  //  public string CousubLongName;
  //  public bool Matched;
  //  public bool IsDuplicate;
  //}

  //public class PlaceInfo
  //{
  //  public string PlaceFips;
  //  public string PlaceName;
  //  public string PlaceLongName;
  //  public readonly List<string> InVoteCounties = new List<string>();
  //  public List<string> InTigerCounties;
  //}

  public class DbPlaceInfo
  {
    public string StateCode;
    public string CountyCode;
    public string TigerType;
    public string TigerCode;
    public string Name;
    public string LongName;
    public string Lsad;
    public int CountyCount;
    public bool Matched;
    public bool IsDuplicate;

    public DbPlaceInfo(DataRow row)
    {
      StateCode = row.StateCode();
      CountyCode = row.CountyCode();
      TigerType = row.TigerType();
      TigerCode = row.TigerCode();
      Name = row.Name();
      LongName = row.LongName();
      Lsad = row.Lsad();
    }

    private static string Strip(string s)
    {
      return Regex.Replace(s, "[^a-z0-9]", "", RegexOptions.IgnoreCase).ToLowerInvariant();
    }

    public bool MatchesVoteName(string voteName, Dictionary<string, string> namesToMatch, OptionGroup options)
    {
      if (namesToMatch.ContainsKey(TigerCode))
         return voteName == namesToMatch[TigerCode];
      var matched = 
        voteName.Split(',')
          .Select(Strip)
          .Any(s => Name.Split(',').Union(LongName.Split(',')).Select(Strip).Any(i => i == s));
      if (!matched && options.MatchToCityOf && LongName.EndsWith(" city"))
        matched = Strip(voteName) == Strip("City of " + Name);
      if (!matched && options.MatchToTownOf && LongName.EndsWith(" town"))
        matched = Strip(voteName) == Strip("Town of " + Name);
      if (!matched && options.MatchToVillageOf && LongName.EndsWith(" village"))
        matched = Strip(voteName) == Strip("Village of " + Name);
      return matched;
    }
  }
}
