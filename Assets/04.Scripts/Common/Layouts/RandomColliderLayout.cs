using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A layout that uses a collider 2D attached to the game object, the rect
/// transform, and a minimum spacing variable to position items randomly such
/// that they're within the bounds and are not overlapping.
/// </summary>
/// <remarks>
/// Tries a bunch of random positions until one fits.
/// 
/// Make sure the child item whose position we're finding uses the center as
/// the anchor and that the parent uses the center as its pivot.
/// </remarks>
public class RandomColliderLayout : MonoBehaviour {
  /// <summary>
  /// Common overlap radius when determining if objects overlap and thus need
  /// to find a new position.
  /// </summary>
  [SerializeField]
  private float minSpacing = 32f;

  /// <summary>
  /// A list of collider 2Ds to use when determining valid bounds.
  /// </summary>
  [SerializeField]
  private Collider2D[] bounds;

  /// <summary>
  /// Try repeatedly to place an item that's within the bounds if a bounding
  /// collider 2D is attached to the game object and that's not overlapping
  /// any other objects based on the provided minimum spacing.
  /// </summary>
  /// <param name="position">The final, valid position.</param>
  /// <param name="maxAttempts">The maximum number of attempts.</param>
  /// <returns>
  /// Boolean indicating if a valid position was found within the number of
  /// attempts permitted.
  /// </returns>
  public bool FindPosition(in RectTransform parent, out Vector2 position, int maxAttempts = 100) {
    Vector2 min = parent.rect.min;
    Vector2 max = parent.rect.max;
    for (int i = 0; i < maxAttempts; ++i) {
      position = StaticRandom.Vector2(min, max);
      Debug.LogFormat("position: {0}", position);
      bool validPosition = true;

      if (this.bounds?.Length != 0) {
        // Convert this to world position because that's what the OverlapPoint
        // function expects even though the collider points are local.
        Vector2 worldPosition = parent.TransformPoint(position);

        // Test if the point is within any of our colliders. If its not, try
        // a new position.
        validPosition = false;
        foreach (Collider2D collider in this.bounds) {
          if (collider.OverlapPoint(worldPosition)) {
            validPosition = true;
            break;
          }
        }
      }

      if (validPosition && this.minSpacing >= 1) {
        // Test for overlapping objects and reject if any are encountered.
        foreach (RectTransform child in parent) {
          if (Vector2.Distance(child.anchoredPosition, position) < this.minSpacing) {
            validPosition = false;
            break;
          }
        }
      }

      if (validPosition) {
        return true;
      }
    }
    // This is only here because we have to assign position before returning.
    position = Vector2.zero;
    return false;
  }
}
