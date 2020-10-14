import { Component, OnInit } from '@angular/core';
import { User } from "../../_models/user";
import { UserService } from "../../_services/user.service";
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];

  constructor(private service: UserService,
    private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    //  this.loadUsers(); 
    //-- This is using route resolver to grab the data and make it avaialable prior to component load
    //--since data is a PaginatedResult<T> we need to specify the body data held in "result"
    this.route.data.subscribe(data => { this.users = data['users'].result; })
  }



}
/*  Get data before the route is activated
  replacing loadUser() w/ routeResolver to get the user + user.id and assigning to user:User
  --see MemberDetailResolver resolve(route: ActivatedRouteSnapshot): Observable<User>
  loadUsers() {

    return this.service.getUsers().subscribe(

      (users: User[]) => { this.users = users },

      error => { this.alertify.error(error) }
    );
  }
  */

