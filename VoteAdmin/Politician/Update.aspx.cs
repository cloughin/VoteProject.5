using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DB;
using DB.Vote;
using Vote;
using static System.String;

namespace VoteAdmin.Politician
{
  public partial class UpdatePage : SecurePoliticianPage
  {

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        var title = PoliticianName + " - Candidate Information";
        Page.Title = title;
        H1.InnerHtml = PageCache.Politicians.GetPoliticianName(PoliticianKey);
        H3.InnerHtml = OfficeAndStatus;
        var officeKey = PageCache.Politicians.GetLiveOfficeKey(PoliticianKey);

        var body = Master.FindControl("body") as HtmlGenericControl;
        body?.Attributes.Add("data-politician-key", PoliticianKey);
        body?.Attributes.Add("data-office-key", officeKey);

        var data = Answers.GetAnswerIssuesNew(PoliticianKey, officeKey)
          .Rows.OfType<DataRow>().GroupBy(r => r.IssueId()).ToList();
        var bio = data.Where(g => g.Key < 1000).ToList();
        var issues = data.Where(g => g.Key >= 1000).ToList();
        BioAccordion.InnerHtml = Join(Empty, bio.Select(FormatIssueAccordion));
        IssuesAccordion.InnerHtml = Join(Empty, issues.Select(FormatIssueAccordion));
      }
    }

    public static void DoPoliticianUpdate(string politicianKey, PoliticianUpdateData data,
      List<UpdateAnswer.UpdateAnswerFeedback> feedback)
    {
      // handle bio & issues
      var updateAnswer = new UpdateAnswer(UpdateAnswer.Usage.ForAll);
      updateAnswer.UpdateAllAnswersNew(politicianKey, data.Issues, feedback);
    }

    private static string FormatIssueAccordion(IGrouping<int, DataRow> g)
    {
      return $"<div class=\"issue-accordion\" data-id=\"{g.First().IssueId()}\">{g.First().Issue()}" +
        "<img class=\"ajax-loader\" src=\"/images/ajax-loader16.gif\"/></div>" +
        "<div class=\"issue-accordion-content\"></div>";
    }

    public static string FormatAnswers(IEnumerable<DataRow> g, int duplicate)
    {
      var parent = new HtmlDiv();
      CreateAnswerControls(parent, g.ToList(), duplicate);
      return parent.RenderToString();
    }

    private static void SetResponseData(int questionId, ICollection<DataRow> responses, HtmlControl sequenceItem)
    {
      UpdateAnswer.OneResponse[] responseArray;
      if (responses.Count == 0)
        // create dummy respnse
        responseArray = new[] {new UpdateAnswer.OneResponse {
          QuestionId = questionId,
          Answer = Empty,
          Source = Empty,
          Date = Empty,
          YouTubeUrl = Empty,
          YouTubeDescription = Empty,
          YouTubeRunningTime = Empty,
          YouTubeSource = Empty,
          YouTubeSourceUrl = Empty,
          YouTubeDate = Empty,
          YouTubeFromCandidate = false,
          //Sequence = "?"
          Sequence = 0
        }};
      else
        responseArray = responses.Select(r => new UpdateAnswer.OneResponse
        {
          // ReSharper disable PossibleInvalidOperationException
          QuestionId = questionId,
          Answer = r.Answer(),
          Source = r.Source(),
          Date = r.DateStamp().IsDefaultDate() ? Empty : r.DateStamp().ToString("d"),
          YouTubeUrl = r.YouTubeUrl().SafeString(),
          YouTubeDescription = r.YouTubeDescription().SafeString(),
          YouTubeRunningTime = r.YouTubeRunningTime().FormatRunningTime(),
          YouTubeSource = r.YouTubeSource().SafeString(),
          YouTubeSourceUrl = r.YouTubeSourceUrl().SafeString(),
          YouTubeDate =
            r.YouTubeDate().IsDefaultDate() ? Empty : r.YouTubeDate().ToString("d"),
          YouTubeFromCandidate =
            r.YouTubeAutoDisable() == YouTubeVideoInfo.VideoUploadedByCandidateMessage,
          Sequence = r.Sequence()/*.ToString(CultureInfo.InvariantCulture)*/
          // ReSharper restore PossibleInvalidOperationException
        }).ToArray();
      var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
      sequenceItem.Attributes.Add("data-responses", javaScriptSerializer.Serialize(responseArray));
    }

    //private static void CreateActionMenu(HtmlControl menuContainer, ICollection<DataRow> responses,
    //  bool isNew)
    //{
    //  menuContainer.Controls.Clear();
    //  new HtmlDiv { InnerText = $"{responses.Count} Response{(responses.Count == 1 ? Empty : "s")}" }
    //    .AddTo(menuContainer, "response-count");
    //  new HtmlSpan { InnerText = "What do you want to do?" }.AddTo(menuContainer);
    //  var dropdown = new HtmlSelect().AddTo(menuContainer);
    //  var edit = dropdown.AddItem("Edit this response", "edit");
    //  var add = dropdown.AddItem("Add another response to this question", "add");
    //  var view = dropdown.AddItem("View or edit other responses to this question", "view");
    //  if (responses.Count == 0)
    //    menuContainer.AddCssClasses("hidden");
    //  else
    //    menuContainer.RemoveCssClass("hidden");
    //  if (isNew && responses.Count > 0)
    //  {
    //    edit.Attributes.Add("disabled", "disabled");
    //    add.Attributes.Add("selected", "selected");
    //  }
    //  else
    //  {
    //    edit.Attributes.Add("selected", "selected");
    //    if (responses.Count < 2)
    //      view.Attributes.Add("disabled", "disabled");
    //  }
    //}

    private static void CreateActionMenu2(Control menuContainer, ICollection<DataRow> responses)
    {
      menuContainer.Controls.Clear();
      new HtmlDiv { InnerText = $"{responses.Count} Response{(responses.Count == 1 ? Empty : "s")}" }
        .AddTo(menuContainer, "response-count");
      if (responses.Count > 0)
      {
        new HtmlSpan { InnerText = "What do you want to do?" }.AddTo(menuContainer);
        var dropdown = new HtmlSelect().AddTo(menuContainer);
        var edit = dropdown.AddItem("Edit this response", "edit");
        /*var add = */dropdown.AddItem("Add another response to this question", "add");
        var view = dropdown.AddItem("View or edit other responses to this question", "view");
        edit.Attributes.Add("selected", "selected");
        if (responses.Count == 1)
          view.Attributes.Add("disabled", "disabled");
      }
      //new HtmlSpan { InnerText = "What do you want to do?" }.AddTo(menuContainer);
      //var dropdown = new HtmlSelect().AddTo(menuContainer);
      //var edit = dropdown.AddItem("Edit this response", "edit");
      //var add = dropdown.AddItem("Add another response to this question", "add");
      //var view = dropdown.AddItem("View or edit other responses to this question", "view");
      //if (responses.Count == 0)
      //  menuContainer.AddCssClasses("hidden");
      //else
      //  menuContainer.RemoveCssClass("hidden");
      //if (isNew && responses.Count > 0)
      //{
      //  edit.Attributes.Add("disabled", "disabled");
      //  add.Attributes.Add("selected", "selected");
      //}
      //else
      //{
      //  edit.Attributes.Add("selected", "selected");
      //  if (responses.Count < 2)
      //    view.Attributes.Add("disabled", "disabled");
      //}
    }

    private static void CreateTextSubTabContent(Control parent, DataRow row, int duplicate)
    {
      var questionId = row.QuestionId();
      var questionIdUnique = $"{questionId}-{duplicate}";
      // the text answer field
      AddTextAreaInputElement(parent, "TextBox" + questionIdUnique,
        $"textbox{questionId} shadow answer-textbox data-field ",
        row.Answer(), "Type or paste your response into this box", true,
        null, false, "Text Response", "answer wide", null, null, null);

      // the remove-line-breaks button
      if (!IsPoliticianUser)
        AddButtonInputElement(parent, null,
          "remove-line-breaks button-2 button-smallest", "Remove Line Breaks", "remove-line-breaks");

      if (!IsPoliticianUser) // add source and date boxes
      {
        var textSourceAndDateDiv = new HtmlDiv().AddTo(parent, 
          "text-source-and-date-container source-and-date-container");
        var textSourceDiv = new HtmlDiv().AddTo(textSourceAndDateDiv,
          "text-source-container source-container");
        var sourceRequiredSpan = row.IsTextSourceOptional()
          ? Empty
          : " <span class=\"reqd\">◄</span>";
        AddTextAreaInputElement(textSourceDiv, "Source" + questionIdUnique,
         $"source{questionId} shadow source-textbox data-field ",
          row.Source(), "Copy and paste the url of the web page (with or without http(s)://) where" +
            " you obtained the answer or a description of the source into this box.", false,
          "Copy and paste the url of the web page (with or without http(s)://) where" +
            " you obtained the answer or a description of the source into this box.",
          false, "Text Source" + sourceRequiredSpan, "text-source source", null, null, null);
        var textDateDiv = new HtmlDiv().AddTo(textSourceAndDateDiv, "text-date-container date-container");
        AddTextInputElement(textDateDiv, "Date" + questionIdUnique,
          $"date{questionId} shadow-2 date-textbox date-picker data-field ",
          row.DateStamp().DbDateToShortDate(),
          "Enter the date of the answer if today is not an appropriate date. For" +
            " example if you obtained the answer from the candidate's campaign website," +
            " and the election is over, enter some date a couple days prior to the" +
            " election.", false, "Text Date", "date", null, null, null);
        AddButtonInputElement(textDateDiv, null,
          "today-button for-date button-1 button-smallest", "Today", "today for-date");
      }
    }

    private static void CreateYouTubeSubTabContent(Control parent, DataRow row, int duplicate)
    {
      var questionId = row.QuestionId();
      var questionIdUnique = $"{questionId}-{duplicate}";
      var wasUploadedByCandidate = !IsNullOrWhiteSpace(row.YouTubeUrl()) &&
        row.YouTubeSource() == YouTubeVideoInfo.VideoUploadedByCandidateMessage;

      var wasUploadedByVoteUsa = !IsNullOrWhiteSpace(row.YouTubeUrl()) &&
        row.YouTubeSource() != YouTubeVideoInfo.VideoUploadedByCandidateMessage;

      // the Source heading and Uploaded by candidate checkbox (only for master users)
      if (!IsPoliticianUser)
      {
        var heading = new HtmlDiv().AddTo(parent, "category-heading");
        new HtmlSpan { InnerText = "Select Type of Video to be Shown" }.AddTo(heading);

        AddCheckboxInputElement(parent, "YouTubeFromCandidate" + questionIdUnique,
         $"youtubefromcandidate{questionId} youtubefrom-checkbox youtubefromcandidate-checkbox ",
          null, wasUploadedByCandidate, "Complete Unedited YouTube.com video - not a YouTube video clip on the Vote USA Channel",
          "fromcandidate clearfix data-field ", null, null, null, Empty);

        AddCheckboxInputElement(parent, "YouTubeFromVoteUSA" + questionIdUnique,
          $"youtubefromvoteusa{questionId} youtubefrom-checkbox youtubefromvoteusa-checkbox data-field ",
          null, wasUploadedByVoteUsa, "YouTube Video on Vote USA Channel - a video clip created by Vote USA from some other video source and uploaded to the Vote USA Channel", "fromvoteusa clearfix",
          null, null, null, Empty);
      }

      var youTubeSourceFields = new HtmlDiv().AddTo(parent, "youtube-source-fields");

      if (!IsPoliticianUser)
      {
        AddTextInputElement(youTubeSourceFields, "YouTubeSourceUrl" + questionIdUnique,
          $"youtubesourceurl{questionId} shadow-2 youtubesourceurl-textbox data-field ",
          row.YouTubeSourceUrl(), "Enter the url (with or without http(s)://) of the source video.",
          false, "Url of the Other YouTube Source used to create the Vote USA Channel video clip", "youtubeurl wide", null, null, null);
      }

      var youTubeSourceAndDateDiv = new HtmlDiv().AddTo(youTubeSourceFields,
        "youtube-source-and-date-container source-and-date-container");
      if (wasUploadedByCandidate) youTubeSourceFields.AddCssClasses("hidden");
      else youTubeSourceFields.RemoveCssClass("hidden");

      if (!IsPoliticianUser)
      {
        var youTubeSourceDiv = new HtmlDiv().AddTo(youTubeSourceAndDateDiv,
          "youtube-source-container source-container");
        AddTextAreaInputElement(youTubeSourceDiv, "YouTubeSource" + questionIdUnique,
          $"youtubesource{questionId} shadow youtubesource-textbox data-field ",
          row.YouTubeSource(), "Enter a description of the source video into this box.", false,
          "Enter a description of the source video into this box.",
          true, "Description of the Organization or Event of Other YouTube Source", "source", 
          null, null, null, false, "Provide a short and concise description like: First NBC Democratic" +
          " Debate; KVUE Austin TX News; PBS New Hour. Do not include any urls or dates." +
          " These should be only entered in the separate Url and Date textboxes.");
        var youTubeDateDiv = new HtmlDiv().AddTo(youTubeSourceAndDateDiv,
          "youtube-date-container date-container");
        AddTextInputElement(youTubeDateDiv, "YouTubeDate" + questionIdUnique,
         $"youtubedate{questionId} shadow-2 youtubedate-textbox date-picker data-field ",
          row.YouTubeDate().DbDateToShortDate(),
          "Enter the date of the video if today is not an appropriate date.", false,
          "Date when Other Source YouTube Content was made", "youtubedate", null, null, null);
        AddButtonInputElement(youTubeDateDiv, null,
          "today-button for-youtubedate button-1 button-smallest", "Today", "today  for-youtubedate");
      }

      AddTextInputElement(parent, "YouTubeUrl" + questionIdUnique,
        $"youtubeurl{questionId} shadow-2 youtubeurl-textbox data-field ", row.YouTubeUrl(), IsPoliticianUser
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

      AddTextAreaInputElement(youTubeDescriptionDiv, "YouTubeDescription" + questionIdUnique,
        $"youtubedescription{questionId} shadow youtubedescription-textbox ",
       displayYouTubeDescription, null, false, null, false, "Video Description", "youtubedescription wide",
       null, null, null, true);

      var youTubeTimeDiv = new HtmlDiv().AddTo(youTubeDescriptionAndTimeDiv,
        "youtube-time-container");

      AddTextInputElement(youTubeTimeDiv, "YouTubeRunningTime" + questionIdUnique,
        $"youtuberunningtime{questionId} shadow-2 youtuberunningtime-textbox ",
        displayYouTubeRunningTime, null, false, "Running Time", "youtuberunningtime",
        null, null, null, true);
    }

    private static void CreateAnswerControls(HtmlControl parent, IList<DataRow> responses, int duplicate)
    {
      var row = responses.First();
      var responsesAsDataRow =
        responses.Where(r => r.SequenceOrNull() != null).ToList();
      parent.AddCssClasses("answer-panel " +
        (IsPoliticianUser ? "politician-answer-panel" : "master-answer-panel"));
      var questionId = row.QuestionId();
      var questionIdUnique = $"{questionId}-{duplicate}";

      // The panel title
      var heading = AddContainer(parent, null, null);

      // Add undo button as next sibling to heading
      var undo = CreateUndoButton("Undo" + questionIdUnique, "undo" + questionId,
        $"Revert \"{row.Question()}\" to the latest saved version");
      heading.AddAfter(undo);

      // Same for clear button
      var clear = CreateClearButton("Clear" + questionIdUnique, "clear" + questionId, 
        $"Clear \"{row.Question()}\"");
      heading.AddAfter(clear);

      var container = AddContainer(parent,
        "Container" + questionIdUnique, $"container{questionId} update-all updated answer-container"
        );
      container.Attributes.Add("data-id", questionId.ToString());

      AddHiddenField(container, "Description" + questionIdUnique, "description" + questionId, row.Question());

      AddHiddenField(container, "SubTab" + questionIdUnique, "subtab subtab-" + questionId);

      var hasNoResponses = row.SequenceOrNull() == null;

      var sequenceItem = AddHiddenField(container, "Sequence" + questionIdUnique,
        $"answer-sequence sequence{questionId}",
        hasNoResponses ? "?" : row.SequenceOrNull()?.ToString(CultureInfo.InvariantCulture));

      SetResponseData(questionId, responsesAsDataRow, sequenceItem);

      // the action menu
      var menuOuter = new HtmlDiv().AddTo(container, "menu-outer");
      var menuContainer =
        AddContainer(menuOuter, "Action" + questionIdUnique, $"action-menu action{questionId}");
      CreateActionMenu2(menuContainer, responsesAsDataRow/*, row.SequenceOrNull() == null*/);

      new HtmlP
      {
        InnerHtml =
          "To completely delete a response (Text and YouTube Video), use the Clear button (red <span>X</span> upper right) then Save Changes."
      }.AddTo(container, "delete-message");

      var subTabs =
        new HtmlDiv
        {
          ID = "answer-subtabs-" + questionIdUnique,
          ClientIDMode = ClientIDMode.Static
        }.AddTo(container, $"answer-subtabs-{questionId} answer-sub-tabs shadow");
      var subTabsTabs = new HtmlUl().AddTo(subTabs, "htabs unselectable");

      var textSubTab = new HtmlLi { EnableViewState = false }.AddTo(subTabsTabs, "tab htab");
      new HtmlAnchor
      {
        HRef = "#tab-textanswer-" + questionIdUnique,
        InnerHtml = "Text<br />Response",
        EnableViewState = false
      }.AddTo(textSubTab);

      var youTubeSubTab =
        new HtmlLi { EnableViewState = false }.AddTo(subTabsTabs, "tab htab");
      new HtmlAnchor
      {
        HRef = "#tab-youtubeanswer-" + questionIdUnique,
        InnerHtml = "YouTube<br />Response",
        EnableViewState = false
      }.AddTo(youTubeSubTab);

      var textSubTabContent = new HtmlDiv
      {
        ID = "tab-textanswer-" + questionIdUnique,
        ClientIDMode = ClientIDMode.Static
      }.AddTo(subTabs, $"tab-textanswer-{questionId}");

      var youTubeSubTabContent = new HtmlDiv
      {
        ID = "tab-youtubeanswer-" + questionIdUnique,
        ClientIDMode = ClientIDMode.Static
      }.AddTo(subTabs, $"tab-youtubeanswer-{questionId}");

      AddHiddenField(container, "HasValue" + questionIdUnique, $"hasvalue{questionId}",
        IsNullOrWhiteSpace(row.Answer()) && IsNullOrWhiteSpace(row.YouTubeUrl()) &&
        (IsNullOrWhiteSpace(row.FacebookVideoUrl()) || !AllowFacebookVideos)
          ? Empty : "Y");

      CreateTextSubTabContent(textSubTabContent, row, duplicate);
      CreateYouTubeSubTabContent(youTubeSubTabContent, row, duplicate);

      AddClearBoth(container);
    }
  }

  // must be public for JSON conversion
  // ReSharper disable once MemberCanBePrivate.Global
  //public class Responses
  //{
  //  // ReSharper disable NotAccessedField.Global
  //  public int QuestionId;
  //  public string Answer;
  //  public string Source;
  //  public string Date;
  //  public string YouTubeSourceUrl;
  //  public string YouTubeSource;
  //  public string YouTubeDate;
  //  public string YouTubeUrl;
  //  public string YouTubeDescription;
  //  public string YouTubeRunningTime;
  //  public bool YouTubeFromCandidate;
  //  public string Sequence;
  //  // ReSharper restore NotAccessedField.Global
  //}

  public sealed class PoliticianUpdateData
  {
    public UpdateAnswer.OneResponse[][] Issues;
  }
}