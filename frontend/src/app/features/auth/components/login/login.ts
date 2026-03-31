import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../../../core/services/auth.service';
import { Router, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.html',
  styleUrl: './login.css',
  imports: [FormsModule],
})

export class Login {
  private authService = inject(AuthService);
  private router = inject(Router);

  username = '';
  password = '';
  loading = false;

  onLogin() {
    if (!this.username || !this.password) {
      // feedback for user....
      return;
    }

    this.loading = true;

    this.authService.login(this.username, this.password).subscribe({
      next: () => {
        this.router.navigate(['/games']);
      },
      error: (err) => {
        // feedback for user....
        console.error('Invalid username or password.', err);
        this.loading = false;
      },
    });
  }
}
