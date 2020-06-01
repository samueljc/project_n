using UnityEngine;

[CreateAssetMenu(fileName = "New Shelf Inventory", menuName = "Scriptable Objects/Inventories/Shelf Inventory")]
public class ShelfInventory : Inventory {
  public float physicalSpaceAvailable = 500f;
  public float physicalItemGap = 1f;

  public override Error Add(PortableItem item) {
    if (!item.details.storeObject) {
      return Error.Inventory_InvalidItem;
    }

    // check for shelf space
    float spaceNeeded = item.width;
    foreach (var i in this) {
      spaceNeeded += i.width + physicalItemGap;
      if (physicalSpaceAvailable < spaceNeeded) {
        return Error.Inventory_OutOfSpace;
      }
    }

    return base.Add(item);
  }
}
