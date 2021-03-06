// Custom open and close handlers for jquery dialogs

function onOpenJqDialog(event, ui) 
{
  // eliminate unneeded scrollbars in IE
  $('body').css('overflow', 'hidden');
  $('.ui-widget-overlay').css('width', '100%');
  // move the dialog back inside the form so postbacks work right
  // $(this).parent().appendTo('/html/body/form[0]');
  $(this).parent().appendTo('form');
  $(this).css('overflow', 'hidden'); // needed for firefox after moving the form
}

function onCloseJqDialog(event, ui)
{
  // restore state 
  $('body').css('overflow', 'auto');
}

function initHoverChildren()
{
  $(".hoverChild").hover(
      function (event) { $(this).parent().addClass("hovering"); },
      function (event) { $(this).parent().removeClass("hovering"); });
}

$(function ()
{
  initHoverChildren();
});

// To handle a strange situation
function getLastValue(elements) {
  for (var n = elements.length - 1; n >= 0; n--) {
    var value = $.trim($(elements[n]).val());
    if (value) return value;
  }
  return '';
}
