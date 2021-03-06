// JavaScript for the Address Entry dialog
function getParameterByName(name) {
  name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
  var regexS = "[\\?&]" + name + "=([^&#]*)";
  var regex = new RegExp(regexS, "i");
  var results = regex.exec(window.location.search);
  if (results === null)
    return "";
  else
    return decodeURIComponent(results[1].replace(/\+/g, " "));
}

function openAddressEntryDialog(clickElement) {
  $('#addressEntryDialog').dialog('open');
  $('#addressEntryDialog .addressToFind').focus();

  if ($.browser.msie && clickElement) {
    var currentPosition = $('.ui-dialog').position().top;
    var proposedPosition = $(clickElement).position().top + $(clickElement).outerHeight();
    // only move it up, never down
    if (proposedPosition < currentPosition) {
      $('#addressEntryDialog').dialog('option', 'position', ['middle', proposedPosition - $(document).scrollTop()]);
    }
  }
}

function getSampleBallot() {
  var state = $.cookie('State');
  var congress = $.cookie('Congress');
  var stateSenate = $.cookie('StateSenate');
  var stateHouse = $.cookie('StateHouse');
  var county = $.cookie('County');

  if (state && congress && stateSenate && stateHouse && county) {
    if ($.globals.getUpcomingElectionsCalled) {
      if ($.globals.upcomingElections)
        handleUpcomingElections();
    }
    else {
      $.ajax({
        type: "POST",
        url: "/WebService.asmx/GetUpcomingElections",
        data: "{" +
          "'stateCode': '" + state +
          "','congress': '" + congress +
          "','stateSenate': '" + stateSenate +
          "','stateHouse': '" + stateHouse +
          "','county': '" + county +
          "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: getUpcomingElectionsSucceeded,
        error: getUpcomingElectionsFailed
      });
      $.globals.getUpcomingElectionsCalled = true;
    }
  }
  else
    openAddressEntryDialog();
}

function redirectToVoters() {
  var url = '/forVoters.aspx';
//  var siteId = getParameterByName('site');
//  if (siteId) url += '?Site=' + siteId;
  document.location.pathname = url;
}

function getSampleBallot2() {
  var state = $.cookie('State');
  var congress = $.cookie('Congress');
  var stateSenate = $.cookie('StateSenate');
  var stateHouse = $.cookie('StateHouse');
  var county = $.cookie('County');

  if (state && congress && stateSenate && stateHouse && county) {
    if (!$.globals.getUpcomingElectionsCalled) {
      $.ajax({
        type: "POST",
        url: "/WebService.asmx/GetUpcomingElections",
        data: "{" +
          "'stateCode': '" + state +
          "','congress': '" + congress +
          "','stateSenate': '" + stateSenate +
          "','stateHouse': '" + stateHouse +
          "','county': '" + county +
          "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: getUpcomingElectionsSucceeded2,
        error: getUpcomingElectionsFailed
      });
      $.globals.getUpcomingElectionsCalled = true;
    }
  }
  else
    redirectToVoters();
}

function getUpcomingElectionsSucceeded(result) {
  var upcomingElections = result.d;
  $.globals.upcomingElections = upcomingElections;
  var menu = $('#socialMediaButtons .sampleBallot .electionMenu');
  if (upcomingElections.length > 1) {
    for (var n = 0; n < upcomingElections.length; n++) {
      var upcomingElection = upcomingElections[n];
      var a = $('<a />',
    {
      href: upcomingElection.Href,
      title: upcomingElection.Description
    }).html(upcomingElection.Description);
      menu.append(a);
    }
  }
  else {
    menu.addClass("padded");
    menu.html("We do not currently have any upcoming <br /> elections for this state.");
  }
  handleUpcomingElections();
}

function getUpcomingElectionsSucceeded2(result) {
  var upcomingElections = result.d;
  if (upcomingElections.length === 1) {
    document.location.href = upcomingElections[0].Href;
  }
  else {
    redirectToVoters();
  }
}

function handleUpcomingElections() {
  var upcomingElections = $.globals.upcomingElections;
  if (upcomingElections) {
    if (upcomingElections.length !== 1) {
      var menu = $('#socialMediaButtons .sampleBallot .electionMenu');
      if (menu.is(":hidden"))
        menu.show('fast');
      else
        menu.hide('slow');
    }
    else {
      document.location.href = upcomingElections[0].Href;
    }
  }
}

function getUpcomingElectionsFailed(result) {
  alert(result.status + ' ' + result.statusText);
}

function initAddressEntryDialog() {
  $('#addressEntryDialog').dialog({
    autoOpen: false,
    width: 708,
    height: 367,
    resizable: false,
    // custom open and close to fix various problems
    open: onOpenAddressEntryDialog,
    close: onCloseJqDialog,
    modal: true
  });
  initAddressEntryAddressToFind();
  initAddressEntryFindAddressButton();
  initAddressEntryFindStateButton();
}

function onOpenAddressEntryDialog() {
  onOpenJqDialog.apply(this);
  $('#addressEntryDialog .ajaxMessage').hide();
}

function initAddressEntryAddressToFind() {
  // enter in this input box triggers button click
  $('#addressEntryDialog .addressToFind').keyup(
    function (evt) {
      $('#addressEntryDialog .ajaxMessage').hide();
      var keyCode = evt ? (evt.which ? evt.which : evt.keyCode) : event.keyCode;
      if (keyCode === 13)
        addressEntryFindAddress();
    });
}

function initAddressEntryFindAddressButton() {
  $("#addressEntryDialog .address .find").click(addressEntryFindAddress);
}

function initAddressEntryFindStateButton() {
  $("#addressEntryDialog .state .find").click(addressEntryFindState);
}

function setAddressEntryAjaxActive(newState) {
  var currentState = $.globals.ajaxState ? true : false;
  if (newState !== currentState) {
    $.globals.ajaxState = newState;
    var visibility = newState ? "visible" : "hidden";
    $('#addressEntryDialog img.ajaxLoader').css("visibility", visibility);
  }
  return currentState;
}

function getEnteredAddress() {
  var addressInput = $('#addressEntryDialog .addressToFind');
  var address = addressInput.val();
  // A workaround for wierd problem
  if (!address && addressInput.length > 1)
    address = $(addressInput[addressInput.length - 1]).val();
  return $.trim(address);
}

function addressEntryFindAddress() {
  if (getEnteredAddress())
    getEmailAndSubmit(submitAddress);
    //submitAddress();
}

function submitAddress() {
  var data = {};
  data.email = $.globals.addressEntryEmail || '';
  data.input = getEnteredAddress();
  data.siteId = getParameterByName('site');
  data.remember = true;
  if (data.input && !setAddressEntryAjaxActive(true)) {
    $('#addressEntryDialog .ajaxMessage').hide();
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/FindAddress",
      data: JSON.stringify(data),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: addressEntryFindAddressSucceeded,
      error: addressEntryFindAddressFailed
    });
  }
}

function getSelectedState() {
  var stateSelect = $('#addressEntryDialog .selectState');
  var state = stateSelect.val();
  // A workaround for wierd problem
  if (!state && stateSelect.length > 1)
    state = $(stateSelect[stateSelect.length - 1]).val();
  return state;
}

function addressEntryFindState() {
  if (getSelectedState())
    getEmailAndSubmit(submitState);
    //submitState();
}

function submitState() {
  var data = {};
  data.email = $.globals.addressEntryEmail || '';
  data.input = getSelectedState();
  data.siteId = getParameterByName('site');
  if (data.input && !setAddressEntryAjaxActive(true)) {
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/FindState",
      data: JSON.stringify(data),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: addressEntryFindStateSucceeded,
      error: addressEntryFindStateFailed
    });
  }
}

function showAddressEntryAjaxMessage(html, cssClass) {
  var o = $('#addressEntryDialog .ajaxMessage');
  o.removeClass('ajaxSuccess ajaxFailure');
  o.addClass(cssClass);
  o.html(html);
  o.show();
}

function showStateAjaxMessage(html, cssClass) {
  var o = $('#addressEntryDialog .ajaxMessage2');
  o.removeClass('ajaxSuccess ajaxFailure');
  o.addClass(cssClass);
  o.html(html);
  o.show();
}

function addressEntryFindAddressSucceeded(result) {
  setAddressEntryAjaxActive(false);
  var addressFinderResult = result.d;
  if (addressFinderResult.SuccessMessage) {
    {
      showAddressEntryAjaxMessage(addressFinderResult.SuccessMessage, "ajaxSuccess");
      if (addressFinderResult.RedirectUrl)
        document.location.href = addressFinderResult.RedirectUrl;
    }
  }
  else {
    var messages = addressFinderResult.ErrorMessages;
    showAddressEntryAjaxMessage(messages.join('<br />'), "ajaxFailure");
  }
}

function addressEntryFindAddressFailed(result) {
  setAddressEntryAjaxActive(false);
  alert(result.status + ' ' + result.statusText);
}

function addressEntryFindStateSucceeded(result) {
  setAddressEntryAjaxActive(false);
  var addressFinderResult = result.d;
  if (addressFinderResult.SuccessMessage) {
    showStateAjaxMessage(addressFinderResult.SuccessMessage, "ajaxSuccess");
    document.location.href = addressFinderResult.RedirectUrl;
  }
}

function addressEntryFindStateFailed(result) {
  setAddressEntryAjaxActive(false);
  alert(result.status + ' ' + result.statusText);
}

function isDefaultPage() {
  var path = location.pathname.toLowerCase();
  return path === "/" || path === "/default.aspx";
}

if (typeof ($.globals) === "undefined")
  $.globals = {}; // create global namespace

function initAddressEntryEmailDialog() {
  $('#addressEntryEmailDialog').dialog({
    autoOpen: false,
    width: 500,
    height: 230,
    resizable: false,
    // custom open and close to fix various problems
    open: onOpenJqDialog,
    close: onCloseJqDialog,
    modal: true
  });
}

function getEmailAndSubmit(submitFn) {
//  if (false && typeof ($.globals.addressEntryEmail) === "undefined") {
//    $("#addressEntryEmailDialog .emailEnter").click(
//      function () {
//        $.globals.addressEntryEmail = '';
//        var elements = $("#addressEntryEmailDialog .email");
//        if (elements)
//          $.globals.addressEntryEmail = getLastValue(elements);
//        $('#addressEntryEmailDialog').dialog('close');
//        submitFn();
//      });
//    $("#addressEntryEmailDialog .emailNoThanks").click(
//      function () {
//        $.globals.addressEntryEmail = '';
//        var elements = $("#addressEntryEmailDialog .email");
//        if (elements)
//          $.globals.addressEntryEmail = getLastValue(elements); ;
//        $('#addressEntryEmailDialog').dialog('close');
//        submitFn();
//      });
//    openAddressEntryEmailDialog();
//  }
//  else 
    submitFn();
}

function openAddressEntryEmailDialog() {
  $('#addressEntryEmailDialog').dialog('open');
  $('#addressEntryEmailDialog .email').focus();
}

// Initialize when ready
$(function () {
  initAddressEntryDialog();
  initAddressEntryEmailDialog();
});
