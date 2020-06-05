using UnityEngine;

public class PortableItem : ScriptableObject {
  public delegate void RemoveHandler();

  public RemoveHandler onRemoved;
  public PortableItemDetails details;

  public new string name {
    get { return this.details.name; }
  }

  public string description {
    get { return this.details.description; }
  }

  public bool storeObject {
    get { return this.details.storeObject; }
  }

  public int price {
    get { return this.details.price; }
  }

  public Sprite inventorySprite {
    get { return this.details.inventorySprite; }
  }

  public Sprite draggingSprite {
    get { return this.details.draggingSprite; }
  }

  public float shelfWidth {
    get { return this.details.shelfWidth; }
  }

  /**
   * Take the current object from whatever owns it by calling its removal
   * delegate and set a new delegate.
   */
  public void Take(RemoveHandler onRemoved) {
    if (this.onRemoved != null) {
      this.onRemoved();
    }
    this.onRemoved = onRemoved;
  }
}
