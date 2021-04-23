define([
    "jquery", "vote/adminMaster", "vote/util", "monitor", "store",
    "vote/controls/navigateJurisdiction",
    "jqueryui", "slimscroll", "dynatree", "resizablecolumns", "alphanum"
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

        case "mc-group-adddistricts":
          afterUpdateContainerAddDistricts();
          break;

        case "mc-group-deletedistricts":
          afterUpdateContainerDeleteDistricts();
          break;

        case "mc-group-setupcitycouncil":
          afterUpdateContainerSetupCityCouncil();
          break;

        case "mc-group-setupschooldistrict":
          afterUpdateContainerSetupSchoolDistrict();
          break;

        case "mc-group-tigersettings":
          afterUpdateContainerTigerSettings();
          break;

        case "mc-group-masteronly":
          afterUpdateContainerMasterOnly();
          break;
      }
    };

    function initRequest(group) {
      if (!group) return true;
      switch (group.group) {
        case "mc-group-deletedistricts":
          return initRequestDeleteDistricts(group);
      }
      return true;
    }

    //
    // Supervisors Custom Data Type
    //

    supervisorsList.prototype = new monitor.DataType();
    supervisorsList.prototype.constructor = supervisorsList;
    supervisorsList.prototype.parent = monitor.DataType.prototype;
    function supervisorsList() {
    }

    supervisorsList.prototype.name = "SupervisorsList";

    supervisorsList.prototype.bindChange = function ($data) {
      $data.parent().on("propertychange change click keyup input paste", "*", 
        function(event) {
          if (this == $data[0]) monitor.dataChanged(event);
          else $data.trigger("change");
        });
    };

    supervisorsList.prototype.dataChanged = function (group, $data) {
      $data.prev().val(this.val($data));
    };

    supervisorsList.prototype.handles = function ($data) {
      var o = $data[0];
      return o.tagName.toLowerCase() === "div" && $data.hasClass("supervisors-list");
    };

    supervisorsList.prototype.unbindChange = function ($data) {
      $data.parent().on("propertychange change click keyup input paste", "*");
    };

    supervisorsList.prototype.val = function ($data, value) {
      if (typeof (value) === "undefined") {
        var supervisors = [];
        $("tbody tr", $data).each(function() {
          var $tr = $(this);
          supervisors.push({
            "New": $tr.hasClass("new"),
            "Delete": $(".delete input", $tr).prop("checked"),
            "Create": $(".create input", $tr).prop("checked"),
            "Exists": $(".exists input", $tr).prop("checked"),
            "Id": $(".code input", $tr).val(),
            "InShapefile": $(".in-shapefile input", $tr).prop("checked"),
            "Name": $(".name input", $tr).val()
            });
        });
        return JSON.stringify(supervisors);
      } else {
        // set not needed
        return $data;
      }
    };

    // ReSharper disable once InconsistentNaming
    monitor.registerDataType(new supervisorsList());

    //
    // Council Custom Data Type
    //

    councilList.prototype = new monitor.DataType();
    councilList.prototype.constructor = councilList;
    councilList.prototype.parent = monitor.DataType.prototype;
    function councilList() {
    }

    councilList.prototype.name = "CouncilList";

    councilList.prototype.bindChange = function ($data) {
      $data.parent().on("propertychange change click keyup input paste", "*",
        function (event) {
          if (this == $data[0]) monitor.dataChanged(event);
          else $data.trigger("change");
        });
    };

    councilList.prototype.dataChanged = function (group, $data) {
      $data.prev().val(this.val($data));
    };

    councilList.prototype.handles = function ($data) {
      var o = $data[0];
      return o.tagName.toLowerCase() === "div" && $data.hasClass("council-list");
    };

    councilList.prototype.unbindChange = function ($data) {
      $data.parent().on("propertychange change click keyup input paste", "*");
    };

    councilList.prototype.val = function ($data, value) {
      if (typeof (value) === "undefined") {
        var council = [];
        $("tbody tr", $data).each(function () {
          var $tr = $(this);
          council.push({
            "New": $tr.hasClass("new"),
            "Delete": $(".delete input", $tr).prop("checked"),
            "Create": $(".create input", $tr).prop("checked"),
            "Exists": $(".exists input", $tr).prop("checked"),
            "Id": $(".code input", $tr).val(),
            "InShapefile": $(".in-shapefile input", $tr).prop("checked"),
            "District": $(".district input", $tr).val(),
            "Name": $(".name input", $tr).val()
          });
        });
        return JSON.stringify(council);
      } else {
        // set not needed
        return $data;
      }
    };

    // ReSharper disable once InconsistentNaming
    monitor.registerDataType(new councilList());

    //
    // SubDistrict Custom Data Type
    //

    subdistrictList.prototype = new monitor.DataType();
    subdistrictList.prototype.constructor = subdistrictList;
    subdistrictList.prototype.parent = monitor.DataType.prototype;
    function subdistrictList() {
    }

    subdistrictList.prototype.name = "SubDistrictList";

    subdistrictList.prototype.bindChange = function ($data) {
      $data.parent().on("propertychange change click keyup input paste", "*",
        function (event) {
          if (this == $data[0]) monitor.dataChanged(event);
          else $data.trigger("change");
        });
    };

    subdistrictList.prototype.dataChanged = function (group, $data) {
      $data.prev().val(this.val($data));
    };

    subdistrictList.prototype.handles = function ($data) {
      var o = $data[0];
      return o.tagName.toLowerCase() === "div" && $data.hasClass("subdistrict-list");
    };

    subdistrictList.prototype.unbindChange = function ($data) {
      $data.parent().on("propertychange change click keyup input paste", "*");
    };

    subdistrictList.prototype.val = function ($data, value) {
      if (typeof (value) === "undefined") {
        var subdistrict = [];
        $("tbody tr", $data).each(function () {
          var $tr = $(this);
          subdistrict.push({
            "New": $tr.hasClass("new"),
            "Delete": $(".delete input", $tr).prop("checked"),
            "Create": $(".create input", $tr).prop("checked"),
            "Exists": $(".exists input", $tr).prop("checked"),
            "Id": $(".code input", $tr).val(),
            "InShapefile": $(".in-shapefile input", $tr).prop("checked"),
            "Name": $(".name input", $tr).val()
          });
        });
        return JSON.stringify(subdistrict);
      } else {
        // set not needed
        return $data;
      }
    };

    // ReSharper disable once InconsistentNaming
    monitor.registerDataType(new subdistrictList());

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
    // AddDistricts
    //

    var afterUpdateContainerAddDistricts = function () {
      var $entireCounty = $("#tab-adddistricts .add-districts-entire-county");
      $entireCounty
        .on("rc_checked", function () {
          $("#tab-adddistricts .tiger-selection").addClass("hidden");
        })
        .on("rc_unchecked", function () {
          $("#tab-adddistricts .tiger-selection").removeClass("hidden");
        })
        .trigger($entireCounty.hasClass("checked") ? "rc_checked" : "rc_unchecked");
    };

    $("#tab-adddistricts").on("change", ".tiger select", function() {
      var $name = $("#tab-adddistricts .input-element.localdistrict input[type=text]");
      if (!$.trim($name.val()))
        $name.val($("option:selected", $(this)).text());
    });

    //
    // DeleteDistricts
    //

    function initRequestDeleteDistricts(group) {
      if ($$(group.button).hasClass("validated")) {
        $$(group.button).removeClass("validated");
        return true;
      }
      var $localKey = $("#tab-deletedistricts .input-element.localkey select");
      var $override = $("#tab-deletedistricts .input-element.override input");
      if (!$localKey.val()) return false;
      if ($override.prop("checked")) return true;
      util.confirm("Are you sure you want to delete district " + 
        $("option:selected", $localKey).text() + ". This cannot be undone.",
       function (button) {
         if (button === "Ok") {
           $$(group.button).addClass("validated").click();
         }
       });
      return false; // cancels the request
    }

    function afterUpdateContainerDeleteDistricts() {
      $("#tab-deletedistricts input.kalypto").kalypto({ toggleClass: "kalypto-checkbox" });
    }

    //
    // Setup County Supervisors
    //

    function initCountySupervisors() {

      $("#tab-setupcountysupervisors")
        .on("click", ".add-supervisor-button", onClickAddCountySupervisorDistrict);

      $("#tab-setupcountysupervisors")
        .on("click", ".bulk-add-supervisors-button",
          onClickBulkAddCountySupervisorDistricts);

      $("#tab-setupcountysupervisors")
        .on("change", "tr.new td.delete input[type=checkbox]", onCountySupervisorsChangeNewDelete);

      $('#bulk-add-supervisors-dialog').dialog({
        autoOpen: false,
        width: 675,
        title: "Bulk Add County Supervisor Districts",
        resizable: false,
        dialogClass: 'bulk-add-supervisors-dialog overlay-dialog',
        modal: true
      });

      $('#bulk-add-supervisors-dialog .supervisors-number-to-create').spinner({
        min: 1,
        max: 99
      });

      $("#bulk-add-supervisors-dialog").on("propertychange change click keyup input paste spinchange",
        ".selection", onBulkAddCountySupervisorSelectionChange);

      onBulkAddCountySupervisorSelectionChange();

      $(".bulk-add-supervisors-dialog .create-supervisors-button")
        .click(onClickCreateCountySupervisorDistricts);
    }

    function triggerCountySupervisorsChanged() {
      $("#county-supervisors-table").trigger("change");
    }

    function enableBulkAddCountySupervisorDistricts() {
      $(".bulk-add-supervisors-button").prop("disabled",
        $("#county-supervisors-table tbody tr").length > 0);
    }

    function onCountySupervisorsChangeNewDelete() {
      // for a new entry, a click in the delete check box deletes it immediately
      $(this).closest("tr").remove();
      enableBulkAddCountySupervisorDistricts();
      triggerCountySupervisorsChanged();
    }

    function addCountySupervisorDistrict(seq, name) {
      var code = seq === ""
        ? ""
        : $.queryString("county") + (seq + 100).toString().substr(1);
      $("#county-supervisors-table tbody").append('<tr class="new">' +
        '<td class="delete"><input type="checkbox"/></td>' +
        '<td class="exists"><input type="checkbox" disabled="disabled"/></td>' +
        '<td class="create"><input type="checkbox" checked="checked"/></td>' +
        '<td class="code"><input type="text" value="' + code + '"/></td>' +
        '<td class="in-shapefile"><input type="checkbox"/></td>' +
        '<td class="name"><input type="text" value="' + (name || "") + '"/></td>' +
        '</tr>');
      triggerCountySupervisorsChanged();
    }

    function onClickAddCountySupervisorDistrict() {
      var county = $.queryString("county");

      var seq = "";
      if ($.queryString("state").toUpperCase() != "DC") {
        // get the next available code in the county
        var maxCode = county + "00";
        $("#county-supervisors-table td.code input[type=text]").each(function () {
          var val = $(this).val();
          if (val > maxCode) maxCode = val;
        });

        if (maxCode.length !== 5) return;
        seq = parseInt(maxCode.substr(3, 2));
        if (seq >= 99) seq = "";
        seq++;
      }

      addCountySupervisorDistrict(seq);
      enableBulkAddCountySupervisorDistricts();
    }

    function onClickCreateCountySupervisorDistricts() {
      var $dialog = $("#bulk-add-supervisors-dialog");
      var type = $(".supervisor-type-dropdown", $dialog).val();
      var district = $(".supervisor-name-dropdown", $dialog).val();
      if (type == "other") type = $.trim($(".supervisor-other-type", $dialog).val());
      if (district == "other") district = $.trim($(".supervisor-other-name", $dialog).val());

      var message = "";
      var numberToCreate = parseInt($(".supervisors-number-to-create", $dialog).val(), 10);
      var includeAtLarge = $(".supervisors-include-at-large", $dialog).prop("checked");
      if (isNaN(numberToCreate) || numberToCreate > 99 || numberToCreate === 99 && includeAtLarge ||
        numberToCreate < 0)
        message = "Invalid Number to Create";
      else if (numberToCreate === 0 && !includeAtLarge)
        message = "No districts would be created";
      if (message) {
        $(".supervisors-number-to-create", $dialog).addClass("error");
        util.alert(message);
        return;
      }
      for (var d = 1; d <= numberToCreate; d++) {
        var name = type +
          (type && district ? " " : "") + district.replace(/#/g, d);
        addCountySupervisorDistrict(d, name);
      }
      if (includeAtLarge) {
        var atLargeName = type + (type ? " " : "") + "At Large";
        addCountySupervisorDistrict(99, atLargeName);
      }
      enableBulkAddCountySupervisorDistricts();
      $dialog.dialog("close");
    }

    function onClickBulkAddCountySupervisorDistricts() {
      $('#bulk-add-supervisors-dialog').dialog("open");
    }

    function onBulkAddCountySupervisorSelectionChange() {
      var $dialog = $("#bulk-add-supervisors-dialog");
      var type = $(".supervisor-type-dropdown", $dialog).val();
      var district = $(".supervisor-name-dropdown", $dialog).val();
      $(".supervisor-other-type", $dialog).prop("disabled", type !== "other");
      $(".supervisor-other-name", $dialog).prop("disabled", district != "other");
      if (type == "other") type = $.trim($(".supervisor-other-type", $dialog).val());
      if (district == "other") district = $.trim($(".supervisor-other-name", $dialog).val());

      var numberToCreate = parseInt($(".supervisors-number-to-create", $dialog).val(), 10);
      var includeAtLarge = $(".supervisors-include-at-large", $dialog).prop("checked");
      if (includeAtLarge && (isNaN(numberToCreate) || numberToCreate < 1))
        district = "At Large";
      else {
        if (isNaN(numberToCreate) || (numberToCreate < 1)) numberToCreate = 1;
        else if (numberToCreate > 99) numberToCreate = 99;
        district = district.replace(/#/g, numberToCreate);
      }
      if (type && district) type += " ";
      $(".sample", $dialog).val(type + district);
      $(".error", $dialog).removeClass("error");
    }

    //
    // Setup City Council
    //

    function initCityCouncil() {

      $("#tab-setupcitycouncil")
        .on("click", ".add-council-button", onClickAddCityCouncilDistrict);

      $("#tab-setupcitycouncil")
        .on("click", ".bulk-add-council-button",
          onClickBulkAddCityCouncilDistricts);

      $("#tab-setupcitycouncil")
        .on("change", "tr.new td.delete input[type=checkbox]", onCityCouncilChangeNewDelete);

      $('#bulk-add-council-dialog').dialog({
        autoOpen: false,
        width: 450,
        title: "Bulk Add City Council Districts",
        resizable: false,
        dialogClass: 'bulk-add-council-dialog overlay-dialog',
        modal: true
      });

      $('#bulk-add-council-dialog .council-number-to-create').spinner({
        min: 1,
        max: 99
      });

      $("#bulk-add-council-dialog").on("propertychange change click keyup input paste spinchange",
        ".selection", onBulkAddCityCouncilSelectionChange);

      $(".bulk-add-council-dialog .create-council-button")
        .click(onClickCreateCityCouncilDistricts);
    }

    function afterUpdateContainerSetupCityCouncil() {
      onBulkAddCityCouncilSelectionChange();
    }

    function triggerCityCouncilChanged() {
      $("#city-council-table").trigger("change");
    }

    function enableBulkAddCityCouncilDistricts() {
      $(".bulk-add-council-button").prop("disabled",
        $("#city-council-table tbody tr").length > 0);
    }

    function onCityCouncilChangeNewDelete() {
      // for a new entry, a click in the delete check box deletes it immediately
      $(this).closest("tr").remove();
      enableBulkAddCityCouncilDistricts();
      triggerCityCouncilChanged();
    }

    function addCityCouncilDistrict(seq, name, district) {
      var code = seq === ""
        ? ""
        : $(".city-council-prefix").val() + (seq + 1000).toString().substr(1);
        $("#city-council-table tbody").append('<tr class="new">' +
        '<td class="delete"><input type="checkbox"/></td>' +
        '<td class="exists"><input type="checkbox" disabled="disabled"/></td>' +
        '<td class="create"><input type="checkbox" checked="checked"/></td>' +
        '<td class="code"><input type="text" value="' + code + '"/></td>' +
        '<td class="in-shapefile"><input type="checkbox"/></td>' +
        '<td class="district"><input type="text" value="' + (district || "") + '"/></td>' +
        '<td class="name"><input type="text" value="' + (name || "") + '"/></td>' +
        '</tr>');
      triggerCityCouncilChanged();
    }

    function onClickAddCityCouncilDistrict() {
      var prefix = $(".city-council-prefix").val();
      var seq;
      // get the next available code in the city
      var maxCode = prefix + "000";
      $("#city-council-table td.code input[type=text]").each(function () {
        var val = $(this).val();
        if (val > maxCode) maxCode = val;
      });

      if (maxCode.length !== 5) return;
      seq = parseInt(maxCode.substr(2, 3));
      if (seq >= 999) seq = "";
      seq++;

      addCityCouncilDistrict(seq);
      enableBulkAddCityCouncilDistricts();
    }

    function onClickCreateCityCouncilDistricts() {
      var $dialog = $("#bulk-add-council-dialog");
      var place = $("#tab-setupcitycouncil .city-council-place-name").val();
      var district = $(".council-name-dropdown", $dialog).val();
      if (district == "other") district = $.trim($(".council-other-name", $dialog).val());

      var message = "";
      var numberToCreate = parseInt($(".council-number-to-create", $dialog).val(), 10);
      var includeAtLarge = $(".council-include-at-large", $dialog).prop("checked");
      if (isNaN(numberToCreate) || numberToCreate > 99 || numberToCreate === 99 && includeAtLarge ||
        numberToCreate < 0)
        message = "Invalid Number to Create";
      else if (numberToCreate === 0 && !includeAtLarge)
        message = "No districts would be created";
      if (message) {
        $(".council-number-to-create", $dialog).addClass("error");
        util.alert(message);
        return;
      }
      for (var d = 1; d <= numberToCreate; d++) {
        var name = place +
          (place && district ? " " : "") + district.replace(/#/g, d);
        addCityCouncilDistrict(d, name, d);
      }
      if (includeAtLarge) {
        var atLargeName = place + (place ? " " : "") + "At Large";
        addCityCouncilDistrict(999, atLargeName, "At Large");
      }
      enableBulkAddCityCouncilDistricts();
      $dialog.dialog("close");
    }

    function onClickBulkAddCityCouncilDistricts() {
      $('#bulk-add-council-dialog').dialog("open");
    }

    function onBulkAddCityCouncilSelectionChange() {
      var $dialog = $("#bulk-add-council-dialog");
      var place = $("#tab-setupcitycouncil .city-council-place-name").val();
      var district = $(".council-name-dropdown", $dialog).val();
      $(".council-other-name", $dialog).prop("disabled", district != "other");
      if (district == "other") district = $.trim($(".council-other-name", $dialog).val());

      var numberToCreate = parseInt($(".council-number-to-create", $dialog).val(), 10);
      var includeAtLarge = $(".council-include-at-large", $dialog).prop("checked");
      if (includeAtLarge && (isNaN(numberToCreate) || numberToCreate < 1))
        district = "At Large";
      else {
        if (isNaN(numberToCreate) || (numberToCreate < 1)) numberToCreate = 1;
        else if (numberToCreate > 99) numberToCreate = 99;
        district = district.replace(/#/g, numberToCreate);
      }
      if (place && district) place += " ";
      $(".sample", $dialog).val(place + district);
      $(".error", $dialog).removeClass("error");
    }

    //
    // Setup School District
    //

    function initSchoolDistrict() {

      $("#tab-setupschooldistrict")
        .on("click", ".add-subdistrict-button", onClickAddSchoolDistrictDistrict);

      $("#tab-setupschooldistrict")
        .on("click", ".bulk-add-subdistricts-button",
          onClickBulkAddSchoolDistrictDistricts);

      $("#tab-setupschooldistrict")
        .on("change", "tr.new td.delete input[type=checkbox]", onSchoolDistrictChangeNewDelete);

      $('#bulk-add-subdistrict-dialog').dialog({
        autoOpen: false,
        width: 450,
        title: "Bulk Add School Sub-Districts",
        resizable: false,
        dialogClass: 'bulk-add-subdistrict-dialog overlay-dialog',
        modal: true
      });

      $('#bulk-add-subdistrict-dialog .subdistrict-number-to-create').spinner({
        min: 1,
        max: 99
      });

      $("#bulk-add-subdistrict-dialog").on("propertychange change click keyup input paste spinchange",
        ".selection", onBulkAddSchoolDistrictSelectionChange);

      $(".bulk-add-subdistrict-dialog .create-subdistricts-button")
        .click(onClickCreateSchoolDistrictDistricts);
    }

    function afterUpdateContainerSetupSchoolDistrict() {
      onBulkAddSchoolDistrictSelectionChange();
    }

    function triggerSchoolDistrictChanged() {
      $("#school-district-table").trigger("change");
    }

    function enableBulkAddSchoolDistrictDistricts() {
      $(".bulk-add-subdistricts-button").prop("disabled",
        $("#school-district-table tbody tr").length > 0);
    }

    function onSchoolDistrictChangeNewDelete() {
      // for a new entry, a click in the delete check box deletes it immediately
      $(this).closest("tr").remove();
      enableBulkAddSchoolDistrictDistricts();
      triggerSchoolDistrictChanged();
    }

    function addSchoolDistrictDistrict(seq, name) {
      var code = seq === ""
        ? ""
        : $(".school-district-prefix").val() + (seq + 100).toString().substr(1);
      $("#school-district-table tbody").append('<tr class="new">' +
        '<td class="delete"><input type="checkbox"/></td>' +
        '<td class="exists"><input type="checkbox" disabled="disabled"/></td>' +
        '<td class="create"><input type="checkbox" checked="checked"/></td>' +
        '<td class="code"><input type="text" value="' + code + '"/></td>' +
        '<td class="in-shapefile"><input type="checkbox"/></td>' +
        '<td class="name"><input type="text" value="' + (name || "") + '"/></td>' +
        '</tr>');
      triggerSchoolDistrictChanged();
    }

    function onClickAddSchoolDistrictDistrict() {
      var prefix = $(".school-district-prefix").val();
      var seq;
      // get the next available code in the city
      var maxCode = prefix + "00";
      $("#school-district-table td.code input[type=text]").each(function () {
        var val = $(this).val();
        if (val > maxCode) maxCode = val;
      });

      if (maxCode.length !== 5) return;
      seq = parseInt(maxCode.substr(3, 2));
      if (seq >= 99) seq = "";
      seq++;

      addSchoolDistrictDistrict(seq);
      enableBulkAddSchoolDistrictDistricts();
    }

    function onClickCreateSchoolDistrictDistricts() {
      var $dialog = $("#bulk-add-subdistrict-dialog");
      var place = $("#tab-setupschooldistrict .school-district-name").val();
      var district = $(".subdistrict-name-dropdown", $dialog).val();
      if (district == "other") district = $.trim($(".subdistrict-other-name", $dialog).val());

      var message = "";
      var numberToCreate = parseInt($(".subdistrict-number-to-create", $dialog).val(), 10);
      var includeAtLarge = $(".subdistrict-include-at-large", $dialog).prop("checked");
      if (isNaN(numberToCreate) || numberToCreate > 99 || numberToCreate === 99 && includeAtLarge ||
        numberToCreate < 0)
        message = "Invalid Number to Create";
      else if (numberToCreate === 0 && !includeAtLarge)
        message = "No districts would be created";
      if (message) {
        $(".subdistrict-number-to-create", $dialog).addClass("error");
        util.alert(message);
        return;
      }
      for (var d = 1; d <= numberToCreate; d++) {
        var name = place +
          (place && district ? " " : "") + district.replace(/#/g, d);
        addSchoolDistrictDistrict(d, name);
      }
      if (includeAtLarge) {
        var atLargeName = place + (place ? " " : "") + "At Large";
        addSchoolDistrictDistrict(99, atLargeName);
      }
      enableBulkAddSchoolDistrictDistricts();
      $dialog.dialog("close");
    }

    function onClickBulkAddSchoolDistrictDistricts() {
      $('#bulk-add-subdistrict-dialog').dialog("open");
    }

    function onBulkAddSchoolDistrictSelectionChange() {
      var $dialog = $("#bulk-add-subdistrict-dialog");
      var place = $("#tab-setupschooldistrict .school-district-name").val();
      var district = $(".subdistrict-name-dropdown", $dialog).val();
      $(".subdistrict-other-name", $dialog).prop("disabled", district != "other");
      if (district == "other") district = $.trim($(".subdistrict-other-name", $dialog).val());

      var numberToCreate = parseInt($(".subdistrict-number-to-create", $dialog).val(), 10);
      var includeAtLarge = $(".subdistrict-include-at-large", $dialog).prop("checked");
      if (includeAtLarge && (isNaN(numberToCreate) || numberToCreate < 1))
        district = "At Large";
      else {
        if (isNaN(numberToCreate) || (numberToCreate < 1)) numberToCreate = 1;
        else if (numberToCreate > 99) numberToCreate = 99;
        district = district.replace(/#/g, numberToCreate);
      }
      if (place && district) place += " ";
      $(".sample", $dialog).val(place + district);
      $(".error", $dialog).removeClass("error");
    }

    //
    // Tiger Settings
    //

    var afterUpdateContainerTigerSettings = function () {
      var $entireCounty = $("#tab-tigersettings .tiger-settings-entire-county");
      $entireCounty
        .on("rc_checked", function () {
          $("#tab-tigersettings .tiger-selection").addClass("hidden");
        })
        .on("rc_unchecked", function () {
          $("#tab-tigersettings .tiger-selection").removeClass("hidden");
        })
        .trigger($entireCounty.hasClass("checked") ? "rc_checked" : "rc_unchecked");
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

        case "ctyj":
          url = "/admin/countyjurisdictionsreport.aspx?state=" + stateCode;
          service = "GetCountyJurisdictionsReport";
          break;

        case "locj":
          url = "/admin/localjurisdictionsreport.aspx?state=" + stateCode;
          service = "GetLocalJurisdictionsReport";
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
        activate: function () {
          // save the current tab index to restore after update
          $("#tab-masteronly .sub-tab-index")
            .val($("#master-only-tabs").tabs("option", "active"));
        }
      });
    };

    var onTabsBeforeActivateMasterOnlySubTab = function () {
    };

    // Misc
    //

    var initPage = function() {
      monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);
      monitor.registerCallback("initRequest", initRequest);
      monitor.init();

      util.safeBind($(".jurisdiction-change-button"), "click",
        navigateJurisdiction.changeJurisdictionButtonClicked);

      window.onbeforeunload = function() {
        var changedGroups = monitor.getChangedGroupNames(true);
        if (changedGroups.length > 0)
          return "There are entries on your form that have not been submitted";
      };

      $$('main-tabs')
        .safeBind("tabsbeforeactivate", onTabsBeforeActivate)
        .safeBind("tabsactivate", onTabsActivate);

      initCountySupervisors();
      initCityCouncil();
      initSchoolDistrict();

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
        case "tab-adddistricts":
        case "tab-bulkadddistricts":
        case "tab-deletedistricts":
        case "tab-includemulticountydistricts":
        case "tab-removemulticountydistricts":
        case "tab-setupcountysupervisors":
        case "tab-setupcitycouncil":
        case "tab-setupschooldistrict":
        case "tab-tigersettings":
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