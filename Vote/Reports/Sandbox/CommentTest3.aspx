<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommentTest3.aspx.cs" Inherits="Vote.Sandbox.CommentTest3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <h1>Test of Commenting #3</h1>
      
<div id="disqus_thread"></div>
<script>
  /**
  *  RECOMMENDED CONFIGURATION VARIABLES: EDIT AND UNCOMMENT THE SECTION BELOW TO INSERT DYNAMIC VALUES FROM YOUR PLATFORM OR CMS.
  *  LEARN WHY DEFINING THESE VARIABLES IS IMPORTANT: https://disqus.com/admin/universalcode/#configuration-variables
  */

  var disqus_config = function () {
    this.page.url = "http://vote-usa.org/sandbox/commenttest3.aspx";  // Replace PAGE_URL with your page's canonical URL variable
    this.page.identifier = "/sandbox/commenttest3.aspx"; // Replace PAGE_IDENTIFIER with your page's unique identifier variable
  };

  (function () {  // DON'T EDIT BELOW THIS LINE
    var d = document, s = d.createElement('script');

    s.src = '//voteusadev.disqus.com/embed.js';

    s.setAttribute('data-timestamp', +new Date());
    (d.head || d.body).appendChild(s);
  })();
</script>
<noscript>Please enable JavaScript to view the <a href="https://disqus.com/?ref_noscript" rel="nofollow">comments powered by Disqus.</a></noscript>      

    
    </div>
    <script id="dsq-count-scr" src="//voteusadev.disqus.com/count.js" async></script>
    </form>
</body>
</html>
