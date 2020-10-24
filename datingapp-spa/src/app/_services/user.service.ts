import { Injectable } from '@angular/core';
import { environment } from "../../environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from "../_models/user";
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { parseI18nMeta } from '@angular/compiler/src/render3/view/i18n/meta';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  //apiUrl: "http://localhost:5000/api/"
  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsers(page?, itemsPerpage?, userParams?): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (page != null && itemsPerpage != null) {

      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerpage);
    }

    if (userParams != null) {
      params = params.append("minAge", userParams.minAge);
      params = params.append("maxAge", userParams.maxAge);
      params = params.append("gender", userParams.gender);
    }

      return this.http.get<User[]>(this.baseUrl + "users", { observe: "response", params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"));
          }
          return paginatedResult;
        }
        )
      );
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + "users/" + id);
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }

  setMainPhoto(userId: number, photoId: number): Observable<Object> {
    return this.http.post(this.baseUrl + "users/" + userId + "/photos/" + photoId + "/setmain", {});
  }

  deletePhoto(userId: number, photoId: number) {
    return this.http.delete(this.baseUrl + "users/" + userId + "/photos/" + photoId)
  }
}

