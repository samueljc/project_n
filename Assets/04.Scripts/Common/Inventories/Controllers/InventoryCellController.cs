using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Abstract controller for interacting with an inventory cell.
/// </summary>
/// <seealso cref="Inventory" />
public abstract class InventoryCellController : InventoryController {
  /// <summary>
  /// The index this cell represents.
  /// </summary>
  protected int index;

  /// <inheritdoc />
  protected override void OnEnable() {
    this.inventory.AddCellChangedHandler(this.index, this.Invalidate);
  }

  /// <inheritdoc />
  protected override void OnDisable() {
    this.inventory.RemoveCellChangedHandler(this.index, this.Invalidate);
  }

  /// <summary>
  /// Initialize the cell before it can be used.
  /// </summary>
  /// <param name="inventory">The underlying inventory.</param>
  /// <param name="index">The inventory index this cell represents.</param>
  public void Initialize(PortableItemController prefab, Inventory inventory, int index) {
    this.prefab = prefab;
    this.inventory = inventory;
    this.index = index;
  }

  /// <inheritdoc />
  /// <remarks>
  /// Takes the dragged object and attempts to add it to the inventory. If
  /// the object cannot be added the <c>HandleDropError</c> method will be
  /// called with the appropriate <c>Error</c> value.
  /// </remarks>
  public override void OnDrop(PointerEventData eventData) {
    // if it's not a portable object what are we doing dragging it into our
    // inventory
    PortableItemController obj = eventData.pointerDrag?.GetComponent<PortableItemController>();
    if (obj == null) {
      this.HandleDropError(InventoryError.InvalidItem);
      return;
    }

    // Try to add it and check for errors.
    InventoryError err = this.inventory.Set(index, obj.Item);
    if (err != InventoryError.NoError) {
      this.HandleDropError(err);
      return;
    }
  }

  /// <summary>
  /// Logic for re-validating the view of this inventory whenever the
  /// underlying inventory is changed.
  /// </summary>
  protected override void ValidateLayout() {
    // clear existing children
    for (int i = 0; i < this.rectTransform.childCount; ++i) {
      Destroy(this.rectTransform.GetChild(i).gameObject);
    }
    // add a new one
    PortableItem item = this.inventory[this.index];
    if (item != null) {
      PortableItemController obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
      obj.Initialize(item, this);
      RectTransform itemTransform = obj.transform as RectTransform;
      // Center the object in the cell.
      itemTransform.pivot = new Vector2(0.5f, 0.5f);
      itemTransform.anchorMin = new Vector2(0.5f, 0.5f);
      itemTransform.anchorMax = new Vector2(0.5f, 0.5f);
      itemTransform.anchoredPosition = Vector2.zero;
    }
  }
}
