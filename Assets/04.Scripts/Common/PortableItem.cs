using UnityEngine;

/// <summary>
/// A item that can be moved and placed in inventories.
/// </summary>
public class PortableItem : ScriptableObject {
  /// <summary>
  /// Delegate for handling the removal of the item from an inventory.
  /// </summary>
  public delegate void RemovedHandler();

  /// <summary>
  /// The unchanging item details.
  /// </summary>
  public PortableItemDetails details;

  /// <summary>
  /// Callback for when the item is removed from an inventory.
  /// </summary>
  /// <remarks>
  /// This is not an <c>event</c> as there should only ever be at most one.
  /// </remarks>
  private RemovedHandler removed;

  /// <inheritdoc cref="M:PortableItemDetails.name" />
  public new string name {
    get { return this.details.name; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.description" />
  public string description {
    get { return this.details.description; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.storeObject" />
  public bool storeObject {
    get { return this.details.storeObject; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.price" />
  public int price {
    get { return this.details.price; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.inventorySprite" />
  public Sprite inventorySprite {
    get { return this.details.inventorySprite; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.draggingSprite" />
  public Sprite draggingSprite {
    get { return this.details.draggingSprite; }
  }

  /// <inheritdoc cref="M:PortableItemDetails.shelfWidth" />
  public float shelfWidth {
    get { return this.details.shelfWidth; }
  }

  /// <summary>
  /// Take the item from its current inventory and put it in a new one.
  /// </summary>
  /// <param name="onRemoved">
  /// A removal callback for when the item is removed from its new inventory.
  /// </param>
  public void Take(RemovedHandler onRemoved) {
    this.removed?.Invoke();
    this.removed = onRemoved;
  }
}
