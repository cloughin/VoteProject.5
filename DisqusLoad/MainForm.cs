using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using DB.Vote;
using UtilityLibrary;
using Vote;
using static System.String;

namespace DisqusLoad
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();

      SetConnectionString();
    }

    private void AppendStatusText(string text)
    {
      if (text == null) return;
      if (StatusTextBox.Text.Length != 0)
        this.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      if (!IsNullOrWhiteSpace(text))
        text = $"{DateTime.Now:HH:mm:ss.fff} {text}";
      this.Invoke(() => StatusTextBox.AppendText(text));
    }

    // ReSharper disable once MethodOverloadWithOptionalParameter
    // ReSharper disable once UnusedMember.Local
    private void AppendStatusText(string text, params object[] arguments)
    {
      AppendStatusText(Format(text, arguments));
    }

    private Dictionary<string, List<DateTime>> ProcessXml()
    {
      var emailList = new Dictionary<string, List<DateTime>>(StringComparer.OrdinalIgnoreCase); 
      var xmlDoc = new XmlDocument();
      xmlDoc.Load(XmlPathTextBox.Text);
      var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
      Debug.Assert(xmlDoc.DocumentElement != null, "xmlDoc.DocumentElement != null");
      nsmgr.AddNamespace("dq", xmlDoc.DocumentElement.NamespaceURI);
      var nodelist = xmlDoc.SelectNodes("/dq:disqus/dq:post", nsmgr);
      Debug.Assert(nodelist != null, nameof(nodelist) + " != null");
      foreach (XmlNode node in nodelist)
      {
        var createdAtNode = node.SelectSingleNode("dq:createdAt", nsmgr);
        //var isDeletedNode = node.SelectSingleNode("dq:isDeleted", nsmgr);
        var isSpamNode = node.SelectSingleNode("dq:isSpam", nsmgr);
        var emailNode = node.SelectSingleNode("dq:author/dq:email", nsmgr);

        Debug.Assert(createdAtNode != null, nameof(createdAtNode) + " != null");
        DateTime.TryParseExact(createdAtNode.InnerText,@"yyyy-MM-dd\THH:mm:ss\Z",
          CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var createdAt);
        createdAt = createdAt.ToUniversalTime();

        //var isDeleted = isDeletedNode != null && bool.Parse(isDeletedNode.InnerText);
        Debug.Assert(isSpamNode != null, nameof(isSpamNode) + " != null");
        var isSpam = bool.Parse(isSpamNode.InnerText);
        Debug.Assert(emailNode != null, nameof(emailNode) + " != null");
        var email = emailNode.InnerText;

        if (/*!isDeleted &&*/ !isSpam && Validation.IsValidEmailAddress(email))
        {
          if (!emailList.TryGetValue(email, out var item))
          {
            item = new List<DateTime>();
            emailList.Add(email, item);
          }
          item.Add(createdAt);
        }
      }
      return emailList;
    }

    private void SetConnectionString()
    {
      string appSettingskey;
      if (TestServerRadioButton.Checked)
        appSettingskey = "TestDb";
      else if (LiveServerRadioButton.Checked)
        appSettingskey = "LiveDb";
      else
        throw new VoteException("Invalid server");
      var connectionString = ConfigurationManager.AppSettings[appSettingskey];
      if (IsNullOrWhiteSpace(connectionString))
        throw new VoteException("Invalid connection string");
      VoteDb.ConnectionString = connectionString;
    }

    private static void UpdateAddresses(Dictionary<string, List<DateTime>> emailList)
    {
      foreach (var kvp in emailList)
      {
        var email = kvp.Key;
        var addressesTable = Addresses.GetDataByEmail(email);
        if (addressesTable.Count == 0) // must add
          addressesTable.AddRow(Empty, Empty, Empty, Empty, Empty,
            Empty, Empty, email, Empty, DateTime.UtcNow, "CMTS", false, false,
            false, VotePage.DefaultDbDate, Empty, Empty, Empty, Empty,
            Empty, Empty, Empty, Empty, Empty, Empty, 
            Empty, Empty, Empty, null, null, VotePage.DefaultDbDate, 0, 
            VotePage.DefaultDbDate, false);
        // find row with most recent date
        var mostRecent = addressesTable.Rows.Cast<AddressesRow>()
          .OrderByDescending(r => r.LastCommentDate).First();
        // only use comments newer than last date
        var lastCommentDate = mostRecent.LastCommentDate;
        var recentComments = kvp.Value.Where(c => c > lastCommentDate).ToList();
        var newCommentCount = mostRecent.CommentCount + recentComments.Count;
        var newCommentDate = recentComments.Max(c => c);
        // update all
        foreach (var row in addressesTable)
        {
          row.CommentCount = newCommentCount;
          row.LastCommentDate = newCommentDate;
        }
        Addresses.UpdateTable(addressesTable);
      }
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        var emailList = ProcessXml();
        UpdateAddresses(emailList);
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
      BrowseXmlPathButton.Enabled = true;
      XmlPathTextBox.Enabled = true;
      ServerGroupBox.Enabled = true;
      SuppressUpdateCheckBox.Enabled = true;
    }

    private void BrowseXmlPathButton_Click(object sender, EventArgs e)
    {
      OpenXmlFileDialog.FileName = XmlPathTextBox.Text;
      if (OpenXmlFileDialog.ShowDialog() == DialogResult.OK)
        XmlPathTextBox.Text = OpenXmlFileDialog.FileName;
    }

    private void ServerRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        if (sender is RadioButton radioButton && radioButton.Checked)
          SetConnectionString();
      }
      catch (VoteException ex)
      {
        AppendStatusText(ex.Message);
      }
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      StartButton.Enabled = false;
      BrowseXmlPathButton.Enabled = false;
      XmlPathTextBox.Enabled = false;
      ServerGroupBox.Enabled = false;
      SuppressUpdateCheckBox.Enabled = false;

      StatusTextBox.Clear();
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion Event handlers
  }
}
