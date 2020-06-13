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
  private MessageKey messageKey;

  /// <summary>
  /// A sibling text mesh pro component.
  /// </summary>
  private TextMeshProUGUI textGUI;

  /// <inheritdoc />
  void Awake() {
    this.textGUI = this.GetComponentInChildren<TextMeshProUGUI>();
  }

  /// <inheritdoc />
  void Start() {
    if (this.messageKey != MessageKey.Ignore) {
      this.textGUI.text = LocalizationManager.GetText(messageKey);
    }
  }
}
