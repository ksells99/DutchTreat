import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import {map} from "rxjs/operators"
import { LoginRequest, LoginResults } from "../shared/LoginResults";
import { Order, OrderItem } from "../shared/Order";
import { Product } from "../shared/Product";

@Injectable()
export class Store {

    constructor(private http: HttpClient) {

    }

    public products: Product[] = [];
    public order: Order = new Order();
    public token = "";
    public expiration = new Date();

    loadProducts(): Observable<void> {
        return this.http.get<[]>("/api/products").pipe(map(data => {
            this.products = data;
            return
        }));
    }

    get loginRequired(): boolean {
        // Returns true if no token or expiry date passed
        return this.token.length === 0 || this.expiration > new Date()
    }

    login(credentials: LoginRequest) {
        return this.http.post<LoginResults>("/account/createtoken", credentials)
            .pipe(map(data => {
                this.token = data.token;
                this.expiration = data.expiration;
            }))
    }

    checkout() {
        const headers = new HttpHeaders().set("Authorization", `Bearer ${this.token}`)
        return this.http.post("/api/orders", this.order, {
            headers: headers
        }).pipe(map(() => {
            // Reset order object once checkout complete
            this.order = new Order();
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
