import { PipeTransform, Pipe } from '@angular/core';
import { GatePassType } from '../model/enums';

@Pipe({
  name: 'gatepassType'
})
export class GatepassTypePipe implements PipeTransform {
  transform(value: number) {
    switch (value) {
      case GatePassType.InwardGatePass:
        return 'IGP';
      case GatePassType.OutwardGatePass:
        return 'OGP';
    }
  }
}
