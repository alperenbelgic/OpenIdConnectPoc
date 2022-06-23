import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject } from '@angular/core';

interface AuthenticationState {
  isAuthenticated: boolean;
  initialized: boolean;
}

interface User {
  userName: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  authenticationState: AuthenticationState = { isAuthenticated: false, initialized: false }
  users: User[] | undefined = undefined;
  myProfile: User | undefined = undefined;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    this.initalize();
  }

  private initalize() {
        this.updateAuthState();

        this.getUsers();

        this.getProfile();
    }

  authorise() {
    console.log('authorise called');

    window.location.href = this.baseUrl + 'auth';
  }

  logout() {
    console.log('logout called');

    this.http.post<AuthenticationState>(this.baseUrl + 'auth/logout', {}).subscribe(result => {
      console.log('logout result');

      this.initalize();

    }, error => console.error(error));
  }

  updateAuthState() {
    this.http.get<AuthenticationState>(this.baseUrl + 'auth/is-authenticated').subscribe(result => {

      this.authenticationState = { ...result, initialized: true };

    }, error => console.error(error));
  }

  getUsers() {

    this.users = undefined;

    this.http.get<User[]>(this.baseUrl + 'values/users').subscribe(result => {

      console.log('users', result);
      this.users = result;

    }, error => console.error(error));
  }
  
  getProfile() {

    this.myProfile = undefined;

    this.http.get<User>(this.baseUrl + 'values/my-profile').subscribe(result => {

      this.myProfile = result;

    }, error => console.error(error));
  }
}
