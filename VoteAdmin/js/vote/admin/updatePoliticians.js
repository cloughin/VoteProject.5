define(["jquery", "vote/adminMaster", "vote/util", "monitor",
  "vote/controls/managePoliticiansPanel", "jqueryui", "slimscroll", "dynatree"],
  function ($, master, util, monitor, managePoliticiansPanel) {

    var $$ = util.$$;

    //
    // Master only
    //

    var masterOnlyStartTab;

    //
    // Monitor callbacks
    //

    var afterUpdateContainer = function (group) {
      if (!group) return;

      switch (group.group) {
        case "mc-group-addcandidates":
          updateContainerAddCandidates();
          break;
      }
    };

    function afterRequest(group) {
      if (!group) return;

      switch (group.group) {
        case "mc-group-addcandidates":
          afterRequestAddCandidates();
          break;
      }
    }

    //
    // Search, Add, Consolidate
    //

    function onClickChangeState() {
      location.href = location.protocol + "//" + location.host + location.pathname + "?state=" + $(this).val();
    }

    function onClickShowPassword() {
      var keys = managePoliticiansPanel.getSelectedCandidateKeys();
      if (keys.length != 1) {
        util.alert("Please select exactly one politician");
        return;
      }
      var key = managePoliticiansPanel.getSelectedCandidateKeys()[0];
      util.openAjaxDialog("Getting password...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetPoliticianPassword",
        data: {
          key: key
        },

        success: function (result) {
          util.closeAjaxDialog();
          util.alert("Id: " + key + "\nPassword: " + result.d, "User ID and Password");
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "Could not get password"));
        }
      });
    }

    function onClickViewEmails() {
      var keys = managePoliticiansPanel.getSelectedCandidateKeys();
      if (keys.length != 1) {
        util.alert("Please select exactly one politician");
        return;
      }
      var key = managePoliticiansPanel.getSelectedCandidateKeys()[0];
      util.openAjaxDialog("Getting emails...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetPoliticianEmails",
        data: {
          key: key
        },

        success: function (result) {
          util.closeAjaxDialog();
          if (result.d.length == 0) {
            util.alert("No emails were found");
          } else {
            var $dialog = $(".view-emails-dialog");
            var $select = $(".select-email", $dialog);
            $select.data("templates", result.d);
            var options = [];
            var inx = 0;
            $.each(result.d, function() {
              options.push('<option value="' +
                inx++ +
                '">' +
                this.TemplateName +
                '</option>');
            });
            $select.html(options.join(""));
            onSelectEmail();
            $('#view-emails-dialog').dialog('open');
          }
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "Could not get emails"));
        }
      });
    }

    function onEditIssues(event) {
      var $target = $(event.target);
      var keys = managePoliticiansPanel.getSelectedCandidateKeys();
      if ($target.hasClass("disabled") || keys.length != 1) {
        event.preventDefault();
        return;
      }
      $target.attr("href", "/politician/UpdateIssues.aspx?id=" + managePoliticiansPanel.getSelectedCandidateKeys()[0]);
    }

    function onEditPolitician(event) {
      var $target = $(event.target);
      var keys = managePoliticiansPanel.getSelectedCandidateKeys();
      if ($target.hasClass("disabled") || keys.length != 1) {
        event.preventDefault();
        return;
      }
      $target.attr("href", "/politician/UpdateIntro.aspx?id=" + managePoliticiansPanel.getSelectedCandidateKeys()[0]);
    }

    function onDeletePolitician() {
      var keys = managePoliticiansPanel.getSelectedCandidateKeys();
      if (keys.length != 1) {
        util.alert("Please select exactly one politician");
        return;
      }
      var key = managePoliticiansPanel.getSelectedCandidateKeys()[0];
      $("#tab-addpolitician .reloading").val("deleting");
      $("#tab-addpolitician .key-to-delete").val(key);
      var searchString = $("#tab-addpolitician .find-politician-control input").val();
      $("#tab-addpolitician .search-to-restore").val(searchString);
      $("#tab-addpolitician input.update-button").click();
    }

    function afterRequestAddCandidates() {
      var searchToRestore = $("#tab-addpolitician .search-to-restore").val();
      if (searchToRestore) {
        $("#tab-addpolitician .search-to-restore").val("");
        $("#tab-addpolitician .find-politician-control input").val(searchToRestore);
        var key = $("#tab-addpolitician .key-to-delete").val();
        var keys = [];
        if (key) keys.push(key);
        managePoliticiansPanel.searchNameChanged(keys);
      }
    }

    function updateContainerAddCandidates() {
      $("#tab-addpolitician input.kalypto").kalypto({ toggleClass: "kalypto-checkbox" });
    }

    //
    // View Emails Dialog
    //

    function initViewEmailsDialog() {
      $('#view-emails-dialog').dialog({
        autoOpen: false,
        width: 500,
        title: "View Candidate Credential Emails",
        resizable: false,
        dialogClass: 'view-emails-dialog overlay-dialog',
        modal: true
      });
      $(".view-emails-dialog")
        .on("click", ".open-email-button", openEmailButton)
        .on("change", ".select-email", onSelectEmail);
    }

    function onSelectEmail()
    {
      var $dialog = $(".view-emails-dialog");
      var $select = $(".select-email", $dialog);
      var template = $select.data("templates")[$select.val()];
      $(".email-subject", $dialog).text(template.Subject);
      $(".email-body", $dialog).html(template.Body);
    }

    function openEmailButton() {
      var $dialog = $(".view-emails-dialog");
      var $select = $(".select-email", $dialog);
      var template = $select.data("templates")[$select.val()];
      window.location.href = "mailto:" +
        template.Email +
        "?subject=" +
        template.SubjectForEmail +
        "&body=" +
        template.BodyForEmail;
    }

    //
    // Misc
    //

    function initPage() {
      monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);
      monitor.registerCallback("afterRequest", afterRequest);
      monitor.init();

      window.onbeforeunload = function() {
        var changedGroups = monitor.getChangedGroupNames(true);
        if (changedGroups.length > 0)
          return "There are entries on your form that have not been submitted";
      };

      $$('main-tabs')
        .safeBind("tabsbeforeactivate", onTabsBeforeActivate)
        .safeBind("tabsactivate", onTabsActivate)
        .on("click", ".show-password-button", onClickShowPassword)
        .on("click", ".view-emails-button", onClickViewEmails)
        .on("click", ".edit-politician-button", onEditPolitician)
        .on("click", ".edit-issues-button", onEditIssues)
        .on("click", ".delete-politician-button", onDeletePolitician)
        .on("change", ".change-state", onClickChangeState);

      managePoliticiansPanel.initControl({ onSelectionChanged: onFindPoliticianSelectionChanged});

      var tabInfo = master.parseFragment("tab-addpolitician", function(info) {
        if (info.tab === "tab-addpolitician") {
          reloadPanel("tab-addpolitician", "reloading");
          return false;
        }
      });
      if (tabInfo.tab === "tab-masteronly")
        masterOnlyStartTab = tabInfo.subTab;

      initViewEmailsDialog();

    }

    function onFindPoliticianSelectionChanged() {
      var singlePolitician = managePoliticiansPanel.getSelectedCandidateKeys().length == 1;
      $(".show-password-button").prop("disabled", !singlePolitician);
      $(".view-emails-button").prop("disabled", !singlePolitician);
      $(".edit-politician-button,.edit-issues-button").toggleClass("disabled", !singlePolitician);
      $(".delete-politician-button").prop("disabled", !singlePolitician);
    }

    var onTabsActivate = function(event, ui) {
      var newPanelId = ui.newPanel[0].id;
      switch (newPanelId) {
      default:
        reloadPanel(newPanelId);
        break;
      }
    };

    var onTabsBeforeActivate = function (event, ui) {
      return monitor.tabsBeforeActivate(event, ui, "main-tabs");
    };

    var reloadPanel = function (id, option) {
      if (!option) option = "reloading";
      switch (id) {
        default:
          $$(id + ' .reloading').val(option);
          $$(id + ' input.update-button').addClass("reloading").click();
          break;
      }
    };

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });