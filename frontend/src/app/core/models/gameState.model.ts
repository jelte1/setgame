import { CardModel } from './card.model';

export enum CardLocation { Deck, Table, Discarded }

export interface GameStateModel {
  id: number;
  location: CardLocation;
  order: number;
  card: CardModel;
}
