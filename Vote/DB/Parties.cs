namespace DB.Vote
{
  public partial class PartiesRow
  {
  }

  public partial class Parties
  {
    public static PartiesTable GetMajorPartyDataByStateCode(string stateCode,
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT PartyKey,PartyCode,StateCode,PartyOrder,PartyName,PartyURL,PartyAddressLine1,PartyAddressLine2,PartyCityStateZip,IsPartyMajor FROM Parties WHERE StateCode=@StateCode AND IsPartyMajor=1 AND LENGTH(PartyCode)=1 ORDER BY PartyOrder";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      return FillTable(cmd, PartiesTable.ColumnSet.All);
    }

    //public static SimpleListItem[] GetVolunteerReportParties(string stateCode, bool forEdit)
    //{
    //  // stateCode can be "" (none) or "*ALL*"
    //  switch (stateCode)
    //  {
    //    case "":
    //      return new[] { new SimpleListItem { Text = "no party", Value = String.Empty } };

    //    case "*ALL*":
    //      return new[] { new SimpleListItem { Text = "any party", Value = String.Empty } };

    //    default:
    //      var result = new List<SimpleListItem>
    //      {
    //        new SimpleListItem {Text = "<select a party>", Value = String.Empty}
    //      };
    //      if (!forEdit)
    //        result.Add(new SimpleListItem { Text = "all parties", Value = "*ALL*" });
    //      return result
    //        .Union(GetDataByStateCode(stateCode)
    //          .Select(row => new SimpleListItem { Text = row.PartyName, Value = row.PartyKey })
    //          .OrderBy(i => i.Text))
    //        .ToArray();
    //  }
    //}
  }
}