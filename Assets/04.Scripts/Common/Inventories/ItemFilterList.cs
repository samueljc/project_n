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
    return item != null && this.Contains(item.details);
  }

  /// <summary>
  /// Check if the given item based on the details is present.
  /// </summary>
  /// <param name="details">The details to compare to.</param>
  /// <returns>Boolean representing if the item is present.</returns>
  /// <remarks>
  /// <para>
  /// If the list of items is <c>null</c> or empty then this is always false.
  /// </para>
  /// If the provided details are <c>null</c> then this is false.
  /// </para>
  /// </remarks>
  public bool Contains(PortableItemDetails details) {
    if (details == null) {
      return false;
    }
    foreach (PortableItemDetails d in this.items) {
      if (d == details) {
        return true;
      }
    }
    return false;
  }
}
