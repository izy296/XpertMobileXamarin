import { NavController } from 'ionic-angular';
import { HelperServiceProvider } from './../../providers/helper-service/helper-service';
import { Component } from '@angular/core';

/**
 * Generated class for the MenuSommaireComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */
@Component({
  selector: 'menu-sommaire',
  templateUrl: 'menu-sommaire.html'
})
export class MenuSommaireComponent {


  text: string;
  dateDebut: string = new Date().toISOString();
  dateFin: string = new Date().toISOString();
  constructor(
    public helperService: HelperServiceProvider,
    public navCtrl: NavController,

  ) {
  }
  add() {
    console.log(this.dateDebut, this.dateFin);
    this.helperService.saveDateStatistic(this.dateDebut, this.dateFin);
    this.navCtrl.pop();
  }
  onChange(event) {
    console.log(event);
  }
}
