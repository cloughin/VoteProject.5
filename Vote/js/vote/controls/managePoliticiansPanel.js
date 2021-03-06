define(["jquery", "vote/adminMaster", "vote/util", "monitor",
  "vote/controls/officeControl", "vote/controls/findPolitician"],
  function ($, master, util, monitor, officeControl, findPolitician) {

    var $$ = util.$$;

    var selectedElectionFn;

    function getSelectedElection() {
      if (typeof selectedElectionFn === "function")
        return selectedElectionFn();
    }

    function getStateCode() {
      var stateCode = officeControl.getSelectedOffice().substr(0, 2).toUpperCase();
      if (!stateCode) // for AddPoliticians
        stateCode = $(".mc-group-addnewcandidate-statecode").val();
      return stateCode;
    }

    function initControl(getSelectedElectionFn) {
      selectedElectionFn = getSelectedElectionFn;
      monitor.registerCallback("afterRequest", afterRequest);
      monitor.registerCallback("afterUndo", afterUndo);
      monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);
      monitor.registerCallback("afterUpdateGroup", afterUpdateGroup);
      monitor.registerCallback("clientChange", clientChange);
      monitor.registerCallback("initRequest", initRequest);
      initEditPoliticianDialog();
      initDuplicateLastNameDialog();
      initConsolidatePoliticiansDialog();
      $("body").on("paste", ".trim-special", function() {
        var $this = $(this);
        setTimeout(function () {
          var val = $this.val();
          var newval = util.trimNonAlpha(val);
          if (val !== newval)
            $this.val(newval);
        }, 1);
      });
    }

    function setCandidateBackground($removeIcon) {
      var $result = null;
      $.each($removeIcon, function () {
        var $parent = $(this).parents(".candidate");
        if ($parent.hasClass("main"))
          $parent = $parent.parents(".candidates-list .outer");
        $parent.toggleClass("deleting", $removeIcon.hasClass("checked"));
        $result = $result || $parent;
      });
      return $result;
    }

    //
    // Monitor callbacks
    //

    function afterRequest(group) {
      switch (group.group) {
        case "mc-group-addnewcandidate":
          afterRequestAddNewCandidate();
          break;

        case "mc-group-editpolitician":
          if (!editPoliticianDialogLoaded) editPoliticianDialogLoaded = true;
          else if (!monitor.groupContainsUpdateError(group))
            $('#edit-politician-dialog').dialog('close');
          break;

        case "mc-group-consolidate":
          afterRequestConsolidate(group);
          break;
      }
    }

    function afterUndo(group) {
      switch (group.group) {
        case "mc-group-addcandidates":
          candidateListActionCompleted("Changes to the " + getModeDescription() + " list were undone.");
          break;
      }
    }

    function afterUpdateContainer(group) {
      if (!group) return;

      //      $(".date-picker", $$(group.container)).datepicker({
      //        changeYear: true,
      //        yearRange: "2010:+1"
      //      });

      switch (group.group) {
        case "mc-group-addcandidates":
          afterUpdateContainerAddCandidates();
          initEditPoliticianDialog();
          initDuplicateLastNameDialog();
          initConsolidatePoliticiansDialog();
          break;

        case "mc-group-editpolitician":
          afterUpdateContainerEditPolitician();
          break;

        case "mc-group-consolidate":
          afterUpdateContainerConsolidate();
          break;
      }
    }

    function afterUpdateGroup(group, args) {
      if (!group) return;
      switch (group.group) {
        case "mc-group-addnewcandidate-statecode":
          afterUpdateGroupAddNewCandidateStateCode(group, args);
          break;
      }
    }

    function clientChange(group, args) {
      switch (group.group) {
        case "mc-group-addnewcandidate":
          onAddNewCandidateChanged(group, args);
          break;
      }
    }

    function initRequest(group) {
      if (!group) return true;
      switch (group.group) {
        case "mc-group-addcandidates":
          return initRequestAddCandidates(group);

        case "mc-group-editpolitician":
          return initRequestEditPolitician(group);

        case "mc-group-consolidate":
          return initRequestConsolidate(group);
      }
      return true;
    }

    //
    // CandidateList Custom Data Type
    //

    candidateList.prototype = new monitor.DataType();
    candidateList.prototype.constructor = candidateList;
    candidateList.prototype.parent = monitor.DataType.prototype;
    function candidateList() {
    }

    candidateList.prototype.name = "CandidateList";

    candidateList.prototype.bindChange = function ($data) {
      this.initControl($data);
      util.safeBind($data, "sortstop", monitor.dataChanged);
    };

    candidateList.prototype.dataChanged = function (group, $data) {
      $data.prev().val(this.val($data));
      candidateListActionCompleted(getModeDescription(true) + " order was changed.");
    };

    candidateList.prototype.handles = function ($data) {
      var o = $data[0];
      return o.tagName.toLowerCase() === "ul" && $data.hasClass("candidates-list");
    };

    candidateList.prototype.initControl = function ($data) {
      if (!$data.hasClass("ui-sortable"))
        $data.sortable({ axis: "y", opacity: 0.5, handle: ".main .icon-move" });
    };

    candidateList.prototype.unbindChange = function ($data) {
      $data.off("sortstop", monitor.dataChanged);
    };

    candidateList.prototype.val = function ($data, value) {
      this.initControl($data);
      if (typeof (value) === "undefined") {
        {
          // Start with the list of id's
          var rawIds = $data.sortable('toArray');
          var ids = [];
          $.each(rawIds, function (index, id) {
            // For each id, we skip it if it's remove flag is checked
            if ($$(id + " .main .icon-remove.checked").length === 0) {
              // get running mate id, if any
              var mateId = $$(id + " .mate-id").val();
              if (mateId && $$(id + " .mate .icon-remove.checked").length === 0)
                id += "+" + mateId;
              ids.push(id);
            }
          });
          return ids.join('|');
        }
      } else {
        ids = value.split("|");
        if (ids.length === 1 && !ids[0]) ids = [];
        $.each(ids, function (index) {
          var split = this.split('+');
          var $o = $$(split[0]);
          if (index !== $o.index())
            $o.insertBefore($o.parent().children()[index]);
          setCandidateBackground($(".main .icon-remove", $o).removeClass("checked"));
          setCandidateBackground($(".mate .icon-remove", $o).toggleClass("checked", !split[1]));
        });
        // This will leave any newly-added candidates at the bottom of the list.
        // We leave them there but mark them as removed.
        setCandidateBackground($(".outer", $data).slice(ids.length).find(".icon-remove").addClass("checked"));
        return $data;
      }
    };

    monitor.registerDataType(new candidateList());

    //
    // Candidate List Control
    //

    function candidateListChanged() {
      monitor.dataChanged("mc-group-addcandidates-candidatelist", null);
    }

    function getMode() {
      return $(".manage-politicians-panel-mode").val();
    }

    function getModeDescription(capitalize) {
      var result = "";
      switch (getMode()) {

        case "AddPolitician":
          result = "Politician";
          break;

        case "ManageCandidates":
          result = "Candidate";
          break;

        case "ManageIncumbents":
          result = "Incumbent";
          break;
      }
      if (!capitalize) result = result.toLowerCase();
      return result;
    }

    function getTargetDescription(capitalize) {
      var result = "";
      switch (getMode()) {
        case "ManageCandidates":
          result = "Election";
          break;

        case "ManageIncumbents":
          result = "Office";
          break;
      }
      if (!capitalize) result = result.toLowerCase();
      return result;
    }

    function onClickRemoveCandidate() {
      var $this = $(this);
      if ($this.hasClass("checked") && $this.closest(".candidate.running-mate").length === 0 &&
        !canAddIncumbent()) return;
      $this.toggleClass("checked");
      setCandidateBackground($this);
      candidateListChanged();
      if ($this.hasClass("checked"))
        candidateListActionCompleted(getModeDescription(true) + " was removed.");
      else
        candidateListActionCompleted(getModeDescription(true) + " was restored.");
    }

    //
    // Add Candidates
    //

    var fp = null; // FindPolitician instance

    function afterUpdateContainerAddCandidates() {
      if (fp) fp.destroy();

      fp = new findPolitician.FindPolitician($("#main-tabs ." +
       findPolitician.getControlName()), {
         onNewList: enableAddSearchCandidateButton,
         onSelectionChanged: enableAddSearchCandidateButton,
         onDblClickCandidate: onClickAddSearchCandidate,
         getIdsToSkip: getSearchIdsToSkip,
         getStateCode: getStateCode,
         canMultiSelect: true
       });
      fp.init();
      fp.setLabel("Enter last name to search for existing politician:");

      if (getMode() != "AddPoliticians") officeControl.initSelectOfficeTree();
      var $candidateList = $("#main-tabs .candidates-list");
      if (!$candidateList.hasClass("ui-sortable"))
        $candidateList.sortable({ axis: "y", opacity: 0.5, handle: ".main .icon-move" });
      initCandidates($("#main-tabs"));
      util.safeBind($("#main-tabs .add-search-candidate-button"),
        "click", onClickAddSearchCandidate);
      util.safeBind($("#main-tabs .consolidate-politicians-button"),
        "click", onClickConsolidatePoliticians);
      util.safeBind($("#main-tabs .add-new-candidate-button"),
        "click", onClickAddNewCandidate);
      util.safeBind($("#duplicate-last-name-dialog .continue-adding-new-politician-button"),
        "click", onClickContinueAddingNewPolitician);
      util.safeBind($("#duplicate-last-name-dialog .use-politician-from-list-button"),
        "click", onClickUsePoliticianFromList);
      $("#add-candidate-tabs").tabs(
      {
        show: 400,
        active: /*getMode() === "AddPoliticians" ? 1 :*/ 0,
        activate: focusAddCandidateTabs
      });

      if (officeControl.getSelectedOffice() || getMode() === "AddPoliticians") {
        $("#main-tabs .add-politician-panel")
          .show("scale", 400, function () {
            $(".search-politician-name input[type=text]").focus();
          });
      }
      enableRunningMateMessage();
    }

    function addCandidate(key, openEditDialog) {
      util.openAjaxDialog("Adding " + getModeDescription() + " to " + getTargetDescription() + "...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetCandidateHtml",
        data: {
          openEditDialog: !!openEditDialog,
          electionKey: getSelectedElection() || "",
          politicianKey: key,
          officeKey: officeControl.getSelectedOffice(),
          mode: getMode()
        },
        success: getCandidateHtmlSucceeded,
        error: getCandidateHtmlFailed
      });
    }

    function addRunningMate(key, mainId, openEditDialog) {
      util.openAjaxDialog("Adding running mate to " + getTargetDescription() + "...");
      var data =
      {
        openEditDialog: !!openEditDialog,
        electionKey: getSelectedElection() || "",
        politicianKey: key,
        mainCandidateKey: mainId,
        mode: getMode()
      };
      util.ajax({
        type: "POST",
        url: "/Admin/WebService.asmx/GetRunningMateHtml",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: getRunningMateHtmlSucceeded,
        error: getRunningMateHtmlFailed
      });
    }

    function canAddIncumbent() {
      if (getMode() !== "ManageIncumbents") return true;
      var maxIncumbents = $(".incumbent-count").val();
      var currentIncumbents = $(".candidates-list>li>div:not(.deleting)").length;
      if (currentIncumbents >= maxIncumbents) {
        util.alert("This would exceed the maximum of " + maxIncumbents + " incumbent" +
          (maxIncumbents == 1 ? " " : "s") + ". To add an incumbent you must first delete one.");
        return false;
      }
      return true;
    }

    function candidateListActionCompleted(action) {
      var isChanged = monitor.isPanelsChanged($(".mc-container", util.getCurrentTabPanel("main-tabs")));
      if (isChanged)
        action += " Click update to save the new list of " + getModeDescription() + "s.";
      monitor.addGroupFeedback("mc-group-addcandidates", "ok", action, true);
    }

    function enableAddSearchCandidateButton() {
      var selectedCount = fp.countSelectedCandidates();
      $("#tab-add-candidate-search .enable-add-candidate-search")
        .toggleClass("disabled", selectedCount !== 1);
      $("#tab-add-candidate-search .consolidate-politicians-button")
        .toggleClass("disabled", selectedCount !== 2);
    }

    function focusAddCandidateTabs() {
      switch ($("#add-candidate-tabs").tabs("option", "active")) {
        case 0:
          // search
          $(".search-politician-name input[type=text]").focus();
          break;
        case 1:
          // add
          var partialName = $("#tab-add-candidate-search .input input").val();
          $(".mc-group-addnewcandidate-lname.mc-data").val(util.trimNonAlpha(partialName));
          $(".mc-group-addnewcandidate-fname.mc-data").focus();
          break;
      }
    }

    function enableRunningMateMessage() {
      $("#add-candidate-tabs .footer div.msg")
        .toggle($(".no-mate").length > 0);
    }

    function getCandidateHtmlFailed(result) {
      monitor.addGroupFeedback("mc-group-addcandidates", "ng",
        "Could not add " + getModeDescription() + ". " + result.status + ' ' + result.statusText, true);
    }

    function getCandidateHtmlSucceeded(result) {
      var $children = $(".candidates-list").append(result.d.CandidateHtml).children();
      initCandidates($($children[$children.length - 1]));
      candidateListChanged();
      $("#tab-add-candidate-search .search-results-container").html("");
      $("#tab-add-candidate-search .input input").val("");
      $(".add-candidates-message").hide();
      enableAddSearchCandidateButton();
      candidateListActionCompleted(getModeDescription(true) + " added to office.");
      util.closeAjaxDialog();
      if (result.d.OpenEditDialog)
        editCandidate(result.d.PoliticianKey);
    }

    function getCandidateKey(id) {
      util.alert(id, "Politician Key");
      return false;
    }

    function getCurrentAddCandidatePanel() {
      return $("#add-candidate-tabs .ui-tabs-panel[aria-hidden='false']");
    }

    function getRunningMateHtmlFailed(result) {
      monitor.addGroupFeedback("mc-group-addcandidates", "ng",
        "Could not add running mate. " + result.status + ' ' + result.statusText, true);
      util.closeAjaxDialog();
    }

    function getRunningMateHtmlSucceeded(result) {
      var $parent = findCandidate(result.d.MainCandidateKey).parent();
      $(".no-mate", $parent).replaceWith(result.d.RunningMateHtml);
      initCandidates($(".mate", $parent));
      candidateListChanged();
      $("#tab-add-candidate-search .search-results-container").html("");
      $("#tab-add-candidate-search .input input").val("");
      enableAddSearchCandidateButton();
      candidateListActionCompleted("Running mate was added to office.");
      util.closeAjaxDialog();
      if (result.d.OpenEditDialog)
        editCandidate(result.d.PoliticianKey);
    }

    function getSearchCandidateKey() {
      var $o = $("#tab-add-candidate-search .search-results-container .selected");
      if ($o.length !== 1) return "";
      var id = $o[0].id;
      return id.substr(id.lastIndexOf('-') + 1);
    }

    function getSearchIdsToSkip() {
      var rawIds = $(".candidates-list").sortable('toArray');
      var ids = [];
      $.each(rawIds, function (index, id) {
        {
          ids.push(id.substr(id.lastIndexOf('-') + 1));
          // get running mate id, if any
          var mateId = $$(id + " .mate-id").val();
          if (mateId) ids.push(mateId);
        }
      });
      return ids;
    }

    function initCandidates($o) {
      $(".icon-remove", $o).safeBind("click", onClickRemoveCandidate);
      $(".icon-add-mate", $o).safeBind("click", onClickAddRunningMate);
      $("ul.candidate-menu", $o).menu({ position: { at: "right+5 top"} });
      $(".edit-candidate-link", $o).safeBind("click", function (event) {
        editCandidate($(event.target).attr("pkey"));
      });
      $(".get-candidate-key-link", $o).safeBind("click", function (event) {
        getCandidateKey($(event.target).attr("pkey"));
      });
      $("ul.candidate-menu .icon-menu", $o).hover(function () {
        // collapse other menus
        var $menus = $("#main-tabs ul.candidate-menu");
        var thisOuter = this;
        $.each($menus, function () {
          if (thisOuter !== $(".icon-menu", this)[0]) {
            $(this).menu("collapseAll", null, true);
          }
        });
      });
    }

    function initRequestAddCandidates(group) {
      if ($$(group.button).hasClass("validated")) {
        $$(group.button).removeClass("validated");
        return true;
      }
      if (monitor.isGroupChanged("mc-group-addnewcandidate")) {
        util.confirm("There is information entered for a new politician" +
          " that has not been updated. Click 'Cancel' to return to adding" +
          " the new politician or 'OK' to discard the new politician and" +
          " update the " + getModeDescription() + " list.",
             function (button) {
               if (button === "Ok") {
                 $$(group.button).addClass("validated").click();
               }
             });
        return false; // cancels the request
      }
    }

    function onClickAddRunningMate() {
      var $this = $(this);
      if ($this.hasClass("disabled")) return;
      var mainId = $this.parents("li")[0].id;
      mainId = mainId.substr(mainId.lastIndexOf('-') + 1);
      var isFromSearch = getCurrentAddCandidatePanel()[0].id ===
        "tab-add-candidate-search";
      var key;
      if (isFromSearch) {
        key = getSearchCandidateKey();
        if (!key) {
          util.alert("Please select a politician to add as a running mate from the panel on the right.");
          return;
        }
        addRunningMate(key, mainId, false);
      } else {
        // from add panel
        if ($("#main-tabs .add-new-candidate-button").hasClass("disabled")) {
          util.alert("Please enter all required fields to add a new politician as a running mate.");
          return;
        }
        addNewCandidate(true, mainId);
      }
    }

    function onClickAddSearchCandidate() {
      if (getMode() === "AddPoliticians") return;
      var $this = $(this);
      var key = getSearchCandidateKey();
      if ($this.hasClass("disabled") || !key || !canAddIncumbent()) return;
      addCandidate(key, false);
    }

    //
    // Add New Politician
    //

    function afterRequestAddNewCandidate() {
      // if there were potential duplicates, show the dialog
      var $duplicatesHtml = $(".add-candidate-duplicates-html");
      var duplicatesHtml = $duplicatesHtml.html();
      if (duplicatesHtml) {
        $(".mc-group-addnewcandidate-lname").removeClass("error");
        $duplicatesHtml.html("");
        var $o = $("#duplicate-last-name-dialog .search-results-container");
        $o.html(duplicatesHtml);
        $(".search-politician", $o).safeBind("click", onClickDuplicatePolitician);
        enableUsePoliticianFromListButton();
        $("#duplicate-last-name-dialog-formatted-name")
          .html($(".add-candidate-formatted-name").val());
        $("#duplicate-last-name-dialog-state-name")
          .html($(".add-candidate-state-name").val());
        $('#duplicate-last-name-dialog')
          .data("mainId", $(".add-candidate-main-id-if-running-mate").val())
          .dialog("open");
      } else if (!monitor.groupHasUpdateError("mc-group-addnewcandidate")) {
        var mainIdIfRunningMate = $(".add-candidate-main-id-if-running-mate").val();
        var openEditDialog = $(".add-candidates-open-edit-panel input")
          .prop("checked");
        var politicianKey = $(".add-candidate-new-id").val();
        if (getMode() === "AddPoliticians") {
          $(".search-politician-name input").val("");
          if (openEditDialog)
            editCandidate(politicianKey);
          util.alert("Politician was added with key " + politicianKey);
        }
        else if (mainIdIfRunningMate) {
          // add as running mate
          addRunningMate(politicianKey, mainIdIfRunningMate, openEditDialog);
        } else {
          // add as main candidate
          addCandidate(politicianKey, openEditDialog);
        }
        // Shift focus back to search
        if (getMode() !== "AddPoliticians") {
          $("#add-candidate-tabs").tabs({ active: 0 });
          focusAddCandidateTabs();
        }
      }
    }

    function afterUpdateGroupAddNewCandidateStateCode(group, args) {
      // preset state code
      var stateCode = getStateCode();
      if (stateCode !== "US")
        $$(group.data).val(stateCode);
    }

    function clearAddNewPolitician() {
      $("#tab-add-candidate-add .fname .data").val("");
      $("#tab-add-candidate-add .mname .data").val("");
      $("#tab-add-candidate-add .nickname .data").val("");
      $("#tab-add-candidate-add .lname .data").val("");
      $("#tab-add-candidate-add .suffix .data").val("");
      if (officeControl.getSelectedOffice().substr(0, 2).toLowerCase() === "us")
        $("#tab-add-candidate-add .statecode .data").val("");
    }

    function enableAddNewCandidateButton() {
      var hasRequiredData = true;
      $.each($("#tab-add-candidate-add .required"), function () {
        if (!$(this).val()) {
          hasRequiredData = false;
          return false;
        }
      });
      $("#tab-add-candidate-add .enable-add-candidate-new")
        .toggleClass("disabled", !hasRequiredData);
    }

    function enableUsePoliticianFromListButton() {
      if (getMode() === "AddPoliticians") return; // button repurposed to "Cancel Addition", always enabled
      $("#duplicate-last-name-dialog .use-politician-from-list-button")
        .toggleClass("disabled", $("#duplicate-last-name-dialog .search-results-container .selected").length === 0);
    }

    function getDuplicatePoliticianKey() {
      var $o = $("#duplicate-last-name-dialog .search-results-container .selected");
      if ($o.length !== 1) return "";
      var id = $o[0].id;
      return id.substr(id.lastIndexOf('-') + 1);
    }

    function onAddNewCandidateChanged(group, args) {
      monitor.clearGroupFeedback(group.group);
      enableAddNewCandidateButton();
    }

    function addNewCandidate(validateDuplicates, mainIdIfRunningMate) {
      $(".add-candidate-validate-duplicates").val((!!validateDuplicates).toString());
      $(".add-candidate-main-id-if-running-mate").val(mainIdIfRunningMate || "");
      $(".button-add-new-candidate").click();
    }

    function onClickAddNewCandidate() {
      var $this = $(this);
      if ($this.hasClass("disabled") || !canAddIncumbent()) return;
      addNewCandidate(true);
    }

    function onClickContinueAddingNewPolitician() {
      var $this = $(this);
      if ($this.hasClass("disabled")) return;
      var $dialog = $('#duplicate-last-name-dialog');
      $dialog.dialog("close");
      addNewCandidate(false, $dialog.data("mainId"));
    }

    function onClickDuplicatePolitician() {
      var $this = $(this);
      if ($this.hasClass("selected")) {
        $this.removeClass("selected");
      } else {
        $("#duplicate-last-name-dialog .search-results-container .selected").removeClass("selected");
        $this.addClass("selected");
      }
      enableUsePoliticianFromListButton();
    }

    function onClickUsePoliticianFromList() {
      var $this = $(this);
      if ($this.hasClass("disabled")) return;
      if (getMode() === "AddPoliticians") {
        // button relabelled as "Cancel Addition
        $("#duplicate-last-name-dialog").dialog("close");
      }
      var key = getDuplicatePoliticianKey();
      if (!key) return;
      var mainId = $('#duplicate-last-name-dialog').dialog("close").data("mainId");
      clearAddNewPolitician();
      if (mainId)
        addRunningMate(key, mainId, false);
      else
        addCandidate(key, false);
    }

    //
    // Edit Candidate Dialog
    //

    function initEditPoliticianDialog() {
      // get rid of existing dialog
      $(".edit-candidate.ui-dialog").remove();

      $('#edit-politician-dialog').dialog({
        autoOpen: false,
        width: 880,
        resizable: false,
        dialogClass: 'edit-candidate overlay-dialog',
        // custom open and close to fix various problems
        open: onOpenEditCandidateDialog,
        close: function () {
          editPoliticianDialogLoaded = false;
          master.onCloseJqDialog();
          focusAddCandidateTabs();
        },
        modal: true
      });

      $('#edit-politician-dialog').on("propertychange change click keyup input paste", ".input-element.nickname input", function () {
        var $this = $(this);
        var val = $this.val();
        var match = /^\s*['"“‘]?(.*?)['"”’]?\s*$/.exec(val);
        if (match != null && val !== match[1]) $this.val(match[1]);
      });

      $('#edit-politician-dialog').on("propertychange change click keyup input paste", ".input-element.lname input", function () {
        var $this = $(this);
        var val = $this.val();
        var match = /^\s*(.*?),?\s*$/.exec(val);
        if (match != null && val !== match[1]) $this.val(match[1]);
      });
    }

    var editPoliticianDialogLoaded;

    function afterUpdateContainerEditPolitician() {
      $("#edit-politician-dialog .date-picker-dob").datepicker({
        changeYear: true,
        yearRange: "-90:+0"
      });
      var container = $(".candidate-html").children();
      if (container.length === 1) {
        var candidate = findCandidate($(".candidate-to-edit").val());
        if (candidate) {
          candidate.html($(container[0]).html());
          initCandidates(candidate);
        }
      }
    }

    function editCandidate(id) {
      $('.candidate-to-edit').val(id);
      if (getMode() !== "AddPoliticians") {
        var $candidate = findCandidate(id);
        var mainId = "";
        if ($candidate.hasClass("mate")) {
          mainId = $candidate.parents("li")[0].id;
          mainId = mainId.substr(mainId.lastIndexOf('-') + 1);
        }
        $('.main-candidate-if-running-mate').val(mainId);
      }
      $('#edit-politician-dialog').dialog('open');
      return false;
    }

    function findCandidate(politicianKey) {
      var result = null;
      $.each($(".candidates-list").children(), function () {
        var id = this.id;
        if (id && id.length >= politicianKey.length &&
      politicianKey === id.substr(id.length - politicianKey.length)) {
          result = $(".main", this);
          return false;
        }
        if ($(".mate-id", this).val() === politicianKey) {
          result = $(".mate", this);
          return false;
        }
      });
      return result;
    }

    function initRequestEditPolitician(group) {
      if ($$(group.button).hasClass("validated")) {
        $$(group.button).removeClass("validated");
        return true;
      }
      // warn if no party is selected
      var partyKeyGroup = null;
      $.each(group.children, function () {
        if (this.group === "mc-group-editpolitician-partykey") {
          partyKeyGroup = this;
          return false;
        }
      });

      if (partyKeyGroup && (partyKeyGroup._current === "X" || !partyKeyGroup._current)) {
        util.confirm("There is no political party selected for this " + getModeDescription() + ".\n\n" +
          "Continue anyway?",
             function (button) {
               if (button === "Ok") {
                 $$(group.button).addClass("validated").click();
               }
             });
        return false;
      }

      return true;
    }

    function onOpenEditCandidateDialog() {
      var $dialog = $("#edit-politician-dialog");
      $(".content-area", $dialog).css("visibility", "hidden");
      monitor.clearGroupFeedback("mc-group-editpolitician");
      monitor.clearGroupFeedback("mc-group-addcandidates");
      master.onOpenJqDialog.apply(this);
      editPoliticianDialogLoaded = false;
      //reloadPanel("edit-politician-dialog");
      $('.reloading', $dialog).val("reloading");
      $('input.update-button', $dialog).addClass("reloading").click();
    }

    //
    // Duplicate Last Name Dialog
    //

    function initDuplicateLastNameDialog() {
      // get rid of existing dialog
      $(".duplicate-last-name-dialog.ui-dialog").remove();

      $('#duplicate-last-name-dialog').dialog({
        autoOpen: false,
        width: 500,
        height: "auto",
        resizable: false,
        dialogClass: 'duplicate-last-name-dialog overlay-dialog',
        title: "Potential Duplicate Politician",
        modal: true
      });

    }

    //
    // Consolidate Policicians Dialog
    //

    function initConsolidatePoliticiansDialog() {
      // get rid of existing dialog
      $(".consolidate-politicians-dialog.ui-dialog").remove();
      var $dialog = $('#consolidate-politicians-dialog');
      $dialog.dialog({
        autoOpen: false,
        width: 880,
        resizable: false,
        title: 'Consolidate Politicians',
        dialogClass: 'consolidate-politicians-dialog overlay-dialog',
        position: { my: "top", at: "top+10" },
        // custom open and close to fix various problems
        open: onOpenConsolidatePoliticiansDialog,
        close: function () {
          master.onCloseJqDialog();
        },
        modal: true
      });
    }

    function afterRequestConsolidate() {
      var $dialog = $('#consolidate-politicians-dialog');
      var reloaded = $(".reloaded", $dialog).val();
      if (reloaded === "reloaded") return;
      $('#consolidate-politicians-dialog').dialog("close");
      fp.refresh(true);
      var message = reloaded === "error"
        ? "An enexpected error occurred. Please check your results."
        : "The consolidation is complete.";
      util.alert(message);
    }

    function afterUpdateContainerConsolidate() {
      var $dialog = $('#consolidate-politicians-dialog');

      $("#consolidate-tabs", $dialog).tabs();

      $(".pick-politician .search-politician").click(function () {
        var $thisItem = $(this);
        var thisIndex = $thisItem.closest(".consolidate-item").hasClass("consolidate-item-1") ? "1" : "2";
        var otherIndex = thisIndex === "1" ? "2" : "1";
        var $otherItem = $(".consolidate-item-" + otherIndex + " .search-politician", $dialog);
        $thisItem.removeClass("not-selected").addClass("selected");
        $otherItem.removeClass("selected").addClass("not-selected");

        // run through all databoxes and select all for the clicked politician unless it has no-content
        // and the other one has content
        $(".databoxes", $dialog).each(function() {
          var $this = $(this);
          var select = ".databox-" + thisIndex;
          var unselect = ".databox-" + otherIndex;
          if ($(select, $this).hasClass("no-content") && !$(unselect, $this).hasClass("no-content")) {
            // swap
            var t = select;
            select = unselect;
            unselect = t;
          }
          $(select, $this).addClass("selected").removeClass("not-selected");
          $(unselect, $this).addClass("not-selected").removeClass("selected");
        });
      });

      $(".databox", $dialog).click(function() {
        var $thisItem = $(this);
        var thisIndex = $thisItem.hasClass("databox-1") ? "1" : "2";
        var otherIndex = thisIndex === "1" ? "2" : "1";
        var $otherItem = $thisItem.parent().find(".databox-" + otherIndex);
        $thisItem.removeClass("not-selected").addClass("selected");
        $otherItem.removeClass("selected").addClass("not-selected");
      });

      $(".response-box", $dialog).click(function () {
        $(this).toggleClass("selected");
      });
    }

    function initRequestConsolidate(group) {
      var validated = false;
      if ($$(group.button).hasClass("validated")) {
        $$(group.button).removeClass("validated");
        validated = true;
      }
      var $dialog = $('#consolidate-politicians-dialog');
      var $selected = $(".search-politician.selected", $dialog);
      if ($selected.length !== 1) {
        util.alert("Please select a politician to consolidate into.");
        return false;
      }

      // get all selected databox's and buuld an object with the selected index value
      var selectedData = {};
      $(".databox.selected", $dialog).each(function () {
        var $this = $(this);
        $this.closest(".databoxes").classes(function (className) {
          if (className.substr(className.length - 10) === "-databoxes") {
            selectedData[className.substr(0, className.length - 10)] =
              ($this.hasClass("databox-1") ? "1" : "2");
          }
        });
      });
      $(".consolidate-selected-data", $dialog).val(JSON.stringify(selectedData));

      //build an array with all selected issue answers
      var selectedResponses = [];
      $(".response-box.selected").each(function () {
        selectedResponses.push($(this).data("key"));
      });
      $(".consolidate-selected-responses", $dialog).val(JSON.stringify(selectedResponses));

      var $item = $selected.closest(".consolidate-item");
      var selectedIndex = $item.hasClass("consolidate-item-1") ? "1" : "2";
      $(".consolidate-selected-index", $dialog).val(selectedIndex);

      if (!validated) {
        util.confirm("You are about to consolidate all selected data items into to the " +
          (selectedIndex === "1" ? "1st" : "2nd") + " politician (the one on the " +
          (selectedIndex === "1" ? "left" : "right") + ". Click \"Ok\" to continue.",
          function (button) {
            if (button === "Ok") {
              $$(group.button).addClass("validated").click();
            }
          });
        return false;
      }
      
      return true;
    }

    function onClickConsolidatePoliticians() {
      if ($("#main-tabs .consolidate-politicians-button").hasClass("disabled")) return;
      var $dialog = $('#consolidate-politicians-dialog');
      $dialog.dialog("open");
    }

    function onOpenConsolidatePoliticiansDialog() {
      var $dialog = $("#consolidate-politicians-dialog");
      var keys = fp.getSelectedCandidateKeys();
      $(".consolidate-key-1", $dialog).val(keys[0]);
      $(".consolidate-key-2", $dialog).val(keys[1]);
      $(".content-area", $dialog).css("visibility", "hidden");
      monitor.clearGroupFeedback("mc-group-consolidate");
      master.onOpenJqDialog.apply(this);
      $('.reloading', $dialog).val("reloading");
      $('input.update-button', $dialog).addClass("reloading").click();
    }

    //
    // Expose public methods
    //

    return {
      initControl: initControl,
      setCandidateBackground: setCandidateBackground
    };

  });