import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;
  private spinnerService = inject(NgxSpinnerService);
  
  busy(): void {
    this.busyRequestCount++;
    this.spinnerService.show(undefined, {
      type: "fire",
      bdColor: "rgba(236, 216, 254, 0.95)",
      color: "rgba(100, 12, 178, 0.79)"
    });
  }

  idle(): void {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }
}
