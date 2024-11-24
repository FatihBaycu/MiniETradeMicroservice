import { Component, signal } from '@angular/core';
import { FlexiGridModule } from 'flexi-grid';
import { TrCurrencyPipe } from 'tr-currency';
import { OrderModel } from '../../models/order.model';
import { HttpClient } from '@angular/common/http';
import { api } from '../../constants';
import { DataResult } from '../../models/data-result.model';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [FlexiGridModule, TrCurrencyPipe],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent {
  orders = signal<OrderModel[]>([]);
  token = signal<string>("");
  
  constructor(
    private http: HttpClient    
  ) {
    if(localStorage.getItem("my-token")){
      this.token.set(localStorage.getItem("my-token")!);

      this.getAll();
    }
  }

  getAll(){
    this.http.get<DataResult<OrderModel[]>>(`${api}/orders/getall`,{
      headers: {
        "Authorization": "Bearer " + this.token()
      }
    }).subscribe(res=> {
      this.orders.set(res.data!);
    });
  }
}
