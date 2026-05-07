import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, map, of } from 'rxjs';

// export const authGuard: CanActivateFn = (route, state) => {
//   const authService = inject(AuthService);
//   const router = inject(Router);
//
//   if (authService.isLoggedIn()) {
//     return true;
//   } else {
//     router.navigate(['/login']);
//     return false;
//   }
// };
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true;
  }

  const refreshToken = localStorage.getItem('refreshToken');

  // no refresh token
  if (!refreshToken) {
    return router.navigate(['/login']);
  }

  // try refreshing if no token
  return authService.refreshToken(refreshToken).pipe(
    map((response) => {
      localStorage.setItem('token', response.token);
      localStorage.setItem('refreshToken', response.refreshToken);
      localStorage.setItem('userId', response.userId);
      return true;
    }),
    catchError(() => {
      authService.logout();
      return router.navigate(['/login']);
    }),
  );
};
