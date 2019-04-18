import { hamburgerMenu } from './../../svg';
import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { SafeHtml, DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  animations: [
    trigger('changeForm', [
      state('hide', style({
        height: '0px',
        opacity: '0',
        overflow: 'hidden'
    })),
    state('show', style({
        height: '*',
        opacity: '1'
    })),
    transition('hide => show', animate('400ms ease-in')),
    transition('show => hide', animate('400ms ease-out'))
    ]),
  ]
})
export class HomeComponent implements OnInit {
  menu: SafeHtml;
  showLogin = false;
  mobileMenu = false;
  toggleName = 'Already registered, click fo login';

  constructor(private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.menu = this.sanitizer.bypassSecurityTrustHtml(hamburgerMenu);
  }

  toggleForm() {
    this.showLogin = !this.showLogin;
    if (this.showLogin) {
      this.toggleName = 'Don\'t have account, Click for signup'
    } else {
      this.toggleName = 'Already registered, click fo login';
    }
  }

  menuToggle() {
    this.mobileMenu = !this.mobileMenu;
  }
}
