using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The dialog controller for the store.
/// </summary>
// I'm not going to thoroughly document this yet as I want to rework it.
public class StoreDialog : MonoBehaviour {
  [SerializeField]
  private DialogEvent dialogEvent;
  [SerializeField]
  private Text playerText;
  [SerializeField]
  private Text shopkeeperText;

  void OnEnable() {
    this.dialogEvent.showDialog += this.ShowDialog;
  }

  void OnDisable() {
    this.dialogEvent.showDialog -= this.ShowDialog;
    this.CancelInvoke();
  }

  void ShowDialog(Dialog dialog) {
    // TODO: actual localization
    switch (dialog) {
      // player dialog
      case Dialog.PlayerInventory_InvalidItem:
        this.playerText.text = "That... doesn't go there...";
        this.playerText.gameObject.SetActive(true);
        break;
      case Dialog.PlayerInventory_OutOfSpace:
        this.playerText.text = "I can't make that fit...";
        this.playerText.gameObject.SetActive(true);
        break;
      case Dialog.Store_Shelf_InvalidItem:
        this.playerText.text = "That... does not belong on the shelf...";
        this.playerText.gameObject.SetActive(true);
        break;
      case Dialog.Store_Shelf_OutOfSpace:
        this.playerText.text = "There's no space left...";
        this.playerText.gameObject.SetActive(true);
        break;
      // shopkeeper dialog
      case Dialog.Store_Checkout_InsufficientFunds:
        this.shopkeeperText.text = "You don't have enough money to leave.";
        this.shopkeeperText.gameObject.SetActive(true);
        break;
      case Dialog.Store_Checkout_NoOutsideItems:
        this.shopkeeperText.text = "Keep your trash outside.";
        this.shopkeeperText.gameObject.SetActive(true);
        break;
      default:
        Debug.LogErrorFormat("Invalid content: {0}", dialog);
        return;
    }
    this.Invoke("HideDialog", 2f);
  }

  void HideDialog() {
    this.playerText.gameObject.SetActive(false);
    this.shopkeeperText.gameObject.SetActive(false);
}
}
