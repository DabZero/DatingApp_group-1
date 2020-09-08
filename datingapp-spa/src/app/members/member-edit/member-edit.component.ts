import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute, RoutesRecognized } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;
  @ViewChild("editForm", { static: true }) editForm: NgForm;

  /*Property decorator that configures a view query. 
  The change detector looks for the first element or the directive 
  matching the selector in the view DOM. If the view DOM changes, 
  and a new child matches the selector, the property is updated.
  Metadata Properties:

selector - The directive type or the name used for querying.
read - Used to read a different token from the queried elements.
static - True to resolve query results before change detection runs, false to resolve after change detection. Defaults to false.*/

  constructor(private route: ActivatedRoute, //<-- data (Url path) from Routes.ts -> resolver()
    private alertify: AlertifyService) { }

  ngOnInit(): void {

    this.route.data.subscribe(data => {
      this.user = data['user']; //access key of route resolver resolve: { user: MemberEditResolver } 
    })
  }

  updateUser() {

    this.alertify.success("Successful update");
    console.log(this.user);
    this.editForm.reset(this.user);
  }

}
