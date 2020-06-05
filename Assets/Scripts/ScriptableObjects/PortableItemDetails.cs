using UnityEngine;

[CreateAssetMenu(fileName="New Portable Item", menuName="Scriptable Objects/Portable Item Details")]
public class PortableItemDetails : ScriptableObject {
  public new string name;
  [TextArea]
  public string description;
  public bool storeObject;
  public int price;
  // Visuals
  public Sprite inventorySprite;
  public Sprite draggingSprite;
  // Dimensions
  public float shelfWidth;
  public float height;
  public float depth;
}