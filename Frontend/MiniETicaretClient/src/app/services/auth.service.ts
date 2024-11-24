import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { FlexiToastService } from 'flexi-toast';


@Injectable({
    providedIn: 'root'
})
export class AuthService {
    constructor(
        private toast: FlexiToastService,
        private router: Router
    ) { }
    
    isLoggedIn(): boolean {
        const token = localStorage.getItem('my-token');
        if (token) {
            return true;
        } else {
            this.toast.showToast("","Giriş sayfasına yönlendiriliyorsunuz.");
            this.router.navigate(['/login']);
            return false;
        }
    }
}