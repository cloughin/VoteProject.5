define(["jquery", "vote/adminMaster", "vote/util", "jqueryui"],
  function ($, master, util) {

    function initAutocomplete() {
      $(".autocomplete").autocomplete({
        source: function (request, response) {
          var $form = $(".main-form");
          util.ajax({
            url: "/Admin/WebService.asmx/GetLocalNameAutosuggest",
            data: {
              term: request.term,
              stateCode: $form.data("state"),
              countyCode: $form.data("county")
            },

            success: function (result) {
              response(result.d);
            },

            error: function (result) {
              response([]);
            }
          });
        }
      });
    };

    function initPage() {
      initAutocomplete();
      window.pageLoad = function () {
        initAutocomplete();
      };
    };

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });