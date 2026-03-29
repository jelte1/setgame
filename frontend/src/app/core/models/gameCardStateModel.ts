import { CardModel } from './card.model';

export enum CardLocation { Deck, Table, Discarded }

export interface GameCardStateModel {
  id: number;
  location: CardLocation;
  order: number;
  card: CardModel;
}
