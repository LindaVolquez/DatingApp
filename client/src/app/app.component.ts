//import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
import { setTheme } from 'ngx-bootstrap/utils';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  title = 'Dating Application';
  //users: any; //users: any is kind of dengerous

  constructor(private accountService: AccountService) {
  setTheme('bs5');}

  ngOnInit(): void{
 //   this.getUsers();
    this.SetCurrentUser();
  }

  /*
  getUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log('Request has completed')
      })

  }
*/

  SetCurrentUser() {
  // const user: User = JSON.parse(localStorage.getItem('user')!);
      const userString = localStorage.getItem('user');
      if (!userString) return;
      const user: User = JSON.parse(userString);
      this.accountService.setCurrentUser(user);
  }

}
