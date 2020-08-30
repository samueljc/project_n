using UnityEngine;

/// <summary>
/// A matcher that compares an item against multiple matchers.
/// </summary>
[CreateAssetMenu(fileName="New Not-Matcher", menuName="N/Matchers/Not-Matcher")]
public class NotMatcher : Matcher {
  /// <summary>
  /// The matcher to negate.
  /// </summary>
  [SerializeField]
  private Matcher matcher;

  /// <summary>
  /// Negate the output of the given matcher.
  /// </summary>
  /// <param name="details">The details to compare against.</param>
  /// <returns>The inverse of the provided matcher.</returns>
  public override bool Matches(in PortableItemDetails details) {
    Debug.LogFormat("Not matcher: {0}", details?.name);
    return !this.matcher.Matches(details);
  }
}
