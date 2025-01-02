import { Component } from '@angular/core';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from './services/ui/custom-toastr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'angular-client';

  constructor(private toastrService: CustomToastrService) {
    toastrService.message("Merhaba", "Eticaret", { type: ToastrMessageType.Info, position: ToastrPosition.TopLeft })
  }
}
