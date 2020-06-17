using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The state of a house in the suburb.
/// </summary>
[CreateAssetMenu(fileName="New Suburb House", menuName="Scriptable Objects/State/Suburb/House")]
public class SuburbHouseState : ScriptableObject {
  /// <summary>
  /// The planter inventory.
  /// </summary>
  public Inventory planter;

  /// <summary>
  /// Clears the planter and adds new, randomly selected items.
  /// </summary>
  /// <param name="plants">The items to pull from when filling the planter.</param>
  public void Repopulate(List<PortableItem> plants) {
    this.planter.Clear();
    for (int i = 0; i < this.planter.capacity; ++i) {
      // Remove from the back of the cache for O(1).
      this.planter.Add(plants[plants.Count - 1]);
      plants.RemoveAt(plants.Count - 1);
    }
  }
}

/// <summary>
/// The total state of the suburb.
/// </summary>
[CreateAssetMenu(fileName="New Suburb", menuName="Scriptable Objects/State/Suburb/Suburb")]
public class SuburbState : ScriptableObject {
  /// <summary>
  /// An array of houses.
  /// </summary>
  [SerializeField]
  private SuburbHouseState[] houses;

  /// <summary>
  /// A factory to use when repopulating planters.
  /// </summary>
  [SerializeField]
  private PortableItemFactory planterFactory;

  /// <summary>
  /// Clear and repopulate the houses.
  /// </summary>
  public void Repopulate() {
    // Generate enough plants to fill the planter of each house with the
    // same type of plant.
    List<PortableItem> plants = new List<PortableItem>();
    for (int h = 0; h < houses.Length; ++h) {
      PortableItem plant = planterFactory.CreateRandomItem();
      for (int i = 0; i < houses[h].planter.capacity; ++i) {
        plants.Add(planterFactory.CreateItem(plant));
      }
    }

    // All plants have been made, so shuffle the array.
    plants.Shuffle();

    // Add the plants to the houses.
    foreach (SuburbHouseState house in this.houses) {
      house.Repopulate(plants);
    }
  }
}
