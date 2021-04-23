define(["jquery", "vote/adminMaster", "vote/util", "monitor",
    "jqueryui"],
  function($,
    master,
    util,
    monitor) {

    function initPage() {
      monitor.init();
      monitor.registerCallback("initRequest", initRequest);

      $("body").on("change", "#ImageFile", function () {
        var name = $(this).val();
        var slashPos = name.lastIndexOf("\\");
        if (slashPos >= 0)
          name = name.substr(slashPos + 1);
        $(".image-file-name").val(name);
        $(".image-file-changed").val("True").trigger("change");
      })
        .on("click", ".delete-button", function (event) {
        var $deleteImage = $(".delete-image");
        if ($deleteImage.val() === "False") {
          util.confirm("Are you sure you want to delete this image?", function(button) {
            if (button === "Ok") {
              $deleteImage.val("True");
              $(".update-button").trigger("click");
            }
          });
          event.preventDefault();
          return;
        }
      });
    }

    function initRequest(group) {
      if (group.group === "mc-group-uploadimage" && $(".image-file-updated").val() === "False") {
        // all of the updating actually takes place via ajax here
        var fileName = $(".image-file-name").val();
        if (!fileName) {
          util.alert("An Image file is required");
          return false;
        }
        var formData = new FormData();
        if ($(".image-file-changed").val().toLowerCase() == "true")
          formData.append("file", $("#ImageFile").get(0).files[0]);
        formData.append("id", $(".dropdown-external-name").val());
        formData.append("externalName", $.trim($(".external-name").val()));
        formData.append("fileName", fileName);
        formData.append("comments", $.trim($(".data-comments").val()));

        $.ajax({
          type: "POST",
          url: "/Admin/WebService.asmx/UploadImage",
          dataType: "json",
          contentType: false, // Not to set any content header
          processData: false, // Not to process data
          data: formData,
          success: function(result) {
            if (result.error) {
              util.alert(result.error);
            } else {
              $(".image-id").val(result.id);
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

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });