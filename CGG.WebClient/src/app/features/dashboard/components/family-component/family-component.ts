import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { Store } from '@ngrx/store';
import { loadChildren } from '../../../../store/family/family.actions';
import { selectChildren, selectChildrenLoading } from '../../../../store/family/family.selectors';
import { ChildProfile } from '../../../../store/family/family.state';

@Component({
  selector: 'app-family-component',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './family-component.html',
  styleUrl: './family-component.scss',
})
export class FamilyComponent implements OnInit {
  private router = inject(Router);
  private store = inject(Store);

  children = this.store.selectSignal(selectChildren);
  isLoading = this.store.selectSignal(selectChildrenLoading);

  ngOnInit(): void {
    this.store.dispatch(loadChildren());
  }

  goToChildProfile(childId: string) {
    this.router.navigate(['/child', childId]);
  }

  addChild() {
    this.router.navigate(['/add-child']);
  }
}

