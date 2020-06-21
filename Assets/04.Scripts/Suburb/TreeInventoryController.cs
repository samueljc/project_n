using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A planter inventory controller.
/// </summary>
public class TreeInventoryController : InventoryCellController {
  /// <summary>
  /// The dialog event handler for the shelf inventory.
  /// </summary>
  [SerializeField]
  private DialogEvent playerDialogEvent;

  /// <summary>
  /// The player's wallet.
  /// </summary>
  [SerializeField]
  private IntVariable playerWallet;

  /// <summary>
  /// The current price of fruit in this inventory cell.
  /// </summary>
  private int fruitPrice = 0;

  /// <inheritdoc />
  protected override void OnEnable() {
    PortableItem fruit = this.inventory[this.index];
    if (fruit != null) {
      this.fruitPrice = fruit.price;
    }
    this.inventory.AddCellChangedHandler(this.index, this.PayForFruit);
    base.OnEnable();
  }

  /// <inheritdoc />
  protected override void OnDisable() {
    this.inventory.RemoveCellChangedHandler(this.index, this.PayForFruit);
    base.OnDisable();
  }

  /// <summary>
  /// Deduct the value of the fruit from the player's wallet.
  /// </summary>
  private void PayForFruit() {
    if (this.fruitPrice > 0) {
      this.playerWallet.value -= this.fruitPrice;
    }
  }

  /// <summary>
  /// Only take items we can afford.
  /// </summary>
  /// <param name="item">The item of interest.</param>
  /// <returns>True if we can afford the item, false otherwise.</returns>
  public override bool CanTakeItem(PortableItem item) {
    if (item.price <= playerWallet.value) {
      return true;
    }
    this.SayInsufficientFunds();
    return false;
  }

  /// <inheritdoc />
  /// <remarks>
  /// Tree inventory can only be taken from; cannot be added to.
  /// </remarks>
  public override void OnDrop(PointerEventData eventData) {
    return;
  }

  /// <inheritdoc />
  protected override void HandleDropError(InventoryError error) {
    // this doesn't support drops, so do nothing
  }

  /// <summary>
  /// Display an invalid item message.
  /// </summary>
  private void SayInsufficientFunds() {
    string text = LocalizationManager.GetText(MessageKey.Tree_InsufficientFunds_1);
    this.playerDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
