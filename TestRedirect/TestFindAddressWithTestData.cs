using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using DB.Vote;
using UtilityLibrary;

namespace TestRedirect
{
  public partial class TestFindAddressWithTestData : Form
  {
    bool UseZip = true;

    public TestFindAddressWithTestData()
    {
      InitializeComponent();
      //Test();
    }

    //private void Test()
    //{
    //  var list = new List<AddressFinder.Part>()
    //  {
    //    {new AddressFinder.Part(0, 0, "ABC")},
    //    {new AddressFinder.Part(0, 0, "def7")},
    //    {new AddressFinder.Part(0, 0, "GHI99")},
    //    {new AddressFinder.Part(0, 0, "00")},
    //  };

    //  var result1 = AddressFinder.PossibleSecondaryParts(list, true).ToList();
    //  var result2 = AddressFinder.PossibleSecondaryParts(list, false).ToList();
    //}

    private void AppendErrorMessages(AddressFinderResult result)
    {
      AppendStatusText("Error messages");
      foreach (string errorMessage in result.ErrorMessages)
        AppendStatusText(errorMessage);
    }

    private void AppendStatusText(string text)
    {
      if (StatusTextBox.Text.Length != 0)
        this.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      this.Invoke(() => StatusTextBox.AppendText(text));
    }

    private void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    private void DoWork()
    {
      int first = 1;
      int last = 500;

      int rowCount = 0;

      //using (var reader = ZipAddressesTesting.GetAllDataReader())
      using (var reader = DB.VoteLog.LogAddressesGood.GetAllDataReader())
      {
        // ZipAddressesTesting
        //int[] knownBadRows = new int[] 
        //{ 
        //  22, 47, 51, 52, 63, 75, 90, 91, 94, 101, 114, 143, 153, 241, 251, 287, 309
        //};

        // LogAddressesGood
        int[] knownBadRows = new int[] 
          { 
            14, 26, 104, 135, 136, 164, 248, 350, 356, 357, 408, 418, 419
          };

        while (reader.Read())
        {
          rowCount++;

          if (rowCount < first) continue;
          if (rowCount > last) break;

          if (knownBadRows.Contains(rowCount))
            continue;

          string addr1 = reader.Address1;
          string addr2 = reader.Address2;
          string city = reader.City;
          string stateCode = reader.StateCode;
          string zip5 = null;
          string zip4 = null;
          if (UseZip)
          {
            zip5 = reader.Zip5;
            zip4 = reader.Zip4;
          }

          CheckNew(rowCount, addr1, addr2, city, stateCode, zip5, zip4);
          //CheckOld(rowCount, addr1, addr2, city, stateCode, zip5, zip4);
        }
      }
    }

    //private void CheckOld(int rowCount, string addr1, string addr2, string city, string stateCode, string zip5, string zip4)
    //{
    //  DataRow dataRow = Default1.Process_Address_Using_Db_Tables(addr1, city, stateCode);
    //  if (dataRow == null)
    //  {
    //    AppendStatusText("{0} Old failed", rowCount);
    //    AppendStatusText("Input: {0}:{1}:{2}", addr1, city, stateCode);
    //    AppendStatusText(string.Empty);
    //  }
    //  else if (UseZip)
    //  {
    //    string zip5Found = dataRow["ZIP5"].ToString();
    //    string zip4Found = dataRow["ZIP4"].ToString();
    //    if (zip5Found != zip5 || zip4Found != zip4)
    //    {
    //      AppendStatusText("{0} Old failed", rowCount);
    //      AppendStatusText("Input: {0}:{1}:{2}:{3}", addr1, addr2, city, stateCode);
    //      AppendStatusText("Non-matching Zip+4: {0}-{1}", zip5, zip4);
    //      AppendStatusText("Found {0}-{1}", zip5Found, zip4Found);
    //      AppendStatusText(string.Empty);
    //    }
    //  }
    //}

    private void CheckNew(int rowCount, string addr1, string addr2, string city, string stateCode, string zip5, string zip4)
    {
      // Try Addr1 + Addr2 + City + StateCode
      // Check that result is successful && Zip5-Zip4 is in zip list
      string input = addr1 + " " + addr2 + " " + city + " " + stateCode;
      var result = AddressFinder.Find(input);
      if (result.SuccessMessage == null) // failure
      {
        AppendStatusText("{0} Failed on input: {1}", rowCount, input);
        if (UseZip)
          AppendStatusText("Zip+4: {0}-{1}", zip5, zip4);
        AppendErrorMessages(result);
        AppendStatusText(string.Empty);
      }
      else if (UseZip)
      {
        string foundMessage = "Found " + zip5 + "-" + zip4;
        if (!result.ErrorMessages.Contains("Zip5 match"))
          if (!result.ErrorMessages.Contains(foundMessage))
          {
            if (!LdsInfoMatches(zip5, zip4, result))
            {
              AppendStatusText("{0} Failed on input: {1}", rowCount, input);
              AppendStatusText("Non-matching Zip+4: {0}-{1}", zip5, zip4);
              AppendStatusText("Addr1 Addr2, City, State: {0} {1}, {2}, {3}",
                addr1, addr2, city, stateCode);
              AppendErrorMessages(result);
              AppendStatusText(string.Empty);
            }
          }
      }

      // Try Addr1 + Addr2 + Zip5
      // Check that result is successful && Zip5-Zip4 is in zip list
      input = addr1 + " " + addr2 + " " + zip5;
      result = AddressFinder.Find(input);
      if (result.SuccessMessage == null) // failure
      {
        AppendStatusText("{0} Failed on input: {1}", rowCount, input);
        AppendStatusText("City, State: {0}, {1}", city, stateCode);
        AppendErrorMessages(result);
        AppendStatusText(string.Empty);
      }
      else if (UseZip)
      {
        string foundMessage = "Found " + zip5 + "-" + zip4;
        if (!result.ErrorMessages.Contains("Zip5 match"))
          if (!result.ErrorMessages.Contains(foundMessage))
          {
            if (!LdsInfoMatches(zip5, zip4, result))
            {
              AppendStatusText("{0} Failed on input: {1}", rowCount, input);
              AppendStatusText("Non-matching Zip+4: {0}-{1}", zip5, zip4);
              AppendStatusText("Addr1 Addr2, City, State: {0} {1}, {2}, {3}",
                addr1, addr2, city, stateCode);
              AppendErrorMessages(result);
              AppendStatusText(string.Empty);
            }
          }
      }

      // Try Zip5-Zip4
      // Check that result is successful
      if (UseZip)
      {
        input = zip5 + "-" + zip4;
        result = AddressFinder.Find(input);
        if (result.SuccessMessage == null) // failure
        {
          AppendStatusText("{0} Failed on input: {1}", rowCount, input);
          AppendStatusText("Zip+4 not found: {0}-{1}", zip5, zip4);
          AppendStatusText("Addr1 Addr2, City, State: {0} {1}, {2}, {3}",
            addr1, addr2, city, stateCode);
          AppendErrorMessages(result);
          AppendStatusText(string.Empty);
        }
      }
    }

    private bool LdsInfoMatches(string zip5, string zip4, AddressFinderResult result)
    {
      var table = DB.VoteZipNew.Uszd.GetDataByZip5Zip4(zip5, zip4);
      if (table.Count == 0) return false;
      var row = table[0];
      if (StateCache.StateCodeFromLdsStateCode(row.LdsStateCode) != result.State)
        return false;
      if (row.Congress.ZeroPad(3) != result.Congress)
        return false;
      if (row.StateSenate.ZeroPad(3) != result.StateSenate)
        return false;
      if (row.StateHouse.ZeroPad(3) != result.StateHouse)
        return false;
      if (row.County.ZeroPad(3) != result.County)
        return false;
      return true;
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        DoWork();
      }
      catch (VoteException ex)
      {
        AppendStatusText(ex.Message);
        AppendStatusText("Terminated.");
      }
      catch (Exception ex)
      {
        AppendStatusText(ex.ToString());
        AppendStatusText("Terminated.");
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
