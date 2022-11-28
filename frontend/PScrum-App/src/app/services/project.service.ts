import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Project } from '@app/models/project';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  baseURL = environment.baseApiUrl + '/project';

  constructor(private http: HttpClient) { }

  public getProjects(jiraDomainId: number) : Observable<Project[]> {
    return this.http.get<Project[]>(`${this.baseURL}/${jiraDomainId}`);
  }
  public updateProject(id: number, userUpt: Project): Observable<Project> {
    return this.http.put<Project>(`${this.baseURL}/${id}`, userUpt);
  }


}
