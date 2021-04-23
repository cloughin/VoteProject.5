define(["jquery", "vote/adminMaster", "vote/util", "moment", "jqueryui"],
  function ($, master, util, moment) {

    var issuesAndTopics;
    var issueDescriptions = {};
    var politicianKey;

    function beginAjax() {
      $(".overlay").removeClass("hide");
    }

    function endAjax() {
      $(".overlay").addClass("hide");
    }

    function hideHideable() {
      $(".hideable").addClass("hide");
    }

    function replaceFakeVideoPlayer() {
      var $this = $(this);
      var $videoPlayer = $this.parent();
      // first restore any existing activated players
      $(".video-player iframe").each(function() {
        var $player = $(this).parent();
        $(this).replaceWith($(this).parent().data("html"));
        $(">div", $player).click(replaceFakeVideoPlayer);
      });
      var $iframe = $("<iframe></iframe>",
        {
          src: "//www.youtube.com/embed/" +
            this.parentNode.dataset.id +
            "?rel=0&autoplay=1&autohide=1&border=0&wmode=opaque&enablejsapi=1&controls=2&showinfo=0",
          frameborder: "0",
          allowfullscreen: "allowfullscreen",
          "class": "video-iframe youtube-iframe"
        });
      // replace the <div> html and save the old html on the .video-player element to be able to restore it
      // so only one player can be active at a time
      $videoPlayer.data("html", "<div>" + $this.replaceWith($iframe).html() + "</div>");
    }

    function initFakeVideoPlayer($container) {
      $(".video-player", $container).each(function () {
        var $div = $("<div></div>");
        var $this = $(this);
        var type = $this.data("type");
        var id = $this.data("id");
        var thumbSource = "//i.ytimg.com/vi/" + id + "/hqdefault.jpg";
        $div.html('<img class="video-thumb" src="' + thumbSource + '">' +
          '<div class="video-play-button ' + type + '-play-button"></div>');
          $div.click(replaceFakeVideoPlayer);
        $this.append($div);
      });
    }

    function millisecondsToDisplay(ms) {
      var secsIn = Math.trunc(ms / 1000);
      var milliSecs = ms % 1000;
      if (milliSecs >= 500) secsIn++;
      var hours = Math.trunc(secsIn / 3600);
      var remainder = secsIn % 3600;
      var minutes = Math.trunc(remainder / 60);
      var seconds = Math.round(remainder % 60);

      return ((hours == 0 ? "" : hours + ":") + minutes + ":" + seconds );
    }

    function displayAnswers(a) {
      var count = a.length;
      var s = "s";
      switch (a.length) {
        case 0:
          count = "no";
          break;

        case 1:
          s = "";
          break;
      }
      $(".answers-container .msg").text("You have " + count + " response" + s +
        " to this topic");

      var html = [];
      $.each(a, function () {
        html.push('<div class="answer response" data-is-video="' + this.IsVideo +
          '" data-question-id="' + this.QuestionId + '" data-sequence="' +
          this.Sequence + '">');
        if (this.IsVideo) {
          html.push('<span class="head">YouTube video posted ' +
            moment(new Date(this.YouTubeDate)).format("M/D/YYYY") + '</span>');
          html.push('<div class="label">YouTube URL</div>' +
            '<input type="text" class="answer-youtube-url the-answer"' +
            ' value="' + this.YouTubeUrl + '" disabled="disabled" />');
          if (this.YouTubeAutoDisable) {
            html.push('<div class="label error">' + util.htmlEscapeText(this.YouTubeAutoDisable) + '</div>');
          } else {
            html.push('<div class="video-wrapper-outer">');
            html.push('<div class="video-container youtube-container">');
            html.push('<div class="video-player youtube-player" data-type="yt" data-id="' + this.YouTubeId + '">');
            html.push('</div></div>');
            html.push('<p class="video-info">');
            if (this.YouTubeRunningTime != 0)
              html.push('<span class="duration">[' + millisecondsToDisplay(this.YouTubeRunningTime) + ']</span> ');
            html.push(util.htmlEscapeText(this.YouTubeDescription) + '</p>');
            if (this.YouTubeSource) {
              html.push('<p class="response-source">');
              html.push('<span>Source: </span>' + util.htmlEscapeText(this.YouTubeSource) + '</p>');
            }
            html.push('</div>');
          }
        } else {
          html.push('<span class="head">Response posted ' +
            moment(this.DateStamp).format("M/D/YYYY") + '</span>');
          //html.push('<textarea class="answer-text the-answer" rows="6" disabled="disabled">' +
          //  this.Answer + '</textarea>');
          html.push('<div class="answer-text the-answer">' +
            util.htmlEscapeText(this.Answer) + '</div>');
          if (this.Source) {
            html.push('<p class="response-source">');
            html.push('<span>Source: </span>' + util.htmlEscapeText(this.Source) + '</p>');
          }
        }
        html.push('<input type="button" value="Edit this response"' +
          ' class="edit-response button-1 button-smallest"/>' +
          '<input type="button" value="Delete response"' +
          ' class="delete-response button-3 button-smallest"/>');
        html.push('</div>');
      });

      $(".answers-container .answers").html(html.join(""));
      initFakeVideoPlayer($(".answers-container"));
      hideHideable();
      $(".answers-container").removeClass("hide");
    }

    var initPage = function () {
      politicianKey = $("body").data("politician-key");
      issuesAndTopics = $("body").data("issues-and-topics");
      $(".issues-select option").each(function () {
        var $this = $(this);
        var val = $this.val();
        if (val)
          issueDescriptions[val] = $this.text();
      });

      $(".search-text").on("input", onSearchInputChange);

      $(".search-text-container .cancel-box").on("click", function() {
        $(".search-text").val("").trigger("input");
      });

      $(".search-results").on("click", ".result", function() {
        var $this = $(this);
        var issueId = $this.data("issue-id");
        var questionId = $this.data("question-id");
        $(".search-results").addClass("hide");
        $(".issues-select").val(issueId).trigger("change");
        $(".topics-select").val(questionId).trigger("change");
      });

      $(".issues-select").on("change", function() {
        var issueId = $(this).val();
        var $topicsSelect = $(".topics-select");
        if (issueId) {
          issueId = parseInt(issueId);
          var options = ['<option value="">&lt; select a topic &gt;</option>'];
          $.each(issuesAndTopics, function() {
            if (this.I === issueId) {
              $.each(this.Q, function() {
                options.push('<option value="' + this.I + '">' + this.Q + '</option>');
              });
              return false;
            }
          });
          $topicsSelect.html(options.join("")).removeClass("hide");
        } else {
          $topicsSelect.html("").addClass("hide");
        }
        $topicsSelect.trigger("change");
      });

      $(".topics-select").on("change", function() {
        var questionId = $(this).val();
        if (!questionId) {
          hideHideable();
          return;
        }
        beginAjax();
        util.ajax({
          url: "/Admin/WebService.asmx/GetAnswers",
          data: {
            questionId: questionId,
            politicianKey: politicianKey
          },

          success: function (result) {
            displayAnswers(result.d);

          },

          complete: function () {
            endAjax();
          },

          error: function (p) {
            util.alert(util.formatAjaxError(p, "Could not get answers"));
          }
        });
      });

      $(".add-text-response").on("click", function () {
        hideHideable();
        $(".new-text-response .the-answer").val("");
        $(".new-text-response").removeClass("hide");
      });

      $(".add-youtube-response").on("click", function () {
        hideHideable();
        $(".new-youtube-response .the-answer").val("");
        $(".new-youtube-response").removeClass("hide");
      });

      $(".cancel-response").on("click", function () {
        hideHideable();
        $(".answers-container").removeClass("hide");
      });

      $(".save-response").on("click", function () {
        var $response = $(this).closest(".response");
        var isVideo = $response.data("is-video");
        var questionId = $(".topics-select").val();
        var value = $(".the-answer", $response).val();
        beginAjax();
        util.ajax({
          url: "/Admin/WebService.asmx/SaveResponse",
          data: {
            politicianKey: politicianKey,
            isVideo: isVideo,
            questionId: questionId,
            value: value
          },

          success: function (result) {
            if (result.d.ErrorMessage) {
              util.alert(result.d.ErrorMessage);
              return;
            }
            util.alert("Response added", "Info");
            displayAnswers(result.d.Answers);
          },

          complete: function () {
            endAjax();
          },

          error: function (p) {
            util.alert(util.formatAjaxError(p, "Could not add this response"));
          }
        });
      });

      $(".update-response").on("click", function () {
        var $response = $(this).closest(".response");
        var isVideo = $response.data("is-video");
        var questionId = $(".question-id", $response).val();
        var sequence = $(".sequence", $response).val();
        var newValue = $(".the-answer", $response).val();
        beginAjax();
        util.ajax({
          url: "/Admin/WebService.asmx/UpdateResponse",
          data: {
            politicianKey: politicianKey,
            isVideo: isVideo,
            questionId: questionId,
            sequence: sequence,
            newValue: newValue
          },

          success: function (result) {
            if (result.d.ErrorMessage) {
              util.alert(result.d.ErrorMessage);
              return;
            }
            util.alert("Response updated", "Info");
            displayAnswers(result.d.Answers);
          },

          complete: function () {
            endAjax();
          },

          error: function (p) {
            util.alert(util.formatAjaxError(p, "Could not update this response"));
          }
        });
      });

      $(".answers").on("click", ".edit-response", function () {
        var $answer = $(this).closest(".answer");
        var isVideo = $answer.data("is-video");
        var $response = isVideo ? $(".edit-youtube-response") : $(".edit-text-response");
        $(".question-id", $response).val($answer.data("question-id"));
        $(".sequence", $response).val($answer.data("sequence"));
        $(".head", $response).text("Edit " + $(".head", $answer).text().toLowerCase());
        var answer = isVideo
          ? $(".the-answer", $answer).val()
          : $(".the-answer", $answer).text();
        $(".the-answer", $response).val(answer);
        hideHideable();
        $response.removeClass("hide");
      });

      $(".answers").on("click", ".delete-response", function () {
        var $response = $(this).closest(".response");
        var isVideo = $response.data("is-video");
        //$(".question-id", $response).val($answer.data("question-id"));
        //$(".sequence", $response).val($answer.data("sequence"));
        var msg = "Are you sure you want to delete the " + $(".head", $response).text().toLowerCase();
        util.confirm(msg, function(button) {
          if (button === "Ok") {
            beginAjax();
            util.ajax({
              url: "/Admin/WebService.asmx/DeleteResponse",
              data: {
                politicianKey: politicianKey,
                isVideo: isVideo,
                questionId: $response.data("question-id"),
                sequence: $response.data("sequence")
              },

              success: function (result) {
                if (result.d.ErrorMessage) {
                  util.alert(result.d.ErrorMessage);
                  return;
                }
                util.alert("Response deleted", "Info");
                displayAnswers(result.d.Answers);
              },

              complete: function () {
                endAjax();
              },

              error: function (p) {
                util.alert(util.formatAjaxError(p, "Could not delete this response"));
              }
            });
          }
        });
      });

    };

    function onSearchInputChange() {

      function format(desc) {
        var regex = /\S+/g;
        return desc.replace(regex, function (match) {
          var matchLower = match.toLowerCase();
          var isKw = false;
          $.each(kws, function () {
            if (matchLower.indexOf(this.toString()) >= 0) {
              isKw = true;
            }
          });
          return isKw ? "<b>" + match + "</b>" : match;
        });
      }

      var rawKws = $(".search-text").val().toLowerCase().split(" ");
      var kws = [];
      // only use kws at least two characters
      $.each(rawKws, function () {
        if (this.length >= 2)
          kws.push(this.toString());
      });
      var hits = [];
      $.each(issuesAndTopics, function () {
        var issueObj = this;
        var issueDesc = issueDescriptions[issueObj.I];
        var issueDescLower = issueDesc.toLowerCase();
        var matchesIssue = 0;
        $.each(kws, function() {
          if (issueDescLower.indexOf(this.toString()) >= 0) {
            matchesIssue++;
          }
        });
        // look for matching questions
        $.each(issueObj.Q, function() {
          var matchesQuestion = matchesIssue;
          var questionDescLower = this.Q.toLowerCase();
          $.each(kws, function () {
            if (questionDescLower.indexOf(this.toString()) >= 0) {
              matchesQuestion++;
            }
          });
          if (matchesQuestion > 0) {
            hits.push({
              issueId: issueObj.I,
              issue: issueDesc,
              questionId: this.I,
              question: this.Q,
              matches: matchesQuestion
            });
          }
        });
      });
      var $searchResults = $(".search-results");
      if (hits.length == 0) {
        $searchResults.addClass("hide");
      } else {
        var results = [];
        hits.sort(function(a, b) {
          if (a.matches > b.matches) return -1;
          if (a.matches < b.matches) return 1;
          return 0;
        });
        $.each(hits, function() {
          results.push('<p class="result" data-issue-id="' + this.issueId +
            '" data-question-id="' + this.questionId + '"><span class="q">' + format(this.question) + '</span>' +
            '<span class="in"> in <span class="i">' + format(this.issue) + '</span></span></p>');
        });
        $(".results", $searchResults).html(results.join("")).removeClass("hide");
        $searchResults.removeClass("hide");
      }
    }

    master.inititializePage({
      callback: initPage
    });
  });