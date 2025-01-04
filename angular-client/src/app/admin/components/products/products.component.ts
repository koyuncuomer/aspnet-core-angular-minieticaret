import { Component, ViewChild } from '@angular/core';
import { BaseComponent, SpinnerName } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { Create_Product } from '../../../contracts/create_product';
import { ListComponent } from './list/list.component';

@Component({
  selector: 'app-products',
  standalone: false,

  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent extends BaseComponent {
  constructor(spinner: NgxSpinnerService) {
    super(spinner)
  }

  ngOnInit(): void {
    this.showSpinner(SpinnerName.SquareJellyBox);
  }

  @ViewChild(ListComponent) listComponent: ListComponent;

  createdProduct(createdProduct: Create_Product) {
    console.log("products.component.ts içerisinden get products", createdProduct);
    this.listComponent.getProducts();
  }
}
