using System;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Windows.Forms;
using Vote;
using DB.VoteZipNewLocal;
using UtilityLibrary;

namespace LoadUSZD
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void AppendStatusText(string text)
    {
      if (StatusTextBox.Text.Length != 0)
        this.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      this.Invoke(() => StatusTextBox.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " " + text));
    }

    private void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    private void ProcessAllDataFiles()
    {
      // Assume data files are names US116ZD0.CSV thru US116ZD9.CSV in
      // selected folder, 116 is version (11.6)
      string fileNameTemplate = "US{1}ZD{0}.CSV";
      string version = VersionTextBox.Text.Replace(".", "");
      for (int fileNo = 0; fileNo <= 9; fileNo++)
      {
        string fileName = string.Format(fileNameTemplate, fileNo, version);
        ProcessOneDataFile(Path.Combine(DataFolderBrowserDialog.SelectedPath,
          fileName));
      }
      AppendStatusText("");
      AppendStatusText("Finished");
    }

    private void ProcessOneDataFile(string path)
    {
      AppendStatusText("Processing file {0}", path);
      string cmdText = string.Format(
        "LOAD DATA INFILE '{0}' into table `votezipnew`.`uszdnew` " +
        "FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' ESCAPED BY '\\\\' " +
        "LINES STARTING BY '' TERMINATED BY '\\r\\n';",
        // need to double up the backslashes in the path
        path.Replace(@"\", @"\\"));

      DbCommand cmd = VoteZipNewLocalDb.GetCommand(cmdText, 0);
      VoteZipNewLocalDb.ExecuteNonQuery(cmd);
    }

    private void TruncateExistingTable()
    {
      AppendStatusText("Truncating USZDNew table");
      UszdNew.TruncateTable();
    }

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        if (MessageBox.Show("OK to truncate USZDNew table?", "Truncate Table",
          MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
          MessageBoxDefaultButton.Button2) == DialogResult.OK)
        {
          TruncateExistingTable();
          ProcessAllDataFiles();
        }
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
      BrowseDataPathButton.Enabled = true;
      DataPathTextBox.Enabled = true;
      VersionTextBox.Enabled = true;
    }

    private void BrowseDataPathButton_Click(object sender, EventArgs e)
    {
      DataFolderBrowserDialog.SelectedPath = DataPathTextBox.Text;
      if (DataFolderBrowserDialog.ShowDialog() == DialogResult.OK)
        DataPathTextBox.Text = DataFolderBrowserDialog.SelectedPath;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      BrowseDataPathButton.Enabled = false;
      DataPathTextBox.Enabled = false;
      VersionTextBox.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }
  }
}
