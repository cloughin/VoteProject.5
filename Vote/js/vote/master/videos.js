define(["jquery", "vote/adminMaster", "vote/util"],
function ($, master, util) {

  var savedState;

  function doRemove($button, force) {
    var $videosList = $(".videos-list", $button.closest(".col"));
    var isAllVideos = $videosList.hasClass("all-videos");
    var $videoToRemove = $("p.selected", $videosList);
    var id = $videoToRemove.data("id");
    if (isAllVideos) {
      if (force) {
        unpopulateData();
        // remove everywhere
        $(".videos-list p").each(function() {
          if ($(this).data("id") == id)
            $(this).remove();
        });
        $button.addClass("disabled");
        stateChanged();
      } else {
        util.confirm("Are you sure you want to completely remove this video?", function(button) {
          if (button === "Ok")
            doRemove($button, true);
        });
      }
    } else {
      $videoToRemove.remove();
      $button.addClass("disabled");
      stateChanged();
    }
  }

  function getState() {
    var obj = {};
    // get all videos
    obj.all = [];
    $(".all-videos p").each(function() {
      var $this = $(this);
      obj.all.push({
        title: $this.text(),
        id: $this.data("id"),
        url: $this.data("url"),
        description: $this.data("description"),
        embedcode: $this.data("embedcode"),
      });
    });
    obj.admin = [];
    $(".admin-videos p").each(function() {
      obj.admin.push($(this).data("id"));
    });
    obj.volunteers = [];
    $(".volunteer-videos p").each(function() {
      obj.volunteers.push($(this).data("id"));
    });
    return JSON.stringify(obj);
  }
  
  function initPage() {
    $(".all-videos p").draggable({
      addClasses: false,
      helper: function () {
        return $(this).clone().removeClass("selected");
      }
    });
    $(".video-drop-target").droppable(
      {
        accept: ".all-videos p",
        addClasses: false,
        hoverClass: "drop-hover",
        tolerance: "pointer",
        drop: function (event, ui) {
          var id = ui.draggable.data("id");
          var $this = $(this);
          // make sure its not a duplicate
          var dup = false;
          $("p", $this).each(function () {
            if ($(this).data("id") === id) {
              dup = true;
              return false;
            }
          });
          if (dup) return;
          $this.append(ui.draggable.clone().removeClass("selected")
            .removeAttr("data-description data-embedcode data-url"));
          stateChanged();
        }
      }).sortable({
        axis: "y"
      });
    $(".add-button").click(onAdd);
    $(".update-button").click(onUpdate);
    $("body")
      .on("videoSelected", ".videos-list p", onVideoSelected)
      .on("videoUnselected", ".videos-list p", onVideoUnselected)
      .on("click", ".videos-list p", onSelectVideo)
      .on("click", ".remove-button", onRemove)
      .on("propertychange change click keyup input paste", ".data-item", onDataChanged);
    savedState = getState();

    window.onbeforeunload = function() {
      if (getState() != savedState)
        return "There are entries on your form that have not been submitted";
    };
  }

  function onAdd() {
    // find the highest id
    var id = 0;
    $(".all-videos p").each(function() {
      id = Math.max(id, $(this).data("id"));
    });
    var $newVideo = $("<p>", {
       text: "<new video>", 
       "data-id": id + 1,
       "data-description": "",
       "data-embedcode": "",
       "data-url": ""
    }).draggable({
      addClasses: false,
      helper: function () {
        return $(this).clone().removeClass("selected");
      }
    });
    $(".all-videos").append($newVideo);
    onSelectVideo.call($newVideo[0]);
    $(".data-title").select();
    stateChanged();
  }

  function onDataChanged() {
    var $this = $(this);
    // get selected video
    var $video = $(".all-videos p.selected");
    if ($video.length === 1) {
      var type = $this.data("type");
      var val = $this.val();
      if (type === "title") {
        var id = $video.data("id");
        // update title everywhere
        $(".videos-list p").each(function () {
          if ($(this).data("id") == id)
            $(this).text(val);
        });
      } else {
        $video.data(type, val);
      }
    }
    stateChanged();
  }

  function onRemove() {
    doRemove($(this), false);
  }

  function onSelectVideo() {
    var $this = $(this);
    if ($this.hasClass("selected")) {
      $this.removeClass("selected").trigger("videoUnselected");
    } else {
      $this.closest(".videos-list").find("p.selected").removeClass("selected").trigger("videoUnselected");
      $this.addClass("selected").trigger("videoSelected");
    }
  }
  
  function onUpdate() {
    util.openAjaxDialog("Updating...");
    var state = getState();
    util.ajax({
      url: "/Admin/WebService.asmx/UpdateInstructionalVideos",
      data: {
        json: state
      },

      success: function () {
        savedState = state;
        stateChanged();
        util.alert("Successfully updated.");
      },

      error: function (result) {
        util.alert(util.formatAjaxError(result, "Could not update"));
      },
        
      complete: function() {
        util.closeAjaxDialog();
      }
    });
  }

  function onVideoSelected() {
    var $this = $(this);
    var $videosList = $this.closest(".videos-list");
    if ($videosList.length) {
      // enable remove
      $(".remove-button", $videosList.closest(".col")).removeClass("disabled");
    }
    var $allVideos = $this.closest(".all-videos");
    if ($allVideos.length) {
      // populate data
      $(".data-title").val($this.text()).prop("disabled", false);
      $(".data-url").val($this.data("url")).prop("disabled", false);
      $(".data-description").val($this.data("description")).prop("disabled", false);
      $(".data-embedcode").val($this.data("embedcode")).prop("disabled", false);
    }
  }

  function onVideoUnselected() {
    var $this = $(this);
    var $videosList = $this.closest(".videos-list");
    if ($videosList.length) {
      $(".remove-button", $videosList.closest(".col")).addClass("disabled");
    }
    var $allVideos = $this.closest(".all-videos");
    if ($allVideos.length) {
      unpopulateData();
    }
  }
  
  function stateChanged() {
    $(".update-button").prop("disabled", getState() === savedState);
  }

  function unpopulateData() {
    $(".data-title").val("").prop("disabled", true);
    $(".data-url").val("").prop("disabled", true);
    $(".data-description").val("").prop("disabled", true);
    $(".data-embedcode").val("").prop("disabled", true);
  }
  
  master.inititializePage({
    callback: initPage
  });
});
