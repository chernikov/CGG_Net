import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin',
  imports: [CommonModule],
  templateUrl: './admin.html',
  styleUrl: './admin.scss'
})
export class AdminComponent {
  protected readonly title = 'Admin Dashboard';
  
  // TODO: Add admin functionality
  // - User management
  // - Survey management
  // - Analytics
  // - System settings
}
