namespace DB.Vote
{
  public /*partial*/ class PoliticiansVideosData
  {
    //#region Public

    //#region ReSharper disable

    //// ReSharper disable MemberCanBePrivate.Global
    //// ReSharper disable MemberCanBeProtected.Global
    //// ReSharper disable UnusedMember.Global
    //// ReSharper disable UnusedMethodReturnValue.Global
    //// ReSharper disable UnusedAutoPropertyAccessor.Global
    //// ReSharper disable UnassignedField.Global

    //#endregion ReSharper disable

    //public static int CountUnprocessed(int commandTimeout = -1)
    //{
    //  const string cmdText =
    //    "SELECT COUNT(*) FROM PoliticiansVideosData" + " WHERE IsProcessed = 0";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  var result = VoteDb.ExecuteScalar(cmd);
    //  return Convert.ToInt32(result);
    //}

    //public static int CountUnprocessedByPoliticianKey(
    //  string politicianKey, int commandTimeout = -1)
    //{
    //  const string cmdText =
    //    "SELECT COUNT(*) FROM PoliticiansVideosData" + " WHERE IsProcessed = 0" +
    //      "   AND PoliticianKey = @PoliticianKey";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
    //  var result = VoteDb.ExecuteScalar(cmd);
    //  return Convert.ToInt32(result);
    //}
    //public static PoliticiansVideosDataTable GetNextUnprocessedVideo(
    //  DateTime uploadDate, string politicianKey, int commandTimeout = -1)
    //{
    //  var cmdText = SelectAllCommandText + " WHERE IsProcessed=0" +
    //    "  AND (UploadDate < @UploadDate" + "   OR (UploadDate = @UploadDate" +
    //    "    AND PoliticianKey > @PoliticianKey))" +
    //    " ORDER BY UploadDate DESC,PoliticianKey";
    //  cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  VoteDb.AddCommandParameter(cmd, "UploadDate", uploadDate);
    //  VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
    //  return FillTable(cmd, PoliticiansVideosDataTable.ColumnSet.All);
    //}

    //public static PoliticiansVideosDataTable GetNextUnprocessedVideoByPoliticianKey(
    //  DateTime uploadDate, string politicianKey, int commandTimeout = -1)
    //{
    //  var cmdText = SelectAllCommandText + " WHERE IsProcessed=0" +
    //    "  AND PoliticianKey = @PoliticianKey" + "  AND UploadDate < @UploadDate" +
    //    " ORDER BY UploadDate DESC";
    //  cmdText = VoteDb.InjectSqlLimit(cmdText, 1);
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  VoteDb.AddCommandParameter(cmd, "UploadDate", uploadDate);
    //  VoteDb.AddCommandParameter(cmd, "PoliticianKey", politicianKey);
    //  return FillTable(cmd, PoliticiansVideosDataTable.ColumnSet.All);
    //}

    //public static void GuaranteePrimaryKeyExists(
    //  String politicianKey, String questionkey, DateTime uploadDate)
    //{
    //  if (
    //    !PoliticianKeyQuestionKeyUploadDateExists(
    //      politicianKey, QuestionKeyColumnName, uploadDate))
    //    Insert(
    //      politicianKey, QuestionKeyColumnName, uploadDate, VoteDb.DateTimeMin, null,
    //      null, string.Empty, false);
    //}

    //  #region ReSharper restore

    //  // ReSharper restore UnassignedField.Global
    //  // ReSharper restore UnusedAutoPropertyAccessor.Global
    //  // ReSharper restore UnusedMethodReturnValue.Global
    //  // ReSharper restore UnusedMember.Global
    //  // ReSharper restore MemberCanBeProtected.Global
    //  // ReSharper restore MemberCanBePrivate.Global

    //  #endregion ReSharper restore

    //  #endregion Public
  }
}