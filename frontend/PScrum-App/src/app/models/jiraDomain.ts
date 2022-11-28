import { Project } from "./project";

export interface JiraDomain {
  id: number;
  email: string;
  apiKey: string;
  baseUrl: string;
  projects: Project[];
}
