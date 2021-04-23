define(["jquery", "vote/adminMaster", "vote/util", "monitor",
    "vote/politician/updateAnswer", "jqueryui"],
  function ($, master, util, monitor, updateAnswer) {

    var $$ = util.$$;

    function afterUpdateContainer(group, args) {
      if (!group) return;
      initGroup($$(group.container));
    }

    function initMainTab($panel) {
      //find the active panel for the new main tab and call initSideTab
      initSideTab($(".ui-tabs-panel[aria-hidden='false']", $panel.find(".vtab-control")).filter(":visible"));
    }

    function initSideTab($panel) {
      //alert($panel.attr("id"));
      var $accordion = $panel.find(".accordion-deferred");
      if ($accordion.length) {
        $accordion.removeClass("accordion-deferred").addClass("accordion")
          .accordion({
            heightStyle: "content",
            active: false,
            collapsible: true
          });
      }
    }

    function initGroup($group) {
//      if ($group != null && $group.hasClass("answer-container")) return; // handled in answer
//      $(".date-picker", $group).datepicker({
//        changeYear: true,
//        yearRange: "2010:+0"
//      });
//      $("#main-tabs")
//        .on("accordionactivate", ".accordion", function (event, ui) {
//          updateAnswer.initGroup(ui.newPanel.find(".answer-container"));
//        });
    }

    var deferredHide = null;

    function initPage() {
      monitor.init();
      monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);

      initGroup(null);

      $("body")
        .on("tabsactivate", ".vtab-control,.htab-control", function (event, ui) {
          var $this = $(this);
          if ($this.hasClass("vtab-control")) {
            initSideTab(ui.newPanel);
            event.stopPropagation();
          }
          else if ($this.hasClass("htab-control"))
            initMainTab(ui.newPanel);
        });
        initMainTab(util.getCurrentTabPanel("main-tabs"));
      
      $("#main-tabs")
        .on("accordionactivate", ".accordion", function (event, ui) {
          updateAnswer.initGroup(ui.newPanel.find(".answer-container"));
        });

      $(".search-box input")
        .on("textchange", function () {
          onSearchChanged();
        })
        .on("focus", function () {
          if (deferredHide) {
            clearTimeout(deferredHide);
            deferredHide = null;
          }
          $(".search-box .results").show(500);
        })
        .on("blur", function () {
          if (!deferredHide) {
//            deferredHide = setTimeout(function() {
//              $(".search-box .results").hide();
//            }, 500);
            $(".search-box .results").hide(500);
          }
        });

      $(".search-box .results").on("click", "p", onClickSearchResult);

      util.safeBind($(".vcentered-tab"), "click", util.tabClick);
      window.onbeforeunload = function () {
        if (monitor.hasChanges())
          return "There are entries on your form that have not been submitted";
      };
    }

    var topics = null;
    var scheduledSearch = null;

    function doSearch() {
      scheduledSearch = null;
      var matchedTopics = [];
      var wds = $(".search-box input").val().replace(/[^a-z0-9 ]/ig, " ").split(" ");
      var searchWords = [];
      $.each(wds, function () {
        var wd = this.toString().toLowerCase();
        if (wd.length > 1 && $.inArray(wd, searchWords) === -1)
          searchWords.push(wd);
      });
      if (searchWords.length > 1 || (searchWords.length == 1 && searchWords[0].length > 2)) {
        if (!topics) {
          // build topics
          topics = [];
          $("h3.accordion-header").each(function () {
            var $this = $(this);
            topics.push({ $: $this, topic: $this.text()/*.replace(/[^a-z0-9 ]/ig, "")*/ });
          });
        }
        var regex = new RegExp("(" + searchWords.join(")|(") + ")", "ig");
        $.each(topics, function (inx) {
          var topic = this.topic;
          var uniques = [];
          var matchedTopic = topic.replace(regex, function (m) {
            var lc = m.toLowerCase();
            if ($.inArray(lc, uniques) === -1)
              uniques.push(lc);
            return '<strong>' + m + '</strong>';
          });
          if (uniques.length === searchWords.length) {
            matchedTopics.push('<p data-inx="' + inx + '">' + matchedTopic + '</p>');
          }
        });
      }
      $(".search-box .results").html(matchedTopics.join(""));
    }

    function onClickSearchResult() {
      var topic = topics[$(this).data("inx")];
      
      // set the correct side tab
      var $vtabs = topic.$.closest(".vtab-control");
      var $vtabPanel = topic.$.closest(".vtab-panel");
      var vtabIndex = $(">div", $vtabs).index($$($vtabPanel.attr("id")));
      $vtabs.tabs("option", "active", vtabIndex);
      initSideTab($vtabPanel);
      
      // set the correct top tab
      var $htabPanel = topic.$.closest(".htab-panel");
      var htabIndex = util.getTabIndex("main-tabs", $htabPanel.attr("id"));
      $("#main-tabs").tabs("option", "active", htabIndex);
      
      // activate the accordion
      var accordionIndex = topic.$.index() / 2;
      topic.$.closest(".accordion").accordion("option", "active", accordionIndex);
    }

    function onSearchChanged() {
      if (scheduledSearch) clearTimeout(scheduledSearch);
      scheduledSearch = setTimeout(doSearch, 100);
    }

    master.inititializePage({
      callback: initPage
    });
  });