using UnityEngine;

public class AisleState : ScriptableObject {
  public ShelfState[] shelfStates;

  public AisleState() {
    shelfStates = new ShelfState[3]{
      new ShelfState(),
      new ShelfState(),
      new ShelfState()
    };
  }
}
