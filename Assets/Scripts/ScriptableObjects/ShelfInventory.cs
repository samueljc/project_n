using UnityEngine;

/// <summary>
/// An inventory that only accepts store items that can sit on a shelf.
/// </summary>
[CreateAssetMenu(fileName="New Shelf Inventory", menuName="Scriptable Objects/Inventories/Shelf Inventory")]
public class ShelfInventory : Inventory {
  /// <summary>
  /// The physical space of the shelf.
  /// </summary>
  public float physicalSpaceAvailable = 500f;

  /// <summary>
  /// The phsical space between items on a shelf.
  /// </summary>
  public float physicalItemGap = 1f;

  /// <summary>
  /// Add a <c>PortableItem</c> to the shelf.
  /// </summary>
  /// <param name="item">The item to be added.</param>
  /// <returns>
  /// An <c>Error</c> denoting any problems adding to the shelf inventory.
  /// </returns>
  /// <remarks>
  /// In addition to performing all of the usual inventory checks this will
  /// also make sure the item physically fits on the shelf using the item's
  /// width and the remaining shelf width.
  /// </remarks>
  public override InventoryError Add(PortableItem item) {
    if (!item.details.storeObject) {
      return InventoryError.InvalidItem;
    }

    // check for shelf space
    float spaceNeeded = item.shelfWidth;
    foreach (var i in this) {
      spaceNeeded += i.shelfWidth + physicalItemGap;
      if (physicalSpaceAvailable < spaceNeeded) {
        return InventoryError.OutOfSpace;
      }
    }

    return base.Add(item);
  }
}
