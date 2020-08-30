using UnityEngine;

public class MovieShelfInventoryController : InventoryController {
  /// <summary>
  /// The player dialog event handler.
  /// </summary>
  [SerializeField]
  private DialogEvent playerDialogEvent;

  /// <summary>
  /// Spacing between shelf items.
  /// </summary>
  [SerializeField]
  private float spacing = 8f;

  /// <inheritdoc />
  protected override void ValidateLayout() {
    // clear existing children
    for (int i = 0; i < this.rectTransform.childCount; ++i) {
      Destroy(this.rectTransform.GetChild(i).gameObject);
    }

    // Layout our objects
    float xOffset = 0;
    foreach (var item in this.inventory) {
      if (item == null) {
        continue;
      }

      PortableItemController obj = Instantiate(this.prefab, Vector3.zero, Quaternion.identity, this.rectTransform);
      obj.Initialize(item, this);
      RectTransform itemTransform = obj.transform as RectTransform;
      // Set the anchor to the bottom left.
      itemTransform.anchorMin = Vector2.zero;
      itemTransform.anchorMax = Vector2.zero;
      // Set the anchor position which is the offset from the anchor. We do
      // half first and half later to avoid overlapping due to the different
      // sizes of our objects.
      itemTransform.anchoredPosition = new Vector2(xOffset, 0f);
      itemTransform.pivot = Vector2.zero;
      xOffset += item.worldSprite.rect.width + spacing;
    }
  }

  /// <inheritdoc />
  protected override void HandleDropError(InventoryError error) {
    switch (error) {
      case InventoryError.InvalidItem:
        this.SayInvalidItem();
        return;
      case InventoryError.OutOfSpace:
        this.SayInventoryFull();
        break;
    }
  }

  /// <summary>
  /// Display an inventory full message.
  /// </summary>
  private void SayInventoryFull() {
    string text = LocalizationManager.GetText("blockbuster/movie shelf/out of space");
    this.playerDialogEvent.Raise(new DialogCue(text, 2f));
  }

  /// <summary>
  /// Display an invalid item message.
  /// </summary>
  private void SayInvalidItem() {
    string text = LocalizationManager.GetText("blockbuster/movie shelf/invalid item");
    this.playerDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
