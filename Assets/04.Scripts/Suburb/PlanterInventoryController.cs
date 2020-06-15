using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A planter inventory controller.
/// </summary>
public class PlanterInventoryController : InventoryCellController {
  /// <summary>
  /// The dialog event handler for the shelf inventory.
  /// </summary>
  [SerializeField]
  private DialogEvent playerDialogEvent;

  /// <summary>
  /// The player's inventory.
  /// </summary>
  private Inventory playerInventory;

  void test() {
    // Make sure we have a shovel before allowing the player to dig up any
    // plants.
    if (!this.playerHasShovel()) {
    }
  }

  /// <summary>
  /// Scan the players inventory to see if they have a shovel.
  /// </summary>
  /// <returns>Boolean indicating if the player has a shovel.</returns>
  private bool playerHasShovel() {
    foreach (PortableItem item in this.playerInventory) {
      if (item.shovel) {
        return true;
      }
    }
    return false;
  }

  protected override void HandleDropError(InventoryError error) {
    // TODO: this
    switch (error) {
      case InventoryError.InvalidItem:
        return;
    }
  }
}
