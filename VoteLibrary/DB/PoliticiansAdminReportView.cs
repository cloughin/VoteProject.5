using System;
using System.Collections.Generic;
using System.Text;
using Vote;
using static System.String;

namespace DB.Vote
{
  public enum PoliticiansAdminReportViewOption
  {
    None,
    ByState,
    ByCounty,
    ByLocal
  }

  public class PoliticiansAdminReportViewOptions
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public PoliticiansAdminReportViewOption Option =
      PoliticiansAdminReportViewOption.None;

    public OfficeClass OfficeClass = OfficeClass.All;
    public string StateCode = Empty;
    public string CountyCode = Empty;
    public string LocalKey = Empty;

    public string BuildWhereClause()
    {
      var sbWhere = new StringBuilder();

      if (OfficeClass != OfficeClass.All ||
        Option != PoliticiansAdminReportViewOption.None)
      {
        var terms = new List<string>();
        if (OfficeClass != OfficeClass.All)
          terms.Add($"OfficeLevel={OfficeClass.ToInt()}");
        terms.Add($"StateCode='{StateCode}'");
        switch (Option)
        {
          case PoliticiansAdminReportViewOption.ByCounty:
            terms.Add($"CountyCode='{CountyCode}'");
            break;

          case PoliticiansAdminReportViewOption.ByLocal:
            terms.Add($"LocalKey='{LocalKey}'");
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

  public partial class PoliticiansAdminReportView
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMethodReturnValue.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global

    public static int CountData(PoliticiansAdminReportViewOptions options,
      int commandTimeout = -1)
    {
      var cmdText = "SELECT COUNT(*) FROM PoliticiansAdminReportView" +
        options.BuildWhereClause();
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var result = VoteDb.ExecuteScalar(cmd);
      return Convert.ToInt32(result);
    }

    public static PoliticiansAdminReportViewTable GetData(
      PoliticiansAdminReportViewOptions options, int commandTimeout = -1)
    {
      var cmdText = SelectAllCommandText + options.BuildWhereClause();
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return FillTable(cmd, PoliticiansAdminReportViewTable.ColumnSet.All);
    }


    // ReSharper restore UnusedAutoPropertyAccessor.Global
    // ReSharper restore UnusedMethodReturnValue.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public
  }
}