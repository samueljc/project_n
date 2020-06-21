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
  public Inventory Planter;

  /// <summary>
  /// The tree inventory.
  /// </summary>
  public Inventory Tree;

  /// <summary>
  /// The number of weeds in the yard.
  /// </summary>
  public Inventory Lawn;

  /// <summary>
  /// Factory used for generating weeds.
  /// </summary>
  [SerializeField]
  private PortableItemFactory weedFactory;

  /// <summary>
  /// Factory used for generating fruit.
  /// </summary>
  [SerializeField]
  private PortableItemFactory fruitFactory;

  /// <summary>
  /// Clears the planter and adds new, randomly selected items.
  /// </summary>
  /// <param name="plants">The items to pull from when filling the planter.</param>
  public void Repopulate(List<PortableItem> plants) {
    this.Lawn.Clear();
    this.Tree.Clear();
    this.Planter.Clear();

    // Planting weeds...
    int weeds = StaticRandom.Range(0, this.Lawn.capacity);
    for (int i = 0; i < weeds; ++i) {
      this.Lawn.Add(this.weedFactory.CreateRandomItem(), true);
    }
    // Grafting fruit...
    int fruits = StaticRandom.Range(0, this.Tree.capacity * 2);
    for (int i = 0; i < fruits; ++i) {
      this.Tree.Add(this.fruitFactory.CreateRandomItem(), true);
    }
    // Planting flowers...
    for (int i = 0; i < this.Planter.capacity; ++i) {
      // Remove from the back of the cache for O(1).
      this.Planter.Add(plants[plants.Count - 1], true);
      plants.RemoveAt(plants.Count - 1);
    }
  }
}
