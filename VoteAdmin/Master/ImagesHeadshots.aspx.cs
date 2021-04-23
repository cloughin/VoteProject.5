using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using DB.Vote;
using DB.VoteLog;
using static System.String;

namespace Vote.Master
{
  public partial class ImagesHeadshots : SecurePage
  {
    #region legacy

    public const int ImageSize15Headshot = 15;
    public const int ImageSize20Headshot = 20;
    public const int ImageSize25Headshot = 25;
    public const int ImageSize35Headshot = 35;
    public const int ImageSize50Headshot = 50;
    public const int ImageSize75Headshot = 75;
    public const int ImageSize100Headshot = 100;
    public const int ImageSize200Profile = 200;
    public const int ImageSize300Profile = 300;

    public static string Ok(string msg)
    {
      return "<span class=" + "\"" + "MsgOk" + "\"" + ">" + "SUCCESS!!! " + msg + "</span>";
    }

    public static string Fail(string msg)
    {
      return "<span class=" + "\"" + "MsgFail" + "\"" + ">" + "****FAILURE**** " + msg +
        "</span>";
    }

    public static string Message(string msg)
    {
      return "<span class=" + "\"" + "Msg" + "\"" + ">" + msg + "</span>";
    }

    public static string GetPoliticianImageUrl(string politicianKey, int imageWidth,
      int noPhotoWidth = 0)
    {
      var url = Empty;
      url += @"/Image.aspx";
      url += "?Id=";
      url += politicianKey;

      url += "&Col=";
      var columnName = ImageManager.GetPoliticianImageColumnNameByWidth(imageWidth);
      url += columnName;

      // This handles the NoPhoto case without having to hit the db for every img href
      // on the page
      if (noPhotoWidth <= 0) return url;
      if (noPhotoWidth == imageWidth) url += "&Def=1"; // the short form
      else
      {
        url += "&Def=";
        var defaultColumnName =
          ImageManager.GetPoliticianImageColumnNameByWidth(noPhotoWidth);
        url += defaultColumnName;
      }
      return url;
    }

    public static string Politician_PartyCode(string politicianKey)
    {
      return Politician_PartyCode(GetPageCache(), politicianKey);
    }

    public static string Politician_PartyCode(PageCache cache, string politicianKey)
    {
      var partyKey = cache.Politicians.GetPartyKey(politicianKey);
      return IsNullOrEmpty(partyKey)
        ? Empty
        : cache.Parties.GetPartyCode(partyKey);
    }

    public static string Url_Image_Politician_Original(string politicianKey)
    {
      var url = Empty;
      url += @"/Image.aspx?Id=";
      url += politicianKey;
      url += "&Col=ProfileOriginal";
      return url;
    }

    #endregion legacy

    #region Data members

    // for ViewState
    private const string ViewStateKeysKey = "PoliticianKeys";

    private const string ViewStateIndexKey = "Index";
    private const string ViewStateOutOfDateKey = "OutOfDate";
    private const string ViewStateStateCodeKey = "StateCode";
    private const string ViewStateOfficeLevelKey = "OfficeLevel";

    #endregion Data members

    #region Private methods

    private void ClearLabels()
    {
      LabelUploadDate.Text = Empty;
      //UploadDatePlaceHolder.Controls.Clear();
      LabelPoliticianKey.Text = Empty;
      LabelOfficeLevel.Text = Empty;
      LabelPolitician.Text = Empty;
      LabelOffice.Text = Empty;
      LabelRows.Text = Empty;
    }

    private void ClearStateOptionStatus()
    {
      ViewState.Remove(ViewStateKeysKey);
      ViewState.Remove(ViewStateOutOfDateKey);
      ViewState.Remove(ViewStateStateCodeKey);
      ViewState.Remove(ViewStateOfficeLevelKey);
      ViewState.Remove(ViewStateIndexKey);
    }

    private void EnableControls()
    {
      switch (RadioButtonListProcessMethod.SelectedValue)
      {
        case "New":
          TextBoxStateCode.Enabled = false;
          TextBoxOfficeLevel.Enabled = false;
          CheckBoxOutOfDateOnly.Enabled = false;
          TextBoxPoliticianKey.Enabled = false;
          break;

        case "State":
          TextBoxStateCode.Enabled = true;
          TextBoxOfficeLevel.Enabled = true;
          CheckBoxOutOfDateOnly.Enabled = true;
          TextBoxPoliticianKey.Enabled = false;
          break;

        case "Single":
          TextBoxStateCode.Enabled = false;
          TextBoxOfficeLevel.Enabled = false;
          CheckBoxOutOfDateOnly.Enabled = false;
          TextBoxPoliticianKey.Enabled = true;
          break;
      }
    }

    private void GetNextPoliticianForNewOption()
    {
      var lastDate = Str.ToDateTime(LabelUploadDate.Text, VoteDb.DateTimeMax);
      var lastKey = LabelPoliticianKey.Text;
      var lastOfficeLevel = Str.ToIntOr0(LabelOfficeLevel.Text);

      var list =
        PoliticianImagesInfo.GetNextOutOfDateHeadshot(lastOfficeLevel, lastDate, lastKey);

      if (list.Count == 0) throw new VoteUIException("No more out-of-date headshots.");

      SetLabelsFromList(list);
    }

    private void GetNextPoliticianForStateOption()
    {
      try
      {
        var politicianKeys = ViewState[ViewStateKeysKey] as List<string>;
        int politicianIndex;
        if (ViewState[ViewStateIndexKey] is int i1)
          politicianIndex = i1;
        else politicianIndex = int.MinValue; // invalid indicator

        // If not found, its the right time to generate the list
        if (politicianKeys == null || politicianIndex == int.MinValue)
        {
          // Edit the StateCode
          var stateCode = TextBoxStateCode.Text.Trim().ToUpperInvariant();
          TextBoxStateCode.Text = stateCode;
          if (stateCode == Empty)
            throw new VoteUIException("A State Code is required.");
          if (!StateCache.IsValidStateCode(stateCode))
            throw new VoteUIException("The State Code is not valid.");

          // Edit the OfficeLevel
          var officeLevel = -1; // default of -1 means not used
          var officeLevelText = TextBoxOfficeLevel.Text.Trim();
          if (officeLevelText != Empty)
            if (!int.TryParse(officeLevelText, out officeLevel))
              throw new VoteUIException("The Office Level is not valid.");

          var onlyOutOfDate = CheckBoxOutOfDateOnly.Checked;

          var stateList =
            PoliticianImagesInfo.GetStateHeadshots(stateCode, officeLevel, onlyOutOfDate);

          // Create the StateKeys object and save it in the ViewState
          politicianIndex = -1;
          politicianKeys = stateList.Select(i => i.PoliticianKey).ToList();
          ViewState[ViewStateKeysKey] = politicianKeys;
          ViewState[ViewStateOutOfDateKey] = onlyOutOfDate;
          ViewState[ViewStateStateCodeKey] = stateCode;
          ViewState[ViewStateOfficeLevelKey] = officeLevel;
          SetRowCountForStateOption();
        }

        // Advance to the next key and fetch if there are any more
        politicianIndex++;
        if (politicianIndex >= politicianKeys.Count)
          throw new VoteUIException(
            "No more selected headshots for this selection criteria.");

        var item =
          PoliticianImagesInfo
            .GetHeadshotDataForPolitician(politicianKeys[politicianIndex]);
        // Update the ViewState with the new index
        ViewState[ViewStateIndexKey] = politicianIndex;

        SetLabelsFromItem(item);
      }
      catch
      {
        // On failure don't leave possibly incomplete state option status
        ClearStateOptionStatus();
        throw; // Exception handled upstream
      }
    }

    private void GetPoliticianForSingleOption()
    {
      var politicianKey = TextBoxPoliticianKey.Text.Trim();

      if (IsNullOrWhiteSpace(politicianKey))
        throw new VoteUIException("A Politician Key is required.");

      if (!Politicians.PoliticianKeyExists(politicianKey))
        throw new VoteUIException(politicianKey + " is not a valid PoliticianKey.");

      var item = PoliticianImagesInfo.GetHeadshotDataForPolitician(politicianKey);
      LabelRows.Text = (item == null ? 0 : 1).ToString(CultureInfo.InvariantCulture);

      if (item == null)
        throw new VoteUIException("Politician for PoliticianKey not found.");

      SetLabelsFromItem(item);
      TextBoxPoliticianKey.Text = Empty; // only clear if valid
    }

    private void HandleException(Exception ex)
    {
      // We normally only catch exceptions in event handlers (ALL event handlers, 
      // unless they absolutely cannot fail). Deeper code that needs to do cleanup
      // after an exception should use either try/finally or try/catch with the
      // caught exception rethrown.

      string message;

      try
      {
        ClearLabels();
        message = Fail(ex.Message);
        // We don't log routine UI exceptions
        if (!(ex is VoteUIException)) //db.Log_Error_Admin(ex);
          LogException("ImagesHeadshots", ex);
      }
      catch (Exception ex2)
      {
        // Don't put anything in here that could possibly
        // throw an exception. In the rare event that this block
        // executes, we use ex.ToString() instead of ex.Message
        // to make sure we capture and report the stack trace.
        message = "Unexpected failure in exception handler: " + ex2 + Environment.NewLine +
          "Original exception: " + ex;
      }

      if (ex.Message.StartsWith("No more"))
      {
        NoMore.InnerHtml = ex.Message;
        Content.Visible = false;
      }
      else Msg.Text = message;
    }

    private void MarkProcessed()
    {
      // No action for Single or New
      switch (RadioButtonListProcessMethod.SelectedValue)
      {
        case "State":
          var politicianKeys = ViewState[ViewStateKeysKey] as List<string>;
          var politicianIndex = -1;
          if (ViewState[ViewStateIndexKey] is int i)
            politicianIndex = i;
          if (politicianKeys != null && politicianIndex >= 0 &&
            politicianIndex < politicianKeys.Count) politicianKeys[politicianIndex] = null;
          ViewState[ViewStateKeysKey] = politicianKeys;
          break;
      }
    }

    private void SetImageUrls()
    {
      var politicianKey = LabelPoliticianKey.Text.Trim();

      var latestLogInfo = LogDataChange.GetTwoLatestProfileImageInfos(politicianKey);
      var date = latestLogInfo.Count < 1
        ? PoliticiansImagesData.GetProfileOriginalDate(politicianKey, DefaultDbDate)
        : latestLogInfo[0].DateStamp;
      var dateTag = LocalDate.AsString(date, "M/D/YYYY h:mm:ss A");
      ProfileImageHeading.InnerHtml = latestLogInfo.Count < 1
        ? $"Newly-Uploaded Profile:<br />{dateTag}"
        : $"Newly-Uploaded Profile<br />({dateTag} by {latestLogInfo[0].UserName}):";

      byte[] loggedBlob = null;
      if (latestLogInfo.Count >= 2)
        loggedBlob = LatestLoggedImagePage.GetLoggedImageByDate(politicianKey,
          new DateTime(latestLogInfo[1].DateStamp.Ticks));
      //if (latestLogInfo.Count < 2)
      if (loggedBlob == null)
      {
        LoggedImage.Visible = false;
        LoggedImageSize.Visible = false;
        LoggedImageHeading.InnerHtml = "No logged profile available";
        LoggedImageHeading.Style.Add(HtmlTextWriterStyle.Color, "red");
        ButtonRevertToLog.Visible = false;
      }
      else
      {
        LoggedImage.Visible = true;
        LoggedImageSize.Visible = true;
        LoggedImageHeading.InnerHtml =
          $"Most Recent Logged Profile<br/>({LocalDate.AsString(latestLogInfo[1].DateStamp, "M/D/YYYY h:mm:ss A")}" +
          $" by {latestLogInfo[1].UserName}):";
        LoggedImageHeading.Style.Add(HtmlTextWriterStyle.Color, "black");
        ButtonRevertToLog.Visible = true;
        LoggedImage.ImageUrl = "/master/latestloggedimage.aspx?id=" + politicianKey +
          "&ticks=" + latestLogInfo[1].DateStamp.Ticks;
        //var loggedBlob =
        //  LatestLoggedImagePage.GetLoggedImageByDate(politicianKey,
        //  new DateTime(latestLogInfo[1].DateStamp.Ticks));
        var loggedImage = Image.FromStream(new MemoryStream(loggedBlob));
        LoggedImageSize.InnerHtml = $"{loggedImage.Width}x{loggedImage.Height}";
      }

      OriginalImage.ImageUrl =
        InsertNoCacheIntoUrl(Url_Image_Politician_Original(politicianKey) + "&Def=1");

      var blob = ImageManager.GetPoliticianImage(politicianKey, "ProfileOriginal",
        "ProfileOriginal", true);
      var image = Image.FromStream(new MemoryStream(blob));
      OriginalImageSize.InnerHtml = $"{image.Width}x{image.Height}";

      Image15.ImageUrl = InsertNoCacheIntoUrl(
        GetPoliticianImageUrl(politicianKey, ImageSize15Headshot) + "&Def=1");

      Image20.ImageUrl = InsertNoCacheIntoUrl(
        GetPoliticianImageUrl(politicianKey, ImageSize20Headshot) + "&Def=1");

      Image25.ImageUrl = InsertNoCacheIntoUrl(
        GetPoliticianImageUrl(politicianKey, ImageSize25Headshot) + "&Def=1");

      Image35.ImageUrl = InsertNoCacheIntoUrl(
        GetPoliticianImageUrl(politicianKey, ImageSize35Headshot) + "&Def=1");

      Image50.ImageUrl = InsertNoCacheIntoUrl(
        GetPoliticianImageUrl(politicianKey, ImageSize50Headshot) + "&Def=1");

      Image75.ImageUrl = InsertNoCacheIntoUrl(
        GetPoliticianImageUrl(politicianKey, ImageSize75Headshot) + "&Def=1");

      Image100.ImageUrl =
        InsertNoCacheIntoUrl(GetPoliticianImageUrl(politicianKey, ImageSize100Headshot) +
          "&Def=1");

      Image200.ImageUrl =
        InsertNoCacheIntoUrl(GetPoliticianImageUrl(politicianKey, ImageSize200Profile) +
          "&Def=1");

      Image300.ImageUrl =
        InsertNoCacheIntoUrl(GetPoliticianImageUrl(politicianKey, ImageSize300Profile) +
          "&Def=1");
    }

    private void SetLabelsFromList(IList<PoliticianImagesInfo> list)
    {
      if (list != null && list.Count > 0) SetLabelsFromItem(list[0]);
    }

    private void SetLabelsFromItem(PoliticianImagesInfo item)
    {
      if (item == null) return;
      LabelUploadDate.Text =
        item.ProfileOriginalDate.ToString(CultureInfo.InvariantCulture);
      //UploadDatePlaceHolder.Controls.Clear();
      //new LocalDate(item.ProfileOriginalDate, "M/D/YYYY h:mm:ss A")
      //  .AddTo(UploadDatePlaceHolder);
      LabelPoliticianKey.Text = item.PoliticianKey;
      LabelOfficeLevel.Text = item.OfficeLevel.ToString(CultureInfo.InvariantCulture);
      LabelPolitician.Text = Politicians.GetFormattedName(item.PoliticianKey) + " - " +
        Politician_PartyCode(item.PoliticianKey);
      var officeKey = GetPageCache().Politicians.GetOfficeKey(item.PoliticianKey);
      LabelOffice.Text = Offices.GetStateCodeFromKey(officeKey) + " - " +
        Offices.FormatOfficeName(officeKey);
    }

    private void SetRowCountForNewOption()
    {
      LabelRows.Text = PoliticiansImagesData.CountOutOfDateHeadshots()
        .ToString(CultureInfo.InvariantCulture);
    }

    private void SetRowCountForStateOption()
    {
      var politicianKeys = ViewState[ViewStateKeysKey] as List<string>;
      var count = politicianKeys?.Count(key => key != null) ?? 0;
      LabelRows.Text = count.ToString(CultureInfo.InvariantCulture);
    }

    private void UpdateRowCountAfterChange()
    {
      // No action for Single
      switch (RadioButtonListProcessMethod.SelectedValue)
      {
        case "New":
          SetRowCountForNewOption();
          break;

        case "State":
          SetRowCountForStateOption();
          break;
      }
    }

    #endregion Private methods

    #region Event handlers

    protected override void OnPreRender(EventArgs e)
    {
      try
      {
        base.OnPreRender(e); // <-- this must be there

        var isPoliticianKeyValid = !IsNullOrWhiteSpace(LabelPoliticianKey.Text);
        ButtonNoCrop.Enabled = isPoliticianKeyValid;
        ButtonAsIs.Enabled = isPoliticianKeyValid;

        SetImageUrls();
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    protected void ButtonAsIs_Click(object sender, EventArgs e)
    {
      try
      {
        var politicianKey = LabelPoliticianKey.Text.Trim();
        PoliticiansImagesData.GuaranteePoliticianKeyExists(politicianKey);
        PoliticiansImagesBlobs.GuaranteePoliticianKeyExists(politicianKey);
        PoliticiansImagesData.UpdateHeadshotDate(DateTime.UtcNow, politicianKey);
        CommonCacheInvalidation.ScheduleInvalidation("politicianimage", politicianKey);

        MarkProcessed();
        UpdateRowCountAfterChange();
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    protected void ButtonNext_Click(object sender, EventArgs e)
    {
      try
      {
        // Action based on selected mode
        switch (RadioButtonListProcessMethod.SelectedValue)
        {
          case "New":
            GetNextPoliticianForNewOption();
            break;

          case "State":
            GetNextPoliticianForStateOption();
            break;

          case "Single":
            GetPoliticianForSingleOption();
            break;
        }
        Msg.Text = Ok("Upload any desired headshot or profile images." +
          " Then click the Go to Next Image Button.");
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    protected void ButtonNoCrop_Click(object sender, EventArgs e)
    {
      try
      {
        var politicianKey = LabelPoliticianKey.Text;

        // We get the current profile original, convert it to a stream, then
        // use it as the HeadshotOriginal image
        var blob = PoliticiansImagesBlobs.GetProfileOriginal(politicianKey);
        if (blob == null) return;
        var uploadTime = DateTime.UtcNow;
        var stream = new MemoryStream(blob);

        ImageManager.UpdateAllPoliticianHeadshotImages(politicianKey, stream, uploadTime,
          out _);
        CommonCacheInvalidation.ScheduleInvalidation("politicianimage", politicianKey);

        MarkProcessed();
        UpdateRowCountAfterChange();
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    protected void ButtonUploadPicture_ServerClick(object sender, EventArgs e)
    {
      try
      {
        var postedFile = Request.Files[ImageFile.Name];
        var politicianKey = LabelPoliticianKey.Text;
        var uploadTime = DateTime.UtcNow;

        if (postedFile == null || postedFile.ContentLength == 0 ||
          IsNullOrEmpty(politicianKey)) return;
        ImageManager.UpdateAllPoliticianHeadshotImages(politicianKey,
          postedFile.InputStream, uploadTime, out _);
        CommonCacheInvalidation.ScheduleInvalidation("politicianimage", politicianKey);

        var memoryStream = new MemoryStream();
        postedFile.InputStream.Position = 0;
        postedFile.InputStream.CopyTo(memoryStream);
        postedFile.InputStream.Position = 0;
        var imageBlob = memoryStream.ToArray();
        //LogPoliticiansImagesHeadshot.Insert(politicianKey, imageBlob, uploadTime,
        //  UserSecurityClass, UserName, 0);
        LogImageHeadshotChange(politicianKey, null, imageBlob, uploadTime);

        MarkProcessed();
        UpdateRowCountAfterChange();

        Msg.Text = Ok("The headshot image for ID " + politicianKey + " was uploaded.");
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    protected void ButtonUploadPictureProfile_ServerClick(object sender, EventArgs e)
    {
      try
      {
        var postedFile = Request.Files[ImageFileProfile.Name];
        var politicianKey = LabelPoliticianKey.Text;
        var uploadTime = DateTime.UtcNow;

        if (postedFile == null || postedFile.ContentLength == 0) return;
        ImageManager.UpdatePoliticianProfileImages(politicianKey, postedFile.InputStream,
          uploadTime, out _);
        CommonCacheInvalidation.ScheduleInvalidation("politicianimage", politicianKey);

        // We only want to propagate the profile to the headshot if there
        // is no current Headshot -- we check Headshot100
        if (PoliticiansImagesBlobs.GetHeadshot100(politicianKey) == null)
          ImageManager.UpdateResizedPoliticianHeadshotImages(politicianKey,
            postedFile.InputStream, uploadTime, out _);

        var memoryStream = new MemoryStream();
        postedFile.InputStream.Position = 0;
        postedFile.InputStream.CopyTo(memoryStream);
        postedFile.InputStream.Position = 0;
        var imageBlob = memoryStream.ToArray();
        //LogPoliticiansImagesOriginal.Insert(politicianKey, imageBlob, uploadTime,
        //  UserSecurityClass, UserName, 0);
        LogImageChange(politicianKey, null, imageBlob, uploadTime);

        UpdateRowCountAfterChange();

        Msg.Text = Ok("The profile image for ID " + politicianKey + " was uploaded.");
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    protected void ButtonRevertToLog_ServerClick(object sender, EventArgs e)
    {
      try
      {
        var politicianKey = LabelPoliticianKey.Text;
        var latestLogDate =
          LogDataChange.GetSecondLatestProfileImageDate(politicianKey, out _);

        var logTime = new DateTime(latestLogDate.Ticks);
        var loggedBlob = LatestLoggedImagePage.GetLoggedImageByDate(politicianKey, logTime);
        if (latestLogDate.IsDefaultDate() || loggedBlob == null || loggedBlob.Length == 0)
          Msg.Text = Fail("No log profile image for ID " + politicianKey + " was found.");
        else
        {
          var now = DateTime.UtcNow;
          ImageManager.UpdatePoliticianProfileImages(politicianKey,
            new MemoryStream(loggedBlob), now, out _);
          CommonCacheInvalidation.ScheduleInvalidation("politicianimage", politicianKey);
          LogImageChange(politicianKey, null, loggedBlob, now);
          Msg.Text = Ok("The profile image for ID " + politicianKey +
            " was reverted to the most recent logged version.");
        }
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    protected void CheckBoxOutOfDateOnly_CheckedChanged(object sender, EventArgs e)
    {
      ClearStateOptionStatus();
    }

    protected void RadioButtonListProcessMethod_SelectedIndexChanged(object sender,
      EventArgs e)
    {
      try
      {
        // Clear inputs and current values and get first match
        TextBoxStateCode.Text = Empty;
        TextBoxOfficeLevel.Text = Empty;
        TextBoxPoliticianKey.Text = Empty;
        ClearLabels();
        ClearStateOptionStatus();
        EnableControls();

        switch (RadioButtonListProcessMethod.SelectedValue)
        {
          case "New":
            Msg.Text = Message("Begin processing");
            SetRowCountForNewOption();
            GetNextPoliticianForNewOption();
            break;

          case "State":
            Msg.Text = Message("Enter a StateCode and Office Level and click Next");
            LabelRows.Text = "0";
            break;

          case "Single":
            Msg.Text = Message("Enter the PoliticianKey");
            LabelRows.Text = "0";
            break;
        }
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    protected void TextBoxOfficeLevel_TextChanged(object sender, EventArgs e)
    {
      ClearStateOptionStatus();
    }

    protected void TextBoxStateCode_TextChanged(object sender, EventArgs e)
    {
      ClearStateOptionStatus();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      try
      {
        H1.InnerHtml = Page.Title = "Image Headshot Maintenance";
        NoMore.InnerHtml = Empty;
        if (IsPostBack) return;
        if (!IsSuperUser) HandleSecurityException();

        if (!IsNullOrWhiteSpace(QueryId))
        {
          var politicianKey = QueryId;
          TextBoxPoliticianKey.Text = politicianKey;
          LabelPoliticianKey.Text = politicianKey;
          RadioButtonListProcessMethod.SelectedValue = "Single";
          GetPoliticianForSingleOption();
        }
        else
        {
          SetRowCountForNewOption();
          GetNextPoliticianForNewOption();
        }

        EnableControls();
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    #endregion Event handlers

    public class PoliticianImagesInfo : IComparer<PoliticianImagesInfo>
      // Helper class that manages the info for each politician
    {
      public string PoliticianKey;
      public DateTime ProfileOriginalDate;
      private string _OfficeKey;
      public int OfficeLevel;

      private void ApplyAdditionalCoding(PageCache pageCache)
      {
        _OfficeKey = pageCache.Politicians.GetOfficeKey(PoliticianKey);
        if (Offices.IsValid(_OfficeKey))
          OfficeLevel = pageCache.Offices.GetOfficeLevel(_OfficeKey);
        else
        {
          if (IsNullOrEmpty(_OfficeKey)) _OfficeKey = "????";
          OfficeLevel = 999; // Indicates unknown
        }
      }

      private static List<PoliticianImagesInfo> ConvertTableToList(
        TypedTableBase<PoliticiansImagesDataRow> table)
      {
        var list = table.Select(row => new PoliticianImagesInfo
        {
          PoliticianKey = row.PoliticianKey,
          ProfileOriginalDate = row.ProfileOriginalDate
        }).ToList();
        return list;
      }

      public static List<PoliticianImagesInfo> GetNextOutOfDateHeadshot(int officeLevel,
        DateTime profileOriginalDate, string politicianKey)
      {
        var table = PoliticiansImagesData.GetDataForOutOfDateHeadshots();
        var list = ConvertTableToList(table);

        // Get OfficeKey and related data
        var tempCache = GetPageCache();
        foreach (var info in list) info.ApplyAdditionalCoding(tempCache);

        // Eliminate any that are prior to the current processing point
        list = list
          .Where(item => item.IsGreater(officeLevel, profileOriginalDate, politicianKey))
          .OrderBy(item => item, new PoliticianImagesInfo()).ToList();

        return list;
      }

      public static IEnumerable<PoliticianImagesInfo> GetStateHeadshots(string stateCode,
        int officeLevel, bool onlyOutOfDate)
      {
        var filterOfficeLevel = officeLevel >= 0;
        var table = onlyOutOfDate
          ? PoliticiansImagesData.GetDataByStateForOutOfDateHeadshots(stateCode)
          : PoliticiansImagesData.GetDataByState(stateCode);
        var list = ConvertTableToList(table);

        // Get OfficeKey and related data
        var tempCache = GetPageCache();
        foreach (var info in list) info.ApplyAdditionalCoding(tempCache);

        // Fiter and sort
        list = list.Where(item => !filterOfficeLevel || item.OfficeLevel == officeLevel)
          .OrderBy(item => item, new PoliticianImagesInfo()).ToList();

        return list;
      }

      public static PoliticianImagesInfo GetHeadshotDataForPolitician(string politicianKey)
      {
        var table = PoliticiansImagesData.GetData(politicianKey);
        if (table.Count == 0) return null;
        var info = ConvertTableToList(table)[0];

        // Get OfficeKey and related data
        var tempCache = GetPageCache();
        info.ApplyAdditionalCoding(tempCache);

        return info;
      }

      //private bool IsGreater(int officeLevel, DateTime profileOriginalDate,
      //  string politicianKey)
      //{
      //  return OfficeLevel > officeLevel || OfficeLevel == officeLevel &&
      //  (ProfileOriginalDate < profileOriginalDate ||
      //    ProfileOriginalDate == profileOriginalDate &&
      //    PoliticianKey.IsGtIgnoreCase(politicianKey));
      //}

      private bool IsGreater(int officeLevel, DateTime profileOriginalDate,
        string politicianKey)
      {
        return OfficeLevel > officeLevel || OfficeLevel == officeLevel &&
        (ProfileOriginalDate > profileOriginalDate ||
          ProfileOriginalDate == profileOriginalDate &&
          PoliticianKey.IsGtIgnoreCase(politicianKey));
      }

      #region IComparer<PoliticianImagesInfo> Members

      public int Compare(PoliticianImagesInfo x, PoliticianImagesInfo y)
      {
        if (x == null) return y == null ? 0 : -1;
        if (y == null) return 1;
        if (x.OfficeLevel > y.OfficeLevel) return 1;
        if (x.OfficeLevel < y.OfficeLevel) return -1;
        //if (x.ProfileOriginalDate > y.ProfileOriginalDate) return -1;
        //if (x.ProfileOriginalDate < y.ProfileOriginalDate) return 1;
        if (x.ProfileOriginalDate > y.ProfileOriginalDate) return 1;
        if (x.ProfileOriginalDate < y.ProfileOriginalDate) return -1;
        if (x.PoliticianKey.IsGtIgnoreCase(y.PoliticianKey)) return 1;
        if (x.PoliticianKey.IsLtIgnoreCase(y.PoliticianKey)) return -1;
        return 0;
      }

      #endregion
    }
  }
}