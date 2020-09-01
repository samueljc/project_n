using UnityEngine;

/// <summary>
/// An event for broadcasting an item.
/// </summary>
[CreateAssetMenu(fileName="New Portable Item Event", menuName="N/Events/Portable Item Event")]
public class PortableItemEvent : ScriptableObject {
  /// <summary>
  /// Handler for protable item events.
  /// </summary>
  /// <param name="cues">The portable item to broadcast.</param>
  public delegate void PortableItemHandler(PortableItem item);

  /// <summary>
  /// Event handlers for portable item events.
  /// </summary>
  protected event PortableItemHandler broadcast;

  /// <summary>
  /// Add a new handler for portable item events.
  /// </summary>
  /// <param name="handler">A handler to call.</param>
  public void AddHandler(PortableItemHandler handler) {
    this.broadcast += handler;
  }

  /// <summary>
  /// Removes a portable item handler.
  /// </summary>
  /// <param name="handler">The handler to remove.</param>
  public void RemoveHandler(PortableItemHandler handler) {
    this.broadcast -= handler;
  }

  /// <summary>
  /// Broadcast a portable item to be handled by the portable item handler.
  /// </summary>
  /// <param name="item">The item to broadcast.</param>
  public void Raise(PortableItem item) {
    this.broadcast?.Invoke(item);
  }
}

