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

  [System.NonSerialized]
  public int weeds;

  /// <summary>
  /// Clears the planter and adds new, randomly selected items.
  /// </summary>
  /// <param name="plants">The items to pull from when filling the planter.</param>
  public void Repopulate(List<PortableItem> plants) {
    this.planter.Clear();
    this.weeds = StaticRandom.Range(4, 16);
    for (int i = 0; i < this.planter.capacity; ++i) {
      // Remove from the back of the cache for O(1).
      this.planter.Add(plants[plants.Count - 1]);
      plants.RemoveAt(plants.Count - 1);
    }
  }
}
