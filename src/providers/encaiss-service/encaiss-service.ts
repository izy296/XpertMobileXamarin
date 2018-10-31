import { HttpInterceptor } from './../../Module/httpInterceptor';
import { HelperServiceProvider } from '../helper-service/helper-service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Motif } from '../../models/motif';
import { Tiers } from '../../models/tiers';
@Injectable()
export class EncaisseServiceProvider {
  private BASE_URL: string = "api";
  private ENCAISSEMENT_URL: string = "Encaissements";
  private ENCAISSEMENT_Per_Page_URL: string = "EncaissementsPerPage";
  private ADD_ENCAISSEMENT_URL: string = "addEncaissement";
  private DELETE_ENCAISSEMENT_URL: string = "deleteEncaissement";
  private MOTIFS_URL: string = "motifs";
  private CAISSES_URL: string = "caisses";
  private COMPTES_URL: string = "comptes"
  private TIERS_URL: string = "tiers";
  private STATISTIC_URL: string = "statistic";
  private SESSION_URL: string = "session";
  private headers: Headers;
  motifsList = [];
  motifFilter: Motif = {
    CODE_MOTIF: "all",
    DESIGN_MOTIF: "Tous"
  };
  motifListForFilter = [];
  tiersList = [];
  tiersListForForm = [];
  tiersForm: Tiers = {
    CODE_TIERS: null,
    NOM_TIERS: "  ",
    NOM_TIERS1: "  ",
    SOLDE_TIERS: 0
  };
  tiersListForFilter = [];
  tiersFilter: Tiers = {
    CODE_TIERS: "all",
    NOM_TIERS: "Tous",
    NOM_TIERS1: "Tous",
    SOLDE_TIERS: 0
  };
  caissesList = [];
  constructor(
    private http: HttpInterceptor,
    private helperService: HelperServiceProvider ) {
    this.loadCaisses();
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');

  }
  getStatisticEncaiss(date_start, date_end): Observable<any> {   
    return this.http.get(
      this.helperService.createLink(this.BASE_URL,this.ENCAISSEMENT_URL,this.STATISTIC_URL,date_start,date_end)  
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  getComptes(): Observable<any> {
    return this.http.get(
      this.helperService.createLink(this.BASE_URL,this.ENCAISSEMENT_URL,this.COMPTES_URL)
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  getEncaissements(type: string): Observable<any> {
    return this.http.get(
      this.helperService.createLink(this.BASE_URL,type)
    )
      .map(this.helperService.extractData)
      .do(this.helperService.logResponse)
      .catch(this.helperService.catchError)
  }
  getEncaissementsPerPage(type: string, page: number, idCaisse: string, dateDebut: string, dateFin: string, codeTiers: string, codeMotif: string, codeCompte: string): Observable<any> {
    let url: string;  
    if (dateDebut == null && dateFin == null) {
      url = this.helperService.createLink(this.BASE_URL,this.ENCAISSEMENT_Per_Page_URL,type,page,idCaisse);
    }
    else {
      url = this.helperService.createLink(this.BASE_URL,this.ENCAISSEMENT_Per_Page_URL,type,page,idCaisse,dateDebut,dateFin,codeTiers,codeMotif,codeMotif,codeCompte)
    }
    return this.http.get(url)
      .map(this.helperService.extractData)
      .do(this.helperService.logResponse)
      .catch(this.helperService.catchError)
  }
  getMotifs(type: string): Observable<any> {
    return this.http.get(
      this.helperService.createLink(this.BASE_URL,this.ENCAISSEMENT_URL,this.MOTIFS_URL,type)
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  getTiers(): Observable<any> {
    return this.http.get(
      this.helperService.createLink(this.BASE_URL,this.ENCAISSEMENT_URL,this.TIERS_URL)
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  getCaisses(): Observable<any> {
    return this.http.get(
      this.helperService.createLink(this.BASE_URL,this.ENCAISSEMENT_URL,this.CAISSES_URL)
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  getSessions(): Observable<any> {
    const url = this.helperService.createLink(this.BASE_URL,this.SESSION_URL);
    console.log("url session get Sessions :  ",url);
    console.log("http://localhost/xpertpharm.rest.api/api/session/");
    
    return this.http.get(url
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
      this.helperService.createLink(this.BASE_URL,this.ENCAISSEMENT_URL,this.ADD_ENCAISSEMENT_URL), body
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
      this.helperService.createLink(this.BASE_URL,this.ENCAISSEMENT_URL),
      body
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }
  deleteEncaissement(codeEncaiss: string) {
    let body = {
      "CODE_ENCAISS": codeEncaiss
    };
    return this.http.post(
      this.helperService.createLink(this.BASE_URL,this.ENCAISSEMENT_URL,this.DELETE_ENCAISSEMENT_URL), body
    )
      .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch(this.helperService.catchError);
  }

  loadMotifs() {
    this.getMotifs("ENC").subscribe(
      data => {
        this.motifsList = [...data];
        this.motifListForFilter = [...data];
        this.motifListForFilter.unshift(this.motifFilter);
        this.loadTiers();
      },
      error => {
        this.helperService.showNotifError(error)
      });
  }
  loadTiers() {
    this.getTiers().subscribe(
      data => {
        this.tiersListForFilter = [...data];
        this.tiersListForForm = [...data];
        this.tiersListForFilter.unshift(this.tiersFilter);
        this.tiersListForForm.unshift(this.tiersForm);
      },
      error => {
        this.helperService.showNotifError(error)
      });
  }
  loadCaisses() {
    this.getCaisses().subscribe(
      data => {
        this.caissesList = data;
        this.loadMotifs();
      }
      , error => {
        this.helperService.showNotifError(error)
      });
  }


}