using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Controller to dispose of an object.
/// </summary>
public abstract class VoidInventoryController : MonoBehaviour, IDropHandler {
  /// <summary>
  /// Handler to call after destroying an item.
  /// </summary>
  /// <param name="item">The item that was destroyed.</param>
  public delegate void DestroyedHandler(PortableItem item);

  private event DestroyedHandler destroyed;

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

  /// <summary>
  /// Add a destroyed handler.
  /// </summary>
  /// <param name="handler">The handler to call.</param>
  public void AddDestroyedHandler(DestroyedHandler handler) {
    this.destroyed += handler;
  }

  /// <summary>
  /// Remove a destroyed handler.
  /// </summary>
  /// <param name="handler">The handler to remove.</param>
  public void RemoveDestroyedHandler(DestroyedHandler handler) {
    this.destroyed -= handler;
  }

  /// <inheritdoc />
  /// <remarks>
  /// Takes the dragged object and attempts to delete it. If the object cannot
  /// be deleted the <c>HandleDropError</c> method will be called with the
  /// appropriate <c>InventoryError</c> value.
  /// </remarks>
  public void OnDrop(PointerEventData eventData) {
    PortableItemController obj = eventData.pointerDrag?.GetComponent<PortableItemController>();
    PortableItem item = obj?.item;
    if (item == null || !item.Filter(this.whitelist, this.blacklist)) {
      this.HandleDropError(InventoryError.InvalidItem);
      return;
    }
    Destroy(eventData.pointerDrag);
    this.destroyed?.Invoke(item);
  }

  /// <summary>
  /// Callback for handling errors raised while attempting to add an item to
  /// the garbage.
  /// </summary>
  /// <param name="error">The <c>InventoryError</c> raised on drop.</param>
  /// <seealso cref="InventoryError" />
  protected abstract void HandleDropError(InventoryError error);
}
