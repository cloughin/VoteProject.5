using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static System.String;

namespace GenerateDbClasses
{
  class MsSqlProvider : Provider
  {
    public override IList<ColumnInfo> GetColumnInfo(string tableName, bool allColumnsNullable)
    {
      var result = new List<ColumnInfo>();

      using (var cn = GetMsSqlConnection())
      {
        var table = new DataTable("ColumnList");
        var command = new SqlCommand("sp_columns " + tableName, cn);
        var adapter = new SqlDataAdapter(command);
        adapter.Fill(table);

        foreach (DataRow row in table.Rows)
        {
          var columnName = row[3] as string;
          var sqlType = row[5] as string; // "int identity"
          var isNullable = false;
          if (row[10] is bool b)
            isNullable = b;
          isNullable |= allColumnsNullable;
          var isAutoIncrement = false;
          if (sqlType?.EndsWith(" identity") == true)
          {
            isAutoIncrement = true;
            sqlType = sqlType.Substring(0, sqlType.Length - 9); // remove " identity"
          }
          // "autoIncrement" for uniqueidentifier
          //if ((row[12] as string) == "(newid())")
          //  isAutoIncrement = true;
          var precision = -1;
          if (row[6] is int i)
            precision = i;
          var type = SqlTypeToDotNetType(sqlType, precision, out var maxSize);
          result.Add(new ColumnInfo(columnName, type, maxSize, isNullable,
            isAutoIncrement));
        }
      }

      return result.AsReadOnly();
    }

    public override System.Data.Common.DbConnection GetConnection()
    {
      return GetMsSqlConnection();
    }

    public override IList<IndexInfo> GetIndexInfo(string tableName)
    {
      var result = new List<IndexInfo>();

      using (var cn = GetMsSqlConnection())
      {
        var cmdText =
          "SELECT" +
          "  ind.name AS indexName," +
          "  col.name AS columnName," +
          "  ind.is_unique AS isUnique," +
          "  ind.is_primary_key AS isPrimary" +
          " FROM sys.indexes ind" +
          "  INNER JOIN sys.index_columns ic" +
          "   ON ind.object_id = ic.object_id" +
          "    AND ind.index_id = ic.index_id" +
          "  INNER JOIN sys.columns col" +
          "   ON ic.object_id = col.object_id" +
          "    AND ic.column_id = col.column_id" +
          "  INNER JOIN sys.tables t" +
          "   ON ind.object_id = t.object_id" +
          " WHERE" +
          "  t.name='{0}' AND" +
          //"  ind.is_unique_constraint = 0 AND" +
          "  t.is_ms_shipped = 0" +
          " ORDER BY t.name, ind.name, ind.index_id, ic.index_column_id";
        var table = new DataTable("IndexList");
        cmdText = Format(cmdText, tableName);
        var command = new SqlCommand(cmdText, cn);
        var adapter = new SqlDataAdapter(command);
        adapter.Fill(table);

        IndexInfo iiCurrent = null;
        string keyNameCurrent = null;

        foreach (DataRow row in table.Rows)
        {
          var keyName = row[0] as string;
          var isPrimary = (bool) row[3];
          var isUnique = (bool) row[2];
          if (keyName != keyNameCurrent)
          {
            iiCurrent = new IndexInfo(isPrimary, isUnique);
            result.Add(iiCurrent);
            keyNameCurrent = keyName;
          }
          iiCurrent?.AddColumn(row[1] as string);
        }
      }

      return FilterUniqueIndexes(result);
    }

    private static SqlConnection GetMsSqlConnection()
    {
      var connection = new SqlConnection(ConnectionString);
      connection.Open();
      return connection;
    }

    public override IEnumerable<TableInfo> GetTableInfo()
    {
      var result = new List<TableInfo>();

      using (var cn = GetMsSqlConnection())
      {
        var table = new DataTable("TableList");
        var command = new SqlCommand(
          "SELECT TABLE_NAME, TABLE_TYPE" + 
          " FROM INFORMATION_SCHEMA.Tables" + 
          " ORDER BY TABLE_TYPE, TABLE_NAME", 
          cn);
        var adapter = new SqlDataAdapter(command);
        adapter.Fill(table);

        foreach (DataRow row in table.Rows)
          result.Add(new TableInfo(row[0] as string,
            row[1] as string == "VIEW"));
      }

      return result.AsReadOnly();
    }

    public override string ProviderName
    {
      get { return "MsSql"; }
    }

    private static Type SqlTypeToDotNetType(string sqlType, int precision, out int maxSize)
    {
      Type result;
      maxSize = -1;

      switch (sqlType)
      {
        case "char":
        case "nchar":
        case "text":
        case "ntext":
        case "varchar":
        case "nvarchar":
          result = typeof(string);
          maxSize = precision;
          break;

        case "datetime":
          result = typeof(DateTime);
          break;

        case "time":
          result = typeof(TimeSpan);
          break;

        case "bit":
          result = typeof(bool);
          break;

        case "int":
          result = typeof(int);
          break;

        case "image":
          result = typeof(byte[]);
          break;

        case "uniqueidentifier":
          result = typeof(Guid);
          break;

        case "decimal":
          result = typeof(decimal);
          break;

        default:
          throw new ApplicationException("Unknown MySql data type '" + sqlType + "'");
      }

      return result;
    }
  }
}
