define(["jquery", "vote/adminMaster", "vote/util", "jqueryui"], 
function ($, master, util) {

  var cancelButtonClicked = function () {
    $('#navigateJurisdiction').dialog('close');
  };

  var changeJurisdictionButtonClicked = function () {
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

  var localDropdownChanged = function () {
    $('#navigateJurisdiction .local-radio').prop('checked', true);
    $('#navigateJurisdiction .local-dropdown').blur();
  };

  var getCountiesFailed = function (result) {
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

  var getLocalCode = function () {
    return $('#navigateJurisdiction .local-dropdown').val();
  };

  var getLocalsFailed = function (result) {
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
      var localCode = getLocalCode();
      if (!localCode) {
        util.alert("Please select a local district");
        return;
      }
      url += "&local=" + localCode;
    }
    document.location.href = url;
  };

  var initNavigateJurisdictionDialog = function () {
    $('#navigateJurisdiction').dialog({
      autoOpen: false,
      width: 708,
      height: 180,
      resizable: false,
      title: 'Change Jurisdiction',
      dialogClass: 'navigate-jurisdiction',
      // custom open and close to fix various problems
      open: onOpenNavigateJurisdictionDialog,
      close: onCloseNavigateJurisdictionDialog,
      modal: true
    });
    $("#navigateJurisdiction .jurisdiction-cancel-button").safeBind("click", 
      cancelButtonClicked);
    $("#navigateJurisdiction .state-dropdown").safeBind("change",
      stateDropdownChanged);
    $("#navigateJurisdiction .county-dropdown").safeBind("change",
      countyDropdownChanged);
    $("#navigateJurisdiction .local-dropdown").safeBind("change",
      localDropdownChanged);
    $("#navigateJurisdiction .jurisdiction-go-button").safeBind("click",
      goButtonClicked);
  };

  var onCloseNavigateJurisdictionDialog = function () {
    master.onCloseJqDialog.apply(this);
  };

  var onOpenNavigateJurisdictionDialog = function () {
    master.onOpenJqDialog.apply(this);
  };

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
  };
  
  $(function () {
    initNavigateJurisdictionDialog();
  });

  return {
    changeJurisdictionButtonClicked: changeJurisdictionButtonClicked
  };
});