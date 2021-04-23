define(function(require, exports, module) {
"use strict";

var oop = require("../lib/oop");
var Tokenizer = require("../tokenizer").Tokenizer;
var HtmlVoteTemplateHighlightRules = require("./html_votetemplate_highlight_rules").HtmlVoteTemplateHighlightRules;
var HtmlMode = require("./html").Mode;
var JavaScriptMode = require("./javascript").Mode;
var CssMode = require("./css").Mode;

var Mode = function() {
    HtmlMode.call(this);
    this.HighlightRules = HtmlVoteTemplateHighlightRules;    
    this.createModeDelegates({
        "js-": JavaScriptMode,
        "css-": CssMode
    });
};
oop.inherits(Mode, HtmlMode);

(function() {

}).call(Mode.prototype);

exports.Mode = Mode;
});