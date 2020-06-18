using System.Collections.Generic;
using UnityEngine;

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
