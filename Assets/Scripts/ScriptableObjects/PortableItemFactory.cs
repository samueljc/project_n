using UnityEngine;

[CreateAssetMenu(fileName="New Portable Item Factory", menuName="Scriptable Objects/Factories/Portable Item Factory")]
public class PortableItemFactory : ScriptableObject {
  public PortableItemDetails[] items;

  public PortableItem CreateRandomItem() {
    PortableItem item = ScriptableObject.CreateInstance<PortableItem>();
    item.details = items[StaticRandom.Range(0, items.Length)];
    return item;
  }
}