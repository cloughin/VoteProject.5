define(["jquery", "vote/adminMaster", "vote/util", "monitor",
  "vote/controls/navigateJurisdiction", "vote/controls/electionControl",
  "vote/controls/officeControl", "vote/controls/managePoliticiansPanel",
    "jqueryui", "slimscroll", "dynatree"],
  function ($, master, util, monitor, navigateJurisdiction, electionControl, officeControl,
   managePoliticiansPanel) {

    var $$ = util.$$;
    var queryOffice;

    //
    // Monitor callbacks
    //

    function afterRequest(group) {
      switch (group.group) {
        case "mc-group-addballotmeasures":
          afterRequestAddBallotMeasures();
          break;
      }
    }

    function afterUndo(group) {
      switch (group.group) {
        case "mc-group-statedefaults":
          stateDefaultsActionCompleted("Changes on this panel were undone.");
          break;

        case "mc-group-changeinfo":
          electionInfoActionCompleted("Changes on this panel were undone.");
          break;

        case "mc-group-changedeadlines":
          electionDeadlinesActionCompleted("Changes on this panel were undone.");
          break;

        case "mc-group-addoffices":
          officeListActionCompleted("Changes to the office list were undone.");
          break;

        case "mc-group-identifywinners":
          identifyWinnersActionCompleted("Changes to the office winners were undone.");
          break;

        case "mc-group-identifywinnersbeta":
          identifyWinnersBetaActionCompleted("Changes to the office winners were undone.");
          break;

        case "mc-group-addballotmeasures":
          addBallotMeasuresActionCompleted("Changes to the ballot measure were undone.");
          break;

        case "mc-group-masteronly":
          $('.election-key-to-copy').val("");
          break;
      }
    }

    function afterUpdateContainer(group) {
      if (!group) return;

      $(".date-picker", $$(group.container)).datepicker({
        changeYear: true,
        yearRange: "2010:+1"
      });

      switch (group.group) {
        case "mc-group-addelection":
          afterUpdateContainerAddElection();
          break;

        case "mc-group-statedefaults":
          afterUpdateContainerStateDefaults();
          break;

        case "mc-group-changeinfo":
          afterUpdateContainerChangeInfo();
          break;

        case "mc-group-electioncontrol":
          initElectionControl(); ;
          break;

        case "mc-group-selectballotmeasure":
          afterUpdateContainerSelectBallotMeasure();
          break;

        case "mc-group-addballotmeasures":
          afterUpdateContainerAddBallotMeasures();
          break;

        case "mc-group-masteronly":
          afterUpdateContainerMasterOnly();
          break;
      }
    }

    function afterUpdateGroup(group, args) {
      if (!group) return;
      switch (group.group) {
        case "mc-group-addballotmeasures-referendumdesc":
        case "mc-group-addballotmeasures-referendumdetail":
        case "mc-group-addballotmeasures-referendumfulltext":
          enableBallotMeasureRemoveLineBreaks(group);
          // the timeout is necessary to allow the browser ui to update
          setTimeout(function () { setReferendumTextArea(group); }, 500);
          break;
      }
    }

    function clientChange(group, args) {
      switch (group.group) {
        case "mc-group-addelection":
        case "mc-group-statedefaults":
        case "mc-group-changeinfo":
        case "mc-group-changedeadlines":
        case "mc-group-addoffices":
        case "mc-group-identifywinners":
        case "mc-group-identifywinnersbeta":
          monitor.clearGroupFeedback(group.group);
          break;

        case "mc-group-addelection-electiondate":
          loadElectionsToCopyOfficesFrom();
          break;

        case "mc-group-addballotmeasures-referendumtitle":
          removeLineBreaks(group);
          break;

        case "mc-group-addballotmeasures-referendumdesc":
        case "mc-group-addballotmeasures-referendumdetail":
        case "mc-group-addballotmeasures-referendumfulltext":
          enableBallotMeasureRemoveLineBreaks(group);
          break;

        case "mc-group-masteronly-generalelectiondate":
          updateGeneralElectionDesc(group);
      }
    }

    function initDynatree($o) {
      if ($o.hasClass("add-offices-control"))
        initAddOfficesTree($o);
      else if ($o.hasClass("identify-winners-control"))
        initIdentifyWinnersTree($o);
      else if ($o.hasClass("identify-winners-beta-control"))
        initIdentifyWinnersBetaTree($o);
    }

    function initPage() {
      monitor.registerCallback("afterRequest", afterRequest);
      monitor.registerCallback("afterUndo", afterUndo);
      monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);
      monitor.registerCallback("afterUpdateGroup", afterUpdateGroup);
      monitor.registerCallback("clientChange", clientChange);
      monitor.registerCallback("initRequest", initRequest);
      monitor.init({ initDynatree: initDynatree });

      util.safeBind($(".jurisdiction-change-button"), "click",
        navigateJurisdiction.changeJurisdictionButtonClicked);

      window.onbeforeunload = function () {
        var changedGroups = monitor.getChangedGroupNames(true);
        var inx;
        // we don't worry about changes on the add election page or in
        // the add candidate sub-tab
        if ((inx = $.inArray("mc-group-addelection", changedGroups)) >= 0)
          changedGroups.splice(inx, 1);
        if ((inx = $.inArray("mc-group-addnewcandidate", changedGroups)) >= 0)
          changedGroups.splice(inx, 1);
        if (changedGroups.length > 0)
          return "There are entries on your form that have not been submitted";
      };

      $(".date-picker").datepicker({
        changeYear: true,
        yearRange: "2010:+1"
      });

      initElectionControl();
      $$('main-tabs')
        .safeBind("tabsbeforeactivate", onTabsBeforeActivate)
        .safeBind("tabsactivate", onTabsActivate);
      $$('add-candidate-tabs')
        .safeBind("tabsbeforeactivate", function (event) { event.stopPropagation(); })
        .safeBind("tabsactivate", function (event) { event.stopPropagation(); });
      afterUpdateContainerAddElection();

      queryOffice = $.queryString("office");
      officeControl.initControl({
        onToggle: function () {
          electionControl.toggleElectionControl(false);
        },
        onSelect: function (flag, dtnode) {
          if (dtnode.data.href)
            window.location.href = dtnode.data.href;
          else
            reloadPanel("tab-addcandidates", "loadoffice");
        },
        officeKey: queryOffice
      });

      managePoliticiansPanel.initControl(getSelectedElection);

      $("#tab-viewreport .get-report-button").safeBind("click", onClickGetReport);

      var queryElection = $.queryString("election");
      $('.selected-election-key').val(queryElection || "");
      if (queryElection) // force tab change => reload
      {
        $$('main-tabs')
          .tabs("option", "collapsible", true)
          .tabs("option", "active", false);
      }

      var defaultTab = "tab-changeinfo";
      var tabInfo = master.parseFragment(defaultTab, function (info) {
        if (queryElection && queryOffice) {
          if (info.tab === "tab-identifywinners" || info.tab === "tab-identifywinnersbeta")
            info.tab = isPrimaryElection(queryElection) ? "tab-identifywinnersbeta" : "tab-identifywinners";
          else
            info.tab = "tab-addcandidates";
        }
      });
      if (tabInfo.tab === "tab-masteronly")
        masterOnlyStartTab = tabInfo.subTab;

      var electionKey = $.queryString("election");
      if (!electionKey) {
        var stateElectionKey = window.sessionStorage.getItem("updateElections.stateElectionKey");
        var matchingKeys = [];
        if (stateElectionKey) {
          $.each(electionControl.getElectionKeys(), function () {
            var key = this.toString();
            if (key.substr(0, 12) === stateElectionKey)
              matchingKeys.push(key);
          });
          if (matchingKeys.length === 1)
            electionKey = matchingKeys[0];
        }
      }
      if (electionKey) {
        electionControl.changeElection(electionKey);
      } else afterElectionChanged();

      $("#tab-identifywinnersbeta").on("mousedown", ".dropdowns select", onMouseDownWinnersBetaSelect);
      $(".winners-tab").on("mousedown", ".next-button", onMouseDownWinnersNextButton);
      $(".winners-tab").on("change", ".runoff-checkbox", onChangeWinnersRunoffCheckbox);
    }

    function initRequest(group) {
      if (!group) return true;
      switch (group.group) {
        case "mc-group-addelection":
          return initRequestAddElection(group);

        case "mc-group-addoffices":
          return initRequestAddOffices(group);

        case "mc-group-masteronly":
          return initRequestMasterOnly(group);
      }
      return true;
    }

    //
    // Election Control
    //

    var onElectionChangingAlternate = null;

    function initElectionControl() {
      electionControl.init({
        electionKey: $('.selected-election-key').val(),
        onSelectedElectionChanging: onSelectedElectionChanging,
        onSelectedElectionChanged: onSelectedElectionChanged,
        onToggleElectionControl: onToggleElectionControl,
        slimScrollOptions: {
          height: '584px',
          width: '200px',
          alwaysVisible: true,
          color: '#666',
          size: '12px'
        }
      });
    }

    function getSelectedElection() {
      return $('.selected-election-key').val();
    }

    function onSelectedElectionChanging(newElectionKey) {
      if (typeof onElectionChangingAlternate === "function") {
        onElectionChangingAlternate(newElectionKey);
        return false;
      }
      var isChanged = monitor.isPanelsChanged($(".mc-container", getCurrentTabPanel()));
      if (isChanged) {
        util.confirm("There are unsaved changes on this panel.\n\n" +
          "Click OK to discard the changes load the new election.\n" +
          "Click Cancel to return to the changed panel.",
          function (button) {
            if (button === "Ok")
              electionControl.changeElection(newElectionKey);
            else
              electionControl.toggleElectionControl(false);
          });
        return false;
      }
      return true;
    }

    function onSelectedElectionChanged(newElectionKey) {
      $('.selected-election-key').val(newElectionKey);
      electionControl.toggleElectionControl(false);
      afterElectionChanged();
      reloadPanel(getCurrentTabPanel()[0].id);
    }

    function afterElectionChanged() {
      var electionKey = getSelectedElection();
      if (electionKey && util.sessionStorageIsAvailable()) {
        window.sessionStorage.setItem("updateElections.stateElectionKey", electionKey.substr(0, 12));
      }
      var isPrimary = isPrimaryElection(electionKey);
      var isGeneral = !isPrimary && !!electionKey;
      $(".adjust-incumbents-tab").toggleClass("hidden", !isGeneral);
      $(".primary-winners-tab").toggleClass("hidden", !isPrimary);
      $(".general-winners-tab").toggleClass("hidden", !isGeneral);
      var panel = getCurrentTabPanel();
      if (panel && panel.length) {
        var panelId = panel[0].id;
      }
      if (isPrimary && panelId === "tab-identifywinners" || isGeneral && panelId === "tab-identifywinnersbeta") {
        $$('main-tabs').tabs("option", "active", util.getTabIndex("main-tabs", "tab-changeinfo"));
      }
    }

    function onToggleElectionControl(show) {
      onElectionChangingAlternate = null;
    }

    //
    // Add Election
    //

    function afterUpdateContainerAddElection() {
      util.safeBind($(".add-election-election-type-dropdown"), "change",
        addElectionElectionTypeDropDownChanged);
      util.safeBind($(".add-election-national-party-dropdown"), "change",
        loadElectionsToCopyOfficesFrom);
      util.safeBind($(".add-election-copy-offices-dropdown"), "change",
        onChangeAddElectionCopyOffices);
      addElectionElectionTypeDropDownChanged();
    }

    function addElectionElectionTypeDropDownChanged() {
      // Adjust the party dropdown when the election type changes
      var type = $('.add-election-election-type-dropdown').val();
      var isPrimary = isPrimaryType(type);
      var isPresidentialPrimary = "AB".indexOf(type) >= 0;
      var isPresidentialComparison = "A".indexOf(type) >= 0;
      var isStatePrimary = 'PQ'.indexOf(type) >= 0;
      var isRunoff = 'QR'.indexOf(type) >= 0;
      var party = $('.add-election-national-party-dropdown').val();
      var canIncludePresidentialCandidates = "PB".indexOf(type) >= 0;
      var $partyDropdown = $('.add-election-national-party-dropdown');
      $.each($('option', $partyDropdown),
        function () {
          switch ($(this).val()) {
            case 'A':
              $(this).prop('disabled', isPrimary && !isPresidentialComparison);
              break;

            case 'X':
              $(this).prop('disabled', !isStatePrimary);
              break;

            default:
              $(this).prop('disabled', !isPrimary);
              break;
          };
        });
      $('#tab-addelection .primaries').toggleClass('hidden', !isPrimary);
      $('#tab-addelection .not-for-runoff').toggleClass('hidden', isRunoff);
      $('#tab-addelection .includepresident').toggleClass('hidden', !isStatePrimary || isRunoff);
      $('#tab-addelection .includepresidentcandidates').toggleClass('hidden', !canIncludePresidentialCandidates);
      $partyDropdown.prop('disabled', !isPrimary);
      if (!isPrimary)
        $partyDropdown.val('A');
      else if (isPresidentialPrimary && "AX".indexOf(party) >= 0)
        $partyDropdown.val(' ');
      else if (isStatePrimary && party === 'A')
        $partyDropdown.val(' ');
      loadElectionsToCopyOfficesFrom();
    }

    function getPartyPrimariesWithOfficesSucceeded(result) {
      if (result.d.length) {
        var dropdown = $('.add-election-copy-offices-dropdown');
        dropdown.find('option')
          .remove();
        dropdown.append('<option value=" ">--- Select an election to copy offices from ---</option>')
          .attr("selected", "selected");
        $.each(result.d, function () {
          dropdown.append(new Option(this.Text, this.Value));
        });
        dropdown.append('<option value=" ">Do not copy offices</option>')
          .attr("selected", "selected");
      } else getPartyPrimariesWithOfficesFailed(result);
    }

    function getPartyPrimariesWithOfficesFailed(result) {
      $('.add-election-copy-offices-dropdown')
        .find('option')
        .remove()
        .end()
        .append('<option value=" ">No primaries with offices found on this date</option>')
        .attr("selected", "selected");
    }

    function initRequestAddElection(group) {
      if ($$(group.button).hasClass("validated")) {
        $$(group.button).removeClass("validated");
        return true;
      }
      var isStatePrimary = $('.add-election-election-type-dropdown').val() === 'P';
      if (isStatePrimary) {
        // warn for potential problems
        // build key starter

        var electionDate = new Date($('.mc-group-addelection-electiondate').val());
        if (isNaN(electionDate.getTime())) return true; // bad date, let server-side handle it
        var keyStarter = $('.client-state-code').val() +
          electionDate.getFullYear() +
          (electionDate.getMonth() + 101).toString().substr(1) +
          (electionDate.getDate() + 100).toString().substr(1) +
          "P";
        var electionKeys = electionControl.getElectionKeys();
        var parties = "";
        $.each(electionKeys, function () {
          if (this.toString().substr(0, 11) === keyStarter)
            parties += this.toString().substr(11);
        });
        var thisParty = $('.add-election-national-party-dropdown').val();
        if (parties.indexOf(thisParty) >= 0) return true; // dup, let server-side handle
        var warning = null;
        if (thisParty == "X") {
          // warn if any other primaries on date
          if (parties.length)
            warning = "There are already party primaries on the same day. Are you sure you want to add a non-partisan primary?";
        }
        else if (parties.indexOf("X") >= 0) {
          // warn if anon-partisan primary on date on date
          warning = "There is already a non-partisan primary on the same da. Are you sure you want to add a party primary?";
        }
        if (!warning) return true;
        util.confirm(warning,
          function (button) {
            if (button === "Ok") {
              $$(group.button).addClass("validated").click();
            }
          });
        return false; // cancels the request
      }
      return true;
    }

    function loadElectionsToCopyOfficesFrom() {
      var isStatePrimary = $('.add-election-election-type-dropdown').val() === 'P';
      var canCopyOffices = isStatePrimary; // && isRealParty;
      $('#tab-addelection .copyoffices').toggleClass('hidden', !canCopyOffices);
      $('#tab-addelection .copyofficescandidates').toggleClass('hidden', !canCopyOffices);
      if (canCopyOffices) {
        util.ajax({
          url: "/Admin/WebService.asmx/GetPartyPrimariesWithOffices",
          data: {
            stateCode: $('.client-state-code').val(),
            electionDate: $('.mc-group-addelection-electiondate').val()
          },
          success: getPartyPrimariesWithOfficesSucceeded,
          error: getPartyPrimariesWithOfficesFailed
        });
      }
    }

    function onChangeAddElectionCopyOffices() {
      $('.add-election-copy-offices-hidden').val($('.add-election-copy-offices-dropdown').val());
    }

    //
    // State Defaults
    //

    function afterUpdateContainerStateDefaults() {
      util.setResizableVertical($("#tab-statedefaults textarea.is-resizable"));
      master.initDisclaimerButtons($("#tab-statedefaults"));
    }

    function stateDefaultsActionCompleted(action) {
      var isChanged = monitor.isPanelsChanged($(".mc-container", getCurrentTabPanel()));
      if (isChanged)
        action += " Click update to save the state defaults.";
      monitor.addGroupFeedback("mc-group-statedefaults", "ok", action, true);
    }

    //
    // Election Information
    //

    function afterUpdateContainerChangeInfo() {
      util.setResizableVertical($("#tab-changeinfo textarea.is-resizable"));
      master.initDisclaimerButtons($("#tab-changeinfo"));
    }

    function electionInfoActionCompleted(action) {
      var isChanged = monitor.isPanelsChanged($(".mc-container", getCurrentTabPanel()));
      if (isChanged)
        action += " Click update to save the election information.";
      monitor.addGroupFeedback("mc-group-changeinfo", "ok", action, true);
    }

    //
    // Election Deadlines
    //

    function electionDeadlinesActionCompleted(action) {
      var isChanged = monitor.isPanelsChanged($(".mc-container", getCurrentTabPanel()));
      if (isChanged)
        action += " Click update to save the new election information.";
      monitor.addGroupFeedback("mc-group-changedeadlines", "ok", action, true);
    }

    //
    // Add Offices Control
    //

    function initAddOfficesTree($o) {
      if (!$('ul', $o).hasClass("dynatree-container")) {
        $.ui.dynatree.nodedatadefaults["icon"] = false; // Turn off icons by default
        $o.dynatree({
          imagePath: "/images/dynatree/skin-vista",
          checkbox: true,
          selectMode: 3,
          autoCollapse: true,
          noLink: true,
          onClick: function (node, event) {
            if (node.getEventTargetType(event) === "title")
              if (node.countChildren() === 0)
                if (node.data.href)
                  window.location.href = node.data.href;
                else
                  node.toggleSelect();
              else
                node.toggleExpand();
          },
          onSelect: function (flag, dtnode) {
            // get the previous value by inverting the just-changed node
            var prevKeys = [];
            this.visit(function (node) {
              if (node.countChildren() === 0 &&
              (node === dtnode ? !node.isSelected() : node.isSelected()))
                prevKeys.push(node.data.key);
            });
            monitor.dataChanged({ target: this.$tree[0] }, prevKeys.join("|"));
            if (!this._undoing)
              if (dtnode.isSelected())
                officeListActionCompleted("Office was added.");
              else
                officeListActionCompleted("Office was removed.");
          },
          classNames: {
            container: "dynatree-container shadow-2"
          }
        });
      }
    }

    function initRequestAddOffices(group) {
      if ($$(group.button).hasClass("validated")) {
        $$(group.button).removeClass("validated");
        return true;
      }
      if (group.children) {
        // warn if we are removing offices with candidates
        // find the officelist monitor group
        var officelistGroup = null;
        $.each(group.children, function () {
          if (this.group === "mc-group-addoffices-officelist") {
            officelistGroup = this;
            return false;
          }
        });

        if (officelistGroup) {
          var original = officelistGroup.val.split("|");
          if (original.length === 1 && !original[0]) original = [];
          var $data = $$(officelistGroup.data);
          var updated = officelistGroup._dataType.val($data)
            .split("|");
          if (updated.length === 1 && !updated[0]) updated = [];
          var warnings = [];
          var tree = $data.dynatree("getTree");
          // Look at each office in the original tree.
          // If it's not in the updated tree it is being removed.
          // If it has candidates, add it to the warning list
          $.each(original, function (index, officeKey) {
            if ($.inArray(officeKey, updated) < 0) {
              var node = tree.getNodeByKey(officeKey);
              if (node.data.candidates) {
                var title = node.parent.data.title || '';
                if (title) title += " ";
                title += node.data.title;
                warnings.push(title);
              }
            }
          });

          if (warnings.length) {
            var officeList = warnings.join("\n");
            util.confirm("You are about to remove offices from this election that have" +
             " candidates entered:\n\n" + officeList + "\n\nContinue?",
             function (button) {
               if (button === "Ok") {
                 $$(group.button).addClass("validated").click();
               }
             });
            return false; // cancels the request
          }
        }
      }
      return true;
    }

    function officeListActionCompleted(action) {
      var isChanged = Monitor.isPanelsChanged($(".mc-container", getCurrentTabPanel()));
      if (isChanged)
        action += " Click update to save the new list of offices.";
      monitor.addGroupFeedback("mc-group-addoffices", "ok", action, true);
    }

    //
    // Identify Winners Control
    //

    function afterSelectedWinnerBetaChanged($dropdowns) {
      // collect all selected items
      var selectedVals = [];
      var $selects = $(".winners-dropdowns select", $dropdowns);
      $selects.each(function () {
        var val = $(this).val();
        if (val) selectedVals.push(val);
      });
      // hide selected items in other selects
      $selects.each(function () {
        var $this = $(this);
        var val = $this.val();
        $("option", $this).each(function () {
          var $opt = $(this);
          var opt = $opt.val();
          $opt.toggleClass("hidden", opt !== val && $.inArray(opt, selectedVals) >= 0);
        });
      });
    }

    function identifyWinnersActionCompleted(action) {
      var isChanged = monitor.isPanelsChanged($(".mc-container", getCurrentTabPanel()));
      if (isChanged)
        action += " Click update to save the new list of winners.";
      monitor.addGroupFeedback("mc-group-identifywinners", "ok", action, true);
    }

    function identifyWinnersBetaActionCompleted(action) {
      var isChanged = monitor.isPanelsChanged($(".mc-container", getCurrentTabPanel()));
      if (isChanged)
        action += " Click update to save the new list of winners.";
      monitor.addGroupFeedback("mc-group-identifywinnersbeta", "ok", action, true);
    }

    function initIdentifyWinnersTree($o) {
      if (!$('ul', $o).hasClass("dynatree-container")) {
        $.ui.dynatree.nodedatadefaults["icon"] = false; // Turn off icons by default
        $o.dynatree({
          imagePath: "/images/dynatree/skin-vista",
          checkbox: true,
          selectMode: 3,
          minExpandLevel: 2,
          autoCollapse: true,
          noLink: true,
          onClick: function (node, event) {
            if (node.getEventTargetType(event) === "title")
              if (node.countChildren() === 0)
                node.toggleSelect();
              else
                node.toggleExpand();
          },
          onSelect: function (flag, dtnode) {
            monitor.dataChanged({ target: this.$tree[0] }, null);
            if (!this._undoing)
              if (dtnode.isSelected())
                identifyWinnersActionCompleted("Office was selected for incumbent update.");
              else
                identifyWinnersActionCompleted("Office was removed from incumbent update.");
          },
          onExpand: function (flag, node) {
            if (flag) {
              var $selects = $("select", $("." + node.data.key).parent());
              $.each($selects, function () {
                var $this = $(this);
                util.safeBind($this, "focus", function () { $(this).data("prev", $(this).val()); });
                util.safeBind($this, "change", onSelectedWinnerChanged);
              });
              var o = new IdentifyWinnersTree();
              o.restoreOfficeClassNode(node);
            }
          },
          classNames: {
            container: "dynatree-container shadow-2"
          }
        });
        // The following is so we can access the dropdowns of all nodes
        $o.dynatree("getTree").renderInvisibleNodes();
        $("select", $o)
          .safeBind("focus", function () { $(this).data("prev", $(this).val()); })
          .safeBind("change", onSelectedWinnerChanged);
      }
    }

    function initIdentifyWinnersBetaTree($o) {
      if (!$('ul', $o).hasClass("dynatree-container")) {
        $.ui.dynatree.nodedatadefaults["icon"] = false; // Turn off icons by default
        $o.dynatree({
          imagePath: "/images/dynatree/skin-vista",
          checkbox: true,
          selectMode: 3,
          minExpandLevel: 2,
          autoCollapse: true,
          noLink: true,
          onClick: function (node, event) {
            if (node.getEventTargetType(event) === "title")
              if (node.countChildren() === 0)
                node.toggleSelect();
              else
                node.toggleExpand();
          },
          onSelect: function (flag, dtnode) {
            monitor.dataChanged({ target: this.$tree[0] }, null);
            if (!this._undoing)
              if (dtnode.isSelected())
                identifyWinnersBetaActionCompleted("Office was selected for incumbent update.");
              else
                identifyWinnersBetaActionCompleted("Office was removed from incumbent update.");
          },
          onExpand: function (flag, node) {
            if (flag) {
              var $selects = $("select", $("." + node.data.key).parent());
              $.each($selects, function () {
                var $this = $(this);
                util.safeBind($this, "focus", function () { $(this).data("prev", $(this).val()); });
                util.safeBind($this, "change", onSelectedWinnerBetaChanged);
              });
              var o = new IdentifyWinnersBetaTree();
              o.restoreOfficeClassNode(node);
            }
          },
          classNames: {
            container: "dynatree-container shadow-2"
          }
        });
        // The following is so we can access the dropdowns of all nodes
        $o.dynatree("getTree").renderInvisibleNodes();
        //$o.dynatree("getTree").visit(function(node) { node.render(); });
        $("select", $o)
          .safeBind("focus", function () { $(this).data("prev", $(this).val()); })
          .safeBind("change", onSelectedWinnerBetaChanged);
      }
    }

    function onChangeWinnersRunoffCheckbox() {
      var checked = $(this).prop("checked");
      var $context = $(this).closest(".dropdowns");
      $(".runoff-dropdown", $context).toggleClass("hidden", !checked);
      $(".winners-dropdowns", $context).toggleClass("hidden", checked);
      var $control = $context.closest(".identify-winners-control,.identify-winners-beta-control");
      monitor.dataChanged({ target: $control[0] }, null);
    }

    function onMouseDownWinnersBetaSelect() {
      //afterSelectedWinnerBetaChanged($(this).closest(".dropdowns"));
    }

    function onMouseDownWinnersNextButton(event) {
      event.preventDefault();
      var $context = $(this).closest(".winners-tab")
        .find(".identify-winners-control,.identify-winners-beta-control");
      if ($(this).hasClass("disabled")) return;
      var $labels = $(".label", $context);
      if (!$labels.length) {
        $(this).addClass("disabled");
        return;
      }
      var $reds = $labels.filter(".red");
      $reds.removeClass("red");
      var startInx = $reds.length ? $labels.index($reds[0]) : -1;
      var $nextIncomplete = null;
      var inxToCheck = startInx + 1;
      while (true) {
        if (inxToCheck >= $labels.length) {
          if (startInx == -1) {
            //$(this).addClass("disabled");
            util.alert("All winners are marked.");
            break;
          }
          else inxToCheck = 0;
        }
        var $dropdowns = $($labels[inxToCheck]).next();
        if ($(".runoff-checkbox", $dropdowns).prop("checked")) {
          if ($(".runoff-dropdown", $dropdowns).text().indexOf('◄') < 0) {
            $nextIncomplete = $($labels[inxToCheck]);
          }
        } else {
          $dropdowns.find(".winners-dropdowns select").each(function () {
            if ($(this).text().indexOf('◄') < 0) {
              $nextIncomplete = $($labels[inxToCheck]);
              return false;
            }
          });
        }
        if ($nextIncomplete) {
          var $node = $nextIncomplete.closest(".dynatree-node");
          var node = $.ui.dynatree.getNode($node[0]);
          node.makeVisible();
          var $label = $(".label", $node).addClass("red");
          var $container = $node.closest(".dynatree-container");
          $container.scrollTo($label[0], { offsetTop: '360' });
          break;
        } else {
          inxToCheck++;
          if (inxToCheck == startInx + 1) break;
        }
      }
    }

    function checkForTooManyRunoffAdvancers($select) {
      var $dropdowns = $select.closest(".dropdowns");
      var $runoffCheckbox = $dropdowns.find(".runoff-checkbox");
      var positions = $runoffCheckbox.attr("rel");
      if ($runoffCheckbox.prop("checked") && positions) {
        var selected = $dropdowns.find(".runoff-dropdown option:selected").length;
        if (selected > positions)
          util.alert("The number of candidates selected (" + selected +
          ") exceeds the maximum number of runoff positions (" + positions + ").");
      }
    }

    function onSelectedWinnerChanged(event) {
      this.blur();
      var $this = $(this);
      checkForTooManyRunoffAdvancers($this);
      var duplicate = false;
      var $siblings = $this.siblings();
      $.each($siblings, function () {
        if ($(this).val() === $this.val()) {
          duplicate = true;
          return false;
        }
      });

      if (duplicate) {
        var dupName = $("option:selected", $this).text();
        while (" ◄□■".indexOf(dupName.substr(dupName.length - 1)) >= 0)
          dupName = dupName.substr(0, dupName.length - 1);
        util.alert("The candidate " + dupName + " is already selected for this office.");
        $(this).val($(this).data("prev"));
      } else {
        monitor.dataChanged({ target: $(".identify-winners-control")[0] }, null);
        identifyWinnersBetaActionCompleted("Winner was changed.");
      }
    }

    function onSelectedWinnerBetaChanged() {
      this.blur();
      var $this = $(this);
      checkForTooManyRunoffAdvancers($this);
      var duplicate = false;
      var $siblings = $this.siblings();
      $.each($siblings, function () {
        if ($(this).val() === $this.val()) {
          duplicate = true;
          return false;
        }
      });

      if (duplicate) {
        var dupName = $("option:selected", $this).text();
        while (" ◄□■".indexOf(dupName.substr(dupName.length - 1)) >= 0)
          dupName = dupName.substr(0, dupName.length - 1);
        util.alert("The candidate " + dupName + " is already selected for this office.");
        $this.val($this.data("prev"));
      } else {
        monitor.dataChanged({ target: $(".identify-winners-beta-control")[0] }, null);
        identifyWinnersBetaActionCompleted("Winner was changed.");
        $this.removeClass("bold");
        afterSelectedWinnerBetaChanged($this.closest(".dropdowns"));
      }
    }

    //
    // AdjustIncumbentsList Custom Data Type
    //

    AdjustIncumbentsList.prototype = new monitor.DataType();
    AdjustIncumbentsList.prototype.constructor = AdjustIncumbentsList;
    AdjustIncumbentsList.prototype.parent = monitor.DataType.prototype;
    // ReSharper disable once InconsistentNaming
    function AdjustIncumbentsList() { }

    AdjustIncumbentsList.prototype.name = "AdjustIncumbentsList";

    AdjustIncumbentsList.prototype.bindChange = function ($data) {
      $data.on("change", "input[type=checkbox]", monitor.dataChanged);
    };

    AdjustIncumbentsList.prototype.dataChanged = function (group, $data) {
      $data.prev().val(this.val($data));
    };

    AdjustIncumbentsList.prototype.handles = function ($data) {
      var o = $data[0];
      return o.tagName.toLowerCase() === "div" && $data.hasClass("adjust-incumbents-control");
    };

    AdjustIncumbentsList.prototype.parseVal = function (val) {
      var offices = val.split("|");
      if (offices.length === 1 && !offices[0]) offices = [];
      $.each(offices, function () {
        var split = this.split(":");
        offices[split[0]] = split[1].split(",");
      });
      return offices;
    };

    AdjustIncumbentsList.prototype.unbindChange = function ($data) {
      $data.off("change", "input[type=checkbox]", monitor.dataChanged);
    };

    AdjustIncumbentsList.prototype.val = function ($data, value) {
      if (typeof (value) === "undefined") {
        var officeEntries = [];
        $data.find(".office").each(function () {
          var $this = $(this);
          var incumbentEntries = [];
          $this.find(".incumbent:checked").each(function () {
            incumbentEntries.push($(this).attr("value"));
          });
          officeEntries.push($this.attr("rel") + ":" + incumbentEntries.join(","));
        });
        return officeEntries.join("|");
      } else {
        var offices = this.parseVal(value);
        $data.find(".office").each(function () {
          var $this = $(this);
          var incumbents = offices[$this.attr("rel")];
          $this.find(".incumbent").each(function () {
            var $this2 = $(this);
            $this2.prop("checked", $.inArray($this2.attr("value"), incumbents) >= 0);
          });
        });
        return $data;
      }
    };

    monitor.registerDataType(new AdjustIncumbentsList());

    //
    // ReinstateIncumbentsList Custom Data Type
    //

    ReinstateIncumbentsList.prototype = new monitor.DataType();
    ReinstateIncumbentsList.prototype.constructor = ReinstateIncumbentsList;
    ReinstateIncumbentsList.prototype.parent = monitor.DataType.prototype;
    // ReSharper disable once InconsistentNaming
    function ReinstateIncumbentsList() { }

    ReinstateIncumbentsList.prototype.name = "ReinstateIncumbentsList";

    ReinstateIncumbentsList.prototype.bindChange = function ($data) {
      $data.on("change", "input[type=checkbox]", monitor.dataChanged);
    };

    ReinstateIncumbentsList.prototype.dataChanged = function (group, $data) {
      $data.prev().val(this.val($data));
    };

    ReinstateIncumbentsList.prototype.handles = function ($data) {
      var o = $data[0];
      return o.tagName.toLowerCase() === "div" && $data.hasClass("reinstate-incumbents-control");
    };

    ReinstateIncumbentsList.prototype.parseVal = function (val) {
      var offices = val.split("|");
      if (offices.length === 1 && !offices[0]) offices = [];
      $.each(offices, function () {
        var split = this.split(":");
        offices[split[0]] = split[1].split(",");
      });
      return offices;
    };

    ReinstateIncumbentsList.prototype.unbindChange = function ($data) {
      $data.off("change", "input[type=checkbox]", monitor.dataChanged);
    };

    ReinstateIncumbentsList.prototype.val = function ($data, value) {
      if (typeof (value) === "undefined") {
        var officeEntries = [];
        $data.find(".office").each(function () {
          var $this = $(this);
          var incumbentEntries = [];
          $this.find(".incumbent:checked").each(function () {
            incumbentEntries.push($(this).attr("value"));
          });
          officeEntries.push($this.attr("rel") + ":" + incumbentEntries.join(","));
        });
        return officeEntries.join("|");
      } else {
        var offices = this.parseVal(value);
        $data.find(".office").each(function () {
          var $this = $(this);
          var incumbents = offices[$this.attr("rel")];
          $this.find(".incumbent").each(function () {
            var $this2 = $(this);
            $this2.prop("checked", $.inArray($this2.attr("value"), incumbents) >= 0);
          });
        });
        return $data;
      }
    };

    monitor.registerDataType(new ReinstateIncumbentsList());

    //
    // IdentifyWinnersTree Custom Data Type
    //

    IdentifyWinnersTree.prototype = new monitor.DataType();
    IdentifyWinnersBetaTree.prototype = new monitor.DataType();
    IdentifyWinnersTree.prototype.constructor = IdentifyWinnersTree;
    IdentifyWinnersBetaTree.prototype.constructor = IdentifyWinnersBetaTree;
    IdentifyWinnersTree.prototype.parent = monitor.DataType.prototype;
    IdentifyWinnersBetaTree.prototype.parent = monitor.DataType.prototype;
    // ReSharper disable once InconsistentNaming
    function IdentifyWinnersTree() { }
    // ReSharper disable once InconsistentNaming
    function IdentifyWinnersBetaTree() { }

    IdentifyWinnersTree.prototype.name = "IdentifyWinnersTree";
    IdentifyWinnersBetaTree.prototype.name = "IdentifyWinnersBetaTree";

    IdentifyWinnersTree.prototype.dataChanged = function (group, $data) {
      $data.prev().val(this.val($data));
    };

    IdentifyWinnersBetaTree.prototype.dataChanged = function (group, $data) {
      $data.prev().val(this.val($data));
    };

    IdentifyWinnersTree.prototype.handles = function ($data) {
      var o = $data[0];
      return o.tagName.toLowerCase() === "div" && $data.hasClass("identify-winners-control");
    };

    IdentifyWinnersBetaTree.prototype.handles = function ($data) {
      var o = $data[0];
      return o.tagName.toLowerCase() === "div" && $data.hasClass("identify-winners-beta-control");
    };

    IdentifyWinnersTree.prototype.parseVal = function (val) {
      var offices = val.split("|");
      if (offices.length === 1 && !offices[0]) offices = [];
      $.each(offices, function (index) {
        var office = {};
        var str = this;
        if (str.substr(0, 1) === "*") {
          office.selected = true;
          str = str.substr(1);
        } else office.selected = false;
        var split = str.split("=");
        if (split[1].substr(0, 1) === "*") {
          office.isRunoff = true;
          split[1] = split[1].substr(1);
        }
        office.officeKey = split[0];
        office.ids = split[1].split(",");
        offices[index] = office;
      });
      return offices;
    };

    IdentifyWinnersBetaTree.prototype.parseVal = function (val) {
      var offices = val.split("|");
      if (offices.length === 1 && !offices[0]) offices = [];
      $.each(offices, function (index) {
        var office = {};
        var str = this;
        var split = str.split("=");
        if (split[1].substr(0, 1) === "*") {
          office.isRunoff = true;
          split[1] = split[1].substr(1);
        }
        office.officeKey = split[0];
        office.ids = split[1].split(",");
        offices[index] = office;
      });
      return offices;
    };

    IdentifyWinnersTree.prototype.restoreOffice = function (officeNode, offices) {
      var office = null;
      var officeKey = officeNode.data.key;
      $.each(offices, function () {
        if (this.officeKey === officeKey) {
          office = this;
          return false;
        }
      });
      if (office) {
        var $context = $(".idwinners-" + officeKey);
        $(".runoff-checkbox", $context).prop("checked", !!office.isRunoff);
        if (office.isRunoff) {
          // clear winners
          $(".winners-dropdowns option:selected", $context).each(function () {
            $(this).prop("selected", false);
          });
          // set runoffs
          $(".runoff-dropdown option", $context).each(function () {
            $(this).prop("selected", $.inArray($(this).val(), office.ids) >= 0);
          });
        } else {
          // clear runoffs
          $(".runoff-dropdown option:selected", $context).each(function () {
            $(this).prop("selected", false);
          });
          // set winners
          $(".winners-dropdowns select", $context).each(function (index) {
            $(this).val(office.ids[index]);
          });
        }
        $(".runoff-dropdown", $context).toggleClass("hidden", !office.isRunoff);
        $(".winners-dropdowns", $context).toggleClass("hidden", !!office.isRunoff);
        if (officeNode.isSelected() !== office.selected)
          officeNode.select(office.selected);
      }
    };

    IdentifyWinnersBetaTree.prototype.restoreOffice = function (officeNode, offices) {
      var office = null;
      var officeKey = officeNode.data.key;
      $.each(offices, function () {
        if (this.officeKey === officeKey) {
          office = this;
          return false;
        }
      });
      if (office) {
        var $context = $(".idwinners-" + officeKey);
        $(".runoff-checkbox", $context).prop("checked", !!office.isRunoff);
        if (office.isRunoff) {
          // clear winners
          $(".winners-dropdowns option:selected", $context).each(function () {
            $(this).prop("selected", false);
          });
          // set runoffs
          $(".runoff-dropdown option", $context).each(function () {
            $(this).prop("selected", $.inArray($(this).val(), office.ids) >= 0);
          });
        } else {
          // clear runoffs
          $(".runoff-dropdown option:selected", $context).each(function () {
            $(this).prop("selected", false);
          });
          // set winners
          $(".winners-dropdowns select", $context).each(function (index) {
            $(this).val(office.ids[index]);
          });
        }
        $(".runoff-dropdown", $context).toggleClass("hidden", !office.isRunoff);
        $(".winners-dropdowns", $context).toggleClass("hidden", !!office.isRunoff);
      }
    };

    IdentifyWinnersTree.prototype.restoreOfficeClassNode = function (node) {
      var baseThis = this;
      var offices = this.parseVal(monitor.getGroupCurrentVal("mc-group-identifywinners-officetree") || "");
      node.visit(function (officeNode) {
        baseThis.restoreOffice(officeNode, offices);
      });
    };

    IdentifyWinnersBetaTree.prototype.restoreOfficeClassNode = function (node) {
      var baseThis = this;
      var offices = this.parseVal(monitor.getGroupCurrentVal("mc-group-identifywinnersbeta-officetree") || "");
      node.visit(function (officeNode) {
        baseThis.restoreOffice(officeNode, offices);
      });
    };

    IdentifyWinnersTree.prototype.val = function ($data, value) {
      if (typeof initDynatree === "function")
        initDynatree($data);
      var $tree = $data.dynatree("getTree");
      if (typeof (value) === "undefined") {
        var officeEntries = [];
        $tree.visit(function (node) {
          if (node.countChildren() === 0) {
            var $context = $(".idwinners-" + node.data.key);
            var ids = [];
            var $vals;
            // see if we are dealing with a runoff
            var isRunoff = $(".runoff-checkbox", $context).prop("checked");
            var officeEntry = node.isSelected() && !isRunoff ? "*" : "";
            officeEntry += node.data.key + "=";
            if (isRunoff) {
              officeEntry += "*";
              $vals = $(".runoff-dropdown option:selected", $context);
            } else {
              $vals = $(".winners-dropdowns select", $context);
            }
            $vals.each(function () {
              ids.push($(this).val());
            });
            officeEntry += ids.join(",");
            officeEntries.push(officeEntry);
          }
        });
        return officeEntries.join("|");
      } else {
        var offices = this.parseVal(value);
        $tree._undoing = true;
        var baseThis = this;
        $tree.visit(function (node) {
          if (node.countChildren() === 0) {
            baseThis.restoreOffice(node, offices);
          }
        });
        $tree._undoing = false;
        return $data;
      }
    };

    IdentifyWinnersBetaTree.prototype.val = function ($data, value) {
      if (typeof initDynatree === "function")
        initDynatree($data);
      var $tree = $data.dynatree("getTree");
      if (typeof (value) === "undefined") {
        var officeEntries = [];
        $tree.visit(function (node) {
          if (node.countChildren() === 0) {
            var $context = $(".idwinners-" + node.data.key);
            // see if we are dealing with a runoff
            var officeEntry = node.data.key + "=";
            var ids = [];
            var $vals;
            var isRunoff = $(".runoff-checkbox", $context).prop("checked");
            if (isRunoff) {
              officeEntry += "*";
              $vals = $(".runoff-dropdown option:selected", $context);
            } else {
              $vals = $(".winners-dropdowns select", $context);
            }
            $vals.each(function () {
              ids.push($(this).val());
            });
            officeEntry += ids.join(",");
            officeEntries.push(officeEntry);
          }
        });
        return officeEntries.join("|");
      } else {
        var offices = this.parseVal(value);
        $tree._undoing = true;
        var baseThis = this;
        $tree.visit(function (node) {
          if (node.countChildren() === 0) {
            baseThis.restoreOffice(node, offices);
          }
        });
        $tree._undoing = false;
        return $data;
      }
    };

    monitor.registerDataType(new IdentifyWinnersTree(), "Dynatree1");
    monitor.registerDataType(new IdentifyWinnersBetaTree(), "Dynatree1");

    // 
    // Select BallotMeasure Control
    //

    var ballotMeasureControlHeight = 240;

    function afterUpdateContainerSelectBallotMeasure() {
      initBallotMeasuresControl();
    }

    function getSelectedBallotMeasure() {
      // can be "add" if adding new ballot measure 
      return $('.selected-ballot-measure-key').val();
    }

    function getBallotMeasuresSlimScrollOptions($selectedElement) {
      var options = {
        height: ballotMeasureControlHeight + 'px',
        width: '350px',
        alwaysVisible: true,
        color: '#666',
        size: '12px'
      };
      if ($selectedElement)
        options.start = $selectedElement;
      return options;
    }

    function initBallotMeasuresControl() {
      var $ballotMeasureControl = $('.select-ballot-measure-control');
      $ballotMeasureControl.sortable({
        axis: "y",
        opacity: 0.5,
        scroll: true,
        handle: ".icon-move"
      });
      util.safeBind($ballotMeasureControl, "click", onClickBallotMeasureControl);
      util.safeBind($('.select-ballot-measure-container .slimscroll-toggler'), "click",
        onClickToggleBallotMeasureScroller);
      util.safeBind($('.select-ballot-measure-container .add-ballot-measure'), "click",
        onClickAddBallotMeasure);
      updateSelectedBallotMeasure();
      var noscroll = $(".ballot-measure-noscroll-state").val() === "true";
      $('.select-ballot-measure-container .slimscroll-toggler').toggleClass("showing", noscroll); ;
      if (!noscroll) {
        var options = getBallotMeasuresSlimScrollOptions();
        toggleSlimScroll($ballotMeasureControl, true, options);
      }
      // show it
      if ($ballotMeasureControl.hasClass("show")) {
        toggleSelectBallotMeasureVisibility();
        $ballotMeasureControl.removeClass("show");
      }
    }

    function onClickAddBallotMeasure() {
      if ($(".add-ballot-measure").hasClass("disabled"))
        return;
      var isChanged = monitor.isPanelsChanged($(".mc-container",
       getCurrentTabPanel()));
      if (isChanged) {
        util.confirm("There are unsaved changes on this panel.\n\n" +
          "Click OK to discard the changes and add the new ballot measure.\n" +
          "Click Cancel to return to the changed panel.",
             function (button) {
               if (button === "Ok")
                 finishAddBallotMeasure();
               else
                 toggleSelectBallotMeasureVisibility();
             });
        return;
      }
      finishAddBallotMeasure();
    }

    function finishAddBallotMeasure() {
      $('.selected-ballot-measure-key').val("add");
      updateSelectedBallotMeasure();
      onSelectedBallotMeasureChanged();
    }

    function onClickBallotMeasureControl(event) {
      var $target = $(event.target);
      if ($('.select-ballot-measure-control').hasClass('disabled') ||
        !$target.hasClass('ballot-measure-desc') ||
        $target.hasClass('selected')) return;
      var isChanged = monitor.isPanelsChanged($(".mc-container",
       getCurrentTabPanel()));
      if (isChanged) {
        util.confirm("There are unsaved changes on this panel.\n\n" +
          "Click OK to discard the changes and load the new ballot measure.\n" +
          "Click Cancel to return to the changed panel.",
             function (button) {
               if (button === "Ok")
                 finishClickBallotMeasureControl($target);
               else
                 toggleSelectBallotMeasureVisibility();
             });
        return;
      }
      finishClickBallotMeasureControl($target);
    }

    function finishClickBallotMeasureControl($target) {
      $('.selected-ballot-measure-key').val($('.ballot-measure-key', $target).val());
      updateSelectedBallotMeasure();
      onSelectedBallotMeasureChanged();
    }

    function onClickToggleBallotMeasureScroller(event) {
      var $control = $('.select-ballot-measure-control');
      var $selectedElement = $(".ballot-measure-key.selected", $control);
      var options = getBallotMeasuresSlimScrollOptions($selectedElement);
      var scrolling = toggleSlimScroll($('.select-ballot-measure-control'), options,
        getBallotMeasuresSlimScrollOptions(null));
      $(".ballot-measure-noscroll-state").val(!scrolling);
      $(event.target).toggleClass("showing", !scrolling);
    }

    function onClickSelectBallotMeasure() {
      electionControl.toggleElectionControl(false);
      toggleSelectBallotMeasureVisibility();
    }

    function onSelectedBallotMeasureChanged() {
      electionControl.toggleElectionControl(false);
      toggleSelectBallotMeasureVisibility();
      reloadPanel(getCurrentTabPanel()[0].id, "loadballotmeasure");
    }

    function toggleSelectBallotMeasureVisibility() {
      var $o = $(".select-ballot-measure-container-outer");
      var wasShowing = $o.css("display") !== "none";
      var $toggle = $('.select-ballot-measure-toggler');
      var $ballotMeasureControl = $('.select-ballot-measure-control');
      var fn = null;
      if (wasShowing) {
        $toggle.removeClass("showing");
        $(".ballot-measure-scroll-position").val($ballotMeasureControl.scrollTop());
      } else {
        $toggle.addClass("showing");
        var scrollTop = $(".ballot-measure-scroll-position").val();
        if (scrollTop && $ballotMeasureControl.parent().hasClass("slimScrollDiv"))
          fn = function () {
            $('.select-ballot-measure-control')
              .slimScroll({ scrollTo: $(".ballot-measure-scroll-position").val() + 'px' });
          };
      }
      $o.toggle("slide", fn);
      return wasShowing;
    }

    function updateSelectedBallotMeasure() {
      var $ballotMeasureControl = $('.select-ballot-measure-control');
      var ballotMeasureKey = getSelectedBallotMeasure();
      var $selectedElement = null;
      $('.select-ballot-measure-control .ballot-measure-desc.selected').removeClass('selected');
      if (ballotMeasureKey && ballotMeasureKey !== "add") {
        $.each($('.ballot-measure-key', $ballotMeasureControl), function () {
          if ($(this).val() === ballotMeasureKey) {
            $selectedElement = $(this).parent();
            $selectedElement.addClass('selected');
            return false;
          }
        });
      }
      return $selectedElement;
    }

    //
    // Add Ballot Measures
    //

    function addBallotMeasuresActionCompleted(action) {
      var isChanged = monitor.isPanelsChanged($(".mc-container", getCurrentTabPanel()));
      if (isChanged)
        action += " Click update to save the ballot measure.";
      monitor.addGroupFeedback("mc-group-addballotmeasures", "ok", action, true);
    }

    function afterRequestAddBallotMeasures() {
      var recased = $("#tab-addballotmeasures .recased").val();
      if (recased) {
        var split = recased.split("|");
        if (split.length === 1)
          util.alert("The following field contained all capital letters:\n\n" +
            split.join("\n") + "\n\nWe have attempted to apply mixed casing" +
            " to this field but further hand editing may be necessary.");
        else
          util.alert("The following fields contained all capital letters:\n\n" +
            split.join("\n") + "\n\nWe have attempted to apply mixed casing" +
            " to these fields but further hand editing may be necessary.");
      }
    }

    function afterUpdateContainerAddBallotMeasures() {
      var active = $("#tab-addballotmeasures .sub-tab-index").val() || 0;
      $("#ballot-measure-tabs").tabs(
      {
        show: 400,
        active: active,
        activate: function (event, ui) {
          // save the current tab index to restore after update
          $("#tab-addballotmeasures .sub-tab-index")
            .val($("#ballot-measure-tabs").tabs("option", "active"));
          var suffix = ui.newPanel[0].id.substr(ui.newPanel[0].id.lastIndexOf("-"));
          setReferendumTextArea("mc-group-addballotmeasures" + suffix);
        }
      });
      if (getSelectedBallotMeasure())
        if ($(".addballotmeasures-animate").val() === "true")
          $("#main-tabs #tab-addballotmeasures .data-area").show("scale", 400);
        else
          $("#main-tabs #tab-addballotmeasures .data-area").show();
      util.safeBind($(".select-ballot-measure-toggler"), "click",
          onClickSelectBallotMeasure);
      util.safeBind($("#ballot-measure-tabs .remove-line-breaks"), "click",
          onClickRemoveBallotMeasureLineBreaks);
      util.safeBind($("#tab-addballotmeasures-referendumfulltext .input-element .referendumfulltext .text-area-toggler"), "click",
          toggleReferendumTextArea);
    }

    function enableBallotMeasureRemoveLineBreaks(group) {
      if (!group) return;
      var suffix = group.group.substr(group.group.lastIndexOf("-"));
      var $button = $("#tab-addballotmeasures" + suffix + " .remove-line-breaks input");
      var enable = group._current && (group._current.indexOf("\r") >= 0 || group._current.indexOf("\n") >= 0);
      $button.toggleClass("disabled", !enable);
    }

    function onClickRemoveBallotMeasureLineBreaks(event) {
      var $tab = $(event.target).parents("#ballot-measure-tabs .ui-tabs-panel");
      // panel id used to get group name: panel id = tab-xxxxx,
      // group name = mc-group-xxxxx
      removeLineBreaks("mc-group" + $tab[0].id.substr(3));
    }

    function setReferendumTextArea(group, state) {
      group = monitor.getGroup(group);
      if (group.expandable) {
        var expandable = $$(group.expandable);
        if (typeof state !== "boolean")
          state = expandable.val() === "true";
        else
          expandable.val(state ? "true" : "false");
        var classSuffix = group.group.substr(group.group.lastIndexOf("-") + 1);
        $("." + classSuffix + " .text-area-toggler").toggleClass("showing", state);
        monitor.dataChanged({ target: $$(group.data)[0] }, group._current);
      }
    }

    function toggleReferendumTextArea(toggler) {
      var $tab = $(toggler).parents("#ballot-measure-tabs .ui-tabs-panel");
      var group = monitor.groups["mc-group" + $tab[0].id.substr(3)];
      if (group.expandable) {
        setReferendumTextArea(group, $$(group.expandable).val() !== "true");
      }
    }

    // 
    // View Report
    //

    function onClickGetReport() {
      var electionKey = getSelectedElection();
      if (!electionKey) {
        util.alert("Please select an election.");
        return;
      }

      var openAll = $("#pre-open-accordions-checkbox").prop("checked")
        ? "&openall=y"
        : "";

      if ($("#get-report-in-new-window-checkbox").prop("checked")) {
        //window.open("/admin/electionreport.aspx?election=" + electionKey, "view");
        window.open("/election.aspx?public=n&election=" + electionKey + openAll, "view");
        return;
      }

      $("#ElectionIFrame").attr("src", "/electionforiframe.aspx?public=n&election=" + electionKey + openAll).removeClass("hidden");

//      util.openAjaxDialog("Getting Election Report...");
//      util.ajax({
//        url: "/Admin/WebService.asmx/GetElectionReport",
//        data: {
//          electionKey: electionKey
//        },
//        success: function (result) {
//          var $report = $("#ElectionReport");
//          $report.html(result.d.Html).show();
//          $("#tab-viewreport .report-head").html(util.htmlEscape(result.d.Title));
//          util.setOffpageTargets($report);
//          util.closeAjaxDialog();
//        },
//        error: function (result) {
//          util.closeAjaxDialog();
//          util.alert(util.formatAjaxError(result,
//              "Could not get the Election Report"));
//        }
//      });
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
        beforeActivate: onTabsBeforeActivateMasterOnlySubTab,
        activate: function (event, ui) {
          // save the current tab index to restore after update
          $("#tab-masteronly .sub-tab-index")
            .val($("#master-only-tabs").tabs("option", "active"));
          checkActivateAddPrimaryWinners();
          checkActivateAddRunoffAdvancers();
        }
      });
      util.safeBind($("#tab-masteronly .select-election-to-copy-button"),
        "click", onClickSelectElectionToCopy);
      if (!getSelectedElection()) // election was deleted
      {
        util.alert("The election was deleted");
        $("#main-tabs").tabs("option", "active", 0);
      }
      $$("voters-edge-button").safeBind("click", onClickVotersEdge);
      $$("vote-smart-button").safeBind("click contextmenu", onClickVoteSmart);
      var electionKey = getSelectedElection();
      var isState = electionKey.length === 12;
      var isPrimary = "ABP".indexOf(electionKey.substr(10, 1)) >= 0;
      var isRunoff = "QR".indexOf(electionKey.substr(10, 1)) >= 0;
      $(".master-includeelection-tab").toggleClass("hidden", !isState || !isPrimary);
      $(".master-addprimarywinners-tab").toggleClass("hidden", isPrimary || isRunoff);
      $(".master-addrunoffadvancers-tab").toggleClass("hidden", !isRunoff);
    }

    function checkActivateAddPrimaryWinners() {
      var activated = (util.getCurrentTabId("main-tabs") + ":" + util.getCurrentTabId("master-only-tabs")) ===
        "tab-masteronly:tab-masteronly-addprimarywinners";
      var original = $("#HiddenMasterOnlyPrimaryDateToCopy").val();
      var current = $(".primary-date-to-copy").val();
      if (activated) {
        if (!current) $(".primary-date-to-copy").val(original);
      } else {
        if (current === original) $(".primary-date-to-copy").val("");
      }
    }

    function checkActivateAddRunoffAdvancers() {
      var activated = (util.getCurrentTabId("main-tabs") + ":" + util.getCurrentTabId("master-only-tabs")) ===
        "tab-masteronly:tab-masteronly-addrunoffadvancers";
      var original = $("#HiddenMasterOnlyElectionDateToCopy").val();
      var current = $(".election-date-to-copy").val();
      if (activated) {
        if (!current) $(".election-date-to-copy").val(original);
      } else {
        if (current === original) $(".election-date-to-copy").val("");
      }
    }

    function initRequestMasterOnly(group) {
      if ($$(group.button).hasClass("validated")) {
        $$(group.button).removeClass("validated");
        return true;
      }
      if (group.children) {
        // warn if we are deleting an election
        // find the deleteelection monitor group
        var deleteElectionGroup = null;
        $.each(group.children, function () {
          if (this.group === "mc-group-masteronly-deleteelection") {
            deleteElectionGroup = this;
            return false;
          }
        });

        if (deleteElectionGroup && deleteElectionGroup._current === "true") {
          util.confirm("You are about to completely delete this election.\n\n" +
            "Ok?",
             function (button) {
               if (button === "Ok") {
                 $$(group.button).addClass("validated").click();
               }
             });
          return false;
        }
      }
      return true;
    }

    function onClickSelectElectionToCopy() {
      electionControl.toggleElectionControl(true);
      onElectionChangingAlternate = onElectionChangingToSelectElectionToCopy;
    }

    function onClickVotersEdge() {
      $$("voters-edge-anchor").attr("href", "/admin/DownloadVotersEdgeCsv.aspx?election=" +
        getSelectedElection())[0].click();
    }

    function onClickVoteSmart() {
      var href = "/admin/votesmartimport.aspx";
      var electionKey = $('.selected-election-key').val();
      if (electionKey) href += "?election=" + electionKey;
      else {
        var stateCode = $(".client-state-code").val();
        if (stateCode) href += "?state=" + stateCode;
      }
      $$("vote-smart-button").attr("href", href);
    }

    function onElectionChangingToSelectElectionToCopy(newElectionKey) {
      $('.election-key-to-copy').val(newElectionKey);
      $("#tab-masteronly-addcandidates .input-element.electiontocopy input[type=text]")
        .val(electionControl.getElectionDesc(newElectionKey)).change();
      electionControl.toggleElectionControl(false);
    }

    function onTabsBeforeActivateMasterOnlySubTab(event, ui) {
      // these values aren't worth protecting
      if (ui.oldPanel.length && ui.oldPanel[0].id === "tab-masteronly-addprimarywinners")
        return true;
      return monitor.tabsBeforeActivate(event, ui, "master-only-tabs",
       "#tab-masteronly .mc-container");
    }

    function updateGeneralElectionDesc(group) {
      util.ajax({
        url: "/Admin/WebService.asmx/GetGeneralElectionDescription",
        data: {
          input: group._current
        },
        success: getGeneralElectionDescriptionSucceeded,
        error: getGeneralElectionDescriptionFailed
      });
    }

    function getGeneralElectionDescriptionSucceeded(result) {
      $(".mc-group-masteronly-generalelectiondesc").val(result.d);
    }

    function getGeneralElectionDescriptionFailed(result) {
      $(".mc-group-masteronly-generalelectiondesc").val("");
    }

    //
    // Misc
    //

    function isPrimaryElection(electionKey) {
      return electionKey && isPrimaryType(electionKey.substr(10, 1));
    }

    function isPrimaryType(electionType) {
      return electionType && "ABPQ".indexOf(electionType.toUpperCase()) >= 0;
    }

    function toggleSlimScroll($o, state, options) {
      var $parent = $o.parent();
      var currentState = $parent.hasClass("slimScrollDiv");
      var scrolling = typeof state === "boolean" ? state : !currentState;
      if (currentState !== scrolling) {
        if (scrolling) {
          $o.slimScroll(options || {});
        } else {
          $o.parent().replaceWith($o);
          $o.attr("style", "");
        }
      }
      return scrolling;
    }

    function getCurrentTabPanel() {
      return util.getCurrentTabPanel("main-tabs");
    }

    function onTabsActivate(event, ui) {
      var newPanelId = ui.newPanel.length ? ui.newPanel[0].id : "";
      var $o = $('.election-control-container');
      switch (newPanelId) {
        case "tab-addelection":
          $o.addClass("disabled");
          electionControl.toggleElectionControl(false);
          break;

        default:
          $o.removeClass("disabled");
          electionControl.toggleElectionControl(!getSelectedElection());
          break;
      }
      checkActivateAddPrimaryWinners();
      checkActivateAddRunoffAdvancers();
      reloadPanel(newPanelId);
    }

    function onTabsBeforeActivate(event, ui) {
      var newPanelId = ui.newPanel.length ? ui.newPanel[0].id : "";
      var oldContainer = $(".mc-container", ui.oldPanel);
      var isChanged = monitor.isPanelsChanged(oldContainer);
      if (isChanged) {
        if (!confirm("There are unsaved changes on the panel you are leaving.\n\nClick OK to discard the changes and continue.\nClick Cancel to return to the changed panel.")) {
          return false;
        }
        monitor.undoPanels(oldContainer);
      }
      var selectedElection = getSelectedElection();
      switch (newPanelId) {
        case "tab-statedefaults":
        case "tab-changeinfo":
        case "tab-changedeadlines":
        case "tab-addoffices":
        case "tab-addcandidates":
        case "tab-identifywinners":
        case "tab-identifywinnersbeta":
        case "tab-addballotmeasures":
        case "tab-masteronly":
          $$(newPanelId + ' .data-area').addClass('invisible');
          var message;
          if (selectedElection) {
            message = "Loading...";
          } else {
            message = "Please select an election from the list on the right.";
          }
          $$(newPanelId + ' .panel-heading').html(message);
          break;
      }
      if (newPanelId === "tab-addcandidates") {
        $(".select-office-container").addClass("invisible");
        $(".select-office-toggler").hide();
      }
      if (newPanelId === "tab-addballotmeasures") {
        $(".select-ballot-measure-container-outer").addClass("invisible");
        $(".select-ballot-measure-toggler").hide();
      }
    }

    function reloadPanel(id, option) {
      if (!option) option = "reloading";
      switch (id) {
        case "tab-statedefaults":
        case "tab-changeinfo":
        case "tab-changedeadlines":
        case "tab-addoffices":
        case "tab-addcandidates":
        case "tab-adjustincumbents":
        case "tab-identifywinners":
        case "tab-identifywinnersbeta":
        case "tab-masteronly":
          if (getSelectedElection()) {
            $$(id + ' .reloading').val(option);
            $$(id + ' input.update-button').addClass("reloading").click();
          }
          break;

        case "tab-addballotmeasures":
          if (option === "reloading") {
            $$(id + ' .data-area').addClass('invisible');
            if (getSelectedElection()) {
              $$(id + ' .sub-panel .reloading').val(option);
              $$(id + ' .sub-panel input.update-button').addClass("reloading").click();
            }
          } else if (getSelectedElection()) {
            $$(id + ' .main-panel .reloading').val(option);
            $$(id + ' .main-panel input.update-button').addClass("reloading").click();
          }
          break;
      }
    }

    function removeLineBreaks(group) {
      group = monitor.getGroup(group);
      var val = group._current
          .replace(/ \r\n/g, " ")
          .replace(/\r\n /g, " ")
          .replace(/\r\n/g, " ")
          .replace(/ \r/g, " ")
          .replace(/\r /g, " ")
          .replace(/\r/g, " ")
          .replace(/ \n/g, " ")
          .replace(/\n /g, " ")
          .replace(/\n/g, " ");
      if (val !== group._current) {
        var $data = $$(group.data);
        group._dataType.val($data, val);
        monitor.dataChanged({ target: $data[0] }, group.val);
      }
    }

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });