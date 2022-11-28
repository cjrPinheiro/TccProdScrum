import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JiraDomain } from '@app/models/jiraDomain';
import { DomainService } from '@app/services/domain.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-domain',
  templateUrl: './domain.component.html',
  styleUrls: ['./domain.component.scss']
})
export class DomainComponent implements OnInit {
  public _filterRows : string = "";
  public filteredDomains: JiraDomain[] = [];
  public domains: JiraDomain[] = [];
  public editedDomain: JiraDomain = {} as JiraDomain;
  public modalRef?: BsModalRef;
  public deleteDomainId: number = 0;
  public operation = '';
  form!: FormGroup;
  public get f(){
    return this.form.controls;
  }
  public get filterRows(){
    return this._filterRows
  }
  public set filterRows(value){
    this._filterRows = value;
    this.filteredDomains = this._filterRows ? this.filterProjects(this._filterRows) : this.domains;
  }
  constructor(private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private domainService: DomainService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.GetDomains();
  }
  public filterProjects(filter : string) : JiraDomain[] {
    filter = filter.toLocaleLowerCase();

    return this.domains.filter(
      (domain : JiraDomain) => domain.baseUrl.toLocaleLowerCase().indexOf(filter) !== -1
    )
  }
  public GetDomains(): void{
    this.spinner.show();
    this.clearTable();
    this.domainService.getDomains().subscribe({
      next: (_domains: JiraDomain[]) => {
        this.domains = _domains,
        this.filteredDomains = this.domains,
        this.spinner.hide();
      },
      error: (e: any) => {
        this.spinner.hide(),
        this.toastr.error("Não foi possível carregar os domínios." + e.error, "Erro")
      }
    });
  }

  public clearTable(): void{
    this.domains =  [];
    this.filteredDomains = [];
  }

  openModal(template: TemplateRef<any>, domainId: number, operation: string) {
    this.operation = operation;
    this.editedDomain = {} as JiraDomain;
    if(this.postOperation()){
      this.form = this.fb.group({
        baseUrl:['', Validators.required],
        email:['', Validators.required],
        apiKey:['', Validators.required],
      });
    }else{
      this.editedDomain = this.domains.filter(
        (domain : JiraDomain) => domain.id == domainId)[0];
      this.form = this.fb.group({
        baseUrl:[this.editedDomain.baseUrl, Validators.required],
        email:[this.editedDomain.email, Validators.required],
        apiKey:['', Validators.required],
      });
    }
    this.modalRef = this.modalService.show(template, {class: 'modal-md'});
  }
  openModalDelete(template: TemplateRef<any>, domainId: number) {
    this.deleteDomainId = domainId;
    this.editedDomain = this.domains.filter(
      (domain : JiraDomain) => domain.id == domainId)[0];
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }
  confirm(): void {
    this.modalRef?.hide();

    if(this.postOperation())
      this.createDomain();
    else
      this.updateDomain();
  }
  confirmDelete(): void {
    this.modalRef?.hide();
    this.deleteDomain();
  }

  decline(): void {
    this.modalRef?.hide();
  }

  public clearForm(event: any): void{
    event.preventDefault();
    this.form.reset();
  }

  public createDomain(): void{
    this.editedDomain = { ...this.form.value };
    this.spinner.show();

    this.domainService.createDomain(this.editedDomain).subscribe(
      (_domain : JiraDomain) => {
        this.spinner.hide();
        this.toastr.success('Domínio criado com sucesso !', 'Successo');
        this.ngOnInit();
      },
      (err : any) => {
        this.toastr.error(`Erro ao cadastrar o domínio ${err.error}`, 'Erro'),
        console.error(err);
        this.spinner.hide();
      }
      )
  }
  public updateDomain(): void{
    let id = this.editedDomain.id;
    this.editedDomain = { ...this.form.value };
    this.spinner.show();

    this.domainService.updateDomain(id, this.editedDomain).subscribe(
      (_domain : JiraDomain) => {
        this.spinner.hide();
        this.toastr.success('Domínio atualizado com sucesso !', 'Successo');
        this.ngOnInit();
      },
      (err : any) => {
        this.toastr.error(err, 'Erro'),
        console.error(err);
        this.spinner.hide();
      }
      )
  }
  public deleteDomain(): void{

    this.spinner.show();
      this.domainService.deleteDomain(this.deleteDomainId).subscribe({
        next: () => {
          this.spinner.hide();
          this.toastr.success('Domínio excluído com sucesso !', 'Successo');
          this.ngOnInit();
        },
        error: (e) => {
          this.toastr.error(`Não foi possível deletar o domínio`, 'Erro'),
          console.error(e);
          this.spinner.hide();
        }
      });
  }
  public postOperation(): boolean {
    return this.operation == 'post';
  }
}
