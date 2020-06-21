using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// An inventory that uses a static list of cells to remember an items
/// position within the inventory.
/// </summary>
[CreateAssetMenu(fileName="New Inventory", menuName="Scriptable Objects/Inventories/Inventory")]
public class Inventory : ScriptableObject, IEnumerable<PortableItem> {
  /// <summary>
  /// Delegate for handling changes to the inventory.
  /// </summary>
  public delegate void ChangedHandler();

  /// <summary>
  /// Event handlers for change events.
  /// </summary>
  protected event ChangedHandler changed;

  /// <summary>
  /// The max capacity of the inventory.
  /// </summary>
  public int capacity = 10;

  /// <summary>
  /// Items not allowed in this inventory.
  /// </summary>
  /// <remarks>
  /// If the whitelist is set this will be ignored.
  /// </remarks>
  public ItemBlacklist blacklist;

  /// <summary>
  /// The only items allowed in this inventory.
  /// </summary>
  /// <remarks>
  /// If this is set the blacklist will be ignored.
  /// </remarks>
  public ItemWhitelist whitelist;

  /// <summary>
  /// A list containing the inventory's <c>PortableItem</c>s.
  /// </summary>
  [NonSerialized]
  protected List<PortableItem> items;

  /// <summary>
  /// Indexed cell change handlers.
  /// </summary>
  private ChangedHandler[] cellChanged;


  /// <summary>
  /// Access a <c>PortableItem</c> by index.
  /// </summary>
  public PortableItem this[int index] {
    get { return this.items[index]; }
    set { this.items[index] = value; }
  }

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

  /// <inheritdoc />
  void OnEnable() {
    this.items = new List<PortableItem>(this.capacity);
    for (int i = 0; i < this.capacity; ++i) {
      this.items.Add(null);
    }
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
  /// Checks if the item is supported in this inventory.
  /// </summary>
  /// <param name="item">The item to check.</param>
  /// <returns>True if supported, false otherwise.</returns>
  public bool Supports(PortableItem item) {
    return item.Filter(this.whitelist, this.blacklist);
  }

  /// <summary>
  /// Get the index of the provided item. If the item doesn't exist will return
  /// -1 instead.
  /// </summary>
  /// <param name="item">The item to look for.</param>
  /// <returns>The index of the item or -1 if not found.</returns>
  public int IndexOf(PortableItem item) {
    for (int i = 0; i < this.items.Count; ++i) {
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
    return this.items.Contains(item);
  }

  /// <summary>
  /// See if the inventory contains an item with the given details.
  /// </summary>
  /// <param name="details">The details to look for.</param>
  /// <returns>
  /// A boolean denoting if an item with the details was present.
  /// </returns>
  public bool Contains(PortableItemDetails details) {
    foreach (PortableItem item in this.items) {
      if (item != null && item.details == details) {
        return true;
      }
    }
    return false;
  }

  /// <summary>
  /// Add an item to the first empty cell in the inventory.
  /// </summary>
  /// <param name="item">The item to add.</param>
  /// <param name="ignoreFilters">Ignore the filters when setting this item.</param>
  /// <returns>Inventory error indicating the status of the operation.</returns>
  public virtual InventoryError Add(PortableItem item, bool ignoreFilters = false) {
    if (!ignoreFilters && !this.Supports(item)) {
      return InventoryError.InvalidItem;
    }

    if (this.Contains(item)) {
      return InventoryError.AlreadyExists;
    }

    // Try to find an empty spot for this item.
    for (int i = 0; i < this.items.Count; ++i) {
      if (this.items[i] == null) {
        item.inventory?.Remove(item);
        this.UpdateIndex(i, item);
        return InventoryError.NoError;
      }
    }

    // If we couldn't find an empty slot it means we don't have space.
    return InventoryError.OutOfSpace;
  }

  /// <summary>
  /// Set the item at the given index.
  /// </summary>
  /// <param name="index">Index to use.</param>
  /// <param name="item">Item to use.</param>
  /// <param name="ignoreFilters">Ignore the filters when setting this item.</param>
  /// <returns>Inventory error indicating the status of the operation.</returns>
  public virtual InventoryError Set(int index, PortableItem item, bool ignoreFilters = false) {
    if (!ignoreFilters && !this.Supports(item)) {
      return InventoryError.InvalidItem;
    }

    if (index < 0 || index >= this.items.Count) {
      return InventoryError.OutOfBounds;
    }

    if (this.items[index] == item) {
      return InventoryError.AlreadyExists;
    }

    // Check if the item is already in the inventory and simply swap locations
    // if it is.
    if (item.inventory == this) {
      int currentIndex = this.IndexOf(item);
      this.Shuffle(index, currentIndex);
      return InventoryError.NoError;
    }

    // Index is already occupied, so we'll have to swap the incoming item with
    // the current item in the cell.
    if (this.items[index] != null) {
      return this.Swap(index, item);
    }

    // Valid item and nothing is currently taking up the specified index, so 
    // add the item to the inventory.
    item.inventory?.Remove(item);
    this.UpdateIndex(index, item);
    return InventoryError.NoError;
  }

  /// <summary>
  /// Remove the given item from this inventory.
  /// </summary>
  /// <param name="item">The item to remove.</param>
  public void Remove(PortableItem item) {
    int index = this.IndexOf(item);
    if (index != -1) {
      this.items[index] = null;
      this.OnCellChanged(index);
      this.OnChanged();
    }
  }

  /// <summary>
  /// Clear the inventory.
  /// </summary>
  public void Clear() {
    int cleared = 0;
    for (int i = 0; i < this.items.Count; ++i) {
      if (this.items[i] != null) {
        this.items[i] = null;
        this.OnCellChanged(i);
        ++cleared;
      }
    }
    if (cleared > 0) {
      this.OnChanged();
    }
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

  /// <summary>
  /// Fire an inventory changed event.
  /// </summary>
  protected void OnChanged() {
    this.changed?.Invoke();
  }

  /// <summary>
  /// Fire an inventory changed event for the specified cell.
  /// </summary>
  /// <param name="index">An index in the inventory.</param>
  protected void OnCellChanged(int index) {
    this.cellChanged[index]?.Invoke();
  }

  /// <summary>
  /// Updates the specified index with the given item.
  /// </summary>
  /// <param name="index">Index to update.</param>
  /// <param name="item">Item to use.</param>
  protected void UpdateIndex(int index, PortableItem item) {
    this.items[index] = item;
    item.inventory = this;
    this.OnCellChanged(index);
    this.OnChanged();
  }

  /// <summary>
  /// Shuffle the location of items in this inventory.
  /// </summary>
  /// <param name="index1">First item index.</param>
  /// <param name="index2">Second item index.</param>
  private void Shuffle(int index1, int index2) {
    PortableItem tmp = this.items[index1];
    this.items[index1] = this.items[index2];
    this.items[index2] = tmp;
    this.OnCellChanged(index1);
    this.OnCellChanged(index2);
    this.OnChanged();
  }

  /// <summary>
  /// Swap the location of items across separate inventories.
  /// </summary>
  /// <param name="index">The index to swap into.</param>
  /// <param name="otherItem">The item to swap in.</param>
  /// <returns>An error representing the result of this operation.</returns>
  private InventoryError Swap(int index, PortableItem otherItem) {
    Inventory otherInventory = otherItem.inventory;
    PortableItem thisItem = this.items[index];

    int otherIndex = otherInventory.IndexOf(otherItem);
    // This item does not exist in the other inventory...?
    if (otherIndex == -1) {
      return InventoryError.Unknown;
    }

    // We set these values to null to act like we're trying to insert an
    // element that doesn't belong to an inventory into an empty inventory
    // slot.
    otherInventory[otherIndex] = null;
    thisItem.inventory = null;
    InventoryError err = otherInventory.Set(otherIndex, thisItem);
    if (err != InventoryError.NoError) {
      // Failed to set this item in the other inventory. Revert.
      otherInventory[otherIndex] = otherItem;
      thisItem.inventory = this;
      return err;
    }

    this.UpdateIndex(index, otherItem);
    return InventoryError.NoError;
  }
}
