import { Component, OnInit } from '@angular/core';
import { AuthTokenService } from 'src/app/services';

@Component({
  selector: 'app-home-layout',
  templateUrl: './home-layout.component.html',
  styleUrls: ['./home-layout.component.scss']
})
export class HomeLayoutComponent implements OnInit {
  constructor(public auth: AuthTokenService) {}

  ngOnInit() {}

  signOut() {
    this.auth.token = undefined;
    location.reload();
  }
}
