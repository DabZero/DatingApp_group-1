import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from "rxjs/operators";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  baseUrl: string = "http://localhost:5000/api/auth/";

  constructor(private http: HttpClient) { }

  login(model: any): Observable<void> {
    return this.http.post(this.baseUrl + "login", model)
      .pipe(
        map((response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem("token", user.token);
          }
        })  //--map transform one-at-a-time
      ); //--pipe transform to Observable
  }

  register(model: any): Observable<object> {
    return this.http.post(this.baseUrl + "register", model);
  }
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
