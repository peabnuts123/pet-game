import { action, computed, observable } from "mobx";

import UserProfile, { UserProfileDto } from "@app/models/UserProfile";
import Api, { ApiError } from '@app/services/api';
import Endpoints from "@app/constants/endpoints";
import Logger from "@app/util/Logger";

import Store from "./base/store";

class UserStore extends Store {
  @observable
  private _hasFetchedUserProfile: boolean = false;
  @observable
  private _currentUserProfile: UserProfile | null;

  public constructor() {
    super();

    this._currentUserProfile = null;
  }

  @action
  public async refreshUserProfile(): Promise<void> {
    try {
      this._hasFetchedUserProfile = false || this._hasFetchedUserProfile;
      const userProfileDto: UserProfileDto = await Api.get<UserProfileDto>(Endpoints.User.getProfile());
      this._currentUserProfile = new UserProfile(userProfileDto);
    } catch (e) {
      if (!(e instanceof ApiError)) {
        Logger.logError("Unknown occurred while fetching user profile", e);
        throw e;
      } else if (e.response.status !== 401) {
        Logger.logError("API error occurred while fetching user profile", e.response, await e.response.text());
        throw e;
      }
    } finally {
      this._hasFetchedUserProfile = true;
    }
  }

  @action
  public async updateUserProfile(updatedProfile: UserProfileDto): Promise<void> {
    const userProfileDto: UserProfileDto = await Api.patchJson<UserProfileDto>(Endpoints.User.getProfile(), {
      body: updatedProfile,
    });
    this._currentUserProfile = new UserProfile(userProfileDto);
  }

  @computed
  public get isFetchingUserProfile(): boolean {
    return this._hasFetchedUserProfile === false;
  }

  @computed get hasFetchedUserProfile(): boolean {
    return this._hasFetchedUserProfile === true;
  }

  @computed
  public get currentUserProfile(): UserProfile | null {
    return this._currentUserProfile;
  }

  @computed
  public get isUserLoggedIn(): boolean {
    return this.hasFetchedUserProfile && this.currentUserProfile !== null;
  }

  @computed
  public get isUserLoggedOut(): boolean {
    return this.hasFetchedUserProfile && this.currentUserProfile === null;
  }
}

export default UserStore;
