define(["jquery", "vote/adminMaster", "vote/util", "monitor",
    "jqueryui"],
  function($,
    master,
    util,
    monitor) {

    function initPage() {
      monitor.init();
      monitor.registerCallback("initRequest", initRequest);

      $("body").on("change", "#AdImageFile", function () {
        var name = $(this).val();
        var slashPos = name.lastIndexOf("\\");
        if (slashPos >= 0)
          name = name.substr(slashPos + 1);
        $(".image-file-name").val(name);
        $(".image-file-changed").val("True").trigger("change");
      });
    }

    function initRequest(group) {
      if (group.group === "mc-group-setupad") {
        // we need to upload the image before the main update if it's new
        var fileName = $(".image-file-name").val();
        if (!fileName) {
          util.alert("An Ad Image file is required");
          return false;
        }
        if ($(".image-file-changed").val().toLowerCase() == "true") {
          var file = $("#AdImageFile").get(0).files[0];
          var formData = new FormData();
          formData.append("file", file);
          formData.append("adType", "H");
          formData.append("stateCode", "");
          formData.append("electionKey", "");
          formData.append("officeKey", "");

          $.ajax({
            type: "POST",
            url: "/Admin/WebService.asmx/UploadBannerAdImage",
            dataType: "json",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data
            data: formData,
            success: function(result) {
              if (result.error) {
                util.alert(result.error);
              } else {
                $(".image-file-changed").val("False");
                $(".image-file-updated").val("True");
                $(".update-button").trigger("click");
              };
            },
            error: function(result) {
              util.alert(util.formatAjaxError(result, "Could not upload image"));
            }
          });
          return false;
        }
      }

    }

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });