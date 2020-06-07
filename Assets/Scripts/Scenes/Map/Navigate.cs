using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller for navigating on the map.
/// </summary>
public class Navigate : MonoBehaviour {
  /// <summary>
  /// Navigate to the store.
  /// </summary>
  public void GoToStore() {
    SceneManager.LoadScene("Store");
  }

  /// <summary>
  /// Navigate to the video store.
  /// </summary>
  public void GoToVideoStore() {
    SceneManager.LoadScene("VideoStore");
  }

  /// <summary>
  /// Navigate to the suburb.
  /// </summary>
  public void GoToSuburb() {
    SceneManager.LoadScene("Suburb");
  }

  /// <summary>
  /// Navigate to the beach.
  /// </summary>
  public void GoToBeach() {
    SceneManager.LoadScene("Beach");
  }
}
