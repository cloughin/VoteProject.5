﻿var getQuery = function (field) {
  var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
  var string = reg.exec(window.location.href);
  return string ? string[1] : '';
};

document.write('<iframe id="voteusa" scrolling="no" src="http://vote-usa.org/Intro2.aspx');
document.write('?Id=' + getQuery('id'));
document.write('" style="width: 1px; min-width: 100%; border: none; overflow: hidden;"></iframe>');
document.write('<script type="text/javascript" src="http://vote-usa.org/js/iframeResizer.js"><' + '/script>');
document.write('<script>iFrameResize({ log: false, checkOrigin: false }, "#voteusa");window.addEventListener("message", function (e) {var data = e.data ? e.data : e.message;if (typeof data === "string"){ if (data.substr(0, 9) === "[compare]") location.href = comparepage + data.substr(9);}});<' + '/script>');