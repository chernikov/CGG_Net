import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="min-h-screen bg-gradient-to-br from-primary to-secondary">
      <div class="container-custom py-20">
        <div class="text-center text-white">
          <h1 class="text-6xl font-bold mb-6">Career Guidance Guild</h1>
          <p class="text-2xl mb-12">Знайди свій шлях до успішної кар'єри</p>
          <div class="flex gap-4 justify-center">
            <button (click)="goToRegister()" class="btn bg-white text-primary hover:bg-gray-100">
              Почати
            </button>
            <button (click)="goToLogin()" class="btn border-2 border-white text-white hover:bg-white hover:text-primary">
              Увійти
            </button>
          </div>
        </div>
      </div>
    </div>
  `
})
export class HomeComponent {
  constructor(private router: Router) {}

  goToRegister() {
    this.router.navigate(['/auth/register']);
  }

  goToLogin() {
    this.router.navigate(['/auth/login']);
  }
}
