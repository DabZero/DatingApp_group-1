import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { JwtHelperService } from "@auth0/angular-jwt"
import { AuthService } from './_services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();

  constructor(private authService: AuthService) { }

  ngOnInit() {
    var token = localStorage.getItem("token");                            //If token exists in browser persistent storage
    if (token) {                                                          //init the var of authService with browser token string value
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);  //pass token (v) to the decoder method of jwthelper
    }                                                                     //authService var "decodedToken" now has token data
    /* This is needed at app-root so, when app loads, the "decodedToken" is available
    and you can refresh the page w/o losing this data in this authService variable
    --> Only persistant data survives a browser refresh...that is why we are 
    getting the data from local storage on the browser page*/
  }


}
