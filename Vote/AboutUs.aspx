<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="AboutUs.aspx.cs" Inherits="Vote.AboutUsPage" %>

<%--<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>--%>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>About Us</h1>
  <div class="intro-copy">
    <h2>Organization and History</h2>

    <p><a href="/">Vote-USA.org</a> was developed, maintained and is the sole endeavor of Vote USA Inc.  
      Vote USA Inc., established in 2004, is a Washington DC corporation with a 501(c)(3) IRS 
      non-profit, non-partisan, tax deductible classification.</p> 
 
    <p>Vote-USA.org began because in 2003, the founder of this project, 
      <a href="https://www.linkedin.com/in/ron-kahlow-ab558557/" target="_blank">Ron Kahlow</a>, 
      went to the polls to vote for the Virginia State Senate and State House offices. He found 
      that he didn’t have the slightest idea who any of the candidates were, or anything about 
      their positions and views, or any knowledge about the ballot measures. Some candidate 
      names, found on the clutter of street and lawn posters, seemed familiar. He questioned 
      the value of voting if voters knew nothing about who or what they were voting for. 
      This seemed like a major flaw in our representative democracy. To address the problem, 
      he launched Vote-USA.org. 2004 was the first year Vote-USA.org provided election coverage 
      on its website.</p> 
 
    <p>Since that time, Vote-USA.org  has researched over 20,000 candidates and over 500 issues. 
      We have covered all general elections in all states and most off-year, special and primary 
      elections since 2004. We have provided voters with over 20 million candidate comparison pages. 
      We have provided voters with over 20 million candidate comparison pages. 
      In the elections of 2016 and 2018 we provided nearly 3 million voters with over 
      9 million pages of information.</p>
 
    <h2>Philosophy and Mission</h2>
 
    <p>We believe that American government should be &ldquo;of the people, by the people, and for 
      the people&rdquo; and that an informed electorate is essential for a thriving democracy.
      We therefore seek to inform and educate voters by producing and posting comprehensive 
      non-partisan voter information covering national and state elections and some local 
      elections. Our funding limited the depth of candidate information which we have been 
      able to provide and the number of local elections which we have been able to cover.</p>
 
    <h2>Our Products</h2>
 
    <h3>1. Information to Help You Vote More Informed:</h3>

    <p>After you enter your postal address, your specific office contests, candidates and 
      ballot measures are presented. Candidate comparisons make it easy 
      to compare choices. These candidate comparisons include pictures, bios, websites, 
      social media links, objectives, and positions on many issues. Our goal is to make the 
      delivery of our information effective and your learning experience satisfying.</p> 
 
    <p>All election contests and ballot measures on this site are obtained from various state and 
      local election authorities. 
      We invite every candidate to upload information to 
      promote his or her candidacy. For candidates who provide no information, we use volunteers 
      and interns to compile information from their websites, their social media, and other sources.</p>
 
    <h3>2. Your Current Elected Officials:</h3>

    <p>You can see your currently elected officials via a button on our home page, once your 
      postal address has been entered.</p>
 
    <h3>3. Historical Information and Reports:</h3>

    <p>By clicking the <a href="/forresearch.aspx">Historical Voter Information</a> link, you can obtain 
      access to all our data from 2004 to the present.</p> 
 
    <p>We also offer special reports, such as all the contests in a particular state election 
      or all the Governor, US Senate or House contests in an election.</p>
 
    <h2>Staffing</h2>
 
    <p>We operate with a small number of full-time and part-time staff, volunteers and interns.</p>
 
    <h2>Funding</h2>
 
    <p>We are funded primarily through contributions from private citizens, typically 
      100% tax-deductible. We also receive funding from a California foundation.</p>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
<%--  <user:DonationRequest ID="DonationRequest" runat="server" />--%>
</asp:Content>
