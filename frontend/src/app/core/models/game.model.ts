import { GameStateModel } from './gameState.model';
import { FoundSetModel } from './foundSet.model';

export interface GameModel {
  id: number;
  createdAt: Date;
  isFinished: boolean;
  userId: string;
  gameStates: GameStateModel[];
  foundSets: FoundSetModel[];
}
