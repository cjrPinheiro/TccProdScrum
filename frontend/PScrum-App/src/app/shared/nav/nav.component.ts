import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JiraDomain } from '@app/models/jiraDomain';
import { AccountService } from '@app/services/account.service';
import { CommonService } from '@app/services/common.service';
import { DomainService } from '@app/services/domain.service';
import { JiraService } from '@app/services/jira.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})


export class NavComponent implements OnInit {
  selectedDomain: number = 0;
  selectedDomain2: string = 'Selecione um domínio...';

  isCollapsed = true;
  public domains: JiraDomain[] = [];
  constructor(private router: Router, public accountService: AccountService,
    public domainService: DomainService, public commonService: CommonService) {
       this.accountService.currentMessage.subscribe(user=> this.loadDomains());
    }

  ngOnInit(): void {
    this.domains = [];
    this.loadDomains();
  }

  showMenu(): boolean{
    return this.router.url != '/user/login';
  }

  logout(): void{
    this.accountService.logout();
    this.router.navigateByUrl('/user/login');
  }


  loadDomains(): void{
    let currentUser = null;
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);
    if(currentUser){
      this.domainService.getDomains().subscribe({
        next: (_domains: JiraDomain[]) => {
          this.domains = _domains;
          if(this.commonService.currentDomain$ !==  null && this.commonService.currentDomain$ !== undefined){
            let domain: any;
            this.commonService.currentDomain$.pipe(take(1)).subscribe(resp => domain = resp);
            this.selectedDomain = domain;
          }else{
            if(this.domains.length){
              this.selectedDomain = this.domains[0].id;
              this.commonService.setCurrentDomain(this.domains[0].id)
            }
          }
        },
        error: (e) => {
          this.commonService.handleHttpErrorResponse(e);
        }
      });
    }
  }
  updateCurrentDomain(): void{
    this.commonService.setCurrentDomain(this.selectedDomain);
    window.location.reload();
  }
}
