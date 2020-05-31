using UnityEngine;

[System.Serializable]
public class StoreShelf {
  public ShelfInventory inventory;

  // TODO: take in a factory to populate the shelves
  public void Repopulate() {
    this.inventory.Clear();
    int newItems = StaticRandom.Range(0, 10);
    for (int i = 0; i < newItems; ++i) {
      this.inventory.Add(new PortableItem());
    }
  }
}

[System.Serializable]
public class StoreAisle {
  public StoreShelf[] shelves;
}

[CreateAssetMenu(fileName="New Store", menuName = "Scriptable Objects/Store")]
public class StoreState : ScriptableObject {
  public StoreAisle[] aisles;
  // TODO: store factory for repopulating?
  
  public void Repopulate() {
    foreach (StoreAisle aisle in this.aisles) {
      foreach (StoreShelf shelf in aisle.shelves) {
        shelf.Repopulate();
      }
    }
  }
}
