import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-student-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './student-register.html',
  styleUrl: './student-register.scss'
})
export class StudentRegisterComponent {
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
    console.log('Student registration:', this.formData);
  }

  goBack() {
    this.router.navigate(['/student-choice']);
  }
}
