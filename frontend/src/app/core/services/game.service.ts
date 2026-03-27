import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { Observable } from 'rxjs';
import { GameModel } from '../models/game.model';

@Injectable({providedIn: 'root'})
export class GameService extends BaseApiService {

  getUserGames(): Observable<GameModel[]> {
    return this.http.get<GameModel[]>(`${this.apiUrl}/games`);
  }

}
