using UnityEngine;

/// <summary>
/// A item that can be moved and placed in inventories.
/// </summary>
public class PortableItem : ScriptableObject {
  /// <summary>
  /// The unchanging item details.
  /// </summary>
  public PortableItemDetails details;

  /// <summary>
  /// The inventory this item currently lives in.
  /// </summary>
  public Inventory inventory;

  /// <inheritdoc cref="M:PortableItemDetails.name" />
  public new string name {
    get { return this.details.name; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.description" />
  public string description {
    get { return this.details.description; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.price" />
  public int price {
    get { return this.details.price; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.worldSprite" />
  public Sprite worldSprite {
    get { return this.details.worldSprite; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.inventorySprite" />
  public Sprite inventorySprite {
    get { return this.details.inventorySprite; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.draggingSprite" />
  public Sprite draggingSprite {
    get { return this.details.draggingSprite; }
  }
}
