var Test = (function () {
    function Test(name) {
        this.name = name;
    }
    Test.prototype.val = function (newval) {
        if (newval !== undefined)
            this.name = newval;
        return this.name;
    };
    Test.stat = "abc";
    return Test;
}());
//# sourceMappingURL=test2.js.map