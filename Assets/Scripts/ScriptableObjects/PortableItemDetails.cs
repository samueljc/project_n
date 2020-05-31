using UnityEngine;

[CreateAssetMenu(fileName = "New Portable Item", menuName = "Scriptable Objects/Portable Item Details")]
public class PortableItemDetails : ScriptableObject {
  public string noun;
  [TextArea]
  public string description;
  public bool storeObject;
  public int price;
  public bool flower;
  public bool vhs;
  // Visuals
  public Sprite forwardSprite;
  public Sprite sideSprite;
}