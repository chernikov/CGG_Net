import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-parent-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './parent-register.html',
  styleUrl: './parent-register.scss'
})
export class ParentRegisterComponent {
  formData = {
    email: '',
    password: '',
    confirmPassword: '',
    firstName: '',
    lastName: ''
  };

  constructor(private router: Router) {}

  onSubmit() {
    // TODO: Implement registration logic
    console.log('Parent registration:', this.formData);
  }

  goBack() {
    this.router.navigate(['/parent-choice']);
  }
}
