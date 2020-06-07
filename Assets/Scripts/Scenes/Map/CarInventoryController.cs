using UnityEngine;

/// <summary>
/// Controller for interacting with the player's car inventory.
/// </summary>
public class CarInventoryController : InventoryController {
  /// <summary>
  /// Prefab for generating <c>PortableObject</c>s.
  /// </summary>
  /// <seealso cref="PortableObject" />
  [SerializeField]
  private PortableObject prefab;

  /// <summary>
  /// A dialog event for handling dialog.
  /// </summary>
  [SerializeField]
  private DialogEvent dialogEvent;

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
      PortableObject obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
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
        dialogEvent.Raise(Dialog.CarInventory_InvalidItem);
        break;
      case InventoryError.OutOfSpace:
        dialogEvent.Raise(Dialog.CarInventory_OutOfSpace);
        break;
    }
  }
}
