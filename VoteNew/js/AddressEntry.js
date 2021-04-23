// JavaScript for the Address Entry dialog 

function initAddressEntryDialog()
{
  $('#addressEntryDialog').dialog({
    autoOpen: false,
    width: 708,
    height: 367,
    resizable: false,
    // custom open and close to fix various problems
    open: openAddressEntryDialog,
    close: closeJqDialog,
    modal: true
  });
}

function openAddressEntryDialog()
{
  openJqDialog.apply(this);
  $('#addressEntryDialog .ajaxMessage').hide();
}

function initAddressEntryAddressToFind()
{
  // enter in this input box triggers button click
  $('#addressEntryDialog .addressToFind').keyup(
    function (evt)
    {
      $('#addressEntryDialog .ajaxMessage').hide();
      var keyCode = evt ? (evt.which ? evt.which : evt.keyCode) : event.keyCode;
      if (keyCode == 13)
        addressEntryFindAddress();
    });
}

function initAddressEntryFindAddressButton()
{
  $("#addressEntryDialog .address .find").click(addressEntryFindAddress);
}

function initAddressEntryFindStateButton()
{
  $("#addressEntryDialog .state .find").click(addressEntryFindState);
}

function setAddressEntryAjaxActive(newState)
{
  var currentState = $.globals.ajaxState ? true : false;
  if (newState != currentState)
  {
    $.globals.ajaxState = newState;
    var visibility = newState ? "visible" : "hidden";
    $('#addressEntryDialog img.ajaxLoader').css("visibility", visibility);
  }
  return currentState;
}

function addressEntryFindAddress()
{
  var input = $.trim($('#addressEntryDialog .addressToFind').val());
  if (input && !setAddressEntryAjaxActive(true))
  {
    $('#addressEntryDialog .ajaxMessage').hide();
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/FindAddress",
      data: "{'input': '" + input + "'}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: addressEntryFindAddressSucceeded,
      error: addressEntryFindAddressFailed
    });
  }
}

function addressEntryFindState()
{
  var input = $('#addressEntryDialog .selectState').val();
  if (input && !setAddressEntryAjaxActive(true))
  {
    //$('#addressEntryDialog .ajaxMessage').hide();
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/FindState",
      data: "{'input': '" + input + "'}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: addressEntryFindStateSucceeded,
      error: addressEntryFindStateFailed
    });
  }
}

function showAddressEntryAjaxMessage(html, cssClass)
{
  var o = $('#addressEntryDialog .ajaxMessage');
  o.removeClass('ajaxSuccess ajaxFailure');
  o.addClass(cssClass);
  o.html(html);
  o.show();
}

function showStateAjaxMessage(html, cssClass)
{
  var o = $('#addressEntryDialog .ajaxMessage2');
  o.removeClass('ajaxSuccess ajaxFailure');
  o.addClass(cssClass);
  o.html(html);
  o.show();
}

function addressEntryFindAddressSucceeded(result)
{
  setAddressEntryAjaxActive(false);
  var addressFinderResult = result.d;
  if (addressFinderResult.SuccessMessage)
  {
    showAddressEntryAjaxMessage(addressFinderResult.SuccessMessage, "ajaxSuccess");
    document.location.href = addressFinderResult.RedirectUrl;
  }
  else
  {
    // only show up to 3 messages *****
    //var messages = addressFinderResult.ErrorMessages.slice(0, 3);
    var messages = addressFinderResult.ErrorMessages;
    showAddressEntryAjaxMessage(messages.join('<br />'), "ajaxFailure");
  }
}

function addressEntryFindAddressFailed(result)
{
  setAddressEntryAjaxActive(false);
  alert(result.status + ' ' + result.statusText);
}

function addressEntryFindStateSucceeded(result)
{
  setAddressEntryAjaxActive(false);
  var addressFinderResult = result.d;
  if (addressFinderResult.SuccessMessage)
  {
    showStateAjaxMessage(addressFinderResult.SuccessMessage, "ajaxSuccess");
    document.location.href = addressFinderResult.RedirectUrl;
  }
}

function addressEntryFindStateFailed(result)
{
  setAddressEntryAjaxActive(false);
  alert(result.status + ' ' + result.statusText);
}

if (typeof ($.globals) == "undefined")
  $.globals = {}; // create global namespace

// Initialize when ready
$(function ()
{
  initAddressEntryDialog();
  initAddressEntryAddressToFind();
  initAddressEntryFindAddressButton();
  initAddressEntryFindStateButton();
});
