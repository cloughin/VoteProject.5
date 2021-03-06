
var PUBLIC = (function ($) {

  var getUpcomingElectionsCalled;
  var $sampleBallotMenu;
  var canUpdateBallot = true;

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

  function getSampleBallot(event) {
    event.preventDefault();

    if ($sampleBallotMenu) {
      $sampleBallotMenu.show();
      return;
    }

    var $this = $(this);
    var state = $.cookie('State');
    var congress = $.cookie('Congress');
    var stateSenate = $.cookie('StateSenate');
    var stateHouse = $.cookie('StateHouse');
    var county = $.cookie('County');

    if (state && congress && stateSenate && stateHouse && county) {
      if (!getUpcomingElectionsCalled) {
        $.ajax({
          type: "POST",
          url: "/WebService.asmx/GetUpcomingElections",
          data: "{" +
          "'stateCode': '" + state +
          "','congress': '" + congress +
          "','stateSenate': '" + stateSenate +
          "','stateHouse': '" + stateHouse +
          "','county': '" + county +
          "'}",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function (result) {
            var upcomingElections = result.d;
            switch (upcomingElections.length) {
              case 1:
                document.location.href = upcomingElections[0].Href;
                break;

              case 0:
                redirectToVoters();
                break;

              default:
                // build a menu
                var $link = $this.closest(".sample-ballot-link");
                var items = [];
                $.each(upcomingElections, function () {
                  items.push('<li><a href="' + this.Href + '">' + this.Description + '</a></li>');
                });
                $link.append('<ul>' + items.join("") + '</ul>');
                $sampleBallotMenu = $("ul", $link);
                $sampleBallotMenu.menu();
                break;
            }
            //if (upcomingElections.length === 1) {
            //  document.location.href = upcomingElections[0].Href;
            //}
            //else {
            //  redirectToVoters();
            //}
          },
          error: function getUpcomingElectionsFailed(result) {
            alert(result.status + ' ' + result.statusText);
            getUpcomingElectionsCalled = false;
          }
        });
        getUpcomingElectionsCalled = true;
      }
    }
    else
      redirectToVoters();
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
    }
    return result;
  }

  function initBallotCheckBoxes() {
    $("input[type=checkbox].kalypto").kalypto({ toggleClass: "kalypto-checkbox" });
  }

  function initBallotEvents() {
    $(".ballot-checks-container")
        .on("rc_checked", "input.candidate-checkbox", function () {
          var $this = $(this);
          $(this).closest(".write-in-cell").addClass("checked");
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
  
  function getOfficeElection($officeCell) {
    return $officeCell.data("election") || $("body").data("election");
  }

  function initYouTubeFakePlayer() {
    $(".youtube-player").each(function () {
      var $div = $("<div></div>");
      var $this = $(this);
      $div.html('<img class="youtube-thumb" src="//i.ytimg.com/vi/' +
        $this.data("id") + '/hqdefault.jpg"><div class="play-button"></div>');
      $div.click(function () {
        var $iframe = $("<iframe></iframe>",
          {
            src: "//www.youtube.com/embed/" + this.parentNode.dataset.id +
              "?rel=0&autoplay=1&autohide=1&border=0&wmode=opaque&enablejsapi=1&controls=2&showinfo=0",
            frameborder: "0",
            allowfullscreen: "allowfullscreen",
            "class": "youtube-iframe"
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

  function redirectToVoters() {
    document.location.pathname = '/forVoters.aspx';
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
      //var key = $("body").data("election") + "." + $office.data("key");
      var key = getOfficeElection($office) + "." + $office.data("key");
      if ($.isEmptyObject(checks)) window.localStorage.removeItem(key);
      else window.localStorage.setItem(key, JSON.stringify(checks));
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
      .on("click", ".answer-youtube .yt-icon", function (event) {
        var $answer = $(this).closest(".answer-youtube");
        var $video = $(".video-wrapper,.youtube-container", $answer);
        if ($video.length) {
          event.preventDefault();
          $video.show(0);
          // we auto-click these hidden videos so they start right up
          // or, on mobile require only 1 more click
          $(".youtube-thumb", $video).parent().click();
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
                initYouTubeFakePlayer(ui.newPanel);
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

    $(".sample-ballot-link a").click(getSampleBallot);

    initYouTubeFakePlayer();

    $('.main-menu').slicknav({
      prependTo: '.main-navigation',
      label: "Main Menu"
    }).addClass("slicknav-auto-close");

    $('.states-menu ul').slicknav({
      prependTo: '.states-menu',
      label: "Select a state",
      duplicate: false
    }).addClass("slicknav-auto-close");

    // close slicknav menu if click outside
    // also sample ballot menu
    $(document).on("click", function (event) {
      if (!$(event.target).closest(".slicknav_menu li,.slicknav_btn").length) {
        $(".slicknav-auto-close").slicknav("close");
      }
      if ($sampleBallotMenu && !$.contains($(".sample-ballot-link")[0], event.target)) {
        $sampleBallotMenu.hide();
      }
    });

    $(".main-banner .states-menu .slicknav_menutxt").text($(".main-banner .base-menu .slicknav_menutxt").text());
  });
  
  function setupBackToBallot() {
    if (localStorageIsAvailable()) {
      var url = window.localStorage.getItem("ballotUrl");
      if (url) {
        $("body")
          .addClass("has-back-to-ballot")
          .append('<div class="back-to-ballot"><a href="' + url + '" class="button-4 button-smaller">Back to My Sample Ballot</a></div>');
      }
    }
  }

  return {
    accordionActivate: accordionActivate,
    getAllBallotChecks: getAllBallotChecks,
    getSampleBallot: getSampleBallot,
    initBallotCheckBoxes: initBallotCheckBoxes,
    initBallotEvents: initBallotEvents,
    initYouTubeFakePlayer: initYouTubeFakePlayer,
    loadMyBallotChoices: loadMyBallotChoices,
    localStorageIsAvailable: localStorageIsAvailable,
    setCanUpdateBallot: setCanUpdateBallot,
    setupBackToBallot: setupBackToBallot,
    updateOfficeChecks: updateOfficeChecks
  };

})(jQuery);
