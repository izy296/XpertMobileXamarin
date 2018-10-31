import { EncaissementsPage } from './../encaissements/encaissements';
import { Tiers } from './../../models/tiers';
import { SelectSearchableComponent } from 'ionic-select-searchable';
import { OnInit, ViewChild, ElementRef } from '@angular/core';
import { EncaisseServiceProvider } from './../../providers/encaiss-service/encaiss-service';
import { Component } from '@angular/core';
import { NavController, NavParams, Button } from 'ionic-angular';
import { HelperServiceProvider } from '../../providers/helper-service/helper-service';
import { Motif } from '../../models/motif';
import { NgForm } from '@angular/forms';
import { XpertCurrencyPipe } from '../../pipes/xpert-currency/xpert-currency';
import { TiersServiceProvider } from '../../providers/tiers-service/tiers-service';
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
  btnValidate: Button;
  update: boolean = false;
  method: string = "Ajouter";
  @ViewChild('f') form: NgForm;
  encaissement: any;
  title: string;
  caissesList = [];
  codeCompte: string;
  codeType: string;
  tiers: Tiers;
  motif: Motif;
  listTiers: Tiers[];
  listMotifs: Motif[];
  totalEncaiss: number;
  dateEncaiss: string = new Date().toISOString();
  hours = new Date().getHours();
  encaissementsPage: EncaissementsPage;
  private el: HTMLInputElement;
  public localDate: Date = new Date();
  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    private encaisseService: EncaisseServiceProvider,
    private helperService: HelperServiceProvider,
    private montantInput: ElementRef,
    public xpertPipe: XpertCurrencyPipe,
    private tiersService : TiersServiceProvider
  ) {
    this.encaissementsPage = this.navParams.get('encaissementsPage');
    this.el = this.montantInput.nativeElement;
  }
  ionViewDidLoad() {

  }
  ionViewWillEnter() {

  }
  setDate(date: Date) {
    this.localDate = date;
    this.dateEncaiss = this.localDate.toISOString();
    this.dateEncaiss = this.localDate.toISOString();
  }
  getMotifs() {
    this.listMotifs = this.encaisseService.motifsList;
  }
  getTiers() {
    this.listTiers = this.encaisseService.tiersListForForm;
    this.tiers = this.encaisseService.tiersForm;
    console.log("tier list : ", this.listTiers);
  }
  getCaisses() {
    this.caissesList = this.encaisseService.caissesList;
  }
  goBack() {
    this.navCtrl.pop();
  }
  updateEncaissement() {
    this.encaisseService.updateEncaissement(
      this.encaissement.CODE_ENCAISS,
      this.dateEncaiss,
      this.codeCompte,
      this.totalEncaiss,
      this.codeType,
      this.motif.CODE_MOTIF,
      this.tiers.CODE_TIERS
    ).subscribe(data => {
      if (this.encaissementsPage) {
        this.encaissementsPage.getEncaissementSPerPage();
      }
      this.helperService.showNotifSuccess("l'encaissement a bien etait modifier")
      this.navCtrl.pop();
    }, (error) => {
      this.form.form.enable();
      this.helperService.showNotifError(error)
    });
  }
  addEncaissement() {
    this.encaisseService.addEncaissement(
      this.dateEncaiss,
      this.codeCompte,
      this.totalEncaiss,
      this.codeType,
      this.motif.CODE_MOTIF,
      this.tiers.CODE_TIERS
    ).subscribe(data => {
      if (this.encaissementsPage)
        this.encaissementsPage.getEncaissementSPerPage();
      this.helperService.showNotifSuccess("l'encaissement a bien etait ajouter")
      this.navCtrl.pop();
    }, (error) => {
      this.form.form.enable();
      this.helperService.showNotifError(error)
    });
  }
  ngOnInit() {
    /// test if we are in the update or add Encaissment
    this.update = this.navParams.get('update');
    this.initForm();
    if (this.update) {      
      this.initUpdateForm();
    } else {
      this.codeType = this.navParams.get('type');
      this.tiers = {
        CODE_TIERS: "",
        NOM_TIERS: "",
        NOM_TIERS1: "",
        SOLDE_TIERS: 0
      }
      //this.initAddForm();
    }
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
    this.codeType = this.encaissement.CODE_TYPE;
    this.dateEncaiss = this.encaissement.DATE_ENCAISS;
    this.totalEncaiss = this.encaissement.TOTAL_ENCAISS;
    this.tiers = {
      CODE_TIERS: this.encaissement.CODE_TIERS,
      NOM_TIERS: this.encaissement.NOM_TIERS,
      NOM_TIERS1: this.encaissement.NOM_TIERS1,
      SOLDE_TIERS: this.encaissement.SOLDE_TIERS
    };
    this.codeCompte = this.encaissement.CODE_COMPTE;
    this.motif = {
      CODE_MOTIF: this.encaissement.CODE_MOTIF,
      DESIGN_MOTIF: this.encaissement.DESIGN_MOTIF
    };
    if(this.tiers.CODE_TIERS!="")
    this.getSoldeTiers(this.tiers.CODE_TIERS);
  }
  getSoldeTiers(codeTiers:string){
    this.tiersService.getTiers(codeTiers).subscribe(data => {
      this.tiers = data;      
    }, error => {
      this.helperService.showNotifError(error)
    });
  }
  initTitle() {
    if (this.codeType == 'ENC') {
      this.title = "Encaissement";
    } else {
      this.title = "Decaissement";
    }
  }
  updateAmount(event) {
    console.log("update ", this.xpertPipe.transform(event.target.value));
    event.target.value = this.xpertPipe.transform(event.target.value);
  }
  validate(f) {
    this.form.form.disable();
    console.log(this.form);
    if (this.update) {
      this.updateEncaissement();
    }
    else {
      this.addEncaissement();
    }
  }
  tiersChange(event: { component: SelectSearchableComponent, value: any }) {
    console.log(this.tiers.SOLDE_TIERS);
  }
}