define([
    "jquery", "vote/adminMaster", "vote/util", "store", "moment",
    "vote/controls/selectJurisdictions",
    "vote/controls/electionsOfficesCandidates",
    "vote/controls/findPolitician",
    "vote/controls/emailTemplateDialogs",
    "vote/controls/electionControl", "ace/ace",
    "ace/mode/html_votetemplate_highlight_rules", "jqueryui",
    "resizablecolumns", "stupidtable", "textchange"
  ],
  function($,
    master,
    util,
    store,
    moment,
    selectJurisdictions,
    electionsOfficesCandidates,
    findPolitician,
    templateDialogs,
    electionControl,
    ace,
    voteTemplateRules) {

    var $$ = util.$$;
    var level = selectJurisdictions.level;
    var jlevel = electionsOfficesCandidates.level;

    var mainTabsName = "main-tabs";
    var $mainTabs;

    //
    // Edit Template Tab
    //

    var editTemplateName = "edittemplate";
    var editTemplateTabName = "tab-" + editTemplateName;
    var $editTemplateTab;

    var bodyEditorTabsName = "body-editor-tabs";
    var $bodyEditorTabs;

    var maxTemplateNameLength = 255;

    var subjectEditor;
    var bodyEditor;
    var $emailType;
    var focusedEditor;
    var templateName;
    var templateIsPublic;
    var isNewTemplate;
    var forceEditorRedrawOnActivate;
    var newTemplateDialogName = "email-template-new-dialog";
    var $newTemplateDialog;

    var savedSubject;
    var savedBody;
    var savedEmailType;
    var savedSelectRecipientOptions;
    var savedEmailOptions;

    function initEditTemplate() {
      $bodyEditorTabs = $$(bodyEditorTabsName);
      $bodyEditorTabs
        .safeBind("tabsactivate", onBodyEditorTabsActivate);

      $newTemplateDialog = $$(newTemplateDialogName);
      $newTemplateDialog.dialog({
        autoOpen: false,
        dialogClass: newTemplateDialogName,
        modal: true,
        resizable: false,
        title: "Select Email Type",
        width: "auto"
      });
      $(".select-button", $newTemplateDialog).safeBind("click", onClickSelectEmailType);
      $(".cancel-button", $newTemplateDialog).safeBind("click",
        function() {
          closeNewTemplateDialog();
          focusedEditor.focus();
        });
      var $table = $('table', $newTemplateDialog);
      $table.safeBind("click", onClickEmailType)
        .safeBind("dblclick", onDblClickEmailType);

      subjectEditor = ace.edit("subject-editor");
      subjectEditor.setShowPrintMargin(false);
      subjectEditor.setTheme("ace/theme/votetemplate");
      subjectEditor.session.setMode("ace/mode/html_votetemplate");
      subjectEditor.on("focus", function() { focusedEditor = subjectEditor; });

      bodyEditor = ace.edit("body-editor");
      bodyEditor.setShowPrintMargin(false);
      bodyEditor.setTheme("ace/theme/votetemplate");
      bodyEditor.session.setMode("ace/mode/html_votetemplate");
      focusedEditor = bodyEditor;
      bodyEditor.on("focus", function() { focusedEditor = bodyEditor; });
      bodyEditor.on("paste", onPasteBodyEditor);

      setupEmbeddedKeyDialog();

      $emailType = $(".email-type select");

      setSubstitutions();
      resetEditors();

      $(".code-edit-heading .undo-redo div", $editTemplateTab)
        .safeBind("click", onClickUndoRedo);
      $(".edit-template-new-button", $editTemplateTab)
        .safeBind("click", onClickNewTemplate);
      $(".edit-template-open-button", $editTemplateTab)
        .safeBind("click", onClickOpenTemplate);
      $(".edit-template-save-button", $editTemplateTab)
        .safeBind("click", onClickSaveTemplate);
      $(".edit-template-save-as-button", $editTemplateTab)
        .safeBind("click", onClickSaveAsTemplate);

      setTimeout(checkUndoRedo, 200);

      if (util.getCurrentTabId(mainTabsName) === editTemplateTabName) {
        onTabActivateEditTemplate();
      }
    }

    function buildOneAvailableSubstitution(type, deprecated) {
      var list = [];
      var skip = deprecated ? ".active" : ".deprecated";
      $(".substitution-table tr." +
        type +
        " td.name:not(.generic,.disabled," +
        skip +
        ") span.id").each(function() {
        list.push($(this).text().toLowerCase());
      });
      return list.join("|");
    }

    function checkUndoRedo() {
      // we have to do this periodically because onChange fires before the undoManager
      // updates
      var subjectUndoManager = subjectEditor.session.getUndoManager();
      $(".subject-edit-heading .undo", $editTemplateTab)
        .toggleClass("disabled", !subjectUndoManager.hasUndo());
      $(".subject-edit-heading .redo", $editTemplateTab)
        .toggleClass("disabled", !subjectUndoManager.hasRedo());
      var bodyUndoManager = bodyEditor.session.getUndoManager();
      $(".body-edit-heading .undo", $editTemplateTab)
        .toggleClass("disabled", !bodyUndoManager.hasUndo());
      $(".body-edit-heading .redo", $editTemplateTab)
        .toggleClass("disabled", !bodyUndoManager.hasRedo());
      var dirty = templateIsDirty();
      $(".edit-template-save-button", $editTemplateTab)
        .toggleClass("disabled", !dirty);
      $("li.edit-template .tab-ast", $mainTabs)
        .toggleClass("hidden", !dirty);
      setTimeout(checkUndoRedo, 200);
    }

    function closeNewTemplateDialog() {
      $newTemplateDialog.dialog("close");
    }

    function forceEditorRedraw(editor) {
      editor.session.getBackgroundTokenizer().start(0);
    }

    function getNewTemplateDialogDisplayRow(target) {
      return $(target).closest("tbody tr", $newTemplateDialog);
    }

    function newTemplate(force) {
      if (!force && templateIsDirty()) {
        templateDialogs.confirmOpen({
          templateName: templateName,
          save: function() {
            saveTemplate("new");
          },
          dontSave: function() {
            newTemplate(true);
          },
          cancel: function() {
            focusedEditor.focus();
          }
        });
      } else {
        openNewTemplateDialog();
      }
    }

    function onBodyEditorTabsActivate(/*event, ui*/) {
      onTabActivateEditTemplate();
    }

    function onClickEmailType(event) {
      var $tr = getNewTemplateDialogDisplayRow(event.target);
      if ($tr.length) {
        if ($tr.hasClass("selected"))
          $tr.removeClass("selected");
        else {
          $("tr.selected", $tr.closest("tbody")).removeClass("selected");
          $tr.addClass("selected");
        }
      }
    };

    function onClickNewTemplate() {
      newTemplate(false);
    }

    function onClickOpenTemplate() {
      openTemplate(false);
    }

    function onClickSaveAsTemplate() {
      saveAsTemplate();
    }

    function onClickSaveOptions() {
      util.openAjaxDialog("Saving options...");
      var selectRecipientOptions = formatSelectionCriteriaAsJson();
      var emailOptions = formatEmailOptionsAsJson();
      util.ajax({
        url: "/Admin/WebService.asmx/SaveEmailTemplateOptions",
        data: {
          name: templateName,
          selectRecipientOptions: selectRecipientOptions,
          emailOptions: emailOptions
        },

        success: function() {
          util.closeAjaxDialog();
          savedSelectRecipientOptions = selectRecipientOptions;
          savedEmailOptions = emailOptions;
          onChangeOptions();
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not save options"));
        }
      });
    }

    function onClickSaveTemplate() {
      if ($(".edit-template-save-button", $editTemplateTab).hasClass("disabled")) return;
      saveTemplate();
    }

    function onClickSelectEmailType() {
      openDocumentTypeRow($("table tr.selected", $newTemplateDialog));
    };

    function onClickUndoRedo(event) {
      var $target = $(event.target);
      if ($target.hasClass("disabled")) return;
      var editor = $target.closest(".code-edit-heading").hasClass("body-edit-heading")
        ? bodyEditor
        : subjectEditor;
      var undoManager = editor.session.getUndoManager();
      $target.hasClass("undo") ? undoManager.undo() : undoManager.redo();
      editor.focus();
    }

    function onDblClickEmailType() {
      openDocumentTypeRow($(event.target).closest("table tr", $newTemplateDialog));
    };

    function onPasteBodyEditor(text) {
      var split = text.split(/\r\n|\r|\n/);
      if (split.length > 1) {
        // if any lines already end in a break tag, don't prompt
        var endsWithBr = false;
        $.each(split, function() {
          if (/<br\s*\/?>\s*$/i.test(this)) {
            endsWithBr = true;
            return false;
          }
        });
        if (endsWithBr) return;
        util.confirm(
          "The pasted text contains line breaks. Do you want to insert HTML break tags?",
          function(button) {
            if (button === "Ok") {
              var cursorPos = bodyEditor.getCursorPosition();
              var row = cursorPos.row;
              var breaks = split.length - 1;
              var document = bodyEditor.session.getDocument();
              while (breaks--) {
                row--;
                var line = document.getLine(row);
                document.insert({ row: row, column: line.length }, '<br />');
              }
            }
          });
      }
    }

    function onTabActivateEditTemplate() {
      switch (util.getCurrentTabId(bodyEditorTabsName)) {
      case "tab-body-html":
        if (forceEditorRedrawOnActivate) {
          subjectEditor.renderer.updateFull();
          bodyEditor.renderer.updateFull();
          forceEditorRedrawOnActivate = false;
        }
        focusedEditor.focus();
        // this jiggling around keeps the scroll bar position synchronized
        bodyEditor.renderer.scrollBy(0, -1);
        setTimeout(function() { bodyEditor.renderer.scrollBy(0, 1); }, 10);
        break;

      case "tab-body-rendered":
        refreshRenderedBody();
        break;
      }
    }

    function openDocumentTypeRow($tr) {
      if ($tr.length === 1) {
        var id = $("td", $tr).attr("template-id");
        if (!id) {
          // blank document
          closeNewTemplateDialog();
          resetEditors();
          focusedEditor.focus();
          return;
        }

        util.openAjaxDialog("Opening document type...");
        util.ajax({
          url: "/Admin/WebService.asmx/OpenEmailTemplate",
          data: {
            id: id
          },

          success: function(result) {
            util.closeAjaxDialog();
            closeNewTemplateDialog();
            openTemplateSucceeded(result, { isStarter: true });
          },

          error: function(result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result,
              "Could not get email type document from server"));
            closeNewTemplateDialog();
            focusedEditor.focus();
          }
        });
      }
    }

    function openNewTemplateDialog() {
      util.openAjaxDialog("Getting email types...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetEmailStarterDocuments",

        success: function(result) {
          util.closeAjaxDialog();
          var rows = [];
          $.each(result.d, function() {
            rows.push(
              '<tr><td template-id="' + this.Value + '">' + this.Text + '</td></tr>');
          });
          var $tbody = $("tbody", $newTemplateDialog);
          $tbody.html(rows.join(""));
          util.assignRotatingClassesToChildren($tbody, ["odd", "even"]);
          $(":first-child", $tbody).addClass("selected");
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not get email type info from server"));
          closeNewTemplateDialog();
        }
      });

      $newTemplateDialog.dialog("open");
    }

    function openTemplate(force) {
      if (!force && templateIsDirty()) {
        templateDialogs.confirmOpen({
          templateName: templateName,
          save: function() {
            saveTemplate("open");
          },
          dontSave: function() {
            openTemplate(true);
          },
          cancel: function() {
            focusedEditor.focus();
          }
        });
      } else {
        templateDialogs.openOpenDialog({
          requirements: currentRequirements,
          success: openTemplateSucceeded,
          error: function(result) {
            util.alert(util.formatAjaxError(result,
                "Could not open the requested template"),
              function() {
                focusedEditor.focus();
              });
          },
          cancel: function() {
            focusedEditor.focus();
          }
        });
      }
    }

    function openTemplateSucceeded(result, options) {
      var resetOptions = {};
      var d = result.d;
      resetOptions.isNewTemplate = !d.IsOwner;
      resetOptions.isStarter = options.isStarter;
      resetOptions.subject = d.Subject;
      resetOptions.body = d.Body;
      resetOptions.emailType = d.EmailTypeCode;
      if (d.IsOwner) {
        resetOptions.name = d.Name;
        resetOptions.isPublic = d.IsPublic;
        resetOptions.selectRecipientOptions = d.SelectRecipientOptions;
        resetOptions.emailOptions = d.EmailOptions;
      } else {
        resetOptions.name = (options.isStarter ? "New " : "Copy of ") + d.Name;
        if (resetOptions.name.length > maxTemplateNameLength)
          resetOptions.name = resetOptions.name.substr(0, maxTemplateNameLength);
        resetOptions.isPublic = false;
        resetOptions.selectRecipientOptions = null;
        resetOptions.emailOptions = null;
      }
      resetEditors(resetOptions, function() {
        if (options.isStarter) {
          bodyEditor.find("<!-- Email content goes here -->", {}, false);
        }
        updateDisabling(getAvailableRequirements());
        bodyEditor.focus();
      });
    }

    function onChangeOptions() {
      var disabled = true;
      if (!isNewTemplate) {
        var selectRecipientOptions = formatSelectionCriteriaAsJson();
        var emailOptions = formatEmailOptionsAsJson();
        disabled = selectRecipientOptions === savedSelectRecipientOptions &&
          emailOptions === savedEmailOptions;
      }
      $(".save-options-button", $mainTabs).prop("disabled", disabled);
    }

    function refreshRenderedBody() {
      var $iframeBody = $("#tab-body-rendered iframe", $bodyEditorTabs).contents()
        .find('body');
      $iframeBody.html(bodyEditor.getValue());
      util.setOffpageTargets($iframeBody);
    }

    function resetEditors(options, onCompletion) {

      function completionFn() {
        if (typeof onCompletion === "function") onCompletion();
        onChangeOptions();
      }

      options = options || { isPublic: true };
      setTemplateName(options.name || "New Template");
      templateIsPublic = !!options.isPublic;
      isNewTemplate = options.isNewTemplate !== false;
      subjectEditor.setValue(options.subject || "");
      subjectEditor.getSelection().selectFileStart();
      bodyEditor.setValue(options.body || "");
      bodyEditor.getSelection().selectFileStart();
      $emailType.val(options.emailType || "");
      if (isNewTemplate && !options.isStarter) {
        savedSubject = "";
        savedBody = "";
        savedEmailType = "";
      } else
        updateSavedTemplate();
      subjectEditor.session.setUndoManager(new ace.UndoManager());
      bodyEditor.session.setUndoManager(new ace.UndoManager());
      savedSelectRecipientOptions = options.selectRecipientOptions;
      savedEmailOptions = options.emailOptions;
      if (savedEmailOptions)
        try {
          restoreEmailOptions(JSON.parse(savedEmailOptions));
        } catch (e) {
        }
      if (savedSelectRecipientOptions)
        try {
          restoreSelectRecipientOptions(JSON.parse(savedSelectRecipientOptions), null, completionFn);
        } catch (e) {
        }
      else completionFn();
    }

    function saveAsTemplate(followAction) {
      templateDialogs.openSaveAsDialog({
        body: bodyEditor.getValue(),
        isPublic: templateIsPublic,
        name: templateName,
        subject: subjectEditor.getValue(),
        emailType: $emailType.val(),
        isNew: isNewTemplate,
        followAction: followAction || "",
        success: function(result, options) {
          setTemplateName(options.name);
          templateIsPublic = options.isPublic;
          savedSubject = options.subject;
          savedBody = options.body;
          savedEmailType = options.emailType,
            isNewTemplate = false;
          onChangeOptions();
          focusedEditor.focus();
          switch (options.followAction) {
          case "open":
            openTemplate(true);
            break;

          case "new":
            newTemplate(true);
            break;
          }
        },
        error: function(result, options) {
          util.alert(util.formatAjaxError(result,
              "Could not save \"" + options.name + "\""),
            function() {
              focusedEditor.focus();
            });
        },
        cancel: function() {
          focusedEditor.focus();
        }
      });
    }

    function saveTemplate(followAction) {
      if (isNewTemplate) {
        saveAsTemplate(followAction);
        return;
      }

      util.openAjaxDialog("Saving " + util.htmlEscape(templateName) + "...");
      util.ajax({
        url: "/Admin/WebService.asmx/SaveEmailTemplate",
        data: {
          name: templateName,
          subject: subjectEditor.getValue(),
          body: bodyEditor.getValue(),
          emailType: $emailType.val(),
          followAction: followAction || ""
        },

        success: function(result) {
          updateSavedTemplate();
          util.closeAjaxDialog();
          isNewTemplate = false;
          onChangeOptions();
          focusedEditor.focus();
          switch (result.d) {
          case "open":
            openTemplate(true);
            break;

          case "new":
            newTemplate(true);
            break;
          };
        },

        error: function(result) {
          util.alert(util.formatAjaxError(result,
              "Could not save \"" + templateName + "\""),
            function() {
              util.closeAjaxDialog();
              focusedEditor.focus();
            });
        }
      });
    }

    function setSubstitutions() {
      voteTemplateRules.setSubstitutions({
        substitutionList: buildOneAvailableSubstitution("text"),
        emailSubstitutionList: buildOneAvailableSubstitution("email"),
        webSubstitutionList: buildOneAvailableSubstitution("web"),
        deprecatedSubstitutionList: buildOneAvailableSubstitution("text", true),
        deprecatedEmailSubstitutionList: buildOneAvailableSubstitution("email", true),
        deprecatedWebSubstitutionList: buildOneAvailableSubstitution("web", true)
      });
    }

    function setTemplateName(name) {
      templateName = name;
      $(".edittemplate-name", $mainTabs).safeHtml(name);
    }

    function setupEmbeddedKeyDialog() {

      var $embeddedKeyDialog = $('#embedded-key-dialog');
      var elections;
      var election;
      var office;

      function resetElections() {
        $(".select-election-key .selected-value", $embeddedKeyDialog).text("");
        $(".select-election-key select", $embeddedKeyDialog).val("")
          .prop("disabled", true)
          .html('<option>&lt;select an election&gt;</option>');
      }

      function resetOffices() {
        $(".select-office-key .selected-value", $embeddedKeyDialog).text("");
        $(".select-office-key select", $embeddedKeyDialog).val("")
          .prop("disabled", true)
          .html('<option>&lt;select an office&gt;</option>');
      }

      function resetPoliticians() {
        $(".select-politician-key .selected-value", $embeddedKeyDialog).text("");
        $(".select-politician-key select", $embeddedKeyDialog).val("")
          .prop("disabled", true)
          .html('<option>&lt;select a politician&gt;</option>');
      }

      $embeddedKeyDialog.dialog({
        autoOpen: false,
        dialogClass: 'embedded-key-dialog',
        modal: true,
        resizable: false,
        title: "Insert Embedded Key Value",
        width: "auto",
        close: function () { focusedEditor.focus(); }
      });

      $(".insert-embedded-button", $editTemplateTab).on("click", function () {
        $embeddedKeyDialog.dialog("open");
      });

      $(".select-state-code select", $embeddedKeyDialog).on("change", function () {
        var stateCode = $(this).val();
        $(".select-state-code .selected-value", $embeddedKeyDialog).text(stateCode);
        $(".select-state-code .insert-button", $embeddedKeyDialog).prop("disabled", !stateCode);

        resetElections();
        resetOffices();
        resetPoliticians();

        if (stateCode) {
          // get state data
          util.openAjaxDialog("Getting state election data...");
          util.ajax({
            url: "/Admin/WebService.asmx/GetEmbeddedKeyDataForState",
            data: {
              stateCode: stateCode
            },

            complete: function() {
              util.closeAjaxDialog();
            },

            success: function (result) {
              var $electionsDropdown = $(".select-election-key select", $embeddedKeyDialog);
              elections = result.d;
              if (elections.length) {
                util.populateDropdown($electionsDropdown, elections, "<select an election>",
                  "");
                $electionsDropdown.prop("disabled", false);
              } else {
                $electionsDropdown.html('<option>&lt;no elections for this state&gt;</option>');
              }
            },

            error: function (result) {
              util.alert(util.formatAjaxError(result, "Could not get elections"));
            }
          });
        }
      });

      $(".select-state-code .insert-button", $embeddedKeyDialog).on("click", function() {
        var text = "[[StateCode=" + $(".select-state-code select").val() + "]]";
        focusedEditor.insert(text);
      });

      $(".select-election-key select", $embeddedKeyDialog).on("change", function () {
        var electionKey = $(this).val().toUpperCase();

        $(".select-election-key .selected-value", $embeddedKeyDialog).text($(this).val());
        $(".select-election-key .insert-button", $embeddedKeyDialog).prop("disabled", !electionKey);

        resetOffices();
        resetPoliticians();

        if (electionKey) {
          // get election data
          election = null;
          $.each(elections, function() {
            if (this.Value.toUpperCase() === electionKey) {
              election = this;
              return false;
            }
          });
          var $officesDropdown = $(".select-office-key select", $embeddedKeyDialog);
          if (election && election.Offices.length) {
            util.populateDropdown($officesDropdown, election.Offices, "<select an office>",
              "");
            $officesDropdown.prop("disabled", false);
          } else {
            $officesDropdown.html('<option>&lt;no offices for this election&gt;</option>');
          }
        }
      });

      $(".select-election-key .insert-button", $embeddedKeyDialog).on("click", function () {
        var text = "[[ElectionKey=" + $(".select-election-key select").val() + "]]";
        focusedEditor.insert(text);
      });

      $(".select-office-key select", $embeddedKeyDialog).on("change", function () {
        var officeKey = $(this).val().toUpperCase();

        $(".select-office-key .selected-value", $embeddedKeyDialog).text($(this).val());
        $(".select-office-key .insert-button", $embeddedKeyDialog).prop("disabled", !officeKey);

        resetPoliticians();

        if (officeKey) {
          // get politician data
          office = null;
          $.each(election.Offices, function () {
            if (this.Value.toUpperCase() === officeKey) {
              office = this;
              return false;
            }
          });
          var $politiciansDropdown = $(".select-politician-key select", $embeddedKeyDialog);
          if (office && office.Politicians.length) {
            util.populateDropdown($politiciansDropdown, office.Politicians, "<select a politician>",
              "");
            $politiciansDropdown.prop("disabled", false);
          } else {
            $politiciansDropdown.html('<option>&lt;no politicians for this office&gt;</option>');
          }
        }
      });

      $(".select-office-key .insert-button", $embeddedKeyDialog).on("click", function () {
        var text = "[[OfficeKey=" + $(".select-office-key select").val() + "]]";
        focusedEditor.insert(text);
      });

      $(".select-politician-key select", $embeddedKeyDialog).on("change", function () {
        var politicianKey = $(this).val().toUpperCase();

        $(".select-politician-key .selected-value", $embeddedKeyDialog).text($(this).val());
        $(".select-politician-key .insert-button", $embeddedKeyDialog).prop("disabled", !politicianKey);
      });

      $(".select-politician-key .insert-button", $embeddedKeyDialog).on("click", function () {
        var text = "[[PoliticianKey=" + $(".select-politician-key select").val() + "]]";
        focusedEditor.insert(text);
      });
    }

    function templateIsDirty() {
      return savedSubject !== subjectEditor.getValue() ||
        savedBody !== bodyEditor.getValue() ||
        savedEmailType !== $emailType.val();
    }

    function updateSavedTemplate() {
      savedSubject = subjectEditor.getValue();
      savedBody = bodyEditor.getValue();
      savedEmailType = $emailType.val();
    };

    function updateTemplateLastUsedDate() {
      if (!isNewTemplate) {
        util.ajax({
          url: "/Admin/WebService.asmx/UpdateEmailTemplateLastUsed",
          data: {
            name: templateName
          }
        });
      }
    };

    //
    // Available Substitutions Tab
    //

    var availableSubstitutionsName = "availablesubstitutions";
    var availableSubstitutionsTabName = "tab-" + availableSubstitutionsName;
    var $availableSubstitutionsTab;

    var allRequirements = [
      "statecode", "countycode", "localkey",
      "politiciankey", "electionkey", "officekey", "issuekey", "partykey",
      "partyemail", "visitorid", "donorid", "orgcontactid"
    ];

    var defaultRequirements = [
      "statecode", "countycode", "localkey",
      "politiciankey", "electionkey", "officekey", "issuekey", "partykey",
      "partyemail", "visitorid", "donorid", "orgcontactid"
    ];

    var currentRequirements = null;

    var initAvailableSubstitutions = function() {
      $availableSubstitutionsTab = $$(availableSubstitutionsTabName);
      $(".elseif-spinner", $availableSubstitutionsTab).spinner({ min: 0, max: 9 });
      $(".substitution-table td.name", $availableSubstitutionsTab)
        .safeBind("dblclick", onDblClickSubstitutionName);
      $(".insert-conditional-button", $availableSubstitutionsTab)
        .safeBind("click", onClickInsertConditional);
      $("p.fail-directive", $availableSubstitutionsTab)
        .safeBind("dblclick", onDblClickInsertFail);
      updateDisabling(defaultRequirements);
    };

    function onClickInsertConditional() {
      var ifCondition = $(".if-condition").val();
      var elseifCondition = $(".elseif-condition").val();
      var elseifs = parseInt($(".elseif-spinner").val());
      var includeElse = $(".else-checkbox").prop("checked");
      var text = "{{if " + ifCondition + "}}  {{then}}\n";
      for (var i = 0; i < elseifs; i++)
        text += "{{elseif " + elseifCondition + "}} {{then}}\n";
      if (includeElse)
        text += "{{else}}\n";
      text += "{{endif}}";
      var oldLead = focusedEditor.getSelection().lead;
      var oldRow = oldLead.row;
      var oldColumn = oldLead.column;
      focusedEditor.insert(text);
      var newColumn = oldColumn + 5 + ifCondition.length;
      if (ifCondition === "empty" || ifCondition === "notempty")
        newColumn += 3;
      focusedEditor.getSelection().moveCursorTo(oldRow, newColumn);
      activateTab(editTemplateName);
      focusedEditor.focus();
    }

    function onDblClickInsertFail() {
      focusedEditor.insert("{{fail }}");
      focusedEditor.getSelection().moveCursorBy(0, -2);
      activateTab(editTemplateName);
      focusedEditor.focus();
    };

    function onDblClickSubstitutionName(event) {
      var $target = $(event.target).closest("td");
      util.clearSelection();
      if ($target.hasClass("disabled")) return;
      var text;
      var pos = 0;
      if ($target.html().toLowerCase().indexOf("<i>") >= 0) {
        // it's an immediate tag
        text = $target.text().substr(0, 2);
        text += text;
        pos = -2;
      } else {
        // normal tag
        text = $target.text();
        if (text.substr(text.length - 4, 2) === "()")
          pos = -3;
      }
      focusedEditor.insert(text);
      if (pos) focusedEditor.getSelection().moveCursorBy(0, pos);
      activateTab(editTemplateName);
      focusedEditor.focus();
    }

    function updateDisabling(available) {
      var unsatisfied = [];
      $.each(allRequirements, function() {
        var value = this.toString();
        if ($.inArray(value, available) < 0)
          unsatisfied.push(".req-" + value);
      });
      $(".substitution-table td.name").addClass("disabled");
      $(".substitution-table td.name:not(" +
        unsatisfied.join(",") +
        ")").removeClass("disabled");
      setSubstitutions();

      // force retokenize
      forceEditorRedraw(subjectEditor);
      forceEditorRedraw(bodyEditor);
      forceEditorRedrawOnActivate = true;

      currentRequirements = (available === allRequirements) ? null : available;
    }

    //
    // Select Recipients Tab
    //

    var selectRecipientsName = "selectrecipients";
    var selectRecipientsTabName = "tab-" + selectRecipientsName;
    var $selectRecipientsTab;

    var rjc; // recipientsJurisdictionsControl
    var recipientsResults = null;
    var formattedSelectionCriteria = null;
    var pendingRecipientBatchId = 0;
    var postEmailBatchStatusInterval = 500;
    var electionsLoadedState = null;
    var emptyElectionsLoaded = { stateCode: "", countyCode: "", localKey: "" };
    var electionsLoaded = $.extend({}, emptyElectionsLoaded);
    var $filterByEmailTypeDialog;
    var $filterByLoginDateDialog;

    function initSelectRecipients() {
      $selectRecipientsTab = $$(selectRecipientsTabName);
      rjc = new selectJurisdictions.SelectJurisdictions(
        $(".select-jurisdictions-control", $selectRecipientsTab),
        { onChange: onChangeJurisdiction });
      rjc.init();
      $(".recipient-buttons input[type=radio]").safeBind("change", onRecipientTypeChange);
      $(".get-recipients-button").safeBind("click", onRecipientsCreateRecipientsList);
      $(".election-filtering input[type=radio]")
        .safeBind("change", onElectionFilteringChange);
      $(".district-filtering input[type=radio]")
        .safeBind("change", onDistrictFilteringChange);
      $(".district-filtering .all", $selectRecipientsTab)
        .safeBind("change", onDistrictFilteringAllChange);
      $(".district-filtering .checkbox-list", $selectRecipientsTab)
        .safeBind("change", onDistrictFilteringListChange);
      $(".org-filtering .select-org-type", $selectRecipientsTab)
        .safeBind("change", onOrgTypeChange);

      $(".filter-by-email-type-button").click(function() {
        $(".error", $filterByEmailTypeDialog).removeClass("error");
        $filterByEmailTypeDialog.dialog("open");
      });

      $(".filter-by-login-date-button").click(function() {
        $(".error", $filterByLoginDateDialog).removeClass("error");
        $filterByLoginDateDialog.dialog("open");
      });

      $filterByEmailTypeDialog = $$("filter-by-email-type-dialog");
      $filterByEmailTypeDialog.dialog({
        autoOpen: false,
        dialogClass: "filter-by-email-type-dialog",
        modal: true,
        resizable: false,
        title: "Filter By Email Type",
        width: "auto"
      });
      $(".filter-by-email-type-ok", $filterByEmailTypeDialog)
        .click(onClickFilterByEmailTypeOk);

      $filterByLoginDateDialog = $$("filter-by-login-date-dialog");
      $filterByLoginDateDialog.dialog({
        autoOpen: false,
        dialogClass: "filter-by-login-date-dialog",
        modal: true,
        resizable: false,
        title: "Filter By Login Date",
        width: "auto"
      });
      $(".filter-by-login-date-ok", $filterByLoginDateDialog)
        .click(onClickFilterByLoginDateOk);

      $(".visitor-donor-options .date-range input[type=text]", $selectRecipientsTab)
        .datepicker({
          changeYear: true,
          yearRange: "2010:+0"
        });

      $("#filter-by-email-type-dialog .date,#filter-by-login-date-dialog .date").datepicker(
        {
          changeYear: true,
          yearRange: "2010:+0"
        });

      util.registerDataMonitor($(".visitor-donor-options .date-range input",
        $selectRecipientsTab));

      onOrgTypeChange();
    }

    function getFilterByEmailTypeChecked() {
      return $(".checkbox", $filterByEmailTypeDialog).prop("checked");
    }

    function getFilterByEmailTypeDidOrNot() {
      return $("input[name=email-type-did-or-not]:checked", $filterByEmailTypeDialog).val();
    }

    function getFilterByEmailType() {
      return $(".email-type-select", $filterByEmailTypeDialog).val();
    }

    function getFilterByEmailTypeDescription() {
      return $(".email-type-select option:selected", $filterByEmailTypeDialog).text();
    }

    function getFilterByLoginDateChecked() {
      return $(".checkbox", $filterByLoginDateDialog).prop("checked");
    }

    function getFilterByLoginDateDidOrNot() {
      return $("input[name=login-date-did-or-not]:checked", $filterByLoginDateDialog).val();
    }

    function onClickFilterByEmailTypeOk() {
      $(".error", $filterByEmailTypeDialog).removeClass("error");
      var checked = getFilterByEmailTypeChecked();
      var didOrNot = getFilterByEmailTypeDidOrNot();
      var emailType = getFilterByEmailType();

      if (checked) {
        if (didOrNot !== "did" && didOrNot !== "didnot") {
          util.alert("Please select \"did\" or \"did not\.");
          return;
        }
        if (!emailType) {
          $(".email-type-select", $filterByEmailTypeDialog).addClass("error");
          util.alert("Please select an email type.");
          return;
        }
        var startDate = getValidatedEmailTypeStartDate();
        if (startDate === null) return;
        var endDate = getValidatedEmailTypeEndDate();
        if (endDate === null) return;
        if (startDate &&
          endDate &&
          util.getUtcDateFromLocalTime(endDate) < util.getUtcDateFromLocalTime(startDate)) {
          $(".date", $filterByEmailTypeDialog).addClass("error");
          util.alert("The end date is before the start date.");
          return;
        }
      }

      $(".filter-by-email-type .filtering-msg").toggleClass("invisible", !checked);
      $filterByEmailTypeDialog.dialog("close");
    }

    function onClickFilterByLoginDateOk() {
      $(".error", $filterByLoginDateDialog).removeClass("error");
      var checked = getFilterByLoginDateChecked();
      var didOrNot = getFilterByLoginDateDidOrNot();

      if (checked) {
        if (didOrNot !== "did" && didOrNot !== "didnot") {
          util.alert("Please select \"did\" or \"did not\.");
          return;
        }
        var startDate = getValidatedLoginDateStartDate();
        if (startDate === null) return;
        var endDate = getValidatedLoginDateEndDate();
        if (endDate === null) return;
        if (startDate &&
          endDate &&
          util.getUtcDateFromLocalTime(endDate) < util.getUtcDateFromLocalTime(startDate)) {
          $(".date", $filterByLoginDateDialog).addClass("error");
          util.alert("The end date is before the start date.");
          return;
        }
      }

      $(".filter-by-login-date .filtering-msg").toggleClass("invisible", !checked);
      $filterByLoginDateDialog.dialog("close");
    }

    function getTitleString() {
      var titleString = "Title";
      switch (getRecipientType()) {
      case "AllCandidates":
      case "StateCandidates":
      case "CountyCandidates":
      case "LocalCandidates":
        titleString = "Office Sought";
        break;

      case "Organizations":
        titleString = "Organization";
        break;
      }

      return titleString;
    }

    function buildRecipientDisplayTable() {
      var head = '<p class="heading"></p><div class="toggler"><div class="ind">' +
        '</div><div><div class="criteria rounded-border">' +
        formattedSelectionCriteria +
        '</div>Click to see the selection criteria used for this list.</div></div>';
      var lines = [];
      var evenOdd = "odd";

      // one line per recipient
      $.each(recipientsResults.Items, function(inx) {
        lines.push('<tr inx="' +
          inx +
          '" class="' +
          evenOdd +
          '">' +
          '<td data-sort-value="' +
          util.emailForSort(this.Email) +
          '"><div class="css-checkbox"/>' +
          this.Email +
          '</td>' +
          '<td data-sort-value="' +
          this.SortName +
          '">' +
          this.Contact +
          '</td>' +
          '<td>' +
          (this.Title || "n/a") +
          '</td>' +
          '<td data-sort-value="' +
          util.jurisdictionForSort(this.Jurisdiction) +
          '">' +
          this.Jurisdiction +
          '</td></tr>');
        evenOdd = evenOdd === "even" ? "odd" : "even";
      });

      // build the heading, including the info for resizablecolumns and stupidtable
      return head +
        '<table data-resizable-columns-id="bulk-email-recipients">' +
        '<thead><tr class="head">' +
        '<th data-sort="string" data-resizable-column-id="email">' +
        '<div class="css-checkbox"/>Email<div class="sort-ind"></div></th>' +
        '<th data-sort="string" data-resizable-column-id="contact">Contact' +
        '<div class="sort-ind"></div></th>' +
        '<th data-sort="string" data-resizable-column-id="title">' +
        getTitleString() +
        '<div class="sort-ind"></div></th>' +
        '<th data-sort="string" data-resizable-column-id="jurisdiction">' +
        'Jurisdiction<div class="sort-ind"></div></th>' +
        '</tr></thead><tbody>' +
        lines.join("") +
        '</tbody></table>';
    }

    function doRecipientsList() {
      var data = formatSelectionCriteriaAsObject();
      data.batchId = pendingRecipientBatchId;
      util.ajax({
        url: "/Admin/WebService.asmx/GetEmailBatch",
        data: data,

        success: doRecipientsListSuccess,

        error: function(result) {
          doRecipientsListError(result);
        }
      });
    }

    function doRecipientsListError(result) {
      var msg;
      if (typeof (result) === "string")
        msg = result;
      else
        msg = util.formatAjaxError(result,
          "An error occurred while retrieving the recipient list").replace("\n", "<br />");
      $(".current-list-results", $viewRecipientsTab)
        .html("<p>" + msg + "</p>");
      activateTab(viewRecipientsName);
      recipientsResults = null;
      $(".preview-selected-recipient input").prop("disabled", true);
      pendingRecipientBatchId = 0;
      util.closeAjaxDialog();
      updateDisabling(defaultRequirements);
    };

    function doRecipientsListSuccess(result) {
      pendingRecipientBatchId = 0;

      if (result.d.Items.length === 0) {
        doRecipientsListError("There were no qualifying email recipients found.");
        return;
      }

      // save the returned list of recipients and criteria
      recipientsResults = result.d;
      formattedSelectionCriteria = formatSelectionCriteria();

      // we have recipients, so enable the button
      $(".preview-selected-recipient input").prop("disabled", false);

      // if the recipients are WebsiteVisitors, Donors or Organizations we preselect the 
      // above button and disable the other button, because I don't have an 
      // effective way to choose an arbitrary visitor, donor or organization
      var noSampleKeys = getRecipientType() === "WebsiteVisitors" ||
        getRecipientType() === "Donors" || getRecipientType() === "Organizations";
      if (noSampleKeys)
        $(".preview-selected-recipient input").prop("checked", true);
      $(".preview-sample-keys-recipient input").prop("disabled", noSampleKeys);

      // create the listing page
      var $container = $(".current-list-results", $viewRecipientsTab);
      $container.html(buildRecipientDisplayTable());

      util.closeAjaxDialog();
      activateTab(viewRecipientsName);
      setPreviewKeysToSelectedRecipient();
      $(".current-list-results th .css-checkbox", $viewRecipientsTab)
        .safeBind("click", onClickListResults); // need to head off sorting

      // attach event handlers
      var $table = $('table', $container);
      $table.resizableColumns({ store: store });
      var table = $table.stupidtable();
      table.on("beforetablesort", function() {
        $table = $(".current-list-results table", $viewRecipientsTab);
        $table.addClass("disabled");
      });
      table.on("aftertablesort", function(event, data) {
        $table = $(".current-list-results table", $viewRecipientsTab);
        $table.removeClass("disabled");
        $table.find("th div.sort-ind").removeClass("asc desc");
        var dir = data.direction === $.fn.stupidtable.dir.ASC ? "asc" : "desc";
        $("div.sort-ind", $table.find("th").eq(data.column)).addClass(dir);
        // redo alternate coloring class
        var rowClass = "odd";
        $("tbody tr", $table).each(function() {
          $(this).removeClass("odd even").addClass(rowClass);
          rowClass = rowClass === "even" ? "odd" : "even";
        });
      });

      // start everything checked
      $('tbody .css-checkbox', $container).addClass("checked");
      setRecipientsHeaderCheckbox();
      updateDisabling(getAvailableRequirements());
    }

    function editRecipientSelections() {
      // There must be at least one jurisdictional selection
      // in each relevant category
      var recipientType = getRecipientType();
      switch (recipientType) {
      case "Local":
      case "LocalCandidates":
        if (!rjc.getCategoryCodes("locals", true).length) {
          util.alert("You need to select at least one local district.");
          return false;
        }
      // break intentionally omitted

      case "County":
      case "CountyCandidates":
        if (!rjc.getCategoryCodes("counties", true).length) {
          util.alert("You need to select at least one county.");
          return false;
        }
      // break intentionally omitted

      case "All":
      case "State":
      case "AllCandidates":
      case "StateCandidates":
      case "PartyOfficials":
      case "Volunteers":
        if (!rjc.getCategoryCodes("states", true).length) {
          util.alert("You need to select at least one state.");
          return false;
        }
        break;

      case "WebsiteVisitors":
        // check for both members of exclusive pair checked
        var ok = true;
        $(".visitor-options-filter .exclusive-pair", $selectRecipientsTab).each(function() {
          var $div = $(this);
          var $first = $("input", $div);
          var $second = $("input", $div.next());
          if ($first.prop("checked") && $second.prop("checked")) {
            util.alert('Both the "' +
              $first.next().text() +
              '" and the "' +
              $second.next().text() +
              '" filters are checked.');
            ok = false;
            return false;
          }
        });
        if (!ok) return false;
        if (getValidatedVisitorStartDate() == null) return false;
        if (getValidatedVisitorEndDate() == null) return false;
        // at least one type must be checked
        if (!$("#RecipientsVisitorsSampleBallots").prop("checked") &&
          !$("#RecipientsVisitorsSharedChoices").prop("checked") &&
          !$("#RecipientsVisitorsDonated").prop("checked")) {
          util.alert("At least one Visitor Type must be checked");
          return false;
        }
        break;

      case "Donors":
        if (getValidatedDonorStartDate() == null) return false;
        if (getValidatedDonorEndDate() == null) return false;
        break;
      }

      // no further editing necessary for PartyOfficials, Volunteers, WebsiteVisitors, Donors or Organizations
      if (recipientType === "PartyOfficials" ||
        recipientType === "Volunteers" ||
        recipientType === "WebsiteVisitors" ||
        recipientType === "Donors" ||
        recipientType === "Organizations") return true;

      // for any of the candidate selections, at least one of the
      // Emails to Use boxes must be checked
      switch (recipientType) {
      case "LocalCandidates":
      case "CountyCandidates":
      case "StateCandidates":
      case "AllCandidates":
        if (!getEmailsToUse().length) {
          util.alert("You need make at least one selection from 'Emails to Use'.");
          return false;
        }
      }

      // for has/does not have election of selected type(s), there must be at least one type selected
      // for single selected election, there must be an election selected
      var electionFilterType = getElectionFiltering();
      switch (electionFilterType) {
      case "HasType":
      case "HasntType":
        if (!getElectionFilterTypes().length) {
          util.alert("You need make at least one selection from 'Election Filter Type'.");
          return false;
        }
        break;

      case "Single":
        if (!electionControl.getSelectedElection()) {
          util.alert("You need select an election.");
          return false;
        }
        break;
      }

      return true;
    }

    function formatSelectionCriteria() {
      var criteriaLines = [];

      criteriaLines.push('<b>Contact Type:</b> ' +
        $(".recipient-buttons input[type=radio]:checked", $selectRecipientsTab)
        .next().text());

      var recipientType = getRecipientType();
      var isCandidateSelection = false;
      switch (recipientType) {
      case "All":
      case "State":
      case "County":
      case "Local":
        criteriaLines.push('<b>Contacts to Use:</b> ' +
          $(".contacts-buttons input[type=radio]:checked", $selectRecipientsTab)
          .next().text());
        break;

      case "AllCandidates":
      case "StateCandidates":
      case "CountyCandidates":
      case "LocalCandidates":
        var emailsToUse = [];
        $(".emails-checkboxes input[type=checkbox]:checked", $selectRecipientsTab)
          .each(function() {
            emailsToUse.push($(this).next().text());
          });
        criteriaLines.push('<b>Emails to Use:</b> ' + emailsToUse.join(", "));
        isCandidateSelection = true;
        break;
      }

      criteriaLines.push('<b>States:</b> ' +
        rjc.getCategoryCodes("states", true).join(", "));

      switch (recipientType) {
      case "County":
      case "CountyCandidates":
      case "Local":
      case "LocalCandidates":
      case "WebsiteVisitors":
      case "Donors":
        criteriaLines.push('<b>Counties:</b> ' +
          rjc.getJurisdictionNames("counties", true).join(", "));
        break;
      }

      switch (recipientType) {
      case "Local":
      case "LocalCandidates":
        criteriaLines.push('<b>Local Districts:</b> ' +
          rjc.getJurisdictionNames("locals", true).join(", "));
        break;
      }

      switch (recipientType) {
      case "All":
      case "State":
      case "County":
      case "Local":
      case "AllCandidates":
      case "StateCandidates":
      case "CountyCandidates":
      case "LocalCandidates":
      case "PartyOfficials":
        criteriaLines.push('<b>Election Filtering:</b> ' +
          $(".election-filtering input[type=radio]:checked", $selectRecipientsTab)
          .next().text());
        switch (getElectionFiltering()) {
        case "HasType":
        case "HasntType":
          var filterTypes = [];
          $(".election-filter-type input[type=checkbox]:checked", $selectRecipientsTab)
            .each(function() {
              filterTypes.push($(this).next().text());
            });
          criteriaLines.push('<b>Election Filter Types:</b> ' + filterTypes.join(", "));
          criteriaLines.push('<b>Election Filter Options:</b> ' +
            $(".election-filter-options input[type=radio]:checked", $selectRecipientsTab)
            .next().text());
          var $viewableCheckbox = $(
            ".viewable-election-type-filtering input[type=checkbox]",
            $selectRecipientsTab);
          if ($viewableCheckbox.prop("checked"))
            criteriaLines.push('<b>' + $viewableCheckbox.next().text() + '</b>');
          break;

        case "Single":
          criteriaLines.push('<b>Selected election:</b> ' +
            $(".selected-election-desc span").text());
          var $allPrimariesCheckbox = $(".include-all-primaries input[type=checkbox]",
            $selectRecipientsTab);
          if ($allPrimariesCheckbox.prop("checked"))
            criteriaLines.push('<b>' + $allPrimariesCheckbox.next().text() + '</b>');
          if (isCandidateSelection) {
            var radio = $(".ad-filtering input[type=radio]:checked");
            criteriaLines.push('<b>' + radio.next().text() + '</b>');
          }
          break;
        }
        break;

      case "WebsiteVisitors":
        criteriaLines.push('<b>District Filtering:</b> ' +
          $(".district-filtering input[type=radio]:checked", $selectRecipientsTab)
          .next().text());
        if (getDistrictFiltering() != "NoFiltering")
          criteriaLines.push('<b>Selected Districts:</b> ' +
            getDistrictFilteringCodes(true).join(","));
        $(".visitor-options input[type=checkbox]:checked").each(function() {
          criteriaLines.push('<b>' + $(this).next().text() + '</b>');
        });
        if (getValidatedVisitorStartDate(true))
          criteriaLines.push('<b>Start Date:</b> ' +
            moment($(".visitor-options .start-date", $selectRecipientsTab).val())
            .format("MM/DD/YYYY"));
        if (getValidatedVisitorEndDate(true))
          criteriaLines.push('<b>End Date:</b> ' +
            moment($(".visitor-options .end-date", $selectRecipientsTab).val())
            .format("MM/DD/YYYY"));
        break;

      case "Donors":
        criteriaLines.push('<b>District Filtering:</b> ' +
          $(".district-filtering input[type=radio]:checked", $selectRecipientsTab)
          .next().text());
        if (getDistrictFiltering() != "NoFiltering")
          criteriaLines.push('<b>Selected Districts:</b> ' +
            getDistrictFilteringCodes(true).join(","));
        if (getValidatedDonorStartDate(true))
          criteriaLines.push('<b>Start Date:</b> ' +
            moment($(".donor-options .start-date", $selectRecipientsTab).val())
            .format("MM/DD/YYYY"));
        if (getValidatedDonorEndDate(true))
          criteriaLines.push('<b>End Date:</b> ' +
            moment($(".donor-options .end-date", $selectRecipientsTab).val())
            .format("MM/DD/YYYY"));
        criteriaLines.push('<b>District Coding:</b> ' +
          $(".donor-options .district-coding input[type=radio]:checked",
            $selectRecipientsTab)
          .next().text());
          break;

        case "Organizations":
          criteriaLines.push('<b>Contacts to Use:</b> ' +
            $(".org-contacts-buttons input[type=radio]:checked", $selectRecipientsTab)
            .next().text());
          criteriaLines.push('<b>Organization Type:</b> ' +
            $(".select-org-type option:selected", $selectRecipientsTab).text());
          criteriaLines.push('<b>Organization SubType:</b> ' +
            $(".select-org-subtype option:selected", $selectRecipientsTab).text());
          criteriaLines.push('<b>Ideology:</b> ' +
            $(".select-ideology option:selected", $selectRecipientsTab).text());
          criteriaLines.push('<b>EmailTags:</b> ' + getEmailTags());
          break;
      }

      function formatDateRange(startDate, endDate) {
        if (startDate) {
          if (endDate)
            return " between " +
              moment(startDate).format("MM/DD/YYYY") +
              " and " +
              moment(endDate).format("MM/DD/YYYY");
          else return " on or after " + moment(startDate).format("MM/DD/YYYY");
        }
        if (endDate)
          return " on or before " + moment(endDate).format("MM/DD/YYYY");
        return "";
      }

      function formatDidOrNot(didOrNot) {
        return didOrNot == "did" ? "Did" : "Did not";
      }

      if (getFilterByEmailTypeChecked()) {
        criteriaLines.push('<b>Email Type:</b> ' +
          formatDidOrNot(getFilterByEmailTypeDidOrNot()) +
          " receive email of type " +
          getFilterByEmailTypeDescription() +
          formatDateRange(getValidatedEmailTypeStartDate(true),
            getValidatedEmailTypeEndDate(true)));
      }

      switch (recipientType) {
      case "AllCandidates":
      case "StateCandidates":
      case "CountyCandidates":
      case "LocalCandidates":
      case "PartyOfficials":
      case "Volunteers":
        if (getFilterByLoginDateChecked()) {
          criteriaLines.push('<b>Login Date:</b> ' +
            formatDidOrNot(getFilterByLoginDateDidOrNot()) +
            " login " +
            formatDateRange(getValidatedLoginDateStartDate(true),
              getValidatedLoginDateEndDate(true)));
        }
        break;
      }


      return '<p>' + criteriaLines.join('</p><p>') + '</p>';
    }

    function formatSelectionCriteriaAsJson() {
      return JSON.stringify(formatSelectionCriteriaAsObject());
    }

    function formatSelectionCriteriaAsObject() {
      return {
        contactType: getRecipientType(),
        stateCodes: rjc.getCategoryCodes("states", true),
        countyCodes: rjc.getCategoryCodes("counties", true),
        localKeysOrCodes: rjc.getCategoryCodes("locals", true),
        partyKeys: rjc.getCategoryCodes("parties", true),
        majorParties: rjc.getIsMajorPartiesChecked(),
        useBothContacts: getUseBothContacts(),
        politicianEmailsToUse: getEmailsToUse(),
        electionFiltering: getElectionFiltering(),
        electionFilterTypes: getElectionFilterTypes(),
        useFutureElections: getUseFutureElection(),
        viewableOnly: getViewableElectionsOnly(),
        electionKey: electionControl.getSelectedElection(),
        includeAllElectionsOnDate: getIncludeAllElectionsOnDate(),
        adFiltering: getAdFiltering(),
        visitorOptions: {
          DistrictFiltering: getDistrictFiltering(),
          Districts: getDistrictFilteringCodes(true),
          SampleBallots: getVisitorOption("SampleBallots"),
          SharedChoices: getVisitorOption("SharedChoices"),
          Donated: getVisitorOption("Donated"),
          WithNames: getVisitorOption("WithNames"),
          WithoutNames: getVisitorOption("WithoutNames"),
          WithDistrictCoding: getVisitorOption("WithDistrictCoding"),
          WithoutDistrictCoding: getVisitorOption("WithoutDistrictCoding"),
          StartDate: getValidatedVisitorStartDate(true),
          EndDate: getValidatedVisitorEndDate(true)
        },
        donorOptions: {
          StartDate: getValidatedDonorStartDate(true),
          EndDate: getValidatedDonorEndDate(true),
          DistrictCoding: getDonorDistrictCoding()
        },
        orgOptions: {
          OrgTypeId: $(".select-org-type", $selectRecipientsTab).val(),
          OrgSubTypeId: $(".select-org-subtype", $selectRecipientsTab).val(),
          IdeologyId: $(".select-ideology", $selectRecipientsTab).val(),
          EmailTagIds: getEmailTagIds(),
          UseAllContacts: getUseAllContacts()
        },
        emailTypeOptions: {
          Checked: getFilterByEmailTypeChecked(),
          DidReceive: getFilterByEmailTypeDidOrNot() === "did",
          EmailType: getFilterByEmailType(),
          StartDate: getValidatedEmailTypeStartDate(true),
          EndDate: getValidatedEmailTypeEndDate(true)
        },
        loginDateOptions: {
          Checked: getFilterByLoginDateChecked(),
          DidLogin: getFilterByLoginDateDidOrNot() === "did",
          StartDate: getValidatedLoginDateStartDate(true),
          EndDate: getValidatedLoginDateEndDate(true)
        }
      };
    }

    function getAdFiltering() {
      return $(".ad-filtering input[type=radio]:checked", $selectRecipientsTab).val();
    }

    function getAvailableRequirements() {
      // get available (requirements) based on the recipientType and filterType
      var available;
      var filterType = getElectionFiltering();
      var hasElection = filterType === "HasType" || filterType === "Single";
      switch (getRecipientType()) {
      case "All":
      case "State":
        available = ["statecode"];
        if (hasElection) available.push("electionkey");
        break;

      case "County":
        available = ["statecode", "countycode"];
        if (hasElection) available.push("electionkey");
        break;

      case "Local":
        available = ["statecode", "countycode", "localkey"];
        if (hasElection) available.push("electionkey");
        break;

      case "AllCandidates":
      case "StateCandidates":
        available = [
          "statecode", "politiciankey", "issuekey", "partykey"
        ];
        if (getElectionFiltering() !== "NoFiltering") {
          available.push("electionkey");
          available.push("officekey");
        }
        break;

      case "CountyCandidates":
        available = [
          "statecode", "countycode", "politiciankey",
          "electionkey", "officekey", "issuekey", "partykey"
        ];
        break;

      case "LocalCandidates":
        available = [
          "statecode", "countycode", "localkey", "politiciankey",
          "electionkey", "officekey", "issuekey", "partykey"
        ];
        break;

      case "PartyOfficials":
        available = ["statecode", "partykey", "partyemail"];
        if (hasElection) available.push("electionkey");
        break;

      case "Volunteers":
        available = ["statecode", "partykey", "partyemail"];
        break;

      case "WebsiteVisitors":
        available = ["statecode", "countycode", "visitorid"];
        break;

      case "Donors":
        available = ["statecode", "countycode", "visitorid", "donorid"];
        break;

      case "Organizations":
        available = ["statecode", "orgcontactid"];
        break;

      default:
        available = [];
      }

      // for embedded keys, add them to available
      var body = bodyEditor.getValue();
      var regex = /\[\[([\da-z]+)=([\da-z]+)\]\]/ig;
      var match = regex.exec(body);
      while (match != null) {
        var key = match[1].toLowerCase();
        if ($.inArray(key, allRequirements) >= 0)
          if ($.inArray(key, available) < 0)
            available.push(key);
        match = regex.exec(body);
      }

      return available;
    }

    function getDistrictFiltering() {
      return $(".district-filtering input[type=radio]:checked", $selectRecipientsTab).val();
    }

    function getDistrictFilteringCodes(reportAll) {
      if (reportAll === true &&
        $(".district-filtering .all", $selectRecipientsTab).prop("checked"))
        return ["all"];
      var codes = [];
      $.each($(".district-filtering .checkbox-list input[type=checkbox]:checked",
          $selectRecipientsTab),
        function() {
          codes.push($(this).val());
        });
      return codes;
    }

    function getDistrictFilteringCodesNotChecked() {
      var codes = [];
      $.each($(".district-filtering .checkbox-list input[type=checkbox]:not(:checked)",
          $selectRecipientsTab),
        function() {
          codes.push($(this).val());
        });
      return codes;
    }

    function getDonorDistrictCoding() {
      return $(".donor-options .district-coding input[type=radio]:checked",
        $selectRecipientsTab).val();
    }

    function getElectionFiltering() {
      return $(".election-filtering input[type=radio]:checked", $selectRecipientsTab).val();
    }

    function getElectionFilterTypes() {
      var codes = [];
      $(".election-filter-type input[type=checkbox]:checked", $selectRecipientsTab).each(
        function() {
          codes.push($(this).val());
        });
      return codes;
    }

    function getEmailsToUse() {
      var codes = [];
      $(".emails-checkboxes input[type=checkbox]:checked", $selectRecipientsTab).each(
        function() {
          codes.push($(this).val());
        });
      return codes;
    }

    function getEmailTagIds() {
      var tags = [];
      $(".email-tags input[type=checkbox]:checked", $selectRecipientsTab)
        .each(function () { tags.push($(this).val()) });
      return tags;
    }

    function getEmailTags() {
      var tags = [];
      $(".email-tags input[type=checkbox]:checked", $selectRecipientsTab)
        .each(function () { tags.push($(this).parent().text()) });
      return tags.length ? tags.join(", ") : "any";
    }

    function getIncludeAllElectionsOnDate() {
      return $(".include-all-primaries input", $selectRecipientsTab).prop("checked");
    }

    function getRecipientType() {
      return $(".recipient-buttons input[type=radio]:checked", $selectRecipientsTab).val();
    }

    function getUseAllContacts() {
      return $(".use-all-contacts input", $selectRecipientsTab).prop("checked");
    }

    function getUseBothContacts() {
      return $(".use-both-contacts input", $selectRecipientsTab).prop("checked");
    }

    function getUseFutureElection() {
      return $(".election-filter-options input[type=radio]:checked", $selectRecipientsTab)
        .val() ===
        "Future";
    }

    function getValidatedDonorStartDate(silent) {
      var $input = $(".donor-options .start-date", $selectRecipientsTab);
      var val = $.trim($input.val());
      if (!val) return ""; // empty is ok
      val = util.getUtcDateFromLocalTime(val);
      if (!val) {
        // now empty means an error
        if (!silent) {
          util.alert("The Start Date is not valid");
          $input.addClass("error");
        }
        return null;
      }
      return val;
    }

    function getValidatedDonorEndDate(silent) {
      var $input = $(".donor-options .end-date", $selectRecipientsTab);
      var val = $.trim($input.val());
      if (!val) return ""; // empty is ok
      val = util.getUtcDateFromLocalTime(val);
      if (!val) {
        // now empty means an error
        if (!silent) {
          util.alert("The End Date is not valid");
          $input.addClass("error");
        }
        return null;
      }
      return val;
    }

    function getValidatedEmailTypeStartDate(silent) {
      var $input = $(".start-date", $filterByEmailTypeDialog);
      var val = $.trim($input.val());
      if (!val) return ""; // empty is ok
      val = util.getUtcDateFromLocalTime(val);
      if (!val) {
        // now empty means an error
        if (!silent) {
          util.alert("The Start Date is not valid");
          $input.addClass("error");
        }
        return null;
      }
      return val;
    }

    function getValidatedEmailTypeEndDate(silent) {
      var $input = $(".end-date", $filterByEmailTypeDialog);
      var val = $.trim($input.val());
      if (!val) return ""; // empty is ok
      val = util.getUtcDateFromLocalTime(val);
      if (!val) {
        // now empty means an error
        if (!silent) {
          util.alert("The End Date is not valid");
          $input.addClass("error");
        }
        return null;
      }
      return val;
    }

    function getValidatedLoginDateStartDate(silent) {
      var $input = $(".start-date", $filterByLoginDateDialog);
      var val = $.trim($input.val());
      if (!val) return ""; // empty is ok
      val = util.getUtcDateFromLocalTime(val);
      if (!val) {
        // now empty means an error
        if (!silent) {
          util.alert("The Start Date is not valid");
          $input.addClass("error");
        }
        return null;
      }
      return val;
    }

    function getValidatedLoginDateEndDate(silent) {
      var $input = $(".end-date", $filterByLoginDateDialog);
      var val = $.trim($input.val());
      if (!val) return ""; // empty is ok
      val = util.getUtcDateFromLocalTime(val);
      if (!val) {
        // now empty means an error
        if (!silent) {
          util.alert("The End Date is not valid");
          $input.addClass("error");
        }
        return null;
      }
      return val;
    }

    function getValidatedVisitorStartDate(silent) {
      var $input = $(".visitor-options .start-date", $selectRecipientsTab);
      var val = $.trim($input.val());
      if (!val) return ""; // empty is ok
      val = util.getUtcDateFromLocalTime(val);
      if (!val) {
        // now empty means an errorv
        if (!silent) {
          util.alert("The Start Date is not valid");
          $input.addClass("error");
        }
        return null;
      }
      return val;
    }

    function getValidatedVisitorEndDate(silent) {
      var $input = $(".visitor-options .end-date", $selectRecipientsTab);
      var val = $.trim($input.val());
      if (!val) return ""; // empty is ok
      val = util.getUtcDateFromLocalTime(val);
      if (!val) {
        // now empty means an errorv
        if (!silent) {
          util.alert("The End Date is not valid");
          $input.addClass("error");
        }
        return null;
      }
      return val;
    }

    function getViewableElectionsOnly() {
      return $(".viewable-election-type-filtering input", $selectRecipientsTab)
        .prop("checked");
    }

    function getVisitorOption(option) {
      return $(".visitor-options input[value=" + option + "]", $selectRecipientsTab)
        .prop("checked");
    }

    function initElectionControl(html, stateCode, countyCode, localKey) {
      $(".election-control").html(html);
      electionControl.init({
        onSelectedElectionChanged: onSelectedElectionChanged,
        slimScrollOptions: { height: '440px' }
      });
      electionsLoadedState = stateCode;
      electionsLoaded.stateCode = stateCode;
      electionsLoaded.countyCode = countyCode || "";
      electionsLoaded.localKey = localKey || "";
    }

    function loadElections(stateCode, countyCode, localKey) {
      electionsLoadedState = null;
      electionsLoaded = $.extend({}, emptyElectionsLoaded);
      util.openAjaxDialog("Getting elections...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetElectionControlHtml",
        data: {
          stateCode: stateCode,
          countyCode: countyCode,
          localKey: localKey
        },

        success: function(result) {
          initElectionControl(result.d, stateCode, countyCode, localKey);
          util.closeAjaxDialog();
        },

        error: function(result) {
          util.alert(util.formatAjaxError(result,
              "Could not get elections"),
            function() {
              util.closeAjaxDialog();
            });
          $(".election-control")
            .html(
              '<div class="no-elections">An error occurred while loading the elections</div>');
          electionControl.init({
            slimScrollOptions: { height: '440px' }
          });
        }
      });
    }

    function onChangeJurisdiction() {
      updateRecipientOptionEnabling();
      onChangeOptions();
    }

    function onDistrictFilteringAllChange() {

      var checked = $(".district-filtering .all", $selectRecipientsTab).prop("checked");
      $.each($(".district-filtering .checkbox-list input[type=checkbox]",
        $selectRecipientsTab), function() {
        $(this).prop("checked", checked);
      });
      onChangeOptions();
    }

    function onDistrictFilteringChange() {
      $(".district-filtering .checkbox-list").addClass("hidden").html("");
      $(".district-filtering .all").prop("checked", true).prop("disabled", true);
      switch (getDistrictFiltering()) {
      case "CongressionalDistrict":
        populateDistrictFilterList("GetCongressionalDistricts");
        break;

      case "StateSenateDistrict":
        populateDistrictFilterList("GetStateSenateDistricts");
        break;

      case "StateHouseDistrict":
        populateDistrictFilterList("GetStateHouseDistricts");
        break;
      }
      onChangeOptions();
    }

    function onDistrictFilteringListChange() {
      var unchecked = getDistrictFilteringCodesNotChecked().length;

      // update the all checkbox
      var $all = $(".district-filtering .all", $selectRecipientsTab);
      $all.prop("checked", unchecked === 0);
      onChangeOptions();
    }

    function onElectionFilteringChange() {
      updateRecipientOptionEnabling();
    }

    function onOrgTypeChange() {
      // update the SubType and Email Tags
      var orgTypeId = $(".select-org-type", $selectRecipientsTab).val();
      var orgData = $("body").data("org-data");
      var orgInfo = null;
      for (var inx = 0; inx < orgData.OrgTypes.length; inx++)
        if (orgData.OrgTypes[inx].OrgTypeId == orgTypeId) {
          orgInfo = orgData.OrgTypes[inx];
          break;
        }
      if (orgInfo) {
        var items = [];
        var subTypes = orgInfo.SubTypes;
        for (var i = 0; i < subTypes.length; i++)
          items.push({
            Value: subTypes[i].OrgSubTypeId,
            Text: subTypes[i].OrgSubType
          });
        util.populateDropdown($(".select-org-subtype", $selectRecipientsTab), items, "All subtypes", "0");
        var tags = orgInfo.EmailTags;
        var $tags = $(".email-tags", $selectRecipientsTab);
        if (tags.length === 0)
          $tags.text("No tags for this organization type");
        else {
          var checkboxes = [];
          for (var j = 0; j < tags.length; j++)
            checkboxes.push('<div><input type="checkbox" class="is-option-click" value="' + tags[j].EmailTagId + '"/>' +
              tags[j].EmailTag + '</div>');
          $tags.html(checkboxes.join(""));
        }
      }
    }

    function onRecipientsCreateRecipientsList() {
      if (!editRecipientSelections()) return;
      util.openAjaxDialog("Creating Recipient List...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetEmailBatchId",

        success: function(result) {
          pendingRecipientBatchId = result.d;
          postEmailBatchStatus();
          doRecipientsList();
        },

        error: function(result) {
          doRecipientsListError(result);
        }
      });
    }

    function onRecipientTypeChange() {
      updateRecipientOptionEnabling();
    }

    function onSelectedElectionChanged(newElectionKey) {
      $(".selected-election-desc span", $selectRecipientsTab)
        .safeHtml(electionControl.getElectionDesc(newElectionKey))
        .removeClass("disabled");
      $(".include-all-primaries", $selectRecipientsTab)
        .toggleClass("hidden",
          !electionControl.hasOtherElectionsOnSameDate(newElectionKey));
      $(".include-all-primaries input", $selectRecipientsTab).prop("checked", false);
      electionControl.toggleElectionControl(false);
      onChangeOptions();
    }

    function populateDistrictFilterList(serviceName) {
      util.ajax({
        url: "/Admin/WebService.asmx/" + serviceName,
        data: {
          stateCode: rjc.getSingleStateCode()
        },

        success: function(result) {
          var $list = $(".district-filtering .checkbox-list", $selectRecipientsTab);
          if (result.d.length) {
            util.populateCheckboxList($list, result.d);
            $(".district-filtering .all", $selectRecipientsTab).prop("disabled", false);
          } else {
            $list.html('<em>No district information is available</em>');
          }
          $list.removeClass("hidden");
        }
      });
    }

    function postEmailBatchStatus() {
      if (!pendingRecipientBatchId) return;

      util.ajax({
        url: "/Admin/WebService.asmx/GetEmailBatchStatus",
        data: {
          batchId: pendingRecipientBatchId
        },

        success: function(result) {
          util.setAjaxDialogStatus(result.d
            ? result.d + " recipients found"
            : "Finding recipients...");
          if (pendingRecipientBatchId) {
            setTimeout(postEmailBatchStatus, postEmailBatchStatusInterval);
          }
        },
        error: function() {
          if (pendingRecipientBatchId) {
            setTimeout(postEmailBatchStatus, postEmailBatchStatusInterval);
          }
        }

      });
    }

    function restoreSelectRecipientOptions(o, restoreInfo, onCompletion) {
      // see if we need to get any restore info (dropdown contents)
      if (!restoreInfo) {
        var allStates = o.stateCodes.length === 1 && o.stateCodes[0] === "all";
        var allCounties = o.countyCodes.length === 1 && o.countyCodes[0] === "all";
        var allLocals = o.localKeysOrCodes.length === 1 && o.localKeysOrCodes[0] === "all";
        var allParties = o.partyKeys.length === 1 && o.partyKeys[0] === "all";
        var isSingleElection = o.electionFiltering === "Single";
        var hasDistricts = o.visitorOptions.DistrictFiltering !== "NoFiltering";
        if (!allCounties ||
          !allLocals ||
          !allParties ||
          isSingleElection ||
          hasDistricts) {
          util.openAjaxDialog("Getting option restore info...");
          util.ajax({
            url: "/Admin/WebService.asmx/GetRestoreInfo",
            data: {
              stateCode: !allStates && o.stateCodes.length === 1
                ? o.stateCodes[0]
                : "",
              countyCode: !allCounties && o.countyCodes.length === 1
                ? o.countyCodes[0]
                : "",
              localKey: !allLocals && o.localKeysOrCodes.length === 1
                ? o.localKeysOrCodes[0]
                : "",
              districtFiltering: o.visitorOptions.DistrictFiltering,
              getCounties: !allCounties,
              getLocals: !allLocals,
              getParties: !allParties,
              getElections: isSingleElection,
              getBestCounty: null
            },

            success: function(result) {
              util.closeAjaxDialog();
              restoreSelectRecipientOptions(o, result.d, onCompletion);
            },

            error: function(result) {
              util.closeAjaxDialog();
              util.alert(util.formatAjaxError(result,
                "Could not get the option restore info"));
            }
          });
          return;
        }
      }

      restoreInfo = restoreInfo || {};
      $(".recipient-buttons input[value=" + o.contactType + "]", $selectRecipientsTab)
        .prop("checked", true);
      rjc.restore(o, restoreInfo);
      $(".contacts-buttons input[value=" +
        (o.useBothContacts ? "Both" : "Main") +
        "]", $selectRecipientsTab).prop("checked", true);
      $(".emails-checkboxes input", $selectRecipientsTab).prop("checked", false);
      $.each(o.politicianEmailsToUse, function() {
        $(".emails-checkboxes input[value=" + this + "]", $selectRecipientsTab)
          .prop("checked", true);
      });
      $(".election-filtering input[value=" + o.electionFiltering + "]",
        $selectRecipientsTab).prop("checked", true);
      $(".ad-filtering input[value=" + o.adFiltering + "]",
        $selectRecipientsTab).prop("checked", true);
      $(".election-filter-type input", $selectRecipientsTab).prop("checked", false);
      $.each(o.electionFilterTypes, function() {
        $(".election-filter-type input[value=" + this + "]", $selectRecipientsTab)
          .prop("checked", true);
      });
      $(".election-filter-options input[value=" +
        (o.useFutureElections ? "Future" : "Past") +
        "]", $selectRecipientsTab).prop("checked", true);
      $(".election-filter-options input[type=checkbox]", $selectRecipientsTab)
        .prop("checked", o.viewableOnly);
      if (restoreInfo.ElectionControlHtml) {
        initElectionControl(restoreInfo.ElectionControlHtml, restoreInfo.StateCode,
          restoreInfo.CountyCode, restoreInfo.LocalKey);
        if (o.electionKey)
          electionControl.changeElection(o.electionKey);
      }
      $(".include-all-primaries input", $selectRecipientsTab)
        .prop("checked", o.includeAllElectionsOnDate);
      $(".district-filtering input[value=" + o.visitorOptions.DistrictFiltering + "]",
        $selectRecipientsTab).prop("checked", true);
      $(".district-filtering .checkbox-list", $selectRecipientsTab).addClass("hidden")
        .html("");
      $(".district-filtering .all", $selectRecipientsTab).prop("checked", true)
        .prop("disabled", true);
      if (o.visitorOptions.DistrictFiltering !== "NoFiltering") {
        var $list = $(".district-filtering .checkbox-list", $selectRecipientsTab);
        if (restoreInfo.Districts && restoreInfo.Districts.length) {
          util.populateCheckboxList($list, restoreInfo.Districts);
          $(".district-filtering .all", $selectRecipientsTab)
            .prop("disabled", false);
          if (o.visitorOptions.Districts.length !== 1 ||
            o.visitorOptions.Districts[0] != "all") {
            $(".district-filtering .all", $selectRecipientsTab).prop("checked", false);
            $("input", $list).prop("checked", false);
            $.each(o.visitorOptions.Districts, function() {
              $("input[value=" + this + "]", $list).prop("checked", true);
            });
          }
        } else
          $list.html('<em>No district information is available</em>');
        $list.removeClass("hidden");
      }
      $.each([
          "SampleBallots", "SharedChoices", "Donated",
          "WithNames", "WithoutNames", "WithDistrictCoding", "WithoutDistrictCoding"
        ],
        function() {
          $(".visitor-options input[value=" + this + "]", $selectRecipientsTab)
            .prop("checked", o.visitorOptions[this]);
        });
      var date = o.visitorOptions.StartDate
        ? moment.utc(o.visitorOptions.StartDate, "YYYY/MM/DD HH:mm:ss.SSS")
        .format("M/D/YYYY")
        : "";
      $(".visitor-options-filter .start-date", $selectRecipientsTab).val(date);
      date = o.visitorOptions.EndDate
        ? moment.utc(o.visitorOptions.EndDate, "YYYY/MM/DD HH:mm:ss.SSS").format("M/D/YYYY")
        : "";
      $(".visitor-options-filter .end-date", $selectRecipientsTab).val(date);
      date = o.donorOptions.StartDate
        ? moment.utc(o.donorOptions.StartDate, "YYYY/MM/DD HH:mm:ss.SSS").format("M/D/YYYY")
        : "";
      $(".donor-options-filter .start-date", $selectRecipientsTab).val(date);
      date = o.donorOptions.EndDate
        ? moment.utc(o.donorOptions.EndDate, "YYYY/MM/DD HH:mm:ss.SSS").format("M/D/YYYY")
        : "";
      $(".donor-options-filter .end-date", $selectRecipientsTab).val(date);
      $(".donor-options .district-coding input[value=" +
        o.donorOptions.DistrictCoding +
        "]", $selectRecipientsTab).prop("checked", true);

      $(".org-contacts-buttons input[value=" +
        (o.orgOptions.UseAllContacts ? "All" : "Primary") +
        "]", $selectRecipientsTab).prop("checked", true);
      $(".select-org-type", $selectRecipientsTab).val(o.orgOptions.OrgTypeId);
      onOrgTypeChange();
      $(".select-org-subtype", $selectRecipientsTab).val(o.orgOptions.OrgSubTypeId);
      $(".select-ideology", $selectRecipientsTab).val(o.orgOptions.IdeologyId);
      $(".email-tags input[type=checkbox]", $selectRecipientsTab).each(function() {
        var $checkbox = $(this);
        $checkbox.prop("checked", o.orgOptions.EmailTagIds.indexOf($checkbox.val()) >= 0);
      });

      // the email type dialog
      $(".checkbox", $filterByEmailTypeDialog).prop("checked", o.emailTypeOptions.Checked);
      $("input[value=did]", $filterByEmailTypeDialog)
        .prop("checked", o.emailTypeOptions.DidReceive);
      $("input[value=didnot]", $filterByEmailTypeDialog)
        .prop("checked", !o.emailTypeOptions.DidReceive);
      $(".email-type-select", $filterByEmailTypeDialog).val(o.emailTypeOptions.EmailType);
      date = o.emailTypeOptions.StartDate
        ? moment.utc(o.emailTypeOptions.StartDate, "YYYY/MM/DD HH:mm:ss.SSS")
        .format("M/D/YYYY")
        : "";
      $(".start-date", $filterByEmailTypeDialog).val(date);
      date = o.emailTypeOptions.EndDate
        ? moment.utc(o.emailTypeOptions.EndDate, "YYYY/MM/DD HH:mm:ss.SSS")
        .format("M/D/YYYY")
        : "";
      $(".end-date", $filterByEmailTypeDialog).val(date);
      $(".filter-by-email-type .filtering-msg", $selectRecipientsTab).toggleClass(
        "invisible",
        !o.emailTypeOptions.Checked);

      // the login date dialog
      $(".checkbox", $filterByLoginDateDialog).prop("checked", o.loginDateOptions.Checked);
      $("input[value=did]", $filterByLoginDateDialog)
        .prop("checked", o.loginDateOptions.DidLogin);
      $("input[value=didnot]", $filterByLoginDateDialog)
        .prop("checked", !o.loginDateOptions.DidLogin);
      date = o.loginDateOptions.StartDate
        ? moment.utc(o.loginDateOptions.StartDate, "YYYY/MM/DD HH:mm:ss.SSS")
        .format("M/D/YYYY")
        : "";
      $(".start-date", $filterByLoginDateDialog).val(date);
      date = o.loginDateOptions.EndDate
        ? moment.utc(o.loginDateOptions.EndDate, "YYYY/MM/DD HH:mm:ss.SSS")
        .format("M/D/YYYY")
        : "";
      $(".end-date", $filterByLoginDateDialog).val(date);
      $(".filter-by-login-date .filtering-msg", $selectRecipientsTab).toggleClass(
        "invisible",
        !o.loginDateOptions.Checked);


      updateRecipientOptionEnabling();
      if (typeof onCompletion === "function") onCompletion();
    }

    function setDistrictFiltering(type) {
      $(".district-filtering input[value=" + type + "]", $selectRecipientsTab)
        .prop("checked", true);
    }

    function setElectionFiltering(type) {
      $(".election-filtering input[value=" + type + "]", $selectRecipientsTab)
        .prop("checked", true);
    }

    function updateRecipientOptionEnabling() {
      var recipientType = getRecipientType();
      var singleStateCode = rjc.getSingleStateCode();
      var singleCountyCode = rjc.getSingleCountyCode();
      var singleLocalKey = rjc.getSingleLocalKey();
      var filterType = getElectionFiltering();
      var districtFiltering = getDistrictFiltering();

      var countiesHidden = false;
      var localsHidden = false;
      var partiesHidden = false;
      var mainOrBothHidden = false;
      var primaryOrAllHidden = true;
      var emailTypesHidden = false;
      var newFilterType = null;
      var electionBoxHidden = false;
      var noElectionDisabled = false;
      var hasElectionDisabled = false;
      var hasntElectionDisabled = false;
      var singleElectionDisabled = false;
      var singleElectionMsgHidden = false;
      var electionOptionsHidden = false;
      var selectElectionHidden = false;
      var districtFilteringDisabled = false;
      var newDistrictFiltering = null;
      var districtBoxHidden = false;
      var orgBoxHidden = true;
      var visitorOptionsHidden = false;
      var donorOptionsHidden = false;
      var loginDateFilteringHidden = false;
      var adFilteringHidden = true;
      var singleElectionMsg = "";
      var isCandidateSelection = false;

      var singleIsValid = false;
      switch (recipientType) {
      case "AllCandidates":
      case "StateCandidates":
      case "PartyOfficials":
      case "WebsiteVisitors":
      case "Donors":
        singleIsValid = !!singleStateCode;
        singleElectionMsg = "Select a single state to enable single election selection.";
        break;

      case "CountyCandidates":
        singleIsValid = !!(singleStateCode && singleCountyCode);
        singleElectionMsg = "Select a single county to enable single election selection.";
        break;

      case "LocalCandidates":
        singleIsValid = !!(singleStateCode && singleCountyCode && singleLocalKey);
        singleElectionMsg =
          "Select a single local district to enable single election selection.";
        break;
      }

      // disable Single election filtering if !singleIsValid, but
      // show message
      singleElectionDisabled = !singleIsValid;
      singleElectionMsgHidden = singleIsValid;

      // if !singleIsValid, change a Single filter type to NoFiltering
      if (!singleIsValid && filterType === "Single")
        newFilterType = "NoFiltering";

      // recipientType enabling
      var effectiveFilterType = newFilterType || filterType;
      switch (recipientType) {
      case "All":
      case "State":
      case "County":
      case "Local":
        // jurisdictional contacts can't have single election filtering
        if (effectiveFilterType === "Single")
          newFilterType = "HasType";
        singleElectionDisabled = true;
        singleElectionMsgHidden = true;
        emailTypesHidden = true;
        districtBoxHidden = true;
        visitorOptionsHidden = true;
        donorOptionsHidden = true;
        districtFilteringDisabled = true;
        loginDateFilteringHidden = true;
        newDistrictFiltering = "NoFiltering";
        break;

      case "AllCandidates":
      case "StateCandidates":
      case "CountyCandidates":
      case "LocalCandidates":
        // candidates require either has election type or single election filtering
        // new: for StateCandidates only, allow NoFiltering
        if (effectiveFilterType === "NoFiltering" && recipientType !== "StateCandidates" ||
          effectiveFilterType === "HasntType")
          newFilterType = "HasType";
        noElectionDisabled = recipientType !== "StateCandidates";
        hasntElectionDisabled = true;
        mainOrBothHidden = true;
        districtBoxHidden = true;
        visitorOptionsHidden = true;
        donorOptionsHidden = true;
        districtFilteringDisabled = true;
        newDistrictFiltering = "NoFiltering";
        isCandidateSelection = true;
        break;

      case "PartyOfficials":
      case "Volunteers":
        if (effectiveFilterType === "HasType" || effectiveFilterType === "HasntType")
          newFilterType = "NoFiltering";
        mainOrBothHidden = true;
        emailTypesHidden = true;
        hasElectionDisabled = true;
        hasntElectionDisabled = true;
        districtBoxHidden = true;
        visitorOptionsHidden = true;
        donorOptionsHidden = true;
        districtFilteringDisabled = true;
        newDistrictFiltering = "NoFiltering";
        electionBoxHidden = recipientType === "Volunteers";
        break;

      case "WebsiteVisitors":
      case "Donors":
        mainOrBothHidden = true;
        emailTypesHidden = true;
        electionBoxHidden = true;
        districtFilteringDisabled = !singleIsValid;
        newDistrictFiltering = getDistrictFiltering();
        visitorOptionsHidden = recipientType === "Donors";
        donorOptionsHidden = recipientType === "WebsiteVisitors";
        loginDateFilteringHidden = true;
        break;
      }

      // jurisdiction boxes
      effectiveFilterType = newFilterType || filterType;
      switch (recipientType) {
      case "All":
      case "State":
      case "AllCandidates":
      case "StateCandidates":
        countiesHidden = true;
        localsHidden = true;
        partiesHidden = true;
        break;

      case "County":
      case "CountyCandidates":
        localsHidden = true;
        partiesHidden = true;
        break;

      case "Local":
      case "LocalCandidates":
        partiesHidden = true;
        break;

      case "PartyOfficials":
      case "Volunteers":
        countiesHidden = true;
        localsHidden = true;
        partiesHidden = effectiveFilterType !== "NoFiltering";
        break;

      case "WebsiteVisitors":
      case "Donors":
        countiesHidden = false;
        localsHidden = true;
        partiesHidden = true;
        break;

        case "Organizations":
          countiesHidden = true;
          localsHidden  = true;
          partiesHidden  = true;
          mainOrBothHidden = true;
          primaryOrAllHidden = false;
          emailTypesHidden  = true;
          electionBoxHidden  = true;
          districtBoxHidden = true;
          orgBoxHidden = false;
          visitorOptionsHidden  = true;
          donorOptionsHidden  = true;
          loginDateFilteringHidden  = true;
        break;
      }

      effectiveFilterType = newFilterType || filterType;
      var isNoElection = effectiveFilterType === "NoFiltering";
      var isSingleElection = effectiveFilterType === "Single";

      // election options
      electionOptionsHidden = isNoElection || isSingleElection;
      selectElectionHidden = !isSingleElection;
      adFilteringHidden = !isCandidateSelection || !isSingleElection;

      // apply
      rjc.toggleLevel("counties", !countiesHidden);
      rjc.toggleLevel("locals", !localsHidden);
      rjc.toggleLevel("parties", !partiesHidden);
      $(".contacts-buttons", $selectRecipientsTab).toggleClass("hidden", mainOrBothHidden);
      $(".org-contacts-buttons", $selectRecipientsTab).toggleClass("hidden", primaryOrAllHidden);
      $(".emails-checkboxes", $selectRecipientsTab).toggleClass("hidden", emailTypesHidden);
      $(".elections-options", $selectRecipientsTab)
        .toggleClass("hidden", electionBoxHidden);
      $(".district-options", $selectRecipientsTab).toggleClass("hidden", districtBoxHidden);
      $(".org-options", $selectRecipientsTab).toggleClass("hidden", orgBoxHidden);
      $(".visitor-options", $selectRecipientsTab)
        .toggleClass("hidden", visitorOptionsHidden);
      $(".donor-options", $selectRecipientsTab).toggleClass("hidden", donorOptionsHidden);
      $(".no-election-filtering input", $selectRecipientsTab)
        .prop("disabled", noElectionDisabled);
      $(".has-election-type-filtering input", $selectRecipientsTab)
        .prop("disabled", hasElectionDisabled);
      $(".hasnt-election-type-filtering input", $selectRecipientsTab)
        .prop("disabled", hasntElectionDisabled);
      $(".single-election-filtering input", $selectRecipientsTab)
        .prop("disabled", singleElectionDisabled);
      $(".single-election-filtering-msg", $selectRecipientsTab)
        .toggleClass("hidden", singleElectionMsgHidden)
        .safeHtml(singleElectionMsg);
      $(".election-filter-type", $selectRecipientsTab)
        .toggleClass("hidden", electionOptionsHidden);
      $(".election-filter-options", $selectRecipientsTab)
        .toggleClass("hidden", electionOptionsHidden);
      $(".ad-filtering", $selectRecipientsTab)
        .toggleClass("hidden", adFilteringHidden);
      $(".select-election-box", $selectRecipientsTab)
        .toggleClass("hidden", selectElectionHidden);
      $(".visitor-district-filtering-msg", $selectRecipientsTab)
        .toggleClass("hidden", !districtFilteringDisabled);
      $(".district-filtering input[type=radio]", $selectRecipientsTab)
        .prop("disabled", districtFilteringDisabled);
      $(".filter-by-login-date", $selectRecipientsTab)
        .toggleClass("hidden", loginDateFilteringHidden);

      if (isSingleElection) {
        if (singleStateCode !== electionsLoaded.stateCode ||
          singleCountyCode !== electionsLoaded.countyCode ||
          singleLocalKey !== electionsLoaded.localKey) {
          // initiate election reload for new state
          $(".selected-election-desc span", $selectRecipientsTab).html("none")
            .addClass("disabled");
          $(".include-all-primaries", $selectRecipientsTab).addClass("hidden");
          $(".include-all-primaries input", $selectRecipientsTab).prop("checked", false);
          loadElections(singleStateCode, singleCountyCode, singleLocalKey);
        }
      }

      if (newFilterType !== filterType)
        setElectionFiltering(newFilterType);

      if (newDistrictFiltering !== districtFiltering) {
        setDistrictFiltering(newDistrictFiltering);
        onDistrictFilteringChange();
      }
    }

    //
    // View Recipients Tab
    //

    var viewRecipientsName = "viewrecipients";
    var viewRecipientsTabName = "tab-" + viewRecipientsName;
    var $viewRecipientsTab;
    var $viewRecipientsSubsetDialog;

    function initViewRecipients() {
      $viewRecipientsTab = $$(viewRecipientsTabName);
      $(".current-list-results", $viewRecipientsTab).safeBind("click", onClickListResults);
      $(".current-list-results", $viewRecipientsTab)
        .safeBind("dblclick", onDblClickListResults);

      util.onContextMenu($("#recipients-context-menu"), onViewRecipientsContextMenuDisplay);
      $("#recipients-context-menu .info").safeBind("click", onClickMoreViewRecipientsInfo);
      $('#more-recipient-info').dialog({
        autoOpen: false,
        dialogClass: 'more-recipient-info',
        modal: true,
        resizable: false,
        title: "More Recipient Information",
        width: "500px"
      });

      $viewRecipientsSubsetDialog = $$("view-recipients-subset-dialog");
      $viewRecipientsSubsetDialog.dialog({
        autoOpen: false,
        dialogClass: "view-recipients-subset-dialog",
        modal: true,
        resizable: false,
        title: "View Recipients Subset",
        width: "auto"
      });
      $(".view-recipients-subset-ok", $viewRecipientsSubsetDialog)
        .click(onViewRecipientsSubsetOk);

      $viewRecipientsTab.on("click", ".select-subset", function() {
        $viewRecipientsSubsetDialog.dialog("open");
      });
    }

    function addInfoTableRow($table, label, value) {
      $("tbody", $table).append('<tr class="added"><td class="col1">' +
        label +
        '</td><td class="col2">' +
        util.htmlEscape(value) +
        '</td></tr>');
    }

    function displayMoreViewRecipientInfo(info) {
      var $dialog = $("#more-recipient-info");
      var $table = $("table", $dialog);
      $(".email .col2", $table).safeHtml(info.Email);
      $(".contact .col2", $table).safeHtml(info.Contact);
      $(".title .col1", $table).safeHtml(getTitleString());
      $(".title .col2", $table).safeHtml(info.Title);
      $(".jurisdiction .col2", $table).safeHtml(info.Jurisdiction);
      $(".phone .col2", $table).safeHtml(info.Phone);

      $("tr.added", $table).remove();

      // remaining items only display if present
      if (info.detail.Election)
        addInfoTableRow($table, "Election",
          info.detail.Election);
      if (info.detail.Office)
        addInfoTableRow($table, "Office",
          info.detail.Office);
      if (info.detail.Party)
        addInfoTableRow($table, "Party",
          info.detail.Party);
      var dateVisited = moment(info.detail.DateAdded);
      if (dateVisited.year() > 1990)
        addInfoTableRow($table, "Date Visited",
          dateVisited.format("MM/DD/YYYY"));
      if (info.detail.Address)
        addInfoTableRow($table, "Address",
          info.detail.Address);
      if (info.detail.CityStateZip)
        addInfoTableRow($table, "City State Zip",
          info.detail.CityStateZip);
      if (info.detail.CongressionalDistrict)
        addInfoTableRow($table, "Congressional District",
          info.detail.CongressionalDistrict);
      if (info.detail.StateSenateDistrict)
        addInfoTableRow($table, "State Senate District",
          info.detail.StateSenateDistrict);
      if (info.detail.StateHouseDistrict)
        addInfoTableRow($table, "State House District",
          info.detail.StateHouseDistrict);
      if (info.detail.OrgAbbreviation)
        addInfoTableRow($table, "Short Name",
          info.detail.OrgAbbreviation);
      if (info.detail.OrgType)
        addInfoTableRow($table, "Organization Type",
          info.detail.OrgType);
      if (info.detail.OrgSubType)
        addInfoTableRow($table, "Sub Type",
          info.detail.OrgSubType);
      if (info.detail.Ideology)
        addInfoTableRow($table, "Ideology",
          info.detail.Ideology);
      if (info.detail.Address1)
        addInfoTableRow($table, "Address Line 1",
          info.detail.Address1);
      if (info.detail.Address2)
        addInfoTableRow($table, "Address Line 2",
          info.detail.Address2);
      if (info.detail.City)
        addInfoTableRow($table, "City",
          info.detail.City);
      if (info.detail.StateCode)
        addInfoTableRow($table, "State",
          info.detail.StateCode);
      if (info.detail.Zip)
        addInfoTableRow($table, "Zip",
          info.detail.Zip);

      //util.assignAlternatingClassesInTable($table, true);
      util.assignRotatingClassesToChildren($table, ["even", "odd"]);
      $dialog.dialog("open");
    }

    function getSelectedRecipientCount() {
      return $(".current-list-results tbody .css-checkbox.checked", $viewRecipientsTab)
        .length;
    }

    function getSelectedRecipientResult() {
      if (recipientsResults === null || recipientsResults.length === 0)
        return null;

      // get the selected row
      var $selected = $(".current-list-results tr.selected", $viewRecipientsTab);
      if (!$selected.length) {
        // none selected, get the first row
        $selected = $(".current-list-results tbody tr:first-child", $viewRecipientsTab);
      }

      return recipientsResults.Items[$selected.attr("inx")];
    }

    function getUnselectedRecipientIds() {
      var ids = [];
      $(".current-list-results tbody .css-checkbox", $viewRecipientsTab).each(function() {
        var $this = $(this);
        if (!$this.hasClass("checked")) {
          ids.push(recipientsResults.Items[$this.closest("tr").attr("inx")].Id);
        }
      });
      return ids;
    }

    function onClickListResults(event) {
      var $target = $(event.target);
      var $tr = $target.closest(".current-list-results tr");
      if ($tr.length) {
        if (!$tr.hasClass("head")) {
          if ($target.hasClass("css-checkbox")) {
            $target.toggleClass("checked");
            setRecipientsHeaderCheckbox();
            event.stopPropagation();
          } else {
            if ($tr.hasClass("selected"))
              $tr.removeClass("selected");
            else {
              $("tr.selected", $tr.closest("table")).removeClass("selected");
              $tr.addClass("selected");
            }
            // defer so dblclick gets thru
            setTimeout(setPreviewKeysToSelectedRecipient, 500);
          }
        } else if ($target.hasClass("css-checkbox")) {
          $("tbody .css-checkbox").toggleClass("checked", !$target.hasClass("checked"));
          setRecipientsHeaderCheckbox();
          event.stopPropagation();
        }
      } else
        $target.closest(".toggler").toggleClass("showing");
    }

    function onClickMoreViewRecipientsInfo(event) {
      if (util.isMenuItemDisabled(event)) return;
      var info = getSelectedRecipientResult();
      if (info.detail) {
        displayMoreViewRecipientInfo(info);
        return;
      }

      util.openAjaxDialog("Getting recipient detail...");
      util.ajax({
          url: "/Admin/WebService.asmx/GetMoreRecipientInfo",
          data: {
            electionKey: info.ElectionKey,
            officeKey: info.OfficeKey,
            partyKey: info.PartyKey,
            visitorId: info.VisitorId,
            orgContactId: info.OrgContactId
          },

          success: function(result) {
            info.detail = result.d;
            util.closeAjaxDialog();
            displayMoreViewRecipientInfo(info);
          },

          error: function(result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result,
              "Could not get the recipient detail"));
          }
        }
      );
    }

    function onDblClickListResults(event) {
      var $tr = $(event.target).closest(".current-list-results tr");
      var $target = $(event.target);
      if ($tr.length && !$tr.hasClass("head") && !$target.hasClass("css-checkbox")) {
        $("tr.selected", $tr.closest("table")).removeClass("selected");
        $tr.addClass("selected");
        // Post the results to the preview page and go to it
        activateTab(previewSampleName);
        if (getPreviewSource() != "Selected"
        ) { // id "Selected", already done in single click
          $(".preview-selected-recipient input[type=radio]", $previewSampleTab)
            .prop("checked", true);
          onChangePreviewSampleSource();
        }
      }
      util.clearSelection();
    }

    function onViewRecipientsSubsetOk() {
      var $checkboxes = $(".current-list-results tbody .css-checkbox", $viewRecipientsTab);
      var max = $checkboxes.length - 1;
      var val = parseInt($.trim($(".subset-count", $viewRecipientsSubsetDialog).val()));
      if (isNaN(val) || val < 1 || val > max)
        util.alert("Please enter a number between 1 and " + max);
      else {
        $checkboxes.removeClass("checked");
        for (var inx = 0; inx < val; inx++)
          $($checkboxes[inx]).addClass("checked");
        setRecipientsHeaderCheckbox();
        $viewRecipientsSubsetDialog.dialog("close");
      }
    }

    function onViewRecipientsContextMenuDisplay(event) {
      var $tr = $(event.target).closest(".current-list-results tbody tr",
        $viewRecipientsTab);
      if ($tr.length) {
        $("tr.selected", $tr.closest("tbody")).removeClass("selected");
        $tr.addClass("selected");
        return true;
      }
    }

    function setRecipientsHeaderCheckbox() {
      var $context = $(".current-list-results", $viewRecipientsTab);
      var $all = $("tbody .css-checkbox", $context);
      var $checked = $all.filter(".checked");
      var head =
        '<input type="button" class="select-subset button-2 button-smallest" value="Select Subset" />' +
        '<em>' +
        $checked.length +
        '</em> of <em>' +
        $all.length +
        '</em> email recipients are selected.';
      if (recipientsResults.Duplicates)
        head += ' <em>' +
          recipientsResults.Duplicates +
          '</em> duplicate emails were skipped.';
      $("thead .css-checkbox", $context)
        .toggleClass("checked", $all.length === $checked.length);
      $("p.heading", $context).html(head);
    }

    //
    // Preview Sample Tab
    //

    var previewSampleName = "previewsample";
    var previewSampleTabName = "tab-" + previewSampleName;
    var $previewSampleTab;

    var previewSampleTabsName = "preview-sample-tabs";
    var $previewSampleTabs;

    var previewEditor;
    var emptyList = '<option selected="selected" value="">&lt;none&gt;</option>';
    var previewChains = [];
    var inSetPreviewKeysToSelectedRecipient = false; // prevent reentrance
    var useTheseValuesSelected;

    function initPreviewSample() {
      $previewSampleTab = $$(previewSampleTabName);
      $previewSampleTabs = $$(previewSampleTabsName);

      $(".preview-selected-recipient input", $previewSampleTab).prop("disabled", true);

      $previewSampleTabs.safeBind("tabsactivate", onPreviewTabsActivate);

      $(".state-dropdown", $previewSampleTab).safeBind("change",
        previewSampleStateDropdownChange);
      $(".county-dropdown", $previewSampleTab).safeBind("change",
        previewSampleCountyDropdownChange);
      $(".local-dropdown", $previewSampleTab).safeBind("change",
        previewSampleLocalDropdownChange);
      $(".election-dropdown", $previewSampleTab).safeBind("change",
        previewSampleElectionDropdownChange);
      $(".office-dropdown", $previewSampleTab).safeBind("change",
        previewSampleOfficeDropdownChange);
      $(".candidate-dropdown", $previewSampleTab).safeBind("change",
        previewSampleCandidateDropdownChange);
      $(".party-dropdown", $previewSampleTab).safeBind("change",
        previewSamplePartyDropdownChange);
      $(".party-email-dropdown", $previewSampleTab).safeBind("change",
        previewSamplePartyEmailDropdownChange);
      $(".preview-source input", $previewSampleTab).safeBind("change",
        onChangePreviewSampleSource);
      $(".generate-preview-button", $previewSampleTab).safeBind("click",
        generateEmailPreview);

      previewEditor = ace.edit("preview-html");
      previewEditor.setPrintMarginColumn(-1);
      previewEditor.setTheme("ace/theme/votetemplate");
      previewEditor.setReadOnly(true);
      previewEditor.setShowPrintMargin(false);
      previewEditor.session.setMode("ace/mode/html");
      previewEditor.renderer.setShowGutter(false);
    };

    function addPreviewOptionsToData(data) {
      var visitorId = null;
      var donorId = null;
      var orgContactId = null;
      var sourceCode = null;
      var recipient = getSelectedRecipientResult();
      if (recipient) {
        visitorId = recipient.VisitorId;
        donorId = recipient.DonorId;
        orgContactId = recipient.OrgContactId;
        sourceCode = recipient.SourceCode;
      }
      var $previewOptions = $('.options', $previewSampleTab);
      data.stateCode = $('.state-dropdown', $previewOptions).val();
      data.countyCode = $('.county-dropdown', $previewOptions).val();
      data.localKey = $('.local-dropdown', $previewOptions).val();
      data.contact = $.trim($('.contact-textbox', $previewOptions).val());
      data.email = $.trim($('.email-textbox', $previewOptions).val());
      data.title = $.trim($('.title-textbox', $previewOptions).val());
      data.phone = $.trim($('.phone-textbox', $previewOptions).val());
      data.electionKey = $('.election-dropdown', $previewOptions).val();
      data.officeKey = $('.office-dropdown', $previewOptions).val();
      data.politicianKey = $('.candidate-dropdown', $previewOptions).val() ||
        $('.politician-key-hidden', $previewOptions).val();
      data.partyKey = $('.party-dropdown', $previewOptions).val();
      data.partyEmail = $('.party-email-dropdown', $previewOptions).val();
      data.visitorId = visitorId || 0;
      data.donorId = donorId || 0;
      data.orgContactId = orgContactId || 0;
      data.sourceCode = sourceCode || "";
    }

    function generateEmailPreview() {
      var data = {
        subjectTemplate: subjectEditor.getValue(),
        bodyTemplate: bodyEditor.getValue()
      };
      addPreviewOptionsToData(data);

      if (!$.trim(data.subjectTemplate) && !$.trim(data.bodyTemplate)) {
        util.alert("Both the subject and body templates are empty.");
        return;
      }

      util.openAjaxDialog("Creating preview...");
      util.ajax({
        url: "/Admin/WebService.asmx/SubtituteEmailTemplate",
        data: data,

        success: function(result) {
          // there could be a parse error
          if (result.d.ErrorMessage) {
            setPreviewError(result.d.ErrorMessage);
          } else {
            setPreviewSubject(result.d.Subject);
            setPreviewBody(result.d.Body);
          }
          util.closeAjaxDialog();
        },

        error: function(result) {
          util.alert(util.formatAjaxError(result, "Could not create preview"));
          util.closeAjaxDialog();
        }
      });
    }

    function onChangePreviewSampleSource() {
      var $overlay = $(".options .overlay", $previewSampleTab);
      var selected =
        $(".preview-source input[type=radio]:checked", $previewSampleTab).val() ===
          "Selected";
      if (selected) {
        sizePreviewOverlay();
        setPreviewKeysToSelectedRecipient();
      } else
        useTheseValuesSelected = true;
      $overlay.toggle(selected);
    }

    function onPreviewTabsActivate(event, ui) {
      var newPanelId = ui.newPanel[0].id;
      switch (newPanelId) {
      case "tab-preview-html":
        previewEditor.renderer.updateFull();
        break;
      }
    }

    function onTabActivatePreviewSample() {
      sizePreviewOverlay();
      var selectedButton = $(".preview-source input[value=Selected]", $previewSampleTab);
      if (!selectedButton.prop("disabled") && !selectedButton.prop("checked")) {
        selectedButton.prop("checked", true);
        setPreviewKeysToSelectedRecipient();
      }
    }

    function previewSampleCandidateDropdownChange() {
      if ($(".candidate-dropdown", $previewSampleTab).val() &&
        !$.trim($(".contact-textbox").val())) {
        $(".contact-textbox")
          .val($(".candidate-dropdown option:selected", $previewSampleTab).text());
      }
    }

    function previewSampleCountyDropdownChange() {
      // remove entries from local
      $(".local-dropdown", $previewSampleTab).html(emptyList);
      $(".election-dropdown", $previewSampleTab).html(emptyList);
      $(".office-dropdown", $previewSampleTab).html(emptyList);
      $(".candidate-dropdown", $previewSampleTab).html(emptyList);

      previewChains = [];
      util.pushChain(previewChains, populatePreviewLocalDropdown);
      if ($(".county-dropdown", $previewSampleTab).val())
        util.pushChain(previewChains, populatePreviewElectionDropdown);
      util.chain(previewChains);
    }

    function previewSampleElectionDropdownChange() {
      $(".office-dropdown", $previewSampleTab).html(emptyList);
      $(".candidate-dropdown", $previewSampleTab).html(emptyList);
      if (!$(".state-dropdown", $previewSampleTab).val()) return;
      previewChains = [];
      populatePreviewOfficeDropdown();
    }

    function previewSampleLocalDropdownChange() {
      $(".election-dropdown", $previewSampleTab).html(emptyList);
      $(".office-dropdown", $previewSampleTab).html(emptyList);
      $(".candidate-dropdown", $previewSampleTab).html(emptyList);
      previewChains = [];
      populatePreviewElectionDropdown();
    }

    function previewSampleOfficeDropdownChange() {
      $(".candidate-dropdown", $previewSampleTab).html(emptyList);
      if (!$(".state-dropdown", $previewSampleTab).val()) return;
      previewChains = [];
      populatePreviewCandidateDropdown();
    }

    function previewSamplePartyDropdownChange() {
      $(".party-email-dropdown", $previewSampleTab).html(emptyList);
      if (!$(".party-dropdown", $previewSampleTab).val()) return;
      previewChains = [];
      populatePreviewPartyEmailDropdown();
    }

    function previewSamplePartyEmailDropdownChange() {
      if ($(".party-email-dropdown", $previewSampleTab).val() &&
        !$.trim($(".email-textbox").val())) {
        var nameAndEmail =
          $(".party-email-dropdown option:selected", $previewSampleTab).text();
        var emailPos = nameAndEmail.indexOf("<");
        if (emailPos > 0) {
          $(".contact-textbox").val(nameAndEmail.substr(0, emailPos - 1));
          $(".email-textbox").val(nameAndEmail.substr(emailPos + 1,
            nameAndEmail.length - emailPos - 2));
        }
      }
    }

    function previewSampleStateDropdownChange() {
      $(".county-dropdown", $previewSampleTab).html(emptyList);
      $(".local-dropdown", $previewSampleTab).html(emptyList);
      $(".election-dropdown", $previewSampleTab).html(emptyList);
      $(".office-dropdown", $previewSampleTab).html(emptyList);
      $(".candidate-dropdown", $previewSampleTab).html(emptyList);
      $(".party-dropdown", $previewSampleTab).html(emptyList);
      $(".party-email-dropdown", $previewSampleTab).html(emptyList);
      if (!$(".state-dropdown", $previewSampleTab).val()) return;

      previewChains = [];
      util.pushChain(previewChains, populatePreviewCountyDropdown);
      util.pushChain(previewChains, populatePreviewPartyDropdown);
      util.pushChain(previewChains, populatePreviewElectionDropdown);
      util.chain(previewChains);
    }

    function populatePreviewCandidateDropdown(val) {
      //util.openAjaxDialog("Getting candidates...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetCandidates",
        data: {
          electionKey: $(".election-dropdown", $previewSampleTab).val(),
          officeKey: $(".office-dropdown", $previewSampleTab).val()
        },

        success: function(result) {
          util.populateDropdown($('.candidate-dropdown', $previewSampleTab), result.d,
            null, null, val || '');
          //util.closeAjaxDialog();
          util.chain(previewChains);
        },

        error: function(result) {
          inSetPreviewKeysToSelectedRecipient = false;
          util.alert(util.formatAjaxError(result, "Could not get candidates"));
          //util.closeAjaxDialog();
        }
      });
    }

    function populatePreviewCountyDropdown(val) {
      //util.openAjaxDialog("Getting counties...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetCounties",
        data: {
          stateCode: $(".state-dropdown", $previewSampleTab).val()
        },

        success: function(result) {
          util.populateDropdown($('.county-dropdown', $previewSampleTab), result.d,
            '<none>', '', val || '');
          //util.closeAjaxDialog();
          util.chain(previewChains);
        },

        error: function(result) {
          inSetPreviewKeysToSelectedRecipient = false;
          util.alert(util.formatAjaxError(result, "Could not get counties"));
          //util.closeAjaxDialog();
        }
      });
    }

    function populatePreviewElectionDropdown(val, altVal) {
      //util.openAjaxDialog("Getting elections...");
      val = val || "";
      altVal = altVal || "";
      var countyCode = $(".county-dropdown", $previewSampleTab).val();
      var localKey = $(".local-dropdown", $previewSampleTab).val();
      //
      // This is experimental -- don't use county and local keys 
      // on state electionKey so will work for county and local admins
      //
      if (val.length == 12) {
        countyCode = "";
        localKey = "";
      }

      util.ajax({
        url: "/Admin/WebService.asmx/GetElections",
        data: {
          stateCode: $(".state-dropdown", $previewSampleTab).val(),
          countyCode: countyCode,
          localKey: localKey
        },

        success: function(result) {
          var selected = val;
          var altSelected = altVal;
          if (val.length == 12) // state code but might be for state/local admins
          {
            selected += localKey.length === 5 ? localKey : (countyCode + localKey);
            altSelected += localKey.length === 5 ? localKey : (countyCode + localKey);
          }
          var $electionDropdown = $('.election-dropdown', $previewSampleTab);
          util.populateDropdown($electionDropdown, result.d, null, null, selected);
          if (altSelected &&
            $electionDropdown.val() != val &&
            $("option[value=" + altSelected + "]", $electionDropdown).length)
            $electionDropdown.val(altSelected);
          //util.closeAjaxDialog();
          util.chain(previewChains);
        },

        error: function(result) {
          inSetPreviewKeysToSelectedRecipient = false;
          util.alert(util.formatAjaxError(result, "Could not get elections"));
          //util.closeAjaxDialog();
        }
      });
    }

    function populatePreviewLocalDropdown(val) {
      //util.openAjaxDialog("Getting local districts...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetLocals",
        data: {
          stateCode: $(".state-dropdown", $previewSampleTab).val(),
          countyCode: $(".county-dropdown", $previewSampleTab).val()
        },

        success: function(result) {
          util.populateDropdown($('.local-dropdown', $previewSampleTab), result.d,
            '<none>', '', val || '');
          //util.closeAjaxDialog();
          util.chain(previewChains);
        },

        error: function(result) {
          inSetPreviewKeysToSelectedRecipient = false;
          util.alert(util.formatAjaxError(result, "Could not get local districts"));
          //util.closeAjaxDialog();
        }
      });

    };

    function populatePreviewOfficeDropdown(val) {
      //util.openAjaxDialog("Getting offices...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetOffices",
        data: {
          electionKey: $(".election-dropdown", $previewSampleTab).val()
        },

        success: function(result) {
          util.populateDropdown($('.office-dropdown', $previewSampleTab), result.d,
            null, null, val || '', null, true);
          //util.closeAjaxDialog();
          util.chain(previewChains);
        },

        error: function(result) {
          inSetPreviewKeysToSelectedRecipient = false;
          util.alert(util.formatAjaxError(result, "Could not get offices"));
          //util.closeAjaxDialog();
        }
      });
    }

    function populatePreviewPartyDropdown(val) {
      //util.openAjaxDialog("Getting parties...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetParties",
        data: {
          stateCode: $(".state-dropdown", $previewSampleTab).val()
        },

        success: function(result) {
          util.populateDropdown($('.party-dropdown', $previewSampleTab), result.d,
            null, null, val || '');
          //util.closeAjaxDialog();
          util.chain(previewChains);
        },

        error: function(result) {
          inSetPreviewKeysToSelectedRecipient = false;
          util.alert(util.formatAjaxError(result, "Could not get parties"));
          //util.closeAjaxDialog();
        }
      });
    }

    function populatePreviewPartyEmailDropdown(val) {
      //util.openAjaxDialog("Getting party emails...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetPartyEmails",
        data: {
          partyKey: $(".party-dropdown", $previewSampleTab).val()
        },

        success: function(result) {
          util.populateDropdown($('.party-email-dropdown', $previewSampleTab), result.d,
            null, null, val || '');
          //util.closeAjaxDialog();
          util.chain(previewChains);
        },

        error: function(result) {
          inSetPreviewKeysToSelectedRecipient = false;
          util.alert(util.formatAjaxError(result, "Could not get party emails"));
          //util.closeAjaxDialog();
        }
      });
    }

    function setPreviewBody(val) {
      var $iframeBody = $("#tab-preview-rendered iframe", $previewSampleTab).contents()
        .find('body');
      $iframeBody.html(val);
      util.setOffpageTargets($iframeBody);
      //$('a', $iframeBody).attr("target", "_blank");
      $(".error", $previewSampleTab).hide();
      previewEditor.setValue(val);
      previewEditor.getSelection().selectFileStart();
      previewEditor.session.setUndoManager(new ace.UndoManager());
      $("#preview-html", $previewSampleTab).show();
    }

    function setPreviewError(message) {
      var html = '<p class="error-head">There was a problem parsing your template:</p>' +
        '<p class="error-message">' +
        util.htmlEscape(message) +
        '</p>';
      $(".error", $previewSampleTab).html(html).show();
      $previewSampleTabs.tabs("option", "active",
        util.getTabIndex(previewSampleTabsName, "tab-preview-html"));
      $(".sample-subject", $previewSampleTab).safeHtml("<template parsing error>");
      $("#tab-preview-rendered iframe", $previewSampleTab).contents().find('body')
        .html("&nbsp;");
      previewEditor.setValue("");
      $("#preview-html", $previewSampleTab).hide();
    }

    function getPreviewSource() {
      return $(".preview-source input[type=radio]:checked", $previewSampleTab).val();
    }

    function setPreviewKeysToSelectedRecipient() {
      if (!inSetPreviewKeysToSelectedRecipient && getPreviewSource() === "Selected") {
        inSetPreviewKeysToSelectedRecipient = true;
        var selected = getSelectedRecipientResult();
        if (selected) {
          $(".contact-textbox", $previewSampleTab).val(selected.Contact)
            .attr('title', selected.Contact);
          $(".email-textbox", $previewSampleTab).val(selected.Email)
            .attr('title', selected.Email);
          $(".title-textbox", $previewSampleTab).val(selected.Title)
            .attr('title', selected.Title);
          $(".phone-textbox", $previewSampleTab).val(selected.Phone)
            .attr('title', selected.Phone);

          previewChains = [];

          var stateChanged = false;
          var countyChanged = false;
          var localChanged = false;
          var electionChanged = false;
          var officeChanged = false;
          var candidateChanged = false;
          var partyChanged = false;
          var partyEmailChanged = false;

          var $state = $(".state-dropdown", $previewSampleTab);
          var $county = $(".county-dropdown", $previewSampleTab);
          var $local = $(".local-dropdown", $previewSampleTab);
          var $election = $(".election-dropdown", $previewSampleTab);
          var $office = $(".office-dropdown", $previewSampleTab);
          var $candidate = $(".candidate-dropdown", $previewSampleTab);
          var $politicianKey = $(".politician-key-hidden", $previewSampleTab);
          var $party = $(".party-dropdown", $previewSampleTab);
          var $partyEmail = $(".party-email-dropdown", $previewSampleTab);

          stateChanged = $state.val() !== (selected.StateCode || "");
          if (stateChanged) {
            $(".county-dropdown", $previewSampleTab).html(emptyList);
            $(".party-dropdown", $previewSampleTab).html(emptyList);
            $state.val(selected.StateCode);
            util.pushChain(previewChains, populatePreviewPartyDropdown,
              selected.PartyKey);
            util.pushChain(previewChains, populatePreviewCountyDropdown,
              selected.CountyCode);
          }

          countyChanged = stateChanged || $county.val() !== (selected.CountyCode || "");
          if (countyChanged) {
            $(".local-dropdown", $previewSampleTab).html(emptyList);
            if (!stateChanged)
              $county.val(selected.CountyCode);
            if (selected.CountyCode)
              util.pushChain(previewChains, populatePreviewLocalDropdown,
                selected.LocalKey);
          }

          localChanged = countyChanged || $local.val() !== (selected.LocalKey || "");
          if (localChanged) {
            if (!countyChanged)
              $local.val(selected.LocalKey);
          }

          var jurisdictionChanged = stateChanged || countyChanged || localChanged;
          if (jurisdictionChanged) {
            $(".election-dropdown", $previewSampleTab).html(emptyList);
            util.pushChain(previewChains, populatePreviewElectionDropdown,
              selected.ElectionKey, selected.ElectionKeyToInclude);
          }

          electionChanged = jurisdictionChanged ||
            $election.val() !== (selected.ElectionKey || "");
          if (electionChanged) {
            $(".office-dropdown", $previewSampleTab).html(emptyList);
            if (!jurisdictionChanged)
              $election.val(selected.ElectionKey);
            if (selected.ElectionKey)
              util.pushChain(previewChains, populatePreviewOfficeDropdown,
                selected.OfficeKey);
          }

          partyChanged = stateChanged || $party.val() !== (selected.PartyKey || "");
          if (partyChanged) {
            $(".party-email-dropdown", $previewSampleTab).html(emptyList);
            if (!stateChanged)
              $party.val(selected.PartyKey);
            if (selected.PartyKey)
              util.pushChain(previewChains, populatePreviewPartyEmailDropdown,
                selected.PartyEmail);
          }

          partyEmailChanged =
            partyChanged || $partyEmail.val() !== (selected.PartyEmail || "");
          if (partyEmailChanged) {
            if (!partyChanged)
              $partyEmail.val(selected.PartyEmail);
          }

          officeChanged = electionChanged || $office.val() !== (selected.OfficeKey || "");
          if (officeChanged) {
            $(".candidate-dropdown", $previewSampleTab).html(emptyList);
            if (!electionChanged)
              $office.val(selected.OfficeKey);
            if (selected.OfficeKey)
              util.pushChain(previewChains, populatePreviewCandidateDropdown,
                selected.PoliticianKey);
          }

          candidateChanged =
            officeChanged || $candidate.val() !== (selected.PoliticianKey || "");
          if (candidateChanged) {
            if (!officeChanged)
              $candidate.val(selected.PoliticianKey);
          }
          $politicianKey.val(selected.PoliticianKey);

          partyChanged = stateChanged || $party.val() !== (selected.PartyKey || "");
          if (partyChanged) {
            $(".party-email-dropdown", $previewSampleTab).html(emptyList);
            if (!stateChanged)
              $party.val(selected.PartyKey);
            if (selected.PartyKey)
              util.pushChain(previewChains, populatePreviewPartyEmailDropdown,
                selected.PartyEmail);
          }

          partyEmailChanged =
            partyChanged || $partyEmail.val() !== (selected.PartyEmail || "");
          if (partyEmailChanged) {
            if (!partyChanged)
              $partyEmail.val(selected.PartyEmail);
          }

          // finally, reenable
          util.pushChain(previewChains, function() {
            inSetPreviewKeysToSelectedRecipient = false;
          });

          util.chain(previewChains);
        }
      }
    }

    function setPreviewSubject(val) {
      val = util.htmlEscape($.trim(val) || "<none>");
      $(".sample-subject", $previewSampleTab).html(val).attr("title", val);
    }

    function sizePreviewOverlay() {
      var $options = $(".options", $previewSampleTab);
      var $overlay = $(".options .overlay", $previewSampleTab);
      $overlay.height($options.height() + 3).width($options.width() + 8);
    }

    //
    // Email Options Tab
    //

    var emailOptionsName = "emailoptions";
    var emailOptionsTabName = "tab-" + emailOptionsName;
    var $emailOptionsTab;

    function initEmailOptions() {
      $emailOptionsTab = $$(emailOptionsTabName);
      $(".email-add-button").safeBind("click", onClickEmailAddButton);
    }

    function formatEmailOptionsAsJson() {
      return JSON.stringify({
        from: getSelectedEmailsFromList($(".bulk-email-options-from")),
        cc: getSelectedEmailsFromList($(".bulk-email-options-cc")),
        bcc: getSelectedEmailsFromList($(".bulk-email-options-bcc"))
      });
    }

    function getSelectedEmailsFromList($emailList) {
      var list = [];
      $('input:checked', $emailList).each(function() { list.push($(this).val()); });
      return list;
    }

    function onClickEmailAddButton(event) {
      var $target = $(event.target);
      var $list = $target.closest(".email-list");
      var $scroller = $(".scrolling-list", $list);
      var isRadio = $list.hasClass("radio");
      var name = util.getServerIdFromClientId($scroller[0].id);
      var $email = $("input[type=text]", $list);
      var email = $.trim($email.val());
      if (!email) return;
      if (!util.isValidEmail(email)) {
        $email.select();
        util.alert(email + " is not a valid email address",
          function() { $email.focus(); });
        return;
      }
      if ($scroller.hasClass("bulk-email-options-from") &&
        !(/@vote-usa\.org$/i.test(email))) {
        $email.select();
        util.alert("All From email addresses must be from the vote-usa.org domain",
          function() { $email.focus(); });
        return;
      }
      var labels = $("label", $scroller);
      var dup = false;
      var lctext = email.toLowerCase();
      $.each(labels, function() {
        if ($(this).text().toLowerCase() === lctext) {
          dup = true;
          return false;
        }
      });
      if (dup) {
        $email.select();
        util.alert(email + " is a duplicate email address",
          function() { $email.focus(); });
        return;
      }
      var currentCount = labels.length;

      var type = isRadio ? "radio" : "checkbox";
      var id = name + (currentCount + 1);
      var html = '<div class="tiptip" title="' + email + '">';
      html += '<input type="' +
        type +
        '" checked="checked" id="' +
        id +
        '" value="' +
        email +
        '"';
      if (isRadio) {
        name = $("input[type=radio]", $scroller).attr("name") || name;
        html += ' name="' + name + '"';
      }
      var isOption = $target.hasClass("is-option");
      if (isOption)
        html += ' class="is-option-click"';
      html += '/><label for="' + id + '">' + email + '</label>';
      html += '</div>';
      $scroller.append(html);
      $email.val("");
      util.initTipTip($(".scrolling-list", $list).children().last());
      if (isOption) onChangeOptions();
    }

    function restoreEmailOption(className, email) {
      var $scroller = $(".scrolling-list." + className, $emailOptionsTab);
      var $list = $scroller.parent().closest(".email-list");
      var isRadio = $list.hasClass("radio");
      var name = util.getServerIdFromClientId($scroller[0].id);

      var labels = $("label", $scroller);
      var found = false;
      var lctext = email.toLowerCase();
      $.each(labels, function() {
        var $this = $(this);
        if ($this.text().toLowerCase() === lctext) {
          found = true;
          $this.prev().prop("checked", true);
          return false;
        }
      });
      if (found) return;

      var currentCount = labels.length;
      var type = isRadio ? "radio" : "checkbox";
      var id = name + (currentCount + 1);
      var html = '<div class="tiptip" title="' + email + '">';
      html += '<input type="' +
        type +
        '" checked="checked" id="' +
        id +
        '" value="' +
        email +
        '"';
      if (isRadio) {
        name = $("input[type=radio]", $scroller).attr("name") || name;
        html += ' name="' + name + '"';
      }
      html += ' class="is-option-click"';
      html += '/><label for="' + id + '">' + email + '</label>';
      html += '</div>';
      $scroller.append(html);
      util.initTipTip($(".scrolling-list", $list).children().last());
    }

    function restoreEmailOptions(o) {
      $(".scrolling-list.email-list input", $emailOptionsTab).prop("checked", false);
      $.each(o.from, function() { restoreEmailOption("bulk-email-options-from", this); });
      $.each(o.cc, function() { restoreEmailOption("bulk-email-options-cc", this); });
      $.each(o.bcc, function() { restoreEmailOption("bulk-email-options-bcc", this); });
    }

    //
    // Send Emails Tab
    //

    var sendEmailsName = "sendemails";
    var sendEmailsTabName = "tab-" + sendEmailsName;
    var $sendEmailsTab;

    var $emailBatchFailureDialog;

    var pendingEmailSendBatchId = 0;
    var postEmailSendStatusInterval = 500;
    var maxBatchDescriptionLength = 500;

    function initSendEmails() {
      $sendEmailsTab = $$(sendEmailsTabName);
      $emailBatchFailureDialog = $("#email-batch-failures");
      $(".send-test-email-button", $sendEmailsTab)
        .safeBind("click", onClickSendTestEmailButton);
      $(".send-all-emails-button", $sendEmailsTab)
        .safeBind("click", onClickSendAllEmailsButton);
      // handles both the tab entry and the context popup
      util.registerDataMonitor($(".batch-change-description textarea"),
        { onChange: onChangeLoggingDescription });

      $emailBatchFailureDialog.dialog($.extend({
          autoOpen: false,
          dialogClass: 'email-batch-failures logged-email-resizeable',
          modal: true,
          title: "Email Batch Failure Log",
          width: 650,
          height: 250,
          minWidth: 350,
          minHeight: 250
        },
        util.dialogOpenAndResizeableOptions()));
    }

    function getBodyTemplate(data) {
      data.bodyTemplate = bodyEditor.getValue();
      if (!$.trim(data.bodyTemplate)) {
        util.alert('The template for the email body is empty.',
          function() {
            bodyEditor.focus();
          });
        activateTab(editTemplateName);
        return false;
      }
      return true;
    }

    function getEmailBatchId(data) {
      if (!recipientsResults || !getSelectedRecipientCount()) {
        util.alert('No email recipients have been selected.');
        activateTab(selectRecipientsName);
        return false;
      }
      data.batchId = recipientsResults.BatchId;
      data.skip = getUnselectedRecipientIds();
      return true;
    }

    function getEmailOptions(data, test) {
      var from = getSelectedEmailsFromList($(".bulk-email-options-from"));
      if (!from.length) {
        util.alert('There is no "From" address selected.');
        activateTab(emailOptionsName);
        return false;
      }
      data.from = from[0];
      if (!test) {
        data.cc = getSelectedEmailsFromList($(".bulk-email-options-cc"));
        data.bcc = getSelectedEmailsFromList($(".bulk-email-options-bcc"));
      }
      return true;
    }

    function getTestEmail(data, restartFn, restartAt) {
      data.to = getSelectedEmailsFromList($(".bulk-email-options-test-to"));

      if (!data.to.length) {
        util.confirm('There are no "Send Test To" addresses selected.' +
          ' Do you want to run the test with no emails sent?',
          function(button) {
            if (button === "Ok") {
              restartFn(data, restartAt);
            }
          });
        return false;
      }
      return true;
    }

    function getPreviewOptions(data, restartFn, restartAt) {
      addPreviewOptionsToData(data);

      var missing = [];
      if (!data.contact) missing.push("no Contact Name");
      if (!data.email) missing.push("no Contact Email");

      if (missing.length > 0) {
        util.confirm("There is " +
          missing.join(" and ") +
          " in the Preview Sample. Do you want to continue?",
          function(button) {
            if (button === "Ok") {
              restartFn(data, restartAt);
            } else {
              activateTab(previewSampleName);
            }
          });
        return false;
      }
      return true;
    }

    function getSubjectTemplate(data, restartFn, restartAt) {
      data.subjectTemplate = subjectEditor.getValue();

      if (!$.trim(data.subjectTemplate)) {
        util.confirm('The template for the email subject is empty.' +
          ' Do you want to continue?',
          function(button) {
            if (button === "Ok") {
              restartFn(data, restartAt);
            } else {
              activateTab(editTemplateName);
              subjectEditor.focus();
            }
          });
        return false;
      }

      return true;
    }

    function onChangeLoggingDescription($input) {
      // this handles both the tab value and the batch dialog
      var val = $input.val();
      var $context = $input.closest(".batch-change-description");
      var $label = $(".mainlabel", $context);
      var label = $label.html();
      var match = label.match(/ \([^\)]+\)$/);
      if (match)
        label = label.substr(0, match.index);
      if (val.length) {
        var len = val.length.toString();
        if (val.length > maxBatchDescriptionLength)
          len = "<em>" + len + "</em>";
        label += " (" + len + " of " + maxBatchDescriptionLength + ")";
      }
      $label.html(label);
      if ($context.hasClass("in-dialog"))
        onChangeBatchDescriptionDialogChanged($context, val);
    }

    function onClickSendAllEmailsButton() {
      var toSend = recipientsResults.Items.length - getUnselectedRecipientIds().length;
      if (toSend === 0) {
        util.alert("There are no checked entries in the recipient list.");
        return;
      }
      util.confirm("You are about to send " +
        toSend +
        " email" +
        util.plural(toSend) +
        " to the actual recipient" +
        util.plural(toSend) +
        ". Do you want to continue?",
        function(button) {
          if (button === "Ok") {
            sendAllEmails();
          }
        });
    }

    function onClickSendTestEmailButton() {
      switch ($(".test-email input[type=radio]:checked", $sendEmailsTab).val()) {
      case "Single":
        sendSingleTestEmail();
        break;

      case "All":
        sendAllTestEmails();
        break;
      }
    }

    function onTabActivateSendEmails() {
      // copy the template name to the logging description
      var name = templateName || "";
      name.length = Math.min(name.length, maxBatchDescriptionLength);
      $(".batch-change-description textarea").val(name);
    }

    function postEmailSendStatus() {
      if (!pendingEmailSendBatchId) return;

      util.ajax({
        url: "/Admin/WebService.asmx/GetEmailSendStatus",
        data: {
          batchId: pendingEmailSendBatchId
        },

        success: function(result) {
          var status = result.d;
          var msg;
          if (status.Sent < 0)
            msg = "";
          else if (status.Sent === 0 && status.Failed === 0)
            msg = "Preparing to send emails...";
          else {
            msg = status.Sent + " sent, " + status.Failed + " failed";
          }
          util.setAjaxDialogStatus(msg);
          if (pendingEmailSendBatchId) {
            setTimeout(postEmailSendStatus, postEmailSendStatusInterval);
          }
        },

        error: function() {
          if (pendingEmailSendBatchId) {
            setTimeout(postEmailSendStatus, postEmailSendStatusInterval);
          }
        }

      });
    }

    function sendAllEmails(data, resume) {

      data = data || {};

      switch (resume || "") {
      case "":
        if (!getEmailBatchId(data)) return;
        if (!getBodyTemplate(data)) return;
        if (!getSubjectTemplate(data, sendAllEmails, "afterSubjectTemplate")) return;
      case "afterSubjectTemplate":
        if (!getEmailOptions(data)) return;
        data.emailType = $emailType.val();
      }

      switch (getRecipientType()) {
      case "All":
      case "State":
      case "County":
      case "Local":
        data.contactType = "A";
        break;

      case "AllCandidates":
      case "StateCandidates":
      case "CountyCandidates":
      case "LocalCandidates":
        data.contactType = "C";
        break;

      case "PartyOfficials":
        data.contactType = "P";
        break;

      case "Volunteers":
        data.contactType = "Z";
        break;

      case "WebsiteVisitors":
        data.contactType = "V";
        break;

      case "Donors":
        data.contactType = "D";
        break;

      case "Organizations":
        data.contactType = "O";
        break;
      }

      data.selectionCriteria = formattedSelectionCriteria;
      data.description = $(".all-email-desc textarea", $sendEmailsTab).val();
      if (data.description.length > maxBatchDescriptionLength) {
        util.alert("The logging description is " +
          data.description.length +
          " characters long. The nmaximum is " +
          maxBatchDescriptionLength +
          ".");
        return;
      }
      if ($.trim(data.description).length === 0) {
        util.alert("Please enter a description of the batch for logging.");
        return;
      }
      data.found = recipientsResults.Items.length;

      util.openAjaxDialog("Sending email batch...");
      util.ajax({
        url: "/Admin/WebService.asmx/SendEmailBatch",
        data: data,

        success: sendAllEmailsSuccess,

        error: function(result) {
          pendingEmailSendBatchId = 0;
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "The email batch could not be sent"));
        }
      });

      pendingEmailSendBatchId = recipientsResults.BatchId;
      postEmailSendStatus();
    }

    function sendAllEmailsSuccess(result) {
      pendingEmailSendBatchId = 0;
      util.closeAjaxDialog();
      updateTemplateLastUsedDate();
      var msg = "The email batch was processed:\n" +
        result.d.Sent +
        " sent\n" +
        result.d.Failed +
        " failed";
      if (result.d.Failed) {
        //util.alert(msg, result.d.Sent ? "Partial Success" : "Failure");
        util.alert(msg, ["Ok", "View Failures", 2],
          result.d.Sent ? "Partial Success" : "Failure",
          function(button) {
            if (button === "View Failures") {
              sendAllEmailsViewFailures(result.d);
            }
          });
      } else
        util.alert(msg, "Success");
    }

    function sendAllEmailsViewFailures(summary) {
      $emailBatchFailureDialog.dialog("open");
      $("h4", $emailBatchFailureDialog).safeHtml(summary.Description);
      var lines = [];
      $.each(summary.Failures, function() {
        lines.push('<tr>' +
          '<td data-sort-value="' +
          this.SortContact +
          '">' +
          util.htmlEscape(this.Contact) +
          '</td>' +
          '<td data-sort-value="' +
          util.emailForSort(this.ToAddresses) +
          '">' +
          util.htmlEscape(this.ToAddresses) +
          '</td>' +
          '<td>' +
          util.htmlEscape(this.Message) +
          '</td>' +
          '</tr>');
      });
      $("tbody", $emailBatchFailureDialog).html(lines.join(""));

      var $table = $('table', $emailBatchFailureDialog);
      util.assignRotatingClassesToChildren($table, ["odd", "even"]);

      // attach event handlers
      $table.resizableColumns({ store: store });
      var table = $table.stupidtable();
      table.on("beforetablesort", function() {
        $table.addClass("disabled");
      });
      table.on("aftertablesort", function(event, data) {
        $table.removeClass("disabled");
        $table.find("th div.sort-ind").removeClass("asc desc");
        var dir = data.direction === $.fn.stupidtable.dir.ASC ? "asc" : "desc";
        $("div.sort-ind", $table.find("th").eq(data.column)).addClass(dir);
        util.assignRotatingClassesToChildren($table, ["odd", "even"]);
      });
    }

    function sendAllTestEmails(data, resume) {
      data = data || {};

      switch (resume || "") {
      case "":
        if (!getEmailBatchId(data)) return;
        if (!getBodyTemplate(data)) return;
        if (!getSubjectTemplate(data, sendAllTestEmails, "afterSubjectTemplate")) return;
      case "afterSubjectTemplate":
        if (!getEmailOptions(data, true)) return;
        if (!getTestEmail(data, sendAllTestEmails, "afterTestEmail")) return;
      case "afterTestEmail":
      }

      util.openAjaxDialog("Sending test email batch...");
      util.ajax({
        url: "/Admin/WebService.asmx/SendTestEmailBatch",
        data: data,

        success: sendAllEmailsSuccess,

        error: function(result) {
          pendingEmailSendBatchId = 0;
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "The test email batch could not be sent"));
        }
      });

      pendingEmailSendBatchId = recipientsResults.BatchId;
      postEmailSendStatus();
    }

    function sendSingleTestEmail(data, resume) {

      data = data || {};

      switch (resume || "") {
      case "":
        if (!getBodyTemplate(data)) return;
        if (!getSubjectTemplate(data, sendSingleTestEmail, "afterSubjectTemplate")) return;
      case "afterSubjectTemplate":
        if (!getPreviewOptions(data, sendSingleTestEmail, "afterPreviewOptions")) return;
      case "afterPreviewOptions":
        if (!getEmailOptions(data, true)) return;
        if (!getTestEmail(data, sendSingleTestEmail, "afterTestEmail")) return;
      case "afterTestEmail":
      }

      util.openAjaxDialog("Sending test email...");
      util.ajax({
        url: "/Admin/WebService.asmx/SendSingleTestEmail",
        data: data,

        success: function(result) {
          util.closeAjaxDialog();
          // there could be a parse error
          if (result.d)
            util.alert('There was a problem parsing your template:\n' +
              util.htmlEscape(result.d));
          else {
            util.alert('The test email was sent.', 'Success');
          }
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "The test email could not be sent"));
        }
      });
    }

    //
    // Email Logging Tab
    //

    var emailLoggingName = "emaillogging";
    var emailLoggingTabName = "tab-" + emailLoggingName;
    var $emailLoggingTab;

    var emailLoggingTabsName = "email-logging-tabs";
    var $emailLoggingTabs;

    function initEmailLogging() {
      $emailLoggingTab = $$(emailLoggingTabName);
      $emailLoggingTabs = $$(emailLoggingTabsName);

      // this must be done before initEmailLoggingBasicFiltering
      initEmailLoggingMoreFiltering($emailLoggingTab);
      initEmailLoggingBasicFiltering($emailLoggingTab);
      initEmailLoggingPoliticianFiltering($emailLoggingTab);
      initEmailLoggingBatchFiltering($emailLoggingTab);
      initEmailLoggingGetLogEntries($emailLoggingTab);

      onClickResetAllLogSelectionCriteria(true);
    }

    function activateLogTab(tabName) {
      $emailLoggingTabs.tabs("option", "active",
        util.getTabIndex(emailLoggingTabsName, emailLoggingTabName + "-" + tabName));
    };

    function onClickResetAllLogSelectionCriteria(force) {
      if (force !== true) {
        util.confirm("Are you sure you want to reset all email logging criteria?",
          function(button) {
            if (button === "Ok")
              onClickResetAllLogSelectionCriteria(true);
          });
        return;
      }
      resetBasicFilteringLogSelectionCriteria();
      resetElectionOfficeAndCandidateLogSelectionCriteria();
      resetPoliticianLogSelectionCriteria();
      resetBatchLogSelectionCriteria();
      resetGetLogEntriesLogSelectionCriteria();
    }

    function onTabActivateEmailLogging() {
      var $select = $("#tab-emaillogging-batchfiltering select.add-template-name");
      util.ajax({
        url: "/Admin/WebService.asmx/GetEmailTemplateNames",
        success: function(result) {
          $("option:not(:first-child)", $select).remove();
          $.each(result.d, function() {
            $select.append($("<option></option>").text(this.toString()));
          });
        }
      });
    }

    //
    // Email Logging | Basic Filtering Tab
    //

    var basicFilteringName = "basicfiltering";
    var basicFilteringTabName = emailLoggingTabName + "-" + basicFilteringName;
    var $basicFilteringTab;

    var ljc; // loggingJurisdictionsControl

    function initEmailLoggingBasicFiltering() {
      $basicFilteringTab = $$(basicFilteringTabName);
      ljc = new selectJurisdictions.SelectJurisdictions(
        $("." + selectJurisdictions.getControlName(), $basicFilteringTab),
        { onChange: onChangeLoggingJurisdiction });
      ljc.init();

      $(".log-level-buttons input[type=radio]", $basicFilteringTab).safeBind("click",
        function(event) {
          var l = $(event.target).val();
          ljc.toggleLevel(level.counties, l !== level.states);
          ljc.toggleLevel(level.locals, l === level.locals);
          onChangeLoggingJurisdiction();
        });

      $(".date-range input[type=text]", $basicFilteringTab).datepicker({
        changeYear: true,
        yearRange: "2010:+0"
      });

      $("input[type=button].get-emails", $basicFilteringTab)
        .safeBind("click", onClickBasicFilteringGetEmails);

      util.registerDataMonitor($(".start-date", $basicFilteringTab));
      util.registerDataMonitor($(".end-date", $basicFilteringTab));
    }

    function getContactTypes(reportAll) {
      var result = [];
      $(".contact-types input[type=checkbox]:checked", $basicFilteringTab)
        .each(function() {
          result.push($(this).val());
        });
      return reportAll && result.length === 7 ? ["all"] : result;
    }

    function getLogFromOption() {
      return util.trimmedSplit(",", $(".from-addresses", $basicFilteringTab).val());
    }

    function getLogLevel() {
      return $(".level-box input[type=radio]:checked", $basicFilteringTab).val();
    }

    function getLogFlaggedOption() {
      return $(".flagged-box input[type=radio]:checked", $basicFilteringTab).val();
    }

    function getLogSuccessOption() {
      return $(".success-box input[type=radio]:checked", $basicFilteringTab).val();
    }

    function getLogToOption() {
      return util.trimmedSplit(",", $(".to-addresses", $basicFilteringTab).val());
    }

    function getLogUserOption() {
      return util.trimmedSplit(",", $(".user-names", $basicFilteringTab).val());
    }

    function getValidatedLogEndDate() {
      var $input = $(".end-date", $basicFilteringTab);
      var val = $.trim($input.val());
      if (!val) return ""; // empty is ok
      val = util.getUtcDateFromLocalTime(val);
      if (!val) {
        // now empty means an error
        util.alert("The End Date is not valid");
        activateLogTab(basicFilteringName);
        $input.addClass("error");
        return null;
      }
      return val;
    }

    function getValidatedLogStartDate() {
      var $input = $(".start-date", $basicFilteringTab);
      var val = $.trim($input.val());
      if (!val) return ""; // empty is ok
      val = util.getUtcDateFromLocalTime(val);
      if (!val) {
        // now empty means an error
        util.alert("The Start Date is not valid");
        activateLogTab(basicFilteringName);
        $input.addClass("error");
        return null;
      }
      return val;
    }

    function onChangeLoggingJurisdiction() {
      // set the heading on the morefiltering tab
      var singleState = ljc.getSingleStateCode();
      var singleCounty = ljc.getSingleCountyCode();
      var singleLocal = ljc.getSingleLocalKey();
      var heading =
        "Select a single jurisdiction on the <em>Basic Filtering</em> tab to enable this control.";
      var name = "";

      switch (getLogLevel()) {
      case "states":
        if (singleState) {
          name = ljc.getSingleStateName();
          eoc.setJurisdiction(singleState);
        }
        break;

      case "counties":
        if (singleCounty) {
          name = ljc.getSingleCountyName() + ", " + ljc.getSingleStateName();
          eoc.setJurisdiction(singleState, singleCounty);
        }
        break;

      case "locals":
        if (singleLocal) {
          name = ljc.getSingleLocalName() +
            ", " +
            ljc.getSingleCountyName() +
            ", " +
            ljc.getSingleStateName();
          eoc.setJurisdiction(singleState, singleCounty, singleLocal);
        }
        break;
      }
      if (name)
        heading = "<em>Selected Jurisdiction:</em> " + name;
      else
        eoc.setJurisdiction();

      $(".heading", $moreFilteringTab).html(heading);
      $("input[type=button].get-emails", $moreFilteringTab).prop("disabled", !name);
    }

    function onClickBasicFilteringGetEmails() {
      var logStartDate = getValidatedLogStartDate();
      if (logStartDate == null) return;
      var logEndDate = getValidatedLogEndDate();
      if (logEndDate == null) return;
      var data = {
        contactTypes: getContactTypes(true),
        beginDate: logStartDate,
        endDate: logEndDate,
        success: getLogSuccessOption(),
        flagged: getLogFlaggedOption(),
        froms: getLogFromOption(),
        tos: getLogToOption(),
        users: getLogUserOption(),
        jurisdictionLevel: getLogLevel(),
        stateCodes: ljc.getCategoryCodes(level.states, true),
        countyCodes: ljc.getCategoryCodes(level.counties, true),
        localKeysOrCodes: ljc.getCategoryCodes(level.locals, true)
      };

      if ($("#GetBasicEmailFiltering", $basicFilteringTab).prop("checked")) {
        data = $.extend(data, {
          electionKeys: eoc.getCategoryCodes(jlevel.elections, true),
          officeKeys: eoc.getCategoryCodes(jlevel.offices, true),
          candidateKeys: eoc.getCategoryCodes(jlevel.candidates, true)
        });
      }

      doGetLoggedEmails(getLoggedEmailsData(data));
    }

    function resetBasicFilteringLogSelectionCriteria() {
      // set level to states, all states checked
      ljc.reset();
      $(".log-level-buttons input[value=states]", $basicFilteringTab).prop("checked", true);
      // check all contact types
      $(".contact-types input", $basicFilteringTab).prop("checked", true);
      // clear the dates
      $(".date-range input").val("");
      // set success and flagged to all
      $(".log-success-buttons input[value=all]", $basicFilteringTab)
        .prop("checked", true);
      $(".log-flagged-buttons input[value=all]", $basicFilteringTab)
        .prop("checked", true);
      // clear the addresses boxes
      $(".addresses-box input", $basicFilteringTab).val("");
      // uncheck the filtering box
      $("#GetBasicEmailFiltering", $basicFilteringTab).prop("checked", false);
    };

    //
    // Email Logging | More Filtering
    //

    var moreFilteringName = "morefiltering";
    var moreFilteringTabName = emailLoggingTabName + "-" + moreFilteringName;
    var $moreFilteringTab;

    var eoc; // electionsOfficesCandidates

    function initEmailLoggingMoreFiltering() {
      $moreFilteringTab = $$(moreFilteringTabName);
      eoc = new electionsOfficesCandidates.ElectionsOfficesCandidates(
        $("." + electionsOfficesCandidates.getControlName(), $moreFilteringTab));
      eoc.init();

      $("input[type=button].get-emails", $moreFilteringTab)
        .safeBind("click", onClickMoreFilteringGetEmails);
    };

    function onClickMoreFilteringGetEmails() {
      var data = {
        contactTypes: ["all"],
        jurisdictionLevel: getLogLevel(),
        stateCodes: ljc.getCategoryCodes(level.states, true),
        countyCodes: ljc.getCategoryCodes(level.counties, true),
        localKeysOrCodes: ljc.getCategoryCodes(level.locals, true),
        electionKeys: eoc.getCategoryCodes(jlevel.elections, true),
        officeKeys: eoc.getCategoryCodes(jlevel.offices, true),
        candidateKeys: eoc.getCategoryCodes(jlevel.candidates, true)
      };

      if ($("#GetEocEmailFiltering", $moreFilteringTab).prop("checked")) {
        var logStartDate = getValidatedLogStartDate();
        if (logStartDate == null) return;
        var logEndDate = getValidatedLogEndDate();
        if (logEndDate == null) return;
        data = $.extend(data, {
          contactTypes: getContactTypes(true),
          beginDate: logStartDate,
          endDate: logEndDate,
          success: getLogSuccessOption(),
          flagged: getLogFlaggedOption(),
          froms: getLogFromOption(),
          tos: getLogToOption(),
          users: getLogUserOption()
        });
      }

      doGetLoggedEmails(getLoggedEmailsData(data));
    }

    function resetElectionOfficeAndCandidateLogSelectionCriteria() {
      // reset the control
      eoc.toggleLevel("elections", false);
      // uncheck basic filtering
      $("#GetPoliticiansEmailFiltering", $moreFilteringTab).prop("checked", false);
    }

    //
    // Email Logging | Politician Filtering Tab
    //

    var politicianFilteringName = "politicianfiltering";
    var politicianFilteringTabName = emailLoggingTabName + "-" + politicianFilteringName;
    var $politicianFilteringTab;

    var fp; // findPolitician

    function initEmailLoggingPoliticianFiltering() {
      $politicianFilteringTab = $$(politicianFilteringTabName);

      fp = new findPolitician.FindPolitician($("." +
        findPolitician.getControlName(), $politicianFilteringTab), {
        onNewList: onSearchPoliticianSelectionChanged,
        onSelectionChanged: onSearchPoliticianSelectionChanged,
        onDblClickCandidate: onClickAddPoliticianToList,
        getIdsToSkip: getPoliticianKeysToInclude,
        getStateCode: function() {
          var codes = ljc.getCategoryCodes(level.states, true);
          if (codes.length === 1 && codes[0] === "all") codes = [];
          return codes.join(",");
        }
      });
      fp.init();

      $(".blue-arrow", $politicianFilteringTab)
        .safeBind("click", onClickAddPoliticianToList);
      $(".selected-politician-list", $politicianFilteringTab)
        .safeBind("click", onClickSelectedPolitician);
      $(".selected-politician-list", $politicianFilteringTab)
        .safeBind("dblclick", onDblClickSelectedPolitician);
      $(".remove-selected-politicians", $politicianFilteringTab)
        .safeBind("click", onClickRemoveSelectedPolitician);
      $(".get-emails", $politicianFilteringTab)
        .safeBind("click", onClickGetPoliticiansEmails);
    }

    function getLoggedEmailsForPoliticians(politicianKeys) {
      var data = { politicianKeys: politicianKeys };
      if ($("#GetPoliticiansEmailFiltering", $politicianFilteringTab).prop("checked")) {
        var logStartDate = getValidatedLogStartDate();
        if (logStartDate == null) return;
        var logEndDate = getValidatedLogEndDate();
        if (logEndDate == null) return;
        data = $.extend(data, {
          beginDate: logStartDate,
          endDate: logEndDate,
          success: getLogSuccessOption(),
          flagged: getLogFlaggedOption(),
          froms: getLogFromOption(),
          tos: getLogToOption(),
          users: getLogUserOption()
        });
      }

      doGetLoggedEmails(getLoggedEmailsData(data));
    }

    function getPoliticianKeysToInclude(selected) {
      var keys = [];
      $(".selected-politician-list div" + (selected ? ".selected" : ""),
        $politicianFilteringTab).each(function() {
        var id = $(this).attr("id");
        var inx = id.indexOf("-");
        if (inx !== -1)
          keys.push(id.substr(inx + 1));
      });
      return keys;
    }

    function onClickAddPoliticianToList() {
      var politicianKey = fp.getSelectedCandidateKey();
      if (!politicianKey) return;
      var stateCode = politicianKey.substr(0, 2);
      var politicianName = fp.getSelectedCandidateName();
      var politicianSortKey = fp.getSelectedCandidateSortKey();
      if (politicianName && politicianName.substr(politicianName.length - 1, 1) === ")")
        politicianName = politicianName.substr(0, politicianName.length - 1) + "-";
      else
        politicianName = politicianName + " (";
      politicianName += stateCode + ")";
      var $list = $(".selected-politician-list", $emailLoggingTab);
      $list.append('<div class="selected-politician" id="logpolitician-' +
        politicianKey +
        '" sort-key="' +
        politicianSortKey +
        '">' +
        politicianName +
        '</div>');
      util.sortChildrenByAttribute($list, "div", "sort-key");
      fp.removeSelectedCandidates();
      onSearchPoliticianSelectionChanged();
      politiciansToIncludeChanged();
    }

    function onClickGetPoliticiansEmails() {
      getLoggedEmailsForPoliticians(getPoliticianKeysToInclude());
    }

    function onClickRemoveSelectedPolitician() {
      $(".selected-politician.selected", $politicianFilteringTab)
        .remove();
      selectedPoliticianChanged();
      politiciansToIncludeChanged();
      fp.refresh(); // to restore the removed entries
    }

    function onClickSelectedPolitician(event) {
      $(event.target).toggleClass("selected");
      selectedPoliticianChanged();
    }

    function onDblClickSelectedPolitician(event) {
      $(".selected-politician.selected", $politicianFilteringTab)
        .removeClass("selected");
      $(event.target).addClass("selected");
      selectedPoliticianChanged();
      getLoggedEmailsForPoliticians(getPoliticianKeysToInclude(true));
      util.clearSelection();
    }

    function onSearchPoliticianSelectionChanged($control, selectedCandidate) {
      $(".blue-arrow", $politicianFilteringTab).toggleClass("disabled", !selectedCandidate);
    }

    function politiciansToIncludeChanged() {
      $("input[type=button].get-emails", $politicianFilteringTab)
        .prop("disabled", getPoliticianKeysToInclude().length === 0);
    }

    function resetPoliticianLogSelectionCriteria() {
      // reset the control
      fp.reset();
      // clear the selected list
      $(".selected-politician-list", $emailLoggingTab).html("");
      // enable properly
      onSearchPoliticianSelectionChanged();
      politiciansToIncludeChanged();
      selectedPoliticianChanged();
    }

    function selectedPoliticianChanged() {
      $(".remove-selected-politicians", $politicianFilteringTab).prop("disabled",
        $(".selected-politician.selected", $politicianFilteringTab).length === 0);
    }

    //
    // Email Logging | Batch Filtering Tab
    //

    var batchFilteringName = "batchfiltering";
    var batchFilteringTabName = emailLoggingTabName + "-" + batchFilteringName;
    var $batchFilteringTab;

    var logBatchResults = null;

    function initEmailLoggingBatchFiltering() {
      $batchFilteringTab = $$(batchFilteringTabName);

      $(".get-batches", $batchFilteringTab).safeBind("click", onClickGetBatches);
      $(".open-batches", $batchFilteringTab).safeBind("click", onClickOpenBatches);
      $(".batch-list-results", $batchFilteringTab).safeBind("click", onClickBatchResults);
      $(".batch-list-results", $batchFilteringTab)
        .safeBind("dblclick", onDblClickBatchResults);

      util.onContextMenu($("#batch-filtering-context-menu"),
        onBatchContextMenuDisplay);
      $("#batch-filtering-context-menu .edit")
        .safeBind("click", onClickEditBatchDescription);
      $("#batch-filtering-context-menu .info").safeBind("click", onClickMoreBatchInfo);
      $('#batch-list-change-description').dialog({
        autoOpen: false,
        dialogClass: 'batch-list-change-description',
        modal: true,
        resizable: false,
        title: "Change Log Batch Description",
        width: "885px"
      });
      $('#batch-list-change-description .cancel-button').safeBind("click", function() {
        $('#batch-list-change-description').dialog("close");
      });
      $('#batch-list-change-description .change-button')
        .safeBind("click", doChangeBatchDescription);

      $("#tab-emaillogging-batchfiltering select.add-template-name").change(function() {
        if ($(this).val() !== "first") {
          var $strings = $("#tab-emaillogging-batchfiltering .search-strings");
          var strings = $strings.val();
          if (strings) strings += "\n";
          strings += $("option:selected", $(this)).text();
          $strings.val(strings);
        }
        $(this).val("first");
      });

      $('#more-batch-info').dialog({
        autoOpen: false,
        dialogClass: 'more-batch-info',
        modal: true,
        resizable: false,
        title: "More Batch Information",
        width: "500px"
      });

      $('#edit-email-types-dialog').dialog({
        autoOpen: false,
        dialogClass: 'edit-email-types-dialog',
        modal: true,
        resizable: false,
        title: "Edit Email Types",
        width: "auto"
      });

      $(".edit-email-type").on("click", function () {
        var $select = $(".select-email-type-to-edit");
        var $dialog = $("#edit-email-types-dialog");
        var key = $select.val();
        $(".email-type-key").val(key);
        if (key) {
          var desc = $("option:selected", $select).text();
          $(".inner p", $dialog).html("<b>Change Email Type: </b>" + desc);
          $(".email-type-input", $dialog).val(desc);
        } else {
          $(".inner p", $dialog).html("<b>Add New Email Type</b>");
          $(".email-type-input", $dialog).val("");
        }
        $dialog.dialog("open");
      });

      $(".save-email-type").on("click", function() {
        var $dialog = $("#edit-email-types-dialog");
        var emailTypeCode = $(".email-type-key").val();
        var description = $.trim($(".email-type-input", $dialog).val());
        if (!description) {
          util.alert("A description is required");
          return;
        }
        util.ajax({
          url: "/Admin/WebService.asmx/UpdateEmailType",
          data: {
            emailTypeCode: emailTypeCode,
            description: description
          },

          success: function (result) {
            util.populateDropdown($(".edit-template-email-type"), result.d);
            util.populateDropdown($(".email-type-select"), result.d);
            result.d[0].Test = "<add new type>";
            util.populateDropdown($(".select-email-type-to-edit"), result.d);
            $dialog.dialog("close");
          },

          error: function () {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result, "The Email Type could not be updated"));
          }
        });
      });
    }

    function buildLogBatchTable() {
      var lines = [];
      var evenOdd = "odd";

      // one line per batch
      $.each(logBatchResults, function(inx) {
        //var creation = util.getUtcMoment(this.CreationTime);
        var creation = moment(this.CreationTime);
        lines.push('<tr inx="' +
          inx +
          '" class="' +
          evenOdd +
          '">' +
          '<td data-sort-value="' +
          creation.format("X") +
          '">' +
          '<div class="css-checkbox"/>' +
          creation.format("MM/DD/YYYY hh:mm A") +
          '</td>' +
          '<td class="description">' +
          util.htmlEscape(this.Description) +
          '</td>' +
          '<td>' +
          this.Sent +
          '</td>' +
          '<td>' +
          this.Failed +
          '</td></tr>');
        evenOdd = evenOdd === "even" ? "odd" : "even";
      });

      // build the heading, including the info for resizablecolumns and stupidtable
      return '<table data-resizable-columns-id="bulk-email-batches">' +
        '<thead><tr class="head">' +
        '<th data-sort="int" data-resizable-column-id="creation">' +
        '<div class="css-checkbox"/>Creation Time<div class="sort-ind"></div></th>' +
        '<th data-sort="string-ins" data-resizable-column-id="desc">Description' +
        '<div class="sort-ind"></th>' +
        '<th data-sort="int" data-resizable-column-id="sent">' +
        'Sent<div class="sort-ind"></th>' +
        '<th data-sort="int" data-resizable-column-id="failed">' +
        'Failed<div class="sort-ind"></th>' +
        '</tr></thead><tbody>' +
        lines.join("") +
        '</tbody></table>';
    }

    function doChangeBatchDescription() {
      var $context = ("#batch-list-change-description");
      var batchInfo = getSelectedBatchResult();
      if (!batchInfo) return;
      var description = $("textarea", $context).val();
      if (description.length > maxBatchDescriptionLength) {
        alert("The description is " +
          description.length +
          " characters long." +
          " The maximum length is " +
          maxBatchDescriptionLength +
          ".");
        return;
      }
      util.openAjaxDialog("Changing batch description...");
      util.ajax({
        url: "/Admin/WebService.asmx/ChangeLogBatchDescription",
        data: {
          id: batchInfo.Id,
          description: description
        },

        success: function() {
          batchInfo.Description = description;
          $(".description", getSelectedBatchRow()).safeHtml(description);
          util.closeAjaxDialog();
          $('#batch-list-change-description').dialog("close");
        },
        error: function() {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "The description could not be changed"));
        }
      });
    }

    function getCheckedBatchIds() {
      var result = [];
      $("tbody div.css-checkbox.checked", $batchFilteringTab)
        .each(function() {
          result.push(logBatchResults[$(this).closest("tr").attr("inx")].Id);
        });
      return result;
    }

    function getLogBatchesError(result) {
      var msg;
      if (typeof (result) === "string")
        msg = result;
      else
        msg = util.formatAjaxError(result,
          "An error occurred while retrieving the log batches").replace("\n", "<br />");
      $(".batch-list-results", $batchFilteringTab)
        .html("<p>" + msg + "</p>");
      logBatchResults = null;
      util.closeAjaxDialog();
    }

    function getLogBatchesSucceeded(result) {
      util.closeAjaxDialog();

      if (result.d.length === 0) {
        getLogBatchesError("There were no qualifying log batches found.");
        return;
      }

      // save the returned list of log batches
      logBatchResults = result.d;

      // create the listing table
      var $container = $(".batch-list-results", $batchFilteringTab);
      $container.html(buildLogBatchTable());

      $(".batch-list-results th .css-checkbox", $batchFilteringTab)
        .safeBind("click", onClickBatchResults); // need to head off sorting

      // attach event handlers
      var $table = $('table', $container);
      $table.resizableColumns({ store: store });
      var table = $table.stupidtable();
      table.on("beforetablesort", function() {
        $table = $(".batch-list-results table", $batchFilteringTab);
        $table.addClass("disabled");
      });
      table.on("aftertablesort", function(event, data) {
        $table = $(".batch-list-results table", $batchFilteringTab);
        $table.removeClass("disabled");
        $table.find("th div.sort-ind").removeClass("asc desc");
        var dir = data.direction === $.fn.stupidtable.dir.ASC ? "asc" : "desc";
        $("div.sort-ind", $table.find("th").eq(data.column)).addClass(dir);
        // redo alternate coloring class
        var rowClass = "odd";
        $("tbody tr", $table).each(function() {
          $(this).removeClass("odd even").addClass(rowClass);
          rowClass = rowClass === "even" ? "odd" : "even";
        });
      });

      // start everything checked
      $('tbody .css-checkbox', $container).addClass("checked");
      setLogBatchesHeaderCheckbox();
    }

    function getLogBatchSearchStrings() {
      return util.trimmedSplit("\n", $(".search-strings", $batchFilteringTab).val());
    }

    function getLogBatchSearchStringJoinOption() {
      return $(".search-string-col input[type=radio]:checked", $batchFilteringTab).val();
    }

    function getLoggedEmailsForBatches(batchIds) {
      var data = { batchIds: batchIds };
      if ($("#OpenBatchesFiltering", $batchFilteringTab).prop("checked")) {
        data = $.extend(data, {
          success: getLogSuccessOption(),
          flagged: getLogFlaggedOption(),
          tos: getLogToOption()
        });
      }
      doGetLoggedEmails(getLoggedEmailsData(data));
    }

    function getSelectedBatchResult() {
      if (logBatchResults === null || logBatchResults.length === 0)
        return null;

      // get the selected row
      var $selected = getSelectedBatchRow();
      if (!$selected.length) return null;

      return logBatchResults[$selected.attr("inx")];
    }

    function getSelectedBatchRow() {
      return $(".batch-list-results tr.selected", $batchFilteringTab);
    }

    function onBatchContextMenuDisplay(event) {
      var $tr = $(event.target).closest(".batch-list-results tbody tr", $batchFilteringTab);
      if ($tr.length) {
        $("tr.selected", $tr.closest("tbody")).removeClass("selected");
        $tr.addClass("selected");
        return true;
      }
    }

    function onChangeBatchDescriptionDialogChanged($context, val) {
      var batchInfo = getSelectedBatchResult();
      if (!batchInfo) return;
      $(".change-button", $context).attr("disabled", val === batchInfo.Description);
    }

    function onClickBatchResults(event) {
      var $target = $(event.target);
      var $tr = $target.closest(".batch-list-results tr");
      if ($tr.length) {
        if (!$tr.hasClass("head")) {
          if ($target.hasClass("css-checkbox")) {
            $target.toggleClass("checked");
            setLogBatchesHeaderCheckbox();
            event.stopPropagation();
          } else {
            if ($tr.hasClass("selected"))
              $tr.removeClass("selected");
            else {
              $("tr.selected", $tr.closest("table")).removeClass("selected");
              $tr.addClass("selected");
            }
          }
        } else if ($target.hasClass("css-checkbox")) {
          $("tbody .css-checkbox").toggleClass("checked", !$target.hasClass("checked"));
          setLogBatchesHeaderCheckbox();
          event.stopPropagation();
        }
      }
    }

    function onClickEditBatchDescription(event) {
      if (util.isMenuItemDisabled(event)) return;
      var $context = $("#batch-list-change-description");
      var batchInfo = getSelectedBatchResult();
      if (!batchInfo) return;
      $("textarea", $context).val(batchInfo.Description);
      $(".change-button", $context).attr("disabled", true);
      $('#batch-list-change-description').dialog("open");
    }

    function onClickGetBatches() {
      var logStartDate = getValidatedLogStartDate();
      if (logStartDate == null) return;
      var logEndDate = getValidatedLogEndDate();
      if (logEndDate == null) return;
      util.openAjaxDialog("Getting log batch data...");
      util.ajax({
          url: "/Admin/WebService.asmx/GetEmailLogBatches",
          data: {
            beginDate: logStartDate,
            endDate: logEndDate,
            success: getLogSuccessOption(),
            froms: getLogFromOption(),
            users: getLogUserOption(),
            searchStrings: getLogBatchSearchStrings(),
            joinOption: getLogBatchSearchStringJoinOption()
          },

          success: getLogBatchesSucceeded,
          error: getLogBatchesError
        }
      );
    }

    function onClickMoreBatchInfo(event) {
      if (util.isMenuItemDisabled(event)) return;
      var $context = $("#more-batch-info tbody");
      var info = getSelectedBatchResult();
      if (!info) return;
      $(".creation-time .col2", $context).safeHtml(moment(info.CreationTime)
        .format("MM/DD/YYYY hh:mm A"));
      $(".selection-criteria .col2", $context).html(info.SelectionCriteria);
      $(".description .col2", $context).safeHtml(info.Description);
      $(".total-found .col2", $context).html(info.Found);
      $(".skipped-by-user .col2", $context).html(info.Skipped);
      $(".sent .col2", $context).html(info.Sent);
      $(".failed .col2", $context).html(info.Failed);
      $(".user-name .col2", $context).safeHtml(info.UserName);
      $(".from .col2", $context).safeHtml(info.FromEmail);
      var cc = info.CcEmails;
      if (cc.length) cc = cc.join("<br />");
      else cc = util.htmlEscape("<none>");
      $(".cc .col2", $context).html(cc);
      var bcc = info.BccEmails;
      if (bcc.length) bcc = bcc.join("<br />");
      else bcc = util.htmlEscape("<none>");
      $(".bcc .col2", $context).html(bcc);
      $('#more-batch-info').dialog("open");
    }

    function onClickOpenBatches() {
      getLoggedEmailsForBatches(getCheckedBatchIds());
    }

    function onDblClickBatchResults(event) {
      var $tr = $(event.target).closest(".batch-list-results tr");
      var $target = $(event.target);
      if ($tr.length && !$tr.hasClass("head") && !$target.hasClass("css-checkbox")) {
        $("tr.selected", $tr.closest("table")).removeClass("selected");
        $tr.addClass("selected");
        // Just get the selected batch
        getLoggedEmailsForBatches([getSelectedBatchResult().Id]);
      }
      util.clearSelection();
    }

    function resetBatchLogSelectionCriteria() {
      // clear the results
      $(".batch-list-results", $batchFilteringTab).html(
        '<p>No batches have been retrieved yet. Click the <em>Get Batches</em>' +
        ' button to create a list of batches.</p>');
      // disable the open button
      setLogBatchesHeaderCheckbox();
      // uncheck basic filtering
      $("#OpenBatchesFiltering", $batchFilteringTab).prop("checked", false);
      // clear the search strings
      $("textarea.search-strings", $batchFilteringTab).val("");
      // set the join button to OR
      $(".search-string-col input[value=or]", $batchFilteringTab)
        .prop("checked", true);
    }

    function setLogBatchesHeaderCheckbox() {
      var $context = $(".batch-list-results", $batchFilteringTab);
      var $all = $("tbody .css-checkbox", $context);
      var $checked = $all.filter(".checked");
      $("thead .css-checkbox", $context).toggleClass("checked",
        $all.length === $checked.length);
      $("input[type=button].open-batches", $batchFilteringTab)
        .prop("disabled", $checked.length === 0);
    }

    //
    // Email Logging | Get Log Entries Tab
    //

    var getLogEntriesName = "getlogentries";
    var getLogEntriesTabName = emailLoggingTabName + "-" + getLogEntriesName;
    var $getLogEntriesTab;

    var logEmailResults = null;

    var $moreResultsInfoDialog;
    var $batchDescriptionDialog;
    var $viewLoggedEmailDialog;
    var $forwardEmailDialog;
    var $loggedEmailNotesDialog;

    function initEmailLoggingGetLogEntries() {
      $getLogEntriesTab = $$(getLogEntriesTabName);
      $moreResultsInfoDialog = $("#more-results-info");
      $batchDescriptionDialog = $("#batch-description");
      $viewLoggedEmailDialog = $("#view-logged-email");
      $forwardEmailDialog = $("#forward-email");
      $loggedEmailNotesDialog = $("#logged-email-notes");

      util.registerDataMonitor($(".maximum-results", $getLogEntriesTab));
      $(".get-emails", $getLogEntriesTab).safeBind("click", onClickGetLoggedEmails);
      $(".reset-criteria", $getLogEntriesTab)
        .safeBind("click", onClickResetAllLogSelectionCriteria);
      util.onContextMenu($("#results-context-menu"),
        onResultsContextMenuDisplay);
      $(".email-list-results", $getLogEntriesTab).safeBind("click", onClickEmailResults);
      $("#results-context-menu .view").safeBind("click", onClickViewEmail);
      $("#results-context-menu .notes").safeBind("click", onClickEmailNotes);
      $("#results-context-menu .flagged").safeBind("click", onClickFlagged);
      $("#results-context-menu .info").safeBind("click", onClickMoreResultsInfo);
      $("#results-context-menu .batch-description")
        .safeBind("click", onClickBatchDescription);

      $moreResultsInfoDialog.dialog({
        autoOpen: false,
        dialogClass: 'more-results-info',
        modal: true,
        resizable: false,
        title: "More Email Information",
        width: 500
      });

      $batchDescriptionDialog.dialog({
        autoOpen: false,
        dialogClass: 'batch-description',
        modal: true,
        resizable: false,
        title: "Batch Description",
        width: 500
      });

      $viewLoggedEmailDialog.dialog($.extend({
          autoOpen: false,
          dialogClass: 'view-logged-email logged-email-resizeable',
          modal: true,
          title: "Logged Email",
          width: 500,
          minWidth: 350,
          minHeight: 450
        },
        util.dialogOpenAndResizeableOptions()));

      $forwardEmailDialog.dialog($.extend({
          autoOpen: false,
          dialogClass: 'forward-email logged-email-resizeable',
          modal: true,
          title: "Forward Logged Email",
          width: 450,
          height: 450,
          minWidth: 350,
          minHeight: 450
        },
        util.dialogOpenAndResizeableOptions()));

      $loggedEmailNotesDialog.dialog($.extend({
          autoOpen: false,
          dialogClass: 'logged-email-notes logged-email-resizeable',
          modal: true,
          title: "Logged Email Notes",
          minWidth: 350,
          minHeight: 350,
          maxHeight: 700
        },
        util.dialogOpenAndResizeableOptions()));

      $('.forward-button', $viewLoggedEmailDialog).safeBind("click", onClickForwardEmail);
      $('.send-button', $forwardEmailDialog).safeBind("click", onClickSendForwardedEmail);
      $('.add-email-note-button', $loggedEmailNotesDialog)
        .safeBind("click", onClickAddNote);
    }

    function buildLogResultsTable() {
      var lines = [];
      var evenOdd = "odd";

      // one line per batch
      $.each(logEmailResults, function(inx) {
        var sent = moment(this.SentTime);
        var errorClass = this.WasSent ? '' : ' class="error"';
        lines.push('<tr inx="' +
          inx +
          '" class="' +
          evenOdd +
          '">' +
          '<td class="flagged">' +
          (this.IsFlagged ? '√' : '&nbsp;') +
          '</td>' +
          '<td' +
          errorClass +
          ' data-sort-value="' +
          sent.format("X") +
          '">' +
          sent.format("MM/DD/YYYY hh:mm A") +
          '</td>' +
          '<td data-sort-value="' +
          this.SortName +
          '">' +
          this.Contact +
          '</td>' +
          '<td data-sort-value="' +
          util.emailForSort(this.ToEmail) +
          '">' +
          this.ToEmail +
          '</td>' +
          '<td>' +
          util.htmlEscape(this.Subject) +
          '</td>' +
          '<td data-sort-value="' +
          util.jurisdictionForSort(this.Jurisdiction) +
          '">' +
          this.Jurisdiction +
          '</td></tr>');
        evenOdd = evenOdd === "even" ? "odd" : "even";
      });

      // build the heading, including the info for resizablecolumns and stupidtable
      return '<table data-resizable-columns-id="log-email-results">' +
        '<thead><tr class="head">' +
        '<th data-sort="string" data-resizable-column-id="flagged">√' +
        '<div class="sort-ind"></th>' +
        '<th data-sort="int" data-resizable-column-id="sent">' +
        'Sent Time<div class="sort-ind"></div></th>' +
        '<th data-sort="string-ins" data-resizable-column-id="desc">Contact' +
        '<div class="sort-ind"></th>' +
        '<th data-sort="string" data-resizable-column-id="email">' +
        'Email<div class="sort-ind"></th>' +
        '<th data-sort="string-ins" data-resizable-column-id="subject">' +
        'Subject<div class="sort-ind"></th>' +
        '<th data-sort="string" data-resizable-column-id="jurisdiction">' +
        'Jurisdiction<div class="sort-ind"></th>' +
        '</tr></thead><tbody>' +
        lines.join("") +
        '</tbody></table>';
    }

    function displayBatchDescription(info) {
      var $context = $("tbody", $batchDescriptionDialog);
      $(".selection-criteria .col2", $context).html(info.detail.SelectionCriteria);
      $(".description .col2", $context).safeHtml(info.detail.BatchDescription);
      $batchDescriptionDialog.dialog("open");
    }

    function displayCommonHeader($context, info, includeCcs, joinWith) {
      $(".sent .col2", $context).safeHtml(moment(info.SentTime).format("M/D/YYYY h:mm A"));
      $(".from .col2", $context).safeHtml(info.FromEmail);
      var to = info.ToEmail;
      var contact = $.trim(info.Contact);
      if (contact) to = contact + " <" + to + ">";
      $(".to .col2", $context).safeHtml(to);
      if (includeCcs) {
        joinWith = joinWith || "<br />";
        if (info.detail.CcEmails.length)
          $(".cc .col2", $context).html(info.detail.CcEmails.join(joinWith));
        $(".cc", $context).toggle(info.detail.CcEmails.length != 0);
        if (info.detail.BccEmails.length)
          $(".bcc .col2", $context).html(info.detail.BccEmails.join(joinWith));
        $(".bcc", $context).toggle(info.detail.BccEmails.length != 0);
      }
      $(".subject .col2", $context).safeHtml(info.Subject);
    }

    function displayEmailNotes(info) {
      displayCommonHeader($loggedEmailNotesDialog, info);
      updateDialogNotes(info.notes);
      $("textarea", $loggedEmailNotesDialog).val("");
      $loggedEmailNotesDialog.dialog("open");
    };

    function displayForwardedCount(info) {
      var msg = "";
      if (info.ForwardedCount === 1)
        msg = "Forwarded once";
      else if (typeof info.ForwardedCount === "number" && info.ForwardedCount > 1)
        msg = "Forwarded " + info.ForwardedCount + " times";
      $(".forward-button-container p", $viewLoggedEmailDialog).html(msg);
    }

    function displayViewEmail(info) {
      displayCommonHeader($viewLoggedEmailDialog, info, true);
      displayForwardedCount(info);
      var $iframeBody = $("iframe", $viewLoggedEmailDialog).contents().find('body');
      $iframeBody.html(info.detail.Body);
      util.setOffpageTargets($iframeBody);
      $viewLoggedEmailDialog.dialog("open");
    }

    function doGetLoggedEmails(data) {
      var maximumResults = getValidatedMaximumResults(-1);
      if (maximumResults == null) return;

      util.openAjaxDialog("Getting logged emails...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetLoggedEmails",
        data: $.extend({}, data, { maximumResults: maximumResults }),
        success: getLoggedEmailsSucceeded,
        error: getLoggedEmailsError
      });
    }

    function doSendForwardedEmail() {
      var info = getSelectedEmailResult();
      util.openAjaxDialog("Forwarding email...");

      util.ajax({
        url: "/Admin/WebService.asmx/ForwardEmail",
        data: {
          id: info.Id,
          to: $.trim($(".top .to-address", $forwardEmailDialog).val()),
          cc: $.trim($(".top .cc-address", $forwardEmailDialog).val()),
          bcc: $.trim($(".top .bcc-address", $forwardEmailDialog).val()),
          subject: $.trim($(".top .subject", $forwardEmailDialog).val()),
          message: $.trim($(".bottom .message-text", $forwardEmailDialog).val())
        },

        success: function() {
          util.closeAjaxDialog();
          $forwardEmailDialog.dialog("close");
          info.notes = null;
          info.ForwardedCount++;
          displayForwardedCount(info);
          util.alert("The email was forwarded");
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result, "The email could not be forwarded"));
        }

      });
    }

    function getLoggedEmailsData(data) {
      return $.extend({
        contactTypes: [],
        jurisdictionLevel: "",
        stateCodes: [],
        countyCodes: [],
        localKeysOrCodes: [],
        beginDate: "",
        endDate: "",
        success: "",
        flagged: "",
        froms: [],
        tos: [],
        users: [],
        electionKeys: [],
        officeKeys: [],
        candidateKeys: [],
        politicianKeys: [],
        batchIds: []
      }, data);
    }

    function getLoggedEmailDetail(onSuccess) {
      var info = getSelectedEmailResult();
      if (info.detail) {
        onSuccess(info);
        return;
      }

      util.openAjaxDialog("Getting email detail...");
      util.ajax({
          url: "/Admin/WebService.asmx/GetLoggedEmailDetail",
          data: {
            id: info.Id
          },

          success: function(result) {
            info.detail = result.d;
            util.closeAjaxDialog();
            if (typeof onSuccess === "function")
              onSuccess(info);
          },

          error: function(result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result,
              "Could not get the email detail"));
          }
        }
      );
    }

    function getLoggedEmailNotes() {
      var info = getSelectedEmailResult();
      if (info.notes) {
        displayEmailNotes(info);
        return;
      }

      util.openAjaxDialog("Getting email notes...");
      util.ajax({
          url: "/Admin/WebService.asmx/GetEmailNotes",
          data: {
            id: info.Id
          },

          success: function(result) {
            info.notes = result.d;
            util.closeAjaxDialog();
            displayEmailNotes(info);
          },

          error: function(result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result,
              "Could not get the email notes"));
          }
        }
      );
    }

    function getLoggedEmailsError(result) {
      var msg;
      if (typeof (result) === "string")
        msg = result;
      else
        msg = util.formatAjaxError(result,
          "An error occurred while retrieving the logged emails").replace("\n", "<br />");
      $(".email-list-results", $getLogEntriesTab).html("<p>" + msg + "</p>");
      $(".msg", $getLogEntriesTab).html("");
      logEmailResults = null;
      util.closeAjaxDialog();
      activateLogTab(getLogEntriesName);
    }

    function getLoggedEmailsSucceeded(result) {
      var $msg = $(".msg", $getLogEntriesTab);

      util.closeAjaxDialog();

      if (result.d.length === 0) {
        getLoggedEmailsError("There were no qualifying logged emails found.");
        $msg.html("");
        return;
      }

      // save the returned list of log batches
      logEmailResults = result.d;
      $msg.html("<em>" + logEmailResults.length + "</em> email results found.");

      // create the listing table
      var $container = $(".email-list-results", $getLogEntriesTab);
      $container.html(buildLogResultsTable());

      activateLogTab(getLogEntriesName);

      // attach event handlers
      var $table = $('table', $container);
      $table.resizableColumns({ store: store });
      var table = $table.stupidtable();
      table.on("beforetablesort", function() {
        $table = $(".email-list-results table", $getLogEntriesTab);
        $table.addClass("disabled");
      });
      table.on("aftertablesort", function(event, data) {
        $table = $(".email-list-results table", $getLogEntriesTab);
        $table.removeClass("disabled");
        $table.find("th div.sort-ind").removeClass("asc desc");
        var dir = data.direction === $.fn.stupidtable.dir.ASC ? "asc" : "desc";
        $("div.sort-ind", $table.find("th").eq(data.column)).addClass(dir);
        // redo alternate coloring class
        var rowClass = "odd";
        $("tbody tr", $table).each(function() {
          $(this).removeClass("odd even").addClass(rowClass);
          rowClass = rowClass === "even" ? "odd" : "even";
        });
      });
    }

    function getSelectedEmailResult() {
      if (logEmailResults === null || logEmailResults.length === 0)
        return null;

      // get the selected row
      var $selected = getSelectedEmailRow();
      if (!$selected.length) return null;

      return logEmailResults[$selected.attr("inx")];
    }

    function getSelectedEmailRow() {
      return $(".email-list-results tr.selected", $getLogEntriesTab);
    }

    function getValidatedMaximumResults(def) {
      var $input = $(".maximum-results", $getLogEntriesTab);
      var val = $.trim($input.val());
      if (!val && typeof def === "number") return def;
      val = parseInt(val);
      if (isNaN(val)) {
        util.alert("The Maximum Results is not valid");
        activateLogTab(getLogEntriesName);
        $input.addClass("error");
        return null;
      }
      return val;
    }

    function onClickAddNote() {
      var $textarea = $('textarea', $loggedEmailNotesDialog);
      var note = $.trim($textarea.val());
      if (note) {
        var info = getSelectedEmailResult();
        util.openAjaxDialog("Adding note...");
        util.ajax({
            url: "/Admin/WebService.asmx/AddEmailNote",
            data: {
              id: info.Id,
              note: note
            },

            success: function(result) {
              util.closeAjaxDialog();
              info.notes = result.d;
              $textarea.val("");
              updateDialogNotes(info.notes);
            },
            error: function(result) {
              util.closeAjaxDialog();
              util.alert(util.formatAjaxError(result,
                "Could not add the email note"));
            }
          }
        );
      }
    }

    function onClickBatchDescription() {
      getLoggedEmailDetail(displayBatchDescription);
    }

    function onClickEmailNotes(event) {
      if (util.isMenuItemDisabled(event)) return;
      getLoggedEmailNotes();
    }

    function onClickEmailResults(event) {
      var $target = $(event.target);
      var $tr = $target.closest(".email-list-results tr");
      if ($tr.length) {
        if (!$tr.hasClass("head")) {
          if ($tr.hasClass("selected"))
            $tr.removeClass("selected");
          else {
            $("tr.selected", $tr.closest("table")).removeClass("selected");
            $tr.addClass("selected");
          }
        }
      }
    }

    function onClickFlagged(event) {
      if (util.isMenuItemDisabled(event)) return;
      var item = getSelectedEmailResult();
      if (item) {
        var data =
        {
          id: item.Id,
          isFlagged: !item.IsFlagged
        };
        util.openAjaxDialog(data.isFlagged ? "Setting flag..." : "Clearing flag...");
        util.ajax({
          url: "/Admin/WebService.asmx/SetLoggedEmailFlag",
          data: data,

          success: function() {
            var $tr = getSelectedEmailRow();
            item.IsFlagged = !item.IsFlagged;
            $("td.flagged", $tr).html((item.IsFlagged ? '√' : '&nbsp;'));
            util.closeAjaxDialog();
          },

          error: function(result) {
            var message = 'Could not change "' +
              util.htmlEscape(i.Name) +
              '" to ' +
              (item.isPublic ? 'Private' : 'Public');
            util.alert(util.formatAjaxError(result, message));
            util.closeAjaxDialog();
          }
        });
      }
    }

    function onClickForwardEmail() {
      var info = getSelectedEmailResult();
      displayCommonHeader($forwardEmailDialog, info, true, ",");
      $(".to-address", $forwardEmailDialog).val(util.htmlEscape(info.ToEmail));
      var subject = info.Subject;
      if (subject.substr(0, 3).toLowerCase !== "re")
        subject = "re: " + subject;
      $(".subject", $forwardEmailDialog).val(util.htmlEscape(subject));
      $(".cc-address", $forwardEmailDialog).val("");
      $(".bcc-address", $forwardEmailDialog).val("");
      $(".message-text", $forwardEmailDialog).val("");
      $forwardEmailDialog.dialog("open");
    }

    function onClickGetLoggedEmails() {
      var logStartDate = getValidatedLogStartDate();
      if (logStartDate == null) return;
      var logEndDate = getValidatedLogEndDate();
      if (logEndDate == null) return;

      var data = {
        contactTypes: getContactTypes(true),
        jurisdictionLevel: getLogLevel(),
        stateCodes: ljc.getCategoryCodes(level.states, true),
        countyCodes: ljc.getCategoryCodes(level.counties, true),
        localKeysOrCodes: ljc.getCategoryCodes(level.locals, true),
        beginDate: logStartDate,
        endDate: logEndDate,
        success: getLogSuccessOption(),
        flagged: getLogFlaggedOption(),
        froms: getLogFromOption(),
        tos: getLogToOption(),
        users: getLogUserOption(),
        electionKeys: eoc.getCategoryCodes(jlevel.elections, true),
        officeKeys: eoc.getCategoryCodes(jlevel.offices, true),
        candidateKeys: eoc.getCategoryCodes(jlevel.candidates, true),
        politicianKeys: getPoliticianKeysToInclude(),
        batchIds: getCheckedBatchIds()
      };

      doGetLoggedEmails(data);
    }

    function onClickMoreResultsInfo(event) {
      if (util.isMenuItemDisabled(event)) return;
      var $context = $("tbody", $moreResultsInfoDialog);
      var info = getSelectedEmailResult();
      if (!info) return;
      $(".sent-time .col2", $context).safeHtml(moment(info.SentTime)
        .format("MM/DD/YYYY hh:mm A"));
      $(".contact .col2", $context).safeHtml(info.Contact);
      $(".to-email .col2", $context).safeHtml(info.ToEmail);
      $(".from-email .col2", $context).safeHtml(info.FromEmail);
      $(".subject .col2", $context).safeHtml(info.Subject);
      $(".successfully-sent .col2", $context).html(info.WasSent ? "yes" : "no");
      var contactType = "";
      switch (info.ContactType) {
      case "A":
        contactType = "Election Administrator";
        break;

      case "C":
        contactType = "Candidate";
        break;

      case "P":
        contactType = "Party Official";
        break;

      case "Z":
        contactType = "Volunteer";
        break;

      case "V":
        contactType = "Website Visitor";
        break;

      case "D":
        contactType = "Donor";
        break;

      case "O":
        contactType = "Organization";
        break;
      };
      $(".contact-type .col2", $context).safeHtml(contactType);
      $(".jurisdiction .col2", $context).safeHtml(info.Jurisdiction);
      $(".user-name .col2", $context).safeHtml(info.UserName);
      $(".office .col2", $context).safeHtml(info.OfficeName);
      $(".election .col2", $context).safeHtml(info.ElectionDescription);
      var politician = info.PoliticianName;
      if (politician && info.PartyCode)
        politician += " (" + info.PartyCode + ")";
      $(".politician .col2", $context).safeHtml(politician);
      $(".forwarded-count .col2", $context).html(info.ForwardedCount.toString());
      $(".is-flagged .col2", $context).html(info.IsFlagged ? "yes" : "no");
      if (info.ErrorMessage)
        $(".error-message .col2", $context).removeClass("hidden")
          .safeHtml(info.ErrorMessage);
      else {
        $(".error-message").addClass("hidden");
      }
      util.assignRotatingClassesToChildren($context, ["even", "odd"]);
      $moreResultsInfoDialog.dialog("open");
    }

    function onClickSendForwardedEmail() {
      var toEmail = $.trim($(".top .to-address", $forwardEmailDialog).val());
      var subject = $.trim($(".top .subject", $forwardEmailDialog).val());
      var message = $.trim($(".bottom .message-text", $forwardEmailDialog).val());
      if (!toEmail) {
        util.alert("The To email address is required");
        return;
      }
      if (!util.isValidEmail(toEmail)) {
        util.alert("The To email address is not valid");
        return;
      }
      if (!subject) {
        util.alert("The Subject is required");
        return;
      }
      if (!message) {
        util.confirm(
          "Are you sure you want to forward the email with no forwarding message?",
          function(button) {
            if (button === "Ok") {
              doSendForwardedEmail();
            }
          });
        return;
      }
      doSendForwardedEmail();
    }

    function onClickViewEmail(event) {
      if (util.isMenuItemDisabled(event)) return;
      getLoggedEmailDetail(displayViewEmail);
    }

    function onResultsContextMenuDisplay(event, $menu) {
      var $tr = $(event.target).closest(".email-list-results tbody tr", $getLogEntriesTab);
      if ($tr.length) {
        $("tr.selected", $tr.closest("tbody")).removeClass("selected");
        $tr.addClass("selected");
        var item = getSelectedEmailResult();
        $(".flagged .icon", $menu).toggleClass("checked", item.IsFlagged);
        $(".view", $menu).toggleClass("disabled", !item.WasSent);
        return true;
      }
    }

    function onMainTabsChange(event) {
      if ($(event.target).hasClass("is-option-change"))
        onChangeOptions();
    }

    function onMainTabsClick(event) {
      if ($(event.target).hasClass("is-option-click"))
        onChangeOptions();
    }

    function resetGetLogEntriesLogSelectionCriteria() {
      $(".maximum-results", $getLogEntriesTab).val(1000);
    }

    function updateDialogNotes(notes) {
      var builder = [];
      $.each(notes, function() {
        builder.push('<div' +
          (this.IsSystemNote ? ' class="system"' : '') +
          '><p class="date">' +
          moment(this.DateStamp).format("MM/DD/YYYY hh:mm:ss A") +
          '</p>');
        builder.push(util.replaceLineBreaksWithParagraphs(this.Note, true) + '</div>');
      });
      var $bottom = $('.bottom', $loggedEmailNotesDialog);
      $bottom.html(builder.join(""));
      util.assignRotatingClassesToChildren($bottom, ["odd", "even"]);
    }

    //
    // Master Only Tab
    //

    var masterOnlyName = "masteronly";
    var masterOnlyTabName = "tab-" + masterOnlyName;
    var $masterOnlyTab;

    function initMasterOnly() {
      $masterOnlyTab = $$(masterOnlyTabName);

      $(".remove-malformed-emails", $masterOnlyTab).click(function() {
        util.openAjaxDialog("Removing malformed emails from visitors...");
        util.ajax({
          url: "/Admin/WebService.asmx/VisitorRemoveMalformedEmailAddresses",
          data: {
          },

          success: function(result) {
            getAddressesDates();
            util.alert(result.d);
          },

          error: function(result) {
            util.alert(util.formatAjaxError(result,
              "The operation could not be completed"));
          },

          complete: function() {
            util.closeAjaxDialog();
          }
        });
      });

      //$(".transfer-from-address-log", $masterOnlyTab).click(function () {
      //  util.openAjaxDialog("Transferring from address log to visitors...");
      //  util.ajax({
      //    url: "/Admin/WebService.asmx/VisitorTransferFromAddressLog",
      //    data: {
      //    },

      //    success: function (result) {
      //      getAddressesDates();
      //      util.alert(result.d);
      //    },

      //    error: function (result) {
      //      util.alert(util.formatAjaxError(result, "The operation could not be completed"));
      //    },

      //    complete: function () {
      //      util.closeAjaxDialog();
      //    }
      //  });
      //});

      $(".transfer-from-sample-ballot-log", $masterOnlyTab).click(function() {
        util.openAjaxDialog("Transferring from ballot choices log to visitors...");
        util.ajax({
          url: "/Admin/WebService.asmx/VisitorTransferFromSampleBallotLog",
          data: {
          },

          success: function(result) {
            getAddressesDates();
            util.alert(result.d);
          },

          error: function(result) {
            util.alert(util.formatAjaxError(result,
              "The operation could not be completed"));
          },

          complete: function() {
            util.closeAjaxDialog();
          }
        });
      });

      //$(".lookup-missing-districts", $masterOnlyTab).click(function () {
      //  util.openAjaxDialog("Looking up missing districts for visitors...");
      //  util.ajax({
      //    url: "/Admin/WebService.asmx/VisitorLookupMissingDistricts",
      //    data: {
      //    },

      //    success: function (result) {
      //      getAddressesDates();
      //      util.alert(result.d);
      //    },

      //    error: function (result) {
      //      util.alert(util.formatAjaxError(result, "The operation could not be completed"));
      //    },

      //    complete: function () {
      //      util.closeAjaxDialog();
      //    }
      //  });
      //});

      $(".refresh-all-districts", $masterOnlyTab).click(function() {
        util.openAjaxDialog("Refreshing districts for all visitors...");
        util.ajax({
          url: "/Admin/WebService.asmx/VisitorRefreshAllDistricts",
          data: {
          },

          success: function(result) {
            getAddressesDates();
            util.alert(result.d);
          },

          error: function(result) {
            util.alert(util.formatAjaxError(result,
              "The operation could not be completed"));
          },

          complete: function() {
            util.closeAjaxDialog();
          }
        });
      });

      getAddressesDates();
    }

    function getAddressesDates() {
      util.ajax({
        url: "/Admin/WebService.asmx/GetAddressesDates",

        success: function(result) {
          function storeDate(date, className) {
            if (date) {
              $("#tab-masteronly .intro." + className + " .last-run").html(
                moment(date).format("MM/DD/YYYY hh:mm A"));
            }
          }

          storeDate(result.d.LastRemoveMalformed, "malformed");
          storeDate(result.d.LastTransferFromAddressLog, "from-address");
          storeDate(result.d.LastTransferFromSampleBallotLog, "from-sample-ballot");
          storeDate(result.d.LastLookupMissingDistricts, "lookup-missing");
          storeDate(result.d.LastRefreshAllDistricts, "refresh-all");
        }
      });
    }

    //
    // Misc
    //

    function activateTab(tabName) {
      $mainTabs.tabs("option", "active",
        util.getTabIndex(mainTabsName, "tab-" + tabName));
    }

    function initPage() {

      if (!$(".update-controls").length) return; // bad stateCode on server

      // in AdminMaster:initializePage
      //util.aspKeepAlive();

      $mainTabs = $$(mainTabsName);

      initEditTemplate();
      initAvailableSubstitutions();
      initSelectRecipients();
      initViewRecipients();
      initPreviewSample();
      initEmailOptions();
      initSendEmails();
      initEmailLogging();
      initMasterOnly();

      // propagate classes that can't be accessed directly
      // on .net controls
      $("span.is-option-click input", $mainTabs).addClass("is-option-click");
      $(".save-options-button", $mainTabs).safeBind("click", onClickSaveOptions);

      $mainTabs
        .safeBind("tabsbeforeactivate", onTabsBeforeActivate)
        .safeBind("tabsactivate", onTabsActivate)
        .safeBind("click", onMainTabsClick)
        .safeBind("change", onMainTabsChange);

      window.onbeforeunload = function() {
        if (templateIsDirty())
          return "There are unsaved changes to the bulk email template";
      };
    }

    function onTabsBeforeActivate(event, ui) {
      var newPanelId = ui.newPanel[0].id;
      switch (newPanelId) {
      case viewRecipientsTabName:
        break;
      }
    }

    function onTabsActivate(event, ui) {
      var newPanelId = ui.newPanel[0].id;
      switch (newPanelId) {
      case editTemplateTabName:
        onTabActivateEditTemplate();
        break;

      case viewRecipientsTabName:
        util.clearSelection(); // work around IE issue
        break;

      case previewSampleTabName:
        onTabActivatePreviewSample();
        break;

      case sendEmailsTabName:
        onTabActivateSendEmails();
        break;

      case emailLoggingTabName:
        onTabActivateEmailLogging();
        break;
      }
    }

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });