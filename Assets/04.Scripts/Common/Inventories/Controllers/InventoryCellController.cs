using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Abstract controller for interacting with an inventory cell.
/// </summary>
/// <seealso cref="Inventory" />
public abstract class InventoryCellController : MonoBehaviour, IDropHandler {
  /// <summary>
  /// Prefab for generating <c>PortableObject</c>s.
  /// </summary>
  /// <seealso cref="PortableItemController" />
  [SerializeField]
  protected PortableItemController prefab;

  /// <summary>
  /// The underlying inventory we want to interact with.
  /// </summary>
  public Inventory inventory;

  /// <summary>
  /// The index this cell represents.
  /// </summary>
  public int inventoryIndex;

  /// <summary>
  /// The <c>GameObject</c>s transform.
  /// </summary>
  protected RectTransform rectTransform;

  /// <summary>
  /// A boolean denoting if we need to re-validate the UI.
  /// </summary>
  protected bool invalidated = true;

  /// <inheritdoc />
  void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
  }

  /// <inheritdoc />
  void OnEnable() {
    this.inventory.AddCellChangedHandler(this.inventoryIndex, this.Invalidate);
  }

  /// <inheritdoc />
  void OnDisable() {
    this.inventory.RemoveCellChangedHandler(this.inventoryIndex, this.Invalidate);
  }

  /// <inheritdoc />
  /// <remarks>
  /// Will re-validate by calling <c>ValidateLayout</c> if the layout were
  /// invalidated.
  /// </remarks>
  void LateUpdate() {
    if (this.invalidated) {
      this.ValidateLayout();
      this.invalidated = false;
    }
  }

  /// <inheritdoc />
  /// <remarks>
  /// Takes the dragged object and attempts to add it to the inventory. If
  /// the object cannot be added the <c>HandleDropError</c> method will be
  /// called with the appropriate <c>Error</c> value.
  /// </remarks>
  public void OnDrop(PointerEventData eventData) {
    // if it's not a portable object what are we doing dragging it into our
    // inventory
    PortableItemController obj = eventData.pointerDrag?.GetComponent<PortableItemController>();
    if (obj == null) {
      this.HandleDropError(InventoryError.InvalidItem);
      return;
    }
    // Try to add it and check for errors.
    this.HandleDropError(this.inventory.Set(inventoryIndex, obj.item));
    return;
  }

  /// <summary>
  /// Logic for re-validating the view of this inventory whenever the
  /// underlying inventory is changed.
  /// </summary>
  protected virtual void ValidateLayout() {
    // clear existing children
    for (int i = 0; i < this.rectTransform.childCount; ++i) {
      Destroy(this.rectTransform.GetChild(i).gameObject);
    }
    // add a new one
    PortableItem item = this.inventory[this.inventoryIndex];
    if (item != null) {
      PortableItemController obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
      obj.item = item;
      RectTransform itemTransform = obj.transform as RectTransform;
      // Center the object in the cell.
      itemTransform.pivot = new Vector2(0.5f, 0.5f);
      itemTransform.anchorMin = new Vector2(0.5f, 0.5f);
      itemTransform.anchorMax = new Vector2(0.5f, 0.5f);
      itemTransform.anchoredPosition = Vector2.zero;
    }
  }

  /// <summary>
  /// Callback for handling errors raised while attempting to add an item to
  /// the inventory.
  /// </summary>
  /// <param name="error">The <c>Error</c> raised while dropping.</param>
  /// <seealso cref="InventoryError" />
  protected abstract void HandleDropError(InventoryError error);

  /// <summary>
  /// Inventory change callback to invalidate the current view and trigger
  /// a redraw.
  /// </summary>
  void Invalidate() {
    this.invalidated = true;
  }
}
