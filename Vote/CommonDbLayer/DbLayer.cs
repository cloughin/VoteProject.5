using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Vote
{
  public enum DbProviders
  {
    MsSql,
    MySql
  }

  public abstract class Db
  {
    #region from db

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }

    #endregion from db

    private const string VoteDbProviderKey = "VoteDbProvider";

    private static Db _Current;

    private string _ConnectionString;

    protected abstract void AddCommandParameter(DbCommand command, string name,
      object value);

    //public abstract void _Bulk_Insert(DataTable dataTable, string DataTable_Name);

    //public static void Bulk_Insert(string connectionString, ref DataTable dataTable,
    //  string DataTable_Name)
    //{
    //  Current._Bulk_Insert(dataTable, DataTable_Name);
    //}

    protected abstract DbCommand Command(string sql, DbConnection connection);

    protected abstract DbConnection Connection { get; }

    protected string ConnectionString => 
      _ConnectionString ?? (_ConnectionString = ConfigurationManager.AppSettings[ConnectionStringKey]);

    protected abstract string ConnectionStringKey { get; }

    private static Db Current
    {
      get
      {
        if (_Current == null) Initialize();
        return _Current;
      }
    }

    protected abstract DbDataAdapter DataAdapter(DbCommand command);

    public static string DbDateTime(DateTime dateTime) => 
      dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

    //public static string DbDateTime_Short(DateTime dateTime)
    //{
    //  return dateTime.ToString("yyyy-MM-dd");
    //}

    //public static string DbDateTime_Long(DateTime dateTime)
    //{
    //  return dateTime.ToString("MMMM d, yyyy");
    //}

    //public static string DbDateTime_Long_Or_Empty(DateTime dateTime)
    //{
    //  if (DbDateTime_Long(dateTime) == "January 1, 1900")
    //    return string.Empty;
    //  else
    //    return DbDateTime_Long(dateTime);
    //}

    //public static string DbNow { get { return DbDateTime(DateTime.UtcNow); } }

    public static string DbToday => DbDateTime(DateTime.Today);

    private static DbProviders DbProvider
    {
      get
      {
        DbProviders result;
        var configured = ConfigurationManager.AppSettings[VoteDbProviderKey];
        try
        {
          result = (DbProviders) Enum.Parse(typeof (DbProviders), configured);
        }
        catch
        {
          throw new ApplicationException("App setting for " + VoteDbProviderKey +
            " not found.");
        }
        return result;
      }
    }

    private static string EncloseName(string name) => 
      Current._EncloseName(name);

    protected abstract string _EncloseName(string name);

    public static void ExecuteSql(string sql)
    {
      using (var cn = Current.Connection)
      {
        cn.Open();
        var cmd = Current.Command(sql, cn);
        cmd.Connection = cn;
        cmd.CommandTimeout = 300;
        cmd.ExecuteNonQuery();
      }
    }

    //public static int ExecuteSQLInt(string SQL)
    //{
    //  using (DbConnection cn = Current.Connection)
    //  {
    //    cn.Open();
    //    DbCommand cmd = Current.Command(SQL, cn);
    //    cmd.Connection = cn;
    //    cmd.CommandTimeout = 300;
    //    return (int) cmd.ExecuteNonQuery();
    //  }
    //}

    private static void Initialize() => Initialize(DbProvider);

    private static void Initialize(DbProviders provider)
    {
      switch (provider)
      {
        case DbProviders.MsSql:
          _Current = new MsSqlDb();
          break;

        case DbProviders.MySql:
          _Current = new MySqlDb();
          break;
      }
    }

    //public static void LogPoliticiansImagesOriginal_Insert(string politicianKey,
    //  byte[] imageBlob, DateTime uploadTime)
    //{
    //  LogPoliticiansImagesOriginal.Insert(politicianKey: politicianKey,
    //    profileOriginal: imageBlob, profileOriginalDate: uploadTime,
    //    userSecurity: db.User_Security(), userName: db.User_Name(),
    //    commandTimeout: 3600);
    //}

    //public static void LogPoliticiansHeadshots_Insert(string politicianKey,
    //  byte[] imageBlob, DateTime uploadTime)
    //{
    //  DB.VoteLog.LogPoliticiansImagesHeadshot.Insert(
    //    politicianKey: politicianKey, headshotOriginal: imageBlob,
    //    headshotDate: uploadTime, userSecurity: db.User_Security(),
    //    userName: db.User_Name(), commandTimeout: 3600);
    //}

    public static int Rows(string table, string keyName, string keyValue)
    {
      using (var cn = Current.Connection)
      {
        cn.Open();
        var sql = "SELECT Count(*) FROM " + table + " WHERE " + keyName + " = " +
          SqlLit(keyValue);
        var cmd = Current.Command(sql, cn);
        //return (int)cmd.ExecuteScalar();
        return Convert.ToInt32(cmd.ExecuteScalar());
      }
    }

    //public static int Rows(string Table, string KeyName, int KeyValue)
    //{
    //  using (DbConnection cn = Current.Connection)
    //  {
    //    cn.Open();
    //    string SQL = "SELECT Count(*) FROM " + Table
    //      + " WHERE " + KeyName + " = " + KeyValue.ToString();
    //    DbCommand cmd = Current.Command(SQL, cn);
    //    //return (int)cmd.ExecuteScalar();
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    //public static int Rows(string table, string keyName1, string keyValue1,
    //  string keyName2, string keyValue2)
    //{
    //  using (var cn = Current.Connection)
    //  {
    //    cn.Open();
    //    var sql = "SELECT Count(*) FROM " + table + " WHERE " + keyName1 + " = " +
    //      SqlLit(keyValue1) + " AND " + keyName2 + " = " + SqlLit(keyValue2);
    //    var cmd = Current.Command(sql, cn);
    //    //return (int)cmd.ExecuteScalar();
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    //public static int Rows(string Table, string KeyName1, int KeyValue1, string KeyName2, string KeyValue2)
    //{
    //  using (DbConnection cn = Current.Connection)
    //  {
    //    cn.Open();
    //    string SQL = "SELECT Count(*) FROM " + Table
    //      + " WHERE " + KeyName1 + " = " + KeyValue1.ToString()
    //      + " AND " + KeyName2 + " = " + db.SQLLit(KeyValue2);
    //    DbCommand cmd = Current.Command(SQL, cn);
    //    //return (int)cmd.ExecuteScalar();
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    public static int Rows(string table, string keyName1, string keyValue1,
      string keyName2, string keyValue2, string keyName3, string keyValue3)
    {
      using (var cn = Current.Connection)
      {
        cn.Open();
        var sql = "SELECT Count(*) FROM " + table + " WHERE " + keyName1 + " = " +
          SqlLit(keyValue1) + " AND " + keyName2 + " = " + SqlLit(keyValue2) +
          " AND " + keyName3 + " = " + SqlLit(keyValue3);
        var cmd = Current.Command(sql, cn);
        //return (int)cmd.ExecuteScalar();
        return Convert.ToInt32(cmd.ExecuteScalar());
      }
    }

    //public static int Rows(
    //  string Table
    //  , string KeyName1
    //  , string KeyValue1
    //  , string KeyName2
    //  , string KeyValue2
    //  , string KeyName3
    //  , string KeyValue3
    //  , string KeyName4
    //  , string KeyValue4
    //  )
    //{
    //  using (DbConnection cn = Current.Connection)
    //  {
    //    cn.Open();
    //    string SQL = "SELECT Count(*) FROM " + Table
    //      + " WHERE " + KeyName1 + " = " + db.SQLLit(KeyValue1)
    //      + " AND " + KeyName2 + " = " + db.SQLLit(KeyValue2)
    //      + " AND " + KeyName3 + " = " + db.SQLLit(KeyValue3
    //      + " AND " + KeyName4 + " = " + db.SQLLit(KeyValue4)
    //      );
    //    DbCommand cmd = Current.Command(SQL, cn);
    //    //return (int)cmd.ExecuteScalar();
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    //public static int Rows(string Table, string KeyName1, int KeyValue1, string KeyName2, string KeyValue2, string KeyName3, string KeyValue3)
    //{
    //  using (DbConnection cn = Current.Connection)
    //  {
    //    cn.Open();
    //    string SQL = "SELECT Count(*) FROM " + Table
    //      + " WHERE " + KeyName1 + " = " + KeyValue1.ToString()
    //      + " AND " + KeyName2 + " = " + db.SQLLit(KeyValue2)
    //      + " AND " + KeyName3 + " = " + db.SQLLit(KeyValue3);
    //    DbCommand cmd = Current.Command(SQL, cn);
    //    //return (int)cmd.ExecuteScalar();
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    //public static int Rows(
    //  string Table
    //  , string KeyName1
    //  , string KeyValue1
    //  , string KeyName2
    //  , string KeyValue2
    //  , string KeyName3
    //  , string KeyValue3
    //  , string KeyName4
    //  , string KeyValue4
    //  , string KeyName5
    //  , string KeyValue5
    //  )
    //{
    //  using (DbConnection cn = Current.Connection)
    //  {
    //    cn.Open();
    //    string SQL = "SELECT Count(*) FROM " + Table
    //      + " WHERE " + KeyName1 + " = " + db.SQLLit(KeyValue1)
    //      + " AND " + KeyName2 + " = " + db.SQLLit(KeyValue2)
    //      + " AND " + KeyName3 + " = " + db.SQLLit(KeyValue3)
    //      + " AND " + KeyName4 + " = " + db.SQLLit(KeyValue4)
    //      + " AND " + KeyName5 + " = " + db.SQLLit(KeyValue5);
    //    DbCommand cmd = Current.Command(SQL, cn);
    //    //return (int)cmd.ExecuteScalar();
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    //public static int Rows(string Table, string KeyName1, int KeyValue1,
    //  string KeyName2, string KeyValue2, string KeyName3, string KeyValue3,
    //  string KeyName4, string KeyValue4)
    //{
    //  using (var cn = Current.Connection)
    //  {
    //    cn.Open();
    //    var SQL = "SELECT Count(*) FROM " + Table + " WHERE " + KeyName1 + " = " +
    //      KeyValue1.ToString() + " AND " + KeyName2 + " = " + db.SQLLit(KeyValue2) +
    //      " AND " + KeyName3 + " = " + db.SQLLit(KeyValue3) + " AND " + KeyName4 +
    //      " = " + db.SQLLit(KeyValue4);
    //    var cmd = Current.Command(SQL, cn);
    //    //return (int)cmd.ExecuteScalar();
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    //public static int Rows_Count(string SQL)
    //{
    //  using (var cn = Current.Connection)
    //  {
    //    cn.Open();
    //    SQL = "SELECT Count(*) " + SQL;
    //    var cmd = Current.Command(SQL, cn);
    //    //return (int)cmd.ExecuteScalar();
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    public static int Rows_Count_From(string sql)
    {
      using (var cn = Current.Connection)
      {
        cn.Open();
        sql = "SELECT Count(*) FROM " + sql;
        var cmd = Current.Command(sql, cn);
        //return (int)cmd.ExecuteScalar();
        return Convert.ToInt32(cmd.ExecuteScalar());
      }
    }

    //public static int Rows_Table(string Table)
    //{
    //  using (var cn = Current.Connection)
    //  {
    //    cn.Open();
    //    var SQL = "SELECT Count(*) FROM " + Table;
    //    var cmd = Current.Command(SQL, cn);
    //    //return (int)cmd.ExecuteScalar();
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    //public static DataSet Set(string SQL)//Using DataReader
    //{
    //  using (DbConnection cn = Current.Connection)
    //  {
    //    cn.Open();
    //    DataSet Dataset = new DataSet();

    //    DbCommand cmd = Current.Command(SQL, cn);
    //    cmd.CommandTimeout = 3600;

    //    DbDataAdapter adapter = Current.DataAdapter(cmd);
    //    adapter.Fill(Dataset);
    //    return Dataset;
    //  }
    //}

    //public static void Single_Key_Update_Image(
    //  string Table
    //  , string Column
    //  , byte[] Image_Blob
    //  , string KeyName
    //  , string KeyValue
    //  )
    //{
    //  string UpdateSQL = "UPDATE " + Table
    //    + " SET " + Column + " = @" + Column
    //    + " WHERE " + KeyName + " = " + db.SQLLit(KeyValue);
    //  using (DbConnection cn = Current.Connection)
    //  {
    //    cn.Open();
    //    DbCommand cmd = Current.Command(UpdateSQL, cn);
    //    cmd.CommandTimeout = 3600;

    //    Current.AddCommandParameter(cmd, Column, Image_Blob);

    //    cmd.ExecuteNonQuery();
    //  }
    //}

    public static DataTable Table(string sql)
    {
      using (var cn = Current.Connection)
      {
        cn.Open();
        var table = new DataTable("AnyTable");

        var cmd = Current.Command(sql, cn);
        cmd.CommandTimeout = 3600;

        var adapter = Current.DataAdapter(cmd);
        adapter.Fill(table);
        return table;
      }
    }

    // handles any data type and nulls
    public static void UpdateColumnByKey(string table, string dataColumn,
      object dataValue, string keyColumn, object keyValue)
    {
      var sql =
        $"UPDATE {EncloseName(table)} SET {EncloseName(dataColumn)}=@p1" +
        $" WHERE {EncloseName(keyColumn)}=@p2";

      using (var cn = Current.Connection)
      {
        cn.Open();
        var cmd = Current.Command(sql, cn);
        cmd.CommandTimeout = 3600;

        Current.AddCommandParameter(cmd, "p1", dataValue);
        Current.AddCommandParameter(cmd, "p2", keyValue);

        cmd.ExecuteNonQuery();
      }
    }

    //public static void UpdateColumnsByKey(string table,
    //  Dictionary<string, object> values,
    //  string keyColumn, object keyValue)
    //{
    //  int n = 0;
    //  string setList = string.Join(",", values.Keys.Select(
    //    columnName =>
    //    {
    //      return EncloseName(columnName) + "=@p" + (++n).ToString();
    //    }));

    //  string sql = string.Format("UPDATE {0} SET {1} WHERE {2}=@key",
    //    EncloseName(table), setList, EncloseName(keyColumn));

    //  using (DbConnection cn = Current.Connection)
    //  {
    //    cn.Open();
    //    DbCommand cmd = Current.Command(sql, cn);
    //    cmd.CommandTimeout = 3600;

    //    n = 0;
    //    foreach (object value in values.Values)
    //      Current.AddCommandParameter(cmd, "p" + (++n).ToString(), value);
    //    Current.AddCommandParameter(cmd, "key", keyValue);

    //    cmd.ExecuteNonQuery();
    //  }
    //}
  }
}