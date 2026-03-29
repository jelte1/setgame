import { Component, computed, inject, signal } from '@angular/core';
import { GameModel } from '../../../../core/models/game.model';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { GameService } from '../../../../core/services/game.service';
import { CardLocation } from '../../../../core/models/gameCardStateModel';
import { Board } from '../board/board';
import { CardModel } from '../../../../core/models/card.model';
import { SET_SIZE } from '../../../../core/constants/constants';

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
  selectedCards = signal(<CardModel[]>[]);

  game = signal<GameModel | null>(null);

  tableCards = computed(() => {
    return (
      this.game()
        ?.gameCardStates.filter((gs) => gs.location === CardLocation.Table)
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
        this.game.set(game);
        console.log(this.game()?.gameCardStates);
      },
      error: () => {
        // errorrrr....
      },
    });
  }

  selectCard(card: CardModel) {
    const currentSelectedCards = this.selectedCards();
    const isSelected = currentSelectedCards.some((c) => c.id === card.id);

    if (isSelected) {
      this.selectedCards.set(currentSelectedCards.filter((c) => c.id !== card.id));
      return;
    }

    if (currentSelectedCards.length < SET_SIZE) {
      this.selectedCards.set([...currentSelectedCards, card]);
    }

    if (this.selectedCards().length === SET_SIZE) {
      this.submitSet();
    }
  }

  submitSet() {
    const cardsToSubmit = this.selectedCards();

    this.gameService.checkSet(this.id, cardsToSubmit[0].id, cardsToSubmit[1].id, cardsToSubmit[2].id)
      .subscribe({
        next: (response) => {
          if (response.isSet) {
            alert("bingo!");
            this.game.update((currentGame) => {

              if (!currentGame) {
                throw new Error("game not loaded");
              }

              const submittedIds = cardsToSubmit.map((c) => c.id);
              const withoutDiscarded = currentGame.gameCardStates.filter(
                gs => !submittedIds.includes(gs.card.id),
              );

              const updated = [...withoutDiscarded, ...response.newGameCardStates];

              return { ...currentGame, gameCardStates: updated };
            });
          } else {
            alert('Invalid set.');
          }
          this.selectedCards.set([]);
        },
        error: () => {
          alert('Error submitting set. Please try again.');
          this.selectedCards.set([]);
        },
      });
  }
}
