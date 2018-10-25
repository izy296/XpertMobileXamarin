import { SelectSearchableComponent } from 'ionic-select-searchable';
import { Tiers } from './../../models/tiers';
import { FormEncaissementPage } from './../form-encaissement/form-encaissement';
import { MenuEncaissementListComponent } from './../../components/menu-encaissement-list/menu-encaissement-list';
import { PopoverController, ToastController, Content, FabContainer } from 'ionic-angular';
import { EncaisseServiceProvider } from './../../providers/encaiss-service/encaiss-service';
import { NavParams } from 'ionic-angular';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NavController } from 'ionic-angular';
import { Motif } from '../../models/motif';
import { HelperServiceProvider } from '../../providers/helper-service/helper-service';
import { MenuFilterComponent } from '../../components/menu-filter/menu-filter';
@Component({
    selector: 'page-encaissements',
    templateUrl: 'encaissements.html'
})
export class EncaissementsPage implements OnInit {
    @ViewChild(Content) content: Content;
    encaissementList = [];
    type: string = "All";
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
    motif: Motif = new Motif();
    tiersAll: Tiers = new Tiers();
    listTiers: Tiers[];
    listMotifs: Motif[];
    filter: boolean = false;
    filterActivated = false;
    caissesList = [];
    codeCompte: string = "all";
    idCaisse: string = "all";
    constructor(
        public navCtrl: NavController,
        public navParams: NavParams,
        private encaisseService: EncaisseServiceProvider,
        private popOverCtrl: PopoverController,
        public toastCtrl: ToastController,
        public helperService: HelperServiceProvider
    ) {
                
    }
    scrolling(event) {
        // si le filtre est activer est que on est dans le haut de la page on affiche le menu filtre
        /*   console.log(event);        
          if(this.filterActivated && event.scrollTop<220 && event.directionY =="up"){
              this.showMenuFilter();
          } */
    }
    scrollComplete(event) {
    }
    ionViewWillEnter() {
        this.getEncaissementSPerPage();
        console.log("will enter");
    }
    ionViewDidEnter() {
        console.log("did enter");
    }
    ionViewCanEnter() {
        console.log("can enter");
    }
    // show filter menu
    public showMenuFilter() {
        let menuFilter = this.popOverCtrl.create(MenuFilterComponent,
            {
                dateDebut: this.localDateDebut,
                dateFin: this.localDateFin,
                motif: this.motif,
                tiers: this.tiers,
                codeCompte: this.codeCompte
            }
        );
        menuFilter.present();
        menuFilter.onDidDismiss(data => {
            console.log("data from popover : ", data);
            if (data.filter) {
                this.localDateDebut = data.dateDebut;
                console.log("date debut ", data.dateDebut);

                console.log("date fin ", data.dateFin);

                this.localDateFin = data.dateFin;
                this.tiers = data.tiers;
                this.codeCompte = data.compte;
                this.motif = data.motif;
                this.filter = true;
                this.getEncaissementSPerPage();
            } else {
                this.filter = false;
                this.getEncaissementSPerPage();
            }
        })
    }
    ngOnInit() {
        if(this.navParams.get('id_caisse'))
        this.idCaisse = this.navParams.get('id_caisse');   
    }
    showMenu(event, encaissement) {
        console.log("presss ");
        
        let menu = this.popOverCtrl.create(MenuEncaissementListComponent, { data: encaissement, parent: this });
        menu.present({
            ev: event
        });
    }
    addEncaissementPage(codeType: string, fab: FabContainer) {
        fab.close();
        this.navCtrl.push(FormEncaissementPage, { type: codeType });
    }
    public getEncaissementSPerPage() {        
        this.isLoading = true;
        this.content.scrollToTop();
        this.page = 1;
        if (!this.filter) {
            this.encaisseService.getEncaissementsPerPage(this.type, this.page,this.idCaisse, null, null, "all", "all", "all")
                .subscribe(
                    (data) => {

                        this.data = data;
                        this.isLoading = false;
                        this.encaissementList = this.data;
                    }, (error) => {
                        console.log(error);
                        this.helperService.showNotifError(error)
                    }
                );
        }
        else {
            this.encaisseService.getEncaissementsPerPage(this.type, this.page,this.idCaisse, this.localDateDebut.toDateString(), this.localDateFin.toDateString(), this.tiers.CODE_TIERS, this.motif.CODE_MOTIF, this.codeCompte)
                .subscribe(
                    (data) => {
                        this.data = data;
                        this.isLoading = false;
                        this.encaissementList = this.data;
                    }, (error) => {
                        console.log(error);
                        this.helperService.showNotifError(error)
                    }
                );
        }
    }
    async doInfinite(infiniteScroll) {
        this.page = this.page + 1;
        if (!this.filter) {
            setTimeout(() => {
                this.encaisseService.getEncaissementsPerPage(this.type, this.page, this.idCaisse,null, null, "all", "all", "all")
                    .subscribe(
                        (data) => {
                            this.data = data;
                            for (let i = 0; i < this.data.length; i++) {
                                this.encaissementList.push(this.data[i]);
                            }
                            console.log(this.encaissementList.length);
                        },
                        (error) => this.helperService.showNotifError(error));

                console.log('Async operation has ended');
                infiniteScroll.complete();
            }, 1000);
        }
        else {
            setTimeout(() => {
                this.encaisseService.getEncaissementsPerPage(this.type, this.page ,this.idCaisse, this.localDateDebut.toDateString(), this.localDateFin.toDateString(), this.tiers.CODE_TIERS, this.motif.CODE_MOTIF, this.codeCompte)
                    .subscribe(
                        (data) => {
                            this.data = data;
                            for (let i = 0; i < this.data.length; i++) {
                                this.encaissementList.push(this.data[i]);
                            }
                            console.log(this.encaissementList.length);
                        },
                        (error) => {
                            this.helperService.showNotifError(error)
                        }
                    );

                infiniteScroll.complete();
            }, 1000);
        }
    }
    tiersChange(event: { component: SelectSearchableComponent, value: any }) {
    }
}
