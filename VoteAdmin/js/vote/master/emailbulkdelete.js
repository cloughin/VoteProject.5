define(["jquery", "vote/adminMaster", "vote/util"],
function ($, master, util) {

  function initPage() {
    $(".submit-form").click(function () {
      if ($(".delete-checkbox input").prop("checked"))
        if (!confirm("You are about to actually delete the selected email addresses. Ok?"))
          return;
      $("#MainForm")[0].submit();
    });
    $(".main-form").on("propertychange change click keyup input paste", ".extra-addresses", function () {
      var $textarea = $(this);
      var val = $textarea.val();
      var newVal = val.replace(/^\s*mailto:\s*/img, "");
      if (val !== newVal)
        $textarea.val(newVal);
    });
    $(".lookup-source").on("click", function() {
      util.openAjaxDialog("Looking up...");
      util.ajax({
        url: "/Admin/WebService.asmx/LookupEmailSourceCode",
        data: {
          sourceCode: $(".lookup-source-id-input").val()
        },

        success: function(result) {
          util.closeAjaxDialog();
          $(".source-id-email").text(result.d);
        },

        error: function(result) {
          util.closeAjaxDialog();
          util.alert(util.formatAjaxError(result,
            "Could lookup"));
        }
      });
    });
  }
  master.inititializePage({
    callback: initPage
  });
});