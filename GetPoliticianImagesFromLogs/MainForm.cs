using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DB.Vote;
using DB.VoteLog;
using UtilityLibrary;
using Vote;
using static System.String;

namespace GetPoliticianImagesFromLogs
{
  public partial class MainForm : Form
  {
    private class ImageInfo
    {
      public string ImageType; /* "Profile" or "Headshot" */
      public DateTime Date;
      public string User;
      public byte[] Blob;
    }

    public MainForm()
    {
      InitializeComponent();
      SetConnectionString();
    }

    private void AppendStatusText(string text, params object[] arguments)
    {
      var form = StatusTextBox.Parent as Form;
      if (StatusTextBox.Text.Length != 0)
        form.Invoke(() => StatusTextBox.AppendText(Environment.NewLine));
      if (IsNullOrWhiteSpace(text)) return;
      text = $"{DateTime.Now:HH:mm:ss.fff} {Format(text, arguments)}";
      form.Invoke(() => StatusTextBox.AppendText(text));
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
      VoteLogDb.ConnectionString = connectionString;
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        var politicianKey = PoliticianKeyTextBox.Text.Trim();
        if (IsNullOrEmpty(politicianKey))
          throw new VoteException("Politician Key is missing");
        if (IsNullOrEmpty(FolderTextBox.Text.Trim()))
          throw new VoteException("Save Folder is missing");
        var folder = new DirectoryInfo(FolderTextBox.Text.Trim());
        if (!folder.Exists)
          throw new VoteException("Save Folder does not exist");
        var newOriginalsTable =
          LogDataChange.GetDataByTableNameColumnNameKeyValues(
            PoliticiansImagesBlobs.TableName,
            PoliticiansImagesBlobs.GetColumnName(
              PoliticiansImagesBlobs.Column.ProfileOriginal), politicianKey, 0);
        var newHeadshotsTable =
          LogDataChange.GetDataByTableNameColumnNameKeyValues(
            PoliticiansImagesBlobs.TableName,
            PoliticiansImagesBlobs.GetColumnName(
              PoliticiansImagesBlobs.Column.Headshot100), politicianKey, 0);
        var images = new List<ImageInfo>();
        images.AddRange(newOriginalsTable.Where(row => row.NewValue != null)
          .Select(row => new ImageInfo
          {
            ImageType = "Profile",
            Date = row.DateStamp,
            User = row.UserName,
            Blob = Convert.FromBase64String(row.NewValue)
          }));
        images.AddRange(newHeadshotsTable.Where(row => row.NewValue != null)
          .Select(row => new ImageInfo
          {
            ImageType = "Headshot",
            Date = row.DateStamp,
            User = row.UserName,
            Blob = Convert.FromBase64String(row.NewValue)
          }));
        if (images.Count == 0)
          throw new VoteException("No logged images found for this Politician Key");
        folder.CreateSubdirectory(politicianKey);
        AppendStatusText("Found {0} images:", images.Count);
        foreach (var imageInfo in images)
        {
          var memoryStream = new MemoryStream(imageInfo.Blob);
          var image = Image.FromStream(memoryStream);
          var contentType = ImageManager.GetContentType(image);
          var extension = $".{contentType.Replace("image/", "")}";
          var filename =
            $"{imageInfo.ImageType}_{imageInfo.Date:yyyy-MM-dd-HH-mm-ss}_{imageInfo.User}{extension}";
          var path = Path.Combine(folder.FullName, politicianKey, filename);
          File.WriteAllBytes(path, imageInfo.Blob);
          AppendStatusText(filename);
        }
        AppendStatusText(Empty);
      }
      catch (VoteException ex)
      {
        AppendStatusText(ex.Message);
      }
      catch (Exception ex)
      {
        AppendStatusText(ex.ToString());
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender,
      RunWorkerCompletedEventArgs e)
    {
      ServerGroupBox.Enabled = true;
      PoliticianKeyTextBox.Enabled = true;
      FolderTextBox.Enabled = true;
      BrowseButton.Enabled = true;
      StartButton.Enabled = true;
    }

    private void BrowseButton_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog.SelectedPath = FolderTextBox.Text;
      if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
        FolderTextBox.Text = FolderBrowserDialog.SelectedPath;
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
      ServerGroupBox.Enabled = false;
      PoliticianKeyTextBox.Enabled = false;
      FolderTextBox.Enabled = false;
      BrowseButton.Enabled = false;
      StartButton.Enabled = false;
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion
  }
}