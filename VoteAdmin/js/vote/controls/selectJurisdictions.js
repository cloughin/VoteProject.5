define(["jquery", "vote/util"], function($, util) {

  var controlName = "select-jurisdictions-control";

  //
  // class members
  //

  var level = {
    states: "states",
    counties: "counties",
    locals: "locals",
    parties: "parties" // states only + parties menu
  };

  var getControlName = function () { return controlName; };

  var majorParties = "DGLR";

  //
  // keep a list of instances so event handlers can establish instance context
  //

  var instances = [];

  var getInstance = function(obj) {
    if (!(obj instanceof $)) {
      // assume event
      obj = $(obj.target);
    }
    var instance = null;
    var $control = obj.closest("." + controlName);
    if ($control.length)
      $.each(instances, function() {
        if (this.isThisControl($control)) {
          instance = this;
          return false;
        }
      });
    return instance;
  };

  //
  // event handlers
  //

  var onAllChange = function(event) {
    return getInstance(event).onAllChange(event);
  };

  var onCountiesGetListClick = function(event) {
    return getInstance(event).onCountiesGetListClick(event);
  };

  var onLocalsGetListClick = function(event) {
    return getInstance(event).onLocalsGetListClick(event);
  };

  var onListChange = function(event) {
    return getInstance(event).onListChange(event);
  };

  var onMajorChange = function (event) {
    return getInstance(event).onMajorChange(event);
  };

  var onPartiesGetListClick = function (event) {
    return getInstance(event).onPartiesGetListClick(event);
  };

  //
  // encapsulate an instance in an object to allow multiple
  //
  // ReSharper disable once InconsistentNaming
  function SelectJurisdictions($control, options) {
    if (!$control.hasClass(controlName))
      $control = $control.find("." + controlName);
    if ($control.length !== 1)
      $.error("couldn't find " + controlName);
    instances.push(this);

    //
    // Private instance members
    //

    var that = this;
    options = $.extend({}, options);

    //
    // Private methods
    //

    function checkMajorParties($list) {
      $("input[type=checkbox]", $list).each(function() {
        var $this = $(this);
        $this.prop("checked",
          majorParties.indexOf($this.val().substr(2)) >= 0);
      });
    };

    function getCategoryLevel($context) {
      // looks for a magic class
      var classes = $context.classes();
      var result = null;
      $.each(level, function() {
        if (classes.indexOf(this.toString()) !== -1) {
          result = this.toString();
          return false;
        }
      });
      return result;
    };

    function populateList($list, listitems) {
      util.populateCheckboxList($list, listitems);
      that.onListChange($list);
    };

    function resolveCategoryContext(obj) {
      // can be $category, event or string (states, counties, locals)
      if (obj instanceof $) return obj.closest(".category");
      if (typeof obj === "string")
        return $("." + obj + ".category", $control);
      // assume event
      return $(obj.target).closest(".category");
    };

    function restoreDropdown($context, codes, list) {
      $context = resolveCategoryContext($context);
      // don't change a specific category
      var $specific = $(".specific", $context);
      if ($specific.length && !$specific.hasClass("hidden")) return;
      var all = codes.length === 1 && codes[0] === "all";
      if (!$context.hasClass("states") && !all)
        populateList($(".checkbox-list", $context), list);
      var $checkboxList = $(".checkbox-list", $context);
      $(".all", $context).prop("checked", all);
      $("input[type=checkbox]", $checkboxList).prop("checked", all);
      if (!all)
        $.each(codes, function() {
          $("input[value=" + this +"]", $checkboxList).prop("checked", true);
        });
      if (!$context.hasClass("states")) {
        // leave all enabled for parties for all/major selection
        if (!$context.hasClass("parties"))
          $(".all", $context).prop("disabled", all);
        $(".get-list", $context).toggleClass("hidden", !all);
        $(".checkbox-list", $context).toggleClass("hidden", all);
      }
      that.onListChange($context);
    }

    function setCategoryList($context) {
      $context = resolveCategoryContext($context);
      // if the context's specific label is showing, we never change enabling
      var $specific = $(".specific", $context);
      if ($specific.length && !$specific.hasClass("hidden")) return;
      var $contextToCheck = getCategoryLevel($context) === level.parties
        ? $(".jurisdiction.states", $control)
        : $context.prev(".jurisdiction");
      var enable = $(".checkbox-list input[type=checkbox]:checked", $contextToCheck).length === 1;
      // leave parties enabled always so all/major can be selected
      if (!$context.hasClass("parties"))
        $context.toggleClass("disabled", !enable);
      // leave all enabled for parties for all/major selection
      if (!$context.hasClass("parties"))
        $(".all", $context).prop("disabled", true).prop("checked", true);
      $(".get-list", $context).removeClass("hidden");
      $(".get-list-button", $context).prop("disabled", !enable);
      $(".checkbox-list", $context).addClass("hidden").html("");
      updateListLabel($context);
    };

    function updateListLabel($context) {
      $context = resolveCategoryContext($context);

      // analyze list
      var checked = that.getCategoryCodes($context).length;
      var unchecked = that.getCategoryCodesNotChecked($context).length;

      var $label = $(".main-label", $context);
      var label = $label.text();
      var match = label.match(/ \([^\)]+\)$/);
      if (match)
        label = label.substr(0, match.index);
      label += unchecked + checked > 1
        ? " (" + (unchecked === 0 ? "all" : checked) + ")" : "";
      $label.html(label);
    };

    //
    // Public methods (privileged)
    //

    this.destroy = function() {
      // remove from instance list
      var inx = $.inArray(that, instances);
      if (inx !== -1) instances.splice(inx, 1);
    };

    this.getCategoryCodes =
      function($context, reportAll) {
        $context = resolveCategoryContext($context);
        if (reportAll === true && $(".all", $context).prop("checked"))
          return ["all"];
        var codes = [];
        $.each($(".checkbox-list input[type=checkbox]:checked", $context),
          function() {
            codes.push($(this).val());
          });
        return codes;
      };

    this.getCategoryCodesNotChecked =
      function($context) {
        $context = resolveCategoryContext($context);
        var codes = [];
        $.each($(".checkbox-list input[type=checkbox]:not(:checked)", $context),
          function() {
            codes.push($(this).val());
          });
        return codes;
      };

    this.getIsMajorPartiesChecked =
      function() {
        var $context = resolveCategoryContext("parties");
        return $(".major", $context).prop("checked");
      };

    this.getJurisdictionName = function($context, code) {
      $context = resolveCategoryContext($context);
      return $(".checkbox-list input[value=" + code + "]", $context).next().text();
    };

    this.getJurisdictionNames = function($context, reportAll) {
      $context = resolveCategoryContext($context);
      if (reportAll === true && $(".all", $context).prop("checked"))
        return ["all"];
      var names = [];
      $.each($(".checkbox-list input[type=checkbox]:checked", $context).next(),
        function() {
          names.push($(this).text());
        });
      return names;
    };

    this.getSingleCountyCode = function() {
      var counties = that.getCategoryCodes(level.counties);
      return counties.length === 1 ? counties[0] : "";
    };

    this.getSingleCountyName = function() {
      return that.getJurisdictionName(level.counties, that.getSingleCountyCode());
    };

    this.getSingleLocalKey = function () {
      var locals = that.getCategoryCodes(level.locals);
      return locals.length === 1 ? locals[0] : "";
    };

    this.getSingleLocalName = function() {
      return that.getJurisdictionName(level.locals, that.getSingleLocalKey());
    };

    this.getSingleStateCode = function() {
      var states = that.getCategoryCodes(level.states);
      return states.length === 1 ? states[0] : "";
    };

    this.getSingleStateName = function() {
      return that.getJurisdictionName(level.states, that.getSingleStateCode());
    };

    this.isThisControl = function(control) {
      if (control instanceof $)
        control = control[0];
      return control === $control[0];
    };

    // this is separate from the constructor so it is free to do callbacks
    // that require the client to have the instance reference
    this.init = function () {
      $(".category .all", $control).safeBind("change", onAllChange);
      $(".category .major", $control).safeBind("change", onMajorChange);
      $(".category .checkbox-list", $control).safeBind("change", onListChange);
      $(".category.counties .get-list-button", $control).safeBind("click", onCountiesGetListClick);
      $(".category.locals .get-list-button", $control).safeBind("click", onLocalsGetListClick);
      $(".category.parties .get-list-button", $control).safeBind("click", onPartiesGetListClick);
      that.onListChange(level.states);
      that.onListChange(level.counties);
      that.onListChange(level.locals);
    };

    this.onAllChange = function(event) {
      var $context = $(event.target).closest(".category");
      var checked = $(".all", $context).prop("checked");
      var isParties = $context.hasClass("parties");
      var $list = $(".checkbox-list", $context);
      var hidden = $list.hasClass("hidden");

      if (isParties && hidden) {
        $(".major", $context).prop("checked", !checked);
      } else {
        $(".checkbox-list input[type=checkbox]", $context).prop("checked", checked);
        that.onListChange(event);
      }
    };

    this.onCountiesGetListClick = function() {
      var $checkedBox = $(".jurisdiction.states .checkbox-list input:checked",
        $control);
      if ($checkedBox.length === 1) {
        $(".jurisdiction.counties .get-list", $control).addClass("hidden");
        util.ajax({
          url: "/Admin/WebService.asmx/GetCounties",
          data: {
            stateCode: $checkedBox.val()
          },

          success: function(result) {
            var $list = $(".jurisdiction.counties .checkbox-list", $control);
            if (result.d.length) {
              populateList($list, result.d);
              $(".jurisdiction.counties .all", $control).prop("disabled", false);
            } else {
              $list.html('<em>No county information is available</em>');
            }
            updateListLabel($list);
            $list.removeClass("hidden");
          }
        });
      }
    };

    this.onListChange = function(event) {
      var $context = resolveCategoryContext(event);
      var isParties = $context.hasClass("parties");
      var $list = $(".checkbox-list", $context);
      var hidden = $list.hasClass("hidden");

      if (isParties && hidden) return;

      // analyze list
      var checkedCodes = that.getCategoryCodes($context);
      var checked = checkedCodes.length;
      var uncheckedCodes = that.getCategoryCodesNotChecked($context);
      var unchecked = uncheckedCodes.length;

      // update the all checkbox
      var $all = $(".all", $context);
      $all.prop("checked", unchecked === 0);

      if (isParties) {
        // update the major checkbox
        var isMajor = true;
        // false if any non-major are checked
        $.each(checkedCodes, function() {
          if (majorParties.indexOf(this.toString().substr(2)) < 0) {
            isMajor = false;
            return false;
          }
        });
        // false if any major are unchecked
        if (isMajor)
          $.each(uncheckedCodes, function () {
            if (majorParties.indexOf(this.toString().substr(2)) >= 0) {
              isMajor = false;
              return false;
            }
          });
        $(".major", $context).prop("checked", isMajor);
      }

      updateListLabel($context);

      // if exactly one selection, then enable next sibling selection
      // otherwise, clear any next sibling selection
      var $c = $context;
      while (($c = $c.next(".jurisdiction")).length) {
        setCategoryList($c);
        if (checked === 1) break; // only continue if disabling
      }

      // states change must check parties
      if (getCategoryLevel($context) === level.states) 
        setCategoryList(level.parties);

      if (typeof options.onChange === "function")
        options.onChange($control, getCategoryLevel($context));
    };

    this.onLocalsGetListClick = function() {
      var $checkedBoxState = $(".jurisdiction.states .checkbox-list input:checked",
        $control);
      var $checkedBoxCounty = $(".jurisdiction.counties .checkbox-list input:checked",
        $control);
      if ($checkedBoxState.length === 1 && $checkedBoxCounty.length === 1) {
        $(".jurisdiction.locals .get-list", $control).addClass("hidden");

        util.ajax({
          url: "/Admin/WebService.asmx/GetLocals",
          data: {
            stateCode: $checkedBoxState.val(),
            countyCode: $checkedBoxCounty.val()
          },

          success: function(result) {
            var $list = $(".jurisdiction.locals .checkbox-list", $control);
            if (result.d.length) {
              populateList($list, result.d);
              $(".jurisdiction.locals .all", $control).prop("disabled", false);
            } else {
              $list.html('<em>No local district information is available</em>');
            }
            updateListLabel($list);
            $list.removeClass("hidden");
          }
        });
      }
    };

    this.onMajorChange = function (event) {
      // only parties
      var $context = $(event.target).closest(".category");
      var checked = $(".major", $context).prop("checked");
      var $list = $(".checkbox-list", $context);
      var hidden = $list.hasClass("hidden");
      if (hidden) {
        $(".all", $context).prop("checked", !checked);
      } else {
        if (checked)
          checkMajorParties($list);
        that.onListChange(event);
      }
    };

    this.onPartiesGetListClick = function () {
      var $checkedBox = $(".jurisdiction.states .checkbox-list input:checked",
        $control);
      if ($checkedBox.length === 1) {
        $(".category.parties .get-list", $control).addClass("hidden");
        util.ajax({
          url: "/Admin/WebService.asmx/GetParties",
          data: {
            stateCode: $checkedBox.val()
          },

          success: function (result) {
            var $list = $(".category.parties .checkbox-list", $control);
            var items = result.d;
            if (items.length && !items[0].Value)
              items.shift();
            if (items.length) {
              populateList($list, items);
              // all start as checked by default -- adjust if major only is selected
              if ($(".category.parties .major", $control).prop("checked")) {
                checkMajorParties($list);
              }
              //$(".category.parties .all", $control).prop("disabled", false);
            } else {
              $list.html('<em>No party information is available</em>');
            }
            updateListLabel($list);
            $list.removeClass("hidden");
          }
        });
      }
    };

    this.reset = function() {
      // set to state level
      that.toggleLevel("counties", false); // will hide counties and locals
      // check all states
      $(".jurisdiction.states .checkbox-list input",
        $control).prop("checked", true);
      that.onListChange("states");
    };

    this.restore = function (o, i) {
      // restore the dropdowns
      restoreDropdown("states", o.stateCodes);
      if (o.countyCodes && i.Counties)
        restoreDropdown("counties", o.countyCodes, i.Counties);
      if (o.localKeysOrCodes && i.Locals)
        restoreDropdown("locals", o.localKeysOrCodes, i.Locals);
      if (o.partyKeys && i.Parties)
        restoreDropdown("parties", o.partyKeys, i.Parties);
    };


    this.toggleLevel = function(l, show, force) {
      show = !!show; // true bool
      var $context = resolveCategoryContext(l);
      var isHidden = $context.hasClass("hidden");

      if (!force && l !== level.parties) {
        // if showing, higher level must be showing
        // if hiding, lower level must be hidden
        var $sibling = show ? $context.prev(".jurisdiction") : $context.next(".jurisdiction");
        if ($sibling.length && $sibling.hasClass("hidden") == show)
          that.toggleLevel($sibling, show, true);
      }

      if (isHidden && show) setCategoryList(l);
      $context.toggleClass("hidden", !show);
      if (getCategoryLevel($context) === level.parties)
        $(".party-box", $control).toggleClass("hidden", !show);
    };

  }

  SelectJurisdictions.prototype.name = "SelectJurisdiction";

  //
  // Exports
  //

  return {
    SelectJurisdictions: SelectJurisdictions,
    getControlName: getControlName,
    level: level
  };
});