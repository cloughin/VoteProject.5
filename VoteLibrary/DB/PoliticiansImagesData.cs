using System;

namespace DB.Vote
{
  public partial class PoliticiansImagesData
  {
    public static int CountOutOfDateHeadshots()
    {
      const string cmdText = "SELECT COUNT(*) FROM PoliticiansImagesData" +
        " WHERE ProfileOriginalDate > HeadshotDate AND PoliticianKey!='NoPhoto'";
      var cmd = VoteDb.GetCommand(cmdText);
      return Convert.ToInt32(VoteDb.ExecuteScalar(cmd));
    }

    public static PoliticiansImagesDataTable GetDataForOutOfDateHeadshots(
      int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT PoliticianKey,ProfileOriginalDate,HeadshotDate,HeadshotResizeDate" +
        " FROM PoliticiansImagesData" + " WHERE ProfileOriginalDate > HeadshotDate AND PoliticianKey!='NoPhoto'";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, PoliticiansImagesDataTable.ColumnSet.All);
    }

    public static PoliticiansImagesDataTable GetDataByState(
      string stateCode, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT PoliticiansImagesData.PoliticianKey,PoliticiansImagesData.ProfileOriginalDate,PoliticiansImagesData.HeadshotDate,PoliticiansImagesData.HeadshotResizeDate" +
        " FROM PoliticiansImagesData,Politicians" +
        " WHERE Politicians.PoliticianKey=PoliticiansImagesData.PoliticianKey" +
        "  AND Politicians.StateCode=@StateCode" +
        "  AND PoliticiansImagesData.PoliticianKey!='NoPhoto'";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      return FillTable(cmd, PoliticiansImagesDataTable.ColumnSet.All);
    }

    public static PoliticiansImagesDataTable GetDataByStateForOutOfDateHeadshots(
      string stateCode, int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT PoliticiansImagesData.PoliticianKey,PoliticiansImagesData.ProfileOriginalDate,PoliticiansImagesData.HeadshotDate,PoliticiansImagesData.HeadshotResizeDate" +
        " FROM PoliticiansImagesData,Politicians" +
        " WHERE ProfileOriginalDate > HeadshotDate" +
        "  AND Politicians.PoliticianKey=PoliticiansImagesData.PoliticianKey" +
        "  AND Politicians.StateCode=@StateCode" +
        "  AND PoliticiansImagesData.PoliticianKey!='NoPhoto'";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      VoteDb.AddCommandParameter(cmd, "StateCode", stateCode);
      return FillTable(cmd, PoliticiansImagesDataTable.ColumnSet.All);
    }

    public static void GuaranteePoliticianKeyExists(string politicianKey)
    {
      if (!PoliticianKeyExists(politicianKey))
        Insert(
          politicianKey, VoteDb.DateTimeMin, VoteDb.DateTimeMin, VoteDb.DateTimeMin);
    }
  }
}