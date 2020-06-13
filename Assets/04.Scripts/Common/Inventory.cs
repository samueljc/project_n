using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of <c>PortableItem</c>s.
/// </summary>
/// <seealso cref="PortableItem" />
/// <remarks>
/// This class includes a change event handler to signify when the underlying
/// list of items has changed.
/// </remarks>
[CreateAssetMenu(fileName="New Inventory", menuName="Scriptable Objects/Inventories/Inventory")]
public class Inventory : ScriptableObject, IEnumerable<PortableItem> {
  /// <summary>
  /// Delegate for handling changes to the inventory.
  /// </summary>
  public delegate void ChangedHandler();

  /// <summary>
  /// Event handlers for change events.
  /// </summary>
  private event ChangedHandler changed;

  /// <summary>
  /// The max capacity of the inventory.
  /// </summary>
  public int capacity = 100;

  /// <summary>
  /// A list containing the inventory's <c>PortableItem</c>s.
  /// </summary>
  [NonSerialized]
  protected List<PortableItem> items;

  /// <summary>
  /// Access a <c>PortableItem</c> by index.
  /// </summary>
  public PortableItem this[int index] {
    get { return this.items[index]; }
    set { this.items[index] = value; }
  }

  /// <summary>
  /// The number of <c>PortableItem</c>s in the inventory.
  /// </summary>
  public int count {
    get { return this.items.Count; }
  }

  /// <inheritdoc />
  void OnEnable() {
    this.items = new List<PortableItem>();
  }

  /// <summary>
  /// Adds a changed handler.
  /// </summary>
  /// <param name="handler">The handler to call on changes.</param>
  public void AddChangedHandler(ChangedHandler handler) {
    this.changed += handler;
  }

  /// <summary>
  /// Removes a changed handler.
  /// </summary>
  /// <param name="handler">The handler to remove.</param>
  public void RemoveChangedHandler(ChangedHandler handler) {
    this.changed -= handler;
  }

  /// <summary>
  /// Add a <c>PortableItem</c> to the inventory.
  /// </summary>
  /// <param name="item">The item to be added.</param>
  /// <returns>
  /// An <c>Error</c> denoting any problems adding to the inventory.
  /// </returns>
  /// <seealso cref="InventoryError" />
  public virtual InventoryError Add(PortableItem item) {
    if (item == null) {
      return InventoryError.InvalidItem;
    }
    if (this.Contains(item)) {
      return InventoryError.AlreadyExists;
    }
    if (this.items.Count >= this.capacity) {
      return InventoryError.OutOfSpace;
    }
    item.Take(() => {
      this.items.Remove(item);
      this.changed?.Invoke();
    });
    this.items.Add(item);
    this.changed?.Invoke();
    return InventoryError.NoError;
  }

  /// <summary>
  /// Check if the item exists in the inventory.
  /// </summary>
  /// <param name="item">The portable item to look for.</param>
  /// <returns>A boolean denoting if the object was present.</returns>
  /// <remarks>
  /// This compares items by reference so you need to pass in the exact
  /// item you want to look for.
  /// </remarks>
  public bool Contains(PortableItem item) {
    return this.items.Contains(item);
  }

  /// <summary>
  /// Clear the inventory.
  /// </summary>
  public void Clear() {
    this.items.Clear();
    this.changed?.Invoke();
  }

  /// <inheritdoc />
  /// <remarks>
  /// Needed for the IEnumerable implementation which is in turn needed to
  /// use foreach loops on this class.
  /// </remarks>
  public IEnumerator<PortableItem> GetEnumerator() {
    return this.items.GetEnumerator();
  }

  /// <inheritdoc />
  /// <remarks>
  /// Needed for the generic IEnumerable implementation.
  /// </remarks>
  IEnumerator IEnumerable.GetEnumerator() {
    return this.GetEnumerator();
  }
}
