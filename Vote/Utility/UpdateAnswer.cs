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
using Jayrock.Json.Conversion;
using Vote.Controls;

namespace Vote
{
  public class UpdateAnswer
  {
    private IList<IGrouping<string, AnswersViewRow>> _DataTable;
    private readonly Usage _Usage;

    private const string MonitorPrefix = "mc";
    private const string MonitorTabPrefix = "mt";
    private const string MonitorSuperTabPrefix = "ms";

    public enum Usage
    {
      ForIssues,
      ForIntroReasons,
      ForIntroBio
    }

    // must be public for JSON conversion
    // ReSharper disable once MemberCanBePrivate.Global
    public class Responses
    {
      // ReSharper disable NotAccessedField.Global
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
      public string Sequence;
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

    private static HtmlContainerControl AddFeedbackWithIe7Floater(
      Control parent, string id, string className)
    {
      var feedback = CreateFeedbackWithIe7Floater(id, className);
      parent.Controls.Add(feedback);
      return feedback;
    }

    private void CreateActionMenu(HtmlContainerControl menuContainer, IList<DataRow> responses,
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

    public void CreateControls(HtmlContainerControl parent, IList<AnswersViewRow> responses,
      MonitorInstance monitor, Dictionary<string, string> alternateHeadings = null)
    {
      var row = responses.First();
      var responsesAsDataRow = responses.Where(r => r.Sequence != null).Cast<DataRow>().ToList();
      parent.AddCssClasses("answer-panel " +
        (_Usage == Usage.ForIssues ? "issues-answer-panel " : "intro-answer-panel ") +
        (SecurePage.IsPoliticianUser ? "politician-answer-panel" : "master-answer-panel"));

      // The panel title
      var heading = SecurePage.AddContainer(parent, null, null);
      var headingText = alternateHeadings != null && alternateHeadings.ContainsKey(row.QuestionKey)
        ? alternateHeadings[row.QuestionKey]
        : row.Question;
      SecurePage.AddHeading(heading, 4, null, null, headingText);
      SecurePage.Center(heading, true, true, "heading");
      if (_Usage == Usage.ForIssues) heading.Visible = false;

      // Add undo button as next sibling to heading
      var undo = SecurePage.CreateUndoButton("Undo" + row.QuestionKey,
        monitor.GetUndoClass(null),
        $"Revert \"{row.Question}\" to the latest saved version");
      heading.AddAfter(undo);

      // Same for clear button
      var clear = SecurePage.CreateClearButton("Clear" + row.QuestionKey,
        monitor.GetClearClass(null),
        $"Clear \"{row.Question}\"");
      heading.AddAfter(clear);

      var updatePanel = SecurePage.AddAjaxUpdatePanel(parent, "UpdatePanel" + row.QuestionKey);

      var container = SecurePage.AddContainer(updatePanel.ContentTemplateContainer,
        "Container" + row.QuestionKey, monitor.GetContainerClass("update-all updated answer-container"));

      SecurePage.AddHiddenField(container, "Description" + row.QuestionKey,
        monitor.GetDescriptionClass(null, "answer"), row.Question);

      SecurePage.AddHiddenField(container, "SubTab" + row.QuestionKey,
        "subtab subtab-" + row.QuestionKey);

      var hasNoResponses = row.Sequence == null;

      var sequenceItem = SecurePage.AddHiddenField(container, "Sequence" + row.QuestionKey, "answer-sequence",
        hasNoResponses ? "?" : row.Sequence.Value.ToString(CultureInfo.InvariantCulture));

      SetResponseData(responsesAsDataRow, row, sequenceItem);

      // the action menu
      var menuContainer = SecurePage.AddContainer(container, "Action" + row.QuestionKey,
        "action-menu");
      CreateActionMenu(menuContainer, responsesAsDataRow, row.Sequence == null);

      new HtmlP
      {
        InnerHtml = "To completely delete a response (Text and YouTube), use the Clear button (red <span>X</span> upper right) then Update."
      }.AddTo(container, "delete-message");

      var subTabs = new HtmlDiv
      {
        ID = "answer-subtabs-" + row.QuestionKey,
        ClientIDMode = ClientIDMode.Static
      }.AddTo(container, "answer-sub-tabs shadow");
      var subTabsTabs = new HtmlUl().AddTo(subTabs, "htabs unselectable");

      var textSubTab = new HtmlLi { EnableViewState = false }.AddTo(subTabsTabs, "tab htab");
      new HtmlAnchor
      {
        HRef = "#tab-textanswer-" + row.QuestionKey,
        InnerHtml = "Text<br />Response",
        EnableViewState = false
      }.AddTo(textSubTab);

      var youTubeSubTab = new HtmlLi { EnableViewState = false }.AddTo(subTabsTabs, "tab htab");
      new HtmlAnchor
      {
        HRef = "#tab-youtubeanswer-" + row.QuestionKey,
        InnerHtml = "YouTube<br />Response",
        EnableViewState = false
      }.AddTo(youTubeSubTab);

      var textSubTabContent = new HtmlDiv
      {
        ID = "tab-textanswer-" + row.QuestionKey,
        ClientIDMode = ClientIDMode.Static
      }.AddTo(subTabs);
      var youTubeSubTabContent = new HtmlDiv
      {
        ID = "tab-youtubeanswer-" + row.QuestionKey,
        ClientIDMode = ClientIDMode.Static
      }.AddTo(subTabs);

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
          var tabPrefix = MonitorPrefix + "-" + MonitorTabPrefix + "-issue" +
            row.IssueKey;
          var superTabPrefix = MonitorPrefix + "-" + MonitorSuperTabPrefix + "-igroup" +
            row.IssueGroupKey;
          moreClasses = tabPrefix + " " + superTabPrefix;
          break;
      }

      SecurePage.AddHiddenField(container, "HasValue" + row.QuestionKey,
        monitor.GetDataClass(moreClasses, "hasvalue"),
        string.IsNullOrWhiteSpace(row.Answer) && string.IsNullOrWhiteSpace(row.YouTubeUrl)
          ? string.Empty
          : "Y");

      CreateTextSubTabContent(textSubTabContent, row, moreClasses, monitor);
      CreateYouTubeSubTabContent(youTubeSubTabContent, row, moreClasses, monitor);

      new HtmlHr().AddTo(container);

      // feedback
      AddFeedbackWithIe7Floater(container, "Feedback" + row.QuestionKey,
        monitor.GetFeedbackClass(null));

      // update button
      SecurePage.AddUpdateButtonInContainer(container, "Button" + row.QuestionKey,
        "update-button", monitor.GetButtonClass("update-button button-1"),
        $"Update \"{row.Question}\"", ButtonUpdate_OnClick);

      SecurePage.AddClearBoth(container);
    }

    private static void CreateTextSubTabContent(HtmlControl parent, AnswersViewRow row, 
      string moreClasses, MonitorInstance monitor)
    {
      // the text answer field
      SecurePage.AddTextAreaInputElement(parent, "TextBox" + row.QuestionKey,
        monitor.GetDataClass("shadow answer-textbox for-star " + moreClasses, "answer"),
        row.Answer, "Type or paste your response into this box", true,
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
        var sourceRequiredSpan = row.IsTextSourceOptional.GetValueOrDefault()
          ? string.Empty
          : " <span class=\"reqd\">◄</span>";
        SecurePage.AddTextAreaInputElement(textSourceDiv, "Source" + row.QuestionKey,
          monitor.GetDataClass("shadow source-textbox " + moreClasses, "source"),
          row.Source, "Copy and paste the url of the web page (with or without http://) where" +
            " you obtained the answer or a description of the source into this box.", false,
          "Copy and paste the url of the web page (with or without http://) where" +
            " you obtained the answer or a description of the source into this box.",
          false, "Text Source" + sourceRequiredSpan, "text-source source", null, null, null);
        var textDateDiv = new HtmlDiv().AddTo(textSourceAndDateDiv, "text-date-container date-container");
        SecurePage.AddTextInputElement(textDateDiv, "Date" + row.QuestionKey,
          monitor.GetDataClass("shadow-2 date-textbox date-picker " + moreClasses, "date sourcedate"),
          row.DateStamp.DbDateToShortDate(),
          "Enter the date of the answer if today is not an appropriate date. For" +
            " example if you obtained the answer from the candidate's campaign website," +
            " and the election is over, enter some date a couple days prior to the" +
            " election.", false, "Text Date", "date", null, null, null);
        SecurePage.AddButtonInputElement(textDateDiv, null,
          "today-button for-date button-1 button-smallest", "Today", "today for-date");
      }
    }

    private static void CreateYouTubeSubTabContent(HtmlControl parent, AnswersViewRow row, 
      string moreClasses, MonitorInstance monitor)
    {
      var wasUploadedByCandidate = !string.IsNullOrWhiteSpace(row.YouTubeUrl) && 
        row.YouTubeSource == YouTubeInfo.VideoUploadedByCandidateMessage;

      var wasUploadedByVoteUsa = !string.IsNullOrWhiteSpace(row.YouTubeUrl) && 
        row.YouTubeSource != YouTubeInfo.VideoUploadedByCandidateMessage;

      // the Source heading and Uploaded by candidate checkbox (only for master users)
      if (!SecurePage.IsPoliticianUser)
      {
        var heading = new HtmlDiv().AddTo(parent, "category-heading");
        new HtmlSpan { InnerText = "Select Type of Video to be Shown" }.AddTo(heading);

        SecurePage.AddCheckboxInputElement(parent, "YouTubeFromCandidate" + row.QuestionKey,
          monitor.GetDataClass("youtubefrom-checkbox youtubefromcandidate-checkbox " + moreClasses,
            "youtubefromcandidate"),
          null, wasUploadedByCandidate, "Complete Video on YouTube (not created by Vote USA)", "fromcandidate clearfix",
          null, null, null, string.Empty);

        SecurePage.AddCheckboxInputElement(parent, "YouTubeFromVoteUSA" + row.QuestionKey,
          monitor.GetDataClass("youtubefrom-checkbox youtubefromvoteusa-checkbox " + moreClasses,
            "youtubefromvoteusa"),
          null, wasUploadedByVoteUsa, "Shortened Video Extracted from an Existing Video (on Vote USA or other Private Channel)", "fromvoteusa clearfix",
          null, null, null, string.Empty);
      }

      // the Vote-USA heading (only for master users)
      //if (!SecurePage.IsPoliticianUser)
      //{
      //  var heading = new HtmlDiv().AddTo(parent, "category-heading");
      //  new HtmlSpan {InnerText = "Uploaded Vote USA YouTube Video"}.AddTo(heading);
      //}

      var youTubeSourceFields = new HtmlDiv().AddTo(parent, "youtube-source-fields");

      if (!SecurePage.IsPoliticianUser)
      {
        SecurePage.AddTextInputElement(youTubeSourceFields, "YouTubeSourceUrl" + row.QuestionKey,
          monitor.GetDataClass("shadow-2 youtubesourceurl-textbox " + moreClasses, "youtubesourceurl"),
          row.YouTubeSourceUrl, "Enter the url (with or without http://) of the source video.",
          false, "Url of Longer Video Used to Extract Shortened Video (on Vote USA or Private Channel)", "youtubeurl wide", null, null, null);
      }

      var youTubeSourceAndDateDiv = new HtmlDiv().AddTo(youTubeSourceFields,
        "youtube-source-and-date-container source-and-date-container");
      if (wasUploadedByCandidate) youTubeSourceFields.AddCssClasses("hidden");
      else youTubeSourceFields.RemoveCssClass("hidden");

      if (!SecurePage.IsPoliticianUser)
      {
        var youTubeSourceDiv = new HtmlDiv().AddTo(youTubeSourceAndDateDiv,
          "youtube-source-container source-container");
        SecurePage.AddTextAreaInputElement(youTubeSourceDiv, "YouTubeSource" + row.QuestionKey,
          monitor.GetDataClass("shadow youtubesource-textbox " + moreClasses,
            "youtube-source source"),
          row.YouTubeSource, "Enter a description of the source video into this box.", false,
          "Enter a description of the source video into this box.",
          true, "Source Description of Longer Video", "source", null, null, null);
        var youTubeDateDiv = new HtmlDiv().AddTo(youTubeSourceAndDateDiv,
          "youtube-date-container date-container");
        SecurePage.AddTextInputElement(youTubeDateDiv, "YouTubeDate" + row.QuestionKey,
          monitor.GetDataClass("shadow-2 youtubedate-textbox date-picker " + moreClasses,
            "youtubedate sourcedate"), row.YouTubeDate.DbDateToShortDate(),
          "Enter the date of the video if today is not an appropriate date.", false,
          "Source Date", "youtubedate", null, null, null);
        SecurePage.AddButtonInputElement(youTubeDateDiv, null,
          "today-button for-youtubedate button-1 button-smallest", "Today", "today  for-youtubedate");
      }

      SecurePage.AddTextInputElement(parent, "YouTubeUrl" + row.QuestionKey,
        monitor.GetDataClass("shadow-2 youtubeurl-textbox for-star " + moreClasses, "youtubeurl"),
        row.YouTubeUrl,
        SecurePage.IsPoliticianUser
         ? "Enter a YouTube url that addresses the question."
         : "Enter the YouTube url of the Vote-USA video.", 
        false,
        //SecurePage.IsPoliticianUser
        // ? "YouTube Url"
        // : "Url Provided by YouTube", 
        "YouTube Url to be Shown",
        "youtubeurl wide", null, null, null);

      new HtmlP { InnerText = "When you Update, the Video Description and Running Time will be automatically retrieved from YouTube." }.AddTo(parent, "url-message");

      var displayYouTubeDescription = row.YouTubeDescription.SafeString();
      var displayYouTubeRunningTime = row.YouTubeRunningTime.FormatRunningTime();

      if (!string.IsNullOrWhiteSpace(row.YouTubeAutoDisable))
      {
        displayYouTubeDescription = ">>> This video has been temporarily disabled on Vote-USA: " +
          row.YouTubeAutoDisable;
        displayYouTubeRunningTime = string.Empty;
      }

      var youTubeDescriptionAndTimeDiv = new HtmlDiv().AddTo(parent,
        "youtube-description-and-time-container");

      var youTubeDescriptionDiv = new HtmlDiv().AddTo(youTubeDescriptionAndTimeDiv,
        "youtube-description-container");

      SecurePage.AddTextAreaInputElement(youTubeDescriptionDiv, "YouTubeDescription" + row.QuestionKey,
       monitor.GetDataClass("shadow youtubedescription-textbox " + moreClasses, "youtubedescription"),
       displayYouTubeDescription, null, false, null, false, "Video Description", "youtubedescription wide", 
       null, null, null, true);

      var youTubeTimeDiv = new HtmlDiv().AddTo(youTubeDescriptionAndTimeDiv,
        "youtube-time-container");

      SecurePage.AddTextInputElement(youTubeTimeDiv, "YouTubeRunningTime" + row.QuestionKey,
        monitor.GetDataClass("shadow-2 youtuberunningtime-textbox " + moreClasses, "youtuberunningtime"),
        displayYouTubeRunningTime, null, false, "Running Time", "youtuberunningtime", 
        null, null, null, true);
    }

    private SecurePage.UpdateStatus DoAnswerUpdate(IList<IGrouping<string, AnswersViewRow>> table, 
      string questionKey, bool reportUnchanged, ref int updateCount)
    {
      var page = VotePage.GetPage<SecurePoliticianPage>();
      var updateStatus = SecurePage.UpdateStatus.Failure; // default
      var description = string.Empty;

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
      var sequenceHidden = page.Master.FindMainContentControl("Sequence" + questionKey) as HtmlInputHidden;
      var hasValue =
        page.Master.FindMainContentControl("HasValue" + questionKey) as HtmlInputHidden;

      var textIsFromCandidate = SecurePage.IsPoliticianUser;

      var youTubeIsFromCandidate = SecurePage.IsPoliticianUser || youTubeFromCandidate.Checked ||
        youTubeSourceBox.Text.Trim() == YouTubeInfo.VideoUploadedByCandidateMessage;

      try
      {
        textAnswerBox.AddCssClasses("badupdate");

        FeedbackContainerControl.ClearValidationErrors(textAnswerBox, textSourceBox, textDateBox, 
          youTubeUrlBox, youTubeDateBox);

        var sequence = sequenceHidden.Value == "?"
          ? Answers.GetNextSequence(page.PoliticianKey, questionKey)
          : int.Parse(sequenceHidden.Value);

        var newTextAnswer = textAnswerBox.GetValue();
        string newTextSource;
        DateTime newTextDate;
        var newYouTubeSource = string.Empty;
        var newYouTubeSourceUrl = string.Empty;
        var newYouTubeDate = VotePage.DefaultDbDate;
        bool success;
        var textDateWasEmpty = true;
        var youTubeDateWasEmpty = true;
        youTubeDescriptionBox.Text = string.Empty;
        youTubeRunningTimeBox.Text = string.Empty;

        newTextAnswer = feedback.StripHtml(newTextAnswer);
        newTextAnswer = newTextAnswer.StripRedundantSpaces();
        var oldResponses = table.Where(g => g.Key.IsEqIgnoreCase(questionKey))
          .SelectMany(g => g);
        var question = oldResponses.First();
        var oldRow = table.Where(g => g.Key.IsEqIgnoreCase(questionKey))
          .SelectMany(g => g)
          .FirstOrDefault(r => r.Sequence == sequence);
        description = '"' + question.Question + '"';

        var isAnswerChanged = oldRow == null || /*newTextAnswer*/textAnswerBox.GetValue().Trim() != oldRow.Answer.Trim();

        if (string.IsNullOrWhiteSpace(newTextAnswer))
        {
          newTextSource = string.Empty;
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
          textDateWasEmpty = string.IsNullOrWhiteSpace(textDateBox.Text);
          newTextDate =
            feedback.ValidateDateOptional(textDateBox, out success, "Text Date",
               isAnswerChanged ? DateTime.UtcNow.Date : VotePage.DefaultDbDate)
              .Date;
        }

        var newYouTubeUrl = youTubeUrlBox.GetValue();
        newYouTubeUrl = feedback.StripHtml(newYouTubeUrl);
        newYouTubeUrl = newYouTubeUrl.StripRedundantSpaces();

        YouTubeInfo youTubeInfo = null;
        if (!string.IsNullOrWhiteSpace(newYouTubeUrl))
        {
          var youTubeId = newYouTubeUrl.GetYouTubeVideoId();
          if (youTubeFromCandidate != null && !youTubeFromCandidate.Checked &&
            youTubeFromVoteUsa != null && !youTubeFromVoteUsa.Checked)
            feedback.PostValidationError(new[] { youTubeFromCandidate, youTubeFromVoteUsa }, "Please select a type of video");
          if (string.IsNullOrWhiteSpace(youTubeId))
            feedback.PostValidationError(youTubeUrlBox, YouTubeInfo.InvalidVideoUrlMessage);
          else 
          {
            youTubeInfo = YouTubeUtility.GetVideoInfo(youTubeId, true, 1);
            if (!youTubeInfo.IsValid)
              feedback.PostValidationError(youTubeUrlBox, YouTubeInfo.VideoIdNotFoundMessage);
            else if (!youTubeInfo.IsPublic)
              feedback.PostValidationError(youTubeUrlBox, YouTubeInfo.VideoNotPublicMessage);
            else
            {
              youTubeDateWasEmpty = string.IsNullOrWhiteSpace(youTubeDateBox?.Text);
              if (youTubeIsFromCandidate) 
              {
                newYouTubeSource = YouTubeInfo.VideoUploadedByCandidateMessage;
                newYouTubeSourceUrl = string.Empty;
                newYouTubeDate = youTubeInfo.PublishedAt;
              }
              else
              {
                newYouTubeSource = youTubeSourceBox.Text;
                newYouTubeSource = feedback.StripHtml(newYouTubeSource);
                newYouTubeSource = newYouTubeSource.StripRedundantSpaces();
                newYouTubeSourceUrl = youTubeSourceUrlBox == null
                  ? string.Empty
                  : youTubeSourceUrlBox.Text;
                newYouTubeSourceUrl = feedback.StripHtml(newYouTubeSourceUrl);
                newYouTubeSourceUrl = Validation.StripWebProtocol(newYouTubeSourceUrl);
                newYouTubeDate = youTubeDateWasEmpty
                  ? youTubeInfo.PublishedAt
                  : feedback.ValidateDate(youTubeDateBox, out success, "YouTube Date",
                  new DateTime(2004, 1, 1), DateTime.UtcNow).Date;
              }
            }
          }
      }

        if (feedback.ValidationErrorCount == 0)
        {
          var oldTextAnswer = string.Empty;
          var oldTextSource = string.Empty;
          var oldTextDate = VotePage.DefaultDbDate;
          var oldYouTubeUrl = string.Empty;
          var oldYouTubeSource = string.Empty;
          var oldYouTubeSourceUrl = string.Empty;
          var oldYouTubeDate = VotePage.DefaultDbDate;

          if (oldRow != null)
          {
            oldTextAnswer = oldRow.Answer.SafeString();
            oldTextSource = oldRow.Source.SafeString();
            oldTextDate = oldRow.DateStamp.SafeDbDate();
            oldYouTubeUrl = oldRow.YouTubeUrl.SafeString();
            oldYouTubeSource = oldRow.YouTubeSource();
            oldYouTubeSourceUrl = oldRow.YouTubeSourceUrl().SafeString();
            oldYouTubeDate = oldRow.YouTubeDate.SafeDbDate()
            .Date;
          }

          var unchanged = oldTextAnswer == newTextAnswer && 
            oldYouTubeUrl == newYouTubeUrl && 
            oldYouTubeSource == newYouTubeSource &&
            oldYouTubeSourceUrl == newYouTubeSourceUrl &&
            (oldYouTubeDate == newYouTubeDate || youTubeDateWasEmpty);
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
            if (!textIsFromCandidate && !question.IsTextSourceOptional.GetValueOrDefault() && 
              !string.IsNullOrWhiteSpace(newTextAnswer) &&
              (isAnswerChanged || !string.IsNullOrWhiteSpace(oldTextSource)))
              feedback.ValidateLength(textSourceBox, "Text Source", 1, 255, out success);
            if (!string.IsNullOrWhiteSpace(newYouTubeUrl))
            {
              if (!youTubeIsFromCandidate)
                feedback.ValidateRequired(youTubeSourceBox, "YouTube Source", out success);
            }
            if (feedback.ValidationErrorCount == 0)
            {
              string videoDescription = null;
              var videoRunningTime = default(TimeSpan);
              if (youTubeInfo != null)
              {
                videoDescription = youTubeInfo.ShortDescription;
                videoRunningTime = youTubeInfo.Duration;
                youTubeDescriptionBox.Text = videoDescription;
                youTubeRunningTimeBox.Text = videoRunningTime.FormatRunningTime();
              }

              if (youTubeSourceBox != null) youTubeSourceBox.Text = newYouTubeSource;
              if (youTubeSourceUrlBox != null) youTubeSourceUrlBox.Text = newYouTubeSourceUrl;
              if (youTubeDateBox != null) youTubeDateBox.Text = newYouTubeDate.DbDateToShortDate();
              if (youTubeFromCandidate != null) youTubeFromCandidate.Checked = youTubeIsFromCandidate;
              if (youTubeFromVoteUsa != null) youTubeFromVoteUsa.Checked = !youTubeIsFromCandidate;

              hasValue.Value = string.IsNullOrWhiteSpace(newTextAnswer) &&
                string.IsNullOrWhiteSpace(newYouTubeUrl)
                ? string.Empty
                : "Y";
              page.LogPoliticianAnswerChange(questionKey, sequence, oldTextAnswer, newTextAnswer,
                newTextSource);
              page.UpdatePoliticianAnswer(questionKey, sequence, question.IssueKey, newTextAnswer,
                newTextSource, newTextDate, newYouTubeUrl, videoDescription, videoRunningTime,
                newYouTubeSource, newYouTubeSourceUrl, newYouTubeDate);
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
            if (string.IsNullOrWhiteSpace(newTextAnswer))
            {
              newTextSource = string.Empty;
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
            var newYouTubeDateText = newYouTubeDate == VotePage.DefaultDbDate 
              ? string.Empty 
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
        if (description == string.Empty) description = "your response";
        feedback.AddError("There was an unexpected error updating " + description);
        feedback.HandleException(ex);
        updatePanel.Update();
      }

      return updateStatus;
    }

    private void DoAnswersUpdate(string questionKey, bool reportUnchanged)
    {
      var updateCount = 0;
      DoAnswerUpdate(GetDataTable(), questionKey, reportUnchanged, ref updateCount);
    }

    public IList<IGrouping<string, AnswersViewRow>> GetDataTable()
    {
      if (_DataTable == null)
      {
        AnswersViewTable table = null;
        if (_Usage == Usage.ForIssues)
        {
          var page = VotePage.GetPage<SecurePoliticianPage>();
          var officeKey = page.PageCache.Politicians.GetOfficeKey(page.PoliticianKey);
          var officeClass = page.PageCache.Offices.GetOfficeClass(officeKey);
          if (officeClass.IsFederal())
            table =
              AnswersView.GetAllDataByIssueLevelStateCodePoliticianKey("B", "US",
                page.PoliticianKey);
          else
            table =
              AnswersView.GetAllDataByIssueLevelStateCodePoliticianKey("C",
                Politicians.GetStateCodeFromKey(page.PoliticianKey), page.PoliticianKey);
        }
        else if (_Usage == Usage.ForIntroBio)
          table =
            AnswersView.GetAllDataByIssueKeyPoliticianKey("ALLBio",
              VotePage.GetPage<SecurePoliticianPage>().PoliticianKey);
        else if (_Usage == Usage.ForIntroReasons)
          table =
            AnswersView.GetAllDataByIssueKeyPoliticianKey("ALLPersonal",
              VotePage.GetPage<SecurePoliticianPage>().PoliticianKey);
        Debug.Assert(table != null, "table != null");
        _DataTable = table
          .GroupBy(r => r.QuestionKey)
          //.Select(g => g.First())
          .ToList();
      }
      return _DataTable;
    }

    private static void SetResponseData(IList<DataRow> responses, DataRow row,
      HtmlInputHidden sequenceItem)
    {
      // if there is more than one response, attach json for additional responses to sequence
      // skip the one we are reporting
      if (responses.Count > 1)
      {
        var responseArray = responses.Where(r => r != row)
          .Select(r => new Responses
          {
            // ReSharper disable PossibleInvalidOperationException
            Answer = r.Answer(),
            Source = r.Source(),
            Date = r.DateStamp() == VotePage.DefaultDbDate ? string.Empty : r.DateStamp().ToString("d"),
            YouTubeUrl = r.YouTubeUrl(),
            YouTubeDescription = r.YouTubeDescription(),
            YouTubeRunningTime = r.YouTubeRunningTime().FormatRunningTime(),
            YouTubeSource = r.YouTubeSource(),
            YouTubeSourceUrl = r.YouTubeSourceUrl(),
            YouTubeDate = r.YouTubeDate() == VotePage.DefaultDbDate ? string.Empty : r.YouTubeDate().ToString("d"),
            YouTubeAutoDisable = r.YouTubeAutoDisable(),
            YouTubeFromCandidate = r.YouTubeAutoDisable() == YouTubeInfo.VideoUploadedByCandidateMessage,
            Sequence = r.Sequence().ToString(CultureInfo.InvariantCulture)
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
        if (DoAnswerUpdate(table, group.First().QuestionKey, showSummary, ref updateCount) ==
          SecurePage.UpdateStatus.Failure) errorCount++;
      return errorCount;
    }

    private void UpdateQuestion(string questionKey, int sequence)
    {
      var page = VotePage.GetPage<SecurePoliticianPage>();
      var responses = Answers.GetDataByPoliticianKeyQuestionKey(page.PoliticianKey, questionKey)
        .Rows.Cast<DataRow>().ToList();
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
      var button = sender as Button;
      if (button == null ||
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