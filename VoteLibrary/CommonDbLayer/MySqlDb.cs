using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Vote
{
  public class MySqlDb : Db
  {
    //protected override void AddCommandParameter(DbCommand command, string name, object value)
    //{
    //  if (name[0] != '@') name = '@' + name;
    //  (command as MySqlCommand)?.Parameters.AddWithValue(name, value);
    //}

    //protected override DbCommand Command(string sql, DbConnection connection) => 
    //  new MySqlCommand(sql, connection as MySqlConnection);

    //protected override DbConnection Connection
    //{
    //  get
    //  {
    //    var connectionString = ConnectionString;
    //    DbConnection connection = new MySqlConnection(connectionString);
    //    return connection;
    //  }
    //}

    //protected override string ConnectionStringKey => "CnVote.MySql";

    //protected override DbDataAdapter DataAdapter(DbCommand command) => 
    //  new MySqlDataAdapter(command as MySqlCommand);

    //protected override string _EncloseName(string name)
    //{
    //  if (name[0] != '`') name = '`' + name + '`';
    //  return name;
    //}
  }
}