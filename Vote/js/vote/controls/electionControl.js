define(["jquery", "vote/util", "slimscroll"],
function ($, util) {

  var options;

  var changeElection = function (newElectionKey) {
    var $key = $(".election-control input.election-key[value=" + newElectionKey + "]");
    if ($key.length === 1) {
      $('.election-control .election-desc.selected').removeClass('selected');
      $key.closest(".election-desc").addClass('selected');
      if (options.onSelectedElectionChanged)
        options.onSelectedElectionChanged(newElectionKey);
    }
  };

  var getSelectedElection = function () {
    var $selected = $(".election-control .election-desc.selected input");
    return $selected.length === 0 ? "" : $selected.val();
  };

  var getElectionDesc = function (electionKey) {
    if (!electionKey) electionKey = getSelectedElection();
    if (!electionKey) return "";
    var $target = $(".election-control input.election-key[value=" + electionKey + "]").parent();
    var $date = $target.prev();
    while (!$date.hasClass("election-date"))
      $date = $date.prev();
    return $date.text() + " - " + $target.text();
  };

  var getElectionKeys = function() {
    var keys = [];
    $(".election-control input").each(function() { keys.push($(this).val()); });
    return keys;
  };

  var hasOtherElectionsOnSameDate = function(electionKey) {
    if (!electionKey) electionKey = getSelectedElection();
    if (!electionKey) return false;
    var $target = $(".election-control input.election-key[value=" + electionKey + "]").parent();
    return $target.prev().hasClass("election-desc") || $target.next().hasClass("election-desc");
  };

  var init = function (opts) {
    // exit if it's already initialized (based on slimScroll initialization)
    if ($('.slimScrollDiv .election-control').length) return;

    options = $.extend({}, opts);
    options.slimScrollOptions = $.extend({}, {
      height: '584px',
      width: '200px',
      alwaysVisible: true,
      color: '#666',
      size: '12px'
    }, options.slimScrollOptions);

    var $electionControl = $('.election-control');
    var selectedElement = null;
    if (options.electionKey) {
      $.each($('.election-key', $electionControl), function () {
        if ($(this).val() === options.electionKey) {
          selectedElement = $(this).parent();
          selectedElement.addClass('selected');
          return false;
        }
      });
    }

    util.safeBind($electionControl, "click", onClickElectionControl);
    util.safeBind($(".select-election-toggler"), "click", onClickElectionToggler);

    while (selectedElement && !selectedElement.hasClass("election-date"))
      selectedElement = selectedElement.prev();
    if (selectedElement)
      options.slimScrollOptions.start = selectedElement;

    $electionControl.slimScroll(options.slimScrollOptions);

    // convert height to max-height, so the window shrinks to fit
    var height = $electionControl.css("height");
    $electionControl.css("height", "").css("max-height", height);
    $electionControl.parent().css("height", "").css("max-height", height);
  };

  var onClickDocument = function(event) {
    // is the control showing?
    if ($('.election-control-slider').css("display") !== "none") {
      // close it if click was outside
      if ($(event.target).closest(".election-control").length === 0)
        toggleElectionControl(false);
    }
  };

  var onClickElectionControl = function (event) {
    var $target = $(event.target);
    if (!$('.election-control-container').hasClass('disabled') &&
        $target.hasClass('election-desc') &&
        !$target.hasClass('selected')) {
      var newElectionKey = $('.election-key', $target).val();
      if (typeof options.onSelectedElectionChanging !== "function" ||
       options.onSelectedElectionChanging(newElectionKey) !== false)
        changeElection(newElectionKey);
    }
  };

  var onClickElectionToggler = function () {
    if (!$(".election-control-container").hasClass("disabled")) {
      toggleElectionControl();
    }
  };

  var toggleElectionControl = function (show) {
    var $slider = $('.election-control-slider');
    var $toggle = $('.select-election-toggler');
    if (show !== true && show !== false)
      show = $slider.css("display") === "none";
    show ? $slider.slideDown() : $slider.slideUp();
    $toggle.toggleClass("showing", show);
    if (show) // delay so we don't do it immediately
      setTimeout(function () 
      { util.safeBind($(document), "click", onClickDocument); }, 100);
    else
      $(document).off("click", onClickDocument);
    if (options.onToggleElectionControl)
      options.onToggleElectionControl(show);
  };

  return {
    changeElection: changeElection,
    getElectionDesc: getElectionDesc,
    getElectionKeys: getElectionKeys,
    getSelectedElection: getSelectedElection,
    hasOtherElectionsOnSameDate: hasOtherElectionsOnSameDate,
    init: init,
    toggleElectionControl: toggleElectionControl
  };
});