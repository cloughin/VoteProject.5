define([
  "jquery", "vote/adminMaster", "vote/util", "monitor",
    "vote/controls/managePoliticiansPanel", "jqueryui", "slimscroll", "dynatree", "ajaxfileupload"
  ],
  function ($, master, util, monitor, managePoliticiansPanel) {

    var mappingId;
    var processingRow;
    var lastDistrictSearchString;

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

    function updateContainerAddCandidates() {
      //$("#tab-addpolitician input.kalypto").kalypto({ toggleClass: "kalypto-checkbox" });
    }

    function afterRequestAddCandidates() {
      //var searchToRestore = $("#tab-addpolitician .search-to-restore").val();
      //if (searchToRestore) {
      //  $("#tab-addpolitician .search-to-restore").val("");
      //  $("#tab-addpolitician .find-politician-control input").val(searchToRestore);
      //  var key = $("#tab-addpolitician .key-to-delete").val();
      //  var keys = [];
      //  if (key) keys.push(key);
      //  managePoliticiansPanel.searchNameChanged(keys);
      //}
    }

    //
    //

    function initPage() {
      monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);
      monitor.registerCallback("afterRequest", afterRequest);
      monitor.init();

      managePoliticiansPanel.initControl({
        // onSelectionChanged: onFindPoliticianSelectionChanged
      });
      $("#tab-addpolitician .reloading").val("reloading");
      $("#tab-addpolitician input.update-button").addClass("reloading").click();

      $("#searchDistricts").dialog({
        autoOpen: false,
        width: "auto",
        resizable: false,
        title: "Search Counties and Districts in State",
        dialogClass: "search-districts-in-state",
        modal: true
      });
      $(".select-jurisdiction-button").on("click",
        function() {
          $('#searchDistricts .search-districts-text').val("");
          $('#searchDistricts .search-districts-select').html("");
          lastDistrictSearchString = "";
          $('#searchDistricts').dialog("open");
        });

      $("#searchDistricts").on("dblclick", ".search-districts-select p",
        function() {
          var key = $(this).data("key");
          $(".selected-jurisdiction").attr("data-key", key).html($(this).html());
          enableUi();
          $('#searchDistricts').dialog("close");
        });

      $('#searchDistricts .search-districts-text')
        .on("propertychange change click keyup input paste", searchDistrictsTextChanged);

      $("input[name=step-1-type-group]").on("change", function() {
        enableUi();
      });

      $("input[name=step-1-scope-group]").on("change", function() {
        var all = $.radioVal("step-1-scope-group") == "A";
        var $list = $(".spreadsheet-list");
        $list.html("");
        util.ajax({
          url: "/Admin/WebService.asmx/GetElectionSpreadsheets",
          data: {
            all: all
          },

          success: function(result) {
            util.closeAjaxDialog();
            $list.html(result.d);
            enableUi();
          },

          error: function(result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result,
              "Could not get spreadsheets"));
            enableUi();
          }
        });
      });

      $(".spreadsheet-list").on("click", "div", function() {
        var $this = $(this);
        if ($this.data("id")) {
          $(".spreadsheet-list div").removeClass("selected");
          $this.addClass("selected");
          enableUi();
        }
      });

      function updateFileInputText() {
        $(".file-input-button span").text($(".file-input").val().replace(/([^\\]*\\)*/, "") ||
          "No spreadsheet selected");
      }

      $(".file-input").on("change", function() {
        updateFileInputText();
        enableUi();
      });

      $(".select-state").on("change", function() {
        util.openAjaxDialog("Getting elections...");
        util.ajax({
          url: "/Admin/WebService.asmx/GetElectionsForState",
          data: {
            stateCode: $(".select-state").val()
          },

          success: function(result) {
            util.closeAjaxDialog();
            util.populateDropdown($(".select-election"), result.d);
            $("#step-1-contest-state").trigger("click");
            $("#step-1-primary-all").trigger("click");
            $(".step-1-primaries").addClass("hidden");
            enableUi();
          },

          error: function(result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result,
              "Could not get elections"));
          }
        });
      });

      $("input[name=step-1-contests-group]").on("change", function () {
        $(".select-jurisdiction-button").prop("disabled",
          !($.radioVal("step-1-contests-group") == "L") || !$(".select-state").val());
        $(".selected-jurisdiction").attr("data-key", null).html("");
        enableUi();
      });

      $(".select-election").on("change", function() {
        enableUi();
        $("#step-1-primary-all").trigger("click");
        $(".step-1-primaries").toggleClass("hidden", !isMultiplePrimary($(this).val()));
      });

      $(".upload-button").on("click", function() {
        if ($(this).hasClass("disabled")) return;
        util.openAjaxDialog("Uploading file...");
        var electionKey = $(".select-election").val();
        var jurisdictionScope = $.radioVal("step-1-contests-group");
        if (jurisdictionScope === "L")
          electionKey += $(".selected-jurisdiction").data("key");
        $.ajaxFileUpload({
          url: "/master/ajaxfileuploader.aspx",
          secureuri: false,
          fileElementId: "UploadFile",
          timeout: 60000,
          restoreFilename: true,
          dataType: "text",
          data: {
            electionKey: electionKey,
            jurisdictionScope: jurisdictionScope,
            electionScope: isMultiplePrimary(electionKey)
              ? $.radioVal("step-1-primary-group")
              : " "
          },
          success: function (result) {
            // convert text result to json (in pre)
            result = JSON.parse($(result).text());
            util.closeAjaxDialog();
            if (!result.Success) {
              util.alert(result.Message, "Could not upload file");
              return;
            }
            $(".spreadsheet-list").html(result.Html);
            $("#step-1-scope-inprocess").trigger("click");
            $("#step-1-type-existing").trigger("click");
          },
          error: function (result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result,
              "Could not upload file"));
          },
          complete: function () { updateFileInputText(); }
        });
      });

      $(".mapping-list").on("change", ".select-mapping", function() {
        enableUi();
        var $this = $(this);
        util.ajax({
          url: "/Admin/WebService.asmx/UpdateElectionSpreadsheetsMapping",
          data: {
            id: mappingId,
            sequence: $this.parent().data("sequence"),
            mapping: $this.val()
          }
        });
      });

      $(".row-spinner").on("spinstop", getRow)
        .spinner({
          min: 1,
          max: 1
        });

      $(".back-button").on("click", function() {
        var $this = $(this);
        if ($this.hasClass("disabled")) return;
        enableUi(getCurrentStep() - 1);
      });

      $(".next-button").on("click", function() {
        var $this = $(this);
        if ($this.hasClass("disabled")) return;
        var step = getCurrentStep();
        switch (step) {
        case 1: // going to 2
        {
          var $selected = $(".spreadsheet-list div.selected");
          var id = $selected.data("id");
          if (id != mappingId) {
            util.openAjaxDialog("Getting mappings...");
            util.ajax({
              url: "/Admin/WebService.asmx/GetElectionSpreadsheetsMapping",
              data: {
                id: id
              },

              success: function(result) {
                util.closeAjaxDialog();
                mappingId = id;
                processingRow = 0;
                $(".spreadsheet-description").text($selected.text());
                $(".mapping-list").html(result.d);
                var $spinner = $(".row-spinner");
                $spinner.val(1);
                var rows = $selected.data("rows");
                $spinner.spinner({ max: rows });
                $(".row-count").text(rows);
                $("#step-4-is-complete").prop("checked", $selected.data("completed"));
                enableUi();
              },

              error: function(result) {
                util.closeAjaxDialog();
                util.alert(util.formatAjaxError(result,
                  "Could not get mappings"));
              }
            });
          }
          break;
        }

        case 2: //going to 3
        {
          function checkDuplicateMaps($maps) {
            if ($maps.length > 1) {
              util.alert("Duplicate mapping for " + $maps.first().text());
              return true;
            }
          }

          function joinMapNames() {
            var names = [];
            for (var x = 0; x < arguments.length; x++)
              if (arguments[x].length)
                names.push(arguments[x].first().text());
            return names.join(", ");
          }

          var $mappingList = $(".mapping-list");
          $(".back-button").removeClass("disabled");
          var $nameFullMaps = $("option[value=NAMEFULL]:selected", $mappingList);
          var $lastFirstMaps = $("option[value=LASTFIRST]:selected", $mappingList);
          var $firstNameMaps = $("option[value=FIRSTNAME]:selected", $mappingList);
          var $middleNameMaps = $("option[value=MIDDLENAME]:selected", $mappingList);
          var $nicknameMaps = $("option[value=NICKNAME]:selected", $mappingList);
          var $lastNameMaps = $("option[value=LASTNAME]:selected", $mappingList);

          // check duplicate mappings
          var hasDuplicate = false;
          $("select", $mappingList).first().find("option").each(function() {
            var $this = $(this);
            if (!$this.data("multiple") && $this.val() &&
              checkDuplicateMaps($("option[value=" + $this.val() + "]:selected",
                $mappingList))) {
                hasDuplicate = true;
                return false;
            }
          });
          if (hasDuplicate) return;

          var hasComponentNames = $firstNameMaps.length ||
            $middleNameMaps.length ||
            $nicknameMaps.length ||
            $lastNameMaps.length;

          if ((Number(hasComponentNames) + $nameFullMaps.length + $lastFirstMaps.length) ==
            0) {
            util.alert("A name mapping is required");
            return;
          }

          if ((Number(hasComponentNames) + $nameFullMaps.length + $lastFirstMaps.length) >
            1) {
            util.alert("Conflicting mappings: " +
              joinMapNames($nameFullMaps, $lastFirstMaps,
                $firstNameMaps, $middleNameMaps, $nicknameMaps, $lastNameMaps));
            return;
          }

          if (hasComponentNames &&
            ($firstNameMaps.length == 0 || $lastNameMaps.length == 0)) {
            util.alert("At least a First Name and Last Name is required.");
            return;
              }

          if ($(".row-spinner").val() != processingRow)
            getRow();

          break;
        }
        }
        enableUi(step + 1);
      });
    }

    function searchDistrictsTextChanged() {
      var text = $.trim($('#searchDistricts .search-districts-text').val());
      var $searchDistrictsSelect = $('#searchDistricts .search-districts-select');
      if (text.length >= 2) {
        // only act on first event triggered
        if (text != lastDistrictSearchString) {
          var data = {
            stateCode: $(".select-state").val(),
            searchString: text,
            includeCounties: true
          };
          util.ajax({
            url: "/Admin/WebService.asmx/GetSearchDistrictsByState",
            data: data,
            success: function (result) {
              // guard against race condition
              if (text == $.trim($('#searchDistricts .search-districts-text').val())) {
                var lines = [];
                for (var n = 0; n < result.d.length; n++) {
                  var i = result.d[n];
                  lines.push('<p data-key="' + i.Value + '">' + i.Text + '</p>');
                }
                $searchDistrictsSelect.html(lines.join(""));
                lastDistrictSearchString = text;
              }
            }
          });
        }
      } else {
        $searchDistrictsSelect.html("");
        lastDistrictSearchString = "";
      }
    }

    function isMultiplePrimary(electionKey) {
      if (!electionKey || !isPrimaryType(electionKey.substr(10, 1)))
        return false;
      // check if multiple
      var matches = 0;
      var stateDate = electionKey.substr(0, 10);
      $(".select-election option").each(function() {
        if ($(this).val().substr(0, 10) === stateDate)
          matches++;
      });
      return matches > 1;
    }

    function isPrimaryType(electionType) {
      return electionType && "ABPQ".indexOf(electionType.toUpperCase()) >= 0;
    }

    //function getRadioVal($group) {
    //  if ($.type($group) === "string")
    //    $group = $($group);
    //  return $(".radio-button.checked", $group).data("val");
    //}

    function getCurrentStep() {
      for (var s = 1; s <= 4; s++) {
        if (!$(".step-" + s).hasClass("hidden")) return s;
      }
      return -1;
    }

    function formatRawData(data, mappings) {

      function isSelected($mapping) {
        var result = false;
        $.each(mappings, function() {
          if ($(".select-mapping", $mapping).val() === this.toString()) {
            result = true;
            return false;
          }
        });
        return result;
      }

      var items = [];
      $(".one-mapping").each(function (index) {
        var $this = $(this);
        if (isSelected($this)) {
          items.push('<span class="label">' + util.htmlEscape($("option:selected", $this).text()) + " [" + util.htmlEscape($(".column-name", this).text()) + "]:</span> " + (util.htmlEscape($.trim(data[index])) || "&lt;empty&gt;"));
        }
      });

      return items.join("<br\>", items);
    }

    function getRow() {
      var sequence = $(".row-spinner").val() - 1; // 0-based
      util.openAjaxDialog("Getting row...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetElectionSpreadsheetsRow",
        data: {
          id: mappingId,
          sequence: sequence
        },

        success: function(result) {
          util.closeAjaxDialog();
          processingRow = $(".row-spinner").val();
          var f = formatRawData(result.d.Columns, ["NAMEFULL", "LASTFIRST", "FIRSTNAME",
            "MIDDLENAME", "NICKNAME", "LASTNAME", "NAMESUFFIX"]);
          $(".candidate-raw .raw").html(f);
          enableUi();
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not get row"));
        }
      });
    }

    function enableUi(step) {
      if (step) {
        $(".step").addClass("hidden");
        $(".step-" + step).removeClass("hidden");
        $(".step-image").removeClass("active");
        $(".step-image-" + step).addClass("active");
      }
      step = getCurrentStep();
      $(".back-button").addClass("disabled");
      $(".next-button").addClass("disabled");
      $(".back-button").removeClass("hidden-important");
      $(".next-button").removeClass("hidden-important");
      switch (step) {
      case 1:
        $(".step-1-option").removeClass("hidden");
          if ($.radioVal("step-1-type-group") == "E") {
          $(".step-1-upload").addClass("hidden");
          $(".next-button")
            .toggleClass("disabled", !$(".spreadsheet-list div.selected").length);
        } else {
          $(".step-1-existing").addClass("hidden");
          $(".upload-button").toggleClass("disabled", !$(".file-input")[0].files.length ||
            !$(".select-state").val() ||
            !$(".select-election").val() ||
            $.radioVal("step-1-contests-group") == "L" && !$(".selected-jurisdiction").data("key")
            );
        }
        $(".back-button").addClass("hidden-important");
        break;

      case 2:
        $(".back-button").removeClass("disabled");
        $(".next-button").removeClass("disabled");
        break;

      case 3:
        $(".back-button").removeClass("disabled");
        $(".next-button").removeClass("disabled");
        break;

      case 4:
        $(".back-button").removeClass("disabled");
        $(".next-button").addClass("hidden-important");
        break;
      }
    }

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });