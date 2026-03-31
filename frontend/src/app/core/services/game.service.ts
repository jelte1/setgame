import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { Observable } from 'rxjs';
import { GameModel } from '../models/game.model';
import { BaseGameModel } from '../models/baseGame.model';
import { CheckSetResponseModel } from '../models/checkSetResponseModel';

@Injectable({ providedIn: 'root' })
export class GameService extends BaseApiService {
  getUserGames(): Observable<BaseGameModel[]> {
    return this.http.get<BaseGameModel[]>(`${this.apiUrl}/games`);
  }

  getGameById(id: number): Observable<GameModel> {
    return this.http.get<GameModel>(`${this.apiUrl}/games/${id}`);
  }

  checkSet(
    gameId: number,
    cardId1: number,
    cardId2: number,
    cardId3: number,
  ): Observable<CheckSetResponseModel> {
    return this.http.post<CheckSetResponseModel>(`${this.apiUrl}/games/${gameId}/checkset`, {
      card1Id: cardId1,
      card2Id: cardId2,
      card3Id: cardId3,
    });
  }

  createGame(): Observable<BaseGameModel> {
    return this.http.post<BaseGameModel>(`${this.apiUrl}/games`, {})
  }
}
