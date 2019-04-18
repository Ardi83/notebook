import { UserService } from './user.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable()
export class AdminGuardService implements CanActivate {

  constructor(private userService: UserService, private router: Router) { }

  canActivate() {
    if (this.userService.isAuthenticated() && this.userService.isAdmin())
      return true;
    this.router.navigate(['']);
      return false;
  }

}
