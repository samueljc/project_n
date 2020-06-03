using UnityEngine;

[CreateAssetMenu(fileName="New Player", menuName="Scriptable Objects/State/Player")]
public class PlayerState : ScriptableObject {
  public Inventory inventory;
  public IntVariable wallet;
}