import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { Observable, tap } from 'rxjs';
import { AuthResponseModel } from '../models/authResponse.model';

@Injectable({ providedIn: 'root' })
export class AuthService extends BaseApiService {

  login(username: string, password: string): Observable<AuthResponseModel> {
    return this.http
      .post<AuthResponseModel>(`${this.apiUrl}/users/login`, {
        userName: username,
        passwordHash: password,
      })
      .pipe(
        tap((response) => {
          localStorage.setItem('token', response.token);
          localStorage.setItem('userId', response.userId);
          localStorage.setItem('refreshToken', response.refreshToken);
        }),
      );
  }

  refreshToken(refreshToken: string): Observable<AuthResponseModel> {
    return this.http
      .post<AuthResponseModel>(`${this.apiUrl}/users/refreshtoken`, {
        userId: localStorage.getItem('userId'),
        token: localStorage.getItem('token'),
        refreshToken: refreshToken,
      })
      .pipe(
        tap((response) => {
          localStorage.setItem('token', response.token);
          localStorage.setItem('userId', response.userId);
          localStorage.setItem('refreshToken', response.refreshToken);
        }),
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('refreshToken');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getUserId(): string | null {
    return localStorage.getItem('userId');
  }
}
