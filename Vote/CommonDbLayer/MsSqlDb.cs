using System.Data.Common;
using System.Data.SqlClient;

namespace Vote
{
  public class MsSqlDb : Db
  {
    protected override void AddCommandParameter(DbCommand command, string name, object value)
    {
      if (name[0] != '@') name = '@' + name;
      (command as SqlCommand)?.Parameters.AddWithValue(name, value);
    }

    //public override void _Bulk_Insert(
    //  DataTable dataTable
    //  , string DataTable_Name
    //  )
    //{

    //  using (SqlBulkCopy bulkCopy =

    //    new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.TableLock))
    //  {

    //    //bulkCopy.DestinationTableName = dataTable.TableName;
    //    bulkCopy.DestinationTableName = DataTable_Name;

    //    bulkCopy.BulkCopyTimeout = 600; // 10 minutes

    //    bulkCopy.WriteToServer(dataTable);

    //    dataTable.Clear();

    //  }

    //}

    protected override DbCommand Command(string sql, DbConnection connection) => 
      new SqlCommand(sql, connection as SqlConnection);

    protected override DbConnection Connection => 
      new SqlConnection(ConnectionString);

    protected override string ConnectionStringKey => "CnVote.MsSql";

    protected override DbDataAdapter DataAdapter(DbCommand command) => new SqlDataAdapter(command as SqlCommand);

    protected override string _EncloseName(string name)
    {
      if (name[0] != '[') name = '[' + name + ']';
      return name;
    }
  }
}