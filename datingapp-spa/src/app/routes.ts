import { Routes } from '@angular/router';
import { HomeComponent } from "./home/home.component";
import { MemberListComponent } from "./members/member-list/member-list.component";
import { MemberDetailComponent } from "./members/member-detail/member-detail.component";
import { MessagesComponent } from "./messages/messages.component";
import { ListComponent } from "./list/list.component";
import { AuthGuard } from "./_guards/auth.guard";
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';

export const appRoutes: Routes = [
    { path: "", component: HomeComponent },
    {
        path: "",                                                   //localhost:4200 +blank path to add onto this
        runGuardsAndResolvers: "always",                            //always run guards for these children.. verify if this path can be accessed or not
        canActivate: [AuthGuard],                                   //This single guard (one of many)
        children: [
            { path: "members", component: MemberListComponent, resolve: { users: MemberListResolver } },
            { path: "members/:id", component: MemberDetailComponent, resolve: { user: MemberDetailResolver } },
            { path: "messages", component: MessagesComponent },
            { path: "lists", component: ListComponent }
        ]
    },

    { path: "**", redirectTo: "", pathMatch: "full" }
]  