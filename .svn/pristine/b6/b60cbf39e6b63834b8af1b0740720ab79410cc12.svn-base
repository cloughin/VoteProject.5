using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vote;
using DB.Vote;

namespace VoteSetHeadshotContentType
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void CheckProfileContentTypeButton_Click(object sender, EventArgs e)
    {
      List<string> errors = new List<string>();
      CheckProfileContentTypeButton.Enabled = false;
      int count = 0;
      var table = PoliticiansImagesData.GetAllData();
      foreach (var row in table)
      {
        count++;
        string politicianKey = row.PoliticianKey;
        byte[] blob =
          PoliticiansImagesBlobs.GetProfileOriginalByPoliticianKey(politicianKey);
        if (blob != null)
        {
          string dbContentType = PoliticiansImagesData
            .GetProfileOriginalContentTypeByPoliticianKey(politicianKey);
          string actualContentType = Images.GetContentType(blob);
          if (dbContentType != actualContentType)
            errors.Add(String.Format("PoliticianKey: {0} Database: {1} Actual: {2}",
              politicianKey, 
              dbContentType == null ? "<null>" : dbContentType, 
              actualContentType == null ? "<null>" : actualContentType));
        }
      }
    }

    private void SetHeadshotContentTypeButton_Click(object sender, EventArgs e)
    {
      List<string> errors = new List<string>();
      SetHeadshotContentTypeButton.Enabled = false;
      int count = 0;
      var table = PoliticiansImagesData.GetAllData();
      foreach (var row in table)
      {
        count++;
        string politicianKey = row.PoliticianKey;
        byte[] blob =
          PoliticiansImagesBlobs.GetHeadshotOriginalByPoliticianKey(politicianKey);
        if (blob != null)
        {
          string contentType = Images.GetContentType(blob);
          if (contentType == null)
            errors.Add(politicianKey);
          else
            PoliticiansImagesData.UpdateHeadshotContentTypeByPoliticianKey(
              contentType, politicianKey);
        }
      }
    }

    private void SetProfileContentTypeButton_Click(object sender, EventArgs e)
    {
      List<string> errors = new List<string>();
      SetProfileContentTypeButton.Enabled = false;
      int count = 0;
      var table = PoliticiansImagesData.GetAllData();
      foreach (var row in table)
      {
        count++;
        string politicianKey = row.PoliticianKey;
        byte[] blob =
          PoliticiansImagesBlobs.GetProfileOriginalByPoliticianKey(politicianKey);
        if (blob != null)
        {
          string contentType = Images.GetContentType(blob);
          if (contentType == null)
            errors.Add(politicianKey);
          else
            PoliticiansImagesData.UpdateProfileOriginalContentTypeByPoliticianKey(
              contentType, politicianKey);
        }
      }
    }
  }
}
