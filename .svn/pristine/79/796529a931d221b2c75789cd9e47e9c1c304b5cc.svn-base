var is_ie;

function getCurrentTabPanel(tabsId) {
  return $("#" + tabsId + " .main-tab.ui-tabs-panel[aria-hidden='false']");
}

function getCurrentTabId(tabsId) {
  return $("#" + tabsId + " .main-tab.ui-tabs-panel[aria-hidden='false']").attr("id");
}

function onInitAdmin() {
  $(function () {
    if (Function('/*@cc_on return document.documentMode===10@*/')()) {
      document.documentElement.className += ' ie10';
      if (typeof is_ie != "undefined") is_ie = true;
    }

    $('.main-admin-menu').menu();

    if (Modernizr.input.placeholder)
      $('.no-ph').removeClass('no-ph').addClass('has-ph');

    $(".jqueryui-tabs").tabs(
      {
        show: 400
      });

    $(".accordion").accordion(
    {
      heightStyle: "content",
      active: false,
      collapsible: true
    });

    // we can start jQuery elements hidden so they don't display uninitialized content, 
    // then show them here after fully loaded and formatted
    $(".start-hidden").show();
    $("input[type=checkbox].kalypto").kalypto({ toggleClass: "kalypto-checkbox" });
    // following for asp checkbox lists
    $("table.kalypto input[type=checkbox]").kalypto({ toggleClass: "kalypto-checkbox" });

    if (typeof initPage == "function")
      initPage();

    initTipTip($(".tiptip")); // run tiptip after user init code runs
    $(".slo-load").hide(0);
  });
}

function initTipTip($o) {
  $o.tipTip({ edgeOffset:10 });
}

// Hook up Application event handlers.
var app = Sys.Application;
//app.add_load(ApplicationLoad);
app.add_init(ApplicationInit);

function ApplicationInit(sender) {
// ReSharper disable UseOfImplicitGlobalInFunctionScope
  var prm = Sys.WebForms.PageRequestManager.getInstance();
// ReSharper restore UseOfImplicitGlobalInFunctionScope
  prm.add_initializeRequest(InitializeRequest);
  prm.add_endRequest(EndRequest);
}

function InitializeRequest(sender, args) {
  if (typeof initializeRequest == "function")
    initializeRequest(sender, args);
}

function EndRequest(sender, args) {
  if (typeof endRequest == "function")
    endRequest(sender, args);
}

var util = {};

util.getClasses = function(o) {
  return $(o).attr("class").split(/\s+/);
};

util.getPrefixedClass = function(o, prefix, removePrefix) {
  var result;
  $.each(util.getClasses(0), function(index, className) {
    if (className.startsWith(prefix)) {
      if (removePrefix) result = className.substr(prefix.length);
      else result = className;
      return false;
    }
  });
  return result;
};

util.stopPropagation = function(event) {
  event.stopPropagation();
};

util.tabClick = function(element) {
  $('a', element).unbind('click', util.stopPropagation).click(util.stopPropagation).click();
};

util.updateNocacheUrl = function(url) {
  var re = /[&?]x=\d+/i;
  var m = re.exec(url);
  if (m) url = url.substr(0, m.index + 3) + Date.now() + url.substr(m.index + m[0].length);
  return url;
};

var $$ = function(idSelector) {
  return $("#" + idSelector);
};

// Custom open and close handlers for jquery dialogs

function onOpenJqDialog(event, ui) {
  // eliminate unneeded scrollbars in IE
  $('body').css('overflow', 'hidden');
  $('.ui-widget-overlay').css('width', '100%');
  // move the dialog back inside the form so postbacks work righ
  $(this).parent().appendTo($("form:first")).css("z-index", "200");
  $(this).css('overflow', 'hidden'); // needed for firefox after moving the form
}

function onCloseJqDialog(event, ui) {
  // restore state 
  $('body').css('overflow', 'auto');
}
