import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UserService } from '../user.service';
import { ToastyService } from 'ngx-toasty';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  errors = '';

  constructor(private fb: FormBuilder, private userService: UserService, private toastyService: ToastyService) {

    this.form = this.fb.group({
      username: [''],
      password: ['']
    });
  }

  ngOnInit() {
  }

  get username() {
    return this.form.get('username');
  }

  get password() {
    return this.form.get('password');
  }

  get authenticated(): boolean {
    return this.userService.isAuthenticated();
  }

  login() {
    this.errors = '';
    this.userService.login(this.form.value)
    .subscribe(() => {
      this.toastyService.success({
        title: 'Success',
        msg: 'Logged in sucessfully.',
        theme: 'bootstrap',
        showClose: true,
        timeout: 5000
      }),
      this.form.reset();
    }, error => this.errors = error);
  }

}
