import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

@Pipe({
  name: 'refactorDate',
  standalone: true,
})
export class RefactorDatePipe implements PipeTransform {
  transform(value: string | Date, format: string = 'dd-MM-yyyy HH:mm'): string {
    if (!value){
      return '';
    }

    const datePipe = new DatePipe('en-US');
    return datePipe.transform(value, format) || '';
  }
}
