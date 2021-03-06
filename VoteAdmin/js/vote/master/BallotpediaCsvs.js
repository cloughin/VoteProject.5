define(["jquery", "vote/adminMaster", "vote/util", "monitor", "store",
 "jqueryui", "resizablecolumns", "stupidtable"],
  function($, master, util, monitor, store) {

    var $$ = util.$$;

    var csvListSelector = ".uploaded-csvs table";
    var candidateListSelector = ".loaded-candidates table";
    var candidatesChanged = false;

    function afterUpload() {
      var overwrite = monitor.getGroup("mc-group-upload-overwrite");
      var saveas = monitor.getGroup("mc-group-upload-saveas");
      if (overwrite) overwrite._dataType.val($$(overwrite.data), false);
      if (saveas) saveas._dataType.val($$(saveas.data), "");
      populateCsvList();
    }


    function beginUpload(group) {
      var overwrite = monitor.getGroup("mc-group-upload-overwrite");
      var saveas = monitor.getGroup("mc-group-upload-saveas");
      if (overwrite && saveas)
        group._uploadData = {
          overwrite: overwrite._dataType.val($$(overwrite.data)),
          saveas: saveas._dataType.val($$(saveas.data))
        };
    }

    function canUnloadCsv(afterUnload) {
      if (!checkCandidatesChanged()) return true;
      util.confirm("There are unsaved changes to candidate ids. Do you want to" +
        " abandon the changes?",
        function(button) {
          if (button === "Ok") {
            afterUnload();
          }
        });
      return false;
    }

    function checkCandidatesChanged() {
      // only incomplete candidates can change, and they all start with empty ids
      // so if any incomplete candidates have an id value, the candidates have changed
      var changed = false;
      $("td.incomplete input", $(candidateListSelector)).each(function() {
        if ($.trim($(this).val())) {
          changed = true;
          return false;
        }
      });
      return changed;
    }

    function enableApplyButton() {
      $(".apply-button input").prop("disabled", !getSelectedCsvListRow().length);
    }

    function enableSelectedCsvButtons() {
      var disabled = !getSelectedCsvListRow().length;
      $(".rename-button input").prop("disabled", disabled);
      $(".delete-button input").prop("disabled", disabled);
      $(".download-button input").prop("disabled", disabled);
    }

    function enableUpdateButton() {
      // enable if there are candidate id changes 
      // or if the complete box has changed
      var completeChanged = isMarkedComplete() !== getSelectedCompleteFlag();
      $(".update-button input").prop("disabled", !candidatesChanged && !completeChanged);
    }

    function getCsvListRow(target) {
      return $(target).closest("tbody tr", $(csvListSelector));
    }

    function getSelectedCompleteFlag() {
      return $("td.completed", getSelectedCsvListRow())
        .attr("data-sort-value") === "1";
    }

    function getSelectedCsvListRow() {
      return $("tbody tr.selected", $(csvListSelector));
    }

    function getSelectedCsvId() {
      return getSelectedCsvListRow().attr("rel");
    }

    function getSelectedFilename() {
      return $("td.filename", getSelectedCsvListRow())
        .html();
    }

    function initPage() {
      monitor.registerCallback("afterUpload", afterUpload);
      monitor.registerCallback("beginUpload", beginUpload);
      monitor.init();

      $(".fileinputs .file-name-clear").click(function() {
        monitor.clearFilename('mc-group-upload');
      });
      $(".upload-boxed-group .upload-button").click(function(event) {
        monitor.upload(event.target);
      });
      $(".rename-button input").click(onClickRename);
      $(".delete-button input").click(onClickDelete);
      $(".download-button input").click(onClickDownload);
      $(".update-button input").click(onClickUpdate);
      $(".apply-button input").click(onClickApplyLinks);

      $(".loaded-candidates").on("click", ".info-arrow", function() {
        var $this = $(this);
        $(".incomplete input", $this.closest("tr")).val($this.attr("rel"));
      });

      $('[placeholder]').placeholder();

      $(".input-element.showallcsvs input").on("rc_checked rc_unchecked",
        onClickShowAllCsvs);

      $(".input-element.markcomplete input").on("rc_checked rc_unchecked",
        enableUpdateButton);

      $(".input-element.showallcandidates input").on("rc_checked rc_unchecked",
        function() { onShowAllCandidatesChanged(); });

      window.onbeforeunload = function() {
        if (monitor.hasChanges() || candidatesChanged)
          return "There are entries on your form that have not been submitted";
      };

      populateCsvList();

      initRename();

      // start the checkCandidatesChanged poll
      setInterval(function() {
        var changed = checkCandidatesChanged();
        if (changed != candidatesChanged) {
          candidatesChanged = changed;
          enableUpdateButton();
        }
      }, 200);
    }

    function isMarkedComplete() {
      return $(".input-element.markcomplete input").prop("checked");
    }

    function loadCandidates(candidates) {
      var all = $(".input-element.showallcandidates input").prop("checked");
      var dirs = $.fn.stupidtable.dir;
      var savedSort = util.saveSort($(candidateListSelector), { col: ".office", dir: dirs.ASC });

      var rows = [];
      var index = 0;
      var coded = 0;
      var complete = getSelectedCompleteFlag();
      $.each(candidates, function() {
        var idCell;
        var rowHidden = 'class="hidden" ';
        if (this.VoteUsaId) coded++;
        else rowHidden = "";
        if (this.VoteUsaId || complete)
          idCell = '<td>' + this.VoteUsaId + '</td>';
        else
          idCell = '<td class="incomplete"><input type="text" /></td>';
        if (all) rowHidden = "";
        var proposedNameCell;
        if (this.Proposed.length && !complete) {
          var proposed = [];
          $.each(this.Proposed, function() {
            proposed.push('<span class="info-arrow" title="' + this.Status + '" rel="' + this.VoteUsaId + '">&#9668;</span>' + this.Name);
          });
          proposedNameCell = '<td>' + proposed.join('<br />') + '</td>';
        }
        else {
          proposedNameCell = '<td></td>';
        }
        rows.push(
          '<tr ' + rowHidden + 'rel="' + index++ + '">' +
          '<td data-sort-value="' + this.SortName + '" title="' + this.SplitName + '">' + this.Name + '</td>' +
          '<td>' + this.StateCode + '</td>' +
          '<td>' + this.Office + '</td>' +
          '<td>' + this.Party + '</td>' +
          '<td data-sort-value="' + (this.IsIncumbent ? '1' : '0') + '">' + (this.IsIncumbent ? '√' : '&nbsp;') + '</td>' +
          idCell +
          proposedNameCell +
          '</tr>'
        );
        $(".loaded-candidates").html(
          '<table data-resizable-columns-id="loaded-candidates"><thead><tr>' +
          '<th class="name" data-sort="string-ins" data-resizable-column-id="name"><div class="label">Name</div><div class="sort-ind"></th>' +
          '<th class="statecode" data-sort="string-ins" data-resizable-column-id="statecode"><div class="label">State</div><div class="sort-ind"></th>' +
          '<th class="office" data-sort="string-ins" data-resizable-column-id="office"><div class="label">Office</div><div class="sort-ind"></th>' +
          '<th class="party" data-sort="string-ins" data-resizable-column-id="party"><div class="label">Party</div><div class="sort-ind"></th>' +
          '<th class="isincumbent" data-sort="int" data-resizable-column-id="isincumbent"><div class="label">Incumbent</div><div class="sort-ind"></th>' +
          '<th class="voteusaid" data-sort="string-ins" data-resizable-column-id="voteusaid"><div class="label">VoteUSA Id</div><div class="sort-ind"></th>' +
          '<th class="proposedname" data-sort="string-ins" data-resizable-column-id="proposedname"><div class="label">Possible Matches</div><div class="sort-ind"></th>' +
          '</tr></thead><tbody>' + rows.join("") + '</tbody></table>');
      });
      $(".listcandidates-boxed-group .line2").removeClass("hidden");

      // attach events
      var $table = $(candidateListSelector);
      $table.resizableColumns({ store: store });
      var table = $table.stupidtable();
      table.safeBind("aftertablesort", function(event, data) {
        $table.find("th div.sort-ind").removeClass("asc desc");
        var dir = data.direction === dirs.ASC ? "asc" : "desc";
        $("div.sort-ind", $table.find("th").eq(data.column)).addClass(dir);
        util.assignRotatingClassesToChildren($table, ["odd", "even"]);
      });

      util.restoreSort($table, savedSort);

      var $markComplete = $(".input-element.markcomplete .kalypto-container");
      $("input", $markComplete).prop("checked", complete);
      $("a", $markComplete).toggleClass("checked", complete);

      // update "coded" count
      $("td.coded", getSelectedCsvListRow()).html(coded);

      if (!all) warnNoIncompleteCandidates();
    }

    function loadCsvList(csvs, selectedId) {
      var dirs = $.fn.stupidtable.dir;
      var savedSort = util.saveSort($(csvListSelector), { col: ".filename", dir: dirs.ASC });

      var rows = [];
      $.each(csvs, function() {
        var uploadTime = moment(this.UploadTime);
        rows.push(
          '<tr rel="' + this.Id + '">' +
          '<td class="filename">' + this.Filename + '</td>' +
          '<td data-sort-value="' + uploadTime.format("X") + '">' + uploadTime.format("M/D/YYYY h:mm:ss A") + '</td>' +
          '<td>' + this.Candidates + '</td>' +
          '<td class="coded">' + this.CandidatesCoded + '</td>' +
          '<td class="completed" data-sort-value="' + (this.Completed ? '1' : '0') + '">' + (this.Completed ? '√' : '&nbsp;') + '</td>' +
          '</tr>'
        );
      });
      $(".uploaded-csvs").html(
        '<table data-resizable-columns-id="uploaded-csvs"><thead><tr>' +
        '<th class="filename" data-sort="string-ins" data-resizable-column-id="filename"><div class="label">Filename</div><div class="sort-ind"></th>' +
        '<th class="uploadtime" data-sort="int" data-resizable-column-id="uploadtime"><div class="label">Upload Time</div><div class="sort-ind"></th>' +
        '<th class="candidates" data-sort="int" data-resizable-column-id="candidates"><div class="label">Candidates</div><div class="sort-ind"></th>' +
        '<th class="coded" data-sort="int" data-resizable-column-id="coded"><div class="label">Coded</div><div class="sort-ind"></th>' +
        '<th class="completed" data-sort="int" data-resizable-column-id="completed"><div class="label">Completed</div><div class="sort-ind"></th>' +
        '</tr></thead><tbody>' + rows.join("") + '</tbody></table>');

      // attach events
      var $table = $(csvListSelector);
      $table.safeBind("click", onClickCsvList).resizableColumns({ store: store });
      var table = $table.stupidtable();
      table.safeBind("aftertablesort", function(event, data) {
        $table.find("th div.sort-ind").removeClass("asc desc");
        var dir = data.direction === dirs.ASC ? "asc" : "desc";
        $("div.sort-ind", $table.find("th").eq(data.column)).addClass(dir);
        util.assignRotatingClassesToChildren($table, ["odd", "even"]);
      });

      util.restoreSort($table, savedSort);
      enableSelectedCsvButtons();
      enableApplyButton();
      if (selectedId)
        $('tr[rel="' + selectedId + '"]', $table).addClass("selected");
    }

    function onClickApplyLinks(event, force) {
      if (!force && checkCandidatesChanged()) {
        util.confirm("There are unsaved changes to candidate ids. These changes" +
          " will not be used when applying the links. Do you want to continue?",
          function(button) {
            if (button === "Ok") {
              onClickApplyLinks(event, true);
            }
          });
        return;
      }

      util.openAjaxDialog("Updating BallotPedia Links...");
      util.ajax({
        url: "/Admin/WebService.asmx/ApplyBallotPediaLinks",
        data: {
          csvId: getSelectedCsvId()
        },

        success: function (result) {
          // update "completed"
          util.alert(result.d);
        },

        error: function (result) {
          util.alert(util.formatAjaxError(result,
            "Could not apply links"));
        },

        complete: function () {
          util.closeAjaxDialog();
        }
      });
    }

    function onClickCsvList(event, force) {
      var forceUnload = function () { onClickCsvList(event, true); };
      var $tr = getCsvListRow(event.target);
      if ($tr.length) {
        if ($tr.hasClass("selected")) {
          if (force || canUnloadCsv(forceUnload)) {
            $tr.removeClass("selected");
            unloadCandidates();
          }
        } else {
          var $selected = $("tr.selected", $tr.closest("tbody"));
          if ($selected.length == 0 || force || canUnloadCsv(forceUnload)) {
            $selected.removeClass("selected");
            $tr.addClass("selected");
            populateCandidates();
          }
        }
        enableSelectedCsvButtons();
        enableApplyButton();
      }
    }

    function onClickDelete() {
      if (getSelectedCsvId()) {
        util.confirm("Are you sure you want to delete " + getSelectedFilename() + "?",
          function(button) {
            if (button === "Ok") {
              var id = getSelectedCsvId();
              util.openAjaxDialog("Deleting CSV...");
              util.ajax({
                url: "/Admin/WebService.asmx/DeleteBallotPediaCsv",
                data: {
                  id: id
                },

                success: function () {
                  unloadCandidates();
                  populateCsvList();
                },

                error: function (result) {
                  var message = 'Could not delete "' + 
                    util.htmlEscape(getSelectedFilename()) + '"';
                  util.alert(util.formatAjaxError(result, message));
                },

                complete: function () {
                  util.closeAjaxDialog();
                }
              });
            }
          });
      }
    };

    function onClickDownload() {
      var id = getSelectedCsvId();
      if (id) {
        $(".download-button a")
        .attr("href", "/master/downloadcsv.aspx?id=" + id)[0]
        .click();
      }
    }

    function onClickRename() {
      if (getSelectedCsvId()) {
        openRename();
      }
    };

    function onClickShowAllCsvs() {
      var all = $(".input-element.showallcsvs input").prop("checked");
      if (!all && getSelectedCompleteFlag()) {
        // unload the read-only complete CSV
        unloadCandidates();
      }
      populateCsvList();
    }

    function onClickUpdate() {
      util.openAjaxDialog("Updating BallotPedia CSV...");

      // build array of changed ids
      var candidateIds = [];
      $("td.incomplete input", $(candidateListSelector)).each(function() {
        var $this = $(this);
        var candidateId = $.trim($(this).val());
        if (candidateId) {
          var rowId = $this.closest("tr").attr("rel");
          candidateIds.push({ Value: rowId, Text: candidateId });
        }
      });

      util.ajax({
        url: "/Admin/WebService.asmx/UpdateBallotPediaCsv",
        data: {
          csvId: getSelectedCsvId(),
          isComplete: isMarkedComplete(),
          candidateIds: candidateIds
        },

        success: function (result) {
          // update "completed"
          var complete = isMarkedComplete();
          $("td.completed", getSelectedCsvListRow())
           .html(complete ? '√' : '&nbsp;')
           .attr("data-sort-value", complete ? '1' : '0');
          util.alert(result.d.Message, function () {
            populateCandidates();
          });
        },

        error: function (result) {
          util.alert(util.formatAjaxError(result,
            "Could updates CSV"));
        },

        complete: function () {
          util.closeAjaxDialog();
        }
      });
    }

    function onShowAllCandidatesChanged() {
      var all = $(".input-element.showallcandidates input").prop("checked");
      var $table = $(candidateListSelector);
      if (all) $("tbody tr", $table).removeClass("hidden");
      else {
        $("tbody tr", $table).addClass("hidden");
         $("td.incomplete", $table).each(function () {
          $(this).closest("tr").removeClass("hidden");
        });
      }
      util.assignRotatingClassesToChildren($table, ["odd", "even"]);
      if (!all) warnNoIncompleteCandidates();
    }

    function populateCandidates() {
      var id = getSelectedCsvId();
      if (id) {
        util.openAjaxDialog("Getting BallotPedia CSV candidates...");
        util.ajax({
          url: "/Admin/WebService.asmx/GetBallotPediaCsv",
          data: {
            id: id
          },

          success: function (result) {
            if (result.d.length)
              loadCandidates(result.d);
            else {
              $(".loaded-candidates")
                .html('<p> The are no candidates in ths CSV</p>');
              $(".listcandidates-boxed-group .line2").removeClass("hidden");
            }
          },

          error: function (result) {
            util.alert(util.formatAjaxError(result, "Could not get candidates for this CSV"),
             function() {
               $("tr.selected", $(csvListSelector)).removeClass("selected");
               enableSelectedCsvButtons();
             });
            $(".loaded-candidates").html("<p>Could not get candidates for this CSV</p>");
            $(".listcandidates-boxed-group .line2").removeClass("hidden");
          },

          complete: function () {
            util.closeAjaxDialog();
          }
        });
      }
    }

    function populateCsvList() {
      var all = $(".input-element.showallcsvs input").prop("checked");
      var selectedId = getSelectedCsvId();
      util.openAjaxDialog("Getting BallotPedia CSV list...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetUploadedBallotPediaCsvs",
        data: {
          all: all
        },

        success: function (result) {
          if (result.d.length)
            loadCsvList(result.d, selectedId);
          else
            $(".uploaded-csvs").html('<p> The are no ' +
             (all ? "" : "incomplete") + " BallotPedia CSVs</p>");
        },

        error: function (result) {
          util.alert(util.formatAjaxError(result, 
            "Could not get BallotPedia CSV list"));
          $(".uploaded-csvs").html("<p>Could not get BallotPedia CSV list</p>");
        },

        complete: function() {
          util.closeAjaxDialog();
        }
      });
    }

    function unloadCandidates() {
      $(candidateListSelector).html("");
      $(".listcandidates-boxed-group .line2").addClass("hidden");
    }

    function warnNoIncompleteCandidates() {
      // check if ther are any unhidden tbody rows (ie,in complete)
      if (!$("tbody tr:not(.hidden)", $(candidateListSelector)).length)
        util.alert("There are no incomplete candidates in this CSV");
    }

    //
    // The Rename Dialog
    //

    var renameDialogName = "ballotpedia-csv-rename-dialog";
    var $renameDialog;

    function closeRename() {
      $renameDialog.dialog("close");
    }

    function initRename() {
      $renameDialog = $$(renameDialogName);
      $renameDialog.dialog({
        autoOpen: false,
        dialogClass: "email-template-dialog email-template-rename-dialog " + renameDialogName,
        modal: true,
        resizable: false,
        title: "Rename BallotPedia CSV",
        width: "auto"
      });
      $('.cancel-button', $renameDialog).safeBind("click", closeRename);
      $('.rename-button', $renameDialog).safeBind("click", doRename);
    }

    var doRename = function () {
      var id = getSelectedCsvId();
      if (id) {
        var newName = $.trim($(".new-name input", $renameDialog).val());
        if (!newName) return;
        var lcNewName = newName.toLowerCase();
        if (lcNewName === getSelectedFilename().toLowerCase()) {
          util.alert("The new name is the same as the old name.");
          return;
        }

        util.openAjaxDialog("Renaming...");
        util.ajax({
          url: "/Admin/WebService.asmx/RenameBallotpediaCsv",
          data: {
            id: id,
            newName: newName
          },

          success: function (result) {
            if (result.d) {
              util.alert(result.d);
            } else {
              $("td.filename", getSelectedCsvListRow())
               .safeHtml(newName);
              closeRename();
            }
          },

          error: function (result) {
            util.alert(util.formatAjaxError(result, 'Could not rename the CSV'));
          },

          complete: function () {
            util.closeAjaxDialog();
          }
        });
      }
    };

    var openRename = function () {
      var filename = getSelectedFilename();
      if (filename) {
        $(".original", $renameDialog).safeHtml(filename);
        $(".new-name input", $renameDialog).val(filename);
        $renameDialog.dialog("open");
        $(".new-name input", $renameDialog).select();
      }
    };

    master.inititializePage({
      callback: initPage
    });
});