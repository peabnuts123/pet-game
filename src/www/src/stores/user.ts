import { action, computed, observable } from "mobx";

import UserProfile from "@app/models/UserProfile";
import Api, { ApiError } from '@app/services/api';
import Endpoints from "@app/constants/endpoints";
import Logger from "@app/util/Logger";

import Store from "./base/store";

class UserStore extends Store {
  @observable
  private _currentUserProfile: UserProfile | null;

  public constructor() {
    super();

    this._currentUserProfile = null;
  }

  @action
  public async refreshUserProfile(): Promise<void> {
    try {
      this._currentUserProfile = await Api.get<UserProfile>(Endpoints.Auth.getProfile());
    } catch (e) {
      if (e instanceof ApiError) {
        Logger.log("Failed to fetch user profile", e);
      } else {
        throw e;
      }
    }
  }

  @computed
  public get currentUserProfile(): UserProfile | null {
    return this._currentUserProfile;
  }

  @computed
  public get isUserLoggedIn(): boolean {
    return this.currentUserProfile !== null;
  }

  @computed
  public get isUserLoggedOut(): boolean {
    return this.currentUserProfile === null;
  }
}

export default UserStore;
