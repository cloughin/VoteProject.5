using System;
using System.Collections.Generic;
using System.Text;
using Vote;

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
    public string StateCode = string.Empty;
    public string CountyCode = string.Empty;
    public string LocalCode = string.Empty;

    public string BuildWhereClause()
    {
      var sbWhere = new StringBuilder();

      if ((OfficeClass != OfficeClass.All) ||
        (Option != OfficesAdminReportViewOption.None))
      {
        var terms = new List<string>();
        if (OfficeClass != OfficeClass.All)
          terms.Add($"OfficeLevel={OfficeClass.ToInt()}");
        terms.Add($"StateCode='{StateCode}'");
        if (Option >= OfficesAdminReportViewOption.ByCounty)
        {
          terms.Add($"CountyCode='{CountyCode}'");
          terms.Add(Option == OfficesAdminReportViewOption.ByLocal
            ? $"LocalCode='{LocalCode}'"
            : "LocalCode=''");
        }
        else
        {
          terms.Add("CountyCode=''");
          terms.Add("LocalCode=''");
        }
        sbWhere.Append(string.Join(" AND ", terms));
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

    public static int CountData(
      OfficesAdminReportViewOption option = OfficesAdminReportViewOption.None,
      OfficeClass officeClass = OfficeClass.All, string stateCode = "",
      string countyCode = "", string localCode = "", int commandTimeout = -1)
    {
      var options = new OfficesAdminReportViewOptions
      {
        Option = option,
        OfficeClass = officeClass,
        StateCode = stateCode,
        CountyCode = countyCode,
        LocalCode = localCode
      };

      return CountData(options, commandTimeout);
    }

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
      OfficesAdminReportViewOption option = OfficesAdminReportViewOption.None,
      OfficeClass officeClass = OfficeClass.All, string stateCode = "",
      string countyCode = "", string localCode = "", int commandTimeout = -1)
    {
      var options = new OfficesAdminReportViewOptions
      {
        Option = option,
        OfficeClass = officeClass,
        StateCode = stateCode,
        CountyCode = countyCode,
        LocalCode = localCode
      };

      return GetData(options, commandTimeout);
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