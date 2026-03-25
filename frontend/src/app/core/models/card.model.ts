export enum Shape { 'Oval' = 1, 'Diamond' = 2, 'Squiggle' = 3 }

export enum Color { Red = 1, Purple = 2, Green = 3 }

export enum Filling { Solid = 1, Striped = 2, Empty = 3 }

export enum Amount { One = 1, Two = 2, Three = 3 }

export class CardModel {
  id: number;
  shape: Shape;
  color: Color;
  filling: Filling;
  amount: Amount;

  constructor(id: number, shape: Shape, color: Color, filling: Filling, amount: Amount) {
    this.id = id;
    this.shape = shape;
    this.color = color;
    this.filling = filling;
    this.amount = amount;
  }
}
