import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { Observable, tap } from 'rxjs';
import { AuthResponseModel } from '../models/authResponse.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService extends BaseApiService {
  login(username: string, password: string): Observable<AuthResponseModel> {
    return this.http
      .post<AuthResponseModel>(`${this.apiUrl}users/login`, {
        userName: username,
        passwordHash: password,
      })
      .pipe(
        tap((response) => {
          console.log(response);
          localStorage.setItem('token', response.token);
          localStorage.setItem('userId', response.userId);
        }),
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getUserId(): string | null {
    return localStorage.getItem('userId');
  }
}
