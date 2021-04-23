using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using Ionic.Zip;
using LumenWorks.Framework.IO.Csv;
using UtilityLibrary;

namespace LoadZipCitiesDatabase
{
  public partial class CsvMainForm : Form
  {
    Common Common;

    public CsvMainForm()
    {
      InitializeComponent();
      Common = new Common(this.StatusTextBox);
    }

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        Common.LoadCities(InputFileTextBox.Text, OutputFileTextBox.Text);
      }
      catch (VoteException ex)
      {
        Common.AppendStatusText(ex.Message);
        Common.AppendStatusText("Terminated.");
      }
      catch (Exception ex)
      {
        Common.AppendStatusText(ex.ToString());
        Common.AppendStatusText("Terminated.");
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
      BrowseInputFileButton.Enabled = true;
      InputFileTextBox.Enabled = true;
      BrowseOutputFileButton.Enabled = true;
      OutputFileTextBox.Enabled = true;
    }

    private void BrowseInputFileButton_Click(object sender, EventArgs e)
    {
      InputFileDialog.FileName = InputFileTextBox.Text;
      if (InputFileDialog.ShowDialog() == DialogResult.OK)
        InputFileTextBox.Text = InputFileDialog.FileName;
    }

    private void BrowseOutputFileButton_Click(object sender, EventArgs e)
    {
      OutputFileDialog.FileName = OutputFileTextBox.Text;
      if (OutputFileDialog.ShowDialog() == DialogResult.OK)
        OutputFileTextBox.Text = OutputFileDialog.FileName;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      BrowseInputFileButton.Enabled = false;
      InputFileTextBox.Enabled = false;
      BrowseOutputFileButton.Enabled = false;
      OutputFileTextBox.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #region Event handlers

    #endregion Event handlers
  }
}
