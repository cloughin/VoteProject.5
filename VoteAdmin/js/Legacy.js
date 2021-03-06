function setRequestClasses() {
  $("input", $(".input-list-report")).addClass("updating-report");
  $("input", $(".input-list-ajax")).addClass("updating-ajax");
}

function initPage() {
  setRequestClasses();
}

function initializeRequest(sender) {
  var jqo = $(sender._activeElement);
  var msg;
  if (jqo.hasClass("updating-report"))
    msg = "Generating report...";
  else if (jqo.hasClass("updating-ajax"))
    msg = "Updating...";
  if (msg) {
    msg = '<div style="display:inline-block;height:12px;width:12px;background:url(/images/small-ajax-loader.gif) -2px -2px" /><span class="Msg">' + msg + '</span>';
    $(".label-msg").html(msg);
  }
}

function endRequest() {
  setRequestClasses();
}