using UnityEngine;

[CreateAssetMenu(fileName = "New Shelf Inventory", menuName = "Scriptable Objects/Inventories/Shelf Inventory")]
public class ShelfInventory : Inventory {
  public float spaceAvailable = 500f;
  public float itemGap = 1f;

  public override bool Add(PortableItem item) {
    if (!item.details.storeObject) {
      // TODO: trigger "that doesn't belong there" dialog
      return false;
    }

    // check for shelf space
    float spaceNeeded = item.width;
    foreach (var i in this) {
      spaceNeeded += i.width + itemGap;
      if (spaceAvailable < spaceNeeded) {
        // TODO: trigger "not enough space" dialog
        return false;
      }
    }

    return base.Add(item);
  }
}
