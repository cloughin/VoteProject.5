namespace DB.Vote
{
  public partial class OfficesAllIdentifiedRow
  {
  }

  public partial class OfficesAllIdentified
  {
    public static OfficesAllIdentifiedTable GetData(string stateCode,
      int officeLevel, string countyCode = "", string localCode = "",
      int commandTimeout = -1)
    {
      const string cmdText = "SELECT StateCode,CountyCode,LocalCode,OfficeLevel," +
        "IsOfficesAllIdentified FROM OfficesAllIdentified" +
        " WHERE StateCode=@StateCode AND OfficeLevel=@OfficeLevel" +
        " AND CountyCode=@CountyCode AND LocalCode=@LocalCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "OfficeLevel", officeLevel);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      VoteDb.AddCommandParameter(cmd, "LocalCode", localCode);
      return FillTable(cmd, OfficesAllIdentifiedTable.ColumnSet.All);
    }

    public static OfficesAllIdentifiedTable GetDataByStateCode(string stateCode,
      string countyCode = "", string localCode = "",
      int commandTimeout = -1)
    {
      const string cmdText = "SELECT StateCode,CountyCode,LocalCode,OfficeLevel," +
        "IsOfficesAllIdentified FROM OfficesAllIdentified" +
        " WHERE StateCode=@StateCode" +
        " AND CountyCode=@CountyCode AND LocalCode=@LocalCode";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      VoteDb.AddCommandParameter(cmd, "LocalCode", localCode);
      return FillTable(cmd, OfficesAllIdentifiedTable.ColumnSet.All);
    }

    public static bool GetIsOfficesAllIdentified(string stateCode, int officeLevel,
      string countyCode = "", string localCode = "")
    {
      // ReSharper disable once PossibleInvalidOperationException
      return _GetIsOfficesAllIdentifiedByStateCodeOfficeLevelCountyCodeLocalCode(
        stateCode, officeLevel, countyCode, localCode, false).Value;
    }

    public static int UpdateIsOfficesAllIdentified(bool newValue,
      string stateCode, int officeLevel, string countyCode = "",
      string localCode = "")
    {
      const string cmdText = "UPDATE OfficesAllIdentified" +
        " SET IsOfficesAllIdentified=@newValue WHERE StateCode=@StateCode" +
        " AND OfficeLevel=@OfficeLevel AND CountyCode=@CountyCode" +
        " AND LocalCode=@LocalCode";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      VoteDb.AddCommandParameter(cmd, "OfficeLevel", officeLevel);
      VoteDb.AddCommandParameter(cmd, "CountyCode", countyCode);
      VoteDb.AddCommandParameter(cmd, "LocalCode", localCode);
      VoteDb.AddCommandParameter(cmd, "newValue", newValue);
      return VoteDb.ExecuteNonQuery(cmd);
    }
  }
}