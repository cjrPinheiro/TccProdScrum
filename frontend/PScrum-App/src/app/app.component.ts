import { Component } from '@angular/core';
import { User } from '@app/models/Identity/user';
import { AccountService } from '@app/services/account.service';
import { CommonService } from './services/common.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'CjEvents-App';
  constructor(private accountService: AccountService, private commonService: CommonService){}

  ngOnInit(): void{
    this.setCurrentUser();
    this.setCurrentDomain();
  }

  setCurrentUser(): void{
    let user: User | null;
    if(localStorage.getItem('user')){
      user = JSON.parse(localStorage.getItem('user') ?? '{}' );
    }else{
      user = null;
    }
    if(user){
       this.accountService.setCurrentUser(user);
    }
  }
  setCurrentDomain(): void{
    let domain: number;
    if(localStorage.getItem('domain')){
      this.commonService.setCurrentDomain(Number.parseInt(localStorage.getItem('domain')!))
    }

  }
}
