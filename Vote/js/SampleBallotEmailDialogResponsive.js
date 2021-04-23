function initSampleBallotEmailDialog() {
  $('#sampleBallotEmailDialog').dialog({
    autoOpen: false,
    width: "auto",
    height: "auto",
    resizable: false,
    // custom open and close to fix various problems
    open: onOpenJqDialog,
    close: onCloseJqDialog(),
    create: function() {
      $(this).css("maxWidth", "500px");
    },
    modal: true,
    dialogClass: "sample-ballot-email-dialog"
  });
  $("#sampleBallotEmailDialog .emailEnter").click(
    function() {
      var email = $("#sampleBallotEmailDialog .email").val();
      if (submitSampleBallotEmail(email) !== false)
        $('#sampleBallotEmailDialog').dialog('close');

    });
  $("#sampleBallotEmailDialog .emailNoThanks").click(
    function() {
      $('#sampleBallotEmailDialog').dialog('close');
    });
  $("#sampleBallotEmailDialog .already-signed-up").click(
    function() {
      setEnteredCookie();
      $('#sampleBallotEmailDialog').dialog('close');
    });
  $(document).keypress(function(event) {
    var keycode = event.keyCode ? event.keyCode : event.which;
    if (keycode === 13) {
      var $email = $('#sampleBallotEmailDialog .email');
      if ($(".sample-ballot-email-dialog").css("display") != "none" && $email.is(":focus"))
        $("#sampleBallotEmailDialog .emailEnter").trigger("click");
    }
  });
}

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

function submitSampleBallotEmail(email) {
  email = $.trim(email);
  if (!UTIL.isValidEmail(email)) {
    UTIL.alert("Please enter a valid email address");
    return false;
  }
  var data = {};
  data.email = email;
  data.siteId = getParameterByName('site');
  data.stateCode = getParameterByName('state');
  data.electionKey = getParameterByName('election');
  data.congressionalDistrict = getParameterByName('congress');
  data.stateSenateDistrict = getParameterByName('stateSenate');
  data.stateHouseDistrict = getParameterByName('stateHouse');
  data.county = getParameterByName('county');
  data.district = getParameterByName('district');
  data.place = getParameterByName('place');
  data.elementary = getParameterByName('esd');
  data.secondary = getParameterByName('ssd');
  data.unified = getParameterByName('usd');
  data.cityCouncil = getParameterByName('cc');
  data.countySupervisors = getParameterByName('cs');
  data.schoolDistrictDistrict = getParameterByName('sdd');
  $.ajax({
    type: "POST",
    url: "/WebService.asmx/SubmitSampleBallotEmail",
    data: JSON.stringify(data),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: setEnteredCookie
  });
}

function setEnteredCookie() {
  $.cookie('sampleBallotEmailEntered', 'true', { expires: 365 });
}

function openSampleBallotEmailDialog() {
  if (PUBLIC.isIframed()) return;
  $('#sampleBallotEmailDialog').dialog('open');
  $('#sampleBallotEmailDialog .email').val("").focus();
}

var pendingSBDDialogTimer;

function showSampleBallotDialog() {
  if (pendingSBDDialogTimer) {
    clearTimeout(pendingSBDDialogTimer);
    pendingSBDDialogTimer = null;
  }
  var shownCount = parseInt($.cookie('sampleBallotEmailShown')) || 0;
  $.cookie('sampleBallotEmailShown', (shownCount + 1) % 5); // every 5th time
  openSampleBallotEmailDialog();
  pendingSBDDialogTimer = null;
}

// Initialize when ready
$(function() {
  initSampleBallotEmailDialog();
  var shownCount = parseInt($.cookie('sampleBallotEmailShown')) || 0;
  if ((typeof window.SampleBallotEmailDialogEnabled === "undefined" ||
      window.SampleBallotEmailDialogEnabled) &&
    shownCount === 0 &&
    $.cookie('sampleBallotEmailEntered') !== 'true') {
    pendingSBDDialogTimer = setTimeout(function() {
      showSampleBallotDialog();
    }, 15000);
  }
});