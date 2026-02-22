import { inject } from '@angular/core';
import { Router, CanActivateFn, UrlTree } from '@angular/router';
import { UserRole } from '../../store/auth/auth.state';

function parseRole(raw: unknown): UserRole | null {
  if (typeof raw === 'number') return raw as UserRole;
  if (typeof raw === 'string') {
    // numeric string: "3"
    const n = Number(raw);
    if (!isNaN(n) && n in UserRole) return n as UserRole;
    // named string: "Admin"
    const key = raw as keyof typeof UserRole;
    if (key in UserRole) return UserRole[key];
  }
  return null;
}

export const homeGuard: CanActivateFn = (): boolean | UrlTree => {
  const router = inject(Router);

  const token = localStorage.getItem('token');
  if (!token) {
    return true; // not authenticated — show landing
  }

  try {
    const user = JSON.parse(localStorage.getItem('currentUser') ?? 'null');
    const role = parseRole(user?.role);
    if (role === UserRole.Admin) {
      return true; // admins can view landing
    }
    return router.createUrlTree(['/dashboard']);
  } catch {
    return true;
  }
};
