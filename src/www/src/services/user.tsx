import { observable, action, computed } from 'mobx';
import { route } from 'preact-router';

class UserService {

  @observable
  private _currentUser: User | null = null;

  @action
  public logIn(username: string, _password: string): User {
    this._currentUser = new User({
      username,
    });
    route('/');

    return this._currentUser;
  }

  @action
  public logOut(): void {
    this._currentUser = null;
    route('/');
  }

  @computed
  public get currentUser(): User | null {
    return this._currentUser;
  }

  @computed
  public get isLoggedIn(): boolean {
    return this._currentUser !== null;
  }

  @computed
  public get isLoggedOut(): boolean {
    return this._currentUser === null;
  }
}

export class User {
  public username: string;

  public constructor(options: {
    username: User['username'];
  }) {
    this.username = options.username;
  }
}

export default UserService;
