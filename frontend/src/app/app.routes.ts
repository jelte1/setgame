import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { Login } from './features/auth/components/login/login';
import { Games } from './features/game/components/games/games';

export const routes: Routes = [
  { path: '', component: Login },
  { path: 'login', component: Login },
  { path: 'games', component: Games, canActivate: [authGuard] },
  // { path: 'games/:id', component: GameComponent, canActivate: [authGuard] },
];
