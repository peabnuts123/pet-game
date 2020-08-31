import { observable } from "mobx";

import ApiModel from "./base/ApiModel";
import Item, { ItemDto } from "./Item";

export interface PlayerInventoryItemDto {
  id: string;
  item: ItemDto;
  count: number;
}

class PlayerInventoryItem extends ApiModel<PlayerInventoryItemDto> {
  public readonly id: string;
  public readonly item: Item;
  @observable
  public count: number;

  public constructor(dto: PlayerInventoryItemDto) {
    super();
    this.id = dto.id;
    this.item = new Item(dto.item);
    this.count = dto.count;
  }

  public toDto(): PlayerInventoryItemDto {
    return {
      id: this.id,
      item: this.item.toDto(),
      count: this.count,
    };
  }
}

export default PlayerInventoryItem;
