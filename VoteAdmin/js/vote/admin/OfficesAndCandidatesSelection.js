define(["jquery", "vote/adminMaster", "vote/util"],
  function ($, master) {

    function initPage() {
      $(".date-picker").datepicker({
        changeYear: true,
        yearRange: "2010:+1"
      });
      $(".generate-report").click(function () {
        var fromDate = $(".from-date").val();
        var toDate = $(".to-date").val();
        var reportType = $.radioVal("ReportType");
        var detail = $.radioVal("Detail");
        location.href = "/admin/OfficesAndCandidatesReport.aspx?from=" + fromDate + "&to=" + toDate +
          "&type=" + reportType + "&detail=" + detail;
      });
    }

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });