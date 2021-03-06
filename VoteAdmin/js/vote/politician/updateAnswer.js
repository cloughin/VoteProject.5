define(["jquery", "vote/adminMaster", "vote/util", "monitor", "jqueryui"],
  function ($, master, util, monitor) {

    var $$ = util.$$;

    // begin: automatically fill in source and date from current

    // notes:
    // - source and date do not show when a politician is updating
    // - they are filled in when the response changes if both fields are empty
    // - "current" is updated on any save operation

    var currentSource = "";
    var currentDate = "";

    function checkAutoSource(group) {
      if (!group.changed || !util.endsWith(group.group, "-answer")) return;
      var $container = $("#" + group.parent.container);
      var $source = $("." + group.parent.group + "-source", $container);
      var $date = $("." + group.parent.group + "-date", $container);
      if ($source.length === 1 && $date.length === 1 &&
       !$source.val() && !$date.val()) {
        $source.val(currentSource);
        $date.val(currentDate);
      }
    }

    function saveSourceAndDate(group) {
      var $container = $("#" + group.container);
      var $source = $("." + group.group + "-source", $container);
      var $date = $("." + group.group + "-date", $container);
      if ($source.length === 1 && $date.length === 1) {
        currentSource = $source.val();
        currentDate = $date.val();
      }
    };

    // end: automatically fill in source and date from current

    function init() {
      monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);
      monitor.registerCallback("clientChange", clientChange);
      monitor.registerCallback("afterUndo", afterUndo);

//      $("#main-tabs")
//        .on("click", "input.remove-line-breaks", function () {
//          var $textarea = $(this).closest(".mc-container").find("textarea");
//          $textarea.val(util.replaceLineBreaksWithSpaces($textarea.val()));
//        })
//        .on("change", ".action-menu select", function () {
//          doActionMenuChange($(this));
//        })
//        .on("click", ".today-button", function (event) {
//          var today = new Date();
//          today = (today.getMonth() + 101).toString().substr(1) + "/" +
//          (today.getDate() + 100).toString().substr(1) + "/" +
//          today.getFullYear();
//          var $this = $(this);
//          var target = $this.hasClass("for-youtubedate") ? ".youtubedate-textbox" : ".date-textbox";
//          $this.closest(".mc-container").find(target).val(today);
//        });

//        $("#view-responses-dialog")
//        .on("click", "input.edit-other-response", function() {
//          doEditOtherResponse($(this));
//        });
//        .on("change", ".youtubefrom-checkbox", function (event) {
//          var $checkbox = $(this);
//          var $context = $checkbox.closest(".answer-sub-tabs");
//          var otherClass = "youtubefromvoteusa-checkbox";
//          var hide = true;
//          if ($checkbox.hasClass(otherClass)) {
//            otherClass = "youtubefromcandidate-checkbox";
//            hide = false;
//          }
//          if ($checkbox.is(":checked")) {
//            $context.find("." + otherClass).prop("checked", false);
//          } else {
//            $checkbox.prop("checked", true);
//          }
//          $context.find(".youtube-source-fields").toggleClass("hidden", hide);
//        });

        if ($("#defer-update-answer-init").val() !== "true") initGroup(null);
    }

    function afterUpdateContainer(group) {
      if (!group) return;
      initGroup($$(group.container));
      saveSourceAndDate(group);
      checkFromCandidate(group);
    }
    
    function afterUndo(group) {
      if (!group || !group.container) return;
      var $container = $$(group.container);
      if ($container.hasClass("answer-container")) {
        var $ck = $(".youtubefromvoteusa-checkbox", $container);
        if ($ck.prop("checked")) $ck.change();
        else {
          $ck = $(".youtubefromcandidate-checkbox", $container);
          if ($ck.prop("checked")) $ck.change();
        }
      }
    }

    function checkFromCandidate(group) {
      var $container = $$(group.container);
      var $checkbox = $container.find(".youtubefromcandidate-checkbox");
      if ($checkbox.length) {
        $container.find(".youtube-source-fields").toggleClass("hidden", $checkbox.prop("checked"));
      }
    }

    function clientChange(group) {
      if (!group) return;
      checkAutoSource(group);
      //checkFromCandidate(group);
    }

    function doActionMenuChange($select, force) {
      var $container = $select.closest(".answer-container");
      var groupName = monitor.getMonitorClass($container);
      var isGroupChanged = monitor.isGroupChanged(groupName);
      var option = $select.val();
      var $sequence = $(".answer-sequence", $container);
      var sequence = $sequence.val();

      switch (option) {
        case "edit":
          // this should never be changed to. If not selected, it's always disabled.
          break;

        case "add":
          if (isGroupChanged && !force) {
            util.confirm("You have unsaved changes to the response you are currently editing." +
              " These will be lost if you continue.",
             function (button) {
               if (button === "Ok")
                 doActionMenuChange($select, true);
             });
            return;
          }

          if (isGroupChanged) monitor.undoGroup(groupName, false);
          saveResponse($container);

          // clear the text boxes, reset the changed status, store "?" as the sequence,
          // disable the "edit" button, and enable the "view" button
          $(".answer-textbox", $container).val("");
          $(".source-textbox", $container).val("");
          $(".date-textbox", $container).val("");
          $(".youtubeurl-textbox", $container).val("");
          $(".youtubedescription-textbox", $container).val("");
          $(".youtubedate-textbox", $container).val("");
          $(".youtubesource-textbox", $container).val("");
          $(".youtubesourceurl-textbox", $container).val("");
          $(".youtuberunningtime-textbox", $container).val("");
          $(".facebookvideourl-textbox", $container).val("");
          $(".facebookvideodescription-textbox", $container).val("");
          $(".facebookvideorunningtime-textbox", $container).val("");
          $(".fromcandidate .kalypto-container a", $container).removeClass("checked");
          $(".fromcandidate .kalypto-container input", $container).prop("checked", false);
          $(".fromvoteusa .kalypto-container a", $container).removeClass("checked");
          $(".fromvoteusa .kalypto-container input", $container).prop("checked", false);
          monitor.clearChangedStatus(groupName);
          $sequence.val("?");
          $("option[value='edit']", $container).prop("disabled", true);
          $("option[value='view']", $container).prop("disabled", false);

          break;

        case "view":
          // restore action in case dialog is cancelled
          $select.val(sequence === "?" ? "add" : "edit");
          var $dialog = $$("view-responses-dialog");
          if (!$dialog.length) {
            $("body").append('<div id="view-responses-dialog" class="hidden"></div>');
            $dialog = $$("view-responses-dialog");
            $dialog.dialog({
              autoOpen: false,
              width: 880,
              resizable: true,
              title: "Other Responses for This Question",
              dialogClass: 'view-responses-dialog overlay-dialog',
              // custom open and close to fix various problems
              open: master.onOpenJqDialog,
              close: master.onCloseJqDialog,
              modal: true
            });
            $dialog.on("click", "input.edit-other-response", function () {
              doEditOtherResponse($(this));
            });
          }

          fillViewResponsesDialog($container);

          $dialog.dialog("open");
          break;
      }
    }

    function doEditOtherResponse($button, force) {
      var $dialog = $$("view-responses-dialog");
      var groupName = $(".group-name", $dialog).val();
      var isGroupChanged = monitor.isGroupChanged(groupName);
      var $container = $("." + groupName);
      var sequence = $button.attr("rel");
      var $sequence = $(".answer-sequence", $container);
      var responses = $sequence.data("responses") || [];
      var response = null;

      $.each(responses, function () {
        if (this.Sequence == sequence) {
          response = this;
          return false;
        }
      });

      if (response) {
        if (isGroupChanged && !force) {
          util.confirm("You have unsaved changes to the response you are currently editing." +
              " These will be lost if you continue.",
             function (button) {
               if (button === "Ok")
                 doEditOtherResponse($button, true);
             });
          return;
        }

        if (isGroupChanged) monitor.undoGroup(groupName, false);
        saveResponse($container);

        // replace the text boxes, reset the changed status, store the sequence,
        // enable and select the "edit" button
        $(".answer-textbox", $container).val(response.Answer);
        $(".source-textbox", $container).val(response.Source);
        $(".date-textbox", $container).val(response.Date);
        $(".youtubeurl-textbox", $container).val(response.YouTubeUrl);
        $(".youtubedescription-textbox", $container).val(response.YouTubeDescription);
        $(".youtubedate-textbox", $container).val(response.YouTubeDate);
        $(".youtubesource-textbox", $container).val(response.YouTubeSource);
        $(".youtubesourceurl-textbox", $container).val(response.YouTubeSourceUrl);
        $(".youtuberunningtime-textbox", $container).val(response.YouTubeRunningTime);
        $(".youtube-source-fields", $container).toggleClass("hidden", response.YouTubeFromCandidate);
        $(".facebookvideourl-textbox", $container).val(response.FacebookVideoUrl);
        $(".facebookvideodescription-textbox", $container).val(response.FacebookVideoDescription);
        $(".facebookvideorunningtime-textbox", $container).val(response.FacebookVideoRunningTime);
        var fromCandidate = response.YouTubeUrl && response.YouTubeFromCandidate;
        var fromVoteUsa = response.YouTubeUrl && !response.YouTubeFromCandidate;
        $(".fromcandidate .kalypto-container a", $container).toggleClass("checked", fromCandidate);
        $(".fromcandidate .kalypto-container input", $container).prop("checked", fromCandidate);
        $(".fromvoteusa .kalypto-container a", $container).toggleClass("checked", fromVoteUsa);
        $(".fromvoteusa .kalypto-container input", $container).prop("checked", fromVoteUsa);
        monitor.clearChangedStatus(groupName);
        $sequence.val(response.Sequence);
        $("option[value='edit']", $container).prop("disabled", false);
        $("option[value='edit']", $container).prop("selected", true);
        $("option[value='view']", $container).prop("disabled", responses.length < 2);

        $dialog.dialog("close");
      }
    }

    function fillViewResponsesDialog($container) {
      var groupName = monitor.getMonitorClass($container);
      var $sequence = $(".answer-sequence", $container);
      var sequence = $sequence.val();
      var $dialog = $$("view-responses-dialog");

      // get the other responses data and sort it Date descending, Sequence descending
      var responses = $sequence.data("responses") || [];
      responses.sort(function (a, b) {
        var datea = new Date(a.Answer ? a.Date : a.YouTubeDate);
        var dateb = new Date(b.Answer ? b.Date : b.YouTubeDate);
        if (datea > dateb) return -1;
        if (datea < dateb) return 1;
        if (a.Sequence > b.Sequence) return -1;
        if (a.Sequence < b.Sequence) return 1;
        return 0;
      });

      var html = [];
      html.push('<input type="hidden" class="group-name" value="' + groupName + '" />');
      for (var inx = 0; inx < responses.length; inx++) {
        var response = responses[inx];
        if (response.Sequence == sequence) continue; // don't show the one we're already editing
        html.push('<div class="one-response clearfix">');
        if (response.YouTubeUrl) {
          html.push('<div class="youtube"><span>YouTube: </span>');
          html.push(response.YouTubeUrl);
          html.push('</div>');
          html.push('<div class="youtube-description">');
          html.push(util.replaceLineBreaksWithParagraphs(response.YouTubeDescription));
          html.push('</div>');
          if (response.YouTubeSource) {
            html.push('<div class="youtube-source-text"><span>YouTube Source: </span>');
            html.push(response.YouTubeSource);
            html.push(" (");
            html.push(response.YouTubeDate);
            html.push(")");
            html.push('</div>');
          }
        }
        if (response.FacebookVideoUrl) {
          html.push('<div class="facebookvideo"><span>Facebook Video: </span>');
          html.push(response.FacebookVideoUrl);
          html.push('</div>');
          html.push('<div class="facebookvideo-description">');
          html.push(util.replaceLineBreaksWithParagraphs(response.FacebookVideoDescription));
          html.push('</div>');
        }
        html.push('<div class="answer-text">');
        html.push(util.replaceLineBreaksWithParagraphs(response.Answer));
        html.push('</div>');
        html.push('<input type="button" value="Edit this response" class="edit-other-response button-2 button-smallest" rel="');
        html.push(response.Sequence);
        html.push('" />');
        if (response.Source) {
          html.push('<div class="source-text"><span>Source: </span>');
          html.push(response.Source);
          html.push(" (");
          html.push(response.Date);
          html.push(")");
          html.push('</div>');
        }
        html.push('</div>');
      }

      $dialog.html(html.join(""));
    }

    function initGroup($group) {
      if ($group != null && !$group.hasClass("answer-container")) return;

      //var active = $group != null && $(".subtab", $group).val() === "1" ? 1 : 0;
      var active = parseInt($(".subtab", $group).val() || "0");
      $(".answer-sub-tabs", $group).tabs(
      {
        show: 400,
        active: active,
        activate: function(event, ui) {
          //$(".subtab", $(this).closest(".answer-container")).val(ui.newPanel[0].id.indexOf("youtube") >= 0 ? "1" : "0");
          var subtab = 0;
          if (ui.newPanel[0].id.indexOf("youtube") >= 0) subtab = 1;
          else if (ui.newPanel[0].id.indexOf("facebookvideo") >= 0) subtab = 2;
          $(".subtab", $(this).closest(".answer-container")).val(subtab);
        }
      });

      $(".date-picker", $group).datepicker({
        changeYear: true,
        yearRange: "2010:+0"
      });

      $(".action-menu select", $group).on("change", function() {
        doActionMenuChange($(this));
      });

      $("input.remove-line-breaks", $group).on("click", function () {
        var $textarea = $(this).closest(".mc-container").find("textarea.answer-textbox");
        $textarea.val(util.replaceLineBreaksWithSpaces($textarea.val())).change();
      });

      $(".today-button", $group).on("click", function () {
          var today = new Date();
          today = (today.getMonth() + 101).toString().substr(1) + "/" +
          (today.getDate() + 100).toString().substr(1) + "/" +
          today.getFullYear();
          var $this = $(this);
          var target = $this.hasClass("for-youtubedate") ? ".youtubedate-textbox" : ".date-textbox";
          $this.closest(".mc-container").find(target).val(today).change();
        });

      $(".youtubefrom-checkbox", $group).on("change", function () {
        var $checkbox = $(this);
        var $context = $checkbox.closest(".answer-sub-tabs");
        var otherClass = "youtubefromvoteusa-checkbox";
        var hide = true;
        if ($checkbox.hasClass(otherClass)) {
          otherClass = "youtubefromcandidate-checkbox";
          hide = false;
        }
        if ($checkbox.is(":checked")) {
          $context.find("." + otherClass).prop("checked", false);
        } else {
          $checkbox.prop("checked", true);
        }
        $context.find(".youtube-source-fields").toggleClass("hidden", hide);
      });

      //$("input[type=checkbox].kalypto-deferred", $group).addClass("kalypto").kalypto({ toggleClass: "kalypto-checkbox" });
    }

    function saveResponse($container) {
      // make sure the current response is saved in the responses data
      var $sequence = $(".answer-sequence", $container);
      var responses = $sequence.data("responses") || [];
      var sequenceExists = false;
      var sequence = $sequence.val();

      if (sequence === "?") return; // we never save a new entry

      $.each(responses, function () {
        if (this.Sequence == sequence) {
          sequenceExists = true;
          return false;
        }
      });

      if (!sequenceExists) {
        var response = {
          Answer: $(".answer-textbox", $container).val(),
          Source: $(".source-textbox", $container).val(),
          Date: $(".date-textbox", $container).val(),
          YouTubeUrl: $(".youtubeurl-textbox", $container).val(),
          YouTubeDescription: $(".youtubedescription-textbox", $container).val(),
          YouTubeDate: $(".youtubedate-textbox", $container).val(),
          YouTubeSource: $(".youtubesource-textbox", $container).val(),
          YouTubeSourceUrl: $(".youtubesourceurl-textbox", $container).val(),
          YouTubeRunningTime: $(".youtuberunningtime-textbox", $container).val(),
          YouTubeFromCandidate: $(".youtubefromcandidate-checkbox", $container).prop("checked"),
          FacebookVideoUrl: $(".facebookvideourl-textbox", $container).val(),
          FacebookVideoDescription: $(".facebookvideodescription-textbox", $container).val(),
          FacebookVideoRunningTime: $(".facebookvideorunningtime-textbox", $container).val(),
          Sequence: sequence
        };
        // save it
        responses.push(response);
        $sequence.data("responses", responses);
      }
    }

    $(function () {
      init();
    });

    return {
      initGroup: initGroup
    };
  });