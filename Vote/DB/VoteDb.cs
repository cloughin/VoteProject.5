using System;
using System.Globalization;

namespace DB.Vote
{
  public static partial class VoteDb
  {
    public static DateTime DateTimeMax
    {
      get
      {
        // to keep MSSQL SmallDateTime happy
        return new DateTime(2079, 6, 6, 23, 59, 0);
      }
    }

    public static DateTime DateTimeMin
    {
      get { return new DateTime(1900, 1, 1); }
    }

    public static string InjectSqlLimit(string selectCmdText, int limit)
    {
      switch (DbProvider)
      {
        case DbProvider.MsSql:
        {
          selectCmdText = selectCmdText.TrimStart();
          if (selectCmdText.StartsWith(
            "SELECT ", StringComparison.OrdinalIgnoreCase))
            selectCmdText = selectCmdText.Insert(
              7, "TOP " + limit.ToString(CultureInfo.InvariantCulture) + " ");
          return selectCmdText;
        }

        case DbProvider.MySql:
          return selectCmdText + " LIMIT " +
            limit.ToString(CultureInfo.InvariantCulture);

        default:
          throw new ApplicationException(
            $"Unsupported provider: {DbProvider}");
      }
    }
  }
}