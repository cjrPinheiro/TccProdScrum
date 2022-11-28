import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { UserLogin } from '@app/models/Identity/userLogin';
import { AccountService } from '@app/services/account.service';
import { environment } from '@environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  model = {} as UserLogin;
  apiUri = environment.baseApiUrl;
  public overlay = document.getElementById("overlay");

  // The sidebars
  public leftText = document.getElementById("sign-in");
  public rightText = document.getElementById("sign-up");

  // The forms
  public accountForm = document.getElementById("sign-in-info");
  public signinForm = document.getElementById("sign-up-info");

  constructor(private accountService: AccountService, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }
  @Output() messageEvent = new EventEmitter<string>();


  ngOnInit() {

  }

  public login(): void {
    this.spinner.show();
    this.accountService.login(this.model).subscribe(

      () => {
        this.accountService.loggedInEvent();
        this.router.navigateByUrl('/dashboard');
        this.spinner.hide();
      },
      (error: any) => {
        if(error.status = 401)
          this.toastr.error('Usuário ou senha inválido !');
        else
          console.error(error);
        this.spinner.hide();
      });
  }
  sendMessage() {
    this.messageEvent.emit();
  }

  // Open the Sign Up page
  openSignUp = () =>{
    // Remove classes so that animations can restart on the next 'switch'
    this.leftText!.classList.remove("overlay-text-left-animation-out");
    this.overlay!.classList.remove("open-sign-in");
    this.rightText!.classList.remove("overlay-text-right-animation");
    // Add classes for animations
    this.accountForm!.className += " form-left-slide-out"
    this.rightText!.className += " overlay-text-right-animation-out";
    this.overlay!.className += " open-sign-up";
    this.leftText!.className += " overlay-text-left-animation";
    // hide the sign up form once it is out of view
    setTimeout(() =>{
      this.accountForm!.classList.remove("form-left-slide-in");
      this.accountForm!.style.display = "none";
      this.accountForm!.classList.remove("form-left-slide-out");
    }, 700);
    // display the sign in form once the overlay begins moving right
    setTimeout(() =>{
      this.signinForm!.style.display = "flex";
      this.signinForm!.classList.add(" form-right-slide-in");
    }, 200);
  }

  // Open the Sign In page
  openSignIn = () =>{
    // Remove classes so that animations can restart on the next 'switch'
    this.leftText!.classList.remove("overlay-text-left-animation");
    this.overlay!.classList.remove("open-sign-up");
    this.rightText!.classList.remove("overlay-text-right-animation-out");
    // Add classes for animations
    this.signinForm!.classList.add(" form-right-slide-out");
    this.leftText!.className += " overlay-text-left-animation-out";
    this.overlay!.className += " open-sign-in";
    this.rightText!.className += " overlay-text-right-animation";
    // hide the sign in form once it is out of view
    setTimeout(() =>{
      this.signinForm!.classList.remove("form-right-slide-in")
      this.signinForm!.style.display = "none";
      this.signinForm!.classList.remove("form-right-slide-out")
    },700);
    // display the sign up form once the overlay begins moving left
    setTimeout(() =>{
      this.accountForm!.style.display = "flex";
      this.accountForm!.classList.add(" form-left-slide-in");
    },200);
  }

}
