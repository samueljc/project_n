public static class ItemFilterListUtils {
  /// <summary>
  /// Filter the item using the given filter lists.
  /// </summary>
  /// <param name="item">The item to evaluate.</param>
  /// <param name="whitelist">A whitelist.</param>
  /// <param name="blacklist">A blacklist.</param>
  /// <returns>True if the item passed the filters, false otherwise.</returns>
  /// <remarks>
  /// A <c>null</c> item will always return false.
  /// </remarks>
  public static bool Filter(this PortableItem item, ItemWhitelist whitelist = null, ItemBlacklist blacklist = null) {
    if (item == null) {
      return false;
    }
    // Check the whitelist and blacklist when permitting items.
    if (whitelist != null) {
      if (!whitelist.Contains(item)) {
        return false;
      }
    } else if (blacklist != null) {
      if (blacklist.Contains(item)) {
        return false;
      }
    }
    return true;
  }
}
