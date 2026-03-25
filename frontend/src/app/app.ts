import { Component, signal } from '@angular/core';
import { Header } from '../header/header';
import { Board } from './features/game/components/board/board';
import { CardModel } from './core/models/card.model';
import { Login } from './features/auth/components/login/login';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [Header, Board, Login],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('frontend');
  cards = [new CardModel(1, 1, 2, 2, 3)];
}
