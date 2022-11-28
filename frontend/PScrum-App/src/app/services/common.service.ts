import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GenericError } from '@app/models/genericError';
import { ToastrService } from 'ngx-toastr';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
private currentDomainSource = new ReplaySubject<number | null>(1);
public currentDomain$ = this.currentDomainSource.asObservable();

constructor(private toast : ToastrService) { }

  public handleHttpError(httpResponse: HttpErrorResponse) : string{
    console.log(httpResponse);
    if(httpResponse.statusText == 'Unknown Error')
      return 'Ocorreu um erro ao se comunicar com o servidor. Por favor, tente mais tarde.'
    else if(httpResponse.status == 401)
      return 'Credenciais inválidas !';
    else if (httpResponse.status == 500)
      return 'Ocorreu um erro interno aplicação! Acione o suporte.'
    else
      return 'Ocorreu um erro ao se comunicar com o servidor. Por favor, tente mais tarde.'


  }
  public handleHttpErrorResponse(httpResponse: HttpErrorResponse) : void{
    console.log(httpResponse);
    if(httpResponse.statusText == 'Unknown Error')
      this.toast.error('Ocorreu um erro ao se comunicar com o servidor. Por favor, tente mais tarde.','Erro');
    else if(httpResponse.status == 401)
      this.toast.warning('Credenciais inválidas !','Aviso');
    else if (httpResponse.status == 500){
      let error : GenericError = httpResponse.error;
      this.toast.error(error.technicalError, error.message);
    }
    else if (httpResponse.status == 400)
      this.toast.warning(httpResponse.error,'Aviso')
    else
    this.toast.error('Ocorreu um erro ao se comunicar com o servidor. Por favor, tente mais tarde.','Erro');
  }
  public setCurrentDomain(domainId: number) : void{
    localStorage.setItem('domain', domainId.toString());

    this.currentDomainSource.next(domainId);
  }
  public removeCurrentDomain(){
    this.currentDomainSource.next(null);
  }

}
