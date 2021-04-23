function initDonateLinks() {
  $(".donateLink").click(
      function (event) {
        var scriptName = document.location.pathname.toLowerCase();
        if (scriptName === "/intro.aspx" || scriptName === "/politicianissue.aspx") {
          openDonateInfoDialog();
          event.preventDefault();
        }
      }
    );
}

function openDonateInfoDialog() {
  $('#donateInfoDialog').dialog('open');
}

function initDonateInfoDialog() {
  $('#donateInfoDialog').dialog({
    autoOpen: false,
    width: 500,
    height: 230,
    resizable: false,
    // custom open and close to fix various problems
    open: onOpenJqDialog,
    close: onCloseJqDialog,
    modal: true
  });
  // attach click events
  $("#donateInfoDialog .proceed").click(
      function (event) {
        var originalLink = $('#mainNavigation .donateLink');
        window.open(originalLink.attr('href'), originalLink.attr('target'), ' ');
        $('#donateInfoDialog').dialog('close');
      });
      $("#donateInfoDialog .cancel").click(
      function () {
        $('#donateInfoDialog').dialog('close');
      });
}

$(function () {
  initDonateLinks();
  initDonateInfoDialog();
});