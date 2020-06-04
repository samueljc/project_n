using UnityEngine;

public sealed class WorldState : MonoBehaviour {
  public PlayerState player;
  public Inventory playerCar;
  public Journal journal;
  public Numbers numbers;

  public StoreState storeState;

  private static WorldState instance;

  public void Awake() {
    // Just using `DontDestroyOnLoad` isn't enough if we want to load into
    // other scenes, so we mark this object if we don't have an instance. When
    // we load a scene if it has its own instance we delete that and just keep
    // using this one.
    if (instance == null) {
      DontDestroyOnLoad(this.gameObject);
      instance = this;
    } else if (instance != this) {
      Destroy(this.gameObject);
    }
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
