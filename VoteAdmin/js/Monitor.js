define(["jquery", "vote/util", "jqueryui", "textchange", "tiptip", "ajaxfileupload"],
 function ($, util) {

   // P R I V A T E

   var callbacks = {};
   var dataTypes = [];
   var global = {};
   var groups = {};
   //var inAjax;
   var options = {
     prefix: "mc",
     tabprefix: "mt",
     supertabprefix: "ms"
   };

   var senderGroup;
   var senderId;

   var $$ = util.$$;

   var addFeedback = function (id, cssClass, feedback, empty) {
     if (id) {
       var $feedback = $$(id + " .feedback");
       if (empty) $feedback.empty();
       $feedback.append('<p class="' + cssClass + '">' + feedback + '</p>');
       showFeedback(id);
     }
   };

   var addToGroups = function (element) {
     var $element = $(element);
     var groupName = getMonitorClass(element);
     var prefix = options.prefix + "-";
     var gprefix = options.prefix + "-g-";
     var group;
     if (groupName) {
       group = groups[groupName] || {};
       groups[groupName] = group;
       group.group = groupName;
     }
     if (element.id) {
       $.each(util.getClasses($element), function (index, className) {
         var property;
         if (className.startsWith(gprefix)) { // 
           property = className.substr(gprefix.length);
           global[property] = element.id;
         } else if (group && (className.startsWith(prefix))) {
           property = className.substr(prefix.length);
           var inx = property.indexOf("-");
           if (inx > 0) { // string property
             var pname = property.substr(0, inx);
             if (pname !== "group")
               group[pname] = property.substr(inx + 1);
           } else { // id property
             group[property] = element.id;
           }
         }
       });
     }
   };

   var animatedToggle = function ($o, state) {
     if (state)
       $o.show("scale", 200);
     else
       $o.hide("scale", 200);
   };

   var animatedToggleClass = function ($o, className, state) {
     if (state)
       $o.addClass(className, 200);
     else
       $o.removeClass(className, 200);
   };

   var clear = function (event) {
     var $target = $(event.target);
     if ($target.hasClass("disabled")) return;
     var group = groups[getMonitorClass($target) || ""];
     clearGroup(group);
     return;
   };

   var clearAllFeedback = function () {
     $.each(groups, function (name, g) {
       clearFeedback(g.feedback);
     });
     clearFeedback(global.feedbackall);
   };

   var clearGroup = function (group) {
     if (group) {
       if (group.data) {
         var $data = $$(group.data);
         var prev = group._dataType.val($data);
         // the unbind/bind gyrations are ugly but they work...
         group._dataType.unbindChange($data);
         group._dataType.val($data, "");
         dataChanged({ target: $data[0] }, prev);
         group._dataType.bindChange($data);
       }
       if (group.children) {
         $.each(group.children, function (name, g) {
           clearGroup(g);
         });
       }
     }
   };

   var disableUi = function (ajaxMessage) {
     util.openAjaxDialog(ajaxMessage);
   };

   var doUpload = function (group, all) {
     if (group && group.data && global.uploadurl && !global._uploadId) {
       if (!all)
         onBeginAjax(group, "Uploading...");
       onBeginUpload(group, { all: all });
       global._uploadId = "#" + Date.now();
       global._uploadGroup = group;
       global._uploadAll = all;
       var data = $.isPlainObject(group._uploadData)
        ? group._uploadData : {};
       data = $.extend({}, data, {
         groupname: group.group,
         uploadid: global._uploadId
       });
       $.ajaxFileUpload({
         url: $$(global.uploadurl).val(),
         secureuri: false,
         fileElementId: group.data,
         dataType: "json",
         data: data,
         restoreFilename: true,
         timeout: 60000,
         success: uploadSuccess,
         error: uploadTimeout
       });
     }
   };

   var enableUi = function () {
     util.closeAjaxDialog();
     var mts = {};
     var mss = {};
     $.each(groups, function (name, g) {
       setClientStateEnabling(g, false);
       if (g.star) $$(g.star).toggle(!!g.val);
       if (g.mt) mts[g.mt] = true;
       if (g.ms) mss[g.ms] = true;
     });
     $.each(mts, function (mt) {
       setOneAsteriskTabEnabling(mt, "mt");
     });
     $.each(mss, function (ms) {
       setOneAsteriskTabEnabling(ms, "ms");
     });
     setUpdateAllEnabling();
   };

   var endRequest = function (sender, args) {
     if (args._response._aborted) return;

     // get a list of all updated groups -- for each include a list of outer
     // updated groups
     var updates = [];
     var $updated = $(".mc-container.updated");
     $.each($updated, function () {
       var $this = $(this);
       var outerUpdates = [];
       $.each($this.parents(".mc-container.updated"), function () {
         outerUpdates.push(getMonitorClass(this));
       });
       if (!outerUpdates.length) outerUpdates = null;
       updates.push({
         groupName: getMonitorClass($this),
         outerUpdates: outerUpdates
       });
     });

     // We save sender info because in some cases in doesn't come back properly
     // (with js initiated postbacks)
     var group = senderGroup;
     var isError = args.get_error() !== null;
     var errorMessage = "";
     var endAjax = true;

     if (isError) {
       errorMessage = args.get_error().message;
       args.set_errorHandled(true);
     }

     // Call onAfterUpdateGroup and onAfterUpdateContainer for each updated group
     $.each(updates, function () {
       var g = groups[this.groupName];
       var a = { isError: isError, errorMessage: errorMessage };
       onAfterUpdateGroup(g, a);
       onAfterUpdateContainer(g, a);
     });

     if (senderId === global.buttonall) {
       // following for the global all container
       onAfterUpdateContainer(null, { errorMessage: errorMessage });
       if (errorMessage) {
         onUpdateError(null, { errorMessage: errorMessage });
       } else {
         var uploadGroup = getChangedUploadGroups()[0];
         if (uploadGroup) {
           endAjax = false;
           doUpload(uploadGroup, true);
         }
       }
     } else if (errorMessage) {
       onUpdateError(group, { errorMessage: errorMessage });
     }

     if (endAjax) onEndAjax(group);
     fireCallbacks("afterRequest", group, updates);
     $updated.removeClass("updated");
   };

   var fireCallbacks = function (eventName, group, args) {
     var cbacks = callbacks[eventName];
     var result = true;
     if (cbacks) {
       args = args || {};
       $.each(cbacks, function (index, callback) {
         if (callback(group, args) === false) {
           result = false;
           return false;
         }
       });
     }
     return result;
   };

   //  var getAllDataGroups = function () {
   //    return groupsWhere(function (group) {
   //      return group.data && group._dataType.name !== "InputFile";
   //    });
   //  };

   var getChangedDataGroups = function () {
     return groupsWhere(function (group) {
       return group.data && group.changed && group._dataType !== "InputFile";
     });
   };

   var getChangedUploadGroups = function () {
     return groupsWhere(function (group) {
       return group.changed && group._dataType.name === "InputFile";
     });
   };

   var getDataType = function (o) {
     var dataType = null;
     $.each(dataTypes, function () {
       if (this.handles(o)) {
         dataType = this;
         return false;
       }
     });
     return dataType;
   };

   var getMonitorClass = function (element) {
     var $o = element instanceof $ ? element : $(element);
     var result;
     if ($o.hasClass(options.prefix)) {
       var prefix = options.prefix + "-group-";
       $.each(util.getClasses($o), function (index, value) {
         if (value.startsWith(prefix)) {
           result = value;
           return false;
         }
       });
     }
     return result;
   };

   var groupHasData = function (group, forStar) {
     if (group.data) {
       var $data = $$(group.data);
       if (forStar && !$data.hasClass("for-star")) return false;
       if (group._dataType.val($data)) return true;
     }
     var hasData = false;
     if (group.children)
       $.each(group.children, function (index, g) {
         if (g.data) {
           var $d = $$(g.data);
           if (!forStar || $d.hasClass("for-star"))
             if (g._dataType.val($d)) {
               hasData = true;
               return false;
             }
         }
       });
     return hasData;
   };

   var groupsWhere = function (evaluator) {
     var result = [];
     $.each(groups, function (key, group) {
       if (evaluator(group))
         result.push(group);
     });
     return result;
   };

   var initializeRequest = function (sender, args) {
     var $o = $(sender._activeElement);
     var reloading = $o.hasClass("reloading");
     $o.removeClass("reloading");
     var groupName = getMonitorClass(sender._activeElement) || "";
     var group = groups[groupName] || {};
     if (!reloading && ($o.hasClass("disabled") ||
      fireCallbacks("initRequest", group) === false)) {
       args.set_cancel(true);
       return;
     }
     senderGroup = group;
     senderId = sender._activeElement.id;
     var all = senderId === global.buttonall;
     onBeginAjax(all ? null : senderGroup, "Contacting server...");
     if (!all) {
       onBeginUpdate(senderGroup, { reloading: reloading });
     } else { // clear/hide all
       if (getChangedDataGroups().length > 0) { // there's data to update
         onBeginUpdate();
       } else // upload only
       {
         clearAllFeedback(global.feedbackall);
         args.set_cancel(true);
         // there should always be excactly one
         doUpload(getChangedUploadGroups()[0], true);
       }
     }
   };

   var initTabEnabling = function () {
     var mts = {};
     var mss = {};
     $.each(groups, function (name, group) {
       if (group.mt) mts[group.mt] = true;
       if (group.ms) mss[group.ms] = true;
     });
     $.each(mts, function (name) {
       setOneAsteriskTabEnabling(name, "mt", false);
       setOneStarTabEnabling(name, "mt", false);
     });
     $.each(mss, function (name) {
       setOneAsteriskTabEnabling(name, "ms", false);
       setOneStarTabEnabling(name, "ms", false);
     });
   };

   var onAfterUpdateContainer = function (group, args) {
     if (typeof group === "string")
       group = groups[group];
     if (group) {
       if (group.undo)
         util.safeBind($$(group.undo), "click", undo);
       if (group.clear)
         util.safeBind($$(group.clear), "click", clear);
       if (group.container) {
         reqd($$(group.container + " .reqd"));
       }
     }
     setTipTip(group);
     fireCallbacks("afterUpdateContainer", group, args);
   };

   function clearChangedStatus(group) {
     if (typeof group === "string")
       group = groups[group];
     if (group.children) {
       $.each(group.children, function (name, g) {
         clearChangedStatus(g);
       });
     }
     else
       group.val = group._dataType.val($$(group.data));
   }

   var onAfterUpdateGroup = function (group, args) {
     args = args || {};
     if (typeof group === "string")
       group = groups[group];
     if (group.children) {
       $.each(group.children, function (name, g) {
         onAfterUpdateGroup(g, args);
       });
       return;
     }
     if (group.undo)
       util.safeBind($$(group.undo), "click", undo);
     if (group.clear)
       util.safeBind($$(group.clear), "click", clear);
     if (group.data) {
       var $data = $$(group.data);
       var prevChanged = group.changed;
       var prevVal = group.val ? true : false;
       if ($data.length === 1) {
         var val = group._dataType.val($data);
         if (!args.isError && !groupHasUpdateError(group)) {
           group.val = val;
           group.changed = false;
         }
         group._current = val;
         group._dataType.bindChange($data);
       }
       if (prevChanged !== group.changed) {
         fireCallbacks("clientStateChange", group);
       }
       var newVal = group.val ? true : false;
       if (prevVal !== newVal) {
         onServerStateChange(group);
       }
       args.errorMessage = args.errorMessage || "";
       fireCallbacks("afterUpdateGroup", group, args);
     }
   };

   var onAfterUpload = function (group, args) {
     args = args || {};
     clearFilename(group.group);
     addFeedback(group.feedback, "ok", args.message, true);
     if (global._uploadAll)
       addFeedback(global.feedbackall, "ok", "Upload complete");
     fireCallbacks("afterUpload", args);
   };

   var onBeginAjax = function (group, ajaxMessage) {
     disableUi(ajaxMessage);
     if (group) {
       clearFeedback(global.feedbackall);
     }
     fireCallbacks("beginAjax", group);
   };

   var onBeginUpdate = function (group, args) {
     args = args || {};
     if (group) { // single group
       clearFeedback(group.feedback);
       addFeedback(group.feedback, "ok", args.reloading ? "Loading..." : "Updating...");
     } else {
       clearAllFeedback();
       addFeedback(global.feedbackall, "ok", "Updating all...");
     }
     fireCallbacks("beginUpdate", group, args);
   };

   var onBeginUpload = function (group, args) {
     args = args || {};
     if (args.all) {
       addFeedback(global.feedbackall, "ok", "Beginning upload...", false);
     } else {
       addFeedback(group.feedback, "ok", "Beginning upload...", true);
     }
     fireCallbacks("beginUpload", group, args);
   };

   var onClientChange = function (group, args) {
     args = args || {};
     if (args.fireCallbacks)
       fireCallbacks("clientChange", group, args);
     if (group.parent) {
       onClientChange(group.parent, { childChanged: true, fireCallbacks: args.fireCallbacks });
     }
   };

   var onClientStateChange = function (group, args) {
     args = args || {};
     setClientStateEnabling(group, true);
     if (group.mt) {
       setOneAsteriskTabEnabling(group.mt, "mt", true);
     }
     if (group.ms) {
       setOneAsteriskTabEnabling(group.ms, "ms", true);
     }
     fireCallbacks("clientStateChange", group, args);
     if (!args.childChanged) {
       if (group.parent) {
         onClientStateChange(group.parent, { childChanged: true });
       }
       setUpdateAllEnabling(true);
     }
   };

   var onEndAjax = function () {
     enableUi();
     fireCallbacks("endAjax", null);
   };

   var onInitGroup = function (group) {
     fireCallbacks("initGroup", group);
   };

   var onInitMonitor = function () {
     initTabEnabling();
     setUpdateAllEnabling();
     reqd($(".reqd"));
     fireCallbacks("initMonitor", null);
   };

   var onServerStateChange = function (group) {
     if (group.star) {
       $$(group.star).toggle(!!group.val);
       if (group.mt)
         setOneStarTabEnabling(group.mt, "mt", false);
       if (group.ms)
         setOneStarTabEnabling(group.ms, "ms", false);
     }
     fireCallbacks("serverStateChange", group);
   };

   var onUpdateError = function (group, args) {
     args = args || {};
     if (group) {
       if (group.feedback) {
         var msg = "A server error occurred";
         if (group.desc)
           msg += " while updating " + $$(group.desc).val();
         msg += ":<br />" + args.errorMessage;
         addFeedback(group.feedback, "ng", msg, true);
       }
     } else {
       addFeedback(global.feedbackall, "ng",
      "A server error occurred: " + args.errorMessage, true);
     }
     fireCallbacks("updateError", group, args);
   };

   var onUploadError = function (group, args) {
     args = args || {};
     if (args.timeout) {
       args.message = "A timeout occurred while uploading your file. We cannot be sure if the upload was successful.";
     } else {
       args.message = "An error occurred on our servers while uploading your file: " + args.message;
     }
     addFeedback(group.feedback, "ng", args.message, true);
     if (global._uploadAll)
       addFeedback(global.feedbackall, "ng", "Upload error, see upload page.");
     fireCallbacks("uploadError", group, args);
   };

   var reqd = function ($all) {
     $all.each(function () {
       var $one = $(this);
       var $parent = $one.parent();
       if ($parent.hasClass("fieldlabel")) {
         $parent = $parent.parent();
         if ($parent.hasClass("input-element")) {
           var input = $("input:disabled", $parent);
           if (input.length === 1) {
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

   var setClientStateEnabling = function (group, animate) {
     var changed = isGroupChanged(group);
     if (group.button) {
       var enabled = changed || group.alwaysupdate;
       if (!$$(group.button).hasClass("no-disable"))
         if (!animate)
           $$(group.button).toggleClass("disabled", !enabled);
         else
           animatedToggleClass($$(group.button), "disabled", !enabled);
     }
     if (group.undo) {
       if (!animate)
         $$(group.undo).toggleClass("disabled", !changed);
       else
         animatedToggleClass($$(group.undo), "disabled", !changed);
     }
     if (group.clear) {
       if (!animate)
         $$(group.clear).toggleClass("disabled", !groupHasData(group));
       else
         animatedToggleClass($$(group.clear), "disabled", !groupHasData(group));
     }
     if (group.ast) {
       if (!animate)
         $$(group.ast).toggle(changed);
       else
         animatedToggle($$(group.ast), changed);
     }
   };

   var setFilename = function (group) {
     if (group.filename) { // update filename -- normalize to show no path in all browsers
       var name = group._dataType.val($$(group.data));
       if (name) {
         var m = null;
         var m1;
         var re = /([\\\/])/g;
         while ((m1 = re.exec(name)) !== null) m = m1;
         if (m)
           name = name.substr(m.index + 1);
         $$(group.filename).html(name).removeClass("disabled");
       } else {
         setFilenamePlaceholder(group);
       }
     }
   };

   var setFilenamePlaceholder = function (group) {
     $$(group.filename).html("no file selected").addClass("disabled");
   };

   var setOneAsteriskTabEnabling = function (name, type, animate) {
     var gps = groupsWhere(function (g) {
       return g[type] === name;
     });
     var changed = false;
     $.each(gps, function (index, g) {
       if ((isGroupChanged(g))) {
         changed = true;
         return false;
       }
     });
     if (!animate)
       $("." + type + "-" + name + " .tab-ast").toggle(changed);
     else
       animatedToggle($("." + type + "-" + name + " .tab-ast"), changed);
   };

   var setOneStarTabEnabling = function (name, type, animate) {
     var gps = groupsWhere(function (g) {
       return g[type] === name;
     });
     var hasData = false;
     $.each(gps, function (index, g) {
       if ((groupHasData(g, true))) {
         hasData = true;
         return false;
       }
     });
     if (!animate)
       $("." + type + "-" + name + " .tab-star").toggle(hasData);
     else
       animatedToggle($("." + type + "-" + name + " .tab-star"), hasData);
   };

   var setTipTip = function (group) {
     if (group) {
       if (group.container) {
         util.initTipTip($$(group.container + " .tiptip"));
       }
     } else {
       util.initTipTip($$(global.containerall + " .tiptip"));
     }
   };

   var setUpdateAllEnabling = function (animate) {
     if (global.buttonall) {
       var changed = false;
       $.each(groups, function (name, g) {
         if (g.changed) {
           changed = true;
           return false;
         }
       });
       if (!animate)
         $$(global.buttonall).toggleClass("disabled", !changed);
       else
         animatedToggleClass($$(global.buttonall), "disabled", !changed);
     }
   };

   var showFeedback = function (id) {
     eval($$(id + " .hider").attr("onclick")
    .replace(".hide(", ".hidex(")
    .replace(".show(", ".hide(")
    .replace(".hidex(", ".show("));
   };

   var timeCheck = function () {
   };

   var tabsBeforeActivate = function (event, ui, tabsId, containerSelector) {
     var oldContainer = containerSelector ?
      $(containerSelector) :
      $(".mc-container", ui.oldPanel);
     var isChanged = isPanelsChanged(oldContainer);
     if (isChanged) {
       util.confirm("There are unsaved changes on the panel you are leaving.\n\n" +
          "Click OK to discard the changes and continue.\n" +
          "Click Cancel to return to the changed panel.",
          function (button) {
            if (button === "Ok") {
              undoPanels(oldContainer);
              $(util.toCssId(tabsId)).tabs("option", "active",
                util.getTabIndex(tabsId, ui.newPanel[0].id));
            }
          });
       return false;
     }
     return true;
   };

   var undo = function (event) {
     var $target = $(event.target);
     if ($target.hasClass("disabled")) return;
     var group = groups[getMonitorClass($target) || ""];
     undoGroup(group, true);
     return;
   };

   var undoGroup = function (group, doCallbacks) {
     if (typeof group === "string")
       group = groups[group];
     if (group) {
       if (group.data) {
         var $data = $$(group.data);
         var prev = group._dataType.val($data);
         // the unbind/bind gyrations are ugly but they work...
         group._dataType.unbindChange($data);
         group._dataType.val($data, group.val);
         dataChanged({ target: $data[0] }, prev, !doCallbacks);
         group._dataType.bindChange($data);
         //$data.change();
       }
       if (group.children) {
         $.each(group.children, function (name, g) {
           undoGroup(g, doCallbacks);
         });
       }
       if (doCallbacks)
         fireCallbacks("afterUndo", group);
     }
   };

   var uploadSuccess = function (data) {
     if (data.UploadId === global._uploadId) {
       var groupname = data.GroupName;
       var group = groups[groupname];
       if (data.Success) {
         onAfterUpload(group, { duplicate: data.Duplicate, message: data.Message });
       } else {
         onUploadError(group, { message: data.Message });
       }
       global._uploadId = null;
       onEndAjax(group);
     }
   };

   var uploadTimeout = function () {
     if (global._uploadId) { // in progress
       var group = global._uploadGroup;
       global._uploadId = null;
       global._uploadGroup = null;
       onUploadError(group, { timeout: true });
       global._uploadId = null;
       onEndAjax(group);
     }
   };

   // P U B L I C

   var addGroupFeedback = function (groupName, cssClass, feedback, empty) {
     addFeedback(groups[groupName].feedback, cssClass, feedback, empty);
   };

   var clearFeedback = function (feedbackId) {
     if (!feedbackId) return;
     if (!$$(feedbackId).is(":hidden")) {
       $$(feedbackId + " .hider").click();
       $$(feedbackId + " .feedback").empty();
     }
   };

   var clearFilename = function (groupname) {
     var group = groups[groupname];
     if (group && group.filename) {
       setFilenamePlaceholder(group);
       if (group.changed) {
         group.val = "";
         group.changed = false;
         // restore clone of original control
         var $cloned = group._clonedData.clone();
         var $old = $$(group.data);
         $cloned.insertBefore($old);
         $old.remove();
         group._dataType.bindChange($cloned);
         setUpdateAllEnabling();
         fireCallbacks("clientStateChange", group);
         onClientStateChange(group);
       }
     }
   };

   var clearGroupFeedback = function (groupName) {
     clearFeedback(groups[groupName].feedback);
   };

   var dataChanged = function (event, prev, suppressCallbacks) {
     if (typeof event === "string")
       event = { target: $$(groups[event].data)[0] };
     var group;
     var target = event.target;

     // if the target isn't an mc-data, it might be in a CheckBoxList. Find the parent.
     if (!$(target).hasClass("mc-data")) {
       target = $(target).parents(".mc-data")[0];
     }

     var id = target.id;

     $.each(groups, function (name, g) {
       if (g.data === id) {
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
         group.changed = group._current !== group.val;
       $data.removeClass("error");
       group._dataType.dataChanged(group, $data);
       if (prevChanged !== group.changed ||
        (typeof prev != "undefined" && prevVal !== !!group._current)) {
         onClientStateChange(group);
       }
       onClientChange(group,
      { childChanged: false, fireCallbacks: suppressCallbacks !== true });
     }
   };

   var getChangedGroupNames = function (omitChildren) {
     var gps = groupsWhere(function (group) {
       return (!omitChildren || !group.parent) && isGroupChanged(group);
     });
     var groupNames = [];
     $.each(gps, function () {
       groupNames.push(this.group);
     });
     return groupNames;
   };

   var getGroup = function (group) {
     if (typeof group === "string")
       group = groups[group];
     return group;
   };

   var getGroupCurrentVal = function (groupName) {
     return groups[groupName]._current;
   };

   var groupContainsUpdateError = function (group) {
     if (typeof group === "string")
       group = groups[group];
     if (!group.children) return fa;
     var hasError = false;
     $.each(group.children, function (name, g) {
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

   var groupHasUpdateError = function (group) {
     if (typeof group === "string")
       group = groups[group];
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

   var hasChanges = function () {
     var changed = false;
     $.each(groups, function (name, group) {
       if (group.changed) {
         changed = true;
         return false;
       }
     });
     return changed;
   };

   var init = function (opts) {
     $.extend(options, opts);

     // construct groups object
     $("." + options.prefix).each(
    function () { addToGroups(this); });

     // establish parent-child relationships
     var groupPrefix = options.prefix + "-group-";
     $.each(groups, function (name, group) {
       var nm = name.substr(groupPrefix.length);
       var inx = nm.indexOf("-");
       if (inx >= 0) { // it is a child
         var parentName = name.substr(0, inx + groupPrefix.length);
         var parent = groups[parentName];
         if (parent) {
           group.parent = parent;
           if (!parent.children) parent.children = [];
           parent.children.push(group);
         }
       }
     });

     // initial processing for each group
     $.each(groups, function (name, group) {
       group.changed = false;
       if (group.undo) // attach click handler
         util.safeBind($$(group.undo), "click", undo);
       if (group.clear) // attach click handler
         util.safeBind($$(group.clear), "click", clear);
       if (group.data) {
         var $data = $$(group.data);
         group._dataType = getDataType($data);
         group.val = group._dataType.val($data);
         group._dataType.initGroup(group, $data);
         group._dataType.bindChange($data);
       }
     });

     enableUi();

     // remove the initial updated flag
     $(".mc-container.updated").removeClass("updated");

     $.each(groups, function (name, group) {
       onInitGroup(group);
     });

     onInitMonitor();

     // attach ajax events
     var prm = window.Sys.WebForms.PageRequestManager.getInstance();
     prm.add_initializeRequest(initializeRequest);
     prm.add_endRequest(endRequest);

     // set the initial timeCheck to detect autocomplete 
     // and other "undetectable" changes
     setTimeout(timeCheck, 500);
   };

   var isGroupChanged = function (group) {
     if (!group) return false;
     if (typeof group === "string")
       group = groups[group];
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

   var isPanelsChanged = function ($panel) {
     var result = false;
     $.each($panel, function () {
       if (isGroupChanged(groups[getMonitorClass(this)])) {
         result = true;
         return false;
       }
     });
     return result;
   };

   var registerCallback = function (eventName, callback) {
     var cbacks = callbacks[eventName] || [];
     cbacks.push(callback);
     callbacks[eventName] = cbacks;
   };

   var registerDataType = function (dataType, before) {
     if (before) {
       var registered = false;
       $.each(dataTypes, function (index) {
         if (this.name === before) {
           dataTypes.splice(index, 0, dataType);
           registered = true;
           return false;
         }
       });
       if (registered) return;
     }
     dataTypes.push(dataType);
   };

   //  var safeBind = function ($o, event, handler) {
   //    $o.off(event, handler).on(event, handler);
   //  };

   var undoPanels = function ($panels) {
     $.each($panels, function () {
       var group = groups[getMonitorClass(this)];
       if (group) {
         clearFeedback(group.feedback);
         undoGroup(group, true);
       }
     });
   };

   var upload = function (button) {
     if ($(button).hasClass("disabled")) return;
     var groupname = getMonitorClass(button) || "";
     doUpload(groups[groupname], false);
   };

   //
   // DataType objects
   //

   // ReSharper disable once InconsistentNaming
   function DataType() {
   }

   DataType.prototype.name = "DataType";

   DataType.prototype.bindChange = function () {
   };

   DataType.prototype.dataChanged = function () {
   };

   DataType.prototype.handles = function () {
     return false;
   };

   DataType.prototype.initControl = function () {
   };

   DataType.prototype.initGroup = function () {
   };

   DataType.prototype.unbindChange = function () {
   };

   DataType.prototype.val = function () {
     $.error("val() method is missing");
   };

   //
   // TextData
   //

   TextData.prototype = new DataType();
   TextData.prototype.constructor = TextData;
   TextData.prototype.parent = DataType.prototype;
   // ReSharper disable once InconsistentNaming
   function TextData() { }

   TextData.prototype.name = "TextData";

   TextData.prototype.bindChange = function ($data) {
     //util.safeBind($data, "textchange", dataChanged);
     //util.safeBind($data, "change", dataChanged);
     $data.on("propertychange change click keyup input paste", dataChanged);
   };

   TextData.prototype.handles = function ($data) {
     var o = $data[0];
     var tagName = o.tagName.toLowerCase();
     if (tagName === "textarea") return true;
     if (tagName !== "input") return false;
     var type = o.type.toLowerCase();
     return type === "text" || type === "password" || type === "number" || type === "hidden";
   };

   TextData.prototype.initGroup = function (group, $data) {
     if ($data[0].tagName !== "textarea") {
       group._timecheck = true;
       group._current = group.val;
     }
   };

   TextData.prototype.unbindChange = function ($data) {
     //$data.off("textchange", dataChanged);
     //$data.off("change", dataChanged);
     $data.off("propertychange change click keyup input paste", dataChanged);
   };

   TextData.prototype.val = function ($data, value) {
     if (typeof (value) === "undefined") {
       var val = $data.val();
       var placeholder = $data.attr("placeholder");
       if (placeholder && val === placeholder)
         val = "";
       return val;
     }
     return $data.val(value);
   };

   registerDataType(new TextData());

   //
   // TimeSpan
   //

   //   TimeSpan.prototype = new DataType();
   //   TimeSpan.prototype.constructor = TimeSpan;
   //   TimeSpan.prototype.parent = DataType.prototype;
   //   // ReSharper disable once InconsistentNaming
   //   function TimeSpan() { }

   //   TimeSpan.prototype.name = "TimeSpan";

   //   //    TimeSpan.prototype.dataChanged = function (group, $data) {
   //   //      $data.prev().val(this.val($data));
   //   //    };

   //   TimeSpan.prototype.bindChange = function ($data) {
   //     util.safeBind($data, "textchange", dataChanged);
   //   };

   //   TimeSpan.prototype.dataChanged = function (group, $data) {
   //     $(".hours", $data).val(($(".hours", $data).val() || "").replace(/[^\d]/ig, ""));
   //     $(".minutes", $data).val(($(".minutes", $data).val() || "").replace(/[^\d]/ig, ""));
   //     $(".seconds", $data).val(($(".seconds", $data).val() || "").replace(/[^\d]/ig, ""));
   //   };

   //   TimeSpan.prototype.handles = function ($data) {
   //     var o = $data[0];
   //     return o.tagName.toLowerCase() === "div" && $data.hasClass("timespan");
   //   };

   //   TimeSpan.prototype.initGroup = function (group, $data) {
   //     group._timecheck = true;
   //     group._current = group.val;
   //   };

   //   TimeSpan.prototype.unbindChange = function ($data) {
   //     $data.off("textchange", dataChanged);
   //   };

   //   TimeSpan.prototype.val = function ($data, value) {
   //     if (typeof (value) === "undefined") {
   //       try {
   //         return ($(".hours", $data).val() || 0) * 3600 +
   //            ($(".minutes", $data).val() || 0) * 60 +
   //            ($(".seconds", $data).val() || 0) | 0;
   //       } catch (e) {
   //         return 0;
   //       }
   //     }
   //     try {
   //       if (value === 0) {
   //         $(".hours", $data).val("");
   //         $(".minutes", $data).val("");
   //         $(".seconds", $data).val("");
   //       } else {
   //         $(".seconds", $data).val(value % 60);
   //         $(".minutes", $data).val(Math.floor(value / 60) % 60);
   //         $(".hours", $data).val(Math.floor(value / 3600));
   //       }
   //     } catch (e) {
   //       $(".hours", $data).val("");
   //       $(".minutes", $data).val("");
   //       $(".seconds", $data).val("");
   //     }
   //     return $data;
   //   };

   //   registerDataType(new TimeSpan(), "TextData");

   //
   // ExpandableTextArea
   //

   ExpandableTextArea.prototype = new TextData();
   ExpandableTextArea.prototype.constructor = ExpandableTextArea;
   ExpandableTextArea.prototype.parent = TextData.prototype;
   // ReSharper disable once InconsistentNaming
   function ExpandableTextArea() { }

   ExpandableTextArea.prototype.name = "ExpandableTextArea";

   ExpandableTextArea.prototype.dataChanged = function (group, $data) {
     var currentHeight = $data.height();
     var isExpandable = !group.expandable || $$(group.expandable).val() === "true";
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
     if (tagName !== "textarea") return false;
     return $data.hasClass("expandable");
   };

   ExpandableTextArea.prototype.initGroup = function (group, $data) {
     this.height = 0;
     this.parent.initGroup(group, $data);
   };

   registerDataType(new ExpandableTextArea(), "TextData");

   //
   // InputFile
   //

   InputFile.prototype = new DataType();
   InputFile.prototype.constructor = InputFile;
   InputFile.prototype.parent = DataType.prototype;
   // ReSharper disable once InconsistentNaming
   function InputFile() { }

   InputFile.prototype.name = "InputFile";

   InputFile.prototype.bindChange = function ($data) {
     util.safeBind($data, "change", dataChanged);
   };

   InputFile.prototype.dataChanged = function (group) {
     setFilename(group);
   };

   InputFile.prototype.handles = function ($data) {
     var o = $data[0];
     return o.tagName.toLowerCase() === "input" && o.type.toLowerCase() === "file";
   };

   InputFile.prototype.initGroup = function (group, $data) {
     // start filename empty and save control clone
     clearFilename(group.group);
     group._clonedData = $data.clone();
   };

   InputFile.prototype.unbindChange = function ($data) {
     $data.off("change", dataChanged);
   };

   InputFile.prototype.val = function ($data, value) {
     if (typeof (value) === "undefined") return $data.val();
     return $data.val(value);
   };

   registerDataType(new InputFile());

   //
   // Select
   //

   Select.prototype = new DataType();
   Select.prototype.constructor = Select;
   Select.prototype.parent = DataType.prototype;
   // ReSharper disable once InconsistentNaming
   function Select() { }

   Select.prototype.name = "Select";

   Select.prototype.bindChange = function ($data) {
     util.safeBind($data, "change", dataChanged);
   };

   Select.prototype.dataChanged = function (group, $data) {
     $data.blur();
   };

   Select.prototype.handles = function ($data) {
     return $data[0].tagName.toLowerCase() === "select";
   };

   Select.prototype.unbindChange = function ($data) {
     $data.off("change", dataChanged);
   };

   Select.prototype.val = function ($data, value) {
     if (typeof (value) === "undefined") return $data.val();
     return $data.val(value);
   };

   registerDataType(new Select());

   //
   // CheckBox
   //

   CheckBox.prototype = new DataType();
   CheckBox.prototype.constructor = CheckBox;
   CheckBox.prototype.parent = DataType.prototype;
   // ReSharper disable once InconsistentNaming
   function CheckBox() { }

   CheckBox.prototype.name = "CheckBox";

   CheckBox.prototype.bindChange = function ($data) {
     util.safeBind($data, "change", dataChanged);
   };

   CheckBox.prototype.handles = function ($data) {
     var o = $data[0];
     return o.tagName.toLowerCase() === "input" && o.type.toLowerCase() === "checkbox";
   };

   CheckBox.prototype.unbindChange = function ($data) {
     $data.off("change", dataChanged);
   };

   CheckBox.prototype.val = function ($data, value) {
     if (typeof (value) === "undefined") {
       return $data.prop('checked').toString();
     } else {
       $data.prop('checked', value === 'true');
       return $data;
     }
   };

   registerDataType(new CheckBox());

   //
   // KalyptoCheckBox
   //

   KalyptoCheckBox.prototype = new DataType();
   KalyptoCheckBox.prototype.constructor = KalyptoCheckBox;
   KalyptoCheckBox.prototype.parent = DataType.prototype;
   // ReSharper disable once InconsistentNaming
   function KalyptoCheckBox() { }

   KalyptoCheckBox.prototype.name = "KalyptoCheckBox";

   KalyptoCheckBox.prototype.bindChange = function ($data) {
     this.initControl($data);
     util.safeBind($data, "rc_checked", dataChanged);
     util.safeBind($data, "rc_unchecked", dataChanged);
   };

   KalyptoCheckBox.prototype.handles = function ($data) {
     var o = $data[0];
     if (o.tagName.toLowerCase() !== "input" || o.type.toLowerCase() !== "checkbox")
       return false;
     return $data.hasClass("kalypto") /*|| $data.hasClass("kalypto-deferred")*/;
   };

   KalyptoCheckBox.prototype.initControl = function ($data) {
     if (!$data.next().hasClass("kalypto-checkbox"))
       $data.kalypto({ toggleClass: "kalypto-checkbox" });
   };

   KalyptoCheckBox.prototype.unbindChange = function ($data) {
     $data.off("rc_checked", dataChanged);
     $data.off("rc_unchecked", dataChanged);
   };

   KalyptoCheckBox.prototype.val = function ($data, value) {
     this.initControl($data);
     if (typeof (value) === "undefined") {
       return $data.prop('checked').toString();
     } else {
       $data.prop('checked', value === 'true');
       if ($data.hasClass('kalypto')) {
         var groupName = getMonitorClass($data);
         if (groupName)
           $("a." + groupName).toggleClass("checked", value === 'true');
       }
       return $data;
     }
   };

   registerDataType(new KalyptoCheckBox(), "CheckBox");

   //
   // KalyptoRadio
   //

   KalyptoRadio.prototype = new DataType();
   KalyptoRadio.prototype.constructor = KalyptoRadio;
   KalyptoRadio.prototype.parent = DataType.prototype;
   // ReSharper disable once InconsistentNaming
   function KalyptoRadio() { }

   KalyptoRadio.prototype.name = "KalyptoRadio";

   KalyptoRadio.prototype.bindChange = function ($data) {
     this.initControl($data);
     util.safeBind($data, "rc_checked", dataChanged);
     util.safeBind($data, "rc_unchecked", dataChanged);
   };

   KalyptoRadio.prototype.handles = function ($data) {
     return $data.hasClass("kalypto-radio-container");
   };

   KalyptoRadio.prototype.initControl = function ($data) {
     $("input[type=radio]", $data).each(function () {
       if (!$(this).hasClass("kalypto-radio"))
         $(this).kalypto({ toggleClass: "kalypto-radio" });
     });
   };

   KalyptoRadio.prototype.unbindChange = function ($data) {
     $data.off("rc_checked", dataChanged);
     $data.off("rc_unchecked", dataChanged);
   };

   KalyptoRadio.prototype.val = function ($data, value) {
     this.initControl($data);
     if (typeof (value) === "undefined") {
       return $("input:checked", $data).val() || "";
     } else {
       $("input[type=radio]", $data).each(function () {
         var $this = $(this);
         $this.prop("checked", $this.val() === value);
         if ($this.hasClass("kalypto"))
           $this.next().toggleClass("checked", $this.val() === value);
       });
       return $data;
     }
   };

   registerDataType(new KalyptoRadio(), "KalyptoCheckBox");

   //
   // CheckBoxList
   //

   CheckBoxList.prototype = new DataType();
   CheckBoxList.prototype.constructor = CheckBoxList;
   CheckBoxList.prototype.parent = DataType.prototype;
   // ReSharper disable once InconsistentNaming
   function CheckBoxList() { }

   CheckBoxList.prototype.bindChange = function ($data) {
     this.initControl($data);
     if ($data.hasClass("kalypto")) {
       $.each($('input[type=checkbox]', $data), function () {
         var $this = $(this);
         util.safeBind($this, "rc_checked", dataChanged);
         util.safeBind($this, "rc_unchecked", dataChanged);
       });
     } else {
       $.each($('input[type=checkbox]', $data), function () {
         util.safeBind($(this), "change", dataChanged);
       });
     }
   };

   CheckBoxList.prototype.handles = function ($data) {
     var o = $data[0];
     return o.tagName.toLowerCase() === "table" && $data.hasClass("check-box-list");
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
       $.each($('input[type=checkbox]', $data), function () {
         var $this = $(this);
         $this.off("rc_checked", dataChanged);
         $this.off("rc_unchecked", dataChanged);
       });
     } else {
       $.each($('input[type=checkbox]', $data), function () {
         $(this).off("change", dataChanged);
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
       if (cbs.length === value.length)
         $.each(cbs, function (inx) {
           $(this).prop('checked', value.substr(inx, 1) === '1');
         });
       cbs = $('a.kalypto-checkbox', $data);
       if (cbs.length === value.length)
         $.each(cbs, function (inx) {
           $(this).toggleClass("checked", value.substr(inx, 1) === '1');
         });
       return $data;
     }
   };

   registerDataType(new CheckBoxList());

   //
   // SortableList
   //

   SortableList.prototype = new DataType();
   SortableList.prototype.constructor = SortableList;
   SortableList.prototype.parent = DataType.prototype;
   // ReSharper disable once InconsistentNaming
   function SortableList() { }

   SortableList.prototype.name = "SortableList";

   SortableList.prototype.bindChange = function ($data) {
     this.initControl($data);
     util.safeBind($data, "sortstop", dataChanged);
   };

   SortableList.prototype.dataChanged = function (group, $data) {
     var id = $data[0].id + "Value";
     $$(id).val(this.val($data));
   };

   SortableList.prototype.handles = function ($data) {
     var o = $data[0];
     return o.tagName.toLowerCase() === "ul" && $data.hasClass("sortablelist");
   };

   SortableList.prototype.initControl = function ($data) {
     if (!$data.hasClass("ui-sortable")) {
       $data.sortable({ axis: "y", opacity: 0.5 });
     }
   };

   SortableList.prototype.unbindChange = function ($data) {
     $data.off("sortstop", dataChanged);
   };

   SortableList.prototype.val = function ($data, value) {
     this.initControl($data);
     if (typeof (value) === "undefined") {
       return $data.sortable('toArray').join('|');
     } else {
       $.each(value ? value.split("|") : [], function (index) {
         var $o = $$(this);
         if (index !== $o.index())
           $o.insertBefore($o.parent().children()[index]);
       });
       return $data;
     }
   };

   registerDataType(new SortableList());

   //
   // Dynatree1
   // Value is piped concatention of data.key properties of selected leaf nodes 
   //

   Dynatree1.prototype = new DataType();
   Dynatree1.prototype.constructor = Dynatree1;
   Dynatree1.prototype.parent = DataType.prototype;
   // ReSharper disable once InconsistentNaming
   function Dynatree1() { }

   Dynatree1.prototype.name = "Dynatree1";

   Dynatree1.prototype.dataChanged = function (group, $data) {
     $data.prev().val(this.val($data));
   };

   Dynatree1.prototype.handles = function ($data) {
     var o = $data[0];
     return o.tagName.toLowerCase() === "div" && $data.hasClass("dynatree-type-1");
   };

   Dynatree1.prototype.val = function ($data, value) {
     if (typeof (value) === "undefined") {
       if (typeof options.initDynatree === "function")
         options.initDynatree($data);
       var selectedKeys = [];
       $.each($data.dynatree("getSelectedNodes"), function () {
         if (this.countChildren() === 0)
           selectedKeys.push(this.data.key);
       });
       return selectedKeys.join("|");
     } else {
       var keys = value ? value.split("|") : [];
       var $tree = $data.dynatree("getTree");
       $tree._undoing = true;
       $tree.visit(function (node) {
         if (node.countChildren() === 0) {
           if (node.isSelected() !== ($.inArray(node.data.key.toString(), keys) >= 0))
             node.select(!node.isSelected());
         }
       });
       $tree._undoing = false;
       return $data;
     }
   };

   registerDataType(new Dynatree1());

   // I N I T I A L I Z E

   return {
     addGroupFeedback: addGroupFeedback,
     clearChangedStatus: clearChangedStatus,
     clearFeedback: clearFeedback,
     clearFilename: clearFilename,
     clearGroupFeedback: clearGroupFeedback,
     dataChanged: dataChanged,
     getGroup: getGroup,
     getGroupCurrentVal: getGroupCurrentVal,
     getChangedGroupNames: getChangedGroupNames,
     getMonitorClass: getMonitorClass,
     groupContainsUpdateError: groupContainsUpdateError,
     groupHasUpdateError: groupHasUpdateError,
     hasChanges: hasChanges,
     init: init,
     isGroupChanged: isGroupChanged,
     //isInAjax: isInAjax,
     isPanelsChanged: isPanelsChanged,
     registerCallback: registerCallback,
     registerDataType: registerDataType,
     //safeBind: safeBind,
     tabsBeforeActivate: tabsBeforeActivate,
     undoGroup: undoGroup,
     undoPanels: undoPanels,
     upload: upload,

     DataType: DataType
   };
 });