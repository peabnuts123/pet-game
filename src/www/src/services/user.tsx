import { observable, action, computed } from 'mobx';
import { route } from 'preact-router';

const ITEMS = [
  'Crud',
  'Coiled Rope',
  'Ripe Orange',
  'Tacking Nails',
  'Elastic Band',
  'Elegant Flask',
  'Gold Ring',
  'WORLDS BEST DAD Mug',
  'Left-hand Glove',
  'Right-hand Glove',
];

class UserService {

  @observable
  private _currentUser: User | null = null;

  @action
  public logIn(username: string, _password: string): User {
    this._currentUser = new User({
      username,
    });

    // Add 5 random items to inventory
    for (let i = 0; i < 5; i++) {
      this._currentUser?.inventory.addItem(ITEMS[Math.floor(Math.random() * ITEMS.length)]);
    }

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

export class Item {
  @observable
  public name: string;

  public constructor(options: {
    name: string,
  }) {
    this.name = options.name;
  }
}

export class InventoryItem {
  @observable
  public item: Item;
  @observable
  public amount: number;

  public constructor(options: {
    item: Item,
    amount: number
  }) {
    this.item = options.item;
    this.amount = options.amount;
  }
}

export class Inventory {
  @observable
  public items: InventoryItem[];

  public constructor() {
    this.items = [];
  }

  @action
  public addItem(name: string): void {
    const existingItem = this.items.find((item) => item.item.name === name);
    if (existingItem) {
      existingItem.amount++;
    } else {
      this.items.push(new InventoryItem({
        item: new Item({ name }),
        amount: 1,
      }));
    }
  }
}

export class User {
  @observable
  public username: string;
  @observable
  public inventory: Inventory;

  public constructor(options: {
    username: User['username'];
  }) {
    this.username = options.username;
    this.inventory = new Inventory();
  }
}

export default UserService;
