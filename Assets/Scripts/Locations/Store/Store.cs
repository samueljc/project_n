using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Store : MonoBehaviour {
  [SerializeField]
  private GameObject aisle1;
  [SerializeField]
  private GameObject aisle2;
  [SerializeField]
  private GameObject checkout;

  [SerializeField]
  private Inventory playerInventory;
  [SerializeField]
  private IntVariable playerWallet;
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

  public void Start() {
    Enter();
  }

  public void GoToAisle1() {
    this.aisle1.SetActive(true);
    this.aisle2.SetActive(false);
    this.checkout.SetActive(false);
  }

  public void GoToAisle2() {
    this.aisle1.SetActive(false);
    this.aisle2.SetActive(true);
    this.checkout.SetActive(false);
  }

  public void GoToCheckout() {
    this.aisle1.SetActive(false);
    this.aisle2.SetActive(false);
    this.checkout.SetActive(true);

    // Configure the checkout buttons
    this.shoppingButton.gameObject.SetActive(true);

    int price = this.CalculateCheckoutPrice();
    cashRegisterText.text = "$" + price + ".00";

    if (this.playerInventory.Count == 0) {
      this.exitButton.gameObject.SetActive(true);
      this.checkoutButton.gameObject.SetActive(false);
    } else {
      this.exitButton.gameObject.SetActive(false);
      if (this.playerWallet.value >= price) {
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
    bool allowedToShop = this.playerInventory.Count == 0;
    if (allowedToShop) {
      this.GoToAisle1();
    } else {
      this.aisle1.SetActive(false);
      this.aisle2.SetActive(false);
      this.checkout.SetActive(true);

      this.cashRegisterText.text = "$0.00";
      this.exitButton.gameObject.SetActive(!allowedToShop);
      this.checkoutButton.gameObject.SetActive(allowedToShop);
      this.shoppingButton.gameObject.SetActive(allowedToShop);
      dialogEvent.Raise(Dialog.Store_Checkout_NoOutsideItems);
    }
  }

  public void Exit() {
    this.playerWallet.value -= this.CalculateCheckoutPrice();
    SceneManager.LoadScene("Map");
  }

  private int CalculateCheckoutPrice() {
    int price = 0;
    foreach (var item in this.playerInventory) {
      if (item.storeObject) {
        price += item.price;
      }
    }
    return price;
  }
}
