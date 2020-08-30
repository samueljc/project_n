using UnityEngine;

/// <summary>
/// An abstract class for making comparisons between item details.
/// </summary>
public abstract class Matcher : ScriptableObject {
  public abstract bool Matches(in PortableItemDetails details);
}
