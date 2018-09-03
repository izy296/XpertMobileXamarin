import { Pipe, PipeTransform, NgModule } from '@angular/core';
import { DecimalPipe } from '@angular/common';

/**
 * Generated class for the XpertCurrencyPipe pipe.
 *
 * See https://angular.io/api/core/Pipe for more info on Angular Pipes.
 */
@Pipe({
  name: 'xpertCurrency',
})
@NgModule({

})
export class XpertCurrencyPipe extends DecimalPipe implements PipeTransform {
  /**
   * Mise en forme du chiffre =>  "0.00"
   */
  transform(value: string, ...args) {
    // si chiffre null renvoyer 0 sionon Mettre en forme
    return (value!=null) ? super.transform(value, '1.2-2').replace(/,/g, ' ') : "0.00";

  }
}
