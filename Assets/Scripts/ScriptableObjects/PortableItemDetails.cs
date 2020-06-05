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
  /// A boolean representing if this item is can be found in the store.
  /// </summary>
  public bool storeObject;

  /// <summary>
  /// The integer price of an item.
  /// </summary>
  public int price;

  // Visuals

  /// <summary>
  /// The sprite to use when this object is in an inventory.
  /// </summary>
  public Sprite inventorySprite;

  /// <summary>
  /// The sprite to use when this object is being dragged.
  /// </summary>
  public Sprite draggingSprite;

  // Dimensions

  /// <summary>
  /// The width of this object for layout purposes.
  /// </summary>
  public float shelfWidth;

  /// <summary>
  /// The height of this object for layout purposes.
  /// </summary>
  public float height;

  /// <summary>
  /// The depth of this object for layout purposes.
  /// </summary>
  public float depth;
}