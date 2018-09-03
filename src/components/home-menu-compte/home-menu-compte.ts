import { HomePage } from './../../pages/home/home';
import { NavParams } from 'ionic-angular';
import { App, NavController, ModalController, ViewController } from 'ionic-angular';
import { Component } from '@angular/core';
@Component({
  template: `
    <ion-list>
      <button ion-item  (click)="modifier()">Modifier </button>      
    </ion-list>
  `
})
export class HomeMenuCompteComponent {
  constructor(
    public viewCtrl: ViewController,
    public navCtrl: NavController,
    public app: App,
    public modalCtrl: ModalController,
    public navParams: NavParams,
  ) {

  }
  showDetail() {
    this.navCtrl.push(HomePage);
  }
  updateEncaissement() {
    this.navCtrl.push(HomePage);
  }
  deleteEncaissement() {
    console.log("log delete");
  }
  modifier(){

  }
}