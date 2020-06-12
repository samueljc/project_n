using System;

/// <summary>
/// A single cue to be fed to a dialogue box.
/// </summary>
[System.Serializable]
public class DialogCue {
  /// <summary>
  /// The text to show.
  /// </summary>
  public string text;

  /// <summary>
  /// The time after the full text has been show to wait until advancing
  /// the cue.
  /// </summary>
  public float advanceTimer;

  /// <summary>
  /// The delay between characters when writing.
  /// </summary>
  public float writeDelay;

  /// <summary>
  /// Handler for dismissed events.
  /// </summary>
  public delegate void DismissedHandler();

  /// <summary>
  /// Event handlers for dismissed events.
  /// </summary>
  public DismissedHandler dismissed;

  /// <summary>
  /// Create a new dialog cue for a dialog box.
  /// </summary>
  public DialogCue(
    string text,
    float advanceTimer = float.PositiveInfinity,
    float writeDelay = 0.02f,
    DismissedHandler dismissed = null
  ) {
    this.text = text;
    this.advanceTimer = advanceTimer;
    this.writeDelay = writeDelay;
    this.dismissed = dismissed;
  }
}
