using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreShelf {
  public ShelfInventory inventory;

  // TODO: take in a factory to populate the shelves
  public void Repopulate(PortableItemFactory factory) {
    this.inventory.Clear();
    int newItems = StaticRandom.Range(0, 10);
    for (int i = 0; i < newItems; ++i) {
      Error err = this.inventory.Add(factory.CreateRandomItem());
      if (err != Error.NoError) {
        Debug.LogFormat("Could not add item to shelf: {0}", err);
      }
    }
  }
}

[System.Serializable]
public class StoreAisle {
  public StoreShelf[] shelves;
}

[CreateAssetMenu(fileName="New Store", menuName="Scriptable Objects/Store")]
public class StoreState : ScriptableObject {
  [SerializeField]
  private StoreAisle[] aisles;
  [SerializeField]
  private PortableItemFactory factory;
  [SerializeField]
  private Numbers numbers;
  [SerializeField]
  private Journal journal;
  
  public void Repopulate() {
    foreach (StoreAisle aisle in this.aisles) {
      foreach (StoreShelf shelf in aisle.shelves) {
        shelf.Repopulate(factory);
      }
    }
  }

  public void RecordEntropy() {
    List<float> scores = new List<float>();
    foreach (StoreAisle aisle in this.aisles) {
      Dictionary<string, int> aisleState = new Dictionary<string, int>();
      int aisleTotal = 0;
      foreach (StoreShelf shelf in aisle.shelves) {
        Dictionary<string, int> shelfState = new Dictionary<string, int>();
        int shelfTotal = 0;
        foreach (PortableItem item in shelf.inventory) {
          if (!shelfState.ContainsKey(item.name)) {
            shelfState[item.name] = 0;
          }
          shelfState[item.name] += 1;
          shelfTotal += 1;

          if (!aisleState.ContainsKey(item.name)) {
            aisleState[item.name] = 0;
          }
          aisleState[item.name] += 1;
          aisleTotal += 1;
        }

        // compute entropy for the shelf
        this.journal.Add(JournalSource.Store_Shelf_Types, numbers.Score(shelfState.Count));
        foreach (int number in shelfState.Values) {
          this.journal.Add(JournalSource.Store_Shelf_TypeCount, numbers.Score(number));
        }
        this.journal.Add(JournalSource.Store_Shelf_TotalCount, numbers.Score(shelfTotal));
      }

      // compute entropy for the aisle
      this.journal.Add(JournalSource.Store_Aisle_Types, numbers.Score(aisleState.Count));
      foreach (int number in aisleState.Values) {
        this.journal.Add(JournalSource.Store_Aisle_TypeCount, numbers.Score(number));
      }
      this.journal.Add(JournalSource.Store_Aisle_TotalCount, numbers.Score(aisleTotal));
    }
  }
}
