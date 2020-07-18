import { PipeTransform, Pipe } from '@angular/core';
import { GatepassType } from '../model/enums';

@Pipe({
  name: 'gatepassType'
})
export class GatepassTypePipe implements PipeTransform {
  transform(value: number) {
    switch (value) {
      case GatepassType.Gatein:
        return 'Gatein';
      case GatepassType.Gateout:
        return 'Gateout';
    }
  }
}
