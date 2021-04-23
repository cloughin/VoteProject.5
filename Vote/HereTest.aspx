<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" 
CodeBehind="HereTest.aspx.cs" Inherits="Vote.HereTestPage" %>

<%@ Register Src="/Controls/GoogleAddressEntry.ascx" TagName="AddressEntry" TagPrefix="user" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
  </asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <style type="text/css">
    .content
    {
      text-align: center;
    }
    .centered
    {
      text-align: left;
      margin: 0 auto;
    }
    .address-entry
    {
      width: 90%;
      max-width: 500px;
      margin-top: 20px;
    }
    .here-entry
    {
      width: 90%;
      max-width: 500px;
      margin-top: 20px;
    }
  </style>
  <script type="text/javascript">
    $(function ()
    {
      $(".here-input").on("keyup", function () {
        var val = $(this).val();
        var data = {
          app_id: "Dh6VnOGAAP5MrlWmzuIw",
          app_code: "BnOWLaaVKk3lG9rKNGUh4g",
          country: "USA",
          beginHighlight: "<b>",
          endHighlight: "</b>",
          query: val
        };
        $.ajax({
          //cache: false,
          data: data,
          dataType: "json",
          error: function(jqXHR, status, error) {
            var x = 1;
          },
          method: "GET",
          success: function(result, status, jqXHR) {
            //var lines = [];
            //$.each(result.suggestions, function() {
            //  //var address = this.label;
            //  //for (var x = 0; x < 3; x++) {
            //  //  var inx = address.indexOf(", ");
            //  //  if (inx > 0)
            //  //    address = address.substr(inx + 2);
            //  //}
            //  var line = "";
            //  if (this.address.houseNumber)
            //  lines.push(address +
            //    "," +
            //    this.address.city +
            //    ", " +
            //    this.address.state +
            //    " " +
            //    this.address.postalCode);
            //});
            var x = 1;
          },
          url: "http://autocomplete.geocoder.cit.api.here.com/6.2/suggest.json"
        });
      });
    });
  </script>
</asp:Content>

<asp:Content ID="BannerContent" ContentPlaceHolderID="MasterBannerContent" runat="server">
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
  <div class="address-entry centered">
    <h3>Google</h3>
    <user:AddressEntry ID="AddressEntry" runat="server" />
  </div>
  
  <div class="here-entry centered">
    <h3>Here.com</h3>
    <input type="text" class="here-input"/>
    <div class="results">

    </div>
  </div>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
