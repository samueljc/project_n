using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordEntropy : MonoBehaviour {
  [SerializeField]
  private WorldState worldState;

  public void Record() {
    this.worldState.RecordEntropy();
  }
}
