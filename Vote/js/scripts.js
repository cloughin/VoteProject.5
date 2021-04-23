jQuery.cookie = function(name, value, options) {
  if (typeof value != 'undefined') {
    options = options || {};
    if (value === null) {
      value = '';
      options.expires = -1;
    }
    var expires = '';
    if (options.expires &&
      (typeof options.expires == 'number' || options.expires.toUTCString)) {
      var date;
      if (typeof options.expires == 'number') {
        date = new Date();
        date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
      } else {
        date = options.expires;
      }
      expires = '; expires=' + date.toUTCString();
    }
    var path = options.path ? '; path=' + (options.path) : '';
    var domain = options.domain ? '; domain=' + (options.domain) : '';
    var secure = options.secure ? '; secure' : '';
    document.cookie = [name, '=', options.noEncode ? value : encodeURIComponent(value), expires, path, domain, secure]
      .join('');
  } else {
    var cookieValue = null;
    if (document.cookie && document.cookie != '') {
      var cookies = document.cookie.split(';');
      for (var i = 0; i < cookies.length; i++) {
        var cookie = jQuery.trim(cookies[i]);
        if (cookie.substring(0, name.length + 1) == (name + '=')) {
          cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
          break;
        }
      }
    }
    return cookieValue;
  }
};
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

$(function() {

  var $alertDialog;
  var $alertDialogButtons;
  var alertDialogCallback = null;
  var alertDialogInitialized = false;
  var forEnrollment = false;
  var forEnrollmentEmail = null;

  var htmlEscape = function(str) {
    return String(str)
      .replace(/&/g, '&amp;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/\u2018/g, "&lsquo;")
      .replace(/\u2019/g, "&rsquo;")
      .replace(/\u201C/g, "&ldquo;")
      .replace(/\u201D/g, "&rdquo;")
      .replace(/\u00AE/g, "&reg;")
      .replace(/\u2122/g, "&trade;")
      .replace(/\u2013/g, "&ndash;")
      .replace(/\u2014/g, "&mdash;");
  };

  var initAlertDialog = function() {
    if (alertDialogInitialized) return;
    $('body').append('<div id="alert-dialog" class="hidden"><div class="inner">' +
      '<div class="message"></div><div class="bottom-box button-box">' +
      '<input type="button" class="alert-button-1 button-1 button-smaller"/>' +
      '<input type="button" class="alert-button-2 button-1 button-smaller"/>' +
      '<input type="button" class="alert-button-3 button-1 button-smaller"/>' +
      '<input type="button" class="alert-button-4 button-1 button-smaller"/>' +
      '</div></div></div>');
    $alertDialog = $('#alert-dialog').dialog({
      autoOpen: false,
      close: onAlertDialogClose,
      dialogClass: "alert-dialog",
      modal: true,
      resizable: false,
      width: "auto",
      minHeight: 0
    });
    $alertDialogButtons = $('input[type=button]', $alertDialog)
      .on("click", function(event) {
        var fn = null;
        if (alertDialogCallback) {
          fn = alertDialogCallback;
          alertDialogCallback = null;
        }
        $alertDialog.dialog("close");
        // ReSharper disable once InvokedExpressionMaybeNonFunction
        if (fn) fn($(event.target).val());
      });
    alertDialogInitialized = true;
  };

  var alert = function() {
    initAlertDialog();
    var ax = 0;
    var message = "Message";
    var title = "Alert";
    var callback = null;
    var buttons = ["Ok"];
    if (arguments.length > 0 && typeof arguments[0] === "string") {
      message = arguments[ax++];
    }
    for (var i = ax; i < arguments.length; i++) {
      switch (typeof arguments[i]) {
      case "string":
        title = arguments[i];
        break;

      case "function":
        callback = arguments[i];
        break;

      default:
        if ($.isArray(arguments[i]))
          buttons = arguments[i];
        break;
      }
    }
    i = 0;
    while (i < buttons.length) {
      var buttonTypeInx = i + 1;
      if (buttonTypeInx === buttons.length ||
        (typeof buttons[buttonTypeInx] !== "number")) {
        buttons.splice(buttonTypeInx, 0, 1);
      }
      i += 2;
    }
    $alertDialog.dialog("option", "title", htmlEscape(title));
    $('.message', $alertDialog).html(htmlEscape(message).replace(/\n/g, "<br />"));
    alertDialogCallback = callback;
    $alertDialogButtons.hide().removeClass("button-1 button-2 button-3");
    for (i = 0; i < buttons.length; i += 2) {
      var j = i / 2;
      $($alertDialogButtons[j])
        .show()
        .val(buttons[i])
        .addClass("button-" + buttons[i + 1]);
    }
    $alertDialog.dialog("open");
    //moveDialogToTop("alert-dialog");
  };

  var onAlertDialogClose = function() {
    if (alertDialogCallback) {
      var fn = alertDialogCallback;
      alertDialogCallback = null;
      fn("esc");
    }
  };

  function setUpLinks() {
    var state = $.cookie('State');
    var congress = $.cookie('Congress');
    var stateSenate = $.cookie('StateSenate');
    var stateHouse = $.cookie('StateHouse');
    var county = $.cookie('County');
    var district = $.cookie('District');
    var place = $.cookie('Place');
    var elementary = $.cookie('Elementary');
    var secondary = $.cookie('Secondary');
    var unified = $.cookie('Unified');
    var cityCouncil = $.cookie('CityCouncil');
    var countySupervisors = $.cookie('CountySupervisors');
    var schoolDistrictDistrict = $.cookie('SchoolDistrictDistrict');

    if (state &&
      congress &&
      stateSenate &&
      (stateHouse || state == "DC" || state == "NE") &&
      county &&
      district != null &&
      place != null) {
      // elected officials
      var q = [];
      q.push("State=" + state);
      q.push("Congress=" + congress);
      q.push("StateSenate=" + stateSenate);
      if (stateHouse)
        q.push("StateHouse=" + stateHouse);
      q.push("County=" + county);
      q.push("District=" + district);
      q.push("Place=" + place);
      if (elementary)
        q.push("Esd=" + elementary);
      if (secondary)
        q.push("Ssd=" + secondary);
      if (unified)
        q.push("Usd=" + unified);
      if (cityCouncil)
        q.push("Cc=" + cityCouncil);
      if (countySupervisors)
        q.push("Cs=" + countySupervisors);
      if (schoolDistrictDistrict)
        q.push("Sdd=" + schoolDistrictDistrict);
      $(".active-button-block .elected-officials")
        .attr("href", "/Elected.aspx?" + q.join("&"));

      // elections
      var data = {
        stateCode: state,
        congress: congress,
        stateSenate: stateSenate,
        stateHouse: stateHouse,
        county: county,
        district: district,
        place: place,
        elementary: elementary || "",
        secondary: secondary || "",
        unified: unified || "",
        cityCouncil: cityCouncil || "",
        countySupervisors: countySupervisors || "",
        schoolDistrictDistrict: schoolDistrictDistrict || ""
      };
      $.ajax({
        type: "POST",
        url: "/WebService.asmx/GetUpcomingElections",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(result) {
          var upcomingElections = result.d.Elections;
          switch (upcomingElections.length) {
          case 0:
            alert("Could not determine elections");
            break;

            default:
              // build links
              var items = [];
              $.each(upcomingElections, function () {
                var extraClasses = this.HasContests ? "" : " disabled no-contests";
                var message = this.HasContests ? "" : '<p class="no-contests-message">There are no office contests or ballot measures for your legislative districts.</p>';
                items.push('<li><a class="big-orange-button bob-with-arrow' + extraClasses +
                  '" href="' + this.Href + '">' + this.Description + '</a>' + message + '</li>');
              });
              $(".active-button-block ul").html(items.join(""));
              var $notAvailable = $(".active-button-block .not-available");
              if (result.d.IsPast) {
                $notAvailable.text("Upcoming election information is not yet available. Here is your most recent election:");
              } else {
                $notAvailable.addClass("hidden");
              }
              $(".address-entry .active-button-block").removeClass("hidden");
            $(".address-entry .default-button-block").addClass("hidden");
            break;
          }
        },
        error: function getUpcomingElectionsFailed(result) {
          alert(result.status + ' ' + result.statusText);
        }
      });
    } else
      alert("Could not determine elections");
  }

  function setForEnrollment() {
    forEnrollment = true;
    $(".default-button-block").addClass("hidden");
    $(".active-button-block").addClass("hidden");
    $(".enrollment-button-block").removeClass("hidden");
  }

  function setForEnrollmentEmail(email) {
    forEnrollmentEmail = email;
  }

  function haveAddress(address) {
    if (address) {
      $address.val(address).prop("disabled", true);
      if (!forEnrollment) setUpLinks();
    } else {
      $address.val(address).prop("disabled", false);
      $(".active-button-block").addClass("hidden");
      $(".enrollment-button-block").addClass("hidden");
      $(".default-button-block").toggleClass("hidden", forEnrollment);
    }
  }

  var $address = $(".address-entry .address-search");
  if ($.cookie('Address') &&
    $.cookie("State") &&
    $.cookie("County") &&
    $.cookie("Congress") &&
    $.cookie("StateSenate") &&
    $.cookie("District") != null &&
    $.cookie("Place") != null &&
    $.cookie("Geo")) {
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/IsCookieTrusted",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(response) {
        if (response.d) {
          haveAddress($.cookie('Address'));
        } else {
          haveAddress("");
        }
      }
    });
  } else {
    haveAddress("");

  }

  $(".address-entry .use-this-address").click(function() {
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/UseThisAddress",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      data: JSON.stringify({
        forEnrollment: !!forEnrollment,
        forEnrollmentEmail: forEnrollmentEmail || ""
      }),
      success: function(response) {
        var result = response.d;
        if (result)
          document.location.href = result;
      },
      error: function() { $message.html(response.status + ' ' + response.statusText); }
    });
  });

// ReSharper disable once UseOfImplicitGlobalInFunctionScope
  if (typeof google === "undefined") return;
  var $message = $(".address-entry .error-message");
  $message.text("");
  $(".address-entry .enter-new-address")
    .click(function () {
      haveAddress("");
      event.preventDefault();    });
  $(".address-entry .use-address")
    .click(function () {
      alert("Vote-USA uses your address only to provide you with customized electoral" +
        " information, including your ballot choices, elected representatives, and various" +
        " election reports. We do not sell, trade, or otherwise transfer any of this" +
        " information to any other person, organization or third party. We cookie your" +
        " address for the sole purpose of saving you the trouble of reentering it when" +
        " next you visit this site.");
      event.preventDefault();
    });
  // ReSharper disable once UseOfImplicitGlobalInFunctionScope
  var autocomplete = new google.maps.places.Autocomplete($address[0], {
    types: ['geocode'],
    componentRestrictions: { country: 'us' }
  });
  $address.on("focus", function() {
      if ($(window).height() <= 432 || $(window).width() <= 432)
        $('html, body').animate({
          scrollTop: $address.offset().top - 10
        }, 500);
    })
    .keypress(function(event) {
      if (event.keyCode === 10 || event.keyCode === 13)
        event.preventDefault();
    });
  autocomplete.addListener('place_changed', function() {

    function warning() {
      alert(
        "There is not enough information to reliably determine your voting districts. Please include a street address along with a zip code or a city and state.");
    }

    var componentForm = {
      street_number: 'short_name',
      route: 'long_name',
      locality: 'long_name',
      sublocality: 'long_name',
      sublocality_level_1: 'long_name',
      administrative_area_level_1: 'short_name',
      postal_code: 'short_name',
      postal_code_suffix: 'short_name'
    };
    var place = autocomplete.getPlace();
    if (!place ||
      !place.address_components ||
      !place.formatted_address ||
      !place.geometry ||
      !place.geometry.location) {
      warning();
      return;
    }
    var address = {};
    for (var i = 0; i < place.address_components.length; i++) {
      var addressType = place.address_components[i].types[0];
      if (componentForm[addressType]) {
        var val = place.address_components[i][componentForm[addressType]];
        address[addressType] = val;
      }
    }
    address.formatted_address = place.formatted_address;
    address.lat = place.geometry.location.lat();
    address.lng = place.geometry.location.lng();
    address.street_address = $.trim((address.street_number || "") +
      " " +
      (address.route || ""));
    address.city = address.locality ||
      address.sublocality ||
      address.sublocality_level_1 ||
      "";
    address.state = address.administrative_area_level_1;
    address.zip5 = address.postal_code || "";
    address.zip4 = address.postal_code_suffix || "";
    var components = [
      address.street_address, address.city, address.state, address.zip5, address.zip4
    ];
    for (var j = 0; j < components.length; j++)
      components[j] = encodeURIComponent(components[j]);
    address.components = components.join("|");
    if (!address.street_address) {
      warning();
      return;
    }
    var data =
    {
      formattedAddress: address.formatted_address,
      components: address.components,
      stateCode: address.administrative_area_level_1,
      latitude: address.lat,
      longitude: address.lng,
      forEnrollment: !!forEnrollment,
      forEnrollmentEmail: forEnrollmentEmail || ""
    };
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/FindLocation",
      contentType: "application/json; charset=utf-8",
      data: JSON.stringify(data),
      dataType: "json",
      success: function(response) {
        var result = response.d;
        if (!result.ErrorMessage) {
          if (result.DomainCode === "US") {
            var expires = 30; // days
            $.cookie('Address', result.FormattedAddress, {
              expires: expires
            });
            $.cookie('Components', address.components, {
              expires: expires,
              noEncode: true
            });
            $.cookie('State', result.StateCode, {
              expires: expires
            });
            $.cookie('Congress', result.Congress, {
              expires: expires
            });
            $.cookie('County', result.County, {
              expires: expires
            });
            $.cookie('StateSenate', result.StateSenate, {
              expires: expires
            });
            $.cookie('StateHouse', result.StateHouse, {
              expires: expires
            });
            $.cookie('District', result.District, {
              expires: expires
            });
            $.cookie('Place', result.Place, {
              expires: expires
            });
            $.cookie('Elementary', result.Elementary, {
              expires: expires
            });
            $.cookie('Secondary', result.Secondary, {
              expires: expires
            });
            $.cookie('Unified', result.Unified, {
              expires: expires
            });
            $.cookie('CityCouncil', result.CityCouncil, {
              expires: expires
            });
            $.cookie('CountySupervisors', result.CountySupervisors, {
              expires: expires
            });
            $.cookie('SchoolDistrictDistrict', result.SchoolDistrictDistrict, {
              expires: expires
            });
            $.cookie('Geo', address.lat.toFixed(6) + "," + address.lng.toFixed(6), {
              expires: expires,
              noEncode: true
            });

            var d = new Date();
            d.setDate(d.getDate() + expires);
            $.cookie('CDate',
              d.getUTCMonth() +
              1 +
              "/" +
              d.getUTCDate() +
              "/" +
              d.getUTCFullYear() +
              " " +
              d.getUTCHours() +
              ":" +
              d.getUTCMinutes() +
              ":" +
              d.getUTCSeconds(), {
                expires: expires
              });
            $("body").data("c", "True");
          }
          if (result.Success) {
            if (forEnrollment)
              document.location.href = result.RedirectUrl;
            else
              haveAddress($.cookie("Address"));
          }
        } else {
          $message.html(result.ErrorMessage);
        }
      },
      error: function(response) {
        $message.html(response.status + ' ' + response.statusText);
      }
    });
  });
  window.ADDRESSENTRY = {
    alert: alert,
    setForEnrollment: setForEnrollment,
    setForEnrollmentEmail: setForEnrollmentEmail
  }
});
;window.UTIL=(function($) {

  // for parsing the query string (static)
  $.extend({
    queryString: function (name) {
      if (name) {
        return $.queryString()[name.toLowerCase()];
      } else {
        var vars = [], hash;
        // remove fragment
        var href = window.location.href.split('#')[0];
        var hashes = href.slice(href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
          hash = hashes[i].split('=');
          hash[0] = hash[0].toLocaleLowerCase();
          vars.push(hash[0]);
          vars[hash[0]] = hash[1];
        }
        return vars;
      }
    }
  });

  // Alerts - AlertDialog

  // A replacement for the plain js alert and confirm
  //
  // arguments:
  // message: must be the first, type string
  // dialog title: a subsequent string argument is used as the dialog title
  // buttons: an array argument
  // callback: a function argument, called when dialog closed, 
  //   passed the label of the clicked button or "esc" if not closed
  //   via button click
  // the buttons argument: array of string giveng the button labels -- each
  //   label optionally followed by a number used to form a classname of
  //   "button-<number>" (default 1).
  //   example: ["Ok", "Cancel", 3] makes two buttons:
  //     "Ok" classname button-1
  //     "Cancel" classname button-3

  var $alertDialog;
  var $alertDialogButtons;
  var alertDialogCallback = null;
  var alertDialogInitialized = false;

  var initAlertDialog = function() {
    if (alertDialogInitialized) return;
    $('body').append('<div id="alert-dialog" class="hidden"><div class="inner">' +
      '<div class="message"></div><div class="bottom-box button-box">' +
      '<input type="button" class="alert-button-1 button-1 button-smallest"/>' +
      '<input type="button" class="alert-button-2 button-1 button-smallest"/>' +
      '<input type="button" class="alert-button-3 button-1 button-smallest"/>' +
      '<input type="button" class="alert-button-4 button-1 button-smallest"/>' +
      '</div></div></div>');
    $alertDialog = $('#alert-dialog').dialog({
      autoOpen: false,
      close: onAlertDialogClose,
      dialogClass: "alert-dialog",
      modal: true,
      resizable: false,
      width: "auto"
    });
    $alertDialogButtons = $('input[type=button]', $alertDialog)
      .on("click", function(event) {
        var fn = null;
        if (alertDialogCallback) {
          fn = alertDialogCallback;
          alertDialogCallback = null;
        }
        $alertDialog.dialog("close");
        // ReSharper disable once InvokedExpressionMaybeNonFunction
        if (fn) fn($(event.target).val());
      });
    alertDialogInitialized = true;
  };

  var alert = function() {
    initAlertDialog();
    var ax = 0;
    var message = "Message";
    var title = "Alert";
    var callback = null;
    var buttons = ["Ok"];
    if (arguments.length > 0 && typeof arguments[0] === "string") {
      message = arguments[ax++];
    }
    for (var i = ax; i < arguments.length; i++) {
      switch (typeof arguments[i]) {
      case "string":
        title = arguments[i];
        break;

      case "function":
        callback = arguments[i];
        break;

      default:
        if ($.isArray(arguments[i]))
          buttons = arguments[i];
        break;
      }
    }
    i = 0;
    while (i < buttons.length) {
      var buttonTypeInx = i + 1;
      if (buttonTypeInx === buttons.length ||
        (typeof buttons[buttonTypeInx] !== "number")) {
        buttons.splice(buttonTypeInx, 0, 1);
      }
      i += 2;
    }
    $alertDialog.dialog("option", "title", htmlEscape(title));
    $('.message', $alertDialog).html(htmlEscape(message).replace(/\n/g, "<br />"));
    alertDialogCallback = callback;
    $alertDialogButtons.hide().removeClass("button-1 button-2 button-3");
    for (i = 0; i < buttons.length; i += 2) {
      var j = i / 2;
      $($alertDialogButtons[j])
        .show()
        .val(buttons[i])
        .addClass("button-" + buttons[i + 1]);
    }
    $alertDialog.dialog("open");
    //moveDialogToTop("alert-dialog");
  };

  var confirm = function() {
    var args = [].slice.call(arguments);
    args.push(["Ok", "Cancel", 3]);
    var strings = 0;
    $.each(args, function(inx) {
      if (typeof args[inx] === "string") strings++;
    });
    if (strings < 2) args.push("Confirm");
    alert.apply(this, args);
  };

  var onAlertDialogClose = function() {
    if (alertDialogCallback) {
      var fn = alertDialogCallback;
      alertDialogCallback = null;
      fn("esc");
    }
  };

  // P U B L I C

  function htmlEscape(str) {
    return String(str)
      .replace(/&/g, '&amp;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/\u2018/g, "&lsquo;")
      .replace(/\u2019/g, "&rsquo;")
      .replace(/\u201C/g, "&ldquo;")
      .replace(/\u201D/g, "&rdquo;")
      .replace(/\u00AE/g, "&reg;")
      .replace(/\u2122/g, "&trade;")
      .replace(/\u2013/g, "&ndash;")
      .replace(/\u2014/g, "&mdash;");
  };

  var modalInitialized = false;
  var $modalOverlay;
  var $modalContent;
  
  function centerModal() {
    var top = Math.max($(window).height() - $modalContent.outerHeight(), 0) / 2;
    var left = Math.max($(window).width() - $modalContent.outerWidth(), 0) / 2;

    $modalContent.css({
      top: top + $(window).scrollTop(),
      left: left + $(window).scrollLeft()
    });
  }
  
  function initModal() {
    if (!modalInitialized) {
      $modalOverlay = $('<div id="ajax-overlay"></div>');
      $modalContent = $('<div id="ajax-overlay-content"><img src="/images/ajax-loader64.gif"/></div>');
      $modalOverlay.hide();
      $modalContent.hide();
      $("body").append($modalOverlay, $modalContent);
    }
  }
  
  function openModal() {
    initModal();

    $modalContent.css({
      width: 'auto',
      height: 'auto'
    });

    centerModal();

    $(window).bind('resize.modal', centerModal);

    $modalContent.show();
    $modalOverlay.show();
  }
  
  function closeModal() {
    $modalContent.hide();
    $modalOverlay.hide();
    $(window).unbind('resize.modal');
  }


  var isValidEmailRegex = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
  function isValidEmail(email) {
    if (typeof email !== "string") return false;
    return !!email.match(isValidEmailRegex);
  };

  // I N I T I A L I Z E

  return {
    alert: alert,
    closeModal: closeModal,
    confirm: confirm,
    htmlEscape: htmlEscape,
    isValidEmail: isValidEmail,
    openModal: openModal
  };

})(jQuery);
function initDonationRequestDialog() {
  $('#donationRequestDialog')
    .dialog({
      autoOpen: false,
      width: "auto",
      height: "auto",
      resizable: false,
      open: onOpenJqDialog,
      close: onCloseJqDialog,
      create: function() { $(this).css("maxWidth", "500px"); },
      modal: true,
      dialogClass: "donation-request-dialog"
    });
  $('#donationRequestDialog .yes a').click(onClickYes);
  $('#donationRequestDialog .no a').click(onClickNo);
  $('#donationRequestDialog .already a').click(onClickAlready);
}

function disableCookie() { $.cookie('dnx', '-1', { expires: 365 }); }

function onClickYes() {
  disableCookie();
  $('#donationRequestDialog').dialog('close');
  return true;
}

function onClickNo() { $('#donationRequestDialog').dialog('close'); }

function onClickAlready() {
  disableCookie();
  $('#donationRequestDialog').dialog('close');
}

function openDonationRequestDialog() {
  if (!PUBLIC.isIframed())
    $('#donationRequestDialog').dialog('open'); 
}  

function getDonationNag() {
  var dnx = parseInt($.cookie('dnx'));
  if (isNaN(dnx))
    dnx = 1;
  if (dnx >= 0) {
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/GetDonationNag",
      data: "{" + "'cookieIndex': " + dnx + "}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: getDonationNagSucceeded
    });
  }
}

function getDonationNagSucceeded(result) {
  var donationNag = result.d;
  $.cookie('dnx', donationNag.NextMessageNumber, { expires: 365 });
  if (donationNag.MessageText) {
    $('#donationRequestDialog .message-text').html(donationNag.MessageText);
    if (donationNag.MessageHeading)
      $('#donationRequestDialog .heading-text').html(donationNag.MessageHeading);
    initDonationRequestDialog();
    setTimeout(openDonationRequestDialog, 2000);
  }
}

$(function () { if ($("#donationRequestDialog").length) getDonationNag(); });
// Initialize when ready
$(function () {

  var $dropdown = $('.email-form-subject');
  
  function onChange() {
    $dropdown.val() === "Other" ? $(".email-form-other-subject").show(200) : $(".email-form-other-subject").hide(200);
  }

  $dropdown.change(onChange);
  onChange();
  var errorLabel = $('.email-form-error-label');
  var goodLabel = $('.email-form-good-label');
  if (errorLabel.length !== 0)
    UTIL.alert(errorLabel.text());
  else if (goodLabel.length !== 0)
    UTIL.alert(goodLabel.text());
});

function initSampleBallotEmailDialog() {
  $('#sampleBallotEmailDialog').dialog({
    autoOpen: false,
    width: "auto",
    height: "auto",
    resizable: false,
    // custom open and close to fix various problems
    open: onOpenJqDialog,
    close: onCloseJqDialog(),
    create: function() {
      $(this).css("maxWidth", "500px");
    },
    modal: true,
    dialogClass: "sample-ballot-email-dialog"
  });
  $("#sampleBallotEmailDialog .emailEnter").click(
    function() {
      var email = $("#sampleBallotEmailDialog .email").val();
      if (submitSampleBallotEmail(email) !== false)
        $('#sampleBallotEmailDialog').dialog('close');

    });
  $("#sampleBallotEmailDialog .emailNoThanks").click(
    function() {
      $('#sampleBallotEmailDialog').dialog('close');
    });
  $("#sampleBallotEmailDialog .already-signed-up").click(
    function() {
      setEnteredCookie();
      $('#sampleBallotEmailDialog').dialog('close');
    });
  $(document).keypress(function(event) {
    var keycode = event.keyCode ? event.keyCode : event.which;
    if (keycode === 13) {
      var $email = $('#sampleBallotEmailDialog .email');
      if ($(".sample-ballot-email-dialog").css("display") != "none" && $email.is(":focus"))
        $("#sampleBallotEmailDialog .emailEnter").trigger("click");
    }
  });
}

function getParameterByName(name) {
  name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
  var regexS = "[\\?&]" + name + "=([^&#]*)";
  var regex = new RegExp(regexS, "i");
  var results = regex.exec(window.location.search);
  if (results === null)
    return "";
  else
    return decodeURIComponent(results[1].replace(/\+/g, " "));
}

function submitSampleBallotEmail(email) {
  email = $.trim(email);
  if (!UTIL.isValidEmail(email)) {
    UTIL.alert("Please enter a valid email address");
    return false;
  }
  var data = {};
  data.email = email;
  data.siteId = getParameterByName('site');
  data.stateCode = getParameterByName('state');
  data.electionKey = getParameterByName('election');
  data.congressionalDistrict = getParameterByName('congress');
  data.stateSenateDistrict = getParameterByName('stateSenate');
  data.stateHouseDistrict = getParameterByName('stateHouse');
  data.county = getParameterByName('county');
  data.district = getParameterByName('district');
  data.place = getParameterByName('place');
  data.elementary = getParameterByName('esd');
  data.secondary = getParameterByName('ssd');
  data.unified = getParameterByName('usd');
  data.cityCouncil = getParameterByName('cc');
  data.countySupervisors = getParameterByName('cs');
  data.schoolDistrictDistrict = getParameterByName('sdd');
  $.ajax({
    type: "POST",
    url: "/WebService.asmx/SubmitSampleBallotEmail",
    data: JSON.stringify(data),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: setEnteredCookie
  });
}

function setEnteredCookie() {
  $.cookie('sampleBallotEmailEntered', 'true', { expires: 365 });
}

function openSampleBallotEmailDialog() {
  if (PUBLIC.isIframed()) return;
  $('#sampleBallotEmailDialog').dialog('open');
  $('#sampleBallotEmailDialog .email').val("").focus();
}

var pendingSBDDialogTimer;

function showSampleBallotDialog() {
  if (pendingSBDDialogTimer) {
    clearTimeout(pendingSBDDialogTimer);
    pendingSBDDialogTimer = null;
  }
  var shownCount = parseInt($.cookie('sampleBallotEmailShown')) || 0;
  $.cookie('sampleBallotEmailShown', (shownCount + 1) % 5); // every 5th time
  openSampleBallotEmailDialog();
  pendingSBDDialogTimer = null;
}

// Initialize when ready
$(function() {
  initSampleBallotEmailDialog();
  var shownCount = parseInt($.cookie('sampleBallotEmailShown')) || 0;
  if ((typeof window.SampleBallotEmailDialogEnabled === "undefined" ||
      window.SampleBallotEmailDialogEnabled) &&
    shownCount === 0 &&
    $.cookie('sampleBallotEmailEntered') !== 'true') {
    pendingSBDDialogTimer = setTimeout(function() {
      showSampleBallotDialog();
    }, 15000);
  }
});
function onOpenJqDialog(/*event, ui*/) {
  $('body').css('overflow', 'hidden');
  $('.ui-widget-overlay').css('width', '100%');
  $(this).parent().appendTo('form');
  $(this).css('overflow', 'hidden');
}

function onCloseJqDialog(/*event, ui*/) { $('body').css('overflow', 'auto'); }

function initHoverChildren() {
  $(".hoverChild")
    .hover(function(/*event*/) { $(this).parent().addClass("hovering"); },
      function (/*event*/) { $(this).parent().removeClass("hovering"); });
}

$(function() { initHoverChildren(); });