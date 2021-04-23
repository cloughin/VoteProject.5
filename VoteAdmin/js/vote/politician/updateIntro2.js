define(["jquery", "vote/adminMaster", "vote/util", "monitor", 
    "vote/politician/updateAnswer", "jqueryui"],
  function ($, master, util, monitor) {

    var $$ = util.$$;

    var afterUpdateContainer = function (group, args) {
      if (!group) return;
      initGroup($$(group.container));
    };

    var afterUpload = function(group, args) {
      $("#tab-upload p.too-small").hide(0);
      var jqpic = $(".image-picture");
      jqpic.hide("scale", 200);
      jqpic.attr("src", util.updateNocacheUrl(jqpic.attr("src")));
      jqpic.show("scale", 1000);
    };

    var checkImageSize = function() {
      var current = !$("#tab-upload p.too-small").is(":hidden");
      if (isImageTooSmall() !== current)
        setTimeout(reCheckImageSize, 750);
    };

    var initGroup = function($group) {
      if ($group != null && $group.hasClass("answer-container")) return; // handled in answer
      
      $(".date-picker", $group).datepicker({
        changeYear: true,
        yearRange: "2010:+0"
      });

      $(".date-picker-dob", $group).datepicker({
        changeYear: true,
        yearRange: "-90:+0"
      });
    };

    var initPage = function() {
      monitor.init();
      monitor.registerCallback("afterUpload", afterUpload);
      monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);

      util.safeBind($(".vcentered-tab"), "click", util.tabClick);
      setInterval(checkImageSize, 2000);
      util.safeBind($("#tab-upload .file-name-clear"), "click", function (event) {
        monitor.clearFilename('mc-group-upload');
      });
      util.safeBind($("#tab-upload .update-button"), "click", function (event) {
        monitor.upload(event.target);
      });

      initGroup(null);

      window.onbeforeunload = function () {
        if (monitor.hasChanges())
          return "There are entries on your form that have not been submitted";
    };
  };

  var isImageTooSmall = function() {
    var width = $("#tab-upload .image-picture").width();
    if (!width) return false;
    return !!(width < 300);
  };

  var reCheckImageSize = function() {
    if (isImageTooSmall())
      $("#tab-upload p.too-small").show(400);
    else
      $("#tab-upload p.too-small").hide(400);
  };

  master.inititializePage({
    callback: initPage
  });
});