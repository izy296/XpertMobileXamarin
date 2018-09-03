import { AuthServiceProvider } from './../providers/auth-service/auth-service';
import {XHRBackend, Http, RequestOptions} from "@angular/http";
import {HttpInterceptor} from "./httpInterceptor";

export function HttpFactory(xhrBackend: XHRBackend, requestOptions: RequestOptions,authService: AuthServiceProvider): Http {
    return new HttpInterceptor(xhrBackend, requestOptions,authService);
}