using System.Globalization;

namespace DB.VoteZipNew
{
  public partial class ZipStreetsRow
  {
  }

  public partial class ZipStreets
  {
    //public static ZipStreetsReader GetAllDataReaderAt(int startAt)
    //{
    //  return GetAllDataReaderAt(startAt, -1);
    //}

    public static ZipStreetsReader GetAllDataReaderAt(
      int startAt, int commandTimeout)
    {
      var cmdText = SelectAllCommandText + " LIMIT " +
        startAt.ToString(CultureInfo.InvariantCulture) + ",1000000000";
      var cn = VoteZipNewDb.GetOpenConnection();
      var cmd = VoteZipNewDb.GetCommand(cmdText, cn, commandTimeout);
      return new ZipStreetsReader(cmd.ExecuteReader(), cn);
    }
  }
}