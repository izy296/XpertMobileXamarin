import { EncaisseServiceProvider } from './../../providers/encaiss-service/encaiss-service';
import { Component, ViewChild, OnInit } from '@angular/core';
import { NavController, PopoverController } from 'ionic-angular';
import { Chart } from 'chart.js';
import { DashboardServiceProvider } from '../../providers/dashboard-service/dashboard-service';
import { IonPullUpFooterState } from 'ionic-pullup';
import { HelperServiceProvider } from '../../providers/helper-service/helper-service';
import { DataChart } from '../../models/datachart';
/**
 * Generated class for the DashboardPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'page-dashboard',
  templateUrl: 'dashboard.html',
})

export class DashboardPage implements OnInit {

  @ViewChild('margeCanvas') margeCanvas;
  margeChart: any;
  @ViewChild('totaleVenteCanvas') totaleVenteCanvas;
  totaleVenteChart: any;
  @ViewChild('totaleAchatCanvas') totaleAchatCanvas;
  totalAchatChart: any;
  isLoading: boolean = true;
  raccourci_date: string = 'annee';
  public localDateDebut: Date = new Date();
  public localDateFin: Date = new Date();
  dateDebut: string = null;
  dateFin: string = null;
  encaissementChart: any;
  encaissementData = [];
  footerState: IonPullUpFooterState;
  dateNow: Date = new Date();
  BACKGROUND_COLOR = ['rgb(255, 0, 255)', 'rgb(255, 0, 0)', 'rgb(128, 0, 0)', 'rgb(128, 0, 128)', 'rgb(0, 0, 255)', 'rgb(0, 128, 128)', 'rgb(0, 255, 0)', 'rgb(0, 128, 0)', 'rgb(25, 128, 50)'];
  ngOnInit(): void {
    this.localDateDebut.setFullYear(this.localDateDebut.getFullYear() - 1);
    const dateNow: Date = new Date();
    this.margeChart = this.createEmptyChart(this.margeCanvas);
    this.totalAchatChart = this.createEmptyChart(this.totaleAchatCanvas);
    this.totaleVenteChart = this.createEmptyChart(this.totaleVenteCanvas);
    this.setDataChart();
  }
  footerExpanded() {
    console.log('Footer expanded!');
  }
  footerCollapsed() {
    console.log('Footer collapsed!');
  }
  toggleFooter() {
    this.footerState = this.footerState == IonPullUpFooterState.Collapsed ? IonPullUpFooterState.Expanded : IonPullUpFooterState.Collapsed;
  }
  constructor(
    public nav: NavController,
    public popoverCtrl: PopoverController,
    public dashboardService: DashboardServiceProvider,
    public encaissementService: EncaisseServiceProvider,
    public helperService: HelperServiceProvider
  ) {
    this.footerState = IonPullUpFooterState.Collapsed;

  }
  onRaccourciDateChange() {    
    this.localDateDebut = new Date();
    this.localDateFin = new Date();
    switch (this.raccourci_date) {
      case 'jour': {
        console.log("Jour :  date debut", this.localDateDebut);
        console.log("Jour : date fin ", this.localDateFin);

        break;
      }
      case 'mois': {        
        this.localDateDebut.setMonth(this.localDateDebut.getMonth() - 1);
        console.log("Mois : dateDebut    ", this.localDateDebut);
        console.log("Mois : dateFin   ", this.localDateFin);

        break;
      }
      case 'annee': {
        this.localDateDebut.setFullYear(this.localDateDebut.getFullYear() - 1);
        console.log("Anne :    ", this.localDateDebut);
        console.log("Anne :    ", this.localDateFin);


        break;
      }
    }
    this.setDataChart();
  }
  setDateDebut(date: Date) {
    this.localDateDebut = date;
    this.dateDebut = this.localDateDebut.toISOString();
    this.setDataChart();

  }
  setDateFin(date: Date) {
    this.localDateFin = date;
    this.dateFin = this.localDateFin.toISOString();
    this.setDataChart();
  }
  setDataChart() {
    this.isLoading = true;
    console.log("data debut", this.localDateDebut.toISOString());
    console.log("data Fin", this.localDateFin.toISOString());
    this.dashboardService.getMargeParVendeur(this.localDateDebut, this.localDateFin).subscribe((data) => {
      let dataChartMarge: DataChart = new DataChart(data, "CREATED_BY", "Sum_MARGE");
      let dataChartTotaleVente: DataChart = new DataChart(data, "CREATED_BY", "Sum_TOTAL_VENTE");
      let dataChartTotaleAchat: DataChart = new DataChart(data, "CREATED_BY", "Sum_TOTAL_ACHAT");
      //Marge par Vendeur
      this.updateChart(this.margeChart, dataChartMarge);

      // Totale Vente
      this.updateChart(this.totaleVenteChart, dataChartTotaleVente);
      // totale achat
      this.updateChart(this.totalAchatChart, dataChartTotaleAchat);
      this.isLoading = false;

    }, error => {
      this.helperService.showNotifError('dashboard MargeVendeur :' + error)
    })
  }
  updateChart(chart, dataChart: DataChart) {
    this.removeDataChart(chart);
    chart.data.labels = dataChart.labels;
    chart.data.datasets.forEach((dataset) => {
      dataset.data = dataChart.values;
      dataset.backgroundColor = this.BACKGROUND_COLOR;
    });
    console.log("the chart after updated ", chart);
    chart.update();
  }
  removeDataChart(chart) {
    chart.data.labels.pop();
    chart.data.datasets.forEach((dataset) => {
      dataset.data.pop();
    });
  }
  createChart(canvas: any, dataChart: DataChart) {
    return new Chart(canvas.nativeElement, {
      type: 'horizontalBar',
      data: {
        labels: (dataChart.labels.length > 0) ? dataChart.labels : ["none"],
        datasets: [{
          data: (dataChart.values.length > 0) ? dataChart.values : [0],
          backgroundColor: this.BACKGROUND_COLOR
        }]
      }, options: {
        legend: {
          display: false,
        }
      }
    });
  }
  createEmptyChart(canvas) {
    return new Chart(canvas.nativeElement, {
      type: 'horizontalBar',
      data: {
        labels: ["none"],
        datasets: [{
          data: [0],
          backgroundColor: this.BACKGROUND_COLOR
        }]
      }, options: {
        legend: {
          display: false,
        }
      }
    });
  }

  ionViewDidLoad() {
  }
}
