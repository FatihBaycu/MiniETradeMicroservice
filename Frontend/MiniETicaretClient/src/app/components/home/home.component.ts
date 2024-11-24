import { Component, signal } from '@angular/core';
import { ProductModel } from '../../models/product.model';
import { HttpClient } from '@angular/common/http';
import { api } from '../../constants';
import { FlexiToastService } from 'flexi-toast';
import { CartsService } from '../../services/carts.service';
import { DataResult } from '../../models/data-result.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  products = signal<ProductModel[]>([]);
  token = signal<string>("");

  constructor(
    private http: HttpClient,
    private toast: FlexiToastService,
    private cart: CartsService
  ) {
    if (localStorage.getItem("my-token")) {
      this.token.set(localStorage.getItem("my-token")!);

      this.getAll();
    }
  }

  getAll() {
    this.http.get<DataResult<ProductModel[]>>(`${api}/Products/getall`, {
      headers: {
        "Authorization": "Bearer " + this.token()
      }
    }).subscribe(res => {
      this.products.set(res.data!);
    });
  }

  addShoppingCart(product: ProductModel) {
    const data = {
      productId: product.id,
      quantity: 1
    }
    this.http.post<DataResult<string>>(`${api}/ShoppingCarts/create`, data, {
      headers: {
        "Authorization": "Bearer " + this.token()
      }
    }).subscribe(res => {
      this.toast.showToast("Başarılı", res.message!, "info");
      this.cart.getAll();
      this.getAll();
    });
  }
}
