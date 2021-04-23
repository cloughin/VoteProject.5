using System;
using System.Collections.Generic;
using Vote;
using static System.String;

namespace DB.Vote
{
  //public class DonorsViewRow
  //{
  //}

  public static class DonorsView
  {
    public static string FormatEmailBatchesWhereClause(IList<string> stateCodes,
      IList<string> countyCodes, DateTime fromDate, DateTime toDate,
      string districtColumnName, IEnumerable<string> districts, string districtCoding)
    {
      var whereElements = new List<string>();
      var whereClause = Empty;

      whereElements.Add("(OptOut=0 OR OptOut IS NULL)");
      whereElements.Add("DonorOptOut=0");
      if (stateCodes != null && stateCodes.Count > 0 && stateCodes[0] != "all")
        whereElements.Add(stateCodes.SqlIn("StateCode"));
      if (countyCodes != null && countyCodes.Count > 0 && countyCodes[0] != "all")
        whereElements.Add(countyCodes.SqlIn("CountyCode"));
      if (fromDate != DateTime.MinValue.Date && fromDate != DateTime.MinValue)
        whereElements.Add("Date>=@fromDate");
      if (toDate != DateTime.MaxValue.Date && toDate != DateTime.MaxValue)
        whereElements.Add("Date<=@toDate");

      switch (districtCoding)
      {
        case "WithCoding":
          whereElements.Add(
            "(CongressionalDistrict<>'00' AND CongressionalDistrict<>'' AND NOT CongressionalDistrict IS NULL)");
          break;
        case "WithoutCoding":
          whereElements.Add(
            "(CongressionalDistrict='00' OR CongressionalDistrict='' OR CongressionalDistrict IS NULL)");
          break;
      }

      if (districts != null &&
        Array.IndexOf(
          new[]
          {
            "CongressionalDistrict", "StateSenateDistrict", "StateHouseDistrict", "County"
          }, districtColumnName) >= 0)
        whereElements.Add(districts.SqlIn(districtColumnName));

      if (whereElements.Count > 0)
        whereClause = $" WHERE {Join(" AND ", whereElements)}";

      return whereClause;
    }
  }
}