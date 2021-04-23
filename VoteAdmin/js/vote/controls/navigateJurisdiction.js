define(["jquery", "vote/adminMaster", "vote/util", "jqueryui"], 
  function ($, master, util) {

    var lastDistrictSearchString;

  var cancelButtonClicked = function () {
    $('#navigateJurisdiction').dialog('close');
  };

  var changeJurisdictionButtonClicked = function () {
    initNavigateJurisdictionDialog();
    $('#navigateJurisdiction').dialog('open');
  };

  var countyDropdownChanged = function () {
    util.ajax({
      url: "/Admin/WebService.asmx/GetLocals",
      data: {
        stateCode: getStateCode(),
        countyCode: getCountyCode()
      },
      success: getLocalsSucceeded,
      error: getLocalsFailed
    });
    $('#navigateJurisdiction .county-radio').prop('checked', true);
    $('#navigateJurisdiction .county-dropdown').blur();
  };

  function enableSearchAllDistricts()
  {
    $('#navigateJurisdiction .search-all-districts-button')
      .toggleClass("disabled", !getStateCode());
  }

  var localDropdownChanged = function () {
    $('#navigateJurisdiction .local-radio').prop('checked', true);
    $('#navigateJurisdiction .local-dropdown').blur();
  };

  var getCountiesFailed = function () {
  };

  var getCountiesSucceeded = function (result) {
    if (result.d.length) {
      util.populateDropdown($('#navigateJurisdiction .county-dropdown'), result.d, '<select county>', '');
      $('#navigateJurisdiction .county-name').addClass('hidden');
      $('#navigateJurisdiction .county-dropdown').removeClass('hidden');
      $('#navigateJurisdiction .county-radio').attr('disabled', false);
    } else {
      $('#navigateJurisdiction .county-name').removeClass('hidden');
      $('#navigateJurisdiction .county-dropdown').addClass('hidden');
      $('#navigateJurisdiction .county-radio').attr('disabled', true);
    }
    $('#navigateJurisdiction .local-name').removeClass('hidden').html('&nbsp;');
    $('#navigateJurisdiction .local-dropdown').addClass('hidden');
    $('#navigateJurisdiction .local-radio').attr('disabled', true);
  };

  var getCountyCode = function () {
    if ($('#navigateJurisdiction .security-class').val() !== 'COUNTY') {
      return $('#navigateJurisdiction .county-dropdown').val();
    } else {
      return $('#navigateJurisdiction .county-code').val();
    }
  };

  var getLocalKey = function () {
    return $('#navigateJurisdiction .local-dropdown').val();
  };

  var getLocalsFailed = function () {
  };

  var getLocalsSucceeded = function (result) {
    if (result.d.length) {
      util.populateDropdown($('#navigateJurisdiction .local-dropdown'), result.d, '<select local district>', '');
      $('#navigateJurisdiction .local-name').addClass('hidden');
      $('#navigateJurisdiction .local-dropdown').removeClass('hidden');
      $('#navigateJurisdiction .local-radio').attr('disabled', false);
    } else {
      $('#navigateJurisdiction .local-name').removeClass('hidden').html('no local districts available');
      $('#navigateJurisdiction .local-dropdown').addClass('hidden');
      $('#navigateJurisdiction .local-radio').attr('disabled', true);
    }
  };

  var getStateCode = function() {
    if ($('#navigateJurisdiction .security-class').val() === 'MASTER') {
      return $('#navigateJurisdiction .state-dropdown').val();
    } else {
      return $('#navigateJurisdiction .state-code').val();
    }
  };

  var goButtonClicked = function () {
    var stateCode = getStateCode();
    if (!stateCode) {
      util.alert("Please select a state");
      return;
    }
    var url = "/admin/" + $('.admin-page-name').val() + ".aspx?state="
      + stateCode;
    if (!$('#navigateJurisdiction .state-radio').is(':checked')) {
      var countyCode = getCountyCode();
      if (!countyCode) {
        util.alert("Please select a county");
        return;
      }
      url += "&county=" + countyCode;
    }
    if ($('#navigateJurisdiction .local-radio').is(':checked')) {
      var localKey = getLocalKey();
      if (!localKey) {
        util.alert("Please select a local district");
        return;
      }
      url += "&local=" + localKey;
    }
    document.location.href = url;
  };

  var initNavigateJurisdictionDialog = function () {
    // if we have an uninitialized instance, it is the newest. Remove
    // any initialized instances and proceed. Otherwise we are good to go.
    // This is necessary because an opened dialog is moved from the context
    // of its UpdatePanel.
    if ($('#navigateJurisdiction:not(.initialized)').length === 0) return;
    $('#navigateJurisdiction.initialized').remove();
    $('#navigateJurisdiction').addClass("initialized").dialog({
      autoOpen: false,
      width: 708,
      height: "auto",
      resizable: false,
      title: 'Change Jurisdiction',
      dialogClass: 'navigate-jurisdiction',
      // custom open and close to fix various problems
      open: onOpenNavigateJurisdictionDialog,
      close: onCloseNavigateJurisdictionDialog,
      modal: true
    });
    $('#searchDistricts').dialog({
      autoOpen: false,
      width: "auto",
      resizable: false,
      title: "Search Districts in State",
      dialogClass: "search-districts-in-state",
      modal: true
    });
    $("#navigateJurisdiction .jurisdiction-cancel-button").on("click", 
      cancelButtonClicked);
    $("#navigateJurisdiction .state-dropdown").on("change",
      stateDropdownChanged);
    $("#navigateJurisdiction .county-dropdown").on("change",
      countyDropdownChanged);
    $("#navigateJurisdiction .local-dropdown").on("change",
      localDropdownChanged);
    $("#navigateJurisdiction .jurisdiction-go-button").on("click",
      goButtonClicked);
    $("#navigateJurisdiction .search-all-districts-button").on("click",
      searchAllDistrictsButtonClicked);
    $("#searchDistricts").on("dblclick", ".search-districts-select p",
      searchAllDistrictsDistrictClicked);
    $('#searchDistricts .search-districts-text')
      .on("propertychange change click keyup input paste", searchDistrictsTextChanged);
    enableSearchAllDistricts();
  };

  var onCloseNavigateJurisdictionDialog = function () {
    master.onCloseJqDialog.apply(this);
  };

  var onOpenNavigateJurisdictionDialog = function () {
    master.onOpenJqDialog.apply(this);
  };

  function searchAllDistrictsButtonClicked() {
    $('#searchDistricts .search-districts-text').val("");
    $('#searchDistricts .search-districts-select').html("");
    lastDistrictSearchString = "";
    $('#searchDistricts').dialog("open");
  }

    function searchDistrictsTextChanged() {
      var text = $.trim($('#searchDistricts .search-districts-text').val());
      var $searchDistrictsSelect = $('#searchDistricts .search-districts-select');
      if (text.length >= 2) {
        // only act on first event triggered
        if (text != lastDistrictSearchString) {
          var data = {
            stateCode: getStateCode(),
            searchString: text,
            includeCounties: false
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

  function searchAllDistrictsDistrictClicked() {
    var url = "/admin/" + $('.admin-page-name').val() + ".aspx?state="
      + getStateCode() + "&local=" + $(this).data("key");
    document.location.href = url;
  }

  var stateDropdownChanged = function () {
    var data = { stateCode: getStateCode() };
    if (data.stateCode) {
      util.ajax({
        url: "/Admin/WebService.asmx/GetCounties",
        data: data,
        success: getCountiesSucceeded,
        error: getCountiesFailed
      });
    } else {
      $('#navigateJurisdiction .county-name').removeClass('hidden');
      $('#navigateJurisdiction .county-dropdown').addClass('hidden');
      $('#navigateJurisdiction .local-name').removeClass('hidden');
      $('#navigateJurisdiction .local-dropdown').addClass('hidden');
    }
    $('#navigateJurisdiction .state-radio').prop('checked', true);
    $('#navigateJurisdiction .state-dropdown').blur();
    enableSearchAllDistricts();
  };
  
  $(function () {
    //initNavigateJurisdictionDialog();
  });

  return {
    changeJurisdictionButtonClicked: changeJurisdictionButtonClicked
  };
});