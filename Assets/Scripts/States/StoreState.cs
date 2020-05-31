using UnityEngine;

public class StoreState : ScriptableObject {
  public AisleState[] aisleStates;
  
  public StoreState() {
    aisleStates = new AisleState[2]{
      new AisleState(),
      new AisleState()
    };
  }

  public void Repopulate() {
    foreach (AisleState aisle in this.aisleStates) {
      foreach (ShelfState shelf in aisle.shelfStates) {
        shelf.Repopulate();
      }
    }
  }
}
