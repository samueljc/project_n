using UnityEngine;

/// <summary>
/// Dialogs notified by dialog events.
/// </summary>
public enum Dialog {
  PlayerInventory_InvalidItem,
  PlayerInventory_OutOfSpace,
  Store_Shelf_InvalidItem,
  Store_Shelf_OutOfSpace,
  Store_Checkout_InsufficientFunds,
  Store_Checkout_NoOutsideItems,
  CarInventory_InvalidItem,
  CarInventory_OutOfSpace,
}

/// <summary>
/// An event for when dialog should be created.
/// </summary>
[CreateAssetMenu(fileName="New Dialog Event", menuName="Scriptable Objects/Events/Dialog Event")]
public class DialogEvent : ScriptableObject {
  /// <summary>
  /// Handler for dialog events.
  /// </summary>
  /// <param name="dialog">The dialog to show.</param>
  public delegate void DialogHandler(Dialog dialog);

  /// <summary>
  /// Event handlers for dialog events.
  /// </summary>
  public event DialogHandler showDialog;

  /// <summary>
  /// Raise a dialog event to be handled by the dialog handlers.
  /// </summary>
  /// <param name="dialog">The dialog to raise.</param>
  public void Raise(Dialog dialog) {
    this.showDialog?.Invoke(dialog);
  }
}
