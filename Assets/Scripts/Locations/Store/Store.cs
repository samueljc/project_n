using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour, ILocation {
  public GameObject aisle1;
  public GameObject aisle2;
  public GameObject checkout;

  public Player player;

  public void Awake() {}

  public void Update() {
    this.aisle1.SetActive(false);
    this.aisle2.SetActive(false);
    this.checkout.SetActive(false);
    switch (this.player.location) {
      case Location.STORE_AISLE_1:
        this.aisle1.SetActive(true);
        break;
      case Location.STORE_AISLE_2:
        this.aisle2.SetActive(true);
        break;
      case Location.STORE_CHECKOUT:
        this.checkout.SetActive(true);
        break;
      default:
        Debug.Log("Shouldn't be in the store");
        break;
    }
  }

  public void GoToAisle1() {
    this.player.SetPlayerLocation(Location.STORE_AISLE_1);
  }

  public void GoToAisle2() {
    this.player.SetPlayerLocation(Location.STORE_AISLE_2);
  }

  public void GoToCheckout() {
    this.player.SetPlayerLocation(Location.STORE_CHECKOUT);
  }

  public void Enter() {
    // Default location is the first aisle
    this.player.SetPlayerLocation(Location.STORE_AISLE_1);
  }

  public void Exit() {
    int price = 0;
    foreach (var item in this.player.inventory) {
      // TODO: determine price
      /*
      if (item is StoreItem storeItem && !storeItem.paid) {
        price += storeItem.price;
      }
      */
    }
    if (price > 0) {
      if (this.player.wallet >= price) {
        this.player.wallet -= price;
      } else {
        // TODO: insufficient funds notification; don't allow
        return;
      }
    }
    this.player.SetPlayerLocation(Location.MAP);
  }
}
