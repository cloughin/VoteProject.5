define(["jquery", "vote/adminMaster", "vote/util", "monitor",
  "jqueryui", "slimscroll", "dynatree"],
  function ($, master, util, monitor) {

    var $$ = util.$$;

    //
    // Misc
    //
   
    function initPage() {
      monitor.init();

      window.onbeforeunload = function () {
        var changedGroups = monitor.getChangedGroupNames(true);
        if (changedGroups.length > 0)
          return "There are entries on your form that have not been submitted";
      };

      $$('main-tabs')
        .safeBind("tabsbeforeactivate", onTabsBeforeActivate)
        .safeBind("tabsactivate", onTabsActivate);
    }

    function onTabsActivate(event, ui) {
      var newPanelId = ui.newPanel[0].id;
      switch (newPanelId) {
        default:
          reloadPanel(newPanelId);
          break;
      }
    }

    function onTabsBeforeActivate(event, ui) {
      return monitor.tabsBeforeActivate(event, ui, "main-tabs");
    };

    function reloadPanel(id, option) {
      if (!option) option = "reloading";
      switch (id) {
        default:
          $$(id + ' .reloading').val(option);
          $$(id + ' input.update-button').addClass("reloading").click();
          break;
      }
    };

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });