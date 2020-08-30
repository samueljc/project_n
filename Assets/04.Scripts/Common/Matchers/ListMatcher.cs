using UnityEngine;

/// <summary>
/// A matcher that returns true if the provided item matches an item in
/// the list.
/// </summary>
[CreateAssetMenu(fileName="New List Matcher", menuName="N/Matchers/List Matcher")]
public class ListMatcher : Matcher {
  /// <summary>
  /// A list of portable item details to compare to.
  /// </summary>
  [SerializeField]
  private PortableItemDetailsList details;

  /// <summary>
  /// See if the given details are contained in the list.
  /// </summary>
  /// <param name="details">The details to compare to.</param>
  /// <returns>True if the details are defined.</returns>
  public override bool Matches(in PortableItemDetails details) {
    Debug.LogFormat("List matcher: {0}", details?.name);
    return details != null && this.details.Contains(details);
  }
}
