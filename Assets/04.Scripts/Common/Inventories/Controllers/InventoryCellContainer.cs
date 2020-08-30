using UnityEngine;

/// <summary>
/// Container for inventory cells.
/// </summary>
/// <seealso cref="Inventory" />
public class InventoryCellContainer : MonoBehaviour {
  /// <summary>
  /// THe prefab to use when initializing the cells.
  /// </summary>
  [SerializeField]
  protected InventoryCellController cellPrefab;

  /// <summary>
  /// The prefab to use for the inventory items.
  /// </summary>
  [SerializeField]
  protected PortableItemController itemPrefab;

  /// <summary>
  /// The underlying inventory.
  /// </summary>
  [SerializeField]
  protected Inventory inventory;

  /// <inheritdoc />
  /// <remarks>
  /// This creates all of the cells needed for the inventory. A corresponding
  /// layout controller should take care of arranging them.
  /// </remarks>
  void Awake() {
    for (int i = 0 ; i < this.inventory.Capacity; ++i) {
      InventoryCellController cell = Instantiate<InventoryCellController>(cellPrefab, this.transform);
      cell.Initialize(this.itemPrefab, this.inventory, i);
      // Inventory cells need be disabled by default so that we can initialize
      // them before they try to connect any listeners.
      cell.gameObject.SetActive(true);
    }
  }
}
