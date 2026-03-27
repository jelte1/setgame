import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { GameService } from '../../../../core/services/game.service';
import { Observable } from 'rxjs';
import { GameModel } from '../../../../core/models/game.model';

@Component({
  selector: 'app-games',
  imports: [RouterLink],
  templateUrl: './games.html',
  styleUrl: './games.css',
})
export class Games {
  private gameService = inject(GameService);
  private router = inject(Router);

  games = signal<GameModel[]>([]);

  ngOnInit(): void {
    this.loadGames();
  }

  loadGames() {
    this.gameService.getUserGames().subscribe({
      next: (games) => {
        this.games.set(games);
        console.log('Games loaded:', games); // or this.games()
      },
      error: (err) => console.error('Error loading games:', err),
    });
  }
}
