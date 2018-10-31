import { HttpInterceptor } from './../../Module/httpInterceptor';
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

  private BASE_URL: string = "api";
  DASHBOARD_URL: string = "Dashboard";
  MARGE_PAR_VENDEUR_URL: string = "MargeParVendeur";
  public labelMargeParvendeur: string[];
  constructor(
    public http: HttpInterceptor,
    private helperService: HelperServiceProvider
  ) {
  }
  public initDashBoard() {

  }
  getMargeParVendeur(date_start: Date, date_end: Date): Observable<any> {

    console.log("url :", this.helperService.networkAddress + this.BASE_URL + this.DASHBOARD_URL + this.MARGE_PAR_VENDEUR_URL + '/' + date_start.toISOString().substring(0, 10) + '/' + date_end.toISOString().substring(0, 10) + '/');
    const url = this.helperService.createLink(this.BASE_URL, this.DASHBOARD_URL, this.MARGE_PAR_VENDEUR_URL, date_start.toISOString().substring(0, 10), date_end.toISOString().substring(0, 10));
    console.log("createLink dashboar durl : ", url);
    return this.http.get(
      url
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
}
