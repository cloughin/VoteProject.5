define(["jquery", "vote/adminMaster", "jqueryui"],
  function ($, master) {

    function initPage() {
      return;
      $(".election-report,.category-content").accordion({
        collapsible: true,
        heightStyle: "content"
      });
    };

    // I N I T I A L I Z E

    master.inititializePage({
      callback: initPage
    });
  });