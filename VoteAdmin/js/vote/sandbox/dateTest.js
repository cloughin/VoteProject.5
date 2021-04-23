define(["jquery", "vote/util", "moment"], function ($, util, moment) {

  $(function () {
    $(".localdate").each(function() {
      try {
        var $this = $(this);
        var date = new Date(parseInt($this.attr("ticks")) / 10000).toString();
        var format = $this.attr("format");
        var str;
        if (format) str = moment(date).format(format);
        else str = date.toString();
        $this.html(str);
      } catch (ex) {
      }
    });
  });

});