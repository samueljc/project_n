using UnityEngine;

[CreateAssetMenu(fileName="New Portable Item", menuName="Scriptable Objects/Portable Item Details")]
public class PortableItemDetails : ScriptableObject {
  public new string name;
  [TextArea]
  public string description;
  public bool storeObject;
  public int price;
  // Visuals
  public Sprite forwardSprite;
  public Sprite sideSprite;
  // Dimensions
  public float width;
  public float height;
  public float depth;
}