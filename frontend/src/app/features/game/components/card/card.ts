import { Component, computed, input } from '@angular/core';
import { CardModel, Color, Filling, Shape } from '../../../../core/models/card.model';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-card',
  standalone: true,
  templateUrl: './card.html',
  styleUrl: './card.css',
})
export class Card {
  card = input.required<CardModel>();
  selected = input(false);

  amount = computed(() =>
    Array(this.card().amount)
      .fill(0)
      .map((_, i) => i),
  );

  shapeHref = computed(() => `#card_${Shape[this.card().shape]}`);

  strokeValue = computed(() => Color[this.card().color].toLowerCase());

  fillValue = computed(() => {
    const color = Color[this.card().color].toLowerCase();
    switch (this.card().filling) {
      case Filling.Solid:
        return color;
      case Filling.Empty:
        return 'none';
      case Filling.Striped:
        return `url(#stripes-${color})`;
    }
  });

  ngOnInit() {
    console.log('card: ' + this.card().amount);
    console.log('func: ' + this.amount);
  }
}
