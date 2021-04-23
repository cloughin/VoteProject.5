define(["jquery", "vote/adminMaster", "vote/util", "monitor",
  "vote/controls/navigateJurisdiction", "vote/controls/electionControl",
  "jqueryui", "slimscroll"],
  function ($, master, util, monitor, navigateJurisdiction, electionControl) {

    var socialMediaClasses = ["email", "website", "facebook", "twitter", 
    "youtube"];

    function autoLocateVoteSmartCandidate() {
      // only autolocate if a VoteSmart election is selected
      if (!getVoteSmartElectionKey()) return;
      var firstThree = $('.vusa-candidates-dropdown option:selected').attr("rel") || "";
      // get the votesmart candidates
      var $dropdown = $(".vs-candidates-dropdown");
      var val = $dropdown.val();
      if (!firstThree) $dropdown.val("");
      else $("option", $dropdown).each(function () {
        if ($(this).text().substr(0, firstThree.length).toLowerCase() === firstThree) {
          $dropdown.val($(this).val());
          return false;
        }
      });
      if (val !== $dropdown.val())
        getVoteSmartCandidate();
    }

    function canCopyEmptyField(field) {
      var $comparison = $(".comparison." + field.toString());
      var vsData = getVsData($comparison);
      var vusaData = $.trim($(".vusa-data .value", $comparison).val());
      return vsData && !vusaData;
    }

    function canCopyEmptyFields(field) {
      var result = false;
      $.each(getTopicClasses(), function () {
        if (canCopyEmptyField(this)) {
          result = true;
          return false;
        }
      });
      return result;
    }

    function copyEmptyField(field) {
      if (canCopyEmptyField(field)) {
        var $comparison = $(".comparison." + field.toString());
        $(".vusa-data .value", $comparison).val(getVsData($comparison));
      }
    }

    function enableVoteUsaCandidateButtons() {
      // next is enabled unless both vusa-offices-dropdown and 
      // vusa-candidates-dropdown are on last element.
      // prev is enabled unless both on 1st or 2nd (1st is dummy entry).

      var $offices = $(".vusa-offices-dropdown");
      var $candidates = $(".vusa-candidates-dropdown");
      var officesInx = $offices.prop("selectedIndex");
      var candidatesInx = $candidates.prop("selectedIndex");
      var next = officesInx < ($offices.children().length - 1) ||
        candidatesInx < ($candidates.children().length - 1);
      var prev = officesInx > 0 || candidatesInx > 0;
      $(".next-vusa-candidate-button").prop("disabled", !next);
      $(".prev-vusa-candidate-button").prop("disabled", !prev);
    }

    function getMagicClass($element, classes) {
      var result = null;
      $.each(classes, function() {
        var className = this.toString();
        if ($element.hasClass(className)) {
          result = className;
          return false;
        }
      });
      return result;
    }

    function getSelectedVoteSmartCandidate() {
      return $('.vs-candidates-dropdown').val();
    };

    function getSelectedVoteUsaCandidate() {
      return $('.vusa-candidates-dropdown').val();
    };

    function getSelectedVoteUsaOffice() {
      return $('.vusa-offices-dropdown').val();
    };

    function getTopicClasses(includePhoto) {
      var result = ["family", "education", "professional", "civic",
      "political", "religion", "birthdate", "email", "website",
      "facebook", "twitter", "youtube"];
      if (includePhoto) result.push("photo");
      return result;
    }

    function getVoteSmartCandidate() {
      function clear() {
        $(".name .vs-data .img").safeHtml("");
        $(".family .vs-data .value").val("");
        $(".education .vs-data .value").val("");
        $(".professional .vs-data .value").val("");
        $(".civic .vs-data .value").val("");
        $(".political .vs-data .value").val("");
        $(".religion .vs-data .value").val("");
        $(".birthdate .vs-data .value").safeHtml("");
        $(".email .vs-data .value").safeHtml("");
        $(".website .vs-data .value").safeHtml("");
        $(".facebook .vs-data .value").safeHtml("");
        $(".twitter .vs-data .value").safeHtml("");
        $(".youtube .vs-data .value").safeHtml("");
        $(".photo .vs-data .value").safeHtml("");
        $(".photo .vs-data .img").safeHtml("");
      }
      var vsCandidateId = getSelectedVoteSmartCandidate();
      $(".refresh-vs-candidate-button").prop("disabled", !vsCandidateId);
      $(".refresh-candidate-date").toggleClass("hidden", !vsCandidateId);
      if (!vsCandidateId) {
        $(".name .vs-data h3").safeHtml("");
        clear();
        onChangeData();
      } else {
        var vsName = $('.vs-candidates-dropdown option[value="' + vsCandidateId + '"]').html();
        // disinvert
        var pos = vsName.lastIndexOf(",");
        if (pos >= 0)
          vsName = vsName.substr(pos + 2) + " " + vsName.substr(0, pos);
        $(".name .vs-data h3").safeHtml(vsName);
        var ajaxKey = "getVoteSmartCandidate";
        util.pushAjaxDialog(ajaxKey, "Getting VoteSmart Candidate...");
        util.ajax({
          url: "/Admin/VsWebService.asmx/GetCandidate",
          data: {
            vsCandidateId: vsCandidateId
          },
          success: function (result) {
            if (!result.d) {
              $(".refresh-candidate-date").safeHtml("Last refresh: never");
              clear();
            } else {

              function findWebAddress(key) {
                var address = "";
                $.each(result.d.WebAddresses, function() {
                  if (this.Value === key) {
                    address = this.Text;
                    return false;
                  }
                });
                return address;
              }

              // update the refresh date
              var refreshDate = moment(result.d.LastRefreshDate);
              refreshDate = refreshDate.year() < 2000 ? "never" : refreshDate.format("MM/DD/YYYY hh:mm A");
              $(".refresh-candidate-date")
              .safeHtml("Last refresh: " + refreshDate)
              .removeClass("hidden");

              $(".family .vs-data .value").val(result.d.Family);
              $(".education .vs-data .value").val(result.d.Education);
              $(".professional .vs-data .value").val(result.d.Profession);
              $(".civic .vs-data .value").val(result.d.OrgMembership);
              $(".political .vs-data .value").val(result.d.Political);
              $(".religion .vs-data .value").val(result.d.Religion);
              var birthDate = moment(result.d.BirthDate);
              birthDate = birthDate.year() < 1901 ? "" : birthDate.format("MM/DD/YYYY");
              $(".birthdate .vs-data .value").safeHtml(birthDate);
              setSocialMediaLink(findWebAddress("Email") || findWebAddress("Webmail"), "email", "vs-data");
              setSocialMediaLink(findWebAddress("Website"), "website", "vs-data");
              setSocialMediaLink(findWebAddress("Website - Facebook"), "facebook", "vs-data");
              setSocialMediaLink(findWebAddress("Website - Twitter"), "twitter", "vs-data");
              setSocialMediaLink(findWebAddress("Website - YouTube"), "youtube", "vs-data");
              $(".photo .vs-data .value").safeHtml(result.d.Photo);
              var img = "";
              if (result.d.Photo) img = '<img src="' + result.d.Photo + '" />';
              $(".photo .vs-data .img").html(img);
              $(".name .vs-data .img").html(img);
              onChangeData();
            }
          },
          error: function (result) {
            util.alert(util.formatAjaxError(result, "Could not get VoteSmart Candidate"));
          },
          complete: function (result) {
            util.popAjaxDialog(ajaxKey);
          }
        });

      }
      setComparisonVisibility();
    }

    function getVoteSmartCandidates() {
      var vsElectionKey = getVoteSmartElectionKey();
      if (!vsElectionKey) return;
      var ajaxKey = "getVoteSmartCandidates";
      util.pushAjaxDialog(ajaxKey, "Getting VoteSmart Candidate List...");
      util.ajax({
        url: "/Admin/VsWebService.asmx/GetCandidates",
        data: {
          vsElectionKey: vsElectionKey
        },
        success: function (result) {
          // update the refresh date
          var refreshDate = moment(result.d.LastRefreshDate);
          refreshDate = refreshDate.year() < 2000 ? "never" : refreshDate.format("MM/DD/YYYY hh:mm A");
          var $refreshDate = $(".refresh-candidates-date");
          $refreshDate.safeHtml("Last refresh: " + refreshDate);
          $refreshDate.removeClass("hidden");
          // load  VoteSmart candidates
          util.populateDropdown($(".vs-candidates-dropdown"),
           result.d.Candidates, "<Select VoteSmart Candidate>");
          $(".vs-candidate-boxed-group").removeClass("hidden");
          setComparisonVisibility();
          autoLocateVoteSmartCandidate();
        },
        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not get VoteSmart Candidate List"));
        },
        complete: function () { util.popAjaxDialog(ajaxKey); }
      });
    }

    function getVoteSmartElectionKey() {
      return $('.vs-elections-dropdown').val();
    };

    function getVoteSmartElections() {
      var electionDesc = electionControl.getElectionDesc(getSelectedElection());
      var year = electionDesc.length >= 4 ? electionDesc.substr(0, 4) : "";
      var stateName = $(".client-state-name").val();
      var desc = year + " VoteSmart Elections for " + stateName;
      var ajaxKey = "getVoteSmartElections";
      util.pushAjaxDialog(ajaxKey, "Getting " + desc + "...");
      util.ajax({
        url: "/Admin/VsWebService.asmx/GetElections",
        data: {
          electionYear: year,
          stateCode: $(".client-state-code").val()
        },
        success: function (result) {
          // update the refresh date
          var refreshDate = moment(result.d.LastRefreshDate);
          refreshDate = refreshDate.year() < 2000 ? "never" : refreshDate.format("MM/DD/YYYY hh:mm A");
          var $refreshDate = $(".refresh-elections-date");
          $refreshDate.safeHtml("Last refresh: " + refreshDate);
          $refreshDate.removeClass("hidden");
          // load  VoteSmart elections
          util.populateDropdown($(".vs-elections-dropdown"),
           result.d.Elections, "<Select VoteSmart Election>");
          $(".vs-election-boxed-group").removeClass("hidden");
        },
        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not get " + desc));
        },
        complete: function() { util.popAjaxDialog(ajaxKey); }
      });
    }

    function getVoteUsaCandidate(force) {
      function clear() {
        $(".name .vusa-data h3").safeHtml("");
        $(".name .vusa-data .politician-key").val("");
        $(".name .vusa-data .img").safeHtml("");
        $(".family .vusa-data .value").val("");
        $(".education .vusa-data .value").val("");
        $(".professional .vusa-data .value").val("");
        $(".military .vusa-data .value").val("");
        $(".civic .vusa-data .value").val("");
        $(".political .vusa-data .value").val("");
        $(".religion .vusa-data .value").val("");
        $(".accomplishments .vusa-data .value").val("");
        $(".birthdate .vusa-data .value").val("");
        $(".email .vusa-data .value").val("");
        $(".website .vusa-data .value").val("");
        $(".facebook .vusa-data .value").val("");
        $(".twitter .vusa-data .value").val("");
        $(".youtube .vusa-data .value").val("");
        $(".photo .vusa-data .img").safeHtml("");
        monitorData();
      }
      if (!force && isDataChanged()) {
        util.confirm("There are insaved changes. Continue?", function(button) {
          if (button === "Ok")
            getVoteUsaCandidate(true);
          else
            $(".vusa-candidates-dropdown").val($(".name .vusa-data .politician-key").val());
        });
        return;
      }
      var politicianKey = getSelectedVoteUsaCandidate();
      if (!politicianKey) {
        clear();
      } else {
        $(".name .vusa-data h3").safeHtml($('.vusa-candidates-dropdown option[value="' + politicianKey + '"]').html());
        var ajaxKey = "getVoteUsaCandidate";
        util.pushAjaxDialog(ajaxKey, "Getting VoteUSA Candidate...");
        util.ajax({
          url: "/Admin/VsWebService.asmx/GetVoteUsaCandidate",
          data: {
            politicianKey: politicianKey
          },
          success: function (result) {
            if (!result.d) {
              clear();
            } else {
              $(".name .vusa-data .politician-key").val(politicianKey);
              $(".family .vusa-data .value").val(result.d.Personal);
              $(".education .vusa-data .value").val(result.d.Education);
              $(".professional .vusa-data .value").val(result.d.Profession);
              $(".military .vusa-data .value").val(result.d.Military);
              $(".civic .vusa-data .value").val(result.d.Civic);
              $(".political .vusa-data .value").val(result.d.Political);
              $(".religion .vusa-data .value").val(result.d.Religion);
              $(".accomplishments .vusa-data .value").val(result.d.Accomplishments);
              var birthDate = moment.utc(result.d.BirthDate);
              birthDate = birthDate.year() < 1901 ? "" : birthDate.format("MM/DD/YYYY");
              $(".birthdate .vusa-data .value").val(birthDate);
              setSocialMediaLink(result.d.Email, "email", "vusa-data");
              setSocialMediaLink(result.d.Website, "website", "vusa-data");
              setSocialMediaLink(result.d.Facebook, "facebook", "vusa-data");
              setSocialMediaLink(result.d.Twitter, "twitter", "vusa-data");
              setSocialMediaLink(result.d.YouTube, "youtube", "vusa-data");
              $(".photo .vusa-data .img").html('<img src="/image.aspx?nc=1&id=' + result.d.PoliticianKey + '" />');
              $(".name .vusa-data .img").html('<img src="/image.aspx?nc=1&col=Headshot100&id=' + result.d.PoliticianKey + '" />');
              monitorData();
            }
          },
          error: function (result) {
            util.alert(util.formatAjaxError(result, "Could not get VoteUSA Candidate"));
          },
          complete: function (result) {
            util.popAjaxDialog(ajaxKey);
          }
        });

      }
      enableVoteUsaCandidateButtons();
      setComparisonVisibility();
    }

    function getVoteUsaCandidates(selectLast) {
      var electionKey = getSelectedElection();
      var officeKey = getSelectedVoteUsaOffice();
      if (!officeKey) {
        $(".vusa-candidates-dropdown").addClass("hidden").html("");
        setComparisonVisibility();
        return;
      }
      var ajaxKey = "getVoteUsaCandidates";
      util.pushAjaxDialog(ajaxKey, "Getting VoteUSA candidates for selected office...");
      util.ajax({
        url: "/Admin/VsWebService.asmx/GetVoteUsaCandidates",
        data: {
          electionKey: electionKey,
          officeKey: officeKey
        },
        success: function (result) {
          // load VoteUSA candidates
          var $dropdown = $(".vusa-candidates-dropdown");
          util.populateDropdown($dropdown, result.d, "<Select Candidate>",
           null, null, "FirstThree");
          if (selectLast)
            $dropdown.prop("selectedIndex", $dropdown.children().length - 1);
          $(".vusa-candidates-dropdown").removeClass("hidden");
          enableVoteUsaCandidateButtons();
          setComparisonVisibility();
          onChangeVoteUsaCandidatesDropdown();
        },
        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not get VoteUSA candidates for selected office"));
        },
        complete: function () { util.popAjaxDialog(ajaxKey); }
      });
    }

    function getVoteUsaOffices() {
      var electionKey = getSelectedElection();
      var electionDesc = electionControl.getElectionDesc(electionKey);
      var ajaxKey = "getVoteUsaOffices";
      util.pushAjaxDialog(ajaxKey, "Getting VoteUSA offices for " +
        electionDesc + "...");
      util.ajax({
        url: "/Admin/VsWebService.asmx/GetVoteUsaOffices",
        data: {
          electionKey: electionKey
        },
        success: function (result) {
          // load VoteUSA offices
          util.populateDropdown($(".vusa-offices-dropdown"), result.d, 
          "<Select Office>");
          $(".vusa-candidates-dropdown").addClass("hidden").html("");
          $(".vusa-office-candidate-boxed-group").removeClass("hidden");
          enableVoteUsaCandidateButtons();
          setComparisonVisibility();
        },
        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not get VoteUSA offices for " + electionDesc));
        },
        complete: function () { util.popAjaxDialog(ajaxKey); }
      });
    }

    function initPage() {
      monitor.init();

      util.safeBind($(".jurisdiction-change-button"), "click",
        navigateJurisdiction.changeJurisdictionButtonClicked);
      util.safeBind($(".refresh-vs-elections-button"), "click",
        onClickRefreshVoteSmartElections);
      util.safeBind($(".refresh-vs-candidate-button"), "click",
        onClickRefreshVoteSmartCandidate);
      util.safeBind($(".prev-vusa-candidate-button"), "click",
        onClickPrevVoteUsaCandidate);
      util.safeBind($(".next-vusa-candidate-button"), "click",
        onClickNextVoteUsaCandidate);
      util.safeBind($(".refresh-vs-candidates-button"), "click",
        onClickRefreshVoteSmartCandidates);
      util.safeBind($(".update-button"), "click",
        onClickUpdate);
      util.safeBind($(".vusa-offices-dropdown"), "change",
        onChangeVoteUsaOfficesDropdown);
      util.safeBind($(".vusa-candidates-dropdown"), "change",
        onChangeVoteUsaCandidatesDropdown);
      util.safeBind($(".vs-elections-dropdown"), "change",
        onChangeVoteSmartElectionsDropdown);
      util.safeBind($(".vs-candidates-dropdown"), "change",
        onChangeVoteSmartCandidatesDropdown);
      util.safeBind($(".copy-buttons img"), "click",
        onClickCopyButton);
      util.safeBind($(".copy-empty-button"), "click",
        onClickCopyEmptyButton);

      util.registerDataMonitor($(".family .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".education .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".professional .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".military .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".civic .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".political .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".religion .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".accomplishments .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".birthdate .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".email .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".website .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".facebook .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".twitter .vusa-data .value"), { onChange: onChangeData });
      util.registerDataMonitor($(".youtube .vusa-data .value"), { onChange: onChangeData });

      window.onbeforeunload = function() {
        if (isDataChanged())
          return "There are entries on your form that have not been submitted";
      };

      $(".date-picker").datepicker({
        changeYear: true,
        yearRange: "2010:+1"
      });

      initElectionControl();

      var queryElection = $.queryString("election");
      if (queryElection) onSelectedElectionChanged(queryElection);
    }

    function isDataChanged() {
      return util.evaluateMonitoredElementsList(monitoredData);
    }

    var monitoredData = null;
    function monitorData() {
      monitoredData = util.getMonitoredElementsList(
        $(".family .vusa-data .value"),
        $(".education .vusa-data .value"),
        $(".professional .vusa-data .value"),
        $(".military .vusa-data .value"),
        $(".civic .vusa-data .value"),
        $(".political .vusa-data .value"),
        $(".religion .vusa-data .value"),
        $(".accomplishments .vusa-data .value"),
        $(".birthdate .vusa-data .value"),
        $(".email .vusa-data .value"),
        $(".website .vusa-data .value"),
        $(".facebook .vusa-data .value"),
        $(".twitter .vusa-data .value"),
        $(".youtube .vusa-data .value")
      );
      onChangeData();
    }

    function onChangeData() {
      if (monitoredData)
        $(".data-comparison-boxed-group .update-button").prop("disabled", !isDataChanged());
      $(".data-comparison-boxed-group .copy-empty-button").prop("disabled", !canCopyEmptyFields());
      $.each(socialMediaClasses, function() {
        var $comparison = $(".comparison." + this.toString());
        var $vsData = $(".vs-data .value", $comparison); 
        var vsData = getVsData($comparison);
        var vusaData = $.trim($(".vusa-data .value", $comparison).val());
        var mismatch = vsData && vusaData &&
          vsData.toLowerCase() !== vusaData.toLowerCase();
        $vsData.toggleClass("mismatch", !!mismatch);
      });
    }

    function onChangeVoteSmartCandidatesDropdown() {
      getVoteSmartCandidate();
    }

    function onChangeVoteSmartElectionsDropdown() {
      getVoteSmartCandidates();
    }

    function onChangeVoteUsaCandidatesDropdown() {
      getVoteUsaCandidate();
      autoLocateVoteSmartCandidate();
    }

    function onChangeVoteUsaOfficesDropdown(event, selectLast) {
      getVoteUsaCandidates(selectLast);
    }

    function getVsData($comparison) {
      var $vsData = $(".vs-data .value", $comparison);
      return $.trim($vsData[0].tagName === "DIV" ? $vsData.text() : $vsData.val());
    }

    function onClickCopyButton(event) {
      if (!getSelectedVoteUsaCandidate()) return;
      var $img = $(this);
      var op = getMagicClass($img, ["replace", "append", "prepend"]);
      var $comparison = $img.closest(".comparison");
      var dataType = getMagicClass($comparison, getTopicClasses(true));
      //var $vsData = $(".vs-data .value", $comparison);
      //var vsData = $.trim($vsData[0].tagName === "DIV" ? $vsData.text() : $vsData.val());
      var vsData = getVsData($comparison);
      var $vusaData = $(".vusa-data .value", $comparison);
      var vusaData = $.trim($vusaData.val());
      if (!vsData) return;

      switch (op) {
        case "replace":
          if (dataType === "photo") {
            util.confirm("Are you sure you want to replace the VoteUSA photo?",
              function(button) {
                if (button === "Ok")
                  replacePhoto(vsData);
              }
            );
          }
          else $vusaData.val(vsData);
          break;

        case "append":
          $vusaData.val(vusaData + "\n\n" + vsData);
          break;

        case "prepend":
          $vusaData.val(vsData + "\n\n" + vusaData);
          break;
      }
    }

    function onClickCopyEmptyButton() {
      $.each(getTopicClasses(), function () {
        copyEmptyField(this);
      });
    }

    function onClickNextVoteUsaCandidate() {
      var $candidates = $(".vusa-candidates-dropdown");
      var candidatesInx = $candidates.prop("selectedIndex") + 1;
      if (candidatesInx > 0 && candidatesInx < $candidates.children().length) {
        $candidates.prop("selectedIndex", candidatesInx);
        onChangeVoteUsaCandidatesDropdown();
      } else {
        var $offices = $(".vusa-offices-dropdown");
        var officesInx = $offices.prop("selectedIndex") + 1;
        if (officesInx > 0 && officesInx < $offices.children().length) {
          $offices.prop("selectedIndex", officesInx);
          onChangeVoteUsaOfficesDropdown();
        }
      }
    }

    function onClickPrevVoteUsaCandidate() {
      var $candidates = $(".vusa-candidates-dropdown");
      var candidatesInx = $candidates.prop("selectedIndex") - 1;
      if (candidatesInx >= 0) {
        $candidates.prop("selectedIndex", candidatesInx);
        onChangeVoteUsaCandidatesDropdown();
      } else {
        var $offices = $(".vusa-offices-dropdown");
        var officesInx = $offices.prop("selectedIndex") - 1;
        if (officesInx >= 0) {
          $offices.prop("selectedIndex", officesInx);
          onChangeVoteUsaOfficesDropdown(null, true);
        }
      }
    }

    function onClickRefreshVoteSmartCandidate() {
      var vsCandidateId = getSelectedVoteSmartCandidate();
      if (!vsCandidateId) return;
      var ajaxKey = "onClickRefreshVoteSmartCandidate";
      util.pushAjaxDialog(ajaxKey, "Refreshing VoteSmart Candidate...");
      util.ajax({
        url: "/Admin/VsWebService.asmx/RefreshCandidate",
        data: {
          vsCandidateId: vsCandidateId
        },
        success: function (result) {
          util.popAjaxDialog(ajaxKey);
          if (result.d)
            util.alert("Could not refresh VoteSmart Candidate:\n" + result.d);
          else getVoteSmartCandidate();
        },
        error: function (result) {
          util.popAjaxDialog(ajaxKey);
          util.alert(util.formatAjaxError(result, "Could not refresh VoteSmart Candidate"));
        }
      });
    }

    function onClickRefreshVoteSmartCandidates() {
      var vsElectionKey = getVoteSmartElectionKey();
      if (!vsElectionKey) return;
      var ajaxKey = "onClickRefreshVoteSmartCandidates";
      util.pushAjaxDialog(ajaxKey, "Refreshing VoteSmart Candidate List...");
      util.ajax({
        url: "/Admin/VsWebService.asmx/RefreshCandidates",
        data: {
          vsElectionKey: vsElectionKey
        },
        success: function (result) {
          util.popAjaxDialog(ajaxKey);
          if (result.d)
            util.alert("Could not refresh VoteSmart Candidate List:\n" + result.d);
          else getVoteSmartCandidates();
        },
        error: function (result) {
          util.popAjaxDialog(ajaxKey);
          util.alert(util.formatAjaxError(result, "Could not refresh VoteSmart Candidate List"));
        }
      });
    }

    function onClickRefreshVoteSmartElections() {
      var electionDesc = electionControl.getElectionDesc(getSelectedElection());
      var year = electionDesc.length >= 4 ? electionDesc.substr(0, 4) : "";
      var stateName = $(".client-state-name").val();
      var desc = year + " VoteSmart Elections for " + stateName;
      var ajaxKey = "onClickRefreshVoteSmartElections";
      util.pushAjaxDialog(ajaxKey, "Refreshing " + desc + "...");
      util.ajax({
        url: "/Admin/VsWebService.asmx/RefreshElections",
        data: {
          electionYear: year,
          stateCode: $(".client-state-code").val()
        },
        success: function (result) {
          util.popAjaxDialog(ajaxKey);
          if (result.d)
            util.alert("Could not refresh " + desc + ":\n" + result.d);
          else getVoteSmartElections();
        },
        error: function (result) {
          util.popAjaxDialog(ajaxKey);
          util.alert(util.formatAjaxError(result, "Could not refresh " + desc));
        }
      });
    }

    function onClickUpdate() {
      var ajaxKey = "onClickUpdate";
      util.pushAjaxDialog(ajaxKey, "Updating VoteUSA Candidate...");
      util.ajax({
        url: "/Admin/VsWebService.asmx/UpdateVoteUsaCandidate",
        data: {
          politicianKey: getSelectedVoteUsaCandidate(),
          family: $(".family .vusa-data .value").val(),
          education: $(".education .vusa-data .value").val(),
          professional: $(".professional .vusa-data .value").val(),
          military: $(".military .vusa-data .value").val(),
          civic: $(".civic .vusa-data .value").val(),
          political: $(".political .vusa-data .value").val(),
          religion: $(".religion .vusa-data .value").val(),
          accomplishments: $(".accomplishments .vusa-data .value").val(),
          birthdate: $(".birthdate .vusa-data .value").val(),
          email: $(".email .vusa-data .value").val(),
          website: $(".website .vusa-data .value").val(),
          facebook: $(".facebook .vusa-data .value").val(),
          twitter: $(".twitter .vusa-data .value").val(),
          youtube: $(".youtube .vusa-data .value").val()
        },
        success: function (result) {
          //monitorData();
          getVoteUsaCandidate(true);
        },
        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not update the VoteUSA Candidate"));
        },
        complete: function (result) {
          util.popAjaxDialog(ajaxKey);
        }
      });
    }

    function replacePhoto(url) {
      var ajaxKey = "replacePhoto";
      util.pushAjaxDialog(ajaxKey, "Updating VoteUSA Candidate photo...");
      util.ajax({
        url: "/Admin/VsWebService.asmx/UpdateVoteUsaImage",
        data: {
          politicianKey: getSelectedVoteUsaCandidate(),
          url: url
        },
        success: function (result) {
          $(".photo.comparison .vusa-data img").attr("src", result.d);
          $(".name.comparison .vusa-data img").attr("src", result.d.replace(/Profile300/g, "Headshot100"));
        },
        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not update the VoteUSA Candidate photo"));
        },
        complete: function (result) {
          util.popAjaxDialog(ajaxKey);
        }
      });
    }

    function setComparisonVisibility() {
      $(".data-comparison-boxed-group").toggleClass("hidden",
        !getSelectedVoteSmartCandidate() && !getSelectedVoteUsaCandidate());
    }

    function setSocialMediaLink(link, linkType, context) {
      var $val = $("." + linkType + " ." + context + " .value");
      var href = link;
      if (href)
        if (href.indexOf("@") >= 0)
          href = "mailto:" + href;
        else if (href.indexOf("://") < 0)
          href = "http://" + href;
      switch (context) {
        case "vusa-data":
          $val.val(link);
          break;
        case "vs-data":
          $val.safeHtml(link);
          break;
      }
      $("." + linkType + " ." + context + " a").attr("href", href || null)
         .toggleClass("disabled", !link);
    }


    //
    // Election Control
    //

    function initElectionControl() {
      electionControl.init({
        electionKey: $('.selected-election-key').val(),
        onSelectedElectionChanging: onSelectedElectionChanging,
        onSelectedElectionChanged: onSelectedElectionChanged,
        slimScrollOptions: {
          height: '584px',
          width: '200px',
          alwaysVisible: true,
          color: '#666',
          size: '12px'
        }
      });
    };

    function getSelectedElection() {
      return $('.selected-election-key').val();
    };

    function onSelectedElectionChanging(newElectionKey) {
      if (isDataChanged()) {
        util.confirm("There are unsaved changes.\n\n" +
          "Click OK to discard the changes load the new election.\n" +
          "Click Cancel to return to the changed data.",
          function (button) {
            if (button === "Ok")
              electionControl.changeElection(newElectionKey);
            else
              electionControl.toggleElectionControl(false);
          });
        return false;
      }
      return true;
    };

    function onSelectedElectionChanged(newElectionKey) {
      $('.selected-election-key').val(newElectionKey);
      electionControl.toggleElectionControl(false);
      var $heading = $("#selected-election-head");
      var electionDesc = electionControl.getElectionDesc(getSelectedElection());
      var year = electionDesc.length >= 4 ? electionDesc.substr(0, 4) : "";
      var stateName = $(".client-state-name").val();
      var $button = $(".refresh-vs-elections-button");
      var desc = year + " VoteSmart Elections for " + stateName;
      $heading.safeHtml(electionDesc);
      $heading.toggleClass("hidden", !electionDesc);
      $button.val("Refresh " + desc);
      $button.prop("disabled", !electionDesc);
      $(".vs-election-boxed-group").addClass("hidden");
      $(".vusa-office-candidate-boxed-group").addClass("hidden");
      if (electionDesc) {
        getVoteSmartElections();
        getVoteUsaOffices();
      }
    };

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
});