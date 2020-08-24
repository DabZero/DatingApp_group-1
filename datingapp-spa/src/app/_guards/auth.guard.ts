import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from "../_services/auth.service";
import { AlertifyService } from "../_services/alertify.service";


@Injectable({
  providedIn: 'root'
})
//  Protection for url path routes.  Has a single method (t/f) to verify if this path can be accessed or not
//
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService,
    private alertify: AlertifyService, private router: Router) { }

  canActivate(): boolean {              //Method call of canActivate true = user can proceed

    if (this.authService.loggedin())    //Call service's method to check if tokens exist for login  
      return true;                      //if true this means user has logged in

    else {
      this.alertify.error("Area:  Access denied");
    }
  }

}
