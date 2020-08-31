import { action, computed, observable } from 'mobx';

import Api from '@app/services/api';
import Endpoints from '@app/constants/endpoints';
import TakingTreeInventoryItem, { TakingTreeInventoryItemDto } from '@app/models/TakingTreeInventoryItem';

import Store from './base/store';

class TakingTreeStore extends Store {
  @observable
  private _inventoryItems: TakingTreeInventoryItem[] | null;

  public constructor() {
    super();
    this._inventoryItems = null;
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
    const inventoryItemDtos = await Api.postJson<TakingTreeInventoryItemDto[]>(Endpoints.TakingTree.claimItem(), {
      body: {
        TakingTreeInventoryItemId: takingTreeInventoryItemId,
      } as TakingTreeClaimItemDto,
    });

    this.updateCollectionFromDtos(inventoryItemDtos);
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
