using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Controller to dispose of an object.
/// </summary>
public abstract class VoidInventoryController : MonoBehaviour, IDropHandler {
  /// <summary>
  /// Items not allowed in this inventory.
  /// </summary>
  /// <remarks>
  /// If the whitelist is set this will be ignored.
  /// </remarks>
  public ItemBlacklist blacklist;

  /// <summary>
  /// The only items allowed in this inventory.
  /// </summary>
  /// <remarks>
  /// If this is set the blacklist will be ignored.
  /// </remarks>
  public ItemWhitelist whitelist;

  /// <inheritdoc />
  /// <remarks>
  /// Takes the dragged object and attempts to delete it. If the object cannot
  /// be deleted the <c>HandleDropError</c> method will be called with the
  /// appropriate <c>InventoryError</c> value.
  /// </remarks>
  public void OnDrop(PointerEventData eventData) {
    PortableItemController obj = eventData.pointerDrag?.GetComponent<PortableItemController>();
    PortableItem item = obj?.Item;
    if (item == null || !item.Filter(this.whitelist, this.blacklist)) {
      this.HandleDropError(InventoryError.InvalidItem);
      return;
    }
    Destroy(eventData.pointerDrag);
    item.inventory.Remove(item);
  }

  /// <summary>
  /// Callback for handling errors raised while attempting to add an item to
  /// the garbage.
  /// </summary>
  /// <param name="error">The <c>InventoryError</c> raised on drop.</param>
  /// <seealso cref="InventoryError" />
  protected abstract void HandleDropError(InventoryError error);
}
