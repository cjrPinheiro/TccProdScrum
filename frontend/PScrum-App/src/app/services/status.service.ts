import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Status } from '@app/models/status';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatusService {

  baseURL = environment.baseApiUrl + '/status';
  constructor(private http: HttpClient) { }

  public getProjects(projectId: number) : Observable<Status[]> {
    return this.http.get<Status[]>(`${this.baseURL}/${projectId}`);
  }
}
