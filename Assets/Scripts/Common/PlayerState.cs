using UnityEngine;

/// <summary>
/// The total state of the player.
/// </summary>
[CreateAssetMenu(fileName="New Player", menuName="Scriptable Objects/State/Player")]
public class PlayerState : ScriptableObject {
  /// <summary>
  /// The player's inventory.
  /// </summary>
  /// <seealso cref="Inventory" />
  public Inventory inventory;

  /// <summary>
  /// The player's wallet.
  /// </summary>
  /// <seealso cref="IntVariable" />
  public IntVariable wallet;
}
