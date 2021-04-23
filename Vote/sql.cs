namespace Vote
{
  /// <summary>
  /// Summary description for sql.
  /// </summary>
  public static class Sql
  {

    public static string SqlLit(string str)
    {
      //Enclose string in single quotes and double up any embededded single quotes
      str = "'" + str.Replace("'", "''") + "'";
      return str;
    }
  }
}