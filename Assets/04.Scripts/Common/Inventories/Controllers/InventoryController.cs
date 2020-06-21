using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Abstract controller for a full inventory.
/// </summary>
/// <seealso cref="Inventory" />
/// <remarks>
/// This should be used when you want to manage all cells from a single
/// single game object. An example use case of this is if you only ever use
/// add and don't care about the index.
/// </remarks>
public abstract class InventoryController : MonoBehaviour, IDropHandler {
  /// <summary>
  /// Prefab for generating <c>PortableObject</c>s.
  /// </summary>
  /// <seealso cref="PortableItemController" />
  [SerializeField]
  protected PortableItemController prefab;

  /// <summary>
  /// The underlying inventory we want to interact with.
  /// </summary>
  [SerializeField]
  protected Inventory inventory;

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
  protected virtual void OnEnable() {
    this.inventory.AddChangedHandler(this.Invalidate);
  }

  /// <inheritdoc />
  protected virtual void OnDisable() {
    this.inventory.RemoveChangedHandler(this.Invalidate);
  }

  /// <inheritdoc />
  /// <remarks>
  /// Will re-validate by calling <c>ValidateLayout</c> if the layout were
  /// invalidated.
  /// </remarks>
  protected virtual void LateUpdate() {
    if (this.invalidated) {
      this.ValidateLayout();
      this.invalidated = false;
    }
  }

  /// <inheritdoc />
  public virtual bool CanTakeItem(PortableItem item) {
    return true;
  }

  /// <inheritdoc />
  /// <remarks>
  /// Takes the dragged object and attempts to add it to the inventory. If
  /// the object cannot be added the <c>HandleDropError</c> method will be
  /// called with the appropriate <c>InventoryError</c> value.
  /// </remarks>
  public virtual void OnDrop(PointerEventData eventData) {
    // if it's not a portable object what are we doing dragging it into our
    // inventory
    PortableItemController obj = eventData.pointerDrag?.GetComponent<PortableItemController>();
    if (obj == null) {
      this.HandleDropError(InventoryError.InvalidItem);
      return;
    }
    // Try to add it and check for errors.
    this.HandleDropError(this.inventory.Add(obj.Item));
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
    // instantiate new ones
    foreach (PortableItem item in this.inventory) {
      PortableItemController obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
      obj.Initialize(item, this);
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
  protected void Invalidate() {
    this.invalidated = true;
  }
}