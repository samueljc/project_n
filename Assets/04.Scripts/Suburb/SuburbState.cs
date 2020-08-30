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
  /// The state of the player.
  /// </summary>
  [SerializeField]
  private PlayerState player;

  /// <summary>
  /// A list of items to purge from the player's inventory at the end of the
  /// day.
  /// </summary>
  [SerializeField]
  private Matcher playerEODBlacklist;

  /// <summary>
  /// A list of items that should not be in the tree at the end of the day.
  /// </summary>
  [SerializeField]
  private Matcher treeEODBlacklist;

  /// <summary>
  /// Denotes if the suburb has been visited for the day.
  /// </summary>
  private bool done = false;

  /// <summary>
  /// Has the suburb been completed today?
  /// </summary>
  public bool Done {
    get { return this.done; }
  }

  /// <summary>
  /// Clear and repopulate the houses.
  /// </summary>
  public void Repopulate() {
    // Generate enough plants to fill the planter of each house with the
    // same type of plant.
    List<PortableItem> plants = new List<PortableItem>();
    for (int h = 0; h < houses.Length; ++h) {
      PortableItem plant = planterFactory.CreateRandomItem();
      for (int i = 0; i < houses[h].Planter.Capacity; ++i) {
        plants.Add(planterFactory.CreateItem(plant));
      }
    }

    // All plants have been made, so shuffle the array.
    plants.Shuffle();

    // Add the plants to the houses.
    foreach (SuburbHouseState house in this.houses) {
      house.Repopulate(plants);
    }

    // Repopulated and ready to be worked.
    this.done = false;
  }

  /// <summary>
  /// Check the houses and add the income to the player's wallet.
  /// </summary>
  public void CashOut() {
    // Can't cashout twice on the same loadout.
    if (this.done) {
      return;
    }

    // Run through the houses and figure out which ones were successfully
    // completed.
    foreach (SuburbHouseState house in this.houses) {
      if (house.Lawn.Count > 0) {
        // TODO: notify house incomplete
        continue;
      }
      foreach (PortableItem item in house.Tree) {
        if (treeEODBlacklist.Matches(item?.details)) {
          // TODO: notify house incomplete
          continue;
        }
      }
      if (house.Planter.Capacity != house.Planter.Count) {
        // TODO: notify house incomplete
        continue;
      }
      this.player.wallet.value += 2;
    }

    // Remove any forbidden items from the player's inventory at the end of
    // the day.
    foreach (PortableItem item in this.player.inventory) {
      if (playerEODBlacklist.Matches(item?.details)) {
        this.player.inventory.Remove(item);
      }
    }

    this.done = true;
  }

  /// <summary>
  /// Record the entropy of the suburb.
  /// </summary>
  /// <param name="numbers">The numbers for determining entropy.</param>
  /// <param name="journal">The journal for tracking entropy changes.</param>
  public void RecordEntropy(EntropyNumbers numbers, EntropyJournal journal) {
    foreach (SuburbHouseState house in this.houses) {
      // TODO: Do you lose points for weeds no matter what or do you gain
      // points if the weeds align with the desired numbers?
      float weedScore = numbers.Score(house.Lawn.Count);
    }
  }
}
