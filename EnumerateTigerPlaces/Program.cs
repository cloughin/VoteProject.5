using System.Collections.Generic;
using System.IO;
using System.Linq;
using EGIS.ShapeFileLib;
using Vote;

namespace EnumerateTigerPlaces
{
  public class PlaceInfo
  {
    public string StateFips;
    public string PlaceFips;
    public string PlaceName;
    public string PlaceLongName;
    public string StateCode;
    public string StateName;
  }

  static class Program
  {
    private static void Main()
    {
      const string dir = @"D:\Users\CurtNew\Dropbox\Documents\vote\tiger";
      var shapeFile = new ShapeFile(Path.Combine(dir, @"2016\tl_2016_us_place\tl_2016_us_place.shp"));
      var enumerator = shapeFile.GetShapeFileEnumerator();
      var placeInfos = new List<PlaceInfo>();
      while (enumerator.MoveNext())
      {
        var fieldValues = shapeFile.GetAttributeFieldValues(enumerator.CurrentShapeIndex);
        var stateFips = fieldValues[0].Trim();
        var stateCode = StateCache.StateCodeFromLdsStateCode(stateFips);
        placeInfos.Add(new PlaceInfo
        {
          StateFips = stateFips,
          PlaceFips = fieldValues[1].Trim(),
          PlaceName = fieldValues[4].Trim(),
          PlaceLongName = fieldValues[5].Trim(),
          StateCode = stateCode,
          StateName = StateCache.GetStateName(stateCode)
        });
      }
      var textWriter = new StreamWriter(Path.Combine(dir, @"places.csv"));
      var csvWriter = new SimpleCsvWriter();
      csvWriter.AddField("State FIPS");
      csvWriter.AddField("Place FIPS");
      csvWriter.AddField("State Code");
      csvWriter.AddField("State Name");
      csvWriter.AddField("Place Name");
      csvWriter.AddField("Place Long Name");
      csvWriter.Write(textWriter);
      foreach (
        var district in
        placeInfos.OrderBy(d => d.StateName)
          .ThenBy(d => d.PlaceLongName))
      {
        csvWriter.AddField(district.StateFips);
        csvWriter.AddField(district.PlaceFips);
        csvWriter.AddField(district.StateCode);
        csvWriter.AddField(district.StateName);
        csvWriter.AddField(district.PlaceName);
        csvWriter.AddField(district.PlaceLongName);
        csvWriter.Write(textWriter);
      }
      textWriter.Close();
    }
  }
}
