import { Component, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { List_Product } from '../../../../contracts/list_product.';
import { ProductService } from '../../../../services/common/models/product.service';
import { BaseComponent, SpinnerName } from '../../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService, MessagePosition, MessageType } from '../../../../services/admin/alertify.service';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-list',
  standalone: false,

  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class ListComponent extends BaseComponent {
  constructor(spinner: NgxSpinnerService, private productService: ProductService, private alertify: AlertifyService) {
    super(spinner)
  }

  displayedColumns: string[] = ['name', 'stock', 'price', 'createdDate', 'updatedDate'];
  dataSource: MatTableDataSource<List_Product> = null
  @ViewChild(MatPaginator) paginator: MatPaginator;

  async getProducts() {
    this.showSpinner(SpinnerName.BallAtom);
    const pageIndex = this.paginator ? this.paginator.pageIndex + 1 : 1;
    const pageSize = this.paginator ? this.paginator.pageSize : 5;
    const allProducts: { totalCount: number; products: List_Product[] } = await this.productService.read(pageIndex, pageSize, () => this.hideSpinner(SpinnerName.BallAtom), (errorMessage) => {
      this.alertify.message(errorMessage, {
        dismissOthers: true,
        type: MessageType.Error,
        position: MessagePosition.TopRight,
        delay: 3
      });
    });

    this.dataSource = new MatTableDataSource<List_Product>(allProducts.products);
    this.paginator.length = allProducts.totalCount;
  }

  async pageChanged(){
    await this.getProducts();
  }

  async ngOnInit() {
    await this.getProducts();
  }

}
