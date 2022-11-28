import { OnInit } from '@angular/core';
import { Component, TemplateRef } from '@angular/core';


@Component({
  selector: 'app-team-performance',
  templateUrl: './team-performance.component.html',
  styleUrls: ['./team-performance.component.scss']
})
export class TeamPerformanceComponent implements OnInit {
  public filterRows: string = "";
  ngOnInit(): void {

  }
}
