import { DashboardServiceProvider } from './../../providers/dashboard-service/dashboard-service';
import { DatePickerDirective } from 'ionic3-datepicker';
import { FormEncaissementPage } from './../form-encaissement/form-encaissement';
import { ToastController } from 'ionic-angular';
import { HelperServiceProvider } from './../../providers/helper-service/helper-service';
import { EncaisseServiceProvider } from './../../providers/encaiss-service/encaiss-service';
import { Component, ViewChild, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams, PopoverController } from 'ionic-angular';
/**
 * Generated class for the DashboardPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
@IonicPage()
@Component({
  selector: 'page-dashboard',
  templateUrl: 'dashboard.html',
})
export class DashboardPage implements OnInit {

  @ViewChild(DatePickerDirective) private datepickerDirective: DatePickerDirective;
  MENU_COMPTE: string = "COMPTE";
  dateNow: string = new Date().toISOString();
  public doughnutChartLabels: string[] = ['Decaissement', 'Encaissement',];
  public doughnutChartData: number[] = [0, 0];
  public doughnutChartType: string = 'doughnut';
  public dataCompte = [];
  public datesStatistic = this.helperService.datesStatistic;
  public localDate: Date = new Date();
  public initDate: Date = new Date();
  public initDate2: Date = new Date(2015, 1, 1);
  public disabledDates: Date[] = [new Date(2017, 7, 14)];
  public dateEncaiss;
  public maxDate: Date = new Date(new Date().setDate(new Date().getDate() + 30));
  public min: Date = new Date();  
  constructor(
    public nav: NavController,
    public popoverCtrl: PopoverController,
    private encaisseService: EncaisseServiceProvider,
    public helperService: HelperServiceProvider,
    public toastCtrl: ToastController,
    public dashboardService: DashboardServiceProvider
  ) {
  }
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  public labelsMargeParVendeur : string[] = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
  public dataMargeParVendeur  : any[] =  [
    {data: [65, 59, 80, 81, 56, 55, 40], label: 'Series A'},
    {data: [28, 48, 40, 19, 86, 27, 90], label: 'Series B'}
  ];
  public barChartLabels: string[] = this.dashboardService.labelMargeParvendeur;
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;
  public barChartData: any[] = [{ data: [0, 0], label: '' },
  { data: [0, 0], label: '' }];
  public closeDatepicker() {
    this.datepickerDirective.modal.dismiss();
  }
  ionViewWillEnter() {
    this.sync();
  }
  public Log(stuff): void {
    console.log(stuff);
  }
  public event(data: Date): void {
    this.dateEncaiss = data;
    this.localDate = data;
  }
  setDate(date: Date) {
    console.log(date);
    this.localDate = date;
  }

  public sync() {
    this.datesStatistic = this.helperService.datesStatistic;
    this.setDataChart();
  }
  public chartClicked(e: any): void {
  }
  public chartHovered(e: any): void {
  }
  ngOnInit(): void {
    var newBarChartData = [];
    var newBarChartLabels = [];
    this.setDataChart();
    console.log("dashboard");
    this.dashboardService.getMargeParVendeur().subscribe(data => {
      console.log(data);
      
      data.forEach(el => {
        if (newBarChartData.find(e => e.label == el.CREATED_BY) == undefined) {
          newBarChartData.push({ data: [], label: el.CREATED_BY });
        }
        newBarChartData.find(e => e.label == el.CREATED_BY).data.push(el.Sum_MARGE);
      });
      this.barChartLabels = newBarChartLabels;
      this.barChartData = newBarChartData;

    }, (error) => {
      console.log(error);
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
      let toast = this.toastCtrl.create({
        message: 'Erreur Statistique Encaisseement ' + error,
        duration: 3000,
        position: 'bottom',
        cssClass: 'dark-trans',
        closeButtonText: 'OK',
        showCloseButton: true
      });
      toast.present();
    });
  }
  addEncaissementPage(codeType: string) {
    this.nav.push(FormEncaissementPage, { type: codeType });
    this.sync();
  }

}
