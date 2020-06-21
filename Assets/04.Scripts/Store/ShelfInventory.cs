using UnityEngine;

/// <summary>
/// A dynamic inventory that's limited by physical space.
/// </summary>
[CreateAssetMenu(fileName="New Shelf Inventory", menuName="Scriptable Objects/Inventories/Shelf Inventory")]
public class ShelfInventory : Inventory {
  /// <summary>
  /// The physical space in the inventory.
  /// </summary>
  public float physicalSpaceAvailable = 400f;

  /// <summary>
  /// The physical space between items in the inventory.
  /// </summary>
  public float physicalItemGap = 1f;

  /// <inheritdoc />
  /// <remarks>
  /// In addition to performing all of the usual inventory checks this will
  /// also make sure the item physically fits using the item's width and the
  /// remaining inventory width.
  /// </remarks>
  public override InventoryError Add(PortableItem item, bool ignoreFilters = false) {
    if (!ignoreFilters && !this.Supports(item)) {
      return InventoryError.InvalidItem;
    }

    if (this.Contains(item)) {
      return InventoryError.AlreadyExists;
    }

    // Check for space on the shelf.
    float spaceNeeded = item.shelfWidth;
    foreach (PortableItem i in this.items) {
      if (i != null) {
        spaceNeeded += physicalItemGap + i.shelfWidth;
        if (physicalSpaceAvailable < spaceNeeded) {
          return InventoryError.OutOfSpace;
        }
      }
    }

    // It's a valid item and everything will fit. Now put it in the first
    // available slot.
    for (int i = 0; i < this.items.Count; ++i) {
      if (this.items[i] == null) {
        item.inventory?.Remove(item);
        this.UpdateIndex(i, item);
        return InventoryError.NoError;
      }
    }

    // If we couldn't find an empty slot it means we don't have space.
    return InventoryError.OutOfSpace;
  }

  /// <see cref="Add" />
  /// <remarks>
  /// This type of inventory doesn't really support setting an item by the
  /// index so this just proxies to <c>Add</c> and drops the index.
  /// </remarks>
  public override InventoryError Set(int index, PortableItem item, bool ignoreFilters = false) {
    return this.Add(item, ignoreFilters);
  }
}
