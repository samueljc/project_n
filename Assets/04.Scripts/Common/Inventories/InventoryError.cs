/// <summary>
/// Errors that could occur while interacting with an inventory.
/// </summary>
public enum InventoryError {
  NoError,
  Unknown,
  OutOfSpace,
  OutOfBounds,
  InvalidItem,
  AlreadyExists,
  Occupied,
}
