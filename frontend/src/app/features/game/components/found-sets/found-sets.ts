import { Component, input } from '@angular/core';
import { FoundSetModel } from '../../../../core/models/foundSet.model';
import { Card } from '../card/card';

@Component({
  selector: 'app-found-sets',
  imports: [Card],
  templateUrl: './found-sets.html',
  styleUrl: './found-sets.css',
})
export class FoundSets {
  foundSets = input.required<FoundSetModel[]>();
}
