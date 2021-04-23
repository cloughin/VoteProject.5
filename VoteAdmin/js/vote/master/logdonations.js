define(["jquery", "vote/adminMaster", "vote/util"],
function ($, master, util) {

  function initPage() {
    $(".find-reversal-button")
      .click(function () {
        var email = $.trim($("#Reversal").val());
        if (!email) {
          util.alert("Please enter an email address");
          return;
        }
        var $reversals = $("#reversals");
        $reversals.html("");
        util.openAjaxDialog("Finding donation...");
        util.ajax({
          url: "/Admin/WebService.asmx/FindDonation",
          data: {
            email: email
          },

          success: function (result) {
            util.closeAjaxDialog();
            var html;
            switch (result.d.length) {
              case 0:
                util.alert("There are no donations for this email address");
                return;

              case 1:
                html = '<span class="one-reversal">' + result.d[0].Date + " " + result.d[0].Amount + '</span>';
                break;

              default:
                var options = ['<option value="">&lt;Select donation date and amount&gt;</option>'];
                $.each(result.d, function() {
                  var text = this.Date + " " + this.Amount;
                  options.push('<option value="' + text + '">' + text + '</option>');
                });
                html = '<select class="multi-reversal">' + options.join("") + '</select>';
                break;
            }

            html = '<span class="reversal-label">Donation to reverse:</span>' +
              html +
              '<input type="checkbox" class="opt-out-checkbox"/><span class="opt-out-label">Opt out of all email</span>' +
              '<input type="button" class="button-3 do-reversal-button button-smallest" value="Reverse Donation" />';
            $reversals.html(html).data("email", email);
          },

          error: function (result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result, "Could not find donation"));
          }
        });
      });

    $("#reversals")
      .on("click", ".do-reversal-button", function() {
        var donation = $("#reversals .one-reversal").text();
        if (!donation) {
          // could be multi
          donation = $("#reversals .multi-reversal").val();
        }
        if (!donation) {
          util.alert("Please select a donation");
          return;
        }
        var optOut = $("#reversals .opt-out-checkbox").prop("checked");
        var email = $("#reversals").data("email");
        //util.alert(donation + ":" + optOut + ":" + email);
        util.openAjaxDialog("Reversing donation...");
        util.ajax({
          url: "/Admin/WebService.asmx/ReverseDonation",
          data: {
            email: email,
            donation: donation,
            optOut: optOut
          },

          success: function () {
            util.closeAjaxDialog();
            util.alert("Donation reversed");
            $("#reversals").html("");
            $("#Reversal").val("");
          },

          error: function (result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result, "Could not reverse donation"));
          }
        });
      });
  }

  master.inititializePage({
    callback: initPage
  });
});