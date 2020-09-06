import { observable } from "mobx";

import ApiModel from "./base/ApiModel";
import PlayerInventoryItem, { PlayerInventoryItemDto } from "./PlayerInventoryItem";

export interface UserProfileDto {
  id: string;
  authId: string;
  username: string;
  inventory: PlayerInventoryItemDto[];
}

class UserProfile extends ApiModel<UserProfileDto>{
  public readonly id: string;
  public readonly authId: string;
  @observable
  public username: string;
  @observable
  public inventory: PlayerInventoryItem[];

  public constructor(dto: UserProfileDto) {
    super();

    this.id = dto.id;
    this.authId = dto.authId;
    this.username = dto.username;
    this.inventory = dto.inventory.map((dto: PlayerInventoryItemDto) => new PlayerInventoryItem(dto));
  }

  public toDto(): UserProfileDto {
    return {
      id: this.id,
      authId: this.authId,
      username: this.username,
      inventory: this.inventory.map((inventoryItem) => inventoryItem.toDto()),
    };
  }
}

export default UserProfile;
