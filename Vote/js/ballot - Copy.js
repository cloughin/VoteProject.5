$(function () {

  var shareWasAbandoned = true;

  function enableChange(state) {
    $(".office-cells a.candidate-checkbox").toggleClass("disabled", !state);
    $(".office-cells input.write-in-text").prop("disabled", !state);
    $(".referendum-content a.referendum-checkbox").toggleClass("disabled", !state);
    PUBLIC.setCanUpdateBallot(state);
  }

  function loadMyChoicesAndFriends() {
    PUBLIC.loadMyBallotChoices();
    enableChange(true);
    setUpFriends();
  }

  function setUpChecks() {
    PUBLIC.initBallotCheckBoxes();
    PUBLIC.initBallotEvents();


    $('#share-ballot-dialog').dialog({
      autoOpen: false,
      width: "auto",
      height: "auto",
      resizable: false,
      title: "Share Your Ballot Choices with Friends",
      modal: true,
      dialogClass: "share-ballot-dialog",
      create: function () {
        $(this).css("maxWidth", "300px");
      },
      open: onOpenJqDialog,
      close: function() {
        onCloseJqDialog();
        if (shareWasAbandoned) {
          $.ajax({
            type: "POST",
            url: "/WebService.asmx/BallotShareAbandoned",
            contentType: "application/json; charset=utf-8",
            dataType: "json"      
            });
          }
        }
    });


    // alert when clicking on another's choice
    $(".office-cells,.referendum-content").on("click", "a.disabled", function() {
      UTIL.alert("You cannot change a friend's choices.\nClick 'Show My Choices' or 'Cancel'.", 
        ["Show My Choices", "Cancel"],
        function(button) {
          if (button === "Show My Choices") {
            loadMyChoicesAndFriends();
          }
        });
    });

    var friend = $.trim(decodeURIComponent($.queryString("friend") || ""));
    var choices = $.trim(decodeURIComponent($.queryString("choices") || ""));
    if (friend && choices) {
      // process friend
      UTIL.openModal();
      var data = { choices: choices };
      $.ajax({
        type: "POST",
        url: "/WebService.asmx/DecodeBallotChoices",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (result) {
          var decoded = result.d;
          if (decoded == null) {
            loadMyChoicesAndFriends();
            return;
          }
          saveFriend(friend, decoded);
          // redirect without choices parameter
          var queries = location.search.substr(1).split("&");
          var q = [];
          $.each(queries, function() {
            var p = this.toString();
            if (p.substr(0, 8) != "choices=")
              q.push(p);
          });
          location.href = location.protocol + "//" + location.host + location.pathname + "?" +
            q.join("&");
        },

        error: function () {
          loadMyChoicesAndFriends();
        },

        complete: function () {
          UTIL.closeModal();
        }
      });
    } else {
      loadMyChoicesAndFriends();
      if (friend)
        selectFriend(friend);
    }
    
    initFriendEvents();
  }
  
  function initFriendEvents() {

    $(".friends-dropdown").on("change", function () {
      var friend = $(this).val();
      if (friend) selectFriend(friend);
      else loadMyChoicesAndFriends();
    });

    $(".make-my-choices-button").on("click", function () {

      function doMakeMyChoices(force) {
        var friend = $(".friends-dropdown").val();
        if (!force) {
          UTIL.confirm("Ok to replace my choices with the choices of my friend " + friend + "?", 
            function(button) {
              if (button === "Ok") doMakeMyChoices(true);
          });
          return;
        }
        var friendChoices = getFriendsData()[friend] || {};
        var electionKey = $("body").data("election");
        $(".office-cell").each(function() {
          var $office = $(this);
          var officeKey = $office.data("key");
          var key = electionKey + "." + officeKey;
          var choices = friendChoices[officeKey] || {};
          if ($.isEmptyObject(choices)) window.localStorage.removeItem(key);
          else window.localStorage.setItem(key, JSON.stringify(choices));
        });
        loadMyChoicesAndFriends();
      }

      doMakeMyChoices();
    });

    $(".delete-choices-button").on("click", function () {
      
      function doDelete(force) {
        var friend = $(".friends-dropdown").val();
        if (!force) {
          UTIL.confirm("Ok to delete choices for friend " + friend + "?", function(button) {
            if (button === "Ok") doDelete(true);
          });
          return;
        }
        var friends = getFriendsData();
        delete friends[friend];
        window.localStorage.setItem(getFriendsKey(), JSON.stringify(friends));
        loadMyChoicesAndFriends();
      }

      doDelete();

    });

    $(".share-choices-button").click(function () {
      // gather the choices
      var checks = PUBLIC.getAllBallotChecks();
      if ($.isEmptyObject(checks)) {
        UTIL.alert("Please check at least one candidate to enable sharing.");
        return;
      }
      $('#share-ballot-dialog').dialog('open');
    });

    $(".share-choices.fbshare2 a").click(function(event) {
      event.preventDefault();
      // gather the choices
      var checks = PUBLIC.getAllBallotChecks();
      if ($.isEmptyObject(checks)) {
        UTIL.alert("Please check at least one candidate to enable sharing.");
        return;
      }
      var popW = 626, popH = 436;
      var leftPos = ($(window).width() - popW) / 2 + window.screenX;
      var topPos = ($(window).height() - popH) / 2 + window.screenY;
      window.open("", 'fbshare', 'width=' + popW + ',height=' + popH + ',top=' + topPos + ',left=' + leftPos).focus();
      var $form = $($('<form style="display:none" method="post" action="/FacebookShare.aspx" target="fbshare">' +
        '<input type="hidden" name="url"><input type="hidden" name="choices"></form>'));
      $form.appendTo("body");
      $form.find("input[name=url]").val(window.location.toString());
      //logDebug("fbshare", window.location.toString() + "||" + $form.find("input[name=url]").val());
      $form.find("input[name=choices]").val(JSON.stringify(checks));
      $form.submit();
    });

    $(".share-ballot-button").click(function() {
      var name = $.trim($(".share-ballot-name").val());
      var email = $.trim($(".share-ballot-email").val());
      if (!name) {
        UTIL.alert("Your name is required.", function () {
          $('.share-ballot-name').focus();
        });
        return;
      }
      if (!email) {
        UTIL.alert("Your email address is required.", function () {
          $('.share-ballot-email').focus();
        });
        return;
      }
      if (!UTIL.isValidEmail(email)) {
        UTIL.alert("Please enter a valid email address.", function () {
          $('.share-ballot-email').focus();
        });
        return;
      }

      var choices = PUBLIC.getAllBallotChecks();
      var data = {
        name: name,
        email: email,
        url: window.location.toString(),
        choices: choices
      };
      $.ajax({
        type: "POST",
        url: "/WebService.asmx/SendBallotChoices",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
          
        success: function() {
          UTIL.alert("An email was sent to " + email);
        },
          
        error: function() {
          UTIL.alert(UTIL.formatAjaxError(result, "Could not send email"));
        },
        
        complete: function() {
          shareWasAbandoned = false;
          $('#share-ballot-dialog').dialog('close');
          shareWasAbandoned = true;
        }
      });
    });

  }
  
  function getFriendsKey() {
    return $("body").data("election") + "|friends";
  }
  
  function getFriendsData() {
    var friendsStr = window.localStorage.getItem(getFriendsKey());
    return friendsStr ? JSON.parse(friendsStr) : {};
  }
  
  function saveFriend(friend, choices) {
    if (PUBLIC.localStorageIsAvailable()) {
      var friends = getFriendsData();
      friends[friend] = choices;
      window.localStorage.setItem(getFriendsKey(), JSON.stringify(friends));
    }
  }
  
  function selectFriend(friend) {
    $(".friends-dropdown").val(friend).addClass("friend");
    $(".make-my-choices-button").prop("disabled", false);
    $(".delete-choices-button").prop("disabled", false);
    var friendChoices = getFriendsData()[friend] || {};
    $(".office-cell").each(function () {
      var $this = $(this);
      var choices = friendChoices[$this.data("key")] || {};
      PUBLIC.updateOfficeChecks($this, choices);
    });
    $(".referendum-content").each(function () {
      var $this = $(this);
      var choices = friendChoices[$this.data("key")];
      PUBLIC.updateBallotMeasureChecks($this, choices);
    });
    enableChange(false);
  }
  
  function setUpFriends() {
    if (PUBLIC.localStorageIsAvailable()) {
      var key = $("body").data("election") + "|friends";
      var friendsStr = window.localStorage.getItem(key);
      var friends = friendsStr ? JSON.parse(friendsStr) : {};
      var friendsList = [];
      for (var friend in friends)
        friendsList.push(friend);
      if (friendsList.length) {
        friendsList.sort(function(a, b) {
          if (a.toLowerCase() > b.toLowerCase()) return 1;
          if (a.toLowerCase() < b.toLowerCase()) return -1;
          return 0;
        });
        var options = ['<option value="">My choices</option>'];
        $.each(friendsList, function () {
          var text = UTIL.htmlEscape(this.toString() || "");
          options.push('<option value="' + text + '">' + text + '</option>');
        });
        $(".friends-dropdown").html(options.join("")).removeClass("friend");
        $(".make-my-choices-button").prop("disabled", true);
        $(".delete-choices-button").prop("disabled", true);
        $("body").addClass("has-choices-panel");
      } else {
        // hide
        $("body").removeClass("has-choices-panel");
      }
    }
  }
  
  function saveBallotUrl() {
    if (PUBLIC.localStorageIsAvailable()) {
      // save without friend or choices parameter
      var queries = location.search.substr(1).split("&");
      var q = [];
      $.each(queries, function() {
        var p = this.toString();
        if (p.substr(0, 7) != "friend=" && p.substr(0, 8) != "choices=")
          q.push(p);
      });
      var url = location.protocol + "//" + location.host + location.pathname + "?" +
        q.join("&");
      window.localStorage.setItem("ballotUrl", url);
    }
  }

  function setUpBanner() {
    var $banner = $(".sample-ballot-link");
    var email = $.queryString("sbe");
    if (UTIL.isValidEmail(email)) {
      setEnteredCookie();
      $banner.html('<input type="button" class="update-address-button button-4 button-smaller" style="margin-top:6px" value="Moved? Update Your Mailing Address">');
      $(".update-address-button").on("click", function () {
        location.href = location.protocol + "//" + location.host + "/sampleballotenrollment.aspx?email=" +
          email;
      });
    } else {
      $banner.html('<input type="button" class="sign-up-button button-4" value="Get Future Ballot Choices via Email">');
      $(".sign-up-button").on("click", function () {
        showSampleBallotDialog();
      });
    }
  }

  $(".local-districts-content,.local-districts-header,.referendums-list,.districts-list").accordion({
    active: false,
    collapsible: true,
    heightStyle: "content",
    activate: PUBLIC.accordionActivate
  });

  $(".referendums-content").accordion({
    collapsible: true,
    heightStyle: "content",
    activate: PUBLIC.accordionActivate
  });

  $(".instructions-accordion").accordion({
    active: false,
    collapsible: true,
    heightStyle: "content",
    activate: PUBLIC.accordionActivate
  });

  // if there's only one top-level accordion, open it Mantis 840
  var topAccordions = $(".ballot-report>.ui-accordion");
  if (topAccordions.length == 1) {
    topAccordions.accordion("option", "active", 0);
  }

  //saveBallotUrl();
  setUpChecks();
  //setUpBanner();
});
