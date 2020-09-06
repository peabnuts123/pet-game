import ApiModel from "./base/ApiModel";
import Item, { ItemDto } from "./Item";
import UserProfile, { UserProfileDto } from "./UserProfile";

export interface TakingTreeInventoryItemDto {
  id: string;
  item: ItemDto;
  donatedBy: UserProfileDto,
}

class TakingTreeInventoryItem extends ApiModel<TakingTreeInventoryItemDto> {
  public readonly id: string;
  public readonly item: Item;
  public readonly donatedBy: UserProfile;

  public constructor(dto: TakingTreeInventoryItemDto) {
    super();

    this.id = dto.id;
    this.item = new Item(dto.item);
    this.donatedBy = new UserProfile(dto.donatedBy);
  }

  public toDto(): TakingTreeInventoryItemDto {
    return {
      id: this.id,
      item: this.item.toDto(),
      donatedBy: this.donatedBy.toDto(),
    };
  }
}

export default TakingTreeInventoryItem;
