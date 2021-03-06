define(["jquery", "vote/util", "jqueryui", "tiptip", "kalypto", "slicknav"], function ($, util) {

  var $$ = util.$$;

  // P R I V A T E

  // ReSharper disable InconsistentNaming
  var initializeRequest_;
  var endRequest_;
  // ReSharper restore InconsistentNaming

  function applicationInit() {
    // ReSharper disable UseOfImplicitGlobalInFunctionScope
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    // ReSharper restore UseOfImplicitGlobalInFunctionScope
    prm.add_initializeRequest(initializeRequest);
    prm.add_endRequest(endRequest);
  }

  function initializeRequest(sender, args) {
    if (typeof initializeRequest_ === "function")
      initializeRequest_(sender, args);
  }

  function endRequest(sender, args) {
    if (typeof endRequest_ === "function")
      endRequest_(sender, args);
  }

  // P U B L I C
  
  function initDisclaimerButtons($context) {
    $(".disclaimer-button", $context).safeBind("click", function () {
      var judicial = $(this).hasClass("judicial");
      var $textarea = $(".electionadditionalinfo textarea", $context);
      $textarea.val(judicial
         ? "In addition to the office contests shown on our ballots, you may find additional county, city, local and judicial contests on the ballot you will see at the polls. Due to our limited resources we are currently unable to cover these contests."
         : "In addition to the office contests shown on our ballots, you may find additional county, city and local contests on the ballot you will see at the polls. Due to our limited resources we are currently unable to cover these contests.");
      $textarea.change();
    });
    $(".disclaimer-clear-button", $context).safeBind("click", function () {
      $(".electionadditionalinfo textarea", $context).val("").change();
    });
  }

  var inititializePage = function (options) {
    options = options || {};
    initializeRequest_ = options.initializeRequest;
    endRequest_ = options.endRequest;

    var isIe11 = Object.hasOwnProperty.call(window, "ActiveXObject") && !window.ActiveXObject;

    if (isIe11 || Function('/*@cc_on return document.documentMode===10@*/')()) {
      document.documentElement.className += ' ie10';
    }

    if (isIe11)
      document.documentElement.className += ' ie11';

    $('.main-admin-menu').slicknav({ appendTo: ".slicknav-container" });

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

//    $("body").on("keypress", ".databox.timespan input", function(event) {
//      return event.which >= 48 && event.which <= 57;
//    });

    // we can start jQuery elements hidden so they don't display uninitialized content, 
    // then show them here after fully loaded and formatted
    $(".start-hidden").show();
    $("input[type=checkbox].kalypto").kalypto({ toggleClass: "kalypto-checkbox" });
    $("input[type=radio].kalypto").kalypto({ toggleClass: "kalypto-radio" });
    // following for asp checkbox lists
    $("table.kalypto input[type=checkbox]").kalypto({ toggleClass: "kalypto-checkbox" });

    if (typeof options.callback === "function")
      options.callback();

    util.initTipTip($(".tiptip")); // run tiptip after user init code runs
    $(".slo-load").hide(0);
    util.aspKeepAlive();
  };

  // Custom open and close handlers for jquery dialogs

  var onOpenJqDialog = function () {
    // eliminate unneeded scrollbars in IE
    $('body').css('overflow', 'hidden');
    $('.ui-widget-overlay').css('width', '100%');
    // move the dialog back inside the form so postbacks work right
    $(this).parent().appendTo($("form:first")).css("z-index", "200");
    $(this).css('overflow', 'hidden'); // needed for firefox after moving the form
  };

  var onCloseJqDialog = function () {
    // restore state 
    $('body').css('overflow', 'auto');
  };

  function parseFragment(defaultTab, callback) {
    var info = { tab: defaultTab || "", subTab: "" };
    var frag = window.location.hash.substr(1).toLowerCase();
    if (frag.slice(0, 4) === "tab-") frag = frag.slice(4);
    frag = frag.split("-");
    if (frag[0]) info.tab = "tab-" + frag[0];
    if (frag[1] && info.tab) info.subTab = info.tab + "-" + frag[1];
    if (!callback || callback(info) !== false)
      $$('main-tabs').tabs("option", "active", util.getTabIndex("main-tabs", info.tab));
    return info;
  }

  // I N I T I A L I Z E

  // Hook up Application event handlers.
  // ReSharper disable once UseOfImplicitGlobalInFunctionScope
  var app = Sys.Application;
  app.add_init(applicationInit);

  return {
    initDisclaimerButtons: initDisclaimerButtons,
    inititializePage: inititializePage,
    onOpenJqDialog: onOpenJqDialog,
    onCloseJqDialog: onCloseJqDialog,
    parseFragment: parseFragment
  };

});