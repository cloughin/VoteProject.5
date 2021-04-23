using System;
using System.Configuration;
using System.Windows.Forms;
using DB.Vote;
using Vote;

namespace TestSubstitutions
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      SetConnectionString();
    }

    private void SetConnectionString()
    {
      string appSettingskey;
      if (TestServerRadioButton.Checked) appSettingskey = "TestDb";
      else if (LiveServerRadioButton.Checked) appSettingskey = "LiveDb";
      else throw new VoteException("Invalid server");
      var connectionString = ConfigurationManager.AppSettings[appSettingskey];
      if (string.IsNullOrWhiteSpace(connectionString)) throw new VoteException("Invalid connection string");
      VoteDb.ConnectionString = connectionString;
    }

    #region Event handlers

    private void ServerRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      var radioButton = sender as RadioButton;
      if (radioButton != null && radioButton.Checked) SetConnectionString();
    }

    #endregion Event handlers

    private void ButtonSubstitute_Click(object sender, EventArgs e)
    {
      try
      {
        var substitutions = new Substitutions();
        for (var i = 1; i <= 6; i++)
        {
          var nameControls = Controls.Find(
            string.Format("TextBoxAdHoc{0}Name", i), true);
          var valueControls =
            Controls.Find(string.Format("TextBoxAdHoc{0}Value", i), true);
          if (nameControls.Length == 1 && valueControls.Length == 1)
          {
            var nameTextBox = nameControls[0] as TextBox;
            var valueTextBox = valueControls[0] as TextBox;
            if (nameTextBox != null && valueTextBox != null)
            {
              var name = nameTextBox.Text.Trim();
              if (!string.IsNullOrEmpty(name)) substitutions.AddSubstitutions(name, valueTextBox.Text);
            }
          }
        }

        substitutions.StateCode = TextBoxStateCode.Text.Trim();
        substitutions.CountyCode = TextBoxCountyCode.Text.Trim();
        substitutions.LocalCode = TextBoxLocalCode.Text.Trim();
        substitutions.PoliticianKey = TextBoxPoliticianKey.Text.Trim();
        substitutions.ElectionKey = TextBoxElectionKey.Text.Trim();
        substitutions.OfficeKey = TextBoxOfficeKey.Text.Trim();
        substitutions.IssueKey = TextBoxIssueKey.Text.Trim();
        substitutions.PartyKey = TextBoxPartyKey.Text.Trim();
        substitutions.PartyEmail = TextBoxPartyEmail.Text.Trim();
        substitutions.OrganizationCode = TextBoxOrganizationCode.Text.Trim();

        TextBoxSubstituted.Text = substitutions.Substitute(TextBoxOriginal.Text);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, @"Substitution Error", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
      }
    }
  }
}