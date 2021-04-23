using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Vote;
using UtilityLibrary;
using System.IO;

namespace LoadZipSingleUszd
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
      if (!string.IsNullOrWhiteSpace(text))
        text = DateTime.Now.ToString("HH:mm:ss.fff") + " " + text;
// ReSharper disable once AssignNullToNotNullAttribute
      this.Invoke(() => StatusTextBox.AppendText(text));
    }

// ReSharper disable once MethodOverloadWithOptionalParameter
    private void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      TextWriter textWriter = null;

      try
      {
        textWriter = new StreamWriter(OutputFileTextBox.Text);
        var writer = new ZipSingleUszdWriter(textWriter) {Feedback = AppendStatusText};

        for (var zip = 0; zip <= 99999; zip++)
          writer.DoOneZipCode(zip.ToString(CultureInfo.InvariantCulture).ZeroPad(5));

        AppendStatusText("Completed.");
      }
      catch (Exception ex)
      {
        AppendStatusText("Error: {0}", ex.Message);
      }
      finally
      {
        if (textWriter != null)
          textWriter.Close();
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
    }

    private void BrowseOutputFileButton_Click(object sender, EventArgs e)
    {
      OutputFileSaveFileDialog.FileName = OutputFileTextBox.Text;
      if (OutputFileSaveFileDialog.ShowDialog() == DialogResult.OK)
        OutputFileTextBox.Text = OutputFileSaveFileDialog.FileName;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
