import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import {map} from "rxjs/operators"
import { Order, OrderItem } from "../shared/Order";
import { Product } from "../shared/Product";

@Injectable()
export class Store {

    constructor(private http: HttpClient) {

    }

    public products: Product[] = [];
    public order: Order = new Order();

    loadProducts(): Observable<void> {
        return this.http.get<[]>("/api/products").pipe(map(data => {
            this.products = data;
            return
        }));
    }

    addToOrder(product: Product) {

        let item: OrderItem;

        // Check if item user adds to basket is already there
        item = this.order.items.find(order => order.productId === product.id)!;

        // If so, increase existing qty by 1
        if (item) {
            item.quantity++;

        // Otherwise add new entry to basket
        } else {
            item = new OrderItem();
            item.productId = product.id;
            item.productTitle = product.title;
            item.productArtId = product.artId;
            item.productArtist = product.artist;
            item.productCategory = product.category;
            item.productSize = product.size;
            item.unitPrice = product.price;
            item.quantity = 1;


            this.order.items.push(item);
        }

    }
}
