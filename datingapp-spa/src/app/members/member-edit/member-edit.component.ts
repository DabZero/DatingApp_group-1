import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
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
  /*  static - True to resolve query results before change detection runs, 
false to resolve after change detection. Defaults to false.*/
  @HostListener("window:beforeunload", ["$event"])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      //before the document is about to be unloaded (close the browser window) return a browser pop-up
      $event.returnValue = true;
    }
  }


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
