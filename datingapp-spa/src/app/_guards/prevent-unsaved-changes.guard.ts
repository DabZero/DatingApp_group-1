import { Injectable } from "@angular/core";
import { MemberEditComponent } from "../members/member-edit/member-edit.component";
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<MemberEditComponent>{

    //If the "editForm" is changed in some way + nav away from page, pop up a diaglog box
    //  Yes= Go to new page (no changes saved)  No = Stay on this page "/member/edit"    
    canDeactivate(component: MemberEditComponent) {
        if (component.editForm.dirty) {
            return confirm("Continue?  Any unsaved changes will be lost");
        }
        return true //move on to another page
    }
}

