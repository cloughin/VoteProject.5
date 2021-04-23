using System.Globalization;

namespace DB.VoteZipNew
{
  public partial class ZipCitiesDownloadedRow
  {
  }

  public partial class ZipCitiesDownloaded
  {
    #region Public

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable UnassignedField.Global

    #endregion ReSharper disable

    public static ZipCitiesDownloadedReader GetAllDataReaderAt(int startAt)
    {
      return GetAllDataReaderAt(startAt, -1);
    }

    public static ZipCitiesDownloadedReader GetAllDataReaderAt(
      int startAt, int commandTimeout)
    {
      var cmdText = SelectAllCommandText + " LIMIT " +
        startAt.ToString(CultureInfo.InvariantCulture) + ",1000000000";
      var cn = VoteZipNewDb.GetOpenConnection();
      var cmd = VoteZipNewDb.GetCommand(cmdText, cn, commandTimeout);
      return new ZipCitiesDownloadedReader(cmd.ExecuteReader(), cn);
    }

    #region ReSharper restore

    // ReSharper restore UnassignedField.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Public
  }
}