using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The state of a shelf.
/// </summary>
[System.Serializable]
public class StoreShelfState {
  /// <summary>
  /// The inventory used by this shelf.
  /// </summary>
  public ShelfInventory inventory;

  /// <summary>
  /// Clears the shelf's inventory and adds new, randomly selected items
  /// based on the provided factory.
  /// </summary>
  /// <param name="factory">
  /// A factory describing items that can be generated on th
  /// </param>
  public void Repopulate(PortableItemFactory factory) {
    this.inventory.Clear();
    int newItems = StaticRandom.Range(0, 10);
    for (int i = 0; i < newItems; ++i) {
      InventoryError err = this.inventory.Add(factory.CreateRandomItem());
      if (err != InventoryError.NoError) {
        Debug.LogFormat("Could not add item to shelf: {0}", err);
      }
    }
  }
}

/// <summary>
/// The state of an aisle.
/// </summary>
[System.Serializable]
public class StoreAisleState {
  /// <summary>
  /// The aisle's shelves.
  /// </summary>
  public StoreShelfState[] shelves;
}

/// <summary>
/// The total state of the store.
/// </summary>
[CreateAssetMenu(fileName="New Store", menuName="Scriptable Objects/State/Store")]
public class StoreState : ScriptableObject {
  /// <summary>
  /// An array of aisles in the store.
  /// </summary>
  [SerializeField]
  private StoreAisleState[] aisles;

  /// <summary>
  /// A factory to use when populating shelves.
  /// </summary>
  [SerializeField]
  private PortableItemFactory factory;

  /// <summary>
  /// Clears the current store state and generates a new one.
  /// </summary>
  public void Repopulate() {
    foreach (StoreAisleState aisle in this.aisles) {
      foreach (StoreShelfState shelf in aisle.shelves) {
        shelf.Repopulate(factory);
      }
    }
  }

  /// <summary>
  /// Record the entropy of the store.
  /// </summary>
  /// <param name="numbers">The numbers for determining entropy.</param>
  /// <param name="journal">The journal for tracking entropy changes.</param>
  public void RecordEntropy(EntropyNumbers numbers, EntropyJournal journal) {
    foreach (StoreAisleState aisle in this.aisles) {
      Dictionary<string, int> aisleState = new Dictionary<string, int>();
      int aisleTotal = 0;
      foreach (StoreShelfState shelf in aisle.shelves) {
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
        journal.Add(JournalSource.Store_Shelf_Types, numbers.Score(shelfState.Count));
        foreach (int number in shelfState.Values) {
          journal.Add(JournalSource.Store_Shelf_TypeCount, numbers.Score(number));
        }
        journal.Add(JournalSource.Store_Shelf_TotalCount, numbers.Score(shelfTotal));
      }

      // compute entropy for the aisle
      journal.Add(JournalSource.Store_Aisle_Types, numbers.Score(aisleState.Count));
      foreach (int number in aisleState.Values) {
        journal.Add(JournalSource.Store_Aisle_TypeCount, numbers.Score(number));
      }
      journal.Add(JournalSource.Store_Aisle_TotalCount, numbers.Score(aisleTotal));
    }
  }
}
