using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper around <c>UnityEngine.Random</c>.
/// </summary>
public static class StaticRandom {
  /// <inheritdoc cref="M:UnityEngine.Random.Range(System.Int32,System.Int32)" />
  public static int Range(int min, int max) {
    return Random.Range(min, max);
  }

  /// <inheritdoc cref="M:UnityEngine.Random.Range(System.Single,System.Single)" />
  public static float Range(float min, float max) {
    return Random.Range(min, max);
  }

  /// <summary>
  /// Shuffle a list in place.
  /// </summary>
  /// <param name="list">The list to shuffle.</param>
  /// <typeparam name="T">The type of the list.</typeparam>
  public static void Shuffle<T>(this IList<T> list) {
    // Fisher-Yates shuffle
    for (int i = 0; i < list.Count - 2; ++i) {
      int j = StaticRandom.Range(i, list.Count);
      T temp = list[i];
      list[i] = list[j];
      list[j] = temp;
    }
  }
}
