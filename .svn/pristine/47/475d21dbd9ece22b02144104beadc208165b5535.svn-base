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

function getScriptName() {
  var url = window.location.pathname;
  var lastUri = url.substring(url.lastIndexOf('/') + 1);
  if (lastUri.indexOf('?') !== -1)
    return lastUri.substring(0, lastUri.indexOf('?'));
  else
    return lastUri;
}

if (typeof ($.globals) === "undefined")
  $.globals = {}; // create global namespace

function onOpenSampleBallotEmailDialog() {
  onOpenJqDialog();
  $(document).keypress(function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode === '13') {
      $("#sampleBallotEmailDialog .emailEnter").click();
    }
  });
}

function onCloseSampleBallotEmailDialog() {
  onCloseJqDialog();
  $(document).keypress(null);
}

function initSampleBallotEmailDialog() {
  $('#sampleBallotEmailDialog').dialog({
    autoOpen: false,
    width: 500,
    height: 230,
    resizable: false,
    // custom open and close to fix various problems
    open: onOpenSampleBallotEmailDialog,
    close: onCloseSampleBallotEmailDialog,
    modal: true
  });
}

function submitSampleBallotEmail() {
  var data = {};
  data.email = $.globals.sampleBallotEmail;
  data.siteId = getParameterByName('site');
  data.script = getScriptName();
  data.stateCode = getParameterByName('state');
  data.electionKey = getParameterByName('election');
  data.congressionalDistrict = getParameterByName('congress');
  data.stateSenateDistrict = getParameterByName('stateSenate');
  data.stateHouseDistrict = getParameterByName('stateHouse');
  data.county = getParameterByName('county');
  $.ajax({
    type: "POST",
    url: "/WebService.asmx/SubmitSampleBallotEmail",
    data: JSON.stringify(data),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: submitSampleBallotEmailSucceeded,
    error: submitSampleBallotEmailFailed
  });
}

function submitSampleBallotEmailSucceeded(result) {
  $.cookie('sampleBallotEmailEntered', 'true', { expires: 365 });
}

function submitSampleBallotEmailFailed(result) {
  // empty
}

var emailRegEx = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i;
function getAndSubmitEmail() {
  if (typeof ($.globals.sampleBallotEmail) === "undefined") {
    $("#sampleBallotEmailDialog .emailEnter").click(
      function () {
        $.globals.sampleBallotEmail = '';
        var elements = $("#sampleBallotEmailDialog .email");
        if (elements)
          $.globals.sampleBallotEmail = getLastValue(elements); ;
        $('#sampleBallotEmailDialog').dialog('close');
        if ($.globals.sampleBallotEmail.search(emailRegEx) >= 0)
          submitSampleBallotEmail();
      });
      $("#sampleBallotEmailDialog .emailNoThanks").click(
      function () {
        $.globals.sampleBallotEmail = '';
        var elements = $("#sampleBallotEmailDialog .email");
        if (elements)
          $.globals.sampleBallotEmail = getLastValue(elements);
        $('#sampleBallotEmailDialog').dialog('close');
      });
      openSampleBallotEmailDialog();
  }
}

function openSampleBallotEmailDialog() {
  $('#sampleBallotEmailDialog').dialog('open');
  $('#sampleBallotEmailDialog .email').focus();
}

// Initialize when ready
$(function () {
  var shownCount = parseInt($.cookie('sampleBallotEmailShown')) || 0;
  if ((typeof SampleBallotEmailDialogEnabled === "undefined" || SampleBallotEmailDialogEnabled) &&
   shownCount === 0 && $.cookie('sampleBallotEmailEntered') !== 'true') {
    initSampleBallotEmailDialog();
    getAndSubmitEmail();
  }
  $.cookie('sampleBallotEmailShown', (shownCount + 1) % 5); // every 5th time
});
