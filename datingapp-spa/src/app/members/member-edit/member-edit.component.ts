import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute, RoutesRecognized } from '@angular/router';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;

  constructor(private route: ActivatedRoute) { } //<-- data (Url path) from Routes.ts -> resolver()

  ngOnInit(): void {

    this.route.data.subscribe(data => {
      this.user = data['user']; //access key of route resolver resolve: { user: MemberEditResolver } 
    })
  }

}
