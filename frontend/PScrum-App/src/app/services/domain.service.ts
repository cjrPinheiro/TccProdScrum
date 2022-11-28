import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JiraDomain } from '@app/models/jiraDomain';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DomainService {

  baseURL = environment.baseApiUrl + '/jiraDomain';

  constructor(private http: HttpClient) { }

  public getDomains() : Observable<JiraDomain[]> {
    return this.http.get<JiraDomain[]>(this.baseURL);
  }
  public createDomain(userUpt: JiraDomain): Observable<JiraDomain> {
    return this.http.post<JiraDomain>(this.baseURL, userUpt);
  }
  public updateDomain(id: number, userUpt: JiraDomain): Observable<JiraDomain> {
    return this.http.put<JiraDomain>(`${this.baseURL}/${id}`, userUpt);
  }
  public deleteDomain(id: number): Observable<any> {
    return this.http.delete(`${this.baseURL}/${id}`);
  }
}
