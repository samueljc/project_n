using UnityEngine;

/// <summary>
/// Controller for interacting with the player's car inventory.
/// </summary>
public class CarInventoryController : InventoryController {
  /// <summary>
  /// Prefab for generating <c>PortableObject</c>s.
  /// </summary>
  /// <seealso cref="PortableItemController" />
  [SerializeField]
  private PortableItemController prefab;

  /// <summary>
  /// A dialog event for handling dialog.
  /// </summary>
  [SerializeField]
  private DialogEvent playerDialogEvent;

  /// <summary>
  /// The <c>GameObject</c>s transform.
  /// </summary>
  private RectTransform rectTransform;

  /// <inheritdoc />
  void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
  }

  /// <inheritdoc />
  protected override void ValidateLayout() {
    // clear existing children
    for (int i = 0; i < this.rectTransform.childCount; ++i) {
      Destroy(this.rectTransform.GetChild(i).gameObject);
    }
    // instantiate new ones
    foreach (PortableItem item in this.inventory) {
      PortableItemController obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
      obj.item = item;
    }
  }

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
    string text = LocalizationManager.GetText(MessageKey.CarInventory_InvalidItem_1);
    playerDialogEvent.Raise(new DialogCue(text, 2f));
  }

  /// <summary>
  /// Broadcast that there's no room in the car.
  /// </summary>
  private void SayCarOutOfSpace() {
    string text = LocalizationManager.GetText(MessageKey.CarInventory_InvalidItem_1);
    playerDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
