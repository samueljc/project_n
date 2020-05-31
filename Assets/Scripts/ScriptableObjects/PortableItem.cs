using UnityEngine;

public class PortableItem : ScriptableObject {
  public delegate void RemoveHandler();

  public RemoveHandler onRemoved;
  public PortableItemDetails details;

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
