using System;
using System.Collections.Generic;
using System.Linq;
using DB.Vote;
using EGIS.ShapeFileLib;
using Vote;

namespace VoteAdmin
{
  public partial class AuditShapeFilesPage : SecurePage, ISuperUser
  {
    #region Private

    private void AuditSchoolDistricts(ShapeFile shapefile, string tigerType, string description)
    {
      if (shapefile == null)
        throw new Exception("Could not find the shapefile");
      var enumerator = shapefile.GetShapeFileEnumerator();
      var list = new List<string[]>();
      while (enumerator.MoveNext())
        list.Add(shapefile.GetAttributeFieldValues(enumerator.CurrentShapeIndex));
      // reproject into nicely-named structure and eliminate "F" (ficticious entry)
      var shapefileData = list
        .Select(i => new
        {
          StateCode = StateCache.StateCodeFromLdsStateCode(i[0].Trim()),
          Id = i[1].Trim(),
          Name = i[3].Trim(),
          FuncStat = i[9].Trim()
        }).Where(i => i.FuncStat != "F").ToList();

      var tigerSchoolsData = TigerSchools.GetDataByTigerType(tigerType);
      var tigerPlacesCountiesData = TigerPlacesCounties.GetDataByTigerType(tigerType);

      // District is in the shapefile, but is not in TigerSchools.
      var notInDatabase = shapefileData
        .Where(s => !tigerSchoolsData.Any(d => d.StateCode == s.StateCode &&
          d.TigerCode == s.Id)).ToList();

      // District is in TigerSchools, but not in the shapefile.
      var notInShapefile = tigerSchoolsData
        .Where(d => !shapefileData.Any(s => s.StateCode == d.StateCode && 
          s.Id == d.TigerCode)).ToList();

      // Check TigerSchools in TigerPlacesCounties
      var notInTigerPlacesCounties = tigerSchoolsData.Where(s =>
        !tigerPlacesCountiesData.Any(c =>
          c.StateCode == s.StateCode && c.TigerCode == s.TigerCode)).ToList();

      // Check TigerPlacesCounties in TigerSchools
      var notInTigerSchools = tigerPlacesCountiesData.Where(c =>
        !tigerSchoolsData.Any(s =>
          s.StateCode == c.StateCode && s.TigerCode == c.TigerCode)).ToList();

      var results = new HtmlDiv().AddTo(ResultsPlaceHolder, "results");
      new HtmlP { InnerText = $"Audit of {description} School District ShapeFile" }.AddTo(results, "head");
      new HtmlP { InnerText = $"There are {shapefileData.Count} entries in the shapefile." }
        .AddTo(results);
      new HtmlP
      {
        InnerText =
          $"There are {tigerSchoolsData.Count} district entries and " + 
          $"{tigerPlacesCountiesData.Count} county entries in the database."
      }.AddTo(results);

      new HtmlP
      {
        InnerText =
          $"{notInDatabase.Count} entries are in the shapefile, but not in the database."
      }.AddTo(results);
      foreach (var i in notInDatabase)
        new HtmlP { InnerText = $"{i.StateCode}:{i.Id} {i.Name}" }
          .AddTo(results, "error");

      new HtmlP
      {
        InnerText =
          $"{notInShapefile.Count} entries are in the database, but not in the shapefile."
      }.AddTo(results);
      foreach (var i in notInShapefile)
        new HtmlP { InnerText = $"{i.StateCode}:{i.TigerCode} {i.Name}" }
          .AddTo(results, "error");

      new HtmlP
      {
        InnerText =
          $"{notInTigerPlacesCounties.Count} entries are in the district table, but not in the county table."
      }.AddTo(results);
      foreach (var i in notInTigerPlacesCounties)
        new HtmlP { InnerText = $"{i.StateCode}:{i.TigerCode} {i.Name}" }
          .AddTo(results, "error");

      new HtmlP
      {
        InnerText =
          $"{notInTigerSchools.Count} entries are in the county table, but not in the district table."
      }.AddTo(results);
      foreach (var i in notInTigerSchools)
        new HtmlP { InnerText = $"{i.StateCode}:{i.TigerCode} {i.CountyCode}" }
          .AddTo(results, "error");
    }

    #endregion Private

    #region Event handlers and overrides

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        Page.Title = "Audit ShapeFiles";
        H1.InnerHtml = "Audit ShapeFiles";
      }
    }

    protected void ButtonAuditCityCouncil_Click(object sender, EventArgs e)
    {
      try
      {
        var shapefile = TigerLookup.ShapeFileCityCouncil;
        if (shapefile == null)
          throw new Exception("Could not find the shapefile");
        var enumerator = shapefile.GetShapeFileEnumerator();
        var list = new List<string[]>();
        while (enumerator.MoveNext())
          list.Add(shapefile.GetAttributeFieldValues(enumerator.CurrentShapeIndex));
        // reproject into nicely-named structure
        var shapefileData = list.Select(i => new
        {
          StateCode = i[0].Trim(),
          Id = i[1].Trim(),
          City = i[2].Trim(),
          District = i[3].Trim()
        }).ToList();

        var databaseData = CityCouncil.GetAllData();

        // District is in the shapefile, but is not in the database.
        var notInDatabase = shapefileData
          .Where(s => !databaseData.Any(d => d.StateCode == s.StateCode &&
            d.CityCouncilCode == s.Id)).ToList();

        // District is in the shapefile, but is marked "not in shapefile" in the database.
        var markedNotInShapefile = shapefileData
          .Where(s => databaseData.Any(d => d.StateCode == s.StateCode &&
            d.CityCouncilCode == s.Id && !d.IsInShapefile)).ToList();

        // District is marked "in shapefile" in the database, but is not in the shapefile.
        var notInShapefile = databaseData
          .Where(d => d.IsInShapefile &&
            !shapefileData.Any(s => s.StateCode == d.StateCode &&
              s.Id == d.CityCouncilCode)).ToList();

        var results = new HtmlDiv().AddTo(ResultsPlaceHolder, "results");
        new HtmlP {InnerText = "Audit of City Council ShapeFile"}.AddTo(results, "head");
        new HtmlP {InnerText = $"There are {shapefileData.Count} entries in the shapefile."}
          .AddTo(results);
        new HtmlP
        {
          InnerText =
            $"There are {databaseData.Count} entries in the database ({databaseData.Count(d => !d.IsInShapefile)} are marked \"not in shapefile\")."
        }.AddTo(results);

        new HtmlP
        {
          InnerText =
            $"{notInDatabase.Count} entries are in the shapefile, but not in the database."
        }.AddTo(results);
        foreach (var i in notInDatabase)
          new HtmlP {InnerText = $"{i.StateCode}:{i.Id} {i.City} {i.District}"}
            .AddTo(results, "error");

        new HtmlP
        {
          InnerText =
            $"{markedNotInShapefile.Count} entries are in the shapefile, but are marked \"not in shapefile\" in the database."
        }.AddTo(results);
        foreach (var i in markedNotInShapefile)
          new HtmlP {InnerText = $"{i.StateCode}:{i.Id} {i.City} {i.District}"}
            .AddTo(results, "error");

        new HtmlP
        {
          InnerText =
            $"{notInShapefile.Count} entries are marked \"in shapefile\" in the database, but are not in the shapefile."
        }.AddTo(results);
        foreach (var i in notInShapefile)
          new HtmlP {InnerText = $"{i.StateCode}:{i.CityCouncilCode} {i.Name}"}
            .AddTo(results, "error");
      }
      catch (Exception ex)
      {
        AuditShapeFileFeedback.HandleException(ex);
      }
    }

    protected void ButtonAuditCountySupervisors_Click(object sender, EventArgs e)
    {
      try
      {
        var shapefile = TigerLookup.ShapeFileCountySupervisor;
        if (shapefile == null)
          throw new Exception("Could not find the shapefile");
        var enumerator = shapefile.GetShapeFileEnumerator();
        var list = new List<string[]>();
        while (enumerator.MoveNext())
          list.Add(shapefile.GetAttributeFieldValues(enumerator.CurrentShapeIndex));
        // reproject into nicely-named structure
        var shapefileData = list.Select(i => new
        {
          StateCode = i[0].Trim(),
          CountyCode = i[1].Trim(),
          Id = i[2].Trim(),
          District = i[3].Trim()
        }).ToList();

        var databaseData = CountySupervisors.GetAllData();

        // District is in the shapefile, but is not in the database.
        var notInDatabase = shapefileData
          .Where(s => !databaseData.Any(d => d.StateCode == s.StateCode &&
            d.CountySupervisorsCode == s.Id)).ToList();

        // District is in the shapefile, but is marked "not in shapefile" in the database.
        var markedNotInShapefile = shapefileData
          .Where(s => databaseData.Any(d => d.StateCode == s.StateCode &&
            d.CountySupervisorsCode == s.Id && !d.IsInShapefile)).ToList();

        // District is marked "in shapefile" in the database, but is not in the shapefile.
        var notInShapefile = databaseData
          .Where(d => d.IsInShapefile &&
            !shapefileData.Any(s => s.StateCode == d.StateCode &&
              s.Id == d.CountySupervisorsCode)).ToList();

        // Shapefile CountyCode does not match 1st three character of Id, except for DC.
        var countyIdMismatch = shapefileData
          .Where(s => s.StateCode != "DC" && !s.Id.StartsWith(s.CountyCode)).ToList();

        // Database CountyCode does not match 1st three character of CountySupervisorsCode, except for DC.
        var countySupervisorsCodeMismatch = databaseData
          .Where(d => d.StateCode != "DC" &&
            !d.CountySupervisorsCode.StartsWith(d.CountyCode)).ToList();

        // Shapefile CountyCode does not match database CountyCode.
        var countyDatabaseMismatch = shapefileData
          .Where(s => databaseData.Any(d => d.StateCode == s.StateCode &&
            d.CountySupervisorsCode == s.Id && d.CountyCode != s.CountyCode)).ToList();

        var results = new HtmlDiv().AddTo(ResultsPlaceHolder, "results");
        new HtmlP { InnerText = "Audit of County Supervisors ShapeFile" }.AddTo(results, "head");
        new HtmlP { InnerText = $"There are {shapefileData.Count} entries in the shapefile." }
          .AddTo(results);
        new HtmlP
        {
          InnerText =
            $"There are {databaseData.Count} entries in the database ({databaseData.Count(d => !d.IsInShapefile)} are marked \"not in shapefile\")."
        }.AddTo(results);

        new HtmlP
        {
          InnerText =
            $"{notInDatabase.Count} entries are in the shapefile, but not in the database."
        }.AddTo(results);
        foreach (var i in notInDatabase)
          new HtmlP { InnerText = $"{i.StateCode}:{i.CountyCode}:{i.Id} {i.District}" }
            .AddTo(results, "error");

        new HtmlP
        {
          InnerText =
            $"{markedNotInShapefile.Count} entries are in the shapefile, but are marked \"not in shapefile\" in the database."
        }.AddTo(results);
        foreach (var i in markedNotInShapefile)
          new HtmlP { InnerText = $"{i.StateCode}:{i.CountyCode}:{i.Id} {i.District}" }
            .AddTo(results, "error");

        new HtmlP
        {
          InnerText =
            $"{notInShapefile.Count} entries are marked \"in shapefile\" in the database, but are not in the shapefile."
        }.AddTo(results);
        foreach (var i in notInShapefile)
          new HtmlP { InnerText = $"{i.StateCode}:{i.CountyCode}:{i.CountySupervisorsCode} {i.Name}" }
            .AddTo(results, "error");

        new HtmlP
        {
          InnerText =
            $"In {countyIdMismatch.Count} shapefile entries the CountyCode did not match the first three characters of the Id."
        }.AddTo(results);
        foreach (var i in countyIdMismatch)
          new HtmlP { InnerText = $"{i.StateCode}:{i.CountyCode}:{i.Id} {i.District}" }
            .AddTo(results, "error");

        new HtmlP
        {
          InnerText =
            $"In {countySupervisorsCodeMismatch.Count} database entries the CountyCode did not match the first three characters of the Id."
        }.AddTo(results);
        foreach (var i in countySupervisorsCodeMismatch)
          new HtmlP { InnerText = $"{i.StateCode}:{i.CountyCode}:{i.CountySupervisorsCode} {i.Name}" }
            .AddTo(results, "error");

        new HtmlP
        {
          InnerText =
            $"In {countyDatabaseMismatch.Count} database entries the CountyCode did not match the shapefile CountyCode."
        }.AddTo(results);
        foreach (var i in countyDatabaseMismatch)
          new HtmlP { InnerText = $"{i.StateCode}:{i.CountyCode}:{i.Id} {i.District}" }
            .AddTo(results, "error");
      }
      catch (Exception ex)
      {
        AuditShapeFileFeedback.HandleException(ex);
      }
    }

    protected void ButtonAuditSchoolSubDistricts_Click(object sender, EventArgs e)
    {
      try
      {
        // this shapefile does not actually exist yet
        //var shapefile = TigerLookup.ShapeFileSchoolSubDistricts;
        //if (shapefile == null)
        //  throw new Exception("Could not find the shapefile");
        //var enumerator = shapefile.GetShapeFileEnumerator();
        // ReSharper disable once CollectionNeverUpdated.Local
        var list = new List<string[]>();
        //while (enumerator.MoveNext())
        //  list.Add(shapefile.GetAttributeFieldValues(enumerator.CurrentShapeIndex));
        // reproject into nicely-named structure
        var shapefileData = list.Select(i => new
        {
          StateCode = i[0].Trim(),
          Id = i[1].Trim(),
          District = i[2].Trim(),
          SubDistrict = i[3].Trim()
        }).ToList();

        var databaseData = SchoolDistrictDistricts.GetAllData();

        // District is in the shapefile, but is not in the database.
        var notInDatabase = shapefileData
          .Where(s => !databaseData.Any(d => d.StateCode == s.StateCode &&
            d.SchoolDistrictDistrictCode == s.Id)).ToList();

        // District is in the shapefile, but is marked "not in shapefile" in the database.
        var markedNotInShapefile = shapefileData
          .Where(s => databaseData.Any(d => d.StateCode == s.StateCode &&
            d.SchoolDistrictDistrictCode == s.Id && !d.IsInShapefile)).ToList();

        // District is marked "in shapefile" in the database, but is not in the shapefile.
        var notInShapefile = databaseData
          .Where(d => d.IsInShapefile &&
            !shapefileData.Any(s => s.StateCode == d.StateCode &&
              s.Id == d.SchoolDistrictDistrictCode)).ToList();

        var results = new HtmlDiv().AddTo(ResultsPlaceHolder, "results");
        new HtmlP { InnerText = "Audit of School Sub-Districts ShapeFile" }.AddTo(results, "head");
        new HtmlP { InnerText = $"There are {shapefileData.Count} entries in the shapefile." }
          .AddTo(results);
        new HtmlP
        {
          InnerText =
            $"There are {databaseData.Count} entries in the database ({databaseData.Count(d => !d.IsInShapefile)} are marked \"not in shapefile\")."
        }.AddTo(results);

        new HtmlP
        {
          InnerText =
            $"{notInDatabase.Count} entries are in the shapefile, but not in the database."
        }.AddTo(results);
        foreach (var i in notInDatabase)
          new HtmlP { InnerText = $"{i.StateCode}:{i.Id} {i.District} {i.SubDistrict}" }
            .AddTo(results, "error");

        new HtmlP
        {
          InnerText =
            $"{markedNotInShapefile.Count} entries are in the shapefile, but are marked \"not in shapefile\" in the database."
        }.AddTo(results);
        foreach (var i in markedNotInShapefile)
          new HtmlP { InnerText = $"{i.StateCode}:{i.Id} {i.District} {i.SubDistrict}" }
            .AddTo(results, "error");

        new HtmlP
        {
          InnerText =
            $"{notInShapefile.Count} entries are marked \"in shapefile\" in the database, but are not in the shapefile."
        }.AddTo(results);
        foreach (var i in notInShapefile)
          new HtmlP { InnerText = $"{i.StateCode}:{i.SchoolDistrictDistrictCode} {i.Name}" }
            .AddTo(results, "error");
      }
      catch (Exception ex)
      {
        AuditShapeFileFeedback.HandleException(ex);
      }
    }

    protected void ButtonAuditElementarySchoolDistricts_Click(object sender, EventArgs e)
    {
      AuditSchoolDistricts(TigerLookup.ShapeFileElementary, 
        TigerPlacesCounties.TigerTypeElementary, "Elementary");
    }

    protected void ButtonAuditSecondarySchoolDistricts_Click(object sender, EventArgs e)
    {
      AuditSchoolDistricts(TigerLookup.ShapeFileSecondary,
        TigerPlacesCounties.TigerTypeSecondary, "Secondary");
    }

    protected void ButtonAuditUnifiedSchoolDistricts_Click(object sender, EventArgs e)
    {
      AuditSchoolDistricts(TigerLookup.ShapeFileUnified,
        TigerPlacesCounties.TigerTypeUnified, "Unified");
    }

    #endregion Event handlers and overrides
  }
}