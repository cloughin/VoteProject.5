using System;
using System.Collections.Generic;
using System.Data;
using System.Security;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace GenerateDbClasses
{
  class MySqlProvider : Provider
  {
    public override IList<ColumnInfo> GetColumnInfo(string tableName, bool allColumnsNullable)
    {
      var result = new List<ColumnInfo>();

      using (var cn = GetMySqlConnection())
      {
        var table = new DataTable("ColumnList");
        var command = new MySqlCommand("SHOW COLUMNS FROM " + tableName, cn);
        var adapter = new MySqlDataAdapter(command);
        adapter.Fill(table);

        foreach (DataRow row in table.Rows)
        {
          var columnName = row[0] as string;
          var sqlType = row[1] as string;
          var isNullable = row[2] as string == "YES" || allColumnsNullable;
          var isAutoIncrement = row[5] as string == "auto_increment";
          var type = SqlTypeToDotNetType(sqlType, out var maxSize);
          result.Add(new ColumnInfo(columnName, type, maxSize, isNullable,
            isAutoIncrement));
        }
      }

      return result.AsReadOnly();
    }

    public override System.Data.Common.DbConnection GetConnection()
    {
      return GetMySqlConnection();
    }

    public override IList<IndexInfo> GetIndexInfo(string tableName)
    {
      var result = new List<IndexInfo>();

      using (var cn = GetMySqlConnection())
      {
        var table = new DataTable("IndexList");
        var command = new MySqlCommand("SHOW INDEXES FROM " + tableName, cn);
        var adapter = new MySqlDataAdapter(command);
        adapter.Fill(table);

        IndexInfo iiCurrent = null;
        string keyNameCurrent = null;

        foreach (DataRow row in table.Rows)
        {
          var keyName = row[2] as string;
          var isPrimary = keyName == "PRIMARY";
          //var isUnique = (long)row[1] == 0;
          var isUnique = Convert.ToInt64(row[1]) == 0;
          if (keyName != keyNameCurrent)
          {
            iiCurrent = new IndexInfo(isPrimary, isUnique);
            result.Add(iiCurrent);
            keyNameCurrent = keyName;
          }
          iiCurrent?.AddColumn(row[4] as string);
        }
      }

      // Use GroupBy to get Distinct entries (so we can use lambda)
      // Duplicate keys would generate compile errors
      //return result.GroupBy(
      //  i => Join(Empty, i.Columns),
      //  (key, group) => group.First())
      //  .ToList()
      //  .AsReadOnly();
      return FilterUniqueIndexes(result);
    }

    private static MySqlConnection GetMySqlConnection()
    {
      var connection = new MySqlConnection(ConnectionString);
      connection.Open();
      return connection;
    }

    public override IEnumerable<TableInfo> GetTableInfo()
    {
      var result = new List<TableInfo>();

      using (var cn = GetMySqlConnection())
      {
        var table = new DataTable("TableList");
        var command = new MySqlCommand("SHOW FULL TABLES", cn);
        var adapter = new MySqlDataAdapter(command);
        adapter.Fill(table);
 
        foreach (DataRow row in table.Rows)
          result.Add(new TableInfo(row[0] as string, 
            row[1] as string == "VIEW"));
      }

      return result.AsReadOnly();
    }

    private static readonly Regex SqlTypeToDotNetTypeRegex =
      new Regex(@"^(?<type>[a-z]+)(?:\((?<size>\d+)(?:,(?<decimals>\d+))?\))?$");
    private static Type SqlTypeToDotNetType(string sqlType, out int maxSize)
    {
      Type result;
      maxSize = -1;

      var match = SqlTypeToDotNetTypeRegex.Match(sqlType);
      if (!match.Success)
        throw new ApplicationException("Unknown MySql data type '" + sqlType + "'");
      var typeName = match.Groups["type"].Captures[0].Value;
      var size = -1;
      var sizeCaptures = match.Groups["size"].Captures;
      if (sizeCaptures.Count != 0)
        int.TryParse(sizeCaptures[0].Value, out size);

      switch (typeName)
      {
        case "char":
        case "longtext":
        case "text":
        case "varchar":
          result = typeof(string);
          maxSize = size;
          break;

        case "date":
        case "datetime":
          result = typeof(DateTime);
          break;

        case "time":
          result = typeof(TimeSpan);
          break;

        case "tinyint":
          result = size == 1
            ? typeof(bool)
            : typeof(byte);
          break;

        case "smallint":
          result = typeof(short);
          break;

        case "int":
          result = typeof(int);
          break;

        case "bigint":
          result = typeof(long);
          break;

        case "longblob":
          result = typeof(byte[]);
          break;

        case "decimal":
          result = typeof(decimal);
          break;

        case "double":
          result = typeof(double);
          break;

        default:
          throw new ApplicationException("Unknown MySql data type '" + sqlType + "'");
      }

      return result;
    }

    public override string ProviderName
    {
      get { return "MySql"; }
    }
  }
}
