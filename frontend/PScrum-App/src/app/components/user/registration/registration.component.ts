import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/validatorField';
import { User } from '@app/models/Identity/user';
import { AccountService } from '@app/services/account.service';
import { CommonService } from '@app/services/common.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
  public form: FormGroup = new FormGroup({});
  user = {} as User;
  public get f(){
    return this.form.controls;
  }
  constructor(private fb: FormBuilder, private accountService: AccountService,
     private toastr: ToastrService, private router: Router,
      private spinner: NgxSpinnerService, private commonService: CommonService ) { }

  ngOnInit() {
    this.validation();
  }

  public validation(): void{
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password','confirmPassword')
    };

    this.form = this.fb.group({
      firstName: ['',Validators.required],
      lastName: ['',Validators.required],
      email: ['',[Validators.required, Validators.email]],
      username: ['',[Validators.required,Validators.maxLength(12)]],
      password: ['',[Validators.required, Validators.minLength(4)]],
      confirmPassword:['',Validators.required]
    }, formOptions);

  }

  public register(): void{
    this.spinner.show();

    this.user = { ...this.form.value };
    this.accountService.register(this.user).subscribe(
      () => {
        this.router.navigateByUrl('/dashboard');
        this.toastr.success('Account created !', 'Success');
        this.spinner.hide();
      },
      (er: any) =>{
         this.toastr.error(this.commonService.handleHttpError(er));
         this.spinner.hide();
      });
    }

  }
