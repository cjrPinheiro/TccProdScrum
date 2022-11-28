import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { SyncResponse } from '@app/models/syncResponse';
import { CommonService } from '@app/services/common.service';
import { JiraService } from '@app/services/jira.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-project-sync',
  templateUrl: './project-sync.component.html',
  styleUrls: ['./project-sync.component.scss']
})
export class ProjectSyncComponent implements OnInit {

  public jiraDomainId: number = 1;
  public projectsImported = 0;
  public membersImported = 0;
  public sprintsImported = 0;
  public statusImported = 0;
  public stepsDone = 0;
  public progressValue = 33;
  public modalRef?: BsModalRef;
  public showLoadComplete = false;
  public templateRef = {} as TemplateRef<any>;
  constructor(private spinner: NgxSpinnerService, private toastr: ToastrService,
     private jiraService: JiraService, private modalService: BsModalService, private commonService: CommonService,
     private router: Router) { }

  ngOnInit() {
    let domain: any;
    this.commonService.currentDomain$.pipe(take(1)).subscribe(resp => domain = resp);
    this.jiraDomainId = domain;
    document.getElementById('btInit')?.click();
    console.log('JiraDomainId',this.jiraDomainId);
  }
  async openModal(template: TemplateRef<any>){
    this.progressValue = 0;
    this.modalRef = this.modalService.show(template, {class: 'modal-md modal-dialog-centered'});
    this.SyncProjects();
  }

  private SyncProjects(): void{
    this.jiraService.syncProjects(this.jiraDomainId).subscribe({
      next: (_resp: SyncResponse) => {
        this.projectsImported = _resp.totalImports;
        this.toastr.success(`${this.projectsImported} Projeto(s) importado(s) !.`);
        this.addProgress();
        this.SyncMembers();
        this.SyncStatus();
      },
      error: (e) => {
        if(e.status != 412){
          this.commonService.handleHttpErrorResponse(e);
          this.addProgress();
          this.SyncMembers();
          this.SyncStatus();
        }else{
          this.toastr.warning('Não há projetos cadastrados! Verifique seu domínio.');
          this.addProgress();
          this.addProgress();
          this.addProgress();
          this.addProgress();
        }

      }
    });
  }
  private async SyncMembers(): Promise<void>{
    this.jiraService.syncMembers(this.jiraDomainId).subscribe({
      next: (_resp: SyncResponse) => {
        this.membersImported = _resp.totalImports;
        this.toastr.success(`${this.membersImported} Membro(s) importado(s) !.`);
        this.addProgress();
        this.SyncSprints();
      },
      error: (err) => {
        this.commonService.handleHttpErrorResponse(err);
        this.addProgress();
        this.SyncSprints();
      }
    });
  }
  private SyncSprints(): void{
    this.jiraService.syncSprints(this.jiraDomainId).subscribe({
      next: (_resp: SyncResponse) => {
        this.sprintsImported = _resp.totalImports;
        this.toastr.success(`${this.sprintsImported} Sprint(s) importada(s) !.`);
        this.addProgress();
      },
      error: (err) => {
        this.commonService.handleHttpErrorResponse(err);
        this.addProgress();
      }
    });
  }
  private SyncStatus(): void{
    this.jiraService.syncStatus(this.jiraDomainId).subscribe({
      next: (_resp: SyncResponse) => {
        this.statusImported = _resp.totalImports;
        this.toastr.success(`${this.statusImported} Status importado(s) !.`);
        this.addProgress();
      },
      error: (err) => {
        this.commonService.handleHttpErrorResponse(err);
        this.addProgress();
      }
    });
  }
  private addProgress(): void{
    this.stepsDone +=1;

    if(this.stepsDone == 4){
      this.progressValue = 100;
      this.showLoadComplete = true;
    }
    else
    {
      console.log("progress added");
      this.progressValue += 25;
    }
  }
  close(): void {
    this.modalRef?.hide();
    this.router.navigateByUrl('/projects/list');
  }
}
