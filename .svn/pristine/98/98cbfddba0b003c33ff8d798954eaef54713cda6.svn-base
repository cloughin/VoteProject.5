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
using UtilityLibrary;

namespace DebugReport
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
      this.Invoke(() => StatusTextBox.AppendText(text));
    }

    private void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    private void RunReport()
    {
      AppendStatusText("Starting report...");

      db.Report_Officials_Update(
        StateTextBox.Text,
        CountyTextBox.Text,
        LocalTextBox.Text);

      AppendStatusText("Report complete.");
    }

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        RunReport();
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
      StateTextBox.Enabled = true;
      CountyTextBox.Enabled = true;
      LocalTextBox.Enabled = true;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      DatabaseTextBox.Text = VoteDb.ConnectionString;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      StateTextBox.Enabled = false;
      CountyTextBox.Enabled = false;
      LocalTextBox.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }
  }
}
