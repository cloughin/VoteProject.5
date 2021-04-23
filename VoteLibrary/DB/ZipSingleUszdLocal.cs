namespace DB.VoteZipNewLocal
{
  public partial class ZipSingleUszdRow
  {
  }

  public partial class ZipSingleUszd
  {
    public static ZipSingleUszdTable GetDataByZipCodes(string[] zipCodes)
    {
      return GetDataByZipCodes(zipCodes, -1);
    }

    public static ZipSingleUszdTable GetDataByZipCodes(
      string[] zipCodes, int commandTimeout)
    {
      var cmdText = $"{SelectAllCommandText} WHERE ZipCode IN ('{string.Join("','", zipCodes)}')";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, ZipSingleUszdTable.ColumnSet.All);
    }
  }
}