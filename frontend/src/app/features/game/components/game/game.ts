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
import { CheckSetModel } from '../../../../core/models/checkSet.model';
import { FoundSets } from '../found-sets/found-sets';

@Component({
  selector: 'app-game',
  standalone: true,
  imports: [Board, RouterLink, RefactorDatePipe, FoundSets],
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
      },
      error: () => {
        console.error('Error loading game');
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
            console.log(response);
            alert('bingo!');
            this.applySetResponse(response);
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

  private applySetResponse(response: CheckSetModel) {
    this.game.update((currentGame) => {
      if (!currentGame) {
        throw new Error('Game not loaded');
      }

      const discardedIds = new Set([
        response.foundSet.card1.id,
        response.foundSet.card2.id,
        response.foundSet.card3.id,
      ]);

      const remainingStates = currentGame.gameCardStates.filter(
        (gcs) => !discardedIds.has(gcs.card.id),
      );

      const newCardIds = new Set(response.newGameCardStates.map((state) => state.id));
      const gameCardStatesWithoutNew = remainingStates.filter((gcs) => !newCardIds.has(gcs.id));

      return {
        ...currentGame,
        gameCardStates: [...gameCardStatesWithoutNew, ...response.newGameCardStates],
        foundSets: [...currentGame.foundSets, response.foundSet],
        possibleSetsCount: response.possibleSetsCount,
        isFinished: response.isFinished,
      };
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
