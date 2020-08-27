import { action, observable } from "mobx";
import { Item } from "./user";

class TakingTreeService {
  @observable
  public items: Item[];

  public constructor() {
    this.items = [];
  }

  @action
  public addItem(item: Item): void {
    this.items.push(item);
  }

  @action
  public takeItem(index: number): Item {
    const item = this.items[index];
    this.items.splice(index, 1);
    return item;
  }
}

export default TakingTreeService;
