using UnityEngine;

public sealed class WorldState : MonoBehaviour {
  public StoreState storeState;
  public Journal journal;

  public void Awake() {
    DontDestroyOnLoad(this.gameObject);
  }

  public void Start() {
    this.Repopulate();
  }

  public void Repopulate() {
    this.storeState.Repopulate();
  }

  public void RecordEntropy() {
    this.storeState.RecordEntropy();
    Debug.LogFormat("total entropy for today: {0}", this.journal.Today.TotalEntropy);
    this.journal.NewDay();
  }
}
