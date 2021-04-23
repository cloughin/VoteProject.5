namespace DB.VoteTemp
{
  public partial class TempEmailAddressesRow
  {
  }

  public partial class TempEmailAddresses
  {
    public static TempEmailAddressesTable
      GetDataSortedByStateCodeElectionKeyPoliticianKey(int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText +
        " ORDER BY StateCode ASC,ElectionKey ASC,PoliticianKey ASC";
      var cmd = VoteTempDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, TempEmailAddressesTable.ColumnSet.All);
    }
  }
}