using System.Collections.Generic;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// A shelf controller.
/// </summary>
public class ShelfInventoryController : InventoryController {
  /// <summary>
  /// Prefab for generating <c>PortableObject</c>s.
  /// </summary>
  /// <seealso cref="PortableItemController" />
  [SerializeField]
  private PortableItemController prefab;
  
  /// <summary>
  /// The dialog event handler for the shelf inventory.
  /// </summary>
  [SerializeField]
  private DialogEvent playerDialogEvent;

  /// <summary>
  /// The <c>GameObject</c>s transform.
  /// </summary>
  private RectTransform rectTransform;

  /// <inheritdoc />
  void Awake() {
    this.rectTransform = GetComponent<RectTransform>();
  }

  /// <inheritdoc />
  /// <remarks>
  /// Arranges the panel so that objects of the same type are clustered.
  /// </remarks>
  protected override void ValidateLayout() {
    // The underlying inventory should be a ShelfInventory
    ShelfInventory shelf = this.inventory as ShelfInventory;
    if (shelf == null) {
      throw new System.InvalidCastException("Inventory is not a ShelfInventory");
    }

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
        PortableItemController obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
        obj.item = item;
        RectTransform itemTransform = obj.transform as RectTransform;
        // Set the pivot relative to the sprite's pivot. This ensures our items
        // all appear on the same vertical axis of the shelf.
        itemTransform.pivot = item.inventorySprite.pivot / itemTransform.sizeDelta;
        // Set the anchor to the bottom left.
        itemTransform.anchorMin = Vector2.zero;
        itemTransform.anchorMax = Vector2.zero;
        // Set the anchor position which is the offset from the anchor. We do
        // half first and half later to avoid overlapping due to the different
        // sizes of our objects.
        xOffset += item.shelfWidth / 2f;
        itemTransform.anchoredPosition = new Vector2(xOffset, 0f);
        xOffset += item.shelfWidth / 2f + shelf.physicalItemGap;
      }
    }
  }

  /// <inheritdoc />
  /// <remarks>
  /// Raises dialog events based on the <c>Error</c>
  /// </remarks>
  protected override void HandleDropError(InventoryError error) {
    switch (error) {
      case InventoryError.InvalidItem:
        this.SayInvalidItem();
        break;
      case InventoryError.OutOfSpace:
        this.SayInventoryFull();
        break;
    }
  }

  /// <summary>
  /// Broadcast an inventory full event.
  /// </summary>
  private void SayInventoryFull() {
    string text = LocalizationManager.GetText(MessageKey.StoreShelf_OutOfSpace_1);
    playerDialogEvent.Raise(new DialogCue(text, 2f));
  }

  /// <summary>
  /// Broadcast an invalid item.
  /// </summary>
  private void SayInvalidItem() {
    string text = LocalizationManager.GetText(MessageKey.StoreShelf_InvalidItem_1);
    playerDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
