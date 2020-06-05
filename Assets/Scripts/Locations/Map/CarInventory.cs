using UnityEngine;

public class CarInventory : InventoryDropHandler {
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
        dialogEvent.Raise(Dialog.CarInventory_InvalidItem);
        break;
      case Error.Inventory_OutOfSpace:
        dialogEvent.Raise(Dialog.CarInventory_OutOfSpace);
        break;
    }
  }
}
