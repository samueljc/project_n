using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour {
  /// <summary>
  /// A dialog event that this subscribes to for displaying text.
  /// </summary>
  [SerializeField]
  private DialogEvent dialogEvent;

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
  /// The precision to use when evaluating a cue's write delay to 0. If the
  /// difference between the delay and 0 is less than this value then the
  /// text will be shown immediately instead of being populated over time.
  /// </summary>
  private const float writeDelaySkipPopulate = 0.00001f;

  /// <summary>
  /// The current cue that's being processed.
  /// </summary>
  public DialogCue currentCue { 
    get {
      if (this.cues == null || this.currentCueIndex < 0 || this.currentCueIndex >= this.cues.Count) {
        return null;
      }
      return this.cues[this.currentCueIndex];
    }
  }

  /// <inheritdoc />
  void OnEnable() {
    if (this.dialogEvent != null) {
      this.dialogEvent.showDialog += this.Show;
    }
  }

  /// <inheritdoc />
  void OnDisable() {
    this.Dismiss();
    if (this.dialogEvent != null) {
      this.dialogEvent.showDialog -= this.Show;
    }
  }

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
      this.text.text = this.currentCue.text;
      this.populating = false;
    } else if (this.currentCueIndex < this.cues.Count - 1) {
      // Dismiss the current cue.
      this.currentCue?.dismissed?.Invoke();
      this.text.text = "";

      // Advance the cue index and start populating the dialog box.
      this.currentCueIndex += 1;
      if (this.currentCue.writeDelay <= DialogBox.writeDelaySkipPopulate) {
        this.text.text = this.currentCue.text;
      } else {
        this.populateCoroutine = this.StartCoroutine(this.Populate());
      }
    } else {
      DialogCue.DismissedHandler callback = this.currentCue?.dismissed;
      // All done; dismiss the dialog box.
      this.Dismiss();
      // Call the last cue's dismiss delegate.
      callback?.Invoke();
    }
  }

  /// <summary>
  /// Dismiss the dialog box.
  /// </summary>
  /// <remarks>
  /// Invoking this explicitly does not trigger any dismiss callbacks. Advancing
  /// will, but if we're explicitly invoking dismiss it's assumed we're just
  /// trying to close the dialog.
  /// </remarks>
  public void Dismiss() {
    this.ResetCoroutines();
    this.ResetState();
    foreach (Transform child in transform) {
      child.gameObject.SetActive(false);
    }
  }

  /// <summary>
  /// Populate the on screen text over time.
  /// </summary>
  IEnumerator Populate() {
    this.populating = true;
    DialogCue cue = this.currentCue;
    WaitForSeconds delay = new WaitForSeconds(cue.writeDelay);

    // Iterate through the text showing 1 character at a time.
    foreach (char c in cue.text) {
      this.text.text += c;
      yield return delay;
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