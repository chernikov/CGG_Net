import { Component, inject, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { addChild, clearAddChildError } from '../../store/family/family.actions';
import { selectAddingChild, selectAddChildError } from '../../store/family/family.selectors';
import { BackHeaderComponent } from '../../shared/components/back-header/back-header.component';

@Component({
  selector: 'app-add-child',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslateModule, BackHeaderComponent],
  templateUrl: './add-child.html',
  styleUrl: './add-child.scss',
})
export class AddChild implements OnDestroy {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private store = inject(Store);

  showAdditionalInfo = false;
  isSubmitting = this.store.selectSignal(selectAddingChild);
  errorMessage = this.store.selectSignal(selectAddChildError);

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
      this.store.dispatch(clearAddChildError());
      const { name, email, age, gender } = this.childForm.value;
      this.store.dispatch(addChild({ name, email: email || undefined, age: +age, gender }));
    } else {
      this.childForm.markAllAsTouched();
    }
  }

  ngOnDestroy(): void {
    this.store.dispatch(clearAddChildError());
  }
}

