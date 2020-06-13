using UnityEngine;

/// <summary>
/// An event for when dialog should be created.
/// </summary>
[CreateAssetMenu(fileName="New Dialog Event", menuName="Scriptable Objects/Events/Dialog Event")]
public class DialogEvent : ScriptableObject {
  /// <summary>
  /// Handler for dialog events.
  /// </summary>
  /// <param name="cues">The dialog cues to show.</param>
  public delegate void DialogHandler(params DialogCue[] cues);

  /// <summary>
  /// Event handlers for dialog events.
  /// </summary>
  public event DialogHandler showDialog;

  /// <summary>
  /// Raise a dialog event to be handled by the dialog handlers.
  /// </summary>
  /// <param name="cues">The dialog cues to show.</param>
  public void Raise(params DialogCue[] cues) {
    this.showDialog?.Invoke(cues);
  }
}
