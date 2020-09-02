using UnityEngine;
using TMPro;

/// <summary>
/// Specify a copy key for a sibling text component.
/// </summary>
public class LocalizedText : MonoBehaviour {
  /// <summary>
  /// The key used to get the copy text.
  /// </summary>
  [SerializeField]
  private string key;

  /// <summary>
  /// The current key of this localized text.
  /// </summary>
  public string Key {
    get { return this.key; }
    set {
      this.SetText(value);
    }
  }

  /// <summary>
  /// A sibling text mesh pro component.
  /// </summary>
  private TextMeshProUGUI textGUI;

  /// <summary>
  /// The text to use when the item is enabled.
  /// </summary>
  private string text;

  /// <inheritdoc />
  private void Awake() {
    this.textGUI = this.GetComponentInChildren<TextMeshProUGUI>();
    if (this.text == null) {
      this.SetText(this.text);
    } else {
      this.SetText(LocalizationManager.GetText(this.key));
    }
  }

  /// <summary>
  /// Set the text to the given message key
  /// </summary>
  /// <param name="key">The key used to identify the string</param>
  public void SetMessage(string key) {
    if (this.key == key || key == "") {
      return;
    }
    this.key = key;
    this.SetText(LocalizationManager.GetText(key));
  }

  /// <summary>
  /// Set the text to the given message key and use the provided args for
  /// formatting the string.
  /// </summary>
  /// <param name="key">The key used to identify the string</param>
  /// <param name="args">The args to use when formatting the string</param>
  public void SetMessage(string key, params object[] args) {
    // can't short-circuit this one unless we want to store the args too
    this.key = key;
    this.SetText(LocalizationManager.GetTextFormat(key, args));
  }

  /// <summary>
  /// Set the localized text to a specific value.
  /// </summary>
  /// <param name="text">The string to use</param>
  /// <remarks>No localization will be attempted.</remarks>
  public void SetText(string text) {
    this.text = text;
    if (this.textGUI != null) {
      this.textGUI.text = this.text;
    }
  }
}
