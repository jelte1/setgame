import { Component, input } from '@angular/core';
import { CardModel, Color, Filling, Shape } from '../../../../core/models/card.model';

@Component({
  selector: 'app-card',
  standalone: true,
  templateUrl: './card.html',
  styleUrl: './card.css',
})
export class Card {
  card = input.required<CardModel>();
  selected = input(false);
  hinted = input(false);

  getCardShape(): string {
    return Shape[this.card().shape];
  }

  getCardColor(): string {
    return Color[this.card().color].toLowerCase();
  }

  getCardFilling(): string {
    if (this.card().filling === Filling.Solid) {
      return this.getCardColor();
    } else if (this.card().filling === Filling.Striped) {
      return `url(#stripes-${this.getCardColor()})`;
    }

    // When filling is "Empty" or null, return 'none' to indicate no fill
    return 'none';
  }

  // create a array with the length of the amount of the card, so we can use it in the template to repeat the shapes
  getCardAmount(): number[] {
    // fill the array with the numbers from 0 to the amount in the card to repeat the shapes
    return Array(this.card().amount)
      .fill(0)
      .map((_, i) => i);
  }
}
