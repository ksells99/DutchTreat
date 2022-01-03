class Shopper {
    firstName = "";
    lastName = "";

    constructor(first: string, last: string) {
        this.firstName = first;
        this.lastName = last;
    }

    showName() {
        alert(`${this.firstName} ${this.lastName}`);
    }

}