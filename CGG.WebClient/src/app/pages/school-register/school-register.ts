import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-school-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './school-register.html',
  styleUrl: './school-register.scss'
})
export class SchoolRegisterComponent {
  formData = {
    email: '',
    password: '',
    confirmPassword: '',
    schoolName: '',
    contactPerson: ''
  };

  constructor(private router: Router) {}

  onSubmit() {
    // TODO: Implement registration logic
    console.log('School registration:', this.formData);
  }

  goBack() {
    this.router.navigate(['/school-choice']);
  }
}
