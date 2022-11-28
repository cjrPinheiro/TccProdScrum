import { Component, OnInit } from '@angular/core';
import * as am5 from '@amcharts/amcharts5';
import * as am5xy from '@amcharts/amcharts5/xy';
import * as am5hierarchy from "@amcharts/amcharts5/hierarchy";
import am5themes_Animated from '@amcharts/amcharts5/themes/Animated';
import { DashboardService } from '@app/services/dashboard.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { GenericChart } from '@app/models/genericChart';
import { take } from 'rxjs/operators';
import { CommonService } from '@app/services/common.service';
import { ChartService } from '@app/services/chart.service';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  public domainId = 0;
  public initialDate: string = '';
  public endDate: string = '';
  public memberId: number = 0;
  public projectId: number = 0;

  public chartLoaded = false;
  constructor(private dashboardService: DashboardService, private spinner: NgxSpinnerService, private toastr: ToastrService,
    private commonService: CommonService, private chartService: ChartService) {}

  ngOnInit() {
    this.setFilters();
    this.domainId = this.getCurrentDomain();
    setTimeout(()=>{
      this.getTreeMapChart();
    },1000);
  }
  getTreeMapChart() {
    if(this.domainId){
      this.spinner.show();
      this.dashboardService.getTreeMapChart(this.domainId, this.initialDate, this.endDate).subscribe(
        (chart : GenericChart) => {
          this.chartService.loadTreeMapChart(chart);
          this.chartLoaded = true;
          this.toastr.success('Gráfico carregado !', 'Successo');
          this.spinner.hide();
        },
        (err) => {
          this.commonService.handleHttpErrorResponse(err);
          this.spinner.hide();
        }
      )
     }else {
        this.toastr.warning('Selecione um domínio !','Aviso')
     }
  }

  getMemberLineChart(){
  if(this.domainId){
    this.spinner.show();
    this.dashboardService.getMemberLineChart(this.domainId,this.projectId,this.memberId, this.initialDate, this.endDate).subscribe(
      (chart : GenericChart) => {
        this.chartService.loadMemberLineChart(chart);
        this.chartLoaded = true;
        this.toastr.success('Gráfico carregado !', 'Successo');
        this.spinner.hide();
      },
      (err) => {
        this.commonService.handleHttpErrorResponse(err);
        this.spinner.hide();
      }
    )
   }else {
      this.toastr.warning('Selecione um domínio !','Aviso')
   }
  }

  setFilters(){
    let currDate = new Date()
    this.endDate = this.formatDate(currDate.toLocaleDateString());
    currDate.setDate(currDate.getDate() - 30);
    this.initialDate = this.formatDate(currDate.toLocaleDateString());
  }
  formatDate(date : string) : string{
    let aux = date.split('/');
    return `${aux[2]}-${aux[1]}-${aux[0]}`
  }
  getCurrentDomain(): number {
    let domain: any;
    this.commonService.currentDomain$.pipe(take(1)).subscribe(resp => domain = resp);
    return domain;
  }

}



