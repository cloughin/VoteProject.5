function onOpenJqDialog(/*event, ui*/) {
  $('body').css('overflow', 'hidden');
  $('.ui-widget-overlay').css('width', '100%');
  $(this).parent().appendTo('form');
  $(this).css('overflow', 'hidden');
}

function onCloseJqDialog(/*event, ui*/) { $('body').css('overflow', 'auto'); }

function initHoverChildren() {
  $(".hoverChild")
    .hover(function(/*event*/) { $(this).parent().addClass("hovering"); },
      function (/*event*/) { $(this).parent().removeClass("hovering"); });
}

$(function() { initHoverChildren(); });
