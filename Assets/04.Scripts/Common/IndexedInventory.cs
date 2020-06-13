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
[CreateAssetMenu(fileName="New Indexed Inventory", menuName="Scriptable Objects/Inventories/Indexed Inventory")]
public class IndexedInventory : ScriptableObject, IEnumerable {
  /// <summary>
  /// Delegate for handling changes to the inventory.
  /// </summary>
  public delegate void ChangedHandler();

  /// <summary>
  /// Event handlers for change events.
  /// </summary>
  private event ChangedHandler changed;

  /// <summary>
  /// Indexed cell change handlers.
  /// </summary>
  private ChangedHandler[] cellChanged;

  /// <summary>
  /// The max capacity of the inventory.
  /// </summary>
  public int capacity = 10;

  /// <summary>
  /// A list containing the inventory's <c>PortableItem</c>s.
  /// </summary>
  [NonSerialized]
  protected PortableItem[] items;

  /// <summary>
  /// Count the number of items in the inventory.
  /// </summary>
  public int count {
    get {
      int count = 0;
      foreach (PortableItem item in this.items) {
        if (item != null) {
          ++count;
        }
      }
      return count;
    }
  }

  /// <summary>
  /// Access a <c>PortableItem</c> by index.
  /// </summary>
  public PortableItem this[int index] {
    get { return this.items[index]; }
    set { this.items[index] = value; }
  }

  /// <inheritdoc />
  void OnEnable() {
    this.items = new PortableItem[this.capacity];
    this.cellChanged = new ChangedHandler[this.capacity];
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
  /// Adds a cell changed handler to listen to changes to the specified
  /// inventory index.
  /// </summary>
  /// <param name="index">The inventory index.</param>
  /// <param name="handler">The handler to call when changed.</param>
  public void AddCellChangedHandler(int index, ChangedHandler handler) {
    if (index < 0 || index >= this.cellChanged.Length) {
      throw new IndexOutOfRangeException("Invalid cell index");
    }
    this.cellChanged[index] += handler;
  }

  /// <summary>
  /// Removes a cell changed handler listening to the specified index.
  /// </summary>
  /// <param name="index">The inventory index.</param>
  /// <param name="handler">The handler to remove.</param>
  public void RemoveCellChangedHandler(int index, ChangedHandler handler) {
    if (index < 0 || index >= this.cellChanged.Length) {
      throw new IndexOutOfRangeException("Invalid cell index");
    }
    this.cellChanged[index] -= handler;
  }

  /// <summary>
  /// Set the item at the given index.
  /// </summary>
  /// <param name="index">Index to use.</param>
  /// <param name="item">Item to use.</param>
  /// <returns>Inventory error indicating the status of the set.</returns>
  public virtual InventoryError Set(int index, PortableItem item) {
    if (item == null) {
      return InventoryError.InvalidItem;
    }
    if (index < 0 || index >= this.items.Length) {
      return InventoryError.OutOfBounds;
    }
    if (this.items[index] == item) {
      return InventoryError.AlreadyExists;
    }

    // Check if the item is already in the inventory and simply swap locations
    // if it is.
    int currentIndex = this.IndexOf(item);
    if (currentIndex != -1) {
      this.items[currentIndex] = this.items[index];
      this.items[index] = item;
      this.changed?.Invoke();
      this.cellChanged[currentIndex]?.Invoke();
      this.cellChanged[index]?.Invoke();
      return InventoryError.NoError;
    }
    // TODO: Do we want to support swapping between different inventories too?

    if (this.items[index] != null) {
      return InventoryError.Occupied;
    }

    item.Take(() => {
      // Need to get the current index as it could have changed from when
      // we first added this item.
      int takeIndex = this.IndexOf(item);
      // -1 should never happen, but better safe than sorry
      if (takeIndex != -1) {
        this.items[takeIndex] = null;
        this.changed?.Invoke();
        this.cellChanged[takeIndex]?.Invoke();
      }
    });
    this.items[index] = item;
    this.changed?.Invoke();
    this.cellChanged[index]?.Invoke();
    return InventoryError.NoError;
  }

  /// <summary>
  /// Get the index of the provided item. If the item doesn't exist will return
  /// -1 instead.
  /// </summary>
  /// <param name="item">The item to look for.</param>
  /// <returns>The index of the item or -1 if not found.</returns>
  public int IndexOf(PortableItem item) {
    for (int i = 0; i < this.items.Length; ++i) {
      if (this.items[i] == item) {
        return i;
      }
    }
    return -1;
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
    return this.IndexOf(item) != -1;
  }

  /// <summary>
  /// Clear the inventory.
  /// </summary>
  public void Clear() {
    for (int i = 0; i < this.items.Length; ++i) {
      this.items[i] = null;
      this.cellChanged[i]?.Invoke();
    }
    this.changed?.Invoke();
  }

  /// <inheritdoc />
  /// <remarks>
  /// Needed for the IEnumerable implementation which is in turn needed to
  /// use foreach loops on this class.
  /// </remarks>
  IEnumerator GetEnumerator() {
    return this.items.GetEnumerator();
  }

  /// <inheritdoc />
  /// <remarks>
  /// Needed for the IEnumerable implementation.
  /// </remarks>
  IEnumerator IEnumerable.GetEnumerator() {
    return this.GetEnumerator();
  }
}
