import { Component, signal } from '@angular/core';
import { Header } from '../header/header';
import { CardModel } from './core/models/card.model';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [Header, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('frontend');
  cards = [new CardModel(1, 1, 2, 2, 3)];
}
