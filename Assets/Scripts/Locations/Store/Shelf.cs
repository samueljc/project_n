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
    // instantiate new ones
    float offset = 0;
    foreach (PortableItem item in this.inventory) {
      PortableObject obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
      (obj.transform as RectTransform).anchoredPosition = new Vector2(offset, 0f);
      offset += 20f;
      obj.item = item;
    }
  }
}
