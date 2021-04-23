using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Vote;

namespace LoadDerivedZipTables
{
  public partial class CsvMainForm : Form
  {
    readonly Common _Common;

    public CsvMainForm()
    {
      InitializeComponent();
      _Common = new Common(StatusTextBox, ErrorsTextBox);
    }

    private void DoWork()
    {
      for (var zip = 0; zip <= 99999; zip++)
        _Common.DoOneZipCode(zip.ToString(CultureInfo.InvariantCulture).ZeroPad(5));
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      TextWriter textWriter = new StreamWriter(OutputFileTextBox.Text);
      _Common.TextWriter = textWriter;
      try
      {
        DoWork();
        _Common.AppendStatusText("Completed: {0} rows", _Common.RowsWritten);
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
      finally
      {
        textWriter.Close();
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
      OutputFileTextBox.Enabled = true;
      BrowseOutputFileButton.Enabled = true;
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
      OutputFileTextBox.Enabled = false;
      BrowseOutputFileButton.Enabled = false;
      StatusTextBox.Clear();
      ErrorsTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
