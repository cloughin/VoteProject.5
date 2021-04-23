function initDonationRequestDialog() {
  $('#donationRequestDialog').dialog({
    autoOpen: false,
    width: 500,
    height: 230,
    resizable: false,
    // custom open and close to fix various problems
    open: onOpenJqDialog,
    close: onCloseJqDialog,
    modal: true
  });
  $('#donationRequestDialog .yes a').click(onClickYes);
  $('#donationRequestDialog .no a').click(onClickNo);
  $('#donationRequestDialog .already a').click(onClickAlready);
}

function disableCookie() {
  $.cookie('dnx', '-1', { expires: 365 });
}

function onClickYes() {
  disableCookie();
  $('#donationRequestDialog').dialog('close');
  return true;
}

function onClickNo() {
  $('#donationRequestDialog').dialog('close');
}

function onClickAlready() {
  disableCookie();
  $('#donationRequestDialog').dialog('close');
}

function openDonationRequestDialog() {
  $('#donationRequestDialog').dialog('open');
}

function getDonationNag() {
  var dnx = parseInt($.cookie('dnx'));
  if (isNaN(dnx)) dnx = 1;
  if (dnx >= 0) {
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/GetDonationNag",
      data: "{" +
          "'cookieIndex': " + dnx + "}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: getDonationNagSucceeded
    });
  }
}

function getDonationNagSucceeded(result) {
  var donationNag = result.d;
  $.cookie('dnx', donationNag.NextMessageNumber, { expires: 365 });
  if (donationNag.MessageText) {
    $('#donationRequestDialog .messageText').html(donationNag.MessageText);
    if (donationNag.MessageHeading)
      $('#donationRequestDialog .headingText').html(donationNag.MessageHeading);
    initDonationRequestDialog();
    setTimeout(openDonationRequestDialog, 2000);
  }
}

// Initialize when ready
$(function () {
  getDonationNag(); // via ajax
  //if ($('#donationRequestDialog .messageText').html()) {
  //  initDonationRequestDialog();
  //  openDonationRequestDialog();
  //}
});
