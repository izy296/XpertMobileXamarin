import { Component, OnInit } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { Observable } from 'rxjs/Observable';
/**
 * Generated class for the EncaissementPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
@Component({
  selector: 'page-encaissement',
  templateUrl: 'encaissement.html',
})
export class EncaissementPage implements OnInit {
  encaissement: any;
  Object = Object;
  constructor(public navCtrl: NavController, public navParams: NavParams) {
  }
  ionViewDidLoad() {
  }
  ngOnInit() {
    this.encaissement = this.navParams.data;
  }

}
