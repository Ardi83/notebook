import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  roles: Role[];
  users: User[];
  user: User;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getRoles()
      .subscribe(roles => this.roles = roles);

    this.userService.getUsers()
      .subscribe(users => this.users = users);
  }

  getUserDetails(id) {
    this.userService.getUser(id)
      .subscribe(user => this.user = user);
  }

}
