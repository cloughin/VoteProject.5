using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace VoteSmart
{
  public partial class MainForm
  {
    private static MySqlConnection GetConnection()
    {
      return new MySqlConnection("Server=localhost;Uid=root;Pwd=p17134;Database=votesmart");
    }

    private static MySqlConnection GetOpenConnection()
    {
      var cn = GetConnection();
      if (cn != null) cn.Open();
      return cn;
    }

    private static DataTable CreateFetchesRawTable()
    {
      var table = new DataTable();
      table.Columns.Add("fetch_time");
      table.Columns["fetch_time"].DataType = typeof(DateTime);
      table.Columns.Add("fetch_method");
      table.Columns.Add("fetch_parameters");
      table.Columns.Add("fetch_type");
      table.Columns.Add("fetch_data");
      return table;
    }

    private static IEnumerable<KeyValuePair<int, string>> GetCandidatesFromTable(string tableName,
      string columnName = "candidateId", string joinTableName = null,
      string joinColumnName = null)
    {
      const string cmdTemplate = "SELECT {0},stateId FROM {1}{2} GROUP BY {0}";
      var joinClause = string.Empty;
      if (!string.IsNullOrWhiteSpace(joinTableName))
      {
        joinClause = string.Format(" LEFT JOIN {0} ON {0}.{2}={1}.{2} ",
          joinTableName, tableName, joinColumnName);
      }
      var cmdText = string.Format(cmdTemplate, columnName, tableName, 
        joinClause);
      var cmd = new MySqlCommand(cmdText);
      var table = new DataTable();
      using (var cn = GetOpenConnection())
      {
        cmd.Connection = cn;
        var adapter = new MySqlDataAdapter(cmd);
        adapter.Fill(table);
      }
      return table.Rows.Cast<DataRow>()
        .Select(r => new KeyValuePair<int, string>(Convert.ToInt32(r[columnName]),
          r["stateId"] as string));
    }

    private static IEnumerable<int> GetOfficeIds()
    {
      const string cmdText = "SELECT officeId FROM offices";
      var cmd = new MySqlCommand(cmdText);
      var table = new DataTable();
      using (var cn = GetOpenConnection())
      {
        cmd.Connection = cn;
        var adapter = new MySqlDataAdapter(cmd);
        adapter.Fill(table);
      }
      return table.Rows.Cast<DataRow>()
        .Select(r => Convert.ToInt32(r["officeId"]));
    }

    //private static Dictionary<int, string> GetCandidateDictionary()
    //{
    //  const string cmdText = "SELECT candidateId FROM bio_candidate_raw";
    //  var cmd = new MySqlCommand(cmdText);
    //  var table = new DataTable();
    //  using (var cn = GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    var adapter = new MySqlDataAdapter(cmd);
    //    adapter.Fill(table);
    //  }
    //  return table.Rows.Cast<DataRow>()
    //    .ToDictionary(r => Convert.ToInt32(r["candidateId"]), r => null as string);
    //}

    private static Dictionary<string, string> GetReverseStageDictionary()
    {
      const string cmdText = "SELECT stageId,stage FROM stages";
      var cmd = new MySqlCommand(cmdText);
      var table = new DataTable();
      using (var cn = GetOpenConnection())
      {
        cmd.Connection = cn;
        var adapter = new MySqlDataAdapter(cmd);
        adapter.Fill(table);
      }
      return table.Rows.Cast<DataRow>()
        .ToDictionary(r => r["stage"] as string, r => r["stageId"] as string);
    }

    //private static Dictionary<string, string> GetStageDictionary()
    //{
    //  const string cmdText = "SELECT stageId,stage FROM stages";
    //  var cmd = new MySqlCommand(cmdText);
    //  var table = new DataTable();
    //  using (var cn = GetOpenConnection())
    //  {
    //    cmd.Connection = cn;
    //    var adapter = new MySqlDataAdapter(cmd);
    //    adapter.Fill(table);
    //  }
    //  return table.Rows.Cast<DataRow>()
    //    .ToDictionary(r => r["stageId"] as string, r => r["stage"] as string);
    //}
  }
}
