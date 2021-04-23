define(function(require, exports, module) {
"use strict";

exports.isDark = false;
exports.cssClass = "ace-votetemplate";
exports.cssText = require("../requirejs/text!./votetemplate.css");

var dom = require("../lib/dom");
dom.importCssString(exports.cssText, exports.cssClass);
});
