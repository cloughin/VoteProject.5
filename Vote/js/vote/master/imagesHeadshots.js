define(["vote/adminMaster", "vote/util"], function (master, util) {
  master.inititializePage({
    callback: util.insertLocalDates,
    endRequest: util.insertLocalDates
  });
});