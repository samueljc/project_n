using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shelf : MonoBehaviour, IDropHandler {
  public ShelfInventory inventory;

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
    // get the object currently being dragged
    PortableObject obj = eventData.pointerDrag?.GetComponent<PortableObject>();
    if (obj == null) {
      dialogEvent.Raise(Dialog.Store_Shelf_InvalidItem);
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
        dialogEvent.Raise(Dialog.Store_Shelf_InvalidItem);
        break;
      case Error.Inventory_OutOfSpace:
        dialogEvent.Raise(Dialog.Store_Shelf_OutOfSpace);
        break;
    }
  }

  public void Invalidate() {
    this.invalidated = true;
  }

  private void ValidateLayout() {
    // clear existing children
    for (int i = 0; i < this.rectTransform.childCount; ++i) {
      Destroy(this.rectTransform.GetChild(i).gameObject);
    }

    // get all of our objects by type so we can sort them
    // FIXME: could keep the dict around if allocating it is slow
    var itemsByType = new Dictionary<string, List<PortableItem>>();
    foreach (PortableItem item in this.inventory) {
      if (!itemsByType.ContainsKey(item.name)) {
        itemsByType[item.name] = new List<PortableItem>();
      }
      itemsByType[item.name].Add(item);
    }

    // Layout our objects
    float xOffset = 0;
    foreach (var items in itemsByType.Values) {
      foreach (var item in items) {
        PortableObject obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
        obj.item = item;
        RectTransform itemTransform = obj.transform as RectTransform;
        // Set the pivot relative to the sprite's pivot. This ensures our items
        // all appear on the same vertical axis of the shelf.
        itemTransform.pivot = item.forwardSprite.pivot / itemTransform.sizeDelta;
        // Set the anchor to the bottom left.
        itemTransform.anchorMin = Vector2.zero;
        itemTransform.anchorMax = Vector2.zero;
        // Set the anchor position which is the offset from the anchor. We do
        // half first and half later to avoid overlapping due to the different
        // sizes of our objects.
        xOffset += item.width / 2f;
        itemTransform.anchoredPosition = new Vector2(xOffset, 0f);
        xOffset += item.width / 2f + this.inventory.physicalItemGap;
      }
    }
  }
}
