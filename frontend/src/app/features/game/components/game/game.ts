import { Component, computed, inject, signal } from '@angular/core';
import { GameModel } from '../../../../core/models/game.model';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { GameService } from '../../../../core/services/game.service';
import { CardLocation } from '../../../../core/models/gameCardState.model';
import { Board } from '../board/board';
import { CardModel } from '../../../../core/models/card.model';
import { SET_SIZE } from '../../../../core/constants/constants';
import { RefactorDatePipe } from '../../../../core/pipes/refactorDate.pipe';

@Component({
  selector: 'app-game',
  standalone: true,
  imports: [Board, RouterLink, RefactorDatePipe],
  templateUrl: './game.html',
  styleUrl: './game.css',
})
export class Game {
  private route = inject(ActivatedRoute);
  private gameService = inject(GameService);

  id!: number;
  private routeSub!: Subscription;

  selectedCards = signal(<CardModel[]>[]);
  hintedCards = signal<number[]>([]);

  game = signal<GameModel | null>(null);

  tableCards = computed(() => {
    return (
      this.game()
        ?.gameCardStates.filter((gs) => gs.location === CardLocation.Table)
        .map((gs) => gs.card) ?? []
    );
  });

  cardsInDeck = computed(() => {
    return (
      this.game()
        ?.gameCardStates.filter((gs) => gs.location === CardLocation.Deck)
        .map((gs) => gs.card) ?? []
    );
  })

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
      },
      error: () => {
        // errorrrr....
      },
    });
  }

  selectCard(card: CardModel) {

    if (this.game()?.isFinished) {
      return;
    }

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

    this.gameService
      .checkSet(this.id, cardsToSubmit[0].id, cardsToSubmit[1].id, cardsToSubmit[2].id)
      .subscribe({
        next: (response) => {
          if (response.isSet) {
            alert('bingo!');
            this.game.update((currentGame) => {
              if (!currentGame) {
                throw new Error('game not loaded');
              }

              const cardIdsToDiscard = [
                response.foundSet.card1.id,
                response.foundSet.card2.id,
                response.foundSet.card3.id,
              ];

              const gameCardStatesWithoutDiscarded = currentGame.gameCardStates.filter(
                (gcs) => !cardIdsToDiscard.includes(gcs.card.id),
              );

              const updatedGameCardStates = [
                ...gameCardStatesWithoutDiscarded,
                ...response.newGameCardStates,
              ];
              const updatedFoundSets = [...currentGame.foundSets, response.foundSet];
              const updatedPossibleSetsCount = response.possibleSetsCount;

              return {
                ...currentGame,
                gameCardStates: updatedGameCardStates,
                foundSets: updatedFoundSets,
                possibleSetsCount: updatedPossibleSetsCount,
                isFinished: response.isFinished,
              };
            });
          } else {
            alert('Invalid set.');
          }

          // BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD
          if (response.isSet) {
            this.loadGame(this.id);
          }
          // BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD BAD

          this.selectedCards.set([]);
        },
        error: () => {
          alert('Error submitting set. Please try again.');
          this.selectedCards.set([]);
        },
      });
  }

  showHint() {

    if (this.game()?.isFinished) {
      alert('Game is finished. No hints available.');
      return;
    }

    this.gameService.getHint(this.id).subscribe({
      next: (hint) => {
        this.hintedCards.set([hint.card1Id, hint.card2Id]);

        setTimeout(() => {
          this.hintedCards.set([]);
        }, 3000);
      },
      error: () => {
        alert('Error getting hint.');
      },
    });
  }
}
