define(["jquery", "vote/adminMaster", "vote/util", "jqueryui"],
  function ($, master, util) {

    var pendingAjaxCompletions;

    var initPage = function () {
      initReport();
      // event handlers for user entry area
      var $entry = $("div.entry");
      $(".lookup-address", $entry).safeBind("click", onClickFindAddress);
      $(".state-list", $entry).safeBind("change", onChangeStateList);
      $(".county-list", $entry).safeBind("change", setRedArrow);
      $(".congress-list", $entry).safeBind("change", setRedArrow);
      $(".state-senate-list", $entry).safeBind("change", setRedArrow);
      $(".state-house-list", $entry).safeBind("change", setRedArrow);
      $(".election-list", $entry).safeBind("change", setRedArrow);
      $(".get-explorer", $entry).safeBind("click", onClickGetExplorer);
      $(".report-placeholder").on("mousedown", ".candidates", onMouseDownCandidates);
      $(".report-placeholder").on("click", ".select-candidates", onSelectCandidates);
      $("#select-candidates-dialog .ok-button").click(onClickSelectCandidatesOk);
      $('#select-candidates-dialog').on("click", 'input[type="checkbox"]', onClickSelectCandidatesCheckbox);

      $('#select-candidates-dialog').dialog({
        autoOpen: false,
        //width: 500,
        minHeight: 0,
        height: "auto",
        resizable: false,
        dialogClass: 'select-candidates-dialog overlay-dialog',
        title: "Select Candidates to Compare",
        modal: true
      });

      $(document).keypress(function(event) {
        if (event.which === 13) {
          if ($(".entry .address-entry").is(":focus"))
            $(".entry .lookup-address").click();
          event.preventDefault();
        }
      });

      var code = ($.queryString("state") || "").toUpperCase();
      if (code) {
        $(".state-list").val(code);
        if ($(".state-list").val() === code) 
          onChangeStateList(true);
      }
    };

    var ajaxCompleted = function() {
      if (--pendingAjaxCompletions <= 0)
        util.closeAjaxDialog();
    };

    function buildIssueListForOffice($tabPanel) {
      var issueList = [];
      $(".issue-group", $tabPanel).each(function () {
        var $this = $(this);
        var c = getIssueClasses($this);
        if (issueList.length === 0 ||
          issueList[issueList.length - 1].issueClass !== c.issueClass)
          issueList.push({ issueClass: c.issueClass, issue: $this.attr("issue") });
      });
      return issueList;
    }

    function buildQuestionListForIssueInOffice($tabPanel) {
      var currentIssueClass = $(".issue-text", $tabPanel).attr("key");
      var questionList = [];
      $(".issue-group." + currentIssueClass, $tabPanel).each(function () {
        var $this = $(this);
        var c = getIssueClasses($this);
        questionList.push({ questionClass: c.questionClass, question: $this.attr("question") });
      });
      return questionList;
    }

    function getCandidateWidth($candidates) {
      var candidatesToShow = getCandidatesToShow($candidates);
      if (isRunningMateOffice($candidates))
        switch (candidatesToShow) {
        case 2:
          return 402;
        default:
          return 402;
      }
      else
        switch (candidatesToShow) {
        case 4:
          return 201;
        case 3:
          return 268;
        default:
          return 402;
      }
    }

    function getCandidatesToShow($candidates) {
      return Math.min(getMaxCandidatesToShow($candidates), getVisibleCandidates($candidates).length);
    }

    var getIssueClasses = function ($question) {
      var result = {};
      $question.classes(function (className) {
        if (className.substr(0, 2) === "i-")
          result.issueClass = className;
        else if (className.substr(0, 2) === "q-")
          result.questionClass = className;
      });
      return result;
    };

    function getMaxCandidatesToShow($candidates) {
      return isRunningMateOffice($candidates) ? 2 : 4;
    }

    var getSelectedCongress = function () {
      return $(".entry .congress-list").val();
    };

    var getSelectedCounty = function () {
      return $(".entry .county-list").val();
    };

    var getSelectedElection = function () {
      return $(".entry .election-list").val();
    };

    var getSelectedState = function () {
      return $(".entry .state-list").val();
    };

    var getSelectedStateHouse = function () {
      return $(".entry .state-house-list").val();
    };

    var getSelectedStateSenate = function () {
      return $(".entry .state-senate-list").val();
    };

    function getVisibleCandidates($candidates) {
      return $candidates.children(":not(.hidden).candidate");
    }

    var initReport = function (fromAjax) {
      var $report = $("#VoteReport");
      setRedArrow();
      if (fromAjax) {
        $(".jqueryui-tabs", $report).tabs(
        {
          show: 400
        });

        $(".accordion", $report).accordion(
        {
          heightStyle: "content",
          active: false,
          collapsible: true
        });

        // we can start jQuery elements hidden so they don't display uninitialized content, 
        // then show them here after fully loaded and formatted
        $(".start-hidden", $report).show();
        $("input[type=checkbox].kalypto", $report).kalypto({ toggleClass: "kalypto-checkbox" });
        // following for asp checkbox lists
        $("table.kalypto input[type=checkbox]", $report).kalypto({ toggleClass: "kalypto-checkbox" });

        util.initTipTip($(".tiptip"), $report);
      }

      $(".vcentered-tab").safeBind("click", util.tabClick);
      $(".accordion").accordion("option", "collapsible", false);
      $("#MainTabs").on("tabsactivate", onMainTabActivate);
      // initial setup
      onMainTabActivate();

      $(".accordion-tab")
        .safeBind("click", function (event) {
          // relay click from fake accordion tab to real tab
          var $this = $(this);
          $(".accordion-tab", $this.closest("ul"))
            .removeClass("ui-state-active");
          $("#" + $this.attr("tabid")).click();
          $this.addClass("ui-state-active");
          event.target.blur();
        })
        .hover(function () { $(this).toggleClass("ui-state-hover"); });

      $(".accordion").safeBind("accordionactivate", function (event, ui) {
        // re-click previous selection if there was one, otherwise the first
        var $active = $(".accordion-tab.ui-state-active", ui.newPanel);
        if ($active.length === 1) $active.click();
        else $(".accordion-tab:first-child", ui.newPanel).click();
      }).accordion("option", "active", 0);
      // bind click to who tab control to handle clicks on the bio-bar
      $("#MainTabs").safeBind("click", function (event) {
        var $target = $(event.target);
        if ($target.hasClass("disabled")) return false;
        if ($target.hasClass("bio-bar-item"))
          return onClickBioBar(event);
        if ($target.hasClass("scroll-arrow"))
          return onClickCandidateScroll(event);
        if ($target.hasClass("issue-bar-arrow"))
          return onClickIssueBarArrow(event);
        if ($target.hasClass("issue-text"))
          return onClickIssueText(event);
        if ($target.hasClass("question-text"))
          return onClickQuestionText(event);
        if ($target.hasClass("text-menu-item"))
          return onClickTextMenu(event);
      });
      $(".candidates").draggable({
        drag: function(event, ui) {
          synchronizeCandidates($(this), ui.position.left);
        },
        stop: function() {
          // make position on candidate boundary
          var $this = $(this);
          var width = getCandidateWidth($this);
          var left = Math.round($this.position().left / width) * width;
          $this.css("left", left);
          synchronizeCandidates($this, left, true);
        }
      });
      $(".candidates").each(function() {
        setCandidatesCursor($(this));
      });

      if (!fromAjax) {
        var office = ($.queryString("office") || "").toLowerCase();
        if (office) {
          var $accordionTab = $(".accordion-tab[tabid=Tab_" + office + "]");
          if ($accordionTab.length) {
            var $accordion = $accordionTab.closest(".accordion");
            var $accordionPanel = $accordionTab.closest("ul");
            var $accordionPanels = $accordion.children("ul");
            $accordion.accordion("option", "active", $accordionPanels.index($accordionPanel));
            $accordionTab.click();
          }
          // pre-select the tab
          var tabid = "tab-" + office;
          var $tab = $("#" + tabid);
          var $tabs = $tab.closest(".tab-control");
          //alert(util.getTabIndex($tabs.attr("id"), tabid));
          $tabs.tabs("option", "active", util.getTabIndex($tabs.attr("id"), tabid));
          var $maintab = $tabs.closest(".tab-panel");
          var $maintabs = $maintab.closest(".tab-control");
          $maintabs.tabs("option", "active", util.getTabIndex($maintabs.attr("id"), $maintab.attr("id")));
        }
      }
    };

    function isRunningMateOffice($candidates) {
      return $candidates.closest(".tab-panel").hasClass("running-mate-office");
    }

    var loadState = function (state, county, congress, stateSenate, stateHouse) {
      var init = state === true;
      if (!init && state) $(".entry .state-list").val(state);
      util.openAjaxDialog("Getting state information...");
      pendingAjaxCompletions = 5;
      populateCountyDropdown(county, init);
      populateCongressDropdown(congress, init);
      populateStateSenateDropdown(stateSenate, init);
      populateStateHouseDropdown(stateHouse, init);
      populateElectionDropdown(init);
    };

    var onChangeStateList = function(init) {
      if (getSelectedState())
        loadState(init === true);
      else {
        util.populateDropdown($(".entry .county-list"), [],
         "<find address or select state>", "");
        util.populateDropdown($(".entry .congress-list"), [],
         "<find address or select state>", "");
        util.populateDropdown($(".entry .state-senate-list"), [],
         "<find address or select state>", "");
        util.populateDropdown($(".entry .state-house-list"), [],
         "<find address or select state>", "");
        util.populateDropdown($(".entry .election-list"), [],
         "<find address or select state>", "");
      }
      setRedArrow();
    };

    var onClickBioBar = function(event) {
      var $target = $(event.target);
      if (!$target.hasClass("disabled selected")) {
        // find the bio class name
        var bioClass = null;
        $target.classes(function(className) {
          if (className.substr(0, 10) === "bio-class-") {
            bioClass = className.substr(10);
            return false;
          }
        });
        // remove existing selection
        $("li", $target.closest("ul")).removeClass("selected");
        // make this item selected
        $target.addClass("selected");
        // get showing panel
        var $tabPanel = $target.closest(".tab-panel");
        var $showing = $(".info-group:not(.hidden)", $tabPanel);
        $showing.slideUp(function () { $showing.addClass("hidden"); });
        var isIssues = bioClass === "issues";
        $(".issue-bar", $tabPanel).toggleClass("hidden", !isIssues);
        if (isIssues) {
          // if no question is selected, show the first one
          var $question = $(".issue-group.selected", $tabPanel);
          if ($question.length === 0)
            $question = $(".issue-group", $tabPanel);
          showQuestion($question);
        } else {
          // show this bio panel
          var $toShow = $(".info-group." + bioClass, $tabPanel);
          $toShow.slideDown(function () { $toShow.removeClass("hidden"); });
        }
      }
    };

    var onClickCandidateScroll = function (event) {
      var $target = $(event.target);
      // get the candidates 
      var $tabPanel = $target.closest(".tab-panel");
      var $candidatesDiv = $(".candidates", $tabPanel);
      var $candidates = getVisibleCandidates($candidatesDiv);
      var candidateCount = $candidates.length;
      var fullWidth = Math.round($($candidates[1]).offset().left - $($candidates[0]).offset().left);
      var currentPosition = $candidatesDiv.css("left"); // in px
      if (currentPosition.substr(currentPosition.length - 2) !== "px")
        return; // should always be true;
      currentPosition = parseInt(currentPosition.substr(0, currentPosition.length - 2));
      var newPosition = $target.hasClass("right") ? currentPosition - fullWidth :
        currentPosition + fullWidth;
      var maxVisible = Math.floor($(".candidate-frame", $tabPanel).width() / fullWidth + .001);
      var min = (maxVisible - candidateCount) * fullWidth;
      if (newPosition > 0 || newPosition < min) return;
      var newPositionPx = newPosition + "px";
      $candidatesDiv.animate({ "left": newPositionPx },
      { duration: 400, queue: false, complete: function () {
        // set enabling
        $(".scroll-arrow.left", $tabPanel).toggleClass("disabled", newPosition === 0);
        $(".scroll-arrow.right", $tabPanel).toggleClass("disabled", newPosition === min);
      } 
      });
      // move each info-group (children of info-group-frame)
      // only animate the visible one
      synchronizeCandidates($candidatesDiv, newPosition, true, true);
    };

    var onClickFindAddress = function () {
      var address = $.trim($(".entry .address-entry").val());
      if (!address) return;
      util.openAjaxDialog("Finding address...");
      util.ajax({
        url: "/WebService.asmx/FindAddress",
        data: {
          input: address,
          email: "",
          siteId: "",
          remember: false
        },

        success: function (result) {
          util.closeAjaxDialog();
          if (result.d.SuccessMessage) {
            //util.alert("Found the address. Select an election then\n" +
            //  "Get the Vote-USA Election Explorer", function() {
                var o = result.d;
              loadState(o.State, o.County, o.Congress, o.StateSenate, o.StateHouse);
            //});
          } else
            util.alert(result.d.ErrorMessages.join("<br />"));
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "Could not find the address"));
        }
      });
    };

    var onClickGetExplorer = function() {
      var state = getSelectedState();
      if (!state) {
        util.alert("Please enter an address or select a state.");
        return;
      }
      var countyCode = getSelectedCounty();
      if (!countyCode) {
        util.alert("Please enter an address or select a county.");
        return;
      }
      var congress = getSelectedCongress();
      if (!congress) {
        util.alert("Please enter an address or select a congressional district.");
        return;
      }
      var stateSenate = getSelectedStateSenate();
      if (!stateSenate) {
        util.alert("Please enter an address or select a state senate district.");
        return;
      }
      var stateHouse = getSelectedStateHouse();
      if (!stateHouse) {
        util.alert("Please enter an address or select a state house district.");
        return;
      }
      var electionKey = getSelectedElection();
      if (!electionKey) {
        util.alert("Please select an election.");
        return;
      }
      util.openAjaxDialog("Getting the Vote-USA Election Explorer...");
      util.ajax({
        url: "/WebService.asmx/GetElectionExplorer",
        data: {
          electionKey: electionKey,
          congress: congress,
          stateSenate: stateSenate,
          stateHouse: stateHouse,
          countyCode: countyCode
        },

        success: function (result) {
          util.closeAjaxDialog();
          if (!result.d.Html)
            util.alert("We could not find any information for the selected election.");
          else {
            $(".report-placeholder").html(result.d.Html);
            $("#H1").html(result.d.Title);
            initReport(true);
          }
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "Could not get the Election Explorer"));
        }
      });
    };

    var onClickIssueBarArrow = function(event) {
      var $target = $(event.target);
      var $tabPanel = $target.closest(".tab-panel");
      var $showing = $(".issue-group:not(.hidden)", $tabPanel);
      var classes = getIssueClasses($showing);
      if ($target.hasClass("issue-arrow")) {
        if ($target.hasClass("left"))
          showQuestion($showing.prevAll(".issue-group:not(." + classes.issueClass + ")"));
        else
          showQuestion($showing.nextAll(".issue-group:not(." + classes.issueClass + ")"));
      } else {
        if ($target.hasClass("left"))
          showQuestion($showing.prev(".issue-group"));
        else
          showQuestion($showing.next(".issue-group"));
      }
    };

    var onClickIssueText = function (event) {
      var $target = $(event.target);
      var $tabPanel = $target.closest(".tab-panel");

      var issueList = buildIssueListForOffice($tabPanel);

      // build the menu
      var currentClass = $target.attr("key");
      var currentIndex = 0;
      var menuItems = [];
      $.each(issueList, function() {
        if (this.issueClass === currentClass) currentIndex = menuItems.length;
        menuItems.push('<div class="text-menu-item" key="' + this.issueClass + '">' + this.issue + '</div>');
      });
      // stick it it
      var $menu = $(".text-issue-menu", $tabPanel)
        .css("top", (-16 * currentIndex) + "px")
        .html(menuItems.join(""))
        .fadeIn(300, function() {
          $(document)
            .safeBind("click", function() {
              $menu.fadeOut(300);
              $(document).off("click");
            });
        });
        };

      function onClickSelectCandidatesCheckbox() {
        var $checkboxes = $('#select-candidates-dialog input[type="checkbox"]:not([rel="all"])');
        if ($(this).attr("rel") === "all")
          $checkboxes.prop("checked", $checkboxes.length !== $checkboxes.filter(":checked").length);
        setSelectCandidatesAllCheckbox();
      }

      function onClickSelectCandidatesOk() {
        var $dialog = $("#select-candidates-dialog");
        var $candidates = $dialog.data("candidates");
        var $panel = $candidates.closest(".tab-panel");
        var $checkboxes = $('input[type="checkbox"]:not([rel="all"])', $dialog);
        var $children = $candidates.children(".candidate");
        var $infoGroups = $(".info-group", $panel);
        var selected = 0;
        var runningMateOffice = isRunningMateOffice($candidates);
        $checkboxes.each(function(inx) {
          var hidden = !$(this).prop("checked");
          if (!hidden) selected++;
          $($children[inx]).toggleClass("hidden", hidden);
          $infoGroups.each(function () {
            if (runningMateOffice) {
              $($(".info-item", $(this))[2 * inx]).toggleClass("hidden", hidden);
              $($(".info-item", $(this))[2 * inx + 1]).toggleClass("hidden", hidden);
            } else
              $($(".info-item", $(this))[inx]).toggleClass("hidden", hidden);
          });
        });
        if (selected == $checkboxes.length) selected = "all";
        $(".number-selected", $panel).html(selected + " are selected.");
        setCandidatesWidthAndContainment($candidates);
        setCandidatesCursor($candidates);
        $candidates.css("left", 0); // reset hscroll
        synchronizeCandidates($candidates, 0, true);
        $(".many-candidates .scroll-message", $panel).toggleClass("hidden",
          $candidates.width() <= $candidates.parent().width());
        $("#select-candidates-dialog").dialog("close");
      }

      var onClickTextMenu = function(event) {
        var $target = $(event.target);
        var $tabPanel = $target.closest(".tab-panel");
        showQuestion($(".issue-group." + $target.attr("key"), $tabPanel));
      };

      var onClickQuestionText = function (event) {
        var $target = $(event.target);
        var $tabPanel = $target.closest(".tab-panel");

        var questionList = buildQuestionListForIssueInOffice($tabPanel);
        // build a list of questions for the current issue for this office
        //var currentIssueClass = $(".issue-text", $tabPanel).attr("key");
        //var questionList = [];
        //$(".issue-group." + currentIssueClass, $tabPanel).each(function() {
        //  var $this = $(this);
        //  var c = getIssueClasses($this);
        //  questionList.push({ questionClass: c.questionClass, question: $this.attr("question") });
        //});

        // build the menu
        var currentClass = $target.attr("key");
        var currentIndex = 0;
        var menuItems = [];
        $.each(questionList, function() {
          if (this.questionClass === currentClass) currentIndex = menuItems.length;
          menuItems.push('<div class="text-menu-item" key="' + this.questionClass + '">' + this.question + '</div>');
        });
        // stick it it
        var $menu = $(".text-question-menu", $tabPanel)
          .css("top", (-16 * currentIndex) + "px")
          .html(menuItems.join(""))
          .fadeIn(300, function() {
            $(document)
              .safeBind("click", function() {
                $menu.fadeOut(300);
                $(document).off("click");
              });
          });
      };

    var onMainTabActivate = function (event, ui) {
      // for the initial call, get the active main tab
      var $newPanel = ui ? ui.newPanel : util.getCurrentTabPanel("MainTabs");
      // if main-tab, find inner active tab
      if ($newPanel.hasClass("main-tab"))
        $newPanel = util.getCurrentTabPanel($(".vtab-control", $newPanel).attr("id"));
      // return if we already did it
      if ($newPanel.hasClass("sized")) return;
      // get its tab control height
      var $tabControl = $newPanel.closest(".vtab-control");
      // we set the max-height of each bio-item to the height difference
      // subtract 10 to allow a little padding. But never less tan 150.
      var available = $tabControl.height() - $newPanel.height() - 10;
      available = Math.max(available, 150);
      // set all max-height
      $(".bio-item", $newPanel).animate({ "max-height": available + "px" });
      // do the issues too, but add 22 px for the issue bar
      // no need to animate
      $(".issue-item", $newPanel).css("max-height", (available - 22) + "px");
      // mark the panel sized
      $newPanel.addClass("sized");
    };

    function onMouseDownCandidates() {
      setCandidatesWidthAndContainment($(this));
    }

    function onSelectCandidates() {
      var $this = $(this);
      var $dialog = $('#select-candidates-dialog');
      var $candidates = $this.closest(".tab-panel").find(".candidates");
      $dialog.data("candidates", $candidates);
      var candidateList = [];
      candidateList.push('<input type="checkbox" rel="all"> <b>All</b>');
      var $children = $candidates.children(".candidate");
      $children.each(function() {
        var $child = $(this);
        var checked = $child.hasClass("hidden") ? "" : ' checked="checked"';
        candidateList.push('<input type="checkbox"' + checked + '> ' + $child.find(".name").first().text());
      });
      $(".candidate-list", $dialog).html("<p>" + candidateList.join("</p><p>") + "</p>");
      setSelectCandidatesAllCheckbox();
      $dialog
        .dialog("option", "position", { my: "right top", at: "right bottom", of: $this })
        .dialog("open");
    }

    var populateCountyDropdown = function (county, init) {
      util.ajax({
        url: "/WebService.asmx/GetCounties",
        data: {
          stateCode: getSelectedState()
        },

        success: function (result) {
          util.populateDropdown($('.entry .county-list'), result.d,
            "<select a county>", "", county || "");
          if (init) {
            var code = ($.queryString("county") || "").toUpperCase();
            if (code) $(".county-list").val(code);
          }
          ajaxCompleted();
          setRedArrow();
        },

        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not get counties"));
          util.populateDropdown($('.entry .county-list'), [],
            "<could not get data>", "", "");
          ajaxCompleted();
          setRedArrow();
        }
      });
    };

    var populateCongressDropdown = function (congress, init) {
      util.ajax({
        url: "/WebService.asmx/GetCongressionalDistricts",
        data: {
          stateCode: getSelectedState(),
          countyCode: "",
          localCode: ""
        },

        success: function (result) {
          if (congress && congress.length === 3)
            congress = congress.substr(1);
          util.populateDropdown($('.entry .congress-list'), result.d,
            "<select congressional district>", "", congress || "");
          if (init) {
            var code = ($.queryString("congress") || "").toUpperCase();
            if (code.length === 3) code = code.substr(1);
            if (code) $(".congress-list").val(code);
          }
          ajaxCompleted();
          setRedArrow();
        },

        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not get congressional district"));
          util.populateDropdown($('.entry .congress-list'), [],
            "<could not get data>", "", "");
          ajaxCompleted();
          setRedArrow();
        }
      });
    };

    var populateElectionDropdown = function (init) {
      util.ajax({
        url: "/WebService.asmx/GetElections",
        data: {
          stateCode: getSelectedState()
        },

        success: function (result) {
          util.populateDropdown($('.entry .election-list'), result.d,
            null, null, "");
          if (init) {
            var code = ($.queryString("election") || "").toUpperCase();
            if (code) $(".election-list").val(code);
          }
          ajaxCompleted();
          setRedArrow();
        },

        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not get elections"));
          util.populateDropdown($('.entry .election-list'), [],
            "<could not get data>", "", "");
          ajaxCompleted();
          setRedArrow();
        }
      });
    };

    var populateStateHouseDropdown = function (stateHouse, init) {
      util.ajax({
        url: "/WebService.asmx/GetStateHouseDistricts",
        data: {
          stateCode: getSelectedState(),
          countyCode: "",
          localCode: ""
        },

        success: function (result) {
          util.populateDropdown($('.entry .state-house-list'), result.d,
            "<select state house district>", "", stateHouse || "");
          if (init) {
            var code = ($.queryString("statehouse") || "").toUpperCase();
            if (code) $(".state-house-list").val(code);
          }
          ajaxCompleted();
          setRedArrow();
        },

        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not get state house district"));
          util.populateDropdown($('.entry .state-house-list'), [],
            "<could not get data>", "", "");
          ajaxCompleted();
          setRedArrow();
        }
      });
    };

    var populateStateSenateDropdown = function (stateSenate, init) {
      util.ajax({
        url: "/WebService.asmx/GetStateSenateDistricts",
        data: {
          stateCode: getSelectedState(),
          countyCode: "",
          localCode: ""
        },

        success: function (result) {
          util.populateDropdown($('.entry .state-senate-list'), result.d,
            "<select state senate district>", "", stateSenate || "");
          if (init) {
            var code = ($.queryString("statesenate") || "").toUpperCase();
            if (code) $(".state-senate-list").val(code);
          }
          ajaxCompleted();
          setRedArrow();
        },

        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not get state senate district"));
          util.populateDropdown($('.entry .state-senate-list'), [],
            "<could not get data>", "", "");
          ajaxCompleted();
          setRedArrow();
        }
      });
    };

    function setCandidatesCursor($candidates) {
      var $children = getVisibleCandidates($candidates);
      var candidatesToShow = getCandidatesToShow($candidates);
      $candidates.toggleClass("canmove", $children.length > getCandidatesToShow($candidates));
      var $panel = $candidates.closest(".tab-panel");
      $panel.removeClass("up4 up3 up2 up2rm up1rm");
      if (isRunningMateOffice($candidates))
        switch (candidatesToShow) {
        case 2:
          $panel.addClass("up2rm");
          break;
        default:
          $panel.addClass("up1rm");
          break;
      }
      else
        switch (candidatesToShow) {
        case 4:
          $panel.addClass("up4");
          break;
        case 3:
          $panel.addClass("up3");
          break;
        default:
          $panel.addClass("up2");
          break;
      }
    }

    function setCandidatesWidthAndContainment($candidates) {
      $candidates.width(getVisibleCandidates($candidates).length * getCandidateWidth($candidates));
      var $parent = $candidates.parent();
      var left = $parent.offset().left;
      var x1 = left - Math.max(0, ($candidates.width() - $parent.width()));
      var y = $parent.offset().top;
      var x2 = left;
      $candidates.draggable("option", "containment", [x1, y, x2, y]);
    }

    function setRedArrow() {
      var step = "0";
      var $report = $("#VoteReport");
      if (!$report.length) {
        step = "1";
        if (getSelectedState() && getSelectedCounty() && getSelectedCongress() &&
          getSelectedStateHouse() && getSelectedStateSenate())
          step = getSelectedElection() ? "3" : "2";
      }
      $(".red-arrow").removeClass("step-1 step-2 step-3");
      switch (step) {
        case "1":
        case "2":
        case "3":
          $(".red-arrow").addClass("step-" + step);
          break;
      }
    }

    function setSelectCandidatesAllCheckbox() {
      var $dialog = $("#select-candidates-dialog");
      var $all = $('input[type="checkbox"][rel="all"]', $dialog);
      var $checkboxes = $('input[type="checkbox"]:not([rel="all"])', $dialog);
      $all.prop("checked", $checkboxes.length === $checkboxes.filter(":checked").length);
    }

    var showQuestion = function ($question) {
      if ($question.length === 0) return;
      $question = $($question[0]); // first only
      var $tabPanel = $question.closest(".tab-panel");
      // first, hide any visible question
      var $showing = $(".issue-group:not(.hidden)", $tabPanel);
      if ($showing.length && $showing[0] === $question[0]) return; // already
      $showing.slideUp(function () { $showing.addClass("hidden"); });
      // .. and remove selected class (used for persistence)
      $(".issue-group.selected", $tabPanel).removeClass("selected");

      // update the issue bar text, titles and keys
      var classes = getIssueClasses($question);
      var issue = $question.attr("issue");
      var question = $question.attr("question");
      var issueCount = buildIssueListForOffice($tabPanel).length;
      $(".issue-bar-text.issue-text", $tabPanel)
       .attr("title", issueCount > 1
         ? "Click to select another issue" 
         : "This is the only issue with responses for this office")
       .attr("key", classes.issueClass)
       .html(util.htmlEscape(issue));
      var questionCount = buildQuestionListForIssueInOffice($tabPanel).length;
      $(".issue-bar-text.question-text", $tabPanel)
       .attr("title", questionCount > 1
         ? "Click to select another question for this issue"
         : "This is the only question with responses for this issue")
       .attr("key", classes.questionClass)
       .html(util.htmlEscape(question));

      // show the new one
      $question.slideDown(function () { $question.removeClass("hidden").addClass("selected"); });

      // get the issue class and question class 
      //var $issueGroup = $question.closest(".issue-group");
      var $frame = $question.parent();

      // get all issues for the office and all questions for the issue
      var $questions = $frame.children(".issue-group");

      // enable the elements based on endpoints
      var $firstQuestion = $($questions[0]);
      var $lastQuestion = $($questions[$questions.length - 1]);
      var atFirstIssue = $firstQuestion.hasClass(classes.issueClass);
      var atLastIssue = $lastQuestion.hasClass(classes.issueClass);
      $(".issue-bar-arrow.issue-arrow.left", $tabPanel)
        .toggleClass("disabled", atFirstIssue);
      $(".issue-bar-arrow.issue-arrow.right", $tabPanel)
        .toggleClass("disabled", atLastIssue);
      $(".issue-bar-text.issue-text", $tabPanel)
      .toggleClass("disabled", atFirstIssue && atLastIssue);

      var atFirstQuestion = $firstQuestion.hasClass(classes.questionClass);
      var atLastQuestion = $lastQuestion.hasClass(classes.questionClass);
      $(".issue-bar-arrow.question-arrow.left", $tabPanel)
        .toggleClass("disabled", atFirstQuestion);
      $(".issue-bar-arrow.question-arrow.right", $tabPanel)
        .toggleClass("disabled", atLastQuestion);
      $(".issue-bar-text.question-text", $tabPanel)
      .toggleClass("disabled", atFirstQuestion && atLastQuestion);
    };

    function synchronizeCandidates($candidates, left, all, animate) {
      // move each info-group (children of info-group-frame)
      // only animate the visible one
      if (typeof left === "undefined")
        left = $candidates.position().left;
      var $tabPanel = $candidates.closest(".tab-panel");
      $(".info-group-frame", $tabPanel).children(
       all ? null : ":not(.hidden)").each(function () {
        var $this = $(this);
        if (!animate || $this.hasClass("hidden"))
          $this.css("left", left);
        else {
          $this.animate({ "left": left }, { duration: 400, queue: false });
        }
      });
      $(".scroll-arrow.left", $tabPanel).toggleClass("disabled", left === 0);
      $(".scroll-arrow.right", $tabPanel).toggleClass("disabled", ($candidates.width() + left) <= $candidates.parent().width());
    }

    master.inititializePage({
      callback: initPage
    });
  });