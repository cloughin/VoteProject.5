define([
  "jquery", "vote/adminMaster", "vote/util", "moment",
    "jqueryui"
  ],
  function ($, master, util, moment) {

    var currentTabData;
    var currentTabInitalJson;

    function initPage() {

      window.onbeforeunload = function () {
        if (panelHasChanges())
          return "There are changes on your form that have not been saved";
      };

      $('#main-tabs')
        .on("tabsbeforeactivate", function (event, ui) {
          return beforeTabActivate(ui.oldPanel[0].id, ui);
        })
        .on("tabsactivate", function (event, ui) {
          tabActivated(ui.newPanel[0].id);
        })
        .on("click", ".mode-button", function () {
          var $mainTab = $(this).closest(".main-tab");
          $mainTab.toggleClass("add-mode").toggleClass("change-mode");
          $mainTab.trigger("modechanged", $mainTab.hasClass("change-mode"));
        })
        .on("click", ".drag-box.can-select p", function () {
          var $this = $(this);
          var wasSelected = $this.hasClass("selected");
          var $dragBox = $this.closest(".drag-box");
          var infoObj = { canSelect: true }
          $dragBox.trigger("beforeselectionchange", infoObj);
          if (infoObj.canSelect) {
            $(".selected", $dragBox).removeClass("selected");
            if (wasSelected) {
              $dragBox.trigger("selectionchanged", -1);
            } else {
              $this.addClass("selected");
              $dragBox.trigger("selectionchanged", $this.data("id"));
            }
          }
        });

      $(".drag-box").sortable({ containment: "parent", distance: 5 });

      initOrganizations();
      initNotes();
      initLogo();
      initAd();
      initOrganizationTypes();
      initOrganizationSubTypes();
      initEmailTags();
      initReport();

      $('#mission-url-dialog').dialog({
          autoOpen: false,
          width: "auto",
          title: "Mission URL",
          resizable: false,
          dialogClass: 'mission-url-dialog overlay-dialog',
          modal: true,
          close: function() {
            $("#tab-organizations .organization-missionurls .drag-box p")
              .removeClass("selected");
          }
        })
        .on("click", ".test-button", onClickMissionUrlTest)
        .on("click", ".ok-button", onClickMissionUrlOk)
        .on("click", ".delete-button", onClickMissionUrlDelete);

      $('#contact-dialog').dialog({
        autoOpen: false,
        width: "auto",
        title: "Contact",
        resizable: false,
        dialogClass: 'contact-dialog overlay-dialog',
        modal: true,
        close: function () {
          $("#tab-organizations .organization-contacts .drag-box p").removeClass("selected");
        }
      })
      .on("click", ".ok-button", onClickContactOk)
      .on("click", ".delete-button", onClickContactDelete);

      tabActivated(util.getCurrentTabId("main-tabs"));
    }

    var startOrgTypeId = null;
    var startOrgId = null;

    function panelHasChanges() {
      switch (util.getCurrentTabId("main-tabs")) {
      case "tab-organizationnotes":
        return editingNote;

      case "tab-organizationlogo":
      case "tab-organizationad":
      case "tab-report":
        return false;

      default:
        return isTabChanged();
      }
    }
    
    function beforeTabActivate(id, ui) {
      switch (id) {
      case "tab-organizations":
      case "tab-organizationnotes":
      case "tab-organizationlogo":
      case "tab-organizationad":
        startOrgTypeId = $(".organization-types-filter", ui.oldPanel).val();
        startOrgId = $(".organizations p.selected", ui.oldPanel).data("id");
        break;
      }
      if (!panelHasChanges()) return true;
      util.confirm("There are unsaved changes on the panel you are leaving.\n\n" +
        "Click OK to discard the changes and continue.\n" +
        "Click Cancel to return to the changed panel.",
        function (button) {
          if (button === "Ok") {
            currentTabData = null;
            editingNote = false;
            $("#main-tabs").tabs("option", "active",
              util.getTabIndex("#main-tabs", ui.newPanel[0].id));
          } else startOrgTypeId = startOrgId = null;
        });
      return false;
    }

    //
    // Common
    //

    function getSelectedOrgId($tab) {
      return $(".organizations p.selected", $tab).data("id");
    }

    function getSelectedOrgTypeId($tab) {
      return $(".organization-types-filter", $tab).val();
    }

    function getSelectedOrgs($tab) {
      var orgTypeId = getSelectedOrgTypeId($tab);
      var result = null;
      $.each(currentTabData.orgs, function () {
        if (this.OrgTypeId == orgTypeId) {
          result = this;
          return false;
        }
      });
      return result;
    }

    function loadOrgs($tab, selectedOrgId) {
      if (!selectedOrgId) selectedOrgId = getSelectedOrgId($tab);
      var orgType = getSelectedOrgs($tab);
      var orgs = [];
      if (orgType)
        $.each(orgType.Organizations, function () {
          var classes = [];
          if (selectedOrgId === this.OrgId) classes.push("selected");
          orgs.push('<p' +
            (classes.length ? ' class="' + classes.join(" ") + '"' : '') +
            ' data-id="' +
            this.OrgId +
            '">' +
            this.Name +
            '</p>');
        });
      $(".organizations", $tab).html(orgs.join(""));
    }

    //
    // Organizations
    //

    var highestOrgId;
    var highestMissionUrId;
    var highestContactId;
    var tempOrg; // only used during add mode

    function initOrganizations() {
      var $tab = $("#tab-organizations");
      $tab
        .on("modechanged", onOrganizationsModeChanged)
        .on("change", ".organization-types-filter", onOrgsOrganizationTypesFilterChanged)
        .on("selectionchanged", ".organizations", onOrganizationsSelectionChanged)
        .on("selectionchanged", ".organization-missionurls .drag-box", onMissionUrlsSelectionChanged)
        .on("selectionchanged", ".organization-contacts .drag-box", onContactsSelectionChanged)
        .on("sortstop", ".organization-missionurls .drag-box", onMissionUrlsSortStop)
        .on("sortstop", ".organization-contacts .drag-box", onContactsSortStop)
        .on("propertychange change click keyup input paste", ".field input,.field select,.field textarea",
          onOrganizationsDataChange)
        .on("click", ".organization-url .button", onClickTestOrganizationUrl)
        .on("click", ".add-organization-button", onClickAddOrganization)
        .on("click", ".delete-organization-button", onClickDeleteOrganization)
        .on("click", ".cancel-button", onClickOrganizationsCancel)
        .on("click", ".add-missionurl-button", onClickMissionUrlAdd)
        .on("click", ".add-contact-button", onClickContactAdd)
        .on("click", ".save-button", onClickOrganizationsSave);
    }

    function initializeTabOrganizations(selectedTypeId, selectedOrgId) {
      var $tab = $("#tab-organizations");
      $tab.addClass("change-mode").removeClass("add-mode");
      $(".disabled", $tab).removeClass("disabled");
      $(".change-button-line .div-button", $tab).addClass("disabled");
      $(".delete-organization-button", $tab).addClass("disabled");
      $(".add-organization-button", $tab).addClass("disabled");
      $(".drag-box", $tab).html("");
      $("input[type=text]", $tab).val("");
      $("select", $tab).val("");
      $("input[type=checkbox]", $tab).prop("checked, false");
      $("textarea", $tab).val("");
      $(".organizations", $tab).addClass("can-select").sortable("option", "disabled", true);
      $(".organization-name input", $tab).prop("disabled", true);
      currentTabData = null;
      currentTabInitalJson = null;
      highestOrgId = 0;
      highestMissionUrId = 0;
      highestContactId = 0;
      util.openAjaxDialog("Loading Organizations...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadOrganizations",

        success: function (result) {
          util.closeAjaxDialog();
          currentTabData = { orgs: result.d.OrgTypes };
          currentTabInitalJson = JSON.stringify(currentTabData);
          $.each(currentTabData.orgs, function () {
            $.each(this.Organizations, function () {
              if (this.OrgId > highestOrgId)
                highestOrgId = this.OrgId;
            });
          });
          $.each(currentTabData.orgs, function () {
            $.each(this.Organizations, function () {
              $.each(this.MissionUrls, function () {
                if (this.OrgMissionUrlId > highestMissionUrId)
                  highestMissionUrId = this.OrgMissionUrlId;
              });
              $.each(this.Contacts, function () {
                if (this.ContactId > highestContactId)
                  highestContactId = this.ContactId;
              });
            });
          });

          // load the ideologies dropdown
          var ideologies = [];
          $.each(result.d.Ideologies, function () {
            ideologies.push({ Text: this.Ideology, Value: this.IdeologyId });
          });
          util.populateDropdown($(".organization-ideology select", $tab), ideologies, "None", "0");

          // load the org types dropdown
          var orgs = [];
          $.each(result.d.OrgTypes, function () {
            orgs.push({ Text: this.OrgType, Value: this.OrgTypeId });
          });
          util.populateDropdown($(".organization-types-filter", $tab), orgs, "<Select Organization Type>", "");

          selectedTypeId = selectedTypeId || startOrgTypeId;
          selectedOrgId = selectedOrgId || startOrgId;
          if (selectedTypeId)
            $(".organization-types-filter", $tab).val(selectedTypeId);
          onOrgsOrganizationTypesFilterChanged(null, selectedOrgId);
          startOrgTypeId = startOrgId = null;
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load Organizations"));
        }
      });
    };

    function adjustOrganizationsUi()
    {
      var $tab = $("#tab-organizations");
      var isChangeMode = $tab.hasClass("change-mode");
      var selectedTypeId = getSelectedOrgTypeId($tab);
      var selectedOrg = getSelectedOrganization();
      var dataEnabled = !!(isChangeMode && selectedOrg || !isChangeMode && selectedTypeId);

      $(".organizations", $tab)
        .toggleClass("can-select", isChangeMode)
        .toggleClass("disabled", !isChangeMode)
        .sortable("option", "disabled", !isChangeMode);
      $(".delete-organization-button", $tab)
        .toggleClass("disabled", !isChangeMode || !selectedOrg);

      $(".organization-name input", $tab).prop("disabled", !dataEnabled);
      $(".organization-abbreviation input", $tab).prop("disabled", !dataEnabled);
      $(".organization-subtype select", $tab).prop("disabled", !dataEnabled);
      $(".organization-ideology select", $tab).prop("disabled", !dataEnabled);
      $(".organization-emailtags input", $tab).prop("disabled", !dataEnabled);
      $(".organization-address1 input", $tab).prop("disabled", !dataEnabled);
      $(".organization-address2 input", $tab).prop("disabled", !dataEnabled);
      $(".organization-city input", $tab).prop("disabled", !dataEnabled);
      $(".organization-state select", $tab).prop("disabled", !dataEnabled);
      $(".organization-zip input", $tab).prop("disabled", !dataEnabled);
      $(".organization-url input", $tab).prop("disabled", !dataEnabled);
      $(".organization-url .button", $tab).prop("disabled", !dataEnabled);
      $(".organization-missionurls .drag-box", $tab)
        .toggleClass("can-select", dataEnabled)
        .toggleClass("disabled", !dataEnabled)
        .sortable("option", "disabled", !dataEnabled);
      $(".add-missionurl-button", $tab).toggleClass("disabled", !dataEnabled);
      $(".organization-contacts .drag-box", $tab)
        .toggleClass("can-select", dataEnabled)
        .toggleClass("disabled", !dataEnabled)
        .sortable("option", "disabled", !dataEnabled);
      $(".add-contact-button", $tab).toggleClass("disabled", !dataEnabled);
      $(".organization-longmission textarea", $tab).prop("disabled", !dataEnabled);
      $(".organization-shortmission textarea", $tab).prop("disabled", !dataEnabled);
      $(".organization-emailmission textarea", $tab).prop("disabled", !dataEnabled);
      if (!dataEnabled) organizationsSetData(null);

      $(".add-organization-button", $tab)
        .toggleClass("disabled", isChangeMode || !$(".organization-name input", $tab).val());

      setTabChanged($tab);
    }

    function getOrganizationById(id) {
      var $tab = $("#tab-organizations");
      var result = null;
      var orgType = getSelectedOrgs($tab);
      if (orgType)
        $.each(orgType.Organizations, function () {
          if (this.OrgId === id) {
            result = this;
            return false;
          }
        });
      return result;
    }

    function getSelectedOrganization() {
      var $tab = $("#tab-organizations");
      return getOrganizationById(getSelectedOrgId($tab));
    }

    function getSelectedOrganizationOrTemp() {
      if ($("#tab-organizations").hasClass("add-mode")) return tempOrg;
      return getSelectedOrganization();
    }

    function onContactsSelectionChanged() {
      var $tab = $("#tab-organizations");
      var $dialog = $('#contact-dialog');
      var org = getSelectedOrganization();
      var id = $(".organization-contacts .drag-box p.selected", $tab).data("id");
      $.each(org.Contacts, function () {
        if (this.ContactId == id) {
          $('.organization-contact input[type=text]', $dialog).val(this.Contact);
          $('.organization-title input[type=text]', $dialog).val(this.Title);
          $('.organization-email input[type=text]', $dialog).val(this.Email);
          $('.organization-phone input[type=text]', $dialog).val(this.Phone);
        }
      });
      $('.id', $dialog).val(id);
      $('.mode', $dialog).val("change");
      $dialog.dialog("open");
    }

    function onClickAddOrganization() {
      if ($(this).hasClass("disabled")) return;
      var $tab = $("#tab-organizations");
      var id = ++highestOrgId;
      var org = { OrgId: id };
      organizationStoreData(org);
      org.Contacts = tempOrg.Contacts;
      org.MissionUrls = tempOrg.MissionUrls;
      getSelectedOrgs($tab).Organizations.push(org);
      loadOrgs($tab, id);
      onOrganizationsSelectionChanged(null, id);
      organizationsSetData(null);
      onOrganizationsDataChange();
    }

    function onClickDeleteOrganization() {
      var $tab = $("#tab-organizations");
      if ($(this).hasClass("disabled")) return;
      var id = getSelectedOrgId($tab);
      var orgs = getSelectedOrgs($tab);
      var temp = [];
      $.each(orgs.Organizations, function () {
        if (this.OrgId !== id)
          temp.push(this);
      });
      orgs.Organizations = temp;
      loadOrgs($tab);
      onOrganizationsSelectionChanged(null);
      onOrganizationsDataChange();
    }

    function onClickContactAdd() {
      if ($(this).hasClass("disabled")) return;
      var $dialog = $('#contact-dialog');
      $('.organization-contact input[type=text]', $dialog).val("");
      $('.organization-title input[type=text]', $dialog).val("");
      $('.organization-email input[type=text]', $dialog).val("");
      $('.organization-phone input[type=text]', $dialog).val("");
      $('.id', $dialog).val("");
      $('.mode', $dialog).val("add");
      adjustOrganizationsUi();
      $dialog.dialog("open");
    }

    function onClickContactDelete() {
      var $dialog = $('#contact-dialog');
      var org = getSelectedOrganizationOrTemp();
      var id = $(".id", $dialog).val();
      var wk = [];
      $.each(org.Contacts, function () {
        if (this.ContactId != id) {
          wk.push(this);
        }
      });
      org.Contacts = wk;
      organizationContactsSetData(org);
      adjustOrganizationsUi();
      $dialog.dialog("close");
    }

    function onClickContactOk() {
      var $dialog = $('#contact-dialog');
      var org = getSelectedOrganizationOrTemp();
      var contact = $.trim($(".organization-contact input[type=text]", $dialog).val());
      var title = $.trim($(".organization-title input[type=text]", $dialog).val());
      var email = $.trim($(".organization-email input[type=text]", $dialog).val());
      var phone = $.trim($(".organization-phone input[type=text]", $dialog).val());
      var id = $(".id", $dialog).val();
      if ($(".mode", $dialog).val() === "add") {
        org.Contacts.push({
          ContactId: ++highestContactId,
          Contact: contact,
          Email: email,
          Phone: phone,
          Title: title
        });
      } else {
        $.each(org.Contacts, function () {
          if (this.ContactId == id) {
            this.Contact = contact;
            this.Email = email;
            this.Phone = phone;
            this.Title = title;
          }
        });
      }
      organizationContactsSetData(org);
      adjustOrganizationsUi();
      $dialog.dialog("close");
    }

    function onContactsSortStop() {
      var $tab = $("#tab-organizations");
      var org = getSelectedOrganizationOrTemp();
      var wk = [];
      $(".organization-contacts .drag-box p", $tab).each(function () {
        var $this = $(this);
        $.each(org.Contacts, function () {
          if ($this.data("id") == this.ContactId)
            wk.push(this);
        });
      });
      org.Contacts = wk;
      organizationContactsSetData(org);
      adjustOrganizationsUi();
    }

    function onMissionUrlsSelectionChanged() {
      var $tab = $("#tab-organizations");
      var $dialog = $('#mission-url-dialog');
      var org = getSelectedOrganization();
      var id = $(".organization-missionurls .drag-box p.selected", $tab).data("id");
      $.each(org.MissionUrls, function () {
        if (this.OrgMissionUrlId == id) {
          $('.organization-missionurl input[type=text]', $dialog).val(this.Url);
        }
      });
      $('.id', $dialog).val(id);
      $('.mode', $dialog).val("change");
      $dialog.dialog("open");
    }

    function onClickMissionUrlAdd() {
      if ($(this).hasClass("disabled")) return;
      var $dialog = $('#mission-url-dialog');
      $('.organization-missionurl input[type=text]', $dialog).val("");
      $('.id', $dialog).val("");
      $('.mode', $dialog).val("add");
      adjustOrganizationsUi();
      $dialog.dialog("open");
    }

    function onClickMissionUrlDelete() {
      var $dialog = $('#mission-url-dialog');
      var org = getSelectedOrganizationOrTemp();
      var id = $(".id", $dialog).val();
      var wk = [];
      $.each(org.MissionUrls, function () {
        if (this.OrgMissionUrlId != id) {
          wk.push(this);
        }
      });
      org.MissionUrls = wk;
      organizationMissionUrlsSetData(org);
      adjustOrganizationsUi();
      $dialog.dialog("close");
    }

    function onClickMissionUrlOk() {
      var $dialog = $('#mission-url-dialog');
      var org = getSelectedOrganizationOrTemp();
      var url = $.trim($(".organization-missionurl input[type=text]", $dialog).val());
      var id = $(".id", $dialog).val();
      if ($(".mode", $dialog).val() === "add") {
        org.MissionUrls.push({
          OrgMissionUrlId: ++highestMissionUrId,
          Url: url
        });
      } else {
        $.each(org.MissionUrls, function () {
          if (this.OrgMissionUrlId == id) {
            this.Url = url;
          }
        });
      }
      organizationMissionUrlsSetData(org);
      adjustOrganizationsUi();
      $dialog.dialog("close");
    }

    function onClickMissionUrlTest()  {
      var url = $.trim(
        $("#mission-url-dialog .organization-missionurl input[type=text]").val());
      if (url) {
        if (url.toLowerCase().substr(0, 4) !== "http")
          url = "http://" + url;
        window.open(url, "test");
      }
    }

    function onClickOrganizationsSave() {
      if ($(this).hasClass("disabled")) return;
      var $tab = $("#tab-organizations");
      var selectedTypeId = getSelectedOrgTypeId($tab);
      var selectedId = getSelectedOrgId($tab);
      var hasError = false;

      function error(org, selector, msg) {
        if (org.OrgTypeId != selectedTypeId)
          $(".organization-types-filter", $tab).val(org.OrgTypeId);
        if (org.OrgId != selectedId)
          onOrgsOrganizationTypesFilterChanged(null, org.OrgId);
        $(selector, $tab).addClass("error-field");
        util.alert(msg);
        hasError = true;
      }

      // edit emails and urls
      $.each(currentTabData.orgs, function () {
        $.each(this.Organizations, function () {
          var org = this;
          if (this.Url && !util.isValidUrl(this.Url))
            error(this, ".organization-url input", "Invalid URL");
          $.each(this.MissionUrls, function () {
            if (!util.isValidUrl(this.Url))
              error(org, ".organization-missionurls p[data-id=" + this.OrgMissionUrlId + "]", "Invalid URL");
            if (hasError) return false;
          });
          $.each(this.Contacts, function () {
            if (this.Email && !util.isValidEmail(this.Email))
              error(org, ".organization-contacts p[data-id=" + this.ContactId + "]", "Invalid Email");
            if (hasError) return false;
          });
          if (hasError) return false;
        });
        if (hasError) return false;
      });
      if (hasError) return;

      // null out all dates
      var data = JSON.parse(JSON.stringify(currentTabData.orgs));
      $.each(data, function() {
        $.each(this.Organizations, function() {
          this.DateStamp = null;
        });
      });

      util.openAjaxDialog("Saving changes...");
      util.ajax({
        url: "/Admin/WebService.asmx/SaveOrganizations",
        data: { data: data },

        success: function () {
          util.closeAjaxDialog();
          initializeTabOrganizations(selectedTypeId, selectedId);
          util.alert("Changes saved");
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not save changes"));
        }
      });
    }

    function onClickOrganizationsCancel() {
      if ($(this).hasClass("disabled")) return;
      initializeTabOrganizations();
    }

    function onMissionUrlsSortStop() {
      var $tab = $("#tab-organizations");
      var org = getSelectedOrganizationOrTemp();
      var wk = [];
      $(".organization-missionurls .drag-box p", $tab).each(function () {
        var $this = $(this);
        $.each(org.MissionUrls, function() {
          if ($this.data("id") == this.OrgMissionUrlId)
            wk.push(this);
        });
      });
      org.MissionUrls = wk;
      organizationMissionUrlsSetData(org);
      adjustOrganizationsUi();
    }

    function onClickTestOrganizationUrl(event) {
      var $tab = $("#tab-organizations");
      var url = $.trim($(".organization-url input", $tab).val());
      if (url) {
        if (url.toLowerCase().substr(0, 4) !== "http")
          url = "http://" + url;
        window.open(url, "test");
      }
      event.preventDefault();
    }

    function onOrganizationsDataChange() {
      $(this).removeClass("error-field");
      var $tab = $("#tab-organizations");
      var org = getSelectedOrganization();
      if ($tab.hasClass("change-mode")) {
        if (org) {
          organizationStoreData(org);
        }
      }
      adjustOrganizationsUi();
    }

    function onOrganizationsModeChanged() {
      var $tab = $("#tab-organizations");
      var isChangeMode = $tab.hasClass("change-mode");
      if (!isChangeMode) tempOrg = { Contacts: [], MissionUrls: [] };
      var org = isChangeMode ? getSelectedOrganization() : null;
      organizationsSetData(org);
      adjustOrganizationsUi();
    }

    function onOrgsOrganizationTypesFilterChanged(dummy, selectedOrgId) {
      var $tab = $("#tab-organizations");

      // load subtypes dropdown
      var items = [{ Text: "None", Value: "0" }];
      var orgType = getSelectedOrgs($tab);
      if (orgType)
        $.each(orgType.SubTypes, function () {
          items.push({ Text: this.OrgSubType, Value: this.OrgSubTypeId });
        });
      util.populateDropdown($(".organization-subtype select", $tab), items);

      // create email tags checkboxes
      var $checkboxes = $(".organization-emailtags .checkboxes", $tab);
      if (orgType && orgType.EmailTags.length) {
        var checkboxes = [];
        $.each(orgType.EmailTags, function () {
          checkboxes.push('<div><input type="checkbox" value="' + this.EmailTagId + '"/>' + this.EmailTag + '</div>');
        });
        $checkboxes.html(checkboxes.join(""));
      } else if (orgType) {
        $checkboxes.html("<p>No email tags for this organization type</p>");
      } else {
        $checkboxes.html("");
      }

      loadOrgs($tab, selectedOrgId);
      onOrganizationsSelectionChanged(dummy, selectedOrgId);
    }

    function onOrganizationsSelectionChanged(dummy, id) {
      var $tab = $("#tab-organizations");
      organizationsSetData(null);
      var orgType = getSelectedOrgs($tab);
      if (orgType)
        $.each(orgType.Organizations, function () {
          if (this.OrgId == id) {
            organizationsSetData(this);
            return false;
          }
        });
      adjustOrganizationsUi();
    }

    function organizationsSetData(org) {
      var $tab = $("#tab-organizations");
      $(".organization-emailtags input", $tab).prop("checked", false);
      $(".organization-lastupdated input", $tab).val("");
      if (!org) {
        $(".organization-name input", $tab).val("");
        $(".organization-abbreviation input", $tab).val("");
        $(".organization-subtype select", $tab).val("0");
        $(".organization-ideology select", $tab).val("0");
        $(".organization-emailtags input", $tab).prop("checked", false);
        $(".organization-address1 input", $tab).val("");
        $(".organization-address2 input", $tab).val("");
        $(".organization-city input", $tab).val("");
        $(".organization-state select", $tab).val("");
        $(".organization-zip input", $tab).val("");
        $(".organization-url input", $tab).val("");
        $(".organization-missionurls .drag-box", $tab).html("");
        $(".organization-contacts .drag-box", $tab).html("");
        $(".organization-longmission textarea", $tab).val("");
        $(".organization-shortmission textarea", $tab).val("");
        $(".organization-emailmission textarea", $tab).val("");
      } else {
        $(".organization-name input", $tab).val(org.Name);
        if (org.DateStamp)
          $(".organization-lastupdated input", $tab).val(moment(org.DateStamp).format("MM/DD/YYYY hh:mm:ss A"));
        $(".organization-abbreviation input", $tab).val(org.OrgAbbreviation);
        $(".organization-subtype select", $tab).val(org.OrgSubTypeId);
        $(".organization-ideology select", $tab).val(org.IdeologyId);
        organizationEmailTagsSetData(org);
        $(".organization-address1 input", $tab).val(org.Address1);
        $(".organization-address2 input", $tab).val(org.Address2);
        $(".organization-city input", $tab).val(org.City);
        $(".organization-state select", $tab).val(org.StateCode);
        $(".organization-zip input", $tab).val(org.Zip);
        $(".organization-url input", $tab).val(org.Url);
        organizationMissionUrlsSetData(org);
        organizationContactsSetData(org);
        $(".organization-longmission textarea", $tab).val(org.LongMission);
        $(".organization-shortmission textarea", $tab).val(org.ShortMission);
        $(".organization-emailmission textarea", $tab).val(org.EmailMission);
      }
    }

    function organizationContactsSetData(org) {
      var $tab = $("#tab-organizations");
      var contacts = [];
      $.each(org.Contacts, function () {
        contacts.push('<p data-id="' + this.ContactId + '">' +
          (this.Contact ? this.Contact : "&lt;no contact name&gt;") +
          (this.Title ? ('<br/>' + this.Title) : "") +
          (this.Email ? ('<br/>' + this.Email) : "") +
          (this.Phone ? ('<br/>' + this.Phone) : "") +
          '</p>');
      });
      $(".organization-contacts .drag-box", $tab).html(contacts.join(""));
    }

    function organizationEmailTagsSetData(org) {
      var $tab = $("#tab-organizations");
      $(".organization-emailtags input", $tab).prop("checked", false);
      $.each(org.EmailTagIds, function () {
        $(".organization-emailtags input[value=" + this + "]", $tab).prop("checked", true);
      });
    }

    function organizationMissionUrlsSetData(org) {
      var $tab = $("#tab-organizations");
      var missionUrls = [];
      $.each(org.MissionUrls, function () {
        missionUrls.push('<p data-id="' + this.OrgMissionUrlId + '">' + this.Url + '</p>');
      });
      $(".organization-missionurls .drag-box", $tab).html(missionUrls.join(""));
    }

    function organizationStoreData(org) {
      var $tab = $("#tab-organizations");
      org.OrgSubTypeId = parseInt($(".organization-subtype select", $tab).val());
      org.IdeologyId = parseInt($(".organization-ideology select", $tab).val());
      org.Name = $.trim($(".organization-name input", $tab).val());
      org.OrgAbbreviation = $.trim($(".organization-abbreviation input", $tab).val());
      org.Address1 = $.trim($(".organization-address1 input", $tab).val());
      org.Address2 = $.trim($(".organization-address2 input", $tab).val());
      org.City = $.trim($(".organization-city input", $tab).val());
      org.StateCode = $(".organization-state select", $tab).val();
      org.Zip = $.trim($(".organization-zip input", $tab).val());
      org.Url = $.trim($(".organization-url input", $tab).val());
      org.LongMission = $.trim($(".organization-longmission textarea", $tab).val());
      org.ShortMission = $.trim($(".organization-shortmission textarea", $tab).val());
      org.EmailMission = $.trim($(".organization-emailmission textarea", $tab).val());
      var emailTags = [];
      $(".organization-emailtags input", $tab).each(function () {
        var $this = $(this);
        if ($this.is(":checked"))
          emailTags.push(parseInt($this.val()));
      });
      emailTags.sort();
      org.EmailTagIds = emailTags;
    }

    //
    // Notes
    //

    var highestNoteId;
    var editingNote;
    var editingContent;
    var lastSelectedNotesOrgTypeId;

    function initNotes() {
      var $tab = $("#tab-organizationnotes");
      $tab
        .on("change", ".organization-types-filter", onNotesOrganizationTypesFilterChanged)
        .on("beforeselectionchange", ".organizations", function (event, infoObj) {
          if (editingNote) {
            infoObj.canSelect = false;
            util.alert("A note is currently being edited. Please save or cancel before changing your selection.");
          }
        })
        .on("selectionchanged", ".organizations", onNotesSelectionChanged)
        .on("click", ".link.edit", function () {
          if (editingNote) {
            util.alert("A note is already being edited. Please save or cancel before editing another note.");
            return;
          }
          var $header = $(this).closest(".note-header");
          var $body = $header.next();
          $body.addClass("editing").prop("contentEditable", true);
          $(".link.edit", $header).addClass("hidden");
          $(".link.delete", $header).addClass("hidden");
          $(".link.save", $header).removeClass("hidden");
          $(".link.cancel", $header).removeClass("hidden");
          editingNote = true;
          editingContent = $body.html();
        })
        .on("click", ".link.delete", function () {
          if (editingNote) {
            util.alert("A note is currently being edited. Please save or cancel before deleting another note.");
            return;
          }
          var $header = $(this).closest(".note-header");
          var id = $header.data("id");
          if (util.confirm("Are you sure you want to delete this note?", function (button) {
            if (button == "Ok") {
              util.openAjaxDialog("Deleting note...");
              util.ajax({
                url: "/Admin/WebService.asmx/DeleteOrganizationNote",
                data: {
                  id: id
                },

                success: function () {
                  initializeTabNotes(getSelectedOrgTypeId($tab),
                    getSelectedOrgId($tab));
                },

                error: function (result) {
                  util.alert(util.formatAjaxError(result, "Could not delete note"));
                },

                complete: function () {
                  util.closeAjaxDialog();
                }
              });
            }
          }));
        })
        .on("click", ".link.save", function () {
          var $header = $(this).closest(".note-header");
          var $body = $header.next();
          var id = $header.data("id");

          util.openAjaxDialog("Saving note...");
          util.ajax({
            url: "/Admin/WebService.asmx/SaveOrganizationNote",
            data: {
              id: id,
              note: $body.html()
            },

            success: function () {
              initializeTabNotes(getSelectedOrgTypeId($tab),
                getSelectedOrgId($tab));
            },

            error: function (result) {
              util.alert(util.formatAjaxError(result, "Could not save note"));
            },

            complete: function () {
              util.closeAjaxDialog();
            }
          });
        })
        .on("click", ".link.cancel", function () {
          var $header = $(this).closest(".note-header");
          var $body = $header.next();
          $body.removeClass("editing").prop("contentEditable", false);
          $(".link.edit", $header).removeClass("hidden");
          $(".link.delete", $header).removeClass("hidden");
          $(".link.save", $header).addClass("hidden");
          $(".link.cancel", $header).addClass("hidden");
          editingNote = false;
          $body.html(editingContent);
        })
        .on("click", ".add-button input", function () {
          if ($(this).hasClass("disabled")) return;
          var $tab = $("#tab-organizationnotes");
          if (editingNote) {
            util.alert("A note is currently being edited. Please save or cancel before adding another note.");
            return;
          }
          util.openAjaxDialog("Adding note...");
          util.ajax({
            url: "/Admin/WebService.asmx/AddOrganizationNote",
            data: {
              orgId: getSelectedOrgId($tab),          
              note: $(".new-note textarea", $tab).val()

            },

            success: function () {
              initializeTabNotes(getSelectedOrgTypeId($tab),
                getSelectedOrgId($tab));
            },

            error: function (result) {
              util.alert(util.formatAjaxError(result, "Could not add note"));
            },

            complete: function () {
              util.closeAjaxDialog();
            }
          });
        })
        .on("propertychange change click keyup input paste", ".new-note textarea",
          onNotesDataChange);
    }

    function initializeTabNotes(selectedTypeId, selectedOrgId) {
      var $tab = $("#tab-organizationnotes");
      lastSelectedNotesOrgTypeId = "";
      $(".disabled", $tab).removeClass("disabled");
      $(".change-button-line .div-button", $tab).addClass("disabled");
      $(".drag-box", $tab).html("");
      $(".org-header", $tab).hide();
      $(".organizations", $tab).addClass("can-select").sortable("option", "disabled", true);
      currentTabData = null;
      currentTabInitalJson = null;
      highestNoteId = 0;
      $(".new-note textarea", $tab).val("");
      util.openAjaxDialog("Loading Notes...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadOrganizationNotes",

        success: function (result) {
          util.closeAjaxDialog();
          currentTabData = { orgs: result.d };
          currentTabInitalJson = JSON.stringify(currentTabData);
          $.each(currentTabData.orgs, function () {
            $.each(this.Organizations, function () {
              $.each(this.Notes, function () {
                if (this.Id > highestNoteId)
                  highestNoteId = this.Id;
              });
            });
          });

          // load the org types dropdown
          var orgs = [];
          $.each(result.d, function () {
            orgs.push({ Text: this.OrgType, Value: this.OrgTypeId });
          });
          util.populateDropdown($(".organization-types-filter", $tab), orgs, "<Select Organization Type>", "");


          selectedTypeId = selectedTypeId || startOrgTypeId;
          selectedOrgId = selectedOrgId || startOrgId;
          if (selectedTypeId)
            $(".organization-types-filter", $tab).val(selectedTypeId);
          onNotesOrganizationTypesFilterChanged(null, selectedOrgId);
          startOrgTypeId = startOrgId = null;
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load Organization Notes"));
        }
      });
    };

    function adjustNotesUi() {
      var $tab = $("#tab-organizationnotes");
      var selectedOrgId = getSelectedOrgId($tab);
      $(".new-note textarea", $tab).prop("disabled", !selectedOrgId);
      $(".add-button input").toggleClass("disabled", !(selectedOrgId && $.trim($(".new-note textarea", $tab).val())));
    }

    function onNotesDataChange() {
      adjustNotesUi();
    }

    function onNotesSelectionChanged(dummy, id) {
      var $tab = $("#tab-organizationnotes");
      $(".org-header", $tab).hide();
      notesSetData(null);
      var orgType = getSelectedOrgs($tab);
      if (orgType)
        $.each(orgType.Organizations, function () {
          if (this.OrgId == id) {
            $(".org-header span", $tab).text(this.Name);
            $(".org-header", $tab).show();
            notesSetData(this);
            return false;
          }
        });
      adjustNotesUi();
    }

    function onNotesOrganizationTypesFilterChanged(dummy, selectedOrgId) {
      var $tab = $("#tab-organizationnotes");
      if (editingNote) {
        $(".organization-types-filter", $tab).val(lastSelectedNotesOrgTypeId);
        util.alert("A note is currently being edited. Please save or cancel before changing your selection.");
        return;
      }
      loadOrgs($tab, selectedOrgId);
      onNotesSelectionChanged(dummy, selectedOrgId);
      lastSelectedNotesOrgTypeId = getSelectedOrgTypeId($tab);
    }

    function notesSetData(org) {
      var $tab = $("#tab-organizationnotes");
      if (!org) {
        $(".notes", $tab).html("");
      } else {
        if (org.Notes.length) {
          var n = [];
          $.each(org.Notes, function () {
            n.push('<div class="note-header" data-id="' + this.Id + '"><span class="date">' +
              moment(this.DateStamp).format("MM/DD/YY hh:mm:ssa") + '</span>' +
              '<span class="edit link">edit</span>' +
              '<span class="delete link">delete</span>' +
              '<span class="save link hidden">save</span>' +
              '<span class="cancel link hidden">cancel</span>' +
              '</div>');
            n.push('<div class="note-body" contentEditable="false">' + this.Notes + '</div>');
          });
          $(".notes", $tab).html(n.join(""));
        } else {
          $(".notes", $tab).text("No notes.");
        }
      }
      editingNote = false;
    }

    //
    //  Image
    //   (code shared by Logo and Ad)
    //

    //
    // Logo
    //

    function initLogo() {
      var $tab = $("#tab-organizationlogo");
      $tab
        .on("change", ".organization-types-filter", onLogoOrganizationTypesFilterChanged)
        .on("selectionchanged", ".organizations", onLogoSelectionChanged);
    }

    function initializeTabLogo(selectedTypeId, selectedOrgId) {
      var $tab = $("#tab-organizationlogo");
      $(".drag-box", $tab).html("");
      $(".org-header", $tab).hide();
      $(".organizations", $tab).addClass("can-select").sortable("option", "disabled", true);
      currentTabData = null;
      util.openAjaxDialog("Initializing...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadOrganizationImageData",

        success: function (result) {
          util.closeAjaxDialog();
          currentTabData = { orgs: result.d };

          // load the org types dropdown
          var orgs = [];
          $.each(result.d, function () {
            orgs.push({ Text: this.OrgType, Value: this.OrgTypeId });
          });
          util.populateDropdown($(".organization-types-filter", $tab), orgs, "<Select Organization Type>", "");


          selectedTypeId = selectedTypeId || startOrgTypeId;
          selectedOrgId = selectedOrgId || startOrgId;
          if (selectedTypeId)
            $(".organization-types-filter", $tab).val(selectedTypeId);
          onLogoOrganizationTypesFilterChanged(null, selectedOrgId);
          startOrgTypeId = startOrgId = null;
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load logo info"));
        }
      });
    };

    function adjustLogoUi() {
    }

    function onLogoSelectionChanged(dummy, id) {
      var $tab = $("#tab-organizationlogo");
      $(".org-header", $tab).hide();
      var orgType = getSelectedOrgs($tab);
      if (orgType)
        $.each(orgType.Organizations, function () {
          if (this.OrgId == id) {
            $(".org-header span", $tab).text(this.Name);
            $(".org-header", $tab).show();
            return false;
          }
        });
      adjustLogoUi();
    }

    function onLogoOrganizationTypesFilterChanged(dummy, selectedOrgId) {
      var $tab = $("#tab-organizationlogo");
      loadOrgs($tab, selectedOrgId);
      onLogoSelectionChanged(dummy, selectedOrgId);
    }

    //
    // Upload Ad
    //

    function initAd() {
      var $tab = $("#tab-organizationad");
      $tab
        .on("change", ".organization-types-filter", onAdOrganizationTypesFilterChanged)
        .on("selectionchanged", ".organizations", onAdSelectionChanged)
        .on("change", "#AdImageFile", function() {
          var name = $(this).val();
          var slashPos = name.lastIndexOf("\\");
          if (slashPos >= 0)
            name = name.substr(slashPos + 1);
          $(".image-file-name", $tab).val(name);
          $(".image-file-changed", $tab).val("True").trigger("change");
          currentTabData.orgData.AdName = name;
          currentTabData.orgData.imageFileChanged = true;
          adjustAdUi($tab);
        })
        .on("input", ".ad-url", function() {
          currentTabData.orgData.AdUrl = $.trim($(".ad-url", $tab).val());
          adjustAdUi($tab);
        })
        .on("click", ".update-ad-button", onUpdateAd)
        .on("click", ".delete-ad-button", function(){onDeleteAd(false)});
    }

    function initializeTabAd(selectedTypeId, selectedOrgId) {
      var $tab = $("#tab-organizationad");
      $(".drag-box", $tab).html("");
      $(".input-area", $tab).hide();
      $(".organizations", $tab).addClass("can-select").sortable("option", "disabled", true);
      currentTabData = null;
      util.openAjaxDialog("Initializing...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadOrganizationImageData",

        success: function (result) {
          util.closeAjaxDialog();
          currentTabData = { orgs: result.d };

          // load the org types dropdown
          var orgs = [];
          $.each(result.d, function () {
            orgs.push({ Text: this.OrgType, Value: this.OrgTypeId });
          });
          util.populateDropdown($(".organization-types-filter", $tab), orgs, "<Select Organization Type>", "");


          selectedTypeId = selectedTypeId || startOrgTypeId;
          selectedOrgId = selectedOrgId || startOrgId;
          if (selectedTypeId)
            $(".organization-types-filter", $tab).val(selectedTypeId);
          onAdOrganizationTypesFilterChanged(null, selectedOrgId);
          startOrgTypeId = startOrgId = null;
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load ad info"));
        }
      });
    };

    function adjustAdUi($tab) {
      //setTabChanged($tab);
      var canUpdate = isTabChanged($tab) &&
        $(".image-file-name", $tab).val() /*&&
        $.trim($(".ad-url", $tab).val())*/;
      $(".update-ad-button", $tab).toggleClass("disabled", !canUpdate);
    }

    function onAdSelectionChanged(dummy, id) {
      var $tab = $("#tab-organizationad");
      $(".input-area", $tab).hide();
      var org = null;
      var orgType = getSelectedOrgs($tab);
      if (orgType)
        $.each(orgType.Organizations, function () {
          if (this.OrgId == id) {
            org = this;
            return false;
          }
        });
      if (org) {
        util.openAjaxDialog("Getting ad...");
        util.ajax({
          url: "/Admin/WebService.asmx/LoadOrganizationAdData",
          data: { orgId: org.OrgId },

          success: function(result) {
            util.closeAjaxDialog();

            // process and remove the sample ad
            var sample = result.d.Sample;
            delete result.d.Sample;
            if (sample) {
              $(".sample-ad", $tab).show();
              $(".sample-ad-content", $tab).html(sample);
              var href = $(".home-page-link", $tab).attr("href").replace(/(.*\?ad=)(\d+)(.*)/g, "$1" + org.OrgId + "$3");
              $(".home-page-link", $tab).attr("href", href);
            } else {
              $(".sample-ad", $tab).hide();
            }

            var orgData = result.d;
            currentTabData.orgData = orgData;
            orgData.imageFileChanged = false;
            currentTabInitalJson = JSON.stringify(currentTabData);
            $(".org-header span", $tab).text(org.Name + " (id=" + id + ")");
            $(".image-file-name", $tab).val(orgData.AdImageName);
            $(".ad-url", $tab).val(orgData.AdUrl);
            $(".default-url span", $tab).text(orgData.OrgUrl);
            var haveAd = !!orgData.AdImageName;
            $(".view-sample", $tab).toggleClass("disabled", !haveAd);
            $(".update-ad-button", $tab).toggleClass("disabled", true);
            $(".delete-ad-button", $tab).toggleClass("disabled", !haveAd);
            $(".image-file-changed", $tab).val("False");
            $(".image-file-updated", $tab).val("False");
            $("#AdImageFile").val(null);
            $(".input-area", $tab).show();
            adjustAdUi($tab);
          },

          error: function(result) {
            util.closeAjaxDialog();
            util.alert(util.formatAjaxError(result,
              "Could not load ad info"));
          }
        });
      } else {
        delete currentTabData.orgData;
        currentTabInitalJson = JSON.stringify(currentTabData);
      }
      adjustAdUi($tab);
    }

    function onAdOrganizationTypesFilterChanged(dummy, selectedOrgId) {
      var $tab = $("#tab-organizationad");
      loadOrgs($tab, selectedOrgId);
      onAdSelectionChanged(dummy, selectedOrgId);
    }

    function onDeleteAd(force) {
      var $tab = $("#tab-organizationad");
      if (!force) {
        util.confirm("Are you sure you want to delete this organization's ad ", function (button) {
          if (button === "Ok") {
            onDeleteAd(true);
          }
        });
        return;
      }
      util.openAjaxDialog("Deleting ad...");
      util.ajax({
        url: "/Admin/WebService.asmx/DeleteOrganizationAd",

        data: {
          orgId: getSelectedOrgId($tab)
        },

        success: function () {
          util.closeAjaxDialog();
          //$("#AdImageFile").val(null);
          //currentTabData.orgData.AdImageName = "";
          //currentTabData.orgData.AdUrl = "";
          //currentTabData.orgData.OrgUrl = "";
          //currentTabData.orgData.imageFileChanged = false;
          //$(".image-file-name", $tab).val("");
          //$(".ad-url", $tab).val("");
          //$(".default-url span", $tab).val("");
          //$(".image-file-changed", $tab).val("False");
          //currentTabInitalJson = JSON.stringify(currentTabData);
          //$(".view-sample", $tab).toggleClass("disabled", true);
          //$(".update-ad-button", $tab).toggleClass("disabled", true);
          //$(".delete-ad-button", $tab).toggleClass("disabled", true);
          //adjustAdUi($tab);
          onAdSelectionChanged(null, getSelectedOrgId($tab));
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not delete ad"));
        }
      });
    }

    function onUpdateAd() {
      var $tab = $("#tab-organizationad");

      // we use a little different technique here because of the uploaded file
      var formdata = new FormData();
      if (currentTabData.orgData.imageFileChanged)
        formdata.append("file", $("#AdImageFile")[0].files[0]);
      formdata.append("orgId", getSelectedOrgId($tab));
      formdata.append("url", $.trim($(".ad-url", $tab).val()));
      formdata.append("filename", $(".image-file-name", $tab).val());
      formdata.append("imageFileChanged", $(".image-file-changed", $tab).val().toLowerCase());

      util.openAjaxDialog("Updating ad...");
      $.ajax({
        url: "/Admin/WebService.asmx/UpdateOrganizationAd",
        data: formdata,
        type: "POST",
        contentType: false, // Not to set any content header
        processData: false,

        success: function () {
          util.closeAjaxDialog();
          //currentTabData.imageFileChanged = false;
          //$(".image-file-changed", $tab).val("False");
          //currentTabInitalJson = JSON.stringify(currentTabData);
          //$(".view-sample", $tab).toggleClass("disabled", false);
          //$(".update-ad-button", $tab).toggleClass("disabled", true);
          //$(".delete-ad-button", $tab).toggleClass("disabled", false);
          //adjustAdUi($tab);
          onAdSelectionChanged(null, getSelectedOrgId($tab));
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not update ad"));
        }
      });
    }

    //function adClearData() {

    //}

    //
    // Organization Types
    //

    var highestOrgTypeId;

    function initOrganizationTypes() {
      var $tab = $("#tab-organizationtypes");
      $tab
        .on("modechanged", onOrganizationTypesModeChanged)
        .on("selectionchanged", ".organization-types", onOrganizationTypesSelectionChanged)
        .on("sortstop", ".organization-types", onOrganizationTypesSortStop)
        .on("propertychange change click keyup input paste", ".organization-type input",
          onOrganizationTypesDataChange)
        .on("click", ".add-organization-type-button", onClickAddOrganizationType)
        .on("click", ".delete-organization-type-button", onClickDeleteOrganizationType)
        .on("click", ".cancel-button", onClickOrganizationTypesCancel)
        .on("click", ".save-button", onClickOrganizationTypesSave);
    }

    function initializeTabOrganizationTypes(selectedId) {
      var $tab = $("#tab-organizationtypes");
      $tab.addClass("change-mode").removeClass("add-mode");
      $(".disabled", $tab).removeClass("disabled");
      $(".change-button-line .div-button", $tab).addClass("disabled");
      $(".delete-organization-type-button", $tab).addClass("disabled");
      $(".add-organization-type-button", $tab).addClass("disabled");
      $(".drag-box", $tab).html("");
      $("input[type=text]", $tab).val("");
      $(".organization-types", $tab).addClass("can-select").sortable("option", "disabled", false);
      $(".organization-type input", $tab).prop("disabled", true);
      currentTabData = null;
      currentTabInitalJson = null;
      highestOrgTypeId = 0;
      util.openAjaxDialog("Loading Organization Types...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadOrganizationTypes",

        success: function (result) {
          util.closeAjaxDialog();
          currentTabData = { orgs: result.d };
          currentTabInitalJson = JSON.stringify(currentTabData);
          $.each(currentTabData.orgs, function () {
            if (this.OrgTypeId > highestOrgTypeId)
              highestOrgTypeId = this.OrgTypeId;
          });
          loadOrganizationTypes(selectedId);
          onOrganizationTypesSelectionChanged(null, selectedId);
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load Organization Types"));
        }
      });
    };

    function adjustTypesUi() {
      var $tab = $("#tab-organizationtypes");
      var isChangeMode = $tab.hasClass("change-mode");
      var selectedType = getSelectedOrganizationType();
      var dataEnabled = !!(isChangeMode && selectedType || !isChangeMode);

      $(".organization-types", $tab)
        .toggleClass("can-select", isChangeMode)
        .toggleClass("disabled", !isChangeMode)
        .sortable("option", "disabled", !isChangeMode);

      $(".delete-organization-type-button", $tab)
        .toggleClass("disabled", !isChangeMode || !selectedType || selectedType.Count !== 0);
      $(".organization-type input", $tab).prop("disabled", !dataEnabled);
      if (!dataEnabled) organizationTypesSetData(null);
      $(".add-organization-type-button", $tab)
        .toggleClass("disabled", isChangeMode || !$(".organization-type input", $tab).val());

      setTabChanged($tab);
    }

    function getOrganizationTypeById(id) {
      var result = null;
      $.each(currentTabData.orgs, function () {
        if (this.OrgTypeId === id) {
          result = this;
          return false;
        }
      });
      return result;
    }

    function getSelectedOrganizationType() {
      return getOrganizationTypeById(getSelectedOrganizationTypeId());
    }

    function getSelectedOrganizationTypeId() {
      return $("#tab-organizationtypes .organization-types p.selected").data("id");
    }

    function loadOrganizationTypes(selectedId) {
      if (!selectedId) selectedId = getSelectedOrganizationTypeId();
      var types = [];
      $.each(currentTabData.orgs, function () {
        var classes = [];
        if (selectedId === this.OrgTypeId) classes.push("selected");
        types.push('<p' +
          (classes.length ? ' class="' + classes.join(" ") + '"' : '') +
          ' data-id="' +
          this.OrgTypeId +
          '">' +
          this.OrgType +
          ' (' +
          this.Count +
          ')</p>');
      });
      $("#tab-organizationtypes .organization-types").html(types.join(""));
    }

    function onClickAddOrganizationType() {
      if ($(this).hasClass("disabled")) return;
      var $tab = $("#tab-organizationtypes");
      var id = ++highestOrgTypeId;
      currentTabData.orgs.push(
        {
          OrgTypeId: id,
          OrgType: $(".organization-type input", $tab).val(),
          Count: 0
        });
      loadOrganizationTypes(id);
      onOrganizationTypesSelectionChanged(null, id);
      organizationTypesSetData(null);
      onOrganizationTypesDataChange();
    }

    function onClickDeleteOrganizationType() {
      if ($(this).hasClass("disabled")) {
        util.alert("Can only delete empty organization types");
        return;
      }
      var id = getSelectedOrganizationTypeId();
      var temp = [];
      $.each(currentTabData.orgs, function () {
        if (this.OrgTypeId !== id)
          temp.push(this);
      });
      currentTabData.orgs = temp;
      loadOrganizationTypes();
      onOrganizationTypesSelectionChanged(null);
      onOrganizationTypesDataChange();
    }

    function onClickOrganizationTypesCancel() {
      if ($(this).hasClass("disabled")) return;
      initializeTabOrganizationTypes();
    }

    function onClickOrganizationTypesSave() {
      if ($(this).hasClass("disabled")) return;
      var selectedId = getSelectedOrganizationTypeId();
      util.openAjaxDialog("Saving changes...");
      util.ajax({
        url: "/Admin/WebService.asmx/SaveOrganizationTypes",
        data: { data: currentTabData.orgs },

        success: function () {
          util.closeAjaxDialog();
          initializeTabOrganizationTypes(selectedId);
          util.alert("Changes saved");
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not save changes"));
        }
      });
    }

    function onOrganizationTypesDataChange() {
      var $tab = $("#tab-organizationtypes");
      if ($tab.hasClass("change-mode")) {
        var orgType = getSelectedOrganizationType();
        if (orgType) {
          orgType.OrgType = $(".organization-type input", $tab).val();
          loadOrganizationTypes();
        }
      }
      adjustTypesUi();
    }

    function onOrganizationTypesModeChanged(event, toChangeMode) {
      var orgType = toChangeMode ? getSelectedOrganizationType() : null;
      organizationTypesSetData(orgType);
      adjustTypesUi();
    }

    function onOrganizationTypesSelectionChanged(dummy, id) {
      organizationTypesSetData(null);
      $.each(currentTabData.orgs, function () {
        if (this.OrgTypeId === id) {
          organizationTypesSetData(this);
          return false;
        }
      });
      adjustTypesUi();
    }

    function onOrganizationTypesSortStop() {
      var temp = [];
      $("#tab-organizationtypes .organization-types p").each(function () {
        temp.push(getOrganizationTypeById($(this).data("id")));
      });
      currentTabData.orgs = temp;
      adjustTypesUi();
    }

    function organizationTypesSetData(orgType) {
      var $tab = $("#tab-organizationtypes");
      if (!orgType) {
        $(".organization-type input", $tab).val("");
      } else {
        $(".organization-type input", $tab).val(orgType.OrgType);
      }
    }

    //
    // Organization SubTypes
    //

    var highestOrgSubTypeId;

    function initOrganizationSubTypes() {
      var $tab = $("#tab-organizationsubtypes");
      $tab
        .on("modechanged", onOrganizationSubTypesModeChanged)
        .on("change", ".organization-types-filter", onSubTypesOrganizationTypesFilterChanged)
        .on("selectionchanged", ".organization-subtypes", onOrganizationSubTypesSelectionChanged)
        .on("sortstop", ".organization-subtypes", onOrganizationSubTypesSortStop)
        .on("propertychange change click keyup input paste", ".organization-subtype input",
          onOrganizationSubTypesDataChange)
        .on("click", ".add-organization-subtype-button", onClickAddOrganizationSubType)
        .on("click", ".delete-organization-subtype-button", onClickDeleteOrganizationSubType)
        .on("click", ".cancel-button", onClickOrganizationSubTypesCancel)
        .on("click", ".save-button", onClickOrganizationSubTypesSave);
    }

    function initializeTabOrganizationSubTypes(selectedTypeId, selectedSubTypeId) {
      var $tab = $("#tab-organizationsubtypes");
      $tab.addClass("change-mode").removeClass("add-mode");
      $(".disabled", $tab).removeClass("disabled");
      $(".change-button-line .div-button", $tab).addClass("disabled");
      $(".delete-organization-subtype-button", $tab).addClass("disabled");
      $(".add-organization-subtype-button", $tab).addClass("disabled");
      $(".drag-box", $tab).html("");
      $("input[type=text]", $tab).val("");
      $(".organization-subtypes", $tab).addClass("can-select").sortable("option", "disabled", false);
      $(".organization-subtype input", $tab).prop("disabled", true);
      currentTabData = null;
      currentTabInitalJson = null;
      highestOrgSubTypeId = 0;
      util.openAjaxDialog("Loading Organization SubTypes...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadOrganizationSubTypes",

        success: function (result) {
          util.closeAjaxDialog();
          currentTabData = { orgs: result.d };
          currentTabInitalJson = JSON.stringify(currentTabData);
          $.each(currentTabData.orgs, function () {
            $.each(this.SubTypes, function() {
              if (this.OrgSubTypeId > highestOrgSubTypeId)
                highestOrgSubTypeId = this.OrgSubTypeId;
            });
          });

          // load the org types dropdown
          var orgs = [];
          $.each(result.d, function() {
            orgs.push({ Text: this.OrgType, Value: this.OrgTypeId });
          });
          util.populateDropdown($(".organization-types-filter", $tab), orgs, "<Select Organization Type>", "");

          if (selectedTypeId)
            $(".organization-types-filter", $tab).val(selectedTypeId);
          loadOrganizationSubTypes(selectedSubTypeId);
          if (selectedTypeId || selectedSubTypeId)
            onOrganizationSubTypesSelectionChanged(null, selectedSubTypeId);
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load Organization SubTypes"));
        }
      });
    };

    function adjustSubTypesUi() {
      var $tab = $("#tab-organizationsubtypes");
      var isChangeMode = $tab.hasClass("change-mode");
      var selectedTypeId = getSelectedSubTypeOrganizationTypeId();
      var selectedSubType = getSelectedOrganizationSubType();
      var dataEnabled = !!(isChangeMode && selectedSubType || !isChangeMode && selectedTypeId);

      $(".organization-subtypes", $tab)
        .toggleClass("can-select", isChangeMode)
        .toggleClass("disabled", !isChangeMode)
        .sortable("option", "disabled", !isChangeMode);
      $(".delete-organization-subtype-button", $tab)
        .toggleClass("disabled", !isChangeMode || !selectedSubType || selectedSubType.Count !== 0);
      $(".organization-subtype input", $tab).prop("disabled", !dataEnabled);
      if (!dataEnabled) organizationSubTypesSetData(null);
      $(".add-organization-subtype-button", $tab)
        .toggleClass("disabled", isChangeMode || !selectedTypeId ||  !$(".organization-subtype input", $tab).val());

      setTabChanged($tab);
    }

    function getOrganizationSubTypeById(id) {
      var result = null;
      var orgType = getSelectedOrganizationSubTypes();
      if (orgType)
        $.each(orgType.SubTypes, function () {
          if (this.OrgSubTypeId === id) {
            result = this;
            return false;
          }
        });
      return result;
    }

    function getSelectedOrganizationSubType() {
      return getOrganizationSubTypeById(getSelectedOrganizationSubTypeId());
    }

    function getSelectedOrganizationSubTypes() {
      var orgTypeId = getSelectedSubTypeOrganizationTypeId();
      var result = null;
      $.each(currentTabData.orgs, function() {
        if (this.OrgTypeId == orgTypeId) {
          result = this;
          return false;
        }
      });
      return result;
    }

    function getSelectedOrganizationSubTypeId() {
      return $("#tab-organizationsubtypes .organization-subtypes p.selected").data("id");
    }

    function getSelectedSubTypeOrganizationTypeId() {
      return $("#tab-organizationsubtypes .organization-types-filter").val();
    }

    function loadOrganizationSubTypes(selectedSubTypeId) {
      if (!selectedSubTypeId) selectedSubTypeId = getSelectedOrganizationSubTypeId();
      var orgType = getSelectedOrganizationSubTypes();
      var subTypes = [];
      if (orgType)
        $.each(orgType.SubTypes, function () {
          var classes = [];
          if (selectedSubTypeId === this.OrgSubTypeId) classes.push("selected");
          subTypes.push('<p' +
            (classes.length ? ' class="' + classes.join(" ") + '"' : '') +
            ' data-id="' +
            this.OrgSubTypeId +
            '">' +
            this.OrgSubType +
            ' (' +
            this.Count +
            ')</p>');
        });
      $("#tab-organizationsubtypes .organization-subtypes").html(subTypes.join(""));
    }

    function onClickAddOrganizationSubType() {
      if ($(this).hasClass("disabled")) return;
      var $tab = $("#tab-organizationsubtypes");
      var id = ++highestOrgSubTypeId;
      var orgType = getSelectedOrganizationSubTypes();
      orgType.SubTypes.push(
        {
          OrgSubTypeId: id,
          OrgSubType: $(".organization-subtype input", $tab).val(),
          Count: 0
        });
      loadOrganizationSubTypes(id);
      onOrganizationSubTypesSelectionChanged(null, id);
      organizationSubTypesSetData(null);
      onOrganizationSubTypesDataChange();
    }

    function onClickDeleteOrganizationSubType() {
      if ($(this).hasClass("disabled")) {
        util.alert("Can only delete empty organization subtypes");
        return;
      }
      var id = getSelectedOrganizationSubTypeId();
      var orgType = getSelectedOrganizationSubTypes();
      var temp = [];
      $.each(orgType.SubTypes, function () {
        if (this.OrgSubTypeId !== id)
          temp.push(this);
      });
      orgType.SubTypes = temp;
      loadOrganizationSubTypes();
      onOrganizationSubTypesSelectionChanged(null);
      onOrganizationSubTypesDataChange();
    }

    function onClickOrganizationSubTypesCancel() {
      if ($(this).hasClass("disabled")) return;
      initializeTabOrganizationSubTypes();
    }

    function onClickOrganizationSubTypesSave() {
      if ($(this).hasClass("disabled")) return;
      var selectedTypeId = getSelectedSubTypeOrganizationTypeId();
      var selectedSubTypeId = getSelectedOrganizationSubTypeId();
      util.openAjaxDialog("Saving changes...");
      util.ajax({
        url: "/Admin/WebService.asmx/SaveOrganizationSubTypes",
        data: { data: currentTabData.orgs },

        success: function () {
          util.closeAjaxDialog();
          initializeTabOrganizationSubTypes(selectedTypeId, selectedSubTypeId);
          util.alert("Changes saved");
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not save changes"));
        }
      });
    }

    function onOrganizationSubTypesDataChange() {
      var $tab = $("#tab-organizationsubtypes");
      if ($tab.hasClass("change-mode")) {
        var subType = getSelectedOrganizationSubType();
        if (subType) {
          subType.OrgSubType = $(".organization-subtype input", $tab).val();
          loadOrganizationSubTypes();
        }
      }
      adjustSubTypesUi();
    }

    function onOrganizationSubTypesModeChanged() {
      var $tab = $("#tab-organizationsubtypes");
      var isChangeMode = $tab.hasClass("change-mode");
      var subType = isChangeMode ? getSelectedOrganizationSubType() : null;
      organizationSubTypesSetData(subType);
      adjustSubTypesUi();
    }

    function onOrganizationSubTypesSelectionChanged(dummy, id) {
      organizationSubTypesSetData(null);
      var orgType = getSelectedOrganizationSubTypes();
      if (orgType)
      $.each(orgType.SubTypes, function () {
        if (this.OrgSubTypeId == id) {
          organizationSubTypesSetData(this);
          return false;
        }
      });
      adjustSubTypesUi();
    }

    function onOrganizationSubTypesSortStop() {
      var temp = [];
      $("#tab-organizationsubtypes .organization-subtypes p").each(function () {
        temp.push(getOrganizationSubTypeById($(this).data("id")));
      });
      var orgType = getSelectedOrganizationSubTypes();
      orgType.SubTypes = temp;
      adjustSubTypesUi();
    }

    function onSubTypesOrganizationTypesFilterChanged() {
      loadOrganizationSubTypes();
      onOrganizationSubTypesSelectionChanged();
    }

    function organizationSubTypesSetData(subType) {
      var $tab = $("#tab-organizationsubtypes");
      if (!subType) {
        $(".organization-subtype input", $tab).val("");
      } else {
        $(".organization-subtype input", $tab).val(subType.OrgSubType);
      }
    }

    //
    // Email Tags
    //

    var highestEmailTagId;

    function initEmailTags() {
      var $tab = $("#tab-emailtags");
      $tab
        .on("modechanged", onEmailTagsModeChanged)
        .on("change", ".organization-types-filter", onEmailTagsOrganizationTypesFilterChanged)
        .on("selectionchanged", ".email-tags", onEmailTagsSelectionChanged)
        .on("sortstop", ".email-tags", onEmailTagsSortStop)
        .on("propertychange change click keyup input paste", ".email-tag input",
        onEmailTagsDataChange)
        .on("click", ".add-email-tag-button", onClickAddEmailTag)
        .on("click", ".delete-email-tag-button", onClickDeleteEmailTag)
        .on("click", ".cancel-button", onClickEmailTagsCancel)
        .on("click", ".save-button", onClickEmailTagsSave);
    }

    function initializeTabEmailTags(selectedTypeId, selectedEmailTagId) {
      var $tab = $("#tab-emailtags");
      $tab.addClass("change-mode").removeClass("add-mode");
      $(".disabled", $tab).removeClass("disabled");
      $(".change-button-line .div-button", $tab).addClass("disabled");
      $(".delete-email-tag-button", $tab).addClass("disabled");
      $(".add-email-tag-button", $tab).addClass("disabled");
      $(".drag-box", $tab).html("");
      $("input[type=text]", $tab).val("");
      $(".email-tags", $tab).addClass("can-select").sortable("option", "disabled", false);
      $(".email-tag input", $tab).prop("disabled", true);
      currentTabData = null;
      currentTabInitalJson = null;
      highestEmailTagId = 0;
      util.openAjaxDialog("Loading Organization Email Tags...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadOrganizationEmailTags",

        success: function (result) {
          util.closeAjaxDialog();
          currentTabData = { orgs: result.d };
          currentTabInitalJson = JSON.stringify(currentTabData);
          $.each(currentTabData.orgs, function () {
            $.each(this.EmailTags, function () {
              if (this.EmailTagId > highestEmailTagId)
                highestEmailTagId = this.EmailTagId;
            });
          });

          // load the org types dropdown
          var orgs = [];
          $.each(result.d, function () {
            orgs.push({ Text: this.OrgType, Value: this.OrgTypeId });
          });
          util.populateDropdown($(".organization-types-filter", $tab), orgs, "<Select Organization Type>", "");

          if (selectedTypeId)
            $(".organization-types-filter", $tab).val(selectedTypeId);
          loadEmailTags(selectedEmailTagId);
          if (selectedTypeId || selectedEmailTagId)
            onEmailTagsSelectionChanged(null, selectedEmailTagId);
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load Organization Email Tags"));
        }
      });
    };

    function adjustEmailTagsUi() {
      var $tab = $("#tab-emailtags");
      var isChangeMode = $tab.hasClass("change-mode");
      var selectedTypeId = getSelectedEmailTagOrganizationTypeId();
      var selectedEmailTag = getSelectedEmailTag();
      var dataEnabled = !!(isChangeMode && selectedEmailTag || !isChangeMode && selectedTypeId);

      $(".email-tags", $tab)
        .toggleClass("can-select", isChangeMode)
        .toggleClass("disabled", !isChangeMode)
        .sortable("option", "disabled", !isChangeMode);
      $(".delete-email-tag-button", $tab)
        .toggleClass("disabled", !isChangeMode || !selectedEmailTag || selectedEmailTag.Count !== 0);
      $(".email-tag input", $tab).prop("disabled", !dataEnabled);
      if (!dataEnabled) emailTagsSetData(null);
      $(".add-email-tag-button", $tab)
        .toggleClass("disabled", isChangeMode || !selectedTypeId || !$(".email-tag input", $tab).val());

      setTabChanged($tab);
    }

    function getEmailTagById(id) {
      var result = null;
      var orgType = getSelectedEmailTags();
      if (orgType)
        $.each(orgType.EmailTags, function () {
          if (this.EmailTagId === id) {
            result = this;
            return false;
          }
        });
      return result;
    }

    function getSelectedEmailTag() {
      return getEmailTagById(getSelectedEmailTagId());
    }

    function getSelectedEmailTags() {
      var orgTypeId = getSelectedEmailTagOrganizationTypeId();
      var result = null;
      $.each(currentTabData.orgs, function () {
        if (this.OrgTypeId == orgTypeId) {
          result = this;
          return false;
        }
      });
      return result;
    }

    function getSelectedEmailTagId() {
      return $("#tab-emailtags .email-tags p.selected").data("id");
    }

    function getSelectedEmailTagOrganizationTypeId() {
      return $("#tab-emailtags .organization-types-filter").val();
    }

    function loadEmailTags(selectedEmailTagId) {
      if (!selectedEmailTagId) selectedEmailTagId = getSelectedEmailTagId();
      var orgType = getSelectedEmailTags();
      var emailTags = [];
      if (orgType)
        $.each(orgType.EmailTags, function () {
          var classes = [];
          if (selectedEmailTagId === this.EmailTagId) classes.push("selected");
          emailTags.push('<p' +
            (classes.length ? ' class="' + classes.join(" ") + '"' : '') +
            ' data-id="' +
            this.EmailTagId +
            '">' +
            this.EmailTag +
            ' (' +
            this.Count +
            ')</p>');
        });
      $("#tab-emailtags .email-tags").html(emailTags.join(""));
    }

    function onClickAddEmailTag() {
      if ($(this).hasClass("disabled")) return;
      var $tab = $("#tab-emailtags");
      var id = ++highestEmailTagId;
      var orgType = getSelectedEmailTags();
      orgType.EmailTags.push(
        {
          EmailTagId: id,
          EmailTag: $(".email-tag input", $tab).val(),
          Count: 0
        });
      loadEmailTags(id);
      onEmailTagsSelectionChanged(null, id);
      emailTagsSetData(null);
      onEmailTagsDataChange();
    }

    function onClickDeleteEmailTag() {
      if ($(this).hasClass("disabled")) {
        util.alert("Can only delete empty email tags");
        return;
      }
      var id = getSelectedEmailTagId();
      var orgType = getSelectedEmailTags();
      var temp = [];
      $.each(orgType.EmailTags, function () {
        if (this.EmailTagId !== id)
          temp.push(this);
      });
      orgType.EmailTags = temp;
      loadEmailTags();
      onEmailTagsSelectionChanged(null);
      onEmailTagsDataChange();
    }

    function onClickEmailTagsCancel() {
      if ($(this).hasClass("disabled")) return;
      initializeTabEmailTags();
    }

    function onClickEmailTagsSave() {
      if ($(this).hasClass("disabled")) return;
      var selectedTypeId = getSelectedEmailTagOrganizationTypeId();
      var selectedEmailTagId = getSelectedEmailTagId();
      util.openAjaxDialog("Saving changes...");
      util.ajax({
        url: "/Admin/WebService.asmx/SaveOrganizationEmailTags",
        data: { data: currentTabData.orgs },

        success: function () {
          util.closeAjaxDialog();
          initializeTabEmailTags(selectedTypeId, selectedEmailTagId);
          util.alert("Changes saved");
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not save changes"));
        }
      });
    }

    function onEmailTagsDataChange() {
      var $tab = $("#tab-emailtags");
      if ($tab.hasClass("change-mode")) {
        var emailTag = getSelectedEmailTag();
        if (emailTag) {
          emailTag.EmailTag = $(".email-tag input", $tab).val();
          loadEmailTags();
        }
      }
      adjustEmailTagsUi();
    }

    function onEmailTagsModeChanged() {
      var $tab = $("#tab-emailtags");
      var isChangeMode = $tab.hasClass("change-mode");
      var emailTag = isChangeMode ? getSelectedEmailTag() : null;
      emailTagsSetData(emailTag);
      adjustEmailTagsUi();
    }

    function onEmailTagsSelectionChanged(dummy, id) {
      emailTagsSetData(null);
      var orgType = getSelectedEmailTags();
      if (orgType)
        $.each(orgType.EmailTags, function () {
          if (this.EmailTagId == id) {
            emailTagsSetData(this);
            return false;
          }
        });
      adjustEmailTagsUi();
    }

    function onEmailTagsSortStop() {
      var temp = [];
      $("#tab-emailtags .email-tags p").each(function () {
        temp.push(getEmailTagById($(this).data("id")));
      });
      var orgType = getSelectedEmailTags();
      orgType.EmailTags = temp;
      adjustEmailTagsUi();
    }

    function onEmailTagsOrganizationTypesFilterChanged() {
      loadEmailTags();
      onEmailTagsSelectionChanged();
    }

    function emailTagsSetData(emailTag) {
      var $tab = $("#tab-emailtags");
      if (!emailTag) {
        $(".email-tag input", $tab).val("");
      } else {
        $(".email-tag input", $tab).val(emailTag.EmailTag);
      }
    }

    //
    // Report
    //

    var reportOrgTypeId;
    var reportSubTypeId;
    var reportIdeologyId;
    var reportStateCode;
    var reportTagIds;
    var reportSortItem = "Name";
    var reportSortDir = "ASC";

    function initReport() {
      var $tab = $("#tab-report");
      $tab
        .on("change", ".organization-types-filter", onReportOrganizationTypesFilterChanged)
        .on("click", ".get-report-button", function() { refreshReport(false) })
        .on("click", ".organization-scroller .link.edit", function() {
          startOrgTypeId = reportOrgTypeId;
          startOrgId = $(this).closest("tr").data("id");
          $("#main-tabs").tabs("option", "active",
            util.getTabIndex("#main-tabs", "tab-organizations"));
        })
        .on("click", ".organization-scroller .link.notes", function() {
          startOrgTypeId = reportOrgTypeId;
          startOrgId = $(this).closest("tr").data("id");
          $("#main-tabs").tabs("option", "active",
            util.getTabIndex("#main-tabs", "tab-organizationnotes"));
        })
        .on("click", ".organization-scroller .link.delete", function() {
          var $tr = $(this).closest("tr");
          var orgId = $tr.data("id");
          var orgName = $(".name", $tr).text();
          util.confirm("Are you sure you want to delete organization " + orgName + "?", function (button) {
            if (button === "Ok") { 
              util.openAjaxDialog("Deleting organization...");
              util.ajax({
                url: "/Admin/WebService.asmx/DeleteOrganization",

                data: {
                  orgId: orgId
                },

                success: function () {
                  util.closeAjaxDialog();
                  refreshReport(true);
                },

                error: function (result) {
                  util.closeAjaxDialog();
                  util.alert(util.formatAjaxError(result,
                    "Could not delete organization"));
                }
              });
            }
          });
        })
        .on("click", ".organization-scroller th.sorted", function () {
          var $this = $(this);
          var dir = $this.hasClass("asc") ? "desc" : "asc";
          reportSortDir = dir.toUpperCase();

          function compareNames(a, b) {
            reportSortItem = "Name";
            if (dir === "asc")
              return $(".name", a).text().localeCompare($(".name", b).text());
            else
              return $(".name", b).text().localeCompare($(".name", a).text());
          }

          function compareStates(a, b) {
            var result;
            reportSortItem = "StateCode";
            if (dir === "asc")
              result = $(".state", a).text().localeCompare($(".state", b).text());
            else
              result = $(".state", b).text().localeCompare($(".state", a).text());
            return result ? result : compareNames(a, b);
          }

          function compareDates(a, b) {
            reportSortItem = "DateStamp";
            var momenta = moment($(".time-stamp", a).data("data"));
            var momentb = moment($(".time-stamp", b).data("data"));
            if (dir === "asc")
              if (momenta.isBefore(momentb)) return -1;
              else if (momentb.isBefore(momenta)) return 1;
              else return compareNames(a, b);
            else
              if (momenta.isBefore(momentb)) return 1;
              else if (momentb.isBefore(momenta)) return -1;
              else return compareNames(a, b);
          }

          var compareFn;
          if ($this.hasClass("name")) {
            compareFn = compareNames;
          }
          else if ($this.hasClass("state"))
            compareFn = compareStates;
          else compareFn = compareDates;
          var $tbody = $(".organization-scroller tbody", $tab);
          $tbody.find("tr").sort(function (a, b) {
            return compareFn(a, b);
          }).appendTo($tbody);
          $(this).closest("tr").find("th").removeClass("asc desc");
          $this.addClass(dir);
        });
    }

    function initializeTabReport() {
      var $tab = $("#tab-report");
      currentTabData = null;
      util.openAjaxDialog("Initializing...");
      util.ajax({
        url: "/Admin/WebService.asmx/LoadOrganizationsSelectReportData",

        success: function (result) {
          util.closeAjaxDialog();
          currentTabData = { orgs: result.d.OrgTypes };

          // load the ideologies dropdown
          var ideologies = [];
          $.each(result.d.Ideologies, function () {
            ideologies.push({ Text: this.Ideology, Value: this.IdeologyId });
          });
          util.populateDropdown($(".report-ideology select", $tab), ideologies, "All Ideologies", "0", reportIdeologyId);

          // load the org types dropdown
          var orgs = [];
          $.each(result.d.OrgTypes, function () {
            orgs.push({ Text: this.OrgType, Value: this.OrgTypeId });
          });
          util.populateDropdown($(".organization-types-filter", $tab), orgs, "<Select Type>", "", reportOrgTypeId);

          //if (selectedTypeId)
          //  $(".organization-types-filter", $tab).val(selectedTypeId);
          onReportOrganizationTypesFilterChanged(null);
          if (reportOrgTypeId) // not first time
            refreshReport(true);
        },

        error: function (result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could not load report info"));
        }
      });
    };


    function onReportOrganizationTypesFilterChanged() {
      var $tab = $("#tab-report");
      // load subtypes dropdown
      var items = [{ Text: "All SubTypes", Value: "0" }];
      var orgType = getSelectedOrgs($tab);
      if (orgType)
        $.each(orgType.SubTypes, function () {
          items.push({ Text: this.OrgSubType, Value: this.OrgSubTypeId });
        });
      util.populateDropdown($(".report-subtype select", $tab), items, null, null, reportSubTypeId);

      // create email tags checkboxes
      var $checkboxes = $(".report-emailtags .checkboxes", $tab);
      if (orgType && orgType.EmailTags.length) {
        var checkboxes = [];
        $.each(orgType.EmailTags, function () {
          var checked = reportTagIds && $.inArray(reportOrgTypeId, reportTagIds) >= 0
            ? ' checked="checked"'
            : '';
          checkboxes.push('<div><input' + checked + ' type="checkbox" value="' + reportOrgTypeId + '"/>' + this.EmailTag + '</div>');
        });
        $checkboxes.html(checkboxes.join(""));
      } else if (orgType) {
        $checkboxes.html("<p>No email tags for this organization type</p>");
      } else {
        $checkboxes.html("");
      }
    }

    function loadReport(data) {
      var $tab = $("#tab-report");
      var $scroller = $(".organization-scroller", $tab);
      var $foundCount = $(".found-count", $tab);
      //$scroller.html("");

      if (!data) {
        $scroller.addClass("hidden");
        $foundCount.addClass("hidden");
        return;
      }

      $scroller.removeClass("hidden");
      $foundCount.removeClass("hidden");
      if (data.length === 0) {
        $scroller.html('<table><tbody><tr><td>No matching organizations found.</td></tr></tbody></table>');
        return;
      }

      var dirClass = " " + reportSortDir.toLowerCase();
      var head = '<tr>' +
        '<th class="name sorted' + (reportSortItem == "Name" ? dirClass : "") + '">Name</th>' +
        '<th class="state sorted' + (reportSortItem == "StateCode" ? dirClass : "") + '">State</th>' +
        '<th class="time-stamp sorted' + (reportSortItem == "DateStamp" ? dirClass : "") + '">Update Date</th>' +
        '<th>&nbsp;</th>' +
        '<th>&nbsp;</th>' +
        '<th>&nbsp;</th>' +
        '</tr>';

      var rows = [];
      $.each(data, function () {
        var name = this.Name;
        if (this.OrgAbbreviation) name += " (" + this.OrgAbbreviation + ")";
        if (this.Url)
          name = '<a href="' + this.Url + '" target="ext">' + name + '</a>';
        rows.push('<tr data-id="' + this.OrgId + '">' +
          '<td class="name">' + name + '</td>' +
          '<td class="state">' + this.StateCode + '</td>' +
          '<td class="time-stamp" data-data="' + this.DateStamp + '">' + moment(this.DateStamp).format("M/D/YY") + '</td>' +
          '<td class="link edit">edit</td>' +
          '<td class="link delete">delete</td>' +
          '<td class="link notes">notes</td>' +
          '</tr>');
      });

      $("span", $foundCount).text(data.length);
      $scroller.html('<table><thead>' + head + '</thead><tbody>' + rows.join("") + '</tbody></table>');
    }

    function refreshReport(silent) {
      var $tab = $("#tab-report");
      loadReport();
      var orgTypeId = $(".report-type select", $tab).val();
      var subTypeId = $(".report-subtype select", $tab).val();
      var ideologyId = $(".report-ideology select", $tab).val();
      var stateCode = $(".report-state select", $tab).val();
      var tagIds = [];
      $(".report-emailtags input", $tab).each(function() {
        var $this = $(this);
        if ($this.prop("checked"))
          tagIds.push($this.val());
      });
      if (!orgTypeId) {
        if (!silent) util.alert("Please select an Organization Type");
        return;
      }

      if (!silent) util.openAjaxDialog("Getting report...");
      util.ajax({
        url: "/Admin/WebService.asmx/GetOrganizationsReportData",
        data: {
          orgTypeId: orgTypeId,
          subTypeId: subTypeId,
          ideologyId: ideologyId,
          stateCode: stateCode,
          tagIds: tagIds,
          sortItem: reportSortItem,
          sortDir: reportSortDir
        },

        success: function (result) {
          reportOrgTypeId = orgTypeId;
          reportSubTypeId = subTypeId;
          reportIdeologyId = ideologyId;
          reportStateCode = stateCode;
          reportTagIds = tagIds;
          loadReport(result.d);
        },

        error: function (result) {
          if (!silent) util.alert(util.formatAjaxError(result, "Could not get report"));
        },

        complete: function () {
          if (!silent) util.closeAjaxDialog();
        }
      });
    }

    //
    // Miscellaneous
    //

    function isTabChanged() {
      return currentTabData != null &&
        currentTabInitalJson != JSON.stringify(currentTabData);
    }

    function setTabChanged($tab) {
      var changed = isTabChanged();
      $tab.toggleClass("has-changes", changed);
      $(".enable-if-changed", $tab).toggleClass("disabled", !changed);
    }

    function tabActivated(id) {
      switch (id) {
      case "tab-organizations":
        initializeTabOrganizations();
        break;

      case "tab-organizationnotes":
        initializeTabNotes();
        break;

      case "tab-organizationlogo":
        initializeTabLogo();
        break;

      case "tab-organizationad":
        initializeTabAd();
        break;

      case "tab-organizationtypes":
        initializeTabOrganizationTypes();
        break;

      case "tab-organizationsubtypes":
        initializeTabOrganizationSubTypes();
        break;

      case "tab-emailtags":
        initializeTabEmailTags();
        break;

      case "tab-report":
        initializeTabReport();
        break;
      }
    }

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });