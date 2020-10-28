import { Component, OnInit } from '@angular/core';
import { User } from "../../_models/user";
import { UserService } from "../../_services/user.service";
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination } from 'src/app/_models/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  userParams: any = {};
  genderList: [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' }
  ];
  pagination: Pagination;



  constructor(private service: UserService,
    private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    //  route.data = stream from resolvers observable Api call, access by data['prop'] or data.prop
    //-- This is using route resolver to grab the data and make it avaialable prior to component load
    //-- since data is a PaginatedResult<T> we can use its properties (result, pagination)  
    //-- this is syntax to get data|url|snapshot|params from the resolver.  Must access using ['prop'] from routes.ts
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });

    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';  //default ordering from Api

  }
  // When bottom page pagination links are clicked
  //
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  resetFilters() {
    this.userParams.gender = (this.user.gender === 'female') ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.loadUsers();
  }


  /*  Get data before the route is activated
    replacing loadUser() w/ routeResolver to get the user + user.id and assigning to user:User
    --see MemberDetailResolver resolve(route: ActivatedRouteSnapshot): Observable<User>
    */
  loadUsers() {

    return this.service.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe(
        //returns (res: PaginatedResult<User[]>)  
        (res) => {
          this.users = res.result;
          this.pagination = res.pagination;
        },

        error => { this.alertify.error(error) }
      );
  }

}
