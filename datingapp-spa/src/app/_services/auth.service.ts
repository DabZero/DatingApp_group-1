import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from "rxjs/operators";
import { Observable } from 'rxjs';
import {BehaviorSubject} from "rxjs";
import { JwtHelperService } from "@auth0/angular-jwt"
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  baseUrl: string = environment.apiUrl + "auth/";
  jwtHelper = new JwtHelperService();

  //this.decodedToken.exp , this.decodedToken.iat, this.decodedToken.nbf, this.decodedToken.nameid =user.id
  decodedToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>("../../assets/user.png");
  currentPhotoUrl: Observable<string> = this.photoUrl.asObservable();

  constructor(private http: HttpClient) { }

  changeMemberPhoto(photoUrl:string){
    this.photoUrl.next(photoUrl);
  }

  login(model: any): Observable<void> {
    return this.http.post(this.baseUrl + "login", model)
      .pipe(
        map((response: any) => {                        //incomming resp is a bearer (token: "token string")
          const user = response;                        //user = this response (k,V)
          if (user) {                                   //set token in same format on the browser local storage
            //user.token = response.key
            localStorage.setItem("token", user.token);
            //user.user = response.key (comes as an object from AuthController login OK resonse)
            localStorage.setItem("user", JSON.stringify(user.user))
            this.decodedToken = this.jwtHelper.decodeToken(user.token);     //decode (v) token string and hold as var
            this.currentUser = user.user;
            this.changeMemberPhoto(this.currentUser.photoUrl);
            console.log(this.decodedToken);                                 //to see the fields of jwt decode method in browser
            /* console.log("user: " + this.decodedToken.unique_name);    --> fields comes from authController ...claims + tokenDescriptor
             this.decodedToken.exp , this.decodedToken.iat, this.decodedToken.nbf, this.decodedToken.nameid =user.id*/
          }
        })  //--map transform one-at-a-time
      ); //--pipe transform to Observable
  }

  register(model: any): Observable<object> {
    return this.http.post(this.baseUrl + "register", model);
  }

  loggedin() {

    const token = localStorage.getItem("token");
    return !this.jwtHelper.isTokenExpired(token);     //If null/expired=true (!) --> returns false
  }                                                   //default(!true) so that Nav can display user/pass fields to login
  /*                                                    Once logged in -->returns true and ngIf no loger display Nav form */
}





/* Example of what you would have to put into every component vs. centralizing here
* values: any;
*   getvalues(){
     this.http.get('http://localhost:5000/api/auth').subscribe
        (resp => {
          this.values = resp;
          },
            error => { console.log(error);

        }); <--End of the subscribe to stream/ catch the response
*   }

*/
