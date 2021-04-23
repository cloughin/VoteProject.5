<%@ Page Title="" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Lookup2.aspx.cs" Inherits="Vote.Lookup2Page" %>

<asp:Content ID="HeadTopContent" ContentPlaceHolderID="MasterHeadTopContent" runat="server">
</asp:Content>

<asp:Content ID="HeadBottomContent" ContentPlaceHolderID="MasterHeadBottomContent" runat="server">
  <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAJGb2AGKOS0mf-VWmBQRRMH-n02RWhNKQ&libraries=places"></script>
  <script>
    $(function () {
      var $address = $(".address");
// ReSharper disable once UseOfImplicitGlobalInFunctionScope
      var autocomplete = new google.maps.places.Autocomplete($address[0], {
        types: ['geocode'], 
        componentRestrictions: { country: 'us' }
      });
      $address.on("focus", function () {
        if ($(window).height() <= 432 || $(window).width() <= 432)
          $('html, body').animate({
            scrollTop: $address.offset().top - 10
          }, 500);
      });
      autocomplete.addListener('place_changed', function () {
        $("#results .data").text("");
        var componentForm = {
          street_number: 'short_name',
          route: 'long_name',
          locality: 'long_name',
          administrative_area_level_1: 'short_name',
          administrative_area_level_2: 'long_name',
          administrative_area_level_3: 'long_name',
          postal_code: 'short_name',
          postal_code_suffix: 'short_name'
        };
        var place = autocomplete.getPlace();
        // place.formatted_address
        var address = {};
        for (var i = 0; i < place.address_components.length; i++) {
          var addressType = place.address_components[i].types[0];
          if (componentForm[addressType]) {
            var val = place.address_components[i][componentForm[addressType]];
            address[addressType] = val;
          }
        }
        address.lat = place.geometry.location.lat();
        address.lng = place.geometry.location.lng();
        //var data = { zip5: address.postal_code, zip4: address.postal_code_suffix }
        //$.ajax({
        //  type: "POST",
        //  url: "/WebService.asmx/GetLdsInfo",
        //  contentType: "application/json; charset=utf-8",
        //  data: JSON.stringify(data),
        //  dataType: "json",
        //  success: function (result) {
        //    for (var prop in result.d) {
        //      $("#" + prop).text(result.d[prop]);

        //    }
        //  }
        //});
        var data2 = { latitude: address.lat, longitude: address.lng };
        $.ajax({
          type: "POST",
          url: "/WebService.asmx/GetTigerData",
          contentType: "application/json; charset=utf-8",
          data: JSON.stringify(data2),
          dataType: "json",
          success: function (result) {
            for (var prop in result.d) {
              $("#Tiger" + prop).text(result.d[prop]);

            }
          }
        });

        for (var prop in address) {
          $("#" + prop).text(address[prop]);
        }
      });
    });
  </script>
  <style type="text/css">
     #address
     {
       max-width: 400px;
       width: 96%;
       margin: 10px 0 4px 2%;
     }
    #results
    {
      border-collapse:collapse;
      border-spacing:0;
      font-family: Arial;
      max-width: 96%;
      margin: 50px 0 0 2%;
    }
    td,th
    {
      border: 1px solid #aaa;
      padding: 3px;
    }
  </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MasterMainContent" runat="server">
    <div>
    <input type="text" id="address" class="address no-zoom" placeholder="Enter your street address" />
    </div>
      <table id="results">
        <thead>
          <tr><th>Item</th><th>Value</th><th>Source</th></tr>
        </thead>
        <tbody>
          <tr><td>Street Number</td><td id="street_number" class="data"></td><td>Google API</td></tr>
          <tr><td>Street</td><td id="route" class="data"></td><td>Google API</td></tr>
          <tr><td>City</td><td id="locality" class="data"></td><td>Google API</td></tr>
          <tr><td>State</td><td id="administrative_area_level_1" class="data"></td><td>Google API</td></tr>
          <tr><td>County</td><td id="administrative_area_level_2" class="data"></td><td>Google API</td></tr>
          <tr><td>Locality</td><td id="administrative_area_level_3" class="data"></td><td>Google API</td></tr>
          <tr><td>Postal Code</td><td id="postal_code" class="data"></td><td>Google API</td></tr>
          <tr><td>Postal Code Suffix</td><td id="postal_code_suffix" class="data"></td><td>Google API</td></tr>
          <tr><td>Latitude</td><td id="lat" class="data"></td><td>Google API</td></tr>
          <tr><td>Longitude</td><td id="lng" class="data"></td><td>Google API</td></tr>
          <%--
          <tr><td>Congress</td><td id="Congress" class="data"></td><td>Existing LDS data</td></tr>
          <tr><td>County Code</td><td id="County" class="data"></td><td>Existing LDS data</td></tr>
          <tr><td>State House</td><td id="StateHouse" class="data"></td><td>Existing LDS data</td></tr>
          <tr><td>State Senate</td><td id="StateSenate" class="data"></td><td>Existing LDS data</td></tr>
          --%>
          <tr><td>County Code</td><td id="TigerCounty" class="data"></td><td>Tiger data</td></tr>
          <tr><td>Congress</td><td id="TigerCongress" class="data"></td><td>Tiger data</td></tr>
          <tr><td>State House</td><td id="TigerLower" class="data"></td><td>Tiger data</td></tr>
          <tr><td>State Senate</td><td id="TigerUpper" class="data"></td><td>Tiger data</td></tr>
          <tr><td>District</td><td id="TigerDistrict" class="data"></td><td>Tiger data</td></tr>
          <tr><td>District Name</td><td id="TigerDistrictName" class="data"></td><td>Tiger data</td></tr>
          <tr><td>District Long Name</td><td id="TigerDistrictLongName" class="data"></td><td>Tiger data</td></tr>
          <tr><td>Place</td><td id="TigerPlace" class="data"></td><td>Tiger data</td></tr>
          <tr><td>Place Name</td><td id="TigerPlaceName" class="data"></td><td>Tiger data</td></tr>
          <tr><td>Place Long Name</td><td id="TigerPlaceLongName" class="data"></td><td>Tiger data</td></tr>
          <tr><td>Elementary</td><td id="TigerElementary" class="data"></td><td>Tiger data</td></tr>
          <tr><td>Elementary Name</td><td id="TigerElementaryName" class="data"></td><td>Tiger data</td></tr>
          <tr><td>Secondary</td><td id="TigerSecondary" class="data"></td><td>Tiger data</td></tr>
          <tr><td>Secondary Name</td><td id="TigerSecondaryName" class="data"></td><td>Tiger data</td></tr>
          <tr><td>Unified</td><td id="TigerUnified" class="data"></td><td>Tiger data</td></tr>
          <tr><td>Unified Name</td><td id="TigerUnifiedName" class="data"></td><td>Tiger data</td></tr>
        </tbody>
      </table>
</asp:Content>

<asp:Content ID="BottomContent" ContentPlaceHolderID="MasterBottomContent" runat="server">
</asp:Content>
