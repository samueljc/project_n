using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// A store controller.
/// </summary>
public class StoreController : MonoBehaviour {
  /// <summary>
  /// The view for the first aisle.
  /// </summary>
  [SerializeField]
  private GameObject aisle1;

  /// <summary>
  /// The view for the second aisle.
  /// </summary>
  [SerializeField]
  private GameObject aisle2;

  /// <summary>
  /// The view for the checkout counter.
  /// </summary>
  [SerializeField]
  private GameObject checkout;

  /// <summary>
  /// The player's inventory.
  /// </summary>
  [SerializeField]
  private Inventory playerInventory;

  /// <summary>
  /// The player's wallet.
  /// </summary>
  [SerializeField]
  private IntVariable playerWallet;

  /// <summary>
  /// The dialog event handler for the shelf inventory.
  /// </summary>
  [SerializeField]
  private DialogEvent shopkeeperDialogEvent;

  /// <summary>
  /// A reference to the checkout button at the checkout counter.
  /// </summary>
  [SerializeField]
  private Button checkoutButton;

  /// <summary>
  /// A reference to the exit button at the checkout counter.
  /// </summary>
  [SerializeField]
  private Button exitButton;

  /// <summary>
  /// A reference to the start / return to shopping button at the checkout
  /// counter.
  /// </summary>
  [SerializeField]
  private Button shoppingButton;

  /// <summary>
  /// A reference to the cash register text at the checkout counter.
  /// </summary>
  [SerializeField]
  private TMPro.TextMeshProUGUI cashRegisterText;

  /// <inheritdoc />
  void Start() {
    Enter();
  }

  /// <summary>
  /// Change the view to aisle 1.
  /// </summary>
  public void GoToAisle1() {
    this.aisle1.SetActive(true);
    this.aisle2.SetActive(false);
    this.checkout.SetActive(false);
  }

  /// <summary>
  /// Change the view to aisle 2.
  /// </summary>
  public void GoToAisle2() {
    this.aisle1.SetActive(false);
    this.aisle2.SetActive(true);
    this.checkout.SetActive(false);
  }

  /// <summary>
  /// Change the view to the checkout counter.
  /// </summary>
  /// <remarks>
  /// This should update the state to show the right buttons and price on the
  /// cash register.
  /// </remarks>
  public void GoToCheckout() {
    this.aisle1.SetActive(false);
    this.aisle2.SetActive(false);
    this.checkout.SetActive(true);

    // Configure the checkout buttons
    this.shoppingButton.gameObject.SetActive(true);

    int price = this.CalculateCheckoutPrice();
    cashRegisterText.text = "$" + price + ".00";

    if (price == 0) {
      this.exitButton.gameObject.SetActive(true);
      this.checkoutButton.gameObject.SetActive(false);
    } else {
      this.exitButton.gameObject.SetActive(false);
      this.checkoutButton.gameObject.SetActive(true);
    }
  }

  /// <summary>
  /// Enter the store and go to the appropriate view.
  /// </summary>
  /// <remarks>
  /// If you have items in your inventory you should go to the checkout counter
  /// and be told to leave, otherwise you should go to aisle 1.
  /// </remarks>
  public void Enter() {
    // if the player brings something in from outside show the checkout
    // counter and display some dialog directing them outside, otherwise
    // take them directly to the first aisle
    bool denyEntry = this.playerInventory.Count != 0;
    if (denyEntry) {
      this.aisle1.SetActive(false);
      this.aisle2.SetActive(false);
      this.checkout.SetActive(true);

      this.cashRegisterText.text = "$0.00";
      this.exitButton.gameObject.SetActive(true);
      this.checkoutButton.gameObject.SetActive(false);
      this.shoppingButton.gameObject.SetActive(false);
      this.SayNoOutsideItems();
    } else {
      this.GoToAisle1();
    }
  }

  /// <summary>
  /// Attempt to checkout and exit the store.
  /// </summary>
  /// <remarks>
  /// If the player has enough money this will take them back to the map,
  /// otherwise it should dispatch a dialog even to inform them of insufficient
  /// funds.
  /// </remarks>
  public void Checkout() {
    int price = this.CalculateCheckoutPrice();
    if (this.playerWallet.value >= price) {
      this.playerWallet.value -= price;
      SceneManager.LoadScene("Map");
    } else {
      this.SayInsufficientFunds();
    }
  }

  /// <summary>
  /// Exit the store.
  /// </summary>
  public void Exit() {
    SceneManager.LoadScene("Map");
  }

  /// <summary>
  /// Calculate the price of items currently in the player's inventory.
  /// </summary>
  /// <returns>Integer denoting the price of the items.</returns>
  private int CalculateCheckoutPrice() {
    int price = 0;
    foreach (PortableItem item in this.playerInventory) {
      if (item != null) {
        price += item.price;
      }
    }
    return price;
  }

  /// <summary>
  /// Broadcast insufficient funds.
  /// </summary>
  private void SayInsufficientFunds() {
    string text = LocalizationManager.GetText("konbini/shopkeeper/insufficient funds");
    shopkeeperDialogEvent.Raise(new DialogCue(text, 2f));
  }

  /// <summary>
  /// Broadcast no outside items.
  /// </summary>
  private void SayNoOutsideItems() {
    string text = LocalizationManager.GetText("konbini/shopkeeper/no outside items");
    shopkeeperDialogEvent.Raise(new DialogCue(text, 2f));
  }
}
