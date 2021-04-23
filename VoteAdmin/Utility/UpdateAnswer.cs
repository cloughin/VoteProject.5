using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DB;
using DB.Vote;
using DB.VoteLog;
using Jayrock.Json.Conversion;
using Vote.Controls;
using static System.String;

namespace Vote
{
  public class UpdateAnswer
  {
    private IList<IGrouping<string, DataRow>> _DataTable;
    private readonly Usage _Usage;

    private const string MonitorPrefix = "mc";
    private const string MonitorTabPrefix = "mt";
    private const string MonitorSuperTabPrefix = "ms";

    public enum Usage
    {
      ForIssues,
      ForIntroReasons,
      ForIntroBio,
      ForAll
    }

    // must be public for JSON conversion
    // ReSharper disable once MemberCanBePrivate.Global
    public class OneResponse
    {
      // ReSharper disable NotAccessedField.Global
      public int QuestionId;
      public string Answer;
      public string Source;
      public string Date;
      public string YouTubeUrl;
      public string YouTubeSource;
      public string YouTubeSourceUrl;
      public string YouTubeDescription;
      public string YouTubeRunningTime; 
      public string YouTubeDate;
      public string YouTubeAutoDisable;
      public bool YouTubeFromCandidate;
      //public string FacebookVideoUrl;
      //public string FacebookVideoDescription;
      //public string FacebookVideoRunningTime;
      //public string FacebookVideoDate;
      //public string FacebookVideoAutoDisable;
      public int Sequence;
      // ReSharper restore NotAccessedField.Global
    }

    public UpdateAnswer(Usage usage)
    {
      _Usage = usage;
    }

    protected static FeedbackContainerControl LoadFeedbackContainerControl()
    {
      var page = VotePage.GetPage<Page>();
      return
        page.LoadControl("/controls/feedbackcontainer.ascx") as
          FeedbackContainerControl;
    }

    private static HtmlContainerControl CreateFeedbackWithIe7Floater(
      string id, string className)
    {
      var floater = new HtmlDiv { EnableViewState = false };
      floater.AddCssClasses("feedback-floater-for-ie7");
      var feedback = LoadFeedbackContainerControl();
      feedback.EnableViewState = false;
      feedback.CssClass = className;
      floater.Controls.Add(feedback);
      if (id != null)
        feedback.ID = id;
      return floater;
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static HtmlContainerControl AddFeedbackWithIe7Floater(
      Control parent, string id, string className)
    {
      var feedback = CreateFeedbackWithIe7Floater(id, className);
      parent.Controls.Add(feedback);
      return feedback;
    }

    private static void CreateActionMenu(HtmlControl menuContainer, ICollection<DataRow> responses,
      bool isNew)
    {
      menuContainer.Controls.Clear();
      new HtmlSpan { InnerText = "Select action:" }.AddTo(menuContainer);
      var dropdown = new HtmlSelect().AddTo(menuContainer);
      var edit = dropdown.AddItem("Edit this response", "edit");
      var add =  dropdown.AddItem("Add another response to this question", "add");
      var view = dropdown.AddItem("View or edit other responses to this question", "view");
      if (responses.Count == 0)
        menuContainer.AddCssClasses("hidden");
      else
        menuContainer.RemoveCssClass("hidden");
      if (isNew && responses.Count > 0)
      {
        edit.Attributes.Add("disabled", "disabled");
        add.Attributes.Add("selected", "selected");
      }
      else
      {
        edit.Attributes.Add("selected", "selected");
        if (responses.Count < 2)
          view.Attributes.Add("disabled", "disabled");
      }
    }

    public void CreateControls(HtmlContainerControl parent, IList<DataRow> responses,
      MonitorInstance monitor, Dictionary<string, string> alternateHeadings = null)
    {
      var row = responses.First();
      var responsesAsDataRow =
        responses.Where(r => r.SequenceOrNull() != null).ToList();
      parent.AddCssClasses("answer-panel " +
        (_Usage == Usage.ForIssues ? "issues-answer-panel " : "intro-answer-panel ") +
        (SecurePage.IsPoliticianUser ? "politician-answer-panel" : "master-answer-panel"));

      // The panel title
      var heading = SecurePage.AddContainer(parent, null, null);
      var headingText =
        alternateHeadings != null && alternateHeadings.ContainsKey(row.QuestionKey())
          ? alternateHeadings[row.QuestionKey()]
          : row.Question();
      SecurePage.AddHeading(heading, 4, null, null, headingText);
      SecurePage.Center(heading, true, true, "heading");
      if (_Usage == Usage.ForIssues) heading.Visible = false;

      // Add undo button as next sibling to heading
      var undo = SecurePage.CreateUndoButton("Undo" + row.QuestionKey(),
        monitor.GetUndoClass(null),
        $"Revert \"{row.Question()}\" to the latest saved version");
      heading.AddAfter(undo);

      // Same for clear button
      var clear = SecurePage.CreateClearButton("Clear" + row.QuestionKey(),
        monitor.GetClearClass(null), $"Clear \"{row.Question()}\"");
      heading.AddAfter(clear);

      var updatePanel =
        SecurePage.AddAjaxUpdatePanel(parent, "UpdatePanel" + row.QuestionKey());

      var container = SecurePage.AddContainer(updatePanel.ContentTemplateContainer,
        "Container" + row.QuestionKey(),
        monitor.GetContainerClass("update-all updated answer-container"));

      SecurePage.AddHiddenField(container, "Description" + row.QuestionKey(),
        monitor.GetDescriptionClass(null, "answer"), row.Question());

      SecurePage.AddHiddenField(container, "SubTab" + row.QuestionKey(),
        "subtab subtab-" + row.QuestionKey());

      var hasNoResponses = row.SequenceOrNull() == null;

      var sequenceItem = SecurePage.AddHiddenField(container, "Sequence" + row.QuestionKey(),
        "answer-sequence",
        hasNoResponses ? "?" : row.SequenceOrNull()?.ToString(CultureInfo.InvariantCulture));

      SetResponseData(responsesAsDataRow, row, sequenceItem);

      // the action menu
      var menuContainer =
        SecurePage.AddContainer(container, "Action" + row.QuestionKey(), "action-menu");
      CreateActionMenu(menuContainer, responsesAsDataRow, row.SequenceOrNull() == null);

      new HtmlP
      {
        InnerHtml =
          "To completely delete a response (Text, YouTube and Facebook Video), use the Clear button (red <span>X</span> upper right) then Update."
      }.AddTo(container, "delete-message");

      var subTabs =
        new HtmlDiv
        {
          ID = "answer-subtabs-" + row.QuestionKey(),
          ClientIDMode = ClientIDMode.Static
        }.AddTo(container, "answer-sub-tabs shadow");
      var subTabsTabs = new HtmlUl().AddTo(subTabs, "htabs unselectable");

      var textSubTab = new HtmlLi {EnableViewState = false}.AddTo(subTabsTabs, "tab htab");
      new HtmlAnchor
      {
        HRef = "#tab-textanswer-" + row.QuestionKey(),
        InnerHtml = "Text<br />Response",
        EnableViewState = false
      }.AddTo(textSubTab);

      var youTubeSubTab =
        new HtmlLi {EnableViewState = false}.AddTo(subTabsTabs, "tab htab");
      new HtmlAnchor
      {
        HRef = "#tab-youtubeanswer-" + row.QuestionKey(),
        InnerHtml = "YouTube<br />Response",
        EnableViewState = false
      }.AddTo(youTubeSubTab);

      if (VotePage.AllowFacebookVideos)
      {
        var facebookVideoSubTab =
          new HtmlLi {EnableViewState = false}.AddTo(subTabsTabs, "tab htab");
        new HtmlAnchor
        {
          HRef = "#tab-facebookvideoanswer-" + row.QuestionKey(),
          InnerHtml = "Facebook Video<br />Response",
          EnableViewState = false
        }.AddTo(facebookVideoSubTab);
      }

      var textSubTabContent = new HtmlDiv
      {
        ID = "tab-textanswer-" + row.QuestionKey(),
        ClientIDMode = ClientIDMode.Static
      }.AddTo(subTabs);

      var youTubeSubTabContent = new HtmlDiv
      {
        ID = "tab-youtubeanswer-" + row.QuestionKey(),
        ClientIDMode = ClientIDMode.Static
      }.AddTo(subTabs);

      HtmlControl facebookVideoSubTabContent = null;
      if (VotePage.AllowFacebookVideos)
      {
        facebookVideoSubTabContent = new HtmlDiv
        {
          ID = "tab-facebookvideoanswer-" + row.QuestionKey(),
          ClientIDMode = ClientIDMode.Static
        }.AddTo(subTabs);
        
      }
      string moreClasses;
      switch (_Usage)
      {
        case Usage.ForIntroReasons:
          moreClasses = "mc-mt-reasons";
          break;

        case Usage.ForIntroBio:
          moreClasses = "mc-mt-bio2";
          break;

        default:
          var tabPrefix = MonitorPrefix + "-" + MonitorTabPrefix + "-issue" + row.IssueKey();
          var superTabPrefix = MonitorPrefix + "-" + MonitorSuperTabPrefix + "-igroup" +
            row.IssueGroupKey();
          moreClasses = tabPrefix + " " + superTabPrefix;
          break;
      }

      SecurePage.AddHiddenField(container, "HasValue" + row.QuestionKey(),
        monitor.GetDataClass(moreClasses, "hasvalue"),
        IsNullOrWhiteSpace(row.Answer()) && IsNullOrWhiteSpace(row.YouTubeUrl()) && 
        (IsNullOrWhiteSpace(row.FacebookVideoUrl()) || !VotePage.AllowFacebookVideos)
          ? Empty : "Y");

      CreateTextSubTabContent(textSubTabContent, row, moreClasses, monitor);
      CreateYouTubeSubTabContent(youTubeSubTabContent, row, moreClasses, monitor);
      if (VotePage.AllowFacebookVideos)
        CreateFacebookVideoSubTabContent(facebookVideoSubTabContent, row, moreClasses, monitor);

      new HtmlHr().AddTo(container);

      // feedback
      AddFeedbackWithIe7Floater(container, "Feedback" + row.QuestionKey(),
        monitor.GetFeedbackClass(null));

      // update button
      SecurePage.AddUpdateButtonInContainer(container, "Button" + row.QuestionKey(),
        "update-button", monitor.GetButtonClass("update-button button-1"),
        $"Update \"{row.Question()}\"", ButtonUpdate_OnClick);

      SecurePage.AddClearBoth(container);
    }

    private static void CreateTextSubTabContent(Control parent, DataRow row, 
      string moreClasses, MonitorInstance monitor)
    {
      // the text answer field
      SecurePage.AddTextAreaInputElement(parent, "TextBox" + row.QuestionKey(),
        monitor.GetDataClass("shadow answer-textbox for-star " + moreClasses, "answer"),
        row.Answer(), "Type or paste your response into this box", true,
        null, false, "Text Response", "answer wide", null, null, null);

      // the remove-line-breaks button
      if (!SecurePage.IsPoliticianUser)
        SecurePage.AddButtonInputElement(parent, null,
          "remove-line-breaks button-2 button-smallest", "Remove Line Breaks", "remove-line-breaks");

      if (!SecurePage.IsPoliticianUser) // add source and date boxes
      {
        var textSourceAndDateDiv = new HtmlDiv().AddTo(parent,
          "text-source-and-date-container source-and-date-container");
        var textSourceDiv = new HtmlDiv().AddTo(textSourceAndDateDiv,
          "text-source-container source-container");
        var sourceRequiredSpan = row.IsTextSourceOptional()
          ? Empty
          : " <span class=\"reqd\">◄</span>";
        SecurePage.AddTextAreaInputElement(textSourceDiv, "Source" + row.QuestionKey(),
          monitor.GetDataClass("shadow source-textbox " + moreClasses, "source"),
          row.Source(), "Copy and paste the url of the web page (with or without http(s)://) where" +
            " you obtained the answer or a description of the source into this box.", false,
          "Copy and paste the url of the web page (with or without http(s)://) where" +
            " you obtained the answer or a description of the source into this box.",
          false, "Text Source" + sourceRequiredSpan, "text-source source", null, null, null);
        var textDateDiv = new HtmlDiv().AddTo(textSourceAndDateDiv, "text-date-container date-container");
        SecurePage.AddTextInputElement(textDateDiv, "Date" + row.QuestionKey(),
          monitor.GetDataClass("shadow-2 date-textbox date-picker " + moreClasses, "date sourcedate"),
          row.DateStamp().DbDateToShortDate(),
          "Enter the date of the answer if today is not an appropriate date. For" +
            " example if you obtained the answer from the candidate's campaign website," +
            " and the election is over, enter some date a couple days prior to the" +
            " election.", false, "Text Date", "date", null, null, null);
        SecurePage.AddButtonInputElement(textDateDiv, null,
          "today-button for-date button-1 button-smallest", "Today", "today for-date");
      }
    }

    private static void CreateYouTubeSubTabContent(Control parent, DataRow row,
      string moreClasses, MonitorInstance monitor)
    {
      var wasUploadedByCandidate = !IsNullOrWhiteSpace(row.YouTubeUrl()) &&
        row.YouTubeSource() == YouTubeVideoInfo.VideoUploadedByCandidateMessage;

      var wasUploadedByVoteUsa = !IsNullOrWhiteSpace(row.YouTubeUrl()) &&
        row.YouTubeSource() != YouTubeVideoInfo.VideoUploadedByCandidateMessage;

      // the Source heading and Uploaded by candidate checkbox (only for master users)
      if (!SecurePage.IsPoliticianUser)
      {
        var heading = new HtmlDiv().AddTo(parent, "category-heading");
        new HtmlSpan { InnerText = "Select Type of Video to be Shown" }.AddTo(heading);

        SecurePage.AddCheckboxInputElement(parent, "YouTubeFromCandidate" + row.QuestionKey(),
          monitor.GetDataClass("youtubefrom-checkbox youtubefromcandidate-checkbox " + moreClasses,
            "youtubefromcandidate"),
          null, wasUploadedByCandidate, "Complete Unedited YouTube.com video - not a YouTube video clip on the Vote USA Channel", "fromcandidate clearfix",
          null, null, null, Empty);

        SecurePage.AddCheckboxInputElement(parent, "YouTubeFromVoteUSA" + row.QuestionKey(),
          monitor.GetDataClass("youtubefrom-checkbox youtubefromvoteusa-checkbox " + moreClasses,
            "youtubefromvoteusa"),
          null, wasUploadedByVoteUsa, "YouTube Video on Vote USA Channel - a video clip created by Vote USA from some other video source and uploaded to the Vote USA Channel", "fromvoteusa clearfix",
          null, null, null, Empty);
      }

      var youTubeSourceFields = new HtmlDiv().AddTo(parent, "youtube-source-fields");

      if (!SecurePage.IsPoliticianUser)
      {
        SecurePage.AddTextInputElement(youTubeSourceFields, "YouTubeSourceUrl" + row.QuestionKey(),
          monitor.GetDataClass("shadow-2 youtubesourceurl-textbox " + moreClasses, "youtubesourceurl"),
          row.YouTubeSourceUrl(), "Enter the url (with or without http(s)://) of the source video.",
          false, "Url of the Other YouTube Source used to create the Vote USA Channel video clip", "youtubeurl wide", null, null, null);
      }

      var youTubeSourceAndDateDiv = new HtmlDiv().AddTo(youTubeSourceFields,
        "youtube-source-and-date-container source-and-date-container");
      if (wasUploadedByCandidate) youTubeSourceFields.AddCssClasses("hidden");
      else youTubeSourceFields.RemoveCssClass("hidden");

      if (!SecurePage.IsPoliticianUser)
      {
        var youTubeSourceDiv = new HtmlDiv().AddTo(youTubeSourceAndDateDiv,
          "youtube-source-container source-container");
        SecurePage.AddTextAreaInputElement(youTubeSourceDiv, "YouTubeSource" + row.QuestionKey(),
          monitor.GetDataClass("shadow youtubesource-textbox " + moreClasses,
            "youtube-source source"),
          row.YouTubeSource(), "Enter a description of the source video into this box.", false,
          "Enter a description of the source video into this box.",
          true, "Description of the Organization or Event of Other YouTube Source", "source", 
          null, null, null, false, "Provide a short and concise description like: First NBC Democratic" +
          " Debate; KVUE Austin TX News; PBS New Hour. Do not include any urls or dates." +
          " These should be only entered in the separate Url and Date textboxes.");
        var youTubeDateDiv = new HtmlDiv().AddTo(youTubeSourceAndDateDiv,
          "youtube-date-container date-container");
        SecurePage.AddTextInputElement(youTubeDateDiv, "YouTubeDate" + row.QuestionKey(),
          monitor.GetDataClass("shadow-2 youtubedate-textbox date-picker " + moreClasses,
            "youtubedate sourcedate"), row.YouTubeDate().DbDateToShortDate(),
          "Enter the date of the video if today is not an appropriate date.", false,
          "Date when Other Source YouTube Content was made", "youtubedate", null, null, null);
        SecurePage.AddButtonInputElement(youTubeDateDiv, null,
          "today-button for-youtubedate button-1 button-smallest", "Today", "today  for-youtubedate");
      }

      SecurePage.AddTextInputElement(parent, "YouTubeUrl" + row.QuestionKey(),
        monitor.GetDataClass("shadow-2 youtubeurl-textbox for-star " + moreClasses, "youtubeurl"),
        row.YouTubeUrl(),
        SecurePage.IsPoliticianUser
         ? "Enter a YouTube url that addresses the question."
         : "Enter the YouTube url of the Vote-USA video.",
        false,
        "Url of YouTube Video to be Shown",
        "youtubeurl wide", null, null, null);

      new HtmlP { InnerText = "When the Update Button is clicked the Description and Running Time of the video on YouTube is automatically extracted and presented in these two textboxes." }.AddTo(parent, "instructions");

      var displayYouTubeDescription = row.YouTubeDescription().SafeString();
      var displayYouTubeRunningTime = row.YouTubeRunningTime().FormatRunningTime();

      if (!IsNullOrWhiteSpace(row.YouTubeAutoDisable()))
      {
        displayYouTubeDescription = ">>> This video has been temporarily disabled on Vote-USA: " +
          row.YouTubeAutoDisable();
        displayYouTubeRunningTime = Empty;
      }

      var youTubeDescriptionAndTimeDiv = new HtmlDiv().AddTo(parent,
        "youtube-description-and-time-container");

      var youTubeDescriptionDiv = new HtmlDiv().AddTo(youTubeDescriptionAndTimeDiv,
        "youtube-description-container");

      SecurePage.AddTextAreaInputElement(youTubeDescriptionDiv, "YouTubeDescription" + row.QuestionKey(),
       monitor.GetDataClass("shadow youtubedescription-textbox " + moreClasses, "youtubedescription"),
       displayYouTubeDescription, null, false, null, false, "Video Description", "youtubedescription wide",
       null, null, null, true);

      var youTubeTimeDiv = new HtmlDiv().AddTo(youTubeDescriptionAndTimeDiv,
        "youtube-time-container");

      SecurePage.AddTextInputElement(youTubeTimeDiv, "YouTubeRunningTime" + row.QuestionKey(),
        monitor.GetDataClass("shadow-2 youtuberunningtime-textbox " + moreClasses, "youtuberunningtime"),
        displayYouTubeRunningTime, null, false, "Running Time", "youtuberunningtime",
        null, null, null, true);
    }

    private static void CreateFacebookVideoSubTabContent(Control parent, DataRow row,
      string moreClasses, MonitorInstance monitor)
    {

      SecurePage.AddTextInputElement(parent, "FacebookVideoUrl" + row.QuestionKey(),
        monitor.GetDataClass("shadow-2 facebookvideourl-textbox for-star " + moreClasses, "facebookvideourl"),
        row.FacebookVideoUrl(),
        "Enter a Facebook Video url that addresses the question.",
        false,
        "Facebook Video Url to be Shown",
        "facebookvideourl wide", null, null, null);

      new HtmlP { InnerText = "When the Update Button is clicked the Description and Running Time of the video on YouTube is automatically extracted and presented in these two textboxes." }.AddTo(parent, "instructions");

      var displayFacebookVideoDescription = row.FacebookVideoDescription().SafeString();
      var displayFacebookVideoRunningTime = row.FacebookVideoRunningTime().FormatRunningTime();

      if (!IsNullOrWhiteSpace(row.FacebookVideoAutoDisable()))
      {
        displayFacebookVideoDescription =
          ">>> This video has been temporarily disabled on Vote-USA: " + 
          row.FacebookVideoAutoDisable();
        displayFacebookVideoRunningTime = Empty;
      }

      var facebookVideoDescriptionAndTimeDiv = new HtmlDiv().AddTo(parent,
        "facebookvideo-description-and-time-container");

      var facebookVideoDescriptionDiv = new HtmlDiv().AddTo(facebookVideoDescriptionAndTimeDiv,
        "facebookvideo-description-container");

      SecurePage.AddTextAreaInputElement(facebookVideoDescriptionDiv, "FacebookVideoDescription" + row.QuestionKey(),
       monitor.GetDataClass("shadow facebookvideodescription-textbox " + moreClasses, "facebookvideodescription"),
       displayFacebookVideoDescription, null, false, null, false, "Video Description", "facebookvideodescription wide",
       null, null, null, true);

      var facebookVideoTimeDiv = new HtmlDiv().AddTo(facebookVideoDescriptionAndTimeDiv,
        "facebookvideo-time-container");

      SecurePage.AddTextInputElement(facebookVideoTimeDiv, "FacebookVideoRunningTime" + row.QuestionKey(),
        monitor.GetDataClass("shadow-2 facebookvideorunningtime-textbox " + moreClasses, "facebookvideorunningtime"),
        displayFacebookVideoRunningTime, null, false, "Running Time", "facebookvideorunningtime",
        null, null, null, true);
    }

    private static SecurePage.UpdateStatus DoAnswerUpdate(IList<IGrouping<string, DataRow>> table, 
      string questionKey, bool reportUnchanged, ref int updateCount)
    {
      var page = VotePage.GetPage<SecurePoliticianPage>();
      var updateStatus = SecurePage.UpdateStatus.Failure; // default
      var description = Empty;

      var feedback =
        page.Master.FindMainContentControl("Feedback" + questionKey) as
          FeedbackContainerControl;
      var updatePanel =
        page.Master.FindMainContentControl("UpdatePanel" + questionKey) as UpdatePanel;
      var textAnswerBox =
        page.Master.FindMainContentControl("TextBox" + questionKey) as TextBox;
      var textSourceBox =
        page.Master.FindMainContentControl("Source" + questionKey) as TextBox;
      var textDateBox = page.Master.FindMainContentControl("Date" + questionKey) as TextBox;
      var youTubeUrlBox =
        page.Master.FindMainContentControl("YouTubeUrl" + questionKey) as TextBox;
      var youTubeSourceBox = page.Master.FindMainContentControl("YouTubeSource" + questionKey) as TextBox;
      var youTubeSourceUrlBox = page.Master.FindMainContentControl("YouTubeSourceUrl" + questionKey) as TextBox;
      var youTubeDateBox = page.Master.FindMainContentControl("YouTubeDate" + questionKey) as TextBox;
      var youTubeDescriptionBox =
        page.Master.FindMainContentControl("YouTubeDescription" + questionKey) as TextBox;
      var youTubeRunningTimeBox =
        page.Master.FindMainContentControl("YouTubeRunningTime" + questionKey) as TextBox;
      var youTubeFromCandidate =
        page.Master.FindMainContentControl("YouTubeFromCandidate" + questionKey) as HtmlInputCheckBox;
      var youTubeFromVoteUsa =
        page.Master.FindMainContentControl("YouTubeFromVoteUSA" + questionKey) as HtmlInputCheckBox;
      var facebookVideoUrlBox =
        page.Master.FindMainContentControl("FacebookVideoUrl" + questionKey) as TextBox;
      var facebookVideoDescriptionBox =
        page.Master.FindMainContentControl("FacebookVideoDescription" + questionKey) as TextBox;
      var facebookVideoRunningTimeBox =
        page.Master.FindMainContentControl("FacebookVideoRunningTime" + questionKey) as TextBox;
      var sequenceHidden = page.Master.FindMainContentControl("Sequence" + questionKey) as HtmlInputHidden;
      var hasValue =
        page.Master.FindMainContentControl("HasValue" + questionKey) as HtmlInputHidden;

      var textIsFromCandidate = SecurePage.IsPoliticianUser;

      var youTubeIsFromCandidate = SecurePage.IsPoliticianUser || youTubeFromCandidate.Checked ||
        youTubeSourceBox.Text.Trim() == YouTubeVideoInfo.VideoUploadedByCandidateMessage;

      try
      {
        textAnswerBox.AddCssClasses("badupdate");

        FeedbackContainerControl.ClearValidationErrors(textAnswerBox, textSourceBox, textDateBox, 
          youTubeUrlBox, youTubeDateBox, facebookVideoUrlBox);

        var sequence = sequenceHidden.Value == "?"
          ? Answers.GetNextSequenceNew(page.PoliticianKey, questionKey)
          : int.Parse(sequenceHidden.Value);

        var newTextAnswer = textAnswerBox.GetValue();
        string newTextSource;
        DateTime newTextDate;
        var newYouTubeSource = Empty;
        var newYouTubeSourceUrl = Empty;
        var newYouTubeDate = VotePage.DefaultDbDate;
        var textDateWasEmpty = true;
        var youTubeDateWasEmpty = true;
        youTubeDescriptionBox.Text = Empty;
        youTubeRunningTimeBox.Text = Empty;
        var newFacebookVideoDate = VotePage.DefaultDbDate;
        if (VotePage.AllowFacebookVideos)
        {
          facebookVideoDescriptionBox.Text = Empty;
          facebookVideoRunningTimeBox.Text = Empty;
        }

        newTextAnswer = feedback.StripHtml(newTextAnswer);
        newTextAnswer = newTextAnswer.StripRedundantSpaces();
        var oldResponses = table.Where(g => g.Key.IsEqIgnoreCase(questionKey))
          .SelectMany(g => g);
        var question = oldResponses.First();
        var oldRow = table.Where(g => g.Key.IsEqIgnoreCase(questionKey))
          .SelectMany(g => g)
          .FirstOrDefault(r => r.SequenceOrNull() == sequence);
        description = '"' + question.Question() + '"';

        var isAnswerChanged = oldRow == null || textAnswerBox.GetValue().Trim() != oldRow.Answer().Trim();

        if (IsNullOrWhiteSpace(newTextAnswer))
        {
          newTextSource = Empty;
          newTextDate = VotePage.DefaultDbDate;
        }
        else if (textIsFromCandidate)
        {
          newTextSource = page.PageCache.Politicians.GetLastName(page.PoliticianKey);
          newTextDate = DateTime.UtcNow.Date;
        }
        else
        {
          newTextSource = textSourceBox.Text;
          newTextSource = feedback.StripHtml(newTextSource);
          newTextSource = newTextSource.StripRedundantSpaces();
          textDateWasEmpty = IsNullOrWhiteSpace(textDateBox.Text);
          newTextDate =
            feedback.ValidateDateOptional(textDateBox, out _, "Text Date",
               isAnswerChanged ? DateTime.UtcNow.Date : VotePage.DefaultDbDate)
              .Date;
        }

        var newYouTubeUrl = youTubeUrlBox.GetValue();
        newYouTubeUrl = feedback.StripHtml(newYouTubeUrl);
        newYouTubeUrl = newYouTubeUrl.StripRedundantSpaces();

        VideoInfo youTubeInfo = null;
        if (!IsNullOrWhiteSpace(newYouTubeUrl))
        {
          var youTubeId = newYouTubeUrl.GetYouTubeVideoId();
          if (youTubeFromCandidate != null && !youTubeFromCandidate.Checked &&
            youTubeFromVoteUsa != null && !youTubeFromVoteUsa.Checked)
            feedback.PostValidationError(new[] { youTubeFromCandidate, youTubeFromVoteUsa }, "Please select a type of video");
          if (IsNullOrWhiteSpace(youTubeId))
            feedback.PostValidationError(youTubeUrlBox, YouTubeVideoInfo.InvalidVideoUrlMessage);
          else 
          {
            youTubeInfo = YouTubeVideoUtility.GetVideoInfo(youTubeId, true, 1);
            if (!youTubeInfo.IsValid)
              feedback.PostValidationError(youTubeUrlBox, YouTubeVideoInfo.VideoIdNotFoundMessage);
            else if (!youTubeInfo.IsPublic)
              feedback.PostValidationError(youTubeUrlBox, YouTubeVideoInfo.VideoNotPublicMessage);
            else
            {
              youTubeDateWasEmpty = IsNullOrWhiteSpace(youTubeDateBox?.Text);
              if (youTubeIsFromCandidate) 
              {
                newYouTubeSource = YouTubeVideoInfo.VideoUploadedByCandidateMessage;
                newYouTubeSourceUrl = Empty;
                newYouTubeDate = youTubeInfo.PublishedAt;
              }
              else
              {
                newYouTubeSource = youTubeSourceBox.Text;
                newYouTubeSource = feedback.StripHtml(newYouTubeSource);
                newYouTubeSource = newYouTubeSource.StripRedundantSpaces();
                newYouTubeSourceUrl = youTubeSourceUrlBox == null
                  ? Empty
                  : youTubeSourceUrlBox.Text;
                newYouTubeSourceUrl = feedback.StripHtml(newYouTubeSourceUrl);
                newYouTubeSourceUrl = Validation.StripWebProtocol(newYouTubeSourceUrl);
                newYouTubeDate = youTubeDateWasEmpty
                  ? youTubeInfo.PublishedAt
                  : feedback.ValidateDate(youTubeDateBox, out _, "YouTube Date",
                  new DateTime(2004, 1, 1), DateTime.UtcNow).Date;
              }
            }
          }
        }

        var newFacebookVideoUrl = (facebookVideoUrlBox?.GetValue()).SafeString();
        newFacebookVideoUrl = feedback.StripHtml(newFacebookVideoUrl);
        newFacebookVideoUrl = newFacebookVideoUrl.StripRedundantSpaces();

        VideoInfo facebookVideoInfo = null;
        if (!IsNullOrWhiteSpace(newFacebookVideoUrl))
        {
          var facebookVideoId = newFacebookVideoUrl.GetFacebookVideoId();
          if (IsNullOrWhiteSpace(facebookVideoId))
            feedback.PostValidationError(facebookVideoUrlBox, FacebookVideoInfo.InvalidVideoUrlMessage);
          else
          {
            facebookVideoInfo = FacebookVideoUtility.GetVideoInfo(facebookVideoId, true, 1);
            if (!facebookVideoInfo.IsValid)
              feedback.PostValidationError(facebookVideoUrlBox, FacebookVideoInfo.VideoIdNotFoundMessage);
            else if (!facebookVideoInfo.IsPublic)
              feedback.PostValidationError(facebookVideoUrlBox, FacebookVideoInfo.VideoNotPublicMessage);
            else
            {
              newFacebookVideoDate = facebookVideoInfo.PublishedAt;
              newFacebookVideoUrl = FacebookVideoUtility.GetUrl(facebookVideoId);
            }
          }
        }

        if (feedback.ValidationErrorCount == 0)
        {
          var oldTextAnswer = Empty;
          var oldTextSource = Empty;
          var oldTextDate = VotePage.DefaultDbDate;
          var oldYouTubeUrl = Empty;
          var oldYouTubeSource = Empty;
          var oldYouTubeSourceUrl = Empty;
          var oldYouTubeDate = VotePage.DefaultDbDate;
          var oldFacebookVideoUrl = Empty;
          var oldFacebookVideoDate = VotePage.DefaultDbDate;

          if (oldRow != null)
          {
            oldTextAnswer = oldRow.Answer().SafeString();
            oldTextSource = oldRow.Source().SafeString();
            oldTextDate = oldRow.DateStamp(VotePage.DefaultDbDate);
            oldYouTubeUrl = oldRow.YouTubeUrl().SafeString();
            oldYouTubeSource = oldRow.YouTubeSource();
            oldYouTubeSourceUrl = oldRow.YouTubeSourceUrl().SafeString();
            oldYouTubeDate = oldRow.YouTubeDate(VotePage.DefaultDbDate)
              .Date;
            oldFacebookVideoUrl = oldRow.FacebookVideoUrl().SafeString();
            oldFacebookVideoDate = oldRow.FacebookVideoDate(VotePage.DefaultDbDate)
              .Date;
          }

          var unchanged = oldTextAnswer == newTextAnswer && 
            oldYouTubeUrl == newYouTubeUrl && 
            oldYouTubeSource == newYouTubeSource &&
            oldYouTubeSourceUrl == newYouTubeSourceUrl &&
            (oldYouTubeDate == newYouTubeDate || youTubeDateWasEmpty) &&
            oldFacebookVideoUrl == newFacebookVideoUrl &&
            oldFacebookVideoDate == newFacebookVideoDate;
          if (unchanged && !textIsFromCandidate)
            unchanged = oldTextSource == newTextSource &&
              (oldTextDate == newTextDate || textDateWasEmpty);

          if (unchanged)
          {
            if (reportUnchanged)
            {
              feedback.AddInfo("Your " + description + " entry was unchanged.");
              updatePanel.Update();
            }
            updateStatus = SecurePage.UpdateStatus.Unchanged;
          }
          else
          {
            if (!textIsFromCandidate && !question.IsTextSourceOptional() && 
              !IsNullOrWhiteSpace(newTextAnswer) &&
              (isAnswerChanged || !IsNullOrWhiteSpace(oldTextSource)))
              feedback.ValidateLength(textSourceBox, "Text Source", 1, 255, out _);
            if (!IsNullOrWhiteSpace(newYouTubeUrl))
            {
              if (!youTubeIsFromCandidate)
              {
                //feedback.ValidateRequired(youTubeSourceBox, "YouTube Source", out _);
                feedback.ValidateLength(youTubeSourceBox, "YouTube Source", 1, 125, out _);
              }
            }
            if (feedback.ValidationErrorCount == 0)
            {
              string youTubeDescription = null;
              var youTubeRunningTime = default(TimeSpan);
              if (youTubeInfo != null)
              {
                youTubeDescription = youTubeInfo.ShortDescription;
                youTubeRunningTime = youTubeInfo.Duration;
                youTubeDescriptionBox.Text = youTubeDescription;
                youTubeRunningTimeBox.Text = youTubeRunningTime.FormatRunningTime();
              }

              youTubeUrlBox.Text = newYouTubeUrl;

              string facebookVideoDescription = null;
              var facebookVideoRunningTime = default(TimeSpan);
              if (facebookVideoInfo != null)
              {
                facebookVideoDescription = facebookVideoInfo.ShortDescription;
                facebookVideoRunningTime = facebookVideoInfo.Duration;
                facebookVideoDescriptionBox.Text = facebookVideoDescription;
                facebookVideoRunningTimeBox.Text = facebookVideoRunningTime.FormatRunningTime();
              }

              if (VotePage.AllowFacebookVideos)
                facebookVideoUrlBox.Text = newFacebookVideoUrl;

              if (youTubeSourceBox != null) youTubeSourceBox.Text = newYouTubeSource;
              if (youTubeSourceUrlBox != null) youTubeSourceUrlBox.Text = newYouTubeSourceUrl;
              if (youTubeDateBox != null) youTubeDateBox.Text = newYouTubeDate.DbDateToShortDate();
              if (youTubeFromCandidate != null) youTubeFromCandidate.Checked = youTubeIsFromCandidate;
              if (youTubeFromVoteUsa != null) youTubeFromVoteUsa.Checked = !youTubeIsFromCandidate;

              hasValue.Value = IsNullOrWhiteSpace(newTextAnswer) &&
                IsNullOrWhiteSpace(newYouTubeUrl) &&
                IsNullOrWhiteSpace(newFacebookVideoUrl)
                  ? Empty
                  : "Y";
              page.LogPoliticianAnswerChange(questionKey, sequence, oldTextAnswer, newTextAnswer,
                newTextSource);
              page.UpdatePoliticianAnswerNew(questionKey, sequence, newTextAnswer,
                newTextSource, newTextDate, newYouTubeUrl, youTubeDescription, youTubeRunningTime,
                newYouTubeSource, newYouTubeSourceUrl, newYouTubeDate, newFacebookVideoUrl,
                facebookVideoDescription, facebookVideoRunningTime, newFacebookVideoDate);
              UpdateQuestion(questionKey, sequence);
              feedback.AddInfo("Your " + description + " entry was updated.");
              updateStatus = SecurePage.UpdateStatus.Success;
              updateCount++;
            }
            updatePanel.Update();
          }
        }

        if (updateStatus != SecurePage.UpdateStatus.Failure)
        {
          if (newTextAnswer != textAnswerBox.Text)
            updatePanel.Update();
          textAnswerBox.SetValue(newTextAnswer);
          if (!textIsFromCandidate)
          {
            if (IsNullOrWhiteSpace(newTextAnswer))
            {
              newTextSource = Empty;
              newTextDate = VotePage.DefaultDbDate;
            }
            var newDateText = newTextDate.DbDateToShortDate();
            Debug.Assert(textSourceBox != null, "sourceBox != null");
            Debug.Assert(textDateBox != null, "dateBox != null");
            if (newTextSource != textSourceBox.Text || newDateText != textDateBox.Text)
              updatePanel.Update();
            textSourceBox.SetValue(newTextSource);
            textDateBox.SetValue(newDateText);
          }
          if (!youTubeIsFromCandidate)
          {
            var newYouTubeDateText = newYouTubeDate.IsDefaultDate()
              ? Empty 
              : newYouTubeDate.DbDateToShortDate();
            if (newYouTubeDateText != youTubeDateBox.Text)
              updatePanel.Update();
            youTubeDateBox.SetValue(newYouTubeDateText);
          }
        }

        textAnswerBox.RemoveCssClass("badupdate");
      }
      catch (Exception ex)
      {
        if (description == Empty) description = "your response";
        feedback.AddError("There was an unexpected error updating " + description);
        feedback.HandleException(ex);
        updatePanel.Update();
      }

      return updateStatus;
    }

    private static void DoAnswerUpdate(string politicianKey, IEnumerable<OneResponse> dataResponses, 
      IList<DataRow> dbResponses, ICollection<UpdateAnswerFeedback> feedback)
    {
      if (dbResponses == null) dbResponses = new List<DataRow>();

      foreach (var dataResponse in dataResponses)
      {
        //var dbResponse = dataResponse.Sequence == "?"
        //  ? null
        //  : dbResponses.First(r => r.Sequence() == int.Parse(dataResponse.Sequence));
        var dbResponse = dbResponses.FirstOrDefault(r => r.Sequence() == dataResponse.Sequence);
        DoResponseUpdate(politicianKey, dataResponse, dbResponse, dbResponses.First(), feedback);
      }
    }

    public class UpdateAnswerFeedback
    {
      // ReSharper disable NotAccessedField.Global
      public int QuestionId;
      public int Sequence;
      public string Type;
      public string Key;
      public string Message;
      // ReSharper restore NotAccessedField.Global
    }

    private static void DoResponseUpdate(string politicianKey, OneResponse data, DataRow oldRow,
      DataRow question, ICollection<UpdateAnswerFeedback> feedback)
    {
      var hasErrors = false;

      void AddToFeedback(int questionId, int sequence, string type, string key, string message)
      {
        feedback.Add(new UpdateAnswerFeedback
        {
          QuestionId = questionId,
          Sequence = sequence,
          Type = type,
          Key = key,
          Message = message
        });
      }

      void AddErrorToFeedback(int questionId, int sequence, string key, string message)
      {
        if (!IsNullOrWhiteSpace(message))
        {
          AddToFeedback(questionId, sequence, "error", key, message);
          hasErrors = true;
        }
      }

      var description = Empty;
      var textIsFromCandidate = SecurePage.IsPoliticianUser;
      var youTubeIsFromCandidate = SecurePage.IsPoliticianUser || data.YouTubeFromCandidate ||
        data.YouTubeSource.Trim() == YouTubeVideoInfo.VideoUploadedByCandidateMessage;

      try
      {
        var newTextAnswer = data.Answer;
        string newTextSource;
        DateTime newTextDate;
        var newYouTubeSource = Empty;
        var newYouTubeSourceUrl = Empty;
        var newYouTubeDate = VotePage.DefaultDbDate;
        var textDateWasEmpty = true;
        var youTubeDateWasEmpty = true;

        newTextAnswer = newTextAnswer.StripHtml().StripRedundantSpaces();
        description = '"' + question.Question() + '"';

        var isAnswerChanged = oldRow == null || newTextAnswer != oldRow.Answer();

        if (IsNullOrWhiteSpace(newTextAnswer))
        {
          newTextSource = Empty;
          newTextDate = VotePage.DefaultDbDate;
        }
        else if (textIsFromCandidate)
        {
          newTextSource = PageCache.GetTemporary().Politicians.GetLastName(politicianKey);
          newTextDate = DateTime.UtcNow.Date;
        }
        else
        {
          newTextSource = data.Source.StripHtml().StripRedundantSpaces();
          textDateWasEmpty = IsNullOrWhiteSpace(data.Date);
          var (date, errorMessage) = Validation.ValidateDateOptional(data.Date, "Text Date",
            isAnswerChanged ? DateTime.UtcNow.Date : VotePage.DefaultDbDate);
          newTextDate = date.Date;
          AddErrorToFeedback(data.QuestionId, data.Sequence, "Date", errorMessage);
        }

        var newYouTubeUrl = data.YouTubeUrl.StripHtml().StripRedundantSpaces();

        VideoInfo youTubeInfo = null;
        if (!IsNullOrWhiteSpace(newYouTubeUrl))
        {
          var youTubeId = newYouTubeUrl.GetYouTubeVideoId();
          if (IsNullOrWhiteSpace(youTubeId))
            AddErrorToFeedback(data.QuestionId, data.Sequence, "YouTubeUrl",
              YouTubeVideoInfo.InvalidVideoUrlMessage);
          else
          {
            youTubeInfo = YouTubeVideoUtility.GetVideoInfo(youTubeId, true, 1);
            if (!youTubeInfo.IsValid)
              AddErrorToFeedback(data.QuestionId, data.Sequence, "YouTubeUrl",
                YouTubeVideoInfo.VideoIdNotFoundMessage);
            else if (!youTubeInfo.IsPublic)
              AddErrorToFeedback(data.QuestionId, data.Sequence, "YouTubeUrl",
                YouTubeVideoInfo.VideoNotPublicMessage);
            else
            {
              youTubeDateWasEmpty = IsNullOrWhiteSpace(data.YouTubeDate);
              if (youTubeIsFromCandidate)
              {
                newYouTubeSource = YouTubeVideoInfo.VideoUploadedByCandidateMessage;
                newYouTubeSourceUrl = Empty;
                newYouTubeDate = youTubeInfo.PublishedAt;
              }
              else
              {
                newYouTubeSource = data.YouTubeSource.StripHtml().StripRedundantSpaces();
                newYouTubeSourceUrl = data.YouTubeSourceUrl.StripHtml();
                newYouTubeSourceUrl = Validation.StripWebProtocol(newYouTubeSourceUrl);
                if (youTubeDateWasEmpty)
                  newYouTubeDate = youTubeInfo.PublishedAt;
                else
                {
                  var (date, errorMessage) = Validation.ValidateDate(data.YouTubeDate,
                    "YouTube Date", new DateTime(2004, 1, 1), DateTime.UtcNow);
                  newYouTubeDate = date.Date;
                  AddErrorToFeedback(data.QuestionId, data.Sequence, "YouTubeDate",
                    errorMessage);
                }
              }
            }
          }
        }

        if (hasErrors) return;

        var oldTextAnswer = Empty;
        var oldTextSource = Empty;
        var oldTextDate = VotePage.DefaultDbDate;
        var oldYouTubeUrl = Empty;
        var oldYouTubeSource = Empty;
        var oldYouTubeSourceUrl = Empty;
        var oldYouTubeDate = VotePage.DefaultDbDate;

        if (oldRow != null)
        {
          oldTextAnswer = oldRow.Answer().SafeString();
          oldTextSource = oldRow.Source().SafeString();
          oldTextDate = oldRow.DateStamp(VotePage.DefaultDbDate);
          oldYouTubeUrl = oldRow.YouTubeUrl().SafeString();
          oldYouTubeSource = oldRow.YouTubeSource();
          oldYouTubeSourceUrl = oldRow.YouTubeSourceUrl().SafeString();
          oldYouTubeDate = oldRow.YouTubeDate(VotePage.DefaultDbDate).Date;
        }

        var unchanged = oldTextAnswer == newTextAnswer && oldYouTubeUrl == newYouTubeUrl &&
          oldYouTubeSource == newYouTubeSource &&
          oldYouTubeSourceUrl == newYouTubeSourceUrl &&
          (oldYouTubeDate == newYouTubeDate || youTubeDateWasEmpty);
        if (unchanged && !textIsFromCandidate)
          unchanged = oldTextSource == newTextSource &&
            (oldTextDate == newTextDate || textDateWasEmpty);

        if (unchanged) return;

        if (!textIsFromCandidate && !question.IsTextSourceOptional() &&
          !IsNullOrWhiteSpace(newTextAnswer) &&
          (isAnswerChanged || !IsNullOrWhiteSpace(oldTextSource)))
        {
          var (_, errorMessage) =
            Validation.ValidateLength(data.Source, "Text Source", 1, 255);
          AddErrorToFeedback(data.QuestionId, data.Sequence, "Source", errorMessage);
        }

        if (!IsNullOrWhiteSpace(newYouTubeUrl))
        {
          if (!youTubeIsFromCandidate)
          {
            var (_, errorMessage) =
              Validation.ValidateRequired(data.YouTubeSource, "YouTube Source");
            AddErrorToFeedback(data.QuestionId, data.Sequence, "YouTube Source",
              errorMessage);
          }
        }

        if (hasErrors) return;

        //    textAnswerBox.SetValue(newTextAnswer);
        //    if (!textIsFromCandidate)
        //    {
        //      if (IsNullOrWhiteSpace(newTextAnswer))
        //      {
        //        newTextSource = Empty;
        //        newTextDate = VotePage.DefaultDbDate;
        //      }
        //      var newDateText = newTextDate.DbDateToShortDate();
        //      textSourceBox.SetValue(newTextSource);
        //      textDateBox.SetValue(newDateText);
        //    }
        //    if (!youTubeIsFromCandidate)
        //    {
        //      var newYouTubeDateText = newYouTubeDate.IsDefaultDate()
        //        ? Empty
        //        : newYouTubeDate.DbDateToShortDate();
        //      youTubeDateBox.SetValue(newYouTubeDateText);
        //    }

        string youTubeDescription = null;
        var youTubeRunningTime = default(TimeSpan);

        if (youTubeInfo != null)
        {
          youTubeDescription = youTubeInfo.ShortDescription;
          youTubeRunningTime = youTubeInfo.Duration;
          AddToFeedback(data.QuestionId, data.Sequence, "value", "YouTubeDescription",
            youTubeDescription);
          AddToFeedback(data.QuestionId, data.Sequence, "value", "YouTubeRunningTime",
            youTubeRunningTime.FormatRunningTime());
        }

        if (data.YouTubeUrl != newYouTubeUrl)
          AddToFeedback(data.QuestionId, data.Sequence, "value", "YouTubeUrl",
            newYouTubeUrl);

        if (data.YouTubeSource != newYouTubeSource)
          AddToFeedback(data.QuestionId, data.Sequence, "value", "YouTubeSource",
            newYouTubeSource);

        if (data.YouTubeSourceUrl != newYouTubeSourceUrl)
          AddToFeedback(data.QuestionId, data.Sequence, "value", "YouTubeSourceUrl",
            newYouTubeSourceUrl);

        var newYouTubeDateText = newYouTubeDate.DbDateToShortDate();
        if (data.YouTubeDate != newYouTubeDateText)
          AddToFeedback(data.QuestionId, data.Sequence, "value", "YouTubeDate",
            newYouTubeDateText);

        if (data.YouTubeFromCandidate != youTubeIsFromCandidate)
          AddToFeedback(data.QuestionId, data.Sequence, "value", "YouTubeFromCandidate",
            youTubeIsFromCandidate.ToString());

        if (data.YouTubeUrl != newYouTubeUrl)
          AddToFeedback(data.QuestionId, data.Sequence, "value", "YouTubeUrl",
            newYouTubeUrl);

        LogDataChange.LogUpdate(Answers2.Column.Answer, oldTextAnswer, newTextAnswer, newTextSource,
          VotePage.UserName, SecurePage.UserSecurityClass, DateTime.UtcNow, politicianKey, 
          data.QuestionId, data.Sequence);
        SecurePoliticianPage.UpdatePoliticianAnswerNew(politicianKey, data.QuestionId.ToString(), 
          data.Sequence, newTextAnswer, newTextSource, newTextDate, newYouTubeUrl, 
          youTubeDescription, youTubeRunningTime,
          newYouTubeSource, newYouTubeSourceUrl, newYouTubeDate, null,
          null, default, VotePage.DefaultDbDate);
      }
      catch (Exception ex)
      {
        if (description == Empty) description = "your response";
        AddErrorToFeedback(data.QuestionId, data.Sequence, "*",
          $"There was an unexpected error updating {description}");
        VotePage.LogException("UpdateAnswer", ex);
      }
    }

    private void DoAnswersUpdate(string questionKey, bool reportUnchanged)
    {
      var updateCount = 0;
      DoAnswerUpdate(GetDataTable(), questionKey, reportUnchanged, ref updateCount);
    }

    public IList<IGrouping<string, DataRow>> GetDataTable()
    {
      if (_DataTable == null)
      {
        IEnumerable<DataRow> table = null;
        if (_Usage == Usage.ForIssues)
        {
          var page = VotePage.GetPage<SecurePoliticianPage>();
          var officeKey = page.PageCache.Politicians.GetOfficeKey(page.PoliticianKey);
          //table = AnswersView.GetAllDataByPoliticianKeyNew(page.PoliticianKey, officeKey).Rows.OfType<DataRow>().Where(
          //  r => r.IssueLevel() != Issues.IssueLevelAll).ToList();
          table = AnswersView.GetAllDataByPoliticianKeyNew(page.PoliticianKey, officeKey).Rows.OfType<DataRow>().Where(
            r => r.IssueId() > 1000).ToList(); // exclude bio & reasons
        }
        else if (_Usage == Usage.ForIntroBio)
        {
          table = AnswersView.GetAllDataByIssueKeyPoliticianKeyNew(
            Issues.IssueId.Biographical.ToInt(),
            VotePage.GetPage<SecurePoliticianPage>().PoliticianKey).Rows.OfType<DataRow>();
        }
        else if (_Usage == Usage.ForIntroReasons)
        {
          table = AnswersView.GetAllDataByIssueKeyPoliticianKeyNew(
            Issues.IssueId.Reasons.ToInt(),
            VotePage.GetPage<SecurePoliticianPage>().PoliticianKey).Rows.OfType<DataRow>();
        }

        Debug.Assert(table != null, "table != null");
        _DataTable = table.GroupBy(r => r.QuestionKey())
          .ToList();
      }
      return _DataTable;
    }

    public IList<IGrouping<int, DataRow>> GetDataTable(string politicianKey)
    {
      IEnumerable<DataRow> table = null;
      if (_Usage == Usage.ForAll)
        table = AnswersView.GetAllDataByPoliticianKeyNew(politicianKey, null).Rows
          .OfType<DataRow>();

      Debug.Assert(table != null, "table != null");
      return table.GroupBy(r => r.QuestionId()).ToList();
    }

    private static void SetResponseData(ICollection<DataRow> responses, DataRow row,
      HtmlControl sequenceItem)
    {
      // if there is more than one response, attach json for additional responses to sequence
      // skip the one we are reporting
      if (responses.Count > 1)
      {
        var responseArray = responses.Where(r => r != row)
          .Select(r => new OneResponse
          {
            // ReSharper disable PossibleInvalidOperationException
            Answer = r.Answer(),
            Source = r.Source(),
            Date = r.DateStamp().IsDefaultDate() ? Empty : r.DateStamp().ToString("d"),
            YouTubeUrl = r.YouTubeUrl(),
            YouTubeDescription = r.YouTubeDescription(),
            YouTubeRunningTime = r.YouTubeRunningTime().FormatRunningTime(),
            YouTubeSource = r.YouTubeSource(),
            YouTubeSourceUrl = r.YouTubeSourceUrl(),
            YouTubeDate = r.YouTubeDate().IsDefaultDate() ? Empty : r.YouTubeDate().ToString("d"),
            YouTubeAutoDisable = r.YouTubeAutoDisable(),
            YouTubeFromCandidate = r.YouTubeAutoDisable() == YouTubeVideoInfo.VideoUploadedByCandidateMessage,
            //FacebookVideoUrl = r.FacebookVideoUrl(),
            //FacebookVideoDescription = r.FacebookVideoDescription(),
            //FacebookVideoRunningTime = r.FacebookVideoRunningTime().FormatRunningTime(),
            //FacebookVideoDate = r.FacebookVideoDate().IsDefaultDate() ? Empty : r.FacebookVideoDate().ToString("d"),
            //FacebookVideoAutoDisable = r.FacebookVideoAutoDisable(),
            Sequence = r.Sequence()/*.ToString(CultureInfo.InvariantCulture)*/
            // ReSharper restore PossibleInvalidOperationException
          })
          .ToArray();
        sequenceItem.Attributes.Add("data-responses", JsonConvert.ExportToString(responseArray));
      }
    }

    public int UpdateAllAnswers(bool showSummary, ref int updateCount)
    {
      var errorCount = 0;
      var table = GetDataTable();
      foreach (var group in table)
        if (DoAnswerUpdate(table, group.First().QuestionKey(), showSummary, ref updateCount) ==
          SecurePage.UpdateStatus.Failure) errorCount++;
      return errorCount;
    }

    public void UpdateAllAnswersNew(string politicianKey, IEnumerable<OneResponse[]> data,
      List<UpdateAnswerFeedback> feedback)
    {
      var table = GetDataTable(politicianKey);

      foreach (var responses in data)
        DoAnswerUpdate(politicianKey, responses,
          table.FirstOrDefault(g => g.Key == responses.First().QuestionId)?.ToList(), feedback);
    }

    private static void UpdateQuestion(string questionKey, int sequence)
    {
      var page = VotePage.GetPage<SecurePoliticianPage>();
      var responses = Answers2.GetDataByPoliticianKeyQuestionId(page.PoliticianKey, int.Parse(questionKey)).Rows.OfType<DataRow>()
        .ToList();
      var row = responses.FirstOrDefault(r => r.Sequence() == sequence);

      // refresh the sequence
      // if the sequence no longer exists, make it "?"
      var sequenceHidden = 
        page.Master.FindMainContentControl("Sequence" + questionKey) as HtmlInputHidden;
      Debug.Assert(sequenceHidden != null, "sequenceHidden != null");
      sequenceHidden.Value = row == null
        ? "?"
        : sequence.ToString(CultureInfo.InvariantCulture);

      SetResponseData(responses, row, sequenceHidden);

      // recreate the action menu (it might need changing)
      var actionContainer =
        page.Master.FindMainContentControl("Action" + questionKey) as HtmlContainerControl;
      CreateActionMenu(actionContainer, responses, row == null);
    }

    #region Event handlers and overrides

    private void ButtonUpdate_OnClick(object sender, EventArgs e)
    {
      var page = VotePage.GetPage<SecurePage>();
      if (!(sender is Button button) ||
        !button.ID.StartsWith("Button", StringComparison.Ordinal)) return;
      var questionKey = button.ID.Substring(6);
      var feedback =
        page.Master.FindMainContentControl("Feedback" + questionKey) as
          FeedbackContainerControl;

      try
      {
        DoAnswersUpdate(questionKey, true);
        page.InvalidatePageCache();
      }
      catch (Exception ex)
      {
        feedback?.HandleException(ex);
      }
    }

    #endregion Event handlers and overrides
  }
}