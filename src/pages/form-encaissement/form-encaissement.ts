import { EncaissementsPage } from './../encaissements/encaissements';
import { Tiers } from './../../models/tiers';
import { SelectSearchableComponent } from 'ionic-select-searchable';
import { OnInit } from '@angular/core';
import { EncaisseServiceProvider } from './../../providers/encaiss-service/encaiss-service';
import { Component } from '@angular/core';
import { NavController, NavParams, ToastController } from 'ionic-angular';

/**
 * Generated class for the FormEncaissementPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
@Component({
  selector: 'page-form-encaissement',
  templateUrl: 'form-encaissement.html',
})
export class FormEncaissementPage implements OnInit {
  myInput;
  update: boolean = false;
  method: string = "Ajouter";
  motifsList = [];
  encaissement: any;
  title: string;
  caissesList = [];
  codeMotif: string;
  codeCompte: string;
  codeType: string;
  tiers: Tiers ;
  listTiers: Tiers[];
  totalEncaiss: number;
  dateEncaiss: string = new Date().toISOString();
  hours = new Date().getHours();
  encaissementsPage : EncaissementsPage;
  public localDate: Date = new Date();

  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    private encaisseService: EncaisseServiceProvider,
    private toastCtrl: ToastController
  ) {
    this.encaissementsPage = this.navParams.get('encaissementsPage');
    console.log("modification page : ", this.encaissementsPage.page);    
  }
  ionViewDidLoad() {

  }
  ionViewWillEnter() {

  }
  setDate(date: Date) {
    this.localDate = date;
    this.dateEncaiss = this.localDate.toISOString();
    console.log(this.localDate.setHours(this.hours));
    this.dateEncaiss = this.localDate.toISOString();
  }
  getMotifs() {
    this.motifsList = this.encaisseService.motifsList;
  }
  getTiers() {
    this.listTiers = this.encaisseService.tiersList;
  }
  getCaisses() {
    this.caissesList = this.encaisseService.caissesList;
  }
  goBack(){
    this.navCtrl.pop();
    console.log("go back");
    
  }
  updateEncaissement() {
    this.encaisseService.updateEncaissement(
      this.encaissement.CODE_ENCAISS,
      this.dateEncaiss,
      this.codeCompte,
      this.totalEncaiss,
      this.codeType,
      this.codeMotif,
      this.tiers.CODE_TIERS
    ).subscribe(data => {
      this.encaissementsPage.getEncaissementSPerPage();
      this.navCtrl.pop();
    }, (error) => {
      let toast = this.toastCtrl.create({
        message: 'Erreur ' + error,
        duration: 3000,
        position: 'bottom',
        cssClass: 'dark-trans',
        closeButtonText: 'OK',
        showCloseButton: true
      });
      toast.present();
    });
  }

  addEncaissement() {
    this.encaisseService.addEncaissement(
      this.dateEncaiss,
      this.codeCompte,
      this.totalEncaiss,
      this.codeType,
      this.codeMotif,
      this.tiers.CODE_TIERS
    ).subscribe(data => {
      this.encaissementsPage.getEncaissementSPerPage();
      this.navCtrl.pop();
    });
  }
  ngOnInit() {
    /// test if we are in the update or add Encaissment
    this.update = this.navParams.get('update');
    if (this.update) {
      this.initUpdateForm();
    } else {
      this.codeType = this.navParams.get('type');
      //this.initAddForm();
    }
    this.initForm();
  }
  initForm() {
    this.initTitle();
    this.getMotifs();
    this.getTiers();
    this.getCaisses();
  }
  initUpdateForm() {
    this.method = 'Modifier';
    this.encaissement = this.navParams.get('encaissement');
    console.log('--------------init update FORM ------------------');
    console.log(this.encaissement);
    console.log('---------------------------------');   
    this.codeType = this.encaissement.CODE_TYPE;
    this.dateEncaiss = this.encaissement.DATE_ENCAISS;
    this.totalEncaiss = this.encaissement.TOTAL_ENCAISS;
    this.tiers = {
      CODE_TIERS : this.encaissement.CODE_TIERS,
      NOM_TIERS : this.encaissement.NOM_TIERS
    };
    this.codeCompte = this.encaissement.CODE_COMPTE;
    this.codeMotif = this.encaissement.CODE_MOTIF;
  }
  initTitle() {
    if (this.codeType == 'ENC') {
      this.title = "Encaissement";
    } else {
      this.title = "Decaissement";
    }
  }
  validate() {
    if (this.update) {
      this.updateEncaissement();
    }
    else {
      this.addEncaissement();
    }
  }
  tiersChange(event: { component: SelectSearchableComponent, value: any }) {
  }
}