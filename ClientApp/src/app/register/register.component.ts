import { CustomValidators } from './../validators';
import { Component } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { UserService } from '../user.service';
import { ToastyService } from 'ngx-toasty';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  form: FormGroup;
  errors = '';

  constructor(private fb: FormBuilder, private userServie: UserService, private toastyService: ToastyService) {

    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['']
    }, {
      validator: [CustomValidators.MatchPassword]
    });

  }

  get email() {
    return this.form.get('email');
  }

  get password() {
    return this.form.get('password');
  }

  register() {
    var sign: Register = {
      email: this.form.value.email,
      password: this.form.value.password
    }
    this.errors = '';
    this.userServie.register(sign)
      .subscribe(result => {
        this.toastyService.success({
          title: 'Success',
          msg: 'Data was sucessfully created.',
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
        }),
        this.form.reset();
      }, error => this.errors = error);
  }

}
