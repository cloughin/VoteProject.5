define(["jquery", "vote/adminMaster", "vote/util", 
 "vote/controls/findPolitician"],
function ($, master, util, findPolitician) {

  var onChange = function($control, l) {
    //alert("change");
  };

  var initPage = function () {
    var fp1 = new findPolitician.FindPolitician($(".find-politician-1"));
    fp1.init();
    var fp2 = new findPolitician.FindPolitician($(".find-politician-2"),
    { getStateCode: function () { return "VA"; } });
    fp2.init();
  };

  // I N I T I A L I Z E

  master.inititializePage({
    callback: initPage
  });

});