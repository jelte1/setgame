import { CardModel } from './card.model';

export interface FoundSetModel {
  id: number;
  gameId: number;
  card1: CardModel;
  card2: CardModel;
  card3: CardModel;
}
