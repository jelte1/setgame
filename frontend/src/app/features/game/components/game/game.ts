import { Component, computed, inject, signal } from '@angular/core';
import { GameModel } from '../../../../core/models/game.model';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { GameService } from '../../../../core/services/game.service';
import { CardLocation } from '../../../../core/models/gameState.model';
import { Board } from '../board/board';

@Component({
  selector: 'app-game',
  standalone: true,
  imports: [Board, RouterLink],
  templateUrl: './game.html',
  styleUrl: './game.css',
})
export class Game {
  private route = inject(ActivatedRoute);
  private gameService = inject(GameService);

  id!: number;
  private routeSub!: Subscription;

  game = signal<GameModel | null>(null);

  tableCards = computed(() => {
    return (
      this.game()
        ?.gameStates.filter((gs) => gs.location === CardLocation.Table)
        .map((gs) => gs.card) ?? []
    );
  });

  ngOnInit(): void {
    this.routeSub = this.route.params.subscribe((params) => {
      this.id = params['id'];
    });

    this.loadGame(this.id);
  }

  ngOnDestroy(): void {
    this.routeSub.unsubscribe();
  }

  loadGame(id: number) {
    this.gameService.getGameById(id).subscribe({
      next: (game) => {
        console.log(game);
        this.game.set(game);
        console.log(this.game()?.gameStates);
      },
      error: () => {
        // errorrrr....
      },
    });
  }
}
