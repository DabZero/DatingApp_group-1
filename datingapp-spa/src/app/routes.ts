import { Routes } from '@angular/router';
import { HomeComponent } from "./home/home.component";
import { MemberListComponent } from "./members/member-list/member-list.component";
import { MessagesComponent } from "./messages/messages.component";
import { ListComponent } from "./list/list.component";
import { AuthGuard } from "./_guards/auth.guard";

export const appRoutes: Routes = [
    { path: "", component: HomeComponent },
    {
        path: "",                                                   //localhost:4200 +blank path to add onto this
        runGuardsAndResolvers: "always",                            //always run guards for these children.. verify if this path can be accessed or not
        canActivate: [AuthGuard],                                   //This single guard (one of many)
        children: [
            { path: "members", component: MemberListComponent },
            { path: "messages", component: MessagesComponent },
            { path: "lists", component: ListComponent }
        ]
    },

    { path: "**", redirectTo: "", pathMatch: "full" }
]  