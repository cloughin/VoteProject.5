define(["jquery", "vote/util", "monitor", "dynatree"],
function ($, util, monitor) {

  //
  // Select Office Control
  //

  var options;

  function deselect() {
    var key = getSelectedOffice();
    if (key) {
      $('.selected-office-key').val("");
      $('.select-office-control').dynatree("getTree").getNodeByKey(key).select(false);
      var $officeControl = $(".office-control");
      var $ownerTab = $officeControl.closest(".ui-tabs-panel");
      $(".office-heading", $ownerTab).html("No office selected");
    }
  }

  function isUndeletable() {
    var key = getSelectedOffice();
    if (!key) return false;
    var node = $('.select-office-control').dynatree("getTree").getNodeByKey(key);
    if (!node) return false;
    return node.data.undeletable;
  }

  function getSelectedOffice() {
    return $('.selected-office-key').val() || "";
  }

  function getOfficePositions() {
    var key = getSelectedOffice();
    if (!key) return null;
    var node = $('.select-office-control').dynatree("getTree").getNodeByKey(key);
    if (!node) return false;
    return node.data.positions;
  }

  function initControl(o) {
    if ($.isPlainObject(o))
      options = o;
  }

  function initSelectOfficeTree() {
    var $o = $('.select-office-control');
    if (!$('ul', $o).hasClass("dynatree-container")) {
      $.ui.dynatree.nodedatadefaults["icon"] = false; // Turn off icons by default
      $o.dynatree({
        imagePath: "/images/dynatree/skin-vista",
        selectMode: 1,
        autoCollapse: true,
        noLink: true,
        onClick: function (node, event) {
          if (node.getEventTargetType(event) === "title")
            if (node.countChildren() === 0)
              node.select(true);
            else
              node.toggleExpand();
        },
        onExpand: function (flag, node) {
          $(".select-office-expanded-node").val(flag ? node.data.key : "");
        },
        onQuerySelect: function (flag, dtnode) {
          if (dtnode.data.href) {
            // bypass actual selection
            if ($.isPlainObject(options) && typeof options.onSelect === "function")
              options.onSelect(flag, dtnode);
            return false;
          }
          if (dtnode.data.validated) {
            dtnode.data.validated = false;
            return true;
          }
          var officeKey = dtnode.data.key;
          var oldOfficeKey = getSelectedOffice();
          if (officeKey !== oldOfficeKey) {
            var isChanged = monitor.isPanelsChanged($(".mc-container",
                util.getCurrentTabPanel("main-tabs")));
            if (isChanged) {
              util.confirm("There are unsaved changes on this panel.\n\n" +
                  "Click OK to discard the changes load the new office.\n" +
                  "Click Cancel to return to the changed panel.",
                  function (button) {
                    if (button === "Ok") {
                      dtnode.data.validated = true;
                      dtnode.select(true);
                    }
                  });
              return false;
            }
          }
        },
        onSelect: function (flag, dtnode) {
          var officeKey = dtnode.data.key;
          var oldOfficeKey = getSelectedOffice();
          if (flag) {
            // select
            if (officeKey !== oldOfficeKey) {
              $(".selected-office-key").val(officeKey);
              onClickSelectOffice();
              setOfficeHeading(flag, dtnode);
              if ($.isPlainObject(options) && typeof options.onSelect === "function")
                options.onSelect(flag, dtnode);
            }
          } 
        },
        classNames: {
          container: "dynatree-container shadow-2"
        }
      });
      var tree = $o.dynatree("getTree");
      var expandedNode = tree.getNodeByKey($(".select-office-expanded-node").val());
      if (expandedNode) expandedNode.expand(true);
      if ($.isPlainObject(options) && options.officeKey) {
        $('.selected-office-key').val(options.officeKey);
        var n = $('.select-office-control').dynatree("getTree").selectKey(options.officeKey);
        if ($.isPlainObject(options) && typeof options.onSelect === "function")
          options.onSelect(true, n);
        //options.officeKey = null;
        // wait till things settle down
        window.setTimeout(function () {
          options.officeKey = null;
        }, 1500);
      } else {
        var currentOfficeKey = getSelectedOffice();
        if (currentOfficeKey) {
          tree.selectKey(currentOfficeKey);
        }
        // show the control
        if ($(".show-select-office-panel").val() === "true")
          onClickSelectOffice();
      }
    }
    util.safeBind($(".select-office-toggler"), "click", onClickSelectOffice);
  }

function isInitialized() {
  return $('.select-office-control ul').hasClass("dynatree-container");
}

  function refreshOfficeHeading() {
    var sel = $('.select-office-control').dynatree("getSelectedNodes");
    var desc = sel.length ? sel[0].data.desc : "No office selected";
    var $officeControl = $(".office-control");
    var $ownerTab = $officeControl.closest(".ui-tabs-panel");
    $(".office-heading", $ownerTab).html(desc);
  }

  function setOfficeHeading(flag, node) {
    if (flag) {
      var $officeControl = $(".office-control");
      var $ownerTab = $officeControl.closest(".ui-tabs-panel");
      $(".office-heading", $ownerTab).html(node.data.desc);
    }
  }

  function isTemplateKey(key) {
    return key.indexOf("#") >= 0;
  }

  function isVisible() {
    return $(".select-office-container").css("display") !== "none";
  }

  function onClickSelectOffice() {
    if ($.isPlainObject(options) && typeof options.onToggle === "function")
      options.onToggle();
    toggle();
  }

  function toggle(state) {
    var wasShowing = isVisible();
    if (state === wasShowing) return;
    $(".select-office-container").toggle("slide", function () {
      if (!wasShowing && getSelectedOffice()) {
        var $treeContainer = $(".select-office-control .dynatree-container");
        var selectedOffset = $(".dynatree-selected", $treeContainer).offset();
        if (selectedOffset)
          $treeContainer.scrollTop(selectedOffset.top -
              $treeContainer.offset().top + $treeContainer.scrollTop());
      }
    });
    var $toggle = $('.select-office-toggler');
    if (wasShowing)
      $toggle.removeClass("showing");
    else
      $toggle.addClass("showing");
  }

  return {
    deselect: deselect,
    getOfficePositions: getOfficePositions,
    getSelectedOffice: getSelectedOffice,
    initControl: initControl,
    initSelectOfficeTree: initSelectOfficeTree,
    isInitialized: isInitialized,
    isTemplateKey: isTemplateKey,
    isUndeletable: isUndeletable,
    refreshOfficeHeading: refreshOfficeHeading,
    toggle: toggle
  };
});