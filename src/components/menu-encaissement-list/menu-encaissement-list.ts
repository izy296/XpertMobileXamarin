import { EncaissementsPage } from './../../pages/encaissements/encaissements';
import { ToastController } from 'ionic-angular';
import { EncaisseServiceProvider } from './../../providers/encaiss-service/encaiss-service';
import { FormEncaissementPage } from './../../pages/form-encaissement/form-encaissement';
import { EncaissementPage } from './../../pages/encaissements/encaissement/encaissement';
import { NavParams, AlertController } from 'ionic-angular';
import { Component } from '@angular/core';
import { App, NavController, ModalController, ViewController } from 'ionic-angular';
@Component({
  template: `
    <ion-list>
      <button ion-item  (click)="updateEncaissement()">Modifier</button>
      <button ion-item (click) ="deleteEncaissement()">Supprimer</button>
      <button ion-item (click)="showDetail()">Detail</button>
    </ion-list>
  `
})
export class MenuEncaissementListComponent {
  encaissement: any;
  encaissementsPage : EncaissementsPage;
  constructor(
    public viewCtrl: ViewController,
    public navCtrl: NavController,
    public app: App,
    public modalCtrl: ModalController,
    public navParams: NavParams,
    public alertCtrl: AlertController,
    public encaisseService: EncaisseServiceProvider,
    public toastCtrl: ToastController
  ) {
    this.encaissement = navParams.get('data');
    this.encaissementsPage = navParams.get('parent');     
  }
  showDetail() {
    this.navCtrl.push(EncaissementPage, this.encaissement);
  }
  updateEncaissement() {
    this.navCtrl.pop();
    this.navCtrl.push(FormEncaissementPage, { 'encaissement': this.encaissement, 'update': true,'encaissementsPage':this.encaissementsPage });
  }
  deleteEncaissement() {
    const confirm = this.alertCtrl.create({
      message: 'voulez-vous supprimer cet encaissement ?',
      buttons: [
        {
          text: 'ok',
          handler: () => {
            this.encaisseService.deleteEncaissement(this.encaissement.CODE_ENCAISS).subscribe((data) => {
              this.toastCtrl.create(
                {
                  message: "l'encaisseemnt a etait bien supprimer'",
                  duration: 5000,
                  position: 'bottom',
                  closeButtonText: 'OK',
                  showCloseButton: true
                }
              ).present();
            }, (error) => {
              this.toastCtrl.create(
                {
                  message: "Erreur : " + error + "est survenue",
                  duration: 5000,
                  position: 'bottom',
                  closeButtonText: 'OK',
                  showCloseButton: true
                }
              ).present();
              console.log(error);
            });
          }
        },
        {
          text: 'Annuler',
          handler: () => {

          }
        }
      ]
    });
    confirm.present();
  }
}