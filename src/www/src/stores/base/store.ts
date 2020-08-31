/**
 * Abstract store. Represents anything that can fetch
 * and store data from somewhere. Manages initialisation.
 */
abstract class Store {
  /**
   * Set of async "initialisation" tasks to resolve before the store
   * should be used
   */
  private initialisers: Promise<void>[] = [];

  /**
   * Enqueue an async initialisation task to resolve before the store
   * should be used
   * @param asyncInitialiser Void asynchronous task to perform
   */
  protected addInitialiser(asyncInitialiser: Promise<void>): void {
    this.initialisers.push(asyncInitialiser);
  }

  /**
   * Wait until all initialisers have resolved
   */
  protected async waitUntilInitialised(): Promise<void> {
    await Promise.all(this.initialisers);
  }
}

export default Store;
