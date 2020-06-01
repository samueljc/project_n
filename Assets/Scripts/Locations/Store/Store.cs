using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour, ILocation {
  [SerializeField]
  private GameObject aisle1;
  [SerializeField]
  private GameObject aisle2;
  [SerializeField]
  private GameObject checkout;
  
  [SerializeField]
  private Player player;
  [SerializeField]
  private DialogEvent dialogEvent;

  [SerializeField]
  private Button checkoutButton;
  [SerializeField]
  private Button exitButton;
  [SerializeField]
  private Button shoppingButton;
  [SerializeField]
  private Text cashRegisterText;

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
    this.shoppingButton.gameObject.SetActive(true);

    int price = this.CalculateCheckoutPrice();
    cashRegisterText.text = "$" + price + ".00";

    if (this.player.inventory.Count == 0) {
      this.exitButton.gameObject.SetActive(true);
      this.checkoutButton.gameObject.SetActive(false);
    } else {
      this.exitButton.gameObject.SetActive(false);
      if (this.player.wallet >= price) {
        this.checkoutButton.gameObject.SetActive(true);
      } else {
        this.checkoutButton.gameObject.SetActive(false);
        dialogEvent.Raise(Dialog.Store_Checkout_InsufficientFunds);
      }
    }
  }

  public void Enter() {
    // if the player brings something in from outside show the checkout
    // counter and display some dialog directing them outside, otherwise
    // take them directly to the first aisle
    bool allowedToShop = this.player.inventory.Count > 0;
    if (allowedToShop) {
      this.player.SetPlayerLocation(Location.STORE_CHECKOUT);
      dialogEvent.Raise(Dialog.Store_Checkout_NoOutsideItems);
    } else {
      this.player.SetPlayerLocation(Location.STORE_AISLE_1);
    }
    this.cashRegisterText.text = "$0.00";
    this.exitButton.gameObject.SetActive(!allowedToShop);
    this.checkoutButton.gameObject.SetActive(allowedToShop);
    this.shoppingButton.gameObject.SetActive(allowedToShop);
  }

  public void Exit() {
    this.player.wallet -= this.CalculateCheckoutPrice();
    this.player.SetPlayerLocation(Location.MAP);
  }

  int CalculateCheckoutPrice() {
    int price = 0;
    foreach (var item in this.player.inventory) {
      if (item.storeObject) {
        price += item.price;
      }
    }
    return price;
  }
}
