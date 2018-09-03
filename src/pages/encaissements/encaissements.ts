import { SelectSearchableComponent } from 'ionic-select-searchable';
import { Tiers } from './../../models/tiers';
import { FormEncaissementPage } from './../form-encaissement/form-encaissement';
import { MenuEncaissementListComponent } from './../../components/menu-encaissement-list/menu-encaissement-list';
import { PopoverController, ToastController, Content } from 'ionic-angular';
import { EncaissementPage } from './encaissement/encaissement';
import { EncaisseServiceProvider } from './../../providers/encaiss-service/encaiss-service';
import { NavParams } from 'ionic-angular';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NavController } from 'ionic-angular';
@Component({
    selector: 'page-encaissements',
    templateUrl: 'encaissements.html'
})
export class EncaissementsPage implements OnInit {
    @ViewChild(Content) content: Content;
    encaissementList = [];
    type: string = "ENC";
    isLoading: boolean = true;
    data: any;
    page = 1;
    perPage = 0;
    totalData = 0;
    totalPage = 0;
    public localDateDebut: Date = new Date();
    public localDateFin: Date = new Date();
    hours = new Date().getHours();
    dateDebut: string = null;
    dateFin: string = null;
    tiers: Tiers = new Tiers();
    listTiers: Tiers[];
    filter: boolean = false;
    caissesList = [];
    motifsList = [];
    codeMotif: string = "all";
    codeCompte: string = "all";
    constructor(
        public navCtrl: NavController,
        public navParams: NavParams,
        private encaisseService: EncaisseServiceProvider,
        private popOverCtrl: PopoverController,
        public toastCtrl: ToastController

    ) {

    }
    ionViewWillEnter() {
        this.getEncaissementSPerPage();
        console.log("will enter");
        
    }
    ionViewDidEnter() {  
        console.log("did enter");           
    }
    ionViewCanEnter(){
        console.log("can enter");
        
    }
   
    getCaisses() {
        this.caissesList = this.encaisseService.caissesList;
    }
    // show filter menu
    public showMenuFilter() {
        this.filter = !(this.filter);
        if (this.filter == false) {
            //this.getEncaissementsPerPage();
        }
    }
    public filterEncaiss() {
        this.getEncaissementSPerPage();
    }
    // get the detail of encaiss and show it in EncaissementPage
    getEncaissementDetail(encaissement) {
        this.navCtrl.push(EncaissementPage, encaissement);
    }
    showMenu(event, encaissement) {
        let menu = this.popOverCtrl.create(MenuEncaissementListComponent, { data: encaissement,parent:this });
        menu.present({
            ev: event
        });
    }
    ngOnInit() {
        this.getMotifs();
        this.getTiers();
        this.getCaisses();
    }
    getTiers() {
        this.listTiers = this.encaisseService.tiersList;
    }
    getMotifs() {
        this.motifsList = this.encaisseService.motifsList;
        console.log(this.motifsList);
    }
    addEncaissementPage(codeType: string) {
        this.navCtrl.push(FormEncaissementPage, { type: codeType });
    }
    setDateDebut(date: Date) {
        this.localDateDebut = date;
        this.dateDebut = this.localDateDebut.toISOString();
        console.log(this.localDateDebut.setHours(this.hours));
        this.dateDebut = this.localDateDebut.toISOString();
    }
    setDateFin(date: Date) {
        this.localDateFin = date;
        this.dateFin = this.localDateFin.toISOString();
        console.log(this.localDateFin.setHours(this.hours));
        this.dateFin = this.localDateFin.toISOString();
    }
    public getEncaissementSPerPage() {
        console.log("this getEncaissement Per Page");        
        this.isLoading = true;
        this.content.scrollToTop();
        this.page = 1;
        if (!this.filter) {
            this.encaisseService.getEncaissementsPerPage(this.type, this.page, null, null, this.tiers.CODE_TIERS, this.codeMotif, this.codeCompte)
                .subscribe(
                    (data) => {
                        this.data = data;
                        console.log("encaissement", data);
                        this.isLoading = false;

                        this.encaissementList = this.data;
                    }, (error) => {
                        console.log(error);
                    }
                );
        }
        else {
            this.encaisseService.getEncaissementsPerPage(this.type, this.page, this.localDateDebut.toDateString(), this.localDateFin.toDateString(), this.tiers.CODE_TIERS, this.codeMotif, this.codeCompte)
                .subscribe(
                    (data) => {
                        this.data = data;
                        this.isLoading = false;
                        this.encaissementList = this.data;
                    }, (error) => {
                        console.log(error);
                    }
                );
        }
    }
    async doInfinite(infiniteScroll) {
        this.page = this.page + 1;
        if (!this.filter) {
            setTimeout(() => {
                this.encaisseService.getEncaissementsPerPage(this.type, this.page, null, null, this.tiers.CODE_TIERS, this.codeMotif, this.codeCompte)
                    .subscribe(
                        (data) => {
                            this.data = data;
                            for (let i = 0; i < this.data.length; i++) {
                                this.encaissementList.push(this.data[i]);
                            }
                        },
                        error => console.log(error));

                console.log('Async operation has ended');
                infiniteScroll.complete();
            }, 1000);
        }

        else {
            setTimeout(() => {
                this.encaisseService.getEncaissementsPerPage(this.type, this.page, this.localDateDebut.toDateString(), this.localDateFin.toDateString(), this.tiers.CODE_TIERS, this.codeMotif, this.codeCompte)
                    .subscribe(
                        (data) => {
                            this.data = data;
                            for (let i = 0; i < this.data.length; i++) {
                                this.encaissementList.push(this.data[i]);
                            }
                        },
                        error => console.log(error));

                console.log('Async operation has ended');
                infiniteScroll.complete();
            }, 1000);
        }
    }
    tiersChange(event: { component: SelectSearchableComponent, value: any }) {
    }
}
