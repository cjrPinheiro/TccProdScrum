import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { User } from "@app/models/Identity/user";
import { AccountService } from "@app/services/account.service";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { Observable, of, throwError } from "rxjs";
import { catchError, take } from "rxjs/operators";

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  test: User = {} as User;
  constructor(private accountService : AccountService, private router: Router,
     private toastr: ToastrService, private spinner: NgxSpinnerService) {}

  private handleAuthError(err: HttpErrorResponse): Observable<any> {
    if (err.status === 401) {
      console.log('entrou handle error 401');
        this.spinner.hide();
        this.router.navigateByUrl(`/user/login`);
        this.accountService.logout();
        this.toastr.error('Credenciais expiradas. Realize o login novamente.', 'Erro');
        return of(err.message);
    }
    return throwError(err);
}
  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: any = null;
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);
    //currentUser = JSON.parse(localStorage.getItem('user')!);
    if(currentUser){
      req = req.clone({
        setHeaders:{
          Authorization: `Bearer ${currentUser.token}`
        }
      });
    }

    return next.handle(req).pipe(catchError(x=> this.handleAuthError(x))); ;
  }

}
