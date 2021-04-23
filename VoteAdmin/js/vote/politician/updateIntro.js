define(["jquery", "vote/adminMaster", "vote/util", "monitor",
    "vote/politician/updateAnswer", "jqueryui"],
  function ($, master, util, monitor) {

    var $$ = util.$$;

    var afterUpdateContainer = function (group) {
      if (!group) return;
      initGroup($$(group.container));
    };

    var afterUpload = function () {
      $("#tab-upload p.too-small").hide(0);
      var jqpic = $(".image-picture");
      jqpic.hide("scale", 200);
      jqpic.attr("src", util.updateNocacheUrl(jqpic.attr("src")));
      jqpic.show("scale", 1000);
    };

    var checkImageSize = function () {
      var current = !$("#tab-upload p.too-small").is(":hidden");
      if (isImageTooSmall() !== current)
        setTimeout(reCheckImageSize, 750);
    };

    var initGroup = function ($group) {
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

    var initPage = function () {
      monitor.init();
      monitor.registerCallback("afterUpload", afterUpload);
      monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);

      util.safeBind($(".vcentered-tab"), "click", util.tabClick);
      setInterval(checkImageSize, 2000);
      util.safeBind($("#tab-upload .file-name-clear"), "click", function () {
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

      var defaultTab = "tab-social";
      var tabInfo = master.parseFragment(defaultTab);
      if (tabInfo.tab === "tab-bio2" && tabInfo.subTab) {
        var active = util.getTabIndex("bio2-tabs", tabInfo.subTab);
        if (active >= 0) {
          $$('bio2-tabs').tabs("option", "active", active);
          var professionalTabId = "tab-bio2-allbio333333";
          if (tabInfo.subTab == professionalTabId && !$(".answer-textbox", $$(professionalTabId)).val()) {
            // if new (empty) text entry, auto fill the source and date
            $(".source-textbox", $$(professionalTabId))
              .val("Provided by Election Authority")
              .change();
            var today = new Date();
            today = (today.getMonth() + 101).toString().substr(1) + "/" +
             (today.getDate() + 100).toString().substr(1) + "/" +
             today.getFullYear();
            $(".date-textbox", $$(professionalTabId))
              .val(today)
              .change();
          }
        }
      }

      // force dob entry if needed
      if ($("body").hasClass("need-dob")) {
        $("#dob-dialog").dialog({
          closeOnEscape: false,
          dialogClass: "dob-dialog",
          modal: true,
          resizable: false,
          title: "Date of Birth"
        });
      }

      $(".dob-dialog .date-picker-dob").on(
        "propertychange change click keyup input paste spinchange",
        function() {
          var str = $(this).val();
          var disabled = !str;
          try {
            $.datepicker.parseDate("m/d/yy", str, {
              yearRange: "-90:+0"
            });
          } catch (e) {
            disabled = true;
          };
          $(".dob-dialog .save-dob-button").toggleClass("disabled", disabled);
        });

      $(".dob-dialog .save-dob-button").click(function() {
        if ($(this).hasClass("disabled"))
          util.alert("Please enter a valid date of birth");
        else {
          $(".input-element.dateofbirth .date-picker-dob")
            .val($(".dob-dialog .date-picker-dob").val())
            .trigger("change");
          $("#tab-contact .update-button").removeClass("disabled").trigger("click");
          $("#dob-dialog").dialog("close");
        }
      });
    };

    var isImageTooSmall = function () {
      var width = $("#tab-upload .image-picture").width();
      if (!width) return false;
      return !!(width < 300);
    };

    var reCheckImageSize = function () {
      if (isImageTooSmall())
        $("#tab-upload p.too-small").show(400);
      else
        $("#tab-upload p.too-small").hide(400);
    };

    master.inititializePage({
      callback: initPage
    });
  });