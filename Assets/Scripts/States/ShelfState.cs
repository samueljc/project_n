using UnityEngine;

public class ShelfState : ScriptableObject {
  public InventoryCollection inventory;

  public ShelfState() {
    this.inventory = new InventoryCollection(15);
  }

  public void Repopulate() {
    this.inventory.Clear();
    int newItems = StaticRandom.Range(0, 10);
    for (int i = 0; i < newItems; ++i) {
      // TODO: create a factory to pump out appropriate items
      this.inventory.Add(new PortableItem());
    }
  }

  public void AddToShelf(PortableItem item) {
    this.inventory.Add(item);
  }
}
