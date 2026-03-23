import { Component, computed, input, Input } from '@angular/core';

@Component({
  selector: 'app-user',
  templateUrl: './user.html',
  styleUrl: './user.css',
})
export class User {
  @Input({ required: true }) avatar!: string;
  @Input({ required: true }) name!: string;

  imagePath = computed(() => {
    return './assets/img/avatars/' + this.avatar;
  });

  onSelectUser() {}
}
