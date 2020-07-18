using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Inventory controller for the trees in the suburb which should have fruit
/// spaced around randomly with no overlap.
/// </summary>
public class TreeInventoryController : InventoryController {
  /// <summary>
  /// Dialog event to notify the player of invalid moves.
  /// </summary>
  [SerializeField]
  private DialogEvent playerDialogEvent;

  /// <summary>
  /// The player's wallet for checking if they're allowed to take fruit.
  /// </summary>
  [SerializeField]
  private IntVariable playerWallet;

  /// <summary>
  /// The minimum space between items.
  /// </summary>
  [SerializeField]
  private float minItemSpacing = 32f;

  /// <inheritdoc />
  private void Start() {
    // Clear all existing children.
    for (int i = 0; i < this.rectTransform.childCount; ++i) {
      Destroy(this.rectTransform.GetChild(i).gameObject);
    }

    // Layout the items.
    Vector2 topRight = new Vector2(this.rectTransform.rect.width, this.rectTransform.rect.height);
    for (int itemIndex = 0; itemIndex < this.inventory.Capacity; ++itemIndex) {
      PortableItem item = this.inventory[itemIndex];
      if (item == null) {
        continue;
      }

      PortableItemController obj = Instantiate(this.prefab, this.rectTransform);
      obj.Initialize(item, this);
      RectTransform itemTransform = obj.transform as RectTransform;
      // Set the anchor to the bottom left of the tree rect.
      itemTransform.anchorMin = Vector2.zero;
      itemTransform.anchorMax = Vector2.zero;
      itemTransform.pivot = new Vector2(0.5f, 0.5f);

      // 100 attempts to place the item with no overlap. There's probably
      // a better approach using some kind of physics algorithm to push
      // overlapping objects apart, but this only needs to happen once per
      // house per day.
      bool remove = true;
      for (int i = 0; i < 100; ++i) {
        bool validPosition = true;
        Vector2 position = StaticRandom.Vector2(Vector2.zero, topRight);
        foreach (RectTransform child in this.rectTransform) {
          if (Vector2.Distance(child.anchoredPosition, position) < this.minItemSpacing) {
            validPosition = false;
            break;
          }
        }
        if (validPosition) {
          itemTransform.anchoredPosition = position;
          remove = false;
          break;
        }
      }

      // Remove this from the inventory because we couldn't place it after
      // 100 attempts and we don't want it to count toward anything.
      if (remove) {
        this.inventory[itemIndex] = null;
      }
    }
  }

  /// <summary>
  /// Only take items we can afford.
  /// </summary>
  /// <param name="item">The item of interest.</param>
  /// <returns>True if we can afford the item, false otherwise.</returns>
  public override bool CanTakeItem(PortableItem item) {
    if (item.price <= this.playerWallet.value) {
      return true;
    }
    this.SayInsufficientFunds();
    return false;
  }

  /// <summary>
  /// Take the fruit; charge the player; delete the game object.
  /// </summary>
  protected override void ValidateLayout() {
    foreach (Transform child in this.rectTransform) {
      PortableItem item = child.gameObject.GetComponent<PortableItemController>()?.Item;
      if (!this.inventory.Contains(item)) {
        child.SetParent(null);
        GameObject.Destroy(child.gameObject);
        if (item.price != 0) {
          this.playerWallet.value -= item.price;
        }
      }
    }
  }

  /// <inheritdoc />
  public override void OnDrop(PointerEventData eventData) {
    // Do nothing; drops are disabled for this inventory.
  }

  /// <inheritdoc />
  protected override void HandleDropError(InventoryError error) {
    // Drops aren't disabled; nothing to do here.
  }

  /// <summary>
  /// Display an invalid item message.
  /// </summary>
  private void SayInsufficientFunds() {
    string text = LocalizationManager.GetText(MessageKey.Tree_InsufficientFunds_1);
    this.playerDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
