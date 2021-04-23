using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilityLibrary;
using Vote;
using DB.Vote;

namespace ValidateDbEmails
{
  public partial class MainForm : Form
  {
    class TableWithEmails
    {
      public string TableName;
      public List<string> KeyColumns;
      public List<string> EmailColumns;
    }

    List<TableWithEmails> TablesWithEmails =
      new List<TableWithEmails>()
      {
        new TableWithEmails()
        {
          TableName = "Counties", 
          KeyColumns = new List<string>(){ "StateCode", "CountyCode" }, 
          EmailColumns = new List<string>(){ "ContactEmail", "AltEMail", "EMail" }
        },
        new TableWithEmails()
        {
          TableName = "LEGIDYY", 
          KeyColumns = new List<string>(){ "LEG_ID_NUM" }, 
          EmailColumns = new List<string>(){ "E_MAIL_ADD" }
        },
        new TableWithEmails()
        {
          TableName = "LocalDistricts", 
          KeyColumns = new List<string>(){ "StateCode", "CountyCode", "LocalCode" }, 
          EmailColumns = new List<string>(){ "ContactEmail", "AltEMail", "EMail" }
        },
        new TableWithEmails()
        {
          TableName = "Organizations", 
          KeyColumns = new List<string>(){ "OrganizationCode" }, 
          EmailColumns = new List<string>(){ "OrganizationEmail", "ContactEmail", "AltContactEMail" }
        },
        new TableWithEmails()
        {
          TableName = "PartiesEmails", 
          KeyColumns = new List<string>(){ "PartyEmail" }, 
          EmailColumns = new List<string>(){ "PartyEmail" }
        },
        new TableWithEmails()
        {
          TableName = "Politicians", 
          KeyColumns = new List<string>(){ "PoliticianKey" }, 
          EmailColumns = new List<string>(){ "EmailAddrVoteUSA", "EmailAddr", "StateEmailAddr", "CampaignEmail", "LDSEmailAddr"  }
        },
        new TableWithEmails()
        {
          TableName = "States", 
          KeyColumns = new List<string>(){ "StateCode" }, 
          EmailColumns = new List<string>(){ "ContactEmail", "AltEMail", "EMail", "SecretaryEMail", "CoordinatorEmail" }
        }
      };

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

    private void ProcessAllTables()
    {
      foreach (TableWithEmails table in TablesWithEmails)
        ProcessOneTable(table);
    }

    private void ProcessOneTable(TableWithEmails table)
    {
      // Create a list of all necessary columns
      List<string> columns = new List<string>();
      foreach (string column in table.KeyColumns)
        if (!columns.Contains(column))
          columns.Add(column);
      foreach (string column in table.EmailColumns)
        if (!columns.Contains(column))
          columns.Add(column);

      // Build the select statement
      StringBuilder sbSelect = new StringBuilder();
      sbSelect.Append("SELECT ");
      sbSelect.Append(string.Join(",", columns));
      sbSelect.Append(" FROM ");
      sbSelect.Append(table.TableName);

      // Create the DbDataReader
      DbConnection cn = VoteDb.GetOpenConnection();
      DbCommand cmd = VoteDb.GetCommand(sbSelect.ToString(), cn, 0);
      DbDataReader reader =  cmd.ExecuteReader();

      // Read and process each row
      AppendStatusText("Processing table {0}, examining columns {1}", table.TableName,
        string.Join(", ", table.EmailColumns));
      int rows = 0;
      using (reader)
        while (reader.Read())
        {
          rows++;
          // Check each email column
          foreach (string emailColumn in table.EmailColumns)
          {
            string email = reader[emailColumn].ToStringOrNull();
            if (!string.IsNullOrWhiteSpace(email) && !Validation.IsValidEmailAddress(email))
            {
              IEnumerable<string> keyDesc = 
                table.KeyColumns
                  .Select(col => col + " = " + reader[col].ToString());
              AppendStatusText("{0} = {1} [{2}]", emailColumn, email, string.Join(", ", keyDesc));
            }
          }
        }
      AppendStatusText("Finished table {0}, rows = {1}", table.TableName, rows);
      AppendStatusText("");
    }

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        ProcessAllTables();
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
  }
}
