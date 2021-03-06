using System.Globalization;

namespace DB.VoteZipNew
{
  public partial class ZipSingleUszdRow
  {
  }

  public partial class ZipSingleUszd
  {
    public static ZipSingleUszdReader GetAllDataReaderAt(int startAt)
    {
      return GetAllDataReaderAt(startAt, -1);
    }

    public static ZipSingleUszdReader GetAllDataReaderAt(
      int startAt, int commandTimeout)
    {
      var cmdText = SelectAllCommandText + " LIMIT " +
        startAt.ToString(CultureInfo.InvariantCulture) + ",1000000000";
      var cn = VoteZipNewDb.GetOpenConnection();
      var cmd = VoteZipNewDb.GetCommand(cmdText, cn, commandTimeout);
      return new ZipSingleUszdReader(cmd.ExecuteReader(), cn);
    }
  }
}