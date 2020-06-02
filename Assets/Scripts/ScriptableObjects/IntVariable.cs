using System;
using UnityEngine;

[CreateAssetMenu(fileName="New Int Variable", menuName="Scriptable Objects/Variables/Int")]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver {
  [SerializeField]
  private int defaultValue;
  [NonSerialized]
  public int value;

  public void OnAfterDeserialize() {
    this.value = this.defaultValue;
  }

  public void OnBeforeSerialize() {}
}
