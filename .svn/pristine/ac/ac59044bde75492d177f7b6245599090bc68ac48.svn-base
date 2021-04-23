// Monitor: class to monitor changes to form controls
// see monitor.txt for docs

function Monitor(options) {
  if (Monitor.current)
    throw "Only one instance of Monitor permitted";
  Monitor.current = this;

  this.options = {};
  if (!options) options = {};
  this.options.prefix = options.prefix || "mc";
  this.options.tabprefix = options.tabprefix || "mt";
  this.options.supertabprefix = options.supertabprefix || "ms";

  this.groups = {};
  this.callbacks = {};
  this.g = {}; // for globals
  this.inAjax = false;
}

//
// Class methods
//

Monitor.addGroupFeedback = function (groupName, cssClass, feedback, empty) {
  return Monitor.current.addGroupFeedback(groupName, cssClass, feedback, empty);
};

Monitor.clear = function (event) {
  return Monitor.current.clear(this, event);
};

Monitor.clearGroupFeedback = function (groupName) {
  return Monitor.current.clearGroupFeedback(groupName);
};

Monitor.dataChanged = function(event, prev) {
  return Monitor.current.dataChanged(event, prev);
};

Monitor.endRequest = function (sender, args) {
  return Monitor.current.endRequest(sender, args);
};

Monitor.getChangedGroupNames = function (omitChildren) {
  return Monitor.current.getChangedGroupNames(omitChildren);
};

Monitor.getDataType = function (o) {
  var dataType = null;
  $.each(Monitor.dataTypes, function () {
    if (this.handles(o)) {
      dataType = this;
      return false;
    }
  });
  return dataType;
};

Monitor.getGroupCurrentVal = function (groupName) {
  return Monitor.current.getGroupCurrentVal(groupName);
};

Monitor.initializeRequest = function (sender, args) {
  return Monitor.current.initializeRequest(sender, args);
};

Monitor.isPanelsChanged = function ($panel) {
  return Monitor.current.isPanelsChanged($panel);
};

Monitor.onAfterUpdateGroup = function (group, args) {
  return Monitor.current.onAfterUpdateGroup(group, args);
};

Monitor.registerDataType = function (dataType, before) {
  if (!Monitor.dataTypes) Monitor.dataTypes = [];
  if (before) {
    var registered = false;
    $.each(Monitor.dataTypes, function (index) {
      if (this.name == before) {
        Monitor.dataTypes.splice(index, 0, dataType);
        registered = true;
        return false;
      }
    });
    if (registered) return;
  }
  Monitor.dataTypes.push(dataType);
};

Monitor.safeBind = function (jqo, event, handler) {
  return Monitor.current.safeBind(jqo, event, handler);
};

Monitor.timeCheck = function () {
  return Monitor.current.timeCheck();
};

Monitor.undo = function (event) {
  return Monitor.current.undo(this, event);
};

Monitor.undoPanels = function ($panel) {
  return Monitor.current.undoPanels($panel);
};

Monitor.uploadSuccess = function(data, status) {
  return Monitor.current.uploadSuccess(data, status);
};

Monitor.uploadTimeout = function() {
  return Monitor.current.uploadTimeout();
};

//
// Instance methods
//

Monitor.prototype.addFeedback = function(id, cssClass, feedback, empty) {
  if (id) {
    var $feedback = $$(id + " .feedback");
    if (empty) $feedback.empty();
    $feedback.append('<p class="' + cssClass + '">' + feedback + '</p>');
    this.showFeedback(id);
  }
};

Monitor.prototype.addGroupFeedback = function (groupName, cssClass, feedback, empty) {
  this.addFeedback(this.groups[groupName].feedback, cssClass, feedback, empty);
};

Monitor.prototype.addToGroups = function(element) {
  var $this = this;
  var $element = $(element);
  var groupName = this.getMonitorClass(element);
  var prefix = this.options.prefix + "-";
  var gprefix = this.options.prefix + "-g-";
  var group;
  if (groupName) {
    group = this.groups[groupName] || {};
    this.groups[groupName] = group;
    group.group = groupName;
  }
  if (element.id) {
    $.each(util.getClasses($element), function(index, className) {
      var property;
      if (className.startsWith(gprefix)) { // 
        property = className.substr(gprefix.length);
        $this.g[property] = element.id;
      } else if (group && (className.startsWith(prefix))) {
        property = className.substr(prefix.length);
        var inx = property.indexOf("-");
        if (inx > 0) { // string property
          var pname = property.substr(0, inx);
          if (pname != "group")
            group[pname] = property.substr(inx + 1);
        } else { // id property
          group[property] = element.id;
        }
      }
    });
  }
};

Monitor.prototype.animatedToggle = function($o, state) {
  if (state)
    $o.show("scale", 200);
  else
    $o.hide("scale", 200);
};

Monitor.prototype.animatedToggleClass = function($o, className, state) {
  if (state)
    $o.addClass(className, 200);
  else
    $o.removeClass(className, 200);
};

Monitor.prototype.clear = function(sender, event) {
  if ($(sender).hasClass("disabled")) return;
  var group = this.groups[this.getMonitorClass(sender) || ""];
  this.clearGroup(group);
  return;
};

Monitor.prototype.clearAllFeedback = function () {
  var $this = this;
  $.each(this.groups, function (name, g) {
    $this.clearFeedback(g.feedback);
  });
  this.clearFeedback(this.g.feedbackall);
};

Monitor.prototype.clearFeedback = function (feedbackId) {
  if (!feedbackId) return;
  if (!$$(feedbackId).is(":hidden")) {
    $$(feedbackId + " .hider").click();
    $$(feedbackId + " .feedback").empty();
  }
};

Monitor.prototype.clearFilename = function (groupname) {
  var group = this.groups[groupname];
  if (group && group.filename) {
    this.setFilenamePlaceholder(group);
    if (group.changed) {
      group.val = "";
      group.changed = false;
      // restore clone of original control
      var $cloned = group._clonedData.clone();
      var $old = $$(group.data);
      $cloned.insertBefore($old);
      $old.remove();
      group._dataType.bindChange($cloned);
      this.setUpdateAllEnabling();
      this.fireCallbacks("clientStateChange", group);
      this.onClientStateChange(group);
    }
  }
};

Monitor.prototype.clearGroup = function (group) {
  var $this = this;
  if (group) {
    if (group.data) {
      var $data = $$(group.data);
      var prev = group._dataType.val($data);
      // the unbind/bind gyrations are ugly but they work...
      group._dataType.unbindChange($data);
      group._dataType.val($data, "");
      this.dataChanged({ target: $data[0] }, prev);
      group._dataType.bindChange($data);
    }
    if (group.children) {
      $.each(group.children, function (name, g) {
        $this.clearGroup(g);
      });
    }
  }
};

Monitor.prototype.clearGroupFeedback = function (groupName) {
  this.clearFeedback(this.groups[groupName].feedback);
};

Monitor.prototype.dataChanged = function (event, prev, suppressCallbacks) {
  if (typeof event == "string")
    event = { target: $$(this.groups[event].data)[0] };
  var group;
  var target = event.target;

  // if the target isn't an mc-data, it might be in a CheckBoxList. Find the parent.
  if (!$(target).hasClass("mc-data")) {
    target = $(target).parents(".mc-data")[0];
  }
  
  var id = target.id;

  $.each(this.groups, function (name, g) {
    if (g.data == id) {
      group = g;
      return false;
    }
  });
  if (group) {
    var prevChanged = group.changed;
    var prevVal = !!prev;
    var $data = $(target);
    group._current = group._dataType.val($data);
    if (!group.nomonitor)
      group.changed = group._current != group.val;
    $data.removeClass("error");
    group._dataType.dataChanged(group, $data);
    if (prevChanged != group.changed ||
      prevVal != !!group._current) {
      this.onClientStateChange(group);
    }
    this.onClientChange(group, 
      { childChanged: false, fireCallbacks: suppressCallbacks != true });
  }
};

Monitor.prototype.disableUI = function() {
  $.each(this.groups, function(name, g) {
    if (this.button) $$(this.button).addClass("disabled");
    if (this.undo) $$(this.undo).addClass("disabled");
    if (this.clear) $$(this.clear).addClass("disabled");
  });
  if (this.g.buttonall) $$(this.g.buttonall).addClass("disabled");
};

Monitor.prototype.doUpload = function(group, all) {
  if (group && group.data && this.g.uploadurl && !this.g._uploadId) {
    if (!all)
      this.onBeginAjax(group);
    this.onBeginUpload(group, { all: all });
    this.g._uploadId = "#" + Date.now();
    this.g._uploadGroup = group;
    this.g._uploadAll = all;
    $.ajaxFileUpload({
      url: $$(this.g.uploadurl).val(),
      secureuri: false,
      fileElementId: group.data,
      dataType: "json",
      data: {
        groupname: group.group,
        uploadid: this.g._uploadId
      },
      restoreFilename: true,
      timeout: 60000,
      success: Monitor.uploadSuccess,
      error: Monitor.uploadTimeout
    });
  }
};

Monitor.prototype.enableUI = function () {
  var $this = this;
  var mts = {};
  var mss = {};
  $.each(this.groups, function (name, g) {
    $this.setClientStateEnabling(g, false);
    if (g.star) $$(g.star).toggle(!!g.val);
    if (g.mt) mts[g.mt] = true;
    if (g.ms) mss[g.ms] = true;
  });
  $.each(mts, function (mt, b) {
    $this.setOneAsteriskTabEnabling(mt, "mt");
  });
  $.each(mss, function (ms, b) {
    $this.setOneAsteriskTabEnabling(ms, "ms");
  });
  this.setUpdateAllEnabling();
};

Monitor.prototype.endRequest = function (sender, args) {
  if (args._response._aborted) return;
  var thisOuter = this;

  // get a list of all updated groups -- for each include a list of outer
  // updated groups
  var updates = [];
  var $updated = $(".mc-container.updated");
  $.each($updated, function () {
    var $this = $(this);
    var outerUpdates = [];
    $.each($this.parents(".mc-container.updated"), function () {
      outerUpdates.push(thisOuter.getMonitorClass(this));
    });
    if (!outerUpdates.length) outerUpdates = null;
    updates.push({
      groupName: thisOuter.getMonitorClass($this),
      outerUpdates: outerUpdates
    });
  });

  // We save sender info because in some cases in doesn't come back properly
  // (with js initiated postbacks)
  var group = this._senderGroup;
  var isError = args.get_error() != undefined;
  var errorMessage = "";
  var endAjax = true;

  if (isError) {
    errorMessage = args.get_error().message;
    args.set_errorHandled(true);
  }

  // Call onAfterUpdateGroup and onAfterUpdateContainer for each updated group
  $.each(updates, function () {
    var g = thisOuter.groups[this.groupName];
    var a = { isError: isError, errorMessage: errorMessage };
    thisOuter.onAfterUpdateGroup(g, a);
    thisOuter.onAfterUpdateContainer(g, a);
  });

  if (this._senderId == this.g.buttonall) {
    // following for the global all container
    this.onAfterUpdateContainer(null, { errorMessage: errorMessage });
    if (errorMessage) {
      this.onUpdateError(null, { errorMessage: errorMessage });
    } else {
      var uploadGroup = this.getChangedUploadGroups()[0];
      if (uploadGroup) {
        endAjax = false;
        this.doUpload(uploadGroup, true);
      }
    }
  } else if (errorMessage) {
    this.onUpdateError(group, { errorMessage: errorMessage });
  }

  if (endAjax) this.onEndAjax(group);
  this.fireCallbacks("afterRequest", group, updates);
  $updated.removeClass("updated");
};

Monitor.prototype.fireCallbacks = function (eventName, group, args) {
  var $this = this;
  var callbacks = this.callbacks[eventName];
  var result = true;
  if (callbacks) {
    args = args || {};
    $.each(callbacks, function (index, callback) {
      if (callback.call($this, group, args) === false) {
        result = false;
        return false;
      }
    });
  }
  return result;
};

Monitor.prototype.getAllDataGroups = function() {
  return this.groupsWhere(function(group) {
    return group.data && group._dataType.name != "InputFile";
  });
};

Monitor.prototype.getChangedDataGroups = function () {
  return this.groupsWhere(function (group) {
    return group.data && group.changed && group._dataType != "InputFile";
  });
};

Monitor.prototype.getChangedGroupNames = function (omitChildren) {
  var baseThis = this;
  var groups = this.groupsWhere(function (group) {
    return (!omitChildren || !group.parent) && baseThis.isGroupChanged(group);
  });
  var groupNames = [];
  $.each(groups, function () {
    groupNames.push(this.group);
  });
  return groupNames;
};

Monitor.prototype.getChangedUploadGroups = function() {
  return this.groupsWhere(function(group) {
    return group.changed && group._dataType.name == "InputFile";
  });
};

Monitor.prototype.getGroupCurrentVal = function (groupName) {
  return this.groups[groupName]._current;
};

Monitor.prototype.getMonitorClass = function(element) {
  var $o = element instanceof $ ? element : $(element);
  var result;
  if ($o.hasClass(this.options.prefix)) {
    var prefix = this.options.prefix + "-group-";
    $.each(util.getClasses($o), function(index, value) {
      if (value.startsWith(prefix)) {
        result = value;
        return false;
      }
    });
  }
  return result;
};

Monitor.prototype.groupHasData = function(group) {
  if (group.data && group._dataType.val($$(group.data))) return true;
  var hasData = false;
  if (group.children)
    $.each(group.children, function (index, g) {
      if (g.data && g._dataType.val($$(g.data))) {
      //if (g.data && $$(g.data).valx()) {
        hasData = true;
        return false;
      }
    });
  return hasData;
};

Monitor.prototype.groupHasUpdateError = function (group) {
  if (typeof group == "string")
    group = this.groups[group];
  if (group.data) {
    var $data = $$(group.data);
    if ($data.hasClass("error") || $data.hasClass("badupdate")) return true;
  }
  var outer = group.children ? group : group.parent;
  if (!outer || !outer.container ||
    !$$(outer.container).hasClass("update-all"))
    return false;
  var hasError = false;
  $.each(outer.children, function (name, g) {
    if (g.data) {
      var j = $$(g.data);
      if (j.hasClass("error") || j.hasClass("badupdate")) {
        hasError = true;
        return false;
      }
    }
  });
  return hasError;
};

Monitor.prototype.groupsWhere = function(evaluator) {
  var result = [];
  $.each(this.groups, function(key, group) {
    if (evaluator(group))
      result.push(group);
  });
  return result;
};

Monitor.prototype.hasChanges = function() {
  var changed = false;
  $.each(this.groups, function(name, group) {
    if (group.changed) {
      changed = true;
      return false;
    }
  });
  return changed;
};

Monitor.prototype.init = function (selector) {
  var $this = this;
  // construct groups object
  $("." + this.options.prefix).each(
    function (index) { $this.addToGroups(this); });

  // establish parent-child relationships
  var groupPrefix = this.options.prefix + "-group-";
  $.each(this.groups, function (name, group) {
    var nm = name.substr(groupPrefix.length);
    var inx = nm.indexOf("-");
    if (inx >= 0) { // it is a child
      var parentName = name.substr(0, inx + groupPrefix.length);
      var parent = $this.groups[parentName];
      if (parent) {
        group.parent = parent;
        if (!parent.children) parent.children = [];
        parent.children.push(group);
      }
    }
  });

  // initial processing for each group
  $.each(this.groups, function (name, group) {
    group.changed = false;
    if (group.undo) // attach click handler
      $this.safeBind($$(group.undo), "click", Monitor.undo);
    if (group.clear) // attach click handler
      $this.safeBind($$(group.clear), "click", Monitor.clear);
    if (group.data) {
      var $data = $$(group.data);
      group._dataType = Monitor.getDataType($data);
      group.val = group._dataType.val($data);
      group._dataType.initGroup(group, $data);
      group._dataType.bindChange($data);
    }
  });

  this.enableUI();

  // remove the initial updated flag
  $(".mc-container.updated").removeClass("updated"); 

  $.each(this.groups, function (name, group) {
    $this.onInitGroup(group);
  });

  this.onInitMonitor();

  // attach ajax events
  var prm = window.Sys.WebForms.PageRequestManager.getInstance();
  prm.add_initializeRequest(Monitor.initializeRequest);
  prm.add_endRequest(Monitor.endRequest);

  // set the initial timeCheck to detect autocomplete 
  // and other "undetectable" changes
  setTimeout(Monitor.timeCheck, 500);
};

Monitor.prototype.initializeRequest = function (sender, args) {
  var $o = $(sender._activeElement);
  var reloading = $o.hasClass("reloading");
  $o.removeClass("reloading");
  var groupName = this.getMonitorClass(sender._activeElement) || "";
  var group = this.groups[groupName] || {};
  if (!reloading && ($o.hasClass("disabled") || 
    this.fireCallbacks("initRequest", group) === false)) {
    args.set_cancel(true);
    return;
  }
  this._senderGroup = group;
  this._senderId = sender._activeElement.id;
  var all = this._senderId == this.g.buttonall;
  this.onBeginAjax(all ? null : this._senderGroup);
  if (!all) {
    this.onBeginUpdate(this._senderGroup, { reloading: reloading });
  } else { // clear/hide all
    if (this.getChangedDataGroups().length > 0) { // there's data to update
      this.onBeginUpdate();
    } else // upload only
    {
      this.clearAllFeedback(this.g.feedbackall);
      args.set_cancel(true);
      // there should always be excactly one
      this.doUpload(this.getChangedUploadGroups()[0], true);
    }
  }
};

Monitor.prototype.initTabEnabling = function () {
  var $this = this;
  var mts = {};
  var mss = {};
  $.each(this.groups, function (name, group) {
    if (group.mt) mts[group.mt] = true;
    if (group.ms) mss[group.ms] = true;
  });
  $.each(mts, function (name, value) {
    $this.setOneAsteriskTabEnabling(name, "mt", false);
    $this.setOneStarTabEnabling(name, "mt", false);
  });
  $.each(mss, function (name, value) {
    $this.setOneAsteriskTabEnabling(name, "ms", false);
    $this.setOneStarTabEnabling(name, "ms", false);
  });
};

Monitor.prototype.isGroupChanged = function (group) {
  if (!group) return false;
  if (typeof group == "string")
    group = this.groups[group];
  if (group.changed) return true;
  var changed = false;
  if (group.children)
    $.each(group.children, function (index, g) {
      if (g.changed) {
        changed = true;
        return false;
      }
    });
  return changed;
};

Monitor.prototype.isPanelsChanged = function ($panel) {
  var result = false;
  var outerThis = this;
  $.each($panel, function () {
    if (outerThis.isGroupChanged(outerThis.groups[outerThis.getMonitorClass(this)])) {
      result = true;
      return false;
    }
  });
  return result;
};

Monitor.prototype.onAfterUpdateContainer = function (group, args) {
  if (typeof group == "string")
    group = this.groups[group];
  if (group) {
    if (group.undo)
      this.safeBind($$(group.undo), "click", Monitor.undo);
    if (group.clear)
      this.safeBind($$(group.clear), "click", Monitor.clear);
    if (group.container) {
      this.reqd($$(group.container + " .reqd"));
    }
  }
  this.setTipTip(group);
  this.fireCallbacks("afterUpdateContainer", group, args);
};

Monitor.prototype.onAfterUpdateGroup = function (group, args) {
  args = args || {};
  if (typeof group == "string")
    group = this.groups[group];
  var $this = this;
  if (group.children) {
    $.each(group.children, function (name, g) {
      $this.onAfterUpdateGroup(g, args);
    });
    return;
  }
  if (group.undo)
    this.safeBind($$(group.undo), "click", Monitor.undo);
  if (group.clear)
    this.safeBind($$(group.clear), "click", Monitor.clear);
  if (group.data) {
    var $data = $$(group.data);
    var prevChanged = group.changed;
    var prevVal = group.val ? true : false;
    if ($data.length == 1) {
      var val = group._dataType.val($data);
      if (!args.isError && !this.groupHasUpdateError(group)) {
        group.val = val;
        group.changed = false;
      }
      group._current = val;
      group._dataType.bindChange($data);
    }
    if (prevChanged != group.changed) {
      this.fireCallbacks("clientStateChange", group);
    }
    var newVal = group.val ? true : false;
    if (prevVal != newVal) {
      this.onServerStateChange(group);
    }
    args.errorMessage = args.errorMessage || "";
    this.fireCallbacks("afterUpdateGroup", group, args);
  }
};

Monitor.prototype.onAfterUpload = function (group, args) {
  args = args || {};
  this.clearFilename(group.group);
  this.addFeedback(group.feedback, "ok", args.message, true);
  if (this.g._uploadAll)
    this.addFeedback(this.g.feedbackall, "ok", "Upload complete");
  this.fireCallbacks("afterUpload", args);
};

Monitor.prototype.onBeginAjax = function (group) {
  this.inAjax = true;
  this.disableUI();
  if (group) {
    this.clearFeedback(this.g.feedbackall);
  }
  this.setAjaxSpinnerEnabling(true, group);
  this.fireCallbacks("beginAjax", group);
};

Monitor.prototype.onBeginUpdate = function (group, args) {
  args = args || {};
  if (group) { // single group
    this.clearFeedback(group.feedback);
    this.addFeedback(group.feedback, "ok", args.reloading ? "Loading..." : "Updating...");
  } else {
    this.clearAllFeedback();
    this.addFeedback(this.g.feedbackall, "ok", "Updating all...");
  }
  this.fireCallbacks("beginUpdate", group, args);
};

Monitor.prototype.onBeginUpload = function (group, args) {
  args = args || {};
  if (args.all) {
    this.addFeedback(this.g.feedbackall, "ok", "Beginning upload...", false);
  } else {
    this.addFeedback(group.feedback, "ok", "Beginning upload...", true);
  }
  this.fireCallbacks("beginUpload", group, args);
};

Monitor.prototype.onClientChange = function (group, args) {
  args = args || {};
  if (args.fireCallbacks)
    this.fireCallbacks("clientChange", group, args);
  if (group.parent) {
    this.onClientChange(group.parent, { childChanged: true, fireCallbacks: args.fireCallbacks });
  }
};

Monitor.prototype.onClientStateChange = function (group, args) {
  args = args || {};
  this.setClientStateEnabling(group, true);
  if (group.mt) {
    this.setOneAsteriskTabEnabling(group.mt, "mt", true);
  }
  if (group.ms) {
    this.setOneAsteriskTabEnabling(group.ms, "ms", true);
  }
  this.fireCallbacks("clientStateChange", group, args);
  if (!args.childChanged) {
    if (group.parent) {
      this.onClientStateChange(group.parent, { childChanged: true });
    }
    this.setUpdateAllEnabling(true);
  }
};

Monitor.prototype.onEndAjax = function (group) {
  this.setAjaxSpinnerEnabling(false, group);
  this.enableUI();
  this.fireCallbacks("endAjax", null);
  this.inAjax = false;
};

Monitor.prototype.onInitGroup = function(group) {
  this.fireCallbacks("initGroup", group);
};

Monitor.prototype.onInitMonitor = function () {
  this.setAjaxSpinnerEnabling();
  this.initTabEnabling();
  this.setUpdateAllEnabling();
  this.reqd($(".reqd"));
  this.fireCallbacks("initMonitor", null);
};

Monitor.prototype.onServerStateChange = function (group) {
  if (group.star) {
    $$(group.star).toggle(!!group.val);
    if (group.mt)
      this.setOneStarTabEnabling(group.mt, "mt", false);
    if (group.ms)
      this.setOneStarTabEnabling(group.ms, "ms", false);
  }
  this.fireCallbacks("serverStateChange", group);
};

Monitor.prototype.onUpdateError = function (group, args) {
  args = args || {};
  if (group) {
    if (group.feedback) {
      var msg = "A server error occurred";
      if (group.desc)
        msg += " while updating " + $$(group.desc).val();
      msg += ":<br />" + args.errorMessage;
      this.addFeedback(group.feedback, "ng", msg, true);
    }
  } else {
    this.addFeedback(this.g.feedbackall, "ng",
      "A server error occurred: " + args.errorMessage, true);
  }
  this.fireCallbacks("updateError", group, args);
};

Monitor.prototype.onUploadError = function (group,args) {
  args = args || {};
  if (args.timeout) {
    args.message = "A timeout occurred while uploading your picture. We cannot be sure if the upload was successful.";
  } else {
    args.message = "An error occurred on our servers while uploading your picture: " + args.message;
  }
  this.addFeedback(group.feedback, "ng", args.message, true);
  if (this.g._uploadAll)
    this.addFeedback(this.g.feedbackall, "ng", "Upload error, see upload page.");
  this.fireCallbacks("uploadError", group, args);
};

Monitor.prototype.registerCallback = function (eventName, callback) {
  var callbacks = this.callbacks[eventName] || [];
  callbacks.push(callback);
  this.callbacks[eventName] = callbacks;
};

Monitor.prototype.reqd = function ($all) {
  $all.each(function (index) {
    var $one = $(this);
    var $parent = $one.parent();
    if ($parent.hasClass("fieldlabel")) {
      $parent = $parent.parent();
      if ($parent.hasClass("input-element")) {
        var input = $("input:disabled", $parent);
        if (input.length == 1) {
          $one.hide();
          return;
        }
      }
    }
    $one.addClass("tiptip");
    if (!$one.attr("title"))
      $one.attr("title", "Required field");
  });
};

Monitor.prototype.safeBind = function($o, event, handler) {
  $o.off(event, handler).on(event, handler);
};

Monitor.prototype.setAjaxSpinnerEnabling = function (state, group) {
  var ajaxloader = group && group.ajaxloader ? group.ajaxloader : this.g.ajaxloader;
  if (ajaxloader)
    if (state)
      $$(ajaxloader).show("scale", 400);
    else
      $$(ajaxloader).hide("scale", 400);
};

Monitor.prototype.setClientStateEnabling = function(group, animate) {
  var changed = this.isGroupChanged(group);
  if (group.button) {
    if (!animate)
      $$(group.button).toggleClass("disabled", !changed);
    else
      this.animatedToggleClass($$(group.button), "disabled", !changed);
  }
  if (group.undo) {
    if (!animate)
      $$(group.undo).toggleClass("disabled", !changed);
    else
      this.animatedToggleClass($$(group.undo), "disabled", !changed);
  }
  if (group.clear) {
    if (!animate)
      $$(group.clear).toggleClass("disabled", !this.groupHasData(group));
    else
      this.animatedToggleClass($$(group.clear), "disabled", !this.groupHasData(group));
  }
  if (group.ast) {
    if (!animate)
      $$(group.ast).toggle(changed);
    else
      this.animatedToggle($$(group.ast), changed);
  }
};

Monitor.prototype.setFilename = function(group) {
  if (group.filename) { // update filename -- normalize to show no path in all browsers
    var name = group._dataType.val($$(group.data));
    if (name) {
      var m = null;
      var m1;
      var re = /([\\\/])/g;
      while ((m1 = re.exec(name)) != null) m = m1;
      if (m)
        name = name.substr(m.index + 1);
      $$(group.filename).html(name).removeClass("disabled");
    } else {
      this.setFilenamePlaceholder(group);
    }
  }
};

Monitor.prototype.setFilenamePlaceholder = function(group) {
  $$(group.filename).html("no file selected").addClass("disabled");
};

Monitor.prototype.setOneAsteriskTabEnabling = function(name, type, animate) {
  var $this = this;
  var groups = this.groupsWhere(
    function(g) {
      return g[type] == name;
    });
  var changed = false;
  $.each(groups, function(index, g) {
    if (($this.isGroupChanged(g))) {
      changed = true;
      return false;
    }
  });
  if (!animate)
    $("." + type + "-" + name + " .tab-ast").toggle(changed);
  else
    this.animatedToggle($("." + type + "-" + name + " .tab-ast"), changed);
};

Monitor.prototype.setOneStarTabEnabling = function(name, type, animate) {
  var $this = this;
  var groups = monitor.groupsWhere(
    function(g) {
      return g[type] == name;
    });
  var hasData = false;
  $.each(groups, function(index, g) {
    if (($this.groupHasData(g))) {
      hasData = true;
      return false;
    }
  });
  if (!animate)
    $("." + type + "-" + name + " .tab-star").toggle(hasData);
  else
    monitor.animatedToggle($("." + type + "-" + name + " .tab-star"), hasData);
};

Monitor.prototype.setTipTip = function(group) {
  if (group) {
    if (group.container) {
      initTipTip($$(group.container + " .tiptip"));
    }
  } else {
    initTipTip($$(monitor.g.containerall + " .tiptip"));
  }
};

Monitor.prototype.setUpdateAllEnabling = function(animate) {
  if (this.g.buttonall) {
    var changed = false;
    $.each(this.groups, function(name, g) {
      if (g.changed) {
        changed = true;
        return false;
      }
    });
    if (!animate)
      $$(this.g.buttonall).toggleClass("disabled", !changed);
    else
      this.animatedToggleClass($$(this.g.buttonall), "disabled", !changed);
  }
};

Monitor.prototype.showFeedback = function(id) {
  eval($$(id + " .hider").attr("onclick")
    .replace(".hide(", ".hidex(")
    .replace(".show(", ".hide(")
    .replace(".hidex(", ".show("));
};

Monitor.prototype.timeCheck = function () {
  $.each(this.groups, function () {
    if (this._timecheck) {
      var val = this._dataType.val($$(this.data));
      if (val != this._current)
        $$(this.data).trigger("textchange");
    }
  });
  setTimeout(Monitor.timeCheck, 500);
};

Monitor.prototype.undo = function(sender, event) {
  if ($(sender).hasClass("disabled")) return;
  var group = this.groups[this.getMonitorClass(sender) || ""];
  this.undoGroup(group, true);
  return;
};

Monitor.prototype.undoGroup = function (group, fireCallbacks) {
  var $this = this;
  if (group) {
    if (group.data) {
      var $data = $$(group.data);
      var prev = group._dataType.val($data);
      // the unbind/bind gyrations are ugly but they work...
      group._dataType.unbindChange($data);
      group._dataType.val($data, group.val);
      this.dataChanged({ target: $data[0] }, prev, !fireCallbacks);
      group._dataType.bindChange($data);
    }
    if (group.children) {
      $.each(group.children, function (name, g) {
        $this.undoGroup(g, fireCallbacks);
      });
    }
    if (fireCallbacks)
      this.fireCallbacks("afterUndo", group);
  }
};

Monitor.prototype.undoPanels = function ($panels) {
  var outerThis = this;
  $.each($panels, function () {
    var group = outerThis.groups[outerThis.getMonitorClass(this)];
    if (group) {
      outerThis.clearFeedback(group.feedback);
      outerThis.undoGroup(group, true);
    }
  });
};

Monitor.prototype.upload = function(button) {
  if ($(button).hasClass("disabled")) return;
  var groupname = this.getMonitorClass(button) || "";
  this.doUpload(this.groups[groupname], false);
};

Monitor.prototype.uploadSuccess = function(data, status) {
  if (data.UploadId == this.g._uploadId) {
    var groupname = data.GroupName;
    var group = this.groups[groupname];
    if (data.Success) {
      this.onAfterUpload(group, { duplicate: data.Duplicate, message: data.Message });
    } else {
      this.onUploadError(group, { message: data.Message });
    }
    this.g._uploadId = null;
    this.onEndAjax(group);
  }
};

Monitor.prototype.uploadTimeout = function() {
  if (this.g._uploadId) { // in progress
    var group = this.g._uploadGroup;
    this.g._uploadId = null;
    this.g._uploadGroup = null;
    this.onUploadError(group, { timeout: true });
    this.g._uploadId = null;
    this.onEndAjax(group);
  }
};

//
// DataType objects
//

function DataType() {
}

DataType.prototype.name = "DataType";

DataType.prototype.bindChange = function ($data) {
};

DataType.prototype.dataChanged = function (group, $data) {
};

DataType.prototype.handles = function ($data) {
  return false;
};

DataType.prototype.initControl = function ($data) {
};

DataType.prototype.initGroup = function (group, $data) {
};

DataType.prototype.unbindChange = function ($data) {
};

DataType.prototype.val = function ($data, value) {
  $.error("val() method is missing");
};

//
// TextData
//

TextData.prototype = new DataType();
TextData.prototype.constructor = TextData;
TextData.prototype.parent = DataType.prototype;
function TextData() {}

TextData.prototype.name = "TextData";

TextData.prototype.bindChange = function ($data) {
  Monitor.safeBind($data, "textchange", Monitor.dataChanged);
};

TextData.prototype.handles = function ($data) {
  var o = $data[0];
  var tagName = o.tagName.toLowerCase();
  if (tagName == "textarea") return true;
  if (tagName != "input") return false;
  var type = o.type.toLowerCase();
  return type == "text" || type == "password";
};

TextData.prototype.initGroup = function (group, $data) {
  if ($data[0].tagName != "textarea")
  {
    group._timecheck = true;
    group._current = group.val;
  }
};

TextData.prototype.unbindChange = function ($data) {
  $data.off("textchange", Monitor.dataChanged);
};

TextData.prototype.val = function ($data, value) {
  if (typeof (value) === "undefined") return $data.val();
  return $data.val(value);
};

Monitor.registerDataType(new TextData());

//
// ExpandableTextArea
//

ExpandableTextArea.prototype = new TextData();
ExpandableTextArea.prototype.constructor = ExpandableTextArea;
ExpandableTextArea.prototype.parent = TextData.prototype;
function ExpandableTextArea() {}

ExpandableTextArea.prototype.name = "ExpandableTextArea";

ExpandableTextArea.prototype.dataChanged = function (group, $data) {
  var currentHeight = $data.height();
  var isExpandable = !group.expandable || $$(group.expandable).val() == "true";
  if (!this.height) this.height = currentHeight;
  var $window = $(window);
  var position = $window.scrollTop();
  $data.height(this.height);
  if (isExpandable) {
    var diff = this.getHeight($data) - $data.outerHeight();
    if (diff > 0) $data.height($data.height() + diff);
    }
  $window.scrollTop(position);
};

ExpandableTextArea.prototype.getHeight = function ($data) {
  return $data[0].scrollHeight + parseFloat($data.css("borderTopWidth")) +
    parseFloat($data.css("borderBottomWidth"));
};

ExpandableTextArea.prototype.handles = function ($data) {
  var o = $data[0];
  var tagName = o.tagName.toLowerCase();
  if (tagName != "textarea") return false;
  return $data.hasClass("expandable");
};

ExpandableTextArea.prototype.initGroup = function (group, $data) {
  this.height = 0;
  this.parent.initGroup(group, $data);
};

Monitor.registerDataType(new ExpandableTextArea(), "TextData");

//
// InputFile
//

InputFile.prototype = new DataType();
InputFile.prototype.constructor = InputFile;
InputFile.prototype.parent = DataType.prototype;
function InputFile() {}

InputFile.prototype.name = "InputFile";

InputFile.prototype.bindChange = function ($data) {
  Monitor.safeBind($data, "change", Monitor.dataChanged);
};

InputFile.prototype.dataChanged = function (group, $data) {
  Monitor.current.setFilename(group);
};

InputFile.prototype.handles = function ($data) {
  var o = $data[0];
  return o.tagName.toLowerCase() == "input" && o.type.toLowerCase() == "file";
};

InputFile.prototype.initGroup = function (group, $data) {
  // start filename empty and save control clone
  Monitor.current.clearFilename(group.group);
  group._clonedData = $data.clone();
};

InputFile.prototype.unbindChange = function ($data) {
  $data.off("change", Monitor.dataChanged);
};

InputFile.prototype.val = function ($data, value) {
  if (typeof (value) === "undefined") return $data.val();
  return $data.val(value);
};

Monitor.registerDataType(new InputFile());

//
// Select
//

Select.prototype = new DataType();
Select.prototype.constructor = Select;
Select.prototype.parent = DataType.prototype;
function Select() { }

Select.prototype.name = "Select";

Select.prototype.bindChange = function ($data) {
  Monitor.safeBind($data, "change", Monitor.dataChanged);
};

Select.prototype.dataChanged = function (group, $data) {
  $data.blur();
};

Select.prototype.handles = function ($data) {
  return $data[0].tagName.toLowerCase() == "select";
};

Select.prototype.unbindChange = function ($data) {
  $data.off("change", Monitor.dataChanged);
};

Select.prototype.val = function ($data, value) {
  if (typeof (value) === "undefined") return $data.val();
  return $data.val(value);
};

Monitor.registerDataType(new Select());

//
// CheckBox
//

CheckBox.prototype = new DataType();
CheckBox.prototype.constructor = CheckBox;
CheckBox.prototype.parent = DataType.prototype;
function CheckBox() {}

CheckBox.prototype.name = "CheckBox";

CheckBox.prototype.bindChange = function ($data) {
  Monitor.safeBind($data, "change", Monitor.dataChanged);
};

CheckBox.prototype.handles = function ($data) {
  var o = $data[0];
  return o.tagName.toLowerCase() == "input" && o.type.toLowerCase() == "checkbox";
};

CheckBox.prototype.unbindChange = function ($data) {
  $data.off("change", Monitor.dataChanged);
};

CheckBox.prototype.val = function ($data, value) {
  if (typeof (value) === "undefined") {
    return $data.prop('checked').toString();
  } else {
    $data.prop('checked', value == 'true');
    return $data;
  }
};

Monitor.registerDataType(new CheckBox());

//
// KalyptoCheckBox
//

KalyptoCheckBox.prototype = new DataType();
KalyptoCheckBox.prototype.constructor = KalyptoCheckBox;
KalyptoCheckBox.prototype.parent = DataType.prototype;
function KalyptoCheckBox() {}

KalyptoCheckBox.prototype.name = "KalyptoCheckBox";

KalyptoCheckBox.prototype.bindChange = function ($data) {
  this.initControl($data);
  Monitor.safeBind($data, "rc_checked", Monitor.dataChanged);
  Monitor.safeBind($data, "rc_unchecked", Monitor.dataChanged);
};

KalyptoCheckBox.prototype.handles = function ($data) {
  var o = $data[0];
  if (o.tagName.toLowerCase() != "input" || o.type.toLowerCase() != "checkbox")
    return false;
  return $data.hasClass("kalypto");
};

KalyptoCheckBox.prototype.initControl = function ($data) {
  if (!$data.next().hasClass("kalypto-checkbox"))
    $data.kalypto({ toggleClass: "kalypto-checkbox" });
};

KalyptoCheckBox.prototype.unbindChange = function ($data) {
  $data.off("rc_checked", Monitor.dataChanged);
  $data.off("rc_unchecked", Monitor.dataChanged);
};

KalyptoCheckBox.prototype.val = function ($data, value) {
  this.initControl($data);
  if (typeof(value) === "undefined") {
    return $data.prop('checked').toString();
  } else {
    $data.prop('checked', value == 'true');
    if ($data.hasClass('kalypto')) {
      var groupName = Monitor.current.getMonitorClass($data);
      if (groupName)
        $("a." + groupName).toggleClass("checked", value == 'true');
    }
    return $data;
  }
};

Monitor.registerDataType(new KalyptoCheckBox(), "CheckBox");

//
// CheckBoxList
//

CheckBoxList.prototype = new DataType();
CheckBoxList.prototype.constructor = CheckBoxList;
CheckBoxList.prototype.parent = DataType.prototype;
function CheckBoxList() { }

CheckBoxList.prototype.bindChange = function ($data) {
  this.initControl($data);
  if ($data.hasClass("kalypto")) {
    $.each($('input[type=checkbox]', $data), function() {
      var $this = $(this);
      Monitor.safeBind($this, "rc_checked", Monitor.dataChanged);
      Monitor.safeBind($this, "rc_unchecked", Monitor.dataChanged);
    });
  } else {
    $.each($('input[type=checkbox]', $data), function() {
      Monitor.safeBind($(this), "change", Monitor.dataChanged);
    });
  }
};

CheckBoxList.prototype.handles = function ($data) {
  var o = $data[0];
  return o.tagName.toLowerCase() == "table" && $data.hasClass("check-box-list");
};

CheckBoxList.prototype.initControl = function ($data) {
  if (!$data.hasClass("kalypto")) return;
  $.each($('input[type=checkbox]', $data), function () {
    var $this = $(this);
    if (!$this.next().hasClass("kalypto-checkbox"))
      $this.kalypto({ toggleClass: "kalypto-checkbox" });
  });
};

CheckBoxList.prototype.unbindChange = function ($data) {
  if ($data.hasClass("kalypto")) {
    $.each($('input[type=checkbox]', $data), function() {
      var $this = $(this);
      $this.off("rc_checked", Monitor.dataChanged);
      $this.off("rc_unchecked", Monitor.dataChanged);
    });
  } else {
    $.each($('input[type=checkbox]', $data), function() {
      $(this).off("change", Monitor.dataChanged);
    });
  }
};

CheckBoxList.prototype.val = function ($data, value) {
  this.initControl($data);
  if (typeof (value) === "undefined") {
    var v = "";
    $.each($('input[type=checkbox]', $data), function () {
      v += $(this).prop('checked') ? "1" : "0";
    });
    return v;
  } else {
    var cbs = $('input[type=checkbox]', $data);
    if (cbs.length == value.length)
      $.each(cbs, function (inx) {
        $(this).prop('checked', value.substr(inx, 1) == '1');
      });
    cbs = $('a.kalypto-checkbox', $data);
    if (cbs.length == value.length)
      $.each(cbs, function (inx) {
        $(this).toggleClass("checked", value.substr(inx, 1) == '1');
      });
    return $data;
  }
};

Monitor.registerDataType(new CheckBoxList());

//
// SortableList
//

SortableList.prototype = new DataType();
SortableList.prototype.constructor = SortableList;
SortableList.prototype.parent = DataType.prototype;
function SortableList() {}

SortableList.prototype.name = "SortableList";

SortableList.prototype.bindChange = function ($data) {
  this.initControl($data);
  Monitor.safeBind($data, "sortstop", Monitor.dataChanged);
};

SortableList.prototype.dataChanged = function (group, $data) {
  var id = $data[0].id + "Value";
  $$(id).val(this.val($data));
};

SortableList.prototype.handles = function ($data) {
  var o = $data[0];
  return o.tagName.toLowerCase() == "ul" && $data.hasClass("sortablelist");
};

SortableList.prototype.initControl = function ($data) {
  if (!$data.hasClass("ui-sortable")) {
    $data.sortable({ axis: "y", opacity: 0.5 });
  }
};

SortableList.prototype.unbindChange = function ($data) {
  $data.off("sortstop", Monitor.dataChanged);
};

SortableList.prototype.val = function ($data, value) {
  this.initControl($data);
  if (typeof (value) === "undefined") {
    return $data.sortable('toArray').join('|');
  } else {
    $.each(value ? value.split("|") : [], function (index) {
      var $o = $$(this);
      if (index != $o.index())
        $o.insertBefore($o.parent().children()[index]);
    });
    return $data;
  }
};

Monitor.registerDataType(new SortableList());

//
// Dynatree1
// Value is piped concatention of data.key properties of selected leaf nodes 
//

Dynatree1.prototype = new DataType();
Dynatree1.prototype.constructor = Dynatree1;
Dynatree1.prototype.parent = DataType.prototype;
function Dynatree1() {}

Dynatree1.prototype.name = "Dynatree1";

Dynatree1.prototype.dataChanged = function (group, $data) {
  $data.prev().val(this.val($data));
};

Dynatree1.prototype.handles = function ($data) {
  var o = $data[0];
  return o.tagName.toLowerCase() == "div" && $data.hasClass("dynatree-type-1");
};

Dynatree1.prototype.val = function ($data, value) {
  if (typeof (value) === "undefined") {
    if (typeof initDynatree == "function")
      initDynatree($data);
    var selectedKeys = [];
    $.each($data.dynatree("getSelectedNodes"), function () {
      if (this.countChildren() == 0)
        selectedKeys.push(this.data.key);
    });
    return selectedKeys.join("|");
  } else {
    var keys = value ? value.split("|") : [];
    var $tree = $data.dynatree("getTree");
    $tree._undoing = true;
    $tree.visit(function (node) {
      if (node.countChildren() == 0) {
        if (node.isSelected() != ($.inArray(node.data.key.toString(), keys) >= 0))
          node.select(!node.isSelected());
      }
    });
    $tree._undoing = false;
    return $data;
  }
};

Monitor.registerDataType(new Dynatree1());
