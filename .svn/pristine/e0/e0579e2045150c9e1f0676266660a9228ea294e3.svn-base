define(["jquery", "vote/adminMaster", "vote/util", "monitor"],
function ($, master, util, monitor) {

  function initPage() {
    monitor.init();
    initAddUser();
    $("#tab-adduser .select-user-security").change(function () {
      var level = $("#tab-adduser .select-user-security").val();
      $("#tab-adduser .input-element.user-state-code").toggleClass("hidden", !level || level === "MASTER");
      $("#tab-adduser .input-element.user-county-code").toggleClass("hidden", level !== "COUNTY" && level !== "LOCAL");
      $("#tab-adduser .input-element.user-local-code").toggleClass("hidden", level !== "LOCAL");
    });
    $("#tab-adduser .select-user-state-code").change(populateAddUsersCountyDropdown);
    $("#tab-adduser .select-user-county-code").change(populateAddUsersLocalDropdown);
    $("input,select", $("#tab-adduser")).on("propertychange change click keyup input paste", addUserDataChanged);
    $("#tab-adduser .add-user-button").click(onAddUserButtonClick);
    $("#tab-changeuser .user-scroller")
      .on("click", "td.change-password", onChangePassword)
      .on("click", "td.delete", onDeleteUser);
    $("#main-tabs").on("tabsactivate", onTabsActivate);

    $('#change-password-dialog').dialog({
      autoOpen: false,
      width: 250,
      height: "auto",
      resizable: false,
      dialogClass: 'change-password-dialog overlay-dialog',
      title: "Change User Password",
      modal: true
    });
    $("#change-password-dialog .change-password-button").click(onClickChangePasswordButton);
  }
  
  function addUserDataChanged() {
    var name = $.trim($("#tab-adduser .textbox-user-name").val());
    var password = $.trim($("#tab-adduser .textbox-user-password").val());
    var level = $("#tab-adduser .select-user-security").val();
    var stateCode = $("#tab-adduser .select-user-state-code").val();
    var countyCode = $("#tab-adduser .select-user-county-code").val();
    var localCode = $("#tab-adduser .select-user-local-code").val();
    var disabled = !name || !password || !level;
    switch (level) {
      case "LOCAL":
        disabled = disabled || !localCode;
        // drop thru

      case "COUNTY":
        disabled = disabled || !countyCode;
        // drop thru

      case "ADMIN":
        disabled = disabled || !stateCode;
        break;
    }
    $("#tab-adduser .add-user-button").toggleClass("disabled", disabled);
  }

  function changePassword($tr, force) {
    var userName = $("td.user-name", $tr).text();
    var oldPassword = $("td.password", $tr).text();
    var $dialog = $("#change-password-dialog");
    $(".change-password-dialog-user-name", $dialog).val(userName);
    $(".change-password-dialog-old-password", $dialog).val(oldPassword);
    $dialog.dialog("open");
  }

  function deleteUser($tr, force) {
    var userName = $("td.user-name", $tr).text();
    if (!force) {
      util.confirm("Are you sure you want to delete user " + userName + "?", function (button) {
        if (button === "Ok")
          deleteUser($tr, true);
      });
      return;
    }
    util.openAjaxDialog("Deleting user...");
    util.ajax({
      url: "/Admin/WebService.asmx/DeleteUser",
      data: {
        userName: userName
      },

      success: function (result) {
        util.alert("User " + userName + " deleted.");
        util.closeAjaxDialog();
        loadUsers();
      },

      error: function (result) {
        util.alert(util.formatAjaxError(result, "Could not delete user"));
        util.closeAjaxDialog();
      }
    });
  }
  
  function loadUsers() {
    var $scroller = $("#tab-changeuser .user-scroller");
    $scroller.html("");
    util.openAjaxDialog("Getting users...");
    util.ajax({
      url: "/Admin/WebService.asmx/GetUserData",

      success: function (result) {
        var rows = [];
        $.each(result.d, function () {
          rows.push('<tr><td>' + this.UserLevel + '</td>' +
            '<td class="user-name">' + this.UserName + '</td>' +
            '<td class="password">' + this.Password + '</td>' +
            '<td>' + this.StateCode + '</td>' +
            '<td>' + this.CountyCode + '</td>' +
            '<td>' + this.LocalCode + '</td>' +
            '<td class="link change-password">Change Password</td>' +
            '<td class="link delete">Delete</td></tr>');
        });
        $scroller.html('<table>' +
            '<thead><tr><th>Level</th><th>User Name</th><th>Password</th><th>State Code</th>' +
            '<th>County Code</th><th>Local Code</th><th></th><th></th></tr></thead>' +
            '<tbody>' + rows.join("") + '</tbody>' +
            '</table>');
        util.assignRotatingClassesToChildren($("tbody", $scroller), ["odd", "even"]);
        util.closeAjaxDialog();
      },

      error: function (result) {
        util.alert(util.formatAjaxError(result, "Could not get users"));
        util.closeAjaxDialog();
      }
    });
  }
  
  function onAddUserButtonClick() {
    var userName = $.trim($("#tab-adduser .textbox-user-name").val());
    var password = $.trim($("#tab-adduser .textbox-user-password").val());
    var level = $("#tab-adduser .select-user-security").val();
    var stateCode = $("#tab-adduser .select-user-state-code").val();
    var countyCode = $("#tab-adduser .select-user-county-code").val();
    var localCode = $("#tab-adduser .select-user-local-code").val();
    util.openAjaxDialog("Adding user...");
    util.ajax({
      url: "/Admin/WebService.asmx/AddUser",
      data: {
        userName: userName,
        password: password,
        level: level,
        stateCode: stateCode,
        countyCode: countyCode,
        localCode: localCode
      },

      success: function (result) {
        if (result.d.substr(0, 1) == "*") {
          util.alert(result.d.substr(1));
        } else {
          util.alert("User " + userName + " added.");
          initAddUser();
        }
        util.closeAjaxDialog();
      },

      error: function (result) {
        util.alert(util.formatAjaxError(result, "Could not add user"));
        util.closeAjaxDialog();
      }
    });
  }

  function onChangePassword() {
    changePassword($(this).closest("tr"));
  }
  
  function onClickChangePasswordButton() {
    var $dialog = $("#change-password-dialog");
    var userName = $(".change-password-dialog-user-name", $dialog).val();
    var password = $.trim($(".change-password-dialog-new-password", $dialog).val());
    if (!password) {
      util.alert("The New Password is required.");
      return;
    }
    util.openAjaxDialog("Changing password...");
    util.ajax({
      url: "/Admin/WebService.asmx/ChangeUserPassword",
      data: {
        userName: userName,
        password: password
      },

      success: function (result) {
        $dialog.dialog("close");
        util.alert("Password changed for " + userName + ".");
        util.closeAjaxDialog();
        loadUsers();
      },

      error: function (result) {
        util.alert(util.formatAjaxError(result, "Could not change password"));
        util.closeAjaxDialog();
      }
    });
  }

  function onDeleteUser() {
    deleteUser($(this).closest("tr"));
  }
  
  function onTabsActivate(event, ui) {
    if (ui.newPanel[0].id == "tab-changeuser") 
      loadUsers();
  }

  function populateAddUsersCountyDropdown() {
    var $dropdown = $("#tab-adduser .select-user-county-code");
    var stateCode = $("#tab-adduser .select-user-state-code").val();
    if (!stateCode) {
      $dropdown.html("");
    }
    util.openAjaxDialog("Getting counties...");
    util.ajax({
      url: "/Admin/WebService.asmx/GetCounties",
      data: {
        stateCode: stateCode
      },

      success: function (result) {
        util.populateDropdown($dropdown, result.d, '<Select County>', '');
        util.closeAjaxDialog();
      },

      error: function (result) {
        util.alert(util.formatAjaxError(result, "Could not get counties"));
        util.closeAjaxDialog();
      }
    });
  }

  function populateAddUsersLocalDropdown() {
    var $dropdown = $("#tab-adduser .select-user-local-code");
    var stateCode = $("#tab-adduser .select-user-state-code").val();
    var countyCode = $("#tab-adduser .select-user-county-code").val();
    if (!stateCode || !countyCode) {
      $dropdown.html("");
    }
    util.openAjaxDialog("Getting local districts...");
    util.ajax({
      url: "/Admin/WebService.asmx/GetLocals",
      data: {
        stateCode: stateCode,
        countyCode: countyCode
      },

      success: function (result) {
        util.populateDropdown($dropdown, result.d, '<Select Local District>', '');
        util.closeAjaxDialog();
      },

      error: function (result) {
        util.alert(util.formatAjaxError(result, "Could not get local districts"));
        util.closeAjaxDialog();
      }
    });
  }
  
  function initAddUser() {
    $("#tab-adduser .textbox-user-name").val("");
    $("#tab-adduser .textbox-user-password").val("");
    $("#tab-adduser .select-user-security").val("");
    $("#tab-adduser .select-user-state-code").val("");
    $("#tab-adduser .select-user-county-code").val("");
    $("#tab-adduser .select-user-local-code").val("");
    $("#tab-adduser .input-element.user-state-code").addClass("hidden");
    $("#tab-adduser .input-element.user-county-code").addClass("hidden");
    $("#tab-adduser .input-element.user-local-code").addClass("hidden");
    $("#tab-adduser .add-user-button").addClass("disabled");
  }
  
  master.inititializePage({
    callback: initPage
  });
});