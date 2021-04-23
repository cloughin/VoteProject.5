<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="latlng.aspx.cs" Inherits="Vote.LatLngPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Latitude Longitude Lookup</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAJGb2AGKOS0mf-VWmBQRRMH-n02RWhNKQ&libraries=places"></script>
  <script>
    $(function() {
      $("#lookup").on("click", function() {
        $("#results .data").text("");
        var latlng = $("#latlng").val().split(",");
        if (latlng.length != 2) return;
        var data2 = { latitude: $.trim(latlng[0]), longitude: $.trim(latlng[1]) };
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
      });
    })
  </script>
  <style type="text/css">
    #latlng {width: 300px;}
     #results
     {
       border-collapse:collapse;
       border-spacing:0;
       font-family: Arial;
       margin: 50px;
     }
    td,th
    {
      border: 1px solid #aaa;
      padding: 3px;
    }
  </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <input type="text" id="latlng" placeholder="Latitude, Longitude"/>
      <input type="button" id="lookup" value="Lookup" />
      <table id="results">
        <thead>
          <tr><th>Item</th><th>Value</th><th>Source</th></tr>
        </thead>
        <tbody>
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
    </div>
    </form>
</body>
</html>
