using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuburbController : MonoBehaviour {
  /// <summary>
  /// The state of the suburb this controls.
  /// </summary>
  [SerializeField]
  private SuburbState state;

  /// <summary>
  /// The view for the suburb.
  /// </summary>
  [SerializeField]
  private GameObject suburb;

  /// <summary>
  /// The views for the houses in the suburb.
  /// </summary>
  [SerializeField]
  private GameObject[] houses;

  /// <summary>
  /// Dialog to show for confirming exit.
  /// </summary>
  [SerializeField]
  private GameObject exitConfirmationDialog;

  // Start is called before the first frame update
  void Start() {
    this.GoToSuburb();
  }

  /// <summary>
  /// Go to the suburb overview screen.
  /// </summary>
  public void GoToSuburb() {
    this.suburb.SetActive(true);
    foreach (GameObject house in this.houses) {
      house.SetActive(false);
    }
  }

  /// <summary>
  /// Go to the given house.
  /// </summary>
  public void GoToHouse(GameObject house) {
    this.suburb.SetActive(false);
    foreach (GameObject h in houses) {
      h.SetActive(h == house);
    }
  }

  /// <summary>
  /// Confirm that the player wants to leave work. After leaving work the
  /// player can't return.
  /// </summary>
  public void CheckExit() {
    this.exitConfirmationDialog.SetActive(true);
  }

  /// <summary>
  /// Exit the suburb and return to the map.
  /// </summary>
  public void Exit() {
    this.state.CashOut();
    SceneManager.LoadScene("Map");
  }
}
