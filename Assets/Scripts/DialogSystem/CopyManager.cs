using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Copy {
  PlayerInventory_InvalidItem_1,
  PlayerInventory_OutOfSpace_1,
  StoreShelf_InvalidItem_1,
  StoreShelf_OutOfSpace_1,
  StoreCheckout_InsufficientFunds_1,
  StoreCheckout_NoOutsideItems_1,
}

public class CopyManager : MonoBehaviour {
  /// <summary>
  /// The singular dialog manager instance.
  /// </summary>
  private static CopyManager instance;

  // TODO: load from json file based on selected language
  private static Dictionary<Copy, string> copy = new Dictionary<Copy, string>(){
    {Copy.PlayerInventory_InvalidItem_1, "I can't carry that."},
    {Copy.PlayerInventory_OutOfSpace_1, "I don't have space for that."},
    {Copy.StoreShelf_InvalidItem_1, "That doesn't go there."},
    {Copy.StoreShelf_OutOfSpace_1, "There's no space on the shelf."},
    {Copy.StoreCheckout_InsufficientFunds_1, "Looks like I don't have enough money."},
    {Copy.StoreCheckout_NoOutsideItems_1, "Please leave your things outside."},
  };

  /// <inheritdoc />
  void Awake() {
    if (instance == null) {
      DontDestroyOnLoad(this.gameObject);
      instance = this;
    } else if (instance != this) {
      Destroy(this.gameObject);
    }
  }

  /// <summary>
  /// Get the text string associated with this copy key.
  /// </summary>
  /// <param name="key">The key that identifies the text.</param>
  /// <returns>The text associated with the key.</returns>
  /// <remarks>
  /// If the key isn't found this will return the string representation of
  /// the key itself.
  /// </remarks>
  public string Text(Copy key) {
    if (copy.ContainsKey(key)) {
      return copy[key];
    } else {
      return key.ToString();
    }
  }
}
