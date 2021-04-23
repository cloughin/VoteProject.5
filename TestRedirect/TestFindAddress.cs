using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Vote;
using DB.Vote;
using System.Data.Common;

namespace TestRedirect
{
  public partial class TestFindAddress : Form
  {
    public TestFindAddress()
    {
      InitializeComponent();

      //CheckZipCitiesDownloaded();
      //CheckZipStreetsDownloaded();
      //CheckZipStreetsDownloadedAllNumeric();
      //CheckZipStreetsDownloadedStreetNameSpaces();
      //CheckZipStreetsDownloadedStreetNameNonAlpha();
      //CheckZipStreetsDownloadedAddressPrimaryLowNumberNonAlpha();
      //CheckZipStreetsDownloadedAddressPrimaryLowNumberBadLength();
      //CheckZipStreetsDownloadedOddEven();
      //CheckAgainstZipAddressesTesting();
      //CheckAliasAbbreviations();
      //CheckHouseNumberLength();
      AddressDashes();
    }

    private void AddressDashes()
    {
      int recordCount = 0;
      using (var reader = DB.VoteZipNew.ZipStreetsDownloaded.GetAllAnalysisDataReader(0))
        while (reader.Read())
        {
          recordCount++;
          if (reader.PrimaryLowNumber == "-" && reader.PrimaryHighNumber == "-")
          {
            var table = DB.VoteZipNew.ZipStreetsDownloaded.GetAnalysisDataByFullStreetName(
              reader.ZipCode, reader.DirectionPrefix, reader.StreetName,
              reader.StreetSuffix, reader.DirectionSuffix);
            foreach (var row in table)
            {
              if (row.UpdateKey != reader.UpdateKey && 
                (row.PrimaryLowNumber.Length !=0 || row.PrimaryHighNumber.Length !=0))
                if (row.PrimaryLowNumber.Length != 10 ||
                  !row.PrimaryLowNumber.IsDigits() ||
                  row.PrimaryHighNumber.Length != 10 ||
                  !row.PrimaryHighNumber.IsDigits())
                {
                }
            }
          }
        }
    }

    private void CheckHouseNumberLength()
    {
      int recordCount = 0;
      var list = new List<string>();
      using (var reader = DB.VoteZipNew.ZipStreets.GetAllDataReader(0))
        while (reader.Read())
        {
          recordCount++;
          if (reader.PrimaryLowNumber.Length != reader.PrimaryHighNumber.Length)
            list.Add(reader.UpdateKey);
        }
    }

    public static DB.VoteZipNew.ZipCitiesDownloadedTable GetCityAliasesDataWithAbbreviations(int commandTimeout)
    {
      string cmdText = "SELECT ZipCode,City,State,CityAliasAbbreviation,CityAliasName,PrimaryRecord FROM ZipCitiesDownloaded WHERE CityAliasAbbreviation<>''";
      DbCommand cmd = VoteDb.GetCommand(cmdText, commandTimeout);
      return DB.VoteZipNew.ZipCitiesDownloaded.FillTable(cmd, DB.VoteZipNew.ZipCitiesDownloadedTable.ColumnSet.CityAliases);
    }

    private void CheckAliasAbbreviations()
    {
      var list = new List<string>();
      var table = GetCityAliasesDataWithAbbreviations(0);
      foreach (var row in table)
        if (DB.VoteZipNew.ZipCitiesDownloaded.CountByStateCityAliasName(row.State, row.CityAliasAbbreviation) == 0)
          list.Add(row.CityAliasAbbreviation + ", " + row.State);
    }

    //private void CheckAgainstZipAddressesTesting()
    //{
    //  string sqlText = DB.VoteZip.ZipAddressesTesting.SelectAllCommandText;

    //  int rowCount = 0;

    //  using (DbConnection cn = VoteDb.GetOpenConnection())
    //  {
    //    DbCommand command = VoteDb.GetCommand(sqlText, cn, 0);

    //    using (DbDataReader reader = command.ExecuteReader())
    //    {
    //      int stateCodeOrd = reader.GetOrdinal("StateCode");
    //      int cityOrd = reader.GetOrdinal("City");
    //      int addr1Ord = reader.GetOrdinal("Addr1");
    //      int addr2Ord = reader.GetOrdinal("Addr2");
    //      int zip5Ord = reader.GetOrdinal("Zip5");
    //      int zip4Ord = reader.GetOrdinal("Zip4");

    //      while (reader.Read())
    //      {
    //        string addr1 = reader.GetString(addr1Ord);
    //        string addr2 = reader.GetString(addr2Ord);
    //        string city = reader.GetString(cityOrd);
    //        string stateCode = reader.GetString(stateCodeOrd);
    //        string zip5 = reader.GetString(zip5Ord);
    //        string zip4 = reader.GetString(zip4Ord);

    //        // Try Addr1 + Addr2 + City + StateCode
    //        // Check that result is successful && Zip5-Zip4 is in zip list
    //        string input = addr1 + " " + addr2 + " " + city + " " + stateCode;
    //        var result = AddressFinder.Find(input);
    //        if (result.SuccessMessage == null) // failure
    //        {
    //        }
    //        else
    //        {
    //          string foundMessage = "Found " + zip5 + "-" + zip4;
    //          if (!result.ErrorMessages.Contains(foundMessage))
    //          {
    //          }
    //        }

    //        // Try Addr1 + Addr2 + Zip5
    //        // Check that result is successful && Zip5-Zip4 is in zip list
    //        input = addr1 + " " + addr2 + " " + zip5;
    //        result = AddressFinder.Find(input);
    //        if (result.SuccessMessage == null) // failure
    //        {
    //        }
    //        else
    //        {
    //          string foundMessage = "Found " + zip5 + "-" + zip4;
    //          if (!result.ErrorMessages.Contains(foundMessage))
    //          {
    //          }
    //        }

    //        // Try Zip5-Zip4
    //        // Check that result is successful
    //        input = zip5 + "-" + zip4;
    //        result = AddressFinder.Find(input);
    //        if (result.SuccessMessage == null) // failure
    //        {
    //        }

    //        rowCount++;
    //      }
    //    }
    //  }
    //}

    private void CheckZipStreetsDownloaded()
    {
      var table = DB.VoteZipNew.ZipStreetsDownloaded.GetAllData();
      var streetsList = table.AsEnumerable<DB.VoteZipNew.ZipStreetsDownloadedRow>()
        .GroupBy(row => row.StreetName)
        .Select(group => group.Key)
        .ToList();

      var specialCharStreets = streetsList
        .Where(street => !Regex.Match(street, @"^[ A-Z0-9]+$").Success)
        .ToList();

      int maxSpaces = 0;
      List<string> manySpaces = new List<string>();
      foreach (string street in streetsList)
      {
        Match match = Regex.Match(street, @"^[^ ]*(?:(?<spaces> )[^ ]*)*$");
        int spaces = match.Groups["spaces"].Captures.Count;
        maxSpaces = Math.Max(maxSpaces, spaces);
        if (spaces > 2) manySpaces.Add(street);
      }
    }

    Regex CountSpacesRegex = new Regex(@"^[^ ]*(?:(?<spaces> )[^ ]*)*$");
    private void CheckZipStreetsDownloadedStreetNameSpaces()
    {
      string sqlText = "SELECT StName FROM ZipStreetsDownloaded";

      int rowCount = 0;
      Dictionary<string, object> manySpaces = new Dictionary<string, object>();
      int maxSpaces = 0;

      using (DbConnection cn = VoteDb.GetOpenConnection())
      {
        DbCommand command = VoteDb.GetCommand(sqlText, cn, 0);

        using (DbDataReader reader = command.ExecuteReader())
        {
          int stNameOrd = reader.GetOrdinal("StName");

          while (reader.Read())
          {
            string streetName = reader.GetString(stNameOrd);

            Match match = CountSpacesRegex.Match(streetName);
            int spaces = match.Groups["spaces"].Captures.Count;
            maxSpaces = Math.Max(maxSpaces, spaces);
            if (spaces > 3)
              manySpaces[streetName] = null;

            rowCount++;
          }
        }
      }
    }

    Regex AllAlphaRegex = new Regex(@"^[ A-Z0-9]+$");
    private void CheckZipStreetsDownloadedStreetNameNonAlpha()
    {
      string sqlText = "SELECT StName FROM ZipStreetsDownloaded";

      int rowCount = 0;
      Dictionary<string, object> nonAlpha = new Dictionary<string, object>();

      using (DbConnection cn = VoteDb.GetOpenConnection())
      {
        DbCommand command = VoteDb.GetCommand(sqlText, cn, 0);

        using (DbDataReader reader = command.ExecuteReader())
        {
          int stNameOrd = reader.GetOrdinal("StName");

          while (reader.Read())
          {
            string streetName = reader.GetString(stNameOrd);
            if (!AllAlphaRegex.Match(streetName).Success)
              nonAlpha[streetName] = null;

            rowCount++;
          }
        }
      }
    }

    private void CheckZipStreetsDownloadedAllNumeric()
    {
      string sqlText = "SELECT StName FROM ZipStreetsDownloaded";

      int rowCount = 0;
      Dictionary<string, object> ofInterest = new Dictionary<string, object>();

      using (DbConnection cn = VoteDb.GetOpenConnection())
      {
        DbCommand command = VoteDb.GetCommand(sqlText, cn, 0);

        using (DbDataReader reader = command.ExecuteReader())
        {
          int stNameOrd = reader.GetOrdinal("StName");

          while (reader.Read())
          {
            string streetName = reader.GetString(stNameOrd);
            if (streetName.IsDigits())
              ofInterest[streetName] = null;

            rowCount++;
          }
        }
      }
    }

    class OddEvenInfo
    {
      public string Low;
      public string High;
      public string EvenOdd;
    }

    private void CheckZipStreetsDownloadedOddEven()
    {
      string sqlText = "SELECT AddressPrimaryLowNumber,AddressPrimaryHighNumber,AddressPrimaryEvenOdd FROM ZipStreetsDownloaded";

      int rowCount = 0;
      List<OddEvenInfo> ofInterest = new List<OddEvenInfo>();

      using (DbConnection cn = VoteDb.GetOpenConnection())
      {
        DbCommand command = VoteDb.GetCommand(sqlText, cn, 0);

        using (DbDataReader reader = command.ExecuteReader())
        {
          int lowOrd = reader.GetOrdinal("AddressPrimaryLowNumber");
          int highOrd = reader.GetOrdinal("AddressPrimaryHighNumber");
          int evenOddOrd = reader.GetOrdinal("AddressPrimaryEvenOdd");

          while (reader.Read())
          {
            string low = reader.GetString(lowOrd);
            string high = reader.GetString(highOrd);
            string evenOdd = reader.GetString(evenOddOrd);
            if (low.Length != 0 && !low.IsDigits() && 
              evenOdd != "B" &&
              low != high &&
              ContainsLetter(low))
              ofInterest.Add(new OddEvenInfo()
              {
                Low = low,
                High = high,
                EvenOdd = evenOdd
              });

            rowCount++;
          }
        }
      }
    }

    private bool ContainsLetter(string low)
    {
      return Regex.Match(low, "[A-Z]", RegexOptions.IgnoreCase).Success;
    }

    Regex AllNumericRegex = new Regex(@"^[0-9]+$");
    //private void CheckZipStreetsDownloadedAddressPrimaryLowNumberNonAlpha()
    //{
    //  string sqlText = "SELECT AddressPrimaryLowNumber FROM ZipStreetsDownloaded";

    //  int rowCount = 0;
    //  int lengthNot10 = 0;
    //  Dictionary<string, object> nonAlpha = new Dictionary<string, object>();

    //  using (DbConnection cn = VoteDb.GetOpenConnection())
    //  {
    //    DbCommand command = VoteDb.GetCommand(sqlText, cn, 0);

    //    using (DbDataReader reader = command.ExecuteReader())
    //    {
    //      int addressPrimaryLowNumberOrd = reader.GetOrdinal("AddressPrimaryLowNumber");

    //      while (reader.Read())
    //      {
    //        string addressPrimaryLowNumber = reader.GetString(addressPrimaryLowNumberOrd);
    //        if (!AllNumericRegex.Match(addressPrimaryLowNumber).Success)
    //          nonAlpha[addressPrimaryLowNumber] = null;
    //        else
    //          if (addressPrimaryLowNumber.Length != 10)
    //            lengthNot10++;

    //        rowCount++;
    //      }
    //    }
    //  }
    //}

    Regex HouseRegex = new Regex(@"^(?:[0-9]+ 1/2)|(?:[-0-9A-Z]+)$", RegexOptions.IgnoreCase);
    Regex HouseRegex2 = new Regex(@"^(?:[0-9]+ 1/2)|(?:[-0-9A-Z]+)", RegexOptions.IgnoreCase);
    private void CheckZipStreetsDownloadedAddressPrimaryLowNumberNonAlpha()
    {
      string sqlText = "SELECT AddressPrimaryLowNumber FROM ZipStreetsDownloaded";

      int rowCount = 0;
      Dictionary<string, object> ofInterest = new Dictionary<string, object>();

      using (DbConnection cn = VoteDb.GetOpenConnection())
      {
        DbCommand command = VoteDb.GetCommand(sqlText, cn, 0);

        using (DbDataReader reader = command.ExecuteReader())
        {
          int addressPrimaryLowNumberOrd = reader.GetOrdinal("AddressPrimaryLowNumber");

          while (reader.Read())
          {
            string addressPrimaryLowNumber = reader.GetString(addressPrimaryLowNumberOrd);
            if (addressPrimaryLowNumber.Length != 0)
              if (!HouseRegex.Match(addressPrimaryLowNumber).Success)
                ofInterest[addressPrimaryLowNumber] = null;

            rowCount++;
          }
        }
      }
    }

    private void CheckZipStreetsDownloadedAddressPrimaryLowNumberBadLength()
    {
      string sqlText = "SELECT UpdateKey,AddressPrimaryLowNumber,AddressPrimaryHighNumber FROM ZipStreetsDownloaded";

      int rowCount = 0;
      Dictionary<string, object> badLength = new Dictionary<string, object>();

      using (DbConnection cn = VoteDb.GetOpenConnection())
      {
        DbCommand command = VoteDb.GetCommand(sqlText, cn, 0);

        using (DbDataReader reader = command.ExecuteReader())
        {
          int updateKeyOrd = reader.GetOrdinal("UpdateKey");
          int addressPrimaryLowNumberOrd = reader.GetOrdinal("AddressPrimaryLowNumber");
          int addressPrimaryHighNumberOrd = reader.GetOrdinal("AddressPrimaryHighNumber");

          while (reader.Read())
          {
            string updateKey = reader.GetString(updateKeyOrd);
            string addressPrimaryLowNumber = reader.GetString(addressPrimaryLowNumberOrd);
            string addressPrimaryHighNumber = reader.GetString(addressPrimaryHighNumberOrd);
            if (AllNumericRegex.Match(addressPrimaryLowNumber).Success &&
              addressPrimaryLowNumber.Length != 10)
              badLength[updateKey] = null;
            if (AllNumericRegex.Match(addressPrimaryHighNumber).Success &&
              addressPrimaryHighNumber.Length != 10)
              badLength[updateKey] = null;

            rowCount++;
          }
        }
      }

      string badKeys = string.Join(Environment.NewLine,
        badLength.Select(kvp => kvp.Key));
    }

    private void CheckZipCitiesDownloaded()
    {
      var table = DB.VoteZipNew.ZipCitiesDownloaded.GetAllData();
      var citiesList = table.AsEnumerable<DB.VoteZipNew.ZipCitiesDownloadedRow>()
        .GroupBy(row => row.City)
        .Select(group => group.Key)
        .ToList();

      var specialCharCities = citiesList
        .Where(city => !Regex.Match(city, @"^[ A-Z]+$").Success)
        .ToList();

      int maxSpaces = 0;
      List<string> manySpaces = new List<string>();
      foreach (string city in citiesList)
      {
        Match match = Regex.Match(city, @"^[^ ]*(?:(?<spaces> )[^ ]*)*$");
        int spaces = match.Groups["spaces"].Captures.Count;
        maxSpaces = Math.Max(maxSpaces, spaces);
        if (spaces > 2) manySpaces.Add(city);
      }
    }

    private void FindButton_Click(object sender, EventArgs e)
    {
      ResultsTextBox.Clear();
      string input = AddressTextBox.Text;
      if (LookupRadioButton.Checked)
      {
        var result = AddressFinder.Find(input);
        ResultsTextBox.Text = result.ToString();
      }
      else
      {
        var result = AddressFinder.Parse(input);
        ResultsTextBox.Text = result.ToString();
      }
    }
  }
}
