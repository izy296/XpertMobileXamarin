import { FormEncaissementPage } from './../form-encaissement/form-encaissement';
import { HelperServiceProvider } from './../../providers/helper-service/helper-service';
import { EncaisseServiceProvider } from './../../providers/encaiss-service/encaiss-service';
import { Component, OnInit, ViewChild } from "@angular/core";
import { NavController, PopoverController, ToastController, FabContainer } from "ionic-angular";
import { DatePickerDirective } from 'ionic3-datepicker';
import { EncaissementsPage } from '../encaissements/encaissements';
@Component({
  selector: 'page-home',
  templateUrl: 'home.html',
  providers: [DatePickerDirective]
})

export class HomePage implements OnInit {
  @ViewChild(DatePickerDirective) private datepickerDirective: DatePickerDirective;
  MENU_COMPTE: string = "COMPTE";
  dateNow: string = new Date().toISOString();
  public doughnutChartLabels: string[] = ['Decaissement', 'Encaissement',];
  public doughnutChartData: number[] = [0, 0];
  public doughnutChartType: string = 'doughnut';
  public dataCompte = [];
  public session = [];
  public datesStatistic = this.helperService.datesStatistic;
  public localDate: Date = new Date();
  public initDate: Date = new Date();
  public initDate2: Date = new Date(2015, 1, 1);
  public disabledDates: Date[] = [new Date(2017, 7, 14)];
  public dateEncaiss;
  public maxDate: Date = new Date(new Date().setDate(new Date().getDate() + 30));
  public min: Date = new Date();
  public showSession : boolean = false;
  public showEncaissement:boolean = false;
  constructor(
    public nav: NavController,
    public popoverCtrl: PopoverController,
    private encaisseService: EncaisseServiceProvider,
    public helperService: HelperServiceProvider,
    public toastCtrl: ToastController
  ) {
  }
  public closeDatepicker() {
    this.datepickerDirective.modal.dismiss();
  }

  ionViewWillEnter() {
    this.sync();
  }
  public Log(stuff): void {
  }

  public event(data: Date): void {
    this.dateEncaiss = data;
    this.localDate = data;
  }
  setDate(date: Date) {
    this.localDate = date;
  }

  public sync() {
    this.datesStatistic = this.helperService.datesStatistic;
    this.getStatistic();
    this.setDataChart();
  }
  public chartClicked(e: any): void {
  }
  public chartHovered(e: any): void {
  }

  ngOnInit(): void {
    this.getSession();
    this.setDataChart();
    this.getCompte();
    this.datesStatistic = this.helperService.datesStatistic;
    if (this.helperService.datesStatistic != null) {
      this.getStatistic();
    }
  }
  
  getCompte() {
    this.encaisseService.getComptes().subscribe((data) => {
      this.dataCompte = data;
      console.log(this.dataCompte);
    }, (error) => {
      this.helperService.showNotifError(error)
    })
  }  
  getStatistic() {
    this.helperService.datesStatistic.map(async e => {
      this.encaisseService.getStatisticEncaiss(e.dateDebut.substring(0, 10), e.dateFin.substring(0, 10)).subscribe((result) => {
        if (result != null) {
          result.map(el => {
            switch (el.CODE_TYPE) {
              case 'ENC':
                e.ENC = (el.TOTAL_ENCAISS == null) ? 0 : el.TOTAL_ENCAISS;
                break;
              case 'DEC':
                e.DEC = (el.TOTAL_ENCAISS == null) ? 0 : el.TOTAL_ENCAISS;
                break;
            }
          });
        }
      }, (error) => {
        this.helperService.showNotifError("statistique "+error)
      });
    });
  }
  setDataChart() {
    let data = [0, 0];
    this.encaisseService.getStatisticEncaiss(this.dateNow.substring(0, 10), this.dateNow.substring(0, 10)).subscribe((result) => {
      if (result != null) {
        result.map(e => {
          switch (e.CODE_TYPE) {
            case 'ENC':
              data[1] = (e.TOTAL_ENCAISS == null) ? 0 : e.TOTAL_ENCAISS;
              break;
            case 'DEC':
              data[0] = (e.TOTAL_ENCAISS == null) ? 0 : e.TOTAL_ENCAISS;
              break;
          }
          this.doughnutChartData = data
        })
      }
    }, (error) => {
      this.helperService.showNotifError(error)
    });
  }
  addEncaissementPage(codeType: string,fab:FabContainer) {
    fab.close();
    this.nav.push(FormEncaissementPage, { type: codeType });
    this.sync();
  }
  showSessionDetail(id_caisse){
    this.nav.setRoot(EncaissementsPage,{id_caisse : id_caisse});
  }
  getSession(){
    this.encaisseService.getSessions().subscribe((data) => {
      console.log("session  : ------------",data); 
      this.session = data;
      this.showSession = true;
      console.log("showSession --------------------> ",this.showSession);
      
    }, error => this.helperService.showNotifError("session : "+error))
  }
}

//
