import ApiModel from "./base/ApiModel";

export interface ItemDto {
  id: string;
  name: string;
}

class Item extends ApiModel<ItemDto> {
  public readonly id: string;
  public readonly name: string;

  public constructor(dto: ItemDto) {
    super();

    this.id = dto.id;
    this.name = dto.name;
  }

  public toDto(): ItemDto {
    return {
      id: this.id,
      name: this.name,
    };
  }
}

export default Item;
