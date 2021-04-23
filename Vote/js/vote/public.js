var PUBLIC = (function ($) {

  //var getUpcomingElectionsCalled
  var $sampleBallotMenu;
  var canUpdateBallot = true;

  window.$ = jQuery;

  function accordionActivate(event, ui) {
    // make sure heading is visible
    var offset = ui.newHeader.offset();
    if (offset) {
      offset = ui.newHeader.offset().top - $(window).scrollTop();
      if (offset < 0 || offset > window.innerHeight) {
        // Not in view so scroll to it
        $('html,body').animate({ scrollTop: ui.newHeader.offset().top }, 200);
      }
    }
  }

  function setCanUpdateBallot(val) {
    canUpdateBallot = !!val;
  }

  function updateOfficeChecks($officeCell, checks) {
    if (!checks) {
      // use My Choices
      var election = getOfficeElection($officeCell);
      var checksStr = window.localStorage.getItem(election + "." + $officeCell.data("key"));
      if (checksStr) checks = JSON.parse(checksStr);
      else checks = {};
    }
    $(".candidate-cell", $officeCell).each(function () {
      var $candidateCell = $(this);
      var checked = false;
      if ($candidateCell.hasClass("write-in-cell")) {
        var val = checks.writeIn;
        if (typeof val === "string") {
          checked = true;
          $candidateCell.addClass("checked");
        } else {
          val = "";
        }
        $("input.write-in-text", $candidateCell).val(val);
      } else {
        checked = checks[$candidateCell.data("key")] === 1;
      }
      $("a.candidate-checkbox", $candidateCell).toggleClass("checked", checked);
      $("input.candidate-checkbox", $candidateCell).prop("checked", checked);
    });
  }
  
  function updateBallotMeasureChecks($referendumContent, val) {
    // val === false: use My Choices
    // val === null: clear both
    // val === "0" or "1": no or yes
    if (val === false) {
      // use My Choices
      val = window.localStorage.getItem($("body").data("election") + "." + 
        $referendumContent.data("key"));
    }
    $(".yes,.no", $referendumContent).each(function () {
      var $this = $(this);
      var checked = $this.hasClass("yes") && val === "1" || $this.hasClass("no") && val === "0";
      $("a.referendum-checkbox", $this).toggleClass("checked", checked);
      $("input.referendum-checkbox", $this).prop("checked", checked);
    });
  }

  function getAllBallotChecks() {
    var result = {};
    if (localStorageIsAvailable()) {
      $(".office-cell").each(function () {
        var $officeCell = $(this);
        var officeKey = $officeCell.data("key");
        var electionKey = getOfficeElection($officeCell);
        var checksStr = window.localStorage.getItem(electionKey + "." + officeKey);
        if (checksStr) result[officeKey] = JSON.parse(checksStr);
      });
      $(".referendum-content").each(function () {
        var $this = $(this);
        var refkey = $this.data("key");
        var electionKey = $("body").data("election");
        var val = window.localStorage.getItem(electionKey + "." + refkey);
        if (val != null) result[refkey] = val;
      });
    }
    return result;
  }

  function initAds() {
    var $body = $("body");
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/GetAds",
      data: "{'electionKey':'" + $body.data("adelection") + "','officeKey':'" +
        $body.data("adoffice") + "','adKey':'" + ($.queryString("ad") || '') + "'}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (result) {
        if (result.d) {
          setTimeout(function () { $(".header-inner").before(result.d); });
        }
      }
    });
  }

  function initAnswerAccordions() {
    $(".accordion-container").accordion({
      collapsible: true,
      heightStyle: "content",
      activate: PUBLIC.accordionActivate,
      active: false
    });
    $("body").on("click", ".show-older-answers", function () {
      $(this).addClass("hidden").next().slideDown(500);
    });
  }

  function initBallotCheckBoxes() {
    $("input[type=checkbox].kalypto").kalypto({ toggleClass: "kalypto-checkbox" });
  }

  function initBallotEvents() {
    $(".ballot-checks-container")
      .on("click", ".candidate-cell .clicker", function (event) {
        var $target = $(event.target);
        if ($target.hasClass("clicker")) {
          $target.closest(".candidate-cell").find(".kalypto-checkbox").trigger($.Event("click"));
        }
      })
      .on("click", ".referendum-content .clicker", function (event) {
        var $target = $(event.target);
        if ($target.hasClass("clicker")) {
          $target.closest(".yes,.no").find(".kalypto-checkbox").trigger($.Event("click"));
        }
      })
        .on("rc_checked", "input.candidate-checkbox", function () {
          var $this = $(this);
          $this.closest(".write-in-cell").addClass("checked");
          var $officeCell = $this.closest(".office-cell");
          var $checkedCandidates = $officeCell.find("input.candidate-checkbox:checked");
          var positions = $officeCell.data("positions") || 1;
          if (positions == 1) {
            // turn off any already checked
            $checkedCandidates.each(function () {
              if (this != $this[0]) {
                $(this).prop("checked", false);
                $(this).next("a").removeClass("checked");
                $(this).closest(".write-in-cell").removeClass("checked");
              }
            });
          } else if ($checkedCandidates.length > positions) {
            // only allow if max not exceeded
            $this.prop("checked", false);
            $this.next("a").removeClass("checked");
            $this.closest(".write-in-cell").removeClass("checked");
            alert("No more than " + positions + " candidates may be selected.");
          }
          updateBallot.apply(this, arguments);
        })
        .on("rc_unchecked", "input.candidate-checkbox", function () {
          $(this).closest(".write-in-cell").removeClass("checked");
          updateBallot.apply(this, arguments);
        })
        .on("rc_checked", "input.referendum-checkbox", function () {
          var $this = $(this);
          var $yesNo = $this.closest(".yes-no");
          var $checkedBoxes = $yesNo.find("input.referendum-checkbox:checked");
          // turn off any already checked
          $checkedBoxes.each(function () {
            if (this != $this[0]) {
              $(this).prop("checked", false);
              $(this).next("a").removeClass("checked");
            }
          });
          updateReferendums.apply(this, arguments);
        })
        .on("rc_unchecked", "input.referendum-checkbox", function () {
          updateReferendums.apply(this, arguments);
        })
        .on("propertychange change click keyup input paste", "input.write-in-text", updateBallot);
    $(window).on("storage", function (event) {
      if (!canUpdateBallot) return;
      var split = event.originalEvent.key.split(".");
      if (split.length == 2) {
        var $officeCell = $(".office-cell[data-key=" + split[1] + "]");
        var officeElection = getOfficeElection($officeCell);
        if (split[0] === officeElection) {
          if ($officeCell.length === 1) updateOfficeChecks($officeCell);
        }
      }
    });
  }

  function initBannerAd(adType, stateCode, electionKey, officeKey) {
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/GetBannerAd",
      data: "{'adType':'" + adType + "','stateCode':'" + (stateCode || "") +
      "','electionKey':'" + (electionKey || "") + "','officeKey':'" +
      (officeKey || "") + "','adKey':'" + $.queryString("ad") + "'}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (result) {
        if (result.d) {
          setTimeout(function () { $(".header-inner").before('<div id="ads">' + result.d + '</div>'); });
        }
      }
    });
  }

  function getOfficeElection($officeCell) {
    return $officeCell.data("election") || $("body").data("election");
  }

  function replaceFakeVideoPlayer() {
    var $this = $(this);
    var $videoPlayer = $this.parent();
    // first restore any existing activated players
    $(".video-player iframe").each(function () {
      var $player = $(this).parent();
      $(this).replaceWith($(this).parent().data("html"));
      $(">div", $player).click(replaceFakeVideoPlayer);
    });
    var $iframe = $("<iframe></iframe>",
      {
        src: "//www.youtube.com/embed/" + this.parentNode.dataset.id +
          "?rel=0&autoplay=1&autohide=1&border=0&wmode=opaque&enablejsapi=1&controls=2&showinfo=0",
        frameborder: "0",
        allowfullscreen: "allowfullscreen",
        "class": "video-iframe youtube-iframe"
      });
    // replace the <div> html and save the old html on the .video-player element to be able to restore it
    // so only one player can be active at a time
    $videoPlayer.data("html", "<div>" + $this.replaceWith($iframe).html() + "</div>");
  }

  function initFakeVideoPlayer($panel) {
    $(".video-player", $panel).each(function () {
      var $div = $("<div></div>");
      var $this = $(this);
      var type = $this.data("type");
      var id = $this.data("id");
      var thumbSource = type == "yt"
        ? "//i.ytimg.com/vi/" + id + "/hqdefault.jpg"
        : "https://graph.facebook.com/" + id + "/picture";
      $div.html('<img class="video-thumb" src="' + thumbSource + '">' +
        '<div class="video-play-button ' + type + '-play-button"></div>');
      if (type == "yt")
        $div.click(replaceFakeVideoPlayer);
      else if (type == "fb")
        $div.click(function () {
          var $iframe = $("<iframe></iframe>",
            {
              //src: "https://www.facebook.com/video/embed?video_id=" +
              //  this.parentNode.dataset.id + "?autoplay=1&allowfullscreen=true",// +
              ////"?rel=0&autoplay=1&autohide=1&border=0&wmode=opaque&enablejsapi=1&controls=2&showinfo=0",
              src: 'https://www.facebook.com/plugins/video.php?href=https%3A%2F%2Fwww.facebook.com%2Fvideo.php?v=' +
                this.parentNode.dataset.id + "&autoplay=1&mute=0&height=236&width=420",
              frameborder: "0",
              allowfullscreen: "allowfullscreen",
              "class": "video-iframe facebookvideo-iframe"
            });
          $(this).replaceWith($iframe);
        });
      $this.append($div);
    });
  }

  function loadMyBallotChoices() {
    if (localStorageIsAvailable()) {
      $(".office-cell").each(function () {
        updateOfficeChecks($(this));
      });
      $(".referendum-content").each(function() {
        updateBallotMeasureChecks($(this), false);
      });
    }
  }

  function localStorageIsAvailable() {
    try {
      var storage = window.localStorage,
			x = '__storage_test__';
      storage.setItem(x, x);
      storage.removeItem(x);
      return true;
    }
    catch (e) {
      return false;
    }
  }

  function updateBallot() {
    if (canUpdateBallot && localStorageIsAvailable()) {
      var checks = {};
      var $office = $(this).closest(".office-cell");
      $("a.candidate-checkbox.checked", $office).each(function () {
        var $candidate = $(this).closest(".candidate-cell");
        if ($candidate.hasClass("write-in-cell")) {
          checks.writeIn = $(".write-in-text", $candidate).val();
        } else {
          checks[$candidate.data("key")] = 1;
        }
      });
      var key = getOfficeElection($office) + "." + $office.data("key");
      if ($.isEmptyObject(checks)) window.localStorage.removeItem(key);
      else window.localStorage.setItem(key, JSON.stringify(checks));
    }
  }

  function updateReferendums() {
    if (canUpdateBallot && localStorageIsAvailable()) {
      var val = null;
      var $yesNo = $(this).closest(".yes-no");
      var refkey = $yesNo.closest(".referendum-content").data("key");
      var $checked = $("a.referendum-checkbox.checked", $yesNo);
      if ($checked.length)
        val = $checked.next().text() === "Yes" ? 1 : 0;
      var key = $("body").data("election") + "." + refkey;
      if (val === null) window.localStorage.removeItem(key);
      else window.localStorage.setItem(key, val);
    }
  }

  $(function () {
    //return; // to simulate no-js

    var isIe11 = Object.hasOwnProperty.call(window, "ActiveXObject") && !window.ActiveXObject;
    if (isIe11 || Function('/*@cc_on return document.documentMode===10@*/')())
      document.documentElement.className += ' ie10';
    if (isIe11) document.documentElement.className += ' ie11';

    $("body").removeClass("no-js").addClass("js")
      .on("click", "span.more-text", function () {
        var $span = $(this);
        var key = $span.removeClass("more-text").addClass("more-ajax").data("key");
        $.ajax({
          type: "POST",
          url: "/WebService.asmx/GetMoreText",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          data: '{"key":"' + key + '"}',
          success: function (result) {
            var $parent = $span.parent();
            $span.remove();
            var inx = result.d.indexOf("</p>");
            if (inx === -1)
              $parent.append(result.d);
            else {
              $parent.append(result.d.substr(0, inx));
              $parent.after(result.d.substr(inx + 4) + "</p>");
            }
          },
          error: function () {
            $span.removeClass("more-ajax").addClass("more-text");
          }
        });
      })
      .on("click", ".answer-video .video-icon", function (event) {
        var $answer = $(this).closest(".answer-video");
        var $video = $(".video-wrapper,.video-container", $answer);
        if ($video.length) {
          event.preventDefault();
          $video.show(0);
          // we auto-click these hidden videos so they start right up
          // or, on mobile require only 1 more click
          $(".video-thumb", $video).parent().click();
        }
      })
      .on("accordionbeforeactivate", ".ui-accordion", function (event, ui) {
        if (ui.newHeader.hasClass("loading")) return false;
        if (ui.newHeader.length && !ui.newHeader.hasClass("loaded")) {
          // expanding an ajax panel
          var ajaxData = ui.newPanel.data("ajax");
          if (ajaxData) {
            var $accordion = $(this);
            var index = ui.newHeader.index() / 2;
            ui.newHeader.addClass("loading");
            $.ajax({
              type: "GET",
              url: "/AjaxContent.aspx?" + ajaxData,
              dataType: "html",
              success: function (result) {
                ui.newPanel.html(result);
                initFakeVideoPlayer(ui.newPanel);
                ui.newHeader.addClass("loaded");
                setTimeout(function () {
                  ui.newHeader.removeClass("loading");
                  $accordion.accordion("option", "active", index);
                }, 200);
                ui.newPanel.data("ajax", null);
              },
              error: function () {
                ui.newHeader.removeClass("loading");
              }
            });
            return false;
          }
        }
      });

    //$(".donate-button").menu({ position: { at: "left bottom+7" } });
    //$(".donate-link").menu({ position: { my: "left bottom", at: "left top-3" } });

    $(".address-entry").on("click", ".need-address", function () {
      event.preventDefault();
      if ($(this).hasClass("disabled")) {
        ADDRESSENTRY.alert("Please enter your street address first");
        return;
      }
    }).on("click", ".no-contests", function () {
      event.preventDefault();
      ADDRESSENTRY.alert("There are no office contests or ballot measures for your legislative districts");
      return;
      });

    // close sample ballot menu if click outside
    $(document).on("click", function (event) {
      if ($sampleBallotMenu && !$.contains($(".sample-ballot-link")[0], event.target)) {
        $sampleBallotMenu.hide();
      }
    });
  });

  function setupBackToBallot() {
    if (localStorageIsAvailable()) {
      var url = window.localStorage.getItem("ballotUrl");
      if (url) {
        $("body")
          .addClass("has-back-to-ballot")
          .append('<div class="back-to-ballot no-print"><a href="' + url + '" class="button-4 button-smaller no-print">Back to Your Ballot Choices</a></div>');
      }
    }
  }

  function isIframed() {
    try {
      return window.self !== window.top;
    }
    catch (e) {
      return true;
    }
  }

  return {
    accordionActivate: accordionActivate,
    getAllBallotChecks: getAllBallotChecks,
    initAds: initAds,
    initAnswerAccordions: initAnswerAccordions,
    initBallotCheckBoxes: initBallotCheckBoxes,
    initBallotEvents: initBallotEvents,
    initBannerAd: initBannerAd,
    isIframed: isIframed,
    loadMyBallotChoices: loadMyBallotChoices,
    localStorageIsAvailable: localStorageIsAvailable,
    setCanUpdateBallot: setCanUpdateBallot,
    setupBackToBallot: setupBackToBallot,
    updateBallotMeasureChecks: updateBallotMeasureChecks,
    updateOfficeChecks: updateOfficeChecks
  };

})(jQuery);
