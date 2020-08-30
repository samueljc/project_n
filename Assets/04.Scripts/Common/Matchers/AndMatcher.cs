using UnityEngine;

/// <summary>
/// A matcher that compares an item against multiple matchers.
/// </summary>
[CreateAssetMenu(fileName="New And-Matcher", menuName="N/Matchers/And-Matcher")]
public class AndMatcher : Matcher {
  /// <summary>
  /// A list of matchers to use.
  /// </summary>
  [SerializeField]
  private Matcher[] matchers;

  /// <summary>
  /// Perform a logical AND against all matchers.
  /// </summary>
  /// <param name="details">The item details to compare to.</param>
  /// <returns>True if all matchers match, false otherwise.</returns>
  public override bool Matches(in PortableItemDetails details) {
    Debug.LogFormat("And matcher: {0}", details?.name);
    foreach (Matcher matcher in this.matchers) {
      if (!matcher.Matches(details)) {
        return false;
      }
    }
    return true;
  }
}
