using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller for navigating on the map.
/// </summary>
public class MapController : MonoBehaviour {
  /// <summary>
  /// The state of the suburb.
  /// </summary>
  [SerializeField]
  private SuburbState suburb;

  /// <summary>
  /// The player's dialog event bus.
  /// </summary>
  [SerializeField]
  private DialogEvent playerDialogEvent;

  /// <summary>
  /// Navigate to the store.
  /// </summary>
  public void GoToStore() {
    SceneManager.LoadScene("Konbini");
  }

  /// <summary>
  /// Navigate to the video store.
  /// </summary>
  public void GoToVideoStore() {
    SceneManager.LoadScene("Blockbuster");
  }

  /// <summary>
  /// Navigate to the suburb.
  /// </summary>
  public void GoToSuburb() {
    if (suburb.Done) {
      string text = LocalizationManager.GetText("suburb/unwelcome");
      playerDialogEvent.Raise(new DialogCue(text, 2f));
    } else {
      SceneManager.LoadScene("Suburb");
    }
  }

  /// <summary>
  /// Navigate to the beach.
  /// </summary>
  public void GoToBeach() {
    SceneManager.LoadScene("Beach");
  }
}
