import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { RouterModule } from "@angular/router";
import { JwtModule } from "@auth0/angular-jwt";

import { AuthService } from "./_services/auth.service";
import { AppComponent } from './app.component';
import { ValueComponent } from './value/value.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from "./_services/error.interceptor";
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListComponent } from './list/list.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from "./routes";
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDeailComponent } from "./members/member-deail/member-deail.component";

export function tokenGetter() {
  return localStorage.getItem("token");
}

@NgModule({
  declarations: [
    AppComponent,
    ValueComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    ListComponent,
    MessagesComponent,
    MemberCardComponent,
    MemberDeailComponent

  ],
  imports: [
    FormsModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    RouterModule.forRoot(appRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5000"],
        //disallowedRoutes: ["localhost:5000/api/auth"],

      }
    })
  ],
  providers: [AuthService, ErrorInterceptorProvider],

  bootstrap: [AppComponent]
})
export class AppModule { }


