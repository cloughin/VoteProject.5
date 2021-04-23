using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using UtilityLibrary;

namespace ExecuteMySqlCommand
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
      this.Invoke(() => StatusTextBox.AppendText(text));
    }

    private void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      MySqlConnection cn = null;
      try
      {
        AppendStatusText("Executing command.");
        cn = new MySqlConnection(ConnectionTextBox.Text);
        cn.Open();
        MySqlCommand cmd = new MySqlCommand(CommandTextBox.Text, cn);
        cmd.CommandTimeout = 0;
        cmd.ExecuteNonQuery(); 
        AppendStatusText("Command successfully executed.");
      }
      catch (Exception ex)
      {
        AppendStatusText(ex.ToString());
        AppendStatusText("Command failed.");
      }
      finally
      {
        if (cn != null)
          cn.Close();
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      ConnectionTextBox.Enabled = true;
      BrowseConnectionButton.Enabled = true;
      CommandTextBox.Enabled = true;
      BrowseCommandButton.Enabled = true;
      ExecuteButton.Enabled = true;
    }

    private void BrowseCommandButton_Click(object sender, EventArgs e)
    {
      if (OpenCommandFileDialog.ShowDialog() == DialogResult.OK)
        try
        {
          CommandTextBox.Text = File.ReadAllText(OpenCommandFileDialog.FileName);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString(), "An error occurred reading the command file", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BrowseConnectionButton_Click(object sender, EventArgs e)
    {
      if (OpenConnectionFileDialog.ShowDialog() == DialogResult.OK)
        try
        {
          ConnectionTextBox.Text = File.ReadAllText(OpenConnectionFileDialog.FileName);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString(), "An error occurred reading the connection file",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ExecuteButton_Click(object sender, EventArgs e)
    {
      ConnectionTextBox.Enabled = false;
      BrowseConnectionButton.Enabled = false;
      CommandTextBox.Enabled = false;
      BrowseCommandButton.Enabled = false;
      ExecuteButton.Enabled = false;

      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
