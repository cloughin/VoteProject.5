define(["jquery"], function ($) {
  $(function () {
    $(".expandable-label").on("click", function () {
      var $this = $(this);
      if ($this.hasClass("office-contests-expandable"))
        $(".office-contests-year").toggle();
      else if ($this.hasClass("elections-expandable"))
        $(".elections-year").toggle();
      else if ($this.hasClass("office-candidates-expandable"))
        $(".office-candidates-year").toggle();
    });
  });
});