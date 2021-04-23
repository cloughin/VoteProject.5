define(["jquery", "vote/adminMaster", "vote/util", "jqueryui", "textchange"],
  function ($, master, util) {

    var politicianKey;
    var officeKey;

    var initialValues = {
      Issues: []
    };
    var currentValues = {
      Issues: []
    };
    var initialIssuesDict = {};
    var currentIssuesDict = {};

    var initPage = function () {

      var $body = $("body");
      var $tabBioAndIssues = $("#tab-bio,#tab-issues");
      politicianKey = $body.data("politician-key");
      officeKey = $body.data("office-key");

      $(".accordion-container").accordion({
        active: false,
        collapsible: true,
        heightStyle: "content",
        activate: util.accordionActivate
      });

      $tabBioAndIssues.on("accordionbeforeactivate", ".ui-accordion", function (event, ui) {
        event.stopPropagation(); // because it fires twice
        var $header = ui.newHeader;
        if ($header.length == 0) return; // collapsing
        var $ajaxLoader = $header.find(".ajax-loader");
        var $content = ui.newPanel;
        if ($content.html()) return;
        $ajaxLoader.css("display", "inline-block");
        var id = $header.data("id");
        if ($header.hasClass("issue-accordion")) {
          var questionId = $header.data("questionid");
          $header.data("questionid", 0);
          util.ajax({
            url: "/Admin/WebService.asmx/GetAnswerQuestionsHtml",
            data: {
              politicianKey: politicianKey,
              officeKey: officeKey,
              issueId: id
            },

            success: function (result) {
              $content.html(result.d);
              $content.find(".accordion-container").accordion({
                active: false,
                collapsible: true,
                heightStyle: "content",
                activate: util.accordionActivate
              });
              var $questionAccordion;
              $(".question-accordion", $content).each(function () {
                var $this = $(this);
                if ($this.data("id") === questionId) {
                  $questionAccordion = $this;
                  return false;
                }
              });
              if ($questionAccordion) {
                $questionAccordion.trigger("click");
              }
            },

            error: function (result) {
              util.alert(util.formatAjaxError(result, "Could not get topics"));
            },

            complete: function () {
              $ajaxLoader.css("display", "none");
            }

          });
        }
        else if ($header.hasClass("question-accordion")) {
          // We create a duplicate id to insure uniqueness of generated id's if the same question
          // is under more than one issue. We find all instances of the question's container class and
          // use the count as the duplicate id
          var duplicateId = $(".container" + id).length;
          util.ajax({
            url: "/Admin/WebService.asmx/GetAnswersHtml",
            data: {
              politicianKey: politicianKey,
              officeKey: officeKey,
              questionId: id,
              duplicate: duplicateId
            },

            success: function (result) {
              // Because the tab hrefs get ../ prepended thanks to ajax response
              $content.html(result.d.replace(/\.\.\/#tab/g, "#tab"));
              $(".answer-sub-tabs", $content).tabs(
              {
                show: 400
              });
              $(".date-picker", $content).datepicker({
                changeYear: true,
                yearRange: "2010:+0"
              });
              if (duplicateId) {
                // this is a duplicate question, initialize it from the stored current responses
                updateIssueContent($content, currentIssuesDict[id], 0);

              } else {
                // this is the first instance of the question, store the values as current and original responses
                var responses = $(".answer-sequence", $content).data("responses");
                initialValues.Issues.push(responses);
                initialIssuesDict[id] = responses;
                var clone = [];
                for (var x = 0; x < responses.length; x++)
                  clone[x] = Object.assign({}, responses[x]);
                currentValues.Issues.push(clone);
                currentIssuesDict[id] = clone;
                enableClearAndUndo($content.find(".answer-container"));

              }
            },

            error: function (result) {
              util.alert(util.formatAjaxError(result, "Could not get answers"));
            },

            complete: function () {
              $ajaxLoader.css("display", "none");
            }

          });
        }
      });

      $tabBioAndIssues.on("click", ".remove-line-breaks",
        function () {
          var $textarea = $(this).closest(".answer-sub-tabs").find(".answer-textbox");
          $textarea.val(util.replaceLineBreaksWithSpaces($textarea.val())).change();
        });

      $tabBioAndIssues.on("click", ".today-button",
        function () {
          var today = new Date();
          today = (today.getMonth() + 101).toString().substr(1) + "/" +
            (today.getDate() + 100).toString().substr(1) + "/" +
            today.getFullYear();
          $(this).closest(".date-container").find(".date-picker").val(today).change();
        });

      $tabBioAndIssues.on("change", ".youtubefrom-checkbox",
        function () {
          var $checkbox = $(this);
          var $context = $checkbox.closest(".answer-sub-tabs");
          var otherClass = "youtubefromvoteusa-checkbox";
          var hide = true;
          if ($checkbox.hasClass(otherClass)) {
            otherClass = "youtubefromcandidate-checkbox";
            hide = false;
          }
          if ($checkbox.is(":checked")) {
            $context.find("." + otherClass).prop("checked", false);
          } else {
            $checkbox.prop("checked", true);
          }
          $context.find(".youtube-source-fields").toggleClass("hidden", hide);
        });

      $tabBioAndIssues.on("propertychange change click keyup input paste", ".data-field", onChangeIssuesData);

      $tabBioAndIssues.on("click", ".clear-button div", function () {
        if ($(this).hasClass("disabled")) return;

        var $container = $(this).closest(".answer-panel").find(".answer-container");
        var questionId = $container.data("id");
        var sequence = $(".answer-sequence", $container).val();
        updateIssueContent($container, null, getResponseIndexFromSequence(questionId, sequence));
        $(".answer-textbox", $container).trigger("change"); // any data element will do
      });

      $tabBioAndIssues.on("click", ".undo-button div", function () {
        if ($(this).hasClass("disabled")) return;

        var $container = $(this).closest(".answer-panel").find(".answer-container");
        var questionId = $container.data("id");
        var clone = [];
        for (var x = 0; x < initialIssuesDict[questionId].length; x++)
          clone[x] = Object.assign({}, initialIssuesDict[questionId][x]);
        currentIssuesDict[questionId] = clone;
        $.each(currentValues.Issues, function(index) {
          if (this[0].QuestionId === clone[0].QuestionId) {
            currentValues.Issues[index] = clone;
            return false;
          }
        });
        forEachQuestionInstance(questionId, function($container) {
          updateIssueContent($container, initialIssuesDict[questionId],
            initialIssuesDict[questionId][0].Sequence);
          $(".answer-textbox", $container).trigger("change"); // any data element will do
        });
      });

      $tabBioAndIssues.on("change", ".action-menu select", function() {
        doActionMenuChange($(this));
      });

      $("#tab-issues .search-box input")
        .on("textchange", onTextchangeTopicSearch)
        .on("focus", function () {
          $("#tab-issues .search-box .results").show(500);
        })
        .on("blur", function () {
          $("#tab-issues .search-box .results").hide(500);
        });

      $("#tab-issues .search-box .results").on("click", "p", onClickSearchResult);


      $(".save-button").on("click", function() {
        if ($(this).hasClass("disabled")) return;
        util.openAjaxDialog("Updating...");
        util.ajax({
          url: "/Admin/WebService.asmx/DoPoliticianUpdate",
          data: {
            politicianKey: politicianKey,
            data: currentValues
          },

          success: function (/*result*/) {
          },

          error: function (result) {
            util.alert(util.formatAjaxError(result, "Could not update"));
          },

          complete: function () {
            util.closeAjaxDialog();
          }

        });
      });

      $(".cancel-button").on("click", function()
      {
        if ($(this).hasClass("disabled")) return;

        // use JSON to clone initial to current
        var jsonInitial = JSON.stringify(initialValues);
        currentValues = JSON.parse(jsonInitial);

        // rebuild the bio/issues dictionary
        currentIssuesDict = {};
        $.each(currentValues.Issues, function() {
          currentIssuesDict[this[0].QuestionId] = this;
        });

        // update all bio/issues panels
        $(".answer-container", $tabBioAndIssues).each(function() {
          var $container = $(this);
          var questionId = $container.data("id");
          var sequence = $(".answer-sequence", $container).val();
          updateIssueContent($container, currentIssuesDict[questionId],
            getResponseIndexFromSequence(questionId, sequence));
          $(".answer-textbox", $container).trigger("change"); // any data element will do
        });
      });
  };

    //var isImageTooSmall = function () {
    //  var width = $("#tab-upload .image-picture").width();
    //  if (!width) return false;
    //  return !!(width < 300);
    //};

    //var reCheckImageSize = function () {
    //  if (isImageTooSmall())
    //    $("#tab-upload p.too-small").show(400);
    //  else
    //    $("#tab-upload p.too-small").hide(400);
    //};

    var nextSearchId = 0;
    var expectedSearchId;

    function forEachQuestionInstance(questionId, fn) {
      $(".answer-container.container" + questionId).each(function() {
        fn($(this));
      });
    }

    function doActionMenuChange($select) {
      var $container = $select.closest(".answer-container");
      var $sequence = $(".answer-sequence", $container);
      var sequence = $sequence.val();
      var questionId = $container.data("id");
      var responses = currentIssuesDict[questionId];
      //var $containers = $(".answer-container.container" + questionId);

      switch ($select.val()) {
        case "edit":
          // this should never be changed to. If not selected, it's always disabled.
          break;

        case "add":
          // clear the text boxes and store the next available number as the sequence,
          // disable the "add" button, and enable the "view" button
          // select the "edit" button
          // add new entry to responses

          responses.unshift({
            QuestionId: questionId,
            Answer: "",
            Source: "",
            Date: "",
            YouTubeUrl: "",
            YouTubeSource: "",
            YouTubeSourceUrl: "",
            YouTubeDescription: "",
            YouTubeRunningTime: "",
            YouTubeDate: "",
            YouTubeAutoDisable: null,
            YouTubeFromCandidate: false,
            Sequence: responses[0].Sequence + 1
            //Sequence: (parseInt(responses[0].Sequence) + 1).toString()
            //Sequence: "?"
          });

          forEachQuestionInstance(questionId, function($container) {
            $(".answer-textbox", $container).val("");
            $(".source-textbox", $container).val("");
            $(".date-textbox", $container).val("");
            $(".youtubeurl-textbox", $container).val("");
            $(".youtubedescription-textbox", $container).val("");
            $(".youtubedate-textbox", $container).val("");
            $(".youtubesource-textbox", $container).val("");
            $(".youtubesourceurl-textbox", $container).val("");
            $(".youtuberunningtime-textbox", $container).val("");
            $(".fromcandidate .kalypto-container a", $container).removeClass("checked");
            $(".fromcandidate .kalypto-container input", $container).prop("checked", false);
            $(".fromvoteusa .kalypto-container a", $container).removeClass("checked");
            $(".fromvoteusa .kalypto-container input", $container).prop("checked", false);
            $(".answer-sequence", $container).val("?");
            $("option[value='add']", $container).prop("disabled", true);
            $("option[value='view']", $container).prop("disabled", false);
            $select.val("edit");
            $(".answer-textbox", $container).trigger("change");
          });

          break;

        case "view":
          // restore action in case dialog is cancelled
          $select.val(sequence === "?" ? "add" : "edit");
          var $dialog = $("#view-responses-dialog");
          if (!$dialog.length) {
            $("body").append('<div id="view-responses-dialog" class="hidden"></div>');
            $dialog = $("#view-responses-dialog");
            $dialog.dialog({
              autoOpen: false,
              width: "95vw",
              resizable: true,
              title: "Other Responses for This Question",
              dialogClass: 'view-responses-dialog overlay-dialog',
              // custom open and close to fix various problems
              open: master.onOpenJqDialog,
              close: master.onCloseJqDialog,
              modal: true
            });
            $dialog.on("click", "input.edit-other-response", function () {
              doEditOtherResponse($(this));
            });
          }

          fillViewResponsesDialog($container);

          $dialog.dialog("open");
          break;
      }
}

    function doEditOtherResponse($button) {
      var $dialog = $("#view-responses-dialog");
      var questionId = $dialog.data("id");
      var sequence = $button.data("seq");
      var $containers = $(".answer-container.container" + questionId);
      var responses = currentIssuesDict[questionId];
      var response;
      $.each(responses, function () {
        if (this.Sequence == sequence) {
          response = this;
          return false;
        }
      });

      // replace the text boxes, reset the changed status, store the sequence,
      // enable and select the "edit" button in each container
      $containers.each(function() {
        var $container = $(this);
        $(".answer-textbox", $container).val(response.Answer);
        $(".source-textbox", $container).val(response.Source);
        $(".date-textbox", $container).val(response.Date);
        $(".youtubeurl-textbox", $container).val(response.YouTubeUrl);
        $(".youtubedescription-textbox", $container).val(response.YouTubeDescription);
        $(".youtubedate-textbox", $container).val(response.YouTubeDate);
        $(".youtubesource-textbox", $container).val(response.YouTubeSource);
        $(".youtubesourceurl-textbox", $container).val(response.YouTubeSourceUrl);
        $(".youtuberunningtime-textbox", $container).val(response.YouTubeRunningTime);
        $(".youtube-source-fields", $container).toggleClass("hidden", response.YouTubeFromCandidate);
        var fromCandidate = response.YouTubeUrl && response.YouTubeFromCandidate;
        var fromVoteUsa = response.YouTubeUrl && !response.YouTubeFromCandidate;
        $(".fromcandidate .kalypto-container a", $container).toggleClass("checked", fromCandidate);
        $(".fromcandidate .kalypto-container input", $container).prop("checked", fromCandidate);
        $(".fromvoteusa .kalypto-container a", $container).toggleClass("checked", fromVoteUsa);
        $(".fromvoteusa .kalypto-container input", $container).prop("checked", fromVoteUsa);
        $(".answer-sequence", $container).val(response.Sequence);
        $("option[value='edit']", $container).prop("disabled", false);
        $(".action-menu", $container).val("edit");
        //$("option[value='edit']", $container).prop("selected", true);
        //$("option[value='view']", $container).prop("disabled", responses.length < 2);
      });

      $dialog.dialog("close");
    }

    function fillViewResponsesDialog($container) {
      var $sequence = $(".answer-sequence", $container);
      var sequence = $sequence.val();
      var questionId = $container.data("id");
      var responses = currentIssuesDict[questionId];
      var $dialog = $("#view-responses-dialog");
      $dialog.data("id", questionId);

      var html = [];
      for (var inx = 0; inx < responses.length; inx++) {
        var response = responses[inx];
        if (response.Sequence == sequence) continue; // don't show the one we're already editing
        html.push('<div class="one-response clearfix">');
        if (response.YouTubeUrl) {
          html.push('<div class="youtube"><span>YouTube: </span>');
          html.push(response.YouTubeUrl);
          html.push('</div>');
          html.push('<div class="youtube-description">');
          html.push(util.replaceLineBreaksWithParagraphs(response.YouTubeDescription));
          html.push('</div>');
          if (response.YouTubeSource) {
            html.push('<div class="youtube-source-text"><span>YouTube Source: </span>');
            html.push(response.YouTubeSource);
            html.push(" (");
            html.push(response.YouTubeDate);
            html.push(")");
            html.push('</div>');
          }
        }
        var answer = response.Answer.trim() ? response.Answer : "<no text>";
        html.push('<div class="answer-text">');
        html.push(util.replaceLineBreaksWithParagraphs(util.htmlEscapeText(answer)));
        html.push('</div>');
        html.push('<input type="button" value="Edit this response" class="edit-other-response button-2 button-smallest" data-seq="');
        html.push(response.Sequence);
        html.push('" />');
        if (response.Source) {
          html.push('<div class="source-text"><span>Source: </span>');
          html.push(response.Source);
          html.push(" (");
          html.push(response.Date);
          html.push(")");
          html.push('</div>');
        }
        html.push('</div>');
      }

      $dialog.html(html.join(""));
    }

    function enableClearAndUndo($container) {
      var questionId = $container.data("id");
      var response = currentIssuesDict[questionId]/*[responseIndex]*/;
      var initialResponse = initialIssuesDict[questionId]/*[responseIndex]*/;
      var canUndo = JSON.stringify(response) != JSON.stringify(initialResponse);
      $(".undo-button div", $container.closest(".answer-panel")).toggleClass("disabled", !canUndo);
    }

    function getResponseIndexFromSequence(questionId, sequence) {
      var responses = currentIssuesDict[questionId];
      var responseIndex;
      $.each(responses, function (inx) {
        if (this.Sequence == sequence) {
          responseIndex = inx;
          return false;
        }
      });
      return responseIndex;
    }

    function onChangeIssuesData() {
      var $container = $(this).closest(".answer-container");
      var questionId = $container.data("id");
      var sequence = $(".answer-sequence", $container).val();
      var responseIndex = getResponseIndexFromSequence(questionId, sequence);
      if (responseIndex === null) return;
      var responses = currentIssuesDict[questionId];
      var response = responses[responseIndex];

      // update the data
      response.Answer = $(".answer-textbox", $container).val();
      response.Source = $(".source-textbox", $container).val();
      response.Date = $(".date-textbox", $container).val();
      response.YouTubeUrl = $(".youtubeurl-textbox", $container).val();
      response.YouTubeDescription = $(".youtubedescription-textbox", $container).val();
      response.YouTubeDate = $(".youtubedate-textbox", $container).val();
      response.YouTubeSource = $(".youtubesource-textbox", $container).val();
      response.YouTubeSourceUrl = $(".youtubesourceurl-textbox", $container).val();
      response.YouTubeRunningTime = $(".youtuberunningtime-textbox", $container).val();
      response.YouTubeFromCandidate =
        $(".youtubefromcandidate-checkbox", $container).prop("checked");
      enableClearAndUndo($container);

      // if same question/response is in multiple topics, update others
      var $others = $(".container" + questionId, $("#tab-bio,#tab-issues"));
      if ($others.length > 1)
        $others.each(function() {
          if (this !== $container[0])
          {
            // don't do to self
            var $otherContainer = $(this);
            if (sequence == $(".answer-sequence", $otherContainer).val())
            {
              // ...and only if on same response
              updateIssueContent($otherContainer, responses, responseIndex);
              enableClearAndUndo($otherContainer);
            }
          }
        });

      // check if changed
      var isChanged = JSON.stringify(currentValues) != JSON.stringify(initialValues);
      $("body").toggleClass("has-changes", isChanged);
      $(".change-button-line .div-button").toggleClass("disabled", !isChanged);
    }

    function onClickSearchResult() {
      var $this = $(this);
      var issueId = $this.data("issueid");
      var questionId = $this.data("questionid");
      // find the issue accordion
      var $issueAccordion;
      $("#tab-issues .issue-accordion").each(function () {
        var $this = $(this);
        if ($this.data("id") === issueId) {
          $issueAccordion = $this;
          return false;
        }
      });
      if ($issueAccordion) {
        var $issueContent = $issueAccordion.next();
        if ($issueContent.html()) { // already populated
          var $questionAccordion;
          $(".question-accordion", $issueContent).each(function () {
            var $this = $(this);
            if ($this.data("id") === questionId) {
              $questionAccordion = $this;
              return false;
            }
          });
          if ($questionAccordion) {
            if (!$questionAccordion.hasClass("ui-state-active")) $questionAccordion.trigger("click");
            if (!$issueAccordion.hasClass("ui-state-active")) $issueAccordion.trigger("click");
          }
        } else { // must populate
          $issueAccordion.data("questionid", questionId).trigger("click");
        }
      }
    }

    function onTextchangeTopicSearch() {
      expectedSearchId = nextSearchId++;
      util.ajax({
        url: "/Admin/WebService.asmx/SearchIssuesAndTopics",
        data: {
          searchString: $("#tab-issues .search-box input").val(),
          politicianKey: politicianKey,
          id: expectedSearchId
        },

        success: function (result) {
          // guard against race
          if (result.d.Id === expectedSearchId) {
            $("#tab-issues .search-box .results").html(result.d.Html);
          }
        }

      });
    }

    function updateIssueContent($container, responses, inx) {
      var response = responses ? responses[inx] : null;
      if (response) {
        $(".answer-textbox", $container).val(response.Answer);
        $(".source-textbox", $container).val(response.Source);
        $(".date-textbox", $container).val(response.Date);
        $(".youtubeurl-textbox", $container).val(response.YouTubeUrl);
        $(".youtubedescription-textbox", $container).val(response.YouTubeDescription);
        $(".youtubedate-textbox", $container).val(response.YouTubeDate);
        $(".youtubesource-textbox", $container).val(response.YouTubeSource);
        $(".youtubesourceurl-textbox", $container).val(response.YouTubeSourceUrl);
        $(".youtuberunningtime-textbox", $container).val(response.YouTubeRunningTime);
        $(".youtube-source-fields", $container)
          .toggleClass("hidden", response.YouTubeFromCandidate);
        var fromCandidate = response.YouTubeUrl && response.YouTubeFromCandidate;
        var fromVoteUsa = response.YouTubeUrl && !response.YouTubeFromCandidate;
        $(".fromcandidate .kalypto-container a", $container)
          .toggleClass("checked", fromCandidate);
        $(".fromcandidate .kalypto-container input", $container)
          .prop("checked", fromCandidate);
        $(".fromvoteusa .kalypto-container a", $container)
          .toggleClass("checked", fromVoteUsa);
        $(".fromvoteusa .kalypto-container input", $container).prop("checked", fromVoteUsa);
        $(".answer-sequence", $container).val(response.Sequence);
        var canAdd = true;
        //$.each(responses, function() {
        //  if (this.Sequence == "?")
        //    canAdd = false;
        //});
        $("option[value='edit']", $container).prop("disabled", false);
        $("option[value='edit']", $container).prop("selected", true);
        $("option[value='add']", $container).prop("disabled", !canAdd);
        $("option[value='view']", $container).prop("disabled", responses == null || responses.length < 2);
      } else {
        $(".answer-textbox", $container).val("");
        $(".source-textbox", $container).val("");
        $(".date-textbox", $container).val("");
        $(".youtubeurl-textbox", $container).val("");
        $(".youtubedescription-textbox", $container).val("");
        $(".youtubedate-textbox", $container).val("");
        $(".youtubesource-textbox", $container).val("");
        $(".youtubesourceurl-textbox", $container).val("");
        $(".youtuberunningtime-textbox", $container).val("");
        $(".youtube-source-fields", $container).toggleClass("hidden", false);
        $(".fromcandidate .kalypto-container a", $container).toggleClass("checked", false);
        $(".fromcandidate .kalypto-container input", $container).prop("checked", false);
        $(".fromvoteusa .kalypto-container a", $container).toggleClass("checked", false);
        $(".fromvoteusa .kalypto-container input", $container).prop("checked", false);
        $(".answer-sequence", $container).val("?");
        $("option[value='edit']", $container).prop("disabled", false);
        $("option[value='edit']", $container).prop("selected", true);
        $("option[value='view']", $container).prop("disabled", true);
      }
    }

    master.inititializePage({
      callback: initPage
    });
  });