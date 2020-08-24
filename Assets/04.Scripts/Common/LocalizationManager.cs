using System.Collections.Generic;

public class LocalizationManager {
  /// <summary>
  /// The singular dialog manager instance.
  /// </summary>
  private static LocalizationManager instance;

  private Dictionary<string, string> messages;

  private LocalizationManager() {
    this.messages = new Dictionary<string, string>(){
      {"yes", "Yes"},
      {"no", "No"},
      // convenience store
      {"konbini/next aisle", "Next Aisle"},
      {"konbini/previous aisle", "Previous Aisle"},
      {"konbini/continue shopping", "Continue Shopping"},
      {"konbini/go to checkout", "Checkout"},
      {"konbini/exit", "Exit"},
      {"konbini/pay & exit", "Pay & Exit"},
      //
      {"konbini/shelf/invalid item", "That doesn't go there."},
      {"konbini/shelf/no space", "There's no space on the shelf."},
      //
      {"konbini/shopkeeper/insufficient funds", "Check the register again, pal, you're coming up short."},
      {"konbini/shopkeeper/no outside items", "Leave your trash outside, I don't want it cluttering up my shelves."},
      //
      {"player/inventory/invalid item", "I can't carry that"},
      {"player/inventory/no space", "I don't have space for that."},
      //
      {"car/inventory/invalid item", "I'm not putting that in my car."},
      {"car/inventory/no space", "My car is too small; it won't fit."},
      //
      {"suburb/confirm finish", "Collect your pay and leave the suburb for the day?"},
      {"suburb/unwelcome", "I've done my job there for today. I'm not welcome back."},
      //
      {"suburb/planter/invalid item", "The only thing that belongs in the planter are plants..."},
      {"suburb/planter/need shovel", "I need a shovel to dig that up."},
      {"suburb/tree/insufficient funds", "I don't have the money to take that."},
      //
      {"trash/invalid item", "I can't just throw that away."},
      //
      {"vhs/clamshell", "VHS (CLAMSHELL}"},
      {"vhs/genre", "GENRE: {0}"},
      {"vhs/rental period", "{0} DAY RENTAL"},
      {"vhs/starring", "STARRING: {0}"},
      {"vhs/director", "DIRECTED BY: {0}"},
      {"vhs/producer", "PRODUCED BY: {0}"},
      {"vhs/rating", "RATING: {0}"},
      {"vhs/duration", "{0} min"},
      {"vhs/color", "Color"},
      {"vhs/b&w", "Black & White"},
      //
      {"movie/studio/universal pictures", "Galactic Pictures"},
      //
      {"movie/genre/horror", "Horror"},
      {"movie/genre/comedy", "Comedy"},
      {"movie/genre/action", "Action"},
      {"movie/genre/adventure", "Adventure"},
      {"movie/genre/drama", "Drama"},
      {"movie/genre/thriller", "Thriller"},
      {"movie/genre/suspense", "Suspense"},
      {"movie/genre/romantic", "Romantic"},
      {"movie/genre/crime", "Crime"},
      {"movie/genre/documentary", "Documentary"},
      {"movie/genre/scifi", "SciFi"},
      {"movie/genre/fantasy", "Fantasy"},
      {"movie/genre/western", "Western"},
      {"movie/genre/childrens", "Childrens"},
      {"movie/genre/musical", "Musical"},
      //
      {"movie/star/kurt russell", "Kurt Russell"},
      //
      {"movie/director/john carpenter", "John Carpenter"},
      //
      {"movie/producer/david foster", "David Foster"},
      //
      {"movie/rating/g", "G"},
      {"movie/rating/pg", "PG"},
      {"movie/rating/pg13", "PG-13"},
      {"movie/rating/r", "R"},
      {"movie/rating/nc17", "NC-17"},
      //
      {"movie/the thing/title", "The Thing"},
      {"movie/the thing/synopsis", ""},
    };
  }

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
  public static string GetText(string key) {
    if (LocalizationManager.instance == null) {
      LocalizationManager.Initialize();
    }
    try {
      return LocalizationManager.instance.messages[key];
    } catch (KeyNotFoundException) {
      throw new KeyNotFoundException($"Localization key \"{key}\" is not defined");
    }
  }

  /// <summary>
  /// Get the text associated with the key and apply formatting to it with the
  /// given parameters.
  /// </summary>
  /// <param name="key">The key of the copy we want.</param>
  /// <param name="args">The params we want to send to the formatter.</param>
  /// <returns>A formatted localized string.</returns>
  public static string GetTextFormat(string key, params object[] args) {
    string text = LocalizationManager.GetText(key);
    return string.Format(text, args);
  }
}
