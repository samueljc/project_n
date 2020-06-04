using UnityEngine;

// TODO: I think there's a lot that could be extracted out into a more generic
// Inventory class that inherits from these pieces which we could use for the
// shelves, player inventory, flower pits, etc.
public class PlayerInventoryDisplay : InventoryDropHandler {
  [SerializeField]
  private PortableObject prefab;
  [SerializeField]
  private DialogEvent dialogEvent;

  private RectTransform rectTransform;

  public void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
  }

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

  protected override void HandleDropError(Error error) {
    switch (error) {
      case Error.Inventory_InvalidItem:
        dialogEvent.Raise(Dialog.PlayerInventory_InvalidItem);
        break;
      case Error.Inventory_OutOfSpace:
        dialogEvent.Raise(Dialog.PlayerInventory_OutOfSpace);
        break;
    }
  }
}
