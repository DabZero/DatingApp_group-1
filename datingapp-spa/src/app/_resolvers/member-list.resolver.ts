import { Injectable } from "@angular/core";
import { User } from "../_models/user";
import { Resolve, ActivatedRouteSnapshot, Router } from "@angular/router";
import { UserService } from "../_services/user.service";
import { AlertifyService } from "../_services/alertify.service";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";


@Injectable()
export class MemberListResolver implements Resolve<User[]>{
    pageNumber = 1;
    pageSize = 5;

    constructor(private userService: UserService,
        private router: Router, private alertify: AlertifyService) { }

    //{ path: "members", component: MemberListComponent, resolve: { users: MemberListResolver } },
    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        return this.userService.getUsers(this.pageNumber, this.pageSize)
            .pipe(              //pipe is used here just to deal with potentail error
                catchError(
                    error => {
                        this.alertify.error("Problem retreiving data");
                        this.router.navigate([""])
                        return of(null);
                    })
            )
    }
    /*Get the user that Matches the route params we expect to get
    Return an error and redirect to another page if we have a problem 
    if no problem, continue to the route that is being activated
    GOAL - Get the data from the route vs. going to userService to get it*/
}


