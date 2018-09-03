import { AuthServiceProvider } from './../providers/auth-service/auth-service';

import { ConnectionBackend, RequestOptions, Request, RequestOptionsArgs, Response, Http, Headers } from "@angular/http";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable()
export class HttpInterceptor extends Http {
    
    private token: string = 'token';
    
    constructor(backend: ConnectionBackend, defaultOptions: RequestOptions,public authService :AuthServiceProvider) {
        super(backend, defaultOptions);
        
    }    
   
    
    request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
        return super.request(url, options);
    }

    /* Performs a request with `get` http method.
    * @param url
    * @param options
    * @returns {Observable<>}
    */
    get(url: string, options?: RequestOptionsArgs): Observable<Response> {

        return super.get(url, this.getRequestOptionArgs(options));
    }

    /* Performs a request with `post` http method.
    * @param url
    * @param options
    * @returns {Observable<>}
    */
    post(url: string, body: any, options?: RequestOptionsArgs): Observable<Response> {
        return super.post(url, body, this.getRequestOptionArgs(options));
    }
    /* Performs a request with `put` http method.
    * @param url
    * @param options
    * @returns {Observable<>}
    */
    put(url: string, body: any, options?: RequestOptionsArgs): Observable<Response> {
        return super.put(url, body, this.getRequestOptionArgs(options));
    }

    /* Performs a request with `delete` http method.
    * @param url
    * @param options
    * @returns {Observable<>}
    */
    delete(url: string, options?: RequestOptionsArgs): Observable<Response> {
        return super.delete(url, this.getRequestOptionArgs(options));
    }


    private getRequestOptionArgs(options?: RequestOptionsArgs): RequestOptionsArgs {
        if (options == null) {
            options = new RequestOptions();
        }
        if (options.headers == null) {
            options.headers = new Headers();
        }
        
        
        options.headers.append('Content-Type', 'application/json');
        options.headers.append('Authorization', 'Bearer ' + this.authService.token);
        return options;

    }

    // /**
    //  * Before any Request.
    //  */
    // private beforeRequest(): void {
    //   //this.notifyService.showPreloader();
    // }
    //
    /**
     * After any request.
     */
    private afterRequest(): void {
        //this.notifyService.hidePreloader();
        console.log("after request")
    }

    /**
     * Error handler.
     * @param error
     * @param caught
     * @returns {ErrorObservable}
     */
    // private onCatch(error: any, caught: Observable<any>): Observable<any> {
    //   //this.notifyService.popError();
    //   return Observable.throw(error);
    // }

    /**
     * onSuccess
     * @param res
     */
    // private onSuccess(res: Response): void {
    //   console.log(res);
    // }

    /**
     * onError
     * @param error
     */
    // private onError(error: any): void {
    //   //this.notifyService.popError();
    //   console.log(error);
    // }
    /**
     * onFinally
     */
    // private onFinally(): void {
    //   this.afterRequest();
    // }
}