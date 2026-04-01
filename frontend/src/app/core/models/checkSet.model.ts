import { GameCardStateModel } from './gameCardState.model';
import { FoundSetModel } from './foundSet.model';

export interface CheckSetModel {
  isSet: boolean;
  newGameCardStates: GameCardStateModel[];
  foundSet: FoundSetModel;
  possibleSetsCount: number;
}
