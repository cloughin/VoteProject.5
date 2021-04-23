using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Web;
using DB;
using DB.Vote;
using MySql.Data.MySqlClient;
using Vote;
using static System.String;

namespace TestDataScienceToolkit
{
  static class Program
  {
    private static DataTable GetAddresses()
    {
      const string cmdText = "SELECT Id,Address,City,StateCode FROM Addresses" +
        " WHERE NOT Latitude IS NULL AND Latitude!=0 AND" +
        " NOT Longitude IS NULL AND Longitude!=0 AND" + 
        " Address != '' AND City != '' AND StateCode != ''";
      var cmd = VoteDb.GetCommand(cmdText, -1);
      using (var cn = VoteDb.GetOpenConnection())
      {
        cmd.Connection = cn;
        var table = new DataTable();
        var adapter = new MySqlDataAdapter(cmd as MySqlCommand);
        adapter.Fill(table);
        return table;
      }
    }

    private static void Main(string[] args)
    {
      var good = 0;
      var bad = 0;
      foreach (var row in GetAddresses().Rows.Cast<DataRow>())
      {
        var address = $"{row.Address()} {row.City()} {row.StateCode()}";
        var url =
          $"http://ec2-35-171-163-158.compute-1.amazonaws.com/street2coordinates/{HttpUtility.UrlEncode(address)}";
        try
        {
          var result = new WebClient().DownloadString(url);
          if (result.IndexOf(": null", StringComparison.Ordinal) >= 0)
            throw new Exception();
          good++;
        }
        catch
        {
          bad++;
          Console.WriteLine(address);
        }
      }

      Console.ReadLine();
    }
  }
}
