import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseService } from './base.service';
import { ConfigService } from './config.service';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/finally';
import { SpinnerService } from './spinner.service';
import * as jwt_decode from 'jwt-decode';
import { Observable } from 'rxjs/Observable';

export const token: string = 'auth_token';
export const admin: string = 'admin';

@Injectable()
export class UserService extends BaseService {
  baseUrl = '';
  private logged = false;
  private _authStatusSource = new BehaviorSubject<boolean>(false);
  authStatus$ = this._authStatusSource.asObservable();

  constructor(private httpClient: HttpClient, private configServise: ConfigService, private spinnerService: SpinnerService) {
    super();

    this.baseUrl = this.configServise.getApiURI();
    this.logged = !!sessionStorage.getItem(token);
    this._authStatusSource.next(this.logged);
  }

  getToken(): string {
    return sessionStorage.getItem(token);
  }

  getTokenExpirationDate(token: string): Date {
    const decoded = jwt_decode(token);

    if (decoded.exp === undefined) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  isTokenExpired(token?: string): boolean {
    if(!token) token = this.getToken();
    if(!token) return true;

    const date = this.getTokenExpirationDate(token);
    if(date === undefined) return false;
    return !(date.valueOf() > new Date().valueOf());
  }

  isAuthenticated(): boolean {
    if (this.logged && !this.isTokenExpired())
      return true;
    return false;
  }

  getUserRoles(token?: string): string[] {
    if (!token) token = this.getToken();
    const decoded = jwt_decode(token);
    return decoded.rol;
  }

  isAdmin(): boolean {
    const roles = this.getUserRoles();
    return roles.includes(admin);
  }

  logoff() {
    sessionStorage.removeItem('auth_token');
    this.logged = false;
    this._authStatusSource.next(false);
  }

  getRoles(): Observable<Role[]> {
    return this.httpClient.get('api/roles')
      .catch(this.handleError);
  }

  getUsers(): Observable<User[]> {
    let headers = new HttpHeaders().set('Content-Type', 'application/json');
    let options = headers.append('Authorization', 'Bearer ' + this.getToken());

    return this.httpClient.get(this.baseUrl + 'users', {headers: options})
      .catch(this.handleError);
    }

  getUser(id): Observable<User> {
    let headers = new HttpHeaders().set('Content-Type', 'application/json');
    let options = headers.append('Authorization', 'Bearer ' + this.getToken());

    return this.httpClient.get(this.baseUrl + 'user/' + id, {headers: options})
      .catch(this.handleError);
  }

  register(data: Register) {
    this.spinnerService.start();
    return this.httpClient.post(this.baseUrl + 'user', data)
      .catch(this.handleError)
      .finally(() => {
        this.spinnerService.stop();
      });
  }

  login(data: Login) {
    this.spinnerService.start();
    return this.httpClient.post(this.baseUrl + 'login', data)
    .map(result => {
      sessionStorage.setItem(token, result['auth_token']);
      this.logged = true;
      this._authStatusSource.next(true);
    })
    .catch(this.handleError)
    .finally(() => {
      this.spinnerService.stop();
    });
  }

}
