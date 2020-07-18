import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
import 'moment-timezone';
@Pipe({
  name: 'localDatetime'
})
export class LocalDatetimePipe implements PipeTransform {

  transform(value: any): string {
    return moment.utc(value).tz('Asia/Karachi').format('DD-MMM-YYYY hh:mm a');
  }

}
