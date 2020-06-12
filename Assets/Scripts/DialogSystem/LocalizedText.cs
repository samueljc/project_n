using UnityEngine;
using TMPro;

/// <summary>
/// Specify a copy key for a sibling text component.
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour {
  /// <summary>
  /// The key used to get the copy text.
  /// </summary>
  public MessageKey copyKey;

  /// <summary>
  /// A sibling text mesh pro component.
  /// </summary>
  private TextMeshProUGUI textGUI;

  /// <inheritdoc />
  void Awake() {
    this.textGUI = this.GetComponent<TextMeshProUGUI>();
  }

  /// <inheritdoc />
  void Start() {
    this.textGUI.text = LocalizationManager.GetText(copyKey);
  }
}
