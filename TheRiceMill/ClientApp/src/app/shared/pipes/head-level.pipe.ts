import { Pipe, PipeTransform } from '@angular/core';
import { HeadLevel } from '../model/enums';

@Pipe({
  name: 'headLevel'
})
export class HeadLevelPipe implements PipeTransform {

  transform(value: number): string {
    switch (value) {
      case HeadLevel.None:
        return 'None';
      case HeadLevel.Level1:
        return 'Head 1';
      case HeadLevel.Level2:
        return 'Head 2';
      case HeadLevel.Level3:
        return 'Head 3';
      case HeadLevel.Level4:
        return 'Head 4';
      case HeadLevel.Level5:
        return 'Head 5';
    }
  }

}
