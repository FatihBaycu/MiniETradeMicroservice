import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { ShoppingCartModel } from '../models/shopping-cart.model';
import { api } from '../constants';
import { FlexiToastService } from 'flexi-toast';
import { DataResult } from '../models/data-result.model';
import { Result } from '../models/result.model';

@Injectable({
  providedIn: 'root'
})
export class CartsService {
  token = signal<string>("");
  carts = signal<ShoppingCartModel[]>([]);

  constructor(
    private http: HttpClient,
    private toast: FlexiToastService
  ) {
    if (localStorage.getItem("my-token")) {
      this.token.set(localStorage.getItem("my-token")!);

      this.getAll();
    }
  }

  getAll() {
    this.http.get<DataResult<ShoppingCartModel[]>>(`${api}/ShoppingCarts/getall`, {
      headers: {
        "Authorization": "Bearer " + this.token()
      }
    }).subscribe(res => {
      this.carts.set(res.data!);
    });
  }

  createOrder() {
    this.toast.showSwal("Sipariş Oluştur?", "Sipariş oluşturmak istiyor musunuz?", () => {
      this.http.get<Result>(`${api}/shoppingCarts/create-order`, {
        headers: {
          "Authorization": "Bearer " + this.token()
        }
      }).subscribe(res => {
        this.getAll();
        this.toast.showToast("Başarılı", res?.message ?? "", "info");
      });
    }, "Oluştur", "Vazgeç")
  }
}
