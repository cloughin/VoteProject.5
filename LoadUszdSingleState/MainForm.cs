using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vote;
using DB.VoteZipNewLocal;
using UtilityLibrary;

namespace LoadUszdSingleState
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

    private static void DeleteState(string ldsStateCode)
    {
      UszdNew.DeleteByLdsStateCode(ldsStateCode, 0);
    }

    private void ProcessDataFile()
    {
      var path = DataPathTextBox.Text.Trim();
      AppendStatusText("Processing file {0}", path);
      // problem with @varaibles & @parameters
      var cmdText =
        $"LOAD DATA INFILE '{path.Replace(@"\", @"\\")}' into table `votezipnew`.`uszdnew` " +
        "FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' ESCAPED BY '\\\\' " +
        "LINES STARTING BY '' TERMINATED BY '\\r\\n' " +
        "(ZIP5,ZIP4,@dummy,@dummy,@dummy,ST,CNTY,C,RT,F,CD,SD,HD)";

      var cmd = VoteZipNewLocalDb.GetCommand(cmdText, 0);
      VoteZipNewLocalDb.AddCommandParameter(cmd, "dummy", string.Empty);
      VoteZipNewLocalDb.ExecuteNonQuery(cmd);
    }

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        var ldsStateCode = LdsStateTextBox.Text.Trim();
        var stateCode = StateCache.StateCodeFromLdsStateCode(ldsStateCode);
        if (string.IsNullOrEmpty(stateCode))
          throw new VoteException("Invalid LDS State");
        if (MessageBox.Show(
          $"OK to delete LDS State {ldsStateCode} ({stateCode}) from USZDNew table?",
          "Delete State",
          MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
          MessageBoxDefaultButton.Button2) == DialogResult.OK)
        {
          DeleteState(ldsStateCode);
          ProcessDataFile();
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
      LdsStateTextBox.Enabled = true;
    }

    private void BrowseDataPathButton_Click(object sender, EventArgs e)
    {
      DataOpenFileDialog.FileName = DataPathTextBox.Text;
      if (DataOpenFileDialog.ShowDialog() == DialogResult.OK)
        DataPathTextBox.Text = DataOpenFileDialog.FileName;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      BrowseDataPathButton.Enabled = false;
      DataPathTextBox.Enabled = false;
      LdsStateTextBox.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }
  }
}
