using UnityEngine;
using UnityEngine.EventSystems;

// TODO: I think there's a lot that could be extracted out into a more generic
// Inventory class that inherits from these pieces which we could use for the
// shelves, player inventory, flower pits, etc.
public class PlayerInventory : MonoBehaviour, IDropHandler {
  public Player player;
  public PortableObject prefab;

  private RectTransform rectTransform;
  private bool invalidated;

  public void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
    this.invalidated = true;
  }

  public void Start() {
    this.player.inventory.onChange += this.Invalidate;
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
      return;
    }
    // TODO: do we need any special handling if this fails?
    this.player.AddToInventory(obj.item);
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
    foreach (PortableItem item in this.player.inventory) {
      PortableObject obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
      obj.item = item;
    }
  }
}
