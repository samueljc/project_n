using UnityEngine;
using UnityEngine.EventSystems;

// TODO: I think there's a lot that could be extracted out into a more generic
// Inventory class that inherits from these pieces which we could use for the
// shelves, player inventory, flower pits, etc.
public class PlayerInventoryDisplay : MonoBehaviour, IDropHandler {
  public Inventory inventory;

  [SerializeField]
  private PortableObject prefab;
  [SerializeField]
  private DialogEvent dialogEvent;

  private RectTransform rectTransform;
  private bool invalidated;

  public void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
    this.invalidated = true;
  }

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
      dialogEvent.Raise(Dialog.PlayerInventory_InvalidItem);
      return;
    }
    // We already have this object.
    if (this.inventory.Contains(obj.item)) {
      return;
    }
    // Try to add it and check for errors.
    Error err = this.inventory.Add(obj.item);
    switch (err) {
      case Error.Inventory_InvalidItem:
        dialogEvent.Raise(Dialog.PlayerInventory_InvalidItem);
        break;
      case Error.Inventory_OutOfSpace:
        dialogEvent.Raise(Dialog.PlayerInventory_OutOfSpace);
        break;
    }
  }

  public void Invalidate() {
    this.invalidated = true;
  }

  public void ValidateLayout() {
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
}
