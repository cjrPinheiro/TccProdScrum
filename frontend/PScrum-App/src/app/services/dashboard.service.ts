import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GenericChart } from '@app/models/genericChart';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor(private http: HttpClient) { }

  baseURL = environment.baseApiUrl + '/dashboard';

  public getTreeMapChart(domainId: number, initialDate: string, endDate: string) : Observable<GenericChart> {
    return this.http.get<GenericChart>(`${this.baseURL}/${domainId}?initialDate=${initialDate}&endDate=${endDate}`);
  }

  public getMemberLineChart(domainId: number,projectId: number, memberId: number, initialDate: string, endDate: string) : Observable<GenericChart> {
    return this.http.get<GenericChart>(`${this.baseURL}/${domainId}/memberChart?memberId=${memberId}&projectId=${projectId}&initialDate=${initialDate}&endDate=${endDate}`);
  }
}
