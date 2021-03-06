<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VoteTest.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>State Elections, Ballots, Candidate Biographical and Issue Comparisons - VoteUSA.org</title>
    <style type="text/css">
      /* BEGIN Eric Meyer reset */
      html, body, div, span, applet, object, iframe,
      h1, h2, h3, h4, h5, h6, p, blockquote, pre,
      a, abbr, acronym, address, big, cite, code,
      del, dfn, em, font, img, ins, kbd, q, s, samp,
      small, strike, strong, sub, sup, tt, var,
      dl, dt, dd, ol, ul, li,
      fieldset, form, label, legend,
      table, caption, tbody, tfoot, thead, tr, th, td {
	      margin: 0;
	      padding: 0;
	      border: 0;
	      outline: 0;
	      font-weight: inherit;
	      font-style: inherit;
	      font-size: 100%;
	      font-family: inherit;
	      vertical-align: baseline;
      }
      /* remember to define focus styles! */
      :focus {
	      outline: 0;
      }
      body {
	      line-height: 1;
	      color: black;
	      background: white;
      }
      ol, ul {
	      list-style: none;
      }
      /* tables still need 'cellspacing="0"' in the markup */
      table {
	      border-collapse: separate;
	      border-spacing: 0;
      }
      caption, th, td {
	      text-align: left;
	      font-weight: normal;
      }
      blockquote:before, blockquote:after,
      q:before, q:after {
	      content: "";
      }
      blockquote, q {
	      quotes: "" "";
      }
/* END Eric Meyer reset */

      body
      {
        margin: 0;
        padding: 0;
        border-top: 5px solid #85a6d1;
        text-align: center;
      }
      img
      {
        border: 0;
      }
      #outer
      {
        width: 100%;
        background: #cbdbdb url(/images/homebg.gif) repeat-x;
      }
      #container
      {
        margin: 0 auto;
        width: 940px;
        text-align: left;
      }
      #socialContainer
      {
        width: 100%;
        overflow: auto; /* to clear floats */
      }
      .addthis_toolbox
      {
        float: right;
        margin: 6px 2px 6px 0;
      }
      #logo
      {
        padding: 42px 0 32px 0;
      }
      #mainNavigation
      {
        width: 100%;
        overflow: auto; /* to clear floats */
      }
      ul.cssmenu
      {
        list-style: none;
        padding: 0px;
        margin: 0px;
      }
      ul.cssmenu li
      {
        float: left;
      }
      ul.cssmenu li a
      {
        display: block;
        height: 32px;
        background: url(/images/homemenubar.png);
        overflow: hidden;
	      text-indent: -10000px;
	      font-size: 0px;
	      line-height: 0px;
      }
      ul.cssmenu li.home a
      {
        width: 68px;
        background-position: 0px 0px;
      }
      ul.cssmenu li.home a:hover
      {
        background-position: 0px -32px;
      }
      ul.cssmenu li.forVoters a
      {
        width: 91px;
        background-position: -68px 0px;
      }
      ul.cssmenu li.forVoters a:hover
      {
        background-position: -68px -32px;
      }
      ul.cssmenu li.forCandidates a
      {
        width: 115px;
        background-position: -159px 0px;
      }
      ul.cssmenu li.forCandidates a:hover
      {
        background-position: -159px -32px;
      }
      ul.cssmenu li.forStateElectionAuthorities a
      {
        width: 193px;
        background-position: -274px 0px;
      }
      ul.cssmenu li.forStateElectionAuthorities a:hover
      {
        background-position: -274px -32px;
      }
      ul.cssmenu li.forVolunteers a
      {
        width: 114px;
        background-position: -467px 0px;
      }
      ul.cssmenu li.forVolunteers a:hover
      {
        background-position: -467px -32px;
      }
      ul.cssmenu li.forPartners a
      {
        width: 97px;
        background-position: -581px 0px;
      }
      ul.cssmenu li.forPartners a:hover
      {
        background-position: -581px -32px;
      }
      ul.cssmenu li.forEducationAndResearch a
      {
        width: 182px;
        background-position: -678px 0px;
      }
      ul.cssmenu li.forEducationAndResearch a:hover
      {
        background-position: -678px -32px;
      }
      ul.cssmenu li.donate a
      {
        width: 80px;
        background-position: -860px 0px;
      }
      ul.cssmenu li.donate a:hover
      {
        background-position: -860px -32px;
      }
      #mainContent
      {
        background: url(/images/flagbg.jpg) no-repeat top right;
      }
      #informedBlock
      {
        width: 100%;
        overflow: auto; /* to clear floats */
      }
      #peopleVoting
      {
        float:left;
      }
      #becomingInformed
      {
        float:left;
        padding-left:21px;
      }
      #becomingInformedTitle
      {
        padding: 21px 0 0 2px;
      }
      #becomingInformedText
      {
        padding: 22px 0 0 0;
      }
      #becomingInformedText div
      {
        width: 358px;
        color: #ffffff;
        font-family: Georgia;
        font-size: 14px;
        line-height: 18px;
      }
      #findState
      {
        padding: 20px 0 0 129px;
      }
      a.findStateButton
      {
        display: block;
        height: 40px;
        width: 230px;
        background: url(/images/findyourstate.png);
        overflow: hidden;
	      text-indent: -10000px;
	      font-size: 0px;
	      line-height: 0px;
      }
      a:hover.findStateButton
      {
        background-position: 0 -40px;
      }
      #missionBlock
      {
        width: 100%;
        overflow: auto; /* to clear floats */
        background: #ffffff;
      }
      #ourMission
      {
        float: left;
        padding-top: 12px;
        padding-left: 25px;
      }
      #ourMission div
      {
        width: 135px;
        font-family: Georgia;
        font-size: 18px;
        font-weight: bold;
        font-style: italic;
        color: #a1000e;
      }
      #missionStatement
      {
        float: left;
        margin-right: 10px;
        padding: 14px 0 0 0;
      }
      #missionStatement div
      {
        width: 550px;
        font-family: Georgia;
        font-size: 12px;
        color: #333333;
        line-height: 19px;
      }
      #missionPullQuote
      {
        float: left;
        padding: 13px 35px 0 0;
        text-align: right;
        background: url(/images/pullbg.png) no-repeat top left;
      }
      #missionPullQuote div
      {
        width: 185px;
        font-family: Georgia;
        font-size: 18px;
        font-style: italic;
        color: #cccccc;
        line-height: 22px;
      }
      #footerBlock
      {
        width: 100%;
        overflow: auto; /* to clear floats */
        background: #ffffff;
        padding-top: 29px;
        padding-bottom: 17px;
        font-family: Arial;
        font-size: 8pt;
      }
      #oneStop
      {
        width: 100%;
        text-align: center;
      }
      #footerBlock a
      {
        text-decoration: none;
        color: #666666;
      }
      #footerBlock a:visited
      {
      }
      #footerBlock a:hover
      {
        color: #000000;
      }
      #footerBlock a:active
      {
      }
      .footerLinks
      {
        float: left;
        padding-left: 21px;
      }
      .footerLinks ul
      {
        width: 159px;
        padding-top: 23px;
      }
      .footerLinks li
      {
        width: 159px;
        padding-top: 2px;
      }
      #footerBlurb
      {
        float: left;
        padding-left: 20px;
        padding-top: 20px;
      }
      #footerBlurb div
      {
        width: 530px;
        color: #999999;
        line-height: 18px;
      }
   </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="outer">
    <div id="container">
      <div id="socialContainer">
      <!-- AddThis Button BEGIN -->
        <div class="addthis_toolbox addthis_default_style ">
        <a class="addthis_button_facebook_like"></a>
        <a class="addthis_button_tweet"></a>
        <a class="addthis_counter addthis_pill_style"></a>
        </div>
        <script type="text/javascript">  var addthis_config = { "data_track_clickback": true };</script>
        <script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#pubid=ra-4d921e57282bf75e"></script>
        <!-- AddThis Button END -->    
      </div>
      <div id="logo">
        <img src="/images/homelogo.png" alt="Vote USA - Connecting voters and candidates" />
      </div>
      <div id="mainNavigation">
        <ul class="cssmenu">
          <li class="home"><a href="#" title="Home">Home</a></li>
          <li class="forVoters"><a href="#" title="for Voters">for Voters</a></li>
          <li class="forCandidates"><a href="#" title="for Candidates">for Candidates</a></li>
          <li class="forStateElectionAuthorities"><a href="#" title="for State Election Authorities">for State Election Authorities</a></li>
          <li class="forVolunteers"><a href="#" title="for Volunteers">for Volunteers</a></li>
          <li class="forPartners"><a href="#" title="for Partners">for Partners</a></li>
          <li class="forEducationAndResearch"><a href="#" title="for Education and Research">for Education and Research</a></li>
          <li class="donate"><a href="#" title="Donate">Donate</a></li>
        </ul>
      </div>
      <div id="mainContent">
        <div id="informedBlock">
        <div id="peopleVoting"><img src="/images/peoplevoting.jpg" alt="people voting" /></div>
        <div id="becomingInformed">
          <div id="becomingInformedTitle"><img src="/images/becominginformed.png" alt="Becoming an Informed Voter Just Got Easier" /></div>
          <div id="becomingInformedText"><div>Get your customized sample ballot and evaluate 
            your candidates and ballot measures.  Pictures, bios, and, most importantly, 
            the candidates’ positions on the issues are all presented side-by-side for 
            easy comparison.  And, all information is candidate authored or obtained 
            from the candidates’ websites.  Vote-USA has no political agenda.  
            We can also provide your elected representatives and information on 
            elections going back to 2004.  So …</div></div>
          <div id="findState">
            <a class="findStateButton" title="Find Your State" href="#">Find Your State</a>        
          </div>
        </div>
        </div>
        <div id="missionBlock">
          <div id="ourMission"><div>Our Mission</div></div>
          <div id="missionStatement">
            <div>Our democracy is so precious; we feel there must 
            be a better way to make it work.  Rather than electing our representatives 
            based on money, deceptive advertising, and political posters, we feel that 
            the Internet and social media can provide a truly vibrant and thriving 
            representative democratic environment.  Ancient Greece was the crucible 
            of Western civilization and democracy.  In an arena, citizens would gather 
            to debate and decide public policy.  Clearly, that model would not work 
            for America; however, the model we present here could work.  Finally, 
            consider the positive impact that the Internet and social media played 
            on the recent events in Tunisia, Egypt, and Algeria.</div>
          </div>
          <div id="missionPullQuote">
            <div>“...provide a truly vibrant and thriving 
            representative democratic environment.”</div></div>
       </div>
       <div id="footerBlock">
         <div id="oneStop">
          <img src="/images/onestop.png" alt="One-Stop Shopping for Voters" />
         </div>
         <div class="footerLinks">
           <ul>
             <li><a href="#" title="Home">Home</a></li>
             <li><a href="#" title="For Voters">For Voters</a></li>
             <li><a href="#" title="For Candidates">For Candidates</a></li>
             <li><a href="#" title="For State Election Authoritie">For State Election Authorities</a></li>
             <li><a href="#" title="For Volunteers">For Volunteers</a></li>
           </ul>
         </div>
         <div class="footerLinks">
           <ul>
             <li><a href="#" title="For Partners">For Partners</a></li>
             <li><a href="#" title="For Education and Research">For Education and Research</a></li>
             <li><a href="#" title="Donate">Donate</a></li>
             <li><a href="#" title="Privacy Policy">Privacy Policy</a></li>
             <li><a href="#" title="Contact Us">Contact Us</a></li>
           </ul>
         </div>
         <div id="footerBlurb">
         <div>This website is not associated with any election authority.  It is hosted and 
         maintained by Vote-USA, LLC.  All information on this site was obtained from 
         various state election suthorities, the candidates themselves or from their 
         staff or websites. Vote-USA, LLC is a 501(c)(3) charitable organization funded 
         by contributions from private citizens.  All contributions are 100% tax-deductible.</div>
         </div>
       </div>
      </div>
    </div>
    </div>
    </form>
</body>
</html>
