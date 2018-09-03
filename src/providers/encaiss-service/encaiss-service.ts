import { Http } from '@angular/http';
import { HttpInterceptor } from './../../Module/httpInterceptor';
import { AuthServiceProvider } from '../auth-service/auth-service';
import { HelperServiceProvider } from '../helper-service/helper-service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
@Injectable()
export class EncaisseServiceProvider {
  private BASE_URL: string = "XpertPharm.Rest.Api/api/";
  private ENCAISSEMENT_URL: string = "Encaissements/";
  private ENCAISSEMENT_Per_Page_URL: string = "EncaissementsPerPage/";
  private ADD_ENCAISSEMENT_URL: string = "addEncaissement";
  private DELETE_ENCAISSEMENT_URL: string = "deleteEncaissement";
  private MOTIFS_URL: string = "motifs/";
  private CAISSES_URL: string = "caisses";
  private COMPTES_URL: string = "comptes"
  private TIERS_URL: string = "tiers";
  private STATISTIC_URL: string = "statistic/";
  private headers: Headers;
  motifsList = [];
  tiersList = [];
  caissesList = [];
  constructor(
    private http: HttpInterceptor,
    private helperService: HelperServiceProvider,
    private authService: AuthServiceProvider) {
    this.loadCaisses();
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');
  }
  getStatisticEncaiss(date_start, date_end): Observable<any> {
   
    return this.http.get(
      this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.STATISTIC_URL + date_start + "/" + date_end
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  getComptes(): Observable<any> {
    return this.http.get(
      this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.COMPTES_URL
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  getEncaissements(type: string): Observable<any> {
    return this.http.get(
      this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + type

    )
      .map(this.helperService.extractData)
      .do(this.helperService.logResponse)
      .catch(this.helperService.catchError)
  }
  getEncaissementsPerPage(type: string, page: number, dateDebut: string, dateFin: string, codeTiers: string, codeMotif: string, codeCompte: string): Observable<any> {
    let url: string;
    if (dateDebut == null && dateFin == null) {
      url = this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_Per_Page_URL + type + "/" + page + "/";
      console.log(url);
      
    }
    else {
      url = this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_Per_Page_URL + type + "/" + page + "/" + dateDebut + "/" + dateFin + "/" + codeTiers + "/" + codeMotif + "/" + codeCompte;
    }
    console.log("At encaiss service",this.authService.getToken());    
    return this.http.get(url)
      .map(this.helperService.extractData)
      .do(this.helperService.logResponse)
      .catch(this.helperService.catchError)
  }
  getMotifs(type: string): Observable<any> {
    return this.http.get(
      this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.MOTIFS_URL + type
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  getTiers(): Observable<any> {
    return this.http.get(
      this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.TIERS_URL
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  getCaisses(): Observable<any> {
    return this.http.get(
      this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.CAISSES_URL
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  addEncaissement(dateEncaiss, codeCompte, totalEncaiss, codeType, codeMotif, codeTiers) {
    let body = {
      "DATE_ENCAISS": dateEncaiss,
      "CODE_COMPTE": codeCompte,
      "TOTAL_ENCAISS": totalEncaiss,
      "CODE_TYPE": codeType,
      "CODE_MOTIF": codeMotif,
      "CODE_TIERS": codeTiers
    };
    return this.http.post(
      this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.ADD_ENCAISSEMENT_URL, body
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }

  updateEncaissement(codeEncaiss, dateEncaiss, codeCompte, totalEncaiss, codeType, codeMotif, codeTiers) {
    let body = {
      "CODE_ENCAISS": codeEncaiss,
      "DATE_ENCAISS": dateEncaiss,
      "CODE_COMPTE": codeCompte,
      "TOTAL_ENCAISS": totalEncaiss,
      "CODE_TYPE": codeType,
      "CODE_MOTIF": codeMotif,
      "CODE_TIERS": codeTiers
    };
    return this.http.put(
      this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL,
      body
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  deleteEncaissement(codeEncaiss: string) {
    console.log("in the method delete");
    
    let body = {
      "CODE_ENCAISS": codeEncaiss
    };
    return this.http.post(
      this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.DELETE_ENCAISSEMENT_URL, body
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }

  loadMotifs() {
    this.getMotifs("ENC").subscribe(
      data => {
        this.motifsList = data;
        this.loadTiers();
      },
      error => {
        console.log(error);
        this.loadMotifs();
      });
  }
  loadTiers() {
    this.getTiers().subscribe(
      data => {
        this.tiersList = data;
      },
      error => {
        console.log(error);
      });

  }
  loadCaisses() {
    this.getCaisses().subscribe(
      data => {
        this.caissesList = data;
        this.loadMotifs();
      }
      , error => {
        console.log(error);
      });

  }

}