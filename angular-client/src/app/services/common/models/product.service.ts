import { Injectable } from '@angular/core';
import { HttpClientService } from '../http-client.service';
import { Create_Product } from '../../../contracts/create_product';
import { HttpErrorResponse } from '@angular/common/http';
import { List_Product } from '../../../contracts/list_product.';
import { firstValueFrom, lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  constructor(private httpClientService: HttpClientService) { }

  create(product: Create_Product, successCallBack?: () => void, errorCallBack?: (errorMessage: string) => void) {
    this.httpClientService.post({ controller: "products" }, product).subscribe({
      next: (result) => {
        console.log("product.service post result, ", result);
        if (successCallBack)
          successCallBack();
      },
      error: (errorResponse: HttpErrorResponse) => {
        console.log(errorResponse);
        const _error: Array<{ key: string; value: Array<string> }> = errorResponse.error;

        let msg = "";
        if (Array.isArray(_error)) {
          _error.forEach((error) => {
            error.value.forEach((value) => {
              msg += `${value}<br>`;
            });
          });
        }
        else
          msg = "An error occurred while creating product";

        if (errorCallBack)
          errorCallBack(msg);
      },
      complete: () => {
        console.log("İstek tamamlandı.");
      }
    });
  }

  async read(page: number = 1, size: number = 5, successCallBack?: () => void, errorCallBack?: (errorMessage: string) => void): Promise<{ totalCount: number; products: List_Product[] }> {
    try {
      const promiseData = await lastValueFrom(
        this.httpClientService.get<{ totalCount: number; products: List_Product[] }>({
          controller: 'products',
          queryString: `page=${page}&size=${size}`
        })
      );
      if (successCallBack)
        successCallBack();

      return promiseData;
    } catch (error) {
      if (errorCallBack && error instanceof HttpErrorResponse)
        errorCallBack(error.message);

      throw error;
    }
  }
}
