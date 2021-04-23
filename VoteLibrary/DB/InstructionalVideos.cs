namespace DB.Vote
{
  public partial class InstructionalVideos
  {
    public static InstructionalVideosTable GetAdminData(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT Id,Title,Description,EmbedCode,Url,AdminOrder,VolunteersOrder" +
        " FROM InstructionalVideos WHERE AdminOrder!=0 ORDER BY AdminOrder";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, InstructionalVideosTable.ColumnSet.All);
    }

    public static InstructionalVideosTable GetVolunteersData(int commandTimeout = -1)
    {
      const string cmdText =
        "SELECT Id,Title,Description,EmbedCode,Url,AdminOrder,VolunteersOrder" +
        " FROM InstructionalVideos WHERE VolunteersOrder!=0 ORDER BY VolunteersOrder";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, InstructionalVideosTable.ColumnSet.All);
    }
  }
}