using System.Collections.Generic;
using System.IO;
using System.Linq;
using EGIS.ShapeFileLib;
using Vote;

namespace EnumerateTigerDistricts
{
  public class DistrictInfo
  {
    public string StateFips;
    public string CountyFips;
    public string DistrictFips;
    public string DistrictName;
    public string DistrictLongName;
    public string StateCode;
    public string StateName;
    public string CountyName;
  }

  static class Program
  {
    private static void Main()
    {
      const string dir = @"D:\Users\CurtNew\Dropbox\Documents\vote\tiger";
      var shapeFile = new ShapeFile(Path.Combine(dir, @"2016\tl_2016_us_cousub\tl_2016_us_cousub.shp"));
      var enumerator = shapeFile.GetShapeFileEnumerator();
      var districtInfos = new List<DistrictInfo>();
      while (enumerator.MoveNext())
      {
        var fieldValues = shapeFile.GetAttributeFieldValues(enumerator.CurrentShapeIndex);
        var stateFips = fieldValues[0].Trim();
        var stateCode = StateCache.StateCodeFromLdsStateCode(stateFips);
        var countyFips = fieldValues[1].Trim();
        districtInfos.Add(new DistrictInfo
        {
          StateFips = stateFips,
          CountyFips = countyFips,
          DistrictFips = fieldValues[2].Trim(),
          DistrictName = fieldValues[5].Trim(),
          DistrictLongName = fieldValues[6].Trim(),
          StateCode = stateCode,
          StateName = StateCache.GetStateName(stateCode),
          CountyName = CountyCache.GetCountyName(stateCode, countyFips)
        });
      }
      var textWriter = new StreamWriter(Path.Combine(dir, @"districts.csv"));
      var csvWriter = new SimpleCsvWriter();
      csvWriter.AddField("State FIPS");
      csvWriter.AddField("County FIPS");
      csvWriter.AddField("District FIPS");
      csvWriter.AddField("State Code");
      csvWriter.AddField("State Name");
      csvWriter.AddField("County Name");
      csvWriter.AddField("District Name");
      csvWriter.AddField("District Long Name");
      csvWriter.Write(textWriter);
      foreach (
        var district in
        districtInfos.OrderBy(d => d.StateName)
          .ThenBy(d => d.CountyName)
          .ThenBy(d => d.DistrictLongName))
      {
        csvWriter.AddField(district.StateFips);
        csvWriter.AddField(district.CountyFips);
        csvWriter.AddField(district.DistrictFips);
        csvWriter.AddField(district.StateCode);
        csvWriter.AddField(district.StateName);
        csvWriter.AddField(district.CountyName);
        csvWriter.AddField(district.DistrictName);
        csvWriter.AddField(district.DistrictLongName);
        csvWriter.Write(textWriter);
      }
      textWriter.Close();
    }
  }
}
