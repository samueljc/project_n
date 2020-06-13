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
}