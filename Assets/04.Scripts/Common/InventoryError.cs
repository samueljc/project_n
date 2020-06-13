/// <summary>
/// Errors that could occur while interacting with an inventory.
/// </summary>
public enum InventoryError {
  NoError,
  OutOfSpace,
  OutOfBounds,
  InvalidItem,
  AlreadyExists,
  Occupied,
}
