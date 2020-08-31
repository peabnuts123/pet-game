import ApiModel from "./base/ApiModel";
import Item, { ItemDto } from "./Item";

export interface TakingTreeInventoryItemDto {
  id: string;
  itemId: string;
  item: ItemDto;
}

class TakingTreeInventoryItem extends ApiModel<TakingTreeInventoryItemDto> {
  public readonly id: string;
  public readonly itemId: string;
  public readonly item: Item;

  public constructor(dto: TakingTreeInventoryItemDto) {
    super();

    this.id = dto.id;
    this.itemId = dto.itemId;
    this.item = new Item(dto.item);
  }

  public toDto(): TakingTreeInventoryItemDto {
    return {
      id: this.id,
      itemId: this.itemId,
      item: this.item.toDto(),
    };
  }
}

export default TakingTreeInventoryItem;
