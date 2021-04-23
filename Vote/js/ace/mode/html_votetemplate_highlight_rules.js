define(function(require, exports, module) {
  "use strict";

  var oop = require("../lib/oop");
  var lang = require("../lib/lang");
  var HtmlHighlightRules = require("./html_highlight_rules").HtmlHighlightRules;

  var substitutions = {};
  var emailSubstitutions = {};
  var webSubstitutions = {};
  var deprecatedSubstitutions = {};
  var deprecatedEmailSubstitutions = {};
  var deprecatedWebSubstitutions = {};

  var HtmlVoteTemplateHighlightRules = function() {
    HtmlHighlightRules.call(this);

    function validateVoteSubstitution(map, value) {
      return map.hasOwnProperty(
       value.toLowerCase().replace(/(\()[^)]*(?=\))/, 
         function($0, $1) { return $1 ? $1 + "" : $0; }));
    }

    var startRules = [
      {
        regex: "(\\[\\[)([^\[@#<]+?)(]])",
        token: function (value1, value2, value3) {
          return ["constant.language.escape",
            validateVoteSubstitution(substitutions, value2)
              ? "meta.substitution.valid"
                : (validateVoteSubstitution(deprecatedSubstitutions, value2)
		                ? "meta.substitution.deprecated"
		                : "meta.substitution"), 
            "constant.language.escape"];
        }
      }, {
        regex: "(@@)((?:[^\[@#<]|@(?!@))+?)(@@)",
        token: function (value1, value2, value3) {
          return ["constant.language.escape",
          value2.indexOf("@") >= 0 ||
            validateVoteSubstitution(emailSubstitutions, value2)
              ? "meta.substitution.valid"
                : (validateVoteSubstitution(deprecatedEmailSubstitutions, value2)
		                ? "meta.substitution.deprecated"
		                : "meta.substitution"),
          "constant.language.escape"];
        }
      }, {
        regex: "(##)([^\[@#<]+?)(##)",
        token: function (value1, value2, value3) {
          return ["constant.language.escape",
          value2.indexOf(".") >= 0 ||
            validateVoteSubstitution(webSubstitutions, value2)
              ? "meta.substitution.valid"
                : (validateVoteSubstitution(deprecatedWebSubstitutions, value2)
		                ? "meta.substitution.deprecated"
		                : "meta.substitution"),
          "constant.language.escape"];
        }
      }, {
        regex: "(\\{\\{)(\\s*)(then|else|endif)(\\s*)(}})",
        token: ["constant.language.escape", "text", "meta.conditional", "text", "constant.language.escape"],
        caseInsensitive: true
      }, {
        regex: "(\\{\\{)(\\s*)((?:if|elseif)(?![a-z]))(\\s*)(>=?|<=?|=?=|!=|(?:(?:match|notmatch|empty|notempty)(?![a-z])))",
        token: ["constant.language.escape", "text", "meta.conditional", "text", "meta.tag.punctuation"],
        caseInsensitive: true
      }, {
        regex: "(\\{\\{)(\\s*)(fail)(\\s*)(.*?)(\\s*)(}})",
        token: ["constant.language.escape", "text", "meta.conditional", "text", "text", "text", "constant.language.escape"],
        caseInsensitive: true
      }, {
        regex: "(\\{\\{)(\\s*)((?:if|elseif|fail)(?![a-z]))",
        token: ["constant.language.escape", "text", "meta.conditional"],
        caseInsensitive: true
      }, {
        regex: "\\[\\[|@@|##|\\{\\{|}}",
        token: "constant.language.escape"
      }
    ];

    var endRules = [
      {
        token: "support.votetemplate_tag",
        regex: "%>",
        next: "pop"
      }, {
        token: "comment",
        regex: "#(?:[^%]|%[^>])*"
      }
    ];

    for (var key in this.$rules)
      this.$rules[key].unshift.apply(this.$rules[key], startRules);

    this.normalizeRules();
  };

  oop.inherits(HtmlVoteTemplateHighlightRules, HtmlHighlightRules);

  var setSubstitutions = function (lists) {
    if (lists.substitutionList)
      substitutions = lang.arrayToMap(lists.substitutionList.split("|"));
    if (lists.emailSubstitutionList)
      emailSubstitutions = lang.arrayToMap(lists.emailSubstitutionList.split("|"));
    if (lists.webSubstitutionList)
      webSubstitutions = lang.arrayToMap(lists.webSubstitutionList.split("|"));
    if (lists.deprecatedSubstitutionList)
      deprecatedSubstitutions = lang.arrayToMap(lists.deprecatedSubstitutionList.split("|"));
    if (lists.deprecatedEmailSubstitutionList)
      deprecatedEmailSubstitutions = lang.arrayToMap(lists.deprecatedEmailSubstitutionList.split("|"));
    if (lists.deprecatedWebSubstitutionList)
      deprecatedWebSubstitutions = lang.arrayToMap(lists.deprecatedWebSubstitutionList.split("|"));
  };

  exports.HtmlVoteTemplateHighlightRules = HtmlVoteTemplateHighlightRules;
  exports.setSubstitutions = setSubstitutions;
});