import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { FamilyService, ChildProfile } from '../../services/family';

@Component({
  selector: 'app-family-component',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './family-component.html',
  styleUrl: './family-component.scss',
})
export class FamilyComponent implements OnInit {
  private router = inject(Router);
  private familyService = inject(FamilyService);

  children: ChildProfile[] = [];
  isLoading = true;

  ngOnInit(): void {
    this.familyService.getChildren().subscribe({
      next: (data) => {
        this.children = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to fetch children', err);
        this.isLoading = false;
      }
    });
  }

  goToChildProfile(childId: string) {
    this.router.navigate(['/child', childId]);
  }

  addChild() {
    this.router.navigate(['/add-child']);
  }
}
