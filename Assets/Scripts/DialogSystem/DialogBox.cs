using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public struct DialogCue {
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
  /// Create a new dialog cue for a dialog box.
  /// </summary>
  public DialogCue(string text, float advanceTimer = float.PositiveInfinity, float writeDelay = 0.05f) {
    this.text = text;
    this.advanceTimer = advanceTimer;
    this.writeDelay = writeDelay;
  }
}

public class DialogBox : MonoBehaviour {
  /// <summary>
  /// Handler for dismissed events.
  /// </summary>
  public delegate void DismissedHandler();

  /// <summary>
  /// Event handlers for dismissed events.
  /// </summary>
  public event DismissedHandler dismissed;

  /// <summary>
  /// The text game object to render to.
  /// </summary>
  [SerializeField]
  private TextMeshProUGUI text;

  /// <summary>
  /// The cues this dialog box should show.
  /// </summary>
  private List<DialogCue> cues;

  /// <summary>
  /// Index of the current cue.
  /// </summary>
  private int currentCueIndex;

  /// <summary>
  /// Whether a click will dismiss.
  /// </summary>
  private bool populating;

  /// <summary>
  /// The populate coroutine.
  /// </summary>
  private Coroutine populateCoroutine;

  /// <summary>
  /// The auto-advance coroutine.
  /// </summary>
  private Coroutine autoAdvanceCoroutine;

  /// <summary>
  /// Show the given content in the dialog box.
  /// </summary>
  /// <param name="cues">The cues to display.</param>
  public void Show(params DialogCue[] cues) {
    this.Show(new List<DialogCue>(cues));
  }

  /// <summary>
  /// Iteratively show the given content in the dialog box.
  /// </summary>
  /// <param name="cues">A list of cues to show sequentially.</param>
  public void Show(List<DialogCue> cues) {
    this.ResetCoroutines();
    this.ResetState();
    this.cues = cues;

    // Start showing the content.
    foreach (Transform child in transform) {
      child.gameObject.SetActive(true);
    }

    // Advance to the first cue.
    this.Advance();
  }

  /// <summary>
  /// Advance the dialog.
  /// </summary>
  /// <remarks>
  /// If the dialog is currently populating it will skip to the end.
  /// If the dialog is done populating it will go to the next cue.
  /// If there are no more remaining cues the dialog will be dismissed.
  /// </remarks>
  public void Advance() {
    this.ResetCoroutines();
    if (this.populating) {
      // Skip to the end of populating.
      this.text.text = this.cues[currentCueIndex].text;
      this.populating = false;
    } else if (currentCueIndex < this.cues.Count - 1) {
      // Start showing the next item in the list.
      this.text.text = "";
      this.currentCueIndex += 1;
      this.populateCoroutine = this.StartCoroutine(this.Populate());
    } else {
      // All done; dismiss the dialog box.
      this.Dismiss();
    }
  }

  /// <summary>
  /// Dismiss the dialog box.
  /// </summary>
  public void Dismiss() {
    this.ResetCoroutines();
    this.ResetState();
    foreach (Transform child in transform) {
      child.gameObject.SetActive(false);
    }
    // We want to invoke at the end in case we end up calling `Show` in
    // the callback.
    this.dismissed?.Invoke();
  }

  /// <summary>
  /// Populate the on screen text over time.
  /// </summary>
  IEnumerator Populate() {
    this.populating = true;
    DialogCue cue = this.cues[this.currentCueIndex];

    // Iterate through the text showing 1 character at a time.
    foreach (char c in cue.text) {
      this.text.text += c;
      if (cue.writeDelay < 0.01f) {
        // At 60 fps this would be less than 1 frame per second, so don't
        // bother allocating a `WaitForSeconds`.
        yield return null;
      } else {
        yield return new WaitForSeconds(cue.writeDelay);
      }
    }

    // Setup auto-advance.
    if (!float.IsPositiveInfinity(cue.advanceTimer)) {
      this.autoAdvanceCoroutine = this.StartCoroutine(this.AutoAdvance(cue.advanceTimer));
    }

    // Give a short delay to make sure people don't try to "skip"
    // the populate and dismiss the dialog.
    yield return new WaitForSeconds(0.2f);
    this.populating = false;
  }

  /// <summary>
  /// Initialize an auto-advance coroutine that will wait a delay and then
  /// move the dialog to the next cue.
  /// </summary>
  /// <param name="delay">Delay to wait in seconds before advancing.</param>
  IEnumerator AutoAdvance(float delay) {
    // This could be done with invoke as it works the same way invoke does,
    // but I've opted for a coroutine here anyway.
    yield return new WaitForSeconds(delay);
    this.Advance();
  }

  /// <summary>
  /// Stop running coroutines used to control the dialog box.
  /// </summary>
  private void ResetCoroutines() {
    if (this.populateCoroutine != null) {
      this.StopCoroutine(this.populateCoroutine);
      this.populateCoroutine = null;
    }
    if (this.autoAdvanceCoroutine != null) {
      this.StopCoroutine(this.autoAdvanceCoroutine);
      this.autoAdvanceCoroutine = null;
    }
  }

  /// <summary>
  /// Reset the dialog box to clear it for new content.
  /// </summary>
  private void ResetState() {
    this.populating = false;
    this.text.text = "";
    this.cues = null;
    // We start at -1 because advance will take us to the next item which
    // is 0 in this case.
    this.currentCueIndex = -1;
  }
}