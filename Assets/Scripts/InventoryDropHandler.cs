using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InventoryDropHandler : MonoBehaviour, IDropHandler {
  public Inventory inventory;

  protected bool invalidated = true;

  public void OnEnable() {
    this.inventory.onChange += this.Invalidate;
  }

  public void OnDisable() {
    this.inventory.onChange -= this.Invalidate;
  }

  public void LateUpdate() {
    if (this.invalidated) {
      this.ValidateLayout();
      this.invalidated = false;
    }
  }

  public void OnDrop(PointerEventData eventData) {
    // if it's not a portable object what are we doing dragging it into our
    // inventory
    PortableObject obj = eventData.pointerDrag?.GetComponent<PortableObject>();
    if (obj == null) {
      HandleDropError(Error.Inventory_InvalidItem);
      return;
    }
    // We already have this object.
    if (this.inventory.Contains(obj.item)) {
      HandleDropError(Error.Inventory_AlreadyExists);
      return;
    }
    // Try to add it and check for errors.
    HandleDropError(this.inventory.Add(obj.item));
    return;
  }

  public void Invalidate() {
    this.invalidated = true;
  }

  protected abstract void ValidateLayout();

  protected abstract void HandleDropError(Error error);
}