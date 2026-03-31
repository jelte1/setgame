import { GameCardStateModel } from './gameCardStateModel';
import { FoundSetModel } from './foundSet.model';

export interface CheckSetResponseModel {
  isSet: boolean;
  newGameCardStates: GameCardStateModel[];
  foundSet: FoundSetModel;
}
