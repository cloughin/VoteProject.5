require.config(
{
  urlArgs: "v=513",  
  waitSeconds: 0, 
  paths: {
    "jquery": "jq/jquery-1.11.3",
    "jqueryui": "jq/jquery-ui-1.11.4",
    "ajaxfileupload": "jq/ajaxfileupload",
    "alphanum": "jq/jquery.alphanum",
    "dynatree": "jq/jquery.dynatree",
    "kalypto": "jq/kalypto",
    "moment": "jq/moment.min",
    "resizablecolumns": "jq/jquery.resizableColumns",
    "slicknav": "jq/jquery.slicknav.min",
    "slimscroll": "jq/jquery.slimscroll.min",
    "store": "jq/store",
    "stupidtable": "jq/stupidtable",
    "textchange": "jq/textchange",
    "tiptip": "jq/jquery.tipTip.minified"
  },
  shim: {
    "jqueryui": ["jquery"],
    "ajaxfileupload": ["jquery"],
    "alphanum": ["jquery"],
    "dynatree": {
      deps: ["vote/util", "jqueryui"],
      init: function (util) {
        util.loadCss("/images/dynatree/skin-vista/ui.dynatree.css");
      }
    },
    "kalypto": ["jquery"],
    "resizablecolumns": ["jquery", "store"],
    "slicknav": ["jquery"],
    "slimscroll": ["jquery"],
    "stupidtable": ["jquery"],
    "textchange": ["jquery"],
    "tiptip": ["jquery"]
  }
});

require(["jqueryui"], function () { });
