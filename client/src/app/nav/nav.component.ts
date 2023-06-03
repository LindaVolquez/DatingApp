import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
 // loggedIn = false; Usen thee async pipe there is not need to use the loggedIn flaag so in the tamplate the *ngIf="loggedin" need to be placed by ="currentUser$ | async"
 // currentUser$: Observable<User | null> = of(null);

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
//    this.getCurrentUser();
//   this.currentUser$ = this.accountService.currentUser$;
  }

  /*
  getCurrentUser() {
    this.accountService.currentUser$.subscribe( {
      next: user => this.loggedIn = !!user,
      error: error => console.log(error)
    })
  }
*/

  login() {
    //console.log(this.model); // following code to handle the observable from the response
    this.accountService.login(this.model).subscribe({
        next: response => {
            console.log(response);
    //this.loggedIn = true;
        },
        error: error => console.log(error)
    })
  }

  logout() {
    this.accountService.logout();
    //this.loggedIn = false;
  }
}
