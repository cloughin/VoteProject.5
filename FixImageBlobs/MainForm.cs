using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using UtilityLibrary;
using Vote;
using DB.Vote;
using DB.VoteLog;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace FixImageBlobs
{
  internal enum RunType
  {
    AnalyzeProfileOriginal,
    ConvertProfileOriginal,
    ConvertProfileImages,
    /*ConvertHeadshotImages,*/
    FixHeadshots,
    FixNoPhotoImage
  };

  public partial class MainForm : Form
  {
    RunType RunType = RunType.FixNoPhotoImage;

    public MainForm()
    {
      InitializeComponent();
      AppendStatusText(RunType.ToString());
      SetConnectionString();
    }

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

    private void AnalyzeProfileOriginal(string key)
    {
      var profileOriginalBlob = PoliticiansImagesBlobs.GetProfileOriginal(key);
      if (profileOriginalBlob != null)
      {
        var dataTable = PoliticiansImagesData.GetData(key);
        bool nullIt = dataTable.Count == 1 &&
          dataTable[0].ProfileOriginalDate <= dataTable[0].HeadshotDate;
        byte[] newBlob = null;
        MemoryStream memoryStream = new MemoryStream(profileOriginalBlob);
        Image image = Image.FromStream(memoryStream);
        if (nullIt)
        {
          AppendStatusText("{0}: {1}x{2} {3}->{4} [{5}:{6}]",
           key, image.Width, image.Height, profileOriginalBlob.Length,
           0, profileOriginalBlob.Length,
           ImageManager.GetContentType(image).Replace("image/", ""));
        }
        else
        {
          newBlob = ImageManager.GetResizedImageBlobAsJpg(image, 0, 0);
          AppendStatusText("{0}: {1}x{2} {3}->{4} [{5}:{6}]",
            key, image.Width, image.Height, profileOriginalBlob.Length,
            newBlob.Length, profileOriginalBlob.Length - newBlob.Length,
            ImageManager.GetContentType(image).Replace("image/", ""));
          File.WriteAllBytes(@"c:\ProfileOriginals\" + key + ".jpg", newBlob);
        }
      }
    }

    private void ConvertProfileOriginal(string key)
    {
      var profileOriginalBlob = PoliticiansImagesBlobs.GetProfileOriginal(key);
      if (profileOriginalBlob != null)
      {
        MemoryStream memoryStream = new MemoryStream(profileOriginalBlob);
        Image image = Image.FromStream(memoryStream);
        var newBlob = ImageManager.GetResizedImageBlobAsJpg(image, 1600, 1600);
        File.WriteAllBytes(@"c:\VoteImages\ProfileOriginal\" + key + ".jpg", newBlob);
        PoliticiansImagesBlobs.UpdateProfileOriginal(newBlob, key);
        AppendStatusText("{0}: {1} -> {2}", key, profileOriginalBlob.Length, newBlob.Length);
      }
    }

    //private void ConvertHeadshotImages(string key)
    //{
    //  var headshotOriginalBlob = PoliticiansImagesBlobs.GetHeadshotOriginal(key);
    //  if (headshotOriginalBlob != null)
    //  {
    //    MemoryStream memoryStream = new MemoryStream(headshotOriginalBlob);
    //    Image image = Image.FromStream(memoryStream);
    //    var newBlob100 = ImageManager.GetResizedImageBlobAsJpg(image, 100, 100);
    //    File.WriteAllBytes(@"c:\VoteImages\Headshot100\" + key + ".jpg", newBlob100);
    //    PoliticiansImagesBlobs.UpdateHeadshot100(newBlob100, key);
    //    var newBlob75 = ImageManager.GetResizedImageBlobAsJpg(image, 75, 75);
    //    File.WriteAllBytes(@"c:\VoteImages\Headshot75\" + key + ".jpg", newBlob75);
    //    PoliticiansImagesBlobs.UpdateHeadshot75(newBlob75, key);
    //    var newBlob50 = ImageManager.GetResizedImageBlobAsJpg(image, 50, 50);
    //    File.WriteAllBytes(@"c:\VoteImages\Headshot50\" + key + ".jpg", newBlob50);
    //    PoliticiansImagesBlobs.UpdateHeadshot50(newBlob50, key);
    //    var newBlob35 = ImageManager.GetResizedImageBlobAsJpg(image, 35, 35);
    //    File.WriteAllBytes(@"c:\VoteImages\Headshot35\" + key + ".jpg", newBlob35);
    //    PoliticiansImagesBlobs.UpdateHeadshot35(newBlob35, key);
    //    var newBlob25 = ImageManager.GetResizedImageBlobAsJpg(image, 25, 25);
    //    File.WriteAllBytes(@"c:\VoteImages\Headshot25\" + key + ".jpg", newBlob25);
    //    PoliticiansImagesBlobs.UpdateHeadshot25(newBlob25, key);
    //    var newBlob20 = ImageManager.GetResizedImageBlobAsJpg(image, 20, 20);
    //    File.WriteAllBytes(@"c:\VoteImages\Headshot20\" + key + ".jpg", newBlob20);
    //    PoliticiansImagesBlobs.UpdateHeadshot20(newBlob20, key);
    //    var newBlob15 = ImageManager.GetResizedImageBlobAsJpg(image, 15, 15);
    //    File.WriteAllBytes(@"c:\VoteImages\Headshot15\" + key + ".jpg", newBlob15);
    //    PoliticiansImagesBlobs.UpdateHeadshot15(newBlob15, key);
    //    AppendStatusText(key);
    //  }
    //  else
    //  {
    //    Byte[] headshotBlob;
    //    headshotBlob = PoliticiansImagesBlobs.GetHeadshot100(key);
    //    if (headshotBlob != null)
    //    {
    //      MemoryStream memoryStream = new MemoryStream(headshotBlob);
    //      Image image = Image.FromStream(memoryStream);
    //      var newBlob = ImageManager.GetResizedImageBlobAsJpg(image, 100, 100);
    //      File.WriteAllBytes(@"c:\VoteImages\Headshot100\" + key + ".jpg", newBlob);
    //      PoliticiansImagesBlobs.UpdateHeadshot100(newBlob, key);
    //    }
    //    headshotBlob = PoliticiansImagesBlobs.GetHeadshot75(key);
    //    if (headshotBlob != null)
    //    {
    //      MemoryStream memoryStream = new MemoryStream(headshotBlob);
    //      Image image = Image.FromStream(memoryStream);
    //      var newBlob = ImageManager.GetResizedImageBlobAsJpg(image, 75, 75);
    //      File.WriteAllBytes(@"c:\VoteImages\Headshot75\" + key + ".jpg", newBlob);
    //      PoliticiansImagesBlobs.UpdateHeadshot75(newBlob, key);
    //    }
    //    headshotBlob = PoliticiansImagesBlobs.GetHeadshot50(key);
    //    if (headshotBlob != null)
    //    {
    //      MemoryStream memoryStream = new MemoryStream(headshotBlob);
    //      Image image = Image.FromStream(memoryStream);
    //      var newBlob = ImageManager.GetResizedImageBlobAsJpg(image, 50, 50);
    //      File.WriteAllBytes(@"c:\VoteImages\Headshot50\" + key + ".jpg", newBlob);
    //      PoliticiansImagesBlobs.UpdateHeadshot50(newBlob, key);
    //    }
    //    headshotBlob = PoliticiansImagesBlobs.GetHeadshot35(key);
    //    if (headshotBlob != null)
    //    {
    //      MemoryStream memoryStream = new MemoryStream(headshotBlob);
    //      Image image = Image.FromStream(memoryStream);
    //      var newBlob = ImageManager.GetResizedImageBlobAsJpg(image, 35, 35);
    //      File.WriteAllBytes(@"c:\VoteImages\Headshot35\" + key + ".jpg", newBlob);
    //      PoliticiansImagesBlobs.UpdateHeadshot35(newBlob, key);
    //    }
    //    headshotBlob = PoliticiansImagesBlobs.GetHeadshot25(key);
    //    if (headshotBlob != null)
    //    {
    //      MemoryStream memoryStream = new MemoryStream(headshotBlob);
    //      Image image = Image.FromStream(memoryStream);
    //      var newBlob = ImageManager.GetResizedImageBlobAsJpg(image, 25, 25);
    //      File.WriteAllBytes(@"c:\VoteImages\Headshot25\" + key + ".jpg", newBlob);
    //      PoliticiansImagesBlobs.UpdateHeadshot25(newBlob, key);
    //    }
    //    headshotBlob = PoliticiansImagesBlobs.GetHeadshot20(key);
    //    if (headshotBlob != null)
    //    {
    //      MemoryStream memoryStream = new MemoryStream(headshotBlob);
    //      Image image = Image.FromStream(memoryStream);
    //      var newBlob = ImageManager.GetResizedImageBlobAsJpg(image, 20, 20);
    //      File.WriteAllBytes(@"c:\VoteImages\Headshot20\" + key + ".jpg", newBlob);
    //      PoliticiansImagesBlobs.UpdateHeadshot20(newBlob, key);
    //    }
    //    headshotBlob = PoliticiansImagesBlobs.GetHeadshot15(key);
    //    if (headshotBlob != null)
    //    {
    //      MemoryStream memoryStream = new MemoryStream(headshotBlob);
    //      Image image = Image.FromStream(memoryStream);
    //      var newBlob = ImageManager.GetResizedImageBlobAsJpg(image, 15, 15);
    //      File.WriteAllBytes(@"c:\VoteImages\Headshot15\" + key + ".jpg", newBlob);
    //      PoliticiansImagesBlobs.UpdateHeadshot15(newBlob, key);
    //    }
    //  }
    //}

    private void ConvertProfileImages(string key)
    {
      var profileOriginalBlob = PoliticiansImagesBlobs.GetProfileOriginal(key);
      if (profileOriginalBlob != null)
      {
        MemoryStream memoryStream = new MemoryStream(profileOriginalBlob);
        Image image = Image.FromStream(memoryStream);
        var newBlob300 = ImageManager.GetResizedImageBlobAsJpg(image, 300, 400);
        File.WriteAllBytes(@"c:\VoteImages\Profile300\" + key + ".jpg", newBlob300);
        PoliticiansImagesBlobs.UpdateProfile300(newBlob300, key);
        var newBlob200 = ImageManager.GetResizedImageBlobAsJpg(image, 200, 275);
        File.WriteAllBytes(@"c:\VoteImages\Profile200\" + key + ".jpg", newBlob200);
        PoliticiansImagesBlobs.UpdateProfile200(newBlob200, key);
        AppendStatusText(key);
      }
      else
      {
        var profile300Blob = PoliticiansImagesBlobs.GetProfile300(key);
        if (profile300Blob != null)
        {
          MemoryStream memoryStream = new MemoryStream(profile300Blob);
          Image image = Image.FromStream(memoryStream);
          var newBlob300 = ImageManager.GetResizedImageBlobAsJpg(image, 300, 400);
          File.WriteAllBytes(@"c:\VoteImages\Profile300\" + key + ".jpg", newBlob300);
          PoliticiansImagesBlobs.UpdateProfile300(newBlob300, key);
        }
        var profile200Blob = PoliticiansImagesBlobs.GetProfile200(key);
        if (profile200Blob != null)
        {
          MemoryStream memoryStream = new MemoryStream(profile200Blob);
          Image image = Image.FromStream(memoryStream);
          var newBlob200 = ImageManager.GetResizedImageBlobAsJpg(image, 200, 275);
          File.WriteAllBytes(@"c:\VoteImages\Profile200\" + key + ".jpg", newBlob200);
          PoliticiansImagesBlobs.UpdateProfile200(newBlob200, key);
        }
      }
    }

    private void FixHeadshots(string key)
    {
      byte[] originalBlob = null;
      var logTable = LogPoliticiansImagesHeadshot.GetLatestData(key);
      if (logTable.Count > 0)
        originalBlob = logTable[0].HeadshotOriginal;
      else
        originalBlob = PoliticiansImagesBlobs.GetProfileOriginal(key);
      if (originalBlob != null)
      {
        MemoryStream memoryStream = new MemoryStream(originalBlob);
        Image image = Image.FromStream(memoryStream);

        var newBlob100 = ImageManager.GetResizedImageBlobAsJpg(image, 100, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot100\" + key + ".jpg", newBlob100);
        PoliticiansImagesBlobs.UpdateHeadshot100(newBlob100, key);

        var newBlob75 = ImageManager.GetResizedImageBlobAsJpg(image, 75, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot75\" + key + ".jpg", newBlob75);
        PoliticiansImagesBlobs.UpdateHeadshot75(newBlob75, key);

        var newBlob50 = ImageManager.GetResizedImageBlobAsJpg(image, 50, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot50\" + key + ".jpg", newBlob50);
        PoliticiansImagesBlobs.UpdateHeadshot50(newBlob50, key);

        var newBlob35 = ImageManager.GetResizedImageBlobAsJpg(image, 35, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot35\" + key + ".jpg", newBlob35);
        PoliticiansImagesBlobs.UpdateHeadshot35(newBlob35, key);

        var newBlob25 = ImageManager.GetResizedImageBlobAsJpg(image, 25, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot25\" + key + ".jpg", newBlob25);
        PoliticiansImagesBlobs.UpdateHeadshot25(newBlob25, key);

        var newBlob20 = ImageManager.GetResizedImageBlobAsJpg(image, 20, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot20\" + key + ".jpg", newBlob20);
        PoliticiansImagesBlobs.UpdateHeadshot20(newBlob20, key);

        var newBlob15 = ImageManager.GetResizedImageBlobAsJpg(image, 15, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot15\" + key + ".jpg", newBlob15);
        PoliticiansImagesBlobs.UpdateHeadshot15(newBlob15, key);

        AppendStatusText("processed: {0}", key);
      }
      else
        AppendStatusText("no original: {0}", key);
    }

    private void FixNoPhotoImage()
    {
      string path = @"c:\Temp\NoPhoto300.png";
      string key = "NoPhoto";
      DateTime now = DateTime.Now;
      using (Stream stream = File.OpenRead(path))
      {
        Image image = Image.FromStream(stream);

        var newBlob300 = ImageManager.GetResizedImageBlob(image, 300, 0);
        File.WriteAllBytes(@"c:\VoteImages\Profile300\" + key + ".png", newBlob300);
        PoliticiansImagesBlobs.UpdateProfile300(newBlob300, key);
        PoliticiansImagesBlobs.UpdateProfileOriginal(newBlob300, key);

        var newBlob200 = ImageManager.GetResizedImageBlob(image, 200, 0);
        File.WriteAllBytes(@"c:\VoteImages\Profile200\" + key + ".png", newBlob200);
        PoliticiansImagesBlobs.UpdateProfile200(newBlob200, key);

        var newBlob100 = ImageManager.GetResizedImageBlob(image, 100, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot100\" + key + ".png", newBlob100);
        PoliticiansImagesBlobs.UpdateHeadshot100(newBlob100, key);

        var newBlob75 = ImageManager.GetResizedImageBlob(image, 75, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot75\" + key + ".png", newBlob75);
        PoliticiansImagesBlobs.UpdateHeadshot75(newBlob75, key);

        var newBlob50 = ImageManager.GetResizedImageBlob(image, 50, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot50\" + key + ".png", newBlob50);
        PoliticiansImagesBlobs.UpdateHeadshot50(newBlob50, key);

        var newBlob35 = ImageManager.GetResizedImageBlob(image, 35, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot35\" + key + ".png", newBlob35);
        PoliticiansImagesBlobs.UpdateHeadshot35(newBlob35, key);

        var newBlob25 = ImageManager.GetResizedImageBlob(image, 25, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot25\" + key + ".png", newBlob25);
        PoliticiansImagesBlobs.UpdateHeadshot25(newBlob25, key);

        var newBlob20 = ImageManager.GetResizedImageBlob(image, 20, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot20\" + key + ".png", newBlob20);
        PoliticiansImagesBlobs.UpdateHeadshot20(newBlob20, key);

        var newBlob15 = ImageManager.GetResizedImageBlob(image, 15, 0);
        File.WriteAllBytes(@"c:\VoteImages\Headshot15\" + key + ".png", newBlob15);
        PoliticiansImagesBlobs.UpdateHeadshot15(newBlob15, key);

        PoliticiansImagesData.UpdateHeadshotDate(now, key);
        PoliticiansImagesData.UpdateHeadshotResizeDate(now, key);
        PoliticiansImagesData.UpdateProfileOriginalDate(now, key);

        AppendStatusText("Complete");
      }
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
      string connectionString = ConfigurationManager.AppSettings[appSettingskey];
      if (string.IsNullOrWhiteSpace(connectionString))
        throw new VoteException("Invalid connection string");
      VoteDb.ConnectionString = connectionString;
    }

    #region Event handlers

    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        switch (RunType)
        {
          case RunType.FixNoPhotoImage:
            FixNoPhotoImage();
            break;

          default:
            // Get list of keys in PoliticiansImagesBlobs
            var keys = PoliticiansImagesBlobs.GetAllKeyData(0)
              .Select(row => row.PoliticianKey)
              .ToList();

            foreach (string key in keys)
            //if (key.IsGeIgnoreCase("OHGreenDoug"))
            {
              switch (RunType)
              {
                case RunType.AnalyzeProfileOriginal:
                  AnalyzeProfileOriginal(key);
                  break;

                case RunType.ConvertProfileOriginal:
                  ConvertProfileOriginal(key);
                  break;

                case RunType.ConvertProfileImages:
                  ConvertProfileImages(key);
                  break;

                //case RunType.ConvertHeadshotImages:
                //  ConvertHeadshotImages(key);
                //  break;

                case RunType.FixHeadshots:
                  FixHeadshots(key);
                  break;
              }
            }
            break;
        }
      }
      catch (Exception ex)
      {
        AppendStatusText(ex.Message);
      }
    }

    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {

    }

    private void ServerRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      try
      {
        RadioButton radioButton = sender as RadioButton;
        if (radioButton != null && radioButton.Checked)
          SetConnectionString();
      }
      catch (VoteException ex)
      {
        AppendStatusText(ex.Message);
      }
    }

    private void StartButton_Click(object sender, EventArgs e)
    {
      BackgroundWorker.RunWorkerAsync();
    }

    #endregion
  }
}
