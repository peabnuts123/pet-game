import { observable } from "mobx";

import ApiModel from "./base/ApiModel";
import PlayerInventoryItem, { PlayerInventoryItemDto } from "./PlayerInventoryItem";

export interface UserProfileDto {
  id: string;
  username: string;
  inventory: PlayerInventoryItemDto[];
}

class UserProfile extends ApiModel<UserProfileDto>{
  public readonly id: string;
  @observable
  public username: string;
  @observable
  public inventory: PlayerInventoryItem[];

  public constructor(dto: UserProfileDto) {
    super();

    this.id = dto.id;
    this.username = dto.username;
    this.inventory = dto.inventory.map((dto: PlayerInventoryItemDto) => new PlayerInventoryItem(dto));
  }

  public toDto(): UserProfileDto {
    return {
      id: this.id,
      username: this.username,
      inventory: this.inventory.map((inventoryItem) => inventoryItem.toDto()),
    };
  }
}

export default UserProfile;
