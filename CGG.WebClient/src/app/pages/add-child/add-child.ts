import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { FamilyService } from '../../features/dashboard/services/family';

@Component({
  selector: 'app-add-child',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule],
  templateUrl: './add-child.html',
  styleUrl: './add-child.scss',
})
export class AddChild {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private familyService = inject(FamilyService);

  showAdditionalInfo = false;
  isSubmitting = false;
  errorMessage = '';

  childForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.email]],
    age: ['', [Validators.required, Validators.min(1), Validators.max(100)]],
    gender: ['Female', Validators.required]
  });

  toggleAdditionalInfo() {
    this.showAdditionalInfo = !this.showAdditionalInfo;
  }

  cancel() {
    this.router.navigate(['/dashboard']);
  }

  add() {
    if (this.childForm.valid) {
      this.isSubmitting = true;
      this.errorMessage = '';
      
      this.familyService.addChild(this.childForm.value).subscribe({
        next: (response) => {
          console.log('Saved kid:', response);
          this.isSubmitting = false;
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          console.error('Error adding child:', err);
          this.errorMessage = err.error?.message || 'Failed to add child';
          this.isSubmitting = false;
        }
      });
    } else {
      this.childForm.markAllAsTouched();
    }
  }
}
