using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Vote;

namespace DB.Vote
{
  public partial class MasterDesign
  {
    public static string GetDesignStringWithSubstitutions(Column dataColumn,
      Column isTextColumn, Substitutions substitutions = null,
      int commandTimeout = -1)
    {
      var dataColumnName = GetColumnName(dataColumn);
      var isTextColumnName = GetColumnName(isTextColumn);
      var cmdText =
        $"SELECT {dataColumnName},{isTextColumnName}  FROM MasterDesign";
      var cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      var table = new DataTable();
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        DbDataAdapter adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
      }
      if (table.Rows.Count != 1) return string.Empty;
      var text = (table.Rows[0][dataColumnName] as string).SafeString();
      if (Convert.ToBoolean(table.Rows[0][isTextColumnName]))
        text = text.ReplaceNewLinesWithBreakTags();
      if (substitutions == null)
        substitutions = new Substitutions();
      return substitutions.Substitute(text);
    }

    public static string GetDesignStringWithSubstitutions(Column dataColumn,
      Substitutions substitutions = null)
    {
      var text = GetColumn(dataColumn) as string;
      if (substitutions == null)
        substitutions = new Substitutions();
      return substitutions.Substitute(text);
    }
  }
}