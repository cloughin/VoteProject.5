using System;
using DB.Vote;
using DB.VoteLog;

namespace Vote
{
  public class SecurePoliticianPage : SecurePage
  {
    #region Private

    private static string GetPoliticianFolderPageUrl(string pageName,
      params string[] queryParametersAndValues)
    {
      if ((queryParametersAndValues.Length == 0) &&
        (UserSecurityClass != PoliticianSecurityClass))
      {
        var id = PoliticianKeyFromSecurePoliticianPage;
        if (!string.IsNullOrWhiteSpace(id))
          queryParametersAndValues = new[] {"id", id};
      }
      return QueryStringCollection.FromPairs(queryParametersAndValues)
        .AddToPath("/politician/" + pageName + ".aspx");
    }

    private static string GetPoliticianPublicPageUrl(string pageName)
    {
      var politicianKey = UserSecurityClass == PoliticianSecurityClass
        ? UserPoliticianKey
        : PoliticianKeyFromSecurePoliticianPage;
      var url = "/" + pageName + ".aspx?id=" + politicianKey;
      return InsertNoCacheIntoUrl(url);
    }

    private static string PoliticianKeyFromSecurePoliticianPage
    {
      get
      {
        var politicianPage = GetPage<SecurePoliticianPage>();
        if (politicianPage == null) // bad call
          throw new VoteException(
            "GetPoliticianFolderPageUrl: cannot determine PoliticianKey");
        return politicianPage.PoliticianKey;
      }
    }

    internal void UpdatePoliticianAnswer(string questionKey, int sequence, string issueKey,
      string newValue, string source, DateTime dateStamp, string youTubeUrl,
      string youTubeDescription, TimeSpan youTubeRunningTime, string youTubeSource,
      string youTubeSourceUrl, DateTime youTubeDate)
    {
      if (string.IsNullOrWhiteSpace(newValue) && string.IsNullOrWhiteSpace(youTubeUrl))
        // Just delete and be done with it
        Answers.DeleteByPoliticianKeyQuestionKeySequence(PoliticianKey, questionKey, sequence);
      else
      {
        var table = Answers.GetDataByPoliticianKeyQuestionKeySequence(PoliticianKey,
          questionKey, sequence);
        AnswersRow row;
        if (table.Count == 0)
        {
          row = table.NewRow();
          row.Sequence = sequence;
        }
        else
          row = table[0];
        row.PoliticianKey = PoliticianKey;
        row.QuestionKey = questionKey;
        row.StateCode = Politicians.GetStateCodeFromKey(PoliticianKey);
        row.IssueKey = issueKey;
        row.Answer = newValue;
        row.Source = source;
        row.DateStamp = dateStamp;
        row.UserName = UserName;
        row.YouTubeUrl = youTubeUrl;
        row.YouTubeDescription = youTubeDescription;
        row.YouTubeRunningTime = youTubeRunningTime;
        row.YouTubeSource = youTubeSource;
        row.YouTubeSourceUrl = youTubeSourceUrl;
        row.YouTubeDate = youTubeDate;
        row.YouTubeRefreshTime = string.IsNullOrWhiteSpace(youTubeUrl)
          ? DefaultDbDate
          : DateTime.UtcNow;
        row.YouTubeAutoDisable = null;
        if (table.Count == 0)
          table.AddRow(row);
        Answers.UpdateTable(table);
      }
    }

    #endregion Private

    #region Protected

    #region ReSharper disable

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable VirtualMemberNeverOverriden.Global
    // ReSharper disable UnusedMember.Global

    #endregion ReSharper disable

    protected void LogImageChange(byte[] oldImageBlob, byte[] newImageBlob)
    {
      LogImageChange(oldImageBlob, newImageBlob, DateTime.UtcNow);
    }

    protected void LogImageChange(byte[] oldImageBlob, byte[] newImageBlob,
      DateTime uploadTime)
    {
      LogDataChange.LogUpdate(PoliticiansImagesBlobs.Column.ProfileOriginal,
        oldImageBlob, newImageBlob, UserName, UserSecurityClass, uploadTime,
        PoliticianKey);
    }

    internal void LogPoliticianAnswerChange(string questionKey, int sequence, string oldAnswer,
      string newAnswer, string source)
    {
      LogDataChange.LogUpdate(Answers.Column.Answer, oldAnswer, newAnswer, source,
        UserName, UserSecurityClass, DateTime.UtcNow, PoliticianKey, questionKey, sequence);
    }

    protected void LogPoliticiansDataChange(string column, string oldValue,
      string newValue)
    {
      LogDataChange.LogUpdate(Politicians.TableName, column, oldValue, newValue,
        UserName, UserSecurityClass, DateTime.UtcNow, PoliticianKey);
    }

    #region ReSharper restore

    // ReSharper restore UnusedMember.Global
    // ReSharper restore VirtualMemberNeverOverriden.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion ReSharper restore

    #endregion Protected

    #region Public

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable MemberCanBeProtected.Global
    // ReSharper disable UnusedMember.Global

    #region Public Properties

    public static string AjaxImageUploaderUrl => GetPoliticianFolderPageUrl("ajaximageuploader");

    public string CurrentElectionKey { get; private set; }

    public static string IntroPageUrl => GetPoliticianPublicPageUrl("intro");

    public static string MainPageUrl => GetPoliticianFolderPageUrl("main");

    public string NoCacheImageProfile200Url
      => GetPoliticianImageUrl(PoliticianKey, "Profile200", true);

    public string NoCacheImageProfile300Url
      => GetPoliticianImageUrl(PoliticianKey, "Profile300", true);

    public string OfficeAndStatus { get; private set; }

    public static string PoliticianIssuePageUrl => GetPoliticianPublicPageUrl("politicianissue");

    public string PoliticianKey { get; private set; }

    public bool PoliticianKeyExists { get; private set; }

    public string PoliticianName { get; private set; }

    public static string UpdateIntroPageUrl => GetPoliticianFolderPageUrl("updateintro");

    public static string UpdateIssuesPageUrl => GetPoliticianFolderPageUrl("updateissues");

    #endregion Public Properties

    #region Public Methods

    public static string GetPoliticianName()
    {
      var page = GetPage<SecurePoliticianPage>();
      return page == null ? string.Empty : page.PoliticianName;
    }

    public static string GetPoliticianKey()
    {
      var page = GetPage<SecurePoliticianPage>();
      return page == null ? string.Empty : page.PoliticianKey;
    }

    public static bool GetPoliticianKeyExists()
    {
      var page = GetPage<SecurePoliticianPage>();
      return page?.PoliticianKeyExists == true;
    }

    public static string GetUpdateIntroPageUrl(string politicianId)
    {
      return GetPoliticianFolderPageUrl("UpdateIntro", "Id", politicianId);
    }

    public static string GetUpdateIssuesPageUrl(string politicianId)
    {
      return GetPoliticianFolderPageUrl("UpdateIssues", "Id", politicianId);
    }

    #endregion Public Methods

    // ReSharper restore UnusedMember.Global
    // ReSharper restore MemberCanBeProtected.Global
    // ReSharper restore MemberCanBePrivate.Global

    #endregion Public

    #region Event handlers and overrides

    protected override void OnLoad(EventArgs e)
    {
      // Skip OnLoad processing if key is missing, handled in OnPreRender
      if (PoliticianKeyExists)
        base.OnLoad(e);
    }

    protected override void OnPreLoad(EventArgs e)
    {
      base.OnPreLoad(e);
      PoliticianKey = ViewStatePoliticianKey;
      PoliticianKeyExists = Politicians.PoliticianKeyExists(PoliticianKey);
      if (PoliticianKeyExists)
      {
        PoliticianName = PageCache.Politicians.GetPoliticianName(PoliticianKey);
        OfficeAndStatus = PageCache.Politicians.GetOfficeAndStatus(PoliticianKey);
        CurrentElectionKey =
          PageCache.Politicians.GetFutureViewableElectionKey(PoliticianKey);
      }
      else if (!IsSignedIn) // if not signed in, dump to sign in pageName
        RedirectToSignInPage();
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);

      if (PoliticianKeyExists) return;
      var mainContent = Master.MainContentControl;
      mainContent.Controls.Clear();
      var text = string.IsNullOrWhiteSpace(PoliticianKey)
        ? "PoliticianKey is missing"
        : "PoliticianKey [" + PoliticianKey + "] is invalid";
      new HtmlP {InnerHtml = text}.AddTo(mainContent,
        "missing-key");
    }

    #endregion Event handlers and overrides
  }
}