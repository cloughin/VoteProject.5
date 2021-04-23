using System;
using System.Data;
using System.Data.Common;
using System.Web.Configuration;

namespace Vote
{
  public abstract class Db
  {
    #region from db

    //public static string SqlLit(string str)
    //{
    //  //Enclose string in single quotes and double up any embededded single quotes
    //  str = "'" + str.Replace("'", "''") + "'";
    //  return str;
    //}

    #endregion from db

    //private static Db _Current;

    //private string _ConnectionString;

    //protected abstract void AddCommandParameter(DbCommand command, string name,
    //  object value);

    //protected abstract DbCommand Command(string sql, DbConnection connection);

    //protected abstract DbConnection Connection { get; }

    //protected string ConnectionString => 
    //  _ConnectionString ?? (_ConnectionString = WebConfigurationManager.AppSettings[ConnectionStringKey]);

    //protected abstract string ConnectionStringKey { get; }

    //private static Db Current
    //{
    //  get
    //  {
    //    if (_Current == null) Initialize();
    //    return _Current;
    //  }
    //}

    //protected abstract DbDataAdapter DataAdapter(DbCommand command);

    //public static string DbDateTime(DateTime dateTime) => 
    //  dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

    //public static string DbToday => DbDateTime(DateTime.Today);

    //private static string EncloseName(string name) =>
    //  Current._EncloseName(name);

    //protected abstract string _EncloseName(string name);

    //public static void ExecuteSql(string sql)
    //{
    //  using (var cn = Current.Connection)
    //  {
    //    cn.Open();
    //    var cmd = Current.Command(sql, cn);
    //    cmd.Connection = cn;
    //    cmd.CommandTimeout = 300;
    //    cmd.ExecuteNonQuery();
    //  }
    //}

    //private static void Initialize()
    //{
    //  _Current = new MySqlDb();
    //}

    //public static int Rows(string table, string keyName, string keyValue)
    //{
    //  using (var cn = Current.Connection)
    //  {
    //    cn.Open();
    //    var sql = "SELECT Count(*) FROM " + table + " WHERE " + keyName + " = " +
    //      SqlLit(keyValue);
    //    var cmd = Current.Command(sql, cn);
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    //public static int Rows_Count_From(string sql)
    //{
    //  using (var cn = Current.Connection)
    //  {
    //    cn.Open();
    //    sql = "SELECT Count(*) FROM " + sql;
    //    var cmd = Current.Command(sql, cn);
    //    return Convert.ToInt32(cmd.ExecuteScalar());
    //  }
    //}

    //public static DataTable Table(string sql)
    //{
    //  using (var cn = Current.Connection)
    //  {
    //    cn.Open();
    //    var table = new DataTable("AnyTable");

    //    var cmd = Current.Command(sql, cn);
    //    cmd.CommandTimeout = 3600;

    //    var adapter = Current.DataAdapter(cmd);
    //    adapter.Fill(table);
    //    return table;
    //  }
    //}

    //// handles any data type and nulls
    //public static void UpdateColumnByKey(string table, string dataColumn,
    //  object dataValue, string keyColumn, object keyValue)
    //{
    //  var sql =
    //    $"UPDATE {EncloseName(table)} SET {EncloseName(dataColumn)}=@p1" +
    //    $" WHERE {EncloseName(keyColumn)}=@p2";

    //  using (var cn = Current.Connection)
    //  {
    //    cn.Open();
    //    var cmd = Current.Command(sql, cn);
    //    cmd.CommandTimeout = 3600;

    //    Current.AddCommandParameter(cmd, "p1", dataValue);
    //    Current.AddCommandParameter(cmd, "p2", keyValue);

    //    cmd.ExecuteNonQuery();
    //  }
    //}
  }
}