using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.Vote
{
  public partial class DomainDesigns
  {
    //public static string GetDesignStringWithSubstitutions(Column dataColumn,
    //  Column isTextColumn, Substitutions substitutions = null,
    //  int commandTimeout = -1)
    //{
    //  var dataColumnName = GetColumnName(dataColumn);
    //  var isTextColumnName = GetColumnName(isTextColumn);
    //  var cmdText =
    //    $"SELECT {dataColumnName},{isTextColumnName}  FROM DomainDesigns" +
    //    " WHERE DomainDesignCode=@DomainDesignCode";
    //  var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //  VoteDb.AddCommandParameter(cmd, "DomainDesignCode",
    //    UrlManager.CurrentDomainDesignCode);
    //  var table = new DataTable();
    //  using (var cn = VoteDb.GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //    adapter.Fill(table);
    //  }
    //  var text = string.Empty;
    //  if (table.Rows.Count == 1)
    //    text = (table.Rows[0][dataColumnName] as string).SafeString();
    //  if ((table.Rows.Count != 1) || string.IsNullOrWhiteSpace(text))
    //  {
    //    cmdText =
    //      $"SELECT {dataColumnName},{isTextColumnName}  FROM MasterDesign";
    //    cmd = VoteDb.GetCommand(cmdText, commandTimeout);
    //    table = new DataTable();
    //    using (var cn = VoteDb.GetOpenConnection())
    //    {
    //      cmd.Connection = cn;
    //      DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
    //      adapter.Fill(table);
    //    }
    //    if (table.Rows.Count == 0) return string.Empty;
    //    text = (table.Rows[0][dataColumnName] as string).SafeString();
    //  }
    //  if (Convert.ToBoolean(table.Rows[0][isTextColumnName]))
    //    text = text.ReplaceNewLinesWithBreakTags();
    //  if (substitutions == null)
    //    substitutions = new Substitutions();
    //  return substitutions.Substitute(text);
    //}

    public static string GetDesignStringWithSubstitutions(string dataColumnName,
      Substitutions substitutions = null)
    {
      return GetDesignStringWithSubstitutions(GetColumn(dataColumnName),
        substitutions);
    }

    public static string GetDesignStringWithSubstitutions(Column dataColumn,
      Substitutions substitutions = null)
    {
      var text = GetColumn(dataColumn, UrlManager.CurrentDomainDesignCode) as string;
      if (string.IsNullOrWhiteSpace(text))
      {
        var dataColumnName = GetColumnName(dataColumn);
        var masterColumn = MasterDesign.GetColumn(dataColumnName);
        text = MasterDesign.GetColumn(masterColumn) as string;
      }
      if (substitutions == null)
        substitutions = new Substitutions();
      return substitutions.Substitute(text);
    }
  }
}