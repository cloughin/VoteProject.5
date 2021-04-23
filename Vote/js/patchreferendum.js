var getQuery = function (field) {
  var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
  var string = reg.exec(window.location.href);
  return string ? string[1] : '';
};

document.write('<iframe id="voteusa" scrolling="no" src="http://vote-usa.org/Referendum2.aspx');
document.write('?Election=' + getQuery('election'));
document.write('&Referendum=' + getQuery('referendum'));
document.write('" style="width: 1px; min-width: 100%; border: none; overflow: hidden;"></iframe>');
document.write('<script type="text/javascript" src="http://vote-usa.org/js/iframeResizer.js"><' + '/script>');
document.write('<script>iFrameResize({ log: false, checkOrigin: false }, "#voteusa");<' + '/script>');