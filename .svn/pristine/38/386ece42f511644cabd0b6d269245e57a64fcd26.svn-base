using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using DB.Vote;
using System.Configuration;

namespace TestFindAddress
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      SetConnectionString();
    }

    private void SetConnectionString()
    {
      string appSettingskey;
      if (TestServerRadioButton.Checked)
        appSettingskey = "TestDb";
      else if (LiveServerRadioButton.Checked)
        appSettingskey = "LiveDb";
      else
        throw new VoteException("Invalid server");
      string connectionString = ConfigurationManager.AppSettings[appSettingskey];
      if (string.IsNullOrWhiteSpace(connectionString))
        throw new VoteException("Invalid connection string");
      VoteDb.ConnectionString = connectionString;
    }

    #region Event handlers

    private void FindButton_Click(object sender, EventArgs e)
    {
      ResultsTextBox.Clear();
      string input = AddressTextBox.Text;
      if (LookupRadioButton.Checked)
      {
        var result = AddressFinder.Find(input);
        ResultsTextBox.Text = result.ToString();
        if (result.SuccessMessage == null)
        {
          var oldResult = AddressFinderOld.Find(input);
        }
      }
      else
      {
        var result = AddressFinder.Parse(input);
        ResultsTextBox.Text = result.ToString();
      }
    }

    private void ServerRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        RadioButton radioButton = sender as RadioButton;
        if (radioButton != null && radioButton.Checked)
          SetConnectionString();
      }
      catch (VoteException ex)
      {
        ResultsTextBox.Text = ex.Message;
      }
    }

    #endregion Event handlers
  }
}
