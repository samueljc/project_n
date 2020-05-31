using UnityEngine;

[CreateAssetMenu(fileName = "New Shelf Inventory", menuName = "Scriptable Objects/Inventories/Shelf Inventory")]
public class ShelfInventory : Inventory {
  public override bool Add(PortableItem item) {
    if (!item.details.storeObject) {
      return false;
    }
    return base.Add(item);
  }
}
