using UnityEngine;

/// <summary>
/// A matcher that compares an item against multiple matchers.
/// </summary>
[CreateAssetMenu(fileName="New Or-Matcher", menuName="N/Matchers/Or-Matcher")]
public class OrMatcher : Matcher {
  /// <summary>
  /// A list of matchers to use.
  /// </summary>
  [SerializeField]
  private Matcher[] matchers;

  /// <summary>
  /// Perform a logical OR against all matchers.
  /// </summary>
  /// <param name="details">The item details to compare to.</param>
  /// <returns>True if any matcher matches, false otherwise.</returns>
  public override bool Matches(in PortableItemDetails details) {
    Debug.LogFormat("Or matcher: {0}", details?.name);
    foreach (var matcher in this.matchers) {
      if (matcher.Matches(details)) {
        return true;
      }
    }
    return false;
  }
}
