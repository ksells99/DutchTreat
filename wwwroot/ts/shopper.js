var Shopper = /** @class */ (function () {
    function Shopper(first, last) {
        this.firstName = "";
        this.lastName = "";
        this.firstName = first;
        this.lastName = last;
    }
    Shopper.prototype.showName = function () {
        alert("".concat(this.firstName, " ").concat(this.lastName));
    };
    return Shopper;
}());
//# sourceMappingURL=shopper.js.map