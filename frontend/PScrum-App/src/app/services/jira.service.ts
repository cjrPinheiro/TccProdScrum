import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TeamMember } from '@app/models/team-member';
import { Project } from '@app/models/project';
import { Sprint } from '@app/models/sprint';
import { SyncResponse } from '@app/models/syncResponse';
import { environment } from '@environments/environment';

@Injectable()
export class JiraService {

  constructor(private http: HttpClient) { }

  baseURL = environment.baseApiUrl + '/jira';

  public getMembers(projectId: number, initialDate: string, endDate: string) : Observable<TeamMember[]> {
    return this.http.get<TeamMember[]>(`${this.baseURL}/GetTeamPoints/${projectId}?initialDate=${initialDate}&endDate=${endDate}`);
  }

  public getMembersBySprintId(projectId: number, sprintId: number) : Observable<TeamMember[]> {
    return this.http.get<TeamMember[]>(`${this.baseURL}/GetTeamPointsBySprintId/${projectId}?sprintId=${sprintId}`);
  }

  public getSprints(projectId: number) : Observable<Sprint[]> {
    return this.http.get<Sprint[]>(`${this.baseURL}/GetSprints/${projectId}`);
  }

  public syncProjects(jiraDomainId: number): Observable<SyncResponse>{
    return this.http.get<SyncResponse>(`${this.baseURL}/importProjects/${jiraDomainId}`);
  }

  public syncSprints(jiraDomainId: number): Observable<SyncResponse>{
    return this.http.get<SyncResponse>(`${this.baseURL}/syncSprints/${jiraDomainId}`);
  }
  public syncStatus(jiraDomainId: number): Observable<SyncResponse>{
    return this.http.get<SyncResponse>(`${this.baseURL}/syncStatuses/${jiraDomainId}`);
  }
  public syncMembers(jiraDomainId: number): Observable<SyncResponse>{
    return this.http.get<SyncResponse>(`${this.baseURL}/syncMembers/${jiraDomainId}`);
  }

}
