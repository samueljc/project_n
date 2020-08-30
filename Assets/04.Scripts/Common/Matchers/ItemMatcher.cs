using UnityEngine;

/// <summary>
/// A matcher that returns true for any valid item details.
/// </summary>
[CreateAssetMenu(fileName="New Item Matcher", menuName="N/Matchers/Item Matcher")]
public class ItemMatcher : Matcher {
  /// <summary>
  /// The details to compare against.
  /// </summary>
  [SerializeField]
  private PortableItemDetails details;
  
  /// <summary>
  /// See if the given details match.
  /// </summary>
  /// <param name="details">The details to compare to.</param>
  /// <returns>True if the details are defined.</returns>
  public override bool Matches(in PortableItemDetails details) {
    Debug.LogFormat("Item matcher: {0}", details?.name);
    return this.details == details;
  }
}
