define([
    "jquery", "vote/adminMaster", "vote/util", "monitor", "store",
    "vote/controls/navigateJurisdiction",
    "jqueryui", "slimscroll", "dynatree", "resizablecolumns"
  ],
  function ($, master, util, monitor, store, navigateJurisdiction) {

    var $$ = util.$$;

    //
    // Monitor callbacks
    //

    var afterUpdateContainer = function (group) {
      if (!group) return;

      switch (group.group) {
        case "mc-group-generalvoterinfo":
          util.setResizableVertical($("#tab-generalvoterinfo textarea.is-resizable"));
          break;

        case "mc-group-ballot":
          master.initDisclaimerButtons($("#tab-ballot"));
          break;

        case "mc-group-electionauthority":
          afterUpdateContainerElectionAuthority();
          break;

        case "mc-group-masteronly":
          afterUpdateContainerMasterOnly();
          break;
      }
    };

    //
    // Election Authority
    //

    var afterUpdateContainerElectionAuthority = function () {
      util.safeBind($("#tab-electionauthority .swap-button"), "click", onClickSwapContacts);
      util.safeBind($("#tab-electionauthority .move-to-notes-button"), "click", onClickMoveNotes);
    };

    var onClickSwapContacts = function () {
      var mainName = $("#tab-electionauthority .contact input[type=text]").val();
      var mainTitle = $("#tab-electionauthority .contacttitle input[type=text]").val();
      var mainPhone = $("#tab-electionauthority .contactphone input[type=text]").val();
      var mainEmail = $("#tab-electionauthority .contactemail input[type=text]").val();
      $("#tab-electionauthority .contact input[type=text]").val($("#tab-electionauthority .altcontact input[type=text]").val()).change();
      $("#tab-electionauthority .contacttitle input[type=text]").val($("#tab-electionauthority .altcontacttitle input[type=text]").val()).change();
      $("#tab-electionauthority .contactphone input[type=text]").val($("#tab-electionauthority .altcontactphone input[type=text]").val()).change();
      $("#tab-electionauthority .contactemail input[type=text]").val($("#tab-electionauthority .altcontactemail input[type=text]").val()).change();
      $("#tab-electionauthority .altcontact input[type=text]").val(mainName).change();
      $("#tab-electionauthority .altcontacttitle input[type=text]").val(mainTitle).change();
      $("#tab-electionauthority .altcontactphone input[type=text]").val(mainPhone).change();
      $("#tab-electionauthority .altcontactemail input[type=text]").val(mainEmail).change();
    };

    var onClickMoveNotes = function (event) {
      var $button = $(event.target);
      var $context = $("#tab-electionauthority");
      var $notes = $(".notes textarea", $context);
      var head;
      var prefix;
      if ($button.hasClass("move-main-to-notes-button")) {
        head = "Moved from Main Contact:";
        prefix = "";
      }
      else if ($button.hasClass("move-alt-to-notes-button")) {
        head = "Moved from Alternate Contact:";
        prefix = "alt";
      }
      else return;

      var $name = $("." + prefix + "contact input[type=text]", $context);
      var $title = $("." + prefix + "contacttitle input[type=text]", $context);
      var $phone = $("." + prefix + "contactphone input[type=text]", $context);
      var $email = $("." + prefix + "contactemail input[type=text]", $context);

      var name = $.trim($name.val());
      var title = $.trim($title.val());
      var phone = $.trim($phone.val());
      var email = $.trim($email.val());

      $name.val("");
      $title.val("");
      $phone.val("");
      $email.val("");

      var lines = [];
      if (name) lines.push("Name: " + name);
      if (title) lines.push("Title: " + title);
      if (phone) lines.push("Phone: " + phone);
      if (email) lines.push("Email: " + email);

      if (!lines.length) return;
      lines.splice(0, 0, head);

      var notes = $notes.val();
      if ($.trim(notes)) {
        lines.push("____________________________________________________________");
        lines.push("");
        lines.push("");
      }
      $notes.val(lines.join("\n") + notes);
    };

    //
    // View Reports
    //

    function onClickGetReport() {
      var reportCode = $("select.select-report").val();
      if (!reportCode) {
        util.alert("Please select a report");
      }
      var stateCode = $("body").data("state");

      var url;
      var service;
      switch (reportCode) {
        case "ctyc":
          url = "/admin/countiesreport.aspx?state=" + stateCode;
          service = "GetCountiesReport";
          break;

        case "elof":
          url = "/admin/officials.aspx?state=" + stateCode + "&report=" + stateCode;
          service = "GetOfficialsReport";
          break;

        default:
          return;
      }

      if ($("#get-report-in-new-window-checkbox").prop("checked")) {
        window.open(url, "view");
        return;
      }

      util.openAjaxDialog("Getting Report...");
      util.ajax({
        url: "/Admin/WebService.asmx/" + service,
        data: {
          stateCode: stateCode
        },
        success: function (result) {
          var $report = $("#Report");
          $report.html(result.d.Html).show();
          util.setOffpageTargets($report);
          util.closeAjaxDialog();
        },
        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
              "Could not get the Report"));
        }
      });
    }

    //
    // Master only
    //

    var afterUpdateContainerMasterOnly = function () {
      var active = $("#tab-masteronly .sub-tab-index").val() || 0;
      $("#master-only-tabs").tabs(
      {
        show: 400,
        active: active,
        beforeActivate: onTabsBeforeActivateMasterOnlySubTab,
        activate: function (event, ui) {
          // save the current tab index to restore after update
          $("#tab-masteronly .sub-tab-index")
            .val($("#master-only-tabs").tabs("option", "active"));
        }
      });
    };

    var onTabsBeforeActivateMasterOnlySubTab = function (event, ui) {
    };

    //
    // Misc
    //

    var initPage = function () {
      monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);
      monitor.init();

      util.safeBind($(".jurisdiction-change-button"), "click",
        navigateJurisdiction.changeJurisdictionButtonClicked);

      window.onbeforeunload = function () {
        var changedGroups = monitor.getChangedGroupNames(true);
        if (changedGroups.length > 0)
          return "There are entries on your form that have not been submitted";
      };

      $$('main-tabs')
        .safeBind("tabsbeforeactivate", onTabsBeforeActivate)
        .safeBind("tabsactivate", onTabsActivate);

      $("#tab-viewreports .get-report-button").safeBind("click", onClickGetReport);

      var defaultTab = "tab-generalvoterinfo";
      master.parseFragment(defaultTab, function (info) {
        if (info.tab === defaultTab) {
          reloadPanel(defaultTab, "reloading");
          return false;
        }
      });

      reloadPanel(util.getCurrentTabId("main-tabs"));
    };

    var onTabsActivate = function (event, ui) {
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
        case "tab-generalvoterinfo":
        case "tab-primaryvoterinfo":
        case "tab-voterurls":
        case "tab-ballot":
        case "tab-electionauthority":
        case "tab-masteronly":
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