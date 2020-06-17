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
  [SerializeField]
  private Inventory playerInventory;

  /// <summary>
  /// Item details for the shovel.
  /// </summary>
  [SerializeField]
  private PortableItemDetails shovel;

  /// <inheritdoc />
  /// <remarks>
  /// Don't allow an item to be dragged out of this inventory unless the
  /// player is carrying a shovel.
  /// </remarks>
  public override bool CanTakeItem(PortableItem item) {
    if (!this.PlayerHasShovel()) {
      this.SayNeedShovel();
      return false;
    }
    return true;
  }

  /// <inheritdoc />
  /// <remarks>
  /// Check that the cell is either open or the user has a shovel before
  /// allowing them to swap bushes.
  /// </remarks>
  public override void OnDrop(PointerEventData eventData) {
    if (this.inventory[this.inventoryIndex] != null && !this.PlayerHasShovel()) {
      this.SayNeedShovel();
      return;
    }
    base.OnDrop(eventData);
  }

  /// <summary>
  /// Scan the players inventory to see if they have a shovel.
  /// </summary>
  /// <returns>Boolean indicating if the player has a shovel.</returns>
  private bool PlayerHasShovel() {
    foreach (PortableItem item in this.playerInventory) {
      if (item != null && item.details == shovel) {
        return true;
      }
    }
    return false;
  }

  /// <inheritdoc />
  protected override void HandleDropError(InventoryError error) {
    switch (error) {
      case InventoryError.InvalidItem:
        this.SayInvalidItem();
        return;
    }
  }

  private void SayInvalidItem() {
    string text = LocalizationManager.GetText(MessageKey.Planter_InvalidItem_1);
    this.playerDialogEvent.Raise(new DialogCue(text, 2f));
  }

  private void SayNeedShovel() {
    string text = LocalizationManager.GetText(MessageKey.Planter_NeedShovel_1);
    this.playerDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
