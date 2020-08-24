using UnityEngine;

/// <summary>
/// Controller for interacting with the player's car inventory.
/// </summary>
public class CarInventoryController : InventoryCellController {
  /// <summary>
  /// A dialog event for handling dialog.
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
        this.SayCarInvalidItem();
        break;
      case InventoryError.OutOfSpace:
        this.SayCarOutOfSpace();
        break;
    }
  }

  /// <summary>
  /// Broadcast that we can't put that in the car.
  /// </summary>
  private void SayCarInvalidItem() {
    string text = LocalizationManager.GetText("car/inventory/invalid item");
    playerDialogEvent.Raise(new DialogCue(text, 2f));
  }

  /// <summary>
  /// Broadcast that there's no room in the car.
  /// </summary>
  private void SayCarOutOfSpace() {
    string text = LocalizationManager.GetText("car/inventory/no space");
    playerDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
