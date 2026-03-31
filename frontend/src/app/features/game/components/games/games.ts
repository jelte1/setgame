import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { GameService } from '../../../../core/services/game.service';
import { BaseGameModel } from '../../../../core/models/baseGame.model';
import { RefactorDatePipe } from '../../../../core/pipes/refactorDate.pipe';

@Component({
  selector: 'app-games',
  imports: [RouterLink, RefactorDatePipe],
  templateUrl: './games.html',
  styleUrl: './games.css',
})
export class Games {
  private gameService = inject(GameService);
  private router = inject(Router);

  games = signal<BaseGameModel[]>([]);

  ngOnInit(): void {
    this.loadGames();
  }

  loadGames() {
    this.gameService.getUserGames().subscribe({
      next: (games) => {
        this.games.set(games);
      },
      error: (err) => console.error('Error loading games:', err),
    });
  }
}
