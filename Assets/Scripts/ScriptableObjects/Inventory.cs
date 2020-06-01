using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Scriptable Objects/Inventories/Inventory")]
public class Inventory : ScriptableObject, IEnumerable<PortableItem> {
  public delegate void OnChangeHandler();
  public event OnChangeHandler onChange;

  public int capacity = int.MaxValue;
  private List<PortableItem> items = new List<PortableItem>();

  public int Count {
    get { return this.items.Count; }
  }

  public void OnEnable() {
    // NOTE: this clears all inventories when they're enabled in order to avoid
    // leaving them with garbage after testing in the editor. If we ever need
    // to enable/disable inventories we need to re-evaluate this.
    this.Clear();
  }

  public virtual bool Add(PortableItem item) {
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