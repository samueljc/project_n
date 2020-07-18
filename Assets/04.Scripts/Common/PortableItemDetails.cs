using UnityEngine;

/// <summary>
/// Unchanging details about an item.
/// </summary>
[CreateAssetMenu(fileName="New Portable Item", menuName="Scriptable Objects/Portable Item Details")]
public class PortableItemDetails : ScriptableObject {
  /// <summary>
  /// The name of the item.
  /// </summary>
  public new string name;

  /// <summary>
  /// The description of the item.
  /// </summary>
  [TextArea]
  public string description;

  /// <summary>
  /// The integer price of an item.
  /// </summary>
  public int price;

  // Visuals

  /// <summary>
  /// The sprite to use when rendering this object in the world.
  /// </summary>
  public Sprite worldSprite;

  /// <summary>
  /// The sprite to use when this object is being dragged.
  /// </summary>
  public Sprite draggingSprite;

  /// <summary>
  /// The sprite to use when this object is in a 64x64 tiled inventory grid.
  /// </summary>
  public Sprite inventorySprite;
}
