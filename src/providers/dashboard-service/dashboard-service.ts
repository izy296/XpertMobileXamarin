import { HttpInterceptor } from './../../Module/httpInterceptor';
import { DashboardPage } from '../../pages/dashboard/dashboard';
import { HelperServiceProvider } from '../helper-service/helper-service';
import { Observable } from 'rxjs/Observable';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';

/*
  Generated class for the StatisticServiceProvider provider.

  See https://angular.io/guide/dependency-injection for more info on providers
  and Angular DI.
*/
@Injectable()
export class DashboardServiceProvider {

  private BASE_URL: string = "XpertPharm.Rest.Api/api/";
  DASHBOARD_URL : string ="Dashboard/";
  MARGE_PAR_VENDEUR_URL : string ="MargeParVendeur";
  public labelMargeParvendeur : string[];
  constructor(
    public http: HttpInterceptor,
    private helperService: HelperServiceProvider
  ) {
    console.log('Hello StatisticServiceProvider Provider');
  }
  public initDashBoard(){
    var newBarChartLabels = [];
    this.getMargeParVendeur().subscribe(data => {
      console.log("dahsboard provider ",data[0].Exercice);
     
      data.forEach(el => {
        if (newBarChartLabels.find(e => e == el.Exercice) == undefined) {
          newBarChartLabels.push(el.Exercice);
        }            
      });
      this.labelMargeParvendeur = newBarChartLabels;
      console.log("dashboard",this.labelMargeParvendeur);
      
    }, (error) => {
      console.log(error);
    });
  }
  getMargeParVendeur(): Observable<any> {
    console.log("url ",this.helperService.networkAddress,this.helperService.networkAddress + this.BASE_URL + this.DASHBOARD_URL + this.MARGE_PAR_VENDEUR_URL);    
    return this.http.get(
      this.helperService.networkAddress + this.BASE_URL + this.DASHBOARD_URL + this.MARGE_PAR_VENDEUR_URL
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
}
