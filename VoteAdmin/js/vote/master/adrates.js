define(["jquery", "vote/adminMaster", "vote/util"], function ($, master, util) {

  var regex = /^0*([1-9][0-9]{0,4}\.[0-9]{2})$/i;
  function normalizeValue(val) {
    var match = regex.exec($.trim(val));
    if (match != null) {
      return match[1];
    } else {
      return false;
    }
  }

  function initPage() {
    $("#AdRatesContainer").on("propertychange change click keyup input paste", "input",
      function () {
        var changed = false;
        $(this).removeClass("error");
        $("#AdRatesContainer input").each(function() {
          var $this = $(this);
          if ($this.attr("data-original") !== normalizeValue($this.val())) {
            changed = true;
            return false;
          }
        });
        $(".update-button").prop("disabled", !changed);
      });

    $(".update-button").on("click", function () {
      var changes = [];
      var valid = true;

      function checkRate($td) {
        if ($td.length == 0) return false;
        var $input = $("input", $td);
        var val = normalizeValue($input.val());
        if (val === false) {
          valid = false;
          $input.addClass("error");
          return false;
        } else return $input.attr("data-original") !== val;
      }

      $(".office-class").each(function() {
        var $this = $(this);
        var changed = checkRate($this.find(".table-general-rate")) ||
          checkRate($this.find(".table-primary-rate"));
        if (valid && changed) {
          changes.push({
            Type: $this.find(".table-name").data("type"),
            OfficeLevel: $this.find(".table-mainlevel").text() || "0",
            AlternateOfficeLevel: $this.find(".table-sublevel").text() || "0",
            GeneralAdRate: $this.find(".table-general-rate input").val(),
            PrimaryAdRate: $this.find(".table-primary-rate input").val() || "0"
          });
        }
      });

      if (!valid) {
        util.alert("Please correct invalid values");
        return;
      }

      util.openAjaxDialog("Saving rates...");
      util.ajax({
        url: "/Admin/WebService.asmx/UpdateAdRates",
        data: {
          rates: changes
        },

        success: function () {
          util.closeAjaxDialog();
          $("#AdRatesContainer input").each(function () {
            var $this = $(this);
            $this.attr("data-original", $this.val());
          }).first().trigger("change");
          util.alert("Updated");
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not save rates"));
        }
      });
    });
  }

  master.inititializePage({
    callback: initPage
  });
});