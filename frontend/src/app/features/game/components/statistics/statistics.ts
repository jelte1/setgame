import { Component, input } from '@angular/core';
import { GameModel } from '../../../../core/models/game.model';
import { RefactorDatePipe } from '../../../../core/pipes/refactorDate.pipe';

@Component({
  selector: 'app-statistics',
  imports: [RefactorDatePipe],
  standalone: true,
  templateUrl: './statistics.html',
  styleUrl: './statistics.css',
})
export class Statistics {
  game = input.required<GameModel>();
  tableCards = input.required<number>();
  cardsInDeck = input.required<number>();
}
