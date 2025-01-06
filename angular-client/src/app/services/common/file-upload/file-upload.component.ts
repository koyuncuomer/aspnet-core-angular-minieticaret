import { Component, Input } from '@angular/core';
import { NgxFileDropEntry } from 'ngx-file-drop';
import { HttpClientService } from '../http-client.service';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { AlertifyService, MessagePosition, MessageType } from '../../admin/alertify.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../ui/custom-toastr.service';
import { FileUploadDialogComponent, FileUploadDialogState } from '../../../dialogs/file-upload-dialog/file-upload-dialog.component';
import { DialogService } from '../dialog.service';

@Component({
  selector: 'app-file-upload',
  standalone: false,

  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss'
})
export class FileUploadComponent {

  constructor(private httpClientService: HttpClientService, private alertfyService: AlertifyService, private customToastrService: CustomToastrService, private dialogService: DialogService) { }

  public files: NgxFileDropEntry[];

  @Input() options: Partial<FileUploadOptions>;

  public selectedFiles(files: NgxFileDropEntry[]) {
    this.files = files;
    const fileData: FormData = new FormData();

    for (const file of files) {
      (file.fileEntry as FileSystemFileEntry).file((_file: File) => {
        fileData.append(_file.name, _file, file.relativePath);
      })
    }

    this.dialogService.openDialog({
      componentType: FileUploadDialogComponent,
      data: FileUploadDialogState.Yes,
      afterClosed: () => {

        this.httpClientService.post({
          controller: this.options.controller,
          action: this.options.action,
          queryString: this.options.queryString,
          headers: new HttpHeaders({ 'responseType': 'blob' })
        }, fileData).subscribe({
          next: (result) => {
            console.log("file.upload.components post result, ", result);
            const msg: string = "Files uploaded successfully";
            if (this.options.isAdminPage) {
              this.alertfyService.message(msg, { dismissOthers: true, type: MessageType.Success, position: MessagePosition.TopRight });
            } else {
              this.customToastrService.message(msg, "Success", { type: ToastrMessageType.Success, position: ToastrPosition.TopRight });
            }
          },
          error: (errorResponse: HttpErrorResponse) => {
            console.log(errorResponse);
            const _error: Array<{ key: string; value: Array<string> }> = errorResponse.error;

            let msg = "";
            _error.forEach((error) => {
              error.value.forEach((value) => {
                msg += `${value}<br>`;
              });
            });

            if (this.options.isAdminPage) {
              this.alertfyService.message(msg, { dismissOthers: true, type: MessageType.Error, position: MessagePosition.TopRight });
            } else {
              this.customToastrService.message(msg, "Error", { type: ToastrMessageType.Error, position: ToastrPosition.TopRight });
            }

          },
          complete: () => {
            console.log("İstek tamamlandı.");
          }
        });
      }
    })


  }

}

export class FileUploadOptions {
  controller?: string;
  action?: string;
  queryString?: string;
  explanation?: string;
  accept?: string;
  isAdminPage?: boolean = false;
}
