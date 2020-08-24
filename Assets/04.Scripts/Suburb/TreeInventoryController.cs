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
  /// The random collider layout we use to find a position for the fruit.
  /// </summary>
  private RandomColliderLayout layout;

  /// <inheritdoc />
  protected override void Awake() {
    this.layout = this.GetComponent<RandomColliderLayout>();
    base.Awake();
  }

  /// <inheritdoc />
  private void Start() {
    // Clear all existing children.
    for (int i = 0; i < this.rectTransform.childCount; ++i) {
      Destroy(this.rectTransform.GetChild(i).gameObject);
    }

    // Layout the items.
    for (int itemIndex = 0; itemIndex < this.inventory.Capacity; ++itemIndex) {
      PortableItem item = this.inventory[itemIndex];
      if (item == null) {
        continue;
      }

      Vector2 position;
      if (this.layout.FindPosition(this.rectTransform, out position)) {
        PortableItemController obj = Instantiate(this.prefab, this.rectTransform);
        obj.Initialize(item, this);
        // Set the anchor to the center because rect.min, rect.max, and the
        // collider are all relative to the center.
        RectTransform itemTransform = obj.transform as RectTransform;
        itemTransform.anchorMin.Set(0.5f, 0.5f);
        itemTransform.anchorMax.Set(0.5f, 0.5f);
        itemTransform.anchoredPosition = position;
        itemTransform.pivot.Set(0.5f, 0.5f);
      } else {
        // Couldn't place it after 100 attempts; clear it from the inventory so
        // we don't count it in the score.
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
    string text = LocalizationManager.GetText("suburb/tree/insufficient funds");
    this.playerDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
