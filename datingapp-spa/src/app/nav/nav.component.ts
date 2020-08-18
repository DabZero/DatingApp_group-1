import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { stringify } from '@angular/compiler/src/util';
import { AlertifyService } from "../_services/alertify.service";


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit(): void {
  }

  login(): void {
    this.authService.login(this.model)
      .subscribe(
        next => { this.alertify.success("Logged in successfully"); },
        error => { this.alertify.error(error); }
      );
  }
  // If user is logged in, the token will exist in local storage + show welcome message
  //
  loggedIn(): boolean {

    const token = localStorage.getItem("token");
    return !!token;
  }

  // To log the user out, we remove token from local storage + show login Nav
  //
  logout(): void {

    localStorage.removeItem("token");
    this.alertify.message("logged out");
  }
}
