define(["jquery", "vote/adminMaster", "vote/util", "monitor",
  "vote/controls/managePoliticiansPanel", "jqueryui", "slimscroll", "dynatree"],
  function ($, master, util, monitor, managePoliticiansPanel) {

    var $$ = util.$$;

    //
    // Master only
    //

    var masterOnlyStartTab = "";

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

      managePoliticiansPanel.initControl();

      var tabInfo = master.parseFragment("tab-addpolitician", function(info) {
        if (info.tab === "tab-addpolitician") {
          reloadPanel("tab-addpolitician", "reloading");
          return false;
        }
      });
      if (tabInfo.tab === "tab-masteronly")
        masterOnlyStartTab = tabInfo.subTab;
    }

    var onTabsActivate = function (event, ui) {
      var newPanelId = ui.newPanel[0].id;
      switch (newPanelId) {
        default:
          reloadPanel(newPanelId);
          break;
      }
    };

    var onTabsBeforeActivate = function (event, ui) {
      return monitor.tabsBeforeActivate(event, ui, "main-tabs");
    };

    var reloadPanel = function (id, option) {
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