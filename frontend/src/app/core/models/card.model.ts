export enum Shape { Oval, Diamond, Squiggle }

export enum Color { Red, Purple, Green }

export enum Filling { Solid, Striped, Empty }

export enum Amount { One = 1, Two = 2, Three = 3 }

export interface CardModel {
  id: number;
  shape: Shape;
  color: Color;
  filling: Filling;
  amount: Amount;
}
