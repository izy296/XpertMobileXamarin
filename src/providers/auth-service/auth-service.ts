import { Storage } from '@ionic/storage';
import { Observable } from 'rxjs';
import { HelperServiceProvider } from '../helper-service/helper-service';
import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';

/*
  Generated class for the AuthServiceProvider provider.
  See https://angular.io/guide/dependency-injection for more info on providers
  and Angular DI.
*/
@Injectable()
export class AuthServiceProvider {
  private BASE_URL: string = "api";
  private TOKEN_URL: string = "token";
  private TEST_URL: string = "test";
  private ACCOUNT_DETAIL_URL:string = "getAccountDetail";
  private GRANT_TYPE: string = "password";
  public token = "token";
  private headers: Headers;
  public authorisationOptions: RequestOptions;
  TOKEN_KEY: string = 'token';
  constructor(
    private http: Http,
    private helperService: HelperServiceProvider,
    private storage: Storage) {
  }
  getAuthentification(username: string, password: string) {
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');
    return this.http.post(this.helperService.networkAddress+this.BASE_URL+"/"+this.TOKEN_URL, 'username=' + username + '&password=' + password + '&grant_type=' + this.GRANT_TYPE
    ).do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch((error) => {
        console.log(error);
        return Observable.throw(error.json().error || "Server error");
      })
  }
  public async setToken(token: string) {
    this.token = token;
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');
    this.headers.append('Authorization', 'bearer ' + this.token);
    this.authorisationOptions = new RequestOptions({ headers: this.headers });
    console.log('Token set ', this.getToken());
    await this.storage.set(this.TOKEN_KEY, this.token);  
  }
  
  public testConnexion() {
    return this.http.get(
      this.helperService.createLink(this.BASE_URL,this.TEST_URL))
    .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch((error) => {
        console.log(error);
        return Observable.throw(error.json().error || "Server error");
      })
  }
  public getAccountDetail() {
    return this.http.get(
      this.helperService.createLink(this.BASE_URL,this.ACCOUNT_DETAIL_URL))
    .do(this.helperService.logResponse)
      .map(this.helperService.extractData)
      .catch((error) => {
        console.log(error);
        return Observable.throw(error.json().error || "Server error");
      })
  }
  public getToken(): string {
    console.log("log in getToken");
    return this.token;
  }
  getAuthorisationToken(): RequestOptions {
    return this.authorisationOptions;
  }

}
