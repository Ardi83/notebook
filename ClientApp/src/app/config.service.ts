import { Injectable } from '@angular/core';
import { isDevMode } from '@angular/core';

@Injectable()
export class ConfigService {
  _apiURI: string;

  constructor() {
    // this._apiURI = 'https://haays.nielsch87.me/api';
    if (!isDevMode()) {
      this._apiURI = 'https://notebook.nielsch87.me/api/';
    } else {
      this._apiURI = 'https://localhost:8081/api/';
      // this._apiURI = 'http://stofa.nielsch87.me:5000/api';
    }
  }

  getApiURI() {
      return this._apiURI;
  }
}
