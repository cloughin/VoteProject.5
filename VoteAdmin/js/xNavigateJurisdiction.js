function getStateCode() {
  if ($('.security-class').val() == 'MASTER') {
    return $('.state-dropdown').val();
  } else {
    return $('.state-code').val();
  }
}

function getCountyCode() {
  if ($('.security-class').val() != 'COUNTY') {
    return $('.county-dropdown').val();
  } else {
    return $('.county-code').val();
  }
}

function getLocalCode() {
  return $('.local-dropdown').val();
}

function stateDropdownChanged() {
  var data = {};
  data.stateCode = getStateCode();
  if (data.stateCode) {
    $.ajax({
      type: "POST",
      url: "/Admin/WebService.asmx/GetCounties",
      data: JSON.stringify(data),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: getCountiesSucceeded,
      error: getCountiesFailed
    });
  } else {
    $('.county-name').removeClass('hidden');
    $('.county-dropdown').addClass('hidden');
    $('.local-name').removeClass('hidden');
    $('.local-dropdown').addClass('hidden');
  }
  $('.state-radio').prop('checked', true);
  $('.state-dropdown').blur();
}

function countyDropdownChanged() {
  var data = {};
  data.stateCode = getStateCode();
  data.countyCode = getCountyCode();
  $.ajax({
    type: "POST",
    url: "/Admin/WebService.asmx/GetLocals",
    data: JSON.stringify(data),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: getLocalsSucceeded,
    error: getLocalsFailed
  });
  $('.county-radio').prop('checked', true);
  $('.county-dropdown').blur();
}

function localDropdownChanged() {
  $('.local-radio').prop('checked', true);
  $('.local-dropdown').blur();
}

function goButtonClicked() {
  var stateCode = getStateCode();
  if (!stateCode) {
    alert("Please select a state");
    return;
  }
  var url = "/admin/" + $('.admin-page-name').val() + ".aspx?state="
    + stateCode;
  if (!$('.state-radio').is(':checked')) {
    var countyCode = getCountyCode();
    if (!countyCode) {
      alert("Please select a county");
      return;
    }
    url += "&county=" + countyCode;
  }
  if ($('.local-radio').is(':checked')) {
    var localCode = getLocalCode();
    if (!localCode) {
      alert("Please select a local district");
      return;
    }
    url += "&local=" + localCode;
  }
  document.location.href = url;
}

function cancelButtonClicked() {
  $('#navigateJurisdiction').dialog('close');
}

function changeJurisdictionButtonClicked() {
  $('#navigateJurisdiction').dialog('open');
}

function htmlEscape(str) {
  return String(str)
    .replace(/&/g, '&amp;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&#39;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;');
}

function populateDropdown($dropdown, listitems, firstitem, firstvalue) {
  var options = [];
  if (firstitem)
    options.push('<option value="' + htmlEscape(firstvalue) + '">' + htmlEscape(firstitem) + '</option>');
  $.each(listitems, function (index, item) {
    options.push('<option value="' + htmlEscape(item.Value) + '">' + htmlEscape(item.Text) + '</option>');
  });
  $dropdown.html(options.join(''));
}

function getCountiesSucceeded(result) {
  if (result.d.length) {
    populateDropdown($('.county-dropdown'), result.d, '<select county>', '');
    $('.county-name').addClass('hidden');
    $('.county-dropdown').removeClass('hidden');
    $('.county-radio').attr('disabled', false);
  } else {
    $('.county-name').removeClass('hidden');
    $('.county-dropdown').addClass('hidden');
    $('.county-radio').attr('disabled', true);
  }
  $('.local-name').removeClass('hidden').html('&nbsp;');
  $('.local-dropdown').addClass('hidden');
  $('.local-radio').attr('disabled', true);
}

function getCountiesFailed(result) {
}

function getLocalsSucceeded(result) {
  if (result.d.length) {
    populateDropdown($('.local-dropdown'), result.d, '<select local district>', '');
    $('.local-name').addClass('hidden');
    $('.local-dropdown').removeClass('hidden');
    $('.local-radio').attr('disabled', false);
  } else {
    $('.local-name').removeClass('hidden').html('no local districts available');
    $('.local-dropdown').addClass('hidden');
    $('.local-radio').attr('disabled', true);
  }
}

function getLocalsFailed(result) {
}

function initNavigateJurisdictionDialog() {
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
}

function onOpenNavigateJurisdictionDialog() {
  onOpenJqDialog.apply(this);
}

function onCloseNavigateJurisdictionDialog() {
  onCloseJqDialog.apply(this);
}

//
// initPage
//

$(function () {
  initNavigateJurisdictionDialog();
});
