<%@ Page Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="forResearch.aspx.cs" Inherits="Vote.ForResearchPage" %>

<%--<%@ Register Src="/Controls/PageHeadingResponsive.ascx" TagName="PageHeading" TagPrefix="user" %>--%>
<%--<%@ Register Src="/Controls/DonationRequestResponsive.ascx" TagName="DonationRequest" TagPrefix="user" %>--%>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    .content .ajax-loader
    {
      position: relative;
      top: 3px;
      visibility: hidden;
    }
    .content .intro-copy
    {
      margin: 10px 0 30px;
    }
    .content .state-link-select {
      max-width: 220px;
    }
    .content .links-outer
    {
      text-align: center;
    }
    .content .link-boxes
    {
      margin: 0 auto;
      font-family: 'Roboto', sans-serif;
      text-align: left;
      max-width: 780px;
    }
    .content .link-boxes.ui-accordion .ui-accordion-content {
      padding: 0;
    }
    .content .link-header {
      background: #c8c8c8;
      padding: 8px 8px 8px 28px;
      margin-top: 15px;
      font-size: 16px;
      color: #444;
      line-height: 120%;
    }
    .content .link-header:first-child {
      margin-top: 0;
    }
    .no-js .content .link-header {
      border: 1px solid #aaaaaa;
      border-top-left-radius: 4px;
      border-top-right-radius: 4px;
      padding-left: 8px;
    }
    .no-js .content .links {
      border: 1px solid #aaaaaa;
      border-top: 0;
      border-bottom-left-radius: 4px;
      border-bottom-right-radius: 4px;
    }
    .content .links p {
      padding: 6px;
      border-bottom: 1px solid #c8c8c8;
      font-size: 16px;
      color: #000000;
      line-height: 120%;
    }
    .content .links p:last-child {
      border-bottom: none;
    }
    /*
    .content .links a {
      color: #333333;
    }
    .content .links a:hover {
      color: #d00000;
    }
    */
  </style>
  <script type="text/javascript">
    $(function () {
      var stateLinksAjaxState = false;
      
      function setStateLinksAjaxActive(newState) {
        var currentState = stateLinksAjaxState;
        if (newState !== currentState) {
          stateLinksAjaxState = newState;
          var visibility = newState ? "visible" : "hidden";
          $('img.ajax-loader').css("visibility", visibility);
        }
        return currentState;
      }

      // to support this page as a no-js page, we generate the states menu
      // with links to forResearch.aspx. For a js page, we want the standard
      // home page links
//      $(".states-menu li a").each(function() {
//        var $this = $(this);
//        $this.attr("href", $this.attr("href").replace("forResearch.aspx", ""));
//      });
//      

      $(".state-link-select").change(function () {
        var select = $(".state-link-select");
        if (!setStateLinksAjaxActive(true)) {
          var stateCode = select.val();
          $.ajax({
            type: "POST",
            url: "/WebService.asmx/GetStateLinks",
            data: "{'input': '" + stateCode + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
              setStateLinksAjaxActive(false);
              $(".state-link-entries").html(result.d);
            },
            error: function (result) {
              setStateLinksAjaxActive(false);
              alert(result.status + ' ' + result.statusText);
            }
          });
        }
        select.blur();
      })
        .click(function (event) { event.stopPropagation(); });
      
      $(".link-boxes").accordion({
        collapsible: true,
        heightStyle: "content",
        activate: PUBLIC.accordionActivate
      });
    });
  </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1>Historical Voter Information</h1>
  <div class="intro-copy">
    <p>
      Vote-USA has election information dating back to 2004. We have information and pictures
      for over 80,000 candidates and office holders and over 200,000 responses to issue
      questions. We have directories for each election listing all candidates, contests
      and ballot measures as well as directories of all current office holders. From these
      directories you can navigate to information of interest, such as pictures, biographical
      information, views on issues, and candidate comparisons.</p>
  </div>
  <div class="links-outer">
    <div class="link-boxes">
      <div class="link-header">
        <p class="js-only">Elections and Elected Officials for
        <select id="StateElectionDropDown" class="state-link-select no-zoom" runat="server">
        </select>
        <img class="ajax-loader" alt="Ajax is processing" src="/images/ajax-loader16.gif" /></p>
        <p class="no-js-only">Elections and Elected Officials for <span id="StateName" runat="server"></span></p>
        <p class="no-js-only">Select a state (top right) to obtain information about another state.</p>
      </div>
      <div class="links state-link-entries" id="StateLinkEntries" runat="server">
      </div>

      <asp:PlaceHolder ID="PresidentLinks" runat="server">
      </asp:PlaceHolder>

      <asp:PlaceHolder ID="SenateLinks" runat="server">
      </asp:PlaceHolder>

      <asp:PlaceHolder ID="HouseLinks" runat="server">
      </asp:PlaceHolder>

      <asp:PlaceHolder ID="GovernorLinks" runat="server">
      </asp:PlaceHolder>
    </div>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
<%--  <user:DonationRequest ID="DonationRequest" runat="server" />--%>
</asp:Content>
