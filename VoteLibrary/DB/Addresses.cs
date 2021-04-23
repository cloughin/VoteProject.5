using System;
using System.Collections.Generic;
using Vote;
using static System.String;

namespace DB.Vote
{
  public partial class Addresses
  {
    public static string FormatEmailBatchesWhereClause(
      IList<string> stateCodes, IList<string> countyCodes, bool sampleBallots,
      bool sharedChoices, bool donated, bool withNames, 
      bool withoutNames, bool withDistrictCoding, 
      bool withoutDistrictCoding, DateTime fromDate, DateTime toDate,
      string districtColumnName, IEnumerable<string> districts)
    {
      var whereElements = new List<string>();
      var whereClause = Empty;

      whereElements.Add("Email<>''");
      whereElements.Add("OptOut=0");
      if (stateCodes != null && stateCodes.Count > 0 && stateCodes[0] != "all")
        whereElements.Add(stateCodes.SqlIn("StateCode"));
      if (countyCodes != null && countyCodes.Count > 0 && countyCodes[0] != "all")
        whereElements.Add(countyCodes.SqlIn("County"));
      if (sampleBallots || sharedChoices || donated)
      {
        var types = new List<string>();
        if (sampleBallots) types.Add("SendSampleBallots=1");
        if (sharedChoices) types.Add("SentBallotChoices=1");
        if (donated) types.Add("IsDonor=1");
        whereElements.Add($"({Join(" OR ", types)})");
      }
      if (withNames) whereElements.Add("(FirstName<>'' OR LastName<>'')");
      if (withoutNames) whereElements.Add("(FirstName='' AND LastName='')");
      //if (withDistrictCoding) whereElements.Add("(CongressionalDistrict<>'00' AND CongressionalDistrict<>'')");
      if (withDistrictCoding) whereElements.Add("(NOT Latitude IS NULL AND NOT Longitude IS NULL)");
      //if (withoutDistrictCoding) whereElements.Add("(CongressionalDistrict='00' OR CongressionalDistrict='')");
      if (withoutDistrictCoding) whereElements.Add("(Latitude IS NULL OR Longitude IS NULL)");
      if (fromDate != DateTime.MinValue.Date && fromDate != DateTime.MinValue)
        whereElements.Add("DateStamp>=@fromDate");
      if (toDate != DateTime.MaxValue.Date && toDate != DateTime.MaxValue) 
        whereElements.Add("DateStamp<=@toDate");
      if (districts != null &&
        Array.IndexOf(
          new[]
            {
              "CongressionalDistrict", "StateSenateDistrict", "StateHouseDistrict",
              "County"
            }, districtColumnName) >= 0)
        whereElements.Add(districts.SqlIn(districtColumnName));

      if (whereElements.Count > 0)
        whereClause = $" WHERE {Join(" AND ", whereElements)}";

      return whereClause;
    }

    public static AddressesTable GetAllGeocodedData(int commandTimeout = -1)
    {
      var cmdText = SelectDistrictCodingCommandText +
        " WHERE NOT Latitude IS NULL AND Latitude!=0 AND NOT Longitude IS NULL AND Longitude!=0";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, AddressesTable.ColumnSet.DistrictCoding);
    }

    public static AddressesTable GetAllNonEmptyEmailsData(int commandTimeout = -1)
    {
      const string cmdText = "SELECT Id,Email FROM Addresses WHERE Email<>''";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, AddressesTable.ColumnSet.Emails);
    }

    //public static AddressesTable GetAllUncodedDistrictCodingData(int commandTimeout = -1)
    //{
    //  const string cmdText = "SELECT Id,Address,City,StateCode,Zip5,Zip4,Email,CongressionalDistrict,StateSenateDistrict,StateHouseDistrict,County,DistrictLookupDate FROM Addresses WHERE Email<>'' AND ((Address<>'') OR (Zip5<>'' AND Zip4<>'')) AND CongressionalDistrict='' AND DistrictLookupDate='1900-01-01'";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  return FillTable(cmd, AddressesTable.ColumnSet.DistrictCoding);
    //}

    //public static AddressesReader GetDataReaderForAddressExtraction(
    //  ICollection<string> stateCodes, bool nameRequired, bool addressRequired,
    //  bool emailRequired, bool noEmailRequired, bool phoneRequired, DateTime fromDate,
    //  DateTime toDate, int commandTimeout = -1)
    //{
    //  var whereClause = FormatAddressExtractionWhereClause(
    //    stateCodes, nameRequired, addressRequired, emailRequired, noEmailRequired,
    //    phoneRequired, fromDate, toDate, null, null);
    //  var cmdText = GetSelectCommandText(AddressesTable.ColumnSet.All) + whereClause +
    //    " ORDER BY Zip5, Zip4";

    //  var cn = VoteDb.GetOpenConnection();
    //  var cmd = VoteDb.GetCommand(cmdText, cn, commandTimeout);
    //  if (fromDate != DateTime.MinValue.Date)
    //    VoteDb.AddCommandParameter(cmd, "fromDate", fromDate);
    //  if (toDate != DateTime.MaxValue.Date)
    //    VoteDb.AddCommandParameter(cmd, "toDate", toDate);
    //  return new AddressesReader(cmd.ExecuteReader(), cn);
    //}
  }
}