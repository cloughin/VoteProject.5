using System.Collections.Generic;
using System.Text;
using Vote;

namespace DB.VoteZipNewLocal
{
  public partial class UszdRow
  {
  }

  public partial class Uszd
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

    public static UszdTable GetDataByZipPlus4List(
      IEnumerable<ZipPlus4> zipPlus4List, int commandTimeout = -1)
    {
      var sbWhere = new StringBuilder();
      sbWhere.Append("(");
      var first = true;
      foreach (var zipPlus4 in zipPlus4List)
      {
        if (!first)
          sbWhere.Append(" OR ");
        first = false;
        sbWhere.Append("(ZIP5='");
        sbWhere.Append(zipPlus4.Zip5);
        sbWhere.Append("' AND ZIP4='");
        sbWhere.Append(zipPlus4.Zip4);
        sbWhere.Append("')");
      }
      sbWhere.Append(")");

      var cmdText = $"{SelectAllCommandText} WHERE {sbWhere}";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, UszdTable.ColumnSet.All);
    }

    public static UszdTable GetDataByZipPlus4Range(
      string zip5, string zip4Low, string zip4High, int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText +
        " WHERE ZIP5=@Zip5 AND ZIP4 >= @Zip4Low AND ZIP4 <= @Zip4High";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "Zip5", zip5);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "Zip4Low", zip4Low);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "Zip4High", zip4High);
      return FillTable(cmd, UszdTable.ColumnSet.All);
    }

    public static UszdTable GetDataByZip4List(
      string zip5, IEnumerable<string> zip4List, int commandTimeout = -1)
    {
      var cmdText =
        $"{SelectAllCommandText} WHERE ZIP5=@Zip5 AND ZIP4 IN ('{string.Join("','", zip4List)}')";
      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, commandTimeout);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "Zip5", zip5);
      return FillTable(cmd, UszdTable.ColumnSet.All);
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