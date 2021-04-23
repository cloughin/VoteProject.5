define(["jquery", "vote/adminMaster", "vote/util", "monitor", "moment"],
function ($, master, util, monitor, moment) {

  var afterUpdateContainer = function (group) {
    if (!group) return;

    switch (group.group) {
      case "mc-group-editvolunteer":
        if ($('#edit-volunteer-dialog .refresh-report').val() === "Y")
          refreshReport(true);
        // unless there was an error, update volunteer-to-edit in case email changed
        var $newEmail = $('#edit-volunteer-dialog .email input');
        if (!$newEmail.hasClass("error"))
          $('.volunteer-to-edit').val($.trim($newEmail.val()));
        break;
    }
  };

  function initPage() {
    monitor.registerCallback("afterUpdateContainer", afterUpdateContainer);
    monitor.init();

    $("#tab-report .get-report-button").click(function () { refreshReport(false); });

    $("#tab-report").on("click", ".edit", function () {
      var email = getClickedEmail($(this));
      $('.volunteer-to-edit').val(email);
      $('#edit-volunteer-dialog').dialog('open');
    });

    $("#tab-report").on("click", ".delete", deleteVolunteer);

    $("#tab-report").on("click", ".notes", function () {
      volunteerNotesKey = getClickedEmail($(this));
      $('#volunteer-notes-dialog').dialog('open');
    });

    $('#edit-volunteer-dialog').dialog({
      autoOpen: false,
      width: 880,
      resizable: false,
      dialogClass: 'edit-volunteer overlay-dialog',
      open: onOpenEditVolunteerDialog,
      close: function () {
        master.onCloseJqDialog();
      },
      modal: true
    })
    .on("click", ".send-email-from-edit", function () {
      var email = $.trim($('#edit-volunteer-dialog .edit-email').val());
      var $this = $(this);
      $this.attr("href", "mailto:" + email);
      // clear the address
      setTimeout(function () {
        $this.removeAttr("href");
      }, 200);
    });

    var $notesDialog = $('#volunteer-notes-dialog');

    $notesDialog.dialog({
      autoOpen: false,
      width: 600,
      resizable: false,
      title: "Volunteer Notes",
      dialogClass: 'volunteer-notes overlay-dialog',
      open: onOpenVolunteerNotesDialog,
      close: function () {
        master.onCloseJqDialog();
      },
      modal: true
    });
    $(".add-button input", $notesDialog).click(function() {
      if (editingNote) {
        util.alert("A note is currently being edited. Please save or cancel before adding another note.");
        return;
      }
      util.openAjaxDialog("Adding note...");
      util.ajax({
        url: "/Admin/WebService.asmx/AddVolunteerNote",
        data: {
          email: volunteerNotesKey,
          note: $(".new-note textarea", $notesDialog).val()
        },

        success: function (result) {
          displayNotes(result.d);
          $(".new-note textarea", $notesDialog).val("").click();
        },

        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not add note"));
        },

        complete: function () {
          util.closeAjaxDialog();
        }
      });
    });
    $('.new-note textarea', $notesDialog).on("propertychange change click keyup input paste",
      function () {
        $(".add-button input", $notesDialog).prop("disabled", !$.trim($(this).val()));
      });
    $notesDialog
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
            url: "/Admin/WebService.asmx/DeleteVolunteerNote",
            data: {
              email: volunteerNotesKey,
              id: id
            },

            success: function (result) {
              displayNotes(result.d);
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
        url: "/Admin/WebService.asmx/SaveVolunteerNote",
        data: {
          email: volunteerNotesKey,
          id: id,
          note: $body.html()
        },

        success: function (result) {
          displayNotes(result.d);
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
    });
  }

  function getClickedEmail($target) {
    return $target.closest("tr").find(".email").text();
  }

  function deleteVolunteer(event, force) {
    var email = getClickedEmail($(event.target));
    if (email) {
      if (!force) {
        util.confirm("Are you sure you want to delete the volunteer with email address " + email + "?",
          function (button) {
            if (button === "Ok") {
              deleteVolunteer(event, true);
            }
          });
        return;
      }

      util.openAjaxDialog("Deleting volunteer...");
      util.ajax({
        url: "/Admin/WebService.asmx/DeleteVolunteer",
        data: {
          email: email
        },

        success: function () {
          util.alert("Volunter deleted.");
          refreshReport(true);
        },

        error: function (result) {
          util.alert(util.formatAjaxError(result, "Could not delete volunteer"));
        },

        complete: function () {
          util.closeAjaxDialog();
        }
      });
    }
  }

  function onOpenEditVolunteerDialog() {
    var $dialog = $("#edit-volunteer-dialog");
    $(".content-area", $dialog).css("visibility", "hidden");
    monitor.clearGroupFeedback("mc-group-editvolunteer");
    master.onOpenJqDialog.apply(this);
    $('.reloading', $dialog).val("reloading");
    $('input.update-button', $dialog).addClass("reloading").click();
  }

  var volunteerNotesKey;
  var editingNote;
  var editingContent;

  function displayNotes(notes) {
    if (notes.length)
    {
      var n = [];
      $.each(notes, function () {
        n.push('<div class="note-header" data-id="' + this.Id + '"><span class="date">' + 
          moment(this.DateStamp).format("MM/DD/YY hh:mm:ssa") + '</span>' +
          '<span class="edit link">edit</span>' +
          '<span class="delete link">delete</span>' +
          '<span class="save link hidden">save</span>' +
          '<span class="cancel link hidden">cancel</span>' +
          '</div>');
        n.push('<div class="note-body" contentEditable="false">' + this.Notes + '</div>');
      });
      $("#volunteer-notes-dialog .notes").html(n.join(""));
    } else {
      $("#volunteer-notes-dialog .notes").text("No notes.");
    }
    editingNote = false;
  }

  function onOpenVolunteerNotesDialog() {
    var $dialog = $("#volunteer-notes-dialog");
    $(".content-area", $dialog).css("visibility", "hidden");
    master.onOpenJqDialog.apply(this);

    util.openAjaxDialog("Getting volunteer notes...");
    util.ajax({
      url: "/Admin/WebService.asmx/GetVolunteerNotes",
      data: {
        email: volunteerNotesKey
      },

      success: function (result) {
        var o = result.d;
        if (o) {
          $(".info-name .value", $dialog).text(o.FirstName + " " + o.LastName);
          $(".info-email .value", $dialog).text(o.Email).attr("href", "mailto:" + o.Email);
          $(".info-state .value", $dialog).text(o.StateName);
          $(".info-party .value", $dialog).text(o.PartyName);
          $(".new-note textarea", $dialog).click();
          $(".content-area", $dialog).css("visibility", "visible");
          displayNotes(o.Notes);
        }
      },

      error: function (result) {
        util.alert(util.formatAjaxError(result, "Could not get volunteer notes"));
      },

      complete: function () {
        util.closeAjaxDialog();
      }
    });

  }

  function loadReport(data) {
    var $scroller = $("#tab-report .volunteer-scroller");
    $scroller.html("");

    if (!data) {
      $scroller.addClass("hidden");
      return;
    }

    $scroller.removeClass("hidden");
    if (data.length === 0) {
      $scroller.html('<table><tbody><tr><td>No matching volunteers found.</td></tr></tbody></table>');
      return;
    }

    var head = '<tr>' +
        '<th>State</th>' +
        '<th>Party</th>' +
        '<th>Email Address</th>' +
        '<th>Phone</th>' +
        '<th>Name</th>' +
        '<th>Password</th>' +
        '<th>Added</th>' +
        '<th>&nbsp;</th>' +
        '<th>&nbsp;</th>' +
        '<th>&nbsp;</th>' +
        '</tr>';

    var rows = [];
    $.each(data, function () {
      var name = this.FirstName || "";
      if (name) name += " ";
      name += this.LastName;
      var date = moment(this.DateStamp);
      date = date.year() < 2000 ? "&nbsp;" : date.format("M/D/YY");
      rows.push('<tr>' +
        '<td>' + this.StateCode + '</td>' +
        '<td>' + this.PartyName + '</td>' +
        '<td class="email"><a href="mailto:' + this.Email + '">' + this.Email + '</a></td>' +
        '<td>' + (this.Phone || '&nbsp;') + '</td>' +
        '<td>' + name + '</td>' +
        '<td>' + (this.Password || '&nbsp;') + '</td>' +
        '<td>' + date + '</td>' +
        '<td class="link edit">edit</td>' +
        '<td class="link delete">delete</td>' +
        '<td class="link notes">notes</td>' +
        '</tr>');
    });

    $scroller.html('<table><thead>' + head + '</thead><tbody>' + rows.join("") + '</tbody></table>');
    util.assignRotatingClassesToChildren($("tbody", $scroller), ["odd", "even"]);
  }

  function refreshReport(silent) {
    loadReport();
    var stateCode = $("#tab-report .select-state").val();
    var partyKey = $("#tab-report .select-party").val();
    if (stateCode !== "" && stateCode != "*ALL*" && !partyKey) {
      if (!silent) util.alert("Please select a Party");
      return;
    }

    if (!silent) util.openAjaxDialog("Getting report...");
    util.ajax({
      url: "/Admin/WebService.asmx/GetVolunteerReport",
      data: {
        stateCode: $("#tab-report .select-state").val(),
        partyKey: $("#tab-report .select-party").val()
      },

      success: function (result) {
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

  master.inititializePage({
    callback: initPage
  });
});