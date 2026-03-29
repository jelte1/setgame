import { GameCardStateModel } from './gameCardStateModel';

export interface CheckSetResponseModel {
  isSet: boolean;
  newGameCardStates: GameCardStateModel[];
}
