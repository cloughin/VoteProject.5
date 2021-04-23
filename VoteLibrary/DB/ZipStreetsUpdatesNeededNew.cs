using System.Globalization;

namespace DB.VoteZipNew
{
  public partial class ZipStreetsUpdatesNeededRow
  {
  }

  public partial class ZipStreetsUpdatesNeeded
  {
    //public static ZipStreetsUpdatesNeededReader GetAllDataReaderAt(int startAt)
    //{
    //  return GetAllDataReaderAt(startAt, -1);
    //}

    public static ZipStreetsUpdatesNeededReader GetAllDataReaderAt(
      int startAt, int commandTimeout)
    {
      var cmdText = SelectAllCommandText + " LIMIT " +
        startAt.ToString(CultureInfo.InvariantCulture) + ",1000000000";
      var cn = VoteZipNewDb.GetOpenConnection();
      var cmd = VoteZipNewDb.GetCommand(cmdText, cn, commandTimeout);
      return new ZipStreetsUpdatesNeededReader(cmd.ExecuteReader(), cn);
    }
  }
}