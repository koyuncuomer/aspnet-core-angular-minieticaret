import { Component, OnInit } from '@angular/core';
import { AlertifyService, MessagePosition, MessageType } from '../../services/admin/alertify.service';


@Component({
  selector: 'app-layout',
  standalone: false,

  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent implements OnInit {
  constructor(private alertify: AlertifyService) { }

  ngOnInit(): void {
    this.alertify.message("Merhaba", { type: MessageType.Success, delay: 3, position: MessagePosition.BottomRight })
  }
}
