import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { LoaderState } from './spinner/loader';

@Injectable()
export class SpinnerService {
  private loaderSubject = new Subject<LoaderState>();
  loaderState = this.loaderSubject.asObservable();

  start() {
    this.loaderSubject.next(<LoaderState>{show: true});
  }
  stop() {
    this.loaderSubject.next(<LoaderState>{show: false});
  }

}
