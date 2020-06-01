using UnityEngine;

public enum Dialog {
  PlayerInventory_InvalidItem,
  PlayerInventory_OutOfSpace,
  Store_Shelf_InvalidItem,
  Store_Shelf_OutOfSpace,
  Store_Checkout_InsufficientFunds,
  Store_Checkout_NoOutsideItems,
}

[CreateAssetMenu(fileName="New Dialog Event", menuName="Scriptable Objects/Events/Dialog Event")]
public class DialogEvent : ScriptableObject {
  public delegate void DialogEventHandler(Dialog dialog);
  private event DialogEventHandler onDialogEvent;

  public void Raise(Dialog dialog) {
    onDialogEvent(dialog);
  }

  public void RegisterListener(DialogEventHandler listener) {
    onDialogEvent += listener;
  }

  public void UnregisterListener(DialogEventHandler listener) {
    onDialogEvent -= listener;
  }
}
