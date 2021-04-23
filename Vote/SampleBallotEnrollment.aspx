<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="SampleBallotEnrollment.aspx.cs" Inherits="Vote.SampleBallotEnrollmentPage" %>

<%@ Register Src="/Controls/GoogleAddressEntry.ascx" TagName="AddressEntry" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    h1
    {
      text-align: left;
      font-size: 20pt;
      font-weight: bold;
      margin: 20px 10px 0 10px;
      line-height: 1.2;
      color: #0645ad;
    }

    .content
    {
      text-align: center;
    }

    .email-fixed
    {
      margin: 20px 10px 0 10px;
      text-align: center;
    }

    .email-fixed span
    {
      font-weight: bold;
    }

    .address-entry hr
    {
      margin-top: 20px;
    }

    .heading
    {
      margin: 20px 0;
    }

    .email-entry,
    .address-entry
    {
      width: 560px;
      text-align: left;
      margin: 0 auto;
    }

    .different-email
    {
      margin-top: 4px;
      font-size: 9pt;
    }

    /*.button-block ul,
    .button-block .or-view,
    .button-block .elected-officials,
    .button-block .need-address
    { display: none;}*/

    @media only screen and (max-width: 600px)
    {
      .email-entry,
      .address-entry
      {
        width: auto;
        margin: 0 20px;
      }
    }

  </style>
<%--  <script type="text/javascript" src="/js/vote/publicutil.js"></script>--%>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <h1 id="H1" runat="server"></h1>

  <div id="EmailFixed" class="email-fixed" runat="server">
    <div>Ballot choices will be emailed to <span id="EmailFixedAddress" class="email-fixed-address" runat="server"></span></div>
    <div class="different-email"><a href="/SampleBallotEnrollment.aspx">Use a different email address</a></div>
  </div>
  
  <div id="EmailEntry" class="email-entry" runat="server">
    <div class="heading">Enter the email address your ballot choices should be emailed to.</div>
    <input type="email" class="email-entry-box"/>
    <input type="button" class="big-orange-button continue-button no-appearance" value="Continue" disabled="disabled"/>
  </div>

  <div id="AddressEntryBlock" class="address-entry" runat="server">
    <hr>
    <div class="heading">Please provide your street address so we can determine your voting districts.</div>
    <user:AddressEntry ID="AddressEntry" runat="server" />
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
  <script type="text/javascript">
    $(function() {
      ADDRESSENTRY.setForEnrollment();
      ADDRESSENTRY.setForEnrollmentEmail($(".email-fixed-address").text());
      $(".email-entry-box").on("propertychange change click keyup input paste spinchange",
          function() {
            var email = $.trim($(".email-entry-box").val());
            var valid = UTIL.isValidEmail(email);
            $(".continue-button").prop("disabled", !valid);
            if (!valid) {
              $(".continue-button").show();
              $(".address-entry").hide();
            } else {
              ADDRESSENTRY.setForEnrollmentEmail(email);
            }
          })
        .on("keypress", function() {
          var keycode = event.keyCode ? event.keyCode : event.which;
          if (keycode === 13)
            $(".continue-button").trigger("click");
        });
      $(".continue-button").on("click", function () {
        $(this).hide();
        $(".address-entry").show(500);
      });
    });
  </script>
</asp:Content>
