import { GameStateModel } from './gameState.model';

export interface CheckSetResponseModel {
  isSet: boolean;
  newGameStates: GameStateModel[];
}
