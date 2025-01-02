import { Injectable } from '@angular/core';
declare var alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  message(message: string, messageOptions: Partial<AlertifyOptions>) {
    alertify.set('notifier', 'position', messageOptions.position);
    alertify.set('notifier', 'delay', messageOptions.delay);
    const msg = alertify[messageOptions.type](message);
    if (messageOptions.dismissOthers)
      msg.dismissOthers();
  }

  dismiss() {
    alertify.dismissAll();
  }
}

export class AlertifyOptions {
  type: MessageType = MessageType.Message;
  position: MessagePosition = MessagePosition.TopRight;
  delay: Number = 3;
  dismissOthers: boolean = false;
}

export enum MessageType {
  Error = "error",
  Message = "message",
  Notify = "notify",
  Success = "success",
  Warning = "warning"
}

export enum MessagePosition {
  TopCenter = "top-center",
  TopRight = "top-right",
  TopLeft = "top-left",
  BottomCenter = "bottom-center",
  BottomRight = "bottom-right",
  BottomLeft = "bottom-left"
}
