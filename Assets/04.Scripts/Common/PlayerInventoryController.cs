using UnityEngine;

/// <summary>
/// Controller for the player's inventory.
/// </summary>
public class PlayerInventoryController : IndexedInventoryCellController {
  /// <summary>
  /// The dialog event handler for the player inventory.
  /// </summary>
  [SerializeField]
  private DialogEvent playerDialogEvent;

  /// <inheritdoc />
  /// <remarks>
  /// Raises dialog events based on the <c>Error</c>
  /// </remarks>
  protected override void HandleDropError(InventoryError error) {
    switch (error) {
      case InventoryError.InvalidItem:
        this.SayInvalidItem();
        break;
      case InventoryError.OutOfSpace:
        this.SayInventoryFull();
        break;
    }
  }

  /// <summary>
  /// Broadcast an inventory full event.
  /// </summary>
  private void SayInventoryFull() {
    string text = LocalizationManager.GetText(MessageKey.PlayerInventory_OutOfSpace_1);
    playerDialogEvent.Raise(new DialogCue(text, 2f));
  }

  /// <summary>
  /// Broadcast an invalid item.
  /// </summary>
  private void SayInvalidItem() {
    string text = LocalizationManager.GetText(MessageKey.PlayerInventory_InvalidItem_1);
    playerDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
