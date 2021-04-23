using System;
using System.Collections.Generic;
using Vote;

namespace DB.Vote
{
  public partial class Addresses
  {
    // Shared with DistrictAddressesView
    internal static string FormatAddressExtractionWhereClause(
      ICollection<string> stateCodes, bool nameRequired, bool addressRequired,
      bool emailRequired, bool noEmailRequired, bool phoneRequired, DateTime fromDate,
      DateTime toDate, string districtColumnName, IEnumerable<string> districts)
    {
      var whereElements = new List<string>();
      var whereClause = string.Empty;

      if (stateCodes != null && stateCodes.Count > 0)
        whereElements.Add(
          $"StateCode IN ('{string.Join("','", stateCodes)}')");
      if (nameRequired) whereElements.Add("(FirstName<>'' OR LastName<>'')");
      if (addressRequired) whereElements.Add("Address<>''");
      if (emailRequired) whereElements.Add("Email<>''");
      if (noEmailRequired) whereElements.Add("Email=''");
      if (phoneRequired) whereElements.Add("Phone<>''");
      if (fromDate != DateTime.MinValue.Date)
        whereElements.Add("DateStamp>=@fromDate");
      if (toDate != DateTime.MaxValue.Date) whereElements.Add("DateStamp<=@toDate");
      if (districts != null &&
        Array.IndexOf(new[] {"CD", "SD", "HD", "CNTY"}, districtColumnName) >= 0)
        whereElements.Add(
          $"{districtColumnName} IN ('{string.Join("','", districts)}')");

      if (whereElements.Count > 0)
        whereClause = $" WHERE {string.Join(" AND ", whereElements)}";

      return whereClause;
    }

    //private static string FormatEmailBatchesWhereClause(
    //  IEnumerable<string> stateCodes, bool withNames, bool withoutNames,
    //  bool withAddresses, bool withoutAddresses, bool appendedEmails,
    //  bool enteredEmails, bool withDistrictCoding, bool withoutDistrictCoding,
    //  DateTime fromDate, DateTime toDate, string districtColumnName,
    //  IEnumerable<string> districts)
    //{
    //  return FormatEmailBatchesWhereClause(stateCodes.ToArray(), null,
    //    withNames, withoutNames, withAddresses, withoutAddresses, appendedEmails,
    //    enteredEmails, withDistrictCoding, withoutDistrictCoding, fromDate, toDate,
    //    districtColumnName, districts.ToArray());
    //}

    public static string FormatEmailBatchesWhereClause(
      IList<string> stateCodes, IList<string> countyCodes, bool sampleBallots,
      bool sharedChoices, bool donated, bool withNames, 
      bool withoutNames, bool withDistrictCoding, 
      bool withoutDistrictCoding, DateTime fromDate, DateTime toDate,
      string districtColumnName, IEnumerable<string> districts)
    {
      var whereElements = new List<string>();
      var whereClause = string.Empty;

      whereElements.Add("Email<>''");
      whereElements.Add("OptOut=0");
      //if (forSampleBallots) whereElements.Add("SendSampleBallots=1");
      if (stateCodes != null && stateCodes.Count > 0 && stateCodes[0] != "all")
        whereElements.Add(stateCodes.SqlIn("StateCode"));
      if (countyCodes != null && countyCodes.Count > 0 && countyCodes[0] != "all")
        whereElements.Add(countyCodes.SqlIn("CountyCode"));
      if (sampleBallots || sharedChoices || donated)
      {
        var types = new List<string>();
        if (sampleBallots) types.Add("SendSampleBallots=1");
        if (sharedChoices) types.Add("SentBallotChoices=1");
        if (donated) types.Add("IsDonor=1");
        whereElements.Add($"({string.Join(" OR ", types)})");
      }
      if (withNames) whereElements.Add("(FirstName<>'' OR LastName<>'')");
      if (withoutNames) whereElements.Add("(FirstName='' AND LastName='')");
      //if (withAddresses) whereElements.Add("Address<>''");
      //if (withoutAddresses) whereElements.Add("Address=''");
      //if (appendedEmails)
      //  whereElements.Add("EmailAttachedDate<>'1900-01-01 00:00:00'");
      //if (enteredEmails)
      //  whereElements.Add("EmailAttachedDate='1900-01-01 00:00:00'");
      if (withDistrictCoding) whereElements.Add("(CongressionalDistrict<>'00' AND CongressionalDistrict<>'')");
      if (withoutDistrictCoding) whereElements.Add("(CongressionalDistrict='00' OR CongressionalDistrict='')");
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
        whereClause = $" WHERE {string.Join(" AND ", whereElements)}";

      return whereClause;
    }

    public static AddressesTable GetAllCodeableDistrictCodingData(int commandTimeout = -1)
    {
      const string cmdText = "SELECT Id,Address,City,StateCode,Zip5,Zip4,Email,CongressionalDistrict,StateSenateDistrict,StateHouseDistrict,County,DistrictLookupDate FROM Addresses WHERE Email<>'' AND ((Address<>'') OR (Zip5<>'' AND Zip4<>''))";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, AddressesTable.ColumnSet.DistrictCoding);
    }

    public static AddressesTable GetAllNonEmptyEmailsData(int commandTimeout = -1)
    {
      const string cmdText = "SELECT Id,Email FROM Addresses WHERE Email<>''";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, AddressesTable.ColumnSet.Emails);
    }

    public static AddressesTable GetAllUncodedDistrictCodingData(int commandTimeout = -1)
    {
      const string cmdText = "SELECT Id,Address,City,StateCode,Zip5,Zip4,Email,CongressionalDistrict,StateSenateDistrict,StateHouseDistrict,County,DistrictLookupDate FROM Addresses WHERE Email<>'' AND ((Address<>'') OR (Zip5<>'' AND Zip4<>'')) AND CongressionalDistrict='' AND DistrictLookupDate='1900-01-01'";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, AddressesTable.ColumnSet.DistrictCoding);
    }

    public static AddressesReader GetDataReaderForAddressExtraction(
      ICollection<string> stateCodes, bool nameRequired, bool addressRequired,
      bool emailRequired, bool noEmailRequired, bool phoneRequired, DateTime fromDate,
      DateTime toDate, int commandTimeout = -1)
    {
      var whereClause = FormatAddressExtractionWhereClause(
        stateCodes, nameRequired, addressRequired, emailRequired, noEmailRequired,
        phoneRequired, fromDate, toDate, null, null);
      var cmdText = GetSelectCommandText(AddressesTable.ColumnSet.All) + whereClause +
        " ORDER BY Zip5, Zip4";

      var cn = VoteDb.GetOpenConnection();
      var cmd = VoteDb.GetCommand(cmdText, cn, commandTimeout);
      if (fromDate != DateTime.MinValue.Date)
        VoteDb.AddCommandParameter(cmd, "fromDate", fromDate);
      if (toDate != DateTime.MaxValue.Date)
        VoteDb.AddCommandParameter(cmd, "toDate", toDate);
      return new AddressesReader(cmd.ExecuteReader(), cn);
    }

    //public static AddressesReader GetDataReaderForEmailBatches(
    //  ICollection<string> stateCodes, bool withNames, bool withoutNames,
    //  bool withAddresses, bool withoutAddresses, bool appendedEmails,
    //  bool enteredEmails, bool withDistrictCoding, bool withoutDistrictCoding,
    //  DateTime fromDate, DateTime toDate, string districtColumnName,
    //  ICollection<string> districts, int commandTimeout = -1)
    //{
    //  var whereClause = FormatEmailBatchesWhereClause(
    //    stateCodes, withNames, withoutNames, withAddresses, withoutAddresses,
    //    appendedEmails, enteredEmails, withDistrictCoding, withoutDistrictCoding, fromDate,
    //    toDate, districtColumnName, districts);
    //  var cmdText = GetSelectCommandText(AddressesTable.ColumnSet.All) + whereClause;

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