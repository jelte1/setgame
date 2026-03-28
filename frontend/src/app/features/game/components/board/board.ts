import { Component, input } from '@angular/core';
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
  cards = input.required<CardModel[]>()

  ngOnInit() {
    console.log(this.cards());
  }
}
