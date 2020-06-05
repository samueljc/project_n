using UnityEngine;

public class RecordEntropyButton : MonoBehaviour {
  [SerializeField]
  private WorldState worldState;

  public void Record() {
    this.worldState.RecordEntropy();
  }
}
