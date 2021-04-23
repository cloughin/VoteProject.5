using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilityLibrary;
using Vote;
using DB.Vote;

namespace IssueGroups
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    Dictionary<string, string> IssueGroupDictionary = new Dictionary<string, string>()
    {
      {"Abortion", "DOMSOCIAL"},
      {"Alcohol", "DOMSOCIAL"},
      {"Marijuana", "DOMSOCIAL"},
      {"Crime", "DOMSOCIAL"},
      {"Gay", "DOMSOCIAL"},
      {"Guns", "DOMSOCIAL"},
      {"IllegalDrugs", "DOMSOCIAL"},
      {"Immigration", "DOMSOCIAL"},
      {"Welfare", "DOMSOCIAL"},
      {"Business", "ECOBUSTAX"},
      {"Economy", "ECOBUSTAX"},
      {"FiscalPolicy", "ECOBUSTAX"},
      {"Jobs", "ECOBUSTAX"},
      {"Trade", "ECOBUSTAX"},
      {"Wages", "ECOBUSTAX"},
      {"Taxes", "ECOBUSTAX"},
      {"OrganizedLabor", "ECOBUSTAX"},
      {"Courts", "GOVMILITARY"},
      {"Government", "GOVMILITARY"},
      {"Veterans", "GOVMILITARY"},
      {"LegalReform", "GOVMILITARY"},
      {"Military", "GOVMILITARY"},
      {"Consumers", "HEALTHFAMEDU"},
      {"HealthCare", "HEALTHFAMEDU"},
      {"Family", "HEALTHFAMEDU"},
      {"Health", "HEALTHFAMEDU"},
      {"Food", "HEALTHFAMEDU"},
      {"Housing", "HEALTHFAMEDU"},
      {"Medical", "HEALTHFAMEDU"},
      {"MedicalInsurance", "HEALTHFAMEDU"},
      {"PrescriptionDrugs", "HEALTHFAMEDU"},
      {"Education", "HEALTHFAMEDU"},
      {"Seniors", "HEALTHFAMEDU"},
      {"Pensions", "HEALTHFAMEDU"},
      {"Religion", "HEALTHFAMEDU"},
      {"Energy", "SCIENCETECH"},
      {"Oil", "SCIENCETECH"},
      {"Environment", "SCIENCETECH"},
      {"Internet", "SCIENCETECH"},
      {"Science", "SCIENCETECH"},
      {"StemCell", "SCIENCETECH"},
      {"Cloning", "SCIENCETECH"},
      {"Technology", "SCIENCETECH"},
      {"Space", "SCIENCETECH"},
      {"Agriculture", "STATE"},
      {"Cigarettes", "STATE"},
      {"CivilLiberties", "STATE"},
      {"Diversity", "STATE"},
      {"Parks", "STATE"},
      {"Development", "STATE"},
      {"Gambling", "STATE"},
      {"HomelandSecurity", "STATE"},
      {"Justice", "STATE"},
      {"Latinos", "STATE"},
      {"AfroAmericans", "STATE"},
      {"NativeAmericans", "STATE"},
      {"Neighborhoods", "STATE"},
      {"PublicService", "STATE"},
      {"Services", "STATE"},
      {"Patriotism", "STATE"},
      {"Property", "STATE"},
      {"Transportation", "STATE"},
      {"Values", "STATE"}
    };

    internal void AppendStatusText(string text)
    {
      Form form = StatusTextBox.Parent as Form;
      if (StatusTextBox.Text.Length != 0)
        form.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      if (!string.IsNullOrWhiteSpace(text))
        text = DateTime.Now.ToString("HH:mm:ss.fff") + " " + text;
      form.Invoke(() => StatusTextBox.AppendText(text));
    }

    internal void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(string.Format(text, arguments));
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        int found = 0;
        int notFound = 0;
        bool update = (bool) e.Argument;
        var stateCode = StateCodeTextBox.Text.Trim().ToUpperInvariant();
        if (!StateCache.IsValidStateCode(stateCode))
          throw new VoteException("Invalid state code");
        var table = Issues.GetDataByStateCode(stateCode);
        foreach (var row in table)
          if (!row.IsIssueOmit)
          {
            if (!row.IssueKey.StartsWith("C" + stateCode))
              throw new VoteException("Bad IssueKey: " + row.IssueKey);
            string tableKey = row.IssueKey.Substring(3);
            string issueGroupSuffix;
            if (IssueGroupDictionary.TryGetValue(tableKey, out issueGroupSuffix))
            {
              found++;
            }
            else
            {
              notFound++;
              AppendStatusText("Key not found: " + row.IssueKey);
            }
          }
        AppendStatusText("{0} found, {1} not found", found, notFound);
        if (notFound > 0 || !update)
          AppendStatusText("Not updated");
        else
        {
         foreach (var row in table)
           if (!row.IsIssueOmit)
           {
             string tableKey = row.IssueKey.Substring(3);
             string issueGroupSuffix = IssueGroupDictionary[tableKey];
             IssueGroupsIssues.Insert(
               issueGroupKey: stateCode + issueGroupSuffix, issueKey: row.IssueKey,
               issueOrder: 0);
           }
         AppendStatusText("Table updated");
        }
     }
      catch (Exception ex)
      {
        AppendStatusText("Exception: " + ex.Message);
      }

      AppendStatusText("Process is complete.");
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      StartButton.Enabled = true;
      UpdateCheckBox.Checked = false;
      UpdateCheckBox.Enabled = true;
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      UpdateCheckBox.Enabled = false;
      StatusTextBox.Clear();

      BackgroundWorker.RunWorkerAsync(UpdateCheckBox.Checked);
    }

    #endregion Event handlers
  }
}
