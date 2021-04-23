using System;
using System.IO;
using DB.VoteZipNew;

namespace Vote
{
  public class StreetAnalysisData
  {
    public string ZipCode;
    public string UpdateKey;
    public string DirectionPrefix;
    public string StreetName;
    public string StreetSuffix;
    public string DirectionSuffix;
    public string PrimaryLowNumber;
    public string PrimaryHighNumber;
    public string PrimaryOddEven;
    public string BuildingName;
    public string SecondaryType;
    public string SecondaryLowNumber;
    public string SecondaryHighNumber;
    public string SecondaryOddEven;
    public string Plus4Low;
    public string Plus4High;
    public string State;
    public LdsInfo LdsInfo;

    // For caching metaphone lookups
    private static string _MetaphoneStreet;
    private static string _MetaphoneValue;

    private StreetAnalysisData()
    {
    }

    public StreetAnalysisData(ZipStreetsDownloadedReader reader)
    {
      ZipCode = reader.ZipCode;
      UpdateKey = reader.UpdateKey;
      DirectionPrefix = reader.DirectionPrefix;
      StreetName = reader.StreetName;
      StreetSuffix = reader.StreetSuffix;
      DirectionSuffix = reader.DirectionSuffix;
      PrimaryLowNumber = reader.PrimaryLowNumber;
      PrimaryHighNumber = reader.PrimaryHighNumber;
      BuildingName = reader.BuildingName;
      PrimaryOddEven = reader.PrimaryOddEven;
      SecondaryType = reader.SecondaryType;
      SecondaryLowNumber = reader.SecondaryLowNumber;
      SecondaryHighNumber = reader.SecondaryHighNumber;
      SecondaryOddEven = reader.SecondaryOddEven;
      Plus4Low = reader.Plus4Low;
      Plus4High = reader.Plus4High;
      State = reader.State;
    }

    //private static void AddField(List<string> fields, string field)
    //{
    //  field = field.Replace("\"", "\"\""); // double up quotes
    //  fields.Add("\"" + field + "\""); // enclose in quotes
    //}

    public StreetAnalysisData Clone()
    {
      var cloned = new StreetAnalysisData
      {
        ZipCode = ZipCode,
        UpdateKey = UpdateKey,
        DirectionPrefix = DirectionPrefix,
        StreetName = StreetName,
        StreetSuffix = StreetSuffix,
        DirectionSuffix = DirectionSuffix,
        PrimaryLowNumber = PrimaryLowNumber,
        BuildingName = BuildingName,
        PrimaryHighNumber = PrimaryHighNumber,
        PrimaryOddEven = PrimaryOddEven,
        SecondaryType = SecondaryType,
        SecondaryLowNumber = SecondaryLowNumber,
        SecondaryHighNumber = SecondaryHighNumber,
        SecondaryOddEven = SecondaryOddEven,
        Plus4Low = Plus4Low,
        Plus4High = Plus4High,
        State = State,
        LdsInfo = LdsInfo
      };

      return cloned;
    }

    public bool IsWildcard => (PrimaryLowNumber.Length == 0) &&
      (PrimaryHighNumber.Length == 0);

    public bool MatchesSecondary(StreetAnalysisData other) =>
      (SecondaryLowNumber == other.SecondaryLowNumber) &&
      (SecondaryHighNumber == other.SecondaryHighNumber) &&
      (SecondaryOddEven == other.SecondaryOddEven) &&
      (BuildingName == other.BuildingName);

    public string PrimaryOddEvenNormalized => (PrimaryOddEven == "E") || (PrimaryOddEven == "O")
      ? PrimaryOddEven
      : "B";

    public void SetMissingLdsInfo() => LdsInfo = LdsInfo.Missing;

    public void Write(TextWriter writer)
    {
      if (StreetName != _MetaphoneStreet)
        try
        {
          var metaphone = DoubleMetaphone.EncodePhrase(StreetName);
          if (metaphone.Length > ZipStreets.MetaphoneMaxLength)
            metaphone = metaphone.Substring(0, ZipStreets.MetaphoneMaxLength);
          _MetaphoneStreet = StreetName;
          _MetaphoneValue = metaphone;
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch (Exception /*ex*/)
        {
        }

      if (writer == null) // write to database
        ZipStreets.Insert(ZipCode, UpdateKey, DirectionPrefix, StreetName, StreetSuffix,
          DirectionSuffix, PrimaryLowNumber, PrimaryHighNumber, PrimaryOddEven,
          BuildingName, SecondaryType, SecondaryLowNumber, SecondaryHighNumber,
          SecondaryOddEven, _MetaphoneValue, State, LdsInfo.Congress,
          LdsInfo.StateSenate, LdsInfo.StateHouse, LdsInfo.County);
      else // write to csv
      {
        var csvWriter = new SimpleCsvWriter();
        csvWriter.AddField(ZipCode);
        csvWriter.AddField(UpdateKey);
        csvWriter.AddField(DirectionPrefix);
        csvWriter.AddField(StreetName);
        csvWriter.AddField(StreetSuffix);
        csvWriter.AddField(DirectionSuffix);
        csvWriter.AddField(PrimaryLowNumber);
        csvWriter.AddField(PrimaryHighNumber);
        csvWriter.AddField(PrimaryOddEven);
        csvWriter.AddField(BuildingName);
        csvWriter.AddField(SecondaryType);
        csvWriter.AddField(SecondaryLowNumber);
        csvWriter.AddField(SecondaryHighNumber);
        csvWriter.AddField(SecondaryOddEven);
        csvWriter.AddField(_MetaphoneValue);
        csvWriter.AddField(State);
        csvWriter.AddField(LdsInfo.Congress);
        csvWriter.AddField(LdsInfo.StateSenate);
        csvWriter.AddField(LdsInfo.StateHouse);
        csvWriter.AddField(LdsInfo.County);
        csvWriter.Write(writer);
      }
    }
  }
}