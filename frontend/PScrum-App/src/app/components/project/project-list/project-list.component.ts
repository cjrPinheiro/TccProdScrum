import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Project } from '@app/models/project';
import { CommonService } from '@app/services/common.service';
import { JiraService } from '@app/services/jira.service';
import { ProjectService } from '@app/services/project.service';
import { StatusService } from '@app/services/status.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map, startWith, take } from 'rxjs/operators';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.scss']
})
export class ProjectListComponent implements OnInit {
  public projects: Project[] = [];
  public editProject = {} as Project;
  public filteredProjects: Project[] = [];
  public _filterRows : string = "";
  public modalRef?: BsModalRef;
  public initialDate : string = '2022-03-05';
  public myControl = new FormControl();
  public jiraDomainId: number = 1;


  form!: FormGroup;
  public get f(){
    return this.form.controls;
  }

  public config = {
    search:true,
    height: 'auto',
    noResultsFound: 'Sem resultados',
    placeholder:'Selecione a sprint',
    searchPlaceholder:'Procurar...'

  }
  public options = new Array();
  filteredOptions = {} as  Observable<any[]>;

  constructor(private modalService: BsModalService,
     private toastr: ToastrService,
     private spinner: NgxSpinnerService,
     private projectService: ProjectService,
     private fb: FormBuilder,
     private commonService: CommonService
     ) { }

  public get filterRows(){
    return this._filterRows
  }
  public set filterRows(value){
    this._filterRows = value;
    this.filteredProjects = this._filterRows ? this.filterProjects(this._filterRows) : this.projects;
  }

  ngOnInit(): void {
    let domain: any;
    this.commonService.currentDomain$.pipe(take(1)).subscribe(resp => domain = resp);
    this.jiraDomainId = domain;
    if(this.jiraDomainId != undefined){
      this.GetProjects();
      this.filteredOptions = this.myControl.valueChanges.pipe(
        startWith(''),
        map(value => this._filter(value)),
      );
    }else{
      this.toastr.warning("Selecione um domínio !","Aviso")
    }
  }

  public GetProjects(): void{
    this.spinner.show();
    this.clearTable();
    console.log('Procurando projetos:', this.jiraDomainId)
    this.projectService.getProjects(this.jiraDomainId).subscribe({
      next: (_projects: Project[]) => {
        this.projects = _projects,
        this.filteredProjects = this.projects,

        this.spinner.hide();
      },
      error: (e) => {
        this.spinner.hide(),
        this.toastr.error("Error on load the projects." + e, "Error")
      }
    });
  }

  public clearTable(): void{
    this.projects =  [];
  }

  openModal(template: TemplateRef<any>, projectId: number) {
    this.editProject = this.projects.filter(
      (prj : Project) => prj.id == projectId)[0];
    this.validate();
    console.log('Projeto modal', this.editProject);
    this.modalRef = this.modalService.show(template, {class: 'modal-md'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.updateProject();
  }

  decline(): void {
    this.modalRef?.hide();
  }
  private _filter(value: string): string[] {
    console.log('_filter')
    console.log(value);
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }
  public filterProjects(filter : string) : Project[] {
    filter = filter.toLocaleLowerCase();

    return this.projects.filter(
      (prj : Project) => prj.name.toLocaleLowerCase().indexOf(filter) !== -1
    )
  }

  public validate(): void{
    this.form = this.fb.group({
      developingStatusId:[this.editProject.developingStatusId, Validators.required],
      completedStatusId:[this.editProject.completedStatusId, Validators.required],
    });

  }


  public clearForm(event: any): void{
    event.preventDefault();
    this.form.reset();
  }

  public updateProject(): void{
    let id = this.editProject.id;
    this.editProject = { ...this.form.value };
    this.spinner.show();

    this.projectService.updateProject(id, this.editProject).subscribe(
      (_project : Project) => {
        this.spinner.hide();
        this.toastr.success('Projeto atualizado com sucesso !', 'Successo');
        this.ngOnInit();
      },
      (err) => {
        this.toastr.error(err, 'Erro'),
        console.error(err);
        this.spinner.hide();
      }
      )
    }



}
