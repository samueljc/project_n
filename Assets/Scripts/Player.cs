using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
  [SerializeField]
  private Location _location;

  public Inventory inventory;
  public int wallet;

  public Location location {
    get { return this._location; }
  }

  public void SetPlayerLocation(Location location) {
    this._location = location;
  }
}
