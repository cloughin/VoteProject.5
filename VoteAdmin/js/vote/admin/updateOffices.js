define([
    "jquery", "vote/adminMaster", "vote/util", "monitor",
    "vote/controls/navigateJurisdiction", "vote/controls/officeControl",
    "vote/controls/managePoliticiansPanel", "jqueryui", "slimscroll", "dynatree"
  ],
  function($,
    master,
    util,
    monitor,
    navigateJurisdiction,
    officeControl,
    managePoliticiansPanel) {

    var $$ = util.$$;
    var queryOffice;

    //
    // Monitor callbacks
    //

    function afterRequest(group) {
      switch (group.group) {
      case "mc-group-editoffice":
        if (!editOfficeDialogLoaded) editOfficeDialogLoaded = true;
        else if (!monitor.groupContainsUpdateError(group))
          $('#edit-office-dialog').dialog('close');
        break;
      }
    }

    function afterUpdateContainer(group, args) {
      if (!group) return;

      switch (group.group) {
      case "mc-group-addincumbents":
        afterUpdateContainerAddIncumbents(group, args);
        break;

      case "mc-group-editoffice":
        officeControl.initSelectOfficeTree();
        officeControl.refreshOfficeHeading();
        break;

      case "mc-group-masteronly":
        afterUpdateContainerMasterOnly(group, args);
        break;
      }
    };

    //
    // Add Office
    //

    function initAddOfficeTab() {
      util.registerDataMonitor($("#tab-addoffice .office-line-1 input"));

      $("#tab-addoffice .select-office-class")
        .change(onChangeAddOfficeClassDropdown);

      $("#tab-addoffice .add-office-button")
        .click(onClickOfficeAddButton);
    }

    function getSelectedOfficeClass() {
      return $("#tab-addoffice .select-office-class").val();
    }

    function onChangeAddOfficeClassDropdown() {
      setUpForNewOffice();
    }

    function onClickOfficeAddButton() {
      var $line1 = $("#tab-addoffice .office-line-1 input");
      var $line2 = $("#tab-addoffice .office-line-2 input");
      var line1 = $.trim($line1.val());
      var line2 = $.trim($line2.val());
      if (!line1) {
        $line1.addClass("error");
        util.alert("1st Line of Office Title is required.");
        return;
      }
      util.openAjaxDialog("Adding office...");
      util.ajax({
        url: "/Admin/WebService.asmx/AddOffice",
        data: {
          stateCode: getStateCode(),
          countyCode: getCountyCode(),
          localKey: getLocalKey(),
          officeClass: getSelectedOfficeClass(),
          line1: line1,
          line2: line2
        },

        success: function(result) {
          util.closeAjaxDialog();
          if (result.d.substr(0, 1) == "*") {
            // error
            util.alert(result.d.substr(1), "An error occurred");
          } else {
            setUpForNewOffice();
            $('.office-to-edit').val(result.d);
            $('#edit-office-dialog').dialog('open');
            window.__doPostBack('UpdateOfficeControl', '');
            util.alert("Office key: " +
              result.d +
              "\n\n" +
              "Use the Edit dialog that follows to change any defaults.\n" +
              "Just dismiss the dialog if no changes are needed.", "The office was added");
          }
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "Could not get current offices"));
        }
      });
    }

    function setUpForNewOffice() {
      var $newOfficeBox = $("#tab-addoffice .new-office-box");
      $newOfficeBox.addClass("hidden");
      var officeClass = getSelectedOfficeClass();

      if (officeClass) {
        // get a list of current offices for this class and display it to check for duplicates
        util.openAjaxDialog("Getting current offices...");
        util.ajax({
          url: "/Admin/WebService.asmx/GetOfficesByClass",
          data: {
            stateCode: getStateCode(),
            countyCode: getCountyCode(),
            localKey: getLocalKey(),
            officeClass: officeClass
          },

          success: function(result) {
            util.closeAjaxDialog();
            $("#tab-addoffice .current-office-list-container")
              .toggleClass("hidden", result.d.length === 0);
            $("#tab-addoffice .current-office-list").html(result.d.join("<br />"));
            $("#tab-addoffice .office-line-1 input").removeClass("error").val("");
            $("#tab-addoffice .office-line-2 input").val("");
            $("#tab-addoffice .new-office-box").removeClass("hidden");

          },

          error: function(result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result, "Could not get current offices"));
          }
        });
      }
    }

    // 
    // Add Office Template
    //

    function initAddOfficeTemplateTab() {
      util.registerDataMonitor($("#tab-addofficetemplate .office-line-1 input"));

      $("#tab-addofficetemplate input[name='AddOfficeTemplate_Level']:radio")
        .change(onChangeTemplateOfficeLevelRadio);

      $("#tab-addofficetemplate .select-office-class")
        .change(onChangeTemplateOfficeClassDropdown);

      $("#tab-addofficetemplate .add-office-template-button")
        .click(onClickTemplateAddButton);
    }

    function getSelectedTemplateOfficeLevel() {
      return $("#tab-addofficetemplate input[name='AddOfficeTemplate_Level']:checked")
        .val();
    }

    function getSelectedTemplateOfficeClass() {
      return $("#tab-addofficetemplate .select-office-class").val();
    }

    function onChangeTemplateOfficeClassDropdown() {
      setUpForNewTemplate();
    }

    function onChangeTemplateOfficeLevelRadio() {
      var $select = $("#tab-addofficetemplate .select-office-class");
      $("#tab-addofficetemplate .new-template-box").addClass("hidden");

      function load(index) {
        var items = $("#tab-addofficetemplate .hidden-data").data("office-classes")[index];
        util.populateDropdown($select, items, "<select an office class>", "");
        $select.prop("disabled", false);
      }

      switch (getSelectedTemplateOfficeLevel()) {
      case "county":
        load(0);
        break;

      case "local":
        load(1);
        break;

      default:
        util.populateDropdown($select, [], "Choose an office class", "");
        $select.prop("disabled", true);
        break;
      }
    }

    function onClickTemplateAddButton() {
      var $line1 = $("#tab-addofficetemplate .office-line-1 input");
      var $line2 = $("#tab-addofficetemplate .office-line-2 input");
      var line1 = $.trim($line1.val());
      var line2 = $.trim($line2.val());
      if (!line1) {
        $line1.addClass("error");
        util.alert("1st Line of Office Title is required.");
        return;
      }
      util.openAjaxDialog("Adding office template...");
      util.ajax({
        url: "/Admin/WebService.asmx/AddOfficeTemplate",
        data: {
          stateCode: getStateCode(),
          officeClass: getSelectedTemplateOfficeClass(),
          line1: line1,
          line2: line2
        },

        success: function(result) {
          util.closeAjaxDialog();
          if (result.d.substr(0, 1) == "*") {
            // error
            util.alert(result.d.substr(1), "An error occurred");
          } else {
            setUpForNewTemplate();
            $('.office-to-edit').val(result.d);
            $('#edit-office-dialog').dialog('open');
            window.__doPostBack('UpdateOfficeControl', '');
            util.alert("Office key: " +
              result.d +
              "\n\n" +
              "Use the Edit dialog that follows to change any defaults.\n" +
              "Just dismiss the dialog if no changes are needed.",
              "The office template was added");
          }
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "Could not get current templates"));
        }
      });
    }

    function setUpForNewTemplate() {
      var $newTemplateBox = $("#tab-addofficetemplate .new-template-box");
      $newTemplateBox.addClass("hidden");
      var officeClass = getSelectedTemplateOfficeClass();

      if (officeClass) {
        // get a list of current office templates for this class and display it to check for duplicates
        util.openAjaxDialog("Getting current templates...");
        util.ajax({
          url: "/Admin/WebService.asmx/GetOfficeTemplatesByClass",
          data: {
            stateCode: getStateCode(),
            officeClass: officeClass
          },

          success: function(result) {
            util.closeAjaxDialog();
            $("#tab-addofficetemplate .current-template-list-container")
              .toggleClass("hidden", result.d.length === 0);
            $("#tab-addofficetemplate .current-template-list")
              .html(result.d.join("<br />"));
            $("#tab-addofficetemplate .office-line-1 input").removeClass("error").val("");
            $("#tab-addofficetemplate .office-line-2 input").val("");
            $("#tab-addofficetemplate .new-template-box").removeClass("hidden");

          },

          error: function(result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result, "Could not get current templates"));
          }
        });
      }
    }

    //
    // Office Information
    //
    var autoSelectEditOffice;

    function initChangeInfoTab() {
      $(".changeinfo-edit-button").click(function() {
        $('.office-to-edit').val(officeControl.getSelectedOffice());
        $('#edit-office-dialog').dialog('open');
      });

      $(".changeinfo-delete-button").click(function() {
        if ($(this).hasClass("disabled")) return;
        var officeKey = officeControl.getSelectedOffice();
        // don't do this check if state level (i.e., getCountyCode() is blank
        if (getCountyCode() && officeKey.indexOf('#') >= 0) {
          util.alert(
            "This is a virtual office that has not yet been actualized. Virtual office deletions should be done at the state level.");
          return;
        }
        util.openAjaxDialog("Getting office information...");
        util.ajax({
          url: "/Admin/WebService.asmx/AnalyzeOfficeForDeletion",
          data: {
            officeKey: officeKey
          },

          success: function(result) {
            util.closeAjaxDialog();
            showOfficeDeleteDialog(result.d);
          },

          error: function(result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result, "Could not get office information"));
          }
        });
      });
    }

    //
    // Add Incumbents
    //

    function afterUpdateContainerAddIncumbents() {
      //officeControl.initSelectOfficeTree();
    }

    //
    // Master only
    //

    var masterOnlyStartTab = "";

    function afterUpdateContainerMasterOnly() {
      var active = $("#tab-masteronly .sub-tab-index").val() || 0;
      if (masterOnlyStartTab) {
        active = util.getTabIndex("master-only-tabs", masterOnlyStartTab);
        masterOnlyStartTab = "";
        $("#tab-masteronly .sub-tab-index").val(active);
      }
      $("#master-only-tabs").tabs(
        {
          show: 400,
          active: active,
          beforeActivate: function(event, ui) {
            onTabsBeforeActivateMasterOnlySubTab(event, ui);
          },
          activate: function() {
            // save the current tab index to restore after update
            $("#tab-masteronly .sub-tab-index")
              .val($("#master-only-tabs").tabs("option", "active"));
          }
        });
    }

    var onTabsBeforeActivateMasterOnlySubTab = function(event, ui) {
      return monitor.tabsBeforeActivate(event, ui, "master-only-tabs",
        "#tab-masteronly .mc-container");
    };

    //
    // Edit Office Dialog
    //

    var editOfficeDialogLoaded;

    function initEditOfficeDialog() {
      var $dialog = $("#edit-office-dialog");
      $dialog.dialog({
        autoOpen: false,
        width: 880,
        resizable: false,
        dialogClass: 'edit-office overlay-dialog',
        // custom open and close to fix various problems
        open: onOpenEditOfficeDialog,
        close: function() {
          editOfficeDialogLoaded = false;
          master.onCloseJqDialog();
          //focusAddCandidateTabs();
        },
        modal: true
      });
      $dialog.on("propertychange change click keyup input paste", ".synced-item", function () {
        if ($(".sync-positions", $dialog).prop("checked")) {
          var syncVal = $(this).val();
          $(".synced-item", $dialog).each(function () {
            var $this = $(this);
            if ($this.val() != syncVal) {
              $this.val(syncVal);
              $this.trigger("change");
            }
          });
        }
      });
      $dialog.on("rc_checked", ".sync-positions", function () {
        if ($(this).prop("checked")) {
          // sync to election positions
          $(".incumbents-item", $dialog).trigger("change");
        }
      });
    }

    function onOpenEditOfficeDialog() {
      var $dialog = $("#edit-office-dialog");
      $(".content-area", $dialog).css("visibility", "hidden");
      monitor.clearGroupFeedback("mc-group-editoffice");
      master.onOpenJqDialog.apply(this);
      editOfficeDialogLoaded = false;
      $('.reloading', $dialog).val("reloading");
      $('input.update-button', $dialog).addClass("reloading").click();
    }

    //
    // Delete Office Dialog
    //

    function initDeleteOfficeDialog() {
      $('#delete-office-dialog').dialog({
        autoOpen: false,
        width: 500,
        title: "Delete Office",
        resizable: false,
        dialogClass: 'delete-office overlay-dialog',
        modal: true
      });

      $("#delete-office-dialog .delete-office-button").click(function () {
        var officeKey = officeControl.getSelectedOffice();
        util.openAjaxDialog("Deleting office...");
        util.ajax({
          url: "/Admin/WebService.asmx/DeleteOffice",
          data: {
            officeKey: officeKey
          },

          success: function () {
            util.closeAjaxDialog();
            $('#delete-office-dialog').dialog('close');
            officeControl.deselect();
            window.__doPostBack('UpdateOfficeControl', '');
            enableChangeInfoButtons();
            util.alert("The office was deleted.");
          },

          error: function (result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result, "Could not delete office"));
          }
        });
      });
    }

    function showOfficeDeleteDialog(info) {
      var infoLines = [];

      function processInfo(table, lines) {
        if (lines.length) {
          infoLines.push('<p class="info-table">In ' + table + '</p>');
          $.each(lines, function () {
            infoLines.push('<p class="info-line">' + this + '</p>');
          });
        }
      }

      processInfo("ElectionsOffices", info.InElectionsOffices);
      processInfo("ElectionsPoliticians", info.InElectionsPoliticians);
      processInfo("OfficesOfficials", info.InOfficesOfficials);
      processInfo("ElectionsIncumbentsRemoved", info.InElectionsIncumbentsRemoved);
      processInfo("Politicians.LiveOfficeKey", info.InPoliticiansLiveOfficeKey);

      if (!infoLines.length)
        infoLines.push('<p class="info-line">No references were found.</p>');

      $("#delete-office-dialog .reference-scroller").html(infoLines.join(""));

      $('#delete-office-dialog').dialog('open');
    }

    //
    // Misc
    //

    function initPage() {
      monitor.registerCallback("afterRequest", afterRequest);
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

      queryOffice = $.queryString("office");
      officeControl.initControl({
        onSelect: function () {
          reloadPanel($(".office-control").closest(".ui-tabs-panel")[0].id, "reloading");
        },
        officeKey: queryOffice
      });
      officeControl.initSelectOfficeTree();

      managePoliticiansPanel.initControl();

      var defaultTab = queryOffice ? "tab-changeinfo" : "tab-addoffice";
      var tabInfo = master.parseFragment(defaultTab);
      switch (tabInfo.tab) {
        case "tab-addoffice":
          // not called automatically for first tab
          reloadPanel("tab-addoffice");
          setUpForNewOffice();
          break;

        case "tab-changeinfo":
          if (queryOffice) autoSelectEditOffice = true;
          break;

        case "tab-masteronly":
          masterOnlyStartTab = tabInfo.subTab;
          break;
      }

      //      if (tabInfo.tab === "tab-masteronly")
      //        masterOnlyStartTab = tabInfo.subTab;

      initAddOfficeTab();
      initAddOfficeTemplateTab();
      initChangeInfoTab();
      initEditOfficeDialog();
      initDeleteOfficeDialog();

      $(".recase-button input").click(function () {
        var $context = $(this).closest(".main-tab");
        var $first = $(".office-line-1 input", $context);
        var $second = $(".office-line-2 input", $context);
        util.ajax({
          url: "/Admin/WebService.asmx/ToTitleCase",
          data: {
            input: [$first.val(), $second.val()]
          },

          success: function (result) {
            util.closeAjaxDialog();
            $first.val(result.d[0]);
            $second.val(result.d[1]);
          },

          error: function (result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result, "Could not recase"));
          }
        });
      });
    }

    function endRequest() {
      if (!$('.select-office-control ul').hasClass("dynatree-container")) {
        officeControl.initSelectOfficeTree();
        officeControl.refreshOfficeHeading();
      }
    }

    function getCountyCode() {
      return $("#MainForm").data("county-code");
    }

    function getLocalKey() {
      return $("#MainForm").data("local-key");
    }

    function getStateCode() {
      return $("#MainForm").data("state-code");
    }

    function onTabsActivate(event, ui) {
      var newPanelId = ui.newPanel[0].id;
      switch (newPanelId) {
        case "tab-changeinfo":
        case "tab-addincumbents":
          // if we don't have the office control, grab it
          var $officeControl = $(".office-control");
          var $ownerTab = $officeControl.closest(".ui-tabs-panel");
          if (newPanelId !== $ownerTab[0].id) {
            // need to move it
            officeControl.toggle(false);
            var $newTab = $$(newPanelId);
            $(".office-control-container", $newTab).append($officeControl);
            $(".office-heading", $newTab).html($(".office-heading", $ownerTab).html());
          }
          // we now allow unactualized offices
          //var officeKey = officeControl.getSelectedOffice();
          //if (newPanelId === "tab-addincumbents" && officeControl.isTemplateKey(officeKey))
          //  officeKey = "";
          //if (officeKey) 
            reloadPanel(newPanelId);
          //else 
          //  officeControl.toggle(true);
          break;

        default:
          reloadPanel(newPanelId);
          break;
      }
    }

    function onTabsBeforeActivate(event, ui) {
      return monitor.tabsBeforeActivate(event, ui, "main-tabs");
    }

    function enableChangeInfoButtons() {
      $(".changeinfo-edit-button").toggleClass("disabled", !officeControl.getSelectedOffice());
      $(".changeinfo-delete-button").toggleClass("disabled",
        !officeControl.getSelectedOffice() /*|| officeControl.isUndeletable()*/);
      if (autoSelectEditOffice) {
        autoSelectEditOffice = false;
        $(".changeinfo-edit-button").click();
      }
    }

    function reloadPanel(id, option) {
      if (!option) option = "reloading";
      switch (id) {
        case "tab-addoffice":
        case "tab-addincumbents":
        case "tab-masteronly":
          $$(id + ' .reloading').val(option);
          $$(id + ' input.update-button').addClass("reloading").click();
          break;

        case "tab-changeinfo":
          // not really a reload -- just enable the buttons
          enableChangeInfoButtons();
          break;
      }
    }

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage,
      endRequest: endRequest
    });
  });