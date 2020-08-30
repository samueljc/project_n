using UnityEngine;

/// <summary>
/// The state of a shelf in the movie store.
/// </summary>
[System.Serializable]
public class MovieShelfState {
  public Inventory inventory;

  /// <summary>
  /// Add a new batch of movies to the shelf.
  /// </summary>
  /// <param name="factory">
  /// A factory describing items that can be generated on the shelf.
  /// </param>
  public void Repopulate(PortableItemFactory factory) {
    this.inventory.Clear();
    int newItemCount = StaticRandom.Range(3, this.inventory.Capacity);
    for (int i = 0; i < newItemCount; ++i) {
      InventoryError err = this.inventory.Add(factory.CreateRandomItem());
      if (err != InventoryError.NoError) {
        Debug.LogFormat("Could not add item to movie shelf: {0}", err);
      }
    }
  }
}

/// <summary>
/// The total state of the video store.
/// </summary>
[CreateAssetMenu(fileName="New Bluckbuster", menuName="Scriptable Objects/State/Blockbuster")]
public class BlockbusterState : ScriptableObject {
  /// <summary>
  /// An array of the video store shelves.
  /// </summary>
  [SerializeField]
  private MovieShelfState[] shelves;

  /// <summary>
  /// The list of movies that can appear.
  /// </summary>
  [SerializeField]
  private PortableItemFactory factory;

  /// <summary>
  /// Repopulate the movie store shelves.
  /// </summary>
  public void Repopulate() {
    foreach (MovieShelfState shelf in this.shelves) {
      shelf.Repopulate(factory);
    }
  }
}
