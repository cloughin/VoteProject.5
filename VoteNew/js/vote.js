// Custom open and close handlers for jquery dialogs

function openJqDialog(event, ui) 
{
  // eliminate unneeded scrollbars in IE
  $('body').css('overflow', 'hidden');
  $('.ui-widget-overlay').css('width', '100%');
  // move the dialog back inside the form so postbacks work right
  $(this).parent().appendTo('/html/body/form[0]');
  $(this).css('overflow', 'hidden'); // needed for firefox after moving the form
}

function closeJqDialog(event, ui)
{
  // restore state 
  $('body').css('overflow', 'auto');
}
