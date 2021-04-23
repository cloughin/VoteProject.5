;window.UTIL=(function($) {

  // for parsing the query string (static)
  $.extend({
    queryString: function (name) {
      if (name) {
        return $.queryString()[name.toLowerCase()];
      } else {
        var vars = [], hash;
        // remove fragment
        var href = window.location.href.split('#')[0];
        var hashes = href.slice(href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
          hash = hashes[i].split('=');
          hash[0] = hash[0].toLocaleLowerCase();
          vars.push(hash[0]);
          vars[hash[0]] = hash[1];
        }
        return vars;
      }
    }
  });

  // Alerts - AlertDialog

  // A replacement for the plain js alert and confirm
  //
  // arguments:
  // message: must be the first, type string
  // dialog title: a subsequent string argument is used as the dialog title
  // buttons: an array argument
  // callback: a function argument, called when dialog closed, 
  //   passed the label of the clicked button or "esc" if not closed
  //   via button click
  // the buttons argument: array of string giveng the button labels -- each
  //   label optionally followed by a number used to form a classname of
  //   "button-<number>" (default 1).
  //   example: ["Ok", "Cancel", 3] makes two buttons:
  //     "Ok" classname button-1
  //     "Cancel" classname button-3

  var $alertDialog;
  var $alertDialogButtons;
  var alertDialogCallback = null;
  var alertDialogInitialized = false;

  var initAlertDialog = function() {
    if (alertDialogInitialized) return;
    $('body').append('<div id="alert-dialog" class="hidden"><div class="inner">' +
      '<div class="message"></div><div class="bottom-box button-box">' +
      '<input type="button" class="alert-button-1 button-1 button-smallest"/>' +
      '<input type="button" class="alert-button-2 button-1 button-smallest"/>' +
      '<input type="button" class="alert-button-3 button-1 button-smallest"/>' +
      '<input type="button" class="alert-button-4 button-1 button-smallest"/>' +
      '</div></div></div>');
    $alertDialog = $('#alert-dialog').dialog({
      autoOpen: false,
      close: onAlertDialogClose,
      dialogClass: "alert-dialog",
      modal: true,
      resizable: false,
      width: "auto"
    });
    $alertDialogButtons = $('input[type=button]', $alertDialog)
      .on("click", function(event) {
        var fn = null;
        if (alertDialogCallback) {
          fn = alertDialogCallback;
          alertDialogCallback = null;
        }
        $alertDialog.dialog("close");
        // ReSharper disable once InvokedExpressionMaybeNonFunction
        if (fn) fn($(event.target).val());
      });
    alertDialogInitialized = true;
  };

  var alert = function() {
    initAlertDialog();
    var ax = 0;
    var message = "Message";
    var title = "Alert";
    var callback = null;
    var buttons = ["Ok"];
    if (arguments.length > 0 && typeof arguments[0] === "string") {
      message = arguments[ax++];
    }
    for (var i = ax; i < arguments.length; i++) {
      switch (typeof arguments[i]) {
      case "string":
        title = arguments[i];
        break;

      case "function":
        callback = arguments[i];
        break;

      default:
        if ($.isArray(arguments[i]))
          buttons = arguments[i];
        break;
      }
    }
    i = 0;
    while (i < buttons.length) {
      var buttonTypeInx = i + 1;
      if (buttonTypeInx === buttons.length ||
        (typeof buttons[buttonTypeInx] !== "number")) {
        buttons.splice(buttonTypeInx, 0, 1);
      }
      i += 2;
    }
    $alertDialog.dialog("option", "title", htmlEscape(title));
    $('.message', $alertDialog).html(htmlEscape(message).replace(/\n/g, "<br />"));
    alertDialogCallback = callback;
    $alertDialogButtons.hide().removeClass("button-1 button-2 button-3");
    for (i = 0; i < buttons.length; i += 2) {
      var j = i / 2;
      $($alertDialogButtons[j])
        .show()
        .val(buttons[i])
        .addClass("button-" + buttons[i + 1]);
    }
    $alertDialog.dialog("open");
    //moveDialogToTop("alert-dialog");
  };

  var confirm = function() {
    var args = [].slice.call(arguments);
    args.push(["Ok", "Cancel", 3]);
    var strings = 0;
    $.each(args, function(inx) {
      if (typeof args[inx] === "string") strings++;
    });
    if (strings < 2) args.push("Confirm");
    alert.apply(this, args);
  };

  var onAlertDialogClose = function() {
    if (alertDialogCallback) {
      var fn = alertDialogCallback;
      alertDialogCallback = null;
      fn("esc");
    }
  };

  // P U B L I C

  function htmlEscape(str) {
    return String(str)
      .replace(/&/g, '&amp;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/\u2018/g, "&lsquo;")
      .replace(/\u2019/g, "&rsquo;")
      .replace(/\u201C/g, "&ldquo;")
      .replace(/\u201D/g, "&rdquo;")
      .replace(/\u00AE/g, "&reg;")
      .replace(/\u2122/g, "&trade;")
      .replace(/\u2013/g, "&ndash;")
      .replace(/\u2014/g, "&mdash;");
  };

  var modalInitialized = false;
  var $modalOverlay;
  var $modalContent;
  
  function centerModal() {
    var top, left;

    top = Math.max($(window).height() - $modalContent.outerHeight(), 0) / 2;
    left = Math.max($(window).width() - $modalContent.outerWidth(), 0) / 2;

    $modalContent.css({
      top: top + $(window).scrollTop(),
      left: left + $(window).scrollLeft()
    });
  }
  
  function initModal() {
    if (!modalInitialized) {
      $modalOverlay = $('<div id="ajax-overlay"></div>');
      $modalContent = $('<div id="ajax-overlay-content"><img src="/images/ajax-loader64.gif"/></div>');
      $modalOverlay.hide();
      $modalContent.hide();
      $("body").append($modalOverlay, $modalContent);
    }
  }
  
  function openModal() {
    initModal();

    $modalContent.css({
      width: 'auto',
      height: 'auto'
    });

    centerModal();

    $(window).bind('resize.modal', centerModal);

    $modalContent.show();
    $modalOverlay.show();
  }
  
  function closeModal() {
    $modalContent.hide();
    $modalOverlay.hide();
    $(window).unbind('resize.modal');
  }


  var isValidEmailRegex = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
  function isValidEmail(email) {
    return !!email.match(isValidEmailRegex);
  };

  // I N I T I A L I Z E

  return {
    alert: alert,
    closeModal: closeModal,
    confirm: confirm,
    htmlEscape: htmlEscape,
    isValidEmail: isValidEmail,
    openModal: openModal
  };

})(jQuery);