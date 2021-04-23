define(["jquery", "vote/util"], function($, util) {

  var controlName = "elections-offices-candidates";

  //
  // class members
  //

  var level = {
    elections: "elections",
    offices: "offices",
    candidates: "candidates"
  };

  var getControlName = function() { return controlName; };

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

  var onCandidatesGetListClick = function(event) {
    return getInstance(event).onCandidatesGetListClick(event);
  };

  var onElectionsGetListClick = function(event) {
    return getInstance(event).onElectionsGetListClick(event);
  };

  var onListChange = function(event) {
    return getInstance(event).onListChange(event);
  };

  var onOfficesGetListClick = function(event) {
    return getInstance(event).onOfficesGetListClick(event);
  };

  //
  // encapsulate an instance in an object to allow multiple
  //
  // ReSharper disable once InconsistentNaming
  function ElectionsOfficesCandidates($control, options) {
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
    var jurisdiction = {
      stateCode: "",
      countyCode: "",
      localKey: ""
    };

    //
    // Private methods
    //

    function getCategoryLevel($o) {
      // looks for a magic class
      var classes = $o.classes();
      var result = null;
      $.each(level, function() {
        if (classes.indexOf(this.toString()) !== -1) {
          result = this;
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
        // can be $category, event or string (elections, offices, candidates)
        if (obj instanceof $) return obj.closest(".category");
        if (typeof obj === "string")
          return $("." + obj + ".category", $control);
        // assume event
        return $(obj.target).closest(".category");
      };

    function setCategoryList($context) {
      $context = resolveCategoryContext($context);
      var $prev = $context.prev(".category");
      var enable = $prev.length
        ? $(".checkbox-list input[type=checkbox]:checked", $prev).length === 1
        : !!jurisdiction.stateCode;
      $context.toggleClass("disabled", !enable);
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

    this.isThisControl = function(control) {
      if (control instanceof $)
        control = control[0];
      return control === $control[0];
    };

    this.getCategoryName =
      function($context, code) {
        $context = resolveCategoryContext($context);
        return $(".checkbox-list input[value=" + code + "]", $context).next().text();
      };

    this.getCategoryNames =
      function($context, reportAll) {
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

    this.getSingleCandidateCode = function () {
      var candidates = that.getCategoryCodes(level.candidates);
      return candidates.length === 1 ? candidates[0] : "";
    };

    this.getSingleCandidateName = function () {
      return that.getCategoryName(level.candidates, that.getSingleCandidateCode());
    };

    this.getSingleElectionCode = function () {
      var elections = that.getCategoryCodes(level.elections);
      return elections.length === 1 ? elections[0] : "";
    };

    this.getSingleLocalName = function () {
      return that.getCategoryName(level.elections, that.getSingleElectionCode());
    };

    this.getSingleOfficeCode = function () {
      var offices = that.getCategoryCodes(level.offices);
      return offices.length === 1 ? offices[0] : "";
    };

    this.getSingleStateName = function () {
      return that.getCategoryName(level.offices, that.getSingleOfficeCode());
    };

    // this is separate from the constructor so it is free to do callbacks
    // that require the client to have the instance reference
    this.init = function () {
      $(".category .all", $control).safeBind("change", onAllChange);
      $(".category .checkbox-list", $control).safeBind("change", onListChange);
      $(".category.elections .get-list-button", $control).safeBind("click", onElectionsGetListClick);
      $(".category.offices .get-list-button", $control).safeBind("click", onOfficesGetListClick);
      $(".category.candidates .get-list-button", $control).safeBind("click", onCandidatesGetListClick);
    };

    this.onAllChange = function (event) {
      var $context = $(event.target).closest(".category");
      var checked = $(".all", $context).prop("checked");
      $.each($(".checkbox-list input[type=checkbox]", $context), function() {
        $(this).prop("checked", checked);
      });
      that.onListChange(event);
    };

    this.onCandidatesGetListClick = function () {
      //util.openAjaxDialog("Getting candidates...");
      var $checkedBoxElection = $(".category.elections .checkbox-list input:checked",
        $control);
      var $checkedBoxOffice = $(".category.offices .checkbox-list input:checked",
        $control);
      if ($checkedBoxElection.length === 1 && $checkedBoxOffice.length === 1) {
        $(".category.candidates .get-list", $control).addClass("hidden");
        util.ajax({
          url: "/Admin/WebService.asmx/GetCandidates",
          data: {
            electionKey: $checkedBoxElection.val(),
            officeKey: $checkedBoxOffice.val()
          },

          success: function(result) {
            var $list = $(".category.candidates .checkbox-list", $control);
            var items = result.d;
            if (items.length && !items[0].Value)
              items.shift();
            if (items.length) {
              populateList($list, items);
              $(".category.candidates .all", $control).prop("disabled", false);
            } else {
              $list.html('<em>No candidate information is available</em>');
            }
            updateListLabel($list);
            $list.removeClass("hidden");
            //util.closeAjaxDialog();
          },

          error: function(result) {
            util.alert(util.formatAjaxError(result, "Could not get candidates"));
            //util.closeAjaxDialog();
          }
        });

      }
    };

    this.onElectionsGetListClick = function () {
      //util.openAjaxDialog("Getting elections...");
      $(".category.elections .get-list", $control).addClass("hidden");
      util.ajax({
        url: "/Admin/WebService.asmx/GetElections",
        data: jurisdiction,

        success: function(result) {
          var $list = $(".category.elections .checkbox-list", $control);
          var items = result.d;
          if (items.length && !items[0].Value)
            items.shift();
          if (items.length) {
            populateList($list, items);
            $(".category.elections .all", $control).prop("disabled", false);
          } else {
            $list.html('<em>No election information is available</em>');
          }
          updateListLabel($list);
          $list.removeClass("hidden");
          //util.closeAjaxDialog();
        },

        error: function(result) {
          util.alert(util.formatAjaxError(result, "Could not get elections"));
          //util.closeAjaxDialog();
        }
      });
    };

    this.onListChange = function (event) {
      var $context = resolveCategoryContext(event);

      // analyze list
      var checked = that.getCategoryCodes($context).length;
      var unchecked = that.getCategoryCodesNotChecked($context).length;

      // update the all checkbox
      var $all = $(".all", $context);
      $all.prop("checked", unchecked === 0);

      updateListLabel($context);

      // if exactly one selection, then enable next sibling selection
      // otherwise, clear any next sibling selection
      var $c = $context;
      while (($c = $c.next(".category")).length) {
        setCategoryList($c);
        if (checked === 1) break; // only continue if disabling
      }

      if (typeof options.onChange === "function")
        options.onChange($control, getCategoryLevel($context));
    };

    this.onOfficesGetListClick = function () {
      //util.openAjaxDialog("Getting offices...");
      var $checkedBox = $(".category.elections .checkbox-list input:checked",
        $control);
      if ($checkedBox.length === 1) {
        $(".category.offices .get-list", $control).addClass("hidden");
        util.ajax({
          url: "/Admin/WebService.asmx/GetOffices",
          data: {
            electionKey: $checkedBox.val()
          },

          success: function(result) {
            var $list = $(".category.offices .checkbox-list", $control);
            var items = result.d;
            if (items.length && !items[0].Value)
              items.shift();
            if (items.length) {
              populateList($list, items);
              $(".category.offices .all", $control).prop("disabled", false);
            } else {
              $list.html('<em>No office information is available</em>');
            }
            updateListLabel($list);
            $list.removeClass("hidden");
            //util.closeAjaxDialog();
          },

          error: function(result) {
            util.alert(util.formatAjaxError(result, "Could not get offices"));
            //util.closeAjaxDialog();
          }
        });

      }
    };

    this.setJurisdiction = function (stateCode, countyCode, localKey) {
      stateCode = stateCode || "";
      countyCode = countyCode || "";
      localKey = localKey || "";
      if (stateCode !== jurisdiction.stateCode ||
        countyCode !== jurisdiction.countyCode ||
        localKey !== jurisdiction.localKey) {
        if (!stateCode) countyCode = "";
        if (!countyCode) localKey = "";
        jurisdiction = {
          stateCode: stateCode,
          countyCode: countyCode,
          localKey: localKey
        };
        that.toggleLevel(level.candidates, false);
        that.toggleLevel(level.offices, false);
        that.toggleLevel(level.elections, !!stateCode);
      }
    };

    this.toggleLevel = function (l, enable, force) {
      enable = !!enable; // true bool
      var $context = resolveCategoryContext(l);
      var isDisabled = $context.hasClass("disabled");

      if (!force) {
        // if enabled, higher level must be enabled
        // if disabled, lower level must be disabled
        var $sibling = enable ? $context.prev(".category") : $context.next(".category");
        if ($sibling.length && $sibling.hasClass("disabled") == enable)
          that.toggleLevel($sibling, enable, true);
      }

      if (isDisabled && enable || !enable) setCategoryList(l);
      $context.toggleClass("disabled", !enable);
    };
  }

  ElectionsOfficesCandidates.prototype.name = "ElectionsOfficesCandidates";

  return {
    ElectionsOfficesCandidates: ElectionsOfficesCandidates,
    getControlName: getControlName,
    level: level
  };
});