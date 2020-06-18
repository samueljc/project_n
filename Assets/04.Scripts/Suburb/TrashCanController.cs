using UnityEngine;

public class TrashCanController : VoidInventoryController {
  /// <summary>
  /// The dialog event handler for the shelf inventory.
  /// </summary>
  [SerializeField]
  private DialogEvent playerDialogEvent;

  /// <inheritdoc />
  protected override void HandleDropError(InventoryError error) {
    switch (error) {
      case InventoryError.InvalidItem:
        this.SayInvalidItem();
        return;
    }
  }

  /// <summary>
  /// Display an invalid item message.
  /// </summary>
  public void SayInvalidItem() {
    string text = LocalizationManager.GetText(MessageKey.TrashCan_InvalidItem_1);
    this.playerDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
