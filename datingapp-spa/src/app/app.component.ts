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
    var user = JSON.parse(localStorage.getItem("user"));                  //This is a string in local storage.  We need to need change to
    /*                                                                      object b/c we are setting/init currentUser: User; in AuthService*/
    if (token) {                                                          //init the var of authService with browser token string value
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);  //pass token (v) to the decoder method of jwthelper
    }                                                                     //authService var "decodedToken" now has token data
    if (user) {
      this.authService.currentUser = user;
      this.authService.changeMemberPhoto(user.photoUrl);
    }
    /* This is needed at app-root so, when app loads, the "decodedToken" is available
    We are setting the value of decodedToken: any; in the AuthService variable.  Remember, 
    in the response stream, we set the token to local storage.  Now we are getting that value
    and setting this value to the variable in AuthService (both token and user)
    Now you can refresh the page w/o losing this data in this authService variable
    --> Only persistanted data survives a browser refresh...that is why we are 
    getting the data from local storage on the browser page*/
  }


}
