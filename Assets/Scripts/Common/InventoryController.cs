using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Abstract class for controlling interactions with UI inventories.
/// </summary>
/// <seealso cref="Inventory" />
public abstract class InventoryController : MonoBehaviour, IDropHandler {
  /// <summary>
  /// The underlying inventory we want to interact with.
  /// </summary>
  public Inventory inventory;

  /// <summary>
  /// A boolean denoting if we need to re-validate the UI.
  /// </summary>
  protected bool invalidated = true;

  /// <inheritdoc />
  void OnEnable() {
    this.inventory.changed += this.Invalidate;
  }

  /// <inheritdoc />
  void OnDisable() {
    this.inventory.changed -= this.Invalidate;
  }

  /// <inheritdoc />
  /// <remarks>
  /// Will re-validate by calling <c>ValidateLayout</c> if the layout were
  /// invalidated.
  /// </remarks>
  void LateUpdate() {
    if (this.invalidated) {
      this.ValidateLayout();
      this.invalidated = false;
    }
  }

  /// <inheritdoc />
  /// <remarks>
  /// Takes the dragged object and attempts to add it to the inventory. If
  /// the object cannot be added the <c>HandleDropError</c> method will be
  /// called with the appropriate <c>Error</c> value.
  /// </remarks>
  public void OnDrop(PointerEventData eventData) {
    // if it's not a portable object what are we doing dragging it into our
    // inventory
    PortableItemController obj = eventData.pointerDrag?.GetComponent<PortableItemController>();
    if (obj == null) {
      HandleDropError(InventoryError.InvalidItem);
      return;
    }
    // We already have this object.
    if (this.inventory.Contains(obj.item)) {
      HandleDropError(InventoryError.AlreadyExists);
      return;
    }
    // Try to add it and check for errors.
    HandleDropError(this.inventory.Add(obj.item));
    return;
  }

  /// <summary>
  /// Logic for re-validating the view of this inventory whenever the
  /// underlying inventory is changed.
  /// </summary>
  protected abstract void ValidateLayout();

  /// <summary>
  /// Callback for handling errors raise while attempting to add an item to
  /// the inventory.
  /// </summary>
  /// <param name="error">The <c>Error</c> raised while dropping.</param>
  /// <seealso cref="InventoryError" />
  protected abstract void HandleDropError(InventoryError error);

  /// <summary>
  /// Inventory change callback to invalidate the current view and trigger
  /// a redraw.
  /// </summary>
  /// <param name="sender">The inventory that raised the event.</param>
  void Invalidate(object sender) {
    this.invalidated = true;
  }
}