using System.Data;

namespace Vote
{
// ReSharper disable InconsistentNaming
  public partial class G // ReSharper restore InconsistentNaming
  {
    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    public static void ExecuteSql(string sql) => Db.ExecuteSql(sql);

    public static DataTable Table(string sql) => Db.Table(sql);

    public static int Rows_Count_From(string sql) => Db.Rows_Count_From(sql);

    public static int Rows(string table, string keyName, string keyValue) => 
      Db.Rows(table, keyName, keyValue);

    #endregion Public
  }
}