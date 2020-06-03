using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigate : MonoBehaviour {
  public void GoToStore() {
    SceneManager.LoadScene("Store");
  }
}
