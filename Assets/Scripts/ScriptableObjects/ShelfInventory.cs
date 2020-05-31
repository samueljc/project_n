using UnityEngine;

[CreateAssetMenu(fileName = "New Shelf Inventory", menuName = "Scriptable Objects/Inventories/Shelf Inventory")]
public class ShelfInventory : Inventory {
  public override bool Add(PortableItem item) {
    // TODO: only allow store items
    return base.Add(item);
  }
}
