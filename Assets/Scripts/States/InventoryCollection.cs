using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCollection : ScriptableObject, IEnumerable<PortableItem> {
  public delegate void OnChangeHandler();
  public event OnChangeHandler onChange;

  private int capacity;
  private List<PortableItem> items;

  public InventoryCollection(int capacity = int.MaxValue) {
    this.capacity = capacity;
    // Definitely don't want to try to allocate an int.MaxValue sized array, so
    // take anything over 16 and clamp it.
    this.items = new List<PortableItem>(capacity > 16 ? 16 : capacity);
  }

  public bool Add(PortableItem item) {
    if (item == null) {
      // no null items
      return false;
    }
    if (this.items.Count >= this.capacity) {
      // no room
      return false;
    }
    item.Take(() => {
      this.items.Remove(item);
      this.NotifyChange();
    });
    this.items.Add(item);
    this.NotifyChange();
    return true;
  }

  public bool Contains(PortableItem item) {
    return this.items.Contains(item);
  }

  public void Clear() {
    this.items.Clear();
    NotifyChange();
  }

  // Needed for the IEnumerable implementation which is in turn needed
  // to use foreach loops.
  public IEnumerator<PortableItem> GetEnumerator() {
    return this.items.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return this.GetEnumerator();
  }

  private void NotifyChange() {
    if (this.onChange != null) {
      onChange();
    }
  }
}