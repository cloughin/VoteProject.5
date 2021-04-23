function findLineEnder(str) {
  // returns either '\r\n', '\n', '\r' or null (if none found)
  if (str.indexOf('\r\n') >= 0) return '\r\n';
  if (str.indexOf('\n') >= 0) return '\n';
  if (str.indexOf('\r') >= 0) return '\r';
  return null;
}

function adjustFormatting(str) {
  var lineEnder = findLineEnder(str);
  var lines;
  if (lineEnder) {
    // force comment onto new line
    str = str.replace(/<!--/g, lineEnder + '<!--');
    lines = str.split(lineEnder);
  }
  else // treat as single long line
    lines = [str];
  // trim and eliminate empty lines, from end so we can still index
  for (var n = lines.length - 1; n >= 0; n--) {
    lines[n] = $.trim(lines[n]);
    if (!lines[n]) lines.splice(n, 1);
  }
  return lines.join(lineEnder);
}

function setRadio(obj) {
  var id = obj.parentElement.id;
  id = id.replace("sample-button", "radio-style");
  $("#" + id).click();
  return false;
}

function modifyButtons() {
  $('.vote-usa-sample-ballot-button').click(function () { return setRadio(this); });
  $('.vote-usa-compare-candidates-button').click(function () { return setRadio(this); });
  $('.vote-usa-logo-button').click(function () { return setRadio(this); });
}

$(function () {
  // Attach events to radios
  $('.radios').click(function (e) {
    var id = this.id;
    var inx = id.substr(id.length - 1, 1);
    var codeElement = $('#sample-button-' + inx);
    $('#code-area').val(adjustFormatting(codeElement.html()));
  });
  // default to first button
  $('#radio-style-1').click();
  modifyButtons();
});
