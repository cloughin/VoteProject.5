$(function () {
  $(".address-entry .address-search").val($.cookie('Address'));
  $(".address-entry-inline .submit-button").click(function (event) {
    event.preventDefault();
    var $message = $(".address-entry .error-message");
    $message.text("");
    var address = $.trim($(".address-entry .address-search").val());
    if (!address) {
      $message.text("Please enter an address or a 9-digit zip");
    } else {
      var data = {
        input: address,
        remember: $(".address-entry input.remember-me").prop("checked"),
        email: "",
        siteId: ""
      };
      $.ajax({
        type: "POST",
        url: "/WebService.asmx/FindAddress",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
          var result = response.d;
          if (result.SuccessMessage) {
            if (result.DomainCode === "US") {
              // set cookie
              var p = {};
              if (result.Remember)
                p.expires = 365;
              $.cookie('Address', address, p);
            }
            if (result.RedirectUrl)
              document.location.href = result.RedirectUrl;
          } else {
            $message.html(result.ErrorMessages.join('<br />'));
          }
        },
        error: function () {
          $message.html(response.status + ' ' + response.statusText);
        }
      });
    }
  });
});
