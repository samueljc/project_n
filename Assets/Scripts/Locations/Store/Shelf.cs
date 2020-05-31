using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shelf : MonoBehaviour, IDropHandler {
  public int aisleIndex;
  public int shelfIndex;

  public PortableObject prefab;

  public WorldState worldState;

  private RectTransform rectTransform;
  private ShelfState shelfState;
  private bool invalidated;

  public void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
    this.invalidated = true;
  }

  public void Start() {
    AisleState aisleState = worldState.storeState.aisleStates[this.aisleIndex];
    this.shelfState = aisleState.shelfStates[this.shelfIndex];
    this.shelfState.inventory.onChange += this.Invalidate;
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
    // TODO: only allow store items
    this.shelfState.AddToShelf(obj.item);
    /*
    if (obj.item is StoreItem storeItem) {
      Debug.Log("Dropping item on shelf");
    } else {
      // TODO: not a store object, so we should show a "that doesn't go there"
      // notice or something
      Debug.Log("Drag object is not a store item");
    }
    */
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
    foreach (var item in this.shelfState.inventory) {
      PortableObject obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
      (obj.transform as RectTransform).anchoredPosition = new Vector2(offset, 0f);
      offset += 20f;
      obj.item = item;
    }
  }
}
