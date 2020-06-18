using System.Collections;
using System.Collections.Generic;

public struct Message {
  public string text { get; }
  public string description { get; }

  public Message(string text, string description = "") {
    this.text = text;
    this.description = description;
  }
}

public class LocalizationManager {
  /// <summary>
  /// The singular dialog manager instance.
  /// </summary>
  private static LocalizationManager instance;

  // TODO: load from json file based on selected language
  private static Dictionary<MessageKey, Message> messages = new Dictionary<MessageKey, Message>(){
    // store buttons
    {
      MessageKey.Store_NextAisle,
      new Message("Next Aisle")
    },
    {
      MessageKey.Store_PreviousAisle,
      new Message("Previous Aisle")
    },
    {
      MessageKey.Store_ReturnToShopping,
      new Message("Continue Shopping")
    },
    {
      MessageKey.Store_GoToCheckout,
      new Message("Checkout")
    },
    {
      MessageKey.Store_Exit,
      new Message("Exit")
    },
    {
      MessageKey.Store_Checkout,
      new Message("Pay & Exit")
    },
    // player inventory dialog
    {
      MessageKey.PlayerInventory_InvalidItem_1,
      new Message("I can't carry that")
    },
    {
      MessageKey.PlayerInventory_OutOfSpace_1,
      new Message("I don't have space for that.")
    },
    // car dialog
    {
      MessageKey.CarInventory_InvalidItem_1,
      new Message("I'm not putting that in my car.")
    },
    {
      MessageKey.CarInventory_OutOfSpace_1,
      new Message("My car is too small; it won't fit.")
    },
    // store dialog
    {
      MessageKey.StoreShelf_InvalidItem_1,
      new Message("That doesn't go there.")
    },
    {
      MessageKey.StoreShelf_OutOfSpace_1,
      new Message("There's no space on the shelf.")
    },
    {
      MessageKey.Shopkeeper_InsufficientFunds_1,
      new Message("Check the register again, pal, you're coming up short.")
    },
    {
      MessageKey.Shopkeeper_NoOutsideItems_1,
      new Message("Leave your trash outside, I don't want it cluttering up my shelves.")
    },
    // suburb dialog
    {
      MessageKey.Planter_InvalidItem_1,
      new Message("The only thing that belongs in the planter are plants...")
    },
    {
      MessageKey.Planter_NeedShovel_1,
      new Message("I need a shovel to dig that up.")
    },
    {
      MessageKey.TrashCan_InvalidItem_1,
      new Message("I can't just throw that away.")
    }
  };

  /// <summary>
  /// Initialize the CopyManager.
  /// </summary>
  /// <remarks>
  /// TODO: take a locale
  /// </remarks>
  public static void Initialize() {
    LocalizationManager.instance = new LocalizationManager();
  }

  /// <summary>
  /// Get the text associated with the key.
  /// </summary>
  /// <param name="key">The key of the copy we want.</param>
  /// <returns>A localized string.</returns>
  public static string GetText(MessageKey key) {
    if (LocalizationManager.instance == null) {
      LocalizationManager.Initialize();
    }
    return LocalizationManager.instance.KeyToText(key);
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
  private string KeyToText(MessageKey key) {
    if (messages.ContainsKey(key)) {
      return messages[key].text;
    } else {
      return key.ToString();
    }
  }
}
