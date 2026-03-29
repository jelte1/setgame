import { GameCardStateModel } from './gameCardStateModel';
import { FoundSetModel } from './foundSet.model';

export interface GameModel {
  id: number;
  createdAt: Date;
  isFinished: boolean;
  userId: string;
  gameCardStates: GameCardStateModel[];
  foundSets: FoundSetModel[];
}
