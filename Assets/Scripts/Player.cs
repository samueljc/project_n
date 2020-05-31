using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
  [SerializeField]
  private Locations _location;

  public Inventory inventory;
  public int wallet;

  public Locations location {
    get { return this._location; }
  }

  public void SetPlayerLocation(Locations location) {
    this._location = location;
  }

  /**
   * Tries to add the given portable object to the player's inventory.
   * Returns true on success; false on failure.
   */
  public bool AddToInventory(PortableItem item) {
    return inventory.Add(item);
  }
}
