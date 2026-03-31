import { Component, computed, input } from '@angular/core';
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

    // When filling is Empty or null, return 'none' to indicate no fill
    return 'none';
  }

  amount = computed(() =>
    Array(this.card().amount)
      .fill(0)
      .map((_, i) => i),
  );
}
