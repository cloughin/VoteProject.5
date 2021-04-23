define(["jquery", "vote/adminMaster", "vote/util"],
  function ($, master, util) {

    function initPage() {
      $(".create-csv-button").on("click", function () {
        var state = $.queryString("state").toUpperCase();
        var level = $.radioVal("level");
        var includeMissing = $("#include-m").prop("checked");
        var includeWith = $("#include-w").prop("checked");
        if (!includeMissing && !includeWith) {
          util.alert("At least one Incumbents option must be selected");
          return;
        }
        var script = level == "C"
          ? "/admin/downloadcountyofficescsv.aspx"
          : "/admin/downloadlocalofficescsv.aspx";
        location.href = script + "?state=" + state + (includeMissing ? "&m=1" : "") + (includeWith ? "&w=1" : "");
        util.alert("Creating CSV");
      });
    }

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });