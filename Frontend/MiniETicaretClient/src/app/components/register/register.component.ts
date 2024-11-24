import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { FlexiToastService } from 'flexi-toast';
import { api } from '../../constants';
import { UserModel } from '../../models/user.model';
import { DataResult } from '../../models/data-result.model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  model = signal<UserModel>(new UserModel());

  constructor(
    private http: HttpClient,
    private router: Router,
    private toast: FlexiToastService
  ) { }

  register() {
    this.http.post<DataResult<UserModel>>(`${api}/Users/register`, this.model()).subscribe({
      next: (res) => {
        this.toast.showToast("Başarılı", res.message!, "success");
        this.router.navigateByUrl("/login");
      },
      error: (err: HttpErrorResponse) => {
        console.log(err);
        this.toast.showToast("Hata!", "Bir şeyler ters gitti", "error");
      }
    })
  }
}
