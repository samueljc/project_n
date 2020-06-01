using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreDialog : MonoBehaviour {
  [SerializeField]
  private DialogEvent dialogEvent;
  [SerializeField]
  private Text playerText;

  void OnEnable() {
    this.dialogEvent.RegisterListener(this.ShowDialog);
  }

  void OnDisable() {
    this.dialogEvent.UnregisterListener(this.ShowDialog);
    this.CancelInvoke();
  }

  void ShowDialog(Dialog dialog) {
    // TODO: actual localization
    switch (dialog) {
      case Dialog.PlayerInventory_InvalidItem:
        this.playerText.text = "That... doesn't go there...";
        break;
      case Dialog.PlayerInventory_OutOfSpace:
        this.playerText.text = "I can't make that fit...";
        break;
      case Dialog.Store_Shelf_InvalidItem:
        this.playerText.text = "That... does not belong on the shelf...";
        break;
      case Dialog.Store_Shelf_OutOfSpace:
        this.playerText.text = "There's no space left...";
        break;
      default:
        Debug.LogErrorFormat("Invalid content: {0}", dialog);
        return;
    }
    this.playerText.gameObject.SetActive(true);
    this.Invoke("HideDialog", 2f);
  }

  void HideDialog() {
    this.playerText.gameObject.SetActive(false);
  }
}
