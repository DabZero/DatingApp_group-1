import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from 'rxjs/operators';

@Injectable()   //Notes at bottom page
export class ErrorInterceptor implements HttpInterceptor {
    intercept(                                              
        req: HttpRequest<any>,
        next: HttpHandler): Observable<HttpEvent<any>>     
    {
        return next.handle(req).pipe(
            catchError(httpErrorResp => {
                if (httpErrorResp.status == 401) {
                    return throwError(httpErrorResp.statusText);    //Throw err back to component
                }
                if (httpErrorResp instanceof HttpErrorResponse) {  //Our custom error, match from API
                    const applicationError = httpErrorResp.headers.get("Application-Error");
                    if (applicationError) {
                        return throwError(applicationError);        //Throw (K,V) value we custom assigned
                    }
                    const serverError = httpErrorResp.error;
                    let modelStateErrors = "";                      //data attribute errors from our DTO
                    if (serverError.errors && typeof serverError.errors === "object") {
                        for (const key in serverError.errors) {
                            if (serverError.errors[key]) {
                                modelStateErrors += serverError.errors[key] + "\n";
                            }
                        }
                    }
                    return throwError(modelStateErrors || serverError || "server Error");
                }
            })
        )
    }
}
export const ErrorInterceptorProvider = {   //<--Register this Name in providers:[] of app.module (root module)
    provide: HTTP_INTERCEPTORS,             //standard Angular provider
    useClass: ErrorInterceptor,             //Use my custom version ^ of the standard provider
    multi: true                             //By default Interceptors are multi-providers ..can't change this
};

/*
Http Interceptors = Global capture/Inspect & modify Http requests
                           capture/Inspect & modify Http response

Requirements:   @Injectable, implements HttpInterceptor, implement intercept method
                export this class using (provide, useclass, multi)

req:  HttpRequest<any> = Param to catch any outgoing requests, deal with what should happen next
next: HttpHandler Obs<HttpEvent<any>> = Passes req to the processing pipeline

              req/resp                   req/resp
(Angular App) ------>    (Interceptor)   ------->     (Server)
              <------                    <-------

1.  You can modify the headers of your http request
2.  Handle errors in a single location for the entire (Angular-client) app

---1st If statement ...This is the browser error -console - log
HttpErrorResponse {headers: HttpHeaders, status: 400, statusText: "Bad Request", url: "http://localhost:5000/api/auth/register", ok: false, …}
error: {type: "https://tools.ietf.org/html/rfc7231#section-6.5.1", title: "One or more validation errors occurred.", status: 400, traceId: "|df820397-422eb6b0aef3982a.", errors: {…}}
headers: HttpHeaders {normalizedNames: Map(0), lazyUpdate: null, lazyInit: ƒ}
message: "Http failure response for http://localhost:5000/api/auth/register: 400 Bad Request"
name: "HttpErrorResponse"
ok: false
status: 400
statusText: "Bad Request"
url: "http://localhost:5000/api/auth/register"

---2nd If statement ....This is the browser error       -log
HttpErrorResponse {headers: HttpHeaders, status: 500, statusText: "Internal Server Error", url: "http://localhost:5000/api/auth/login", ok: false, …}
error: "500 Computer says no"
                                                        -network  -headers
Request URL: http://localhost:5000/api/auth/login
Request Method: POST
Status Code: 500 Internal Server Error
Remote Address: [::1]:5000
Referrer Policy: no-referrer-when-downgrade
Access-Control-Allow-Origin: *
Access-Control-Expose-Headers: Application-Error
Application-Error: 500 Computer says no
Cache-Control: no-cache

--3rd If Statement... This is the browser error       -log
HttpErrorResponse {headers: HttpHeaders, status: 400, statusText: "Bad Request", url: "http://localhost:5000/api/auth/register", ok: false, …}
error:
  errors: {Password: Array(1), Username: Array(1)}     < ---This is an object holding 2 arrays, data attribute errors from our DTO
  status: 400
  title: "One or more validation errors occurred."


*/