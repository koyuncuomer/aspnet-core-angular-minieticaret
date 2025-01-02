import { Component } from '@angular/core';
declare var $: any

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'angular-client';

  constructor() { }
}

$.get("https://localhost:7046/api/products", data => console.log(data))
