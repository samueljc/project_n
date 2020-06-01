using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Scriptable Objects/Inventories/Inventory")]
public class Inventory : ScriptableObject, IEnumerable<PortableItem> {
  public delegate void OnChangeHandler();
  public event OnChangeHandler onChange;

  public int capacity = int.MaxValue;
  protected List<PortableItem> items = new List<PortableItem>();

  public int Count {
    get { return this.items.Count; }
  }

  public void OnEnable() {
    // NOTE: this clears all inventories when they're enabled in order to avoid
    // leaving them with garbage after testing in the editor. If we ever need
    // to enable/disable inventories we need to re-evaluate this.
    this.Clear();
  }

  public virtual Error Add(PortableItem item) {
    if (item == null) {
      return Error.Inventory_InvalidItem;
    }
    if (this.items.Count >= this.capacity) {
      return Error.Inventory_OutOfSpace;
    }
    item.Take(() => {
      this.items.Remove(item);
      this.NotifyChange();
    });
    this.items.Add(item);
    this.NotifyChange();
    return Error.NoError;
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
