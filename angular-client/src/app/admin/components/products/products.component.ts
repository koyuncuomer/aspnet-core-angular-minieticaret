import { Component } from '@angular/core';
import { BaseComponent, SpinnerName } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { HttpClientService } from '../../../services/common/http-client.service';
import { Product } from '../../../contracts/product';

@Component({
  selector: 'app-products',
  standalone: false,

  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent extends BaseComponent {
  constructor(spinner: NgxSpinnerService, private httpClientService: HttpClientService) {
    super(spinner)
  }

  ngOnInit(): void {
    this.showSpinner(SpinnerName.SquareJellyBox);

    this.httpClientService.get<Product[]>({
      controller: "products"
    }).subscribe(data => console.log("Get işlemi ", data));

    // this.httpClientService.post({
    //   controller: "products"
    // }, {
    //   name: "Kalem",
    //   stock: 100,
    //   price: 15
    // }).subscribe(data => console.log("Post işlemi ", data));

    // this.httpClientService.put({
    //   controller: "products"
    // }, {
    //   id: "ce7e9e19-d1da-4f65-9987-b66d3e3f1b79",
    //   name: "Uçlu Kalem",
    //   stock: 110,
    //   price: 15.5
    // }).subscribe(data => console.log("Put işlemi ", data));

    // this.httpClientService.delete({
    //   controller: "products"
    // }, "c188131a-772c-4204-ac5d-c9c1575c3a26").subscribe(data => console.log("Delete işlemi ", data));
  }
}
