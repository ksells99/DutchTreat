import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "../services/store.service";
import { LoginRequest } from "../shared/LoginResults";

@Component({
    selector: 'app-login',
    templateUrl: "loginPage.component.html",
})

export default class LoginPage {


    constructor(private store: Store, private router: Router) {
    }

    public credentials: LoginRequest = {
        username: "",
        password: ""
    }

    public errorMessage = "";

    onLogin() {
        this.store.login(this.credentials).subscribe(() => {
            // If user has items in basket, go to checkout
            if (this.store.order.items.length > 0) {
                this.router.navigate(["checkout"]);

            // Otherwise go to main shop page
            } else {
                this.router.navigate([""]);
            }
        }, error => {
            console.log(error);
            this.errorMessage = "Failed to login";
        })
    }
}