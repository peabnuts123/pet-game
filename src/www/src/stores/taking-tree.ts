import { action, computed, observable } from 'mobx';

import Api from '@app/services/api';
import Endpoints from '@app/constants/endpoints';
import TakingTreeInventoryItem, { TakingTreeInventoryItemDto } from '@app/models/TakingTreeInventoryItem';

import Store from './base/store';

class TakingTreeStore extends Store {
  @observable
  private _inventoryItems: TakingTreeInventoryItem[] | null;
  @observable
  private _itemsBeingClaimed: string[];

  public constructor() {
    super();
    this._inventoryItems = null;
    this._itemsBeingClaimed = [];
  }

  @action
  public async refreshInventoryItems(): Promise<void> {
    const inventoryItemDtos = await Api.get<TakingTreeInventoryItemDto[]>(Endpoints.TakingTree.getAll());
    this.updateCollectionFromDtos(inventoryItemDtos);
  }

  @action
  public async donateItem(playerInventoryItemId: string): Promise<void> {
    const inventoryItemDtos = await Api.postJson<TakingTreeInventoryItemDto[]>(Endpoints.TakingTree.donateItem(), {
      body: {
        PlayerInventoryItemId: playerInventoryItemId,
      } as TakingTreeDonateItemDto,
    });

    this.updateCollectionFromDtos(inventoryItemDtos);
  }

  @action
  public async claimItem(takingTreeInventoryItemId: string): Promise<void> {
    // Add to collection of items being claimed
    this._itemsBeingClaimed.push(takingTreeInventoryItemId);

    // Inform the API
    const inventoryItemDtos = await Api.postJson<TakingTreeInventoryItemDto[]>(Endpoints.TakingTree.claimItem(), {
      body: {
        TakingTreeInventoryItemId: takingTreeInventoryItemId,
      } as TakingTreeClaimItemDto,
    });

    // Update collection to remove this item
    this._itemsBeingClaimed = this._itemsBeingClaimed.filter((item) => item !== takingTreeInventoryItemId);

    this.updateCollectionFromDtos(inventoryItemDtos);
  }

  public isItemBeingClaimed(takingTreeInventoryItemId: string): boolean {
    return this._itemsBeingClaimed.includes(takingTreeInventoryItemId);
  }

  @action
  private updateCollectionFromDtos(inventoryItemDtos: TakingTreeInventoryItemDto[]): void {
    this._inventoryItems = inventoryItemDtos.map((dto) => new TakingTreeInventoryItem(dto));
  }

  @computed
  public get inventoryItems(): TakingTreeInventoryItem[] | null {
    return this._inventoryItems;
  }
}

interface TakingTreeDonateItemDto {
  PlayerInventoryItemId: string;
}

interface TakingTreeClaimItemDto {
  TakingTreeInventoryItemId: string;
}

export default TakingTreeStore;
