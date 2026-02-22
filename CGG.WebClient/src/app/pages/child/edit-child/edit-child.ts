import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-edit-child',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-child.html',
  styleUrl: './edit-child.scss'
})
export class EditChild implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  childId: string | null = null;
  showAdditionalInfo = false;

  childForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.email]],
    age: ['', [Validators.required, Validators.min(1), Validators.max(100)]],
    gender: ['Female', Validators.required]
  });

  toggleAdditionalInfo() {
    this.showAdditionalInfo = !this.showAdditionalInfo;
  }

  ngOnInit(): void {
    this.childId = this.route.snapshot.paramMap.get('id');
    if (this.childId) {
      // TODO: Fetch child data from state/backend using this.childId
      // Mock data for now:
      this.childForm.patchValue({
        name: 'Mock Child',
        email: 'child@example.com',
        age: 12,
        gender: 'Male'
      });
    }
  }

  cancel() {
    this.router.navigate(['/child', this.childId]);
  }

  saveChanges() {
    if (this.childForm.valid) {
      // TODO: Update child data in state/backend
      console.log('Updated kid:', this.childForm.value);
      this.router.navigate(['/child', this.childId]);
    } else {
      this.childForm.markAllAsTouched();
    }
  }
}
