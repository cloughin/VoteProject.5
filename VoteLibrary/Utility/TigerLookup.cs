using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using DB.Vote;
using EGIS.ShapeFileLib;
using static System.String;

namespace Vote
{
  public static class TigerLookup
  {
    private static readonly ShapeFile CongressShapeFile;
    private static readonly ShapeFile CountyShapeFile;
    private static readonly ShapeFile DistrictShapeFile;
    private static readonly ShapeFile PlaceShapeFile;
    private static readonly ShapeFile UpperShapeFile;
    private static readonly ShapeFile LowerShapeFile;
    private static readonly ShapeFile ElementaryShapeFile;
    private static readonly ShapeFile SecondaryShapeFile;
    private static readonly ShapeFile UnifiedShapeFile;
    private static readonly ShapeFile CityCouncilShapeFile;
    private static readonly ShapeFile CountySupervisorsShapeFile;

    private const string CongressFilename = "tl_2017_us_cd115";
    private const string CountyFilename = "tl_2017_us_county";
    private const string DistrictFilename = "tl_2017_us_cousub";
    private const string PlaceFilename = "tl_2017_us_place";
    private const string UpperFilename = "tl_2017_us_sldu";
    private const string LowerFilename = "tl_2017_us_sldl";
    private const string ElementaryFilename = "tl_2017_us_elsd";
    private const string SecondaryFilename = "tl_2017_us_scsd";
    private const string UnifiedFilename = "tl_2017_us_unsd";
    private const string CityCouncilFilename = "city_council_us";
    private const string CountySupervisorFilename = "county_supervisors_us";

    private static Dictionary<string, string> TranslationDictionary;
    private const string TableTypeCongressionalDistrict = "CD";
    private const string TableTypeStateSenate = "SS";
    private const string TableTypeStateHouse = "SH";


    static TigerLookup()
    {
      // load all the shapefiles
      var shapeFileDirectoryName = WebConfigurationManager.AppSettings["ShapeFileDirectory"];
      if (IsNullOrWhiteSpace(shapeFileDirectoryName)) return;

      if (!Directory.Exists(shapeFileDirectoryName)) return;
      var shapeFileDirectory = new DirectoryInfo(shapeFileDirectoryName);
      foreach (var directory in shapeFileDirectory.EnumerateDirectories())
        switch (directory.Name)
        {
          case CongressFilename:
            CongressShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{CongressFilename}.shp"));
            break;

          case CountyFilename:
            CountyShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{CountyFilename}.shp"));
            break;

          case DistrictFilename:
            DistrictShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{DistrictFilename}.shp"));
            break;

          case PlaceFilename:
            PlaceShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{PlaceFilename}.shp"));
            break;

          case UpperFilename:
            UpperShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{UpperFilename}.shp"));
            break;

          case LowerFilename:
            LowerShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{LowerFilename}.shp"));
            break;

          case ElementaryFilename:
            ElementaryShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{ElementaryFilename}.shp"));
            break;

          case SecondaryFilename:
            SecondaryShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{SecondaryFilename}.shp"));
            break;

          case UnifiedFilename:
            UnifiedShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{UnifiedFilename}.shp"));
            break;

          case CityCouncilFilename:
            CityCouncilShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{CityCouncilFilename}.shp"));
            break;

          case CountySupervisorFilename:
            CountySupervisorsShapeFile = new ShapeFile(Path.Combine(directory.FullName,
             $"{CountySupervisorFilename}.shp"));
            break;
        }
    }

    private static ShapeFile GetShapeFile(string filename)
    {
      var shapefileName =
        Path.Combine(WebConfigurationManager.AppSettings["ShapeFileDirectory"], filename,
          $"{filename}.shp");
      if (!File.Exists(shapefileName)) return null;
      return new ShapeFile(shapefileName);
    }

    public static ShapeFile ShapeFileCongress => GetShapeFile(CongressFilename);
    public static ShapeFile ShapeFileCounty => GetShapeFile(CountyFilename);
    public static ShapeFile ShapeFileDistrict => GetShapeFile(DistrictFilename);
    public static ShapeFile ShapeFilePlace => GetShapeFile(PlaceFilename);
    public static ShapeFile ShapeFileUpper => GetShapeFile(UpperFilename);
    public static ShapeFile ShapeFileLower => GetShapeFile(LowerFilename);
    public static ShapeFile ShapeFileElementary => GetShapeFile(ElementaryFilename);
    public static ShapeFile ShapeFileSecondary => GetShapeFile(SecondaryFilename);
    public static ShapeFile ShapeFileUnified => GetShapeFile(UnifiedFilename);
    public static ShapeFile ShapeFileCityCouncil => GetShapeFile(CityCouncilFilename);
    public static ShapeFile ShapeFileCountySupervisor => GetShapeFile(CountySupervisorFilename);

    private static string Translate(string tableType, string stateCode, string tigerCode)
    {
      if (TranslationDictionary == null)
      {
        var translationDictionary = TigerToVoteCodes.GetAllData()
          .ToDictionary(r => r.TableType + r.StateCode + r.TigerCode, r => r.VoteCode,
            StringComparer.OrdinalIgnoreCase);
        TranslationDictionary = translationDictionary;
      }

      return TranslationDictionary.TryGetValue(tableType + stateCode + tigerCode, out var voteCode)
        ? voteCode
        : tigerCode;
    }

    public static TigerLookupAllData LookupAll(double latitude, double longitude)
    {
      var result = new TigerLookupAllData();
      lock (ShapeFile.Sync)
      {
        var countyIndex = CountyShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (countyIndex < 0) return result;
        var countyValues = CountyShapeFile.GetAttributeFieldValues(countyIndex);
        result.Fips = countyValues[0].Trim();
        result.StateCode = StateCache.StateCodeFromLdsStateCode(result.Fips);
        result.County = countyValues[1].Trim();

        var congressIndex =
          CongressShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (congressIndex >= 0)
        {
          var congressValues = CongressShapeFile.GetAttributeFieldValues(congressIndex);
          result.Congress = Translate(TableTypeCongressionalDistrict, result.StateCode,
            congressValues[1].Trim());
        }

        var upperIndex = UpperShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (upperIndex >= 0)
        {
          var upperValues = UpperShapeFile.GetAttributeFieldValues(upperIndex);
          result.Upper = Translate(TableTypeStateSenate, result.StateCode,
            upperValues[1].Trim());
        }

        var lowerIndex = LowerShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (lowerIndex >= 0)
        {
          var lowerValues = LowerShapeFile.GetAttributeFieldValues(lowerIndex);
          result.Lower = Translate(TableTypeStateHouse, result.StateCode,
            lowerValues[1].Trim());
        }

        var districtIndex =
          DistrictShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (districtIndex >= 0)
        {
          var districtValues = DistrictShapeFile.GetAttributeFieldValues(districtIndex);
          result.District = districtValues[2].Trim();
          result.DistrictName = districtValues[5].Trim();
          result.DistrictLongName = districtValues[6].Trim();
        }

        var placeIndex =
          PlaceShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (placeIndex >= 0)
        {
          var placeValues = PlaceShapeFile.GetAttributeFieldValues(placeIndex);
          result.Place = placeValues[1].Trim();
          result.PlaceName = placeValues[4].Trim();
          result.PlaceLongName = placeValues[5].Trim();
        }

        var elementaryIndex =
          ElementaryShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (elementaryIndex >= 0)
        {
          var elementaryValues = ElementaryShapeFile.GetAttributeFieldValues(elementaryIndex);
          result.Elementary = elementaryValues[1].Trim();
          result.ElementaryName = elementaryValues[3].Trim();
        }

        var secondaryIndex =
          SecondaryShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (secondaryIndex >= 0)
        {
          var secondaryValues = SecondaryShapeFile.GetAttributeFieldValues(secondaryIndex);
          result.Secondary = secondaryValues[1].Trim();
          result.SecondaryName = secondaryValues[3].Trim();
        }

        var unifiedIndex = UnifiedShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (unifiedIndex >= 0)
        {
          var unifiedValues = UnifiedShapeFile.GetAttributeFieldValues(unifiedIndex);
          result.Unified = unifiedValues[1].Trim();
          result.UnifiedName = unifiedValues[3].Trim();
        }

        var cityCouncilIndex = CityCouncilShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (cityCouncilIndex >= 0)
        {
          var cityCouncilValues = CityCouncilShapeFile.GetAttributeFieldValues(cityCouncilIndex);
          result.CityCouncil = cityCouncilValues[1].Trim();
          result.CityCouncilDistrict = cityCouncilValues[3].Trim();
        }

        var countySupervisorsIndex = CountySupervisorsShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (countySupervisorsIndex >= 0)
        {
          var countySupervisorsValues = CountySupervisorsShapeFile.GetAttributeFieldValues(countySupervisorsIndex);
          result.CountySupervisors = countySupervisorsValues[2].Trim();
          result.CountySupervisorsDistrict = countySupervisorsValues[3].Trim();
        }
      }
      return result;
    }

    public static TigerLookupData Lookup(double latitude, double longitude)
    {
      var result = new TigerLookupData();
      lock (ShapeFile.Sync)
      {
        var countyIndex = CountyShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude),
          0.0);
        if (countyIndex < 0) return result;
        var countyValues = CountyShapeFile.GetAttributeFieldValues(countyIndex);
        result.StateCode = StateCache.StateCodeFromLdsStateCode(countyValues[0].Trim());
        result.County = countyValues[1].Trim();

        var congressIndex =
          CongressShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (congressIndex >= 0)
        {
          var congressValues = CongressShapeFile.GetAttributeFieldValues(congressIndex);
          result.Congress = Translate(TableTypeCongressionalDistrict, result.StateCode,
            congressValues[1].Trim());
        }

        var upperIndex = UpperShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (upperIndex >= 0)
        {
          var upperValues = UpperShapeFile.GetAttributeFieldValues(upperIndex);
          result.Upper = Translate(TableTypeStateSenate, result.StateCode,
            upperValues[1].Trim());
        }

        var lowerIndex = LowerShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (lowerIndex >= 0)
        {
          var lowerValues = LowerShapeFile.GetAttributeFieldValues(lowerIndex);
          result.Lower = Translate(TableTypeStateHouse, result.StateCode,
            lowerValues[1].Trim());
        }

        var districtIndex =
          DistrictShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (districtIndex >= 0)
        {
          var districtValues = DistrictShapeFile.GetAttributeFieldValues(districtIndex);
          result.District = districtValues[2].Trim();
        }

        var placeIndex =
          PlaceShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (placeIndex >= 0)
        {
          var placeValues = PlaceShapeFile.GetAttributeFieldValues(placeIndex);
          result.Place = placeValues[1].Trim();
        }

        var elementaryIndex =
          ElementaryShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (elementaryIndex >= 0)
        {
          var elementaryValues = ElementaryShapeFile.GetAttributeFieldValues(elementaryIndex);
          result.Elementary = elementaryValues[1].Trim();
        }

        var secondaryIndex =
          SecondaryShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (secondaryIndex >= 0)
        {
          var secondaryValues = SecondaryShapeFile.GetAttributeFieldValues(secondaryIndex);
          result.Secondary = secondaryValues[1].Trim();
        }

        var unifiedIndex = UnifiedShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (unifiedIndex >= 0)
        {
          var unifiedValues = UnifiedShapeFile.GetAttributeFieldValues(unifiedIndex);
          result.Unified = unifiedValues[1].Trim();
        }

        var cityCouncilIndex = CityCouncilShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (cityCouncilIndex >= 0)
        {
          var cityCouncilValues = CityCouncilShapeFile.GetAttributeFieldValues(cityCouncilIndex);
          result.CityCouncil = cityCouncilValues[1].Trim();
        }

        var countySupervisorsIndex = CountySupervisorsShapeFile.GetShapeIndexContainingPoint(new PointD(longitude, latitude), 0.0);
        if (countySupervisorsIndex >= 0)
        {
          var countySupervisorsValues = CountySupervisorsShapeFile.GetAttributeFieldValues(countySupervisorsIndex);
          result.CountySupervisors = countySupervisorsValues[2].Trim();
        }
      }
      return result;
    }
  }

  public class TigerLookupData
  {
    // ReSharper disable NotAccessedField.Global
    public string StateCode = Empty;
    public string County = Empty;
    public string Congress = Empty;
    public string Upper = Empty;
    public string Lower = Empty;
    public string District = Empty;
    public string Place = Empty;
    public string Elementary = Empty;
    public string Secondary = Empty;
    public string Unified = Empty;
    public string CityCouncil = Empty;
    public string CountySupervisors = Empty;
    public string SchoolDistrictDistrict = Empty;
    // ReSharper restore NotAccessedField.Global
  }

  public class TigerLookupAllData
  {
    // ReSharper disable NotAccessedField.Global
    public string Fips = Empty;
    public string StateCode = Empty;
    public string County = Empty;
    public string Congress = Empty;
    public string Upper = Empty;
    public string Lower = Empty;
    public string District = Empty;
    public string DistrictName = Empty;
    public string DistrictLongName = Empty;
    public string Place = Empty;
    public string PlaceName = Empty;
    public string PlaceLongName = Empty;
    public string Elementary = Empty;
    public string ElementaryName = Empty;
    public string Secondary = Empty;
    public string SecondaryName = Empty;
    public string Unified = Empty;
    public string UnifiedName = Empty;
    public string CityCouncil = Empty;
    public string CityCouncilDistrict = Empty;
    public string CountySupervisors = Empty;
    public string CountySupervisorsDistrict = Empty;
    // ReSharper restore NotAccessedField.Global
  }
}
