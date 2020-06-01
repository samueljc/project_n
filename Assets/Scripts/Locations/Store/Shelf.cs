using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shelf : MonoBehaviour, IDropHandler {
  public PortableObject prefab;
  public ShelfInventory inventory;

  private RectTransform rectTransform;
  private bool invalidated;

  public void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
    this.invalidated = true;
  }

  public void Start() {
    this.inventory.onChange += this.Invalidate;
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
      Debug.Log("Drag object is not a portable object");
      return;
    }
    if (!this.inventory.Add(obj.item)) {
      // TODO: not a store item or full; figure out which and display a
      // dialog or something
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
        itemTransform.pivot = item.forwardSprite.pivot / itemTransform.sizeDelta;
        xOffset += item.width / 2f;
        itemTransform.anchoredPosition = new Vector2(xOffset, 0f);
        itemTransform.anchorMin = Vector2.zero;
        itemTransform.anchorMax = Vector2.zero;
        xOffset += item.width / 2f + this.inventory.itemGap;
      }
    }
  }
}
