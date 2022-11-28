import { Status } from "./status";

export interface Project {
  id: number;
  name: string;
  key: string;
  developingStatusId: string;
  completedStatusId: string;
  developingStatus: string;
  completedStatus: string;
  jiraBoardCode : number;
  statuses: Status[];
}
