define(["jquery", "vote/util"], function ($, util) {

  var key;
  var currentAdInfo;
  var originalAdInfo;
  var originalAdInfoJson;
  var $control;

  function init() {
    $control = $("#banner-ad-input");

    $control.on("rc_checked", ".adtype input", function () {
      var isYouTube = $("input.youtube-type", $control).prop("checked");
      var isImage = $("input.image-type", $control).prop("checked");
      //$(".imagefile", $control).toggle(isImage);
      $(".only-youtube", $control).toggleClass("hidden", isImage);
      $(".only-image", $control).toggleClass("hidden", isYouTube);
      $(".video", $control).toggle(isYouTube);
      $(".targeturl", $control).toggle(isImage);
      dataChanged();
    });

    $control.on("rc_checked rc_unchecked", "input[type=checkbox].kalypto", function () {
      dataChanged();
    });

    $control.on("input", ".textdata", function () {
      dataChanged();
    });

    $control.on("change", "#AdImageFile", function () {
      var name = $(this).val();
      var slashPos = name.lastIndexOf("\\");
      if (slashPos >= 0)
        name = name.substr(slashPos + 1);
      $(".image-file-name", $control).val(name);
      resetKalypto($("input.remove-image", $control));
      dataChanged();
    });

    $control.on("click", ".update-button", doUpdate);

    $control.on("click", ".view-button", function () {
      var url = $(this).closest(".databox").find("input").val();
      if (url) {
        if (url.toLowerCase().substr(0, 4) !== "http")
          url = "http://" + url;
        window.open(url, "view");
      }
    });
  }

  function dataChanged() {
    updateCurrentAdInfo();
    $("input.update-button", $control).prop("disabled", originalAdInfoJson === JSON.stringify(currentAdInfo));
  }

  function doUpdate() {
    var data = new FormData();
    data.append("adType", key.adType || "");
    data.append("stateCode", key.stateCode || "");
    data.append("electionKey", key.electionKey || "");
    data.append("officeKey", key.officeKey || "");
    var file = $("#AdImageFile").get(0).files[0];
    data.append("file", file);
    data.append("hasAdImage", currentAdInfo.HasAdImage);
    data.append("adImageName", currentAdInfo.AdImageName);
    data.append("adUrl", currentAdInfo.AdUrl);
    data.append("adEnabled", currentAdInfo.AdEnabled.toString());
    data.append("adMediaType", currentAdInfo.AdMediaType);
    data.append("adYouTubeUrl", currentAdInfo.AdYouTubeUrl);
    data.append("adDescription1", currentAdInfo.AdDescription1);
    data.append("adDescription2", currentAdInfo.AdDescription2);
    data.append("adDescriptionUrl", currentAdInfo.AdDescriptionUrl);
    data.append("adIsPaid", currentAdInfo.AdIsPaid.toString());
    data.append("removeImage", currentAdInfo.RemoveImage.toString());

    util.openAjaxDialog("Updating banner ad...");
    $.ajax({
      type: "POST",
      url: "/admin/webservice.asmx/UploadBannerAd",
      dataType: "json",
      contentType: false, // Not to set any content header
      processData: false, // Not to process data
      data: data,
      success: function (result) {
        if (result.error) {
          util.alert(result.error);
        } else {
          util.alert("Ad was updated");
          setKey(key);
        };
      },
      error: function (result) {
        util.alert(util.formatAjaxError(result, "Could not upload ad"));
      },
      complete: function () {
        util.closeAjaxDialog();
      }
    });
  }

  function resetKalypto($input) {
    $input.prop("checked", false);
    $input.next().removeClass("checked");
  }

  function setKey(o) {

    key = o;

    // get the current row if it exists
    util.openAjaxDialog("Getting banner ad info...");
    util.ajax({
      url: "/Admin/WebService.asmx/GetBannerAdInfo",

      data: {
        adType: key.adType || "",
        stateCode: key.stateCode || "",
        electionKey: key.electionKey || "",
        officeKey: key.officeKey || ""
       },

      success: function (result) {
        originalAdInfo = result.d;
        originalAdInfo.RemoveImage = false;
        currentAdInfo = originalAdInfo;
        originalAdInfoJson = JSON.stringify(originalAdInfo);
        resetKalypto($("input.youtube-type", $control));
        resetKalypto($("input.image-type", $control));
        resetKalypto($("input.ad-is-paid", $control));
        resetKalypto($("input.ad-enabled", $control));
        resetKalypto($("input.remove-image", $control));
        switch (originalAdInfo.AdMediaType) {
          case "Y":
            $("input.youtube-type", $control).prop("checked", true).deferredTrigger("change");
            break;

          case "I":
            $("input.image-type", $control).prop("checked", true).deferredTrigger("change");
            break;
        }
        $("input.youtube-url", $control).val(originalAdInfo.AdYouTubeUrl);
        $("input.image-file-name", $control).val(originalAdInfo.AdImageName);
        $("input.target-url", $control).val(originalAdInfo.AdUrl);
        $("input.description1", $control).val(originalAdInfo.AdDescription1);
        $("input.description2", $control).val(originalAdInfo.AdDescription2);
        $("input.description-url", $control).val(originalAdInfo.AdDescriptionUrl);
        if (originalAdInfo.AdIsPaid)
          $("input.ad-is-paid", $control).prop("checked", true).deferredTrigger("change");
        if (originalAdInfo.AdEnabled)
          $("input.ad-enabled", $control).prop("checked", true).deferredTrigger("change");
        $("input.update-button", $control).prop("disabled", true);
        $("#AdImageFile").val(null);
        $("#ads").html(originalAdInfo.AdHtml);
      },

      error: function (result) {
        util.alert(util.formatAjaxError(result, "Could not get banner ad info"));
      },

      complete: function () {
        util.closeAjaxDialog();
      }

    });
  }

  function updateCurrentAdInfo() {
    currentAdInfo.AdImageName = $.trim($("input.image-file-name", $control).val());
    currentAdInfo.AdUrl = $.trim($("input.target-url", $control).val());
    currentAdInfo.AdEnabled = $("input.ad-enabled", $control).prop("checked");
    currentAdInfo.AdMediaType = $("input[name=SetupAdAdType]:checked", $control).val() || "";
    currentAdInfo.AdYouTubeUrl = $.trim($("input.youtube-url", $control).val());
    currentAdInfo.AdDescription1 = $.trim($("input.description1", $control).val());
    currentAdInfo.AdDescription2 = $.trim($("input.description2", $control).val());
    currentAdInfo.AdDescriptionUrl = $.trim($("input.description-url", $control).val());
    currentAdInfo.AdIsPaid = $("input.ad-is-paid", $control).prop("checked");
    currentAdInfo.RemoveImage = $(".remove-image", $control).prop("checked");
  }

  $(function () {
    init();
  });

  //
  // Exports
  //

  return {
    setKey: setKey
  };
});