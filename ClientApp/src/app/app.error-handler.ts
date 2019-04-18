import { ErrorHandler, Inject, NgZone } from '@angular/core';
import { ToastyService } from 'ngx-toasty';

export class AppErrorHandler implements ErrorHandler {
  constructor(
    @Inject(NgZone) private ngZone: NgZone,
    @Inject(ToastyService) private toastyService: ToastyService) {}
    
  handleError(error: any): void {
    this.ngZone.run(() => {
      this.toastyService.error({
        title: 'Error',
        msg: 'Unexpected error happened!',
        theme: 'bootstrap',
        showClose: true,
        timeout: 5000
      });
    });
  }
}