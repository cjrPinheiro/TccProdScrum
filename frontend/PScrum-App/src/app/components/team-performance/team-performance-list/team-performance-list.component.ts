import { OnInit } from '@angular/core';
import { TeamMember } from '@app/models/team-member';
import { JiraService } from '@app/services/jira.service';
import { Component, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { Project } from '@app/models/project';
import { Sprint } from '@app/models/sprint';
import { Observable } from 'rxjs';
import { FormControl } from '@angular/forms';
import { filter, map, startWith, take } from 'rxjs/operators';
import { ProjectService } from '@app/services/project.service';
import { CommonService } from '@app/services/common.service';

@Component({
  selector: 'app-TeamPerformance-list',
  templateUrl: './team-performance-list.component.html',
  styleUrls: ['./team-performance-list.component.scss']
})
export class TeamPerformanceListComponent implements OnInit {

  public jiraDomainId: number = 0;
  public projects: Project[] = [];

  public teamMembers: TeamMember[] = [];
  public filteredMembers: TeamMember[] = [];
  public sprints: Sprint[] = [];
  public widthImg : number = 150;
  public marginImg : number = 2;
  public showImage : boolean = true;
  public _filterRows : string = "";
  public modalRef?: BsModalRef;
  public initialDate : string = '';
  public endDate : string = '';
  public myControl = new FormControl();
  public mainFilterSelected : number = 0;
  public sprintLoaded = false;

  public config = {
    search:true,
    height: 'auto',
    noResultsFound: 'Sem resultados',
    placeholder:'Selecione a sprint',
    searchPlaceholder:'Procurar...'

  }
  public filteredSprints = {} as Sprint[];
  public selectedSprint = {} as Sprint;

  constructor(private modalService: BsModalService,
     private jiraService: JiraService,
     private toastr: ToastrService,
     private spinner: NgxSpinnerService,
     private router: Router,
     private projectService: ProjectService,
     private commonService: CommonService
     ) { }

  public get filterRows(){
    return this._filterRows
  }
  public set filterRows(value){
    this._filterRows = value;
    this.filteredMembers = this._filterRows ? this.filterMembers(this._filterRows) : this.teamMembers;
  }

  ngOnInit(): void {
    this.resetFilters();
    let domain: any;
    this.commonService.currentDomain$.pipe(take(1)).subscribe(resp => domain = resp);
    this.jiraDomainId = domain;
    if(this.jiraDomainId){
      this.GetProjects();
    }
    else{
      this.toastr.warning('Selecione um domínio !','Aviso')
    }
    // this.filteredSprints = this.myControl.valueChanges.pipe(
    //   startWith(''),
    //   map(value => this._filter(value)  ),
    // );

  }

  public AlterImage(): void {
    this.showImage = !this.showImage;
  }
  public GetMembers(projectIndex: number, sprintId?: number, resetFilters?: boolean ): void {
    this.spinner.show();
    let projectSelected = this.projects[projectIndex];
    this.sprintLoaded = false;

    if(resetFilters)
      this.resetFilters();

    if(!sprintId){
      if(projectSelected){
      this.jiraService.getMembers(projectSelected.id, this.initialDate,this.endDate).subscribe({
        next: (_events: TeamMember[]) => {
          this.teamMembers = _events,
          this.filteredMembers = this.teamMembers
          this.GetSprints(projectIndex);
        },
        error: (e) => {
          this.spinner.hide();
          console.log(e);
          this.commonService.handleHttpErrorResponse(e);
        }
      }).add(()=> this.spinner.hide());
    }else{
      this.spinner.hide();
      this.toastr.warning("Não há projetos cadastrados.", "Aviso")
    }
    }
    else
    {
      this.jiraService.getMembersBySprintId(projectSelected.id, sprintId).subscribe({
        next: (_events: TeamMember[]) => {
          this.teamMembers = _events,
          this.filteredMembers = this.teamMembers,
          this.sprintLoaded = true;
          let sprint = this.sprints.filter(q=> q.id == sprintId)[0];
          console.log('sprint', sprint)
          this.initialDate = new Date(sprint.startDate).toLocaleDateString();
          this.endDate = new Date(sprint.endDate).toLocaleDateString();
        },
        error: (e) => {
          this.spinner.hide();
          console.log("Erro filtro sprints",e);
          this.toastr.error("Erro ao carregar os dados." + e.error.message, "Erro")
        }
      }).add(()=> this.spinner.hide());
    }
  }
  resetFilters() {
    let currDate = new Date()
    this.endDate = this.formatDate(currDate.toLocaleDateString());

    currDate.setDate( currDate.getDate() - 7 );

    this.initialDate = this.formatDate(currDate.toLocaleDateString());
    this.mainFilterSelected = 0;
    this.selectedSprint =  {} as Sprint;
  }
  formatDate(date : string) : string{
    let aux = date.split('/');
    return `${aux[2]}-${aux[1]}-${aux[0]}`
  }
  public GetSprints(projectIndex: number): void {
    let projectSelected = this.projects[projectIndex];

    this.jiraService.getSprints(projectSelected.id).subscribe({
      next: (_sprints: Sprint[]) => {
        this.sprints = _sprints;
        this.filteredSprints = this.sprints;
      },
      error: (e) => {
        this.toastr.error("Erro ao carregar as sprints." + e, "Error")
      }
    });
  }

  public filterMembers(filter : string) : TeamMember[] {
    filter = filter.toLocaleLowerCase();

    return this.teamMembers.filter(
      (event : TeamMember) => event.name.toLocaleLowerCase().indexOf(filter) !== -1
    )
  }

  public GetProjects(): void{
    this.spinner.show();
    this.clearTable();
    this.projectService.getProjects(this.jiraDomainId).subscribe({
      next: (_projects: Project[]) => {
        this.projects = _projects,
        this.GetMembers(0);
        this.GetSprints(0);
        this.spinner.hide();
      },
      error: (e) => {
        this.spinner.hide(),
        this.commonService.handleHttpError(e);
      }
    });
  }

  public clearTable(): void{
    this.teamMembers =  [];

  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.toastr.success("Event deleted with sucess!", 'Deleted!')
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detailEvent(id: number):void{
    this.router.navigate([`/events/detail/${id}`])
  }


  public selectSprintsChanged($event : any, projectIndex: number) : void{
    console.log("sprints changed", $event);
    this.GetMembers(projectIndex, $event.id);
  }

  filterSprint(event : any) {
    //in a real application, make a request to a remote url with the query and return filtered results, for demo we filter at client side
    let filtered: any[] = [];
    let query = event.query;
    for (let i = 0; i < this.sprints.length; i++) {
      let sprint = this.sprints[i];
      if (sprint.name.toLowerCase().indexOf(query.toLowerCase()) == 0) {
        filtered.push(sprint);
      }
    }

    this.filteredSprints = filtered;
  }

}
