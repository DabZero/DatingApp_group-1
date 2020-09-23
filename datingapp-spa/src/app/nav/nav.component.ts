import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { stringify } from '@angular/compiler/src/util';
import { AlertifyService } from "../_services/alertify.service";
import { Router } from '@angular/router';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string;

  constructor(public authService: AuthService, private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit(): void {
    this.authService.photoUrl.subscribe(p => this.photoUrl = p);
  }

  login(): void {
    this.authService.login(this.model)
      .subscribe(
        next => {
          this.alertify.success("Logged in successfully");
          //            this.router.navigate(['/members'])    next=Req is sucessful + Resp has data from api
        },                                                  //route to this url after all successful req/resp
        error => { this.alertify.error(error); },
        () => { this.router.navigate(['/members']); }      //alternate use complete():void => after sucess req/resp
      );
  }
  // If user is logged in, the token will exist in local storage + show welcome message
  //
  loggedIn(): boolean {

    return this.authService.loggedin();
    // const token = localStorage.getItem("token");
    //   return !!token; 
  }



  // To log the user out, we remove token from local storage + show login Nav
  //
  logout(): void {

    localStorage.removeItem("token");
    localStorage.removeItem("user");
    this.authService.decodedToken = null;     //remove values populated from local storage
    this.authService.currentUser = null;
    this.alertify.message("logged out");
    this.router.navigate(['']);
  }
}
