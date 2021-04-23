function onChangeSelect()
{
  var select = $(".stateLinks select");
  if (!setStateLinksAjaxActive(true))
  {
    var stateCode = select.val();
    $.ajax({
      type: "POST",
      url: "/WebService.asmx/GetStateLinks",
      data: "{'input': '" + stateCode + "'}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: getStateLinksSucceeded,
      error: getStateLinksFailed
    });
  }
  select.blur();
}

function getStateLinksSucceeded(result)
{
  setStateLinksAjaxActive(false);
  $(".stateLinks .links").html(result.d);
}

function getStateLinksFailed(result)
{
  setStateLinksAjaxActive(false);
  alert(result.status + ' ' + result.statusText);
}

function setStateLinksAjaxActive(newState)
{
  var currentState = $.globals.stateLinksAjaxState ? true : false;
  if (newState !== currentState)
  {
    $.globals.stateLinksAjaxState = newState;
    var visibility = newState ? "visible" : "hidden";
    $('.stateLinks img.ajaxLoader').css("visibility", visibility);
  }
  return currentState;
}

$(function ()
{
  $(".stateLinks select").change(onChangeSelect);
});
