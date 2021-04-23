using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Vote
{
  public class MySqlDb : Db
  {
    protected override void AddCommandParameter(DbCommand command, string name, object value)
    {
      if (name[0] != '@') name = '@' + name;
      (command as MySqlCommand)?.Parameters.AddWithValue(name, value);
    }

    //public override void _Bulk_Insert(
    //  DataTable dataTable
    //  , string DataTable_Name
    //  )
    //{
    //  throw new ApplicationException("Bulk_Insert not yet supported in MySql");
    //}

    protected override DbCommand Command(string sql, DbConnection connection) => 
      new MySqlCommand(sql, connection as MySqlConnection);

    protected override DbConnection Connection
    {
      get
      {
        //Console.WriteLine("Enter MySqlDb.Connection");
        var connectionString = ConnectionString;
        //Console.WriteLine("ConnectionString=\"{0}\"", connectionString);
        DbConnection connection = new MySqlConnection(connectionString);
        //Console.WriteLine("Exit MySqlDb.Connection");
        return connection;
      }
    }

    protected override string ConnectionStringKey => "CnVote.MySql";

    protected override DbDataAdapter DataAdapter(DbCommand command) => 
      new MySqlDataAdapter(command as MySqlCommand);

    protected override string _EncloseName(string name)
    {
      if (name[0] != '`') name = '`' + name + '`';
      return name;
    }
  }
}