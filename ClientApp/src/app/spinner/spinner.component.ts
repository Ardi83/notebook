import { Component, OnInit } from '@angular/core';
import { SpinnerService } from '../spinner.service';
import { Subscription } from 'rxjs/Subscription';
import { LoaderState } from './loader';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent implements OnInit {
  private subscription: Subscription;
  show = false;

  constructor(private spinnerService: SpinnerService) { }

  ngOnInit() {
    this.subscription = this.spinnerService.loaderState
      .subscribe((state: LoaderState) => {
        this.show = state.show;
      });
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
