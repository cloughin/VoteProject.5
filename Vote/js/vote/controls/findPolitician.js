define(["jquery", "vote/util", "textchange"], function ($, util) {

  var controlName = "find-politician-control";

  //
  // class members
  //

  var getControlName = function () { return controlName; };

  //
  // keep a list of instances so event handlers can establish instance context
  //

  var instances = [];

  var getInstance = function (obj) {
    if (!(obj instanceof $)) {
      // assume event
      obj = $(obj.target);
    }
    var instance = null;
    var $control = obj.closest("." + controlName);
    if ($control.length)
      $.each(instances, function () {
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

  var onClickSearchPolitician = function (event) {
    return getInstance(event).onClickSearchPolitician(event);
  };

  var onDblClickSearchPolitician = function (event) {
    return getInstance(event).onDblClickSearchPolitician(event);
  };

  var onSearchNameChanged = function (event) {
    return getInstance(event).onSearchNameChanged(event);
  };

  //
  // encapsulate an instance in an object to allow multiple
  //
  // ReSharper disable once InconsistentNaming
  function FindPolitician($control, options) {
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

    function getCandidateListSucceeded(result) {
      var $o = $(".search-results-container", $control);
      $o.html(result.d);
      var $list = $(".search-politician", $o);
      $list.safeBind("click", onClickSearchPolitician);
      if (typeof options.onDblClickCandidate === "function")
        $list.safeBind("dblclick", onDblClickSearchPolitician);
      if (typeof options.onNewList === "function")
        options.onNewList($control);
    };

    function clearSelectedCandidates() {
      $(".search-results-container .selected", $control).removeClass("selected");
    }

    //
    // Public methods (privileged)
    //

    this.countSelectedCandidates = function() {
      return $(".search-results-container .selected", $control).length;
    };

    this.destroy = function () {
      // remove from instance list
      var inx = $.inArray(that, instances);
      if (inx !== -1) instances.splice(inx, 1);
    };

    this.getSelectedCandidateAddress = function () {
      var $address = $(".search-results-container .selected .address", $control);
      return $address.length === 1 ? $address.text() : "";
      //return $(".search-results-container .selected .address", $control).text();
    };

    this.getSelectedCandidateKey = function () {
      var $o = $(".search-results-container .selected", $control);
      if ($o.length !== 1) return "";
      var id = $o[0].id;
      return id.substr(id.lastIndexOf('-') + 1);
    };

    this.getSelectedCandidateKeys = function () {
      var $o = $(".search-results-container .selected", $control);
      var result = [];
      $o.each(function () {
        result.push(this.id.substr(this.id.lastIndexOf('-') + 1));
      });
      return result;
    };

    this.getSelectedCandidateName = function () {
      var $name = $(".search-results-container .selected .name", $control);
      return $name.length === 1 ? $name.text() : "";
      //return $(".search-results-container .selected .name", $control).text();
    };

    this.getSelectedCandidateOfficeName = function () {
      var $office = $(".search-results-container .selected .office", $control);
      return $office.length === 1 ? $office.text() : "";
      //return $(".search-results-container .selected .office", $control).text();
    };

    this.getSelectedCandidateSortKey = function () {
      var $o = $(".search-results-container .selected", $control);
      if ($o.length !== 1) return "";
      return $o.attr("sort-key");
    };

    // this is separate from the constructor so it is free to do callbacks
    // that require the client to have the instance reference
    this.init = function () {
      util.safeBind($("input[type=text]", $control), "textchange",
      onSearchNameChanged);
    };

    this.isThisControl = function (control) {
      if (control instanceof $)
        control = control[0];
      return control === $control[0];
    };

    this.onClickSearchPolitician = function (event) {
      var $target = $(event.target).closest(".search-politician");
      var wasSelected = $target.hasClass("selected");
      if (!options.canMultiSelect || !event.ctrlKey) {
        wasSelected &= that.countSelectedCandidates() < 2;
        clearSelectedCandidates();
      }
      $target.toggleClass("selected", !wasSelected);

      if (typeof options.onSelectionChanged === "function")
        options.onSelectionChanged($control, that.getSelectedCandidateKey(), that.getSelectedCandidateKeys());
    };

    this.onDblClickSearchPolitician = function (event) {
      var $target = $(event.target).closest(".search-politician");
      clearSelectedCandidates();
      $target.addClass("selected");
      util.clearSelection();

      if (typeof options.onDblClickCandidate === "function")
        options.onDblClickCandidate($control, that.getSelectedCandidateKey());
    };

    this.onSearchNameChanged = function (noCacheSelected) {
      var idsToSkip = null;
      var stateCode = null;

      if (typeof options.getIdsToSkip === "function")
        idsToSkip = options.getIdsToSkip($control);
      if (!$.isArray(idsToSkip)) idsToSkip = [];

      if (typeof options.getStateCode === "function")
        stateCode = options.getStateCode($control);
      if (typeof stateCode !== "string") stateCode = "";

      util.ajax({
        url: "/Admin/WebService.asmx/GetCandidateList",
        data: {
          partialName: $("input[type=text]", $control).val(),
          selectedPoliticians: that.getSelectedCandidateKeys(),
          officeKey: stateCode,
          currentCandidates: idsToSkip,
          noCacheSelected: !!noCacheSelected
        },
        success: function (result) {
          getCandidateListSucceeded(result);
        },
        error: function (result) {
          getCandidateListSucceeded({ d: "" });
        }
      });
    };

    this.refresh = function (noCacheSelected) {
      that.onSearchNameChanged();
    };

    this.removeSelectedCandidates = function () {
      $(".search-results-container .selected", $control).remove();
    };

    this.reset = function () {
      $("input[type=text]", $control).val("");
      $(".search-results-container", $control).html("");
    };

    this.setLabel = function (label) {
      $(".label", $control).html(label);
    };
  }

  FindPolitician.prototype.name = "FindPolitician";

  return {
    FindPolitician: FindPolitician,
    getControlName: getControlName
  };
});