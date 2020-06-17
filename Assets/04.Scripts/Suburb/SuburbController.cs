using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuburbController : MonoBehaviour {
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
  /// Exit the suburb and return to the map. This will end work for the day and
  /// you won't be able to return to the suburb until tomorrow.
  /// </summary>
  public void Exit() {
    SceneManager.LoadScene("Map");
  }
}
