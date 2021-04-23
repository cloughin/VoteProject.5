using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vote;

namespace LoadZipDatabase
{
  public partial class CsvMainForm : Form
  {
    readonly Common _Common;

    public CsvMainForm()
    {
      InitializeComponent();
      _Common = new Common(StatusTextBox);
    }

    private void ProcessAllStates()
    {
      _Common.AppendStatusText("Starting...");

      TextWriter writer = null;
      try
      {
        var directoryInfo = new DirectoryInfo(ZipPlus4PathTextBox.Text);
        if (!directoryInfo.Exists)
          throw new VoteException("Directory \"{0}\" does not exist.", directoryInfo.FullName);

        writer = new StreamWriter(OutputFileTextBox.Text);

        foreach (var stateCode in StateCache.All51StateCodes)
          _Common.ProcessOneState(directoryInfo, stateCode, writer);
      }
      finally
      {
        if (writer != null)
          writer.Close();
      }

      _Common.AppendStatusText("Completed.");
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        ProcessAllStates();
      }
      catch (VoteException ex)
      {
        _Common.AppendStatusText(ex.Message);
        _Common.AppendStatusText("Terminated.");
      }
      catch (Exception ex)
      {
        _Common.AppendStatusText(ex.ToString());
        _Common.AppendStatusText("Terminated.");
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
      BrowseZipPlus4PathButton.Enabled = true;
      ZipPlus4PathTextBox.Enabled = true;
      BrowseOutputFileButton.Enabled = true;
      OutputFileTextBox.Enabled = true;
    }

    private void BrowseOutputFileButton_Click(object sender, EventArgs e)
    {
      OutputFileSaveFileDialog.FileName = OutputFileTextBox.Text;
      if (OutputFileSaveFileDialog.ShowDialog() == DialogResult.OK)
        OutputFileTextBox.Text = OutputFileSaveFileDialog.FileName;
    }

    private void BrowseZipPlus4PathButton_Click(object sender, EventArgs e)
    {
      ZipPlus4FolderBrowserDialog.SelectedPath = ZipPlus4PathTextBox.Text;
      if (ZipPlus4FolderBrowserDialog.ShowDialog() == DialogResult.OK)
        ZipPlus4PathTextBox.Text = ZipPlus4FolderBrowserDialog.SelectedPath;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      BrowseZipPlus4PathButton.Enabled = false;
      ZipPlus4PathTextBox.Enabled = false;
      BrowseOutputFileButton.Enabled = false;
      OutputFileTextBox.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
