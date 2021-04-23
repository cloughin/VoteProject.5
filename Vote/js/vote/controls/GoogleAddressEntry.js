$(function() {

  var $alertDialog;
  var $alertDialogButtons;
  var alertDialogCallback = null;
  var alertDialogInitialized = false;
  var forEnrollment = false;
  var forEnrollmentEmail = null;

  var htmlEscape = function(str) {
    return String(str)
      .replace(/&/g, '&amp;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/\u2018/g, "&lsquo;")
      .replace(/\u2019/g, "&rsquo;")
      .replace(/\u201C/g, "&ldquo;")
      .replace(/\u201D/g, "&rdquo;")
      .replace(/\u00AE/g, "&reg;")
      .replace(/\u2122/g, "&trade;")
      .replace(/\u2013/g, "&ndash;")
      .replace(/\u2014/g, "&mdash;");
  };

  var initAlertDialog = function() {
    if (alertDialogInitialized) return;
    $('body').append('<div id="alert-dialog" class="hidden"><div class="inner">' +
      '<div class="message"></div><div class="bottom-box button-box">' +
      '<input type="button" class="alert-button-1 button-1 button-smaller"/>' +
      '<input type="button" class="alert-button-2 button-1 button-smaller"/>' +
      '<input type="button" class="alert-button-3 button-1 button-smaller"/>' +
      '<input type="button" class="alert-button-4 button-1 button-smaller"/>' +
      '</div></div></div>');
    $alertDialog = $('#alert-dialog').dialog({
      autoOpen: false,
      close: onAlertDialogClose,
      dialogClass: "alert-dialog",
      modal: true,
      resizable: false,
      width: "auto",
      minHeight: 0
    });
    $alertDialogButtons = $('input[type=button]', $alertDialog)
      .on("click", function(event) {
        var fn = null;
        if (alertDialogCallback) {
          fn = alertDialogCallback;
          alertDialogCallback = null;
        }
        $alertDialog.dialog("close");
        // ReSharper disable once InvokedExpressionMaybeNonFunction
        if (fn) fn($(event.target).val());
      });
    alertDialogInitialized = true;
  };

  var alert = function() {
    initAlertDialog();
    var ax = 0;
    var message = "Message";
    var title = "Alert";
    var callback = null;
    var buttons = ["Ok"];
    if (arguments.length > 0 && typeof arguments[0] === "string") {
      message = arguments[ax++];
    }
    for (var i = ax; i < arguments.length; i++) {
      switch (typeof arguments[i]) {
      case "string":
        title = arguments[i];
        break;

      case "function":
        callback = arguments[i];
        break;

      default:
        if ($.isArray(arguments[i]))
          buttons = arguments[i];
        break;
      }
    }
    i = 0;
    while (i < buttons.length) {
      var buttonTypeInx = i + 1;
      if (buttonTypeInx === buttons.length ||
        (typeof buttons[buttonTypeInx] !== "number")) {
        buttons.splice(buttonTypeInx, 0, 1);
      }
      i += 2;
    }
    $alertDialog.dialog("option", "title", htmlEscape(title));
    $('.message', $alertDialog).html(htmlEscape(message).replace(/\n/g, "<br />"));
    alertDialogCallback = callback;
    $alertDialogButtons.hide().removeClass("button-1 button-2 button-3");
    for (i = 0; i < buttons.length; i += 2) {
      var j = i / 2;
      $($alertDialogButtons[j])
        .show()
        .val(buttons[i])
        .addClass("button-" + buttons[i + 1]);
    }
    $alertDialog.dialog("open");
    //moveDialogToTop("alert-dialog");
  };

  var onAlertDialogClose = function() {
    if (alertDialogCallback) {
      var fn = alertDialogCallback;
      alertDialogCallback = null;
      fn("esc");
    }
  };

  function setUpLinks() {
    var state = $.cookie('State');
    var congress = $.cookie('Congress');
    var stateSenate = $.cookie('StateSenate');
    var stateHouse = $.cookie('StateHouse');
    var county = $.cookie('County');
    var district = $.cookie('District');
    var place = $.cookie('Place');
    var elementary = $.cookie('Elementary');
    var secondary = $.cookie('Secondary');
    var unified = $.cookie('Unified');
    var cityCouncil = $.cookie('CityCouncil');
    var countySupervisors = $.cookie('CountySupervisors');
    var schoolDistrictDistrict = $.cookie('SchoolDistrictDistrict');

    if (state &&
      congress &&
      stateSenate &&
      (stateHouse || state == "DC" || state == "NE") &&
      county &&
      district != null &&
      place != null) {
      // elected officials
      var q = [];
      q.push("State=" + state);
      q.push("Congress=" + congress);
      q.push("StateSenate=" + stateSenate);
      if (stateHouse)
        q.push("StateHouse=" + stateHouse);
      q.push("County=" + county);
      q.push("District=" + district);
      q.push("Place=" + place);
      if (elementary)
        q.push("Esd=" + elementary);
      if (secondary)
        q.push("Ssd=" + secondary);
      if (unified)
        q.push("Usd=" + unified);
      if (cityCouncil)
        q.push("Cc=" + cityCouncil);
      if (countySupervisors)
        q.push("Cs=" + countySupervisors);
      if (schoolDistrictDistrict)
        q.push("Sdd=" + schoolDistrictDistrict);
      $(".active-button-block .elected-officials")
        .attr("href", "/Elected.aspx?" + q.join("&"));

      // elections
      var data = {
        stateCode: state,
        congress: congress,
        stateSenate: stateSenate,
        stateHouse: stateHouse,
        county: county,
        district: district,
        place: place,
        elementary: elementary || "",
        secondary: secondary || "",
        unified: unified || "",
        cityCouncil: cityCouncil || "",
        countySupervisors: countySupervisors || "",
        schoolDistrictDistrict: schoolDistrictDistrict || ""
      };
      $.ajax({
        type: "POST",
        url: "/WebService.asmx/GetUpcomingElections",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(result) {
          var upcomingElections = result.d.Elections;
          switch (upcomingElections.length) {
          case 0:
            alert("Could not determine elections");
            break;

            default:
              // build links
              var items = [];
              $.each(upcomingElections, function () {
                var extraClasses = this.HasContests ? "" : " disabled no-contests";
                var message = this.HasContests ? "" : '<p class="no-contests-message">There are no office contests or ballot measures for your legislative districts.</p>';
                items.push('<li><a class="big-orange-button bob-with-arrow' + extraClasses +
                  '" href="' + this.Href + '">' + this.Description + '</a>' + message + '</li>');
              });
              $(".active-button-block ul").html(items.join(""));
              var $notAvailable = $(".active-button-block .not-available");
              if (result.d.IsPast) {
                $notAvailable.text("Upcoming election information is not yet available. Here is your most recent election:");
              } else {
                $notAvailable.addClass("hidden");
              }
              $(".address-entry .active-button-block").removeClass("hidden");
            $(".address-entry .default-button-block").addClass("hidden");
            break;
          }
        },
        error: function getUpcomingElectionsFailed(result) {
          alert(result.status + ' ' + result.statusText);
        }
      });
    } else
      alert("Could not determine elections");
  }

  function setForEnrollment() {
    forEnrollment = true;
    $(".default-button-block").addClass("hidden");
    $(".active-button-block").addClass("hidden");
    $(".enrollment-button-block").removeClass("hidden");
  }

  function setForEnrollmentEmail(email) {
    forEnrollmentEmail = email;
  }

  function haveAddress(address) {
    if (address) {
      $address.val(address).prop("disabled", true);
      if (!forEnrollment) setUpLinks();
    } else {
      $address.val(address).prop("disabled", false);
      $(".active-button-block").addClass("hidden");
      $(".enrollment-button-block").addClass("hidden");
      $(".default-button-block").toggleClass("hidden", forEnrollment);
    }
  }

  var $address = $(".address-entry .address-search");
  if ($.cookie('Address') &&
    $.cookie("State") &&
    $.cookie("County") &&
    $.cookie("Congress") &&
    $.cookie("StateSenate") &&
    $.cookie("District") != null &&
    $.cookie("Place") != null &&
    $.cookie("Geo")) {
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/IsCookieTrusted",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(response) {
        if (response.d) {
          haveAddress($.cookie('Address'));
        } else {
          haveAddress("");
        }
      }
    });
  } else {
    haveAddress("");

  }

  $(".address-entry .use-this-address").click(function() {
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/UseThisAddress",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      data: JSON.stringify({
        forEnrollment: !!forEnrollment,
        forEnrollmentEmail: forEnrollmentEmail || ""
      }),
      success: function(response) {
        var result = response.d;
        if (result)
          document.location.href = result;
      },
      error: function() { $message.html(response.status + ' ' + response.statusText); }
    });
  });

// ReSharper disable once UseOfImplicitGlobalInFunctionScope
  if (typeof google === "undefined") return;
  var $message = $(".address-entry .error-message");
  $message.text("");
  $(".address-entry .enter-new-address")
    .click(function () {
      haveAddress("");
      event.preventDefault();    });
  $(".address-entry .use-address")
    .click(function () {
      alert("Vote-USA uses your address only to provide you with customized electoral" +
        " information, including your ballot choices, elected representatives, and various" +
        " election reports. We do not sell, trade, or otherwise transfer any of this" +
        " information to any other person, organization or third party. We cookie your" +
        " address for the sole purpose of saving you the trouble of reentering it when" +
        " next you visit this site.");
      event.preventDefault();
    });
  // ReSharper disable once UseOfImplicitGlobalInFunctionScope
  var autocomplete = new google.maps.places.Autocomplete($address[0], {
    types: ['geocode'],
    componentRestrictions: { country: 'us' }
  });
  $address.on("focus", function() {
      if ($(window).height() <= 432 || $(window).width() <= 432)
        $('html, body').animate({
          scrollTop: $address.offset().top - 10
        }, 500);
    })
    .keypress(function(event) {
      if (event.keyCode === 10 || event.keyCode === 13)
        event.preventDefault();
    });
  autocomplete.addListener('place_changed', function() {

    function warning() {
      alert(
        "There is not enough information to reliably determine your voting districts. Please include a street address along with a zip code or a city and state.");
    }

    var componentForm = {
      street_number: 'short_name',
      route: 'long_name',
      locality: 'long_name',
      sublocality: 'long_name',
      sublocality_level_1: 'long_name',
      administrative_area_level_1: 'short_name',
      postal_code: 'short_name',
      postal_code_suffix: 'short_name'
    };
    var place = autocomplete.getPlace();
    if (!place ||
      !place.address_components ||
      !place.formatted_address ||
      !place.geometry ||
      !place.geometry.location) {
      warning();
      return;
    }
    var address = {};
    for (var i = 0; i < place.address_components.length; i++) {
      var addressType = place.address_components[i].types[0];
      if (componentForm[addressType]) {
        var val = place.address_components[i][componentForm[addressType]];
        address[addressType] = val;
      }
    }
    address.formatted_address = place.formatted_address;
    address.lat = place.geometry.location.lat();
    address.lng = place.geometry.location.lng();
    address.street_address = $.trim((address.street_number || "") +
      " " +
      (address.route || ""));
    address.city = address.locality ||
      address.sublocality ||
      address.sublocality_level_1 ||
      "";
    address.state = address.administrative_area_level_1;
    address.zip5 = address.postal_code || "";
    address.zip4 = address.postal_code_suffix || "";
    var components = [
      address.street_address, address.city, address.state, address.zip5, address.zip4
    ];
    for (var j = 0; j < components.length; j++)
      components[j] = encodeURIComponent(components[j]);
    address.components = components.join("|");
    if (!address.street_address) {
      warning();
      return;
    }
    var data =
    {
      formattedAddress: address.formatted_address,
      components: address.components,
      stateCode: address.administrative_area_level_1,
      latitude: address.lat,
      longitude: address.lng,
      forEnrollment: !!forEnrollment,
      forEnrollmentEmail: forEnrollmentEmail || ""
    };
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/FindLocation",
      contentType: "application/json; charset=utf-8",
      data: JSON.stringify(data),
      dataType: "json",
      success: function(response) {
        var result = response.d;
        if (!result.ErrorMessage) {
          if (result.DomainCode === "US") {
            var expires = 30; // days
            $.cookie('Address', result.FormattedAddress, {
              expires: expires
            });
            $.cookie('Components', address.components, {
              expires: expires,
              noEncode: true
            });
            $.cookie('State', result.StateCode, {
              expires: expires
            });
            $.cookie('Congress', result.Congress, {
              expires: expires
            });
            $.cookie('County', result.County, {
              expires: expires
            });
            $.cookie('StateSenate', result.StateSenate, {
              expires: expires
            });
            $.cookie('StateHouse', result.StateHouse, {
              expires: expires
            });
            $.cookie('District', result.District, {
              expires: expires
            });
            $.cookie('Place', result.Place, {
              expires: expires
            });
            $.cookie('Elementary', result.Elementary, {
              expires: expires
            });
            $.cookie('Secondary', result.Secondary, {
              expires: expires
            });
            $.cookie('Unified', result.Unified, {
              expires: expires
            });
            $.cookie('CityCouncil', result.CityCouncil, {
              expires: expires
            });
            $.cookie('CountySupervisors', result.CountySupervisors, {
              expires: expires
            });
            $.cookie('SchoolDistrictDistrict', result.SchoolDistrictDistrict, {
              expires: expires
            });
            $.cookie('Geo', address.lat.toFixed(6) + "," + address.lng.toFixed(6), {
              expires: expires,
              noEncode: true
            });

            var d = new Date();
            d.setDate(d.getDate() + expires);
            $.cookie('CDate',
              d.getUTCMonth() +
              1 +
              "/" +
              d.getUTCDate() +
              "/" +
              d.getUTCFullYear() +
              " " +
              d.getUTCHours() +
              ":" +
              d.getUTCMinutes() +
              ":" +
              d.getUTCSeconds(), {
                expires: expires
              });
            $("body").data("c", "True");
          }
          if (result.Success) {
            if (forEnrollment)
              document.location.href = result.RedirectUrl;
            else
              haveAddress($.cookie("Address"));
          }
        } else {
          $message.html(result.ErrorMessage);
        }
      },
      error: function(response) {
        $message.html(response.status + ' ' + response.statusText);
      }
    });
  });
  window.ADDRESSENTRY = {
    alert: alert,
    setForEnrollment: setForEnrollment,
    setForEnrollmentEmail: setForEnrollmentEmail
  }
});