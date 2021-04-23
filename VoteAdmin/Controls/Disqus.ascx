<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Disqus.ascx.cs" Inherits="Vote.Controls.Disqus" %>

<div style="margin:10px">
<hr/>
<div class="disqus-check-spam">Be sure to check spam folder for validation email.</div>
<div id="disqus_thread"></div>
<script>
  var disqus_config = function () {
    this.page.url = '<%=CanonicalUrl %>';
    this.page.identifier = '<%=CacheKey %>'; 
  };
  (function () {  
    var d = document, s = d.createElement('script');
    s.src = '<%=EmbedFilename %>';
    s.setAttribute('data-timestamp', +new Date());
    (d.head || d.body).appendChild(s);
  })();
</script>
<noscript>Please enable JavaScript to view the <a href="https://disqus.com/?ref_noscript" rel="nofollow">comments powered by Disqus.</a></noscript>
<hr/>
</div>