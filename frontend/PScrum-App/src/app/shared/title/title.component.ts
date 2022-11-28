import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-title',
  templateUrl: './title.component.html',
  styleUrls: ['./title.component.scss']
})
export class TitleComponent implements OnInit {
  @Input() title: string='';
  @Input() route: string='';
  @Input() subTitle: string='Desde 2022';
  @Input() iconClass: string='fa fa-user';
  @Input() listBtn: boolean = false;
  @Input() syncBtn: boolean = false;


  constructor(private router: Router) { }

  ngOnInit() {
  }

  list(): void {
    this.router.navigate([`/${this.route.toLocaleLowerCase()}/list`]);
  }
  sync(): void {
    this.router.navigate([`/${this.route.toLocaleLowerCase()}/sync`]);
  }
}
