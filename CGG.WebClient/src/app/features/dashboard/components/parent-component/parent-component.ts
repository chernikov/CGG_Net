import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { selectUser } from '../../../../store/auth/auth.selectors';
import { FamilyComponent } from '../family-component/family-component';
import { TakeCareerTestCardComponent } from '../take-career-test-card/take-career-test-card.component';
import { CreditsBalanceComponent } from '../credits-balance/credits-balance';

@Component({
  selector: 'app-parent-component',
  standalone: true,
  imports: [CommonModule, FamilyComponent, TakeCareerTestCardComponent, CreditsBalanceComponent],
  templateUrl: './parent-component.html',
  styleUrl: './parent-component.scss',
})
export class ParentComponent implements OnInit {
  private store = inject(Store);
  private router = inject(Router);

  parentName: string = 'User';
  parentAvatar: string = 'assets/images/avatar-parent.png';

  ngOnInit(): void {
    this.store.select(selectUser).subscribe(user => {
      if (user) {
        this.parentName = user.firstName || 'User';
      }
    });
  }

  goToProfile() {
    this.router.navigate(['/profile']);
  }

  goToBuyCredits() {
    this.router.navigate(['/buy-credits']);
  }
}
