using UnityEngine.EventSystems;

/// <summary>
/// Interface to handle child item drag events.
/// </summary>
public interface IInventoryController {
  /// <summary>
  /// Handler to check when the player tries to take an item from an inventory.
  /// </summary>
  /// <param name="item">The item trying to be taken.</param>
  bool CanTakeItem(PortableItem item);
}
