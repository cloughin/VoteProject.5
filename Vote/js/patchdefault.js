document.write('<iframe id="voteusa" scrolling="no" src="http://vote-usa.org/default2.aspx" style="width: 1px; min-width: 100%; border: none; overflow: hidden;"></iframe>');
document.write('<script type="text/javascript" src="http://vote-usa.org/js/iframeResizer.js"><' + '/script>');
document.write('<script>iFrameResize({ log: false, checkOrigin: false }, "#voteusa");window.addEventListener("message", function (e) {var data = e.data ? e.data : e.message;if (typeof data === "string" && data.substr(0, 8) === "[ballot]")location.href = ballotpage + data.substr(8);});<' + '/script>');
