define(["jquery", "vote/adminMaster", "vote/util"],
function ($, master, util) {

  var queryPaused = false;
  var queryPausePending = false;

  function doQuery() {
    var $tr = $(".youtube-table tbody tr:not(.done)").slice(0, 1);
    if (!$tr.length) {
      $(".pause-button").prop("disabled", true);
      return;
    }
    var key = $tr.data("key");
    util.ajax({
      url: "/Admin/WebService.asmx/GetYouTubeVideoChannel",
      data: {
        politicianKey: key
      },

      success: function (result) {
        var info = result.d;
        if (info.IsValid && info.IsPublic) {
          var $td = $("td.channel-url", $tr);

          var url = "www.youtube.com/channel/" + info.Id;
          var html = '<div title="' + url + '"><a target="view" href="https://' +
            url + '">' + info.Id + '</a></div>';
          $td.html(html);

          $td = $("td.channel-title", $tr);
          html = '<div title="' + util.htmlEscapeAttribute(info.Title) + '">' +
            util.htmlEscapeText(info.Title) + '</div>';
          $td.html(html);

          $td = $("td.channel-desc", $tr);
          html = '<div title="' + util.htmlEscapeAttribute(info.ShortDescription) + '">' +
            util.htmlEscapeText(info.ShortDescription) + '</div>';
          $td.html(html);

          $td = $("td.url-to-use", $tr);
          html = '<div><input type="button" value="Video" class="video-button button-1 button-smallest"/>&nbsp;' +
            '<input type="button" value="Channel" class="channel-button button-1 button-smallest"/></div>';
          $td.html(html);

        } else {
          var error = info.IsValid ? "The channel is not public" : "The channel could not be found on YouTube";

          $td = $("td.channel-desc", $tr);
          html = '<div class="error">' + error + '</div>';
          $td.html(html);
        }
      },

      error: function (result) {
        var $td = $("td.channel-desc", $tr);
        var html = '<div class="error">' + util.formatAjaxError(result,
                "Could not get the channel info from YouTube") + '</div>';
        $td.html(html);
      },

      complete: function () {
        $tr.addClass("done");
        if (queryPausePending) queryPaused = true;
        else doQuery();
      }
    });
  }

  function channelButtonClicked() {
    var $tr = $(this).closest("tr");
    var $div = $(".url-to-use>div", $tr);
    var channelId = $(".channel-url>div a", $tr).text();
    $div.addClass("info").html('Updating...');
    var key = $tr.data("key");
    util.ajax({
      url: "/Admin/WebService.asmx/YouTubeUseChannel",
      data: {
        politicianKey: key,
        channelId: channelId
      },
      
      success: function() {
        $div.html('Channel is being used');
      },
      
      error: function(result) {
        $div.removeClass("info").addClass("error").html(util.formatAjaxError(result,
          "An update error occurred"));
      }
    });
  }

  function doVideoButtonClicked(button, all) {
    var $tr = $(button).closest("tr");
    var $div = $(".url-to-use>div", $tr);
    $div.addClass("info").html('Updating...');
    var key = $tr.data("key");
    util.ajax({
      url: "/Admin/WebService.asmx/YouTubeUseVideo",
      data: {
        politicianKey: key
      },

      success: function () {
        $div.html('Video is being used');
      },

      error: function (result) {
        $div.removeClass("info").addClass("error").html(util.formatAjaxError(result,
          "An update error occurred"));
      },
      
      complete: function() {
        if (all) allVideosClicked(true);
      }
    });
  }

  function videoButtonClicked() {
    doVideoButtonClicked(this);
  }
  
  function allVideosClicked(force) {
    if (!force) {
      util.confirm("Are you sure you want to use the video for all remaining entries?",
        function(button) {
          if (button === "Ok") {
            $(".all-videos-button").prop("disabled", true);
            allVideosClicked(true);
          }
        });
      return;
    }
    var $videoButtons = $("input.video-button");
    if ($videoButtons.length)
      doVideoButtonClicked($videoButtons[0], true);
  }

  function initPage() {
    doQuery();
    $(".pause-button").click(function () {
      var $this = $(this);
      queryPausePending = !queryPausePending;
      $this.val(queryPausePending ? "Resume Queries" : "Pause Queries");
      if (!queryPausePending && queryPaused) {
        queryPaused = false;
        doQuery();
      }
    });
    $("body")
      .on("click", ".video-button", videoButtonClicked)
      .on("click", ".channel-button", channelButtonClicked)
      .on("click", ".all-videos-button", function() { allVideosClicked(false); });
  }

  master.inititializePage({
    callback: initPage
  });
});