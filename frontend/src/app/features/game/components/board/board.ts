import { Component, input, output } from '@angular/core';
import { CardModel } from '../../../../core/models/card.model';
import { Card } from '../card/card';

@Component({
  selector: 'app-board',
  standalone: true,
  imports: [Card],
  templateUrl: './board.html',
  styleUrl: './board.css',
})
export class Board {
  cards = input.required<CardModel[]>();
  selectedCards = input.required<CardModel[]>();
  cardClicked = output<CardModel>();

  ngOnInit() {
  }

  isSelected(card: CardModel): boolean {
    return this.selectedCards().some((c) => c.id === card.id);
  }
}
