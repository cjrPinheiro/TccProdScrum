export interface Sprint {
  id : number;
  projectId : number;
  sprintCode : number;
  state : string;
  name : string;
  startDate : Date;
  endDate : Date;
  completeDate : Date;
  goal : string;
}
