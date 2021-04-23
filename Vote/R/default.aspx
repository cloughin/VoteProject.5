<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Vote.R._default" %>

<!DOCTYPE html>

<html>
<head runat="server">
  <meta name="viewport" content="width=device-width,initial-scale=1">
  <title>State Elections, Ballots, Candidate Biographical and Issue Comparisons - VoteUSA.org</title>
  <style>
    #donation-request
    {
      position: fixed;
      bottom: 0;
      left: 0;
      right: 0;
      background-color: #888;
    }
    #donation-request,
    #donation-request-spacer
    {
      height: 80px;
    }
    #intro
    {
      float: right;
    }
  </style>
</head>
<body>
  <form id="form1" runat="server">
    <div>
      <img src="/images/designs/Vote-USA/lgbanner.png" 
        id="logo" alt="Vote USA - Connecting voters and candidates">
      <div id="intro">
        <p>Becoming an Informed Voter</p>
        <p>... just got easier with our</p>
        <ul>
          <li>Customized sample ballots</li>
          <li>Pictures, videos, social media links</li>
          <li>Bios and position statements</li>
          <li>Side-by-side candidate comparisons</li>
        </ul>
        <p>All information is provided by the candidates or is from their web sites. Vote-USA has no political agenda.</p>
      </div>
      <div id="sample-ballot-form">
        <p>Get your enhanced sample ballot</p>
        <p>Address or nine-digit zip code</p>
        <p><input type="text" /></p>
      </div>
      <div id="mission">
        <p>Our Mission</p>
        <p>Our democracy is so precious; we feel there must be a better way to make it work. Rather than electing our representatives based on money, deceptive advertising, and political posters, we feel that the Internet and social media can provide a truly vibrant and thriving representative democratic environment. Ancient Greece was the crucible of Western civilization and democracy. In an arena, citizens would gather to debate and decide public policy. Clearly, that model would not work for America; however, the model we present here could work. So please help us make this work with a 100% tax-deductible donation.</p>
        <p>&ldquo;&hellip;provide a truly vibrant and thriving representative democratic environment.&rdquo;</p>
      </div>
      <div id="footer">
        <div id="one-stop">
          <div id="disclaimer">
            
          </div>
          <div id="footer-links">
            
          </div>
        </div>
        <div id="sponsors">
          
        </div>
      </div>
      <div id="donation-request-spacer"></div>
      <div id="donation-request">Donation Request</div>
    </div>
  </form>
</body>
</html>
