import { Component, OnInit } from "@angular/core";
import { Store } from "../services/store.service";

@Component({
    selector: 'app-basket',
    templateUrl: "basketView.component.html",
    styleUrls: ["basketView.component.css"]
})

export default class BasketView implements OnInit {


    constructor(public store: Store) {
    }

    ngOnInit(): void {
  
    }
}
