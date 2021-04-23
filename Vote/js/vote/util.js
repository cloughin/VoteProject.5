define(["jquery", "moment", "jqueryui", "tiptip", "stupidtable"], 
function($, moment) {

  // J Q U E R Y   P L U G - I N S

// ReSharper disable UseOfImplicitGlobalInFunctionScope
  var modernizr = typeof Modernizr === "undefined" ? null : Modernizr;
// ReSharper restore UseOfImplicitGlobalInFunctionScope

  // placeholder shim
  (function () {
    $.fn.placeholder = function() {
      required(modernizr, "modernizr");
      if (!modernizr.input.placeholder) {
        this.focus(function() {
          var input = $(this);
          if (input.val() == input.attr('placeholder')) {
            input.val('');
            input.removeClass('placeholder');
          }
        }).blur(function() {
          var input = $(this);
          if (input.val() == '' || input.val() == input.attr('placeholder')) {
            input.addClass('placeholder');
            input.val(input.attr('placeholder'));
          }
        }).blur();
        this.parents('form').submit(function() {
          $(this).find('[placeholder]').each(function() {
            var input = $(this);
            if (input.val() == input.attr('placeholder')) {
              input.val('');
            }
          });
        });
      }
    };
  })();

  // appends @2x to src name if on retina display and explicit size
  (function () {
    $.fn.retina = function() {
      if (typeof(window.devicePixelRatio) === "number" &&
        window.devicePixelRatio >= 2)
        this.each(function() {
          var src = this.src;
          if (typeof(src) === "string" && (!!this.width || !!this.height)) {
            var pos = src.lastIndexOf(".");
            if (pos >= 0)
              this.src = src.substr(0, pos) + "@2x" + src.substr(pos);
          }
        });
    };
  })();

  // Iterates over all class names
  (function() {
    $.fn.classes = function(callback) {
      var classes = [];
      $.each(this, function() {
        var splitClassName = this.className.split(/\s+/);
        for (var j in splitClassName) {
          var className = splitClassName[j];
          if (classes.indexOf(className) === -1) {
            classes.push(className);
          }
        }
      });
      if (typeof callback === 'function') {
        for (var i in classes) {
          if (callback(classes[i]) === false)
            break;
        }
      }
      return classes;
    };
  })();

  // unbinds before binding
  (function() {
    $.fn.safeBind = function(event, handler) {
      safeBind(this, event, handler);
      return this;
    };
  })();

  // escapes the html
  (function () {
    $.fn.safeHtml = function (html) {
      this.html(htmlEscape(html));
      return this;
    };
  })();

  // val that handles checkboxes and radios
  // read only for now
  (function () {
    $.fn.valEx = function () {
      if (this.length === 0) return null;
      switch (this[0].type) {
        case "checkbox":
        case "radio":
          return this.prop("checked").toString();

        default:
          return this.val();
      }
    };
  })();

  // scrollTo extension
  (function() {
    $.fn.scrollTo = function(target, options, callback) {
      if (typeof options == 'function' && arguments.length == 2) {
        callback = options;
        options = target;
      }
      var settings = $.extend({
        scrollTarget: target,
        offsetTop: 50,
        duration: 500,
        easing: 'swing'
      }, options);
      return this.each(function() {
        var scrollPane = $(this);
        var scrollTarget = (typeof settings.scrollTarget == "number") ? settings.scrollTarget : $(settings.scrollTarget);
        var scrollY = (typeof scrollTarget == "number") ? scrollTarget : scrollTarget.offset().top + scrollPane.scrollTop() - parseInt(settings.offsetTop);
        scrollPane.animate({ scrollTop: scrollY }, parseInt(settings.duration), settings.easing, function() {
          if (typeof callback == 'function') {
            callback.call(this);
          }
        });
      });
    };
  })();

  // for parsing the query string (static)
  $.extend({
    queryString: function(name) {
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

  // P R I V A T E

  var entities = {
    // Reserved HTML Characters
    reservedHtml: [
      { number: "&#38;", name: "&amp;", unicode: "\u0026", description: "ampersand" },
      { number: "&#34;", name: "&quot;", unicode: "\u0022", description: "quotation mark" },
      { number: "&#39;", name: "&apos;", unicode: "\u0027", description: "apostrophe " },
      { number: "&#60;", name: "&lt;", unicode: "\u003C", description: "less-than" },
      { number: "&#62;", name: "&gt;", unicode: "\u003E", description: "greater-than" }
    ],

    // Symbols
    symbols: [
      { number: "&#160;", name: "&nbsp;", unicode: "\u00A0", description: "non-breaking space" },
      { number: "&#161;", name: "&iexcl;", unicode: "\u00A1", description: "inverted exclamation mark" },
      { number: "&#162;", name: "&cent;", unicode: "\u00A2", description: "cent" },
      { number: "&#163;", name: "&pound;", unicode: "\u00A3", description: "pound" },
      { number: "&#164;", name: "&curren;", unicode: "\u00A4", description: "currency" },
      { number: "&#165;", name: "&yen;", unicode: "\u00A5", description: "yen" },
      { number: "&#166;", name: "&brvbar;", unicode: "\u00A6", description: "broken vertical bar" },
      { number: "&#167;", name: "&sect;", unicode: "\u00A7", description: "section" },
      { number: "&#168;", name: "&uml;", unicode: "\u00A8", description: "spacing diaeresis" },
      { number: "&#169;", name: "&copy;", unicode: "\u00A9", description: "copyright" },
      { number: "&#170;", name: "&ordf;", unicode: "\u00AA", description: "feminine ordinal indicator" },
      { number: "&#171;", name: "&laquo;", unicode: "\u00AB", description: "angle quotation mark (left)" },
      { number: "&#172;", name: "&not;", unicode: "\u00AC", description: "negation" },
      { number: "&#173;", name: "&shy;", unicode: "\u00AD", description: "soft hyphen" },
      { number: "&#174;", name: "&reg;", unicode: "\u00AE", description: "registered trademark" },
      { number: "&#175;", name: "&macr;", unicode: "\u00AF", description: "spacing macron" },
      { number: "&#176;", name: "&deg;", unicode: "\u00B0", description: "degree" },
      { number: "&#177;", name: "&plusmn;", unicode: "\u00B1", description: "plus-or-minus " },
      { number: "&#178;", name: "&sup2;", unicode: "\u00B2", description: "superscript 2" },
      { number: "&#179;", name: "&sup3;", unicode: "\u00B3", description: "superscript 3" },
      { number: "&#180;", name: "&acute;", unicode: "\u00B4", description: "spacing acute" },
      { number: "&#181;", name: "&micro;", unicode: "\u00B5", description: "micro" },
      { number: "&#182;", name: "&para;", unicode: "\u00B6", description: "paragraph" },
      { number: "&#183;", name: "&middot;", unicode: "\u00B7", description: "middle dot" },
      { number: "&#184;", name: "&cedil;", unicode: "\u00B8", description: "spacing cedilla" },
      { number: "&#185;", name: "&sup1;", unicode: "\u00B9", description: "superscript 1" },
      { number: "&#186;", name: "&ordm;", unicode: "\u00BA", description: "masculine ordinal indicator" },
      { number: "&#187;", name: "&raquo;", unicode: "\u00BB", description: "angle quotation mark (right)" },
      { number: "&#188;", name: "&frac14;", unicode: "\u00BC", description: "fraction 1/4" },
      { number: "&#189;", name: "&frac12;", unicode: "\u00BD", description: "fraction 1/2" },
      { number: "&#190;", name: "&frac34;", unicode: "\u00BE", description: "fraction 3/4" },
      { number: "&#191;", name: "&iquest;", unicode: "\u00BF", description: "inverted question mark" },
      { number: "&#215;", name: "&times;", unicode: "\u00D7", description: "multiplication" },
      { number: "&#247;", name: "&divide;", unicode: "\u00F7", description: "division" }
    ],

    // Characters
    characters: [
      { number: "&#192;", name: "&Agrave;", unicode: "\u00C0", description: "capital a, grave accent" },
      { number: "&#193;", name: "&Aacute;", unicode: "\u00C1", description: "capital a, acute accent" },
      { number: "&#194;", name: "&Acirc;", unicode: "\u00C2", description: "capital a, circumflex accent" },
      { number: "&#195;", name: "&Atilde;", unicode: "\u00C3", description: "capital a, tilde" },
      { number: "&#196;", name: "&Auml;", unicode: "\u00C4", description: "capital a, umlaut mark" },
      { number: "&#197;", name: "&Aring;", unicode: "\u00C5", description: "capital a, ring" },
      { number: "&#198;", name: "&AElig;", unicode: "\u00C6", description: "capital ae" },
      { number: "&#199;", name: "&Ccedil;", unicode: "\u00C7", description: "capital c, cedilla" },
      { number: "&#200;", name: "&Egrave;", unicode: "\u00C8", description: "capital e, grave accent" },
      { number: "&#201;", name: "&Eacute;", unicode: "\u00C9", description: "capital e, acute accent" },
      { number: "&#202;", name: "&Ecirc;", unicode: "\u00CA", description: "capital e, circumflex accent" },
      { number: "&#203;", name: "&Euml;", unicode: "\u00CB", description: "capital e, umlaut mark" },
      { number: "&#204;", name: "&Igrave;", unicode: "\u00CC", description: "capital i, grave accent" },
      { number: "&#205;", name: "&Iacute;", unicode: "\u00CD", description: "capital i, acute accent" },
      { number: "&#206;", name: "&Icirc;", unicode: "\u00CE", description: "capital i, circumflex accent" },
      { number: "&#207;", name: "&Iuml;", unicode: "\u00CF", description: "capital i, umlaut mark" },
      { number: "&#208;", name: "&ETH;", unicode: "\u00D0", description: "capital eth, Icelandic" },
      { number: "&#209;", name: "&Ntilde;", unicode: "\u00D1", description: "capital n, tilde" },
      { number: "&#210;", name: "&Ograve;", unicode: "\u00D2", description: "capital o, grave accent" },
      { number: "&#211;", name: "&Oacute;", unicode: "\u00D3", description: "capital o, acute accent" },
      { number: "&#212;", name: "&Ocirc;", unicode: "\u00D4", description: "capital o, circumflex accent" },
      { number: "&#213;", name: "&Otilde;", unicode: "\u00D5", description: "capital o, tilde" },
      { number: "&#214;", name: "&Ouml;", unicode: "\u00D6", description: "capital o, umlaut mark" },
      { number: "&#216;", name: "&Oslash;", unicode: "\u00D8", description: "capital o, slash" },
      { number: "&#217;", name: "&Ugrave;", unicode: "\u00D9", description: "capital u, grave accent" },
      { number: "&#218;", name: "&Uacute;", unicode: "\u00DA", description: "capital u, acute accent" },
      { number: "&#219;", name: "&Ucirc;", unicode: "\u00DB", description: "capital u, circumflex accent" },
      { number: "&#220;", name: "&Uuml;", unicode: "\u00DC", description: "capital u, umlaut mark" },
      { number: "&#221;", name: "&Yacute;", unicode: "\u00DD", description: "capital y, acute accent" },
      { number: "&#222;", name: "&THORN;", unicode: "\u00DE", description: "capital THORN, Icelandic" },
      { number: "&#223;", name: "&szlig;", unicode: "\u00DF", description: "small sharp s, German" },
      { number: "&#224;", name: "&agrave;", unicode: "\u00E0", description: "small a, grave accent" },
      { number: "&#225;", name: "&aacute;", unicode: "\u00E1", description: "small a, acute accent" },
      { number: "&#226;", name: "&acirc;", unicode: "\u00E2", description: "small a, circumflex accent" },
      { number: "&#227;", name: "&atilde;", unicode: "\u00E3", description: "small a, tilde" },
      { number: "&#228;", name: "&auml;", unicode: "\u00E4", description: "small a, umlaut mark" },
      { number: "&#229;", name: "&aring;", unicode: "\u00E5", description: "small a, ring" },
      { number: "&#230;", name: "&aelig;", unicode: "\u00E6", description: "small ae" },
      { number: "&#231;", name: "&ccedil;", unicode: "\u00E7", description: "small c, cedilla" },
      { number: "&#232;", name: "&egrave;", unicode: "\u00E8", description: "small e, grave accent" },
      { number: "&#233;", name: "&eacute;", unicode: "\u00E9", description: "small e, acute accent" },
      { number: "&#234;", name: "&ecirc;", unicode: "\u00EA", description: "small e, circumflex accent" },
      { number: "&#235;", name: "&euml;", unicode: "\u00EB", description: "small e, umlaut mark" },
      { number: "&#236;", name: "&igrave;", unicode: "\u00EC", description: "small i, grave accent" },
      { number: "&#237;", name: "&iacute;", unicode: "\u00ED", description: "small i, acute accent" },
      { number: "&#238;", name: "&icirc;", unicode: "\u00EE", description: "small i, circumflex accent" },
      { number: "&#239;", name: "&iuml;", unicode: "\u00EF", description: "small i, umlaut mark" },
      { number: "&#240;", name: "&eth;", unicode: "\u00F0", description: "small eth, Icelandic" },
      { number: "&#241;", name: "&ntilde;", unicode: "\u00F1", description: "small n, tilde" },
      { number: "&#242;", name: "&ograve;", unicode: "\u00F2", description: "small o, grave accent" },
      { number: "&#243;", name: "&oacute;", unicode: "\u00F3", description: "small o, acute accent" },
      { number: "&#244;", name: "&ocirc;", unicode: "\u00F4", description: "small o, circumflex accent" },
      { number: "&#245;", name: "&otilde;", unicode: "\u00F5", description: "small o, tilde" },
      { number: "&#246;", name: "&ouml;", unicode: "\u00F6", description: "small o, umlaut mark" },
      { number: "&#248;", name: "&oslash;", unicode: "\u00F8", description: "small o, slash" },
      { number: "&#249;", name: "&ugrave;", unicode: "\u00F9", description: "small u, grave accent" },
      { number: "&#250;", name: "&uacute;", unicode: "\u00FA", description: "small u, acute accent" },
      { number: "&#251;", name: "&ucirc;", unicode: "\u00FB", description: "small u, circumflex accent" },
      { number: "&#252;", name: "&uuml;", unicode: "\u00FC", description: "small u, umlaut mark" },
      { number: "&#253;", name: "&yacute;", unicode: "\u00FD", description: "small y, acute accent" },
      { number: "&#254;", name: "&thorn;", unicode: "\u00FE", description: "small thorn, Icelandic" },
      { number: "&#255;", name: "&yuml;", unicode: "\u00FF", description: "small y, umlaut mark" },
      { name: "&alefsym;", unicode: "\u2135" },
      { name: "&Alpha;", unicode: "\u0391" },
      { name: "&alpha;", unicode: "\u03B1" },
      { name: "&and;", unicode: "\u2227" },
      { name: "&ang;", unicode: "\u2220" },
      { name: "&asymp;", unicode: "\u2248" },
      { name: "&bdquo;", unicode: "\u201E" },
      { name: "&Beta;", unicode: "\u0392" },
      { name: "&beta;", unicode: "\u03B2" },
      { name: "&bull;", unicode: "\u2022" },
      { name: "&cap;", unicode: "\u2229" },
      { name: "&Chi;", unicode: "\u03A7" },
      { name: "&chi;", unicode: "\u03C7" },
      { name: "&circ;", unicode: "\u02C6" },
      { name: "&clubs;", unicode: "\u2663" },
      { name: "&cong;", unicode: "\u2245" },
      { name: "&crarr;", unicode: "\u21B5" },
      { name: "&cup;", unicode: "\u222A" },
      { name: "&Dagger;", unicode: "\u2021" },
      { name: "&dagger;", unicode: "\u2020" },
      { name: "&dArr;", unicode: "\u21D3" },
      { name: "&darr;", unicode: "\u2193" },
      { name: "&Delta;", unicode: "\u0394" },
      { name: "&delta;", unicode: "\u03B4" },
      { name: "&diams;", unicode: "\u2666" },
      { name: "&empty;", unicode: "\u2205" },
      { name: "&emsp;", unicode: "\u2003" },
      { name: "&ensp;", unicode: "\u2002" },
      { name: "&Epsilon;", unicode: "\u0395" },
      { name: "&epsilon;", unicode: "\u03B5" },
      { name: "&equiv;", unicode: "\u2261" },
      { name: "&Eta;", unicode: "\u0397" },
      { name: "&eta;", unicode: "\u03B7" },
      { name: "&euro;", unicode: "\u20AC" },
      { name: "&exist;", unicode: "\u2203" },
      { name: "&fnof;", unicode: "\u0192" },
      { name: "&forall;", unicode: "\u2200" },
      { name: "&frasl;", unicode: "\u2044" },
      { name: "&Gamma;", unicode: "\u0393" },
      { name: "&gamma;", unicode: "\u03B3" },
      { name: "&ge;", unicode: "\u2265" },
      { name: "&hArr;", unicode: "\u21D4" },
      { name: "&harr;", unicode: "\u2194" },
      { name: "&hearts;", unicode: "\u2665" },
      { name: "&hellip;", unicode: "\u2026" },
      { name: "&image;", unicode: "\u2111" },
      { name: "&infin;", unicode: "\u221E" },
      { name: "&int;", unicode: "\u222B" },
      { name: "&Iota;", unicode: "\u0399" },
      { name: "&iota;", unicode: "\u03B9" },
      { name: "&isin;", unicode: "\u2208" },
      { name: "&Kappa;", unicode: "\u039A" },
      { name: "&kappa;", unicode: "\u03BA" },
      { name: "&Lambda;", unicode: "\u039B" },
      { name: "&lambda;", unicode: "\u03BB" },
      { name: "&lang;", unicode: "\u2329" },
      { name: "&lArr;", unicode: "\u21D0" },
      { name: "&larr;", unicode: "\u2190" },
      { name: "&lceil;", unicode: "\u2308" },
      { name: "&ldquo;", unicode: "\u201C" },
      { name: "&le;", unicode: "\u2264" },
      { name: "&lfloor;", unicode: "\u230A" },
      { name: "&lowast;", unicode: "\u2217" },
      { name: "&loz;", unicode: "\u25CA" },
      { name: "&lrm;", unicode: "\u200E" },
      { name: "&lsaquo;", unicode: "\u2039" },
      { name: "&lsquo;", unicode: "\u2018" },
      { name: "&mdash;", unicode: "\u2014" },
      { name: "&minus;", unicode: "\u2212" },
      { name: "&Mu;", unicode: "\u039C" },
      { name: "&mu;", unicode: "\u03BC" },
      { name: "&nabla;", unicode: "\u2207" },
      { name: "&ndash;", unicode: "\u2013" },
      { name: "&ne;", unicode: "\u2260" },
      { name: "&ni;", unicode: "\u220B" },
      { name: "&notin;", unicode: "\u2209" },
      { name: "&nsub;", unicode: "\u2284" },
      { name: "&Nu;", unicode: "\u039D" },
      { name: "&nu;", unicode: "\u03BD" },
      { name: "&OElig;", unicode: "\u0152" },
      { name: "&oelig;", unicode: "\u0153" },
      { name: "&oline;", unicode: "\u203E" },
      { name: "&Omega;", unicode: "\u03A9" },
      { name: "&omega;", unicode: "\u03C9" },
      { name: "&Omicron;", unicode: "\u039F" },
      { name: "&omicron;", unicode: "\u03BF" },
      { name: "&oplus;", unicode: "\u2295" },
      { name: "&or;", unicode: "\u2228" },
      { name: "&ordm;", unicode: "\u00BA" },
      { name: "&otimes;", unicode: "\u2297" },
      { name: "&part;", unicode: "\u2202" },
      { name: "&permil;", unicode: "\u2030" },
      { name: "&perp;", unicode: "\u22A5" },
      { name: "&Phi;", unicode: "\u03A6" },
      { name: "&phi;", unicode: "\u03C6" },
      { name: "&Pi;", unicode: "\u03A0" },
      { name: "&pi;", unicode: "\u03C0" },
      { name: "&piv;", unicode: "\u03D6" },
      { name: "&Prime;", unicode: "\u2033" },
      { name: "&prime;", unicode: "\u2032" },
      { name: "&prod;", unicode: "\u220F" },
      { name: "&prop;", unicode: "\u221D" },
      { name: "&Psi;", unicode: "\u03A8" },
      { name: "&psi;", unicode: "\u03C8" },
      { name: "&radic;", unicode: "\u221A" },
      { name: "&rang;", unicode: "\u232A" },
      { name: "&rArr;", unicode: "\u21D2" },
      { name: "&rarr;", unicode: "\u2192" },
      { name: "&rceil;", unicode: "\u2309" },
      { name: "&rdquo;", unicode: "\u201D" },
      { name: "&real;", unicode: "\u211C" },
      { name: "&rfloor;", unicode: "\u230B" },
      { name: "&Rho;", unicode: "\u03A1" },
      { name: "&rho;", unicode: "\u03C1" },
      { name: "&rlm;", unicode: "\u200F" },
      { name: "&rsaquo;", unicode: "\u203A" },
      { name: "&rsquo;", unicode: "\u2019" },
      { name: "&sbquo;", unicode: "\u201A" },
      { name: "&Scaron;", unicode: "\u0160" },
      { name: "&scaron;", unicode: "\u0161" },
      { name: "&sdot;", unicode: "\u22C5" },
      { name: "&Sigma;", unicode: "\u03A3" },
      { name: "&sigma;", unicode: "\u03C3" },
      { name: "&sigmaf;", unicode: "\u03C2" },
      { name: "&sim;", unicode: "\u223C" },
      { name: "&spades;", unicode: "\u2660" },
      { name: "&sub;", unicode: "\u2282" },
      { name: "&sube;", unicode: "\u2286" },
      { name: "&sum;", unicode: "\u2211" },
      { name: "&sup;", unicode: "\u2283" },
      { name: "&supe;", unicode: "\u2287" },
      { name: "&Tau;", unicode: "\u03A4" },
      { name: "&tau;", unicode: "\u03C4" },
      { name: "&there4;", unicode: "\u2234" },
      { name: "&Theta;", unicode: "\u0398" },
      { name: "&theta;", unicode: "\u03B8" },
      { name: "&thetasym;", unicode: "\u03D1" },
      { name: "&thinsp;", unicode: "\u2009" },
      { name: "&tilde;", unicode: "\u02DC" },
      { name: "&trade;", unicode: "\u2122" },
      { name: "&uArr;", unicode: "\u21D1" },
      { name: "&uarr;", unicode: "\u2191" },
      { name: "&upsih;", unicode: "\u03D2" },
      { name: "&Upsilon;", unicode: "\u03A5" },
      { name: "&upsilon;", unicode: "\u03C5" },
      { name: "&weierp;", unicode: "\u2118" },
      { name: "&Xi;", unicode: "\u039E" },
      { name: "&xi;", unicode: "\u03BE" },
      { name: "&Yuml;", unicode: "\u0178" },
      { name: "&Zeta;", unicode: "\u0396" },
      { name: "&zeta;", unicode: "\u03B6" },
      { name: "&zwj;", unicode: "\u200D" },
      { name: "&zwnj;", unicode: "\u200C" }
    ]
  };

  var stopPropagation = function(event) {
    event.stopPropagation();
  };

  //
  // AjaxDialog
  //

  // A dialog to enforce modality while ajaxing

  var $ajaxDialog;
  var ajaxDialogInitialized = false;
  var ajaxDialogOpened = false;
  var ajaxDialogStack = [];

  function initAjaxDialog() {
    if (ajaxDialogInitialized) return;
    $('body').append('<div id="ajax-dialog" class="hidden"><div class="inner">' +
      '<div class="inner2 horz-center"><div class="center-inner">' +
      '<h6 class="inverted center-element msg"></h6>' +
      '<img src="/images/ajax-loader32.gif" alt=""/>' +
      '<div class="inner3 horz-center"><div class="center-inner">' +
      '<div class="inverted center-element status"></div>' +
      '</div></div></div></div>');
    $ajaxDialog = $('#ajax-dialog').dialog({
      autoOpen: false,
      closeOnEscape: false,
      width: 400,
      resizable: false,
      dialogClass: 'ajax-dialog',
      modal: true
    });
    ajaxDialogInitialized = true;
  }

  function openAjaxDialog(htmlMessage) {
    initAjaxDialog();
    htmlMessage = htmlMessage || "Contacting server...";
    setAjaxDialogStatus("");
    $('.msg', $ajaxDialog).html(htmlMessage);
    $ajaxDialog.dialog("open");
    moveDialogToTop("ajax-dialog");
    ajaxDialogOpened = true;
  }

  function popAjaxDialog(key) {
    var found = false;
    $.each(ajaxDialogStack, function (inx) {
      if (this.key === key) {
        found = true;
        ajaxDialogStack.splice(inx, 1);
        return false;
      }
    });
    if (!found) throw "popAjaxDialog: missing key " + key;
    if (ajaxDialogStack.length) setStackedAjaxDialogStatus();
    else closeAjaxDialog();
  }

  function pushAjaxDialog(key, msg) {
    if (!ajaxDialogStack.length) openAjaxDialog();
    ajaxDialogStack.push({ key: key, msg: msg });
    setStackedAjaxDialogStatus();
  }

  function setAjaxDialogStatus(htmlStatus) {
    $('.status', $ajaxDialog).html(htmlStatus);
  }

  function setStackedAjaxDialogStatus() {
    var msgs = [];
    $.each(ajaxDialogStack, function () {
      msgs.push(this.msg);
    });
    setAjaxDialogStatus(msgs.join('<br />'));
  }

  function closeAjaxDialog() {
    if ($ajaxDialog) $ajaxDialog.dialog("close");
    ajaxDialogOpened = false;
  }

  function inAjaxDialog() {
    return ajaxDialogOpened;
  }

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
      .safeBind("click", function(event) {
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
    moveDialogToTop("alert-dialog");
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

  // Call chaining (uses plain array)
  //
  // Use pushChain(array, fn, p1, p2...) to add an entry
  // Use chain(array) to execute the oldest entry, or return if none

  var pushChain = function() {
    var args = $.makeArray(arguments);
    if (args.length < 2) throw "pushChain: at least two args required";
    if (!$.isArray(args[0])) throw "pushChain: 1st arg must be array";
    if (typeof args[1] !== "function") throw "pushChain: 2nd arg must be function";
    args[0].push({ fn: args[1], args: args.slice(2) });
  };

  var chain = function(chains) {
    if (!$.isArray(chains)) throw "chain: arg must be array";
    if (chains.length) {
      var f = chains.shift();
      f.fn.apply(null, f.args);
    }
  };

  function required(obj, name) {
    if (!obj) throw "A required script is missing: " + name;
  }

  // P U B L I C

  var $$ = function(idSelector) {
    return $(toCssId(idSelector));
  };

  var adjustLoggedEmailResizeableDialogSize = function (event) {
    // dialog must contain two divs, top and bottom, thst fill the dialog. The
    // bottom height is adjusted to maintain the fill.
    var $target = $(event.target);
    $(".bottom", $target).height($target.height() -
    $(".top", $target).height() - 12);
  };

  var ajax = function(options) {
    options.type = options.type || "POST";
    options.contentType = options.contentType || "application/json; charset=utf-8";
    options.dataType = options.dataType || "json";
    if (options.data && typeof options.data !== "string")
      options.data = JSON.stringify(options.data);
    return $.ajax(options);
  };

  var aspKeepAliveId = null;
  var aspKeepAlive = function (ms) {
    if (aspKeepAliveId) {
      clearInterval(aspKeepAliveId);
      aspKeepAliveId = null;
    }
    // 15 minutes = 900 seconds = 900000 ms
    aspKeepAliveId = setInterval(function () {
      ajax({
        //url: "/Admin/WebService.asmx/AspKeepAlive"
        url: "/Admin/KeepAlive.aspx"
    });
    }, ms || 900000);
  };
  
  var assignRotatingClassesToChildren = function($parent, classes) {
    if (!classes.length || !$parent.length) return;
    if ($parent[0].tagName === "TABLE") $parent = $("tbody", $parent);
    var toRemove = classes.join(" ");
    var index = 0;
    $parent.children().each(function () {
      var $this = $(this);
      if (!$this.hasClass("hidden")) {
        if (index >= classes.length) index = 0;
        $this.removeClass(toRemove).addClass(classes[index++]);
      }
    });
  };

  var clearSelection = function() {
    try {
      if (document.selection && document.selection.empty) {
        document.selection.empty();
        return;
      }
    } catch (e) {
    }
    try {
      if (window.getSelection) {
        var sel = window.getSelection();
        if (sel && sel.removeAllRanges)
          sel.removeAllRanges();
      }
    } catch (e) {
    }
  };

  var dialogOpenAndResizeableOptions = function (open) {
    return {
        resizable: true,
        resize: adjustLoggedEmailResizeableDialogSize,
        resizeStart: function (event) {
          // fiddling the width makes sure it is correct when done
          $(".bottom", $(event.target)).css("width", "99%");
        },
        resizeStop: function (event) {
          adjustLoggedEmailResizeableDialogSize(event);
          $(".bottom", $(event.target)).css("width", "100%");
        },
        open: function (event, ui) {
          setTimeout(function () { adjustLoggedEmailResizeableDialogSize(event); }, 50);
          if (typeof open === "function") open(event, ui);
        }
    };
  };

  var emailForSort = function(email) {
    email = email.toLowerCase();
    var atAt = email.indexOf("@");
    if (atAt >= 0)
      email = email.substr(atAt + 1) + " " + email.substr(0, atAt);
    return email;
  };

  function endsWith(str, pattern) {
    var d = str.length - pattern.length;
    return d >= 0 && str.lastIndexOf(pattern) === d;
  };

  var entitize = function(str, options) {
    var optionTarget = null;
    var entityType;
    var i;
    var iMax;
    var unicodeRegex;

    if (typeof str !== "string") {
      throw new TypeError("The value of 'str' is not of a valid type.");
    }

    if (typeof options === "object") {
      if (typeof options.target === "string" && options.target.length) {
        optionTarget = options.target;
      }
    }

    switch (optionTarget) {
    case "html": // Do not entitize html reserved characters
      for (entityType in entities) {
        if (entities.hasOwnProperty(entityType)) {
          switch (entityType) {
          case "":
          case "reservedHtml":
            // ignore entity types
            break;
          default:
            for (i = 0, iMax = entities[entityType].length; i < iMax; i = i + 1) {
              unicodeRegex = new RegExp(entities[entityType][i].unicode, "g");

              if (entities[entityType][i].name) {
                str = str.replace(unicodeRegex, entities[entityType][i].name);
              } else if (entities[entityType][i].number) {
                str = str.replace(unicodeRegex, entities[entityType][i].number);
              } else {
                str = str.replace(unicodeRegex, "");
              }
            }
          }
        }
      }

      str = str.replace(/(?:<sup>)\s*(&(?:copy|trade|reg);)\s*(?:<\/sup>)/g, "$1");
      str = str.replace(/(&(?:copy|reg);)/g, "<sup>$1<\/sup>");
      break;

    case "htmlAttribute": // entititize html reserved characters; strip superscript tags from copyright/trademarks
      for (entityType in entities) {
        if (entities.hasOwnProperty(entityType)) {
          switch (entityType) {
          case "":
            // ignore entity types
            break;
          default:
            for (i = 0, iMax = entities[entityType].length; i < iMax; i = i + 1) {
              unicodeRegex = new RegExp(entities[entityType][i].unicode, "g");

              if (entities[entityType][i].name) {
                str = str.replace(unicodeRegex, entities[entityType][i].name);
              } else if (entities[entityType][i].number) {
                str = str.replace(unicodeRegex, entities[entityType][i].number);
              } else {
                str = str.replace(unicodeRegex, "");
              }
            }
          }
        }
      }

      str = str.replace(/(?:<sup>)\s*(&(?:copy|trade|reg);)\s*(?:<\/sup>)/g, "$1");
      break;

    case "htmlText": // entitize html reserved characters; strip superscript tags from copyright/trademarks
      for (entityType in entities) {
        if (entities.hasOwnProperty(entityType)) {
          switch (entityType) {
          case "":
            //case "reservedHtml":
            // ignore entity types
            break;
          default:
            for (i = 0, iMax = entities[entityType].length; i < iMax; i = i + 1) {
              unicodeRegex = new RegExp(entities[entityType][i].unicode, "g");

              if (entities[entityType][i].name) {
                str = str.replace(unicodeRegex, entities[entityType][i].name);
              } else if (entities[entityType][i].number) {
                str = str.replace(unicodeRegex, entities[entityType][i].number);
              } else {
                str = str.replace(unicodeRegex, "");
              }
            }
          }
        }
      }

      str = str.replace(/(?:<sup>)\s*(&(?:copy|trade|reg);)\s*(?:<\/sup>)/g, "$1");
      break;

    default: // Legacy implementation
      str = str.replace(/\u0020\u0026\u0020/g, " &amp; ");
      str = str.replace(/\u00A9/g, "&copy;");
      str = str.replace(/\u00B0/g, "&deg;");
      str = str.replace(/\u0022/g, "&quot;");
      //str = str.replace(/\u0027/g, "&apos;");
      str = str.replace(/\u2018/g, "&apos;");
      str = str.replace(/\u2019/g, "&apos;");
      str = str.replace(/\u201C/g, "&quot;");
      str = str.replace(/\u201D/g, "&quot;");
      str = str.replace(/\u00AE/g, "&reg;");
      str = str.replace(/\u2122/g, "&trade;");
      str = str.replace(/\u2013/g, "&ndash;");
      str = str.replace(/\u2014/g, "&mdash;");
      str = str.replace(/\u0097/g, "&mdash;");
    };
    return str;
  };

  function escapeJsQuotes(str) {
    return str.replace(/'|"/g, "\\$&");
  }

  var escapeRegex = function(str) {
    return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
  };

  var formatAjaxError = function(result, message) {
    message = message || "";
    if (message) message += ": ";
    message += result.statusText + " (" + result.status + ")";
    try {
      message += "\n" + JSON.parse(result.responseText).Message;
    } catch (e) {
    };
    return message;
  };

  var getClasses = function(o) {
    if (!(o instanceof $)) o = $(o);
    var classNames = o.attr("class");
    if (!classNames) return [];
    return classNames.split(/\s+/);
  };

  var getCurrentTabPanel = function(tabsId) {
    return $(toCssId(tabsId) + " .ui-tabs-panel[aria-hidden='false']").filter(":visible");
  };

  var getCurrentTabId = function(tabsId) {
    return $(toCssId(tabsId) + " .ui-tabs-panel[aria-hidden='false']").filter(":visible").attr("id");
  };

  var getPrefixedClass = function(o, prefix, removePrefix) {
    var result;
    $.each(getClasses(0), function(index, className) {
      if (className.startsWith(prefix)) {
        if (removePrefix) result = className.substr(prefix.length);
        else result = className;
        return false;
      }
    });
    return result;
  };

  var getServerIdFromClientId = function(id) {
    var inx = id.lastIndexOf("_");
    if (inx >= 0) return id.substr(inx + 1);
    return id;
  };

  var getTabIndex = function(tabsId, tabId) {
    if (tabsId.substr(1))
      return $(toCssId(tabsId) + ">div").index($(toCssId(tabId)));
  };

  var getUtcDateFromLocalTime = function(datetime) {
    var m = moment(datetime).hour(0).minute(0).second(0)
      .millisecond(0).utc();
    if (!m.isValid()) return "";
    return m.format("YYYY/MM/DD HH:mm:ss.SSS");
  };

  var htmlEscape = function (str) {
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

  var htmlEscapeAttribute = function (str) {
    return entitize(str, { target: "htmlAttribute" });
  };

  var htmlEscapeHtml = function (str) {
    return entitize(str, { target: "html" });
  };

  var htmlEscapeText = function (str) {
    return entitize(str, { target: "htmlText" });
  };

  var initTipTip = function($o) {
    $o.tipTip({ edgeOffset: 10 });
  };

  var insertLocalDates = function() {
    $(".localdate").each(function() {
      try {
        var $this = $(this);
        var date = new Date(parseInt($this.attr("ticks")) / 10000).toString();
        var format = $this.attr("format");
        var str;
        if (format) str = moment(date).format(format);
        else str = date.toString();
        $this.html(str);
      } catch (ex) {
      }
    });
  };

//  var isIeCompatibilityMode = function() {
//    var ua = navigator.userAgent;
//    var ieRegex = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
//    if (ieRegex.exec(ua) !== null) {
//      if (ua.indexOf("Trident/6.0") > -1)
//        return ua.indexOf("MSIE 7.0") > -1;
//      else if (ua.indexOf("Trident/5.0") > -1)
//        return ua.indexOf("MSIE 7.0") > -1;
//      else if (ua.indexOf("Trident/4.0") > -1)
//        return ua.indexOf("MSIE 7.0") > -1;
//    }
//    return false;
//  };

  var isMenuItemDisabled = function(event) {
    // looks for class "disabled" in li
    return $(event.target).closest("li").hasClass("disabled");
  };

  var isValidEmailRegex = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
  var isValidEmail = function (email) {
    return !!email.match(isValidEmailRegex);
  };

  var isValidUrlRegex = /^(?:(http(?:s)?\:\/\/)?\S+(?:(?:\.)\S+)+(?:\/S+)*)$/i;
  var isValidUrl = function (url) {
    return !!url.match(isValidUrlRegex);
  };

  var jurisdictionForSort = function(jurisdiction) {
    return jurisdiction.toLowerCase().split(",").reverse().join(" ");
  };
  
  var lCase = function(str, lc) {
    // optional toLowerCase for succinct optional recasing
    return (lc) ? str.toLowerCase() : str;
  };

  var loadCss = function(url) {
    var link = document.createElement("link");
    link.type = "text/css";
    link.rel = "stylesheet";
    link.href = url;
    document.getElementsByTagName("head")[0].appendChild(link);
  };

  var moveDialogToTop = function(dialogClass) {
    // hack because moveToTop won't work for me
    if (dialogClass.substr(0, 1) === ".")
      dialogClass = dialogClass.substr(1);
    var bigz = 0;
    $(".ui-dialog, .ui-widget-overlay").each(function () {
      var $this = $(this);
      if (!$this.hasClass(dialogClass))
        bigz = Math.max(bigz, parseInt($this.css("z-index")));
    });
    if (bigz) $("." + dialogClass).css("z-index", bigz + 1);
  };

  var contextMenuCallbacks = [];
  var contextElements = [];
  var listening = false;

  var offContextMenu = function($menu) {
    $menu.each(function() {
      var inx = $.inArray(this, contextElements);
      if (inx !== -1) {
        contextElements.splice(inx, 1);
        contextMenuCallbacks.splice(inx, 1);
      }
    });
  };

  var onContextMenu = function($menu, callback) {
    $menu.each(function() {
      if ($.inArray(this, contextElements) === -1) {
        offContextMenu($(this));
        contextElements.push(this);
        contextMenuCallbacks.push(callback);
        if (!listening) {
          $(document).safeBind("contextmenu", contextMenuPrivate);
          listening = true;
        }
      }
    });
  };

  var contextMenuPrivate = function(event) {
    var handled = false;
    $.each(contextMenuCallbacks, function(inx) {
      var $menu = $(contextElements[inx]);
      handled = this(event, $menu) === true;
      if (handled) {
        $menu.css("left", event.pageX);
        $menu.css("top", event.pageY);
        $menu.fadeIn(100, function() {
          $(document)
            .safeBind("click", function() {
              $menu.hide(100);
              $(document).off("click");
            });
        });
      } else
        $menu.hide();
      if (handled || event.isPropagationStopped()) return false;
    });
    if (handled) event.preventDefault();
  };

  var parseJsonDate = function(value) {
    return new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
  };

  var plural = function(n, pluralValue, singularValue) {
    return n === 1
      ? (typeof singularValue !== "string" ? "" : singularValue)
      : (typeof pluralValue !== "string" ? "s" : pluralValue);
  };

  var populateCheckboxList = function($list, listitems) {      
    var opts = [];
    $.each(listitems, function(index, item) {
      opts.push('<p class="sub-label" title="' + htmlEscape(item.Text) +
        '"><input type="checkbox" checked="checked" value="'
        + htmlEscape(item.Value) + '"/><span>' + htmlEscape(item.Text) + '</span></p>');
    });
    $list.html(opts.join(''));
};

  var populateDropdown = function($dropdown, listitems, firstitem, firstvalue, selected, rel,
    ignoreCase) {
    if (ignoreCase) selected = selected.toUpperCase();
    var options = [];
    var selectedAttr = function(value) {
      if (ignoreCase) value = value.toUpperCase();
      return value === selected ? ' selected="selected" ' : ' ';
    };
    var relAttr = function(item) {
      return rel ? ' rel="' + htmlEscape(item[rel]) + '" ' : ' ';
    };
    firstvalue = firstvalue || "";
    if (firstitem)
      options.push('<option value="' + htmlEscape(firstvalue) + '"' + selectedAttr(firstvalue) + '>' + htmlEscape(firstitem) + '</option>');
    $.each(listitems, function(index, item) {
      options.push('<option value="' + htmlEscape(item.Value) + '"' + selectedAttr(item.Value) + relAttr(item) + '>' + htmlEscape(item.Text) + '</option>');
    });
    $dropdown.html(options.join(''));
  };

  var errorMonitored = [];
  var errorMonitoredCallback = [];
  var errorMonitorId = null;

  var errorMonitor = function () {
    $.each(errorMonitored, function (inx) {
      var $this = $(this);
      var val;
      switch ($this[0].type) {
        case "checkbox":
          val = $this.prop("checked").toString();
          break;

        default:
          val = $this.val();
          break;
      }
      if ($this.attr("old-val") !== val) {
        $this.removeClass("error");
        $this.attr("old-val", val);
        if (typeof errorMonitoredCallback[inx] === "function")
          errorMonitoredCallback[inx]($this);
      }
    });
    if (errorMonitored.length)
      errorMonitorId = setTimeout(errorMonitor, 200);
    else
      errorMonitorId = null;
  };

  var registerDataMonitor = function($input, options) {
    options = options || {};
    $input.each(function() {
      var inx = $.inArray(this, errorMonitored);
      if (options.unregister) {
        if (inx !== -1) {
          errorMonitored.splice(inx, 1);
          errorMonitoredCallback.splice(inx, 1);
        }
      } else if (inx === -1) {
        errorMonitored.push(this);
        errorMonitoredCallback.push(options.onChange);
        if (!errorMonitorId)
          errorMonitorId = setTimeout(errorMonitor, 200);
      }
    });
  };

  // returns true if any elements in the list are changed
  function evaluateMonitoredElementsList(list) {
    if (!list) return false;
    var result = false;
    $.each(list, function () {
      if (this.$.valEx() !== this.val) {
        result = true;
        return false;
      }
    });
    return result;
  }

  // pass in a list of jQuery objects as arguments
  // builds a list to be used by evaluateMonitoredElementsList()
  function getMonitoredElementsList() {
    var list = [];
    for (var n = 0; n < arguments.length; n++)
      if (arguments[n].length)
        list.push({ $: arguments[n], val: arguments[n].valEx() });
    return list;
  }

  function localStorageIsAvailable() {
    try {
      var storage = window.localStorage,
			x = '__storage_test__';
      storage.setItem(x, x);
      storage.removeItem(x);
      return true;
    }
    catch (e) {
      return false;
    }
  }

  var replaceLineBreaksWithParagraphs = function (text, escape) {
    if (escape) text = htmlEscape(text);
    return '<p>' + text.replace(/[\n\r]+/g, "</p><p>") + '</p>';
  };

  var replaceLineBreaksWithSpaces = function (text, escape) {
    if (escape) text = htmlEscape(text);
    return text.replace(/[\n\r]+/g, " ");
  };

  var restoreSort = function($table, saved) {
    var dirs = $.fn.stupidtable.dir;
    if (!saved || saved.col === -1) return;
    var $th = (typeof saved.col === "string") 
      ? $("th" + saved.col, $table)
      : $($("th", $table)[saved.col]);
     $th.data("sort-dir", saved.dir === dirs.ASC ? dirs.DESC : dirs.ASC);
     $th.click();
  };

  var safeBind = function($o, event, handler) {
    $o.off(event, handler).on(event, handler);
  };
  
  var saveSort = function($table, def) {
    var dirs = $.fn.stupidtable.dir;
    var result = {};
    var $sortTh = $('th[class*="sorting-"]', $table);
    result.col = $sortTh.index();
    result.dir = $sortTh.hasClass("sorting-desc") ? dirs.DESC : dirs.ASC;

    // try to convert to class name in case columns change
    if (result.col !== -1) {
      var classes = [];
      if (result.col !== -1)
        $.each(getClasses($sortTh), function() {
          if (this.toString().substr(0, 8) !== "sorting-")
            classes.push(this.toString());
        });
      if (classes.length) result.col = "." + classes.join(".");
    }

    if (result.col === -1 && def) result = def;

    return result;
  };

  function sessionStorageIsAvailable() {
    try {
      var storage = window.sessionStorage,
			x = '__storage_test__';
      storage.setItem(x, x);
      storage.removeItem(x);
      return true;
    }
    catch (e) {
      return false;
    }
  }

  var setOffpageTargets = function ($html, target) {
    target = target || "_blank";
    $('a', $html).each(function() {
      var $this = $(this);
      var href = $this.attr("href");
      if (href) {
        if (href.toLowerCase().substr(0, 7) !== "mailto:")
          $this.attr("target", target);
      };
    });
  };

  function setResizableVertical($elements) {
    $elements.resizable({
      grid: [10000, 1] // vertical
    });
  }

  var sortChildrenByAttribute = function($parent, childSelector, attribute,
    caseIns) {
    var items = $parent.children(childSelector).sort(function(a, b) {
      var vA = lCase($(a).attr(attribute), caseIns);
      var vB = lCase($(b).attr(attribute), caseIns);
      return (vA < vB) ? -1 : (vA > vB) ? 1 : 0;
    });
    $parent.append(items);
  };

  var sortChildrenByContent = function($parent, childSelector, keySelector,
    caseIns) {
    var items = $parent.children(childSelector).sort(function(a, b) {
      var vA = lCase($(keySelector, a).text(), caseIns);
      var vB = lCase($(keySelector, b).text(), caseIns);
      return (vA < vB) ? -1 : (vA > vB) ? 1 : 0;
    });
    $parent.append(items);
  };

  var tabClick = function(event) {
    var $element = $(event.target).closest(".vcentered-tab");
    $('a', $element).unbind('click', stopPropagation).click(stopPropagation).click();
  };

  var toCssId = function(id) {
    if (id && id.length && id.substr(0, 1) !== "#")
      return "#" + id;
    return id;
  };
  
  function trimNonAlpha(s) {
    return s.replace(/^[^_\w\d]*(.*?)[^_\w\d]*$/i, "$1");
  }

  var trimmedSplit = function(ch, str) {
    var result = [];
    $.each(str.split(ch), function() {
      var x = $.trim(this);
      if (x) result.push(x);
    });
    return result;
  };

  var updateNocacheUrl = function(url) {
    var re = /[&?]x=\d+/i;
    var m = re.exec(url);
    if (m) url = url.substr(0, m.index + 3) + Date.now() + url.substr(m.index + m[0].length);
    return url;
  };

  // I N I T I A L I Z E

  return {
    $$: $$,
    ajax: ajax,
    alert: alert,
    aspKeepAlive: aspKeepAlive,
    assignRotatingClassesToChildren: assignRotatingClassesToChildren,
    chain: chain,
    clearSelection: clearSelection,
    closeAjaxDialog: closeAjaxDialog,
    confirm: confirm,
    endsWith: endsWith,
    entitize: entitize,
    emailForSort: emailForSort,
    escapeJsQuotes: escapeJsQuotes,
    escapeRegex: escapeRegex,
    evaluateMonitoredElementsList: evaluateMonitoredElementsList,
    formatAjaxError: formatAjaxError,
    getMonitoredElementsList: getMonitoredElementsList,
    getClasses: getClasses,
    getCurrentTabPanel: getCurrentTabPanel,
    getCurrentTabId: getCurrentTabId,
    getPrefixedClass: getPrefixedClass,
    getServerIdFromClientId: getServerIdFromClientId,
    getTabIndex: getTabIndex,
    getUtcDateFromLocalTime: getUtcDateFromLocalTime,
    htmlEscape: htmlEscape,
    htmlEscapeAttribute: htmlEscapeAttribute,
    htmlEscapeHtml: htmlEscapeHtml,
    htmlEscapeText: htmlEscapeText,
    inAjaxDialog: inAjaxDialog,
    initTipTip: initTipTip,
    insertLocalDates: insertLocalDates,
//    isIeCompatibilityMode: isIeCompatibilityMode,
    isMenuItemDisabled: isMenuItemDisabled,
    isValidEmail: isValidEmail,
    isValidUrl: isValidUrl, 
    jurisdictionForSort: jurisdictionForSort,
    lCase: lCase,
    loadCss: loadCss,
    localStorageIsAvailable: localStorageIsAvailable,
    moveDialogToTop: moveDialogToTop,
    offContextMenu: offContextMenu,
    onContextMenu: onContextMenu,
    openAjaxDialog: openAjaxDialog,
    parseJsonDate: parseJsonDate,
    plural: plural,
    popAjaxDialog: popAjaxDialog,
    populateDropdown: populateDropdown,
    populateCheckboxList: populateCheckboxList,
    pushAjaxDialog: pushAjaxDialog,
    pushChain: pushChain,
    registerDataMonitor: registerDataMonitor,
    replaceLineBreaksWithParagraphs: replaceLineBreaksWithParagraphs,
    replaceLineBreaksWithSpaces: replaceLineBreaksWithSpaces,
    restoreSort: restoreSort,
    safeBind: safeBind,
    saveSort: saveSort,
    setAjaxDialogStatus: setAjaxDialogStatus,
    dialogOpenAndResizeableOptions: dialogOpenAndResizeableOptions,
    sessionStorageIsAvailable: sessionStorageIsAvailable,
    setOffpageTargets: setOffpageTargets,
    setResizableVertical: setResizableVertical,
    sortChildrenByAttribute: sortChildrenByAttribute,
    sortChildrenByContent: sortChildrenByContent,
    tabClick: tabClick,
    toCssId: toCssId,
    trimNonAlpha: trimNonAlpha,
    trimmedSplit: trimmedSplit,
    updateNocacheUrl: updateNocacheUrl
  };

});