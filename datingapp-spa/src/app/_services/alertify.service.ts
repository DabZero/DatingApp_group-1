import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  confirm(message: string, OkCallback: () => any) {

    alertify.confirm(message, (evnt: any) => {

      if (evnt) OkCallback();                //ok=if user clicks ok -> callback ...define in components

      else { }                               //Do nothing, cxl the callback, cxl the confirm
    });
  }

  success(message: string) {                //Pass hard coded value from a component while calling this function       
    alertify.success(message);              //Assume you have injected this class into constructor
  }                                         //if method in component just does stuff ...this.alertify.xxx("pass hard coded msg");
  /*                                          if stream .subscribe( next=> { this.alertify.xxx("pass hard coded msg");     */
  error(message: string) {                  //                      error => { this.alertify.error(error)  ...pass error from stream
    alertify.error(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }
  message(message: string) {
    alertify.message(message);
  }
}
