define(["jquery", "vote/util", "store", "jqueryui", "resizablecolumns", 
  "stupidtable"],
  function ($, util, store) {

    var $$ = util.$$;

    //
    // Shared Methods
    //

    var $activeDialog = null;
    var templates = null;

    var init = function () {
      initConfirm();
    initRename();
      initContextMenu();
      initOpen();
      initSaveAs();
    };

    var doDelete = function () {
      var item = getSelectedTemplateItem();
      util.openAjaxDialog("Deleting template...");
      util.ajax({
        url: "/Admin/WebService.asmx/DeleteEmailTemplate",
        data: {
          id: item.Id
        },

        success: function () {
          var $tr = getSelectedTemplateRow();
          var i = getTemplateItemFromRow($tr);

          // delete from the array
          var inx = templates.indexOf(i);
          if (inx >= 0)
            templates.splice(inx, 1);

          refreshDisplay();
          util.closeAjaxDialog();
        },

        error: function (result) {
          var i = getSelectedTemplateItem();
          var message = 'Could not delete "' + util.htmlEscape(i.Name) + '"';
          util.alert(util.formatAjaxError(result, message));
          util.closeAjaxDialog();
        }
      });
    };

    var getDisplayRow = function (target) {
      if (!$activeDialog) return $();
      return $(target).closest("tbody tr", $activeDialog);
    };

    var getSelectedTemplateItem = function () {
      return getTemplateItemFromRow(getSelectedTemplateRow());
    };

    var getSelectedTemplateRow = function () {
      if (!$activeDialog) return $();
      return $(".display-box tr.selected", $activeDialog);
    };

    var getTemplateItemFromRow = function ($tr) {
      var item = null;
      // get the template server id from the row id
      var id = $tr.attr("id");
      id = parseInt(id.substr(id.indexOf("_") + 1));
      $.each(templates, function () {
        if (this.Id === id) {
          item = this;
          return false;
        }
      });
      return item;
    };

    var onClickDisplay = function (event) {
      var $tr = getDisplayRow(event.target);
      if ($tr.length) {
        if ($tr.hasClass("selected"))
          $tr.removeClass("selected");
        else {
          $("tr.selected", $tr.closest("tbody")).removeClass("selected");
          $tr.addClass("selected");
          $('.template-name', $activeDialog)
            .val(getTemplateItemFromRow($tr).Name);
        }
      }
    };

    var onContextMenuDisplay = function (event, $menu) {
      var $tr = getDisplayRow(event.target);
      if ($tr.length) {
        $("tr.selected", $tr.closest("tbody")).removeClass("selected");
        $tr.addClass("selected");
        var item = getTemplateItemFromRow($tr);
        $("li", $menu).toggleClass("disabled", !item.IsOwner);
        $(".public .icon", $menu).toggleClass("checked", item.IsPublic);
        return true;
      }
    };

    var refreshDisplay = function () {
      if (!templates) return;

      var isSaveAsDialog = $activeDialog[0] === $saveAsDialog[0];
      var isOpenDialog = !isSaveAsDialog;
      var dirs = $.fn.stupidtable.dir;
      var idPrefix = isSaveAsDialog ? "etsaid_" : "etoid_";

      var savedSort = util.saveSort($('table', $activeDialog), { col: ".name", dir: dirs.ASC });

      var available = null;
      if (isOpenDialog && openOptions.requirements && showCompatibleOnly) {
        available = openOptions.requirements;
      }

      // create rows
      var rows = [];
      var oddEven = "even";
      $.each(templates, function () {
        // check requirements if selected
        if (available && !isAvailable(available, this.Requirements)) return;
        // skip if open dialog, not owner, and not showing all
        if (isOpenDialog && !this.IsOwner && !showAll) return;
        var name = util.htmlEscape(this.Name);
        var moddate = moment(this.ModTime);
        var lastuseddate = moment(this.LastUsedTime);
        var lastuseddisplay = "";
        if (lastuseddate.year() > 1900)
          lastuseddisplay = lastuseddate.format("M/D/YYYY h:mm:ss A");
        oddEven = oddEven === "even" ? "odd" : "even";
        var ownerCell = isOpenDialog && showAll
          ? '<td><div class="owner">' + this.Owner + '</div></td>'
          : '';
        rows.push('<tr class="' + oddEven + (this.IsOwner ? ' owned' : '') +
          '" id="' + idPrefix + this.Id + '">' +
          '<td><div class="name" title="' + name + '">' + name + '</div></td>' +
          ownerCell +
          '<td data-sort-value="' + (this.IsPublic ? '1' : '0') +
          '"><div class="public">' + (this.IsPublic ? '√' : '&nbsp;') + '</div></td>' +
          '<td data-sort-value="' + moddate.format("X") + '"><div class="moddate">' +
          moddate.format("M/D/YYYY h:mm:ss A") + '</div></td>' +
          '<td data-sort-value="' + lastuseddate.format("X") + '"><div class="lastuseddate">' +
          lastuseddisplay + '</div></td>' +
          '</tr>');
      });

      // combine with heading
      var ownerHead = isOpenDialog && showAll
        ? '<th class="owner" data-sort="string-ins" data-resizable-column-id="owner"><div class="label">Owner</div><div class="sort-ind"></th>'
        : '';
      var resizeId = isSaveAsDialog
        ? "template-save-as"
        : "template-open-" + (showAll ? "all" : "owner");
      var tableHtml = '<table data-resizable-columns-id="' + resizeId + '"><thead><tr>' +
        '<th class="name" data-sort="string-ins" data-resizable-column-id="name"><div class="label">Name</div><div class="sort-ind"></th>' +
        ownerHead +
        '<th class="public" data-sort="int" data-resizable-column-id="public"><div class="label">Public</div><div class="sort-ind"></th>' +
        '<th class="moddate" data-sort="int" data-resizable-column-id="moddate"><div class="label">Date modified</div><div class="sort-ind"></th>' +
        '<th class="lastuseddate" data-sort="int" data-resizable-column-id="lastuseddate"><div class="label">Last used</div><div class="sort-ind"></th>' +
        '</tr></thead><tbody>' + rows.join("") + '</tbody></table>';
      $('.display-box', $activeDialog).html(tableHtml);

      // attach events
      var $table = $('table', $activeDialog);
      $table.safeBind("click", onClickDisplay).resizableColumns({ store: store });
      if (isOpenDialog)
        $table.safeBind("dblclick", onDblClickDisplay);
      var table = $table.stupidtable();
      table.safeBind("aftertablesort", function (event, data) {
        $table.find("th div.sort-ind").removeClass("asc desc");
        var dir = data.direction === dirs.ASC ? "asc" : "desc";
        $("div.sort-ind", $table.find("th").eq(data.column)).addClass(dir);
        // redo alternate coloring class
        //util.assignAlternatingClassesInTable($table);
        util.assignRotatingClassesToChildren($table, ["odd", "even"]);
      });

      util.restoreSort($table, savedSort);
    };

    //
    // The Confirm Dialog
    //

    var confirmDialogName = "email-template-open-dialog-confirm";
    var confirmOpenOptions = null;
    var $confirmDialog;

    var closeConfirm = function () {
      $confirmDialog.dialog("close");
    };

    var initConfirm = function () {
      $confirmDialog = $$(confirmDialogName);
      $confirmDialog.dialog({
        autoOpen: false,
        close: onCloseConfirm,
        dialogClass: "email-template-dialog " + confirmDialogName,
        modal: true,
        resizable: false,
        title: "Save changes?",
        width: "auto"
      });
      $('.save-button', $confirmDialog).safeBind("click", onConfirmSave);
      $('.dont-save-button', $confirmDialog).safeBind("click", onConfirmDontSave);
      $('.cancel-button', $confirmDialog).safeBind("click", closeConfirm);
    };

    var onCloseConfirm = function () {
      if (typeof confirmOpenOptions.cancel === "function")
        confirmOpenOptions.cancel(confirmOpenOptions);
      confirmOpenOptions = null;
    };

    var onConfirmSave = function () {
      if (typeof confirmOpenOptions.save === "function")
        confirmOpenOptions.save(confirmOpenOptions);
      closeConfirm();
    };

    var onConfirmDontSave = function () {
      if (typeof confirmOpenOptions.dontSave === "function")
        confirmOpenOptions.dontSave(confirmOpenOptions);
      closeConfirm();
    };

    var confirmOpen = function (options) {
      confirmOpenOptions = options || {};
      var message = confirmOpenOptions.templateName
        ? 'Do you want to save changes to "' + options.templateName + '"?'
        : 'Do you want to save changes to your current template?';
      $('.message', $confirmDialog).safeHtml(message);
      $confirmDialog.dialog("open");
    };

    //
    // The Rename Dialog
    //

    var renameDialogName = "email-template-rename-dialog";
    var $renameDialog;

    var closeRename = function () {
      $renameDialog.dialog("close");
    };

    var initRename = function () {
      $renameDialog = $$(renameDialogName);
      $renameDialog.dialog({
        autoOpen: false,
        dialogClass: "email-template-dialog " + renameDialogName,
        modal: true,
        resizable: false,
        title: "Rename Template",
        width: "auto"
      });
      $('.cancel-button', $renameDialog).safeBind("click", closeRename);
      $('.rename-button', $renameDialog).safeBind("click", doRename);
    };

    var doRename = function () {
      var item = getSelectedTemplateItem();
      if (item) {
        var newName = $.trim($(".new-name input", $renameDialog).val());
        if (!newName) return;
        var lcNewName = newName.toLowerCase();
        if (lcNewName === item.Name.toLowerCase()) {
          util.alert("The new name is the same as the old name.");
          return;
        }
        var dup = false;
        $.each(templates, function () {
          if (lcNewName === this.Name.toLowerCase()) {
            dup = true;
            return false;
          }
        });
        if (dup) {
          util.alert("You already have a template with this name.");
          return;
        }

        util.openAjaxDialog("Renaming...");
        util.ajax({
          url: "/Admin/WebService.asmx/RenameEmailTemplate",
          data: {
            id: item.Id,
            newName: newName
          },

          success: function (result) {
            getSelectedTemplateItem().Name = result.d;
            refreshDisplay();
            closeRename();
            util.closeAjaxDialog();
          },

          error: function (result) {
            util.alert(util.formatAjaxError(result, 'Could not rename template'));
            util.closeAjaxDialog();
          }
        });
      }
    };

    var openRename = function (item) {
      if (item) {
        $(".original", $renameDialog).safeHtml(item.Name);
        $(".new-name input", $renameDialog).val(item.Name);
        $renameDialog.dialog("open");
        $(".new-name input", $renameDialog).select();
      }
    };

    //
    // The Context Menu

    var contextMenuName = "email-template-dialog-context-menu";
    var $contextMenu;

    var initContextMenu = function() {
      $contextMenu = $$(contextMenuName);
      util.onContextMenu($contextMenu, onContextMenuDisplay);
      $('li.public', $contextMenu).safeBind("click", onClickMenuPublic);
      $('li.rename', $contextMenu).safeBind("click", onClickMenuRename);
      $('li.delete', $contextMenu).safeBind("click", onClickMenuDelete);
    };

    var onClickMenuDelete = function (event) {
      if (util.isMenuItemDisabled(event)) return;
      var item = getSelectedTemplateItem();
      if (item) {
        util.confirm('Are you sure you want to delete "' +
         util.htmlEscape(item.Name) + '"?\n' +
          'This action cannot be undone.',
          function (button) {
            if (button === "Ok")
              doDelete();
          });
      }
    };

    var onClickMenuPublic = function (event) {
      if (util.isMenuItemDisabled(event)) return;
      var item = getSelectedTemplateItem();
      if (item) {
        var data =
        {
          id: item.Id,
          isPublic: !item.IsPublic
        };
        util.openAjaxDialog("Setting template to " + (data.isPublic ? "public" : "private") + "...");
        util.ajax({
          url: "/Admin/WebService.asmx/SetEmailTemplatePublicFlag",
          data: data,

          success: function () {
            var $tr = getSelectedTemplateRow();
            var i = getTemplateItemFromRow($tr);
            i.IsPublic = !item.IsPublic;
            $("div.public", $tr).html((i.IsPublic ? '√' : '&nbsp;'));
            util.closeAjaxDialog();
          },

          error: function (result) {
            var i = getSelectedTemplateItem();
            var message = 'Could not change "' + util.htmlEscape(i.Name) +
              '" to ' + (i.isPublic ? 'Private' : 'Public');
            util.alert(util.formatAjaxError(result, message));
            util.closeAjaxDialog();
          }
        });
      }
    };

    var onClickMenuRename = function (event) {
      if (util.isMenuItemDisabled(event)) return;
      var item = getSelectedTemplateItem();
      if (item) {
        openRename(item);
      }
    };

    //
    // The Open Dialog
    //

    var openDialogName = "email-template-open-dialog";
    var $openDialog;
    var openOptions = null;
    var showAll = false;
    var showCompatibleOnly = false;

    var initOpen = function() {
      $openDialog = $$(openDialogName);
      $openDialog.dialog({
        autoOpen: false,
        close: onCloseOpenDialog,
        dialogClass: 'email-template-dialog email-template-open-dialog',
        modal: true,
        resizable: false,
        title: "Open Email Template",
        width: "auto"
      });
      $('.cancel-button', $openDialog).safeBind("click", closeOpenDialog);
      $('.open-button', $openDialog).safeBind("click", onClickOpenButton);
      $('.compatible-box input[type=checkbox]', $openDialog).safeBind("change", onChangeCompatibleCheckbox);
      $('.radio-box input[type=radio]', $openDialog).safeBind("change", onChangeShowRadio);
    };

    var closeOpenDialog = function () {
      $openDialog.dialog("close");
    };

    var isAvailable = function (available, requirements) {
      var result = true;
      if (requirements)
        $.each(requirements.split(","), function () {
          if ($.inArray(this.toString(), available) < 0) {
            result = false;
            return false;
          }
        });
      return result;
    };

    var onChangeCompatibleCheckbox = function () {
      showCompatibleOnly = $(".compatible-checkbox", $openDialog).prop("checked");
      refreshDisplay();
    };

    var onChangeShowRadio = function () {
      showAll = $('input:radio[value="All"]', $openDialog).prop('checked');
      refreshDisplay();
    };

    var onClickOpenButton = function () {
      openRow($("table tr.selected", $openDialog));
    };

    var onCloseOpenDialog = function () {
      if (typeof openOptions.cancel === "function")
        openOptions.cancel(openOptions);
      openOptions = null;
      $activeDialog = null;
    };

    var onDblClickDisplay = function (event) {
      openRow($(event.target).closest("table tr", $openDialog));
    };

    var openOpenDialog = function (options) {
      openOptions = options || {};
      var $compatible = $('.compatible-box input[type=checkbox]', $openDialog);
      if (!openOptions.requirements) {
        $compatible.prop('checked', false).prop('disabled', true);
        showCompatibleOnly = false;
      } else
        $compatible.prop('checked', showCompatibleOnly).prop('disabled', false);
      $(".compatible-box label", $openDialog).toggleClass("disabled", $compatible.prop("disabled"));

      setShowRadio(showAll);

      util.openAjaxDialog("Getting template list...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetEmailTemplateInfo",
        data: {
          allPublic: true
        },

        success: function (result) {
          util.closeAjaxDialog();
          templates = result.d;
          $activeDialog = $openDialog;
          refreshDisplay();
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "Could not get template data from server"));
          closeOpenDialog();
        }
      });
      $openDialog.dialog("open");
    };

    var openRow = function ($tr) {
      if ($tr.length === 1) {
        // get the template server id from the row id
        var id = $tr.attr("id");
        id = parseInt(id.substr(id.indexOf("_") + 1));

        util.openAjaxDialog("Opening template...");
        util.ajax({
          url: "/Admin/WebService.asmx/OpenEmailTemplate",
          data: {
            id: id
          },

          success: function (result) {
            util.closeAjaxDialog();
            if (typeof openOptions.success === "function")
              openOptions.success(result, openOptions);
            closeOpenDialog();
          },

          error: function (result) {
            util.closeAjaxDialog();
            if (typeof openOptions.error === "function")
              openOptions.error(result, openOptions);
            closeOpenDialog();
          }
        });
      }
    };

    var setShowRadio = function (all) {
      $('input:radio[value="' + (all ? "All" : "Owned") + '"]', $openDialog)
       .prop('checked', true);
    };

    //
    // The SaveAs Dialog
    //

    var saveAsDialogName = "email-template-save-as-dialog";
    var $saveAsDialog;

    var maxTemplateNameLength = 255;
    var saveOptions = null;

    var closeSaveAsDialog = function () {
      $saveAsDialog.dialog("close");
    };

    var initSaveAs = function()
    {
      $saveAsDialog = $$(saveAsDialogName);
      $saveAsDialog.dialog({
        autoOpen: false,
        close: onCloseSaveAsDialog,
        dialogClass: "email-template-dialog email-template-file-dialog " + saveAsDialogName,
        modal: true,
        resizable: false,
        title: "Save Email Template As",
        width: "auto"
      });
      $('.cancel-button', $saveAsDialog).safeBind("click", closeSaveAsDialog);
      $('.save-button', $saveAsDialog).safeBind("click", onSave);
    };

    var doSave = function () {
      saveOptions.isPublic = $('input:radio[value="Public"]', $saveAsDialog).prop('checked');
      util.openAjaxDialog("Saving template...");
      util.ajax({
        url: "/Admin/WebService.asmx/SaveEmailTemplateAs",
        data: {
          name: saveOptions.name,
          isPublic: saveOptions.isPublic,
          subject: saveOptions.subject,
          body: saveOptions.body,
          isNew: saveOptions.isNew
        },

        success: function (result) {
          util.closeAjaxDialog();
          if (typeof saveOptions.success === "function")
            saveOptions.success(result, saveOptions);
          closeSaveAsDialog();
        },

        error: function (result) {
          util.closeAjaxDialog();
          if (typeof saveOptions.error === "function")
            saveOptions.error(result, saveOptions);
          closeSaveAsDialog();
        }
      });
    };

    var onCloseSaveAsDialog = function () {
      if (typeof saveOptions.cancel === "function")
        saveOptions.cancel(saveOptions);
      saveOptions = null;
      $activeDialog = null;
    };

    var onSave = function () {
      saveOptions.name = $.trim($('.template-name', $saveAsDialog).val());
      if (saveOptions.name.length > maxTemplateNameLength) {
        util.alert("The Template Name cannot exceed " + maxTemplateNameLength +
          " characters");
        return;
      }
      var exists = false;
      var lcname = saveOptions.name.toLowerCase();
      if (templates)
        $.each(templates, function () {
          if (lcname === this.Name.toLowerCase()) {
            exists = true;
            return false;
          }
        });
      if (exists) {
        util.confirm("A template with the name \"" + saveOptions.name +
         "\" already exists. Replace it?",
         function(button) {
           if (button === "Ok")
             doSave();
         });
        return;
      }
      doSave();
    };

    var openSaveAsDialog = function (options) {
      saveOptions = options || {};
      var $templateName =
        $('.template-name', $saveAsDialog).val(options.name || "");
      var radioValue = options.isPublic ? "Public" : "Private";
       $('input:radio[value="' + radioValue + '"]', $saveAsDialog).prop('checked', true);

      util.openAjaxDialog("Getting template list...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetEmailTemplateInfo",
        data: {
          allPublic: false
        },

        success: function (result) {      
          util.closeAjaxDialog();
          templates = result.d;
          $activeDialog = $saveAsDialog;
          refreshDisplay();
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
           "Could not get template data from server"));
          closeSaveAsDialog();
        }
      });
      $saveAsDialog.dialog("open");
      $templateName.select();
    };

    $(function () {
      init();
    });

    return {
      closeOpenDialog: closeOpenDialog,
      closeSaveAsDialog: closeSaveAsDialog,
      confirmOpen: confirmOpen,
      openOpenDialog: openOpenDialog,
      openSaveAsDialog: openSaveAsDialog
    };
  });