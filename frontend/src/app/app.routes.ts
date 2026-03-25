import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { Login } from './features/auth/components/login/login';

export const routes: Routes = [
  { path: 'login', component: Login },
  // { path: 'games', component: GameListComponent, canActivate: [authGuard] },
  // { path: 'games/:id', component: GameComponent, canActivate: [authGuard] },
  // { path: '', redirectTo: 'games', pathMatch: 'full' },
];
