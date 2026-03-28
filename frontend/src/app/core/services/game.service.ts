import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { Observable } from 'rxjs';
import { GameModel } from '../models/game.model';
import { BaseGameModel } from '../models/baseGame.model';

@Injectable({ providedIn: 'root' })
export class GameService extends BaseApiService {

  getUserGames(): Observable<BaseGameModel[]> {
    return this.http.get<BaseGameModel[]>(`${this.apiUrl}/games`);
  }

  getGameById(id: number): Observable<GameModel> {
    return this.http.get<GameModel>(`${this.apiUrl}/games/${id}`);
  }
}
