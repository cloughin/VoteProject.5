define([
  "jquery", "vote/adminMaster", "vote/util", "vote/controls/selectJurisdictions", "moment",
    "jqueryui", "slimscroll", "dynatree"
  ],
  function ($, master, util, selectJurisdictions, moment) {

    //
    // Misc
    //

    var currentTabData;
    var currentTabInitalJson;

    function initPage() {

      window.onbeforeunload = function() {
        if (isTabChanged())
          return "There are changes on your form that have not been saved";
      };

      $('#main-tabs')
        .on("tabsbeforeactivate", function(event, ui) {
          return beforeTabActivate(ui.oldPanel[0].id, ui);
        })
        .on("tabsactivate", function(event, ui) {
          tabActivated(ui.newPanel[0].id);
        })
        .on("click", ".mode-button", function() {
          var $mainTab = $(this).closest(".main-tab");
          $mainTab.toggleClass("add-mode").toggleClass("change-mode");
          $mainTab.trigger("modechanged", $mainTab.hasClass("change-mode"));
        })
        .on("click", ".included-radios input[type=radio]", function() {
          var $this = $(this);
          if (!$this.prop("checked")) return;
          var $container = $this.closest(".included-container");
          var included = $this.val() === "I";
          $container.toggleClass("included", included).trigger("includedchanged", included);
        })
        .on("click", ".drag-box.can-select p", function() {
          var $this = $(this);
          var wasSelected = $this.hasClass("selected");
          var $dragBox = $this.closest(".drag-box");
          $(".selected", $dragBox).removeClass("selected");
          if (wasSelected) {
            $dragBox.trigger("selectionchanged", -1);
          } else {
            $this.addClass("selected");
            $dragBox.trigger("selectionchanged", $this.data("id"));
          }
        })
        .on("click", ".no-of-answers-msg span", onClickAnswerDates);

      $(".drag-box").sortable({ containment: "parent", distance: 5 });

      initIssueGroups();
      initIssues();
      initTopics();
      initConsolidateTopics();

      tabActivated(util.getCurrentTabId("main-tabs"));
    }

    function beforeTabActivate(id, ui) {
      if (!isTabChanged()) return true;
      util.confirm("There are unsaved changes on the panel you are leaving.\n\n" +
        "Click OK to discard the changes and continue.\n" +
        "Click Cancel to return to the changed panel.",
        function(button) {
          if (button === "Ok") {
            currentTabData = null;
            $("#main-tabs").tabs("option", "active",
              util.getTabIndex("#main-tabs", ui.newPanel[0].id));
          }
        });
      return false;
    }

    function getIncludedExcluded($tab) {
      return $(".included-radios input[type=radio]:checked", $tab).val();
    }

    function isTabChanged() {
      return currentTabData != null &&
        currentTabInitalJson != JSON.stringify(currentTabData);
    }

    function setTabChanged($tab) {
      var changed = isTabChanged();
      $tab.toggleClass("has-changes", changed);
      $(".enable-if-changed", $tab).toggleClass("disabled", !changed);
    }

    function tabActivated(id) {
      switch (id) {
      case "tab-issuegroups":
        initializeTabIssueGroups();
        break;

      case "tab-issues":
        initializeTabIssues();
        break;

      case "tab-questions":
        initializeTabTopics();
        break;

      case "tab-consolidatetopics":
        initializeTabConsolidateTopics();
        break;
      }
    }

    //
    // Issue Groups
    //

    var highestIssueGroupId;

    function initIssueGroups() {
      var $tab = $("#tab-issuegroups");
      $tab
        .on("modechanged", onIssueGroupsModeChanged)
        .on("selectionchanged", ".issue-groups", onIssueGroupsSelectionChanged)
        .on("sortstop", ".issue-groups", onIssueGroupsSortStop)
        .on("selectionchanged", ".issue-groups-issues", onIssueGroupsIssuesSelectionChanged)
        .on("sortstop", ".issue-groups-issues", onIssueGroupsIssuesSortStop)
        .on("includedchanged", ".included-container", onIssueGroupsIncludedChanged)
        .on("propertychange change click keyup input paste",
          ".issue-group-name,.issue-group-subheading,#issue-group-disabled",
          onIssueGroupsDataChange)
        .on("click", ".add-issue-group-button", onClickAddIssueGroup)
        .on("click", ".add-issue-to-group-button", onClickAddIssueToGroup)
        .on("click", ".delete-issue-group-button", onClickDeleteIssueGroup)
        .on("click", ".cancel-button", onClickIssueGroupsCancel)
        .on("click", ".save-button", onClickIssueGroupsSave);
    }

    function getIssueGroupById(id) {
      var result = null;
      $.each(currentTabData, function() {
        if (this.IssueGroupId === id) {
          result = this;
          return false;
        }
      });
      return result;
    }

    function getIssueGroupIssueById(issueGroup, id) {
      var result = null;
      $.each(issueGroup.Issues, function() {
        if (this.IssueId === id) {
          result = this;
          return false;
        }
      });
      return result;
    }

    function getSelectedIssueGroup() {
      return getIssueGroupById(getSelectedIssueGroupId());
    }

    function getSelectedIssueGroupId() {
      return $("#tab-issuegroups .issue-groups p.selected").data("id");
    }

    function initializeTabIssueGroups(selectedId) {
      var $tab = $("#tab-issuegroups");
      $tab.addClass("change-mode").removeClass("add-mode");
      $(".disabled", $tab).removeClass("disabled");
      $(".change-button-line .div-button", $tab).addClass("disabled");
      $(".delete-issue-group-button", $tab).addClass("disabled");
      $(".add-issue-to-group-button", $tab).addClass("disabled");
      $(".add-issue-group-button", $tab).addClass("disabled");
      $(".included-container", $tab).addClass("included");
      $("input", $tab).prop("checked", false);
      $("#show-included-issues", $tab).prop("checked", true);
      $(".drag-box", $tab).html("");
      $("input[type=text]", $tab).val("");
      $("#issue-group-disabled", $tab).prop("checked", false);
      $(".issue-groups", $tab).addClass("can-select").sortable("option", "disabled", true);
      $(".issue-groups-issues", $tab).removeClass("can-select").sortable("option", "disabled", false);
      issueGroupsEnableData(false);
      currentTabData = null;
      currentTabInitalJson = null;
      highestIssueGroupId = 0;
      util.openAjaxDialog("Loading Issue Groups...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadIssueGroups",

        success: function(result) {
          util.closeAjaxDialog();
          currentTabData = result.d;
          currentTabInitalJson = JSON.stringify(currentTabData);
          $.each(currentTabData, function() {
            if (this.IssueGroupId > highestIssueGroupId)
              highestIssueGroupId = this.IssueGroupId;
          });
          loadIssueGroups();
          if (selectedId) {
            $("#tab-issuegroups .issue-groups p").each(function() {
              var $this = $(this);
              if ($this.data("id") === selectedId) {
                $this.addClass("selected");
                onIssueGroupsSelectionChanged(null, selectedId);
              }
            });
          }
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load issue groups"));
        }
      });
    }

    function issueGroupsClearData() {
      var $tab = $("#tab-issuegroups");
      $(".issue-group-name", $tab).val("");
      $(".issue-group-subheading", $tab).val("");
      $("#issue-group-disabled", $tab).prop("checked", false);
    }

    function issueGroupsEnableData(enabled) {
      var $tab = $("#tab-issuegroups");
      $(".issue-group-name", $tab).prop("disabled", !enabled);
      $(".issue-group-subheading", $tab).prop("disabled", !enabled);
      $("#issue-group-disabled", $tab).prop("disabled", !enabled);
    }

    function issueGroupsDataChanged() {
      setTabChanged($("#tab-issuegroups"));
    }

    function loadIssueGroups(selectedId) {
      if (!selectedId) selectedId = getSelectedIssueGroupId();
      var groups = [];
      $.each(currentTabData, function() {
        var classes = [];
        if (selectedId === this.IssueGroupId) classes.push("selected");
        if (!this.IsEnabled) classes.push("omitted");
        groups.push('<p' +
          (classes.length ? ' class="' + classes.join(" ") + '"' : '') +
          ' data-id="' +
          this.IssueGroupId +
          '">' +
          this.Heading +
          ' (' +
          this.Issues.length +
          ')</p>');
      });
      $("#tab-issuegroups .issue-groups").html(groups.join(""));
    }

    function loadIssueGroupIssues() {
      var $tab = $("#tab-issuegroups");
      $(".add-issue-to-group-button", $tab).addClass("disabled");
      var $issues = $("#tab-issuegroups .issue-groups-issues");
      $issues.html("");
      var issueGroup = getSelectedIssueGroup();
      var included = getIncludedExcluded($("#tab-issuegroups"));
      if (issueGroup != null) {
        var issues = [];
        if (included === "I") {
          $.each(issueGroup.Issues, function() {
            issues.push('<p' +
              (this.IsEnabled ? '' : ' class="omitted"') +
              ' data-id="' +
              this.IssueId +
              '">' +
              this.Issue + " (" + this.Topics + ")" +
              '</p>');
          });
        } else {
          var excluded = [];
          $.each(currentTabData, function() {
            if (this !== issueGroup) {
              $.each(this.Issues, function() {
                excluded.push(this);
              });
            }
          });
          excluded.sort(function(a, b) {
            if (a.Issue > b.Issue) return 1;
            if (a.Issue < b.Issue) return -1;
            return 0;
          });
          $.each(excluded, function() {
            issues.push('<p' +
              (this.IsEnabled ? '' : ' class="omitted"') +
              ' data-id="' +
              this.IssueId +
              '">' +
              this.Issue + " (" + this.Topics + ")" +
              '</p>');
          });
        }
        $issues.html(issues.join(""));
      }
    }

    function onClickAddIssueGroup() {
      if ($(this).hasClass("disabled")) return;
      var $tab = $("#tab-issuegroups");
      var id = ++highestIssueGroupId;
      currentTabData.push(
        {
          IssueGroupId: id,
          IsEnabled: !$("#issue-group-disabled", $tab).prop("checked"),
          Heading: $(".issue-group-name", $tab).val(),
          SubHeading: $(".issue-group-subheading", $tab).val(),
          Issues: []
        });
      loadIssueGroups(id);
      onIssueGroupsSelectionChanged(null, id);
      issueGroupsClearData();
      onIssueGroupsDataChange();
    }

    function onClickAddIssueToGroup() {
      if ($(this).hasClass("disabled")) return;
      var issueId = $("#tab-issuegroups .issue-groups-issues p.selected").data("id");
      var issueGroup = getSelectedIssueGroup();
      var issue;
      $.each(currentTabData, function() {
        var g = this;
        var temp = [];
        $.each(g.Issues, function() {
          if (this.IssueId !== issueId) {
            temp.push(this);
          } else
            issue = this;
        });
        g.Issues = temp;
      });
      if (issue)
        issueGroup.Issues.push(issue);
      loadIssueGroups();
      onIssueGroupsSelectionChanged(null, getSelectedIssueGroupId()); // to enable properly
      issueGroupsDataChanged();
    }

    function onClickIssueGroupsCancel() {
      if ($(this).hasClass("disabled")) return;
      initializeTabIssueGroups();
    }

    function onClickDeleteIssueGroup() {
      if ($(this).hasClass("disabled")) {
        util.alert("Can only delete empty issue groups");
        return;
      }
      var id = getSelectedIssueGroupId();
      var temp = [];
      $.each(currentTabData, function() {
        if (this.IssueGroupId !== id)
          temp.push(this);
      });
      currentTabData = temp;
      loadIssueGroups();
      onIssueGroupsSelectionChanged(null);
      onIssueGroupsDataChange();
    }

    function onClickIssueGroupsSave() {
      if ($(this).hasClass("disabled")) return;
      var selectedId = getSelectedIssueGroupId();
      util.openAjaxDialog("Saving changes...");
      util.ajax({
        url: "/Admin/WebService.asmx/SaveIssueGroups",
        data: { data: currentTabData },

        success: function() {
          util.closeAjaxDialog();
          initializeTabIssueGroups(selectedId);
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not save changes"));
        }
      });
    }

    function onIssueGroupsDataChange() {
      var $tab = $("#tab-issuegroups");
      if ($tab.hasClass("change-mode")) {
        var issueGroup = getSelectedIssueGroup();
        if (issueGroup) {
          issueGroup.Heading = $(".issue-group-name", $tab).val();
          issueGroup.SubHeading = $(".issue-group-subheading", $tab).val();
          issueGroup.IsEnabled = !$("#issue-group-disabled", $tab).prop("checked");
          loadIssueGroups();
        }
      } else {
        $(".add-issue-group-button", $tab)
          .toggleClass("disabled", !$(".issue-group-name", $tab).val());
      }
      issueGroupsDataChanged();
    }

    function onIssueGroupsIncludedChanged(event, included) {
      var $issues = $("#tab-issuegroups .issue-groups-issues");
      $issues.toggleClass("can-select", !included);
      $issues.sortable("option", "disabled", !included);
      loadIssueGroupIssues();
    }

    function onIssueGroupsIssuesSelectionChanged(event, data) {
      $(".add-issue-to-group-button", $("#tab-issuegroups"))
        .toggleClass("disabled", data < 0);
    }

    function onIssueGroupsIssuesSortStop() {
      var temp = [];
      var issueGroup = getSelectedIssueGroup();
      $("#tab-issuegroups .issue-groups-issues p").each(function() {
        temp.push(getIssueGroupIssueById(issueGroup, $(this).data("id")));
      });
      issueGroup.Issues = temp;
      issueGroupsDataChanged();
    }

    function onIssueGroupsModeChanged(event, toChangeMode) {
      var $tab = $("#tab-issuegroups");
      $(".issue-groups", $tab)
        .toggleClass("can-select", toChangeMode)
        .toggleClass("disabled", !toChangeMode)
        .sortable("option", "disabled", !toChangeMode);
      var issueGroup = toChangeMode ? getSelectedIssueGroup() : null;
      $(".add-issue-group-button").addClass("disabled");
      if (issueGroup === null) {
        issueGroupsClearData();
      } else {
        // ReSharper disable once QualifiedExpressionMaybeNull
        $(".issue-group-name", $tab).val(issueGroup.Heading);
        $(".issue-group-subheading", $tab).val(issueGroup.SubHeading);
        $("#issue-group-disabled", $tab).prop("checked", !issueGroup.IsEnabled);
      }
      issueGroupsEnableData(!toChangeMode || issueGroup);
    }

    function onIssueGroupsSelectionChanged(dummy, id) {
      var $tab = $("#tab-issuegroups");
      issueGroupsClearData();
      $(".delete-issue-group-button", $tab).addClass("disabled");
      $.each(currentTabData, function() {
        if (this.IssueGroupId === id) {
          $(".issue-group-name", $tab).val(this.Heading);
          $(".issue-group-subheading", $tab).val(this.SubHeading);
          $("#issue-group-disabled", $tab).prop("checked", !this.IsEnabled);
          if (this.Issues.length === 0)
            $(".delete-issue-group-button", $tab).removeClass("disabled");
          return false;
        }
      });
      loadIssueGroupIssues();
      issueGroupsEnableData(getSelectedIssueGroupId() >= 0);
    }

    function onIssueGroupsSortStop() {
      var temp = [];
      $("#tab-issuegroups .issue-groups p").each(function() {
        temp.push(getIssueGroupById($(this).data("id")));
      });
      currentTabData = temp;
      issueGroupsDataChanged();
    }

    //
    // Issues
    //

    var issuesData;
    var highestIssueId;

    function initIssues() {
      var $tab = $("#tab-issues");
      $tab
        .on("modechanged", onIssuesModeChanged)
        .on("change", onIssuesFilterChanged)
        .on("selectionchanged", ".issues", onIssuesSelectionChanged)
        .on("sortstop", ".issues", onIssuesSortStop)
        .on("selectionchanged", ".issue-questions", issuesQuestionsSelectionChanged)
        .on("sortstop", ".issue-questions", issuesQuestionsSortStop)
        .on("includedchanged", ".included-container", onIssuesIncludedChanged)
        .on("propertychange change click keyup input paste",
          ".issue-name,.issue-group,#issue-disabled",
          onIssuesDataChange)
        .on("click", ".add-issue-button", onClickAddIssue)
        .on("click", ".add-question-to-issue-button", onClickAddQuestionToIssue)
        .on("click", ".remove-question-from-issue-button", onClickRemoveQuestionFromIssue)
        .on("click", ".reorder-issue-button", onClickReorderIssue)
        .on("click", ".reorder-topics-button", onClickReorderTopics)
        .on("click", ".delete-issue-button", onClickDeleteIssue)
        .on("click", ".cancel-button", onClickIssuesCancel)
        .on("click", ".save-button", onClickIssuesSave)
        .on("click", ".order-by", onClickIssuesOrderBy);
    }

    function getIssuesFilter() {
      var $tab = $("#tab-issues");
      return $(".issues-filter", $tab).val();
    }

    function getIssueById(id) {
      var result = null;
      $.each(currentTabData, function() {
        if (this.IssueId === id) {
          result = this;
          return false;
        }
      });
      return result;
    }

    function getIssueQuestion(id) {
      var result = null;
      $.each(issuesData.Questions, function () {
        if (this.QuestionId == id) {
          result = this;
          return false;
        }
      });
      return result;
    }

    function getIssueQuestionCount(id) {
      id = Number(id);
      var result = 0;
      $.each(currentTabData, function() {
        $.each(this.QuestionIds, function() {
          if (Number(this) == id)
            result++;
        });
      });
      return result;
    }

    function getSelectedIssue() {
      return getIssueById(getSelectedIssueId());
    }

    function getSelectedIssueId() {
      return $("#tab-issues .issues p.selected").data("id");
    }

    function getSelectedIssueQuestionId() {
      return $("#tab-issues .issue-questions p.selected").data("id");
    }

    function initializeTabIssues(selectedId) {
      var $tab = $("#tab-issues");
      $tab.addClass("change-mode").removeClass("add-mode").addClass("filter-all");
      $(".disabled", $tab).removeClass("disabled");
      $(".change-button-line .div-button", $tab).addClass("disabled");
      $(".delete-issue-button", $tab).addClass("disabled");
      $(".add-question-to-issue-button", $tab).addClass("disabled");
      $(".remove-question-from-issue-button", $tab).addClass("disabled");
      $(".add-issue-button", $tab).addClass("disabled");
      $(".included-container", $tab).addClass("included");
      $("input", $tab).prop("checked", false);
      $("#show-included-questions", $tab).prop("checked", true);
      $(".drag-box", $tab).html("");
      issuesClearData();
      $(".issues", $tab).addClass("can-select").sortable("option", "disabled", false);
      $(".issue-questions", $tab).addClass("can-select").sortable("option", "disabled", false);
      $(".topics-info", $tab).html("(<span>" + getAnswersMessage() + "</span> in parens</br>" +
        "preceding * =  topic assigned to multiple issues)");
      issuesEnableData(false);
      currentTabData = null;
      currentTabInitalJson = null;
      highestIssueId = 0;
      var answerDates = getAnswerDates();
      util.openAjaxDialog("Loading Issues...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadIssues",

        data: answerDates,

        success: function(result) {
          util.closeAjaxDialog();
          issuesData = result.d;
          currentTabData = issuesData.Issues;
          currentTabInitalJson = JSON.stringify(currentTabData);
          $.each(currentTabData, function() {
            if (this.IssueId > highestIssueId)
              highestIssueId = this.IssueId;
          });
          loadIssuesIssueGroups();
          onIssuesFilterChanged();
          loadIssuesIssues();
          if (selectedId) {
            $("#tab-issues .issues p").each(function() {
              var $this = $(this);
              if ($this.data("id") === selectedId) {
                $this.addClass("selected");
                onIssuesSelectionChanged(null, selectedId);
              }
            });
          }
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load issues"));
        }
      });
    }

    function issuesClearData() {
      var $tab = $("#tab-issues");
      $(".issue-name", $tab).val("");
      $(".issue-group", $tab).val("");
      $("#issue-disabled", $tab).prop("checked", false);
    }

    function issuesDataChanged() {
      setTabChanged($("#tab-issues"));
    }

    function issuesEnableData(enabled) {
      var $tab = $("#tab-issues");
      $(".issue-name", $tab).prop("disabled", !enabled);
      $("#issue-disabled", $tab).prop("disabled", !enabled);
    }

    function loadIssuesIssues(selectedId) {
      var $tab = $("#tab-issues");
      if (!selectedId) selectedId = getSelectedIssueId();
      var issues = [];
      var filter = getIssuesFilter();
      var clone = $.merge([], currentTabData);
      if (filter)
        if ($(".order-by", $tab).data("order") === "alpha") {
          clone.sort(function(a, b) {
            var p1 = a.Issue.toLowerCase();
            var p2 = b.Issue.toLowerCase();
            if (p1 > p2) return 1;
            if (p1 < p2) return -1;
            return 0;
          });
        } else {
          clone.sort(function (a, b) {
            if (a.Answers > b.Answers) return -1;
            if (a.Answers < b.Answers) return 1;
            var p1 = a.Issue.toLowerCase();
            var p2 = b.Issue.toLowerCase();
            if (p1 > p2) return 1;
            if (p1 < p2) return -1;
            return 0;
          });
        }
      $.each(clone, function() {
        if (filter && this.IssueGroupId != filter) return;
        var classes = [];
        if (selectedId === this.IssueId) classes.push("selected");
        if (!this.IsEnabled) classes.push("omitted");
        issues.push('<p' +
          (classes.length ? ' class="' + classes.join(" ") + '"' : '') +
          ' data-id="' +
          this.IssueId +
          '">' +
          this.Issue +
          ' (' +
          //(filter ? this.Answers : this.QuestionIds.length) +
          this.QuestionIds.length + "/" + this.Answers +
          ')</p>');
      });
      $(".issues", $tab).html(issues.join(""));
      loadIssueQuestions();
    }

    function loadIssuesIssueGroups() {
      // load the issue groups into the filter dropdown and the add mode dropdown
      var $tab = $("#tab-issues");
      var options = [];
      $.each(issuesData.IssueGroups, function() {
        options.push('<option value="' +
          this.IssueGroupId +
          '">' +
          this.Heading +
          '</option>');
      });
      var html = options.join("");
      $(".issue-group", $tab).html(html);
      html = '<option value="">All Groups</option>' + html;
      $(".issues-filter", $tab).html(html).val(3);
    }

    function onClickReorderTopics() {
      var issue = getSelectedIssue();
      if (issue != null) {
        issue.QuestionIds.sort(function(a, b) {
          var qa = getIssueQuestion(a).Answers;
          var qb = getIssueQuestion(b).Answers;
          if (qa > qb) return -1;
          if (qa < qb) return 1;
          return 0;
        });
      }
      loadIssueQuestions();
      issuesDataChanged();
    }

    function loadIssueQuestions() {
      var $tab = $("#tab-issues");
      $(".add-question-to-issue-button", $tab).addClass("disabled");
      $(".remove-question-from-issue-button", $tab).addClass("disabled");
      var $questions = $("#tab-issues .issue-questions");
      $questions.html("");
      var issue = getSelectedIssue();
      var included = getIncludedExcluded($("#tab-issues"));
      if (issue != null) {
        var questions = [];
        if (included === "I") {
          $.each(issue.QuestionIds, function() {
            var questionCount = getIssueQuestionCount(this);
            var question = getIssueQuestion(this);
            questions.push('<p' +
              (question.IsEnabled ? '' : ' class="omitted"') +
              ' data-id="' +
              question.QuestionId +
              '">' +
              question.Question +
              //(questionCount > 1 ? ' (*)' : '') +
              " (" + (questionCount > 1 ? '*/' : '') + question.Answers + ")" +
              '</p>');
          });
        } else {
          $.each(issuesData.Questions, function() {
            if ($.inArray(this.QuestionId, issue.QuestionIds) < 0) {
              questions.push('<p' +
                (this.IsEnabled ? '' : ' class="omitted"') +
                ' data-id="' +
                this.QuestionId +
                '">' +
                this.Question +
                '</p>');
            }
          });
        }
        $questions.html(questions.join(""));
      }
    }

    function onClickAddIssue() {
      if ($(this).hasClass("disabled")) return;
      var $tab = $("#tab-issues");
      var id = ++highestIssueId;
      currentTabData.push(
        {
          IssueId: id,
          IssueGroupId: parseInt($(".issue-group", $tab).val()),
          IsEnabled: !$("#issue-disabled", $tab).prop("checked"),
          Issue: $(".issue-name", $tab).val(),
          QuestionIds: []
        });
      loadIssuesIssues(id);
      onIssuesSelectionChanged(null, id);
      issuesClearData();
      onIssuesDataChange();
    }

    function onClickAddQuestionToIssue() {
      if ($(this).hasClass("disabled")) return;
      var questionId = getSelectedIssueQuestionId();
      if (!questionId) return;
      var issue = getSelectedIssue();
      issue.QuestionIds.push(parseInt(questionId));
      loadIssuesIssues();
      onIssuesSelectionChanged(null, getSelectedIssueId()); // to enable properly
      issuesDataChanged();
    }

    function onClickDeleteIssue() {
      if ($(this).hasClass("disabled")) {
        util.alert("Can only delete empty issues");
        return;
      }
      var id = getSelectedIssueId();
      var temp = [];
      $.each(currentTabData, function() {
        if (this.IssueId !== id)
          temp.push(this);
      });
      currentTabData = temp;
      loadIssuesIssues();
      onIssuesSelectionChanged(null);
      onIssuesDataChange();
    }

    function onClickIssuesCancel() {
      if ($(this).hasClass("disabled")) return;
      initializeTabIssues();
    }

    function onClickIssuesOrderBy() {
      var $this = $(this);
      var order = $(this).data("order");
      var isAlpha = order === "alpha";
      $this.data("order", isAlpha ? "count" : "alpha");
      $this.text(isAlpha ? "ordered by answer count" : "ordered alphabetically");
      loadIssuesIssues();
    }

    function onClickIssuesSave() {
      if ($(this).hasClass("disabled")) return;

      // make sure all issues have non-blank descriptions
      var issueMissingDesc = null;
      $.each(currentTabData, function () {
        if (!$.trim(this.Issue)) {
          issueMissingDesc = this;
          return false;
        }
      });
      if (issueMissingDesc) {
        var $tab = $("#tab-issues");
        // set filter to all groups
        if (getIssuesFilter()) {
          $(".issues-filter", $tab).val("");
          onIssuesFilterChanged();
        }
        // select the bad issue entry
        var $issuesBox = $(".issues", $tab);
        $("p", $issuesBox).each(function() {
          var $entry = $(this);
          if ($entry.data("id") == issueMissingDesc.IssueId) {
            if (!$entry.hasClass("selected"))
              $entry.trigger("click");
            return false;
          }
        });
        util.alert("Issue Name is required");
        return;
      }

      var selectedId = getSelectedIssueId();
      util.openAjaxDialog("Saving changes...");
      util.ajax({
        url: "/Admin/WebService.asmx/SaveIssues",
        data: { data: currentTabData },

        success: function () {
          util.closeAjaxDialog();
          initializeTabIssues(selectedId);
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not save changes"));
        }
      });
    }

    function onClickRemoveQuestionFromIssue() {
      if ($(this).hasClass("disabled")) return;
      var selectedQuestionId = getSelectedIssueQuestionId();
      if (!selectedQuestionId) return;
      var issue = getSelectedIssue();
      var temp = [];
      $.each(issue.QuestionIds, function() {
        var questionId = Number(this);
        if (questionId != selectedQuestionId)
          temp.push(questionId);
      });
      issue.QuestionIds = temp;
      loadIssuesIssues();
      onIssuesSelectionChanged(null, getSelectedIssueId()); // to enable properly
      issuesDataChanged();
    }

    function onClickReorderIssue() {
      if ($(this).hasClass("disabled")) {
        return;
      }
      currentTabData.sort(function(a, b) {
        if (a.Answers > b.Answers) return -1;
        if (a.Answers < b.Answers) return 1;
        return 0;
      });
      loadIssuesIssues();
      onIssuesSelectionChanged(null);
      onIssuesDataChange();
    }

    function onIssuesDataChange() {
      var $tab = $("#tab-issues");
      if ($tab.hasClass("change-mode")) {
        var issue = getSelectedIssue();
        if (issue) {
          issue.Issue = $(".issue-name", $tab).val();
          issue.IsEnabled = !$("#issue-disabled", $tab).prop("checked");
          loadIssuesIssues();
        }
      } else {
        $(".add-issue-button", $tab)
          .toggleClass("disabled",
            !$(".issue-name", $tab).val() || !$(".issue-group", $tab).val());
      }
      issuesDataChanged();
    }

    function onIssuesFilterChanged() {
      var $tab = $("#tab-issues");
      var filter = getIssuesFilter();
      $tab.toggleClass("filter-all", !filter);
      $(".order-by", $tab).toggleClass("hidden", !filter);
      $(".issues", $tab).sortable("option", "disabled", !!filter);
      $(".issues-info", $tab).html("(# of topics / <span>" + getAnswersMessage() + "</span> in parens)");
      loadIssuesIssues();
    }

    function onIssuesIncludedChanged(event, included) {
      var $questions = $("#tab-issues .issue-questions");
      $questions.sortable("option", "disabled", !included);
      loadIssueQuestions();
    }

    function onIssuesModeChanged(event, toChangeMode) {
      var $tab = $("#tab-issues");
      $(".issues", $tab)
        .toggleClass("can-select", toChangeMode)
        .toggleClass("disabled", !toChangeMode)
        .sortable("option", "disabled", !toChangeMode || getIssuesFilter());
      var issue = toChangeMode ? getSelectedIssue() : null;
      $(".add-issue-button").addClass("disabled");
      if (issue === null) {
        issuesClearData();
      } else {
        // ReSharper disable once QualifiedExpressionMaybeNull
        $(".issue-name", $tab).val(issue.Issue);
        $("#issue-disabled", $tab).prop("checked", !issue.IsEnabled);
      }
      issuesEnableData(!toChangeMode || issue);
    }

    function issuesQuestionsSelectionChanged(event, data) {
      if (getIncludedExcluded($("#tab-issues")) === "I") {
        $(".remove-question-from-issue-button", $("#tab-issues"))
          .toggleClass("disabled",
            data < 0 || getIssueQuestionCount(getSelectedIssueQuestionId()) <= 1);
      } else {
        $(".add-question-to-issue-button", $("#tab-issues"))
          .toggleClass("disabled", data < 0);
      }
    }

    function issuesQuestionsSortStop() {
      var temp = [];
      var issue = getSelectedIssue();
      $("#tab-issues .issue-questions p").each(function() {
        temp.push(parseInt($(this).data("id")));
      });
      issue.QuestionIds = temp;
      issuesDataChanged();
    }

    function onIssuesSelectionChanged(dummy, id) {
      var $tab = $("#tab-issues");
      issuesClearData();
      $(".delete-issue-button", $tab).addClass("disabled");
      $.each(currentTabData, function() {
        if (this.IssueId === id) {
          $(".issue-name", $tab).val(this.Issue);
          $("#issue-disabled", $tab).prop("checked", !this.IsEnabled);
          if (this.QuestionIds.length === 0)
            $(".delete-issue-button", $tab).removeClass("disabled");
          return false;
        }
      });
      loadIssueQuestions();
      issuesEnableData(getSelectedIssueId() >= 0);
    }

    function onIssuesSortStop() {
      var temp = [];
      $("#tab-issues .issues p").each(function() {
        temp.push(getIssueById($(this).data("id")));
      });
      currentTabData = temp;
      issuesDataChanged();
    }

    //
    // T O P I C S (set up jurisdictions)
    //

    //var questionsData;
    var highestQuestionId;
    var issuesDictionary;
    var topicForAdd/* = initTopicForAdd()*/;
    var jc; // jurisdictionsControl

    function initTopics() {
      var $tab = $("#tab-questions");
      jc = new selectJurisdictions.SelectJurisdictions(
        $(".select-jurisdictions-control"),
        { onChange: onChangeJurisdiction });
      jc.init();
      $tab
        .on("modechanged", onTopicsModeChanged)
        .on("change", onTopicsFilterChanged)
        .on("selectionchanged", ".topics", onTopicsSelectionChanged)
        .on("propertychange change click keyup input paste",
          ".topic-name,.issue,#topic-disabled,.jbutton",
          onTopicsDataChange)
        .on("click", ".jurisdiction-selection-button", onClickJurisdictionSelection)
        .on("click", ".add-topic-button", onClickAddTopic)
        .on("click", ".delete-topic-button", onClickDeleteTopic)
        .on("click", ".cancel-button", onClickTopicsCancel)
        .on("click", ".save-button", onClickTopicsSave);
      $(".jurisdictions-ok-button").on("click", onJurisdictionsOk);
      $(".answer-dates-set-button").on("click", onClickAnswerDatesSet);
      $(".answer-date-item input").on("propertychange change click keyup input paste",
        function() {
          $("#show-between-dates").prop("checked", true);
        });

      $("#jurisdictions-dialog").dialog({
        autoOpen: false,
        dialogClass: 'jurisdictions-dialog',
        modal: true,
        resizable: false,
        title: "Select Jurisdictions"
      });

      $("#answer-dates-dialog").dialog({
        autoOpen: false,
        dialogClass: 'answer-dates-dialog',
        modal: true,
        resizable: false,
        width: 565,
        title: "Set Answer Dates"
      });
    }

    function enableModifySelectedJurisdictionButtons() {
      var $tab = $("#tab-questions");
      $(".state-selection-button").toggleClass("disabled",
        !$("#state-radios-selected", $tab).prop("checked"));
      $(".county-selection-button").toggleClass("disabled",
        !$("#county-radios-selected", $tab).prop("checked"));
      $(".local-selection-button").toggleClass("disabled",
        !$("#local-radios-selected", $tab).prop("checked"));
    }

    function getAnswerDates() {

      function parseDate(date) {
        var components = date.split("/");
        if (components.length !== 3) return null;
        return new Date(components[2], components[0] - 1, components[1]);
      }

      var result = {
        minDate: null,
        maxDate: null
      };
      var dates = ($.cookie("issuedates") || "").split(",");
      if (dates.length === 2) {
        result.minDate = parseDate(dates[0]);
        result.maxDate = parseDate(dates[1]);
      }
      return result;
    }

    function getSelectedJurisdictions(level) {
      var $tab = $("#tab-questions");
      var topic = $tab.hasClass("add-mode") ? topicForAdd : getSelectedTopic();
      var result = { stateCodes: [], countyCodes: [], localKeysOrCodes: [] };
      $.each(topic.Jurisdictions, function() {
        if (this.IssueLevel == level) {
          if (this.StateCode) {
            if (!result.stateCodes.includes(this.StateCode))
              result.stateCodes.push(this.StateCode);
            var code = this.CountyOrLocal ? this.CountyOrLocal : "all";
            if (level === "D")
              result.countyCodes.push(code);
            else if (level === "E") {
              result.localKeysOrCodes.push(code);
            }
          } else {
            result.stateCodes = ["all"];
            return false;
          }
        }
      });
      if (result.localKeysOrCodes.length == 1 &&
        result.localKeysOrCodes[0] == "all" &&
        result.countyCodes.length == 0)
        result.countyCodes.push("all");
      return result;
    }

    function getSelectedTopic() {
      return getTopicById(getSelectedTopicId());
    }

    function getSelectedTopicId() {
      return $("#tab-questions .topics p.selected").data("id");
    }

    function getTopicById(id) {
      var result = null;
      $.each(currentTabData.Questions, function () {
        if (this.QuestionId === id) {
          result = this;
          return false;
        }
      });
      return result;
    }

    function getTopicsFilter() {
      var $tab = $("#tab-questions");
      return $(".topics-filter", $tab).val();
    }

    function initializeTabTopics(selectedId) {
      var $tab = $("#tab-questions");
      $tab.addClass("change-mode").removeClass("add-mode").addClass("filter-all");
      $(".disabled", $tab).removeClass("disabled");
      $(".change-button-line .div-button", $tab).addClass("disabled");
      $(".delete-topic-button", $tab).addClass("disabled");
      $(".add-topic-button", $tab).addClass("disabled");
      $(".drag-box", $tab).html("");
      topicsClearData();
      $(".topics", $tab).addClass("can-select").sortable("option", "disabled", true);
      topicsEnableData(false);
      currentTabData = null;
      currentTabInitalJson = null;
      highestQuestionId = 0;
      var answerDates = getAnswerDates();
      util.openAjaxDialog("Loading Topics...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadTopics",

        data: answerDates,

        success: function (result) {
          util.closeAjaxDialog();
          currentTabData = result.d;
          issuesDictionary = {};
          $.each(currentTabData.Issues, function() {
            issuesDictionary[this.IssueId] = this.QuestionIds;
          });
          currentTabInitalJson = JSON.stringify(currentTabData);
          $.each(currentTabData.Questions, function () {
            if (this.QuestionId > highestQuestionId)
              highestQuestionId = this.QuestionId;
          });
          loadTopicsIssues();
          loadTopicsTopics();
          if (selectedId) {
            $("#tab-questions .topics p").each(function () {
              var $this = $(this);
              if ($this.data("id") === selectedId) {
                $this.addClass("selected");
                onTopicsSelectionChanged(null, selectedId);
              }
            });
          }
          // update the "number of answers" message
          $(".no-of-answers-msg", $tab).html("(<span>" + getAnswersMessage(answerDates) + "</span> in parens)");
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load topics"));
        }
      });
    }

    function getAnswersMessage(answerDates) {
      if (!answerDates) answerDates = getAnswerDates();
      var msg;
      if (answerDates.minDate) {
        if (answerDates.maxDate) {
          msg = "# of answers between " + moment(answerDates.minDate).format("M/D/YYYY") +
            " and " + moment(answerDates.maxDate).format("M/D/YYYY");
        } else {
          msg = "# of answers after " + moment(answerDates.minDate).format("M/D/YYYY");
        }
      } else if (answerDates.maxDate) {
        msg = "# of answers before " + moment(answerDates.maxDate).format("M/D/YYYY");
      } else {
        msg = "total # of answers";
      }
      return msg;
    }

    function loadTopicsIssues() {
      // load the issues into the filter dropdown and the add mode dropdown
      var $tab = $("#tab-questions");
      var options = [];
      var clone = $.merge([], currentTabData.Issues);
      clone.sort(function (a, b) {
        if (a.IssueId < 1000)
          if (b.IssueId < 1000) {
            if (a.IssueId < b.IssueId) return -1;
            if (a.IssueId > b.IssueId) return 1;
            return 0;
          } else {
            return -1;
          }
        else if (b.IssueId < 1000) {
          return 1;
        } else {
          var p1 = a.Issue.toLowerCase();
          var p2 = b.Issue.toLowerCase();
          if (p1 > p2) return 1;
          if (p1 < p2) return -1;
          return 0;
        }
      });
      $.each(clone, function () {
        options.push('<option value="' +
          this.IssueId +
          '">' +
          this.Issue +
          '</option>');
      });
      var html = options.join("");
      $(".issue", $tab).html('<option value="">&lt;Select an issue for the new topic&gt;</option>' + html);
      $(".topics-filter", $tab).html('<option value="">All Issues</option>' + html);
    }

    function loadTopicsTopics(selectedId) {
      var $tab = $("#tab-questions");
      if (!selectedId) selectedId = getSelectedTopicId();
      var topics = [];
      var filter = getTopicsFilter();
      var questionIds = issuesDictionary[filter];
      $.each(currentTabData.Questions, function () {
        if (filter && !questionIds.includes(this.QuestionId)) return;
        var classes = [];
        if (selectedId === this.QuestionId) classes.push("selected");
        if (!this.IsEnabled) classes.push("omitted");
        topics.push('<p' +
          (classes.length ? ' class="' + classes.join(" ") + '"' : '') +
          ' data-id="' +
          this.QuestionId +
          '">' +
          this.Question +
          ' (' +
          this.Answers +
          ')</p>');
      });
      $(".topics", $tab).html(topics.join(""));
    }

    function onChangeJurisdiction() {
      // if we are in locals mode and have a single county, we need to open the locals
      setTimeout(function () {
        // this needs to be deferred so the locals-list test works
        var $dialog = $("#jurisdictions-dialog");
        if ($dialog.data("issue-level") === "E" && jc.getCategoryCodes("counties").length === 1 &&
          !$(".locals-list", $dialog).html()) {
          $(".jurisdiction.locals .get-list-button", $dialog).trigger("click");
        }
      }, 10);
    }

    function onClickAddTopic() {
      if ($(this).hasClass("disabled")) return;
      var $tab = $("#tab-questions");
      var id = ++highestQuestionId;
      topicForAdd.QuestionId = id;
      topicForAdd.IsEnabled = !$("#topic-disabled", $tab).prop("checked");
      topicForAdd.Question = $(".topic-name", $tab).val();
      topicForAdd.Answers = 0;
      currentTabData.Questions.push(topicForAdd);
      var issueId = $(".issue", $tab).val();
      $.each(currentTabData.Issues, function() {
        if (this.IssueId == issueId) {
          this.QuestionIds.push(id);
        }
      });
      loadTopicsTopics(id);
      onTopicsSelectionChanged(null, id);
      topicsClearData();
      initTopicForAdd();
      $("#tab-questions #national-candidates").prop("checked", true);
      onTopicsDataChange();
    }

    function onClickAnswerDates() {
      var $dialog = $("#answer-dates-dialog");
      var $tab = $(this).closest(".main-tab");
      $dialog.data("tab", $tab);
      var dates = getAnswerDates();
      $(".answer-date-from", $dialog).val(dates.minDate
        ? moment(dates.minDate).format("M/D/YYYY") : "");
      $(".answer-date-to", $dialog).val(dates.maxDate
        ? moment(dates.maxDate).format("M/D/YYYY") : "");
      var isAll = !dates.minDate && !dates.maxDate;
      $("#show-all-answers", $dialog).prop("checked", isAll);
      $("#show-between-dates", $dialog).prop("checked", !isAll);
      $dialog.dialog("open");
    }

    function onClickAnswerDatesSet() {
      var formats = ["M/D/YYYY", "M/DD/YYYY", "MM/D/YYYY", "MM/DD/YYYY"];
      var $dialog = $("#answer-dates-dialog");
      var fromInput = $.trim($(".answer-date-from", $dialog).val());
      var toInput = $.trim($(".answer-date-to", $dialog).val());
      var fromDate;
      var toDate;
      var fromString = "";
      var toString = "";
      if (!$("#show-all-answers", $dialog).prop("checked")) {
        if (fromInput) {
          fromDate = moment(fromInput, formats, true);
          if (!fromDate.isValid()) {
            util.alert("From date is not valid");
            return;
          }
          fromString = fromDate.format("M/D/YYYY");
        }
        if (toInput) {
          toDate = moment($(".answer-date-to", $dialog).val(), formats, true);
          if (!toDate.isValid()) {
            util.alert("To date is not valid");
            return;
          }
          toString = toDate.format("M/D/YYYY");
        }
        // ReSharper disable UsageOfPossiblyUnassignedValue
        if (fromDate && toDate && fromDate.isAfter(toDate)) {
          // ReSharper restore UsageOfPossiblyUnassignedValue
          util.alert("The From date is after the To date");
          return;
        }
      }


      $.cookie('issuedates', fromString + "," + toString,
        { expires: 1000 });
      var $tab = $dialog.data("tab");
      switch ($tab.attr("id")) {
        case "tab-issues":
          initializeTabIssues();
          break;
        case "tab-questions":
          initializeTabTopics();
          break;
      }
      $dialog.dialog("close");
    }

    function onClickDeleteTopic() {
      if ($(this).hasClass("disabled")) {
        return;
      }
      var id = getSelectedTopicId();
      var temp = [];
      $.each(currentTabData.Questions, function () {
        if (this.QuestionId !== id)
          temp.push(this);
      });
      currentTabData.Questions = temp;
      loadTopicsTopics();
      onTopicsSelectionChanged(null);
      onTopicsDataChange();
    }

    function onClickJurisdictionSelection() {

      function getCountyRestoreInfo(selected) {
        if (selected.stateCodes.length != 1 || selected.stateCodes[0] == "all") {
          complete(selected, { Counties: [], Locals: [] });
          return;
        }
        util.openAjaxDialog("Getting county restore info...");
        util.ajax({
          url: "/Admin/WebService.asmx/GetRestoreInfo",
          data: {
            stateCode: selected.stateCodes[0],
            countyCode: "",
            localKey: "",
            districtFiltering: false,
            getCounties: true,
            getLocals: false,
            getParties: false,
            getElections: false,
            getBestCounty: null
          },

          success: function (result) {
            util.closeAjaxDialog();
            complete(selected, result.d);
          },

          error: function (result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result,
              "Could not get the county restore info"));
          }
        });
      }

      function getLocalRestoreInfo(selected) {
        if (selected.stateCodes.length != 1 || selected.stateCodes[0] == "all") {
          complete(selected, { Counties: [], Locals: [] });
          return;
        }
        var needBestCounty = selected.localKeysOrCodes[0] != "all";
        util.openAjaxDialog("Getting local restore info...");
        util.ajax({
          url: "/Admin/WebService.asmx/GetRestoreInfo",
          data: {
            stateCode: selected.stateCodes[0],
            countyCode: "",
            localKey: "",
            districtFiltering: false,
            getCounties: true,
            getLocals: needBestCounty,
            getParties: false,
            getElections: false,
            getBestCounty: needBestCounty ? selected.localKeysOrCodes : null
          },

          success: function (result) {
            util.closeAjaxDialog();
            if (needBestCounty)
              selected.countyCodes = [result.d.CountyCode];
            complete(selected, result.d);
          },

          error: function (result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result,
              "Could not get the local restore info"));
          }
        });
      }

      function complete(selected, restoreInfo) {
        jc.restore(selected, restoreInfo);
        $("#jurisdictions-dialog").dialog("open");
        //setTimeout(function () { jc.restore(selected, restoreInfo);}, 1000);
      }

      var $button = $(this);
      var $dialog = $("#jurisdictions-dialog");
      if ($button.hasClass("disabled")) return;
      if ($button.hasClass("state-selection-button")) {
        jc.toggleLevel("locals", false);
        jc.toggleLevel("counties", false);
        $dialog.data("issue-level", "C");
        $dialog.dialog("option", "width", 250);
        jc.restore({ stateCodes: getSelectedJurisdictions("C").stateCodes });
        $("#jurisdictions-dialog").dialog("open");
      } else if ($button.hasClass("county-selection-button")) {
        jc.toggleLevel("locals", false);
        jc.toggleLevel("counties", true);
        $dialog.data("issue-level", "D");
        $dialog.dialog("option", "width", 494);
        getCountyRestoreInfo(getSelectedJurisdictions("D"));
      } else if ($button.hasClass("local-selection-button")) {
        jc.toggleLevel("counties", true);
        jc.toggleLevel("locals", true);
        $dialog.data("issue-level", "E");
        $dialog.dialog("option", "width", 734);
        getLocalRestoreInfo(getSelectedJurisdictions("E"));
      }
    }

    function onClickTopicsCancel() {
      if ($(this).hasClass("disabled")) return;
      initializeTabTopics();
    }

    function onClickTopicsSave() {
      if ($(this).hasClass("disabled")) return;

      // make sure all topics have non-blank descriptions
      var topicMissingDesc;
      $.each(currentTabData.Questions, function () {
        if (!$.trim(this.Question)) {
          topicMissingDesc = this;
          return false;
        }
      });
      if (topicMissingDesc) {
        var $tab = $("#tab-questions");
        // set filter to all issues
        if (getTopicsFilter()) {
          $(".topics-filter", $tab).val("");
          onTopicsFilterChanged();
        }
        // select the bad issue entry
        var $topicsBox = $(".topics", $tab);
        $("p", $topicsBox).each(function () {
          var $entry = $(this);
          if ($entry.data("id") == topicMissingDesc.QuestionId) {
            if (!$entry.hasClass("selected"))
              $entry.trigger("click");
            return false;
          }
        });
        util.alert("Topic Name is required");
        return;
      }

      var selectedId = getSelectedTopicId();
      util.openAjaxDialog("Saving changes...");
      util.ajax({
        url: "/Admin/WebService.asmx/SaveTopics",
        data: { data: currentTabData },

        success: function () {
          util.closeAjaxDialog();
          initializeTabTopics(selectedId);
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not save changes"));
        }
      });
    }

    function onJurisdictionsOk() {
      var $tab = $("#tab-questions");
      var $dialog = $("#jurisdictions-dialog");
      var issueLevel = $dialog.data("issue-level");
      var topic = $tab.hasClass("add-mode") ? topicForAdd : getSelectedTopic();
      var j = [];
      $.each(topic.Jurisdictions, function () {
        if (this.IssueLevel != issueLevel)
          j.push(this);
      });
      var states = jc.getCategoryCodes("states", true);

      switch (issueLevel) {
        case "C":
          if (states.length == 0) {
            $("#state-radios-all", $tab).prop("checked", false);
            $("#state-radios-selected", $tab).prop("checked", false);
            $("#state-candidates", $tab).prop("checked", false);
          }
          else if (states.length === 1 && states[0] === "all") {
            j.push({ IssueLevel: "C", StateCode: "", CountyOrLocal: "" });
            $("#state-radios-all", $tab).prop("checked", true);
            $("#state-radios-selected", $tab).prop("checked", false);
          }
          else {
            $.each(states, function () {
              j.push({ IssueLevel: "C", StateCode: this.toString(), CountyOrLocal: "" });
            });
          }
          break;

      case "D":
        var counties = jc.getCategoryCodes("counties", true);
        if (states.length == 0 || counties.length == 0) {
          $("#county-radios-all", $tab).prop("checked", false);
          $("#county-radios-selected", $tab).prop("checked", false);
          $("#county-candidates", $tab).prop("checked", false);
        } else if (states.length === 1 && states[0] === "all") {
          j.push({ IssueLevel: "D", StateCode: "", CountyOrLocal: "" });
          $("#county-radios-all", $tab).prop("checked", true);
          $("#county-radios-selected", $tab).prop("checked", false);
        } else if (states.length === 1 &&
          (counties.length !== 1 || counties[0] !== "all")) {
          // selected counties
          $.each(counties, function () {
            j.push({ IssueLevel: "D", StateCode: states[0], CountyOrLocal: this.toString() });
          });
        } else {
          $.each(states, function () {
            j.push({ IssueLevel: "D", StateCode: this.toString(), CountyOrLocal: "" });
          });
        }
        break;

      case "E":
        var lcounties = jc.getCategoryCodes("counties", true);
        if (lcounties.length != 1) {
          util.alert("For local candidates select either all counties or a single county.");
          return;
          }
        var locals = jc.getCategoryCodes("locals", false);
        var localsAll = jc.getCategoryCodes("locals", true);
          if (states.length == 0 || localsAll.length == 0) {
            $("#local-radios-all", $tab).prop("checked", false);
            $("#local-radios-selected", $tab).prop("checked", false);
            $("#local-candidates", $tab).prop("checked", false);
          } else if (states.length === 1 && states[0] === "all") {
            j.push({ IssueLevel: "E", StateCode: "", CountyOrLocal: "" });
            $("#local-radios-all", $tab).prop("checked", true);
            $("#local-radios-selected", $tab).prop("checked", false);
          } else if (states.length === 1 && lcounties[0] === "all") {
            j.push({ IssueLevel: "E", StateCode: states[0], CountyOrLocal: "" });
          } else if (states.length === 1) {
            // selected locals in a county
            $.each(locals, function () {
              j.push({ IssueLevel: "E", StateCode: states[0], CountyOrLocal: this.toString() });
            });
          } else {
            $.each(states, function () {
              j.push({ IssueLevel: "E", StateCode: this.toString(), CountyOrLocal: "" });
            });
          }
        break;
      }

      j.sort(function(a, b) {
        if (a.IssueLevel < b.IssueLevel) return -1;
        if (a.IssueLevel > b.IssueLevel) return 1;
        if (a.StateCode < b.StateCode) return -1;
        if (a.StateCode > b.StateCode) return 1;
        if (a.CountyOrLocal < b.CountyOrLocal) return -1;
        if (a.CountyOrLocal > b.CountyOrLocal) return 1;
        return 0;
      });
      topic.Jurisdictions = j;
      onTopicsDataChange();
      $dialog.dialog("close");
    }

    function onTopicsDataChange(event) {
      var $tab = $("#tab-questions");
      if ($tab.hasClass("change-mode")) {
        var topic = getSelectedTopic();
        if (topic) {
          topic.Question = $(".topic-name", $tab).val();
          topic.IsEnabled = !$("#topic-disabled", $tab).prop("checked");
          updateTopicJurisdictionsFromControls(event, topic);
          loadTopicsTopics();
          enableModifySelectedJurisdictionButtons();
        }
      } else {
        updateTopicJurisdictionsFromControls(event, topicForAdd);
        enableModifySelectedJurisdictionButtons();
        $(".add-topic-button", $tab)
          .toggleClass("disabled",
            !$(".topic-name", $tab).val() || !$(".issue", $tab).val());
      }
      topicsDataChanged();
    }

    function onTopicsFilterChanged() {
      loadTopicsTopics();
    }

    function initTopicForAdd() {
      topicForAdd = {
        QuestionId: 0,
        IsEnabled: true,
        Question: "",
        Answers: 0,
        Jurisdictions: [{
          IssueLevel: "B",
          StateCode: "",
          CountyOrLocal: ""
        }]
      };
    }

    function onTopicsModeChanged(event, toChangeMode) {
      var $tab = $("#tab-questions");
      $(".topics", $tab)
        .toggleClass("can-select", toChangeMode)
        .toggleClass("disabled", !toChangeMode);
      var topic = toChangeMode ? getSelectedTopic() : null;
      $(".add-topic-button").addClass("disabled");
      topicsClearData();
      if (topic === null) { // to add mode
        initTopicForAdd();
        $("#tab-questions #national-candidates").prop("checked", true);
        $(".issue", $tab).val($(".topics-filter", $tab).val());
      } else {
        setTopicData(topic);
      }
      topicsEnableData(!toChangeMode || topic);
    }

    function onTopicsSelectionChanged(dummy, id) {
      var $tab = $("#tab-questions");
      topicsClearData();
      $(".delete-topic-button", $tab).addClass("disabled");
      $.each(currentTabData.Questions, function () {
        if (this.QuestionId === id) {
          setTopicData(this);
          return false;
        }
      });
      topicsEnableData(getSelectedTopicId() >= 0);
    }

    function setTopicData(topic)
    {
      var $tab = $("#tab-questions");
      $(".topic-name", $tab).val(topic.Question);
      $("#topic-disabled", $tab).prop("checked", !topic.IsEnabled);
      $(".delete-topic-button", $tab).removeClass("disabled");
      $.each(topic.Jurisdictions, function () {
        switch (this.IssueLevel) {
        case "A":
          $(".for-all", $tab).prop("checked", true);
          return false;

        case "B":
          $("#national-candidates", $tab).prop("checked", true);
          break;

        case "C":
          $("#state-candidates", $tab).prop("checked", true);
          if (!this.StateCode)
            $("#state-radios-all", $tab).prop("checked", true);
          else
            $("#state-radios-selected", $tab).prop("checked", true);
          break;

        case "D":
          $("#county-candidates", $tab).prop("checked", true);
          if (!this.StateCode)
            $("#county-radios-all", $tab).prop("checked", true);
          else
            $("#county-radios-selected", $tab).prop("checked", true);
          break;

        case "E":
          $("#local-candidates", $tab).prop("checked", true);
          if (!this.StateCode)
            $("#local-radios-all", $tab).prop("checked", true);
          else
            $("#local-radios-selected", $tab).prop("checked", true);
          break;
        }
      });
    }

    function topicsClearData() {
      var $tab = $("#tab-questions");
      $(".topic-name", $tab).val("");
      $("#topic-disabled", $tab).prop("checked", false);
      $("input", $tab).prop("checked", false);
    }

    function topicsDataChanged() {
      setTabChanged($("#tab-questions"));
    }

    function topicsEnableData(enabled) {
      var $tab = $("#tab-questions");
      $("input", $tab).prop("disabled", !enabled);
      $(".jurisdiction-selection-button", $tab).toggleClass("disabled", !enabled);
      enableModifySelectedJurisdictionButtons();
    }

    function updateTopicJurisdictionsFromControls(event, topic) {
      var $tab = $("#tab-questions");
      var oldJurisdictions = topic.Jurisdictions;
      topic.Jurisdictions = []; // clear existing
      var isNational = $("#national-candidates", $tab).prop("checked");

      if (!$("#state-candidates", $tab).prop("checked") &&
        event && event.target.id !== "state-candidates" &&
        ($("#state-radios-all", $tab).prop("checked") ||
        $("#state-radios-selected", $tab).prop("checked")))
        $("#state-candidates", $tab).prop("checked", true);

      if (!$("#county-candidates", $tab).prop("checked") &&
        event && event.target.id !== "county-candidates" &&
        ($("#county-radios-all", $tab).prop("checked") ||
        $("#county-radios-selected", $tab).prop("checked")))
        $("#county-candidates", $tab).prop("checked", true);

      if (!$("#local-candidates", $tab).prop("checked") &&
        event && event.target.id !== "local-candidates" &&
        ($("#local-radios-all", $tab).prop("checked") ||
        $("#local-radios-selected", $tab).prop("checked")))
        $("#local-candidates", $tab).prop("checked", true);

      var isState = $("#state-candidates", $tab).prop("checked");
      if (!isState) {
        $("#state-radios-all", $tab).prop("checked", false);
        $("#state-radios-selected", $tab).prop("checked", false);
      }
      var isSelectedStates = $("#state-radios-selected", $tab).prop("checked");
      if (isState && !isSelectedStates)
        $("#state-radios-all", $tab).prop("checked", true);
      var isAllStates = $("#state-radios-all", $tab).prop("checked");

      var isCounty = $("#county-candidates", $tab).prop("checked");
      if (!isCounty) {
        $("#county-radios-all", $tab).prop("checked", false);
        $("#county-radios-selected", $tab).prop("checked", false);
      }
      var isSelectedCounties = $("#county-radios-selected", $tab).prop("checked");
      if (isCounty && !isSelectedCounties)
        $("#county-radios-all", $tab).prop("checked", true);
      var isAllCounties = $("#county-radios-all", $tab).prop("checked");

      var isLocal = $("#local-candidates", $tab).prop("checked");
      if (!isLocal) {
        $("#local-radios-all", $tab).prop("checked", false);
        $("#local-radios-selected", $tab).prop("checked", false);
      }
      var isSelectedLocals = $("#local-radios-selected", $tab).prop("checked");
      if (isLocal && !isSelectedLocals)
        $("#local-radios-all", $tab).prop("checked", true);
      var isAllLocals = $("#local-radios-all", $tab).prop("checked");

      // check for type "A"
      if (isNational &&
        isState &&
        isAllStates &&
        isCounty &&
        isAllCounties &&
        isLocal &&
        isAllLocals) {
        topic.Jurisdictions.push({
          IssueLevel: "A",
          StateCode: "",
          CountyOrLocal: ""
        });
        return;
      }
      if (isNational) {
        topic.Jurisdictions.push({
          IssueLevel: "B",
          StateCode: "",
          CountyOrLocal: ""
        });
      }
      if (isState) {
        if (isAllStates) {
          topic.Jurisdictions.push({
            IssueLevel: "C",
            StateCode: "",
            CountyOrLocal: ""
          });
        } else {
          // updated via dialog
          $.each(oldJurisdictions, function () {
            if (this.IssueLevel === "C")
              topic.Jurisdictions.push(this);
          });
        }
      }
      if (isCounty) {
        if (isAllCounties) {
          topic.Jurisdictions.push({
            IssueLevel: "D",
            StateCode: "",
            CountyOrLocal: ""
          });
        } else {
          // updated via dialog
          $.each(oldJurisdictions, function () {
            if (this.IssueLevel === "D")
              topic.Jurisdictions.push(this);
          });
        }
      }
      if (isLocal) {
        if (isAllLocals) {
          topic.Jurisdictions.push({
            IssueLevel: "E",
            StateCode: "",
            CountyOrLocal: ""
          });
        } else {
          // updated via dialog
          $.each(oldJurisdictions, function () {
            if (this.IssueLevel === "E")
              topic.Jurisdictions.push(this);
          });
        }
      }
    }

    //
    // Consolidate Topics
    //

    var consolidateToId;
    var consolidateFromId;

    function initConsolidateTopics() {
      var $tab = $("#tab-consolidatetopics");
      $tab
        .on("selectionchanged", ".topics-to,.topics-from",
          onConsolidateTopicsSelectionChanged)
        .on("click", ".consolidate-topics-button", function () { doConsolidateTopics(false) });
    }

    function doConsolidateTopics(force) {
      var toTopic;
      var fromTopic;
      $.each(currentTabData.Questions, function () {
        if (this.QuestionId === consolidateToId) toTopic = this;
        if (this.QuestionId === consolidateFromId) fromTopic = this;
      });
      if (!force) {
        util.confirm("All " +
          fromTopic.Answers +
          " responses from " +
          fromTopic.Question +
          " will be consolidated into " +
          toTopic.Question,
          function(button) {
            if (button === "Ok") doConsolidateTopics(true);
          });
        return;
      }

      util.openAjaxDialog("Consolidating Topics...");
      util.ajax({
        url: "/Admin/WebService.asmx/ConsolidateTopics",
        data: {
          toId: consolidateToId,
          fromId: consolidateFromId
        },

        success: function () {
          util.closeAjaxDialog();
          util.alert("All " +
            fromTopic.Answers +
            " responses from " +
            fromTopic.Question +
            " have been consolidated into " +
            toTopic.Question);
          initializeTabConsolidateTopics();
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load topics"));
        }
      });
    }

    function initializeTabConsolidateTopics() {
      var $tab = $("#tab-consolidatetopics");
      $(".topics-to,.topics-from", $tab).addClass("can-select").sortable("option", "disabled", true);
      util.openAjaxDialog("Loading Topics...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadTopics",

        data: {
          minDate: null,
          maxDate: null
        },

        success: function (result) {
          util.closeAjaxDialog();
          currentTabData = result.d;
          //currentTabData.Questions.sort(function(a, b) {
          //  a = a.Question.toLowerCase();
          //  b = b.Question.toLowerCase();
          //  if (a > b) return 1;
          //  if (a < b) return -1;
          //  return 0;
          //});
          currentTabInitalJson = JSON.stringify(currentTabData);
          loadConsolidationTopics(".topics-to");
          loadConsolidationTopics(".topics-from");
          consolidateToId = null;
          consolidateFromId = null;
          enableConsolidateButton();
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load topics"));
        }
      });
    }

    function loadConsolidationTopics(className) {
      var $tab = $("#tab-consolidatetopics");
      var topics = [];
      $.each(currentTabData.Questions, function () {
        var classes = [];
        if (!this.IsEnabled) classes.push("omitted");
        topics.push('<p' +
          (classes.length ? ' class="' + classes.join(" ") + '"' : '') +
          ' data-id="' +
          this.QuestionId +
          '">' +
          this.Question +
          ' (' +
          this.Answers +
          ')</p>');
      });
      $(className, $tab).html(topics.join(""));
    }

    function onConsolidateTopicsSelectionChanged() {
      var $this = $(this);
      var topicId = $("p.selected", $this).data("id");
      if ($this.hasClass("topics-to")) consolidateToId = topicId;
      else consolidateFromId = topicId;
      enableConsolidateButton();
    }

    function enableConsolidateButton() {
      var $tab = $("#tab-consolidatetopics");
      $(".consolidate-topics-button", $tab)
        .prop("disabled", !consolidateToId || !consolidateFromId || consolidateToId == consolidateFromId);
    }

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });