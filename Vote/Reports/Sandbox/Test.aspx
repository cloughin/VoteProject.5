<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Vote.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
</head>
<body>
  
   <div id="fb-root"></div>
   <script>     (function (d, s, id) {
       var js, fjs = d.getElementsByTagName(s)[0];
       if (d.getElementById(id)) return;
       js = d.createElement(s); js.id = id;
       js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.7";
       fjs.parentNode.insertBefore(js, fjs);
     } (document, 'script', 'facebook-jssdk'));</script>
    <form id="form1" runat="server">
    <div class="fb-share-button" data-href="http://vote-va.org/Ballot.aspx?State=VA&amp;Election=VA20161108GA&amp;Congress=010&amp;StateSenate=032&amp;StateHouse=067&amp;County=059" data-layout="button" data-size="small" data-mobile-iframe="true"><a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=http%3A%2F%2Fvote-va.org%2FBallot.aspx%3FState%3DVA%26Election%3DVA20161108GA%26Congress%3D010%26StateSenate%3D032%26StateHouse%3D067%26County%3D059&amp;src=sdkpreparse">Share</a></div>
    </form>
</body>
</html>
