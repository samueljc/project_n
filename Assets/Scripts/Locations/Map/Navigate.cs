using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller for navigating on the map.
/// </summary>
public class Navigate : MonoBehaviour {
  /// <summary>
  /// Go to the store.
  /// </summary>
  public void GoToStore() {
    SceneManager.LoadScene("Store");
  }
}
