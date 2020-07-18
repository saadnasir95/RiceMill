import { PipeTransform, Pipe } from '@angular/core';
import { GateinDirection } from '../model/enums';

@Pipe({
  name: 'gateinDirection'
})
export class GateinDirectionPipe implements PipeTransform {
  transform(value: number) {
    switch (value) {
      case GateinDirection.Milling:
        return 'Milling';
      case GateinDirection.Outside:
        return 'Outside';
      case GateinDirection.Stockpile:
        return 'Stockpile';
    }
  }
}
