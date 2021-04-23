
function doNext($tr, inx) {
  var $this = $($tr[inx]);
  var address = $(".address", $this).text();
  var lat = parseFloat($(".lat", $this).text());
  var lng = parseFloat($(".lng", $this).text());
  var geocoder = new google.maps.Geocoder();
  geocoder.geocode({ 'address': address }, function (results, status) {
    var glat;
    var glng;
    var dlat;
    var dlng;
    if (status == google.maps.GeocoderStatus.OK) {
      glat = results[0].geometry.location.lat();
      glng = results[0].geometry.location.lng();
    }
    else {
      glat = 0.0;
      glng = 0.0;
    }

    if (glat !== 0.0 && glng !== 0.0) {
      dlat = Math.abs(lat - glat);
      dlng = Math.abs(lng - glng);
    } else {
      dlat = "";
      dlng = "";
    }

    $(".glat", $this).html(glat);
    $(".glng", $this).html(glng);
    $(".dlat", $this).html(dlat);
    $(".dlng", $this).html(dlng);
    inx++;
    if (inx < $tr.length)
      doNext($tr, inx);
  });
}
