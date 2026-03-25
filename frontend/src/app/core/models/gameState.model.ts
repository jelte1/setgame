import { GameModel } from './game.model';

export enum CardLocation { Deck, Table, Discarded }

export interface GameStateModel {
  id: number;
  location: CardLocation;
  order: number;
  card: CardLocation;
}
