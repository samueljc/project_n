using UnityEngine;

/// <summary>
/// An abstract list for controlling inventory access.
/// </summary>
public abstract class ItemFilterList : ScriptableObject {
  /// <summary>
  /// A list of items that are permitted.
  /// </summary>
  public PortableItemDetails[] items;

  /// <summary>
  /// Check if the given item (based on the details) is present.
  /// </summary>
  /// <param name="item">The item to compare to.</param>
  /// <returns>Boolean representing if the item is present.</returns>
  public bool Contains(PortableItem item) {
    return this.Contains(item.details);
  }

  /// <summary>
  /// Check if the given item based on the details is present.
  /// </summary>
  /// <param name="details">The details to compare to.</param>
  /// <returns>Boolean representing if the item is present.</returns>
  /// <remarks>
  /// If the list of items is null or empty then this is always false.
  /// </remarks>
  public bool Contains(PortableItemDetails details) {
    foreach (PortableItemDetails d in this.items) {
      if (d == details) {
        return true;
      }
    }
    return false;
  }
}
