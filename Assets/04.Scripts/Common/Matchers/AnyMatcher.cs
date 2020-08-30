using UnityEngine;

/// <summary>
/// A matcher that returns true for any valid item details.
/// </summary>
[CreateAssetMenu(fileName="New Any Matcher", menuName="N/Matchers/Any Matcher")]
public class AnyMatcher : Matcher {
  /// <summary>
  /// See if the given details are not null.
  /// </summary>
  /// <param name="details">The details to compare to.</param>
  /// <returns>True if the details are defined.</returns>
  public override bool Matches(in PortableItemDetails details) {
    Debug.LogFormat("Any matcher: {0}", details?.name);
    return details != null;
  }
}
