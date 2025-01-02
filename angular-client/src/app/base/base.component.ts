import { NgxSpinnerService } from 'ngx-spinner';

export class BaseComponent {
  constructor(private spinner: NgxSpinnerService) { }

  showSpinner(name: SpinnerName) {
    this.spinner.show(name);

    // Şimdilik 3sn sonra spinner kapanması için ilerde api istekleri bittiğinde kapanacak
    setTimeout(() => {
      this.hideSpinner(name);
    }, 3000);
  }

  hideSpinner(name: SpinnerName) {
    this.spinner.hide(name);
  }
}

export enum SpinnerName {
  SquareJellyBox = "s-square-jelly-box",
  BallAtom = "s-ball-atom",
  BallNewtonCradle = "s-ball-newton-cradle",
  BallScaleMultiple = "s-ball-scale-multiple"
}
