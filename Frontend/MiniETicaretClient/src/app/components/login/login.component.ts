import { Component, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserModel } from '../../models/user.model';

import { FormsModule } from '@angular/forms'
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { api } from '../../constants';
import { FlexiToastService } from 'flexi-toast';
import { DataResult } from '../../models/data-result.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  model = signal<UserModel>(new UserModel());

  constructor(
    private http: HttpClient,
    private toast: FlexiToastService,
    private router: Router
  ) { }

  login() {
    this.http.post<DataResult<string>>(`${api}/Users/login`, this.model()).subscribe({
      next: (res) => {
        localStorage.setItem("my-token", res.data!);
        this.router.navigateByUrl("/");
      },
      error: (err: HttpErrorResponse) => {
        console.log(err);
        this.toast.showToast("Hata!", "Bir şeyler ters gitti", "error");
      }
    })
  }
}
