using UnityEngine;

/// <summary>
/// The overall world state.
/// </summary>
public sealed class WorldState : MonoBehaviour {
  /// <summary>
  /// The state of the player.
  /// </summary>
  public PlayerState player;

  /// <summary>
  /// The state of the player's car.
  /// </summary>
  public Inventory playerCar;

  /// <summary>
  /// The journal tracking entropy.
  /// </summary>
  public EntropyJournal journal;

  /// <summary>
  /// The numbers representing entropy.
  /// </summary>
  public EntropyNumbers numbers;

  /// <summary>
  /// The state of the store.
  /// </summary>
  public StoreState storeState;

  /// <summary>
  /// The state of the blockbuster.
  /// </summary>
  public BlockbusterState blockbusterState;

  /// <summary>
  /// The state of the suburb.
  /// </summary>
  public SuburbState suburbState;

  [SerializeField]
  private PortableItemDetails shovel;

  /// <summary>
  /// The singular world state instance.
  /// </summary>
  private static WorldState instance;

  /// <inheritdoc />
  void Awake() {
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

  /// <inheritdoc />
  void Start() {
    this.Repopulate();
  }

  /// <summary>
  /// Repopulate the world state at the end of a day.
  /// </summary>
  public void Repopulate() {
    this.playerCar.Clear();
    this.playerCar.Add(this.CreateShovel());
    this.storeState.Repopulate();
    this.blockbusterState.Repopulate();
    this.suburbState.Repopulate();
  }

  private PortableItem CreateShovel() {
    PortableItem item = ScriptableObject.CreateInstance<PortableItem>();
    item.details = shovel;
    return item;
  }

  /// <summary>
  /// Record the generated entropy at the end of the day.
  /// </summary>
  public void RecordEntropy() {
    this.storeState.RecordEntropy(this.numbers, this.journal);
    Debug.LogFormat("total entropy for today: {0}", this.journal.today.entropy);
    this.journal.NewDay();
  }
}
