var getQuery = function (field) {
  var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
  var string = reg.exec(window.location.href);
  return string ? string[1] : '';
};

document.write('<iframe id="voteusa" scrolling="no" src="http://vote-usa.org/Ballot2.aspx');
document.write('?Election=' + getQuery('election'));
document.write('&Congress=' + getQuery('congress'));
document.write('&StateSenate=' + getQuery('statesenate'));
if (getQuery('statehouse'))
  document.write('&StateHouse=' + getQuery('statehouse'));
document.write('&County=' + getQuery('county'));
document.write('&District=' + getQuery('district'));
document.write('&Place=' + getQuery('place'));
if (getQuery('esd'))
  document.write('&Esd=' + getQuery('esd'));
if (getQuery('ssd'))
  document.write('&Ssd=' + getQuery('ssd'));
if (getQuery('usd'))
  document.write('&Usd=' + getQuery('usd'));
if (getQuery('cc'))
  document.write('&Cc=' + getQuery('cc'));
if (getQuery('cs'))
  document.write('&Cs=' + getQuery('cs'));
if (getQuery('sdd'))
  document.write('&Sdd=' + getQuery('ssd'));
document.write('" style="width: 1px; min-width: 100%; border: none; overflow: hidden;"></iframe>');
document.write('<script type="text/javascript" src="http://vote-usa.org/js/iframeResizer.js"><' + '/script>');
document.write('<script>iFrameResize({ log: false, checkOrigin: false }, "#voteusa");window.addEventListener("message", function (e) {var data = e.data ? e.data : e.message;if (typeof data === "string"){ if (data.substr(0, 9) === "[compare]") location.href = comparepage + data.substr(9); else if (data.substr(0, 7) === "[intro]") location.href = intropage + data.substr(7); else if (data.substr(0, 12) === "[referendum]") location.href = referendumpage + data.substr(12);}});<' + '/script>');