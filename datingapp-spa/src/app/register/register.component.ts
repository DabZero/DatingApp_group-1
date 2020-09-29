import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { User } from '../_models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //model: any = {};
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService, private alertify: AlertifyService,
    private router: Router, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.bsConfig = { containerClass: "theme-red" }

    this.createRegisterForm();
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(
        () => { this.alertify.success("Registration Successful") },
        error => {this.alertify.error(error)},
        () => {this.authService.login(this.user).subscribe(
          () => {this.router.navigate(["/members"])}
        )}
      )
    }


  }

  cancel() {
    console.log("Remove user/password from input fields");

  }

  passwordMatchValidator(cv1: FormGroup) {
    return cv1.get("password").value === cv1.get("confirmPassword").value
      ? null : { mismatch: true };
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ["male"],
      username: ["", Validators.required],
      knownAs: ["", Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ["", Validators.required],
      country: ["", Validators.required],
      password: ["",
        [Validators.required, Validators.minLength(4), Validators.maxLength(8)]
      ],
      confirmPassword: ["", Validators.required]
    }, { validator: this.passwordMatchValidator });
  }

}


