using System;
using System.Collections.Generic;
using System.Text;
using Vote;
using static System.String;

namespace DB.Vote
{
  public enum OfficesAdminReportViewOption
  {
    None,
    ByState,
    ByCounty,
    ByLocal
  }

  public class OfficesAdminReportViewOptions
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public OfficesAdminReportViewOption Option =
      OfficesAdminReportViewOption.None;

    public OfficeClass OfficeClass = OfficeClass.All;
    public string StateCode = Empty;
    public string CountyCode = Empty;
    public string LocalKey = Empty;

    public string BuildWhereClause()
    {
      var sbWhere = new StringBuilder();

      if (OfficeClass != OfficeClass.All ||
        Option != OfficesAdminReportViewOption.None)
      {
        var terms = new List<string>();
        if (OfficeClass != OfficeClass.All)
          terms.Add($"OfficeLevel={OfficeClass.ToInt()}");
        terms.Add($"StateCode='{StateCode}'");
        switch (Option)
        {
          case OfficesAdminReportViewOption.ByCounty:
            terms.Add($"CountyCode='{CountyCode}'");
            terms.Add("LocalKey=''");
            break;

          case OfficesAdminReportViewOption.ByLocal:
            terms.Add("CountyCode=''");
            terms.Add($"LocalKey='{LocalKey}'");
            break;

          default:
            terms.Add("CountyCode=''");
            terms.Add("LocalKey=''");
            break;
        }
        sbWhere.Append(Join(" AND ", terms));
      }

      if (sbWhere.Length == 0)
        throw new ArgumentException("no selections were made");

      return " WHERE " + sbWhere;
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }

  public partial class OfficesAdminReportView
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static int CountData(OfficesAdminReportViewOptions options,
      int commandTimeout = -1)
    {
      var cmdText = "SELECT COUNT(*) FROM OfficesAdminReportView" +
        options.BuildWhereClause();
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static OfficesAdminReportViewTable GetData(
      OfficesAdminReportViewOptions options, int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText + options.BuildWhereClause();
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, OfficesAdminReportViewTable.ColumnSet.All);
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}