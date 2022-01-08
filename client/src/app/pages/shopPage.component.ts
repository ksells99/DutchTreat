import { Component, OnInit } from "@angular/core";
import { Store } from "../services/store.service";

@Component({
    selector: 'app-shop',
    templateUrl: "shopPage.component.html",
})

export default class ShopPage implements OnInit {


    constructor(public store: Store) {
    }

    ngOnInit(): void {

    }
}
